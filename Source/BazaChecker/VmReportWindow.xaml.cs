using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;

namespace BazaChecker;

public partial class VmReportWindow : Window
{

	public VmReportWindow(string log)
	{
		InitializeComponent();
		FormatLog(log);
	}

	private void FormatLog(string log)
	{
		Paragraph paragraph = new Paragraph();
		string[] array = log.Split(new string[2] { "\r\n", "\n" }, StringSplitOptions.None);
		foreach (string text in array)
		{
			if (text.Contains("DETECTED"))
			{
				Run item = new Run(text + "\n")
				{
					Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#EF4444")),
					FontWeight = FontWeights.Bold
				};
				paragraph.Inlines.Add(item);
			}
			else if (text.Contains("[!] RESULT"))
			{
				Run item2 = new Run(text + "\n")
				{
					Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FBBF24")),
					FontWeight = FontWeights.Bold
				};
				paragraph.Inlines.Add(item2);
			}
			else if (!text.Contains("[*] RESULT") && !text.Contains("CLEAN"))
			{
				if (!text.StartsWith("[-]") && !text.StartsWith("--"))
				{
					Run item3 = new Run(text + "\n")
					{
						Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#D1D5DB"))
					};
					paragraph.Inlines.Add(item3);
				}
				else
				{
					Run item4 = new Run(text + "\n")
					{
						Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#888888"))
					};
					paragraph.Inlines.Add(item4);
				}
			}
			else
			{
				Run item5 = new Run(text + "\n")
				{
					Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#22C55E")),
					FontWeight = FontWeights.Bold
				};
				paragraph.Inlines.Add(item5);
			}
		}
		ConsoleOutput.Document = new FlowDocument(paragraph);
	}

	private void TitleBar_MouseDown(object sender, MouseButtonEventArgs e)
	{
		if (e.ChangedButton == MouseButton.Left)
		{
			DragMove();
		}
	}

	private void Close_Click(object sender, RoutedEventArgs e)
	{
		Close();
	}


}
