using Microsoft.Xna.Framework;
using Terraria.GameContent.Generation.Dungeon.Halls;
using Terraria.GameContent.Generation.Dungeon.Rooms;
using Terraria.Localization;
using Terraria.Utilities;
using Terraria.WorldBuilding;

namespace Terraria.GameContent.Generation.Dungeon.LayoutProviders;

public class LegacyDungeonLayoutProvider : DungeonLayoutProvider
{
	public LegacyDungeonLayoutProvider(DungeonLayoutProviderSettings settings)
		: base(settings)
	{
	}

	public override void ProvideLayout(DungeonData data, GenerationProgress progress, UnifiedRandom genRand, ref int roomDelay)
	{
		LegacyDungeonLayoutProviderSettings legacyDungeonLayoutProviderSettings = (LegacyDungeonLayoutProviderSettings)settings;
		int steps = legacyDungeonLayoutProviderSettings.Steps;
		int maxSteps = legacyDungeonLayoutProviderSettings.MaxSteps;
		LegacyDungeonLayout(data, progress, genRand, settings.StyleData.BrickTileType, settings.StyleData.BrickCrackedTileType, settings.StyleData.BrickWallType, steps, maxSteps, ref roomDelay);
	}

	public void LegacyDungeonLayout(DungeonData data, GenerationProgress progress, UnifiedRandom genRand, ushort tileType, ushort crackedTileType, ushort wallType, int steps, int maxSteps, ref int roomDelay)
	{
		//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		//IL_030a: Unknown result type (might be due to invalid IL or missing references)
		//IL_030f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0296: Unknown result type (might be due to invalid IL or missing references)
		//IL_029b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0240: Unknown result type (might be due to invalid IL or missing references)
		//IL_0245: Unknown result type (might be due to invalid IL or missing references)
		if (data.genVars.preGenDungeonEntranceSettings.PrecalculateEntrancePosition)
		{
			data.genVars.generatingDungeonPositionX = -10 + (int)data.genVars.dungeonEntrancePosition.X + genRand.Next(20);
			data.genVars.generatingDungeonPositionY = (int)data.genVars.dungeonEntrancePosition.Y + 30;
		}
		data.outerProgressionBounds = new DungeonBounds[1];
		data.outerProgressionBounds[0] = data.genVars.outerPotentialDungeonBounds;
		LegacyDungeonHallSettings legacyDungeonHallSettings = new LegacyDungeonHallSettings
		{
			StyleData = data.genVars.dungeonStyle,
			RandomSeed = genRand.Next()
		};
		LegacyDungeonRoomSettings legacyDungeonRoomSettings = new LegacyDungeonRoomSettings
		{
			StyleData = data.genVars.dungeonStyle,
			RandomSeed = genRand.Next()
		};
		DungeonCrawler.MakeDungeon_GetRoom(new LegacyDungeonRoomSettings
		{
			StyleData = data.genVars.dungeonStyle,
			StartingRoom = true,
			RandomSeed = genRand.Next(),
			RoomPosition = new Point(data.genVars.generatingDungeonPositionX, data.genVars.generatingDungeonPositionY)
		}).GenerateRoom(data);
		while (steps > 0)
		{
			data.dungeonBounds.UpdateBounds(data.genVars.generatingDungeonPositionX, data.genVars.generatingDungeonPositionY);
			steps--;
			int num = (maxSteps - steps) / maxSteps * 60;
			DungeonUtils.UpdateDungeonProgress(progress, (float)num / 100f, Language.GetTextValue("WorldGeneration.DungeonRoomsAndHalls"));
			if (roomDelay > 0)
			{
				roomDelay--;
			}
			if ((roomDelay == 0) & (genRand.Next(3) == 0))
			{
				roomDelay = 5;
				if (genRand.Next(2) == 0)
				{
					int generatingDungeonPositionX = data.genVars.generatingDungeonPositionX;
					int generatingDungeonPositionY = data.genVars.generatingDungeonPositionY;
					legacyDungeonHallSettings.RandomSeed = genRand.Next();
					DungeonCrawler.MakeDungeon_GetHall_Legacy(legacyDungeonHallSettings).GenerateHall(data, data.genVars.generatingDungeonPositionX, data.genVars.generatingDungeonPositionY);
					if (genRand.Next(2) == 0)
					{
						legacyDungeonHallSettings.RandomSeed = genRand.Next();
						DungeonCrawler.MakeDungeon_GetHall_Legacy(legacyDungeonHallSettings).GenerateHall(data, data.genVars.generatingDungeonPositionX, data.genVars.generatingDungeonPositionY);
					}
					legacyDungeonRoomSettings.RandomSeed = genRand.Next();
					legacyDungeonRoomSettings.RoomPosition = new Point(data.genVars.generatingDungeonPositionX, data.genVars.generatingDungeonPositionY);
					DungeonCrawler.MakeDungeon_GetRoom(legacyDungeonRoomSettings).GenerateRoom(data);
					data.genVars.generatingDungeonPositionX = generatingDungeonPositionX;
					data.genVars.generatingDungeonPositionY = generatingDungeonPositionY;
				}
				else
				{
					legacyDungeonRoomSettings.RandomSeed = genRand.Next();
					legacyDungeonRoomSettings.RoomPosition = new Point(data.genVars.generatingDungeonPositionX, data.genVars.generatingDungeonPositionY);
					DungeonCrawler.MakeDungeon_GetRoom(legacyDungeonRoomSettings).GenerateRoom(data);
				}
			}
			else
			{
				legacyDungeonHallSettings.RandomSeed = genRand.Next();
				DungeonCrawler.MakeDungeon_GetHall_Legacy(legacyDungeonHallSettings).GenerateHall(data, data.genVars.generatingDungeonPositionX, data.genVars.generatingDungeonPositionY);
			}
		}
		legacyDungeonRoomSettings.RandomSeed = genRand.Next();
		legacyDungeonRoomSettings.RoomPosition = new Point(data.genVars.generatingDungeonPositionX, data.genVars.generatingDungeonPositionY);
		DungeonCrawler.MakeDungeon_GetRoom(legacyDungeonRoomSettings).GenerateRoom(data);
		data.outerProgressionBounds[0] = data.dungeonBounds;
	}
}
