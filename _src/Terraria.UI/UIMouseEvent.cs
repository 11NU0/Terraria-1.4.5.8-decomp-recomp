using Microsoft.Xna.Framework;

namespace Terraria.UI;

public class UIMouseEvent : UIEvent
{
	public readonly Vector2 MousePosition;

	public UIMouseEvent(UIElement target, Vector2 mousePosition)
		: base(target)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		MousePosition = mousePosition;
	}
}
