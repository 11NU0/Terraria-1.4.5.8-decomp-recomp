using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.GameContent.Liquid;
using Terraria.ID;
using Terraria.IO;

namespace Terraria;

public class WaterfallManager
{
	public struct WaterfallData
	{
		public int x;

		public int y;

		public int type;

		public int stopAtStep;
	}

	private const int minWet = 160;

	private const int maxWaterfallCountDefault = 1000;

	private const int maxLength = 100;

	private const int maxTypes = 28;

	public int maxWaterfallCount = 1000;

	private int qualityMax;

	private int currentMax;

	private WaterfallData[] waterfalls = new WaterfallData[1000];

	private Asset<Texture2D>[] waterfallTexture = new Asset<Texture2D>[28];

	private int wFallFrCounter;

	private int regularFrame;

	private int wFallFrCounter2;

	private int slowFrame;

	private int rainFrameCounter;

	private int rainFrameForeground;

	private int rainFrameBackground;

	private int lavaRainFrameCounter;

	private int lavaRainFrameForeground;

	private int lavaRainFrameBackground;

	private int snowFrameCounter;

	private int snowFrameForeground;

	private int findWaterfallCount;

	private int waterfallDist = 100;

	private static readonly uint Layer_Rain = 0u;

	private static readonly uint Layer_Waterfall = 1u;

	private static bool _shouldShowInvisibleBlocksAndWalls = false;

	public void BindTo(Preferences preferences)
	{
		preferences.OnLoad += Configuration_OnLoad;
	}

	private void Configuration_OnLoad(Preferences preferences)
	{
		maxWaterfallCount = Math.Max(0, preferences.Get("WaterfallDrawLimit", 1000));
		waterfalls = new WaterfallData[maxWaterfallCount];
	}

	public void LoadContent()
	{
		for (int i = 0; i < 28; i++)
		{
			waterfallTexture[i] = Main.Assets.Request<Texture2D>("Images/Waterfall_" + i, (AssetRequestMode)2);
		}
	}

	public bool CheckForWaterfall(int i, int j)
	{
		for (int k = 0; k < currentMax; k++)
		{
			if (waterfalls[k].x == i && waterfalls[k].y == j)
			{
				return true;
			}
		}
		return false;
	}

	public void FindWaterfalls(bool forced = false)
	{
		findWaterfallCount++;
		if (findWaterfallCount < 30 && !forced)
		{
			return;
		}
		findWaterfallCount = 0;
		TimeLogger.StartTimestamp fromTimestamp = TimeLogger.Start();
		waterfallDist = (int)(75f * Main.gfxQuality) + 25;
		qualityMax = (int)((float)maxWaterfallCount * Main.gfxQuality);
		currentMax = 0;
		int num = (int)(Main.screenPosition.X / 16f - 1f);
		int num2 = (int)((Main.screenPosition.X + (float)Main.screenWidth) / 16f) + 2;
		int num3 = (int)(Main.screenPosition.Y / 16f - 1f);
		int num4 = (int)((Main.screenPosition.Y + (float)Main.screenHeight) / 16f) + 2;
		num -= waterfallDist;
		num2 += waterfallDist;
		num3 -= waterfallDist;
		num4 += 20;
		if (num <= 0)
		{
			num = 1;
		}
		if (num2 >= Main.maxTilesX)
		{
			num2 = Main.maxTilesX - 1;
		}
		if (num3 <= 0)
		{
			num3 = 1;
		}
		if (num4 >= Main.maxTilesY)
		{
			num4 = Main.maxTilesY - 1;
		}
		for (int i = num; i < num2; i++)
		{
			for (int j = num3; j < num4; j++)
			{
				Tile tile = Main.tile[i, j];
				if (tile == null)
				{
					tile = new Tile();
					Main.tile[i, j] = tile;
				}
				if (!tile.active())
				{
					continue;
				}
				if (tile.halfBrick())
				{
					Tile tile2 = Main.tile[i, j - 1];
					if (tile2 == null)
					{
						tile2 = new Tile();
						Main.tile[i, j - 1] = tile2;
					}
					if (tile2.liquid < 16 || WorldGen.SolidTile(tile2))
					{
						Tile tile3 = Main.tile[i - 1, j];
						if (tile3 == null)
						{
							tile3 = new Tile();
							Main.tile[i - 1, j] = tile3;
						}
						Tile tile4 = Main.tile[i + 1, j];
						if (tile4 == null)
						{
							tile4 = new Tile();
							Main.tile[i + 1, j] = tile4;
						}
						if ((tile3.liquid > 160 || tile4.liquid > 160) && ((tile3.liquid == 0 && !WorldGen.SolidTile(tile3) && tile3.slope() == 0) || (tile4.liquid == 0 && !WorldGen.SolidTile(tile4) && tile4.slope() == 0)) && currentMax < qualityMax)
						{
							waterfalls[currentMax].type = 0;
							if (tile2.lava() || tile4.lava() || tile3.lava())
							{
								waterfalls[currentMax].type = 1;
							}
							else if (tile2.honey() || tile4.honey() || tile3.honey())
							{
								waterfalls[currentMax].type = 14;
							}
							else if (tile2.shimmer() || tile4.shimmer() || tile3.shimmer())
							{
								waterfalls[currentMax].type = 25;
							}
							else
							{
								waterfalls[currentMax].type = 0;
							}
							waterfalls[currentMax].x = i;
							waterfalls[currentMax].y = j;
							currentMax++;
						}
					}
				}
				if (tile.type == 196)
				{
					Tile tile5 = Main.tile[i, j + 1];
					if (tile5 == null)
					{
						tile5 = new Tile();
						Main.tile[i, j + 1] = tile5;
					}
					if (!WorldGen.SolidTile(tile5) && tile5.liquid == 0 && tile5.slope() == 0 && currentMax < qualityMax)
					{
						waterfalls[currentMax].type = 11;
						waterfalls[currentMax].x = i;
						waterfalls[currentMax].y = j + 1;
						currentMax++;
					}
				}
				if (tile.type == 460)
				{
					Tile tile6 = Main.tile[i, j + 1];
					if (tile6 == null)
					{
						tile6 = new Tile();
						Main.tile[i, j + 1] = tile6;
					}
					if (!WorldGen.SolidTile(tile6) && tile6.liquid == 0 && tile6.slope() == 0 && currentMax < qualityMax)
					{
						waterfalls[currentMax].type = 22;
						waterfalls[currentMax].x = i;
						waterfalls[currentMax].y = j + 1;
						currentMax++;
					}
				}
				if (tile.type == 717)
				{
					Tile tile7 = Main.tile[i, j + 1];
					if (tile7 == null)
					{
						tile7 = new Tile();
						Main.tile[i, j + 1] = tile7;
					}
					if (!WorldGen.SolidTile(tile7) && tile7.liquid == 0 && tile7.slope() == 0 && currentMax < qualityMax)
					{
						waterfalls[currentMax].type = 26;
						waterfalls[currentMax].x = i;
						waterfalls[currentMax].y = j + 1;
						currentMax++;
					}
				}
			}
		}
		TimeLogger.FindingWaterfalls.AddTime(fromTimestamp);
	}

	public void UpdateFrame()
	{
		wFallFrCounter++;
		if (wFallFrCounter > 2)
		{
			wFallFrCounter = 0;
			regularFrame++;
			if (regularFrame > 15)
			{
				regularFrame = 0;
			}
		}
		wFallFrCounter2++;
		if (wFallFrCounter2 > 6)
		{
			wFallFrCounter2 = 0;
			slowFrame++;
			if (slowFrame > 15)
			{
				slowFrame = 0;
			}
		}
		rainFrameCounter++;
		if (rainFrameCounter > 0)
		{
			rainFrameForeground++;
			if (rainFrameForeground > 7)
			{
				rainFrameForeground -= 8;
			}
			if (rainFrameCounter > 2)
			{
				rainFrameCounter = 0;
				rainFrameBackground--;
				if (rainFrameBackground < 0)
				{
					rainFrameBackground = 7;
				}
			}
		}
		lavaRainFrameCounter++;
		if (lavaRainFrameCounter == 1 || lavaRainFrameCounter == 3)
		{
			lavaRainFrameForeground++;
			if (lavaRainFrameForeground > 7)
			{
				lavaRainFrameForeground -= 8;
			}
		}
		else if (lavaRainFrameCounter > 3)
		{
			lavaRainFrameCounter = 0;
			lavaRainFrameBackground--;
			if (lavaRainFrameBackground < 0)
			{
				lavaRainFrameBackground = 7;
			}
		}
		if (++snowFrameCounter > 3)
		{
			snowFrameCounter = 0;
			if (++snowFrameForeground > 7)
			{
				snowFrameForeground = 0;
			}
		}
	}

	private void DrawWaterfall(int Style = 0, float Alpha = 1f)
	{
		//IL_05d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0285: Unknown result type (might be due to invalid IL or missing references)
		//IL_0296: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_035b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0360: Unknown result type (might be due to invalid IL or missing references)
		//IL_0389: Unknown result type (might be due to invalid IL or missing references)
		//IL_08f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_08f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_03cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_03cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03da: Unknown result type (might be due to invalid IL or missing references)
		//IL_03df: Unknown result type (might be due to invalid IL or missing references)
		//IL_092c: Unknown result type (might be due to invalid IL or missing references)
		//IL_092e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0933: Unknown result type (might be due to invalid IL or missing references)
		//IL_0907: Unknown result type (might be due to invalid IL or missing references)
		//IL_03fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_03fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_03fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0405: Unknown result type (might be due to invalid IL or missing references)
		//IL_0488: Unknown result type (might be due to invalid IL or missing references)
		//IL_048a: Unknown result type (might be due to invalid IL or missing references)
		//IL_048c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0493: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_04be: Unknown result type (might be due to invalid IL or missing references)
		//IL_0430: Unknown result type (might be due to invalid IL or missing references)
		//IL_0432: Unknown result type (might be due to invalid IL or missing references)
		//IL_0434: Unknown result type (might be due to invalid IL or missing references)
		//IL_043b: Unknown result type (might be due to invalid IL or missing references)
		//IL_045b: Unknown result type (might be due to invalid IL or missing references)
		//IL_045d: Unknown result type (might be due to invalid IL or missing references)
		//IL_045f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0466: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b8a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b8f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b94: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ba7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bac: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b43: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b48: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b4d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b60: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b65: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bda: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bdf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d35: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d3a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d3f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d50: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d55: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c45: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c4a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c4f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c5f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c64: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c01: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c06: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c0b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c1b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c20: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cf1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cf6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cfb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d0c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d11: Unknown result type (might be due to invalid IL or missing references)
		//IL_0caa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0caf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cb4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cc5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cca: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f71: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f76: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f7b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f8b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f90: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f33: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f38: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f3d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f4d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f52: Unknown result type (might be due to invalid IL or missing references)
		//IL_0eec: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ef1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ef6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f06: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f0b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ea8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ead: Unknown result type (might be due to invalid IL or missing references)
		//IL_0eb2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ec2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ec7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e51: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e56: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e5b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e67: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e6c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e14: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e19: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e1e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e2a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e2f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dd1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dd6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ddb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0de7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dec: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d94: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d99: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d9e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0daa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0daf: Unknown result type (might be due to invalid IL or missing references)
		//IL_1493: Unknown result type (might be due to invalid IL or missing references)
		//IL_1495: Unknown result type (might be due to invalid IL or missing references)
		//IL_10a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_10a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_10ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_10ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_10bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fd2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fd7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fdc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0feb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ff0: Unknown result type (might be due to invalid IL or missing references)
		//IL_142e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1433: Unknown result type (might be due to invalid IL or missing references)
		//IL_1438: Unknown result type (might be due to invalid IL or missing references)
		//IL_1447: Unknown result type (might be due to invalid IL or missing references)
		//IL_144c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1274: Unknown result type (might be due to invalid IL or missing references)
		//IL_1279: Unknown result type (might be due to invalid IL or missing references)
		//IL_127e: Unknown result type (might be due to invalid IL or missing references)
		//IL_128d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1292: Unknown result type (might be due to invalid IL or missing references)
		//IL_112e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1133: Unknown result type (might be due to invalid IL or missing references)
		//IL_1138: Unknown result type (might be due to invalid IL or missing references)
		//IL_1148: Unknown result type (might be due to invalid IL or missing references)
		//IL_114d: Unknown result type (might be due to invalid IL or missing references)
		//IL_10ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_10ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_10f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_1104: Unknown result type (might be due to invalid IL or missing references)
		//IL_1109: Unknown result type (might be due to invalid IL or missing references)
		//IL_1059: Unknown result type (might be due to invalid IL or missing references)
		//IL_105e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1063: Unknown result type (might be due to invalid IL or missing references)
		//IL_1073: Unknown result type (might be due to invalid IL or missing references)
		//IL_1078: Unknown result type (might be due to invalid IL or missing references)
		//IL_1018: Unknown result type (might be due to invalid IL or missing references)
		//IL_101d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1022: Unknown result type (might be due to invalid IL or missing references)
		//IL_1032: Unknown result type (might be due to invalid IL or missing references)
		//IL_1037: Unknown result type (might be due to invalid IL or missing references)
		//IL_13bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_13c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_13c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_13d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_13d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_11dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_11e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_11e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_11fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_1200: Unknown result type (might be due to invalid IL or missing references)
		//IL_1324: Unknown result type (might be due to invalid IL or missing references)
		//IL_1329: Unknown result type (might be due to invalid IL or missing references)
		//IL_132e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1342: Unknown result type (might be due to invalid IL or missing references)
		//IL_1347: Unknown result type (might be due to invalid IL or missing references)
		Main.tileSolid[546] = false;
		float num = 0f;
		float num2 = 99999f;
		float num3 = 99999f;
		int num4 = -1;
		int num5 = -1;
		float num6 = 0f;
		float num7 = 99999f;
		float num8 = 99999f;
		int num9 = -1;
		int num10 = -1;
		for (int i = 0; i < currentMax; i++)
		{
			int num11 = 0;
			int num12 = waterfalls[i].type;
			int num13 = waterfalls[i].x;
			int num14 = waterfalls[i].y;
			int num15 = 0;
			int num16 = 0;
			int num17 = 0;
			int num18 = 0;
			int num19 = 0;
			int num20 = 0;
			int num21;
			int num22;
			if (num12 == 1 || num12 == 14 || num12 == 25)
			{
				if (Main.drewLava || waterfalls[i].stopAtStep == 0)
				{
					continue;
				}
				num21 = 32 * slowFrame;
			}
			else
			{
				switch (num12)
				{
				case 11:
				case 22:
				case 26:
				{
					if (Main.drewLava)
					{
						continue;
					}
					num22 = waterfallDist / 4;
					if (num12 == 22)
					{
						num22 = waterfallDist / 2;
					}
					if (waterfalls[i].stopAtStep > num22)
					{
						waterfalls[i].stopAtStep = num22;
					}
					if (waterfalls[i].stopAtStep == 0 || (float)(num14 + num22) < Main.screenPosition.Y / 16f || (float)num13 < Main.screenPosition.X / 16f - 20f || (float)num13 > (Main.screenPosition.X + (float)Main.screenWidth) / 16f + 20f)
					{
						continue;
					}
					int num23 = 0;
					int num24 = 0;
					if (num13 % 2 == 0)
					{
						switch (num12)
						{
						case 22:
							num23 = snowFrameForeground + 3;
							if (num23 > 7)
							{
								num23 -= 8;
							}
							break;
						case 26:
							num23 = lavaRainFrameForeground + 3;
							if (num23 > 7)
							{
								num23 -= 8;
							}
							num24 = lavaRainFrameBackground + 2;
							if (num24 > 7)
							{
								num24 -= 8;
							}
							break;
						default:
							num23 = rainFrameForeground + 3;
							if (num23 > 7)
							{
								num23 -= 8;
							}
							num24 = rainFrameBackground + 2;
							if (num24 > 7)
							{
								num24 -= 8;
							}
							break;
						}
					}
					else
					{
						switch (num12)
						{
						case 22:
							num23 = snowFrameForeground;
							break;
						case 26:
							num23 = lavaRainFrameForeground;
							num24 = lavaRainFrameBackground;
							break;
						default:
							num23 = rainFrameForeground;
							num24 = rainFrameBackground;
							break;
						}
					}
					Rectangle sourceRectangle = new Rectangle(num24 * 18, 0, 16, 16);
					Rectangle sourceRectangle2 = new Rectangle(num23 * 18, 0, 16, 16);
					Vector2 origin = new Vector2(8f, 8f);
					Vector2 position = ((num14 % 2 != 0) ? (new Vector2((float)(num13 * 16 + 8), (float)(num14 * 16 + 8)) - Main.screenPosition) : (new Vector2((float)(num13 * 16 + 9), (float)(num14 * 16 + 8)) - Main.screenPosition));
					if (!WorldGen.InWorld(num13, num14 - 1))
					{
						continue;
					}
					Tile tile = Main.tile[num13, num14 - 1];
					if (tile.active() && tile.bottomSlope())
					{
						position.Y -= 16f;
					}
					bool flag = false;
					for (int j = 0; j < num22; j++)
					{
						Main.tileBatch.SetLayer(Layer_Rain, 0);
						Color val = Lighting.GetColor(num13, num14);
						float num25 = 0.6f;
						float num26 = 0.3f;
						if (num12 == 26)
						{
							val = new Color(255, 255, 255, 127);
							AddLight(num12, num13, num14);
							num25 = 0.9f;
							num26 = 0.4f;
						}
						if (j > num22 - 8)
						{
							float num27 = (float)(num22 - j) / 8f;
							num25 *= num27;
							num26 *= num27;
						}
						Color val2 = val * num25;
						Color val3 = val * num26;
						switch (num12)
						{
						case 22:
							Main.tileBatch.Draw(waterfallTexture[22].Value, position, sourceRectangle2, val2, origin, 1f, (SpriteEffects)0);
							break;
						case 26:
							Main.tileBatch.Draw(waterfallTexture[27].Value, position, sourceRectangle, val3, origin, 1f, (SpriteEffects)0);
							Main.tileBatch.Draw(waterfallTexture[26].Value, position, sourceRectangle2, val2, origin, 1f, (SpriteEffects)0);
							break;
						default:
							Main.tileBatch.Draw(waterfallTexture[12].Value, position, sourceRectangle, val3, origin, 1f, (SpriteEffects)0);
							Main.tileBatch.Draw(waterfallTexture[11].Value, position, sourceRectangle2, val2, origin, 1f, (SpriteEffects)0);
							break;
						}
						if (flag)
						{
							break;
						}
						num14++;
						if (num14 >= Main.maxTilesY)
						{
							break;
						}
						Tile tile2 = Main.tile[num13, num14];
						if (WorldGen.SolidTile(tile2))
						{
							flag = true;
						}
						if (tile2.liquid > 0)
						{
							int num28 = (int)(16f * ((float)(int)tile2.liquid / 255f)) & 0xFE;
							if (num28 >= 15)
							{
								break;
							}
							sourceRectangle2.Height -= num28;
							sourceRectangle.Height -= num28;
						}
						if (num14 % 2 == 0)
						{
							position.X++;
						}
						else
						{
							position.X--;
						}
						position.Y += 16f;
					}
					waterfalls[i].stopAtStep = 0;
					continue;
				}
				case 0:
					num12 = Style;
					break;
				case 2:
					if (Main.drewLava)
					{
						continue;
					}
					break;
				}
				num21 = 32 * regularFrame;
			}
			int num29 = 0;
			num22 = waterfallDist;
			Color color = Color.White;
			for (int k = 0; k < num22; k++)
			{
				if (num29 >= 2)
				{
					break;
				}
				AddLight(num12, num13, num14);
				Tile tile3 = Main.tile[num13, num14];
				if (tile3 == null)
				{
					tile3 = new Tile();
					Main.tile[num13, num14] = tile3;
				}
				if (tile3.nactive() && Main.tileSolid[tile3.type] && !Main.tileSolidTop[tile3.type] && !TileID.Sets.Platforms[tile3.type] && tile3.blockType() == 0)
				{
					break;
				}
				Tile tile4 = Main.tile[num13 - 1, num14];
				if (tile4 == null)
				{
					tile4 = new Tile();
					Main.tile[num13 - 1, num14] = tile4;
				}
				Tile tile5 = Main.tile[num13, num14 + 1];
				if (tile5 == null)
				{
					tile5 = new Tile();
					Main.tile[num13, num14 + 1] = tile5;
				}
				Tile tile6 = Main.tile[num13 + 1, num14];
				if (tile6 == null)
				{
					tile6 = new Tile();
					Main.tile[num13 + 1, num14] = tile6;
				}
				if (WorldGen.SolidTile(tile5) && !tile3.halfBrick())
				{
					num11 = 8;
				}
				else if (num16 != 0)
				{
					num11 = 0;
				}
				int num30 = 0;
				int num31 = num18;
				int num32 = 0;
				int num33 = 0;
				bool flag2 = false;
				if (tile5.topSlope() && !tile3.halfBrick() && tile5.type != 19)
				{
					flag2 = true;
					if (tile5.slope() == 1)
					{
						num30 = 1;
						num32 = 1;
						num17 = 1;
						num18 = num17;
					}
					else
					{
						num30 = -1;
						num32 = -1;
						num17 = -1;
						num18 = num17;
					}
					num33 = 1;
				}
				else if ((!WorldGen.SolidTile(tile5) && !tile5.bottomSlope() && !tile3.halfBrick()) || (!tile5.active() && !tile3.halfBrick()))
				{
					num29 = 0;
					num33 = 1;
					num32 = 0;
				}
				else if ((WorldGen.SolidTile(tile4) || tile4.topSlope() || tile4.liquid > 0) && !WorldGen.SolidTile(tile6) && tile6.liquid == 0)
				{
					if (num17 == -1)
					{
						num29++;
					}
					num32 = 1;
					num33 = 0;
					num17 = 1;
				}
				else if ((WorldGen.SolidTile(tile6) || tile6.topSlope() || tile6.liquid > 0) && !WorldGen.SolidTile(tile4) && tile4.liquid == 0)
				{
					if (num17 == 1)
					{
						num29++;
					}
					num32 = -1;
					num33 = 0;
					num17 = -1;
				}
				else if (((!WorldGen.SolidTile(tile6) && !tile3.topSlope()) || tile6.liquid == 0) && !WorldGen.SolidTile(tile4) && !tile3.topSlope() && tile4.liquid == 0)
				{
					num33 = 0;
					num32 = num17;
				}
				else
				{
					num29++;
					num33 = 0;
					num32 = 0;
				}
				if (num29 >= 2)
				{
					num17 *= -1;
					num32 *= -1;
				}
				int num34 = -1;
				if (num12 != 1 && num12 != 14 && num12 != 25)
				{
					if (tile5.active())
					{
						num34 = tile5.type;
					}
					if (tile3.active())
					{
						num34 = tile3.type;
					}
				}
				switch (num34)
				{
				case 160:
					num12 = 2;
					break;
				case 262:
				case 263:
				case 264:
				case 265:
				case 266:
				case 267:
				case 268:
					num12 = 15 + num34 - 262;
					break;
				}
				Color color2 = Lighting.GetColor(num13, num14);
				if (k > 50)
				{
					TrySparkling(num13, num14, num17, color2);
				}
				float alpha = GetAlpha(Alpha, num22, num12, num14, k, tile3);
				color2 = StylizeColor(alpha, num22, num12, num14, k, tile3, color2);
				if (num12 == 1)
				{
					float num35 = Math.Abs((float)(num13 * 16 + 8) - (Main.screenPosition.X + (float)(Main.screenWidth / 2)));
					float num36 = Math.Abs((float)(num14 * 16 + 8) - (Main.screenPosition.Y + (float)(Main.screenHeight / 2)));
					if (num35 < (float)(Main.screenWidth * 2) && num36 < (float)(Main.screenHeight * 2))
					{
						float num37 = (float)Math.Sqrt(num35 * num35 + num36 * num36);
						float num38 = 1f - num37 / ((float)Main.screenWidth * 0.75f);
						if (num38 > 0f)
						{
							num6 += num38;
						}
					}
					if (num35 < num7)
					{
						num7 = num35;
						num9 = num13 * 16 + 8;
					}
					if (num36 < num8)
					{
						num8 = num35;
						num10 = num14 * 16 + 8;
					}
				}
				else if (num12 != 1 && num12 != 14 && num12 != 25 && num12 != 11 && num12 != 12 && num12 != 22)
				{
					float num39 = Math.Abs((float)(num13 * 16 + 8) - (Main.screenPosition.X + (float)(Main.screenWidth / 2)));
					float num40 = Math.Abs((float)(num14 * 16 + 8) - (Main.screenPosition.Y + (float)(Main.screenHeight / 2)));
					if (num39 < (float)(Main.screenWidth * 2) && num40 < (float)(Main.screenHeight * 2))
					{
						float num41 = (float)Math.Sqrt(num39 * num39 + num40 * num40);
						float num42 = 1f - num41 / ((float)Main.screenWidth * 0.75f);
						if (num42 > 0f)
						{
							num += num42;
						}
					}
					if (num39 < num2)
					{
						num2 = num39;
						num4 = num13 * 16 + 8;
					}
					if (num40 < num3)
					{
						num3 = num39;
						num5 = num14 * 16 + 8;
					}
				}
				int num43 = tile3.liquid / 16;
				Main.tileBatch.SetLayer(Layer_Waterfall, 0);
				if (flag2 && num17 != num31)
				{
					int num44 = 2;
					if (num31 == 1)
					{
						DrawWaterfall(num12, num13, num14, alpha, new Vector2((float)(num13 * 16 - 16), (float)(num14 * 16 + 16 - num44)) - Main.screenPosition, new Rectangle(num21, 24, 32, 16 - num43 - num44), color2, (SpriteEffects)1);
					}
					else
					{
						DrawWaterfall(num12, num13, num14, alpha, new Vector2((float)(num13 * 16), (float)(num14 * 16 + 16 - num44)) - Main.screenPosition, new Rectangle(num21, 24, 32, 16 - num43 - num44), color2, (SpriteEffects)0);
					}
				}
				if (num15 == 0 && num30 != 0 && num16 == 1 && num17 != num18)
				{
					num30 = 0;
					num17 = num18;
					color2 = Color.White;
					if (num17 == 1)
					{
						DrawWaterfall(num12, num13, num14, alpha, new Vector2((float)(num13 * 16 - 16), (float)(num14 * 16 + 16)) - Main.screenPosition, new Rectangle(num21, 24, 32, 16 - num43), color2, (SpriteEffects)1);
					}
					else
					{
						DrawWaterfall(num12, num13, num14, alpha, new Vector2((float)(num13 * 16 - 16), (float)(num14 * 16 + 16)) - Main.screenPosition, new Rectangle(num21, 24, 32, 16 - num43), color2, (SpriteEffects)1);
					}
				}
				if (num19 != 0 && num32 == 0 && num33 == 1)
				{
					if (num17 == 1)
					{
						if (num20 != num12)
						{
							DrawWaterfall(num12, num13, num14, alpha, new Vector2((float)(num13 * 16), (float)(num14 * 16 + num11 + 8)) - Main.screenPosition, new Rectangle(num21, 0, 16, 16 - num43 - 8), color, (SpriteEffects)1);
						}
						else
						{
							DrawWaterfall(num12, num13, num14, alpha, new Vector2((float)(num13 * 16), (float)(num14 * 16 + num11 + 8)) - Main.screenPosition, new Rectangle(num21, 0, 16, 16 - num43 - 8), color2, (SpriteEffects)1);
						}
					}
					else
					{
						DrawWaterfall(num12, num13, num14, alpha, new Vector2((float)(num13 * 16), (float)(num14 * 16 + num11 + 8)) - Main.screenPosition, new Rectangle(num21, 0, 16, 16 - num43 - 8), color2, (SpriteEffects)0);
					}
				}
				if (num11 == 8 && num16 == 1 && num19 == 0)
				{
					if (num18 == -1)
					{
						if (num20 != num12)
						{
							DrawWaterfall(num20, num13, num14, alpha, new Vector2((float)(num13 * 16), (float)(num14 * 16)) - Main.screenPosition, new Rectangle(num21, 24, 32, 8), color, (SpriteEffects)0);
						}
						else
						{
							DrawWaterfall(num12, num13, num14, alpha, new Vector2((float)(num13 * 16), (float)(num14 * 16)) - Main.screenPosition, new Rectangle(num21, 24, 32, 8), color2, (SpriteEffects)0);
						}
					}
					else if (num20 != num12)
					{
						DrawWaterfall(num20, num13, num14, alpha, new Vector2((float)(num13 * 16 - 16), (float)(num14 * 16)) - Main.screenPosition, new Rectangle(num21, 24, 32, 8), color, (SpriteEffects)1);
					}
					else
					{
						DrawWaterfall(num12, num13, num14, alpha, new Vector2((float)(num13 * 16 - 16), (float)(num14 * 16)) - Main.screenPosition, new Rectangle(num21, 24, 32, 8), color2, (SpriteEffects)1);
					}
				}
				if (num30 != 0 && num15 == 0)
				{
					if (num31 == 1)
					{
						if (num20 != num12)
						{
							DrawWaterfall(num20, num13, num14, alpha, new Vector2((float)(num13 * 16 - 16), (float)(num14 * 16)) - Main.screenPosition, new Rectangle(num21, 24, 32, 16 - num43), color, (SpriteEffects)1);
						}
						else
						{
							DrawWaterfall(num12, num13, num14, alpha, new Vector2((float)(num13 * 16 - 16), (float)(num14 * 16)) - Main.screenPosition, new Rectangle(num21, 24, 32, 16 - num43), color2, (SpriteEffects)1);
						}
					}
					else if (num20 != num12)
					{
						DrawWaterfall(num20, num13, num14, alpha, new Vector2((float)(num13 * 16), (float)(num14 * 16)) - Main.screenPosition, new Rectangle(num21, 24, 32, 16 - num43), color, (SpriteEffects)0);
					}
					else
					{
						DrawWaterfall(num12, num13, num14, alpha, new Vector2((float)(num13 * 16), (float)(num14 * 16)) - Main.screenPosition, new Rectangle(num21, 24, 32, 16 - num43), color2, (SpriteEffects)0);
					}
				}
				if (num33 == 1 && num30 == 0 && num19 == 0)
				{
					if (num17 == -1)
					{
						if (num16 == 0)
						{
							DrawWaterfall(num12, num13, num14, alpha, new Vector2((float)(num13 * 16), (float)(num14 * 16 + num11)) - Main.screenPosition, new Rectangle(num21, 0, 16, 16 - num43), color2, (SpriteEffects)0);
						}
						else if (num20 != num12)
						{
							DrawWaterfall(num20, num13, num14, alpha, new Vector2((float)(num13 * 16), (float)(num14 * 16)) - Main.screenPosition, new Rectangle(num21, 24, 32, 16 - num43), color, (SpriteEffects)0);
						}
						else
						{
							DrawWaterfall(num12, num13, num14, alpha, new Vector2((float)(num13 * 16), (float)(num14 * 16)) - Main.screenPosition, new Rectangle(num21, 24, 32, 16 - num43), color2, (SpriteEffects)0);
						}
					}
					else if (num16 == 0)
					{
						DrawWaterfall(num12, num13, num14, alpha, new Vector2((float)(num13 * 16), (float)(num14 * 16 + num11)) - Main.screenPosition, new Rectangle(num21, 0, 16, 16 - num43), color2, (SpriteEffects)1);
					}
					else if (num20 != num12)
					{
						DrawWaterfall(num20, num13, num14, alpha, new Vector2((float)(num13 * 16 - 16), (float)(num14 * 16)) - Main.screenPosition, new Rectangle(num21, 24, 32, 16 - num43), color, (SpriteEffects)1);
					}
					else
					{
						DrawWaterfall(num12, num13, num14, alpha, new Vector2((float)(num13 * 16 - 16), (float)(num14 * 16)) - Main.screenPosition, new Rectangle(num21, 24, 32, 16 - num43), color2, (SpriteEffects)1);
					}
				}
				else
				{
					switch (num32)
					{
					case 1:
						if (Main.tile[num13, num14].liquid > 0 && !Main.tile[num13, num14].halfBrick())
						{
							break;
						}
						if (num30 == 1)
						{
							for (int m = 0; m < 8; m++)
							{
								int num49 = m * 2;
								int num50 = 14 - m * 2;
								int num51 = num49;
								num11 = 8;
								if (num15 == 0 && m < 2)
								{
									num51 = 4;
								}
								DrawWaterfall(num12, num13, num14, alpha, new Vector2((float)(num13 * 16 + num49), (float)(num14 * 16 + num11 + num51)) - Main.screenPosition, new Rectangle(16 + num21 + num50, 0, 2, 16 - num11), color2, (SpriteEffects)1);
							}
						}
						else
						{
							int num52 = 16;
							if (TileID.Sets.BlocksWaterDrawingBehindSelf[Main.tile[num13, num14].type])
							{
								num52 = 8;
							}
							else if (TileID.Sets.BlocksWaterDrawingBehindSelf[Main.tile[num13, num14 + 1].type])
							{
								num52 = 8;
							}
							DrawWaterfall(num12, num13, num14, alpha, new Vector2((float)(num13 * 16), (float)(num14 * 16 + num11)) - Main.screenPosition, new Rectangle(16 + num21, 0, 16, num52), color2, (SpriteEffects)1);
						}
						break;
					case -1:
						if (Main.tile[num13, num14].liquid > 0 && !Main.tile[num13, num14].halfBrick())
						{
							break;
						}
						if (num30 == -1)
						{
							for (int l = 0; l < 8; l++)
							{
								int num45 = l * 2;
								int num46 = l * 2;
								int num47 = 14 - l * 2;
								num11 = 8;
								if (num15 == 0 && l > 5)
								{
									num47 = 4;
								}
								DrawWaterfall(num12, num13, num14, alpha, new Vector2((float)(num13 * 16 + num45), (float)(num14 * 16 + num11 + num47)) - Main.screenPosition, new Rectangle(16 + num21 + num46, 0, 2, 16 - num11), color2, (SpriteEffects)1);
							}
						}
						else
						{
							int num48 = 16;
							if (TileID.Sets.BlocksWaterDrawingBehindSelf[Main.tile[num13, num14].type])
							{
								num48 = 8;
							}
							else if (TileID.Sets.BlocksWaterDrawingBehindSelf[Main.tile[num13, num14 + 1].type])
							{
								num48 = 8;
							}
							DrawWaterfall(num12, num13, num14, alpha, new Vector2((float)(num13 * 16), (float)(num14 * 16 + num11)) - Main.screenPosition, new Rectangle(16 + num21, 0, 16, num48), color2, (SpriteEffects)0);
						}
						break;
					case 0:
						if (num33 == 0)
						{
							if (Main.tile[num13, num14].liquid <= 0 || Main.tile[num13, num14].halfBrick())
							{
								DrawWaterfall(num12, num13, num14, alpha, new Vector2((float)(num13 * 16), (float)(num14 * 16 + num11)) - Main.screenPosition, new Rectangle(16 + num21, 0, 16, 16), color2, (SpriteEffects)0);
							}
							k = 1000;
						}
						break;
					}
				}
				if (tile3.liquid > 0 && !tile3.halfBrick())
				{
					k = 1000;
				}
				num16 = num33;
				num18 = num17;
				num15 = num32;
				num13 += num32;
				num14 += num33;
				num19 = num30;
				color = color2;
				if (num20 != num12)
				{
					num20 = num12;
				}
				if ((tile4.active() && (tile4.type == 189 || tile4.type == 196)) || (tile6.active() && (tile6.type == 189 || tile6.type == 196)) || (tile5.active() && (tile5.type == 189 || tile5.type == 196)))
				{
					num22 = (int)(40f * ((float)Main.maxTilesX / 4200f) * Main.gfxQuality);
				}
				if (!WorldGen.InWorld(num13, num14))
				{
					break;
				}
			}
		}
		Main.ambientWaterfallX = num4;
		Main.ambientWaterfallY = num5;
		Main.ambientWaterfallStrength = num;
		Main.ambientLavafallX = num9;
		Main.ambientLavafallY = num10;
		Main.ambientLavafallStrength = num6;
		Main.tileSolid[546] = true;
	}

	private void DrawWaterfall(int waterfallType, int x, int y, float opacity, Vector2 position, Rectangle sourceRect, Color color, SpriteEffects effects)
	{
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		Texture2D value = waterfallTexture[waterfallType].Value;
		if (waterfallType == 25)
		{
			Lighting.GetCornerColors(x, y, out var vertices);
			LiquidRenderer.SetShimmerVertexColors(ref vertices, opacity, x, y);
			Main.tileBatch.Draw(value, position + new Vector2(0f, 0f), sourceRect, vertices, default, 1f, effects);
			sourceRect.Y += 42;
			LiquidRenderer.SetShimmerVertexColors_Sparkle(ref vertices, opacity, x, y, top: true);
			Main.tileBatch.Draw(value, position + new Vector2(0f, 0f), sourceRect, vertices, default, 1f, effects);
		}
		else
		{
			Main.tileBatch.Draw(value, position, sourceRect, color, default, 1f, effects);
		}
	}

	private static Color StylizeColor(float alpha, int maxSteps, int waterfallType, int y, int s, Tile tileCache, Color aColor)
	{
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		float num = (float)(int)aColor.R * alpha;
		float num2 = (float)(int)aColor.G * alpha;
		float num3 = (float)(int)aColor.B * alpha;
		float num4 = (float)(int)aColor.A * alpha;
		switch (waterfallType)
		{
		case 1:
			if (num < 190f * alpha)
			{
				num = 190f * alpha;
			}
			if (num2 < 190f * alpha)
			{
				num2 = 190f * alpha;
			}
			if (num3 < 190f * alpha)
			{
				num3 = 190f * alpha;
			}
			break;
		case 2:
			num = (float)Main.DiscoR * alpha;
			num2 = (float)Main.DiscoG * alpha;
			num3 = (float)Main.DiscoB * alpha;
			break;
		case 15:
		case 16:
		case 17:
		case 18:
		case 19:
		case 20:
		case 21:
			num = 255f * alpha;
			num2 = 255f * alpha;
			num3 = 255f * alpha;
			break;
		}
		aColor = new Color((int)num, (int)num2, (int)num3, (int)num4);
		return aColor;
	}

	private static float GetAlpha(float Alpha, int maxSteps, int waterfallType, int y, int s, Tile tileCache)
	{
		float num = waterfallType switch
		{
			1 => 1f, 
			14 => 0.8f, 
			25 => 0.75f, 
			_ => ((tileCache.wall != 0 && (_shouldShowInvisibleBlocksAndWalls || (tileCache.wall != 318 && !tileCache.invisibleWall()))) || !((double)y < Main.worldSurface)) ? (0.6f * Alpha) : Alpha, 
		};
		if (s > maxSteps - 10)
		{
			num *= (float)(maxSteps - s) / 10f;
		}
		return num;
	}

	private static void TrySparkling(int x, int y, int direction, Color aColor2)
	{
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		if (aColor2.R > 20 || aColor2.B > 20 || aColor2.G > 20)
		{
			float num = (int)aColor2.R;
			if ((float)(int)aColor2.G > num)
			{
				num = (int)aColor2.G;
			}
			if ((float)(int)aColor2.B > num)
			{
				num = (int)aColor2.B;
			}
			if ((float)Main.rand.Next(20000) < num / 30f)
			{
				int num2 = Dust.NewDust(new Vector2((float)(x * 16 - direction * 7), (float)(y * 16 + 6)), 10, 8, 43, 0f, 0f, 254, Color.White, 0.5f);
				Dust obj = Main.dust[num2];
				obj.velocity *= 0f;
			}
		}
	}

	private static void AddLight(int waterfallType, int x, int y)
	{
		switch (waterfallType)
		{
		case 1:
		{
			float r;
			float num4 = (r = (0.55f + (float)(270 - Main.mouseTextColor) / 900f) * 0.4f);
			float g = num4 * 0.3f;
			float b = num4 * 0.1f;
			Lighting.AddLight(x, y, r, g, b);
			break;
		}
		case 26:
		{
			float r;
			float num3 = (r = (0.55f + (float)(270 - Main.mouseTextColor) / 900f) * 0.6f);
			float g = num3 * 0.3f;
			float b = num3 * 0.1f;
			Lighting.AddLight(x, y, r, g, b);
			break;
		}
		case 2:
		{
			float r = (float)Main.DiscoR / 255f;
			float g = (float)Main.DiscoG / 255f;
			float b = (float)Main.DiscoB / 255f;
			r *= 0.2f;
			g *= 0.2f;
			b *= 0.2f;
			Lighting.AddLight(x, y, r, g, b);
			break;
		}
		case 15:
		{
			float r = 0f;
			float g = 0f;
			float b = 0.2f;
			Lighting.AddLight(x, y, r, g, b);
			break;
		}
		case 16:
		{
			float r = 0f;
			float g = 0.2f;
			float b = 0f;
			Lighting.AddLight(x, y, r, g, b);
			break;
		}
		case 17:
		{
			float r = 0f;
			float g = 0f;
			float b = 0.2f;
			Lighting.AddLight(x, y, r, g, b);
			break;
		}
		case 18:
		{
			float r = 0f;
			float g = 0.2f;
			float b = 0f;
			Lighting.AddLight(x, y, r, g, b);
			break;
		}
		case 19:
		{
			float r = 0.2f;
			float g = 0f;
			float b = 0f;
			Lighting.AddLight(x, y, r, g, b);
			break;
		}
		case 20:
			Lighting.AddLight(x, y, 0.2f, 0.2f, 0.2f);
			break;
		case 21:
		{
			float r = 0.2f;
			float g = 0f;
			float b = 0f;
			Lighting.AddLight(x, y, r, g, b);
			break;
		}
		case 25:
		{
			float num = 0.7f;
			float num2 = 0.7f;
			num += (float)(270 - Main.mouseTextColor) / 900f;
			num2 += (float)(270 - Main.mouseTextColor) / 125f;
			Lighting.AddLight(x, y, num * 0.6f, num2 * 0.25f, num * 0.9f);
			break;
		}
		}
	}

	public void Draw()
	{
		_shouldShowInvisibleBlocksAndWalls = Main.ShouldShowInvisibleBlocksAndWalls();
		for (int i = 0; i < currentMax; i++)
		{
			waterfalls[i].stopAtStep = waterfallDist;
		}
		Main.drewLava = false;
		if (Main.liquidAlpha[0] > 0f)
		{
			DrawWaterfall(0, Main.liquidAlpha[0]);
		}
		if (Main.liquidAlpha[2] > 0f)
		{
			DrawWaterfall(3, Main.liquidAlpha[2]);
		}
		if (Main.liquidAlpha[3] > 0f)
		{
			DrawWaterfall(4, Main.liquidAlpha[3]);
		}
		if (Main.liquidAlpha[4] > 0f)
		{
			DrawWaterfall(5, Main.liquidAlpha[4]);
		}
		if (Main.liquidAlpha[5] > 0f)
		{
			DrawWaterfall(6, Main.liquidAlpha[5]);
		}
		if (Main.liquidAlpha[6] > 0f)
		{
			DrawWaterfall(7, Main.liquidAlpha[6]);
		}
		if (Main.liquidAlpha[7] > 0f)
		{
			DrawWaterfall(8, Main.liquidAlpha[7]);
		}
		if (Main.liquidAlpha[8] > 0f)
		{
			DrawWaterfall(9, Main.liquidAlpha[8]);
		}
		if (Main.liquidAlpha[9] > 0f)
		{
			DrawWaterfall(10, Main.liquidAlpha[9]);
		}
		if (Main.liquidAlpha[10] > 0f)
		{
			DrawWaterfall(13, Main.liquidAlpha[10]);
		}
		if (Main.liquidAlpha[12] > 0f)
		{
			DrawWaterfall(23, Main.liquidAlpha[12]);
		}
		if (Main.liquidAlpha[13] > 0f)
		{
			DrawWaterfall(24, Main.liquidAlpha[13]);
		}
	}
}
