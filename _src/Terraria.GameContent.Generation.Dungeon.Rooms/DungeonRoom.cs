using System;
using Microsoft.Xna.Framework;
using ReLogic.Utilities;
using Terraria.GameContent.Generation.Dungeon.Features;
using Terraria.Utilities;

namespace Terraria.GameContent.Generation.Dungeon.Rooms;

public abstract class DungeonRoom
{
	public DungeonRoomSettings settings;

	public bool calculated;

	public bool generated;

	public DungeonBounds InnerBounds = new DungeonBounds();

	public DungeonBounds OuterBounds = new DungeonBounds();

	public bool Processed
	{
		get
		{
			if (!calculated)
			{
				return generated;
			}
			return true;
		}
	}

	public Point Center
	{
		get
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			return InnerBounds.Center;
		}
	}

	public DungeonRoom(DungeonRoomSettings settings)
	{
		this.settings = settings;
	}

	public virtual bool CanGenerateFeatureAt(DungeonData data, IDungeonFeature feature, int x, int y)
	{
		if (feature is DungeonWindow && data.Type != DungeonType.DualDungeon)
		{
			return false;
		}
		if (feature is DungeonPitTrap && ((DungeonPitTrapSettings)((DungeonPitTrap)feature).settings).ConnectedRoom != this)
		{
			return false;
		}
		return settings.StyleData.CanGenerateFeatureAt(data, this, feature, x, y);
	}

	public virtual void GeneratePreHallwaysDungeonFeaturesInRoom(DungeonData data)
	{
		if ((settings.StyleData.Style == 4 || settings.StyleData.Style == 5) && InnerBounds.Width > 10 && InnerBounds.Height > 10)
		{
			DungeonUtils.GenerateSpeleothemsInArea(data, settings.StyleData, InnerBounds.Left, InnerBounds.Top, InnerBounds.Width, InnerBounds.Height, Math.Max(3, InnerBounds.Width / 3), settings.StyleData.BrickTileType, (settings.OverridePaintTile >= 0 || !settings.StyleData.TilePaintColor.HasValue) ? settings.OverridePaintTile : settings.StyleData.TilePaintColor.Value);
		}
	}

	public virtual void GenerateEarlyDungeonFeaturesInRoom(DungeonData data)
	{
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_011c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0121: Unknown result type (might be due to invalid IL or missing references)
		//IL_0127: Unknown result type (might be due to invalid IL or missing references)
		//IL_012e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0142: Unknown result type (might be due to invalid IL or missing references)
		//IL_0149: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0201: Unknown result type (might be due to invalid IL or missing references)
		//IL_020c: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01da: Unknown result type (might be due to invalid IL or missing references)
		//IL_0241: Unknown result type (might be due to invalid IL or missing references)
		//IL_024c: Unknown result type (might be due to invalid IL or missing references)
		//IL_021d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0228: Unknown result type (might be due to invalid IL or missing references)
		//IL_025d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0268: Unknown result type (might be due to invalid IL or missing references)
		UnifiedRandom unifiedRandom = new UnifiedRandom(settings.RandomSeed);
		if (data.Type != DungeonType.DualDungeon)
		{
			return;
		}
		if (unifiedRandom.Next(3) == 0)
		{
			int num = 1;
			DungeonWindowBasicSettings dungeonWindowBasicSettings = new DungeonWindowBasicSettings
			{
				Style = settings.StyleData,
				Closed = !((double)InnerBounds.Bottom <= Main.worldSurface)
			};
			int width = InnerBounds.Width;
			int height = InnerBounds.Height;
			bool flag = true;
			int num2 = unifiedRandom.Next(3);
			if (num2 >= 1 && num2 <= 2 && (width <= 36 || height <= 15))
			{
				num2 = 0;
			}
			if (num2 == 0 && (width <= 14 || height <= 10))
			{
				flag = false;
			}
			if (flag)
			{
				Point center = InnerBounds.Center;
				if (num2 == 0 || (uint)(num2 - 1) > 1u)
				{
					num = Math.Max(3, InnerBounds.Width / 3);
					if (num % 2 == 0)
					{
						num++;
					}
					dungeonWindowBasicSettings.Width = Math.Max(3, num);
					dungeonWindowBasicSettings.Height = Math.Max(5, InnerBounds.Height / 3);
					DungeonWindow dungeonWindow = new DungeonWindowBasic(dungeonWindowBasicSettings);
					center = GetRoomCenterForDungeonFeature(data, dungeonWindow);
					if (CanGenerateFeatureAt(data, dungeonWindow, center.X, center.Y))
					{
						dungeonWindow.GenerateFeature(data, center.X, center.Y);
					}
				}
				else
				{
					num = Math.Min(7, Math.Max(3, InnerBounds.Width / 5));
					if (num % 2 == 0)
					{
						num++;
					}
					dungeonWindowBasicSettings.Width = Math.Max(3, num);
					dungeonWindowBasicSettings.Height = Math.Max(5, InnerBounds.Height / 3);
					DungeonWindow dungeonWindow = new DungeonWindowBasic(dungeonWindowBasicSettings);
					center = GetRoomCenterForDungeonFeature(data, dungeonWindow);
					if (CanGenerateFeatureAt(data, dungeonWindow, center.X, center.Y))
					{
						dungeonWindow.GenerateFeature(data, center.X, center.Y);
					}
					dungeonWindowBasicSettings.Height -= 2;
					dungeonWindow = new DungeonWindowBasic(dungeonWindowBasicSettings);
					if (CanGenerateFeatureAt(data, dungeonWindow, center.X - num - 2, center.Y))
					{
						dungeonWindow.GenerateFeature(data, center.X - num - 2, center.Y);
					}
					dungeonWindow = new DungeonWindowBasic(dungeonWindowBasicSettings);
					if (CanGenerateFeatureAt(data, dungeonWindow, center.X + num + 2, center.Y))
					{
						dungeonWindow.GenerateFeature(data, center.X + num + 2, center.Y);
					}
				}
			}
		}
		int liquidType = settings.StyleData.LiquidType;
		if (liquidType >= 0)
		{
			FloodRoom((byte)liquidType);
		}
	}

	public virtual void GenerateLateDungeonFeaturesInRoom(DungeonData data)
	{
	}

	public virtual Point GetRoomCenterForDungeonFeature(DungeonData data, DungeonFeature feature)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		return Center;
	}

	public virtual Point GetRoomCenterForHallway(Vector2D otherRoomPos)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		return Center;
	}

	public abstract void CalculateRoom(DungeonData data);

	public virtual void CalculatePlatformsAndDoors(DungeonData data)
	{
		DungeonUtils.CalculatePlatformsAndDoorsOnEdgesOfRoom(data, InnerBounds, settings.ForceStyleForDoorsAndPlatforms ? settings.StyleData : null, 3, 3);
	}

	public virtual ConnectionPointQuality GetHallwayConnectionPoint(Vector2D otherRoomPos, out Vector2D connectionPoint)
	{
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_0113: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0178: Unknown result type (might be due to invalid IL or missing references)
		//IL_0194: Unknown result type (might be due to invalid IL or missing references)
		//IL_0199: Unknown result type (might be due to invalid IL or missing references)
		//IL_019e: Unknown result type (might be due to invalid IL or missing references)
		//IL_011b: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_01af: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01be: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0150: Unknown result type (might be due to invalid IL or missing references)
		//IL_0155: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_020c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0211: Unknown result type (might be due to invalid IL or missing references)
		//IL_0216: Unknown result type (might be due to invalid IL or missing references)
		if (settings.HallwayConnectionPointOverride != null)
		{
			ConnectionPointQuality result = settings.HallwayConnectionPointOverride(this, otherRoomPos, out connectionPoint);
			if (settings.HallwayPointAdjuster.HasValue)
			{
				Vector2D val = (otherRoomPos - connectionPoint).SafeNormalize(Vector2D.UnitX);
				connectionPoint -= val * (double)settings.HallwayPointAdjuster.Value;
			}
			return result;
		}
		connectionPoint = (Vector2D)(GetRoomCenterForHallway(otherRoomPos));
		Vector2D val2 = (otherRoomPos - connectionPoint).SafeNormalize(Vector2D.UnitX);
		if (-0.5 < val2.Y && val2.Y < 0.7 && WorldGen.genRand.Next(2) == 0)
		{
			while (IsInsideRoom(connectionPoint.ToPoint()))
			{
				connectionPoint.Y++;
			}
			connectionPoint.Y -= 3.0;
		}
		else if (-0.7 < val2.Y && val2.Y < 0.5 && WorldGen.genRand.Next(3) == 0)
		{
			while (IsInsideRoom(connectionPoint.ToPoint()))
			{
				connectionPoint.Y--;
			}
			connectionPoint.Y += 3.0;
		}
		else
		{
			connectionPoint += WorldGen.genRand.NextVector2DCircularEdge(4.0, 4.0);
		}
		val2 = (otherRoomPos - connectionPoint).SafeNormalize(Vector2D.UnitX);
		while (IsInsideRoom(connectionPoint.ToPoint()))
		{
			connectionPoint += val2;
		}
		if (settings.HallwayPointAdjuster.HasValue)
		{
			connectionPoint -= val2 * (double)settings.HallwayPointAdjuster.Value;
		}
		return ConnectionPointQuality.Good;
	}

	public abstract bool GenerateRoom(DungeonData data);

	public virtual bool TryGenerateChestInRoom(DungeonData data, DungeonGlobalBasicChests feature)
	{
		return DungeonUtils.GenerateDungeonRegularChest(data, feature, settings.StyleData, InnerBounds);
	}

	public virtual bool DualDungeons_TryGenerateBiomeChestInRoom(DungeonData data, DungeonGlobalBiomeChests feature)
	{
		return DungeonUtils.GenerateDungeonBiomeChest(data, feature, settings.StyleData, InnerBounds);
	}

	public virtual ProtectionType GetProtectionTypeFromPoint(int x, int y)
	{
		if (!OuterBounds.Contains(x, y))
		{
			return ProtectionType.None;
		}
		return ProtectionType.Walls;
	}

	public bool IsInsideRoom(Point point)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		return IsInsideRoom(point.X, point.Y);
	}

	public virtual bool IsInsideRoom(int x, int y)
	{
		return InnerBounds.Contains(x, y);
	}

	public virtual int GetFloodedRoomTileCount()
	{
		return InnerBounds.Width * InnerBounds.Height;
	}

	public virtual void FloodRoom(byte liquidType)
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		for (int i = InnerBounds.Left; i <= InnerBounds.Right; i++)
		{
			for (int j = InnerBounds.Center.Y; j <= InnerBounds.Bottom; j++)
			{
				Tile tile = Main.tile[i, j];
				if (!tile.active())
				{
					tile.liquid = byte.MaxValue;
					tile.liquidType(liquidType);
				}
			}
		}
	}

	public virtual int GetFurnitureCount(int defaultCount)
	{
		return defaultCount;
	}

	public void GenerateDungeonSquareRoom(DungeonData data, DungeonBounds innerBounds, DungeonBounds outerBounds, Vector2D currentPoint, DungeonGenerationStyleData styleData, int innerBoundsSize, int totalBoundsSize, bool genTiles = true, bool genWalls = true)
	{
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		byte? b = ((settings.OverridePaintTile > -1) ? new byte?((byte)settings.OverridePaintTile) : styleData.TilePaintColor);
		GenerateDungeonSquareRoom(wallPaint: (settings.OverridePaintWall > -1) ? new byte?((byte)settings.OverridePaintWall) : styleData.WallPaintColor, data: data, innerBounds: innerBounds, outerBounds: outerBounds, currentPoint: currentPoint, tileType: styleData.BrickTileType, wallType: styleData.BrickWallType, newStyleID: styleData.Style, tilePaint: b, innerBoundsSize: innerBoundsSize, totalBoundsSize: totalBoundsSize, genTiles: genTiles, genWalls: genWalls);
	}

	public void GenerateDungeonSquareRoom(DungeonData data, DungeonBounds innerBounds, DungeonBounds outerBounds, Vector2D currentPoint, ushort tileType, ushort wallType, int newStyleID, int? tilePaint, int? wallPaint, int innerBoundsSize, int totalBoundsSize, bool genTiles = true, bool genWalls = true)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		for (int i = -totalBoundsSize; i <= totalBoundsSize; i++)
		{
			int num = (int)currentPoint.X + i;
			for (int j = -totalBoundsSize; j <= totalBoundsSize; j++)
			{
				int num2 = (int)currentPoint.Y + j;
				Tile tile = Main.tile[num, num2];
				if (Math.Abs(i) <= innerBoundsSize && Math.Abs(j) <= innerBoundsSize)
				{
					innerBounds.UpdateBounds(num, num2);
					if (!CanRemoveTileAt(data, tile))
					{
						continue;
					}
					if (genWalls)
					{
						DungeonUtils.ChangeWallType(tile, wallType, resetTile: true, wallPaint);
					}
				}
				else
				{
					outerBounds.UpdateBounds(num, num2);
					if (CanPlaceTileAt(data, tile, tileType, wallType, newStyleID))
					{
						bool flag = genWalls && i > -totalBoundsSize && i < totalBoundsSize && j > -totalBoundsSize && j < totalBoundsSize;
						if (genTiles)
						{
							DungeonUtils.ChangeTileType(tile, tileType, flag, tilePaint);
						}
						if (flag)
						{
							DungeonUtils.ChangeWallType(tile, wallType, resetTile: false, wallPaint);
						}
					}
				}
				tile.liquid = 0;
			}
		}
	}

	public virtual bool CanPlaceTileAt(DungeonData data, Tile tile, int tileType, int wallType, int newStyleID)
	{
		if (tile.wall == 350)
		{
			return false;
		}
		return !DungeonUtils.IsHigherOrEqualTieredDungeonWall(data, tile.wall, wallType, newStyleID, newStyleID, includeWindows: false, includeExtras: true);
	}

	public virtual bool CanRemoveTileAt(DungeonData data, Tile tile)
	{
		if (tile.wall == 350)
		{
			return false;
		}
		return true;
	}
}
