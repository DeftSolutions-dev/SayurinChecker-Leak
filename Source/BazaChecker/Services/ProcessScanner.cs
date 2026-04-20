using System.Diagnostics;
using BazaChecker.Models;

namespace BazaChecker.Services;

public static class ProcessScanner
{
	public static ScanResult Scan()
	{
		ScanResult scanResult = new ScanResult();
		try
		{
			Process[] processes = Process.GetProcesses();
			scanResult.TotalProcesses = processes.Length;
			Process[] array = processes;
			foreach (Process process in array)
			{
				try
				{
					string text = process.ProcessName.ToLowerInvariant();
					foreach (string signature in CheatDatabase.Signatures)
					{
						string value = signature.ToLowerInvariant().Replace(" ", "");
						if (text.Contains(value))
						{
							scanResult.ThreatsFound++;
							if (!scanResult.DetectedThreats.Contains(process.ProcessName))
							{
								scanResult.DetectedThreats.Add(process.ProcessName);
							}
							break;
						}
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
		return scanResult;
	}
}
