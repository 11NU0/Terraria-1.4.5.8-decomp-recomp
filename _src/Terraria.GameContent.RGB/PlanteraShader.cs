using System;
using Microsoft.Xna.Framework;
using ReLogic.Peripherals.RGB;

namespace Terraria.GameContent.RGB;

public class PlanteraShader : ChromaShader
{
	private readonly Vector4 _bulbColor;

	private readonly Vector4 _vineColor;

	private readonly Vector4 _backgroundColor;

	public PlanteraShader(Color bulbColor, Color vineColor, Color backgroundColor)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		_bulbColor = bulbColor.ToVector4();
		_vineColor = vineColor.ToVector4();
		_backgroundColor = backgroundColor.ToVector4();
	}

	[RgbProcessor(/*Could not decode attribute arguments.*/)]
	private void ProcessLowDetail(RgbDevice device, Fragment fragment, EffectDetailLevel quality, float time)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		for (int i = 0; i < fragment.Count; i++)
		{
			Vector2 canvasPositionOfIndex = fragment.GetCanvasPositionOfIndex(i);
			Vector4 val = Vector4.Lerp(_bulbColor, _vineColor, (float)Math.Sin(time * 2f + canvasPositionOfIndex.X * 10f) * 0.5f + 0.5f);
			fragment.SetColor(i, val);
		}
	}

	[RgbProcessor(/*Could not decode attribute arguments.*/)]
	private void ProcessHighDetail(RgbDevice device, Fragment fragment, EffectDetailLevel quality, float time)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0135: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_011c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0122: Unknown result type (might be due to invalid IL or missing references)
		//IL_012c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0131: Unknown result type (might be due to invalid IL or missing references)
		//IL_0102: Unknown result type (might be due to invalid IL or missing references)
		//IL_0108: Unknown result type (might be due to invalid IL or missing references)
		//IL_0112: Unknown result type (might be due to invalid IL or missing references)
		//IL_0117: Unknown result type (might be due to invalid IL or missing references)
		for (int i = 0; i < fragment.Count; i++)
		{
			Point gridPositionOfIndex = fragment.GetGridPositionOfIndex(i);
			Vector2 canvasPositionOfIndex = fragment.GetCanvasPositionOfIndex(i);
			canvasPositionOfIndex.X -= 1.8f;
			if (canvasPositionOfIndex.X < 0f)
			{
				canvasPositionOfIndex.X *= -1f;
				gridPositionOfIndex.Y += 101;
			}
			float staticNoise = NoiseHelper.GetStaticNoise(gridPositionOfIndex.Y);
			staticNoise = (staticNoise * 5f + time * 0.4f) % 5f;
			float num = 1f;
			if (staticNoise > 1f)
			{
				num = 1f - MathHelper.Clamp((staticNoise - 0.4f - 1f) / 0.4f, 0f, 1f);
				staticNoise = 1f;
			}
			float num2 = staticNoise - canvasPositionOfIndex.X / 5f;
			Vector4 val = _backgroundColor;
			if (num2 > 0f)
			{
				float num3 = 1f;
				if (num2 < 0.2f)
				{
					num3 = num2 / 0.2f;
				}
				val = (((gridPositionOfIndex.X + 7 * gridPositionOfIndex.Y) % 5 != 0) ? Vector4.Lerp(_backgroundColor, _vineColor, num3 * num) : Vector4.Lerp(_backgroundColor, _bulbColor, num3 * num));
			}
			fragment.SetColor(i, val);
		}
	}
}
