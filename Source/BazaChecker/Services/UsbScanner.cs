using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Xml;
using BazaChecker.Models;
using Microsoft.Win32;

namespace BazaChecker.Services;

public static class UsbScanner
{
	[StructLayout(LayoutKind.Auto)]
	[CompilerGenerated]
	internal struct _003C_003Ec__DisplayClass2_0
	{
		public XmlNode item;
	}

	private static readonly string[] ExcludedServices = new string[14]
	{
		"usbhub", "usbhub3", "usbccgp", "iusb3hub", "usbehci", "usbxhci", "usbuhci", "usbohci", "RootHub", "pci",
		"ACPI", "volume", "partmgr", "disk"
	};

	private static readonly string[] ExcludedDescPatterns = new string[2] { "root hub", "host controller" };

	public static List<UsbDeviceInfo> GetUsbHistory()
	{
		List<UsbDeviceInfo> list = new List<UsbDeviceInfo>();
		string text = new string[2]
		{
			Path.Combine(AppLauncher.AppsFolder, "USBDeview.exe"),
			Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "apps", "USBDeview.exe")
		}.FirstOrDefault(File.Exists) ?? "";
		string text2 = Path.Combine(Path.GetTempPath(), $"usb_history_{Guid.NewGuid():N}.xml");
		try
		{
			if (File.Exists(text))
			{
				using (Process process = Process.Start(new ProcessStartInfo
				{
					FileName = text,
					Arguments = "/sxml \"" + text2 + "\"",
					UseShellExecute = false,
					CreateNoWindow = true,
					WindowStyle = ProcessWindowStyle.Hidden
				}))
				{
					process?.WaitForExit(10000);
				}
				if (File.Exists(text2))
				{
					XmlDocument xmlDocument = new XmlDocument();
					xmlDocument.Load(text2);
					XmlNodeList xmlNodeList = xmlDocument.SelectNodes("//item");
					if (xmlNodeList != null)
					{
						_003C_003Ec__DisplayClass2_0 _003C_003Ec__DisplayClass2_0_ = default(_003C_003Ec__DisplayClass2_0);
						foreach (XmlNode item in xmlNodeList)
						{
							_003C_003Ec__DisplayClass2_0_.item = item;
							string text3 = smethod_0(new string[3] { "description", "friendly_name", "device_name" }, ref _003C_003Ec__DisplayClass2_0_);
							string text4 = smethod_0(new string[2] { "device_name", "instance_id" }, ref _003C_003Ec__DisplayClass2_0_);
							string text5 = smethod_0(new string[1] { "drive_letter" }, ref _003C_003Ec__DisplayClass2_0_);
							bool flag = smethod_0(new string[1] { "connected" }, ref _003C_003Ec__DisplayClass2_0_).Equals("Yes", StringComparison.OrdinalIgnoreCase);
							string installDate = smethod_0(new string[3] { "install_date", "install_time", "registry_time_1" }, ref _003C_003Ec__DisplayClass2_0_);
							string text6 = smethod_0(new string[4] { "disconnect_date", "disconnect_time", "registry_time_2", "last_plug_unplug_date" }, ref _003C_003Ec__DisplayClass2_0_);
							string text7 = smethod_0(new string[3] { "serial_number", "serialnumber", "instance_id" }, ref _003C_003Ec__DisplayClass2_0_);
							string text8 = smethod_0(new string[3] { "vendorid", "vendor_id", "vid" }, ref _003C_003Ec__DisplayClass2_0_);
							string text9 = smethod_0(new string[3] { "productid", "product_id", "pid" }, ref _003C_003Ec__DisplayClass2_0_);
							smethod_0(new string[1] { "service_name" }, ref _003C_003Ec__DisplayClass2_0_);
							string text10 = smethod_0(new string[1] { "device_type" }, ref _003C_003Ec__DisplayClass2_0_);
							string descLower = text3.ToLower() + " " + text4.ToLower() + " " + text10.ToLower();
							if (!ExcludedDescPatterns.Any((string string_0) => descLower.Contains(string_0)) && (!string.IsNullOrEmpty(text8) || !string.IsNullOrEmpty(text9)))
							{
								if (text8.Length > 4 && text8.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
								{
									text8 = text8.Substring(2);
								}
								if (text9.Length > 4 && text9.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
								{
									text9 = text9.Substring(2);
								}
								text8 = text8.ToUpper().Trim();
								text9 = text9.ToUpper().Trim();
								if (text7.Contains("&"))
								{
									text7 = text7.Split('&').LastOrDefault() ?? "";
								}
								if (text7.Length > 20)
								{
									text7 = text7.Substring(0, 20) + "...";
								}
								string text11 = text3;
								if (!string.IsNullOrEmpty(text5))
								{
									text11 = "(" + text5 + ") " + text11;
								}
								list.Add(new UsbDeviceInfo
								{
									Name = text11,
									Description = text10,
									DriveLetter = text5,
									VID = text8,
									PID = text9,
									SerialNumber = text7,
									IsConnected = flag,
									InstallDate = installDate,
									DisconnectDate = (flag ? "" : text6)
								});
							}
						}
					}
				}
			}
		}
		catch
		{
		}
		finally
		{
			if (File.Exists(text2))
			{
				try
				{
					File.Delete(text2);
				}
				catch
				{
				}
			}
		}
		List<UsbDeviceInfo> list2 = (from usbDeviceInfo_0 in list
			orderby usbDeviceInfo_0.IsConnected descending, string.IsNullOrEmpty(usbDeviceInfo_0.InstallDate) ? "" : usbDeviceInfo_0.InstallDate descending
			select usbDeviceInfo_0).ToList();
		if (list2.Count == 0)
		{
			list2 = GetUsbHistoryNative();
		}
		for (int num = 0; num < list2.Count; num++)
		{
			list2[num].DeviceIndex = num + 1;
		}
		return list2;
	}

	private static List<UsbDeviceInfo> GetUsbHistoryNative()
	{
		List<UsbDeviceInfo> list = new List<UsbDeviceInfo>();
		try
		{
			using RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("SYSTEM\\CurrentControlSet\\Enum\\USBSTOR");
			if (registryKey != null)
			{
				string[] subKeyNames = registryKey.GetSubKeyNames();
				foreach (string text in subKeyNames)
				{
					using RegistryKey registryKey2 = registryKey.OpenSubKey(text);
					if (registryKey2 == null)
					{
						continue;
					}
					string[] subKeyNames2 = registryKey2.GetSubKeyNames();
					foreach (string text2 in subKeyNames2)
					{
						using RegistryKey registryKey3 = registryKey2.OpenSubKey(text2);
						if (registryKey3 == null)
						{
							continue;
						}
						string text3 = registryKey3.GetValue("FriendlyName")?.ToString();
						if (string.IsNullOrEmpty(text3))
						{
							string text4 = registryKey3.GetValue("DeviceDesc")?.ToString();
							if (!string.IsNullOrEmpty(text4))
							{
								text3 = text4.Split(';').Last();
							}
						}
						if (string.IsNullOrEmpty(text3))
						{
							text3 = text;
						}
						list.Add(new UsbDeviceInfo
						{
							Name = text3,
							Description = "USB Storage",
							SerialNumber = text2,
							VID = "N/A",
							PID = "N/A",
							IsConnected = false,
							InstallDate = "Unknown",
							DisconnectDate = ""
						});
					}
				}
			}
		}
		catch
		{
		}
		return list;
	}

	[CompilerGenerated]
	internal static string smethod_0(string[] tags, ref _003C_003Ec__DisplayClass2_0 _003C_003Ec__DisplayClass2_0_0)
	{
		int num = 0;
		string text;
		while (true)
		{
			if (num < tags.Length)
			{
				string xpath = tags[num];
				text = _003C_003Ec__DisplayClass2_0_0.item.SelectSingleNode(xpath)?.InnerText?.Trim();
				if (!string.IsNullOrEmpty(text))
				{
					break;
				}
				num++;
				continue;
			}
			return "";
		}
		return text;
	}
}
