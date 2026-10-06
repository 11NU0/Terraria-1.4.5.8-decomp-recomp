using Newtonsoft.Json;

namespace Terraria.Utilities;

public struct IntRange(int minimum, int maximum)
{
	[JsonProperty("Min")]
	public readonly int Minimum = minimum;

	[JsonProperty("Max")]
	public readonly int Maximum = maximum;

	public static IntRange operator *(IntRange range, float scale)
	{
		return new IntRange((int)((float)range.Minimum * scale), (int)((float)range.Maximum * scale));
	}

	public static IntRange operator *(float scale, IntRange range)
	{
		return range * scale;
	}

	public static IntRange operator /(IntRange range, float scale)
	{
		return new IntRange((int)((float)range.Minimum / scale), (int)((float)range.Maximum / scale));
	}

	public static IntRange operator /(float scale, IntRange range)
	{
		return range / scale;
	}
}
