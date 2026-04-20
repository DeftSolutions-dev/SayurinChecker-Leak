using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Media;

namespace BazaChecker.Models;

public class ProgramInfo : INotifyPropertyChanged
{
	public string DisplayName { get; set; } = string.Empty;

	public string InstallPath { get; set; } = string.Empty;

	public string Publisher { get; set; } = string.Empty;

	public string InstallDate { get; set; } = string.Empty;

	public DateTime? Timestamp { get; set; }

	public ProgramStatus Status { get; set; }

	public bool IsHistory { get; set; }

	public string StatusText => Status switch
	{
		ProgramStatus.Installed => "INSTALLED", 
		ProgramStatus.Deleted => "DELETED", 
		ProgramStatus.Trace => "TRACE", 
		_ => "UNKNOWN", 
	};

	public SolidColorBrush StatusColor => Status switch
	{
		ProgramStatus.Installed => new SolidColorBrush(Color.FromRgb(34, 197, 94)), 
		ProgramStatus.Deleted => new SolidColorBrush(Color.FromRgb(239, 68, 68)), 
		ProgramStatus.Trace => new SolidColorBrush(Color.FromRgb(249, 115, 22)), 
		_ => Brushes.Gray, 
	};

	public SolidColorBrush StatusBackground => Status switch
	{
		ProgramStatus.Installed => new SolidColorBrush(Color.FromArgb(38, 34, 197, 94)), 
		ProgramStatus.Deleted => new SolidColorBrush(Color.FromArgb(38, 239, 68, 68)), 
		ProgramStatus.Trace => new SolidColorBrush(Color.FromArgb(38, 249, 115, 22)), 
		_ => new SolidColorBrush(Color.FromArgb(38, 128, 128, 128)), 
	};

	public event PropertyChangedEventHandler? PropertyChanged;

	protected void OnPropertyChanged(string propertyName)
	{
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
