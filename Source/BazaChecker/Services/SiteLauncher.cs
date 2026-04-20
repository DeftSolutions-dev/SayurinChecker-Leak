using System.Diagnostics;

namespace BazaChecker.Services;

public static class SiteLauncher
{
	public static void OpenUrl(string url)
	{
		try
		{
			Process.Start(new ProcessStartInfo
			{
				FileName = url,
				UseShellExecute = true
			});
		}
		catch
		{
		}
	}
}
