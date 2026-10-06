using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.Audio;
using Terraria.GameContent.UI.States;
using Terraria.ID;
using Terraria.IO;
using Terraria.Localization;
using Terraria.Social;
using Terraria.UI;

namespace Terraria.GameContent.UI.Elements;

public class UICharacterListItem : UIPanel
{
	private PlayerFileData _data;

	private Asset<Texture2D> _dividerTexture;

	private Asset<Texture2D> _innerPanelTexture;

	private UICharacter _playerPanel;

	private UIText _buttonLabel;

	private UIText _deleteButtonLabel;

	private Asset<Texture2D> _buttonCloudActiveTexture;

	private Asset<Texture2D> _buttonCloudInactiveTexture;

	private Asset<Texture2D> _buttonFavoriteActiveTexture;

	private Asset<Texture2D> _buttonFavoriteInactiveTexture;

	private Asset<Texture2D> _buttonPlayTexture;

	private Asset<Texture2D> _buttonRenameTexture;

	private Asset<Texture2D> _buttonDeleteTexture;

	private UIImageButton _deleteButton;

	private int _orderInList;

	public bool IsFavorite => _data.IsFavorite;

	public UICharacterListItem(PlayerFileData data, int orderInList)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		BorderColor = new Color(89, 116, 213) * 0.7f;
		_dividerTexture = Main.Assets.Request<Texture2D>("Images/UI/Divider", (AssetRequestMode)1);
		_innerPanelTexture = Main.Assets.Request<Texture2D>("Images/UI/InnerPanelBackground", (AssetRequestMode)1);
		_buttonCloudActiveTexture = Main.Assets.Request<Texture2D>("Images/UI/ButtonCloudActive", (AssetRequestMode)1);
		_buttonCloudInactiveTexture = Main.Assets.Request<Texture2D>("Images/UI/ButtonCloudInactive", (AssetRequestMode)1);
		_buttonFavoriteActiveTexture = Main.Assets.Request<Texture2D>("Images/UI/ButtonFavoriteActive", (AssetRequestMode)1);
		_buttonFavoriteInactiveTexture = Main.Assets.Request<Texture2D>("Images/UI/ButtonFavoriteInactive", (AssetRequestMode)1);
		_buttonPlayTexture = Main.Assets.Request<Texture2D>("Images/UI/ButtonPlay", (AssetRequestMode)1);
		_buttonRenameTexture = Main.Assets.Request<Texture2D>("Images/UI/ButtonRename", (AssetRequestMode)1);
		_buttonDeleteTexture = Main.Assets.Request<Texture2D>("Images/UI/ButtonDelete", (AssetRequestMode)1);
		Height.Set(96f, 0f);
		Width.Set(0f, 1f);
		SetPadding(6f);
		_data = data;
		_orderInList = orderInList;
		_playerPanel = new UICharacter(data.Player, animated: false, hasBackPanel: true, 1f, useAClone: true);
		_playerPanel.Left.Set(4f, 0f);
		_playerPanel.OnLeftDoubleClick += PlayGame;
		OnLeftDoubleClick += PlayGame;
		Append(_playerPanel);
		float num = 4f;
		UIImageButton uIImageButton = new UIImageButton(_buttonPlayTexture);
		uIImageButton.VAlign = 1f;
		uIImageButton.Left.Set(num, 0f);
		uIImageButton.OnLeftClick += PlayGame;
		uIImageButton.OnMouseOver += PlayMouseOver;
		uIImageButton.OnMouseOut += ButtonMouseOut;
		Append(uIImageButton);
		num += 24f;
		UIImageButton uIImageButton2 = new UIImageButton(_data.IsFavorite ? _buttonFavoriteActiveTexture : _buttonFavoriteInactiveTexture);
		uIImageButton2.VAlign = 1f;
		uIImageButton2.Left.Set(num, 0f);
		uIImageButton2.OnLeftClick += FavoriteButtonClick;
		uIImageButton2.OnMouseOver += FavoriteMouseOver;
		uIImageButton2.OnMouseOut += ButtonMouseOut;
		uIImageButton2.SetVisibility(1f, _data.IsFavorite ? 0.8f : 0.4f);
		Append(uIImageButton2);
		num += 24f;
		if (SocialAPI.Cloud != null)
		{
			UIImageButton uIImageButton3 = new UIImageButton(_data.IsCloudSave ? _buttonCloudActiveTexture : _buttonCloudInactiveTexture);
			uIImageButton3.VAlign = 1f;
			uIImageButton3.Left.Set(num, 0f);
			uIImageButton3.OnLeftClick += CloudButtonClick;
			uIImageButton3.OnMouseOver += CloudMouseOver;
			uIImageButton3.OnMouseOut += ButtonMouseOut;
			Append(uIImageButton3);
			uIImageButton3.SetSnapPoint("Cloud", orderInList);
			num += 24f;
		}
		UIImageButton uIImageButton4 = new UIImageButton(_buttonRenameTexture);
		uIImageButton4.VAlign = 1f;
		uIImageButton4.Left.Set(num, 0f);
		uIImageButton4.OnLeftClick += RenameButtonClick;
		uIImageButton4.OnMouseOver += RenameMouseOver;
		uIImageButton4.OnMouseOut += ButtonMouseOut;
		Append(uIImageButton4);
		num += 24f;
		UIImageButton uIImageButton5 = new UIImageButton(_buttonDeleteTexture)
		{
			VAlign = 1f,
			HAlign = 1f
		};
		if (!_data.IsFavorite)
		{
			uIImageButton5.OnLeftClick += DeleteButtonClick;
		}
		uIImageButton5.OnMouseOver += DeleteMouseOver;
		uIImageButton5.OnMouseOut += DeleteMouseOut;
		_deleteButton = uIImageButton5;
		Append(uIImageButton5);
		num += 4f;
		_buttonLabel = new UIText("");
		_buttonLabel.VAlign = 1f;
		_buttonLabel.Left.Set(num, 0f);
		_buttonLabel.Top.Set(-3f, 0f);
		Append(_buttonLabel);
		_deleteButtonLabel = new UIText("");
		_deleteButtonLabel.VAlign = 1f;
		_deleteButtonLabel.HAlign = 1f;
		_deleteButtonLabel.Left.Set(-30f, 0f);
		_deleteButtonLabel.Top.Set(-3f, 0f);
		Append(_deleteButtonLabel);
		uIImageButton.SetSnapPoint("Play", orderInList);
		uIImageButton2.SetSnapPoint("Favorite", orderInList);
		uIImageButton4.SetSnapPoint("Rename", orderInList);
		uIImageButton5.SetSnapPoint("Delete", orderInList);
	}

	private void RenameMouseOver(UIMouseEvent evt, UIElement listeningElement)
	{
		_buttonLabel.SetText(Language.GetTextValue("UI.Rename"));
	}

	private void FavoriteMouseOver(UIMouseEvent evt, UIElement listeningElement)
	{
		if (_data.IsFavorite)
		{
			_buttonLabel.SetText(Language.GetTextValue("UI.Unfavorite"));
		}
		else
		{
			_buttonLabel.SetText(Language.GetTextValue("UI.Favorite"));
		}
	}

	private void CloudMouseOver(UIMouseEvent evt, UIElement listeningElement)
	{
		if (_data.IsCloudSave)
		{
			_buttonLabel.SetText(Language.GetTextValue("UI.MoveOffCloud"));
		}
		else
		{
			_buttonLabel.SetText(Language.GetTextValue("UI.MoveToCloud"));
		}
	}

	private void PlayMouseOver(UIMouseEvent evt, UIElement listeningElement)
	{
		_buttonLabel.SetText(Language.GetTextValue("UI.Play"));
	}

	private void DeleteMouseOver(UIMouseEvent evt, UIElement listeningElement)
	{
		if (_data.IsFavorite)
		{
			_deleteButtonLabel.SetText(Language.GetTextValue("UI.CannotDeleteFavorited"));
		}
		else
		{
			_deleteButtonLabel.SetText(Language.GetTextValue("UI.Delete"));
		}
	}

	private void DeleteMouseOut(UIMouseEvent evt, UIElement listeningElement)
	{
		_deleteButtonLabel.SetText("");
	}

	private void ButtonMouseOut(UIMouseEvent evt, UIElement listeningElement)
	{
		_buttonLabel.SetText("");
	}

	private void RenameButtonClick(UIMouseEvent evt, UIElement listeningElement)
	{
		SoundEngine.PlaySound(10);
		Main.clrInput();
		UIVirtualKeyboard state = new UIVirtualKeyboard(Lang.menu[45].Value, _data.Name, OnFinishedSettingName, GoBackHere, 0, allowEmpty: true);
		Main.MenuUI.SetState(state);
		if (Parent.Parent is UIList uIList)
		{
			uIList.UpdateOrder();
		}
	}

	private void OnFinishedSettingName(string name)
	{
		string newName = name.Trim();
		Main.menuMode = 10;
		_data.Rename(newName);
		Main.OpenCharacterSelectUI();
	}

	private void GoBackHere()
	{
		Main.OpenCharacterSelectUI();
	}

	private void CloudButtonClick(UIMouseEvent evt, UIElement listeningElement)
	{
		if (_data.IsCloudSave)
		{
			_data.MoveToLocal();
		}
		else
		{
			_data.MoveToCloud();
		}
		((UIImageButton)evt.Target).SetImage(_data.IsCloudSave ? _buttonCloudActiveTexture : _buttonCloudInactiveTexture);
		if (_data.IsCloudSave)
		{
			_buttonLabel.SetText(Language.GetTextValue("UI.MoveOffCloud"));
		}
		else
		{
			_buttonLabel.SetText(Language.GetTextValue("UI.MoveToCloud"));
		}
	}

	private void DeleteButtonClick(UIMouseEvent evt, UIElement listeningElement)
	{
		for (int i = 0; i < Main.PlayerList.Count; i++)
		{
			if (Main.PlayerList[i] == _data)
			{
				SoundEngine.PlaySound(10);
				Main.selectedPlayer = i;
				Main.menuMode = 5;
				break;
			}
		}
	}

	private void PlayGame(UIMouseEvent evt, UIElement listeningElement)
	{
		if (listeningElement == evt.Target && _data.Player.loadStatus == StatusID.Ok)
		{
			Main.SelectPlayer(_data);
		}
	}

	private void FavoriteButtonClick(UIMouseEvent evt, UIElement listeningElement)
	{
		_data.ToggleFavorite();
		((UIImageButton)evt.Target).SetImage(_data.IsFavorite ? _buttonFavoriteActiveTexture : _buttonFavoriteInactiveTexture);
		((UIImageButton)evt.Target).SetVisibility(1f, _data.IsFavorite ? 0.8f : 0.4f);
		if (_data.IsFavorite)
		{
			_buttonLabel.SetText(Language.GetTextValue("UI.Unfavorite"));
			_deleteButton.OnLeftClick -= DeleteButtonClick;
		}
		else
		{
			_buttonLabel.SetText(Language.GetTextValue("UI.Favorite"));
			_deleteButton.OnLeftClick += DeleteButtonClick;
		}
		if (Parent.Parent is UIList uIList)
		{
			uIList.UpdateOrder();
		}
	}

	public override int CompareTo(object obj)
	{
		if (obj is UICharacterListItem uICharacterListItem)
		{
			return _orderInList.CompareTo(uICharacterListItem._orderInList);
		}
		return base.CompareTo(obj);
	}

	public override void MouseOver(UIMouseEvent evt)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		base.MouseOver(evt);
		BackgroundColor = new Color(73, 94, 171);
		BorderColor = new Color(89, 116, 213);
		_playerPanel.SetAnimated(animated: true);
	}

	public override void MouseOut(UIMouseEvent evt)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		base.MouseOut(evt);
		BackgroundColor = new Color(63, 82, 151) * 0.7f;
		BorderColor = new Color(89, 116, 213) * 0.7f;
		_playerPanel.SetAnimated(animated: false);
	}

	private void DrawPanel(SpriteBatch spriteBatch, Vector2 position, float width)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		spriteBatch.Draw(_innerPanelTexture.Value, position, (Rectangle?)new Rectangle(0, 0, 8, _innerPanelTexture.Height()), Color.White);
		spriteBatch.Draw(_innerPanelTexture.Value, new Vector2(position.X + 8f, position.Y), (Rectangle?)new Rectangle(8, 0, 8, _innerPanelTexture.Height()), Color.White, 0f, Vector2.Zero, new Vector2((width - 16f) / 8f, 1f), (SpriteEffects)0, 0f);
		spriteBatch.Draw(_innerPanelTexture.Value, new Vector2(position.X + width - 8f, position.Y), (Rectangle?)new Rectangle(16, 0, 8, _innerPanelTexture.Height()), Color.White);
	}

	protected override void DrawSelf(SpriteBatch spriteBatch)
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_0115: Unknown result type (might be due to invalid IL or missing references)
		//IL_013a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0146: Unknown result type (might be due to invalid IL or missing references)
		//IL_0148: Unknown result type (might be due to invalid IL or missing references)
		//IL_014c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0160: Unknown result type (might be due to invalid IL or missing references)
		//IL_016c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0171: Unknown result type (might be due to invalid IL or missing references)
		//IL_0176: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_020c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0218: Unknown result type (might be due to invalid IL or missing references)
		//IL_021d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0222: Unknown result type (might be due to invalid IL or missing references)
		//IL_026d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0279: Unknown result type (might be due to invalid IL or missing references)
		//IL_027e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0283: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_032d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0332: Unknown result type (might be due to invalid IL or missing references)
		//IL_0342: Unknown result type (might be due to invalid IL or missing references)
		//IL_0347: Unknown result type (might be due to invalid IL or missing references)
		//IL_0357: Unknown result type (might be due to invalid IL or missing references)
		//IL_035c: Unknown result type (might be due to invalid IL or missing references)
		//IL_035e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0374: Unknown result type (might be due to invalid IL or missing references)
		//IL_038a: Unknown result type (might be due to invalid IL or missing references)
		//IL_038f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0394: Unknown result type (might be due to invalid IL or missing references)
		//IL_0399: Unknown result type (might be due to invalid IL or missing references)
		//IL_039b: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0438: Unknown result type (might be due to invalid IL or missing references)
		//IL_044e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0464: Unknown result type (might be due to invalid IL or missing references)
		//IL_0469: Unknown result type (might be due to invalid IL or missing references)
		//IL_046e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0473: Unknown result type (might be due to invalid IL or missing references)
		//IL_0475: Unknown result type (might be due to invalid IL or missing references)
		base.DrawSelf(spriteBatch);
		CalculatedStyle innerDimensions = GetInnerDimensions();
		CalculatedStyle dimensions = _playerPanel.GetDimensions();
		float num = dimensions.X + dimensions.Width;
		Color color = Color.White;
		string text = _data.Name;
		if (_data.Player.loadStatus != StatusID.Ok)
		{
			color = Color.Gray;
			string name = StatusID.Search.GetName(_data.Player.loadStatus);
			text = "(" + name + ") " + text;
		}
		Utils.DrawBorderString(spriteBatch, text, new Vector2(num + 6f, dimensions.Y - 2f), color);
		spriteBatch.Draw(_dividerTexture.Value, new Vector2(num, innerDimensions.Y + 21f), (Rectangle?)null, Color.White, 0f, Vector2.Zero, new Vector2((GetDimensions().X + GetDimensions().Width - num) / 8f, 1f), (SpriteEffects)0, 0f);
		Vector2 val = new Vector2(num + 6f, innerDimensions.Y + 29f);
		float num2 = 200f;
		Vector2 val2 = val;
		DrawPanel(spriteBatch, val2, num2);
		spriteBatch.Draw(TextureAssets.Heart.Value, val2 + new Vector2(5f, 2f), Color.White);
		val2.X += 10f + (float)TextureAssets.Heart.Width();
		Utils.DrawBorderString(spriteBatch, _data.Player.statLifeMax + Language.GetTextValue("GameUI.PlayerLifeMax"), val2 + new Vector2(0f, 3f), Color.White);
		val2.X += 65f;
		spriteBatch.Draw(TextureAssets.Mana.Value, val2 + new Vector2(5f, 2f), Color.White);
		val2.X += 10f + (float)TextureAssets.Mana.Width();
		Utils.DrawBorderString(spriteBatch, _data.Player.statManaMax + Language.GetTextValue("GameUI.PlayerManaMax"), val2 + new Vector2(0f, 3f), Color.White);
		val.X += num2 + 5f;
		Vector2 val3 = val;
		float num3 = 140f;
		if (GameCulture.FromCultureName(GameCulture.CultureName.Russian).IsActive)
		{
			num3 = 180f;
		}
		DrawPanel(spriteBatch, val3, num3);
		string text2 = "";
		Color color2 = Color.White;
		switch (_data.Player.difficulty)
		{
		case 0:
			text2 = Language.GetTextValue("UI.Softcore");
			break;
		case 1:
			text2 = Language.GetTextValue("UI.Mediumcore");
			color2 = Main.mcColor;
			break;
		case 2:
			text2 = Language.GetTextValue("UI.Hardcore");
			color2 = Main.hcColor;
			break;
		case 3:
			text2 = Language.GetTextValue("UI.Creative");
			color2 = Main.creativeModeColor;
			break;
		}
		val3 += new Vector2(num3 * 0.5f - FontAssets.MouseText.Value.MeasureString(text2).X * 0.5f, 3f);
		Utils.DrawBorderString(spriteBatch, text2, val3, color2);
		val.X += num3 + 5f;
		Vector2 val4 = val;
		float num4 = innerDimensions.X + innerDimensions.Width - val4.X;
		DrawPanel(spriteBatch, val4, num4);
		TimeSpan playTime = _data.GetPlayTime();
		int num5 = playTime.Days * 24 + playTime.Hours;
		string text3 = ((num5 < 10) ? "0" : "") + num5 + playTime.ToString("\\:mm\\:ss");
		val4 += new Vector2(num4 * 0.5f - FontAssets.MouseText.Value.MeasureString(text3).X * 0.5f, 3f);
		Utils.DrawBorderString(spriteBatch, text3, val4, Color.White);
	}
}
