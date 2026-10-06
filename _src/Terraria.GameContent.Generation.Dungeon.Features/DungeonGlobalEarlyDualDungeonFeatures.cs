using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Terraria.GameContent.Generation.Dungeon.Rooms;
using Terraria.Utilities;

namespace Terraria.GameContent.Generation.Dungeon.Features;

public class DungeonGlobalEarlyDualDungeonFeatures : GlobalDungeonFeature
{
	public DungeonGlobalEarlyDualDungeonFeatures(DungeonFeatureSettings settings)
		: base(settings)
	{
		DungeonCrawler.CurrentDungeonData.dungeonFeatures.Add(this);
	}

	public override bool GenerateFeature(DungeonData data)
	{
		generated = false;
		EarlyDungeonFeatures(data);
		generated = true;
		return true;
	}

	public void EarlyDungeonFeatures(DungeonData data)
	{
		//IL_034b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0350: Unknown result type (might be due to invalid IL or missing references)
		//IL_0357: Unknown result type (might be due to invalid IL or missing references)
		//IL_035e: Unknown result type (might be due to invalid IL or missing references)
		//IL_036b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0372: Unknown result type (might be due to invalid IL or missing references)
		//IL_0384: Unknown result type (might be due to invalid IL or missing references)
		//IL_038b: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_04af: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0403: Unknown result type (might be due to invalid IL or missing references)
		//IL_0408: Unknown result type (might be due to invalid IL or missing references)
		//IL_040f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0416: Unknown result type (might be due to invalid IL or missing references)
		//IL_0423: Unknown result type (might be due to invalid IL or missing references)
		//IL_042a: Unknown result type (might be due to invalid IL or missing references)
		//IL_043c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0443: Unknown result type (might be due to invalid IL or missing references)
		//IL_12f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_1315: Unknown result type (might be due to invalid IL or missing references)
		//IL_0581: Unknown result type (might be due to invalid IL or missing references)
		UnifiedRandom genRand = WorldGen.genRand;
		int num = 20;
		int num2 = 8;
		int num3 = 8;
		int num4 = 6;
		int num5 = 4;
		int num6 = 4;
		int num7 = 40;
		int num8 = 40;
		switch (WorldGen.GetWorldSize())
		{
		case 0:
			num = 20;
			num2 = 8;
			num3 = 8;
			num4 = 6;
			num5 = 4;
			num6 = 4;
			num7 = 40;
			num8 = 40;
			break;
		case 1:
			num = 30;
			num2 = 14;
			num3 = 12;
			num4 = 10;
			num5 = 6;
			num6 = 8;
			num7 = 60;
			num8 = 60;
			break;
		case 2:
			num = 40;
			num2 = 18;
			num3 = 16;
			num4 = 14;
			num5 = 8;
			num6 = 12;
			num7 = 80;
			num8 = 80;
			break;
		}
		if (WorldGen.SecretSeed.Variations.actuallyNoTrapsForRealIMeanIt)
		{
			num3 = 0;
			num4 = 0;
			num5 = 0;
		}
		for (int i = 0; i < data.genVars.dungeonGenerationStyles.Count; i++)
		{
			DungeonGenerationStyleData dungeonGenerationStyleData = data.genVars.dungeonGenerationStyles[i];
			byte style = dungeonGenerationStyleData.Style;
			DungeonBounds dungeonBounds = data.outerProgressionBounds[i];
			if (style != 4 && style != 5)
			{
				continue;
			}
			bool flag = style == 5;
			int num9 = num;
			int num10 = 1000;
			while (num9 > 0)
			{
				num10--;
				if (num10 <= 0)
				{
					break;
				}
				int num11 = dungeonBounds.Left + genRand.Next(dungeonBounds.Width);
				int num12 = dungeonBounds.Top + genRand.Next(dungeonBounds.Height);
				Tile tile = Main.tile[num11, num12];
				Tile tile2 = Main.tile[num11, num12 + 1];
				while (!tile.active() && num12 < Main.maxTilesY - 10)
				{
					num12++;
					tile = Main.tile[num11, num12];
				}
				num12--;
				tile = Main.tile[num11, num12];
				tile2 = Main.tile[num11, num12 + 1];
				if (tile.active() || tile.wall != dungeonGenerationStyleData.BrickWallType)
				{
					continue;
				}
				DungeonGenerationStyleData styleForTile = DungeonGenerationStyles.GetStyleForTile(data.genVars.dungeonGenerationStyles, tile2.type);
				if (styleForTile != null && styleForTile.Style == (flag ? 5 : 4) && tile2.type != styleForTile.BrickCrackedTileType && tile2.type != styleForTile.PitTrapTileType)
				{
					WorldGen.Place3x2(num11, num12, 26, flag ? 1 : 0);
					tile = Main.tile[num11, num12];
					if (tile.active() && tile.type == 26)
					{
						num9--;
					}
				}
			}
		}
		Dictionary<int, List<DungeonRoom>> dictionary = new Dictionary<int, List<DungeonRoom>>();
		BiomeDungeonRoom biomeDungeonRoom = null;
		for (int j = 0; j < data.dungeonRooms.Count; j++)
		{
			DungeonRoom dungeonRoom = data.dungeonRooms[j];
			byte style2 = dungeonRoom.settings.StyleData.Style;
			if (!dictionary.ContainsKey(style2))
			{
				dictionary.Add(style2, new List<DungeonRoom>());
			}
			dictionary[style2].Add(dungeonRoom);
			if (dungeonRoom is BiomeDungeonRoom && dungeonRoom.settings.StyleData.Style == 10)
			{
				biomeDungeonRoom = (BiomeDungeonRoom)dungeonRoom;
			}
		}
		if (dictionary.ContainsKey(4))
		{
			int num13 = num2;
			List<DungeonRoom> list = dictionary[4].ToList();
			while (list.Count > 0 && num13 > 0)
			{
				DungeonRoom dungeonRoom2 = list[genRand.Next(list.Count)];
				Point center = dungeonRoom2.InnerBounds.Center;
				_ = Main.tile[center.X, center.Y];
				WorldGen.AddShadowOrb(center.X, center.Y, crimsonHeart: false);
				if (Main.tile[center.X, center.Y].type == 31)
				{
					num13--;
				}
				list.Remove(dungeonRoom2);
			}
		}
		if (dictionary.ContainsKey(5))
		{
			int num14 = num2;
			List<DungeonRoom> list2 = dictionary[5].ToList();
			while (list2.Count > 0 && num14 > 0)
			{
				DungeonRoom dungeonRoom3 = list2[genRand.Next(list2.Count)];
				Point center2 = dungeonRoom3.InnerBounds.Center;
				_ = Main.tile[center2.X, center2.Y];
				WorldGen.AddShadowOrb(center2.X, center2.Y, crimsonHeart: true);
				if (Main.tile[center2.X, center2.Y].type == 31)
				{
					num14--;
				}
				list2.Remove(dungeonRoom3);
			}
		}
		if (dictionary.ContainsKey(9))
		{
			List<DungeonRoom> list3 = dictionary[9].ToList();
			while (list3.Count > 0)
			{
				DungeonRoom dungeonRoom4 = list3[0];
				Point center3 = dungeonRoom4.InnerBounds.Center;
				WorldGen.AddBeeLarva(center3.X - 1, center3.Y - 3);
				list3.Remove(dungeonRoom4);
			}
		}
		if (data.Type == DungeonType.DualDungeon)
		{
			for (int k = 0; k < data.genVars.dungeonGenerationStyles.Count; k++)
			{
				DungeonGenerationStyleData dungeonGenerationStyleData2 = data.genVars.dungeonGenerationStyles[k];
				List<DungeonRoom> list4 = dictionary[dungeonGenerationStyleData2.Style];
				int num15 = num6;
				int num16 = 2;
				num16 = WorldGen.GetWorldSize() switch
				{
					1 => 4, 
					2 => 6, 
					_ => 2, 
				};
				if (list4 == null)
				{
					continue;
				}
				while (list4.Count > 0 && num15 > 0)
				{
					DungeonRoom dungeonRoom5 = list4[genRand.Next(list4.Count)];
					if (dungeonRoom5 is BiomeDungeonRoom)
					{
						list4.Remove(dungeonRoom5);
						continue;
					}
					int x = dungeonRoom5.InnerBounds.Center.X;
					int y = dungeonRoom5.InnerBounds.Bottom - 5;
					int width = dungeonRoom5.InnerBounds.Width / 2;
					int height = (int)((float)dungeonRoom5.InnerBounds.Height * 0.75f);
					bool flag2 = num16 > 0 || genRand.Next(8) == 0;
					DungeonGenerationStyleData styleData = dungeonRoom5.settings.StyleData;
					DungeonPitTrap dungeonPitTrap = new DungeonPitTrap(new DungeonPitTrapSettings
					{
						Style = styleData,
						Width = width,
						Height = height,
						EdgeWidth = 2,
						EdgeHeight = 2,
						TopDensity = 8,
						ConnectedRoom = dungeonRoom5,
						Flooded = flag2
					}, addToFeatures: false);
					if (!dungeonRoom5.settings.StyleData.CanGenerateFeatureAt(data, dungeonRoom5, dungeonPitTrap, x, y))
					{
						list4.Remove(dungeonRoom5);
						continue;
					}
					if (dungeonPitTrap.GenerateFeature(data, x, y))
					{
						DungeonCrawler.CurrentDungeonData.dungeonFeatures.Add(dungeonPitTrap);
						if (flag2 && num16 > 0)
						{
							num16--;
						}
						num15--;
					}
					else
					{
						height = Math.Max(10, (int)((float)dungeonRoom5.InnerBounds.Height * 0.5f));
						dungeonPitTrap = new DungeonPitTrap(new DungeonPitTrapSettings
						{
							Style = styleData,
							Width = width,
							Height = height,
							EdgeWidth = 2,
							EdgeHeight = 2,
							TopDensity = 8,
							ConnectedRoom = dungeonRoom5,
							Flooded = flag2
						}, addToFeatures: false);
						if (!dungeonRoom5.settings.StyleData.CanGenerateFeatureAt(data, dungeonRoom5, dungeonPitTrap, x, y))
						{
							list4.Remove(dungeonRoom5);
							continue;
						}
						if (dungeonPitTrap.GenerateFeature(data, x, y))
						{
							DungeonCrawler.CurrentDungeonData.dungeonFeatures.Add(dungeonPitTrap);
							if (flag2 && num16 > 0)
							{
								num16--;
							}
							num15--;
						}
						else
						{
							width = (int)((float)(dungeonRoom5.InnerBounds.Width / 2) * 0.75f);
							dungeonPitTrap = new DungeonPitTrap(new DungeonPitTrapSettings
							{
								Style = styleData,
								Width = width,
								Height = height,
								EdgeWidth = 2,
								EdgeHeight = 2,
								TopDensity = 8,
								ConnectedRoom = dungeonRoom5,
								Flooded = flag2
							}, addToFeatures: false);
							if (!dungeonRoom5.settings.StyleData.CanGenerateFeatureAt(data, dungeonRoom5, dungeonPitTrap, x, y))
							{
								list4.Remove(dungeonRoom5);
								continue;
							}
							if (dungeonPitTrap.GenerateFeature(data, x, y))
							{
								DungeonCrawler.CurrentDungeonData.dungeonFeatures.Add(dungeonPitTrap);
								if (flag2 && num16 > 0)
								{
									num16--;
								}
								num15--;
							}
						}
					}
					list4.Remove(dungeonRoom5);
				}
			}
		}
		if (dictionary.ContainsKey(3))
		{
			List<DungeonRoom> list5 = dictionary[3].ToList();
			while (list5.Count > 0 && num3 > 0)
			{
				DungeonRoom dungeonRoom6 = list5[genRand.Next(list5.Count)];
				int num17 = 20;
				while (num17 > 0 && num3 > 0)
				{
					num17--;
					int num18 = dungeonRoom6.InnerBounds.Left + genRand.Next(dungeonRoom6.InnerBounds.Width);
					int num19 = dungeonRoom6.InnerBounds.Top + genRand.Next(dungeonRoom6.InnerBounds.Height);
					if (!WorldGen.InWorld(num18, num19, 25))
					{
						continue;
					}
					Tile tile3 = Main.tile[num18, num19];
					while (num19 < Main.UnderworldLayer - 10 && !tile3.active())
					{
						num19++;
						tile3 = Main.tile[num18, num19];
					}
					if (tile3.active() && tile3.type == DungeonGenerationStyles.Desert.BrickTileType)
					{
						DungeonDropTrap dungeonDropTrap = new DungeonDropTrap(new DungeonDropTrapSettings
						{
							StyleData = DungeonGenerationStyles.Desert,
							DropTrapType = ((genRand.Next(2) != 0) ? DungeonDropTrapType.Lava : DungeonDropTrapType.Sand)
						}, addToFeatures: false);
						if (dungeonDropTrap.GenerateFeature(data, num18, num19))
						{
							DungeonCrawler.CurrentDungeonData.dungeonFeatures.Add(dungeonDropTrap);
							num3--;
						}
					}
				}
				list5.Remove(dungeonRoom6);
			}
		}
		if (dictionary.ContainsKey(2))
		{
			List<DungeonRoom> list6 = dictionary[2].ToList();
			while (list6.Count > 0 && num4 > 0)
			{
				DungeonRoom dungeonRoom7 = list6[genRand.Next(list6.Count)];
				int num20 = 20;
				while (num20 > 0 && num4 > 0)
				{
					num20--;
					int num21 = dungeonRoom7.InnerBounds.Left + genRand.Next(dungeonRoom7.InnerBounds.Width);
					int num22 = dungeonRoom7.InnerBounds.Top + genRand.Next(dungeonRoom7.InnerBounds.Height);
					if (!WorldGen.InWorld(num21, num22, 25))
					{
						continue;
					}
					Tile tile4 = Main.tile[num21, num22];
					while (num22 < Main.UnderworldLayer - 10 && !tile4.active())
					{
						num22++;
						tile4 = Main.tile[num21, num22];
					}
					if (tile4.active() && tile4.type == DungeonGenerationStyles.Snow.BrickTileType)
					{
						DungeonDropTrap dungeonDropTrap2 = new DungeonDropTrap(new DungeonDropTrapSettings
						{
							StyleData = DungeonGenerationStyles.Snow,
							DropTrapType = DungeonDropTrapType.Slush
						}, addToFeatures: false);
						if (dungeonDropTrap2.GenerateFeature(data, num21, num22))
						{
							DungeonCrawler.CurrentDungeonData.dungeonFeatures.Add(dungeonDropTrap2);
							num4--;
						}
					}
				}
				list6.Remove(dungeonRoom7);
			}
		}
		if (dictionary.ContainsKey(1))
		{
			List<DungeonRoom> list7 = dictionary[1].ToList();
			while (list7.Count > 0 && num5 > 0)
			{
				DungeonRoom dungeonRoom8 = list7[genRand.Next(list7.Count)];
				int num23 = 20;
				while (num23 > 0 && num5 > 0)
				{
					num23--;
					int num24 = dungeonRoom8.InnerBounds.Left + genRand.Next(dungeonRoom8.InnerBounds.Width);
					int num25 = dungeonRoom8.InnerBounds.Top + genRand.Next(dungeonRoom8.InnerBounds.Height);
					if (!WorldGen.InWorld(num24, num25, 25))
					{
						continue;
					}
					Tile tile5 = Main.tile[num24, num25];
					while (num25 < Main.UnderworldLayer - 10 && !tile5.active())
					{
						num25++;
						tile5 = Main.tile[num24, num25];
					}
					if (tile5.active() && tile5.type == DungeonGenerationStyles.Cavern.BrickTileType)
					{
						DungeonDropTrap dungeonDropTrap3 = new DungeonDropTrap(new DungeonDropTrapSettings
						{
							StyleData = DungeonGenerationStyles.Cavern,
							DropTrapType = DungeonDropTrapType.Silt
						}, addToFeatures: false);
						if (dungeonDropTrap3.GenerateFeature(data, num24, num25))
						{
							DungeonCrawler.CurrentDungeonData.dungeonFeatures.Add(dungeonDropTrap3);
							num5--;
						}
					}
				}
				list7.Remove(dungeonRoom8);
			}
		}
		for (int l = 0; l < data.genVars.dungeonGenerationStyles.Count; l++)
		{
			DungeonGenerationStyleData dungeonGenerationStyleData3 = data.genVars.dungeonGenerationStyles[l];
			byte style3 = dungeonGenerationStyleData3.Style;
			DungeonBounds dungeonBounds2 = data.outerProgressionBounds[l];
			if (style3 == 3)
			{
				int num26 = 1000;
				int num27 = num7;
				while (num27 > 0)
				{
					num26--;
					if (num26 <= 0)
					{
						break;
					}
					int num28 = dungeonBounds2.Left + genRand.Next(dungeonBounds2.Width);
					int num29 = dungeonBounds2.Top + genRand.Next(dungeonBounds2.Height);
					Tile tile6 = Main.tile[num28, num29];
					if (tile6.wall == dungeonGenerationStyleData3.BrickWallType)
					{
						DungeonGenerationStyleData styleForTile2 = DungeonGenerationStyles.GetStyleForTile(data.genVars.dungeonGenerationStyles, tile6.type);
						if (styleForTile2 != null && styleForTile2.Style == 3)
						{
							DungeonTileClumpSettings dungeonTileClumpSettings = new DungeonTileClumpSettings();
							dungeonTileClumpSettings.RandomSeed = genRand.Next();
							dungeonTileClumpSettings.Strength = 25 + genRand.Next(10);
							dungeonTileClumpSettings.Steps = 25 + genRand.Next(10);
							dungeonTileClumpSettings.TileType = 53;
							dungeonTileClumpSettings.WallType = 216;
							dungeonTileClumpSettings.AreaToGenerateIn = null;
							dungeonTileClumpSettings.OnlyReplaceThisTileType = styleForTile2.BrickTileType;
							dungeonTileClumpSettings.OnlyReplaceThisWallType = styleForTile2.BrickWallType;
							new DungeonTileClump(dungeonTileClumpSettings).GenerateFeature(data, num28, num29);
							num27--;
						}
					}
				}
				num26 = 1000;
				num27 = num7;
				while (num27 > 0)
				{
					num26--;
					if (num26 <= 0)
					{
						break;
					}
					int num30 = dungeonBounds2.Left + genRand.Next(dungeonBounds2.Width);
					int num31 = dungeonBounds2.Top + genRand.Next(dungeonBounds2.Height);
					Tile tile7 = Main.tile[num30, num31];
					if (tile7.wall == dungeonGenerationStyleData3.BrickWallType)
					{
						DungeonGenerationStyleData styleForTile3 = DungeonGenerationStyles.GetStyleForTile(data.genVars.dungeonGenerationStyles, tile7.type);
						if (styleForTile3 != null && styleForTile3.Style == 3)
						{
							DungeonTileClumpSettings dungeonTileClumpSettings2 = new DungeonTileClumpSettings();
							dungeonTileClumpSettings2.RandomSeed = genRand.Next();
							dungeonTileClumpSettings2.Strength = 25 + genRand.Next(10);
							dungeonTileClumpSettings2.Steps = 25 + genRand.Next(10);
							dungeonTileClumpSettings2.TileType = 397;
							dungeonTileClumpSettings2.WallType = 216;
							dungeonTileClumpSettings2.AreaToGenerateIn = null;
							dungeonTileClumpSettings2.OnlyReplaceThisTileType = styleForTile3.BrickTileType;
							dungeonTileClumpSettings2.OnlyReplaceThisWallType = styleForTile3.BrickWallType;
							new DungeonTileClump(dungeonTileClumpSettings2).GenerateFeature(data, num30, num31);
							num27--;
						}
					}
				}
				num26 = 1000;
				num27 = num7 * 2;
				while (num27 > 0)
				{
					num26--;
					if (num26 <= 0)
					{
						break;
					}
					int num32 = dungeonBounds2.Left + genRand.Next(dungeonBounds2.Width);
					int num33 = dungeonBounds2.Top + genRand.Next(dungeonBounds2.Height);
					Tile tile8 = Main.tile[num32, num33];
					if (tile8.wall == dungeonGenerationStyleData3.BrickWallType)
					{
						DungeonGenerationStyleData styleForTile4 = DungeonGenerationStyles.GetStyleForTile(data.genVars.dungeonGenerationStyles, tile8.type);
						if (styleForTile4 != null && styleForTile4.Style == 3)
						{
							DungeonTileClumpSettings dungeonTileClumpSettings3 = new DungeonTileClumpSettings();
							dungeonTileClumpSettings3.RandomSeed = genRand.Next();
							dungeonTileClumpSettings3.Strength = 15 + genRand.Next(5);
							dungeonTileClumpSettings3.Steps = 15 + genRand.Next(5);
							dungeonTileClumpSettings3.TileType = 404;
							dungeonTileClumpSettings3.WallType = 223;
							dungeonTileClumpSettings3.AreaToGenerateIn = null;
							dungeonTileClumpSettings3.OnlyReplaceThisTileType = styleForTile4.BrickTileType;
							dungeonTileClumpSettings3.OnlyReplaceThisWallType = styleForTile4.BrickWallType;
							new DungeonTileClump(dungeonTileClumpSettings3).GenerateFeature(data, num32, num33);
							num27--;
						}
					}
				}
			}
			if (style3 != 2)
			{
				continue;
			}
			int num34 = 1000;
			int num35 = num8;
			while (num35 > 0)
			{
				num34--;
				if (num34 <= 0)
				{
					break;
				}
				int num36 = dungeonBounds2.Left + genRand.Next(dungeonBounds2.Width);
				int num37 = dungeonBounds2.Top + genRand.Next(dungeonBounds2.Height);
				Tile tile9 = Main.tile[num36, num37];
				if (tile9.wall == dungeonGenerationStyleData3.BrickWallType)
				{
					DungeonGenerationStyleData styleForTile5 = DungeonGenerationStyles.GetStyleForTile(data.genVars.dungeonGenerationStyles, tile9.type);
					if (styleForTile5 != null && styleForTile5.Style == 2)
					{
						DungeonTileClumpSettings dungeonTileClumpSettings4 = new DungeonTileClumpSettings();
						dungeonTileClumpSettings4.RandomSeed = genRand.Next();
						dungeonTileClumpSettings4.Strength = 25 + genRand.Next(10);
						dungeonTileClumpSettings4.Steps = 25 + genRand.Next(10);
						dungeonTileClumpSettings4.TileType = 147;
						dungeonTileClumpSettings4.WallType = 40;
						dungeonTileClumpSettings4.AreaToGenerateIn = null;
						dungeonTileClumpSettings4.OnlyReplaceThisTileType = styleForTile5.BrickTileType;
						dungeonTileClumpSettings4.OnlyReplaceThisWallType = styleForTile5.BrickWallType;
						new DungeonTileClump(dungeonTileClumpSettings4).GenerateFeature(data, num36, num37);
						num35--;
					}
				}
			}
			num34 = 1000;
			num35 = num8;
			while (num35 > 0)
			{
				num34--;
				if (num34 <= 0)
				{
					break;
				}
				int num38 = dungeonBounds2.Left + genRand.Next(dungeonBounds2.Width);
				int num39 = dungeonBounds2.Top + genRand.Next(dungeonBounds2.Height);
				Tile tile10 = Main.tile[num38, num39];
				if (tile10.wall == dungeonGenerationStyleData3.BrickWallType)
				{
					DungeonGenerationStyleData styleForTile6 = DungeonGenerationStyles.GetStyleForTile(data.genVars.dungeonGenerationStyles, tile10.type);
					if (styleForTile6 != null && styleForTile6.Style == 2)
					{
						DungeonTileClumpSettings dungeonTileClumpSettings5 = new DungeonTileClumpSettings();
						dungeonTileClumpSettings5.RandomSeed = genRand.Next();
						dungeonTileClumpSettings5.Strength = 25 + genRand.Next(10);
						dungeonTileClumpSettings5.Steps = 25 + genRand.Next(10);
						dungeonTileClumpSettings5.TileType = 224;
						dungeonTileClumpSettings5.WallType = 40;
						dungeonTileClumpSettings5.AreaToGenerateIn = null;
						dungeonTileClumpSettings5.OnlyReplaceThisTileType = styleForTile6.BrickTileType;
						dungeonTileClumpSettings5.OnlyReplaceThisWallType = styleForTile6.BrickWallType;
						new DungeonTileClump(dungeonTileClumpSettings5).GenerateFeature(data, num38, num39);
						num35--;
					}
				}
			}
		}
		for (int m = 0; m < data.dungeonRooms.Count; m++)
		{
			data.dungeonRooms[m].GenerateEarlyDungeonFeaturesInRoom(data);
		}
		if (biomeDungeonRoom != null)
		{
			int x2 = biomeDungeonRoom.InnerBounds.Center.X;
			int num40 = (biomeDungeonRoom.InnerBounds.Top + biomeDungeonRoom.InnerBounds.Center.Y) / 2;
			Tile tile11 = Main.tile[x2, num40];
			while (!tile11.active())
			{
				num40++;
				tile11 = Main.tile[x2, num40];
			}
			for (int n = -1; n <= 1; n++)
			{
				int num41 = x2 + n;
				int num42 = num40;
				Tile tile12 = Main.tile[num41, num42];
				while (!tile12.active())
				{
					tile12.ClearTile();
					tile12.active(active: true);
					tile12.type = 226;
					num42++;
					tile12 = Main.tile[num41, num42];
				}
			}
			WorldGen.AddLihzahrdAltar(x2 - 1, num40 - 2);
		}
		if (data.Type != DungeonType.Default)
		{
			return;
		}
		num6 = (int)((double)Main.maxTilesX * 2.0 * data.dungeonStepScalar);
		int num43;
		for (num43 = 0; num43 < num6; num43++)
		{
			int x3 = genRand.Next(data.dungeonBounds.Left, data.dungeonBounds.Right);
			int num44 = data.dungeonBounds.Top;
			if (num44 < Main.dungeonY + 25)
			{
				num44 = Main.dungeonY + 25;
			}
			if ((double)num44 < Main.worldSurface)
			{
				num44 = (int)Main.worldSurface;
			}
			int y2 = genRand.Next(num44, data.dungeonBounds.Bottom);
			bool flag3 = data.makeNextPitTrapFlooded || genRand.Next(8) == 0;
			int num45 = genRand.Next(6, 10);
			if (new DungeonPitTrap(new DungeonPitTrapSettings
			{
				Style = data.genVars.dungeonStyle,
				Width = genRand.Next(8, 19),
				Height = genRand.Next(19, 46),
				EdgeWidth = genRand.Next(6, 10),
				EdgeHeight = num45,
				TopDensity = num45,
				Flooded = flag3
			}).GenerateFeature(data, x3, y2))
			{
				if (flag3)
				{
					data.makeNextPitTrapFlooded = false;
				}
				num43 += 1500;
			}
			else
			{
				num43++;
			}
		}
	}
}
