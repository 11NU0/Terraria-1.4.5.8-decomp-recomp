using System;
using Microsoft.Xna.Framework;
using ReLogic.Peripherals.RGB;

namespace Terraria.GameContent.RGB;

public class MartianMadnessShader : ChromaShader
{
	private readonly Vector4 _metalColor;

	private readonly Vector4 _glassColor;

	private readonly Vector4 _beamColor;

	private readonly Vector4 _backgroundColor;

	public MartianMadnessShader(Color metalColor, Color glassColor, Color beamColor, Color backgroundColor)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		_metalColor = metalColor.ToVector4();
		_glassColor = glassColor.ToVector4();
		_beamColor = beamColor.ToVector4();
		_backgroundColor = backgroundColor.ToVector4();
	}

	[RgbProcessor(/*Could not decode attribute arguments.*/)]
	private void ProcessLowDetail(RgbDevice device, Fragment fragment, EffectDetailLevel quality, float time)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		for (int i = 0; i < fragment.Count; i++)
		{
			Vector2 canvasPositionOfIndex = fragment.GetCanvasPositionOfIndex(i);
			Point gridPositionOfIndex = fragment.GetGridPositionOfIndex(i);
			float num = (float)Math.Sin(time * 2f + canvasPositionOfIndex.X * 5f) * 0.5f + 0.5f;
			int num2 = (gridPositionOfIndex.X + gridPositionOfIndex.Y) % 2;
			if (num2 < 0)
			{
				num2 += 2;
			}
			Vector4 val = ((num2 == 1) ? Vector4.Lerp(_glassColor, _beamColor, num) : _metalColor);
			fragment.SetColor(i, val);
		}
	}

	[RgbProcessor(/*Could not decode attribute arguments.*/)]
	private void ProcessHighDetail(RgbDevice device, Fragment fragment, EffectDetailLevel quality, float time)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Invalid comparison between Unknown and I4
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_011a: Unknown result type (might be due to invalid IL or missing references)
		//IL_011b: Unknown result type (might be due to invalid IL or missing references)
		//IL_011c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0121: Unknown result type (might be due to invalid IL or missing references)
		//IL_0191: Unknown result type (might be due to invalid IL or missing references)
		//IL_0192: Unknown result type (might be due to invalid IL or missing references)
		//IL_0193: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_0180: Unknown result type (might be due to invalid IL or missing references)
		//IL_0183: Unknown result type (might be due to invalid IL or missing references)
		//IL_018a: Unknown result type (might be due to invalid IL or missing references)
		//IL_018f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0106: Unknown result type (might be due to invalid IL or missing references)
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		//IL_0113: Unknown result type (might be due to invalid IL or missing references)
		//IL_0118: Unknown result type (might be due to invalid IL or missing references)
		//IL_023c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0229: Unknown result type (might be due to invalid IL or missing references)
		//IL_022c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0233: Unknown result type (might be due to invalid IL or missing references)
		//IL_0238: Unknown result type (might be due to invalid IL or missing references)
		if ((int)device.Type != 0 && (int)device.Type != 6)
		{
			ProcessLowDetail(device, fragment, quality, time);
			return;
		}
		float num = time * 0.5f % ((float)Math.PI * 2f);
		if (num > (float)Math.PI)
		{
			num = (float)Math.PI * 2f - num;
		}
		Vector2 val = new Vector2(1.7f + (float)Math.Cos(num) * 2f, -0.5f + (float)Math.Sin(num) * 1.1f);
		for (int i = 0; i < fragment.Count; i++)
		{
			Vector2 canvasPositionOfIndex = fragment.GetCanvasPositionOfIndex(i);
			Vector4 val2 = _backgroundColor;
			float num2 = Math.Abs(val.X - canvasPositionOfIndex.X);
			if (canvasPositionOfIndex.Y > val.Y && num2 < 0.2f)
			{
				float num3 = 1f - MathHelper.Clamp((num2 - 0.2f + 0.2f) / 0.2f, 0f, 1f);
				float num4 = Math.Abs((num - (float)Math.PI / 2f) / ((float)Math.PI / 2f));
				num4 = Math.Max(0f, 1f - num4 * 3f);
				val2 = Vector4.Lerp(val2, _beamColor, num3 * num4);
			}
			Vector2 val3 = val - canvasPositionOfIndex;
			val3.X /= 1f;
			val3.Y /= 0.2f;
			float num5 = val3.Length();
			if (num5 < 1f)
			{
				float num6 = 1f - MathHelper.Clamp((num5 - 1f + 0.2f) / 0.2f, 0f, 1f);
				val2 = Vector4.Lerp(val2, _metalColor, num6);
			}
			Vector2 val4 = val - canvasPositionOfIndex + new Vector2(0f, -0.1f);
			val4.X /= 0.3f;
			val4.Y /= 0.3f;
			if (val4.Y < 0f)
			{
				val4.Y *= 2f;
			}
			float num7 = val4.Length();
			if (num7 < 1f)
			{
				float num8 = 1f - MathHelper.Clamp((num7 - 1f + 0.2f) / 0.2f, 0f, 1f);
				val2 = Vector4.Lerp(val2, _glassColor, num8);
			}
			fragment.SetColor(i, val2);
		}
	}
}
