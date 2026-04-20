using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Security;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using BazaChecker.Models;
using Microsoft.Win32;

namespace BazaChecker.Services;

public static class SteamScanner
{
	private static readonly HttpClient _httpClient = new HttpClient(new HttpClientHandler
	{
		AutomaticDecompression = DecompressionMethods.All,
		ServerCertificateCustomValidationCallback = (HttpRequestMessage sender, X509Certificate2? cert, X509Chain? chain, SslPolicyErrors sslPolicyErrors) => true
	})
	{
		Timeout = TimeSpan.FromSeconds(15L)
	};

	private static readonly Regex SteamIdPattern = new Regex("\"(\\d{17})\"[^{]*\\{([^}]+)\\}", RegexOptions.Compiled | RegexOptions.Singleline);

	private static readonly Regex AvatarMediumPattern = new Regex("<avatarMedium><!\\[CDATA\\[(.*?)\\]\\]></avatarMedium>", RegexOptions.Compiled | RegexOptions.Singleline);

	private static readonly Regex AvatarFullPattern = new Regex("<avatarFull><!\\[CDATA\\[(.*?)\\]\\]></avatarFull>", RegexOptions.Compiled | RegexOptions.Singleline);

	private static readonly Regex VacBannedPattern = new Regex("<vacBanned>(\\d+)</vacBanned>", RegexOptions.Compiled);

	private static readonly Regex TradeBanPattern = new Regex("<tradeBanState><!\\[CDATA\\[(.*?)\\]\\]></tradeBanState>", RegexOptions.Compiled | RegexOptions.Singleline);

	private static readonly Regex BadgeClassPattern = new Regex("user_badge\\s+badge_(\\w+)", RegexOptions.IgnoreCase | RegexOptions.Compiled);

	private static readonly Regex BadgeTextPattern = new Regex("<div[^>]*class=\"[^\"]*user_badge[^\"]*\"[^>]*>(.*?)</div>", RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.Singleline);

	private static readonly Regex HtmlTagPattern = new Regex("<[^>]+>", RegexOptions.Compiled);

	private static readonly Regex WhitespacePattern = new Regex("\\s+", RegexOptions.Compiled);

	private static readonly Regex BansContentPattern = new Regex("class=\"bans_comms_content\"[^>]*>\\s*<ul[^>]*>(.*?)</ul>", RegexOptions.Compiled | RegexOptions.Singleline);

	private static readonly Regex ListItemPattern = new Regex("<li[^>]*>(.*?)</li>", RegexOptions.Compiled | RegexOptions.Singleline);

	private static readonly Regex SpanPattern = new Regex("<span[^>]*>(.*?)</span>", RegexOptions.Compiled | RegexOptions.Singleline);

	private static readonly Regex SvgPathPattern = new Regex("d=\"([^\"]+)\"", RegexOptions.IgnoreCase | RegexOptions.Compiled);

	public static List<SteamAccount> GetAccounts()
	{
		List<SteamAccount> list = new List<SteamAccount>();
		try
		{
			string text = null;
			using (RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("Software\\Valve\\Steam"))
			{
				text = registryKey?.GetValue("SteamPath")?.ToString();
			}
			if (string.IsNullOrEmpty(text))
			{
				string[] array = new string[2] { "C:\\Program Files (x86)\\Steam", "C:\\Program Files\\Steam" };
				foreach (string text2 in array)
				{
					if (Directory.Exists(text2))
					{
						text = text2;
						break;
					}
				}
			}
			if (string.IsNullOrEmpty(text))
			{
				return list;
			}
			string path = Path.Combine(text, "config", "loginusers.vdf");
			if (!File.Exists(path))
			{
				return list;
			}
			string input = File.ReadAllText(path);
			foreach (Match item in SteamIdPattern.Matches(input))
			{
				string value = item.Groups[1].Value;
				string value2 = item.Groups[2].Value;
				string text3 = ExtractValue(value2, "AccountName");
				string text4 = ExtractValue(value2, "PersonaName");
				string text5 = ExtractValue(value2, "MostRecent");
				if (!string.IsNullOrEmpty(text3))
				{
					list.Add(new SteamAccount
					{
						SteamId64 = value,
						AccountName = text3,
						PersonaName = ((!string.IsNullOrEmpty(text4)) ? text4 : text3),
						IsActive = (text5 == "1"),
						IsVacBanned = false
					});
				}
			}
		}
		catch
		{
		}
		return list;
	}

	public static async Task EnrichAccountsWithSteamDataAsync(List<SteamAccount> accounts)
	{
		if (accounts.Count != 0)
		{
			try
			{
				ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls13;
			}
			catch
			{
			}
			await Task.WhenAll(accounts.Select(async account =>
			{
				await Task.WhenAll(FetchSteamProfileDataAsync(account), FetchBazaDataAsync(account));
			}));
		}
	}

	private static async Task FetchBazaDataAsync(SteamAccount account)
	{
		try
		{
			account.IsBazaLoading = true;
			string requestUri = "https://baza-cs2.ru/profiles/" + account.SteamId64 + "/block/0/";
			if (!_httpClient.DefaultRequestHeaders.Contains("User-Agent"))
			{
				_httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/120.0.0.0 Safari/537.36");
			}
			HttpResponseMessage httpResponseMessage = await _httpClient.GetAsync(requestUri);
			if (httpResponseMessage.StatusCode == HttpStatusCode.NotFound)
			{
				account.BazaBanInfo = "Не зарегистрирован (Not Registered)";
				account.BazaAdminStatus = "N/A";
				return;
			}
			httpResponseMessage.EnsureSuccessStatusCode();
			string text = await httpResponseMessage.Content.ReadAsStringAsync();
			string bazaAdminStatus = "Игрок (PLAYER)";
			Match match = Regex.Match(text, "user_badge\\s+badge_(\\w+)", RegexOptions.IgnoreCase);
			string text2 = (match.Success ? match.Groups[1].Value.ToLower() : "");
			string text3 = "";
			Match match2 = Regex.Match(text, "<div[^>]*class=\"[^\"]*user_badge[^\"]*\"[^>]*>(.*?)</div>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
			if (match2.Success)
			{
				text3 = smethod_0(match2.Groups[1].Value).ToLower();
			}
			if (!(text2 == "root") && !text3.Contains("создатель") && !text3.Contains("владелец"))
			{
				if (!(text2 == "co_owner") && !text3.Contains("совладелец"))
				{
					if (text3.Contains("зам") && (text3.Contains("создател") || text3.Contains("владельц")))
					{
						bazaAdminStatus = "Зам. Создателя (DEP. CREATOR)";
					}
					else if (!(text2 == "main_admin") && !text3.Contains("главный") && !text3.Contains("га"))
					{
						if (text3.Contains("зам") && (text3.Contains("главного") || text3.Contains("зга")))
						{
							bazaAdminStatus = "Зам. Гл. Админа (DEP. HEAD ADMIN)";
						}
						else if (!(text2 == "curator") && !text3.Contains("куратор"))
						{
							if (!(text2 == "spec_admin") && !text3.Contains("спец"))
							{
								if (!(text2 == "admin") && !(text3 == "администратор") && !(text3 == "admin"))
								{
									if (!(text2 == "moder") && !text3.Contains("модератор"))
									{
										if (!(text2 == "vip") && !text3.Contains("vip"))
										{
											if (text2 == "player" || text3.Contains("игрок"))
											{
												bazaAdminStatus = "Игрок (PLAYER)";
											}
										}
										else
										{
											bazaAdminStatus = "VIP PLAYER";
										}
									}
									else
									{
										bazaAdminStatus = "Модератор (MODERATOR)";
									}
								}
								else
								{
									bazaAdminStatus = "Администратор (ADMIN)";
								}
							}
							else
							{
								bazaAdminStatus = "Спец. Админ (SPECIAL ADMIN)";
							}
						}
						else
						{
							bazaAdminStatus = "Куратор (CURATOR)";
						}
					}
					else
					{
						bazaAdminStatus = "Главный Админ (HEAD ADMIN)";
					}
				}
				else
				{
					bazaAdminStatus = "Совладелец (CO-OWNER)";
				}
			}
			else
			{
				bazaAdminStatus = "Создатель / Владелец (CREATOR)";
			}
			account.BazaAdminStatus = bazaAdminStatus;
			account.BazaBans = new List<BazaBanEntry>();
			account.BazaMutes = new List<BazaBanEntry>();
			List<BazaBanEntry> list = smethod_1(text, "Блокировки", isMuteSection: false, "Мут");
			if (list.Count == 0)
			{
				list = smethod_1(text, "История банов", isMuteSection: false, "Мут");
			}
			if (list.Count == 0)
			{
				list = smethod_1(text, "Последние баны", isMuteSection: false, "Мут");
			}
			List<BazaBanEntry> list2 = smethod_1(text, "Муты", isMuteSection: true);
			if (list2.Count == 0)
			{
				list2 = smethod_1(text, "Последние муты", isMuteSection: true);
			}
			account.BazaBans = list;
			account.BazaMutes = list2;
			string text4 = "Чист (Clean)";
			bool flag;
			if (flag = account.BazaBans.Count > 0 || account.BazaMutes.Count > 0)
			{
				List<string> list3 = new List<string>();
				if (account.BazaBans.Count > 0)
				{
					list3.Add($"БАНЫ ({account.BazaBans.Count})");
				}
				if (account.BazaMutes.Count > 0)
				{
					list3.Add($"МУТЫ ({account.BazaMutes.Count})");
				}
				text4 = string.Join(" | ", list3);
			}
			if (account.BazaBans.Any((BazaBanEntry bazaBanEntry_0) => smethod_2(bazaBanEntry_0.Status)) || account.BazaMutes.Any((BazaBanEntry bazaBanEntry_0) => smethod_2(bazaBanEntry_0.Status)))
			{
				text4 = "АКТИВНОЕ НАКАЗАНИЕ";
				if (account.BazaBans.Any((BazaBanEntry bazaBanEntry_0) => smethod_2(bazaBanEntry_0.Status)))
				{
					text4 = "ЗАБАНЕН (ACTIVE BAN)";
				}
				else if (account.BazaMutes.Any((BazaBanEntry bazaBanEntry_0) => smethod_2(bazaBanEntry_0.Status)))
				{
					text4 = "МУТ (ACTIVE MUTE)";
				}
			}
			else if (flag && !text4.Contains("ИСТЁК/РАЗБАН"))
			{
				text4 += " (ИСТЁК/РАЗБАН)";
			}
			account.BazaBanInfo = text4;
		}
		catch
		{
			account.BazaBanInfo = "Ошибка парсинга";
		}
		finally
		{
			account.IsBazaLoading = false;
		}
	}

	private static async Task FetchSteamProfileDataAsync(SteamAccount account)
	{
		try
		{
			string requestUri = "https://steamcommunity.com/profiles/" + account.SteamId64 + "/?xml=1";
			string xml = await _httpClient.GetStringAsync(requestUri);
			string avatarUrl = null;
			Match match = AvatarMediumPattern.Match(xml);
			if (match.Success && !string.IsNullOrEmpty(match.Groups[1].Value))
			{
				avatarUrl = match.Groups[1].Value;
			}
			else
			{
				Match match2 = AvatarFullPattern.Match(xml);
				if (match2.Success)
				{
					avatarUrl = match2.Groups[1].Value;
				}
			}
			if (!string.IsNullOrEmpty(avatarUrl))
			{
				account.AvatarUrl = (await AvatarCache.GetOrFetchAvatarAsync(account.SteamId64, avatarUrl)) ?? avatarUrl;
			}
			Match match3 = VacBannedPattern.Match(xml);
			if (match3.Success)
			{
				int num = int.Parse(match3.Groups[1].Value);
				account.IsVacBanned = num > 0;
				if (account.IsVacBanned)
				{
					account.NumberOfVACBans = 1;
				}
			}
			Match match4 = TradeBanPattern.Match(xml);
			if (match4.Success)
			{
				string text = match4.Groups[1].Value.ToLower();
				account.EconomyBan = text != "none" && !string.IsNullOrEmpty(text);
			}
		}
		catch
		{
		}
	}

	private static string ExtractValue(string block, string key)
	{
		Match match = new Regex("\"" + key + "\"\\s+\"([^\"]+)\"", RegexOptions.IgnoreCase).Match(block);
		if (!match.Success)
		{
			return "";
		}
		return match.Groups[1].Value;
	}

	[CompilerGenerated]
	internal static string smethod_0(string input)
	{
		if (string.IsNullOrEmpty(input))
		{
			return "";
		}
		return Regex.Replace(WebUtility.HtmlDecode(Regex.Replace(input, "<[^>]+>", " ")), "\\s+", " ").Trim();
	}

	[CompilerGenerated]
	internal static List<BazaBanEntry> smethod_1(string pageHtml, string headerText, bool isMuteSection, string? stopAt = null)
	{
		List<BazaBanEntry> list = new List<BazaBanEntry>();
		int num = pageHtml.IndexOf(headerText, StringComparison.OrdinalIgnoreCase);
		if (num == -1)
		{
			return list;
		}
		string text = pageHtml.Substring(num);
		if (!string.IsNullOrEmpty(stopAt))
		{
			int num2 = text.IndexOf(stopAt, StringComparison.OrdinalIgnoreCase);
			if (num2 != -1)
			{
				text = text.Substring(0, num2);
			}
		}
		if (!text.Contains("Нет банов") && !text.Contains("Нет мутов") && !text.Contains("class=\"no_data\""))
		{
			Match match = Regex.Match(text, "class=\"bans_comms_content\"[^>]*>\\s*<ul[^>]*>(.*?)</ul>", RegexOptions.Singleline);
			if (!match.Success)
			{
				return list;
			}
			{
				foreach (Match item in Regex.Matches(match.Groups[1].Value, "<li[^>]*>(.*?)</li>", RegexOptions.Singleline))
				{
					MatchCollection matchCollection = Regex.Matches(item.Groups[1].Value, "<span[^>]*>(.*?)</span>", RegexOptions.Singleline);
					if (matchCollection.Count < 5)
					{
						continue;
					}
					string text2 = smethod_0(matchCollection[0].Groups[1].Value);
					if (!char.IsDigit(text2.FirstOrDefault()))
					{
						continue;
					}
					string reason = smethod_0(matchCollection[1].Groups[1].Value);
					string admin = smethod_0(matchCollection[2].Groups[1].Value);
					string type = "Ban";
					string iconData = "";
					string iconColor = "#888888";
					string text3 = "—";
					string text4 = "";
					bool isImage = false;
					if (isMuteSection)
					{
						string value = matchCollection[3].Groups[1].Value;
						text4 = smethod_0(matchCollection[4].Groups[1].Value);
						text3 = "—";
						Match match2 = Regex.Match(value, "d=\"([^\"]+)\"", RegexOptions.IgnoreCase);
						if (match2.Success)
						{
							iconData = match2.Groups[1].Value;
						}
						if (!value.Contains("M38.8") && !value.Contains("microphone"))
						{
							if (!value.Contains("M6.009") && !value.Contains("M64.0") && !value.Contains("comment") && !value.Contains("gag"))
							{
								if (value.Contains("M367.2") || value.Contains("slash") || value.Contains("block"))
								{
									type = "Block";
									iconColor = "#A855F7";
									iconData = "M367.2 412.5L99.5 144.8C77.1 176.1 64 214.5 64 256c0 106 86 192 192 192c41.5 0 79.9-13.1 111.2-35.5zm45.3-45.3C434.9 335.9 448 297.5 448 256c0-106-86-192-192-192c-41.5 0-79.9 13.1-111.2 35.5L412.5 367.2zM512 256c0 141.4-114.6 256-256 256S0 397.4 0 256S114.6 0 256 0S512 114.6 512 256z";
									isImage = false;
								}
							}
							else
							{
								type = "Chat";
								iconColor = "#A855F7";
								iconData = "M64.03 239.1c0 49.59 21.38 94.1 56.97 130.7c-12.5 50.39-54.31 95.3-54.81 95.8c-2.187 2.297-2.781 5.703-1.5 8.703c1.312 3 4.125 4.797 7.312 4.797c66.31 0 116-31.8 140.6-51.41c32.72 12.31 69.02 19.41 107.4 19.41c37.39 0 72.78-6.663 104.8-18.36L82.93 161.7C70.81 185.9 64.03 212.3 64.03 239.1zM630.8 469.1l-118.1-92.59C551.1 340 576 292.4 576 240c0-114.9-114.6-207.1-255.1-207.1c-67.74 0-129.1 21.55-174.9 56.47L38.81 5.117C28.21-3.154 13.16-1.096 5.115 9.19C-3.072 19.63-1.249 34.72 9.188 42.89l591.1 463.1c10.5 8.203 25.57 6.333 33.7-4.073C643.1 492.4 641.2 477.3 630.8 469.1z";
								isImage = false;
							}
						}
						else
						{
							type = "Voice";
							iconColor = "#A855F7";
							iconData = "M38.8 5.1C28.4-3.1 13.3-1.2 5.1 9.2S-1.2 34.7 9.2 42.9l592 464c10.4 8.2 25.5 6.3 33.7-4.1s6.3-25.5-4.1-33.7L472.1 344.7c15.2-26 23.9-56.3 23.9-88.7V216c0-13.3-10.7-24-24-24s-24 10.7-24 24v40c0 21.2-5.1 41.1-14.2 58.7L416 300.8V96c0-53-43-96-96-96s-96 43-96 96v54.3L38.8 5.1zM344 430.4c20.4-2.8 39.7-9.1 57.3-18.2l-43.1-33.9C346.1 382 333.3 384 320 384c-70.7 0-128-57.3-128-128v-8.7L144.7 210c-.5 1.9-.7 3.9-.7 6v40c0 89.1 66.2 162.7 152 174.4V464H248c-13.3 0-24 10.7-24 24s10.7 24 24 24h72 72c13.3 0 24-10.7 24-24s-10.7-24-24-24H344V430.4z";
							isImage = false;
						}
					}
					else
					{
						text3 = smethod_0(matchCollection[3].Groups[1].Value);
						text4 = smethod_0(matchCollection[4].Groups[1].Value);
						type = "Ban";
						iconColor = "#EF4444";
						iconData = "M256 8C119.034 8 8 119.033 8 256s111.034 248 248 248 248-111.034 248-248S392.967 8 256 8zm130.108 117.892c65.448 65.448 70 165.481 20.677 235.637L150.47 105.216c70.204-49.356 170.226-44.735 235.638 20.676zM125.892 386.108c-65.448-65.448-70-165.481-20.677-235.637L361.53 406.784c-70.203 49.356-170.226 44.736-235.638-20.676z";
						isImage = false;
					}
					list.Add(new BazaBanEntry
					{
						Type = type,
						Date = text2,
						Reason = reason,
						Admin = admin,
						Duration = text3,
						Status = text4,
						IconData = iconData,
						IconColor = iconColor,
						IsImage = isImage
					});
				}
				return list;
			}
		}
		return list;
	}

	[CompilerGenerated]
	internal static bool smethod_2(string status)
	{
		string text = status.ToLower();
		if (!text.Contains("ист") && !text.Contains("exp") && !text.Contains("разб") && !text.Contains("unb"))
		{
			return !text.Contains("снят");
		}
		return false;
	}
}
