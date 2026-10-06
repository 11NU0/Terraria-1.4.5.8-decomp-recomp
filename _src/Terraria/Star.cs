using System;
using Microsoft.Xna.Framework;
using Terraria.Utilities;

namespace Terraria;

public class Star
{
	public Vector2 position;

	public float scale;

	public float rotation;

	public int type;

	public float twinkle;

	public float twinkleSpeed;

	public float rotationSpeed;

	public bool falling;

	public bool hidden;

	public Vector2 fallSpeed;

	public int fallTime;

	public Vector2 velocity;

	public static bool dayCheck = false;

	public static float starfallBoost = 1f;

	public static int starFallCount = 0;

	public float fadeIn;

	public static void NightSetup()
	{
		starfallBoost = 1f;
		int maxValue = 10;
		int maxValue2 = 3;
		if (Main.tenthAnniversaryWorld)
		{
			maxValue = 5;
			maxValue2 = 2;
		}
		if (Main.rand.Next(maxValue) == 0)
		{
			starfallBoost = (float)Main.rand.Next(300, 501) * 0.01f;
		}
		else if (Main.rand.Next(maxValue2) == 0)
		{
			starfallBoost = (float)Main.rand.Next(100, 151) * 0.01f;
		}
		starFallCount = 0;
	}

	public static void StarFall(float positionX)
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		starFallCount++;
		int num = -1;
		float num2 = -1f;
		float num3 = positionX / Main.rightWorld * (float)Main.MaxWorldViewSize.X;
		for (int i = 0; i < Main.numStars; i++)
		{
			if (!Main.star[i].hidden && !Main.star[i].falling)
			{
				float num4 = Math.Abs(Main.star[i].position.X - num3);
				if (num2 == -1f || num4 < num2)
				{
					num = i;
					num2 = num4;
				}
			}
		}
		if (num >= 0)
		{
			Main.star[num].Fall();
		}
	}

	public static void SpawnStars(int s = -1)
	{
		//IL_01ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f9: Unknown result type (might be due to invalid IL or missing references)
		FastRandom fastRandom = FastRandom.CreateWithRandomSeed();
		int num = fastRandom.Next(200, 400);
		int num2 = 0;
		int num3 = num;
		if (s >= 0)
		{
			num2 = s;
			num3 = s + 1;
		}
		for (int i = num2; i < num3; i++)
		{
			Main.star[i] = new Star();
			if (s >= 0)
			{
				Main.star[i].fadeIn = 1f;
				int num4 = 10;
				int num5 = -2000;
				for (int j = 0; j < num4; j++)
				{
					float num6 = fastRandom.Next(1921);
					int num7 = 2000;
					for (int k = 0; k < Main.numStars; k++)
					{
						if (k != s && !Main.star[k].hidden && !Main.star[k].falling)
						{
							int num8 = (int)Math.Abs(num6 - Main.star[k].position.X);
							if (num8 < num7)
							{
								num7 = num8;
							}
						}
					}
					if (s == 0 || num7 > num5)
					{
						num5 = num7;
						Main.star[i].position.X = num6;
					}
				}
			}
			else
			{
				Main.star[i].position.X = fastRandom.Next(1921);
			}
			Main.star[i].position.Y = fastRandom.Next(1201);
			Main.star[i].rotation = (float)fastRandom.Next(628) * 0.01f;
			Main.star[i].scale = (float)fastRandom.Next(70, 130) * 0.006f;
			Main.star[i].type = fastRandom.Next(0, 4);
			Main.star[i].twinkle = (float)fastRandom.Next(60, 101) * 0.01f;
			Main.star[i].twinkleSpeed = (float)fastRandom.Next(30, 110) * 0.0001f;
			Star obj = Main.star[i];
			obj.velocity *= 0f;
			if (fastRandom.Next(2) == 0)
			{
				Main.star[i].twinkleSpeed *= -1f;
			}
			Main.star[i].rotationSpeed = (float)fastRandom.Next(5, 50) * 0.0001f;
			if (fastRandom.Next(2) == 0)
			{
				Main.star[i].rotationSpeed *= -1f;
			}
			if (fastRandom.Next(40) == 0)
			{
				Main.star[i].scale *= 2f;
				Main.star[i].twinkleSpeed /= 2f;
				Main.star[i].rotationSpeed /= 2f;
			}
		}
		if (s == -1)
		{
			Main.numStars = num;
		}
	}

	public void Fall()
	{
		if (!WorldGen.SecretSeed.anySecretSeedIsActive || Main.starGame)
		{
			fallTime = 0;
			falling = true;
			fallSpeed.Y = (float)Main.rand.Next(700, 1001) * 0.01f;
			fallSpeed.X = (float)Main.rand.Next(-400, 401) * 0.01f;
		}
	}

	public void Update()
	{
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		if (falling && !hidden)
		{
			fallTime += Main.dayRate;
			position += fallSpeed * (float)(Main.dayRate + 99) / 100f;
			if (position.Y > 1500f)
			{
				hidden = true;
			}
			if (Main.starGame && position.Length() > 99999f)
			{
				hidden = true;
			}
			twinkle += twinkleSpeed * 3f;
			if (twinkle > 1f)
			{
				twinkle = 1f;
				twinkleSpeed *= -1f;
			}
			else if ((double)twinkle < 0.6)
			{
				twinkle = 0.6f;
				twinkleSpeed *= -1f;
			}
			rotation += 0.5f;
			if ((double)rotation > 6.28)
			{
				rotation -= 6.28f;
			}
			if (rotation < 0f)
			{
				rotation += 6.28f;
			}
			return;
		}
		if (fadeIn > 0f)
		{
			float num = 6.1728395E-05f * (float)Main.dayRate;
			num *= 10f;
			fadeIn -= num;
			if (fadeIn < 0f)
			{
				fadeIn = 0f;
			}
		}
		twinkle += twinkleSpeed;
		if (twinkle > 1f)
		{
			twinkle = 1f;
			twinkleSpeed *= -1f;
		}
		else if ((double)twinkle < 0.6)
		{
			twinkle = 0.6f;
			twinkleSpeed *= -1f;
		}
		rotation += rotationSpeed;
		if ((double)rotation > 6.28)
		{
			rotation -= 6.28f;
		}
		if (rotation < 0f)
		{
			rotation += 6.28f;
		}
	}

	public static void UpdateStars()
	{
		//IL_029d: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0332: Unknown result type (might be due to invalid IL or missing references)
		//IL_0102: Unknown result type (might be due to invalid IL or missing references)
		//IL_010e: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0124: Unknown result type (might be due to invalid IL or missing references)
		//IL_0130: Unknown result type (might be due to invalid IL or missing references)
		//IL_0135: Unknown result type (might be due to invalid IL or missing references)
		//IL_013a: Unknown result type (might be due to invalid IL or missing references)
		//IL_013c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0143: Unknown result type (might be due to invalid IL or missing references)
		//IL_014b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0152: Unknown result type (might be due to invalid IL or missing references)
		//IL_015d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0171: Unknown result type (might be due to invalid IL or missing references)
		//IL_0176: Unknown result type (might be due to invalid IL or missing references)
		//IL_0181: Unknown result type (might be due to invalid IL or missing references)
		//IL_0261: Unknown result type (might be due to invalid IL or missing references)
		//IL_0273: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01dc: Unknown result type (might be due to invalid IL or missing references)
		if (Main.onlyDrawFancyUI)
		{
			return;
		}
		if (!Main.dayTime)
		{
			dayCheck = false;
		}
		else if (!dayCheck && Main.time >= 27000.0)
		{
			for (int i = 0; i < Main.numStars; i++)
			{
				if (Main.star[i].hidden)
				{
					SpawnStars(i);
				}
			}
		}
		for (int j = 0; j < Main.numStars; j++)
		{
			Main.star[j].Update();
		}
		if (!Main.gameMenu || !WorldGen.generatingWorld || !WorldGen.SecretSeed.anySecretSeedIsActive)
		{
			return;
		}
		for (int k = 0; k < Main.numStars; k++)
		{
			if (Main.star[k].falling || Main.star[k].hidden || !(Main.star[k].scale > 0f))
			{
				continue;
			}
			for (int l = 0; l < Main.numStars; l++)
			{
				if (k == l || Main.star[l].falling || Main.star[l].hidden || !(Main.star[l].position != Main.star[k].position))
				{
					continue;
				}
				Vector2 val = Main.star[l].position - Main.star[k].position;
				float num = val.X * val.X + val.Y * val.Y;
				val *= 0.005f * Main.star[l].scale;
				if (num != 0f)
				{
					if (val.X != 0f)
					{
						Main.star[k].velocity.X += val.X / num / Main.star[k].scale;
					}
					if (val.Y != 0f)
					{
						Main.star[k].velocity.Y += val.Y / num / Main.star[k].scale;
					}
				}
				if (float.IsNaN(Main.star[k].velocity.X) || float.IsInfinity(Main.star[k].velocity.X) || float.IsNaN(Main.star[k].velocity.Y) || float.IsInfinity(Main.star[k].velocity.Y))
				{
					Main.star[k].velocity = default;
					Main.star[k].position = default;
					Main.star[k].hidden = true;
				}
			}
			Star obj = Main.star[k];
			obj.position += Main.star[k].velocity;
			Main.star[k].rotation += Main.star[k].velocity.X * 0.02f;
			if (Main.star[k].position.X < 0f)
			{
				Main.star[k].velocity.X = Math.Abs(Main.star[k].velocity.X);
			}
			if (Main.star[k].position.X > (float)Main.MaxWorldViewSize.X)
			{
				Main.star[k].velocity.X = 0f - Math.Abs(Main.star[k].velocity.X);
			}
			if (Main.star[k].position.Y < 0f)
			{
				Main.star[k].velocity.Y = Math.Abs(Main.star[k].velocity.Y);
			}
			if (Main.star[k].position.Y > (float)Main.MaxWorldViewSize.Y)
			{
				Main.star[k].velocity.Y = 0f - Math.Abs(Main.star[k].velocity.Y);
			}
		}
	}
}
