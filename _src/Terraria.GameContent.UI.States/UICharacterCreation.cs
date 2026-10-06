using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using ReLogic.Content;
using ReLogic.OS;
using Terraria.Audio;
using Terraria.GameContent.Creative;
using Terraria.GameContent.UI.Elements;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.IO;
using Terraria.Initializers;
using Terraria.Localization;
using Terraria.UI;
using Terraria.UI.Gamepad;

namespace Terraria.GameContent.UI.States;

public class UICharacterCreation : UIState, IHaveBackButtonCommand
{
	private enum CategoryId
	{
		CharInfo,
		Clothing,
		HairStyle,
		HairColor,
		Eye,
		Skin,
		Shirt,
		Undershirt,
		Pants,
		Shoes,
		Count
	}

	private enum HSLSliderId
	{
		Hue,
		Saturation,
		Luminance
	}

	private struct ArmorAssignments
	{
		public int HeadItem;

		public int BodyItem;

		public int LegItem;

		public int Accessory1Item;
	}

	private int[] _validClothStyles = new int[10] { 0, 2, 1, 3, 8, 9, 7, 5, 6, 4 };

	private Dictionary<int, int> _defaultHairstylesForClothStyle = new Dictionary<int, int>
	{
		{ 0, 0 },
		{ 2, 1 },
		{ 1, 12 },
		{ 3, 2 },
		{ 8, 28 },
		{ 9, 68 },
		{ 7, 18 },
		{ 5, 22 },
		{ 6, 81 },
		{ 4, 5 }
	};

	private int[] _validVoiceStyles = new int[3] { 1, 2, 3 };

	private readonly Player _player;

	private UIColoredImageButton[] _colorPickers;

	private CategoryId _selectedPicker;

	private Vector3 _currentColorHSL;

	private UIColoredImageButton _clothingStylesCategoryButton;

	private UIColoredImageButton _hairStylesCategoryButton;

	private UIColoredImageButton _charInfoCategoryButton;

	private UIElement _topContainer;

	private UIElement _middleContainer;

	private UIElement _hslContainer;

	private UIElement _hairstylesContainer;

	private UIElement _clothStylesContainer;

	private UIElement _infoContainer;

	private UIText _hslHexText;

	private UIText _difficultyDescriptionText;

	private UIElement _copyHexButton;

	private UIElement _pasteHexButton;

	private UIElement _randomColorButton;

	private UIElement _copyTemplateButton;

	private UIElement _pasteTemplateButton;

	private UIElement _randomizePlayerButton;

	private UIElement _pitchSlider;

	private UIElement _voiceNext;

	private UIElement _voicePrevious;

	private UIElement _voicePlay;

	private float _pitchAmount;

	private UIElement[] _previewArmorButton = new UIElement[0];

	private UICharacterNameButton _charName;

	private UIText _helpGlyphLeft;

	private UIText _helpGlyphRight;

	private bool _oldMaleForVoiceAutoSwitch = true;

	private int? _lastSelectedHairstyle;

	private UIImageFramed[] _characterPreviewLayers = new UIImageFramed[7];

	public const int MAX_NAME_LENGTH = 20;

	private bool _playedVoicePreviewThisFrame;

	private ArmorAssignments _maleArmor;

	private ArmorAssignments _femaleArmor;

	private GameTipsDisplay _tips;

	public static UIState BackupConfirmationState;

	private static bool dirty;

	private string initialState;

	private bool _pitchChanged;

	private int _pitchChangedCooldown;

	private UIGamepadHelper _helper;

	private List<int> _foundPoints = new List<int>();

	public UICharacterCreation(Player player)
	{
		_player = player;
		_player.difficulty = 0;
		_tips = new GameTipsDisplay(new CharacterCreationTipsProvider());
		BuildPage();
		initialState = GetPlayerTemplateValues();
		dirty = false;
	}

	public override void Update(GameTime gameTime)
	{
		_playedVoicePreviewThisFrame = false;
		base.Update(gameTime);
	}

	private void BuildPage()
	{
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		RemoveAllChildren();
		int num = 4;
		UIElement uIElement = new UIElement
		{
			Width = StyleDimension.FromPixels(500f),
			Height = StyleDimension.FromPixels(380 + num),
			Top = StyleDimension.FromPixels(220f),
			HAlign = 0.5f,
			VAlign = 0f
		};
		uIElement.SetPadding(0f);
		Append(uIElement);
		UIPanel uIPanel = new UIPanel
		{
			Width = StyleDimension.FromPercent(1f),
			Height = StyleDimension.FromPixels(uIElement.Height.Pixels - 150f - (float)num),
			Top = StyleDimension.FromPixels(50f),
			BackgroundColor = new Color(33, 43, 79) * 0.8f
		};
		uIPanel.SetPadding(0f);
		uIElement.Append(uIPanel);
		MakeBackAndCreatebuttons(uIElement);
		MakeCharPreview(uIPanel);
		UIElement uIElement2 = new UIElement
		{
			Width = StyleDimension.FromPixelsAndPercent(0f, 1f),
			Height = StyleDimension.FromPixelsAndPercent(50f, 0f)
		};
		uIElement2.SetPadding(0f);
		uIElement2.PaddingTop = 4f;
		uIElement2.PaddingBottom = 0f;
		uIPanel.Append(uIElement2);
		UIElement uIElement3 = new UIElement
		{
			Top = StyleDimension.FromPixelsAndPercent(uIElement2.Height.Pixels + 6f, 0f),
			Width = StyleDimension.FromPixelsAndPercent(0f, 1f),
			Height = StyleDimension.FromPixelsAndPercent(uIPanel.Height.Pixels - 70f, 0f)
		};
		uIElement3.SetPadding(0f);
		uIElement3.PaddingTop = 3f;
		uIElement3.PaddingBottom = 0f;
		uIPanel.Append(uIElement3);
		_topContainer = uIElement2;
		_middleContainer = uIElement3;
		MakeInfoMenu(uIElement3);
		MakeHSLMenu(uIElement3);
		MakeHairstylesMenu(uIElement3);
		MakeClothStylesMenu(uIElement3);
		MakeCategoriesBar(uIElement2);
		Click_CharInfo(null, null);
	}

	private void MakeCharPreview(UIPanel container)
	{
		float num = 70f;
		for (float num2 = 0f; num2 < 1f; num2++)
		{
			UICharacter uICharacter = new UICharacter(_player, animated: true, hasBackPanel: false, 1.5f)
			{
				Width = StyleDimension.FromPixels(80f),
				Height = StyleDimension.FromPixelsAndPercent(80f, 0f),
				Top = StyleDimension.FromPixelsAndPercent(0f - num, 0f),
				VAlign = 0f,
				HAlign = 0.5f
			};
			uICharacter.PrepareAction = PreparePreview_Main;
			container.Append(uICharacter);
		}
	}

	private void MakeHairstylesMenu(UIElement middleInnerPanel)
	{
		Main.Hairstyles.UpdateUnlocks();
		UIElement uIElement = new UIElement
		{
			Width = StyleDimension.FromPixelsAndPercent(-10f, 1f),
			Height = StyleDimension.FromPixelsAndPercent(0f, 1f),
			HAlign = 0.5f,
			VAlign = 0.5f,
			Top = StyleDimension.FromPixels(6f)
		};
		middleInnerPanel.Append(uIElement);
		uIElement.SetPadding(0f);
		UIList uIList = new UIList
		{
			Width = StyleDimension.FromPixelsAndPercent(-18f, 1f),
			Height = StyleDimension.FromPixelsAndPercent(-6f, 1f)
		};
		uIList.SetPadding(4f);
		uIElement.Append(uIList);
		UIScrollbar uIScrollbar = new UIScrollbar
		{
			HAlign = 1f,
			Height = StyleDimension.FromPixelsAndPercent(-30f, 1f),
			Top = StyleDimension.FromPixels(10f)
		};
		uIScrollbar.SetView(100f, 1000f);
		uIList.SetScrollbar(uIScrollbar);
		uIElement.Append(uIScrollbar);
		int count = Main.Hairstyles.AvailableHairstyles.Count;
		UIElement uIElement2 = new UIElement
		{
			Width = StyleDimension.FromPixelsAndPercent(0f, 1f),
			Height = StyleDimension.FromPixelsAndPercent(48 * (count / 10 + ((count % 10 != 0) ? 1 : 0)), 0f)
		};
		uIList.Add(uIElement2);
		uIElement2.SetPadding(0f);
		for (int i = 0; i < count; i++)
		{
			UIHairStyleButton uIHairStyleButton = new UIHairStyleButton(_player, Main.Hairstyles.AvailableHairstyles[i])
			{
				Left = StyleDimension.FromPixels((float)(i % 10) * 46f + 6f),
				Top = StyleDimension.FromPixels((float)(i / 10) * 48f + 1f)
			};
			uIHairStyleButton.SetSnapPoint("Middle", i);
			uIHairStyleButton.SkipRenderingContent(i);
			uIHairStyleButton.OnLeftMouseDown += RecordThatHairWasSelected;
			uIElement2.Append(uIHairStyleButton);
		}
		_hairstylesContainer = uIElement;
	}

	private void RecordThatHairWasSelected(UIMouseEvent evt, UIElement listeningElement)
	{
		_lastSelectedHairstyle = _player.hair;
	}

	private void MakeClothStylesMenu(UIElement middleInnerPanel)
	{
		//IL_01e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_06aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_06bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_06c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_06d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_06d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_070f: Unknown result type (might be due to invalid IL or missing references)
		//IL_074a: Unknown result type (might be due to invalid IL or missing references)
		//IL_074f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0773: Unknown result type (might be due to invalid IL or missing references)
		//IL_07a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_07e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_07e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0809: Unknown result type (might be due to invalid IL or missing references)
		//IL_08bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_08c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0940: Unknown result type (might be due to invalid IL or missing references)
		//IL_094a: Unknown result type (might be due to invalid IL or missing references)
		//IL_09c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a5c: Unknown result type (might be due to invalid IL or missing references)
		UIElement uIElement = new UIElement
		{
			Width = StyleDimension.FromPixelsAndPercent(0f, 1f),
			Height = StyleDimension.FromPixelsAndPercent(0f, 1f),
			HAlign = 0.5f,
			VAlign = 0.5f
		};
		middleInnerPanel.Append(uIElement);
		uIElement.SetPadding(0f);
		int num = 0;
		for (int i = 0; i < _validClothStyles.Length; i++)
		{
			int num2 = 19;
			num2 = ((i < _validClothStyles.Length / 2) ? (num2 - 8) : (num2 + 10));
			UIClothStyleButton uIClothStyleButton = new UIClothStyleButton(_player, _validClothStyles[i], PreparePreview_ClothStyle)
			{
				Left = StyleDimension.FromPixels((float)i * 46f + (float)num2),
				Top = StyleDimension.FromPixels(num)
			};
			uIClothStyleButton.OnLeftMouseDown += Click_CharClothStyle;
			uIClothStyleButton.SetSnapPoint("Middle", i);
			uIElement.Append(uIClothStyleButton);
		}
		int num3 = 15;
		int num4 = 60;
		UIElement uIElement2 = new UIElement
		{
			Width = StyleDimension.FromPixels(170f),
			Height = StyleDimension.FromPixels(50f),
			HAlign = 0f,
			Left = new StyleDimension((float)num4 - 34f, 0.5f),
			VAlign = 1f,
			Top = StyleDimension.FromPixels(-num3 - 7)
		};
		uIElement.Append(uIElement2);
		UIColoredImageButton uIColoredImageButton = new UIColoredImageButton(Main.Assets.Request<Texture2D>("Images/Item_" + (short)271, (AssetRequestMode)1), isSmall: true)
		{
			VAlign = 0.5f,
			HAlign = 0f,
			Left = StyleDimension.FromPixelsAndPercent(0f, 0f)
		};
		uIColoredImageButton.SetColor(_player.hairColor);
		uIColoredImageButton.OnLeftMouseDown += EquipArmorNone;
		uIElement2.Append(uIColoredImageButton);
		UIColoredImageButton uIColoredImageButton2 = new UIColoredImageButton(Main.Assets.Request<Texture2D>("Images/Item_" + (short)5660, (AssetRequestMode)1), isSmall: true)
		{
			VAlign = 0.5f,
			HAlign = 0.5f
		};
		uIColoredImageButton2.OnLeftMouseDown += EquipArmorHallowed;
		uIElement2.Append(uIColoredImageButton2);
		UIColoredImageButton uIColoredImageButton3 = new UIColoredImageButton(Main.Assets.Request<Texture2D>("Images/Item_" + (short)91, (AssetRequestMode)1), isSmall: true)
		{
			VAlign = 0.5f,
			HAlign = 0.25f
		};
		uIColoredImageButton3.OnLeftMouseDown += EquipArmorSilver;
		uIElement2.Append(uIColoredImageButton3);
		UIColoredImageButton uIColoredImageButton4 = new UIColoredImageButton(Main.Assets.Request<Texture2D>("Images/Item_" + (short)239, (AssetRequestMode)1), isSmall: true)
		{
			VAlign = 0.5f,
			HAlign = 0.75f
		};
		uIColoredImageButton4.OnLeftMouseDown += EquipArmorFormal;
		uIElement2.Append(uIColoredImageButton4);
		UIColoredImageButton uIColoredImageButton5 = new UIColoredImageButton(Main.Assets.Request<Texture2D>("Images/Item_" + (short)237, (AssetRequestMode)1), isSmall: true)
		{
			VAlign = 0.5f,
			HAlign = 1f
		};
		uIColoredImageButton5.OnLeftMouseDown += EquipArmorSwimming;
		uIElement2.Append(uIColoredImageButton5);
		_previewArmorButton = new UIElement[5];
		_previewArmorButton[0] = uIColoredImageButton;
		_previewArmorButton[1] = uIColoredImageButton2;
		_previewArmorButton[2] = uIColoredImageButton3;
		_previewArmorButton[3] = uIColoredImageButton4;
		_previewArmorButton[4] = uIColoredImageButton5;
		_previewArmorButton[0].SetSnapPoint("Preview", 0);
		_previewArmorButton[2].SetSnapPoint("Preview", 1);
		_previewArmorButton[1].SetSnapPoint("Preview", 2);
		_previewArmorButton[3].SetSnapPoint("Preview", 3);
		_previewArmorButton[4].SetSnapPoint("Preview", 4);
		UIElement uIElement3 = new UIElement
		{
			Width = StyleDimension.FromPixels(100f),
			Height = StyleDimension.FromPixels(50f),
			HAlign = 0f,
			Left = new StyleDimension(num4, 0.5f),
			VAlign = 1f,
			Top = StyleDimension.FromPixels(-num3 + 38 - 9)
		};
		uIElement.Append(uIElement3);
		UIColoredImageButton uIColoredImageButton6 = new UIColoredImageButton(Main.Assets.Request<Texture2D>("Images/UI/CharCreation/Copy", (AssetRequestMode)1), isSmall: true)
		{
			VAlign = 0.5f,
			HAlign = 0f,
			Left = StyleDimension.FromPixelsAndPercent(0f, 0f)
		};
		uIColoredImageButton6.OnLeftMouseDown += Click_CopyPlayerTemplate;
		uIElement3.Append(uIColoredImageButton6);
		_copyTemplateButton = uIColoredImageButton6;
		UIColoredImageButton uIColoredImageButton7 = new UIColoredImageButton(Main.Assets.Request<Texture2D>("Images/UI/CharCreation/Paste", (AssetRequestMode)1), isSmall: true)
		{
			VAlign = 0.5f,
			HAlign = 0.5f
		};
		uIColoredImageButton7.OnLeftMouseDown += Click_PastePlayerTemplate;
		uIElement3.Append(uIColoredImageButton7);
		_pasteTemplateButton = uIColoredImageButton7;
		UIColoredImageButton uIColoredImageButton8 = new UIColoredImageButton(Main.Assets.Request<Texture2D>("Images/UI/CharCreation/Randomize", (AssetRequestMode)1), isSmall: true)
		{
			VAlign = 0.5f,
			HAlign = 1f
		};
		uIColoredImageButton8.OnLeftMouseDown += Click_RandomizePlayer;
		uIElement3.Append(uIColoredImageButton8);
		_randomizePlayerButton = uIColoredImageButton8;
		UIElement uIElement4 = new UIElement
		{
			Width = StyleDimension.FromPixels(90f),
			Height = StyleDimension.FromPixels(50f),
			HAlign = 1f,
			Left = new StyleDimension(-num4, -0.5f),
			VAlign = 1f,
			Top = StyleDimension.FromPixels(-num3)
		};
		uIElement.Append(uIElement4);
		UIHorizontalSeparator element = new UIHorizontalSeparator
		{
			Width = StyleDimension.FromPixelsAndPercent(-38f, 1f),
			HAlign = 0.5f,
			VAlign = 1f,
			Top = StyleDimension.FromPixelsAndPercent(-52 - num3, 0f),
			Left = new StyleDimension(-3f, 0f),
			Color = Color.Lerp(Color.White, new Color(63, 65, 151, 255), 0.85f) * 0.9f
		};
		uIElement.Append(element);
		Asset<Texture2D> val = Main.Assets.Request<Texture2D>("Images/UI/TexturePackButtons", (AssetRequestMode)1);
		Asset<Texture2D> val2 = Main.Assets.Request<Texture2D>("Images/UI/TexturePackButtonsOutline", (AssetRequestMode)1);
		UIImageButton uIImageButton = new UIImageButton(val, val.Frame(2, 2, 0, 1))
		{
			VAlign = 0.5f,
			HAlign = 0f,
			Left = StyleDimension.FromPixelsAndPercent(0f, 0f),
			BorderColor = Main.OurFavoriteColor
		};
		uIImageButton.SetVisibility(1f, 1f);
		uIImageButton.SetHoverImage(val2, val2.Frame(2, 2, 0, 1));
		uIImageButton.OnLeftMouseDown += Click_VoiceCycleBack;
		uIElement4.Append(uIImageButton);
		UIImageButton uIImageButton2 = new UIImageButton(val, val.Frame(2, 2, 1, 1))
		{
			VAlign = 0.5f,
			HAlign = 1f,
			Left = StyleDimension.FromPixelsAndPercent(0f, 0f),
			BorderColor = Main.OurFavoriteColor
		};
		uIImageButton2.SetVisibility(1f, 1f);
		uIImageButton2.SetHoverImage(val2, val2.Frame(2, 2, 1, 1));
		uIImageButton2.OnLeftMouseDown += Click_VoiceCycleForward;
		uIElement4.Append(uIImageButton2);
		UIColoredImageButton uIColoredImageButton9 = new UIColoredImageButton(null)
		{
			VAlign = 0.5f,
			HAlign = 0.5f,
			Left = StyleDimension.FromPixelsAndPercent(0f, 0f),
			Width = StyleDimension.FromPixels(52f),
			Height = StyleDimension.FromPixels(52f)
		};
		UIImage uIImage = new UIImage(Main.Assets.Request<Texture2D>("Images/UI/CharCreation/Voice", (AssetRequestMode)1))
		{
			VAlign = 0.5f,
			HAlign = 0.5f,
			IgnoresMouseInteraction = true,
			Color = Main.OurFavoriteColor
		};
		uIImage.OnUpdate += voiceIcon_OnUpdate;
		uIColoredImageButton9.Append(uIImage);
		UIText uIText = new UIText("", 0.85f)
		{
			VAlign = 1f,
			HAlign = 1f,
			TextOriginX = 0.5f,
			TextOriginY = 1f,
			Top = StyleDimension.FromPixels(-6f),
			Left = StyleDimension.FromPixels(-12f),
			ShadowColor = Color.Black * 0.3f
		};
		uIText.OnUpdate += voiceNumber_OnUpdate;
		uIColoredImageButton9.Append(uIText);
		uIColoredImageButton9.OnLeftMouseDown += Click_VoicePlay;
		uIElement4.Append(uIColoredImageButton9);
		UIColoredSlider uIColoredSlider = new UIColoredSlider(LocalizedText.Empty, GetPitchSlider, SetPitchSlider_Keyboard, SetPitchSlider_GamePad, GetVoicePitchColorAt, Color.Transparent)
		{
			VAlign = 1f,
			HAlign = 0.5f,
			Width = StyleDimension.FromPixelsAndPercent(187f, 0f),
			Top = StyleDimension.FromPixels(-10f),
			Left = StyleDimension.FromPixels(55f)
		};
		uIColoredSlider.OnLeftMouseDown += Click_VoicePitch;
		uIColoredSlider.OnUpdate += PitchSliderUpdate;
		uIColoredSlider.SetSnapPoint("pitch", 0, null, new Vector2(-93f, 16f));
		uIElement4.Append(uIColoredSlider);
		_pitchSlider = uIColoredSlider;
		uIImageButton.SetSnapPoint("Low", 1);
		uIColoredImageButton9.SetSnapPoint("Low", 2);
		uIImageButton2.SetSnapPoint("Low", 3);
		_voicePrevious = uIImageButton;
		_voiceNext = uIImageButton2;
		_voicePlay = uIColoredImageButton9;
		uIColoredImageButton6.SetSnapPoint("Low", 4);
		uIColoredImageButton7.SetSnapPoint("Low", 5);
		uIColoredImageButton8.SetSnapPoint("Low", 6);
		_clothStylesContainer = uIElement;
	}

	private void EquipArmorNone(UIMouseEvent evt, UIElement listeningElement)
	{
		SoundEngine.PlaySound(12);
		_femaleArmor = (_maleArmor = default);
	}

	private void EquipArmorGold(UIMouseEvent evt, UIElement listeningElement)
	{
		SoundEngine.PlaySound(12);
		_femaleArmor = (_maleArmor = new ArmorAssignments
		{
			HeadItem = 92,
			BodyItem = 83,
			LegItem = 79
		});
	}

	private void EquipArmorSilver(UIMouseEvent evt, UIElement listeningElement)
	{
		SoundEngine.PlaySound(12);
		_femaleArmor = (_maleArmor = new ArmorAssignments
		{
			HeadItem = 91,
			BodyItem = 82,
			LegItem = 78
		});
	}

	private void EquipArmorFuneral(UIMouseEvent evt, UIElement listeningElement)
	{
		SoundEngine.PlaySound(12);
		_femaleArmor = (_maleArmor = new ArmorAssignments
		{
			HeadItem = 4704,
			BodyItem = 4705,
			LegItem = 4706
		});
	}

	private void EquipArmorHallowed(UIMouseEvent evt, UIElement listeningElement)
	{
		SoundEngine.PlaySound(12);
		_femaleArmor = (_maleArmor = new ArmorAssignments
		{
			HeadItem = 5660,
			BodyItem = 551,
			LegItem = 552
		});
	}

	private void EquipArmorFormal(UIMouseEvent evt, UIElement listeningElement)
	{
		SoundEngine.PlaySound(12);
		_maleArmor = new ArmorAssignments
		{
			HeadItem = 239,
			BodyItem = 240,
			LegItem = 241
		};
		_femaleArmor = new ArmorAssignments
		{
			HeadItem = 3478,
			BodyItem = 3479,
			LegItem = 0
		};
	}

	private void EquipArmorSwimming(UIMouseEvent evt, UIElement listeningElement)
	{
		SoundEngine.PlaySound(12);
		_maleArmor = new ArmorAssignments
		{
			HeadItem = 237,
			BodyItem = 3785,
			LegItem = 5649
		};
		_femaleArmor = new ArmorAssignments
		{
			HeadItem = 237,
			BodyItem = 5646,
			LegItem = 5647,
			Accessory1Item = 208
		};
	}

	private void PreparePreview_Main()
	{
		_player.direction = 1;
		TryAutoAssigningHair();
		UpdatePreviewItems();
	}

	private void PreparePreview_ClothStyle()
	{
		_player.direction = (_player.Male ? 1 : (-1));
		TryAutoAssigningHair();
		UpdatePreviewItems();
	}

	private void TryAutoAssigningHair()
	{
		if (!_lastSelectedHairstyle.HasValue && _defaultHairstylesForClothStyle.TryGetValue(_player.skinVariant, out var value))
		{
			_player.hair = value;
		}
	}

	private void UpdatePreviewItems()
	{
		ArmorAssignments armorAssignments = _femaleArmor;
		if (_player.Male)
		{
			armorAssignments = _maleArmor;
		}
		_player.armor[0].SetDefaults(armorAssignments.HeadItem);
		_player.armor[1].SetDefaults(armorAssignments.BodyItem);
		_player.armor[2].SetDefaults(armorAssignments.LegItem);
	}

	private void PitchSliderUpdate(UIElement affectedElement)
	{
		if (_pitchChanged && --_pitchChangedCooldown <= 0)
		{
			_pitchChanged = false;
			PlayVoicePreview();
		}
	}

	private void PitchChanged()
	{
		_pitchChanged = true;
		_pitchChangedCooldown = 3;
	}

	private void SetPitchSlider_GamePad()
	{
		if (PlayerInput.UsingGamepad)
		{
			float pitchAmount = _pitchAmount;
			float num = UILinksInitializer.HandleSliderHorizontalInput(Utils.Remap(_pitchAmount, -1f, 1f, 0f, 1f), 0f, 1f, PlayerInput.CurrentProfile.InterfaceDeadzoneX, 0.35f);
			_pitchAmount = Utils.Remap(num, 0f, 1f, -1f, 1f);
			num = RemapPitchSliderKnob(num);
			_player.voicePitchOffset = Utils.Remap(num, 0f, 1f, -1f, 1f);
			if (pitchAmount != _pitchAmount)
			{
				PitchChanged();
			}
		}
	}

	private float RemapPitchSliderKnob(float pitchSliderValue)
	{
		int num = 20;
		return (float)Math.Round(pitchSliderValue * (float)num) / (float)num;
	}

	private void SetPitchSlider_Keyboard(float amount)
	{
		amount = RemapPitchSliderKnob(amount);
		float voicePitchOffset = _player.voicePitchOffset;
		_pitchAmount = (_player.voicePitchOffset = Utils.Remap(amount, 0f, 1f, -1f, 1f));
		_pitchChangedCooldown = 3;
		if (voicePitchOffset != _player.voicePitchOffset)
		{
			PitchChanged();
		}
	}

	private float GetPitchSlider()
	{
		return Utils.Remap(RemapPitchSliderKnob(_pitchAmount), -1f, 1f, 0f, 1f);
	}

	private Color GetVoicePitchColorAt(float x)
	{
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		float fromValue = (x * 4f + 0.5f) % 1f;
		float num = Utils.Remap(fromValue, 0f, 0.5f, 0f, 1f) * Utils.Remap(fromValue, 0.5f, 1f, 1f, 0f);
		float num2 = num * num * num * num * num;
		return Color.Lerp(new Color(90, 90, 120), Color.White, num2);
	}

	private void voiceNumber_OnUpdate(UIElement affectedElement)
	{
		int num = 0;
		int[] variantOrder = PlayerVoiceID.VariantOrder;
		for (int i = 0; i < variantOrder.Length; i++)
		{
			if (variantOrder[i] == _player.voiceVariant)
			{
				num = i;
				break;
			}
		}
		(affectedElement as UIText).SetText((num + 1).ToString());
	}

	private void voiceIcon_OnUpdate(UIElement affectedElement)
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		(affectedElement as UIImage).Color = PlayerVoiceID.Sets.Colors[_player.voiceVariant];
	}

	private void MakeCategoriesBar(UIElement categoryContainer)
	{
		//IL_0136: Unknown result type (might be due to invalid IL or missing references)
		//IL_014e: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0309: Unknown result type (might be due to invalid IL or missing references)
		//IL_030e: Unknown result type (might be due to invalid IL or missing references)
		float xPositionStart = -240f;
		float xPositionPerId = 48f;
		_colorPickers = new UIColoredImageButton[10];
		categoryContainer.Append(CreateColorPicker(CategoryId.HairColor, "Images/UI/CharCreation/ColorHair", xPositionStart, xPositionPerId));
		categoryContainer.Append(CreateColorPicker(CategoryId.Eye, "Images/UI/CharCreation/ColorEye", xPositionStart, xPositionPerId));
		categoryContainer.Append(CreateColorPicker(CategoryId.Skin, "Images/UI/CharCreation/ColorSkin", xPositionStart, xPositionPerId));
		categoryContainer.Append(CreateColorPicker(CategoryId.Shirt, "Images/UI/CharCreation/ColorShirt", xPositionStart, xPositionPerId));
		categoryContainer.Append(CreateColorPicker(CategoryId.Undershirt, "Images/UI/CharCreation/ColorUndershirt", xPositionStart, xPositionPerId));
		categoryContainer.Append(CreateColorPicker(CategoryId.Pants, "Images/UI/CharCreation/ColorPants", xPositionStart, xPositionPerId));
		categoryContainer.Append(CreateColorPicker(CategoryId.Shoes, "Images/UI/CharCreation/ColorShoes", xPositionStart, xPositionPerId));
		_colorPickers[4].SetMiddleTexture(Main.Assets.Request<Texture2D>("Images/UI/CharCreation/ColorEyeBack", (AssetRequestMode)1));
		_clothingStylesCategoryButton = CreatePickerWithoutClick(CategoryId.Clothing, "Images/UI/CharCreation/ClothStyleMale", xPositionStart, xPositionPerId);
		_clothingStylesCategoryButton.OnLeftMouseDown += Click_ClothStyles;
		_clothingStylesCategoryButton.SetSnapPoint("Top", 1);
		categoryContainer.Append(_clothingStylesCategoryButton);
		Asset<Texture2D> val = Main.Assets.Request<Texture2D>("Images/UI/CharCreation/ColorCharacter", (AssetRequestMode)1);
		_clothingStylesCategoryButton.SetColor(Color.Transparent);
		for (int i = 0; i < _characterPreviewLayers.Length; i++)
		{
			UIImageFramed uIImageFramed = new UIImageFramed(val, val.Frame(1, 7, 0, i))
			{
				HAlign = 0.5f,
				VAlign = 0.5f
			};
			_characterPreviewLayers[i] = uIImageFramed;
			_clothingStylesCategoryButton.Append(uIImageFramed);
			_clothingStylesCategoryButton.OnUpdate += _clothingStylesCategoryButton_OnUpdate;
		}
		_hairStylesCategoryButton = CreatePickerWithoutClick(CategoryId.HairStyle, "Images/UI/CharCreation/HairStyle_Hair", xPositionStart, xPositionPerId);
		_hairStylesCategoryButton.OnLeftMouseDown += Click_HairStyles;
		_hairStylesCategoryButton.SetMiddleTexture(Main.Assets.Request<Texture2D>("Images/UI/CharCreation/HairStyle_Arrow", (AssetRequestMode)1));
		_hairStylesCategoryButton.SetSnapPoint("Top", 2);
		categoryContainer.Append(_hairStylesCategoryButton);
		_charInfoCategoryButton = CreatePickerWithoutClick(CategoryId.CharInfo, "Images/UI/CharCreation/CharInfo", xPositionStart, xPositionPerId);
		_charInfoCategoryButton.OnLeftMouseDown += Click_CharInfo;
		_charInfoCategoryButton.SetSnapPoint("Top", 0);
		categoryContainer.Append(_charInfoCategoryButton);
		UpdateColorPickers();
		UIHorizontalSeparator element = new UIHorizontalSeparator
		{
			Width = StyleDimension.FromPixelsAndPercent(-25f, 1f),
			Top = StyleDimension.FromPixels(6f),
			Left = new StyleDimension(-2.5f, 0f),
			VAlign = 1f,
			HAlign = 0.5f,
			Color = Color.Lerp(Color.White, new Color(63, 65, 151, 255), 0.85f) * 0.9f
		};
		categoryContainer.Append(element);
		int num = 21;
		UIText uIText = new UIText(PlayerInput.GenerateInputTag_ForCurrentGamemode(tagForGameplay: false, "HotbarMinus"))
		{
			Left = new StyleDimension(-num, 0f),
			VAlign = 0.5f,
			Top = new StyleDimension(-4f, 0f)
		};
		categoryContainer.Append(uIText);
		UIText uIText2 = new UIText(PlayerInput.GenerateInputTag_ForCurrentGamemode(tagForGameplay: false, "HotbarMinus"))
		{
			HAlign = 1f,
			Left = new StyleDimension(12 + num, 0f),
			VAlign = 0.5f,
			Top = new StyleDimension(-4f, 0f)
		};
		categoryContainer.Append(uIText2);
		_helpGlyphLeft = uIText;
		_helpGlyphRight = uIText2;
		categoryContainer.OnUpdate += UpdateHelpGlyphs;
	}

	private void _clothingStylesCategoryButton_OnUpdate(UIElement affectedElement)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		_characterPreviewLayers[0].Color = _player.hairColor;
		_characterPreviewLayers[1].Color = _player.eyeColor;
		_characterPreviewLayers[2].Color = _player.skinColor;
		_characterPreviewLayers[3].Color = _player.shirtColor;
		_characterPreviewLayers[4].Color = _player.underShirtColor;
		_characterPreviewLayers[5].Color = _player.pantsColor;
		_characterPreviewLayers[6].Color = _player.shoeColor;
	}

	private void UpdateHelpGlyphs(UIElement element)
	{
		string text = "";
		string text2 = "";
		if (PlayerInput.UsingGamepad)
		{
			text = PlayerInput.GenerateInputTag_ForCurrentGamemode(tagForGameplay: false, "HotbarMinus");
			text2 = PlayerInput.GenerateInputTag_ForCurrentGamemode(tagForGameplay: false, "HotbarPlus");
		}
		_helpGlyphLeft.SetText(text);
		_helpGlyphRight.SetText(text2);
	}

	private UIColoredImageButton CreateColorPicker(CategoryId id, string texturePath, float xPositionStart, float xPositionPerId)
	{
		UIColoredImageButton uIColoredImageButton = new UIColoredImageButton(Main.Assets.Request<Texture2D>(texturePath, (AssetRequestMode)1));
		_colorPickers[(int)id] = uIColoredImageButton;
		uIColoredImageButton.VAlign = 0f;
		uIColoredImageButton.HAlign = 0f;
		uIColoredImageButton.Left.Set(xPositionStart + (float)id * xPositionPerId, 0.5f);
		uIColoredImageButton.OnLeftMouseDown += Click_ColorPicker;
		uIColoredImageButton.SetSnapPoint("Top", (int)id);
		return uIColoredImageButton;
	}

	private UIColoredImageButton CreatePickerWithoutClick(CategoryId id, string texturePath, float xPositionStart, float xPositionPerId)
	{
		UIColoredImageButton uIColoredImageButton = new UIColoredImageButton(Main.Assets.Request<Texture2D>(texturePath, (AssetRequestMode)1));
		uIColoredImageButton.VAlign = 0f;
		uIColoredImageButton.HAlign = 0f;
		uIColoredImageButton.Left.Set(xPositionStart + (float)id * xPositionPerId, 0.5f);
		return uIColoredImageButton;
	}

	private void MakeInfoMenu(UIElement parentContainer)
	{
		//IL_01d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01de: Unknown result type (might be due to invalid IL or missing references)
		//IL_020e: Unknown result type (might be due to invalid IL or missing references)
		//IL_027a: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_034d: Unknown result type (might be due to invalid IL or missing references)
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
		UICharacterNameButton uICharacterNameButton = new UICharacterNameButton(Language.GetText("UI.WorldCreationName"), Language.GetText("UI.PlayerEmptyName"));
		uICharacterNameButton.Width = StyleDimension.FromPixelsAndPercent(0f, 1f);
		uICharacterNameButton.HAlign = 0.5f;
		uIElement.Append(uICharacterNameButton);
		_charName = uICharacterNameButton;
		uICharacterNameButton.OnLeftMouseDown += Click_Naming;
		uICharacterNameButton.SetSnapPoint("Middle", 0);
		float num = 4f;
		float num2 = 0f;
		float num3 = 0.4f;
		UIElement uIElement2 = new UIElement
		{
			HAlign = 0f,
			VAlign = 1f,
			Width = StyleDimension.FromPixelsAndPercent(0f - num, num3),
			Height = StyleDimension.FromPixelsAndPercent(-50f, 1f)
		};
		uIElement2.SetPadding(0f);
		uIElement.Append(uIElement2);
		UISlicedImage uISlicedImage = new UISlicedImage(Main.Assets.Request<Texture2D>("Images/UI/CharCreation/CategoryPanelHighlight", (AssetRequestMode)1))
		{
			HAlign = 1f,
			VAlign = 1f,
			Width = StyleDimension.FromPixelsAndPercent((0f - num) * 2f, 1f - num3),
			Left = StyleDimension.FromPixels(0f - num),
			Height = StyleDimension.FromPixelsAndPercent(uIElement2.Height.Pixels, uIElement2.Height.Precent)
		};
		uISlicedImage.SetSliceDepths(10);
		uISlicedImage.Color = Color.LightGray * 0.7f;
		uIElement.Append(uISlicedImage);
		float num4 = 4f;
		UIDifficultyButton uIDifficultyButton = new UIDifficultyButton(_player, Lang.menu[26], Lang.menu[31], 0, Color.Cyan)
		{
			HAlign = 0f,
			VAlign = 1f / (num4 - 1f),
			Width = StyleDimension.FromPixelsAndPercent(0f, 1f),
			Height = StyleDimension.FromPixelsAndPercent(0f - num2, 1f / num4)
		};
		UIDifficultyButton uIDifficultyButton2 = new UIDifficultyButton(_player, Lang.menu[25], Lang.menu[30], 1, Main.mcColor)
		{
			HAlign = 0f,
			VAlign = 2f / (num4 - 1f),
			Width = StyleDimension.FromPixelsAndPercent(0f, 1f),
			Height = StyleDimension.FromPixelsAndPercent(0f - num2, 1f / num4)
		};
		UIDifficultyButton uIDifficultyButton3 = new UIDifficultyButton(_player, Lang.menu[24], Lang.menu[29], 2, Main.hcColor)
		{
			HAlign = 0f,
			VAlign = 1f,
			Width = StyleDimension.FromPixelsAndPercent(0f, 1f),
			Height = StyleDimension.FromPixelsAndPercent(0f - num2, 1f / num4)
		};
		UIDifficultyButton uIDifficultyButton4 = new UIDifficultyButton(_player, Language.GetText("UI.Creative"), Language.GetText("UI.CreativeDescriptionPlayer"), 3, Main.creativeModeColor)
		{
			HAlign = 0f,
			VAlign = 0f,
			Width = StyleDimension.FromPixelsAndPercent(0f, 1f),
			Height = StyleDimension.FromPixelsAndPercent(0f - num2, 1f / num4)
		};
		UIText uIText = new UIText(Lang.menu[26])
		{
			HAlign = 0f,
			VAlign = 0.5f,
			Width = StyleDimension.FromPixelsAndPercent(0f, 1f),
			Height = StyleDimension.FromPixelsAndPercent(0f, 1f),
			Top = StyleDimension.FromPixelsAndPercent(15f, 0f),
			IsWrapped = true
		};
		uIText.PaddingLeft = 20f;
		uIText.PaddingRight = 20f;
		uISlicedImage.Append(uIText);
		uIElement2.Append(uIDifficultyButton4);
		uIElement2.Append(uIDifficultyButton);
		uIElement2.Append(uIDifficultyButton2);
		uIElement2.Append(uIDifficultyButton3);
		_infoContainer = uIElement;
		_difficultyDescriptionText = uIText;
		uIDifficultyButton4.OnLeftMouseDown += UpdateDifficultyDescription;
		uIDifficultyButton.OnLeftMouseDown += UpdateDifficultyDescription;
		uIDifficultyButton2.OnLeftMouseDown += UpdateDifficultyDescription;
		uIDifficultyButton3.OnLeftMouseDown += UpdateDifficultyDescription;
		UpdateDifficultyDescription(null, null);
		uIDifficultyButton4.SetSnapPoint("Middle", 1);
		uIDifficultyButton.SetSnapPoint("Middle", 2);
		uIDifficultyButton2.SetSnapPoint("Middle", 3);
		uIDifficultyButton3.SetSnapPoint("Middle", 4);
	}

	private void UpdateDifficultyDescription(UIMouseEvent evt, UIElement listeningElement)
	{
		LocalizedText text = Lang.menu[31];
		switch (_player.difficulty)
		{
		case 0:
			text = Lang.menu[31];
			break;
		case 1:
			text = Lang.menu[30];
			break;
		case 2:
			text = Lang.menu[29];
			break;
		case 3:
			text = Language.GetText("UI.CreativeDescriptionPlayer");
			break;
		}
		_difficultyDescriptionText.SetText(text);
	}

	private void MakeHSLMenu(UIElement parentContainer)
	{
		UIElement uIElement = new UIElement
		{
			Width = StyleDimension.FromPixelsAndPercent(220f, 0f),
			Height = StyleDimension.FromPixelsAndPercent(158f, 0f),
			HAlign = 0.5f,
			VAlign = 0f
		};
		uIElement.SetPadding(0f);
		parentContainer.Append(uIElement);
		UIElement uIElement2 = new UIPanel
		{
			Width = StyleDimension.FromPixelsAndPercent(220f, 0f),
			Height = StyleDimension.FromPixelsAndPercent(104f, 0f),
			HAlign = 0.5f,
			VAlign = 0f,
			Top = StyleDimension.FromPixelsAndPercent(10f, 0f)
		};
		uIElement2.SetPadding(0f);
		uIElement2.PaddingTop = 3f;
		uIElement.Append(uIElement2);
		uIElement2.Append(CreateHSLSlider(HSLSliderId.Hue));
		uIElement2.Append(CreateHSLSlider(HSLSliderId.Saturation));
		uIElement2.Append(CreateHSLSlider(HSLSliderId.Luminance));
		UIPanel uIPanel = new UIPanel
		{
			VAlign = 1f,
			HAlign = 1f,
			Width = StyleDimension.FromPixelsAndPercent(100f, 0f),
			Height = StyleDimension.FromPixelsAndPercent(32f, 0f)
		};
		UIText uIText = new UIText("FFFFFF")
		{
			VAlign = 0.5f,
			HAlign = 0.5f
		};
		uIPanel.Append(uIText);
		uIElement.Append(uIPanel);
		UIColoredImageButton uIColoredImageButton = new UIColoredImageButton(Main.Assets.Request<Texture2D>("Images/UI/CharCreation/Copy", (AssetRequestMode)1), isSmall: true)
		{
			VAlign = 1f,
			HAlign = 0f,
			Left = StyleDimension.FromPixelsAndPercent(0f, 0f)
		};
		uIColoredImageButton.OnLeftMouseDown += Click_CopyHex;
		uIElement.Append(uIColoredImageButton);
		_copyHexButton = uIColoredImageButton;
		UIColoredImageButton uIColoredImageButton2 = new UIColoredImageButton(Main.Assets.Request<Texture2D>("Images/UI/CharCreation/Paste", (AssetRequestMode)1), isSmall: true)
		{
			VAlign = 1f,
			HAlign = 0f,
			Left = StyleDimension.FromPixelsAndPercent(40f, 0f)
		};
		uIColoredImageButton2.OnLeftMouseDown += Click_PasteHex;
		uIElement.Append(uIColoredImageButton2);
		_pasteHexButton = uIColoredImageButton2;
		UIColoredImageButton uIColoredImageButton3 = new UIColoredImageButton(Main.Assets.Request<Texture2D>("Images/UI/CharCreation/Randomize", (AssetRequestMode)1), isSmall: true)
		{
			VAlign = 1f,
			HAlign = 0f,
			Left = StyleDimension.FromPixelsAndPercent(80f, 0f)
		};
		uIColoredImageButton3.OnLeftMouseDown += Click_RandomizeSingleColor;
		uIElement.Append(uIColoredImageButton3);
		_randomColorButton = uIColoredImageButton3;
		_hslContainer = uIElement;
		_hslHexText = uIText;
		uIColoredImageButton.SetSnapPoint("Low", 0);
		uIColoredImageButton2.SetSnapPoint("Low", 1);
		uIColoredImageButton3.SetSnapPoint("Low", 2);
	}

	private void Click_VoicePitch(UIMouseEvent evt, UIElement listeningElement)
	{
		SoundEngine.PlaySound(12);
	}

	private UIColoredSlider CreateHSLSlider(HSLSliderId id)
	{
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		UIColoredSlider uIColoredSlider = CreateHSLSliderButtonBase(id);
		uIColoredSlider.VAlign = 0f;
		uIColoredSlider.HAlign = 0f;
		uIColoredSlider.Width = StyleDimension.FromPixelsAndPercent(-10f, 1f);
		uIColoredSlider.Top.Set(30 * (int)id, 0f);
		uIColoredSlider.OnLeftMouseDown += Click_ColorPicker;
		uIColoredSlider.SetSnapPoint("Middle", (int)id, null, new Vector2(0f, 20f));
		return uIColoredSlider;
	}

	private UIColoredSlider CreateHSLSliderButtonBase(HSLSliderId id)
	{
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
		return id switch
		{
			HSLSliderId.Saturation => new UIColoredSlider(LocalizedText.Empty, () => GetHSLSliderPosition(HSLSliderId.Saturation), (float x) =>
			{
				UpdateHSLValue(HSLSliderId.Saturation, x);
			}, UpdateHSL_S, (float x) =>
			{
				//IL_0003: Unknown result type (might be due to invalid IL or missing references)
				return GetHSLSliderColorAt(HSLSliderId.Saturation, x);
			}, Color.Transparent), 
			HSLSliderId.Luminance => new UIColoredSlider(LocalizedText.Empty, () => GetHSLSliderPosition(HSLSliderId.Luminance), (float x) =>
			{
				UpdateHSLValue(HSLSliderId.Luminance, x);
			}, UpdateHSL_L, (float x) =>
			{
				//IL_0003: Unknown result type (might be due to invalid IL or missing references)
				return GetHSLSliderColorAt(HSLSliderId.Luminance, x);
			}, Color.Transparent), 
			_ => new UIColoredSlider(LocalizedText.Empty, () => GetHSLSliderPosition(HSLSliderId.Hue), (float x) =>
			{
				UpdateHSLValue(HSLSliderId.Hue, x);
			}, UpdateHSL_H, (float x) =>
			{
				//IL_0003: Unknown result type (might be due to invalid IL or missing references)
				return GetHSLSliderColorAt(HSLSliderId.Hue, x);
			}, Color.Transparent), 
		};
	}

	private void UpdateHSL_H()
	{
		float value = UILinksInitializer.HandleSliderHorizontalInput(_currentColorHSL.X, 0f, 1f, PlayerInput.CurrentProfile.InterfaceDeadzoneX, 0.35f);
		UpdateHSLValue(HSLSliderId.Hue, value);
	}

	private void UpdateHSL_S()
	{
		float value = UILinksInitializer.HandleSliderHorizontalInput(_currentColorHSL.Y, 0f, 1f, PlayerInput.CurrentProfile.InterfaceDeadzoneX, 0.35f);
		UpdateHSLValue(HSLSliderId.Saturation, value);
	}

	private void UpdateHSL_L()
	{
		float value = UILinksInitializer.HandleSliderHorizontalInput(_currentColorHSL.Z, 0f, 1f, PlayerInput.CurrentProfile.InterfaceDeadzoneX, 0.35f);
		UpdateHSLValue(HSLSliderId.Luminance, value);
	}

	private float GetHSLSliderPosition(HSLSliderId id)
	{
		return id switch
		{
			HSLSliderId.Hue => _currentColorHSL.X, 
			HSLSliderId.Saturation => _currentColorHSL.Y, 
			HSLSliderId.Luminance => _currentColorHSL.Z, 
			_ => 1f, 
		};
	}

	private void UpdateHSLValue(HSLSliderId id, float value)
	{
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		switch (id)
		{
		case HSLSliderId.Hue:
			_currentColorHSL.X = value;
			break;
		case HSLSliderId.Saturation:
			_currentColorHSL.Y = value;
			break;
		case HSLSliderId.Luminance:
			_currentColorHSL.Z = value;
			break;
		}
		Color val = ScaledHslToRgb(_currentColorHSL.X, _currentColorHSL.Y, _currentColorHSL.Z);
		ApplyPendingColor(val);
		_colorPickers[(int)_selectedPicker]?.SetColor(val);
		if (_selectedPicker == CategoryId.HairColor)
		{
			_hairStylesCategoryButton.SetColor(val);
		}
		UpdateHexText(val);
	}

	private Color GetHSLSliderColorAt(HSLSliderId id, float pointAt)
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		return id switch
		{
			HSLSliderId.Hue => ScaledHslToRgb(pointAt, 1f, 0.5f), 
			HSLSliderId.Saturation => ScaledHslToRgb(_currentColorHSL.X, pointAt, _currentColorHSL.Z), 
			HSLSliderId.Luminance => ScaledHslToRgb(_currentColorHSL.X, _currentColorHSL.Y, pointAt), 
			_ => Color.White, 
		};
	}

	private void ApplyPendingColor(Color pendingColor)
	{
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		switch (_selectedPicker)
		{
		case CategoryId.HairColor:
			_player.hairColor = pendingColor;
			break;
		case CategoryId.Eye:
			_player.eyeColor = pendingColor;
			break;
		case CategoryId.Skin:
			_player.skinColor = pendingColor;
			break;
		case CategoryId.Shirt:
			_player.shirtColor = pendingColor;
			break;
		case CategoryId.Undershirt:
			_player.underShirtColor = pendingColor;
			break;
		case CategoryId.Pants:
			_player.pantsColor = pendingColor;
			break;
		case CategoryId.Shoes:
			_player.shoeColor = pendingColor;
			break;
		}
	}

	private void UpdateHexText(Color pendingColor)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		_hslHexText.SetText(GetHexText(pendingColor));
	}

	private static string GetHexText(Color pendingColor)
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		return "#" + pendingColor.Hex3().ToUpper();
	}

	private void MakeBackAndCreatebuttons(UIElement outerContainer)
	{
		UITextPanel<LocalizedText> uITextPanel = new UITextPanel<LocalizedText>(Language.GetText("UI.Back"), 0.7f, large: true)
		{
			Width = StyleDimension.FromPixelsAndPercent(-10f, 0.5f),
			Height = StyleDimension.FromPixels(50f),
			VAlign = 1f,
			HAlign = 0f,
			Top = StyleDimension.FromPixels(-45f)
		};
		uITextPanel.OnMouseOver += FadedMouseOver;
		uITextPanel.OnMouseOut += FadedMouseOut;
		uITextPanel.OnLeftMouseDown += Click_GoBack;
		uITextPanel.SetSnapPoint("Back", 0);
		outerContainer.Append(uITextPanel);
		UITextPanel<LocalizedText> uITextPanel2 = new UITextPanel<LocalizedText>(Language.GetText("UI.Create"), 0.7f, large: true)
		{
			Width = StyleDimension.FromPixelsAndPercent(-10f, 0.5f),
			Height = StyleDimension.FromPixels(50f),
			VAlign = 1f,
			HAlign = 1f,
			Top = StyleDimension.FromPixels(-45f)
		};
		uITextPanel2.OnMouseOver += FadedMouseOver;
		uITextPanel2.OnMouseOut += FadedMouseOut;
		uITextPanel2.OnLeftMouseDown += Click_NamingAndCreating;
		uITextPanel2.SetSnapPoint("Create", 0);
		outerContainer.Append(uITextPanel2);
	}

	private void Click_GoBack(UIMouseEvent evt, UIElement listeningElement)
	{
		GoBack();
	}

	private static void GoBack()
	{
		SoundEngine.PlaySound(11);
		if (dirty)
		{
			BackupConfirmationState = Main.MenuUI.CurrentState;
			Main.menuMode = 40;
		}
		else
		{
			Main.OpenCharacterSelectUI();
		}
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

	private void Click_ColorPicker(UIMouseEvent evt, UIElement listeningElement)
	{
		SoundEngine.PlaySound(12);
		for (int i = 0; i < _colorPickers.Length; i++)
		{
			if (_colorPickers[i] == evt.Target)
			{
				SelectColorPicker((CategoryId)i);
				break;
			}
		}
	}

	private void Click_ClothStyles(UIMouseEvent evt, UIElement listeningElement)
	{
		SoundEngine.PlaySound(12);
		UnselectAllCategories();
		_selectedPicker = CategoryId.Clothing;
		_middleContainer.Append(_clothStylesContainer);
		_clothingStylesCategoryButton.SetSelected(selected: true);
	}

	private void Click_HairStyles(UIMouseEvent evt, UIElement listeningElement)
	{
		SoundEngine.PlaySound(12);
		UnselectAllCategories();
		_selectedPicker = CategoryId.HairStyle;
		_middleContainer.Append(_hairstylesContainer);
		_hairStylesCategoryButton.SetSelected(selected: true);
	}

	private void Click_CharInfo(UIMouseEvent evt, UIElement listeningElement)
	{
		SoundEngine.PlaySound(12);
		UnselectAllCategories();
		_selectedPicker = CategoryId.CharInfo;
		_middleContainer.Append(_infoContainer);
		_charInfoCategoryButton.SetSelected(selected: true);
	}

	private void Click_CharClothStyle(UIMouseEvent evt, UIElement listeningElement)
	{
		if (_maleArmor.HeadItem != 0 || _maleArmor.BodyItem != 0 || _maleArmor.LegItem != 0)
		{
			EquipArmorNone(evt, listeningElement);
			return;
		}
		if (listeningElement is UIClothStyleButton { ClothStyleId: var clothStyleId })
		{
			_player.skinVariant = clothStyleId;
		}
		SoundEngine.PlaySound(12);
		_clothingStylesCategoryButton.SetImageWithoutSettingSize(Main.Assets.Request<Texture2D>("Images/UI/CharCreation/" + (_player.Male ? "ClothStyleMale" : "ClothStyleFemale"), (AssetRequestMode)1));
		UpdateSelectedGender();
	}

	private void TryChangingVoice()
	{
		if (_player.Male && _player.voiceVariant == 2)
		{
			_player.voiceVariant = 1;
		}
		if (!_player.Male && _player.voiceVariant == 1)
		{
			_player.voiceVariant = 2;
		}
	}

	private void UpdateSelectedGender()
	{
		if (_oldMaleForVoiceAutoSwitch == _player.Male)
		{
			PlayVoicePreview();
			return;
		}
		switch (_player.voiceVariant)
		{
		case 1:
			if (_oldMaleForVoiceAutoSwitch)
			{
				_player.voiceVariant = 2;
			}
			break;
		case 2:
			if (!_oldMaleForVoiceAutoSwitch)
			{
				_player.voiceVariant = 1;
			}
			break;
		}
		_oldMaleForVoiceAutoSwitch = _player.Male;
		PlayVoicePreview();
	}

	private void Click_CopyHex(UIMouseEvent evt, UIElement listeningElement)
	{
		SoundEngine.PlaySound(12);
		Platform.Get<IClipboard>().Value = _hslHexText.Text;
	}

	private void Click_PasteHex(UIMouseEvent evt, UIElement listeningElement)
	{
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		SoundEngine.PlaySound(12);
		string value = Platform.Get<IClipboard>().Value;
		if (GetHexColor(value, out var hsl))
		{
			ApplyPendingColor(ScaledHslToRgb(hsl.X, hsl.Y, hsl.Z));
			_currentColorHSL = hsl;
			UpdateHexText(ScaledHslToRgb(hsl.X, hsl.Y, hsl.Z));
			UpdateColorPickers();
		}
	}

	private string GetPlayerTemplateValues()
	{
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0115: Unknown result type (might be due to invalid IL or missing references)
		//IL_0130: Unknown result type (might be due to invalid IL or missing references)
		//IL_013f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0144: Unknown result type (might be due to invalid IL or missing references)
		//IL_014b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0152: Unknown result type (might be due to invalid IL or missing references)
		//IL_015e: Expected Obj, but got Unknown
		string text = JsonConvert.SerializeObject((object)new Dictionary<string, object>
		{
			{ "version", 1 },
			{ "hairStyle", _player.hair },
			{ "clothingStyle", _player.skinVariant },
			{ "voiceStyle", _player.voiceVariant },
			{ "voicePitch", _player.voicePitchOffset },
			{
				"hairColor",
				GetHexText(_player.hairColor)
			},
			{
				"eyeColor",
				GetHexText(_player.eyeColor)
			},
			{
				"skinColor",
				GetHexText(_player.skinColor)
			},
			{
				"shirtColor",
				GetHexText(_player.shirtColor)
			},
			{
				"underShirtColor",
				GetHexText(_player.underShirtColor)
			},
			{
				"pantsColor",
				GetHexText(_player.pantsColor)
			},
			{
				"shoeColor",
				GetHexText(_player.shoeColor)
			}
		}, new JsonSerializerSettings
		{
			TypeNameHandling = (TypeNameHandling)4,
			MetadataPropertyHandling = (MetadataPropertyHandling)1,
			Formatting = (Formatting)1
		});
		PlayerInput.PrettyPrintProfiles(ref text);
		return text;
	}

	private void Click_CopyPlayerTemplate(UIMouseEvent evt, UIElement listeningElement)
	{
		SoundEngine.PlaySound(12);
		Platform.Get<IClipboard>().Value = GetPlayerTemplateValues();
	}

	private void Click_PastePlayerTemplate(UIMouseEvent evt, UIElement listeningElement)
	{
		//IL_01dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01de: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_020f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0210: Unknown result type (might be due to invalid IL or missing references)
		//IL_0215: Unknown result type (might be due to invalid IL or missing references)
		//IL_0241: Unknown result type (might be due to invalid IL or missing references)
		//IL_0242: Unknown result type (might be due to invalid IL or missing references)
		//IL_0247: Unknown result type (might be due to invalid IL or missing references)
		//IL_0273: Unknown result type (might be due to invalid IL or missing references)
		//IL_0274: Unknown result type (might be due to invalid IL or missing references)
		//IL_0279: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0309: Unknown result type (might be due to invalid IL or missing references)
		//IL_030a: Unknown result type (might be due to invalid IL or missing references)
		//IL_030f: Unknown result type (might be due to invalid IL or missing references)
		SoundEngine.PlaySound(12);
		try
		{
			string value = Platform.Get<IClipboard>().Value;
			int num = value.IndexOf("{");
			if (num == -1)
			{
				return;
			}
			value = value.Substring(num);
			int num2 = value.LastIndexOf("}");
			if (num2 == -1)
			{
				return;
			}
			value = value.Substring(0, num2 + 1);
			Dictionary<string, object> dictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(value);
			if (dictionary == null)
			{
				return;
			}
			Dictionary<string, object> dictionary2 = new Dictionary<string, object>();
			foreach (KeyValuePair<string, object> item in dictionary)
			{
				dictionary2[item.Key.ToLower()] = item.Value;
			}
			if (dictionary2.TryGetValue("version", out var value2))
			{
				_ = (long)value2;
			}
			if (dictionary2.TryGetValue("hairstyle", out value2))
			{
				int num3 = (int)(long)value2;
				if (Main.Hairstyles.AvailableHairstyles.Contains(num3))
				{
					_player.hair = num3;
					_lastSelectedHairstyle = num3;
				}
			}
			if (dictionary2.TryGetValue("clothingstyle", out value2))
			{
				int num4 = (int)(long)value2;
				if (_validClothStyles.Contains(num4))
				{
					_player.skinVariant = num4;
				}
			}
			if (dictionary2.TryGetValue("voicestyle", out value2))
			{
				int num5 = (int)(long)value2;
				if (_validVoiceStyles.Contains(num5))
				{
					_player.voiceVariant = num5;
				}
			}
			if (dictionary2.TryGetValue("voicepitch", out value2))
			{
				float num6 = (float)(double)value2;
				_player.voicePitchOffset = num6;
				_pitchAmount = num6;
			}
			if (dictionary2.TryGetValue("haircolor", out value2) && GetHexColor((string)value2, out var hsl))
			{
				_player.hairColor = ScaledHslToRgb(hsl);
			}
			if (dictionary2.TryGetValue("eyecolor", out value2) && GetHexColor((string)value2, out hsl))
			{
				_player.eyeColor = ScaledHslToRgb(hsl);
			}
			if (dictionary2.TryGetValue("skincolor", out value2) && GetHexColor((string)value2, out hsl))
			{
				_player.skinColor = ScaledHslToRgb(hsl);
			}
			if (dictionary2.TryGetValue("shirtcolor", out value2) && GetHexColor((string)value2, out hsl))
			{
				_player.shirtColor = ScaledHslToRgb(hsl);
			}
			if (dictionary2.TryGetValue("undershirtcolor", out value2) && GetHexColor((string)value2, out hsl))
			{
				_player.underShirtColor = ScaledHslToRgb(hsl);
			}
			if (dictionary2.TryGetValue("pantscolor", out value2) && GetHexColor((string)value2, out hsl))
			{
				_player.pantsColor = ScaledHslToRgb(hsl);
			}
			if (dictionary2.TryGetValue("shoecolor", out value2) && GetHexColor((string)value2, out hsl))
			{
				_player.shoeColor = ScaledHslToRgb(hsl);
			}
			Click_CharClothStyle(null, null);
			UpdateColorPickers();
		}
		catch
		{
		}
	}

	private void Click_VoicePlay(UIMouseEvent evt, UIElement listeningElement)
	{
		PlayVoicePreview();
	}

	private void PlayVoicePreview()
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		if (!_playedVoicePreviewThisFrame)
		{
			_playedVoicePreviewThisFrame = true;
			Vector2 position = _player.position;
			_player.position = new Vector2(-1f, -1f);
			_player.PlayHurtSound();
			_player.position = position;
		}
	}

	private void Click_VoiceCycleBack(UIMouseEvent evt, UIElement listeningElement)
	{
		Main.CycleVoiceStyle(_player, -1);
		PlayVoicePreview();
	}

	private void Click_VoiceCycleForward(UIMouseEvent evt, UIElement listeningElement)
	{
		Main.CycleVoiceStyle(_player, 1);
		PlayVoicePreview();
	}

	private void Update_VoiceIconColor()
	{
	}

	private void Click_RandomizePlayer(UIMouseEvent evt, UIElement listeningElement)
	{
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_012e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0133: Unknown result type (might be due to invalid IL or missing references)
		//IL_0138: Unknown result type (might be due to invalid IL or missing references)
		//IL_013e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0143: Unknown result type (might be due to invalid IL or missing references)
		//IL_0148: Unknown result type (might be due to invalid IL or missing references)
		//IL_014e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0153: Unknown result type (might be due to invalid IL or missing references)
		//IL_0158: Unknown result type (might be due to invalid IL or missing references)
		//IL_015e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0163: Unknown result type (might be due to invalid IL or missing references)
		//IL_0168: Unknown result type (might be due to invalid IL or missing references)
		//IL_016e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0173: Unknown result type (might be due to invalid IL or missing references)
		//IL_0178: Unknown result type (might be due to invalid IL or missing references)
		SoundEngine.PlaySound(12);
		Player player = _player;
		int index = Main.rand.Next(Main.Hairstyles.AvailableHairstyles.Count);
		player.hair = Main.Hairstyles.AvailableHairstyles[index];
		_lastSelectedHairstyle = player.hair;
		player.eyeColor = ScaledHslToRgb(GetRandomColorVector());
		while (player.eyeColor.R + player.eyeColor.G + player.eyeColor.B > 300)
		{
			player.eyeColor = ScaledHslToRgb(GetRandomColorVector());
		}
		float num = (float)Main.rand.Next(60, 120) * 0.01f;
		if (num > 1f)
		{
			num = 1f;
		}
		player.skinColor.R = (byte)((float)Main.rand.Next(240, 255) * num);
		player.skinColor.G = (byte)((float)Main.rand.Next(110, 140) * num);
		player.skinColor.B = (byte)((float)Main.rand.Next(75, 110) * num);
		player.hairColor = ScaledHslToRgb(GetRandomColorVector());
		player.shirtColor = ScaledHslToRgb(GetRandomColorVector());
		player.underShirtColor = ScaledHslToRgb(GetRandomColorVector());
		player.pantsColor = ScaledHslToRgb(GetRandomColorVector());
		player.shoeColor = ScaledHslToRgb(GetRandomColorVector());
		player.skinVariant = _validClothStyles[Main.rand.Next(_validClothStyles.Length)];
		player.voiceVariant = (player.Male ? 1 : 2);
		if (Main.rand.Next(2) == 0)
		{
			player.voiceVariant = 3;
		}
		switch (player.hair + 1)
		{
		case 5:
		case 6:
		case 7:
		case 10:
		case 12:
		case 19:
		case 22:
		case 23:
		case 26:
		case 27:
		case 30:
		case 33:
		case 34:
		case 35:
		case 37:
		case 38:
		case 39:
		case 40:
		case 41:
		case 44:
		case 45:
		case 46:
		case 47:
		case 48:
		case 49:
		case 51:
		case 56:
		case 65:
		case 66:
		case 67:
		case 68:
		case 69:
		case 70:
		case 71:
		case 72:
		case 73:
		case 74:
		case 79:
		case 80:
		case 81:
		case 82:
		case 84:
		case 85:
		case 86:
		case 87:
		case 88:
		case 90:
		case 91:
		case 92:
		case 93:
		case 95:
		case 96:
		case 98:
		case 100:
		case 102:
		case 104:
		case 107:
		case 108:
		case 113:
		case 124:
		case 126:
		case 133:
		case 134:
		case 135:
		case 144:
		case 146:
		case 147:
		case 163:
		case 165:
			player.Male = false;
			break;
		default:
			player.Male = true;
			break;
		}
		_femaleArmor = (_maleArmor = default);
		Click_CharClothStyle(null, null);
		UpdateSelectedGender();
		UpdateColorPickers();
	}

	private void Click_Naming(UIMouseEvent evt, UIElement listeningElement)
	{
		SoundEngine.PlaySound(10);
		_player.name = "";
		Main.clrInput();
		UIVirtualKeyboard state = new UIVirtualKeyboard(Lang.menu[45].Value, "", OnFinishedNaming, OnCanceledNaming, 0, allowEmpty: true);
		Main.MenuUI.SetState(state);
	}

	private void Click_NamingAndCreating(UIMouseEvent evt, UIElement listeningElement)
	{
		SoundEngine.PlaySound(10);
		if (string.IsNullOrEmpty(_player.name))
		{
			_player.name = "";
			Main.clrInput();
			UIVirtualKeyboard state = new UIVirtualKeyboard(Lang.menu[45].Value, "", OnFinishedNamingAndCreating, OnCanceledNaming);
			Main.MenuUI.SetState(state);
		}
		else
		{
			FinishCreatingCharacter();
		}
	}

	private void OnFinishedNaming(string name)
	{
		_player.name = name.Trim();
		Main.MenuUI.SetState(this);
		_charName.SetContents(_player.name);
	}

	private void OnCanceledNaming()
	{
		Main.MenuUI.SetState(this);
	}

	private void OnFinishedNamingAndCreating(string name)
	{
		_player.name = name.Trim();
		Main.MenuUI.SetState(this);
		_charName.SetContents(_player.name);
		FinishCreatingCharacter();
	}

	private void FinishCreatingCharacter()
	{
		TryAutoAssigningHair();
		SetupPlayerStatsAndInventoryBasedOnDifficulty();
		PlayerFileData.CreateAndSave(_player);
		Main.LoadPlayers();
		Main.menuMode = 1;
	}

	private void SetupPlayerStatsAndInventoryBasedOnDifficulty()
	{
		_femaleArmor = (_maleArmor = default);
		UpdatePreviewItems();
		int num = 0;
		byte difficulty = _player.difficulty;
		if (difficulty == 3)
		{
			_player.statLife = (_player.statLifeMax = 100);
			_player.statMana = (_player.statManaMax = 20);
			_player.inventory[num].SetDefaults(6);
			_player.inventory[num++].Prefix(-1);
			_player.inventory[num].SetDefaults(1);
			_player.inventory[num++].Prefix(-1);
			_player.inventory[num].SetDefaults(10);
			_player.inventory[num++].Prefix(-1);
			_player.inventory[num].SetDefaults(7);
			_player.inventory[num++].Prefix(-1);
			_player.inventory[num].SetDefaults(4281);
			_player.inventory[num++].Prefix(-1);
			_player.inventory[num].SetDefaults(8);
			_player.inventory[num++].stack = 100;
			_player.inventory[num].SetDefaults(965);
			_player.inventory[num++].stack = 100;
			_player.inventory[num++].SetDefaults(50);
			_player.inventory[num++].SetDefaults(84);
			_player.armor[3].SetDefaults(4978);
			_player.armor[3].Prefix(-1);
			string text = _player.name.ToLower();
			if (text == "wolf pet" || text == "wolfpet")
			{
				_player.miscEquips[3].SetDefaults(5130);
			}
			_player.AddBuff(216, 3600);
		}
		else
		{
			_player.inventory[num].SetDefaults(3507);
			_player.inventory[num++].Prefix(-1);
			_player.inventory[num].SetDefaults(3509);
			_player.inventory[num++].Prefix(-1);
			_player.inventory[num].SetDefaults(3506);
			_player.inventory[num++].Prefix(-1);
		}
		if (Main.runningCollectorsEdition)
		{
			_player.inventory[num++].SetDefaults(603);
		}
		_player.savedPerPlayerFieldsThatArentInThePlayerClass = new Player.SavedPlayerDataWithAnnoyingRules();
		CreativePowerManager.Instance.ResetDataForNewPlayer(_player);
	}

	private bool GetHexColor(string hexString, out Vector3 hsl)
	{
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		if (hexString.StartsWith("#"))
		{
			hexString = hexString.Substring(1);
		}
		if (hexString.Length <= 6 && uint.TryParse(hexString, NumberStyles.HexNumber, CultureInfo.CurrentCulture, out var result))
		{
			uint num = result & 0xFF;
			uint num2 = (result >> 8) & 0xFF;
			uint num3 = (result >> 16) & 0xFF;
			hsl = RgbToScaledHsl(new Color((int)num3, (int)num2, (int)num));
			return true;
		}
		hsl = Vector3.Zero;
		return false;
	}

	private void Click_RandomizeSingleColor(UIMouseEvent evt, UIElement listeningElement)
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		SoundEngine.PlaySound(12);
		Vector3 randomColorVector = GetRandomColorVector();
		ApplyPendingColor(ScaledHslToRgb(randomColorVector.X, randomColorVector.Y, randomColorVector.Z));
		_currentColorHSL = randomColorVector;
		UpdateHexText(ScaledHslToRgb(randomColorVector.X, randomColorVector.Y, randomColorVector.Z));
		UpdateColorPickers();
	}

	private static Vector3 GetRandomColorVector()
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		return new Vector3(Main.rand.NextFloat(), Main.rand.NextFloat(), Main.rand.NextFloat());
	}

	private void UnselectAllCategories()
	{
		UIColoredImageButton[] colorPickers = _colorPickers;
		for (int i = 0; i < colorPickers.Length; i++)
		{
			colorPickers[i]?.SetSelected(selected: false);
		}
		_clothingStylesCategoryButton.SetSelected(selected: false);
		_hairStylesCategoryButton.SetSelected(selected: false);
		_charInfoCategoryButton.SetSelected(selected: false);
		_hslContainer.Remove();
		_hairstylesContainer.Remove();
		_clothStylesContainer.Remove();
		_infoContainer.Remove();
	}

	private void SelectColorPicker(CategoryId selection)
	{
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0104: Unknown result type (might be due to invalid IL or missing references)
		//IL_010d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0112: Unknown result type (might be due to invalid IL or missing references)
		//IL_0117: Unknown result type (might be due to invalid IL or missing references)
		//IL_0120: Unknown result type (might be due to invalid IL or missing references)
		//IL_0125: Unknown result type (might be due to invalid IL or missing references)
		//IL_012a: Unknown result type (might be due to invalid IL or missing references)
		//IL_012c: Unknown result type (might be due to invalid IL or missing references)
		//IL_012d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0133: Unknown result type (might be due to invalid IL or missing references)
		//IL_0139: Unknown result type (might be due to invalid IL or missing references)
		//IL_013f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0145: Unknown result type (might be due to invalid IL or missing references)
		_selectedPicker = selection;
		switch (selection)
		{
		case CategoryId.CharInfo:
			Click_CharInfo(null, null);
			return;
		case CategoryId.Clothing:
			Click_ClothStyles(null, null);
			return;
		case CategoryId.HairStyle:
			Click_HairStyles(null, null);
			return;
		}
		UnselectAllCategories();
		_middleContainer.Append(_hslContainer);
		for (int i = 0; i < _colorPickers.Length; i++)
		{
			if (_colorPickers[i] != null)
			{
				_colorPickers[i].SetSelected(i == (int)selection);
			}
		}
		Vector3 val = Vector3.One;
		switch (_selectedPicker)
		{
		case CategoryId.HairColor:
			val = RgbToScaledHsl(_player.hairColor);
			break;
		case CategoryId.Eye:
			val = RgbToScaledHsl(_player.eyeColor);
			break;
		case CategoryId.Skin:
			val = RgbToScaledHsl(_player.skinColor);
			break;
		case CategoryId.Shirt:
			val = RgbToScaledHsl(_player.shirtColor);
			break;
		case CategoryId.Undershirt:
			val = RgbToScaledHsl(_player.underShirtColor);
			break;
		case CategoryId.Pants:
			val = RgbToScaledHsl(_player.pantsColor);
			break;
		case CategoryId.Shoes:
			val = RgbToScaledHsl(_player.shoeColor);
			break;
		}
		_currentColorHSL = val;
		UpdateHexText(ScaledHslToRgb(val.X, val.Y, val.Z));
	}

	private void UpdateColorPickers()
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		_ = _selectedPicker;
		_colorPickers[3].SetColor(_player.hairColor);
		_hairStylesCategoryButton.SetColor(_player.hairColor);
		_colorPickers[4].SetColor(_player.eyeColor);
		_colorPickers[5].SetColor(_player.skinColor);
		_colorPickers[6].SetColor(_player.shirtColor);
		_colorPickers[7].SetColor(_player.underShirtColor);
		_colorPickers[8].SetColor(_player.pantsColor);
		_colorPickers[9].SetColor(_player.shoeColor);
	}

	public override void Draw(SpriteBatch spriteBatch)
	{
		//IL_028b: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02da: Unknown result type (might be due to invalid IL or missing references)
		//IL_030a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0311: Unknown result type (might be due to invalid IL or missing references)
		//IL_032c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0331: Unknown result type (might be due to invalid IL or missing references)
		//IL_0336: Unknown result type (might be due to invalid IL or missing references)
		base.Draw(spriteBatch);
		string text = null;
		if (_copyHexButton.IsMouseHovering)
		{
			text = Language.GetTextValue("UI.CopyColorToClipboard");
		}
		if (_pasteHexButton.IsMouseHovering)
		{
			text = Language.GetTextValue("UI.PasteColorFromClipboard");
		}
		if (_randomColorButton.IsMouseHovering)
		{
			text = Language.GetTextValue("UI.RandomizeColor");
		}
		if (_copyTemplateButton.IsMouseHovering)
		{
			text = Language.GetTextValue("UI.CopyPlayerToClipboard");
		}
		if (_pasteTemplateButton.IsMouseHovering)
		{
			text = Language.GetTextValue("UI.PastePlayerFromClipboard");
		}
		if (_randomizePlayerButton.IsMouseHovering)
		{
			text = Language.GetTextValue("UI.RandomizePlayer");
		}
		if (_previewArmorButton[0].IsMouseHovering)
		{
			text = Language.GetTextValue("UI.PreviewArmorNone");
		}
		if (_previewArmorButton[1].IsMouseHovering)
		{
			text = Language.GetTextValue("UI.PreviewArmorHallowed");
		}
		if (_previewArmorButton[2].IsMouseHovering)
		{
			text = Language.GetTextValue("UI.PreviewArmorSilver");
		}
		if (_previewArmorButton[3].IsMouseHovering)
		{
			text = Language.GetTextValue("UI.PreviewArmorFormal");
		}
		if (_previewArmorButton[4].IsMouseHovering)
		{
			text = Language.GetTextValue("UI.PreviewArmorSwimming");
		}
		if (UISliderBase.CurrentAimedSlider == _pitchSlider)
		{
			text = Language.GetTextValue("UI.PlayerCreateVoicePitch");
		}
		if (_voicePrevious.IsMouseHovering)
		{
			text = Language.GetTextValue("UI.PlayerCreateVoicePrev");
		}
		if (_voiceNext.IsMouseHovering)
		{
			text = Language.GetTextValue("UI.PlayerCreateVoiceNext");
		}
		if (_voicePlay.IsMouseHovering)
		{
			text = Language.GetTextValue("UI.PlayerCreateVoicePlay");
		}
		if (_charInfoCategoryButton.IsMouseHovering)
		{
			text = Language.GetTextValue("UI.PlayerCreateCategoryInfo");
		}
		if (_clothingStylesCategoryButton.IsMouseHovering)
		{
			text = Language.GetTextValue("UI.PlayerCreateCategoryBodyStyle");
		}
		if (_hairStylesCategoryButton.IsMouseHovering)
		{
			text = Language.GetTextValue("UI.PlayerCreateCategoryHairStyle");
		}
		if (_colorPickers[3].IsMouseHovering)
		{
			text = Language.GetTextValue("UI.PlayerCreateCategoryHairColor");
		}
		if (_colorPickers[4].IsMouseHovering)
		{
			text = Language.GetTextValue("UI.PlayerCreateCategoryEyeColor");
		}
		if (_colorPickers[5].IsMouseHovering)
		{
			text = Language.GetTextValue("UI.PlayerCreateCategorySkinColor");
		}
		if (_colorPickers[6].IsMouseHovering)
		{
			text = Language.GetTextValue("UI.PlayerCreateCategoryShirtColor");
		}
		if (_colorPickers[7].IsMouseHovering)
		{
			text = Language.GetTextValue("UI.PlayerCreateCategoryUndershirtColor");
		}
		if (_colorPickers[8].IsMouseHovering)
		{
			text = Language.GetTextValue("UI.PlayerCreateCategoryPantsColor");
		}
		if (_colorPickers[9].IsMouseHovering)
		{
			text = Language.GetTextValue("UI.PlayerCreateCategoryShoesColor");
		}
		if (text != null)
		{
			float x = FontAssets.MouseText.Value.MeasureString(text).X;
			Vector2 val = new Vector2((float)Main.mouseX, (float)Main.mouseY) + new Vector2(16f);
			if (val.Y > (float)(Main.screenHeight - 30))
			{
				val.Y = Main.screenHeight - 30;
			}
			if (val.X > (float)Main.screenWidth - x)
			{
				val.X = Main.screenWidth - 460;
			}
			Utils.DrawBorderStringFourWay(spriteBatch, FontAssets.MouseText.Value, text, val.X, val.Y, new Color((int)Main.mouseTextColor, (int)Main.mouseTextColor, (int)Main.mouseTextColor, (int)Main.mouseTextColor), Color.Black, Vector2.Zero);
		}
		SetupGamepadPoints(spriteBatch);
		_tips.Update();
		int num = Main.screenHeight - 560;
		if (num < 0)
		{
			num = 0;
		}
		int num2 = 150;
		if (num < 300)
		{
			num2 = num / 2;
		}
		if (num > 30)
		{
			_tips.TipOffsetY = -num2;
			_tips.Draw();
		}
		if (!dirty)
		{
			if (!string.IsNullOrEmpty(_player.name))
			{
				dirty = true;
			}
			if (GetPlayerTemplateValues() != initialState)
			{
				dirty = true;
			}
		}
	}

	private void SetupGamepadPoints(SpriteBatch spriteBatch)
	{
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0164: Unknown result type (might be due to invalid IL or missing references)
		UILinkPointNavigator.Shortcuts.BackButtonCommand = 7;
		int num = 3000;
		int num2 = num + 20;
		int num3 = num;
		List<SnapPoint> snapPoints = GetSnapPoints();
		SnapPoint snapPoint = snapPoints.First((SnapPoint a) => a.Name == "Back");
		SnapPoint snapPoint2 = snapPoints.First((SnapPoint a) => a.Name == "Create");
		UILinkPoint uILinkPoint = UILinkPointNavigator.Points[num3];
		uILinkPoint.Unlink();
		UILinkPointNavigator.SetPosition(num3, snapPoint.Position);
		num3++;
		UILinkPoint uILinkPoint2 = UILinkPointNavigator.Points[num3];
		uILinkPoint2.Unlink();
		UILinkPointNavigator.SetPosition(num3, snapPoint2.Position);
		num3++;
		uILinkPoint.Right = uILinkPoint2.ID;
		uILinkPoint2.Left = uILinkPoint.ID;
		_foundPoints.Clear();
		_foundPoints.Add(uILinkPoint.ID);
		_foundPoints.Add(uILinkPoint2.ID);
		List<SnapPoint> list = snapPoints.Where((SnapPoint a) => a.Name == "Top").ToList();
		list.Sort(SortPoints);
		for (int num4 = 0; num4 < list.Count; num4++)
		{
			UILinkPoint uILinkPoint3 = UILinkPointNavigator.Points[num3];
			uILinkPoint3.Unlink();
			UILinkPointNavigator.SetPosition(num3, list[num4].Position);
			uILinkPoint3.Left = num3 - 1;
			uILinkPoint3.Right = num3 + 1;
			uILinkPoint3.Down = num2;
			if (num4 == 0)
			{
				uILinkPoint3.Left = -3;
			}
			if (num4 == list.Count - 1)
			{
				uILinkPoint3.Right = -4;
			}
			if (_selectedPicker == CategoryId.HairStyle || _selectedPicker == CategoryId.Clothing)
			{
				uILinkPoint3.Down = num2 + num4;
			}
			_foundPoints.Add(num3);
			num3++;
		}
		List<SnapPoint> list2 = snapPoints.Where((SnapPoint a) => a.Name == "Middle").ToList();
		list2.Sort(SortPoints);
		num3 = num2;
		switch (_selectedPicker)
		{
		case CategoryId.CharInfo:
		{
			for (int num13 = 0; num13 < list2.Count; num13++)
			{
				UILinkPoint andSet7 = GetAndSet(num3, list2[num13]);
				andSet7.Up = andSet7.ID - 1;
				andSet7.Down = andSet7.ID + 1;
				if (num13 == 0)
				{
					andSet7.Up = num + 2;
				}
				if (num13 == list2.Count - 1)
				{
					andSet7.Down = uILinkPoint.ID;
					uILinkPoint.Up = andSet7.ID;
					uILinkPoint2.Up = andSet7.ID;
				}
				_foundPoints.Add(num3);
				num3++;
			}
			break;
		}
		case CategoryId.HairStyle:
		{
			if (list2.Count == 0)
			{
				break;
			}
			_helper.CullPointsOutOfElementArea(spriteBatch, list2, _hairstylesContainer);
			SnapPoint snapPoint3 = list2[list2.Count - 1];
			_ = snapPoint3.Id / 10;
			_ = snapPoint3.Id % 10;
			int count = Main.Hairstyles.AvailableHairstyles.Count;
			for (int num12 = 0; num12 < list2.Count; num12++)
			{
				SnapPoint snapPoint4 = list2[num12];
				UILinkPoint andSet6 = GetAndSet(num3, snapPoint4);
				andSet6.Left = andSet6.ID - 1;
				if (snapPoint4.Id == 0)
				{
					andSet6.Left = -3;
				}
				andSet6.Right = andSet6.ID + 1;
				if (snapPoint4.Id == count - 1)
				{
					andSet6.Right = -4;
				}
				andSet6.Up = andSet6.ID - 10;
				if (num12 < 10)
				{
					andSet6.Up = num + 2 + num12;
				}
				andSet6.Down = andSet6.ID + 10;
				if (snapPoint4.Id + 10 > snapPoint3.Id)
				{
					if (snapPoint4.Id % 10 < 5)
					{
						andSet6.Down = uILinkPoint.ID;
					}
					else
					{
						andSet6.Down = uILinkPoint2.ID;
					}
				}
				if (num12 == list2.Count - 1)
				{
					uILinkPoint.Up = andSet6.ID;
					uILinkPoint2.Up = andSet6.ID;
				}
				_foundPoints.Add(num3);
				num3++;
			}
			break;
		}
		default:
		{
			List<SnapPoint> list5 = snapPoints.Where((SnapPoint a) => a.Name == "Low").ToList();
			list5.Sort(SortPoints);
			num3 = num2 + 20;
			for (int num10 = 0; num10 < list5.Count; num10++)
			{
				UILinkPoint andSet4 = GetAndSet(num3, list5[num10]);
				andSet4.Up = num2 + 2;
				andSet4.Down = uILinkPoint.ID;
				andSet4.Left = andSet4.ID - 1;
				andSet4.Right = andSet4.ID + 1;
				if (num10 == 0)
				{
					andSet4.Left = andSet4.ID + 2;
					uILinkPoint.Up = andSet4.ID;
				}
				if (num10 == list5.Count - 1)
				{
					andSet4.Right = andSet4.ID - 2;
					uILinkPoint2.Up = andSet4.ID;
				}
				_foundPoints.Add(num3);
				num3++;
			}
			num3 = num2;
			for (int num11 = 0; num11 < list2.Count; num11++)
			{
				UILinkPoint andSet5 = GetAndSet(num3, list2[num11]);
				andSet5.Up = andSet5.ID - 1;
				andSet5.Down = andSet5.ID + 1;
				if (num11 == 0)
				{
					andSet5.Up = num + 2 + 5;
				}
				if (num11 == list2.Count - 1)
				{
					andSet5.Down = num2 + 20 + 2;
				}
				_foundPoints.Add(num3);
				num3++;
			}
			break;
		}
		case CategoryId.Clothing:
		{
			List<SnapPoint> list3 = snapPoints.Where((SnapPoint a) => a.Name == "Preview").ToList();
			list3.Sort(SortPoints);
			List<SnapPoint> list4 = snapPoints.Where((SnapPoint a) => a.Name == "Low").ToList();
			list4.Sort(SortPoints);
			int down = -2;
			SnapPoint point = null;
			UILinkPoint uILinkPoint4 = null;
			if (_pitchSlider.GetSnapPoint(out point))
			{
				uILinkPoint4 = GetAndSet(num2 + 40, point);
				_foundPoints.Add(uILinkPoint4.ID);
			}
			uILinkPoint4.Down = uILinkPoint.ID;
			int num5 = num2 + 20;
			num3 = num2 + 20;
			int num6 = num3 + list4.Count;
			UILinkPoint uILinkPoint5 = null;
			for (int num7 = 0; num7 < list4.Count; num7++)
			{
				UILinkPoint andSet = GetAndSet(num3, list4[num7]);
				andSet.Up = num2 + num7 + 2;
				andSet.Down = uILinkPoint4.ID;
				if (num7 >= 3)
				{
					andSet.Up = num6 + (num7 - 3) + 1;
					andSet.Down = uILinkPoint2.ID;
				}
				andSet.Left = andSet.ID - 1;
				andSet.Right = andSet.ID + 1;
				if (num7 == 0)
				{
					down = andSet.ID;
					andSet.Left = andSet.ID + 5;
					uILinkPoint.Up = andSet.ID;
				}
				if (num7 == list4.Count - 1)
				{
					_ = andSet.ID;
					andSet.Right = andSet.ID - 5;
					uILinkPoint2.Up = andSet.ID;
				}
				if (num7 == 1)
				{
					uILinkPoint5 = andSet;
				}
				_foundPoints.Add(num3);
				num3++;
			}
			for (int num8 = 0; num8 < list3.Count; num8++)
			{
				UILinkPoint andSet2 = GetAndSet(num3, list3[num8]);
				andSet2.Up = num2 + num8 + 5;
				andSet2.Down = num5 + ((int)MathHelper.Clamp((float)num8, 1f, 4f) - 1) + 3;
				andSet2.Left = andSet2.ID - 1;
				andSet2.Right = andSet2.ID + 1;
				if (num8 == 0)
				{
					andSet2.Left = num5 + 2;
				}
				if (num8 == list3.Count - 1)
				{
					andSet2.Right = num5;
				}
				_foundPoints.Add(num3);
				num3++;
			}
			if (list4.Count > 1)
			{
				uILinkPoint4.Up = uILinkPoint5.ID;
			}
			uILinkPoint.Up = uILinkPoint4.ID;
			num3 = num2;
			for (int num9 = 0; num9 < list2.Count; num9++)
			{
				UILinkPoint andSet3 = GetAndSet(num3, list2[num9]);
				andSet3.Up = num + 2 + num9;
				andSet3.Left = andSet3.ID - 1;
				andSet3.Right = andSet3.ID + 1;
				if (num9 == 0)
				{
					andSet3.Left = andSet3.ID + 9;
				}
				if (num9 == list2.Count - 1)
				{
					andSet3.Right = andSet3.ID - 9;
				}
				andSet3.Down = down;
				if (num9 >= 5)
				{
					andSet3.Down = num6 + num9 - 5;
				}
				_foundPoints.Add(num3);
				num3++;
			}
			break;
		}
		}
		if (PlayerInput.UsingGamepadUI && !_foundPoints.Contains(UILinkPointNavigator.CurrentPoint))
		{
			MoveToVisuallyClosestPoint();
		}
	}

	private void MoveToVisuallyClosestPoint()
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		Dictionary<int, UILinkPoint> points = UILinkPointNavigator.Points;
		Vector2 mouseScreen = Main.MouseScreen;
		UILinkPoint uILinkPoint = null;
		foreach (int foundPoint in _foundPoints)
		{
			if (!points.TryGetValue(foundPoint, out var value))
			{
				return;
			}
			if (uILinkPoint == null || Vector2.Distance(mouseScreen, uILinkPoint.Position) > Vector2.Distance(mouseScreen, value.Position))
			{
				uILinkPoint = value;
			}
		}
		if (uILinkPoint != null)
		{
			UILinkPointNavigator.ChangePoint(uILinkPoint.ID);
		}
	}

	public void TryMovingCategory(int direction)
	{
		int num = (int)(_selectedPicker + direction) % 10;
		if (num < 0)
		{
			num += 10;
		}
		SelectColorPicker((CategoryId)num);
	}

	private UILinkPoint GetAndSet(int ptid, SnapPoint snap)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		UILinkPoint uILinkPoint = UILinkPointNavigator.Points[ptid];
		uILinkPoint.Unlink();
		UILinkPointNavigator.SetPosition(uILinkPoint.ID, snap.Position);
		return uILinkPoint;
	}

	private bool PointWithName(SnapPoint a, string comp)
	{
		return a.Name == comp;
	}

	private int SortPoints(SnapPoint a, SnapPoint b)
	{
		return a.Id.CompareTo(b.Id);
	}

	private static Color ScaledHslToRgb(Vector3 hsl)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		return ScaledHslToRgb(hsl.X, hsl.Y, hsl.Z);
	}

	private static Color ScaledHslToRgb(float hue, float saturation, float luminosity)
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		return Main.hslToRgb(hue, saturation, luminosity * 0.85f + 0.15f);
	}

	private static Vector3 RgbToScaledHsl(Color color)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		Vector3 val = Main.rgbToHsl(color);
		val.Z = (val.Z - 0.15f) / 0.85f;
		return Vector3.Clamp(val, Vector3.Zero, Vector3.One);
	}

	public void HandleBackButtonUsage()
	{
		if (_selectedPicker != CategoryId.CharInfo)
		{
			SoundEngine.PlaySound(12);
			UnselectAllCategories();
			_selectedPicker = CategoryId.CharInfo;
			_middleContainer.Append(_infoContainer);
			_charInfoCategoryButton.SetSelected(selected: true);
		}
		else
		{
			GoBack();
		}
	}
}
