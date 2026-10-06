using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.GameContent.Bestiary;
using Terraria.UI;

namespace Terraria.GameContent.UI.Elements;

public class UIBestiaryNPCEntryPortrait : UIElement
{
	public BestiaryEntry Entry { get; private set; }

	public UIBestiaryNPCEntryPortrait(BestiaryEntry entry, Asset<Texture2D> portraitBackgroundAsset, Color portraitColor, List<IBestiaryBackgroundOverlayAndColorProvider> overlays)
	{
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0190: Unknown result type (might be due to invalid IL or missing references)
		//IL_017c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0181: Unknown result type (might be due to invalid IL or missing references)
		//IL_0187: Unknown result type (might be due to invalid IL or missing references)
		//IL_0195: Unknown result type (might be due to invalid IL or missing references)
		Entry = entry;
		Height.Set(112f, 0f);
		Width.Set(193f, 0f);
		SetPadding(0f);
		UIElement uIElement = new UIElement
		{
			Width = new StyleDimension(-4f, 1f),
			Height = new StyleDimension(-4f, 1f),
			IgnoresMouseInteraction = true,
			OverflowHidden = true,
			HAlign = 0.5f,
			VAlign = 0.5f
		};
		uIElement.SetPadding(0f);
		if (portraitBackgroundAsset != null)
		{
			uIElement.Append(new UIImage(portraitBackgroundAsset)
			{
				HAlign = 0.5f,
				VAlign = 0.5f,
				ScaleToFit = true,
				Width = new StyleDimension(0f, 1f),
				Height = new StyleDimension(0f, 1f),
				Color = portraitColor
			});
		}
		for (int i = 0; i < overlays.Count; i++)
		{
			Asset<Texture2D> backgroundOverlayImage = overlays[i].GetBackgroundOverlayImage();
			Color? backgroundOverlayColor = overlays[i].GetBackgroundOverlayColor();
			uIElement.Append(new UIImage(backgroundOverlayImage)
			{
				HAlign = 0.5f,
				VAlign = 0.5f,
				ScaleToFit = true,
				Width = new StyleDimension(0f, 1f),
				Height = new StyleDimension(0f, 1f),
				Color = (backgroundOverlayColor.HasValue ? backgroundOverlayColor.Value : Color.Lerp(Color.White, portraitColor, 0.5f))
			});
		}
		UIBestiaryEntryIcon element = new UIBestiaryEntryIcon(entry, isPortrait: true);
		uIElement.Append(element);
		Append(uIElement);
		Append(new UIImage(Main.Assets.Request<Texture2D>("Images/UI/Bestiary/Portrait_Front", (AssetRequestMode)1))
		{
			VAlign = 0.5f,
			HAlign = 0.5f,
			IgnoresMouseInteraction = true
		});
	}
}
