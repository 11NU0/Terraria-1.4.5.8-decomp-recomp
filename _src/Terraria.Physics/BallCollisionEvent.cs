using Microsoft.Xna.Framework;

namespace Terraria.Physics;

public struct BallCollisionEvent(float timeScale, Vector2 normal, Vector2 impactPoint, Tile tile, Entity entity)
{
	public readonly Vector2 Normal = normal;

	public readonly Vector2 ImpactPoint = impactPoint;

	public readonly Tile Tile = tile;

	public readonly Entity Entity = entity;

	public readonly float TimeScale = timeScale;
}
