using System;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Utilities;

namespace Terraria.GameContent.Generation.Dungeon.Features;

public class DungeonGlobalPaintings : GlobalDungeonFeature
{
	public static int lihzahrdPaintingsPlaced = 0;

	public static int lihzahrdPaintingsMax = 1;

	public DungeonGlobalPaintings(DungeonFeatureSettings settings)
		: base(settings)
	{
		DungeonCrawler.CurrentDungeonData.dungeonFeatures.Add(this);
	}

	public override bool GenerateFeature(DungeonData data)
	{
		generated = false;
		Paintings(data);
		generated = true;
		return true;
	}

	public void Paintings(DungeonData data)
	{
		UnifiedRandom genRand = WorldGen.genRand;
		float num = (float)Main.maxTilesX / 4200f;
		lihzahrdPaintingsPlaced = 0;
		switch (WorldGen.GetWorldSize())
		{
		default:
			lihzahrdPaintingsMax = 1;
			break;
		case 1:
			lihzahrdPaintingsMax = 2;
			break;
		case 2:
			lihzahrdPaintingsMax = 2 + genRand.Next(2);
			break;
		}
		int num2 = data.wallVariants[0];
		double num3 = Math.Max(1.0, data.globalFeatureScalar * 0.75);
		int num4 = (int)((double)(100f * num) * num3);
		int num5 = num4 * 3;
		for (int i = 0; i < num4; i++)
		{
			num5--;
			if (num5 <= 0)
			{
				break;
			}
			int num6 = genRand.Next(data.dungeonBounds.Left, data.dungeonBounds.Right);
			int num7 = genRand.Next((int)Main.worldSurface, data.dungeonBounds.Bottom);
			int num8 = 1000;
			while (!DungeonUtils.IsConsideredDungeonWall(Main.tile[num6, num7].wall) || Main.tile[num6, num7].active())
			{
				num8--;
				if (num8 <= 0)
				{
					break;
				}
				num6 = genRand.Next(data.dungeonBounds.Left, data.dungeonBounds.Right);
				num7 = genRand.Next((int)Main.worldSurface, data.dungeonBounds.Bottom);
			}
			if (WorldGen.InWorld(num6, num7, 5) && Main.tile[num6, num7] != null)
			{
				DungeonGenerationStyleData styleForWall = DungeonGenerationStyles.GetStyleForWall(data.genVars.dungeonGenerationStyles, Main.tile[num6, num7].wall);
				if (styleForWall != null && styleForWall.Style == 10 && lihzahrdPaintingsPlaced >= lihzahrdPaintingsMax)
				{
					i--;
					continue;
				}
			}
			int num9 = num6;
			int num10 = num6;
			int num11 = num7;
			int num12 = num7;
			int num13 = 0;
			int num14 = 0;
			for (int j = 0; j < 2; j++)
			{
				num9 = num6;
				num10 = num6;
				while (num9 > 20 && !Main.tile[num9, num7].active() && DungeonUtils.IsConsideredDungeonWall(Main.tile[num9, num7].wall))
				{
					num9--;
				}
				num9++;
				for (; num10 < Main.maxTilesX - 20 && !Main.tile[num10, num7].active() && DungeonUtils.IsConsideredDungeonWall(Main.tile[num10, num7].wall); num10++)
				{
				}
				num10--;
				num6 = (num9 + num10) / 2;
				num11 = num7;
				num12 = num7;
				while (num11 > 20 && !Main.tile[num6, num11].active() && DungeonUtils.IsConsideredDungeonWall(Main.tile[num6, num11].wall))
				{
					num11--;
				}
				num11++;
				for (; num12 < Main.maxTilesY - 20 && !Main.tile[num6, num12].active() && DungeonUtils.IsConsideredDungeonWall(Main.tile[num6, num12].wall); num12++)
				{
				}
				num12--;
				num7 = (num11 + num12) / 2;
			}
			num9 = num6;
			num10 = num6;
			while (num9 > 20 && !Main.tile[num9, num7].active() && !Main.tile[num9, num7 - 1].active() && !Main.tile[num9, num7 + 1].active())
			{
				num9--;
			}
			num9++;
			for (; num10 < Main.maxTilesX - 20 && !Main.tile[num10, num7].active() && !Main.tile[num10, num7 - 1].active() && !Main.tile[num10, num7 + 1].active(); num10++)
			{
			}
			num10--;
			num11 = num7;
			num12 = num7;
			while (num11 > 20 && !Main.tile[num6, num11].active() && !Main.tile[num6 - 1, num11].active() && !Main.tile[num6 + 1, num11].active())
			{
				num11--;
			}
			num11++;
			for (; num12 < Main.maxTilesY - 20 && !Main.tile[num6, num12].active() && !Main.tile[num6 - 1, num12].active() && !Main.tile[num6 + 1, num12].active(); num12++)
			{
			}
			num12--;
			num6 = (num9 + num10) / 2;
			num7 = (num11 + num12) / 2;
			num13 = num10 - num9;
			num14 = num12 - num11;
			if (num13 <= 7 || num14 <= 5)
			{
				continue;
			}
			bool[] array = new bool[3] { true, false, false };
			if (num13 > num14 * 3 && num13 > 21)
			{
				array[1] = true;
			}
			if (num14 > num13 * 3 && num14 > 21)
			{
				array[2] = true;
			}
			int num15 = genRand.Next(3);
			if (Main.tile[num6, num7].wall == num2)
			{
				num15 = 0;
			}
			while (!array[num15])
			{
				num15 = genRand.Next(3);
			}
			if (WorldGen.nearPicture2(num6, num7))
			{
				num15 = -1;
			}
			switch (num15)
			{
			case 0:
			{
				PaintingEntry entry3 = Paintings_GetPaintingEntry(data, Main.tile[num6, num7].wall);
				new DungeonBounds();
				if (data.CanGenerateFeatureInArea(this, num6, num7, 3) && !WorldGen.nearPicture(num6, num7))
				{
					Paintings_PlacePainting(num6, num7, entry3);
				}
				break;
			}
			case 1:
			{
				PaintingEntry entry2 = Paintings_GetPaintingEntry(data, Main.tile[num6, num7].wall);
				if (!data.CanGenerateFeatureInArea(this, num6, num7, 3))
				{
					break;
				}
				if (!Main.tile[num6, num7].active())
				{
					Paintings_PlacePainting(num6, num7, entry2);
				}
				if (Main.tile[num6, num7].active())
				{
					break;
				}
				int num19 = num6;
				int num20 = num7;
				int num21 = num7;
				for (int m = 0; m < 2; m++)
				{
					num6 += 7;
					num11 = num21;
					num12 = num21;
					while (num11 > 0 && !Main.tile[num6, num11].active() && !Main.tile[num6 - 1, num11].active() && !Main.tile[num6 + 1, num11].active())
					{
						num11--;
					}
					num11++;
					for (; num12 < Main.maxTilesY - 1 && !Main.tile[num6, num12].active() && !Main.tile[num6 - 1, num12].active() && !Main.tile[num6 + 1, num12].active(); num12++)
					{
					}
					num12--;
					num21 = (num11 + num12) / 2;
					if (data.CanGenerateFeatureInArea(this, num6, num21, 3))
					{
						entry2 = Paintings_GetPaintingEntry(data, Main.tile[num6, num21].wall);
						if (Math.Abs(num20 - num21) >= 4 || WorldGen.nearPicture(num6, num21))
						{
							break;
						}
						Paintings_PlacePainting(num6, num21, entry2);
					}
				}
				num21 = num7;
				num6 = num19;
				for (int n = 0; n < 2; n++)
				{
					num6 -= 7;
					num11 = num21;
					num12 = num21;
					while (num11 > 0 && !Main.tile[num6, num11].active() && !Main.tile[num6 - 1, num11].active() && !Main.tile[num6 + 1, num11].active())
					{
						num11--;
					}
					num11++;
					for (; num12 < Main.maxTilesY - 1 && !Main.tile[num6, num12].active() && !Main.tile[num6 - 1, num12].active() && !Main.tile[num6 + 1, num12].active(); num12++)
					{
					}
					num12--;
					num21 = (num11 + num12) / 2;
					if (data.CanGenerateFeatureInArea(this, num6, num21, 3))
					{
						entry2 = Paintings_GetPaintingEntry(data, Main.tile[num6, num21].wall);
						if (Math.Abs(num20 - num21) >= 4 || WorldGen.nearPicture(num6, num21))
						{
							break;
						}
						Paintings_PlacePainting(num6, num21, entry2);
					}
				}
				break;
			}
			case 2:
			{
				PaintingEntry entry = Paintings_GetPaintingEntry(data, Main.tile[num6, num7].wall);
				if (!data.CanGenerateFeatureInArea(this, num6, num7, 3))
				{
					break;
				}
				if (!Main.tile[num6, num7].active())
				{
					Paintings_PlacePainting(num6, num7, entry);
				}
				if (Main.tile[num6, num7].active())
				{
					break;
				}
				int num16 = num7;
				int num17 = num6;
				int num18 = num6;
				for (int k = 0; k < 3; k++)
				{
					num7 += 7;
					num9 = num18;
					num10 = num18;
					while (num9 > 0 && !Main.tile[num9, num7].active() && !Main.tile[num9, num7 - 1].active() && !Main.tile[num9, num7 + 1].active())
					{
						num9--;
					}
					num9++;
					for (; num10 < Main.maxTilesX - 1 && !Main.tile[num10, num7].active() && !Main.tile[num10, num7 - 1].active() && !Main.tile[num10, num7 + 1].active(); num10++)
					{
					}
					num10--;
					num18 = (num9 + num10) / 2;
					if (data.CanGenerateFeatureInArea(this, num18, num7, 3))
					{
						entry = Paintings_GetPaintingEntry(data, Main.tile[num18, num7].wall);
						if (Math.Abs(num17 - num18) >= 4 || WorldGen.nearPicture(num18, num7))
						{
							break;
						}
						Paintings_PlacePainting(num18, num7, entry);
					}
				}
				num18 = num6;
				num7 = num16;
				for (int l = 0; l < 3; l++)
				{
					num7 -= 7;
					num9 = num18;
					num10 = num18;
					while (num9 > 0 && !Main.tile[num9, num7].active() && !Main.tile[num9, num7 - 1].active() && !Main.tile[num9, num7 + 1].active())
					{
						num9--;
					}
					num9++;
					for (; num10 < Main.maxTilesX - 1 && !Main.tile[num10, num7].active() && !Main.tile[num10, num7 - 1].active() && !Main.tile[num10, num7 + 1].active(); num10++)
					{
					}
					num10--;
					num18 = (num9 + num10) / 2;
					if (data.CanGenerateFeatureInArea(this, num18, num7, 3))
					{
						entry = Paintings_GetPaintingEntry(data, Main.tile[num18, num7].wall);
						if (Math.Abs(num17 - num18) >= 4 || WorldGen.nearPicture(num18, num7))
						{
							break;
						}
						Paintings_PlacePainting(num18, num7, entry);
					}
				}
				break;
			}
			}
		}
	}

	private static void Paintings_PlacePainting(int x, int y, PaintingEntry entry)
	{
		WorldGen.PlaceTile(x, y, entry.tileType, mute: true, forced: false, -1, entry.style);
		if (Main.tile[x, y].wall == 87)
		{
			lihzahrdPaintingsPlaced++;
		}
	}

	private static PaintingEntry Paintings_GetPaintingEntry(DungeonData data, int currentWall)
	{
		int num = data.wallVariants[0];
		switch ((int)(DungeonGenerationStyles.GetStyleForWall(data.genVars.dungeonGenerationStyles, currentWall)?.Style ?? 0))
		{
		case 3:
			return WorldGen.RandHousePictureDesert();
		case 4:
		case 5:
			return Paintings_RandomBonePainting();
		case 10:
		{
			PlacementDetails placementDetails = ItemID.Sets.DerivedPlacementDetails[5230];
			return new PaintingEntry
			{
				tileType = placementDetails.tileType,
				style = placementDetails.tileStyle
			};
		}
		case 0:
			if (currentWall != num)
			{
				return Paintings_RandomBonePainting();
			}
			return Paintings_RandomDungeonPainting();
		default:
			return WorldGen.RandHousePicture();
		}
	}

	private static PaintingEntry Paintings_RandomDungeonPainting()
	{
		UnifiedRandom genRand = WorldGen.genRand;
		int num = genRand.Next(3);
		int num2 = 0;
		if (num <= 1)
		{
			int maxValue = 7;
			num = 240;
			num2 = genRand.Next(maxValue);
			if (num2 == 6)
			{
				num2 = genRand.Next(maxValue);
			}
			switch (num2)
			{
			case 0:
				num2 = 12;
				break;
			case 1:
				num2 = 13;
				break;
			case 2:
				num2 = 14;
				break;
			case 3:
				num2 = 15;
				break;
			case 4:
				num2 = 18;
				break;
			case 5:
				num2 = 19;
				break;
			case 6:
				num2 = 23;
				break;
			}
		}
		else if (num == 2)
		{
			num = 242;
			int maxValue2 = 17;
			num2 = genRand.Next(maxValue2);
			switch (num2)
			{
			case 14:
				num2 = 15;
				break;
			case 15:
				num2 = 16;
				break;
			case 16:
				num2 = 30;
				break;
			}
		}
		return new PaintingEntry
		{
			tileType = num,
			style = num2
		};
	}

	private static PaintingEntry Paintings_RandomBonePainting()
	{
		UnifiedRandom genRand = WorldGen.genRand;
		int num = genRand.Next(2);
		int num2 = 0;
		switch (num)
		{
		case 0:
			num = 240;
			num2 = genRand.Next(2);
			switch (num2)
			{
			case 0:
				num2 = 16;
				break;
			case 1:
				num2 = 17;
				break;
			}
			break;
		case 1:
			num = 241;
			num2 = genRand.Next(9);
			break;
		}
		return new PaintingEntry
		{
			tileType = num,
			style = num2
		};
	}
}
