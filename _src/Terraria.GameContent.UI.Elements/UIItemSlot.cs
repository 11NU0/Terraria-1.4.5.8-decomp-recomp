using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.UI;

namespace Terraria.GameContent.UI.Elements;

public class UIItemSlot : UIElement
{
	private Item[] _itemArray;

	private int _itemIndex;

	private int _itemSlotContext;

	public UIItemSlot(Item[] itemArray, int itemIndex, int itemSlotContext)
	{
		_itemArray = itemArray;
		_itemIndex = itemIndex;
		_itemSlotContext = itemSlotContext;
		Width = new StyleDimension(48f, 0f);
		Height = new StyleDimension(48f, 0f);
	}

	private void HandleItemSlotLogic()
	{
		if (IsMouseHovering)
		{
			Main.LocalPlayer.mouseInterface = true;
			ItemSlot.Handle(_itemArray, _itemSlotContext, _itemIndex);
		}
	}

	protected override void DrawSelf(SpriteBatch spriteBatch)
	{
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		HandleItemSlotLogic();
		Item inv = _itemArray[_itemIndex];
		Vector2 position = GetDimensions().Center() + new Vector2(52f, 52f) * -0.5f * Main.inventoryScale;
		ItemSlot.Draw(spriteBatch, ref inv, _itemSlotContext, position);
	}
}
