using System;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;

namespace Terraria.Physics;

public static class CollisionVisualizer
{
	public static bool enabled;

	public static Color colorSelf;

	public static Color colorTarget;

	public static void VisualizeElipticalLine(Vector2 attacker, Vector2 target, float allowedDistance, float squishX, float squishY, bool needsCanHitCheck)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		if (enabled)
		{
			Vector2 val = (target - attacker).SafeNormalize(Vector2.UnitX);
			val.X /= squishX;
			val.Y /= squishY;
			Vector2 val2 = Vector2.Zero.MoveTowards(target - attacker, allowedDistance) * new Vector2(squishX, squishY);
			Utils.DrawLine(Main.spriteBatch, attacker, attacker + val2, colorSelf, colorSelf, 8f);
		}
	}

	public static void VisualizeRects(Rectangle attacker, Rectangle target, bool needsCanHitCheck)
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		if (enabled)
		{
			Utils.DrawLine(Main.spriteBatch, new Vector2((float)(attacker.Left + attacker.Width / 2), (float)attacker.Top), new Vector2((float)(attacker.Left + attacker.Width / 2), (float)attacker.Bottom), colorSelf, colorSelf, attacker.Width);
		}
	}

	public static void VisualizeConeFast(Vector2 attacker, Rectangle targetRect, float correctedAngle, float maximumAngle, float allowedDistance, bool needsCanHitCheck)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		if (enabled)
		{
			Color val = colorSelf * 0.3f;
			for (float num = maximumAngle; num > 0f; num -= (float)Math.PI / 180f)
			{
				Vector2 val2 = (correctedAngle - num).ToRotationVector2();
				Vector2 val3 = (correctedAngle + num).ToRotationVector2();
				Utils.DrawLine(Main.spriteBatch, attacker, attacker + val2 * allowedDistance, val, val, 4f);
				Utils.DrawLine(Main.spriteBatch, attacker, attacker + val3 * allowedDistance, val, val, 4f);
			}
		}
	}

	public static void VisualizeConeSlow(Vector2 attacker, Rectangle targetRect, float correctedAngle, float maximumAngle, float allowedDistance, bool needsCanHitCheck)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		if (enabled)
		{
			Color val = colorSelf * 0.3f;
			for (float num = maximumAngle; num > 0f; num -= (float)Math.PI / 180f)
			{
				Vector2 val2 = (correctedAngle - num).ToRotationVector2();
				Vector2 val3 = (correctedAngle + num).ToRotationVector2();
				Utils.DrawLine(Main.spriteBatch, attacker, attacker + val2 * allowedDistance, val, val, 4f);
				Utils.DrawLine(Main.spriteBatch, attacker, attacker + val3 * allowedDistance, val, val, 4f);
			}
		}
	}

	public static void VisualizeAABBvLine(Rectangle targetRect, Vector2 lineStart, Vector2 lineEnd, float lineWidth)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		if (enabled)
		{
			Utils.DrawLine(Main.spriteBatch, lineStart, lineEnd, colorSelf, colorSelf, lineWidth);
		}
	}

	public static void VisualizeAABBvMultiPoint(Rectangle targetRectangle, MultiPointHitbox lightning)
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		if (enabled)
		{
			Vector2[] points = lightning.Points;
			foreach (Vector2 center in points)
			{
				Utils.DrawRect(Main.spriteBatch, Utils.CenteredRectangle(center, new Vector2(4f, 4f)), colorSelf);
			}
		}
	}
}
