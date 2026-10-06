using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria.GameInput;

namespace Terraria.UI.Gamepad;

public class GamepadMainMenuHandler
{
	public static int LastMainMenu = -1;

	public static List<Vector2> MenuItemPositions = new List<Vector2>(20);

	public static int LastDrew = -1;

	public static bool CanRun = false;

	public static bool MoveCursorOnNextRun = false;

	public static void Update()
	{
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_012e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0133: Unknown result type (might be due to invalid IL or missing references)
		//IL_014f: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0211: Unknown result type (might be due to invalid IL or missing references)
		//IL_0213: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e1: Unknown result type (might be due to invalid IL or missing references)
		if (!CanRun)
		{
			UILinkPage uILinkPage = UILinkPointNavigator.Pages[1000];
			uILinkPage.CurrentPoint = uILinkPage.DefaultPoint;
			Vector2 val = new Vector2((float)Math.Cos(Main.GlobalTimeWrappedHourly * ((float)Math.PI * 2f)), (float)Math.Sin(Main.GlobalTimeWrappedHourly * ((float)Math.PI * 2f) * 2f)) * new Vector2(30f, 15f) + Vector2.UnitY * 20f;
			UILinkPointNavigator.SetPosition(2000, new Vector2((float)Main.screenWidth, (float)Main.screenHeight) / 2f + val);
		}
		else
		{
			if (!Main.gameMenu || Main.MenuUI.IsVisible || LastDrew != Main.menuMode)
			{
				return;
			}
			int lastMainMenu = LastMainMenu;
			LastMainMenu = Main.menuMode;
			switch (Main.menuMode)
			{
			case 17:
			case 18:
			case 19:
			case 21:
			case 22:
			case 23:
			case 24:
			case 26:
				if (MenuItemPositions.Count >= 4)
				{
					Vector2 item = MenuItemPositions[3];
					MenuItemPositions.RemoveAt(3);
					if (Main.menuMode == 17)
					{
						MenuItemPositions.Insert(0, item);
					}
				}
				break;
			case 28:
				if (MenuItemPositions.Count >= 3)
				{
					MenuItemPositions.RemoveAt(1);
				}
				break;
			}
			UILinkPage uILinkPage2 = UILinkPointNavigator.Pages[1000];
			if (lastMainMenu != Main.menuMode)
			{
				uILinkPage2.CurrentPoint = uILinkPage2.DefaultPoint;
			}
			for (int i = 0; i < MenuItemPositions.Count; i++)
			{
				Vector2 val2 = MenuItemPositions[i] * Main.UIScale;
				if (i == 0 && lastMainMenu != LastMainMenu && PlayerInput.UsingGamepad && Main.InvisibleCursorForGamepad)
				{
					Main.mouseX = (PlayerInput.MouseX = (int)val2.X);
					Main.mouseY = (PlayerInput.MouseY = (int)val2.Y);
					Main.menuFocus = -1;
				}
				UILinkPoint uILinkPoint = uILinkPage2.LinkMap[2000 + i];
				uILinkPoint.Position = val2;
				if (i == 0)
				{
					uILinkPoint.Up = -1;
				}
				else
				{
					uILinkPoint.Up = 2000 + i - 1;
				}
				uILinkPoint.Left = -3;
				uILinkPoint.Right = -4;
				if (i == MenuItemPositions.Count - 1)
				{
					uILinkPoint.Down = -2;
				}
				else
				{
					uILinkPoint.Down = 2000 + i + 1;
				}
				if (MoveCursorOnNextRun)
				{
					MoveCursorOnNextRun = false;
					UILinkPointNavigator.ChangePoint(uILinkPoint.ID);
				}
			}
			MenuItemPositions.Clear();
		}
	}
}
