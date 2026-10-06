using Terraria.Audio;
using Terraria.GameContent.UI;
using Terraria.GameInput;
using Terraria.Graphics.Capture;

namespace Terraria.UI;

public static class IngameUIWindows
{
	public static void CloseAll(bool quiet = false)
	{
		if (Main.mapFullscreen)
		{
			Main.mapFullscreen = false;
			if (!quiet)
			{
				SoundEngine.PlaySound(11);
			}
		}
		if (PlayerInput.InBuildingMode)
		{
			PlayerInput.ExitBuildingMode(quiet);
		}
		if (Main.ingameOptionsWindow)
		{
			IngameOptions.Close(quiet);
		}
		if (Main.inFancyUI)
		{
			IngameFancyUI.Close(quiet);
		}
		CaptureManager.Instance.Active = false;
		if (Main.LocalPlayer.talkNPC >= 0)
		{
			Main.LocalPlayer.SetTalkNPC(-1);
			Main.npcChatCornerItem = 0;
			Main.npcChatText = "";
			if (!quiet)
			{
				SoundEngine.PlaySound(11);
			}
		}
		Main.LocalPlayer.CloseSign(quiet);
		Main.CancelHairWindow(quiet);
		Main.CancelClothesWindow(quiet);
		if (Main.LocalPlayer.tileEntityAnchor.InUse)
		{
			Main.LocalPlayer.tileEntityAnchor.Clear();
		}
		if (Main.LocalPlayer.chest != -1)
		{
			Main.LocalPlayer.chest = -1;
			if (!quiet)
			{
				SoundEngine.PlaySound(11);
			}
		}
		if (Main.playerInventory)
		{
			Main.playerInventory = false;
			if (!quiet)
			{
				SoundEngine.PlaySound(11);
			}
		}
		Main.SetNPCShopIndex(0);
		Main.CreativeMenu.CloseMenu();
		NewCraftingUI.Close(quiet: true);
		CraftingUI.ClearHacks();
	}
}
