using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using System.Windows.Threading;
using BazaChecker.Models;
using BazaChecker.Services;
using Microsoft.Win32;

namespace BazaChecker;

public partial class MainWindow : Window
{
	private enum Enum0
	{
		const_0,
		const_1,
		const_2,
		const_3
	}

	[Serializable]
	[CompilerGenerated]
	private sealed class Class3
	{
		public static readonly Class3 class3_0 = new Class3();

		public static Action action_0;

		public static Func<DiskInfo, bool> func_0;

		public static Func<List<ProgramInfo>> func_1;

		public static Func<ProgramInfo, bool> func_2;

		public static Func<ProgramInfo, DateTime> func_3;

		public static Func<ProgramInfo, string> func_4;

		public static Func<List<SteamAccount>> func_5;

		public static Func<List<UsbDeviceInfo>> func_6;

		public static Func<List<UsbDeviceInfo>> func_7;

		public static Func<UsbDeviceInfo, bool> func_8;

		public static Func<SystemSummary> func_9;

		internal void method_0()
		{
			try
			{
				GC.Collect(2, GCCollectionMode.Forced, blocking: true, compacting: true);
				GC.WaitForPendingFinalizers();
				using Process process = Process.GetCurrentProcess();
				EmptyWorkingSet(process.Handle);
				SetProcessWorkingSetSize(process.Handle, -1, -1);
			}
			catch
			{
			}
		}

		internal bool method_1(DiskInfo diskInfo_0)
		{
			return diskInfo_0.IsSelected;
		}

		internal List<ProgramInfo> method_2()
		{
			return RegistryScanner.GetPrograms();
		}

		internal bool method_3(ProgramInfo programInfo_0)
		{
			return programInfo_0.IsHistory;
		}

		internal DateTime method_4(ProgramInfo programInfo_0)
		{
			return programInfo_0.Timestamp ?? DateTime.MinValue;
		}

		internal string method_5(ProgramInfo programInfo_0)
		{
			return programInfo_0.DisplayName;
		}

		internal List<SteamAccount> method_6()
		{
			return SteamScanner.GetAccounts();
		}

		internal List<UsbDeviceInfo> method_7()
		{
			return UsbScanner.GetUsbHistory();
		}

		internal List<UsbDeviceInfo> method_8()
		{
			return UsbScanner.GetUsbHistory();
		}

		internal bool method_9(UsbDeviceInfo usbDeviceInfo_0)
		{
			return usbDeviceInfo_0.IsConnected;
		}

		internal SystemSummary method_10()
		{
			return SystemInfoProvider.GetSummary();
		}
	}

	[CompilerGenerated]
	private sealed class Class4
	{
		public string string_0;

		internal bool method_0()
		{
			return AppLauncher.RunTool(string_0);
		}
	}

	[CompilerGenerated]
	private sealed class Class5
	{
		public Process process_0;

		public MainWindow mainWindow_0;

		public System.Windows.Controls.Button button_0;

		internal void method_0()
		{
			try
			{
				ShowWindow(process_0.MainWindowHandle, 9);
				SetForegroundWindow(process_0.MainWindowHandle);
				Thread.Sleep(1000);
				byte[] keys = new byte[12]
				{
					112, 113, 114, 115, 116, 117, 118, 119, 120, 121,
					122, 123
				};
				byte[] keys2 = new byte[6] { 45, 46, 36, 35, 33, 34 };
				byte[] keys3 = new byte[13]
				{
					192, 49, 50, 51, 52, 53, 54, 55, 56, 57,
					48, 189, 187
				};
				byte[] keys4 = new byte[15]
				{
					96, 97, 98, 99, 100, 101, 102, 103, 104, 105,
					106, 107, 109, 110, 111
				};
				mainWindow_0.PressList(process_0.MainWindowHandle, keys);
				mainWindow_0.PressList(process_0.MainWindowHandle, keys2);
				mainWindow_0.PressList(process_0.MainWindowHandle, keys4);
				for (byte b = 65; b <= 90; b++)
				{
					mainWindow_0.PressKeySafe(process_0.MainWindowHandle, b);
				}
				mainWindow_0.PressModifiedList(process_0.MainWindowHandle, 18, keys2);
				byte[] keys5 = new byte[9] { 115, 116, 117, 118, 119, 120, 121, 122, 123 };
				mainWindow_0.PressModifiedList(process_0.MainWindowHandle, 18, keys5);
				mainWindow_0.PressModifiedList(process_0.MainWindowHandle, 16, keys2);
				mainWindow_0.PressModifiedList(process_0.MainWindowHandle, 16, keys);
				List<byte> list = new List<byte>();
				for (byte b2 = 65; b2 <= 90; b2++)
				{
					list.Add(b2);
				}
				mainWindow_0.PressModifiedList(process_0.MainWindowHandle, 16, list.ToArray());
				mainWindow_0.PressModifiedList(process_0.MainWindowHandle, 17, keys2);
				mainWindow_0.PressModifiedList(process_0.MainWindowHandle, 17, keys);
				mainWindow_0.PressList(process_0.MainWindowHandle, keys3);
			}
			catch
			{
			}
		}

		internal void method_1()
		{
			try
			{
				ShowWindow(process_0.MainWindowHandle, 6);
			}
			catch
			{
			}
			if (mainWindow_0.WindowState == WindowState.Minimized)
			{
				mainWindow_0.WindowState = WindowState.Normal;
			}
			mainWindow_0.Activate();
			if (button_0 != null)
			{
				button_0.IsEnabled = true;
				button_0.Content = "\ud83e\udd16 НАЧАТЬ ПРОВЕРКУ";
			}
			mainWindow_0.ShowNotification("Проверка прошла успешно");
		}
	}

	[CompilerGenerated]
	private sealed class Class6
	{
		public Guid guid_0;

		internal bool method_0(NotificationItem notificationItem_0)
		{
			return notificationItem_0.Id == guid_0;
		}
	}

	[CompilerGenerated]
	private sealed class Class7
	{
		public string string_0;

		internal bool method_0(ProgramInfo programInfo_0)
		{
			if (!programInfo_0.DisplayName.ToLowerInvariant().Contains(string_0))
			{
				return programInfo_0.InstallPath.ToLowerInvariant().Contains(string_0);
			}
			return true;
		}
	}

	[CompilerGenerated]
	private sealed class Class8
	{
		public string string_0;

		internal bool method_0(ProgramInfo programInfo_0)
		{
			if (programInfo_0.IsHistory)
			{
				if (!string.IsNullOrEmpty(string_0) && !programInfo_0.DisplayName.ToLowerInvariant().Contains(string_0))
				{
					return programInfo_0.InstallPath.ToLowerInvariant().Contains(string_0);
				}
				return true;
			}
			return false;
		}
	}

	private const int DWMWA_WINDOW_CORNER_PREFERENCE = 33;

	private CancellationTokenSource? _memoryCts;

	private const int SW_RESTORE = 9;

	private const int SW_MINIMIZE = 6;

	private const int KEYEVENTF_KEYUP = 2;

	private List<ProgramInfo> _allPrograms = new List<ProgramInfo>();

	private List<SteamAccount> _accounts = new List<SteamAccount>();

	private List<UsbDeviceInfo> _allUsbDevices = new List<UsbDeviceInfo>();

	private List<UsbDeviceInfo> _usbDevices = new List<UsbDeviceInfo>();

	private List<ProgramInfo>? _preloadedPrograms;

	private List<SteamAccount>? _preloadedAccounts;

	private List<UsbDeviceInfo>? _preloadedUsb;

	private int _registryPageSize = 50;

	private int _registryCurrentPage;

	private SystemSummary? _systemSummary;

	private bool _isSystemInfoLoaded;

	private List<DiskInfo> _disks = new List<DiskInfo>();

	private CancellationTokenSource? _scanCts;

	private bool _isScanning;

	private ObservableCollection<NotificationItem> _notifications = new ObservableCollection<NotificationItem>();

	private DispatcherTimer? _monitorCheckTimer;

	private NotificationItem? _monitorWarning;

	private NotificationItem? _recordingWarning;

	private ICollectionView? _registryView;

	private ICollectionView? _usbView;

	private DispatcherTimer? _searchDebounceTimer;

	private string _pendingSearchType = "";

	private readonly HashSet<string> _prohibitedProcesses = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
	{
		"obs64", "obs32", "obs", "obs-browser-page", "streamlabs obs", "streamlabs obs-browser-page", "NVIDIA Share", "gamebar", "GameBarFTServer", "bdcam",
		"Bandicam", "Fraps", "XSplit.Core", "XSplit.Gamecaster", "Medal", "MedalEncoder", "MedalRecorder", "Action_x64", "Action", "CameraHub",
		"Elgato Camera Hub", "ControlCenter", "RECentral", "RECentral 4", "AVerMedia RECentral", "PrismLiveStudio"
	};

	private FrameworkElement[] _navButtons = Array.Empty<FrameworkElement>();

	private FrameworkElement[] _pages = Array.Empty<FrameworkElement>();

	private FrameworkElement? _currentPage;

	private bool _isChangingPage;

	private string _currentFilter = "Flash";




































































	[DllImport("psapi.dll")]
	private static extern bool EmptyWorkingSet(nint hProcess);

	[DllImport("kernel32.dll")]
	private static extern bool SetProcessWorkingSetSize(nint hProcess, nint dwMinimumWorkingSetSize, nint dwMaximumWorkingSetSize);

	[DllImport("dwmapi.dll")]
	private static extern int DwmSetWindowAttribute(nint hwnd, int attr, ref int attrValue, int attrSize);

	private async void OptimizeMemoryAsync()
	{
		_memoryCts?.Cancel();
		_memoryCts = new CancellationTokenSource();
		CancellationToken token = _memoryCts.Token;
		try
		{
			await Task.Delay(3000, token);
			await Task.Run(delegate
			{
				try
				{
					GC.Collect(2, GCCollectionMode.Forced, blocking: true, compacting: true);
					GC.WaitForPendingFinalizers();
					using Process process = Process.GetCurrentProcess();
					EmptyWorkingSet(process.Handle);
					SetProcessWorkingSetSize(process.Handle, -1, -1);
				}
				catch
				{
				}
			}, token);
		}
		catch (OperationCanceledException)
		{
		}
	}

	private void OptimizeMemory()
	{
		OptimizeMemoryAsync();
	}

	[DllImport("user32.dll")]
	private static extern bool SetForegroundWindow(nint hWnd);

	[DllImport("user32.dll")]
	private static extern bool ShowWindow(nint hWnd, int nCmdShow);

	[DllImport("user32.dll")]
	private static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, int dwExtraInfo);

	private async void ShowNotification(string message, string title = "Успешно", NotificationType type = NotificationType.Success, bool persistent = false)
	{
		NotificationItem notificationItem = new NotificationItem();
		notificationItem.Title = title;
		notificationItem.Message = message;
		NotificationItem notificationItem2 = notificationItem;
		notificationItem2.Icon = type switch
		{
			NotificationType.Warning => "⚠", 
			NotificationType.Error => "❌", 
			_ => "✅", 
		};
		notificationItem.Type = type;
		notificationItem.IsPersistent = persistent;
		NotificationItem item = notificationItem;
		_notifications.Add(item);
		if (persistent)
		{
			return;
		}
		await Task.Delay(3000);
		if (_notifications.Contains(item))
		{
			await AnimateNotificationClose(item);
			if (_notifications.Contains(item))
			{
				_notifications.Remove(item);
			}
		}
	}

	private async Task AnimateNotificationClose(NotificationItem item)
	{
		item.IsClosing = true;
		int millisecondsDelay = 20;
		for (int i = 0; i < 15; i++)
		{
			double num = (double)(i + 1) / 15.0;
			double num2 = 1.0 - (1.0 - num) * (1.0 - num);
			item.OpacityValue = 1.0 - num2;
			await Task.Delay(millisecondsDelay);
		}
	}

	private void Notification_Click(object sender, MouseButtonEventArgs e)
	{
		if (!(sender is FrameworkElement { DataContext: NotificationItem dataContext }) || string.IsNullOrEmpty(dataContext.TargetProcessName))
		{
			return;
		}
		try
		{
			Process[] processesByName = Process.GetProcessesByName(dataContext.TargetProcessName);
			foreach (Process obj in processesByName)
			{
				obj.Kill();
				obj.WaitForExit(1000);
			}
			ShowNotification("Программа " + dataContext.TargetProcessName + " была принудительно закрыта.");
			DismissNotification(dataContext);
		}
		catch (Exception ex)
		{
			ShowNotification("Не удалось закрыть " + dataContext.TargetProcessName + ": " + ex.Message, "Ошибка", NotificationType.Error);
		}
	}

	private async void CloseNotification_Click(object sender, RoutedEventArgs e)
	{
		if (!(sender is System.Windows.Controls.Button { Tag: var tag }) || !(tag is Guid))
		{
			return;
		}
		Guid guid_0 = (Guid)tag;
		NotificationItem notificationItem = _notifications.FirstOrDefault((NotificationItem notificationItem_0) => notificationItem_0.Id == guid_0);
		if (notificationItem != null && !notificationItem.IsClosing)
		{
			await AnimateNotificationClose(notificationItem);
			if (_notifications.Contains(notificationItem))
			{
				_notifications.Remove(notificationItem);
			}
		}
	}

	public MainWindow()
	{
		//IL_022d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0232: Unknown result type (might be due to invalid IL or missing references)
		//IL_0254: Expected O, but got Unknown
		InitializeComponent();
		NotificationStack.ItemsSource = _notifications;
		_currentPage = PagePCCheck;
		base.Opacity = 0.0;
		base.Loaded += MainWindow_Loaded;
		base.Closing += MainWindow_Closing;
		base.PreviewKeyDown += MainWindow_PreviewKeyDown;
		_searchDebounceTimer = new DispatcherTimer
		{
			Interval = TimeSpan.FromMilliseconds(300L)
		};
		_searchDebounceTimer.Tick += SearchDebounceTimer_Tick;
	}

	public void InjectPreloadedData(List<ProgramInfo> programs, List<SteamAccount> accounts, List<UsbDeviceInfo> usb, SystemSummary? system = null)
	{
		_preloadedPrograms = programs;
		_preloadedAccounts = accounts;
		_preloadedUsb = usb;
		if (system != null)
		{
			_systemSummary = system;
			_isSystemInfoLoaded = true;
			UpdateSystemInfoUI();
		}
		_allPrograms = programs;
		_allUsbDevices = usb;
		_accounts = accounts;
		InitializeCollectionView();
		AccountsListView.ItemsSource = _accounts;
	}

	private void InitializeCollectionView()
	{
		_registryView = CollectionViewSource.GetDefaultView(_allPrograms);
		_registryView.Filter = RegistryFilterPredicate;
		RegistryListView.ItemsSource = (IEnumerable)_registryView;
		_usbView = CollectionViewSource.GetDefaultView(_allUsbDevices);
		_usbView.Filter = UsbFilterPredicate;
		UsbItemsControl.ItemsSource = (IEnumerable)_usbView;
	}

	private bool RegistryFilterPredicate(object obj)
	{
		if (!(obj is ProgramInfo programInfo))
		{
			return false;
		}
		bool valueOrDefault = FilterInstalled.IsChecked == true;
		bool valueOrDefault2 = FilterDeleted.IsChecked == true;
		bool valueOrDefault3 = FilterHistory.IsChecked == true;
		string text = SearchBox.Text;
		object obj2;
		if (text == null)
		{
			obj2 = null;
		}
		else
		{
			obj2 = text.ToLowerInvariant();
			if (obj2 != null)
			{
				goto IL_006b;
			}
		}
		obj2 = "";
		goto IL_006b;
		IL_006b:
		string value = (string)obj2;
		if (!string.IsNullOrEmpty(value) && !programInfo.DisplayName.ToLowerInvariant().Contains(value) && !programInfo.InstallPath.ToLowerInvariant().Contains(value))
		{
			return false;
		}
		if (valueOrDefault3)
		{
			if (!programInfo.IsHistory)
			{
				return false;
			}
			return true;
		}
		if (valueOrDefault && !programInfo.IsHistory && programInfo.Status == ProgramStatus.Installed)
		{
			return true;
		}
		if (valueOrDefault2 && !programInfo.IsHistory && programInfo.Status == ProgramStatus.Deleted)
		{
			return true;
		}
		return false;
	}

	private bool UsbFilterPredicate(object obj)
	{
		if (!(obj is UsbDeviceInfo usbDeviceInfo))
		{
			return false;
		}
		string text = UsbSearchBox.Text;
		object obj2;
		if (text == null)
		{
			obj2 = null;
		}
		else
		{
			obj2 = text.Trim().ToLower();
			if (obj2 != null)
			{
				goto IL_0031;
			}
		}
		obj2 = "";
		goto IL_0031;
		IL_0031:
		string value = (string)obj2;
		if (!string.IsNullOrEmpty(value) && !usbDeviceInfo.Name.ToLower().Contains(value) && !usbDeviceInfo.SerialNumber.ToLower().Contains(value) && !usbDeviceInfo.VID.ToLower().Contains(value))
		{
			return false;
		}
		if (_currentFilter == "Flash")
		{
			string text2 = usbDeviceInfo.Name.ToLower();
			string text3 = usbDeviceInfo.Description.ToLower();
			bool num = text2.Contains("flash") || text2.Contains("disk") || text2.Contains("mass storage") || text3.Contains("storage");
			bool flag = text2.Contains("mtp") || text2.Contains("mobile") || text2.Contains("android") || text2.Contains("iphone") || text2.Contains("samsung") || text2.Contains("xiaomi") || text2.Contains("redmi") || text2.Contains("poco") || text2.Contains("pixel");
			if (!num && !flag)
			{
				return false;
			}
		}
		else if (_currentFilter == "Keyboard")
		{
			string text4 = usbDeviceInfo.Name.ToLower();
			string text5 = usbDeviceInfo.Description.ToLower();
			if ((text4.Contains("flash") || text4.Contains("disk") || text4.Contains("mass storage") || text5.Contains("storage")) | (text4.Contains("mtp") || text4.Contains("mobile") || text4.Contains("android") || text4.Contains("iphone") || text4.Contains("samsung") || text4.Contains("xiaomi") || text4.Contains("redmi") || text4.Contains("poco") || text4.Contains("pixel")))
			{
				return false;
			}
		}
		return true;
	}

	private void SearchDebounceTimer_Tick(object? sender, EventArgs e)
	{
		DispatcherTimer? searchDebounceTimer = _searchDebounceTimer;
		if (searchDebounceTimer != null)
		{
			searchDebounceTimer.Stop();
		}
		if (_pendingSearchType == "Registry")
		{
			_registryCurrentPage = 0;
			ApplyRegistryFilters();
		}
		else if (_pendingSearchType == "USB")
		{
			ICollectionView? usbView = _usbView;
			if (usbView != null)
			{
				usbView.Refresh();
			}
		}
	}

	private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
	{
		if (e.ChangedButton == MouseButton.Left && e.ButtonState == MouseButtonState.Pressed)
		{
			try
			{
				DragMove();
			}
			catch (InvalidOperationException)
			{
			}
		}
	}

	private void MainWindow_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Invalid comparison between Unknown and I4
		if ((int)e.Key != 2 || !(Mouse.DirectlyOver is FrameworkElement frameworkElement))
		{
			return;
		}
		FrameworkElement frameworkElement2 = frameworkElement;
		NotificationItem notificationItem;
		while (true)
		{
			if (frameworkElement2 != null)
			{
				notificationItem = frameworkElement2.DataContext as NotificationItem;
				if (notificationItem != null)
				{
					break;
				}
				frameworkElement2 = VisualTreeHelper.GetParent((DependencyObject)(object)frameworkElement2) as FrameworkElement;
				continue;
			}
			return;
		}
		DismissNotification(notificationItem);
		e.Handled = true;
	}

	private async void DismissNotification(NotificationItem item)
	{
		if (!item.IsClosing)
		{
			await AnimateNotificationClose(item);
			if (_notifications.Contains(item))
			{
				_notifications.Remove(item);
			}
			if (item == _monitorWarning)
			{
				_monitorWarning = null;
			}
			if (item == _recordingWarning)
			{
				_recordingWarning = null;
			}
		}
	}

	private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
	{
		await Task.CompletedTask;
		double primaryScreenWidth = SystemParameters.PrimaryScreenWidth;
		double primaryScreenHeight = SystemParameters.PrimaryScreenHeight;
		if (primaryScreenWidth < 1400.0 || primaryScreenHeight < 830.0)
		{
			base.Width = Math.Min(1400.0, primaryScreenWidth - 50.0);
			base.Height = Math.Min(830.0, primaryScreenHeight - 80.0);
			base.Left = (primaryScreenWidth - base.Width) / 2.0;
			base.Top = (primaryScreenHeight - base.Height) / 2.0;
		}
		DoubleAnimation animation = new DoubleAnimation
		{
			From = 0.0,
			To = 1.0,
			Duration = TimeSpan.FromMilliseconds(300L),
			EasingFunction = new QuadraticEase
			{
				EasingMode = EasingMode.EaseOut
			}
		};
		BeginAnimation(UIElement.OpacityProperty, animation);
		try
		{
			_navButtons = new FrameworkElement[9] { NavPCCheck, NavRegistry, NavAccounts, NavUSB, NavPrograms, NavFolders, NavSites, NavAdditional, NavExtra };
			_pages = new FrameworkElement[9] { PagePCCheck, PageRegistry, PageAccounts, PageUSB, PagePrograms, PageFolders, PageSites, PageAdditional, PageExtra };
			CheckAdminStatus();
			AppLauncher.ExtractEmbeddedTools();
			LoadDisks();
			if (!_isSystemInfoLoaded)
			{
				_isSystemInfoLoaded = true;
				LoadSystemInfoAsync();
			}
			DiscordService.Initialize("1463981045555789825");
			_monitorCheckTimer = new DispatcherTimer();
			_monitorCheckTimer.Interval = TimeSpan.FromMinutes(1L);
			_monitorCheckTimer.Tick += delegate
			{
				CheckMonitorStatus();
				CheckRecordingStatus();
			};
			_monitorCheckTimer.Start();
			CheckMonitorStatus();
			CheckRecordingStatus();
			OptimizeMemory();
		}
		catch (Exception ex)
		{
			System.Windows.MessageBox.Show("Startup Error: " + ex.Message + "\nStack: " + ex.StackTrace, "Error", MessageBoxButton.OK, MessageBoxImage.Hand);
		}
	}

	private void MainWindow_Closing(object? sender, CancelEventArgs e)
	{
		DiscordService.Deinitialize();
	}

	private void LoadDisks()
	{
		_disks = DiskScanner.GetAvailableDisks();
		DiskList.ItemsSource = _disks;
	}

	private void CheckAdminStatus()
	{
		bool flag = false;
		try
		{
			using WindowsIdentity ntIdentity = WindowsIdentity.GetCurrent();
			flag = new WindowsPrincipal(ntIdentity).IsInRole(WindowsBuiltInRole.Administrator);
		}
		catch
		{
		}
		if (flag)
		{
			AdminStatus.Text = "✓ ADMIN MODE";
			AdminStatus.Foreground = (Brush)FindResource("SuccessBrush");
		}
		else
		{
			AdminStatus.Text = "⚠ LIMITED MODE";
			AdminStatus.Foreground = (Brush)FindResource("DangerBrush");
		}
	}

	private void CheckMonitorStatus()
	{
		try
		{
			bool flag;
			if ((flag = Screen.AllScreens.Length > 1) && _monitorWarning == null)
			{
				AddMonitorWarning();
			}
			else if (!flag && _monitorWarning != null)
			{
				DismissNotification(_monitorWarning);
			}
		}
		catch
		{
		}
	}

	private void AddMonitorWarning()
	{
		_monitorWarning = new NotificationItem
		{
			Title = "Второй монитор!",
			Message = "Обнаружен активный второй экран. Пожалуйста, выключите режим 'Расширить' для чистоты проверки.",
			Type = NotificationType.Warning,
			Icon = "⚠",
			IsPersistent = true
		};
		_notifications.Add(_monitorWarning);
	}

	private void CheckRecordingStatus()
	{
		try
		{
			string text = null;
			foreach (string prohibitedProcess in _prohibitedProcesses)
			{
				Process[] processesByName = Process.GetProcessesByName(prohibitedProcess);
				if (processesByName.Length != 0)
				{
					text = prohibitedProcess;
					Process[] array = processesByName;
					for (int i = 0; i < array.Length; i++)
					{
						array[i].Dispose();
					}
					break;
				}
			}
			if (text != null)
			{
				if (_recordingWarning == null)
				{
					AddRecordingWarning(text);
				}
			}
			else if (_recordingWarning != null)
			{
				DismissNotification(_recordingWarning);
			}
		}
		catch
		{
		}
	}

	private void AddRecordingWarning(string processName)
	{
		_recordingWarning = new NotificationItem
		{
			Title = "Идет запись экрана!",
			Message = "Обнаружен процесс записи: " + processName + ". Нажмите сюда, чтобы закрыть программу.",
			Type = NotificationType.Error,
			Icon = "\ud83d\udd34",
			IsPersistent = true,
			TargetProcessName = processName
		};
		_notifications.Add(_recordingWarning);
	}

	private void TitleBar_MouseDown(object sender, MouseButtonEventArgs e)
	{
		if (e.LeftButton == MouseButtonState.Pressed)
		{
			DragMove();
		}
	}

	private void Minimize_Click(object sender, RoutedEventArgs e)
	{
		base.WindowState = WindowState.Minimized;
	}

	private void Maximize_Click(object sender, RoutedEventArgs e)
	{
		if (base.WindowState == WindowState.Maximized)
		{
			base.WindowState = WindowState.Normal;
		}
		else
		{
			base.WindowState = WindowState.Maximized;
		}
	}

	private void Close_Click(object sender, RoutedEventArgs e)
	{
		Close();
	}

	private void Window_StateChanged(object sender, EventArgs e)
	{
		if (base.WindowState == WindowState.Minimized)
		{
			DispatcherTimer? monitorCheckTimer = _monitorCheckTimer;
			if (monitorCheckTimer != null)
			{
				monitorCheckTimer.Stop();
			}
			OptimizeMemory();
			return;
		}
		DispatcherTimer? monitorCheckTimer2 = _monitorCheckTimer;
		if (monitorCheckTimer2 != null)
		{
			monitorCheckTimer2.Start();
		}
		CheckMonitorStatus();
		CheckRecordingStatus();
	}

	private async Task SwitchPageAsync(FrameworkElement newPage)
	{
		if (_currentPage != newPage && !_isChangingPage)
		{
			_isChangingPage = true;
			if (_currentPage != null)
			{
				_currentPage.Visibility = Visibility.Collapsed;
			}
			_currentPage = newPage;
			_currentPage.Opacity = 1.0;
			_currentPage.Visibility = Visibility.Visible;
			if (_currentPage.RenderTransform is TranslateTransform translateTransform)
			{
				translateTransform.Y = 0.0;
			}
			_isChangingPage = false;
			await Task.CompletedTask;
		}
	}

	private async void Nav_Click(object sender, RoutedEventArgs e)
	{
		if (!(sender is System.Windows.Controls.Button { Tag: var tag } button))
		{
			return;
		}
		string text = tag?.ToString();
		if (!string.IsNullOrEmpty(text))
		{
			Style style = (Style)FindResource("NavButtonStyle");
			Style style2 = (Style)FindResource("NavButtonActiveStyle");
			FrameworkElement[] navButtons = _navButtons;
			for (int i = 0; i < navButtons.Length; i++)
			{
				navButtons[i].Style = style;
			}
			button.Style = style2;
			await SwitchPageAsync(text switch
			{
				"USB" => PageUSB, 
				"Sites" => PageSites, 
				"Extra" => PageExtra, 
				"PCCheck" => PagePCCheck, 
				"Folders" => PageFolders, 
				"Registry" => PageRegistry, 
				"Programs" => PagePrograms, 
				"Accounts" => PageAccounts, 
				"Additional" => PageAdditional, 
				_ => PagePCCheck, 
			});
			if (text == "Extra" || (text == "PCCheck" && !_isSystemInfoLoaded))
			{
				_isSystemInfoLoaded = true;
				LoadSystemInfoAsync();
			}
			DiscordService.UpdatePresence(text switch
			{
				"USB" => "\ud83d\udd0c История USB", 
				"Sites" => "\ud83c\udf10 Полезные ссылки", 
				"Extra" => "\ud83d\udda5\ufe0f Система", 
				"PCCheck" => "\ud83d\udcd1 Проверка на читы", 
				"Folders" => "\ud83d\udcc2 Проверка папок", 
				"Registry" => "\ud83d\udd0d Сканирование реестра", 
				"Programs" => "\ud83d\udee0\ufe0f Инструменты", 
				"Accounts" => "\ud83d\udc64 Просмотр аккаунтов", 
				"Additional" => "✨ Дополнительно", 
				_ => "\ud83d\udcd1 Проверка на читы", 
			});
			if (text == "Registry" && _allPrograms.Count == 0)
			{
				await LoadRegistryAsync();
			}
			if (text == "Accounts" && _accounts.Count == 0)
			{
				await LoadAccountsAsync();
			}
			if (text == "USB")
			{
				LoadUsbAsync();
			}
		}
	}

	private void ShowDiskSelection_Click(object sender, RoutedEventArgs e)
	{
		LoadDisks();
		DiskSelectionModal.Visibility = Visibility.Visible;
	}

	private void CloseModal_Click(object sender, RoutedEventArgs e)
	{
		DiskSelectionModal.Visibility = Visibility.Collapsed;
	}

	private void ResetScan_Click(object sender, RoutedEventArgs e)
	{
		ResultsView.Visibility = Visibility.Collapsed;
		ScanInitialView.Visibility = Visibility.Visible;
		ThreatsListView.ItemsSource = null;
		ThreatCountText.Text = "0";
		ProcessCountText.Text = "0 files";
		EmptyState.Visibility = Visibility.Visible;
	}

	private async void StartScan_Click(object sender, RoutedEventArgs e)
	{
		if (_isScanning)
		{
			CancelScan();
			return;
		}
		List<DiskInfo> list = _disks.Where((DiskInfo diskInfo_0) => diskInfo_0.IsSelected).ToList();
		if (list.Count == 0)
		{
			System.Windows.MessageBox.Show("Please select at least one drive to scan.", "No Drives Selected", MessageBoxButton.OK, MessageBoxImage.Exclamation);
			return;
		}
		_isScanning = true;
		_scanCts = new CancellationTokenSource();
		DiskSelectionModal.Visibility = Visibility.Collapsed;
		ScanInitialView.Visibility = Visibility.Collapsed;
		ResultsView.Visibility = Visibility.Visible;
		ProgressPanel.Visibility = Visibility.Visible;
		EmptyState.Visibility = Visibility.Collapsed;
		ScanningIndicator.Visibility = Visibility.Visible;
		ThreatsListView.ItemsSource = null;
		ThreatCountText.Text = "0";
		ProcessCountText.Text = "Scanning...";
		ScanStatusText.Text = "Initializing scan...";
		Progress<string> progress = new Progress<string>(delegate(string status)
		{
			ProgressText.Text = status;
			ScanStatusText.Text = status;
		});
		try
		{
			DiscordService.UpdatePresence("\ud83d\udd0e Идёт сканирование ПК...", "\ud83d\udccb Проверка на читы");
			bool valueOrDefault = CheckIconScan.IsChecked == true;
			bool valueOrDefault2 = CheckSignatureScan.IsChecked == true;
			DiskScanResult diskScanResult = await DiskScanner.ScanDisksAsync(list, valueOrDefault, valueOrDefault2, progress, _scanCts.Token);
			ThreatsListView.ItemsSource = diskScanResult.Threats;
			ThreatCountText.Text = diskScanResult.Threats.Count.ToString();
			ProcessCountText.Text = $"{diskScanResult.TotalFilesScanned:N0} files";
			if (diskScanResult.WasCancelled)
			{
				ScanStatusText.Text = "Scan cancelled";
				ScanningIndicator.Visibility = Visibility.Collapsed;
			}
			else if (diskScanResult.Threats.Count > 0)
			{
				ScanStatusText.Text = $"Found {diskScanResult.Threats.Count} threats in {diskScanResult.ScanDuration.TotalSeconds:F1}s";
				EmptyState.Visibility = Visibility.Collapsed;
				ScanningIndicator.Visibility = Visibility.Collapsed;
			}
			else
			{
				ScanStatusText.Text = $"Clean! Scanned in {diskScanResult.ScanDuration.TotalSeconds:F1}s";
				ScanningIndicator.Visibility = Visibility.Collapsed;
				EmptyState.Visibility = Visibility.Visible;
			}
			DiscordService.UpdatePresence("✅ Сканирование завершено", "\ud83d\udccb Проверка на читы");
		}
		catch (OperationCanceledException)
		{
			ScanStatusText.Text = "Scan cancelled";
			ScanningIndicator.Visibility = Visibility.Collapsed;
		}
		catch (Exception ex2)
		{
			ScanStatusText.Text = "Error: " + ex2.Message;
			System.Windows.MessageBox.Show("Scan error: " + ex2.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Hand);
		}
		finally
		{
			_isScanning = false;
			_scanCts?.Dispose();
			_scanCts = null;
			ProgressPanel.Visibility = Visibility.Collapsed;
		}
	}

	private void CancelScan_Click(object sender, RoutedEventArgs e)
	{
		CancelScan();
	}

	private void CancelScan()
	{
		_scanCts?.Cancel();
		ScanStatusText.Text = "Cancelling...";
	}

	private void ThreatsListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
	{
		if (!(ThreatsListView.SelectedItem is ThreatInfo threatInfo))
		{
			return;
		}
		try
		{
			string path = threatInfo.Path;
			if (!(threatInfo.Type == "Process") && !path.StartsWith("Process:"))
			{
				if (File.Exists(path))
				{
					Process.Start("explorer.exe", "/select,\"" + path + "\"");
				}
				else if (Directory.Exists(path))
				{
					Process.Start("explorer.exe", "\"" + path + "\"");
				}
			}
		}
		catch
		{
		}
	}

	private async Task LoadRegistryAsync()
	{
		if (_preloadedPrograms != null)
		{
			_allPrograms = _preloadedPrograms;
			_preloadedPrograms = null;
		}
		else if (_allPrograms.Count == 0)
		{
			await RefreshRegistryAsyncInternal();
		}
		if (_registryView == null)
		{
			InitializeCollectionView();
		}
		ApplyRegistryFilters();
		UpdateRegistryCounts();
	}

	private async Task RefreshRegistryAsyncInternal()
	{
		List<ProgramInfo> obj = await Task.Run(() => RegistryScanner.GetPrograms());
		RegistryListView.ItemsSource = null;
		_allPrograms.Clear();
		foreach (ProgramInfo item in obj)
		{
			_allPrograms.Add(item);
		}
		if (_registryView == null)
		{
			InitializeCollectionView();
		}
		RegistryListView.ItemsSource = (IEnumerable)_registryView;
		ApplyRegistryFilters();
		UpdateRegistryCounts();
		OptimizeMemory();
	}

	private async void RefreshRegistry_Click(object sender, RoutedEventArgs e)
	{
		await RefreshRegistryAsyncInternal();
		ShowNotification((string)FindResource("Notif_RegistryUpdated"), (string)FindResource("Notif_Success"));
	}

	private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
	{
		DispatcherTimer? searchDebounceTimer = _searchDebounceTimer;
		if (searchDebounceTimer != null)
		{
			searchDebounceTimer.Stop();
		}
		_pendingSearchType = "Registry";
		DispatcherTimer? searchDebounceTimer2 = _searchDebounceTimer;
		if (searchDebounceTimer2 != null)
		{
			searchDebounceTimer2.Start();
		}
	}

	private void Filter_Click(object sender, RoutedEventArgs e)
	{
		if (sender is ToggleButton { IsChecked: var isChecked } toggleButton)
		{
			if (isChecked == true)
			{
				FilterInstalled.IsChecked = toggleButton == FilterInstalled;
				FilterDeleted.IsChecked = toggleButton == FilterDeleted;
				FilterHistory.IsChecked = toggleButton == FilterHistory;
			}
			else if (FilterInstalled.IsChecked == false && FilterDeleted.IsChecked == false && FilterHistory.IsChecked == false)
			{
				FilterInstalled.IsChecked = true;
			}
		}
		_registryCurrentPage = 0;
		ApplyRegistryFilters();
		UpdateRegistryCounts();
	}

	private void ApplyRegistryFilters()
	{
		if (_allPrograms == null || _registryView == null)
		{
			return;
		}
		bool valueOrDefault = FilterHistory.IsChecked == true;
		RegistryPaginationPanel.Visibility = ((!valueOrDefault) ? Visibility.Collapsed : Visibility.Visible);
		object obj;
		if (valueOrDefault)
		{
			string text = SearchBox.Text;
			if (text == null)
			{
				obj = null;
			}
			else
			{
				obj = text.ToLowerInvariant();
				if (obj != null)
				{
					goto IL_0069;
				}
			}
			obj = "";
			goto IL_0069;
		}
		RegistryListView.ItemsSource = (IEnumerable)_registryView;
		_registryView.Refresh();
		if (RegistryListView.Items.Count > 0)
		{
			RegistryListView.ScrollIntoView(RegistryListView.Items[0]);
		}
		return;
		IL_0069:
		string string_0 = (string)obj;
		List<ProgramInfo> source = _allPrograms.Where((ProgramInfo programInfo_0) => programInfo_0.IsHistory).ToList();
		if (!string.IsNullOrEmpty(string_0))
		{
			source = source.Where((ProgramInfo programInfo_0) => programInfo_0.DisplayName.ToLowerInvariant().Contains(string_0) || programInfo_0.InstallPath.ToLowerInvariant().Contains(string_0)).ToList();
		}
		source = (from programInfo_0 in source
			orderby programInfo_0.Timestamp ?? DateTime.MinValue descending, programInfo_0.DisplayName
			select programInfo_0).ToList();
		int count = source.Count;
		int num = Math.Max(0, (count - 1) / _registryPageSize);
		if (_registryCurrentPage > num)
		{
			_registryCurrentPage = num;
		}
		if (_registryCurrentPage < 0)
		{
			_registryCurrentPage = 0;
		}
		List<ProgramInfo> list = source.Skip(_registryCurrentPage * _registryPageSize).Take(_registryPageSize).ToList();
		RegistryPageText.Text = $"Страница {_registryCurrentPage + 1} из {num + 1}";
		RegistryListView.ItemsSource = list;
		if (list.Count > 0)
		{
			RegistryListView.ScrollIntoView(list[0]);
		}
	}

	private void PrevPage_Click(object sender, RoutedEventArgs e)
	{
		if (_registryCurrentPage > 0)
		{
			_registryCurrentPage--;
			ApplyRegistryFilters();
		}
	}

	private void NextPage_Click(object sender, RoutedEventArgs e)
	{
		string text = SearchBox.Text;
		object obj;
		if (text == null)
		{
			obj = null;
		}
		else
		{
			obj = text.ToLowerInvariant();
			if (obj != null)
			{
				goto IL_0027;
			}
		}
		obj = "";
		goto IL_0027;
		IL_0027:
		string string_0 = (string)obj;
		int num = (_allPrograms.Count((ProgramInfo programInfo_0) => programInfo_0.IsHistory && (string.IsNullOrEmpty(string_0) || programInfo_0.DisplayName.ToLowerInvariant().Contains(string_0) || programInfo_0.InstallPath.ToLowerInvariant().Contains(string_0))) - 1) / _registryPageSize;
		if (_registryCurrentPage < num)
		{
			_registryCurrentPage++;
			ApplyRegistryFilters();
		}
	}

	private void UpdateRegistryCounts()
	{
		if (_allPrograms == null || _allPrograms.Count == 0)
		{
			return;
		}
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		foreach (ProgramInfo allProgram in _allPrograms)
		{
			if (allProgram.IsHistory)
			{
				num++;
			}
			else if (allProgram.Status == ProgramStatus.Installed)
			{
				num2++;
			}
			else if (allProgram.Status == ProgramStatus.Deleted)
			{
				num3++;
			}
		}
		FilterInstalled.Content = $"{(string)FindResource("Filter_Installed")} ({num2})";
		FilterDeleted.Content = $"{(string)FindResource("Filter_Deleted")} ({num3})";
		FilterHistory.Content = $"{(string)FindResource("Filter_Traces")} ({num})";
	}

	private void Buffer_Click(object sender, RoutedEventArgs e)
	{
		if (sender is System.Windows.Controls.Button { Tag: not null } button)
		{
			System.Windows.Clipboard.SetText(button.Tag.ToString());
			ShowNotification($"{button.Content} скопирован!", "Буфер обмена");
		}
	}

	private void OpenRegistry_Click(object sender, RoutedEventArgs e)
	{
		if (sender is System.Windows.Controls.Button { Tag: not null } button)
		{
			string text = button.Tag.ToString();
			if (!string.IsNullOrEmpty(text))
			{
				OpenRegistryKey(text);
			}
		}
	}

	private void OpenFolder_Click(object sender, RoutedEventArgs e)
	{
		Folder_Click(sender, e);
	}

	private void OpenRegistryKey(string path)
	{
		try
		{
			path = path.Replace("HKCU", "HKEY_CURRENT_USER").Replace("HKLM", "HKEY_LOCAL_MACHINE");
			if (path.StartsWith("Computer\\"))
			{
				path = path.Substring(9);
			}
			if (path.StartsWith("Компьютер\\"))
			{
				path = path.Substring(10);
			}
			path = "Компьютер\\" + path;
			using (RegistryKey registryKey = Registry.CurrentUser.CreateSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Applets\\Regedit"))
			{
				registryKey?.SetValue("LastKey", path);
			}
			Process[] processesByName = Process.GetProcessesByName("regedit");
			for (int i = 0; i < processesByName.Length; i++)
			{
				processesByName[i].Kill();
			}
			Process.Start("regedit.exe");
		}
		catch (Exception ex)
		{
			ShowNotification("Ошибка открытия реестра: " + ex.Message, "Ошибка", NotificationType.Error);
		}
	}

	private async Task LoadAccountsAsync()
	{
		if (_preloadedAccounts != null)
		{
			_accounts = _preloadedAccounts;
			_preloadedAccounts = null;
		}
		else
		{
			if (_accounts.Count > 0)
			{
				AccountsListView.ItemsSource = _accounts;
				return;
			}
			List<SteamAccount> obj = await Task.Run(() => SteamScanner.GetAccounts());
			AccountsListView.ItemsSource = null;
			_accounts.Clear();
			foreach (SteamAccount item in obj)
			{
				_accounts.Add(item);
			}
			AccountsListView.ItemsSource = _accounts;
		}
		if (_accounts.Count == 0)
		{
			System.Windows.MessageBox.Show("Steam аккаунты не найдены.\n\nУбедитесь, что Steam установлен.", "Нет аккаунтов", MessageBoxButton.OK, MessageBoxImage.Asterisk);
			return;
		}
		await SteamScanner.EnrichAccountsWithSteamDataAsync(_accounts);
		AccountsListView.ItemsSource = null;
		AccountsListView.ItemsSource = _accounts;
	}

	private async void RefreshAccounts_Click(object sender, RoutedEventArgs e)
	{
		_accounts.Clear();
		await LoadAccountsAsync();
	}

	private void AccountCard_Click(object sender, MouseButtonEventArgs e)
	{
		if (sender is FrameworkElement { DataContext: SteamAccount dataContext })
		{
			dataContext.IsExpanded = !dataContext.IsExpanded;
		}
	}

	private void CopySteamID_Click(object sender, MouseButtonEventArgs e)
	{
		if (sender is FrameworkElement { Tag: string tag })
		{
			try
			{
				System.Windows.Clipboard.SetText(tag);
				ShowNotification("SteamID скопирован!");
			}
			catch
			{
			}
		}
		e.Handled = true;
	}

	private void OpenSteamProfile_Click(object sender, RoutedEventArgs e)
	{
		if (sender is FrameworkElement { Tag: string tag })
		{
			try
			{
				Process.Start(new ProcessStartInfo
				{
					FileName = "https://steamcommunity.com/profiles/" + tag,
					UseShellExecute = true
				});
			}
			catch (Exception ex)
			{
				ShowNotification("Ошибка: " + ex.Message, "Ошибка", NotificationType.Error);
			}
		}
		e.Handled = true;
	}

	private void ClearAccounts_Click(object sender, RoutedEventArgs e)
	{
		_accounts.Clear();
		AccountsListView.ItemsSource = null;
	}

	private async void LoadUsbAsync()
	{
		if (_allUsbDevices.Count > 0 && _preloadedUsb == null)
		{
			return;
		}
		if (_preloadedUsb != null)
		{
			_allUsbDevices = _preloadedUsb;
			_preloadedUsb = null;
		}
		else
		{
			List<UsbDeviceInfo> obj = await Task.Run(() => UsbScanner.GetUsbHistory());
			UsbItemsControl.ItemsSource = null;
			_allUsbDevices.Clear();
			foreach (UsbDeviceInfo item in obj)
			{
				_allUsbDevices.Add(item);
			}
			if (_usbView == null)
			{
				InitializeCollectionView();
			}
			UsbItemsControl.ItemsSource = (IEnumerable)_usbView;
		}
		UpdateUsbCounter();
		OptimizeMemory();
	}

	private async void RefreshUSB_Click(object sender, RoutedEventArgs e)
	{
		List<UsbDeviceInfo> obj = await Task.Run(() => UsbScanner.GetUsbHistory());
		UsbItemsControl.ItemsSource = null;
		_allUsbDevices.Clear();
		foreach (UsbDeviceInfo item in obj)
		{
			_allUsbDevices.Add(item);
		}
		if (_usbView == null)
		{
			InitializeCollectionView();
		}
		UsbItemsControl.ItemsSource = (IEnumerable)_usbView;
		ICollectionView? usbView = _usbView;
		if (usbView != null)
		{
			usbView.Refresh();
		}
		UpdateUsbCounter();
		int value = _allUsbDevices.Count((UsbDeviceInfo usbDeviceInfo_0) => usbDeviceInfo_0.IsConnected);
		ShowNotification($"Список USB устройств обновлен. Подключено: {value}");
	}

	private void UsbSearchBox_TextChanged(object sender, TextChangedEventArgs e)
	{
		DispatcherTimer? searchDebounceTimer = _searchDebounceTimer;
		if (searchDebounceTimer != null)
		{
			searchDebounceTimer.Stop();
		}
		_pendingSearchType = "USB";
		DispatcherTimer? searchDebounceTimer2 = _searchDebounceTimer;
		if (searchDebounceTimer2 != null)
		{
			searchDebounceTimer2.Start();
		}
	}

	private void UsbFilter_All_Click(object sender, MouseButtonEventArgs e)
	{
		SetFilter("All");
	}

	private void UsbFilter_Flash_Click(object sender, MouseButtonEventArgs e)
	{
		SetFilter("Flash");
	}

	private void UsbFilter_Keyboard_Click(object sender, MouseButtonEventArgs e)
	{
		SetFilter("Keyboard");
	}

	private void SetFilter(string filter)
	{
		_currentFilter = filter;
		UpdateFilterVisuals();
		if (_usbView == null)
		{
			InitializeCollectionView();
		}
		ICollectionView? usbView = _usbView;
		if (usbView != null)
		{
			usbView.Refresh();
		}
	}

	private void UpdateFilterVisuals()
	{
		UsbFilterAll.Tag = ((_currentFilter == "All") ? "Active" : "Inactive");
		UsbFilterFlash.Tag = ((_currentFilter == "Flash") ? "Active" : "Inactive");
		UsbFilterKeyboard.Tag = ((_currentFilter == "Keyboard") ? "Active" : "Inactive");
	}

	private void UpdateUsbCounter()
	{
		if (UsbDeviceCounter != null)
		{
			int num = _allUsbDevices.Count(IsUsbCarrier);
			UsbDeviceCounter.Text = num.ToString();
		}
	}

	private bool IsUsbCarrier(UsbDeviceInfo usbDeviceInfo_0)
	{
		string text = usbDeviceInfo_0.Name.ToLower();
		string text2 = (usbDeviceInfo_0.Description ?? "").ToLower();
		return (text.Contains("flash") || text.Contains("disk") || text.Contains("mass storage") || text2.Contains("storage")) | (text.Contains("mtp") || text.Contains("mobile") || text.Contains("android") || text.Contains("iphone") || text.Contains("samsung") || text.Contains("xiaomi") || text.Contains("redmi") || text.Contains("poco") || text.Contains("pixel"));
	}

	private async void Tool_Click(object sender, RoutedEventArgs e)
	{
		if (!(sender is System.Windows.Controls.Button button))
		{
			return;
		}
		string string_0 = button.Tag?.ToString();
		if (!string.IsNullOrEmpty(string_0))
		{
			await Task.Run(() => AppLauncher.RunTool(string_0));
		}
	}

	private void Support_Click(object sender, RoutedEventArgs e)
	{
		try
		{
			Process.Start(new ProcessStartInfo
			{
				FileName = "https://baza-cs2.ru/tickets/",
				UseShellExecute = true
			});
		}
		catch (Exception ex)
		{
			ShowNotification("Ошибка: " + ex.Message, "Ошибка", NotificationType.Error);
		}
	}

	private void Folder_Click(object sender, RoutedEventArgs e)
	{
		if (!(sender is System.Windows.Controls.Button { Tag: string tag }) || tag == null)
		{
			return;
		}
		switch (tag.Length)
		{
		case 4:
			if (tag == "Temp")
			{
				FolderLauncher.OpenTemp();
			}
			break;
		case 5:
			if (tag == "Users")
			{
				FolderLauncher.OpenUsers();
			}
			break;
		case 6:
			if (tag == "Recent")
			{
				FolderLauncher.OpenRecent();
			}
			break;
		case 7:
			if (tag == "AppData")
			{
				FolderLauncher.OpenAppData();
			}
			break;
		case 8:
			if (tag == "Prefetch")
			{
				FolderLauncher.OpenPrefetch();
			}
			break;
		case 9:
			switch (tag[2])
			{
			case 'w':
				if (tag == "Downloads")
				{
					FolderLauncher.OpenDownloads();
				}
				break;
			case 'c':
				if (tag == "Documents")
				{
					FolderLauncher.OpenDocuments();
				}
				break;
			}
			break;
		case 10:
			if (tag == "CrashDumps")
			{
				FolderLauncher.OpenCrashDumps();
			}
			break;
		case 11:
			if (tag == "ProgramData")
			{
				FolderLauncher.OpenProgramData();
			}
			break;
		case 12:
			if (tag == "ProgramFiles")
			{
				FolderLauncher.OpenProgramFiles();
			}
			break;
		case 13:
			if (tag == "ReportArchive")
			{
				FolderLauncher.OpenReportArchive();
			}
			break;
		}
	}

	private void Site_Click(object sender, RoutedEventArgs e)
	{
		if (sender is System.Windows.Controls.Button { Tag: var tag })
		{
			string text = tag?.ToString();
			if (!string.IsNullOrEmpty(text))
			{
				SiteLauncher.OpenUrl(text);
			}
		}
	}

	private async Task LoadSystemInfoAsync()
	{
		_systemSummary = await Task.Run(() => SystemInfoProvider.GetSummary());
		UpdateSystemInfoUI();
	}

	private void UpdateSystemInfoUI()
	{
		if (_systemSummary != null)
		{
			OSText.Text = _systemSummary.OSName;
			InstallDateText.Text = _systemSummary.InstallDateText;
			ScreenCountText.Text = _systemSummary.MonitorCount.ToString();
			LastBootText.Text = _systemSummary.LastBootText;
			RAMText.Text = _systemSummary.RAM;
			SSDText.Text = SanitizeHardwareInfo(_systemSummary.SSD, isDisk: true);
			CPUText.Text = $"{SanitizeHardwareInfo(_systemSummary.CPU)} ({_systemSummary.CPUCores}/{_systemSummary.CPUThreads})";
			GPUText.Text = SanitizeHardwareInfo(_systemSummary.GPU) + " (" + _systemSummary.GPUVRAM + ")";
			MotherboardText.Text = SanitizeHardwareInfo(_systemSummary.Motherboard);
			MachineNameText.Text = _systemSummary.MachineName;
			UserNameText.Text = _systemSummary.UserName;
			VMText.Text = _systemSummary.IsVirtualMachineText;
		}
	}

	private string SanitizeHardwareInfo(string? input, bool isDisk = false)
	{
		if (string.IsNullOrEmpty(input))
		{
			return "Unknown";
		}
		string input2 = input.Replace("(R)", "").Replace("(TM)", "").Replace("®", "")
			.Replace("™", "");
		input2 = Regex.Replace(input2, "geforce", "", RegexOptions.IgnoreCase);
		if (isDisk)
		{
			input2 = input2.Replace("NVMe", "").Replace("SCSI", "").Replace("Disk", "")
				.Replace("Device", "")
				.Trim();
			string[] array = input2.Split(new char[3] { ' ', '-', '_' }, StringSplitOptions.RemoveEmptyEntries);
			string text = ((array.Length != 0) ? array[0] : "Unknown");
			Match match = Regex.Match(input, "\\((\\d+)\\s*GB\\)", RegexOptions.IgnoreCase);
			if (match.Success)
			{
				int num = int.Parse(match.Groups[1].Value);
				if (num > 440 && num < 512)
				{
					num = 512;
				}
				else if (num >= 900 && num <= 1024)
				{
					num = 1024;
				}
				else if (num > 220 && num < 256)
				{
					num = 256;
				}
				else if (num > 110 && num < 128)
				{
					num = 120;
				}
				return $"{text} {num} GB";
			}
			return text + " (Unknown Size)";
		}
		input2 = Regex.Replace(input2, "\\b[A-Z0-9]{8,}\\b", "");
		if (input2.Length > 35)
		{
			string[] array2 = input2.Split(new char[1] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
			if (array2.Length > 4)
			{
				return string.Join(" ", array2.Take(4));
			}
		}
		return Regex.Replace(input2, "\\s+", " ").Trim();
	}

	private async void RefreshSystemInfo_Click(object sender, RoutedEventArgs e)
	{
		_systemSummary = null;
		await LoadSystemInfoAsync();
	}

	private void OpenNvidia_Click(object sender, RoutedEventArgs e)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Expected O, but got Unknown
		bool flag = false;
		try
		{
			ManagementObjectSearcher val = new ManagementObjectSearcher("SELECT Name FROM Win32_VideoController");
			try
			{
				var enumerator = val.Get().GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						object obj = enumerator.Current["Name"];
						object obj2;
						if (obj == null)
						{
							obj2 = null;
						}
						else
						{
							obj2 = obj.ToString();
							if (obj2 != null)
							{
								goto IL_0041;
							}
						}
						obj2 = "";
						goto IL_0041;
						IL_0041:
						if (((string)obj2).Contains("NVIDIA", StringComparison.OrdinalIgnoreCase))
						{
							flag = true;
							break;
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
		catch
		{
		}
		if (!flag)
		{
			ShowNotification("У вас видеокарта не от NVIDIA", "Ошибка", NotificationType.Error);
			return;
		}
		string[] array = new string[4] { "shell:AppsFolder\\NVIDIACorp.NVIDIAControlPanel_56jybvy8sckqj!NVIDIACorp.NVIDIAControlPanel", "C:\\Windows\\System32\\nvcplui.exe", "C:\\Program Files\\NVIDIA Corporation\\Control Panel Client\\nvcplui.exe", "C:\\Program Files (x86)\\NVIDIA Corporation\\Control Panel Client\\nvcplui.exe" };
		foreach (string text in array)
		{
			try
			{
				Process.Start(new ProcessStartInfo
				{
					FileName = (text.StartsWith("shell:") ? "explorer.exe" : text),
					Arguments = (text.StartsWith("shell:") ? text : ""),
					UseShellExecute = !text.StartsWith("shell:")
				});
				return;
			}
			catch
			{
			}
		}
		ShowNotification("NVIDIA Control Panel не установлен", "Ошибка", NotificationType.Error);
	}

	private void OpenNetwork_Click(object sender, RoutedEventArgs e)
	{
		try
		{
			Process.Start(new ProcessStartInfo
			{
				FileName = "ms-settings:network",
				UseShellExecute = true
			});
		}
		catch (Exception ex)
		{
			System.Windows.MessageBox.Show("Ошибка: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Hand);
		}
	}

	private void OpenServices_Click(object sender, RoutedEventArgs e)
	{
		try
		{
			Process.Start(new ProcessStartInfo
			{
				FileName = "services.msc",
				UseShellExecute = true
			});
		}
		catch (Exception ex)
		{
			System.Windows.MessageBox.Show("Ошибка: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Hand);
		}
	}

	private void OpenMacroCheck_Click(object sender, RoutedEventArgs e)
	{
		MacroCheckWindow macroCheckWindow = new MacroCheckWindow();
		macroCheckWindow.Owner = this;
		macroCheckWindow.ShowDialog();
	}

	private void BtnOpenOSK_Click(object sender, RoutedEventArgs e)
	{
		try
		{
			Process.Start(new ProcessStartInfo
			{
				FileName = "osk",
				UseShellExecute = true
			});
		}
		catch
		{
			try
			{
				string text = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "Sysnative", "osk.exe");
				if (File.Exists(text))
				{
					Process.Start(new ProcessStartInfo
					{
						FileName = text,
						UseShellExecute = true
					});
				}
				else
				{
					string fileName = Path.Combine(Environment.SystemDirectory, "osk.exe");
					Process.Start(new ProcessStartInfo
					{
						FileName = fileName,
						UseShellExecute = true
					});
				}
			}
			catch (Exception ex)
			{
				System.Windows.MessageBox.Show("Не удалось запустить экранную клавиатуру:\n" + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Hand);
			}
		}
	}

	private void OpenMouseSoftware_Click(object sender, RoutedEventArgs e)
	{
		(string, string[])[] array = new(string, string[])[7]
		{
			("Bloody", new string[4] { "C:\\Program Files (x86)\\Bloody7\\Bloody7\\Bloody7.exe", "C:\\Program Files\\Bloody7\\Bloody7\\Bloody7.exe", "C:\\Program Files (x86)\\Bloody\\Bloody.exe", "C:\\Program Files\\A4Tech\\Bloody\\Bloody.exe" }),
			("Logitech G Hub", new string[4] { "C:\\Program Files\\LGHUB\\lghub.exe", "C:\\Program Files (x86)\\LGHUB\\lghub.exe", "C:\\Program Files\\Logitech Gaming Software\\LCore.exe", "C:\\Program Files (x86)\\Logitech Gaming Software\\LCore.exe" }),
			("Razer Synapse", new string[7] { "C:\\Program Files (x86)\\Razer\\Synapse3\\WPFUI\\Framework\\Razer Synapse 3 Host\\Razer Synapse 3.exe", "C:\\Program Files\\Razer\\Synapse3\\WPFUI\\Framework\\Razer Synapse 3 Host\\Razer Synapse 3.exe", "C:\\Program Files (x86)\\Razer\\Razer Services\\Razer Central\\Razer Central.exe", "C:\\Program Files\\Razer\\Razer Services\\Razer Central\\Razer Central.exe", "C:\\Program Files (x86)\\Razer\\Synapse\\RzSynapse.exe", "C:\\Program Files\\Razer\\Synapse\\RzSynapse.exe", "C:\\Program Files (x86)\\Razer\\Razer Central\\Razer Central.exe" }),
			("SteelSeries Engine", new string[4] { "C:\\Program Files\\SteelSeries\\SteelSeries Engine 3\\SteelSeriesEngine3.exe", "C:\\Program Files (x86)\\SteelSeries\\SteelSeries Engine 3\\SteelSeriesEngine3.exe", "C:\\Program Files\\SteelSeries\\GG\\SteelSeriesGG.exe", "C:\\Program Files (x86)\\SteelSeries\\GG\\SteelSeriesGG.exe" }),
			("Corsair iCUE", new string[3] { "C:\\Program Files\\Corsair\\CORSAIR iCUE 4 Software\\iCUE.exe", "C:\\Program Files (x86)\\Corsair\\CORSAIR iCUE Software\\iCUE.exe", "C:\\Program Files\\Corsair\\CORSAIR iCUE 5 Software\\iCUE.exe" }),
			("HyperX NGENUITY", new string[1] { "C:\\Program Files\\HyperX NGENUITY\\HyperX NGENUITY.exe" }),
			("Roccat Swarm", new string[2] { "C:\\Program Files (x86)\\ROCCAT\\Swarm\\ROCCAT_Swarm.exe", "C:\\Program Files\\ROCCAT\\Swarm\\ROCCAT_Swarm.exe" })
		};
		for (int i = 0; i < array.Length; i++)
		{
			string[] item = array[i].Item2;
			foreach (string text in item)
			{
				if (File.Exists(text))
				{
					try
					{
						Process.Start(new ProcessStartInfo(text)
						{
							UseShellExecute = true
						});
						return;
					}
					catch
					{
					}
				}
			}
		}
		ShowNotification("ПО для управления мышью не найдено", "Ошибка", NotificationType.Error);
	}

	private void BtnVmReport_Click(object sender, RoutedEventArgs e)
	{
		VmReportWindow vmReportWindow = new VmReportWindow(VmDetector.GetDetectionLog());
		vmReportWindow.Owner = this;
		vmReportWindow.ShowDialog();
	}

	private async void BtnStartBruteForce_Click(object sender, RoutedEventArgs e)
	{
		System.Windows.Controls.Button button_0 = sender as System.Windows.Controls.Button;
		Process process_0 = Process.GetProcessesByName("cs2").FirstOrDefault();
		if (process_0 == null)
		{
			ShowNotification("CS2 не запущен! Запустите игру перед проверкой.", "Ошибка", NotificationType.Error);
			return;
		}
		if (button_0 != null)
		{
			button_0.IsEnabled = false;
			button_0.Content = "⏳ ИДЕТ ПРОВЕРКА...";
		}
		await Task.Run(delegate
		{
			try
			{
				ShowWindow(process_0.MainWindowHandle, 9);
				SetForegroundWindow(process_0.MainWindowHandle);
				Thread.Sleep(1000);
				byte[] keys = new byte[12]
				{
					112, 113, 114, 115, 116, 117, 118, 119, 120, 121,
					122, 123
				};
				byte[] keys2 = new byte[6] { 45, 46, 36, 35, 33, 34 };
				byte[] keys3 = new byte[13]
				{
					192, 49, 50, 51, 52, 53, 54, 55, 56, 57,
					48, 189, 187
				};
				byte[] keys4 = new byte[15]
				{
					96, 97, 98, 99, 100, 101, 102, 103, 104, 105,
					106, 107, 109, 110, 111
				};
				PressList(process_0.MainWindowHandle, keys);
				PressList(process_0.MainWindowHandle, keys2);
				PressList(process_0.MainWindowHandle, keys4);
				for (byte b = 65; b <= 90; b++)
				{
					PressKeySafe(process_0.MainWindowHandle, b);
				}
				PressModifiedList(process_0.MainWindowHandle, 18, keys2);
				byte[] keys5 = new byte[9] { 115, 116, 117, 118, 119, 120, 121, 122, 123 };
				PressModifiedList(process_0.MainWindowHandle, 18, keys5);
				PressModifiedList(process_0.MainWindowHandle, 16, keys2);
				PressModifiedList(process_0.MainWindowHandle, 16, keys);
				List<byte> list = new List<byte>();
				for (byte b2 = 65; b2 <= 90; b2++)
				{
					list.Add(b2);
				}
				PressModifiedList(process_0.MainWindowHandle, 16, list.ToArray());
				PressModifiedList(process_0.MainWindowHandle, 17, keys2);
				PressModifiedList(process_0.MainWindowHandle, 17, keys);
				PressList(process_0.MainWindowHandle, keys3);
			}
			catch
			{
			}
		});
		((DispatcherObject)this).Dispatcher.Invoke((Action)delegate
		{
			try
			{
				ShowWindow(process_0.MainWindowHandle, 6);
			}
			catch
			{
			}
			if (base.WindowState == WindowState.Minimized)
			{
				base.WindowState = WindowState.Normal;
			}
			Activate();
			if (button_0 != null)
			{
				button_0.IsEnabled = true;
				button_0.Content = "\ud83e\udd16 НАЧАТЬ ПРОВЕРКУ";
			}
			ShowNotification("Проверка прошла успешно");
		});
	}

	private void PressList(nint hWnd, byte[] keys)
	{
		foreach (byte key in keys)
		{
			PressKeySafe(hWnd, key);
		}
	}

	private void PressModifiedList(nint hWnd, byte modifier, byte[] keys)
	{
		foreach (byte b in keys)
		{
			if (b != 91 && b != 92 && modifier != 91 && modifier != 92)
			{
				SetForegroundWindow(hWnd);
				keybd_event(modifier, 0, 0u, 0);
				Thread.Sleep(50);
				keybd_event(b, 0, 0u, 0);
				Thread.Sleep(50);
				keybd_event(b, 0, 2u, 0);
				Thread.Sleep(50);
				keybd_event(modifier, 0, 2u, 0);
				Thread.Sleep(150);
			}
		}
	}

	private void PressKeySafe(nint hWnd, byte key)
	{
		if (key != 91 && key != 92)
		{
			SetForegroundWindow(hWnd);
			keybd_event(key, 0, 0u, 0);
			Thread.Sleep(50);
			keybd_event(key, 0, 2u, 0);
			Thread.Sleep(150);
		}
	}

	private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
	{
		try
		{
			Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri)
			{
				UseShellExecute = true
			});
			e.Handled = true;
		}
		catch
		{
		}
	}

	private void NazeNick_Click(object sender, MouseButtonEventArgs e)
	{
		Process.Start(new ProcessStartInfo("https://discord.com/users/877811080867491880")
		{
			UseShellExecute = true
		});
	}

	private void BazaLink_Click(object sender, MouseButtonEventArgs e)
	{
		Process.Start(new ProcessStartInfo("https://baza-cs2.ru")
		{
			UseShellExecute = true
		});
	}

	private void Banner_Click(object sender, MouseButtonEventArgs e)
	{
		Process.Start(new ProcessStartInfo("https://baza-cs2.ru/")
		{
			UseShellExecute = true
		});
	}

	private void GitHub_Click(object sender, RoutedEventArgs e)
	{
		Process.Start(new ProcessStartInfo("https://github.com/Mujqk/SayurinOwnChecker")
		{
			UseShellExecute = true
		});
	}

	private void GitPages_Click(object sender, RoutedEventArgs e)
	{
		Process.Start(new ProcessStartInfo("https://mujqk.github.io/SayurinOwnChecker/")
		{
			UseShellExecute = true
		});
	}



}
