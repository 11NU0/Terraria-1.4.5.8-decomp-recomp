using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Tile_Entities;
using Terraria.GameContent.UI;
using Terraria.GameInput;
using Terraria.Testing;

namespace Terraria.UI.Gamepad;

public class UILinkPointNavigator
{
	public static class Shortcuts
	{
		public static int NPCS_IconsPerColumn = 100;

		public static int NPCS_IconsTotal = 0;

		public static int NPCS_HoveredBanner = -2;

		public static int NPCS_SelectedNPC = -2;

		public static bool NPCS_IconsDisplay = false;

		public static int CRAFT_IconsPerRow = 100;

		public static int CRAFT_IconsPerColumn = 100;

		public static int CRAFT_CurrentIngredientsCount = 0;

		public static int CRAFT_CurrentRecipeBig = 0;

		public static int CRAFT_CurrentRecipeSmall = 0;

		public static int NewCraftingUI_MaterialIndex = 0;

		public static bool NPCCHAT_ButtonsNew = false;

		public static int NPCCHAT_ButtonsCount = 1;

		public static bool NPCCHAT_ButtonsLeft = false;

		public static bool NPCCHAT_ButtonsMiddle = false;

		public static bool NPCCHAT_ButtonsRight = false;

		public static bool NPCCHAT_ButtonsRight2 = false;

		public static int INGAMEOPTIONS_BUTTONS_LEFT = 0;

		public static int INGAMEOPTIONS_BUTTONS_RIGHT = 0;

		public static bool ItemSlotShouldHighlightAsSelected = false;

		public static bool ItemSlotShouldHighlightAsPreviouslySelected = false;

		public static int OPTIONS_BUTTON_SPECIALFEATURE;

		public static int BackButtonCommand;

		public static bool BackButtonInUse = false;

		public static bool BackButtonLock;

		public static int FANCYUI_HIGHEST_INDEX = 1;

		public static int FANCYUI_SPECIAL_INSTRUCTIONS = 0;

		public static int INFOACCCOUNT = 0;

		public static int BUILDERACCCOUNT = 0;

		public static int BUFFS_PER_COLUMN = 0;

		public static int BUFFS_DRAWN = 0;

		public static int INV_MOVE_OPTION_CD = 0;
	}

	public static Dictionary<int, UILinkPage> Pages = new Dictionary<int, UILinkPage>();

	public static Dictionary<int, UILinkPoint> Points = new Dictionary<int, UILinkPoint>();

	public static int CurrentPage = 1000;

	public static int OldPage = 1000;

	private static int XCooldown;

	private static int YCooldown;

	private static Vector2 LastInput;

	private static int PageLeftCD;

	private static int PageRightCD;

	public static bool InUse;

	public static int OverridePoint = -1;

	private static int? _suggestedPointID;

	private static int? _preSuggestionPoint;

	private static HashSet<UILinkPoint> _visited = new HashSet<UILinkPoint>();

	private static Queue<UILinkPoint> _queue = new Queue<UILinkPoint>();

	public static int CurrentPoint => Pages[CurrentPage].CurrentPoint;

	public static bool Available
	{
		get
		{
			if (!Main.playerInventory && !Main.ingameOptionsWindow && Main.player[Main.myPlayer].talkNPC == -1 && Main.player[Main.myPlayer].sign == -1 && !Main.mapFullscreen && !Main.clothesWindow && !Main.MenuUI.IsVisible)
			{
				return Main.InGameUI.IsVisible;
			}
			return true;
		}
	}

	public static void SuggestUsage(int PointID)
	{
		if (Points.ContainsKey(PointID))
		{
			_suggestedPointID = PointID;
		}
	}

	public static void ConsumeSuggestion()
	{
		if (_suggestedPointID.HasValue)
		{
			int value = _suggestedPointID.Value;
			ClearSuggestion();
			CurrentPage = Points[value].Page;
			OverridePoint = value;
			ProcessChanges();
			PlayerInput.Triggers.Current.UsedMovementKey = true;
		}
	}

	public static void ClearSuggestion()
	{
		_suggestedPointID = null;
	}

	public static void GoToDefaultPage(int specialFlag = 0)
	{
		TileEntity tileEntity = Main.LocalPlayer.tileEntityAnchor.GetTileEntity();
		if (Main.MenuUI.IsVisible)
		{
			CurrentPage = 1004;
		}
		else if (Main.InGameUI.IsVisible || specialFlag == 1)
		{
			CurrentPage = 1004;
		}
		else if (Main.gameMenu)
		{
			CurrentPage = 1000;
		}
		else if (Main.ingameOptionsWindow)
		{
			CurrentPage = 1001;
		}
		else if (Main.CreativeMenu.Enabled)
		{
			CurrentPage = 1005;
		}
		else if (NewCraftingUI.Visible)
		{
			CurrentPage = 24;
		}
		else if (Main.hairWindow)
		{
			CurrentPage = 12;
		}
		else if (Main.clothesWindow)
		{
			CurrentPage = 15;
		}
		else if (Main.npcShop != 0)
		{
			CurrentPage = 13;
		}
		else if (Main.InGuideCraftMenu)
		{
			CurrentPage = 0;
		}
		else if (Main.InReforgeMenu)
		{
			CurrentPage = 0;
		}
		else if (Main.player[Main.myPlayer].chest != -1)
		{
			CurrentPage = 4;
		}
		else if (tileEntity is TEDisplayDoll)
		{
			CurrentPage = 20;
		}
		else if (tileEntity is TEHatRack)
		{
			CurrentPage = 21;
		}
		else if (Main.player[Main.myPlayer].talkNPC != -1 || Main.player[Main.myPlayer].sign != -1)
		{
			CurrentPage = 1003;
		}
		else
		{
			CurrentPage = 0;
		}
	}

	public static void Update()
	{
		//IL_019a: Unknown result type (might be due to invalid IL or missing references)
		//IL_019f: Unknown result type (might be due to invalid IL or missing references)
		//IL_022a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0243: Unknown result type (might be due to invalid IL or missing references)
		//IL_027a: Unknown result type (might be due to invalid IL or missing references)
		//IL_027c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0307: Unknown result type (might be due to invalid IL or missing references)
		//IL_0337: Unknown result type (might be due to invalid IL or missing references)
		//IL_0367: Unknown result type (might be due to invalid IL or missing references)
		//IL_0397: Unknown result type (might be due to invalid IL or missing references)
		//IL_0432: Unknown result type (might be due to invalid IL or missing references)
		//IL_0437: Unknown result type (might be due to invalid IL or missing references)
		//IL_0445: Unknown result type (might be due to invalid IL or missing references)
		//IL_045f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0463: Unknown result type (might be due to invalid IL or missing references)
		//IL_0468: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0471: Unknown result type (might be due to invalid IL or missing references)
		//IL_0478: Unknown result type (might be due to invalid IL or missing references)
		//IL_049a: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_048e: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b7: Unknown result type (might be due to invalid IL or missing references)
		bool inUse = InUse;
		InUse = false;
		bool flag = true;
		if (flag)
		{
			InputMode currentInputMode = PlayerInput.CurrentInputMode;
			if ((uint)currentInputMode <= 2u && !Main.gameMenu)
			{
				flag = false;
			}
		}
		if (flag && PlayerInput.NavigatorRebindingLock > 0)
		{
			flag = false;
		}
		if (flag && !Main.gameMenu && !PlayerInput.UsingGamepadUI)
		{
			flag = false;
		}
		if (flag && !Main.gameMenu && PlayerInput.InBuildingMode)
		{
			flag = false;
		}
		if (flag && !Main.gameMenu && !Available)
		{
			flag = false;
		}
		if (flag && Main.gameMenu && Main.MenuUI.IsVisible && Main.MenuUI.CurrentState != null && Main.MenuUI.CurrentState.NoGamepadSupport)
		{
			flag = false;
		}
		bool flag2 = false;
		if (!Pages.TryGetValue(CurrentPage, out var value))
		{
			flag2 = true;
		}
		else if (!value.IsValid())
		{
			flag2 = true;
		}
		if (flag2)
		{
			GoToDefaultPage();
			ProcessChanges();
			flag = false;
		}
		if (inUse != flag)
		{
			if (!flag)
			{
				value.Leave();
				GoToDefaultPage();
				ProcessChanges();
			}
			else
			{
				GoToDefaultPage();
				ProcessChanges();
				ConsumeSuggestion();
				value.Enter();
			}
			if (flag)
			{
				if (!PlayerInput.SteamDeckIsUsed || PlayerInput.PreventCursorModeSwappingToGamepad)
				{
					Main.player[Main.myPlayer].releaseInventory = false;
				}
				Main.player[Main.myPlayer].releaseUseTile = false;
				PlayerInput.LockGamepadTileUseButton = true;
			}
			if (!Main.gameMenu)
			{
				if (flag)
				{
					PlayerInput.NavigatorCachePosition();
				}
				else
				{
					PlayerInput.NavigatorUnCachePosition();
				}
			}
		}
		ClearSuggestion();
		if (!flag)
		{
			return;
		}
		InUse = true;
		OverridePoint = -1;
		if (PageLeftCD > 0)
		{
			PageLeftCD--;
		}
		if (PageRightCD > 0)
		{
			PageRightCD--;
		}
		Vector2 navigatorDirections = PlayerInput.Triggers.Current.GetNavigatorDirections();
		bool num = PlayerInput.Triggers.Current.HotbarMinus && !PlayerInput.Triggers.Current.HotbarPlus;
		bool flag3 = PlayerInput.Triggers.Current.HotbarPlus && !PlayerInput.Triggers.Current.HotbarMinus;
		if (!num)
		{
			PageLeftCD = 0;
		}
		if (!flag3)
		{
			PageRightCD = 0;
		}
		bool num2 = num && PageLeftCD == 0;
		flag3 = flag3 && PageRightCD == 0;
		if (LastInput.X != navigatorDirections.X)
		{
			XCooldown = 0;
		}
		if (LastInput.Y != navigatorDirections.Y)
		{
			YCooldown = 0;
		}
		if (XCooldown > 0)
		{
			XCooldown--;
		}
		if (YCooldown > 0)
		{
			YCooldown--;
		}
		LastInput = navigatorDirections;
		if (num2)
		{
			PageLeftCD = 16;
		}
		if (flag3)
		{
			PageRightCD = 16;
		}
		Pages[CurrentPage].Update();
		int num3 = 10;
		if (!Main.gameMenu && Main.playerInventory && !Main.ingameOptionsWindow && !Main.inFancyUI && (CurrentPage == 0 || CurrentPage == 4 || CurrentPage == 2 || CurrentPage == 1 || CurrentPage == 20 || CurrentPage == 21))
		{
			num3 = PlayerInput.CurrentProfile.InventoryMoveCD;
		}
		if (navigatorDirections.X == -1f && XCooldown == 0)
		{
			XCooldown = num3;
			Pages[CurrentPage].TravelLeft();
		}
		if (navigatorDirections.X == 1f && XCooldown == 0)
		{
			XCooldown = num3;
			Pages[CurrentPage].TravelRight();
		}
		if (navigatorDirections.Y == -1f && YCooldown == 0)
		{
			YCooldown = num3;
			Pages[CurrentPage].TravelUp();
		}
		if (navigatorDirections.Y == 1f && YCooldown == 0)
		{
			YCooldown = num3;
			Pages[CurrentPage].TravelDown();
		}
		XCooldown = (YCooldown = Math.Max(XCooldown, YCooldown));
		if (num2)
		{
			Pages[CurrentPage].SwapPageLeft();
		}
		if (flag3)
		{
			Pages[CurrentPage].SwapPageRight();
		}
		if (PlayerInput.Triggers.Current.UsedMovementKey)
		{
			Vector2 position = Points[CurrentPoint].Position;
			Vector2 val = new Vector2((float)PlayerInput.MouseX, (float)PlayerInput.MouseY);
			float num4 = 0.3f;
			if (PlayerInput.InvisibleGamepadInMenus)
			{
				num4 = 1f;
			}
			Vector2 val2 = Vector2.Lerp(val, position, num4);
			if (Main.gameMenu)
			{
				if (Math.Abs(val2.X - position.X) <= 5f)
				{
					val2.X = position.X;
				}
				if (Math.Abs(val2.Y - position.Y) <= 5f)
				{
					val2.Y = position.Y;
				}
			}
			PlayerInput.MouseX = (int)val2.X;
			PlayerInput.MouseY = (int)val2.Y;
		}
		ResetFlagsEnd();
		if (DebugOptions.DrawLinkPoints)
		{
			DrawLinks();
		}
	}

	public static void ResetFlagsEnd()
	{
		Shortcuts.OPTIONS_BUTTON_SPECIALFEATURE = 0;
		Shortcuts.BackButtonLock = false;
		Shortcuts.BackButtonCommand = 0;
	}

	public static string GetInstructions()
	{
		UILinkPage uILinkPage = Pages[CurrentPage];
		UILinkPoint uILinkPoint = Points[CurrentPoint];
		if (_suggestedPointID.HasValue)
		{
			SwapToSuggestion();
			uILinkPoint = Points[_suggestedPointID.Value];
			uILinkPage = Pages[uILinkPoint.Page];
			CurrentPage = uILinkPage.ID;
			uILinkPage.CurrentPoint = _suggestedPointID.Value;
		}
		string text = uILinkPage.SpecialInteractions();
		if ((PlayerInput.SettingsForUI.CurrentCursorMode == CursorMode.Gamepad && PlayerInput.Triggers.Current.UsedMovementKey && InUse) || _suggestedPointID.HasValue)
		{
			text += uILinkPoint.SpecialInteractions();
		}
		text += uILinkPage.SpecialInteractionsLate();
		ConsumeSuggestionSwap();
		return text;
	}

	public static void SwapToSuggestion()
	{
		_preSuggestionPoint = CurrentPoint;
	}

	public static void ConsumeSuggestionSwap()
	{
		if (_preSuggestionPoint.HasValue)
		{
			int value = _preSuggestionPoint.Value;
			CurrentPage = Points[value].Page;
			Pages[CurrentPage].CurrentPoint = value;
		}
		_preSuggestionPoint = null;
	}

	public static void ForceMovementCooldown(int time)
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		LastInput = PlayerInput.Triggers.Current.GetNavigatorDirections();
		XCooldown = time;
		YCooldown = time;
	}

	public static void SetPosition(int ID, Vector2 Position)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		Points[ID].Position = Position * Main.UIScale;
	}

	public static Vector2 GetPosition(int ID)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		Vector2 val = (Points.TryGetValue(ID, out var value) ? value.Position : Vector2.Zero);
		if (val == Vector2.Zero)
		{
			if (ID >= 180 && ID <= 184)
			{
				val = GetPosition(ID - 180 + 100);
			}
			else if (ID >= 185 && ID <= 189)
			{
				val = GetPosition(ID - 185 + 110);
			}
		}
		return val / Main.UIScale;
	}

	public static void RegisterPage(UILinkPage page, int ID, bool automatedDefault = true)
	{
		if (automatedDefault)
		{
			page.DefaultPoint = page.LinkMap.Keys.First();
		}
		page.CurrentPoint = page.DefaultPoint;
		page.ID = ID;
		Pages.Add(page.ID, page);
		foreach (KeyValuePair<int, UILinkPoint> item in page.LinkMap)
		{
			item.Value.SetPage(ID);
			Points.Add(item.Key, item.Value);
		}
	}

	public static void ChangePage(int PageID)
	{
		if (Pages.ContainsKey(PageID) && Pages[PageID].CanEnter())
		{
			SoundEngine.PlaySound(12);
			CurrentPage = PageID;
			ProcessChanges();
		}
	}

	public static void ChangePoint(int PointID)
	{
		if (Points.ContainsKey(PointID))
		{
			CurrentPage = Points[PointID].Page;
			OverridePoint = PointID;
			ProcessChanges();
		}
	}

	public static void ProcessChanges()
	{
		UILinkPage value = Pages[OldPage];
		if (OldPage != CurrentPage)
		{
			value.Leave();
			if (!Pages.TryGetValue(CurrentPage, out value))
			{
				GoToDefaultPage();
				ProcessChanges();
				OverridePoint = -1;
			}
			value.CurrentPoint = value.DefaultPoint;
			value.Enter();
			value.Update();
			OldPage = CurrentPage;
		}
		if (OverridePoint != -1 && value.LinkMap.ContainsKey(OverridePoint))
		{
			value.CurrentPoint = OverridePoint;
		}
	}

	private static void DrawLinks()
	{
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0106: Unknown result type (might be due to invalid IL or missing references)
		//IL_0116: Unknown result type (might be due to invalid IL or missing references)
		if (Points.TryGetValue(CurrentPoint, out var value))
		{
			_visited.Clear();
			_visited.Add(value);
			_queue.Clear();
			_queue.Enqueue(value);
			while (_queue.Any())
			{
				UILinkPoint uILinkPoint = _queue.Dequeue();
				DrawLink(uILinkPoint, uILinkPoint.Up, new Vector2(0f, -1f), new Color(120, 0, 20), new Color(255, 0, 255));
				DrawLink(uILinkPoint, uILinkPoint.Down, new Vector2(0f, 1f), new Color(0, 0, 255), new Color(0, 255, 255));
				DrawLink(uILinkPoint, uILinkPoint.Left, new Vector2(-1f, 0f), new Color(0, 100, 0), new Color(50, 205, 50));
				DrawLink(uILinkPoint, uILinkPoint.Right, new Vector2(1f, 0f), new Color(100, 100, 0), new Color(255, 215, 0));
			}
		}
	}

	private static void DrawLink(UILinkPoint src, int targetId, Vector2 dir, Color colorStart, Color colorEnd)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0145: Unknown result type (might be due to invalid IL or missing references)
		//IL_0146: Unknown result type (might be due to invalid IL or missing references)
		//IL_0147: Unknown result type (might be due to invalid IL or missing references)
		//IL_0148: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0104: Unknown result type (might be due to invalid IL or missing references)
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		//IL_0113: Unknown result type (might be due to invalid IL or missing references)
		//IL_011d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0122: Unknown result type (might be due to invalid IL or missing references)
		//IL_0127: Unknown result type (might be due to invalid IL or missing references)
		//IL_0128: Unknown result type (might be due to invalid IL or missing references)
		//IL_0129: Unknown result type (might be due to invalid IL or missing references)
		//IL_012d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0133: Unknown result type (might be due to invalid IL or missing references)
		if (Points.TryGetValue(targetId, out var value) && !(value.Position == Vector2.Zero))
		{
			if (_visited.Add(value))
			{
				_queue.Enqueue(value);
			}
			Vector2 val = dir.RotatedBy(Math.PI / 2.0);
			Vector2 val2 = src.Position / Main.UIScale + val * 2f;
			Vector2 val3 = value.Position / Main.UIScale + val * 2f;
			if (Vector2.Dot(val3 - val2, dir) < 0f)
			{
				DebugVisualizer.UI.AddLine(val2, val2 += (val + dir * 2f) * 2f * 2f, colorStart);
				DebugVisualizer.UI.AddLine(val3, val3 += (val - dir * 2f) * 2f * 2f, colorEnd);
			}
			DebugVisualizer.UI.AddLine(val2, val3, colorStart, colorEnd);
		}
	}
}
