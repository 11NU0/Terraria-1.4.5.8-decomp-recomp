using Terraria.WorldBuilding;

namespace Terraria.GameContent.Generation.Dungeon.Rooms;

public class GenShapeDungeonRoomSettings : DungeonRoomSettings
{
	public GenShapeType ShapeType;

	public GenShape InnerShape;

	public GenShape OuterShape;

	public int BoundingRadius;

	public override int GetBoundingRadius()
	{
		return BoundingRadius;
	}
}
