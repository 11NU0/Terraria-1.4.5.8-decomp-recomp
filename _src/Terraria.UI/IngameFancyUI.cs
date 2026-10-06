using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Achievements;
using Terraria.Audio;
using Terraria.GameContent.UI.States;
using Terraria.GameInput;
using Terraria.Localization;
using Terraria.UI.Gamepad;

namespace Terraria.UI;

public class IngameFancyUI
{
	private static bool CoverForOneUIFrame;

	public static void CoverNextFrame()
	{
		CoverForOneUIFrame = true;
	}

	public static bool CanCover()
	{
		if (CoverForOneUIFrame)
		{
			CoverForOneUIFrame = false;
			return true;
		}
		if (Main.inFancyUI)
		{
			return true;
		}
		return false;
	}

	public static void OpenAchievements()
	{
		if (Main.gameMenu)
		{
			Main.MenuUI.SetState(Main.AchievementsMenu);
		}
		else
		{
			OpenUIState(Main.AchievementsMenu);
		}
	}

	public static void OpenAchievementsAndGoto(Achievement achievement)
	{
		OpenAchievements();
		Main.AchievementsMenu.GotoAchievement(achievement);
	}

	private static void ClearChat()
	{
		Main.ClosePlayerChat();
		Main.chatText = "";
	}

	public static void OpenKeybinds()
	{
		OpenUIState(Main.ManageControlsMenu);
	}

	public static void OpenUIState(UIState uiState)
	{
		OpenUIState(uiState, true);
	}

	public static void OpenUIState(UIState uiState, bool closeIngameWindows = true)
	{
		CoverNextFrame();
		ClearChat();
		if (!Main.inFancyUI & closeIngameWindows)
		{
			IngameUIWindows.CloseAll(quiet: true);
		}
		Main.inFancyUI = true;
		Main.InGameUI.SetState(uiState);
	}

	public static bool CanShowVirtualKeyboard(int context)
	{
		return UIVirtualKeyboard.CanDisplay(context);
	}

	public static void OpenVirtualKeyboard(int keyboardContext)
	{
		CoverNextFrame();
		ClearChat();
		SoundEngine.PlaySound(12);
		string labelText = "";
		switch (keyboardContext)
		{
		case 1:
			Main.editSign = true;
			labelText = Language.GetTextValue("UI.EnterMessage");
			break;
		case 2:
		{
			labelText = Language.GetTextValue("UI.EnterNewName");
			Player player = Main.player[Main.myPlayer];
			Main.npcChatText = Main.chest[player.chest].name;
			Tile tile = Main.tile[player.chestX, player.chestY];
			if (tile.type == 21)
			{
				Main.defaultChestName = Lang.chestType[tile.frameX / 36].Value;
			}
			else if (tile.type == 467 && tile.frameX / 36 == 4)
			{
				Main.defaultChestName = Lang.GetItemNameValue(3988);
			}
			else if (tile.type == 467)
			{
				Main.defaultChestName = Lang.chestType2[tile.frameX / 36].Value;
			}
			else if (tile.type == 88)
			{
				Main.defaultChestName = Lang.dresserType[tile.frameX / 54].Value;
			}
			if (Main.npcChatText == "")
			{
				Main.npcChatText = Main.defaultChestName;
			}
			Main.editChest = true;
			break;
		}
		}
		Main.clrInput();
		if (!CanShowVirtualKeyboard(keyboardContext))
		{
			return;
		}
		Main.inFancyUI = true;
		switch (keyboardContext)
		{
		case 1:
			Main.InGameUI.SetState(new UIVirtualKeyboard(labelText, Main.npcChatText, (string s) =>
			{
				Main.SubmitSignText();
				Close(quiet: true);
			}, () =>
			{
				Main.InputTextSignCancel();
				Close(quiet: true);
			}, keyboardContext, allowEmpty: false, 1200));
			break;
		case 2:
			Main.InGameUI.SetState(new UIVirtualKeyboard(labelText, Main.npcChatText, (string s) =>
			{
				ChestUI.RenameChestSubmit(Main.player[Main.myPlayer]);
				Close(quiet: true);
			}, () =>
			{
				ChestUI.RenameChestCancel();
				Close(quiet: true);
			}, keyboardContext));
			break;
		}
		UILinkPointNavigator.GoToDefaultPage(1);
	}

	public static void Close(bool quiet = false)
	{
		Main.inFancyUI = false;
		if (!quiet)
		{
			SoundEngine.PlaySound(11);
		}
		bool flag = false;
		if (!Main.gameMenu)
		{
			if (Main.InGameUI.CurrentState is UIVirtualKeyboard)
			{
				flag = UIVirtualKeyboard.KeyboardContext == 2;
			}
			else if (!(Main.InGameUI.CurrentState is UIEmotesMenu))
			{
				flag = true;
			}
		}
		if (flag)
		{
			Main.playerInventory = true;
		}
		Main.LocalPlayer.releaseInventory = false;
		Main.InGameUI.SetState(null);
		UILinkPointNavigator.Shortcuts.FANCYUI_SPECIAL_INSTRUCTIONS = 0;
	}

	public static bool Draw(SpriteBatch spriteBatch, GameTime gameTime)
	{
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
		bool result = false;
		if (Main.InGameUI.CurrentState is UIVirtualKeyboard && UIVirtualKeyboard.KeyboardContext > 0)
		{
			if (!Main.inFancyUI)
			{
				Main.InGameUI.SetState(null);
			}
			if (Main.screenWidth >= 1705 || !PlayerInput.UsingGamepad)
			{
				result = true;
			}
		}
		if (!Main.gameMenu)
		{
			Main.mouseText = false;
			if (Main.InGameUI != null && Main.InGameUI.IsElementUnderMouse())
			{
				Main.player[Main.myPlayer].mouseInterface = true;
			}
			Main.instance.GUIBarsDraw();
			if (Main.InGameUI.CurrentState is UIVirtualKeyboard && UIVirtualKeyboard.KeyboardContext > 0)
			{
				Main.instance.GUIChatDraw();
			}
			if (!Main.inFancyUI)
			{
				Main.InGameUI.SetState(null);
			}
			Main.instance.DrawMouseOver();
			Main.spriteBatch.End();
			Main.spriteBatch.Begin((SpriteSortMode)0, BlendState.AlphaBlend, Main.SamplerStateForCursor, DepthStencilState.None, RasterizerState.CullCounterClockwise, (Effect)null, Main.UIScaleMatrix);
			Main.DrawCursor(Main.DrawThickCursor());
		}
		return result;
	}

	public static void MouseOver()
	{
		if (Main.inFancyUI && Main.InGameUI.IsElementUnderMouse())
		{
			Main.mouseText = true;
		}
	}
}
