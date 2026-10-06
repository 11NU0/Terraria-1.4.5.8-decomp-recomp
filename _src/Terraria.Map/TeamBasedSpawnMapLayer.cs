using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.Localization;
using Terraria.UI;

namespace Terraria.Map;

public class TeamBasedSpawnMapLayer : IMapLayer
{
	public void Draw(ref MapOverlayDrawContext context, ref string text)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		if (Main.teamBasedSpawnsSeed)
		{
			int team = Main.LocalPlayer.team;
			Point spawnPoint = Point.Zero;
			if (ExtraSpawnPointManager.TryGetExtraSpawnPointForTeam(team, out spawnPoint) && context.Draw(TextureAssets.Extra[282].Value, spawnPoint.ToVector2(), new SpriteFrame(6, 1, (byte)team, 0), Alignment.Bottom).IsMouseOver)
			{
				text = Language.GetTextValue("UI.TeamSpawnPoint");
			}
		}
	}
}
