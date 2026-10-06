using System;
using System.Collections.Generic;
using ReLogic.Reflection;
using Terraria.DataStructures;

namespace Terraria.ID;

public class BuffID
{
	public class Sets
	{
		public static SetFactory Factory = new SetFactory(Count);

		public static bool[] IsWellFed = Factory.CreateBoolSet(26, 206, 207);

		public static bool[] IsFedState = Factory.CreateBoolSet(26, 206, 207, 332, 333, 334);

		public static int[] SortingPriorityFoodBuffs = Factory.CreateIntSet(-1, 207, 4, 206, 3, 26, 2, 25, 1);

		public static bool[] IsAnNPCWhipDebuff = Factory.CreateBoolSet(310, 362);

		public static bool[] TimeLeftDoesNotDecrease = Factory.CreateBoolSet(28, 334, 29, 159, 150, 93, 348, 366);

		public static bool[] CanBeRemovedByNetMessage = Factory.CreateBoolSet();

		public static bool[] IsAFlaskBuff = Factory.CreateBoolSet(71, 72, 73, 74, 75, 76, 77, 78, 79);

		public static bool[] BuffTimeIsExtendedWithGameDifficulty = Factory.CreateBoolSet(20, 22, 23, 24, 323, 30, 31, 32, 33, 35, 36, 39, 44, 324, 46, 47, 69, 70, 80);

		public static bool[] BuffTimeIsExtendedByDeadCellsPotionStationBuff = Factory.CreateBoolSet(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 104, 105, 106, 107, 108, 109, 110, 111, 112, 113, 114, 115, 116, 117, 121, 122, 123, 124, 257, 343);

		public static bool[] NurseCannotRemoveDebuff = Factory.CreateBoolSet(28, 34, 87, 89, 21, 86, 199, 332, 333, 334, 165, 146, 48, 158, 157, 350, 215, 147, 321, 43);

		public static int[] AddBuffTimeAdditivelyToCap = Factory.CreateIntSet(0, 94, 600, 383, 43200);

		public static Dictionary<int, IBuffTextHandler> BuffTextHandlers = new Dictionary<int, IBuffTextHandler>
		{
			{
				64,
				new CachedProjectileCounterBuffTextHandler(266)
			},
			{
				125,
				new CachedProjectileCounterBuffTextHandler(373)
			},
			{
				49,
				new CachedProjectileCounterBuffTextHandler(191, 192, 193, 194)
			},
			{
				83,
				new CachedProjectileCounterBuffTextHandler(317)
			},
			{
				126,
				new CachedProjectileCounterBuffTextHandler(375)
			},
			{
				134,
				new CachedProjectileCounterBuffTextHandler(387)
			},
			{
				133,
				new CachedProjectileCounterBuffTextHandler(390, 391, 392)
			},
			{
				135,
				new CachedProjectileCounterBuffTextHandler(393, 394, 395)
			},
			{
				139,
				new CachedProjectileCounterBuffTextHandler(407)
			},
			{
				140,
				new CachedProjectileCounterBuffTextHandler(423)
			},
			{
				161,
				new CachedProjectileCounterBuffTextHandler(533)
			},
			{
				182,
				new CachedProjectileCounterBuffTextHandler(613)
			},
			{
				188,
				new CachedProjectileCounterBuffTextHandler(626)
			},
			{
				213,
				new CachedProjectileCounterBuffTextHandler(755)
			},
			{
				214,
				new CachedProjectileCounterBuffTextHandler(758)
			},
			{
				216,
				new CachedProjectileCounterBuffTextHandler(759)
			},
			{
				263,
				new CachedProjectileCounterBuffTextHandler(831)
			},
			{
				271,
				new CachedProjectileCounterBuffTextHandler(864)
			},
			{
				322,
				new CachedProjectileCounterBuffTextHandler(946)
			},
			{
				325,
				new CachedProjectileCounterBuffTextHandler(951)
			},
			{
				335,
				new CachedProjectileCounterBuffTextHandler(970)
			},
			{
				355,
				new CachedProjectileCounterBuffTextHandler(1022)
			},
			{
				385,
				new CachedProjectileCounterBuffTextHandler(1093)
			},
			{
				386,
				new CachedProjectileCounterBuffTextHandler(1094)
			},
			{
				389,
				new CachedProjectileCounterBuffTextHandler(1112)
			},
			{
				390,
				new CachedProjectileCounterBuffTextHandler(1113)
			},
			{
				393,
				new CachedProjectileCounterBuffTextHandler(1118)
			},
			{
				394,
				new CachedProjectileCounterBuffTextHandler(1119)
			}
		};

		public static int[] MountType = Factory.CreateIntSet(-1, 118, 6, 184, 13, 166, 11, 208, 15, 210, 16, 220, 18, 222, 19, 224, 20, 226, 21, 228, 22, 231, 24, 233, 25, 235, 26, 237, 27, 239, 28, 241, 29, 243, 30, 245, 31, 247, 32, 249, 33, 251, 34, 253, 35, 255, 36, 269, 38, 272, 39, 338, 51, 346, 53, 90, 0, 128, 1, 129, 2, 130, 3, 131, 4, 132, 5, 168, 12, 141, 7, 142, 8, 143, 9, 162, 10, 193, 14, 212, 17, 230, 23, 265, 37, 275, 40, 276, 41, 277, 42, 278, 43, 279, 44, 280, 45, 281, 46, 282, 47, 283, 48, 305, 49, 318, 50, 342, 52, 370, 54, 374, 55, 377, 56, 378, 57, 379, 58, 380, 59, 381, 60, 384, 61, 387, 62, 388, 63, 391, 64, 392, 65);
	}

	public const int ObsidianSkin = 1;

	public const int Regeneration = 2;

	public const int Swiftness = 3;

	public const int Gills = 4;

	public const int Ironskin = 5;

	public const int ManaRegeneration = 6;

	public const int MagicPower = 7;

	public const int Featherfall = 8;

	public const int Spelunker = 9;

	public const int Invisibility = 10;

	public const int Shine = 11;

	public const int NightOwl = 12;

	public const int Battle = 13;

	public const int Thorns = 14;

	public const int WaterWalking = 15;

	public const int Archery = 16;

	public const int Hunter = 17;

	public const int Gravitation = 18;

	public const int ShadowOrb = 19;

	public const int Poisoned = 20;

	public const int PotionSickness = 21;

	public const int Darkness = 22;

	public const int Cursed = 23;

	public const int OnFire = 24;

	public const int Tipsy = 25;

	public const int WellFed = 26;

	public const int FairyBlue = 27;

	public const int Werewolf = 28;

	public const int Clairvoyance = 29;

	public const int Bleeding = 30;

	public const int Confused = 31;

	public const int Slow = 32;

	public const int Weak = 33;

	public const int Merfolk = 34;

	public const int Silenced = 35;

	public const int BrokenArmor = 36;

	public const int Horrified = 37;

	public const int TheTongue = 38;

	public const int CursedInferno = 39;

	public const int PetBunny = 40;

	public const int BabyPenguin = 41;

	public const int PetTurtle = 42;

	public const int PaladinsShield = 43;

	public const int Frostburn = 44;

	public const int BabyEater = 45;

	public const int Chilled = 46;

	public const int Frozen = 47;

	public const int Honey = 48;

	public const int Pygmies = 49;

	public const int BabySkeletronHead = 50;

	public const int BabyHornet = 51;

	public const int TikiSpirit = 52;

	public const int PetLizard = 53;

	public const int PetParrot = 54;

	public const int BabyTruffle = 55;

	public const int PetSapling = 56;

	public const int Wisp = 57;

	public const int RapidHealing = 58;

	public const int ShadowDodge = 59;

	public const int LeafCrystal = 60;

	public const int BabyDinosaur = 61;

	public const int IceBarrier = 62;

	public const int Panic = 63;

	public const int BabySlime = 64;

	public const int EyeballSpring = 65;

	public const int BabySnowman = 66;

	public const int Burning = 67;

	public const int Suffocation = 68;

	public const int Ichor = 69;

	public const int Venom = 70;

	public const int WeaponImbueVenom = 71;

	public const int Midas = 72;

	public const int WeaponImbueCursedFlames = 73;

	public const int WeaponImbueFire = 74;

	public const int WeaponImbueGold = 75;

	public const int WeaponImbueIchor = 76;

	public const int WeaponImbueNanites = 77;

	public const int WeaponImbueConfetti = 78;

	public const int WeaponImbuePoison = 79;

	public const int Blackout = 80;

	public const int PetSpider = 81;

	public const int Squashling = 82;

	public const int Ravens = 83;

	public const int BlackCat = 84;

	public const int CursedSapling = 85;

	public const int WaterCandle = 86;

	public const int Campfire = 87;

	public const int ChaosState = 88;

	public const int HeartLamp = 89;

	public const int Rudolph = 90;

	public const int Puppy = 91;

	public const int BabyGrinch = 92;

	public const int AmmoBox = 93;

	public const int ManaSickness = 94;

	public const int BeetleEndurance1 = 95;

	public const int BeetleEndurance2 = 96;

	public const int BeetleEndurance3 = 97;

	public const int BeetleMight1 = 98;

	public const int BeetleMight2 = 99;

	public const int BeetleMight3 = 100;

	public const int FairyRed = 101;

	public const int FairyGreen = 102;

	public const int Wet = 103;

	public const int Mining = 104;

	public const int Heartreach = 105;

	public const int Calm = 106;

	public const int Builder = 107;

	public const int Titan = 108;

	public const int Flipper = 109;

	public const int Summoning = 110;

	public const int Dangersense = 111;

	public const int AmmoReservation = 112;

	public const int Lifeforce = 113;

	public const int Endurance = 114;

	public const int Rage = 115;

	public const int Inferno = 116;

	public const int Wrath = 117;

	public const int Minecart = 118;

	public const int Lovestruck = 119;

	public const int Stinky = 120;

	public const int Fishing = 121;

	public const int Sonar = 122;

	public const int Crate = 123;

	public const int Warmth = 124;

	public const int HornetMinion = 125;

	public const int ImpMinion = 126;

	public const int ZephyrFish = 127;

	public const int BunnyMount = 128;

	public const int PigronMount = 129;

	public const int SlimeMount = 130;

	public const int TurtleMount = 131;

	public const int BeeMount = 132;

	public const int SpiderMinion = 133;

	public const int TwinEyesMinion = 134;

	public const int PirateMinion = 135;

	public const int MiniMinotaur = 136;

	public const int Slimed = 137;

	public const int MinecartLegacyUnused = 138;

	public const int SharknadoMinion = 139;

	public const int UFOMinion = 140;

	public const int UFOMount = 141;

	public const int DrillMount = 142;

	public const int ScutlixMount = 143;

	public const int Electrified = 144;

	public const int MoonLeech = 145;

	public const int Sunflower = 146;

	public const int MonsterBanner = 147;

	public const int Rabies = 148;

	public const int Webbed = 149;

	public const int Bewitched = 150;

	public const int SoulDrain = 151;

	public const int MagicLantern = 152;

	public const int ShadowFlame = 153;

	public const int BabyFaceMonster = 154;

	public const int CrimsonHeart = 155;

	public const int Stoned = 156;

	public const int PeaceCandle = 157;

	public const int StarInBottle = 158;

	public const int Sharpened = 159;

	public const int Dazed = 160;

	public const int DeadlySphere = 161;

	public const int UnicornMount = 162;

	public const int Obstructed = 163;

	public const int VortexDebuff = 164;

	public const int DryadsWard = 165;

	public const int MinecartMech = 166;

	public const int MinecartMechLegacyUnused = 167;

	public const int CuteFishronMount = 168;

	public const int BoneJavelin = 169;

	public const int SolarShield1 = 170;

	public const int SolarShield2 = 171;

	public const int SolarShield3 = 172;

	public const int NebulaUpLife1 = 173;

	public const int NebulaUpLife2 = 174;

	public const int NebulaUpLife3 = 175;

	public const int NebulaUpMana1 = 176;

	public const int NebulaUpMana2 = 177;

	public const int NebulaUpMana3 = 178;

	public const int NebulaUpDmg1 = 179;

	public const int NebulaUpDmg2 = 180;

	public const int NebulaUpDmg3 = 181;

	public const int StardustMinion = 182;

	public const int StardustMinionBleed = 183;

	public const int MinecartWood = 184;

	public const int MinecartWoodLegacyUnused = 185;

	public const int DryadsWardDebuff = 186;

	public const int StardustGuardianMinion = 187;

	public const int StardustDragonMinion = 188;

	public const int Daybreak = 189;

	public const int SuspiciousTentacle = 190;

	public const int CompanionCube = 191;

	public const int SugarRush = 192;

	public const int BasiliskMount = 193;

	public const int WindPushed = 194;

	public const int WitheredArmor = 195;

	public const int WitheredWeapon = 196;

	public const int OgreSpit = 197;

	public const int ParryDamageBuff = 198;

	public const int NoBuilding = 199;

	public const int PetDD2Gato = 200;

	public const int PetDD2Ghost = 201;

	public const int PetDD2Dragon = 202;

	public const int BetsysCurse = 203;

	public const int Oiled = 204;

	public const int BallistaPanic = 205;

	public const int WellFed2 = 206;

	public const int WellFed3 = 207;

	public const int DesertMinecart = 208;

	public const int DesertMinecartLegacyUnused = 209;

	public const int FishMinecart = 210;

	public const int FishMinecartLegacyUnused = 211;

	public const int GolfCartMount = 212;

	public const int BatOfLight = 213;

	public const int VampireFrog = 214;

	public const int CatBast = 215;

	public const int BabyBird = 216;

	public const int UpbeatStar = 217;

	public const int SugarGlider = 218;

	public const int SharkPup = 219;

	public const int BeeMinecart = 220;

	public const int BeeMinecartLegacyUnused = 221;

	public const int LadybugMinecart = 222;

	public const int LadybugMinecartLegacyUnused = 223;

	public const int PigronMinecart = 224;

	public const int PigronMinecartLegacyUnused = 225;

	public const int SunflowerMinecart = 226;

	public const int SunflowerMinecartLegacyUnused = 227;

	public const int HellMinecart = 228;

	public const int HellMinecartLegacyUnused = 229;

	public const int WitchBroom = 230;

	public const int ShroomMinecart = 231;

	public const int ShroomMinecartLegacyUnused = 232;

	public const int AmethystMinecart = 233;

	public const int AmethystMinecartLegacyUnused = 234;

	public const int TopazMinecart = 235;

	public const int TopazMinecartLegacyUnused = 236;

	public const int SapphireMinecart = 237;

	public const int SapphireMinecartLegacyUnused = 238;

	public const int EmeraldMinecart = 239;

	public const int EmeraldMinecartLegacyUnused = 240;

	public const int RubyMinecart = 241;

	public const int RubyMinecartLegacyUnused = 242;

	public const int DiamondMinecart = 243;

	public const int DiamondMinecartLegacyUnused = 244;

	public const int AmberMinecart = 245;

	public const int AmberMinecartLegacyUnused = 246;

	public const int BeetleMinecart = 247;

	public const int BeetleMinecartLegacyUnused = 248;

	public const int MeowmereMinecart = 249;

	public const int MeowmereMinecartLegacyUnused = 250;

	public const int PartyMinecart = 251;

	public const int PartyMinecartLegacyUnused = 252;

	public const int PirateMinecart = 253;

	public const int PirateMinecartLegacyUnused = 254;

	public const int SteampunkMinecart = 255;

	public const int SteampunkMinecartLegacyUnused = 256;

	public const int Lucky = 257;

	public const int LilHarpy = 258;

	public const int FennecFox = 259;

	public const int GlitteryButterfly = 260;

	public const int BabyImp = 261;

	public const int BabyRedPanda = 262;

	public const int StormTiger = 263;

	public const int Plantero = 264;

	public const int Flamingo = 265;

	public const int DynamiteKitten = 266;

	public const int BabyWerewolf = 267;

	public const int ShadowMimic = 268;

	public const int CoffinMinecart = 269;

	public const int CoffinMinecartLegacyUnused = 270;

	public const int Smolstar = 271;

	public const int DiggingMoleMinecart = 272;

	public const int DiggingMoleMinecartLegacyUnused = 273;

	public const int VoltBunny = 274;

	public const int PaintedHorseMount = 275;

	public const int MajesticHorseMount = 276;

	public const int DarkHorseMount = 277;

	public const int PogoStickMount = 278;

	public const int PirateShipMount = 279;

	public const int SpookyWoodMount = 280;

	public const int SantankMount = 281;

	public const int WallOfFleshGoatMount = 282;

	public const int DarkMageBookMount = 283;

	public const int KingSlimePet = 284;

	public const int EyeOfCthulhuPet = 285;

	public const int EaterOfWorldsPet = 286;

	public const int BrainOfCthulhuPet = 287;

	public const int SkeletronPet = 288;

	public const int QueenBeePet = 289;

	public const int DestroyerPet = 290;

	public const int TwinsPet = 291;

	public const int SkeletronPrimePet = 292;

	public const int PlanteraPet = 293;

	public const int GolemPet = 294;

	public const int DukeFishronPet = 295;

	public const int LunaticCultistPet = 296;

	public const int MoonLordPet = 297;

	public const int FairyQueenPet = 298;

	public const int PumpkingPet = 299;

	public const int EverscreamPet = 300;

	public const int IceQueenPet = 301;

	public const int MartianPet = 302;

	public const int DD2OgrePet = 303;

	public const int DD2BetsyPet = 304;

	public const int LavaSharkMount = 305;

	public const int TitaniumStorm = 306;

	[Obsolete("Removed", true)]
	public const int BlandWhipEnemyDebuff = 307;

	public const int SwordWhipPlayerBuff = 308;

	[Obsolete("Removed", true)]
	public const int SwordWhipNPCDebuff = 309;

	public const int ScytheWhipEnemyDebuff = 310;

	public const int ScytheWhipPlayerBuff = 311;

	public const int CoolWhipPlayerBuff = 312;

	[Obsolete("Removed", true)]
	public const int FlameWhipEnemyDebuff = 313;

	public const int ThornWhipPlayerBuff = 314;

	[Obsolete("Removed", true)]
	public const int ThornWhipNPCDebuff = 315;

	[Obsolete("Removed", true)]
	public const int RainbowWhipNPCDebuff = 316;

	public const int QueenSlimePet = 317;

	public const int QueenSlimeMount = 318;

	[Obsolete("Removed", true)]
	public const int MaceWhipNPCDebuff = 319;

	public const int GelBalloonBuff = 320;

	public const int BrainOfConfusionBuff = 321;

	public const int EmpressBlade = 322;

	public const int OnFire3 = 323;

	public const int Frostburn2 = 324;

	public const int FlinxMinion = 325;

	[Obsolete("Removed", true)]
	public const int BoneWhipNPCDebuff = 326;

	public const int BerniePet = 327;

	public const int GlommerPet = 328;

	public const int DeerclopsPet = 329;

	public const int PigPet = 330;

	public const int ChesterPet = 331;

	public const int NeutralHunger = 332;

	public const int Hunger = 333;

	public const int Starving = 334;

	public const int AbigailMinion = 335;

	public const int HeartyMeal = 336;

	public const int TentacleSpike = 337;

	public const int FartMinecart = 338;

	public const int FartMinecartLegacyUnused = 339;

	[Obsolete("Removed", true)]
	public const int CoolWhipNPCDebuff = 340;

	public const int DualSlimePet = 341;

	public const int WolfMount = 342;

	public const int BiomeSight = 343;

	public const int BloodButcherer = 344;

	public const int JunimoPet = 345;

	public const int TerraFartMinecart = 346;

	public const int TerraFartMinecartLegacyUnused = 347;

	public const int WarTable = 348;

	public const int BlueChickenPet = 349;

	public const int ShadowCandle = 350;

	public const int Spiffo = 351;

	public const int CavelingGardener = 352;

	public const int Shimmer = 353;

	public const int DirtiestBlock = 354;

	public const int DeadCellsMushroomBoiMinion = 355;

	public const int DeadCellsSwarmBiter = 356;

	[Obsolete("Removed", true)]
	public const int CobWhipNPCDebuff = 357;

	[Obsolete("Removed", true)]
	public const int CorruptWhipNPCDebuff = 358;

	[Obsolete("Removed", true)]
	public const int CrimsonWhipNPCDebuff = 359;

	[Obsolete("Removed", true)]
	public const int MeteorWhipNPCDebuff = 360;

	[Obsolete("Removed", true)]
	public const int FlowerWhipNPCDebuff = 361;

	public const int EelWhipNPCDebuff = 362;

	[Obsolete("Removed", true)]
	public const int ConstellationWhipNPCDebuff = 363;

	[Obsolete("Removed", true)]
	public const int MoonLordWhipNPCDebuff = 364;

	public const int CobWhipPlayerBuff = 365;

	public const int DeadCellsPotionStation = 366;

	[Obsolete("Removed", true)]
	public const int FlowerWhipNPCDebuffProc = 367;

	[Obsolete("Removed", true)]
	public const int MoonLordWhipNPCDebuffProc = 368;

	[Obsolete("Removed", true)]
	public const int MeteorWhipNPCDebuffProc = 369;

	public const int VelociraptorMount = 370;

	public const int Pufferfish = 371;

	public const int AxeFairyPet = 372;

	public const int BoulderPet = 373;

	public const int RatMount = 374;

	public const int Hemorrhage = 375;

	public const int TorchGodPotion = 376;

	public const int BatMount = 377;

	public const int RollerSkatesMount = 378;

	public const int RollerSkatesGreenMount = 379;

	public const int RollerSkatesWhiteMount = 380;

	public const int RollerSkatesPinkMount = 381;

	public const int RainbowBoulderPet = 382;

	public const int Kite = 383;

	public const int PixieMount = 384;

	public const int PalworldMinionCattiva = 385;

	public const int PalworldMinionFoxsparks = 386;

	public const int PalworldPetChillet = 387;

	public const int PalworldPetChilletIgnis = 388;

	public const int PalworldMinionTrustyCattiva = 389;

	public const int PalworldMinionTrustyFoxsparks = 390;

	public const int PalworldMountTrustyChillet = 391;

	public const int PalworldMountTrustyChilletIgnis = 392;

	public const int ClayPotMinion = 393;

	public const int ForbiddenMinion = 394;

	public const int PotentAcid = 395;

	public const int ManaHeat = 396;

	public const int ChlorophyteSpore = 397;

	public const int AcceleratePoisons = 398;

	public const int BlueLightning = 399;

	public const int RedLightning = 400;

	public static readonly int Count = 401;

	public static readonly IdDictionary Search = IdDictionary.Create<BuffID, int>();
}
