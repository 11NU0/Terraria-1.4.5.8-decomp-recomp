using Microsoft.Xna.Framework;

namespace Terraria.DataStructures;

public class ColorSlidersSet
{
	public float Hue;

	public float Saturation;

	public float Luminance;

	public float Alpha = 1f;

	public void SetHSL(Color color)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		Vector3 val = Main.rgbToHsl(color);
		Hue = val.X;
		Saturation = val.Y;
		Luminance = val.Z;
	}

	public void SetHSL(Vector3 vector)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		Hue = vector.X;
		Saturation = vector.Y;
		Luminance = vector.Z;
	}

	public Color GetColor()
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		Color result = Main.hslToRgb(Hue, Saturation, Luminance);
		result.A = (byte)(Alpha * 255f);
		return result;
	}

	public Vector3 GetHSLVector()
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		return new Vector3(Hue, Saturation, Luminance);
	}

	public void ApplyToMainLegacyBars()
	{
		Main.hBar = Hue;
		Main.sBar = Saturation;
		Main.lBar = Luminance;
		Main.aBar = Alpha;
	}
}
