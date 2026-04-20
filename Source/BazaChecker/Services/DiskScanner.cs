using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BazaChecker.Services;

public static class DiskScanner
{
	public static List<DiskInfo> GetAvailableDisks()
	{
		List<DiskInfo> list = new List<DiskInfo>();
		try
		{
			DriveInfo[] drives = DriveInfo.GetDrives();
			foreach (DriveInfo driveInfo in drives)
			{
				if (driveInfo.IsReady && driveInfo.DriveType == DriveType.Fixed)
				{
					list.Add(new DiskInfo
					{
						DriveLetter = driveInfo.Name.TrimEnd('\\'),
						Label = driveInfo.VolumeLabel,
						TotalSize = driveInfo.TotalSize,
						FreeSpace = driveInfo.AvailableFreeSpace,
						IsSelected = true
					});
				}
			}
		}
		catch
		{
		}
		return list;
	}

	public static async Task<DiskScanResult> ScanDisksAsync(List<DiskInfo> selectedDisks, bool useIconScan, bool useSignatureScan, IProgress<string>? progress = null, CancellationToken cancellationToken = default(CancellationToken))
	{
		DiskScanResult result = new DiskScanResult();
		Stopwatch stopwatch = Stopwatch.StartNew();
		StringBuilder logBuilder = new StringBuilder();
		StringBuilder stringBuilder = logBuilder;
		StringBuilder stringBuilder2 = stringBuilder;
		StringBuilder.AppendInterpolatedStringHandler handler = new StringBuilder.AppendInterpolatedStringHandler(17, 1, stringBuilder);
		handler.AppendLiteral("DEBUG LOG START: ");
		handler.AppendFormatted(DateTime.Now);
		stringBuilder2.AppendLine(ref handler);
		logBuilder.AppendLine("Scan Depth: 25");
		logBuilder.AppendLine("SystemPathsToSkip contents: " + string.Join(", ", CheatDatabase.SystemPathsToSkip));
		progress?.Report("Scanning running processes...");
		await Task.Run(delegate
		{
			ScanProcesses(result);
		}, cancellationToken);
		foreach (DiskInfo item in selectedDisks.Where((DiskInfo diskInfo_0) => diskInfo_0.IsSelected))
		{
			if (!cancellationToken.IsCancellationRequested)
			{
				progress?.Report("Scanning " + item.DriveLetter + "...");
				List<string> scanPaths = GetScanPaths(item.DriveLetter);
				foreach (string path in scanPaths)
				{
					if (cancellationToken.IsCancellationRequested)
					{
						break;
					}
					if (!Directory.Exists(path))
					{
						stringBuilder = logBuilder;
						StringBuilder stringBuilder3 = stringBuilder;
						handler = new StringBuilder.AppendInterpolatedStringHandler(21, 1, stringBuilder);
						handler.AppendLiteral("Root path not found: ");
						handler.AppendFormatted(path);
						stringBuilder3.AppendLine(ref handler);
						continue;
					}
					stringBuilder = logBuilder;
					StringBuilder stringBuilder4 = stringBuilder;
					handler = new StringBuilder.AppendInterpolatedStringHandler(23, 1, stringBuilder);
					handler.AppendLiteral("Starting scan of root: ");
					handler.AppendFormatted(path);
					stringBuilder4.AppendLine(ref handler);
					await Task.Run(delegate
					{
						ScanDirectory(path, result, progress, cancellationToken, 0, 25, useIconScan, useSignatureScan, logBuilder);
					}, cancellationToken);
				}
				continue;
			}
			result.WasCancelled = true;
			break;
		}
		stopwatch.Stop();
		result.ScanDuration = stopwatch.Elapsed;
		stringBuilder = logBuilder;
		StringBuilder stringBuilder5 = stringBuilder;
		handler = new StringBuilder.AppendInterpolatedStringHandler(33, 2, stringBuilder);
		handler.AppendLiteral("Scan finished in ");
		handler.AppendFormatted(stopwatch.Elapsed.TotalSeconds);
		handler.AppendLiteral("s. Total files: ");
		handler.AppendFormatted(result.TotalFilesScanned);
		stringBuilder5.AppendLine(ref handler);
		try
		{
			File.WriteAllText("scan_log.txt", logBuilder.ToString());
		}
		catch
		{
		}
		return result;
	}

	private static List<string> GetScanPaths(string driveLetter)
	{
		List<string> list = new List<string>();
		string item = driveLetter.TrimEnd('\\', ':') + ":\\";
		list.Add(item);
		return list;
	}

	private static void ScanProcesses(DiskScanResult result)
	{
		try
		{
			Process[] processes = Process.GetProcesses();
			foreach (Process process in processes)
			{
				try
				{
					string item = process.ProcessName.ToLowerInvariant();
					if (CheatDatabase.Signatures.Contains(item))
					{
						result.Threats.Add(new ThreatInfo
						{
							Name = process.ProcessName,
							Path = $"Process: {process.ProcessName} (PID: {process.Id})",
							Type = "Suspicious Process"
						});
					}
				}
				catch
				{
				}
			}
		}
		catch
		{
		}
	}

	private static void ScanDirectory(string path, DiskScanResult result, IProgress<string>? progress, CancellationToken cancellationToken_0, int depth, int maxDepth, bool useIconScan, bool useSignatureScan, StringBuilder log)
	{
		if (cancellationToken_0.IsCancellationRequested)
		{
			return;
		}
		if (depth > maxDepth)
		{
			if (path.Contains("swiftsoft", StringComparison.OrdinalIgnoreCase))
			{
				StringBuilder stringBuilder = log;
				StringBuilder stringBuilder2 = stringBuilder;
				StringBuilder.AppendInterpolatedStringHandler handler = new StringBuilder.AppendInterpolatedStringHandler(22, 1, stringBuilder);
				handler.AppendLiteral("Reached MAX DEPTH at: ");
				handler.AppendFormatted(path);
				stringBuilder2.AppendLine(ref handler);
			}
			return;
		}
		try
		{
			string fileName = Path.GetFileName(path);
			string text = fileName.ToLowerInvariant();
			if (text.Contains("faceit") || text.Contains("firefox"))
			{
				return;
			}
			if (useSignatureScan)
			{
				foreach (string signature in CheatDatabase.Signatures)
				{
					string value = signature.ToLowerInvariant().Replace(" ", "");
					if (text.Contains(value))
					{
						result.Threats.Add(new ThreatInfo
						{
							Name = fileName,
							Path = path,
							Type = "Suspicious Folder"
						});
						StringBuilder stringBuilder = log;
						StringBuilder stringBuilder3 = stringBuilder;
						StringBuilder.AppendInterpolatedStringHandler handler = new StringBuilder.AppendInterpolatedStringHandler(31, 2, stringBuilder);
						handler.AppendLiteral("THREAT FOUND (FOLDER): ");
						handler.AppendFormatted(path);
						handler.AppendLiteral(" [Sig: ");
						handler.AppendFormatted(signature);
						handler.AppendLiteral("]");
						stringBuilder3.AppendLine(ref handler);
						break;
					}
				}
			}
			result.TotalFoldersScanned++;
			try
			{
				EnumerationOptions enumerationOptions = new EnumerationOptions
				{
					IgnoreInaccessible = true,
					AttributesToSkip = FileAttributes.None
				};
				string[] files = Directory.GetFiles(path, "*", enumerationOptions);
				foreach (string text2 in files)
				{
					if (cancellationToken_0.IsCancellationRequested)
					{
						return;
					}
					string fileName2 = Path.GetFileName(text2);
					string text3 = fileName2.ToLowerInvariant();
					string extension = Path.GetExtension(text3);
					result.TotalFilesScanned++;
					if ((extension != ".exe" && extension != ".ahk" && extension != ".bat" && extension != ".cmd" && extension != ".ini") || !useSignatureScan)
					{
						continue;
					}
					foreach (string signature2 in CheatDatabase.Signatures)
					{
						string value2 = signature2.ToLowerInvariant().Replace(" ", "");
						if (text3.Contains(value2))
						{
							result.Threats.Add(new ThreatInfo
							{
								Name = fileName2,
								Path = text2,
								Type = "Suspicious File"
							});
							StringBuilder stringBuilder = log;
							StringBuilder stringBuilder4 = stringBuilder;
							StringBuilder.AppendInterpolatedStringHandler handler = new StringBuilder.AppendInterpolatedStringHandler(29, 2, stringBuilder);
							handler.AppendLiteral("THREAT FOUND (FILE): ");
							handler.AppendFormatted(text2);
							handler.AppendLiteral(" [Sig: ");
							handler.AppendFormatted(signature2);
							handler.AppendLiteral("]");
							stringBuilder4.AppendLine(ref handler);
							break;
						}
					}
				}
			}
			catch (Exception ex)
			{
				if (path.Contains("Windows", StringComparison.OrdinalIgnoreCase))
				{
					StringBuilder stringBuilder = log;
					StringBuilder stringBuilder5 = stringBuilder;
					StringBuilder.AppendInterpolatedStringHandler handler = new StringBuilder.AppendInterpolatedStringHandler(26, 2, stringBuilder);
					handler.AppendLiteral("Error scanning files in ");
					handler.AppendFormatted(path);
					handler.AppendLiteral(": ");
					handler.AppendFormatted(ex.Message);
					stringBuilder5.AppendLine(ref handler);
				}
			}
			try
			{
				EnumerationOptions enumerationOptions2 = new EnumerationOptions
				{
					IgnoreInaccessible = true,
					AttributesToSkip = FileAttributes.None
				};
				string[] files = Directory.GetDirectories(path, "*", enumerationOptions2);
				foreach (string text4 in files)
				{
					if (cancellationToken_0.IsCancellationRequested)
					{
						break;
					}
					string text5 = Path.GetFileName(text4).ToLowerInvariant();
					string subDirPathLower = text4.ToLowerInvariant();
					if (!text5.StartsWith("."))
					{
						switch (text5)
						{
						default:
							if (!CheatDatabase.SystemPathsToSkip.Any((string skip) => subDirPathLower.Contains(skip)))
							{
								ScanDirectory(text4, result, progress, cancellationToken_0, depth + 1, maxDepth, useIconScan, useSignatureScan, log);
								continue;
							}
							break;
						case "node_modules":
						case "cache":
						case "caches":
						case "__pycache__":
						case "packages":
							break;
						}
					}
					if (subDirPathLower.Contains("windows") || subDirPathLower.Contains("program files"))
					{
						StringBuilder stringBuilder = log;
						StringBuilder stringBuilder6 = stringBuilder;
						StringBuilder.AppendInterpolatedStringHandler handler = new StringBuilder.AppendInterpolatedStringHandler(24, 1, stringBuilder);
						handler.AppendLiteral("Skipped: ");
						handler.AppendFormatted(text4);
						handler.AppendLiteral(" [Filter Match]");
						stringBuilder6.AppendLine(ref handler);
					}
				}
			}
			catch (Exception ex2)
			{
				if (path.Contains("Windows", StringComparison.OrdinalIgnoreCase))
				{
					StringBuilder stringBuilder = log;
					StringBuilder stringBuilder7 = stringBuilder;
					StringBuilder.AppendInterpolatedStringHandler handler = new StringBuilder.AppendInterpolatedStringHandler(28, 2, stringBuilder);
					handler.AppendLiteral("Error scanning subdirs in ");
					handler.AppendFormatted(path);
					handler.AppendLiteral(": ");
					handler.AppendFormatted(ex2.Message);
					stringBuilder7.AppendLine(ref handler);
				}
			}
		}
		catch (Exception ex3)
		{
			StringBuilder stringBuilder = log;
			StringBuilder stringBuilder8 = stringBuilder;
			StringBuilder.AppendInterpolatedStringHandler handler = new StringBuilder.AppendInterpolatedStringHandler(34, 2, stringBuilder);
			handler.AppendLiteral("Critical error in ScanDirectory ");
			handler.AppendFormatted(path);
			handler.AppendLiteral(": ");
			handler.AppendFormatted(ex3.Message);
			stringBuilder8.AppendLine(ref handler);
		}
	}
}
