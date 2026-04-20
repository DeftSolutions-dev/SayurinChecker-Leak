using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using BazaChecker.Models;
using BazaChecker.Services;

namespace BazaChecker;

public partial class App : Application
{
	[Serializable]
	[CompilerGenerated]
	private sealed class Class0
	{
		public static readonly Class0 class0_0 = new Class0();

		public static Func<List<ProgramInfo>> func_0;

		public static Func<List<SteamAccount>> func_1;

		public static Func<List<UsbDeviceInfo>> func_2;

		public static Func<SystemSummary> func_3;

		internal List<ProgramInfo> method_0()
		{
			return RegistryScanner.GetPrograms();
		}

		internal List<SteamAccount> method_1()
		{
			return SteamScanner.GetAccounts();
		}

		internal List<UsbDeviceInfo> method_2()
		{
			return UsbScanner.GetUsbHistory();
		}

		internal SystemSummary method_3()
		{
			return SystemInfoProvider.GetSummary();
		}
	}

	[CompilerGenerated]
	private sealed class Class1
	{
		public App app_0;

		public MainWindow mainWindow_0;

		public Action action_0;

		internal void method_0()
		{
			((DispatcherObject)app_0).Dispatcher.Invoke((Action)delegate
			{
				mainWindow_0 = new MainWindow();
			});
		}

		internal void method_1()
		{
			mainWindow_0 = new MainWindow();
		}
	}

	public App()
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Expected O, but got Unknown
		base.DispatcherUnhandledException += new DispatcherUnhandledExceptionEventHandler(App_DispatcherUnhandledException);
		AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
	}

	protected override async void OnStartup(StartupEventArgs startupEventArgs_0)
	{
		base.OnStartup(startupEventArgs_0);
		SplashScreen splashScreen = new SplashScreen();
		splashScreen.Show();
		await Task.Delay(50);
		splashScreen.UpdateStatus("Инициализация сервисов...");
		splashScreen.UpdateProgress(5.0);
		MainWindow mainWindow_0 = null;
		await Task.Run(delegate
		{
			((DispatcherObject)this).Dispatcher.Invoke((Action)delegate
			{
				mainWindow_0 = new MainWindow();
			});
		});
		splashScreen.UpdateStatus("Сканирование системы...");
		splashScreen.UpdateProgress(20.0);
		Stopwatch stopwatch = Stopwatch.StartNew();
		new List<ProgramInfo>();
		new List<SteamAccount>();
		new List<UsbDeviceInfo>();
		try
		{
			Task<List<ProgramInfo>> task = Task.Run(() => RegistryScanner.GetPrograms());
			Task<List<SteamAccount>> task2 = Task.Run(() => SteamScanner.GetAccounts());
			Task<List<UsbDeviceInfo>> task3 = Task.Run(() => UsbScanner.GetUsbHistory());
			Task<SystemSummary> task4 = Task.Run(() => SystemInfoProvider.GetSummary());
			splashScreen.UpdateStatus("Чтение реестра и данных системы...");
			splashScreen.UpdateProgress(40.0);
			await Task.WhenAll(task, task2, task3, task4);
			List<ProgramInfo> programs = await task;
			List<SteamAccount> list = await task2;
			List<UsbDeviceInfo> usb = await task3;
			SystemSummary system = await task4;
			splashScreen.UpdateStatus("Кэширование данных...");
			splashScreen.UpdateProgress(80.0);
			if (list.Count > 0)
			{
				splashScreen.UpdateStatus($"Проверка {list.Count} аккаунтов Steam...");
				await SteamScanner.EnrichAccountsWithSteamDataAsync(list);
			}
			mainWindow_0.InjectPreloadedData(programs, list, usb, system);
		}
		catch (Exception exception_)
		{
			LogException(exception_, "Startup Data Loading");
		}
		splashScreen.UpdateProgress(90.0);
		if (stopwatch.ElapsedMilliseconds < 1000L)
		{
			await Task.Delay(500);
		}
		stopwatch.Stop();
		splashScreen.UpdateStatus("Готово!");
		splashScreen.UpdateProgress(100.0);
		await Task.Delay(100);
		mainWindow_0.Show();
		mainWindow_0.Activate();
		mainWindow_0.Focus();
		splashScreen.Close();
	}

	private void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
	{
		LogException(e.Exception, "UI Thread Exception");
		e.Handled = true;
		Shutdown();
	}

	private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
	{
		LogException(e.ExceptionObject as Exception, "Background Thread Exception");
	}

	private void LogException(Exception? exception_0, string source)
	{
		if (exception_0 == null)
		{
			return;
		}
		string contents = $"[{DateTime.Now}] CRITICAL ERROR ({source}):\n{exception_0.Message}\n\nStack Trace:\n{exception_0.StackTrace}\n\nExisting Inner Exception:\n{exception_0.InnerException?.Message}\n--------------------------------------------------\n";
		try
		{
			File.AppendAllText("crash_log.txt", contents);
			MessageBox.Show("Application crashed!\n\nError: " + exception_0.Message + "\n\nCheck crash_log.txt for details.", "Critical Error", MessageBoxButton.OK, MessageBoxImage.Hand);
		}
		catch
		{
			MessageBox.Show("Crashed: " + exception_0.Message, "Critical Error", MessageBoxButton.OK, MessageBoxImage.Hand);
		}
	}

}
