using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Terraria.Audio;
using Terraria.GameContent.UI;
using Terraria.GameContent.UI.Chat;
using Terraria.GameContent.UI.States;
using Terraria.ID;
using Terraria.Localization;
using Terraria.Social;
using Terraria.Testing;
using Terraria.Testing.Cloning;
using Terraria.UI;
using Terraria.UI.Gamepad;

namespace Terraria.GameInput;

public class PlayerInput
{
	public class MiscSettingsTEMP
	{
		public static bool HotbarRadialShouldBeUsed = true;
	}

	public static class SettingsForUI
	{
		public static CursorMode CurrentCursorMode { get; private set; }

		public static bool ShowGamepadHints
		{
			get
			{
				if (!UsingGamepad)
				{
					return SteamDeckIsUsed;
				}
				return true;
			}
		}

		public static bool AllowSecondaryGamepadAim
		{
			get
			{
				if (CurrentCursorMode != CursorMode.Gamepad)
				{
					return !SteamDeckIsUsed;
				}
				return true;
			}
		}

		public static bool PushEquipmentAreaUp
		{
			get
			{
				if (UsingGamepad && !SteamDeckIsUsed)
				{
					if (!Main.showFrameRate)
					{
						return !Main.GamepadDisableInstructionsDisplay;
					}
					return true;
				}
				return false;
			}
		}

		public static bool ShowGamepadCursor
		{
			get
			{
				if (ControllerHousingCursorActive)
				{
					return true;
				}
				if (!SteamDeckIsUsed)
				{
					return UsingGamepad;
				}
				return CurrentCursorMode == CursorMode.Gamepad;
			}
		}

		public static bool HighlightThingsForMouse
		{
			get
			{
				if (UsingGamepadUI)
				{
					return CurrentCursorMode == CursorMode.Mouse;
				}
				return true;
			}
		}

		public static int FramesSinceLastTimeInMouseMode { get; private set; }

		public static bool PreventHighlightsForGamepad => FramesSinceLastTimeInMouseMode == 0;

		public static void SetCursorMode(CursorMode cursorMode)
		{
			if (!DebugOptions.ForceGamepad || cursorMode != CursorMode.Mouse)
			{
				CurrentCursorMode = cursorMode;
				if (CurrentCursorMode == CursorMode.Mouse)
				{
					FramesSinceLastTimeInMouseMode = 0;
				}
			}
		}

		public static void UpdateCounters()
		{
			if (CurrentCursorMode != CursorMode.Mouse)
			{
				FramesSinceLastTimeInMouseMode++;
			}
		}

		public static void TryRevertingToMouseMode()
		{
			if (FramesSinceLastTimeInMouseMode <= 0)
			{
				SetCursorMode(CursorMode.Mouse);
				CurrentInputMode = InputMode.Mouse;
				Triggers.Current.UsedMovementKey = false;
				NavigatorUnCachePosition();
			}
		}

		internal static void AddInputSnapshotComponents()
		{
			StateSnapshot.Input.AddVal("PlayerInput.SettingsForUI.CurrentCursorMode", () => CurrentCursorMode, (CursorMode v) =>
			{
				CurrentCursorMode = v;
			});
			StateSnapshot.Input.AddVal("PlayerInput.SettingsForUI.FramesSinceLastTimeInMouseMode", () => FramesSinceLastTimeInMouseMode, (int v) =>
			{
				FramesSinceLastTimeInMouseMode = v;
			});
		}
	}

	private struct FastUseItemMemory
	{
		private int _slot;

		private int _itemType;

		private bool _shouldFastUse;

		private bool _isMouseItem;

		[CloneByReference]
		private Player _player;

		public bool TryStartForItemSlot(Player player, int itemSlot)
		{
			if (player.UsingOrReusingItem)
			{
				return false;
			}
			if (itemSlot < 0 || itemSlot >= 50)
			{
				Clear();
				return false;
			}
			_player = player;
			_slot = itemSlot;
			_itemType = _player.inventory[itemSlot].type;
			_shouldFastUse = true;
			_isMouseItem = false;
			ItemSlot.PickupItemIntoMouse(player.inventory, 0, itemSlot, player);
			return true;
		}

		public bool TryStartForMouse(Player player)
		{
			if (player.UsingOrReusingItem)
			{
				return false;
			}
			_player = player;
			_slot = -1;
			_itemType = Main.mouseItem.type;
			_shouldFastUse = true;
			_isMouseItem = true;
			return true;
		}

		public void Clear()
		{
			_shouldFastUse = false;
		}

		public bool CanFastUse()
		{
			if (!_shouldFastUse)
			{
				return false;
			}
			if (_isMouseItem)
			{
				return Main.mouseItem.type == _itemType;
			}
			return _player.inventory[_slot].type == _itemType;
		}

		public void EndFastUse()
		{
			if (_shouldFastUse)
			{
				if (!_isMouseItem && _player.inventory[_slot].IsAir)
				{
					Utils.Swap(ref Main.mouseItem, ref _player.inventory[_slot]);
				}
				Clear();
			}
		}
	}

	public static Vector2 RawMouseScale = Vector2.One;

	public static TriggersPack Triggers = new TriggersPack();

	public static List<string> KnownTriggers = new List<string>
	{
		"MouseLeft", "MouseRight", "Up", "Down", "Left", "Right", "Jump", "Throw", "Inventory", "Grapple",
		"SmartSelect", "SmartCursor", "QuickMount", "QuickHeal", "QuickMana", "QuickBuff", "MapZoomIn", "MapZoomOut", "MapAlphaUp", "MapAlphaDown",
		"MapFull", "MapStyle", "Hotbar1", "Hotbar2", "Hotbar3", "Hotbar4", "Hotbar5", "Hotbar6", "Hotbar7", "Hotbar8",
		"Hotbar9", "Hotbar10", "HotbarMinus", "HotbarPlus", "DpadRadial1", "DpadRadial2", "DpadRadial3", "DpadRadial4", "RadialHotbar", "RadialQuickbar",
		"DpadSnap1", "DpadSnap2", "DpadSnap3", "DpadSnap4", "MenuUp", "MenuDown", "MenuLeft", "MenuRight", "LockOn", "ViewZoomIn",
		"ViewZoomOut", "Loadout1", "Loadout2", "Loadout3", "PreviousLoadout", "NextLoadout", "ToggleCameraMode", "ToggleCreativeMenu", "ArmorSetAbility", "Dash"
	};

	private static bool _canReleaseRebindingLock = true;

	private static int _memoOfLastPoint = -1;

	public static int NavigatorRebindingLock;

	public static string BlockedKey = "";

	private static string _listeningTrigger;

	private static InputMode _listeningInputMode;

	public static Dictionary<string, PlayerInputProfile> Profiles = new Dictionary<string, PlayerInputProfile>();

	public static Dictionary<string, PlayerInputProfile> OriginalProfiles = new Dictionary<string, PlayerInputProfile>();

	private static string _selectedProfile;

	private static PlayerInputProfile _currentProfile;

	private static InputMode _inputMode = InputMode.Keyboard;

	private static Buttons[] ButtonsGamepad = (Buttons[])Enum.GetValues(typeof(Buttons));

	public static bool GrappleAndInteractAreShared;

	public static SmartSelectGamepadPointer smartSelectPointer = new SmartSelectGamepadPointer();

	public static bool UseSteamDeckIfPossible;

	private static string _invalidatorCheck = "";

	private static bool _lastActivityState;

	public static MouseState MouseInfo;

	public static MouseState MouseInfoOld;

	public static int MouseX;

	public static int MouseY;

	public static bool LockGamepadTileUseButton = false;

	public static List<string> MouseKeys = new List<string>();

	public static int PreUIX;

	public static int PreUIY;

	public static int PreLockOnX;

	public static int PreLockOnY;

	public static int ScrollWheelValue;

	public static int ScrollWheelValueOld;

	public static int ScrollWheelDelta;

	public static int ScrollWheelDeltaForUI;

	public static bool GamepadAllowScrolling;

	public static int GamepadScrollValue;

	public static Vector2 GamepadThumbstickLeft = Vector2.Zero;

	public static Vector2 GamepadThumbstickRight = Vector2.Zero;

	private static FastUseItemMemory _fastUseMemory;

	private static bool _InBuildingMode;

	private static int _UIPointForBuildingMode = -1;

	public static bool WritingText;

	private static int _originalMouseX;

	private static int _originalMouseY;

	private static int _originalLastMouseX;

	private static int _originalLastMouseY;

	private static int _originalScreenWidth;

	private static int _originalScreenHeight;

	private static List<string> _buttonsLocked = new List<string>();

	public static Vector2 HousingMouseOffset;

	public static bool PreventCursorModeSwappingToGamepad = false;

	public static bool PreventFirstMousePositionGrab = false;

	private static int[] DpadSnapCooldown = new int[4];

	public static bool AllowExecutionOfGamepadInstructions = true;

	private static readonly List<string> _keysToLocalize = new List<string>();

	public static string ListeningTrigger => _listeningTrigger;

	public static bool CurrentlyRebinding => _listeningTrigger != null;

	public static bool InvisibleGamepadInMenus
	{
		get
		{
			if (ControllerHousingCursorActive)
			{
				return false;
			}
			if ((!Main.gameMenu && !Main.ingameOptionsWindow && !Main.playerInventory && Main.player[Main.myPlayer].talkNPC == -1 && Main.player[Main.myPlayer].sign == -1 && Main.InGameUI.CurrentState == null) || _InBuildingMode || !Main.InvisibleCursorForGamepad)
			{
				if (CursorIsBusy)
				{
					return !_InBuildingMode;
				}
				return false;
			}
			return true;
		}
	}

	public static PlayerInputProfile CurrentProfile => _currentProfile;

	public static KeyConfiguration ProfileGamepadUI => CurrentProfile.InputModes[InputMode.XBoxGamepadUI];

	public static InputMode CurrentInputMode
	{
		get
		{
			return _inputMode;
		}
		set
		{
			bool usingGamepad = UsingGamepad;
			if (!(DebugOptions.ForceGamepad & usingGamepad) || value == InputMode.XBoxGamepad || value == InputMode.XBoxGamepadUI)
			{
				_inputMode = value;
				if (UsingGamepad != usingGamepad && OnBindingChange != null)
				{
					OnBindingChange();
				}
			}
		}
	}

	public static bool UsingGamepad
	{
		get
		{
			if (CurrentInputMode != InputMode.XBoxGamepad)
			{
				return CurrentInputMode == InputMode.XBoxGamepadUI;
			}
			return true;
		}
	}

	public static bool UsingGamepadUI => CurrentInputMode == InputMode.XBoxGamepadUI;

	public static bool IgnoreMouseInterface
	{
		get
		{
			if (Main.LocalPlayer.UsingOrReusingItem && !UsingGamepad && !Main.gamePaused)
			{
				return true;
			}
			bool flag = UsingGamepad && !UILinkPointNavigator.Available;
			if (flag && SteamDeckIsUsed && SettingsForUI.CurrentCursorMode == CursorMode.Mouse && !Main.mouseRight)
			{
				return false;
			}
			return flag;
		}
	}

	public static bool SteamDeckIsUsed => UseSteamDeckIfPossible;

	public static bool ShouldFastUseItem => _fastUseMemory.CanFastUse();

	public static bool InBuildingMode => _InBuildingMode;

	public static int RealScreenWidth => _originalScreenWidth;

	public static int RealScreenHeight => _originalScreenHeight;

	public static int UIScreenWidth => (int)((float)_originalScreenWidth / Main.UIScale);

	public static int UIScreenHeight => (int)((float)_originalScreenHeight / Main.UIScale);

	public static bool CursorIsBusy
	{
		get
		{
			if (!(ItemSlot.CircularRadialOpacity > 0f))
			{
				return ItemSlot.QuicksRadialOpacity > 0f;
			}
			return true;
		}
	}

	public static Vector2 OriginalScreenSize
	{
		get
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			return new Vector2((float)_originalScreenWidth, (float)_originalScreenHeight);
		}
	}

	public static bool ControllerHousingCursorActive
	{
		get
		{
			if (CurrentInputMode == InputMode.XBoxGamepadUI && UILinkPointNavigator.CurrentPoint >= 600)
			{
				return UILinkPointNavigator.CurrentPoint < 650;
			}
			return false;
		}
	}

	public static Vector2 HousingWorldPosition
	{
		get
		{
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			return Main.LocalPlayer.Center + HousingMouseOffset;
		}
		set
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			HousingMouseOffset = value - Main.LocalPlayer.Center;
		}
	}

	public static Vector2 HousingScreenPosition
	{
		get
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			return HousingWorldPosition.ToScreenPosition();
		}
	}

	public static event Action OnBindingChange;

	public static event Action OnActionableInput;

	public static void ListenFor(string triggerName, InputMode inputmode)
	{
		_listeningTrigger = triggerName;
		_listeningInputMode = inputmode;
	}

	private static bool InvalidateKeyboardSwap()
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		if (_invalidatorCheck.Length == 0)
		{
			return false;
		}
		string text = "";
		List<Keys> pressedKeys = GetPressedKeys();
		for (int i = 0; i < pressedKeys.Count; i++)
		{
			text = string.Concat(text, pressedKeys[i], ", ");
		}
		if (text == _invalidatorCheck)
		{
			return true;
		}
		_invalidatorCheck = "";
		return false;
	}

	public static void ResetInputsOnActiveStateChange()
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		bool allowInputProcessing = FocusHelper.AllowInputProcessing;
		if (_lastActivityState != allowInputProcessing)
		{
			MouseInfo = default;
			MouseInfoOld = default;
			Main.keyState = Keyboard.GetState();
			Main.inputText = Keyboard.GetState();
			Main.oldInputText = Keyboard.GetState();
			Main.keyCount = 0;
			Triggers.Reset();
			Triggers.Reset();
			string text = "";
			List<Keys> pressedKeys = GetPressedKeys();
			for (int i = 0; i < pressedKeys.Count; i++)
			{
				text = string.Concat(text, pressedKeys[i], ", ");
			}
			_invalidatorCheck = text;
		}
		_lastActivityState = allowInputProcessing;
	}

	public static List<Keys> GetPressedKeys()
	{
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Invalid comparison between Unknown and I4
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		List<Keys> list = Main.keyState.GetPressedKeys().ToList();
		for (int num = list.Count - 1; num >= 0; num--)
		{
			Keys val = list[num];
			if ((int)val == 0 || (int)val == 25 || IsImeStateKeycode(val))
			{
				list.RemoveAt(num);
			}
		}
		return list;
	}

	private static bool IsImeStateKeycode(Keys k)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Invalid comparison between Unknown and I4
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Invalid comparison between Unknown and I4
		if ((int)k >= 240)
		{
			return (int)k <= 253;
		}
		return false;
	}

	internal static void AddInputSnapshotComponents()
	{
		StateSnapshot.Input.AddRef("PlayerInput.Triggers", () => Triggers, (TriggersPack v) =>
		{
			Triggers = v;
		});
		StateSnapshot.Input.AddVal("PlayerInput.MouseX", () => MouseX, (int v) =>
		{
			MouseX = v;
		});
		StateSnapshot.Input.AddVal("PlayerInput.MouseY", () => MouseY, (int v) =>
		{
			MouseY = v;
		});
		StateSnapshot.Input.AddVal("PlayerInput.ScrollWheelDelta", () => ScrollWheelDelta, (int v) =>
		{
			ScrollWheelDelta = v;
		});
		StateSnapshot.Input.AddVal("PlayerInput.ScrollWheelDeltaForUI", () => ScrollWheelDeltaForUI, (int v) =>
		{
			ScrollWheelDeltaForUI = v;
		});
		StateSnapshot.Input.AddVal<Vector2>("PlayerInput.GamepadThumbstickLeft", (Func<Vector2>)(() =>
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			return GamepadThumbstickLeft;
		}), (Action<Vector2>)((Vector2 v) =>
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			GamepadThumbstickLeft = v;
		}));
		StateSnapshot.Input.AddVal<Vector2>("PlayerInput.GamepadThumbstickRight", (Func<Vector2>)(() =>
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			return GamepadThumbstickRight;
		}), (Action<Vector2>)((Vector2 v) =>
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			GamepadThumbstickRight = v;
		}));
		StateSnapshot.Input.AddVal("PlayerInput.GrappleAndInteractAreShared", () => GrappleAndInteractAreShared, (bool v) =>
		{
			GrappleAndInteractAreShared = v;
		});
		StateSnapshot.Input.AddVal("PlayerInput.WritingText", () => WritingText, (bool v) =>
		{
			WritingText = v;
		});
		StateSnapshot.Input.AddRef("PlayerInput.MouseKeys", () => MouseKeys, (List<string> v) =>
		{
			MouseKeys = v;
		});
		StateSnapshot.Input.AddRef("PlayerInput._buttonsLocked", () => _buttonsLocked, (List<string> v) =>
		{
			_buttonsLocked = v;
		});
		StateSnapshot.Input.AddVal("PlayerInput._inputMode", () => _inputMode, (InputMode v) =>
		{
			_inputMode = v;
		});
		StateSnapshot.Input.AddVal("PlayerInput._InBuildingMode", () => _InBuildingMode, (bool v) =>
		{
			_InBuildingMode = v;
		});
		StateSnapshot.Input.AddVal("PlayerInput._UIPointForBuildingMode", () => _UIPointForBuildingMode, (int v) =>
		{
			_UIPointForBuildingMode = v;
		});
		StateSnapshot.Input.AddVal("PlayerInput.LockGamepadTileUseButton", () => LockGamepadTileUseButton, (bool v) =>
		{
			LockGamepadTileUseButton = v;
		});
		StateSnapshot.Input.AddRef("PlayerInput.DpadSnapCooldown", () => DpadSnapCooldown, (int[] v) =>
		{
			DpadSnapCooldown = v;
		});
		StateSnapshot.Input.AddRef("PlayerInput.smartSelectPointer", () => smartSelectPointer, (SmartSelectGamepadPointer v) =>
		{
			smartSelectPointer = v;
		});
		StateSnapshot.Input.AddVal<Vector2>("PlayerInput.HousingMouseOffset", (Func<Vector2>)(() =>
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			return HousingMouseOffset;
		}), (Action<Vector2>)((Vector2 v) =>
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			HousingMouseOffset = v;
		}));
		StateSnapshot.Input.AddVal("PlayerInput._fastUseMemory", () => _fastUseMemory, (FastUseItemMemory v) =>
		{
			_fastUseMemory = v;
		});
		StateSnapshot.Input.AddVal("PlayerInput.MiscSettingsTEMP.HotbarRadialShouldBeUsed", () => MiscSettingsTEMP.HotbarRadialShouldBeUsed, (bool v) =>
		{
			MiscSettingsTEMP.HotbarRadialShouldBeUsed = v;
		});
		SettingsForUI.AddInputSnapshotComponents();
	}

	public static void TryEnteringFastUseModeForInventorySlot(int inventorySlot)
	{
		_fastUseMemory.TryStartForItemSlot(Main.LocalPlayer, inventorySlot);
	}

	public static void TryEnteringFastUseModeForMouseItem()
	{
		_fastUseMemory.TryStartForMouse(Main.LocalPlayer);
	}

	public static void TryEndingFastUse()
	{
		_fastUseMemory.EndFastUse();
	}

	public static void EnterBuildingMode()
	{
		if (Main.LocalPlayer.UsingOrReusingItem)
		{
			return;
		}
		SoundEngine.PlaySound(10);
		_InBuildingMode = true;
		_UIPointForBuildingMode = UILinkPointNavigator.CurrentPoint;
		if (Main.mouseItem.IsAir)
		{
			int uIPointForBuildingMode = _UIPointForBuildingMode;
			if (uIPointForBuildingMode < 50 && uIPointForBuildingMode >= 0 && Main.player[Main.myPlayer].inventory[uIPointForBuildingMode].stack > 0)
			{
				Utils.Swap(ref Main.mouseItem, ref Main.player[Main.myPlayer].inventory[uIPointForBuildingMode]);
			}
		}
	}

	public static void ExitBuildingMode(bool quiet = false)
	{
		if (!quiet)
		{
			SoundEngine.PlaySound(11);
		}
		_InBuildingMode = false;
		if (UsingGamepad)
		{
			Main.LocalPlayer.mouseInterface = true;
		}
		UILinkPointNavigator.ChangePoint(_UIPointForBuildingMode);
		if (Main.mouseItem.stack > 0 && Main.player[Main.myPlayer].itemAnimation == 0)
		{
			int uIPointForBuildingMode = _UIPointForBuildingMode;
			if (uIPointForBuildingMode < 50 && uIPointForBuildingMode >= 0 && Main.player[Main.myPlayer].inventory[uIPointForBuildingMode].stack <= 0)
			{
				Utils.Swap(ref Main.mouseItem, ref Main.player[Main.myPlayer].inventory[uIPointForBuildingMode]);
			}
		}
		_UIPointForBuildingMode = -1;
	}

	public static void VerifyBuildingMode()
	{
		if (_InBuildingMode)
		{
			Player obj = Main.player[Main.myPlayer];
			bool flag = false;
			if (Main.mouseItem.stack <= 0)
			{
				flag = true;
			}
			if (obj.dead)
			{
				flag = true;
			}
			if (flag)
			{
				ExitBuildingMode();
			}
		}
	}

	public static void SetSelectedProfile(string name)
	{
		if (Profiles.ContainsKey(name))
		{
			_selectedProfile = name;
			_currentProfile = Profiles[_selectedProfile];
		}
	}

	public static void Initialize()
	{
		Main.InputProfiles.OnProcessText += PrettyPrintProfiles;
		Player.Hooks.OnEnterWorld += Hook_OnEnterWorld;
		PlayerInputProfile playerInputProfile = new PlayerInputProfile("Redigit's Pick");
		playerInputProfile.Initialize(PresetProfiles.Redigit);
		Profiles.Add(playerInputProfile.Name, playerInputProfile);
		playerInputProfile = new PlayerInputProfile("Yoraiz0r's Pick");
		playerInputProfile.Initialize(PresetProfiles.Yoraiz0r);
		Profiles.Add(playerInputProfile.Name, playerInputProfile);
		playerInputProfile = new PlayerInputProfile("Console (Playstation)");
		playerInputProfile.Initialize(PresetProfiles.ConsolePS);
		Profiles.Add(playerInputProfile.Name, playerInputProfile);
		playerInputProfile = new PlayerInputProfile("Console (Xbox)");
		playerInputProfile.Initialize(PresetProfiles.ConsoleXBox);
		Profiles.Add(playerInputProfile.Name, playerInputProfile);
		playerInputProfile = new PlayerInputProfile("Custom");
		playerInputProfile.Initialize(PresetProfiles.Redigit);
		Profiles.Add(playerInputProfile.Name, playerInputProfile);
		playerInputProfile = new PlayerInputProfile("Redigit's Pick");
		playerInputProfile.Initialize(PresetProfiles.Redigit);
		OriginalProfiles.Add(playerInputProfile.Name, playerInputProfile);
		playerInputProfile = new PlayerInputProfile("Yoraiz0r's Pick");
		playerInputProfile.Initialize(PresetProfiles.Yoraiz0r);
		OriginalProfiles.Add(playerInputProfile.Name, playerInputProfile);
		playerInputProfile = new PlayerInputProfile("Console (Playstation)");
		playerInputProfile.Initialize(PresetProfiles.ConsolePS);
		OriginalProfiles.Add(playerInputProfile.Name, playerInputProfile);
		playerInputProfile = new PlayerInputProfile("Console (Xbox)");
		playerInputProfile.Initialize(PresetProfiles.ConsoleXBox);
		OriginalProfiles.Add(playerInputProfile.Name, playerInputProfile);
		SetSelectedProfile("Custom");
		Triggers.Initialize();
	}

	public static void Hook_OnEnterWorld(Player player)
	{
		if (player.whoAmI == Main.myPlayer)
		{
			Main.SmartCursorWanted_GamePad = true;
		}
	}

	public static bool Save()
	{
		Main.InputProfiles.Clear();
		Main.InputProfiles.Put("Selected Profile", _selectedProfile);
		foreach (KeyValuePair<string, PlayerInputProfile> profile in Profiles)
		{
			Main.InputProfiles.Put(profile.Value.Name, profile.Value.Save());
		}
		return Main.InputProfiles.Save();
	}

	public static void Load()
	{
		Main.InputProfiles.Load();
		Dictionary<string, PlayerInputProfile> dictionary = new Dictionary<string, PlayerInputProfile>();
		string currentValue = null;
		Main.InputProfiles.Get("Selected Profile", ref currentValue);
		List<string> allKeys = Main.InputProfiles.GetAllKeys();
		for (int i = 0; i < allKeys.Count; i++)
		{
			string text = allKeys[i];
			if (text == "Selected Profile" || string.IsNullOrEmpty(text))
			{
				continue;
			}
			Dictionary<string, object> currentValue2 = new Dictionary<string, object>();
			Main.InputProfiles.Get(text, ref currentValue2);
			if (currentValue2.Count > 0)
			{
				PlayerInputProfile playerInputProfile = new PlayerInputProfile(text);
				playerInputProfile.Initialize(PresetProfiles.None);
				if (playerInputProfile.Load(currentValue2))
				{
					dictionary.Add(text, playerInputProfile);
				}
			}
		}
		if (dictionary.Count > 0)
		{
			Profiles = dictionary;
			if (!string.IsNullOrEmpty(currentValue) && Profiles.ContainsKey(currentValue))
			{
				SetSelectedProfile(currentValue);
			}
			else
			{
				SetSelectedProfile(Profiles.Keys.First());
			}
		}
	}

	public static void ManageVersion_1_3()
	{
		PlayerInputProfile playerInputProfile = Profiles["Custom"];
		string[,] array = new string[20, 2]
		{
			{ "KeyUp", "Up" },
			{ "KeyDown", "Down" },
			{ "KeyLeft", "Left" },
			{ "KeyRight", "Right" },
			{ "KeyJump", "Jump" },
			{ "KeyThrowItem", "Throw" },
			{ "KeyInventory", "Inventory" },
			{ "KeyQuickHeal", "QuickHeal" },
			{ "KeyQuickMana", "QuickMana" },
			{ "KeyQuickBuff", "QuickBuff" },
			{ "KeyUseHook", "Grapple" },
			{ "KeyAutoSelect", "SmartSelect" },
			{ "KeySmartCursor", "SmartCursor" },
			{ "KeyMount", "QuickMount" },
			{ "KeyMapStyle", "MapStyle" },
			{ "KeyFullscreenMap", "MapFull" },
			{ "KeyMapZoomIn", "MapZoomIn" },
			{ "KeyMapZoomOut", "MapZoomOut" },
			{ "KeyMapAlphaUp", "MapAlphaUp" },
			{ "KeyMapAlphaDown", "MapAlphaDown" }
		};
		for (int i = 0; i < array.GetLength(0); i++)
		{
			string currentValue = null;
			Main.Configuration.Get(array[i, 0], ref currentValue);
			if (currentValue != null)
			{
				playerInputProfile.InputModes[InputMode.Keyboard].KeyStatus[array[i, 1]] = new List<string> { currentValue };
				playerInputProfile.InputModes[InputMode.KeyboardUI].KeyStatus[array[i, 1]] = new List<string> { currentValue };
			}
		}
	}

	public static void LockGamepadButtons(string TriggerName)
	{
		List<string> value = null;
		KeyConfiguration value2 = null;
		if (CurrentProfile.InputModes.TryGetValue(CurrentInputMode, out value2) && value2.KeyStatus.TryGetValue(TriggerName, out value))
		{
			_buttonsLocked.AddRange(value);
		}
	}

	public static bool IsGamepadButtonLockedFromUse(string keyName)
	{
		return _buttonsLocked.Contains(keyName);
	}

	public static void UpdateInput()
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		SettingsForUI.UpdateCounters();
		Triggers.Reset();
		ScrollWheelValueOld = ScrollWheelValue;
		ScrollWheelValue = 0;
		GamepadThumbstickLeft = Vector2.Zero;
		GamepadThumbstickRight = Vector2.Zero;
		GrappleAndInteractAreShared = (UsingGamepad || SteamDeckIsUsed) && CurrentProfile.InputModes[InputMode.XBoxGamepad].DoGrappleAndInteractShareTheSameKey;
		if (InBuildingMode && !UsingGamepad)
		{
			ExitBuildingMode();
		}
		if (_canReleaseRebindingLock && NavigatorRebindingLock > 0)
		{
			NavigatorRebindingLock--;
			Triggers.Current.UsedMovementKey = false;
			if (NavigatorRebindingLock == 0 && _memoOfLastPoint != -1)
			{
				UIManageControls.ForceMoveTo = _memoOfLastPoint;
				_memoOfLastPoint = -1;
			}
		}
		_canReleaseRebindingLock = true;
		VerifyBuildingMode();
		MouseInput();
		int num = (int)(0u | (KeyboardInput() ? 1u : 0u)) | (GamePadInput() ? 1 : 0);
		Triggers.Update();
		PostInput();
		ScrollWheelDelta = ScrollWheelValue - ScrollWheelValueOld;
		ScrollWheelDeltaForUI = ScrollWheelDelta;
		WritingText = false;
		UpdateMainMouse();
		Main.mouseLeft = Triggers.Current.MouseLeft;
		Main.mouseRight = Triggers.Current.MouseRight;
		CacheZoomableValues();
		if (num != 0 && OnActionableInput != null)
		{
			OnActionableInput();
		}
	}

	public static void UpdateMainMouse()
	{
		Main.lastMouseX = Main.mouseX;
		Main.lastMouseY = Main.mouseY;
		Main.mouseX = MouseX;
		Main.mouseY = MouseY;
	}

	public static void CacheZoomableValues()
	{
		CacheOriginalInput();
		CacheOriginalScreenDimensions();
	}

	public static void CacheMousePositionForZoom()
	{
		float num = 1f;
		_originalMouseX = (int)((float)Main.mouseX * num);
		_originalMouseY = (int)((float)Main.mouseY * num);
	}

	private static void CacheOriginalInput()
	{
		_originalMouseX = Main.mouseX;
		_originalMouseY = Main.mouseY;
		_originalLastMouseX = Main.lastMouseX;
		_originalLastMouseY = Main.lastMouseY;
	}

	public static void CacheOriginalScreenDimensions()
	{
		_originalScreenWidth = Main.screenWidth;
		_originalScreenHeight = Main.screenHeight;
	}

	public static void UpdateHousingCursor()
	{
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		if (ControllerHousingCursorActive)
		{
			Vector2 gamepadThumbstickRight = GamepadThumbstickRight;
			Vector2 val = gamepadThumbstickRight;
			if (val != Vector2.Zero)
			{
				val.Normalize();
			}
			_ = Main.LocalPlayer.Center;
			float m = Main.GameViewMatrix.ZoomMatrix.M11;
			if (gamepadThumbstickRight != Vector2.Zero)
			{
				Vector2 val2 = new Vector2(10f);
				Vector2 val3 = gamepadThumbstickRight * val2 * m;
				HousingMouseOffset += val3;
			}
			Vector2 val4 = new Vector2(20f, 20f);
			Vector2 val5 = new Vector2((float)Main.screenWidth, (float)Main.screenHeight);
			HousingWorldPosition = Vector2.Clamp(HousingWorldPosition, val4.ScreenToWorldPosition(), (val5 - val4).ScreenToWorldPosition());
		}
		else
		{
			HousingMouseOffset = Vector2.Zero;
		}
	}

	private static bool GamePadInput()
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0191: Unknown result type (might be due to invalid IL or missing references)
		//IL_0196: Unknown result type (might be due to invalid IL or missing references)
		//IL_019a: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0200: Unknown result type (might be due to invalid IL or missing references)
		//IL_0205: Unknown result type (might be due to invalid IL or missing references)
		//IL_022e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0233: Unknown result type (might be due to invalid IL or missing references)
		//IL_0238: Unknown result type (might be due to invalid IL or missing references)
		//IL_023d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0242: Unknown result type (might be due to invalid IL or missing references)
		//IL_0244: Unknown result type (might be due to invalid IL or missing references)
		//IL_0249: Unknown result type (might be due to invalid IL or missing references)
		//IL_024b: Unknown result type (might be due to invalid IL or missing references)
		//IL_024d: Unknown result type (might be due to invalid IL or missing references)
		//IL_024f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0251: Unknown result type (might be due to invalid IL or missing references)
		//IL_0264: Unknown result type (might be due to invalid IL or missing references)
		//IL_0266: Unknown result type (might be due to invalid IL or missing references)
		//IL_0268: Unknown result type (might be due to invalid IL or missing references)
		//IL_026a: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0331: Unknown result type (might be due to invalid IL or missing references)
		//IL_0336: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_039a: Unknown result type (might be due to invalid IL or missing references)
		//IL_039f: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0341: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0409: Unknown result type (might be due to invalid IL or missing references)
		//IL_040e: Unknown result type (might be due to invalid IL or missing references)
		//IL_03af: Unknown result type (might be due to invalid IL or missing references)
		//IL_0359: Unknown result type (might be due to invalid IL or missing references)
		//IL_0319: Unknown result type (might be due to invalid IL or missing references)
		//IL_0472: Unknown result type (might be due to invalid IL or missing references)
		//IL_0477: Unknown result type (might be due to invalid IL or missing references)
		//IL_047c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0419: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0382: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0487: Unknown result type (might be due to invalid IL or missing references)
		//IL_0431: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_054a: Unknown result type (might be due to invalid IL or missing references)
		//IL_054f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0554: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_045a: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_05be: Unknown result type (might be due to invalid IL or missing references)
		//IL_055f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0509: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0624: Unknown result type (might be due to invalid IL or missing references)
		//IL_0629: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0578: Unknown result type (might be due to invalid IL or missing references)
		//IL_0532: Unknown result type (might be due to invalid IL or missing references)
		//IL_067e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0683: Unknown result type (might be due to invalid IL or missing references)
		//IL_063b: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0695: Unknown result type (might be due to invalid IL or missing references)
		//IL_0664: Unknown result type (might be due to invalid IL or missing references)
		//IL_060a: Unknown result type (might be due to invalid IL or missing references)
		//IL_08ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_08ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_08fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_08fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_08bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_08c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_08c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_08d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_08d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_08da: Unknown result type (might be due to invalid IL or missing references)
		//IL_0909: Unknown result type (might be due to invalid IL or missing references)
		//IL_090b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0930: Unknown result type (might be due to invalid IL or missing references)
		//IL_095f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0966: Unknown result type (might be due to invalid IL or missing references)
		//IL_09a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_09ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_09af: Unknown result type (might be due to invalid IL or missing references)
		//IL_098b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0af3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0af5: Unknown result type (might be due to invalid IL or missing references)
		//IL_09c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a05: Unknown result type (might be due to invalid IL or missing references)
		//IL_09e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b2a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b2e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b33: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b35: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b3a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b3e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b4e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b53: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b58: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a74: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a80: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a82: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a84: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a8b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a90: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a22: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ab2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0abf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a54: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a59: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a63: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a68: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a6d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a44: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a46: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a4b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a50: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b7d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b89: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ba3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ba8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0baf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bb4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bda: Unknown result type (might be due to invalid IL or missing references)
		//IL_0be4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bcc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bd1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bd3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bd8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cdd: Unknown result type (might be due to invalid IL or missing references)
		bool flag = false;
		ScrollWheelValue += GamepadScrollValue;
		GamePadState val = default;
		bool flag2 = false;
		for (int i = 0; i < 4; i++)
		{
			GamePadState state = GamePad.GetState((PlayerIndex)i);
			if (state.IsConnected)
			{
				flag2 = true;
				val = state;
				break;
			}
		}
		if (Main.SettingBlockGamepadsEntirely)
		{
			return false;
		}
		if (!flag2)
		{
			return false;
		}
		if (!FocusHelper.AllowInputProcessingForGamepad)
		{
			return false;
		}
		Player player = Main.player[Main.myPlayer];
		bool flag3 = UILinkPointNavigator.Available && !InBuildingMode;
		InputMode inputMode = InputMode.XBoxGamepad;
		if ((Main.gameMenu | flag3) || player.talkNPC != -1 || player.sign != -1 || IngameFancyUI.CanCover())
		{
			inputMode = InputMode.XBoxGamepadUI;
		}
		if (!Main.gameMenu && InBuildingMode)
		{
			inputMode = InputMode.XBoxGamepad;
		}
		if (CurrentInputMode == InputMode.XBoxGamepad && inputMode == InputMode.XBoxGamepadUI)
		{
			flag = true;
		}
		if (CurrentInputMode == InputMode.XBoxGamepadUI && inputMode == InputMode.XBoxGamepad)
		{
			flag = true;
		}
		if (flag)
		{
			CurrentInputMode = inputMode;
		}
		KeyConfiguration keyConfiguration = CurrentProfile.InputModes[inputMode];
		int num = 2145386496;
		for (int j = 0; j < ButtonsGamepad.Length; j++)
		{
			if ((int)((uint)num & (uint)ButtonsGamepad[j]) > 0)
			{
				continue;
			}
			string text = ((object)ButtonsGamepad[j]/*cast due to constrained. prefix*/).ToString();
			bool flag4 = _buttonsLocked.Contains(text);
			if (val.IsButtonDown(ButtonsGamepad[j]))
			{
				if (!flag4)
				{
					if (CheckRebindingProcessGamepad(text))
					{
						return false;
					}
					keyConfiguration.Processkey(Triggers.Current, text, inputMode);
					flag = true;
				}
			}
			else
			{
				_buttonsLocked.Remove(text);
			}
		}
		GamePadThumbSticks thumbSticks = val.ThumbSticks;
		GamepadThumbstickLeft = thumbSticks.Left * new Vector2(1f, -1f) * new Vector2((float)(CurrentProfile.LeftThumbstickInvertX.ToDirectionInt() * -1), (float)(CurrentProfile.LeftThumbstickInvertY.ToDirectionInt() * -1));
		thumbSticks = val.ThumbSticks;
		GamepadThumbstickRight = thumbSticks.Right * new Vector2(1f, -1f) * new Vector2((float)(CurrentProfile.RightThumbstickInvertX.ToDirectionInt() * -1), (float)(CurrentProfile.RightThumbstickInvertY.ToDirectionInt() * -1));
		Vector2 gamepadThumbstickRight = GamepadThumbstickRight;
		Vector2 gamepadThumbstickLeft = GamepadThumbstickLeft;
		Vector2 val2 = gamepadThumbstickRight;
		if (val2 != Vector2.Zero)
		{
			val2.Normalize();
		}
		Vector2 val3 = gamepadThumbstickLeft;
		if (val3 != Vector2.Zero)
		{
			val3.Normalize();
		}
		float num2 = 0.6f;
		float triggersDeadzone = CurrentProfile.TriggersDeadzone;
		if (inputMode == InputMode.XBoxGamepadUI)
		{
			num2 = 0.4f;
			if (GamepadAllowScrolling)
			{
				GamepadScrollValue -= (int)(gamepadThumbstickRight.Y * 16f);
			}
			GamepadAllowScrolling = false;
		}
		if (Vector2.Dot(-Vector2.UnitX, val3) >= num2 && gamepadThumbstickLeft.X < 0f - CurrentProfile.LeftThumbstickDeadzoneX)
		{
			if (CheckRebindingProcessGamepad(((object)(Buttons)2097152/*cast due to constrained. prefix*/).ToString()))
			{
				return false;
			}
			keyConfiguration.Processkey(Triggers.Current, ((object)(Buttons)2097152/*cast due to constrained. prefix*/).ToString(), inputMode);
			flag = true;
		}
		if (Vector2.Dot(Vector2.UnitX, val3) >= num2 && gamepadThumbstickLeft.X > CurrentProfile.LeftThumbstickDeadzoneX)
		{
			if (CheckRebindingProcessGamepad(((object)(Buttons)1073741824/*cast due to constrained. prefix*/).ToString()))
			{
				return false;
			}
			keyConfiguration.Processkey(Triggers.Current, ((object)(Buttons)1073741824/*cast due to constrained. prefix*/).ToString(), inputMode);
			flag = true;
		}
		if (Vector2.Dot(-Vector2.UnitY, val3) >= num2 && gamepadThumbstickLeft.Y < 0f - CurrentProfile.LeftThumbstickDeadzoneY)
		{
			if (CheckRebindingProcessGamepad(((object)(Buttons)268435456/*cast due to constrained. prefix*/).ToString()))
			{
				return false;
			}
			keyConfiguration.Processkey(Triggers.Current, ((object)(Buttons)268435456/*cast due to constrained. prefix*/).ToString(), inputMode);
			flag = true;
		}
		if (Vector2.Dot(Vector2.UnitY, val3) >= num2 && gamepadThumbstickLeft.Y > CurrentProfile.LeftThumbstickDeadzoneY)
		{
			if (CheckRebindingProcessGamepad(((object)(Buttons)536870912/*cast due to constrained. prefix*/).ToString()))
			{
				return false;
			}
			keyConfiguration.Processkey(Triggers.Current, ((object)(Buttons)536870912/*cast due to constrained. prefix*/).ToString(), inputMode);
			flag = true;
		}
		if (Vector2.Dot(-Vector2.UnitX, val2) >= num2 && gamepadThumbstickRight.X < 0f - CurrentProfile.RightThumbstickDeadzoneX)
		{
			if (CheckRebindingProcessGamepad(((object)(Buttons)134217728/*cast due to constrained. prefix*/).ToString()))
			{
				return false;
			}
			keyConfiguration.Processkey(Triggers.Current, ((object)(Buttons)134217728/*cast due to constrained. prefix*/).ToString(), inputMode);
			flag = true;
		}
		if (Vector2.Dot(Vector2.UnitX, val2) >= num2 && gamepadThumbstickRight.X > CurrentProfile.RightThumbstickDeadzoneX)
		{
			if (CheckRebindingProcessGamepad(((object)(Buttons)67108864/*cast due to constrained. prefix*/).ToString()))
			{
				return false;
			}
			keyConfiguration.Processkey(Triggers.Current, ((object)(Buttons)67108864/*cast due to constrained. prefix*/).ToString(), inputMode);
			flag = true;
		}
		if (Vector2.Dot(-Vector2.UnitY, val2) >= num2 && gamepadThumbstickRight.Y < 0f - CurrentProfile.RightThumbstickDeadzoneY)
		{
			if (CheckRebindingProcessGamepad(((object)(Buttons)16777216/*cast due to constrained. prefix*/).ToString()))
			{
				return false;
			}
			keyConfiguration.Processkey(Triggers.Current, ((object)(Buttons)16777216/*cast due to constrained. prefix*/).ToString(), inputMode);
			flag = true;
		}
		if (Vector2.Dot(Vector2.UnitY, val2) >= num2 && gamepadThumbstickRight.Y > CurrentProfile.RightThumbstickDeadzoneY)
		{
			if (CheckRebindingProcessGamepad(((object)(Buttons)33554432/*cast due to constrained. prefix*/).ToString()))
			{
				return false;
			}
			keyConfiguration.Processkey(Triggers.Current, ((object)(Buttons)33554432/*cast due to constrained. prefix*/).ToString(), inputMode);
			flag = true;
		}
		GamePadTriggers triggers = val.Triggers;
		if (triggers.Left > triggersDeadzone)
		{
			if (CheckRebindingProcessGamepad(((object)(Buttons)8388608/*cast due to constrained. prefix*/).ToString()))
			{
				return false;
			}
			keyConfiguration.Processkey(Triggers.Current, ((object)(Buttons)8388608/*cast due to constrained. prefix*/).ToString(), inputMode);
			flag = true;
		}
		triggers = val.Triggers;
		if (triggers.Right > triggersDeadzone)
		{
			string newKey = ((object)(Buttons)4194304/*cast due to constrained. prefix*/).ToString();
			if (CheckRebindingProcessGamepad(newKey))
			{
				return false;
			}
			if (inputMode == InputMode.XBoxGamepadUI && SteamDeckIsUsed && SettingsForUI.CurrentCursorMode == CursorMode.Mouse)
			{
				Triggers.Current.MouseLeft = true;
			}
			else
			{
				keyConfiguration.Processkey(Triggers.Current, newKey, inputMode);
				flag = true;
			}
		}
		bool flag5 = player.scope;
		ButtonControlMode panControl_Gamepad = CurrentProfile.PanControl_Gamepad;
		if (panControl_Gamepad == ButtonControlMode.OffAlways)
		{
			flag5 = false;
		}
		if (panControl_Gamepad == ButtonControlMode.Click)
		{
			flag5 &= player.hasClickPanOn;
		}
		if (panControl_Gamepad == ButtonControlMode.Hold)
		{
			flag5 &= Triggers.Current.MouseRight;
		}
		bool flag6 = ItemID.Sets.GamepadWholeScreenUseRange[player.inventory[player.selectedItem].type] | flag5;
		Item item = player.inventory[player.selectedItem];
		bool flag7 = false;
		bool flag8 = !Main.gameMenu && !flag3 && Main.SmartCursorWanted_GamePad;
		bool num3 = player.rulerGrid && player.builderAccStatus[1] == 0;
		bool flag9 = player.rulerLine && player.builderAccStatus[0] == 0;
		bool flag10 = num3 | flag9;
		if (!flag8 & flag10)
		{
			flag6 = true;
		}
		int num4 = item.tileBoost + ItemID.Sets.GamepadExtraRange[item.type];
		if (player.yoyoString && ItemID.Sets.Yoyo[item.type])
		{
			num4 += 5;
		}
		else if (item.createTile < 0 && item.createWall <= 0 && item.shoot > 0)
		{
			num4 += 10;
		}
		else if (player.controlTorch)
		{
			num4++;
		}
		if (item.createWall > 0 || item.createTile > 0 || item.tileWand > 0)
		{
			num4 += player.blockRange;
		}
		if (flag6)
		{
			num4 += 30;
		}
		if (player.mount.Active && player.mount.Type == 8)
		{
			num4 = 10;
		}
		if (!CursorIsBusy)
		{
			bool flag11 = Main.mapFullscreen || (!Main.gameMenu && !flag3);
			int num5 = Main.screenWidth / 2;
			int num6 = Main.screenHeight / 2;
			if ((!Main.mapFullscreen & flag11) && !flag6)
			{
				Point val4 = Main.ReverseGravitySupport(player.Center - Main.screenPosition).ToPoint();
				num5 = val4.X;
				num6 = val4.Y;
			}
			if ((player.velocity == Vector2.Zero && gamepadThumbstickLeft == Vector2.Zero && gamepadThumbstickRight == Vector2.Zero) & flag8)
			{
				num5 += player.direction * 10;
			}
			float m = Main.GameViewMatrix.ZoomMatrix.M11;
			smartSelectPointer.UpdateSize(new Vector2((float)(Player.tileRangeX * 16 + num4 * 16), (float)(Player.tileRangeY * 16 + num4 * 16)) * m);
			if (flag6)
			{
				smartSelectPointer.UpdateSize(new Vector2((float)(Math.Max(Main.screenWidth, Main.screenHeight) / 2)));
			}
			smartSelectPointer.UpdateCenter(new Vector2((float)num5, (float)num6));
			bool flag12 = false;
			if ((gamepadThumbstickRight != Vector2.Zero) & flag11)
			{
				Vector2 val5 = new Vector2(8f);
				if (!Main.gameMenu && Main.mapFullscreen)
				{
					val5 = new Vector2(16f);
				}
				if (flag8)
				{
					val5 = new Vector2((float)((Player.tileRangeX + num4) * 16), (float)((Player.tileRangeY + num4) * 16));
					if (flag6)
					{
						val5 = new Vector2((float)(Math.Max(Main.screenWidth, Main.screenHeight) / 2));
					}
				}
				else if (!Main.mapFullscreen)
				{
					val5 = ((!player.inventory[player.selectedItem].mech) ? (val5 + new Vector2((float)num4) / 4f) : (val5 + Vector2.Zero));
				}
				float m2 = Main.GameViewMatrix.ZoomMatrix.M11;
				Vector2 val6 = gamepadThumbstickRight * val5 * m2;
				int num7 = MouseX - num5;
				int num8 = MouseY - num6;
				if (flag8)
				{
					num7 = 0;
					num8 = 0;
				}
				num7 += (int)val6.X;
				num8 += (int)val6.Y;
				MouseX = num7 + num5;
				MouseY = num8 + num6;
				flag = true;
				flag7 = true;
				SettingsForUI.SetCursorMode(CursorMode.Gamepad);
				flag12 = true;
			}
			bool allowSecondaryGamepadAim = SettingsForUI.AllowSecondaryGamepadAim;
			if ((gamepadThumbstickLeft != Vector2.Zero) & flag11)
			{
				float num9 = 8f;
				if (!Main.gameMenu && Main.mapFullscreen)
				{
					num9 = 3f;
				}
				if (Main.mapFullscreen)
				{
					Vector2 val7 = gamepadThumbstickLeft * num9;
					Main.mapFullscreenPos += val7 * num9 * (1f / Main.mapFullscreenScale);
					flag = true;
				}
				else if ((!flag7 && Main.SmartCursorWanted_GamePad) & allowSecondaryGamepadAim)
				{
					float m3 = Main.GameViewMatrix.ZoomMatrix.M11;
					Vector2 val8 = gamepadThumbstickLeft * new Vector2((float)((Player.tileRangeX + num4) * 16), (float)((Player.tileRangeY + num4) * 16)) * m3;
					if (flag6)
					{
						val8 = new Vector2((float)(Math.Max(Main.screenWidth, Main.screenHeight) / 2)) * gamepadThumbstickLeft;
					}
					int num10 = (int)val8.X;
					int num11 = (int)val8.Y;
					MouseX = num10 + num5;
					MouseY = num11 + num6;
					flag7 = true;
				}
				flag = true;
			}
			if ((CurrentInputMode == InputMode.XBoxGamepad) | flag12)
			{
				HandleDpadSnap();
				if (SettingsForUI.AllowSecondaryGamepadAim)
				{
					int num12 = MouseX - num5;
					int num13 = MouseY - num6;
					if (!Main.gameMenu && !flag3)
					{
						if (flag6 && !Main.mapFullscreen)
						{
							float num14 = 1f;
							int num15 = Main.screenWidth / 2;
							int num16 = Main.screenHeight / 2;
							num12 = (int)Utils.Clamp(num12, (float)(-num15) * num14, (float)num15 * num14);
							num13 = (int)Utils.Clamp(num13, (float)(-num16) * num14, (float)num16 * num14);
						}
						else
						{
							float num17 = 0f;
							if (player.HeldItem.createTile >= 0 || player.HeldItem.createWall > 0 || player.HeldItem.tileWand >= 0)
							{
								num17 = 0.5f;
							}
							float m4 = Main.GameViewMatrix.ZoomMatrix.M11;
							float num18 = (0f - ((float)(Player.tileRangeY + num4) - num17)) * 16f * m4;
							float max = ((float)(Player.tileRangeY + num4) - num17) * 16f * m4;
							num18 -= (float)(player.height / 16 / 2 * 16);
							num12 = (int)Utils.Clamp(num12, (0f - ((float)(Player.tileRangeX + num4) - num17)) * 16f * m4, ((float)(Player.tileRangeX + num4) - num17) * 16f * m4);
							num13 = (int)Utils.Clamp(num13, num18, max);
						}
						if (flag8 && (!flag | flag6))
						{
							float num19 = 0.81f;
							if (flag6)
							{
								num19 = 0.95f;
							}
							num12 = (int)((float)num12 * num19);
							num13 = (int)((float)num13 * num19);
						}
					}
					else
					{
						num12 = Utils.Clamp(num12, -num5 + 10, num5 - 10);
						num13 = Utils.Clamp(num13, -num6 + 10, num6 - 10);
					}
					MouseX = num12 + num5;
					MouseY = num13 + num6;
				}
			}
		}
		if (flag)
		{
			CurrentInputMode = inputMode;
		}
		if ((CurrentInputMode != InputMode.XBoxGamepadUI) & flag)
		{
			PreventCursorModeSwappingToGamepad = true;
		}
		if (!flag)
		{
			PreventCursorModeSwappingToGamepad = false;
		}
		if (((CurrentInputMode == InputMode.XBoxGamepadUI) & flag) && !PreventCursorModeSwappingToGamepad)
		{
			SettingsForUI.SetCursorMode(CursorMode.Gamepad);
		}
		return flag;
	}

	private static void MouseInput()
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_0115: Unknown result type (might be due to invalid IL or missing references)
		//IL_011b: Invalid comparison between Unknown and I4
		//IL_0133: Unknown result type (might be due to invalid IL or missing references)
		//IL_0139: Invalid comparison between Unknown and I4
		//IL_0151: Unknown result type (might be due to invalid IL or missing references)
		//IL_0157: Invalid comparison between Unknown and I4
		//IL_016f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0175: Invalid comparison between Unknown and I4
		//IL_018d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0193: Invalid comparison between Unknown and I4
		bool flag = false;
		MouseInfoOld = MouseInfo;
		MouseKeys.Clear();
		if (!FocusHelper.AllowInputProcessing)
		{
			MouseInfo = new MouseState(MouseInfo.X, MouseInfo.Y, 0, (ButtonState)0, (ButtonState)0, (ButtonState)0, (ButtonState)0, (ButtonState)0);
			MouseState state = Mouse.GetState();
			ScrollWheelValue = (ScrollWheelValueOld = state.ScrollWheelValue);
			return;
		}
		MouseInfo = Mouse.GetState();
		ScrollWheelValue += MouseInfo.ScrollWheelValue;
		if (DebugOptions.ForceGamepad)
		{
			return;
		}
		if (MouseInfo.X != MouseInfoOld.X || MouseInfo.Y != MouseInfoOld.Y || MouseInfo.ScrollWheelValue != MouseInfoOld.ScrollWheelValue)
		{
			MouseX = (int)((float)MouseInfo.X * RawMouseScale.X);
			MouseY = (int)((float)MouseInfo.Y * RawMouseScale.Y);
			if (!PreventFirstMousePositionGrab)
			{
				flag = true;
				SettingsForUI.SetCursorMode(CursorMode.Mouse);
			}
			PreventFirstMousePositionGrab = false;
		}
		if ((int)MouseInfo.LeftButton == 1)
		{
			MouseKeys.Add("Mouse1");
			flag = true;
		}
		if ((int)MouseInfo.RightButton == 1)
		{
			MouseKeys.Add("Mouse2");
			flag = true;
		}
		if ((int)MouseInfo.MiddleButton == 1)
		{
			MouseKeys.Add("Mouse3");
			flag = true;
		}
		if ((int)MouseInfo.XButton1 == 1)
		{
			MouseKeys.Add("Mouse4");
			flag = true;
		}
		if ((int)MouseInfo.XButton2 == 1)
		{
			MouseKeys.Add("Mouse5");
			flag = true;
		}
		if (flag)
		{
			CurrentInputMode = InputMode.Mouse;
			Triggers.Current.UsedMovementKey = false;
		}
	}

	private static bool KeyboardInput()
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Invalid comparison between Unknown and I4
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Invalid comparison between Unknown and I4
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Invalid comparison between Unknown and I4
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Invalid comparison between Unknown and I4
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0167: Unknown result type (might be due to invalid IL or missing references)
		//IL_016c: Unknown result type (might be due to invalid IL or missing references)
		//IL_010c: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_021a: Unknown result type (might be due to invalid IL or missing references)
		//IL_022e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0274: Unknown result type (might be due to invalid IL or missing references)
		bool flag = false;
		bool flag2 = false;
		List<Keys> pressedKeys = GetPressedKeys();
		DebugKeys(pressedKeys);
		if (pressedKeys.Count == 0 && MouseKeys.Count == 0)
		{
			Main.blockKey = ((object)(Keys)0/*cast due to constrained. prefix*/).ToString();
			return false;
		}
		for (int i = 0; i < pressedKeys.Count; i++)
		{
			if ((int)pressedKeys[i] == 160 || (int)pressedKeys[i] == 161)
			{
				flag = true;
			}
			else if ((int)pressedKeys[i] == 164 || (int)pressedKeys[i] == 165)
			{
				flag2 = true;
			}
			Main.ChromaPainter.PressKey(pressedKeys[i]);
		}
		if (Main.blockKey != ((object)(Keys)0/*cast due to constrained. prefix*/).ToString())
		{
			bool flag3 = false;
			for (int j = 0; j < pressedKeys.Count; j++)
			{
				if (((object)pressedKeys[j]/*cast due to constrained. prefix*/).ToString() == Main.blockKey)
				{
					pressedKeys[j] = (Keys)0;
					flag3 = true;
				}
			}
			if (!flag3)
			{
				Main.blockKey = ((object)(Keys)0/*cast due to constrained. prefix*/).ToString();
			}
		}
		KeyConfiguration keyConfiguration = CurrentProfile.InputModes[InputMode.Keyboard];
		if (Main.gameMenu && !WritingText)
		{
			keyConfiguration = CurrentProfile.InputModes[InputMode.KeyboardUI];
		}
		List<string> list = new List<string>(pressedKeys.Count);
		for (int k = 0; k < pressedKeys.Count; k++)
		{
			list.Add(((object)pressedKeys[k]/*cast due to constrained. prefix*/).ToString());
		}
		if (WritingText)
		{
			list.Clear();
		}
		int count = list.Count;
		list.AddRange(MouseKeys);
		bool flag4 = false;
		for (int l = 0; l < list.Count; l++)
		{
			if (l < count && (int)pressedKeys[l] == 0)
			{
				continue;
			}
			string newKey = list[l];
			if (!(list[l] == ((object)(Keys)9/*cast due to constrained. prefix*/).ToString()) || !((flag && SocialAPI.Mode == SocialMode.Steam) | flag2))
			{
				if (CheckRebindingProcessKeyboard(newKey))
				{
					return false;
				}
				_ = Main.oldKeyState;
				if (l >= count || !Main.oldKeyState.IsKeyDown(pressedKeys[l]))
				{
					keyConfiguration.Processkey(Triggers.Current, newKey, InputMode.Keyboard);
				}
				else
				{
					keyConfiguration.CopyKeyState(Triggers.Old, Triggers.Current, newKey);
				}
				if (l >= count || (int)pressedKeys[l] != 0)
				{
					flag4 = true;
				}
			}
		}
		if (flag4)
		{
			CurrentInputMode = InputMode.Keyboard;
		}
		return flag4;
	}

	private static void DebugKeys(List<Keys> keys)
	{
	}

	private static void FixDerpedRebinds()
	{
		List<string> list = new List<string> { "MouseLeft", "MouseRight", "Inventory" };
		foreach (InputMode value in Enum.GetValues(typeof(InputMode)))
		{
			if (value == InputMode.Mouse)
			{
				continue;
			}
			FixKeysConflict(value, list);
			foreach (string item in list)
			{
				if (CurrentProfile.InputModes[value].KeyStatus[item].Count < 1)
				{
					ResetKeyBinding(value, item);
				}
			}
		}
	}

	private static void FixKeysConflict(InputMode inputMode, List<string> triggers)
	{
		for (int i = 0; i < triggers.Count; i++)
		{
			for (int j = i + 1; j < triggers.Count; j++)
			{
				List<string> list = CurrentProfile.InputModes[inputMode].KeyStatus[triggers[i]];
				List<string> list2 = CurrentProfile.InputModes[inputMode].KeyStatus[triggers[j]];
				foreach (string item in list.Intersect(list2).ToList())
				{
					list.Remove(item);
					list2.Remove(item);
				}
			}
		}
	}

	private static void ResetKeyBinding(InputMode inputMode, string trigger)
	{
		string key = "Redigit's Pick";
		if (OriginalProfiles.ContainsKey(_selectedProfile))
		{
			key = _selectedProfile;
		}
		CurrentProfile.InputModes[inputMode].KeyStatus[trigger].Clear();
		CurrentProfile.InputModes[inputMode].KeyStatus[trigger].AddRange(OriginalProfiles[key].InputModes[inputMode].KeyStatus[trigger]);
	}

	private static bool CheckRebindingProcessGamepad(string newKey)
	{
		_canReleaseRebindingLock = false;
		if (!CurrentlyRebinding)
		{
			return NavigatorRebindingLock > 0;
		}
		if (CurrentlyRebinding && _listeningInputMode == InputMode.XBoxGamepad)
		{
			NavigatorRebindingLock = 3;
			_memoOfLastPoint = UILinkPointNavigator.CurrentPoint;
			SoundEngine.PlaySound(12);
			if (CurrentProfile.InputModes[InputMode.XBoxGamepad].KeyStatus[ListeningTrigger].Contains(newKey))
			{
				CurrentProfile.InputModes[InputMode.XBoxGamepad].KeyStatus[ListeningTrigger].Remove(newKey);
			}
			else
			{
				CurrentProfile.InputModes[InputMode.XBoxGamepad].KeyStatus[ListeningTrigger] = new List<string> { newKey };
			}
			ListenFor(null, InputMode.XBoxGamepad);
		}
		if (CurrentlyRebinding && _listeningInputMode == InputMode.XBoxGamepadUI)
		{
			NavigatorRebindingLock = 3;
			_memoOfLastPoint = UILinkPointNavigator.CurrentPoint;
			SoundEngine.PlaySound(12);
			if (CurrentProfile.InputModes[InputMode.XBoxGamepadUI].KeyStatus[ListeningTrigger].Contains(newKey))
			{
				CurrentProfile.InputModes[InputMode.XBoxGamepadUI].KeyStatus[ListeningTrigger].Remove(newKey);
			}
			else
			{
				CurrentProfile.InputModes[InputMode.XBoxGamepadUI].KeyStatus[ListeningTrigger] = new List<string> { newKey };
			}
			ListenFor(null, InputMode.XBoxGamepadUI);
		}
		FixDerpedRebinds();
		if (OnBindingChange != null)
		{
			OnBindingChange();
		}
		return NavigatorRebindingLock > 0;
	}

	private static bool CheckRebindingProcessKeyboard(string newKey)
	{
		_canReleaseRebindingLock = false;
		if (!CurrentlyRebinding)
		{
			return NavigatorRebindingLock > 0;
		}
		if (CurrentlyRebinding && _listeningInputMode == InputMode.Keyboard)
		{
			NavigatorRebindingLock = 3;
			_memoOfLastPoint = UILinkPointNavigator.CurrentPoint;
			SoundEngine.PlaySound(12);
			if (CurrentProfile.InputModes[InputMode.Keyboard].KeyStatus[ListeningTrigger].Contains(newKey))
			{
				CurrentProfile.InputModes[InputMode.Keyboard].KeyStatus[ListeningTrigger].Remove(newKey);
			}
			else
			{
				CurrentProfile.InputModes[InputMode.Keyboard].KeyStatus[ListeningTrigger] = new List<string> { newKey };
			}
			ListenFor(null, InputMode.Keyboard);
			Main.blockKey = newKey;
			Main.blockInput = false;
			Main.ChromaPainter.CollectBoundKeys();
		}
		if (CurrentlyRebinding && _listeningInputMode == InputMode.KeyboardUI)
		{
			NavigatorRebindingLock = 3;
			_memoOfLastPoint = UILinkPointNavigator.CurrentPoint;
			SoundEngine.PlaySound(12);
			if (CurrentProfile.InputModes[InputMode.KeyboardUI].KeyStatus[ListeningTrigger].Contains(newKey))
			{
				CurrentProfile.InputModes[InputMode.KeyboardUI].KeyStatus[ListeningTrigger].Remove(newKey);
			}
			else
			{
				CurrentProfile.InputModes[InputMode.KeyboardUI].KeyStatus[ListeningTrigger] = new List<string> { newKey };
			}
			ListenFor(null, InputMode.KeyboardUI);
			Main.blockKey = newKey;
			Main.blockInput = false;
			Main.ChromaPainter.CollectBoundKeys();
		}
		FixDerpedRebinds();
		if (OnBindingChange != null)
		{
			OnBindingChange();
		}
		return NavigatorRebindingLock > 0;
	}

	private static void PostInput()
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		Main.GamepadCursorAlpha = MathHelper.Clamp(Main.GamepadCursorAlpha + ((Main.SmartCursorIsUsed && !UILinkPointNavigator.Available && GamepadThumbstickLeft == Vector2.Zero && GamepadThumbstickRight == Vector2.Zero) ? (-0.05f) : 0.05f), 0f, 1f);
		if (CurrentProfile.HotbarAllowsRadial)
		{
			int num = Triggers.Current.HotbarPlus.ToInt() - Triggers.Current.HotbarMinus.ToInt();
			if (MiscSettingsTEMP.HotbarRadialShouldBeUsed)
			{
				switch (num)
				{
				case 1:
					Triggers.Current.RadialHotbar = true;
					Triggers.JustReleased.RadialHotbar = false;
					break;
				case -1:
					Triggers.Current.RadialQuickbar = true;
					Triggers.JustReleased.RadialQuickbar = false;
					break;
				}
			}
		}
		MiscSettingsTEMP.HotbarRadialShouldBeUsed = false;
	}

	private static void HandleDpadSnap()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0134: Unknown result type (might be due to invalid IL or missing references)
		//IL_0135: Unknown result type (might be due to invalid IL or missing references)
		//IL_014f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0159: Unknown result type (might be due to invalid IL or missing references)
		//IL_015e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0163: Unknown result type (might be due to invalid IL or missing references)
		//IL_0165: Unknown result type (might be due to invalid IL or missing references)
		//IL_0167: Unknown result type (might be due to invalid IL or missing references)
		//IL_016c: Unknown result type (might be due to invalid IL or missing references)
		//IL_016e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0173: Unknown result type (might be due to invalid IL or missing references)
		//IL_0175: Unknown result type (might be due to invalid IL or missing references)
		//IL_018f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0195: Unknown result type (might be due to invalid IL or missing references)
		//IL_019a: Unknown result type (might be due to invalid IL or missing references)
		//IL_019f: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_0116: Unknown result type (might be due to invalid IL or missing references)
		//IL_0117: Unknown result type (might be due to invalid IL or missing references)
		//IL_0119: Unknown result type (might be due to invalid IL or missing references)
		//IL_011e: Unknown result type (might be due to invalid IL or missing references)
		Vector2 val = Vector2.Zero;
		Player player = Main.player[Main.myPlayer];
		for (int i = 0; i < 4; i++)
		{
			bool flag = false;
			Vector2 val2 = Vector2.Zero;
			if (Main.gameMenu || (UILinkPointNavigator.Available && !InBuildingMode))
			{
				return;
			}
			switch (i)
			{
			case 0:
				flag = Triggers.Current.DpadMouseSnap1;
				val2 = -Vector2.UnitY;
				break;
			case 1:
				flag = Triggers.Current.DpadMouseSnap2;
				val2 = Vector2.UnitX;
				break;
			case 2:
				flag = Triggers.Current.DpadMouseSnap3;
				val2 = Vector2.UnitY;
				break;
			case 3:
				flag = Triggers.Current.DpadMouseSnap4;
				val2 = -Vector2.UnitX;
				break;
			}
			if (DpadSnapCooldown[i] > 0)
			{
				DpadSnapCooldown[i]--;
			}
			if (flag)
			{
				if (DpadSnapCooldown[i] == 0)
				{
					int num = 6;
					if (ItemSlot.IsABuildingItem(player.inventory[player.selectedItem]))
					{
						num = player.inventory[player.selectedItem].useTime;
					}
					DpadSnapCooldown[i] = num;
					val += val2;
				}
			}
			else
			{
				DpadSnapCooldown[i] = 0;
			}
		}
		if (val != Vector2.Zero)
		{
			Main.SmartCursorWanted_GamePad = false;
			Matrix val3 = Main.GameViewMatrix.ZoomMatrix * Main.GameViewMatrix.EffectMatrix;
			Matrix val4 = Matrix.Invert(val3);
			Vector2 val5 = Vector2.Transform(Main.MouseScreen, val4);
			val.Y *= Main.LocalPlayer.gravDir;
			Vector2 val6 = Vector2.Transform((val5 + val * new Vector2(16f) + Main.screenPosition).ToTileCoordinates().ToWorldCoordinates() - Main.screenPosition, val3);
			MouseX = (int)val6.X;
			MouseY = (int)val6.Y;
			SettingsForUI.SetCursorMode(CursorMode.Gamepad);
		}
	}

	private static bool ShouldShowInstructionsForGamepad()
	{
		if (!UsingGamepad)
		{
			return SteamDeckIsUsed;
		}
		return true;
	}

	public static string ComposeInstructionsForGamepad()
	{
		string empty = string.Empty;
		InputMode inputMode = InputMode.XBoxGamepad;
		if (Main.gameMenu || UILinkPointNavigator.Available)
		{
			inputMode = InputMode.XBoxGamepadUI;
		}
		if (InBuildingMode && !Main.gameMenu)
		{
			inputMode = InputMode.XBoxGamepad;
		}
		KeyConfiguration keyConfiguration = CurrentProfile.InputModes[inputMode];
		Player localPlayer = Main.LocalPlayer;
		if (Main.mapFullscreen && !Main.gameMenu)
		{
			empty += "          ";
			empty += BuildCommand(Lang.misc[56].Value, ProfileGamepadUI.KeyStatus["Inventory"]);
			empty += BuildCommand(Lang.inter[118].Value, ProfileGamepadUI.KeyStatus["HotbarPlus"]);
			empty += BuildCommand(Lang.inter[119].Value, ProfileGamepadUI.KeyStatus["HotbarMinus"]);
			if (Main.netMode == 1 && Main.player[Main.myPlayer].HasItem(2997))
			{
				empty += BuildCommand(Lang.inter[120].Value, ProfileGamepadUI.KeyStatus["MouseRight"]);
			}
		}
		else if (inputMode == InputMode.XBoxGamepadUI && !InBuildingMode)
		{
			empty = UILinkPointNavigator.GetInstructions();
		}
		else if ((localPlayer.dead && localPlayer.CanDeathSpectate && localPlayer.AnyoneToSpectate()) || localPlayer.spectating >= 0)
		{
			string textValue = Language.GetTextValue((localPlayer.spectating >= 0) ? "Game.GamepadSpectateChangeTarget" : "Game.GamepadSpectate");
			empty += BuildCommand(textValue, keyConfiguration.KeyStatus["Left"], keyConfiguration.KeyStatus["Right"]);
			if (localPlayer.spectating >= 0)
			{
				empty += BuildCommand(Language.GetTextValue("Game.GamepadSpectateCancel"), keyConfiguration.KeyStatus["Jump"]);
			}
			if (localPlayer.CanWormholeToSpectating())
			{
				empty += BuildCommand(Language.GetTextValue("Game.GamepadSpectateWormhole"), keyConfiguration.KeyStatus["QuickBuff"]);
			}
		}
		else
		{
			if (localPlayer.dead)
			{
				return empty;
			}
			empty += BuildCommand(Lang.misc[58].Value, keyConfiguration.KeyStatus["Jump"]);
			empty += BuildCommand(Lang.misc[59].Value, keyConfiguration.KeyStatus["HotbarMinus"], keyConfiguration.KeyStatus["HotbarPlus"]);
			if (InBuildingMode)
			{
				empty += BuildCommand(Lang.menu[6].Value, keyConfiguration.KeyStatus["Inventory"], keyConfiguration.KeyStatus["MouseRight"]);
			}
			if (WiresUI.Open)
			{
				empty += BuildCommand(Lang.misc[53].Value, keyConfiguration.KeyStatus["MouseLeft"]);
				empty += BuildCommand(Lang.misc[56].Value, keyConfiguration.KeyStatus["MouseRight"]);
			}
			else
			{
				Item item = Main.player[Main.myPlayer].inventory[Main.player[Main.myPlayer].selectedItem];
				if (item.damage > 0 && item.ammo == 0)
				{
					empty += BuildCommand(Lang.misc[60].Value, keyConfiguration.KeyStatus["MouseLeft"]);
				}
				else
				{
					empty = ((item.createTile < 0 && item.createWall <= 0) ? (empty + BuildCommand(Lang.misc[63].Value, keyConfiguration.KeyStatus["MouseLeft"])) : (empty + BuildCommand(Lang.misc[61].Value, keyConfiguration.KeyStatus["MouseLeft"])));
				}
				bool showGrapple = true;
				bool flag = Main.SmartInteractProj != -1 || Main.HasInteractableObjectThatIsNotATile;
				bool flag2 = !Main.SmartInteractShowingGenuine && Main.SmartInteractShowingFake;
				if ((Main.SmartInteractShowingGenuine || Main.SmartInteractShowingFake) | flag)
				{
					if (Main.SmartInteractNPC != -1)
					{
						if (flag2)
						{
							showGrapple = false;
						}
						empty += BuildCommand(Lang.misc[80].Value, keyConfiguration.KeyStatus["MouseRight"]);
					}
					else if (flag)
					{
						if (flag2)
						{
							showGrapple = false;
						}
						empty += BuildCommand(Lang.misc[79].Value, keyConfiguration.KeyStatus["MouseRight"]);
					}
					else if (Main.SmartInteractX != -1 && Main.SmartInteractY != -1)
					{
						if (flag2)
						{
							showGrapple = false;
						}
						Tile tile = Main.tile[Main.SmartInteractX, Main.SmartInteractY];
						empty = ((!TileID.Sets.TileInteractRead[tile.type]) ? (empty + BuildCommand(Lang.misc[79].Value, keyConfiguration.KeyStatus["MouseRight"])) : (empty + BuildCommand(Lang.misc[81].Value, keyConfiguration.KeyStatus["MouseRight"])));
					}
				}
				else if (WiresUI.Settings.DrawToolModeUI)
				{
					empty += BuildCommand(Lang.misc[89].Value, keyConfiguration.KeyStatus["MouseRight"]);
				}
				else if (ItemID.Sets.HasRightFire[item.type])
				{
					empty += BuildCommand(Language.GetTextValue("UI.ActionAltAction"), keyConfiguration.KeyStatus["MouseRight"]);
				}
				if (!GrappleAndInteractAreShared || !GrappleInstructionOccupied(item, showGrapple))
				{
					if (GrappleAndInteractAreShared)
					{
						if (Main.LocalPlayer.CanTogglePanControlModeWithInteract())
						{
							empty += BuildCommand(Language.GetTextValue("UI.LookAheadGamepadInstruction"), keyConfiguration.KeyStatus["MouseRight"]);
						}
						else if (Main.LocalPlayer.QuickGrapple_GetItemToUse() != null)
						{
							empty += BuildCommand(Lang.misc[57].Value, keyConfiguration.KeyStatus["Grapple"]);
						}
					}
					else
					{
						if (Main.LocalPlayer.CanTogglePanControlModeWithInteract())
						{
							empty += BuildCommand(Language.GetTextValue("UI.LookAheadGamepadInstruction"), keyConfiguration.KeyStatus["MouseRight"]);
						}
						if (Main.LocalPlayer.QuickGrapple_GetItemToUse() != null)
						{
							empty += BuildCommand(Lang.misc[57].Value, keyConfiguration.KeyStatus["Grapple"]);
						}
					}
				}
			}
		}
		return empty;
	}

	public static bool GrappleInstructionOccupied(Item item, bool showGrapple)
	{
		if (!WiresUI.Settings.DrawToolModeUI && (!Main.SmartInteractShowingGenuine || !Main.HasSmartInteractTarget) && (!Main.SmartInteractShowingFake || showGrapple))
		{
			return ItemID.Sets.HasRightFire[item.type];
		}
		return true;
	}

	public static string BuildCommand(string CommandText, params List<string>[] Bindings)
	{
		string text = "";
		if (Bindings.Length == 0)
		{
			return text;
		}
		text += GenerateGlyphList(Bindings[0]);
		for (int i = 1; i < Bindings.Length; i++)
		{
			string text2 = GenerateGlyphList(Bindings[i]);
			if (text2.Length > 0)
			{
				text = text + "/" + text2;
			}
		}
		if (text.Length > 0)
		{
			text = text + ": " + CommandText + "   ";
		}
		return text;
	}

	public static string GenerateInputTag_ForCurrentGamemode(bool tagForGameplay, string triggerName)
	{
		if (UsingGamepad)
		{
			return GenerateGlyphList(CurrentProfile.InputModes[tagForGameplay ? InputMode.XBoxGamepad : InputMode.XBoxGamepadUI].KeyStatus[triggerName]);
		}
		return GenerateRawInputList(CurrentProfile.InputModes[InputMode.Keyboard].KeyStatus[triggerName]);
	}

	private static string LocalizeKey(string keyName)
	{
		if (keyName == "Mouse1")
		{
			return Language.GetTextValue("Controls.LeftClick");
		}
		if (keyName == "Mouse2")
		{
			return Language.GetTextValue("Controls.RightClick");
		}
		return keyName;
	}

	private static string GenerateGlyphList(List<string> list)
	{
		if (list.Count == 0)
		{
			return "";
		}
		string text = GlyphTagHandler.GenerateTag(list[0]);
		for (int i = 1; i < list.Count; i++)
		{
			text = text + "/" + GlyphTagHandler.GenerateTag(list[i]);
		}
		return text;
	}

	private static string GenerateRawInputList(List<string> list)
	{
		if (list.Count == 0)
		{
			return "";
		}
		string text = LocalizeKey(list[0]);
		for (int i = 1; i < list.Count; i++)
		{
			text = text + "/" + LocalizeKey(list[i]);
		}
		return text;
	}

	public static void NavigatorCachePosition()
	{
		PreUIX = MouseX;
		PreUIY = MouseY;
	}

	public static void NavigatorUnCachePosition()
	{
		MouseX = PreUIX;
		MouseY = PreUIY;
	}

	public static void LockOnCachePosition()
	{
		PreLockOnX = MouseX;
		PreLockOnY = MouseY;
	}

	public static void LockOnUnCachePosition()
	{
		MouseX = PreLockOnX;
		MouseY = PreLockOnY;
	}

	public static void PrettyPrintProfiles(ref string text)
	{
		string[] array = text.Split(new string[1] { "\r\n" }, StringSplitOptions.None);
		foreach (string text2 in array)
		{
			if (text2.Contains(": {"))
			{
				string text3 = text2.Substring(0, text2.IndexOf('"'));
				string text4 = text2 + "\r\n  ";
				string newValue = text4.Replace(": {\r\n  ", ": \r\n" + text3 + "{\r\n  ");
				text = text.Replace(text4, newValue);
			}
		}
		text = text.Replace("[\r\n        ", "[");
		text = text.Replace("[\r\n      ", "[");
		text = text.Replace("\"\r\n      ", "\"");
		text = text.Replace("\",\r\n        ", "\", ");
		text = text.Replace("\",\r\n      ", "\", ");
		text = text.Replace("\r\n    ]", "]");
	}

	public static void PrettyPrintProfilesOld(ref string text)
	{
		text = text.Replace(": {\r\n  ", ": \r\n  {\r\n  ");
		text = text.Replace("[\r\n      ", "[");
		text = text.Replace("\"\r\n      ", "\"");
		text = text.Replace("\",\r\n      ", "\", ");
		text = text.Replace("\r\n    ]", "]");
	}

	public static void Reset(KeyConfiguration c, PresetProfiles style, InputMode mode)
	{
		//IL_0541: Unknown result type (might be due to invalid IL or missing references)
		//IL_10f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_1cfb: Unknown result type (might be due to invalid IL or missing references)
		//IL_2886: Unknown result type (might be due to invalid IL or missing references)
		switch (style)
		{
		case PresetProfiles.Redigit:
			switch (mode)
			{
			case InputMode.Keyboard:
				c.KeyStatus["MouseLeft"].Add("Mouse1");
				c.KeyStatus["MouseRight"].Add("Mouse2");
				c.KeyStatus["Up"].Add("W");
				c.KeyStatus["Down"].Add("S");
				c.KeyStatus["Left"].Add("A");
				c.KeyStatus["Right"].Add("D");
				c.KeyStatus["Jump"].Add("Space");
				c.KeyStatus["Inventory"].Add("Escape");
				c.KeyStatus["Grapple"].Add("E");
				c.KeyStatus["SmartSelect"].Add("LeftShift");
				c.KeyStatus["SmartCursor"].Add("LeftControl");
				c.KeyStatus["QuickMount"].Add("R");
				c.KeyStatus["QuickHeal"].Add("H");
				c.KeyStatus["QuickMana"].Add("J");
				c.KeyStatus["QuickBuff"].Add("B");
				c.KeyStatus["MapStyle"].Add("Tab");
				c.KeyStatus["MapFull"].Add("M");
				c.KeyStatus["MapZoomIn"].Add("Add");
				c.KeyStatus["MapZoomOut"].Add("Subtract");
				c.KeyStatus["MapAlphaUp"].Add("PageUp");
				c.KeyStatus["MapAlphaDown"].Add("PageDown");
				c.KeyStatus["Hotbar1"].Add("D1");
				c.KeyStatus["Hotbar2"].Add("D2");
				c.KeyStatus["Hotbar3"].Add("D3");
				c.KeyStatus["Hotbar4"].Add("D4");
				c.KeyStatus["Hotbar5"].Add("D5");
				c.KeyStatus["Hotbar6"].Add("D6");
				c.KeyStatus["Hotbar7"].Add("D7");
				c.KeyStatus["Hotbar8"].Add("D8");
				c.KeyStatus["Hotbar9"].Add("D9");
				c.KeyStatus["Hotbar10"].Add("D0");
				c.KeyStatus["ViewZoomOut"].Add("OemMinus");
				c.KeyStatus["ViewZoomIn"].Add("OemPlus");
				c.KeyStatus["ToggleCreativeMenu"].Add("C");
				c.KeyStatus["Loadout1"].Add("F1");
				c.KeyStatus["Loadout2"].Add("F2");
				c.KeyStatus["Loadout3"].Add("F3");
				c.KeyStatus["ToggleCameraMode"].Add("F4");
				break;
			case InputMode.KeyboardUI:
				c.KeyStatus["MouseLeft"].Add("Mouse1");
				c.KeyStatus["MouseLeft"].Add("Space");
				c.KeyStatus["MouseRight"].Add("Mouse2");
				c.KeyStatus["Up"].Add("W");
				c.KeyStatus["Up"].Add("Up");
				c.KeyStatus["Down"].Add("S");
				c.KeyStatus["Down"].Add("Down");
				c.KeyStatus["Left"].Add("A");
				c.KeyStatus["Left"].Add("Left");
				c.KeyStatus["Right"].Add("D");
				c.KeyStatus["Right"].Add("Right");
				c.KeyStatus["Inventory"].Add(((object)(Keys)27/*cast due to constrained. prefix*/).ToString());
				c.KeyStatus["MenuUp"].Add(string.Concat((object)(Buttons)1));
				c.KeyStatus["MenuDown"].Add(string.Concat((object)(Buttons)2));
				c.KeyStatus["MenuLeft"].Add(string.Concat((object)(Buttons)4));
				c.KeyStatus["MenuRight"].Add(string.Concat((object)(Buttons)8));
				break;
			case InputMode.XBoxGamepad:
				c.KeyStatus["MouseLeft"].Add(string.Concat((object)(Buttons)4194304));
				c.KeyStatus["MouseRight"].Add(string.Concat((object)(Buttons)8192));
				c.KeyStatus["Up"].Add(string.Concat((object)(Buttons)268435456));
				c.KeyStatus["Down"].Add(string.Concat((object)(Buttons)536870912));
				c.KeyStatus["Left"].Add(string.Concat((object)(Buttons)2097152));
				c.KeyStatus["Right"].Add(string.Concat((object)(Buttons)1073741824));
				c.KeyStatus["Jump"].Add(string.Concat((object)(Buttons)8388608));
				c.KeyStatus["Inventory"].Add(string.Concat((object)(Buttons)32768));
				c.KeyStatus["Grapple"].Add(string.Concat((object)(Buttons)8192));
				c.KeyStatus["LockOn"].Add(string.Concat((object)(Buttons)16384));
				c.KeyStatus["QuickMount"].Add(string.Concat((object)(Buttons)4096));
				c.KeyStatus["SmartSelect"].Add(string.Concat((object)(Buttons)128));
				c.KeyStatus["SmartCursor"].Add(string.Concat((object)(Buttons)64));
				c.KeyStatus["HotbarMinus"].Add(string.Concat((object)(Buttons)256));
				c.KeyStatus["HotbarPlus"].Add(string.Concat((object)(Buttons)512));
				c.KeyStatus["MapFull"].Add(string.Concat((object)(Buttons)16));
				c.KeyStatus["DpadSnap1"].Add(string.Concat((object)(Buttons)1));
				c.KeyStatus["DpadSnap3"].Add(string.Concat((object)(Buttons)2));
				c.KeyStatus["DpadSnap4"].Add(string.Concat((object)(Buttons)4));
				c.KeyStatus["DpadSnap2"].Add(string.Concat((object)(Buttons)8));
				c.KeyStatus["MapStyle"].Add(string.Concat((object)(Buttons)32));
				break;
			case InputMode.XBoxGamepadUI:
				c.KeyStatus["MouseLeft"].Add(string.Concat((object)(Buttons)4096));
				c.KeyStatus["MouseRight"].Add(string.Concat((object)(Buttons)256));
				c.KeyStatus["SmartCursor"].Add(string.Concat((object)(Buttons)512));
				c.KeyStatus["Up"].Add(string.Concat((object)(Buttons)268435456));
				c.KeyStatus["Down"].Add(string.Concat((object)(Buttons)536870912));
				c.KeyStatus["Left"].Add(string.Concat((object)(Buttons)2097152));
				c.KeyStatus["Right"].Add(string.Concat((object)(Buttons)1073741824));
				c.KeyStatus["Inventory"].Add(string.Concat((object)(Buttons)8192));
				c.KeyStatus["Inventory"].Add(string.Concat((object)(Buttons)32768));
				c.KeyStatus["HotbarMinus"].Add(string.Concat((object)(Buttons)8388608));
				c.KeyStatus["HotbarPlus"].Add(string.Concat((object)(Buttons)4194304));
				c.KeyStatus["Grapple"].Add(string.Concat((object)(Buttons)16384));
				c.KeyStatus["MapFull"].Add(string.Concat((object)(Buttons)16));
				c.KeyStatus["SmartSelect"].Add(string.Concat((object)(Buttons)32));
				c.KeyStatus["QuickMount"].Add(string.Concat((object)(Buttons)128));
				c.KeyStatus["DpadSnap1"].Add(string.Concat((object)(Buttons)1));
				c.KeyStatus["DpadSnap3"].Add(string.Concat((object)(Buttons)2));
				c.KeyStatus["DpadSnap4"].Add(string.Concat((object)(Buttons)4));
				c.KeyStatus["DpadSnap2"].Add(string.Concat((object)(Buttons)8));
				c.KeyStatus["MenuUp"].Add(string.Concat((object)(Buttons)1));
				c.KeyStatus["MenuDown"].Add(string.Concat((object)(Buttons)2));
				c.KeyStatus["MenuLeft"].Add(string.Concat((object)(Buttons)4));
				c.KeyStatus["MenuRight"].Add(string.Concat((object)(Buttons)8));
				break;
			case InputMode.Mouse:
				break;
			}
			break;
		case PresetProfiles.Yoraiz0r:
			switch (mode)
			{
			case InputMode.Keyboard:
				c.KeyStatus["MouseLeft"].Add("Mouse1");
				c.KeyStatus["MouseRight"].Add("Mouse2");
				c.KeyStatus["Up"].Add("W");
				c.KeyStatus["Down"].Add("S");
				c.KeyStatus["Left"].Add("A");
				c.KeyStatus["Right"].Add("D");
				c.KeyStatus["Jump"].Add("Space");
				c.KeyStatus["Inventory"].Add("Escape");
				c.KeyStatus["Grapple"].Add("E");
				c.KeyStatus["SmartSelect"].Add("LeftShift");
				c.KeyStatus["SmartCursor"].Add("LeftControl");
				c.KeyStatus["QuickMount"].Add("R");
				c.KeyStatus["QuickHeal"].Add("H");
				c.KeyStatus["QuickMana"].Add("J");
				c.KeyStatus["QuickBuff"].Add("B");
				c.KeyStatus["MapStyle"].Add("Tab");
				c.KeyStatus["MapFull"].Add("M");
				c.KeyStatus["MapZoomIn"].Add("Add");
				c.KeyStatus["MapZoomOut"].Add("Subtract");
				c.KeyStatus["MapAlphaUp"].Add("PageUp");
				c.KeyStatus["MapAlphaDown"].Add("PageDown");
				c.KeyStatus["Hotbar1"].Add("D1");
				c.KeyStatus["Hotbar2"].Add("D2");
				c.KeyStatus["Hotbar3"].Add("D3");
				c.KeyStatus["Hotbar4"].Add("D4");
				c.KeyStatus["Hotbar5"].Add("D5");
				c.KeyStatus["Hotbar6"].Add("D6");
				c.KeyStatus["Hotbar7"].Add("D7");
				c.KeyStatus["Hotbar8"].Add("D8");
				c.KeyStatus["Hotbar9"].Add("D9");
				c.KeyStatus["Hotbar10"].Add("D0");
				c.KeyStatus["ViewZoomOut"].Add("OemMinus");
				c.KeyStatus["ViewZoomIn"].Add("OemPlus");
				c.KeyStatus["ToggleCreativeMenu"].Add("C");
				c.KeyStatus["Loadout1"].Add("F1");
				c.KeyStatus["Loadout2"].Add("F2");
				c.KeyStatus["Loadout3"].Add("F3");
				c.KeyStatus["ToggleCameraMode"].Add("F4");
				break;
			case InputMode.KeyboardUI:
				c.KeyStatus["MouseLeft"].Add("Mouse1");
				c.KeyStatus["MouseLeft"].Add("Space");
				c.KeyStatus["MouseRight"].Add("Mouse2");
				c.KeyStatus["Up"].Add("W");
				c.KeyStatus["Up"].Add("Up");
				c.KeyStatus["Down"].Add("S");
				c.KeyStatus["Down"].Add("Down");
				c.KeyStatus["Left"].Add("A");
				c.KeyStatus["Left"].Add("Left");
				c.KeyStatus["Right"].Add("D");
				c.KeyStatus["Right"].Add("Right");
				c.KeyStatus["Inventory"].Add(((object)(Keys)27/*cast due to constrained. prefix*/).ToString());
				c.KeyStatus["MenuUp"].Add(string.Concat((object)(Buttons)1));
				c.KeyStatus["MenuDown"].Add(string.Concat((object)(Buttons)2));
				c.KeyStatus["MenuLeft"].Add(string.Concat((object)(Buttons)4));
				c.KeyStatus["MenuRight"].Add(string.Concat((object)(Buttons)8));
				break;
			case InputMode.XBoxGamepad:
				c.KeyStatus["MouseLeft"].Add(string.Concat((object)(Buttons)4194304));
				c.KeyStatus["MouseRight"].Add(string.Concat((object)(Buttons)8192));
				c.KeyStatus["Up"].Add(string.Concat((object)(Buttons)268435456));
				c.KeyStatus["Down"].Add(string.Concat((object)(Buttons)536870912));
				c.KeyStatus["Left"].Add(string.Concat((object)(Buttons)2097152));
				c.KeyStatus["Right"].Add(string.Concat((object)(Buttons)1073741824));
				c.KeyStatus["Jump"].Add(string.Concat((object)(Buttons)8388608));
				c.KeyStatus["Inventory"].Add(string.Concat((object)(Buttons)32768));
				c.KeyStatus["Grapple"].Add(string.Concat((object)(Buttons)256));
				c.KeyStatus["SmartSelect"].Add(string.Concat((object)(Buttons)64));
				c.KeyStatus["SmartCursor"].Add(string.Concat((object)(Buttons)128));
				c.KeyStatus["QuickMount"].Add(string.Concat((object)(Buttons)16384));
				c.KeyStatus["QuickHeal"].Add(string.Concat((object)(Buttons)4096));
				c.KeyStatus["RadialHotbar"].Add(string.Concat((object)(Buttons)512));
				c.KeyStatus["MapFull"].Add(string.Concat((object)(Buttons)16));
				c.KeyStatus["DpadSnap1"].Add(string.Concat((object)(Buttons)1));
				c.KeyStatus["DpadSnap3"].Add(string.Concat((object)(Buttons)2));
				c.KeyStatus["DpadSnap4"].Add(string.Concat((object)(Buttons)4));
				c.KeyStatus["DpadSnap2"].Add(string.Concat((object)(Buttons)8));
				c.KeyStatus["MapStyle"].Add(string.Concat((object)(Buttons)32));
				break;
			case InputMode.XBoxGamepadUI:
				c.KeyStatus["MouseLeft"].Add(string.Concat((object)(Buttons)4096));
				c.KeyStatus["MouseRight"].Add(string.Concat((object)(Buttons)256));
				c.KeyStatus["SmartCursor"].Add(string.Concat((object)(Buttons)512));
				c.KeyStatus["Up"].Add(string.Concat((object)(Buttons)268435456));
				c.KeyStatus["Down"].Add(string.Concat((object)(Buttons)536870912));
				c.KeyStatus["Left"].Add(string.Concat((object)(Buttons)2097152));
				c.KeyStatus["Right"].Add(string.Concat((object)(Buttons)1073741824));
				c.KeyStatus["LockOn"].Add(string.Concat((object)(Buttons)8192));
				c.KeyStatus["Inventory"].Add(string.Concat((object)(Buttons)32768));
				c.KeyStatus["HotbarMinus"].Add(string.Concat((object)(Buttons)8388608));
				c.KeyStatus["HotbarPlus"].Add(string.Concat((object)(Buttons)4194304));
				c.KeyStatus["Grapple"].Add(string.Concat((object)(Buttons)16384));
				c.KeyStatus["MapFull"].Add(string.Concat((object)(Buttons)16));
				c.KeyStatus["SmartSelect"].Add(string.Concat((object)(Buttons)32));
				c.KeyStatus["QuickMount"].Add(string.Concat((object)(Buttons)128));
				c.KeyStatus["DpadSnap1"].Add(string.Concat((object)(Buttons)1));
				c.KeyStatus["DpadSnap3"].Add(string.Concat((object)(Buttons)2));
				c.KeyStatus["DpadSnap4"].Add(string.Concat((object)(Buttons)4));
				c.KeyStatus["DpadSnap2"].Add(string.Concat((object)(Buttons)8));
				c.KeyStatus["MenuUp"].Add(string.Concat((object)(Buttons)1));
				c.KeyStatus["MenuDown"].Add(string.Concat((object)(Buttons)2));
				c.KeyStatus["MenuLeft"].Add(string.Concat((object)(Buttons)4));
				c.KeyStatus["MenuRight"].Add(string.Concat((object)(Buttons)8));
				break;
			case InputMode.Mouse:
				break;
			}
			break;
		case PresetProfiles.ConsolePS:
			switch (mode)
			{
			case InputMode.Keyboard:
				c.KeyStatus["MouseLeft"].Add("Mouse1");
				c.KeyStatus["MouseRight"].Add("Mouse2");
				c.KeyStatus["Up"].Add("W");
				c.KeyStatus["Down"].Add("S");
				c.KeyStatus["Left"].Add("A");
				c.KeyStatus["Right"].Add("D");
				c.KeyStatus["Jump"].Add("Space");
				c.KeyStatus["Inventory"].Add("Escape");
				c.KeyStatus["Grapple"].Add("E");
				c.KeyStatus["SmartSelect"].Add("LeftShift");
				c.KeyStatus["SmartCursor"].Add("LeftControl");
				c.KeyStatus["QuickMount"].Add("R");
				c.KeyStatus["QuickHeal"].Add("H");
				c.KeyStatus["QuickMana"].Add("J");
				c.KeyStatus["QuickBuff"].Add("B");
				c.KeyStatus["MapStyle"].Add("Tab");
				c.KeyStatus["MapFull"].Add("M");
				c.KeyStatus["MapZoomIn"].Add("Add");
				c.KeyStatus["MapZoomOut"].Add("Subtract");
				c.KeyStatus["MapAlphaUp"].Add("PageUp");
				c.KeyStatus["MapAlphaDown"].Add("PageDown");
				c.KeyStatus["Hotbar1"].Add("D1");
				c.KeyStatus["Hotbar2"].Add("D2");
				c.KeyStatus["Hotbar3"].Add("D3");
				c.KeyStatus["Hotbar4"].Add("D4");
				c.KeyStatus["Hotbar5"].Add("D5");
				c.KeyStatus["Hotbar6"].Add("D6");
				c.KeyStatus["Hotbar7"].Add("D7");
				c.KeyStatus["Hotbar8"].Add("D8");
				c.KeyStatus["Hotbar9"].Add("D9");
				c.KeyStatus["Hotbar10"].Add("D0");
				c.KeyStatus["ViewZoomOut"].Add("OemMinus");
				c.KeyStatus["ViewZoomIn"].Add("OemPlus");
				c.KeyStatus["ToggleCreativeMenu"].Add("C");
				c.KeyStatus["Loadout1"].Add("F1");
				c.KeyStatus["Loadout2"].Add("F2");
				c.KeyStatus["Loadout3"].Add("F3");
				c.KeyStatus["ToggleCameraMode"].Add("F4");
				break;
			case InputMode.KeyboardUI:
				c.KeyStatus["MouseLeft"].Add("Mouse1");
				c.KeyStatus["MouseLeft"].Add("Space");
				c.KeyStatus["MouseRight"].Add("Mouse2");
				c.KeyStatus["Up"].Add("W");
				c.KeyStatus["Up"].Add("Up");
				c.KeyStatus["Down"].Add("S");
				c.KeyStatus["Down"].Add("Down");
				c.KeyStatus["Left"].Add("A");
				c.KeyStatus["Left"].Add("Left");
				c.KeyStatus["Right"].Add("D");
				c.KeyStatus["Right"].Add("Right");
				c.KeyStatus["MenuUp"].Add(string.Concat((object)(Buttons)1));
				c.KeyStatus["MenuDown"].Add(string.Concat((object)(Buttons)2));
				c.KeyStatus["MenuLeft"].Add(string.Concat((object)(Buttons)4));
				c.KeyStatus["MenuRight"].Add(string.Concat((object)(Buttons)8));
				c.KeyStatus["Inventory"].Add(((object)(Keys)27/*cast due to constrained. prefix*/).ToString());
				break;
			case InputMode.XBoxGamepad:
				c.KeyStatus["MouseLeft"].Add(string.Concat((object)(Buttons)512));
				c.KeyStatus["MouseRight"].Add(string.Concat((object)(Buttons)8192));
				c.KeyStatus["Up"].Add(string.Concat((object)(Buttons)268435456));
				c.KeyStatus["Down"].Add(string.Concat((object)(Buttons)536870912));
				c.KeyStatus["Left"].Add(string.Concat((object)(Buttons)2097152));
				c.KeyStatus["Right"].Add(string.Concat((object)(Buttons)1073741824));
				c.KeyStatus["Jump"].Add(string.Concat((object)(Buttons)4096));
				c.KeyStatus["LockOn"].Add(string.Concat((object)(Buttons)16384));
				c.KeyStatus["Inventory"].Add(string.Concat((object)(Buttons)32768));
				c.KeyStatus["Grapple"].Add(string.Concat((object)(Buttons)256));
				c.KeyStatus["SmartSelect"].Add(string.Concat((object)(Buttons)64));
				c.KeyStatus["SmartCursor"].Add(string.Concat((object)(Buttons)128));
				c.KeyStatus["HotbarMinus"].Add(string.Concat((object)(Buttons)8388608));
				c.KeyStatus["HotbarPlus"].Add(string.Concat((object)(Buttons)4194304));
				c.KeyStatus["MapFull"].Add(string.Concat((object)(Buttons)16));
				c.KeyStatus["DpadRadial1"].Add(string.Concat((object)(Buttons)1));
				c.KeyStatus["DpadRadial3"].Add(string.Concat((object)(Buttons)2));
				c.KeyStatus["DpadRadial4"].Add(string.Concat((object)(Buttons)4));
				c.KeyStatus["DpadRadial2"].Add(string.Concat((object)(Buttons)8));
				c.KeyStatus["QuickMount"].Add(string.Concat((object)(Buttons)32));
				break;
			case InputMode.XBoxGamepadUI:
				c.KeyStatus["MouseLeft"].Add(string.Concat((object)(Buttons)4096));
				c.KeyStatus["MouseRight"].Add(string.Concat((object)(Buttons)256));
				c.KeyStatus["SmartCursor"].Add(string.Concat((object)(Buttons)512));
				c.KeyStatus["Up"].Add(string.Concat((object)(Buttons)268435456));
				c.KeyStatus["Down"].Add(string.Concat((object)(Buttons)536870912));
				c.KeyStatus["Left"].Add(string.Concat((object)(Buttons)2097152));
				c.KeyStatus["Right"].Add(string.Concat((object)(Buttons)1073741824));
				c.KeyStatus["Inventory"].Add(string.Concat((object)(Buttons)8192));
				c.KeyStatus["Inventory"].Add(string.Concat((object)(Buttons)32768));
				c.KeyStatus["HotbarMinus"].Add(string.Concat((object)(Buttons)8388608));
				c.KeyStatus["HotbarPlus"].Add(string.Concat((object)(Buttons)4194304));
				c.KeyStatus["Grapple"].Add(string.Concat((object)(Buttons)16384));
				c.KeyStatus["MapFull"].Add(string.Concat((object)(Buttons)16));
				c.KeyStatus["SmartSelect"].Add(string.Concat((object)(Buttons)32));
				c.KeyStatus["QuickMount"].Add(string.Concat((object)(Buttons)128));
				c.KeyStatus["DpadRadial1"].Add(string.Concat((object)(Buttons)1));
				c.KeyStatus["DpadRadial3"].Add(string.Concat((object)(Buttons)2));
				c.KeyStatus["DpadRadial4"].Add(string.Concat((object)(Buttons)4));
				c.KeyStatus["DpadRadial2"].Add(string.Concat((object)(Buttons)8));
				c.KeyStatus["MenuUp"].Add(string.Concat((object)(Buttons)1));
				c.KeyStatus["MenuDown"].Add(string.Concat((object)(Buttons)2));
				c.KeyStatus["MenuLeft"].Add(string.Concat((object)(Buttons)4));
				c.KeyStatus["MenuRight"].Add(string.Concat((object)(Buttons)8));
				break;
			case InputMode.Mouse:
				break;
			}
			break;
		case PresetProfiles.ConsoleXBox:
			switch (mode)
			{
			case InputMode.Keyboard:
				c.KeyStatus["MouseLeft"].Add("Mouse1");
				c.KeyStatus["MouseRight"].Add("Mouse2");
				c.KeyStatus["Up"].Add("W");
				c.KeyStatus["Down"].Add("S");
				c.KeyStatus["Left"].Add("A");
				c.KeyStatus["Right"].Add("D");
				c.KeyStatus["Jump"].Add("Space");
				c.KeyStatus["Inventory"].Add("Escape");
				c.KeyStatus["Grapple"].Add("E");
				c.KeyStatus["SmartSelect"].Add("LeftShift");
				c.KeyStatus["SmartCursor"].Add("LeftControl");
				c.KeyStatus["QuickMount"].Add("R");
				c.KeyStatus["QuickHeal"].Add("H");
				c.KeyStatus["QuickMana"].Add("J");
				c.KeyStatus["QuickBuff"].Add("B");
				c.KeyStatus["MapStyle"].Add("Tab");
				c.KeyStatus["MapFull"].Add("M");
				c.KeyStatus["MapZoomIn"].Add("Add");
				c.KeyStatus["MapZoomOut"].Add("Subtract");
				c.KeyStatus["MapAlphaUp"].Add("PageUp");
				c.KeyStatus["MapAlphaDown"].Add("PageDown");
				c.KeyStatus["Hotbar1"].Add("D1");
				c.KeyStatus["Hotbar2"].Add("D2");
				c.KeyStatus["Hotbar3"].Add("D3");
				c.KeyStatus["Hotbar4"].Add("D4");
				c.KeyStatus["Hotbar5"].Add("D5");
				c.KeyStatus["Hotbar6"].Add("D6");
				c.KeyStatus["Hotbar7"].Add("D7");
				c.KeyStatus["Hotbar8"].Add("D8");
				c.KeyStatus["Hotbar9"].Add("D9");
				c.KeyStatus["Hotbar10"].Add("D0");
				c.KeyStatus["ViewZoomOut"].Add("OemMinus");
				c.KeyStatus["ViewZoomIn"].Add("OemPlus");
				c.KeyStatus["ToggleCreativeMenu"].Add("C");
				c.KeyStatus["Loadout1"].Add("F1");
				c.KeyStatus["Loadout2"].Add("F2");
				c.KeyStatus["Loadout3"].Add("F3");
				c.KeyStatus["ToggleCameraMode"].Add("F4");
				break;
			case InputMode.KeyboardUI:
				c.KeyStatus["MouseLeft"].Add("Mouse1");
				c.KeyStatus["MouseLeft"].Add("Space");
				c.KeyStatus["MouseRight"].Add("Mouse2");
				c.KeyStatus["Up"].Add("W");
				c.KeyStatus["Up"].Add("Up");
				c.KeyStatus["Down"].Add("S");
				c.KeyStatus["Down"].Add("Down");
				c.KeyStatus["Left"].Add("A");
				c.KeyStatus["Left"].Add("Left");
				c.KeyStatus["Right"].Add("D");
				c.KeyStatus["Right"].Add("Right");
				c.KeyStatus["MenuUp"].Add(string.Concat((object)(Buttons)1));
				c.KeyStatus["MenuDown"].Add(string.Concat((object)(Buttons)2));
				c.KeyStatus["MenuLeft"].Add(string.Concat((object)(Buttons)4));
				c.KeyStatus["MenuRight"].Add(string.Concat((object)(Buttons)8));
				c.KeyStatus["Inventory"].Add(((object)(Keys)27/*cast due to constrained. prefix*/).ToString());
				break;
			case InputMode.XBoxGamepad:
				c.KeyStatus["MouseLeft"].Add(string.Concat((object)(Buttons)4194304));
				c.KeyStatus["MouseRight"].Add(string.Concat((object)(Buttons)8192));
				c.KeyStatus["Up"].Add(string.Concat((object)(Buttons)268435456));
				c.KeyStatus["Down"].Add(string.Concat((object)(Buttons)536870912));
				c.KeyStatus["Left"].Add(string.Concat((object)(Buttons)2097152));
				c.KeyStatus["Right"].Add(string.Concat((object)(Buttons)1073741824));
				c.KeyStatus["Jump"].Add(string.Concat((object)(Buttons)4096));
				c.KeyStatus["LockOn"].Add(string.Concat((object)(Buttons)16384));
				c.KeyStatus["Inventory"].Add(string.Concat((object)(Buttons)32768));
				c.KeyStatus["Grapple"].Add(string.Concat((object)(Buttons)8388608));
				c.KeyStatus["SmartSelect"].Add(string.Concat((object)(Buttons)64));
				c.KeyStatus["SmartCursor"].Add(string.Concat((object)(Buttons)128));
				c.KeyStatus["HotbarMinus"].Add(string.Concat((object)(Buttons)256));
				c.KeyStatus["HotbarPlus"].Add(string.Concat((object)(Buttons)512));
				c.KeyStatus["MapFull"].Add(string.Concat((object)(Buttons)16));
				c.KeyStatus["DpadRadial1"].Add(string.Concat((object)(Buttons)1));
				c.KeyStatus["DpadRadial3"].Add(string.Concat((object)(Buttons)2));
				c.KeyStatus["DpadRadial4"].Add(string.Concat((object)(Buttons)4));
				c.KeyStatus["DpadRadial2"].Add(string.Concat((object)(Buttons)8));
				c.KeyStatus["QuickMount"].Add(string.Concat((object)(Buttons)32));
				break;
			case InputMode.XBoxGamepadUI:
				c.KeyStatus["MouseLeft"].Add(string.Concat((object)(Buttons)4096));
				c.KeyStatus["MouseRight"].Add(string.Concat((object)(Buttons)256));
				c.KeyStatus["SmartCursor"].Add(string.Concat((object)(Buttons)512));
				c.KeyStatus["Up"].Add(string.Concat((object)(Buttons)268435456));
				c.KeyStatus["Down"].Add(string.Concat((object)(Buttons)536870912));
				c.KeyStatus["Left"].Add(string.Concat((object)(Buttons)2097152));
				c.KeyStatus["Right"].Add(string.Concat((object)(Buttons)1073741824));
				c.KeyStatus["Inventory"].Add(string.Concat((object)(Buttons)8192));
				c.KeyStatus["Inventory"].Add(string.Concat((object)(Buttons)32768));
				c.KeyStatus["HotbarMinus"].Add(string.Concat((object)(Buttons)8388608));
				c.KeyStatus["HotbarPlus"].Add(string.Concat((object)(Buttons)4194304));
				c.KeyStatus["Grapple"].Add(string.Concat((object)(Buttons)16384));
				c.KeyStatus["MapFull"].Add(string.Concat((object)(Buttons)16));
				c.KeyStatus["SmartSelect"].Add(string.Concat((object)(Buttons)32));
				c.KeyStatus["QuickMount"].Add(string.Concat((object)(Buttons)128));
				c.KeyStatus["DpadRadial1"].Add(string.Concat((object)(Buttons)1));
				c.KeyStatus["DpadRadial3"].Add(string.Concat((object)(Buttons)2));
				c.KeyStatus["DpadRadial4"].Add(string.Concat((object)(Buttons)4));
				c.KeyStatus["DpadRadial2"].Add(string.Concat((object)(Buttons)8));
				c.KeyStatus["MenuUp"].Add(string.Concat((object)(Buttons)1));
				c.KeyStatus["MenuDown"].Add(string.Concat((object)(Buttons)2));
				c.KeyStatus["MenuLeft"].Add(string.Concat((object)(Buttons)4));
				c.KeyStatus["MenuRight"].Add(string.Concat((object)(Buttons)8));
				break;
			case InputMode.Mouse:
				break;
			}
			break;
		}
	}

	public static void SetZoom_UI()
	{
		SetZoom_Scaled(1f / Main.UIScale);
	}

	public static void SetZoom_Background()
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		SetZoom_Scaled(1f / Main.BackgroundViewMatrix.RenderZoom.X);
	}

	public static void SetZoom_World()
	{
		SetZoom_Unscaled();
		SetZoom_MouseInWorld();
	}

	public static void SetZoom_Unscaled()
	{
		Main.lastMouseX = _originalLastMouseX;
		Main.lastMouseY = _originalLastMouseY;
		Main.mouseX = _originalMouseX;
		Main.mouseY = _originalMouseY;
		Main.screenWidth = _originalScreenWidth;
		Main.screenHeight = _originalScreenHeight;
	}

	public static void SetZoom_MouseInWorld()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		Vector2 val = Main.screenPosition + new Vector2((float)Main.screenWidth, (float)Main.screenHeight) / 2f;
		Vector2 val2 = Main.screenPosition + new Vector2((float)_originalMouseX, (float)_originalMouseY);
		Vector2 val3 = Main.screenPosition + new Vector2((float)_originalLastMouseX, (float)_originalLastMouseY);
		Vector2 val4 = val2 - val;
		Vector2 val5 = val3 - val;
		float num = 1f / Main.GameViewMatrix.RenderZoom.X;
		Vector2 val6 = val - Main.screenPosition + val4 * num;
		Main.mouseX = (int)val6.X;
		Main.mouseY = (int)val6.Y;
		Vector2 val7 = val - Main.screenPosition + val5 * num;
		Main.lastMouseX = (int)val7.X;
		Main.lastMouseY = (int)val7.Y;
	}

	private static void SetZoom_Scaled(float scale)
	{
		Main.lastMouseX = (int)((float)_originalLastMouseX * scale);
		Main.lastMouseY = (int)((float)_originalLastMouseY * scale);
		Main.mouseX = (int)((float)_originalMouseX * scale);
		Main.mouseY = (int)((float)_originalMouseY * scale);
		Main.screenWidth = (int)((float)_originalScreenWidth * scale);
		Main.screenHeight = (int)((float)_originalScreenHeight * scale);
	}

	static PlayerInput()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_031f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0324: Unknown result type (might be due to invalid IL or missing references)
		//IL_0329: Unknown result type (might be due to invalid IL or missing references)
		//IL_032e: Unknown result type (might be due to invalid IL or missing references)
	}
}
