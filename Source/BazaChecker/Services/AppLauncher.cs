using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;

namespace BazaChecker.Services;

public static class AppLauncher
{
	public static readonly string AppsFolder = GetAppsFolder();

	private static string GetAppsFolder()
	{
		string path = string.Empty;
		try
		{
			path = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule?.FileName) ?? string.Empty;
		}
		catch
		{
		}
		string[] array = new string[3]
		{
			Path.Combine(path, "apps"),
			Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "apps"),
			Path.Combine(Environment.CurrentDirectory, "apps")
		};
		int num = 0;
		string text;
		while (true)
		{
			if (num < array.Length)
			{
				text = array[num];
				if (!string.IsNullOrEmpty(text) && Directory.Exists(text))
				{
					break;
				}
				num++;
				continue;
			}
			return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "apps");
		}
		return text;
	}

	public static bool RunTool(string exeName)
	{
		try
		{
			string text = Path.Combine(AppsFolder, exeName);
			if (!File.Exists(text))
			{
				MessageBox.Show("Инструмент не найден:\n" + text + "\n\nУбедитесь, что папка 'apps' находится рядом с программой или дождитесь завершения инициализации.", "Ошибка запуска", MessageBoxButton.OK, MessageBoxImage.Exclamation);
				return false;
			}
			UnblockFile(text);
			ProcessStartInfo processStartInfo = new ProcessStartInfo
			{
				FileName = text,
				WorkingDirectory = AppsFolder,
				UseShellExecute = true
			};
			if (exeName.EndsWith(".exe", StringComparison.OrdinalIgnoreCase))
			{
				processStartInfo.Verb = "runas";
			}
			Process.Start(processStartInfo);
			return true;
		}
		catch (Win32Exception ex) when (ex.NativeErrorCode == 1223)
		{
			return false;
		}
		catch (Exception ex2)
		{
			MessageBox.Show($"Не удалось запустить {exeName}:\n{ex2.Message}\n\nПопробуйте запустить основную программу от имени администратора.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Hand);
			return false;
		}
	}

	[DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
	[return: MarshalAs(UnmanagedType.Bool)]
	private static extern bool DeleteFile(string name);

	public static void UnblockFile(string filename)
	{
		try
		{
			DeleteFile(filename + ":Zone.Identifier");
		}
		catch
		{
		}
	}

	public static void EnsureAppsFolderExists()
	{
		if (!Directory.Exists(AppsFolder))
		{
			Directory.CreateDirectory(AppsFolder);
		}
	}

	public static void ExtractEmbeddedTools()
	{
		try
		{
			string text = new string[3]
			{
				Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "appdata.bin"),
				Path.Combine(Path.GetDirectoryName(Process.GetCurrentProcess().MainModule?.FileName) ?? "", "appdata.bin"),
				"appdata.bin"
			}.FirstOrDefault(File.Exists) ?? "";
			if (string.IsNullOrEmpty(text))
			{
				if (Directory.Exists(AppsFolder))
				{
					_ = Directory.GetFiles(AppsFolder, "*", SearchOption.AllDirectories).LongLength;
				}
			}
			else
			{
				if (Directory.Exists(AppsFolder) && Directory.GetFiles(AppsFolder, "*", SearchOption.AllDirectories).Length != 0)
				{
					return;
				}
				EnsureAppsFolderExists();
				byte[] array = new byte[32]
				{
					68, 105, 97, 103, 67, 111, 114, 101, 95, 83,
					101, 99, 117, 114, 101, 95, 65, 69, 83, 95,
					80, 114, 111, 116, 101, 99, 116, 95, 50, 48,
					50, 54
				};
				byte[] array2 = File.ReadAllBytes(text);
				byte[] array3 = new byte[array2.Length];
				for (int i = 0; i < array2.Length; i++)
				{
					byte b = (byte)(array[i % array.Length] ^ (i & 0xFF) ^ ((i >> 8) & 0xFF));
					array3[i] = (byte)(array2[i] ^ b);
				}
				using MemoryStream stream = new MemoryStream(array3);
				using ZipArchive zipArchive = new ZipArchive(stream);
				foreach (ZipArchiveEntry entry in zipArchive.Entries)
				{
					if (!string.IsNullOrEmpty(entry.Name))
					{
						string text2 = Path.Combine(AppsFolder, entry.FullName);
						string path = Path.GetDirectoryName(text2) ?? AppsFolder;
						if (!Directory.Exists(path))
						{
							Directory.CreateDirectory(path);
						}
						entry.ExtractToFile(text2, overwrite: true);
					}
				}
				return;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show("Ошибка при распаковке инструментов: " + ex.Message + "\n\nПопробуйте запустить программу от имени Администратора.", "Ошибка инициализации", MessageBoxButton.OK, MessageBoxImage.Exclamation);
		}
	}
}
