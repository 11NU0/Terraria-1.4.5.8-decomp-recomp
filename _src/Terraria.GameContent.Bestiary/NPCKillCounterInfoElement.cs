using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.DataStructures;
using Terraria.GameContent.UI.Elements;
using Terraria.Localization;
using Terraria.UI;

namespace Terraria.GameContent.Bestiary;

public class NPCKillCounterInfoElement : IBestiaryInfoElement
{
	private NPC _instance;

	public NPCKillCounterInfoElement(int npcNetId)
	{
		_instance = new NPC();
		_instance.SetDefaults(npcNetId, new NPCSpawnParams
		{
			difficultyOverride = GameDifficultyLevel.Classic
		});
	}

	public UIElement ProvideUIElement(BestiaryUICollectionInfo info)
	{
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		//IL_0108: Unknown result type (might be due to invalid IL or missing references)
		int? killCount = GetKillCount();
		if (!killCount.HasValue || killCount.Value < 1)
		{
			return null;
		}
		UIElement uIElement = new UIElement
		{
			Width = new StyleDimension(0f, 1f),
			Height = new StyleDimension(109f, 0f)
		};
		if (killCount.HasValue)
		{
			_ = killCount.Value > 0;
		}
		else
			_ = 0;
		_ = 3;
		int num = 0;
		int num2 = 30;
		int num3 = num2;
		string text = killCount.Value.ToString();
		_ = text.Length;
		int num4 = Math.Max(0, -48 + 8 * text.Length);
		float num5 = 0.5f;
		_ = 0;
		num4 = -3;
		num5 = 1f;
		int num6 = 8;
		UIElement uIElement2 = new UIPanel(Main.Assets.Request<Texture2D>("Images/UI/Bestiary/Stat_Panel", (AssetRequestMode)1), null, 12, 7)
		{
			Width = new StyleDimension(-8 + num4, num5),
			Height = new StyleDimension(num2, 0f),
			BackgroundColor = new Color(43, 56, 101),
			BorderColor = Color.Transparent,
			Top = new StyleDimension(num, 0f),
			Left = new StyleDimension(-num6, 0f),
			HAlign = 1f
		};
		uIElement2.SetPadding(0f);
		uIElement2.PaddingRight = 5f;
		uIElement.Append(uIElement2);
		uIElement2.OnUpdate += ShowDescription;
		float textScale = 0.85f;
		UIText element = new UIText(text, textScale)
		{
			HAlign = 1f,
			VAlign = 0.5f,
			Left = new StyleDimension(-3f, 0f),
			Top = new StyleDimension(0f, 0f)
		};
		Item item = new Item();
		item.SetDefaults(321);
		item.scale = 0.8f;
		UIItemIcon element2 = new UIItemIcon(item, blackedOut: false)
		{
			IgnoresMouseInteraction = true,
			HAlign = 0f,
			Left = new StyleDimension(-1f, 0f)
		};
		uIElement.Height.Pixels = num3;
		uIElement2.Append(element2);
		uIElement2.Append(element);
		return uIElement;
	}

	private void ShowDescription(UIElement element)
	{
		if (element.IsMouseHovering)
		{
			Main.instance.MouseText(Language.GetTextValue("BestiaryInfo.Slain"), 0, 0);
		}
	}

	private int? GetKillCount()
	{
		return Main.BestiaryTracker.Kills.GetKillCount(_instance);
	}
}
