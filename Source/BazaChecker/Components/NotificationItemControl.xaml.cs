using System;
using System.Windows;
using System.Windows.Controls;

namespace BazaChecker.Components;

public partial class NotificationItemControl : UserControl
{
	public event EventHandler<int>? CloseRequested;

	public NotificationItemControl()
	{
		InitializeComponent();
	}

	private void Close_Click(object sender, RoutedEventArgs e)
	{
		if (sender is Button { Tag: int id })
		{
			CloseRequested?.Invoke(this, id);
			return;
		}
		if (sender is Button { Tag: var tag } && int.TryParse(tag?.ToString(), out var parsed))
		{
			CloseRequested?.Invoke(this, parsed);
		}
	}
}
