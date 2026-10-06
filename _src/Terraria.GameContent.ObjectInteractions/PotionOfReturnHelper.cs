using Microsoft.Xna.Framework;

namespace Terraria.GameContent.ObjectInteractions;

public class PotionOfReturnHelper
{
	public static bool TryGetGateHitbox(Player player, out Rectangle homeHitbox)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		homeHitbox = Rectangle.Empty;
		if (!player.PotionOfReturnHomePosition.HasValue)
		{
			return false;
		}
		Vector2 val = new Vector2(0f, -21f);
		Vector2 center = player.PotionOfReturnHomePosition.Value + val;
		homeHitbox = Utils.CenteredRectangle(center, new Vector2(24f, 40f));
		return true;
	}
}
