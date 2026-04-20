using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;
using System.Net.NetworkInformation;
using System.Text;
using Microsoft.Win32;

namespace BazaChecker.Services;

public static class VmDetector
{
	private static StringBuilder _log = new StringBuilder();

	public static string GetDetectionLog()
	{
		if (_log.Length == 0)
		{
			return "Отчет еще не сформирован. Запустите проверку.";
		}
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.AppendLine("[-] Sayurin Checker VM Scan Report");
		stringBuilder.AppendLine("[-] Started at: " + DateTime.Now);
		stringBuilder.AppendLine("--------------------------------------------------");
		stringBuilder.Append(_log.ToString());
		stringBuilder.AppendLine("--------------------------------------------------");
		stringBuilder.AppendLine("[-] End of report.");
		return stringBuilder.ToString();
	}

	public static bool IsVirtualMachine()
	{
		_log.Clear();
		bool flag = false;
		_log.AppendLine("[-] Checking Registry keys...");
		if (CheckRegistry())
		{
			flag = true;
		}
		_log.AppendLine("\n[-] Checking Processes...");
		if (CheckProcesses())
		{
			flag = true;
		}
		_log.AppendLine("\n[-] Checking Driver Files...");
		if (CheckFiles())
		{
			flag = true;
		}
		_log.AppendLine("\n[-] Checking MAC Addresses...");
		if (CheckMacAddress())
		{
			flag = true;
		}
		_log.AppendLine("\n[-] Checking WMI Objects...");
		if (CheckWMI())
		{
			flag = true;
		}
		if (flag)
		{
			_log.AppendLine("\n[!] RESULT: VIRTUAL MACHINE DETECTED!");
		}
		else
		{
			_log.AppendLine("\n[*] RESULT: System appears to be CLEAN.");
		}
		return flag;
	}

	private static bool CheckRegistry()
	{
		bool result = false;
		string[] array = new string[3] { "SOFTWARE\\Oracle\\VirtualBox Guest Additions", "SOFTWARE\\VMware, Inc.\\VMware Tools", "HARDWARE\\DESCRIPTION\\System\\SystemBiosVersion" };
		foreach (string text in array)
		{
			using RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(text);
			if (registryKey != null)
			{
				StringBuilder log = _log;
				StringBuilder stringBuilder = log;
				StringBuilder.AppendInterpolatedStringHandler handler = new StringBuilder.AppendInterpolatedStringHandler(29, 1, log);
				handler.AppendLiteral("[!] Found Key: ");
				handler.AppendFormatted(text);
				handler.AppendLiteral(" ... DETECTED!");
				stringBuilder.AppendLine(ref handler);
				result = true;
			}
			else
			{
				StringBuilder log = _log;
				StringBuilder stringBuilder2 = log;
				StringBuilder.AppendInterpolatedStringHandler handler = new StringBuilder.AppendInterpolatedStringHandler(16, 1, log);
				handler.AppendLiteral("[*] Key: ");
				handler.AppendFormatted(text);
				handler.AppendLiteral(" ... OK");
				stringBuilder2.AppendLine(ref handler);
			}
		}
		using (RegistryKey registryKey2 = Registry.LocalMachine.OpenSubKey("HARDWARE\\DESCRIPTION\\System"))
		{
			object obj;
			object obj2;
			object obj3;
			if (registryKey2 != null)
			{
				obj = registryKey2.GetValue("SystemBiosVersion") as string[];
				if (obj == null)
				{
					obj = new string[1];
					obj2 = obj;
					object? value = registryKey2.GetValue("SystemBiosVersion");
					if (value == null)
					{
						obj3 = null;
					}
					else
					{
						obj3 = value.ToString();
						if (obj3 != null)
						{
							goto IL_012c;
						}
					}
					obj3 = "";
					goto IL_012c;
				}
				goto IL_012d;
			}
			goto end_IL_00e7;
			IL_012d:
			string[] array2 = (string[])obj;
			object obj4 = registryKey2.GetValue("VideoBiosVersion") as string[];
			object obj5;
			object obj6;
			if (obj4 == null)
			{
				obj4 = new string[1];
				obj5 = obj4;
				object? value2 = registryKey2.GetValue("VideoBiosVersion");
				if (value2 == null)
				{
					obj6 = null;
				}
				else
				{
					obj6 = value2.ToString();
					if (obj6 != null)
					{
						goto IL_016d;
					}
				}
				obj6 = "";
				goto IL_016d;
			}
			goto IL_016e;
			IL_012c:
			((object[])obj2)[0] = obj3;
			goto IL_012d;
			IL_016e:
			string[] array3 = (string[])obj4;
			bool flag = true;
			array = array2;
			foreach (string text2 in array)
			{
				if (CheckString(text2))
				{
					StringBuilder log = _log;
					StringBuilder stringBuilder3 = log;
					StringBuilder.AppendInterpolatedStringHandler handler = new StringBuilder.AppendInterpolatedStringHandler(46, 1, log);
					handler.AppendLiteral("[!] SystemBiosVersion artifact: ");
					handler.AppendFormatted(text2);
					handler.AppendLiteral(" ... DETECTED!");
					stringBuilder3.AppendLine(ref handler);
					result = true;
					flag = false;
				}
			}
			if (flag)
			{
				_log.AppendLine("[*] SystemBiosVersion check ... OK");
			}
			bool flag2 = true;
			array = array3;
			foreach (string text3 in array)
			{
				if (CheckString(text3))
				{
					StringBuilder log = _log;
					StringBuilder stringBuilder4 = log;
					StringBuilder.AppendInterpolatedStringHandler handler = new StringBuilder.AppendInterpolatedStringHandler(45, 1, log);
					handler.AppendLiteral("[!] VideoBiosVersion artifact: ");
					handler.AppendFormatted(text3);
					handler.AppendLiteral(" ... DETECTED!");
					stringBuilder4.AppendLine(ref handler);
					result = true;
					flag2 = false;
				}
			}
			if (flag2)
			{
				_log.AppendLine("[*] VideoBiosVersion check ... OK");
			}
			goto end_IL_00e7;
			IL_016d:
			((object[])obj5)[0] = obj6;
			goto IL_016e;
			end_IL_00e7:;
		}
		return result;
	}

	private static bool CheckString(string string_0)
	{
		string text = string_0.ToLower();
		if (!text.Contains("vbox") && !text.Contains("vmware") && !text.Contains("qemu"))
		{
			return text.Contains("virt");
		}
		return true;
	}

	private static bool CheckProcesses()
	{
		bool result = false;
		string[] source = new string[12]
		{
			"vboxservice", "vboxtray", "vmtoolsd", "vmwaretray", "vmwareuser", "vgauthservice", "vmacthlp", "vmsrvc", "vmusrvc", "prl_cc",
			"prl_tools", "xenservice"
		};
		Process[] processes = Process.GetProcesses();
		bool flag = false;
		Process[] array = processes;
		foreach (Process process in array)
		{
			if (source.Contains(process.ProcessName.ToLower()))
			{
				StringBuilder log = _log;
				StringBuilder.AppendInterpolatedStringHandler handler = new StringBuilder.AppendInterpolatedStringHandler(36, 1, log);
				handler.AppendLiteral("[!] Found VM Process: ");
				handler.AppendFormatted(process.ProcessName);
				handler.AppendLiteral(" ... DETECTED!");
				log.AppendLine(ref handler);
				result = true;
				flag = true;
			}
		}
		if (!flag)
		{
			_log.AppendLine("[*] Checking known VM processes ... OK");
		}
		return result;
	}

	private static bool CheckFiles()
	{
		bool result = false;
		string path = Path.Combine(Environment.GetEnvironmentVariable("windir") ?? "C:\\Windows", "System32");
		string path2 = Path.Combine(path, "drivers");
		string[] array = new string[22]
		{
			Path.Combine(path2, "VBoxMouse.sys"),
			Path.Combine(path2, "VBoxGuest.sys"),
			Path.Combine(path2, "VBoxSF.sys"),
			Path.Combine(path2, "VBoxVideo.sys"),
			Path.Combine(path, "vboxdisp.dll"),
			Path.Combine(path, "vboxhook.dll"),
			Path.Combine(path, "vboxmrxnp.dll"),
			Path.Combine(path, "vboxogl.dll"),
			Path.Combine(path, "vboxoglarrayspu.dll"),
			Path.Combine(path, "vboxoglcrutil.dll"),
			Path.Combine(path, "vboxoglerrorspu.dll"),
			Path.Combine(path, "vboxoglfeedbackspu.dll"),
			Path.Combine(path, "vboxoglpackspu.dll"),
			Path.Combine(path, "vboxoglpassthroughspu.dll"),
			Path.Combine(path, "vboxservice.exe"),
			Path.Combine(path, "vboxtray.exe"),
			Path.Combine(path, "VBoxControl.exe"),
			Path.Combine(path2, "vmmouse.sys"),
			Path.Combine(path2, "vmnet.sys"),
			Path.Combine(path2, "vmxnet.sys"),
			Path.Combine(path2, "vmhgfs.sys"),
			Path.Combine(path2, "vmtools.sys")
		};
		foreach (string text in array)
		{
			if (File.Exists(text))
			{
				StringBuilder log = _log;
				StringBuilder stringBuilder = log;
				StringBuilder.AppendInterpolatedStringHandler handler = new StringBuilder.AppendInterpolatedStringHandler(37, 1, log);
				handler.AppendLiteral("[!] Found Driver/Tool: ");
				handler.AppendFormatted(text);
				handler.AppendLiteral(" ... DETECTED!");
				stringBuilder.AppendLine(ref handler);
				result = true;
			}
			else
			{
				StringBuilder log = _log;
				StringBuilder stringBuilder2 = log;
				StringBuilder.AppendInterpolatedStringHandler handler = new StringBuilder.AppendInterpolatedStringHandler(17, 1, log);
				handler.AppendLiteral("[*] File: ");
				handler.AppendFormatted(text);
				handler.AppendLiteral(" ... OK");
				stringBuilder2.AppendLine(ref handler);
			}
		}
		return result;
	}

	private static bool CheckMacAddress()
	{
		bool result = false;
		int num = 0;
		try
		{
			NetworkInterface[] allNetworkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
			foreach (NetworkInterface networkInterface in allNetworkInterfaces)
			{
				if (networkInterface.NetworkInterfaceType == NetworkInterfaceType.Loopback)
				{
					continue;
				}
				string text = networkInterface.GetPhysicalAddress().ToString().ToUpper();
				string value = "";
				if (!text.StartsWith("000569") && !text.StartsWith("000C29") && !text.StartsWith("001C14") && !text.StartsWith("005056"))
				{
					if (text.StartsWith("080027"))
					{
						value = "VirtualBox";
					}
					else if (text.StartsWith("001C42"))
					{
						value = "Parallels";
					}
					else if (text.StartsWith("00163E"))
					{
						value = "Xen";
					}
				}
				else
				{
					value = "VMware";
				}
				if (!string.IsNullOrEmpty(value))
				{
					StringBuilder log = _log;
					StringBuilder stringBuilder = log;
					StringBuilder.AppendInterpolatedStringHandler handler = new StringBuilder.AppendInterpolatedStringHandler(29, 3, log);
					handler.AppendLiteral("[!] MAC: ");
					handler.AppendFormatted(networkInterface.Name);
					handler.AppendLiteral(" (");
					handler.AppendFormatted(text);
					handler.AppendLiteral(") - ");
					handler.AppendFormatted(value);
					handler.AppendLiteral(" ... DETECTED!");
					stringBuilder.AppendLine(ref handler);
					result = true;
				}
				else
				{
					StringBuilder log = _log;
					StringBuilder stringBuilder2 = log;
					StringBuilder.AppendInterpolatedStringHandler handler = new StringBuilder.AppendInterpolatedStringHandler(19, 2, log);
					handler.AppendLiteral("[*] MAC: ");
					handler.AppendFormatted(networkInterface.Name);
					handler.AppendLiteral(" (");
					handler.AppendFormatted(text);
					handler.AppendLiteral(") ... OK");
					stringBuilder2.AppendLine(ref handler);
				}
				num++;
			}
			if (num == 0)
			{
				_log.AppendLine("[?] No network interfaces found to check.");
			}
		}
		catch (Exception ex)
		{
			StringBuilder log = _log;
			StringBuilder stringBuilder3 = log;
			StringBuilder.AppendInterpolatedStringHandler handler = new StringBuilder.AppendInterpolatedStringHandler(21, 1, log);
			handler.AppendLiteral("[!] MAC Check Error: ");
			handler.AppendFormatted(ex.Message);
			stringBuilder3.AppendLine(ref handler);
		}
		return result;
	}

	private static bool CheckWMI()
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Expected O, but got Unknown
		bool result = false;
		try
		{
			ManagementObjectSearcher val = new ManagementObjectSearcher("SELECT Manufacturer, Model FROM Win32_ComputerSystem");
			try
			{
				var enumerator = val.Get().GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						ManagementBaseObject current = enumerator.Current;
						object obj = current["Manufacturer"];
						object obj2;
						if (obj == null)
						{
							obj2 = null;
						}
						else
						{
							string? text = obj.ToString();
							if (text == null)
							{
								obj2 = null;
							}
							else
							{
								obj2 = text.ToLower();
								if (obj2 != null)
								{
									goto IL_0051;
								}
							}
						}
						obj2 = "";
						goto IL_0051;
						IL_0051:
						string text2 = (string)obj2;
						object obj3 = current["Model"];
						object obj4;
						if (obj3 == null)
						{
							obj4 = null;
						}
						else
						{
							string? text3 = obj3.ToString();
							if (text3 == null)
							{
								obj4 = null;
							}
							else
							{
								obj4 = text3.ToLower();
								if (obj4 != null)
								{
									goto IL_007d;
								}
							}
						}
						obj4 = "";
						goto IL_007d;
						IL_007d:
						string text4 = (string)obj4;
						bool flag = true;
						if (text2.Contains("microsoft corporation") && text4.Contains("virtual"))
						{
							StringBuilder log = _log;
							StringBuilder stringBuilder = log;
							StringBuilder.AppendInterpolatedStringHandler handler = new StringBuilder.AppendInterpolatedStringHandler(36, 1, log);
							handler.AppendLiteral("[!] WMI Manufacturer: ");
							handler.AppendFormatted(text2);
							handler.AppendLiteral(" ... DETECTED!");
							stringBuilder.AppendLine(ref handler);
							result = true;
							flag = false;
						}
						if (text2.Contains("vmware") || text4.Contains("vmware"))
						{
							_log.AppendLine("[!] WMI Manufacturer/Model: VMware ... DETECTED!");
							result = true;
							flag = false;
						}
						if (text4.Contains("virtualbox"))
						{
							_log.AppendLine("[!] WMI Model: VirtualBox ... DETECTED!");
							result = true;
							flag = false;
						}
						if (text4.Contains("kvm") || text4.Contains("qemu") || text4.Contains("bochs"))
						{
							_log.AppendLine("[!] WMI Model: QEMU/KVM ... DETECTED!");
							result = true;
							flag = false;
						}
						if (flag)
						{
							_log.AppendLine("[*] WMI System Info ... OK");
						}
					}
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
		}
		catch (Exception ex)
		{
			StringBuilder log = _log;
			StringBuilder stringBuilder2 = log;
			StringBuilder.AppendInterpolatedStringHandler handler = new StringBuilder.AppendInterpolatedStringHandler(21, 1, log);
			handler.AppendLiteral("[!] WMI Check Error: ");
			handler.AppendFormatted(ex.Message);
			stringBuilder2.AppendLine(ref handler);
		}
		return result;
	}
}
