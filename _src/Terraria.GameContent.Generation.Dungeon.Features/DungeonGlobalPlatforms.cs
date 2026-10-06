using System;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Utilities;

namespace Terraria.GameContent.Generation.Dungeon.Features;

public class DungeonGlobalPlatforms : GlobalDungeonFeature
{
	public DungeonGlobalPlatforms(DungeonFeatureSettings settings)
		: base(settings)
	{
		DungeonCrawler.CurrentDungeonData.dungeonFeatures.Add(this);
	}

	public override bool GenerateFeature(DungeonData data)
	{
		generated = false;
		Platforms(data);
		generated = true;
		return true;
	}

	public void Platforms(DungeonData data)
	{
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		UnifiedRandom genRand = WorldGen.genRand;
		PlacementDetails placementDetails = ItemID.Sets.DerivedPlacementDetails[data.platformItemType];
		for (int i = 0; i < data.dungeonPlatformData.Count; i++)
		{
			DungeonPlatformData dungeonPlatformData = data.dungeonPlatformData[i];
			if (!WorldGen.InWorld(dungeonPlatformData.Position, 30))
			{
				continue;
			}
			int x = dungeonPlatformData.Position.X;
			int y = dungeonPlatformData.Position.Y;
			int num = placementDetails.tileStyle;
			if (dungeonPlatformData.OverrideStyle.HasValue && dungeonPlatformData.OverrideStyle >= 0)
			{
				num = dungeonPlatformData.OverrideStyle.Value;
			}
			if (Main.tile[x, y].wall >= 94 && Main.tile[x, y].wall <= 105)
			{
				num = 54;
			}
			int num2 = -1;
			bool forcePlacement = dungeonPlatformData.ForcePlacement;
			int num3 = 5;
			int num4 = 10;
			if ((double)y < Main.worldSurface + 50.0)
			{
				num4 = 20;
			}
			if (dungeonPlatformData.OverrideMaxLengthAllowed > 0)
			{
				num4 = dungeonPlatformData.OverrideMaxLengthAllowed;
			}
			if (dungeonPlatformData.OverrideHeightFluff.HasValue)
			{
				num3 = dungeonPlatformData.OverrideHeightFluff.Value;
			}
			double num5 = (dungeonPlatformData.InAHallway ? data.HallSizeScalar : data.RoomSizeScalar);
			num4 = (int)((double)num4 * num5);
			for (int j = y - num3; j <= y + num3; j++)
			{
				int num6 = x;
				int num7 = x;
				bool flag = false;
				if (!forcePlacement && Main.tile[num6, j].active())
				{
					flag = true;
				}
				else
				{
					while (!Main.tile[num6, j].active())
					{
						num6--;
						if (!forcePlacement && ((Main.tile[num6, j].active() && !DungeonUtils.IsConsideredDungeonTile(Main.tile[num6, j].type)) || num6 == 0))
						{
							flag = true;
							break;
						}
						if (dungeonPlatformData.canPlaceHereCallback != null && !dungeonPlatformData.canPlaceHereCallback(data, num6, j))
						{
							flag = true;
							break;
						}
						if (num6 <= 10)
						{
							break;
						}
					}
					while (!Main.tile[num7, j].active())
					{
						num7++;
						if (!forcePlacement && ((Main.tile[num7, j].active() && !DungeonUtils.IsConsideredDungeonTile(Main.tile[num7, j].type)) || num7 == Main.maxTilesX - 1))
						{
							flag = true;
							break;
						}
						if (dungeonPlatformData.canPlaceHereCallback != null && !dungeonPlatformData.canPlaceHereCallback(data, num7, j))
						{
							flag = true;
							break;
						}
						if (num7 >= Main.maxTilesX - 10)
						{
							break;
						}
					}
				}
				if (flag || (!forcePlacement && num7 - num6 > num4))
				{
					continue;
				}
				bool flag2 = true;
				int num8 = Math.Max(0, x - num4 / 2 - 2);
				int num9 = Math.Min(Main.maxTilesX - 1, x + num4 / 2 + 2);
				int num10 = j - num3;
				int num11 = j + num3;
				if (!forcePlacement)
				{
					if (!dungeonPlatformData.SkipOtherPlatformsCheck)
					{
						for (int k = num8; k <= num9; k++)
						{
							for (int l = num10; l <= num11; l++)
							{
								if (Main.tile[k, l].active() && Main.tile[k, l].type == 19)
								{
									flag2 = false;
									break;
								}
							}
						}
					}
					if (!dungeonPlatformData.SkipSpaceCheck)
					{
						for (int num12 = j + 3; num12 >= j - 5; num12--)
						{
							if (Main.tile[x, num12].active())
							{
								flag2 = false;
								break;
							}
						}
					}
				}
				if (flag2)
				{
					num2 = j;
					break;
				}
			}
			if ((!forcePlacement || num2 <= 0) && (num2 <= y - num3 - 5 || num2 >= y + num3 + 5))
			{
				continue;
			}
			int num13 = x;
			int num14 = num2;
			int num15 = x + 1;
			while (!Main.tile[num13, num14].active())
			{
				Main.tile[num13, num14].active(active: true);
				Main.tile[num13, num14].type = 19;
				Main.tile[num13, num14].Clear(TileDataType.Slope);
				Main.tile[num13, num14].frameY = (short)(18 * num);
				WorldGen.TileFrame(num13, num14);
				num13--;
				if (num13 <= 10)
				{
					break;
				}
			}
			while (!Main.tile[num15, num14].active())
			{
				Main.tile[num15, num14].active(active: true);
				Main.tile[num15, num14].type = 19;
				Main.tile[num15, num14].Clear(TileDataType.Slope);
				Main.tile[num15, num14].frameY = (short)(18 * num);
				WorldGen.TileFrame(num15, num14);
				num15++;
				if (num15 >= Main.maxTilesX - 10)
				{
					break;
				}
			}
			if (!dungeonPlatformData.IsAShelf)
			{
				continue;
			}
			for (int m = num13; m < num15; m++)
			{
				if (dungeonPlatformData.PlaceWaterCandlesChance > 0.0 && genRand.NextDouble() < dungeonPlatformData.PlaceWaterCandlesChance)
				{
					DungeonUtils.GenerateDungeonWaterCandle(m, num14 - 1);
				}
				else if (dungeonPlatformData.PlacePotsChance > 0.0 && genRand.NextDouble() < dungeonPlatformData.PlacePotsChance)
				{
					DungeonUtils.GenerateDungeonPot(m, num14 - 1);
				}
				else if (dungeonPlatformData.PlacePotionBottlesChance > 0.0 && genRand.NextDouble() < dungeonPlatformData.PlacePotionBottlesChance)
				{
					DungeonUtils.GenerateDungeonPotionBottle(m, num14 - 1);
				}
				else if (dungeonPlatformData.PlaceBooksChance > 0.0 && genRand.NextDouble() < dungeonPlatformData.PlaceBooksChance)
				{
					if (dungeonPlatformData.NoWaterbolt)
					{
						DungeonUtils.GenerateDungeonBook(m, num14 - 1, waterbolt: false);
					}
					else
					{
						DungeonUtils.GenerateDungeonBook(m, num14 - 1);
					}
				}
			}
		}
	}
}
