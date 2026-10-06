using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using ReLogic.Utilities;
using Terraria.GameContent.Biomes;
using Terraria.GameContent.Generation.Dungeon.Entrances;
using Terraria.GameContent.Generation.Dungeon.Features;
using Terraria.GameContent.Generation.Dungeon.Halls;
using Terraria.GameContent.Generation.Dungeon.LayoutProviders;
using Terraria.GameContent.Generation.Dungeon.Rooms;
using Terraria.ID;
using Terraria.Localization;
using Terraria.Utilities;
using Terraria.WorldBuilding;

namespace Terraria.GameContent.Generation.Dungeon;

public static class DungeonCrawler
{
	public static List<DungeonData> dungeonData = new List<DungeonData>();

	public static DungeonData CurrentDungeonData
	{
		get
		{
			return dungeonData[GenVars.CurrentDungeon];
		}
		set
		{
			dungeonData[GenVars.CurrentDungeon] = value;
		}
	}

	public static void SetupDungeonData(int currentDungeon, bool clearOld = false)
	{
		if (clearOld)
		{
			dungeonData.Clear();
		}
		GenVars.CurrentDungeon = currentDungeon;
		DungeonType type = DungeonType.Default;
		if (WorldGen.SecretSeed.dualDungeons.Enabled)
		{
			type = DungeonType.DualDungeon;
		}
		DungeonData item = new DungeonData
		{
			Type = type,
			Iteration = currentDungeon
		};
		dungeonData.Add(item);
	}

	public static void SetupDungeonGenVarVariables(DungeonGenVars genVars, UnifiedRandom genRand)
	{
		int num = genRand.Next(3);
		if (WorldGen.remixWorldGen)
		{
			num = (WorldGen.crimson ? 2 : 0);
		}
		switch (num)
		{
		case 0:
			genVars.dungeonColor = DungeonColor.Blue;
			genVars.brickTileType = 41;
			genVars.brickWallType = 7;
			genVars.brickCrackedTileType = 481;
			genVars.windowGlassWallType = 91;
			genVars.windowClosedGlassWallType = 96;
			genVars.windowEdgeWallType = 8;
			genVars.windowPlatformItemTypes = new int[1] { 1386 };
			break;
		case 1:
			genVars.dungeonColor = DungeonColor.Green;
			genVars.brickTileType = 43;
			genVars.brickWallType = 8;
			genVars.brickCrackedTileType = 482;
			genVars.windowGlassWallType = 92;
			genVars.windowClosedGlassWallType = 94;
			genVars.windowEdgeWallType = 9;
			genVars.windowPlatformItemTypes = new int[1] { 1385 };
			break;
		default:
			genVars.dungeonColor = DungeonColor.Pink;
			genVars.brickTileType = 44;
			genVars.brickWallType = 9;
			genVars.brickCrackedTileType = 483;
			genVars.windowGlassWallType = 90;
			genVars.windowClosedGlassWallType = 98;
			genVars.windowEdgeWallType = 7;
			genVars.windowPlatformItemTypes = new int[1] { 1384 };
			break;
		}
		if (WorldGen.drunkWorldGen)
		{
			switch (genRand.Next(3))
			{
			case 0:
				genVars.brickWallType = 7;
				break;
			case 1:
				genVars.brickWallType = 8;
				break;
			default:
				genVars.brickWallType = 9;
				break;
			}
		}
		DungeonUtils.CreatePotentialDungeonBounds(out genVars.innerPotentialDungeonBounds, out genVars.outerPotentialDungeonBounds, genVars.dungeonSide == DungeonSide.Left, 0.10000000149011612, 0.05000000074505806);
		genVars.dungeonStyle = DungeonGenerationStyles.GetCurrentDungeonStyle();
		if (WorldGen.SecretSeed.dualDungeons.Enabled)
		{
			int num2 = GenVars.CurrentDungeon % 2;
			if (num2 == 0 || num2 != 1)
			{
				genVars.dungeonGenerationStyles.Add(DungeonGenerationStyles.Cavern);
				genVars.dungeonGenerationStyles.Add(WorldGen.crimson ? DungeonGenerationStyles.Crimson : DungeonGenerationStyles.Corruption);
				genVars.dungeonGenerationStyles.Add(DungeonGenerationStyles.Jungle);
				genVars.dungeonGenerationStyles.Add(genVars.dungeonStyle);
			}
			else
			{
				genVars.dungeonGenerationStyles.Add(DungeonGenerationStyles.Snow);
				genVars.dungeonGenerationStyles.Add(DungeonGenerationStyles.Desert);
				genVars.dungeonGenerationStyles.Add(DungeonGenerationStyles.Hallow);
				genVars.dungeonGenerationStyles.Add(DungeonGenerationStyles.Temple);
			}
		}
		else
		{
			genVars.dungeonGenerationStyles.Add(genVars.dungeonStyle);
		}
		genVars.isDungeonTile = Main.tileDungeon;
		genVars.isCrackedBrick = TileID.Sets.CrackedBricks;
		genVars.isPitTrapTile = TileID.Sets.CrackedBricks;
		genVars.isDungeonWall = Main.wallDungeon;
		genVars.isDungeonWallEdge = Main.wallDungeon;
		genVars.isDungeonWallGlass = WallID.Sets.Glass;
		if (WorldGen.SecretSeed.dualDungeons.Enabled)
		{
			genVars.isDungeonTile = (bool[])genVars.isDungeonTile.Clone();
			genVars.isCrackedBrick = (bool[])genVars.isCrackedBrick.Clone();
			genVars.isPitTrapTile = (bool[])genVars.isPitTrapTile.Clone();
			genVars.isDungeonWall = (bool[])genVars.isDungeonWall.Clone();
			genVars.isDungeonWallEdge = (bool[])genVars.isDungeonWallEdge.Clone();
			genVars.isDungeonWallGlass = (bool[])genVars.isDungeonWallGlass.Clone();
			List<DungeonGenerationStyleData> list = new List<DungeonGenerationStyleData>(genVars.dungeonGenerationStyles);
			foreach (DungeonGenerationStyleData dungeonGenerationStyle in genVars.dungeonGenerationStyles)
			{
				if (dungeonGenerationStyle.SubStyles != null)
				{
					list.AddRange(dungeonGenerationStyle.SubStyles);
				}
			}
			foreach (DungeonGenerationStyleData item in list)
			{
				genVars.isDungeonTile[item.BrickTileType] = true;
				if (item.BrickGrassTileType.HasValue)
				{
					genVars.isDungeonTile[item.BrickGrassTileType.Value] = true;
				}
				genVars.isCrackedBrick[item.BrickCrackedTileType] = true;
				genVars.isPitTrapTile[item.PitTrapTileType] = true;
				genVars.isDungeonWall[item.BrickWallType] = true;
				genVars.isDungeonWallEdge[item.WindowEdgeWallType] = true;
				genVars.isDungeonWallGlass[item.WindowGlassWallType] = true;
				genVars.isDungeonWallGlass[item.WindowClosedGlassWallType] = true;
				if (item.ExtraTileTypes != null)
				{
					for (int i = 0; i < item.ExtraTileTypes.Length; i++)
					{
						genVars.isDungeonTile[item.ExtraTileTypes[i]] = true;
					}
				}
				if (item.ExtraWallTypes != null)
				{
					for (int j = 0; j < item.ExtraWallTypes.Length; j++)
					{
						genVars.isDungeonWall[item.ExtraWallTypes[j]] = true;
					}
				}
			}
		}
		DungeonEntranceType dungeonEntranceType = DungeonEntranceType.Legacy;
		bool flag = false;
		int num3 = 50;
		while (!flag)
		{
			num3--;
			if (num3 <= 0)
			{
				dungeonEntranceType = DungeonEntranceType.Legacy;
				break;
			}
			dungeonEntranceType = DungeonEntranceType.Legacy;
			if (genRand.Next(3) == 0)
			{
				dungeonEntranceType = DungeonEntranceType.Dome;
			}
			if (genRand.Next(3) == 0)
			{
				dungeonEntranceType = DungeonEntranceType.Tower;
			}
			flag = true;
			if (WorldGen.SecretSeed.surfaceIsInSpace.Enabled && dungeonEntranceType == DungeonEntranceType.Tower)
			{
				flag = false;
			}
		}
		genVars.preGenDungeonEntranceSettings = (PreGenDungeonEntranceSettings)MakeDungeon_GetEntranceSettings(dungeonEntranceType, genVars.dungeonStyle, null);
	}

	public static void SetupDungeonDataVariables(int iteration, UnifiedRandom genRand)
	{
		//IL_0495: Unknown result type (might be due to invalid IL or missing references)
		//IL_049a: Unknown result type (might be due to invalid IL or missing references)
		//IL_045d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0462: Unknown result type (might be due to invalid IL or missing references)
		DungeonData dungeonData = DungeonCrawler.dungeonData[iteration];
		dungeonData.wallVariants = new int[3];
		switch (dungeonData.genVars.brickWallType)
		{
		default:
			dungeonData.wallVariants[0] = 7;
			dungeonData.wallVariants[1] = 94;
			dungeonData.wallVariants[2] = 95;
			break;
		case 8:
			dungeonData.wallVariants[0] = 8;
			dungeonData.wallVariants[1] = 98;
			dungeonData.wallVariants[2] = 99;
			break;
		case 9:
			dungeonData.wallVariants[0] = 9;
			dungeonData.wallVariants[1] = 96;
			dungeonData.wallVariants[2] = 97;
			break;
		}
		dungeonData.platformItemType = 1384;
		dungeonData.chandelierItemType = 2652;
		dungeonData.doorItemType = 1411;
		switch (dungeonData.genVars.dungeonColor)
		{
		default:
			dungeonData.platformItemType = 1384;
			dungeonData.chandelierItemType = 2652;
			dungeonData.doorItemType = 1411;
			break;
		case DungeonColor.Green:
			dungeonData.platformItemType = 1386;
			dungeonData.chandelierItemType = 2653;
			dungeonData.doorItemType = 1412;
			break;
		case DungeonColor.Pink:
			dungeonData.platformItemType = 1385;
			dungeonData.chandelierItemType = 2654;
			dungeonData.doorItemType = 1413;
			break;
		}
		dungeonData.shelfStyles = new int[3];
		dungeonData.shelfStyles[0] = genRand.Next(9, 13);
		dungeonData.shelfStyles[1] = genRand.Next(9, 13);
		while (dungeonData.shelfStyles[1] == dungeonData.shelfStyles[0])
		{
			dungeonData.shelfStyles[1] = genRand.Next(9, 13);
		}
		dungeonData.shelfStyles[2] = genRand.Next(9, 13);
		while (dungeonData.shelfStyles[2] == dungeonData.shelfStyles[0] || dungeonData.shelfStyles[2] == dungeonData.shelfStyles[1])
		{
			dungeonData.shelfStyles[2] = genRand.Next(9, 13);
		}
		dungeonData.lanternStyles = new int[3];
		dungeonData.lanternStyles[0] = genRand.Next(7);
		dungeonData.lanternStyles[1] = genRand.Next(7);
		while (dungeonData.lanternStyles[1] == dungeonData.lanternStyles[0])
		{
			dungeonData.lanternStyles[1] = genRand.Next(7);
		}
		dungeonData.lanternStyles[2] = genRand.Next(7);
		while (dungeonData.lanternStyles[2] == dungeonData.lanternStyles[0] || dungeonData.lanternStyles[2] == dungeonData.lanternStyles[1])
		{
			dungeonData.lanternStyles[2] = genRand.Next(7);
		}
		dungeonData.bannerStyles = new int[6];
		dungeonData.bannerStyles[0] = 10;
		dungeonData.bannerStyles[1] = 11;
		dungeonData.bannerStyles[2] = 12;
		dungeonData.bannerStyles[3] = 13;
		dungeonData.bannerStyles[4] = 14;
		dungeonData.bannerStyles[5] = 15;
		dungeonData.useSkewedDungeonEntranceHalls = genRand.Next(4) == 0;
		if (!dungeonData.genVars.preGenDungeonEntranceSettings.PrecalculateEntrancePosition)
		{
			return;
		}
		int num = dungeonData.genVars.dungeonLocation;
		int num2 = 0;
		bool flag = false;
		int num3 = 100;
		int num4 = 3000;
		while (!flag)
		{
			num4--;
			if (num4 <= 0)
			{
				break;
			}
			num = dungeonData.genVars.dungeonLocation - num3 + genRand.Next(num3 * 2);
			if (num > WorldGen.beachDistance && num < Main.maxTilesX - WorldGen.beachDistance)
			{
				num2 = 10;
				if (SpecialSeedFeatures.DungeonEntranceIsBuried)
				{
					num2 = (int)Main.worldSurface - 10 + GenVars.CurrentDungeonGenVars.preGenDungeonEntranceSettings.BuriedEntranceYOffset;
				}
				if (SpecialSeedFeatures.DungeonEntranceIsUnderground)
				{
					if (SpecialSeedFeatures.DungeonEntranceHasATree)
					{
						num2 = (int)GenVars.rockLayer - 20;
					}
					else
					{
						num2 = ((dungeonData.Type != DungeonType.DualDungeon) ? ((int)GenVars.rockLayer - 20) : ((int)GenVars.worldSurfaceHigh - 20));
					}
				}
				Tile tile = Main.tile[num, num2];
				while (tile != null && !tile.active() && tile.liquid <= 0 && tile.wall <= 0)
				{
					num2++;
					tile = Main.tile[num, num2];
				}
				if (!WorldGen.AreAnyTilesInSetNearby(num, num2, TileID.Sets.Clouds, 15) && !WorldGen.AreAnyTilesInSetNearby(num, Math.Max(50, num2 - 50), TileID.Sets.Clouds, 50) && num2 - 40 - dungeonData.genVars.preGenDungeonEntranceSettings.RoughHeight > 0)
				{
					flag = true;
				}
			}
		}
		if (flag)
		{
			dungeonData.genVars.dungeonLocation = num + 25 - genRand.Next(50);
			dungeonData.genVars.dungeonEntrancePosition = new Vector2D((double)num, (double)num2);
		}
		else
		{
			dungeonData.genVars.preGenDungeonEntranceSettings = (PreGenDungeonEntranceSettings)MakeDungeon_GetEntranceSettings(DungeonEntranceType.Legacy, dungeonData.genVars.preGenDungeonEntranceSettings.StyleData, null);
			dungeonData.genVars.dungeonEntrancePosition = Vector2D.Zero;
		}
	}

	public static void MakeDungeon(int x, int y, GenerationProgress progress = null)
	{
		//IL_01ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_01be: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_02af: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0311: Unknown result type (might be due to invalid IL or missing references)
		//IL_020e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0221: Unknown result type (might be due to invalid IL or missing references)
		//IL_0226: Unknown result type (might be due to invalid IL or missing references)
		//IL_0325: Unknown result type (might be due to invalid IL or missing references)
		//IL_0327: Unknown result type (might be due to invalid IL or missing references)
		//IL_0231: Unknown result type (might be due to invalid IL or missing references)
		//IL_023a: Unknown result type (might be due to invalid IL or missing references)
		//IL_045d: Unknown result type (might be due to invalid IL or missing references)
		//IL_03aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_03af: Unknown result type (might be due to invalid IL or missing references)
		//IL_0407: Unknown result type (might be due to invalid IL or missing references)
		//IL_040c: Unknown result type (might be due to invalid IL or missing references)
		UnifiedRandom genRand = WorldGen.genRand;
		DungeonData currentDungeonData = CurrentDungeonData;
		DungeonFeatureSettings settings = new DungeonFeatureSettings();
		currentDungeonData.genVars.GeneratingDungeon = true;
		DungeonUtils.UpdateDungeonProgress(progress, 0f, Language.GetTextValue("WorldGeneration.DungeonVariableSetup"));
		_ = currentDungeonData.genVars.brickTileType;
		_ = currentDungeonData.genVars.brickCrackedTileType;
		_ = currentDungeonData.genVars.brickWallType;
		WorldGen.SetCrackedBrickSolidity(solid: false);
		currentDungeonData.makeNextPitTrapFlooded = true;
		currentDungeonData.genVars.generatingDungeonPositionX = x;
		currentDungeonData.genVars.generatingDungeonPositionY = y;
		currentDungeonData.dungeonBounds.SetBounds(x, y, x, y);
		currentDungeonData.dungeonEntranceStrengthX = genRand.Next(25, 30);
		currentDungeonData.dungeonEntranceStrengthY = genRand.Next(20, 25);
		currentDungeonData.dungeonEntranceStrengthX2 = genRand.Next(35, 50);
		currentDungeonData.dungeonEntranceStrengthY2 = genRand.Next(10, 15);
		int num = Main.maxTilesX / 60;
		num += genRand.Next(0, num / 3);
		num = (int)((double)num * currentDungeonData.dungeonStepScalar);
		int maxSteps = num;
		int roomDelay = 5;
		currentDungeonData.globalFeatureScalar = 1.0;
		if (currentDungeonData.Type == DungeonType.DualDungeon)
		{
			DualDungeonLayoutProviderSettings dualDungeonLayoutProviderSettings = new DualDungeonLayoutProviderSettings();
			dualDungeonLayoutProviderSettings.StyleData = currentDungeonData.genVars.dungeonStyle;
			new DualDungeonLayoutProvider(dualDungeonLayoutProviderSettings).ProvideLayout(currentDungeonData, progress, genRand, ref roomDelay);
			currentDungeonData.globalFeatureScalar = Math.Max(1.0, (double)currentDungeonData.dungeonRooms.Count / 20.0);
		}
		else
		{
			LegacyDungeonLayoutProviderSettings legacyDungeonLayoutProviderSettings = new LegacyDungeonLayoutProviderSettings();
			legacyDungeonLayoutProviderSettings.StyleData = currentDungeonData.genVars.dungeonStyle;
			legacyDungeonLayoutProviderSettings.Steps = num;
			legacyDungeonLayoutProviderSettings.MaxSteps = maxSteps;
			new LegacyDungeonLayoutProvider(legacyDungeonLayoutProviderSettings).ProvideLayout(currentDungeonData, progress, genRand, ref roomDelay);
		}
		DungeonBounds innerBounds = currentDungeonData.dungeonRooms[0].InnerBounds;
		Vector2 val = new Vector2((float)innerBounds.Center.X, (float)innerBounds.Top);
		float x2 = val.X;
		float y2 = val.Y;
		if (currentDungeonData.Type == DungeonType.Default)
		{
			for (int i = 1; i < currentDungeonData.dungeonRooms.Count; i++)
			{
				if (currentDungeonData.dungeonRooms[i].generated)
				{
					innerBounds = currentDungeonData.dungeonRooms[i].InnerBounds;
					val = new Vector2((float)innerBounds.Center.X, (float)innerBounds.Top);
					if (val.Y < y2)
					{
						x2 = val.X;
						y2 = val.Y;
					}
				}
			}
		}
		currentDungeonData.genVars.generatingDungeonPositionX = (int)x2;
		currentDungeonData.genVars.generatingDungeonPositionY = (int)y2;
		currentDungeonData.genVars.generatingDungeonTopX = (int)x2;
		DungeonUtils.UpdateDungeonProgress(progress, 0.65f, Language.GetTextValue("WorldGeneration.DungeonEntranceHallway"));
		currentDungeonData.createdDungeonEntranceOnSurface = false;
		roomDelay = 5;
		Vector2D dungeonEntrancePosition = currentDungeonData.genVars.dungeonEntrancePosition;
		bool flag = dungeonEntrancePosition != Vector2D.Zero;
		if (flag && WorldGen.SecretSeed.surfaceIsDesert.Enabled && currentDungeonData.Type == DungeonType.DualDungeon)
		{
			currentDungeonData.createdDungeonEntranceOnSurface = true;
		}
		if (WorldGen.drunkWorldGen || WorldGen.SecretSeed.noSurface.Enabled)
		{
			currentDungeonData.createdDungeonEntranceOnSurface = true;
		}
		Vector2D currentPos = new Vector2D((double)currentDungeonData.genVars.generatingDungeonPositionX, (double)currentDungeonData.genVars.generatingDungeonPositionY);
		double num2 = (flag ? dungeonEntrancePosition.Distance(currentPos) : 0.0);
		int amountPassed = (int)num2;
		int num3 = 100;
		while (!currentDungeonData.createdDungeonEntranceOnSurface)
		{
			num3--;
			if (num3 <= 0)
			{
				break;
			}
			if (roomDelay > 0)
			{
				roomDelay--;
			}
			if (roomDelay == 0 && genRand.Next(5) == 0 && (double)currentDungeonData.genVars.generatingDungeonPositionY > Main.worldSurface + 100.0)
			{
				roomDelay = 10;
				int generatingDungeonPositionX = currentDungeonData.genVars.generatingDungeonPositionX;
				int generatingDungeonPositionY = currentDungeonData.genVars.generatingDungeonPositionY;
				MakeDungeon_GetHall_Legacy((LegacyDungeonHallSettings)MakeDungeon_GetHallSettings(DungeonHallType.Legacy, currentDungeonData, Vector2.Zero, Vector2.Zero, currentDungeonData.genVars.dungeonStyle)).GenerateHall(currentDungeonData, currentDungeonData.genVars.generatingDungeonPositionX, currentDungeonData.genVars.generatingDungeonPositionY);
				MakeDungeon_GetRoom(new LegacyDungeonRoomSettings
				{
					RoomPosition = new Point(currentDungeonData.genVars.generatingDungeonPositionX, currentDungeonData.genVars.generatingDungeonPositionY),
					RandomSeed = genRand.Next(),
					StyleData = currentDungeonData.genVars.dungeonStyle
				}).GenerateRoom(currentDungeonData);
				currentDungeonData.genVars.generatingDungeonPositionX = generatingDungeonPositionX;
				currentDungeonData.genVars.generatingDungeonPositionY = generatingDungeonPositionY;
			}
			if (flag)
			{
				MakeDungeon_GenerateNextEntranceHall_Precalculated(currentDungeonData, genRand, num2, dungeonEntrancePosition, ref amountPassed, ref currentPos);
			}
			else
			{
				MakeDungeon_GenerateNextEntranceHall_Legacy(currentDungeonData, currentDungeonData.genVars.generatingDungeonPositionX, currentDungeonData.genVars.generatingDungeonPositionY);
			}
		}
		MakeDungeon_GetEntrance(MakeDungeon_GetEntranceSettings(currentDungeonData.genVars.preGenDungeonEntranceSettings, currentDungeonData)).GenerateEntrance(currentDungeonData, currentDungeonData.genVars.generatingDungeonPositionX, currentDungeonData.genVars.generatingDungeonPositionY);
		if (WorldGen.SecretSeed.surfaceIsInSpace.Enabled)
		{
			currentDungeonData.dungeonBounds.Top = 25;
		}
		DungeonUtils.UpdateDungeonProgress(progress, 0.675f, Language.GetTextValue("WorldGeneration.DungeonFindingDoorsAndPlatforms"));
		for (int j = 0; j < currentDungeonData.dungeonRooms.Count; j++)
		{
			DungeonRoom dungeonRoom = currentDungeonData.dungeonRooms[j];
			if (dungeonRoom.Processed)
			{
				dungeonRoom.CalculatePlatformsAndDoors(currentDungeonData);
			}
		}
		for (int k = 0; k < currentDungeonData.dungeonHalls.Count; k++)
		{
			DungeonHall dungeonHall = currentDungeonData.dungeonHalls[k];
			if (dungeonHall.Processed)
			{
				dungeonHall.CalculatePlatformsAndDoors(currentDungeonData);
			}
		}
		DungeonUtils.UpdateDungeonProgress(progress, 0.7f, Language.GetTextValue("WorldGeneration.DungeonEarly"));
		new DungeonGlobalEarlyDualDungeonFeatures(settings).GenerateFeature(currentDungeonData);
		DungeonUtils.UpdateDungeonProgress(progress, 0.75f, Language.GetTextValue("WorldGeneration.DungeonSpikes"));
		new DungeonGlobalSpikes(settings).GenerateFeature(currentDungeonData);
		DungeonUtils.UpdateDungeonProgress(progress, 0.8f, Language.GetTextValue("WorldGeneration.DungeonDoors"));
		new DungeonGlobalDoors(settings).GenerateFeature(currentDungeonData);
		DungeonUtils.UpdateDungeonProgress(progress, 0.825f, Language.GetTextValue("WorldGeneration.DungeonWallVariants"));
		new DungeonGlobalWallVariants(settings).GenerateFeature(currentDungeonData);
		DungeonUtils.UpdateDungeonProgress(progress, 0.85f, Language.GetTextValue("WorldGeneration.DungeonPlatforms"));
		new DungeonGlobalPlatforms(settings).GenerateFeature(currentDungeonData);
		DungeonUtils.UpdateDungeonProgress(progress, 0.875f, Language.GetTextValue("WorldGeneration.DungeonBiomeChests"));
		new DungeonGlobalBiomeChests(settings).GenerateFeature(currentDungeonData);
		DungeonUtils.UpdateDungeonProgress(progress, 0.9f, Language.GetTextValue("WorldGeneration.DungeonBookshelves"));
		new DungeonGlobalBookshelves(settings).GenerateFeature(currentDungeonData);
		DungeonUtils.UpdateDungeonProgress(progress, 0.92f, Language.GetTextValue("WorldGeneration.DungeonChests"));
		new DungeonGlobalBasicChests(settings).GenerateFeature(currentDungeonData);
		DungeonUtils.UpdateDungeonProgress(progress, 0.935f, Language.GetTextValue("WorldGeneration.DungeonArea"));
		int amount = 25;
		currentDungeonData.dungeonBounds.Inflate(amount);
		DungeonUtils.UpdateDungeonProgress(progress, 0.94f, Language.GetTextValue("WorldGeneration.DungeonLights"));
		new DungeonGlobalLights(settings).GenerateFeature(currentDungeonData);
		DungeonUtils.UpdateDungeonProgress(progress, 0.95f, Language.GetTextValue("WorldGeneration.DungeonTraps"));
		new DungeonGlobalTraps(settings).GenerateFeature(currentDungeonData);
		DungeonUtils.UpdateDungeonProgress(progress, 0.96f, Language.GetTextValue("WorldGeneration.DungeonFurniture"));
		new DungeonGlobalGroundFurniture(settings).GenerateFeature(currentDungeonData);
		DungeonUtils.UpdateDungeonProgress(progress, 0.97f, Language.GetTextValue("WorldGeneration.DungeonPictures"));
		new DungeonGlobalPaintings(settings).GenerateFeature(currentDungeonData);
		DungeonUtils.UpdateDungeonProgress(progress, 0.98f, Language.GetTextValue("WorldGeneration.DungeonBanners"));
		new DungeonGlobalBanners(settings).GenerateFeature(currentDungeonData);
		DungeonUtils.UpdateDungeonProgress(progress, 0.99f, Language.GetTextValue("WorldGeneration.DungeonLate"));
		new DungeonGlobalLateDualDungeonFeatures(settings).GenerateFeature(currentDungeonData);
		DungeonUtils.UpdateDungeonProgress(progress, 1f, Language.GetTextValue("WorldGeneration.DungeonComplete"));
		currentDungeonData.genVars.GeneratingDungeon = false;
	}

	public static void MakeDungeon_GenerateNextEntranceHall_Legacy(DungeonData data, int x, int y)
	{
		((LegacyEntranceDungeonHall)MakeDungeon_GetHall(new LegacyEntranceDungeonHallSettings
		{
			HallType = DungeonHallType.LegacyEntrance,
			StyleData = data.genVars.dungeonStyle,
			RandomSeed = WorldGen.genRand.Next()
		})).GenerateHall(data, x, y);
	}

	public static void MakeDungeon_GenerateNextEntranceHall_Precalculated(DungeonData data, UnifiedRandom genRand, double dist, Vector2D entrancePos, ref int amountPassed, ref Vector2D currentPos)
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		int num = genRand.Next(10, 30);
		if ((double)num > dist - (double)amountPassed)
		{
			num = Math.Max(1, (int)dist - amountPassed);
		}
		Vector2D val = Vector2D.Lerp(currentPos, entrancePos, (double)amountPassed / dist);
		LegacyEntranceDungeonHallSettings legacyEntranceDungeonHallSettings = new LegacyEntranceDungeonHallSettings
		{
			HallType = DungeonHallType.LegacyEntrance,
			StyleData = data.genVars.dungeonStyle,
			RandomSeed = WorldGen.genRand.Next(),
			OverrideSteps = num,
			UsePrecalculatedEntrance = true
		};
		if (data.Type == DungeonType.DualDungeon)
		{
			legacyEntranceDungeonHallSettings.CrackedBrickChance = 0.0;
		}
		DungeonHall dungeonHall = MakeDungeon_GetHall(legacyEntranceDungeonHallSettings);
		dungeonHall.CalculateHall(data, currentPos, val);
		dungeonHall.GenerateHall(data);
		amountPassed -= num;
		currentPos = val;
		if (amountPassed <= 0)
		{
			data.createdDungeonEntranceOnSurface = true;
		}
	}

	public static DungeonRoomSettings MakeDungeon_GetRoomSettings(DungeonRoomType roomType, DungeonData data, DungeonControlLine line)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		UnifiedRandom genRand = WorldGen.genRand;
		DungeonRoomSettings dungeonRoomSettings = null;
		int progressionStage = line.ProgressionStage;
		DungeonGenerationStyleData style = line.Style;
		Vector2D normalizedLineDirection = line.NormalizedLineDirection;
		bool curveLine = line.CurveLine;
		int num = (int)(15.0 * data.roomStrengthScalar);
		int num2 = genRand.Next(13);
		int num3 = genRand.Next(13);
		int num4 = 6;
		int num5 = genRand.Next(12);
		int num6 = genRand.Next(12);
		float num7 = 1f;
		if (data.Type == DungeonType.DualDungeon)
		{
			num7 = 1.25f;
		}
		if ((roomType == DungeonRoomType.GenShapeDoughnut || roomType == DungeonRoomType.GenShapeQuadCircle) & curveLine)
		{
			roomType = DungeonRoomType.GenShapeCircle;
		}
		switch (roomType)
		{
		default:
			dungeonRoomSettings = new LegacyDungeonRoomSettings
			{
				OverrideStrength = num + num2,
				OverrideSteps = num4 + num5,
				OverrideVelocity = normalizedLineDirection.SafeNormalize(Vector2D.UnitY)
			};
			break;
		case DungeonRoomType.Regular:
			num = (int)((double)num * 0.8);
			num2 = (int)((double)num2 * 0.8);
			dungeonRoomSettings = new RegularDungeonRoomSettings
			{
				OverrideOuterBoundsSize = 8,
				OverrideInnerBoundsSize = num + num2
			};
			break;
		case DungeonRoomType.Wormlike:
		{
			int firstSideIterations = num4 * 3 + num5;
			int secondSideIterations = num4 * 3 + num6;
			dungeonRoomSettings = new WormlikeDungeonRoomSettings
			{
				FirstSideIterations = firstSideIterations,
				SecondSideIterations = secondSideIterations
			};
			break;
		}
		case DungeonRoomType.LivingTree:
		{
			num = (int)((double)num * 0.3);
			num2 = (int)((double)num2 * 0.5);
			int innerWidth = num + num2;
			int num23 = num4 * 6 + num5;
			int num24 = 4 + genRand.Next(3);
			int boundingRadius = (num23 + num24 + num24) / 2;
			dungeonRoomSettings = new LivingTreeDungeonRoomSettings
			{
				InnerWidth = innerWidth,
				InnerHeight = num23,
				Depth = num24,
				BoundingRadius = boundingRadius,
				ForceStyleForDoorsAndPlatforms = true
			};
			break;
		}
		case DungeonRoomType.BiomeSquare:
		case DungeonRoomType.BiomeRugged:
		case DungeonRoomType.BiomeStructured:
			dungeonRoomSettings = new BiomeDungeonRoomSettings();
			break;
		case DungeonRoomType.GenShapeCircle:
		{
			num = (int)((double)num * 0.8);
			num2 = (int)((double)num2 * 0.8);
			if (num7 != 1f && genRand.Next(3) == 0)
			{
				num = (int)((float)num * num7);
				num2 = (int)((float)num2 * num7);
			}
			int num15 = num;
			int num16 = num15 + 8;
			DungeonShapes.CircleRoom innerShape3 = new DungeonShapes.CircleRoom(num15 + num2);
			DungeonShapes.CircleRoom outerShape3 = new DungeonShapes.CircleRoom(num16 + num2);
			dungeonRoomSettings = new GenShapeDungeonRoomSettings
			{
				ShapeType = GenShapeType.Circle,
				InnerShape = innerShape3,
				OuterShape = outerShape3,
				BoundingRadius = num16 + num2,
				HallwayPointAdjuster = 10
			};
			break;
		}
		case DungeonRoomType.GenShapeMound:
		{
			if (num7 != 1f && genRand.Next(3) == 0)
			{
				num = (int)((float)num * num7);
				num2 = (int)((float)num2 * num7);
			}
			int num17 = num + num2;
			int num18 = num17 + 8;
			DungeonShapes.MoundRoom innerShape4 = new DungeonShapes.MoundRoom(num17, (int)((double)num17 * 1.5));
			DungeonShapes.MoundRoom outerShape4 = new DungeonShapes.MoundRoom(num18, (int)((double)num18 * 1.5));
			dungeonRoomSettings = new GenShapeDungeonRoomSettings
			{
				ShapeType = GenShapeType.Mound,
				InnerShape = innerShape4,
				OuterShape = outerShape4,
				BoundingRadius = (int)((double)num18 * 1.2)
			};
			break;
		}
		case DungeonRoomType.GenShapeHourglass:
		{
			if (num7 != 1f && genRand.Next(3) == 0)
			{
				num = (int)((float)num * num7);
				num2 = (int)((float)num2 * num7);
			}
			int num19 = num + num2 + 10;
			int num20 = num + num3 + 10;
			int num21 = num19 + 16;
			int num22 = num20 + 16;
			DungeonShapes.HourglassRoom innerShape5 = new DungeonShapes.HourglassRoom(num19, num20, 0f);
			DungeonShapes.HourglassRoom outerShape5 = new DungeonShapes.HourglassRoom(num21, num22, 0.4f);
			dungeonRoomSettings = new GenShapeDungeonRoomSettings
			{
				ShapeType = GenShapeType.Hourglass,
				InnerShape = innerShape5,
				OuterShape = outerShape5,
				BoundingRadius = ((num21 > num22) ? (num21 / 2) : (num22 / 2)) + 5,
				HallwayPointAdjuster = 5
			};
			break;
		}
		case DungeonRoomType.GenShapeDoughnut:
		{
			num = (int)((double)num * 0.8);
			num2 = (int)((double)num2 * 0.8);
			int num11 = num + num2;
			int num12 = num + num3;
			int num13 = num11 + 8;
			int num14 = num12 + 8;
			DungeonShapes.CircleRoom innerShape2 = new DungeonShapes.CircleRoom(num11, num12);
			DungeonShapes.CircleRoom outerShape2 = new DungeonShapes.CircleRoom(num13, num14);
			dungeonRoomSettings = new GenShapeDungeonRoomSettings
			{
				ShapeType = GenShapeType.Doughnut,
				InnerShape = innerShape2,
				OuterShape = outerShape2,
				BoundingRadius = ((num13 > num14) ? num13 : num14) + 5,
				HallwayPointAdjuster = 5
			};
			break;
		}
		case DungeonRoomType.GenShapeQuadCircle:
		{
			if (num7 != 1f && genRand.Next(3) == 0)
			{
				num = (int)((float)num * 1.5f);
				num2 = (int)((float)num2 * 1.5f);
			}
			int num8 = Math.Max(5, (int)((float)(num + num2) * 0.5f * 0.75f));
			int num9 = num8 + 8;
			int num10 = (int)((float)num8 * 1.5f);
			DungeonShapes.QuadCircleRoom innerShape = new DungeonShapes.QuadCircleRoom(num8, num10);
			DungeonShapes.QuadCircleRoom outerShape = new DungeonShapes.QuadCircleRoom(num9, num10);
			dungeonRoomSettings = new GenShapeDungeonRoomSettings
			{
				ShapeType = GenShapeType.QuadCircle,
				InnerShape = innerShape,
				OuterShape = outerShape,
				BoundingRadius = num9 / 2 + num10 + 4,
				HallwayPointAdjuster = 5
			};
			break;
		}
		}
		dungeonRoomSettings.RandomSeed = genRand.Next();
		dungeonRoomSettings.RoomType = roomType;
		dungeonRoomSettings.ProgressionStage = progressionStage;
		dungeonRoomSettings.StyleData = style;
		dungeonRoomSettings.OnCurvedLine = curveLine;
		dungeonRoomSettings.Orientation = SnakeOrientation.Unknown;
		dungeonRoomSettings.ControlLine = line;
		return dungeonRoomSettings;
	}

	public static DungeonHallSettings MakeDungeon_GetHallSettings(DungeonHallType hallType, DungeonData data, Vector2 hallStart, Vector2 hallEnd, DungeonGenerationStyleData style)
	{
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		UnifiedRandom genRand = WorldGen.genRand;
		DungeonHallSettings dungeonHallSettings = null;
		switch (hallType)
		{
		default:
			dungeonHallSettings = new LegacyDungeonHallSettings();
			break;
		case DungeonHallType.Regular:
			dungeonHallSettings = new RegularDungeonHallSettings();
			break;
		case DungeonHallType.Stairwell:
			dungeonHallSettings = new StairwellDungeonHallSettings
			{
				CrackedBrickChance = 0.0
			};
			break;
		case DungeonHallType.Sine:
		{
			Vector2 val = hallStart - hallEnd;
			int num = Math.Max(1, (int)(val.Length() / 30f));
			int iterations = ((num <= 1) ? 1 : (1 + genRand.Next(num - 1)));
			float magnitude = 8f + genRand.NextFloat() * 4f;
			dungeonHallSettings = new SineDungeonHallSettings
			{
				CrackedBrickChance = 0.0,
				Magnitude = magnitude,
				Iterations = iterations,
				FlipSine = (genRand.Next(2) == 0)
			};
			break;
		}
		}
		dungeonHallSettings.RandomSeed = genRand.Next();
		dungeonHallSettings.HallType = hallType;
		dungeonHallSettings.StyleData = style;
		return dungeonHallSettings;
	}

	public static DungeonEntranceSettings MakeDungeon_GetEntranceSettings(PreGenDungeonEntranceSettings preSettings, DungeonData data)
	{
		DungeonEntranceSettings dungeonEntranceSettings = MakeDungeon_GetEntranceSettings(preSettings.EntranceType, preSettings.StyleData, data);
		dungeonEntranceSettings.RandomSeed = preSettings.RandomSeed;
		return dungeonEntranceSettings;
	}

	public static DungeonEntranceSettings MakeDungeon_GetEntranceSettings(DungeonEntranceType entranceType, DungeonGenerationStyleData styleData, DungeonData data)
	{
		UnifiedRandom genRand = WorldGen.genRand;
		DungeonEntranceSettings dungeonEntranceSettings = null;
		if (data == null)
		{
			PreGenDungeonEntranceSettings preGenDungeonEntranceSettings = new PreGenDungeonEntranceSettings
			{
				EntranceType = entranceType,
				StyleData = styleData
			};
			int num = 0;
			if (WorldGen.SecretSeed.dualDungeons.Enabled)
			{
				num += 30;
			}
			switch (entranceType)
			{
			default:
				preGenDungeonEntranceSettings.BuriedEntranceYOffset = num;
				preGenDungeonEntranceSettings.BuriedEntranceSandDugoutYOffset = -num;
				preGenDungeonEntranceSettings.RoughHeight = 40;
				break;
			case DungeonEntranceType.Dome:
				preGenDungeonEntranceSettings.PrecalculateEntrancePosition = true;
				preGenDungeonEntranceSettings.BuriedEntranceYOffset = 20 + num;
				preGenDungeonEntranceSettings.BuriedEntranceSandDugoutYOffset = -num;
				preGenDungeonEntranceSettings.RoughHeight = 55;
				break;
			case DungeonEntranceType.Tower:
				preGenDungeonEntranceSettings.PrecalculateEntrancePosition = true;
				preGenDungeonEntranceSettings.BuriedEntranceYOffset = 20 + num;
				preGenDungeonEntranceSettings.BuriedEntranceSandDugoutYOffset = -num;
				preGenDungeonEntranceSettings.RoughHeight = 120;
				break;
			}
			preGenDungeonEntranceSettings.RandomSeed = genRand.Next();
			return preGenDungeonEntranceSettings;
		}
		bool flag = false;
		switch (entranceType)
		{
		default:
			dungeonEntranceSettings = new LegacyDungeonEntranceSettings();
			break;
		case DungeonEntranceType.Dome:
			dungeonEntranceSettings = new DomeDungeonEntranceSettings();
			dungeonEntranceSettings.PrecalculateEntrancePosition = true;
			break;
		case DungeonEntranceType.Tower:
			dungeonEntranceSettings = new TowerDungeonEntranceSettings();
			dungeonEntranceSettings.PrecalculateEntrancePosition = true;
			break;
		}
		dungeonEntranceSettings.RandomSeed = genRand.Next();
		dungeonEntranceSettings.EntranceType = entranceType;
		if (!flag)
		{
			dungeonEntranceSettings.StyleData = styleData;
		}
		return dungeonEntranceSettings;
	}

	public static DungeonRoom MakeDungeon_TryRoom(DungeonData data, int i, int j, DungeonRoomSettings roomSettings, bool addToData = true, int fluff = 0, bool noRoomOverlap = true)
	{
		DungeonRoom roomFound = null;
		if (data.IsAnyRoomInSpot(out roomFound, i, j, new DungeonRoomSearchSettings
		{
			Fluff = fluff
		}))
		{
			return null;
		}
		return MakeDungeon_GetRoom(roomSettings, addToData);
	}

	public static DungeonRoom MakeDungeon_GetRoom(DungeonRoomSettings settings, bool addToData = true)
	{
		DungeonRoom dungeonRoom = null;
		switch (settings.RoomType)
		{
		default:
			dungeonRoom = new LegacyDungeonRoom(settings);
			break;
		case DungeonRoomType.Regular:
			dungeonRoom = new RegularDungeonRoom(settings);
			break;
		case DungeonRoomType.Wormlike:
			dungeonRoom = new WormlikeDungeonRoom(settings);
			break;
		case DungeonRoomType.LivingTree:
			dungeonRoom = new LivingTreeDungeonRoom(settings);
			break;
		case DungeonRoomType.BiomeSquare:
			dungeonRoom = new BiomeSquareDungeonRoom(settings);
			break;
		case DungeonRoomType.BiomeRugged:
			dungeonRoom = new BiomeRuggedDungeonRoom(settings);
			break;
		case DungeonRoomType.BiomeStructured:
			dungeonRoom = new BiomeStructuredDungeonRoom(settings);
			break;
		case DungeonRoomType.GenShapeCircle:
		case DungeonRoomType.GenShapeMound:
		case DungeonRoomType.GenShapeHourglass:
		case DungeonRoomType.GenShapeDoughnut:
		case DungeonRoomType.GenShapeQuadCircle:
			dungeonRoom = new GenShapeDungeonRoom(settings);
			break;
		}
		if (addToData && dungeonRoom != null)
		{
			CurrentDungeonData.dungeonRooms.Add(dungeonRoom);
		}
		return dungeonRoom;
	}

	public static LegacyDungeonHall MakeDungeon_GetHall_Legacy(LegacyDungeonHallSettings settings)
	{
		return (LegacyDungeonHall)MakeDungeon_GetHall(settings);
	}

	public static DungeonHall MakeDungeon_GetHall(DungeonHallSettings settings, bool addToData = true)
	{
		DungeonHall dungeonHall = null;
		dungeonHall = settings.HallType switch
		{
			DungeonHallType.LegacyEntrance => (DungeonHall)new LegacyEntranceDungeonHall(settings), 
			DungeonHallType.Regular => new RegularDungeonHall(settings), 
			DungeonHallType.Stairwell => new StairwellDungeonHall((StairwellDungeonHallSettings)settings), 
			DungeonHallType.Sine => new SineDungeonHall(settings), 
			_ => new LegacyDungeonHall(settings), 
		};
		if (addToData && dungeonHall != null)
		{
			CurrentDungeonData.dungeonHalls.Add(dungeonHall);
		}
		return dungeonHall;
	}

	public static DungeonEntrance MakeDungeon_GetEntrance(DungeonEntranceSettings settings, bool addToData = true)
	{
		DungeonEntrance dungeonEntrance = null;
		dungeonEntrance = settings.EntranceType switch
		{
			DungeonEntranceType.Dome => (DungeonEntrance)new DomeDungeonEntrance(settings), 
			DungeonEntranceType.Tower => new TowerDungeonEntrance(settings), 
			_ => new LegacyDungeonEntrance(settings), 
		};
		if (addToData && dungeonEntrance != null)
		{
			CurrentDungeonData.dungeonEntrance = dungeonEntrance;
		}
		return dungeonEntrance;
	}
}
