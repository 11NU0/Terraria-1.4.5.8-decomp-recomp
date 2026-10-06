using Terraria.GameContent.Generation.Dungeon.Rooms;

namespace Terraria.GameContent.Generation.Dungeon.Features;

public class DungeonGlobalBasicChests : GlobalDungeonFeature
{
	public DungeonGlobalBasicChests(DungeonFeatureSettings settings)
		: base(settings)
	{
		DungeonCrawler.CurrentDungeonData.dungeonFeatures.Add(this);
	}

	public override bool GenerateFeature(DungeonData data)
	{
		generated = false;
		BasicChests(data);
		generated = true;
		return true;
	}

	private void BasicChests(DungeonData data)
	{
		for (int i = 0; i < data.dungeonRooms.Count; i++)
		{
			DungeonRoom dungeonRoom = data.dungeonRooms[i];
			for (int j = 0; j < 1000; j++)
			{
				if (dungeonRoom.TryGenerateChestInRoom(data, this))
				{
					break;
				}
			}
		}
	}
}
