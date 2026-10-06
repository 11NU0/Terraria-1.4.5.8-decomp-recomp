using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using Terraria.Audio;
using Terraria.Chat;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.GameContent;
using Terraria.GameContent.Achievements;
using Terraria.GameContent.Creative;
using Terraria.GameContent.Events;
using Terraria.GameContent.Golf;
using Terraria.GameContent.Tile_Entities;
using Terraria.GameContent.UI;
using Terraria.ID;
using Terraria.Localization;
using Terraria.Map;
using Terraria.Net;
using Terraria.Net.Sockets;
using Terraria.Social;
using Terraria.Social.Steam;
using Terraria.Testing;
using Terraria.UI;

namespace Terraria;

public class MessageBuffer
{
	public const int readBufferMax = 131070;

	public const int writeBufferMax = 131070;

	public bool broadcast;

	public byte[] readBuffer = new byte[131070];

	public byte[] writeBuffer = new byte[131070];

	public bool writeLocked;

	public int messageLength;

	public int totalData;

	public int whoAmI;

	public int spamCount;

	public int maxSpam;

	public bool checkBytes;

	public MemoryStream readerStream;

	public MemoryStream writerStream;

	public BinaryReader reader;

	public BinaryWriter writer;

	public PacketHistory History = new PacketHistory();

	private float[] _temporaryProjectileAI = new float[Projectile.maxAI];

	private float[] _temporaryNPCAI = new float[NPC.maxAI];

	public int RemainingReadBufferLength => readBuffer.Length - totalData;

	public static event TileChangeReceivedEvent OnTileChangeReceived;

	public void Reset()
	{
		Array.Clear(readBuffer, 0, readBuffer.Length);
		Array.Clear(writeBuffer, 0, writeBuffer.Length);
		writeLocked = false;
		messageLength = 0;
		totalData = 0;
		spamCount = 0;
		broadcast = false;
		checkBytes = false;
		ResetReader();
		ResetWriter();
	}

	public void ResetReader()
	{
		if (readerStream != null)
		{
			readerStream.Close();
		}
		readerStream = new MemoryStream(readBuffer);
		reader = new BinaryReader(readerStream);
	}

	public void ResetWriter()
	{
		if (writerStream != null)
		{
			writerStream.Close();
		}
		writerStream = new MemoryStream(writeBuffer);
		writer = new BinaryWriter(writerStream);
	}

	private float[] ReUseTemporaryProjectileAI()
	{
		for (int i = 0; i < _temporaryProjectileAI.Length; i++)
		{
			_temporaryProjectileAI[i] = 0f;
		}
		return _temporaryProjectileAI;
	}

	private float[] ReUseTemporaryNPCAI()
	{
		for (int i = 0; i < _temporaryNPCAI.Length; i++)
		{
			_temporaryNPCAI[i] = 0f;
		}
		return _temporaryNPCAI;
	}

	public void GetData(int start, int length, out int messageType)
	{
		//IL_3364: Unknown result type (might be due to invalid IL or missing references)
		//IL_3369: Unknown result type (might be due to invalid IL or missing references)
		//IL_3371: Unknown result type (might be due to invalid IL or missing references)
		//IL_3376: Unknown result type (might be due to invalid IL or missing references)
		//IL_3bba: Unknown result type (might be due to invalid IL or missing references)
		//IL_3bbf: Unknown result type (might be due to invalid IL or missing references)
		//IL_3bc7: Unknown result type (might be due to invalid IL or missing references)
		//IL_3bcc: Unknown result type (might be due to invalid IL or missing references)
		//IL_3ff5: Unknown result type (might be due to invalid IL or missing references)
		//IL_3ffa: Unknown result type (might be due to invalid IL or missing references)
		//IL_8134: Unknown result type (might be due to invalid IL or missing references)
		//IL_8139: Unknown result type (might be due to invalid IL or missing references)
		//IL_8145: Unknown result type (might be due to invalid IL or missing references)
		//IL_814a: Unknown result type (might be due to invalid IL or missing references)
		//IL_82b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_82b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_9c1c: Unknown result type (might be due to invalid IL or missing references)
		//IL_9c21: Unknown result type (might be due to invalid IL or missing references)
		//IL_36f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_36fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_372a: Unknown result type (might be due to invalid IL or missing references)
		//IL_372f: Unknown result type (might be due to invalid IL or missing references)
		//IL_3737: Unknown result type (might be due to invalid IL or missing references)
		//IL_373c: Unknown result type (might be due to invalid IL or missing references)
		//IL_4f8c: Unknown result type (might be due to invalid IL or missing references)
		//IL_4f91: Unknown result type (might be due to invalid IL or missing references)
		//IL_67e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_67ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_72fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_7303: Unknown result type (might be due to invalid IL or missing references)
		//IL_7322: Unknown result type (might be due to invalid IL or missing references)
		//IL_7327: Unknown result type (might be due to invalid IL or missing references)
		//IL_7f35: Unknown result type (might be due to invalid IL or missing references)
		//IL_7f3a: Unknown result type (might be due to invalid IL or missing references)
		//IL_7f46: Unknown result type (might be due to invalid IL or missing references)
		//IL_7f4b: Unknown result type (might be due to invalid IL or missing references)
		//IL_804d: Unknown result type (might be due to invalid IL or missing references)
		//IL_8052: Unknown result type (might be due to invalid IL or missing references)
		//IL_82e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_82ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_87b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_87d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_87f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_87f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_881e: Unknown result type (might be due to invalid IL or missing references)
		//IL_8963: Unknown result type (might be due to invalid IL or missing references)
		//IL_8974: Unknown result type (might be due to invalid IL or missing references)
		//IL_7369: Unknown result type (might be due to invalid IL or missing references)
		//IL_736e: Unknown result type (might be due to invalid IL or missing references)
		//IL_738d: Unknown result type (might be due to invalid IL or missing references)
		//IL_7392: Unknown result type (might be due to invalid IL or missing references)
		//IL_945e: Unknown result type (might be due to invalid IL or missing references)
		//IL_96e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_96ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_96ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_9cae: Unknown result type (might be due to invalid IL or missing references)
		//IL_9cb3: Unknown result type (might be due to invalid IL or missing references)
		//IL_9c6b: Unknown result type (might be due to invalid IL or missing references)
		//IL_9c76: Unknown result type (might be due to invalid IL or missing references)
		//IL_a2cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_a2d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_a2e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_a2e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_4098: Unknown result type (might be due to invalid IL or missing references)
		//IL_409f: Unknown result type (might be due to invalid IL or missing references)
		//IL_8178: Unknown result type (might be due to invalid IL or missing references)
		//IL_818b: Unknown result type (might be due to invalid IL or missing references)
		//IL_8191: Unknown result type (might be due to invalid IL or missing references)
		//IL_8197: Unknown result type (might be due to invalid IL or missing references)
		//IL_81a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_81a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_8b42: Unknown result type (might be due to invalid IL or missing references)
		//IL_9d8e: Unknown result type (might be due to invalid IL or missing references)
		//IL_402a: Unknown result type (might be due to invalid IL or missing references)
		//IL_7b38: Unknown result type (might be due to invalid IL or missing references)
		//IL_7f79: Unknown result type (might be due to invalid IL or missing references)
		//IL_7f8b: Unknown result type (might be due to invalid IL or missing references)
		//IL_7f91: Unknown result type (might be due to invalid IL or missing references)
		//IL_9dab: Unknown result type (might be due to invalid IL or missing references)
		//IL_9db0: Unknown result type (might be due to invalid IL or missing references)
		//IL_9dd2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0acc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ad1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ade: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ae3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0af0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0af5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b02: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b07: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b14: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b19: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b26: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b2b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b38: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b3d: Unknown result type (might be due to invalid IL or missing references)
		//IL_4038: Unknown result type (might be due to invalid IL or missing references)
		//IL_4145: Unknown result type (might be due to invalid IL or missing references)
		//IL_414a: Unknown result type (might be due to invalid IL or missing references)
		//IL_416c: Unknown result type (might be due to invalid IL or missing references)
		//IL_7fb1: Unknown result type (might be due to invalid IL or missing references)
		//IL_7fbc: Unknown result type (might be due to invalid IL or missing references)
		//IL_a0b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_4046: Unknown result type (might be due to invalid IL or missing references)
		//IL_4054: Unknown result type (might be due to invalid IL or missing references)
		//IL_5060: Unknown result type (might be due to invalid IL or missing references)
		//IL_68a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_68a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_8380: Unknown result type (might be due to invalid IL or missing references)
		//IL_4064: Unknown result type (might be due to invalid IL or missing references)
		//IL_4066: Unknown result type (might be due to invalid IL or missing references)
		//IL_50a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_50a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_60aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_68c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_839b: Unknown result type (might be due to invalid IL or missing references)
		//IL_83a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_83ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_83b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_83b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_83bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_83c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_83c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_980e: Unknown result type (might be due to invalid IL or missing references)
		//IL_9819: Unknown result type (might be due to invalid IL or missing references)
		//IL_25dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_25e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_25e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_25e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_50c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_50d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_5e0a: Unknown result type (might be due to invalid IL or missing references)
		//IL_612c: Unknown result type (might be due to invalid IL or missing references)
		//IL_6977: Unknown result type (might be due to invalid IL or missing references)
		//IL_699b: Unknown result type (might be due to invalid IL or missing references)
		//IL_69a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_69aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_68f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_68fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_8405: Unknown result type (might be due to invalid IL or missing references)
		//IL_8415: Unknown result type (might be due to invalid IL or missing references)
		//IL_25fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_25ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_350f: Unknown result type (might be due to invalid IL or missing references)
		//IL_351b: Unknown result type (might be due to invalid IL or missing references)
		//IL_3520: Unknown result type (might be due to invalid IL or missing references)
		//IL_69c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_7ddf: Unknown result type (might be due to invalid IL or missing references)
		//IL_260d: Unknown result type (might be due to invalid IL or missing references)
		//IL_2612: Unknown result type (might be due to invalid IL or missing references)
		//IL_2616: Unknown result type (might be due to invalid IL or missing references)
		//IL_261b: Unknown result type (might be due to invalid IL or missing references)
		//IL_3479: Unknown result type (might be due to invalid IL or missing references)
		//IL_347b: Unknown result type (might be due to invalid IL or missing references)
		//IL_3482: Unknown result type (might be due to invalid IL or missing references)
		//IL_3484: Unknown result type (might be due to invalid IL or missing references)
		//IL_34a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_391c: Unknown result type (might be due to invalid IL or missing references)
		//IL_3923: Unknown result type (might be due to invalid IL or missing references)
		//IL_3928: Unknown result type (might be due to invalid IL or missing references)
		//IL_392d: Unknown result type (might be due to invalid IL or missing references)
		//IL_61dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_61b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_69f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_6a09: Unknown result type (might be due to invalid IL or missing references)
		//IL_6a14: Unknown result type (might be due to invalid IL or missing references)
		//IL_26ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_26f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_26f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_26fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_2701: Unknown result type (might be due to invalid IL or missing references)
		//IL_2706: Unknown result type (might be due to invalid IL or missing references)
		//IL_262a: Unknown result type (might be due to invalid IL or missing references)
		//IL_262f: Unknown result type (might be due to invalid IL or missing references)
		//IL_359d: Unknown result type (might be due to invalid IL or missing references)
		//IL_359f: Unknown result type (might be due to invalid IL or missing references)
		//IL_35a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_35a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_39f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_39f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_39f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_39fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_3a04: Unknown result type (might be due to invalid IL or missing references)
		//IL_3a06: Unknown result type (might be due to invalid IL or missing references)
		//IL_3938: Unknown result type (might be due to invalid IL or missing references)
		//IL_393d: Unknown result type (might be due to invalid IL or missing references)
		//IL_393f: Unknown result type (might be due to invalid IL or missing references)
		//IL_3944: Unknown result type (might be due to invalid IL or missing references)
		//IL_6350: Unknown result type (might be due to invalid IL or missing references)
		//IL_1bb1: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c9b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ca0: Unknown result type (might be due to invalid IL or missing references)
		//IL_2641: Unknown result type (might be due to invalid IL or missing references)
		//IL_2648: Unknown result type (might be due to invalid IL or missing references)
		//IL_264d: Unknown result type (might be due to invalid IL or missing references)
		//IL_264f: Unknown result type (might be due to invalid IL or missing references)
		//IL_2654: Unknown result type (might be due to invalid IL or missing references)
		//IL_2659: Unknown result type (might be due to invalid IL or missing references)
		//IL_395f: Unknown result type (might be due to invalid IL or missing references)
		//IL_3966: Unknown result type (might be due to invalid IL or missing references)
		//IL_396b: Unknown result type (might be due to invalid IL or missing references)
		//IL_396d: Unknown result type (might be due to invalid IL or missing references)
		//IL_3972: Unknown result type (might be due to invalid IL or missing references)
		//IL_3974: Unknown result type (might be due to invalid IL or missing references)
		//IL_3979: Unknown result type (might be due to invalid IL or missing references)
		//IL_397e: Unknown result type (might be due to invalid IL or missing references)
		//IL_3985: Unknown result type (might be due to invalid IL or missing references)
		//IL_398a: Unknown result type (might be due to invalid IL or missing references)
		//IL_62f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_8485: Unknown result type (might be due to invalid IL or missing references)
		//IL_8497: Unknown result type (might be due to invalid IL or missing references)
		//IL_849d: Unknown result type (might be due to invalid IL or missing references)
		//IL_84a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_84a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_84ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_84c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_84c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_84cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_84d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_84d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_8504: Unknown result type (might be due to invalid IL or missing references)
		//IL_850a: Unknown result type (might be due to invalid IL or missing references)
		//IL_853a: Unknown result type (might be due to invalid IL or missing references)
		//IL_853f: Unknown result type (might be due to invalid IL or missing references)
		//IL_8576: Unknown result type (might be due to invalid IL or missing references)
		//IL_857b: Unknown result type (might be due to invalid IL or missing references)
		//IL_8586: Unknown result type (might be due to invalid IL or missing references)
		//IL_858b: Unknown result type (might be due to invalid IL or missing references)
		//IL_8590: Unknown result type (might be due to invalid IL or missing references)
		//IL_1cb0: Unknown result type (might be due to invalid IL or missing references)
		//IL_1cb9: Unknown result type (might be due to invalid IL or missing references)
		//IL_2680: Unknown result type (might be due to invalid IL or missing references)
		//IL_2685: Unknown result type (might be due to invalid IL or missing references)
		//IL_2674: Unknown result type (might be due to invalid IL or missing references)
		//IL_2679: Unknown result type (might be due to invalid IL or missing references)
		//IL_6b45: Unknown result type (might be due to invalid IL or missing references)
		//IL_274e: Unknown result type (might be due to invalid IL or missing references)
		//IL_2765: Unknown result type (might be due to invalid IL or missing references)
		//IL_6a87: Unknown result type (might be due to invalid IL or missing references)
		//IL_6a98: Unknown result type (might be due to invalid IL or missing references)
		//IL_6a9d: Unknown result type (might be due to invalid IL or missing references)
		//IL_6aa2: Unknown result type (might be due to invalid IL or missing references)
		//IL_39aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_39ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_39b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_39bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_39c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_39c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_39d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_39d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_2878: Unknown result type (might be due to invalid IL or missing references)
		//IL_26a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_26a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_26b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_26b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_26bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_26c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_26cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_26d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_6444: Unknown result type (might be due to invalid IL or missing references)
		//IL_648d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ea9: Unknown result type (might be due to invalid IL or missing references)
		//IL_1eb7: Unknown result type (might be due to invalid IL or missing references)
		//IL_3d71: Unknown result type (might be due to invalid IL or missing references)
		//IL_3d73: Unknown result type (might be due to invalid IL or missing references)
		//IL_3d7a: Unknown result type (might be due to invalid IL or missing references)
		//IL_3d7c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c60: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d5a: Unknown result type (might be due to invalid IL or missing references)
		if (whoAmI < 256)
		{
			Netplay.Clients[whoAmI].TimeOutTimer = 0;
		}
		else
		{
			Netplay.Connection.TimeOutTimer = 0;
		}
		byte b = 0;
		int num = 0;
		num = start + 1;
		b = (byte)(messageType = readBuffer[start]);
		if (b >= MessageID.Count)
		{
			return;
		}
		Main.ActiveNetDiagnosticsUI.CountReadMessage(b, length);
		if (Main.netMode == 1 && Netplay.Connection.StatusMax > 0)
		{
			Netplay.Connection.StatusCount++;
		}
		if (Main.verboseNetplay)
		{
			for (int i = start; i < start + length; i++)
			{
			}
			for (int j = start; j < start + length; j++)
			{
				_ = readBuffer[j];
			}
		}
		if (Main.netMode == 2 && b != 38 && Netplay.Clients[whoAmI].State == -1)
		{
			NetMessage.TrySendData(2, whoAmI, -1, Lang.mp[1].ToNetworkText());
			return;
		}
		if (Main.netMode == 2)
		{
			if (Netplay.Clients[whoAmI].State < 10 && b > 12 && b != 93 && b != 16 && b != 42 && b != 50 && b != 38 && b != 68 && b != 147 && b != 161)
			{
				NetMessage.BootPlayer(whoAmI, Lang.mp[2].ToNetworkText());
			}
			if (Netplay.Clients[whoAmI].State == 0 && b != 1)
			{
				NetMessage.BootPlayer(whoAmI, Lang.mp[2].ToNetworkText());
			}
		}
		if (reader == null)
		{
			ResetReader();
		}
		reader.BaseStream.Position = num;
		switch (b)
		{
		case 1:
		{
			if (Main.netMode != 2)
			{
				break;
			}
			NetServerSocialModule netServerSocialModule = SocialAPI.Network as NetServerSocialModule;
			RemoteAddress remoteAddress = Netplay.Clients[whoAmI].Socket.GetRemoteAddress();
			if (netServerSocialModule != null && netServerSocialModule.ShouldBlockAsNotFriends(remoteAddress))
			{
				NetMessage.TrySendData(2, whoAmI, -1, NetworkText.FromKey("Net.SteamHostOnlyAllowsFriends"));
			}
			else if (Main.dedServ && Netplay.IsBanned(remoteAddress))
			{
				NetMessage.TrySendData(2, whoAmI, -1, Lang.mp[3].ToNetworkText());
			}
			else
			{
				if (Netplay.Clients[whoAmI].State != 0)
				{
					break;
				}
				if (reader.ReadString() == "Terraria" + 326)
				{
					if (string.IsNullOrEmpty(Netplay.ServerPassword))
					{
						Netplay.Clients[whoAmI].State = 1;
						NetMessage.TrySendData(3, whoAmI);
					}
					else
					{
						Netplay.Clients[whoAmI].State = -1;
						NetMessage.TrySendData(37, whoAmI);
					}
				}
				else
				{
					NetMessage.TrySendData(2, whoAmI, -1, Lang.mp[4].ToNetworkText());
				}
			}
			break;
		}
		case 2:
			if (Main.netMode == 1)
			{
				Netplay.Disconnect = true;
				Main.statusText = NetworkText.Deserialize(reader).ToString();
			}
			break;
		case 3:
			if (Main.netMode == 1)
			{
				if (Netplay.Connection.State == 1)
				{
					Netplay.Connection.State = 2;
				}
				int num136 = reader.ReadByte();
				bool value2 = reader.ReadBoolean();
				Netplay.Connection.ServerSpecialFlags[2] = value2;
				if (num136 != Main.myPlayer)
				{
					Main.player[num136] = Main.ActivePlayerFileData.Player;
					Main.player[Main.myPlayer] = new Player();
				}
				Main.player[num136].whoAmI = num136;
				Main.myPlayer = num136;
				Player player17 = Main.player[num136];
				NetMessage.TrySendData(4, -1, -1, null, num136);
				NetMessage.TrySendData(68, -1, -1, null, num136);
				NetMessage.TrySendData(16, -1, -1, null, num136);
				NetMessage.TrySendData(42, -1, -1, null, num136);
				NetMessage.TrySendData(50, -1, -1, null, num136);
				NetMessage.TrySendData(147, -1, -1, null, num136, player17.CurrentLoadoutIndex);
				for (int num137 = 0; num137 < 59; num137++)
				{
					NetMessage.TrySendData(5, -1, -1, null, num136, PlayerItemSlotID.Inventory0 + num137);
				}
				TrySendingItemArray(num136, player17.armor, PlayerItemSlotID.Armor0);
				TrySendingItemArray(num136, player17.dye, PlayerItemSlotID.Dye0);
				TrySendingItemArray(num136, player17.miscEquips, PlayerItemSlotID.Misc0);
				TrySendingItemArray(num136, player17.miscDyes, PlayerItemSlotID.MiscDye0);
				TrySendingItemArray(num136, player17.bank.item, PlayerItemSlotID.Bank1_0);
				TrySendingItemArray(num136, player17.bank2.item, PlayerItemSlotID.Bank2_0);
				NetMessage.TrySendData(5, -1, -1, null, num136, PlayerItemSlotID.TrashItem);
				TrySendingItemArray(num136, player17.bank3.item, PlayerItemSlotID.Bank3_0);
				TrySendingItemArray(num136, player17.bank4.item, PlayerItemSlotID.Bank4_0);
				TrySendingItemArray(num136, player17.Loadouts[0].Armor, PlayerItemSlotID.Loadout1_Armor_0);
				TrySendingItemArray(num136, player17.Loadouts[0].Dye, PlayerItemSlotID.Loadout1_Dye_0);
				TrySendingItemArray(num136, player17.Loadouts[1].Armor, PlayerItemSlotID.Loadout2_Armor_0);
				TrySendingItemArray(num136, player17.Loadouts[1].Dye, PlayerItemSlotID.Loadout2_Dye_0);
				TrySendingItemArray(num136, player17.Loadouts[2].Armor, PlayerItemSlotID.Loadout3_Armor_0);
				TrySendingItemArray(num136, player17.Loadouts[2].Dye, PlayerItemSlotID.Loadout3_Dye_0);
				if (!string.IsNullOrWhiteSpace(Netplay.HostToken))
				{
					NetMessage.TrySendData(161, -1, -1, NetworkText.FromLiteral(Netplay.HostToken));
				}
				NetMessage.TrySendData(6);
				if (Netplay.Connection.State == 2)
				{
					Netplay.Connection.State = 3;
				}
			}
			break;
		case 4:
		{
			int num194 = reader.ReadByte();
			if (Main.netMode == 2)
			{
				num194 = whoAmI;
			}
			if (num194 == Main.myPlayer && !Main.ServerSideCharacter)
			{
				break;
			}
			Player player18 = Main.player[num194];
			player18.whoAmI = num194;
			player18.skinVariant = reader.ReadByte();
			player18.skinVariant = (int)MathHelper.Clamp((float)player18.skinVariant, 0f, (float)(PlayerVariantID.Count - 1));
			player18.voiceVariant = reader.ReadByte();
			player18.voiceVariant = Utils.Clamp(player18.voiceVariant, 1, 4);
			player18.voicePitchOffset = reader.ReadSingle();
			if (float.IsNaN(player18.voicePitchOffset))
			{
				player18.voicePitchOffset = 0f;
			}
			player18.voicePitchOffset = Utils.Clamp(player18.voicePitchOffset, -1f, 1f);
			player18.hair = reader.ReadByte();
			if (player18.hair >= 228)
			{
				player18.hair = 0;
			}
			player18.name = reader.ReadString().Trim().Trim();
			player18.hairDye = reader.ReadByte();
			ReadAccessoryVisibility(reader, player18.hideVisibleAccessory);
			player18.hideMisc = reader.ReadByte();
			player18.hairColor = reader.ReadRGB();
			player18.skinColor = reader.ReadRGB();
			player18.eyeColor = reader.ReadRGB();
			player18.shirtColor = reader.ReadRGB();
			player18.underShirtColor = reader.ReadRGB();
			player18.pantsColor = reader.ReadRGB();
			player18.shoeColor = reader.ReadRGB();
			BitsByte bitsByte32 = reader.ReadByte();
			player18.difficulty = 0;
			if (bitsByte32[0])
			{
				player18.difficulty = 1;
			}
			if (bitsByte32[1])
			{
				player18.difficulty = 2;
			}
			if (bitsByte32[3])
			{
				player18.difficulty = 3;
			}
			if (player18.difficulty > 3)
			{
				player18.difficulty = 3;
			}
			player18.extraAccessory = bitsByte32[2];
			BitsByte bitsByte33 = reader.ReadByte();
			player18.UsingBiomeTorches = bitsByte33[0];
			player18.happyFunTorchTime = bitsByte33[1];
			player18.unlockedBiomeTorches = bitsByte33[2];
			player18.unlockedSuperCart = bitsByte33[3];
			player18.enabledSuperCart = bitsByte33[4];
			BitsByte bitsByte34 = reader.ReadByte();
			player18.usedAegisCrystal = bitsByte34[0];
			player18.usedAegisFruit = bitsByte34[1];
			player18.usedArcaneCrystal = bitsByte34[2];
			player18.usedGalaxyPearl = bitsByte34[3];
			player18.usedGummyWorm = bitsByte34[4];
			player18.usedAmbrosia = bitsByte34[5];
			player18.ateArtisanBread = bitsByte34[6];
			if (Main.netMode != 2)
			{
				break;
			}
			bool flag21 = false;
			if (Netplay.Clients[whoAmI].State < 10)
			{
				for (int num195 = 0; num195 < 255; num195++)
				{
					if (num195 != num194 && player18.name == Main.player[num195].name && Netplay.Clients[num195].IsActive)
					{
						flag21 = true;
					}
				}
			}
			if (flag21)
			{
				NetMessage.TrySendData(2, whoAmI, -1, NetworkText.FromKey(Lang.mp[5].Key, player18.name));
			}
			else if (player18.name.Length > Player.nameLen)
			{
				NetMessage.TrySendData(2, whoAmI, -1, NetworkText.FromKey("Net.NameTooLong"));
			}
			else if (player18.name == "")
			{
				NetMessage.TrySendData(2, whoAmI, -1, NetworkText.FromKey("Net.EmptyName"));
			}
			else if (player18.difficulty == 3 && !Main.IsJourneyMode)
			{
				NetMessage.TrySendData(2, whoAmI, -1, NetworkText.FromKey("Net.PlayerIsCreativeAndWorldIsNotCreative"));
			}
			else if (player18.difficulty != 3 && Main.IsJourneyMode)
			{
				NetMessage.TrySendData(2, whoAmI, -1, NetworkText.FromKey("Net.PlayerIsNotCreativeAndWorldIsCreative"));
			}
			else
			{
				Netplay.Clients[whoAmI].Name = player18.name;
				Netplay.Clients[whoAmI].Name = player18.name;
				NetMessage.TrySendData(4, -1, whoAmI, null, num194);
			}
			break;
		}
		case 5:
		{
			int num25 = reader.ReadByte();
			int num26 = reader.ReadInt16();
			int stack2 = reader.ReadInt16();
			int prefixWeWant = reader.ReadByte();
			int type2 = reader.ReadInt16();
			BitsByte bitsByte2 = reader.ReadByte();
			bool favorited = bitsByte2[0];
			bool flag = bitsByte2[1];
			if (Main.netMode == 2)
			{
				num25 = whoAmI;
			}
			if (num25 == Main.myPlayer && !Main.ServerSideCharacter && !Main.player[num25].HasLockedInventory())
			{
				break;
			}
			Player player2 = Main.player[num25];
			lock (player2)
			{
				PlayerItemSlotID.SlotReference slot = new PlayerItemSlotID.SlotReference(player2, num26);
				PlayerItemSlotID.SlotReference slotReference = new PlayerItemSlotID.SlotReference(Main.clientPlayer, num26);
				Item item = new Item();
				item.SetDefaults(type2);
				item.stack = stack2;
				item.Prefix(prefixWeWant);
				item.favorited = favorited;
				slot.Item = item;
				if (num25 == Main.myPlayer && !Main.ServerSideCharacter)
				{
					slotReference.Item = item.Clone();
				}
				if (num26 >= PlayerItemSlotID.Bank4_0 && num26 < PlayerItemSlotID.Loadout1_Armor_0)
				{
					if (Main.netMode == 1 && player2.disableVoidBag == num26 - PlayerItemSlotID.Bank4_0)
					{
						player2.disableVoidBag = -1;
					}
				}
				else if (num26 <= 58)
				{
					if (num25 == Main.myPlayer && num26 == 58)
					{
						Main.mouseItem = item.Clone();
					}
					if (num25 == Main.myPlayer && Main.netMode == 1)
					{
						Main.player[num25].inventoryChestStack[num26] = false;
					}
				}
				if ((Main.netMode == 1 && num25 == Main.myPlayer) & flag)
				{
					ItemSlot.IndicateBlockedSlot(slot);
				}
				bool[] canRelay = PlayerItemSlotID.CanRelay;
				if (Main.netMode == 2 && num25 == whoAmI && canRelay.IndexInRange(num26) && canRelay[num26])
				{
					NetMessage.TrySendData(5, -1, whoAmI, null, num25, num26);
				}
				break;
			}
		}
		case 6:
			if (Main.netMode == 2)
			{
				if (Netplay.Clients[whoAmI].State == 1)
				{
					Netplay.Clients[whoAmI].State = 2;
				}
				NetMessage.TrySendData(7, whoAmI);
				Main.SyncAnInvasion(whoAmI);
			}
			break;
		case 7:
			if (Main.netMode == 1)
			{
				Main.time = reader.ReadInt32();
				BitsByte bitsByte6 = reader.ReadByte();
				Main.dayTime = bitsByte6[0];
				Main.bloodMoon = bitsByte6[1];
				Main.eclipse = bitsByte6[2];
				Main.moonPhase = reader.ReadByte();
				Main.maxTilesX = reader.ReadInt16();
				Main.maxTilesY = reader.ReadInt16();
				Main.spawnTileX = reader.ReadInt16();
				Main.spawnTileY = reader.ReadInt16();
				Main.worldSurface = reader.ReadInt16();
				Main.rockLayer = reader.ReadInt16();
				Main.ActiveWorldFileData.WorldId = reader.ReadInt32();
				Main.worldName = reader.ReadString();
				Main.GameMode = reader.ReadByte();
				Main.ActiveWorldFileData.UniqueId = new Guid(reader.ReadBytes(16));
				Main.ActiveWorldFileData.WorldGeneratorVersion = reader.ReadUInt64();
				Main.moonType = reader.ReadByte();
				WorldGen.setBG(0, reader.ReadByte());
				WorldGen.setBG(10, reader.ReadByte());
				WorldGen.setBG(11, reader.ReadByte());
				WorldGen.setBG(12, reader.ReadByte());
				WorldGen.setBG(1, reader.ReadByte());
				WorldGen.setBG(2, reader.ReadByte());
				WorldGen.setBG(3, reader.ReadByte());
				WorldGen.setBG(4, reader.ReadByte());
				WorldGen.setBG(5, reader.ReadByte());
				WorldGen.setBG(6, reader.ReadByte());
				WorldGen.setBG(7, reader.ReadByte());
				WorldGen.setBG(8, reader.ReadByte());
				WorldGen.setBG(9, reader.ReadByte());
				Main.iceBackStyle = reader.ReadByte();
				Main.jungleBackStyle = reader.ReadByte();
				Main.hellBackStyle = reader.ReadByte();
				Main.windSpeedTarget = reader.ReadSingle();
				Main.numClouds = reader.ReadByte();
				for (int l = 0; l < 3; l++)
				{
					Main.treeX[l] = reader.ReadInt32();
				}
				for (int m = 0; m < 4; m++)
				{
					Main.treeStyle[m] = reader.ReadByte();
				}
				for (int n = 0; n < 3; n++)
				{
					Main.caveBackX[n] = reader.ReadInt32();
				}
				for (int num78 = 0; num78 < 4; num78++)
				{
					Main.caveBackStyle[num78] = reader.ReadByte();
				}
				WorldGen.TreeTops.SyncReceive(reader);
				WorldGen.BackgroundsCache.UpdateCache();
				Main.maxRaining = reader.ReadSingle();
				Main.raining = Main.maxRaining > 0f;
				BitsByte bitsByte7 = reader.ReadByte();
				WorldGen.shadowOrbSmashed = bitsByte7[0];
				NPC.downedBoss1 = bitsByte7[1];
				NPC.downedBoss2 = bitsByte7[2];
				NPC.downedBoss3 = bitsByte7[3];
				Main.hardMode = bitsByte7[4];
				NPC.downedClown = bitsByte7[5];
				Main.ServerSideCharacter = bitsByte7[6];
				NPC.downedPlantBoss = bitsByte7[7];
				if (Main.ServerSideCharacter)
				{
					Main.ActivePlayerFileData.MarkAsServerSide();
				}
				BitsByte bitsByte8 = reader.ReadByte();
				NPC.downedMechBoss1 = bitsByte8[0];
				NPC.downedMechBoss2 = bitsByte8[1];
				NPC.downedMechBoss3 = bitsByte8[2];
				NPC.downedMechBossAny = bitsByte8[3];
				Main.cloudBGActive = (bitsByte8[4] ? 1 : 0);
				WorldGen.crimson = bitsByte8[5];
				Main.pumpkinMoon = bitsByte8[6];
				Main.snowMoon = bitsByte8[7];
				BitsByte bitsByte9 = reader.ReadByte();
				Main.fastForwardTimeToDawn = bitsByte9[1];
				Main.UpdateTimeRate();
				bool flag8 = bitsByte9[2];
				NPC.downedSlimeKing = bitsByte9[3];
				NPC.downedQueenBee = bitsByte9[4];
				NPC.downedFishron = bitsByte9[5];
				NPC.downedMartians = bitsByte9[6];
				NPC.downedAncientCultist = bitsByte9[7];
				BitsByte bitsByte10 = reader.ReadByte();
				NPC.downedMoonlord = bitsByte10[0];
				NPC.downedHalloweenKing = bitsByte10[1];
				NPC.downedHalloweenTree = bitsByte10[2];
				NPC.downedChristmasIceQueen = bitsByte10[3];
				NPC.downedChristmasSantank = bitsByte10[4];
				NPC.downedChristmasTree = bitsByte10[5];
				NPC.downedGolemBoss = bitsByte10[6];
				BirthdayParty.ManualParty = bitsByte10[7];
				BitsByte bitsByte11 = reader.ReadByte();
				NPC.downedPirates = bitsByte11[0];
				NPC.downedFrost = bitsByte11[1];
				NPC.downedGoblins = bitsByte11[2];
				Sandstorm.Happening = bitsByte11[3];
				DD2Event.Ongoing = bitsByte11[4];
				DD2Event.DownedInvasionT1 = bitsByte11[5];
				DD2Event.DownedInvasionT2 = bitsByte11[6];
				DD2Event.DownedInvasionT3 = bitsByte11[7];
				BitsByte bitsByte12 = reader.ReadByte();
				NPC.combatBookWasUsed = bitsByte12[0];
				LanternNight.ManualLanterns = bitsByte12[1];
				NPC.downedTowerSolar = bitsByte12[2];
				NPC.downedTowerVortex = bitsByte12[3];
				NPC.downedTowerNebula = bitsByte12[4];
				NPC.downedTowerStardust = bitsByte12[5];
				Main.forceHalloweenForToday = bitsByte12[6];
				Main.forceXMasForToday = bitsByte12[7];
				BitsByte bitsByte13 = reader.ReadByte();
				NPC.boughtCat = bitsByte13[0];
				NPC.boughtDog = bitsByte13[1];
				NPC.boughtBunny = bitsByte13[2];
				NPC.freeCake = bitsByte13[3];
				Main.drunkWorld = bitsByte13[4];
				NPC.downedEmpressOfLight = bitsByte13[5];
				NPC.downedQueenSlime = bitsByte13[6];
				Main.getGoodWorld = bitsByte13[7];
				BitsByte bitsByte14 = reader.ReadByte();
				Main.tenthAnniversaryWorld = bitsByte14[0];
				Main.dontStarveWorld = bitsByte14[1];
				NPC.downedDeerclops = bitsByte14[2];
				Main.notTheBeesWorld = bitsByte14[3];
				Main.remixWorld = bitsByte14[4];
				NPC.unlockedSlimeBlueSpawn = bitsByte14[5];
				NPC.combatBookVolumeTwoWasUsed = bitsByte14[6];
				NPC.peddlersSatchelWasUsed = bitsByte14[7];
				BitsByte bitsByte15 = reader.ReadByte();
				NPC.unlockedSlimeGreenSpawn = bitsByte15[0];
				NPC.unlockedSlimeOldSpawn = bitsByte15[1];
				NPC.unlockedSlimePurpleSpawn = bitsByte15[2];
				NPC.unlockedSlimeRainbowSpawn = bitsByte15[3];
				NPC.unlockedSlimeRedSpawn = bitsByte15[4];
				NPC.unlockedSlimeYellowSpawn = bitsByte15[5];
				NPC.unlockedSlimeCopperSpawn = bitsByte15[6];
				Main.fastForwardTimeToDusk = bitsByte15[7];
				BitsByte bitsByte16 = reader.ReadByte();
				Main.noTrapsWorld = bitsByte16[0];
				Main.zenithWorld = bitsByte16[1];
				NPC.unlockedTruffleSpawn = bitsByte16[2];
				Main.vampireSeed = bitsByte16[3];
				Main.infectedSeed = bitsByte16[4];
				Main.teamBasedSpawnsSeed = bitsByte16[5];
				Main.skyblockWorld = bitsByte16[6];
				Main.dualDungeonsSeed = bitsByte16[7];
				BitsByte bitsByte17 = reader.ReadByte();
				WorldGen.Skyblock.lowTiles = bitsByte17[0];
				Main.forceHalloweenForever = bitsByte17[1];
				Main.forceXMasForever = bitsByte17[2];
				Main.moreLightningSeed = bitsByte17[3];
				Main.noLightningSeed = bitsByte17[4];
				Main.sundialCooldown = reader.ReadByte();
				Main.moondialCooldown = reader.ReadByte();
				WorldGen.SavedOreTiers.Copper = reader.ReadInt16();
				WorldGen.SavedOreTiers.Iron = reader.ReadInt16();
				WorldGen.SavedOreTiers.Silver = reader.ReadInt16();
				WorldGen.SavedOreTiers.Gold = reader.ReadInt16();
				WorldGen.SavedOreTiers.Cobalt = reader.ReadInt16();
				WorldGen.SavedOreTiers.Mythril = reader.ReadInt16();
				WorldGen.SavedOreTiers.Adamantite = reader.ReadInt16();
				if (flag8)
				{
					Main.StartSlimeRain(announce: false);
				}
				else
				{
					Main.StopSlimeRain();
				}
				Main.invasionType = reader.ReadSByte();
				Main.LobbyId = reader.ReadUInt64();
				Sandstorm.IntendedSeverity = reader.ReadSingle();
				ExtraSpawnPointManager.Read(reader, networking: true);
				Main.dungeonX = reader.ReadInt16();
				Main.dungeonY = reader.ReadInt16();
				if (Netplay.Connection.State == 3)
				{
					Main.windSpeedCurrent = Main.windSpeedTarget;
					Netplay.Connection.State = 4;
				}
				Main.checkHalloween();
				Main.checkXMas();
			}
			break;
		case 8:
		{
			if (Main.netMode != 2)
			{
				break;
			}
			NetMessage.TrySendData(7, whoAmI);
			int num200 = reader.ReadInt32();
			int num201 = reader.ReadInt32();
			int num202 = reader.ReadByte();
			bool flag22 = true;
			if (num200 == -1 || num201 == -1)
			{
				flag22 = false;
			}
			else if (num200 < 10 || num200 > Main.maxTilesX - 10)
			{
				flag22 = false;
			}
			else if (num201 < 10 || num201 > Main.maxTilesY - 10)
			{
				flag22 = false;
			}
			bool flag23 = false;
			if (Main.teamBasedSpawnsSeed && num202 != 0)
			{
				flag23 = true;
			}
			int num203 = Netplay.GetSectionX(Main.spawnTileX) - 2;
			int num204 = Netplay.GetSectionY(Main.spawnTileY) - 1;
			int num205 = num203 + 5;
			int num206 = num204 + 3;
			if (num203 < 0)
			{
				num203 = 0;
			}
			if (num205 >= Main.maxSectionsX)
			{
				num205 = Main.maxSectionsX;
			}
			if (num204 < 0)
			{
				num204 = 0;
			}
			if (num206 >= Main.maxSectionsY)
			{
				num206 = Main.maxSectionsY;
			}
			int num207 = (num205 - num203) * (num206 - num204);
			List<Point> list = new List<Point>();
			for (int num208 = num203; num208 < num205; num208++)
			{
				for (int num209 = num204; num209 < num206; num209++)
				{
					list.Add(new Point(num208, num209));
				}
			}
			int num210 = -1;
			int num211 = -1;
			if (flag22)
			{
				num200 = Netplay.GetSectionX(num200) - 2;
				num201 = Netplay.GetSectionY(num201) - 1;
				num210 = num200 + 5;
				num211 = num201 + 3;
				if (num200 < 0)
				{
					num200 = 0;
				}
				if (num210 >= Main.maxSectionsX)
				{
					num210 = Main.maxSectionsX - 1;
				}
				if (num201 < 0)
				{
					num201 = 0;
				}
				if (num211 >= Main.maxSectionsY)
				{
					num211 = Main.maxSectionsY - 1;
				}
				for (int num212 = num200; num212 <= num210; num212++)
				{
					for (int num213 = num201; num213 <= num211; num213++)
					{
						if (num212 < num203 || num212 >= num205 || num213 < num204 || num213 >= num206)
						{
							list.Add(new Point(num212, num213));
							num207++;
						}
					}
				}
			}
			int num214 = -1;
			int num215 = -1;
			int num216 = -1;
			int num217 = -1;
			if (flag23)
			{
				Point spawnPoint2 = Point.Zero;
				if (ExtraSpawnPointManager.TryGetExtraSpawnPointForTeam(num202, out spawnPoint2))
				{
					num214 = spawnPoint2.X;
					num215 = spawnPoint2.Y;
					num214 = Netplay.GetSectionX(num214) - 2;
					num215 = Netplay.GetSectionY(num215) - 1;
					num216 = num214 + 5;
					num217 = num215 + 3;
					if (num214 < 0)
					{
						num214 = 0;
					}
					if (num216 >= Main.maxSectionsX)
					{
						num216 = Main.maxSectionsX - 1;
					}
					if (num215 < 0)
					{
						num215 = 0;
					}
					if (num217 >= Main.maxSectionsY)
					{
						num217 = Main.maxSectionsY - 1;
					}
					for (int num218 = num214; num218 <= num216; num218++)
					{
						for (int num219 = num215; num219 <= num217; num219++)
						{
							if ((num218 < num203 || num218 >= num205 || num219 < num204 || num219 >= num206) && (num218 < num200 || num218 >= num210 || num219 < num201 || num219 >= num211))
							{
								list.Add(new Point(num218, num219));
								num207++;
							}
						}
					}
				}
				else
				{
					flag23 = false;
				}
			}
			PortalHelper.SyncPortalsOnPlayerJoin(whoAmI, 1, list, out var portalSections);
			num207 += portalSections.Count;
			if (Netplay.Clients[whoAmI].State == 2)
			{
				Netplay.Clients[whoAmI].State = 3;
			}
			NetMessage.TrySendData(9, whoAmI, -1, Lang.inter[44].ToNetworkText(), num207);
			for (int num220 = num203; num220 < num205; num220++)
			{
				for (int num221 = num204; num221 < num206; num221++)
				{
					NetMessage.SendSection(whoAmI, num220, num221);
				}
			}
			if (flag22)
			{
				for (int num222 = num200; num222 <= num210; num222++)
				{
					for (int num223 = num201; num223 <= num211; num223++)
					{
						NetMessage.SendSection(whoAmI, num222, num223);
					}
				}
			}
			if (flag23)
			{
				for (int num224 = num214; num224 <= num216; num224++)
				{
					for (int num225 = num215; num225 <= num217; num225++)
					{
						NetMessage.SendSection(whoAmI, num224, num225);
					}
				}
			}
			for (int num226 = 0; num226 < portalSections.Count; num226++)
			{
				NetMessage.SendSection(whoAmI, portalSections[num226].X, portalSections[num226].Y);
			}
			for (int num227 = 0; num227 < 400; num227++)
			{
				if (Main.item[num227].active)
				{
					NetMessage.TrySendData(21, whoAmI, -1, null, num227);
					NetMessage.TrySendData(22, whoAmI, -1, null, num227);
				}
			}
			for (int num228 = 0; num228 < Main.maxNPCs; num228++)
			{
				if (Main.npc[num228].active)
				{
					NetMessage.TrySendData(23, whoAmI, -1, null, num228);
					NetMessage.TrySendData(54, whoAmI, -1, null, num228);
				}
			}
			for (int num229 = 0; num229 < 1000; num229++)
			{
				if (Main.projectile[num229].active && (Main.projPet[Main.projectile[num229].type] || Main.projectile[num229].netImportant))
				{
					NetMessage.TrySendData(27, whoAmI, -1, null, num229);
				}
			}
			NetManager.Instance.SendToClient(BannerSystem.NetBannersModule.WriteFullState(), whoAmI);
			NetMessage.TrySendData(57, whoAmI);
			NetMessage.TrySendData(103);
			NetMessage.TrySendData(101, whoAmI);
			NetMessage.TrySendData(136, whoAmI);
			Main.BestiaryTracker.OnPlayerJoining(whoAmI);
			CreativePowerManager.Instance.SyncThingsToJoiningPlayer(whoAmI);
			Main.PylonSystem.OnPlayerJoining(whoAmI);
			NetMessage.TrySendData(49, whoAmI);
			break;
		}
		case 9:
			if (Main.netMode == 1)
			{
				Netplay.Connection.StatusMax += reader.ReadInt32();
				Netplay.Connection.StatusText = NetworkText.Deserialize(reader).ToString();
				BitsByte bitsByte35 = reader.ReadByte();
				BitsByte serverSpecialFlags = Netplay.Connection.ServerSpecialFlags;
				serverSpecialFlags[0] = bitsByte35[0];
				serverSpecialFlags[1] = bitsByte35[1];
				Netplay.Connection.ServerSpecialFlags = serverSpecialFlags;
			}
			break;
		case 10:
			if (Main.netMode == 1)
			{
				NetMessage.DecompressTileBlock(reader.BaseStream);
			}
			break;
		case 11:
			if (Main.netMode == 1)
			{
				WorldGen.SectionTileFrame(reader.ReadInt16(), reader.ReadInt16(), reader.ReadInt16(), reader.ReadInt16());
			}
			break;
		case 12:
		{
			int num106 = reader.ReadByte();
			if (Main.netMode == 2)
			{
				num106 = whoAmI;
			}
			Player player10 = Main.player[num106];
			player10.SpawnX = reader.ReadInt16();
			player10.SpawnY = reader.ReadInt16();
			player10.respawnTimer = reader.ReadInt32();
			player10.numberOfDeathsPVE = reader.ReadInt16();
			player10.numberOfDeathsPVP = reader.ReadInt16();
			player10.team = reader.ReadByte();
			if (player10.respawnTimer > 0)
			{
				player10.dead = true;
			}
			PlayerSpawnContext playerSpawnContext = (PlayerSpawnContext)reader.ReadByte();
			player10.Spawn(playerSpawnContext);
			if (Main.netMode != 2 || Netplay.Clients[whoAmI].State < 3)
			{
				break;
			}
			if (Netplay.Clients[whoAmI].State == 3)
			{
				Netplay.Clients[whoAmI].State = 10;
				NetMessage.buffer[whoAmI].broadcast = true;
				NetMessage.SyncConnectedPlayer(whoAmI);
				bool flag14 = NetMessage.DoesPlayerSlotCountAsAHost(whoAmI);
				Main.countsAsHostForGameplay[whoAmI] = flag14;
				if (NetMessage.DoesPlayerSlotCountAsAHost(whoAmI))
				{
					NetMessage.TrySendData(139, whoAmI, -1, null, whoAmI, flag14.ToInt());
				}
				NetMessage.TrySendData(12, -1, whoAmI, null, whoAmI, (int)(byte)playerSpawnContext);
				NetMessage.TrySendData(129, whoAmI);
				NetMessage.greetPlayer(whoAmI);
				if (Main.player[num106].unlockedBiomeTorches)
				{
					NPC nPC = new NPC();
					nPC.SetDefaults(664);
					Main.BestiaryTracker.Kills.RegisterKill(nPC);
				}
			}
			else
			{
				NetMessage.TrySendData(12, -1, whoAmI, null, whoAmI, (int)(byte)playerSpawnContext);
			}
			break;
		}
		case 13:
		{
			int num123 = reader.ReadByte();
			if (num123 == Main.myPlayer && !Main.ServerSideCharacter)
			{
				break;
			}
			if (Main.netMode == 2)
			{
				num123 = whoAmI;
			}
			Player player13 = Main.player[num123];
			BitsByte bitsByte24 = reader.ReadByte();
			BitsByte bitsByte25 = reader.ReadByte();
			BitsByte bitsByte26 = reader.ReadByte();
			BitsByte bitsByte27 = reader.ReadByte();
			player13.releaseDash |= !player13.controlDash;
			player13.controlUp = bitsByte24[0];
			player13.controlDown = bitsByte24[1];
			player13.controlLeft = bitsByte24[2];
			player13.controlRight = bitsByte24[3];
			player13.controlJump = bitsByte24[4];
			player13.controlUseItem = bitsByte24[5];
			player13.direction = (bitsByte24[6] ? 1 : (-1));
			player13.controlDash = bitsByte24[7];
			if (bitsByte25[0])
			{
				player13.pulley = true;
				player13.pulleyDir = (byte)((!bitsByte25[1]) ? 1u : 2u);
			}
			else
			{
				player13.pulley = false;
			}
			player13.vortexStealthActive = bitsByte25[3];
			player13.gravDir = (bitsByte25[4] ? 1 : (-1));
			player13.TryTogglingShield(bitsByte25[5]);
			player13.ghost = bitsByte25[6];
			player13.accSnappingStoneLightUp = bitsByte27[7];
			player13.selectedItemState.Select(reader.ReadByte());
			Vector2 val12 = reader.ReadVector2();
			Vector2 velocity2 = Vector2.Zero;
			if (bitsByte25[2])
			{
				velocity2 = reader.ReadVector2();
			}
			if (player13.unacknowledgedTeleports > 0)
			{
				val12 = player13.position;
				velocity2 = player13.velocity;
			}
			if (Main.netMode == 1 && player13.position != Vector2.Zero)
			{
				player13.netOffset += player13.position - val12;
				if (player13.netOffset.Length() > (float)Main.multiplayerNPCSmoothingRange)
				{
					player13.netOffset = Vector2.Zero;
				}
				if (player13.netOffset != Vector2.Zero && DebugOptions.ShowNetOffset)
				{
					using (DebugVisualizer.InPhase(DebugVisualizer.UpdatePhase.UpdateInWorld))
					{
						DebugVisualizer.World.AddLine(val12 + player13.Size / 2f, player13.Center, Color.Red, default, 20, 2f);
					}
				}
			}
			player13.position = val12;
			player13.velocity = velocity2;
			Vector2 t = player13.position;
			if (bitsByte25[7])
			{
				player13.mount.SetMount(reader.ReadUInt16(), player13);
			}
			else
			{
				player13.mount.Dismount(player13);
			}
			if (bitsByte26[6])
			{
				player13.PotionOfReturnOriginalUsePosition = reader.ReadVector2();
				player13.PotionOfReturnHomePosition = reader.ReadVector2();
			}
			else
			{
				player13.PotionOfReturnOriginalUsePosition = null;
				player13.PotionOfReturnHomePosition = null;
			}
			player13.tryKeepingHoveringUp = bitsByte26[0];
			player13.IsVoidVaultEnabled = bitsByte26[1];
			player13.sitting.isSitting = bitsByte26[2];
			player13.downedDD2EventAnyDifficulty = bitsByte26[3];
			player13.petting.isPetting = bitsByte26[4];
			player13.petting.isPetSmall = bitsByte26[5];
			player13.tryKeepingHoveringDown = bitsByte26[7];
			player13.sleeping.SetIsSleepingAndAdjustPlayerRotation(player13, bitsByte27[0]);
			player13.autoReuseAllWeapons = bitsByte27[1];
			player13.controlDownHold = bitsByte27[2];
			player13.isOperatingAnotherEntity = bitsByte27[3];
			player13.controlUseTile = bitsByte27[4];
			player13.netCameraTarget = (bitsByte27[5] ? new Vector2?(reader.ReadVector2()) : ((Vector2?)null));
			player13.lastItemUseAttemptSuccess = bitsByte27[6];
			Utils.Swap(ref t, ref player13.position);
			if (Main.netMode == 2 && Netplay.Clients[whoAmI].State == 10)
			{
				NetMessage.TrySendData(13, -1, whoAmI, null, num123);
			}
			Utils.Swap(ref t, ref player13.position);
			break;
		}
		case 14:
		{
			int num241 = reader.ReadByte();
			int num242 = reader.ReadByte();
			if (Main.netMode != 1)
			{
				break;
			}
			bool active = Main.player[num241].active;
			if (num242 == 1)
			{
				if (!Main.player[num241].active)
				{
					Main.player[num241] = new Player();
				}
				Main.player[num241].active = true;
			}
			else
			{
				Main.player[num241].active = false;
			}
			if (active != Main.player[num241].active)
			{
				if (Main.player[num241].active)
				{
					Player.Hooks.PlayerConnect(num241);
				}
				else
				{
					Player.Hooks.PlayerDisconnect(num241);
				}
			}
			break;
		}
		case 16:
		{
			int num72 = reader.ReadByte();
			if (num72 != Main.myPlayer || Main.ServerSideCharacter)
			{
				if (Main.netMode == 2)
				{
					num72 = whoAmI;
				}
				Player player9 = Main.player[num72];
				player9.statLife = reader.ReadInt16();
				player9.statLifeMax = reader.ReadInt16();
				if (player9.statLifeMax < 20)
				{
					player9.statLifeMax = 20;
				}
				player9.dead = player9.statLife <= 0;
				if (Main.netMode == 2)
				{
					NetMessage.TrySendData(16, -1, whoAmI, null, num72);
				}
			}
			break;
		}
		case 17:
		{
			byte b13 = reader.ReadByte();
			int num159 = reader.ReadInt16();
			int num160 = reader.ReadInt16();
			short num161 = reader.ReadInt16();
			int num162 = reader.ReadByte();
			bool flag16 = num161 == 1;
			if (!WorldGen.InWorld(num159, num160, 3))
			{
				break;
			}
			if (Main.tile[num159, num160] == null)
			{
				Main.tile[num159, num160] = new Tile();
			}
			if (Main.netMode == 2)
			{
				if (!flag16)
				{
					if (b13 == 0 || b13 == 2 || b13 == 4)
					{
						Netplay.Clients[whoAmI].SpamDeleteBlock++;
					}
					if (b13 == 1 || b13 == 3)
					{
						Netplay.Clients[whoAmI].SpamAddBlock++;
					}
				}
				if (!Netplay.Clients[whoAmI].TileSections[Netplay.GetSectionX(num159), Netplay.GetSectionY(num160)])
				{
					flag16 = true;
				}
			}
			MapUpdateQueue.Add(num159, num160);
			bool flag17 = false;
			using (Item.DefaultAssignNewItemsToPlayer(whoAmI))
			{
				if (b13 == 0)
				{
					WorldGen.KillTile(num159, num160, flag16);
					if (Main.netMode == 1 && !flag16)
					{
						HitTile.ClearAllTilesAtThisLocation(num159, num160);
					}
				}
				if (b13 == 1)
				{
					bool forced = true;
					if (WorldGen.CheckTileBreakability2_ShouldTileSurvive(num159, num160))
					{
						flag17 = true;
						forced = false;
					}
					WorldGen.PlaceTile(num159, num160, num161, mute: false, forced, -1, num162);
				}
				if (b13 == 2)
				{
					WorldGen.KillWall(num159, num160, flag16);
				}
				if (b13 == 3)
				{
					WorldGen.PlaceWall(num159, num160, num161);
				}
				if (b13 == 4)
				{
					WorldGen.KillTile(num159, num160, flag16, effectOnly: false, noItem: true);
				}
				if (b13 == 5)
				{
					WorldGen.PlaceWire(num159, num160);
				}
				if (b13 == 6)
				{
					WorldGen.KillWire(num159, num160);
				}
				if (b13 == 7)
				{
					WorldGen.PoundTile(num159, num160);
				}
				if (b13 == 8)
				{
					WorldGen.PlaceActuator(num159, num160);
				}
				if (b13 == 9)
				{
					WorldGen.KillActuator(num159, num160);
				}
				if (b13 == 10)
				{
					WorldGen.PlaceWire2(num159, num160);
				}
				if (b13 == 11)
				{
					WorldGen.KillWire2(num159, num160);
				}
				if (b13 == 12)
				{
					WorldGen.PlaceWire3(num159, num160);
				}
				if (b13 == 13)
				{
					WorldGen.KillWire3(num159, num160);
				}
				if (b13 == 14)
				{
					WorldGen.SlopeTile(num159, num160, num161);
				}
				if (b13 == 15)
				{
					Minecart.FrameTrack(num159, num160, pound: true);
				}
				if (b13 == 16)
				{
					WorldGen.PlaceWire4(num159, num160);
				}
				if (b13 == 17)
				{
					WorldGen.KillWire4(num159, num160);
				}
				switch (b13)
				{
				case 18:
					Wiring.SetCurrentUser(whoAmI);
					Wiring.PokeLogicGate(num159, num160);
					Wiring.SetCurrentUser();
					return;
				case 19:
					Wiring.SetCurrentUser(whoAmI);
					Wiring.Actuate(num159, num160);
					Wiring.SetCurrentUser();
					return;
				case 20:
					if (WorldGen.InWorld(num159, num160, 2))
					{
						int type16 = Main.tile[num159, num160].type;
						WorldGen.KillTile(num159, num160, flag16);
						num161 = (short)((Main.tile[num159, num160].active() && Main.tile[num159, num160].type == type16) ? 1 : 0);
						if (Main.netMode == 2)
						{
							NetMessage.TrySendData(17, -1, -1, null, b13, num159, num160, num161, num162);
						}
					}
					return;
				case 21:
					WorldGen.ReplaceTile(num159, num160, (ushort)num161, num162);
					break;
				}
				if (b13 == 22)
				{
					WorldGen.ReplaceWall(num159, num160, (ushort)num161);
				}
				if (b13 == 23 && WorldGen.CanPoundTile(num159, num160))
				{
					Main.tile[num159, num160].slope((byte)num161);
					WorldGen.PoundTile(num159, num160);
				}
			}
			if (Main.netMode == 2)
			{
				if (flag17)
				{
					NetMessage.SendTileSquare(-1, num159, num160, 5);
				}
				else if ((b13 != 1 && b13 != 21) || !TileID.Sets.Falling[num161] || Main.tile[num159, num160].active())
				{
					NetMessage.TrySendData(17, -1, whoAmI, null, b13, num159, num160, num161, num162);
				}
			}
			break;
		}
		case 18:
			if (Main.netMode == 1)
			{
				Main.dayTime = reader.ReadByte() == 1;
				Main.time = reader.ReadInt32();
				Main.sunModY = reader.ReadInt16();
				Main.moonModY = reader.ReadInt16();
			}
			break;
		case 19:
		{
			byte b5 = reader.ReadByte();
			int num45 = reader.ReadInt16();
			int num46 = reader.ReadInt16();
			if (WorldGen.InWorld(num45, num46, 3))
			{
				int num47 = ((reader.ReadByte() != 0) ? 1 : (-1));
				switch (b5)
				{
				case 0:
					WorldGen.OpenDoor(num45, num46, num47);
					break;
				case 1:
					WorldGen.CloseDoor(num45, num46, forced: true);
					break;
				case 2:
					WorldGen.ShiftTrapdoor(num45, num46, num47 == 1, 1);
					break;
				case 3:
					WorldGen.ShiftTrapdoor(num45, num46, num47 == 1, 0);
					break;
				case 4:
					WorldGen.ShiftTallGate(num45, num46, closing: false, forced: true);
					break;
				case 5:
					WorldGen.ShiftTallGate(num45, num46, closing: true, forced: true);
					break;
				}
				if (Main.netMode == 2)
				{
					NetMessage.TrySendData(19, -1, whoAmI, null, b5, num45, num46, (num47 == 1) ? 1 : 0);
				}
			}
			break;
		}
		case 20:
		{
			int num93 = reader.ReadInt16();
			int num94 = reader.ReadInt16();
			ushort num95 = reader.ReadByte();
			ushort num96 = reader.ReadByte();
			byte b7 = reader.ReadByte();
			if (!WorldGen.InWorld(num93, num94, 3))
			{
				break;
			}
			TileChangeType type7 = TileChangeType.None;
			if (Enum.IsDefined(typeof(TileChangeType), b7))
			{
				type7 = (TileChangeType)b7;
			}
			if (OnTileChangeReceived != null)
			{
				OnTileChangeReceived(num93, num94, Math.Max(num95, num96), type7);
			}
			BitsByte bitsByte19 = (byte)0;
			BitsByte bitsByte20 = (byte)0;
			BitsByte bitsByte21 = (byte)0;
			Tile tile4 = null;
			for (int num97 = num93; num97 < num93 + num95; num97++)
			{
				for (int num98 = num94; num98 < num94 + num96; num98++)
				{
					if (Main.tile[num97, num98] == null)
					{
						Main.tile[num97, num98] = new Tile();
					}
					tile4 = Main.tile[num97, num98];
					bool flag10 = tile4.active();
					bitsByte19 = reader.ReadByte();
					bitsByte20 = reader.ReadByte();
					bitsByte21 = reader.ReadByte();
					tile4.active(bitsByte19[0]);
					tile4.wall = (byte)(bitsByte19[2] ? 1u : 0u);
					bool flag11 = bitsByte19[3];
					if (Main.netMode != 2)
					{
						tile4.liquid = (byte)(flag11 ? 1u : 0u);
					}
					tile4.wire(bitsByte19[4]);
					tile4.halfBrick(bitsByte19[5]);
					tile4.actuator(bitsByte19[6]);
					tile4.inActive(bitsByte19[7]);
					tile4.wire2(bitsByte20[0]);
					tile4.wire3(bitsByte20[1]);
					if (bitsByte20[2])
					{
						tile4.color(reader.ReadByte());
					}
					if (bitsByte20[3])
					{
						tile4.wallColor(reader.ReadByte());
					}
					if (tile4.active())
					{
						int type8 = tile4.type;
						tile4.type = reader.ReadUInt16();
						if (Main.tileFrameImportant[tile4.type])
						{
							tile4.frameX = reader.ReadInt16();
							tile4.frameY = reader.ReadInt16();
						}
						else if (!flag10 || tile4.type != type8)
						{
							tile4.frameX = -1;
							tile4.frameY = -1;
						}
						byte b8 = 0;
						if (bitsByte20[4])
						{
							b8++;
						}
						if (bitsByte20[5])
						{
							b8 += 2;
						}
						if (bitsByte20[6])
						{
							b8 += 4;
						}
						tile4.slope(b8);
					}
					tile4.wire4(bitsByte20[7]);
					tile4.fullbrightBlock(bitsByte21[0]);
					tile4.fullbrightWall(bitsByte21[1]);
					tile4.invisibleBlock(bitsByte21[2]);
					tile4.invisibleWall(bitsByte21[3]);
					if (tile4.wall > 0)
					{
						tile4.wall = reader.ReadUInt16();
					}
					if (flag11)
					{
						tile4.liquid = reader.ReadByte();
						tile4.liquidType(reader.ReadByte());
					}
				}
			}
			WorldGen.RangeFrame(num93, num94, num93 + num95, num94 + num96);
			if (Main.netMode == 2)
			{
				NetMessage.TrySendData(b, -1, whoAmI, null, num93, num94, (int)num95, (int)num96, b7);
			}
			break;
		}
		case 21:
		case 90:
		{
			int num41 = reader.ReadInt16();
			Vector2 val = reader.ReadVector2();
			Vector2 velocity = reader.ReadVector2();
			int stack3 = reader.ReadInt16();
			int num42 = reader.ReadByte();
			BitsByte bitsByte4 = reader.ReadByte();
			bool flag3 = bitsByte4[2];
			bool flag4 = bitsByte4[3];
			int num43 = reader.ReadInt16();
			bool shimmered = flag3 && reader.ReadBoolean();
			float shimmerTime = (flag3 ? reader.ReadSingle() : 0f);
			int enemyGrabDelayTime = (flag4 ? reader.ReadByte() : 0);
			WorldItem worldItem = Main.item[num41];
			if (Main.netMode == 1)
			{
				if (worldItem.IsAir)
				{
					WorldItem[] item2 = Main.item;
					int num44 = num41;
					WorldItem worldItem2 = new WorldItem(new Item(num43))
					{
						whoAmI = num41
					};
					worldItem = worldItem2;
					item2[num44] = worldItem2;
				}
				else if (worldItem.type != num43)
				{
					worldItem.inner.SetDefaults(num43);
				}
				if (worldItem.prefix != num42)
				{
					worldItem.Prefix(num42);
				}
				worldItem.stack = stack3;
				worldItem.position = val;
				worldItem.velocity = velocity;
				worldItem.shimmered = shimmered;
				worldItem.shimmerTime = shimmerTime;
				worldItem.enemyGrabDelayTime = enemyGrabDelayTime;
				worldItem.wet = Collision.WetCollision(worldItem.position, worldItem.width, worldItem.height);
				if (b == 90)
				{
					worldItem.instanced = true;
					worldItem.playerIndexTheItemIsReservedFor = Main.myPlayer;
				}
			}
			else
			{
				if (Main.timeItemSlotCannotBeReusedFor[num41] > 0)
				{
					break;
				}
				NewItemOwnership owner = (NewItemOwnership)((byte)bitsByte4 & 3);
				bool flag5 = num41 == 400;
				if (flag5)
				{
					num41 = Item.NewItem(new EntitySource_Sync(), val + new Vector2(8f, 8f), num43, stack3, 0, NewItemOwnership.None, null, null, noBroadcast: true);
					worldItem = Main.item[num41];
				}
				else
				{
					if (worldItem.IsAir || worldItem.playerIndexTheItemIsReservedFor != whoAmI)
					{
						break;
					}
					if (num43 != worldItem.type)
					{
						worldItem.inner.SetDefaults(num43);
					}
				}
				if (num42 != worldItem.prefix)
				{
					worldItem.Prefix(num42);
				}
				worldItem.stack = stack3;
				worldItem.position = val;
				worldItem.velocity = velocity;
				worldItem.shimmered = shimmered;
				worldItem.shimmerTime = shimmerTime;
				worldItem.enemyGrabDelayTime = enemyGrabDelayTime;
				NetMessage.TrySendData(b, -1, flag5 ? (-1) : whoAmI, null, num41);
				if (flag5)
				{
					worldItem.ApplySpawnOwnership(owner, whoAmI);
				}
			}
			break;
		}
		case 151:
		{
			int num142 = reader.ReadInt16();
			WorldItem worldItem4 = Main.item[num142];
			if ((Main.netMode != 2 || Main.timeItemSlotCannotBeReusedFor[num142] <= 0) && (Main.netMode != 2 || worldItem4.playerIndexTheItemIsReservedFor == whoAmI))
			{
				worldItem4.playerIndexTheItemIsReservedFor = 255;
				worldItem4.TurnToAir();
				if (Main.netMode == 2)
				{
					NetMessage.TrySendData(151, -1, whoAmI, null, num142);
				}
			}
			break;
		}
		case 22:
			if (Main.netMode != 2)
			{
				WorldItem obj7 = Main.item[reader.ReadInt16()];
				obj7.playerIndexTheItemIsReservedFor = reader.ReadByte();
				obj7.timeToKeepReservation = reader.Read7BitEncodedInt();
				obj7.grabDelayPlayer = reader.ReadByte();
				obj7.grabDelayTime = reader.Read7BitEncodedInt();
				obj7.position = reader.ReadVector2();
			}
			break;
		case 23:
		{
			if (Main.netMode != 1)
			{
				break;
			}
			byte b14 = reader.ReadByte();
			byte b15 = reader.ReadByte();
			Vector2 val13 = reader.ReadVector2();
			Vector2 velocity5 = reader.ReadVector2();
			int num181 = reader.ReadUInt16();
			BitsByte bitsByte30 = reader.ReadByte();
			BitsByte bitsByte31 = reader.ReadByte();
			float[] array2 = ReUseTemporaryNPCAI();
			for (int num182 = 0; num182 < NPC.maxAI; num182++)
			{
				if (bitsByte30[num182 + 2])
				{
					array2[num182] = reader.ReadSingle();
				}
				else
				{
					array2[num182] = 0f;
				}
			}
			int num183 = reader.ReadInt16();
			int? playerCountForMultiplayerDifficultyOverride = 1;
			if (bitsByte31[0])
			{
				playerCountForMultiplayerDifficultyOverride = reader.ReadByte();
			}
			float value3 = 1f;
			if (bitsByte31[2])
			{
				value3 = reader.ReadSingle();
			}
			int num184 = 0;
			if (!bitsByte30[7])
			{
				num184 = reader.ReadByte() switch
				{
					2 => reader.ReadInt16(), 
					4 => reader.ReadInt32(), 
					_ => reader.ReadSByte(), 
				};
			}
			NPC nPC5 = Main.npc[b14];
			bool flag19 = bitsByte31[3] || nPC5.generation != b15;
			int num185 = -1;
			if (flag19)
			{
				nPC5 = NPC.NewNPCInstanceInSlot(b14, b15);
				nPC5.SetDefaults(num183, new NPCSpawnParams
				{
					playerCountForMultiplayerDifficultyOverride = playerCountForMultiplayerDifficultyOverride,
					difficultyOverride = value3
				});
			}
			else if (nPC5.netID != num183)
			{
				num185 = nPC5.type;
				nPC5.active = true;
				nPC5.SetDefaults(num183, new NPCSpawnParams
				{
					playerCountForMultiplayerDifficultyOverride = playerCountForMultiplayerDifficultyOverride,
					difficultyOverride = value3
				});
			}
			else if (!nPC5.active)
			{
				nPC5.active = true;
			}
			Vector2 val14 = NPCID.Sets.SyncAnchor[nPC5.type] * nPC5.Size;
			if (!flag19 && Vector2.DistanceSquared(nPC5.position + val14, val13) <= (float)(Main.multiplayerNPCSmoothingRange * Main.multiplayerNPCSmoothingRange))
			{
				NPC nPC6 = nPC5;
				nPC6.netOffset += nPC5.position + val14 - val13;
				if (nPC5.netOffset != Vector2.Zero && DebugOptions.ShowNetOffset)
				{
					using (DebugVisualizer.InPhase(DebugVisualizer.UpdatePhase.UpdateInWorld))
					{
						DebugVisualizer.World.AddLine(val13 + nPC5.Size / 2f, nPC5.Center, Color.Red, default, 20, 2f);
					}
				}
			}
			nPC5.position = val13 - val14;
			nPC5.velocity = velocity5;
			if (nPC5.target != num181)
			{
				nPC5.targetSetFrame = Main.EverLastingTicker;
			}
			nPC5.target = num181;
			nPC5.direction = (bitsByte30[0] ? 1 : (-1));
			nPC5.directionY = (bitsByte30[1] ? 1 : (-1));
			nPC5.spriteDirection = (bitsByte30[6] ? 1 : (-1));
			if (bitsByte30[7])
			{
				num184 = nPC5.lifeMax;
			}
			if (num184 <= 0)
			{
				nPC5.life = num184;
				nPC5.active = false;
			}
			else
			{
				NPC.GetPendingDamage(nPC5, out var damage4, out var phaseChange);
				if (!phaseChange)
				{
					nPC5.life = num184 - damage4;
					for (int num186 = 0; num186 < NPC.maxAI; num186++)
					{
						nPC5.ai[num186] = array2[num186];
					}
				}
			}
			if (num181 == 65535 && nPC5.active)
			{
				nPC5.target = 0;
				Invariant.Assert(condition: false, "npc ({0}) had invalid target -1", nPC5);
			}
			nPC5.SpawnedFromStatue = bitsByte31[1];
			if (nPC5.SpawnedFromStatue)
			{
				nPC5.value = 0f;
			}
			if (bitsByte31[4])
			{
				nPC5.shimmerTransparency = 1f;
			}
			if (num185 > -1)
			{
				nPC5.TransformVisuals(num185, nPC5.type);
			}
			if (nPC5.type >= 0 && nPC5.type < NPCID.Count && Main.npcCatchable[nPC5.type])
			{
				nPC5.releaseOwner = reader.ReadByte();
			}
			if (flag19)
			{
				nPC5.OnSpawn(new EntitySource_Sync());
			}
			break;
		}
		case 24:
			Invariant.Assert(condition: false, "UnusedMeleeStrike");
			break;
		case 27:
		{
			ProjectileKey key2 = (ProjectileKey)reader.ReadInt32();
			Vector2 position3 = reader.ReadVector2();
			Vector2 velocity4 = reader.ReadVector2();
			int num164 = reader.ReadInt16();
			BitsByte bitsByte28 = reader.ReadByte();
			BitsByte bitsByte29 = (byte)(bitsByte28[2] ? reader.ReadByte() : 0);
			float[] array = ReUseTemporaryProjectileAI();
			array[0] = (bitsByte28[0] ? reader.ReadSingle() : 0f);
			array[1] = (bitsByte28[1] ? reader.ReadSingle() : 0f);
			int bannerIdToRespondTo = (bitsByte28[3] ? reader.ReadUInt16() : 0);
			int damage3 = (bitsByte28[4] ? reader.ReadInt16() : 0);
			float knockBack2 = (bitsByte28[5] ? reader.ReadSingle() : 0f);
			int originalDamage = (bitsByte28[6] ? reader.ReadInt16() : 0);
			array[2] = (bitsByte29[0] ? reader.ReadSingle() : 0f);
			if (Main.netMode == 2 && (Main.projHostile[num164] || key2.Spawner != whoAmI))
			{
				break;
			}
			bool flag18 = false;
			if (!key2.TryGet(out var proj2))
			{
				flag18 = true;
				proj2 = Projectile.NewProjectileSetup(key2);
				proj2.SetDefaults(num164);
				if (Main.netMode == 2)
				{
					Netplay.Clients[whoAmI].SpamProjectile++;
				}
			}
			else if (num164 != proj2.type)
			{
				proj2.SetDefaults(num164);
			}
			proj2.owner = key2.Spawner;
			proj2.position = position3;
			proj2.velocity = velocity4;
			proj2.type = num164;
			proj2.damage = damage3;
			proj2.bannerIdToRespondTo = bannerIdToRespondTo;
			proj2.originalDamage = originalDamage;
			proj2.knockBack = knockBack2;
			for (int num165 = 0; num165 < Projectile.maxAI; num165++)
			{
				proj2.ai[num165] = array[num165];
			}
			if (flag18)
			{
				proj2.FinalizeProjectile();
			}
			if (Main.netMode == 2)
			{
				NetMessage.TrySendData(27, -1, whoAmI, null, proj2.whoAmI);
			}
			break;
		}
		case 28:
		{
			if (Main.netMode == 2)
			{
				NetMessage.TrySendData(162, whoAmI);
			}
			int num246 = reader.ReadByte();
			int num247 = reader.ReadByte();
			int num248 = reader.ReadInt16();
			float num249 = reader.ReadSingle();
			int num250 = reader.ReadByte() - 1;
			byte b18 = reader.ReadByte();
			NPC nPC8 = Main.npc[num246];
			if (Main.netMode == 2)
			{
				if (nPC8.generation != num247)
				{
					break;
				}
				if (num248 < 0)
				{
					num248 = 0;
				}
				nPC8.PlayerInteraction(whoAmI);
			}
			else
			{
				Invariant.Assert(nPC8.generation == num247, "NPC Generation desync slot: {0} type: {1}", num246, num247);
			}
			if (num248 >= 0)
			{
				nPC8.StrikeNPC(num248, num249, num250, b18 == 1, fromNet: true, (Main.netMode == 2) ? whoAmI : 255);
			}
			else
			{
				nPC8.life = 0;
				nPC8.HitEffect();
				nPC8.active = false;
			}
			if (Main.netMode == 2)
			{
				NetMessage.TrySendData(28, -1, whoAmI, null, num246, num248, num249, num250, b18);
				if (nPC8.life <= 0)
				{
					NetMessage.TrySendData(23, -1, -1, null, num246);
				}
				if (nPC8.realLife >= 0 && Main.npc[nPC8.realLife].life <= 0)
				{
					NetMessage.TrySendData(23, -1, -1, null, nPC8.realLife);
				}
			}
			break;
		}
		case 162:
			if (Main.netMode == 1)
			{
				NPC.AckDamage();
			}
			break;
		case 29:
		{
			ProjectileKey projectileKey = (ProjectileKey)reader.ReadInt32();
			Vector2 val10 = reader.ReadVector2();
			if (projectileKey.TryGet(out var proj) && proj.active)
			{
				if (Main.netMode == 2 && proj.owner != whoAmI)
				{
					break;
				}
				if (!float.IsInfinity(val10.X) && !float.IsNaN(val10.X) && !float.IsInfinity(val10.Y) && !float.IsNaN(val10.Y))
				{
					proj.position = val10;
					proj.Kill();
				}
				else
				{
					proj.active = false;
				}
			}
			if (Main.netMode == 2)
			{
				NetMessage.TrySendData(29, -1, whoAmI, null, projectileKey, val10.X, val10.Y);
			}
			break;
		}
		case 30:
		{
			int num51 = reader.ReadByte();
			if (Main.netMode == 2)
			{
				num51 = whoAmI;
			}
			bool flag6 = reader.ReadBoolean();
			Main.player[num51].hostile = flag6;
			if (Main.netMode == 2)
			{
				NetMessage.TrySendData(30, -1, whoAmI, null, num51);
				LocalizedText obj = (flag6 ? Lang.mp[11] : Lang.mp[12]);
				ChatHelper.BroadcastChatMessage(color: Main.teamColor[Main.player[num51].team], text: NetworkText.FromKey(obj.Key, Main.player[num51].name));
			}
			break;
		}
		case 31:
		{
			if (Main.netMode != 2)
			{
				break;
			}
			int num261 = reader.ReadInt16();
			int num262 = reader.ReadInt16();
			int num263 = Chest.FindChest(num261, num262);
			if (num263 > -1 && Chest.UsingChest(num263) == -1)
			{
				NetMessage.SendChestContentsTo(num263, whoAmI);
				NetMessage.TrySendData(33, whoAmI, -1, null, num263);
				Main.player[whoAmI].chest = num263;
				if (Main.myPlayer == whoAmI)
				{
					Main.PipsUseGrid = false;
				}
				NetMessage.TrySendData(80, -1, whoAmI, null, whoAmI, num263);
				if (Main.netMode == 2 && WorldGen.IsChestRigged(num261, num262))
				{
					Wiring.SetCurrentUser(whoAmI);
					Wiring.HitSwitch(num261, num262);
					Wiring.SetCurrentUser();
					NetMessage.TrySendData(59, -1, whoAmI, null, num261, num262);
				}
			}
			break;
		}
		case 32:
		{
			int num155 = reader.ReadInt16();
			int num156 = reader.ReadByte();
			int stack7 = reader.ReadInt16();
			int prefixWeWant3 = reader.ReadByte();
			int type15 = reader.ReadInt16();
			if (num155 >= 0 && num155 < 8000 && Main.chest[num155] != null)
			{
				if (Main.chest[num155].item[num156] == null)
				{
					Main.chest[num155].item[num156] = new Item();
				}
				Main.chest[num155].item[num156].SetDefaults(type15);
				Main.chest[num155].item[num156].Prefix(prefixWeWant3);
				Main.chest[num155].item[num156].stack = stack7;
				if (Main.netMode == 2)
				{
					NetMessage.TrySendData(32, -1, whoAmI, null, num155, num156);
				}
			}
			break;
		}
		case 33:
		{
			int num21 = reader.ReadInt16();
			int num22 = reader.ReadInt16();
			int num23 = reader.ReadInt16();
			int num24 = reader.ReadByte();
			string name = string.Empty;
			if (num24 != 0)
			{
				if (num24 <= 20)
				{
					name = reader.ReadString();
				}
				else if (num24 != 255)
				{
					num24 = 0;
				}
			}
			if (Main.netMode == 1)
			{
				Player player = Main.player[Main.myPlayer];
				if (player.chest == -1)
				{
					Main.playerInventory = true;
					SoundEngine.PlaySound(10);
					if (num21 != -1)
					{
						ItemSlot.SetGlowForChest(Main.chest[num21]);
					}
				}
				else if (player.chest != num21 && num21 != -1)
				{
					Main.playerInventory = true;
					SoundEngine.PlaySound(12);
					Main.PipsUseGrid = false;
					ItemSlot.SetGlowForChest(Main.chest[num21]);
				}
				else if (player.chest != -1 && num21 == -1)
				{
					SoundEngine.PlaySound(11);
					Main.PipsUseGrid = false;
				}
				player.chest = num21;
				player.chestX = num22;
				player.chestY = num23;
				if (Main.tile[num22, num23].frameX >= 36 && Main.tile[num22, num23].frameX < 72)
				{
					AchievementsHelper.HandleSpecialEvent(Main.player[Main.myPlayer], 16);
				}
			}
			else
			{
				if (num24 != 0)
				{
					int chest3 = Main.player[whoAmI].chest;
					Chest chest4 = Main.chest[chest3];
					chest4.name = name;
					NetMessage.TrySendData(69, -1, whoAmI, null, chest3, chest4.x, chest4.y);
				}
				Main.player[whoAmI].chest = num21;
				NetMessage.TrySendData(80, -1, whoAmI, null, whoAmI, num21);
			}
			break;
		}
		case 34:
		{
			byte b4 = reader.ReadByte();
			int num32 = reader.ReadInt16();
			int num33 = reader.ReadInt16();
			int num34 = reader.ReadInt16();
			int num35 = reader.ReadInt16();
			if (Main.netMode == 2)
			{
				num35 = 0;
			}
			if (Main.netMode == 2)
			{
				using (Item.DefaultAssignNewItemsToPlayer(whoAmI))
				{
					switch (b4)
					{
					case 0:
					{
						int num38 = WorldGen.PlaceChest(num32, num33, 21, notNearOtherChests: false, num34);
						if (num38 == -1)
						{
							NetMessage.TrySendData(34, whoAmI, -1, null, b4, num32, num33, num34, num38);
							int itemDrop_Chests2 = WorldGen.GetItemDrop_Chests(num34, secondType: false);
							if (itemDrop_Chests2 > 0)
							{
								Item.NewItem(new EntitySource_TileBreak(num32, num33), num32 * 16, num33 * 16, 32, 32, itemDrop_Chests2, 1, noBroadcast: true);
							}
						}
						else
						{
							NetMessage.TrySendData(34, -1, -1, null, b4, num32, num33, num34, num38);
						}
						break;
					}
					case 1:
						if (Main.tile[num32, num33].type == 21)
						{
							Tile tile = Main.tile[num32, num33];
							if (tile.frameX % 36 != 0)
							{
								num32--;
							}
							if (tile.frameY % 36 != 0)
							{
								num33--;
							}
							int number = Chest.FindChest(num32, num33);
							WorldGen.KillTile(num32, num33);
							if (!tile.active())
							{
								NetMessage.TrySendData(34, -1, -1, null, b4, num32, num33, 0f, number);
							}
							break;
						}
						goto default;
					default:
						switch (b4)
						{
						case 2:
						{
							int num36 = WorldGen.PlaceChest(num32, num33, 88, notNearOtherChests: false, num34);
							if (num36 == -1)
							{
								NetMessage.TrySendData(34, whoAmI, -1, null, b4, num32, num33, num34, num36);
								Item.NewItem(new EntitySource_TileBreak(num32, num33), num32 * 16, num33 * 16, 32, 32, WorldGen.GetItemDrop_Dressers(num34), 1, noBroadcast: true);
							}
							else
							{
								NetMessage.TrySendData(34, -1, -1, null, b4, num32, num33, num34, num36);
							}
							break;
						}
						case 3:
							if (Main.tile[num32, num33].type == 88)
							{
								Tile tile2 = Main.tile[num32, num33];
								num32 -= tile2.frameX % 54 / 18;
								if (tile2.frameY % 36 != 0)
								{
									num33--;
								}
								int number2 = Chest.FindChest(num32, num33);
								WorldGen.KillTile(num32, num33);
								if (!tile2.active())
								{
									NetMessage.TrySendData(34, -1, -1, null, b4, num32, num33, 0f, number2);
								}
								break;
							}
							goto default;
						default:
							switch (b4)
							{
							case 4:
							{
								int num37 = WorldGen.PlaceChest(num32, num33, 467, notNearOtherChests: false, num34);
								if (num37 == -1)
								{
									NetMessage.TrySendData(34, whoAmI, -1, null, b4, num32, num33, num34, num37);
									int itemDrop_Chests = WorldGen.GetItemDrop_Chests(num34, secondType: true);
									if (itemDrop_Chests > 0)
									{
										Item.NewItem(new EntitySource_TileBreak(num32, num33), num32 * 16, num33 * 16, 32, 32, itemDrop_Chests, 1, noBroadcast: true);
									}
								}
								else
								{
									NetMessage.TrySendData(34, -1, -1, null, b4, num32, num33, num34, num37);
								}
								break;
							}
							case 5:
								if (Main.tile[num32, num33].type == 467)
								{
									Tile tile3 = Main.tile[num32, num33];
									if (tile3.frameX % 36 != 0)
									{
										num32--;
									}
									if (tile3.frameY % 36 != 0)
									{
										num33--;
									}
									int number3 = Chest.FindChest(num32, num33);
									WorldGen.KillTile(num32, num33);
									if (!tile3.active())
									{
										NetMessage.TrySendData(34, -1, -1, null, b4, num32, num33, 0f, number3);
									}
								}
								break;
							}
							break;
						}
						break;
					}
					break;
				}
			}
			switch (b4)
			{
			case 0:
				if (num35 == -1)
				{
					WorldGen.KillTile(num32, num33);
					break;
				}
				SoundEngine.PlaySound(0, num32 * 16, num33 * 16);
				WorldGen.PlaceChestDirect(num32, num33, 21, num34, num35);
				break;
			case 2:
				if (num35 == -1)
				{
					WorldGen.KillTile(num32, num33);
					break;
				}
				SoundEngine.PlaySound(0, num32 * 16, num33 * 16);
				WorldGen.PlaceDresserDirect(num32, num33, 88, num34, num35);
				break;
			case 4:
				if (num35 == -1)
				{
					WorldGen.KillTile(num32, num33);
					break;
				}
				SoundEngine.PlaySound(0, num32 * 16, num33 * 16);
				WorldGen.PlaceChestDirect(num32, num33, 467, num34, num35);
				break;
			default:
				Chest.DestroyChestDirect(num32, num33, num35);
				WorldGen.KillTile(num32, num33);
				break;
			}
			break;
		}
		case 35:
		{
			int num173 = reader.ReadByte();
			if (Main.netMode == 2)
			{
				num173 = whoAmI;
			}
			int num174 = reader.ReadInt16();
			if (num173 != Main.myPlayer || Main.ServerSideCharacter)
			{
				Main.player[num173].HealEffect(num174);
			}
			if (Main.netMode == 2)
			{
				NetMessage.TrySendData(35, -1, whoAmI, null, num173, num174);
			}
			break;
		}
		case 36:
		{
			int num133 = reader.ReadByte();
			if (Main.netMode == 2)
			{
				num133 = whoAmI;
			}
			Player player16 = Main.player[num133];
			bool flag15 = player16.zone5[0];
			player16.zone1 = reader.ReadByte();
			player16.zone2 = reader.ReadByte();
			player16.zone3 = reader.ReadByte();
			player16.zone4 = reader.ReadByte();
			player16.zone5 = reader.ReadByte();
			player16.townNPCs = reader.ReadByte();
			if (Main.netMode == 2)
			{
				if (!flag15 && player16.zone5[0])
				{
					NPC.Spawner.SpawnFaelings(player16);
				}
				NetMessage.TrySendData(36, -1, whoAmI, null, num133);
			}
			break;
		}
		case 37:
			if (Main.netMode == 1)
			{
				if (Main.autoPass)
				{
					NetMessage.TrySendData(38);
					Main.autoPass = false;
				}
				else
				{
					Netplay.ServerPassword = "";
					Main.menuMode = 31;
				}
			}
			break;
		case 38:
			if (Main.netMode == 2)
			{
				if (reader.ReadString() == Netplay.ServerPassword)
				{
					Netplay.Clients[whoAmI].State = 1;
					NetMessage.TrySendData(3, whoAmI);
				}
				else
				{
					NetMessage.TrySendData(2, whoAmI, -1, Lang.mp[1].ToNetworkText());
				}
			}
			break;
		case 39:
		{
			int num60 = reader.ReadInt16();
			WorldItem worldItem3 = Main.item[num60];
			bool forceAssignToServer = reader.ReadBoolean();
			if (Main.netMode == 1)
			{
				if (worldItem3.playerIndexTheItemIsReservedFor == Main.myPlayer)
				{
					worldItem3.FindOwner(forceAssignToServer: true);
				}
			}
			else if (worldItem3.playerIndexTheItemIsReservedFor == whoAmI)
			{
				worldItem3.timeSinceTheItemHasBeenReservedForSomeone = 0;
				worldItem3.playerIndexTheItemIsReservedFor = 255;
				worldItem3.FindOwner(forceAssignToServer);
				if (worldItem3.playerIndexTheItemIsReservedFor == 255)
				{
					NetMessage.TrySendData(22, -1, whoAmI, null, num60);
				}
			}
			break;
		}
		case 40:
		{
			int num52 = reader.ReadByte();
			if (Main.netMode == 2)
			{
				num52 = whoAmI;
			}
			int talkNPC = reader.ReadInt16();
			Main.player[num52].SetTalkNPC(talkNPC);
			if (Main.netMode == 2)
			{
				NetMessage.TrySendData(40, -1, whoAmI, null, num52);
			}
			break;
		}
		case 41:
		{
			int num30 = reader.ReadByte();
			if (Main.netMode == 2)
			{
				num30 = whoAmI;
			}
			Player player3 = Main.player[num30];
			float itemRotation = reader.ReadSingle();
			int itemAnimation = reader.ReadInt16();
			player3.itemRotation = itemRotation;
			player3.itemAnimation = itemAnimation;
			player3.channel = player3.inventory[player3.selectedItem].channel;
			if (Main.netMode == 2)
			{
				NetMessage.TrySendData(41, -1, whoAmI, null, num30);
			}
			break;
		}
		case 42:
		{
			int num251 = reader.ReadByte();
			if (Main.netMode == 2)
			{
				num251 = whoAmI;
			}
			else if (Main.myPlayer == num251 && !Main.ServerSideCharacter)
			{
				break;
			}
			int statMana = reader.ReadInt16();
			int statManaMax = reader.ReadInt16();
			Main.player[num251].statMana = statMana;
			Main.player[num251].statManaMax = statManaMax;
			break;
		}
		case 43:
		{
			int num187 = reader.ReadByte();
			if (Main.netMode == 2)
			{
				num187 = whoAmI;
			}
			int num188 = reader.ReadInt16();
			if (num187 != Main.myPlayer)
			{
				Main.player[num187].ManaEffect(num188);
			}
			if (Main.netMode == 2)
			{
				NetMessage.TrySendData(43, -1, whoAmI, null, num187, num188);
			}
			break;
		}
		case 45:
		case 157:
		{
			int num124 = reader.ReadByte();
			if (Main.netMode == 2)
			{
				num124 = whoAmI;
			}
			int num125 = reader.ReadByte();
			Player player14 = Main.player[num124];
			int team = player14.team;
			player14.team = num125;
			Color color = Main.teamColor[num125];
			if (Main.netMode != 2)
			{
				break;
			}
			NetMessage.TrySendData(45, -1, whoAmI, null, num124);
			LocalizedText localizedText = Lang.mp[13 + num125];
			if (num125 == 5)
			{
				localizedText = Lang.mp[22];
			}
			for (int num126 = 0; num126 < 255; num126++)
			{
				if (num126 == whoAmI || (team > 0 && Main.player[num126].team == team) || (num125 > 0 && Main.player[num126].team == num125))
				{
					ChatHelper.SendChatMessageToClient(NetworkText.FromKey(localizedText.Key, player14.name), color, num126);
				}
			}
			if (b == 157 && Main.teamBasedSpawnsSeed)
			{
				Point spawnPoint = Point.Zero;
				if (ExtraSpawnPointManager.TryGetExtraSpawnPointForTeam(num125, out spawnPoint))
				{
					RemoteClient.CheckSection(whoAmI, spawnPoint.ToWorldCoordinates());
					NetMessage.SendData(158, num124, -1, null, num124);
				}
			}
			break;
		}
		case 46:
			if (Main.netMode == 2)
			{
				short i3 = reader.ReadInt16();
				int j3 = reader.ReadInt16();
				int num122 = Sign.ReadSign(i3, j3);
				if (num122 >= 0)
				{
					NetMessage.TrySendData(47, whoAmI, -1, null, num122, whoAmI);
				}
			}
			break;
		case 47:
		{
			int num58 = reader.ReadInt16();
			int x3 = reader.ReadInt16();
			int y3 = reader.ReadInt16();
			string text2 = reader.ReadString();
			int num59 = reader.ReadByte();
			BitsByte bitsByte5 = reader.ReadByte();
			if (num58 >= 0 && num58 < 32000)
			{
				string text3 = null;
				if (Main.sign[num58] != null)
				{
					text3 = Main.sign[num58].text;
				}
				Main.sign[num58] = new Sign();
				Main.sign[num58].x = x3;
				Main.sign[num58].y = y3;
				Sign.TextSign(num58, text2);
				if (Main.netMode == 2 && text3 != text2)
				{
					num59 = whoAmI;
					NetMessage.TrySendData(47, -1, whoAmI, null, num58, num59);
				}
				if (Main.netMode == 1 && num59 == Main.myPlayer && Main.sign[num58] != null && !bitsByte5[0])
				{
					Main.LocalPlayer.OpenSign(num58);
				}
			}
			break;
		}
		case 48:
		{
			int num3 = reader.ReadInt16();
			int num4 = reader.ReadInt16();
			byte b2 = reader.ReadByte();
			byte liquidType = reader.ReadByte();
			if (Main.netMode == 2 && Netplay.SpamCheck)
			{
				int num5 = whoAmI;
				int num6 = (int)(Main.player[num5].position.X + (float)(Main.player[num5].width / 2));
				int num7 = (int)(Main.player[num5].position.Y + (float)(Main.player[num5].height / 2));
				int num8 = 10;
				int num9 = num6 - num8;
				int num10 = num6 + num8;
				int num11 = num7 - num8;
				int num12 = num7 + num8;
				if (num3 < num9 || num3 > num10 || num4 < num11 || num4 > num12)
				{
					Netplay.Clients[whoAmI].SpamWater++;
				}
			}
			if (Main.tile[num3, num4] == null)
			{
				Main.tile[num3, num4] = new Tile();
			}
			lock (Main.tile[num3, num4])
			{
				Main.tile[num3, num4].liquid = b2;
				Main.tile[num3, num4].liquidType(liquidType);
				if (Main.netMode == 2)
				{
					WorldGen.SquareTileFrame(num3, num4);
					if (b2 == 0)
					{
						NetMessage.SendData(48, -1, whoAmI, null, num3, num4);
					}
				}
				break;
			}
		}
		case 49:
			if (Netplay.Connection.State == 6)
			{
				Netplay.Connection.State = 10;
				Main.player[Main.myPlayer].Spawn(PlayerSpawnContext.SpawningIntoWorld);
			}
			break;
		case 50:
		{
			int num234 = reader.ReadByte();
			if (Main.netMode == 2)
			{
				num234 = whoAmI;
			}
			else if (num234 == Main.myPlayer && !Main.ServerSideCharacter)
			{
				break;
			}
			Player player19 = Main.player[num234];
			int num235 = 0;
			int num236;
			while ((num236 = reader.ReadUInt16()) > 0)
			{
				player19.buffType[num235] = num236;
				player19.buffTime[num235] = 60;
				num235++;
			}
			Array.Clear(player19.buffType, num235, player19.buffType.Length - num235);
			Array.Clear(player19.buffTime, num235, player19.buffTime.Length - num235);
			if (Main.netMode == 2)
			{
				NetMessage.TrySendData(50, -1, whoAmI, null, num234);
			}
			break;
		}
		case 51:
		{
			byte b16 = reader.ReadByte();
			if (Main.netMode == 2)
			{
				b16 = (byte)whoAmI;
			}
			byte b17 = reader.ReadByte();
			switch (b17)
			{
			case 1:
				NPC.SpawnSkeletron(b16);
				break;
			case 2:
				if (Main.netMode == 2)
				{
					NetMessage.TrySendData(51, -1, whoAmI, null, b16, (int)b17);
				}
				else
				{
					SoundEngine.PlaySound(SoundID.Item1, (int)Main.player[b16].position.X, (int)Main.player[b16].position.Y);
				}
				break;
			case 3:
				if (Main.netMode == 2)
				{
					Main.Sundialing();
				}
				break;
			case 4:
				Main.npc[b16].BigMimicSpawnSmoke();
				break;
			case 5:
				if (Main.netMode == 2)
				{
					NPC nPC7 = new NPC();
					nPC7.SetDefaults(664);
					Main.BestiaryTracker.Kills.RegisterKill(nPC7);
				}
				break;
			case 6:
				if (Main.netMode == 2)
				{
					Main.Moondialing();
				}
				break;
			}
			break;
		}
		case 52:
		{
			int num139 = reader.ReadByte();
			int num140 = reader.ReadInt16();
			int num141 = reader.ReadInt16();
			if (num139 == 1)
			{
				Chest.Unlock(num140, num141);
				if (Main.netMode == 2)
				{
					NetMessage.TrySendData(52, -1, whoAmI, null, 0, num139, num140, num141);
					NetMessage.SendTileSquare(-1, num140, num141, 2);
				}
			}
			if (num139 == 2)
			{
				WorldGen.UnlockDoor(num140, num141);
				if (Main.netMode == 2)
				{
					NetMessage.TrySendData(52, -1, whoAmI, null, 0, num139, num140, num141);
					NetMessage.SendTileSquare(-1, num140, num141, 2);
				}
			}
			if (num139 == 3)
			{
				Chest.Lock(num140, num141);
				if (Main.netMode == 2)
				{
					NetMessage.TrySendData(52, -1, whoAmI, null, 0, num139, num140, num141);
					NetMessage.SendTileSquare(-1, num140, num141, 2);
				}
			}
			break;
		}
		case 53:
		{
			int num138 = reader.ReadInt16();
			int type13 = reader.ReadUInt16();
			int time2 = reader.ReadInt16();
			Main.npc[num138].AddBuff(type13, time2, quiet: true);
			if (Main.netMode == 2)
			{
				NetMessage.TrySendData(54, -1, -1, null, num138);
			}
			break;
		}
		case 54:
			if (Main.netMode == 1)
			{
				int num127 = reader.ReadInt16();
				NPC nPC2 = Main.npc[num127];
				int num128 = 0;
				int num129;
				while ((num129 = reader.ReadUInt16()) > 0)
				{
					nPC2.buffType[num128] = num129;
					nPC2.buffTime[num128] = reader.ReadUInt16();
					num128++;
				}
				Array.Clear(nPC2.buffType, num128, nPC2.buffType.Length - num128);
				Array.Clear(nPC2.buffTime, num128, nPC2.buffTime.Length - num128);
			}
			break;
		case 55:
		{
			int num80 = reader.ReadByte();
			int num81 = reader.ReadUInt16();
			int num82 = reader.ReadInt32();
			if ((Main.netMode != 2 || (Main.player[num80].hostile && Main.player[whoAmI].hostile && Main.pvpBuff[num81])) && (Main.netMode != 1 || num80 == Main.myPlayer))
			{
				if (Main.netMode == 2)
				{
					NetMessage.TrySendData(55, num80, -1, null, num80, num81, num82);
				}
				else
				{
					Main.player[num80].AddBuff(num81, num82, fromNetPvP: true);
				}
			}
			break;
		}
		case 56:
		{
			int num55 = reader.ReadInt16();
			if (num55 >= 0 && num55 < Main.maxNPCs)
			{
				if (Main.netMode == 1)
				{
					string givenName = reader.ReadString();
					Main.npc[num55].GivenName = givenName;
					int townNpcVariationIndex = reader.ReadInt32();
					Main.npc[num55].townNpcVariationIndex = townNpcVariationIndex;
				}
				else if (Main.netMode == 2)
				{
					NetMessage.TrySendData(56, whoAmI, -1, null, num55);
				}
			}
			break;
		}
		case 57:
			if (Main.netMode == 1)
			{
				WorldGen.tGood = reader.ReadByte();
				WorldGen.tEvil = reader.ReadByte();
				WorldGen.tBlood = reader.ReadByte();
			}
			break;
		case 58:
		{
			int num39 = reader.ReadByte();
			if (Main.netMode == 2)
			{
				num39 = whoAmI;
			}
			float num40 = reader.ReadSingle();
			if (Main.netMode == 2)
			{
				NetMessage.TrySendData(58, -1, whoAmI, null, whoAmI, num40);
				break;
			}
			Player player4 = Main.player[num39];
			int type3 = player4.inventory[player4.selectedItem].type;
			switch (type3)
			{
			case 4057:
			case 4372:
			case 4715:
				player4.PlayGuitarChord(num40);
				break;
			case 4673:
				player4.PlayDrums(num40);
				break;
			default:
			{
				Main.musicPitch = num40;
				LegacySoundStyle type4 = SoundID.Item26;
				if (type3 == 507)
				{
					type4 = SoundID.Item35;
				}
				if (type3 == 1305)
				{
					type4 = SoundID.Item47;
				}
				SoundEngine.PlaySound(type4, player4.position);
				break;
			}
			}
			break;
		}
		case 59:
		{
			int num53 = reader.ReadInt16();
			int num54 = reader.ReadInt16();
			Wiring.SetCurrentUser(whoAmI);
			Wiring.HitSwitch(num53, num54);
			Wiring.SetCurrentUser();
			if (Main.netMode == 2)
			{
				NetMessage.TrySendData(59, -1, whoAmI, null, num53, num54);
			}
			break;
		}
		case 60:
		{
			int num267 = reader.ReadInt16();
			int num268 = reader.ReadInt16();
			int num269 = reader.ReadInt16();
			byte b19 = reader.ReadByte();
			if (num267 >= Main.maxNPCs)
			{
				NetMessage.BootPlayer(whoAmI, NetworkText.FromKey("Net.CheatingInvalid"));
				break;
			}
			NPC nPC9 = Main.npc[num267];
			bool isLikeATownNPC = nPC9.isLikeATownNPC;
			if (Main.netMode == 1)
			{
				nPC9.homeless = b19 == 1;
				nPC9.homeTileX = num268;
				nPC9.homeTileY = num269;
			}
			if (!isLikeATownNPC)
			{
				break;
			}
			if (Main.netMode == 1)
			{
				switch (b19)
				{
				case 1:
					WorldGen.TownManager.KickOut(nPC9.type);
					break;
				case 2:
					WorldGen.TownManager.SetRoom(nPC9.type, num268, num269);
					break;
				}
			}
			else if (b19 == 1)
			{
				WorldGen.kickOut(num267);
			}
			else
			{
				WorldGen.moveRoom(num268, num269, num267);
			}
			break;
		}
		case 61:
		{
			int num243 = reader.ReadInt16();
			int num244 = reader.ReadInt16();
			if (Main.netMode != 2)
			{
				break;
			}
			if (num244 >= 0 && num244 < NPCID.Count && NPCID.Sets.MPAllowedEnemies[num244])
			{
				if (!NPC.AnyNPCs(num244))
				{
					NPC.SpawnOnPlayer(num243, num244);
				}
			}
			else if (num244 == -4)
			{
				if (!Main.dayTime && !DD2Event.Ongoing)
				{
					ChatHelper.BroadcastChatMessage(NetworkText.FromKey(Lang.misc[31].Key), ChatColors.World);
					Main.startPumpkinMoon();
					NetMessage.TrySendData(7);
					NetMessage.TrySendData(78, -1, -1, null, 0, 1f, 2f, 1f);
				}
			}
			else if (num244 == -5)
			{
				if (!Main.dayTime && !DD2Event.Ongoing)
				{
					ChatHelper.BroadcastChatMessage(NetworkText.FromKey(Lang.misc[34].Key), ChatColors.World);
					Main.startSnowMoon();
					NetMessage.TrySendData(7);
					NetMessage.TrySendData(78, -1, -1, null, 0, 1f, 1f, 1f);
				}
			}
			else if (num244 == -6)
			{
				if (Main.dayTime && !Main.eclipse)
				{
					if (Main.remixWorld)
					{
						ChatHelper.BroadcastChatMessage(NetworkText.FromKey(Lang.misc[106].Key), ChatColors.World);
					}
					else
					{
						ChatHelper.BroadcastChatMessage(NetworkText.FromKey(Lang.misc[20].Key), ChatColors.World);
					}
					Main.eclipse = true;
					NetMessage.TrySendData(7);
				}
			}
			else if (num244 == -7)
			{
				Main.invasionDelay = 0;
				Main.StartInvasion(4);
				NetMessage.TrySendData(7);
				NetMessage.TrySendData(78, -1, -1, null, 0, 1f, Main.invasionType + 3);
			}
			else if (num244 == -8)
			{
				if (NPC.downedGolemBoss && Main.hardMode && !NPC.AnyDanger() && !NPC.AnyoneNearCultists())
				{
					WorldGen.StartImpendingDoom(720);
					NetMessage.TrySendData(7);
				}
			}
			else if (num244 == -10)
			{
				if (!Main.dayTime && !Main.bloodMoon)
				{
					ChatHelper.BroadcastChatMessage(NetworkText.FromKey(Lang.misc[8].Key), ChatColors.World);
					Main.bloodMoon = true;
					if (Main.GetMoonPhase() == MoonPhase.Empty)
					{
						Main.moonPhase = 5;
					}
					AchievementsHelper.NotifyProgressionEvent(4);
					NetMessage.TrySendData(7);
				}
			}
			else if (num244 == -11)
			{
				ChatHelper.BroadcastChatMessage(NetworkText.FromKey("Misc.CombatBookUsed"), ChatColors.World);
				NPC.combatBookWasUsed = true;
				NetMessage.TrySendData(7);
			}
			else if (num244 == -12)
			{
				NPC.UnlockOrExchangePet(ref NPC.boughtCat, 637, "Misc.LicenseCatUsed", num244);
			}
			else if (num244 == -13)
			{
				NPC.UnlockOrExchangePet(ref NPC.boughtDog, 638, "Misc.LicenseDogUsed", num244);
			}
			else if (num244 == -14)
			{
				NPC.UnlockOrExchangePet(ref NPC.boughtBunny, 656, "Misc.LicenseBunnyUsed", num244);
			}
			else if (num244 == -15)
			{
				NPC.UnlockOrExchangePet(ref NPC.unlockedSlimeBlueSpawn, 670, "Misc.LicenseSlimeUsed", num244);
			}
			else if (num244 == -16)
			{
				NPC.SpawnMechQueen(num243);
			}
			else if (num244 == -17)
			{
				ChatHelper.BroadcastChatMessage(NetworkText.FromKey("Misc.CombatBookVolumeTwoUsed"), ChatColors.World);
				NPC.combatBookVolumeTwoWasUsed = true;
				NetMessage.TrySendData(7);
			}
			else if (num244 == -18)
			{
				ChatHelper.BroadcastChatMessage(NetworkText.FromKey("Misc.PeddlersSatchelUsed"), ChatColors.World);
				NPC.peddlersSatchelWasUsed = true;
				NetMessage.TrySendData(7);
			}
			else if (num244 == -19)
			{
				Main.StartSlimeRain();
			}
			else if (num244 < 0)
			{
				int num245 = 1;
				if (num244 > -InvasionID.Count)
				{
					num245 = -num244;
				}
				if (num245 > 0 && Main.invasionType == 0)
				{
					Main.invasionDelay = 0;
					Main.StartInvasion(num245);
				}
				NetMessage.TrySendData(7);
				NetMessage.TrySendData(78, -1, -1, null, 0, 1f, Main.invasionType + 3);
			}
			break;
		}
		case 62:
		{
			int num179 = reader.ReadByte();
			int num180 = reader.ReadByte();
			if (Main.netMode == 2)
			{
				num179 = whoAmI;
			}
			if (num180 == 1)
			{
				Main.player[num179].NinjaDodge();
			}
			if (num180 == 2)
			{
				Main.player[num179].ShadowDodge();
			}
			if (num180 == 4)
			{
				Main.player[num179].BrainOfConfusionDodge();
			}
			if (num180 == 5)
			{
				Main.player[num179].DoMysticSashDodge();
			}
			if (Main.netMode == 2)
			{
				NetMessage.TrySendData(62, -1, whoAmI, null, num179, num180);
			}
			break;
		}
		case 63:
		{
			int num147 = reader.ReadInt16();
			int num148 = reader.ReadInt16();
			byte b11 = reader.ReadByte();
			byte b12 = reader.ReadByte();
			if (b12 == 0)
			{
				WorldGen.paintTile(num147, num148, b11);
			}
			else
			{
				WorldGen.paintCoatTile(num147, num148, b11);
			}
			if (Main.netMode == 2)
			{
				NetMessage.TrySendData(63, -1, whoAmI, null, num147, num148, (int)b11, (int)b12);
			}
			break;
		}
		case 64:
		{
			int num130 = reader.ReadInt16();
			int num131 = reader.ReadInt16();
			byte b9 = reader.ReadByte();
			byte b10 = reader.ReadByte();
			if (b10 == 0)
			{
				WorldGen.paintWall(num130, num131, b9);
			}
			else
			{
				WorldGen.paintCoatWall(num130, num131, b9);
			}
			if (Main.netMode == 2)
			{
				NetMessage.TrySendData(64, -1, whoAmI, null, num130, num131, (int)b9, (int)b10);
			}
			break;
		}
		case 65:
		{
			BitsByte bitsByte18 = reader.ReadByte();
			int num84 = reader.ReadInt16();
			if (Main.netMode == 2)
			{
				num84 = whoAmI;
			}
			Vector2 val2 = reader.ReadVector2();
			int num85 = 0;
			num85 = reader.ReadByte();
			int num86 = 0;
			if (bitsByte18[0])
			{
				num86++;
			}
			if (bitsByte18[1])
			{
				num86 += 2;
			}
			bool flag9 = false;
			if (bitsByte18[2])
			{
				flag9 = true;
			}
			int num87 = 0;
			if (bitsByte18[3])
			{
				num87 = reader.ReadInt32();
			}
			if (flag9)
			{
				val2 = Main.player[num84].position;
			}
			switch (num86)
			{
			case 0:
				Main.player[num84].Teleport(val2, num85, num87);
				if (Main.netMode == 2)
				{
					NetMessage.TrySendData(65, -1, whoAmI, null, 0, num84, val2.X, val2.Y, num85, flag9.ToInt(), num87);
				}
				if (Main.netMode == 1 && num84 == Main.myPlayer)
				{
					NetMessage.TrySendData(65, -1, -1, null, 3, num84);
				}
				break;
			case 1:
			{
				Main.npc[num84].Teleport(val2, num85, num87);
				NPC obj3 = Main.npc[num84];
				obj3.netOffset *= 0f;
				break;
			}
			case 2:
			{
				Main.player[num84].Teleport(val2, num85, num87);
				if (Main.netMode != 2)
				{
					break;
				}
				RemoteClient.CheckSection(whoAmI, val2);
				NetMessage.TrySendData(65, -1, -1, null, 0, num84, val2.X, val2.Y, num85, flag9.ToInt(), num87);
				int num88 = -1;
				float num89 = 9999f;
				for (int num90 = 0; num90 < 255; num90++)
				{
					if (Main.player[num90].active && num90 != whoAmI)
					{
						Vector2 val3 = Main.player[num90].position - Main.player[whoAmI].position;
						if (val3.Length() < num89)
						{
							num89 = val3.Length();
							num88 = num90;
						}
					}
				}
				if (num88 >= 0)
				{
					ChatHelper.BroadcastChatMessage(NetworkText.FromKey("Game.HasTeleportedTo", Main.player[whoAmI].name, Main.player[num88].name), new Color(250, 250, 0));
				}
				break;
			}
			case 3:
				Invariant.Assert(Main.netMode == 2, "TeleportEntity player ack on client");
				Invariant.Assert(Main.player[num84].unacknowledgedTeleports-- >= 0, "TeleportEntity player acks > teleports");
				break;
			}
			break;
		}
		case 66:
		{
			int num70 = reader.ReadByte();
			int num71 = reader.ReadInt16();
			if (num71 > 0)
			{
				Player player8 = Main.player[num70];
				player8.statLife += num71;
				if (player8.statLife > player8.statLifeMax2)
				{
					player8.statLife = player8.statLifeMax2;
				}
				player8.HealEffect(num71, broadcast: false);
				if (Main.netMode == 2)
				{
					NetMessage.TrySendData(66, -1, whoAmI, null, num70, num71);
				}
			}
			break;
		}
		case 68:
			reader.ReadString();
			break;
		case 69:
		{
			int num18 = reader.ReadInt16();
			int num19 = reader.ReadInt16();
			int num20 = reader.ReadInt16();
			if (Main.netMode == 1)
			{
				if (num18 >= 0 && num18 < 8000)
				{
					Chest chest = Main.chest[num18];
					if (chest == null)
					{
						chest = Chest.CreateWorldChest(num18, num19, num20);
					}
					else if (chest.x != num19 || chest.y != num20)
					{
						break;
					}
					chest.name = reader.ReadString();
				}
			}
			else
			{
				if (num18 < -1 || num18 >= 8000)
				{
					break;
				}
				if (num18 == -1)
				{
					num18 = Chest.FindChest(num19, num20);
					if (num18 == -1)
					{
						break;
					}
				}
				Chest chest2 = Main.chest[num18];
				if (chest2.x == num19 && chest2.y == num20)
				{
					NetMessage.TrySendData(69, whoAmI, -1, null, num18, num19, num20);
				}
			}
			break;
		}
		case 70:
			if (Main.netMode == 2)
			{
				int num16 = reader.ReadInt16();
				if (num16 >= 0 && num16 < Main.maxNPCs)
				{
					NPC.CatchNPC(num16, whoAmI);
				}
			}
			break;
		case 71:
			if (Main.netMode == 2)
			{
				int x11 = reader.ReadInt32();
				int y11 = reader.ReadInt32();
				int type20 = reader.ReadInt16();
				byte style2 = reader.ReadByte();
				NPC.ReleaseNPC(x11, y11, type20, style2, whoAmI);
			}
			break;
		case 72:
			if (Main.netMode == 1)
			{
				for (int num264 = 0; num264 < Main.TravelShopMaxSlots; num264++)
				{
					Main.travelShop[num264] = reader.ReadInt16();
				}
			}
			break;
		case 73:
			switch (reader.ReadByte())
			{
			case 0:
				Main.player[whoAmI].TeleportationPotion();
				break;
			case 1:
				Main.player[whoAmI].MagicConch();
				break;
			case 2:
				Main.player[whoAmI].DemonConch();
				break;
			case 3:
				Main.player[whoAmI].Shellphone_Spawn();
				break;
			case 4:
				Main.player[whoAmI].PlayerNoSpaceTeleport();
				break;
			}
			break;
		case 74:
			if (Main.netMode == 1)
			{
				Main.anglerQuest = reader.ReadByte();
				Main.anglerQuestFinished = reader.ReadBoolean();
			}
			break;
		case 75:
			if (Main.netMode == 2)
			{
				string name2 = Main.player[whoAmI].name;
				if (!Main.anglerWhoFinishedToday.Contains(name2))
				{
					Main.anglerWhoFinishedToday.Add(name2);
				}
			}
			break;
		case 76:
		{
			int num196 = reader.ReadByte();
			if (num196 != Main.myPlayer || Main.ServerSideCharacter)
			{
				if (Main.netMode == 2)
				{
					num196 = whoAmI;
				}
				Player obj9 = Main.player[num196];
				obj9.anglerQuestsFinished = reader.ReadInt32();
				obj9.golferScoreAccumulated = reader.ReadInt32();
				if (Main.netMode == 2)
				{
					NetMessage.TrySendData(76, -1, whoAmI, null, num196);
				}
			}
			break;
		}
		case 77:
		{
			short type18 = reader.ReadInt16();
			ushort tileType = reader.ReadUInt16();
			short x9 = reader.ReadInt16();
			short y9 = reader.ReadInt16();
			Animation.NewTemporaryAnimation(type18, tileType, x9, y9);
			break;
		}
		case 78:
			if (Main.netMode == 1)
			{
				Main.ReportInvasionProgress(reader.ReadInt32(), reader.ReadInt32(), reader.ReadSByte(), reader.ReadSByte());
			}
			break;
		case 79:
		{
			int x8 = reader.ReadInt16();
			int y8 = reader.ReadInt16();
			short type17 = reader.ReadInt16();
			int style = reader.ReadInt16();
			int num171 = reader.ReadByte();
			int random = reader.ReadSByte();
			int direction = (reader.ReadBoolean() ? 1 : (-1));
			if (Main.netMode == 2)
			{
				Netplay.Clients[whoAmI].SpamAddBlock++;
				if (!WorldGen.InWorld(x8, y8, 10) || !Netplay.Clients[whoAmI].TileSections[Netplay.GetSectionX(x8), Netplay.GetSectionY(y8)])
				{
					break;
				}
			}
			WorldGen.PlaceObject(x8, y8, type17, mute: false, style, num171, random, direction);
			if (Main.netMode == 2)
			{
				NetMessage.SendObjectPlacement(whoAmI, x8, y8, type17, style, num171, random, direction);
			}
			break;
		}
		case 80:
			if (Main.netMode == 1)
			{
				int num157 = reader.ReadByte();
				int num158 = reader.ReadInt16();
				if (num158 >= -3 && num158 < 8000)
				{
					Main.player[num157].chest = num158;
				}
			}
			break;
		case 81:
			if (Main.netMode == 1)
			{
				int num149 = (int)reader.ReadSingle();
				int num150 = (int)reader.ReadSingle();
				CombatText.NewText(color: reader.ReadRGB(), amount: reader.ReadInt32(), location: new Rectangle(num149, num150, 0, 0));
			}
			break;
		case 119:
			if (Main.netMode == 1)
			{
				int num153 = (int)reader.ReadSingle();
				int num154 = (int)reader.ReadSingle();
				CombatText.NewText(color: reader.ReadRGB(), text: NetworkText.Deserialize(reader).ToString(), location: new Rectangle(num153, num154, 0, 0));
			}
			break;
		case 82:
			NetManager.Instance.Read(reader, whoAmI, length);
			break;
		case 84:
		{
			int num132 = reader.ReadByte();
			if (Main.netMode == 2)
			{
				num132 = whoAmI;
			}
			float stealth = reader.ReadSingle();
			Main.player[num132].stealth = stealth;
			if (Main.netMode == 2)
			{
				NetMessage.TrySendData(84, -1, whoAmI, null, num132);
			}
			break;
		}
		case 85:
			if (Main.netMode == 2 && whoAmI < 255)
			{
				Player player15 = Main.player[whoAmI];
				QuickStacking.SourceInventory inventory = QuickStacking.ReadNetInventory(player15, reader);
				bool smartStack = reader.ReadBoolean();
				QuickStacking.QuickStackToNearbyChests(player15, inventory, smartStack);
			}
			else if (Main.netMode == 1)
			{
				QuickStacking.IndicateBlockedChests(Main.LocalPlayer, QuickStacking.ReadBlockedChestList(reader));
			}
			break;
		case 86:
		{
			if (Main.netMode != 1)
			{
				break;
			}
			int num118 = reader.ReadInt32();
			if (!reader.ReadBoolean())
			{
				if (TileEntity.TryGet<TileEntity>(num118, out var result3))
				{
					TileEntity.Remove(result3);
				}
			}
			else
			{
				TileEntity tileEntity = TileEntity.Read(reader, 326, networkSend: true);
				tileEntity.ID = num118;
				TileEntity.Add(tileEntity);
			}
			break;
		}
		case 87:
			if (Main.netMode == 2)
			{
				int x6 = reader.ReadInt16();
				int y6 = reader.ReadInt16();
				int type10 = reader.ReadByte();
				if (WorldGen.InWorld(x6, y6) && !TileEntity.TryGetAt<TileEntity>(x6, y6, out var _))
				{
					TileEntity.PlaceEntityNet(x6, y6, type10);
				}
			}
			break;
		case 88:
		{
			if (Main.netMode != 1)
			{
				break;
			}
			int num2 = reader.ReadInt16();
			if (num2 < 0 || num2 > 400)
			{
				break;
			}
			Item inner = Main.item[num2].inner;
			BitsByte bitsByte = reader.ReadByte();
			if (bitsByte[0])
			{
				inner.color.PackedValue = reader.ReadUInt32();
			}
			if (bitsByte[1])
			{
				inner.damage = reader.ReadUInt16();
			}
			if (bitsByte[2])
			{
				inner.knockBack = reader.ReadSingle();
			}
			if (bitsByte[3])
			{
				inner.useAnimation = reader.ReadUInt16();
			}
			if (bitsByte[4])
			{
				inner.useTime = reader.ReadUInt16();
			}
			if (bitsByte[5])
			{
				inner.shoot = reader.ReadInt16();
			}
			if (bitsByte[6])
			{
				inner.shootSpeed = reader.ReadSingle();
			}
			if (bitsByte[7])
			{
				bitsByte = reader.ReadByte();
				if (bitsByte[0])
				{
					inner.width = reader.ReadInt16();
				}
				if (bitsByte[1])
				{
					inner.height = reader.ReadInt16();
				}
				if (bitsByte[2])
				{
					inner.scale = reader.ReadSingle();
				}
				if (bitsByte[3])
				{
					inner.ammo = reader.ReadInt16();
				}
				if (bitsByte[4])
				{
					inner.useAmmo = reader.ReadInt16();
				}
				if (bitsByte[5])
				{
					inner.notAmmo = reader.ReadBoolean();
				}
			}
			break;
		}
		case 89:
		{
			if (Main.netMode != 2)
			{
				break;
			}
			int x = reader.ReadInt16();
			int y = reader.ReadInt16();
			int type = reader.ReadInt16();
			int prefix = reader.ReadByte();
			int stack = reader.ReadInt16();
			using (Item.DefaultAssignNewItemsToPlayer(whoAmI))
			{
				TEItemFrame.TryPlacing(x, y, type, prefix, stack);
				break;
			}
		}
		case 91:
		{
			if (Main.netMode != 1)
			{
				break;
			}
			int num252 = reader.ReadInt32();
			int num253 = reader.ReadByte();
			if (num253 == 255)
			{
				if (EmoteBubble.byID.ContainsKey(num252))
				{
					EmoteBubble.byID.Remove(num252);
				}
				break;
			}
			int num254 = reader.ReadUInt16();
			int num255 = reader.ReadUInt16();
			int num256 = reader.ReadByte();
			int metadata = 0;
			if (num256 < 0)
			{
				metadata = reader.ReadInt16();
			}
			WorldUIAnchor worldUIAnchor = EmoteBubble.DeserializeNetAnchor(num253, num254);
			if (num253 == 1)
			{
				Main.player[num254].emoteTime = 360;
			}
			lock (EmoteBubble.byID)
			{
				if (!EmoteBubble.byID.ContainsKey(num252))
				{
					EmoteBubble.byID[num252] = new EmoteBubble(num256, worldUIAnchor, num255);
				}
				else
				{
					EmoteBubble.byID[num252].lifeTime = num255;
					EmoteBubble.byID[num252].lifeTimeStart = num255;
					EmoteBubble.byID[num252].emote = num256;
					EmoteBubble.byID[num252].anchor = worldUIAnchor;
				}
				EmoteBubble.byID[num252].ID = num252;
				EmoteBubble.byID[num252].metadata = metadata;
				EmoteBubble.OnBubbleChange(num252);
				break;
			}
		}
		case 92:
		{
			int num237 = reader.ReadInt16();
			int num238 = reader.ReadInt32();
			float num239 = reader.ReadSingle();
			float num240 = reader.ReadSingle();
			if (num237 >= 0 && num237 <= Main.maxNPCs)
			{
				if (Main.netMode == 1)
				{
					Main.npc[num237].moneyPing(new Vector2(num239, num240));
					Main.npc[num237].extraValue = num238;
				}
				else
				{
					Main.npc[num237].extraValue += num238;
					NetMessage.TrySendData(92, -1, -1, null, num237, Main.npc[num237].extraValue, num239, num240);
				}
			}
			break;
		}
		case 94:
		{
			string text5 = reader.ReadString();
			int num232 = reader.ReadInt32();
			int num233 = (int)reader.ReadSingle();
			reader.ReadSingle();
			if (!DebugOptions.enableDebugCommands)
			{
				break;
			}
			switch (text5)
			{
			case "/showdebug":
				DebugOptions.Shared_ReportCommandUsage = num233 == 1;
				break;
			case "/randomizeprojslots":
				DebugOptions.Shared_RandomizeProjectileSlots = num233 == 1;
				break;
			case "/setserverping":
				DebugOptions.Shared_ServerPing = num233;
				DebugNetworkStream.Latency = (uint)(num233 / 2);
				break;
			case "/quickload-clientprobe":
				if (Main.netMode == 2)
				{
					NetMessage.SendData(94, -1, whoAmI, NetworkText.FromLiteral(text5), whoAmI);
				}
				else if (Netplay.ServerIPText == "127.0.0.1")
				{
					NetMessage.SendData(94, -1, -1, NetworkText.FromLiteral("/quickload-clientresp " + QuickLoad.Serialize(new QuickLoad.JoinWorld().WithCurrentState())), num232);
				}
				break;
			default:
			{
				if (!text5.StartsWith("/quickload-clientresp "))
				{
					break;
				}
				if (Main.netMode == 2)
				{
					NetMessage.SendData(94, num232, -1, NetworkText.FromLiteral(text5));
					break;
				}
				QuickLoad.JoinWorld joinWorld = (QuickLoad.JoinWorld)QuickLoad.Deserialize(text5.Substring("/quickload-clientresp ".Length));
				if (QuickLoad.TryRead(out var config) && config is QuickLoad.JoinWorld)
				{
					QuickLoad.JoinWorld joinWorld2 = (QuickLoad.JoinWorld)config;
					joinWorld2.ExtraClients.Add(joinWorld);
					QuickLoad.Set(joinWorld2);
					ChatHelper.DisplayMessage(NetworkText.FromLiteral("/quickload added " + Path.GetFileName(joinWorld.PlayerPath)), new Color(250, 250, 0), byte.MaxValue);
				}
				break;
			}
			}
			break;
		}
		case 95:
		{
			ushort num197 = reader.ReadUInt16();
			int num198 = reader.ReadByte();
			if (Main.netMode != 2)
			{
				break;
			}
			for (int num199 = 0; num199 < 1000; num199++)
			{
				if (Main.projectile[num199].owner == num197 && Main.projectile[num199].active && Main.projectile[num199].type == 602 && Main.projectile[num199].ai[1] == (float)num198)
				{
					Main.projectile[num199].Kill();
					NetMessage.TrySendData(29, -1, -1, null, Main.projectile[num199].key);
					break;
				}
			}
			break;
		}
		case 96:
		{
			int num192 = reader.ReadByte();
			if (Main.netMode == 2)
			{
				num192 = whoAmI;
			}
			Player obj8 = Main.player[num192];
			int num193 = reader.ReadInt16();
			Vector2 val15 = reader.ReadVector2();
			Vector2 velocity6 = reader.ReadVector2();
			int lastPortalColorIndex2 = num193 + ((num193 % 2 == 0) ? 1 : (-1));
			obj8.lastPortalColorIndex = lastPortalColorIndex2;
			obj8.Teleport(val15, 4, num193);
			obj8.velocity = velocity6;
			if (Main.netMode == 2)
			{
				NetMessage.SendData(96, -1, num192, null, num192, val15.X, val15.Y, num193);
			}
			break;
		}
		case 97:
			if (Main.netMode == 1)
			{
				AchievementsHelper.NotifyNPCKilledDirect(Main.player[Main.myPlayer], reader.ReadInt16());
			}
			break;
		case 98:
			if (Main.netMode == 1)
			{
				AchievementsHelper.NotifyProgressionEvent(reader.ReadInt16());
			}
			break;
		case 99:
		{
			int num172 = reader.ReadByte();
			if (Main.netMode == 2)
			{
				num172 = whoAmI;
			}
			Main.player[num172].MinionRestTargetPoint = reader.ReadVector2();
			if (Main.netMode == 2)
			{
				NetMessage.TrySendData(99, -1, whoAmI, null, num172);
			}
			break;
		}
		case 115:
		{
			int num163 = reader.ReadByte();
			if (Main.netMode == 2)
			{
				num163 = whoAmI;
			}
			Main.player[num163].MinionAttackTargetNPC = reader.ReadInt16();
			if (Main.netMode == 2)
			{
				NetMessage.TrySendData(115, -1, whoAmI, null, num163);
			}
			break;
		}
		case 100:
		{
			int num151 = reader.ReadUInt16();
			NPC obj6 = Main.npc[num151];
			int num152 = reader.ReadInt16();
			Vector2 newPos = reader.ReadVector2();
			Vector2 velocity3 = reader.ReadVector2();
			int lastPortalColorIndex = num152 + ((num152 % 2 == 0) ? 1 : (-1));
			obj6.lastPortalColorIndex = lastPortalColorIndex;
			obj6.Teleport(newPos, 4, num152);
			obj6.velocity = velocity3;
			obj6.netOffset *= 0f;
			break;
		}
		case 101:
			if (Main.netMode != 2)
			{
				NPC.ShieldStrengthTowerSolar = reader.ReadUInt16();
				NPC.ShieldStrengthTowerVortex = reader.ReadUInt16();
				NPC.ShieldStrengthTowerNebula = reader.ReadUInt16();
				NPC.ShieldStrengthTowerStardust = reader.ReadUInt16();
				if (NPC.ShieldStrengthTowerSolar < 0)
				{
					NPC.ShieldStrengthTowerSolar = 0;
				}
				if (NPC.ShieldStrengthTowerVortex < 0)
				{
					NPC.ShieldStrengthTowerVortex = 0;
				}
				if (NPC.ShieldStrengthTowerNebula < 0)
				{
					NPC.ShieldStrengthTowerNebula = 0;
				}
				if (NPC.ShieldStrengthTowerStardust < 0)
				{
					NPC.ShieldStrengthTowerStardust = 0;
				}
				if (NPC.ShieldStrengthTowerSolar > NPC.LunarShieldPowerMax)
				{
					NPC.ShieldStrengthTowerSolar = NPC.LunarShieldPowerMax;
				}
				if (NPC.ShieldStrengthTowerVortex > NPC.LunarShieldPowerMax)
				{
					NPC.ShieldStrengthTowerVortex = NPC.LunarShieldPowerMax;
				}
				if (NPC.ShieldStrengthTowerNebula > NPC.LunarShieldPowerMax)
				{
					NPC.ShieldStrengthTowerNebula = NPC.LunarShieldPowerMax;
				}
				if (NPC.ShieldStrengthTowerStardust > NPC.LunarShieldPowerMax)
				{
					NPC.ShieldStrengthTowerStardust = NPC.LunarShieldPowerMax;
				}
			}
			break;
		case 102:
		{
			int num109 = reader.ReadByte();
			ushort num110 = reader.ReadUInt16();
			Vector2 val6 = reader.ReadVector2();
			if (Main.netMode == 2)
			{
				num109 = whoAmI;
				NetMessage.TrySendData(102, -1, -1, null, num109, (int)num110, val6.X, val6.Y);
				break;
			}
			Player player11 = Main.player[num109];
			for (int num111 = 0; num111 < 255; num111++)
			{
				Player player12 = Main.player[num111];
				if (!player12.active || player12.dead || (player11.team != 0 && player11.team != player12.team) || !(player12.Distance(val6) < 700f))
				{
					continue;
				}
				Vector2 val7 = player11.Center - player12.Center;
				Vector2 val8 = Vector2.Normalize(val7);
				if (!val8.HasNaNs())
				{
					int type11 = 90;
					float num112 = 0f;
					float num113 = (float)Math.PI / 15f;
					Vector2 spinningpoint = new Vector2(0f, -8f);
					Vector2 val9 = new Vector2(-3f);
					float num114 = 0f;
					float num115 = 0.005f;
					switch (num110)
					{
					case 179:
						type11 = 86;
						break;
					case 173:
						type11 = 90;
						break;
					case 176:
						type11 = 88;
						break;
					}
					for (int num116 = 0; (float)num116 < val7.Length() / 6f; num116++)
					{
						Vector2 position2 = player12.Center + 6f * (float)num116 * val8 + spinningpoint.RotatedBy(num112) + val9;
						num112 += num113;
						int num117 = Dust.NewDust(position2, 6, 6, type11, 0f, 0f, 100, default, 1.5f);
						Main.dust[num117].noGravity = true;
						Main.dust[num117].velocity = Vector2.Zero;
						num114 = (Main.dust[num117].fadeIn = num114 + num115);
						Dust obj5 = Main.dust[num117];
						obj5.velocity += val8 * 1.5f;
					}
				}
				player12.NebulaLevelup(num110);
			}
			break;
		}
		case 103:
			if (Main.netMode == 1)
			{
				NPC.MaxMoonLordCountdown = reader.ReadInt32();
				NPC.MoonLordCountdown = reader.ReadInt32();
			}
			break;
		case 104:
			if (Main.netMode == 1 && Main.npcShop > 0)
			{
				Item[] item4 = Main.instance.shop[Main.npcShop].item;
				int num103 = reader.ReadByte();
				int type9 = reader.ReadInt16();
				int stack5 = reader.ReadInt16();
				int prefixWeWant2 = reader.ReadByte();
				int value = reader.ReadInt32();
				BitsByte bitsByte22 = reader.ReadByte();
				if (num103 < item4.Length)
				{
					item4[num103] = new Item();
					item4[num103].SetDefaults(type9);
					item4[num103].stack = stack5;
					item4[num103].Prefix(prefixWeWant2);
					item4[num103].value = value;
					item4[num103].buyOnce = bitsByte22[0];
				}
			}
			break;
		case 105:
			if (Main.netMode != 1)
			{
				short i2 = reader.ReadInt16();
				int j2 = reader.ReadInt16();
				bool flag13 = reader.ReadBoolean();
				WorldGen.ToggleGemLock(i2, j2, flag13);
			}
			break;
		case 106:
			if (Main.netMode == 1)
			{
				HalfVector2 val5 = default;
				val5.PackedValue = reader.ReadUInt32();
				Utils.PoofOfSmoke(val5.ToVector2());
			}
			break;
		case 107:
			if (Main.netMode == 1)
			{
				Color c = reader.ReadRGB();
				string text4 = NetworkText.Deserialize(reader).ToString();
				int widthLimit = reader.ReadInt16();
				Main.NewTextMultiline(text4, force: false, c, widthLimit);
			}
			break;
		case 108:
			if (Main.netMode == 1)
			{
				int damage2 = reader.ReadInt16();
				float knockBack = reader.ReadSingle();
				int x5 = reader.ReadInt16();
				int y5 = reader.ReadInt16();
				int angle = reader.ReadInt16();
				int ammo = reader.ReadInt16();
				int num79 = reader.ReadByte();
				if (num79 == Main.myPlayer)
				{
					WorldGen.ShootFromCannon(x5, y5, angle, ammo, damage2, knockBack, num79, fromWire: true);
				}
			}
			break;
		case 109:
			if (Main.netMode == 2)
			{
				short num73 = reader.ReadInt16();
				int num74 = reader.ReadInt16();
				int num75 = reader.ReadInt16();
				int num76 = reader.ReadInt16();
				byte toolMode = reader.ReadByte();
				int num77 = whoAmI;
				WiresUI.Settings.MultiToolMode toolMode2 = WiresUI.Settings.ToolMode;
				WiresUI.Settings.ToolMode = (WiresUI.Settings.MultiToolMode)toolMode;
				Wiring.MassWireOperation(new Point((int)num73, num74), new Point(num75, num76), Main.player[num77]);
				WiresUI.Settings.ToolMode = toolMode2;
			}
			break;
		case 110:
		{
			if (Main.netMode != 1)
			{
				break;
			}
			int type5 = reader.ReadInt16();
			int num66 = reader.ReadInt16();
			int num67 = reader.ReadByte();
			if (num67 == Main.myPlayer)
			{
				Player player7 = Main.player[num67];
				for (int k = 0; k < num66; k++)
				{
					player7.ConsumeItem(type5);
				}
				player7.wireOperationsCooldown = 0;
			}
			break;
		}
		case 111:
			if (Main.netMode == 2)
			{
				BirthdayParty.ToggleManualParty();
			}
			break;
		case 112:
		{
			int num61 = reader.ReadByte();
			int num62 = reader.ReadInt32();
			int num63 = reader.ReadInt32();
			int num64 = reader.ReadByte();
			int num65 = reader.ReadInt16();
			bool flag7 = reader.ReadByte() == 1;
			switch (num61)
			{
			case 1:
				if (Main.netMode == 1)
				{
					WorldGen.TreeGrowFX(num62, num63, num64, num65, flag7);
				}
				if (Main.netMode == 2)
				{
					NetMessage.TrySendData(b, -1, -1, null, num61, num62, num63, num64, num65, flag7 ? 1 : 0);
				}
				break;
			case 2:
				NPC.FairyEffects(new Vector2((float)num62, (float)num63), num65);
				break;
			}
			break;
		}
		case 113:
		{
			int x2 = reader.ReadInt16();
			int y2 = reader.ReadInt16();
			if (Main.netMode == 2 && !Main.snowMoon && !Main.pumpkinMoon)
			{
				if (DD2Event.WouldFailSpawningHere(x2, y2))
				{
					DD2Event.FailureMessage(whoAmI);
				}
				DD2Event.SummonCrystal(x2, y2, whoAmI);
			}
			break;
		}
		case 114:
			if (Main.netMode == 1)
			{
				DD2Event.WipeEntities();
			}
			break;
		case 116:
			if (Main.netMode == 1)
			{
				DD2Event.TimeLeftBetweenWaves = reader.ReadInt32();
			}
			break;
		case 117:
		{
			int num27 = reader.ReadByte();
			if (Main.netMode != 2 || whoAmI == num27 || (Main.player[num27].hostile && Main.player[whoAmI].hostile))
			{
				PlayerDeathReason playerDeathReason2 = PlayerDeathReason.FromReader(reader);
				int damage = reader.ReadInt16();
				int num28 = reader.ReadByte() - 1;
				BitsByte bitsByte3 = reader.ReadByte();
				bool flag2 = bitsByte3[0];
				bool pvp2 = bitsByte3[1];
				int num29 = reader.ReadSByte();
				Main.player[num27].Hurt(playerDeathReason2, damage, num28, pvp2, quiet: true, flag2, num29);
				if (Main.netMode == 2)
				{
					NetMessage.SendPlayerHurt(num27, playerDeathReason2, damage, num28, flag2, pvp2, num29, -1, whoAmI);
				}
			}
			break;
		}
		case 118:
		{
			int num13 = reader.ReadByte();
			if (Main.netMode == 2)
			{
				num13 = whoAmI;
			}
			PlayerDeathReason playerDeathReason = PlayerDeathReason.FromReader(reader);
			int num14 = reader.ReadInt16();
			int num15 = reader.ReadByte() - 1;
			bool pvp = ((BitsByte)reader.ReadByte())[0];
			Main.player[num13].KillMe(playerDeathReason, num14, num15, pvp);
			if (Main.netMode == 2)
			{
				NetMessage.SendPlayerDeath(num13, playerDeathReason, num14, num15, pvp, -1, whoAmI);
			}
			break;
		}
		case 120:
		{
			int num265 = reader.ReadByte();
			if (Main.netMode == 2)
			{
				num265 = whoAmI;
			}
			int num266 = reader.ReadByte();
			if (num266 >= 0 && num266 < EmoteID.Count && Main.netMode == 2)
			{
				EmoteBubble.NewBubble(num266, new WorldUIAnchor((Entity)Main.player[num265]), 360);
				EmoteBubble.CheckForNPCsToReactToEmoteBubble(num266, Main.player[num265]);
			}
			break;
		}
		case 121:
		{
			int num257 = reader.ReadByte();
			if (Main.netMode == 2)
			{
				num257 = whoAmI;
			}
			int num258 = reader.ReadInt32();
			int num259 = reader.ReadByte();
			int num260 = reader.ReadByte();
			if (!TileEntity.TryGet<TEDisplayDoll>(num258, out var result7))
			{
				TEDisplayDoll.ReadDummySync(num259, num260, reader);
				break;
			}
			result7.ReadData(num259, num260, reader);
			if (Main.netMode == 2)
			{
				NetMessage.TrySendData(b, -1, num257, null, num257, num258, num259, num260);
			}
			break;
		}
		case 122:
		{
			int num230 = reader.ReadInt32();
			int num231 = reader.ReadByte();
			if (Main.netMode == 2)
			{
				num231 = whoAmI;
			}
			if (Main.netMode == 2)
			{
				if (num230 == -1)
				{
					Main.player[num231].tileEntityAnchor.Clear();
					NetMessage.TrySendData(b, -1, -1, null, num230, num231);
					break;
				}
				if (!TileEntity.IsOccupied(num230, out var _) && TileEntity.TryGet<TileEntity>(num230, out var result5))
				{
					Main.player[num231].tileEntityAnchor.Set(num230, result5.Position.X, result5.Position.Y);
					NetMessage.TrySendData(b, -1, -1, null, num230, num231);
				}
			}
			if (Main.netMode == 1)
			{
				TileEntity result6;
				if (num230 == -1)
				{
					Main.player[num231].tileEntityAnchor.Clear();
				}
				else if (TileEntity.TryGet<TileEntity>(num230, out result6))
				{
					TileEntity.SetInteractionAnchor(Main.player[num231], result6.Position.X, result6.Position.Y, num230);
				}
			}
			break;
		}
		case 123:
		{
			if (Main.netMode != 2)
			{
				break;
			}
			int x10 = reader.ReadInt16();
			int y10 = reader.ReadInt16();
			int type19 = reader.ReadInt16();
			int prefix4 = reader.ReadByte();
			int stack8 = reader.ReadInt16();
			using (Item.DefaultAssignNewItemsToPlayer(whoAmI))
			{
				TEWeaponsRack.TryPlacing(x10, y10, type19, prefix4, stack8);
				break;
			}
		}
		case 124:
		{
			int num189 = reader.ReadByte();
			if (Main.netMode == 2)
			{
				num189 = whoAmI;
			}
			int num190 = reader.ReadInt32();
			int num191 = reader.ReadByte();
			bool flag20 = false;
			if (num191 >= 2)
			{
				flag20 = true;
				num191 -= 2;
			}
			if (!TileEntity.TryGet<TEHatRack>(num190, out var result4) || num191 >= 2)
			{
				reader.ReadInt32();
				reader.ReadByte();
				break;
			}
			result4.ReadItem(num191, reader, flag20);
			if (Main.netMode == 2)
			{
				NetMessage.TrySendData(b, -1, num189, null, num189, num190, num191, flag20.ToInt());
			}
			break;
		}
		case 125:
		{
			int num175 = reader.ReadByte();
			int num176 = reader.ReadInt16();
			int num177 = reader.ReadInt16();
			int num178 = reader.ReadByte();
			if (Main.netMode == 2)
			{
				num175 = whoAmI;
			}
			if (Main.netMode == 1)
			{
				Main.player[Main.myPlayer].GetOtherPlayersPickTile(num176, num177, num178);
			}
			if (Main.netMode == 2)
			{
				NetMessage.TrySendData(125, -1, num175, null, num175, num176, num177, num178);
			}
			break;
		}
		case 126:
			if (Main.netMode == 1)
			{
				NPC.RevengeManager.AddMarkerFromReader(reader);
			}
			break;
		case 127:
		{
			int markerUniqueID = reader.ReadInt32();
			if (Main.netMode == 1)
			{
				NPC.RevengeManager.DestroyMarker(markerUniqueID);
			}
			break;
		}
		case 128:
		{
			int num166 = reader.ReadByte();
			int num167 = reader.ReadUInt16();
			int num168 = reader.ReadUInt16();
			int num169 = reader.ReadUInt16();
			int num170 = reader.ReadUInt16();
			if (Main.netMode == 2)
			{
				NetMessage.SendData(128, -1, num166, null, num166, num169, num170, 0f, num167, num168);
			}
			else
			{
				GolfHelper.ContactListener.PutBallInCup_TextAndEffects(new Point(num167, num168), num166, num169, num170);
			}
			break;
		}
		case 129:
			if (Main.netMode == 1)
			{
				if (Main.LocalPlayer.team > 0)
				{
					NetMessage.SendData(45, -1, -1, null, Main.myPlayer);
				}
				Main.FixUIScale();
				Main.TrySetPreparationState(Main.WorldPreparationState.ProcessingData);
			}
			break;
		case 130:
		{
			if (Main.netMode != 2)
			{
				break;
			}
			int num143 = reader.ReadUInt16();
			int num144 = reader.ReadUInt16();
			int num145 = reader.ReadInt16();
			if (num145 == 682)
			{
				if (NPC.unlockedSlimeRedSpawn)
				{
					break;
				}
				NPC.unlockedSlimeRedSpawn = true;
				NetMessage.TrySendData(7);
			}
			num143 *= 16;
			num144 *= 16;
			NPC nPC4 = new NPC();
			nPC4.SetDefaults(num145);
			int type14 = nPC4.type;
			int netID = nPC4.netID;
			int num146 = NPC.NewNPC(new EntitySource_FishedOut(Main.player[whoAmI]), num143, num144, num145);
			if (netID != type14)
			{
				Main.npc[num146].SetDefaults(netID);
				NetMessage.TrySendData(23, -1, -1, null, num146);
			}
			if (num145 == 682)
			{
				WorldGen.CheckAchievement_RealEstateAndTownSlimes();
			}
			break;
		}
		case 131:
			if (Main.netMode == 1)
			{
				int num134 = reader.ReadUInt16();
				NPC nPC3 = null;
				nPC3 = ((num134 >= Main.maxNPCs) ? new NPC() : Main.npc[num134]);
				int num135 = reader.ReadByte();
				if (num135 == 1)
				{
					int time = reader.ReadInt32();
					int fromWho = reader.ReadInt16();
					nPC3.GetImmuneTime(fromWho, time);
				}
			}
			break;
		case 132:
			if (Main.netMode == 1)
			{
				Point val11 = reader.ReadVector2().ToPoint();
				ushort key = reader.ReadUInt16();
				LegacySoundStyle legacySoundStyle = SoundID.SoundByIndex[key];
				BitsByte bitsByte23 = reader.ReadByte();
				SoundEngine.PlaySound(Style: (!bitsByte23[0]) ? legacySoundStyle.Style : reader.ReadInt32(), volumeScale: (!bitsByte23[1]) ? legacySoundStyle.Volume : MathHelper.Clamp(reader.ReadSingle(), 0f, 1f), pitchOffset: (!bitsByte23[2]) ? legacySoundStyle.GetRandomPitch() : MathHelper.Clamp(reader.ReadSingle(), -1f, 1f), type: legacySoundStyle.SoundId, x: val11.X, y: val11.Y);
			}
			break;
		case 133:
		{
			if (Main.netMode != 2)
			{
				break;
			}
			int x7 = reader.ReadInt16();
			int y7 = reader.ReadInt16();
			int type12 = reader.ReadInt16();
			int prefix3 = reader.ReadByte();
			int stack6 = reader.ReadInt16();
			using (Item.DefaultAssignNewItemsToPlayer(whoAmI))
			{
				TEFoodPlatter.TryPlacing(x7, y7, type12, prefix3, stack6);
				break;
			}
		}
		case 134:
		{
			int num108 = reader.ReadByte();
			int ladyBugLuckTimeLeft = reader.ReadInt32();
			float torchLuck = reader.ReadSingle();
			byte luckPotion = reader.ReadByte();
			bool hasGardenGnomeNearby = reader.ReadBoolean();
			bool brokenMirrorBadLuck = reader.ReadBoolean();
			float equipmentBasedLuckBonus = reader.ReadSingle();
			float coinLuck = reader.ReadSingle();
			byte kiteLuckLevel = reader.ReadByte();
			if (Main.netMode == 2)
			{
				num108 = whoAmI;
			}
			Player obj4 = Main.player[num108];
			obj4.ladyBugLuckTimeLeft = ladyBugLuckTimeLeft;
			obj4.torchLuck = torchLuck;
			obj4.luckPotion = luckPotion;
			obj4.HasGardenGnomeNearby = hasGardenGnomeNearby;
			obj4.brokenMirrorBadLuck = brokenMirrorBadLuck;
			obj4.equipmentBasedLuckBonus = equipmentBasedLuckBonus;
			obj4.coinLuck = coinLuck;
			obj4.kiteLuckLevel = kiteLuckLevel;
			obj4.RecalculateLuck();
			if (Main.netMode == 2)
			{
				NetMessage.SendData(134, -1, num108, null, num108);
			}
			break;
		}
		case 135:
		{
			int num107 = reader.ReadByte();
			if (Main.netMode == 1)
			{
				Main.player[num107].immuneAlpha = 255;
			}
			break;
		}
		case 136:
		{
			if (Main.netMode == 2)
			{
				break;
			}
			for (int num104 = 0; num104 < 2; num104++)
			{
				for (int num105 = 0; num105 < 3; num105++)
				{
					NPC.cavernMonsterType[num104, num105] = reader.ReadUInt16();
				}
			}
			break;
		}
		case 137:
			if (Main.netMode == 2)
			{
				int num102 = reader.ReadInt16();
				int buffTypeToRemove = reader.ReadUInt16();
				if (num102 >= 0 && num102 < Main.maxNPCs)
				{
					Main.npc[num102].RequestBuffRemoval(buffTypeToRemove);
				}
			}
			break;
		case 139:
			if (Main.netMode != 2)
			{
				int num101 = reader.ReadByte();
				bool flag12 = reader.ReadBoolean();
				Main.countsAsHostForGameplay[num101] = flag12;
			}
			break;
		case 140:
		{
			int num99 = reader.ReadByte();
			int num100 = reader.ReadInt32();
			switch (num99)
			{
			case 0:
				if (Main.netMode == 1)
				{
					CreditsRollEvent.SetRemainingTimeDirect(num100);
				}
				break;
			case 1:
				if (Main.netMode == 2)
				{
					NPC.TransformCopperSlime(num100);
				}
				break;
			case 2:
				if (Main.netMode == 2)
				{
					NPC.TransformElderSlime(num100);
				}
				break;
			}
			break;
		}
		case 141:
		{
			LucyAxeMessage.MessageSource messageSource = (LucyAxeMessage.MessageSource)reader.ReadByte();
			byte b6 = reader.ReadByte();
			Vector2 val4 = reader.ReadVector2();
			int num91 = reader.ReadInt32();
			int num92 = reader.ReadInt32();
			if (Main.netMode == 2)
			{
				NetMessage.SendData(141, -1, whoAmI, null, (int)messageSource, (int)b6, val4.X, val4.Y, num91, num92);
			}
			else
			{
				LucyAxeMessage.CreateFromNet(messageSource, b6, new Vector2((float)num91, (float)num92), val4);
			}
			break;
		}
		case 142:
		{
			int num83 = reader.ReadByte();
			if (Main.netMode == 2)
			{
				num83 = whoAmI;
			}
			Player obj2 = Main.player[num83];
			obj2.piggyBankProjTracker.Read(reader);
			obj2.voidLensChest.Read(reader);
			if (Main.netMode == 2)
			{
				NetMessage.TrySendData(142, -1, whoAmI, null, num83);
			}
			break;
		}
		case 143:
			if (Main.netMode == 2)
			{
				DD2Event.AttemptToSkipWaitTime();
			}
			break;
		case 144:
			if (Main.netMode == 2)
			{
				NPC.HaveDryadDoStardewAnimation();
			}
			break;
		case 146:
			if (Main.netMode == 1)
			{
				switch ((int)reader.ReadByte())
				{
				case 0:
					WorldItem.ShimmerEffect(reader.ReadVector2());
					break;
				case 1:
				{
					Vector2 coinPosition = reader.ReadVector2();
					int coinAmount = reader.ReadInt32();
					Main.player[Main.myPlayer].AddCoinLuck(coinPosition, coinAmount);
					break;
				}
				}
			}
			break;
		case 147:
		{
			int num68 = reader.ReadByte();
			if (Main.netMode == 2)
			{
				num68 = whoAmI;
			}
			int num69 = reader.ReadByte();
			Main.player[num68].TrySwitchingLoadout(num69);
			ReadAccessoryVisibility(reader, Main.player[num68].hideVisibleAccessory);
			if (Main.netMode == 2)
			{
				NetMessage.TrySendData(b, -1, num68, null, num68, num69);
			}
			break;
		}
		case 149:
		{
			if (Main.netMode != 2)
			{
				break;
			}
			int x4 = reader.ReadInt16();
			int y4 = reader.ReadInt16();
			int type6 = reader.ReadInt16();
			int prefix2 = reader.ReadByte();
			int stack4 = reader.ReadInt16();
			using (Item.DefaultAssignNewItemsToPlayer(whoAmI))
			{
				TEDeadCellsDisplayJar.TryPlacing(x4, y4, type6, prefix2, stack4);
				break;
			}
		}
		case 150:
		{
			int num56 = reader.ReadByte();
			if (Main.netMode == 2)
			{
				num56 = whoAmI;
			}
			int num57 = reader.ReadInt16();
			Player player6 = Main.player[num56];
			if (Main.netMode == 2)
			{
				if (num57 >= 0)
				{
					player6.SetOrRequestSpectating(num57);
					break;
				}
				player6.spectating = -1;
				NetMessage.SendData(150, -1, whoAmI, null, whoAmI, num57);
			}
			else if (player6 != Main.LocalPlayer || player6.spectating >= 0)
			{
				player6.spectating = num57;
			}
			break;
		}
		case 152:
		{
			int num50 = reader.ReadByte();
			if (Main.netMode == 2)
			{
				num50 = whoAmI;
			}
			if (Main.netMode == 2)
			{
				NetMessage.TrySendData(152, -1, whoAmI, null, num50);
			}
			if (Main.netMode == 1)
			{
				Player player5 = Main.player[num50];
				Item item3 = player5.inventory[player5.selectedItem];
				if (item3.UseSound != null)
				{
					SoundEngine.PlaySound(item3.UseSound, player5.Center, item3.useSoundPitch);
				}
			}
			break;
		}
		case 153:
		{
			int num48 = reader.ReadByte();
			int num49 = reader.ReadInt16();
			Main.npc[num48].GetHurtByDebuff(num49);
			if (Main.netMode == 2)
			{
				NetMessage.TrySendData(153, -1, whoAmI, null, num48, num49);
			}
			break;
		}
		case 154:
			if (Main.netMode == 2)
			{
				NetMessage.TrySendData(154, whoAmI);
			}
			else
			{
				Ping.PingRecieved();
			}
			break;
		case 155:
		{
			short num31 = reader.ReadInt16();
			short newSize = reader.ReadInt16();
			if (num31 >= 0 && num31 < 8000)
			{
				Main.chest[num31].Resize(newSize);
			}
			break;
		}
		case 156:
			if (Main.netMode == 2)
			{
				Point16 point = new Point16(reader.ReadInt16(), reader.ReadInt16());
				int itemType = reader.ReadInt16();
				if (TileEntity.TryGetAt<TELeashedEntityAnchorWithItem>(point.X, point.Y, out var result))
				{
					result.InsertItem(itemType);
				}
			}
			break;
		case 158:
			if (Main.netMode != 2)
			{
				byte b3 = reader.ReadByte();
				Main.player[b3].Spawn(PlayerSpawnContext.TeamSwap);
			}
			break;
		case 159:
			if (Main.netMode == 2)
			{
				int sectionX = reader.ReadUInt16();
				int sectionY = reader.ReadUInt16();
				NetMessage.SendSection(whoAmI, sectionX, sectionY);
			}
			break;
		case 160:
			if (Main.netMode != 2)
			{
				int num17 = reader.ReadInt16();
				Vector2 position = reader.ReadVector2();
				Main.item[num17].position = position;
			}
			break;
		case 161:
		{
			string text = reader.ReadString();
			Main.player[whoAmI].host = !string.IsNullOrWhiteSpace(Netplay.HostToken) && Netplay.HostToken == text;
			break;
		}
		default:
			if (Main.netMode == 2 && Netplay.Clients[whoAmI].State == 0)
			{
				NetMessage.BootPlayer(whoAmI, Lang.mp[2].ToNetworkText());
			}
			break;
		case 15:
		case 25:
		case 26:
		case 44:
		case 67:
		case 83:
		case 93:
			break;
		}
	}

	private static void ReadAccessoryVisibility(BinaryReader reader, bool[] hideVisibleAccessory)
	{
		ushort num = reader.ReadUInt16();
		for (int i = 0; i < hideVisibleAccessory.Length; i++)
		{
			hideVisibleAccessory[i] = (num & (1 << i)) != 0;
		}
	}

	private static void TrySendingItemArray(int plr, Item[] array, int slotStartIndex)
	{
		for (int i = 0; i < array.Length; i++)
		{
			NetMessage.TrySendData(5, -1, -1, null, plr, slotStartIndex + i);
		}
	}
}
