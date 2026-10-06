using Microsoft.Xna.Framework;

namespace Terraria.Graphics;

public struct VertexColors
{
	public Color TopLeftColor;

	public Color TopRightColor;

	public Color BottomLeftColor;

	public Color BottomRightColor;

	public VertexColors(Color color)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		TopLeftColor = color;
		TopRightColor = color;
		BottomRightColor = color;
		BottomLeftColor = color;
	}

	public VertexColors(Color topLeft, Color topRight, Color bottomRight, Color bottomLeft)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		TopLeftColor = topLeft;
		TopRightColor = topRight;
		BottomLeftColor = bottomLeft;
		BottomRightColor = bottomRight;
	}

	public static implicit operator VertexColors(Color color)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		return new VertexColors(color);
	}
}
