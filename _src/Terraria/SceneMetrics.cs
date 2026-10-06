using System;
using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;
using Terraria.GameContent;
using Terraria.GameContent.Events;
using Terraria.ID;
using Terraria.WorldBuilding;

namespace Terraria;

public class SceneMetrics
{
	private static readonly Point AssumedConstantScreenSize = new Point(1920, 1200);

	private static readonly int ZoneScanPadding = 25;

	public static readonly Point ZoneScanSize = new Point(AssumedConstantScreenSize.X / 16 + ZoneScanPadding * 2 - 1, AssumedConstantScreenSize.Y / 16 + ZoneScanPadding * 2 - 1);

	public static readonly Vector2 TownNPCRectSize = AssumedConstantScreenSize.ToVector2() * 2f;

	private int _bestOreDistSq;

	public int BestOreType;

	public static int ShimmerTileThreshold = 300;

	public static int CorruptionTileThreshold = 300;

	public static int CorruptionTileMax = 1000;

	public static int CrimsonTileThreshold = 300;

	public static int CrimsonTileMax = 1000;

	public static int HallowTileThreshold = 125;

	public static int HallowTileMax = 600;

	public static int JungleTileThreshold = 140;

	public static int JungleTileMax = 700;

	public static int SnowTileNormalThreshold = 1500;

	public static int SnowTileSkyblockThreshold = 300;

	public static int SnowTileMax = 6000;

	public static int DesertTileNormalThreshold = 1500;

	public static int DesertTileSkyblockThreshold = 300;

	public static int MushroomTileThreshold = 100;

	public static int MushroomTileMax = 160;

	public static int MeteorTileThreshold = 75;

	public static int DungeonTileThreshold = 250;

	public static int GraveyardTileMax = 36;

	public static int GraveyardTileMin = 16;

	public static int GraveyardTileThreshold = 28;

	public bool BelowSurface;

	public bool ZoneSkyHeight;

	public bool ZoneOverworldHeight;

	public bool ZoneDirtLayerHeight;

	public bool ZoneRockLayerHeight;

	public bool ZoneUnderworldHeight;

	public bool ZoneCorrupt;

	public bool ZoneCrimson;

	public bool ZoneHallow;

	public bool ZoneJungle;

	public bool ZoneSnow;

	public bool ZoneDesert;

	public bool ZoneGlowshroom;

	public bool ZoneMeteor;

	public bool ZoneGraveyard;

	public bool ZoneDungeon;

	public bool ZoneLihzhardTemple;

	public bool ZoneGranite;

	public bool ZoneMarble;

	public bool ZoneHive;

	public bool ZoneGemCave;

	public bool ZoneBeach;

	public bool ZoneUndergroundDesert;

	public bool ZoneRain;

	public bool ZoneSandstorm;

	public bool SurfaceAtmospherics;

	public bool UndergroundForShimmering;

	public bool ZoneShimmer;

	public bool ZoneWaterCandle;

	public bool ZonePeaceCandle;

	public bool ZoneShadowCandle;

	public bool InTorchGodMinigame;

	public static int NPCEventZoneRadius = 4000;

	public bool CanPlayCreditsRoll;

	public bool[] NPCBannerBuff = new bool[BannerSystem.MaxBannerTypes];

	public bool hasBanner;

	public Vector2[] ClosestNPCPosition = new Vector2[NPCID.Count];

	private static Player _dummyPlayer = new Player();

	private readonly int[] _tileCounts = new int[TileID.Count];

	private readonly int[] _liquidCounts = new int[LiquidID.Count];

	public uint LastScanTime { get; private set; }

	public Vector2 Center
	{
		[CompilerGenerated]
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return field;
		}
		[CompilerGenerated]
		private set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			field = value;
		}
	}

	public Point TileCenter
	{
		[CompilerGenerated]
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return field;
		}
		[CompilerGenerated]
		private set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			field = value;
		}
	}

	public Point BestOrePosition
	{
		[CompilerGenerated]
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return field;
		}
		[CompilerGenerated]
		private set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			field = value;
		}
	}

	public static int SnowTileThreshold
	{
		get
		{
			if (WorldGen.Skyblock.lowTiles)
			{
				return SnowTileSkyblockThreshold;
			}
			return SnowTileNormalThreshold;
		}
	}

	public static int DesertTileThreshold
	{
		get
		{
			if (WorldGen.Skyblock.lowTiles)
			{
				return DesertTileSkyblockThreshold;
			}
			return DesertTileNormalThreshold;
		}
	}

	public int ShimmerTileCount { get; set; }

	public int EvilTileCount { get; set; }

	public int HolyTileCount { get; set; }

	public int HoneyBlockCount { get; set; }

	public int ActiveMusicBox { get; set; }

	public bool MusicBoxSilence { get; set; }

	public int SandTileCount { get; private set; }

	public int MushroomTileCount { get; private set; }

	public int SnowTileCount { get; private set; }

	public int WaterCandleCount { get; private set; }

	public int PeaceCandleCount { get; private set; }

	public int ShadowCandleCount { get; private set; }

	public int PartyMonolithCount { get; private set; }

	public int MeteorTileCount { get; private set; }

	public int BloodTileCount { get; private set; }

	public int JungleTileCount { get; private set; }

	public int DungeonTileCount { get; private set; }

	public bool HasSunflower { get; private set; }

	public bool HasGardenGnome { get; private set; }

	public bool HasClock { get; private set; }

	public bool HasCampfire { get; private set; }

	public bool HasStarInBottle { get; private set; }

	public bool HasHeartLantern { get; private set; }

	public int ActiveFountainColor { get; private set; }

	public int ActiveMonolithType { get; private set; }

	public bool BloodMoonMonolith { get; private set; }

	public bool MoonLordMonolith { get; private set; }

	public bool EchoMonolith { get; private set; }

	public int ShimmerMonolithState { get; private set; }

	public bool CRTMonolith { get; private set; }

	public bool RetroMonolith { get; private set; }

	public bool NoirMonolith { get; private set; }

	public bool RadioThingMonolith { get; private set; }

	public bool HasCatBast { get; private set; }

	public int GraveyardTileCount { get; private set; }

	public int DesertSandTileCount { get; private set; }

	public int OceanSandTileCount { get; private set; }

	public bool EnoughTilesForShimmer => ShimmerTileCount >= ShimmerTileThreshold;

	public bool EnoughTilesForJungle => JungleTileCount >= JungleTileThreshold;

	public bool EnoughTilesForHallow => HolyTileCount >= HallowTileThreshold;

	public bool EnoughTilesForSnow => SnowTileCount >= SnowTileThreshold;

	public bool EnoughTilesForGlowingMushroom => MushroomTileCount >= MushroomTileThreshold;

	public bool EnoughTilesForDesert => DesertSandTileCount >= DesertTileThreshold;

	public bool EnoughTilesForCorruption => EvilTileCount >= CorruptionTileThreshold;

	public bool EnoughTilesForCrimson => BloodTileCount >= CrimsonTileThreshold;

	public bool EnoughTilesForMeteor => MeteorTileCount >= MeteorTileThreshold;

	public bool EnoughTilesForDungeon => DungeonTileCount >= DungeonTileThreshold;

	public bool EnoughTilesForGraveyard => GraveyardTileCount >= GraveyardTileThreshold;

	public bool BehindBackwall { get; private set; }

	public bool CloseEnoughToSolarTower => WithinRangeOfNPC(517, NPCEventZoneRadius);

	public bool CloseEnoughToVortexTower => WithinRangeOfNPC(422, NPCEventZoneRadius);

	public bool CloseEnoughToNebulaTower => WithinRangeOfNPC(507, NPCEventZoneRadius);

	public bool CloseEnoughToStardustTower => WithinRangeOfNPC(493, NPCEventZoneRadius);

	public bool CloseEnoughToDD2LanePortal => WithinRangeOfNPC(549, NPCEventZoneRadius);

	public float? DistanceToMoonLord
	{
		get
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			Vector2 val = ClosestNPCPosition[398];
			if (val == Vector2.Zero)
			{
				return null;
			}
			return Vector2.Distance(Center, val);
		}
	}

	public float? MoonLordSkyIntensity
	{
		get
		{
			float? distanceToMoonLord = Main.SceneMetrics.DistanceToMoonLord;
			if (distanceToMoonLord.HasValue)
			{
				float value = distanceToMoonLord.Value;
				return 1f - Utils.SmoothStep(3000f, 6000f, value);
			}
			return null;
		}
	}

	public int TownNPCCount { get; private set; }

	public Player PerspectivePlayer { get; private set; }

	public bool AnyNPCs(int type)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		return ClosestNPCPosition[type] != Vector2.Zero;
	}

	public SceneMetrics()
	{
		Reset();
	}

	public void Scan(SceneMetricsScanSettings settings)
	{
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		if (LastScanTime != Main.GameUpdateCount || !(Center == settings.BiomeScanCenterPositionInWorld))
		{
			Reset();
			LastScanTime = Main.GameUpdateCount;
			Center = settings.BiomeScanCenterPositionInWorld;
			TileCenter = Center.ToTileCoordinates().ClampedInWorld();
			ScanTiles();
			if (settings.VisualScanArea.HasValue)
			{
				ScanOnScreenTiles(settings.VisualScanArea.Value);
			}
			if (settings.ScanNPCPositions)
			{
				ScanNPCPositions();
			}
			AggregateTileCounts();
			CalculateZones();
			if (settings.PerspectivePlayer != null)
			{
				AddPlayerEffects(settings.PerspectivePlayer);
			}
			CanPlayCreditsRoll = ActiveMusicBox == 85;
		}
	}

	private void ScanTiles()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d9: Unknown result type (might be due to invalid IL or missing references)
		Rectangle tileRectangle = Utils.CenteredRectangle(TileCenter, ZoneScanSize);
		tileRectangle = WorldUtils.ClampToWorld(tileRectangle);
		for (int i = tileRectangle.Left; i < tileRectangle.Right; i++)
		{
			for (int j = tileRectangle.Top; j < tileRectangle.Bottom; j++)
			{
				Tile tile = Main.tile[i, j];
				if (tile == null)
				{
					continue;
				}
				if (!tile.active())
				{
					if (tile.liquid > 0)
					{
						_liquidCounts[tile.liquidType()]++;
					}
					continue;
				}
				_tileCounts[tile.type]++;
				if (TileID.Sets.isDesertBiomeSand[tile.type] && WorldGen.oceanDepths(i, j))
				{
					OceanSandTileCount++;
				}
				if (TileID.Sets.Campfires[tile.type] && tile.frameY < 36)
				{
					HasCampfire = true;
				}
				if (tile.type == 49 && tile.frameX < 18)
				{
					WaterCandleCount++;
				}
				if (tile.type == 372 && tile.frameX < 18)
				{
					PeaceCandleCount++;
				}
				if (tile.type == 646 && tile.frameX < 18)
				{
					ShadowCandleCount++;
				}
				if (tile.type == 405 && tile.frameX < 54)
				{
					HasCampfire = true;
				}
				if (tile.type == 506 && tile.frameX < 72)
				{
					HasCatBast = true;
				}
				if (tile.type == 42 && tile.frameY >= 324 && tile.frameY <= 358)
				{
					HasHeartLantern = true;
				}
				if (tile.type == 42 && tile.frameY >= 252 && tile.frameY <= 286)
				{
					HasStarInBottle = true;
				}
				if (tile.type == 91)
				{
					int num = tile.frameX / 18;
					for (short num2 = tile.frameY; num2 >= 54; num2 -= 54)
					{
						num += 111;
					}
					bool flag = false;
					if ((tile.frameX < 396 && tile.frameY < 54) || num == 311 || num == 312)
					{
						flag = true;
					}
					if (!flag)
					{
						int num3 = tile.frameX / 18 - 21;
						for (int num4 = tile.frameY; num4 >= 54; num4 -= 54)
						{
							num3 += 90;
							num3 += 21;
						}
						if (num >= 311)
						{
							num3--;
						}
						if (num >= 312)
						{
							num3--;
						}
						int num5 = BannerSystem.BannerToItem(num3);
						if (ItemID.Sets.BannerStrength.IndexInRange(num5) && ItemID.Sets.BannerStrength[num5].Enabled)
						{
							NPCBannerBuff[num3] = true;
							hasBanner = true;
						}
					}
				}
				UpdateOreFinder(new Point(i, j), tile);
			}
		}
	}

	private void ScanOnScreenTiles(Rectangle visualScanArea)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		visualScanArea = WorldUtils.ClampToWorld(visualScanArea);
		for (int i = visualScanArea.Left; i < visualScanArea.Right; i++)
		{
			for (int j = visualScanArea.Top; j < visualScanArea.Bottom; j++)
			{
				Tile tile = Main.tile[i, j];
				if (tile == null || !tile.active())
				{
					continue;
				}
				if (tile.type == 104)
				{
					HasClock = true;
				}
				switch (tile.type)
				{
				case 139:
					if (tile.frameX >= 36)
					{
						int num = tile.frameY / 36;
						if (num == 100)
						{
							MusicBoxSilence = true;
						}
						else
						{
							ActiveMusicBox = num;
						}
					}
					break;
				case 207:
					if (tile.frameY >= 72)
					{
						switch (tile.frameX / 36)
						{
						case 0:
							ActiveFountainColor = 0;
							break;
						case 1:
							ActiveFountainColor = 12;
							break;
						case 2:
							ActiveFountainColor = 3;
							break;
						case 3:
							ActiveFountainColor = 5;
							break;
						case 4:
							ActiveFountainColor = 2;
							break;
						case 5:
							ActiveFountainColor = 10;
							break;
						case 6:
							ActiveFountainColor = 4;
							break;
						case 7:
							ActiveFountainColor = 9;
							break;
						case 8:
							ActiveFountainColor = 8;
							break;
						case 9:
							ActiveFountainColor = 6;
							break;
						default:
							ActiveFountainColor = -1;
							break;
						}
					}
					break;
				case 410:
					if (tile.frameY >= 56)
					{
						int activeMonolithType = tile.frameX / 36;
						ActiveMonolithType = activeMonolithType;
					}
					break;
				case 509:
					if (tile.frameY >= 56)
					{
						ActiveMonolithType = 4;
					}
					break;
				case 480:
					if (tile.frameY >= 54)
					{
						BloodMoonMonolith = true;
					}
					break;
				case 657:
					if (tile.frameY >= 54)
					{
						EchoMonolith = true;
					}
					break;
				case 658:
				{
					int shimmerMonolithState = tile.frameY / 54;
					ShimmerMonolithState = shimmerMonolithState;
					break;
				}
				case 720:
					if (tile.frameY >= 54)
					{
						CRTMonolith = true;
					}
					break;
				case 721:
					if (tile.frameY >= 54)
					{
						RetroMonolith = true;
					}
					break;
				case 725:
					if (tile.frameY >= 54)
					{
						NoirMonolith = true;
					}
					break;
				case 733:
					if (tile.frameY >= 54)
					{
						RadioThingMonolith = true;
					}
					break;
				}
			}
		}
	}

	private void AggregateTileCounts()
	{
		int num = -10;
		if (Main.infectedSeed)
		{
			num *= 3;
		}
		if (_tileCounts[27] > 0)
		{
			HasSunflower = true;
		}
		if (_tileCounts[567] > 0)
		{
			HasGardenGnome = true;
		}
		ShimmerTileCount = _liquidCounts[3];
		HoneyBlockCount = _tileCounts[229];
		HolyTileCount = _tileCounts[109] + _tileCounts[492] + _tileCounts[110] + _tileCounts[113] + _tileCounts[117] + _tileCounts[116] + _tileCounts[164] + _tileCounts[403] + _tileCounts[402];
		SnowTileCount = _tileCounts[147] + _tileCounts[148] + _tileCounts[161] + _tileCounts[162] + _tileCounts[164] + _tileCounts[163] + _tileCounts[200];
		if (Main.remixWorld)
		{
			JungleTileCount = _tileCounts[60] + _tileCounts[61] + _tileCounts[62] + _tileCounts[74] + _tileCounts[225];
			EvilTileCount = _tileCounts[23] + _tileCounts[661] + _tileCounts[24] + _tileCounts[25] + _tileCounts[32] + _tileCounts[112] + _tileCounts[163] + _tileCounts[400] + _tileCounts[398] + _tileCounts[27] * num + _tileCounts[474];
			BloodTileCount = _tileCounts[199] + _tileCounts[662] + _tileCounts[201] + _tileCounts[203] + _tileCounts[200] + _tileCounts[401] + _tileCounts[399] + _tileCounts[234] + _tileCounts[352] + _tileCounts[27] * num + _tileCounts[195];
		}
		else
		{
			JungleTileCount = _tileCounts[60] + _tileCounts[61] + _tileCounts[62] + _tileCounts[74] + _tileCounts[226] + _tileCounts[225];
			EvilTileCount = _tileCounts[23] + _tileCounts[661] + _tileCounts[24] + _tileCounts[25] + _tileCounts[32] + _tileCounts[112] + _tileCounts[163] + _tileCounts[400] + _tileCounts[398] + _tileCounts[27] * num;
			BloodTileCount = _tileCounts[199] + _tileCounts[662] + _tileCounts[201] + _tileCounts[203] + _tileCounts[200] + _tileCounts[401] + _tileCounts[399] + _tileCounts[234] + _tileCounts[352] + _tileCounts[27] * num;
		}
		MushroomTileCount = _tileCounts[70] + _tileCounts[71] + _tileCounts[72] + _tileCounts[528];
		MeteorTileCount = _tileCounts[37];
		DungeonTileCount = _tileCounts[41] + _tileCounts[43] + _tileCounts[44] + _tileCounts[481] + _tileCounts[482] + _tileCounts[483];
		SandTileCount = _tileCounts[53] + _tileCounts[112] + _tileCounts[116] + _tileCounts[234] + _tileCounts[397] + _tileCounts[398] + _tileCounts[402] + _tileCounts[399] + _tileCounts[396] + _tileCounts[400] + _tileCounts[403] + _tileCounts[401];
		PartyMonolithCount = _tileCounts[455];
		GraveyardTileCount = _tileCounts[85];
		GraveyardTileCount -= _tileCounts[27] / 2;
		if (_tileCounts[27] > 0)
		{
			HasSunflower = true;
		}
		if (GraveyardTileCount > GraveyardTileMin)
		{
			HasSunflower = false;
		}
		if (GraveyardTileCount < 0)
		{
			GraveyardTileCount = 0;
		}
		if (HolyTileCount < 0)
		{
			HolyTileCount = 0;
		}
		if (EvilTileCount < 0)
		{
			EvilTileCount = 0;
		}
		if (BloodTileCount < 0)
		{
			BloodTileCount = 0;
		}
		int holyTileCount = HolyTileCount;
		HolyTileCount -= EvilTileCount;
		HolyTileCount -= BloodTileCount;
		EvilTileCount -= holyTileCount;
		BloodTileCount -= holyTileCount;
		if (HolyTileCount < 0)
		{
			HolyTileCount = 0;
		}
		if (EvilTileCount < 0)
		{
			EvilTileCount = 0;
		}
		if (BloodTileCount < 0)
		{
			BloodTileCount = 0;
		}
		DesertSandTileCount = Math.Max(0, SandTileCount - OceanSandTileCount);
	}

	private void CalculateZones()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0267: Unknown result type (might be due to invalid IL or missing references)
		//IL_0272: Unknown result type (might be due to invalid IL or missing references)
		//IL_02dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0346: Unknown result type (might be due to invalid IL or missing references)
		//IL_0363: Unknown result type (might be due to invalid IL or missing references)
		//IL_0400: Unknown result type (might be due to invalid IL or missing references)
		//IL_040b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0421: Unknown result type (might be due to invalid IL or missing references)
		//IL_042c: Unknown result type (might be due to invalid IL or missing references)
		Tile tileSafely = Framing.GetTileSafely(TileCenter);
		BehindBackwall = tileSafely.wall > 0;
		ZoneSkyHeight = (double)TileCenter.Y <= Main.worldSurface * 0.3499999940395355;
		ZoneOverworldHeight = (double)TileCenter.Y <= Main.worldSurface && (double)TileCenter.Y > Main.worldSurface * 0.3499999940395355;
		BelowSurface = (double)TileCenter.Y > Main.worldSurface;
		ZoneDirtLayerHeight = (double)TileCenter.Y <= Main.rockLayer && (double)TileCenter.Y > Main.worldSurface;
		ZoneRockLayerHeight = TileCenter.Y <= Main.UnderworldLayer && (double)TileCenter.Y > Main.rockLayer;
		ZoneUnderworldHeight = TileCenter.Y > Main.UnderworldLayer;
		ZoneCorrupt = EnoughTilesForCorruption;
		ZoneCrimson = EnoughTilesForCrimson;
		ZoneHallow = EnoughTilesForHallow;
		ZoneJungle = EnoughTilesForJungle && !ZoneUnderworldHeight;
		ZoneSnow = EnoughTilesForSnow;
		ZoneDesert = EnoughTilesForDesert;
		ZoneGlowshroom = EnoughTilesForGlowingMushroom;
		ZoneMeteor = EnoughTilesForMeteor;
		ZoneGraveyard = EnoughTilesForGraveyard;
		double num = Main.worldSurface;
		if (Main.drunkWorld || !Main.isThereAWorldSurface)
		{
			num = Math.Max(num, Main.dungeonY + 40);
		}
		ZoneDungeon = EnoughTilesForDungeon && (double)TileCenter.Y > num && Main.wallDungeon[tileSafely.wall];
		ZoneLihzhardTemple = tileSafely.wall == 87;
		ZoneGranite = tileSafely.wall == 184 || tileSafely.wall == 180;
		ZoneMarble = tileSafely.wall == 183 || tileSafely.wall == 178;
		ZoneHive = tileSafely.wall == 108 || tileSafely.wall == 86;
		ZoneGemCave = tileSafely.wall >= 48 && tileSafely.wall <= 53;
		ZoneBeach = WorldGen.oceanDepths(TileCenter.X, TileCenter.Y);
		ZoneUndergroundDesert = ZoneDesert && BelowSurface && (WallID.Sets.Conversion.Sandstone[tileSafely.wall] || WallID.Sets.Conversion.HardenedSand[tileSafely.wall] || tileSafely.wall == 223) && !Main.wallHouse[tileSafely.wall];
		SurfaceAtmospherics = WorldGen.IsSurfaceForAtmospherics(TileCenter);
		if (Main.remixWorld && ZoneDungeon)
		{
			SurfaceAtmospherics = false;
		}
		ZoneRain = Main.raining && SurfaceAtmospherics;
		ZoneSandstorm = ZoneDesert && SurfaceAtmospherics && Sandstorm.Happening;
		if (ZoneSandstorm)
		{
			ZoneRain = false;
		}
		UndergroundForShimmering = (double)TileCenter.Y > Main.worldSurface + 84.0 && TileCenter.Y < Main.maxTilesY - 396;
		ZoneShimmer = EnoughTilesForShimmer && UndergroundForShimmering && !ZoneDungeon;
		ZoneWaterCandle = WaterCandleCount > 0;
		ZonePeaceCandle = PeaceCandleCount > 0;
		ZoneShadowCandle = ShadowCandleCount > 0;
		if (Main.dualDungeonsSeed && BelowSurface && !ZoneUnderworldHeight)
		{
			NPCSpawningFlagsForDualDungeons nPCSpawningFlagsForDualDungeons = default;
			for (int i = 0; i < 300 && (!nPCSpawningFlagsForDualDungeons.CanScan(TileCenter.X, TileCenter.Y + i) || !nPCSpawningFlagsForDualDungeons.ScanZonesFor(TileCenter.X, TileCenter.Y + i)); i++)
			{
			}
			ZoneDungeon = nPCSpawningFlagsForDualDungeons.ZoneDungeon && Main.wallDungeon[tileSafely.wall];
			ZoneSnow = nPCSpawningFlagsForDualDungeons.ZoneSnow;
			ZoneGlowshroom = nPCSpawningFlagsForDualDungeons.ZoneGlowshroom;
			ZoneCorrupt = nPCSpawningFlagsForDualDungeons.ZoneCorrupt;
			ZoneCrimson = nPCSpawningFlagsForDualDungeons.ZoneCrimson;
			ZoneJungle = nPCSpawningFlagsForDualDungeons.ZoneJungle;
			ZoneHallow = nPCSpawningFlagsForDualDungeons.ZoneHallow;
			ZoneLihzhardTemple = nPCSpawningFlagsForDualDungeons.ZoneLihzhardTemple;
			ZoneUndergroundDesert = nPCSpawningFlagsForDualDungeons.ZoneUndergroundDesert && (WallID.Sets.Conversion.Sandstone[tileSafely.wall] || WallID.Sets.Conversion.HardenedSand[tileSafely.wall] || tileSafely.wall == 223) && !Main.wallHouse[tileSafely.wall];
		}
	}

	private void ScanNPCPositions()
	{
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		for (int i = 0; i < Main.maxNPCs; i++)
		{
			NPC nPC = Main.npc[i];
			if (!nPC.active)
			{
				continue;
			}
			Vector2 val = ClosestNPCPosition[nPC.type];
			if (val == Vector2.Zero || Vector2.DistanceSquared(Center, nPC.Center) < Vector2.DistanceSquared(Center, val))
			{
				ClosestNPCPosition[nPC.type] = nPC.Center;
			}
			if (nPC.townNPC)
			{
				Rectangle val2 = Utils.CenteredRectangle(Center, TownNPCRectSize);
				if (val2.Contains(nPC.Center.ToPoint()))
				{
					TownNPCCount++;
				}
			}
		}
	}

	private void AddPlayerEffects(Player player)
	{
		PerspectivePlayer = player;
		if (player.inventory[player.selectedItem].type == 148)
		{
			ZoneWaterCandle = true;
		}
		if (player.inventory[player.selectedItem].type == 3117)
		{
			ZonePeaceCandle = true;
		}
		if (player.inventory[player.selectedItem].type == 5322)
		{
			ZoneShadowCandle = true;
		}
		if (player.musicBox >= 0)
		{
			ActiveMusicBox = player.musicBox;
		}
		if (player.musicBoxSilence)
		{
			MusicBoxSilence = true;
		}
		if (player.happyFunTorchTime)
		{
			InTorchGodMinigame = true;
		}
	}

	public int GetTileCount(ushort tileId)
	{
		return _tileCounts[tileId];
	}

	public void Reset()
	{
		//IL_0259: Unknown result type (might be due to invalid IL or missing references)
		//IL_025f: Unknown result type (might be due to invalid IL or missing references)
		LastScanTime = uint.MaxValue;
		Array.Clear(_tileCounts, 0, _tileCounts.Length);
		Array.Clear(_liquidCounts, 0, _liquidCounts.Length);
		Array.Clear(ClosestNPCPosition, 0, ClosestNPCPosition.Length);
		SandTileCount = 0;
		EvilTileCount = 0;
		BloodTileCount = 0;
		GraveyardTileCount = 0;
		DesertSandTileCount = 0;
		MushroomTileCount = 0;
		SnowTileCount = 0;
		HolyTileCount = 0;
		HoneyBlockCount = 0;
		ShimmerTileCount = 0;
		MeteorTileCount = 0;
		JungleTileCount = 0;
		DungeonTileCount = 0;
		OceanSandTileCount = 0;
		HasCampfire = false;
		HasSunflower = false;
		HasGardenGnome = false;
		HasStarInBottle = false;
		HasHeartLantern = false;
		HasClock = false;
		HasCatBast = false;
		ActiveMusicBox = -1;
		MusicBoxSilence = false;
		WaterCandleCount = 0;
		PeaceCandleCount = 0;
		ShadowCandleCount = 0;
		ActiveFountainColor = -1;
		ActiveMonolithType = -1;
		PartyMonolithCount = 0;
		BloodMoonMonolith = false;
		MoonLordMonolith = false;
		EchoMonolith = false;
		ShimmerMonolithState = 0;
		CRTMonolith = false;
		RetroMonolith = false;
		NoirMonolith = false;
		RadioThingMonolith = false;
		BehindBackwall = false;
		BelowSurface = false;
		ZoneSkyHeight = false;
		ZoneOverworldHeight = false;
		ZoneDirtLayerHeight = false;
		ZoneRockLayerHeight = false;
		ZoneUnderworldHeight = false;
		ZoneCorrupt = false;
		ZoneCrimson = false;
		ZoneHallow = false;
		ZoneJungle = false;
		ZoneSnow = false;
		ZoneDesert = false;
		ZoneGlowshroom = false;
		ZoneMeteor = false;
		ZoneGraveyard = false;
		ZoneDungeon = false;
		ZoneLihzhardTemple = false;
		ZoneGranite = false;
		ZoneMarble = false;
		ZoneHive = false;
		ZoneGemCave = false;
		ZoneBeach = false;
		ZoneUndergroundDesert = false;
		SurfaceAtmospherics = false;
		ZoneRain = false;
		ZoneSandstorm = false;
		UndergroundForShimmering = false;
		ZoneShimmer = false;
		ZoneWaterCandle = false;
		ZonePeaceCandle = false;
		ZoneShadowCandle = false;
		InTorchGodMinigame = false;
		Array.Clear(NPCBannerBuff, 0, NPCBannerBuff.Length);
		hasBanner = false;
		CanPlayCreditsRoll = false;
		BestOreType = -1;
		BestOrePosition = default;
		_bestOreDistSq = int.MaxValue;
		TownNPCCount = 0;
		PerspectivePlayer = _dummyPlayer;
	}

	private void UpdateOreFinder(Point pos, Tile tile)
	{
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		int num = Main.tileOreFinderPriority[tile.type];
		if (num <= 0)
		{
			return;
		}
		int num2 = ((BestOreType < 0) ? (-1) : Main.tileOreFinderPriority[BestOreType]);
		if (num >= num2 && IsValidForOreFinder(tile))
		{
			Point val = new Point(pos.X - TileCenter.X, pos.Y - TileCenter.Y);
			int num3 = val.X * val.X + val.Y * val.Y;
			if (num != num2 || num3 < _bestOreDistSq)
			{
				BestOreType = tile.type;
				BestOrePosition = pos;
				_bestOreDistSq = num3;
			}
		}
	}

	public static bool IsValidForOreFinder(Tile t)
	{
		if (t.type == 227)
		{
			if (t.frameX >= 272)
			{
				return t.frameX <= 374;
			}
			return false;
		}
		if (t.type == 129)
		{
			return t.frameX >= 324;
		}
		return true;
	}

	public bool WithinRangeOfNPC(int type, double range)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		Vector2 val = ClosestNPCPosition[type];
		if (val != Vector2.Zero)
		{
			return (double)Vector2.DistanceSquared(Center, val) <= range * range;
		}
		return false;
	}

	static SceneMetrics()
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
	}
}
