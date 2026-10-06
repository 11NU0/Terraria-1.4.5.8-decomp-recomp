using ReLogic.Utilities;

namespace Terraria.GameContent.Generation.Dungeon.Rooms;

public abstract class StepBasedDungeonRoomSettings : DungeonRoomSettings
{
	public int OverrideStrength;

	public int OverrideSteps;

	public Vector2D OverrideStartPosition;

	public Vector2D OverrideEndPosition;

	public Vector2D OverrideVelocity;

	public double OverrideInteriorToExteriorRatio;

	public override int GetBoundingRadius()
	{
		return (int)((double)OverrideStrength * 0.8 + 5.0 + (double)OverrideSteps * 0.5 * 1.4);
	}
}
