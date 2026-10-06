using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.Audio;
using Terraria.GameContent.UI.States;
using Terraria.Localization;
using Terraria.UI;

namespace Terraria.GameContent.UI.Elements;

public class UIWrappedSearchBar : UIElement
{
	public enum ColorTheme
	{
		Blue,
		Red
	}

	private Action _goBackFromVirtualKeyboard;

	private LocalizedText _emptyText;

	private UISearchBar _searchBar;

	private UIPanel _searchBoxPanel;

	private UIElement _searchButton;

	private UIElement _searchCancelButton;

	private string _searchString;

	public Action<UIState> CustomOpenVirtualKeyboard;

	private ColorTheme _theme;

	public bool HasContents => _searchBar.HasContents;

	public bool IsWritingText => _searchBar.IsWritingText;

	public int MaxInputLength
	{
		get
		{
			return _searchBar.MaxInputLength;
		}
		set
		{
			_searchBar.MaxInputLength = value;
		}
	}

	public event Action<string> OnSearchContentsChanged;

	public void SetContents(string contents, bool forced = false)
	{
		_searchBar.SetContents(contents, forced);
	}

	public void ToggleTakingText()
	{
		_searchBar.ToggleTakingText();
	}

	public void SetSearchSnapPoint(string name, int id, Vector2? anchor = null, Vector2? offset = null)
	{
		_searchButton.SetSnapPoint(name, id, anchor, offset);
	}

	public void SetSearchCancelSnapPoint(string name, int id, Vector2? anchor = null, Vector2? offset = null)
	{
		_searchCancelButton.SetSnapPoint(name, id, anchor, offset);
	}

	public UIWrappedSearchBar(Action goBackFromVirtualKeyboard, LocalizedText emptyText = null, ColorTheme theme = ColorTheme.Blue)
	{
		_theme = theme;
		_goBackFromVirtualKeyboard = goBackFromVirtualKeyboard;
		_emptyText = ((emptyText != null) ? emptyText : Language.GetText("UI.PlayerNameSlot"));
		Height = new StyleDimension(24f, 0f);
		Width = new StyleDimension(0f, 1f);
		SetPadding(0f);
		AddSearchBar();
		SetContents(null, forced: true);
	}

	public void HideSearchButton()
	{
		RemoveChild(_searchButton);
		_searchBoxPanel.Width = new StyleDimension(-3f, 1f);
		Recalculate();
	}

	private void AddSearchBar()
	{
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		//IL_0113: Unknown result type (might be due to invalid IL or missing references)
		//IL_0118: Unknown result type (might be due to invalid IL or missing references)
		//IL_011d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0124: Unknown result type (might be due to invalid IL or missing references)
		//IL_0129: Unknown result type (might be due to invalid IL or missing references)
		//IL_012e: Unknown result type (might be due to invalid IL or missing references)
		string text = "Images/UI/Bestiary/Button_Search";
		if (_theme == ColorTheme.Red)
		{
			text = "Images/UI/Bestiary/Button_Search_2";
		}
		UIImageButton uIImageButton = (UIImageButton)(_searchButton = new UIImageButton(Main.Assets.Request<Texture2D>(text, (AssetRequestMode)1))
		{
			VAlign = 0.5f
		});
		uIImageButton.OnLeftClick += Click_SearchArea;
		uIImageButton.SetHoverImage(Main.Assets.Request<Texture2D>("Images/UI/Bestiary/Button_Search_Border", (AssetRequestMode)1));
		uIImageButton.SetVisibility(1f, 1f);
		Append(uIImageButton);
		UIPanel uIPanel = (_searchBoxPanel = new UIPanel
		{
			Width = new StyleDimension(0f - uIImageButton.Width.Pixels - 3f, 1f),
			Height = new StyleDimension(0f, 1f),
			VAlign = 0.5f,
			HAlign = 1f
		});
		uIPanel.BackgroundColor = new Color(35, 40, 83);
		uIPanel.BorderColor = new Color(35, 40, 83);
		if (_theme == ColorTheme.Red)
		{
			uIPanel.BackgroundColor = Utils.ShiftBlueToCyanTheme(uIPanel.BackgroundColor);
			uIPanel.BorderColor = Utils.ShiftBlueToCyanTheme(uIPanel.BorderColor);
		}
		uIPanel.SetPadding(0f);
		Append(uIPanel);
		UISearchBar uISearchBar = (_searchBar = new UISearchBar(_emptyText, 0.8f)
		{
			Width = new StyleDimension(0f, 1f),
			Height = new StyleDimension(0f, 1f),
			HAlign = 0f,
			VAlign = 0.5f,
			Left = new StyleDimension(0f, 0f),
			IgnoresMouseInteraction = true
		});
		uIPanel.OnLeftClick += Click_SearchArea;
		uIPanel.OnRightClick += SearchBox_OnRightClick;
		uISearchBar.OnContentsChanged += UpdateSearchContents;
		uIPanel.Append(uISearchBar);
		uISearchBar.OnStartTakingInput += OnStartTakingInput;
		uISearchBar.OnEndTakingInput += OnEndTakingInput;
		uISearchBar.OnNeedingVirtualKeyboard += OpenVirtualKeyboardWhenNeeded;
		UIImageButton uIImageButton2 = (UIImageButton)(_searchCancelButton = new UIImageButton(Main.Assets.Request<Texture2D>("Images/UI/SearchCancel", (AssetRequestMode)1))
		{
			HAlign = 1f,
			VAlign = 0.5f,
			Left = new StyleDimension(-2f, 0f)
		});
		uIImageButton2.OnMouseOver += searchCancelButton_OnMouseOver;
		uIImageButton2.OnLeftClick += searchCancelButton_OnClick;
		uIPanel.Append(uIImageButton2);
	}

	private void searchCancelButton_OnClick(UIMouseEvent evt, UIElement listeningElement)
	{
		if (HasContents)
		{
			SetContents(null, forced: true);
			SoundEngine.PlaySound(11);
		}
		else
		{
			SoundEngine.PlaySound(12);
		}
	}

	private void searchCancelButton_OnMouseOver(UIMouseEvent evt, UIElement listeningElement)
	{
		SoundEngine.PlaySound(12);
	}

	private void OpenVirtualKeyboardWhenNeeded()
	{
		UIVirtualKeyboard uIVirtualKeyboard = new UIVirtualKeyboard(_emptyText.Value, _searchString, SubmitVirtualText, GoBackFromVirtualKeyboard, 0, allowEmpty: true, MaxInputLength);
		if (CustomOpenVirtualKeyboard != null)
		{
			CustomOpenVirtualKeyboard(uIVirtualKeyboard);
		}
		else
		{
			UserInterface.ActiveInstance.SetState(uIVirtualKeyboard);
		}
	}

	private void SubmitVirtualText(string text)
	{
		SetContents(text.Trim());
		GoBackFromVirtualKeyboard();
	}

	private void GoBackFromVirtualKeyboard()
	{
		if (_searchBar.IsWritingText)
		{
			_searchBar.ToggleTakingText();
		}
		_goBackFromVirtualKeyboard();
	}

	private void OnStartTakingInput()
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		_searchBoxPanel.BorderColor = Main.OurFavoriteColor;
	}

	private void OnEndTakingInput()
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		_searchBoxPanel.BorderColor = new Color(35, 40, 83);
		if (_theme == ColorTheme.Red)
		{
			_searchBoxPanel.BorderColor = Utils.ShiftBlueToCyanTheme(_searchBoxPanel.BorderColor);
		}
	}

	private void UpdateSearchContents(string contents)
	{
		_searchString = contents;
		if (OnSearchContentsChanged != null)
		{
			OnSearchContentsChanged(contents);
		}
	}

	private void Click_SearchArea(UIMouseEvent evt, UIElement listeningElement)
	{
		if (evt.Target.Parent != _searchBoxPanel)
		{
			ToggleTakingText();
		}
	}

	private void SearchBox_OnRightClick(UIMouseEvent evt, UIElement listeningElement)
	{
		SetContents(null, forced: true);
		if (!IsWritingText)
		{
			ToggleTakingText();
		}
	}

	public override void Update(GameTime gameTime)
	{
		base.Update(gameTime);
		if (IsWritingText && (Main.mouseLeft || Main.mouseRight) && !Elements.Any((UIElement e) => e.IsMouseHovering))
		{
			ToggleTakingText();
		}
	}
}
