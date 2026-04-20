namespace BazaChecker.Models;

public class ToolInfo
{
	public string Name { get; set; } = string.Empty;

	public string FileName { get; set; } = string.Empty;

	public string Icon { get; set; } = "\ud83d\udd27";

	public bool IsAvailable { get; set; }
}
