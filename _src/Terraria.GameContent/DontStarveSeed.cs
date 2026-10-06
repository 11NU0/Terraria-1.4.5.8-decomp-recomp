using System;
using Microsoft.Xna.Framework;
using Terraria.Enums;

namespace Terraria.GameContent;

public class DontStarveSeed
{
	public static void ModifyNightColor(ref Color bgColorToSet, ref Color moonColor)
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		if (Main.GetMoonPhase() != MoonPhase.Full)
		{
			float fromValue = (float)(Main.time / 32400.0);
			Color val = bgColorToSet;
			Color black = Color.Black;
			Color val2 = bgColorToSet;
			float num = Utils.Remap(fromValue, 0f, 0.5f, 0f, 1f);
			float num2 = Utils.Remap(fromValue, 0.5f, 1f, 0f, 1f);
			Color val3 = Color.Lerp(Color.Lerp(val, black, num), val2, num2);
			bgColorToSet = val3;
		}
	}

	public static void ModifyMinimumLightColorAtNight(ref byte minimalLight)
	{
		switch (Main.GetMoonPhase())
		{
		case MoonPhase.Empty:
			minimalLight = 1;
			break;
		case MoonPhase.QuarterAtLeft:
		case MoonPhase.QuarterAtRight:
			minimalLight = 1;
			break;
		case MoonPhase.HalfAtLeft:
		case MoonPhase.HalfAtRight:
			minimalLight = 1;
			break;
		case MoonPhase.ThreeQuartersAtLeft:
		case MoonPhase.ThreeQuartersAtRight:
			minimalLight = 1;
			break;
		case MoonPhase.Full:
			minimalLight = 45;
			break;
		}
		if (Main.bloodMoon)
		{
			minimalLight = Utils.Max(new byte[2] { minimalLight, 35 });
		}
	}

	public static void FixBiomeDarkness(ref Color bgColor, ref int R, ref int G, ref int B)
	{
		if (Main.dontStarveWorld)
		{
			R = (byte)Math.Min(bgColor.R, R);
			G = (byte)Math.Min(bgColor.G, G);
			B = (byte)Math.Min(bgColor.B, B);
		}
	}

	public static void Initialize()
	{
		Player.Hooks.OnEnterWorld += Hook_OnEnterWorld;
	}

	private static void Hook_OnEnterWorld(Player player)
	{
		player.UpdateStarvingState(withEmote: false);
	}
}
