using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.Chat;
using Terraria.Localization;
using Terraria.Testing;
using Terraria.UI;

namespace Terraria.GameContent.UI.Elements;

public class UIDebugLoadoutItem : UIPanel
{
	private readonly Asset<Texture2D> _dividerTexture;

	private readonly Asset<Texture2D> _innerPanelTexture;

	private readonly UIText _hoverInfoLabel;

	private ItemTooltip _preparedTooltip;

	public ScenarioTestLoadout Loadout { get; set; }

	public string Name { get; set; }

	public override int CompareTo(object obj)
	{
		return Name.CompareTo(((UIDebugLoadoutItem)obj).Name);
	}

	public UIDebugLoadoutItem(ScenarioTestLoadout loadout, string friendlyName)
	{
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		Loadout = loadout;
		Name = friendlyName;
		Height.Set(70f, 0f);
		Width.Set(0f, 1f);
		SetPadding(6f);
		BackgroundColor = new Color(76, 90, 149) * 0.2f;
		BorderColor = new Color(50, 60, 86) * 0.2f;
		_dividerTexture = Main.Assets.Request<Texture2D>("Images/UI/Divider", (AssetRequestMode)1);
		_innerPanelTexture = Main.Assets.Request<Texture2D>("Images/UI/InnerPanelBackground", (AssetRequestMode)1);
		_hoverInfoLabel = new UIText("");
		_hoverInfoLabel.VAlign = 1f;
		_hoverInfoLabel.Left.Set(80f, 0f);
		_hoverInfoLabel.Top.Set(-3f, 0f);
		Append(_hoverInfoLabel);
		Item[] array = new Item[Main.LocalPlayer.inventory.Length];
		Item[] array2 = new Item[Main.LocalPlayer.armor.Length];
		loadout.Apply(array, loadout.Items_Inventory);
		loadout.Apply(array, loadout.Items_Ammo);
		loadout.Apply(array2, loadout.Items_Armor);
		List<UIElement> list = new List<UIElement>();
		for (int i = 0; i < 5; i++)
		{
			Item item = array[i];
			if (item != null)
			{
				UIItemIcon item2 = new UIItemIcon(item, blackedOut: false);
				list.Add(item2);
			}
		}
		List<UIElement> list2 = new List<UIElement>();
		for (int j = 54; j < 58; j++)
		{
			Item item3 = array[j];
			if (item3 != null)
			{
				UIItemIcon item4 = new UIItemIcon(item3, blackedOut: false);
				list2.Add(item4);
			}
		}
		List<UIElement> list3 = new List<UIElement>();
		for (int k = 0; k < 3; k++)
		{
			Item item5 = array2[k];
			if (item5 != null)
			{
				UIItemIcon item6 = new UIItemIcon(item5, blackedOut: false);
				list3.Add(item6);
			}
		}
		List<UIElement> list4 = new List<UIElement>();
		for (int l = 3; l < 10; l++)
		{
			Item item7 = array2[l];
			if (item7 != null)
			{
				UIItemIcon item8 = new UIItemIcon(item7, blackedOut: false);
				list4.Add(item8);
			}
		}
		PositionItems(list, 0.1f, 0.3f, 0.75f);
		PositionItems(list2, 0.35f, 0.45f, 0.75f);
		PositionItems(list3, 0.65f, 0.75f, 0.25f);
		PositionItems(list4, 0.6f, 0.9f, 0.75f);
	}

	private void PositionItems(List<UIElement> items, float horizontalStart, float horizontalEnd, float vertical)
	{
		UIElement uIElement = new UIElement();
		Append(uIElement);
		uIElement.Left = StyleDimension.FromPercent(horizontalStart);
		uIElement.Width = StyleDimension.FromPercent(horizontalEnd - horizontalStart);
		uIElement.Top = StyleDimension.FromPercent(vertical);
		uIElement.SetPadding(0f);
		for (int i = 0; i < items.Count; i++)
		{
			UIElement uIElement2 = items[i];
			uIElement2.HAlign = 0.5f;
			if (items.Count > 1)
			{
				uIElement2.HAlign = (float)i / (float)((items.Count == 1) ? 1 : (items.Count - 1));
			}
			uIElement.Append(uIElement2);
		}
	}

	protected override void DrawSelf(SpriteBatch spriteBatch)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		base.DrawSelf(spriteBatch);
		CalculatedStyle innerDimensions = GetInnerDimensions();
		Vector2 val = innerDimensions.Position() - innerDimensions.Position();
		float num = 6f;
		float num2 = val.X + num;
		FontAssets.MouseText.Value.MeasureString(Name);
		Color white = Color.White;
		_ = Color.Gold;
		Utils.DrawBorderString(spriteBatch, Name, innerDimensions.Position() + new Vector2(num2 + 6f, val.Y - 2f), white, 1.1f);
	}

	public override void MouseOver(UIMouseEvent evt)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		base.MouseOver(evt);
		BackgroundColor = new Color(76, 90, 149);
		BorderColor = new Color(50, 60, 86);
		_ = _preparedTooltip;
		string item = FontAssets.ItemStack.Value.CreateWrappedText("test".Replace("\n", " "), 480f, Language.ActiveCulture.CultureInfo);
		List<string> list = new List<string> { item };
		_preparedTooltip = ItemTooltip.FromHardcodedText(list.ToArray());
	}

	public override void MouseOut(UIMouseEvent evt)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		base.MouseOut(evt);
		BackgroundColor = new Color(76, 90, 149) * 0.2f;
		BorderColor = new Color(50, 60, 86) * 0.2f;
	}

	public override void LeftClick(UIMouseEvent evt)
	{
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		IngameFancyUI.Close();
		Loadout.Apply(Main.LocalPlayer);
		Main.NewText("Loadout " + Name + " set!", ChatColors.Command);
	}
}
