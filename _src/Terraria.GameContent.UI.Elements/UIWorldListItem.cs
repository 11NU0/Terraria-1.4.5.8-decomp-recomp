using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using ReLogic.OS;
using Terraria.Audio;
using Terraria.GameContent.UI.States;
using Terraria.ID;
using Terraria.IO;
using Terraria.Localization;
using Terraria.Social;
using Terraria.UI;

namespace Terraria.GameContent.UI.Elements;

public class UIWorldListItem : AWorldListItem
{
	private Asset<Texture2D> _dividerTexture;

	private Asset<Texture2D> _innerPanelTexture;

	private UIElement _worldIcon;

	private UIText _buttonLabel;

	private UIText _deleteButtonLabel;

	private Asset<Texture2D> _buttonCloudActiveTexture;

	private Asset<Texture2D> _buttonCloudInactiveTexture;

	private Asset<Texture2D> _buttonFavoriteActiveTexture;

	private Asset<Texture2D> _buttonFavoriteInactiveTexture;

	private Asset<Texture2D> _buttonPlayTexture;

	private Asset<Texture2D> _buttonSeedTexture;

	private Asset<Texture2D> _buttonRenameTexture;

	private Asset<Texture2D> _buttonDeleteTexture;

	private Asset<Texture2D> _hasBeenPlayedByActivePlayerTexture;

	private Asset<Texture2D> _isNewlyGeneratedTexture;

	private UIImageButton _deleteButton;

	private int _orderInList;

	private bool _canBePlayed;

	private bool _hasBeenPlayedByActivePlayer;

	private bool _isNewlyGenerated;

	public bool IsFavorite => _data.IsFavorite;

	public UIWorldListItem(WorldFileData data, int orderInList, bool canBePlayed, bool hasBeenPlayedByActivePlayer, bool isNewlyGenerated)
	{
		_orderInList = orderInList;
		_data = data;
		_canBePlayed = canBePlayed;
		_hasBeenPlayedByActivePlayer = hasBeenPlayedByActivePlayer;
		_isNewlyGenerated = isNewlyGenerated;
		LoadTextures();
		InitializeAppearance();
		_worldIcon = GetIconElement();
		_worldIcon.OnLeftDoubleClick += PlayGame;
		Append(_worldIcon);
		if (_data.DefeatedMoonlord)
		{
			UIImage element = new UIImage(Main.Assets.Request<Texture2D>("Images/UI/IconCompletion", (AssetRequestMode)1))
			{
				HAlign = 0.5f,
				VAlign = 0.5f,
				Top = new StyleDimension(-10f, 0f),
				Left = new StyleDimension(-3f, 0f),
				IgnoresMouseInteraction = true
			};
			_worldIcon.Append(element);
		}
		if (GetIcons().Count >= 2 && !_data.ZenithWorld)
		{
			UIImage element2 = new UIImage(Main.Assets.Request<Texture2D>("Images/UI/IconMixedSeed", (AssetRequestMode)1))
			{
				HAlign = 0f,
				VAlign = 1f,
				Top = StyleDimension.FromPixels(0f),
				Left = StyleDimension.FromPixels(0f)
			};
			_worldIcon.Append(element2);
		}
		float num = 4f;
		UIImageButton uIImageButton = new UIImageButton(_buttonPlayTexture);
		uIImageButton.VAlign = 1f;
		uIImageButton.Left.Set(num, 0f);
		uIImageButton.OnLeftClick += PlayGame;
		OnLeftDoubleClick += PlayGame;
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
			uIImageButton3.SetSnapPoint("Cloud", orderInList);
			Append(uIImageButton3);
			num += 24f;
		}
		if (_data.WorldGeneratorVersion != 0L)
		{
			UIImageButton uIImageButton4 = new UIImageButton(_buttonSeedTexture);
			uIImageButton4.VAlign = 1f;
			uIImageButton4.Left.Set(num, 0f);
			uIImageButton4.OnLeftClick += SeedButtonClick;
			uIImageButton4.OnMouseOver += SeedMouseOver;
			uIImageButton4.OnMouseOut += ButtonMouseOut;
			uIImageButton4.SetSnapPoint("Seed", orderInList);
			Append(uIImageButton4);
			num += 24f;
		}
		UIImageButton uIImageButton5 = new UIImageButton(_buttonRenameTexture);
		uIImageButton5.VAlign = 1f;
		uIImageButton5.Left.Set(num, 0f);
		uIImageButton5.OnLeftClick += RenameButtonClick;
		uIImageButton5.OnMouseOver += RenameMouseOver;
		uIImageButton5.OnMouseOut += ButtonMouseOut;
		uIImageButton5.SetSnapPoint("Rename", orderInList);
		Append(uIImageButton5);
		num += 24f;
		UIImageButton uIImageButton6 = new UIImageButton(_buttonDeleteTexture)
		{
			VAlign = 1f,
			HAlign = 1f
		};
		if (!_data.IsFavorite)
		{
			uIImageButton6.OnLeftClick += DeleteButtonClick;
		}
		uIImageButton6.OnMouseOver += DeleteMouseOver;
		uIImageButton6.OnMouseOut += DeleteMouseOut;
		_deleteButton = uIImageButton6;
		Append(uIImageButton6);
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
		int num2 = 0;
		if (_hasBeenPlayedByActivePlayer)
		{
			UIImage uIImage = new UIImage(_hasBeenPlayedByActivePlayerTexture)
			{
				HAlign = 1f,
				Left = new StyleDimension(num2, 0f),
				Top = new StyleDimension(-6f, 0f),
				ImageScale = 0.75f,
				UseTextureSizeForOrigin = false
			};
			uIImage.OnMouseOver += HasPlayedMouseOver;
			uIImage.OnMouseOut += DeleteMouseOut;
			Append(uIImage);
			num2 -= 24;
		}
		if (_isNewlyGenerated)
		{
			UIImage uIImage2 = new UIImage(_isNewlyGeneratedTexture)
			{
				HAlign = 1f,
				Left = new StyleDimension(num2 - 2, 0f),
				Top = new StyleDimension(-6f, 0f),
				ImageScale = 0.75f,
				UseTextureSizeForOrigin = false
			};
			uIImage2.OnMouseOver += NewlyGeneratedMouseOver;
			uIImage2.OnMouseOut += DeleteMouseOut;
			Append(uIImage2);
			num2 -= 24;
		}
		uIImageButton.SetSnapPoint("Play", orderInList);
		uIImageButton2.SetSnapPoint("Favorite", orderInList);
		uIImageButton5.SetSnapPoint("Rename", orderInList);
		uIImageButton6.SetSnapPoint("Delete", orderInList);
	}

	private void LoadTextures()
	{
		_dividerTexture = Main.Assets.Request<Texture2D>("Images/UI/Divider", (AssetRequestMode)1);
		_innerPanelTexture = Main.Assets.Request<Texture2D>("Images/UI/InnerPanelBackground", (AssetRequestMode)1);
		_buttonCloudActiveTexture = Main.Assets.Request<Texture2D>("Images/UI/ButtonCloudActive", (AssetRequestMode)1);
		_buttonCloudInactiveTexture = Main.Assets.Request<Texture2D>("Images/UI/ButtonCloudInactive", (AssetRequestMode)1);
		_buttonFavoriteActiveTexture = Main.Assets.Request<Texture2D>("Images/UI/ButtonFavoriteActive", (AssetRequestMode)1);
		_buttonFavoriteInactiveTexture = Main.Assets.Request<Texture2D>("Images/UI/ButtonFavoriteInactive", (AssetRequestMode)1);
		_buttonPlayTexture = Main.Assets.Request<Texture2D>("Images/UI/ButtonPlay", (AssetRequestMode)1);
		_buttonSeedTexture = Main.Assets.Request<Texture2D>("Images/UI/ButtonSeed", (AssetRequestMode)1);
		_buttonRenameTexture = Main.Assets.Request<Texture2D>("Images/UI/ButtonRename", (AssetRequestMode)1);
		_buttonDeleteTexture = Main.Assets.Request<Texture2D>("Images/UI/ButtonDelete", (AssetRequestMode)1);
		_hasBeenPlayedByActivePlayerTexture = Main.Assets.Request<Texture2D>("Images/UI/IconPlayedBefore", (AssetRequestMode)1);
		_isNewlyGeneratedTexture = Main.Assets.Request<Texture2D>("Images/UI/IconNewlyGenerated", (AssetRequestMode)1);
	}

	private void InitializeAppearance()
	{
		Height.Set(96f, 0f);
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
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		BackgroundColor = new Color(73, 94, 171);
		BorderColor = new Color(89, 116, 213);
		if (!_canBePlayed)
		{
			BorderColor = new Color(150, 150, 150) * 1f;
			BackgroundColor = Color.Lerp(BackgroundColor, new Color(120, 120, 120), 0.5f) * 1f;
		}
	}

	private void SetColorsToNotHovered()
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		BackgroundColor = new Color(63, 82, 151) * 0.7f;
		BorderColor = new Color(89, 116, 213) * 0.7f;
		if (!_canBePlayed)
		{
			BorderColor = new Color(127, 127, 127) * 0.7f;
			BackgroundColor = Color.Lerp(new Color(63, 82, 151), new Color(80, 80, 80), 0.5f) * 0.7f;
		}
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

	private void SeedMouseOver(UIMouseEvent evt, UIElement listeningElement)
	{
		_buttonLabel.SetText(Language.GetTextValue("UI.CopySeed", _data.GetFullSeedText(allowCropping: true)));
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

	private void NewlyGeneratedMouseOver(UIMouseEvent evt, UIElement listeningElement)
	{
		_deleteButtonLabel.SetText(Language.GetTextValue("UI.WorldNewlyGenerated"));
	}

	private void HasPlayedMouseOver(UIMouseEvent evt, UIElement listeningElement)
	{
		_deleteButtonLabel.SetText(Language.GetTextValue("UI.WorldHasBeenPlayed"));
	}

	private void ButtonMouseOut(UIMouseEvent evt, UIElement listeningElement)
	{
		_buttonLabel.SetText("");
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
		for (int i = 0; i < Main.WorldList.Count; i++)
		{
			if (Main.WorldList[i] == _data)
			{
				SoundEngine.PlaySound(10);
				Main.selectedWorld = i;
				Main.menuMode = 9;
				break;
			}
		}
	}

	private void PlayGame(UIMouseEvent evt, UIElement listeningElement)
	{
		if (listeningElement == evt.Target && _data.IsValid && !TryMovingToRejectionMenuIfNeeded(_data.GameMode))
		{
			_data.SetAsActive();
			SoundEngine.PlaySound(10);
			Main.clrInput();
			Main.GetInputText("");
			if (Main.menuMultiplayer && SocialAPI.Network != null)
			{
				Main.menuMode = 889;
			}
			else if (Main.menuMultiplayer)
			{
				Main.menuMode = 30;
			}
			else
			{
				Main.menuMode = 10;
			}
			if (!Main.menuMultiplayer)
			{
				WorldGen.playWorld();
			}
		}
	}

	private bool TryMovingToRejectionMenuIfNeeded(int worldGameMode)
	{
		if (!GameModeID.IsValid(worldGameMode))
		{
			SoundEngine.PlaySound(10);
			Main.statusText = Language.GetTextValue("UI.WorldCannotBeLoadedBecauseItHasAnInvalidGameMode");
			Main.menuMode = 1000000;
			return true;
		}
		bool flag = Main.ActivePlayerFileData.Player.difficulty == 3;
		bool flag2 = worldGameMode == 3;
		if (flag && !flag2)
		{
			SoundEngine.PlaySound(10);
			Main.statusText = Language.GetTextValue("UI.PlayerIsCreativeAndWorldIsNotCreative");
			Main.menuMode = 1000000;
			return true;
		}
		if (!flag & flag2)
		{
			SoundEngine.PlaySound(10);
			Main.statusText = Language.GetTextValue("UI.PlayerIsNotCreativeAndWorldIsCreative");
			Main.menuMode = 1000000;
			return true;
		}
		return false;
	}

	private void RenameButtonClick(UIMouseEvent evt, UIElement listeningElement)
	{
		SoundEngine.PlaySound(10);
		Main.clrInput();
		UIVirtualKeyboard state = new UIVirtualKeyboard(Lang.menu[48].Value, _data.GetWorldName(), OnFinishedSettingName, GoBackHere, 0, allowEmpty: true, 27);
		Main.MenuUI.SetState(state);
		if (Parent.Parent is UIList uIList)
		{
			uIList.UpdateOrder();
		}
	}

	private void OnFinishedSettingName(string name)
	{
		string newDisplayName = name.Trim();
		Main.menuMode = 10;
		_data.Rename(newDisplayName);
	}

	private void GoBackHere()
	{
		Main.GoToWorldSelect();
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

	private void SeedButtonClick(UIMouseEvent evt, UIElement listeningElement)
	{
		Platform.Get<IClipboard>().Value = _data.GetFullSeedText();
		_buttonLabel.SetText(Language.GetTextValue("UI.SeedCopied"));
	}

	public override int CompareTo(object obj)
	{
		if (obj is UIWorldListItem uIWorldListItem)
		{
			return _orderInList.CompareTo(uIWorldListItem._orderInList);
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
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Unknown result type (might be due to invalid IL or missing references)
		//IL_0107: Unknown result type (might be due to invalid IL or missing references)
		//IL_012c: Unknown result type (might be due to invalid IL or missing references)
		//IL_013a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0159: Unknown result type (might be due to invalid IL or missing references)
		//IL_017b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0184: Unknown result type (might be due to invalid IL or missing references)
		//IL_0189: Unknown result type (might be due to invalid IL or missing references)
		//IL_018e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0204: Unknown result type (might be due to invalid IL or missing references)
		//IL_0226: Unknown result type (might be due to invalid IL or missing references)
		//IL_022f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0234: Unknown result type (might be due to invalid IL or missing references)
		//IL_0239: Unknown result type (might be due to invalid IL or missing references)
		//IL_0273: Unknown result type (might be due to invalid IL or missing references)
		//IL_027f: Unknown result type (might be due to invalid IL or missing references)
		//IL_02da: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0305: Unknown result type (might be due to invalid IL or missing references)
		//IL_030a: Unknown result type (might be due to invalid IL or missing references)
		//IL_030f: Unknown result type (might be due to invalid IL or missing references)
		base.DrawSelf(spriteBatch);
		CalculatedStyle innerDimensions = GetInnerDimensions();
		CalculatedStyle dimensions = _worldIcon.GetDimensions();
		float num = dimensions.X + dimensions.Width;
		Color color = Color.White;
		string text = _data.GetWorldName(allowCropping: true);
		if (!_data.IsValid)
		{
			color = Color.Gray;
			string name = StatusID.Search.GetName(_data.LoadStatus);
			text = "(" + name + ") " + text;
		}
		Utils.DrawBorderString(spriteBatch, text, new Vector2(num + 6f, dimensions.Y - 2f), color);
		spriteBatch.Draw(_dividerTexture.Value, new Vector2(num, innerDimensions.Y + 21f), (Rectangle?)null, Color.White, 0f, Vector2.Zero, new Vector2((GetDimensions().X + GetDimensions().Width - num) / 8f, 1f), (SpriteEffects)0, 0f);
		Vector2 val = new Vector2(num + 6f, innerDimensions.Y + 29f);
		float num2 = 100f;
		DrawPanel(spriteBatch, val, num2);
		GetDifficulty(out var expertText, out var gameModeColor);
		float x = FontAssets.MouseText.Value.MeasureString(expertText).X;
		float num3 = num2 * 0.5f - x * 0.5f;
		Utils.DrawBorderString(spriteBatch, expertText, val + new Vector2(num3, 3f), gameModeColor);
		val.X += num2 + 5f;
		float num4 = 150f;
		if (!GameCulture.FromCultureName(GameCulture.CultureName.English).IsActive)
		{
			num4 += 40f;
		}
		DrawPanel(spriteBatch, val, num4);
		string textValue = Language.GetTextValue("UI.WorldSizeFormat", _data.WorldSizeName);
		float x2 = FontAssets.MouseText.Value.MeasureString(textValue).X;
		float num5 = num4 * 0.5f - x2 * 0.5f;
		Utils.DrawBorderString(spriteBatch, textValue, val + new Vector2(num5, 3f), Color.White);
		val.X += num4 + 5f;
		float num6 = innerDimensions.X + innerDimensions.Width - val.X;
		DrawPanel(spriteBatch, val, num6);
		string arg = ((!GameCulture.FromCultureName(GameCulture.CultureName.English).IsActive) ? _data.CreationTime.ToShortDateString() : _data.CreationTime.ToString("d MMMM yyyy"));
		string textValue2 = Language.GetTextValue("UI.WorldCreatedFormat", arg);
		float x3 = FontAssets.MouseText.Value.MeasureString(textValue2).X;
		float num7 = num6 * 0.5f - x3 * 0.5f;
		Utils.DrawBorderString(spriteBatch, textValue2, val + new Vector2(num7, 3f), Color.White);
		val.X += num6 + 5f;
	}
}
