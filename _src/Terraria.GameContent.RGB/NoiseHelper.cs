using System;
using Microsoft.Xna.Framework;
using Terraria.Utilities;

namespace Terraria.GameContent.RGB;

public static class NoiseHelper
{
	private const int RANDOM_SEED = 1;

	private const int NOISE_2D_SIZE = 32;

	private const int NOISE_2D_SIZE_MASK = 31;

	private const int NOISE_SIZE_MASK = 1023;

	private static readonly float[] StaticNoise = CreateStaticNoise(1024);

	private static float[] CreateStaticNoise(int length)
	{
		UnifiedRandom r = new UnifiedRandom(1);
		float[] array = new float[length];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = r.NextFloat();
		}
		return array;
	}

	public static float GetDynamicNoise(int index, float currentTime)
	{
		float num = StaticNoise[index & 0x3FF];
		float num2 = currentTime % 1f;
		return Math.Abs(Math.Abs(num - num2) - 0.5f) * 2f;
	}

	public static float GetStaticNoise(int index)
	{
		return StaticNoise[index & 0x3FF];
	}

	public static float GetDynamicNoise(int x, int y, float currentTime)
	{
		return GetDynamicNoiseInternal(x, y, currentTime % 1f);
	}

	private static float GetDynamicNoiseInternal(int x, int y, float wrappedTime)
	{
		x &= 0x1F;
		y &= 0x1F;
		return Math.Abs(Math.Abs(StaticNoise[y * 32 + x] - wrappedTime) - 0.5f) * 2f;
	}

	public static float GetStaticNoise(int x, int y)
	{
		x &= 0x1F;
		y &= 0x1F;
		return StaticNoise[y * 32 + x];
	}

	public static float GetDynamicNoise(Vector2 position, float currentTime)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		position *= 10f;
		currentTime %= 1f;
		Vector2 val = new Vector2((float)Math.Floor(position.X), (float)Math.Floor(position.Y));
		Point val2 = new Point((int)val.X, (int)val.Y);
		Vector2 val3 = new Vector2(position.X - val.X, position.Y - val.Y);
		float num = MathHelper.Lerp(GetDynamicNoiseInternal(val2.X, val2.Y, currentTime), GetDynamicNoiseInternal(val2.X, val2.Y + 1, currentTime), val3.Y);
		float num2 = MathHelper.Lerp(GetDynamicNoiseInternal(val2.X + 1, val2.Y, currentTime), GetDynamicNoiseInternal(val2.X + 1, val2.Y + 1, currentTime), val3.Y);
		return MathHelper.Lerp(num, num2, val3.X);
	}

	public static float GetStaticNoise(Vector2 position)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		position *= 10f;
		Vector2 val = new Vector2((float)Math.Floor(position.X), (float)Math.Floor(position.Y));
		Point val2 = new Point((int)val.X, (int)val.Y);
		Vector2 val3 = new Vector2(position.X - val.X, position.Y - val.Y);
		float num = MathHelper.Lerp(GetStaticNoise(val2.X, val2.Y), GetStaticNoise(val2.X, val2.Y + 1), val3.Y);
		float num2 = MathHelper.Lerp(GetStaticNoise(val2.X + 1, val2.Y), GetStaticNoise(val2.X + 1, val2.Y + 1), val3.Y);
		return MathHelper.Lerp(num, num2, val3.X);
	}
}
