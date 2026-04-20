using System.Windows.Media;

namespace BazaChecker.Models;

public class UsbDeviceInfo
{
	public string Name { get; set; } = string.Empty;

	public string Description { get; set; } = string.Empty;

	public string DriveLetter { get; set; } = string.Empty;

	public string InstallDate { get; set; } = string.Empty;

	public string LastConnected { get; set; } = string.Empty;

	public string DisconnectDate { get; set; } = string.Empty;

	public string VID { get; set; } = string.Empty;

	public string PID { get; set; } = string.Empty;

	public string SerialNumber { get; set; } = string.Empty;

	public int DeviceIndex { get; set; }

	public bool IsConnected { get; set; }

	public SolidColorBrush StatusColor
	{
		get
		{
			if (!IsConnected)
			{
				return new SolidColorBrush(Color.FromRgb(100, 100, 110));
			}
			return new SolidColorBrush(Color.FromRgb(0, byte.MaxValue, 102));
		}
	}

	public SolidColorBrush StatusBackground
	{
		get
		{
			if (!IsConnected)
			{
				return new SolidColorBrush(Color.FromRgb(168, 85, 247));
			}
			return new SolidColorBrush(Color.FromRgb(34, 197, 94));
		}
	}

	public string StatusText
	{
		get
		{
			if (!IsConnected)
			{
				return "Отключено";
			}
			return "Подключено";
		}
	}

	public SolidColorBrush CardBorderBrush => new SolidColorBrush(Color.FromRgb(168, 85, 247));
}
