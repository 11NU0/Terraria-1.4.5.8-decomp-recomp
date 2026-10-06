using System;
using Terraria.DataStructures;
using Terraria.Utilities;

namespace Terraria.GameContent.Generation.Dungeon.Features;

public class DungeonGlobalBookshelves : GlobalDungeonFeature
{
	public DungeonGlobalBookshelves(DungeonFeatureSettings settings)
		: base(settings)
	{
		DungeonCrawler.CurrentDungeonData.dungeonFeatures.Add(this);
	}

	public override bool GenerateFeature(DungeonData data)
	{
		generated = false;
		Bookshelves(data);
		generated = true;
		return true;
	}

	public void Bookshelves(DungeonData data)
	{
		UnifiedRandom genRand = WorldGen.genRand;
		_ = data.dungeonEntrance;
		int num = 0;
		int num2 = 1000;
		int num3 = 0;
		int num4 = -1;
		if (data.Type == DungeonType.DualDungeon)
		{
			num4 = WorldGen.GetWorldSize() switch
			{
				1 => 10, 
				2 => 15, 
				_ => 5, 
			};
		}
		while (num3 < Main.maxTilesX / 20)
		{
			num++;
			int num5 = genRand.Next(data.dungeonBounds.Left, data.dungeonBounds.Right);
			int num6 = genRand.Next(data.dungeonBounds.Top, data.dungeonBounds.Bottom);
			bool flag = true;
			if (DungeonUtils.IsConsideredDungeonWall(Main.tile[num5, num6].wall) && !Main.tile[num5, num6].active())
			{
				int num7 = 1;
				if (genRand.Next(2) == 0)
				{
					num7 = -1;
				}
				while (flag && !Main.tile[num5, num6].active())
				{
					num5 -= num7;
					if (num5 < 5 || num5 > Main.maxTilesX - 5)
					{
						flag = false;
					}
					else if (Main.tile[num5, num6].active() && !DungeonUtils.IsConsideredDungeonTile(Main.tile[num5, num6].type))
					{
						flag = false;
					}
				}
				if (flag && Main.tile[num5, num6].active() && DungeonUtils.IsConsideredDungeonTile(Main.tile[num5, num6].type) && Main.tile[num5, num6 - 1].active() && DungeonUtils.IsConsideredDungeonTile(Main.tile[num5, num6 - 1].type) && Main.tile[num5, num6 + 1].active() && DungeonUtils.IsConsideredDungeonTile(Main.tile[num5, num6 + 1].type))
				{
					num5 += num7;
					for (int i = num5 - 3; i <= num5 + 3; i++)
					{
						for (int j = num6 - 3; j <= num6 + 3; j++)
						{
							if (Main.tile[i, j].active() && Main.tile[i, j].type == 19)
							{
								flag = false;
								break;
							}
						}
					}
					if (flag && (!Main.tile[num5, num6 - 1].active() & !Main.tile[num5, num6 - 2].active() & !Main.tile[num5, num6 - 3].active()))
					{
						if (!data.CanGenerateFeatureAt(this, num5, num6))
						{
							flag = false;
							continue;
						}
						int k = num5;
						int num8 = num5;
						for (; k > data.dungeonBounds.Left && k < data.dungeonBounds.Right && !Main.tile[k, num6].active() && !Main.tile[k, num6 - 1].active() && !Main.tile[k, num6 + 1].active(); k += num7)
						{
						}
						k = Math.Abs(num5 - k);
						bool flag2 = true;
						bool flag3 = genRand.Next(2) == 0;
						if (k > 5)
						{
							int num9 = -1;
							int min = 1;
							int max = 4;
							DungeonGenerationStyleData styleForWall = DungeonGenerationStyles.GetStyleForWall(data.genVars.dungeonGenerationStyles, Main.tile[num5, num6].wall);
							if (styleForWall != null)
							{
								flag2 = styleForWall.Style == 0;
								if (!flag2)
								{
									flag3 = false;
								}
								num9 = styleForWall.GetPlatformStyle(genRand);
								styleForWall.GetBookshelfMinMaxSizes(min, max, out min, out max);
							}
							for (int num10 = genRand.Next(min, max); num10 > 0; num10--)
							{
								Tile tile = Main.tile[num5, num6];
								tile.active(active: true);
								tile.Clear(TileDataType.Slope);
								tile.type = 19;
								int num11 = data.shelfStyles[0];
								if (tile.wall == data.wallVariants[1])
								{
									num11 = data.shelfStyles[1];
								}
								if (tile.wall == data.wallVariants[2])
								{
									num11 = data.shelfStyles[2];
								}
								if (num9 > -1)
								{
									num11 = num9;
								}
								tile.frameY = (short)(18 * num11);
								WorldGen.TileFrame(num5, num6);
								if (flag3)
								{
									short frameX = 90;
									WorldGen.PlaceTile(num5, num6 - 1, 50, mute: true);
									if (genRand.Next(50) == 0 && (double)num6 > (Main.worldSurface + Main.rockLayer) / 2.0 && Main.tile[num5, num6 - 1].type == 50)
									{
										Main.tile[num5, num6 - 1].frameX = frameX;
									}
								}
								num5 += num7;
							}
							num = 0;
							num3++;
							if (!flag3 && genRand.Next(2) == 0)
							{
								num5 = num8;
								num6--;
								if (flag2)
								{
									int num12 = ((genRand.Next(4) == 0) ? 1 : 0);
									if (num4 > 0)
									{
										num12 = 1;
									}
									switch (num12)
									{
									case 0:
										num12 = 13;
										break;
									case 1:
										num12 = 49;
										break;
									}
									WorldGen.PlaceTile(num5, num6, num12, mute: true);
									if (Main.tile[num5, num6].type == 13)
									{
										if (genRand.Next(2) == 0)
										{
											Main.tile[num5, num6].frameX = 18;
										}
										else
										{
											Main.tile[num5, num6].frameX = 36;
										}
									}
									if (Main.tile[num5, num6].active() && Main.tile[num5, num6].type == 49)
									{
										num4--;
									}
								}
								else
								{
									ushort type = 13;
									WorldGen.PlaceTile(num5, num6, type, mute: true);
									if (Main.tile[num5, num6].type == 13)
									{
										if (genRand.Next(2) == 0)
										{
											Main.tile[num5, num6].frameX = 18;
										}
										else
										{
											Main.tile[num5, num6].frameX = 36;
										}
									}
								}
							}
						}
					}
				}
			}
			if (num > num2)
			{
				num = 0;
				num3++;
			}
		}
	}
}
