using System;
using System.ComponentModel;

namespace BazaChecker.Models;

public class NotificationItem : INotifyPropertyChanged
{
	private bool _isClosing;

	private double _opacityValue = 1.0;

	private double _slideX;

	public Guid Id { get; set; }

	public string Title { get; set; }

	public string Message { get; set; }

	public string Icon { get; set; }

	public NotificationType Type { get; set; }

	public bool IsPersistent { get; set; }

	public string? TargetProcessName { get; set; }

	public bool IsClosing
	{
		get
		{
			return _isClosing;
		}
		set
		{
			_isClosing = value;
			OnPropertyChanged("IsClosing");
		}
	}

	public double OpacityValue
	{
		get
		{
			return _opacityValue;
		}
		set
		{
			_opacityValue = value;
			OnPropertyChanged("OpacityValue");
		}
	}

	public double SlideX
	{
		get
		{
			return _slideX;
		}
		set
		{
			_slideX = value;
			OnPropertyChanged("SlideX");
		}
	}

	public event PropertyChangedEventHandler? PropertyChanged;

	protected virtual void OnPropertyChanged(string propertyName)
	{
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}

	public NotificationItem()
	{
		Id = Guid.NewGuid();
		Title = string.Empty;
		Message = string.Empty;
		Icon = "✅";
	}
}
