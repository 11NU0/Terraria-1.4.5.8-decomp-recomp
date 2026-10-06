using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.Localization;
using Terraria.UI;

namespace Terraria.Map;

public class DigtoiseMapLayer : IMapLayer
{
	public void Draw(ref MapOverlayDrawContext context, ref string text)
	{
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		Player localPlayer = Main.LocalPlayer;
		for (int i = 0; i < 1000; i++)
		{
			Projectile projectile = Main.projectile[i];
			if (projectile.active && projectile.owner == localPlayer.whoAmI && (projectile.type == 1098 || projectile.type == 1123))
			{
				Vector2 val = new Vector2(0.5f, 0f);
				Vector2 val2 = projectile.Bottom / 16f;
				if (context.Draw(TextureAssets.Extra[305].Value, val2 + val, new SpriteFrame(2, 1, (projectile.spriteDirection <= 0) ? ((byte)1) : ((byte)0), 0), Alignment.Bottom).IsMouseOver)
				{
					text = Language.GetTextValue("UI.Digtoise");
				}
				break;
			}
		}
	}
}
