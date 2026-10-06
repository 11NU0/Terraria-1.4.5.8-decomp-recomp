using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ReLogic.Graphics;
using ReLogic.Localization.IME;
using ReLogic.OS;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.GameContent.UI;
using Terraria.GameInput;
using Terraria.UI.Chat;
using Terraria.UI.Gamepad;

namespace Terraria.UI;

public class ChestUI
{
	public class ButtonID
	{
		public const int LootAll = 0;

		public const int DepositAll = 1;

		public const int QuickStack = 2;

		public const int Restock = 3;

		public const int Sort = 4;

		public const int RenameChest = 5;

		public const int RenameChestCancel = 6;

		public static readonly int Count = 7;
	}

	public const float buttonScaleMinimum = 0.75f;

	public const float buttonScaleMaximum = 1f;

	public static float[] ButtonScale = new float[ButtonID.Count];

	public static bool[] ButtonHovered = new bool[ButtonID.Count];

	public static int StartingRowForDrawing = 0;

	public static int LastHighestChestRow = 0;

	public static Rectangle LastChestDisplayRectangle;

	public static void UpdateHover(int ID, bool hovering)
	{
		if (hovering)
		{
			if (!ButtonHovered[ID])
			{
				SoundEngine.PlaySound(12);
			}
			ButtonHovered[ID] = true;
			ButtonScale[ID] += 0.05f;
			if (ButtonScale[ID] > 1f)
			{
				ButtonScale[ID] = 1f;
			}
		}
		else
		{
			ButtonHovered[ID] = false;
			ButtonScale[ID] -= 0.05f;
			if (ButtonScale[ID] < 0.75f)
			{
				ButtonScale[ID] = 0.75f;
			}
		}
	}

	public static void Draw(SpriteBatch spritebatch)
	{
		if (Main.player[Main.myPlayer].chest != -1 && !Main.PipsUseGrid && !NewCraftingUI.Visible)
		{
			Main.inventoryScale = 0.755f;
			if (Utils.FloatIntersect(Main.mouseX, Main.mouseY, 0f, 0f, 73f, Main.instance.invBottom, 560f * Main.inventoryScale, 224f * Main.inventoryScale))
			{
				Main.player[Main.myPlayer].mouseInterface = true;
			}
			DrawName(spritebatch);
			DrawButtons(spritebatch);
			DrawSlots(spritebatch);
		}
		else
		{
			for (int i = 0; i < ButtonID.Count; i++)
			{
				ButtonScale[i] = 0.75f;
				ButtonHovered[i] = false;
			}
		}
	}

	private static void DrawName(SpriteBatch spritebatch)
	{
		//IL_019a: Unknown result type (might be due to invalid IL or missing references)
		//IL_019f: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0201: Unknown result type (might be due to invalid IL or missing references)
		//IL_0206: Unknown result type (might be due to invalid IL or missing references)
		//IL_022f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0240: Unknown result type (might be due to invalid IL or missing references)
		//IL_024c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0251: Unknown result type (might be due to invalid IL or missing references)
		//IL_027e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0280: Unknown result type (might be due to invalid IL or missing references)
		//IL_028a: Unknown result type (might be due to invalid IL or missing references)
		//IL_028f: Unknown result type (might be due to invalid IL or missing references)
		//IL_02af: Unknown result type (might be due to invalid IL or missing references)
		//IL_0310: Unknown result type (might be due to invalid IL or missing references)
		//IL_0312: Unknown result type (might be due to invalid IL or missing references)
		//IL_0318: Unknown result type (might be due to invalid IL or missing references)
		//IL_031d: Unknown result type (might be due to invalid IL or missing references)
		Player player = Main.player[Main.myPlayer];
		string text = string.Empty;
		if (Main.editChest)
		{
			text = Main.npcChatText;
		}
		else if (player.chest > -1 && Main.chest[player.chest] != null)
		{
			Chest chest = Main.chest[player.chest];
			if (chest.name != "")
			{
				text = chest.name;
			}
			else
			{
				Tile tile = Main.tile[player.chestX, player.chestY];
				if (tile.type == 21)
				{
					text = Lang.chestType[tile.frameX / 36].Value;
				}
				else if (tile.type == 467 && tile.frameX / 36 == 4)
				{
					text = Lang.GetItemNameValue(3988);
				}
				else if (tile.type == 467)
				{
					text = Lang.chestType2[tile.frameX / 36].Value;
				}
				else if (tile.type == 88)
				{
					text = Lang.dresserType[tile.frameX / 54].Value;
				}
			}
		}
		else if (player.chest == -2)
		{
			text = Lang.inter[32].Value;
		}
		else if (player.chest == -3)
		{
			text = Lang.inter[33].Value;
		}
		else if (player.chest == -4)
		{
			text = Lang.GetItemNameValue(3813);
		}
		else if (player.chest == -5)
		{
			text = Lang.GetItemNameValue(4076);
		}
		Color baseColor = new Color((int)Main.mouseTextColor, (int)Main.mouseTextColor, (int)Main.mouseTextColor, (int)Main.mouseTextColor);
		baseColor = Color.White * (1f - (255f - (float)(int)Main.mouseTextColor) / 255f * 0.5f);
		baseColor.A = byte.MaxValue;
		DynamicSpriteFont value = FontAssets.MouseText.Value;
		Vector2 val = new Vector2(504f, (float)Main.instance.invBottom);
		ChatManager.DrawColorCodedStringWithShadow(spritebatch, value, text, val, baseColor, 0f, Vector2.Zero, Vector2.One, -1f, 1.5f);
		if (Main.editChest)
		{
			val.X += value.MeasureString(text).X;
			Main.instance.SetIMEPanelAnchor(val + new Vector2(0f, 56f), 0f);
			string compositionString = Platform.Get<IImeService>().CompositionString;
			if (compositionString != null && compositionString.Length > 0)
			{
				ChatManager.DrawColorCodedStringWithShadow(spritebatch, value, compositionString, val, Main.imeCompositionStringColor, 0f, Vector2.Zero, Vector2.One, -1f, 1.5f);
				val.X += value.MeasureString(compositionString).X;
			}
			if (++Main.instance.textBlinkerCount >= 20)
			{
				Main.instance.textBlinkerState = ((Main.instance.textBlinkerState == 0) ? 1 : 0);
				Main.instance.textBlinkerCount = 0;
			}
			if (Main.instance.textBlinkerState == 1)
			{
				ChatManager.DrawColorCodedStringWithShadow(spritebatch, value, "|", val, baseColor, 0f, Vector2.Zero, Vector2.One, -1f, 1.5f);
			}
		}
	}

	private static void DrawButtons(SpriteBatch spritebatch)
	{
		for (int i = 0; i < ButtonID.Count; i++)
		{
			DrawButton(spritebatch, i, 506, Main.instance.invBottom + 40);
		}
	}

	private static void DrawButton(SpriteBatch spriteBatch, int ID, int X, int Y)
	{
		//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_0104: Unknown result type (might be due to invalid IL or missing references)
		//IL_010a: Unknown result type (might be due to invalid IL or missing references)
		//IL_010f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0111: Unknown result type (might be due to invalid IL or missing references)
		//IL_011b: Unknown result type (might be due to invalid IL or missing references)
		//IL_013e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0143: Unknown result type (might be due to invalid IL or missing references)
		//IL_0151: Unknown result type (might be due to invalid IL or missing references)
		//IL_0188: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0202: Unknown result type (might be due to invalid IL or missing references)
		//IL_0209: Unknown result type (might be due to invalid IL or missing references)
		//IL_020f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0215: Unknown result type (might be due to invalid IL or missing references)
		//IL_0229: Unknown result type (might be due to invalid IL or missing references)
		//IL_022b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0230: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_025f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0276: Unknown result type (might be due to invalid IL or missing references)
		//IL_028c: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_030d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0324: Unknown result type (might be due to invalid IL or missing references)
		//IL_0337: Unknown result type (might be due to invalid IL or missing references)
		//IL_034e: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fa: Unknown result type (might be due to invalid IL or missing references)
		Player player = Main.player[Main.myPlayer];
		if ((ID == 5 && player.chest < -1) || (ID == 6 && !Main.editChest))
		{
			UpdateHover(ID, hovering: false);
			return;
		}
		Y += ID * 26;
		float num = ButtonScale[ID];
		string text = "";
		switch (ID)
		{
		case 0:
			text = Lang.inter[29].Value;
			break;
		case 1:
			text = Lang.inter[30].Value;
			break;
		case 2:
			text = Lang.inter[31].Value;
			break;
		case 3:
			text = Lang.inter[82].Value;
			break;
		case 5:
			text = Lang.inter[Main.editChest ? 47 : 61].Value;
			break;
		case 6:
			text = Lang.inter[63].Value;
			break;
		case 4:
			text = Lang.inter[122].Value;
			break;
		}
		Vector2 val = FontAssets.MouseText.Value.MeasureString(text);
		Color baseColor = new Color((int)Main.mouseTextColor, (int)Main.mouseTextColor, (int)Main.mouseTextColor, (int)Main.mouseTextColor) * num;
		baseColor = Color.White * 0.97f * (1f - (255f - (float)(int)Main.mouseTextColor) / 255f * 0.5f);
		baseColor.A = byte.MaxValue;
		int num2 = (int)(val.X * num / 2f);
		X += num2;
		bool flag = Utils.FloatIntersect(Main.mouseX, Main.mouseY, 0f, 0f, X - num2, Y - 12, val.X * num, 24f);
		if (ButtonHovered[ID])
		{
			flag = Utils.FloatIntersect(Main.mouseX, Main.mouseY, 0f, 0f, X - num2 - 10, Y - 12, val.X * num + 16f, 24f);
		}
		if (flag)
		{
			baseColor = Main.OurFavoriteColor;
		}
		ChatManager.DrawColorCodedStringWithShadow(spriteBatch, FontAssets.MouseText.Value, text, new Vector2((float)X, (float)Y), baseColor, 0f, val / 2f, new Vector2(num), -1f, 1.5f);
		val *= num;
		switch (ID)
		{
		case 0:
			UILinkPointNavigator.SetPosition(500, new Vector2((float)X - val.X * num / 2f * 0.8f, (float)Y));
			break;
		case 1:
			UILinkPointNavigator.SetPosition(501, new Vector2((float)X - val.X * num / 2f * 0.8f, (float)Y));
			break;
		case 2:
			UILinkPointNavigator.SetPosition(502, new Vector2((float)X - val.X * num / 2f * 0.8f, (float)Y));
			break;
		case 5:
			UILinkPointNavigator.SetPosition(504, new Vector2((float)X, (float)Y));
			break;
		case 6:
			UILinkPointNavigator.SetPosition(504, new Vector2((float)X, (float)Y));
			break;
		case 3:
			UILinkPointNavigator.SetPosition(503, new Vector2((float)X - val.X * num / 2f * 0.8f, (float)Y));
			break;
		case 4:
			UILinkPointNavigator.SetPosition(505, new Vector2((float)X - val.X * num / 2f * 0.8f, (float)Y));
			break;
		}
		if (!flag)
		{
			UpdateHover(ID, hovering: false);
			return;
		}
		UpdateHover(ID, hovering: true);
		if (PlayerInput.IgnoreMouseInterface)
		{
			return;
		}
		player.mouseInterface = true;
		if (Main.mouseLeft && Main.mouseLeftRelease)
		{
			switch (ID)
			{
			case 0:
				LootAll();
				break;
			case 1:
				DepositAll();
				break;
			case 2:
				QuickStack();
				break;
			case 5:
				RenameChest();
				break;
			case 6:
				RenameChestCancel();
				break;
			case 3:
				Restock();
				break;
			case 4:
				ItemSorting.SortChest();
				break;
			}
		}
	}

	public static void Scroll(int mouseWheel)
	{
		int startingRowForDrawing = StartingRowForDrawing;
		if (mouseWheel > 0)
		{
			StartingRowForDrawing--;
		}
		else
		{
			StartingRowForDrawing++;
		}
		StartingRowForDrawing = Utils.Clamp(StartingRowForDrawing, 0, LastHighestChestRow);
		if (startingRowForDrawing != StartingRowForDrawing)
		{
			SoundEngine.PlaySound(12);
		}
	}

	private static void DrawSlots(SpriteBatch spriteBatch)
	{
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_019a: Unknown result type (might be due to invalid IL or missing references)
		//IL_020a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0211: Unknown result type (might be due to invalid IL or missing references)
		//IL_0217: Unknown result type (might be due to invalid IL or missing references)
		int num = 10;
		int num2 = 4;
		Player player = Main.player[Main.myPlayer];
		int context = 0;
		Chest chest = null;
		if (player.chest > -1)
		{
			context = 3;
			chest = Main.chest[player.chest];
		}
		if (player.chest == -2)
		{
			context = 4;
			chest = player.bank;
		}
		if (player.chest == -3)
		{
			context = 4;
			chest = player.bank2;
		}
		if (player.chest == -4)
		{
			context = 4;
			chest = player.bank3;
		}
		if (player.chest == -5)
		{
			context = 32;
			chest = player.bank4;
		}
		Item[] item = chest.item;
		int maxItems = chest.maxItems;
		Main.inventoryScale = 0.755f;
		Rectangle val = (LastChestDisplayRectangle = new Rectangle(73, Main.instance.invBottom, (int)((float)(num * 56) * Main.inventoryScale), (int)((float)(num2 * 56) * Main.inventoryScale)));
		if (val.Contains(Main.mouseX, Main.mouseY) && !PlayerInput.IgnoreMouseInterface)
		{
			player.mouseInterface = true;
		}
		int num3 = (int)Math.Max(0.0, Math.Ceiling((float)maxItems / (float)num) - 4.0);
		StartingRowForDrawing = Utils.Clamp(StartingRowForDrawing, 0, num3);
		LastHighestChestRow = num3;
		ItemSlot.PrepareForChest(chest);
		for (int i = 0; i < num; i++)
		{
			for (int j = 0; j < num2; j++)
			{
				int num4 = i + j * num + StartingRowForDrawing * num;
				if (num4 < item.Length)
				{
					int num5 = (int)(73f + (float)(i * 56) * Main.inventoryScale);
					int num6 = (int)((float)Main.instance.invBottom + (float)(j * 56) * Main.inventoryScale);
					new Color(100, 100, 100, 100);
					if (Utils.FloatIntersect(Main.mouseX, Main.mouseY, 0f, 0f, num5, num6, (float)TextureAssets.InventoryBack.Width() * Main.inventoryScale, (float)TextureAssets.InventoryBack.Height() * Main.inventoryScale) && !PlayerInput.IgnoreMouseInterface)
					{
						player.mouseInterface = true;
						ItemSlot.Handle(item, context, num4);
					}
					ItemSlot.Draw(spriteBatch, item, context, num4, new Vector2((float)num5, (float)num6));
				}
			}
		}
	}

	public static void LootAll()
	{
		Player player = Main.player[Main.myPlayer];
		GetItemSettings settings = ((player.chest > -1) ? GetItemSettings.LootAllFromChest : GetItemSettings.LootAllFromBank);
		if (player.chest > -1)
		{
			Chest chest = Main.chest[player.chest];
			for (int i = 0; i < chest.maxItems; i++)
			{
				if (chest.item[i].type > 0)
				{
					Player.GetItemLogger.Start();
					chest.item[i] = player.GetItem(chest.item[i], settings);
					Player.GetItemLogger.Stop();
					ItemSlot.DisplayTransfer_GetItem(chest.item, 3, i);
					if (Main.netMode == 1)
					{
						NetMessage.SendData(32, -1, -1, null, player.chest, i);
					}
					CoinSlot.ForceSlotState(i, 3, chest.item[i]);
				}
			}
			return;
		}
		if (player.chest == -3)
		{
			for (int j = 0; j < player.bank2.maxItems; j++)
			{
				if (player.bank2.item[j].type > 0)
				{
					Player.GetItemLogger.Start();
					player.bank2.item[j] = player.GetItem(player.bank2.item[j], settings);
					Player.GetItemLogger.Stop();
					ItemSlot.DisplayTransfer_GetItem(player.bank2.item, 4, j);
					CoinSlot.ForceSlotState(j, 4, player.bank2.item[j]);
				}
			}
			return;
		}
		if (player.chest == -4)
		{
			for (int k = 0; k < player.bank3.maxItems; k++)
			{
				if (player.bank3.item[k].type > 0)
				{
					Player.GetItemLogger.Start();
					player.bank3.item[k] = player.GetItem(player.bank3.item[k], settings);
					Player.GetItemLogger.Stop();
					ItemSlot.DisplayTransfer_GetItem(player.bank3.item, 4, k);
					CoinSlot.ForceSlotState(k, 4, player.bank3.item[k]);
				}
			}
			return;
		}
		if (player.chest == -5)
		{
			for (int l = 0; l < player.bank4.maxItems; l++)
			{
				if (player.bank4.item[l].type > 0 && !player.bank4.item[l].favorited)
				{
					Player.GetItemLogger.Start();
					player.bank4.item[l] = player.GetItem(player.bank4.item[l], settings);
					Player.GetItemLogger.Stop();
					ItemSlot.DisplayTransfer_GetItem(player.bank4.item, 32, l);
					CoinSlot.ForceSlotState(l, 32, player.bank4.item[l]);
				}
			}
			return;
		}
		for (int m = 0; m < player.bank.maxItems; m++)
		{
			if (player.bank.item[m].type > 0)
			{
				Player.GetItemLogger.Start();
				player.bank.item[m] = player.GetItem(player.bank.item[m], settings);
				Player.GetItemLogger.Stop();
				ItemSlot.DisplayTransfer_GetItem(player.bank.item, 4, m);
				CoinSlot.ForceSlotState(m, 4, player.bank.item[m]);
			}
		}
	}

	private static void DepositAll_IntoWorldChest(Player player, Chest chest, int playerInventorySlot)
	{
		for (int i = 0; i < chest.maxItems; i++)
		{
			if (chest.item[i].stack >= chest.item[i].maxStack || !Item.CanStack(player.inventory[playerInventorySlot], chest.item[i]))
			{
				continue;
			}
			int num = player.inventory[playerInventorySlot].stack;
			if (player.inventory[playerInventorySlot].stack + chest.item[i].stack > chest.item[i].maxStack)
			{
				num = chest.item[i].maxStack - chest.item[i].stack;
			}
			player.inventory[playerInventorySlot].stack -= num;
			chest.item[i].stack += num;
			SoundEngine.PlaySound(7);
			if (player.inventory[playerInventorySlot].stack <= 0)
			{
				player.inventory[playerInventorySlot].SetDefaults(0);
				if (Main.netMode == 1)
				{
					NetMessage.SendData(32, -1, -1, null, player.chest, i);
				}
				break;
			}
			if (chest.item[i].type == 0)
			{
				chest.item[i] = player.inventory[playerInventorySlot].Clone();
				player.inventory[playerInventorySlot].SetDefaults(0);
			}
			if (Main.netMode == 1)
			{
				NetMessage.SendData(32, -1, -1, null, player.chest, i);
			}
		}
	}

	private static void DepositAll_IntoLocalChest(Player player, Chest chest, int p)
	{
		for (int i = 0; i < chest.maxItems; i++)
		{
			if (chest.item[i].stack < chest.item[i].maxStack && Item.CanStack(player.inventory[p], chest.item[i]))
			{
				int num = player.inventory[p].stack;
				if (player.inventory[p].stack + chest.item[i].stack > chest.item[i].maxStack)
				{
					num = chest.item[i].maxStack - chest.item[i].stack;
				}
				player.inventory[p].stack -= num;
				chest.item[i].stack += num;
				SoundEngine.PlaySound(7);
				if (player.inventory[p].stack <= 0)
				{
					player.inventory[p].SetDefaults(0);
					break;
				}
				if (chest.item[i].type == 0)
				{
					chest.item[i] = player.inventory[p].Clone();
					player.inventory[p].SetDefaults(0);
				}
			}
		}
	}

	public static void DepositAll()
	{
		Player player = Main.player[Main.myPlayer];
		if (player.chest > -1)
		{
			MoveCoins(player.inventory, Main.chest[player.chest]);
		}
		else if (player.chest == -3)
		{
			MoveCoins(player.inventory, player.bank2);
		}
		else if (player.chest == -4)
		{
			MoveCoins(player.inventory, player.bank3);
		}
		else if (player.chest == -5)
		{
			MoveCoins(player.inventory, player.bank4);
		}
		else
		{
			MoveCoins(player.inventory, player.bank);
		}
		for (int num = 49; num >= 10; num--)
		{
			if (player.inventory[num].stack > 0 && player.inventory[num].type > 0 && !player.inventory[num].favorited)
			{
				if (player.inventory[num].maxStack > 1)
				{
					Chest currentContainer = player.GetCurrentContainer();
					if (player.chest > -1)
					{
						DepositAll_IntoWorldChest(player, currentContainer, num);
					}
					else
					{
						DepositAll_IntoLocalChest(player, currentContainer, num);
					}
				}
				if (player.inventory[num].stack > 0)
				{
					if (player.chest > -1)
					{
						for (int i = 0; i < Main.chest[player.chest].maxItems; i++)
						{
							if (Main.chest[player.chest].item[i].stack == 0)
							{
								SoundEngine.PlaySound(7);
								Main.chest[player.chest].item[i] = player.inventory[num].Clone();
								player.inventory[num].SetDefaults(0);
								ItemSlot.DisplayTransfer_OneWay(player.inventory, 0, num, Main.chest[player.chest].item, 3, i, Main.chest[player.chest].item[i].stack);
								if (Main.netMode == 1)
								{
									NetMessage.SendData(32, -1, -1, null, player.chest, i);
								}
								break;
							}
						}
					}
					else if (player.chest == -3)
					{
						for (int j = 0; j < player.bank2.maxItems; j++)
						{
							if (player.bank2.item[j].stack == 0)
							{
								SoundEngine.PlaySound(7);
								player.bank2.item[j] = player.inventory[num].Clone();
								player.inventory[num].SetDefaults(0);
								ItemSlot.DisplayTransfer_OneWay(player.inventory, 0, num, player.bank2.item, 4, j, player.bank2.item[j].stack);
								break;
							}
						}
					}
					else if (player.chest == -4)
					{
						for (int k = 0; k < player.bank3.maxItems; k++)
						{
							if (player.bank3.item[k].stack == 0)
							{
								SoundEngine.PlaySound(7);
								player.bank3.item[k] = player.inventory[num].Clone();
								player.inventory[num].SetDefaults(0);
								ItemSlot.DisplayTransfer_OneWay(player.inventory, 0, num, player.bank3.item, 4, k, player.bank3.item[k].stack);
								break;
							}
						}
					}
					else if (player.chest == -5)
					{
						for (int l = 0; l < player.bank4.maxItems; l++)
						{
							if (player.bank4.item[l].stack == 0)
							{
								SoundEngine.PlaySound(7);
								player.bank4.item[l] = player.inventory[num].Clone();
								player.inventory[num].SetDefaults(0);
								ItemSlot.DisplayTransfer_OneWay(player.inventory, 0, num, player.bank4.item, 32, l, player.bank4.item[l].stack);
								break;
							}
						}
					}
					else
					{
						for (int m = 0; m < player.bank.maxItems; m++)
						{
							if (player.bank.item[m].stack == 0)
							{
								SoundEngine.PlaySound(7);
								player.bank.item[m] = player.inventory[num].Clone();
								player.inventory[num].SetDefaults(0);
								ItemSlot.DisplayTransfer_OneWay(player.inventory, 0, num, player.bank.item, 4, m, player.bank.item[m].stack);
								break;
							}
						}
					}
				}
				CoinSlot.ForceSlotState(num, 0, player.inventory[num]);
			}
		}
	}

	public static void QuickStack(bool voidStack = false)
	{
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		Player player = Main.player[Main.myPlayer];
		Item[] array = player.inventory;
		if (voidStack)
		{
			array = player.bank4.item;
		}
		_ = player.Center;
		if (!voidStack && player.chest == -5)
		{
			MoveCoins(array, player.bank4);
		}
		else if (player.chest == -4)
		{
			MoveCoins(array, player.bank3);
		}
		else if (player.chest == -3)
		{
			MoveCoins(array, player.bank2);
		}
		else if (player.chest == -2)
		{
			MoveCoins(array, player.bank);
		}
		Chest currentContainer = player.GetCurrentContainer();
		Item[] item = currentContainer.item;
		int toContext = 3;
		if (voidStack || player.chest == -5)
		{
			toContext = 32;
		}
		else if (player.chest < -1)
		{
			toContext = 4;
		}
		List<int> list = new List<int>();
		List<int> list2 = new List<int>();
		List<int> list3 = new List<int>();
		Dictionary<int, int> dictionary = new Dictionary<int, int>();
		List<int> list4 = new List<int>();
		bool[] array2 = new bool[item.Length];
		for (int i = 0; i < currentContainer.maxItems; i++)
		{
			if (item[i].type > 0 && item[i].stack > 0 && (item[i].type < 71 || item[i].type > 74))
			{
				list2.Add(i);
				list.Add(item[i].type);
			}
			if (item[i].type == 0 || item[i].stack <= 0)
			{
				list3.Add(i);
			}
		}
		int num = 50;
		int num2 = 10;
		if (player.chest <= -2)
		{
			num += 4;
		}
		if (voidStack)
		{
			num2 = 0;
			num = player.bank4.maxItems;
		}
		for (int j = num2; j < num; j++)
		{
			if (list.Contains(array[j].type) && !array[j].favorited)
			{
				dictionary.Add(j, array[j].type);
			}
		}
		for (int k = 0; k < list2.Count; k++)
		{
			int num3 = list2[k];
			foreach (KeyValuePair<int, int> item2 in dictionary)
			{
				if (Item.CanStack(array[item2.Key], item[num3]))
				{
					int num4 = array[item2.Key].stack;
					int num5 = item[num3].maxStack - item[num3].stack;
					if (num5 == 0)
					{
						break;
					}
					if (num4 > num5)
					{
						num4 = num5;
					}
					SoundEngine.PlaySound(7);
					item[num3].stack += num4;
					array[item2.Key].stack -= num4;
					if (array[item2.Key].stack == 0)
					{
						array[item2.Key].SetDefaults(0);
					}
					array2[num3] = true;
				}
			}
		}
		foreach (KeyValuePair<int, int> item3 in dictionary)
		{
			if (array[item3.Key].stack == 0)
			{
				list4.Add(item3.Key);
			}
		}
		foreach (int item4 in list4)
		{
			dictionary.Remove(item4);
		}
		for (int l = 0; l < list3.Count; l++)
		{
			int num6 = list3[l];
			bool flag = true;
			foreach (KeyValuePair<int, int> item5 in dictionary)
			{
				if (array[item5.Key].stack == 0 || (!flag && !Item.CanStack(array[item5.Key], item[num6])))
				{
					continue;
				}
				SoundEngine.PlaySound(7);
				if (flag)
				{
					item[num6] = array[item5.Key];
					array[item5.Key] = new Item();
					ItemSlot.DisplayTransfer_OneWay(player.inventory, 0, item5.Key, item, toContext, num6);
				}
				else
				{
					int num7 = array[item5.Key].stack;
					int num8 = item[num6].maxStack - item[num6].stack;
					if (num8 == 0)
					{
						break;
					}
					if (num7 > num8)
					{
						num7 = num8;
					}
					item[num6].stack += num7;
					array[item5.Key].stack -= num7;
					if (array[item5.Key].stack == 0)
					{
						array[item5.Key] = new Item();
					}
					ItemSlot.DisplayTransfer_OneWay(player.inventory, 0, item5.Key, item, toContext, num6);
				}
				array2[num6] = true;
				flag = false;
			}
		}
		if (Main.netMode == 1 && player.chest >= 0)
		{
			for (int m = 0; m < array2.Length; m++)
			{
				NetMessage.SendData(32, -1, -1, null, player.chest, m);
			}
		}
		list.Clear();
		list2.Clear();
		list3.Clear();
		dictionary.Clear();
		list4.Clear();
	}

	public static void RenameChest()
	{
		Player player = Main.player[Main.myPlayer];
		if (!Main.editChest)
		{
			IngameFancyUI.OpenVirtualKeyboard(2);
		}
		else
		{
			RenameChestSubmit(player);
		}
	}

	public static void RenameChestSubmit(Player player)
	{
		SoundEngine.PlaySound(12);
		Main.editChest = false;
		int chest = player.chest;
		if (chest < 0)
		{
			return;
		}
		if (Main.npcChatText == Main.defaultChestName)
		{
			Main.npcChatText = "";
		}
		if (Main.chest[chest].name != Main.npcChatText)
		{
			Main.chest[chest].name = Main.npcChatText;
			if (Main.netMode == 1)
			{
				player.editedChestName = true;
			}
		}
	}

	public static void RenameChestCancel()
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		SoundEngine.PlaySound(12);
		Main.editChest = false;
		Main.npcChatText = string.Empty;
		Main.blockKey = ((object)(Keys)27/*cast due to constrained. prefix*/).ToString();
	}

	public static void Restock()
	{
		Player player = Main.player[Main.myPlayer];
		Item[] inventory = player.inventory;
		Item[] item = player.bank.item;
		if (player.chest > -1)
		{
			item = Main.chest[player.chest].item;
		}
		else if (player.chest == -2)
		{
			item = player.bank.item;
		}
		else if (player.chest == -3)
		{
			item = player.bank2.item;
		}
		else if (player.chest == -4)
		{
			item = player.bank3.item;
		}
		else if (player.chest == -5)
		{
			item = player.bank4.item;
		}
		HashSet<int> hashSet = new HashSet<int>();
		List<int> list = new List<int>();
		List<int> list2 = new List<int>();
		for (int num = 57; num >= 0; num--)
		{
			Item item2 = inventory[num];
			if ((num < 50 || num >= 54) && (item2.type < 71 || item2.type > 74))
			{
				if (item2.stack == 0 || item2.type == 0 || item2.type == 0)
				{
					list2.Add(num);
				}
				else if (item2.maxStack > 1 && (!item2.favorited || !item2.OnlyNeedOneInInventory()))
				{
					hashSet.Add(item2.type);
					if (item2.stack < item2.maxStack)
					{
						list.Add(num);
					}
				}
			}
		}
		bool flag = false;
		for (int i = 0; i < item.Length; i++)
		{
			if (item[i].stack < 1 || item[i].prefix != 0 || !hashSet.Contains(item[i].type))
			{
				continue;
			}
			bool flag2 = false;
			for (int j = 0; j < list.Count; j++)
			{
				int num2 = list[j];
				int context = 0;
				if (num2 >= 50)
				{
					context = 2;
				}
				if (!Item.CanStack(inventory[num2], item[i]) || ItemSlot.PickItemMovementAction(inventory, context, num2, item[i]) == -1)
				{
					continue;
				}
				int num3 = item[i].stack;
				if (inventory[num2].maxStack - inventory[num2].stack < num3)
				{
					num3 = inventory[num2].maxStack - inventory[num2].stack;
				}
				inventory[num2].stack += num3;
				item[i].stack -= num3;
				flag = true;
				if (inventory[num2].stack == inventory[num2].maxStack)
				{
					if (Main.netMode == 1 && Main.player[Main.myPlayer].chest > -1)
					{
						NetMessage.SendData(32, -1, -1, null, Main.player[Main.myPlayer].chest, i);
					}
					list.RemoveAt(j);
					j--;
				}
				if (item[i].stack == 0)
				{
					item[i] = new Item();
					flag2 = true;
					if (Main.netMode == 1 && Main.player[Main.myPlayer].chest > -1)
					{
						NetMessage.SendData(32, -1, -1, null, Main.player[Main.myPlayer].chest, i);
					}
					break;
				}
			}
			if (flag2 || list2.Count <= 0 || item[i].ammo == 0)
			{
				continue;
			}
			for (int k = 0; k < list2.Count; k++)
			{
				int context2 = 0;
				if (list2[k] >= 50)
				{
					context2 = 2;
				}
				if (ItemSlot.PickItemMovementAction(inventory, context2, list2[k], item[i]) != -1)
				{
					Utils.Swap(ref inventory[list2[k]], ref item[i]);
					if (Main.netMode == 1 && Main.player[Main.myPlayer].chest > -1)
					{
						NetMessage.SendData(32, -1, -1, null, Main.player[Main.myPlayer].chest, i);
					}
					list.Add(list2[k]);
					list2.RemoveAt(k);
					flag = true;
					break;
				}
			}
		}
		if (flag)
		{
			SoundEngine.PlaySound(7);
		}
	}

	public static long MoveCoins(Item[] pInv, Chest chest)
	{
		return MoveCoins(pInv, chest.item, chest.maxItems);
	}

	public static long MoveCoins(Item[] pInv, Item[] cInv, int chestMaxItems)
	{
		bool flag = false;
		int[] array = new int[4];
		List<int> list = new List<int>();
		List<int> list2 = new List<int>();
		bool flag2 = false;
		int[] array2 = new int[chestMaxItems];
		long num = Utils.CoinsCount(out var overFlowing, pInv);
		for (int i = 0; i < cInv.Length; i++)
		{
			array2[i] = -1;
			if (cInv[i].stack < 1 || cInv[i].type < 1)
			{
				list2.Add(i);
				cInv[i] = new Item();
			}
			if (cInv[i] != null && cInv[i].stack > 0)
			{
				int num2 = 0;
				if (cInv[i].type == 71)
				{
					num2 = 1;
				}
				if (cInv[i].type == 72)
				{
					num2 = 2;
				}
				if (cInv[i].type == 73)
				{
					num2 = 3;
				}
				if (cInv[i].type == 74)
				{
					num2 = 4;
				}
				array2[i] = num2 - 1;
				if (num2 > 0)
				{
					array[num2 - 1] += cInv[i].stack;
					list2.Add(i);
					cInv[i] = new Item();
					flag2 = true;
				}
			}
		}
		if (!flag2)
		{
			return 0L;
		}
		for (int j = 0; j < pInv.Length; j++)
		{
			if (j != 58 && pInv[j] != null && pInv[j].stack > 0 && !pInv[j].favorited)
			{
				int num3 = 0;
				if (pInv[j].type == 71)
				{
					num3 = 1;
				}
				if (pInv[j].type == 72)
				{
					num3 = 2;
				}
				if (pInv[j].type == 73)
				{
					num3 = 3;
				}
				if (pInv[j].type == 74)
				{
					num3 = 4;
				}
				if (num3 > 0)
				{
					flag = true;
					array[num3 - 1] += pInv[j].stack;
					list.Add(j);
					pInv[j] = new Item();
				}
			}
		}
		for (int k = 0; k < 3; k++)
		{
			while (array[k] >= 100)
			{
				array[k] -= 100;
				array[k + 1]++;
			}
		}
		for (int l = 0; l < chestMaxItems; l++)
		{
			if (array2[l] < 0 || cInv[l].type != 0)
			{
				continue;
			}
			int num4 = l;
			int num5 = array2[l];
			if (array[num5] > 0)
			{
				cInv[num4].SetDefaults(71 + num5);
				cInv[num4].stack = array[num5];
				if (cInv[num4].stack > cInv[num4].maxStack)
				{
					cInv[num4].stack = cInv[num4].maxStack;
				}
				array[num5] -= cInv[num4].stack;
				array2[l] = -1;
			}
			if (Main.netMode == 1 && Main.player[Main.myPlayer].chest > -1)
			{
				NetMessage.SendData(32, -1, -1, null, Main.player[Main.myPlayer].chest, num4);
			}
			list2.Remove(num4);
		}
		for (int m = 0; m < chestMaxItems; m++)
		{
			if (array2[m] < 0 || cInv[m].type != 0)
			{
				continue;
			}
			int num6 = m;
			int num7 = 3;
			while (num7 >= 0)
			{
				if (array[num7] > 0)
				{
					cInv[num6].SetDefaults(71 + num7);
					cInv[num6].stack = array[num7];
					if (cInv[num6].stack > cInv[num6].maxStack)
					{
						cInv[num6].stack = cInv[num6].maxStack;
					}
					array[num7] -= cInv[num6].stack;
					array2[m] = -1;
					break;
				}
				if (array[num7] == 0)
				{
					num7--;
				}
			}
			if (Main.netMode == 1 && Main.player[Main.myPlayer].chest > -1)
			{
				NetMessage.SendData(32, -1, -1, null, Main.player[Main.myPlayer].chest, num6);
			}
			list2.Remove(num6);
		}
		while (list2.Count > 0)
		{
			int num8 = list2[0];
			int num9 = 3;
			while (num9 >= 0)
			{
				if (array[num9] > 0)
				{
					cInv[num8].SetDefaults(71 + num9);
					cInv[num8].stack = array[num9];
					if (cInv[num8].stack > cInv[num8].maxStack)
					{
						cInv[num8].stack = cInv[num8].maxStack;
					}
					array[num9] -= cInv[num8].stack;
					break;
				}
				if (array[num9] == 0)
				{
					num9--;
				}
			}
			if (Main.netMode == 1 && Main.player[Main.myPlayer].chest > -1)
			{
				NetMessage.SendData(32, -1, -1, null, Main.player[Main.myPlayer].chest, list2[0]);
			}
			list2.RemoveAt(0);
		}
		int num10 = 3;
		while (num10 >= 0 && list.Count > 0)
		{
			int num11 = list[0];
			if (array[num10] > 0)
			{
				pInv[num11].SetDefaults(71 + num10);
				pInv[num11].stack = array[num10];
				if (pInv[num11].stack > pInv[num11].maxStack)
				{
					pInv[num11].stack = pInv[num11].maxStack;
				}
				array[num10] -= pInv[num11].stack;
				flag = false;
				list.RemoveAt(0);
			}
			if (array[num10] == 0)
			{
				num10--;
			}
		}
		if (flag)
		{
			SoundEngine.PlaySound(7);
		}
		long num12 = Utils.CoinsCount(out var overFlowing2, pInv);
		if (overFlowing | overFlowing2)
		{
			return 0L;
		}
		return num - num12;
	}

	public static bool TryPlacingInChest(Item[] inv, int slot, bool justCheck, int itemSlotContext)
	{
		Item item = inv[slot];
		bool flag = Main.LocalPlayer.chest > -1 && Main.netMode == 1;
		Chest currentContainer = Main.LocalPlayer.GetCurrentContainer();
		Item[] item2 = currentContainer.item;
		if (IsBlockedFromTransferIntoChest(item, item2))
		{
			return false;
		}
		Player player = Main.player[Main.myPlayer];
		bool flag2 = false;
		if (item.maxStack > 1)
		{
			for (int i = 0; i < currentContainer.maxItems; i++)
			{
				if (item2[i].stack >= item2[i].maxStack || !Item.CanStack(item, item2[i]))
				{
					continue;
				}
				int num = item.stack;
				if (item.stack + item2[i].stack > item2[i].maxStack)
				{
					num = item2[i].maxStack - item2[i].stack;
				}
				if (justCheck)
				{
					flag2 = flag2 || num > 0;
					break;
				}
				item.stack -= num;
				item2[i].stack += num;
				SoundEngine.PlaySound(7);
				if (item.stack <= 0)
				{
					item.SetDefaults(0);
					if (flag)
					{
						NetMessage.SendData(32, -1, -1, null, player.chest, i);
					}
					break;
				}
				if (item2[i].type == 0)
				{
					item2[i] = item.Clone();
					item.SetDefaults(0);
				}
				if (flag)
				{
					NetMessage.SendData(32, -1, -1, null, player.chest, i);
				}
			}
		}
		if (item.stack > 0)
		{
			int toContext = 3;
			for (int j = 0; j < currentContainer.maxItems; j++)
			{
				if (item2[j].stack != 0)
				{
					continue;
				}
				if (justCheck)
				{
					flag2 = true;
					break;
				}
				SoundEngine.PlaySound(7);
				item2[j] = item.Clone();
				item.SetDefaults(0);
				ItemSlot.AnnounceTransfer(new ItemSlot.ItemTransferInfo(item2[j], itemSlotContext, toContext));
				ItemSlot.DisplayTransfer_OneWay(inv, itemSlotContext, slot, item2, toContext, j, item2[j].stack);
				if (flag)
				{
					NetMessage.SendData(32, -1, -1, null, player.chest, j);
				}
				break;
			}
		}
		return flag2;
	}

	public static bool IsBlockedFromTransferIntoChest(Item item, Item[] container)
	{
		if (item.type == 3213 && item.favorited && container == Main.LocalPlayer.bank.item)
		{
			return true;
		}
		if ((item.type == 4131 || item.type == 5325) && item.favorited && container == Main.LocalPlayer.bank4.item)
		{
			return true;
		}
		return false;
	}
}
