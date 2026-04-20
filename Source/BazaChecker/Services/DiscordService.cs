using System;
using System.Runtime.CompilerServices;
using DiscordRPC;
using DiscordRPC.Events;
using DiscordRPC.Logging;
using DiscordRPC.Message;

namespace BazaChecker.Services;

public static class DiscordService
{
	[Serializable]
	[CompilerGenerated]
	private sealed class _003C_003Ec
	{
		public static readonly _003C_003Ec _003C_003E9 = new _003C_003Ec();

		public static OnReadyEvent _003C_003E9__2_0;

		internal void _003CInitialize_003Eb__2_0(object sender, ReadyMessage readyMessage_0)
		{
		}
	}

	private static DiscordRpcClient? _client;

	private static Timestamps? _startTime;

	public static void Initialize(string clientId)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Expected O, but got Unknown
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Expected O, but got Unknown
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Expected O, but got Unknown
		if (_client == null)
		{
			_client = new DiscordRpcClient(clientId);
			_startTime = Timestamps.Now;
			_client.Logger = (ILogger)new ConsoleLogger
			{
				Level = (LogLevel)3
			};
			DiscordRpcClient? client = _client;
			object obj = _003C_003Ec._003C_003E9__2_0;
			if (obj == null)
			{
				OnReadyEvent val = delegate
				{
				};
				_003C_003Ec._003C_003E9__2_0 = val;
				obj = (object)val;
			}
			client.OnReady += (OnReadyEvent)obj;
			_client.Initialize();
			UpdatePresence();
		}
	}

	public static void UpdatePresence(string? details = "\ud83d\udccb Проверка на читы", string? state = null)
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Expected O, but got Unknown
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Expected O, but got Unknown
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Expected O, but got Unknown
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Expected O, but got Unknown
		if (_client != null && _client.IsInitialized)
		{
			DiscordRpcClient? client = _client;
			RichPresence val = new RichPresence();
			((BaseRichPresence)val).Details = details;
			((BaseRichPresence)val).State = state;
			((BaseRichPresence)val).Assets = new Assets
			{
				LargeImageKey = "logo",
				LargeImageText = "\ud83c\udf00 Sayurin Checker",
				SmallImageKey = "info",
				SmallImageText = "By NazeSayurin?"
			};
			val.Buttons = (Button[])(object)new Button[2]
			{
				new Button
				{
					Label = "GitHub репозиторий",
					Url = "https://github.com/Mujqk/SayurinOwnChecker/releases"
				},
				new Button
				{
					Label = "Скачать с сайта",
					Url = "https://baza-cs2.ru/"
				}
			};
			((BaseRichPresence)val).Timestamps = _startTime;
			client.SetPresence(val);
		}
	}

	public static void Deinitialize()
	{
		DiscordRpcClient? client = _client;
		if (client != null)
		{
			client.Dispose();
		}
		_client = null;
	}
}
