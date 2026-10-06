namespace Terraria.GameContent.Generation.Dungeon.Rooms;

public class WormlikeDungeonRoomSettings : DungeonRoomSettings
{
	public int FirstSideIterations;

	public int SecondSideIterations;

	public override int GetBoundingRadius()
	{
		return (int)((16.200000000000003 + (double)(FirstSideIterations + SecondSideIterations) * 0.5 * 1.4) * 0.5);
	}
}
