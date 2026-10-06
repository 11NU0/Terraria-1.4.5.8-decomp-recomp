using System;
using Microsoft.Xna.Framework;
using Terraria.GameContent.Generation.Dungeon.Rooms;
using Terraria.ID;

namespace Terraria.GameContent.Generation.Dungeon.Features;

public class DungeonPitTrap : DungeonFeature
{
	public bool Flooded;

	public DungeonPitTrap(DungeonFeatureSettings settings, bool addToFeatures = true)
		: base(settings)
	{
		if (addToFeatures)
		{
			DungeonCrawler.CurrentDungeonData.dungeonFeatures.Add(this);
		}
	}

	public override bool GenerateFeature(DungeonData data, int x, int y)
	{
		generated = false;
		DungeonGenerationStyleData style = ((DungeonPitTrapSettings)settings).Style;
		if (PitTrap(data, x, y, style, generating: true))
		{
			generated = true;
			return true;
		}
		return false;
	}

	public override bool CanGenerateFeatureAt(DungeonData data, IDungeonFeature feature, int x, int y)
	{
		if (feature is DungeonGlobalPaintings || feature is DungeonGlobalWallVariants)
		{
			return true;
		}
		return false;
	}

	public bool PitTrap(DungeonData data, int i, int j, DungeonGenerationStyleData styleData, bool generating = false)
	{
		//IL_03aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a10: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a15: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b8d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b8f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bbf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ba1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ba8: Unknown result type (might be due to invalid IL or missing references)
		_ = WorldGen.genRand;
		DungeonPitTrapSettings dungeonPitTrapSettings = (DungeonPitTrapSettings)settings;
		bool flag = data.Type == DungeonType.DualDungeon;
		ushort brickTileType = styleData.BrickTileType;
		ushort pitTrapTileType = styleData.PitTrapTileType;
		ushort brickWallType = styleData.BrickWallType;
		byte? tilePaintColor = styleData.TilePaintColor;
		byte? wallPaintColor = styleData.WallPaintColor;
		bool flag2 = TileID.Sets.Falling[pitTrapTileType];
		bool flag3 = TileID.Sets.CrackedBricks[pitTrapTileType];
		bool flag4 = !flag2 && !flag3;
		int num = j;
		int num2 = num;
		int width = dungeonPitTrapSettings.Width;
		int num3 = dungeonPitTrapSettings.Height;
		if (width < 1 || num3 < 1)
		{
			return false;
		}
		if (flag && (Flooded || dungeonPitTrapSettings.Flooded))
		{
			int num4 = 300;
			for (int num5 = width * Math.Max(1, num3 - dungeonPitTrapSettings.TopDensity); num5 < num4; num5 = width * Math.Max(1, num3 - dungeonPitTrapSettings.TopDensity))
			{
				num3++;
			}
		}
		int num6 = width + dungeonPitTrapSettings.EdgeWidth;
		int num7 = num3 + dungeonPitTrapSettings.EdgeHeight;
		if (!WorldGen.InWorld(i, num, (num6 > num7) ? num6 : num7))
		{
			return false;
		}
		if (!DungeonUtils.IsConsideredDungeonWall(Main.tile[i, num].wall))
		{
			return false;
		}
		if (flag && !data.genVars.outerPotentialDungeonBounds.Contains(i, num))
		{
			return false;
		}
		if (Main.tile[i, num].active())
		{
			return false;
		}
		for (int k = num; k < Main.maxTilesY; k++)
		{
			if (k > Main.UnderworldLayer)
			{
				return false;
			}
			if (Main.tile[i, k].active() && WorldGen.SolidTile(i, k))
			{
				if (Main.tile[i, k].type == 48)
				{
					return false;
				}
				num = k;
				num2 = k;
				break;
			}
		}
		if (!DungeonUtils.IsConsideredDungeonWall(Main.tile[i - width, num].wall) || !DungeonUtils.IsConsideredDungeonWall(Main.tile[i + width, num].wall))
		{
			return false;
		}
		if (data.Type == DungeonType.DualDungeon)
		{
			for (int l = i - num6; l <= i + num6; l++)
			{
				for (int m = num; m < num + num7 + 2; m++)
				{
					Tile tile = Main.tile[l, m];
					if (tile.active() && tile.type != brickTileType)
					{
						return false;
					}
					if (tile.wall != brickWallType)
					{
						return false;
					}
				}
			}
		}
		int num8 = 30;
		bool flag5 = true;
		for (int n = num; n < num + num8; n++)
		{
			flag5 = true;
			for (int num9 = i - width; num9 <= i + width; num9++)
			{
				Tile tile2 = Main.tile[num9, n];
				if (tile2.active() && DungeonUtils.IsConsideredDungeonTile(tile2.type))
				{
					flag5 = false;
					break;
				}
			}
			if (flag5)
			{
				num = n;
				break;
			}
		}
		if (num + num7 >= Main.UnderworldLayer)
		{
			return false;
		}
		int[] array = new int[num6 * 2 + 1];
		if (flag)
		{
			for (int num10 = i - num6; num10 <= i + num6; num10++)
			{
				int num11 = num;
				Tile tile3 = Main.tile[num10, num11];
				while (num11 > 10 && tile3.active() && (DungeonUtils.IsConsideredDungeonTile(tile3.type) || DungeonUtils.IsConsideredCrackedDungeonTile(tile3.type) || DungeonUtils.IsConsideredPitTrapTile(tile3.type)))
				{
					num11--;
					tile3 = Main.tile[num10, num11];
				}
				array[num10 - (i - num6)] = num11 + 1;
			}
		}
		Bounds.SetBounds(i - num6, num2, i + num6, num + num7);
		Bounds.CalculateHitbox();
		if (flag)
		{
			if (!data.CanGenerateFeatureInArea(this, Bounds))
			{
				return false;
			}
			if (dungeonPitTrapSettings.ConnectedRoom != null)
			{
				DungeonRoom connectedRoom = dungeonPitTrapSettings.ConnectedRoom;
				for (int num12 = i - num6; num12 <= i + num6; num12++)
				{
					for (int num13 = num2; num13 <= num + num7; num13++)
					{
						if ((num12 < i - width || num12 > i + width || num13 < num || num13 > num + num3) && !connectedRoom.OuterBounds.Contains(num12, num13))
						{
							Tile tile4 = Main.tile[num12, num13];
							if (!tile4.active() && DungeonUtils.IsConsideredDungeonWall(tile4.wall))
							{
								return false;
							}
						}
					}
				}
			}
		}
		else
		{
			for (int num14 = i - width; num14 <= i + width; num14++)
			{
				for (int num15 = num; num15 <= num + num3; num15++)
				{
					Tile tile5 = Main.tile[num14, num15];
					if (tile5.active() && (DungeonUtils.IsConsideredDungeonTile(tile5.type) || DungeonUtils.IsConsideredCrackedDungeonTile(tile5.type) || DungeonUtils.IsConsideredPitTrapTile(tile5.type)))
					{
						return false;
					}
				}
			}
		}
		if (generating && !flag)
		{
			for (int num16 = i - width; num16 <= i + width; num16++)
			{
				for (int num17 = num2; num17 <= num + num3; num17++)
				{
					Tile tile6 = Main.tile[num16, num17];
					if (tile6.active() && DungeonUtils.IsConsideredDungeonTile(tile6.type))
					{
						DungeonUtils.ChangeTileType(tile6, pitTrapTileType, resetTile: true, -1);
						DungeonUtils.ChangeWallType(tile6, brickWallType, resetTile: false, wallPaintColor);
					}
				}
			}
		}
		if (generating)
		{
			for (int num18 = i - num6; num18 <= i + num6; num18++)
			{
				int num19 = num2;
				if (flag)
				{
					num19 = GetHeight(array, i, num18 - (i - num6), width, num6, inner: false);
				}
				for (int num20 = num19; num20 <= num + num7; num20++)
				{
					Tile tile7 = Main.tile[num18, num20];
					tile7.liquidType(0);
					tile7.liquid = 0;
					if (DungeonUtils.IsConsideredDungeonWall(tile7.wall))
					{
						continue;
					}
					if (num18 > i - num6 && num18 < i + num6 && num20 < num + num7)
					{
						ushort wall = tile7.wall;
						DungeonUtils.ChangeTileType(tile7, brickTileType, resetTile: true, tilePaintColor);
						if (!DungeonUtils.IsConsideredDungeonWall(wall))
						{
							DungeonUtils.ChangeWallType(tile7, brickWallType, resetTile: false, wallPaintColor);
						}
					}
					else
					{
						DungeonUtils.ChangeTileType(tile7, brickTileType, resetTile: false, tilePaintColor);
					}
				}
			}
		}
		if (generating)
		{
			for (int num21 = i - width; num21 <= i + width; num21++)
			{
				int num22 = num2;
				if (flag)
				{
					num22 = GetHeight(array, i, num21 - (i - width), width, num6, inner: true);
				}
				for (int num23 = num22; num23 <= num + num3; num23++)
				{
					bool flag6 = false;
					if (flag && num23 <= num2 + dungeonPitTrapSettings.TopDensity)
					{
						flag6 = false;
						if (Main.tile[num21, num23].active())
						{
							DungeonUtils.ChangeTileType(Main.tile[num21, num23], pitTrapTileType, resetTile: false, -1);
						}
						Main.tile[num21, num23].liquidType(0);
						Main.tile[num21, num23].liquid = 0;
					}
					else
					{
						flag6 = Main.tile[num21, num23].type != pitTrapTileType;
					}
					if (!flag6)
					{
						continue;
					}
					if (dungeonPitTrapSettings.Flooded)
					{
						Main.tile[num21, num23].liquidType(0);
						Main.tile[num21, num23].liquid = byte.MaxValue;
					}
					else
					{
						Main.tile[num21, num23].liquidType(0);
						Main.tile[num21, num23].liquid = 0;
					}
					bool num24 = num21 == i - width && Main.tile[num21 - 1, num23].active();
					bool flag7 = num21 == i + width && Main.tile[num21 + 1, num23].active();
					bool flag8 = num23 == num + num3 && Main.tile[num21, num23 + 1].active();
					bool flag9 = num21 == i - width + 1 && num23 % 2 == 0 && Main.tile[num21 - 1, num23].active();
					bool flag10 = num21 == i + width - 1 && num23 % 2 == 0 && Main.tile[num21 + 1, num23].active();
					bool flag11 = num23 == num + num3 - 1 && num21 % 2 == 0 && Main.tile[num21, num23 + 1].active();
					if (num24 | flag7 | flag8)
					{
						DungeonUtils.ChangeTileType(Main.tile[num21, num23], 48, resetTile: false, -1);
					}
					else if (flag9 | flag10 | flag11)
					{
						DungeonUtils.ChangeTileType(Main.tile[num21, num23], 48, resetTile: false, -1);
					}
					else if (flag2)
					{
						if (num21 <= i - width + 2 || num21 >= i + width - 2 || num23 >= num + num3 - 2)
						{
							DungeonUtils.ChangeTileType(Main.tile[num21, num23], brickTileType, resetTile: false, tilePaintColor);
							Main.tile[num21, num23].inActive(inActive: true);
						}
						else
						{
							Main.tile[num21, num23].active(active: false);
						}
					}
					else
					{
						Main.tile[num21, num23].active(active: false);
					}
				}
			}
		}
		if (generating && !flag3)
		{
			Point val = Point.Zero;
			for (int num25 = i - num6; num25 <= i + num6; num25++)
			{
				int num26 = num2;
				if (flag)
				{
					num26 = GetHeight(array, i, num25 - (i - num6), width, num6, inner: false);
				}
				for (int num27 = num26 - 1; num27 <= num + num7; num27++)
				{
					Tile tile8 = Main.tile[num25, num27];
					if (!tile8.active() || tile8.type != pitTrapTileType)
					{
						continue;
					}
					bool flag12 = false;
					bool flag13 = false;
					if (flag4)
					{
						flag12 = (flag13 = true);
					}
					else if (flag2)
					{
						Tile tile9 = Main.tile[num25, num27 + 1];
						if (num25 == i - width)
						{
							flag12 = true;
						}
						if (!tile9.active() || tile9.type != pitTrapTileType)
						{
							flag12 = (flag13 = true);
							tile8.type = brickTileType;
							if (tilePaintColor.HasValue)
							{
								tile8.color(tilePaintColor.Value);
							}
						}
					}
					if (flag12)
					{
						tile8.wire(wire: true);
					}
					if (flag13)
					{
						tile8.actuator(actuator: true);
					}
					if (tile8.slope() != 0 || tile8.halfBrick())
					{
						continue;
					}
					Tile tile10 = Main.tile[num25, num27 - 1];
					if (tile10.active())
					{
						continue;
					}
					WorldGen.PlaceTile(num25, num27 - 1, 135, mute: true, forced: false, -1, 7);
					tile10 = Main.tile[num25, num27 - 1];
					if (tile10.active() && tile10.type == 135)
					{
						tile10.wire(wire: true);
						if (val != Point.Zero)
						{
							WorldGen.AddWireFromPointToPoint(num25, num27 - 1, val.X, val.Y);
						}
						val = new Point(num25, num27 - 1);
					}
				}
			}
		}
		Flooded = dungeonPitTrapSettings.Flooded;
		return true;
	}

	public int GetHeight(int[] heights, int baseX, int x, int innerWidth, int outerWidth, bool inner)
	{
		if (inner)
		{
			x += outerWidth - innerWidth;
		}
		return heights[x];
	}
}
