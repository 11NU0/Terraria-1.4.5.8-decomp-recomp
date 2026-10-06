namespace Terraria.GameContent.Generation.Dungeon.Rooms;

public class BiomeDungeonRoomSettings : DungeonRoomSettings
{
	public override int GetBoundingRadius()
	{
		return BiomeDungeonRoom.GetBiomeRoomOuterSize(StyleData);
	}
}
