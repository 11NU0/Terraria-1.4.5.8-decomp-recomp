using System;
using Microsoft.Xna.Framework;

namespace Terraria.GameContent;

public class TeleportHelpers
{
	public static bool FindClosestTeleportSpotNoSpace(Player player, out Vector2 resultPosition)
	{
		//IL_0004: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0158: Unknown result type (might be due to invalid IL or missing references)
		//IL_016d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0182: Unknown result type (might be due to invalid IL or missing references)
		//IL_0197: Unknown result type (might be due to invalid IL or missing references)
		//IL_0199: Unknown result type (might be due to invalid IL or missing references)
		//IL_019a: Unknown result type (might be due to invalid IL or missing references)
		//IL_019f: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b3: Unknown result type (might be due to invalid IL or missing references)
		bool result = false;
		resultPosition = player.position;
		player.velocity = Vector2.Zero;
		Vector2 val = new Vector2((float)player.width * 0.5f, (float)player.height);
		Vector2 bottom = player.Bottom;
		Point val2 = bottom.ToTileCoordinates();
		int value = val2.X - 25;
		int value2 = val2.X + 25;
		int value3 = val2.Y - 25;
		int value4 = val2.Y + 25;
		value = Utils.Clamp(value, 40, Main.maxTilesX - 40);
		value2 = Utils.Clamp(value2, 40, Main.maxTilesX - 40);
		value3 = Utils.Clamp(value3, 40, Main.maxTilesY - 40);
		value4 = Utils.Clamp(value4, 40, Main.maxTilesY - 40);
		float num = float.MaxValue;
		for (int i = value; i < value2; i++)
		{
			for (int j = value3; j < value4; j++)
			{
				Vector2 val3 = new Vector2((float)(i * 16 + 8), (float)(j * 16 + 15)) - val;
				Tile tile = Main.tile[i, j];
				Tile tile2 = Main.tile[i, j + 1];
				bool flag = WorldGen.SolidOrSlopedTile(tile) || tile.liquid > 0;
				bool flag2 = WorldGen.SolidOrSlopedTile(tile2) && tile2.liquid == 0;
				if (((!TileIsDangerous(i, j, player) && !flag) & flag2) && !Collision.LavaCollision(val3, player.width, player.height) && !Collision.AnyHurtingTiles(val3, player.width, player.height) && !Collision.SolidCollision(val3, player.width, player.height))
				{
					Vector2 val4 = val3 - bottom;
					float num2 = val4.Length();
					if (num2 < num)
					{
						resultPosition = val3;
						num = num2;
						result = true;
					}
				}
			}
		}
		return result;
	}

	public static bool RequestMagicConchTeleportPosition(Player player, int crawlOffsetX, bool rightOcean, out Point landingPoint)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0250: Unknown result type (might be due to invalid IL or missing references)
		//IL_025d: Unknown result type (might be due to invalid IL or missing references)
		//IL_026b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0270: Unknown result type (might be due to invalid IL or missing references)
		//IL_0272: Unknown result type (might be due to invalid IL or missing references)
		//IL_0278: Unknown result type (might be due to invalid IL or missing references)
		//IL_012b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0134: Unknown result type (might be due to invalid IL or missing references)
		//IL_0147: Unknown result type (might be due to invalid IL or missing references)
		//IL_0150: Unknown result type (might be due to invalid IL or missing references)
		//IL_0163: Unknown result type (might be due to invalid IL or missing references)
		//IL_016c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0182: Unknown result type (might be due to invalid IL or missing references)
		//IL_018b: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_030c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0311: Unknown result type (might be due to invalid IL or missing references)
		//IL_0313: Unknown result type (might be due to invalid IL or missing references)
		//IL_0319: Unknown result type (might be due to invalid IL or missing references)
		//IL_0676: Unknown result type (might be due to invalid IL or missing references)
		//IL_0678: Unknown result type (might be due to invalid IL or missing references)
		//IL_0526: Unknown result type (might be due to invalid IL or missing references)
		//IL_0537: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0399: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_033c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0594: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0606: Unknown result type (might be due to invalid IL or missing references)
		//IL_0617: Unknown result type (might be due to invalid IL or missing references)
		//IL_040e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0417: Unknown result type (might be due to invalid IL or missing references)
		//IL_0362: Unknown result type (might be due to invalid IL or missing references)
		//IL_0429: Unknown result type (might be due to invalid IL or missing references)
		//IL_0430: Unknown result type (might be due to invalid IL or missing references)
		//IL_0456: Unknown result type (might be due to invalid IL or missing references)
		//IL_0477: Unknown result type (might be due to invalid IL or missing references)
		//IL_047e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0223: Unknown result type (might be due to invalid IL or missing references)
		landingPoint = default;
		int num = 40;
		int num2 = 0;
		int num3 = (rightOcean ? (Main.maxTilesX - num) : num);
		int num4 = 50;
		int num5 = (int)Main.worldSurface - num2;
		Point val = new Point(num3, num4);
		int num6 = 1;
		int num7 = -1;
		int num8 = 1;
		int num9 = 0;
		int num10 = 5000;
		Vector2 val2 = new Vector2((float)player.width * 0.5f, (float)player.height);
		int num11 = 40;
		bool flag = WorldGen.SolidOrSlopedTile(Main.tile[val.X, val.Y], includePlatforms: true);
		int num12 = 0;
		int num13 = 400;
		if (WorldGen.Skyblock.lowTiles)
		{
			num10 = num13 * ((int)Main.worldSurface - 10);
		}
		while (num9 < num10 && num12 < num13)
		{
			num9++;
			Tile tile = Main.tile[val.X, val.Y];
			Tile tile2 = Main.tile[val.X, val.Y + num8];
			bool flag2 = WorldGen.SolidOrSlopedTile(tile, includePlatforms: true) || tile.liquid > 0;
			bool flag3 = WorldGen.SolidOrSlopedTile(tile2, includePlatforms: true) || tile2.liquid > 0;
			if (!flag2 && !flag3)
			{
				Tile tile3 = Main.tile[val.X - 1, val.Y];
				Tile tile4 = Main.tile[val.X + 1, val.Y];
				Tile tile5 = Main.tile[val.X - 1, val.Y + num8];
				Tile tile6 = Main.tile[val.X + 1, val.Y + num8];
				bool num14 = WorldGen.SolidOrSlopedTile(tile3, includePlatforms: true) || tile3.liquid > 0;
				bool flag4 = WorldGen.SolidOrSlopedTile(tile4, includePlatforms: true) || tile4.liquid > 0;
				bool flag5 = WorldGen.SolidOrSlopedTile(tile5, includePlatforms: true) || tile5.liquid > 0;
				bool flag6 = WorldGen.SolidOrSlopedTile(tile6, includePlatforms: true) || tile6.liquid > 0;
				if (!num14 && !flag4 && !flag5 && !flag6)
				{
					val.Y += num6;
					if (WorldGen.Skyblock.lowTiles && val.Y >= num5)
					{
						val.Y = num4;
						val.X += crawlOffsetX;
						num12++;
					}
					continue;
				}
			}
			if (IsInSolidTilesExtended(new Vector2((float)(val.X * 16 + 8), (float)(val.Y * 16 + 15)) - val2, player.velocity, player.width, player.height, (int)player.gravDir))
			{
				if (flag)
				{
					val.Y += num6;
				}
				else
				{
					val.Y += num7;
				}
				continue;
			}
			if (flag2)
			{
				if (flag)
				{
					val.Y += num6;
				}
				else
				{
					val.Y += num7;
				}
				continue;
			}
			flag = false;
			if (!IsInSolidTilesExtended(new Vector2((float)(val.X * 16 + 8), (float)(val.Y * 16 + 15 + 16)) - val2, player.velocity, player.width, player.height, (int)player.gravDir) && !flag3 && (double)val.Y < Main.worldSurface)
			{
				val.Y += num6;
				if (WorldGen.Skyblock.lowTiles && val.Y >= num5)
				{
					val.Y = num4;
					val.X += crawlOffsetX;
					num12++;
				}
				continue;
			}
			if (tile2.liquid > 0 && !WorldGen.SolidOrSlopedTile(val.X, val.Y + num8, includePlatforms: true))
			{
				val.X += crawlOffsetX;
				num12++;
				continue;
			}
			if (TileIsDangerous(val.X - crawlOffsetX, val.Y, player) || TileIsDangerous(val.X, val.Y, player))
			{
				val.X += crawlOffsetX;
				num12++;
				continue;
			}
			if (TileIsDangerous(val.X - crawlOffsetX, val.Y + num8, player) || TileIsDangerous(val.X, val.Y + num8, player))
			{
				val.X += crawlOffsetX;
				num12++;
				continue;
			}
			if (val.Y < num11)
			{
				val.Y += num6;
				continue;
			}
			if (!WorldGen.Skyblock.lowTiles && !WorldGen.SolidOrSlopedTile(val.X, val.Y + num8, includePlatforms: true))
			{
				val.X += crawlOffsetX;
				num12++;
			}
			break;
		}
		if (num9 == num10 || num12 >= num13)
		{
			return false;
		}
		if (!WorldGen.InWorld(val.X, val.Y, 40))
		{
			return false;
		}
		int num15 = 20;
		if (WorldGen.Skyblock.lowTiles)
		{
			num15 = 10;
		}
		bool flag7 = false;
		Tile tile7 = Main.tile[val.X, val.Y];
		if (WorldGen.SolidOrSlopedTile(tile7, includePlatforms: true) || tile7.liquid > 0)
		{
			flag7 = true;
		}
		if (!flag7)
		{
			for (int i = 0; i < num15; i++)
			{
				int num16 = val.Y + i;
				Tile tile8 = Main.tile[val.X, num16];
				if (WorldGen.SolidOrSlopedTile(tile8, includePlatforms: true) || tile8.liquid > 0)
				{
					flag7 = true;
					val.Y += Math.Max(0, i - 1);
					break;
				}
			}
		}
		if (WorldGen.Skyblock.lowTiles)
		{
			if (!flag7)
			{
				for (int j = 0; j < num15; j++)
				{
					int num17 = val.Y + j;
					Tile tile9 = Main.tile[val.X - 1, num17];
					if (WorldGen.SolidOrSlopedTile(tile9, includePlatforms: true) || tile9.liquid > 0)
					{
						flag7 = true;
						val.X--;
						val.Y += Math.Max(0, j - 1);
						break;
					}
				}
			}
			if (!flag7)
			{
				for (int k = 0; k < num15; k++)
				{
					int num18 = val.Y + k;
					Tile tile10 = Main.tile[val.X + 1, num18];
					if (WorldGen.SolidOrSlopedTile(tile10, includePlatforms: true) || tile10.liquid > 0)
					{
						flag7 = true;
						val.X++;
						val.Y += Math.Max(0, k - 1);
						break;
					}
				}
			}
		}
		if (!flag7)
		{
			return false;
		}
		landingPoint = val;
		return true;
	}

	private static bool TileIsDangerous(int x, int y, Player player)
	{
		Tile tile = Main.tile[x, y];
		if (tile.liquid > 0 && tile.lava())
		{
			return true;
		}
		if (tile.wall == 87 && (double)y > Main.worldSurface && !NPC.downedPlantBoss)
		{
			return true;
		}
		if (Main.wallDungeon[tile.wall] && (double)y > Main.worldSurface && !NPC.downedBoss3)
		{
			return true;
		}
		if (tile.active() && Collision.CanTileHurt(tile.type, x, y, player))
		{
			return true;
		}
		return false;
	}

	private static bool IsInSolidTilesExtended(Vector2 testPosition, Vector2 playerVelocity, int width, int height, int gravDir)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		if (Collision.LavaCollision(testPosition, width, height))
		{
			return true;
		}
		if (Collision.AnyHurtingTiles(testPosition, width, height))
		{
			return true;
		}
		if (Collision.SolidCollision(testPosition, width, height))
		{
			return true;
		}
		Vector2 val = Vector2.UnitX * 16f;
		if (Collision.TileCollision(testPosition - val, val, width, height, fallThrough: true, fall2: true, gravDir) != val)
		{
			return true;
		}
		val = -Vector2.UnitX * 16f;
		if (Collision.TileCollision(testPosition - val, val, width, height, fallThrough: true, fall2: true, gravDir) != val)
		{
			return true;
		}
		val = Vector2.UnitY * 16f;
		if (Collision.TileCollision(testPosition - val, val, width, height, fallThrough: true, fall2: true, gravDir) != val)
		{
			return true;
		}
		val = -Vector2.UnitY * 16f;
		if (Collision.TileCollision(testPosition - val, val, width, height, fallThrough: true, fall2: true, gravDir) != val)
		{
			return true;
		}
		return false;
	}
}
