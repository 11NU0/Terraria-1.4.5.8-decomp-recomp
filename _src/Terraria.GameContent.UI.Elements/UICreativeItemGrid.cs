using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent.Creative;
using Terraria.UI;
using Terraria.UI.Gamepad;

namespace Terraria.GameContent.UI.Elements;

public class UICreativeItemGrid : UIDynamicItemCollection<Item>
{
	private Item _item = new Item();

	protected override Item GetItem(Item entry)
	{
		return entry;
	}

	protected override void DrawSlot(SpriteBatch spriteBatch, Item item, Vector2 pos, bool hovering)
	{
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		ItemsSacrificedUnlocksTracker itemSacrifices = Main.LocalPlayerCreativeTracker.ItemSacrifices;
		int context = (itemSacrifices.IsFullyResearched(item.type) ? 29 : 34);
		if (hovering)
		{
			_item.SetDefaults(item.type);
			item = _item;
			Main.LocalPlayer.mouseInterface = true;
			ItemSlot.Handle(ref item, context);
			itemSacrifices.ClearNewlyResearchedStatus(item.type);
		}
		UILinkPointNavigator.Shortcuts.ItemSlotShouldHighlightAsSelected = hovering;
		ItemSlot.DrawNewAndShinyHighlightOverride = itemSacrifices.IsNewlyResearched(item.type);
		ItemSlot.Draw(spriteBatch, ref item, context, pos);
		ItemSlot.DrawNewAndShinyHighlightOverride = false;
	}
}
