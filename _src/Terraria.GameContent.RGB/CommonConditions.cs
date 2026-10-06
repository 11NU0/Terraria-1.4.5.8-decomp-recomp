using System;
using Microsoft.Xna.Framework;
using ReLogic.Peripherals.RGB;

namespace Terraria.GameContent.RGB;

public static class CommonConditions
{
	private class SimpleCondition : ChromaCondition
	{
		private Func<bool> _condition;

		public SimpleCondition(Func<bool> condition)
		{
			_condition = condition;
		}

		public override bool IsActive()
		{
			return _condition();
		}
	}

	private class SceneCondition : SimpleCondition
	{
		public SceneCondition(Func<SceneMetrics, bool> condition)
			: base(() => condition(Main.SceneMetrics))
		{
		}
	}

	private class PlayerCondition : SimpleCondition
	{
		public PlayerCondition(Func<Player, bool> condition)
			: base(() => condition(Main.LocalPlayer))
		{
		}
	}

	public static class SurfaceBiome
	{
		private class SurfaceCondition : SceneCondition
		{
			public SurfaceCondition(Func<SceneMetrics, bool> condition)
				: base((SceneMetrics scene) => scene.ZoneOverworldHeight && condition(scene))
			{
			}
		}

		public static readonly ChromaCondition Ocean = (ChromaCondition)(object)new SurfaceCondition((SceneMetrics scene) => scene.ZoneBeach);

		public static readonly ChromaCondition Desert = (ChromaCondition)(object)new SurfaceCondition((SceneMetrics scene) => scene.ZoneDesert);

		public static readonly ChromaCondition Jungle = (ChromaCondition)(object)new SurfaceCondition((SceneMetrics scene) => scene.ZoneJungle);

		public static readonly ChromaCondition Snow = (ChromaCondition)(object)new SurfaceCondition((SceneMetrics scene) => scene.ZoneSnow);

		public static readonly ChromaCondition Mushroom = (ChromaCondition)(object)new SurfaceCondition((SceneMetrics scene) => scene.ZoneGlowshroom);

		public static readonly ChromaCondition Corruption = (ChromaCondition)(object)new SurfaceCondition((SceneMetrics scene) => scene.ZoneCorrupt);

		public static readonly ChromaCondition Hallow = (ChromaCondition)(object)new SurfaceCondition((SceneMetrics scene) => scene.ZoneHallow);

		public static readonly ChromaCondition Crimson = (ChromaCondition)(object)new SurfaceCondition((SceneMetrics scene) => scene.ZoneCrimson);
	}

	public static class MiscBiome
	{
		public static readonly ChromaCondition Meteorite = (ChromaCondition)(object)new SceneCondition((SceneMetrics scene) => scene.ZoneMeteor);
	}

	public static class UndergroundBiome
	{
		private class UndergroundCondition : SceneCondition
		{
			public UndergroundCondition(Func<SceneMetrics, bool> condition)
				: base((SceneMetrics scene) => !scene.ZoneOverworldHeight && condition(scene))
			{
			}
		}

		public static readonly ChromaCondition Hive = (ChromaCondition)(object)new SceneCondition((SceneMetrics scene) => scene.ZoneHive);

		public static readonly ChromaCondition Jungle = (ChromaCondition)(object)new UndergroundCondition((SceneMetrics scene) => scene.ZoneJungle);

		public static readonly ChromaCondition Mushroom = (ChromaCondition)(object)new UndergroundCondition((SceneMetrics scene) => scene.ZoneGlowshroom);

		public static readonly ChromaCondition Ice = (ChromaCondition)(object)new UndergroundCondition((SceneMetrics scene) => scene.ZoneSnow);

		public static readonly ChromaCondition HallowIce = (ChromaCondition)(object)new UndergroundCondition((SceneMetrics scene) => scene.ZoneSnow && scene.ZoneHallow);

		public static readonly ChromaCondition CrimsonIce = (ChromaCondition)(object)new UndergroundCondition((SceneMetrics scene) => scene.ZoneSnow && scene.ZoneCrimson);

		public static readonly ChromaCondition CorruptIce = (ChromaCondition)(object)new UndergroundCondition((SceneMetrics scene) => scene.ZoneSnow && scene.ZoneCorrupt);

		public static readonly ChromaCondition Hallow = (ChromaCondition)(object)new UndergroundCondition((SceneMetrics scene) => scene.ZoneHallow);

		public static readonly ChromaCondition Crimson = (ChromaCondition)(object)new UndergroundCondition((SceneMetrics scene) => scene.ZoneCrimson);

		public static readonly ChromaCondition Corrupt = (ChromaCondition)(object)new UndergroundCondition((SceneMetrics scene) => scene.ZoneCorrupt);

		public static readonly ChromaCondition Desert = (ChromaCondition)(object)new UndergroundCondition((SceneMetrics scene) => scene.ZoneDesert);

		public static readonly ChromaCondition HallowDesert = (ChromaCondition)(object)new UndergroundCondition((SceneMetrics scene) => scene.ZoneDesert && scene.ZoneHallow);

		public static readonly ChromaCondition CrimsonDesert = (ChromaCondition)(object)new UndergroundCondition((SceneMetrics scene) => scene.ZoneDesert && scene.ZoneCrimson);

		public static readonly ChromaCondition CorruptDesert = (ChromaCondition)(object)new UndergroundCondition((SceneMetrics scene) => scene.ZoneDesert && scene.ZoneCorrupt);

		public static readonly ChromaCondition Temple = (ChromaCondition)(object)new SceneCondition((SceneMetrics scene) => scene.ZoneLihzhardTemple);

		public static readonly ChromaCondition Dungeon = (ChromaCondition)(object)new SceneCondition((SceneMetrics scene) => scene.ZoneDungeon);

		public static readonly ChromaCondition Marble = (ChromaCondition)(object)new SceneCondition((SceneMetrics scene) => scene.ZoneMarble);

		public static readonly ChromaCondition Granite = (ChromaCondition)(object)new SceneCondition((SceneMetrics scene) => scene.ZoneGranite);

		public static readonly ChromaCondition GemCave = (ChromaCondition)(object)new SceneCondition((SceneMetrics scene) => scene.ZoneGemCave);

		public static readonly ChromaCondition Shimmer = (ChromaCondition)(object)new SceneCondition((SceneMetrics scene) => scene.ZoneShimmer);
	}

	public static class Boss
	{
		public static int HighestTierBossOrEvent;

		public static readonly ChromaCondition EaterOfWorlds = (ChromaCondition)(object)new SceneCondition((SceneMetrics _) => HighestTierBossOrEvent == 13);

		public static readonly ChromaCondition Destroyer = (ChromaCondition)(object)new SceneCondition((SceneMetrics _) => HighestTierBossOrEvent == 134);

		public static readonly ChromaCondition KingSlime = (ChromaCondition)(object)new SceneCondition((SceneMetrics _) => HighestTierBossOrEvent == 50);

		public static readonly ChromaCondition QueenSlime = (ChromaCondition)(object)new SceneCondition((SceneMetrics _) => HighestTierBossOrEvent == 657);

		public static readonly ChromaCondition BrainOfCthulhu = (ChromaCondition)(object)new SceneCondition((SceneMetrics _) => HighestTierBossOrEvent == 266);

		public static readonly ChromaCondition DukeFishron = (ChromaCondition)(object)new SceneCondition((SceneMetrics _) => HighestTierBossOrEvent == 370);

		public static readonly ChromaCondition QueenBee = (ChromaCondition)(object)new SceneCondition((SceneMetrics _) => HighestTierBossOrEvent == 222);

		public static readonly ChromaCondition Plantera = (ChromaCondition)(object)new SceneCondition((SceneMetrics _) => HighestTierBossOrEvent == 262);

		public static readonly ChromaCondition Empress = (ChromaCondition)(object)new SceneCondition((SceneMetrics _) => HighestTierBossOrEvent == 636);

		public static readonly ChromaCondition EyeOfCthulhu = (ChromaCondition)(object)new SceneCondition((SceneMetrics _) => HighestTierBossOrEvent == 4);

		public static readonly ChromaCondition TheTwins = (ChromaCondition)(object)new SceneCondition((SceneMetrics _) => HighestTierBossOrEvent == 126);

		public static readonly ChromaCondition MoonLord = (ChromaCondition)(object)new SceneCondition((SceneMetrics _) => HighestTierBossOrEvent == 398);

		public static readonly ChromaCondition WallOfFlesh = (ChromaCondition)(object)new SceneCondition((SceneMetrics _) => HighestTierBossOrEvent == 113);

		public static readonly ChromaCondition Golem = (ChromaCondition)(object)new SceneCondition((SceneMetrics _) => HighestTierBossOrEvent == 245);

		public static readonly ChromaCondition Cultist = (ChromaCondition)(object)new SceneCondition((SceneMetrics _) => HighestTierBossOrEvent == 439);

		public static readonly ChromaCondition Skeletron = (ChromaCondition)(object)new SceneCondition((SceneMetrics _) => HighestTierBossOrEvent == 35);

		public static readonly ChromaCondition SkeletronPrime = (ChromaCondition)(object)new SceneCondition((SceneMetrics _) => HighestTierBossOrEvent == 127);

		public static readonly ChromaCondition Deerclops = (ChromaCondition)(object)new SceneCondition((SceneMetrics _) => HighestTierBossOrEvent == 668);
	}

	public static class Weather
	{
		public static readonly ChromaCondition Rain = (ChromaCondition)(object)new SceneCondition((SceneMetrics scene) => scene.ZoneRain && !scene.ZoneSnow);

		public static readonly ChromaCondition Sandstorm = (ChromaCondition)(object)new SceneCondition((SceneMetrics scene) => scene.ZoneSandstorm);

		public static readonly ChromaCondition Blizzard = (ChromaCondition)(object)new SceneCondition((SceneMetrics scene) => scene.ZoneSnow && scene.ZoneRain);

		public static readonly ChromaCondition SlimeRain = (ChromaCondition)(object)new SceneCondition((SceneMetrics scene) => Main.slimeRain && scene.ZoneOverworldHeight);
	}

	public static class Depth
	{
		public static readonly ChromaCondition Sky = (ChromaCondition)(object)new SceneCondition((SceneMetrics scene) => scene.ZoneSkyHeight);

		public static readonly ChromaCondition Surface = (ChromaCondition)(object)new SceneCondition((SceneMetrics scene) =>
		{
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			return scene.ZoneOverworldHeight && !IsInFrontOfDirtWall(scene.TileCenter);
		});

		public static readonly ChromaCondition Vines = (ChromaCondition)(object)new SceneCondition((SceneMetrics scene) =>
		{
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			return scene.ZoneOverworldHeight && IsInFrontOfDirtWall(scene.TileCenter);
		});

		public static readonly ChromaCondition Underground = (ChromaCondition)(object)new SceneCondition((SceneMetrics scene) => scene.ZoneDirtLayerHeight);

		public static readonly ChromaCondition Caverns = (ChromaCondition)(object)new SceneCondition((SceneMetrics scene) =>
		{
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			return scene.ZoneRockLayerHeight && scene.TileCenter.Y <= Main.maxTilesY - 400;
		});

		public static readonly ChromaCondition Magma = (ChromaCondition)(object)new SceneCondition((SceneMetrics scene) =>
		{
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			return scene.ZoneRockLayerHeight && scene.TileCenter.Y > Main.maxTilesY - 400;
		});

		public static readonly ChromaCondition Underworld = (ChromaCondition)(object)new SceneCondition((SceneMetrics scene) => scene.ZoneUnderworldHeight);

		private static bool IsInFrontOfDirtWall(Point tilePosition)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			if (!WorldGen.InWorld(tilePosition.X, tilePosition.Y))
			{
				return false;
			}
			if (Main.tile[tilePosition.X, tilePosition.Y] == null)
			{
				return false;
			}
			switch (Main.tile[tilePosition.X, tilePosition.Y].wall)
			{
			case 2:
			case 16:
			case 54:
			case 55:
			case 56:
			case 57:
			case 58:
			case 59:
			case 61:
			case 170:
			case 171:
			case 185:
			case 196:
			case 197:
			case 198:
			case 199:
			case 212:
			case 213:
			case 214:
			case 215:
				return true;
			default:
				return false;
			}
		}
	}

	public static class Events
	{
		public static readonly ChromaCondition BloodMoon = (ChromaCondition)(object)new SceneCondition((SceneMetrics _) => Main.bloodMoon && !Main.snowMoon && !Main.pumpkinMoon);

		public static readonly ChromaCondition FrostMoon = (ChromaCondition)(object)new SceneCondition((SceneMetrics _) => Main.snowMoon);

		public static readonly ChromaCondition PumpkinMoon = (ChromaCondition)(object)new SceneCondition((SceneMetrics _) => Main.pumpkinMoon);

		public static readonly ChromaCondition SolarEclipse = (ChromaCondition)(object)new SceneCondition((SceneMetrics _) => Main.eclipse);

		public static readonly ChromaCondition SolarPillar = (ChromaCondition)(object)new SceneCondition((SceneMetrics scene) => scene.CloseEnoughToSolarTower);

		public static readonly ChromaCondition NebulaPillar = (ChromaCondition)(object)new SceneCondition((SceneMetrics scene) => scene.CloseEnoughToNebulaTower);

		public static readonly ChromaCondition VortexPillar = (ChromaCondition)(object)new SceneCondition((SceneMetrics scene) => scene.CloseEnoughToVortexTower);

		public static readonly ChromaCondition StardustPillar = (ChromaCondition)(object)new SceneCondition((SceneMetrics scene) => scene.CloseEnoughToStardustTower);

		public static readonly ChromaCondition PirateInvasion = (ChromaCondition)(object)new SceneCondition((SceneMetrics _) => Boss.HighestTierBossOrEvent == -3);

		public static readonly ChromaCondition DD2Event = (ChromaCondition)(object)new SceneCondition((SceneMetrics _) => Boss.HighestTierBossOrEvent == -6);

		public static readonly ChromaCondition FrostLegion = (ChromaCondition)(object)new SceneCondition((SceneMetrics _) => Boss.HighestTierBossOrEvent == -2);

		public static readonly ChromaCondition MartianMadness = (ChromaCondition)(object)new SceneCondition((SceneMetrics _) => Boss.HighestTierBossOrEvent == -4);

		public static readonly ChromaCondition GoblinArmy = (ChromaCondition)(object)new SceneCondition((SceneMetrics _) => Boss.HighestTierBossOrEvent == -1);
	}

	public static class Alert
	{
		public static readonly ChromaCondition MoonlordComing = (ChromaCondition)(object)new SceneCondition((SceneMetrics _) => NPC.MoonLordCountdown > 0);

		public static readonly ChromaCondition Keybinds = (ChromaCondition)(object)new SimpleCondition(() => Main.InGameUI.CurrentState == Main.ManageControlsMenu || Main.MenuUI.CurrentState == Main.ManageControlsMenu);

		public static readonly ChromaCondition Drowning = (ChromaCondition)(object)new PlayerCondition((Player player) => player.breath != player.breathMax);

		public static readonly ChromaCondition LavaIndicator = (ChromaCondition)(object)new PlayerCondition((Player player) => player.lavaWet);
	}

	public static class CriticalAlert
	{
		public static readonly ChromaCondition LowLife = (ChromaCondition)(object)new PlayerCondition((Player player) => Main.ChromaPainter.PotionAlert);

		public static readonly ChromaCondition Death = (ChromaCondition)(object)new PlayerCondition((Player player) => player.dead);
	}

	public static readonly ChromaCondition InMenu = (ChromaCondition)(object)new SimpleCondition(() => Main.gameMenu && !Main.drunkWorld);

	public static readonly ChromaCondition DrunkMenu = (ChromaCondition)(object)new SimpleCondition(() => Main.gameMenu && Main.drunkWorld);
}
