namespace Terraria.GameContent.Generation.Dungeon.Features;

public class DungeonPillar : DungeonFeature
{
	public DungeonPillar(DungeonFeatureSettings settings)
		: base(settings)
	{
		DungeonCrawler.CurrentDungeonData.dungeonFeatures.Add(this);
	}

	public override bool GenerateFeature(DungeonData data, int x, int y)
	{
		generated = false;
		DungeonGenerationStyleData style = ((DungeonPillarSettings)settings).Style;
		if (Pillar(data, x, y, style, generating: true))
		{
			generated = true;
			return true;
		}
		return false;
	}

	public override bool CanGenerateFeatureAt(DungeonData data, IDungeonFeature feature, int x, int y)
	{
		return true;
	}

	public bool Pillar(DungeonData data, int i, int j, DungeonGenerationStyleData styleData, bool generating = false)
	{
		//IL_0145: Unknown result type (might be due to invalid IL or missing references)
		_ = WorldGen.genRand;
		DungeonPillarSettings dungeonPillarSettings = (DungeonPillarSettings)settings;
		int width = dungeonPillarSettings.Width;
		int height = dungeonPillarSettings.Height;
		bool crowningOnTop = dungeonPillarSettings.CrowningOnTop;
		bool crowningOnBottom = dungeonPillarSettings.CrowningOnBottom;
		bool crowningStopsAtPillar = dungeonPillarSettings.CrowningStopsAtPillar;
		int num = 3;
		int topY = 0;
		Bounds.SetBounds(i, j, i, j);
		_ = width / 2;
		for (int k = 0; k < width; k++)
		{
			int num2 = i + k - width / 2;
			int topY2 = j;
			int bottomY = j;
			GenerateTileStrip(dungeonPillarSettings, upwards: true, out topY2, out bottomY, num2, j, height, styleData, smoothTop: false, smoothBottom: false);
			Bounds.UpdateBounds(num2, topY2, num2, bottomY);
			if (crowningOnTop)
			{
				int pillarHeight = (crowningStopsAtPillar ? (num + 1) : 0);
				if (k == 0)
				{
					GenerateTileStrip(dungeonPillarSettings, upwards: true, out topY, out topY, num2 - 1, topY2 + num, pillarHeight, styleData, smoothTop: false, smoothBottom: true);
				}
				else if (k == width - 1)
				{
					GenerateTileStrip(dungeonPillarSettings, upwards: true, out topY, out topY, num2 + 1, topY2 + num, pillarHeight, styleData, smoothTop: false, smoothBottom: true);
				}
			}
			if (crowningOnBottom)
			{
				int pillarHeight2 = (crowningStopsAtPillar ? (num + 1) : 0);
				if (k == 0)
				{
					GenerateTileStrip(dungeonPillarSettings, upwards: false, out topY, out topY, num2 - 1, bottomY - num, pillarHeight2, styleData, smoothTop: true, smoothBottom: false);
				}
				else if (k == width - 1)
				{
					GenerateTileStrip(dungeonPillarSettings, upwards: false, out topY, out topY, num2 + 1, bottomY - num, pillarHeight2, styleData, smoothTop: true, smoothBottom: false);
				}
			}
		}
		Bounds.CalculateHitbox();
		return true;
	}

	public static void GenerateTileStrip(DungeonPillarSettings pillarSettings, bool upwards, out int topY, out int bottomY, int placeX, int placeY, int pillarHeight, DungeonGenerationStyleData styleData, bool smoothTop, bool smoothBottom)
	{
		ushort brickTileType = styleData.BrickTileType;
		ushort brickWallType = styleData.BrickWallType;
		byte? tilePaintColor = styleData.TilePaintColor;
		byte? wallPaintColor = styleData.WallPaintColor;
		PillarType pillarType = pillarSettings.PillarType;
		ushort num = ((pillarType == PillarType.Wall) ? brickWallType : brickTileType);
		bool flag = pillarType == PillarType.Wall;
		byte? b = (flag ? wallPaintColor : tilePaintColor);
		if (flag && pillarSettings.OverridePaintWall >= 0)
		{
			b = (byte)pillarSettings.OverridePaintWall;
		}
		if (!flag && pillarSettings.OverridePaintTile >= 0)
		{
			b = (byte)pillarSettings.OverridePaintTile;
		}
		bool flag2 = pillarType == PillarType.BlockActuatedSolidTop || pillarType == PillarType.BlockActuatedSolidTopAndBottom;
		bool flag3 = pillarType == PillarType.BlockActuatedSolidBottom || pillarType == PillarType.BlockActuatedSolidTopAndBottom;
		bool flag4 = pillarType == PillarType.BlockActuated || pillarType == PillarType.BlockActuatedSolidTop || pillarType == PillarType.BlockActuatedSolidBottom || pillarType == PillarType.BlockActuatedSolidTopAndBottom;
		int num2 = pillarHeight;
		if (num2 == 0)
		{
			num2 = 0;
			int i = 0;
			if (upwards)
			{
				while (i > -100 && WorldGen.InWorld(placeX, placeY + i, 10) && !Main.tile[placeX, placeY + i].active())
				{
					i--;
				}
				num2 = -i;
			}
			else
			{
				for (; i < 100 && WorldGen.InWorld(placeX, placeY + i, 10) && !Main.tile[placeX, placeY + i].active(); i++)
				{
				}
				num2 = i;
				placeY += num2 - 1;
			}
		}
		topY = placeY;
		bottomY = placeY;
		if (num2 == 0)
		{
			return;
		}
		int num3 = -num2 + 1;
		int num4 = 0;
		if (upwards)
		{
			for (int j = num3; j <= num4; j++)
			{
				int num5 = placeY + j;
				if (num5 <= 10)
				{
					break;
				}
				Tile tile = Main.tile[placeX, num5];
				if (!pillarSettings.AlwaysPlaceEntirePillar && tile.active())
				{
					break;
				}
				if (flag)
				{
					tile.wall = num;
					if (b.HasValue && b.Value >= 0)
					{
						tile.wallColor(b.Value);
					}
				}
				else
				{
					tile.ClearTile();
					tile.active(active: true);
					tile.type = num;
					if (b.HasValue && b.Value >= 0)
					{
						tile.color(b.Value);
					}
					if (((j == num3) & smoothTop) || ((j == num4) & smoothBottom))
					{
						Tile.SmoothSlope(placeX, num5, applyToNeighbors: false);
					}
					if ((!flag2 || j >= num3 + 2) && (!flag3 || j <= num4 - 2) && flag4)
					{
						tile.inActive(inActive: true);
					}
				}
				if (num5 < topY)
				{
					topY = num5;
				}
				if (num5 > bottomY)
				{
					bottomY = num5;
				}
			}
			return;
		}
		for (int num6 = num4; num6 >= num3; num6--)
		{
			int num7 = placeY + num6;
			if (num7 >= Main.maxTilesY - 10)
			{
				break;
			}
			Tile tile2 = Main.tile[placeX, num7];
			if (!pillarSettings.AlwaysPlaceEntirePillar && tile2.active())
			{
				break;
			}
			if (flag)
			{
				tile2.wall = num;
				if (b.HasValue && b.Value >= 0)
				{
					tile2.wallColor(b.Value);
				}
			}
			else
			{
				tile2.ClearTile();
				tile2.active(active: true);
				tile2.type = num;
				if (b.HasValue && b.Value >= 0)
				{
					tile2.color(b.Value);
				}
				if (((num6 == num3) & smoothTop) || ((num6 == num4) & smoothBottom))
				{
					Tile.SmoothSlope(placeX, num7, applyToNeighbors: false);
				}
				if ((!flag2 || num6 >= num3 + 2) && (!flag3 || num6 <= num4 - 2) && flag4)
				{
					tile2.inActive(inActive: true);
				}
			}
			if (num7 < topY)
			{
				topY = num7;
			}
			if (num7 > bottomY)
			{
				bottomY = num7;
			}
		}
	}
}
