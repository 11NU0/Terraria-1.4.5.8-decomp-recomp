using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using ReLogic.Graphics;
using Terraria.Audio;
using Terraria.GameContent.Drawing;
using Terraria.GameContent.UI.Elements;
using Terraria.Graphics;
using Terraria.Graphics.Renderers;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.IO;
using Terraria.Localization;
using Terraria.Social;
using Terraria.Testing;
using Terraria.UI;
using Terraria.UI.Gamepad;
using Terraria.WorldBuilding;

namespace Terraria.GameContent.UI.States;

public class UIWorldCreation : UIState, IHaveBackButtonCommand
{
	private enum WorldSizeId
	{
		Small,
		Medium,
		Large
	}

	private enum WorldDifficultyId
	{
		Normal,
		Expert,
		Master,
		Creative
	}

	private enum WorldEvilId
	{
		Random,
		Corruption,
		Crimson
	}

	public delegate void SubmitSeedEvent();

	private string _optionwWorldName;

	private string _optionSeed;

	private bool _isSpecialSeedText;

	private List<string> _secretSeedTextsEntered = new List<string>();

	private List<string> _disabledSecretSeedTextsEntered = new List<string>();

	private ParticleRenderer SeedParticleSystem = new ParticleRenderer();

	private UIDust SeedDust = new UIDust();

	private GroupOptionButton<bool> _advancedSeedButton;

	private UICharacterNameButton _namePlate;

	private UICharacterNameButton _seedPlate;

	private UIWorldCreationPreview _previewPlate;

	private GroupOptionButton<WorldSizeId>[] _sizeButtons;

	private GroupOptionButton<WorldDifficultyId>[] _difficultyButtons;

	private GroupOptionButton<WorldEvilId>[] _evilButtons;

	private UIText _descriptionText;

	public const int MAX_NAME_LENGTH = 27;

	private UIState _goBackTarget;

	public SubmitSeedEvent SubmitSeed;

	private float ringPoint;

	private const int numSteps = 61;

	private Vector2[] oldPos = new Vector2[61];

	private Vector2[] oldTangent = new Vector2[61];

	private int specialSeedIndex;

	private static VertexStrip _vertexStrip = new VertexStrip();

	private float opacity = 0.6f;

	private float opacity2 = 0.5f;

	private float trial;

	private float animationSpeed = 0.5f;

	private float saturation = 0.5f;

	private WorldSizeId _optionSize
	{
		get
		{
			return (WorldSizeId)WorldGen.GetWorldSize();
		}
		set
		{
			WorldGen.SetWorldSize((int)value);
		}
	}

	private WorldDifficultyId _optionDifficulty
	{
		get
		{
			return (WorldDifficultyId)Main.GameMode;
		}
		set
		{
			Main.GameMode = (int)value;
		}
	}

	private WorldEvilId _optionEvil
	{
		get
		{
			return (WorldEvilId)(WorldGen.WorldGenParam_Evil + 1);
		}
		set
		{
			WorldGen.WorldGenParam_Evil = (int)(value - 1);
		}
	}

	public bool HasEnteredSpecialSeed => _secretSeedTextsEntered.Count > 0;

	public bool HasDisabledSecretSeed => _disabledSecretSeedTextsEntered.Count > 0;

	public UIWorldCreation()
	{
		_goBackTarget = this;
		BuildPage();
		SeedDust.Clear();
		SeedParticleSystem.Clear();
		ResetSpecialSeedRing();
	}

	public void SetGoBackTarget(UIState state)
	{
		_goBackTarget = state;
	}

	private void BuildPage()
	{
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		int num = 18;
		RemoveAllChildren();
		UIElement uIElement = new UIElement
		{
			Width = StyleDimension.FromPixels(500f),
			Height = StyleDimension.FromPixels(434f + (float)num),
			Top = StyleDimension.FromPixels(170f - (float)num),
			HAlign = 0.5f,
			VAlign = 0f
		};
		uIElement.SetPadding(0f);
		Append(uIElement);
		UIPanel uIPanel = new UIPanel
		{
			Width = StyleDimension.FromPercent(1f),
			Height = StyleDimension.FromPixels(280 + num),
			Top = StyleDimension.FromPixels(50f),
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

	private void PreparePreviouslyUnlockedSecretSeeds()
	{
		SecretSeedsTracker.PrepareInterface();
	}

	private void MakeInfoMenu(UIElement parentContainer)
	{
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_0255: Unknown result type (might be due to invalid IL or missing references)
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
		float num = 0f;
		float num2 = 44f;
		float num3 = 88f + num2;
		float pixels = num2;
		GroupOptionButton<bool> groupOptionButton = new GroupOptionButton<bool>(option: true, null, Language.GetText("UI.WorldCreationRandomizeNameDescription"), Color.White, "Images/UI/WorldCreation/IconRandomName")
		{
			Width = StyleDimension.FromPixelsAndPercent(40f, 0f),
			Height = new StyleDimension(40f, 0f),
			HAlign = 0f,
			Top = StyleDimension.FromPixelsAndPercent(num, 0f),
			ShowHighlightWhenSelected = false
		};
		groupOptionButton.OnLeftMouseDown += ClickRandomizeName;
		groupOptionButton.OnMouseOver += ShowOptionDescription;
		groupOptionButton.OnMouseOut += ClearOptionDescription;
		groupOptionButton.SetSnapPoint("RandomizeName", 0);
		uIElement.Append(groupOptionButton);
		UICharacterNameButton uICharacterNameButton = new UICharacterNameButton(Language.GetText("UI.WorldCreationName"), Language.GetText("UI.WorldCreationNameEmpty"), Language.GetText("UI.WorldDescriptionName"))
		{
			Width = StyleDimension.FromPixelsAndPercent(0f - num3, 1f),
			HAlign = 0f,
			Left = new StyleDimension(pixels, 0f),
			Top = StyleDimension.FromPixelsAndPercent(num, 0f)
		};
		uICharacterNameButton.OnLeftMouseDown += Click_SetName;
		uICharacterNameButton.OnMouseOver += ShowOptionDescription;
		uICharacterNameButton.OnMouseOut += ClearOptionDescription;
		uICharacterNameButton.SetSnapPoint("Name", 0);
		uIElement.Append(uICharacterNameButton);
		_namePlate = uICharacterNameButton;
		num += uICharacterNameButton.GetDimensions().Height + 4f;
		_advancedSeedButton = new GroupOptionButton<bool>(option: true, null, Language.GetText("UI.WorldCreationSeedMenuDescription"), Color.White, "Images/UI/WorldCreation/IconRandomSeed")
		{
			Width = StyleDimension.FromPixelsAndPercent(40f, 0f),
			Height = new StyleDimension(40f, 0f),
			HAlign = 0f,
			Top = StyleDimension.FromPixelsAndPercent(num, 0f),
			ShowHighlightWhenSelected = false
		};
		_advancedSeedButton.OnLeftMouseDown += ClickAdvancedSeedMenu;
		_advancedSeedButton.OnMouseOver += ShowOptionDescription;
		_advancedSeedButton.OnMouseOut += ClearOptionDescription;
		_advancedSeedButton.SetSnapPoint("RandomizeSeed", 0);
		_advancedSeedButton.OnDraw += DrawSpecialSeedRingCallback;
		uIElement.Append(_advancedSeedButton);
		UICharacterNameButton uICharacterNameButton2 = new UICharacterNameButton(Language.GetText("UI.WorldCreationSeed"), Language.GetText("UI.WorldCreationSeedEmpty"), Language.GetText("UI.WorldDescriptionSeed"))
		{
			Width = StyleDimension.FromPixelsAndPercent(0f - num3, 1f),
			HAlign = 0f,
			Left = new StyleDimension(pixels, 0f),
			Top = StyleDimension.FromPixelsAndPercent(num, 0f),
			DistanceFromTitleToOption = 29f
		};
		uICharacterNameButton2.OnLeftMouseDown += Click_SetSeed;
		uICharacterNameButton2.OnMouseOver += ShowOptionDescription;
		uICharacterNameButton2.OnMouseOut += ClearOptionDescription;
		uICharacterNameButton2.SetSnapPoint("Seed", 0);
		uIElement.Append(uICharacterNameButton2);
		_seedPlate = uICharacterNameButton2;
		UIWorldCreationPreview uIWorldCreationPreview = new UIWorldCreationPreview
		{
			Width = StyleDimension.FromPixels(84f),
			Height = StyleDimension.FromPixels(84f),
			HAlign = 1f,
			VAlign = 0f
		};
		uIElement.Append(uIWorldCreationPreview);
		_previewPlate = uIWorldCreationPreview;
		num += uICharacterNameButton2.GetDimensions().Height + 10f;
		AddHorizontalSeparator(uIElement, num + 2f);
		float usableWidthPercent = 1f;
		AddWorldSizeOptions(uIElement, num, ClickSizeOption, "size", usableWidthPercent);
		num += 48f;
		AddHorizontalSeparator(uIElement, num);
		AddWorldDifficultyOptions(uIElement, num, ClickDifficultyOption, "difficulty", usableWidthPercent);
		num += 48f;
		AddHorizontalSeparator(uIElement, num);
		AddWorldEvilOptions(uIElement, num, ClickEvilOption, "evil", usableWidthPercent);
		num += 48f;
		AddHorizontalSeparator(uIElement, num);
		AddDescriptionPanel(uIElement, num, "desc");
		SetDefaultOptions();
	}

	private static void AddHorizontalSeparator(UIElement Container, float accumualtedHeight)
	{
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		UIHorizontalSeparator element = new UIHorizontalSeparator
		{
			Width = StyleDimension.FromPercent(1f),
			Top = StyleDimension.FromPixels(accumualtedHeight - 8f),
			Color = Color.Lerp(Color.White, new Color(63, 65, 151, 255), 0.85f) * 0.9f
		};
		Container.Append(element);
	}

	private void SetDefaultOptions()
	{
		Main.ActiveWorldFileData = new WorldFileData();
		AssignRandomWorldName();
		ClearSeed();
		_optionSize = WorldSizeId.Medium;
		if (Main.ActivePlayerFileData.Player.difficulty == 3)
		{
			_optionDifficulty = WorldDifficultyId.Creative;
		}
		_optionEvil = WorldEvilId.Random;
		UpdateSliders();
		UpdatePreviewPlate();
	}

	private void AddDescriptionPanel(UIElement container, float accumulatedHeight, string tagGroup)
	{
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		float num = 0f;
		UISlicedImage uISlicedImage = new UISlicedImage(Main.Assets.Request<Texture2D>("Images/UI/CharCreation/CategoryPanelHighlight", (AssetRequestMode)1))
		{
			HAlign = 0.5f,
			VAlign = 1f,
			Width = StyleDimension.FromPixelsAndPercent((0f - num) * 2f, 1f),
			Left = StyleDimension.FromPixels(0f - num),
			Height = StyleDimension.FromPixelsAndPercent(40f, 0f),
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
			Height = StyleDimension.FromPixelsAndPercent(0f, 1f),
			Top = StyleDimension.FromPixelsAndPercent(5f, 0f)
		};
		uIText.PaddingLeft = 20f;
		uIText.PaddingRight = 20f;
		uIText.PaddingTop = 6f;
		uISlicedImage.Append(uIText);
		_descriptionText = uIText;
	}

	private void AddWorldSizeOptions(UIElement container, float accumualtedHeight, MouseEvent clickEvent, string tagGroup, float usableWidthPercent)
	{
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
		WorldSizeId[] array = new WorldSizeId[3]
		{
			WorldSizeId.Small,
			WorldSizeId.Medium,
			WorldSizeId.Large
		};
		LocalizedText[] array2 = new LocalizedText[3]
		{
			Lang.menu[92],
			Lang.menu[93],
			Lang.menu[94]
		};
		LocalizedText[] array3 = new LocalizedText[3]
		{
			Language.GetText("UI.WorldDescriptionSizeSmall"),
			Language.GetText("UI.WorldDescriptionSizeMedium"),
			Language.GetText("UI.WorldDescriptionSizeLarge")
		};
		Color[] array4 = new Color[3]
		{
			Color.Cyan,
			Color.Lerp(Color.Cyan, Color.LimeGreen, 0.5f),
			Color.LimeGreen
		};
		string[] array5 = new string[3] { "Images/UI/WorldCreation/IconSizeSmall", "Images/UI/WorldCreation/IconSizeMedium", "Images/UI/WorldCreation/IconSizeLarge" };
		GroupOptionButton<WorldSizeId>[] array6 = new GroupOptionButton<WorldSizeId>[array.Length];
		for (int i = 0; i < array6.Length; i++)
		{
			GroupOptionButton<WorldSizeId> groupOptionButton = new GroupOptionButton<WorldSizeId>(array[i], array2[i], array3[i], array4[i], array5[i], 1f, 1f, 16f);
			groupOptionButton.Width = StyleDimension.FromPixelsAndPercent(-4 * (array6.Length - 1), 1f / (float)array6.Length * usableWidthPercent);
			groupOptionButton.Left = StyleDimension.FromPercent(1f - usableWidthPercent);
			groupOptionButton.HAlign = (float)i / (float)(array6.Length - 1);
			groupOptionButton.Top.Set(accumualtedHeight, 0f);
			groupOptionButton.OnLeftMouseDown += clickEvent;
			groupOptionButton.OnMouseOver += ShowOptionDescription;
			groupOptionButton.OnMouseOut += ClearOptionDescription;
			groupOptionButton.SetSnapPoint(tagGroup, i);
			container.Append(groupOptionButton);
			array6[i] = groupOptionButton;
		}
		_sizeButtons = array6;
	}

	private void AddWorldDifficultyOptions(UIElement container, float accumualtedHeight, MouseEvent clickEvent, string tagGroup, float usableWidthPercent)
	{
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		WorldDifficultyId[] array = new WorldDifficultyId[4]
		{
			WorldDifficultyId.Creative,
			WorldDifficultyId.Normal,
			WorldDifficultyId.Expert,
			WorldDifficultyId.Master
		};
		LocalizedText[] array2 = new LocalizedText[4]
		{
			Language.GetText("UI.Creative"),
			Language.GetText("UI.Normal"),
			Language.GetText("UI.Expert"),
			Language.GetText("UI.Master")
		};
		LocalizedText[] array3 = new LocalizedText[4]
		{
			Language.GetText("UI.WorldDescriptionCreative"),
			Language.GetText("UI.WorldDescriptionNormal"),
			Language.GetText("UI.WorldDescriptionExpert"),
			Language.GetText("UI.WorldDescriptionMaster")
		};
		Color[] array4 = new Color[4]
		{
			Main.creativeModeColor,
			Color.White,
			Main.mcColor,
			Main.hcColor
		};
		string[] array5 = new string[4] { "Images/UI/WorldCreation/IconDifficultyCreative", "Images/UI/WorldCreation/IconDifficultyNormal", "Images/UI/WorldCreation/IconDifficultyExpert", "Images/UI/WorldCreation/IconDifficultyMaster" };
		GroupOptionButton<WorldDifficultyId>[] array6 = new GroupOptionButton<WorldDifficultyId>[array.Length];
		for (int i = 0; i < array6.Length; i++)
		{
			GroupOptionButton<WorldDifficultyId> groupOptionButton = new GroupOptionButton<WorldDifficultyId>(array[i], array2[i], array3[i], array4[i], array5[i], 1f, 1f, 16f);
			groupOptionButton.Width = StyleDimension.FromPixelsAndPercent(-1 * (array6.Length - 1), 1f / (float)array6.Length * usableWidthPercent);
			groupOptionButton.Left = StyleDimension.FromPercent(1f - usableWidthPercent);
			groupOptionButton.HAlign = (float)i / (float)(array6.Length - 1);
			groupOptionButton.Top.Set(accumualtedHeight, 0f);
			groupOptionButton.OnLeftMouseDown += clickEvent;
			groupOptionButton.OnMouseOver += ShowOptionDescription;
			groupOptionButton.OnMouseOut += ClearOptionDescription;
			groupOptionButton.SetSnapPoint(tagGroup, i);
			container.Append(groupOptionButton);
			array6[i] = groupOptionButton;
		}
		_difficultyButtons = array6;
	}

	private void AddWorldEvilOptions(UIElement container, float accumualtedHeight, MouseEvent clickEvent, string tagGroup, float usableWidthPercent)
	{
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		WorldEvilId[] array = new WorldEvilId[3]
		{
			WorldEvilId.Random,
			WorldEvilId.Corruption,
			WorldEvilId.Crimson
		};
		LocalizedText[] array2 = new LocalizedText[3]
		{
			Lang.misc[103],
			Lang.misc[101],
			Lang.misc[102]
		};
		LocalizedText[] array3 = new LocalizedText[3]
		{
			Language.GetText("UI.WorldDescriptionEvilRandom"),
			Language.GetText("UI.WorldDescriptionEvilCorrupt"),
			Language.GetText("UI.WorldDescriptionEvilCrimson")
		};
		Color[] array4 = new Color[3]
		{
			Color.White,
			Color.MediumPurple,
			Color.IndianRed
		};
		string[] array5 = new string[3] { "Images/UI/WorldCreation/IconEvilRandom", "Images/UI/WorldCreation/IconEvilCorruption", "Images/UI/WorldCreation/IconEvilCrimson" };
		GroupOptionButton<WorldEvilId>[] array6 = new GroupOptionButton<WorldEvilId>[array.Length];
		for (int i = 0; i < array6.Length; i++)
		{
			GroupOptionButton<WorldEvilId> groupOptionButton = new GroupOptionButton<WorldEvilId>(array[i], array2[i], array3[i], array4[i], array5[i], 1f, 1f, 16f);
			groupOptionButton.Width = StyleDimension.FromPixelsAndPercent(-4 * (array6.Length - 1), 1f / (float)array6.Length * usableWidthPercent);
			groupOptionButton.Left = StyleDimension.FromPercent(1f - usableWidthPercent);
			groupOptionButton.HAlign = (float)i / (float)(array6.Length - 1);
			groupOptionButton.Top.Set(accumualtedHeight, 0f);
			groupOptionButton.OnLeftMouseDown += clickEvent;
			groupOptionButton.OnMouseOver += ShowOptionDescription;
			groupOptionButton.OnMouseOut += ClearOptionDescription;
			groupOptionButton.SetSnapPoint(tagGroup, i);
			container.Append(groupOptionButton);
			array6[i] = groupOptionButton;
		}
		_evilButtons = array6;
	}

	private void ClickRandomizeName(UIMouseEvent evt, UIElement listeningElement)
	{
		AssignRandomWorldName();
		UpdateInputFields();
		UpdateSliders();
		UpdatePreviewPlate();
	}

	private void ClickAdvancedSeedMenu(UIMouseEvent evt, UIElement listeningElement)
	{
		ResetSpecialSeedRing();
		UIWorldCreationAdvanced uIWorldCreationAdvanced = new UIWorldCreationAdvanced(this);
		SetGoBackTarget(uIWorldCreationAdvanced);
		Main.MenuUI.SetState(uIWorldCreationAdvanced);
	}

	public void ClearSeedText()
	{
		_optionSeed = "";
		_isSpecialSeedText = false;
		UpdateInputFields();
	}

	public void ClearSeed()
	{
		_optionSeed = string.Empty;
		_isSpecialSeedText = false;
		_secretSeedTextsEntered.Clear();
		_disabledSecretSeedTextsEntered.Clear();
		WorldGenerationOptions.Reset();
		WorldGen.SecretSeed.ClearAllSeeds();
		PreparePreviouslyUnlockedSecretSeeds();
		UpdateInputFields();
	}

	public void RandomizeSeed()
	{
		_optionSeed = Main.rand.Next().ToString();
		_isSpecialSeedText = false;
		UpdateInputFields();
		UpdateSliders();
		UpdatePreviewPlate();
	}

	private void ClickSizeOption(UIMouseEvent evt, UIElement listeningElement)
	{
		GroupOptionButton<WorldSizeId> groupOptionButton = (GroupOptionButton<WorldSizeId>)listeningElement;
		_optionSize = groupOptionButton.OptionValue;
		GroupOptionButton<WorldSizeId>[] sizeButtons = _sizeButtons;
		for (int i = 0; i < sizeButtons.Length; i++)
		{
			sizeButtons[i].SetCurrentOption(groupOptionButton.OptionValue);
		}
		UpdatePreviewPlate();
	}

	private void ClickDifficultyOption(UIMouseEvent evt, UIElement listeningElement)
	{
		GroupOptionButton<WorldDifficultyId> groupOptionButton = (GroupOptionButton<WorldDifficultyId>)listeningElement;
		_optionDifficulty = groupOptionButton.OptionValue;
		GroupOptionButton<WorldDifficultyId>[] difficultyButtons = _difficultyButtons;
		for (int i = 0; i < difficultyButtons.Length; i++)
		{
			difficultyButtons[i].SetCurrentOption(groupOptionButton.OptionValue);
		}
		UpdatePreviewPlate();
	}

	private void ClickEvilOption(UIMouseEvent evt, UIElement listeningElement)
	{
		GroupOptionButton<WorldEvilId> groupOptionButton = (GroupOptionButton<WorldEvilId>)listeningElement;
		_optionEvil = groupOptionButton.OptionValue;
		GroupOptionButton<WorldEvilId>[] evilButtons = _evilButtons;
		for (int i = 0; i < evilButtons.Length; i++)
		{
			evilButtons[i].SetCurrentOption(groupOptionButton.OptionValue);
		}
		UpdatePreviewPlate();
	}

	private void UpdatePreviewPlate()
	{
		_previewPlate.UpdateOption((byte)_optionDifficulty, (byte)_optionEvil, (byte)_optionSize);
	}

	private void UpdateSliders()
	{
		GroupOptionButton<WorldSizeId>[] sizeButtons = _sizeButtons;
		for (int i = 0; i < sizeButtons.Length; i++)
		{
			sizeButtons[i].SetCurrentOption(_optionSize);
		}
		GroupOptionButton<WorldDifficultyId>[] difficultyButtons = _difficultyButtons;
		for (int i = 0; i < difficultyButtons.Length; i++)
		{
			difficultyButtons[i].SetCurrentOption(_optionDifficulty);
		}
		GroupOptionButton<WorldEvilId>[] evilButtons = _evilButtons;
		for (int i = 0; i < evilButtons.Length; i++)
		{
			evilButtons[i].SetCurrentOption(_optionEvil);
		}
	}

	public void ShowOptionDescription(UIMouseEvent evt, UIElement listeningElement)
	{
		LocalizedText localizedText = null;
		if (listeningElement is GroupOptionButton<WorldSizeId> groupOptionButton)
		{
			localizedText = groupOptionButton.Description;
		}
		if (listeningElement is GroupOptionButton<WorldDifficultyId> groupOptionButton2)
		{
			localizedText = groupOptionButton2.Description;
		}
		if (listeningElement is GroupOptionButton<WorldEvilId> groupOptionButton3)
		{
			localizedText = groupOptionButton3.Description;
		}
		if (listeningElement is UICharacterNameButton uICharacterNameButton)
		{
			localizedText = uICharacterNameButton.Description;
		}
		if (listeningElement is GroupOptionButton<bool> groupOptionButton4)
		{
			localizedText = groupOptionButton4.Description;
		}
		if (localizedText != null)
		{
			_descriptionText.SetText(localizedText);
		}
	}

	public void ClearOptionDescription(UIMouseEvent evt, UIElement listeningElement)
	{
		_descriptionText.SetText(Language.GetText("UI.WorldDescriptionDefault"));
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
		Main.OpenWorldSelectUI();
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

	private void Click_SetName(UIMouseEvent evt, UIElement listeningElement)
	{
		SoundEngine.PlaySound(10);
		Main.clrInput();
		UIVirtualKeyboard state = new UIVirtualKeyboard(Lang.menu[48].Value, "", OnFinishedSettingName, GoBackHere, 0, allowEmpty: true, 27);
		Main.MenuUI.SetState(state);
	}

	private void Click_SetSeed(UIMouseEvent evt, UIElement listeningElement)
	{
		OpenSeedInputMenu();
	}

	public void OpenSeedInputMenu()
	{
		SoundEngine.PlaySound(10);
		Main.clrInput();
		UIVirtualKeyboard state = new UIVirtualKeyboard(Language.GetTextValue("UI.EnterSeed"), "", OnFinishedSettingSeed, GoBackHere, 0, allowEmpty: true, int.MaxValue);
		Main.MenuUI.SetState(state);
	}

	private void Click_NamingAndCreating(UIMouseEvent evt, UIElement listeningElement)
	{
		SoundEngine.PlaySound(10);
		if (string.IsNullOrEmpty(_optionwWorldName))
		{
			_optionwWorldName = "";
			Main.clrInput();
			UIVirtualKeyboard state = new UIVirtualKeyboard(Lang.menu[48].Value, "", OnFinishedNamingAndCreating, GoBackHere, 0, allowEmpty: false, 27);
			Main.MenuUI.SetState(state);
		}
		else
		{
			FinishCreatingWorld();
		}
	}

	private void OnFinishedSettingName(string name)
	{
		_optionwWorldName = name.Trim();
		UpdateInputFields();
		GoBackHere();
	}

	private void UpdateInputFields()
	{
		_namePlate.SetContents(_optionwWorldName);
		_namePlate.Recalculate();
		_namePlate.TrimDisplayIfOverElementDimensions(27);
		_namePlate.Recalculate();
		FillSeedContent(_seedPlate);
	}

	public void FillSeedContent(UICharacterNameButton button)
	{
		button.SetContents(_optionSeed);
		button.Recalculate();
		button.TrimDisplayIfOverElementDimensions(WorldFileData.MAX_USER_SEED_TEXT_LENGTH);
		button.Recalculate();
	}

	public void ToggleSeedOption(AWorldGenerationOption seedOption)
	{
		if (_isSpecialSeedText)
		{
			_optionSeed = string.Empty;
			_isSpecialSeedText = false;
			UpdateInputFields();
			UpdateSliders();
			UpdatePreviewPlate();
		}
		seedOption.Enabled = !seedOption.Enabled;
	}

	public void EnableSecretSeedOptions(bool enabled)
	{
		if (enabled)
		{
			for (int i = 0; i < _disabledSecretSeedTextsEntered.Count; i++)
			{
				if (WorldGen.SecretSeed.CheckInputForSecretSeed(_disabledSecretSeedTextsEntered[i], out var secretSeed) && !secretSeed.Enabled)
				{
					_secretSeedTextsEntered.Add(_disabledSecretSeedTextsEntered[i]);
					WorldGen.SecretSeed.Enable(secretSeed, playSound: false);
				}
			}
			_disabledSecretSeedTextsEntered.Clear();
		}
		else
		{
			_disabledSecretSeedTextsEntered.Clear();
			_disabledSecretSeedTextsEntered.AddRange(_secretSeedTextsEntered);
			WorldGen.SecretSeed.ClearAllSeeds();
			_secretSeedTextsEntered.Clear();
		}
	}

	public string GetJoinedSecretSeedString(DynamicSpriteFont font, float maxWidth, float maxHeight)
	{
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		float num = 0f;
		string text = string.Empty;
		List<string> list = (HasEnteredSpecialSeed ? _secretSeedTextsEntered : _disabledSecretSeedTextsEntered);
		if (list.Count == 0)
		{
			return "-";
		}
		string text2 = list[0];
		for (int i = 1; i < list.Count; i++)
		{
			string text3 = $"{text2}|{list[i]}";
			if (font.MeasureString(text3).X >= maxWidth)
			{
				if (num <= maxHeight)
				{
					text = text + text2 + "\n";
				}
				num += (float)font.LineSpacing;
				text3 = list[i];
			}
			text2 = text3;
		}
		if (num <= maxHeight)
		{
			text += text2;
		}
		return text;
	}

	private void OnFinishedSettingSeed(string seed)
	{
		//IL_011b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0120: Unknown result type (might be due to invalid IL or missing references)
		//IL_0131: Unknown result type (might be due to invalid IL or missing references)
		//IL_0136: Unknown result type (might be due to invalid IL or missing references)
		//IL_0143: Unknown result type (might be due to invalid IL or missing references)
		//IL_0145: Unknown result type (might be due to invalid IL or missing references)
		//IL_0156: Unknown result type (might be due to invalid IL or missing references)
		//IL_015b: Unknown result type (might be due to invalid IL or missing references)
		//IL_015d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0162: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_0182: Unknown result type (might be due to invalid IL or missing references)
		//IL_0184: Unknown result type (might be due to invalid IL or missing references)
		//IL_0195: Unknown result type (might be due to invalid IL or missing references)
		//IL_019a: Unknown result type (might be due to invalid IL or missing references)
		//IL_019c: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_020d: Unknown result type (might be due to invalid IL or missing references)
		//IL_020f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0220: Unknown result type (might be due to invalid IL or missing references)
		//IL_0225: Unknown result type (might be due to invalid IL or missing references)
		//IL_0227: Unknown result type (might be due to invalid IL or missing references)
		//IL_022c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0248: Unknown result type (might be due to invalid IL or missing references)
		//IL_0262: Unknown result type (might be due to invalid IL or missing references)
		//IL_0267: Unknown result type (might be due to invalid IL or missing references)
		//IL_026c: Unknown result type (might be due to invalid IL or missing references)
		_optionSeed = seed.Trim();
		if (WorldFileData.TryApplyingCopiedSeed(_optionSeed, playSound: true, out var processedSeed, out var _, out var secretSeedTexts))
		{
			_optionSeed = processedSeed;
			_secretSeedTextsEntered = secretSeedTexts;
			_disabledSecretSeedTextsEntered.Clear();
		}
		else
		{
			_optionSeed = Utils.TrimUserString(_optionSeed, WorldFileData.MAX_USER_SEED_TEXT_LENGTH);
			AWorldGenerationOption optionFromSeedText = WorldGenerationOptions.GetOptionFromSeedText(_optionSeed);
			_isSpecialSeedText = optionFromSeedText != null;
			if (_isSpecialSeedText)
			{
				WorldGenerationOptions.SelectOption(optionFromSeedText);
				SoundEngine.PlaySound(24);
			}
			if (WorldGen.SecretSeed.CheckInputForSecretSeed(_optionSeed, out var secretSeed))
			{
				if (!secretSeed.Enabled)
				{
					_secretSeedTextsEntered.Add(_optionSeed);
					WorldGen.SecretSeed.Enable(secretSeed);
					EnableSecretSeedOptions(enabled: true);
					CalculatedStyle dimensions = _advancedSeedButton.GetDimensions();
					if (_goBackTarget != this && _goBackTarget is UIWorldCreationAdvanced uIWorldCreationAdvanced)
					{
						uIWorldCreationAdvanced.RefreshSecretSeedButton();
						dimensions = uIWorldCreationAdvanced.GetSecretSeedButton().GetDimensions();
						uIWorldCreationAdvanced.GetSecretSeedButton().SetCurrentOption(HasEnteredSpecialSeed);
					}
					Vector2 val = dimensions.Center();
					Vector2 val2 = Main.rand.NextVector2Circular(5f, 5f);
					Spawn_RainbowRodHit(new ParticleOrchestraSettings
					{
						PositionInWorld = val,
						MovementVector = new Vector2(16f, 0f) + val2
					});
					if (_goBackTarget != this)
					{
						Spawn_RainbowRodHit(new ParticleOrchestraSettings
						{
							PositionInWorld = val,
							MovementVector = new Vector2(16f, 0f) - val2
						});
					}
					Vector2 val3 = Main.rand.NextVector2Circular(5f, 5f);
					Spawn_RainbowRodHit(new ParticleOrchestraSettings
					{
						PositionInWorld = val,
						MovementVector = new Vector2(0f, 16f) + val3
					});
					if (_goBackTarget != this)
					{
						Spawn_RainbowRodHit(new ParticleOrchestraSettings
						{
							PositionInWorld = val,
							MovementVector = new Vector2(0f, 16f) - val3
						});
					}
					for (int i = 0; i < 3; i++)
					{
						Spawn_BestReforge(new ParticleOrchestraSettings
						{
							PositionInWorld = val + new Vector2(dimensions.Width * 0.25f * (float)(i - 1), 0f)
						});
					}
				}
				ClearSeedText();
			}
		}
		UpdateInputFields();
		UpdateSliders();
		UpdatePreviewPlate();
		if (SubmitSeed != null)
		{
			SubmitSeed();
		}
		GoBackHere();
	}

	private void Spawn_BestReforge(ParticleOrchestraSettings settings)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		Vector2 accelerationPerFrame = new Vector2(0f, 0.16350001f);
		Asset<Texture2D> textureAsset = Main.Assets.Request<Texture2D>("Images/UI/Creative/Research_Spark", (AssetRequestMode)1);
		for (int i = 0; i < 8; i++)
		{
			Vector2 val = Main.rand.NextVector2Circular(3f, 4f);
			SeedParticleSystem.Add(new CreativeSacrificeParticle(textureAsset, null, settings.MovementVector + val, settings.PositionInWorld)
			{
				AccelerationPerFrame = accelerationPerFrame,
				ScaleOffsetPerFrame = -1f / 60f
			});
		}
	}

	private void Spawn_RainbowRodHit(ParticleOrchestraSettings settings)
	{
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0108: Unknown result type (might be due to invalid IL or missing references)
		//IL_010d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0112: Unknown result type (might be due to invalid IL or missing references)
		//IL_0151: Unknown result type (might be due to invalid IL or missing references)
		//IL_0156: Unknown result type (might be due to invalid IL or missing references)
		//IL_016b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0170: Unknown result type (might be due to invalid IL or missing references)
		//IL_0172: Unknown result type (might be due to invalid IL or missing references)
		//IL_0177: Unknown result type (might be due to invalid IL or missing references)
		//IL_0187: Unknown result type (might be due to invalid IL or missing references)
		//IL_0189: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01da: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_020c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0211: Unknown result type (might be due to invalid IL or missing references)
		//IL_0219: Unknown result type (might be due to invalid IL or missing references)
		//IL_021e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0220: Unknown result type (might be due to invalid IL or missing references)
		//IL_0225: Unknown result type (might be due to invalid IL or missing references)
		//IL_0235: Unknown result type (might be due to invalid IL or missing references)
		//IL_023c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0241: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_030e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0318: Unknown result type (might be due to invalid IL or missing references)
		//IL_031d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0322: Unknown result type (might be due to invalid IL or missing references)
		//IL_03bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c1: Unknown result type (might be due to invalid IL or missing references)
		float num = Main.rand.NextFloat() * ((float)Math.PI * 2f);
		float num2 = 6f;
		float num3 = Main.rand.NextFloat();
		for (float num4 = 0f; num4 < 1f; num4 += 1f / num2)
		{
			Vector2 val = settings.MovementVector * Main.rand.NextFloatDirection() * 0.15f;
			Vector2 val2 = new Vector2(Main.rand.NextFloat() * 0.4f + 0.4f);
			float f = num + Main.rand.NextFloat() * ((float)Math.PI * 2f);
			float rotation = (float)Math.PI / 2f;
			Vector2 val3 = 1.5f * val2;
			float num5 = 60f;
			Vector2 val4 = Main.rand.NextVector2Circular(8f, 8f) * val2;
			PrettySparkleParticle prettySparkleParticle = new PrettySparkleParticle();
			prettySparkleParticle.Velocity = f.ToRotationVector2() * val3 + val;
			prettySparkleParticle.AccelerationPerFrame = f.ToRotationVector2() * -(val3 / num5) - val * 1f / 60f;
			prettySparkleParticle.ColorTint = Main.hslToRgb((num3 + Main.rand.NextFloat() * 0.33f) % 1f, 1f, 0.4f + Main.rand.NextFloat() * 0.25f);
			prettySparkleParticle.ColorTint.A = 0;
			prettySparkleParticle.LocalPosition = settings.PositionInWorld + val4;
			prettySparkleParticle.Rotation = rotation;
			prettySparkleParticle.Scale = val2;
			SeedParticleSystem.Add(prettySparkleParticle);
			prettySparkleParticle = new PrettySparkleParticle();
			prettySparkleParticle.Velocity = f.ToRotationVector2() * val3 + val;
			prettySparkleParticle.AccelerationPerFrame = f.ToRotationVector2() * -(val3 / num5) - val * 1f / 60f;
			prettySparkleParticle.ColorTint = new Color(255, 255, 255, 0);
			prettySparkleParticle.LocalPosition = settings.PositionInWorld + val4;
			prettySparkleParticle.Rotation = rotation;
			prettySparkleParticle.Scale = val2 * 0.6f;
			SeedParticleSystem.Add(prettySparkleParticle);
		}
		for (int i = 0; i < 12; i++)
		{
			Color newColor = Main.hslToRgb((num3 + Main.rand.NextFloat() * 0.12f) % 1f, 1f, 0.4f + Main.rand.NextFloat() * 0.25f);
			Dust dust = SeedDust.NewDust(settings.PositionInWorld, 0, 0, 267, 0f, 0f, 0, newColor);
			dust.velocity = Main.rand.NextVector2Circular(1f, 1f);
			dust.velocity += settings.MovementVector * Main.rand.NextFloatDirection() * 0.5f;
			dust.noGravity = true;
			dust.scale = 0.6f + Main.rand.NextFloat() * 0.9f;
			dust.fadeIn = 0.7f + Main.rand.NextFloat() * 0.8f;
			if (dust.dustIndex != 200)
			{
				Dust dust2 = SeedDust.CloneDust(dust);
				dust2.scale /= 2f;
				dust2.fadeIn *= 0.75f;
				dust2.color = new Color(255, 255, 255, 255);
			}
		}
	}

	private void GoBackHere()
	{
		Main.MenuUI.SetState(_goBackTarget);
	}

	private void OnFinishedNamingAndCreating(string name)
	{
		OnFinishedSettingName(name);
		FinishCreatingWorld();
	}

	private void FinishCreatingWorld()
	{
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		string name = (Main.worldName = _optionwWorldName.Trim());
		WorldDifficultyId optionDifficulty = _optionDifficulty;
		Main.ActiveWorldFileData = WorldFile.CreateMetadata(name, SocialAPI.Cloud != null && SocialAPI.Cloud.EnabledByDefault, Main.GameMode);
		_optionDifficulty = optionDifficulty;
		if (_optionSeed.Length == 0 || _isSpecialSeedText)
		{
			Main.ActiveWorldFileData.SetSeedToRandomWithCurrentEvents();
		}
		else
		{
			Main.ActiveWorldFileData.SetSeed(_optionSeed);
		}
		if (_secretSeedTextsEntered.Count > 0)
		{
			string seed = string.Join("|", _secretSeedTextsEntered) + "|" + Main.ActiveWorldFileData.SeedText;
			Main.ActiveWorldFileData.SetSeed(seed);
		}
		WorldGenerator.Controller controller = new WorldGenerator.Controller
		{
			Paused = (DebugOptions.enableDebugCommands && Main.keyState.PressingControl())
		};
		WorldGen.CreateNewWorld(null, controller);
	}

	private void AssignRandomWorldName()
	{
		do
		{
			LocalizedText localizedText = Language.SelectRandom(Lang.CreateDialogFilter("RandomWorldName_Composition.", checkConditions: false));
			LocalizedText localizedText2 = Language.SelectRandom(Lang.CreateDialogFilter("RandomWorldName_Adjective."));
			LocalizedText localizedText3 = Language.SelectRandom(Lang.CreateDialogFilter("RandomWorldName_Location."));
			LocalizedText localizedText4 = Language.SelectRandom(Lang.CreateDialogFilter("RandomWorldName_Noun."));
			var obj = new
			{
				Adjective = localizedText2.Value,
				Location = localizedText3.Value,
				Noun = localizedText4.Value
			};
			_optionwWorldName = localizedText.FormatWith(obj);
			if (Main.rand.Next(10000) == 0)
			{
				_optionwWorldName = Language.GetTextValue("SpecialWorldName.TheConstant");
			}
		}
		while (_optionwWorldName.Length > 27);
	}

	public override void Draw(SpriteBatch spriteBatch)
	{
		if (_goBackTarget != this)
		{
			_goBackTarget = this;
		}
		base.Draw(spriteBatch);
		SetupGamepadPoints(spriteBatch);
		DrawSeedSystems(spriteBatch);
	}

	public void ResetSpecialSeedRing()
	{
		ringPoint = 0f;
		Array.Clear(oldPos, 0, oldPos.Length);
		Array.Clear(oldTangent, 0, oldTangent.Length);
	}

	public void DrawSpecialSeedRingCallback(UIElement element, SpriteBatch spriteBatch)
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		if (!HasEnteredSpecialSeed)
		{
			return;
		}
		spriteBatch.End();
		spriteBatch.Begin((SpriteSortMode)0, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, (Effect)null, Main.UIScaleMatrix);
		if (oldPos[0] == Vector2.Zero)
		{
			for (int i = 0; i < 61; i++)
			{
				UpdateSpecialSeedRing(element);
			}
		}
		else
		{
			specialSeedIndex = (specialSeedIndex + 1) % 4;
			if (specialSeedIndex % 4 == 0)
			{
				UpdateSpecialSeedRing(element);
			}
		}
		DrawSpecialSeedRing();
		spriteBatch.End();
		spriteBatch.Begin((SpriteSortMode)0, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, (Effect)null, Main.UIScaleMatrix);
	}

	public void DrawSpecialSeedRingCallbackWithoutCondition(UIElement element, SpriteBatch spriteBatch)
	{
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		spriteBatch.End();
		spriteBatch.Begin((SpriteSortMode)0, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, (Effect)null, Main.UIScaleMatrix);
		if (oldPos[0] == Vector2.Zero)
		{
			for (int i = 0; i < 61; i++)
			{
				UpdateSpecialSeedRing(element);
			}
		}
		else
		{
			UpdateSpecialSeedRing(element);
		}
		DrawSpecialSeedRing();
		spriteBatch.End();
		spriteBatch.Begin((SpriteSortMode)0, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, (Effect)null, Main.UIScaleMatrix);
	}

	private void UpdateSpecialSeedRing(UIElement element)
	{
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		//IL_0119: Unknown result type (might be due to invalid IL or missing references)
		//IL_0125: Unknown result type (might be due to invalid IL or missing references)
		//IL_0153: Unknown result type (might be due to invalid IL or missing references)
		//IL_0157: Unknown result type (might be due to invalid IL or missing references)
		//IL_015c: Unknown result type (might be due to invalid IL or missing references)
		//IL_015e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0162: Unknown result type (might be due to invalid IL or missing references)
		//IL_0167: Unknown result type (might be due to invalid IL or missing references)
		//IL_013b: Unknown result type (might be due to invalid IL or missing references)
		//IL_013f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0144: Unknown result type (might be due to invalid IL or missing references)
		//IL_0146: Unknown result type (might be due to invalid IL or missing references)
		//IL_014a: Unknown result type (might be due to invalid IL or missing references)
		//IL_014f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0169: Unknown result type (might be due to invalid IL or missing references)
		//IL_016d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0172: Unknown result type (might be due to invalid IL or missing references)
		//IL_0177: Unknown result type (might be due to invalid IL or missing references)
		//IL_017c: Unknown result type (might be due to invalid IL or missing references)
		//IL_019e: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e6: Unknown result type (might be due to invalid IL or missing references)
		CalculatedStyle dimensions = _advancedSeedButton.GetDimensions();
		if (_goBackTarget != this && _goBackTarget is UIWorldCreationAdvanced uIWorldCreationAdvanced)
		{
			uIWorldCreationAdvanced.RefreshSecretSeedButton();
			dimensions = uIWorldCreationAdvanced.GetSecretSeedButton().GetDimensions();
		}
		if (element is GroupOptionButton<WorldGen.SecretSeed>)
		{
			dimensions = element.GetDimensions();
		}
		Rectangle val = dimensions.ToRectangle();
		val.Inflate(-1, -1);
		int num = val.Width * 2 + val.Height * 2;
		float num2 = (float)num / 60f;
		ringPoint += num2;
		if (ringPoint >= (float)num)
		{
			ringPoint -= num;
		}
		float num3 = (float)Math.Sqrt(val.Width / 2 * val.Width / 2 + val.Height / 2 * val.Height / 2);
		float num4 = (float)Math.PI * 2f * ringPoint / (float)num;
		Vector2 val2 = new Vector2((float)Math.Cos(num4), (float)Math.Sin(num4));
		Vector2 val3 = val2 * num3;
		float num5 = Math.Abs(val3.X) / ((float)val.Width / 2f);
		float num6 = Math.Abs(val3.Y) / ((float)val.Height / 2f);
		if (num5 > num6)
		{
			val3 /= num5;
			val2 /= num5;
		}
		else
		{
			val3 /= num6;
			val2 /= num6;
		}
		val3 += val.Center.ToVector2();
		for (int num7 = oldPos.Length - 1; num7 > 0; num7--)
		{
			oldPos[num7] = oldPos[num7 - 1];
			oldTangent[num7] = oldTangent[num7 - 1];
		}
		oldPos[0] = val3;
		oldTangent[0] = val2;
	}

	private void DrawSpecialSeedRing()
	{
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0108: Unknown result type (might be due to invalid IL or missing references)
		//IL_011b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0125: Unknown result type (might be due to invalid IL or missing references)
		//IL_0127: Unknown result type (might be due to invalid IL or missing references)
		//IL_0129: Unknown result type (might be due to invalid IL or missing references)
		//IL_012e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0130: Unknown result type (might be due to invalid IL or missing references)
		//IL_0132: Unknown result type (might be due to invalid IL or missing references)
		//IL_0134: Unknown result type (might be due to invalid IL or missing references)
		MiscShaderData miscShaderData = GameShaders.Misc["RainbowRod"];
		miscShaderData.UseSaturation(trial);
		miscShaderData.UseOpacity((_goBackTarget != this) ? opacity2 : opacity);
		miscShaderData.UseSpriteTransformMatrix(Main.UIScaleMatrix);
		miscShaderData.Apply();
		float num = 4f;
		if (_goBackTarget != this)
		{
			num = 5f;
		}
		int num2 = oldPos.Length;
		_vertexStrip.Reset(num2 * 2);
		int num3 = num2;
		for (int i = 0; i < num2 && !(oldPos[i] == Vector2.Zero); i++)
		{
			Vector2 val = oldPos[i];
			float num4 = (float)i / (float)(num3 - 1);
			num4 *= 0.6f;
			Color vertexColor = StripColors(num4);
			float num5 = StripWidth(num4);
			Vector2 val2 = oldTangent[i] * num;
			Vector3 uvA = new Vector3(num4, num5 / 2f, num5);
			Vector3 uvB = new Vector3(num4, num5 / 2f, num5);
			_vertexStrip.AddVertexPair(val + val2, val, uvA, uvB, vertexColor);
		}
		_vertexStrip.PrepareIndices(includeBacksides: true);
		_vertexStrip.DrawTrail();
		Main.pixelShader.CurrentTechnique.Passes[0].Apply();
		miscShaderData.UseSpriteTransformMatrix(null);
	}

	private Color StripColors(float progressOnStrip)
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		Color result = Main.hslToRgb((progressOnStrip - Main.GlobalTimeWrappedHourly * animationSpeed) % 1f, saturation, 0.5f);
		result.A = 0;
		return result;
	}

	private float StripWidth(float progressOnStrip)
	{
		_ = 1f;
		float lerpValue = Utils.GetLerpValue(0f, 0.2f, progressOnStrip, clamped: true);
		return 24f * lerpValue;
	}

	public void DrawSeedSystems(SpriteBatch spriteBatch)
	{
		SeedDust.UpdateDust();
		SeedDust.DrawDust();
		SeedParticleSystem.Update();
		SeedParticleSystem.Draw(spriteBatch);
	}

	private void SetupGamepadPoints(SpriteBatch spriteBatch)
	{
		//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_011b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0141: Unknown result type (might be due to invalid IL or missing references)
		//IL_0167: Unknown result type (might be due to invalid IL or missing references)
		//IL_018d: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0247: Unknown result type (might be due to invalid IL or missing references)
		//IL_029b: Unknown result type (might be due to invalid IL or missing references)
		UILinkPointNavigator.Shortcuts.BackButtonCommand = 7;
		int num = 3000;
		List<SnapPoint> snapPoints = GetSnapPoints();
		SnapPoint snapPoint = null;
		SnapPoint snapPoint2 = null;
		SnapPoint snapPoint3 = null;
		SnapPoint snapPoint4 = null;
		SnapPoint snapPoint5 = null;
		SnapPoint snapPoint6 = null;
		for (int i = 0; i < snapPoints.Count; i++)
		{
			SnapPoint snapPoint7 = snapPoints[i];
			switch (snapPoint7.Name)
			{
			case "Back":
				snapPoint = snapPoint7;
				break;
			case "Create":
				snapPoint2 = snapPoint7;
				break;
			case "Name":
				snapPoint3 = snapPoint7;
				break;
			case "Seed":
				snapPoint4 = snapPoint7;
				break;
			case "RandomizeName":
				snapPoint5 = snapPoint7;
				break;
			case "RandomizeSeed":
				snapPoint6 = snapPoint7;
				break;
			}
		}
		List<SnapPoint> snapGroup = GetSnapGroup(snapPoints, "size");
		List<SnapPoint> snapGroup2 = GetSnapGroup(snapPoints, "difficulty");
		List<SnapPoint> snapGroup3 = GetSnapGroup(snapPoints, "evil");
		UILinkPointNavigator.SetPosition(num, snapPoint.Position);
		UILinkPoint uILinkPoint = UILinkPointNavigator.Points[num];
		uILinkPoint.Unlink();
		UILinkPoint uILinkPoint2 = uILinkPoint;
		num++;
		UILinkPointNavigator.SetPosition(num, snapPoint2.Position);
		uILinkPoint = UILinkPointNavigator.Points[num];
		uILinkPoint.Unlink();
		UILinkPoint uILinkPoint3 = uILinkPoint;
		num++;
		UILinkPointNavigator.SetPosition(num, snapPoint5.Position);
		uILinkPoint = UILinkPointNavigator.Points[num];
		uILinkPoint.Unlink();
		UILinkPoint uILinkPoint4 = uILinkPoint;
		num++;
		UILinkPointNavigator.SetPosition(num, snapPoint3.Position);
		uILinkPoint = UILinkPointNavigator.Points[num];
		uILinkPoint.Unlink();
		UILinkPoint uILinkPoint5 = uILinkPoint;
		num++;
		UILinkPointNavigator.SetPosition(num, snapPoint6.Position);
		uILinkPoint = UILinkPointNavigator.Points[num];
		uILinkPoint.Unlink();
		UILinkPoint uILinkPoint6 = uILinkPoint;
		num++;
		UILinkPointNavigator.SetPosition(num, snapPoint4.Position);
		uILinkPoint = UILinkPointNavigator.Points[num];
		uILinkPoint.Unlink();
		UILinkPoint uILinkPoint7 = uILinkPoint;
		num++;
		UILinkPoint[] array = new UILinkPoint[snapGroup.Count];
		for (int j = 0; j < snapGroup.Count; j++)
		{
			UILinkPointNavigator.SetPosition(num, snapGroup[j].Position);
			uILinkPoint = UILinkPointNavigator.Points[num];
			uILinkPoint.Unlink();
			array[j] = uILinkPoint;
			num++;
		}
		UILinkPoint[] array2 = new UILinkPoint[snapGroup2.Count];
		for (int k = 0; k < snapGroup2.Count; k++)
		{
			UILinkPointNavigator.SetPosition(num, snapGroup2[k].Position);
			uILinkPoint = UILinkPointNavigator.Points[num];
			uILinkPoint.Unlink();
			array2[k] = uILinkPoint;
			num++;
		}
		UILinkPoint[] array3 = new UILinkPoint[snapGroup3.Count];
		for (int l = 0; l < snapGroup3.Count; l++)
		{
			UILinkPointNavigator.SetPosition(num, snapGroup3[l].Position);
			uILinkPoint = UILinkPointNavigator.Points[num];
			uILinkPoint.Unlink();
			array3[l] = uILinkPoint;
			num++;
		}
		LoopHorizontalLineLinks(array);
		LoopHorizontalLineLinks(array2);
		EstablishUpDownRelationship(array, array2);
		for (int m = 0; m < array.Length; m++)
		{
			array[m].Up = uILinkPoint7.ID;
		}
		if (true)
		{
			LoopHorizontalLineLinks(array3);
			EstablishUpDownRelationship(array2, array3);
			for (int n = 0; n < array3.Length; n++)
			{
				array3[n].Down = uILinkPoint2.ID;
			}
			array3[array3.Length - 1].Down = uILinkPoint3.ID;
			uILinkPoint3.Up = array3[array3.Length - 1].ID;
			uILinkPoint2.Up = array3[0].ID;
		}
		uILinkPoint3.Left = uILinkPoint2.ID;
		uILinkPoint2.Right = uILinkPoint3.ID;
		uILinkPoint5.Down = uILinkPoint7.ID;
		uILinkPoint5.Left = uILinkPoint4.ID;
		uILinkPoint4.Right = uILinkPoint5.ID;
		uILinkPoint7.Up = uILinkPoint5.ID;
		uILinkPoint7.Down = array[0].ID;
		uILinkPoint7.Left = uILinkPoint6.ID;
		uILinkPoint6.Right = uILinkPoint7.ID;
		uILinkPoint6.Up = uILinkPoint4.ID;
		uILinkPoint6.Down = array[0].ID;
		uILinkPoint4.Down = uILinkPoint6.ID;
	}

	private void EstablishUpDownRelationship(UILinkPoint[] topSide, UILinkPoint[] bottomSide)
	{
		int num = Math.Max(topSide.Length, bottomSide.Length);
		for (int i = 0; i < num; i++)
		{
			int num2 = Math.Min(i, topSide.Length - 1);
			int num3 = Math.Min(i, bottomSide.Length - 1);
			topSide[num2].Down = bottomSide[num3].ID;
			bottomSide[num3].Up = topSide[num2].ID;
		}
	}

	private void LoopHorizontalLineLinks(UILinkPoint[] pointsLine)
	{
		for (int i = 1; i < pointsLine.Length - 1; i++)
		{
			pointsLine[i - 1].Right = pointsLine[i].ID;
			pointsLine[i].Left = pointsLine[i - 1].ID;
			pointsLine[i].Right = pointsLine[i + 1].ID;
			pointsLine[i + 1].Left = pointsLine[i].ID;
		}
	}

	private List<SnapPoint> GetSnapGroup(List<SnapPoint> ptsOnPage, string groupName)
	{
		List<SnapPoint> list = ptsOnPage.Where((SnapPoint a) => a.Name == groupName).ToList();
		list.Sort(SortPoints);
		return list;
	}

	private int SortPoints(SnapPoint a, SnapPoint b)
	{
		return a.Id.CompareTo(b.Id);
	}

	public void AddSeedFromSeedmenu(string seed)
	{
		_secretSeedTextsEntered.Add(seed);
	}

	public void RemoveSeedFromSeedMenu(string seed)
	{
		_secretSeedTextsEntered.Remove(seed);
	}

	public void HandleBackButtonUsage()
	{
		GoBack();
	}
}
