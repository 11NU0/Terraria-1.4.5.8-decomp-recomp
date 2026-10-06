using Microsoft.Xna.Framework;
using ReLogic.Peripherals.RGB;

namespace Terraria.GameContent.RGB;

public class DD2Shader : ChromaShader
{
	private readonly Vector4 _darkGlowColor;

	private readonly Vector4 _lightGlowColor;

	public DD2Shader(Color darkGlowColor, Color lightGlowColor)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		_darkGlowColor = darkGlowColor.ToVector4();
		_lightGlowColor = lightGlowColor.ToVector4();
	}

	[RgbProcessor(/*Could not decode attribute arguments.*/)]
	private void ProcessHighDetail(RgbDevice device, Fragment fragment, EffectDetailLevel quality, float time)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0100: Unknown result type (might be due to invalid IL or missing references)
		//IL_0104: Unknown result type (might be due to invalid IL or missing references)
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		//IL_014f: Unknown result type (might be due to invalid IL or missing references)
		//IL_013e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0140: Unknown result type (might be due to invalid IL or missing references)
		//IL_0147: Unknown result type (might be due to invalid IL or missing references)
		//IL_014c: Unknown result type (might be due to invalid IL or missing references)
		Vector2 val = fragment.CanvasCenter;
		if ((int)quality == 0)
		{
			val = new Vector2(1.7f, 0.5f);
		}
		time *= 0.5f;
		for (int i = 0; i < fragment.Count; i++)
		{
			Vector2 canvasPositionOfIndex = fragment.GetCanvasPositionOfIndex(i);
			Vector4 val2 = new Vector4(0f, 0f, 0f, 1f);
			Vector2 val3 = canvasPositionOfIndex - val;
			float num = val3.Length();
			float num2 = num * num * 0.75f;
			float num3 = (num - time) % 1f;
			if (num3 < 0f)
			{
				num3++;
			}
			num3 = ((!(num3 > 0.8f)) ? (num3 / 0.8f) : (num3 * (1f - (num3 - 1f + 0.2f) / 0.2f)));
			Vector4 val4 = Vector4.Lerp(_darkGlowColor, _lightGlowColor, num3 * num3);
			num3 *= MathHelper.Clamp(1f - num2, 0f, 1f) * 0.75f + 0.25f;
			val2 = Vector4.Lerp(val2, val4, num3);
			if (num < 0.5f)
			{
				float num4 = 1f - MathHelper.Clamp((num - 0.5f + 0.4f) / 0.4f, 0f, 1f);
				val2 = Vector4.Lerp(val2, _lightGlowColor, num4);
			}
			fragment.SetColor(i, val2);
		}
	}
}
