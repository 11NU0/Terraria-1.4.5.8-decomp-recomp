namespace Terraria.GameContent.Generation.Dungeon.Rooms;

public class RegularDungeonRoomSettings : DungeonRoomSettings
{
	public int OverrideInnerBoundsSize;

	public int OverrideOuterBoundsSize;

	public override int GetBoundingRadius()
	{
		return (OverrideInnerBoundsSize + OverrideOuterBoundsSize) * 142 / 100;
	}
}
