using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Tile_Entities;
using Terraria.GameContent.UI;
using Terraria.GameContent.UI.States;
using Terraria.GameInput;
using Terraria.Localization;
using Terraria.Social;
using Terraria.UI;
using Terraria.UI.Gamepad;

namespace Terraria.Initializers;

public class UILinksInitializer
{
	public class SomeVarsForUILinkers
	{
		public static Recipe SequencedCraftingCurrent;

		public static int HairMoveCD;
	}

	private static List<string> RightStickGlyphBinding = new List<string> { "RightStickAxis" };

	public static int MainfocusRecipe
	{
		get
		{
			return Main.focusRecipe;
		}
		set
		{
			Main.focusRecipe = value;
		}
	}

	public static int MainFocusBanner
	{
		get
		{
			return Main.focusRecipe;
		}
		set
		{
			Main.focusRecipe = value;
		}
	}

	public static int MainnumAvailableRecipes
	{
		get
		{
			return Main.numAvailableRecipes;
		}
		set
		{
			Main.numAvailableRecipes = value;
		}
	}

	public static int MainnumAvailableRecipes2
	{
		get
		{
			return Main.numAvailableRecipes;
		}
		set
		{
			Main.numAvailableRecipes = value;
		}
	}

	public static bool NothingMoreImportantThanNPCChat()
	{
		if (!Main.hairWindow && Main.npcShop == 0)
		{
			return Main.player[Main.myPlayer].chest == -1;
		}
		return false;
	}

	public static float HandleSliderHorizontalInput(float currentValue, float min, float max, float deadZone = 0.2f, float sensitivity = 0.5f)
	{
		float x = PlayerInput.GamepadThumbstickLeft.X;
		x = ((!(x < 0f - deadZone) && !(x > deadZone)) ? 0f : (MathHelper.Lerp(0f, sensitivity / 60f, (Math.Abs(x) - deadZone) / (1f - deadZone)) * (float)Math.Sign(x)));
		return MathHelper.Clamp((currentValue - min) / (max - min) + x, 0f, 1f) * (max - min) + min;
	}

	public static float HandleSliderVerticalInput(float currentValue, float min, float max, float deadZone = 0.2f, float sensitivity = 0.5f)
	{
		float num = 0f - PlayerInput.GamepadThumbstickLeft.Y;
		num = ((!(num < 0f - deadZone) && !(num > deadZone)) ? 0f : (MathHelper.Lerp(0f, sensitivity / 60f, (Math.Abs(num) - deadZone) / (1f - deadZone)) * (float)Math.Sign(num)));
		return MathHelper.Clamp((currentValue - min) / (max - min) + num, 0f, 1f) * (max - min) + min;
	}

	public static bool CanExecuteInputCommand()
	{
		return PlayerInput.AllowExecutionOfGamepadInstructions;
	}

	public static void Load()
	{
		Func<string> value = () => PlayerInput.BuildCommand(Lang.misc[53].Value, PlayerInput.ProfileGamepadUI.KeyStatus["MouseLeft"]);
		UILinkPage uILinkPage = new UILinkPage();
		uILinkPage.UpdateEvent += () =>
		{
			PlayerInput.GamepadAllowScrolling = true;
		};
		for (int num = 0; num < 20; num++)
		{
			uILinkPage.LinkMap.Add(2000 + num, new UILinkPoint(2000 + num, enabled: true, -3, -4, -1, -2));
		}
		uILinkPage.OnSpecialInteracts += () => PlayerInput.BuildCommand(Lang.misc[53].Value, PlayerInput.ProfileGamepadUI.KeyStatus["MouseLeft"]) + PlayerInput.BuildCommand(Lang.misc[82].Value, PlayerInput.ProfileGamepadUI.KeyStatus["Inventory"]);
		uILinkPage.UpdateEvent += () =>
		{
			bool flag2 = PlayerInput.Triggers.JustPressed.Inventory;
			if (Main.inputTextEscape)
			{
				Main.inputTextEscape = false;
				flag2 = true;
			}
			if (CanExecuteInputCommand() & flag2)
			{
				FancyExit();
			}
			UILinkPointNavigator.Shortcuts.BackButtonInUse = flag2;
			HandleOptionsSpecials();
		};
		uILinkPage.IsValidEvent += () => Main.gameMenu && !Main.MenuUI.IsVisible;
		uILinkPage.CanEnterEvent += () => Main.gameMenu && !Main.MenuUI.IsVisible;
		UILinkPointNavigator.RegisterPage(uILinkPage, 1000);
		UILinkPage cp = new UILinkPage();
		cp.LinkMap.Add(2500, new UILinkPoint(2500, enabled: true, -3, -4, -1, -2));
		cp.LinkMap.Add(2501, new UILinkPoint(2501, enabled: true, -3, -4, -1, -2));
		cp.LinkMap.Add(2502, new UILinkPoint(2502, enabled: true, -3, -4, -1, -2));
		cp.LinkMap.Add(2503, new UILinkPoint(2503, enabled: true, -3, -4, -1, -2));
		cp.LinkMap.Add(2504, new UILinkPoint(2504, enabled: true, -3, -4, -1, -2));
		cp.LinkMap.Add(2505, new UILinkPoint(2505, enabled: true, -3, -4, -1, -2));
		cp.LinkMap.Add(2506, new UILinkPoint(2506, enabled: true, -3, -4, -1, -2));
		cp.LinkMap.Add(2507, new UILinkPoint(2507, enabled: true, -3, -4, -1, -2));
		cp.LinkMap.Add(2508, new UILinkPoint(2508, enabled: true, -3, -4, -1, -2));
		cp.LinkMap.Add(2509, new UILinkPoint(2509, enabled: true, -3, -4, -1, -2));
		cp.LinkMap.Add(2510, new UILinkPoint(2510, enabled: true, -3, -4, -1, -2));
		cp.LinkMap.Add(2511, new UILinkPoint(2511, enabled: true, -3, -4, -1, -2));
		cp.UpdateEvent += () =>
		{
			if (UILinkPointNavigator.Shortcuts.NPCCHAT_ButtonsNew)
			{
				for (int i = 0; i < UILinkPointNavigator.Shortcuts.NPCCHAT_ButtonsCount; i++)
				{
					if (i - 4 >= 0)
					{
						cp.LinkMap[2500 + i].Up = 2500 + i - 4;
					}
					else
					{
						cp.LinkMap[2500 + i].Up = -1;
					}
					if (i + 4 < UILinkPointNavigator.Shortcuts.NPCCHAT_ButtonsCount)
					{
						cp.LinkMap[2500 + i].Down = 2500 + i + 4;
					}
					else
					{
						cp.LinkMap[2500 + i].Down = -2;
					}
					cp.LinkMap[2500 + i].Left = ((i > 0) ? (2500 + i - 1) : (-3));
					cp.LinkMap[2500 + i].Right = ((i < UILinkPointNavigator.Shortcuts.NPCCHAT_ButtonsCount - 1) ? (2500 + i + 1) : (-4));
				}
			}
			else
			{
				cp.LinkMap[2501].Right = (UILinkPointNavigator.Shortcuts.NPCCHAT_ButtonsRight ? 2502 : (-4));
				if (cp.LinkMap[2501].Right == -4 && UILinkPointNavigator.Shortcuts.NPCCHAT_ButtonsRight2)
				{
					cp.LinkMap[2501].Right = 2503;
				}
				cp.LinkMap[2502].Right = (UILinkPointNavigator.Shortcuts.NPCCHAT_ButtonsRight2 ? 2503 : (-4));
				cp.LinkMap[2503].Left = (UILinkPointNavigator.Shortcuts.NPCCHAT_ButtonsRight ? 2502 : 2501);
			}
		};
		cp.OnSpecialInteracts += () => PlayerInput.BuildCommand(Lang.misc[53].Value, PlayerInput.ProfileGamepadUI.KeyStatus["MouseLeft"]) + PlayerInput.BuildCommand(Lang.misc[56].Value, PlayerInput.ProfileGamepadUI.KeyStatus["Inventory"]);
		cp.IsValidEvent += () => (Main.player[Main.myPlayer].talkNPC != -1 || Main.player[Main.myPlayer].sign != -1) && NothingMoreImportantThanNPCChat();
		cp.CanEnterEvent += () => (Main.player[Main.myPlayer].talkNPC != -1 || Main.player[Main.myPlayer].sign != -1) && NothingMoreImportantThanNPCChat();
		cp.EnterEvent += () =>
		{
			Main.player[Main.myPlayer].releaseInventory = false;
		};
		cp.LeaveEvent += () =>
		{
			Main.npcChatRelease = false;
			Main.player[Main.myPlayer].LockGamepadTileInteractions();
		};
		UILinkPointNavigator.RegisterPage(cp, 1003);
		UILinkPage cp2 = new UILinkPage();
		cp2.OnSpecialInteracts += () => PlayerInput.BuildCommand(Lang.misc[56].Value, PlayerInput.ProfileGamepadUI.KeyStatus["Inventory"]) + PlayerInput.BuildCommand(Lang.misc[64].Value, PlayerInput.ProfileGamepadUI.KeyStatus["HotbarMinus"], PlayerInput.ProfileGamepadUI.KeyStatus["HotbarPlus"]);
		Func<string> value2 = () =>
		{
			int currentPoint = UILinkPointNavigator.CurrentPoint;
			return ItemSlot.GetGamepadInstructions(Main.player[Main.myPlayer].inventory, 0, currentPoint);
		};
		Func<string> value3 = () => ItemSlot.GetGamepadInstructions(ref Main.player[Main.myPlayer].trashItem, 6);
		for (int num2 = 0; num2 <= 49; num2++)
		{
			UILinkPoint uILinkPoint = new UILinkPoint(num2, enabled: true, num2 - 1, num2 + 1, num2 - 10, num2 + 10);
			uILinkPoint.OnSpecialInteracts += value2;
			int num3 = num2;
			if (num3 < 10)
			{
				uILinkPoint.Up = -1;
			}
			if (num3 >= 40)
			{
				uILinkPoint.Down = -2;
			}
			if (num3 % 10 == 9)
			{
				uILinkPoint.Right = -4;
			}
			if (num3 % 10 == 0)
			{
				uILinkPoint.Left = -3;
			}
			cp2.LinkMap.Add(num2, uILinkPoint);
		}
		cp2.LinkMap[9].Right = 0;
		cp2.LinkMap[19].Right = 50;
		cp2.LinkMap[29].Right = 51;
		cp2.LinkMap[39].Right = 52;
		cp2.LinkMap[49].Right = 53;
		cp2.LinkMap[0].Left = 9;
		cp2.LinkMap[10].Left = 54;
		cp2.LinkMap[20].Left = 55;
		cp2.LinkMap[30].Left = 56;
		cp2.LinkMap[40].Left = 57;
		cp2.LinkMap.Add(300, new UILinkPoint(300, enabled: true, 309, 310, 49, -2));
		cp2.LinkMap.Add(309, new UILinkPoint(309, enabled: true, 310, 300, 302, 54));
		cp2.LinkMap.Add(310, new UILinkPoint(310, enabled: true, 300, 309, 301, 50));
		cp2.LinkMap.Add(301, new UILinkPoint(301, enabled: true, 300, 302, 53, 310));
		cp2.LinkMap.Add(302, new UILinkPoint(302, enabled: true, 301, 300, 57, 309));
		cp2.LinkMap.Add(311, new UILinkPoint(311, enabled: true, -3, -4, 40, -2));
		cp2.LinkMap[301].OnSpecialInteracts += value;
		cp2.LinkMap[302].OnSpecialInteracts += value;
		cp2.LinkMap[309].OnSpecialInteracts += value;
		cp2.LinkMap[310].OnSpecialInteracts += value;
		cp2.LinkMap[300].OnSpecialInteracts += value3;
		cp2.UpdateEvent += () =>
		{
			bool inReforgeMenu = Main.InReforgeMenu;
			bool flag2 = Main.LocalPlayer.chest != -1;
			bool flag3 = Main.npcShop != 0;
			TileEntity tileEntity = Main.LocalPlayer.tileEntityAnchor.GetTileEntity();
			bool flag4 = tileEntity is TEHatRack;
			bool flag5 = tileEntity is TEDisplayDoll;
			if (NewCraftingUI.Visible)
			{
				flag2 = false;
			}
			for (int i = 40; i <= 49; i++)
			{
				if (inReforgeMenu)
				{
					cp2.LinkMap[i].Down = ((i < 45) ? 303 : 304);
				}
				else if (flag2)
				{
					cp2.LinkMap[i].Down = 400 + i - 40;
				}
				else if (flag3)
				{
					cp2.LinkMap[i].Down = 2700 + i - 40;
				}
				else if (i == 40 && Main.IsJourneyMode && !Main.CreativeMenu.Blocked)
				{
					cp2.LinkMap[i].Down = 311;
				}
				else if (!NewCraftingUI.Visible)
				{
					cp2.LinkMap[i].Down = -2;
				}
			}
			if (flag5)
			{
				for (int j = 41; j <= 48; j++)
				{
					cp2.LinkMap[j].Down = 5100 + (int)Math.Round((double)((j - 40) * 10) / 9.0) - 1;
				}
				cp2.LinkMap[40].Down = 5118;
			}
			if (flag4)
			{
				for (int k = 44; k <= 45; k++)
				{
					cp2.LinkMap[k].Down = 5000 + k - 44;
				}
			}
			if (NewCraftingUI.Visible && Main.LocalPlayer.chest != -1)
			{
				cp2.LinkMap[49].Down = 300;
				cp2.LinkMap[300].Up = 49;
				cp2.LinkMap[300].Right = 310;
				cp2.LinkMap[310].Up = 53;
				cp2.LinkMap[309].Up = 57;
			}
			else if (flag2)
			{
				cp2.LinkMap[300].Up = 439;
				cp2.LinkMap[300].Right = 310;
				cp2.LinkMap[300].Left = 309;
				cp2.LinkMap[310].Up = ((Main.LocalPlayer.chest < -1) ? 505 : 504);
				cp2.LinkMap[309].Up = ((Main.LocalPlayer.chest < -1) ? 505 : 504);
			}
			else if (flag3)
			{
				cp2.LinkMap[300].Up = 2739;
				cp2.LinkMap[300].Right = 310;
				cp2.LinkMap[300].Left = 309;
				cp2.LinkMap[310].Up = 53;
				cp2.LinkMap[309].Up = 57;
			}
			else
			{
				cp2.LinkMap[49].Down = 300;
				cp2.LinkMap[300].Up = 49;
				cp2.LinkMap[300].Right = 301;
				if (!NewCraftingUI.Visible)
				{
					cp2.LinkMap[300].Left = 302;
				}
				cp2.LinkMap[309].Up = 302;
				cp2.LinkMap[310].Up = 301;
			}
			if (!NewCraftingUI.Visible)
			{
				cp2.LinkMap[311].Right = -1;
				cp2.LinkMap[311].Down = -1;
				cp2.LinkMap[300].Down = -1;
			}
			cp2.LinkMap[0].Left = 9;
			cp2.LinkMap[10].Left = 54;
			cp2.LinkMap[20].Left = 55;
			cp2.LinkMap[30].Left = 56;
			cp2.LinkMap[40].Left = 57;
			if (UILinkPointNavigator.Shortcuts.BUILDERACCCOUNT > 0)
			{
				cp2.LinkMap[0].Left = 6000;
			}
			if (UILinkPointNavigator.Shortcuts.BUILDERACCCOUNT > 2)
			{
				cp2.LinkMap[10].Left = 6002;
			}
			if (UILinkPointNavigator.Shortcuts.BUILDERACCCOUNT > 4)
			{
				cp2.LinkMap[20].Left = 6004;
			}
			if (UILinkPointNavigator.Shortcuts.BUILDERACCCOUNT > 6)
			{
				cp2.LinkMap[30].Left = 6006;
			}
			if (UILinkPointNavigator.Shortcuts.BUILDERACCCOUNT > 8)
			{
				cp2.LinkMap[40].Left = 6008;
			}
			cp2.PageOnLeft = 9;
			if (Main.InPipBanner)
			{
				cp2.PageOnLeft = 22;
			}
			if (Main.CreativeMenu.Enabled)
			{
				cp2.PageOnLeft = 1005;
			}
			if (NewCraftingUI.Visible)
			{
				cp2.PageOnLeft = 24;
			}
			if (Main.InReforgeMenu)
			{
				cp2.PageOnLeft = 5;
			}
			if (flag5)
			{
				cp2.PageOnLeft = 20;
			}
			if (flag4)
			{
				cp2.PageOnLeft = 21;
			}
		};
		cp2.IsValidEvent += () => Main.playerInventory;
		cp2.PageOnLeft = 9;
		cp2.PageOnRight = 2;
		UILinkPointNavigator.RegisterPage(cp2, 0);
		UILinkPage cp3 = new UILinkPage();
		cp3.OnSpecialInteracts += () => PlayerInput.BuildCommand(Lang.misc[56].Value, PlayerInput.ProfileGamepadUI.KeyStatus["Inventory"]) + PlayerInput.BuildCommand(Lang.misc[64].Value, PlayerInput.ProfileGamepadUI.KeyStatus["HotbarMinus"], PlayerInput.ProfileGamepadUI.KeyStatus["HotbarPlus"]);
		Func<string> value4 = () =>
		{
			int currentPoint = UILinkPointNavigator.CurrentPoint;
			return ItemSlot.GetGamepadInstructions(Main.player[Main.myPlayer].inventory, 1, currentPoint);
		};
		for (int num4 = 50; num4 <= 53; num4++)
		{
			UILinkPoint uILinkPoint2 = new UILinkPoint(num4, enabled: true, -3, -4, num4 - 1, num4 + 1);
			uILinkPoint2.OnSpecialInteracts += value4;
			cp3.LinkMap.Add(num4, uILinkPoint2);
		}
		cp3.LinkMap[50].Left = 19;
		cp3.LinkMap[51].Left = 29;
		cp3.LinkMap[52].Left = 39;
		cp3.LinkMap[53].Left = 49;
		cp3.LinkMap[50].Right = 54;
		cp3.LinkMap[51].Right = 55;
		cp3.LinkMap[52].Right = 56;
		cp3.LinkMap[53].Right = 57;
		cp3.LinkMap[50].Up = 310;
		cp3.UpdateEvent += () =>
		{
			if (Main.npcShop != 0)
			{
				cp3.LinkMap[53].Down = 310;
			}
			else if (Main.player[Main.myPlayer].chest != -1)
			{
				cp3.LinkMap[53].Down = (NewCraftingUI.Visible ? 310 : 500);
			}
			else
			{
				cp3.LinkMap[53].Down = 301;
			}
		};
		cp3.IsValidEvent += () => Main.playerInventory;
		cp3.PageOnLeft = 0;
		cp3.PageOnRight = 2;
		UILinkPointNavigator.RegisterPage(cp3, 1);
		UILinkPage cp4 = new UILinkPage();
		cp4.OnSpecialInteracts += () => PlayerInput.BuildCommand(Lang.misc[56].Value, PlayerInput.ProfileGamepadUI.KeyStatus["Inventory"]) + PlayerInput.BuildCommand(Lang.misc[64].Value, PlayerInput.ProfileGamepadUI.KeyStatus["HotbarMinus"], PlayerInput.ProfileGamepadUI.KeyStatus["HotbarPlus"]);
		Func<string> value5 = () =>
		{
			int currentPoint = UILinkPointNavigator.CurrentPoint;
			return ItemSlot.GetGamepadInstructions(Main.player[Main.myPlayer].inventory, 2, currentPoint);
		};
		for (int num5 = 54; num5 <= 57; num5++)
		{
			UILinkPoint uILinkPoint3 = new UILinkPoint(num5, enabled: true, -3, -4, num5 - 1, num5 + 1);
			uILinkPoint3.OnSpecialInteracts += value5;
			cp4.LinkMap.Add(num5, uILinkPoint3);
		}
		cp4.LinkMap[54].Left = 50;
		cp4.LinkMap[55].Left = 51;
		cp4.LinkMap[56].Left = 52;
		cp4.LinkMap[57].Left = 53;
		cp4.LinkMap[54].Right = 10;
		cp4.LinkMap[55].Right = 20;
		cp4.LinkMap[56].Right = 30;
		cp4.LinkMap[57].Right = 40;
		cp4.LinkMap[54].Up = 309;
		cp4.UpdateEvent += () =>
		{
			if (Main.npcShop != 0)
			{
				cp4.LinkMap[57].Down = 309;
			}
			else if (Main.player[Main.myPlayer].chest != -1)
			{
				cp4.LinkMap[57].Down = (NewCraftingUI.Visible ? 310 : 500);
			}
			else
			{
				cp4.LinkMap[57].Down = 302;
			}
		};
		cp4.PageOnLeft = 0;
		cp4.PageOnRight = 8;
		UILinkPointNavigator.RegisterPage(cp4, 2);
		UILinkPage cp5 = new UILinkPage();
		cp5.OnSpecialInteracts += () => PlayerInput.BuildCommand(Lang.misc[56].Value, PlayerInput.ProfileGamepadUI.KeyStatus["Inventory"]) + PlayerInput.BuildCommand(Lang.misc[64].Value, PlayerInput.ProfileGamepadUI.KeyStatus["HotbarMinus"], PlayerInput.ProfileGamepadUI.KeyStatus["HotbarPlus"]);
		Func<string> value6 = () =>
		{
			int num39 = UILinkPointNavigator.CurrentPoint - 100;
			if (num39 % 10 == 8 && !Main.LocalPlayer.CanDemonHeartAccessoryBeShown())
			{
				num39++;
			}
			bool flag2 = num39 >= 10;
			int context = ((num39 % 10 >= 3) ? (flag2 ? 11 : 10) : (flag2 ? 9 : 8));
			return ItemSlot.GetGamepadInstructions(Main.LocalPlayer.armor, context, num39);
		};
		Func<string> value7 = () =>
		{
			int num39 = UILinkPointNavigator.CurrentPoint - 120;
			if (num39 % 10 == 8 && !Main.LocalPlayer.CanDemonHeartAccessoryBeShown())
			{
				num39++;
			}
			return ItemSlot.GetGamepadInstructions(Main.LocalPlayer.dye, 12, num39);
		};
		for (int num6 = 100; num6 <= 119; num6++)
		{
			UILinkPoint uILinkPoint4 = new UILinkPoint(num6, enabled: true, num6 + 10, num6 - 10, num6 - 1, num6 + 1);
			uILinkPoint4.OnSpecialInteracts += value6;
			int num7 = num6 - 100;
			if (num7 == 0)
			{
				uILinkPoint4.Up = 305;
			}
			if (num7 == 10)
			{
				uILinkPoint4.Up = 306;
			}
			if (num7 == 9 || num7 == 19)
			{
				uILinkPoint4.Down = -2;
			}
			if (num7 >= 10)
			{
				uILinkPoint4.Left = 120 + num7 % 10;
			}
			else if (num7 >= 3)
			{
				uILinkPoint4.Right = -4;
			}
			else
			{
				uILinkPoint4.Right = 312 + num7;
			}
			cp5.LinkMap.Add(num6, uILinkPoint4);
		}
		for (int num8 = 120; num8 <= 129; num8++)
		{
			UILinkPoint uILinkPoint4 = new UILinkPoint(num8, enabled: true, -3, -10 + num8, num8 - 1, num8 + 1);
			uILinkPoint4.OnSpecialInteracts += value7;
			bool flag = Main.IsSettingsButtonPushedToSide();
			int num9 = num8 - 120;
			if (num9 == 0)
			{
				uILinkPoint4.Up = 307;
			}
			if (num9 == 9)
			{
				uILinkPoint4.Down = 308;
				if (flag)
				{
					uILinkPoint4.Left = 308;
				}
				else
				{
					uILinkPoint4.Left = 1557;
				}
			}
			if (num9 == 8)
			{
				if (flag)
				{
					uILinkPoint4.Left = 1557;
				}
				else
				{
					uILinkPoint4.Left = 1570;
				}
			}
			if (flag && num9 == 7)
			{
				uILinkPoint4.Left = 1570;
			}
			cp5.LinkMap.Add(num8, uILinkPoint4);
		}
		for (int num10 = 312; num10 <= 314; num10++)
		{
			int num11 = num10 - 312;
			UILinkPoint uILinkPoint4 = new UILinkPoint(num10, enabled: true, 100 + num11, -4, num10 - 1, num10 + 1);
			if (num11 == 0)
			{
				uILinkPoint4.Up = -1;
			}
			if (num11 == 2)
			{
				uILinkPoint4.Down = -2;
			}
			uILinkPoint4.OnSpecialInteracts += value;
			cp5.LinkMap.Add(num10, uILinkPoint4);
		}
		cp5.IsValidEvent += () => Main.playerInventory && Main.EquipPage == 0;
		cp5.UpdateEvent += () =>
		{
			bool flag2 = Main.IsSettingsButtonPushedToSide();
			int num39 = 107;
			int amountOfExtraAccessorySlotsToShow = Main.player[Main.myPlayer].GetAmountOfExtraAccessorySlotsToShow();
			for (int i = 0; i < amountOfExtraAccessorySlotsToShow; i++)
			{
				cp5.LinkMap[num39 + i].Down = num39 + i + 1;
				cp5.LinkMap[num39 - 100 + 120 + i].Down = num39 - 100 + 120 + i + 1;
				cp5.LinkMap[num39 + 10 + i].Down = num39 + 10 + i + 1;
			}
			cp5.LinkMap[num39 + amountOfExtraAccessorySlotsToShow].Down = 308;
			cp5.LinkMap[num39 + 10 + amountOfExtraAccessorySlotsToShow].Down = 308;
			cp5.LinkMap[num39 - 100 + 120 + amountOfExtraAccessorySlotsToShow].Down = 308;
			int num40 = 0;
			if (flag2 && amountOfExtraAccessorySlotsToShow == 2)
			{
				num40 = 1;
			}
			for (int j = 120; j <= 129; j++)
			{
				UILinkPoint uILinkPoint16 = cp5.LinkMap[j];
				int num41 = j - 120;
				uILinkPoint16.Left = -3;
				if (num41 == num40)
				{
					uILinkPoint16.Left = (Main.ShouldPVPDraw ? 1550 : (-3));
				}
				if (num41 == 1 + num40)
				{
					uILinkPoint16.Left = (Main.ShouldTeamSelectDraw ? 1552 : (-3));
				}
				if (num41 == 2 + num40)
				{
					uILinkPoint16.Left = (Main.ShouldTeamSelectDraw ? 1556 : (-3));
				}
				if (num41 == 3 + num40)
				{
					ref int left = ref uILinkPoint16.Left;
					int num42 = ((UILinkPointNavigator.Shortcuts.INFOACCCOUNT < 1) ? (-3) : ((UILinkPointNavigator.Shortcuts.INFOACCCOUNT >= 2) ? 1559 : 1558));
					left = num42;
				}
				if (num41 == 4 + num40)
				{
					uILinkPoint16.Left = ((UILinkPointNavigator.Shortcuts.INFOACCCOUNT < 5) ? (-3) : ((UILinkPointNavigator.Shortcuts.INFOACCCOUNT >= 6) ? 1563 : 1562));
				}
				if (num41 == 5 + num40)
				{
					uILinkPoint16.Left = ((UILinkPointNavigator.Shortcuts.INFOACCCOUNT < 9) ? (-3) : ((UILinkPointNavigator.Shortcuts.INFOACCCOUNT >= 10) ? 1567 : 1566));
				}
			}
			if (flag2)
			{
				cp5.LinkMap[num39 - 100 + 120 + amountOfExtraAccessorySlotsToShow].Left = 308;
				cp5.LinkMap[num39 - 100 + 120 + amountOfExtraAccessorySlotsToShow - 1].Left = 1557;
				if (Main.HasSpaceForControllerAchievementAdvisor)
				{
					cp5.LinkMap[num39 - 100 + 120 + amountOfExtraAccessorySlotsToShow - 2].Left = 1570;
				}
			}
			else
			{
				cp5.LinkMap[num39 - 100 + 120 + amountOfExtraAccessorySlotsToShow].Left = 1557;
				if (Main.HasSpaceForControllerAchievementAdvisor)
				{
					cp5.LinkMap[num39 - 100 + 120 + amountOfExtraAccessorySlotsToShow - 1].Left = 1570;
				}
				cp5.LinkMap[num39 - 100 + 120 + amountOfExtraAccessorySlotsToShow - 2].Left = -3;
			}
		};
		cp5.PageOnLeft = 8;
		cp5.PageOnRight = 8;
		UILinkPointNavigator.RegisterPage(cp5, 3);
		UILinkPage cp6 = new UILinkPage();
		cp6.OnSpecialInteracts += () => PlayerInput.BuildCommand(Lang.misc[56].Value, PlayerInput.ProfileGamepadUI.KeyStatus["Inventory"]) + PlayerInput.BuildCommand(Lang.misc[64].Value, PlayerInput.ProfileGamepadUI.KeyStatus["HotbarMinus"], PlayerInput.ProfileGamepadUI.KeyStatus["HotbarPlus"]);
		Func<string> value8 = () =>
		{
			int slot = UILinkPointNavigator.CurrentPoint - 400;
			int context = 4;
			Item[] item = Main.player[Main.myPlayer].bank.item;
			switch (Main.player[Main.myPlayer].chest)
			{
			case -1:
				return "";
			case -3:
				item = Main.player[Main.myPlayer].bank2.item;
				break;
			case -4:
				item = Main.player[Main.myPlayer].bank3.item;
				break;
			case -5:
				item = Main.player[Main.myPlayer].bank4.item;
				context = 32;
				break;
			default:
				item = Main.chest[Main.player[Main.myPlayer].chest].item;
				context = 3;
				break;
			case -2:
				break;
			}
			return ItemSlot.GetGamepadInstructions(item, context, slot);
		};
		for (int num12 = 400; num12 <= 439; num12++)
		{
			UILinkPoint uILinkPoint5 = new UILinkPoint(num12, enabled: true, num12 - 1, num12 + 1, num12 - 10, num12 + 10);
			uILinkPoint5.OnSpecialInteracts += value8;
			int num13 = num12 - 400;
			if (num13 < 10)
			{
				uILinkPoint5.Up = 40 + num13;
			}
			if (num13 >= 30)
			{
				uILinkPoint5.Down = -2;
			}
			if (num13 % 10 == 9)
			{
				uILinkPoint5.Right = -4;
			}
			if (num13 % 10 == 0)
			{
				uILinkPoint5.Left = -3;
			}
			cp6.LinkMap.Add(num12, uILinkPoint5);
		}
		cp6.LinkMap.Add(500, new UILinkPoint(500, enabled: true, 409, -4, 53, 501));
		cp6.LinkMap.Add(501, new UILinkPoint(501, enabled: true, 419, -4, 500, 502));
		cp6.LinkMap.Add(502, new UILinkPoint(502, enabled: true, 429, -4, 501, 503));
		cp6.LinkMap.Add(503, new UILinkPoint(503, enabled: true, 439, -4, 502, 505));
		cp6.LinkMap.Add(505, new UILinkPoint(505, enabled: true, 439, -4, 503, 504));
		cp6.LinkMap.Add(504, new UILinkPoint(504, enabled: true, 439, -4, 505, 310));
		cp6.LinkMap[500].OnSpecialInteracts += value;
		cp6.LinkMap[501].OnSpecialInteracts += value;
		cp6.LinkMap[502].OnSpecialInteracts += value;
		cp6.LinkMap[503].OnSpecialInteracts += value;
		cp6.LinkMap[504].OnSpecialInteracts += value;
		cp6.LinkMap[505].OnSpecialInteracts += value;
		cp6.LinkMap[409].Right = 500;
		cp6.LinkMap[419].Right = 501;
		cp6.LinkMap[429].Right = 502;
		cp6.LinkMap[439].Right = 503;
		cp6.LinkMap[439].Down = 300;
		cp6.PageOnLeft = 0;
		cp6.PageOnRight = 0;
		cp6.DefaultPoint = 400;
		cp6.UpdateEvent += () =>
		{
			if (Main.LocalPlayer.chest < -1)
			{
				cp6.LinkMap[505].Down = 310;
			}
			else
			{
				cp6.LinkMap[505].Down = 504;
			}
		};
		UILinkPointNavigator.RegisterPage(cp6, 4, automatedDefault: false);
		cp6.IsValidEvent += () => Main.playerInventory && Main.player[Main.myPlayer].chest != -1 && !NewCraftingUI.Visible;
		UILinkPage uILinkPage2 = new UILinkPage();
		uILinkPage2.OnSpecialInteracts += () => PlayerInput.BuildCommand(Lang.misc[56].Value, PlayerInput.ProfileGamepadUI.KeyStatus["Inventory"]) + PlayerInput.BuildCommand(Lang.misc[64].Value, PlayerInput.ProfileGamepadUI.KeyStatus["HotbarMinus"], PlayerInput.ProfileGamepadUI.KeyStatus["HotbarPlus"]);
		uILinkPage2.EnterEvent += () =>
		{
			if (!Main.mouseItem.IsAir)
			{
				MoveToDisplayDollIfHighlighted();
			}
		};
		Func<string> value9 = () =>
		{
			int slot = UILinkPointNavigator.CurrentPoint - 5100;
			return (!(Main.LocalPlayer.tileEntityAnchor.GetTileEntity() is TEDisplayDoll tEDisplayDoll)) ? "" : tEDisplayDoll.GetItemGamepadInstructions(slot);
		};
		int num14;
		UILinkPoint uILinkPoint6;
		for (num14 = 5100; num14 < 5118; num14++)
		{
			uILinkPoint6 = new UILinkPoint(num14, enabled: true, num14 - 1, num14 + 1, num14 - 9, num14 + 9);
			uILinkPoint6.OnSpecialInteracts += value9;
			int num15 = num14 - 5100;
			if (num15 < 9)
			{
				uILinkPoint6.Up = 40 + (int)Math.Round((double)(num15 + 1) * 0.9);
			}
			if (num15 >= 9)
			{
				uILinkPoint6.Down = -2;
			}
			if (num15 % 9 == 8)
			{
				uILinkPoint6.Right = -4;
			}
			if (num15 % 9 == 0)
			{
				uILinkPoint6.Left = -3;
			}
			uILinkPage2.LinkMap.Add(num14, uILinkPoint6);
		}
		uILinkPoint6 = new UILinkPoint(num14, enabled: true, -3, 5100, 40, -2);
		uILinkPoint6.OnSpecialInteracts += value9;
		uILinkPage2.LinkMap.Add(num14, uILinkPoint6);
		uILinkPage2.LinkMap[5100].Left = num14;
		uILinkPage2.PageOnLeft = 0;
		uILinkPage2.PageOnRight = 0;
		uILinkPage2.DefaultPoint = 5100;
		UILinkPointNavigator.RegisterPage(uILinkPage2, 20, automatedDefault: false);
		uILinkPage2.IsValidEvent += () => Main.playerInventory && Main.LocalPlayer.tileEntityAnchor.GetTileEntity() is TEDisplayDoll;
		UILinkPage uILinkPage3 = new UILinkPage();
		uILinkPage3.OnSpecialInteracts += () => PlayerInput.BuildCommand(Lang.misc[56].Value, PlayerInput.ProfileGamepadUI.KeyStatus["Inventory"]) + PlayerInput.BuildCommand(Lang.misc[64].Value, PlayerInput.ProfileGamepadUI.KeyStatus["HotbarMinus"], PlayerInput.ProfileGamepadUI.KeyStatus["HotbarPlus"]);
		uILinkPage3.EnterEvent += () =>
		{
			if (!Main.mouseItem.IsAir)
			{
				MoveToHatRackIfHighlighted();
			}
		};
		Func<string> value10 = () =>
		{
			int slot = UILinkPointNavigator.CurrentPoint - 5000;
			return (!(Main.LocalPlayer.tileEntityAnchor.GetTileEntity() is TEHatRack tEHatRack)) ? "" : tEHatRack.GetItemGamepadInstructions(slot);
		};
		for (int num16 = 5000; num16 <= 5003; num16++)
		{
			UILinkPoint uILinkPoint7 = new UILinkPoint(num16, enabled: true, num16 - 1, num16 + 1, num16 - 2, num16 + 2);
			uILinkPoint7.OnSpecialInteracts += value10;
			int num17 = num16 - 5000;
			if (num17 < 2)
			{
				uILinkPoint7.Up = 44 + num17;
			}
			if (num17 >= 2)
			{
				uILinkPoint7.Down = -2;
			}
			if (num17 % 2 == 1)
			{
				uILinkPoint7.Right = -4;
			}
			if (num17 % 2 == 0)
			{
				uILinkPoint7.Left = -3;
			}
			uILinkPage3.LinkMap.Add(num16, uILinkPoint7);
		}
		uILinkPage3.PageOnLeft = 0;
		uILinkPage3.PageOnRight = 0;
		uILinkPage3.DefaultPoint = 5000;
		UILinkPointNavigator.RegisterPage(uILinkPage3, 21, automatedDefault: false);
		uILinkPage3.IsValidEvent += () => Main.playerInventory && Main.LocalPlayer.tileEntityAnchor.GetTileEntity() is TEHatRack;
		UILinkPage uILinkPage4 = new UILinkPage();
		uILinkPage4.OnSpecialInteracts += () => PlayerInput.BuildCommand(Lang.misc[56].Value, PlayerInput.ProfileGamepadUI.KeyStatus["Inventory"]) + PlayerInput.BuildCommand(Lang.misc[64].Value, PlayerInput.ProfileGamepadUI.KeyStatus["HotbarMinus"], PlayerInput.ProfileGamepadUI.KeyStatus["HotbarPlus"]);
		Func<string> value11 = () =>
		{
			if (Main.npcShop == 0)
			{
				return "";
			}
			int slot = UILinkPointNavigator.CurrentPoint - 2700;
			return ItemSlot.GetGamepadInstructions(Main.instance.shop[Main.npcShop].item, 15, slot);
		};
		for (int num18 = 2700; num18 <= 2739; num18++)
		{
			UILinkPoint uILinkPoint8 = new UILinkPoint(num18, enabled: true, num18 - 1, num18 + 1, num18 - 10, num18 + 10);
			uILinkPoint8.OnSpecialInteracts += value11;
			int num19 = num18 - 2700;
			if (num19 < 10)
			{
				uILinkPoint8.Up = 40 + num19;
			}
			if (num19 >= 30)
			{
				uILinkPoint8.Down = -2;
			}
			if (num19 % 10 == 9)
			{
				uILinkPoint8.Right = -4;
			}
			if (num19 % 10 == 0)
			{
				uILinkPoint8.Left = -3;
			}
			uILinkPage4.LinkMap.Add(num18, uILinkPoint8);
		}
		uILinkPage4.LinkMap[2739].Down = 300;
		uILinkPage4.PageOnLeft = 0;
		uILinkPage4.PageOnRight = 0;
		UILinkPointNavigator.RegisterPage(uILinkPage4, 13);
		uILinkPage4.IsValidEvent += () => Main.playerInventory && Main.npcShop != 0;
		UILinkPage cp7 = new UILinkPage();
		cp7.LinkMap.Add(303, new UILinkPoint(303, enabled: true, 304, 304, 40, -2));
		cp7.LinkMap.Add(304, new UILinkPoint(304, enabled: true, 303, 303, 40, -2));
		cp7.OnSpecialInteracts += () => PlayerInput.BuildCommand(Lang.misc[56].Value, PlayerInput.ProfileGamepadUI.KeyStatus["Inventory"]) + PlayerInput.BuildCommand(Lang.misc[64].Value, PlayerInput.ProfileGamepadUI.KeyStatus["HotbarMinus"], PlayerInput.ProfileGamepadUI.KeyStatus["HotbarPlus"]);
		Func<string> value12 = () => ItemSlot.GetGamepadInstructions(ref Main.reforgeItem, 5);
		cp7.LinkMap[303].OnSpecialInteracts += value12;
		cp7.LinkMap[304].OnSpecialInteracts += () => Lang.misc[53].Value;
		cp7.UpdateEvent += () =>
		{
			if (Main.reforgeItem.type > 0)
			{
				cp7.LinkMap[303].Left = (cp7.LinkMap[303].Right = 304);
			}
			else
			{
				if (UILinkPointNavigator.OverridePoint == -1 && cp7.CurrentPoint == 304)
				{
					UILinkPointNavigator.ChangePoint(303);
				}
				cp7.LinkMap[303].Left = -3;
				cp7.LinkMap[303].Right = -4;
			}
		};
		cp7.IsValidEvent += () => Main.playerInventory && Main.InReforgeMenu;
		cp7.PageOnLeft = 0;
		cp7.PageOnRight = 0;
		cp7.EnterEvent += () =>
		{
			PlayerInput.LockGamepadButtons("MouseLeft");
		};
		UILinkPointNavigator.RegisterPage(cp7, 5);
		UILinkPage cp8 = new UILinkPage();
		cp8.OnSpecialInteracts += () =>
		{
			//IL_0193: Unknown result type (might be due to invalid IL or missing references)
			//IL_0198: Unknown result type (might be due to invalid IL or missing references)
			//IL_019d: Unknown result type (might be due to invalid IL or missing references)
			//IL_01cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f4: Unknown result type (might be due to invalid IL or missing references)
			string text = PlayerInput.BuildCommand(Lang.misc[56].Value, PlayerInput.ProfileGamepadUI.KeyStatus["Inventory"]) + PlayerInput.BuildCommand(Lang.misc[64].Value, PlayerInput.ProfileGamepadUI.KeyStatus["HotbarMinus"], PlayerInput.ProfileGamepadUI.KeyStatus["HotbarPlus"]);
			if (PlayerInput.ControllerHousingCursorActive)
			{
				bool flag2 = UILinkPointNavigator.CurrentPoint == 600;
				bool flag3 = UILinkPointNavigator.Shortcuts.NPCS_HoveredBanner >= 0;
				if (flag3)
				{
					string fullName = Main.npc[UILinkPointNavigator.Shortcuts.NPCS_HoveredBanner].FullName;
					text += PlayerInput.BuildCommand(Language.GetTextValue("UI.HousingEvict", fullName), PlayerInput.ProfileGamepadUI.KeyStatus["Grapple"]);
				}
				else if (flag2)
				{
					text += PlayerInput.BuildCommand(Lang.misc[70].Value, PlayerInput.ProfileGamepadUI.KeyStatus["Grapple"]);
				}
				else if (UILinkPointNavigator.Shortcuts.NPCS_SelectedNPC >= 0)
				{
					string fullName2 = Main.npc[UILinkPointNavigator.Shortcuts.NPCS_SelectedNPC].FullName;
					text += PlayerInput.BuildCommand(Language.GetTextValue("UI.HousingAssign", fullName2), PlayerInput.ProfileGamepadUI.KeyStatus["Grapple"]);
				}
				if (CanExecuteInputCommand() && PlayerInput.Triggers.JustPressed.Grapple)
				{
					Point val = PlayerInput.HousingWorldPosition.ToTileCoordinates();
					if (flag3)
					{
						WorldGen.kickOut(UILinkPointNavigator.Shortcuts.NPCS_HoveredBanner);
						SoundEngine.PlaySound(12);
					}
					else if (flag2)
					{
						Main.instance.PerformHousingCheck(val.X, val.Y);
					}
					else if (UILinkPointNavigator.Shortcuts.NPCS_SelectedNPC >= 0)
					{
						Main.instance.TryMovingNPC(val.X, val.Y, UILinkPointNavigator.Shortcuts.NPCS_SelectedNPC);
					}
					PlayerInput.LockGamepadButtons("Grapple");
					PlayerInput.SettingsForUI.TryRevertingToMouseMode();
				}
				text += PlayerInput.BuildCommand(Language.GetTextValue("UI.HousingAim"), RightStickGlyphBinding);
			}
			return text;
		};
		for (int num20 = 600; num20 <= 650; num20++)
		{
			UILinkPoint value13 = new UILinkPoint(num20, enabled: true, num20 + 10, num20 - 10, num20 - 1, num20 + 1);
			cp8.LinkMap.Add(num20, value13);
		}
		cp8.UpdateEvent += () =>
		{
			int num39 = UILinkPointNavigator.Shortcuts.NPCS_IconsPerColumn;
			if (num39 == 0)
			{
				num39 = 100;
			}
			for (int i = 0; i < 50; i++)
			{
				cp8.LinkMap[600 + i].Up = ((i % num39 == 0) ? (-1) : (600 + i - 1));
				if (cp8.LinkMap[600 + i].Up == -1)
				{
					if (i >= num39 * 2)
					{
						cp8.LinkMap[600 + i].Up = 307;
					}
					else if (i >= num39)
					{
						cp8.LinkMap[600 + i].Up = 306;
					}
					else
					{
						cp8.LinkMap[600 + i].Up = 305;
					}
				}
				cp8.LinkMap[600 + i].Down = (((i + 1) % num39 == 0 || i == UILinkPointNavigator.Shortcuts.NPCS_IconsTotal - 1) ? 308 : (600 + i + 1));
				cp8.LinkMap[600 + i].Left = ((i < UILinkPointNavigator.Shortcuts.NPCS_IconsTotal - num39) ? (600 + i + num39) : (-3));
				cp8.LinkMap[600 + i].Right = ((i < num39) ? (-4) : (600 + i - num39));
			}
		};
		cp8.IsValidEvent += () => Main.playerInventory && Main.EquipPage == 1;
		cp8.PageOnLeft = 8;
		cp8.PageOnRight = 8;
		UILinkPointNavigator.RegisterPage(cp8, 6);
		UILinkPage cp9 = new UILinkPage();
		cp9.OnSpecialInteracts += () => PlayerInput.BuildCommand(Lang.misc[56].Value, PlayerInput.ProfileGamepadUI.KeyStatus["Inventory"]) + PlayerInput.BuildCommand(Lang.misc[64].Value, PlayerInput.ProfileGamepadUI.KeyStatus["HotbarMinus"], PlayerInput.ProfileGamepadUI.KeyStatus["HotbarPlus"]);
		Func<string> value14 = () =>
		{
			int slot = UILinkPointNavigator.CurrentPoint - 180;
			return ItemSlot.GetGamepadInstructions(Main.player[Main.myPlayer].miscEquips, 20, slot);
		};
		Func<string> value15 = () =>
		{
			int slot = UILinkPointNavigator.CurrentPoint - 180;
			return ItemSlot.GetGamepadInstructions(Main.player[Main.myPlayer].miscEquips, 19, slot);
		};
		Func<string> value16 = () =>
		{
			int slot = UILinkPointNavigator.CurrentPoint - 180;
			return ItemSlot.GetGamepadInstructions(Main.player[Main.myPlayer].miscEquips, 18, slot);
		};
		Func<string> value17 = () =>
		{
			int slot = UILinkPointNavigator.CurrentPoint - 180;
			return ItemSlot.GetGamepadInstructions(Main.player[Main.myPlayer].miscEquips, 17, slot);
		};
		Func<string> value18 = () =>
		{
			int slot = UILinkPointNavigator.CurrentPoint - 180;
			return ItemSlot.GetGamepadInstructions(Main.player[Main.myPlayer].miscEquips, 16, slot);
		};
		Func<string> value19 = () =>
		{
			int slot = UILinkPointNavigator.CurrentPoint - 185;
			return ItemSlot.GetGamepadInstructions(Main.player[Main.myPlayer].miscDyes, 33, slot);
		};
		for (int num21 = 180; num21 <= 184; num21++)
		{
			UILinkPoint uILinkPoint9 = new UILinkPoint(num21, enabled: true, 185 + num21 - 180, -4, num21 - 1, num21 + 1);
			int num22 = num21 - 180;
			if (num22 == 0)
			{
				uILinkPoint9.Up = 305;
			}
			if (num22 == 4)
			{
				uILinkPoint9.Down = 308;
			}
			cp9.LinkMap.Add(num21, uILinkPoint9);
			switch (num21)
			{
			case 180:
				uILinkPoint9.OnSpecialInteracts += value15;
				break;
			case 181:
				uILinkPoint9.OnSpecialInteracts += value14;
				break;
			case 182:
				uILinkPoint9.OnSpecialInteracts += value16;
				break;
			case 183:
				uILinkPoint9.OnSpecialInteracts += value17;
				break;
			case 184:
				uILinkPoint9.OnSpecialInteracts += value18;
				break;
			}
		}
		for (int num23 = 185; num23 <= 189; num23++)
		{
			UILinkPoint uILinkPoint9 = new UILinkPoint(num23, enabled: true, -3, -5 + num23, num23 - 1, num23 + 1);
			uILinkPoint9.OnSpecialInteracts += value19;
			int num24 = num23 - 185;
			if (num24 == 0)
			{
				uILinkPoint9.Up = 306;
			}
			if (num24 == 4)
			{
				uILinkPoint9.Down = 308;
			}
			cp9.LinkMap.Add(num23, uILinkPoint9);
		}
		cp9.UpdateEvent += () =>
		{
			cp9.LinkMap[184].Down = ((UILinkPointNavigator.Shortcuts.BUFFS_DRAWN > 0) ? 9000 : 308);
			cp9.LinkMap[189].Down = ((UILinkPointNavigator.Shortcuts.BUFFS_DRAWN > 0) ? 9000 : 308);
		};
		cp9.IsValidEvent += () => Main.playerInventory && Main.EquipPage == 2;
		cp9.PageOnLeft = 8;
		cp9.PageOnRight = 8;
		UILinkPointNavigator.RegisterPage(cp9, 7);
		UILinkPage cp10 = new UILinkPage();
		cp10.OnSpecialInteracts += () => PlayerInput.BuildCommand(Lang.misc[56].Value, PlayerInput.ProfileGamepadUI.KeyStatus["Inventory"]) + PlayerInput.BuildCommand(Lang.misc[64].Value, PlayerInput.ProfileGamepadUI.KeyStatus["HotbarMinus"], PlayerInput.ProfileGamepadUI.KeyStatus["HotbarPlus"]);
		cp10.LinkMap.Add(305, new UILinkPoint(305, enabled: true, 306, -4, 308, -2));
		cp10.LinkMap.Add(306, new UILinkPoint(306, enabled: true, 307, 305, 308, -2));
		cp10.LinkMap.Add(307, new UILinkPoint(307, enabled: true, -3, 306, 308, -2));
		cp10.LinkMap.Add(308, new UILinkPoint(308, enabled: true, -3, -4, -1, 305));
		cp10.LinkMap[305].OnSpecialInteracts += value;
		cp10.LinkMap[306].OnSpecialInteracts += value;
		cp10.LinkMap[307].OnSpecialInteracts += value;
		cp10.LinkMap[308].OnSpecialInteracts += value;
		cp10.UpdateEvent += () =>
		{
			switch (Main.EquipPage)
			{
			case 0:
				cp10.LinkMap[305].Down = 100;
				cp10.LinkMap[306].Down = 110;
				cp10.LinkMap[307].Down = 120;
				if (Main.IsSettingsButtonPushedToSide())
				{
					cp10.LinkMap[308].Up = 1557;
					cp10.LinkMap[308].Right = 128 + Main.player[Main.myPlayer].GetAmountOfExtraAccessorySlotsToShow() - 1;
				}
				else
				{
					cp10.LinkMap[308].Right = -4;
					cp10.LinkMap[308].Up = 108 + Main.player[Main.myPlayer].GetAmountOfExtraAccessorySlotsToShow() - 1;
				}
				break;
			case 1:
			{
				cp10.LinkMap[305].Down = 600;
				cp10.LinkMap[306].Down = ((UILinkPointNavigator.Shortcuts.NPCS_IconsTotal > UILinkPointNavigator.Shortcuts.NPCS_IconsPerColumn) ? (600 + UILinkPointNavigator.Shortcuts.NPCS_IconsPerColumn) : 600);
				cp10.LinkMap[307].Down = ((UILinkPointNavigator.Shortcuts.NPCS_IconsTotal > UILinkPointNavigator.Shortcuts.NPCS_IconsPerColumn * 2) ? (600 + UILinkPointNavigator.Shortcuts.NPCS_IconsPerColumn * 2) : cp10.LinkMap[306].Down);
				int num39 = UILinkPointNavigator.Shortcuts.NPCS_IconsPerColumn;
				if (num39 == 0)
				{
					num39 = 100;
				}
				if (num39 == 100)
				{
					num39 = UILinkPointNavigator.Shortcuts.NPCS_IconsTotal;
				}
				cp10.LinkMap[308].Up = 600 + num39 - 1;
				break;
			}
			case 2:
				cp10.LinkMap[305].Down = 180;
				cp10.LinkMap[306].Down = 185;
				cp10.LinkMap[307].Down = -2;
				cp10.LinkMap[308].Up = ((UILinkPointNavigator.Shortcuts.BUFFS_DRAWN > 0) ? 9000 : 184);
				break;
			}
			cp10.PageOnRight = GetCornerWrapPageIdFromRightToLeft();
		};
		cp10.EnterEvent += () =>
		{
			if (!Main.mouseItem.IsAir)
			{
				switch (Main.EquipPage)
				{
				case 0:
					if (UILinkPointNavigator.OldPage != 3)
					{
						MoveToArmorIfHighlighted();
					}
					break;
				case 2:
					if (UILinkPointNavigator.OldPage != 7)
					{
						MoveToEquipIfHighlighted();
					}
					break;
				}
			}
		};
		cp10.IsValidEvent += () => Main.playerInventory;
		cp10.PageOnLeft = 0;
		cp10.PageOnRight = 0;
		UILinkPointNavigator.RegisterPage(cp10, 8);
		UILinkPage cp11 = new UILinkPage();
		cp11.OnSpecialInteracts += () => PlayerInput.BuildCommand(Lang.misc[56].Value, PlayerInput.ProfileGamepadUI.KeyStatus["Inventory"]) + PlayerInput.BuildCommand(Lang.misc[64].Value, PlayerInput.ProfileGamepadUI.KeyStatus["HotbarMinus"], PlayerInput.ProfileGamepadUI.KeyStatus["HotbarPlus"]);
		cp11.OnSpecialInteractsLate += () => ItemSlot.GetGamepadInstructions(Main.InPipBanner ? 35 : 22);
		for (int num25 = 1500; num25 < 1550; num25++)
		{
			UILinkPoint value20 = new UILinkPoint(num25, enabled: true, num25, num25, -1, -2);
			cp11.LinkMap.Add(num25, value20);
		}
		cp11.LinkMap.Add(11001, new UILinkPoint(11001, enabled: true, 1501, 11002, -1, 11003));
		cp11.LinkMap.Add(11002, new UILinkPoint(11002, enabled: true, 11001, -4, -1, 11003));
		cp11.LinkMap.Add(11003, new UILinkPoint(11003, enabled: true, 1501, -4, 11001, 1502));
		cp11.LinkMap[1500].OnSpecialInteracts += () => ItemSlot.GetGamepadInstructions(ref Main.guideItem, 7);
		cp11.LinkMap[11001].OnSpecialInteracts += () => PlayerInput.BuildCommand(Language.GetTextValue("UI.ToggleClassicGrid"), PlayerInput.ProfileGamepadUI.KeyStatus["MouseRight"]);
		cp11.UpdateEvent += () =>
		{
			bool flag2 = MainnumAvailableRecipes > 0 || Main.InGuideCraftMenu;
			cp11.PageOnLeft = (((Player.Settings.CraftingGridControl == Player.Settings.CraftingGridMode.Classic) & flag2) ? 10 : 8);
			int num39 = UILinkPointNavigator.Shortcuts.CRAFT_CurrentIngredientsCount;
			int num40 = num39;
			if (MainnumAvailableRecipes > 0)
			{
				num40 += 2;
			}
			if (num39 < num40)
			{
				num39 = num40;
			}
			if (UILinkPointNavigator.OverridePoint == -1)
			{
				if (cp11.CurrentPoint == 11003)
				{
					if (Main.InGuideCraftMenu)
					{
						UILinkPointNavigator.ChangePoint(1501);
					}
				}
				else if (cp11.CurrentPoint != 11001)
				{
					if (cp11.CurrentPoint == 11002)
					{
						if (!Main.bannerUI.AnyAvailableBanners || Main.InGuideCraftMenu)
						{
							UILinkPointNavigator.ChangePoint(11001);
						}
					}
					else if (cp11.CurrentPoint == 1500)
					{
						if (!Main.InGuideCraftMenu)
						{
							UILinkPointNavigator.ChangePoint(1501);
						}
					}
					else if (cp11.CurrentPoint > 1500 + num39)
					{
						UILinkPointNavigator.ChangePoint(1500);
					}
				}
			}
			int num41 = (flag2 ? 1501 : (-1));
			int num42 = (flag2 ? 1502 : (-1));
			int num43 = (flag2 ? 1503 : (-1));
			bool flag3 = !Main.ChestOrShopUIVisible;
			for (int i = 1; i < num39; i++)
			{
				cp11.LinkMap[1500 + i].Left = 1500 + i - 1;
				cp11.LinkMap[1500 + i].Right = ((i == num39 - 2) ? (-4) : (1500 + i + 1));
				if (i >= 2)
				{
					ref int up = ref cp11.LinkMap[1500 + i].Up;
					int num44 = (Main.InGuideCraftMenu ? 1500 : (flag3 ? 11003 : (-1)));
					up = num44;
					cp11.LinkMap[1500 + i].Down = (flag3 ? (-1) : ((i >= 3 && Main.bannerUI.AnyAvailableBanners) ? 11002 : 11001));
				}
			}
			cp11.LinkMap[1501].Left = -3;
			if (num39 > 0)
			{
				cp11.LinkMap[1500 + num39 - 1].Right = -4;
			}
			cp11.LinkMap[1500].Down = ((num39 >= 2) ? num42 : (-2));
			cp11.LinkMap[1500].Left = ((num39 >= 1) ? num41 : (-3));
			cp11.LinkMap[1500].Up = 11001;
			cp11.LinkMap[11001].Left = (Main.InPipCrafting ? num41 : 12000);
			cp11.LinkMap[11001].Down = ((!Main.InPipCrafting) ? (-1) : (Main.InGuideCraftMenu ? 1500 : 11003));
			cp11.LinkMap[11001].Right = ((!Main.bannerUI.AnyAvailableBanners || Main.InGuideCraftMenu) ? (-1) : 11002);
			cp11.LinkMap[11001].Up = (flag3 ? (-1) : num42);
			cp11.LinkMap[11002].Down = ((!Main.InPipCrafting) ? (-1) : 11003);
			cp11.LinkMap[11002].Up = (flag3 ? (-1) : ((num39 >= 5) ? num43 : num42));
			cp11.LinkMap[11003].Down = (flag3 ? num42 : (-1));
			cp11.LinkMap[11003].Left = num41;
		};
		cp11.LinkMap[1501].OnSpecialInteracts += () => ItemSlot.GetCraftSlotGamepadInstructions();
		cp11.ReachEndEvent += (int current, int next) =>
		{
			switch (current)
			{
			case 1501:
				switch (next)
				{
				case -1:
					if (MainfocusRecipe > 0)
					{
						MainfocusRecipe--;
					}
					break;
				case -2:
					if (MainfocusRecipe < MainnumAvailableRecipes - 1)
					{
						MainfocusRecipe++;
					}
					break;
				}
				break;
			default:
				switch (next)
				{
				case -1:
					if (MainfocusRecipe > 0)
					{
						UILinkPointNavigator.ChangePoint(1501);
						MainfocusRecipe--;
					}
					break;
				case -2:
					if (MainfocusRecipe < MainnumAvailableRecipes - 1)
					{
						UILinkPointNavigator.ChangePoint(1501);
						MainfocusRecipe++;
					}
					break;
				}
				break;
			case 1500:
				break;
			}
		};
		cp11.EnterEvent += () =>
		{
			Main.PipsUseGrid = false;
			PlayerInput.LockGamepadButtons("MouseLeft");
			if (cp11.CurrentPoint == 1500 && MainnumAvailableRecipes <= 0 && !Main.InGuideCraftMenu)
			{
				cp11.CurrentPoint = 11001;
			}
		};
		cp11.CanEnterEvent += () => Main.playerInventory;
		cp11.IsValidEvent += () => Main.playerInventory;
		cp11.PageOnLeft = 8;
		cp11.PageOnRight = 0;
		UILinkPointNavigator.RegisterPage(cp11, 9);
		UILinkPage cp12 = new UILinkPage();
		cp12.OnSpecialInteracts += () => PlayerInput.BuildCommand(Lang.misc[56].Value, PlayerInput.ProfileGamepadUI.KeyStatus["Inventory"]) + PlayerInput.BuildCommand(Lang.misc[64].Value, PlayerInput.ProfileGamepadUI.KeyStatus["HotbarMinus"], PlayerInput.ProfileGamepadUI.KeyStatus["HotbarPlus"]);
		cp12.OnSpecialInteractsLate += () => ItemSlot.GetGamepadInstructions(Main.InPipBanner ? 35 : 22);
		for (int num26 = 22000; num26 < 30000; num26++)
		{
			UILinkPoint uILinkPoint10 = new UILinkPoint(num26, enabled: true, num26, num26, num26, num26);
			int IHateLambda = num26;
			uILinkPoint10.OnSpecialInteracts += () =>
			{
				string text = PlayerInput.BuildCommand(Lang.misc[73].Value, PlayerInput.ProfileGamepadUI.KeyStatus["MouseLeft"]);
				if (TryQuickCrafting(22000, IHateLambda))
				{
					text += PlayerInput.BuildCommand(Lang.misc[71].Value, PlayerInput.ProfileGamepadUI.KeyStatus["Grapple"]);
				}
				return text;
			};
			cp12.LinkMap.Add(num26, uILinkPoint10);
		}
		cp12.UpdateEvent += () =>
		{
			int num39 = UILinkPointNavigator.Shortcuts.CRAFT_IconsPerRow;
			int cRAFT_IconsPerColumn = UILinkPointNavigator.Shortcuts.CRAFT_IconsPerColumn;
			if (num39 == 0)
			{
				num39 = 100;
			}
			int num40 = num39 * cRAFT_IconsPerColumn;
			if (num40 > 8000)
			{
				num40 = 8000;
			}
			if (num40 > MainnumAvailableRecipes)
			{
				num40 = MainnumAvailableRecipes;
			}
			for (int i = 0; i < num40; i++)
			{
				cp12.LinkMap[22000 + i].Left = ((i % num39 == 0) ? (-3) : (22000 + i - 1));
				cp12.LinkMap[22000 + i].Right = (((i + 1) % num39 == 0 || i == MainnumAvailableRecipes - 1) ? (-4) : (22000 + i + 1));
				cp12.LinkMap[22000 + i].Down = ((i < num40 - num39) ? (22000 + i + num39) : (-2));
				cp12.LinkMap[22000 + i].Up = ((i < num39) ? (-1) : (22000 + i - num39));
			}
			cp12.PageOnLeft = GetCornerWrapPageIdFromLeftToRight();
		};
		cp12.ReachEndEvent += (int current, int next) =>
		{
			int cRAFT_IconsPerRow = UILinkPointNavigator.Shortcuts.CRAFT_IconsPerRow;
			switch (next)
			{
			case -1:
				Main.recStart -= cRAFT_IconsPerRow;
				if (Main.recStart < 0)
				{
					Main.recStart = 0;
				}
				break;
			case -2:
				Main.recStart += cRAFT_IconsPerRow;
				SoundEngine.PlaySound(12);
				if (Main.recStart > MainnumAvailableRecipes - cRAFT_IconsPerRow)
				{
					Main.recStart = MainnumAvailableRecipes - cRAFT_IconsPerRow;
				}
				break;
			}
		};
		cp12.EnterEvent += () =>
		{
			Main.PipsUseGrid = true;
		};
		cp12.LeaveEvent += () =>
		{
			Main.PipsUseGrid = false;
		};
		cp12.CanEnterEvent += () => Main.playerInventory && MainnumAvailableRecipes > 0;
		cp12.IsValidEvent += () => Main.playerInventory && Main.PipsUseGrid && MainnumAvailableRecipes > 0;
		cp12.PageOnLeft = 0;
		cp12.PageOnRight = 9;
		UILinkPointNavigator.RegisterPage(cp12, 10);
		UILinkPage cp13 = new UILinkPage();
		cp13.OnSpecialInteracts += () => PlayerInput.BuildCommand(Lang.misc[56].Value, PlayerInput.ProfileGamepadUI.KeyStatus["Inventory"]) + PlayerInput.BuildCommand(Lang.misc[64].Value, PlayerInput.ProfileGamepadUI.KeyStatus["HotbarMinus"], PlayerInput.ProfileGamepadUI.KeyStatus["HotbarPlus"]);
		for (int num27 = 2605; num27 < 2620; num27++)
		{
			UILinkPoint uILinkPoint11 = new UILinkPoint(num27, enabled: true, num27, num27, num27, num27);
			uILinkPoint11.OnSpecialInteracts += () => PlayerInput.BuildCommand(Lang.misc[73].Value, PlayerInput.ProfileGamepadUI.KeyStatus["MouseLeft"]);
			cp13.LinkMap.Add(num27, uILinkPoint11);
		}
		cp13.UpdateEvent += () =>
		{
			int num39 = 5;
			int num40 = 3;
			int num41 = num39 * num40;
			int count = Main.Hairstyles.AvailableHairstyles.Count;
			for (int i = 0; i < num41; i++)
			{
				cp13.LinkMap[2605 + i].Left = ((i % num39 == 0) ? (-3) : (2605 + i - 1));
				cp13.LinkMap[2605 + i].Right = (((i + 1) % num39 == 0 || i == count - 1) ? (-4) : (2605 + i + 1));
				cp13.LinkMap[2605 + i].Down = ((i < num41 - num39) ? (2605 + i + num39) : (-2));
				cp13.LinkMap[2605 + i].Up = ((i < num39) ? (-1) : (2605 + i - num39));
			}
		};
		cp13.ReachEndEvent += (int current, int next) =>
		{
			int num39 = 5;
			switch (next)
			{
			case -1:
				Main.hairStart -= num39;
				SoundEngine.PlaySound(12);
				break;
			case -2:
				Main.hairStart += num39;
				SoundEngine.PlaySound(12);
				break;
			}
		};
		cp13.CanEnterEvent += () => Main.hairWindow;
		cp13.IsValidEvent += () => Main.hairWindow;
		cp13.PageOnLeft = 12;
		cp13.PageOnRight = 12;
		UILinkPointNavigator.RegisterPage(cp13, 11);
		UILinkPage uILinkPage5 = new UILinkPage();
		uILinkPage5.OnSpecialInteracts += () => PlayerInput.BuildCommand(Lang.misc[56].Value, PlayerInput.ProfileGamepadUI.KeyStatus["Inventory"]) + PlayerInput.BuildCommand(Lang.misc[64].Value, PlayerInput.ProfileGamepadUI.KeyStatus["HotbarMinus"], PlayerInput.ProfileGamepadUI.KeyStatus["HotbarPlus"]);
		uILinkPage5.LinkMap.Add(2600, new UILinkPoint(2600, enabled: true, -3, -4, -1, 2601));
		uILinkPage5.LinkMap.Add(2601, new UILinkPoint(2601, enabled: true, -3, -4, 2600, 2602));
		uILinkPage5.LinkMap.Add(2602, new UILinkPoint(2602, enabled: true, -3, -4, 2601, 2603));
		uILinkPage5.LinkMap.Add(2603, new UILinkPoint(2603, enabled: true, -3, 2604, 2602, -2));
		uILinkPage5.LinkMap.Add(2604, new UILinkPoint(2604, enabled: true, 2603, -4, 2602, -2));
		uILinkPage5.UpdateEvent += () =>
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_0104: Unknown result type (might be due to invalid IL or missing references)
			//IL_0109: Unknown result type (might be due to invalid IL or missing references)
			//IL_010a: Unknown result type (might be due to invalid IL or missing references)
			//IL_010f: Unknown result type (might be due to invalid IL or missing references)
			Vector3 val = Main.rgbToHsl(Main.selColor);
			float interfaceDeadzoneX = PlayerInput.CurrentProfile.InterfaceDeadzoneX;
			float x = PlayerInput.GamepadThumbstickLeft.X;
			x = ((!(x < 0f - interfaceDeadzoneX) && !(x > interfaceDeadzoneX)) ? 0f : (MathHelper.Lerp(0f, 1f / 120f, (Math.Abs(x) - interfaceDeadzoneX) / (1f - interfaceDeadzoneX)) * (float)Math.Sign(x)));
			int currentPoint = UILinkPointNavigator.CurrentPoint;
			if (currentPoint == 2600)
			{
				Main.hBar = MathHelper.Clamp(Main.hBar + x, 0f, 1f);
			}
			if (currentPoint == 2601)
			{
				Main.sBar = MathHelper.Clamp(Main.sBar + x, 0f, 1f);
			}
			if (currentPoint == 2602)
			{
				Main.lBar = MathHelper.Clamp(Main.lBar + x, 0.15f, 1f);
			}
			Vector3.Clamp(val, Vector3.Zero, Vector3.One);
			if (x != 0f)
			{
				if (Main.hairWindow)
				{
					Main.player[Main.myPlayer].hairColor = (Main.selColor = Main.hslToRgb(Main.hBar, Main.sBar, Main.lBar));
				}
				SoundEngine.PlaySound(12);
			}
		};
		uILinkPage5.CanEnterEvent += () => Main.hairWindow;
		uILinkPage5.IsValidEvent += () => Main.hairWindow;
		uILinkPage5.PageOnLeft = 11;
		uILinkPage5.PageOnRight = 11;
		UILinkPointNavigator.RegisterPage(uILinkPage5, 12);
		UILinkPage cp14 = new UILinkPage();
		for (int num28 = 0; num28 < 30; num28++)
		{
			cp14.LinkMap.Add(2900 + num28, new UILinkPoint(2900 + num28, enabled: true, -3, -4, -1, -2));
			cp14.LinkMap[2900 + num28].OnSpecialInteracts += value;
		}
		cp14.OnSpecialInteracts += () => PlayerInput.BuildCommand(Lang.misc[56].Value, PlayerInput.ProfileGamepadUI.KeyStatus["Inventory"]) + PlayerInput.BuildCommand(Lang.misc[64].Value, PlayerInput.ProfileGamepadUI.KeyStatus["HotbarMinus"], PlayerInput.ProfileGamepadUI.KeyStatus["HotbarPlus"]);
		cp14.TravelEvent += () =>
		{
			if (UILinkPointNavigator.CurrentPage == cp14.ID)
			{
				int num39 = cp14.CurrentPoint - 2900;
				if (num39 < 5)
				{
					IngameOptions.category = num39;
				}
			}
		};
		cp14.UpdateEvent += () =>
		{
			int num39 = UILinkPointNavigator.Shortcuts.INGAMEOPTIONS_BUTTONS_LEFT;
			if (num39 == 0)
			{
				num39 = 5;
			}
			if (UILinkPointNavigator.OverridePoint == -1 && cp14.CurrentPoint < 2930 && cp14.CurrentPoint > 2900 + num39 - 1)
			{
				UILinkPointNavigator.ChangePoint(2900);
			}
			for (int i = 2900; i < 2900 + num39; i++)
			{
				cp14.LinkMap[i].Up = i - 1;
				cp14.LinkMap[i].Down = i + 1;
			}
			cp14.LinkMap[2900].Up = 2900 + num39 - 1;
			cp14.LinkMap[2900 + num39 - 1].Down = 2900;
			int num40 = cp14.CurrentPoint - 2900;
			if (num40 < 4 && CanExecuteInputCommand() && PlayerInput.Triggers.JustPressed.MouseLeft)
			{
				IngameOptions.category = num40;
				UILinkPointNavigator.ChangePage(1002);
			}
			int num41 = ((SocialAPI.Network != null && SocialAPI.Network.CanInvite()) ? 1 : 0);
			if (num40 == 4 + num41 && CanExecuteInputCommand() && PlayerInput.Triggers.JustPressed.MouseLeft)
			{
				UILinkPointNavigator.ChangePage(1004);
			}
		};
		cp14.EnterEvent += () =>
		{
			cp14.CurrentPoint = 2900 + IngameOptions.category;
		};
		cp14.PageOnLeft = (cp14.PageOnRight = 1002);
		cp14.IsValidEvent += () => Main.ingameOptionsWindow && !Main.InGameUI.IsVisible;
		cp14.CanEnterEvent += () => Main.ingameOptionsWindow && !Main.InGameUI.IsVisible;
		UILinkPointNavigator.RegisterPage(cp14, 1001);
		UILinkPage cp15 = new UILinkPage();
		for (int num29 = 0; num29 < 30; num29++)
		{
			cp15.LinkMap.Add(2930 + num29, new UILinkPoint(2930 + num29, enabled: true, -3, -4, -1, -2));
			cp15.LinkMap[2930 + num29].OnSpecialInteracts += value;
		}
		cp15.EnterEvent += () =>
		{
			Main.mouseLeftRelease = false;
		};
		cp15.OnSpecialInteracts += () => PlayerInput.BuildCommand(Lang.misc[56].Value, PlayerInput.ProfileGamepadUI.KeyStatus["Inventory"]) + PlayerInput.BuildCommand(Lang.misc[64].Value, PlayerInput.ProfileGamepadUI.KeyStatus["HotbarMinus"], PlayerInput.ProfileGamepadUI.KeyStatus["HotbarPlus"]);
		cp15.UpdateEvent += () =>
		{
			int num39 = UILinkPointNavigator.Shortcuts.INGAMEOPTIONS_BUTTONS_RIGHT;
			if (num39 == 0)
			{
				num39 = 5;
			}
			if (UILinkPointNavigator.OverridePoint == -1 && cp15.CurrentPoint >= 2930 && cp15.CurrentPoint > 2930 + num39 - 1)
			{
				UILinkPointNavigator.ChangePoint(2930);
			}
			for (int i = 2930; i < 2930 + num39; i++)
			{
				cp15.LinkMap[i].Up = i - 1;
				cp15.LinkMap[i].Down = i + 1;
			}
			cp15.LinkMap[2930].Up = -1;
			cp15.LinkMap[2930 + num39 - 1].Down = -2;
			HandleOptionsSpecials();
		};
		cp15.PageOnLeft = (cp15.PageOnRight = 1001);
		cp15.IsValidEvent += () => Main.ingameOptionsWindow;
		cp15.CanEnterEvent += () => Main.ingameOptionsWindow;
		UILinkPointNavigator.RegisterPage(cp15, 1002);
		UILinkPage cp16 = new UILinkPage();
		cp16.OnSpecialInteracts += () => PlayerInput.BuildCommand(Lang.misc[56].Value, PlayerInput.ProfileGamepadUI.KeyStatus["Inventory"]) + PlayerInput.BuildCommand(Lang.misc[64].Value, PlayerInput.ProfileGamepadUI.KeyStatus["HotbarMinus"], PlayerInput.ProfileGamepadUI.KeyStatus["HotbarPlus"]);
		for (int num30 = 1550; num30 < 1558; num30++)
		{
			UILinkPoint uILinkPoint12 = new UILinkPoint(num30, enabled: true, -3, -4, -1, -2);
			switch (num30)
			{
			case 1551:
			case 1553:
			case 1555:
				uILinkPoint12.Up = uILinkPoint12.ID - 2;
				uILinkPoint12.Down = uILinkPoint12.ID + 2;
				uILinkPoint12.Right = uILinkPoint12.ID + 1;
				break;
			case 1552:
			case 1554:
			case 1556:
				uILinkPoint12.Up = uILinkPoint12.ID - 2;
				uILinkPoint12.Down = uILinkPoint12.ID + 2;
				uILinkPoint12.Left = uILinkPoint12.ID - 1;
				break;
			}
			cp16.LinkMap.Add(num30, uILinkPoint12);
		}
		cp16.LinkMap[1550].Down = 1551;
		cp16.LinkMap[1550].Right = 120;
		cp16.LinkMap[1550].Up = 307;
		cp16.LinkMap[1552].Right = 121;
		cp16.LinkMap[1554].Right = 121;
		cp16.LinkMap[1555].Down = 1570;
		cp16.LinkMap[1556].Down = 1570;
		cp16.LinkMap[1556].Right = 122;
		cp16.LinkMap[1557].Up = 1570;
		cp16.LinkMap[1557].Down = 308;
		cp16.LinkMap[1557].Right = 127;
		cp16.LinkMap.Add(1570, new UILinkPoint(1570, enabled: true, -3, -4, -1, -2));
		cp16.LinkMap[1570].Up = 1555;
		cp16.LinkMap[1570].Down = 1557;
		cp16.LinkMap[1570].Right = 126;
		for (int num31 = 0; num31 < 7; num31++)
		{
			cp16.LinkMap[1550 + num31].OnSpecialInteracts += value;
		}
		cp16.UpdateEvent += () =>
		{
			cp16.LinkMap[1551].Up = (Main.ShouldPVPDraw ? 1550 : (-1));
			cp16.LinkMap[1552].Up = (Main.ShouldPVPDraw ? 1550 : (-1));
			cp16.LinkMap[1570].Up = (Main.ShouldTeamSelectDraw ? 1555 : (-1));
			int amountOfExtraAccessorySlotsToShow = Main.player[Main.myPlayer].GetAmountOfExtraAccessorySlotsToShow();
			if (Main.IsSettingsButtonPushedToSide() && amountOfExtraAccessorySlotsToShow == 2)
			{
				cp16.LinkMap[1550].Right = 121;
				cp16.LinkMap[1552].Right = 122;
				cp16.LinkMap[1554].Right = 122;
				cp16.LinkMap[1556].Right = 123;
			}
			else
			{
				cp16.LinkMap[1550].Right = 120;
				cp16.LinkMap[1552].Right = 121;
				cp16.LinkMap[1554].Right = 121;
				cp16.LinkMap[1556].Right = 122;
			}
			int iNFOACCCOUNT = UILinkPointNavigator.Shortcuts.INFOACCCOUNT;
			if (iNFOACCCOUNT > 0)
			{
				if (Main.HasSpaceForControllerAchievementAdvisor)
				{
					cp16.LinkMap[1570].Up = 1558 + (iNFOACCCOUNT - 1) / 2 * 2;
					cp16.LinkMap[1557].Up = 1570;
				}
				else
				{
					cp16.LinkMap[1557].Up = 1558 + (iNFOACCCOUNT - 1) / 2 * 2;
				}
			}
			else
			{
				cp16.LinkMap[1570].Up = (Main.ShouldTeamSelectDraw ? 1556 : (-1));
				cp16.LinkMap[1557].Up = 1570;
			}
			if (Main.ShouldTeamSelectDraw)
			{
				if (iNFOACCCOUNT >= 1)
				{
					cp16.LinkMap[1555].Down = 1558;
					cp16.LinkMap[1556].Down = 1558;
				}
				else
				{
					cp16.LinkMap[1555].Down = 1570;
					cp16.LinkMap[1556].Down = 1570;
				}
				if (iNFOACCCOUNT >= 2)
				{
					cp16.LinkMap[1556].Down = 1559;
				}
				else
				{
					cp16.LinkMap[1556].Down = (Main.HasSpaceForControllerAchievementAdvisor ? 1570 : 1557);
				}
			}
			int num39 = 107;
			int amountOfExtraAccessorySlotsToShow2 = Main.player[Main.myPlayer].GetAmountOfExtraAccessorySlotsToShow();
			if (Main.IsSettingsButtonPushedToSide())
			{
				cp16.LinkMap[1557].Right = num39 - 100 + 120 + amountOfExtraAccessorySlotsToShow2 - 1;
				cp16.LinkMap[1570].Right = num39 - 100 + 120 + amountOfExtraAccessorySlotsToShow2 - 2;
			}
			else
			{
				cp16.LinkMap[1557].Right = num39 - 100 + 120 + amountOfExtraAccessorySlotsToShow2;
				cp16.LinkMap[1570].Right = num39 - 100 + 120 + amountOfExtraAccessorySlotsToShow2 - 1;
			}
		};
		cp16.IsValidEvent += () => Main.playerInventory;
		cp16.PageOnLeft = 8;
		cp16.PageOnRight = 8;
		UILinkPointNavigator.RegisterPage(cp16, 16);
		UILinkPage cp17 = new UILinkPage();
		cp17.OnSpecialInteracts += () => PlayerInput.BuildCommand(Lang.misc[56].Value, PlayerInput.ProfileGamepadUI.KeyStatus["Inventory"]) + PlayerInput.BuildCommand(Lang.misc[64].Value, PlayerInput.ProfileGamepadUI.KeyStatus["HotbarMinus"], PlayerInput.ProfileGamepadUI.KeyStatus["HotbarPlus"]);
		for (int num32 = 1558; num32 < 1570; num32++)
		{
			UILinkPoint uILinkPoint13 = new UILinkPoint(num32, enabled: true, -3, -4, -1, -2);
			uILinkPoint13.OnSpecialInteracts += value;
			switch (num32)
			{
			case 1559:
			case 1561:
			case 1563:
				uILinkPoint13.Up = uILinkPoint13.ID - 2;
				uILinkPoint13.Down = uILinkPoint13.ID + 2;
				uILinkPoint13.Right = uILinkPoint13.ID + 1;
				break;
			case 1560:
			case 1562:
			case 1564:
				uILinkPoint13.Up = uILinkPoint13.ID - 2;
				uILinkPoint13.Down = uILinkPoint13.ID + 2;
				uILinkPoint13.Left = uILinkPoint13.ID - 1;
				break;
			}
			cp17.LinkMap.Add(num32, uILinkPoint13);
		}
		cp17.UpdateEvent += () =>
		{
			int num39 = 3;
			int amountOfExtraAccessorySlotsToShow = Main.player[Main.myPlayer].GetAmountOfExtraAccessorySlotsToShow();
			if (Main.IsSettingsButtonPushedToSide() && amountOfExtraAccessorySlotsToShow == 2)
			{
				num39 = 4;
			}
			int iNFOACCCOUNT = UILinkPointNavigator.Shortcuts.INFOACCCOUNT;
			if (UILinkPointNavigator.OverridePoint == -1 && cp17.CurrentPoint - 1558 >= iNFOACCCOUNT)
			{
				UILinkPointNavigator.ChangePoint(1558 + iNFOACCCOUNT - 1);
			}
			for (int i = 0; i < iNFOACCCOUNT; i++)
			{
				bool flag2 = i % 2 == 0;
				int num40 = i + 1558;
				ref int down = ref cp17.LinkMap[num40].Down;
				int num41 = ((i < iNFOACCCOUNT - 2) ? (num40 + 2) : (Main.HasSpaceForControllerAchievementAdvisor ? 1570 : 1557));
				down = num41;
				cp17.LinkMap[num40].Up = ((i > 1) ? (num40 - 2) : (Main.ShouldTeamSelectDraw ? (flag2 ? 1555 : 1556) : (-1)));
				cp17.LinkMap[num40].Right = ((flag2 && i + 1 < iNFOACCCOUNT) ? (num40 + 1) : (120 + num39 + i / 4));
				cp17.LinkMap[num40].Left = (flag2 ? (-3) : (num40 - 1));
			}
		};
		cp17.IsValidEvent += () => Main.playerInventory && UILinkPointNavigator.Shortcuts.INFOACCCOUNT > 0;
		cp17.PageOnLeft = 8;
		cp17.PageOnRight = 8;
		UILinkPointNavigator.RegisterPage(cp17, 17);
		UILinkPage cp18 = new UILinkPage();
		cp18.OnSpecialInteracts += () => PlayerInput.BuildCommand(Lang.misc[56].Value, PlayerInput.ProfileGamepadUI.KeyStatus["Inventory"]) + PlayerInput.BuildCommand(Lang.misc[64].Value, PlayerInput.ProfileGamepadUI.KeyStatus["HotbarMinus"], PlayerInput.ProfileGamepadUI.KeyStatus["HotbarPlus"]);
		for (int num33 = 6000; num33 < 6012; num33++)
		{
			UILinkPoint uILinkPoint14 = new UILinkPoint(num33, enabled: true, -3, -4, -1, -2);
			switch (num33)
			{
			case 6000:
				uILinkPoint14.Right = 0;
				break;
			case 6001:
			case 6002:
				uILinkPoint14.Right = 10;
				break;
			case 6003:
			case 6004:
				uILinkPoint14.Right = 20;
				break;
			case 6005:
			case 6006:
				uILinkPoint14.Right = 30;
				break;
			default:
				uILinkPoint14.Right = 40;
				break;
			}
			cp18.LinkMap.Add(num33, uILinkPoint14);
		}
		cp18.UpdateEvent += () =>
		{
			int bUILDERACCCOUNT = UILinkPointNavigator.Shortcuts.BUILDERACCCOUNT;
			if (UILinkPointNavigator.OverridePoint == -1 && cp18.CurrentPoint - 6000 >= bUILDERACCCOUNT)
			{
				UILinkPointNavigator.ChangePoint(6000 + bUILDERACCCOUNT - 1);
			}
			for (int i = 0; i < bUILDERACCCOUNT; i++)
			{
				_ = i % 2;
				int num39 = i + 6000;
				cp18.LinkMap[num39].Down = ((i < bUILDERACCCOUNT - 1) ? (num39 + 1) : (-2));
				cp18.LinkMap[num39].Up = ((i > 0) ? (num39 - 1) : (-1));
			}
		};
		cp18.IsValidEvent += () => Main.playerInventory && UILinkPointNavigator.Shortcuts.BUILDERACCCOUNT > 0;
		cp18.PageOnLeft = 8;
		cp18.PageOnRight = 8;
		UILinkPointNavigator.RegisterPage(cp18, 18);
		UILinkPage uILinkPage6 = new UILinkPage();
		uILinkPage6.OnSpecialInteracts += () => PlayerInput.BuildCommand(Lang.misc[56].Value, PlayerInput.ProfileGamepadUI.KeyStatus["Inventory"]) + PlayerInput.BuildCommand(Lang.misc[64].Value, PlayerInput.ProfileGamepadUI.KeyStatus["HotbarMinus"], PlayerInput.ProfileGamepadUI.KeyStatus["HotbarPlus"]);
		uILinkPage6.LinkMap.Add(2806, new UILinkPoint(2806, enabled: true, 2805, 2807, -1, 2808));
		uILinkPage6.LinkMap.Add(2807, new UILinkPoint(2807, enabled: true, 2806, 2810, -1, 2809));
		uILinkPage6.LinkMap.Add(2808, new UILinkPoint(2808, enabled: true, 2813, 2809, 2806, -2));
		uILinkPage6.LinkMap.Add(2809, new UILinkPoint(2809, enabled: true, 2808, 2811, 2807, -2));
		uILinkPage6.LinkMap.Add(2810, new UILinkPoint(2810, enabled: true, 2807, -4, -1, 2811));
		uILinkPage6.LinkMap.Add(2811, new UILinkPoint(2811, enabled: true, 2809, -4, 2810, -2));
		uILinkPage6.LinkMap.Add(2805, new UILinkPoint(2805, enabled: true, -3, 2806, -1, 2813));
		uILinkPage6.LinkMap.Add(2813, new UILinkPoint(2813, enabled: true, -3, 2808, 2805, -2));
		uILinkPage6.LinkMap[2806].OnSpecialInteracts += value;
		uILinkPage6.LinkMap[2807].OnSpecialInteracts += value;
		uILinkPage6.LinkMap[2808].OnSpecialInteracts += value;
		uILinkPage6.LinkMap[2809].OnSpecialInteracts += value;
		uILinkPage6.LinkMap[2805].OnSpecialInteracts += value;
		uILinkPage6.LinkMap[2813].OnSpecialInteracts += value;
		uILinkPage6.CanEnterEvent += () => Main.clothesWindow;
		uILinkPage6.IsValidEvent += () => Main.clothesWindow;
		uILinkPage6.EnterEvent += () =>
		{
			Main.player[Main.myPlayer].releaseInventory = false;
		};
		uILinkPage6.LeaveEvent += () =>
		{
			Main.player[Main.myPlayer].LockGamepadTileInteractions();
		};
		uILinkPage6.PageOnLeft = 15;
		uILinkPage6.PageOnRight = 15;
		UILinkPointNavigator.RegisterPage(uILinkPage6, 14);
		UILinkPage uILinkPage7 = new UILinkPage();
		uILinkPage7.OnSpecialInteracts += () => PlayerInput.BuildCommand(Lang.misc[56].Value, PlayerInput.ProfileGamepadUI.KeyStatus["Inventory"]) + PlayerInput.BuildCommand(Lang.misc[64].Value, PlayerInput.ProfileGamepadUI.KeyStatus["HotbarMinus"], PlayerInput.ProfileGamepadUI.KeyStatus["HotbarPlus"]);
		uILinkPage7.LinkMap.Add(2800, new UILinkPoint(2800, enabled: true, -3, -4, -1, 2801));
		uILinkPage7.LinkMap.Add(2801, new UILinkPoint(2801, enabled: true, -3, -4, 2800, 2802));
		uILinkPage7.LinkMap.Add(2802, new UILinkPoint(2802, enabled: true, -3, -4, 2801, 2812));
		uILinkPage7.LinkMap.Add(2812, new UILinkPoint(2812, enabled: true, -3, -4, 2802, 2803));
		uILinkPage7.LinkMap.Add(2803, new UILinkPoint(2803, enabled: true, -3, 2804, 2812, -2));
		uILinkPage7.LinkMap.Add(2804, new UILinkPoint(2804, enabled: true, 2803, -4, 2812, -2));
		uILinkPage7.LinkMap[2800].OnSpecialInteracts += value;
		uILinkPage7.LinkMap[2801].OnSpecialInteracts += value;
		uILinkPage7.LinkMap[2802].OnSpecialInteracts += value;
		uILinkPage7.LinkMap[2812].OnSpecialInteracts += value;
		uILinkPage7.LinkMap[2803].OnSpecialInteracts += value;
		uILinkPage7.LinkMap[2804].OnSpecialInteracts += value;
		uILinkPage7.UpdateEvent += () =>
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_0101: Unknown result type (might be due to invalid IL or missing references)
			//IL_0106: Unknown result type (might be due to invalid IL or missing references)
			//IL_010b: Unknown result type (might be due to invalid IL or missing references)
			//IL_013a: Unknown result type (might be due to invalid IL or missing references)
			//IL_013f: Unknown result type (might be due to invalid IL or missing references)
			//IL_016d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0172: Unknown result type (might be due to invalid IL or missing references)
			//IL_0184: Unknown result type (might be due to invalid IL or missing references)
			//IL_0189: Unknown result type (might be due to invalid IL or missing references)
			//IL_019b: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b7: Unknown result type (might be due to invalid IL or missing references)
			Vector3 val = Main.rgbToHsl(Main.selColor);
			float interfaceDeadzoneX = PlayerInput.CurrentProfile.InterfaceDeadzoneX;
			float x = PlayerInput.GamepadThumbstickLeft.X;
			x = ((!(x < 0f - interfaceDeadzoneX) && !(x > interfaceDeadzoneX)) ? 0f : (MathHelper.Lerp(0f, 1f / 120f, (Math.Abs(x) - interfaceDeadzoneX) / (1f - interfaceDeadzoneX)) * (float)Math.Sign(x)));
			int currentPoint = UILinkPointNavigator.CurrentPoint;
			if (currentPoint == 2800)
			{
				Main.hBar = MathHelper.Clamp(Main.hBar + x, 0f, 1f);
			}
			if (currentPoint == 2801)
			{
				Main.sBar = MathHelper.Clamp(Main.sBar + x, 0f, 1f);
			}
			if (currentPoint == 2802)
			{
				Main.lBar = MathHelper.Clamp(Main.lBar + x, 0.15f, 1f);
			}
			if (currentPoint == 2812)
			{
				Main.player[Main.myPlayer].voicePitchOffset = MathHelper.Clamp(Main.player[Main.myPlayer].voicePitchOffset + x, -1f, 1f);
			}
			Vector3.Clamp(val, Vector3.Zero, Vector3.One);
			if (x != 0f)
			{
				if (Main.clothesWindow)
				{
					Main.selColor = Main.hslToRgb(Main.hBar, Main.sBar, Main.lBar);
					switch (Main.selClothes)
					{
					case 0:
						Main.player[Main.myPlayer].shirtColor = Main.selColor;
						break;
					case 1:
						Main.player[Main.myPlayer].underShirtColor = Main.selColor;
						break;
					case 2:
						Main.player[Main.myPlayer].pantsColor = Main.selColor;
						break;
					case 3:
						Main.player[Main.myPlayer].shoeColor = Main.selColor;
						break;
					}
				}
				if (currentPoint != 2812)
				{
					SoundEngine.PlaySound(12);
				}
			}
			if (currentPoint == 2812)
			{
				bool flag2 = x != 0f;
				if (Main.WasDraggingPlayerAudio && !flag2)
				{
					Main.player[Main.myPlayer].PlayHurtSound();
				}
				Main.WasDraggingPlayerAudio = flag2;
			}
		};
		uILinkPage7.CanEnterEvent += () => Main.clothesWindow;
		uILinkPage7.IsValidEvent += () => Main.clothesWindow;
		uILinkPage7.EnterEvent += () =>
		{
			Main.player[Main.myPlayer].releaseInventory = false;
			Main.WasDraggingPlayerAudio = false;
		};
		uILinkPage7.LeaveEvent += () =>
		{
			Main.player[Main.myPlayer].LockGamepadTileInteractions();
		};
		uILinkPage7.PageOnLeft = 14;
		uILinkPage7.PageOnRight = 14;
		UILinkPointNavigator.RegisterPage(uILinkPage7, 15);
		UILinkPage cp19 = new UILinkPage();
		cp19.UpdateEvent += () =>
		{
			PlayerInput.GamepadAllowScrolling = true;
		};
		for (int num34 = 3000; num34 <= 4999; num34++)
		{
			cp19.LinkMap.Add(num34, new UILinkPoint(num34, enabled: true, -3, -4, -1, -2));
		}
		cp19.OnSpecialInteracts += () => (Main.InGameUI.CurrentState is UIBestiaryTest) ? (PlayerInput.BuildCommand(Lang.misc[82].Value, PlayerInput.ProfileGamepadUI.KeyStatus["Inventory"]) + PlayerInput.BuildCommand(Language.GetText("UI.SwitchPage").Value, PlayerInput.ProfileGamepadUI.KeyStatus["HotbarMinus"], PlayerInput.ProfileGamepadUI.KeyStatus["HotbarPlus"]) + PlayerInput.BuildCommand(Lang.misc[53].Value, PlayerInput.ProfileGamepadUI.KeyStatus["MouseLeft"]) + FancyUISpecialInstructions()) : (PlayerInput.BuildCommand(Lang.misc[53].Value, PlayerInput.ProfileGamepadUI.KeyStatus["MouseLeft"]) + PlayerInput.BuildCommand(Lang.misc[82].Value, PlayerInput.ProfileGamepadUI.KeyStatus["Inventory"]) + FancyUISpecialInstructions());
		cp19.UpdateEvent += () =>
		{
			if (CanExecuteInputCommand() && PlayerInput.Triggers.JustPressed.Inventory)
			{
				FancyExit();
			}
		};
		cp19.EnterEvent += () =>
		{
			cp19.CurrentPoint = 3002;
		};
		cp19.CanEnterEvent += () => Main.MenuUI.IsVisible || Main.InGameUI.IsVisible;
		cp19.IsValidEvent += () => Main.MenuUI.IsVisible || Main.InGameUI.IsVisible;
		cp19.OnPageMoveAttempt += OnFancyUIPageMoveAttempt;
		UILinkPointNavigator.RegisterPage(cp19, 1004);
		UILinkPage cp20 = new UILinkPage();
		cp20.UpdateEvent += () =>
		{
			PlayerInput.GamepadAllowScrolling = true;
		};
		for (int num35 = 10000; num35 <= 11000; num35++)
		{
			cp20.LinkMap.Add(num35, new UILinkPoint(num35, enabled: true, -3, -4, -1, -2));
		}
		for (int num36 = 15000; num36 <= 15000; num36++)
		{
			cp20.LinkMap.Add(num36, new UILinkPoint(num36, enabled: true, -3, -4, -1, -2));
		}
		cp20.OnSpecialInteracts += () => PlayerInput.BuildCommand(Lang.misc[56].Value, PlayerInput.ProfileGamepadUI.KeyStatus["Inventory"]) + PlayerInput.BuildCommand(Lang.misc[64].Value, PlayerInput.ProfileGamepadUI.KeyStatus["HotbarMinus"], PlayerInput.ProfileGamepadUI.KeyStatus["HotbarPlus"]) + PlayerInput.BuildCommand(Lang.misc[53].Value, PlayerInput.ProfileGamepadUI.KeyStatus["MouseLeft"]) + FancyUISpecialInstructions();
		cp20.UpdateEvent += () =>
		{
			if (CanExecuteInputCommand() && PlayerInput.Triggers.JustPressed.Inventory)
			{
				FancyExit();
			}
		};
		cp20.EnterEvent += () =>
		{
			cp20.CurrentPoint = 10000;
		};
		cp20.CanEnterEvent += CanEnterCreativeMenu;
		cp20.IsValidEvent += CanEnterCreativeMenu;
		cp20.OnPageMoveAttempt += OnFancyUIPageMoveAttempt;
		cp20.PageOnLeft = 8;
		cp20.PageOnRight = 0;
		UILinkPointNavigator.RegisterPage(cp20, 1005);
		UILinkPage uILinkPage8 = new UILinkPage();
		for (int num37 = 20000; num37 < 21000; num37++)
		{
			uILinkPage8.LinkMap.Add(num37, new UILinkPoint(num37, enabled: true, -3, -4, -1, -2));
		}
		uILinkPage8.CanEnterEvent += () => NewCraftingUI.Visible;
		uILinkPage8.IsValidEvent += () => NewCraftingUI.Visible;
		uILinkPage8.OnPageMoveAttempt += OnFancyUIPageMoveAttempt;
		uILinkPage8.PageOnLeft = 8;
		uILinkPage8.PageOnRight = 0;
		UILinkPointNavigator.RegisterPage(uILinkPage8, 24);
		UILinkPage cp21 = new UILinkPage();
		cp21.OnSpecialInteracts += () => PlayerInput.BuildCommand(Lang.misc[56].Value, PlayerInput.ProfileGamepadUI.KeyStatus["Inventory"]) + PlayerInput.BuildCommand(Lang.misc[64].Value, PlayerInput.ProfileGamepadUI.KeyStatus["HotbarMinus"], PlayerInput.ProfileGamepadUI.KeyStatus["HotbarPlus"]);
		Func<string> value21 = () => PlayerInput.BuildCommand(Lang.misc[94].Value, PlayerInput.ProfileGamepadUI.KeyStatus["MouseLeft"]);
		for (int num38 = 9000; num38 <= 9050; num38++)
		{
			UILinkPoint uILinkPoint15 = new UILinkPoint(num38, enabled: true, num38 + 10, num38 - 10, num38 - 1, num38 + 1);
			cp21.LinkMap.Add(num38, uILinkPoint15);
			uILinkPoint15.OnSpecialInteracts += value21;
		}
		cp21.UpdateEvent += () =>
		{
			int num39 = UILinkPointNavigator.Shortcuts.BUFFS_PER_COLUMN;
			if (num39 == 0)
			{
				num39 = 100;
			}
			for (int i = 0; i < 50; i++)
			{
				cp21.LinkMap[9000 + i].Up = ((i % num39 == 0) ? (-1) : (9000 + i - 1));
				if (cp21.LinkMap[9000 + i].Up == -1)
				{
					if (i >= num39)
					{
						cp21.LinkMap[9000 + i].Up = 184;
					}
					else
					{
						cp21.LinkMap[9000 + i].Up = 189;
					}
				}
				cp21.LinkMap[9000 + i].Down = (((i + 1) % num39 == 0 || i == UILinkPointNavigator.Shortcuts.BUFFS_DRAWN - 1) ? 308 : (9000 + i + 1));
				cp21.LinkMap[9000 + i].Left = ((i < UILinkPointNavigator.Shortcuts.BUFFS_DRAWN - num39) ? (9000 + i + num39) : (-3));
				cp21.LinkMap[9000 + i].Right = ((i < num39) ? (-4) : (9000 + i - num39));
			}
		};
		cp21.IsValidEvent += () => Main.playerInventory && Main.EquipPage == 2 && UILinkPointNavigator.Shortcuts.BUFFS_DRAWN > 0;
		cp21.PageOnLeft = 8;
		cp21.PageOnRight = 8;
		UILinkPointNavigator.RegisterPage(cp21, 19);
		UILinkPage uILinkPage9 = new UILinkPage();
		uILinkPage9.OnSpecialInteracts += () => PlayerInput.BuildCommand(Lang.misc[56].Value, PlayerInput.ProfileGamepadUI.KeyStatus["Inventory"]) + PlayerInput.BuildCommand(Lang.misc[64].Value, PlayerInput.ProfileGamepadUI.KeyStatus["HotbarMinus"], PlayerInput.ProfileGamepadUI.KeyStatus["HotbarPlus"]);
		uILinkPage9.OnSpecialInteractsLate += () => ItemSlot.GetGamepadInstructions(35);
		UILinkPoint value22 = new UILinkPoint(12000, enabled: true, -3, 11001, -1, -2);
		uILinkPage9.LinkMap.Add(12000, value22);
		uILinkPage9.LinkMap[12000].OnSpecialInteracts += () =>
		{
			string text = "";
			if (Main.mouseItem.stack <= 0 || (Main.mouseItem.type == Main.bannerUI.FocusedItemType && Main.mouseItem.stack < Main.mouseItem.maxStack))
			{
				text += PlayerInput.BuildCommand(Language.GetTextValue("UI.GamepadClaimBanner"), PlayerInput.ProfileGamepadUI.KeyStatus["MouseLeft"], PlayerInput.ProfileGamepadUI.KeyStatus["MouseRight"]);
			}
			return text;
		};
		uILinkPage9.ReachEndEvent += (int current, int next) =>
		{
			bool value24 = next == -1;
			int yOffset = (next == -2).ToInt() - value24.ToInt();
			Main.bannerUI.NavigatePipsList(yOffset);
		};
		uILinkPage9.EnterEvent += () =>
		{
			Main.PipsUseGrid = false;
			PlayerInput.LockGamepadButtons("MouseLeft");
		};
		uILinkPage9.CanEnterEvent += () => Main.playerInventory && Main.bannerUI.AnyAvailableBanners;
		uILinkPage9.IsValidEvent += () => Main.playerInventory && Main.bannerUI.AnyAvailableBanners;
		uILinkPage9.PageOnLeft = 23;
		uILinkPage9.PageOnRight = 0;
		UILinkPointNavigator.RegisterPage(uILinkPage9, 22);
		UILinkPage cp22 = new UILinkPage();
		cp22.OnSpecialInteracts += () => PlayerInput.BuildCommand(Lang.misc[56].Value, PlayerInput.ProfileGamepadUI.KeyStatus["Inventory"]) + PlayerInput.BuildCommand(Lang.misc[64].Value, PlayerInput.ProfileGamepadUI.KeyStatus["HotbarMinus"], PlayerInput.ProfileGamepadUI.KeyStatus["HotbarPlus"]);
		cp22.OnSpecialInteractsLate += () => ItemSlot.GetGamepadInstructions(35);
		UILinkPoint value23 = new UILinkPoint(11100, enabled: true, -3, -4, -1, -2);
		cp22.LinkMap.Add(11100, value23);
		cp22.UpdateEvent += () =>
		{
			_ = UILinkPointNavigator.Shortcuts.CRAFT_IconsPerRow;
			_ = UILinkPointNavigator.Shortcuts.CRAFT_IconsPerColumn;
			cp22.PageOnLeft = GetCornerWrapPageIdFromLeftToRight();
		};
		cp22.ReachEndEvent += (int current, int next) =>
		{
			bool value24 = next == -3;
			int xOffset = (next == -4).ToInt() - value24.ToInt();
			bool value25 = next == -1;
			int yOffset = (next == -2).ToInt() - value25.ToInt();
			Main.bannerUI.NavigatePipsGrid(xOffset, yOffset);
		};
		cp22.EnterEvent += () =>
		{
			Main.PipsUseGrid = true;
			Main.bannerUI.ResetGridSelection();
		};
		cp22.LeaveEvent += () =>
		{
			Main.PipsUseGrid = false;
		};
		cp22.CanEnterEvent += () => Main.playerInventory && Main.bannerUI.AnyAvailableBanners;
		cp22.IsValidEvent += () => Main.playerInventory && Main.PipsUseGrid && Main.bannerUI.AnyAvailableBanners;
		cp22.PageOnLeft = 0;
		cp22.PageOnRight = 22;
		UILinkPointNavigator.RegisterPage(cp22, 23);
		UILinkPage uILinkPage10 = UILinkPointNavigator.Pages[UILinkPointNavigator.CurrentPage];
		uILinkPage10.CurrentPoint = uILinkPage10.DefaultPoint;
		uILinkPage10.Enter();
	}

	private static void MoveToArmorIfHighlighted()
	{
		for (int i = 0; i < 20; i++)
		{
			int context;
			if (i >= 10)
			{
				context = ((i - 10 > 2) ? 11 : 9);
			}
			else
			{
				context = ((i > 2) ? 10 : 8);
			}
			if (ItemSlot.ShouldHighlightSlotForMouseItem(context, i, Main.mouseItem))
			{
				UILinkPointNavigator.ChangePoint(ItemSlot.GetGamepadPointForSlot(Main.player[Main.myPlayer].armor, context, i));
				return;
			}
		}
		for (int j = 0; j < 10; j++)
		{
			int context2 = 12;
			if (ItemSlot.ShouldHighlightSlotForMouseItem(context2, j, Main.mouseItem))
			{
				UILinkPointNavigator.ChangePoint(ItemSlot.GetGamepadPointForSlot(Main.player[Main.myPlayer].dye, context2, j));
				break;
			}
		}
	}

	private static void MoveToEquipIfHighlighted()
	{
		for (int i = 0; i < 2; i++)
		{
			Item[] inv = ((i == 0) ? Main.player[Main.myPlayer].miscEquips : Main.player[Main.myPlayer].miscDyes);
			for (int j = 0; j < 5; j++)
			{
				int context = 0;
				if (i == 0)
				{
					switch (j)
					{
					case 0:
						context = 19;
						break;
					case 1:
						context = 20;
						break;
					case 2:
						context = 18;
						break;
					case 3:
						context = 17;
						break;
					case 4:
						context = 16;
						break;
					}
				}
				else
				{
					context = 33;
				}
				if (ItemSlot.ShouldHighlightSlotForMouseItem(context, j, Main.mouseItem))
				{
					UILinkPointNavigator.ChangePoint(ItemSlot.GetGamepadPointForSlot(inv, context, j));
					return;
				}
			}
		}
	}

	private static void MoveToDisplayDollIfHighlighted()
	{
		if (ItemSlot.ShouldHighlightSlotForMouseItem(38, 0, Main.mouseItem))
		{
			int gamepadPointForSlot = ItemSlot.GetGamepadPointForSlot(null, 38, 0);
			if (gamepadPointForSlot != UILinkPointNavigator.OverridePoint)
			{
				UILinkPointNavigator.ChangePoint(gamepadPointForSlot);
			}
			return;
		}
		for (int i = 0; i < 2; i++)
		{
			for (int j = 0; j < 9; j++)
			{
				int num = 0;
				if (i == 1)
				{
					num = 25;
				}
				else if (j < 3)
				{
					num = 23;
				}
				else
				{
					num = ((j - 3 >= 5) ? 39 : 24);
				}
				if (ItemSlot.ShouldHighlightSlotForMouseItem(num, j, Main.mouseItem))
				{
					int gamepadPointForSlot2 = ItemSlot.GetGamepadPointForSlot(null, num, j);
					if (gamepadPointForSlot2 != UILinkPointNavigator.OverridePoint)
					{
						UILinkPointNavigator.ChangePoint(gamepadPointForSlot2);
					}
					return;
				}
			}
		}
	}

	private static void MoveToHatRackIfHighlighted()
	{
		for (int i = 0; i < 2; i++)
		{
			for (int j = 0; j < 2; j++)
			{
				int context = ((i == 1) ? 27 : 26);
				if (ItemSlot.ShouldHighlightSlotForMouseItem(context, j, Main.mouseItem))
				{
					int gamepadPointForSlot = ItemSlot.GetGamepadPointForSlot(null, context, j);
					if (gamepadPointForSlot != UILinkPointNavigator.OverridePoint)
					{
						UILinkPointNavigator.ChangePoint(gamepadPointForSlot);
					}
					return;
				}
			}
		}
	}

	private static bool TryQuickCrafting(int startPoint, int pointOffset)
	{
		Player player = Main.player[Main.myPlayer];
		int num = Main.recStart + pointOffset;
		if (num >= MainnumAvailableRecipes)
		{
			return false;
		}
		bool result = false;
		int num2 = num - startPoint;
		Recipe recipe = Main.recipe[Main.availableRecipe[num2]];
		if (Main.mouseItem.type == 0 && recipe.createItem.maxStack > 1 && player.ItemSpace(recipe.createItem).CanTakeItemToPersonalInventory && !player.HasLockedInventory())
		{
			result = true;
			if (CanExecuteInputCommand() && PlayerInput.Triggers.JustPressed.Grapple)
			{
				SomeVarsForUILinkers.SequencedCraftingCurrent = recipe;
			}
			if (CanExecuteInputCommand() && PlayerInput.Triggers.Current.Grapple && Main.stackSplit <= 1)
			{
				ItemSlot.RefreshStackSplitCooldown();
				if (SomeVarsForUILinkers.SequencedCraftingCurrent == recipe)
				{
					CraftingRequests.CraftItem(recipe, 1, quickCraft: true);
				}
			}
		}
		return result;
	}

	private static bool CanEnterCreativeMenu()
	{
		if (Main.LocalPlayer.chest != -1)
		{
			return false;
		}
		if (Main.LocalPlayer.talkNPC != -1)
		{
			return false;
		}
		if (Main.playerInventory)
		{
			return Main.CreativeMenu.Enabled;
		}
		return false;
	}

	private static int GetCornerWrapPageIdFromLeftToRight()
	{
		return 8;
	}

	private static int GetCornerWrapPageIdFromRightToLeft()
	{
		if (Main.CreativeMenu.Enabled)
		{
			return 1005;
		}
		if (NewCraftingUI.Visible)
		{
			return 24;
		}
		if (Main.InPipBanner)
		{
			return 23;
		}
		TileEntity tileEntity = Main.LocalPlayer.tileEntityAnchor.GetTileEntity();
		if (tileEntity is TEDisplayDoll)
		{
			return 20;
		}
		if (tileEntity is TEHatRack)
		{
			return 21;
		}
		return 9;
	}

	private static void OnFancyUIPageMoveAttempt(int direction)
	{
		if (Main.MenuUI.CurrentState is UICharacterCreation uICharacterCreation)
		{
			uICharacterCreation.TryMovingCategory(direction);
		}
		if (UserInterface.ActiveInstance.CurrentState is UIBestiaryTest uIBestiaryTest)
		{
			uIBestiaryTest.TryMovingPages(direction);
		}
	}

	public static void FancyExit()
	{
		switch (UILinkPointNavigator.Shortcuts.BackButtonCommand)
		{
		case 1:
			SoundEngine.PlaySound(11);
			Main.menuMode = 0;
			break;
		case 2:
			SoundEngine.PlaySound(11);
			Main.menuMode = ((!Main.menuMultiplayer) ? 1 : 12);
			break;
		case 3:
			Main.menuMode = 0;
			IngameFancyUI.Close();
			break;
		case 4:
			SoundEngine.PlaySound(11);
			Main.menuMode = 11;
			break;
		case 5:
			SoundEngine.PlaySound(11);
			Main.menuMode = 11;
			break;
		case 6:
			Main.LocalPlayer.releaseInventory = false;
			UIVirtualKeyboard.Cancel();
			break;
		case 7:
			if (Main.MenuUI.CurrentState is IHaveBackButtonCommand haveBackButtonCommand)
			{
				haveBackButtonCommand.HandleBackButtonUsage();
			}
			break;
		}
	}

	public static string FancyUISpecialInstructions()
	{
		string text = "";
		int fANCYUI_SPECIAL_INSTRUCTIONS = UILinkPointNavigator.Shortcuts.FANCYUI_SPECIAL_INSTRUCTIONS;
		if (fANCYUI_SPECIAL_INSTRUCTIONS == 1)
		{
			if (CanExecuteInputCommand() && PlayerInput.Triggers.JustPressed.HotbarMinus)
			{
				UIVirtualKeyboard.CycleSymbols();
				PlayerInput.LockGamepadButtons("HotbarMinus");
				PlayerInput.SettingsForUI.TryRevertingToMouseMode();
			}
			text += PlayerInput.BuildCommand(Lang.menu[235].Value, PlayerInput.ProfileGamepadUI.KeyStatus["HotbarMinus"]);
			if (CanExecuteInputCommand() && PlayerInput.Triggers.JustPressed.MouseRight)
			{
				UIVirtualKeyboard.BackSpace();
				PlayerInput.LockGamepadButtons("MouseRight");
				PlayerInput.SettingsForUI.TryRevertingToMouseMode();
			}
			text += PlayerInput.BuildCommand(Lang.menu[236].Value, PlayerInput.ProfileGamepadUI.KeyStatus["MouseRight"]);
			if (CanExecuteInputCommand() && PlayerInput.Triggers.JustPressed.SmartCursor)
			{
				UIVirtualKeyboard.Write(" ");
				PlayerInput.LockGamepadButtons("SmartCursor");
				PlayerInput.SettingsForUI.TryRevertingToMouseMode();
			}
			text += PlayerInput.BuildCommand(Lang.menu[238].Value, PlayerInput.ProfileGamepadUI.KeyStatus["SmartCursor"]);
			if (UIVirtualKeyboard.CanSubmit)
			{
				if (CanExecuteInputCommand() && PlayerInput.Triggers.JustPressed.HotbarPlus)
				{
					UIVirtualKeyboard.Submit();
					PlayerInput.LockGamepadButtons("HotbarPlus");
					PlayerInput.SettingsForUI.TryRevertingToMouseMode();
				}
				text += PlayerInput.BuildCommand(Lang.menu[237].Value, PlayerInput.ProfileGamepadUI.KeyStatus["HotbarPlus"]);
			}
		}
		return text;
	}

	public static void HandleOptionsSpecials()
	{
		//IL_018a: Unknown result type (might be due to invalid IL or missing references)
		//IL_018f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0190: Unknown result type (might be due to invalid IL or missing references)
		//IL_0195: Unknown result type (might be due to invalid IL or missing references)
		//IL_01be: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0226: Unknown result type (might be due to invalid IL or missing references)
		//IL_022b: Unknown result type (might be due to invalid IL or missing references)
		//IL_022c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0231: Unknown result type (might be due to invalid IL or missing references)
		//IL_025a: Unknown result type (might be due to invalid IL or missing references)
		//IL_025f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0260: Unknown result type (might be due to invalid IL or missing references)
		//IL_0265: Unknown result type (might be due to invalid IL or missing references)
		//IL_028b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0290: Unknown result type (might be due to invalid IL or missing references)
		//IL_0291: Unknown result type (might be due to invalid IL or missing references)
		//IL_0296: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_038f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0394: Unknown result type (might be due to invalid IL or missing references)
		//IL_0395: Unknown result type (might be due to invalid IL or missing references)
		//IL_039a: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_03fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_03fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0402: Unknown result type (might be due to invalid IL or missing references)
		//IL_042b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0430: Unknown result type (might be due to invalid IL or missing references)
		//IL_0431: Unknown result type (might be due to invalid IL or missing references)
		//IL_0436: Unknown result type (might be due to invalid IL or missing references)
		//IL_045f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0464: Unknown result type (might be due to invalid IL or missing references)
		//IL_0465: Unknown result type (might be due to invalid IL or missing references)
		//IL_046a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0490: Unknown result type (might be due to invalid IL or missing references)
		//IL_0495: Unknown result type (might be due to invalid IL or missing references)
		//IL_0496: Unknown result type (might be due to invalid IL or missing references)
		//IL_049b: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_04cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_0616: Unknown result type (might be due to invalid IL or missing references)
		//IL_061b: Unknown result type (might be due to invalid IL or missing references)
		//IL_061c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0621: Unknown result type (might be due to invalid IL or missing references)
		//IL_064a: Unknown result type (might be due to invalid IL or missing references)
		//IL_064f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0650: Unknown result type (might be due to invalid IL or missing references)
		//IL_0655: Unknown result type (might be due to invalid IL or missing references)
		//IL_067e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0683: Unknown result type (might be due to invalid IL or missing references)
		//IL_0684: Unknown result type (might be due to invalid IL or missing references)
		//IL_0689: Unknown result type (might be due to invalid IL or missing references)
		//IL_06af: Unknown result type (might be due to invalid IL or missing references)
		//IL_06b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_06b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_06e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_06e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_06e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_06eb: Unknown result type (might be due to invalid IL or missing references)
		switch (UILinkPointNavigator.Shortcuts.OPTIONS_BUTTON_SPECIALFEATURE)
		{
		case 1:
			Main.bgScroll = (int)HandleSliderHorizontalInput(Main.bgScroll, 0f, 100f, PlayerInput.CurrentProfile.InterfaceDeadzoneX, 1f);
			Main.caveParallax = 1f - (float)Main.bgScroll / 500f;
			break;
		case 2:
			Main.musicVolume = HandleSliderHorizontalInput(Main.musicVolume, 0f, 1f, PlayerInput.CurrentProfile.InterfaceDeadzoneX, 0.35f);
			break;
		case 3:
			Main.soundVolume = HandleSliderHorizontalInput(Main.soundVolume, 0f, 1f, PlayerInput.CurrentProfile.InterfaceDeadzoneX, 0.35f);
			break;
		case 4:
			Main.ambientVolume = HandleSliderHorizontalInput(Main.ambientVolume, 0f, 1f, PlayerInput.CurrentProfile.InterfaceDeadzoneX, 0.35f);
			break;
		case 5:
		{
			float hBar = Main.hBar;
			float num3 = (Main.hBar = HandleSliderHorizontalInput(hBar, 0f, 1f));
			if (hBar != num3)
			{
				switch (Main.menuMode)
				{
				case 17:
					Main.player[Main.myPlayer].hairColor = (Main.selColor = Main.hslToRgb(Main.hBar, Main.sBar, Main.lBar));
					break;
				case 18:
					Main.player[Main.myPlayer].eyeColor = (Main.selColor = Main.hslToRgb(Main.hBar, Main.sBar, Main.lBar));
					break;
				case 19:
					Main.player[Main.myPlayer].skinColor = (Main.selColor = Main.hslToRgb(Main.hBar, Main.sBar, Main.lBar));
					break;
				case 21:
					Main.player[Main.myPlayer].shirtColor = (Main.selColor = Main.hslToRgb(Main.hBar, Main.sBar, Main.lBar));
					break;
				case 22:
					Main.player[Main.myPlayer].underShirtColor = (Main.selColor = Main.hslToRgb(Main.hBar, Main.sBar, Main.lBar));
					break;
				case 23:
					Main.player[Main.myPlayer].pantsColor = (Main.selColor = Main.hslToRgb(Main.hBar, Main.sBar, Main.lBar));
					break;
				case 24:
					Main.player[Main.myPlayer].shoeColor = (Main.selColor = Main.hslToRgb(Main.hBar, Main.sBar, Main.lBar));
					break;
				case 25:
					Main.mouseColorSlider.Hue = num3;
					break;
				case 252:
					Main.mouseBorderColorSlider.Hue = num3;
					break;
				}
				SoundEngine.PlaySound(12);
			}
			break;
		}
		case 6:
		{
			float sBar = Main.sBar;
			float num2 = (Main.sBar = HandleSliderHorizontalInput(sBar, 0f, 1f, PlayerInput.CurrentProfile.InterfaceDeadzoneX));
			if (sBar != num2)
			{
				switch (Main.menuMode)
				{
				case 17:
					Main.player[Main.myPlayer].hairColor = (Main.selColor = Main.hslToRgb(Main.hBar, Main.sBar, Main.lBar));
					break;
				case 18:
					Main.player[Main.myPlayer].eyeColor = (Main.selColor = Main.hslToRgb(Main.hBar, Main.sBar, Main.lBar));
					break;
				case 19:
					Main.player[Main.myPlayer].skinColor = (Main.selColor = Main.hslToRgb(Main.hBar, Main.sBar, Main.lBar));
					break;
				case 21:
					Main.player[Main.myPlayer].shirtColor = (Main.selColor = Main.hslToRgb(Main.hBar, Main.sBar, Main.lBar));
					break;
				case 22:
					Main.player[Main.myPlayer].underShirtColor = (Main.selColor = Main.hslToRgb(Main.hBar, Main.sBar, Main.lBar));
					break;
				case 23:
					Main.player[Main.myPlayer].pantsColor = (Main.selColor = Main.hslToRgb(Main.hBar, Main.sBar, Main.lBar));
					break;
				case 24:
					Main.player[Main.myPlayer].shoeColor = (Main.selColor = Main.hslToRgb(Main.hBar, Main.sBar, Main.lBar));
					break;
				case 25:
					Main.mouseColorSlider.Saturation = num2;
					break;
				case 252:
					Main.mouseBorderColorSlider.Saturation = num2;
					break;
				}
				SoundEngine.PlaySound(12);
			}
			break;
		}
		case 7:
		{
			float lBar = Main.lBar;
			float min = 0.15f;
			if (Main.menuMode == 252)
			{
				min = 0f;
			}
			Main.lBar = HandleSliderHorizontalInput(lBar, min, 1f, PlayerInput.CurrentProfile.InterfaceDeadzoneX);
			float lBar2 = Main.lBar;
			if (lBar != lBar2)
			{
				switch (Main.menuMode)
				{
				case 17:
					Main.player[Main.myPlayer].hairColor = (Main.selColor = Main.hslToRgb(Main.hBar, Main.sBar, Main.lBar));
					break;
				case 18:
					Main.player[Main.myPlayer].eyeColor = (Main.selColor = Main.hslToRgb(Main.hBar, Main.sBar, Main.lBar));
					break;
				case 19:
					Main.player[Main.myPlayer].skinColor = (Main.selColor = Main.hslToRgb(Main.hBar, Main.sBar, Main.lBar));
					break;
				case 21:
					Main.player[Main.myPlayer].shirtColor = (Main.selColor = Main.hslToRgb(Main.hBar, Main.sBar, Main.lBar));
					break;
				case 22:
					Main.player[Main.myPlayer].underShirtColor = (Main.selColor = Main.hslToRgb(Main.hBar, Main.sBar, Main.lBar));
					break;
				case 23:
					Main.player[Main.myPlayer].pantsColor = (Main.selColor = Main.hslToRgb(Main.hBar, Main.sBar, Main.lBar));
					break;
				case 24:
					Main.player[Main.myPlayer].shoeColor = (Main.selColor = Main.hslToRgb(Main.hBar, Main.sBar, Main.lBar));
					break;
				case 25:
					Main.mouseColorSlider.Luminance = lBar2;
					break;
				case 252:
					Main.mouseBorderColorSlider.Luminance = lBar2;
					break;
				}
				SoundEngine.PlaySound(12);
			}
			break;
		}
		case 8:
		{
			float aBar = Main.aBar;
			float num4 = (Main.aBar = HandleSliderHorizontalInput(aBar, 0f, 1f, PlayerInput.CurrentProfile.InterfaceDeadzoneX));
			if (aBar != num4)
			{
				int menuMode = Main.menuMode;
				if (menuMode == 252)
				{
					Main.mouseBorderColorSlider.Alpha = num4;
				}
				SoundEngine.PlaySound(12);
			}
			break;
		}
		case 9:
		{
			bool left = PlayerInput.Triggers.Current.Left;
			bool right = PlayerInput.Triggers.Current.Right;
			if (PlayerInput.Triggers.JustPressed.Left || PlayerInput.Triggers.JustPressed.Right)
			{
				SomeVarsForUILinkers.HairMoveCD = 0;
			}
			else if (SomeVarsForUILinkers.HairMoveCD > 0)
			{
				SomeVarsForUILinkers.HairMoveCD--;
			}
			if (SomeVarsForUILinkers.HairMoveCD == 0 && (left | right))
			{
				if (left)
				{
					Main.PendingPlayer.hair--;
				}
				if (right)
				{
					Main.PendingPlayer.hair++;
				}
				SomeVarsForUILinkers.HairMoveCD = 12;
			}
			int num = 51;
			if (Main.PendingPlayer.hair >= num)
			{
				Main.PendingPlayer.hair = 0;
			}
			if (Main.PendingPlayer.hair < 0)
			{
				Main.PendingPlayer.hair = num - 1;
			}
			break;
		}
		case 10:
			Main.GameZoomTarget = HandleSliderHorizontalInput(Main.GameZoomTarget, 1f, 2f, PlayerInput.CurrentProfile.InterfaceDeadzoneX, 0.35f);
			break;
		case 11:
			Main.UIScale = HandleSliderHorizontalInput(Main.UIScaleWanted, 0.5f, 2f, PlayerInput.CurrentProfile.InterfaceDeadzoneX, 0.35f);
			Main.temporaryGUIScaleSlider = Main.UIScaleWanted;
			break;
		case 12:
			Main.MapScale = HandleSliderHorizontalInput(Main.MapScale, 0.5f, 1f, PlayerInput.CurrentProfile.InterfaceDeadzoneX, 0.7f);
			break;
		}
	}
}
