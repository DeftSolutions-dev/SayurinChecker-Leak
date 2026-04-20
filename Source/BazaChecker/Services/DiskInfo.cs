namespace BazaChecker.Services;

public class DiskInfo
{
	public string DriveLetter { get; set; } = "";

	public string Label { get; set; } = "";

	public string DisplayName
	{
		get
		{
			if (!string.IsNullOrEmpty(Label))
			{
				return Label + " (" + DriveLetter + ")";
			}
			return DriveLetter;
		}
	}

	public long TotalSize { get; set; }

	public long FreeSpace { get; set; }

	public bool IsSelected { get; set; } = true;

	public string SizeText => $"{FreeSpace / 1073741824L:N0} GB free of {TotalSize / 1073741824L:N0} GB";
}
