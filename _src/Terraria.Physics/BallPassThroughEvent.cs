namespace Terraria.Physics;

public struct BallPassThroughEvent(float timeScale, Tile tile, Entity entity, BallPassThroughType type)
{
	public readonly Tile Tile = tile;

	public readonly Entity Entity = entity;

	public readonly BallPassThroughType Type = type;

	public readonly float TimeScale = timeScale;
}
