using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace BazaChecker;

public partial class MacroCheckWindow : Window
{
	[CompilerGenerated]
	private sealed class Class2
	{
		public Process process_0;

		public MacroCheckWindow macroCheckWindow_0;

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
				macroCheckWindow_0.PressList(process_0.MainWindowHandle, keys);
				macroCheckWindow_0.PressList(process_0.MainWindowHandle, keys2);
				macroCheckWindow_0.PressList(process_0.MainWindowHandle, keys4);
				for (byte b = 65; b <= 90; b++)
				{
					macroCheckWindow_0.PressKeySafe(process_0.MainWindowHandle, b);
				}
				macroCheckWindow_0.PressModifiedList(process_0.MainWindowHandle, 18, keys2);
				macroCheckWindow_0.PressModifiedList(process_0.MainWindowHandle, 18, keys);
				macroCheckWindow_0.PressModifiedList(process_0.MainWindowHandle, 16, keys2);
				macroCheckWindow_0.PressModifiedList(process_0.MainWindowHandle, 16, keys);
				macroCheckWindow_0.PressModifiedList(process_0.MainWindowHandle, 17, keys2);
				macroCheckWindow_0.PressModifiedList(process_0.MainWindowHandle, 17, keys);
				macroCheckWindow_0.PressList(process_0.MainWindowHandle, keys3);
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
			macroCheckWindow_0.Show();
			macroCheckWindow_0.WindowState = WindowState.Normal;
			macroCheckWindow_0.Topmost = true;
			macroCheckWindow_0.Topmost = false;
			macroCheckWindow_0.Activate();
			macroCheckWindow_0.BtnConfirmStart.IsEnabled = true;
			macroCheckWindow_0.BtnConfirmStart.Content = "Начать проверку \ud83d\ude80";
			macroCheckWindow_0.PreCheckOverlay.Visibility = Visibility.Collapsed;
			MessageBox.Show("Проверка завершена!\nЕсли меню не открылось — биндов нет.", "Результат", MessageBoxButton.OK, MessageBoxImage.Asterisk);
		}
	}

	private bool _isDrawing;

	private Point _lastPoint;

	private Brush _currentBrush = Brushes.White;

	private const int SW_RESTORE = 9;

	private const int SW_MINIMIZE = 6;

	private const int KEYEVENTF_KEYUP = 2;




	[DllImport("user32.dll")]
	private static extern bool SetForegroundWindow(nint hWnd);

	[DllImport("user32.dll")]
	private static extern bool ShowWindow(nint hWnd, int nCmdShow);

	[DllImport("user32.dll")]
	private static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, int dwExtraInfo);

	public MacroCheckWindow(bool autoStartCheck = false)
	{
		InitializeComponent();
		if (autoStartCheck)
		{
			PreCheckOverlay.Visibility = Visibility.Visible;
		}
	}

	private void TitleBar_MouseDown(object sender, MouseButtonEventArgs e)
	{
		if (e.LeftButton == MouseButtonState.Pressed)
		{
			DragMove();
		}
	}

	private void Close_Click(object sender, RoutedEventArgs e)
	{
		Close();
	}

	private void MacroCanvas_MouseDown(object sender, MouseButtonEventArgs e)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		_isDrawing = true;
		_lastPoint = e.GetPosition(MacroCanvas);
		MacroCanvas.CaptureMouse();
	}

	private void MacroCanvas_MouseMove(object sender, MouseEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		if (_isDrawing)
		{
			Point position = e.GetPosition(MacroCanvas);
			Line element = new Line
			{
				X1 = _lastPoint.X,
				Y1 = _lastPoint.Y,
				X2 = position.X,
				Y2 = position.Y,
				Stroke = _currentBrush,
				StrokeThickness = 2.0,
				StrokeStartLineCap = PenLineCap.Round,
				StrokeEndLineCap = PenLineCap.Round
			};
			MacroCanvas.Children.Add(element);
			_lastPoint = position;
		}
	}

	private void MacroCanvas_MouseUp(object sender, MouseButtonEventArgs e)
	{
		_isDrawing = false;
		MacroCanvas.ReleaseMouseCapture();
	}

	private void Color_Click(object sender, MouseButtonEventArgs e)
	{
		if (sender is Border { Tag: string tag })
		{
			try
			{
				_currentBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString(tag));
			}
			catch
			{
			}
		}
	}

	private void Clear_Click(object sender, RoutedEventArgs e)
	{
		MacroCanvas.Children.Clear();
	}

	private void BtnShowPreCheck_Click(object sender, RoutedEventArgs e)
	{
		PreCheckOverlay.Visibility = Visibility.Visible;
	}

	private void BtnCancelPreCheck_Click(object sender, RoutedEventArgs e)
	{
		PreCheckOverlay.Visibility = Visibility.Collapsed;
	}

	private void BtnOpenOSK_Click(object sender, RoutedEventArgs e)
	{
		try
		{
			string text = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonProgramFiles), "Microsoft Shared\\ink\\TabTip.exe");
			if (File.Exists(text))
			{
				Process.Start(text);
			}
			else
			{
				Process.Start(System.IO.Path.Combine(Environment.SystemDirectory, "osk.exe"));
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show("Не удалось запустить экранную клавиатуру:\n" + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Hand);
		}
	}

	private async void BtnStartBruteForce_Click(object sender, RoutedEventArgs e)
	{
		Process process_0 = Process.GetProcessesByName("cs2").FirstOrDefault();
		if (process_0 == null)
		{
			MessageBox.Show("CS2 не запущен! Запустите игру перед проверкой.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Hand);
			return;
		}
		BtnConfirmStart.IsEnabled = false;
		BtnConfirmStart.Content = "ИДЕТ ПРОВЕРКА...";
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
				PressModifiedList(process_0.MainWindowHandle, 18, keys);
				PressModifiedList(process_0.MainWindowHandle, 16, keys2);
				PressModifiedList(process_0.MainWindowHandle, 16, keys);
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
			Show();
			base.WindowState = WindowState.Normal;
			base.Topmost = true;
			base.Topmost = false;
			Activate();
			BtnConfirmStart.IsEnabled = true;
			BtnConfirmStart.Content = "Начать проверку \ud83d\ude80";
			PreCheckOverlay.Visibility = Visibility.Collapsed;
			MessageBox.Show("Проверка завершена!\nЕсли меню не открылось — биндов нет.", "Результат", MessageBoxButton.OK, MessageBoxImage.Asterisk);
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


}
