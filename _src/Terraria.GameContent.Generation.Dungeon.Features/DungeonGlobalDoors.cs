using System;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Utilities;

namespace Terraria.GameContent.Generation.Dungeon.Features;

public class DungeonGlobalDoors : GlobalDungeonFeature
{
	public DungeonGlobalDoors(DungeonFeatureSettings settings)
		: base(settings)
	{
		DungeonCrawler.CurrentDungeonData.dungeonFeatures.Add(this);
	}

	public override bool GenerateFeature(DungeonData data)
	{
		generated = false;
		Doors(data);
		generated = true;
		return true;
	}

	public void Doors(DungeonData data)
	{
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_013d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0164: Unknown result type (might be due to invalid IL or missing references)
		//IL_019f: Unknown result type (might be due to invalid IL or missing references)
		//IL_038b: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f2: Unknown result type (might be due to invalid IL or missing references)
		UnifiedRandom genRand = WorldGen.genRand;
		ushort brickTileType = data.genVars.dungeonStyle.BrickTileType;
		ushort brickWallType = data.genVars.dungeonStyle.BrickWallType;
		byte? tilePaintColor = data.genVars.dungeonStyle.TilePaintColor;
		byte? wallPaintColor = data.genVars.dungeonStyle.WallPaintColor;
		PlacementDetails placementDetails = ItemID.Sets.DerivedPlacementDetails[data.doorItemType];
		for (int i = 0; i < data.dungeonDoorData.Count; i++)
		{
			DungeonDoorData dungeonDoorData = data.dungeonDoorData[i];
			if (!WorldGen.InWorld(dungeonDoorData.Position, 30))
			{
				continue;
			}
			ushort num = brickTileType;
			ushort wall = brickWallType;
			byte? b = tilePaintColor;
			byte? b2 = wallPaintColor;
			if (dungeonDoorData.OverrideGenerationStyle != null)
			{
				num = dungeonDoorData.OverrideGenerationStyle.BrickTileType;
				wall = dungeonDoorData.OverrideGenerationStyle.BrickWallType;
				b = dungeonDoorData.OverrideGenerationStyle.TilePaintColor;
				b2 = dungeonDoorData.OverrideGenerationStyle.WallPaintColor;
			}
			int style = 13;
			if (genRand.Next(3) == 0)
			{
				style = placementDetails.tileStyle;
			}
			if (dungeonDoorData.OverrideStyle.HasValue)
			{
				style = dungeonDoorData.OverrideStyle.Value;
			}
			int num2 = 20;
			int num3 = num2 + 5;
			int num4 = 10;
			if (dungeonDoorData.OverrideWidthFluff.HasValue)
			{
				num4 = dungeonDoorData.OverrideWidthFluff.Value;
			}
			int num5 = Math.Max(num3, Math.Min(Main.maxTilesX - num3, dungeonDoorData.Position.X - num4));
			int num6 = Math.Max(num3, Math.Min(Main.maxTilesX - num3, Math.Max(num5, dungeonDoorData.Position.X + num4 - 1)));
			int num7 = 100;
			int num8 = 0;
			int num9 = 0;
			int num10 = 0;
			for (int j = num5; j <= num6; j++)
			{
				bool flag = true;
				int num11 = dungeonDoorData.Position.Y;
				while (num11 > 10 && !Main.tile[j, num11].active())
				{
					num11--;
				}
				if (!DungeonUtils.IsConsideredDungeonTile(Main.tile[j, num11].type))
				{
					flag = false;
				}
				num9 = num11;
				for (num11 = dungeonDoorData.Position.Y; !Main.tile[j, num11].active(); num11++)
				{
				}
				if (!DungeonUtils.IsConsideredDungeonTile(Main.tile[j, num11].type))
				{
					flag = false;
				}
				num10 = num11;
				if (num10 - num9 < 3)
				{
					continue;
				}
				if (!dungeonDoorData.SkipOtherDoorsCheck)
				{
					int num12 = j - 20;
					int num13 = j + 20;
					int num14 = num10 - 10;
					int num15 = num10 + 10;
					for (int k = num12; k < num13; k++)
					{
						for (int l = num14; l < num15; l++)
						{
							if (Main.tile[k, l].active() && Main.tile[k, l].type == 10)
							{
								flag = false;
								break;
							}
						}
					}
				}
				if (flag && !dungeonDoorData.SkipSpaceCheck)
				{
					for (int m = num10 - 3; m < num10; m++)
					{
						for (int n = j - 3; n <= j + 3; n++)
						{
							if (Main.tile[n, m].active())
							{
								flag = false;
								break;
							}
						}
					}
				}
				if (flag && num10 - num9 < num2)
				{
					bool flag2 = false;
					if (dungeonDoorData.Direction == 0 && num10 - num9 < num7)
					{
						flag2 = true;
					}
					if (dungeonDoorData.Direction == -1 && j > num8)
					{
						flag2 = true;
					}
					if (dungeonDoorData.Direction == 1 && (j < num8 || num8 == 0))
					{
						flag2 = true;
					}
					if (flag2)
					{
						num8 = j;
						num7 = num10 - num9;
					}
				}
			}
			if (num7 >= num2)
			{
				continue;
			}
			int num16 = num8;
			int num17 = dungeonDoorData.Position.Y;
			int num18 = num17;
			for (; !Main.tile[num16, num17].active(); num17++)
			{
				Main.tile[num16, num17].active(active: false);
			}
			while (!Main.tile[num16, num18].active())
			{
				num18--;
			}
			num17--;
			num18++;
			for (int num19 = num18; num19 < num17 - 2; num19++)
			{
				Main.tile[num16, num19].Clear(TileDataType.Slope);
				Main.tile[num16, num19].active(active: true);
				Main.tile[num16, num19].type = num;
				if (b.HasValue)
				{
					Main.tile[num16, num19].color(b.Value);
				}
				if (Main.tile[num16 - 1, num19].active() && WorldGen.CanKillTile(num16 - 1, num19))
				{
					Main.tile[num16 - 1, num19].active(active: false);
					Main.tile[num16 - 1, num19].ClearEverything();
					Main.tile[num16 - 1, num19].wall = wall;
					if (b2.HasValue)
					{
						Main.tile[num16 - 1, num19].wallColor(b2.Value);
					}
				}
				if (Main.tile[num16 - 2, num19].active() && WorldGen.CanKillTile(num16 - 2, num19))
				{
					Main.tile[num16 - 2, num19].active(active: false);
					Main.tile[num16 - 2, num19].ClearEverything();
					Main.tile[num16 - 2, num19].wall = wall;
					if (b2.HasValue)
					{
						Main.tile[num16 - 2, num19].wallColor(b2.Value);
					}
				}
				if (Main.tile[num16 + 1, num19].active() && WorldGen.CanKillTile(num16 + 1, num19))
				{
					Main.tile[num16 + 1, num19].active(active: false);
					Main.tile[num16 + 1, num19].ClearEverything();
					Main.tile[num16 + 1, num19].wall = wall;
					if (b2.HasValue)
					{
						Main.tile[num16 + 1, num19].wallColor(b2.Value);
					}
				}
				if (Main.tile[num16 + 2, num19].active() && WorldGen.CanKillTile(num16 + 2, num19))
				{
					Main.tile[num16 + 2, num19].active(active: false);
					Main.tile[num16 + 2, num19].ClearEverything();
					Main.tile[num16 + 2, num19].wall = wall;
					if (b2.HasValue)
					{
						Main.tile[num16 + 2, num19].wallColor(b2.Value);
					}
				}
			}
			WorldGen.PlaceTile(num16, num17, 10, mute: true, forced: false, -1, style);
			num16--;
			int num20 = num17 - 3;
			while (!Main.tile[num16, num20].active())
			{
				num20--;
			}
			bool flag3 = num17 - num20 < num17 - num18 + 5 && DungeonUtils.IsConsideredDungeonTile(Main.tile[num16, num20].type);
			if (dungeonDoorData.AlwaysClearArea | flag3)
			{
				for (int num21 = num17 - 4 - genRand.Next(3); num21 > num20; num21--)
				{
					if (flag3)
					{
						Main.tile[num16, num21].Clear(TileDataType.Slope);
						Main.tile[num16, num21].active(active: true);
						Main.tile[num16, num21].type = num;
						if (b.HasValue)
						{
							Main.tile[num16, num21].color(b.Value);
						}
					}
					if (dungeonDoorData.AlwaysClearArea || Main.tile[num16 - 1, num21].type == num)
					{
						Main.tile[num16 - 1, num21].active(active: false);
						Main.tile[num16 - 1, num21].ClearEverything();
						Main.tile[num16 - 1, num21].wall = wall;
						if (b2.HasValue)
						{
							Main.tile[num16 - 1, num21].wallColor(b2.Value);
						}
					}
					if (dungeonDoorData.AlwaysClearArea || Main.tile[num16 - 2, num21].type == num)
					{
						Main.tile[num16 - 2, num21].active(active: false);
						Main.tile[num16 - 2, num21].ClearEverything();
						Main.tile[num16 - 2, num21].wall = wall;
						if (b2.HasValue)
						{
							Main.tile[num16 - 2, num21].wallColor(b2.Value);
						}
					}
				}
			}
			num16 += 2;
			num20 = num17 - 3;
			while (!Main.tile[num16, num20].active())
			{
				num20--;
			}
			flag3 = num17 - num20 < num17 - num18 + 5 && DungeonUtils.IsConsideredDungeonTile(Main.tile[num16, num20].type);
			if (dungeonDoorData.AlwaysClearArea | flag3)
			{
				for (int num22 = num17 - 4 - genRand.Next(3); num22 > num20; num22--)
				{
					if (flag3)
					{
						Main.tile[num16, num22].active(active: true);
						Main.tile[num16, num22].Clear(TileDataType.Slope);
						Main.tile[num16, num22].type = num;
						if (b.HasValue)
						{
							Main.tile[num16, num22].wallColor(b.Value);
						}
					}
					if (dungeonDoorData.AlwaysClearArea || Main.tile[num16 + 1, num22].type == num)
					{
						Main.tile[num16 + 1, num22].active(active: false);
						Main.tile[num16 + 1, num22].ClearEverything();
						Main.tile[num16 + 1, num22].wall = wall;
						if (b2.HasValue)
						{
							Main.tile[num16 + 1, num22].wallColor(b2.Value);
						}
					}
					if (dungeonDoorData.AlwaysClearArea || Main.tile[num16 + 2, num22].type == num)
					{
						Main.tile[num16 + 2, num22].active(active: false);
						Main.tile[num16 + 2, num22].ClearEverything();
						Main.tile[num16 + 2, num22].wall = wall;
						if (b2.HasValue)
						{
							Main.tile[num16 + 2, num22].wallColor(b2.Value);
						}
					}
				}
			}
			num17++;
			num16--;
			for (int num23 = num17 - 8; num23 < num17; num23++)
			{
				if (dungeonDoorData.AlwaysClearArea || Main.tile[num16 + 2, num23].type == num)
				{
					Main.tile[num16 + 2, num23].active(active: false);
					Main.tile[num16 + 2, num23].ClearEverything();
					Main.tile[num16 + 2, num23].wall = wall;
					if (b2.HasValue)
					{
						Main.tile[num16 + 2, num23].wallColor(b2.Value);
					}
				}
				if (dungeonDoorData.AlwaysClearArea || Main.tile[num16 + 3, num23].type == num)
				{
					Main.tile[num16 + 3, num23].active(active: false);
					Main.tile[num16 + 3, num23].ClearEverything();
					Main.tile[num16 + 3, num23].wall = wall;
					if (b2.HasValue)
					{
						Main.tile[num16 + 3, num23].wallColor(b2.Value);
					}
				}
				if (dungeonDoorData.AlwaysClearArea || Main.tile[num16 - 2, num23].type == num)
				{
					Main.tile[num16 - 2, num23].active(active: false);
					Main.tile[num16 - 2, num23].ClearEverything();
					Main.tile[num16 - 2, num23].wall = wall;
					if (b2.HasValue)
					{
						Main.tile[num16 - 2, num23].wallColor(b2.Value);
					}
				}
				if (dungeonDoorData.AlwaysClearArea || Main.tile[num16 - 3, num23].type == num)
				{
					Main.tile[num16 - 3, num23].active(active: false);
					Main.tile[num16 - 3, num23].ClearEverything();
					Main.tile[num16 - 3, num23].wall = wall;
					if (b2.HasValue)
					{
						Main.tile[num16 - 3, num23].wallColor(b2.Value);
					}
				}
			}
			Main.tile[num16 - 1, num17].active(active: true);
			Main.tile[num16 - 1, num17].type = num;
			Main.tile[num16 - 1, num17].Clear(TileDataType.Slope);
			if (b.HasValue)
			{
				Main.tile[num16 - 1, num17].color(b.Value);
			}
			Main.tile[num16 + 1, num17].active(active: true);
			Main.tile[num16 + 1, num17].type = num;
			Main.tile[num16 + 1, num17].Clear(TileDataType.Slope);
			if (b.HasValue)
			{
				Main.tile[num16 + 1, num17].color(b.Value);
			}
		}
	}
}
