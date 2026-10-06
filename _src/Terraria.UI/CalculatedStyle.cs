using Microsoft.Xna.Framework;

namespace Terraria.UI;

public struct CalculatedStyle(float x, float y, float width, float height)
{
	public float X = x;

	public float Y = y;

	public float Width = width;

	public float Height = height;

	public Rectangle ToRectangle()
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		return new Rectangle((int)X, (int)Y, (int)Width, (int)Height);
	}

	public Vector2 Position()
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		return new Vector2(X, Y);
	}

	public Vector2 Center()
	{
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		return new Vector2(X + Width * 0.5f, Y + Height * 0.5f);
	}
}
