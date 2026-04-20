using System;
using System.Diagnostics;
using System.IO;
using System.Windows;

namespace BazaChecker.Services;

public static class FolderLauncher
{
	public static void OpenAppData()
	{
		OpenFolder(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData));
	}

	public static void OpenPrefetch()
	{
		OpenFolder(Path.Combine(Environment.GetEnvironmentVariable("windir") ?? "C:\\Windows", "Prefetch"));
	}

	public static void OpenRecent()
	{
		OpenFolder(Environment.GetFolderPath(Environment.SpecialFolder.Recent));
	}

	public static void OpenTemp()
	{
		OpenFolder(Path.GetTempPath());
	}

	public static void OpenProgramFiles()
	{
		OpenFolder(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles));
	}

	public static void OpenProgramData()
	{
		OpenFolder(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData));
	}

	public static void OpenDocuments()
	{
		OpenFolder(Environment.GetFolderPath(Environment.SpecialFolder.Personal));
	}

	public static void OpenDownloads()
	{
		OpenFolder(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads"));
	}

	public static void OpenUsers()
	{
		OpenFolder(Path.Combine(Environment.GetEnvironmentVariable("SystemDrive") ?? "C:", "Users"));
	}

	public static void OpenCrashDumps()
	{
		OpenFolder(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "CrashDumps"));
	}

	public static void OpenReportArchive()
	{
		OpenFolder(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "Microsoft\\Windows\\WER\\ReportArchive"));
	}

	public static void OpenFolder(string path)
	{
		try
		{
			if (Directory.Exists(path))
			{
				Process.Start("explorer.exe", path);
			}
			else
			{
				MessageBox.Show("Folder not found:\n" + path, "Not Found", MessageBoxButton.OK, MessageBoxImage.Exclamation);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show("Error:\n" + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Hand);
		}
	}
}
