namespace BazaChecker.Models;

public class BazaBanEntry
{
	public string Type { get; set; } = "Unknown";

	public string Date { get; set; } = "";

	public string Reason { get; set; } = "";

	public string Admin { get; set; } = "";

	public string Duration { get; set; } = "";

	public string Status { get; set; } = "";

	public string IconData { get; set; } = "";

	public string IconColor { get; set; } = "#888888";

	public bool IsImage { get; set; }
}
