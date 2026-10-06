using Terraria.DataStructures;
using Terraria.Utilities;

namespace Terraria.GameContent.Generation.Dungeon.Features;

public class DungeonWindowBasic : DungeonWindow
{
	public DungeonWindowBasic(DungeonFeatureSettings settings)
		: base(settings)
	{
	}

	public override bool GenerateFeature(DungeonData data, int x, int y)
	{
		generated = false;
		DungeonGenerationStyleData style = ((DungeonWindowBasicSettings)settings).Style;
		if (Window(data, x, y, style, generating: true))
		{
			generated = true;
			return true;
		}
		return false;
	}

	public bool Window(DungeonData data, int placeX, int placeY, DungeonGenerationStyleData style, bool generating = false)
	{
		//IL_0334: Unknown result type (might be due to invalid IL or missing references)
		UnifiedRandom genRand = WorldGen.genRand;
		DungeonWindowBasicSettings dungeonWindowBasicSettings = (DungeonWindowBasicSettings)settings;
		int width = dungeonWindowBasicSettings.Width;
		int height = dungeonWindowBasicSettings.Height;
		int overrideGlassPaint = dungeonWindowBasicSettings.OverrideGlassPaint;
		ushort wall = (dungeonWindowBasicSettings.Closed ? style.WindowClosedGlassWallType : style.WindowGlassWallType);
		ushort windowEdgeWallType = style.WindowEdgeWallType;
		int num = style.GetWindowPlatformStyle(genRand);
		if (dungeonWindowBasicSettings.OverrideGlassType > 0)
		{
			wall = (ushort)dungeonWindowBasicSettings.OverrideGlassType;
		}
		if (dungeonWindowBasicSettings.OverridePlatformStyle > -1)
		{
			num = dungeonWindowBasicSettings.OverridePlatformStyle;
		}
		Bounds.SetBounds(placeX, placeY, placeX, placeY);
		for (int i = 0; i < width; i++)
		{
			int num2 = placeX + i - width / 2;
			for (int j = 0; j < height; j++)
			{
				if (!Window_ValidWindowSpot(i, j, width, height))
				{
					continue;
				}
				int num3 = placeY + j - height / 2;
				if (i == width / 2 || j == height / 2)
				{
					Main.tile[num2, num3].wall = windowEdgeWallType;
					Main.tile[num2, num3].wallColor(0);
				}
				else
				{
					Main.tile[num2, num3].wall = wall;
					if (overrideGlassPaint > 0)
					{
						Main.tile[num2, num3].wallColor((byte)overrideGlassPaint);
					}
					else
					{
						Main.tile[num2, num3].wallColor(0);
					}
				}
				Bounds.UpdateBounds(num2, num3);
				if (!Window_ValidWindowSpot(i - 1, j, width, height))
				{
					Main.tile[num2 - 1, num3].wall = windowEdgeWallType;
					Main.tile[num2 - 1, num3].wallColor(0);
					Bounds.UpdateBounds(num2 - 1, num3);
				}
				if (!Window_ValidWindowSpot(i + 1, j, width, height))
				{
					Main.tile[num2 + 1, num3].wall = windowEdgeWallType;
					Main.tile[num2 + 1, num3].wallColor(0);
					Bounds.UpdateBounds(num2 + 1, num3);
				}
				if (!Window_ValidWindowSpot(i, j - 1, width, height))
				{
					Main.tile[num2, num3 - 1].wall = windowEdgeWallType;
					Main.tile[num2, num3 - 1].wallColor(0);
					Bounds.UpdateBounds(num2, num3 - 1);
				}
				if (!Window_ValidWindowSpot(i, j + 1, width, height))
				{
					Main.tile[num2, num3 + 1].wall = windowEdgeWallType;
					Main.tile[num2, num3 + 1].wallColor(0);
					Bounds.UpdateBounds(num2, num3 + 1);
					if (num > -1)
					{
						Main.tile[num2, num3 + 1].active(active: true);
						Main.tile[num2, num3 + 1].type = 19;
						Main.tile[num2, num3 + 1].Clear(TileDataType.Slope);
						Main.tile[num2, num3 + 1].frameY = (short)(num * 18);
						Main.tile[num2, num3 + 1].color(0);
						WorldGen.TileFrame(num2, num3 + 1);
					}
				}
			}
		}
		Bounds.CalculateHitbox();
		return true;
	}

	private bool Window_ValidWindowSpot(int x, int y, int width, int height)
	{
		if (x < 0 || y < 0 || x >= width || y >= height)
		{
			return false;
		}
		if (y == 0 && (x == 0 || x == width - 1))
		{
			return false;
		}
		return true;
	}
}
