namespace BazaChecker.Services;

public class ThreatInfo
{
	public string Name { get; set; } = "";

	public string Path { get; set; } = "";

	public string Type { get; set; } = "";

	public string DisplayPath
	{
		get
		{
			if (!string.IsNullOrEmpty(Path) && Path.Length > 80)
			{
				try
				{
					return Path.Substring(0, 30) + "..." + Path.Substring(Path.Length - 45);
				}
				catch
				{
					return Path;
				}
			}
			return Path;
		}
	}

	public string TypeIcon => Type switch
	{
		"Process" => "⚡", 
		"Suspicious File" => "\ud83d\udcc4", 
		"Suspicious Folder" => "\ud83d\udcc1", 
		"Injector" => "\ud83d\udc89", 
		"Cheat" => "\ud83c\udfae", 
		_ => "⚠\ufe0f", 
	};
}
