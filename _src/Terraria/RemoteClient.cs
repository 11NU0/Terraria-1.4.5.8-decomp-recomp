using System;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria.Localization;
using Terraria.Net.Sockets;

namespace Terraria;

public class RemoteClient
{
	public ISocket Socket;

	public int Id;

	public string Name = "Anonymous";

	public bool IsActive;

	public bool PendingTermination;

	public bool PendingTerminationApproved;

	public bool Kicked;

	public bool IsAnnouncementCompleted;

	public int State;

	public int TimeOutTimer;

	public bool[,] TileSections = new bool[Main.maxTilesX / 200 + 1, Main.maxTilesY / 150 + 1];

	public uint[,] TileSectionsCheckTime = new uint[Main.maxTilesX / 200 + 1, Main.maxTilesY / 150 + 1];

	public bool CheckingSections;

	public byte[] ReadBuffer;

	public float SpamProjectile;

	public float SpamAddBlock;

	public float SpamDeleteBlock;

	public float SpamWater;

	public float SpamProjectileMax = 100f;

	public float SpamAddBlockMax = 100f;

	public float SpamDeleteBlockMax = 500f;

	public float SpamWaterMax = 50f;

	private volatile bool _isReading;

	public bool ReadBufferFull => NetMessage.buffer[Id].RemainingReadBufferLength < ReadBuffer.Length;

	public static event Action<int, Point> NetSectionActivated;

	public bool IsConnected()
	{
		if (Socket != null)
		{
			return Socket.IsConnected();
		}
		return false;
	}

	public void SpamUpdate()
	{
		if (!Netplay.SpamCheck)
		{
			SpamProjectile = 0f;
			SpamDeleteBlock = 0f;
			SpamAddBlock = 0f;
			SpamWater = 0f;
			return;
		}
		if (SpamProjectile > SpamProjectileMax)
		{
			NetMessage.BootPlayer(Id, NetworkText.FromKey("Net.CheatingProjectileSpam"));
		}
		if (SpamAddBlock > SpamAddBlockMax)
		{
			NetMessage.BootPlayer(Id, NetworkText.FromKey("Net.CheatingTileSpam"));
		}
		if (SpamDeleteBlock > SpamDeleteBlockMax)
		{
			NetMessage.BootPlayer(Id, NetworkText.FromKey("Net.CheatingTileRemovalSpam"));
		}
		if (SpamWater > SpamWaterMax)
		{
			NetMessage.BootPlayer(Id, NetworkText.FromKey("Net.CheatingLiquidSpam"));
		}
		SpamProjectile -= 0.4f;
		if (SpamProjectile < 0f)
		{
			SpamProjectile = 0f;
		}
		SpamAddBlock -= 0.3f;
		if (SpamAddBlock < 0f)
		{
			SpamAddBlock = 0f;
		}
		SpamDeleteBlock -= 5f;
		if (SpamDeleteBlock < 0f)
		{
			SpamDeleteBlock = 0f;
		}
		SpamWater -= 0.2f;
		if (SpamWater < 0f)
		{
			SpamWater = 0f;
		}
	}

	public void SpamClear()
	{
		SpamProjectile = 0f;
		SpamAddBlock = 0f;
		SpamDeleteBlock = 0f;
		SpamWater = 0f;
	}

	public static void CheckSection(int playerIndex, Vector2 position, int fluff = 1)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		RemoteClient remoteClient = Netplay.Clients[playerIndex];
		if (remoteClient.CheckingSections)
		{
			return;
		}
		CheckSection_ForClient(remoteClient, position, fluff);
		try
		{
			remoteClient.CheckingSections = true;
			for (int i = 0; i < 255; i++)
			{
				Player player = Main.player[i];
				if (player.active && player.spectating == playerIndex)
				{
					CheckSection(i, position, fluff);
				}
			}
		}
		finally
		{
			remoteClient.CheckingSections = false;
		}
	}

	private static void CheckSection_ForClient(RemoteClient client, Vector2 position, int fluff)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		ActiveSections.CheckSection(position, fluff);
		int sectionX = Netplay.GetSectionX((int)(position.X / 16f));
		int sectionY = Netplay.GetSectionY((int)(position.Y / 16f));
		int num = 0;
		for (int i = sectionX - fluff; i < sectionX + fluff + 1; i++)
		{
			for (int j = sectionY - fluff; j < sectionY + fluff + 1; j++)
			{
				if (i >= 0 && i < Main.maxSectionsX && j >= 0 && j < Main.maxSectionsY)
				{
					bool flag = client.IsSectionActive(new Point(i, j));
					client.TileSectionsCheckTime[i, j] = Main.GameUpdateCount;
					if (!flag)
					{
						NetSectionActivated(client.Id, new Point(i, j));
					}
					if (!client.TileSections[i, j])
					{
						num++;
					}
				}
			}
		}
		if (num <= 0)
		{
			return;
		}
		int number = num;
		NetMessage.SendData(9, client.Id, -1, Lang.inter[44].ToNetworkText(), number);
		for (int k = sectionX - fluff; k < sectionX + fluff + 1; k++)
		{
			for (int l = sectionY - fluff; l < sectionY + fluff + 1; l++)
			{
				NetMessage.SendSection(client.Id, k, l);
			}
		}
	}

	public bool SectionRange(int size, int firstX, int firstY)
	{
		for (int i = 0; i < 4; i++)
		{
			int num = firstX;
			int num2 = firstY;
			if (i == 1)
			{
				num += size;
			}
			if (i == 2)
			{
				num2 += size;
			}
			if (i == 3)
			{
				num += size;
				num2 += size;
			}
			int sectionX = Netplay.GetSectionX(num);
			int sectionY = Netplay.GetSectionY(num2);
			if (TileSections[sectionX, sectionY])
			{
				return true;
			}
		}
		return false;
	}

	public bool IsSectionActive(Point sectionCoords)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		sectionCoords = sectionCoords.ClampSectionCoords();
		return TileSectionsCheckTime[sectionCoords.X, sectionCoords.Y] + ActiveSections.SectionInactiveTime >= Main.GameUpdateCount;
	}

	public void Reset()
	{
		Array.Clear(TileSections, 0, TileSections.Length);
		Array.Clear(TileSectionsCheckTime, 0, TileSectionsCheckTime.Length);
		if (Id < 255)
		{
			Main.player[Id] = new Player();
		}
		TimeOutTimer = 0;
		State = 0;
		_isReading = false;
		PendingTermination = false;
		PendingTerminationApproved = false;
		Kicked = false;
		SpamClear();
		IsActive = false;
		NetMessage.buffer[Id].Reset();
		if (Socket != null)
		{
			Socket.Close();
			Socket = null;
		}
	}

	public void ServerWriteCallBack(object state)
	{
		NetMessage.buffer[Id].spamCount--;
	}

	public void Update()
	{
		if (!IsActive)
		{
			State = 0;
			IsActive = true;
		}
		TryRead();
	}

	private void TryRead()
	{
		if (_isReading)
		{
			return;
		}
		try
		{
			if (Socket.IsDataAvailable() && !ReadBufferFull)
			{
				_isReading = true;
				Socket.AsyncReceive(ReadBuffer, 0, ReadBuffer.Length, ServerReadCallBack);
			}
		}
		catch
		{
			PendingTermination = true;
		}
	}

	private void ServerReadCallBack(object state, int length)
	{
		if (!Netplay.Disconnect)
		{
			if (length == 0)
			{
				PendingTermination = true;
			}
			else
			{
				try
				{
					NetMessage.ReceiveBytes(ReadBuffer, length, Id);
				}
				catch
				{
					if (!Main.ignoreErrors)
					{
						throw;
					}
				}
			}
		}
		_isReading = false;
	}
}
