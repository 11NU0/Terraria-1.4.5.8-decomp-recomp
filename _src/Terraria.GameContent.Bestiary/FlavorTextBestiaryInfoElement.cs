using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.GameContent.UI.Elements;
using Terraria.Localization;
using Terraria.UI;

namespace Terraria.GameContent.Bestiary;

public class FlavorTextBestiaryInfoElement : IBestiaryInfoElement
{
	private string _key;

	public FlavorTextBestiaryInfoElement(string languageKey)
	{
		_key = languageKey;
	}

	public UIElement ProvideUIElement(BestiaryUICollectionInfo info)
	{
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		if (info.UnlockState < BestiaryEntryUnlockState.CanShowStats_2)
		{
			return null;
		}
		UIPanel uIPanel = new UIPanel(Main.Assets.Request<Texture2D>("Images/UI/Bestiary/Stat_Panel", (AssetRequestMode)1), null, 12, 7)
		{
			Width = new StyleDimension(-11f, 1f),
			Height = new StyleDimension(109f, 0f),
			BackgroundColor = new Color(43, 56, 101),
			BorderColor = Color.Transparent,
			Left = new StyleDimension(3f, 0f),
			PaddingLeft = 4f,
			PaddingRight = 4f
		};
		UIText uIText = new UIText(Language.GetText(_key), 0.8f)
		{
			HAlign = 0f,
			VAlign = 0f,
			Width = StyleDimension.FromPixelsAndPercent(0f, 1f),
			Height = StyleDimension.FromPixelsAndPercent(0f, 1f),
			IsWrapped = true
		};
		AddDynamicResize(uIPanel, uIText);
		uIPanel.Append(uIText);
		return uIPanel;
	}

	private static void AddDynamicResize(UIElement container, UIText text)
	{
		text.OnInternalTextChange += () =>
		{
			container.Height = new StyleDimension(text.MinHeight.Pixels, 0f);
		};
	}
}
