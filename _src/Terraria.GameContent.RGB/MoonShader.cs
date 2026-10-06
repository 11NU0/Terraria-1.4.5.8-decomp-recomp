using System;
using Microsoft.Xna.Framework;
using ReLogic.Peripherals.RGB;

namespace Terraria.GameContent.RGB;

public class MoonShader : ChromaShader
{
	private readonly Vector4 _moonCoreColor;

	private readonly Vector4 _moonRingColor;

	private readonly Vector4 _skyColor;

	private readonly Vector4 _cloudColor;

	private float _progress;

	public MoonShader(Color skyColor, Color moonRingColor, Color moonCoreColor)
		: this(skyColor, moonRingColor, moonCoreColor, Color.White)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		//IL_0004: Unknown result type (might be due to invalid IL or missing references)
	}

	public MoonShader(Color skyColor, Color moonColor)
		: this(skyColor, moonColor, moonColor)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
	}

	public MoonShader(Color skyColor, Color moonRingColor, Color moonCoreColor, Color cloudColor)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		_skyColor = skyColor.ToVector4();
		_moonRingColor = moonRingColor.ToVector4();
		_moonCoreColor = moonCoreColor.ToVector4();
		_cloudColor = cloudColor.ToVector4();
	}

	public override void Update(float elapsedTime)
	{
		if (Main.dayTime)
		{
			_progress = (float)(Main.time / 54000.0);
		}
		else
		{
			_progress = (float)(Main.time / 32400.0);
		}
	}

	[RgbProcessor(/*Could not decode attribute arguments.*/)]
	private void ProcessLowDetail(RgbDevice device, Fragment fragment, EffectDetailLevel quality, float time)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		for (int i = 0; i < fragment.Count; i++)
		{
			float dynamicNoise = NoiseHelper.GetDynamicNoise(fragment.GetCanvasPositionOfIndex(i) * new Vector2(0.1f, 0.5f) + new Vector2(time * 0.02f, 0f), time / 40f);
			dynamicNoise = (float)Math.Sqrt(Math.Max(0f, 1f - 2f * dynamicNoise));
			Vector4 val = Vector4.Lerp(_skyColor, _cloudColor, dynamicNoise * 0.1f);
			fragment.SetColor(i, val);
		}
	}

	[RgbProcessor(/*Could not decode attribute arguments.*/)]
	private void ProcessHighDetail(RgbDevice device, Fragment fragment, EffectDetailLevel quality, float time)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Invalid comparison between Unknown and I4
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0105: Unknown result type (might be due to invalid IL or missing references)
		//IL_010a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0116: Unknown result type (might be due to invalid IL or missing references)
		//IL_011c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0139: Unknown result type (might be due to invalid IL or missing references)
		//IL_013e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0175: Unknown result type (might be due to invalid IL or missing references)
		//IL_014b: Unknown result type (might be due to invalid IL or missing references)
		//IL_014e: Unknown result type (might be due to invalid IL or missing references)
		//IL_016b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0170: Unknown result type (might be due to invalid IL or missing references)
		if ((int)device.Type != 0 && (int)device.Type != 6)
		{
			ProcessLowDetail(device, fragment, quality, time);
			return;
		}
		Vector2 val = new Vector2(2f, 0.5f);
		Vector2 val2 = new Vector2(2.5f, 1f);
		float num = _progress * (float)Math.PI + (float)Math.PI;
		Vector2 val3 = new Vector2((float)Math.Cos(num), (float)Math.Sin(num)) * val2 + val;
		for (int i = 0; i < fragment.Count; i++)
		{
			Vector2 canvasPositionOfIndex = fragment.GetCanvasPositionOfIndex(i);
			float dynamicNoise = NoiseHelper.GetDynamicNoise(canvasPositionOfIndex * new Vector2(0.1f, 0.5f) + new Vector2(time * 0.02f, 0f), time / 40f);
			dynamicNoise = (float)Math.Sqrt(Math.Max(0f, 1f - 2f * dynamicNoise));
			Vector2 val4 = canvasPositionOfIndex - val3;
			float num2 = val4.Length();
			Vector4 val5 = Vector4.Lerp(_skyColor, _cloudColor, dynamicNoise * 0.15f);
			if (num2 < 0.8f)
			{
				val5 = Vector4.Lerp(_moonRingColor, _moonCoreColor, Math.Min(0.1f, 0.8f - num2) / 0.1f);
			}
			else if (num2 < 1f)
			{
				val5 = Vector4.Lerp(val5, _moonRingColor, Math.Min(0.2f, 1f - num2) / 0.2f);
			}
			fragment.SetColor(i, val5);
		}
	}
}
