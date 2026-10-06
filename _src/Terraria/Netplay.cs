using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using ReLogic.OS;
using Terraria.Audio;
using Terraria.Localization;
using Terraria.Map;
using Terraria.Net;
using Terraria.Net.Sockets;
using Terraria.Social;
using Terraria.Utilities;

namespace Terraria;

public class Netplay
{
	private class SetRemoteIPRequestInfo
	{
		public int RequestId;

		public Action SuccessCallback;

		public string RemoteAddress;
	}

	public const int MaxConnections = 256;

	public const int NetBufferSize = 1024;

	public const int DefaultPort = 7777;

	public static string BanFilePath = "banlist.txt";

	public static string ServerPassword = "";

	public static RemoteClient[] Clients = new RemoteClient[256];

	public static RemoteServer Connection = new RemoteServer();

	private static IPAddress _serverIP;

	public static string ServerIPText = "";

	public static bool IsHostAndPlay;

	public static string HostToken;

	public static ISocket TcpListener;

	public static int ListenPort = 7777;

	public static bool IsListening = true;

	public static bool UseUPNP = true;

	public static bool SaveOnServerExit = true;

	public static volatile bool Disconnect;

	public static volatile bool ClientLoopThreadRunning;

	public static bool SpamCheck = false;

	public static bool HasFullyConnectedClients;

	private static Thread _serverThread;

	public static MessageBuffer fullBuffer = new MessageBuffer();

	private static int _currentRequestId;

	private static long swTicksLast;

	private static UdpClient BroadcastClient = null;

	private static Thread broadcastThread = null;

	public static IPAddress ServerIP
	{
		get
		{
			return _serverIP;
		}
		private set
		{
			IsHostAndPlay = false;
			_serverIP = value;
		}
	}

	public static event Action OnDisconnect;

	private static void UpdateServerInMainThread()
	{
		for (int i = 0; i < 256; i++)
		{
			NetMessage.CheckBytes(i);
		}
	}

	private static string GetLocalIPAddress()
	{
		string result = "";
		IPAddress[] addressList = Dns.GetHostEntry(Dns.GetHostName()).AddressList;
		foreach (IPAddress iPAddress in addressList)
		{
			if (AcceptedFamilyType(iPAddress.AddressFamily))
			{
				result = iPAddress.ToString();
				break;
			}
		}
		return result;
	}

	private static void ResetNetDiag()
	{
		Main.ActiveNetDiagnosticsUI.Reset();
	}

	public static void ResetSections()
	{
		RemoteClient[] clients = Clients;
		foreach (RemoteClient remoteClient in clients)
		{
			Array.Clear(remoteClient.TileSections, 0, remoteClient.TileSections.Length);
		}
	}

	public static void AddBan(int plr)
	{
		RemoteAddress remoteAddress = Clients[plr].Socket.GetRemoteAddress();
		using StreamWriter streamWriter = new StreamWriter(BanFilePath, append: true);
		streamWriter.WriteLine("//" + Main.player[plr].name);
		streamWriter.WriteLine(remoteAddress.GetIdentifier());
	}

	public static bool IsBanned(RemoteAddress address)
	{
		try
		{
			string identifier = address.GetIdentifier();
			if (File.Exists(BanFilePath))
			{
				using StreamReader streamReader = new StreamReader(BanFilePath);
				string text;
				while ((text = streamReader.ReadLine()) != null)
				{
					if (text == identifier)
					{
						return true;
					}
				}
			}
		}
		catch (Exception)
		{
		}
		return false;
	}

	private static void OpenPort(int port)
	{
	}

	private static void ClosePort(int port)
	{
	}

	private static void ServerFullWriteCallBack(object state)
	{
	}

	private static void OnConnectionAccepted(ISocket client)
	{
		int num = FindNextOpenClientSlot();
		if (num != -1)
		{
			Clients[num].Reset();
			Clients[num].Socket = client;
			Console.WriteLine(Language.GetTextValue("Net.ClientConnecting", client.GetRemoteAddress()));
			return;
		}
		lock (fullBuffer)
		{
			KickClient(client, NetworkText.FromKey("CLI.ServerIsFull"));
		}
	}

	private static void KickClient(ISocket client, NetworkText kickMessage)
	{
		BinaryWriter writer = fullBuffer.writer;
		if (writer == null)
		{
			fullBuffer.ResetWriter();
			writer = fullBuffer.writer;
		}
		writer.BaseStream.Position = 0L;
		long position = writer.BaseStream.Position;
		writer.BaseStream.Position += 2L;
		writer.Write((byte)2);
		kickMessage.Serialize(writer);
		if (Main.dedServ)
		{
			Console.WriteLine(Language.GetTextValue("CLI.ClientWasBooted", client.GetRemoteAddress().ToString(), kickMessage));
		}
		int num = (int)writer.BaseStream.Position;
		writer.BaseStream.Position = position;
		writer.Write((short)num);
		writer.BaseStream.Position = num;
		client.AsyncSend(fullBuffer.writeBuffer, 0, num, ServerFullWriteCallBack, client);
	}

	public static void OnConnectedToSocialServer(ISocket client)
	{
		StartSocialClient(client);
	}

	private static bool StartListening()
	{
		if (SocialAPI.Network != null)
		{
			SocialAPI.Network.StartListening(OnConnectionAccepted);
		}
		return TcpListener.StartListening(OnConnectionAccepted);
	}

	public static void StartServer()
	{
		InitializeServer();
		_serverThread = new Thread(ServerLoop)
		{
			IsBackground = true,
			Name = "Server Loop Thread"
		};
		_serverThread.Start();
	}

	private static void InitializeServer()
	{
		Connection.ResetSpecialFlags();
		ResetNetDiag();
		if (Main.rand == null)
		{
			Main.rand = new UnifiedRandom((int)DateTime.Now.Ticks);
		}
		Main.myPlayer = 255;
		ServerIP = IPAddress.Any;
		Main.menuMode = 14;
		Main.statusText = Lang.menu[8].Value;
		Main.netMode = 2;
		Disconnect = false;
		for (int i = 0; i < 256; i++)
		{
			Clients[i] = new RemoteClient();
			Clients[i].Reset();
			Clients[i].Id = i;
			Clients[i].ReadBuffer = new byte[1024];
		}
		TcpListener = new TcpSocket();
		if (!Disconnect)
		{
			if (!StartListening())
			{
				Main.statusText = Language.GetTextValue("Error.TriedToRunServerTwice");
				SaveOnServerExit = false;
				Disconnect = true;
			}
			Main.statusText = Language.GetTextValue("CLI.ServerStarted");
		}
		if (!UseUPNP)
		{
			return;
		}
		try
		{
			OpenPort(ListenPort);
		}
		catch (Exception)
		{
		}
	}

	private static void ServerLoop()
	{
		int num = 0;
		StartBroadCasting();
		while (!Disconnect)
		{
			UpdateConnectedClients();
			num = (num + 1) % 10;
			Thread.Sleep((num == 0) ? 1 : 0);
		}
		StopBroadCasting();
	}

	private static void UpdateConnectedClients()
	{
		int num = 0;
		for (int i = 0; i < 256; i++)
		{
			if (Clients[i].PendingTermination)
			{
				if (Clients[i].PendingTerminationApproved)
				{
					if (!Clients[i].Kicked && !Clients[i].IsAnnouncementCompleted)
					{
						Console.WriteLine(Language.GetTextValue("Net.ClientLostConnection", Clients[i].Socket.GetRemoteAddress()));
					}
					Clients[i].Reset();
					NetMessage.SyncDisconnectedPlayer(i);
				}
			}
			else if (Clients[i].IsConnected())
			{
				Clients[i].Update();
				if (Clients[i].State == 10)
				{
					num++;
				}
			}
			else if (Clients[i].IsActive)
			{
				Clients[i].PendingTermination = true;
			}
		}
		HasFullyConnectedClients = num != 0;
	}

	private static void UpdateClientInMainThread()
	{
		if (Main.netMode == 1 && Connection.IsActive)
		{
			NetMessage.CheckBytes();
		}
	}

	public static void AddCurrentServerToRecentList()
	{
		if (Connection.Socket.GetRemoteAddress().Type != AddressType.Tcp)
		{
			return;
		}
		for (int i = 0; i < Main.maxMP; i++)
		{
			if (Main.recentIP[i].ToLower() == ServerIPText.ToLower() && Main.recentPort[i] == ListenPort)
			{
				for (int j = i; j < Main.maxMP - 1; j++)
				{
					Main.recentIP[j] = Main.recentIP[j + 1];
					Main.recentPort[j] = Main.recentPort[j + 1];
					Main.recentWorld[j] = Main.recentWorld[j + 1];
				}
			}
		}
		for (int num = Main.maxMP - 1; num > 0; num--)
		{
			Main.recentIP[num] = Main.recentIP[num - 1];
			Main.recentPort[num] = Main.recentPort[num - 1];
			Main.recentWorld[num] = Main.recentWorld[num - 1];
		}
		Main.recentIP[0] = ServerIPText;
		Main.recentPort[0] = ListenPort;
		Main.recentWorld[0] = Main.worldName;
		Main.SaveRecent();
	}

	public static void SocialClientLoop(object threadContext)
	{
		ISocket socket = (ISocket)threadContext;
		try
		{
			ClientLoopSetup(socket.GetRemoteAddress());
			Connection.Socket = socket;
			InnerClientLoop();
		}
		finally
		{
			ClientLoopThreadRunning = false;
		}
	}

	public static void TcpClientLoop()
	{
		try
		{
			ClientLoopSetup(new TcpAddress(ServerIP, ListenPort));
			TcpConnectLoop();
			InnerClientLoop();
		}
		finally
		{
			ClientLoopThreadRunning = false;
		}
	}

	private static void TcpConnectLoop()
	{
		while (true)
		{
			try
			{
				Connection.Socket.Connect(new TcpAddress(ServerIP, ListenPort));
				break;
			}
			catch
			{
				if (Platform.IsOSX)
				{
					Thread.Sleep(200);
					Connection.Socket.Close();
					Connection.Socket = new TcpSocket();
				}
				if (Disconnect || !Main.gameMenu)
				{
					break;
				}
			}
		}
	}

	private static void ClientLoopSetup(RemoteAddress address)
	{
		Connection.ResetSpecialFlags();
		ResetNetDiag();
		Main.ServerSideCharacter = false;
		if (Main.rand == null)
		{
			Main.rand = new UnifiedRandom((int)DateTime.Now.Ticks);
		}
		Main.player[Main.myPlayer].hostile = false;
		Main.player[Main.myPlayer].clientClone(Main.clientPlayer);
		for (int i = 0; i < 255; i++)
		{
			if (i != Main.myPlayer)
			{
				Main.player[i] = new Player();
			}
		}
		Disconnect = false;
		NetMessage.buffer[256].Reset();
		Connection = new RemoteServer
		{
			ReadBuffer = new byte[1024]
		};
		Main.menuMode = 14;
		if (!Main.autoPass)
		{
			Main.statusText = Language.GetTextValue("Net.ConnectingTo", address.GetFriendlyName());
		}
		Main.netMode = 1;
		Main.TrySetPreparationState(Main.WorldPreparationState.AwaitingData);
		ClientLoopThreadRunning = true;
	}

	private static void InnerClientLoop()
	{
		try
		{
			int num = -1;
			while (!Disconnect)
			{
				if (Connection.IsConnected())
				{
					Connection.IsActive = true;
					if (Connection.State == 0)
					{
						Main.statusText = Language.GetTextValue("Net.FoundServer");
						Connection.State = 1;
						NetMessage.SendData(1);
						Ping.Reset();
					}
					if (Connection.State == 2 && num != Connection.State)
					{
						Main.statusText = Language.GetTextValue("Net.SendingPlayerData");
					}
					if (Connection.State == 3 && num != Connection.State)
					{
						Main.statusText = Language.GetTextValue("Net.RequestingWorldInformation");
					}
					if (Connection.State == 4)
					{
						WorldGen.worldCleared = false;
						Connection.State = 5;
						if (Main.cloudBGActive >= 1f)
						{
							Main.cloudBGAlpha = 1f;
						}
						else
						{
							Main.cloudBGAlpha = 0f;
						}
						Main.windSpeedCurrent = Main.windSpeedTarget;
						Cloud.resetClouds();
						Main.cloudAlpha = Main.maxRaining;
						Main.ToggleGameplayUpdates(state: false);
						WorldGen.clearWorld();
						if (Main.mapEnabled)
						{
							Main.Map.Load();
						}
					}
					if (Connection.State == 5 && Main.loadMapLock)
					{
						float num2 = (float)Main.loadMapLastX / (float)Main.maxTilesX;
						Main.statusText = Lang.gen[68].Value + " " + (int)(num2 * 100f + 1f) + "%";
					}
					else if (Connection.State == 5 && WorldGen.worldCleared)
					{
						Connection.State = 6;
						Main.player[Main.myPlayer].FindSpawn();
						NetMessage.SendData(8, -1, -1, null, Main.player[Main.myPlayer].SpawnX, Main.player[Main.myPlayer].SpawnY, Main.player[Main.myPlayer].team);
					}
					if (Connection.State == 6 && num != Connection.State)
					{
						Main.statusText = Language.GetTextValue("Net.RequestingTileData");
					}
					if (!Connection.IsReading && !Disconnect && Connection.Socket.IsDataAvailable() && !Connection.ReadBufferFull)
					{
						Connection.IsReading = true;
						Connection.Socket.AsyncReceive(Connection.ReadBuffer, 0, Connection.ReadBuffer.Length, Connection.ClientReadCallBack);
					}
					if (Connection.StatusMax > 0 && Connection.StatusText != "")
					{
						if (Connection.StatusCount >= Connection.StatusMax)
						{
							Main.statusText = Language.GetTextValue("Net.StatusComplete", Connection.StatusText);
							Connection.StatusText = "";
							Connection.StatusMax = 0;
							Connection.StatusCount = 0;
						}
						else
						{
							Main.ActiveNetDiagnosticsUI.GetLastSentRecvBytes(out var _, out var recv);
							Main.statusText = $"{Connection.StatusText}: {Connection.StatusCount * 100 / Connection.StatusMax}% ({(double)recv / 1024.0:0.0} kB/s)";
						}
					}
					Thread.Sleep(1);
				}
				num = Connection.State;
			}
			try
			{
				Connection.IsActive = false;
				Connection.Socket.Close();
			}
			catch
			{
			}
			if (!Main.gameMenu)
			{
				Main.menuMode = 14;
				Main.gameMenu = true;
				Main.SwitchNetMode(0);
				MapHelper.noStatusText = true;
				Player.SavePlayer(Main.ActivePlayerFileData, skipMapSave: false, canBeSkipped: true);
				Player.ClearPlayerTempInfo();
				Main.ActivePlayerFileData.StopPlayTimer();
				SoundEngine.StopTrackedSounds();
				MapHelper.noStatusText = false;
			}
			if (Main.menuMode == 15 && Main.statusText == Language.GetTextValue("Net.LostConnection"))
			{
				Main.menuMode = 14;
			}
			if (Connection.StatusText != "" && Connection.StatusText != null)
			{
				Main.statusText = Language.GetTextValue("Net.LostConnection");
			}
			Connection.StatusCount = 0;
			Connection.StatusMax = 0;
			Connection.StatusText = "";
			Main.SwitchNetMode(0);
		}
		catch (Exception value)
		{
			try
			{
				using StreamWriter streamWriter = new StreamWriter("client-crashlog.txt", append: true);
				streamWriter.WriteLine(DateTime.Now);
				streamWriter.WriteLine(value);
				streamWriter.WriteLine("");
			}
			catch
			{
			}
			Disconnect = true;
		}
		if (OnDisconnect != null)
		{
			OnDisconnect();
		}
	}

	private static int FindNextOpenClientSlot()
	{
		for (int i = 0; i < Main.maxNetPlayers; i++)
		{
			if (!Clients[i].IsConnected())
			{
				return i;
			}
		}
		return -1;
	}

	public static void StartSocialClient(ISocket socket)
	{
		Thread thread = new Thread(SocialClientLoop);
		thread.Name = "Social Client Thread";
		thread.IsBackground = true;
		thread.Start(socket);
	}

	public static void StartTcpClient()
	{
		Thread thread = new Thread(TcpClientLoop);
		thread.Name = "TCP Client Thread";
		thread.IsBackground = true;
		thread.Start();
	}

	public static bool SetRemoteIP(string remoteAddress)
	{
		return SetRemoteIPOld(remoteAddress);
	}

	public static bool SetRemoteIPOld(string remoteAddress)
	{
		try
		{
			if (IPAddress.TryParse(remoteAddress, out var address))
			{
				ServerIP = address;
				ServerIPText = address.ToString();
				return true;
			}
			IPAddress[] addressList = Dns.GetHostEntry(remoteAddress).AddressList;
			for (int i = 0; i < addressList.Length; i++)
			{
				if (AcceptedFamilyType(addressList[i].AddressFamily))
				{
					ServerIP = addressList[i];
					ServerIPText = remoteAddress;
					return true;
				}
			}
		}
		catch (Exception)
		{
		}
		return false;
	}

	public static void SetRemoteIPAsync(string remoteAddress, Action successCallBack)
	{
		try
		{
			if (IPAddress.TryParse(remoteAddress, out var address))
			{
				ServerIP = address;
				ServerIPText = address.ToString();
				successCallBack();
			}
			else
			{
				InvalidateAllOngoingIPSetAttempts();
				Dns.BeginGetHostAddresses(remoteAddress, SetRemoteIPAsyncCallback, new SetRemoteIPRequestInfo
				{
					RequestId = _currentRequestId,
					SuccessCallback = successCallBack,
					RemoteAddress = remoteAddress
				});
			}
		}
		catch (Exception)
		{
		}
	}

	public static void InvalidateAllOngoingIPSetAttempts()
	{
		_currentRequestId++;
	}

	private static bool AcceptedFamilyType(AddressFamily family)
	{
		if (family != AddressFamily.InterNetwork)
		{
			return false;
		}
		return true;
	}

	private static void SetRemoteIPAsyncCallback(IAsyncResult ar)
	{
		SetRemoteIPRequestInfo setRemoteIPRequestInfo = (SetRemoteIPRequestInfo)ar.AsyncState;
		if (setRemoteIPRequestInfo.RequestId != _currentRequestId)
		{
			return;
		}
		try
		{
			bool flag = false;
			IPAddress[] array = Dns.EndGetHostAddresses(ar);
			for (int i = 0; i < array.Length; i++)
			{
				if (AcceptedFamilyType(array[i].AddressFamily))
				{
					ServerIP = array[i];
					ServerIPText = setRemoteIPRequestInfo.RemoteAddress;
					flag = true;
					break;
				}
			}
			if (flag)
			{
				setRemoteIPRequestInfo.SuccessCallback();
			}
		}
		catch (Exception)
		{
		}
	}

	public static void Initialize()
	{
		if (Main.dedServ)
		{
			for (int i = 0; i < 257; i++)
			{
				if (i < 256)
				{
					Clients[i] = new RemoteClient();
				}
				NetMessage.buffer[i] = new MessageBuffer();
				NetMessage.buffer[i].whoAmI = i;
			}
		}
		NetMessage.buffer[256] = new MessageBuffer();
		NetMessage.buffer[256].whoAmI = 256;
	}

	public static void UpdateInMainThread()
	{
		if (Main.dedServ)
		{
			UpdateServerInMainThread();
		}
		else
		{
			UpdateClientInMainThread();
		}
		UpdateDataRates();
	}

	public static void UpdateDataRates()
	{
		long timestamp = Stopwatch.GetTimestamp();
		if (!(Utils.SWTicksToTimeSpan(timestamp - swTicksLast).TotalSeconds < 1.0))
		{
			swTicksLast = timestamp;
			Main.ActiveNetDiagnosticsUI.RotateSendRecvCounters();
		}
	}

	public static int GetSectionX(int x)
	{
		return x / 200;
	}

	public static int GetSectionY(int y)
	{
		return y / 150;
	}

	private static void BroadcastThread()
	{
		BroadcastClient = new UdpClient();
		new IPEndPoint(IPAddress.Any, 0);
		BroadcastClient.EnableBroadcast = true;
		new DateTime(0L);
		int num = 0;
		byte[] array;
		using (MemoryStream memoryStream = new MemoryStream())
		{
			using BinaryWriter binaryWriter = new BinaryWriter(memoryStream);
			int value = 1010;
			binaryWriter.Write(value);
			binaryWriter.Write(ListenPort);
			binaryWriter.Write(Main.worldName);
			string text = Dns.GetHostName();
			if (text == "localhost")
			{
				text = Environment.MachineName;
			}
			binaryWriter.Write(text);
			binaryWriter.Write((ushort)Main.maxTilesX);
			binaryWriter.Write(Main.ActiveWorldFileData.HasCrimson);
			binaryWriter.Write(Main.ActiveWorldFileData.GameMode);
			binaryWriter.Write((byte)Main.maxNetPlayers);
			num = (int)memoryStream.Position;
			binaryWriter.Write((byte)0);
			binaryWriter.Write(Main.ActiveWorldFileData.IsHardMode);
			binaryWriter.Flush();
			array = memoryStream.ToArray();
		}
		while (true)
		{
			int num2 = 0;
			for (int i = 0; i < 255; i++)
			{
				if (Main.player[i].active)
				{
					num2++;
				}
			}
			array[num] = (byte)num2;
			try
			{
				BroadcastClient.Send(array, array.Length, new IPEndPoint(IPAddress.Broadcast, 8888));
			}
			catch
			{
			}
			Thread.Sleep(1000);
		}
	}

	public static void StartBroadCasting()
	{
		if (broadcastThread != null)
		{
			StopBroadCasting();
		}
		broadcastThread = new Thread(BroadcastThread);
		broadcastThread.Start();
	}

	public static void StopBroadCasting()
	{
		if (broadcastThread != null)
		{
			broadcastThread.Abort();
			broadcastThread = null;
		}
		if (BroadcastClient != null)
		{
			BroadcastClient.Close();
			BroadcastClient = null;
		}
	}
}
