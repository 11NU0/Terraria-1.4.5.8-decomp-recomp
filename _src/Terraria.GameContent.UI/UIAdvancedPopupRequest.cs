using Microsoft.Xna.Framework;

namespace Terraria.GameContent.UI;

public struct UIAdvancedPopupRequest
{
	public UIPopupTextContext Context;

	public UIPopupTextAlignment Alignment;

	public string Text;

	public Color Color;

	public int DurationInFrames;

	public Vector2 Position;

	public Vector2 Velocity;
}
