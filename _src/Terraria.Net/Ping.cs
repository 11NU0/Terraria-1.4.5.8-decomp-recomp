using System;
using System.Diagnostics;
using Terraria.Net.Sockets;
using Terraria.Testing;

namespace Terraria.Net;

public class Ping
{
	private static Stopwatch _stopwatch = new Stopwatch();

	private static bool _waitingForResponse;

	public static int CurrentPing { get; private set; }

	public static void Reset()
	{
		CurrentPing = 0;
		_stopwatch.Restart();
		_waitingForResponse = false;
	}

	public static void Update()
	{
		if (_waitingForResponse)
		{
			CurrentPing = Math.Max(CurrentPing, (int)_stopwatch.ElapsedMilliseconds);
		}
		else if (_stopwatch.ElapsedMilliseconds >= 250)
		{
			NetMessage.SendData(154);
			_waitingForResponse = true;
			_stopwatch.Restart();
		}
	}

	internal static void PingRecieved()
	{
		CurrentPing = (int)_stopwatch.ElapsedMilliseconds;
		_waitingForResponse = false;
		if (DebugOptions.Shared_ServerPing > 0)
		{
			int num = (DebugOptions.Shared_ServerPing - CurrentPing) / 2;
			num /= 5;
			DebugNetworkStream.Latency = (uint)Utils.Clamp(DebugNetworkStream.Latency + num, 0L, 5000L);
		}
	}
}
