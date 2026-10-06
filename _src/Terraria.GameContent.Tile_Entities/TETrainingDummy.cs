using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;

namespace Terraria.GameContent.Tile_Entities;

public class TETrainingDummy : TileEntityType<TETrainingDummy>
{
	private static List<Rectangle> playerBoxes = new List<Rectangle>();

	private static bool playerBoxFilled;

	private static bool npcSlotsFull;

	public int npc;

	public int activationRetryCooldown;

	public override void RegisterTileEntityID(int assignedID)
	{
		base.RegisterTileEntityID(assignedID);
		TileEntity._UpdateStart += ClearBoxes;
	}

	public override void NetPlaceEntityAttempt(int x, int y)
	{
		TileEntityType<TETrainingDummy>.Place(x, y);
	}

	public override bool IsTileValidForEntity(int x, int y)
	{
		return ValidTile(x, y);
	}

	public static void ClearBoxes()
	{
		playerBoxes.Clear();
		playerBoxFilled = false;
		npcSlotsFull = false;
	}

	public override void Update()
	{
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
		if (npc != -1)
		{
			if (!Main.npc[npc].active || Main.npc[npc].type != 488 || Main.npc[npc].ai[0] != (float)Position.X || Main.npc[npc].ai[1] != (float)Position.Y)
			{
				Deactivate();
			}
		}
		else
		{
			if (npcSlotsFull)
			{
				return;
			}
			FillPlayerHitboxes();
			Rectangle val = new Rectangle(Position.X * 16, Position.Y * 16, 32, 48);
			val.Inflate(1600, 1600);
			bool flag = false;
			foreach (Rectangle playerBox in playerBoxes)
			{
				Rectangle current = playerBox;
				if (current.Intersects(val))
				{
					flag = true;
					break;
				}
			}
			if (flag)
			{
				Activate();
			}
		}
	}

	private static void FillPlayerHitboxes()
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		if (playerBoxFilled)
		{
			return;
		}
		for (int i = 0; i < 255; i++)
		{
			if (Main.player[i].active)
			{
				playerBoxes.Add(Main.player[i].getRect());
			}
		}
		playerBoxFilled = true;
	}

	public static bool ValidTile(int x, int y)
	{
		if (!Main.tile[x, y].active() || Main.tile[x, y].type != 378 || Main.tile[x, y].frameY != 0 || Main.tile[x, y].frameX % 36 != 0)
		{
			return false;
		}
		return true;
	}

	public TETrainingDummy()
	{
		npc = -1;
		RequiresUpdates = true;
	}

	public static int Hook_AfterPlacement(int x, int y, int type = 378, int style = 0, int direction = 1, int alternate = 0)
	{
		if (Main.netMode == 1)
		{
			NetMessage.SendTileSquare(Main.myPlayer, x - 1, y - 2, 2, 3);
			NetMessage.SendData(87, -1, -1, null, x - 1, y - 2, (int)TileEntityType<TETrainingDummy>.EntityTypeID);
			return -1;
		}
		return TileEntityType<TETrainingDummy>.Place(x - 1, y - 2);
	}

	public override void WriteExtraData(BinaryWriter writer, bool networkSend)
	{
		writer.Write((short)npc);
	}

	public override void ReadExtraData(BinaryReader reader, int gameVersion, bool networkSend)
	{
		npc = reader.ReadInt16();
	}

	private void Activate()
	{
		int num = NPC.NewNPC(new EntitySource_TileEntity(this), Position.X * 16 + 16, Position.Y * 16 + 48, 488, 100);
		if (num == Main.maxNPCs)
		{
			npcSlotsFull = true;
			return;
		}
		Main.npc[num].ai[0] = Position.X;
		Main.npc[num].ai[1] = Position.Y;
		Main.npc[num].netUpdate = true;
		npc = num;
		if (Main.netMode != 1)
		{
			NetMessage.SendData(86, -1, -1, null, ID, Position.X, Position.Y);
		}
	}

	public void Deactivate()
	{
		if (npc != -1)
		{
			Main.npc[npc].active = false;
		}
		npc = -1;
		if (Main.netMode != 1)
		{
			NetMessage.SendData(86, -1, -1, null, ID, Position.X, Position.Y);
		}
	}

	public override string ToString()
	{
		return Position.X + "x  " + Position.Y + "y npc: " + npc;
	}
}
