using System;
using System.Collections.Generic;

namespace BazaChecker.Services;

public static class CheatDatabase
{
	public static readonly HashSet<string> Signatures = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
	{
		"xone", "exloader", "com.swiftsoft", "interium", "mason", "lunocs2", "neverlose", "midnight", "hellshack", "hells-hack",
		"blasthack", "nixware", "en1gma", "enigma", "sharhack", "sharkhack", "exhack", "uwuware", "espd2x", "wallhack",
		"skinchanger", "vredux", "neverkernel", "aquila", "luno", "fecurity", "tkazer", "pellix", "pussycat", "axios",
		"syncware", "onemacro", "softhub", "mvploader", "detorus", "proext", "sapphire", "interwebz", "spirthack", "haxcs",
		"plaguecheat", "vapehook", "smurfwrecker", "iniuia", "inuria", "memesense", "yeahnot", "leaguemode", "legendware", "eghack",
		"hauntedproject", "externalcrack", "rager9", "rager8", "phoenixhack", "obr", "onebyteradar", "ezinjector", "reborn", "onebytewallhack",
		"osiris", "multihack", "breakthrough", "rhcheats", "fatality", "onetap", "ev0lve", "bhop", "bunnyhop", "compkiller",
		"tripit", "rawetrip", "plague", "neoxahack", "fizzy", "expandera", "ekknod", "axion", "doomxtf", "jestkii",
		"wh-satano", "cheatcsgo", "r8cheats", "ezcheats", "cs-elect", "rf-cheats", "anyx", "hackvshack", "ezyhack", "unknowncheats",
		"cheater", "insanitycheats", "elitehacks", "novamacro", "securecheats", "ezcs", "dhjcheats", "nanogon", "extract_merc", "undetek",
		"millionware", "xy0", "aristois", "w1nner", "desync", "ragebot", "legitbot", "freeqn", "reboot", "aimware",
		"force-project", "csghost", "extreme injector", "xenos", "kirin", "processhacker", "otv3", "otv2", "skeet", "fatality.win",
		"onetap.su", "getze.us", "mutiny", "d3cheats", "r3cheats", "hvh", "baim", "doubletap", "hideshots", "process hacker",
		"cheat engine", "x64dbg", "ida pro", "http debugger", ".amc", ".ahk"
	};

	public static readonly HashSet<string> SystemPathsToSkip = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "winsxs", "driverstore", "syswow64", "assembly", "servicing", "microsoft.net", "fonts", "boot", "recovery" };
}
