using Microsoft.Xna.Framework;

namespace Terraria.DataStructures;

public struct DrillDebugDraw(Vector2 p, Color c)
{
	public Vector2 point = p;

	public Color color = c;
}
