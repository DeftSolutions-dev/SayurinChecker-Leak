using System;
using System.Collections.Generic;

namespace BazaChecker.Models;

public class ScanResult
{
	public int TotalProcesses { get; set; }

	public int ThreatsFound { get; set; }

	public List<string> DetectedThreats { get; set; } = new List<string>();

	public DateTime ScanTime { get; set; } = DateTime.Now;
}
