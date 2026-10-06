using System;

namespace Terraria.Utilities;

public struct LCG32Random(uint seed)
{
	public uint state = seed;

	public void Advance()
	{
		state = (uint)((int)state * -1856014347 + 1);
	}

	public uint Next(uint maxValue)
	{
		Advance();
		return (uint)((ulong)((long)state * (long)maxValue) >> 32);
	}

	public int Next(int maxValue)
	{
		if (maxValue < 0)
		{
			throw new ArgumentOutOfRangeException("maxValue", "maxValue must be positive.");
		}
		return (int)Next((uint)maxValue);
	}

	public int Next(int minValue, int maxValue)
	{
		return minValue + (int)Next((uint)(maxValue - minValue));
	}

	public double NextDouble()
	{
		Advance();
		return (double)state / 4294967296.0;
	}

	public float NextFloat()
	{
		return (float)NextDouble();
	}
}
