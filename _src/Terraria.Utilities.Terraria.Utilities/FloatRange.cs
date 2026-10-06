using Microsoft.Xna.Framework;
using Newtonsoft.Json;

namespace Terraria.Utilities.Terraria.Utilities;

public struct FloatRange(float minimum, float maximum)
{
	[JsonProperty("Min")]
	public readonly float Minimum = minimum;

	[JsonProperty("Max")]
	public readonly float Maximum = maximum;

	public bool Contains(float f)
	{
		if (Minimum <= f)
		{
			return f <= Maximum;
		}
		return false;
	}

	public float Lerp(float amount)
	{
		return MathHelper.Lerp(Minimum, Maximum, amount);
	}

	public static FloatRange operator *(FloatRange range, float scale)
	{
		return new FloatRange(range.Minimum * scale, range.Maximum * scale);
	}

	public static FloatRange operator *(float scale, FloatRange range)
	{
		return range * scale;
	}

	public static FloatRange operator /(FloatRange range, float scale)
	{
		return new FloatRange(range.Minimum / scale, range.Maximum / scale);
	}

	public static FloatRange operator /(float scale, FloatRange range)
	{
		return range / scale;
	}
}
