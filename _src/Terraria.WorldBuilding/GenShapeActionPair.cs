namespace Terraria.WorldBuilding;

public struct GenShapeActionPair(GenShape shape, GenAction action)
{
	public readonly GenShape Shape = shape;

	public readonly GenAction Action = action;
}
