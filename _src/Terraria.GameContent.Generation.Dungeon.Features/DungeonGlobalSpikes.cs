using System;
using Terraria.DataStructures;
using Terraria.Utilities;

namespace Terraria.GameContent.Generation.Dungeon.Features;

public class DungeonGlobalSpikes : GlobalDungeonFeature
{
	public DungeonGlobalSpikes(DungeonFeatureSettings settings)
		: base(settings)
	{
		DungeonCrawler.CurrentDungeonData.dungeonFeatures.Add(this);
	}

	public override bool GenerateFeature(DungeonData data)
	{
		generated = false;
		Spikes(data);
		generated = true;
		return true;
	}

	public void Spikes(DungeonData data)
	{
		UnifiedRandom genRand = WorldGen.genRand;
		int num = data.wallVariants[0];
		float num2 = (float)Main.maxTilesX / 4200f;
		int num3 = 0;
		int num4 = 1000;
		int num5 = 0;
		double num6 = Math.Max(1.0, data.globalFeatureScalar * 0.25);
		int num7 = (int)((double)(42f * num2) * num6);
		if (WorldGen.getGoodWorldGen)
		{
			num7 *= 3;
		}
		while (num5 < num7)
		{
			num3++;
			int num8 = genRand.Next(data.dungeonBounds.Left, data.dungeonBounds.Right);
			int i = genRand.Next((int)Main.worldSurface + 25, data.dungeonBounds.Bottom);
			if (WorldGen.drunkWorldGen || WorldGen.SecretSeed.noSurface.Enabled)
			{
				i = genRand.Next(data.genVars.generatingDungeonPositionY + 25, data.dungeonBounds.Bottom);
			}
			int num9 = num8;
			ushort type = 48;
			bool flag = true;
			bool flag2 = Main.tile[num8, i].wall == num;
			if (data.Type == DungeonType.DualDungeon)
			{
				flag2 = DungeonUtils.IsConsideredDungeonWall(Main.tile[num8, i].wall);
				if (Main.tile[num8, i].wall == 87)
				{
					type = 232;
					flag = false;
				}
			}
			if (flag2 && !Main.tile[num8, i].active())
			{
				int num10 = 1;
				if (genRand.Next(2) == 0)
				{
					num10 = -1;
				}
				for (; !Main.tile[num8, i].active(); i += num10)
				{
				}
				if (Main.tile[num8 - 1, i].active() && Main.tile[num8 + 1, i].active() && Spikes_CanSupportSpike(num8 - 1, i) && !Main.tile[num8 - 1, i - num10].active() && !Main.tile[num8 + 1, i - num10].active())
				{
					num5++;
					int num11 = genRand.Next(5, 13);
					while (Main.tile[num8 - 1, i].active() && Spikes_CanSupportSpike(num8 - 1, i) && Main.tile[num8, i + num10].active() && Main.tile[num8, i].active() && !Main.tile[num8, i - num10].active() && num11 > 0)
					{
						if (!data.CanGenerateFeatureAt(this, num8, i) || !data.CanGenerateFeatureAt(this, num8, i - num10))
						{
							num8--;
							num11 = 0;
							continue;
						}
						Main.tile[num8, i].type = type;
						if (!Main.tile[num8 - 1, i - num10].active() && !Main.tile[num8 + 1, i - num10].active())
						{
							Main.tile[num8, i - num10].Clear(TileDataType.Slope);
							Main.tile[num8, i - num10].type = type;
							Main.tile[num8, i - num10].active(active: true);
							if (flag)
							{
								Main.tile[num8, i - num10 * 2].Clear(TileDataType.Slope);
								Main.tile[num8, i - num10 * 2].type = type;
								Main.tile[num8, i - num10 * 2].active(active: true);
							}
						}
						num8--;
						num11--;
					}
					num11 = genRand.Next(5, 13);
					num8 = num9 + 1;
					while (Main.tile[num8 + 1, i].active() && Spikes_CanSupportSpike(num8 + 1, i) && Main.tile[num8, i + num10].active() && Main.tile[num8, i].active() && !Main.tile[num8, i - num10].active() && num11 > 0)
					{
						if (!data.CanGenerateFeatureAt(this, num8, i) || !data.CanGenerateFeatureAt(this, num8, i - num10))
						{
							num8++;
							num11 = 0;
							continue;
						}
						Main.tile[num8, i].type = type;
						if (!Main.tile[num8 - 1, i - num10].active() && !Main.tile[num8 + 1, i - num10].active())
						{
							Main.tile[num8, i - num10].Clear(TileDataType.Slope);
							Main.tile[num8, i - num10].type = type;
							Main.tile[num8, i - num10].active(active: true);
							if (flag)
							{
								Main.tile[num8, i - num10 * 2].Clear(TileDataType.Slope);
								Main.tile[num8, i - num10 * 2].type = type;
								Main.tile[num8, i - num10 * 2].active(active: true);
							}
						}
						num8++;
						num11--;
					}
				}
			}
			if (num3 > num4)
			{
				num3 = 0;
				num5++;
			}
		}
		num3 = 0;
		num4 = 1000;
		num5 = 0;
		while (num5 < num7)
		{
			num3++;
			int j = genRand.Next(data.dungeonBounds.Left, data.dungeonBounds.Right);
			int num12 = genRand.Next((int)Main.worldSurface + 25, data.dungeonBounds.Bottom);
			if (WorldGen.SecretSeed.noSurface.Enabled)
			{
				num12 = genRand.Next(data.genVars.generatingDungeonPositionY + 25, data.dungeonBounds.Bottom);
			}
			int num13 = num12;
			ushort type2 = 48;
			bool flag3 = true;
			bool flag4 = Main.tile[j, num12].wall == num;
			if (data.Type == DungeonType.DualDungeon)
			{
				flag4 = DungeonUtils.IsConsideredDungeonWall(Main.tile[j, num12].wall);
				if (Main.tile[j, num12].wall == 87)
				{
					type2 = 232;
					flag3 = false;
				}
			}
			if (flag4 && !Main.tile[j, num12].active())
			{
				int num14 = 1;
				if (genRand.Next(2) == 0)
				{
					num14 = -1;
				}
				for (; j > 5 && j < Main.maxTilesX - 5 && !Main.tile[j, num12].active(); j += num14)
				{
				}
				if (Main.tile[j, num12 - 1].active() && Main.tile[j, num12 + 1].active() && Spikes_CanSupportSpike(j, num12 - 1) && !Main.tile[j - num14, num12 - 1].active() && !Main.tile[j - num14, num12 + 1].active())
				{
					num5++;
					int num15 = genRand.Next(5, 13);
					while (Main.tile[j, num12 - 1].active() && Spikes_CanSupportSpike(j, num12 - 1) && Main.tile[j + num14, num12].active() && Main.tile[j, num12].active() && !Main.tile[j - num14, num12].active() && num15 > 0)
					{
						if (!data.CanGenerateFeatureAt(this, j, num12) || !data.CanGenerateFeatureAt(this, j - num14, num12))
						{
							num12--;
							num15 = 0;
							continue;
						}
						Main.tile[j, num12].type = type2;
						if (!Main.tile[j - num14, num12 - 1].active() && !Main.tile[j - num14, num12 + 1].active())
						{
							Main.tile[j - num14, num12].type = type2;
							Main.tile[j - num14, num12].active(active: true);
							Main.tile[j - num14, num12].Clear(TileDataType.Slope);
							if (flag3)
							{
								Main.tile[j - num14 * 2, num12].type = type2;
								Main.tile[j - num14 * 2, num12].active(active: true);
								Main.tile[j - num14 * 2, num12].Clear(TileDataType.Slope);
							}
						}
						num12--;
						num15--;
					}
					num15 = genRand.Next(5, 13);
					num12 = num13 + 1;
					while (Main.tile[j, num12 + 1].active() && Spikes_CanSupportSpike(j, num12 + 1) && Main.tile[j + num14, num12].active() && Main.tile[j, num12].active() && !Main.tile[j - num14, num12].active() && num15 > 0)
					{
						if (!data.CanGenerateFeatureAt(this, j, num12) || !data.CanGenerateFeatureAt(this, j - num14, num12))
						{
							num12++;
							num15 = 0;
							continue;
						}
						Main.tile[j, num12].type = type2;
						if (!Main.tile[j - num14, num12 - 1].active() && !Main.tile[j - num14, num12 + 1].active())
						{
							Main.tile[j - num14, num12].type = type2;
							Main.tile[j - num14, num12].active(active: true);
							Main.tile[j - num14, num12].Clear(TileDataType.Slope);
							if (flag3)
							{
								Main.tile[j - num14 * 2, num12].type = type2;
								Main.tile[j - num14 * 2, num12].active(active: true);
								Main.tile[j - num14 * 2, num12].Clear(TileDataType.Slope);
							}
						}
						num12++;
						num15--;
					}
				}
			}
			if (num3 > num4)
			{
				num3 = 0;
				num5++;
			}
		}
	}

	private bool Spikes_CanSupportSpike(int x, int y)
	{
		Tile tile = Main.tile[x, y];
		if (!tile.active())
		{
			return false;
		}
		if (tile.type >= 0 && (Main.tileFrameImportant[tile.type] || Main.tileCut[tile.type]))
		{
			return false;
		}
		if (DungeonUtils.IsConsideredCrackedDungeonTile(tile.type))
		{
			return false;
		}
		return true;
	}
}
