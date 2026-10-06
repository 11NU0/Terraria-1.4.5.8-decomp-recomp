using System;
using Microsoft.Xna.Framework;
using Terraria.Testing;

namespace Terraria.WorldBuilding;

public static class WorldUtils
{
	public static Rectangle ClampToWorld(Rectangle tileRectangle, int fluff = 0)
	{
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		int num = Math.Max(fluff, Math.Min(tileRectangle.Left, Main.maxTilesX - fluff));
		int num2 = Math.Max(fluff, Math.Min(tileRectangle.Top, Main.maxTilesY - fluff));
		int num3 = Math.Max(fluff, Math.Min(tileRectangle.Right, Main.maxTilesX - fluff));
		int num4 = Math.Max(fluff, Math.Min(tileRectangle.Bottom, Main.maxTilesY - fluff));
		return new Rectangle(num, num2, num3 - num, num4 - num2);
	}

	public static Rectangle GetWorldPlayArea()
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		int num = 640;
		Point val = new Point((int)Main.leftWorld + num, (int)Main.topWorld + num);
		Point val2 = new Point((int)Main.rightWorld - num, (int)Main.bottomWorld - num);
		return new Rectangle(val.X, val.Y, val2.X - val.X, val2.Y - val.Y);
	}

	public static Rectangle ClampToWorldBorders(Rectangle worldRect)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		if (DebugOptions.noLimits)
		{
			return worldRect;
		}
		return Utils.Clamp(worldRect, GetWorldPlayArea());
	}

	public static bool Gen(Point origin, GenShape shape, GenAction action)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		return shape.Perform(origin, action);
	}

	public static bool Gen(Point origin, GenShapeActionPair pair)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		return pair.Shape.Perform(origin, pair.Action);
	}

	public static bool Find(Point origin, GenSearch search, out Point result)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		result = search.Find(origin);
		if (result == GenSearch.NOT_FOUND)
		{
			return false;
		}
		return true;
	}

	public static void ClearTile(int x, int y, bool frameNeighbors = false)
	{
		Main.tile[x, y].ClearTile();
		if (frameNeighbors)
		{
			WorldGen.TileFrame(x + 1, y);
			WorldGen.TileFrame(x - 1, y);
			WorldGen.TileFrame(x, y + 1);
			WorldGen.TileFrame(x, y - 1);
		}
	}

	public static void ClearWall(int x, int y, bool frameNeighbors = false)
	{
		Main.tile[x, y].wall = 0;
		if (frameNeighbors)
		{
			WorldGen.SquareWallFrame(x + 1, y);
			WorldGen.SquareWallFrame(x - 1, y);
			WorldGen.SquareWallFrame(x, y + 1);
			WorldGen.SquareWallFrame(x, y - 1);
		}
	}

	public static void TileFrame(int x, int y, bool frameNeighbors = false)
	{
		WorldGen.TileFrame(x, y, resetFrame: true);
		if (frameNeighbors)
		{
			WorldGen.TileFrame(x + 1, y, resetFrame: true);
			WorldGen.TileFrame(x - 1, y, resetFrame: true);
			WorldGen.TileFrame(x, y + 1, resetFrame: true);
			WorldGen.TileFrame(x, y - 1, resetFrame: true);
		}
	}

	public static void WallFrame(int x, int y, bool frameNeighbors = false)
	{
		Framing.WallFrame(x, y, resetFrame: true);
		if (frameNeighbors)
		{
			Framing.WallFrame(x + 1, y, resetFrame: true);
			Framing.WallFrame(x - 1, y, resetFrame: true);
			Framing.WallFrame(x, y + 1, resetFrame: true);
			Framing.WallFrame(x, y - 1, resetFrame: true);
		}
	}

	public static void ClearChestLocation(int x, int y)
	{
		ClearTile(x, y, frameNeighbors: true);
		ClearTile(x - 1, y, frameNeighbors: true);
		ClearTile(x, y - 1, frameNeighbors: true);
		ClearTile(x - 1, y - 1, frameNeighbors: true);
	}

	public static void WireLine(Point start, Point end)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		//IL_0004: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		Point val = start;
		Point val2 = end;
		if (end.X < start.X)
		{
			Utils.Swap(ref end.X, ref start.X);
		}
		if (end.Y < start.Y)
		{
			Utils.Swap(ref end.Y, ref start.Y);
		}
		for (int i = start.X; i <= end.X; i++)
		{
			WorldGen.PlaceWire(i, val.Y);
		}
		for (int j = start.Y; j <= end.Y; j++)
		{
			WorldGen.PlaceWire(val2.X, j);
		}
	}

	public static void DebugRegen()
	{
		WorldGen.GenerateWorld();
		Main.NewText("World Regen Complete.");
	}

	public static void DebugRotate()
	{
		int num = 0;
		int num2 = 0;
		int maxTilesY = Main.maxTilesY;
		for (int i = 0; i < Main.maxTilesX / Main.maxTilesY; i++)
		{
			for (int j = 0; j < maxTilesY / 2; j++)
			{
				for (int k = j; k < maxTilesY - j; k++)
				{
					Tile tile = Main.tile[k + num, j + num2];
					Main.tile[k + num, j + num2] = Main.tile[j + num, maxTilesY - k + num2];
					Main.tile[j + num, maxTilesY - k + num2] = Main.tile[maxTilesY - k + num, maxTilesY - j + num2];
					Main.tile[maxTilesY - k + num, maxTilesY - j + num2] = Main.tile[maxTilesY - j + num, k + num2];
					Main.tile[maxTilesY - j + num, k + num2] = tile;
				}
			}
			num += maxTilesY;
		}
	}
}
