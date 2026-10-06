using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;

namespace Terraria.GameContent.Tile_Entities;

public class TELogicSensor : TileEntityType<TELogicSensor>
{
	public enum LogicCheckType
	{
		None,
		Day,
		Night,
		PlayerAbove,
		Water,
		Lava,
		Honey,
		Liquid
	}

	private static Dictionary<int, Rectangle> playerBox = new Dictionary<int, Rectangle>();

	private static List<Tuple<Point16, bool>> tripPoints = new List<Tuple<Point16, bool>>();

	private static List<int> markedIDsForRemoval = new List<int>();

	private static bool inUpdateLoop;

	private static bool playerBoxFilled;

	public LogicCheckType logicCheck;

	public bool On;

	public int CountedData;

	public override void RegisterTileEntityID(int assignedID)
	{
		base.RegisterTileEntityID(assignedID);
		TileEntity._UpdateStart += UpdateStartInternal;
		TileEntity._UpdateEnd += UpdateEndInternal;
	}

	public override void OnPlaced()
	{
		FigureCheckState();
	}

	public override bool IsTileValidForEntity(int x, int y)
	{
		return ValidTile(x, y);
	}

	private static void UpdateStartInternal()
	{
		inUpdateLoop = true;
		markedIDsForRemoval.Clear();
		playerBox.Clear();
		playerBoxFilled = false;
		FillPlayerHitboxes();
	}

	private static void FillPlayerHitboxes()
	{
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		if (playerBoxFilled)
		{
			return;
		}
		for (int i = 0; i < 255; i++)
		{
			Player player = Main.player[i];
			if (player.active && !player.dead && !player.ghost)
			{
				playerBox[i] = player.getRect();
			}
		}
		playerBoxFilled = true;
	}

	private static void UpdateEndInternal()
	{
		inUpdateLoop = false;
		foreach (Tuple<Point16, bool> tripPoint in tripPoints)
		{
			Wiring.blockPlayerTeleportationForOneIteration = tripPoint.Item2;
			Wiring.HitSwitch(tripPoint.Item1.X, tripPoint.Item1.Y);
			if (Main.netMode == 2)
			{
				NetMessage.SendData(59, -1, -1, null, tripPoint.Item1.X, tripPoint.Item1.Y);
			}
		}
		Wiring.blockPlayerTeleportationForOneIteration = false;
		tripPoints.Clear();
		foreach (int item in markedIDsForRemoval)
		{
			if (TileEntity.TryGet<TELogicSensor>(item, out var result))
			{
				TileEntity.Remove(result);
			}
		}
		markedIDsForRemoval.Clear();
	}

	public override void Update()
	{
		bool state = GetState(Position.X, Position.Y, logicCheck, this);
		switch (logicCheck)
		{
		case LogicCheckType.Day:
		case LogicCheckType.Night:
			if (!On & state)
			{
				ChangeState(onState: true, TripWire: true);
			}
			if (On && !state)
			{
				ChangeState(onState: false, TripWire: false);
			}
			break;
		case LogicCheckType.PlayerAbove:
		case LogicCheckType.Water:
		case LogicCheckType.Lava:
		case LogicCheckType.Honey:
		case LogicCheckType.Liquid:
			if (On != state)
			{
				ChangeState(state, TripWire: true);
			}
			break;
		}
	}

	public void ChangeState(bool onState, bool TripWire)
	{
		if (onState == On || SanityCheck(Position.X, Position.Y))
		{
			Main.tile[Position.X, Position.Y].frameX = (short)(onState ? 18 : 0);
			On = onState;
			if (Main.netMode == 2)
			{
				NetMessage.SendTileSquare(-1, Position.X, Position.Y);
			}
			if (TripWire && Main.netMode != 1)
			{
				tripPoints.Add(Tuple.Create(Position, logicCheck == LogicCheckType.PlayerAbove));
			}
		}
	}

	public static bool ValidTile(int x, int y)
	{
		if (!Main.tile[x, y].active() || Main.tile[x, y].type != 423 || Main.tile[x, y].frameY % 18 != 0 || Main.tile[x, y].frameX % 18 != 0)
		{
			return false;
		}
		return true;
	}

	public TELogicSensor()
	{
		logicCheck = LogicCheckType.None;
		On = false;
		RequiresUpdates = true;
	}

	public static LogicCheckType FigureCheckType(int x, int y, out bool on)
	{
		on = false;
		if (!WorldGen.InWorld(x, y))
		{
			return LogicCheckType.None;
		}
		Tile tile = Main.tile[x, y];
		if (tile == null)
		{
			return LogicCheckType.None;
		}
		LogicCheckType result = LogicCheckType.None;
		switch (tile.frameY / 18)
		{
		case 0:
			result = LogicCheckType.Day;
			break;
		case 1:
			result = LogicCheckType.Night;
			break;
		case 2:
			result = LogicCheckType.PlayerAbove;
			break;
		case 3:
			result = LogicCheckType.Water;
			break;
		case 4:
			result = LogicCheckType.Lava;
			break;
		case 5:
			result = LogicCheckType.Honey;
			break;
		case 6:
			result = LogicCheckType.Liquid;
			break;
		}
		on = GetState(x, y, result);
		return result;
	}

	public static bool GetState(int x, int y, LogicCheckType type, TELogicSensor instance = null)
	{
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		switch (type)
		{
		case LogicCheckType.Day:
			return Main.dayTime;
		case LogicCheckType.Night:
			return !Main.dayTime;
		case LogicCheckType.PlayerAbove:
		{
			bool result = false;
			Rectangle val = new Rectangle(x * 16 - 32 - 1, y * 16 - 160 - 1, 82, 162);
			foreach (KeyValuePair<int, Rectangle> item in playerBox)
			{
				Rectangle value = item.Value;
				if (value.Intersects(val))
				{
					result = true;
					break;
				}
			}
			return result;
		}
		case LogicCheckType.Water:
		case LogicCheckType.Lava:
		case LogicCheckType.Honey:
		case LogicCheckType.Liquid:
		{
			if (instance == null)
			{
				return false;
			}
			Tile tile = Main.tile[x, y];
			bool flag = true;
			if (tile == null || tile.liquid == 0)
			{
				flag = false;
			}
			if (!tile.lava() && type == LogicCheckType.Lava)
			{
				flag = false;
			}
			if (!tile.honey() && type == LogicCheckType.Honey)
			{
				flag = false;
			}
			if ((tile.honey() || tile.lava() || tile.shimmer()) && type == LogicCheckType.Water)
			{
				flag = false;
			}
			if (!flag && instance.On)
			{
				if (instance.CountedData == 0)
				{
					instance.CountedData = 15;
				}
				else if (instance.CountedData > 0)
				{
					instance.CountedData--;
				}
				flag = instance.CountedData > 0;
			}
			return flag;
		}
		default:
			return false;
		}
	}

	public void FigureCheckState()
	{
		logicCheck = FigureCheckType(Position.X, Position.Y, out On);
		GetFrame(Position.X, Position.Y, logicCheck, On);
	}

	public static void GetFrame(int x, int y, LogicCheckType type, bool on)
	{
		Main.tile[x, y].frameX = (short)(on ? 18 : 0);
		switch (type)
		{
		case LogicCheckType.Day:
			Main.tile[x, y].frameY = 0;
			break;
		case LogicCheckType.Night:
			Main.tile[x, y].frameY = 18;
			break;
		case LogicCheckType.PlayerAbove:
			Main.tile[x, y].frameY = 36;
			break;
		case LogicCheckType.Water:
			Main.tile[x, y].frameY = 54;
			break;
		case LogicCheckType.Lava:
			Main.tile[x, y].frameY = 72;
			break;
		case LogicCheckType.Honey:
			Main.tile[x, y].frameY = 90;
			break;
		case LogicCheckType.Liquid:
			Main.tile[x, y].frameY = 108;
			break;
		default:
			Main.tile[x, y].frameY = 0;
			break;
		}
	}

	public static bool SanityCheck(int x, int y)
	{
		if (!Main.tile[x, y].active() || Main.tile[x, y].type != 423)
		{
			Kill(x, y);
			return false;
		}
		return true;
	}

	public static int Hook_AfterPlacement(int x, int y, int type = 423, int style = 0, int direction = 1, int alternate = 0)
	{
		LogicCheckType logicCheckType = FigureCheckType(x, y, out var on);
		GetFrame(x, y, logicCheckType, on);
		if (Main.netMode == 1)
		{
			NetMessage.SendTileSquare(Main.myPlayer, x, y);
			NetMessage.SendData(87, -1, -1, null, x, y, (int)TileEntityType<TELogicSensor>.EntityTypeID);
			return -1;
		}
		return TileEntityType<TELogicSensor>.Place(x, y);
	}

	public new static void Kill(int x, int y)
	{
		if (TileEntity.TryGetAt<TELogicSensor>(x, y, out var result))
		{
			Wiring.blockPlayerTeleportationForOneIteration = result.logicCheck == LogicCheckType.PlayerAbove;
			bool flag = false;
			if (result.logicCheck == LogicCheckType.PlayerAbove && result.On)
			{
				flag = true;
			}
			else if (result.logicCheck == LogicCheckType.Water && result.On)
			{
				flag = true;
			}
			else if (result.logicCheck == LogicCheckType.Lava && result.On)
			{
				flag = true;
			}
			else if (result.logicCheck == LogicCheckType.Honey && result.On)
			{
				flag = true;
			}
			else if (result.logicCheck == LogicCheckType.Liquid && result.On)
			{
				flag = true;
			}
			if (flag)
			{
				Wiring.HitSwitch(result.Position.X, result.Position.Y);
				NetMessage.SendData(59, -1, -1, null, result.Position.X, result.Position.Y);
			}
			Wiring.blockPlayerTeleportationForOneIteration = false;
			if (inUpdateLoop)
			{
				markedIDsForRemoval.Add(result.ID);
			}
			else
			{
				TileEntity.Remove(result);
			}
		}
	}

	public override void WriteExtraData(BinaryWriter writer, bool networkSend)
	{
		if (!networkSend)
		{
			writer.Write((byte)logicCheck);
			writer.Write(On);
		}
	}

	public override void ReadExtraData(BinaryReader reader, int gameVersion, bool networkSend)
	{
		if (!networkSend)
		{
			logicCheck = (LogicCheckType)reader.ReadByte();
			On = reader.ReadBoolean();
		}
	}

	public override string ToString()
	{
		return Position.X + "x  " + Position.Y + "y " + logicCheck;
	}
}
