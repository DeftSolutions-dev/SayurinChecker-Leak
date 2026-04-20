using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Media;

namespace BazaChecker.Models;

public class SteamAccount : INotifyPropertyChanged
{
	private bool _isBazaLoading;

	private string _bazaAdminStatus = "Unknown";

	private string _bazaBanInfo = "Not checked";

	private List<BazaBanEntry> _bazaBans = new List<BazaBanEntry>();

	private List<BazaBanEntry> _bazaMutes = new List<BazaBanEntry>();

	private bool _isExpanded;

	public string SteamId64 { get; set; } = string.Empty;

	public string AccountName { get; set; } = string.Empty;

	public string PersonaName { get; set; } = string.Empty;

	public string AvatarUrl { get; set; } = "https://avatars.steamstatic.com/fef49e7fa7e1997310d705b2a6158ff8dc1cdfeb_medium.jpg";

	public bool IsVacBanned { get; set; }

	public bool IsActive { get; set; }

	public long LastLogin { get; set; }

	public int NumberOfVACBans { get; set; }

	public int NumberOfGameBans { get; set; }

	public int DaysSinceLastBan { get; set; }

	public bool CommunityBanned { get; set; }

	public bool EconomyBan { get; set; }

	public List<BanEntry> BanHistory { get; set; } = new List<BanEntry>();

	public bool IsBazaLoading
	{
		get
		{
			return _isBazaLoading;
		}
		set
		{
			_isBazaLoading = value;
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsBazaLoading"));
		}
	}

	public string BazaAdminStatus
	{
		get
		{
			return _bazaAdminStatus;
		}
		set
		{
			_bazaAdminStatus = value;
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("BazaAdminStatus"));
		}
	}

	public string BazaBanInfo
	{
		get
		{
			return _bazaBanInfo;
		}
		set
		{
			_bazaBanInfo = value;
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("BazaBanInfo"));
		}
	}

	public List<BazaBanEntry> BazaBans
	{
		get
		{
			return _bazaBans;
		}
		set
		{
			_bazaBans = value;
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("BazaBans"));
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("HasBans"));
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("BazaDetailVisibility"));
		}
	}

	public List<BazaBanEntry> BazaMutes
	{
		get
		{
			return _bazaMutes;
		}
		set
		{
			_bazaMutes = value;
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("BazaMutes"));
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("HasMutes"));
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("BazaDetailVisibility"));
		}
	}

	public bool HasBans
	{
		get
		{
			if (BazaBans != null)
			{
				return BazaBans.Count > 0;
			}
			return false;
		}
	}

	public bool HasMutes
	{
		get
		{
			if (BazaMutes != null)
			{
				return BazaMutes.Count > 0;
			}
			return false;
		}
	}

	public Visibility BazaDetailVisibility
	{
		get
		{
			if (!HasBans && !HasMutes)
			{
				return Visibility.Collapsed;
			}
			return Visibility.Visible;
		}
	}

	public bool IsExpanded
	{
		get
		{
			return _isExpanded;
		}
		set
		{
			_isExpanded = value;
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsExpanded"));
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("BanPanelVisibility"));
		}
	}

	public string VacStatusText
	{
		get
		{
			if (!IsVacBanned)
			{
				return "✓ NO VAC";
			}
			return "VAC BAN";
		}
	}

	public string ActiveStatusText
	{
		get
		{
			if (!IsActive)
			{
				return "Неактивен";
			}
			return "Активен";
		}
	}

	public SolidColorBrush VacColor
	{
		get
		{
			if (!IsVacBanned)
			{
				return new SolidColorBrush(Color.FromRgb(34, 197, 94));
			}
			return new SolidColorBrush(Color.FromRgb(239, 68, 68));
		}
	}

	public SolidColorBrush ActiveColor
	{
		get
		{
			if (!IsActive)
			{
				return new SolidColorBrush(Color.FromRgb(107, 114, 128));
			}
			return new SolidColorBrush(Color.FromRgb(168, 85, 247));
		}
	}

	public Visibility BanPanelVisibility
	{
		get
		{
			if (!IsExpanded)
			{
				return Visibility.Collapsed;
			}
			return Visibility.Visible;
		}
	}

	public string BanSummary
	{
		get
		{
			if (NumberOfVACBans == 0 && NumberOfGameBans == 0)
			{
				return "Нет банов";
			}
			List<string> list = new List<string>();
			if (NumberOfVACBans > 0)
			{
				list.Add($"{NumberOfVACBans} VAC бан(ов)");
			}
			if (NumberOfGameBans > 0)
			{
				list.Add($"{NumberOfGameBans} игровых бан(ов)");
			}
			if (DaysSinceLastBan > 0)
			{
				list.Add($"{DaysSinceLastBan} дней назад");
			}
			return string.Join(" | ", list);
		}
	}

	public event PropertyChangedEventHandler? PropertyChanged;
}
