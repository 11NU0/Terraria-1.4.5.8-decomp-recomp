using System;
using Microsoft.Xna.Framework;
using Terraria.WorldBuilding;

namespace Terraria.GameContent.Biomes.Desert;

public static class PitEntrance
{
	public static void Place(DesertDescription description, GenerationProgress progress, float progressMin, float progressMax)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		int holeRadius = WorldGen.genRand.Next(6, 9);
		Rectangle combinedArea = description.CombinedArea;
		Point center = combinedArea.Center;
		center.Y = description.Surface[center.X];
		PlaceAt(description, center, holeRadius, progress, progressMin, progressMax);
	}

	private static void PlaceAt(DesertDescription description, Point position, int holeRadius, GenerationProgress progress, float progressMin, float progressMax)
	{
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_017f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0184: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_011c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0142: Unknown result type (might be due to invalid IL or missing references)
		//IL_015a: Unknown result type (might be due to invalid IL or missing references)
		//IL_016b: Unknown result type (might be due to invalid IL or missing references)
		int num = holeRadius + 3;
		int num2 = num + holeRadius + 3;
		for (int i = -holeRadius - 3; i < holeRadius + 3; i++)
		{
			progress.Set((float)(i + num) / (float)num2, progressMin, progressMax);
			int num3 = description.Surface[i + position.X];
			while (true)
			{
				int num4 = num3;
				Rectangle val = description.Hive;
				if (num4 > val.Top + 10)
				{
					break;
				}
				double num5 = num3 - description.Surface[i + position.X];
				val = description.Hive;
				int top = val.Top;
				val = description.Desert;
				double value = num5 / (double)(top - val.Top);
				value = Utils.Clamp(value, 0.0, 1.0);
				int num6 = (int)(GetHoleRadiusScaleAt(value) * (double)holeRadius);
				if (Math.Abs(i) < num6)
				{
					Main.tile[i + position.X, num3].ClearEverything();
				}
				else if (Math.Abs(i) < num6 + 3 && value > 0.35)
				{
					Main.tile[i + position.X, num3].ResetToType(397);
				}
				double num7 = Math.Abs((double)i / (double)holeRadius);
				num7 *= num7;
				if (Math.Abs(i) < num6 + 3 && (double)(num3 - position.Y) > 15.0 - 3.0 * num7)
				{
					Main.tile[i + position.X, num3].wall = 187;
					WorldGen.SquareWallFrame(i + position.X, num3 - 1);
					WorldGen.SquareWallFrame(i + position.X, num3);
				}
				num3++;
			}
		}
		holeRadius += 4;
		for (int j = -holeRadius; j < holeRadius; j++)
		{
			int num8 = holeRadius - Math.Abs(j);
			num8 = Math.Min(10, num8 * num8);
			for (int k = 0; k < num8; k++)
			{
				Main.tile[j + position.X, k + description.Surface[j + position.X]].ClearEverything();
			}
		}
	}

	private static double GetHoleRadiusScaleAt(double yProgress)
	{
		if (yProgress < 0.6)
		{
			return 1.0;
		}
		return (1.0 - SmootherStep((yProgress - 0.6) / 0.4)) * 0.5 + 0.5;
	}

	private static double SmootherStep(double delta)
	{
		delta = Utils.Clamp(delta, 0.0, 1.0);
		return 1.0 - Math.Cos(delta * 3.1415927410125732) * 0.5 - 0.5;
	}
}
