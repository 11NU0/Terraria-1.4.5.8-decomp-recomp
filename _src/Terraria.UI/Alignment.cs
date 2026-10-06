using Microsoft.Xna.Framework;

namespace Terraria.UI;

public struct Alignment
{
	public static readonly Alignment TopLeft = new Alignment(0f, 0f);

	public static readonly Alignment Top = new Alignment(0.5f, 0f);

	public static readonly Alignment TopRight = new Alignment(1f, 0f);

	public static readonly Alignment Left = new Alignment(0f, 0.5f);

	public static readonly Alignment Center = new Alignment(0.5f, 0.5f);

	public static readonly Alignment Right = new Alignment(1f, 0.5f);

	public static readonly Alignment BottomLeft = new Alignment(0f, 1f);

	public static readonly Alignment Bottom = new Alignment(0.5f, 1f);

	public static readonly Alignment BottomRight = new Alignment(1f, 1f);

	public readonly float VerticalOffsetMultiplier;

	public readonly float HorizontalOffsetMultiplier;

	public Vector2 OffsetMultiplier
	{
		get
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			return new Vector2(HorizontalOffsetMultiplier, VerticalOffsetMultiplier);
		}
	}

	private Alignment(float horizontal, float vertical)
	{
		HorizontalOffsetMultiplier = horizontal;
		VerticalOffsetMultiplier = vertical;
	}
}
