using System.Collections.Generic;
using Terraria.GameContent.Generation.Dungeon.Features;
using Terraria.GameContent.Generation.Dungeon.Rooms;
using Terraria.ID;
using Terraria.WorldBuilding;

namespace Terraria.GameContent.Generation.Dungeon;

public static class DungeonGenerationStyles
{
	private class ShimmerStyleData : DungeonGenerationStyleData
	{
		public override bool CanGenerateFeatureAt(DungeonData data, DungeonRoom room, IDungeonFeature feature, int x, int y)
		{
			if (!(feature is DungeonPitTrap))
			{
				return !(feature is DungeonWindow);
			}
			return false;
		}
	}

	private class LivingWoodStyleData : DungeonGenerationStyleData
	{
		public override bool CanGenerateFeatureAt(DungeonData data, DungeonRoom room, IDungeonFeature feature, int x, int y)
		{
			if (!(feature is DungeonGlobalSpikes))
			{
				return !(feature is DungeonPitTrap);
			}
			return false;
		}

		public override void GetBookshelfMinMaxSizes(int defaultMin, int defaultMax, out int min, out int max)
		{
			min = 3;
			max = 7;
		}
	}

	private class BeehiveStyleData : DungeonGenerationStyleData
	{
		public override bool CanGenerateFeatureAt(DungeonData data, DungeonRoom room, IDungeonFeature feature, int x, int y)
		{
			if (!(feature is DungeonGlobalPaintings) && !(feature is DungeonGlobalSpikes) && !(feature is DungeonPitTrap))
			{
				return !(feature is DungeonWindow);
			}
			return false;
		}
	}

	private class TempleStyleData : DungeonGenerationStyleData
	{
		public override bool CanGenerateFeatureAt(DungeonData data, DungeonRoom room, IDungeonFeature feature, int x, int y)
		{
			if (!(feature is DungeonPitTrap))
			{
				return !(feature is DungeonPillar);
			}
			return false;
		}
	}

	public static DungeonGenerationStyleData Shimmer = new ShimmerStyleData
	{
		Style = 11,
		UnbreakableWallProgressionTier = DualDungeonUnbreakableWallTiers.EarlyGame,
		BrickTileType = 667,
		BrickCrackedTileType = 123,
		BrickWallType = 322,
		WindowGlassWallType = 93,
		WindowClosedGlassWallType = 149,
		WindowEdgeWallType = 37,
		WindowPlatformItemTypes = new int[1] { 94 },
		PitTrapTileType = 123,
		LiquidType = 3,
		LockedBiomeChestType = -1,
		LockedBiomeChestStyle = -1,
		BiomeChestItemType = -1,
		BiomeChestLootItemType = -1,
		ChestItemTypes = new int[1] { 5556 },
		DoorItemTypes = new int[1] { 5558 },
		PlatformItemTypes = new int[1] { 5562 },
		ChandelierItemTypes = new int[1] { 5555 },
		LanternItemTypes = new int[1] { 5560 },
		TableItemTypes = new int[1] { 5565 },
		WorkbenchItemTypes = new int[1] { 5566 },
		CandleItemTypes = new int[1] { 5553 },
		VaseOrStatueItemTypes = null,
		BookcaseItemTypes = new int[1] { 5550 },
		ChairItemTypes = new int[1] { 5554 },
		BedItemTypes = new int[1] { 5549 },
		PianoItemTypes = new int[1] { 5561 },
		DresserItemTypes = new int[1] { 5551 },
		SofaItemTypes = new int[1] { 5564 },
		BathtubItemTypes = new int[1] { 5548 },
		LampItemTypes = new int[1] { 5559 },
		CandelabraItemTypes = new int[1] { 5552 },
		ClockItemTypes = new int[1] { 5557 },
		BannerItemTypes = new int[6] { 337, 339, 338, 340, 5497, 5498 },
		EdgeDither = true,
		BiomeRoomType = DungeonRoomType.BiomeRugged
	};

	public static DungeonGenerationStyleData Spider = new DungeonGenerationStyleData
	{
		Style = 12,
		UnbreakableWallProgressionTier = DualDungeonUnbreakableWallTiers.EarlyGame,
		BrickTileType = 156,
		BrickCrackedTileType = 123,
		BrickWallType = 62,
		WindowGlassWallType = 21,
		WindowClosedGlassWallType = 4,
		WindowEdgeWallType = 36,
		WindowPlatformItemTypes = new int[1] { 94 },
		PitTrapTileType = 123,
		LockedBiomeChestType = -1,
		LockedBiomeChestStyle = -1,
		BiomeChestItemType = -1,
		BiomeChestLootItemType = -1,
		ChestItemTypes = new int[1] { 952 },
		DoorItemTypes = new int[1] { 4415 },
		PlatformItemTypes = new int[1] { 4416 },
		ChandelierItemTypes = new int[6] { 106, 107, 108, 710, 711, 712 },
		LanternItemTypes = new int[1] { 2037 },
		TableItemTypes = new int[1] { 32 },
		WorkbenchItemTypes = new int[1] { 36 },
		CandleItemTypes = new int[2] { 105, 713 },
		VaseOrStatueItemTypes = null,
		BookcaseItemTypes = new int[1] { 354 },
		ChairItemTypes = new int[1] { 34 },
		BedItemTypes = new int[1] { 224 },
		PianoItemTypes = new int[1] { 333 },
		DresserItemTypes = new int[1] { 334 },
		SofaItemTypes = new int[1] { 2397 },
		BathtubItemTypes = new int[1] { 336 },
		LampItemTypes = new int[1] { 342 },
		CandelabraItemTypes = new int[2] { 349, 714 },
		ClockItemTypes = new int[1] { 359 },
		BannerItemTypes = null,
		EdgeDither = true,
		BiomeRoomType = DungeonRoomType.BiomeRugged
	};

	public static DungeonGenerationStyleData LivingWood = new LivingWoodStyleData
	{
		Style = 13,
		UnbreakableWallProgressionTier = DualDungeonUnbreakableWallTiers.EarlyGame,
		BrickTileType = 191,
		BrickCrackedTileType = 192,
		BrickWallType = 244,
		WindowGlassWallType = 21,
		WindowClosedGlassWallType = 4,
		WindowEdgeWallType = 196,
		WindowPlatformItemTypes = new int[1] { 2629 },
		PitTrapTileType = 123,
		LockedBiomeChestType = -1,
		LockedBiomeChestStyle = -1,
		BiomeChestItemType = -1,
		BiomeChestLootItemType = -1,
		ChestItemTypes = new int[1] { 831 },
		DoorItemTypes = new int[1] { 819 },
		PlatformItemTypes = new int[1] { 2629 },
		ChandelierItemTypes = new int[1] { 2141 },
		LanternItemTypes = new int[1] { 2145 },
		TableItemTypes = new int[1] { 829 },
		WorkbenchItemTypes = new int[1] { 2633 },
		CandleItemTypes = new int[1] { 2153 },
		VaseOrStatueItemTypes = null,
		BookcaseItemTypes = new int[1] { 2135 },
		ChairItemTypes = new int[1] { 806 },
		BedItemTypes = new int[1] { 2139 },
		PianoItemTypes = new int[1] { 2245 },
		DresserItemTypes = new int[1] { 3914 },
		SofaItemTypes = new int[1] { 2636 },
		BathtubItemTypes = new int[1] { 2126 },
		LampItemTypes = new int[1] { 2131 },
		CandelabraItemTypes = new int[1] { 2149 },
		ClockItemTypes = new int[1] { 2596 },
		BannerItemTypes = null,
		EdgeDither = true,
		BiomeRoomType = DungeonRoomType.BiomeRugged
	};

	public static DungeonGenerationStyleData Cavern = new DungeonGenerationStyleData
	{
		Style = 1,
		UnbreakableWallProgressionTier = DualDungeonUnbreakableWallTiers.EarlyGame,
		BrickTileType = 38,
		BrickCrackedTileType = 123,
		BrickWallType = 349,
		WindowGlassWallType = 21,
		WindowClosedGlassWallType = 4,
		WindowEdgeWallType = 5,
		WindowPlatformItemTypes = new int[2] { 94, 4416 },
		PitTrapTileType = 123,
		LockedBiomeChestType = -1,
		LockedBiomeChestStyle = -1,
		BiomeChestItemType = -1,
		BiomeChestLootItemType = -1,
		ChestItemTypes = new int[2] { 306, 5886 },
		DoorItemTypes = new int[2] { 25, 4415 },
		PlatformItemTypes = new int[2] { 94, 4416 },
		ChandelierItemTypes = new int[7] { 106, 107, 108, 710, 711, 712, 5885 },
		LanternItemTypes = new int[2] { 2037, 5890 },
		TableItemTypes = new int[2] { 32, 5894 },
		WorkbenchItemTypes = new int[2] { 36, 5896 },
		CandleItemTypes = new int[3] { 105, 713, 5883 },
		VaseOrStatueItemTypes = null,
		BookcaseItemTypes = new int[2] { 354, 5881 },
		ChairItemTypes = new int[2] { 34, 5884 },
		BedItemTypes = new int[2] { 224, 5880 },
		PianoItemTypes = new int[2] { 333, 5891 },
		DresserItemTypes = new int[2] { 334, 5888 },
		SofaItemTypes = new int[2] { 2397, 5893 },
		BathtubItemTypes = new int[2] { 336, 5879 },
		LampItemTypes = new int[2] { 342, 5889 },
		CandelabraItemTypes = new int[3] { 349, 714, 5882 },
		ClockItemTypes = new int[2] { 359, 5887 },
		BannerItemTypes = new int[6] { 337, 339, 338, 340, 5497, 5498 },
		EdgeDither = true,
		BiomeRoomType = DungeonRoomType.BiomeStructured,
		SubStyles = new List<DungeonGenerationStyleData> { Shimmer, Spider, LivingWood }
	};

	public static DungeonGenerationStyleData Snow = new DungeonGenerationStyleData
	{
		Style = 2,
		UnbreakableWallProgressionTier = DualDungeonUnbreakableWallTiers.EarlyGame,
		BrickTileType = 161,
		BrickCrackedTileType = 224,
		ExtraTileTypes = new ushort[1] { 147 },
		BrickWallType = 71,
		ExtraWallTypes = new ushort[1] { 40 },
		WindowGlassWallType = 90,
		WindowClosedGlassWallType = 149,
		WindowEdgeWallType = 31,
		WindowPlatformItemTypes = new int[1] { 3908 },
		PitTrapTileType = 224,
		LockedBiomeChestType = 21,
		LockedBiomeChestStyle = 27,
		BiomeChestItemType = 1532,
		BiomeChestLootItemType = 1572,
		ChestItemTypes = new int[2] { 681, 5805 },
		DoorItemTypes = new int[2] { 2044, 5807 },
		PlatformItemTypes = new int[2] { 3908, 5812 },
		ChandelierItemTypes = new int[2] { 2059, 5804 },
		LanternItemTypes = new int[2] { 2040, 5810 },
		TableItemTypes = new int[2] { 2248, 5815 },
		WorkbenchItemTypes = new int[2] { 2252, 5817 },
		CandleItemTypes = new int[2] { 2049, 5802 },
		VaseOrStatueItemTypes = null,
		BookcaseItemTypes = new int[2] { 2031, 5800 },
		ChairItemTypes = new int[2] { 2288, 5803 },
		BedItemTypes = new int[2] { 2068, 5799 },
		PianoItemTypes = new int[2] { 2247, 5811 },
		DresserItemTypes = new int[2] { 3913, 5808 },
		SofaItemTypes = new int[2] { 2635, 5814 },
		BathtubItemTypes = new int[2] { 2076, 5798 },
		LampItemTypes = new int[2] { 2086, 5809 },
		CandelabraItemTypes = new int[2] { 2100, 5801 },
		ClockItemTypes = new int[2] { 2594, 5806 },
		BannerItemTypes = null,
		EdgeDither = true,
		BiomeRoomType = DungeonRoomType.BiomeRugged
	};

	public static DungeonGenerationStyleData Desert = new DungeonGenerationStyleData
	{
		Style = 3,
		UnbreakableWallProgressionTier = DualDungeonUnbreakableWallTiers.EarlyGame,
		BrickTileType = 396,
		BrickCrackedTileType = 53,
		ExtraTileTypes = new ushort[2] { 397, 404 },
		BrickWallType = 187,
		ExtraWallTypes = new ushort[2] { 216, 223 },
		WindowGlassWallType = 89,
		WindowClosedGlassWallType = 151,
		WindowEdgeWallType = 34,
		WindowPlatformItemTypes = new int[1] { 4311 },
		PitTrapTileType = 53,
		LockedBiomeChestType = 467,
		LockedBiomeChestStyle = 13,
		BiomeChestItemType = 4712,
		BiomeChestLootItemType = 4607,
		ChestItemTypes = new int[1] { 4267 },
		DoorItemTypes = new int[1] { 4307 },
		PlatformItemTypes = new int[1] { 4311 },
		ChandelierItemTypes = new int[1] { 4305 },
		LanternItemTypes = new int[1] { 4309 },
		TableItemTypes = new int[1] { 4314 },
		WorkbenchItemTypes = new int[1] { 4315 },
		CandleItemTypes = new int[1] { 4303 },
		VaseOrStatueItemTypes = null,
		BookcaseItemTypes = new int[1] { 4300 },
		ChairItemTypes = new int[1] { 4304 },
		BedItemTypes = new int[1] { 4299 },
		PianoItemTypes = new int[1] { 4310 },
		DresserItemTypes = new int[1] { 4301 },
		SofaItemTypes = new int[1] { 4313 },
		BathtubItemTypes = new int[1] { 4298 },
		LampItemTypes = new int[1] { 4308 },
		CandelabraItemTypes = new int[1] { 4302 },
		ClockItemTypes = new int[1] { 4306 },
		BannerItemTypes = new int[3] { 790, 791, 789 },
		EdgeDither = false,
		BiomeRoomType = DungeonRoomType.BiomeRugged
	};

	public static DungeonGenerationStyleData Corruption = new DungeonGenerationStyleData
	{
		Style = 4,
		UnbreakableWallProgressionTier = DualDungeonUnbreakableWallTiers.EvilBoss,
		BrickTileType = 25,
		BrickCrackedTileType = 112,
		BrickWallType = 3,
		ExtraWallTypes = new ushort[4] { 188, 189, 190, 191 },
		WindowGlassWallType = 88,
		WindowClosedGlassWallType = 41,
		WindowEdgeWallType = 33,
		WindowPlatformItemTypes = new int[1] { 631 },
		PitTrapTileType = 112,
		LockedBiomeChestType = 21,
		LockedBiomeChestStyle = 24,
		BiomeChestItemType = 1529,
		BiomeChestLootItemType = 1571,
		ChestItemTypes = new int[3] { 625, 3965, 5763 },
		DoorItemTypes = new int[3] { 650, 3967, 5765 },
		PlatformItemTypes = new int[3] { 631, 3957, 5770 },
		ChandelierItemTypes = new int[3] { 2056, 3964, 5762 },
		LanternItemTypes = new int[3] { 2033, 3970, 5768 },
		TableItemTypes = new int[3] { 638, 3974, 5773 },
		WorkbenchItemTypes = new int[3] { 635, 3975, 5775 },
		CandleItemTypes = new int[3] { 2046, 3962, 5760 },
		VaseOrStatueItemTypes = null,
		BookcaseItemTypes = new int[3] { 2021, 3960, 5758 },
		ChairItemTypes = new int[3] { 628, 3963, 5761 },
		BedItemTypes = new int[3] { 644, 3959, 5757 },
		PianoItemTypes = new int[3] { 641, 3971, 5769 },
		DresserItemTypes = new int[3] { 647, 3968, 5766 },
		SofaItemTypes = new int[3] { 2398, 3973, 5772 },
		BathtubItemTypes = new int[3] { 2073, 3958, 5756 },
		LampItemTypes = new int[3] { 2083, 3969, 5767 },
		CandelabraItemTypes = new int[3] { 2093, 3961, 5759 },
		ClockItemTypes = new int[3] { 2593, 3966, 5764 },
		BannerItemTypes = null,
		EdgeDither = true,
		BiomeRoomType = DungeonRoomType.BiomeRugged
	};

	public static DungeonGenerationStyleData Crimson = new DungeonGenerationStyleData
	{
		Style = 5,
		UnbreakableWallProgressionTier = DualDungeonUnbreakableWallTiers.EvilBoss,
		BrickTileType = 203,
		BrickCrackedTileType = 234,
		BrickWallType = 83,
		ExtraWallTypes = new ushort[4] { 192, 193, 194, 195 },
		WindowGlassWallType = 92,
		WindowClosedGlassWallType = 85,
		WindowEdgeWallType = 174,
		WindowPlatformItemTypes = new int[1] { 913 },
		PitTrapTileType = 234,
		LockedBiomeChestType = 21,
		LockedBiomeChestStyle = 25,
		BiomeChestItemType = 1530,
		BiomeChestLootItemType = 1569,
		ChestItemTypes = new int[3] { 914, 2617, 5784 },
		DoorItemTypes = new int[3] { 912, 817, 5786 },
		PlatformItemTypes = new int[3] { 913, 3907, 5791 },
		ChandelierItemTypes = new int[3] { 2142, 2057, 5783 },
		LanternItemTypes = new int[3] { 2146, 2034, 5789 },
		TableItemTypes = new int[3] { 917, 828, 5794 },
		WorkbenchItemTypes = new int[3] { 916, 813, 5796 },
		CandleItemTypes = new int[3] { 2154, 2047, 5781 },
		VaseOrStatueItemTypes = null,
		BookcaseItemTypes = new int[3] { 2136, 2022, 5779 },
		ChairItemTypes = new int[3] { 915, 809, 5782 },
		BedItemTypes = new int[3] { 920, 2067, 5778 },
		PianoItemTypes = new int[3] { 919, 2246, 5790 },
		DresserItemTypes = new int[3] { 918, 2640, 5787 },
		SofaItemTypes = new int[3] { 2401, 2634, 5793 },
		BathtubItemTypes = new int[3] { 2127, 2074, 5777 },
		LampItemTypes = new int[3] { 2132, 2084, 5788 },
		CandelabraItemTypes = new int[3] { 2150, 2094, 5780 },
		ClockItemTypes = new int[3] { 2604, 2598, 5785 },
		BannerItemTypes = null,
		EdgeDither = true,
		BiomeRoomType = DungeonRoomType.BiomeRugged
	};

	public static DungeonGenerationStyleData Crystal = new ShimmerStyleData
	{
		Style = 15,
		UnbreakableWallProgressionTier = DualDungeonUnbreakableWallTiers.Hallow,
		BrickTileType = 385,
		BrickCrackedTileType = 116,
		BrickWallType = 186,
		WindowGlassWallType = 88,
		WindowClosedGlassWallType = 43,
		WindowEdgeWallType = 22,
		WindowPlatformItemTypes = new int[1] { 633 },
		PitTrapTileType = 116,
		LockedBiomeChestType = -1,
		LockedBiomeChestStyle = -1,
		BiomeChestItemType = -1,
		BiomeChestLootItemType = -1,
		ChestItemTypes = new int[1] { 3884 },
		DoorItemTypes = new int[1] { 3888 },
		PlatformItemTypes = new int[1] { 3903 },
		ChandelierItemTypes = new int[1] { 3894 },
		LanternItemTypes = new int[1] { 3891 },
		TableItemTypes = new int[1] { 3920 },
		WorkbenchItemTypes = new int[1] { 3909 },
		CandleItemTypes = new int[1] { 3890 },
		VaseOrStatueItemTypes = null,
		BookcaseItemTypes = new int[1] { 3917 },
		ChairItemTypes = new int[1] { 3889 },
		BedItemTypes = new int[1] { 3897 },
		PianoItemTypes = new int[1] { 3915 },
		DresserItemTypes = new int[1] { 3911 },
		SofaItemTypes = new int[1] { 3918 },
		BathtubItemTypes = new int[1] { 3895 },
		LampItemTypes = new int[1] { 3892 },
		CandelabraItemTypes = new int[1] { 3893 },
		ClockItemTypes = new int[1] { 3898 },
		BannerItemTypes = null,
		EdgeDither = false,
		BiomeRoomType = DungeonRoomType.BiomeStructured
	};

	public static DungeonGenerationStyleData Hallow = new DungeonGenerationStyleData
	{
		Style = 6,
		UnbreakableWallProgressionTier = DualDungeonUnbreakableWallTiers.Hallow,
		BrickTileType = 117,
		BrickCrackedTileType = 116,
		BrickWallType = 28,
		ExtraWallTypes = new ushort[4] { 200, 201, 202, 203 },
		WindowGlassWallType = 91,
		WindowClosedGlassWallType = 43,
		WindowEdgeWallType = 22,
		WindowPlatformItemTypes = new int[1] { 633 },
		PitTrapTileType = 116,
		LockedBiomeChestType = 21,
		LockedBiomeChestStyle = 26,
		BiomeChestItemType = 1531,
		BiomeChestLootItemType = 1260,
		ChestItemTypes = new int[2] { 627, 3884 },
		DoorItemTypes = new int[2] { 652, 3888 },
		PlatformItemTypes = new int[2] { 633, 3903 },
		ChandelierItemTypes = new int[2] { 2061, 3894 },
		LanternItemTypes = new int[2] { 2039, 3891 },
		TableItemTypes = new int[2] { 640, 3920 },
		WorkbenchItemTypes = new int[2] { 637, 3909 },
		CandleItemTypes = new int[2] { 2051, 3890 },
		VaseOrStatueItemTypes = null,
		BookcaseItemTypes = new int[2] { 2027, 3917 },
		ChairItemTypes = new int[2] { 630, 3889 },
		BedItemTypes = new int[2] { 646, 3897 },
		PianoItemTypes = new int[2] { 643, 3915 },
		DresserItemTypes = new int[2] { 649, 3911 },
		SofaItemTypes = new int[2] { 2400, 3918 },
		BathtubItemTypes = new int[2] { 2078, 3895 },
		LampItemTypes = new int[2] { 2088, 3892 },
		CandelabraItemTypes = new int[2] { 2099, 3893 },
		ClockItemTypes = new int[2] { 2602, 3898 },
		BannerItemTypes = null,
		EdgeDither = true,
		BiomeRoomType = DungeonRoomType.BiomeRugged,
		SubStyles = new List<DungeonGenerationStyleData> { Crystal }
	};

	public static DungeonGenerationStyleData GlowingMushroom = new DungeonGenerationStyleData
	{
		Style = 7,
		UnbreakableWallProgressionTier = DualDungeonUnbreakableWallTiers.JungleBoss,
		BrickTileType = 59,
		BrickGrassTileType = 70,
		BrickCrackedTileType = 123,
		BrickWallType = 80,
		WindowGlassWallType = 90,
		WindowClosedGlassWallType = 60,
		WindowEdgeWallType = 78,
		WindowPlatformItemTypes = new int[1] { 2549 },
		PitTrapTileType = 123,
		LockedBiomeChestType = -1,
		LockedBiomeChestStyle = -1,
		BiomeChestItemType = -1,
		BiomeChestLootItemType = -1,
		ChestItemTypes = new int[1] { 2544 },
		DoorItemTypes = new int[1] { 818 },
		PlatformItemTypes = new int[1] { 2549 },
		ChandelierItemTypes = new int[1] { 2543 },
		LanternItemTypes = new int[1] { 2546 },
		TableItemTypes = new int[1] { 2550 },
		WorkbenchItemTypes = new int[1] { 814 },
		CandleItemTypes = new int[1] { 2542 },
		VaseOrStatueItemTypes = null,
		BookcaseItemTypes = new int[1] { 2540 },
		ChairItemTypes = new int[1] { 810 },
		BedItemTypes = new int[1] { 2538 },
		PianoItemTypes = new int[1] { 2548 },
		DresserItemTypes = new int[1] { 2545 },
		SofaItemTypes = new int[1] { 2413 },
		BathtubItemTypes = new int[1] { 2537 },
		LampItemTypes = new int[1] { 2547 },
		CandelabraItemTypes = new int[1] { 2541 },
		ClockItemTypes = new int[1] { 2599 },
		BannerItemTypes = null,
		EdgeDither = true,
		BiomeRoomType = DungeonRoomType.BiomeRugged
	};

	public static DungeonGenerationStyleData Beehive = new BeehiveStyleData
	{
		Style = 9,
		UnbreakableWallProgressionTier = DualDungeonUnbreakableWallTiers.JungleBoss,
		BrickTileType = 225,
		BrickCrackedTileType = 123,
		BrickWallType = 86,
		WindowGlassWallType = 89,
		WindowClosedGlassWallType = 172,
		WindowEdgeWallType = 151,
		WindowPlatformItemTypes = new int[1] { 2630 },
		PitTrapTileType = 123,
		LiquidType = 2,
		LockedBiomeChestType = -1,
		LockedBiomeChestStyle = -1,
		BiomeChestItemType = -1,
		BiomeChestLootItemType = -1,
		ChestItemTypes = new int[1] { 2249 },
		DoorItemTypes = new int[1] { 1711 },
		PlatformItemTypes = new int[1] { 2630 },
		ChandelierItemTypes = new int[1] { 2058 },
		LanternItemTypes = new int[1] { 2035 },
		TableItemTypes = new int[1] { 1717 },
		WorkbenchItemTypes = new int[1] { 2251 },
		CandleItemTypes = new int[1] { 2648 },
		VaseOrStatueItemTypes = null,
		BookcaseItemTypes = new int[1] { 2023 },
		ChairItemTypes = new int[1] { 1707 },
		BedItemTypes = new int[1] { 1721 },
		PianoItemTypes = new int[1] { 2255 },
		DresserItemTypes = new int[1] { 2395 },
		SofaItemTypes = new int[1] { 2411 },
		BathtubItemTypes = new int[1] { 2124 },
		LampItemTypes = new int[1] { 2129 },
		CandelabraItemTypes = new int[1] { 2095 },
		ClockItemTypes = new int[1] { 2240 },
		BannerItemTypes = null,
		EdgeDither = true,
		BiomeRoomType = DungeonRoomType.BiomeRugged
	};

	public static DungeonGenerationStyleData LivingMahogany = new LivingWoodStyleData
	{
		Style = 14,
		UnbreakableWallProgressionTier = DualDungeonUnbreakableWallTiers.JungleBoss,
		BrickTileType = 383,
		BrickCrackedTileType = 384,
		BrickWallType = 244,
		WallPaintColor = 12,
		WindowGlassWallType = 21,
		WindowClosedGlassWallType = 42,
		WindowEdgeWallType = 196,
		WindowPlatformItemTypes = new int[1] { 2629 },
		PitTrapTileType = 123,
		LockedBiomeChestType = -1,
		LockedBiomeChestStyle = -1,
		BiomeChestItemType = -1,
		BiomeChestLootItemType = -1,
		ChestItemTypes = new int[1] { 831 },
		DoorItemTypes = new int[1] { 819 },
		PlatformItemTypes = new int[1] { 2629 },
		ChandelierItemTypes = new int[1] { 2141 },
		LanternItemTypes = new int[1] { 2145 },
		TableItemTypes = new int[1] { 829 },
		WorkbenchItemTypes = new int[1] { 2633 },
		CandleItemTypes = new int[1] { 2153 },
		VaseOrStatueItemTypes = null,
		BookcaseItemTypes = new int[1] { 2135 },
		ChairItemTypes = new int[1] { 806 },
		BedItemTypes = new int[1] { 2139 },
		PianoItemTypes = new int[1] { 2245 },
		DresserItemTypes = new int[1] { 3914 },
		SofaItemTypes = new int[1] { 2636 },
		BathtubItemTypes = new int[1] { 2126 },
		LampItemTypes = new int[1] { 2131 },
		CandelabraItemTypes = new int[1] { 2149 },
		ClockItemTypes = new int[1] { 2596 },
		BannerItemTypes = null,
		EdgeDither = true,
		BiomeRoomType = DungeonRoomType.BiomeRugged
	};

	public static DungeonGenerationStyleData Jungle = new DungeonGenerationStyleData
	{
		Style = 8,
		UnbreakableWallProgressionTier = DualDungeonUnbreakableWallTiers.JungleBoss,
		BrickTileType = 59,
		BrickGrassTileType = 60,
		BrickCrackedTileType = 123,
		BrickWallType = 64,
		ExtraWallTypes = new ushort[5] { 204, 205, 206, 207, 15 },
		WindowGlassWallType = 91,
		WindowClosedGlassWallType = 42,
		WindowEdgeWallType = 24,
		WindowPlatformItemTypes = new int[1] { 632 },
		PitTrapTileType = 123,
		LockedBiomeChestType = 21,
		LockedBiomeChestStyle = 23,
		BiomeChestItemType = 1528,
		BiomeChestLootItemType = 1156,
		ChestItemTypes = new int[2] { 626, 680 },
		DoorItemTypes = new int[1] { 651 },
		PlatformItemTypes = new int[1] { 632 },
		ChandelierItemTypes = new int[1] { 2060 },
		LanternItemTypes = new int[2] { 2038, 4578 },
		TableItemTypes = new int[1] { 639 },
		WorkbenchItemTypes = new int[1] { 636 },
		CandleItemTypes = new int[1] { 2050 },
		VaseOrStatueItemTypes = null,
		BookcaseItemTypes = new int[1] { 2026 },
		ChairItemTypes = new int[1] { 629 },
		BedItemTypes = new int[1] { 645 },
		PianoItemTypes = new int[1] { 642 },
		DresserItemTypes = new int[1] { 648 },
		SofaItemTypes = new int[1] { 2399 },
		BathtubItemTypes = new int[1] { 2077 },
		LampItemTypes = new int[1] { 2087 },
		CandelabraItemTypes = new int[1] { 2098 },
		ClockItemTypes = new int[1] { 2597 },
		BannerItemTypes = null,
		EdgeDither = true,
		BiomeRoomType = DungeonRoomType.BiomeRugged,
		SubStyles = new List<DungeonGenerationStyleData> { Beehive, LivingMahogany }
	};

	public static DungeonGenerationStyleData Temple = new TempleStyleData
	{
		Style = 10,
		BrickTileType = 226,
		BrickCrackedTileType = 123,
		BrickWallType = 87,
		WindowGlassWallType = 92,
		WindowClosedGlassWallType = 42,
		WindowEdgeWallType = 24,
		WindowPlatformItemTypes = new int[1] { 3906 },
		PitTrapTileType = 123,
		LockedBiomeChestType = -1,
		LockedBiomeChestStyle = -1,
		BiomeChestItemType = -1,
		BiomeChestLootItemType = -1,
		ChestItemTypes = new int[1] { 1142 },
		DoorItemTypes = new int[1] { 1137 },
		PlatformItemTypes = new int[1] { 3906 },
		ChandelierItemTypes = new int[1] { 2062 },
		LanternItemTypes = new int[1] { 2041 },
		TableItemTypes = new int[1] { 1144 },
		WorkbenchItemTypes = new int[1] { 1145 },
		CandleItemTypes = new int[1] { 2052 },
		VaseOrStatueItemTypes = new int[3] { 1152, 1153, 1154 },
		BookcaseItemTypes = new int[1] { 2030 },
		ChairItemTypes = new int[1] { 1143 },
		BedItemTypes = new int[1] { 2069 },
		PianoItemTypes = new int[1] { 2385 },
		DresserItemTypes = new int[1] { 2396 },
		SofaItemTypes = new int[1] { 2416 },
		BathtubItemTypes = new int[1] { 2079 },
		LampItemTypes = new int[1] { 2089 },
		CandelabraItemTypes = new int[1] { 2101 },
		ClockItemTypes = new int[1] { 2595 },
		BannerItemTypes = null,
		EdgeDither = false,
		BiomeRoomType = DungeonRoomType.BiomeStructured
	};

	public static DungeonGenerationStyleData GetCurrentDungeonStyle()
	{
		DungeonGenerationStyleData dungeonGenerationStyleData = new DungeonGenerationStyleData();
		dungeonGenerationStyleData.Style = 0;
		dungeonGenerationStyleData.UnbreakableWallProgressionTier = DualDungeonUnbreakableWallTiers.Dungeon;
		dungeonGenerationStyleData.BrickTileType = GenVars.CurrentDungeonGenVars.brickTileType;
		dungeonGenerationStyleData.BrickCrackedTileType = GenVars.CurrentDungeonGenVars.brickCrackedTileType;
		dungeonGenerationStyleData.BrickWallType = GenVars.CurrentDungeonGenVars.brickWallType;
		dungeonGenerationStyleData.WindowGlassWallType = GenVars.CurrentDungeonGenVars.windowGlassWallType;
		dungeonGenerationStyleData.WindowClosedGlassWallType = GenVars.CurrentDungeonGenVars.windowClosedGlassWallType;
		dungeonGenerationStyleData.WindowEdgeWallType = GenVars.CurrentDungeonGenVars.windowEdgeWallType;
		dungeonGenerationStyleData.WindowPlatformItemTypes = GenVars.CurrentDungeonGenVars.windowPlatformItemTypes;
		dungeonGenerationStyleData.PitTrapTileType = GenVars.CurrentDungeonGenVars.brickCrackedTileType;
		dungeonGenerationStyleData.LockedBiomeChestType = -1;
		dungeonGenerationStyleData.LockedBiomeChestStyle = -1;
		dungeonGenerationStyleData.BiomeChestItemType = -1;
		dungeonGenerationStyleData.BiomeChestLootItemType = -1;
		dungeonGenerationStyleData.ChestItemTypes = new int[0];
		dungeonGenerationStyleData.DoorItemTypes = new int[0];
		dungeonGenerationStyleData.PlatformItemTypes = new int[0];
		dungeonGenerationStyleData.ChandelierItemTypes = new int[0];
		dungeonGenerationStyleData.LanternItemTypes = new int[0];
		dungeonGenerationStyleData.TableItemTypes = new int[0];
		dungeonGenerationStyleData.WorkbenchItemTypes = new int[0];
		dungeonGenerationStyleData.CandleItemTypes = new int[0];
		dungeonGenerationStyleData.VaseOrStatueItemTypes = new int[0];
		dungeonGenerationStyleData.BookcaseItemTypes = new int[0];
		dungeonGenerationStyleData.ChairItemTypes = new int[0];
		dungeonGenerationStyleData.BedItemTypes = new int[0];
		dungeonGenerationStyleData.PianoItemTypes = new int[0];
		dungeonGenerationStyleData.DresserItemTypes = new int[0];
		dungeonGenerationStyleData.SofaItemTypes = new int[0];
		dungeonGenerationStyleData.BathtubItemTypes = new int[0];
		dungeonGenerationStyleData.LampItemTypes = new int[0];
		dungeonGenerationStyleData.CandelabraItemTypes = new int[0];
		dungeonGenerationStyleData.ClockItemTypes = new int[0];
		dungeonGenerationStyleData.BannerItemTypes = new int[0];
		dungeonGenerationStyleData.EdgeDither = false;
		dungeonGenerationStyleData.BiomeRoomType = DungeonRoomType.BiomeStructured;
		return dungeonGenerationStyleData;
	}

	public static DungeonGenerationStyleData GetStyleForTile(List<DungeonGenerationStyleData> styles, int tileType)
	{
		foreach (DungeonGenerationStyleData style in styles)
		{
			if (style.TileIsInStyle(tileType, includeCracked: false, includeExtras: true))
			{
				return style;
			}
			if (style.SubStyles == null || style.SubStyles.Count <= 0)
			{
				continue;
			}
			foreach (DungeonGenerationStyleData subStyle in style.SubStyles)
			{
				if (subStyle.TileIsInStyle(tileType, includeCracked: false, includeExtras: true))
				{
					return subStyle;
				}
			}
		}
		return null;
	}

	public static DungeonGenerationStyleData GetStyleForWall(List<DungeonGenerationStyleData> styles, int wallType)
	{
		foreach (DungeonGenerationStyleData style in styles)
		{
			if (style.Style == 0 && wallType >= 0 && wallType < WallID.Count && Main.wallDungeon[wallType])
			{
				return style;
			}
			if (style.WallIsInStyle(wallType, includeWindows: false, includeExtras: true))
			{
				return style;
			}
			if (style.SubStyles == null || style.SubStyles.Count <= 0)
			{
				continue;
			}
			foreach (DungeonGenerationStyleData subStyle in style.SubStyles)
			{
				if (subStyle.WallIsInStyle(wallType, includeWindows: false, includeExtras: true))
				{
					return subStyle;
				}
			}
		}
		return null;
	}
}
