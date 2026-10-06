using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using ReLogic.Utilities;
using Terraria.DataStructures;
using Terraria.GameContent.Biomes;
using Terraria.GameContent.Generation.Dungeon;

namespace Terraria.WorldBuilding;

public static class GenVars
{
	[JsonIgnore]
	public static WorldGenConfiguration configuration;

	public static StructureMap structures;

	public static int copper;

	public static int iron;

	public static int silver;

	public static int gold;

	public static int copperBar = 20;

	public static int ironBar = 22;

	public static int silverBar = 21;

	public static int goldBar = 19;

	public static bool worldSpawnHasBeenRandomized = false;

	public static List<LandmassData> landmassData = new List<LandmassData>();

	public static int remixSurfaceLayerLow;

	public static int remixSurfaceLayerHigh;

	public static int remixMushroomLayerLow;

	public static int remixMushroomLayerHigh;

	public static int lowestCloud = -1;

	public static int boulderPetsPlaced = 0;

	public static ushort crimStoneWall = 83;

	public static ushort crimStone = 203;

	public static ushort ebonStoneWall = 3;

	public static ushort ebonStone = 25;

	public static ushort mossTile = 179;

	public static ushort mossWall = 54;

	public static int lavaLine;

	public static int waterLine;

	public static double worldSurfaceLow;

	public static double worldSurface;

	public static double worldSurfaceHigh;

	public static double rockLayerLow;

	public static double rockLayer;

	public static double rockLayerHigh;

	public static int snowTop;

	public static int snowBottom;

	public static int snowOriginLeft;

	public static int snowOriginRight;

	public static int[] snowMinX;

	public static int[] snowMaxX;

	public static int leftBeachEnd;

	public static int rightBeachStart;

	public static int beachBordersWidth;

	public static int beachSandRandomCenter;

	public static int beachSandRandomWidthRange;

	public static int beachSandDungeonExtraWidth;

	public static int beachSandJungleExtraWidth;

	public static int shellStartXLeft;

	public static int shellStartYLeft;

	public static int shellStartXRight;

	public static int shellStartYRight;

	public static int oceanWaterStartRandomMin;

	public static int oceanWaterStartRandomMax;

	public static int oceanWaterForcedJungleLength;

	public static int evilBiomeBeachAvoidance;

	public static int evilBiomeAvoidanceMidFixer;

	public static int lakesBeachAvoidance;

	public static int smallHolesBeachAvoidance;

	public static int surfaceCavesBeachAvoidance;

	public static int surfaceCavesBeachAvoidance2;

	public static readonly int maxOceanCaveTreasure = 2;

	public static int numOceanCaveTreasure = 0;

	public static Point[] oceanCaveTreasure = new Point[maxOceanCaveTreasure];

	public static bool skipDesertTileCheck = false;

	public static Rectangle UndergroundDesertLocation = Rectangle.Empty;

	public static Rectangle UndergroundDesertHiveLocation = Rectangle.Empty;

	public static int desertHiveHigh;

	public static int desertHiveLow;

	public static int desertHiveLeft;

	public static int desertHiveRight;

	public static int numLarva;

	public static int[] larvaY = new int[100];

	public static int[] larvaX = new int[100];

	public static int numPyr;

	public static int[] PyrX;

	public static int[] PyrY;

	public static int extraBastStatueCount;

	public static int extraBastStatueCountMax;

	public static int jungleOriginX;

	public static int jungleMinX;

	public static int jungleMaxX;

	public static int JungleX;

	public static ushort jungleHut;

	public static bool mudWall;

	public static int JungleItemCount;

	public static bool gennedLivingMahoganyWands;

	public static int[] JChestX = new int[100];

	public static int[] JChestY = new int[100];

	public static int numJChests;

	public static int tLeft;

	public static int tRight;

	public static int tTop;

	public static int tBottom;

	public static int tRooms;

	public static int lAltarX;

	public static int lAltarY;

	public static List<DungeonGenVars> dungeonGenVars = new List<DungeonGenVars>();

	private static int _currentDungeon;

	public static bool DualDungeon_AllowTrapGenerationInDungeon = false;

	public static readonly int dungeonBeachPadding = 50;

	public static int skyLakes;

	public static bool generatedShadowKey;

	public static bool generatedRamRune;

	public static int numIslandHouses;

	public static int skyIslandHouseCount;

	public static bool[] skyLake = new bool[300];

	public static int[] floatingIslandHouseX = new int[300];

	public static int[] floatingIslandHouseY = new int[300];

	public static int[] floatingIslandStyle = new int[300];

	public static int numMCaves;

	public static int[] mCaveX = new int[30];

	public static int[] mCaveY = new int[30];

	public static readonly int maxTunnels = 50;

	public static int numTunnels;

	public static int[] tunnelX = new int[maxTunnels];

	public static readonly int maxOrePatch = 50;

	public static int numOrePatch;

	public static int[] orePatchX = new int[maxOrePatch];

	public static readonly int maxMushroomBiomes = 50;

	public static int numMushroomBiomes = 0;

	public static Point[] mushroomBiomesPosition = new Point[maxMushroomBiomes];

	public static int logX;

	public static int logY;

	public static readonly int maxLakes = 50;

	public static int numLakes = 0;

	public static int[] LakeX = new int[maxLakes];

	public static readonly int maxOasis = 20;

	public static int numOasis = 0;

	public static Point[] oasisPosition = new Point[maxOasis];

	public static int[] oasisWidth = new int[maxOasis];

	public static readonly int oasisHeight = 20;

	public static int hellChest;

	public static int[] hellChestItem;

	public static Point16[] statueList;

	public static List<int> StatuesWithTraps = new List<int>(new int[4] { 4, 7, 10, 18 });

	public static bool crimsonLeft = true;

	public static Vector2D shimmerPosition;

	public static bool notTheBeesAndForTheWorthyNoCelebration;

	public static bool noTrapsAndForTheWorthyNoCelebration;

	public static bool flipInfections;

	public static int CurrentDungeon
	{
		get
		{
			return _currentDungeon;
		}
		set
		{
			_currentDungeon = (int)MathHelper.Max(0f, (float)value);
		}
	}

	public static DungeonGenVars CurrentDungeonGenVars => dungeonGenVars[CurrentDungeon];

	public static double DualDungeon_NormalizedDistanceSafeFromDither
	{
		get
		{
			return DungeonControlLine.NormalizedDistanceSafeFromDither;
		}
		set
		{
			DungeonControlLine.NormalizedDistanceSafeFromDither = value;
		}
	}

	public static bool hardMode
	{
		get
		{
			return Main.hardMode;
		}
		set
		{
			Main.hardMode = value;
		}
	}

	public static int spawnTileX
	{
		get
		{
			return Main.spawnTileX;
		}
		set
		{
			Main.spawnTileX = value;
		}
	}

	public static int spawnTileY
	{
		get
		{
			return Main.spawnTileY;
		}
		set
		{
			Main.spawnTileY = value;
		}
	}

	public static bool[] townNPCCanSpawn
	{
		get
		{
			return Main.townNPCCanSpawn;
		}
		set
		{
			Main.townNPCCanSpawn = value;
		}
	}

	public static bool[] tileSolid
	{
		get
		{
			return Main.tileSolid;
		}
		set
		{
			Main.tileSolid = value;
		}
	}

	public static double mainWorldSurface
	{
		get
		{
			return Main.worldSurface;
		}
		set
		{
			Main.worldSurface = value;
		}
	}

	public static double mainRockLayer
	{
		get
		{
			return Main.rockLayer;
		}
		set
		{
			Main.rockLayer = value;
		}
	}

	public static int mainDungeonX
	{
		get
		{
			return Main.dungeonX;
		}
		set
		{
			Main.dungeonX = value;
		}
	}

	public static int mainDungeonY
	{
		get
		{
			return Main.dungeonY;
		}
		set
		{
			Main.dungeonY = value;
		}
	}

	static GenVars()
	{
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
	}
}
