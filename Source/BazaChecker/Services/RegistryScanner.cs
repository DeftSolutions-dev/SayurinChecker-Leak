using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BazaChecker.Models;
using Microsoft.Win32;

namespace BazaChecker.Services;

public static class RegistryScanner
{
	private static readonly string[] UninstallPaths = new string[2] { "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Uninstall", "SOFTWARE\\WOW6432Node\\Microsoft\\Windows\\CurrentVersion\\Uninstall" };

	public static List<ProgramInfo> GetPrograms()
	{
		List<ProgramInfo> list = new List<ProgramInfo>();
		ScanUninstallKeys(Registry.CurrentUser, list);
		ScanUninstallKeys(Registry.LocalMachine, list);
		ScanMuiCache(list);
		ScanAppCompatFlags(list);
		ScanUserAssist(list);
		return (from programInfo_0 in list
			group programInfo_0 by programInfo_0.DisplayName.ToLowerInvariant() into igrouping_0
			select igrouping_0.First() into programInfo_0
			orderby programInfo_0.DisplayName
			select programInfo_0).ToList();
	}

	private static void ScanUninstallKeys(RegistryKey rootKey, List<ProgramInfo> programs)
	{
		string[] uninstallPaths = UninstallPaths;
		foreach (string name in uninstallPaths)
		{
			using RegistryKey registryKey = rootKey.OpenSubKey(name);
			if (registryKey == null)
			{
				continue;
			}
			string[] subKeyNames = registryKey.GetSubKeyNames();
			foreach (string name2 in subKeyNames)
			{
				using RegistryKey registryKey2 = registryKey.OpenSubKey(name2);
				if (registryKey2 == null)
				{
					continue;
				}
				string text = registryKey2.GetValue("DisplayName")?.ToString();
				if (string.IsNullOrWhiteSpace(text))
				{
					continue;
				}
				string? installLoc = registryKey2.GetValue("InstallLocation")?.ToString();
				string uninst = registryKey2.GetValue("UninstallString")?.ToString();
				string icon = registryKey2.GetValue("DisplayIcon")?.ToString();
				object? value = registryKey2.GetValue("InstallDate");
				object obj;
				if (value == null)
				{
					obj = null;
				}
				else
				{
					obj = value.ToString();
					if (obj != null)
					{
						goto IL_00ea;
					}
				}
				obj = "";
				goto IL_00ea;
				IL_01b2:
				object obj2;
				object obj3;
				((ProgramInfo)obj2).Publisher = (string)obj3;
				string text2;
				((ProgramInfo)obj2).InstallDate = text2;
				DateTime? timestamp;
				((ProgramInfo)obj2).Timestamp = timestamp;
				ProgramStatus item;
				((ProgramInfo)obj2).Status = item;
				programs.Add((ProgramInfo)obj2);
				goto end_IL_004c;
				IL_00ea:
				text2 = (string)obj;
				(string path, ProgramStatus status) tuple = VerifyProgramStatus(installLoc, uninst, icon);
				string item2 = tuple.path;
				item = tuple.status;
				timestamp = null;
				if (text2.Length == 8 && int.TryParse(text2.Substring(0, 4), out var result) && int.TryParse(text2.Substring(4, 2), out var result2) && int.TryParse(text2.Substring(6, 2), out var result3))
				{
					try
					{
						timestamp = new DateTime(result, result2, result3);
						text2 = timestamp.Value.ToString("dd.MM.yyyy");
					}
					catch
					{
					}
				}
				obj2 = new ProgramInfo
				{
					DisplayName = text,
					InstallPath = item2
				};
				object? value2 = registryKey2.GetValue("Publisher");
				if (value2 == null)
				{
					obj3 = null;
				}
				else
				{
					obj3 = value2.ToString();
					if (obj3 != null)
					{
						goto IL_01b2;
					}
				}
				obj3 = "";
				goto IL_01b2;
				end_IL_004c:;
			}
		}
	}

	private static void ScanMuiCache(List<ProgramInfo> programs)
	{
		try
		{
			using RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("Software\\Classes\\Local Settings\\Software\\Microsoft\\Windows\\Shell\\MuiCache");
			if (registryKey == null)
			{
				return;
			}
			string[] valueNames = registryKey.GetValueNames();
			foreach (string text in valueNames)
			{
				if (text == "LangID" || string.IsNullOrEmpty(text))
				{
					continue;
				}
				string text2 = text.Split('|')[0];
				if (!text2.Contains('\\'))
				{
					continue;
				}
				object? value = registryKey.GetValue(text);
				object obj;
				if (value == null)
				{
					obj = null;
				}
				else
				{
					obj = value.ToString();
					if (obj != null)
					{
						goto IL_007b;
					}
				}
				obj = Path.GetFileNameWithoutExtension(text2);
				goto IL_007b;
				IL_007b:
				string displayName = (string)obj;
				var (installPath, programStatus) = VerifyProgramStatus(text2, null, null);
				if (programStatus == ProgramStatus.Deleted)
				{
					programStatus = ProgramStatus.Trace;
				}
				ProgramInfo programInfo = new ProgramInfo
				{
					DisplayName = displayName,
					InstallPath = installPath,
					Status = programStatus,
					IsHistory = true
				};
				EnrichWithFileTime(programInfo);
				programs.Add(programInfo);
			}
		}
		catch
		{
		}
	}

	private static void ScanAppCompatFlags(List<ProgramInfo> programs)
	{
		try
		{
			using RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows NT\\CurrentVersion\\AppCompatFlags\\Compatibility Assistant\\Store");
			if (registryKey == null)
			{
				return;
			}
			string[] valueNames = registryKey.GetValueNames();
			foreach (string text in valueNames)
			{
				if (!string.IsNullOrEmpty(text) && text.Contains('\\'))
				{
					string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(text);
					var (installPath, programStatus) = VerifyProgramStatus(text, null, null);
					if (programStatus == ProgramStatus.Deleted)
					{
						programStatus = ProgramStatus.Trace;
					}
					ProgramInfo programInfo = new ProgramInfo
					{
						DisplayName = "[Trace] " + fileNameWithoutExtension,
						InstallPath = installPath,
						Status = programStatus,
						IsHistory = true
					};
					EnrichWithFileTime(programInfo);
					programs.Add(programInfo);
				}
			}
		}
		catch
		{
		}
	}

	private static void ScanUserAssist(List<ProgramInfo> programs)
	{
		try
		{
			using RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Explorer\\UserAssist");
			if (registryKey == null)
			{
				return;
			}
			string[] subKeyNames = registryKey.GetSubKeyNames();
			foreach (string text in subKeyNames)
			{
				using RegistryKey registryKey2 = registryKey.OpenSubKey(text + "\\Count");
				if (registryKey2 == null)
				{
					continue;
				}
				string[] valueNames = registryKey2.GetValueNames();
				foreach (string text2 in valueNames)
				{
					string text3 = Rot13(text2);
					if (!text3.Contains('\\') || !text3.EndsWith(".exe", StringComparison.OrdinalIgnoreCase))
					{
						continue;
					}
					string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(text3);
					var (installPath, programStatus) = VerifyProgramStatus(text3, null, null);
					if (programStatus == ProgramStatus.Deleted)
					{
						programStatus = ProgramStatus.Trace;
					}
					DateTime? timestamp = null;
					string installDate = "";
					if (registryKey2.GetValue(text2) is byte[] array && array.Length >= 72)
					{
						long num = BitConverter.ToInt64(array, 60);
						if (num > 0L)
						{
							try
							{
								timestamp = DateTime.FromFileTimeUtc(num).ToLocalTime();
								installDate = timestamp.Value.ToString("dd.MM.yyyy HH:mm");
							}
							catch
							{
							}
						}
					}
					programs.Add(new ProgramInfo
					{
						DisplayName = "[Run History] " + fileNameWithoutExtension,
						InstallPath = installPath,
						InstallDate = installDate,
						Timestamp = timestamp,
						Status = programStatus,
						IsHistory = true
					});
				}
			}
		}
		catch
		{
		}
	}

	private static void EnrichWithFileTime(ProgramInfo info)
	{
		if (info.Timestamp.HasValue || string.IsNullOrEmpty(info.InstallPath) || info.InstallPath == "Unknown")
		{
			return;
		}
		try
		{
			if (File.Exists(info.InstallPath))
			{
				FileInfo fileInfo = new FileInfo(info.InstallPath);
				info.Timestamp = fileInfo.LastWriteTime;
				info.InstallDate = info.Timestamp.Value.ToString("dd.MM.yyyy HH:mm");
			}
			else if (Directory.Exists(info.InstallPath))
			{
				DirectoryInfo directoryInfo = new DirectoryInfo(info.InstallPath);
				info.Timestamp = directoryInfo.LastWriteTime;
				info.InstallDate = info.Timestamp.Value.ToString("dd.MM.yyyy");
			}
		}
		catch
		{
		}
	}

	private static (string path, ProgramStatus status) VerifyProgramStatus(string? installLoc, string? uninst, string? icon)
	{
		string text = null;
		if (!string.IsNullOrWhiteSpace(installLoc))
		{
			text = installLoc.Trim('"', ' ');
			if (Directory.Exists(text))
			{
				return (path: text, status: ProgramStatus.Installed);
			}
		}
		if (!string.IsNullOrWhiteSpace(icon))
		{
			string text2 = icon.Split(',')[0].Trim('"', ' ');
			if (File.Exists(text2))
			{
				return (path: text2, status: ProgramStatus.Installed);
			}
			if (text == null)
			{
				text = text2;
			}
		}
		if (!string.IsNullOrWhiteSpace(uninst))
		{
			int num = uninst.IndexOf(".exe", StringComparison.OrdinalIgnoreCase);
			if (num != -1)
			{
				string text3 = uninst.Substring(0, num + 4).Trim('"', ' ');
				if (File.Exists(text3))
				{
					return (path: text3, status: ProgramStatus.Installed);
				}
				if (text == null)
				{
					text = Path.GetDirectoryName(text3) ?? text3;
				}
			}
		}
		if (!string.IsNullOrEmpty(text))
		{
			return (path: text, status: ProgramStatus.Deleted);
		}
		return (path: "Unknown", status: ProgramStatus.Deleted);
	}

	private static string Rot13(string input)
	{
		return new string(input.Select(delegate(char char_0)
		{
			if (char_0 >= 'a' && char_0 <= 'z')
			{
				return (char)((char_0 - 97 + 13) % 26 + 97);
			}
			return (char_0 >= 'A' && char_0 <= 'Z') ? ((char)((char_0 - 65 + 13) % 26 + 65)) : char_0;
		}).ToArray());
	}

	public static (int installed, int deleted, int trace) GetCounts(List<ProgramInfo> programs)
	{
		return (installed: programs.Count((ProgramInfo programInfo_0) => programInfo_0.Status == ProgramStatus.Installed), deleted: programs.Count((ProgramInfo programInfo_0) => programInfo_0.Status == ProgramStatus.Deleted || programInfo_0.Status == ProgramStatus.Trace), trace: programs.Count((ProgramInfo programInfo_0) => programInfo_0.Status == ProgramStatus.Trace));
	}
}
