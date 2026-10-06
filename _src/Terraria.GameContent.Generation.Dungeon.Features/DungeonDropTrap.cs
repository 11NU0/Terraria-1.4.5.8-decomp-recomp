using Terraria.ID;
using Terraria.Utilities;

namespace Terraria.GameContent.Generation.Dungeon.Features;

public class DungeonDropTrap : DungeonFeature
{
	public DungeonDropTrap(DungeonFeatureSettings settings, bool addToFeatures = true)
		: base(settings)
	{
		if (addToFeatures)
		{
			DungeonCrawler.CurrentDungeonData.dungeonFeatures.Add(this);
		}
	}

	public override bool GenerateFeature(DungeonData data, int x, int y)
	{
		generated = false;
		if (DropTrap(data, x, y))
		{
			generated = true;
			return true;
		}
		return false;
	}

	public override bool CanGenerateFeatureAt(DungeonData data, IDungeonFeature feature, int x, int y)
	{
		return false;
	}

	public bool DropTrap(DungeonData data, int i, int j)
	{
		//IL_0398: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b5f: Unknown result type (might be due to invalid IL or missing references)
		UnifiedRandom genRand = WorldGen.genRand;
		DungeonDropTrapSettings dungeonDropTrapSettings = (DungeonDropTrapSettings)settings;
		bool flag = dungeonDropTrapSettings.StyleData.Style == 0;
		ushort num = (flag ? data.genVars.dungeonStyle.BrickTileType : dungeonDropTrapSettings.StyleData.BrickTileType);
		ushort num2 = (flag ? data.genVars.dungeonStyle.BrickWallType : dungeonDropTrapSettings.StyleData.BrickWallType);
		byte? b = (flag ? data.genVars.dungeonStyle.TilePaintColor : dungeonDropTrapSettings.StyleData.TilePaintColor);
		if (!flag)
		{
			_ = dungeonDropTrapSettings.StyleData.WallPaintColor;
		}
		else
		{
			_ = data.genVars.dungeonStyle.WallPaintColor;
		}
		ushort num3 = num;
		int num4 = -1;
		int num5 = -1;
		switch (dungeonDropTrapSettings.DropTrapType)
		{
		default:
			num4 = 53;
			num3 = 396;
			break;
		case DungeonDropTrapType.Silt:
			num4 = 123;
			break;
		case DungeonDropTrapType.Slush:
			num4 = 224;
			num3 = 147;
			break;
		case DungeonDropTrapType.Lava:
			num5 = 1;
			break;
		}
		int num6 = 6;
		int num7 = 4;
		int num8 = 25;
		int k = j;
		if (!WorldGen.InWorld(i, k, num8))
		{
			return false;
		}
		for (; !Main.tile[i, k].active() && k < Main.UnderworldLayer; k++)
		{
		}
		if (!WorldGen.InWorld(i, k, num8))
		{
			return false;
		}
		if (Main.tile[i, k - 1].active())
		{
			return false;
		}
		if (!Main.tileSolid[Main.tile[i, k].type] || Main.tile[i, k].halfBrick() || Main.tile[i, k].topSlope())
		{
			return false;
		}
		if ((Main.tile[i, k].type != num4 && Main.tile[i, k].type != num3 && Main.tile[i, k].type != num) || Main.tile[i, k].wall != num2)
		{
			return false;
		}
		k--;
		_ = Main.tile[i, k];
		int num9 = -1;
		int num10 = genRand.Next(6, 12);
		int num11 = genRand.Next(6, 14);
		for (int l = i - num8; l <= i + num8; l++)
		{
			for (int m = k - num8; m < k + num8; m++)
			{
				Tile tile = Main.tile[l, m];
				if (tile.wire())
				{
					return false;
				}
				if (TileID.Sets.BasicChest[tile.type])
				{
					return false;
				}
			}
		}
		for (int num12 = k; num12 > k - 30; num12--)
		{
			if (Main.tile[i, num12].active())
			{
				if (Main.tile[i, num12].type == num)
				{
					num9 = num12;
					break;
				}
				return false;
			}
		}
		if (num9 <= -1)
		{
			return false;
		}
		if (k - num9 < num11 + num7)
		{
			return false;
		}
		int num13 = 0;
		_ = (k + num9) / 2;
		for (int n = i - num10; n <= i + num10; n++)
		{
			for (int num14 = num9 - num11; num14 <= num9; num14++)
			{
				Tile tile2 = Main.tile[n, num14];
				if (tile2.active() && Main.tileSolid[tile2.type])
				{
					num13++;
				}
			}
		}
		double num15 = (double)((num10 * 2 + 1) * (num11 + 1)) * 0.75;
		if ((double)num13 < num15)
		{
			return false;
		}
		Bounds.SetBounds(i - num10 - 1, num9 - num11, i + num10 + 1, num9 + 20);
		Bounds.CalculateHitbox();
		if (!data.CanGenerateFeatureInArea(this, Bounds))
		{
			return false;
		}
		Bounds = new DungeonBounds();
		for (int num16 = i - num10 - 1; num16 <= i + num10 + 1; num16++)
		{
			for (int num17 = num9 - num11; num17 <= num9; num17++)
			{
				Tile tile3 = Main.tile[num16, num17];
				bool flag2 = false;
				if (tile3.active() && Main.tileSolid[tile3.type])
				{
					flag2 = true;
				}
				if (num17 == num9)
				{
					tile3.slope(0);
					tile3.halfBrick(halfBrick: false);
					if (!flag2)
					{
						tile3.active(active: true);
						tile3.type = num;
						if (b.HasValue)
						{
							tile3.color(b.Value);
						}
					}
					continue;
				}
				if (num17 == num9 - num11)
				{
					tile3.ClearTile();
					tile3.active(active: true);
					if (flag2 && Main.tile[num16, num17 - 1].active() && Main.tileSolid[Main.tile[num16, num17 - 1].type])
					{
						tile3.type = num3;
					}
					else
					{
						tile3.type = num;
					}
					if (b.HasValue)
					{
						tile3.color(b.Value);
					}
					continue;
				}
				if (num16 == i - num10 - 1 || num16 == i + num10 + 1)
				{
					if (!flag2)
					{
						tile3.ClearTile();
						tile3.active(active: true);
						tile3.type = num;
						if (b.HasValue)
						{
							tile3.color(b.Value);
						}
					}
					else
					{
						tile3.slope(0);
						tile3.halfBrick(halfBrick: false);
					}
					continue;
				}
				tile3.ClearTile();
				if (num5 > -1)
				{
					tile3.liquid = byte.MaxValue;
					tile3.liquidType(num5);
					continue;
				}
				tile3.active(active: true);
				tile3.type = (ushort)num4;
				if (b.HasValue)
				{
					tile3.color(b.Value);
				}
			}
		}
		for (int num18 = (int)((double)num9 - (double)num11 * 0.6600000262260437); (double)num18 <= (double)num9 - (double)num11 * 0.33000001311302185; num18++)
		{
			if ((double)num18 < (double)num9 - (double)num11 * 0.4000000059604645)
			{
				if (Main.tile[i - num10 - 2, num18].bottomSlope())
				{
					Main.tile[i - num10 - 2, num18].slope(0);
				}
			}
			else if ((double)num18 > (double)num9 - (double)num11 * 0.6000000238418579)
			{
				if (Main.tile[i - num10 - 2, num18].topSlope())
				{
					Main.tile[i - num10 - 2, num18].slope(0);
				}
				Main.tile[i - num10 - 2, num18].halfBrick(halfBrick: false);
			}
			else
			{
				Main.tile[i - num10 - 2, num18].halfBrick(halfBrick: false);
				Main.tile[i - num10 - 2, num18].slope(0);
			}
			if (!Main.tile[i - num10 - 2, num18].active() || !Main.tileSolid[Main.tile[i - num10 - 2, num18].type])
			{
				Main.tile[i - num10 - 2, num18].active(active: true);
				Main.tile[i - num10 - 2, num18].type = num;
				if (b.HasValue)
				{
					Main.tile[i - num10 - 2, num18].color(b.Value);
				}
			}
			if (!Main.tile[i + num10 + 2, num18].active() || !Main.tileSolid[Main.tile[i + num10 + 2, num18].type])
			{
				Main.tile[i + num10 + 2, num18].active(active: true);
				Main.tile[i + num10 + 2, num18].type = num;
				if (b.HasValue)
				{
					Main.tile[i + num10 + 2, num18].color(b.Value);
				}
			}
		}
		for (int num19 = num9 - num11; num19 <= num9; num19++)
		{
			Main.tile[i - num10 - 2, num19].slope(0);
			Main.tile[i - num10 - 2, num19].halfBrick(halfBrick: false);
			Main.tile[i - num10 - 1, num19].slope(0);
			Main.tile[i - num10 - 1, num19].halfBrick(halfBrick: false);
			Main.tile[i - num10 + 1, num19].slope(0);
			Main.tile[i - num10 + 1, num19].halfBrick(halfBrick: false);
			Main.tile[i - num10 + 2, num19].slope(0);
			Main.tile[i - num10 + 2, num19].halfBrick(halfBrick: false);
		}
		for (int num20 = i - num10 - 1; num20 < i + num10 + 1; num20++)
		{
			int num21 = k - num11 - 1;
			if (Main.tile[num20, num21].bottomSlope())
			{
				Main.tile[num20, num21].slope(0);
			}
			Main.tile[num20, num21].halfBrick(halfBrick: false);
		}
		WorldGen.KillTile(i - 2, k);
		WorldGen.KillTile(i - 1, k);
		WorldGen.KillTile(i + 1, k);
		WorldGen.KillTile(i + 2, k);
		WorldGen.PlaceTile(i, k, 135, mute: true, forced: false, -1, 7);
		for (int num22 = i - num10; num22 <= i + num10; num22++)
		{
			int num23 = k;
			if ((float)num22 < (float)i - (float)num10 * 0.8f || (float)num22 > (float)i + (float)num10 * 0.8f)
			{
				num23 = k - 3;
			}
			else if ((float)num22 < (float)i - (float)num10 * 0.6f || (float)num22 > (float)i + (float)num10 * 0.6f)
			{
				num23 = k - 2;
			}
			else if ((float)num22 < (float)i - (float)num10 * 0.4f || (float)num22 > (float)i + (float)num10 * 0.4f)
			{
				num23 = k - 1;
			}
			for (int num24 = num9; num24 <= k; num24++)
			{
				if (num22 == i && num24 <= k)
				{
					Main.tile[i, num24].wire(wire: true);
				}
				if (Main.tile[num22, num24].active() && Main.tileSolid[Main.tile[num22, num24].type])
				{
					if (num24 < num9 + num6 - 4)
					{
						Main.tile[num22, num24].actuator(actuator: true);
						Main.tile[num22, num24].wire(wire: true);
					}
					else if (num24 < num23)
					{
						WorldGen.KillTile(num22, num24);
					}
				}
			}
		}
		int num25 = k;
		for (int num26 = i - num10; num26 <= i + num10; num26++)
		{
			for (int num27 = num25; num27 < Main.UnderworldLayer; num27++)
			{
				if (num27 >= num25)
				{
					Tile tile4 = Main.tile[num26, num27];
					if (tile4.active() && !TileID.Sets.Platforms[tile4.type] && Main.tileSolid[tile4.type])
					{
						num25 = num27;
						break;
					}
				}
			}
		}
		Bounds.SetBounds(i - num10 - 1, num9 - num11, i + num10 + 1, num25);
		Bounds.CalculateHitbox();
		return true;
	}
}
