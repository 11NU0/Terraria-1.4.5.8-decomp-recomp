using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria.GameContent.Generation.Dungeon.Features;
using Terraria.Utilities;
using Terraria.WorldBuilding;

namespace Terraria.GameContent.Generation.Dungeon.Entrances;

public class DomeDungeonEntrance : DungeonEntrance
{
	public DomeDungeonEntrance(DungeonEntranceSettings settings)
		: base(settings)
	{
	}

	public override void CalculateEntrance(DungeonData data, int x, int y)
	{
		calculated = false;
		DomeEntrance(data, x, y, generating: false);
		calculated = true;
	}

	public override bool GenerateEntrance(DungeonData data, int x, int y)
	{
		generated = false;
		DomeEntrance(data, x, y, generating: true);
		generated = true;
		return true;
	}

	public override bool CanGenerateFeatureAt(DungeonData data, IDungeonFeature feature, int x, int y)
	{
		if (feature is DungeonGlobalBookshelves || feature is DungeonGlobalPaintings || feature is DungeonGlobalSpikes)
		{
			return false;
		}
		return base.CanGenerateFeatureAt(data, feature, x, y);
	}

	public void DomeEntrance(DungeonData data, int i, int j, bool generating)
	{
		//IL_06aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0724: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0893: Unknown result type (might be due to invalid IL or missing references)
		//IL_07a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0826: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c4e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c53: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ccc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cd1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d4a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d4f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dc0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dc5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fb3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fb8: Unknown result type (might be due to invalid IL or missing references)
		//IL_101d: Unknown result type (might be due to invalid IL or missing references)
		//IL_052e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0533: Unknown result type (might be due to invalid IL or missing references)
		//IL_0586: Unknown result type (might be due to invalid IL or missing references)
		//IL_058b: Unknown result type (might be due to invalid IL or missing references)
		UnifiedRandom unifiedRandom = new UnifiedRandom(((DomeDungeonEntranceSettings)settings).RandomSeed);
		ushort brickTileType = settings.StyleData.BrickTileType;
		ushort brickWallType = settings.StyleData.BrickWallType;
		byte? tilePaintColor = settings.StyleData.TilePaintColor;
		byte? wallPaintColor = settings.StyleData.WallPaintColor;
		bool dungeonEntranceIsBuried = SpecialSeedFeatures.DungeonEntranceIsBuried;
		bool dungeonEntranceIsUnderground = SpecialSeedFeatures.DungeonEntranceIsUnderground;
		bool flag = data.genVars.dungeonSide == DungeonSide.Left;
		if (Main.drunkWorld)
		{
			flag = !flag;
		}
		bool flag2 = unifiedRandom.Next(4) != 0;
		WindowType windowType = WindowType.RegularWindows;
		windowType = unifiedRandom.Next(3) switch
		{
			1 => WindowType.SkeletronMosaic, 
			2 => WindowType.MoonLordMosaic, 
			_ => WindowType.RegularWindows, 
		};
		Bounds.SetBounds(i, j, i, j);
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
		int num2 = 5;
		int num3 = 35;
		int num4 = num3 + num2;
		int num5 = 100;
		int num6 = 30;
		int num7 = j - num6;
		int m = 10;
		int num8 = 50;
		if (data.Type == DungeonType.DualDungeon)
		{
			num5 = DungeonUtils.GetDualDungeonBrickSupportCutoffY(data) - num7;
		}
		else if (dungeonEntranceIsUnderground)
		{
			num5 = num8 - m + 5;
		}
		if (generating && !dungeonEntranceIsBuried && !dungeonEntranceIsUnderground)
		{
			int num9 = i - num4 + 1;
			if (flag)
			{
				num9 = i + num4 - 1;
			}
			int num10 = 20;
			WorldUtils.Gen(new Point(num9, num7 - num10), new Shapes.Circle(num10, num10), Actions.Chain(new Actions.Clear()));
		}
		Bounds.UpdateBounds(i - num4, num7 - num4, i + num4 + 1, num7 + 10);
		if (generating)
		{
			int num11 = -5;
			int num12 = num5;
			for (int n = -num4; n <= num4; n++)
			{
				for (int num13 = num11; num13 < num12; num13++)
				{
					int num14 = i + n;
					int num15 = num7 + num13;
					if (!WorldGen.InWorld(num14, num15))
					{
						continue;
					}
					Tile tile = Main.tile[num14, num15];
					bool flag3 = tile.active() && !settings.StyleData.TileIsInStyle(tile.type);
					bool flag4 = !settings.StyleData.WallIsInStyle(tile.wall);
					bool flag5 = DungeonUtils.IsConsideredDungeonWall(tile.wall);
					if (num13 < 0)
					{
						tile.ClearEverything();
					}
					else if (num13 >= 0 && num13 < 5)
					{
						if ((n >= -num3 + num2 && n <= -num3 + num2 * 2) || (n >= num3 - num2 * 2 && n <= num3 - num2))
						{
							tile.ClearEverything();
							if (!flag5)
							{
								tile.wall = brickWallType;
								if (wallPaintColor.HasValue)
								{
									tile.wallColor(wallPaintColor.Value);
								}
							}
						}
						else if (!flag5)
						{
							tile.liquid = 0;
							tile.active(active: true);
							tile.type = brickTileType;
							if (tilePaintColor.HasValue)
							{
								tile.color(tilePaintColor.Value);
							}
							if (n != -num4 && n != num4)
							{
								tile.wall = brickWallType;
								if (wallPaintColor.HasValue)
								{
									tile.wallColor(wallPaintColor.Value);
								}
							}
						}
					}
					else if (num13 >= 5 && num13 < 10)
					{
						if (n >= -num3 + num2 && n <= num3 - num2)
						{
							tile.ClearEverything();
							tile.wall = brickWallType;
							if (wallPaintColor.HasValue)
							{
								tile.wallColor(wallPaintColor.Value);
							}
						}
						else if (!flag5)
						{
							tile.liquid = 0;
							tile.active(active: true);
							tile.type = brickTileType;
							if (tilePaintColor.HasValue)
							{
								tile.color(tilePaintColor.Value);
							}
							if (n != -num4 && n != num4)
							{
								tile.wall = brickWallType;
								if (wallPaintColor.HasValue)
								{
									tile.wallColor(wallPaintColor.Value);
								}
							}
						}
					}
					else if ((tile.active() & flag3) || !flag5)
					{
						tile.liquid = 0;
						tile.active(active: true);
						tile.type = brickTileType;
						if (tilePaintColor.HasValue)
						{
							tile.color(tilePaintColor.Value);
						}
						if (n != -num4 && n != num4)
						{
							tile.wall = brickWallType;
							if (wallPaintColor.HasValue)
							{
								tile.wallColor(wallPaintColor.Value);
							}
						}
					}
					else if (flag4)
					{
						tile.liquid = 0;
						if (n != -num4 && n != num4)
						{
							tile.wall = brickWallType;
							if (wallPaintColor.HasValue)
							{
								tile.wallColor(wallPaintColor.Value);
							}
						}
					}
					if (num13 == 1 && (n == -num3 + num2 || n == num3 - num2 * 2))
					{
						DungeonPlatformData item = new DungeonPlatformData
						{
							Position = new Point(num14, num15),
							OverrideHeightFluff = 0,
							ForcePlacement = true,
							PlacePotsChance = 0.33000001311302185
						};
						data.dungeonPlatformData.Add(item);
					}
					if (num13 == 10 && n == 0)
					{
						DungeonPlatformData item2 = new DungeonPlatformData
						{
							Position = new Point(num14, num15),
							OverrideHeightFluff = 0,
							ForcePlacement = true,
							PlacePotsChance = 0.33000001311302185
						};
						data.dungeonPlatformData.Add(item2);
					}
				}
			}
			int num16 = -1;
			int num17 = 6;
			for (; m < num8; m++)
			{
				Tile tile2 = Main.tile[i, num7 + m];
				if (num16 == -1 && !tile2.active())
				{
					num16 = 15;
				}
				if (num16 > 0)
				{
					num16--;
					if (num16 <= 0)
					{
						break;
					}
					if (num16 <= 5)
					{
						num17--;
					}
				}
				for (int num18 = -num17; num18 <= num17; num18++)
				{
					Tile tile3 = Main.tile[i + num18, num7 + m];
					tile3.ClearEverything();
					if (!DungeonUtils.IsConsideredDungeonWall(tile3.wall))
					{
						tile3.wall = brickWallType;
						if (wallPaintColor.HasValue)
						{
							tile3.wallColor(wallPaintColor.Value);
						}
					}
				}
			}
		}
		int num19 = num7 + 1;
		if (generating)
		{
			WorldUtils.Gen(new Point(i, num7), new Shapes.Slime(num4, 1.0, 1.0), Actions.Chain(new Modifiers.IsAboveHeight(num19), new Modifiers.SkipWalls(brickWallType), new Actions.UpdateBounds(data.dungeonBounds), new Actions.Clear(), new Actions.SetTile(brickTileType, setSelfFrames: false, setNeighborFrames: false, clearTile: false), new Actions.SetTilePaint(tilePaintColor)));
			WorldUtils.Gen(new Point(i, num7), new Shapes.Slime(num4 - 2, 1.0, 1.0), Actions.Chain(new Modifiers.IsAboveHeight(num19 + 1), new Actions.SetWall(brickWallType, setSelfFrames: false, setNeighborFrames: false, clearTile: false), new Actions.SetWallPaint(wallPaintColor)));
		}
		if (generating)
		{
			ushort num20 = 0;
			int num21 = 2;
			if (WorldGen.SecretSeed.surfaceIsDesert.Enabled)
			{
				num20 = 53;
				num21 = -1;
			}
			WorldUtils.Gen(new Point(i, num7 - num2 + 1), new Shapes.Slime(num4, 0.8999999761581421, 1.100000023841858), Actions.Chain(new Modifiers.IsAboveHeight(num19 - 2), new Modifiers.SkipTiles(brickTileType), new Modifiers.SkipWalls(brickWallType), new Actions.Clear(), new Actions.SetTile(num20, setSelfFrames: false, setNeighborFrames: false, clearTile: false)));
			if (!dungeonEntranceIsUnderground && num21 > -1)
			{
				WorldUtils.Gen(new Point(i, num7 - num2 + 1), new Shapes.Slime(num4, 0.8999999761581421, 1.100000023841858), Actions.Chain(new Modifiers.IsAboveHeight(num19 - 2), new Modifiers.OnlyTiles(num20), new Modifiers.IsTouchingAir(useDiagonals: true), new Actions.SetTile((ushort)num21, setSelfFrames: false, setNeighborFrames: false, clearTile: false)));
			}
		}
		if (generating)
		{
			WorldUtils.Gen(new Point(i, num7), new Shapes.Slime(num3, 1.0, 1.0), Actions.Chain(new Modifiers.IsAboveHeight(num19), new Actions.ClearTile()));
		}
		if (generating)
		{
			DomeEntrance_Door(data, i, num7, num4, num3, flag, dungeonEntranceIsBuried);
			if (dungeonEntranceIsBuried | dungeonEntranceIsUnderground)
			{
				DomeEntrance_Door(data, i, num7, num4, num3, !flag, dungeonEntranceIsBuried);
			}
		}
		if (generating)
		{
			DungeonWindowBasicSettings dungeonWindowBasicSettings = new DungeonWindowBasicSettings
			{
				Style = settings.StyleData,
				Width = 5,
				Height = 24,
				Closed = dungeonEntranceIsUnderground
			};
			DungeonWindowMosaicSettings dungeonWindowMosaicSettings = new DungeonWindowMosaicSettings
			{
				Style = settings.StyleData,
				Closed = dungeonEntranceIsUnderground,
				MosaicType = windowType
			};
			switch (windowType)
			{
			case WindowType.RegularWindows:
				new DungeonWindowBasic(dungeonWindowBasicSettings).GenerateFeature(data, i - 8, num7 - 16);
				new DungeonWindowBasic(dungeonWindowBasicSettings).GenerateFeature(data, i + 8, num7 - 16);
				dungeonWindowBasicSettings.Height = 28;
				new DungeonWindowBasic(dungeonWindowBasicSettings).GenerateFeature(data, i, num7 - 17);
				dungeonWindowBasicSettings.Height = 10;
				new DungeonWindowBasic(dungeonWindowBasicSettings).GenerateFeature(data, i - num3 + 6, num7 - 8);
				new DungeonWindowBasic(dungeonWindowBasicSettings).GenerateFeature(data, i + num3 - 6, num7 - 8);
				dungeonWindowBasicSettings.Height = 11;
				new DungeonWindowBasic(dungeonWindowBasicSettings).GenerateFeature(data, i - num3 + 15, num7 - 11);
				new DungeonWindowBasic(dungeonWindowBasicSettings).GenerateFeature(data, i + num3 - 15, num7 - 11);
				break;
			case WindowType.SkeletronMosaic:
				if (!dungeonEntranceIsUnderground)
				{
					dungeonWindowMosaicSettings.OverrideGlassType = 89;
				}
				dungeonWindowMosaicSettings.OverrideGlassPaint = 26;
				new DungeonWindowMosaic(dungeonWindowMosaicSettings).GenerateFeature(data, i, num7 - 19);
				dungeonWindowBasicSettings.OverrideGlassPaint = 26;
				dungeonWindowBasicSettings.Height = 10;
				new DungeonWindowBasic(dungeonWindowBasicSettings).GenerateFeature(data, i - num3 + 6, num7 - 8);
				new DungeonWindowBasic(dungeonWindowBasicSettings).GenerateFeature(data, i + num3 - 6, num7 - 8);
				dungeonWindowBasicSettings.Height = 11;
				new DungeonWindowBasic(dungeonWindowBasicSettings).GenerateFeature(data, i - num3 + 15, num7 - 11);
				new DungeonWindowBasic(dungeonWindowBasicSettings).GenerateFeature(data, i + num3 - 15, num7 - 11);
				break;
			case WindowType.MoonLordMosaic:
				if (!dungeonEntranceIsUnderground)
				{
					dungeonWindowMosaicSettings.OverrideGlassType = 91;
				}
				new DungeonWindowMosaic(dungeonWindowMosaicSettings).GenerateFeature(data, i, num7 - 17);
				dungeonWindowBasicSettings.Height = 10;
				if (!dungeonEntranceIsUnderground)
				{
					dungeonWindowBasicSettings.OverrideGlassType = 241;
				}
				new DungeonWindowBasic(dungeonWindowBasicSettings).GenerateFeature(data, i - num3 + 6, num7 - 8);
				dungeonWindowBasicSettings.OverrideGlassType = 91;
				new DungeonWindowBasic(dungeonWindowBasicSettings).GenerateFeature(data, i + num3 - 6, num7 - 8);
				dungeonWindowBasicSettings.Height = 11;
				if (!dungeonEntranceIsUnderground)
				{
					dungeonWindowBasicSettings.OverrideGlassType = 90;
				}
				new DungeonWindowBasic(dungeonWindowBasicSettings).GenerateFeature(data, i - num3 + 15, num7 - 11);
				if (!dungeonEntranceIsUnderground)
				{
					dungeonWindowBasicSettings.OverrideGlassType = 88;
				}
				new DungeonWindowBasic(dungeonWindowBasicSettings).GenerateFeature(data, i + num3 - 15, num7 - 11);
				break;
			}
		}
		DungeonPillarSettings dungeonPillarSettings = new DungeonPillarSettings
		{
			Style = settings.StyleData,
			PillarType = PillarType.BlockActuated,
			Width = 3,
			Height = 0,
			CrowningOnTop = true,
			CrowningOnBottom = true,
			CrowningStopsAtPillar = false,
			AlwaysPlaceEntirePillar = true
		};
		if (generating)
		{
			new DungeonPillar(dungeonPillarSettings).GenerateFeature(data, i - num3 + 21, num7);
			new DungeonPillar(dungeonPillarSettings).GenerateFeature(data, i + num3 - 21, num7);
			DungeonPlatformData item3 = new DungeonPlatformData
			{
				Position = new Point(i - num3 + 15, num7 - 25),
				OverrideHeightFluff = 0,
				ForcePlacement = true,
				PlacePotsChance = 0.33000001311302185,
				PlaceBooksChance = 0.75,
				PlacePotionBottlesChance = 0.10000000149011612,
				NoWaterbolt = true
			};
			data.dungeonPlatformData.Add(item3);
			item3 = new DungeonPlatformData
			{
				Position = new Point(i + num3 - 15, num7 - 25),
				OverrideHeightFluff = 0,
				ForcePlacement = true,
				PlacePotsChance = 0.33000001311302185,
				PlaceBooksChance = 0.75,
				PlacePotionBottlesChance = 0.10000000149011612,
				NoWaterbolt = true
			};
			data.dungeonPlatformData.Add(item3);
			item3 = new DungeonPlatformData
			{
				Position = new Point(i - num3 + 15, num7 - 20),
				OverrideHeightFluff = 0,
				ForcePlacement = true,
				PlacePotsChance = 0.33000001311302185,
				PlaceBooksChance = 0.75,
				PlacePotionBottlesChance = 0.10000000149011612
			};
			data.dungeonPlatformData.Add(item3);
			item3 = new DungeonPlatformData
			{
				Position = new Point(i + num3 - 15, num7 - 20),
				OverrideHeightFluff = 0,
				ForcePlacement = true,
				PlacePotsChance = 0.33000001311302185,
				PlaceBooksChance = 0.75,
				PlacePotionBottlesChance = 0.10000000149011612
			};
			data.dungeonPlatformData.Add(item3);
		}
		if (generating)
		{
			int num22 = 16;
			dungeonPillarSettings.PillarType = PillarType.Block;
			dungeonPillarSettings.CrowningOnTop = false;
			dungeonPillarSettings.CrowningOnBottom = false;
			dungeonPillarSettings.Width = 5;
			dungeonPillarSettings.Height = num22;
			new DungeonPillar(dungeonPillarSettings).GenerateFeature(data, i - num4 + 2, num7 - 10);
			new DungeonPillar(dungeonPillarSettings).GenerateFeature(data, i + num4 - 2, num7 - 10);
			dungeonPillarSettings.Width = 4;
			dungeonPillarSettings.Height = num22 - 2;
			new DungeonPillar(dungeonPillarSettings).GenerateFeature(data, i - num3 + 8, num7 - 28);
			new DungeonPillar(dungeonPillarSettings).GenerateFeature(data, i + num3 - 8, num7 - 28);
			dungeonPillarSettings.Width = 3;
			dungeonPillarSettings.Height = num22 - 3;
			new DungeonPillar(dungeonPillarSettings).GenerateFeature(data, i - num3 + 21, num7 - 37);
			new DungeonPillar(dungeonPillarSettings).GenerateFeature(data, i + num3 - 21, num7 - 37);
			if (flag2)
			{
				DomeEntrance_TreeOnPillar(unifiedRandom, i - num4 + 2, num7 - 10 - num22 + 1);
				DomeEntrance_TreeOnPillar(unifiedRandom, i - num3 + 8, num7 - 28 - num22 + 2 + 1);
				DomeEntrance_TreeOnPillar(unifiedRandom, i - num3 + 21, num7 - 37 - num22 + 3 + 1);
				DomeEntrance_TreeOnPillar(unifiedRandom, i + num4 - 2, num7 - 10 - num22 + 1);
				DomeEntrance_TreeOnPillar(unifiedRandom, i + num3 - 8, num7 - 28 - num22 + 2 + 1);
				DomeEntrance_TreeOnPillar(unifiedRandom, i + num3 - 21, num7 - 37 - num22 + 3 + 1);
			}
		}
		OldManSpawn = DungeonUtils.SetOldManSpawnAndSpawnOldManIfDefaultDungeon(i, num7, generating);
		if (generating && SpecialSeedFeatures.DungeonEntranceHasATree)
		{
			DungeonUtils.GenerateDungeonTree(data, i, (int)Main.worldSurface, num7 - num3 + 5, generateRoots: false);
		}
		if (generating && SpecialSeedFeatures.DungeonEntranceHasStairs)
		{
			int i2 = (flag ? (i + num4) : (i - num4));
			DungeonUtils.GenerateDungeonStairs(data, i2, num7, flag ? 1 : (-1), settings.StyleData, num5);
		}
		Bounds.CalculateHitbox();
	}

	public void DomeEntrance_Door(DungeonData data, int i, int entranceFloor, int outerSize, int innerSize, bool leftDungeonDoor, bool buried)
	{
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_0100: Unknown result type (might be due to invalid IL or missing references)
		//IL_0106: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		int num = (leftDungeonDoor ? (innerSize - 1) : (-outerSize - 2));
		int num2 = (leftDungeonDoor ? (outerSize + 2) : (-innerSize + 1));
		if (buried)
		{
			num += 2 * ((!leftDungeonDoor) ? 1 : 0);
			num2 += 2 * (leftDungeonDoor ? (-1) : 0);
		}
		Point val = new Point(i + (leftDungeonDoor ? (outerSize - 1) : (-outerSize + 1)), entranceFloor);
		Point val2 = new Point(i + (leftDungeonDoor ? (innerSize + 1) : (-innerSize - 1)), entranceFloor);
		for (int j = num; j <= num2; j++)
		{
			for (int k = -3; k <= 1; k++)
			{
				int num3 = j + i;
				int num4 = k + entranceFloor;
				Tile tile = Main.tile[num3, num4];
				if (!buried && ((leftDungeonDoor && num3 >= val.X) || (!leftDungeonDoor && num3 <= val.X)))
				{
					tile.wall = 0;
				}
				if (k >= -2 && k <= 0)
				{
					tile.ClearTile();
				}
			}
		}
		WorldGen.PlaceTile(val.X, val.Y, 10, mute: true, forced: true, -1, 13);
		WorldGen.PlaceTile(val2.X, val2.Y, 10, mute: true, forced: true, -1, 13);
	}

	public void DomeEntrance_TreeOnPillar(UnifiedRandom genRand, int pillarX, int pillarY)
	{
		if (!WorldGen.InWorld(pillarX, pillarY, 5) || Main.tile[pillarX, pillarY - 1].active())
		{
			return;
		}
		ushort num = 0;
		int num2 = 2;
		if (WorldGen.SecretSeed.surfaceIsDesert.Enabled)
		{
			num = 53;
			num2 = -1;
		}
		int num3 = 5;
		int num4 = num3 / 2;
		for (int i = 0; i < num3; i++)
		{
			int num5 = pillarX + i - num4;
			for (int j = 0; j <= 3; j++)
			{
				int num6 = pillarY + j;
				Tile tile = Main.tile[num5, num6];
				if (tile.wall != settings.StyleData.BrickWallType)
				{
					tile.wall = 0;
				}
				if ((j != 1 || genRand.Next(2) != 0) && (j != 2 || genRand.Next(3) == 0) && (j != 3 || genRand.Next(4) == 0))
				{
					if (num2 > -1 && WorldGen.TileIsExposedToAir(num5, num6))
					{
						tile.type = (ushort)num2;
					}
					else
					{
						tile.type = num;
					}
				}
			}
		}
		if (num == 53)
		{
			WorldGen.TryGrowingTreeByType(323, pillarX, pillarY, 0, ignoreWalls: true);
		}
		else
		{
			WorldGen.TryGrowingTreeByType(5, pillarX, pillarY, 0, ignoreWalls: true);
		}
	}
}
