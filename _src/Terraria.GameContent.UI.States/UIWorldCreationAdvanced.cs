using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using ReLogic.Graphics;
using Terraria.Audio;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.Localization;
using Terraria.UI;
using Terraria.UI.Gamepad;
using Terraria.WorldBuilding;

namespace Terraria.GameContent.UI.States;

public class UIWorldCreationAdvanced : UIState, IHaveBackButtonCommand
{
	private struct WorldSpecialSeedOption
	{
		public AWorldGenerationOption Seed;

		public UIElement Element;

		public LocalizedText Description;

		public LocalizedText Title;
	}

	private UIWorldCreation _creationState;

	private UIText _descriptionText;

	private UIText _titleText;

	private UICharacterNameButton _seedPlate;

	private UIElement _backButton;

	private UIElement _optionList;

	private UIElement _randomButton;

	private GroupOptionButton<AWorldGenerationOption>[] _seedButtons;

	private UIElement _seedButtonRegion;

	private GroupOptionButton<bool> _secretSeedButton;

	private bool _allowScrolling;

	private UIGamepadHelper _helper;

	public UIWorldCreationAdvanced(UIWorldCreation state, bool allowScrolling = false)
	{
		_creationState = state;
		_creationState.SubmitSeed = UpdateContents;
		_allowScrolling = allowScrolling;
		BuildPage();
		Prepare();
	}

	private void Prepare()
	{
		UpdateContents();
	}

	private void UpdateContents()
	{
		_creationState.FillSeedContent(_seedPlate);
		GroupOptionButton<AWorldGenerationOption>[] seedButtons = _seedButtons;
		foreach (GroupOptionButton<AWorldGenerationOption> groupOptionButton in seedButtons)
		{
			groupOptionButton.SetCurrentOption(groupOptionButton.OptionValue.Enabled ? groupOptionButton.OptionValue : null);
		}
	}

	private void BuildPage()
	{
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		RemoveAllChildren();
		UIElement uIElement = new UIElement
		{
			Width = StyleDimension.FromPixels(500f),
			Height = StyleDimension.FromPixelsAndPercent(-200f, 1f),
			Top = StyleDimension.FromPixels(202f),
			HAlign = 0.5f,
			VAlign = 0f
		};
		if (!_allowScrolling)
		{
			uIElement.MaxHeight = StyleDimension.FromPixels(400f);
		}
		uIElement.SetPadding(0f);
		Append(uIElement);
		UIPanel uIPanel = new UIPanel
		{
			Width = StyleDimension.FromPercent(1f),
			Height = StyleDimension.FromPixelsAndPercent(-102f, 1f),
			BackgroundColor = new Color(33, 43, 79) * 0.8f
		};
		uIPanel.SetPadding(0f);
		uIElement.Append(uIPanel);
		MakeBackAndCreatebuttons(uIElement);
		UIElement uIElement2 = new UIElement
		{
			Top = StyleDimension.FromPixelsAndPercent(0f, 0f),
			Width = StyleDimension.FromPixelsAndPercent(0f, 1f),
			Height = StyleDimension.FromPixelsAndPercent(0f, 1f),
			HAlign = 1f
		};
		uIElement2.SetPadding(0f);
		uIElement2.PaddingTop = 8f;
		uIElement2.PaddingBottom = 12f;
		uIPanel.Append(uIElement2);
		MakeInfoMenu(uIElement2);
	}

	private void MakeInfoMenu(UIElement parentContainer)
	{
		UIElement uIElement = new UIElement
		{
			Width = StyleDimension.FromPixelsAndPercent(0f, 1f),
			Height = StyleDimension.FromPixelsAndPercent(0f, 1f),
			HAlign = 0.5f,
			VAlign = 0f
		};
		uIElement.SetPadding(10f);
		uIElement.PaddingBottom = 0f;
		uIElement.PaddingTop = 0f;
		parentContainer.Append(uIElement);
		AddSeedButtons(uIElement);
		AddListArea(uIElement);
		AddDescriptionPanel(uIElement);
	}

	private void AddListArea(UIElement infoContainer)
	{
		int num = 0;
		UIList uIList = new UIList
		{
			Width = StyleDimension.FromPixelsAndPercent(-48f, 1f),
			Height = StyleDimension.FromPixelsAndPercent(-138 - num * 2, 1f),
			HAlign = 0f,
			VAlign = 0f,
			Top = StyleDimension.FromPixels(44 + num),
			Left = StyleDimension.FromPixels(24f)
		};
		num = 4;
		UIScrollbar uIScrollbar = new UIScrollbar
		{
			Height = StyleDimension.FromPixelsAndPercent(-138 - num * 2, 1f),
			Top = StyleDimension.FromPixels(44 + num),
			HAlign = 1f
		};
		uIList.SetScrollbar(uIScrollbar);
		infoContainer.Append(uIList);
		if (_allowScrolling)
		{
			infoContainer.Append(uIScrollbar);
		}
		AddSpecialSeedOptions(uIList);
		_optionList = uIList;
	}

	public void RefreshSecretSeedButton()
	{
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		bool flag = SecretSeedsTracker.SeedsForInterface.Count > 0 || _creationState.HasEnteredSpecialSeed || _creationState.HasDisabledSecretSeed;
		if ((_secretSeedButton == null) & flag)
		{
			int num = _seedButtons.Length;
			int num2 = num % 6;
			int num3 = num / 6;
			_secretSeedButton = new GroupOptionButton<bool>(option: true, null, null, Color.White, null)
			{
				Width = StyleDimension.FromPixels(60f),
				Height = StyleDimension.FromPixels(60f),
				InnerHighlightRim = 4,
				HAlign = (float)num2 / 5f,
				Top = StyleDimension.FromPixelsAndPercent(num3 * 67 + 3, 0f),
				ShowHighlightWhenSelected = true
			};
			_secretSeedButton.SetCurrentOption(_creationState.HasEnteredSpecialSeed);
			UIImage uIImage = new UIImage(Main.Assets.Request<Texture2D>("Images/UI/WorldCreation/Seed_Secret", (AssetRequestMode)1).Value)
			{
				Left = StyleDimension.FromPixels(-1f)
			};
			uIImage.OnUpdate += UpdateIconOpacity;
			_secretSeedButton.Append(uIImage);
			_secretSeedButton.SetSnapPoint("seeds", num);
			_secretSeedButton.OnMouseOver += ShowSecretSeedDescription;
			_secretSeedButton.OnMouseOut += ClearOptionDescription;
			_secretSeedButton.OnDraw += _creationState.DrawSpecialSeedRingCallback;
			_secretSeedButton.OnLeftClick += SecretSeedButton_OnLeftClick;
			_seedButtonRegion.Append(_secretSeedButton);
		}
		else if (_secretSeedButton != null && !flag)
		{
			_seedButtonRegion.RemoveChild(_secretSeedButton);
			_secretSeedButton = null;
		}
		else if (_secretSeedButton != null)
		{
			_secretSeedButton.SetCurrentOption(_creationState.HasEnteredSpecialSeed);
		}
	}

	private void SecretSeedButton_OnLeftClick(UIMouseEvent evt, UIElement listeningElement)
	{
		UIWorldCreationAdvancedSecretSeedsList state = new UIWorldCreationAdvancedSecretSeedsList(this, _creationState);
		Main.MenuUI.SetState(state);
		SoundEngine.PlaySound(12);
	}

	private void AddSpecialSeedOptions(UIList listArea)
	{
		int num = 6;
		GroupOptionButton<AWorldGenerationOption>[] array = (_seedButtons = PrepareSeedButtons());
		_seedButtonRegion = new UIElement
		{
			Width = StyleDimension.FromPercent(1f),
			Height = StyleDimension.FromPixels((float)Math.Ceiling((double)array.Length / (double)num) * 70f - 10f)
		};
		listArea.Add(_seedButtonRegion);
		for (int i = 0; i < array.Length; i++)
		{
			GroupOptionButton<AWorldGenerationOption> groupOptionButton = array[i];
			int num2 = i % 6;
			int num3 = i / 6;
			groupOptionButton.HAlign = (float)num2 / 5f;
			groupOptionButton.Top.Set(num3 * 67 + 3, 0f);
			groupOptionButton.OnLeftMouseDown += ClickSeedOption;
			groupOptionButton.SetSnapPoint("seeds", i);
			_seedButtonRegion.Append(groupOptionButton);
			array[i] = groupOptionButton;
		}
		RefreshSecretSeedButton();
	}

	private GroupOptionButton<AWorldGenerationOption>[] PrepareSeedButtons()
	{
		List<GroupOptionButton<AWorldGenerationOption>> list = new List<GroupOptionButton<AWorldGenerationOption>>();
		foreach (AWorldGenerationOption option in WorldGenerationOptions.Options)
		{
			option.Load();
			list.Add(CreateButton(new WorldSpecialSeedOption
			{
				Seed = option,
				Description = option.Description,
				Title = option.Title,
				Element = option.ProvideUIElement()
			}));
		}
		return list.ToArray();
	}

	private void ClickSeedOption(UIMouseEvent evt, UIElement listeningElement)
	{
		AWorldGenerationOption optionValue = ((GroupOptionButton<AWorldGenerationOption>)listeningElement).OptionValue;
		_creationState.ToggleSeedOption(optionValue);
		UpdateContents();
	}

	private GroupOptionButton<AWorldGenerationOption> CreateButton(WorldSpecialSeedOption option)
	{
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		GroupOptionButton<AWorldGenerationOption> groupOptionButton = new GroupOptionButton<AWorldGenerationOption>(option.Seed, null, option.Description, Color.White, null, 1f, 1f, 16f)
		{
			Width = StyleDimension.FromPixels(60f),
			Height = StyleDimension.FromPixels(60f),
			InnerHighlightRim = 4
		};
		groupOptionButton.OnMouseOver += (UIMouseEvent evt, UIElement elem) =>
		{
			ShowOptionDescription(option.Description, option.Title);
		};
		groupOptionButton.OnMouseOut += ClearOptionDescription;
		UIElement element = option.Element;
		element.OnUpdate += UpdateIconOpacity;
		groupOptionButton.Append(element);
		return groupOptionButton;
	}

	private void UpdateIconOpacity(UIElement affectedElement)
	{
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		if (affectedElement.Parent is GroupOptionButton<AWorldGenerationOption> groupOptionButton)
		{
			float num = 0.5f;
			bool flag = groupOptionButton.IsSelected || groupOptionButton.IsMouseHovering;
			if (affectedElement is UIImage uIImage)
			{
				uIImage.Color = (flag ? Color.White : (Color.White * num));
			}
			if (affectedElement is UIImageFramed uIImageFramed)
			{
				uIImageFramed.Color = (flag ? Color.White : (Color.White * num));
			}
		}
	}

	private void AddDescriptionPanel(UIElement container)
	{
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0181: Unknown result type (might be due to invalid IL or missing references)
		//IL_0186: Unknown result type (might be due to invalid IL or missing references)
		float num = 0f;
		UISlicedImage uISlicedImage = new UISlicedImage(Main.Assets.Request<Texture2D>("Images/UI/CharCreation/CategoryPanelHighlight", (AssetRequestMode)1))
		{
			HAlign = 0.5f,
			VAlign = 1f,
			Width = StyleDimension.FromPixelsAndPercent((0f - num) * 2f, 1f),
			Left = StyleDimension.FromPixels(0f - num),
			Height = StyleDimension.FromPixelsAndPercent(88f, 0f),
			Top = StyleDimension.FromPixels(2f)
		};
		uISlicedImage.SetSliceDepths(10);
		uISlicedImage.Color = Color.LightGray * 0.7f;
		container.Append(uISlicedImage);
		UIText uIText = new UIText(Language.GetText("UI.WorldDescriptionDefault"), 0.82f)
		{
			HAlign = 0f,
			VAlign = 0f,
			Width = StyleDimension.FromPixelsAndPercent(0f, 1f),
			Height = StyleDimension.FromPixelsAndPercent(24f, 0f),
			Top = StyleDimension.FromPixelsAndPercent(0f, 0f),
			PaddingLeft = 20f,
			PaddingRight = 20f,
			PaddingTop = 6f
		};
		uISlicedImage.Append(uIText);
		_titleText = uIText;
		UIHorizontalSeparator element = new UIHorizontalSeparator
		{
			Width = StyleDimension.FromPercent(1f),
			Top = StyleDimension.FromPixels(22f),
			VAlign = 0f,
			Color = new Color(131, 135, 183, 255)
		};
		uISlicedImage.Append(element);
		UIText uIText2 = new UIText(Language.GetText("UI.WorldDescriptionDefault"), 0.7f)
		{
			HAlign = 0f,
			VAlign = 0f,
			Width = StyleDimension.FromPixelsAndPercent(0f, 1f),
			Height = StyleDimension.FromPixelsAndPercent(-30f, 1f),
			Top = StyleDimension.FromPixelsAndPercent(25f, 0f),
			PaddingLeft = 20f,
			PaddingRight = 20f,
			PaddingTop = 6f,
			IsWrapped = true
		};
		uISlicedImage.Append(uIText2);
		_descriptionText = uIText2;
	}

	private void ShowOptionDescription(UIMouseEvent evt, UIElement listeningElement)
	{
		LocalizedText localizedText = null;
		if (listeningElement is UICharacterNameButton uICharacterNameButton)
		{
			localizedText = uICharacterNameButton.Description;
		}
		if (listeningElement is GroupOptionButton<bool> groupOptionButton)
		{
			localizedText = groupOptionButton.Description;
		}
		if (localizedText != null)
		{
			ShowOptionDescription(localizedText, Language.Exists(localizedText.Key + "_Title") ? Language.GetText(localizedText.Key + "_Title") : null);
		}
	}

	private void ShowSecretSeedDescription(UIMouseEvent evt, UIElement listeningElement)
	{
		DynamicSpriteFont value = FontAssets.MouseText.Value;
		string joinedSecretSeedString = _creationState.GetJoinedSecretSeedString(value, _descriptionText.GetInnerDimensions().Width / 0.7f, _descriptionText.GetInnerDimensions().Height / 0.7f);
		_descriptionText.SetText(joinedSecretSeedString);
		_titleText.SetText(Language.GetText("UI.WorldDescriptionSecretSeeds_Title"));
	}

	private void ShowOptionDescription(LocalizedText description, LocalizedText title)
	{
		_descriptionText.SetText(description);
		if (title != null)
		{
			_titleText.SetText(title);
		}
	}

	private void ClearOptionDescription(UIMouseEvent evt, UIElement listeningElement)
	{
		ShowOptionDescription(Language.GetText("UI.WorldDescriptionDefault"), Language.GetText("UI.WorldDescriptionDefault_Title"));
	}

	private void AddSeedButtons(UIElement infoContainer)
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		float num = 44f;
		float num2 = 0f + num;
		float pixels = num;
		float pixels2 = 0f;
		GroupOptionButton<bool> groupOptionButton = new GroupOptionButton<bool>(option: true, null, Language.GetText("UI.WorldCreationRandomizeSeedDescription"), Color.White, null)
		{
			Width = StyleDimension.FromPixelsAndPercent(40f, 0f),
			Height = new StyleDimension(40f, 0f),
			HAlign = 0f,
			Top = StyleDimension.FromPixelsAndPercent(pixels2, 0f),
			ShowHighlightWhenSelected = false
		};
		UIImage element = new UIImage(Main.Assets.Request<Texture2D>("Images/UI/WorldCreation/IconEvilRandom", (AssetRequestMode)1))
		{
			IgnoresMouseInteraction = true,
			HAlign = 0.5f,
			VAlign = 0.5f
		};
		groupOptionButton.Append(element);
		groupOptionButton.OnLeftMouseDown += ClickRandomizeSeed;
		groupOptionButton.OnMouseOver += ShowOptionDescription;
		groupOptionButton.OnMouseOut += ClearOptionDescription;
		groupOptionButton.SetSnapPoint("RandomizeSeed", 0);
		infoContainer.Append(groupOptionButton);
		_randomButton = groupOptionButton;
		UICharacterNameButton uICharacterNameButton = new UICharacterNameButton(Language.GetText("UI.WorldCreationSeed"), Language.GetText("UI.WorldCreationSeedEmpty"), Language.GetText("UI.WorldDescriptionSeed"))
		{
			Width = StyleDimension.FromPixelsAndPercent(0f - num2, 1f),
			HAlign = 0f,
			Left = new StyleDimension(pixels, 0f),
			Top = StyleDimension.FromPixelsAndPercent(pixels2, 0f),
			DistanceFromTitleToOption = 29f
		};
		uICharacterNameButton.OnLeftMouseDown += Click_SetSeed;
		uICharacterNameButton.OnMouseOver += ShowOptionDescription;
		uICharacterNameButton.OnMouseOut += ClearOptionDescription;
		uICharacterNameButton.SetSnapPoint("Seed", 0);
		infoContainer.Append(uICharacterNameButton);
		_seedPlate = uICharacterNameButton;
	}

	private void ClickRandomizeSeed(UIMouseEvent evt, UIElement listeningElement)
	{
		_creationState.RandomizeSeed();
		UpdateContents();
	}

	private void Click_SetSeed(UIMouseEvent evt, UIElement listeningElement)
	{
		_creationState.OpenSeedInputMenu();
	}

	private void MakeBackAndCreatebuttons(UIElement outerContainer)
	{
		UITextPanel<LocalizedText> uITextPanel = new UITextPanel<LocalizedText>(Language.GetText("UI.Apply"), 0.65f, large: true)
		{
			Width = StyleDimension.FromPixelsAndPercent(-10f, 0.5f),
			Height = StyleDimension.FromPixels(50f),
			VAlign = 1f,
			HAlign = 0.5f,
			Top = StyleDimension.FromPixels(-43f)
		};
		uITextPanel.OnMouseOver += FadedMouseOver;
		uITextPanel.OnMouseOut += FadedMouseOut;
		uITextPanel.OnLeftMouseDown += Click_GoBack;
		uITextPanel.SetSnapPoint("Back", 0);
		outerContainer.Append(uITextPanel);
		_backButton = uITextPanel;
	}

	private void Click_GoBack(UIMouseEvent evt, UIElement listeningElement)
	{
		GoBack();
	}

	private void GoBack()
	{
		_creationState.ResetSpecialSeedRing();
		_creationState.SetGoBackTarget(_creationState);
		SoundEngine.PlaySound(11);
		Main.MenuUI.SetState(_creationState);
	}

	private void FadedMouseOver(UIMouseEvent evt, UIElement listeningElement)
	{
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		SoundEngine.PlaySound(12);
		((UIPanel)evt.Target).BackgroundColor = new Color(73, 94, 171);
		((UIPanel)evt.Target).BorderColor = Colors.FancyUIFatButtonMouseOver;
	}

	private void FadedMouseOut(UIMouseEvent evt, UIElement listeningElement)
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		((UIPanel)evt.Target).BackgroundColor = new Color(63, 82, 151) * 0.8f;
		((UIPanel)evt.Target).BorderColor = Color.Black;
	}

	public override void Draw(SpriteBatch spriteBatch)
	{
		base.Draw(spriteBatch);
		SetupGamepadPoints(spriteBatch);
		_creationState.DrawSeedSystems(spriteBatch);
	}

	private void SetupGamepadPoints(SpriteBatch spriteBatch)
	{
		UILinkPointNavigator.Shortcuts.BackButtonCommand = 7;
		int num = 3000;
		int currentID = num;
		GetSnapPoints();
		UILinkPoint linkPoint = _helper.GetLinkPoint(currentID++, _backButton);
		UILinkPoint linkPoint2 = _helper.GetLinkPoint(currentID++, _seedPlate);
		UILinkPoint linkPoint3 = _helper.GetLinkPoint(currentID++, _randomButton);
		List<SnapPoint> snapPoints = _optionList.GetSnapPoints();
		UILinkPoint[,] array = _helper.CreateUILinkPointGrid(ref currentID, snapPoints, 6, linkPoint2, null, null, linkPoint);
		_helper.PairLeftRight(linkPoint3, linkPoint2);
		UILinkPoint downSide = array[0, 0];
		_helper.PairUpDown(linkPoint3, downSide);
		_helper.PairUpDown(linkPoint2, downSide);
		UILinkPoint upSide = array[0, array.GetLength(1) - 1];
		_helper.PairUpDown(upSide, linkPoint);
		_helper.MoveToVisuallyClosestPoint(num, currentID);
	}

	public GroupOptionButton<bool> GetSecretSeedButton()
	{
		return _secretSeedButton;
	}

	public void HandleBackButtonUsage()
	{
		GoBack();
	}
}
