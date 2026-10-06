using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Terraria.Utilities;
using Terraria.WorldBuilding;

namespace Terraria.GameContent.Generation.Dungeon.Rooms;

public class LivingTreeDungeonRoom(DungeonRoomSettings settings) : DungeonRoom(settings)
{
	private ShapeData _innerShapeData = new ShapeData();

	private ShapeData _outerShapeData = new ShapeData();

	private int _floodedTileCount;

	public Point[] Positions;

	public override void CalculateRoom(DungeonData data)
	{
		calculated = false;
		int x = settings.RoomPosition.X;
		int y = settings.RoomPosition.Y;
		LivingTreeRoom(data, x, y, generating: false);
		calculated = true;
	}

	public override bool GenerateRoom(DungeonData data)
	{
		generated = false;
		int x = settings.RoomPosition.X;
		int y = settings.RoomPosition.Y;
		LivingTreeRoom(data, x, y, generating: true);
		generated = true;
		return true;
	}

	public override int GetFloodedRoomTileCount()
	{
		return _floodedTileCount;
	}

	public override void FloodRoom(byte liquidType)
	{
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		if (_innerShapeData == null || Positions == null)
		{
			base.FloodRoom(liquidType);
			return;
		}
		_ = (WormlikeDungeonRoomSettings)settings;
		WorldUtils.Gen(Positions[0], new ModShapes.All(_innerShapeData), Actions.Chain(new Modifiers.IsBelowHeight(Center.Y, inclusive: true), new Modifiers.IsNotSolid(), new Actions.SetLiquid(liquidType)));
	}

	public override ProtectionType GetProtectionTypeFromPoint(int x, int y)
	{
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		if (_innerShapeData == null || _outerShapeData == null || Positions == null || (calculated && !OuterBounds.Contains(x, y)))
		{
			return base.GetProtectionTypeFromPoint(x, y);
		}
		Point val = Positions[0];
		if (!_outerShapeData.Contains(x - val.X, y - val.Y))
		{
			return ProtectionType.None;
		}
		return ProtectionType.Walls;
	}

	public override bool IsInsideRoom(int x, int y)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		if (Positions == null)
		{
			return base.IsInsideRoom(x, y);
		}
		Point val = Positions[0];
		if (base.IsInsideRoom(x, y))
		{
			return _innerShapeData.Contains(x - val.X, y - val.Y);
		}
		return false;
	}

	public override void GeneratePreHallwaysDungeonFeaturesInRoom(DungeonData data)
	{
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_019d: Unknown result type (might be due to invalid IL or missing references)
		//IL_01aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0258: Unknown result type (might be due to invalid IL or missing references)
		//IL_0265: Unknown result type (might be due to invalid IL or missing references)
		UnifiedRandom unifiedRandom = new UnifiedRandom(settings.RandomSeed);
		ushort brickTileType = settings.StyleData.BrickTileType;
		ushort brickCrackedTileType = settings.StyleData.BrickCrackedTileType;
		byte? b = ((settings.OverridePaintTile > -1) ? new byte?((byte)settings.OverridePaintTile) : settings.StyleData.TilePaintColor);
		if (settings.OverridePaintWall <= -1)
		{
			_ = settings.StyleData.WallPaintColor;
		}
		else
		{
			_ = settings.OverridePaintWall;
		}
		int growthLength = (int)((float)InnerBounds.Height * 0.1f) + unifiedRandom.Next(4);
		int branchDensity = 2 + unifiedRandom.Next(2);
		int leafDensity = 3 + unifiedRandom.Next(4);
		DungeonUtils.GenerateHangingLeafCluster(startPoint: new Point(InnerBounds.Center.X, InnerBounds.Top), data: data, genRand: unifiedRandom, bounds: OuterBounds, growthLength: growthLength, branchDensity: branchDensity, leafDensity: leafDensity, leafType: brickCrackedTileType, woodType: brickTileType, leafPaintColor: b, woodPaintColor: b);
		growthLength = (int)((float)InnerBounds.Height * 0.15f) + unifiedRandom.Next(5);
		branchDensity = 3 + unifiedRandom.Next(2);
		leafDensity = 4 + unifiedRandom.Next(4);
		DungeonUtils.GenerateHangingLeafCluster(startPoint: new Point(InnerBounds.Left + 2 + unifiedRandom.Next(3), InnerBounds.Top), data: data, genRand: unifiedRandom, bounds: OuterBounds, growthLength: growthLength, branchDensity: branchDensity, leafDensity: leafDensity, leafType: brickCrackedTileType, woodType: brickTileType, leafPaintColor: b, woodPaintColor: b);
		growthLength = (int)((float)InnerBounds.Height * 0.15f) + unifiedRandom.Next(5);
		branchDensity = 3 + unifiedRandom.Next(2);
		leafDensity = 4 + unifiedRandom.Next(4);
		DungeonUtils.GenerateHangingLeafCluster(startPoint: new Point(InnerBounds.Right - 2 - unifiedRandom.Next(3), InnerBounds.Top), data: data, genRand: unifiedRandom, bounds: OuterBounds, growthLength: growthLength, branchDensity: branchDensity, leafDensity: leafDensity, leafType: brickCrackedTileType, woodType: brickTileType, leafPaintColor: b, woodPaintColor: b);
		base.GeneratePreHallwaysDungeonFeaturesInRoom(data);
	}

	public override void GenerateLateDungeonFeaturesInRoom(DungeonData data)
	{
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d8: Unknown result type (might be due to invalid IL or missing references)
		UnifiedRandom unifiedRandom = new UnifiedRandom(settings.RandomSeed);
		_ = (LivingTreeDungeonRoomSettings)settings;
		ushort brickTileType = settings.StyleData.BrickTileType;
		ushort brickCrackedTileType = settings.StyleData.BrickCrackedTileType;
		ushort brickWallType = settings.StyleData.BrickWallType;
		byte? b = ((settings.OverridePaintTile > -1) ? new byte?((byte)settings.OverridePaintTile) : settings.StyleData.TilePaintColor);
		for (int i = 0; i < 50; i++)
		{
			int num = unifiedRandom.Next(InnerBounds.Left + 1, InnerBounds.Right);
			int num2 = unifiedRandom.Next(InnerBounds.Top + 1, InnerBounds.Bottom);
			Point val = DungeonUtils.FirstSolid(ceiling: false, new Point(num, num2), InnerBounds);
			num = val.X;
			num2 = val.Y - 1;
			Tile tile = Main.tile[num, num2];
			if (tile.active() || tile.wall != brickWallType)
			{
				continue;
			}
			if (unifiedRandom.Next(2) == 0)
			{
				WorldGen.PlaceTile(num, num2, 187, mute: true, forced: false, -1, unifiedRandom.Next(47, 50));
				continue;
			}
			int num3 = unifiedRandom.Next(2);
			int pileStyle = 72;
			if (num3 == 1)
			{
				pileStyle = unifiedRandom.Next(59, 62);
			}
			WorldGen.PlaceSmallPile(num, num2, pileStyle, num3, 185);
		}
		for (int j = 0; j < 10; j++)
		{
			int num4 = unifiedRandom.Next(InnerBounds.Left + 1, InnerBounds.Right);
			int num5 = unifiedRandom.Next(InnerBounds.Top + 1, InnerBounds.Bottom);
			Point val2 = DungeonUtils.FirstSolid(ceiling: true, new Point(num4, num5), InnerBounds);
			num4 = val2.X;
			num5 = val2.Y + 1;
			Tile tile2 = Main.tile[num4, num5];
			Tile tile3 = Main.tile[num4, num5 - 1];
			if (tile2.active() || tile2.wall != brickWallType || !tile3.active() || tile3.type != brickCrackedTileType)
			{
				continue;
			}
			ushort type = 52;
			if (brickTileType == 383)
			{
				type = 62;
			}
			for (int num6 = unifiedRandom.Next(3, 12); num6 > 0; num6--)
			{
				Tile tile4 = Main.tile[num4, num5];
				if (tile4.active())
				{
					break;
				}
				tile4.ClearTile();
				tile4.active(active: true);
				tile4.type = type;
				if (b.HasValue && b.Value >= 0)
				{
					WorldGen.paintTile(num4, num5, b.Value, broadCast: false, paintEffects: false);
				}
				num5++;
			}
		}
	}

	public void LivingTreeRoom(DungeonData data, int i, int j, bool generating)
	{
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0120: Unknown result type (might be due to invalid IL or missing references)
		//IL_0126: Unknown result type (might be due to invalid IL or missing references)
		//IL_012c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0132: Unknown result type (might be due to invalid IL or missing references)
		//IL_0143: Unknown result type (might be due to invalid IL or missing references)
		//IL_0149: Unknown result type (might be due to invalid IL or missing references)
		//IL_014f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0155: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0184: Unknown result type (might be due to invalid IL or missing references)
		//IL_018e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0195: Unknown result type (might be due to invalid IL or missing references)
		//IL_019f: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0206: Unknown result type (might be due to invalid IL or missing references)
		//IL_0210: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0306: Unknown result type (might be due to invalid IL or missing references)
		//IL_023a: Unknown result type (might be due to invalid IL or missing references)
		//IL_023c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0225: Unknown result type (might be due to invalid IL or missing references)
		//IL_0284: Unknown result type (might be due to invalid IL or missing references)
		//IL_027b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0280: Unknown result type (might be due to invalid IL or missing references)
		UnifiedRandom unifiedRandom = new UnifiedRandom(settings.RandomSeed);
		LivingTreeDungeonRoomSettings livingTreeDungeonRoomSettings = (LivingTreeDungeonRoomSettings)settings;
		_ = settings.StyleData.BrickTileType;
		_ = settings.StyleData.BrickCrackedTileType;
		_ = settings.StyleData.BrickWallType;
		if (settings.OverridePaintTile <= -1)
		{
			_ = settings.StyleData.TilePaintColor;
		}
		else
		{
			_ = settings.OverridePaintTile;
		}
		if (settings.OverridePaintWall <= -1)
		{
			_ = settings.StyleData.WallPaintColor;
		}
		else
		{
			_ = settings.OverridePaintWall;
		}
		List<Point> list = new List<Point>();
		Point val = new Point(i, j);
		if (calculated)
		{
			val = Positions[0];
		}
		else
		{
			list.Add(val);
		}
		Point val2 = new Point(val.X, val.Y + livingTreeDungeonRoomSettings.InnerHeight / 2);
		int num = val2.Y - livingTreeDungeonRoomSettings.InnerHeight;
		int innerWidth = livingTreeDungeonRoomSettings.InnerWidth;
		int depth = livingTreeDungeonRoomSettings.Depth;
		int num2 = innerWidth;
		int num3 = num2 + depth;
		OuterBounds.SetBounds(val.X, val.Y, val.X, val.Y);
		InnerBounds.SetBounds(val.X, val.Y, val.X, val.Y);
		_outerShapeData.Clear();
		_innerShapeData.Clear();
		int num4 = 1;
		while ((!Processed && val2.Y > num) || (Processed && num4 < Positions.Length))
		{
			_outerShapeData.AddBounds(val2.X - num3 - val.X, val2.Y - num3 - val.Y, val2.X + num3 - val.X, val2.Y + num3 - val.Y);
			_innerShapeData.AddBounds(val2.X - num2 - val.X, val2.Y - num2 - val.Y, val2.X + num2 - val.X, val2.Y + num2 - val.Y);
			if (!Processed)
			{
				list.Add(val2);
			}
			GenerateDungeonSquareRoom(data, InnerBounds, OuterBounds, val2.ToVector2D(), settings.StyleData, num2, num3, generating, generating);
			if (Processed)
			{
				num4++;
				if (num4 < Positions.Length)
				{
					val2 = Positions[num4];
				}
			}
			else
			{
				if (val2.Y % 4 == 0)
				{
					val2.X += ((unifiedRandom.Next(2) != 0) ? 1 : (-1));
				}
				val2.Y--;
			}
		}
		if (!Processed)
		{
			Positions = Enumerable.ToArray(list);
		}
		InnerBounds.CalculateHitbox();
		OuterBounds.CalculateHitbox();
		_floodedTileCount = DungeonUtils.CalculateFloodedTileCountFromShapeData(InnerBounds, _innerShapeData);
	}
}
