using System;

namespace BazaChecker.Models;

public class SystemSummary
{
	public string OSName { get; set; } = "Unknown";

	public string OSVersion { get; set; } = string.Empty;

	public DateTime InstallDate { get; set; }

	public int MonitorCount { get; set; } = 1;

	public DateTime LastBootTime { get; set; }

	public string MachineName { get; set; } = Environment.MachineName;

	public string UserName { get; set; } = Environment.UserName;

	public string CPU { get; set; } = "Unknown";

	public string GPU { get; set; } = "Unknown";

	public string RAM { get; set; } = "Unknown";

	public string SSD { get; set; } = "Unknown";

	public string Motherboard { get; set; } = "Unknown";

	public int CPUCores { get; set; }

	public int CPUThreads { get; set; }

	public string GPUVRAM { get; set; } = "Unknown";

	public bool IsVirtualMachine { get; set; }

	public DateTime SessionStartTime { get; set; } = DateTime.Now;

	public string InstallDateText => InstallDate.ToString("dd.MM.yyyy");

	public string LastBootText => LastBootTime.ToString("dd.MM.yyyy HH:mm");

	public string SessionTimeText => SessionStartTime.ToString("dd.MM.yyyy HH:mm");

	public string IsVirtualMachineText
	{
		get
		{
			if (!IsVirtualMachine)
			{
				return "Нет";
			}
			return "Да";
		}
	}
}
