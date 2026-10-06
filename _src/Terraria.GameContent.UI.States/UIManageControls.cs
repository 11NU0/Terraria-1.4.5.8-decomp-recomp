using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ReLogic.Content;
using Terraria.Audio;
using Terraria.GameContent.UI.Chat;
using Terraria.GameContent.UI.Elements;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.Initializers;
using Terraria.Localization;
using Terraria.UI;
using Terraria.UI.Gamepad;

namespace Terraria.GameContent.UI.States;

public class UIManageControls : UIState
{
	public class SpecialControls
	{
		public const string MouseSnapToggle = "sp1";

		public const string MouseHotbarToggle = "sp2";

		public const string TriggersDeadZone = "sp3";

		public const string SlidersDeadZone = "sp4";

		public const string LeftXDeadZone = "sp5";

		public const string LeftYDeadZone = "sp6";

		public const string RightXDeadZone = "sp7";

		public const string RightYDeadZone = "sp8";

		public const string ResetGameplay = "sp9";

		public const string ResetHotbar = "sp10";

		public const string ResetMap = "sp11";

		public const string ResetGamepad = "sp12";

		public const string ResetGamepadAdvanced = "sp13";

		public const string InvertLeftX = "sp14";

		public const string InvertLeftY = "sp15";

		public const string InvertRightX = "sp16";

		public const string InvertRightY = "sp17";

		public const string TimeBeforeRadial = "sp18";

		public const string TicksPerInventoryMovement = "sp19";

		public const string DisableDoubleTapForDashing = "sp20";

		public const string CameraPanSetting_Keyboard = "sp21";

		public const string CameraPanSetting_Gamepad = "sp22";
	}

	public static int ForceMoveTo = -1;

	private const float PanelTextureHeight = 30f;

	private static List<string> _BindingsFullLine = new List<string>
	{
		"Throw", "Inventory", "RadialHotbar", "RadialQuickbar", "LockOn", "ToggleCreativeMenu", "Loadout1", "Loadout2", "Loadout3", "ToggleCameraMode",
		"sp3", "sp4", "sp5", "sp6", "sp7", "sp8", "sp18", "sp19", "sp9", "sp10",
		"sp11", "sp12", "sp13", "ArmorSetAbility", "sp22", "sp21"
	};

	private static List<string> _BindingsHalfSingleLine = new List<string> { "sp9", "sp10", "sp11", "sp12", "sp13", "sp20" };

	private bool OnKeyboard = true;

	private bool OnGameplay = true;

	private List<UIElement> _bindsKeyboard = new List<UIElement>();

	private List<UIElement> _bindsGamepad = new List<UIElement>();

	private List<UIElement> _bindsKeyboardUI = new List<UIElement>();

	private List<UIElement> _bindsGamepadUI = new List<UIElement>();

	private UIElement _outerContainer;

	private UIList _uilist;

	private UIImageFramed _buttonKeyboard;

	private UIImageFramed _buttonGamepad;

	private UIImageFramed _buttonBorder1;

	private UIImageFramed _buttonBorder2;

	private UIKeybindingSimpleListItem _buttonProfile;

	private UIKeybindingSimpleListItem _buttonStyle;

	private UIElement _buttonBack;

	private UIImageFramed _buttonVs1;

	private UIImageFramed _buttonVs2;

	private UIImageFramed _buttonBorderVs1;

	private UIImageFramed _buttonBorderVs2;

	private Asset<Texture2D> _KeyboardGamepadTexture;

	private Asset<Texture2D> _keyboardGamepadBorderTexture;

	private Asset<Texture2D> _GameplayVsUITexture;

	private Asset<Texture2D> _GameplayVsUIBorderTexture;

	private static int SnapPointIndex;

	public override void OnInitialize()
	{
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0105: Unknown result type (might be due to invalid IL or missing references)
		//IL_010a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0129: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0320: Unknown result type (might be due to invalid IL or missing references)
		//IL_0355: Unknown result type (might be due to invalid IL or missing references)
		//IL_03be: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_0581: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_061f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0688: Unknown result type (might be due to invalid IL or missing references)
		//IL_06c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_06cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_07c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_07d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_09e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_09e7: Unknown result type (might be due to invalid IL or missing references)
		_KeyboardGamepadTexture = Main.Assets.Request<Texture2D>("Images/UI/Settings_Inputs", (AssetRequestMode)1);
		_keyboardGamepadBorderTexture = Main.Assets.Request<Texture2D>("Images/UI/Settings_Inputs_Border", (AssetRequestMode)1);
		_GameplayVsUITexture = Main.Assets.Request<Texture2D>("Images/UI/Settings_Inputs_2", (AssetRequestMode)1);
		_GameplayVsUIBorderTexture = Main.Assets.Request<Texture2D>("Images/UI/Settings_Inputs_2_Border", (AssetRequestMode)1);
		UIElement uIElement = new UIElement();
		uIElement.Width.Set(0f, 0.8f);
		uIElement.MaxWidth.Set(600f, 0f);
		uIElement.Top.Set(220f, 0f);
		uIElement.Height.Set(-200f, 1f);
		uIElement.HAlign = 0.5f;
		_outerContainer = uIElement;
		UIPanel uIPanel = new UIPanel();
		uIPanel.Width.Set(0f, 1f);
		uIPanel.Height.Set(-110f, 1f);
		uIPanel.BackgroundColor = new Color(33, 43, 79) * 0.8f;
		uIElement.Append(uIPanel);
		_buttonKeyboard = new UIImageFramed(_KeyboardGamepadTexture, _KeyboardGamepadTexture.Frame(2, 2));
		_buttonKeyboard.VAlign = 0f;
		_buttonKeyboard.HAlign = 0f;
		_buttonKeyboard.Left.Set(0f, 0f);
		_buttonKeyboard.Top.Set(8f, 0f);
		_buttonKeyboard.OnLeftClick += KeyboardButtonClick;
		_buttonKeyboard.OnMouseOver += ManageBorderKeyboardOn;
		_buttonKeyboard.OnMouseOut += ManageBorderKeyboardOff;
		uIPanel.Append(_buttonKeyboard);
		_buttonGamepad = new UIImageFramed(_KeyboardGamepadTexture, _KeyboardGamepadTexture.Frame(2, 2, 1, 1));
		_buttonGamepad.VAlign = 0f;
		_buttonGamepad.HAlign = 0f;
		_buttonGamepad.Left.Set(76f, 0f);
		_buttonGamepad.Top.Set(8f, 0f);
		_buttonGamepad.OnLeftClick += GamepadButtonClick;
		_buttonGamepad.OnMouseOver += ManageBorderGamepadOn;
		_buttonGamepad.OnMouseOut += ManageBorderGamepadOff;
		uIPanel.Append(_buttonGamepad);
		_buttonBorder1 = new UIImageFramed(_keyboardGamepadBorderTexture, _keyboardGamepadBorderTexture.Frame());
		_buttonBorder1.VAlign = 0f;
		_buttonBorder1.HAlign = 0f;
		_buttonBorder1.Left.Set(0f, 0f);
		_buttonBorder1.Top.Set(8f, 0f);
		_buttonBorder1.Color = Color.Silver;
		_buttonBorder1.IgnoresMouseInteraction = true;
		uIPanel.Append(_buttonBorder1);
		_buttonBorder2 = new UIImageFramed(_keyboardGamepadBorderTexture, _keyboardGamepadBorderTexture.Frame());
		_buttonBorder2.VAlign = 0f;
		_buttonBorder2.HAlign = 0f;
		_buttonBorder2.Left.Set(76f, 0f);
		_buttonBorder2.Top.Set(8f, 0f);
		_buttonBorder2.Color = Color.Transparent;
		_buttonBorder2.IgnoresMouseInteraction = true;
		uIPanel.Append(_buttonBorder2);
		_buttonVs1 = new UIImageFramed(_GameplayVsUITexture, _GameplayVsUITexture.Frame(2, 2));
		_buttonVs1.VAlign = 0f;
		_buttonVs1.HAlign = 0f;
		_buttonVs1.Left.Set(172f, 0f);
		_buttonVs1.Top.Set(8f, 0f);
		_buttonVs1.OnLeftClick += VsGameplayButtonClick;
		_buttonVs1.OnMouseOver += ManageBorderGameplayOn;
		_buttonVs1.OnMouseOut += ManageBorderGameplayOff;
		uIPanel.Append(_buttonVs1);
		_buttonVs2 = new UIImageFramed(_GameplayVsUITexture, _GameplayVsUITexture.Frame(2, 2, 1, 1));
		_buttonVs2.VAlign = 0f;
		_buttonVs2.HAlign = 0f;
		_buttonVs2.Left.Set(212f, 0f);
		_buttonVs2.Top.Set(8f, 0f);
		_buttonVs2.OnLeftClick += VsMenuButtonClick;
		_buttonVs2.OnMouseOver += ManageBorderMenuOn;
		_buttonVs2.OnMouseOut += ManageBorderMenuOff;
		uIPanel.Append(_buttonVs2);
		_buttonBorderVs1 = new UIImageFramed(_GameplayVsUIBorderTexture, _GameplayVsUIBorderTexture.Frame());
		_buttonBorderVs1.VAlign = 0f;
		_buttonBorderVs1.HAlign = 0f;
		_buttonBorderVs1.Left.Set(172f, 0f);
		_buttonBorderVs1.Top.Set(8f, 0f);
		_buttonBorderVs1.Color = Color.Silver;
		_buttonBorderVs1.IgnoresMouseInteraction = true;
		uIPanel.Append(_buttonBorderVs1);
		_buttonBorderVs2 = new UIImageFramed(_GameplayVsUIBorderTexture, _GameplayVsUIBorderTexture.Frame());
		_buttonBorderVs2.VAlign = 0f;
		_buttonBorderVs2.HAlign = 0f;
		_buttonBorderVs2.Left.Set(212f, 0f);
		_buttonBorderVs2.Top.Set(8f, 0f);
		_buttonBorderVs2.Color = Color.Transparent;
		_buttonBorderVs2.IgnoresMouseInteraction = true;
		uIPanel.Append(_buttonBorderVs2);
		_buttonStyle = new UIKeybindingSimpleListItem(() => controllerGlyphStyle(), new Color(73, 94, 171, 255) * 0.9f);
		_buttonStyle.VAlign = 0f;
		_buttonStyle.HAlign = 1f;
		_buttonStyle.Width.Set(100f, 0f);
		_buttonStyle.Height.Set(30f, 0f);
		_buttonStyle.MarginRight = 220f;
		_buttonStyle.Left.Set(0f, 0f);
		_buttonStyle.Top.Set(8f, 0f);
		_buttonStyle.OnLeftClick += controllerGlyphButtonClick;
		uIPanel.Append(_buttonStyle);
		_buttonProfile = new UIKeybindingSimpleListItem(() => PlayerInput.CurrentProfile.ShowName, new Color(73, 94, 171, 255) * 0.9f);
		_buttonProfile.VAlign = 0f;
		_buttonProfile.HAlign = 1f;
		_buttonProfile.Width.Set(180f, 0f);
		_buttonProfile.Height.Set(30f, 0f);
		_buttonProfile.MarginRight = 30f;
		_buttonProfile.Left.Set(0f, 0f);
		_buttonProfile.Top.Set(8f, 0f);
		_buttonProfile.OnLeftClick += profileButtonClick;
		uIPanel.Append(_buttonProfile);
		_uilist = new UIList();
		_uilist.Width.Set(-25f, 1f);
		_uilist.Height.Set(-50f, 1f);
		_uilist.VAlign = 1f;
		_uilist.PaddingBottom = 5f;
		_uilist.ListPadding = 20f;
		uIPanel.Append(_uilist);
		AssembleBindPanels();
		FillList();
		UIScrollbar uIScrollbar = new UIScrollbar();
		uIScrollbar.SetView(100f, 1000f);
		uIScrollbar.Height.Set(-67f, 1f);
		uIScrollbar.HAlign = 1f;
		uIScrollbar.VAlign = 1f;
		uIScrollbar.MarginBottom = 11f;
		uIPanel.Append(uIScrollbar);
		_uilist.SetScrollbar(uIScrollbar);
		UITextPanel<LocalizedText> uITextPanel = new UITextPanel<LocalizedText>(Language.GetText("UI.Keybindings"), 0.7f, large: true);
		uITextPanel.HAlign = 0.5f;
		uITextPanel.Top.Set(-45f, 0f);
		uITextPanel.Left.Set(-10f, 0f);
		uITextPanel.SetPadding(15f);
		uITextPanel.BackgroundColor = new Color(73, 94, 171);
		uIElement.Append(uITextPanel);
		UITextPanel<LocalizedText> uITextPanel2 = new UITextPanel<LocalizedText>(Language.GetText("UI.Back"), 0.7f, large: true);
		uITextPanel2.Width.Set(-10f, 0.5f);
		uITextPanel2.Height.Set(50f, 0f);
		uITextPanel2.VAlign = 1f;
		uITextPanel2.HAlign = 0.5f;
		uITextPanel2.Top.Set(-45f, 0f);
		uITextPanel2.OnMouseOver += FadedMouseOver;
		uITextPanel2.OnMouseOut += FadedMouseOut;
		uITextPanel2.OnLeftClick += GoBackClick;
		uIElement.Append(uITextPanel2);
		_buttonBack = uITextPanel2;
		Append(uIElement);
	}

	private void AssembleBindPanels()
	{
		List<string> bindings = new List<string>
		{
			"MouseLeft", "MouseRight", "Up", "Down", "Left", "Right", "Jump", "Grapple", "SmartSelect", "SmartCursor",
			"QuickMount", "QuickHeal", "QuickMana", "QuickBuff", "Throw", "Inventory", "ToggleCreativeMenu", "ViewZoomIn", "ViewZoomOut", "Loadout1",
			"Loadout2", "Loadout3", "NextLoadout", "PreviousLoadout", "ToggleCameraMode", "ArmorSetAbility", "Dash", "sp20", "sp21", "sp9"
		};
		List<string> bindings2 = new List<string>
		{
			"MouseLeft", "MouseRight", "Up", "Down", "Left", "Right", "Jump", "Grapple", "SmartSelect", "SmartCursor",
			"QuickMount", "QuickHeal", "QuickMana", "QuickBuff", "LockOn", "Throw", "Inventory", "Loadout1", "Loadout2", "Loadout3",
			"NextLoadout", "PreviousLoadout", "ToggleCameraMode", "ArmorSetAbility", "Dash", "sp20", "sp22", "sp9"
		};
		List<string> bindings3 = new List<string>
		{
			"HotbarMinus", "HotbarPlus", "Hotbar1", "Hotbar2", "Hotbar3", "Hotbar4", "Hotbar5", "Hotbar6", "Hotbar7", "Hotbar8",
			"Hotbar9", "Hotbar10", "sp10"
		};
		List<string> bindings4 = new List<string> { "MapZoomIn", "MapZoomOut", "MapAlphaUp", "MapAlphaDown", "MapFull", "MapStyle", "sp11" };
		List<string> bindings5 = new List<string> { "sp1", "sp2", "RadialHotbar", "RadialQuickbar", "sp12" };
		List<string> bindings6 = new List<string>
		{
			"sp3", "sp4", "sp5", "sp6", "sp7", "sp8", "sp14", "sp15", "sp16", "sp17",
			"sp18", "sp19", "sp13"
		};
		InputMode currentInputMode = InputMode.Keyboard;
		_bindsKeyboard.Add(CreateBindingGroup(0, bindings, currentInputMode));
		_bindsKeyboard.Add(CreateBindingGroup(1, bindings4, currentInputMode));
		_bindsKeyboard.Add(CreateBindingGroup(2, bindings3, currentInputMode));
		currentInputMode = InputMode.XBoxGamepad;
		_bindsGamepad.Add(CreateBindingGroup(0, bindings2, currentInputMode));
		_bindsGamepad.Add(CreateBindingGroup(1, bindings4, currentInputMode));
		_bindsGamepad.Add(CreateBindingGroup(2, bindings3, currentInputMode));
		_bindsGamepad.Add(CreateBindingGroup(3, bindings5, currentInputMode));
		_bindsGamepad.Add(CreateBindingGroup(4, bindings6, currentInputMode));
		currentInputMode = InputMode.KeyboardUI;
		_bindsKeyboardUI.Add(CreateBindingGroup(0, bindings, currentInputMode));
		_bindsKeyboardUI.Add(CreateBindingGroup(1, bindings4, currentInputMode));
		_bindsKeyboardUI.Add(CreateBindingGroup(2, bindings3, currentInputMode));
		currentInputMode = InputMode.XBoxGamepadUI;
		_bindsGamepadUI.Add(CreateBindingGroup(0, bindings2, currentInputMode));
		_bindsGamepadUI.Add(CreateBindingGroup(1, bindings4, currentInputMode));
		_bindsGamepadUI.Add(CreateBindingGroup(2, bindings3, currentInputMode));
		_bindsGamepadUI.Add(CreateBindingGroup(3, bindings5, currentInputMode));
		_bindsGamepadUI.Add(CreateBindingGroup(4, bindings6, currentInputMode));
	}

	private UISortableElement CreateBindingGroup(int elementIndex, List<string> bindings, InputMode currentInputMode)
	{
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_0115: Unknown result type (might be due to invalid IL or missing references)
		//IL_011a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0124: Unknown result type (might be due to invalid IL or missing references)
		//IL_0129: Unknown result type (might be due to invalid IL or missing references)
		//IL_0132: Unknown result type (might be due to invalid IL or missing references)
		//IL_0137: Unknown result type (might be due to invalid IL or missing references)
		//IL_0141: Unknown result type (might be due to invalid IL or missing references)
		//IL_0146: Unknown result type (might be due to invalid IL or missing references)
		//IL_014f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0154: Unknown result type (might be due to invalid IL or missing references)
		//IL_015e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0163: Unknown result type (might be due to invalid IL or missing references)
		//IL_016c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0171: Unknown result type (might be due to invalid IL or missing references)
		//IL_017b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0180: Unknown result type (might be due to invalid IL or missing references)
		//IL_0189: Unknown result type (might be due to invalid IL or missing references)
		//IL_018e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0198: Unknown result type (might be due to invalid IL or missing references)
		//IL_019d: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01be: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c8: Unknown result type (might be due to invalid IL or missing references)
		UISortableElement uISortableElement = new UISortableElement(elementIndex);
		uISortableElement.HAlign = 0.5f;
		uISortableElement.Width.Set(0f, 1f);
		uISortableElement.Height.Set(2000f, 0f);
		UIPanel uIPanel = new UIPanel();
		uIPanel.Width.Set(0f, 1f);
		uIPanel.Height.Set(-16f, 1f);
		uIPanel.VAlign = 1f;
		uIPanel.BackgroundColor = new Color(33, 43, 79) * 0.8f;
		uISortableElement.Append(uIPanel);
		UIList uIList = new UIList();
		uIList.OverflowHidden = false;
		uIList.Width.Set(0f, 1f);
		uIList.Height.Set(-8f, 1f);
		uIList.VAlign = 1f;
		uIList.ListPadding = 5f;
		uIPanel.Append(uIList);
		_ = uIPanel.BackgroundColor;
		switch (elementIndex)
		{
		case 0:
			uIPanel.BackgroundColor = Color.Lerp(uIPanel.BackgroundColor, Color.Green, 0.18f);
			break;
		case 1:
			uIPanel.BackgroundColor = Color.Lerp(uIPanel.BackgroundColor, Color.Goldenrod, 0.18f);
			break;
		case 2:
			uIPanel.BackgroundColor = Color.Lerp(uIPanel.BackgroundColor, Color.HotPink, 0.18f);
			break;
		case 3:
			uIPanel.BackgroundColor = Color.Lerp(uIPanel.BackgroundColor, Color.Indigo, 0.18f);
			break;
		case 4:
			uIPanel.BackgroundColor = Color.Lerp(uIPanel.BackgroundColor, Color.Turquoise, 0.18f);
			break;
		}
		CreateElementGroup(uIList, bindings, currentInputMode, uIPanel.BackgroundColor);
		uIPanel.BackgroundColor = uIPanel.BackgroundColor.MultiplyRGBA(new Color(111, 111, 111));
		LocalizedText text = LocalizedText.Empty;
		switch (elementIndex)
		{
		case 0:
			text = ((currentInputMode == InputMode.Keyboard || currentInputMode == InputMode.XBoxGamepad) ? Lang.menu[164] : Lang.menu[243]);
			break;
		case 1:
			text = Lang.menu[165];
			break;
		case 2:
			text = Lang.menu[166];
			break;
		case 3:
			text = Lang.menu[167];
			break;
		case 4:
			text = Lang.menu[198];
			break;
		}
		UITextPanel<LocalizedText> element = new UITextPanel<LocalizedText>(text, 0.7f)
		{
			VAlign = 0f,
			HAlign = 0.5f
		};
		uISortableElement.Append(element);
		uISortableElement.Recalculate();
		float totalHeight = uIList.GetTotalHeight();
		uISortableElement.Width.Set(0f, 1f);
		uISortableElement.Height.Set(totalHeight + 30f + 16f, 0f);
		return uISortableElement;
	}

	private void CreateElementGroup(UIList parent, List<string> bindings, InputMode currentInputMode, Color color)
	{
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0173: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f1: Unknown result type (might be due to invalid IL or missing references)
		for (int i = 0; i < bindings.Count; i++)
		{
			_ = bindings[i];
			UISortableElement uISortableElement = new UISortableElement(i);
			uISortableElement.Width.Set(0f, 1f);
			uISortableElement.Height.Set(30f, 0f);
			uISortableElement.HAlign = 0.5f;
			parent.Add(uISortableElement);
			if (_BindingsHalfSingleLine.Contains(bindings[i]))
			{
				UIElement uIElement = CreatePanel(bindings[i], currentInputMode, color);
				uIElement.Width.Set(0f, 0.5f);
				uIElement.HAlign = 0.5f;
				uIElement.Height.Set(0f, 1f);
				uIElement.SetSnapPoint("Wide", SnapPointIndex++);
				uISortableElement.Append(uIElement);
				continue;
			}
			if (_BindingsFullLine.Contains(bindings[i]))
			{
				UIElement uIElement2 = CreatePanel(bindings[i], currentInputMode, color);
				uIElement2.Width.Set(0f, 1f);
				uIElement2.Height.Set(0f, 1f);
				uIElement2.SetSnapPoint("Wide", SnapPointIndex++);
				uISortableElement.Append(uIElement2);
				continue;
			}
			UIElement uIElement3 = CreatePanel(bindings[i], currentInputMode, color);
			uIElement3.Width.Set(-5f, 0.5f);
			uIElement3.Height.Set(0f, 1f);
			uIElement3.SetSnapPoint("Thin", SnapPointIndex++);
			uISortableElement.Append(uIElement3);
			i++;
			if (i < bindings.Count)
			{
				uIElement3 = CreatePanel(bindings[i], currentInputMode, color);
				uIElement3.Width.Set(-5f, 0.5f);
				uIElement3.Height.Set(0f, 1f);
				uIElement3.HAlign = 1f;
				uIElement3.SetSnapPoint("Thin", SnapPointIndex++);
				uISortableElement.Append(uIElement3);
			}
		}
	}

	public UIElement CreatePanel(string bind, InputMode currentInputMode, Color color)
	{
		//IL_0c2a: Unknown result type (might be due to invalid IL or missing references)
		//IL_07dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_07a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0734: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b23: Unknown result type (might be due to invalid IL or missing references)
		//IL_076c: Unknown result type (might be due to invalid IL or missing references)
		//IL_09a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b8d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0814: Unknown result type (might be due to invalid IL or missing references)
		//IL_093f: Unknown result type (might be due to invalid IL or missing references)
		//IL_08d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0375: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bf7: Unknown result type (might be due to invalid IL or missing references)
		//IL_086b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0466: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_03cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_05fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ade: Unknown result type (might be due to invalid IL or missing references)
		//IL_0576: Unknown result type (might be due to invalid IL or missing references)
		//IL_0686: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a56: Unknown result type (might be due to invalid IL or missing references)
		//IL_070e: Unknown result type (might be due to invalid IL or missing references)
		switch (bind)
		{
		case "sp1":
		{
			UIKeybindingToggleListItem uIKeybindingToggleListItem5 = new UIKeybindingToggleListItem(() => Lang.menu[196].Value, () =>
			{
				//IL_0020: Unknown result type (might be due to invalid IL or missing references)
				//IL_0058: Unknown result type (might be due to invalid IL or missing references)
				//IL_0090: Unknown result type (might be due to invalid IL or missing references)
				//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
				//IL_0100: Unknown result type (might be due to invalid IL or missing references)
				//IL_0138: Unknown result type (might be due to invalid IL or missing references)
				//IL_016d: Unknown result type (might be due to invalid IL or missing references)
				//IL_01a2: Unknown result type (might be due to invalid IL or missing references)
				return PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepad].KeyStatus["DpadSnap1"].Contains(((object)(Buttons)1/*cast due to constrained. prefix*/).ToString()) && PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepad].KeyStatus["DpadSnap2"].Contains(((object)(Buttons)8/*cast due to constrained. prefix*/).ToString()) && PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepad].KeyStatus["DpadSnap3"].Contains(((object)(Buttons)2/*cast due to constrained. prefix*/).ToString()) && PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepad].KeyStatus["DpadSnap4"].Contains(((object)(Buttons)4/*cast due to constrained. prefix*/).ToString()) && PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepadUI].KeyStatus["DpadSnap1"].Contains(((object)(Buttons)1/*cast due to constrained. prefix*/).ToString()) && PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepadUI].KeyStatus["DpadSnap2"].Contains(((object)(Buttons)8/*cast due to constrained. prefix*/).ToString()) && PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepadUI].KeyStatus["DpadSnap3"].Contains(((object)(Buttons)2/*cast due to constrained. prefix*/).ToString()) && PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepadUI].KeyStatus["DpadSnap4"].Contains(((object)(Buttons)4/*cast due to constrained. prefix*/).ToString());
			}, color);
			uIKeybindingToggleListItem5.OnLeftClick += SnapButtonClick;
			return uIKeybindingToggleListItem5;
		}
		case "sp2":
		{
			UIKeybindingToggleListItem uIKeybindingToggleListItem6 = new UIKeybindingToggleListItem(() => Lang.menu[197].Value, () =>
			{
				//IL_0020: Unknown result type (might be due to invalid IL or missing references)
				//IL_0058: Unknown result type (might be due to invalid IL or missing references)
				//IL_0090: Unknown result type (might be due to invalid IL or missing references)
				//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
				//IL_0100: Unknown result type (might be due to invalid IL or missing references)
				//IL_0138: Unknown result type (might be due to invalid IL or missing references)
				//IL_016d: Unknown result type (might be due to invalid IL or missing references)
				//IL_01a2: Unknown result type (might be due to invalid IL or missing references)
				return PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepad].KeyStatus["DpadRadial1"].Contains(((object)(Buttons)1/*cast due to constrained. prefix*/).ToString()) && PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepad].KeyStatus["DpadRadial2"].Contains(((object)(Buttons)8/*cast due to constrained. prefix*/).ToString()) && PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepad].KeyStatus["DpadRadial3"].Contains(((object)(Buttons)2/*cast due to constrained. prefix*/).ToString()) && PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepad].KeyStatus["DpadRadial4"].Contains(((object)(Buttons)4/*cast due to constrained. prefix*/).ToString()) && PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepadUI].KeyStatus["DpadRadial1"].Contains(((object)(Buttons)1/*cast due to constrained. prefix*/).ToString()) && PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepadUI].KeyStatus["DpadRadial2"].Contains(((object)(Buttons)8/*cast due to constrained. prefix*/).ToString()) && PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepadUI].KeyStatus["DpadRadial3"].Contains(((object)(Buttons)2/*cast due to constrained. prefix*/).ToString()) && PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepadUI].KeyStatus["DpadRadial4"].Contains(((object)(Buttons)4/*cast due to constrained. prefix*/).ToString());
			}, color);
			uIKeybindingToggleListItem6.OnLeftClick += RadialButtonClick;
			return uIKeybindingToggleListItem6;
		}
		case "sp3":
			return new UIKeybindingSliderItem(() => Lang.menu[199].Value + " (" + PlayerInput.CurrentProfile.TriggersDeadzone.ToString("P1") + ")", () => PlayerInput.CurrentProfile.TriggersDeadzone, (float f) =>
			{
				PlayerInput.CurrentProfile.TriggersDeadzone = f;
			}, () =>
			{
				PlayerInput.CurrentProfile.TriggersDeadzone = UILinksInitializer.HandleSliderHorizontalInput(PlayerInput.CurrentProfile.TriggersDeadzone, 0f, 0.95f, PlayerInput.CurrentProfile.InterfaceDeadzoneX, 0.35f);
			}, 1000, color);
		case "sp4":
			return new UIKeybindingSliderItem(() => Lang.menu[200].Value + " (" + PlayerInput.CurrentProfile.InterfaceDeadzoneX.ToString("P1") + ")", () => PlayerInput.CurrentProfile.InterfaceDeadzoneX, (float f) =>
			{
				PlayerInput.CurrentProfile.InterfaceDeadzoneX = f;
			}, () =>
			{
				PlayerInput.CurrentProfile.InterfaceDeadzoneX = UILinksInitializer.HandleSliderHorizontalInput(PlayerInput.CurrentProfile.InterfaceDeadzoneX, 0f, 0.95f, 0.35f, 0.35f);
			}, 1001, color);
		case "sp5":
			return new UIKeybindingSliderItem(() => Lang.menu[201].Value + " (" + PlayerInput.CurrentProfile.LeftThumbstickDeadzoneX.ToString("P1") + ")", () => PlayerInput.CurrentProfile.LeftThumbstickDeadzoneX, (float f) =>
			{
				PlayerInput.CurrentProfile.LeftThumbstickDeadzoneX = f;
			}, () =>
			{
				PlayerInput.CurrentProfile.LeftThumbstickDeadzoneX = UILinksInitializer.HandleSliderHorizontalInput(PlayerInput.CurrentProfile.LeftThumbstickDeadzoneX, 0f, 0.95f, PlayerInput.CurrentProfile.InterfaceDeadzoneX, 0.35f);
			}, 1002, color);
		case "sp6":
			return new UIKeybindingSliderItem(() => Lang.menu[202].Value + " (" + PlayerInput.CurrentProfile.LeftThumbstickDeadzoneY.ToString("P1") + ")", () => PlayerInput.CurrentProfile.LeftThumbstickDeadzoneY, (float f) =>
			{
				PlayerInput.CurrentProfile.LeftThumbstickDeadzoneY = f;
			}, () =>
			{
				PlayerInput.CurrentProfile.LeftThumbstickDeadzoneY = UILinksInitializer.HandleSliderHorizontalInput(PlayerInput.CurrentProfile.LeftThumbstickDeadzoneY, 0f, 0.95f, PlayerInput.CurrentProfile.InterfaceDeadzoneX, 0.35f);
			}, 1003, color);
		case "sp7":
			return new UIKeybindingSliderItem(() => Lang.menu[203].Value + " (" + PlayerInput.CurrentProfile.RightThumbstickDeadzoneX.ToString("P1") + ")", () => PlayerInput.CurrentProfile.RightThumbstickDeadzoneX, (float f) =>
			{
				PlayerInput.CurrentProfile.RightThumbstickDeadzoneX = f;
			}, () =>
			{
				PlayerInput.CurrentProfile.RightThumbstickDeadzoneX = UILinksInitializer.HandleSliderHorizontalInput(PlayerInput.CurrentProfile.RightThumbstickDeadzoneX, 0f, 0.95f, PlayerInput.CurrentProfile.InterfaceDeadzoneX, 0.35f);
			}, 1004, color);
		case "sp8":
			return new UIKeybindingSliderItem(() => Lang.menu[204].Value + " (" + PlayerInput.CurrentProfile.RightThumbstickDeadzoneY.ToString("P1") + ")", () => PlayerInput.CurrentProfile.RightThumbstickDeadzoneY, (float f) =>
			{
				PlayerInput.CurrentProfile.RightThumbstickDeadzoneY = f;
			}, () =>
			{
				PlayerInput.CurrentProfile.RightThumbstickDeadzoneY = UILinksInitializer.HandleSliderHorizontalInput(PlayerInput.CurrentProfile.RightThumbstickDeadzoneY, 0f, 0.95f, PlayerInput.CurrentProfile.InterfaceDeadzoneX, 0.35f);
			}, 1005, color);
		case "sp9":
		{
			UIKeybindingSimpleListItem uIKeybindingSimpleListItem3 = new UIKeybindingSimpleListItem(() => Lang.menu[86].Value, color);
			uIKeybindingSimpleListItem3.OnLeftClick += (UIMouseEvent evt, UIElement listeningElement) =>
			{
				string copyableProfileName = GetCopyableProfileName();
				PlayerInput.CurrentProfile.CopyGameplaySettingsFrom(PlayerInput.OriginalProfiles[copyableProfileName], currentInputMode);
			};
			return uIKeybindingSimpleListItem3;
		}
		case "sp10":
		{
			UIKeybindingSimpleListItem uIKeybindingSimpleListItem5 = new UIKeybindingSimpleListItem(() => Lang.menu[86].Value, color);
			uIKeybindingSimpleListItem5.OnLeftClick += (UIMouseEvent evt, UIElement listeningElement) =>
			{
				string copyableProfileName = GetCopyableProfileName();
				PlayerInput.CurrentProfile.CopyHotbarSettingsFrom(PlayerInput.OriginalProfiles[copyableProfileName], currentInputMode);
			};
			return uIKeybindingSimpleListItem5;
		}
		case "sp11":
		{
			UIKeybindingSimpleListItem uIKeybindingSimpleListItem2 = new UIKeybindingSimpleListItem(() => Lang.menu[86].Value, color);
			uIKeybindingSimpleListItem2.OnLeftClick += (UIMouseEvent evt, UIElement listeningElement) =>
			{
				string copyableProfileName = GetCopyableProfileName();
				PlayerInput.CurrentProfile.CopyMapSettingsFrom(PlayerInput.OriginalProfiles[copyableProfileName], currentInputMode);
			};
			return uIKeybindingSimpleListItem2;
		}
		case "sp12":
		{
			UIKeybindingSimpleListItem uIKeybindingSimpleListItem = new UIKeybindingSimpleListItem(() => Lang.menu[86].Value, color);
			uIKeybindingSimpleListItem.OnLeftClick += (UIMouseEvent evt, UIElement listeningElement) =>
			{
				string copyableProfileName = GetCopyableProfileName();
				PlayerInput.CurrentProfile.CopyGamepadSettingsFrom(PlayerInput.OriginalProfiles[copyableProfileName], currentInputMode);
			};
			return uIKeybindingSimpleListItem;
		}
		case "sp13":
		{
			UIKeybindingSimpleListItem uIKeybindingSimpleListItem4 = new UIKeybindingSimpleListItem(() => Lang.menu[86].Value, color);
			uIKeybindingSimpleListItem4.OnLeftClick += (UIMouseEvent evt, UIElement listeningElement) =>
			{
				string copyableProfileName = GetCopyableProfileName();
				PlayerInput.CurrentProfile.CopyGamepadAdvancedSettingsFrom(PlayerInput.OriginalProfiles[copyableProfileName], currentInputMode);
			};
			return uIKeybindingSimpleListItem4;
		}
		case "sp14":
		{
			UIKeybindingToggleListItem uIKeybindingToggleListItem = new UIKeybindingToggleListItem(() => Lang.menu[205].Value, () => PlayerInput.CurrentProfile.LeftThumbstickInvertX, color);
			uIKeybindingToggleListItem.OnLeftClick += (UIMouseEvent evt, UIElement listeningElement) =>
			{
				if (PlayerInput.CurrentProfile.AllowEditing)
				{
					PlayerInput.CurrentProfile.LeftThumbstickInvertX = !PlayerInput.CurrentProfile.LeftThumbstickInvertX;
				}
			};
			return uIKeybindingToggleListItem;
		}
		case "sp15":
		{
			UIKeybindingToggleListItem uIKeybindingToggleListItem4 = new UIKeybindingToggleListItem(() => Lang.menu[206].Value, () => PlayerInput.CurrentProfile.LeftThumbstickInvertY, color);
			uIKeybindingToggleListItem4.OnLeftClick += (UIMouseEvent evt, UIElement listeningElement) =>
			{
				if (PlayerInput.CurrentProfile.AllowEditing)
				{
					PlayerInput.CurrentProfile.LeftThumbstickInvertY = !PlayerInput.CurrentProfile.LeftThumbstickInvertY;
				}
			};
			return uIKeybindingToggleListItem4;
		}
		case "sp16":
		{
			UIKeybindingToggleListItem uIKeybindingToggleListItem2 = new UIKeybindingToggleListItem(() => Lang.menu[207].Value, () => PlayerInput.CurrentProfile.RightThumbstickInvertX, color);
			uIKeybindingToggleListItem2.OnLeftClick += (UIMouseEvent evt, UIElement listeningElement) =>
			{
				if (PlayerInput.CurrentProfile.AllowEditing)
				{
					PlayerInput.CurrentProfile.RightThumbstickInvertX = !PlayerInput.CurrentProfile.RightThumbstickInvertX;
				}
			};
			return uIKeybindingToggleListItem2;
		}
		case "sp17":
		{
			UIKeybindingToggleListItem uIKeybindingToggleListItem7 = new UIKeybindingToggleListItem(() => Lang.menu[208].Value, () => PlayerInput.CurrentProfile.RightThumbstickInvertY, color);
			uIKeybindingToggleListItem7.OnLeftClick += (UIMouseEvent evt, UIElement listeningElement) =>
			{
				if (PlayerInput.CurrentProfile.AllowEditing)
				{
					PlayerInput.CurrentProfile.RightThumbstickInvertY = !PlayerInput.CurrentProfile.RightThumbstickInvertY;
				}
			};
			return uIKeybindingToggleListItem7;
		}
		case "sp18":
			return new UIKeybindingSliderItem(() =>
			{
				int hotbarRadialHoldTimeRequired = PlayerInput.CurrentProfile.HotbarRadialHoldTimeRequired;
				return (hotbarRadialHoldTimeRequired == -1) ? Lang.menu[228].Value : (Lang.menu[227].Value + " (" + ((float)hotbarRadialHoldTimeRequired / 60f).ToString("F2") + "s)");
			}, () => (PlayerInput.CurrentProfile.HotbarRadialHoldTimeRequired == -1) ? 1f : ((float)PlayerInput.CurrentProfile.HotbarRadialHoldTimeRequired / 301f), (float f) =>
			{
				PlayerInput.CurrentProfile.HotbarRadialHoldTimeRequired = (int)(f * 301f);
				if ((float)PlayerInput.CurrentProfile.HotbarRadialHoldTimeRequired == 301f)
				{
					PlayerInput.CurrentProfile.HotbarRadialHoldTimeRequired = -1;
				}
			}, () =>
			{
				float currentValue = ((PlayerInput.CurrentProfile.HotbarRadialHoldTimeRequired == -1) ? 1f : ((float)PlayerInput.CurrentProfile.HotbarRadialHoldTimeRequired / 301f));
				currentValue = UILinksInitializer.HandleSliderHorizontalInput(currentValue, 0f, 1f, PlayerInput.CurrentProfile.InterfaceDeadzoneX);
				PlayerInput.CurrentProfile.HotbarRadialHoldTimeRequired = (int)(currentValue * 301f);
				if ((float)PlayerInput.CurrentProfile.HotbarRadialHoldTimeRequired == 301f)
				{
					PlayerInput.CurrentProfile.HotbarRadialHoldTimeRequired = -1;
				}
			}, 1007, color);
		case "sp19":
			return new UIKeybindingSliderItem(() =>
			{
				int inventoryMoveCD = PlayerInput.CurrentProfile.InventoryMoveCD;
				return Lang.menu[252].Value + " (" + ((float)inventoryMoveCD / 60f).ToString("F2") + "s)";
			}, () => Utils.GetLerpValue(4f, 12f, PlayerInput.CurrentProfile.InventoryMoveCD, clamped: true), (float f) =>
			{
				PlayerInput.CurrentProfile.InventoryMoveCD = (int)Math.Round(MathHelper.Lerp(4f, 12f, f));
			}, () =>
			{
				if (UILinkPointNavigator.Shortcuts.INV_MOVE_OPTION_CD > 0)
				{
					UILinkPointNavigator.Shortcuts.INV_MOVE_OPTION_CD--;
				}
				if (UILinkPointNavigator.Shortcuts.INV_MOVE_OPTION_CD == 0)
				{
					float lerpValue = Utils.GetLerpValue(4f, 12f, PlayerInput.CurrentProfile.InventoryMoveCD, clamped: true);
					float num = UILinksInitializer.HandleSliderHorizontalInput(lerpValue, 0f, 1f, PlayerInput.CurrentProfile.InterfaceDeadzoneX);
					if (lerpValue != num)
					{
						UILinkPointNavigator.Shortcuts.INV_MOVE_OPTION_CD = 8;
						int num2 = Math.Sign(num - lerpValue);
						PlayerInput.CurrentProfile.InventoryMoveCD = (int)MathHelper.Clamp((float)(PlayerInput.CurrentProfile.InventoryMoveCD + num2), 4f, 12f);
					}
				}
			}, 1008, color);
		case "sp20":
		{
			UIKeybindingToggleListItem uIKeybindingToggleListItem3 = new UIKeybindingToggleListItem(() => Language.GetTextValue("UI.DoubleTapDash"), () => Player.Settings.DashControl == Player.Settings.DashPreference.AllowDoubleTap, color);
			uIKeybindingToggleListItem3.OnLeftClick += (UIMouseEvent evt, UIElement listeningElement) =>
			{
				switch (Player.Settings.DashControl)
				{
				case Player.Settings.DashPreference.AllowDoubleTap:
					Player.Settings.DashControl = Player.Settings.DashPreference.OnlyThroughHotkeys;
					break;
				case Player.Settings.DashPreference.OnlyThroughHotkeys:
					Player.Settings.DashControl = Player.Settings.DashPreference.AllowDoubleTap;
					break;
				}
			};
			return uIKeybindingToggleListItem3;
		}
		case "sp21":
		{
			UIKeybindingCycleListItem uIKeybindingCycleListItem2 = new UIKeybindingCycleListItem(() => Language.GetTextValue("UI.PanControlMode"), () => PlayerInput.CurrentProfile.PanControl_Keyboard switch
			{
				ButtonControlMode.OnAlways => Language.GetTextValue("UI.PanControlMode_OnAlways"), 
				ButtonControlMode.OffAlways => Language.GetTextValue("UI.PanControlMode_OffAlways"), 
				ButtonControlMode.Hold => Language.GetTextValue("UI.PanControlMode_Hold"), 
				ButtonControlMode.Click => Language.GetTextValue("UI.PanControlMode_Click"), 
				_ => "???", 
			}, color);
			uIKeybindingCycleListItem2.OnLeftClick += (UIMouseEvent evt, UIElement listeningElement) =>
			{
				Utils.CycleControlControl_OffOnClickHold(ref PlayerInput.CurrentProfile.PanControl_Keyboard);
			};
			return uIKeybindingCycleListItem2;
		}
		case "sp22":
		{
			UIKeybindingCycleListItem uIKeybindingCycleListItem = new UIKeybindingCycleListItem(() => Language.GetTextValue("UI.PanControlMode"), () => PlayerInput.CurrentProfile.PanControl_Gamepad switch
			{
				ButtonControlMode.OnAlways => Language.GetTextValue("UI.PanControlMode_OnAlways"), 
				ButtonControlMode.OffAlways => Language.GetTextValue("UI.PanControlMode_OffAlways"), 
				ButtonControlMode.Hold => Language.GetTextValue("UI.PanControlMode_Hold"), 
				ButtonControlMode.Click => Language.GetTextValue("UI.PanControlMode_Click"), 
				_ => "???", 
			}, color);
			uIKeybindingCycleListItem.OnLeftClick += (UIMouseEvent evt, UIElement listeningElement) =>
			{
				Utils.CycleControlControl_OffOnClickHold(ref PlayerInput.CurrentProfile.PanControl_Gamepad);
			};
			return uIKeybindingCycleListItem;
		}
		default:
			return new UIKeybindingListItem(bind, currentInputMode, color);
		}
	}

	public override void OnActivate()
	{
		if (Main.gameMenu)
		{
			_outerContainer.Top.Set(220f, 0f);
			_outerContainer.Height.Set(-220f, 1f);
		}
		else
		{
			_outerContainer.Top.Set(120f, 0f);
			_outerContainer.Height.Set(-120f, 1f);
		}
		if (PlayerInput.UsingGamepadUI)
		{
			UILinkPointNavigator.ChangePoint(3002);
		}
	}

	private static string GetCopyableProfileName()
	{
		string result = "Redigit's Pick";
		if (PlayerInput.OriginalProfiles.ContainsKey(PlayerInput.CurrentProfile.Name))
		{
			result = PlayerInput.CurrentProfile.Name;
		}
		return result;
	}

	private void FillList()
	{
		List<UIElement> list = _bindsKeyboard;
		if (!OnKeyboard)
		{
			list = _bindsGamepad;
		}
		if (!OnGameplay)
		{
			list = (OnKeyboard ? _bindsKeyboardUI : _bindsGamepadUI);
		}
		_uilist.Clear();
		foreach (UIElement item in list)
		{
			_uilist.Add(item);
		}
	}

	private void SnapButtonClick(UIMouseEvent evt, UIElement listeningElement)
	{
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0446: Unknown result type (might be due to invalid IL or missing references)
		//IL_047f: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_052a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0563: Unknown result type (might be due to invalid IL or missing references)
		//IL_059c: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_0124: Unknown result type (might be due to invalid IL or missing references)
		//IL_015c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0194: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cc: Unknown result type (might be due to invalid IL or missing references)
		if (PlayerInput.CurrentProfile.AllowEditing)
		{
			SoundEngine.PlaySound(12);
			if (PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepad].KeyStatus["DpadSnap1"].Contains(((object)(Buttons)1/*cast due to constrained. prefix*/).ToString()) && PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepad].KeyStatus["DpadSnap2"].Contains(((object)(Buttons)8/*cast due to constrained. prefix*/).ToString()) && PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepad].KeyStatus["DpadSnap3"].Contains(((object)(Buttons)2/*cast due to constrained. prefix*/).ToString()) && PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepad].KeyStatus["DpadSnap4"].Contains(((object)(Buttons)4/*cast due to constrained. prefix*/).ToString()) && PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepadUI].KeyStatus["DpadSnap1"].Contains(((object)(Buttons)1/*cast due to constrained. prefix*/).ToString()) && PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepadUI].KeyStatus["DpadSnap2"].Contains(((object)(Buttons)8/*cast due to constrained. prefix*/).ToString()) && PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepadUI].KeyStatus["DpadSnap3"].Contains(((object)(Buttons)2/*cast due to constrained. prefix*/).ToString()) && PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepadUI].KeyStatus["DpadSnap4"].Contains(((object)(Buttons)4/*cast due to constrained. prefix*/).ToString()))
			{
				PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepad].KeyStatus["DpadSnap1"].Clear();
				PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepad].KeyStatus["DpadSnap2"].Clear();
				PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepad].KeyStatus["DpadSnap3"].Clear();
				PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepad].KeyStatus["DpadSnap4"].Clear();
				PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepadUI].KeyStatus["DpadSnap1"].Clear();
				PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepadUI].KeyStatus["DpadSnap2"].Clear();
				PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepadUI].KeyStatus["DpadSnap3"].Clear();
				PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepadUI].KeyStatus["DpadSnap4"].Clear();
				return;
			}
			PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepad].KeyStatus["DpadRadial1"].Clear();
			PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepad].KeyStatus["DpadRadial2"].Clear();
			PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepad].KeyStatus["DpadRadial3"].Clear();
			PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepad].KeyStatus["DpadRadial4"].Clear();
			PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepadUI].KeyStatus["DpadRadial1"].Clear();
			PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepadUI].KeyStatus["DpadRadial2"].Clear();
			PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepadUI].KeyStatus["DpadRadial3"].Clear();
			PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepadUI].KeyStatus["DpadRadial4"].Clear();
			PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepad].KeyStatus["DpadSnap1"] = new List<string> { ((object)(Buttons)1/*cast due to constrained. prefix*/).ToString() };
			PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepad].KeyStatus["DpadSnap2"] = new List<string> { ((object)(Buttons)8/*cast due to constrained. prefix*/).ToString() };
			PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepad].KeyStatus["DpadSnap3"] = new List<string> { ((object)(Buttons)2/*cast due to constrained. prefix*/).ToString() };
			PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepad].KeyStatus["DpadSnap4"] = new List<string> { ((object)(Buttons)4/*cast due to constrained. prefix*/).ToString() };
			PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepadUI].KeyStatus["DpadSnap1"] = new List<string> { ((object)(Buttons)1/*cast due to constrained. prefix*/).ToString() };
			PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepadUI].KeyStatus["DpadSnap2"] = new List<string> { ((object)(Buttons)8/*cast due to constrained. prefix*/).ToString() };
			PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepadUI].KeyStatus["DpadSnap3"] = new List<string> { ((object)(Buttons)2/*cast due to constrained. prefix*/).ToString() };
			PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepadUI].KeyStatus["DpadSnap4"] = new List<string> { ((object)(Buttons)4/*cast due to constrained. prefix*/).ToString() };
		}
	}

	private void RadialButtonClick(UIMouseEvent evt, UIElement listeningElement)
	{
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0446: Unknown result type (might be due to invalid IL or missing references)
		//IL_047f: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_052a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0563: Unknown result type (might be due to invalid IL or missing references)
		//IL_059c: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_0124: Unknown result type (might be due to invalid IL or missing references)
		//IL_015c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0194: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cc: Unknown result type (might be due to invalid IL or missing references)
		if (PlayerInput.CurrentProfile.AllowEditing)
		{
			SoundEngine.PlaySound(12);
			if (PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepad].KeyStatus["DpadRadial1"].Contains(((object)(Buttons)1/*cast due to constrained. prefix*/).ToString()) && PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepad].KeyStatus["DpadRadial2"].Contains(((object)(Buttons)8/*cast due to constrained. prefix*/).ToString()) && PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepad].KeyStatus["DpadRadial3"].Contains(((object)(Buttons)2/*cast due to constrained. prefix*/).ToString()) && PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepad].KeyStatus["DpadRadial4"].Contains(((object)(Buttons)4/*cast due to constrained. prefix*/).ToString()) && PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepadUI].KeyStatus["DpadRadial1"].Contains(((object)(Buttons)1/*cast due to constrained. prefix*/).ToString()) && PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepadUI].KeyStatus["DpadRadial2"].Contains(((object)(Buttons)8/*cast due to constrained. prefix*/).ToString()) && PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepadUI].KeyStatus["DpadRadial3"].Contains(((object)(Buttons)2/*cast due to constrained. prefix*/).ToString()) && PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepadUI].KeyStatus["DpadRadial4"].Contains(((object)(Buttons)4/*cast due to constrained. prefix*/).ToString()))
			{
				PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepad].KeyStatus["DpadRadial1"].Clear();
				PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepad].KeyStatus["DpadRadial2"].Clear();
				PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepad].KeyStatus["DpadRadial3"].Clear();
				PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepad].KeyStatus["DpadRadial4"].Clear();
				PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepadUI].KeyStatus["DpadRadial1"].Clear();
				PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepadUI].KeyStatus["DpadRadial2"].Clear();
				PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepadUI].KeyStatus["DpadRadial3"].Clear();
				PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepadUI].KeyStatus["DpadRadial4"].Clear();
				return;
			}
			PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepad].KeyStatus["DpadSnap1"].Clear();
			PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepad].KeyStatus["DpadSnap2"].Clear();
			PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepad].KeyStatus["DpadSnap3"].Clear();
			PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepad].KeyStatus["DpadSnap4"].Clear();
			PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepadUI].KeyStatus["DpadSnap1"].Clear();
			PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepadUI].KeyStatus["DpadSnap2"].Clear();
			PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepadUI].KeyStatus["DpadSnap3"].Clear();
			PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepadUI].KeyStatus["DpadSnap4"].Clear();
			PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepad].KeyStatus["DpadRadial1"] = new List<string> { ((object)(Buttons)1/*cast due to constrained. prefix*/).ToString() };
			PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepad].KeyStatus["DpadRadial2"] = new List<string> { ((object)(Buttons)8/*cast due to constrained. prefix*/).ToString() };
			PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepad].KeyStatus["DpadRadial3"] = new List<string> { ((object)(Buttons)2/*cast due to constrained. prefix*/).ToString() };
			PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepad].KeyStatus["DpadRadial4"] = new List<string> { ((object)(Buttons)4/*cast due to constrained. prefix*/).ToString() };
			PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepadUI].KeyStatus["DpadRadial1"] = new List<string> { ((object)(Buttons)1/*cast due to constrained. prefix*/).ToString() };
			PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepadUI].KeyStatus["DpadRadial2"] = new List<string> { ((object)(Buttons)8/*cast due to constrained. prefix*/).ToString() };
			PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepadUI].KeyStatus["DpadRadial3"] = new List<string> { ((object)(Buttons)2/*cast due to constrained. prefix*/).ToString() };
			PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepadUI].KeyStatus["DpadRadial4"] = new List<string> { ((object)(Buttons)4/*cast due to constrained. prefix*/).ToString() };
		}
	}

	private void KeyboardButtonClick(UIMouseEvent evt, UIElement listeningElement)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		_buttonKeyboard.SetFrame(_KeyboardGamepadTexture.Frame(2, 2));
		_buttonGamepad.SetFrame(_KeyboardGamepadTexture.Frame(2, 2, 1, 1));
		OnKeyboard = true;
		FillList();
	}

	private void GamepadButtonClick(UIMouseEvent evt, UIElement listeningElement)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		_buttonKeyboard.SetFrame(_KeyboardGamepadTexture.Frame(2, 2, 0, 1));
		_buttonGamepad.SetFrame(_KeyboardGamepadTexture.Frame(2, 2, 1));
		OnKeyboard = false;
		FillList();
	}

	private void ManageBorderKeyboardOn(UIMouseEvent evt, UIElement listeningElement)
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		_buttonBorder2.Color = ((!OnKeyboard) ? Color.Silver : Color.Black);
		_buttonBorder1.Color = Main.OurFavoriteColor;
	}

	private void ManageBorderKeyboardOff(UIMouseEvent evt, UIElement listeningElement)
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		_buttonBorder2.Color = ((!OnKeyboard) ? Color.Silver : Color.Black);
		_buttonBorder1.Color = (OnKeyboard ? Color.Silver : Color.Black);
	}

	private void ManageBorderGamepadOn(UIMouseEvent evt, UIElement listeningElement)
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		_buttonBorder1.Color = (OnKeyboard ? Color.Silver : Color.Black);
		_buttonBorder2.Color = Main.OurFavoriteColor;
	}

	private void ManageBorderGamepadOff(UIMouseEvent evt, UIElement listeningElement)
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		_buttonBorder1.Color = (OnKeyboard ? Color.Silver : Color.Black);
		_buttonBorder2.Color = ((!OnKeyboard) ? Color.Silver : Color.Black);
	}

	private void VsGameplayButtonClick(UIMouseEvent evt, UIElement listeningElement)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		_buttonVs1.SetFrame(_GameplayVsUITexture.Frame(2, 2));
		_buttonVs2.SetFrame(_GameplayVsUITexture.Frame(2, 2, 1, 1));
		OnGameplay = true;
		FillList();
	}

	private void VsMenuButtonClick(UIMouseEvent evt, UIElement listeningElement)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		_buttonVs1.SetFrame(_GameplayVsUITexture.Frame(2, 2, 0, 1));
		_buttonVs2.SetFrame(_GameplayVsUITexture.Frame(2, 2, 1));
		OnGameplay = false;
		FillList();
	}

	private void ManageBorderGameplayOn(UIMouseEvent evt, UIElement listeningElement)
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		_buttonBorderVs2.Color = ((!OnGameplay) ? Color.Silver : Color.Black);
		_buttonBorderVs1.Color = Main.OurFavoriteColor;
	}

	private void ManageBorderGameplayOff(UIMouseEvent evt, UIElement listeningElement)
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		_buttonBorderVs2.Color = ((!OnGameplay) ? Color.Silver : Color.Black);
		_buttonBorderVs1.Color = (OnGameplay ? Color.Silver : Color.Black);
	}

	private void ManageBorderMenuOn(UIMouseEvent evt, UIElement listeningElement)
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		_buttonBorderVs1.Color = (OnGameplay ? Color.Silver : Color.Black);
		_buttonBorderVs2.Color = Main.OurFavoriteColor;
	}

	private void ManageBorderMenuOff(UIMouseEvent evt, UIElement listeningElement)
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		_buttonBorderVs1.Color = (OnGameplay ? Color.Silver : Color.Black);
		_buttonBorderVs2.Color = ((!OnGameplay) ? Color.Silver : Color.Black);
	}

	private string controllerGlyphStyle()
	{
		return GlyphTagHandler.GlyphStyle switch
		{
			-1 => Language.GetText("UI.ControllerGlyphAuto").Value, 
			1 => Language.GetText("UI.ControllerGlyphPlayStation").Value, 
			2 => Language.GetText("UI.ControllerGlyphSwitch").Value, 
			_ => Language.GetText("UI.ControllerGlyphXbox").Value, 
		};
	}

	private void controllerGlyphButtonClick(UIMouseEvent evt, UIElement listeningElement)
	{
		GlyphTagHandler.GlyphStyle++;
		if (GlyphTagHandler.GlyphStyle > 2)
		{
			GlyphTagHandler.GlyphStyle = -1;
		}
	}

	private void profileButtonClick(UIMouseEvent evt, UIElement listeningElement)
	{
		string name = PlayerInput.CurrentProfile.Name;
		List<string> list = PlayerInput.Profiles.Keys.ToList();
		int num = list.IndexOf(name);
		num++;
		if (num >= list.Count)
		{
			num -= list.Count;
		}
		PlayerInput.SetSelectedProfile(list[num]);
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
		((UIPanel)evt.Target).BackgroundColor = new Color(63, 82, 151) * 0.7f;
		((UIPanel)evt.Target).BorderColor = Color.Black;
	}

	private void GoBackClick(UIMouseEvent evt, UIElement listeningElement)
	{
		Main.menuMode = 1127;
		IngameFancyUI.Close();
	}

	public override void Draw(SpriteBatch spriteBatch)
	{
		base.Draw(spriteBatch);
		SetupGamepadPoints(spriteBatch);
	}

	private void SetupGamepadPoints(SpriteBatch spriteBatch)
	{
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0101: Unknown result type (might be due to invalid IL or missing references)
		//IL_0105: Unknown result type (might be due to invalid IL or missing references)
		//IL_010a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0128: Unknown result type (might be due to invalid IL or missing references)
		//IL_012d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0131: Unknown result type (might be due to invalid IL or missing references)
		//IL_0136: Unknown result type (might be due to invalid IL or missing references)
		//IL_0296: Unknown result type (might be due to invalid IL or missing references)
		//IL_029b: Unknown result type (might be due to invalid IL or missing references)
		//IL_029c: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0383: Unknown result type (might be due to invalid IL or missing references)
		//IL_0431: Unknown result type (might be due to invalid IL or missing references)
		UILinkPointNavigator.Shortcuts.BackButtonCommand = 4;
		int num = 3000;
		Rectangle val = _buttonBack.GetInnerDimensions().ToRectangle();
		UILinkPointNavigator.SetPosition(3000, val.Center.ToVector2());
		val = _buttonKeyboard.GetInnerDimensions().ToRectangle();
		UILinkPointNavigator.SetPosition(3001, val.Center.ToVector2());
		val = _buttonGamepad.GetInnerDimensions().ToRectangle();
		UILinkPointNavigator.SetPosition(3002, val.Center.ToVector2());
		val = _buttonProfile.GetInnerDimensions().ToRectangle();
		UILinkPointNavigator.SetPosition(3003, val.Center.ToVector2());
		val = _buttonVs1.GetInnerDimensions().ToRectangle();
		UILinkPointNavigator.SetPosition(3004, val.Center.ToVector2());
		val = _buttonVs2.GetInnerDimensions().ToRectangle();
		UILinkPointNavigator.SetPosition(3005, val.Center.ToVector2());
		val = _buttonStyle.GetInnerDimensions().ToRectangle();
		UILinkPointNavigator.SetPosition(3006, val.Center.ToVector2());
		int key = num;
		UILinkPoint uILinkPoint = UILinkPointNavigator.Points[key];
		uILinkPoint.Unlink();
		uILinkPoint.Up = num + 7;
		key = 3001;
		UILinkPoint uILinkPoint2 = UILinkPointNavigator.Points[key];
		uILinkPoint2.Unlink();
		uILinkPoint2.Right = 3002;
		uILinkPoint2.Down = num + 7;
		key = 3002;
		UILinkPoint uILinkPoint3 = UILinkPointNavigator.Points[key];
		uILinkPoint3.Unlink();
		uILinkPoint3.Left = 3001;
		uILinkPoint3.Right = 3004;
		uILinkPoint3.Down = num + 7;
		key = 3004;
		UILinkPoint uILinkPoint4 = UILinkPointNavigator.Points[key];
		uILinkPoint4.Unlink();
		uILinkPoint4.Left = 3002;
		uILinkPoint4.Right = 3005;
		uILinkPoint4.Down = num + 7;
		key = 3005;
		UILinkPoint uILinkPoint5 = UILinkPointNavigator.Points[key];
		uILinkPoint5.Unlink();
		uILinkPoint5.Left = 3004;
		uILinkPoint5.Right = 3006;
		uILinkPoint5.Down = num + 7;
		key = 3006;
		UILinkPoint uILinkPoint6 = UILinkPointNavigator.Points[key];
		uILinkPoint6.Unlink();
		uILinkPoint6.Left = 3005;
		uILinkPoint6.Right = 3003;
		uILinkPoint6.Down = num + 7;
		key = 3003;
		UILinkPoint uILinkPoint7 = UILinkPointNavigator.Points[key];
		uILinkPoint7.Unlink();
		uILinkPoint7.Left = 3006;
		uILinkPoint7.Down = num + 7;
		float num2 = 1f / Main.UIScale;
		Rectangle clippingRectangle = _uilist.GetClippingRectangle(spriteBatch);
		Vector2 minimum = clippingRectangle.TopLeft() * num2;
		Vector2 maximum = clippingRectangle.BottomRight() * num2;
		List<SnapPoint> snapPoints = _uilist.GetSnapPoints();
		for (int i = 0; i < snapPoints.Count; i++)
		{
			if (!snapPoints[i].Position.Between(minimum, maximum))
			{
				_ = snapPoints[i].Position;
				snapPoints.Remove(snapPoints[i]);
				i--;
			}
		}
		snapPoints.Sort((SnapPoint x, SnapPoint y) => x.Id.CompareTo(y.Id));
		for (int num3 = 0; num3 < snapPoints.Count; num3++)
		{
			key = num + 7 + num3;
			if (snapPoints[num3].Name == "Thin")
			{
				UILinkPoint uILinkPoint8 = UILinkPointNavigator.Points[key];
				uILinkPoint8.Unlink();
				UILinkPointNavigator.SetPosition(key, snapPoints[num3].Position);
				uILinkPoint8.Right = key + 1;
				uILinkPoint8.Down = ((num3 < snapPoints.Count - 2) ? (key + 2) : num);
				ref int up = ref uILinkPoint8.Up;
				int num4;
				if (num3 < 2)
				{
					num4 = 3001;
				}
				else
				{
					num4 = ((snapPoints[num3 - 1].Name == "Wide") ? (key - 1) : (key - 2));
				}
				up = num4;
				UILinkPointNavigator.Points[num].Up = key;
				UILinkPointNavigator.Shortcuts.FANCYUI_HIGHEST_INDEX = key;
				num3++;
				if (num3 < snapPoints.Count)
				{
					key = num + 7 + num3;
					UILinkPoint uILinkPoint9 = UILinkPointNavigator.Points[key];
					uILinkPoint9.Unlink();
					UILinkPointNavigator.SetPosition(key, snapPoints[num3].Position);
					uILinkPoint9.Left = key - 1;
					ref int down = ref uILinkPoint9.Down;
					int num5;
					if (num3 >= snapPoints.Count - 1)
					{
						num5 = num;
					}
					else
					{
						num5 = ((snapPoints[num3 + 1].Name == "Wide") ? (key + 1) : (key + 2));
					}
					down = num5;
					uILinkPoint9.Up = ((num3 < 2) ? 3001 : (key - 2));
					UILinkPointNavigator.Shortcuts.FANCYUI_HIGHEST_INDEX = key;
				}
			}
			else
			{
				UILinkPoint uILinkPoint10 = UILinkPointNavigator.Points[key];
				uILinkPoint10.Unlink();
				UILinkPointNavigator.SetPosition(key, snapPoints[num3].Position);
				uILinkPoint10.Down = ((num3 < snapPoints.Count - 1) ? (key + 1) : num);
				ref int up2 = ref uILinkPoint10.Up;
				int num6;
				if (num3 < 1)
				{
					num6 = 3001;
				}
				else
				{
					num6 = ((snapPoints[num3 - 1].Name == "Wide") ? (key - 1) : (key - 2));
				}
				up2 = num6;
				UILinkPointNavigator.Shortcuts.FANCYUI_HIGHEST_INDEX = key;
				UILinkPointNavigator.Points[num].Up = key;
			}
		}
		if (ForceMoveTo != -1)
		{
			UILinkPointNavigator.ChangePoint((int)MathHelper.Clamp((float)ForceMoveTo, (float)num, (float)UILinkPointNavigator.Shortcuts.FANCYUI_HIGHEST_INDEX));
			ForceMoveTo = -1;
		}
	}
}
