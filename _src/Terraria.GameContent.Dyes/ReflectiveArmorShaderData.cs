using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;

namespace Terraria.GameContent.Dyes;

public class ReflectiveArmorShaderData : ArmorShaderData
{
	private Effect _effect;

	private EffectParameter<Vector3> uLightSource;

	public ReflectiveArmorShaderData(Asset<Effect> shader, string passName)
		: base(shader, passName)
	{
	}

	private void CheckCachedParameters()
	{
		if (_effect == null || _effect != Shader)
		{
			_effect = Shader;
			uLightSource = Shader.GetParameter<Vector3>("uLightSource");
		}
	}

	public override void Apply(Entity entity, DrawData? drawData)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0101: Unknown result type (might be due to invalid IL or missing references)
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		//IL_0113: Unknown result type (might be due to invalid IL or missing references)
		//IL_011a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0122: Unknown result type (might be due to invalid IL or missing references)
		//IL_012c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0133: Unknown result type (might be due to invalid IL or missing references)
		//IL_013b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0151: Unknown result type (might be due to invalid IL or missing references)
		//IL_016f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0173: Unknown result type (might be due to invalid IL or missing references)
		//IL_0178: Unknown result type (might be due to invalid IL or missing references)
		//IL_0193: Unknown result type (might be due to invalid IL or missing references)
		//IL_019a: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0224: Unknown result type (might be due to invalid IL or missing references)
		CheckCachedParameters();
		if (entity == null)
		{
			uLightSource.SetValue(Vector3.Zero);
		}
		else
		{
			float num = 0f;
			if (drawData.HasValue)
			{
				num = drawData.Value.rotation;
			}
			Vector2 position = entity.position;
			float num2 = entity.width;
			float num3 = entity.height;
			Vector2 val = position + new Vector2(num2, num3) * 0.1f;
			num2 *= 0.8f;
			num3 *= 0.8f;
			Vector3 subLight = Lighting.GetSubLight(val + new Vector2(num2 * 0.5f, 0f));
			Vector3 subLight2 = Lighting.GetSubLight(val + new Vector2(0f, num3 * 0.5f));
			Vector3 subLight3 = Lighting.GetSubLight(val + new Vector2(num2, num3 * 0.5f));
			Vector3 subLight4 = Lighting.GetSubLight(val + new Vector2(num2 * 0.5f, num3));
			float num4 = subLight.X + subLight.Y + subLight.Z;
			float num5 = subLight2.X + subLight2.Y + subLight2.Z;
			float num6 = subLight3.X + subLight3.Y + subLight3.Z;
			float num7 = subLight4.X + subLight4.Y + subLight4.Z;
			Vector2 val2 = new Vector2(num6 - num5, num7 - num4);
			float num8 = val2.Length();
			if (num8 > 1f)
			{
				num8 = 1f;
				val2 /= num8;
			}
			if (entity.direction == -1)
			{
				val2.X *= -1f;
			}
			val2 = val2.RotatedBy(0f - num);
			Vector3 value = new Vector3(val2, 1f - (val2.X * val2.X + val2.Y * val2.Y));
			value.X *= 2f;
			value.Y -= 0.15f;
			value.Y *= 2f;
			value.Normalize();
			value.Z *= 0.6f;
			uLightSource.SetValue(value);
		}
		base.Apply(entity, drawData);
	}
}
