using System;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.Utilities;

namespace Terraria.GameContent.Events;

public class Sandstorm
{
	private const int SANDSTORM_DURATION_MINIMUM = 28800;

	private const int SANDSTORM_DURATION_MAXIMUM = 86400;

	public static bool Happening;

	public static int TimeLeft;

	public static float Severity;

	public static float IntendedSeverity;

	private static bool HasSufficientWind()
	{
		return Math.Abs(Main.windSpeedCurrent) >= 0.6f;
	}

	public static void WorldClear()
	{
		Happening = false;
	}

	public static void UpdateTime()
	{
		if (Main.netMode != 1)
		{
			if (Happening)
			{
				if (TimeLeft > 86400)
				{
					TimeLeft = 0;
				}
				TimeLeft -= Main.dayRate;
				if (!HasSufficientWind())
				{
					TimeLeft -= 15 * Main.dayRate;
				}
				if (Main.windSpeedCurrent == 0f)
				{
					TimeLeft = 0;
				}
				if (TimeLeft <= 0)
				{
					StopSandstorm();
				}
			}
			else
			{
				int num = 21600;
				num = ((!Main.hardMode) ? (num * 3) : (num * 2));
				if (HasSufficientWind())
				{
					for (int i = 0; i < Main.dayRate; i++)
					{
						if (Main.rand.Next(num) == 0)
						{
							StartSandstorm();
						}
					}
				}
			}
			if (Main.rand.Next(18000) == 0)
			{
				ChangeSeverityIntentions();
			}
		}
		UpdateSeverity();
	}

	private static void ChangeSeverityIntentions()
	{
		if (Happening)
		{
			IntendedSeverity = 0.4f + Main.rand.NextFloat();
		}
		else if (Main.rand.Next(3) == 0)
		{
			IntendedSeverity = 0f;
		}
		else
		{
			IntendedSeverity = Main.rand.NextFloat() * 0.3f;
		}
		if (Main.netMode != 1)
		{
			NetMessage.SendData(7);
		}
	}

	private static void UpdateSeverity()
	{
		if (float.IsNaN(Severity))
		{
			Severity = 0f;
		}
		if (float.IsNaN(IntendedSeverity))
		{
			IntendedSeverity = 0f;
		}
		int num = Math.Sign(IntendedSeverity - Severity);
		Severity = MathHelper.Clamp(Severity + 0.003f * (float)num, 0f, 1f);
		int num2 = Math.Sign(IntendedSeverity - Severity);
		if (num != num2)
		{
			Severity = IntendedSeverity;
		}
	}

	private static void StartSandstorm()
	{
		Happening = true;
		TimeLeft = Main.rand.Next(28800, 86401);
		ChangeSeverityIntentions();
	}

	private static void StopSandstorm()
	{
		Happening = false;
		TimeLeft = 0;
		ChangeSeverityIntentions();
	}

	public static bool ShowSandstormVisuals()
	{
		if (Happening && Main.SceneMetrics.ZoneSandstorm && SurfaceBackgroundID.Sets.IsDesertVariant[Main.bgStyle])
		{
			return Main.bgDelay < 50;
		}
		return false;
	}

	public static void EmitDust()
	{
		//IL_011a: Unknown result type (might be due to invalid IL or missing references)
		//IL_015e: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_031f: Unknown result type (might be due to invalid IL or missing references)
		//IL_032d: Unknown result type (might be due to invalid IL or missing references)
		//IL_035a: Unknown result type (might be due to invalid IL or missing references)
		//IL_035c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0361: Unknown result type (might be due to invalid IL or missing references)
		//IL_0366: Unknown result type (might be due to invalid IL or missing references)
		//IL_0368: Unknown result type (might be due to invalid IL or missing references)
		//IL_0375: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_049e: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_04bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_04cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_04de: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0530: Unknown result type (might be due to invalid IL or missing references)
		//IL_0541: Unknown result type (might be due to invalid IL or missing references)
		//IL_0548: Unknown result type (might be due to invalid IL or missing references)
		//IL_0552: Unknown result type (might be due to invalid IL or missing references)
		//IL_0557: Unknown result type (might be due to invalid IL or missing references)
		//IL_055c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0561: Unknown result type (might be due to invalid IL or missing references)
		//IL_0563: Unknown result type (might be due to invalid IL or missing references)
		//IL_0570: Unknown result type (might be due to invalid IL or missing references)
		if (Main.gamePaused)
		{
			return;
		}
		int desertSandTileCount = Main.SceneMetrics.DesertSandTileCount;
		if (!ShowSandstormVisuals() || desertSandTileCount < 100)
		{
			return;
		}
		int maxValue = 1;
		if (Main.rand.Next(maxValue) != 0)
		{
			return;
		}
		int num = Math.Sign(Main.windSpeedCurrent);
		float num2 = Math.Abs(Main.windSpeedCurrent);
		if (num2 < 0.01f)
		{
			return;
		}
		float num3 = (float)num * MathHelper.Lerp(0.9f, 1f, num2);
		float num4 = 2000f / (float)desertSandTileCount;
		float num5 = 3f / num4;
		num5 = MathHelper.Clamp(num5, 0.77f, 1f);
		int num6 = (int)num4;
		float num7 = (float)Main.screenWidth / (float)Main.maxScreenW;
		int num8 = (int)(1000f * num7);
		float num9 = 20f * Severity;
		float num10 = (float)num8 * (Main.gfxQuality * 0.5f + 0.5f) + (float)num8 * 0.1f - (float)Dust.SandStormCount;
		if (num10 <= 0f)
		{
			return;
		}
		float num11 = (float)Main.screenWidth + 1000f;
		float num12 = Main.screenHeight;
		WeightedRandom<Color> weightedRandom = new WeightedRandom<Color>();
		weightedRandom.Add(new Color(200, 160, 20, 180), Main.SceneMetrics.GetTileCount(53) + Main.SceneMetrics.GetTileCount(396) + Main.SceneMetrics.GetTileCount(397));
		weightedRandom.Add(new Color(103, 98, 122, 180), Main.SceneMetrics.GetTileCount(112) + Main.SceneMetrics.GetTileCount(400) + Main.SceneMetrics.GetTileCount(398));
		weightedRandom.Add(new Color(135, 43, 34, 180), Main.SceneMetrics.GetTileCount(234) + Main.SceneMetrics.GetTileCount(401) + Main.SceneMetrics.GetTileCount(399));
		weightedRandom.Add(new Color(213, 196, 197, 180), Main.SceneMetrics.GetTileCount(116) + Main.SceneMetrics.GetTileCount(403) + Main.SceneMetrics.GetTileCount(402));
		float num13 = MathHelper.Lerp(0.2f, 0.35f, Severity);
		float num14 = MathHelper.Lerp(0.5f, 0.7f, Severity);
		float num15 = (num5 - 0.77f) / 0.23000002f;
		int maxValue2 = (int)MathHelper.Lerp(1f, 10f, num15);
		for (int i = 0; (float)i < num9; i++)
		{
			if (Main.rand.Next(num6 / 4) != 0)
			{
				continue;
			}
			Vector2 val = new Vector2(Main.rand.NextFloat() * num11 - 500f, Main.rand.NextFloat() * -50f);
			if (Main.rand.Next(3) == 0 && num == 1)
			{
				val.X = Main.rand.Next(500) - 500;
			}
			else if (Main.rand.Next(3) == 0 && num == -1)
			{
				val.X = Main.rand.Next(500) + Main.screenWidth;
			}
			if (val.X < 0f || val.X > (float)Main.screenWidth)
			{
				val.Y += Main.rand.NextFloat() * num12 * 0.9f;
			}
			val += Main.screenPosition;
			int num16 = (int)val.X / 16;
			int num17 = (int)val.Y / 16;
			if (!WorldGen.InWorld(num16, num17, 10) || Main.tile[num16, num17] == null || Main.tile[num16, num17].wall != 0)
			{
				continue;
			}
			for (int j = 0; j < 1; j++)
			{
				Dust dust = Main.dust[Dust.NewDust(val, 10, 10, 268)];
				dust.velocity.Y = 2f + Main.rand.NextFloat() * 0.2f;
				dust.velocity.Y *= dust.scale;
				dust.velocity.Y *= 0.35f;
				dust.velocity.X = num3 * 5f + Main.rand.NextFloat() * 1f;
				dust.velocity.X += num3 * num14 * 20f;
				dust.fadeIn += num14 * 0.2f;
				dust.velocity *= 1f + num13 * 0.5f;
				dust.color = weightedRandom;
				dust.velocity *= 1f + num13;
				dust.velocity *= num5;
				dust.scale = 0.9f;
				num10--;
				if (num10 <= 0f)
				{
					break;
				}
				if (Main.rand.Next(maxValue2) != 0)
				{
					j--;
					val += Utils.RandomVector2(Main.rand, -10f, 10f) + dust.velocity * -1.1f;
					num16 = (int)val.X / 16;
					num17 = (int)val.Y / 16;
					if (WorldGen.InWorld(num16, num17, 10) && Main.tile[num16, num17] != null)
					{
						_ = Main.tile[num16, num17].wall;
					}
				}
			}
			if (num10 <= 0f)
			{
				break;
			}
		}
	}
}
