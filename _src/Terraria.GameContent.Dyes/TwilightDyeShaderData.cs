using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;

namespace Terraria.GameContent.Dyes;

public class TwilightDyeShaderData : ArmorShaderData
{
	public TwilightDyeShaderData(Asset<Effect> shader, string passName)
		: base(shader, passName)
	{
	}

	public override void Apply(Entity entity, DrawData? drawData)
	{
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		if (drawData.HasValue)
		{
			if (entity is Player { isDisplayDollOrInanimate: false, isHatRackDoll: false })
			{
				UseTargetPosition(Main.screenPosition + drawData.Value.position);
			}
			else if (entity is Projectile)
			{
				UseTargetPosition(Main.screenPosition + drawData.Value.position);
			}
			else
			{
				UseTargetPosition(drawData.Value.position);
			}
		}
		base.Apply(entity, drawData);
	}
}
