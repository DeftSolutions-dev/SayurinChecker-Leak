using System;
using System.Collections.Generic;
using System.Management;
using System.Windows.Forms;
using BazaChecker.Models;

namespace BazaChecker.Services;

public static class SystemInfoProvider
{
	public static SystemSummary GetSummary()
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		//IL_049c: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a3: Expected O, but got Unknown
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Expected O, but got Unknown
		//IL_04be: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0115: Unknown result type (might be due to invalid IL or missing references)
		//IL_011c: Expected O, but got Unknown
		//IL_0137: Unknown result type (might be due to invalid IL or missing references)
		//IL_013e: Expected O, but got Unknown
		//IL_01c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cd: Expected O, but got Unknown
		//IL_01f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fb: Expected O, but got Unknown
		//IL_02f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f8: Expected O, but got Unknown
		//IL_0313: Unknown result type (might be due to invalid IL or missing references)
		//IL_0385: Unknown result type (might be due to invalid IL or missing references)
		//IL_038c: Expected O, but got Unknown
		//IL_03ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b1: Unknown result type (might be due to invalid IL or missing references)
		SystemSummary systemSummary = new SystemSummary();
		try
		{
			ManagementObjectSearcher val = new ManagementObjectSearcher("SELECT Caption, InstallDate, LastBootUpTime, Version FROM Win32_OperatingSystem");
			try
			{
				var enumerator = val.Get().GetEnumerator();
				try
				{
					ManagementObject val2;
					object obj2;
					if (enumerator.MoveNext())
					{
						val2 = (ManagementObject)enumerator.Current;
						object obj = ((ManagementBaseObject)val2)["Caption"];
						if (obj == null)
						{
							obj2 = null;
						}
						else
						{
							obj2 = obj.ToString();
							if (obj2 != null)
							{
								goto IL_0055;
							}
						}
						obj2 = "Windows";
						goto IL_0055;
					}
					goto end_IL_001d;
					IL_0055:
					systemSummary.OSName = (string)obj2;
					object obj3 = ((ManagementBaseObject)val2)["Version"];
					object obj4;
					if (obj3 == null)
					{
						obj4 = null;
					}
					else
					{
						obj4 = obj3.ToString();
						if (obj4 != null)
						{
							goto IL_007b;
						}
					}
					obj4 = "";
					goto IL_007b;
					IL_007b:
					systemSummary.OSVersion = (string)obj4;
					string text = ((ManagementBaseObject)val2)["InstallDate"]?.ToString();
					if (!string.IsNullOrEmpty(text))
					{
						systemSummary.InstallDate = ManagementDateTimeConverter.ToDateTime(text);
					}
					string text2 = ((ManagementBaseObject)val2)["LastBootUpTime"]?.ToString();
					if (!string.IsNullOrEmpty(text2))
					{
						systemSummary.LastBootTime = ManagementDateTimeConverter.ToDateTime(text2);
						systemSummary.SessionStartTime = ManagementDateTimeConverter.ToDateTime(text2);
					}
					end_IL_001d:;
				}
				finally
				{
					((IDisposable)enumerator)?.Dispose();
				}
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
			systemSummary.MonitorCount = Screen.AllScreens.Length;
			ManagementObjectSearcher val3 = new ManagementObjectSearcher("SELECT Name, NumberOfCores, NumberOfLogicalProcessors FROM Win32_Processor");
			try
			{
				var enumerator = val3.Get().GetEnumerator();
				try
				{
					ManagementObject val4;
					object obj6;
					if (enumerator.MoveNext())
					{
						val4 = (ManagementObject)enumerator.Current;
						object obj5 = ((ManagementBaseObject)val4)["Name"];
						if (obj5 == null)
						{
							obj6 = null;
						}
						else
						{
							obj6 = obj5.ToString();
							if (obj6 != null)
							{
								goto IL_0160;
							}
						}
						obj6 = "Unknown";
						goto IL_0160;
					}
					goto end_IL_0129;
					IL_0160:
					systemSummary.CPU = (string)obj6;
					systemSummary.CPUCores = Convert.ToInt32(((ManagementBaseObject)val4)["NumberOfCores"] ?? ((object)0));
					systemSummary.CPUThreads = Convert.ToInt32(((ManagementBaseObject)val4)["NumberOfLogicalProcessors"] ?? ((object)0));
					end_IL_0129:;
				}
				finally
				{
					((IDisposable)enumerator)?.Dispose();
				}
			}
			finally
			{
				((IDisposable)val3)?.Dispose();
			}
			ManagementObjectSearcher val5 = new ManagementObjectSearcher("SELECT Name, AdapterRAM FROM Win32_VideoController");
			try
			{
				List<string> list = new List<string>();
				List<string> list2 = new List<string>();
				var enumerator = val5.Get().GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						ManagementObject val6 = (ManagementObject)enumerator.Current;
						string text3 = ((ManagementBaseObject)val6)["Name"]?.ToString();
						if (!string.IsNullOrWhiteSpace(text3))
						{
							list.Add(text3);
							double value = (double)Convert.ToInt64(((ManagementBaseObject)val6)["AdapterRAM"] ?? ((object)0)) / 1073741824.0;
							list2.Add($"{Math.Round(value, 1)} GB");
						}
					}
				}
				finally
				{
					((IDisposable)enumerator)?.Dispose();
				}
				systemSummary.GPU = ((list.Count > 0) ? string.Join(" + ", list) : "Unknown");
				systemSummary.GPUVRAM = ((list2.Count > 0) ? string.Join(" + ", list2) : "Unknown");
			}
			finally
			{
				((IDisposable)val5)?.Dispose();
			}
			ManagementObjectSearcher val7 = new ManagementObjectSearcher("SELECT TotalPhysicalMemory FROM Win32_ComputerSystem");
			try
			{
				var enumerator = val7.Get().GetEnumerator();
				try
				{
					if (enumerator.MoveNext())
					{
						int value2 = (int)Math.Round((double)Convert.ToInt64(((ManagementBaseObject)(ManagementObject)enumerator.Current)["TotalPhysicalMemory"]) / 1073741824.0);
						systemSummary.RAM = $"{value2} GB";
					}
				}
				finally
				{
					((IDisposable)enumerator)?.Dispose();
				}
			}
			finally
			{
				((IDisposable)val7)?.Dispose();
			}
			try
			{
				ManagementObjectSearcher val8 = new ManagementObjectSearcher("SELECT Model, Size FROM Win32_DiskDrive");
				try
				{
					List<string> list3 = new List<string>();
					var enumerator = val8.Get().GetEnumerator();
					try
					{
						ManagementObject val9;
						object obj8;
						string value3;
						double a;
						for (; enumerator.MoveNext(); value3 = (string)obj8, a = (double)Convert.ToInt64(((ManagementBaseObject)val9)["Size"] ?? ((object)0)) / 1073741824.0, list3.Add($"{value3} ({Math.Round(a)} GB)"))
						{
							val9 = (ManagementObject)enumerator.Current;
							object obj7 = ((ManagementBaseObject)val9)["Model"];
							if (obj7 == null)
							{
								obj8 = null;
							}
							else
							{
								obj8 = obj7.ToString();
								if (obj8 != null)
								{
									continue;
								}
							}
							obj8 = "Unknown";
						}
					}
					finally
					{
						((IDisposable)enumerator)?.Dispose();
					}
					systemSummary.SSD = ((list3.Count > 0) ? string.Join(", ", list3) : "Unknown");
				}
				finally
				{
					((IDisposable)val8)?.Dispose();
				}
			}
			catch
			{
				systemSummary.SSD = "Unknown";
			}
			ManagementObjectSearcher val10 = new ManagementObjectSearcher("SELECT Manufacturer, Product FROM Win32_BaseBoard");
			try
			{
				var enumerator = val10.Get().GetEnumerator();
				try
				{
					ManagementObject val11;
					object obj11;
					if (enumerator.MoveNext())
					{
						val11 = (ManagementObject)enumerator.Current;
						object obj10 = ((ManagementBaseObject)val11)["Manufacturer"];
						if (obj10 == null)
						{
							obj11 = null;
						}
						else
						{
							obj11 = obj10.ToString();
							if (obj11 != null)
							{
								goto IL_04e3;
							}
						}
						obj11 = "";
						goto IL_04e3;
					}
					goto end_IL_04b0;
					IL_04e3:
					string text4 = (string)obj11;
					object obj12 = ((ManagementBaseObject)val11)["Product"];
					object obj13;
					if (obj12 == null)
					{
						obj13 = null;
					}
					else
					{
						obj13 = obj12.ToString();
						if (obj13 != null)
						{
							goto IL_0504;
						}
					}
					obj13 = "";
					goto IL_0504;
					IL_0504:
					string text5 = (string)obj13;
					systemSummary.Motherboard = (text4 + " " + text5).Trim();
					if (string.IsNullOrEmpty(systemSummary.Motherboard))
					{
						systemSummary.Motherboard = "Unknown";
					}
					end_IL_04b0:;
				}
				finally
				{
					((IDisposable)enumerator)?.Dispose();
				}
			}
			finally
			{
				((IDisposable)val10)?.Dispose();
			}
			if (!systemSummary.IsVirtualMachine)
			{
				systemSummary.IsVirtualMachine = VmDetector.IsVirtualMachine();
			}
			if (systemSummary.IsVirtualMachine)
			{
				systemSummary.Motherboard += " (Virtual)";
			}
			if (string.IsNullOrEmpty(systemSummary.OSName))
			{
				systemSummary.OSName = Environment.OSVersion.ToString();
				systemSummary.MonitorCount = 1;
			}
		}
		catch
		{
			systemSummary.OSName = Environment.OSVersion.ToString();
			systemSummary.MonitorCount = 1;
		}
		return systemSummary;
	}
}
