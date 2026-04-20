using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace BazaChecker.Services;

public static class AvatarCache
{
	private static readonly string CacheDir;

	private static readonly HttpClient _httpClient;

	private const int MaxCacheAgeDays = 7;

	static AvatarCache()
	{
		CacheDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "SayurinChecker", "AvatarCache");
		_httpClient = new HttpClient
		{
			Timeout = TimeSpan.FromSeconds(10L)
		};
		try
		{
			Directory.CreateDirectory(CacheDir);
		}
		catch
		{
		}
	}

	public static async Task<string?> GetOrFetchAvatarAsync(string steamId, string? url)
	{
		if (!string.IsNullOrEmpty(url))
		{
			try
			{
				string cacheFile = Path.Combine(CacheDir, steamId + ".jpg");
				if (File.Exists(cacheFile))
				{
					FileInfo fileInfo = new FileInfo(cacheFile);
					if ((DateTime.Now - fileInfo.LastWriteTime).TotalDays < 7.0)
					{
						return cacheFile;
					}
				}
				await File.WriteAllBytesAsync(cacheFile, await _httpClient.GetByteArrayAsync(url));
				return cacheFile;
			}
			catch
			{
				return null;
			}
		}
		return null;
	}

	public static void CleanOldCache()
	{
		try
		{
			if (!Directory.Exists(CacheDir))
			{
				return;
			}
			string[] files = Directory.GetFiles(CacheDir);
			foreach (string text in files)
			{
				FileInfo fileInfo = new FileInfo(text);
				if ((DateTime.Now - fileInfo.LastWriteTime).TotalDays > 7.0)
				{
					File.Delete(text);
				}
			}
		}
		catch
		{
		}
	}
}
