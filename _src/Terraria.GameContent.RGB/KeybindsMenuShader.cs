using System;
using Microsoft.Xna.Framework;
using ReLogic.Peripherals.RGB;

namespace Terraria.GameContent.RGB;

internal class KeybindsMenuShader : ChromaShader
{
	private static Vector4 _baseColor;

	[RgbProcessor(/*Could not decode attribute arguments.*/)]
	private void ProcessHighDetail(RgbDevice device, Fragment fragment, EffectDetailLevel quality, float time)
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		float num = (float)Math.Cos(time * ((float)Math.PI / 2f)) * 0.2f + 0.8f;
		Vector4 val = _baseColor * num;
		val.W = _baseColor.W;
		for (int i = 0; i < fragment.Count; i++)
		{
			fragment.SetColor(i, val);
		}
	}

	static KeybindsMenuShader()
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		Color val = new Color(20, 20, 20, 245);
		_baseColor = val.ToVector4();
	}
}
