using Microsoft.Xna.Framework;

namespace Terraria.GameContent.Generation.Dungeon.Features;

public class DungeonWindowMosaic : DungeonWindow
{
	public DungeonWindowMosaic(DungeonFeatureSettings settings)
		: base(settings)
	{
	}

	public override bool GenerateFeature(DungeonData data, int x, int y)
	{
		generated = false;
		DungeonGenerationStyleData style = ((DungeonWindowMosaicSettings)settings).Style;
		if (Window(data, x, y, style, generating: true))
		{
			generated = true;
			return true;
		}
		return false;
	}

	public bool Window(DungeonData data, int placeX, int placeY, DungeonGenerationStyleData style, bool generating = false)
	{
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		_ = WorldGen.genRand;
		DungeonWindowMosaicSettings dungeonWindowMosaicSettings = (DungeonWindowMosaicSettings)settings;
		int overrideGlassPaint = dungeonWindowMosaicSettings.OverrideGlassPaint;
		ushort glassWallType = (dungeonWindowMosaicSettings.Closed ? style.WindowClosedGlassWallType : style.WindowGlassWallType);
		ushort windowEdgeWallType = style.WindowEdgeWallType;
		if (dungeonWindowMosaicSettings.OverrideGlassType > 0)
		{
			glassWallType = (ushort)dungeonWindowMosaicSettings.OverrideGlassType;
		}
		Bounds.SetBounds(placeX, placeY, placeX + 1, placeY + 1);
		switch (dungeonWindowMosaicSettings.MosaicType)
		{
		case WindowType.SkeletronMosaic:
			Window_Skeletron(placeX, placeY, glassWallType, windowEdgeWallType, (byte)((overrideGlassPaint > 0) ? ((byte)overrideGlassPaint) : 0));
			break;
		case WindowType.MoonLordMosaic:
			Window_MoonLord(placeX, placeY, glassWallType, windowEdgeWallType, (byte)((overrideGlassPaint > 0) ? ((byte)overrideGlassPaint) : 0));
			break;
		}
		Bounds.CalculateHitbox();
		return true;
	}

	public void Window_Skeletron(int placeX, int placeY, ushort glassWallType, ushort edgeWallType, byte paint = 0)
	{
		//IL_0004: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0385: Unknown result type (might be due to invalid IL or missing references)
		//IL_038d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0398: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_04be: Unknown result type (might be due to invalid IL or missing references)
		//IL_0461: Unknown result type (might be due to invalid IL or missing references)
		//IL_0617: Unknown result type (might be due to invalid IL or missing references)
		Point val = new Point(placeX, placeY);
		int num = 17;
		int num2 = num / 2;
		int num3 = 15;
		int num4 = num3 / 2;
		int num5 = 11;
		int num6 = num5 / 2;
		int num7 = 7;
		int num8 = num7 / 2;
		for (int i = 0; i < num; i++)
		{
			int num9 = val.X + i - num2;
			for (int j = 0; j < num3; j++)
			{
				if (Skeletron_ValidSkullSpot(i, j, num, num3))
				{
					int num10 = val.Y + j - num4;
					Tile tile = Main.tile[num9, num10];
					tile.wall = glassWallType;
					if (paint > 0)
					{
						tile.wallColor(paint);
					}
					else
					{
						tile.wallColor(0);
					}
					if (!Skeletron_ValidSkullSpot(i - 1, j, num, num3))
					{
						Main.tile[num9 - 1, num10].wall = edgeWallType;
						Main.tile[num9 - 1, num10].wallColor(0);
					}
					if (!Skeletron_ValidSkullSpot(i + 1, j, num, num3))
					{
						Main.tile[num9 + 1, num10].wall = edgeWallType;
						Main.tile[num9 + 1, num10].wallColor(0);
					}
					if (!Skeletron_ValidSkullSpot(i, j - 1, num, num3))
					{
						Main.tile[num9, num10 - 1].wall = edgeWallType;
						Main.tile[num9, num10 - 1].wallColor(0);
					}
					if (j < num3 - 1 && !Skeletron_ValidSkullSpot(i, j + 1, num, num3))
					{
						Main.tile[num9, num10 + 1].wall = edgeWallType;
						Main.tile[num9, num10 + 1].wallColor(0);
					}
				}
			}
		}
		for (int k = 0; k < num5; k++)
		{
			int num11 = val.X + k - num6;
			for (int l = 0; l < num7; l++)
			{
				int num12 = val.Y + l + num4 + num8 - 2;
				Tile tile2 = Main.tile[num11, num12];
				if (Skeletron_ValidJawSpot(tile2, glassWallType, k, l, num5, num7))
				{
					tile2.wall = glassWallType;
					if (paint > 0)
					{
						tile2.wallColor(paint);
					}
					else
					{
						tile2.wallColor(0);
					}
					if (!Skeletron_ValidJawSpot(Main.tile[num11 - 1, num12], glassWallType, k - 1, l, num5, num7))
					{
						Main.tile[num11 - 1, num12].wall = edgeWallType;
						Main.tile[num11 - 1, num12].wallColor(0);
					}
					if (!Skeletron_ValidJawSpot(Main.tile[num11 + 1, num12], glassWallType, k + 1, l, num5, num7))
					{
						Main.tile[num11 + 1, num12].wall = edgeWallType;
						Main.tile[num11 + 1, num12].wallColor(0);
					}
					if (!Skeletron_ValidJawSpot(Main.tile[num11, num12 - 1], glassWallType, k, l - 1, num5, num7))
					{
						Main.tile[num11, num12 - 1].wall = edgeWallType;
						Main.tile[num11, num12 - 1].wallColor(0);
					}
					if (!Skeletron_ValidJawSpot(Main.tile[num11, num12 + 1], glassWallType, k, l + 1, num5, num7))
					{
						Main.tile[num11, num12 + 1].wall = edgeWallType;
						Main.tile[num11, num12 + 1].wallColor(0);
					}
				}
			}
		}
		Point val2 = new Point(val.X - num2, val.Y + num4 - 2);
		for (int m = 0; m < num; m++)
		{
			int num13 = val2.X + m;
			if (m >= 2 && m <= 5)
			{
				int num14 = m - 2;
				for (int n = 0; n < 6; n++)
				{
					if ((num14 != 3 || n > 1) && (num14 != 2 || n != 0) && (num14 != 1 || n != 5) && (num14 != 0 || n < 4))
					{
						Tile tile3 = Main.tile[num13, val.Y + n - 1];
						tile3.wall = edgeWallType;
						tile3.wallColor(0);
					}
				}
			}
			if (m >= 11 && m <= 14)
			{
				int num15 = m - 11;
				for (int num16 = 0; num16 < 6; num16++)
				{
					if ((num15 != 0 || num16 > 1) && (num15 != 1 || num16 != 0) && (num15 != 2 || num16 != 5) && (num15 != 3 || num16 < 4))
					{
						Tile tile4 = Main.tile[num13, val.Y + num16 - 1];
						tile4.wall = edgeWallType;
						tile4.wallColor(0);
					}
				}
			}
			if (m >= 7 && m <= 9)
			{
				int num17 = m - 7;
				for (int num18 = 0; num18 < 4; num18++)
				{
					if (((num17 != 0 && num17 != 2) || num18 != 0) && (num17 != 1 || num18 != 3))
					{
						Tile tile5 = Main.tile[num13, val.Y + num18 + 3];
						tile5.wall = edgeWallType;
						tile5.wallColor(0);
					}
				}
			}
			int num19 = val2.Y + 1;
			switch (m)
			{
			case 2:
			case 3:
				num19++;
				break;
			case 4:
			case 5:
			case 6:
				num19 += 2;
				break;
			case 7:
			case 8:
			case 9:
				num19 += 3;
				break;
			case 10:
			case 11:
			case 12:
				num19 += 2;
				break;
			case 13:
			case 14:
				num19++;
				break;
			}
			Tile tile6 = Main.tile[num13, num19];
			tile6.wall = edgeWallType;
			tile6.wallColor(0);
			if (m == 0 || m == num - 1)
			{
				Tile tile7 = Main.tile[num13, num19 - 1];
				Tile tile8 = Main.tile[num13, num19 + 1];
				tile7.wall = (tile8.wall = edgeWallType);
				tile7.wallColor(0);
				tile8.wallColor(0);
			}
			if (m == 4 || m == 6 || m == 8 || m == 10 || m == 12)
			{
				for (int num20 = 0; num20 < 4; num20++)
				{
					Tile tile9 = Main.tile[num13, num19 + num20];
					tile9.wall = edgeWallType;
					tile9.wallColor(0);
				}
			}
			if (m >= 5 && m <= 11)
			{
				int num21 = val2.Y + 7;
				if (m >= 7 && m <= 9)
				{
					num21++;
				}
				Tile tile10 = Main.tile[num13, num21];
				tile10.wall = edgeWallType;
				tile10.wallColor(0);
			}
		}
	}

	private bool Skeletron_ValidSkullSpot(int x, int y, int width, int height)
	{
		if (x < 0 || y < 0 || x >= width || y >= height)
		{
			return false;
		}
		if (y == 0 && (x <= 5 || x >= width - 6))
		{
			return false;
		}
		if (y == 1 && (x <= 3 || x >= width - 4))
		{
			return false;
		}
		if (y == 2 && (x <= 1 || x >= width - 2))
		{
			return false;
		}
		if (y == 3 && (x == 0 || x >= width - 1))
		{
			return false;
		}
		if ((x == 0 && y >= height - 2) || (x <= 1 && y == height - 1) || (x == width - 1 && y >= height - 2) || (x >= width - 2 && y == height - 1))
		{
			return false;
		}
		return true;
	}

	private bool Skeletron_ValidJawSpot(Tile tile, int glassWallType, int x, int y, int width, int height)
	{
		if (tile.wall == glassWallType)
		{
			return true;
		}
		if (x < 0 || y < 0 || x >= width || y >= height)
		{
			return false;
		}
		if (y == height - 1 && (x <= 2 || x >= width - 3))
		{
			return false;
		}
		if (y == height - 2 && (x <= 1 || x >= width - 2))
		{
			return false;
		}
		if (y == height - 3 && (x == 0 || x == width - 1))
		{
			return false;
		}
		return true;
	}

	public void Window_MoonLord(int placeX, int placeY, ushort glassWallType, ushort edgeWallType, byte paint = 0)
	{
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0462: Unknown result type (might be due to invalid IL or missing references)
		//IL_0473: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0254: Unknown result type (might be due to invalid IL or missing references)
		//IL_0270: Unknown result type (might be due to invalid IL or missing references)
		int num = 8;
		int num2 = num - 1;
		int num3 = 7;
		int num4 = num3 - 1;
		int num5 = 7;
		int num6 = num5 + 1;
		int num7 = 9;
		Point val = new Point(placeX, placeY);
		for (int i = 0; i < 2; i++)
		{
			bool flag = i == 0;
			for (int j = 0; j < num; j++)
			{
				for (int k = 0; k < num2; k++)
				{
					if (MoonLord_ValidSideEyeSpot(j, k, flag, num, num2))
					{
						int num8 = val.X + j + (flag ? (-num7 - 1) : (num7 - num + 2));
						int num9 = val.Y + k + num2 - 2;
						Bounds.UpdateBounds(num8, num9);
						Main.tile[num8, num9].wall = glassWallType;
						if (paint > 0)
						{
							Main.tile[num8, num9].wallColor(paint);
						}
						else
						{
							Main.tile[num8, num9].wallColor(0);
						}
						if (!MoonLord_ValidSideEyeSpot(j - 1, k, flag, num, num2))
						{
							Bounds.UpdateBounds(num8 - 1, num9);
							Main.tile[num8 - 1, num9].wall = edgeWallType;
							Main.tile[num8 - 1, num9].wallColor(0);
						}
						if (!MoonLord_ValidSideEyeSpot(j + 1, k, flag, num, num2))
						{
							Bounds.UpdateBounds(num8 + 1, num9);
							Main.tile[num8 + 1, num9].wall = edgeWallType;
							Main.tile[num8 + 1, num9].wallColor(0);
						}
						if (!MoonLord_ValidSideEyeSpot(j, k - 1, flag, num, num2))
						{
							Bounds.UpdateBounds(num8, num9 - 1);
							Main.tile[num8, num9 - 1].wall = edgeWallType;
							Main.tile[num8, num9 - 1].wallColor(0);
						}
						if (!MoonLord_ValidSideEyeSpot(j, k + 1, flag, num, num2))
						{
							Bounds.UpdateBounds(num8, num9 + 1);
							Main.tile[num8, num9 + 1].wall = edgeWallType;
							Main.tile[num8, num9 + 1].wallColor(0);
						}
					}
				}
			}
			for (int l = 0; l < num3; l++)
			{
				for (int m = 0; m < num4; m++)
				{
					if (MoonLord_ValidSideEyeSpot(l, m, flag, num3, num4))
					{
						int num10 = val.X + l + (flag ? (-num7 + 1) : (num7 - num3));
						int num11 = val.Y + m - num4 + 2;
						Bounds.UpdateBounds(num10 - 1, num11);
						Main.tile[num10, num11].wall = glassWallType;
						if (paint > 0)
						{
							Main.tile[num10, num11].wallColor(paint);
						}
						else
						{
							Main.tile[num10, num11].wallColor(0);
						}
						if (!MoonLord_ValidSideEyeSpot(l - 1, m, flag, num3, num4))
						{
							Bounds.UpdateBounds(num10 - 1, num11);
							Main.tile[num10 - 1, num11].wall = edgeWallType;
							Main.tile[num10 - 1, num11].wallColor(0);
						}
						if (!MoonLord_ValidSideEyeSpot(l + 1, m, flag, num3, num4))
						{
							Bounds.UpdateBounds(num10 + 1, num11);
							Main.tile[num10 + 1, num11].wall = edgeWallType;
							Main.tile[num10 + 1, num11].wallColor(0);
						}
						if (!MoonLord_ValidSideEyeSpot(l, m - 1, flag, num3, num4))
						{
							Bounds.UpdateBounds(num10, num11 - 1);
							Main.tile[num10, num11 - 1].wall = edgeWallType;
							Main.tile[num10, num11 - 1].wallColor(0);
						}
						if (!MoonLord_ValidSideEyeSpot(l, m + 1, flag, num3, num4))
						{
							Bounds.UpdateBounds(num10, num11 + 1);
							Main.tile[num10, num11 + 1].wall = edgeWallType;
							Main.tile[num10, num11 + 1].wallColor(0);
						}
					}
				}
			}
		}
		for (int n = 0; n < num5; n++)
		{
			for (int num12 = 0; num12 < num6; num12++)
			{
				if (MoonLord_ValidMidEyeSpot(n, num12, num5, num6))
				{
					int num13 = val.X + n - num5 / 2;
					int num14 = val.Y + num12 - num4 - num6;
					Bounds.UpdateBounds(num13, num14);
					Main.tile[num13, num14].wall = glassWallType;
					if (paint > 0)
					{
						Main.tile[num13, num14].wallColor(paint);
					}
					else
					{
						Main.tile[num13, num14].wallColor(0);
					}
					if (!MoonLord_ValidMidEyeSpot(n - 1, num12, num5, num6))
					{
						Bounds.UpdateBounds(num13 - 1, num14);
						Main.tile[num13 - 1, num14].wall = edgeWallType;
						Main.tile[num13 - 1, num14].wallColor(0);
					}
					if (!MoonLord_ValidMidEyeSpot(n + 1, num12, num5, num6))
					{
						Bounds.UpdateBounds(num13 + 1, num14);
						Main.tile[num13 + 1, num14].wall = edgeWallType;
						Main.tile[num13 + 1, num14].wallColor(0);
					}
					if (!MoonLord_ValidMidEyeSpot(n, num12 - 1, num5, num6))
					{
						Bounds.UpdateBounds(num13, num14 - 1);
						Main.tile[num13, num14 - 1].wall = edgeWallType;
						Main.tile[num13, num14 - 1].wallColor(0);
					}
					if (!MoonLord_ValidMidEyeSpot(n, num12 + 1, num5, num6))
					{
						Bounds.UpdateBounds(num13, num14 + 1);
						Main.tile[num13, num14 + 1].wall = edgeWallType;
						Main.tile[num13, num14 + 1].wallColor(0);
					}
				}
			}
		}
	}

	private bool MoonLord_ValidSideEyeSpot(int x, int y, bool leftEye, int width, int height)
	{
		if (x < 0 || y < 0 || x >= width || y >= height)
		{
			return false;
		}
		if (leftEye && ((x <= 1 && y == height - 1) || (x == width - 1 && y <= 1)))
		{
			return false;
		}
		if (leftEye && ((x == 0 && y >= height - 2) || (x >= width - 2 && y == 0)))
		{
			return false;
		}
		if (!leftEye && ((x <= 1 && y == 0) || (x >= width - 2 && y == height - 1)))
		{
			return false;
		}
		if (!leftEye && ((x == 0 && y <= 1) || (x == width - 1 && y >= height - 2)))
		{
			return false;
		}
		return true;
	}

	private bool MoonLord_ValidMidEyeSpot(int x, int y, int width, int height)
	{
		if (x < 0 || y < 0 || x >= width || y >= height)
		{
			return false;
		}
		if ((y == 1 && (x == 0 || x == width - 1)) || (y == height - 2 && (x == 0 || x == width - 1)))
		{
			return false;
		}
		if (y == 0 && ((x >= 0 && x <= 1) || (x >= width - 2 && x <= width - 1)))
		{
			return false;
		}
		if (y == height - 1 && ((x >= 0 && x <= 1) || (x >= width - 2 && x <= width - 1)))
		{
			return false;
		}
		return true;
	}
}
