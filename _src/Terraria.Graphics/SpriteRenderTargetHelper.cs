using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;

namespace Terraria.Graphics;

[StructLayout(LayoutKind.Sequential, Size = 1)]
public struct SpriteRenderTargetHelper
{
	public static void GetDrawBoundary(List<DrawData> playerDrawData, out Vector2 lowest, out Vector2 highest)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		lowest = Vector2.Zero;
		highest = Vector2.Zero;
		for (int i = 0; i <= playerDrawData.Count; i++)
		{
			if (i != playerDrawData.Count)
			{
				DrawData cdd = playerDrawData[i];
				if (i == 0)
				{
					lowest = cdd.position;
					highest = cdd.position;
				}
				GetHighsAndLowsOf(ref lowest, ref highest, ref cdd);
			}
		}
	}

	public static void GetHighsAndLowsOf(ref Vector2 lowest, ref Vector2 highest, ref DrawData cdd)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		Vector2 origin = cdd.origin;
		Rectangle val = cdd.destinationRectangle;
		if (cdd.sourceRect.HasValue)
		{
			val = cdd.sourceRect.Value;
		}
		if (!cdd.sourceRect.HasValue)
		{
			val = cdd.texture.Frame();
		}
		val.X = 0;
		val.Y = 0;
		Vector2 pos = cdd.position;
		GetHighsAndLowsOf(ref lowest, ref highest, ref cdd, ref pos, ref origin, new Vector2(0f, 0f));
		GetHighsAndLowsOf(ref lowest, ref highest, ref cdd, ref pos, ref origin, new Vector2((float)val.Width, 0f));
		GetHighsAndLowsOf(ref lowest, ref highest, ref cdd, ref pos, ref origin, new Vector2(0f, (float)val.Height));
		GetHighsAndLowsOf(ref lowest, ref highest, ref cdd, ref pos, ref origin, new Vector2((float)val.Width, (float)val.Height));
	}

	public static void GetHighsAndLowsOf(ref Vector2 lowest, ref Vector2 highest, ref DrawData cdd, ref Vector2 pos, ref Vector2 origin, Vector2 corner)
	{
		//IL_0004: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		Vector2 corner2 = GetCorner(ref cdd, ref pos, ref origin, corner);
		lowest = Vector2.Min(lowest, corner2);
		highest = Vector2.Max(highest, corner2);
	}

	public static Vector2 GetCorner(ref DrawData cdd, ref Vector2 pos, ref Vector2 origin, Vector2 corner)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		Vector2 spinningpoint = corner - origin;
		return pos + spinningpoint.RotatedBy(cdd.rotation) * cdd.scale;
	}
}
