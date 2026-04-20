using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace BazaChecker;

public partial class ConfirmationWindow : Window
{

	public ConfirmationWindow(string message)
	{
		InitializeComponent();
		TxtMessage.Text = message;
	}

	private void Yes_Click(object sender, RoutedEventArgs e)
	{
		base.DialogResult = true;
		Close();
	}

	private void No_Click(object sender, RoutedEventArgs e)
	{
		base.DialogResult = false;
		Close();
	}


}
