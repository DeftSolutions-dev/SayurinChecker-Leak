using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;

namespace BazaChecker;

public partial class SplashScreen : Window
{









	public SplashScreen()
	{
		InitializeComponent();
	}

	public void UpdateStatus(string status)
	{
		TxtStatus.Text = status;
	}

	public void UpdateProgress(double percent)
	{
		double width = percent / 100.0 * 360.0;
		ProgressFill.Width = width;
		if (ProgressGlow != null)
		{
			ProgressGlow.Width = width;
		}
	}


}
