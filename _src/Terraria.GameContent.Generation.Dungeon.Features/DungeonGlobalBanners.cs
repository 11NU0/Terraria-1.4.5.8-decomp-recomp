using System;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Utilities;

namespace Terraria.GameContent.Generation.Dungeon.Features;

public class DungeonGlobalBanners : GlobalDungeonFeature
{
	public DungeonGlobalBanners(DungeonFeatureSettings settings)
		: base(settings)
	{
		DungeonCrawler.CurrentDungeonData.dungeonFeatures.Add(this);
	}

	public override bool GenerateFeature(DungeonData data)
	{
		generated = false;
		Banners(data);
		generated = true;
		return true;
	}

	public void Banners(DungeonData data)
	{
		UnifiedRandom genRand = WorldGen.genRand;
		float num = (float)Main.maxTilesX / 4200f;
		double num2 = Math.Max(1.0, data.globalFeatureScalar * 0.75);
		int num3 = (int)((double)(200f * num) * num2);
		for (int i = 0; i < num3; i++)
		{
			int num4 = genRand.Next(data.dungeonBounds.Left, data.dungeonBounds.Right);
			int num5 = genRand.Next(data.dungeonBounds.Top, data.dungeonBounds.Bottom);
			int num6 = 1000;
			while (!DungeonUtils.IsConsideredDungeonWall(Main.tile[num4, num5].wall) || Main.tile[num4, num5].active())
			{
				num6--;
				if (num6 <= 0)
				{
					break;
				}
				num4 = genRand.Next(data.dungeonBounds.Left, data.dungeonBounds.Right);
				num5 = genRand.Next(data.dungeonBounds.Top, data.dungeonBounds.Bottom);
			}
			num6 = 1000;
			while (!WorldGen.SolidTile(num4, num5) && num5 > 10)
			{
				num6--;
				if (num6 <= 0)
				{
					break;
				}
				num5--;
			}
			num5++;
			if (!data.CanGenerateFeatureAt(this, num4, num5) || !DungeonUtils.IsConsideredDungeonWall(Main.tile[num4, num5].wall) || Main.tile[num4, num5 - 1].type == 48 || Main.tile[num4, num5].active() || Main.tile[num4, num5 + 1].active() || Main.tile[num4, num5 + 2].active() || Main.tile[num4, num5 + 3].active())
			{
				continue;
			}
			bool flag = true;
			for (int j = num4 - 1; j <= num4 + 1; j++)
			{
				for (int k = num5; k <= num5 + 3; k++)
				{
					if (Main.tile[j, k].active() && (Main.tile[j, k].type == 10 || Main.tile[j, k].type == 11 || Main.tile[j, k].type == 91))
					{
						flag = false;
					}
				}
			}
			if (!flag)
			{
				continue;
			}
			ushort type = 91;
			int num7 = 0;
			DungeonGenerationStyleData styleForWall = DungeonGenerationStyles.GetStyleForWall(data.genVars.dungeonGenerationStyles, Main.tile[num4, num5].wall);
			if (styleForWall != null && styleForWall.BannerItemTypes == null)
			{
				continue;
			}
			if (styleForWall == null || styleForWall.Style == 0 || styleForWall.BannerItemTypes.Length == 0)
			{
				int num8 = 0;
				if (Main.tile[num4, num5].wall == data.wallVariants[1])
				{
					num8 = 1;
				}
				if (Main.tile[num4, num5].wall == data.wallVariants[2])
				{
					num8 = 2;
				}
				num8 *= 2;
				num8 += genRand.Next(2);
				num7 = data.bannerStyles[num8];
			}
			else
			{
				PlacementDetails placementDetails = ItemID.Sets.DerivedPlacementDetails[styleForWall.BannerItemTypes[genRand.Next(styleForWall.BannerItemTypes.Length)]];
				type = (ushort)placementDetails.tileType;
				num7 = placementDetails.tileStyle;
			}
			WorldGen.PlaceTile(num4, num5, type, mute: true, forced: false, -1, num7);
		}
	}
}
