using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Steamworks;

namespace Terraria.Social.Steam;

public class SteamP2PWriter
{
	public class WriteInformation
	{
		public byte[] Data;

		public int Size;

		public WriteInformation(byte[] data)
		{
			Data = data;
			Size = 0;
		}
	}

	private static readonly int BUFFER_SIZE = 1024;

	private Dictionary<CSteamID, List<WriteInformation>> _pendingSendData = new Dictionary<CSteamID, List<WriteInformation>>();

	private Dictionary<CSteamID, List<WriteInformation>> _pendingSendDataSwap = new Dictionary<CSteamID, List<WriteInformation>>();

	private ConcurrentBag<byte[]> _bufferPool = new ConcurrentBag<byte[]>();

	private int _channel;

	private object _lock = new object();

	public SteamP2PWriter(int channel)
	{
		_channel = channel;
	}

	public void QueueSend(CSteamID user, byte[] data, int length)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		lock (_lock)
		{
			if (!_pendingSendData.TryGetValue(user, out var value))
			{
				value = (_pendingSendData[user] = new List<WriteInformation>());
			}
			int num = length;
			int num2 = 0;
			while (num > 0)
			{
				WriteInformation writeInformation = value.LastOrDefault();
				if (writeInformation == null || writeInformation.Size == writeInformation.Data.Length)
				{
					if (!_bufferPool.TryTake(out var result))
					{
						result = new byte[BUFFER_SIZE];
					}
					writeInformation = new WriteInformation(result);
					value.Add(writeInformation);
				}
				int num3 = Math.Min(num, writeInformation.Data.Length - writeInformation.Size);
				Array.Copy(data, num2, writeInformation.Data, writeInformation.Size, num3);
				writeInformation.Size += num3;
				num -= num3;
				num2 += num3;
			}
		}
	}

	public void SendAll()
	{
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		lock (_lock)
		{
			Utils.Swap(ref _pendingSendData, ref _pendingSendDataSwap);
		}
		foreach (KeyValuePair<CSteamID, List<WriteInformation>> item in _pendingSendDataSwap)
		{
			foreach (WriteInformation item2 in item.Value)
			{
				SteamNetworking.SendP2PPacket(item.Key, item2.Data, (uint)item2.Size, (EP2PSend)2, _channel);
				_bufferPool.Add(item2.Data);
			}
			item.Value.Clear();
		}
	}
}
