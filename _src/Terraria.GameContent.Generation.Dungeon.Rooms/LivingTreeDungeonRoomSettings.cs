namespace Terraria.GameContent.Generation.Dungeon.Rooms;

public class LivingTreeDungeonRoomSettings : DungeonRoomSettings
{
	public int InnerWidth;

	public int InnerHeight;

	public int Depth;

	public int BoundingRadius;

	public override int GetBoundingRadius()
	{
		return BoundingRadius;
	}
}
