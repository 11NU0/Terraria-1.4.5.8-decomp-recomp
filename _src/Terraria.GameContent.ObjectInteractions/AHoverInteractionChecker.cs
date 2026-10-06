using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria.GameInput;

namespace Terraria.GameContent.ObjectInteractions;

public abstract class AHoverInteractionChecker
{
	internal enum HoverStatus
	{
		NotSelectable,
		SelectableButNotSelected,
		Selected
	}

	internal HoverStatus AttemptInteraction(Player player, Rectangle Hitbox)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
		Point val = Hitbox.ClosestPointInRect(player.Center).ToTileCoordinates();
		if (!player.IsInTileInteractionRange(val.X, val.Y, TileReachCheckSettings.Simple))
		{
			return HoverStatus.NotSelectable;
		}
		Vector2 v = Main.ReverseGravitySupport(Main.MouseScreen) + Main.screenPosition;
		bool flag = Hitbox.Contains(v.ToPoint());
		bool flag2 = flag;
		bool? flag3 = AttemptOverridingHoverStatus(player, Hitbox);
		if (flag3.HasValue)
		{
			flag2 = flag3.Value;
		}
		flag2 &= !player.lastMouseInterface;
		bool flag4 = !Main.SmartCursorIsUsed && !PlayerInput.UsingGamepad;
		if (!flag2)
		{
			if (!flag4)
			{
				return HoverStatus.SelectableButNotSelected;
			}
			return HoverStatus.NotSelectable;
		}
		Main.HasInteractableObjectThatIsNotATile = true;
		if (flag)
		{
			DoHoverEffect(player, Hitbox);
		}
		if (PlayerInput.UsingGamepad)
		{
			player.GamepadEnableGrappleCooldown();
		}
		bool flag5 = ShouldBlockInteraction(player, Hitbox);
		if (Main.mouseRight && Main.mouseRightRelease && !flag5)
		{
			Main.mouseRightRelease = false;
			player.tileInteractAttempted = true;
			player.tileInteractionHappened = true;
			player.releaseUseTile = false;
			PerformInteraction(player, Hitbox);
		}
		if (!Main.SmartCursorIsUsed && !PlayerInput.UsingGamepad)
		{
			return HoverStatus.NotSelectable;
		}
		if (!flag4)
		{
			return HoverStatus.Selected;
		}
		return HoverStatus.NotSelectable;
	}

	internal abstract bool? AttemptOverridingHoverStatus(Player player, Rectangle rectangle);

	internal abstract void DoHoverEffect(Player player, Rectangle hitbox);

	internal abstract bool ShouldBlockInteraction(Player player, Rectangle hitbox);

	internal abstract void PerformInteraction(Player player, Rectangle hitbox);
}
