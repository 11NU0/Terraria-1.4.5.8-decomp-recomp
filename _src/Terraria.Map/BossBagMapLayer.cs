using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ID;
using Terraria.UI;

namespace Terraria.Map;

public class BossBagMapLayer : IMapLayer
{
	public void Draw(ref MapOverlayDrawContext context, ref string text)
	{
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		for (int i = 0; i < 400; i++)
		{
			WorldItem worldItem = Main.item[i];
			if (worldItem != null && worldItem.active && ItemID.Sets.BossBag[worldItem.type])
			{
				Main.instance.LoadItem(worldItem.type);
				if (Main.ItemMapIconRenderer.RequestAndTryGet(worldItem.type, out var renderTarget) && context.Draw((Texture2D)(object)renderTarget, worldItem.Center.ToTileCoordinates().ToVector2() + new Vector2(0.5f, 0.5f), Alignment.Center).IsMouseOver)
				{
					text = worldItem.Name;
				}
			}
		}
	}
}
