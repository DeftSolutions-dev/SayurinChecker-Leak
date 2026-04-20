using System;
using System.Windows.Media;

namespace BazaChecker.Models;

public class BanEntry
{
	public string Type { get; set; } = "VAC";

	public DateTime Date { get; set; }

	public string Game { get; set; } = "Unknown";

	public int DaysAgo => (DateTime.Now - Date).Days;

	public SolidColorBrush TypeColor
	{
		get
		{
			string type = Type;
			if (!(type == "VAC"))
			{
				if (!(type == "Game Ban"))
				{
					return new SolidColorBrush(Color.FromRgb(107, 114, 128));
				}
				return new SolidColorBrush(Color.FromRgb(249, 115, 22));
			}
			return new SolidColorBrush(Color.FromRgb(239, 68, 68));
		}
	}
}
