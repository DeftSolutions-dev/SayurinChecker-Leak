using System;
using System.Collections.Generic;

namespace BazaChecker.Services;

public class DiskScanResult
{
	public int TotalFilesScanned { get; set; }

	public int TotalFoldersScanned { get; set; }

	public List<ThreatInfo> Threats { get; set; } = new List<ThreatInfo>();

	public TimeSpan ScanDuration { get; set; }

	public bool WasCancelled { get; set; }
}
