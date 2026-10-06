using System.Collections.Generic;
using System.IO;
using Terraria.Net.Sockets;
using Terraria.Testing;

namespace Terraria.Net;

public class NetManager
{
	private class PacketTypeStorage<T> where T : NetModule
	{
		public static ushort Id;

		public static T Module;
	}

	public delegate bool BroadcastCondition(int clientIndex);

	public static readonly NetManager Instance = new NetManager();

	private Dictionary<ushort, NetModule> _modules = new Dictionary<ushort, NetModule>();

	private ushort _moduleCount;

	private NetManager()
	{
	}

	public void Register<T>() where T : NetModule, new()
	{
		T val = new T();
		PacketTypeStorage<T>.Id = _moduleCount;
		PacketTypeStorage<T>.Module = val;
		_modules[_moduleCount] = val;
		_moduleCount++;
	}

	public NetModule GetModule<T>() where T : NetModule
	{
		return PacketTypeStorage<T>.Module;
	}

	public ushort GetId<T>() where T : NetModule
	{
		return PacketTypeStorage<T>.Id;
	}

	public void Read(BinaryReader reader, int userId, int readLength)
	{
		Read(reader, userId, readLength, addToDiagnostics: true);
	}

	private void Read(BinaryReader reader, int userId, int readLength, bool addToDiagnostics)
	{
		ushort num = reader.ReadUInt16();
		if (_modules.ContainsKey(num))
		{
			_modules[num].Deserialize(reader, userId);
		}
		if (addToDiagnostics)
		{
			Main.ActiveNetDiagnosticsUI.CountReadModuleMessage(num, readLength);
		}
	}

	public void Broadcast(NetPacket packet, int ignoreClient = -1)
	{
		Invariant.Assert(Main.dedServ, "Packets can only be broadcast from a server.");
		for (int i = 0; i < 256; i++)
		{
			if (i != ignoreClient && Netplay.Clients[i].IsConnected())
			{
				SendData(Netplay.Clients[i].Socket, packet);
			}
		}
		packet.Recycle();
	}

	public void Broadcast(NetPacket packet, BroadcastCondition conditionToBroadcast, int ignoreClient = -1)
	{
		Invariant.Assert(Main.dedServ, "Packets can only be broadcast from a server.");
		for (int i = 0; i < 256; i++)
		{
			if (i != ignoreClient && Netplay.Clients[i].IsConnected() && conditionToBroadcast(i))
			{
				SendData(Netplay.Clients[i].Socket, packet);
			}
		}
		packet.Recycle();
	}

	private void SendToSelf(NetPacket packet)
	{
		packet.Reader.BaseStream.Position = 3L;
		Read(packet.Reader, Main.myPlayer, packet.Length, addToDiagnostics: false);
		packet.Recycle();
	}

	public void BroadcastOrLoopback(NetPacket packet)
	{
		if (Main.netMode == 2)
		{
			Broadcast(packet);
		}
		else if (Main.netMode == 0)
		{
			SendToSelf(packet);
		}
		else
		{
			packet.Recycle();
		}
	}

	public void SendToServerOrLoopback(NetPacket packet)
	{
		if (Main.netMode == 1)
		{
			SendToServer(packet);
		}
		else if (Main.netMode == 0)
		{
			SendToSelf(packet);
		}
		else
		{
			packet.Recycle();
		}
	}

	public void SendToServerOrBroadcast(NetPacket packet)
	{
		if (Main.netMode == 1)
		{
			SendToServer(packet);
		}
		else if (Main.netMode == 2)
		{
			Broadcast(packet);
		}
		else
		{
			packet.Recycle();
		}
	}

	public void SendToServer(NetPacket packet)
	{
		Invariant.Assert(!Main.dedServ, "Packets can only be sent to the server from a client.");
		SendData(Netplay.Connection.Socket, packet);
		packet.Recycle();
	}

	public void SendToClient(NetPacket packet, int playerId)
	{
		Invariant.Assert(Main.dedServ, "Packets can only be sent to a client from a server.");
		if (Netplay.Clients[playerId].IsConnected())
		{
			SendData(Netplay.Clients[playerId].Socket, packet);
		}
		packet.Recycle();
	}

	public void SendToClientOrLoopback(NetPacket packet, int playerId)
	{
		if (Main.netMode == 0 && playerId == Main.myPlayer)
		{
			SendToSelf(packet);
		}
		else
		{
			SendToClient(packet, playerId);
		}
	}

	private void SendData(ISocket socket, NetPacket packet)
	{
		if (Main.netMode == 0)
		{
			return;
		}
		packet.ShrinkToFit();
		try
		{
			Main.ActiveNetDiagnosticsUI.CountSentModuleMessage(packet.Id, packet.Length);
			socket.AsyncSend(packet.Buffer.Data, 0, packet.Length, EmptyCallback);
		}
		catch
		{
		}
	}

	private static void EmptyCallback(object state)
	{
	}
}
