using Microsoft.Xna.Framework;
using Terraria.Graphics.Shaders;

namespace Terraria.GameContent.Dyes;

public class LegacyHairShaderData : HairShaderData
{
	public delegate Color ColorProcessingMethod(Player player, Color color, ref bool lighting);

	private ColorProcessingMethod _colorProcessor;

	public LegacyHairShaderData()
		: base(null, null)
	{
		_shaderDisabled = true;
	}

	public override Color GetColor(Player player, Color lightColor)
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		bool lighting = true;
		Color result = _colorProcessor(player, player.hairColor, ref lighting);
		if (lighting)
		{
			return new Color(result.ToVector4() * lightColor.ToVector4());
		}
		return result;
	}

	public LegacyHairShaderData UseLegacyMethod(ColorProcessingMethod colorProcessor)
	{
		_colorProcessor = colorProcessor;
		return this;
	}
}
