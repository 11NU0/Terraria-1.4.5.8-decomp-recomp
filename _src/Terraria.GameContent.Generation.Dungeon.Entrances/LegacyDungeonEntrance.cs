using System;
using Microsoft.Xna.Framework;
using ReLogic.Utilities;
using Terraria.DataStructures;
using Terraria.Utilities;

namespace Terraria.GameContent.Generation.Dungeon.Entrances;

public class LegacyDungeonEntrance : DungeonEntrance
{
	public LegacyDungeonEntrance(DungeonEntranceSettings settings)
		: base(settings)
	{
	}

	public override void CalculateEntrance(DungeonData data, int x, int y)
	{
		calculated = false;
		LegacyEntrance(data, x, y, generating: false);
		calculated = true;
	}

	public override bool GenerateEntrance(DungeonData data, int x, int y)
	{
		generated = false;
		LegacyEntrance(data, x, y, generating: true);
		generated = true;
		return true;
	}

	public void LegacyEntrance(DungeonData data, int i, int j, bool generating)
	{
		//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_0126: Unknown result type (might be due to invalid IL or missing references)
		//IL_015d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0165: Unknown result type (might be due to invalid IL or missing references)
		//IL_016d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0175: Unknown result type (might be due to invalid IL or missing references)
		//IL_018a: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0223: Unknown result type (might be due to invalid IL or missing references)
		//IL_07a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_07cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_07f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0821: Unknown result type (might be due to invalid IL or missing references)
		//IL_08bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_08d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a55: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a88: Unknown result type (might be due to invalid IL or missing references)
		//IL_0abb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aee: Unknown result type (might be due to invalid IL or missing references)
		//IL_094d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dcd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0df6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0925: Unknown result type (might be due to invalid IL or missing references)
		//IL_092a: Unknown result type (might be due to invalid IL or missing references)
		//IL_09a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_09a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b9d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b7f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1216: Unknown result type (might be due to invalid IL or missing references)
		//IL_123f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1268: Unknown result type (might be due to invalid IL or missing references)
		//IL_1291: Unknown result type (might be due to invalid IL or missing references)
		//IL_110f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1138: Unknown result type (might be due to invalid IL or missing references)
		//IL_1161: Unknown result type (might be due to invalid IL or missing references)
		//IL_118a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1366: Unknown result type (might be due to invalid IL or missing references)
		//IL_1372: Unknown result type (might be due to invalid IL or missing references)
		//IL_1377: Unknown result type (might be due to invalid IL or missing references)
		//IL_1401: Unknown result type (might be due to invalid IL or missing references)
		//IL_1418: Unknown result type (might be due to invalid IL or missing references)
		//IL_1669: Unknown result type (might be due to invalid IL or missing references)
		//IL_1692: Unknown result type (might be due to invalid IL or missing references)
		//IL_16bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_16e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_1731: Unknown result type (might be due to invalid IL or missing references)
		//IL_1733: Unknown result type (might be due to invalid IL or missing references)
		//IL_1512: Unknown result type (might be due to invalid IL or missing references)
		//IL_1529: Unknown result type (might be due to invalid IL or missing references)
		//IL_17fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_17d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_17e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_177c: Unknown result type (might be due to invalid IL or missing references)
		//IL_178d: Unknown result type (might be due to invalid IL or missing references)
		UnifiedRandom unifiedRandom = new UnifiedRandom(((LegacyDungeonEntranceSettings)settings).RandomSeed);
		ushort brickTileType = settings.StyleData.BrickTileType;
		ushort brickWallType = settings.StyleData.BrickWallType;
		byte? tilePaintColor = settings.StyleData.TilePaintColor;
		byte? wallPaintColor = settings.StyleData.WallPaintColor;
		bool dungeonEntranceIsBuried = SpecialSeedFeatures.DungeonEntranceIsBuried;
		bool dungeonEntranceIsUnderground = SpecialSeedFeatures.DungeonEntranceIsUnderground;
		if (generating)
		{
			int num = 60;
			for (int k = i - num; k < i + num; k++)
			{
				for (int l = j - num; l < j + num; l++)
				{
					if (WorldGen.InWorld(k, l))
					{
						Main.tile[k, l].liquid = 0;
						Main.tile[k, l].lava(lava: false);
						Main.tile[k, l].Clear(TileDataType.Slope);
					}
				}
			}
		}
		Vector2D zero = Vector2D.Zero;
		double dungeonEntranceStrengthX = data.dungeonEntranceStrengthX;
		double dungeonEntranceStrengthY = data.dungeonEntranceStrengthY;
		zero.X = i;
		zero.Y = (double)j - dungeonEntranceStrengthY / 2.0;
		data.dungeonBounds.Top = (int)zero.Y;
		int num2 = 1;
		if (i > Main.maxTilesX / 2)
		{
			num2 = -1;
		}
		if (WorldGen.drunkWorldGen || WorldGen.getGoodWorldGen)
		{
			num2 *= -1;
		}
		Bounds.SetBounds((int)zero.X, (int)zero.Y, (int)zero.X, (int)zero.Y);
		int num3 = Math.Max(0, Math.Min(Main.maxTilesX - 1, (int)(zero.X - dungeonEntranceStrengthX * 0.6000000238418579 - (double)unifiedRandom.Next(2, 5))));
		int num4 = Math.Max(0, Math.Min(Main.maxTilesX - 1, (int)(zero.X + dungeonEntranceStrengthX * 0.6000000238418579 + (double)unifiedRandom.Next(2, 5))));
		int num5 = Math.Max(0, Math.Min(Main.maxTilesY - 1, (int)(zero.Y - dungeonEntranceStrengthY * 0.6000000238418579 - (double)unifiedRandom.Next(2, 5))));
		int num6 = Math.Max(0, Math.Min(Main.maxTilesY - 1, (int)(zero.Y + dungeonEntranceStrengthY * 0.6000000238418579 + (double)unifiedRandom.Next(8, 16))));
		Bounds.UpdateBounds(num3, num5, num4, num6);
		if (generating)
		{
			for (int m = num3; m < num4; m++)
			{
				for (int n = num5; n < num6; n++)
				{
					Tile tile = Main.tile[m, n];
					tile.liquid = 0;
					if (tile.wall == brickWallType)
					{
						continue;
					}
					tile.wall = 0;
					if (m > num3 + 1 && m < num4 - 2 && n > num5 + 1 && n < num6 - 2)
					{
						tile.wall = brickWallType;
						if (wallPaintColor.HasValue)
						{
							tile.wallColor(wallPaintColor.Value);
						}
					}
					tile.active(active: true);
					tile.type = brickTileType;
					tile.Clear(TileDataType.Slope);
					if (tilePaintColor.HasValue)
					{
						tile.color(tilePaintColor.Value);
					}
				}
			}
		}
		int num7 = Math.Max(0, Math.Min(Main.maxTilesX - 1, num3));
		int num8 = Math.Max(0, Math.Min(Main.maxTilesX - 1, num3 + 5 + unifiedRandom.Next(4)));
		int num9 = Math.Max(0, Math.Min(Main.maxTilesY - 1, num5 - 3 - unifiedRandom.Next(3)));
		int num10 = Math.Max(0, Math.Min(Main.maxTilesY - 1, num5));
		Bounds.UpdateBounds(num7, num9, num8, num10);
		if (generating)
		{
			for (int num11 = num7; num11 < num8; num11++)
			{
				for (int num12 = num9; num12 < num10; num12++)
				{
					Tile tile2 = Main.tile[num11, num12];
					tile2.liquid = 0;
					if (tile2.wall != brickWallType)
					{
						tile2.active(active: true);
						tile2.type = brickTileType;
						tile2.Clear(TileDataType.Slope);
						if (tilePaintColor.HasValue)
						{
							tile2.color(tilePaintColor.Value);
						}
					}
				}
			}
		}
		num7 = Math.Max(0, Math.Min(Main.maxTilesX - 1, num4 - 5 - unifiedRandom.Next(4)));
		num8 = Math.Max(0, Math.Min(Main.maxTilesX - 1, num4));
		num9 = Math.Max(0, Math.Min(Main.maxTilesY - 1, num5 - 3 - unifiedRandom.Next(3)));
		num10 = Math.Max(0, Math.Min(Main.maxTilesY - 1, num5));
		Bounds.UpdateBounds(num7, num9, num8, num10);
		if (generating)
		{
			for (int num13 = num7; num13 < num8; num13++)
			{
				for (int num14 = num9; num14 < num10; num14++)
				{
					Tile tile3 = Main.tile[num13, num14];
					tile3.liquid = 0;
					if (tile3.wall != brickWallType)
					{
						tile3.active(active: true);
						tile3.type = brickTileType;
						tile3.Clear(TileDataType.Slope);
						if (tilePaintColor.HasValue)
						{
							tile3.color(tilePaintColor.Value);
						}
					}
				}
			}
		}
		int num15 = 2 + unifiedRandom.Next(4);
		int num16 = 1 + unifiedRandom.Next(2);
		int num17 = 0;
		int num18 = Math.Max(0, Math.Min(Main.maxTilesY - 1, num5 - num16));
		data.dungeonBounds.UpdateBounds(num3, num18, num4, num5);
		if (generating)
		{
			for (int num19 = num3; num19 < num4; num19++)
			{
				for (int num20 = num18; num20 < num5; num20++)
				{
					Bounds.UpdateBounds(num19, num20);
					Tile tile4 = Main.tile[num19, num20];
					tile4.liquid = 0;
					if (tile4.wall != brickWallType)
					{
						tile4.active(active: true);
						tile4.type = brickTileType;
						tile4.Clear(TileDataType.Slope);
						if (tilePaintColor.HasValue)
						{
							tile4.color(tilePaintColor.Value);
						}
					}
				}
				num17++;
				if (num17 >= num15)
				{
					num19 += num15;
					num17 = 0;
				}
			}
		}
		if (generating)
		{
			double num21 = Main.worldSurface;
			if (data.Type == DungeonType.DualDungeon)
			{
				num21 = DungeonUtils.GetDualDungeonBrickSupportCutoffY(data);
			}
			for (int num22 = num3; num22 < num4; num22++)
			{
				for (int num23 = num5; (double)num23 < num21; num23++)
				{
					Main.tile[num22, num23].liquid = 0;
					if (DungeonUtils.InAnyPotentialDungeonBounds(num22, num23 - 5))
					{
						continue;
					}
					Tile tile5 = Main.tile[num22, num23];
					bool flag = tile5.active() && !settings.StyleData.TileIsInStyle(tile5.type);
					bool flag2 = !settings.StyleData.WallIsInStyle(tile5.wall);
					bool flag3 = DungeonUtils.IsConsideredDungeonWall(tile5.wall);
					if ((tile5.active() & flag) || !flag3)
					{
						tile5.active(active: true);
						tile5.type = brickTileType;
						if (num22 > num3 && num22 < num4 - 1)
						{
							tile5.wall = brickWallType;
							if (wallPaintColor.HasValue)
							{
								tile5.wallColor(wallPaintColor.Value);
							}
						}
						tile5.Clear(TileDataType.Slope);
						if (tilePaintColor.HasValue)
						{
							tile5.color(tilePaintColor.Value);
						}
					}
					else if (flag2 && num22 > num3 && num22 < num4 - 1)
					{
						tile5.wall = brickWallType;
						if (wallPaintColor.HasValue)
						{
							tile5.wallColor(wallPaintColor.Value);
						}
					}
				}
			}
		}
		num3 = Math.Max(0, Math.Min(Main.maxTilesX - 1, (int)(zero.X - dungeonEntranceStrengthX * 0.5)));
		num4 = Math.Max(0, Math.Min(Main.maxTilesX - 1, (int)(zero.X + dungeonEntranceStrengthX * 0.5)));
		num5 = Math.Max(0, Math.Min(Main.maxTilesY - 1, (int)(zero.Y - dungeonEntranceStrengthY * 0.5)));
		num6 = Math.Max(0, Math.Min(Main.maxTilesY - 1, (int)(zero.Y + dungeonEntranceStrengthY * 0.5)));
		Bounds.UpdateBounds(num3, num5, num4, num6);
		if (generating)
		{
			for (int num24 = num3; num24 < num4; num24++)
			{
				for (int num25 = num5; num25 < num6; num25++)
				{
					Tile tile6 = Main.tile[num24, num25];
					tile6.liquid = 0;
					tile6.active(active: false);
					tile6.wall = brickWallType;
					if (wallPaintColor.HasValue)
					{
						tile6.wallColor(wallPaintColor.Value);
					}
				}
			}
		}
		int num26 = (int)zero.X;
		int num27 = num6;
		for (int num28 = 0; num28 < 20; num28++)
		{
			num26 = (int)zero.X - num28;
			if (num26 <= 0)
			{
				break;
			}
			if (!Main.tile[num26, num27].active() && Main.wallDungeon[Main.tile[num26, num27].wall])
			{
				DungeonPlatformData item = new DungeonPlatformData
				{
					Position = new Point(num26, num27),
					InAHallway = false
				};
				data.dungeonPlatformData.Add(item);
				break;
			}
			num26 = (int)zero.X + num28;
			if (num26 >= Main.maxTilesX)
			{
				break;
			}
			if (!Main.tile[num26, num27].active() && Main.wallDungeon[Main.tile[num26, num27].wall])
			{
				DungeonPlatformData item2 = new DungeonPlatformData
				{
					Position = new Point(num26, num27),
					InAHallway = false
				};
				data.dungeonPlatformData.Add(item2);
				break;
			}
		}
		zero.X += dungeonEntranceStrengthX * 0.6000000238418579 * (double)num2;
		zero.Y += dungeonEntranceStrengthY * 0.5;
		dungeonEntranceStrengthX = data.dungeonEntranceStrengthX2;
		dungeonEntranceStrengthY = data.dungeonEntranceStrengthY2;
		zero.X += dungeonEntranceStrengthX * 0.550000011920929 * (double)num2;
		zero.Y -= dungeonEntranceStrengthY * 0.5;
		num3 = Math.Max(0, Math.Min(Main.maxTilesX - 1, (int)(zero.X - dungeonEntranceStrengthX * 0.6000000238418579 - (double)unifiedRandom.Next(1, 3))));
		num4 = Math.Max(0, Math.Min(Main.maxTilesX - 1, (int)(zero.X + dungeonEntranceStrengthX * 0.6000000238418579 + (double)unifiedRandom.Next(1, 3))));
		num5 = Math.Max(0, Math.Min(Main.maxTilesY - 1, (int)(zero.Y - dungeonEntranceStrengthY * 0.6000000238418579 - (double)unifiedRandom.Next(1, 3))));
		num6 = Math.Max(0, Math.Min(Main.maxTilesY - 1, (int)(zero.Y + dungeonEntranceStrengthY * 0.6000000238418579 + (double)unifiedRandom.Next(6, 16))));
		Bounds.UpdateBounds(num3, num5, num4, num6);
		if (generating)
		{
			for (int num29 = num3; num29 < num4; num29++)
			{
				for (int num30 = num5; num30 < num6; num30++)
				{
					Tile tile7 = Main.tile[num29, num30];
					if (tile7.active() && tile7.type == brickTileType)
					{
						continue;
					}
					tile7.liquid = 0;
					bool flag4 = true;
					if (num2 < 0)
					{
						if ((double)num29 < zero.X - dungeonEntranceStrengthX * 0.5)
						{
							flag4 = false;
						}
					}
					else if ((double)num29 > zero.X + dungeonEntranceStrengthX * 0.5 - 1.0)
					{
						flag4 = false;
					}
					if (flag4)
					{
						tile7.wall = 0;
						tile7.active(active: true);
						tile7.type = brickTileType;
						tile7.Clear(TileDataType.Slope);
						if (tilePaintColor.HasValue)
						{
							tile7.color(tilePaintColor.Value);
						}
					}
				}
			}
		}
		Bounds.UpdateBounds(num3, num5, num4, (int)Main.worldSurface);
		if (generating)
		{
			double num31 = Main.worldSurface;
			if (data.Type == DungeonType.DualDungeon)
			{
				num31 = DungeonCrawler.CurrentDungeonData.genVars.outerPotentialDungeonBounds.Top - 5;
			}
			for (int num32 = num3; num32 < num4; num32++)
			{
				for (int num33 = num6; (double)num33 < num31; num33++)
				{
					Main.tile[num32, num33].liquid = 0;
					if (DungeonUtils.InAnyPotentialDungeonBounds(num32, num33 - 5))
					{
						continue;
					}
					Tile tile8 = Main.tile[num32, num33];
					bool flag5 = tile8.active() && !settings.StyleData.TileIsInStyle(tile8.type);
					bool flag6 = !settings.StyleData.WallIsInStyle(tile8.wall);
					bool flag7 = DungeonUtils.IsConsideredDungeonWall(tile8.wall);
					if ((tile8.active() & flag5) || !flag7)
					{
						tile8.active(active: true);
						tile8.type = brickTileType;
						if (num32 > num3 && num32 < num4 - 1)
						{
							tile8.wall = brickWallType;
							if (wallPaintColor.HasValue)
							{
								tile8.wallColor(wallPaintColor.Value);
							}
						}
						tile8.Clear(TileDataType.Slope);
						if (tilePaintColor.HasValue)
						{
							tile8.color(tilePaintColor.Value);
						}
					}
					else if (flag6 && num32 > num3 && num32 < num4 - 1)
					{
						tile8.wall = brickWallType;
						if (wallPaintColor.HasValue)
						{
							tile8.wallColor(wallPaintColor.Value);
						}
					}
				}
			}
		}
		num3 = Math.Max(0, Math.Min(Main.maxTilesX - 1, (int)(zero.X - dungeonEntranceStrengthX * 0.5)));
		num4 = Math.Max(0, Math.Min(Main.maxTilesX - 1, (int)(zero.X + dungeonEntranceStrengthX * 0.5)));
		num7 = num3;
		if (num2 < 0)
		{
			Math.Max(0, Math.Min(Main.maxTilesX - 1, num7++));
		}
		num8 = Math.Max(0, Math.Min(Main.maxTilesX - 1, num7 + 5 + unifiedRandom.Next(4)));
		num9 = Math.Max(0, Math.Min(Main.maxTilesY - 1, num5 - 3 - unifiedRandom.Next(3)));
		num10 = Math.Max(0, Math.Min(Main.maxTilesY - 1, num5));
		Bounds.UpdateBounds(num7, num9, num8, num10);
		if (generating)
		{
			for (int num34 = num7; num34 < num8; num34++)
			{
				for (int num35 = num9; num35 < num10; num35++)
				{
					Tile tile9 = Main.tile[num34, num35];
					tile9.liquid = 0;
					if (tile9.wall != brickWallType)
					{
						tile9.active(active: true);
						tile9.type = brickTileType;
						tile9.Clear(TileDataType.Slope);
						if (tilePaintColor.HasValue)
						{
							tile9.color(tilePaintColor.Value);
						}
					}
				}
			}
		}
		num7 = Math.Max(0, Math.Min(Main.maxTilesX - 1, num4 - 5 - unifiedRandom.Next(4)));
		num8 = Math.Max(0, Math.Min(Main.maxTilesX - 1, num4));
		num9 = Math.Max(0, Math.Min(Main.maxTilesY - 1, num5 - 3 - unifiedRandom.Next(3)));
		num10 = Math.Max(0, Math.Min(Main.maxTilesY - 1, num5));
		Bounds.UpdateBounds(num7, num9, num8, num10);
		if (generating)
		{
			for (int num36 = num7; num36 < num8; num36++)
			{
				for (int num37 = num9; num37 < num10; num37++)
				{
					Tile tile10 = Main.tile[num36, num37];
					tile10.liquid = 0;
					if (tile10.wall != brickWallType)
					{
						tile10.active(active: true);
						tile10.type = brickTileType;
						tile10.Clear(TileDataType.Slope);
						if (tilePaintColor.HasValue)
						{
							tile10.color(tilePaintColor.Value);
						}
					}
				}
			}
		}
		if (num2 < 0)
		{
			num4++;
		}
		num16 = 1 + unifiedRandom.Next(2);
		num15 = 2 + unifiedRandom.Next(4);
		num17 = 0;
		num18 = Math.Max(0, Math.Min(Main.maxTilesY - 1, num5 - num16));
		if (generating)
		{
			for (int num38 = num3 + 1; num38 < num4 - 1; num38++)
			{
				for (int num39 = num18; num39 < num5; num39++)
				{
					Tile tile11 = Main.tile[num38, num39];
					tile11.liquid = 0;
					if (tile11.wall != brickWallType)
					{
						tile11.active(active: true);
						tile11.type = brickTileType;
						tile11.Clear(TileDataType.Slope);
						if (tilePaintColor.HasValue)
						{
							tile11.color(tilePaintColor.Value);
						}
					}
				}
				num17++;
				if (num17 >= num15)
				{
					num38 += num15;
					num17 = 0;
				}
			}
		}
		if (!dungeonEntranceIsUnderground && !dungeonEntranceIsBuried)
		{
			num3 = Math.Max(0, Math.Min(Main.maxTilesX - 1, (int)(zero.X - dungeonEntranceStrengthX * 0.6)));
			num4 = Math.Max(0, Math.Min(Main.maxTilesX - 1, (int)(zero.X + dungeonEntranceStrengthX * 0.6)));
			num5 = Math.Max(0, Math.Min(Main.maxTilesX - 1, (int)(zero.Y - dungeonEntranceStrengthY * 0.6)));
			num6 = Math.Max(0, Math.Min(Main.maxTilesX - 1, (int)(zero.Y + dungeonEntranceStrengthY * 0.6)));
			Bounds.UpdateBounds(num3, num5, num4, num6);
			if (generating)
			{
				for (int num40 = num3; num40 < num4; num40++)
				{
					for (int num41 = num5; num41 < num6; num41++)
					{
						Main.tile[num40, num41].liquid = 0;
						Main.tile[num40, num41].wall = 0;
					}
				}
			}
		}
		num3 = Math.Max(0, Math.Min(Main.maxTilesX - 1, (int)(zero.X - dungeonEntranceStrengthX * 0.5)));
		num4 = Math.Max(0, Math.Min(Main.maxTilesX - 1, (int)(zero.X + dungeonEntranceStrengthX * 0.5)));
		num5 = Math.Max(0, Math.Min(Main.maxTilesY - 1, (int)(zero.Y - dungeonEntranceStrengthY * 0.5)));
		num6 = Math.Max(0, Math.Min(Main.maxTilesY - 1, (int)(zero.Y + dungeonEntranceStrengthY * 0.5)));
		if ((dungeonEntranceIsUnderground | dungeonEntranceIsBuried) && num2 == -1)
		{
			num3 = Math.Max(0, Math.Min(Main.maxTilesX - 1, num3 + 1));
			num4 = Math.Max(0, Math.Min(Main.maxTilesX - 1, num4 + 1));
		}
		Bounds.UpdateBounds(num3, num5, num4, num6);
		if (generating)
		{
			for (int num42 = num3; num42 < num4; num42++)
			{
				for (int num43 = num5; num43 < num6; num43++)
				{
					Main.tile[num42, num43].liquid = 0;
					Main.tile[num42, num43].active(active: false);
					Main.tile[num42, num43].wall = 0;
				}
			}
		}
		OldManSpawn = DungeonUtils.SetOldManSpawnAndSpawnOldManIfDefaultDungeon((int)zero.X, num6, generating);
		if (generating && SpecialSeedFeatures.DungeonEntranceHasATree)
		{
			DungeonUtils.GenerateDungeonTree(data, data.genVars.generatingDungeonPositionX, (int)Main.worldSurface, data.genVars.generatingDungeonPositionY);
		}
		if (generating && SpecialSeedFeatures.DungeonEntranceHasStairs)
		{
			int i2 = ((num2 == 1) ? num4 : num3);
			int depth = DungeonUtils.GetDualDungeonBrickSupportCutoffY(data) - num6 + 5;
			DungeonUtils.GenerateDungeonStairs(data, i2, num6, num2, settings.StyleData, depth);
		}
		num16 = 1 + unifiedRandom.Next(2);
		num15 = 2 + unifiedRandom.Next(4);
		num17 = 0;
		num3 = (int)(zero.X - dungeonEntranceStrengthX * 0.5);
		num4 = (int)(zero.X + dungeonEntranceStrengthX * 0.5);
		if (dungeonEntranceIsUnderground | dungeonEntranceIsBuried)
		{
			if (num2 == -1)
			{
				num3++;
				num4++;
			}
		}
		else
		{
			num3 += 2;
			num4 -= 2;
		}
		num3 = Math.Max(0, Math.Min(Main.maxTilesX - 1, num3));
		num4 = Math.Max(0, Math.Min(Main.maxTilesX - 1, num4));
		if (generating)
		{
			for (int num44 = num3; num44 < num4; num44++)
			{
				for (int num45 = num5; num45 < num6 + 1; num45++)
				{
					WorldGen.PlaceWall(num44, num45, brickWallType, mute: true);
					if (wallPaintColor.HasValue)
					{
						Main.tile[num44, num45].wallColor(wallPaintColor.Value);
					}
				}
				if (!dungeonEntranceIsUnderground && !dungeonEntranceIsBuried)
				{
					num17++;
					if (num17 >= num15)
					{
						num44 += num15 * 2;
						num17 = 0;
					}
				}
			}
		}
		if (WorldGen.drunkWorldGen && !WorldGen.SecretSeed.noSurface.Enabled)
		{
			num3 = (int)(zero.X - dungeonEntranceStrengthX * 0.5);
			num4 = (int)(zero.X + dungeonEntranceStrengthX * 0.5);
			if (num2 == 1)
			{
				num3 = num4 - 3;
			}
			else
			{
				num4 = num3 + 3;
			}
			num3 = Math.Max(0, Math.Min(Main.maxTilesX - 1, num3));
			num4 = Math.Max(0, Math.Min(Main.maxTilesX - 1, num4));
			Bounds.UpdateBounds(num3, num5, num4, num6);
			if (generating)
			{
				for (int num46 = num3; num46 < num4; num46++)
				{
					for (int num47 = num5; num47 < num6 + 1; num47++)
					{
						Tile tile12 = Main.tile[num46, num47];
						tile12.active(active: true);
						tile12.type = brickTileType;
						tile12.Clear(TileDataType.Slope);
						if (tilePaintColor.HasValue)
						{
							tile12.color(tilePaintColor.Value);
						}
					}
				}
			}
		}
		zero.X -= dungeonEntranceStrengthX * 0.6000000238418579 * (double)num2;
		zero.Y += dungeonEntranceStrengthY * 0.5;
		dungeonEntranceStrengthX = 15.0;
		dungeonEntranceStrengthY = 3.0;
		zero.Y -= dungeonEntranceStrengthY * 0.5;
		num3 = Math.Max(0, Math.Min(Main.maxTilesX - 1, (int)(zero.X - dungeonEntranceStrengthX * 0.5)));
		num4 = Math.Max(0, Math.Min(Main.maxTilesX - 1, (int)(zero.X + dungeonEntranceStrengthX * 0.5)));
		num5 = Math.Max(0, Math.Min(Main.maxTilesY - 1, (int)(zero.Y - dungeonEntranceStrengthY * 0.5)));
		num6 = Math.Max(0, Math.Min(Main.maxTilesY - 1, (int)(zero.Y + dungeonEntranceStrengthY * 0.5)));
		Bounds.UpdateBounds(num3, num5, num4, num6);
		if (num2 < 0)
		{
			zero.X--;
		}
		Vector2D val = zero;
		val.Y++;
		if (generating)
		{
			for (int num48 = num3; num48 < num4; num48++)
			{
				for (int num49 = num5; num49 < num6; num49++)
				{
					Tile tile13 = Main.tile[num48, num49];
					tile13.active(active: false);
					if ((num2 > 0 && (double)num48 < val.X) || (num2 < 0 && (double)num48 > val.X) || (dungeonEntranceIsUnderground | dungeonEntranceIsBuried))
					{
						tile13.wall = brickWallType;
						if (wallPaintColor.HasValue)
						{
							tile13.wallColor(wallPaintColor.Value);
						}
					}
				}
			}
		}
		if (generating)
		{
			WorldGen.PlaceTile((int)val.X, (int)val.Y, 10, mute: true, forced: false, -1, 13);
		}
		Bounds.CalculateHitbox();
	}
}
