using System;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria.GameContent.Generation.Dungeon.Halls;
using Terraria.GameContent.Generation.Dungeon.Rooms;
using Terraria.ID;
using Terraria.Utilities;

namespace Terraria.GameContent.Generation.Dungeon.Features;

public class DungeonGlobalGroundFurniture : GlobalDungeonFeature
{
	public DungeonGlobalGroundFurniture(DungeonFeatureSettings settings)
		: base(settings)
	{
		DungeonCrawler.CurrentDungeonData.dungeonFeatures.Add(this);
	}

	public override bool GenerateFeature(DungeonData data)
	{
		generated = false;
		if (data.Type == DungeonType.DualDungeon)
		{
			GroundFurniture_DualDungeons(data);
		}
		else
		{
			GroundFurniture(data);
		}
		generated = true;
		return true;
	}

	public void GroundFurniture_DualDungeons(DungeonData data)
	{
		//IL_0139: Unknown result type (might be due to invalid IL or missing references)
		//IL_013e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0145: Unknown result type (might be due to invalid IL or missing references)
		//IL_014c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0179: Unknown result type (might be due to invalid IL or missing references)
		//IL_017c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0181: Unknown result type (might be due to invalid IL or missing references)
		//IL_0194: Unknown result type (might be due to invalid IL or missing references)
		//IL_019b: Unknown result type (might be due to invalid IL or missing references)
		//IL_032a: Unknown result type (might be due to invalid IL or missing references)
		//IL_032f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0336: Unknown result type (might be due to invalid IL or missing references)
		//IL_033d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0218: Unknown result type (might be due to invalid IL or missing references)
		//IL_021f: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0248: Unknown result type (might be due to invalid IL or missing references)
		//IL_024f: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_036a: Unknown result type (might be due to invalid IL or missing references)
		//IL_036e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0373: Unknown result type (might be due to invalid IL or missing references)
		//IL_0386: Unknown result type (might be due to invalid IL or missing references)
		//IL_038d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0402: Unknown result type (might be due to invalid IL or missing references)
		//IL_0409: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_042d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0434: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e0: Unknown result type (might be due to invalid IL or missing references)
		UnifiedRandom genRand = WorldGen.genRand;
		float num = (float)Main.maxTilesX / 4200f;
		int alchTableCount = (int)((double)(1f + (float)(int)num) * data.globalFeatureScalar);
		int bewitchTableCount = (int)((double)(1f + (float)(int)num) * data.globalFeatureScalar);
		bool flag = false;
		for (int i = 0; i < data.genVars.dungeonGenerationStyles.Count; i++)
		{
			if (data.genVars.dungeonGenerationStyles[i].Style == 0)
			{
				flag = true;
				break;
			}
		}
		if (!flag)
		{
			alchTableCount = 0;
			bewitchTableCount = 0;
		}
		int minimumWaterCandles = -1;
		if (data.Type == DungeonType.DualDungeon)
		{
			minimumWaterCandles = WorldGen.GetWorldSize() switch
			{
				1 => 10, 
				2 => 15, 
				_ => 5, 
			};
		}
		int defaultCount = 4;
		int defaultCount2 = 6;
		int num2 = 0;
		for (int j = 0; j < data.dungeonRooms.Count; j++)
		{
			DungeonRoom dungeonRoom = data.dungeonRooms[j];
			if (!dungeonRoom.generated)
			{
				continue;
			}
			DungeonGenerationStyleData styleData = dungeonRoom.settings.StyleData;
			DungeonBounds innerBounds = dungeonRoom.InnerBounds;
			int num3 = dungeonRoom.GetFurnitureCount(defaultCount);
			bool flag2 = styleData.Style == 0 && (alchTableCount > 0 || bewitchTableCount > 0);
			int num4 = 50;
			while (num3 > 0)
			{
				num4--;
				if (num4 <= 0)
				{
					break;
				}
				Point val = innerBounds.RandomPointInBounds(genRand);
				Tile tile = Main.tile[val.X, val.Y];
				if (!DungeonUtils.IsConsideredDungeonWall(tile.wall) || tile.active())
				{
					continue;
				}
				val = DungeonUtils.FirstSolid(ceiling: false, val, null);
				val.Y--;
				tile = Main.tile[val.X, val.Y];
				int num5 = alchTableCount;
				int num6 = bewitchTableCount;
				bool flag3 = false;
				int alchTableCount2 = 0;
				if (flag2)
				{
					flag3 = GroundFurniture_ActuallyGenerateFurniture(data, genRand, val.X, val.Y, tile.wall, ref alchTableCount, ref bewitchTableCount, ref minimumWaterCandles);
					if (!flag3)
					{
						flag3 = GroundFurniture_ActuallyGenerateFurniture(data, genRand, val.X, val.Y, tile.wall, ref alchTableCount2, ref alchTableCount2, ref minimumWaterCandles, stricterSpecialCheck: false, num4 > 25);
					}
				}
				else
				{
					flag3 = GroundFurniture_ActuallyGenerateFurniture(data, genRand, val.X, val.Y, tile.wall, ref alchTableCount2, ref alchTableCount2, ref minimumWaterCandles, stricterSpecialCheck: false, num4 > 25);
					if (!flag3)
					{
						flag3 = GroundFurniture_ActuallyGenerateFurniture(data, genRand, val.X, val.Y, tile.wall, ref alchTableCount2, ref alchTableCount2, ref minimumWaterCandles, stricterSpecialCheck: false, num4 > 25);
					}
				}
				if ((flag2 && num5 != alchTableCount) || num6 != bewitchTableCount)
				{
					flag2 = false;
				}
				if (flag3)
				{
					num3--;
					num2++;
				}
			}
		}
		for (int k = 0; k < data.dungeonHalls.Count; k++)
		{
			DungeonHall dungeonHall = data.dungeonHalls[k];
			if (!dungeonHall.generated)
			{
				continue;
			}
			DungeonGenerationStyleData styleData2 = dungeonHall.settings.StyleData;
			DungeonBounds bounds = dungeonHall.Bounds;
			int num7 = dungeonHall.GetFurnitureCount(defaultCount2);
			bool flag4 = styleData2.Style == 0 && (alchTableCount > 0 || bewitchTableCount > 0);
			int num8 = 50;
			while (num7 > 0)
			{
				num8--;
				if (num8 <= 0)
				{
					break;
				}
				Point val2 = bounds.RandomPointInBounds(genRand);
				Tile tile2 = Main.tile[val2.X, val2.Y];
				if (!DungeonUtils.IsConsideredDungeonWall(tile2.wall) || tile2.active())
				{
					continue;
				}
				val2 = DungeonUtils.FirstSolid(ceiling: false, val2, bounds);
				val2.Y--;
				tile2 = Main.tile[val2.X, val2.Y];
				int num9 = alchTableCount;
				int num10 = bewitchTableCount;
				bool flag5 = false;
				int alchTableCount3 = 0;
				if (flag4)
				{
					flag5 = GroundFurniture_ActuallyGenerateFurniture(data, genRand, val2.X, val2.Y, tile2.wall, ref alchTableCount, ref bewitchTableCount, ref minimumWaterCandles);
					if (!flag5)
					{
						flag5 = GroundFurniture_ActuallyGenerateFurniture(data, genRand, val2.X, val2.Y, tile2.wall, ref alchTableCount3, ref alchTableCount3, ref minimumWaterCandles, stricterSpecialCheck: true, noRegularFurnitureAreaChecks: true);
					}
				}
				else
				{
					flag5 = GroundFurniture_ActuallyGenerateFurniture(data, genRand, val2.X, val2.Y, tile2.wall, ref alchTableCount3, ref alchTableCount3, ref minimumWaterCandles);
					if (!flag5)
					{
						flag5 = GroundFurniture_ActuallyGenerateFurniture(data, genRand, val2.X, val2.Y, tile2.wall, ref alchTableCount3, ref alchTableCount3, ref minimumWaterCandles, stricterSpecialCheck: true, noRegularFurnitureAreaChecks: true);
					}
				}
				if ((flag4 && num9 != alchTableCount) || num10 != bewitchTableCount)
				{
					flag4 = false;
				}
				if (flag5)
				{
					num7--;
					num2++;
				}
			}
		}
	}

	public void GroundFurniture(DungeonData data)
	{
		UnifiedRandom genRand = WorldGen.genRand;
		ushort wallType = (ushort)data.wallVariants[0];
		float num = (float)Main.maxTilesX / 4200f;
		int num2 = (int)((double)(2000f * num) * data.globalFeatureScalar);
		int alchTableCount = (int)((double)(1f + (float)(int)num) * data.globalFeatureScalar);
		int bewitchTableCount = (int)((double)(1f + (float)(int)num) * data.globalFeatureScalar);
		bool flag = false;
		for (int i = 0; i < data.genVars.dungeonGenerationStyles.Count; i++)
		{
			if (data.genVars.dungeonGenerationStyles[i].Style == 0)
			{
				flag = true;
				break;
			}
		}
		if (!flag)
		{
			alchTableCount = 0;
			bewitchTableCount = 0;
		}
		int minimumWaterCandles = -1;
		if (data.Type == DungeonType.DualDungeon)
		{
			minimumWaterCandles = WorldGen.GetWorldSize() switch
			{
				1 => 10, 
				2 => 15, 
				_ => 5, 
			};
		}
		int num3 = 2000;
		for (int j = 0; j < num2; j++)
		{
			if (alchTableCount > 0 || bewitchTableCount > 0)
			{
				j--;
				num3--;
				if (num3 <= 0)
				{
					break;
				}
			}
			int num4 = genRand.Next(data.dungeonBounds.Left, data.dungeonBounds.Right);
			int k = genRand.Next(Math.Max(data.dungeonBounds.Top, (int)Main.worldSurface + 10), data.dungeonBounds.Bottom);
			int num5 = 1000;
			while (!DungeonUtils.IsConsideredDungeonWall(Main.tile[num4, k].wall) || Main.tile[num4, k].active())
			{
				num5--;
				if (num5 <= 0)
				{
					break;
				}
				num4 = genRand.Next(data.dungeonBounds.Left, data.dungeonBounds.Right);
				k = genRand.Next(Math.Max(data.dungeonBounds.Top, (int)Main.worldSurface + 10), data.dungeonBounds.Bottom);
			}
			if (DungeonUtils.IsConsideredDungeonWall(Main.tile[num4, k].wall) && !Main.tile[num4, k].active())
			{
				for (; !WorldGen.SolidTile(num4, k) && k < Main.UnderworldLayer; k++)
				{
				}
				k--;
				GroundFurniture_ActuallyGenerateFurniture(data, genRand, num4, k, wallType, ref alchTableCount, ref bewitchTableCount, ref minimumWaterCandles, j < num2 / 2);
			}
		}
	}

	private bool GroundFurniture_ActuallyGenerateFurniture(DungeonData data, UnifiedRandom genRand, int i, int j, ushort wallType, ref int alchTableCount, ref int bewitchTableCount, ref int minimumWaterCandles, bool stricterSpecialCheck = true, bool noRegularFurnitureAreaChecks = false)
	{
		int num = i;
		int k = i;
		while (!Main.tile[num, j].active() && WorldGen.SolidTile(num, j + 1))
		{
			num--;
		}
		num++;
		for (; !Main.tile[k, j].active() && WorldGen.SolidTile(k, j + 1); k++)
		{
		}
		k--;
		int num2 = k - num;
		int num3 = (k + num) / 2;
		if (!data.CanGenerateFeatureAt(this, num3, j))
		{
			return false;
		}
		if (!Main.tile[num3, j].active() && DungeonUtils.IsConsideredDungeonWall(Main.tile[num3, j].wall) && WorldGen.SolidTile(num3, j + 1) && Main.tile[num3, j + 1].type != 48)
		{
			int num4 = 1396;
			int num5 = 1397;
			int num6 = 1398;
			int num7 = 1405;
			int num8 = 1408;
			int num9 = 1414;
			int num10 = 1470;
			int num11 = 2376;
			int num12 = 2386;
			int num13 = 2402;
			int num14 = 2658;
			int num15 = 2664;
			int num16 = 2645;
			int num17 = 3900;
			switch (wallType)
			{
			case 8:
				num4 = 1399;
				num5 = 1400;
				num6 = 1401;
				num7 = 1406;
				num8 = 1409;
				num9 = 1415;
				num10 = 1471;
				num11 = 2377;
				num12 = 2387;
				num13 = 2403;
				num14 = 2659;
				num15 = 2665;
				num16 = 2646;
				num17 = 3901;
				break;
			case 9:
				num4 = 1402;
				num5 = 1403;
				num6 = 1404;
				num7 = 1407;
				num8 = 1410;
				num9 = 1416;
				num10 = 1472;
				num11 = 2378;
				num12 = 2388;
				num13 = 2404;
				num14 = 2660;
				num15 = 2666;
				num16 = 2647;
				num17 = 3902;
				break;
			}
			if (Main.tile[num3, j].wall >= 94 && Main.tile[num3, j].wall <= 105)
			{
				num4 = 1509;
				num5 = 1510;
				num6 = 1511;
				num7 = 5743;
				num8 = -1;
				num9 = 1512;
				num10 = 5740;
				num11 = 5750;
				num12 = 5741;
				num13 = 5753;
				num14 = 5739;
				num15 = 5742;
				num16 = 5748;
				num17 = 5746;
			}
			bool flag = true;
			bool flag2 = true;
			DungeonGenerationStyleData styleForWall = DungeonGenerationStyles.GetStyleForWall(data.genVars.dungeonGenerationStyles, Main.tile[num3, j].wall);
			if (styleForWall != null)
			{
				flag = styleForWall.Style == 0;
				flag2 = flag;
				num5 = GroundFurniture_GetFurnitureItem(styleForWall, genRand, num5, styleForWall.TableItemTypes);
				num6 = GroundFurniture_GetFurnitureItem(styleForWall, genRand, num6, styleForWall.WorkbenchItemTypes);
				num7 = GroundFurniture_GetFurnitureItem(styleForWall, genRand, num7, styleForWall.CandleItemTypes);
				num8 = GroundFurniture_GetFurnitureItem(styleForWall, genRand, num8, styleForWall.VaseOrStatueItemTypes);
				num9 = GroundFurniture_GetFurnitureItem(styleForWall, genRand, num9, styleForWall.BookcaseItemTypes);
				num4 = GroundFurniture_GetFurnitureItem(styleForWall, genRand, num4, styleForWall.ChairItemTypes);
				num10 = GroundFurniture_GetFurnitureItem(styleForWall, genRand, num10, styleForWall.BedItemTypes);
				num11 = GroundFurniture_GetFurnitureItem(styleForWall, genRand, num11, styleForWall.PianoItemTypes);
				num12 = GroundFurniture_GetFurnitureItem(styleForWall, genRand, num12, styleForWall.DresserItemTypes);
				num13 = GroundFurniture_GetFurnitureItem(styleForWall, genRand, num13, styleForWall.SofaItemTypes);
				num14 = GroundFurniture_GetFurnitureItem(styleForWall, genRand, num14, styleForWall.BathtubItemTypes);
				num16 = GroundFurniture_GetFurnitureItem(styleForWall, genRand, num16, styleForWall.LampItemTypes);
				num15 = GroundFurniture_GetFurnitureItem(styleForWall, genRand, num15, styleForWall.CandelabraItemTypes);
				num17 = GroundFurniture_GetFurnitureItem(styleForWall, genRand, num17, styleForWall.ClockItemTypes);
			}
			int num18 = genRand.Next(13);
			if ((num18 == 10 || num18 == 11 || num18 == 12) && genRand.Next(4) != 0)
			{
				num18 = genRand.Next(13);
			}
			while ((num18 == 2 && num8 == -1) || (num18 == 5 && num10 == -1) || (num18 == 6 && num11 == -1) || (num18 == 7 && num12 == -1) || (num18 == 8 && num13 == -1) || (num18 == 9 && num14 == -1) || (num18 == 10 && num15 == -1) || (num18 == 11 && num16 == -1) || (num18 == 12 && num17 == -1))
			{
				num18 = genRand.Next(13);
			}
			int num19 = 0;
			int num20 = 0;
			if (num18 == 0)
			{
				num19 = 5;
				num20 = 4;
			}
			if (num18 == 1)
			{
				num19 = 4;
				num20 = 3;
			}
			if (num18 == 2)
			{
				num19 = 3;
				num20 = 5;
			}
			if (num18 == 3)
			{
				num19 = 4;
				num20 = 6;
			}
			if (num18 == 4)
			{
				num19 = 3;
				num20 = 3;
			}
			if (num18 == 5)
			{
				num19 = 5;
				num20 = 3;
			}
			if (num18 == 6)
			{
				num19 = 5;
				num20 = 4;
			}
			if (num18 == 7)
			{
				num19 = 5;
				num20 = 4;
			}
			if (num18 == 8)
			{
				num19 = 5;
				num20 = 4;
			}
			if (num18 == 9)
			{
				num19 = 5;
				num20 = 3;
			}
			if (num18 == 10)
			{
				num19 = 2;
				num20 = 4;
			}
			if (num18 == 11)
			{
				num19 = 3;
				num20 = 3;
			}
			if (num18 == 12)
			{
				num19 = 2;
				num20 = 5;
			}
			if (noRegularFurnitureAreaChecks)
			{
				if (num18 == 0)
				{
					num19 = 3;
					num20 = 4;
				}
				if (num18 == 1)
				{
					num19 = 2;
					num20 = 3;
				}
				if (num18 == 2)
				{
					num19 = 3;
					num20 = 5;
				}
				if (num18 == 3)
				{
					num19 = 3;
					num20 = 6;
				}
				if (num18 == 4)
				{
					num19 = 1;
					num20 = 3;
				}
				if (num18 == 5)
				{
					num19 = 4;
					num20 = 3;
				}
				if (num18 == 6)
				{
					num19 = 4;
					num20 = 4;
				}
				if (num18 == 7)
				{
					num19 = 4;
					num20 = 4;
				}
				if (num18 == 8)
				{
					num19 = 4;
					num20 = 4;
				}
				if (num18 == 9)
				{
					num19 = 4;
					num20 = 3;
				}
				if (num18 == 10)
				{
					num19 = 1;
					num20 = 4;
				}
				if (num18 == 11)
				{
					num19 = 2;
					num20 = 3;
				}
				if (num18 == 12)
				{
					num19 = 2;
					num20 = 5;
				}
			}
			bool flag3 = false;
			bool flag4 = false;
			int num21 = 0;
			if (alchTableCount > 0 || bewitchTableCount > 0)
			{
				num21 = 15;
			}
			for (int l = num3 - num19 - num21; l <= num3 + num19 + num21; l++)
			{
				for (int m = j - num20 - num21; m <= j + num21; m++)
				{
					if (!WorldGen.InWorld(l, m))
					{
						continue;
					}
					Tile tile = Main.tile[l, m];
					if (l >= num3 - num19 && l <= num3 + num19 && m >= j - num20 && m <= j)
					{
						if (!data.CanGenerateFeatureAt(this, l, m))
						{
							flag3 = true;
							break;
						}
						if (!noRegularFurnitureAreaChecks && tile.active())
						{
							num18 = -1;
							break;
						}
					}
					if (stricterSpecialCheck && (alchTableCount > 0 || bewitchTableCount > 0) && tile.active() && (tile.type == 355 || tile.type == 354))
					{
						flag4 = true;
					}
				}
			}
			if (flag3)
			{
				return false;
			}
			float num22 = (float)num19 * 1.75f;
			if (noRegularFurnitureAreaChecks)
			{
				num22 = num19;
			}
			if ((float)num2 < num22)
			{
				num18 = -1;
			}
			if ((!flag4 & flag2) && (alchTableCount > 0 || bewitchTableCount > 0))
			{
				if (alchTableCount > 0)
				{
					WorldGen.PlaceTile(num3, j, 355, mute: true);
					if (Main.tile[num3, j].active() && Main.tile[num3, j].type == 355)
					{
						alchTableCount--;
						return true;
					}
				}
				else if (bewitchTableCount > 0)
				{
					WorldGen.PlaceTile(num3, j, 354, mute: true);
					if (Main.tile[num3, j].active() && Main.tile[num3, j].type == 354)
					{
						bewitchTableCount--;
						return true;
					}
				}
			}
			else if (num5 > -1 && num18 == 0)
			{
				PlacementDetails placementDetails = ItemID.Sets.DerivedPlacementDetails[num5];
				WorldGen.PlaceTile(num3, j, placementDetails.tileType, mute: true, forced: false, -1, placementDetails.tileStyle);
				if (Main.tile[num3, j].active() && Main.tile[num3, j].type == placementDetails.tileType)
				{
					if (num4 > -1)
					{
						PlacementDetails placementDetails2 = ItemID.Sets.DerivedPlacementDetails[num4];
						if (!Main.tile[num3 - 2, j].active())
						{
							WorldGen.PlaceTile(num3 - 2, j, placementDetails2.tileType, mute: true, forced: false, -1, placementDetails2.tileStyle);
							if (Main.tile[num3 - 2, j].active())
							{
								Main.tile[num3 - 2, j].frameX += 18;
								Main.tile[num3 - 2, j - 1].frameX += 18;
							}
						}
						if (!Main.tile[num3 + 2, j].active())
						{
							WorldGen.PlaceTile(num3 + 2, j, placementDetails2.tileType, mute: true, forced: false, -1, placementDetails2.tileStyle);
						}
					}
					for (int n = num3 - 1; n <= num3 + 1; n++)
					{
						if (genRand.Next(2) != 0 || Main.tile[n, j - 2].active())
						{
							continue;
						}
						if (flag)
						{
							int num23 = genRand.Next(5);
							if (minimumWaterCandles > 0)
							{
								num23 = 2;
							}
							if (num7 > -1 && num23 <= 1 && !Main.tileLighted[Main.tile[n - 1, j - 2].type])
							{
								PlacementDetails placementDetails3 = ItemID.Sets.DerivedPlacementDetails[num7];
								WorldGen.PlaceTile(n, j - 2, placementDetails3.tileType, mute: true, forced: false, -1, placementDetails3.tileStyle);
							}
							if (num23 == 2 && !Main.tileLighted[Main.tile[n - 1, j - 2].type])
							{
								WorldGen.PlaceTile(n, j - 2, 49, mute: true);
								if (Main.tile[n, j - 2].active() && Main.tile[n, j - 2].type == 49)
								{
									minimumWaterCandles--;
								}
								continue;
							}
							switch (num23)
							{
							case 3:
								WorldGen.PlaceTile(n, j - 2, 50, mute: true);
								break;
							case 4:
								WorldGen.PlaceTile(n, j - 2, 103, mute: true);
								break;
							}
						}
						else
						{
							int num24 = genRand.Next(3);
							if (num7 > -1 && num24 <= 1 && !Main.tileLighted[Main.tile[n - 1, j - 2].type])
							{
								PlacementDetails placementDetails4 = ItemID.Sets.DerivedPlacementDetails[num7];
								WorldGen.PlaceTile(n, j - 2, placementDetails4.tileType, mute: true, forced: false, -1, placementDetails4.tileStyle);
							}
							else if (num24 == 2)
							{
								WorldGen.PlaceTile(n, j - 2, 103, mute: true);
							}
						}
					}
					return true;
				}
			}
			else if (num6 > -1 && num18 == 1)
			{
				PlacementDetails placementDetails5 = ItemID.Sets.DerivedPlacementDetails[num6];
				PlacementDetails placementDetails6 = ItemID.Sets.DerivedPlacementDetails[num4];
				WorldGen.PlaceTile(num3, j, placementDetails5.tileType, mute: true, forced: false, -1, placementDetails5.tileStyle);
				if (Main.tile[num3, j].active() && Main.tile[num3, j].type == placementDetails5.tileType)
				{
					if (num4 > -1)
					{
						if (genRand.Next(2) == 0)
						{
							if (!Main.tile[num3 - 1, j].active())
							{
								WorldGen.PlaceTile(num3 - 1, j, placementDetails6.tileType, mute: true, forced: false, -1, placementDetails6.tileStyle);
								if (Main.tile[num3 - 1, j].active())
								{
									Main.tile[num3 - 1, j].frameX += 18;
									Main.tile[num3 - 1, j - 1].frameX += 18;
								}
							}
						}
						else if (!Main.tile[num3 + 2, j].active())
						{
							WorldGen.PlaceTile(num3 + 2, j, placementDetails6.tileType, mute: true, forced: false, -1, placementDetails6.tileStyle);
						}
					}
					for (int num25 = num3; num25 <= num3 + 1; num25++)
					{
						if (genRand.Next(2) != 0 || Main.tile[num25, j - 1].active())
						{
							continue;
						}
						if (flag)
						{
							int num26 = genRand.Next(5);
							if (minimumWaterCandles > 0)
							{
								num26 = 2;
							}
							if (num7 != -1 && num26 <= 1 && !Main.tileLighted[Main.tile[num25 - 1, j - 1].type])
							{
								PlacementDetails placementDetails7 = ItemID.Sets.DerivedPlacementDetails[num7];
								WorldGen.PlaceTile(num25, j - 1, placementDetails7.tileType, mute: true, forced: false, -1, placementDetails7.tileStyle);
								continue;
							}
							if (num26 == 2 && !Main.tileLighted[Main.tile[num25 - 1, j - 1].type])
							{
								WorldGen.PlaceTile(num25, j - 1, 49, mute: true);
								if (Main.tile[num25, j - 1].active() && Main.tile[num25, j - 1].type == 49)
								{
									minimumWaterCandles--;
								}
								continue;
							}
							switch (num26)
							{
							case 3:
								WorldGen.PlaceTile(num25, j - 1, 50, mute: true);
								break;
							case 4:
								WorldGen.PlaceTile(num25, j - 1, 103, mute: true);
								break;
							}
						}
						else
						{
							int num27 = genRand.Next(3);
							if (num7 != -1 && num27 <= 1 && !Main.tileLighted[Main.tile[num25 - 1, j - 1].type])
							{
								PlacementDetails placementDetails8 = ItemID.Sets.DerivedPlacementDetails[num7];
								WorldGen.PlaceTile(num25, j - 1, placementDetails8.tileType, mute: true, forced: false, -1, placementDetails8.tileStyle);
							}
							else if (num27 == 2)
							{
								WorldGen.PlaceTile(num25, j - 1, 103, mute: true);
							}
						}
					}
					return true;
				}
			}
			else if (num8 > -1 && num18 == 2)
			{
				PlacementDetails placementDetails9 = ItemID.Sets.DerivedPlacementDetails[num8];
				WorldGen.PlaceTile(num3, j, placementDetails9.tileType, mute: true, forced: false, -1, placementDetails9.tileStyle);
				if (Main.tile[num3, j].active() && Main.tile[num3, j].type == placementDetails9.tileType)
				{
					return true;
				}
			}
			else if (num9 > -1 && num18 == 3)
			{
				PlacementDetails placementDetails10 = ItemID.Sets.DerivedPlacementDetails[num9];
				WorldGen.PlaceTile(num3, j, placementDetails10.tileType, mute: true, forced: false, -1, placementDetails10.tileStyle);
			}
			else if (num4 > -1 && num18 == 4)
			{
				PlacementDetails placementDetails11 = ItemID.Sets.DerivedPlacementDetails[num4];
				if (genRand.Next(2) == 0)
				{
					WorldGen.PlaceTile(num3, j, placementDetails11.tileType, mute: true, forced: false, -1, placementDetails11.tileStyle);
					Main.tile[num3, j].frameX += 18;
					Main.tile[num3, j - 1].frameX += 18;
				}
				else
				{
					WorldGen.PlaceTile(num3, j, placementDetails11.tileType, mute: true, forced: false, -1, placementDetails11.tileStyle);
				}
				if (Main.tile[num3, j].active() && Main.tile[num3, j].type == placementDetails11.tileType)
				{
					return true;
				}
			}
			else if (num10 > -1 && num18 == 5)
			{
				PlacementDetails placementDetails12 = ItemID.Sets.DerivedPlacementDetails[num10];
				if (placementDetails12.tileType >= 0)
				{
					if (genRand.Next(2) == 0)
					{
						WorldGen.Place4x2(num3, j, (ushort)placementDetails12.tileType, 1, placementDetails12.tileStyle);
					}
					else
					{
						WorldGen.Place4x2(num3, j, (ushort)placementDetails12.tileType, -1, placementDetails12.tileStyle);
					}
					if (Main.tile[num3, j].active() && Main.tile[num3, j].type == placementDetails12.tileType)
					{
						return true;
					}
				}
			}
			else if (num11 > -1 && num18 == 6)
			{
				PlacementDetails placementDetails13 = ItemID.Sets.DerivedPlacementDetails[num11];
				WorldGen.PlaceTile(num3, j, placementDetails13.tileType, mute: true, forced: false, -1, placementDetails13.tileStyle);
				if (Main.tile[num3, j].active() && Main.tile[num3, j].type == placementDetails13.tileType)
				{
					return true;
				}
			}
			else if (num12 > -1 && num18 == 7)
			{
				PlacementDetails placementDetails14 = ItemID.Sets.DerivedPlacementDetails[num12];
				WorldGen.PlaceTile(num3, j, placementDetails14.tileType, mute: true, forced: false, -1, placementDetails14.tileStyle);
				if (Main.tile[num3, j].active() && Main.tile[num3, j].type == placementDetails14.tileType)
				{
					return true;
				}
			}
			else if (num13 > -1 && num18 == 8)
			{
				PlacementDetails placementDetails15 = ItemID.Sets.DerivedPlacementDetails[num13];
				WorldGen.PlaceTile(num3, j, placementDetails15.tileType, mute: true, forced: false, -1, placementDetails15.tileStyle);
				if (Main.tile[num3, j].active() && Main.tile[num3, j].type == placementDetails15.tileType)
				{
					return true;
				}
			}
			else if (num14 > -1 && num18 == 9)
			{
				PlacementDetails placementDetails16 = ItemID.Sets.DerivedPlacementDetails[num14];
				if (placementDetails16.tileType >= 0)
				{
					if (genRand.Next(2) == 0)
					{
						WorldGen.Place4x2(num3, j, (ushort)placementDetails16.tileType, 1, placementDetails16.tileStyle);
					}
					else
					{
						WorldGen.Place4x2(num3, j, (ushort)placementDetails16.tileType, -1, placementDetails16.tileStyle);
					}
					if (Main.tile[num3, j].active() && Main.tile[num3, j].type == placementDetails16.tileType)
					{
						return true;
					}
				}
			}
			else if (num16 > -1 && num18 == 10)
			{
				PlacementDetails placementDetails17 = ItemID.Sets.DerivedPlacementDetails[num16];
				WorldGen.PlaceTile(num3, j, placementDetails17.tileType, mute: true, forced: false, -1, placementDetails17.tileStyle);
				if (Main.tile[num3, j].active() && Main.tile[num3, j].type == placementDetails17.tileType)
				{
					return true;
				}
			}
			else if (num15 > -1 && num18 == 11)
			{
				PlacementDetails placementDetails18 = ItemID.Sets.DerivedPlacementDetails[num15];
				WorldGen.PlaceTile(num3, j, placementDetails18.tileType, mute: true, forced: false, -1, placementDetails18.tileStyle);
				if (Main.tile[num3, j].active() && Main.tile[num3, j].type == placementDetails18.tileType)
				{
					return true;
				}
			}
			else if (num17 > -1 && num18 == 12)
			{
				PlacementDetails placementDetails19 = ItemID.Sets.DerivedPlacementDetails[num17];
				WorldGen.PlaceTile(num3, j, placementDetails19.tileType, mute: true, forced: false, -1, placementDetails19.tileStyle);
				if (Main.tile[num3, j].active() && Main.tile[num3, j].type == placementDetails19.tileType)
				{
					return true;
				}
			}
		}
		return false;
	}

	private int GroundFurniture_GetFurnitureItem(DungeonGenerationStyleData styleData, UnifiedRandom genRand, int defaultItem, int[] items)
	{
		if (items == null)
		{
			return -1;
		}
		if (items.Length == 0 || styleData.Style == 0)
		{
			return defaultItem;
		}
		return items[genRand.Next(items.Length)];
	}
}
