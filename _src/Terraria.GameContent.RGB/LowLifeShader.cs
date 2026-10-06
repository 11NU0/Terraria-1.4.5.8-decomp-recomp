using System;
using Microsoft.Xna.Framework;
using ReLogic.Peripherals.RGB;

namespace Terraria.GameContent.RGB;

public class LowLifeShader : ChromaShader
{
	private static Vector4 _baseColor;

	[RgbProcessor(/*Could not decode attribute arguments.*/)]
	private void ProcessAnyDetail(RgbDevice device, Fragment fragment, EffectDetailLevel quality, float time)
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		float num = (float)Math.Cos(time * (float)Math.PI) * 0.3f + 0.7f;
		Vector4 val = _baseColor * num;
		val.W = _baseColor.W;
		for (int i = 0; i < fragment.Count; i++)
		{
			fragment.SetColor(i, val);
		}
	}

	static LowLifeShader()
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		Color val = new Color(40, 0, 8, 255);
		_baseColor = val.ToVector4();
	}
}
