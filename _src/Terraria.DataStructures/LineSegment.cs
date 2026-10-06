using Microsoft.Xna.Framework;

namespace Terraria.DataStructures;

public struct LineSegment(Vector2 start, Vector2 end)
{
	public Vector2 Start = start;

	public Vector2 End = end;
}
