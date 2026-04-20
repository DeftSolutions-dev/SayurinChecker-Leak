using System.Runtime.InteropServices;

namespace BazaChecker.Services;

public static class NativeInterop
{
	private const string DllName = "SayurinCore.dll";

	[DllImport("SayurinCore.dll", CallingConvention = CallingConvention.Cdecl)]
	private static extern void SC_FreeString(nint str);

	[DllImport("SayurinCore.dll", CallingConvention = CallingConvention.Cdecl)]
	public static extern bool SC_IsVirtualMachine();

	[DllImport("SayurinCore.dll", CallingConvention = CallingConvention.Cdecl)]
	private static extern nint SC_GetVmDetectionLog();

	public static string GetVmDetectionLog()
	{
		nint num = SC_GetVmDetectionLog();
		string result = Marshal.PtrToStringAnsi(num) ?? "[]";
		SC_FreeString(num);
		return result;
	}

	[DllImport("SayurinCore.dll", CallingConvention = CallingConvention.Cdecl)]
	public static extern bool SC_IsAdmin();

	[DllImport("SayurinCore.dll", CallingConvention = CallingConvention.Cdecl)]
	public static extern int SC_GetMonitorCount();

	[DllImport("SayurinCore.dll", CallingConvention = CallingConvention.Cdecl)]
	private static extern nint SC_GetCPU();

	[DllImport("SayurinCore.dll", CallingConvention = CallingConvention.Cdecl)]
	private static extern nint SC_GetGPU();

	[DllImport("SayurinCore.dll", CallingConvention = CallingConvention.Cdecl)]
	private static extern nint SC_GetRAM();

	[DllImport("SayurinCore.dll", CallingConvention = CallingConvention.Cdecl)]
	private static extern nint SC_GetOS();

	[DllImport("SayurinCore.dll", CallingConvention = CallingConvention.Cdecl)]
	private static extern nint SC_GetSystemSummary();

	public static string GetCPU()
	{
		nint num = SC_GetCPU();
		string result = Marshal.PtrToStringAnsi(num) ?? "";
		SC_FreeString(num);
		return result;
	}

	public static string GetGPU()
	{
		nint num = SC_GetGPU();
		string result = Marshal.PtrToStringAnsi(num) ?? "";
		SC_FreeString(num);
		return result;
	}

	public static string GetRAM()
	{
		nint num = SC_GetRAM();
		string result = Marshal.PtrToStringAnsi(num) ?? "";
		SC_FreeString(num);
		return result;
	}

	public static string GetOS()
	{
		nint num = SC_GetOS();
		string result = Marshal.PtrToStringAnsi(num) ?? "";
		SC_FreeString(num);
		return result;
	}

	public static string GetSystemSummary()
	{
		nint num = SC_GetSystemSummary();
		string result = Marshal.PtrToStringAnsi(num) ?? "{}";
		SC_FreeString(num);
		return result;
	}

	[DllImport("SayurinCore.dll", CallingConvention = CallingConvention.Cdecl)]
	private static extern nint SC_ScanRegistry();

	public static string ScanRegistry()
	{
		nint num = SC_ScanRegistry();
		string result = Marshal.PtrToStringAnsi(num) ?? "[]";
		SC_FreeString(num);
		return result;
	}

	[DllImport("SayurinCore.dll", CallingConvention = CallingConvention.Cdecl)]
	private static extern nint SC_GetSteamAccounts();

	[DllImport("SayurinCore.dll", CallingConvention = CallingConvention.Cdecl)]
	private static extern nint SC_EnrichSteamAccount([MarshalAs(UnmanagedType.LPStr)] string steamId64);

	public static string GetSteamAccounts()
	{
		nint num = SC_GetSteamAccounts();
		string result = Marshal.PtrToStringAnsi(num) ?? "[]";
		SC_FreeString(num);
		return result;
	}

	public static string EnrichSteamAccount(string steamId64)
	{
		nint num = SC_EnrichSteamAccount(steamId64);
		string result = Marshal.PtrToStringAnsi(num) ?? "{}";
		SC_FreeString(num);
		return result;
	}

	[DllImport("SayurinCore.dll", CallingConvention = CallingConvention.Cdecl)]
	private static extern nint SC_GetUsbHistory();

	public static string GetUsbHistory()
	{
		nint num = SC_GetUsbHistory();
		string result = Marshal.PtrToStringAnsi(num) ?? "[]";
		SC_FreeString(num);
		return result;
	}

	[DllImport("SayurinCore.dll", CallingConvention = CallingConvention.Cdecl)]
	private static extern nint SC_ScanDisks();

	public static string ScanDisks()
	{
		nint num = SC_ScanDisks();
		string result = Marshal.PtrToStringAnsi(num) ?? "[]";
		SC_FreeString(num);
		return result;
	}

	[DllImport("SayurinCore.dll", CallingConvention = CallingConvention.Cdecl)]
	private static extern nint SC_GetCheatSignatures();

	[DllImport("SayurinCore.dll", CallingConvention = CallingConvention.Cdecl)]
	public static extern bool SC_IsCheatSignature([MarshalAs(UnmanagedType.LPStr)] string name);

	public static string GetCheatSignatures()
	{
		nint num = SC_GetCheatSignatures();
		string result = Marshal.PtrToStringAnsi(num) ?? "[]";
		SC_FreeString(num);
		return result;
	}

	public static bool IsCheatSignature(string name)
	{
		return SC_IsCheatSignature(name);
	}
}
