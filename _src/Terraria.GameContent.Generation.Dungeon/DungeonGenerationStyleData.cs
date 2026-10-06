using System.Collections.Generic;
using Terraria.GameContent.Generation.Dungeon.Features;
using Terraria.GameContent.Generation.Dungeon.Rooms;
using Terraria.ID;
using Terraria.Utilities;

namespace Terraria.GameContent.Generation.Dungeon;

public class DungeonGenerationStyleData
{
	public byte Style;

	public int UnbreakableWallProgressionTier = -1;

	public ushort BrickTileType;

	public ushort? BrickGrassTileType;

	public ushort BrickCrackedTileType;

	public ushort BrickWallType;

	public ushort[] ExtraTileTypes;

	public byte? TilePaintColor;

	public byte? WallPaintColor;

	public ushort WindowGlassWallType;

	public ushort WindowClosedGlassWallType;

	public ushort WindowEdgeWallType;

	public int[] WindowPlatformItemTypes;

	public ushort[] ExtraWallTypes;

	public ushort PitTrapTileType;

	public int LiquidType = -1;

	public int LockedBiomeChestType;

	public int LockedBiomeChestStyle;

	public int BiomeChestItemType;

	public int BiomeChestLootItemType;

	public int[] ChestItemTypes;

	public int[] DoorItemTypes;

	public int[] PlatformItemTypes;

	public int[] ChandelierItemTypes;

	public int[] LanternItemTypes;

	public int[] TableItemTypes;

	public int[] WorkbenchItemTypes;

	public int[] CandleItemTypes;

	public int[] VaseOrStatueItemTypes;

	public int[] BookcaseItemTypes;

	public int[] ChairItemTypes;

	public int[] BedItemTypes;

	public int[] PianoItemTypes;

	public int[] DresserItemTypes;

	public int[] SofaItemTypes;

	public int[] BathtubItemTypes;

	public int[] LampItemTypes;

	public int[] CandelabraItemTypes;

	public int[] ClockItemTypes;

	public int[] BannerItemTypes;

	public bool EdgeDither;

	public DungeonRoomType BiomeRoomType;

	public List<DungeonGenerationStyleData> SubStyles;

	public virtual bool CanGenerateFeatureAt(DungeonData data, DungeonRoom room, IDungeonFeature feature, int x, int y)
	{
		return true;
	}

	public virtual void GetBookshelfMinMaxSizes(int defaultMin, int defaultMax, out int min, out int max)
	{
		min = defaultMin;
		max = defaultMax;
	}

	public bool TileIsInStyle(int tileType, bool includeCracked = true, bool includeExtras = false)
	{
		if (BrickGrassTileType.HasValue && tileType == BrickGrassTileType.Value)
		{
			return true;
		}
		if (includeCracked && tileType == BrickCrackedTileType)
		{
			return true;
		}
		if (includeExtras && ExtraTileTypes != null)
		{
			for (int i = 0; i < ExtraTileTypes.Length; i++)
			{
				if (tileType == ExtraTileTypes[i])
				{
					return true;
				}
			}
		}
		return tileType == BrickTileType;
	}

	public bool WallIsInStyle(int wallType, bool includeWindows = false, bool includeExtras = false)
	{
		if (includeWindows && (wallType == WindowGlassWallType || wallType == WindowEdgeWallType || wallType == WindowClosedGlassWallType))
		{
			return true;
		}
		if (includeExtras && ExtraWallTypes != null)
		{
			for (int i = 0; i < ExtraWallTypes.Length; i++)
			{
				if (wallType == ExtraWallTypes[i])
				{
					return true;
				}
			}
		}
		return wallType == BrickWallType;
	}

	public int GetPlatformStyle(UnifiedRandom genRand)
	{
		int num = ((PlatformItemTypes == null || PlatformItemTypes.Length == 0) ? (-1) : PlatformItemTypes[genRand.Next(PlatformItemTypes.Length)]);
		if (num >= 0)
		{
			return ItemID.Sets.DerivedPlacementDetails[num].tileStyle;
		}
		return -1;
	}

	public int GetWindowPlatformStyle(UnifiedRandom genRand)
	{
		int num = ((WindowPlatformItemTypes == null || WindowPlatformItemTypes.Length == 0) ? (-1) : WindowPlatformItemTypes[genRand.Next(WindowPlatformItemTypes.Length)]);
		if (num >= 0)
		{
			return ItemID.Sets.DerivedPlacementDetails[num].tileStyle;
		}
		return -1;
	}
}
