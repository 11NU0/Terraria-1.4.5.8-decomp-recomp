using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.GameContent.UI.States;
using Terraria.IO;
using Terraria.Localization;
using Terraria.UI;

namespace Terraria.GameContent.UI.Elements;

public class UIWorkshopPublishWorldListItem : AWorldListItem
{
	private Asset<Texture2D> _workshopIconTexture;

	private Asset<Texture2D> _innerPanelTexture;

	private UIElement _worldIcon;

	private UIElement _publishButton;

	private int _orderInList;

	private UIState _ownerState;

	public UIWorkshopPublishWorldListItem(UIState ownerState, WorldFileData data, int orderInList)
	{
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		_ownerState = ownerState;
		_orderInList = orderInList;
		_data = data;
		LoadTextures();
		InitializeAppearance();
		_worldIcon = GetIconElement();
		_worldIcon.Left.Set(4f, 0f);
		_worldIcon.VAlign = 0.5f;
		_worldIcon.OnLeftDoubleClick += PublishButtonClick_ImportWorldToLocalFiles;
		Append(_worldIcon);
		_ = 4f;
		_publishButton = new UIIconTextButton(Language.GetText("Workshop.Publish"), Color.White, "Images/UI/Workshop/Publish");
		_publishButton.HAlign = 1f;
		_publishButton.VAlign = 1f;
		_publishButton.OnLeftClick += PublishButtonClick_ImportWorldToLocalFiles;
		OnLeftDoubleClick += PublishButtonClick_ImportWorldToLocalFiles;
		Append(_publishButton);
		_publishButton.SetSnapPoint("Publish", orderInList);
	}

	private void LoadTextures()
	{
		_innerPanelTexture = Main.Assets.Request<Texture2D>("Images/UI/InnerPanelBackground", (AssetRequestMode)1);
		_workshopIconTexture = TextureAssets.Extra[243];
	}

	private void InitializeAppearance()
	{
		Height.Set(82f, 0f);
		Width.Set(0f, 1f);
		SetPadding(6f);
		SetColorsToNotHovered();
	}

	private void SetColorsToHovered()
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		BackgroundColor = new Color(73, 94, 171);
		BorderColor = new Color(89, 116, 213);
	}

	private void SetColorsToNotHovered()
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		BackgroundColor = new Color(63, 82, 151) * 0.7f;
		BorderColor = new Color(89, 116, 213) * 0.7f;
	}

	private void PublishButtonClick_ImportWorldToLocalFiles(UIMouseEvent evt, UIElement listeningElement)
	{
		if (listeningElement == evt.Target)
		{
			Main.MenuUI.SetState(new WorkshopPublishInfoStateForWorld(_ownerState, _data));
		}
	}

	public override int CompareTo(object obj)
	{
		if (obj is UIWorkshopPublishWorldListItem uIWorkshopPublishWorldListItem)
		{
			return _orderInList.CompareTo(uIWorkshopPublishWorldListItem._orderInList);
		}
		return base.CompareTo(obj);
	}

	public override void MouseOver(UIMouseEvent evt)
	{
		base.MouseOver(evt);
		SetColorsToHovered();
	}

	public override void MouseOut(UIMouseEvent evt)
	{
		base.MouseOut(evt);
		SetColorsToNotHovered();
	}

	private void DrawPanel(SpriteBatch spriteBatch, Vector2 position, float width, float height)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		Utils.DrawSplicedPanel(spriteBatch, _innerPanelTexture.Value, (int)position.X, (int)position.Y, (int)width, (int)height, 10, 10, 10, 10, Color.White);
	}

	protected override void DrawSelf(SpriteBatch spriteBatch)
	{
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_010f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0114: Unknown result type (might be due to invalid IL or missing references)
		//IL_014c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0158: Unknown result type (might be due to invalid IL or missing references)
		//IL_015d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0162: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01da: Unknown result type (might be due to invalid IL or missing references)
		//IL_0212: Unknown result type (might be due to invalid IL or missing references)
		//IL_021e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0223: Unknown result type (might be due to invalid IL or missing references)
		//IL_0228: Unknown result type (might be due to invalid IL or missing references)
		base.DrawSelf(spriteBatch);
		CalculatedStyle innerDimensions = GetInnerDimensions();
		CalculatedStyle dimensions = _worldIcon.GetDimensions();
		float num = dimensions.X + dimensions.Width;
		Color color = (_data.IsValid ? Color.White : Color.Gray);
		string worldName = _data.GetWorldName(allowCropping: true);
		Utils.DrawBorderString(spriteBatch, worldName, new Vector2(num + 6f, innerDimensions.Y + 3f), color);
		float num2 = (innerDimensions.Width - 22f - dimensions.Width - _publishButton.GetDimensions().Width) / 2f;
		float height = _publishButton.GetDimensions().Height;
		Vector2 val = new Vector2(num + 6f, innerDimensions.Y + innerDimensions.Height - height);
		float num3 = num2;
		DrawPanel(spriteBatch, val, num3, height);
		string expertText = "";
		Color gameModeColor = Color.White;
		GetDifficulty(out expertText, out gameModeColor);
		Vector2 val2 = FontAssets.MouseText.Value.MeasureString(expertText);
		float x = val2.X;
		float y = val2.Y;
		float num4 = num3 * 0.5f - x * 0.5f;
		float num5 = height * 0.5f - y * 0.5f;
		Utils.DrawBorderString(spriteBatch, expertText, val + new Vector2(num4, num5 + 3f), gameModeColor);
		val.X += num3 + 5f;
		float num6 = num2;
		if (!GameCulture.FromCultureName(GameCulture.CultureName.English).IsActive)
		{
			num6 += 40f;
		}
		DrawPanel(spriteBatch, val, num6, height);
		string textValue = Language.GetTextValue("UI.WorldSizeFormat", _data.WorldSizeName);
		Vector2 val3 = FontAssets.MouseText.Value.MeasureString(textValue);
		float x2 = val3.X;
		float y2 = val3.Y;
		float num7 = num6 * 0.5f - x2 * 0.5f;
		float num8 = height * 0.5f - y2 * 0.5f;
		Utils.DrawBorderString(spriteBatch, textValue, val + new Vector2(num7, num8 + 3f), Color.White);
		val.X += num6 + 5f;
	}
}
