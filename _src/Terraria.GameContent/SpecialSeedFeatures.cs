namespace Terraria.GameContent;

public class SpecialSeedFeatures
{
	public static bool ShouldDropExtraGel
	{
		get
		{
			if (Main.tenthAnniversaryWorld && Main.drunkWorld && !Main.remixWorld)
			{
				return !Main.notTheBeesWorld;
			}
			return false;
		}
	}

	public static bool ShouldDropExtraWood
	{
		get
		{
			if (Main.tenthAnniversaryWorld && Main.drunkWorld && !Main.remixWorld)
			{
				return !Main.notTheBeesWorld;
			}
			return false;
		}
	}

	public static bool DungeonEntranceHasATree
	{
		get
		{
			if (Main.drunkWorld)
			{
				return !NoDungeonGuardian;
			}
			return false;
		}
	}

	public static bool DungeonEntranceHasStairs
	{
		get
		{
			if (!DungeonEntranceIsUnderground)
			{
				return !WorldGen.SecretSeed.roundLandmasses.Enabled;
			}
			return false;
		}
	}

	public static bool DungeonEntranceIsBuried
	{
		get
		{
			if (WorldGen.SecretSeed.surfaceIsDesert.Enabled)
			{
				return !DungeonEntranceIsUnderground;
			}
			return false;
		}
	}

	public static bool DungeonEntranceIsUnderground
	{
		get
		{
			if (!Main.drunkWorld)
			{
				return WorldGen.SecretSeed.noSurface.Enabled;
			}
			return true;
		}
	}

	public static bool NoDungeonGuardian => Main.onlyShimmerOceanWorlds;

	public static bool BossesKeepSpawning
	{
		get
		{
			if (Main.getGoodWorld && Main.dontStarveWorld)
			{
				return !Main.tenthAnniversaryWorld;
			}
			return false;
		}
	}

	public static bool ShimmerSpawnHalfOfWorld => Main.onlyShimmerOceanWorlds;

	public static bool RainbowSandAndBlackSandWalls => Main.onlyShimmerOceanWorlds;

	public static bool SpawnOnBeach
	{
		get
		{
			if (Main.tenthAnniversaryWorld && !Main.remixWorld)
			{
				return !Main.dontStarveWorld;
			}
			return false;
		}
	}

	public static bool SpawnOnBeachOnDungeonSide
	{
		get
		{
			if (SpawnOnBeach)
			{
				return Main.onlyShimmerOceanWorlds;
			}
			return false;
		}
	}

	public static bool Mechdusa
	{
		get
		{
			if (Main.remixWorld)
			{
				return Main.getGoodWorld;
			}
			return false;
		}
	}
}
