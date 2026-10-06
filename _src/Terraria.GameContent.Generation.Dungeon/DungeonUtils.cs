using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using ReLogic.Utilities;
using Terraria.DataStructures;
using Terraria.GameContent.Generation.Dungeon.Features;
using Terraria.GameContent.Generation.Dungeon.Rooms;
using Terraria.ID;
using Terraria.Utilities;
using Terraria.WorldBuilding;

namespace Terraria.GameContent.Generation.Dungeon;

public class DungeonUtils
{
	public delegate ConnectionPointQuality GetHallwayConnectionPoint(DungeonRoom room, Vector2D otherRoomPos, out Vector2D connectionPoint);

	public const int DOORSTYLE_WOODEN = 13;

	public const int DOORSTYLE_BLUEBRICK = 16;

	public const int DOORSTYLE_GREENBRICK = 17;

	public const int DOORSTYLE_PINKBRICK = 18;

	public const int DOORSTYLE_GOTHIC = 53;

	public const int POTSTYLE_NORMAL_1 = 0;

	public const int POTSTYLE_NORMAL_2 = 1;

	public const int POTSTYLE_NORMAL_3 = 2;

	public const int POTSTYLE_NORMAL_4 = 3;

	public const int POTSTYLE_SKULL_1 = 10;

	public const int POTSTYLE_SKULL_2 = 11;

	public const int POTSTYLE_SKULL_3 = 12;

	public const int PLATFORMSTYLE_BLUEBRICK = 6;

	public const int PLATFORMSTYLE_GREENBRICK = 8;

	public const int PLATFORMSTYLE_PINKBRICK = 7;

	public const int PLATFORMSTYLE_METALSHELF = 9;

	public const int PLATFORMSTYLE_BRASSSHELF = 10;

	public const int PLATFORMSTYLE_WOODSHELF = 11;

	public const int PLATFORMSTYLE_DUNGEONSHELF = 12;

	public const int PLATFORMSTYLE_GOTHIC = 54;

	public const int BANNERSTYLE_BRICK_MARCHINGBONES = 10;

	public const int BANNERSTYLE_BRICK_NECROMANTICSIGN = 11;

	public const int BANNERSTYLE_SLAB_RUGGEDCOMPANY = 12;

	public const int BANNERSTYLE_SLAB_RAGGEDBROTHERHOOD = 13;

	public const int BANNERSTYLE_TILES_MOLTENLEGION = 14;

	public const int BANNERSTYLE_TILES_DIABOLICSIGIL = 15;

	public const int TRAPTYPE_DART = 0;

	public const int DUNGEONHALL_DEFAULT_INNER_AREA_DEPTH = 3;

	public const int DUNGEONHALL_DEFAULT_OUTER_WALL_DEPTH = 8;

	public const int DUNGEONROOM_DEFAULT_INNER_AREA_DEPTH = 6;

	public const int DUNGEONROOM_DEFAULT_OUTER_WALL_DEPTH = 8;

	public static void CalculatePlatformsAndDoorsOnEdgesOfRoom(DungeonData dungeonData, DungeonBounds innerBounds, DungeonGenerationStyleData styleData, int? doorFluff = null, int? platformFluff = null)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0120: Unknown result type (might be due to invalid IL or missing references)
		//IL_0125: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0299: Unknown result type (might be due to invalid IL or missing references)
		//IL_029e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0372: Unknown result type (might be due to invalid IL or missing references)
		//IL_0377: Unknown result type (might be due to invalid IL or missing references)
		UnifiedRandom genRand = WorldGen.genRand;
		if (styleData == null)
		{
			if (!WorldGen.InWorld(innerBounds.Center.X, innerBounds.Center.Y, 5))
			{
				return;
			}
			Tile tile = Main.tile[innerBounds.Center.X, innerBounds.Center.Y];
			styleData = DungeonGenerationStyles.GetStyleForWall(dungeonData.genVars.dungeonGenerationStyles, tile.wall);
			if (styleData == null && tile.active())
			{
				styleData = DungeonGenerationStyles.GetStyleForTile(dungeonData.genVars.dungeonGenerationStyles, tile.type);
			}
			if (styleData == null)
			{
				styleData = dungeonData.genVars.dungeonStyle;
			}
		}
		bool flag = styleData.Style == 0;
		int num = Math.Max(5, innerBounds.Left);
		int num2 = Math.Min(Main.maxTilesX - 5, innerBounds.Right);
		int num3 = Math.Max(5, innerBounds.Top);
		int num4 = Math.Min(Main.maxTilesY - 5, innerBounds.Bottom);
		bool flag2 = false;
		bool flag3 = false;
		for (int i = num; i <= num2; i++)
		{
			if (!flag2 && !Main.tile[i, num3 - 1].active())
			{
				DungeonPlatformData item = new DungeonPlatformData
				{
					Position = new Point(i, num3 - 1),
					InAHallway = false,
					OverrideStyle = styleData.GetPlatformStyle(genRand)
				};
				if (platformFluff.HasValue)
				{
					item.OverrideHeightFluff = platformFluff.Value;
				}
				dungeonData.dungeonPlatformData.Add(item);
				flag2 = true;
			}
			if (!flag3 && !Main.tile[i, num4 + 1].active())
			{
				DungeonPlatformData item2 = new DungeonPlatformData
				{
					Position = new Point(i, num4 + 1),
					InAHallway = false,
					OverrideStyle = styleData.GetPlatformStyle(genRand)
				};
				if (platformFluff.HasValue)
				{
					item2.OverrideHeightFluff = platformFluff.Value;
				}
				dungeonData.dungeonPlatformData.Add(item2);
				flag3 = true;
			}
			if (flag2 & flag3)
			{
				break;
			}
		}
		if (styleData.DoorItemTypes == null)
		{
			return;
		}
		int num5 = ((flag || styleData.DoorItemTypes.Length == 0) ? (-1) : styleData.DoorItemTypes[genRand.Next(styleData.DoorItemTypes.Length)]);
		bool flag4 = false;
		bool flag5 = false;
		for (int j = num3; j <= num4; j++)
		{
			if (!flag4 && !Main.tile[num - 1, j].active())
			{
				bool flag6 = doorFluff.HasValue && doorFluff.Value == 0;
				DungeonDoorData item3 = new DungeonDoorData
				{
					OverrideGenerationStyle = styleData,
					Position = new Point(num - 1, j),
					Direction = -1,
					InAHallway = false,
					SkipOtherDoorsCheck = flag6,
					SkipSpaceCheck = flag6,
					AlwaysClearArea = true
				};
				if (doorFluff.HasValue)
				{
					item3.OverrideWidthFluff = doorFluff.Value;
				}
				if (num5 >= 0)
				{
					PlacementDetails placementDetails = ItemID.Sets.DerivedPlacementDetails[num5];
					item3.OverrideStyle = placementDetails.tileStyle;
				}
				dungeonData.dungeonDoorData.Add(item3);
				flag4 = true;
			}
			if (!flag5 && !Main.tile[num2 + 1, j].active())
			{
				bool flag7 = doorFluff.HasValue && doorFluff.Value == 0;
				DungeonDoorData item4 = new DungeonDoorData
				{
					OverrideGenerationStyle = styleData,
					Position = new Point(num2 + 1, j),
					Direction = 1,
					InAHallway = false,
					SkipOtherDoorsCheck = flag7,
					SkipSpaceCheck = flag7,
					AlwaysClearArea = true
				};
				if (doorFluff.HasValue)
				{
					item4.OverrideWidthFluff = doorFluff.Value;
				}
				if (num5 >= 0)
				{
					PlacementDetails placementDetails2 = ItemID.Sets.DerivedPlacementDetails[num5];
					item4.OverrideStyle = placementDetails2.tileStyle;
				}
				dungeonData.dungeonDoorData.Add(item4);
				flag5 = true;
			}
			if (flag4 & flag5)
			{
				break;
			}
		}
	}

	public static void CalculatePlatformAndDoorsOnHallway(DungeonData dungeonData, Vector2D hallwayPoint, double hallwayDirectionY, DungeonGenerationStyleData styleData, double doorVariance = 0.1)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0140: Unknown result type (might be due to invalid IL or missing references)
		//IL_0141: Unknown result type (might be due to invalid IL or missing references)
		//IL_0146: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		UnifiedRandom genRand = WorldGen.genRand;
		if (styleData == null)
		{
			if (!WorldGen.InWorld((int)hallwayPoint.X, (int)hallwayPoint.Y, 5))
			{
				return;
			}
			Tile tile = Main.tile[(int)hallwayPoint.X, (int)hallwayPoint.Y];
			styleData = DungeonGenerationStyles.GetStyleForWall(dungeonData.genVars.dungeonGenerationStyles, tile.wall);
			if (styleData == null && tile.active())
			{
				styleData = DungeonGenerationStyles.GetStyleForTile(dungeonData.genVars.dungeonGenerationStyles, tile.type);
			}
			if (styleData == null)
			{
				styleData = dungeonData.genVars.dungeonStyle;
			}
		}
		bool flag = styleData.Style == 0;
		if (Math.Abs(hallwayDirectionY) <= doorVariance)
		{
			if (styleData.DoorItemTypes != null)
			{
				int num = ((flag || styleData.DoorItemTypes.Length == 0) ? (-1) : styleData.DoorItemTypes[genRand.Next(styleData.DoorItemTypes.Length)]);
				DungeonDoorData item = new DungeonDoorData
				{
					OverrideGenerationStyle = styleData,
					Position = hallwayPoint.ToPoint(),
					Direction = 0,
					InAHallway = true,
					AlwaysClearArea = true
				};
				if (num >= 0)
				{
					PlacementDetails placementDetails = ItemID.Sets.DerivedPlacementDetails[num];
					item.OverrideStyle = placementDetails.tileStyle;
				}
				dungeonData.dungeonDoorData.Add(item);
			}
		}
		else
		{
			DungeonPlatformData item2 = new DungeonPlatformData
			{
				Position = hallwayPoint.ToPoint(),
				InAHallway = true,
				OverrideStyle = styleData.GetPlatformStyle(genRand)
			};
			dungeonData.dungeonPlatformData.Add(item2);
		}
	}

	public static void GenerateShimmerPool(int x, int y, int outerShapeSize = 15)
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		int num = 5;
		int num2 = Math.Max(1, outerShapeSize - num);
		Shapes.HalfCircle shape = new Shapes.HalfCircle(outerShapeSize, bottomHalf: true);
		Shapes.HalfCircle shape2 = new Shapes.HalfCircle(num2, bottomHalf: true);
		Point val = new Point(x, y + num2);
		WorldUtils.Gen(val, shape, Actions.Chain(new Actions.SetTile(667, setSelfFrames: false, setNeighborFrames: false, clearTile: false)));
		WorldUtils.Gen(new Point(val.X, val.Y - num), shape2, Actions.Chain(new Actions.ClearTile(), new Actions.SetLiquid(3)));
	}

	public static bool GenerateDungeonBiomeChest(DungeonData data, DungeonGlobalBiomeChests feature, DungeonGenerationStyleData styleData, DungeonBounds innerBounds, bool locked = true)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		int left = innerBounds.Left;
		int right = innerBounds.Right;
		int y = innerBounds.Center.Y;
		int bottom = innerBounds.Bottom;
		return GenerateDungeonBiomeChest(data, feature, styleData, left, y, right, bottom, locked);
	}

	public static bool GenerateDungeonBiomeChest(DungeonData data, DungeonGlobalBiomeChests feature, DungeonGenerationStyleData styleData, int minX, int minY, int maxX, int maxY, bool locked = true)
	{
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		UnifiedRandom genRand = WorldGen.genRand;
		int num = (int)Utils.Lerp(minX, maxX, genRand.NextDouble());
		int num2 = (int)Utils.Lerp(minY, maxY, genRand.NextDouble());
		if (!data.CanGenerateFeatureInArea(feature, num, num2, 1))
		{
			return false;
		}
		int num3 = 0;
		ushort chestTileType = 21;
		int chestStyle = 2;
		if (styleData.BiomeChestLootItemType >= 0)
		{
			num3 = styleData.BiomeChestLootItemType;
		}
		if (styleData.BiomeChestItemType >= 0)
		{
			PlacementDetails placementDetails = ItemID.Sets.DerivedPlacementDetails[styleData.BiomeChestItemType];
			chestTileType = (ushort)placementDetails.tileType;
			chestStyle = placementDetails.tileStyle;
		}
		if (locked && styleData.LockedBiomeChestStyle >= 0)
		{
			chestStyle = styleData.LockedBiomeChestStyle;
		}
		if (num3 == 0)
		{
			return false;
		}
		Point chestLocation = Point.Zero;
		return WorldGen.AddBuriedChest(num, num2, out chestLocation, num3, notNearOtherChests: false, chestStyle, trySlope: false, chestTileType);
	}

	public static bool GenerateDungeonRegularChest(DungeonData data, DungeonGlobalBasicChests feature, DungeonGenerationStyleData styleData, DungeonBounds innerBounds)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		int left = innerBounds.Left;
		int right = innerBounds.Right;
		int y = innerBounds.Center.Y;
		int bottom = innerBounds.Bottom;
		return GenerateDungeonRegularChest(data, feature, styleData, left, y, right, bottom);
	}

	public static bool GenerateDungeonRegularChest(DungeonData data, DungeonGlobalBasicChests feature, DungeonGenerationStyleData styleData, int minX, int minY, int maxX, int maxY)
	{
		UnifiedRandom genRand = WorldGen.genRand;
		int num = (int)Utils.Lerp(minX, maxX, genRand.NextDouble());
		int num2 = (int)Utils.Lerp(minY, maxY, genRand.NextDouble());
		if (!data.CanGenerateFeatureInArea(feature, num, num2, 1))
		{
			return false;
		}
		int itemType = -1;
		ushort chestTileType = 21;
		int chestStyle = 2;
		bool flag = false;
		switch (styleData.Style)
		{
		case 0:
			WorldGen.GetDungeonLootAndChestStyle(num, num2, ref itemType, ref chestStyle);
			flag = true;
			break;
		case 8:
		case 9:
		case 14:
			itemType = WorldGen.GetNextJungleChestItem();
			break;
		case 10:
			itemType = 1293;
			break;
		case 13:
			itemType = 832;
			if (genRand.Next(3) == 0)
			{
				itemType = 4281;
			}
			break;
		}
		if (!flag && styleData.ChestItemTypes.Length != 0)
		{
			PlacementDetails placementDetails = ItemID.Sets.DerivedPlacementDetails[styleData.ChestItemTypes[genRand.Next(styleData.ChestItemTypes.Length)]];
			chestTileType = (ushort)placementDetails.tileType;
			chestStyle = placementDetails.tileStyle;
		}
		if (itemType == 0 && genRand.Next(2) == 0)
		{
			return true;
		}
		bool flag2 = WorldGen.AddBuriedChest(num, num2, itemType, notNearOtherChests: false, chestStyle, trySlope: false, chestTileType);
		if (flag2 && styleData.Style == 0)
		{
			GenVars.CurrentDungeonGenVars.dungeonLootStyle++;
		}
		return flag2;
	}

	public static void GenerateDungeonWaterCandle(int placeX, int placeY)
	{
		WorldGen.PlaceTile(placeX, placeY, 49, mute: true);
	}

	public static void GenerateDungeonPotionBottle(int placeX, int placeY)
	{
		WorldGen.PlaceTile(placeX, placeY, 13, mute: true);
		if (Main.tile[placeX, placeY].type == 13)
		{
			if (WorldGen.genRand.Next(2) == 0)
			{
				Main.tile[placeX, placeY].frameX = 18;
			}
			else
			{
				Main.tile[placeX, placeY].frameX = 36;
			}
		}
	}

	public static void GenerateDungeonPot(int placeX, int placeY)
	{
		int style = WorldGen.genRand.Next(10, 13);
		WorldGen.PlacePot(placeX, placeY, 28, style);
		WorldGen.SquareTileFrame(placeX, placeY);
	}

	public static void GenerateDungeonBook(int placeX, int placeY)
	{
		GenerateDungeonBook(placeX, placeY, WorldGen.genRand.Next(50) == 0);
	}

	public static void GenerateDungeonBook(int placeX, int placeY, bool waterbolt)
	{
		short frameX = 90;
		WorldGen.PlaceTile(placeX, placeY, 50, mute: true);
		if (waterbolt && (double)placeY > (Main.worldSurface + Main.rockLayer) / 2.0 && Main.tile[placeY, placeY].type == 50)
		{
			Main.tile[placeX, placeY].frameX = frameX;
		}
	}

	public static void GenerateBottomWedge(int placeX, int placeY, int pillarWidth, ushort pillarType, bool left = true, bool wall = false, bool actuated = false, bool crowningBottom = false, int? paint = -1)
	{
		if (crowningBottom)
		{
			pillarWidth += 2;
		}
		int topY = 0;
		for (int i = 0; i <= pillarWidth; i++)
		{
			int placeX2 = placeX + i - pillarWidth / 2;
			int pillarHeight = (left ? (i + 1) : (pillarWidth - (i - 1)));
			GenerateTileStrip(upwards: false, out topY, out topY, placeX2, placeY, pillarHeight, pillarType, wall, actuated, paint);
		}
		for (int j = 0; j <= pillarWidth; j++)
		{
			_ = pillarWidth / 2;
			int num = (left ? (j + 1) : (pillarWidth - (j - 1)));
			Tile.SmoothSlope(placeX, placeY + num, applyToNeighbors: false);
		}
	}

	private static void GenerateTileStrip(bool upwards, out int topY, out int bottomY, int placeX, int placeY, int pillarHeight, ushort pillarType, bool wall = false, bool actuated = false, int? paint = -1, bool smoothTop = false, bool smoothBottom = false, bool solidTop = false)
	{
		topY = placeY;
		bottomY = placeY;
		int num = pillarHeight;
		if (num == -1)
		{
			num = 0;
			int i = 0;
			if (upwards)
			{
				while (i > -100 && WorldGen.InWorld(placeX, placeY + i, 10) && !Main.tile[placeX, placeY + i].active())
				{
					i--;
				}
				num = -i;
			}
			else
			{
				for (; i < 100 && WorldGen.InWorld(placeX, placeY + i, 10) && !Main.tile[placeX, placeY + i].active(); i++)
				{
				}
				num = i;
			}
		}
		if (num == 0)
		{
			return;
		}
		int num2 = -num + 1;
		int num3 = 0;
		if (!upwards)
		{
			num2 = 0;
			num3 = num - 1;
		}
		for (int j = num2; j <= num3; j++)
		{
			int num4 = placeY + j;
			if (!WorldGen.InWorld(placeX, num4, 10))
			{
				continue;
			}
			Tile tile = Main.tile[placeX, num4];
			if (wall)
			{
				tile.wall = pillarType;
				if (paint.HasValue && paint.Value >= 0)
				{
					tile.wallColor((byte)paint.Value);
				}
			}
			else
			{
				tile.ClearTile();
				tile.active(active: true);
				tile.type = pillarType;
				if (paint.HasValue && paint.Value >= 0)
				{
					tile.color((byte)paint.Value);
				}
				if (((j == num2) & smoothTop) || ((j == num3) & smoothBottom))
				{
					Tile.SmoothSlope(placeX, num4, applyToNeighbors: false);
				}
				if ((!solidTop || j >= num2 + 2) && actuated)
				{
					tile.inActive(inActive: true);
				}
			}
			if (num4 < topY)
			{
				topY = num4;
			}
			if (num4 > bottomY)
			{
				bottomY = num4;
			}
		}
	}

	public static Point FirstSolid(bool ceiling, Point currentPoint, DungeonBounds bounds)
	{
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		int num = Main.maxTilesX * 5;
		do
		{
			num--;
			if (num <= 0)
			{
				break;
			}
			if (ceiling && WorldGen.SolidTileAllowTopSlope(currentPoint.X, currentPoint.Y))
			{
				return currentPoint;
			}
			if (!ceiling && WorldGen.SolidTileAllowBottomSlope(currentPoint.X, currentPoint.Y))
			{
				return currentPoint;
			}
			if (ceiling)
			{
				currentPoint.Y--;
			}
			else
			{
				currentPoint.Y++;
			}
		}
		while (currentPoint.Y > 10 && currentPoint.Y < Main.maxTilesY - 10 && (bounds == null || bounds.Contains(currentPoint)));
		return currentPoint;
	}

	public static void GenerateHangingLeafCluster(DungeonData data, UnifiedRandom genRand, DungeonBounds bounds, Point startPoint, int growthLength, int branchDensity, int leafDensity, ushort leafType, ushort woodType, int? leafPaintColor = 0, int? woodPaintColor = 0, bool goDown = true, bool includeVines = true)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_010e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0114: Unknown result type (might be due to invalid IL or missing references)
		//IL_0128: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0105: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_013b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0165: Unknown result type (might be due to invalid IL or missing references)
		Point currentPoint = new Point(startPoint.X, startPoint.Y);
		int num = (goDown ? 1 : (-1));
		currentPoint = FirstSolid(goDown, currentPoint, bounds);
		Point val = currentPoint;
		Point? val2 = null;
		int num2 = growthLength;
		while (growthLength > 0)
		{
			int y = currentPoint.Y;
			float num3 = (float)growthLength / (float)num2;
			if (!val2.HasValue && num3 > 0.65f)
			{
				val2 = currentPoint;
			}
			int num4 = (int)Utils.Lerp(0.0, (float)branchDensity, num3);
			for (int i = -num4; i <= num4; i++)
			{
				int num5 = currentPoint.X + i;
				ChangeTileType(Main.tile[num5, y], woodType, resetTile: false, woodPaintColor);
			}
			currentPoint.Y += num;
			growthLength--;
		}
		int num6 = genRand.Next(3);
		if (!goDown)
		{
			num6 *= -1;
		}
		currentPoint = ((!val2.HasValue) ? new Point(val.X, val.Y + (int)((float)num2 * 0.65f) + num6) : new Point(val2.Value.X, val2.Value.Y + num6));
		int num7 = leafDensity * 2 + 3;
		int num8 = num7;
		while (num7 > 0)
		{
			int y2 = currentPoint.Y;
			float percent = (float)num7 / (float)num8;
			int num9 = (int)Utils.WrappedLerp(1f, leafDensity, percent);
			for (int j = -num9; j <= num9; j++)
			{
				int num10 = currentPoint.X + j;
				Tile tile = Main.tile[num10, y2];
				if (!tile.active())
				{
					ChangeTileType(tile, leafType, resetTile: false, leafPaintColor);
				}
			}
			currentPoint.Y += num;
			num7--;
		}
	}

	public static void GenerateDungeonTree(DungeonData data, int x, int y, int attachmentY, bool generateRoots = true)
	{
		if (!WorldGen.InWorld(x, y, 20))
		{
			return;
		}
		int num = y;
		while (Main.tile[x, num].active() || Main.tile[x, num].wall > 0 || Main.tile[x, num - 1].active() || Main.tile[x, num - 1].wall > 0 || Main.tile[x, num - 2].active() || Main.tile[x, num - 2].wall > 0 || Main.tile[x, num - 3].active() || Main.tile[x, num - 3].wall > 0 || Main.tile[x, num - 4].active() || Main.tile[x, num - 4].wall > 0)
		{
			num--;
			if (num < 50)
			{
				break;
			}
		}
		if (num > 50)
		{
			GrowDungeonTree(data, x, num, attachmentY, patch: false, generateRoots);
		}
	}

	private static bool GrowDungeonTree(DungeonData data, int i, int j, int attachmentY, bool patch = false, bool generateRoots = true)
	{
		//IL_0c3c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c47: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c4c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c51: Unknown result type (might be due to invalid IL or missing references)
		byte b = 28;
		byte b2 = 28;
		if (WorldGen.notTheBees)
		{
			b = 0;
			b2 = 0;
		}
		UnifiedRandom genRand = WorldGen.genRand;
		int num = 0;
		int[] array = new int[1000];
		int[] array2 = new int[1000];
		int[] array3 = new int[1000];
		int[] array4 = new int[1000];
		int num2 = 0;
		int[] array5 = new int[2000];
		int[] array6 = new int[2000];
		bool[] array7 = new bool[2000];
		int num3 = i - genRand.Next(2, 3);
		int num4 = i + genRand.Next(2, 3);
		if (genRand.Next(5) == 0)
		{
			if (genRand.Next(2) == 0)
			{
				num3--;
			}
			else
			{
				num4++;
			}
		}
		int num5 = num4 - num3;
		int num6 = num3;
		int num7 = num4;
		int minimumLeft = num3;
		int minimumRight = num4;
		bool flag = true;
		int num8 = genRand.Next(-8, -4);
		int num9 = genRand.Next(2);
		int num10 = j;
		int num11 = genRand.Next(5, 15);
		Main.tileSolid[48] = false;
		while (flag)
		{
			num8++;
			if (num8 > num11)
			{
				num11 = genRand.Next(5, 15);
				num8 = 0;
				array2[num] = num10 + genRand.Next(5);
				if (genRand.Next(5) == 0)
				{
					num9 = ((num9 == 0) ? 1 : 0);
				}
				if (num9 == 0)
				{
					array3[num] = -1;
					array[num] = num3;
					array4[num] = num4 - num3;
					if (genRand.Next(2) == 0)
					{
						num3++;
					}
					num6++;
					num9 = 1;
				}
				else
				{
					array3[num] = 1;
					array[num] = num4;
					array4[num] = num4 - num3;
					if (genRand.Next(2) == 0)
					{
						num4--;
					}
					num7--;
					num9 = 0;
				}
				if (num6 == num7)
				{
					flag = false;
				}
				num++;
			}
			for (int k = num3; k <= num4; k++)
			{
				Main.tile[k, num10].type = 191;
				Main.tile[k, num10].active(active: true);
				Main.tile[k, num10].Clear(TileDataType.Slope);
				if (b != 0)
				{
					Main.tile[k, num10].color(b);
				}
			}
			num10--;
		}
		for (int l = 0; l < num - 1; l++)
		{
			int num12 = array[l] + array3[l];
			int num13 = array2[l];
			int num14 = (int)((double)array4[l] * (1.0 + (double)genRand.Next(20, 30) * 0.1));
			Main.tile[num12, num13 + 1].type = 191;
			Main.tile[num12, num13 + 1].active(active: true);
			Main.tile[num12, num13 + 1].Clear(TileDataType.Slope);
			if (b != 0)
			{
				Main.tile[num12, num13 + 1].color(b);
			}
			int num15 = genRand.Next(3, 5);
			while (num14 > 0)
			{
				num14--;
				Main.tile[num12, num13].type = 191;
				Main.tile[num12, num13].active(active: true);
				Main.tile[num12, num13].Clear(TileDataType.Slope);
				if (b != 0)
				{
					Main.tile[num12, num13].color(b);
				}
				if (genRand.Next(10) == 0)
				{
					num13 = ((genRand.Next(2) != 0) ? (num13 + 1) : (num13 - 1));
				}
				else
				{
					num12 += array3[l];
				}
				if (num15 > 0)
				{
					num15--;
				}
				else if (genRand.Next(2) == 0)
				{
					num15 = genRand.Next(2, 5);
					if (genRand.Next(2) == 0)
					{
						Main.tile[num12, num13].type = 191;
						Main.tile[num12, num13].active(active: true);
						Main.tile[num12, num13].Clear(TileDataType.Slope);
						if (b != 0)
						{
							Main.tile[num12, num13].color(b);
						}
						Main.tile[num12, num13 - 1].type = 191;
						Main.tile[num12, num13 - 1].Clear(TileDataType.Slope);
						if (b != 0)
						{
							Main.tile[num12, num13 - 1].color(b);
						}
						array5[num2] = num12;
						array6[num2] = num13;
						num2++;
					}
					else
					{
						Main.tile[num12, num13].type = 191;
						Main.tile[num12, num13].active(active: true);
						Main.tile[num12, num13].Clear(TileDataType.Slope);
						if (b != 0)
						{
							Main.tile[num12, num13].color(b);
						}
						Main.tile[num12, num13 + 1].type = 191;
						Main.tile[num12, num13 + 1].active(active: true);
						Main.tile[num12, num13 + 1].Clear(TileDataType.Slope);
						if (b != 0)
						{
							Main.tile[num12, num13 + 1].color(b);
						}
						array5[num2] = num12;
						array6[num2] = num13;
						num2++;
					}
				}
				if (num14 == 0)
				{
					array5[num2] = num12;
					array6[num2] = num13;
					num2++;
				}
			}
		}
		int num16 = (num3 + num4) / 2;
		int num17 = num10;
		int num18 = genRand.Next(num5 * 3, num5 * 5);
		int num19 = 0;
		int num20 = 0;
		while (num18 > 0)
		{
			Main.tile[num16, num17].type = 191;
			Main.tile[num16, num17].active(active: true);
			Main.tile[num16, num17].Clear(TileDataType.Slope);
			if (b != 0)
			{
				Main.tile[num16, num17].color(b);
			}
			if (num19 > 0)
			{
				num19--;
			}
			if (num20 > 0)
			{
				num20--;
			}
			for (int m = -1; m < 2; m++)
			{
				if (m == 0 || ((m >= 0 || num19 != 0) && (m <= 0 || num20 != 0)) || genRand.Next(2) != 0)
				{
					continue;
				}
				int num21 = num16;
				int num22 = num17;
				int num23 = genRand.Next(num5, num5 * 3);
				if (m < 0)
				{
					num19 = genRand.Next(3, 5);
				}
				if (m > 0)
				{
					num20 = genRand.Next(3, 5);
				}
				int num24 = 0;
				while (num23 > 0)
				{
					num23--;
					num21 += m;
					Main.tile[num21, num22].type = 191;
					Main.tile[num21, num22].active(active: true);
					Main.tile[num21, num22].Clear(TileDataType.Slope);
					if (b != 0)
					{
						Main.tile[num21, num22].color(b);
					}
					if (num23 == 0)
					{
						array5[num2] = num21;
						array6[num2] = num22;
						array7[num2] = true;
						num2++;
					}
					if (genRand.Next(5) == 0)
					{
						num22 = ((genRand.Next(2) != 0) ? (num22 + 1) : (num22 - 1));
						Main.tile[num21, num22].type = 191;
						Main.tile[num21, num22].active(active: true);
						Main.tile[num21, num22].Clear(TileDataType.Slope);
						if (b != 0)
						{
							Main.tile[num21, num22].color(b);
						}
					}
					if (num24 > 0)
					{
						num24--;
					}
					else if (genRand.Next(3) == 0)
					{
						num24 = genRand.Next(2, 4);
						int num25 = num21;
						int num26 = num22;
						num26 = ((genRand.Next(2) != 0) ? (num26 + 1) : (num26 - 1));
						Main.tile[num25, num26].type = 191;
						Main.tile[num25, num26].active(active: true);
						Main.tile[num25, num26].Clear(TileDataType.Slope);
						if (b != 0)
						{
							Main.tile[num25, num26].color(b);
						}
						array5[num2] = num25;
						array6[num2] = num26;
						array7[num2] = true;
						num2++;
						array5[num2] = num25 + genRand.Next(-5, 6);
						array6[num2] = num26 + genRand.Next(-5, 6);
						array7[num2] = true;
						num2++;
					}
				}
			}
			array5[num2] = num16;
			array6[num2] = num17;
			num2++;
			if (genRand.Next(4) == 0)
			{
				num16 = ((genRand.Next(2) != 0) ? (num16 + 1) : (num16 - 1));
				Main.tile[num16, num17].type = 191;
				Main.tile[num16, num17].active(active: true);
				Main.tile[num16, num17].Clear(TileDataType.Slope);
				if (b != 0)
				{
					Main.tile[num16, num17].color(b);
				}
			}
			num17--;
			num18--;
		}
		if (generateRoots)
		{
			for (int n = minimumLeft; n <= minimumRight; n++)
			{
				int num27 = genRand.Next(1, 6);
				int num28 = j + 1;
				while (num27 > 0)
				{
					if (WorldGen.SolidTile(n, num28))
					{
						num27--;
					}
					Main.tile[n, num28].type = 191;
					Main.tile[n, num28].active(active: true);
					Main.tile[n, num28].Clear(TileDataType.Slope);
					num28++;
				}
				int num29 = num28;
				int num30 = genRand.Next(2, num5 + 1);
				for (int num31 = 0; num31 < num30; num31++)
				{
					num28 = num29;
					int num32 = (minimumLeft + minimumRight) / 2;
					int num33 = 0;
					int num34 = 1;
					num33 = ((n >= num32) ? 1 : (-1));
					if (n == num32 || (num5 > 6 && (n == num32 - 1 || n == num32 + 1)))
					{
						num33 = 0;
					}
					int num35 = num33;
					int num36 = n;
					num27 = genRand.Next((int)((double)num5 * 3.5), num5 * 6);
					while (num27 > 0)
					{
						num27--;
						num36 += num33;
						if (Main.tile[num36, num28].wall != 244)
						{
							Main.tile[num36, num28].type = 191;
							Main.tile[num36, num28].active(active: true);
							Main.tile[num36, num28].Clear(TileDataType.Slope);
						}
						num28 += num34;
						if (Main.tile[num36, num28].wall != 244)
						{
							Main.tile[num36, num28].type = 191;
							Main.tile[num36, num28].active(active: true);
							Main.tile[num36, num28].Clear(TileDataType.Slope);
						}
						if (!Main.tile[num36, num28 + 1].active())
						{
							num33 = 0;
							num34 = 1;
						}
						if (genRand.Next(3) == 0)
						{
							if (num35 < 0)
							{
								num33 = ((num33 == 0) ? (-1) : 0);
							}
							else
							{
								num33 = ((num35 <= 0) ? genRand.Next(-1, 2) : ((num33 == 0) ? 1 : 0));
							}
						}
						if (genRand.Next(3) == 0)
						{
							num34 = ((num34 == 0) ? 1 : 0);
						}
					}
				}
			}
		}
		if (!WorldGen.remixWorldGen)
		{
			for (int num37 = 0; num37 < num2; num37++)
			{
				int num38 = genRand.Next(5, 8);
				num38 = (int)((double)num38 * (1.0 + (double)num5 * 0.05));
				if (array7[num37])
				{
					num38 = genRand.Next(6, 12) + num5;
				}
				int num39 = array5[num37] - num38 * 2;
				int num40 = array5[num37] + num38 * 2;
				int num41 = array6[num37] - num38 * 2;
				int num42 = array6[num37] + num38 * 2;
				double num43 = 2.0 - (double)genRand.Next(5) * 0.1;
				for (int num44 = num39; num44 <= num40; num44++)
				{
					for (int num45 = num41; num45 <= num42; num45++)
					{
						if (Main.tile[num44, num45].type == 191)
						{
							continue;
						}
						if (array7[num37])
						{
							Vector2D val = new Vector2D((double)array5[num37], (double)array6[num37]) - new Vector2D((double)num44, (double)num45);
							if (val.Length() < (double)num38 * 0.9)
							{
								Main.tile[num44, num45].type = 192;
								Main.tile[num44, num45].active(active: true);
								Main.tile[num44, num45].Clear(TileDataType.Slope);
								if (b2 != 0)
								{
									Main.tile[num44, num45].color(b2);
								}
							}
						}
						else if ((double)Math.Abs(array5[num37] - num44) + (double)Math.Abs(array6[num37] - num45) * num43 < (double)num38)
						{
							Main.tile[num44, num45].type = 192;
							Main.tile[num44, num45].active(active: true);
							Main.tile[num44, num45].Clear(TileDataType.Slope);
							if (b2 != 0)
							{
								Main.tile[num44, num45].color(b2);
							}
						}
					}
				}
			}
		}
		GrowDungeonTree_MakePassage(data, j, attachmentY, num5, ref minimumLeft, ref minimumRight, patch);
		Main.tileSolid[48] = true;
		return true;
	}

	private static void GrowDungeonTree_MakePassage(DungeonData data, int j, int attachmentY, int width, ref int minimumLeft, ref int minimumRight, bool noSecretRoom = false)
	{
		UnifiedRandom genRand = WorldGen.genRand;
		int num = minimumLeft;
		int num2 = minimumRight;
		_ = (minimumLeft + minimumRight) / 2;
		int num3 = 5;
		int num4 = j - 6;
		int num5 = 0;
		bool flag = true;
		genRand.Next(5, 16);
		PlacementDetails placementDetails = ItemID.Sets.DerivedPlacementDetails[data.platformItemType];
		int tileType = placementDetails.tileType;
		short tileStyle = placementDetails.tileStyle;
		while (true)
		{
			num4++;
			if (num4 > attachmentY)
			{
				break;
			}
			int num6 = (minimumLeft + minimumRight) / 2;
			int num7 = 1;
			if (num4 > j && width <= 4)
			{
				num7++;
			}
			for (int i = minimumLeft - num7; i <= minimumRight + num7; i++)
			{
				if (i > num6 - 2 && i <= num6 + 1)
				{
					if (num4 > j - 4)
					{
						if (Main.tile[i, num4].type != 19 && Main.tile[i, num4].type != 15 && Main.tile[i, num4].type != 304 && Main.tile[i, num4].type != 21 && Main.tile[i, num4].type != 10 && Main.tile[i, num4 - 1].type != 15 && Main.tile[i, num4 - 1].type != 304 && Main.tile[i, num4 - 1].type != 21 && Main.tile[i, num4 - 1].type != 10 && Main.tile[i, num4 + 1].type != 10)
						{
							Main.tile[i, num4].ClearTileAndPaint();
						}
						if (!IsConsideredDungeonWall(Main.tile[i, num4].wall))
						{
							Main.tile[i, num4].wall = 244;
						}
						if (!IsConsideredDungeonWall(Main.tile[i - 1, num4].wall) && (Main.tile[i - 1, num4].wall > 0 || (double)num4 >= Main.worldSurface))
						{
							Main.tile[i - 1, num4].wall = 244;
						}
						if (!IsConsideredDungeonWall(Main.tile[i + 1, num4].wall) && (Main.tile[i + 1, num4].wall > 0 || (double)num4 >= Main.worldSurface))
						{
							Main.tile[i + 1, num4].wall = 244;
						}
						if (num4 == j && i > num6 - 2 && i <= num6 + 1)
						{
							Main.tile[i, num4 + 1].ClearTileAndPaint();
							WorldGen.PlaceTile(i, num4 + 1, 19, mute: true, forced: false, -1, 23);
						}
					}
				}
				else
				{
					if (Main.tile[i, num4].type != 15 && Main.tile[i, num4].type != 304 && Main.tile[i, num4].type != 21 && Main.tile[i, num4].type != 10 && Main.tile[i - 1, num4].type != 10 && Main.tile[i + 1, num4].type != 10)
					{
						if (!IsConsideredDungeonWall(Main.tile[i, num4].wall))
						{
							Main.tile[i, num4].type = 191;
							Main.tile[i, num4].active(active: true);
							Main.tile[i, num4].Clear(TileDataType.Slope);
						}
						if (Main.tile[i - 1, num4].type == 40)
						{
							Main.tile[i - 1, num4].type = 0;
						}
						if (Main.tile[i + 1, num4].type == 40)
						{
							Main.tile[i + 1, num4].type = 0;
						}
					}
					if (num4 <= j && num4 > j - 4 && i > minimumLeft - num7 && i <= minimumRight + num7 - 1)
					{
						Main.tile[i, num4].wall = 244;
					}
				}
				if (!WorldGen.isGeneratingOrLoadingWorld)
				{
					WorldGen.SquareTileFrame(i, num4);
					WorldGen.SquareWallFrame(i, num4);
				}
			}
			num5++;
			if (num5 < 6)
			{
				continue;
			}
			num5 = 0;
			int num8 = genRand.Next(3);
			if (num8 == 0)
			{
				num8 = -1;
			}
			if (flag)
			{
				num8 = 2;
			}
			if (num8 == -1 && Main.tile[minimumLeft - num3, num4].wall == 244)
			{
				num8 = 1;
			}
			else if (num8 == 1 && Main.tile[minimumRight + num3, num4].wall == 244)
			{
				num8 = -1;
			}
			if (num8 == 2)
			{
				flag = false;
				ushort type = 19;
				int style = 23;
				bool flag2 = false;
				if (IsConsideredDungeonWall(Main.tile[minimumLeft, num4 + 1].wall) || IsConsideredDungeonWall(Main.tile[minimumLeft + 1, num4 + 1].wall) || IsConsideredDungeonWall(Main.tile[minimumLeft + 2, num4 + 1].wall))
				{
					flag2 = true;
					type = (ushort)tileType;
					style = tileStyle;
				}
				if (!WorldGen.SolidTile(minimumLeft - 1, num4 + 1) && !WorldGen.SolidTile(minimumRight + 1, num4 + 1) && flag2)
				{
					continue;
				}
				for (int k = minimumLeft; k <= minimumRight; k++)
				{
					if (k > num6 - 2 && k <= num6 + 1)
					{
						Main.tile[k, num4 + 1].ClearTileAndPaint();
						WorldGen.PlaceTile(k, num4 + 1, type, mute: true, forced: false, -1, style);
					}
				}
			}
			else
			{
				minimumLeft += num8;
				minimumRight += num8;
			}
		}
		minimumLeft = num;
		minimumRight = num2;
		for (int l = minimumLeft; l <= minimumRight; l++)
		{
			for (int m = j - 3; m <= j; m++)
			{
				Tile tile = Main.tile[l, m];
				tile.ClearTileAndPaint();
				if (!IsConsideredDungeonWall(tile.wall) && !IsConsideredDungeonWallGlass(tile.wall))
				{
					tile.wall = 244;
				}
			}
		}
	}

	public static void GenerateDungeonStairs(DungeonData data, int i, int j, int direction, DungeonGenerationStyleData styleData, int depth = 100)
	{
		if (!WorldGen.InWorld(i, j, 20))
		{
			return;
		}
		ushort brickTileType = styleData.BrickTileType;
		ushort brickWallType = styleData.BrickWallType;
		byte? tilePaintColor = styleData.TilePaintColor;
		byte? wallPaintColor = styleData.WallPaintColor;
		int num = depth;
		int num2 = depth;
		int num3 = ((direction == 1) ? 1 : (-1));
		int num4 = 0;
		for (int k = i; (direction == 1) ? (k < i + num2) : (k > i - num2); k += num3)
		{
			num4++;
			for (int l = j + num4; l < j + num; l++)
			{
				if (WorldGen.InWorld(k, l, 10) && !GenerateDungeonStairs_CanPlaceTile(k, l + 5) && num > l)
				{
					num = l;
					break;
				}
			}
		}
		num2 = num;
		depth = num;
		num4 = 0;
		for (int m = i; (direction == 1) ? (m < i + num2) : (m > i - num2); m += num3)
		{
			num4++;
			for (int n = j + num4; n < j + depth; n++)
			{
				if (!WorldGen.InWorld(m, n, 10) || n >= DungeonCrawler.CurrentDungeonData.genVars.outerPotentialDungeonBounds.Top - 5)
				{
					continue;
				}
				Tile tile = Main.tile[m, n];
				tile.liquid = 0;
				Main.tile[m, n - 1].liquid = 0;
				Main.tile[m, n - 2].liquid = 0;
				Main.tile[m, n - 3].liquid = 0;
				if (!GenerateDungeonStairs_CanPlaceTile(m, n))
				{
					continue;
				}
				bool flag = data.genVars.dungeonStyle.WallIsInStyle(tile.wall);
				if (!flag)
				{
					foreach (DungeonGenerationStyleData dungeonGenerationStyle in data.genVars.dungeonGenerationStyles)
					{
						if (dungeonGenerationStyle.WallIsInStyle(tile.wall))
						{
							flag = true;
							break;
						}
					}
				}
				if (flag)
				{
					if (tile.active())
					{
						tile.active(active: true);
						tile.type = brickTileType;
						tile.Clear(TileDataType.Slope);
						if (tilePaintColor.HasValue)
						{
							tile.color(tilePaintColor.Value);
						}
					}
					tile.wall = brickWallType;
					if (wallPaintColor.HasValue)
					{
						tile.wallColor(wallPaintColor.Value);
					}
					continue;
				}
				tile.active(active: true);
				tile.type = brickTileType;
				tile.Clear(TileDataType.Slope);
				if (tilePaintColor.HasValue)
				{
					tile.color(tilePaintColor.Value);
				}
				if (n != j + num4)
				{
					tile.wall = brickWallType;
					if (wallPaintColor.HasValue)
					{
						tile.wallColor(wallPaintColor.Value);
					}
				}
			}
		}
	}

	private static bool GenerateDungeonStairs_CanPlaceTile(int x, int y)
	{
		if (y >= DungeonCrawler.CurrentDungeonData.genVars.outerPotentialDungeonBounds.Top - 5)
		{
			return false;
		}
		Tile tile = Main.tile[x, y];
		if (tile.active())
		{
			if (!WorldGen.CanKillTile(x, y))
			{
				return false;
			}
			if (tile.type >= 0 && Main.tileFrameImportant[tile.type])
			{
				return false;
			}
		}
		return true;
	}

	public static void GenerateFloatingRocksInArea(DungeonData data, DungeonGenerationStyleData styleData, DungeonBounds bounds, ushort tileType, bool includePlatform = true, int? paint = -1)
	{
		GenerateFloatingRocksInArea(data, styleData, bounds.Left, bounds.Top, bounds.Width, bounds.Height, tileType, includePlatform, paint);
	}

	public static void GenerateFloatingRocksInArea(DungeonData data, DungeonGenerationStyleData styleData, int x, int y, int width, int height, ushort tileType, bool includePlatform = true, int? paint = -1)
	{
		int platformStyle = -1;
		if (includePlatform)
		{
			UnifiedRandom genRand = WorldGen.genRand;
			platformStyle = styleData.GetPlatformStyle(genRand);
		}
		GenerateFloatingRocksInArea(data, x, y, width, height, tileType, includePlatform, platformStyle, paint);
	}

	public static void GenerateFloatingRocksInArea(DungeonData data, DungeonBounds bounds, ushort tileType, bool includePlatform = true, int platformStyle = -1, int? paint = -1, int platformDistance = 7)
	{
		GenerateFloatingRocksInArea(data, bounds.Left, bounds.Top, bounds.Width, bounds.Height, tileType, includePlatform, platformStyle, paint, platformDistance);
	}

	public static void GenerateFloatingRocksInArea(DungeonData data, int x, int y, int width, int height, ushort tileType, bool includePlatform = true, int platformStyle = -1, int? paint = -1, int platformDistance = 7)
	{
		//IL_0180: Unknown result type (might be due to invalid IL or missing references)
		//IL_0185: Unknown result type (might be due to invalid IL or missing references)
		UnifiedRandom genRand = WorldGen.genRand;
		int num = Math.Max(2, (width + height) / 2 / platformDistance);
		int i = 0;
		int num2 = 100;
		int num3 = x + width / 2;
		int num4 = 8;
		bool flag = true;
		for (; i < num; i++)
		{
			num2--;
			if (num2 <= 0)
			{
				break;
			}
			int width2 = 4 + genRand.Next(3);
			int num5 = x;
			int num6 = y;
			if (i % 2 == 0)
			{
				int num7 = width / 2;
				num5 = ((!flag) ? (x + num4 + num7 + genRand.Next(Math.Max(1, num7 - num4 * 2))) : (x + num4 + genRand.Next(Math.Max(1, num7 - num4))));
				flag = !flag;
			}
			else
			{
				num5 = x + num4 + genRand.Next(width - num4 * 2);
			}
			num6 = y + num4 + (int)((float)(height - num4 * 2) * ((float)i / (float)num));
			GenerateRockPlatform(genRand, num5, num6, width2, tileType, paint);
			if (includePlatform)
			{
				int num8 = ((num5 < num3) ? (num5 - 5) : (num5 + 5));
				int num9 = ((num5 < num3) ? (num5 - x) : (x + width - num5));
				DungeonPlatformData item = new DungeonPlatformData
				{
					Position = new Point(num8, num6),
					InAHallway = false,
					OverrideHeightFluff = 0,
					ForcePlacement = true,
					OverrideMaxLengthAllowed = 5 + num9,
					canPlaceHereCallback = (DungeonData dungeonData, int platformX, int platformY) => platformX >= x && platformX <= x + width && platformY >= y && platformY <= y + height
				};
				if (platformStyle > -1)
				{
					item.OverrideStyle = platformStyle;
				}
				data.dungeonPlatformData.Add(item);
			}
		}
	}

	public static void GenerateRockPlatform(UnifiedRandom genRand, int x, int y, int width, ushort tileType, int? paint = -1)
	{
		int num = width / 2;
		int num2 = Math.Max(2, num + genRand.Next(2));
		for (int i = 0; i < width; i++)
		{
			int num3 = x + i - num;
			int num4 = num2;
			if (i == 0 || i == width - 1)
			{
				num4 = Math.Max(1, num4 / 2);
			}
			else if (i == 1 || i == width - 2)
			{
				num4 = Math.Max(2, (int)((float)num4 * 0.66f));
			}
			for (int j = 0; j < num4; j++)
			{
				int num5 = y + j;
				ChangeTileType(Main.tile[num3, num5], tileType, resetTile: false, paint);
			}
		}
	}

	public static void GenerateSpeleothemsInArea(DungeonData data, DungeonGenerationStyleData styleData, DungeonBounds bounds, int maxSpeleothems, ushort tileType, int? paint = -1, int speleothemWidth = -1, int speleothemHeight = -1)
	{
		GenerateSpeleothemsInArea(data, styleData, bounds.Left, bounds.Top, bounds.Width, bounds.Height, maxSpeleothems, tileType, paint);
	}

	public static void GenerateSpeleothemsInArea(DungeonData data, DungeonGenerationStyleData styleData, int x, int y, int width, int height, int maxSpeleothems, ushort tileType, int? paint = -1, int speleothemWidth = -1, int speleothemHeight = -1)
	{
		GenerateSpeleothemsInArea(data, x, y, width, height, maxSpeleothems, tileType, paint);
	}

	public static void GenerateSpeleothemsInArea(DungeonData data, int x, int y, int width, int height, int maxSpeleothems, ushort tileType, int? paint = -1, int speleothemWidth = -1, int speleothemHeight = -1)
	{
		UnifiedRandom genRand = WorldGen.genRand;
		int i = 1;
		int num = maxSpeleothems - 1;
		int num2 = 100;
		_ = width / 2;
		int num3 = y + height;
		int num4 = y + height / 2;
		int num5 = genRand.Next(2);
		if (speleothemWidth <= -1)
		{
			speleothemWidth = (int)Math.Max(5f, (float)width / (float)num);
		}
		if (speleothemHeight <= -1)
		{
			speleothemHeight = height / 4;
		}
		for (; i < num; i++)
		{
			num2--;
			if (num2 <= 0)
			{
				break;
			}
			int width2 = speleothemWidth + genRand.Next(3);
			int num6 = x + (int)((float)width * ((float)i / (float)(num - 1)));
			int num7 = num4;
			int num8 = num6;
			int num9 = num7;
			bool flag = (i + num5) % 2 == 0;
			if (genRand.Next(3) == 0)
			{
				flag = !flag;
			}
			if (flag)
			{
				Tile tile = Main.tile[num6, num7];
				int num10 = Math.Max((int)((float)width * 1.5f), (int)((float)height * 1.5f));
				while (!tile.active() && num7 < num3)
				{
					num10--;
					if (num10 <= 0)
					{
						break;
					}
					num7++;
					tile = Main.tile[num6, num7];
				}
			}
			else
			{
				Tile tile2 = Main.tile[num6, num7];
				int num11 = Math.Max((int)((float)width * 1.5f), (int)((float)height * 1.5f));
				while (!tile2.active() && num7 > y)
				{
					num11--;
					if (num11 <= 0)
					{
						break;
					}
					num7--;
					tile2 = Main.tile[num6, num7];
				}
			}
			bool flag2 = true;
			if (flag2 && Math.Abs(num4 - num7) < 10)
			{
				flag2 = false;
			}
			if (flag2)
			{
				GenerateSpeleothem(data, genRand, num6, num7, width2, speleothemHeight + genRand.Next(3), tileType, paint);
			}
			if (genRand.Next(3) != 0)
			{
				continue;
			}
			width2 = speleothemWidth + genRand.Next(3);
			num6 = num8;
			num7 = num9;
			if (!flag)
			{
				Tile tile3 = Main.tile[num6, num7];
				int num12 = Math.Max((int)((float)width * 1.5f), (int)((float)height * 1.5f));
				while (!tile3.active() && num7 < num3)
				{
					num12--;
					if (num12 <= 0)
					{
						break;
					}
					num7++;
					tile3 = Main.tile[num6, num7];
				}
			}
			else
			{
				Tile tile4 = Main.tile[num6, num7];
				int num13 = Math.Max((int)((float)width * 1.5f), (int)((float)height * 1.5f));
				while (!tile4.active() && num7 > y)
				{
					num13--;
					if (num13 <= 0)
					{
						break;
					}
					num7--;
					tile4 = Main.tile[num6, num7];
				}
			}
			flag2 = true;
			if (flag2 && Math.Abs(num4 - num7) < 10)
			{
				flag2 = false;
			}
			if (flag2)
			{
				GenerateSpeleothem(data, genRand, num6, num7, width2, speleothemHeight + genRand.Next(3), tileType, paint);
			}
		}
	}

	public static void GenerateSpeleothem(DungeonData data, UnifiedRandom genRand, int x, int y, int width, int height = -1, ushort tileType = 1, int? paint = -1)
	{
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		if (width % 2 == 1)
		{
			width++;
		}
		int num = width / 2;
		if (height <= -1)
		{
			height = width * 2 + genRand.Next(2);
		}
		int num2 = height / 2;
		y -= num2;
		if (!Main.tile[x, y].active())
		{
			y++;
		}
		for (int i = 0; i < data.dungeonDoorData.Count; i++)
		{
			if (data.dungeonDoorData[i].Position.ToVector2().Distance(new Vector2((float)x, (float)y)) <= 5f)
			{
				return;
			}
		}
		for (int j = 0; j <= width; j++)
		{
			int num3 = x + j - num;
			int num4 = (int)Utils.WrappedLerp(1f, height, (float)j / (float)width);
			if (genRand.Next(2) == 0)
			{
				num4 += 2;
			}
			int num5 = (height - num4) / 2;
			for (int k = 0; k < num4; k++)
			{
				int num6 = y + k + num5;
				ChangeTileType(Main.tile[num3, num6], tileType, resetTile: false, paint);
			}
		}
	}

	public static void ChangeTileType(Tile tile, ushort tileType, bool resetTile, int? paint = -1)
	{
		if (resetTile)
		{
			tile.ClearEverything();
		}
		tile.active(active: true);
		tile.Clear(TileDataType.Slope);
		tile.type = tileType;
		if (paint.HasValue && paint.Value >= 0)
		{
			tile.color((byte)paint.Value);
		}
	}

	public static void ChangeWallType(Tile tile, ushort wallType, bool resetTile, int? paint = -1)
	{
		if (resetTile)
		{
			tile.ClearEverything();
		}
		tile.wall = wallType;
		if (paint.HasValue && paint.Value >= 0)
		{
			tile.wallColor((byte)paint.Value);
		}
	}

	public static int GetDualDungeonBrickSupportCutoffY(DungeonData data)
	{
		if (SpecialSeedFeatures.DungeonEntranceIsUnderground)
		{
			return data.genVars.outerPotentialDungeonBounds.Top - 5;
		}
		return data.genVars.outerPotentialDungeonBounds.Top - 10;
	}

	public static void UpdateDungeonProgress(GenerationProgress progress, float percentile, string debugString, bool noFormatting = false)
	{
		Main.statusText = debugString;
		if (progress != null)
		{
			if (noFormatting)
			{
				progress.MessageNoFormatting = debugString;
			}
			else
			{
				progress.Message = debugString;
			}
			progress.Set(percentile);
		}
	}

	public static Point SetOldManSpawnAndSpawnOldManIfDefaultDungeon(int x, int y, bool generating = false)
	{
		//IL_0004: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		Point val = new Point(x, y);
		if (GenVars.CurrentDungeon == 0)
		{
			Main.dungeonX = val.X;
			Main.dungeonY = val.Y;
			if (generating)
			{
				int num = NPC.NewNPC(new EntitySource_WorldGen(), Main.dungeonX * 16 + 8, Main.dungeonY * 16, 37);
				Main.npc[num].homeless = false;
				Main.npc[num].homeTileX = Main.dungeonX;
				Main.npc[num].homeTileY = Main.dungeonY;
				if (Main.onlyShimmerOceanWorldsGeneration)
				{
					Main.npc[num].GivenName = "Old Man James";
				}
			}
		}
		return val;
	}

	public static bool IsPointOfProtectionType(int i2, int j2, List<DungeonRoom> roomsInArea, ProtectionType protectionToCheck)
	{
		ProtectionType highestProtectionTypeFromPoint = GetHighestProtectionTypeFromPoint(i2, j2, roomsInArea);
		switch (protectionToCheck)
		{
		default:
			return highestProtectionTypeFromPoint == protectionToCheck;
		case ProtectionType.Tiles:
		case ProtectionType.Walls:
			if (highestProtectionTypeFromPoint != protectionToCheck)
			{
				return highestProtectionTypeFromPoint == ProtectionType.TilesAndWalls;
			}
			return true;
		case ProtectionType.TilesAndWalls:
			if (highestProtectionTypeFromPoint != protectionToCheck && highestProtectionTypeFromPoint != ProtectionType.Tiles)
			{
				return highestProtectionTypeFromPoint == ProtectionType.Walls;
			}
			return true;
		}
	}

	public static ProtectionType GetHighestProtectionTypeFromPoint(int i2, int j2, List<DungeonRoom> roomsInArea)
	{
		ProtectionType protectionType = ProtectionType.None;
		for (int k = 0; k < roomsInArea.Count; k++)
		{
			switch (roomsInArea[k].GetProtectionTypeFromPoint(i2, j2))
			{
			case ProtectionType.Tiles:
				protectionType = ((protectionType != ProtectionType.Walls) ? ProtectionType.Tiles : ProtectionType.TilesAndWalls);
				break;
			case ProtectionType.Walls:
				protectionType = ((protectionType != ProtectionType.Tiles) ? ProtectionType.Walls : ProtectionType.TilesAndWalls);
				break;
			case ProtectionType.TilesAndWalls:
				protectionType = ProtectionType.TilesAndWalls;
				break;
			}
			if (protectionType == ProtectionType.TilesAndWalls)
			{
				break;
			}
		}
		return protectionType;
	}

	public static DungeonRoom GetClosestRoomTo(List<DungeonRoom> roomsToCheck, Point point, DungeonRoomSearchSettings settings)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		return GetClosestRoomTo(roomsToCheck, point.X, point.Y, settings);
	}

	public static DungeonRoom GetClosestRoomTo(List<DungeonRoom> roomsToCheck, int x, int y, DungeonRoomSearchSettings settings)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		Vector2 val = new Vector2((float)x, (float)y);
		DungeonRoom result = null;
		float num = 999999f;
		for (int i = 0; i < roomsToCheck.Count; i++)
		{
			DungeonRoom dungeonRoom = roomsToCheck[i];
			if (RoomCanBeChosen(dungeonRoom, settings))
			{
				if (dungeonRoom.OuterBounds.ContainsWithFluff(x, y, settings.Fluff))
				{
					return dungeonRoom;
				}
				float num2 = Vector2.Distance(val, dungeonRoom.Center.ToVector2());
				if (num2 < num)
				{
					result = dungeonRoom;
					num = num2;
				}
			}
		}
		return result;
	}

	public static List<DungeonRoom> GetAllRoomsNearSpot(List<DungeonRoom> roomsToCheck, int x, int y, DungeonRoomSearchSettings settings)
	{
		List<DungeonRoom> list = new List<DungeonRoom>();
		for (int i = 0; i < roomsToCheck.Count; i++)
		{
			DungeonRoom dungeonRoom = roomsToCheck[i];
			if (RoomCanBeChosen(dungeonRoom, settings) && dungeonRoom.OuterBounds.ContainsWithFluff(x, y, settings.Fluff))
			{
				list.Add(dungeonRoom);
			}
		}
		return list;
	}

	public static List<DungeonRoom> GetAllRoomsInSpots(List<DungeonRoom> roomsToCheck, Vector2D startPos, Vector2D endPos, DungeonRoomSearchSettings settings)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		Point point = startPos.ToPoint();
		Point point2 = ((endPos - startPos) / 2.0).ToPoint();
		Point point3 = endPos.ToPoint();
		List<DungeonRoom> list = new List<DungeonRoom>();
		for (int i = 0; i < roomsToCheck.Count; i++)
		{
			DungeonRoom dungeonRoom = roomsToCheck[i];
			if (RoomCanBeChosen(dungeonRoom, settings) && (dungeonRoom.OuterBounds.ContainsWithFluff(point, settings.Fluff) || dungeonRoom.OuterBounds.ContainsWithFluff(point2, settings.Fluff) || dungeonRoom.OuterBounds.ContainsWithFluff(point3, settings.Fluff)))
			{
				list.Add(dungeonRoom);
			}
		}
		return list;
	}

	public static bool RoomCanBeChosen(DungeonRoom room, DungeonRoomSearchSettings settings)
	{
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		if (room == null)
		{
			return false;
		}
		if (settings.ProgressionStage.HasValue)
		{
			int progressionStage = room.settings.ProgressionStage;
			int value = settings.ProgressionStage.Value;
			switch (settings.ProgressionStageCheck)
			{
			case ProgressionStageCheck.Equals:
				if (progressionStage != value)
				{
					return false;
				}
				break;
			case ProgressionStageCheck.LesserThenOrEqualTo:
				if (progressionStage > value)
				{
					return false;
				}
				break;
			case ProgressionStageCheck.GreaterThenOrEqualTo:
				if (progressionStage < value)
				{
					return false;
				}
				break;
			}
		}
		DungeonRoom excludedRoom = settings.ExcludedRoom;
		if (excludedRoom != null)
		{
			if (excludedRoom == room)
			{
				return false;
			}
			if (settings.MaximumDistance.HasValue && room.Center.ToVector2().Distance(excludedRoom.Center.ToVector2()) >= (float)settings.MaximumDistance.Value)
			{
				return false;
			}
		}
		return true;
	}

	public static bool IsConsideredDungeonTile(int tileType, bool allDungeons = false)
	{
		if (tileType > 0 && Main.tileDungeon[tileType])
		{
			return true;
		}
		if (allDungeons)
		{
			for (int i = 0; i < GenVars.dungeonGenVars.Count; i++)
			{
				if (GenVars.dungeonGenVars[i].isDungeonTile[tileType])
				{
					return true;
				}
			}
		}
		else if (GenVars.CurrentDungeonGenVars.isDungeonTile[tileType])
		{
			return true;
		}
		return false;
	}

	public static bool IsConsideredCrackedDungeonTile(int tileType, bool allDungeons = false)
	{
		if (allDungeons)
		{
			for (int i = 0; i < GenVars.dungeonGenVars.Count; i++)
			{
				if (GenVars.dungeonGenVars[i].isCrackedBrick[tileType])
				{
					return true;
				}
			}
		}
		else if (GenVars.CurrentDungeonGenVars.isCrackedBrick[tileType])
		{
			return true;
		}
		return false;
	}

	public static bool IsConsideredPitTrapTile(int tileType, bool allDungeons = false)
	{
		if (allDungeons)
		{
			for (int i = 0; i < GenVars.dungeonGenVars.Count; i++)
			{
				if (GenVars.dungeonGenVars[i].isPitTrapTile[tileType])
				{
					return true;
				}
			}
		}
		else if (GenVars.CurrentDungeonGenVars.isPitTrapTile[tileType])
		{
			return true;
		}
		return false;
	}

	public static bool IsConsideredAnyDungeonWall(int wallType, bool allDungeons = false)
	{
		if (wallType > 0 && Main.wallDungeon[wallType])
		{
			return true;
		}
		if (allDungeons)
		{
			for (int i = 0; i < GenVars.dungeonGenVars.Count; i++)
			{
				DungeonGenVars dungeonGenVars = GenVars.dungeonGenVars[i];
				if (dungeonGenVars.isDungeonWall[wallType] || dungeonGenVars.isDungeonWallEdge[wallType] || dungeonGenVars.isDungeonWallGlass[wallType])
				{
					return true;
				}
			}
		}
		else if (GenVars.CurrentDungeonGenVars.isDungeonWall[wallType] || GenVars.CurrentDungeonGenVars.isDungeonWallEdge[wallType] || GenVars.CurrentDungeonGenVars.isDungeonWallGlass[wallType])
		{
			return true;
		}
		return false;
	}

	public static bool IsConsideredDungeonWall(int wallType, bool allDungeons = false)
	{
		if (wallType > 0 && Main.wallDungeon[wallType])
		{
			return true;
		}
		if (allDungeons)
		{
			for (int i = 0; i < GenVars.dungeonGenVars.Count; i++)
			{
				if (GenVars.dungeonGenVars[i].isDungeonWall[wallType])
				{
					return true;
				}
			}
		}
		else if (GenVars.CurrentDungeonGenVars.isDungeonWall[wallType])
		{
			return true;
		}
		return false;
	}

	public static bool IsConsideredDungeonWallEdge(int wallType, bool allDungeons = false)
	{
		if (wallType > 0 && Main.wallDungeon[wallType])
		{
			return true;
		}
		if (allDungeons)
		{
			for (int i = 0; i < GenVars.dungeonGenVars.Count; i++)
			{
				if (GenVars.dungeonGenVars[i].isDungeonWallEdge[wallType])
				{
					return true;
				}
			}
		}
		else if (GenVars.CurrentDungeonGenVars.isDungeonWallEdge[wallType])
		{
			return true;
		}
		return false;
	}

	public static bool IsConsideredDungeonWallGlass(int wallType, bool allDungeons = false)
	{
		if (allDungeons)
		{
			for (int i = 0; i < GenVars.dungeonGenVars.Count; i++)
			{
				if (GenVars.dungeonGenVars[i].isDungeonWallGlass[wallType])
				{
					return true;
				}
			}
		}
		else if (GenVars.CurrentDungeonGenVars.isDungeonWallGlass[wallType])
		{
			return true;
		}
		return false;
	}

	public static bool IsHigherOrEqualTieredDungeonTile(DungeonData data, int currentTileType, int newTileType, int currentStyleID, int newStyleID, bool includeCracked = false, bool includeExtras = false)
	{
		double tierForDungeonTile = GetTierForDungeonTile(data.genVars, currentTileType, currentStyleID, includeCracked, includeExtras);
		double tierForDungeonTile2 = GetTierForDungeonTile(data.genVars, newTileType, newStyleID, includeCracked, includeExtras);
		return tierForDungeonTile >= tierForDungeonTile2;
	}

	public static bool IsHigherOrEqualTieredDungeonWall(DungeonData data, int currentWallType, int newWallType, int currentStyleID, int newStyleID, bool includeWindows = false, bool includeExtras = false)
	{
		double tierForDungeonWall = GetTierForDungeonWall(data.genVars, currentWallType, currentStyleID, includeWindows, includeExtras);
		double tierForDungeonWall2 = GetTierForDungeonWall(data.genVars, newWallType, newStyleID, includeWindows, includeExtras);
		return tierForDungeonWall >= tierForDungeonWall2;
	}

	public static double GetTierForDungeonTile(DungeonGenVars genVars, int tileType, int genStyleID, bool includeCracked = false, bool includeExtras = false)
	{
		if (WorldGen.SecretSeed.dualDungeons.Enabled)
		{
			for (int i = 0; i < genVars.dungeonGenerationStyles.Count; i++)
			{
				DungeonGenerationStyleData dungeonGenerationStyleData = genVars.dungeonGenerationStyles[i];
				if (dungeonGenerationStyleData.Style == 1 && DungeonGenerationStyles.Spider.TileIsInStyle(tileType, includeCracked, includeExtras))
				{
					return (double)i + 0.25;
				}
				if (dungeonGenerationStyleData.Style == 1 && (genStyleID == -1 || genStyleID == 13) && DungeonGenerationStyles.LivingWood.TileIsInStyle(tileType, includeCracked, includeExtras))
				{
					return (double)i + 0.5;
				}
				if (dungeonGenerationStyleData.Style == 1 && DungeonGenerationStyles.Shimmer.TileIsInStyle(tileType, includeCracked, includeExtras))
				{
					return (double)i + 0.75;
				}
				if (dungeonGenerationStyleData.Style == 8 && (genStyleID == -1 || genStyleID == 14) && DungeonGenerationStyles.LivingMahogany.TileIsInStyle(tileType, includeCracked, includeExtras))
				{
					return (double)i + 0.33;
				}
				if (dungeonGenerationStyleData.Style == 8 && DungeonGenerationStyles.Beehive.TileIsInStyle(tileType, includeCracked, includeExtras))
				{
					return (double)i + 0.66;
				}
				if (dungeonGenerationStyleData.Style == 6 && DungeonGenerationStyles.Crystal.TileIsInStyle(tileType, includeCracked, includeExtras))
				{
					return (double)i + 0.5;
				}
				if (dungeonGenerationStyleData.Style == 0 && Main.tileDungeon[tileType])
				{
					return i;
				}
				if (dungeonGenerationStyleData.TileIsInStyle(tileType))
				{
					return i;
				}
			}
			if (Main.tileDungeon[tileType])
			{
				return -0.5;
			}
			return -1.0;
		}
		return (tileType > 0 && Main.tileDungeon[tileType]) ? 1f : (-1f);
	}

	public static double GetTierForDungeonWall(DungeonGenVars genVars, int wallType, int genStyleID, bool includeWindows = false, bool includeExtras = false)
	{
		if (WorldGen.SecretSeed.dualDungeons.Enabled)
		{
			for (int i = 0; i < genVars.dungeonGenerationStyles.Count; i++)
			{
				DungeonGenerationStyleData dungeonGenerationStyleData = genVars.dungeonGenerationStyles[i];
				if (dungeonGenerationStyleData.Style == 1 && DungeonGenerationStyles.Spider.WallIsInStyle(wallType, includeWindows, includeExtras))
				{
					return (double)i + 0.25;
				}
				if (dungeonGenerationStyleData.Style == 1 && (genStyleID == -1 || genStyleID == 13) && DungeonGenerationStyles.LivingWood.WallIsInStyle(wallType, includeWindows, includeExtras))
				{
					return (double)i + 0.5;
				}
				if (dungeonGenerationStyleData.Style == 1 && DungeonGenerationStyles.Shimmer.WallIsInStyle(wallType, includeWindows, includeExtras))
				{
					return (double)i + 0.75;
				}
				if (dungeonGenerationStyleData.Style == 8 && (genStyleID == -1 || genStyleID == 14) && DungeonGenerationStyles.LivingMahogany.WallIsInStyle(wallType, includeWindows, includeExtras))
				{
					return (double)i + 0.33;
				}
				if (dungeonGenerationStyleData.Style == 8 && DungeonGenerationStyles.Beehive.WallIsInStyle(wallType, includeWindows, includeExtras))
				{
					return (double)i + 0.66;
				}
				if (dungeonGenerationStyleData.Style == 6 && DungeonGenerationStyles.Crystal.WallIsInStyle(wallType, includeWindows, includeExtras))
				{
					return (double)i + 0.5;
				}
				if (dungeonGenerationStyleData.Style == 0 && Main.wallDungeon[wallType])
				{
					return i;
				}
				if (dungeonGenerationStyleData.WallIsInStyle(wallType))
				{
					return i;
				}
			}
			if (Main.wallDungeon[wallType])
			{
				return -0.5;
			}
			return -1.0;
		}
		return (wallType > 0 && Main.wallDungeon[wallType]) ? 1f : (-1f);
	}

	public static void CreatePotentialDungeonBounds(out DungeonBounds innerBounds, out DungeonBounds outerBounds, bool leftDungeon, double percentInMiddle = 0.02, double percentOnEdges = 0.02, double percentOnTop = -1.0, double percentOnBottom = -1.0, int innerBuffer = 10)
	{
		if (percentOnTop == -1.0)
		{
			percentOnTop = ((!SpecialSeedFeatures.DungeonEntranceIsUnderground) ? ((Main.worldSurface + 10.0) / (double)Main.maxTilesY) : ((GenVars.worldSurfaceHigh + 10.0) / (double)Main.maxTilesY));
		}
		if (percentOnBottom == -1.0)
		{
			percentOnBottom = ((double)Main.UnderworldLayer - 10.0) / (double)Main.maxTilesY;
		}
		double num = percentInMiddle / 2.0;
		_ = (double)Main.maxTilesX / 4200.0;
		int num2 = (leftDungeon ? ((int)((double)Main.maxTilesX * percentOnEdges)) : ((int)((double)Main.maxTilesX * (0.5 + num))));
		int num3 = (leftDungeon ? ((int)((double)Main.maxTilesX * (0.5 - num))) : (Main.maxTilesX - (int)((double)Main.maxTilesX * percentOnEdges)));
		int num4 = (int)((double)Main.maxTilesY * percentOnTop);
		int num5 = (int)((double)Main.maxTilesY * percentOnBottom);
		outerBounds = new DungeonBounds();
		outerBounds.SetBounds(num2, num4, num3, num5);
		innerBounds = new DungeonBounds();
		innerBounds.SetBounds(num2 + innerBuffer, num4 + innerBuffer, num3 - innerBuffer, num5 - innerBuffer);
	}

	public static bool InAnyPotentialDungeonBounds(int x, int y, int fluff = 0, bool inner = false)
	{
		int iteration;
		return InAnyPotentialDungeonBounds(out iteration, x, y, fluff, inner);
	}

	public static bool InAnyPotentialDungeonBounds(out int iteration, int x, int y, int fluff = 0, bool inner = false)
	{
		iteration = -1;
		for (int i = 0; i < GenVars.dungeonGenVars.Count; i++)
		{
			DungeonGenVars dungeonGenVars = GenVars.dungeonGenVars[i];
			if ((inner && dungeonGenVars.innerPotentialDungeonBounds.ContainsWithFluff(x, y, fluff)) || (!inner && dungeonGenVars.outerPotentialDungeonBounds.ContainsWithFluff(x, y, fluff)))
			{
				iteration = i;
				return true;
			}
		}
		return false;
	}

	public static bool IntersectsAnyPotentialDungeonBounds(Rectangle rect, bool inner = false)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		int iteration;
		return IntersectsAnyPotentialDungeonBounds(out iteration, rect, inner);
	}

	public static bool IntersectsAnyPotentialDungeonBounds(out int iteration, Rectangle rect, bool inner = false)
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		iteration = -1;
		for (int i = 0; i < GenVars.dungeonGenVars.Count; i++)
		{
			DungeonGenVars dungeonGenVars = GenVars.dungeonGenVars[i];
			if ((inner && dungeonGenVars.innerPotentialDungeonBounds.Intersects(rect)) || (!inner && dungeonGenVars.outerPotentialDungeonBounds.Intersects(rect)))
			{
				iteration = i;
				return true;
			}
		}
		return false;
	}

	public static Rectangle GetExpandedDungeonAreaFromPoint(int x, int y)
	{
		//IL_0238: Unknown result type (might be due to invalid IL or missing references)
		int num = x;
		int num2 = x;
		int num3 = y;
		int num4 = y;
		for (int i = 0; i < 2; i++)
		{
			num = x;
			num2 = x;
			while (!Main.tile[num, y].active() && IsConsideredDungeonWall(Main.tile[num, y].wall))
			{
				num--;
			}
			num++;
			for (; !Main.tile[num2, y].active() && IsConsideredDungeonWall(Main.tile[num2, y].wall); num2++)
			{
			}
			num2--;
			x = (num + num2) / 2;
			num3 = y;
			num4 = y;
			while (!Main.tile[x, num3].active() && IsConsideredDungeonWall(Main.tile[x, num3].wall))
			{
				num3--;
			}
			num3++;
			for (; !Main.tile[x, num4].active() && IsConsideredDungeonWall(Main.tile[x, num4].wall); num4++)
			{
			}
			num4--;
			y = (num3 + num4) / 2;
		}
		num = x;
		num2 = x;
		while (!Main.tile[num, y].active() && !Main.tile[num, y - 1].active() && !Main.tile[num, y + 1].active())
		{
			num--;
		}
		num++;
		for (; !Main.tile[num2, y].active() && !Main.tile[num2, y - 1].active() && !Main.tile[num2, y + 1].active(); num2++)
		{
		}
		num2--;
		num3 = y;
		num4 = y;
		while (!Main.tile[x, num3].active() && !Main.tile[x - 1, num3].active() && !Main.tile[x + 1, num3].active())
		{
			num3--;
		}
		num3++;
		for (; !Main.tile[x, num4].active() && !Main.tile[x - 1, num4].active() && !Main.tile[x + 1, num4].active(); num4++)
		{
		}
		num4--;
		return new Rectangle(num, num3, num4 - num3, num2 - num);
	}

	public static int CalculateFloodedTileCountFromShapeData(DungeonBounds innerBounds, ShapeData data)
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		Point16[] array = data.GetData().ToArray();
		int num = 0;
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i].Y > innerBounds.Center.Y)
			{
				num++;
			}
		}
		return num;
	}
}
