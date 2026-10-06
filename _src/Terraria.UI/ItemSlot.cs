using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Achievements;
using Terraria.GameContent.Creative;
using Terraria.GameContent.Tile_Entities;
using Terraria.GameContent.UI;
using Terraria.GameContent.UI.Chat;
using Terraria.GameInput;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.Initializers;
using Terraria.Localization;
using Terraria.Testing;
using Terraria.UI.Chat;
using Terraria.UI.Gamepad;

namespace Terraria.UI;

public class ItemSlot
{
	public class Options
	{
		public static bool DisableLeftShiftTrashCan = true;

		public static bool DisableQuickTrash = false;

		public static bool HighlightNewItems = true;
	}

	public class Context
	{
		public const int InventoryItem = 0;

		public const int InventoryCoin = 1;

		public const int InventoryAmmo = 2;

		public const int ChestItem = 3;

		public const int BankItem = 4;

		public const int PrefixItem = 5;

		public const int TrashItem = 6;

		public const int GuideItem = 7;

		public const int EquipArmor = 8;

		public const int EquipArmorVanity = 9;

		public const int EquipAccessory = 10;

		public const int EquipAccessoryVanity = 11;

		public const int EquipDye = 12;

		public const int HotbarItem = 13;

		public const int ChatItem = 14;

		public const int ShopItem = 15;

		public const int EquipGrapple = 16;

		public const int EquipMount = 17;

		public const int EquipMinecart = 18;

		public const int EquipPet = 19;

		public const int EquipLight = 20;

		public const int MouseItem = 21;

		public const int CraftingMaterial = 22;

		public const int DisplayDollArmor = 23;

		public const int DisplayDollAccessory = 24;

		public const int DisplayDollDye = 25;

		public const int HatRackHat = 26;

		public const int HatRackDye = 27;

		public const int GoldDebug = 28;

		public const int CreativeInfinite = 29;

		public const int CreativeSacrifice = 30;

		public const int InWorld = 31;

		public const int VoidItem = 32;

		public const int EquipMiscDye = 33;

		public const int CreativeInfiniteLocked = 34;

		public const int BannerClaiming = 35;

		public const int HotbarItemSmartSelected = 36;

		public const int OverdrawGlow = 37;

		public const int DisplayDollWeapon = 38;

		public const int DisplayDollMount = 39;

		public const int InWorldDisplay = 40;

		public const int NewCraftingUIRecipe = 41;

		public const int NewCraftingUICraftSlot = 42;

		public const int NewCraftingUIMaterial = 43;

		public static readonly int Count = 44;
	}

	public struct ItemDisplayKey
	{
		public int Context;

		public int Slot;

		public bool Equals(ItemDisplayKey other)
		{
			if (Context == other.Context)
			{
				return Slot == other.Slot;
			}
			return false;
		}

		public override bool Equals(object obj)
		{
			if (obj is ItemDisplayKey)
			{
				return Equals((ItemDisplayKey)obj);
			}
			return false;
		}

		public override int GetHashCode()
		{
			return (Context * 397) ^ Slot;
		}

		public static bool operator ==(ItemDisplayKey left, ItemDisplayKey right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(ItemDisplayKey left, ItemDisplayKey right)
		{
			return !left.Equals(right);
		}
	}

	public struct AlternateClickAction(int cursorOverride, LocalizedText gamepadHintText)
	{
		public readonly int cursorOverride = cursorOverride;

		public readonly LocalizedText gamepadHintText = gamepadHintText;

		public static AlternateClickAction Trash = new AlternateClickAction(6, Lang.misc[74]);

		public static AlternateClickAction TransferToBackpack = new AlternateClickAction(7, Lang.misc[76]);

		public static AlternateClickAction Unequip = new AlternateClickAction(7, Lang.misc[68]);

		public static AlternateClickAction TransferFromChest = new AlternateClickAction(8, Lang.misc[76]);

		public static AlternateClickAction TransferToChest = new AlternateClickAction(9, Lang.misc[76]);

		public static AlternateClickAction Sell = new AlternateClickAction(10, Lang.misc[75]);

		public static AlternateClickAction? GetSellOrTrash(Item item)
		{
			if (Main.npcShop <= 0)
			{
				return Trash;
			}
			if (item.type >= 71 && item.type <= 74)
			{
				return null;
			}
			return Sell;
		}
	}

	public struct ItemTransferInfo
	{
		public int ItemType;

		public int TransferAmount;

		public int FromContenxt;

		public int ToContext;

		public ItemTransferInfo(Item itemAfter, int fromContext, int toContext, int transferAmount = 0)
		{
			ItemType = itemAfter.type;
			TransferAmount = itemAfter.stack;
			if (transferAmount != 0)
			{
				TransferAmount = transferAmount;
			}
			FromContenxt = fromContext;
			ToContext = toContext;
		}
	}

	public delegate void ItemTransferEvent(ItemTransferInfo info);

	public struct PulseEffect(PlayerItemSlotID.SlotReference slotRef, Color color)
	{
		public static readonly int EffectDuration = 40;

		public static readonly int NumPulses = 2;

		public readonly Color color = color;

		public readonly PlayerItemSlotID.SlotReference slotRef = slotRef;

		public readonly Item itemInSlot = slotRef.Item;

		public int time = 0;

		public bool IsActive => itemInSlot != null;
	}

	public delegate void ItemPickupAction<TItemInfo>(TItemInfo info, int stackToGet);

	private static Dictionary<ItemDisplayKey, ulong> _nextTickDrawAvailable;

	public static bool DrawGoldBGForCraftingMaterial;

	public static bool DrawSelectionHighlightForGridSlot;

	public static bool DrawNewAndShinyHighlightOverride;

	public static bool ShiftForcedOn;

	private static Item[] singleSlotArray;

	private static bool[] canFavoriteAt;

	private static bool[] retainFavoriteOnMoveAt;

	public static bool[] canLoadoutShareAt;

	private static bool[] canShareAt;

	private static bool[] canQuickDropAt;

	private static bool[] canItemSwapAt;

	private static bool[] hideNewAndShinyHighlightAt;

	private static float[] inventoryGlowHue;

	private static int[] inventoryGlowTime;

	private static float[] inventoryGlowHueChest;

	private static int[] inventoryGlowTimeChest;

	private static PulseEffect[] playerSlotPulseEffects;

	private static int _customCurrencyForSavings;

	public static bool forceClearGlowsOnChest;

	private static double _lastTimeForVisualEffectsThatLoadoutWasChanged;

	private static Color[,] LoadoutSlotColors;

	public static float OverdrawGlowSize;

	public static Color OverdrawGlowColorMultiplier;

	private static int dyeSwapCounter;

	private static Item[] _dirtyHack;

	public static float CircularRadialOpacity;

	public static float QuicksRadialOpacity;

	public static bool ShiftInUse
	{
		get
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			if (!Main.keyState.PressingShift())
			{
				return ShiftForcedOn;
			}
			return true;
		}
	}

	public static bool ControlInUse
	{
		get
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			return Main.keyState.PressingControl();
		}
	}

	public static event ItemTransferEvent OnItemTransferred;

	static ItemSlot()
	{
		//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		//IL_0111: Unknown result type (might be due to invalid IL or missing references)
		//IL_0116: Unknown result type (might be due to invalid IL or missing references)
		//IL_0124: Unknown result type (might be due to invalid IL or missing references)
		//IL_0129: Unknown result type (might be due to invalid IL or missing references)
		//IL_013a: Unknown result type (might be due to invalid IL or missing references)
		//IL_013f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0150: Unknown result type (might be due to invalid IL or missing references)
		//IL_0155: Unknown result type (might be due to invalid IL or missing references)
		//IL_0163: Unknown result type (might be due to invalid IL or missing references)
		//IL_0168: Unknown result type (might be due to invalid IL or missing references)
		//IL_0176: Unknown result type (might be due to invalid IL or missing references)
		//IL_017b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0189: Unknown result type (might be due to invalid IL or missing references)
		//IL_018e: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a7: Unknown result type (might be due to invalid IL or missing references)
		_nextTickDrawAvailable = new Dictionary<ItemDisplayKey, ulong>();
		DrawGoldBGForCraftingMaterial = false;
		DrawSelectionHighlightForGridSlot = false;
		DrawNewAndShinyHighlightOverride = false;
		singleSlotArray = new Item[1];
		canFavoriteAt = new bool[Context.Count];
		retainFavoriteOnMoveAt = new bool[Context.Count];
		canLoadoutShareAt = new bool[Context.Count];
		canShareAt = new bool[Context.Count];
		canQuickDropAt = new bool[Context.Count];
		canItemSwapAt = new bool[Context.Count];
		hideNewAndShinyHighlightAt = new bool[Context.Count];
		inventoryGlowHue = new float[58];
		inventoryGlowTime = new int[58];
		inventoryGlowHueChest = new float[58];
		inventoryGlowTimeChest = new int[58];
		playerSlotPulseEffects = new PulseEffect[PlayerItemSlotID.Count];
		_customCurrencyForSavings = -1;
		forceClearGlowsOnChest = false;
		LoadoutSlotColors = new Color[3, 3]
		{
			{
				new Color(50, 106, 64),
				new Color(46, 106, 98),
				new Color(45, 85, 105)
			},
			{
				new Color(35, 106, 126),
				new Color(50, 89, 140),
				new Color(57, 70, 128)
			},
			{
				new Color(122, 63, 83),
				new Color(104, 46, 85),
				new Color(84, 37, 87)
			}
		};
		OverdrawGlowSize = 1f;
		OverdrawGlowColorMultiplier = Color.White;
		_dirtyHack = new Item[0];
		canFavoriteAt[0] = true;
		canFavoriteAt[1] = true;
		canFavoriteAt[2] = true;
		canFavoriteAt[32] = true;
		retainFavoriteOnMoveAt[0] = true;
		retainFavoriteOnMoveAt[1] = true;
		retainFavoriteOnMoveAt[2] = true;
		retainFavoriteOnMoveAt[32] = true;
		retainFavoriteOnMoveAt[5] = true;
		canLoadoutShareAt[8] = true;
		canLoadoutShareAt[9] = true;
		canLoadoutShareAt[10] = true;
		canLoadoutShareAt[11] = true;
		canLoadoutShareAt[12] = true;
		canShareAt[0] = true;
		canShareAt[1] = true;
		canShareAt[2] = true;
		canShareAt[32] = true;
		canShareAt[15] = true;
		canShareAt[4] = true;
		canShareAt[32] = true;
		canShareAt[5] = true;
		canShareAt[6] = true;
		canShareAt[7] = true;
		canShareAt[27] = true;
		canShareAt[26] = true;
		canShareAt[23] = true;
		canShareAt[24] = true;
		canShareAt[39] = true;
		canShareAt[25] = true;
		canShareAt[38] = true;
		canShareAt[22] = true;
		canShareAt[35] = true;
		canShareAt[3] = true;
		canShareAt[8] = true;
		canShareAt[9] = true;
		canShareAt[10] = true;
		canShareAt[11] = true;
		canShareAt[12] = true;
		canShareAt[33] = true;
		canShareAt[16] = true;
		canShareAt[20] = true;
		canShareAt[18] = true;
		canShareAt[19] = true;
		canShareAt[17] = true;
		canShareAt[29] = true;
		canShareAt[34] = true;
		canShareAt[30] = true;
		canShareAt[41] = true;
		canShareAt[42] = true;
		canShareAt[43] = true;
		canQuickDropAt[0] = true;
		canQuickDropAt[1] = true;
		canQuickDropAt[2] = true;
		canQuickDropAt[6] = true;
		canQuickDropAt[15] = true;
		canQuickDropAt[4] = true;
		canQuickDropAt[32] = true;
		canQuickDropAt[3] = true;
		hideNewAndShinyHighlightAt[13] = true;
		hideNewAndShinyHighlightAt[21] = true;
		hideNewAndShinyHighlightAt[37] = true;
		hideNewAndShinyHighlightAt[14] = true;
		hideNewAndShinyHighlightAt[22] = true;
		hideNewAndShinyHighlightAt[35] = true;
		canItemSwapAt[0] = true;
		canItemSwapAt[4] = true;
		canItemSwapAt[32] = true;
		canItemSwapAt[3] = true;
		canItemSwapAt[10] = true;
		canItemSwapAt[8] = true;
	}

	public static void AnnounceTransfer(ItemTransferInfo info)
	{
		if (OnItemTransferred != null)
		{
			OnItemTransferred(info);
		}
	}

	public static void PrepareForChest(Chest chest)
	{
		int maxItems = chest.maxItems;
		if (inventoryGlowTimeChest.Length < maxItems)
		{
			Array.Resize(ref inventoryGlowTimeChest, maxItems);
		}
		if (inventoryGlowHueChest.Length < maxItems)
		{
			Array.Resize(ref inventoryGlowHueChest, maxItems);
		}
	}

	public static void SetGlowForChest(Chest chest)
	{
		PrepareForChest(chest);
		int maxItems = chest.maxItems;
		for (int i = 0; i < maxItems; i++)
		{
			SetGlow(i, -1f, chest: true);
		}
		for (int j = 0; j < maxItems; j++)
		{
			CoinSlot.ForceSlotState(j, 3, chest.item[j]);
		}
	}

	public static void SetGlow(int index, float hue, bool chest)
	{
		if (chest)
		{
			if (hue < 0f)
			{
				inventoryGlowTimeChest[index] = 0;
				inventoryGlowHueChest[index] = 0f;
			}
			else
			{
				inventoryGlowTimeChest[index] = 300;
				inventoryGlowHueChest[index] = hue;
			}
		}
		else
		{
			inventoryGlowTime[index] = 300;
			inventoryGlowHue[index] = hue;
		}
	}

	public static void UpdateInterface()
	{
		if (!Main.playerInventory || Main.player[Main.myPlayer].talkNPC == -1)
		{
			_customCurrencyForSavings = -1;
		}
		for (int i = 0; i < inventoryGlowTime.Length; i++)
		{
			if (inventoryGlowTime[i] > 0)
			{
				inventoryGlowTime[i]--;
				if (inventoryGlowTime[i] == 0)
				{
					inventoryGlowHue[i] = 0f;
				}
			}
		}
		for (int j = 0; j < inventoryGlowTimeChest.Length; j++)
		{
			if (inventoryGlowTimeChest[j] > 0)
			{
				inventoryGlowTimeChest[j]--;
				if (inventoryGlowTimeChest[j] == 0 || forceClearGlowsOnChest)
				{
					inventoryGlowHueChest[j] = 0f;
				}
			}
		}
		forceClearGlowsOnChest = false;
		for (int k = 0; k < playerSlotPulseEffects.Length; k++)
		{
			PulseEffect pulseEffect = playerSlotPulseEffects[k];
			if (pulseEffect.itemInSlot != null && (++playerSlotPulseEffects[k].time >= PulseEffect.EffectDuration || pulseEffect.slotRef.Item.IsAir))
			{
				playerSlotPulseEffects[k] = default;
			}
		}
	}

	public static void IndicateBlockedSlot(PlayerItemSlotID.SlotReference slot)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		AddPulseEffect(slot, new Color(250, 40, 40, 255));
	}

	public static void AddPulseEffect(PlayerItemSlotID.SlotReference slot, Color color)
	{
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		PulseEffect pulseEffect = new PulseEffect(slot, color);
		if (!pulseEffect.itemInSlot.IsAir)
		{
			playerSlotPulseEffects[slot.SlotId] = pulseEffect;
		}
	}

	public static void Handle(ref Item inv, int context = 0, bool allowInteract = true)
	{
		singleSlotArray[0] = inv;
		Handle(singleSlotArray, context, 0, allowInteract);
		inv = singleSlotArray[0];
	}

	public static bool HoverOverrideClick(Item inv, int context = 0)
	{
		singleSlotArray[0] = inv;
		OverrideHover(singleSlotArray, context);
		if (Main.cursorOverride >= 0 && Main.mouseLeftRelease && Main.mouseLeft)
		{
			OverrideLeftClick(singleSlotArray, context);
			return true;
		}
		return false;
	}

	public static void Handle(Item[] inv, int context = 0, int slot = 0, bool allowInteract = true)
	{
		OverrideHover(inv, context, slot);
		if (allowInteract)
		{
			LeftClick(inv, context, slot);
			RightClick(inv, context, slot);
		}
		MouseHover(inv, context, slot);
	}

	public static void OverrideHover(Item[] inv, int context = 0, int slot = 0)
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		if (!PlayerInput.UsingGamepad)
		{
			UILinkPointNavigator.SuggestUsage(GetGamepadPointForSlot(inv, context, slot));
		}
		if (inv[slot].IsAir)
		{
			return;
		}
		if (Main.keyState.IsKeyDown(Main.FavoriteKey))
		{
			if (Main.drawingPlayerChat && canShareAt[context])
			{
				Main.cursorOverride = 2;
				return;
			}
			if (canFavoriteAt[context] || canLoadoutShareAt[context])
			{
				Main.cursorOverride = 3;
				return;
			}
		}
		AlternateClickAction? alternateClickAction = GetAlternateClickAction(inv, context, slot);
		if (alternateClickAction.HasValue)
		{
			Main.cursorOverride = alternateClickAction.Value.cursorOverride;
		}
	}

	public static AlternateClickAction? GetAlternateClickAction(Item[] inv, int context, int slot)
	{
		Item item = inv[slot];
		if (item.IsAir || item.favorited)
		{
			return null;
		}
		bool flag = Options.DisableLeftShiftTrashCan && !PlayerInput.UsingGamepad;
		if ((ControlInUse && !Options.DisableQuickTrash) & flag)
		{
			if ((uint)context <= 4u || context == 7 || context == 32)
			{
				return AlternateClickAction.GetSellOrTrash(item);
			}
		}
		else if (ShiftInUse)
		{
			if (Main.LocalPlayer.tileEntityAnchor.IsInValidUseTileEntity())
			{
				AlternateClickAction? shiftClickAction = Main.LocalPlayer.tileEntityAnchor.GetTileEntity().GetShiftClickAction(inv, context, slot);
				if (shiftClickAction.HasValue)
				{
					return shiftClickAction;
				}
			}
			switch (context)
			{
			case 0:
			case 1:
			case 2:
				if (Main.CreativeMenu.IsShowingResearchMenu())
				{
					if (context == 0)
					{
						return AlternateClickAction.TransferToChest;
					}
				}
				else if (Main.InReforgeMenu)
				{
					if (context == 0 && item.CanHavePrefixes())
					{
						return AlternateClickAction.TransferToChest;
					}
				}
				else if (Main.InGuideCraftMenu)
				{
					if (context == 0 && item.material)
					{
						return AlternateClickAction.TransferToChest;
					}
				}
				else if (Main.player[Main.myPlayer].chest != -1)
				{
					if (ChestUI.TryPlacingInChest(inv, slot, justCheck: true, context))
					{
						return AlternateClickAction.TransferToChest;
					}
				}
				else if (!Options.DisableQuickTrash && !flag)
				{
					return AlternateClickAction.GetSellOrTrash(item);
				}
				break;
			case 3:
			case 4:
			case 32:
				if (Main.player[Main.myPlayer].ItemSpace(item).CanTakeItemToPersonalInventory)
				{
					return AlternateClickAction.TransferFromChest;
				}
				break;
			case 8:
			case 9:
			case 10:
			case 11:
			case 12:
			case 16:
			case 17:
			case 18:
			case 19:
			case 20:
			case 33:
				if (Main.player[Main.myPlayer].ItemSpace(inv[slot]).CanTakeItemToPersonalInventory)
				{
					return AlternateClickAction.Unequip;
				}
				break;
			case 5:
			case 6:
			case 7:
			case 29:
				if (Main.player[Main.myPlayer].ItemSpace(inv[slot]).CanTakeItemToPersonalInventory)
				{
					return AlternateClickAction.TransferToBackpack;
				}
				break;
			}
		}
		return null;
	}

	private static bool OverrideLeftClick(Item[] inv, int context = 0, int slot = 0)
	{
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		if (Main.LocalPlayer.tileEntityAnchor.IsInValidUseTileEntity() && ShiftInUse && Main.LocalPlayer.tileEntityAnchor.GetTileEntity().PerformShiftClickAction(inv, context, slot))
		{
			return true;
		}
		Item item = inv[slot];
		if (Main.cursorOverride == 2)
		{
			if (ChatManager.AddChatText(FontAssets.MouseText.Value, ItemTagHandler.GenerateTag(item), Vector2.One))
			{
				SoundEngine.PlaySound(12);
			}
			return true;
		}
		if (Main.cursorOverride == 3)
		{
			if (canFavoriteAt[context])
			{
				ToggleFavorited(item);
				return true;
			}
			if (canLoadoutShareAt[context])
			{
				ToggleLoadoutShare(inv, context, slot);
				return true;
			}
			return false;
		}
		if (Main.cursorOverride == 6)
		{
			Item item2 = item.DeepClone();
			if (!(TryResearchingItem(ref item2, onlySacrificeIfItWouldFinishResearch: true) | TryResearchingItem(ref Main.LocalPlayer.trashItem)))
			{
				SoundEngine.PlaySound(SoundID.TrashItem);
			}
			SoundEngine.PlaySound(7);
			Main.LocalPlayer.trashItem = item.Clone();
			AnnounceTransfer(new ItemTransferInfo(Main.LocalPlayer.trashItem, context, 6));
			item.TurnToAir();
			if (context == 3 && Main.netMode == 1)
			{
				NetMessage.SendData(32, -1, -1, null, Main.LocalPlayer.chest, slot);
			}
			CoinSlot.ForceSlotState(slot, context, inv[slot]);
			return true;
		}
		if (Main.cursorOverride == 7)
		{
			if (context == 29)
			{
				Item item3 = new Item();
				item3.SetDefaults(inv[slot].type);
				item3.stack = (item3.OnlyNeedOneInInventory() ? 1 : item3.maxStack);
				item3.OnCreated(new JourneyDuplicationItemCreationContext());
				Player.GetItemLogger.Start();
				item3 = Main.LocalPlayer.GetItem(item3, GetItemSettings.QuickTransferFromSlot);
				Player.GetItemLogger.Stop();
				DisplayTransfer_GetItem(inv, context, slot);
				SoundEngine.PlaySound(12);
				return true;
			}
			Player.GetItemLogger.Start();
			inv[slot] = Main.LocalPlayer.GetItem(inv[slot], GetItemSettings.QuickTransferFromSlot);
			Player.GetItemLogger.Stop();
			DisplayTransfer_GetItem(inv, context, slot);
			CoinSlot.ForceSlotState(slot, context, inv[slot]);
			SoundEngine.PlaySound(12);
			return true;
		}
		if (Main.cursorOverride == 8)
		{
			Player.GetItemLogger.Start();
			inv[slot] = Main.LocalPlayer.GetItem(inv[slot], GetItemSettings.QuickTransferFromSlot);
			Player.GetItemLogger.Stop();
			DisplayTransfer_GetItem(inv, context, slot);
			if (Main.player[Main.myPlayer].chest > -1)
			{
				NetMessage.SendData(32, -1, -1, null, Main.player[Main.myPlayer].chest, slot);
			}
			CoinSlot.ForceSlotState(slot, context, inv[slot]);
			return true;
		}
		if (Main.cursorOverride == 9)
		{
			if (Main.CreativeMenu.IsShowingResearchMenu())
			{
				Main.CreativeMenu.SwapItem(ref inv[slot]);
				SoundEngine.PlaySound(7);
				Main.CreativeMenu.SacrificeItemInSacrificeSlot();
			}
			else if (Main.InReforgeMenu)
			{
				if (item.stack <= 1)
				{
					Utils.Swap(ref inv[slot], ref Main.reforgeItem);
					DisplayTransfer_TwoWay(inv, slot, context, Main.reforgeItem, 5);
					SoundEngine.PlaySound(7);
				}
				else if (Main.reforgeItem.IsAir)
				{
					Main.reforgeItem = item.Clone();
					Main.reforgeItem.stack = 1;
					item.stack--;
					DisplayTransfer_TwoWay(inv, slot, context, Main.reforgeItem, 5);
					SoundEngine.PlaySound(7);
				}
			}
			else if (Main.InGuideCraftMenu)
			{
				Utils.Swap(ref inv[slot], ref Main.guideItem);
				DisplayTransfer_TwoWay(inv, slot, context, Main.guideItem, 7);
				SoundEngine.PlaySound(7);
			}
			else
			{
				ChestUI.TryPlacingInChest(inv, slot, justCheck: false, context);
			}
			CoinSlot.ForceSlotState(slot, context, inv[slot]);
			return true;
		}
		if (Main.cursorOverride == 10)
		{
			Chest chest = Main.instance.shop[Main.npcShop];
			if (Main.LocalPlayer.SellItem(item))
			{
				chest.AddItemToShop(item);
				AnnounceTransfer(new ItemTransferInfo(item, context, 15));
				item.TurnToAir();
				SoundEngine.PlaySound(18);
			}
			else if (item.value == 0)
			{
				chest.AddItemToShop(item);
				AnnounceTransfer(new ItemTransferInfo(item, context, 15));
				item.TurnToAir();
				SoundEngine.PlaySound(7);
			}
			return true;
		}
		return false;
	}

	private static void ToggleFavorited(Item item)
	{
		item.favorited = !item.favorited;
		SoundEngine.PlaySound(12);
	}

	private static void ToggleLoadoutShare(Item[] inv, int context, int slot)
	{
		Item obj = inv[slot];
		obj.favorited = !obj.favorited;
		SoundEngine.PlaySound(12);
		if (obj.favorited)
		{
			Player.Settings.DisableLoadoutShareHint();
			ClearSlotShareInOtherLoadouts(inv, context, slot);
		}
	}

	private static void ClearSlotShareInOtherLoadouts(Item[] inv, int context, int slot)
	{
		EquipmentLoadout[] loadouts = Main.LocalPlayer.Loadouts;
		foreach (EquipmentLoadout equipmentLoadout in loadouts)
		{
			Item[] array;
			if (inv == Main.LocalPlayer.armor)
			{
				array = equipmentLoadout.Armor;
			}
			else
			{
				if (inv != Main.LocalPlayer.dye)
				{
					Invariant.Assert(condition: false, "Invalid inventory for loadout share action");
					break;
				}
				array = equipmentLoadout.Dye;
			}
			array[slot].favorited = false;
		}
	}

	public static void LeftClick(Item[] inv, int context = 0, int slot = 0)
	{
		if (Main.LocalPlayerHasPendingInventoryActions())
		{
			return;
		}
		Player player = Main.player[Main.myPlayer];
		inv[slot].newAndShiny = false;
		bool flag = Main.mouseLeftRelease && Main.mouseLeft;
		if (flag && (OverrideLeftClick(inv, context, slot) || player.itemAnimation != 0 || player.itemTime != 0))
		{
			return;
		}
		int num = PickItemMovementAction(inv, context, slot, Main.mouseItem);
		if (num != 3 && !flag)
		{
			return;
		}
		switch (num)
		{
		case 0:
		{
			bool flag2 = false;
			if (context == 6 && Main.mouseItem.type != 0)
			{
				Item item = Main.mouseItem.DeepClone();
				if (TryResearchingItem(ref item, onlySacrificeIfItWouldFinishResearch: true) | TryResearchingItem(inv, slot))
				{
					flag2 = true;
				}
				inv[slot].SetDefaults(0);
			}
			MouseItemSwap(ref inv[slot], context);
			if (inv[slot].stack > 0)
			{
				AnnounceTransfer(new ItemTransferInfo(inv[slot], 21, context, inv[slot].stack));
			}
			else
			{
				AnnounceTransfer(new ItemTransferInfo(Main.mouseItem, context, 21, Main.mouseItem.stack));
			}
			if (inv[slot].stack > 0)
			{
				switch (context)
				{
				case 0:
					AchievementsHelper.NotifyItemPickup(player, inv[slot]);
					break;
				case 8:
				case 9:
				case 10:
				case 11:
				case 12:
				case 16:
				case 17:
				case 25:
				case 27:
				case 33:
					AchievementsHelper.HandleOnEquip(player, inv[slot], context);
					break;
				}
			}
			if (inv[slot].type == 0 || inv[slot].stack < 1)
			{
				inv[slot] = new Item();
			}
			if (Item.CanStack(Main.mouseItem, inv[slot]))
			{
				Utils.Swap(ref inv[slot].favorited, ref Main.mouseItem.favorited);
				if (inv[slot].stack != inv[slot].maxStack && Main.mouseItem.stack != Main.mouseItem.maxStack)
				{
					if (Main.mouseItem.stack + inv[slot].stack <= Main.mouseItem.maxStack)
					{
						inv[slot].stack += Main.mouseItem.stack;
						Main.mouseItem.stack = 0;
						AnnounceTransfer(new ItemTransferInfo(inv[slot], 21, context, inv[slot].stack));
					}
					else
					{
						int num2 = Main.mouseItem.maxStack - inv[slot].stack;
						inv[slot].stack += num2;
						Main.mouseItem.stack -= num2;
						AnnounceTransfer(new ItemTransferInfo(inv[slot], 21, context, num2));
					}
				}
			}
			if (Main.mouseItem.type == 0 || Main.mouseItem.stack < 1)
			{
				Main.mouseItem = new Item();
			}
			if (Main.mouseItem.type > 0 || inv[slot].type > 0)
			{
				if (context == 6 && Main.mouseItem.type == 0 && !flag2)
				{
					SoundEngine.PlaySound(SoundID.TrashItem);
				}
				SoundEngine.PlaySound(7);
			}
			if (context == 3 && Main.netMode == 1)
			{
				NetMessage.SendData(32, -1, -1, null, player.chest, slot);
			}
			CoinSlot.ForceSlotState(slot, context, inv[slot]);
			break;
		}
		case 1:
			if (Main.mouseItem.stack == 1 && Main.mouseItem.type > 0 && inv[slot].type > 0 && inv[slot].IsNotTheSameAs(Main.mouseItem))
			{
				MouseItemSwap(ref inv[slot], context);
				SoundEngine.PlaySound(7);
				if (inv[slot].stack > 0)
				{
					switch (context)
					{
					case 0:
						AchievementsHelper.NotifyItemPickup(player, inv[slot]);
						break;
					case 8:
					case 9:
					case 10:
					case 11:
					case 12:
					case 16:
					case 17:
					case 33:
						AchievementsHelper.HandleOnEquip(player, inv[slot], context);
						break;
					}
				}
			}
			else if (Main.mouseItem.type == 0 && inv[slot].type > 0)
			{
				MouseItemSwap(ref inv[slot], context);
				if (inv[slot].type == 0 || inv[slot].stack < 1)
				{
					inv[slot] = new Item();
				}
				if (Main.mouseItem.type == 0 || Main.mouseItem.stack < 1)
				{
					Main.mouseItem = new Item();
				}
				if (Main.mouseItem.type > 0 || inv[slot].type > 0)
				{
					SoundEngine.PlaySound(7);
				}
			}
			else if (Main.mouseItem.type > 0 && inv[slot].type == 0)
			{
				if (Main.mouseItem.stack == 1)
				{
					MouseItemSwap(ref inv[slot], context);
					if (inv[slot].type == 0 || inv[slot].stack < 1)
					{
						inv[slot] = new Item();
					}
					if (Main.mouseItem.type == 0 || Main.mouseItem.stack < 1)
					{
						Main.mouseItem = new Item();
					}
					if (Main.mouseItem.type > 0 || inv[slot].type > 0)
					{
						SoundEngine.PlaySound(7);
					}
				}
				else
				{
					inv[slot] = Main.mouseItem.Clone();
					Main.mouseItem.stack--;
					inv[slot].stack = 1;
					SoundEngine.PlaySound(7);
				}
				if (inv[slot].stack > 0)
				{
					switch (context)
					{
					case 0:
						AchievementsHelper.NotifyItemPickup(player, inv[slot]);
						break;
					case 8:
					case 9:
					case 10:
					case 11:
					case 12:
					case 16:
					case 17:
					case 25:
					case 27:
					case 33:
						AchievementsHelper.HandleOnEquip(player, inv[slot], context);
						break;
					}
				}
			}
			if ((context == 23 || context == 24 || context == 39) && Main.netMode == 1)
			{
				NetMessage.SendData(121, -1, -1, null, Main.myPlayer, player.tileEntityAnchor.interactEntityID, slot);
			}
			if (context == 38 && Main.netMode == 1)
			{
				NetMessage.SendData(121, -1, -1, null, Main.myPlayer, player.tileEntityAnchor.interactEntityID, slot, 3f);
			}
			if (context == 26 && Main.netMode == 1)
			{
				NetMessage.SendData(124, -1, -1, null, Main.myPlayer, player.tileEntityAnchor.interactEntityID, slot);
			}
			CoinSlot.ForceSlotState(slot, context, inv[slot]);
			break;
		case 2:
			if (Main.mouseItem.stack == 1 && Main.mouseItem.dye > 0 && inv[slot].type > 0 && inv[slot].type != Main.mouseItem.type)
			{
				MouseItemSwap(ref inv[slot], context);
				SoundEngine.PlaySound(7);
				if (inv[slot].stack > 0)
				{
					switch (context)
					{
					case 0:
						AchievementsHelper.NotifyItemPickup(player, inv[slot]);
						break;
					case 8:
					case 9:
					case 10:
					case 11:
					case 12:
					case 16:
					case 17:
					case 25:
					case 27:
					case 33:
						AchievementsHelper.HandleOnEquip(player, inv[slot], context);
						break;
					}
				}
			}
			else if (Main.mouseItem.type == 0 && inv[slot].type > 0)
			{
				MouseItemSwap(ref inv[slot], context);
				if (inv[slot].type == 0 || inv[slot].stack < 1)
				{
					inv[slot] = new Item();
				}
				if (Main.mouseItem.type == 0 || Main.mouseItem.stack < 1)
				{
					Main.mouseItem = new Item();
				}
				if (Main.mouseItem.type > 0 || inv[slot].type > 0)
				{
					SoundEngine.PlaySound(7);
				}
			}
			else if (Main.mouseItem.dye > 0 && inv[slot].type == 0)
			{
				if (Main.mouseItem.stack == 1)
				{
					MouseItemSwap(ref inv[slot], context);
					if (inv[slot].type == 0 || inv[slot].stack < 1)
					{
						inv[slot] = new Item();
					}
					if (Main.mouseItem.type == 0 || Main.mouseItem.stack < 1)
					{
						Main.mouseItem = new Item();
					}
					if (Main.mouseItem.type > 0 || inv[slot].type > 0)
					{
						SoundEngine.PlaySound(7);
					}
				}
				else
				{
					Main.mouseItem.stack--;
					inv[slot].SetDefaults(Main.mouseItem.type);
					SoundEngine.PlaySound(7);
				}
				if (inv[slot].stack > 0)
				{
					switch (context)
					{
					case 0:
						AchievementsHelper.NotifyItemPickup(player, inv[slot]);
						break;
					case 8:
					case 9:
					case 10:
					case 11:
					case 12:
					case 16:
					case 17:
					case 25:
					case 27:
					case 33:
						AchievementsHelper.HandleOnEquip(player, inv[slot], context);
						break;
					}
				}
			}
			if (context == 25 && Main.netMode == 1)
			{
				NetMessage.SendData(121, -1, -1, null, Main.myPlayer, player.tileEntityAnchor.interactEntityID, slot, 1f);
			}
			if (context == 27 && Main.netMode == 1)
			{
				NetMessage.SendData(124, -1, -1, null, Main.myPlayer, player.tileEntityAnchor.interactEntityID, slot, 1f);
			}
			break;
		case 3:
			HandleShopSlot(inv, slot, rightClickIsValid: false, leftClickIsValid: true);
			break;
		case 4:
		{
			Chest chest = Main.instance.shop[Main.npcShop];
			if (player.SellItem(Main.mouseItem))
			{
				chest.AddItemToShop(Main.mouseItem);
				Main.mouseItem.SetDefaults(0);
				SoundEngine.PlaySound(18);
				AnnounceTransfer(new ItemTransferInfo(inv[slot], 21, 15));
			}
			else if (Main.mouseItem.value == 0)
			{
				chest.AddItemToShop(Main.mouseItem);
				Main.mouseItem.SetDefaults(0);
				SoundEngine.PlaySound(7);
				AnnounceTransfer(new ItemTransferInfo(inv[slot], 21, 15));
			}
			Main.stackSplit = 9999;
			break;
		}
		case 5:
			if (Main.mouseItem.IsAir)
			{
				SoundEngine.PlaySound(7);
				Main.mouseItem.SetDefaults(inv[slot].type);
				Main.mouseItem.stack = (Main.mouseItem.OnlyNeedOneInInventory() ? 1 : Main.mouseItem.maxStack);
				Main.mouseItem.OnCreated(new JourneyDuplicationItemCreationContext());
				AnnounceTransfer(new ItemTransferInfo(inv[slot], 29, 21));
			}
			break;
		}
	}

	private static void MouseItemSwap(ref Item slot, int context)
	{
		Utils.Swap(ref slot, ref Main.mouseItem);
		if (!retainFavoriteOnMoveAt[context])
		{
			slot.favorited = false;
			Main.mouseItem.favorited = false;
		}
	}

	private static void SwapClearFavorite(ref Item item1, ref Item item2)
	{
		item1.favorited = false;
		item2.favorited = false;
		Utils.Swap(ref item1, ref item2);
	}

	private static bool TryResearchingItem(Item[] inv, int slot, bool onlySacrificeIfItWouldFinishResearch = false)
	{
		return TryResearchingItem(ref inv[slot], onlySacrificeIfItWouldFinishResearch);
	}

	private static bool TryResearchingItem(ref Item item, bool onlySacrificeIfItWouldFinishResearch = false)
	{
		if (!Main.IsJourneyMode)
		{
			return false;
		}
		if (item == null || item.IsAir)
		{
			return false;
		}
		int amountWeSacrificed;
		switch (Main.CreativeMenu.SacrificeItem(ref item, out amountWeSacrificed, spawnExcessItem: false, onlySacrificeIfItWouldFinishResearch))
		{
		case CreativeUI.ItemSacrificeResult.CannotSacrifice:
			return false;
		case CreativeUI.ItemSacrificeResult.SacrificedAndDone:
			SoundEngine.PlaySound(64);
			break;
		default:
			SoundEngine.PlaySound(63);
			break;
		}
		return true;
	}

	public static bool ShouldHighlightSlotForMouseItem(int context, int slot, Item checkItem)
	{
		bool result = false;
		if (!checkItem.IsAir)
		{
			switch (context)
			{
			case 8:
			case 9:
			case 10:
			case 11:
			{
				bool flag = slot >= 10;
				result = checkItem.vanity == flag && Main.LocalPlayer.armor[slot].type != checkItem.type && Main.LocalPlayer.IsItemSlotUnlockedAndUsable(slot) && CanEquipInArmorSlot(Main.LocalPlayer, checkItem, slot);
				break;
			}
			case 23:
				result = (checkItem.headSlot > 0 && slot == 0) || (checkItem.bodySlot > 0 && slot == 1) || (checkItem.legSlot > 0 && slot == 2);
				break;
			case 26:
				result = checkItem.headSlot > -1;
				break;
			case 12:
				result = Main.LocalPlayer.IsItemSlotUnlockedAndUsable(slot) && checkItem.dye > 0;
				break;
			case 25:
			case 27:
			case 33:
				result = checkItem.dye > 0;
				break;
			case 16:
				result = checkItem.mountType == -1 && Main.projHook[checkItem.shoot];
				break;
			case 17:
				result = checkItem.mountType != -1 && !MountID.Sets.Cart[checkItem.mountType];
				break;
			case 39:
				result = checkItem.mountType != -1;
				break;
			case 19:
				result = checkItem.buffType > 0 && Main.vanityPet[checkItem.buffType] && !Main.lightPet[checkItem.buffType];
				break;
			case 18:
				result = checkItem.mountType != -1 && MountID.Sets.Cart[checkItem.mountType];
				break;
			case 20:
				result = checkItem.buffType > 0 && Main.lightPet[checkItem.buffType];
				break;
			case 24:
			{
				TEDisplayDoll tEDisplayDoll = Main.LocalPlayer.tileEntityAnchor.GetTileEntity() as TEDisplayDoll;
				result = checkItem.accessory && CanEquipAccessoryInSlot(checkItem, vanity: true, tEDisplayDoll.AccSlots, slot);
				break;
			}
			case 38:
				result = TEDisplayDoll.AcceptedInWeaponSlot(checkItem);
				break;
			}
		}
		return result;
	}

	public static void GetDimSlotForMouseItem(int context, int slot, Item checkItem, out float itemFade)
	{
		bool flag = false;
		itemFade = 1f;
		if (!checkItem.IsAir)
		{
			switch (context)
			{
			case 8:
			case 9:
			case 10:
			case 11:
				flag = !Main.LocalPlayer.IsItemSlotUnlockedAndUsable(slot) || !CanEquipInArmorSlot(Main.LocalPlayer, checkItem, slot);
				break;
			case 23:
				flag = (checkItem.headSlot <= 0 || slot != 0) && (checkItem.bodySlot <= 0 || slot != 1) && (checkItem.legSlot <= 0 || slot != 2);
				break;
			case 26:
				flag = checkItem.headSlot <= -1;
				break;
			case 12:
				flag = !Main.LocalPlayer.IsItemSlotUnlockedAndUsable(slot) || checkItem.dye <= 0;
				break;
			case 25:
			case 27:
			case 33:
				flag = checkItem.dye <= 0;
				break;
			case 16:
				flag = checkItem.mountType != -1 || !Main.projHook[checkItem.shoot];
				break;
			case 17:
				flag = checkItem.mountType == -1 || MountID.Sets.Cart[checkItem.mountType];
				break;
			case 39:
				flag = checkItem.mountType == -1 || MountID.Sets.Cart[checkItem.mountType];
				break;
			case 19:
				flag = checkItem.buffType <= 0 || !Main.vanityPet[checkItem.buffType] || Main.lightPet[checkItem.buffType];
				break;
			case 18:
				flag = checkItem.mountType == -1 || !MountID.Sets.Cart[checkItem.mountType];
				break;
			case 20:
				flag = checkItem.buffType <= 0 || !Main.lightPet[checkItem.buffType];
				break;
			case 24:
			{
				TEDisplayDoll tEDisplayDoll = Main.LocalPlayer.tileEntityAnchor.GetTileEntity() as TEDisplayDoll;
				flag = !checkItem.accessory || !CanEquipAccessoryInSlot(checkItem, vanity: true, tEDisplayDoll.AccSlots, slot);
				break;
			}
			case 38:
				flag = !TEDisplayDoll.AcceptedInWeaponSlot(checkItem);
				break;
			}
		}
		if (flag)
		{
			itemFade = 0.5f;
		}
	}

	public static bool CanEquipInArmorSlot(Player player, Item checkItem, int slot)
	{
		bool flag = slot % 10 < 3;
		bool flag2 = slot < 10;
		if (flag)
		{
			switch (slot % 10)
			{
			case 0:
				if (checkItem.headSlot < 0)
				{
					return false;
				}
				break;
			case 1:
				if (checkItem.bodySlot < 0)
				{
					return false;
				}
				break;
			case 2:
				if (checkItem.legSlot < 0)
				{
					return false;
				}
				break;
			}
			if (flag2)
			{
				if (!ItemID.Sets.DualEquipArmor[checkItem.type])
				{
					return !HasSameItemInSlot(checkItem, VanityArmorSlots(Main.LocalPlayer));
				}
				return true;
			}
			if (!ItemID.Sets.DualEquipArmor[checkItem.type])
			{
				return !HasSameItemInSlot(checkItem, ArmorSlots(Main.LocalPlayer));
			}
			return true;
		}
		if (!checkItem.accessory)
		{
			return false;
		}
		if (flag2)
		{
			if (CanEquipAccessoryInSlot(checkItem, vanity: false, AccSlots(player), slot))
			{
				return !HasSameItemInSlot(checkItem, VanityAccSlots(player));
			}
			return false;
		}
		if (CanEquipAccessoryInSlot(checkItem, vanity: true, VanityAccSlots(player), slot))
		{
			return !HasSameItemInSlot(checkItem, AccSlots(player));
		}
		return false;
	}

	public static int PickItemMovementAction(Item[] inv, int context, int slot, Item checkItem)
	{
		_ = Main.player[Main.myPlayer];
		int result = -1;
		switch (context)
		{
		case 0:
			result = 0;
			break;
		case 1:
			if (checkItem.type == 0 || checkItem.type == 71 || checkItem.type == 72 || checkItem.type == 73 || checkItem.type == 74)
			{
				result = 0;
			}
			break;
		case 2:
			if (checkItem.FitsAmmoSlot())
			{
				result = 0;
			}
			break;
		case 3:
			result = 0;
			break;
		case 4:
		case 32:
		{
			Item[] item = Main.LocalPlayer.GetCurrentContainer().item;
			if (!ChestUI.IsBlockedFromTransferIntoChest(checkItem, item))
			{
				result = 0;
			}
			break;
		}
		case 5:
			if (checkItem.CanHavePrefixes() || checkItem.type == 0)
			{
				result = 1;
			}
			break;
		case 6:
			result = 0;
			break;
		case 7:
			if (checkItem.material || checkItem.type == 0)
			{
				result = 0;
			}
			break;
		case 8:
		case 9:
		case 10:
		case 11:
			if (checkItem.type == 0 || CanEquipInArmorSlot(Main.LocalPlayer, checkItem, slot))
			{
				result = 1;
			}
			break;
		case 23:
			if (checkItem.type == 0 || (checkItem.headSlot > 0 && slot == 0) || (checkItem.bodySlot > 0 && slot == 1) || (checkItem.legSlot > 0 && slot == 2))
			{
				result = 1;
			}
			break;
		case 26:
			if (checkItem.type == 0 || checkItem.headSlot > 0)
			{
				result = 1;
			}
			break;
		case 24:
		{
			TEDisplayDoll tEDisplayDoll = Main.LocalPlayer.tileEntityAnchor.GetTileEntity() as TEDisplayDoll;
			if (checkItem.type == 0 || (checkItem.accessory && CanEquipAccessoryInSlot(checkItem, vanity: true, tEDisplayDoll.AccSlots, slot)))
			{
				result = 1;
			}
			break;
		}
		case 12:
		case 25:
		case 27:
		case 33:
			result = 2;
			break;
		case 15:
			if (checkItem.type == 0 && inv[slot].type > 0)
			{
				result = 3;
			}
			else if (checkItem.type == inv[slot].type && checkItem.type > 0 && checkItem.stack < checkItem.maxStack && inv[slot].stack > 0)
			{
				result = 3;
			}
			else if (inv[slot].type == 0 && checkItem.type > 0 && (checkItem.type < 71 || checkItem.type > 74))
			{
				result = 4;
			}
			break;
		case 16:
			if (checkItem.type == 0 || Main.projHook[checkItem.shoot])
			{
				result = 1;
			}
			break;
		case 17:
			if (checkItem.type == 0 || (checkItem.mountType != -1 && !MountID.Sets.Cart[checkItem.mountType]))
			{
				result = 1;
			}
			break;
		case 39:
			if (checkItem.type == 0 || checkItem.mountType != -1)
			{
				result = 1;
			}
			break;
		case 38:
			if (checkItem.type == 0 || TEDisplayDoll.AcceptedInWeaponSlot(checkItem))
			{
				result = 1;
			}
			break;
		case 19:
			if (checkItem.type == 0 || (checkItem.buffType > 0 && Main.vanityPet[checkItem.buffType] && !Main.lightPet[checkItem.buffType]))
			{
				result = 1;
			}
			break;
		case 18:
			if (checkItem.type == 0 || (checkItem.mountType != -1 && MountID.Sets.Cart[checkItem.mountType]))
			{
				result = 1;
			}
			break;
		case 20:
			if (checkItem.type == 0 || (checkItem.buffType > 0 && Main.lightPet[checkItem.buffType]))
			{
				result = 1;
			}
			break;
		case 29:
			if (checkItem.type == 0 && inv[slot].type > 0)
			{
				result = 5;
			}
			break;
		}
		if (context == 30)
		{
			result = 0;
		}
		return result;
	}

	public static void RightClick(Item[] inv, int context = 0, int slot = 0)
	{
		if (Main.LocalPlayerHasPendingInventoryActions())
		{
			return;
		}
		Player player = Main.player[Main.myPlayer];
		inv[slot].newAndShiny = false;
		if (player.itemAnimation > 0)
		{
			return;
		}
		if (context == 15)
		{
			HandleShopSlot(inv, slot, rightClickIsValid: true, leftClickIsValid: false);
		}
		else
		{
			if (!Main.mouseRight || context == 6 || context == 34)
			{
				return;
			}
			if (Main.mouseItem.IsAir || !Item.CanStack(inv[slot], Main.mouseItem))
			{
				if (!PlayerInput.UsingGamepadUI && context == 0 && ItemID.Sets.OpenableBag[inv[slot].type])
				{
					if (Main.mouseRightRelease)
					{
						TryOpenContainer(inv, context, slot, player);
					}
					return;
				}
				if (!PlayerInput.UsingGamepadUI && canItemSwapAt[context] && inv[slot].stack == 1 && ItemID.Sets.HasItemSwap[inv[slot].type])
				{
					if (Main.mouseRightRelease)
					{
						TryItemSwap(inv[slot]);
					}
					return;
				}
				switch (context)
				{
				case 9:
				case 11:
					if (Main.mouseRightRelease)
					{
						SwapVanityEquip(inv, context, slot, player);
					}
					return;
				case 0:
				case 3:
				case 4:
				case 32:
					if (inv[slot].stack == 1 && !PlayerInput.UsingGamepadUI && (inv[slot].CanBeEquipped() || inv[slot].dye > 0))
					{
						if (Main.mouseRightRelease)
						{
							SwapEquip(inv, context, slot);
						}
						return;
					}
					break;
				}
			}
			if (Main.stackSplit > 1 || inv[slot].IsAir)
			{
				return;
			}
			int num = Main.superFastStack + 1;
			for (int i = 0; i < num; i++)
			{
				if ((Item.CanStack(Main.mouseItem, inv[slot]) && Main.mouseItem.stack < Main.mouseItem.maxStack) || Main.mouseItem.type == 0)
				{
					PickupItemIntoMouse(inv, context, slot, player);
					SoundEngine.PlaySound(12);
					RefreshStackSplitCooldown();
				}
			}
		}
	}

	public static void PickupItemIntoMouse(Item[] inv, int context, int slot, Player player)
	{
		if (Main.mouseItem.type == 0)
		{
			Main.mouseItem = inv[slot].Clone();
			if (context == 29)
			{
				Main.mouseItem.SetDefaults(Main.mouseItem.type);
				Main.mouseItem.OnCreated(new JourneyDuplicationItemCreationContext());
			}
			Main.mouseItem.stack = 0;
			if (inv[slot].favorited && inv[slot].stack == 1)
			{
				Main.mouseItem.favorited = true;
			}
			else
			{
				Main.mouseItem.favorited = false;
			}
			AnnounceTransfer(new ItemTransferInfo(inv[slot], context, 21));
		}
		Main.mouseItem.stack++;
		if (context != 29)
		{
			inv[slot].stack--;
		}
		if (inv[slot].stack <= 0)
		{
			inv[slot] = new Item();
		}
		CoinSlot.ForceSlotState(slot, context, inv[slot]);
		if (context == 3 && Main.netMode == 1)
		{
			NetMessage.SendData(32, -1, -1, null, player.chest, slot);
		}
		if ((context == 23 || context == 24 || context == 39) && Main.netMode == 1)
		{
			NetMessage.SendData(121, -1, -1, null, Main.myPlayer, player.tileEntityAnchor.interactEntityID, slot);
		}
		if (context == 25 && Main.netMode == 1)
		{
			NetMessage.SendData(121, -1, -1, null, Main.myPlayer, player.tileEntityAnchor.interactEntityID, slot, 1f);
		}
		if (context == 38 && Main.netMode == 1)
		{
			NetMessage.SendData(121, -1, -1, null, Main.myPlayer, player.tileEntityAnchor.interactEntityID, slot, 3f);
		}
		if (context == 26 && Main.netMode == 1)
		{
			NetMessage.SendData(124, -1, -1, null, Main.myPlayer, player.tileEntityAnchor.interactEntityID, slot);
		}
		if (context == 27 && Main.netMode == 1)
		{
			NetMessage.SendData(124, -1, -1, null, Main.myPlayer, player.tileEntityAnchor.interactEntityID, slot, 1f);
		}
	}

	public static void RefreshStackSplitCooldown()
	{
		if (Main.stackSplit == 0)
		{
			Main.stackSplit = 30;
		}
		else
		{
			Main.stackSplit = Main.stackDelay;
		}
	}

	private static void TryOpenContainer(Item[] inv, int context, int slot, Player player)
	{
		Item item = inv[slot];
		Player.GetItemLogger.Start();
		bool flag = TryOpenContainer_GrantItems(item, player);
		Player.GetItemLogger.Stop();
		DisplayTransfer_GetItem(inv, context, slot);
		if (flag)
		{
			item.stack--;
			if (item.stack == 0)
			{
				item.SetDefaults(0);
			}
			SoundEngine.PlaySound(7);
			Main.stackSplit = 30;
			Main.mouseRightRelease = false;
		}
	}

	private static bool TryOpenContainer_GrantItems(Item item, Player player)
	{
		if (ItemID.Sets.BossBag[item.type])
		{
			player.OpenBossBag(item.type);
		}
		else if (ItemID.Sets.IsFishingCrate[item.type])
		{
			player.OpenFishingCrate(item.type);
		}
		else if (item.type == 3093)
		{
			player.OpenHerbBag(3093);
		}
		else if (item.type == 4345)
		{
			player.OpenCanofWorms(item.type);
		}
		else if (item.type == 4410)
		{
			player.OpenOyster(item.type);
		}
		else if (item.type == 1774)
		{
			player.OpenGoodieBag(1774);
		}
		else if (item.type == 6142)
		{
			player.OpenChilletEgg(6142);
		}
		else if (item.type == 3085)
		{
			if (!player.ConsumeItem(327, reverseOrder: false, includeVoidBag: true))
			{
				return false;
			}
			player.OpenLockBox(3085);
		}
		else if (item.type == 4879)
		{
			if (!player.HasItemInInventoryOrOpenVoidBag(329))
			{
				return false;
			}
			player.OpenShadowLockbox(4879);
		}
		else if (item.type == 1869)
		{
			player.OpenPresent(1869);
		}
		else
		{
			if (item.type != 599 && item.type != 600 && item.type != 601)
			{
				return false;
			}
			player.OpenLegacyPresent(item.type);
		}
		return true;
	}

	private static void SwapVanityEquip(Item[] inv, int context, int slot, Player player)
	{
		if (Main.npcShop > 0)
		{
			return;
		}
		Item item = inv[slot - 10];
		if ((inv[slot].IsAir && item.IsAir) || (context == 11 && ((!inv[slot].IsAir && !CanEquipAccessoryInSlot(inv[slot], vanity: false, AccSlots(Main.LocalPlayer), slot - 10)) || (!item.IsAir && !CanEquipAccessoryInSlot(item, vanity: true, VanityAccSlots(Main.LocalPlayer), slot)))))
		{
			return;
		}
		SwapClearFavorite(ref inv[slot], ref inv[slot - 10]);
		SoundEngine.PlaySound(7);
		if (inv[slot].stack > 0)
		{
			switch (context)
			{
			case 0:
				AchievementsHelper.NotifyItemPickup(player, inv[slot]);
				break;
			case 8:
			case 9:
			case 10:
			case 11:
			case 12:
			case 16:
			case 17:
			case 33:
				AchievementsHelper.HandleOnEquip(player, inv[slot], context);
				break;
			}
		}
	}

	private static void TryItemSwap(Item item)
	{
		int type = item.type;
		switch (type)
		{
		case 4131:
		case 5325:
			item.ChangeItemType((item.type == 5325) ? 4131 : 5325);
			AfterItemSwap(type, item.type);
			break;
		case 5059:
		case 5060:
			item.ChangeItemType((item.type == 5059) ? 5060 : 5059);
			AfterItemSwap(type, item.type);
			break;
		case 5324:
			item.ChangeItemType(5329);
			AfterItemSwap(type, item.type);
			break;
		case 5329:
			item.ChangeItemType(5330);
			AfterItemSwap(type, item.type);
			break;
		case 5330:
			item.ChangeItemType(5324);
			AfterItemSwap(type, item.type);
			break;
		case 4346:
		case 5391:
			item.ChangeItemType((item.type == 4346) ? 5391 : 4346);
			AfterItemSwap(type, item.type);
			break;
		case 6190:
		case 6195:
			item.ChangeItemType((item.type == 6190) ? 6195 : 6190);
			AfterItemSwap(type, item.type);
			break;
		case 5323:
		case 5455:
			item.ChangeItemType((item.type == 5323) ? 5455 : 5323);
			AfterItemSwap(type, item.type);
			break;
		case 4767:
		case 5453:
			item.ChangeItemType((item.type == 4767) ? 5453 : 4767);
			AfterItemSwap(type, item.type);
			break;
		case 5309:
		case 5454:
			item.ChangeItemType((item.type == 5309) ? 5454 : 5309);
			AfterItemSwap(type, item.type);
			break;
		case 5358:
			item.ChangeItemType(5360);
			AfterItemSwap(type, item.type);
			break;
		case 5360:
			item.ChangeItemType(5361);
			AfterItemSwap(type, item.type);
			break;
		case 5361:
			item.ChangeItemType(5359);
			AfterItemSwap(type, item.type);
			break;
		case 5359:
			item.ChangeItemType(5358);
			AfterItemSwap(type, item.type);
			break;
		case 5437:
			item.ChangeItemType(5358);
			AfterItemSwap(type, item.type);
			break;
		case 6168:
			item.ChangeItemType(6169);
			AfterItemSwap(type, item.type);
			break;
		case 6169:
			item.ChangeItemType(6193);
			AfterItemSwap(type, item.type);
			break;
		case 6193:
			item.ChangeItemType(6194);
			AfterItemSwap(type, item.type);
			break;
		case 6194:
			item.ChangeItemType(6168);
			AfterItemSwap(type, item.type);
			break;
		case 2611:
		case 5526:
			item.ChangeItemType((item.type == 2611) ? 5526 : 2611);
			AfterItemSwap(type, item.type);
			break;
		}
	}

	private static void AfterItemSwap(int oldType, int newType)
	{
		if (newType == 5324 || newType == 5329 || newType == 5330 || newType == 4346 || newType == 5391 || newType == 5358 || newType == 5361 || newType == 5360 || newType == 5359 || newType == 2611 || newType == 5526 || newType == 6168 || newType == 6169 || newType == 6193 || newType == 6194)
		{
			SoundEngine.PlaySound(22);
		}
		else
		{
			SoundEngine.PlaySound(7);
		}
		Main.stackSplit = 30;
		Main.mouseRightRelease = false;
	}

	public static void HandleItemPickupAction<TItemInfo>(ItemPickupAction<TItemInfo> action, TItemInfo entry, int slotItemType, int slotItemStack, ref int stackLimiter)
	{
		if (!Main.mouseItem.IsAir && slotItemType != Main.mouseItem.type)
		{
			return;
		}
		bool flag = stackLimiter == -1;
		if (!((Main.stackSplit > 1) | flag))
		{
			if (stackLimiter == 0)
			{
				stackLimiter = slotItemStack;
			}
			int num = Math.Min(Main.superFastStack + 1, stackLimiter);
			stackLimiter -= num;
			if (stackLimiter == 0)
			{
				stackLimiter = -1;
			}
			action(entry, num);
			RefreshStackSplitCooldown();
		}
	}

	private static void HandleShopSlot(Item[] inv, int slot, bool rightClickIsValid, bool leftClickIsValid)
	{
		if (Main.cursorOverride == 2)
		{
			return;
		}
		_ = Main.instance.shop[Main.npcShop];
		bool flag = (Main.mouseRight & rightClickIsValid) || (Main.mouseLeft & leftClickIsValid);
		if (!((Main.stackSplit <= 1) & flag) || inv[slot].type <= 0 || (!Item.CanStack(Main.mouseItem, inv[slot]) && Main.mouseItem.type != 0))
		{
			return;
		}
		int num = Main.superFastStack + 1;
		if (CanBulkBuy(inv[slot]))
		{
			num *= GetBulkBuyAmount(inv[slot]);
		}
		Player localPlayer = Main.LocalPlayer;
		for (int i = 0; i < num; i++)
		{
			if (Main.mouseItem.stack >= Main.mouseItem.maxStack && Main.mouseItem.type != 0)
			{
				continue;
			}
			localPlayer.GetItemExpectedPrice(inv[slot], out var _, out var calcForBuying);
			if (!localPlayer.BuyItem(calcForBuying, inv[slot].shopSpecialCurrency) || inv[slot].stack <= 0)
			{
				continue;
			}
			if (i == 0)
			{
				if (inv[slot].value > 0)
				{
					SoundEngine.PlaySound(18);
				}
				else
				{
					SoundEngine.PlaySound(7);
				}
			}
			if (Main.mouseItem.type == 0)
			{
				Main.mouseItem.SetDefaults(inv[slot].type);
				if (inv[slot].prefix != 0)
				{
					Main.mouseItem.Prefix(inv[slot].prefix);
				}
				Main.mouseItem.stack = 0;
			}
			if (!inv[slot].buyOnce && inv[slot].shopSpecialCurrency == -1)
			{
				Main.shopSellbackHelper.Add(inv[slot]);
			}
			Main.mouseItem.stack++;
			RefreshStackSplitCooldown();
			if (inv[slot].buyOnce && --inv[slot].stack <= 0)
			{
				inv[slot].SetDefaults(0);
			}
			AnnounceTransfer(new ItemTransferInfo(Main.mouseItem, 15, 21));
		}
	}

	public static void Draw(SpriteBatch spriteBatch, ref Item inv, int context, Vector2 position, Color lightColor = default(Color))
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		singleSlotArray[0] = inv;
		Draw(spriteBatch, singleSlotArray, context, 0, position, lightColor);
		inv = singleSlotArray[0];
	}

	public static void Draw(SpriteBatch spriteBatch, Item[] inv, int context, int slot, Vector2 position, Color lightColor = default(Color))
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		//IL_0167: Unknown result type (might be due to invalid IL or missing references)
		//IL_016f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0174: Unknown result type (might be due to invalid IL or missing references)
		//IL_0179: Unknown result type (might be due to invalid IL or missing references)
		//IL_026b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0270: Unknown result type (might be due to invalid IL or missing references)
		//IL_069e: Unknown result type (might be due to invalid IL or missing references)
		//IL_06a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_06c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_06c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_06cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_06e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_06f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_07e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_081f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0821: Unknown result type (might be due to invalid IL or missing references)
		//IL_0823: Unknown result type (might be due to invalid IL or missing references)
		//IL_0828: Unknown result type (might be due to invalid IL or missing references)
		//IL_0754: Unknown result type (might be due to invalid IL or missing references)
		//IL_075b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0760: Unknown result type (might be due to invalid IL or missing references)
		//IL_0809: Unknown result type (might be due to invalid IL or missing references)
		//IL_080e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0795: Unknown result type (might be due to invalid IL or missing references)
		//IL_079c: Unknown result type (might be due to invalid IL or missing references)
		//IL_07a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_07bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_07c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_07cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0771: Unknown result type (might be due to invalid IL or missing references)
		//IL_0778: Unknown result type (might be due to invalid IL or missing references)
		//IL_077d: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_021c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0230: Unknown result type (might be due to invalid IL or missing references)
		//IL_0235: Unknown result type (might be due to invalid IL or missing references)
		//IL_023a: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_020b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0210: Unknown result type (might be due to invalid IL or missing references)
		//IL_0215: Unknown result type (might be due to invalid IL or missing references)
		//IL_088b: Unknown result type (might be due to invalid IL or missing references)
		//IL_088f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0894: Unknown result type (might be due to invalid IL or missing references)
		//IL_082f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0834: Unknown result type (might be due to invalid IL or missing references)
		//IL_0836: Unknown result type (might be due to invalid IL or missing references)
		//IL_083a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0840: Unknown result type (might be due to invalid IL or missing references)
		//IL_0845: Unknown result type (might be due to invalid IL or missing references)
		//IL_08a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_08ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_08b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_08bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a14: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a16: Unknown result type (might be due to invalid IL or missing references)
		//IL_0865: Unknown result type (might be due to invalid IL or missing references)
		//IL_086a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a35: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a59: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ab3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aba: Unknown result type (might be due to invalid IL or missing references)
		//IL_0abf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0acc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ad8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ae1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ae7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a7d: Unknown result type (might be due to invalid IL or missing references)
		//IL_092c: Unknown result type (might be due to invalid IL or missing references)
		//IL_096b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0970: Unknown result type (might be due to invalid IL or missing references)
		//IL_0972: Unknown result type (might be due to invalid IL or missing references)
		//IL_0985: Unknown result type (might be due to invalid IL or missing references)
		//IL_098a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0997: Unknown result type (might be due to invalid IL or missing references)
		//IL_09a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_09ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_09b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d26: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d2c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d31: Unknown result type (might be due to invalid IL or missing references)
		//IL_030b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0310: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cac: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cb1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cce: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cd2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cdc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ce2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ce7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cec: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cf3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cfd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d07: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d09: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d13: Unknown result type (might be due to invalid IL or missing references)
		//IL_09d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_09e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_09ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_09f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ba6: Unknown result type (might be due to invalid IL or missing references)
		//IL_1baa: Unknown result type (might be due to invalid IL or missing references)
		//IL_1bb0: Unknown result type (might be due to invalid IL or missing references)
		//IL_1bba: Unknown result type (might be due to invalid IL or missing references)
		//IL_1bbf: Unknown result type (might be due to invalid IL or missing references)
		//IL_1bc6: Unknown result type (might be due to invalid IL or missing references)
		//IL_1bcc: Unknown result type (might be due to invalid IL or missing references)
		//IL_1bd6: Unknown result type (might be due to invalid IL or missing references)
		//IL_1bdb: Unknown result type (might be due to invalid IL or missing references)
		//IL_1be0: Unknown result type (might be due to invalid IL or missing references)
		//IL_1be5: Unknown result type (might be due to invalid IL or missing references)
		//IL_1bf9: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c05: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c0b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0daa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dac: Unknown result type (might be due to invalid IL or missing references)
		//IL_0db3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0db8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dcb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dd0: Unknown result type (might be due to invalid IL or missing references)
		//IL_034a: Unknown result type (might be due to invalid IL or missing references)
		//IL_034f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1cdb: Unknown result type (might be due to invalid IL or missing references)
		//IL_1cdd: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ce4: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ce9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dfb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e60: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e62: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e69: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e6e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e79: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e1f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e21: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e28: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e2d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e40: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e45: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e50: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c4e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c53: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c9b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ca7: Unknown result type (might be due to invalid IL or missing references)
		//IL_1cad: Unknown result type (might be due to invalid IL or missing references)
		//IL_1cb2: Unknown result type (might be due to invalid IL or missing references)
		//IL_1cb7: Unknown result type (might be due to invalid IL or missing references)
		//IL_1cbe: Unknown result type (might be due to invalid IL or missing references)
		//IL_1cc4: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c6b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c70: Unknown result type (might be due to invalid IL or missing references)
		//IL_1072: Unknown result type (might be due to invalid IL or missing references)
		//IL_1078: Unknown result type (might be due to invalid IL or missing references)
		//IL_107d: Unknown result type (might be due to invalid IL or missing references)
		//IL_10c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_10cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_10d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_10d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_10d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_10e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_10ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_10ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_10f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_10fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_10fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_1108: Unknown result type (might be due to invalid IL or missing references)
		//IL_110a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1114: Unknown result type (might be due to invalid IL or missing references)
		//IL_1148: Unknown result type (might be due to invalid IL or missing references)
		//IL_114d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1152: Unknown result type (might be due to invalid IL or missing references)
		//IL_1154: Unknown result type (might be due to invalid IL or missing references)
		//IL_1156: Unknown result type (might be due to invalid IL or missing references)
		//IL_1165: Unknown result type (might be due to invalid IL or missing references)
		//IL_116b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1170: Unknown result type (might be due to invalid IL or missing references)
		//IL_1175: Unknown result type (might be due to invalid IL or missing references)
		//IL_117c: Unknown result type (might be due to invalid IL or missing references)
		//IL_117f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1189: Unknown result type (might be due to invalid IL or missing references)
		//IL_118b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1195: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f65: Unknown result type (might be due to invalid IL or missing references)
		//IL_127b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1281: Unknown result type (might be due to invalid IL or missing references)
		//IL_1286: Unknown result type (might be due to invalid IL or missing references)
		//IL_11c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_11cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_11d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_11d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_11d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_11e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_11e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_11ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_11f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_11fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_11fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_1207: Unknown result type (might be due to invalid IL or missing references)
		//IL_1209: Unknown result type (might be due to invalid IL or missing references)
		//IL_1213: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f92: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f94: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f9b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fa0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fb3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fb8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ffd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fff: Unknown result type (might be due to invalid IL or missing references)
		//IL_1006: Unknown result type (might be due to invalid IL or missing references)
		//IL_100b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1019: Unknown result type (might be due to invalid IL or missing references)
		//IL_0405: Unknown result type (might be due to invalid IL or missing references)
		//IL_139a: Unknown result type (might be due to invalid IL or missing references)
		//IL_139f: Unknown result type (might be due to invalid IL or missing references)
		//IL_13a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_13a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_13ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_13b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_13c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_13c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_13c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_13cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_13d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_13d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_13e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_12d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_12dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_12df: Unknown result type (might be due to invalid IL or missing references)
		//IL_12e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_12eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_12f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_1301: Unknown result type (might be due to invalid IL or missing references)
		//IL_1303: Unknown result type (might be due to invalid IL or missing references)
		//IL_1305: Unknown result type (might be due to invalid IL or missing references)
		//IL_130a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1311: Unknown result type (might be due to invalid IL or missing references)
		//IL_1314: Unknown result type (might be due to invalid IL or missing references)
		//IL_131e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fe6: Unknown result type (might be due to invalid IL or missing references)
		//IL_13f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_13fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_13ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_1401: Unknown result type (might be due to invalid IL or missing references)
		//IL_140b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1410: Unknown result type (might be due to invalid IL or missing references)
		//IL_1421: Unknown result type (might be due to invalid IL or missing references)
		//IL_1423: Unknown result type (might be due to invalid IL or missing references)
		//IL_1425: Unknown result type (might be due to invalid IL or missing references)
		//IL_142a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1431: Unknown result type (might be due to invalid IL or missing references)
		//IL_1434: Unknown result type (might be due to invalid IL or missing references)
		//IL_143e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1339: Unknown result type (might be due to invalid IL or missing references)
		//IL_133e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1340: Unknown result type (might be due to invalid IL or missing references)
		//IL_1342: Unknown result type (might be due to invalid IL or missing references)
		//IL_134c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1351: Unknown result type (might be due to invalid IL or missing references)
		//IL_1362: Unknown result type (might be due to invalid IL or missing references)
		//IL_1364: Unknown result type (might be due to invalid IL or missing references)
		//IL_1366: Unknown result type (might be due to invalid IL or missing references)
		//IL_136b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1372: Unknown result type (might be due to invalid IL or missing references)
		//IL_1375: Unknown result type (might be due to invalid IL or missing references)
		//IL_137f: Unknown result type (might be due to invalid IL or missing references)
		//IL_044e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1643: Unknown result type (might be due to invalid IL or missing references)
		//IL_164f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1655: Unknown result type (might be due to invalid IL or missing references)
		//IL_165a: Unknown result type (might be due to invalid IL or missing references)
		//IL_165f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1665: Unknown result type (might be due to invalid IL or missing references)
		//IL_1671: Unknown result type (might be due to invalid IL or missing references)
		//IL_0474: Unknown result type (might be due to invalid IL or missing references)
		//IL_047e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0483: Unknown result type (might be due to invalid IL or missing references)
		//IL_17b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_17c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_17c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_17cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_17d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_17d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_17dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_16ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_04da: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0500: Unknown result type (might be due to invalid IL or missing references)
		//IL_0505: Unknown result type (might be due to invalid IL or missing references)
		//IL_0507: Unknown result type (might be due to invalid IL or missing references)
		//IL_0509: Unknown result type (might be due to invalid IL or missing references)
		//IL_050e: Unknown result type (might be due to invalid IL or missing references)
		//IL_17ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_1803: Unknown result type (might be due to invalid IL or missing references)
		//IL_1809: Unknown result type (might be due to invalid IL or missing references)
		//IL_1813: Unknown result type (might be due to invalid IL or missing references)
		//IL_1818: Unknown result type (might be due to invalid IL or missing references)
		//IL_1827: Unknown result type (might be due to invalid IL or missing references)
		//IL_182d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1837: Unknown result type (might be due to invalid IL or missing references)
		//IL_183c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1841: Unknown result type (might be due to invalid IL or missing references)
		//IL_1844: Unknown result type (might be due to invalid IL or missing references)
		//IL_1845: Unknown result type (might be due to invalid IL or missing references)
		//IL_1859: Unknown result type (might be due to invalid IL or missing references)
		//IL_185e: Unknown result type (might be due to invalid IL or missing references)
		//IL_186b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1877: Unknown result type (might be due to invalid IL or missing references)
		//IL_1880: Unknown result type (might be due to invalid IL or missing references)
		//IL_1886: Unknown result type (might be due to invalid IL or missing references)
		//IL_16df: Unknown result type (might be due to invalid IL or missing references)
		//IL_16eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_16f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_16f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_1959: Unknown result type (might be due to invalid IL or missing references)
		//IL_195d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1963: Unknown result type (might be due to invalid IL or missing references)
		//IL_196d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1972: Unknown result type (might be due to invalid IL or missing references)
		//IL_1981: Unknown result type (might be due to invalid IL or missing references)
		//IL_1987: Unknown result type (might be due to invalid IL or missing references)
		//IL_1991: Unknown result type (might be due to invalid IL or missing references)
		//IL_1996: Unknown result type (might be due to invalid IL or missing references)
		//IL_199b: Unknown result type (might be due to invalid IL or missing references)
		//IL_199e: Unknown result type (might be due to invalid IL or missing references)
		//IL_199f: Unknown result type (might be due to invalid IL or missing references)
		//IL_19a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_19ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_19bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_19c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_19d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_19d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_18a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_18ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_18b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_18bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_18c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_18d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_18d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_18e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_18e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_18eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_18ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_18ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_1915: Unknown result type (might be due to invalid IL or missing references)
		//IL_191a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1927: Unknown result type (might be due to invalid IL or missing references)
		//IL_1933: Unknown result type (might be due to invalid IL or missing references)
		//IL_193c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1942: Unknown result type (might be due to invalid IL or missing references)
		//IL_1735: Unknown result type (might be due to invalid IL or missing references)
		//IL_1737: Unknown result type (might be due to invalid IL or missing references)
		//IL_173a: Unknown result type (might be due to invalid IL or missing references)
		//IL_173f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1744: Unknown result type (might be due to invalid IL or missing references)
		//IL_1747: Unknown result type (might be due to invalid IL or missing references)
		//IL_1766: Unknown result type (might be due to invalid IL or missing references)
		//IL_176d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0613: Unknown result type (might be due to invalid IL or missing references)
		//IL_061d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0622: Unknown result type (might be due to invalid IL or missing references)
		//IL_062f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0639: Unknown result type (might be due to invalid IL or missing references)
		//IL_063e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0640: Unknown result type (might be due to invalid IL or missing references)
		//IL_0642: Unknown result type (might be due to invalid IL or missing references)
		//IL_0647: Unknown result type (might be due to invalid IL or missing references)
		//IL_057d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0584: Unknown result type (might be due to invalid IL or missing references)
		//IL_0589: Unknown result type (might be due to invalid IL or missing references)
		//IL_1acc: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ad1: Unknown result type (might be due to invalid IL or missing references)
		//IL_1adf: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ae1: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ae8: Unknown result type (might be due to invalid IL or missing references)
		//IL_1af2: Unknown result type (might be due to invalid IL or missing references)
		//IL_1afc: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a07: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a0b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a11: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a1b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a20: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a2f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a35: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a3f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a44: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a49: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a4b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a50: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a5d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a69: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a6d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a79: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a7f: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_05fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b4f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b54: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b62: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b64: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b6b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b75: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b7f: Unknown result type (might be due to invalid IL or missing references)
		Player player = Main.player[Main.myPlayer];
		Item item = inv[slot];
		float inventoryScale = Main.inventoryScale;
		Color val = Color.White;
		if (lightColor != Color.Transparent)
		{
			val = lightColor;
		}
		bool flag = false;
		switch (context)
		{
		case 36:
			flag = true;
			context = 13;
			break;
		case 13:
			if (slot == player.selectedItem && !player.selectedItemState.HasActiveOverride)
			{
				flag = true;
			}
			break;
		}
		bool flag2 = false;
		int num = 0;
		int gamepadPointForSlot = GetGamepadPointForSlot(inv, context, slot);
		if (PlayerInput.UsingGamepadUI)
		{
			flag2 = UILinkPointNavigator.CurrentPoint == gamepadPointForSlot;
			if (PlayerInput.SettingsForUI.PreventHighlightsForGamepad)
			{
				flag2 = false;
			}
			if (context == 0)
			{
				num = player.DpadRadial.GetDrawMode(slot);
				if (num > 0 && !PlayerInput.CurrentProfile.UsingDpadHotbar())
				{
					num = 0;
				}
			}
		}
		Texture2D value = TextureAssets.InventoryBack.Value;
		Color val2 = Main.inventoryBack;
		bool flag3 = false;
		bool highlightThingsForMouse = PlayerInput.SettingsForUI.HighlightThingsForMouse;
		if (!item.IsAir && item.favorited && retainFavoriteOnMoveAt[context])
		{
			value = TextureAssets.InventoryBack10.Value;
			if (context == 32)
			{
				value = TextureAssets.InventoryBack19.Value;
			}
		}
		else if (!item.IsAir && (DrawNewAndShinyHighlightOverride || (Options.HighlightNewItems && item.newAndShiny && !hideNewAndShinyHighlightAt[context] && !ItemID.Sets.NeverAppearsAsNewInInventory[item.type])))
		{
			value = TextureAssets.InventoryBack15.Value;
			float num2 = (float)(int)Main.mouseTextColor / 255f;
			num2 = num2 * 0.2f + 0.8f;
			val2 = val2.MultiplyRGBA(new Color(num2, num2, num2));
		}
		else if (!highlightThingsForMouse && item.type > 0 && item.stack > 0 && num != 0 && context != 13 && context != 21 && context != 37 && context != 22 && context != 35)
		{
			value = TextureAssets.InventoryBack15.Value;
			float num3 = (float)(int)Main.mouseTextColor / 255f;
			num3 = num3 * 0.2f + 0.8f;
			val2 = ((num != 1) ? val2.MultiplyRGBA(new Color(num3 / 2f, num3, num3 / 2f)) : val2.MultiplyRGBA(new Color(num3, num3 / 2f, num3 / 2f)));
		}
		else if (context == 0 && slot < 10)
		{
			value = TextureAssets.InventoryBack9.Value;
		}
		else if (context == 28)
		{
			value = TextureAssets.InventoryBack7.Value;
			val2 = Color.White;
		}
		else if (context == 16 || context == 17 || context == 19 || context == 18 || context == 20 || context == 17)
		{
			value = TextureAssets.InventoryBack3.Value;
		}
		else
		{
			switch (context)
			{
			case 8:
			case 10:
				value = TextureAssets.InventoryBack13.Value;
				val2 = GetColorByLoadout(slot, context);
				break;
			case 23:
			case 24:
			case 26:
			case 38:
			case 39:
				value = TextureAssets.InventoryBack8.Value;
				break;
			case 9:
			case 11:
				value = TextureAssets.InventoryBack13.Value;
				val2 = GetColorByLoadout(slot, context);
				break;
			case 25:
			case 27:
			case 33:
				value = TextureAssets.InventoryBack12.Value;
				break;
			case 12:
				value = TextureAssets.InventoryBack13.Value;
				val2 = GetColorByLoadout(slot, context);
				break;
			case 3:
				value = TextureAssets.InventoryBack5.Value;
				break;
			case 4:
			case 32:
				value = TextureAssets.InventoryBack2.Value;
				break;
			case 5:
			case 7:
				value = TextureAssets.InventoryBack4.Value;
				break;
			case 6:
				value = TextureAssets.InventoryBack7.Value;
				break;
			case 13:
			{
				byte b = 200;
				if (slot == Main.LocalPlayer.selectedItemState.Hotbar)
				{
					value = TextureAssets.InventoryBack20.Value;
					b = byte.MaxValue;
				}
				if (flag)
				{
					value = TextureAssets.InventoryBack14.Value;
					b = byte.MaxValue;
				}
				val2 = new Color((int)b, (int)b, (int)b, (int)b);
				break;
			}
			case 14:
			case 21:
			case 37:
				flag3 = true;
				break;
			case 15:
				value = TextureAssets.InventoryBack6.Value;
				break;
			case 29:
				val2 = new Color(53, 69, 127, 255);
				value = TextureAssets.InventoryBack18.Value;
				break;
			case 34:
				val2 = new Color(25, 44, 65, 180) * 0.9f;
				value = TextureAssets.InventoryBack18.Value;
				break;
			case 30:
				flag3 = !flag2;
				break;
			case 22:
			case 42:
			case 43:
				value = TextureAssets.InventoryBack4.Value;
				if (context == 42 || context == 43)
				{
					val2 = new Color(20, 40, 60, 180) * 0.9f;
					val2 = new Color(16, 36, 56, 180) * 0.9f;
					val2 = Utils.ShiftBlueToCyanTheme(val2);
					value = TextureAssets.InventoryBack18.Value;
					if (slot == 0)
					{
						value = TextureAssets.InventoryBack18.Value;
					}
				}
				if (DrawGoldBGForCraftingMaterial)
				{
					DrawGoldBGForCraftingMaterial = false;
					value = TextureAssets.InventoryBack14.Value;
					float num5 = (float)(int)val2.A / 255f;
					num5 = ((!(num5 < 0.7f)) ? 1f : Utils.GetLerpValue(0f, 0.7f, num5, clamped: true));
					val2 = Color.White * num5;
				}
				break;
			case 35:
				value = TextureAssets.InventoryBack2.Value;
				if (DrawGoldBGForCraftingMaterial)
				{
					DrawGoldBGForCraftingMaterial = false;
					value = TextureAssets.InventoryBack14.Value;
					float num4 = (float)(int)val2.A / 255f;
					num4 = ((!(num4 < 0.7f)) ? 1f : Utils.GetLerpValue(0f, 0.7f, num4, clamped: true));
					val2 = Color.White * num4;
				}
				break;
			case 41:
				val2 = new Color(20, 40, 60, 180) * 0.9f;
				val2 = new Color(16, 36, 56, 180) * 0.9f;
				val2 = Utils.ShiftBlueToCyanTheme(val2);
				value = TextureAssets.InventoryBack18.Value;
				break;
			}
		}
		if ((context == 0 || context == 2) && inventoryGlowTime[slot] > 0 && !inv[slot].favorited && !inv[slot].IsAir)
		{
			float num6 = Main.invAlpha / 255f;
			Color val3 = new Color(63, 65, 151, 255) * num6;
			Color val4 = Main.hslToRgb(inventoryGlowHue[slot], 1f, 0.5f) * num6;
			float num7 = (float)inventoryGlowTime[slot] / 300f;
			num7 *= num7;
			val2 = Color.Lerp(val3, val4, num7 / 2f);
			value = TextureAssets.InventoryBack13.Value;
		}
		if ((context == 4 || context == 32 || context == 3) && inventoryGlowTimeChest[slot] > 0 && !inv[slot].favorited && !inv[slot].IsAir)
		{
			float num8 = Main.invAlpha / 255f;
			Color val5 = new Color(130, 62, 102, 255) * num8;
			if (context == 3)
			{
				val5 = new Color(104, 52, 52, 255) * num8;
			}
			Color val6 = Main.hslToRgb(inventoryGlowHueChest[slot], 1f, 0.5f) * num8;
			float num9 = (float)inventoryGlowTimeChest[slot] / 300f;
			num9 *= num9;
			val2 = Color.Lerp(val5, val6, num9 / 2f);
			value = TextureAssets.InventoryBack13.Value;
		}
		if (flag2)
		{
			value = TextureAssets.InventoryBack14.Value;
			val2 = Color.White;
			if (item.favorited)
			{
				value = TextureAssets.InventoryBack17.Value;
			}
			if (context == 34)
			{
				val2 = Color.Gray;
			}
		}
		if (context == 41 || context == 43 || context == 42)
		{
			val2 = val2.MultiplyRGBA(lightColor);
		}
		if (context == 28 && Main.MouseScreen.Between(position, position + value.Size() * inventoryScale) && !player.mouseInterface)
		{
			value = TextureAssets.InventoryBack14.Value;
			val2 = Color.White;
		}
		CoinSlot.UpdateDrawState(slot, context, item, out var drawState);
		float itemFade = 1f;
		GetDimSlotForMouseItem(context, slot, Main.mouseItem, out itemFade);
		val2 *= itemFade;
		if (!flag3)
		{
			spriteBatch.Draw(value, position, (Rectangle?)null, val2, 0f, default(Vector2), inventoryScale, (SpriteEffects)0, 0f);
			if (context == 32 || context == 0 || context == 2 || context == 1)
			{
				int num10 = ((context == 32) ? (slot + PlayerItemSlotID.Bank4_0) : slot);
				PulseEffect pulseEffect = playerSlotPulseEffects[num10];
				if (pulseEffect.IsActive)
				{
					float num11 = PulseEffect.EffectDuration;
					float num12 = 0.5f;
					float num13 = (float)Math.PI;
					float num14 = (float)pulseEffect.time / num11;
					Color val7 = pulseEffect.color * (float)(0.5 + 0.2 * (0.0 - Math.Cos((double)num14 * Math.PI * 2.0 * (double)num12 + (double)num13)));
					val7 *= 1f - num14 * num14 * num14 * num14;
					spriteBatch.Draw(TextureAssets.InventoryBack21.Value, position, (Rectangle?)null, val7, 0f, default(Vector2), inventoryScale, (SpriteEffects)0, 0f);
				}
			}
			if (context == 41 && DrawSelectionHighlightForGridSlot)
			{
				spriteBatch.Draw(TextureAssets.InventoryBack24.Value, position, (Rectangle?)null, Main.inventoryBack, 0f, default(Vector2), inventoryScale, (SpriteEffects)0, 0f);
			}
		}
		if (ShouldHighlightSlotForMouseItem(context, slot, Main.mouseItem))
		{
			Color color = val2;
			if (value == TextureAssets.InventoryBack3.Value)
			{
				color = new Color(50, 106, 46, (int)val2.A);
			}
			else if (value == TextureAssets.InventoryBack8.Value)
			{
				color = new Color(46, 106, 98, (int)val2.A);
			}
			else if (value == TextureAssets.InventoryBack12.Value)
			{
				color = new Color(45, 85, 105, (int)val2.A);
			}
			else if (value == TextureAssets.InventoryBack13.Value)
			{
				TryGetSlotColor(Main.LocalPlayer.CurrentLoadoutIndex, context, out color);
				color.A = val2.A;
			}
			color *= 2f;
			spriteBatch.Draw(TextureAssets.InventoryBack22.Value, position, (Rectangle?)null, color, 0f, default(Vector2), inventoryScale, (SpriteEffects)0, 0f);
		}
		else if (!item.IsAir && item.favorited)
		{
			_ = canLoadoutShareAt[context];
		}
		int num15 = -1;
		switch (context)
		{
		case 8:
		case 23:
			if (slot == 0)
			{
				num15 = 0;
			}
			if (slot == 1)
			{
				num15 = 6;
			}
			if (slot == 2)
			{
				num15 = 12;
			}
			break;
		case 26:
			num15 = 0;
			break;
		case 9:
			if (slot == 10)
			{
				num15 = 3;
			}
			if (slot == 11)
			{
				num15 = 9;
			}
			if (slot == 12)
			{
				num15 = 15;
			}
			break;
		case 10:
		case 24:
			num15 = 11;
			break;
		case 11:
			num15 = 2;
			break;
		case 12:
		case 25:
		case 27:
		case 33:
			num15 = 1;
			break;
		case 16:
			num15 = 4;
			break;
		case 17:
		case 39:
			num15 = 13;
			break;
		case 19:
			num15 = 10;
			break;
		case 18:
			num15 = 7;
			break;
		case 20:
			num15 = 17;
			break;
		case 7:
			num15 = 18;
			break;
		}
		int num16 = -1;
		float num17 = 1f;
		int num18 = -1;
		if (canLoadoutShareAt[context])
		{
			Item sharedLoadoutItem = GetSharedLoadoutItem(context, slot, out var sharedFromLoadout);
			if (item.IsAir && sharedFromLoadout.HasValue)
			{
				item = sharedLoadoutItem;
				num15 = -1;
				num17 = itemFade;
				num16 = sharedFromLoadout.Value;
				itemFade *= 0.6f;
			}
			else if (!item.IsAir && item.favorited)
			{
				num18 = Main.LocalPlayer.CurrentLoadoutIndex;
			}
		}
		if (item.IsAir && num15 != -1)
		{
			float num19 = 0.35f;
			Texture2D value2 = TextureAssets.Extra[54].Value;
			Rectangle val8 = value2.Frame(3, 7, num15 % 3, num15 / 3);
			val8.Width -= 2;
			val8.Height -= 2;
			spriteBatch.Draw(value2, position + value.Size() / 2f * inventoryScale, (Rectangle?)val8, Color.White * (num19 * itemFade), 0f, val8.Size() / 2f, inventoryScale, (SpriteEffects)0, 0f);
		}
		Vector2 val9 = value.Size() * inventoryScale;
		bool flag4 = (item.type > 0 && item.stack > 0) || drawState.fadeItem > 0;
		if (flag4)
		{
			ItemDisplayKey key = new ItemDisplayKey
			{
				Context = context,
				Slot = slot
			};
			if (_nextTickDrawAvailable.TryGetValue(key, out var value3) && value3 > Main.EverLastingTicker)
			{
				flag4 = false;
			}
		}
		if (flag4)
		{
			float num20;
			if (item.IsACoin || drawState.fadeItem > 0)
			{
				num20 = CoinSlot.DrawItemCoin(spriteBatch, position + val9 / 2f - new Vector2(0f, drawState.coinYOffset * inventoryScale), (drawState.fadeItem > 0) ? drawState.fadeItem : item.type, drawState.coinAnimFrame, inventoryScale, 32f, val, itemFade * drawState.fadeScale);
			}
			else
			{
				num20 = ((item.type != 3817) ? DrawItemIcon(item, context, spriteBatch, position + val9 / 2f, inventoryScale, 32f, val, itemFade) : DrawItemIcon(item, context, spriteBatch, position + val9 / 2f - new Vector2(0f, drawState.coinYOffset * inventoryScale), inventoryScale, 32f, val, itemFade));
			}
			if (Main.DoGlowingMouseItemDraw)
			{
				float glow = CraftingEffects.GetGlow(item);
				float num21 = glow;
				num21 *= num21;
				bool flag5 = true;
				if (!Main.FlashyEffectsInterface)
				{
					flag5 = false;
				}
				if ((glow > 0f) & flag5)
				{
					float num22 = Utils.Remap(num21, 1f, 0f, 1f, 2f);
					float fromValue = Utils.Remap(Main.stackCounter, 0f, 8f, 0f, 1f);
					if (Main.superFastStack > 0)
					{
						fromValue = 1f;
					}
					Utils.Remap(fromValue, 0f, 1f, 1f, 0.5f);
					num21 *= Utils.Remap(glow, 1f, 0.95f, 0.5f, 1f);
					MiscShaderData miscShaderData = GameShaders.Misc["MouseItem"];
					miscShaderData.UseSaturation(num21);
					miscShaderData.UseColor(Main.OurFavoriteColor);
					miscShaderData.Apply();
					num20 = ((!item.IsACoin && drawState.fadeItem <= 0) ? DrawItemIcon(item, context, spriteBatch, position + val9 / 2f, inventoryScale * num22, 32f, val, itemFade) : CoinSlot.DrawItemCoin(spriteBatch, position + val9 / 2f - new Vector2(0f, drawState.coinYOffset * inventoryScale), (drawState.fadeItem > 0) ? drawState.fadeItem : item.type, drawState.coinAnimFrame, inventoryScale * num22, 32f, val, itemFade * drawState.fadeScale));
					Main.pixelShader.CurrentTechnique.Passes[0].Apply();
				}
			}
			if (item.type == 5324 || item.type == 5329 || item.type == 5330)
			{
				Vector2 val10 = new Vector2(2f, -6f) * inventoryScale;
				switch (item.type)
				{
				case 5324:
				{
					Texture2D value6 = TextureAssets.Extra[257].Value;
					Rectangle val13 = value6.Frame(3, 1, 2);
					spriteBatch.Draw(value6, position + val10 + new Vector2(40f, 40f) * inventoryScale, (Rectangle?)val13, val * itemFade, 0f, val13.Size() / 2f, 1f, (SpriteEffects)0, 0f);
					break;
				}
				case 5329:
				{
					Texture2D value5 = TextureAssets.Extra[257].Value;
					Rectangle val12 = value5.Frame(3, 1, 1);
					spriteBatch.Draw(value5, position + val10 + new Vector2(40f, 40f) * inventoryScale, (Rectangle?)val12, val * itemFade, 0f, val12.Size() / 2f, 1f, (SpriteEffects)0, 0f);
					break;
				}
				case 5330:
				{
					Texture2D value4 = TextureAssets.Extra[257].Value;
					Rectangle val11 = value4.Frame(3);
					spriteBatch.Draw(value4, position + val10 + new Vector2(40f, 40f) * inventoryScale, (Rectangle?)val11, val * itemFade, 0f, val11.Size() / 2f, 1f, (SpriteEffects)0, 0f);
					break;
				}
				}
			}
			if (item.type == 6168 || item.type == 6169 || item.type == 6193 || item.type == 6194)
			{
				Texture2D value7 = TextureAssets.Extra[308].Value;
				Vector2 val14 = new Vector2(39f, 40f) * inventoryScale;
				switch (item.type)
				{
				case 6168:
				{
					Rectangle val21 = value7.Frame(1, 4);
					Vector2 val22 = val21.Size() / 2f;
					val21.Height--;
					spriteBatch.Draw(value7, position + val14, (Rectangle?)val21, val * itemFade, 0f, val22, inventoryScale, (SpriteEffects)0, 0f);
					break;
				}
				case 6169:
				{
					Rectangle val19 = value7.Frame(1, 4, 0, 1);
					Vector2 val20 = val19.Size() / 2f;
					val19.Height--;
					spriteBatch.Draw(value7, position + val14, (Rectangle?)val19, val * itemFade, 0f, val20, inventoryScale, (SpriteEffects)0, 0f);
					break;
				}
				case 6193:
				{
					Rectangle val17 = value7.Frame(1, 4, 0, 2);
					Vector2 val18 = val17.Size() / 2f;
					val17.Height--;
					spriteBatch.Draw(value7, position + val14, (Rectangle?)val17, val * itemFade, 0f, val18, inventoryScale, (SpriteEffects)0, 0f);
					break;
				}
				case 6194:
				{
					Rectangle val15 = value7.Frame(1, 4, 0, 3);
					Vector2 val16 = val15.Size() / 2f;
					val15.Height--;
					spriteBatch.Draw(value7, position + val14, (Rectangle?)val15, val * itemFade, 0f, val16, inventoryScale, (SpriteEffects)0, 0f);
					break;
				}
				}
			}
			int num23 = -1;
			if (context == 13)
			{
				if (item.DD2Summon)
				{
					for (int i = 0; i < 58; i++)
					{
						if (inv[i].type == 3822)
						{
							num23 += inv[i].stack;
						}
					}
					if (num23 >= 0)
					{
						num23++;
					}
				}
				if (item.useAmmo > 0)
				{
					int useAmmo = item.useAmmo;
					num23 = 0;
					for (int j = 0; j < 58; j++)
					{
						if (inv[j].ammo == useAmmo)
						{
							num23 += inv[j].stack;
						}
					}
				}
				if (item.fishingPole > 0)
				{
					num23 = 0;
					for (int k = 0; k < 58; k++)
					{
						if (inv[k].bait > 0)
						{
							num23 += inv[k].stack;
						}
					}
				}
				if (item.tileWand > 0)
				{
					int tileWand = item.tileWand;
					num23 = 0;
					for (int l = 0; l < 58; l++)
					{
						if (inv[l].type == tileWand)
						{
							num23 += inv[l].stack;
						}
					}
				}
				if (player.TryGetFlexibleWandAvailableUsageCount(item, out var amount))
				{
					num23 = amount;
				}
				else if (item.GetFlexibleTileWand() != null && item.GetFlexibleTileWand().ShowsHoverAmmoIcon)
				{
					num23 = 0;
				}
				int type = item.type;
				if ((uint)(type - 1071) <= 1u || (uint)(type - 1543) <= 1u)
				{
					Item item2 = player.FindPaintOrCoating();
					if (item2 != null)
					{
						num23 = item2.stack;
					}
				}
				if (item.type == 509 || item.type == 851 || item.type == 850 || item.type == 3612 || item.type == 3625 || item.type == 3611)
				{
					num23 = 0;
					for (int m = 0; m < 58; m++)
					{
						if (inv[m].type == 530)
						{
							num23 += inv[m].stack;
						}
					}
				}
			}
			if (num23 != -1)
			{
				ChatManager.DrawColorCodedStringWithShadow(spriteBatch, FontAssets.ItemStack.Value, num23.ToString(), position + new Vector2(8f, 30f) * inventoryScale, val, 0f, Vector2.Zero, new Vector2(inventoryScale * 0.8f), -1f, inventoryScale);
			}
			if (context != 37 && (item.stack > 1 || drawState.stackTextDrawFadeOverload > 0f) && num23 == -1)
			{
				Vector2 val23 = new Vector2(10f, (float)(26 + FontAssets.ItemStack.Value.LineSpacing));
				float num24 = inventoryScale * drawState.stackTextScale;
				if (context == 43)
				{
					val23 += new Vector2(-5f, 7f);
					num24 *= 1.2f;
				}
				float num25 = ((drawState.stackTextDrawFadeOverload > 0f) ? drawState.stackTextDrawFadeOverload : 1f);
				ChatManager.DrawColorCodedStringWithShadow(spriteBatch, FontAssets.ItemStack.Value, item.stack.ToString(), position + val23 * inventoryScale, val * num25, 0f, new Vector2(0f, (float)FontAssets.ItemStack.Value.LineSpacing), new Vector2(num24), -1f, num24);
			}
			if (context == 13)
			{
				string text = string.Concat(slot + 1);
				if (text == "10")
				{
					text = "0";
				}
				ChatManager.DrawColorCodedStringWithShadow(spriteBatch, FontAssets.ItemStack.Value, text, position + new Vector2(8f, 4f) * inventoryScale, val, 0f, Vector2.Zero, new Vector2(inventoryScale), -1f, inventoryScale);
			}
			if (context == 13 && item.potion)
			{
				Vector2 val24 = position + value.Size() * inventoryScale / 2f - TextureAssets.Cd.Value.Size() * inventoryScale / 2f;
				Color val25 = item.GetAlpha(val) * ((float)player.potionDelay / (float)player.potionDelayTime);
				spriteBatch.Draw(TextureAssets.Cd.Value, val24, (Rectangle?)null, val25, 0f, default(Vector2), num20, (SpriteEffects)0, 0f);
			}
			if (context == 13 && item.healMana > 0)
			{
				Vector2 val26 = position + value.Size() * inventoryScale / 2f - TextureAssets.Cd.Value.Size() * inventoryScale / 2f;
				Color val27 = item.GetAlpha(val) * Utils.Remap(player.manaPotionDelay, 300f, 0f, 1f, 0f);
				spriteBatch.Draw(TextureAssets.Cd.Value, val26, (Rectangle?)null, val27, 0f, default(Vector2), num20, (SpriteEffects)0, 0f);
			}
			if (context == 34)
			{
				Vector2 val28 = position + value.Size() * inventoryScale / 2f - TextureAssets.Cd.Value.Size() * inventoryScale / 2f;
				Color val29 = item.GetAlpha(val) * 0.5f;
				spriteBatch.Draw(TextureAssets.Cd.Value, val28, (Rectangle?)null, val29, 0f, default(Vector2), num20, (SpriteEffects)0, 0f);
			}
			if ((context == 10 || context == 18) && item.expertOnly && !Main.expertMode)
			{
				Vector2 val30 = position + value.Size() * inventoryScale / 2f - TextureAssets.Cd.Value.Size() * inventoryScale / 2f;
				Color white = Color.White;
				spriteBatch.Draw(TextureAssets.Cd.Value, val30, (Rectangle?)null, white * itemFade, 0f, default(Vector2), num20, (SpriteEffects)0, 0f);
			}
			if (num18 != -1)
			{
				float num26 = 0.7f;
				int num27 = (int)MathHelper.Clamp((float)(310 + num18), 310f, 312f);
				Texture2D value8 = TextureAssets.Extra[num27].Value;
				Rectangle value9 = value8.Frame();
				float num28 = 1f * inventoryScale;
				spriteBatch.Draw(value8, position, (Rectangle?)value9, Color.White * (num26 * num17), 0f, Vector2.Zero, num28, (SpriteEffects)0, 0f);
			}
			if (num16 != -1)
			{
				float num29 = 0.7f;
				int num30 = (int)MathHelper.Clamp((float)(313 + num16), 313f, 315f);
				Texture2D value10 = TextureAssets.Extra[num30].Value;
				Rectangle value11 = value10.Frame();
				float num31 = 1f * inventoryScale;
				spriteBatch.Draw(value10, position, (Rectangle?)value11, Color.White * (num29 * num17), 0f, Vector2.Zero, num31, (SpriteEffects)0, 0f);
			}
		}
		else if (context == 6)
		{
			Texture2D value12 = TextureAssets.Trash.Value;
			Vector2 val31 = position + value.Size() * inventoryScale / 2f - value12.Size() * inventoryScale / 2f;
			spriteBatch.Draw(value12, val31, (Rectangle?)null, new Color(100, 100, 100, 100), 0f, default(Vector2), inventoryScale, (SpriteEffects)0, 0f);
		}
		if (context == 0 && slot < 10)
		{
			float num32 = inventoryScale;
			string text2 = string.Concat(slot + 1);
			if (text2 == "10")
			{
				text2 = "0";
			}
			Color baseColor = Main.inventoryBack;
			int num33 = 0;
			if (Main.player[Main.myPlayer].selectedItem == slot)
			{
				baseColor = Color.White;
				baseColor.A = 200;
				num33 -= 2;
				num32 *= 1.4f;
			}
			ChatManager.DrawColorCodedStringWithShadow(spriteBatch, FontAssets.ItemStack.Value, text2, position + new Vector2(6f, (float)(4 + num33)) * inventoryScale, baseColor, 0f, Vector2.Zero, new Vector2(inventoryScale), -1f, inventoryScale);
		}
		if (gamepadPointForSlot != -1)
		{
			UILinkPointNavigator.SetPosition(gamepadPointForSlot, position + val9 * 0.75f);
		}
	}

	public static Color GetColorByLoadout(int slot, int context)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		Color val = Color.White;
		if (TryGetSlotColor(Main.LocalPlayer.CurrentLoadoutIndex, context, out var color))
		{
			val = color;
		}
		Color val2 = new Color(val.ToVector4() * Main.inventoryBack.ToVector4());
		float num = Utils.Remap((float)(Main.timeForVisualEffects - _lastTimeForVisualEffectsThatLoadoutWasChanged), 0f, 30f, 0.5f, 0f);
		if (!Main.FlashyEffectsInterface)
		{
			num = 0f;
		}
		return Color.Lerp(val2, Color.White, num * num * num);
	}

	public static void RecordLoadoutChange()
	{
		_lastTimeForVisualEffectsThatLoadoutWasChanged = Main.timeForVisualEffects;
	}

	public static bool TryGetSlotColor(int loadoutIndex, int context, out Color color)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		color = default;
		if (loadoutIndex < 0 || loadoutIndex >= 3)
		{
			return false;
		}
		int num = -1;
		switch (context)
		{
		case 8:
		case 10:
			num = 0;
			break;
		case 9:
		case 11:
			num = 1;
			break;
		case 12:
			num = 2;
			break;
		}
		if (num == -1)
		{
			return false;
		}
		color = LoadoutSlotColors[loadoutIndex, num];
		return true;
	}

	public static float ShiftHueByLoadout(float hue, int loadoutIndex)
	{
		return (hue + (float)loadoutIndex / 8f) % 1f;
	}

	public static Color GetLoadoutColor(int loadoutIndex)
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		return Main.hslToRgb(ShiftHueByLoadout(0.41f, loadoutIndex), 0.7f, 0.5f);
	}

	public static Item GetSharedLoadoutItem(int context, int slot, out int? sharedFromLoadout)
	{
		if (context != 12)
		{
			return Main.LocalPlayer.GetEffectiveArmor(slot, out sharedFromLoadout);
		}
		return Main.LocalPlayer.GetEffectiveDye(slot, out sharedFromLoadout);
	}

	public static float DrawItemIcon(Item item, int context, SpriteBatch spriteBatch, Vector2 screenPositionForItemCenter, float scale, float sizeLimit, Color environmentColor, float itemFade = 1f, bool flip = false)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0102: Unknown result type (might be due to invalid IL or missing references)
		//IL_0107: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0113: Unknown result type (might be due to invalid IL or missing references)
		//IL_0115: Unknown result type (might be due to invalid IL or missing references)
		//IL_012a: Unknown result type (might be due to invalid IL or missing references)
		//IL_012b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0133: Unknown result type (might be due to invalid IL or missing references)
		//IL_0135: Unknown result type (might be due to invalid IL or missing references)
		//IL_013a: Unknown result type (might be due to invalid IL or missing references)
		//IL_013b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0142: Unknown result type (might be due to invalid IL or missing references)
		//IL_014c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0153: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_03fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0406: Unknown result type (might be due to invalid IL or missing references)
		//IL_040d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0412: Unknown result type (might be due to invalid IL or missing references)
		//IL_041f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0420: Unknown result type (might be due to invalid IL or missing references)
		//IL_0422: Unknown result type (might be due to invalid IL or missing references)
		//IL_042c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0436: Unknown result type (might be due to invalid IL or missing references)
		//IL_043a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0449: Unknown result type (might be due to invalid IL or missing references)
		//IL_047b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0480: Unknown result type (might be due to invalid IL or missing references)
		//IL_0482: Unknown result type (might be due to invalid IL or missing references)
		//IL_048c: Unknown result type (might be due to invalid IL or missing references)
		//IL_049b: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_04db: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_04fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0509: Unknown result type (might be due to invalid IL or missing references)
		//IL_0198: Unknown result type (might be due to invalid IL or missing references)
		//IL_019a: Unknown result type (might be due to invalid IL or missing references)
		//IL_019c: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0307: Unknown result type (might be due to invalid IL or missing references)
		//IL_030c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0283: Unknown result type (might be due to invalid IL or missing references)
		//IL_028e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0295: Unknown result type (might be due to invalid IL or missing references)
		//IL_029a: Unknown result type (might be due to invalid IL or missing references)
		//IL_023c: Unknown result type (might be due to invalid IL or missing references)
		//IL_057a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0595: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_05aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_05cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_021f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0224: Unknown result type (might be due to invalid IL or missing references)
		//IL_033a: Unknown result type (might be due to invalid IL or missing references)
		//IL_033b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0342: Unknown result type (might be due to invalid IL or missing references)
		//IL_0346: Unknown result type (might be due to invalid IL or missing references)
		//IL_0350: Unknown result type (might be due to invalid IL or missing references)
		//IL_0352: Unknown result type (might be due to invalid IL or missing references)
		//IL_035c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0366: Unknown result type (might be due to invalid IL or missing references)
		//IL_0379: Unknown result type (might be due to invalid IL or missing references)
		//IL_037e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0254: Unknown result type (might be due to invalid IL or missing references)
		//IL_0392: Unknown result type (might be due to invalid IL or missing references)
		//IL_0393: Unknown result type (might be due to invalid IL or missing references)
		//IL_039a: Unknown result type (might be due to invalid IL or missing references)
		//IL_039e: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_03aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_03be: Unknown result type (might be due to invalid IL or missing references)
		Color secondColor = Color.White;
		Color secondColor2 = Color.White;
		int type = item.type;
		Main.instance.LoadItem(type);
		Texture2D value = TextureAssets.Item[type].Value;
		Rectangle frame = value.Frame();
		DrawAnimation drawAnimation = Main.itemAnimations[type];
		if (drawAnimation != null)
		{
			int frameCounterOverride = -1;
			if (type == 5644 && context != 31 && !Main.LocalPlayer.AnyoneToSpectate())
			{
				frameCounterOverride = 0;
			}
			frame = drawAnimation.GetFrame(value, frameCounterOverride);
		}
		float num = 1f;
		if (context == 37)
		{
			secondColor = OverdrawGlowColorMultiplier;
			secondColor2 = OverdrawGlowColorMultiplier;
			environmentColor = Color.White;
			num = OverdrawGlowSize;
		}
		DrawItem_GetColorAndScale(item, scale, ref environmentColor, sizeLimit, ref frame, out var itemLight, out var finalDrawScale);
		SpriteEffects val = (SpriteEffects)0;
		if (flip)
		{
			val = (SpriteEffects)1;
		}
		Vector2 val2 = frame.Size() / 2f;
		Color val3 = item.GetAlpha(itemLight).MultiplyRGBA(secondColor);
		spriteBatch.Draw(value, screenPositionForItemCenter, (Rectangle?)frame, val3 * itemFade, 0f, val2, finalDrawScale * num, val, 0f);
		if (item.color != Color.Transparent)
		{
			Color newColor = environmentColor;
			if (context == 13)
			{
				newColor.A = byte.MaxValue;
			}
			spriteBatch.Draw(value, screenPositionForItemCenter, (Rectangle?)frame, item.GetColor(newColor).MultiplyRGBA(secondColor2) * itemFade, 0f, val2, finalDrawScale * num, val, 0f);
		}
		if (item.glowMask != -1 && item.type != 3779 && item.type != 46 && item.type != 5462)
		{
			Rectangle val4 = frame;
			Color val5 = Color.White;
			switch (item.type)
			{
			case 5670:
			case 5671:
				val5 = Item.GetPhaseColor(item.shoot);
				break;
			case 5146:
				val5 = new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB);
				break;
			case 5462:
				val5 = new Color(255, 140, 0, 5);
				break;
			case 5669:
			{
				float num2 = Utils.WrappedLerp(0.5f, 1f, (float)(Main.LocalPlayer.miscCounter % 100) / 100f);
				val5 = Color.Lerp(val5, new Color(180, 85, 30), num2);
				val5.A = (byte)item.alpha;
				break;
			}
			case 3858:
				val5 = new Color(255, 255, 255, 63) * 0.75f;
				val4 = TextureAssets.GlowMask[233].Value.Frame(1, 3, 0, Main.LocalPlayer.miscCounter % 15 / 5);
				val4.Height -= 2;
				break;
			case 6148:
			case 6149:
			case 6150:
			case 6151:
			case 6174:
				val5 = GetTrustyPalGlowColor();
				break;
			}
			if (item.type == 5670 || item.type == 5671)
			{
				spriteBatch.Draw(TextureAssets.GlowMask[item.glowMask].Value, screenPositionForItemCenter, (Rectangle?)val4, val5 * itemFade, 0f, val4.Size() / 2f, finalDrawScale * num, val, 0f);
				val5 = Item.GetPhaseColor(item.shoot, drawColor: true);
			}
			spriteBatch.Draw(TextureAssets.GlowMask[item.glowMask].Value, screenPositionForItemCenter, (Rectangle?)val4, val5 * itemFade, 0f, val4.Size() / 2f, finalDrawScale * num, val, 0f);
		}
		if (ItemID.Sets.TrapSigned[item.type])
		{
			Vector2 val6 = new Vector2(1f, -1f);
			Vector2 val7 = frame.Size() * 0.45f * val6 * finalDrawScale * num;
			spriteBatch.Draw(TextureAssets.Wire.Value, screenPositionForItemCenter + val7, (Rectangle?)new Rectangle(4, 58, 8, 8), environmentColor * itemFade, 0f, new Vector2(4f), finalDrawScale * num, (SpriteEffects)0, 0f);
		}
		if (ItemID.Sets.DrawUnsafeIndicator[item.type])
		{
			Vector2 val8 = new Vector2(1f, -1f);
			Vector2 val9 = (frame.Size() * 0.45f + new Vector2(-4f, -4f)) * val8 * finalDrawScale * num;
			Texture2D value2 = TextureAssets.Extra[258].Value;
			Rectangle val10 = value2.Frame();
			spriteBatch.Draw(value2, screenPositionForItemCenter + val9, (Rectangle?)val10, environmentColor * itemFade, 0f, val10.Size() / 2f, finalDrawScale * num, (SpriteEffects)0, 0f);
		}
		if ((item.type == 5667 || item.type == 6174) && context == 13 && (Main.LocalPlayer.ownedProjectileCounts[1098] > 0 || Main.LocalPlayer.ownedProjectileCounts[1123] > 0))
		{
			Vector2 val11 = new Vector2(1f, -1f);
			TextureAssets.Extra[304].Value.Frame();
			_ = new Vector2(16f, 16f) * val11 * scale;
			Utils.DrawNotificationIcon(spriteBatch, screenPositionForItemCenter + new Vector2(14f, -8f) * scale, 0f, worldSpace: false, scale);
		}
		return finalDrawScale;
	}

	public static Color GetTrustyPalGlowColor()
	{
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		float num = Utils.WrappedLerp(0f, 1f, Main.GlobalTimeWrappedHourly / 3f % 1f);
		float num2 = Utils.Remap(num, 0.2f, 1f, 0f, 1f);
		return Main.hslToRgb((0.04f + 0.07f * num) % 1f, 1f, 0.5f, 127) * 0.5f * num2;
	}

	public static void DrawItem_GetColorAndScale(Item item, float scale, ref Color currentWhite, float sizeLimit, ref Rectangle frame, out Color itemLight, out float finalDrawScale)
	{
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		itemLight = currentWhite;
		float scale2 = 1f;
		GetItemLight(ref itemLight, ref scale2, item);
		float num = 1f;
		if ((float)frame.Width > sizeLimit || (float)frame.Height > sizeLimit)
		{
			num = ((frame.Width <= frame.Height) ? (sizeLimit / (float)frame.Height) : (sizeLimit / (float)frame.Width));
		}
		if (item.type == 5669 && sizeLimit == 20f)
		{
			num = 0.5f;
		}
		finalDrawScale = scale * num * scale2;
	}

	public static int GetGamepadPointForSlot(Item[] inv, int context, int slot)
	{
		Player localPlayer = Main.LocalPlayer;
		int result = -1;
		switch (context)
		{
		case 0:
		case 1:
		case 2:
			result = slot;
			break;
		case 8:
		case 9:
		case 10:
		case 11:
		{
			int num2 = slot;
			if (num2 % 10 == 9 && !localPlayer.CanDemonHeartAccessoryBeShown())
			{
				num2--;
			}
			result = 100 + num2;
			break;
		}
		case 12:
			if (inv == localPlayer.dye)
			{
				int num = slot;
				if (num % 10 == 9 && !localPlayer.CanDemonHeartAccessoryBeShown())
				{
					num--;
				}
				result = 120 + num;
			}
			break;
		case 33:
			if (inv == localPlayer.miscDyes)
			{
				result = 185 + slot;
			}
			break;
		case 19:
			result = 180;
			break;
		case 20:
			result = 181;
			break;
		case 18:
			result = 182;
			break;
		case 17:
			result = 183;
			break;
		case 16:
			result = 184;
			break;
		case 3:
		case 4:
		case 32:
			result = 400 + slot - ChestUI.StartingRowForDrawing * 10;
			break;
		case 15:
			result = 2700 + slot;
			break;
		case 6:
			result = 300;
			break;
		case 22:
			if (UILinkPointNavigator.Shortcuts.CRAFT_CurrentRecipeBig != -1)
			{
				result = 22000 + UILinkPointNavigator.Shortcuts.CRAFT_CurrentRecipeBig;
			}
			if (UILinkPointNavigator.Shortcuts.CRAFT_CurrentRecipeSmall != -1)
			{
				result = 1500 + UILinkPointNavigator.Shortcuts.CRAFT_CurrentRecipeSmall + 1;
			}
			break;
		case 35:
			if (UILinkPointNavigator.Shortcuts.CRAFT_CurrentRecipeSmall != -1)
			{
				result = 12000;
			}
			if (UILinkPointNavigator.Shortcuts.CRAFT_CurrentRecipeBig != -1)
			{
				result = 11100;
			}
			break;
		case 7:
			result = (NewCraftingUI.Visible ? 20020 : 1500);
			break;
		case 5:
			result = 303;
			break;
		case 23:
		case 24:
		case 39:
			result = 5100 + slot;
			break;
		case 25:
			result = 5109 + slot;
			break;
		case 38:
			result = 5118;
			break;
		case 26:
			result = 5000 + slot;
			break;
		case 27:
			result = 5002 + slot;
			break;
		case 29:
		case 34:
		case 41:
			result = 3000 + slot;
			if (UILinkPointNavigator.Shortcuts.ItemSlotShouldHighlightAsSelected)
			{
				result = UILinkPointNavigator.CurrentPoint;
			}
			break;
		case 30:
			result = 15000 + slot;
			break;
		case 42:
			result = 20000;
			break;
		case 43:
			result = 20001 + UILinkPointNavigator.Shortcuts.NewCraftingUI_MaterialIndex;
			break;
		}
		return result;
	}

	public static void MouseHover(int context = 0)
	{
		MouseHover(Main.HoverItem, context);
	}

	public static void MouseHover(Item item, int context = 0)
	{
		singleSlotArray[0] = item;
		MouseHover(singleSlotArray, context);
	}

	public static int GetBulkBuyAmount(Item item)
	{
		int num = 10;
		if (!item.buyOnce)
		{
			return num;
		}
		return Math.Min(num, item.stack);
	}

	public static bool CanBulkBuy(Item item)
	{
		if (item.isAShopItem || item.buyOnce)
		{
			return ShiftInUse;
		}
		return false;
	}

	public static int GetBulkCraftAmount(Item item)
	{
		int num = item.maxStack / item.stack;
		if (num < 1)
		{
			num = 1;
		}
		return Math.Min(num, 10);
	}

	public static int EstimateDisplayStack(Item item)
	{
		int num = (item.buyOnce ? 1 : item.stack);
		if (CanBulkBuy(item))
		{
			int bulkBuyAmount = GetBulkBuyAmount(item);
			return num * bulkBuyAmount;
		}
		if (Main.TryingToBulkCraft() && ((item.tooltipContext == 22 && item.tooltipSlot == 0) || item.tooltipContext == 42 || item.tooltipContext == 41))
		{
			return GetBulkCraftAmount(item) * item.stack;
		}
		return num;
	}

	public static void MouseHover(Item[] inv, int context = 0, int slot = 0)
	{
		if (context == 6 && Main.hoverItemName == null)
		{
			Main.hoverItemName = Lang.inter[3].Value;
		}
		Item item = inv[slot];
		if (canLoadoutShareAt[context] && item.IsAir)
		{
			item = GetSharedLoadoutItem(context, slot, out var _);
		}
		if (!item.IsAir)
		{
			_customCurrencyForSavings = item.shopSpecialCurrency;
			Main.hoverItemName = item.Name;
			if (item.stack > 1)
			{
				Main.hoverItemName = Main.hoverItemName + " (" + item.stack + ")";
			}
			Main.HoverItem = item.Clone();
			Main.HoverItem.tooltipContext = context;
			Main.HoverItem.tooltipSlot = slot;
			switch (context)
			{
			case 8:
				Main.HoverItem.wornArmor = true;
				break;
			case 9:
			case 11:
				Main.HoverItem.social = true;
				break;
			case 15:
				Main.HoverItem.buy = true;
				break;
			}
			return;
		}
		if (context == 10 || context == 11 || context == 24)
		{
			Main.hoverItemName = Lang.inter[9].Value;
		}
		if (context == 11)
		{
			Main.hoverItemName = Lang.inter[11].Value + " " + Main.hoverItemName;
		}
		if (context == 8 || context == 9 || context == 23 || context == 26)
		{
			if (slot == 0 || slot == 10 || context == 26)
			{
				Main.hoverItemName = Lang.inter[12].Value;
			}
			else if (slot == 1 || slot == 11)
			{
				Main.hoverItemName = Lang.inter[13].Value;
			}
			else if (slot == 2 || slot == 12)
			{
				Main.hoverItemName = Lang.inter[14].Value;
			}
			else if (slot >= 10)
			{
				Main.hoverItemName = Lang.inter[11].Value + " " + Main.hoverItemName;
			}
		}
		if (context == 12 || context == 25 || context == 27 || context == 33)
		{
			Main.hoverItemName = Lang.inter[57].Value;
		}
		if (context == 16)
		{
			Main.hoverItemName = Lang.inter[90].Value;
		}
		if (context == 17 || context == 39)
		{
			Main.hoverItemName = Lang.inter[91].Value;
		}
		if (context == 19)
		{
			Main.hoverItemName = Lang.inter[92].Value;
		}
		if (context == 18)
		{
			Main.hoverItemName = Lang.inter[93].Value;
		}
		if (context == 20)
		{
			Main.hoverItemName = Lang.inter[94].Value;
		}
		if (context == 38)
		{
			Main.hoverItemName = Language.GetTextValue("UI.DisplayDollWeapon");
		}
	}

	public static void ResetInventoryStateCounters()
	{
		dyeSwapCounter = 0;
	}

	public static void SwapEquip(ref Item inv, int context = 0)
	{
		singleSlotArray[0] = inv;
		SwapEquip(singleSlotArray, context, 0);
		inv = singleSlotArray[0];
	}

	public static bool CanSwapEquip(Item item)
	{
		if (item.IsAir)
		{
			return false;
		}
		if (item.dye > 0 || Main.projHook[item.shoot] || item.mountType != -1 || (item.buffType > 0 && (Main.lightPet[item.buffType] || Main.vanityPet[item.buffType])) || item.headSlot != -1 || item.bodySlot != -1 || item.legSlot != -1 || item.accessory)
		{
			return true;
		}
		return false;
	}

	public static void SwapEquip(Item[] inv, int context, int slot)
	{
		Player player = Main.player[Main.myPlayer];
		if (inv[slot].IsAir)
		{
			return;
		}
		bool success;
		if (inv[slot].dye > 0)
		{
			inv[slot] = DyeSwap(inv[slot], out success, out var targetSlot);
			if (success)
			{
				Main.EquipPageSelected = 0;
				AchievementsHelper.HandleOnEquip(player, inv[slot], 12);
				DisplayTransfer_TwoWay(inv, context, slot, player.dye, 12, targetSlot);
			}
		}
		else if (Main.projHook[inv[slot].shoot])
		{
			inv[slot] = EquipSwap(inv[slot], player.miscEquips, 4, out success);
			if (success)
			{
				Main.EquipPageSelected = 2;
				AchievementsHelper.HandleOnEquip(player, player.miscEquips[4], 16);
				DisplayTransfer_TwoWay(inv, context, slot, player.miscEquips, 16, 4);
			}
		}
		else if (inv[slot].mountType != -1 && !MountID.Sets.Cart[inv[slot].mountType])
		{
			inv[slot] = EquipSwap(inv[slot], player.miscEquips, 3, out success);
			if (success)
			{
				Main.EquipPageSelected = 2;
				AchievementsHelper.HandleOnEquip(player, inv[slot], 17);
				DisplayTransfer_TwoWay(inv, context, slot, player.miscEquips, 17, 3);
			}
		}
		else if (inv[slot].mountType != -1 && MountID.Sets.Cart[inv[slot].mountType])
		{
			inv[slot] = EquipSwap(inv[slot], player.miscEquips, 2, out success);
			if (success)
			{
				Main.EquipPageSelected = 2;
				DisplayTransfer_TwoWay(inv, context, slot, player.miscEquips, 18, 2);
			}
		}
		else if (inv[slot].buffType > 0 && Main.lightPet[inv[slot].buffType])
		{
			inv[slot] = EquipSwap(inv[slot], player.miscEquips, 1, out success);
			if (success)
			{
				Main.EquipPageSelected = 2;
				DisplayTransfer_TwoWay(inv, context, slot, player.miscEquips, 20, 1);
			}
		}
		else if (inv[slot].buffType > 0 && Main.vanityPet[inv[slot].buffType])
		{
			inv[slot] = EquipSwap(inv[slot], player.miscEquips, 0, out success);
			if (success)
			{
				Main.EquipPageSelected = 2;
				DisplayTransfer_TwoWay(inv, context, slot, player.miscEquips, 19, 0);
			}
		}
		else
		{
			int num;
			if (inv[slot].accessory)
			{
				num = (inv[slot].vanity ? 11 : 10);
			}
			else
			{
				num = (inv[slot].vanity ? 9 : 8);
			}
			inv[slot] = ArmorSwap(inv[slot], out success, out var targetSlot2);
			if (success)
			{
				Main.EquipPageSelected = 0;
				Item item = player.armor[targetSlot2];
				AchievementsHelper.HandleOnEquip(player, item, num);
				DisplayTransfer_TwoWay(inv, context, slot, player.armor, num, targetSlot2);
			}
		}
		if (context == 3 && Main.netMode == 1)
		{
			NetMessage.SendData(32, -1, -1, null, player.chest, slot);
		}
	}

	public static void DisplayTransfer_GetItem(Item[] arrayFrom, int fromContext, int fromSlot)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		Vector2 pointPositionFrom = UILinkPointNavigator.GetPosition(GetGamepadPointForSlot(arrayFrom, fromContext, fromSlot));
		foreach (PlayerGetItemLogger.GetItemLoggerEntry entry in Player.GetItemLogger.Entries)
		{
			Vector2 pointPositionTo = UILinkPointNavigator.GetPosition(GetGamepadPointForSlot(entry.TargetArray, entry.TargetItemSlotContext, entry.TargetSlot));
			int timeToAnimate = GetTimeToAnimate(pointPositionFrom, pointPositionTo);
			Item item = entry.TargetArray[entry.TargetSlot];
			bool flag = TryDisplayTransfer(ref pointPositionFrom, ref pointPositionTo, item, entry.Stack, timeToAnimate);
			bool flag2 = item.stack == entry.Stack;
			if (flag & flag2)
			{
				AddCooldown(entry.TargetItemSlotContext, entry.TargetSlot, timeToAnimate);
			}
		}
	}

	private static int GetTimeToAnimate(Vector2 startPosition, Vector2 endPosition)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		int num = (int)Vector2.Distance(startPosition, endPosition) / 20;
		if (num > 15)
		{
			num = 15;
		}
		return 15 + num;
	}

	public static void DisplayTransfer_OneWay(Item[] arrayFrom, int fromContext, int fromSlot, Item[] arrayTo, int toContext, int toSlot, int stackSize = 1)
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		int gamepadPointForSlot = GetGamepadPointForSlot(arrayFrom, fromContext, fromSlot);
		int gamepadPointForSlot2 = GetGamepadPointForSlot(arrayTo, toContext, toSlot);
		Vector2 pointPositionFrom = UILinkPointNavigator.GetPosition(gamepadPointForSlot);
		Vector2 pointPositionTo = UILinkPointNavigator.GetPosition(gamepadPointForSlot2);
		int timeToAnimate = GetTimeToAnimate(pointPositionFrom, pointPositionTo);
		bool flag = TryDisplayTransfer(ref pointPositionFrom, ref pointPositionTo, arrayTo[toSlot], stackSize, timeToAnimate);
		bool flag2 = arrayTo[toSlot].stack == stackSize;
		if (flag & flag2)
		{
			AddCooldown(toContext, toSlot, timeToAnimate);
		}
	}

	public static void DisplayTransfer_TwoWay(Item[] arrayFrom, int fromContext, int fromSlot, Item toItem, int toContext)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		int gamepadPointForSlot = GetGamepadPointForSlot(arrayFrom, fromContext, fromSlot);
		int gamepadPointForSlot2 = GetGamepadPointForSlot(_dirtyHack, toContext, 0);
		Vector2 pointPositionFrom = UILinkPointNavigator.GetPosition(gamepadPointForSlot);
		Vector2 pointPositionTo = UILinkPointNavigator.GetPosition(gamepadPointForSlot2);
		int timeToAnimate = GetTimeToAnimate(pointPositionFrom, pointPositionTo);
		if (TryDisplayTransfer(ref pointPositionFrom, ref pointPositionTo, toItem, toItem.stack, timeToAnimate))
		{
			AddCooldown(toContext, 0, timeToAnimate);
		}
		if (TryDisplayTransfer(ref pointPositionTo, ref pointPositionFrom, arrayFrom[fromSlot], arrayFrom[fromSlot].stack, timeToAnimate))
		{
			AddCooldown(fromContext, fromSlot, timeToAnimate);
		}
	}

	public static void DisplayTransfer_TwoWay(Item[] arrayFrom, int fromContext, int fromSlot, Item[] arrayTo, int toContext, int toSlot)
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		int gamepadPointForSlot = GetGamepadPointForSlot(arrayFrom, fromContext, fromSlot);
		int gamepadPointForSlot2 = GetGamepadPointForSlot(arrayTo, toContext, toSlot);
		Vector2 pointPositionFrom = UILinkPointNavigator.GetPosition(gamepadPointForSlot);
		Vector2 pointPositionTo = UILinkPointNavigator.GetPosition(gamepadPointForSlot2);
		int timeToAnimate = GetTimeToAnimate(pointPositionFrom, pointPositionTo);
		if (TryDisplayTransfer(ref pointPositionFrom, ref pointPositionTo, arrayTo[toSlot], arrayTo[toSlot].stack, timeToAnimate))
		{
			AddCooldown(toContext, toSlot, timeToAnimate);
		}
		if (TryDisplayTransfer(ref pointPositionTo, ref pointPositionFrom, arrayFrom[fromSlot], arrayFrom[fromSlot].stack, timeToAnimate))
		{
			AddCooldown(fromContext, fromSlot, timeToAnimate);
		}
	}

	private static void AddCooldown(int context, int slot, int time)
	{
		_nextTickDrawAvailable[new ItemDisplayKey
		{
			Slot = slot,
			Context = context
		}] = Main.EverLastingTicker + (ulong)time;
	}

	private static bool TryDisplayTransfer(ref Vector2 pointPositionFrom, ref Vector2 pointPositionTo, Item toItem, int stackSize, int animationTime)
	{
		return false;
	}

	public static bool CanEquipBothAccessories(Item acc1, Item acc2, bool vanity)
	{
		if (acc1.type == acc2.type)
		{
			return false;
		}
		if (acc1.wingSlot > 0 && acc2.wingSlot > 0)
		{
			if (vanity && (acc1.type == 3580 || acc2.type == 3580))
			{
				return true;
			}
			return false;
		}
		int num = ItemID.Sets.AccessoryIncompatibilityType[acc1.type];
		int num2 = ItemID.Sets.AccessoryIncompatibilityType[acc2.type];
		if (num != -1 && num == num2)
		{
			return false;
		}
		return true;
	}

	public static bool HasIncompatibleAccessory(Item newAcc, bool vanity, ArraySegment<Item> accessories, out int collisionSlot)
	{
		for (int i = 0; i < accessories.Count; i++)
		{
			if (!CanEquipBothAccessories(accessories.Array[i + accessories.Offset], newAcc, vanity))
			{
				collisionSlot = i + accessories.Offset;
				return true;
			}
		}
		collisionSlot = -1;
		return false;
	}

	public static ArraySegment<Item> ArmorSlots(Player player)
	{
		return new ArraySegment<Item>(player.armor, 0, 3);
	}

	public static ArraySegment<Item> AccSlots(Player player)
	{
		return new ArraySegment<Item>(player.armor, 3, 7);
	}

	public static ArraySegment<Item> VanityArmorSlots(Player player)
	{
		return new ArraySegment<Item>(player.armor, 10, 3);
	}

	public static ArraySegment<Item> VanityAccSlots(Player player)
	{
		return new ArraySegment<Item>(player.armor, 13, 7);
	}

	public static bool HasSameItemInSlot(Item newItem, ArraySegment<Item> items)
	{
		if (newItem.IsAir)
		{
			return false;
		}
		int num = ItemID.Sets.AccessoryIncompatibilityType[newItem.type];
		for (int i = 0; i < items.Count; i++)
		{
			Item item = items.Array[i + items.Offset];
			if (item.type == newItem.type)
			{
				return true;
			}
			int num2 = ItemID.Sets.AccessoryIncompatibilityType[item.type];
			if (num != -1 && num == num2)
			{
				return true;
			}
		}
		return false;
	}

	public static bool CanEquipAccessoryInSlot(Item newAcc, bool vanity, ArraySegment<Item> accessories, int slot)
	{
		if (HasIncompatibleAccessory(newAcc, vanity, accessories, out var collisionSlot))
		{
			return slot == collisionSlot;
		}
		return true;
	}

	private static Item DyeSwap(Item item, out bool success, out int targetSlot)
	{
		targetSlot = -1;
		success = false;
		if (item.dye <= 0)
		{
			return item;
		}
		Player player = Main.player[Main.myPlayer];
		while (!player.IsItemSlotUnlockedAndUsable(dyeSwapCounter))
		{
			dyeSwapCounter = (dyeSwapCounter + 1) % 10;
		}
		for (int i = 0; i < 10; i++)
		{
			if (player.IsItemSlotUnlockedAndUsable(i) && player.dye[i].IsAir)
			{
				dyeSwapCounter = i;
				break;
			}
		}
		SoundEngine.PlaySound(7);
		SwapClearFavorite(ref item, ref player.dye[dyeSwapCounter]);
		targetSlot = dyeSwapCounter;
		dyeSwapCounter = (dyeSwapCounter + 1) % 10;
		success = true;
		return item;
	}

	private static Item ArmorSwap(Item item, out bool success, out int targetSlot)
	{
		targetSlot = -1;
		success = false;
		if (item.stack < 1)
		{
			return item;
		}
		if (item.headSlot == -1 && item.bodySlot == -1 && item.legSlot == -1 && !item.accessory)
		{
			return item;
		}
		Player player = Main.player[Main.myPlayer];
		if (item.headSlot != -1 || item.bodySlot != -1 || item.legSlot != -1)
		{
			if (HasSameItemInSlot(item, ArmorSlots(player)) || HasSameItemInSlot(item, VanityArmorSlots(player)))
			{
				return item;
			}
		}
		else if (item.accessory && HasSameItemInSlot(item, item.vanity ? AccSlots(player) : VanityAccSlots(player)))
		{
			return item;
		}
		int num = (item.vanity ? 10 : 0);
		if (item.headSlot != -1)
		{
			targetSlot = num;
		}
		else if (item.bodySlot != -1)
		{
			targetSlot = num + 1;
		}
		else if (item.legSlot != -1)
		{
			targetSlot = num + 2;
		}
		else if (item.accessory)
		{
			ArraySegment<Item> accessories = new ArraySegment<Item>(player.armor, 3 + num, 7);
			if (HasIncompatibleAccessory(item, item.vanity, accessories, out targetSlot))
			{
				if (!player.IsItemSlotUnlockedAndUsable(targetSlot))
				{
					return item;
				}
			}
			else
			{
				targetSlot = accessories.Offset;
				for (int i = 0; i < accessories.Count; i++)
				{
					int num2 = i + accessories.Offset;
					if (player.IsItemSlotUnlockedAndUsable(num2) && accessories.Array[num2].IsAir)
					{
						targetSlot = num2;
						break;
					}
				}
			}
		}
		if (targetSlot == -1)
		{
			return item;
		}
		SoundEngine.PlaySound(7);
		SwapClearFavorite(ref item, ref player.armor[targetSlot]);
		success = true;
		return item;
	}

	private static Item EquipSwap(Item item, Item[] inv, int slot, out bool success)
	{
		success = false;
		_ = Main.player[Main.myPlayer];
		item.favorited = false;
		Item result = inv[slot].Clone();
		inv[slot] = item.Clone();
		SoundEngine.PlaySound(7);
		success = true;
		return result;
	}

	public static void DrawMoney(SpriteBatch sb, string text, float shopx, float shopy, int[] coinsArray, bool horizontal = false, bool fromSavings = false)
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0118: Unknown result type (might be due to invalid IL or missing references)
		//IL_013a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0146: Unknown result type (might be due to invalid IL or missing references)
		//IL_0163: Unknown result type (might be due to invalid IL or missing references)
		//IL_0168: Unknown result type (might be due to invalid IL or missing references)
		//IL_0182: Unknown result type (might be due to invalid IL or missing references)
		//IL_0216: Unknown result type (might be due to invalid IL or missing references)
		//IL_0230: Unknown result type (might be due to invalid IL or missing references)
		//IL_027c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0281: Unknown result type (might be due to invalid IL or missing references)
		//IL_029b: Unknown result type (might be due to invalid IL or missing references)
		Utils.DrawBorderStringFourWay(sb, FontAssets.MouseText.Value, text, shopx, shopy + 40f, Color.White * ((float)(int)Main.mouseTextColor / 255f), Color.Black, Vector2.Zero);
		CoinSlot.CoinDrawState drawState = new CoinSlot.CoinDrawState
		{
			coinAnimFrame = 0,
			coinYOffset = 0f,
			stackTextScale = 1f
		};
		if (horizontal)
		{
			for (int i = 0; i < 4; i++)
			{
				Main.instance.LoadItem(74 - i);
				if (i == 0)
				{
					_ = coinsArray[3 - i];
					_ = 99;
				}
				int num = coinsArray[3 - i];
				if (num > 999)
				{
					num = 999;
				}
				if (fromSavings)
				{
					CoinSlot.UpdateSavings(i, num, out drawState);
				}
				Vector2 val = new Vector2(shopx + ChatManager.GetStringSize(FontAssets.MouseText.Value, text, Vector2.One).X + (float)(24 * i) + 45f, shopy + 50f);
				CoinSlot.DrawItemCoin(sb, val + new Vector2(0f, 0f - drawState.coinYOffset), 74 - i, drawState.coinAnimFrame, 1f, 1024f, Color.White);
				Utils.DrawBorderStringFourWay(sb, FontAssets.ItemStack.Value, num.ToString(), val.X - 11f, val.Y + (float)FontAssets.ItemStack.Value.LineSpacing * 0.75f, Color.White, Color.Black, new Vector2(0f, (float)FontAssets.ItemStack.Value.LineSpacing), 0.75f * drawState.stackTextScale);
			}
			return;
		}
		for (int j = 0; j < 4; j++)
		{
			Main.instance.LoadItem(74 - j);
			int num2 = ((j == 0 && coinsArray[3 - j] > 99) ? (-6) : 0);
			int num3 = coinsArray[3 - j];
			if (num3 > 999)
			{
				num3 = 999;
			}
			if (fromSavings)
			{
				CoinSlot.UpdateSavings(j, num3, out drawState);
			}
			CoinSlot.DrawItemCoin(sb, new Vector2(shopx + 11f + (float)(24 * j), shopy + 75f - drawState.coinYOffset), 74 - j, drawState.coinAnimFrame, 1f, 1024f, Color.White);
			Utils.DrawBorderStringFourWay(sb, FontAssets.ItemStack.Value, num3.ToString(), shopx + (float)(24 * j) + (float)num2, shopy + 75f + (float)FontAssets.ItemStack.Value.LineSpacing * 0.75f, Color.White, Color.Black, new Vector2(0f, (float)FontAssets.ItemStack.Value.LineSpacing), 0.75f * drawState.stackTextScale);
		}
	}

	public static void DrawSavings(SpriteBatch sb, float shopx, float shopy, bool horizontal = false)
	{
		//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0104: Unknown result type (might be due to invalid IL or missing references)
		//IL_0106: Unknown result type (might be due to invalid IL or missing references)
		//IL_0110: Unknown result type (might be due to invalid IL or missing references)
		//IL_0115: Unknown result type (might be due to invalid IL or missing references)
		//IL_0124: Unknown result type (might be due to invalid IL or missing references)
		//IL_0145: Unknown result type (might be due to invalid IL or missing references)
		//IL_014a: Unknown result type (might be due to invalid IL or missing references)
		//IL_014c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0156: Unknown result type (might be due to invalid IL or missing references)
		//IL_015b: Unknown result type (might be due to invalid IL or missing references)
		//IL_016a: Unknown result type (might be due to invalid IL or missing references)
		//IL_018a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0191: Unknown result type (might be due to invalid IL or missing references)
		//IL_019b: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01af: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f4: Unknown result type (might be due to invalid IL or missing references)
		Player player = Main.player[Main.myPlayer];
		if (_customCurrencyForSavings != -1)
		{
			CustomCurrencyManager.DrawSavings(sb, _customCurrencyForSavings, shopx, shopy, horizontal);
			return;
		}
		long num = Utils.CoinsCount(out var overFlowing, player.bank.item);
		long num2 = Utils.CoinsCount(out overFlowing, player.bank2.item);
		long num3 = Utils.CoinsCount(out overFlowing, player.bank3.item);
		long num4 = Utils.CoinsCount(out overFlowing, player.bank4.item);
		long num5 = Utils.CoinsCombineStacks(out overFlowing, num, num2, num3, num4);
		if (num5 > 0)
		{
			Main.GetItemDrawFrame(4076, out var itemTexture, out var itemFrame);
			Main.GetItemDrawFrame(3813, out var itemTexture2, out var itemFrame2);
			Main.GetItemDrawFrame(346, out var itemTexture3, out var _);
			Main.GetItemDrawFrame(87, out var itemTexture4, out var _);
			if (num4 > 0)
			{
				sb.Draw(itemTexture, Utils.CenteredRectangle(new Vector2(shopx + 70f, shopy + 45f), itemFrame.Size() * 0.65f), (Rectangle?)null, Color.White);
			}
			if (num3 > 0)
			{
				sb.Draw(itemTexture2, Utils.CenteredRectangle(new Vector2(shopx + 92f, shopy + 45f), itemFrame2.Size() * 0.65f), (Rectangle?)null, Color.White);
			}
			if (num2 > 0)
			{
				sb.Draw(itemTexture3, Utils.CenteredRectangle(new Vector2(shopx + 80f, shopy + 50f), itemTexture3.Size() * 0.65f), (Rectangle?)null, Color.White);
			}
			if (num > 0)
			{
				sb.Draw(itemTexture4, Utils.CenteredRectangle(new Vector2(shopx + 70f, shopy + 60f), itemTexture4.Size() * 0.65f), (Rectangle?)null, Color.White);
			}
			DrawMoney(sb, Lang.inter[66].Value, shopx, shopy, Utils.CoinsSplit(num5), horizontal, fromSavings: true);
		}
	}

	public static void GetItemLight(ref Color currentColor, Item item, bool outInTheWorld = false, float lightScalar = 1f)
	{
		float scale = 1f;
		GetItemLight(ref currentColor, ref scale, item, outInTheWorld, lightScalar);
	}

	public static void GetItemLight(ref Color currentColor, int type, bool outInTheWorld = false, float lightScalar = 1f)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		float scale = 1f;
		GetItemLight(ref currentColor, ref scale, type, outInTheWorld, lightScalar);
	}

	public static void GetItemLight(ref Color currentColor, ref float scale, Item item, bool outInTheWorld = false, float lightScalar = 1f)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		GetItemLight(ref currentColor, ref scale, item.type, outInTheWorld, lightScalar);
	}

	public static Color GetItemLight(ref Color currentColor, ref float scale, int type, bool outInTheWorld = false, float lightScalar = 1f)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0122: Unknown result type (might be due to invalid IL or missing references)
		//IL_0129: Unknown result type (might be due to invalid IL or missing references)
		//IL_012e: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_01af: Unknown result type (might be due to invalid IL or missing references)
		if (type < 0 || type > ItemID.Count)
		{
			return currentColor;
		}
		if (type == 662 || type == 663 || type == 5444 || type == 5450 || type == 5643)
		{
			currentColor.R = (byte)Main.DiscoR;
			currentColor.G = (byte)Main.DiscoG;
			currentColor.B = (byte)Main.DiscoB;
			currentColor.A = byte.MaxValue;
			currentColor *= lightScalar;
		}
		if (type == 5128)
		{
			currentColor.R = (byte)Main.DiscoR;
			currentColor.G = (byte)Main.DiscoG;
			currentColor.B = (byte)Main.DiscoB;
			currentColor.A = byte.MaxValue;
			currentColor *= lightScalar;
		}
		else if (ItemID.Sets.ItemIconPulse[type])
		{
			scale = Main.essScale;
			currentColor.R = (byte)((float)(int)currentColor.R * scale);
			currentColor.G = (byte)((float)(int)currentColor.G * scale);
			currentColor.B = (byte)((float)(int)currentColor.B * scale);
			currentColor.A = (byte)((float)(int)currentColor.A * scale);
			currentColor *= lightScalar;
		}
		else if (type == 58 || type == 184 || type == 4143)
		{
			scale = Main.essScale * 0.25f + 0.75f;
			currentColor.R = (byte)((float)(int)currentColor.R * scale);
			currentColor.G = (byte)((float)(int)currentColor.G * scale);
			currentColor.B = (byte)((float)(int)currentColor.B * scale);
			currentColor.A = (byte)((float)(int)currentColor.A * scale);
			currentColor *= lightScalar;
		}
		return currentColor;
	}

	public static void DrawRadialCircular(SpriteBatch sb, Vector2 position, Player.SelectionRadial radial, Item[] items)
	{
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		//IL_010b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0111: Unknown result type (might be due to invalid IL or missing references)
		//IL_0116: Unknown result type (might be due to invalid IL or missing references)
		//IL_0132: Unknown result type (might be due to invalid IL or missing references)
		//IL_0133: Unknown result type (might be due to invalid IL or missing references)
		//IL_0135: Unknown result type (might be due to invalid IL or missing references)
		//IL_0144: Unknown result type (might be due to invalid IL or missing references)
		//IL_0148: Unknown result type (might be due to invalid IL or missing references)
		//IL_0153: Unknown result type (might be due to invalid IL or missing references)
		//IL_015d: Unknown result type (might be due to invalid IL or missing references)
		//IL_018a: Unknown result type (might be due to invalid IL or missing references)
		//IL_018b: Unknown result type (might be due to invalid IL or missing references)
		//IL_018d: Unknown result type (might be due to invalid IL or missing references)
		//IL_019c: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a6: Unknown result type (might be due to invalid IL or missing references)
		CircularRadialOpacity = MathHelper.Clamp(CircularRadialOpacity + ((PlayerInput.UsingGamepad && PlayerInput.Triggers.Current.RadialHotbar) ? 0.25f : (-0.15f)), 0f, 1f);
		if (CircularRadialOpacity == 0f)
		{
			return;
		}
		Texture2D value = TextureAssets.HotbarRadial[2].Value;
		float num = CircularRadialOpacity * 0.9f;
		float num2 = CircularRadialOpacity * 1f;
		float num3 = (float)(int)Main.mouseTextColor / 255f;
		float num4 = 1f - (1f - num3) * (1f - num3);
		num4 *= 0.785f;
		Color val = Color.White * num4 * num;
		value = TextureAssets.HotbarRadial[1].Value;
		float num5 = (float)Math.PI * 2f / (float)radial.RadialCount;
		float num6 = -(float)Math.PI / 2f;
		for (int i = 0; i < radial.RadialCount; i++)
		{
			int num7 = radial.Bindings[i];
			Vector2 val2 = new Vector2(150f, 0f).RotatedBy(num6 + num5 * (float)i) * num2;
			float num8 = 0.85f;
			if (radial.SelectedBinding == i)
			{
				num8 = 1.7f;
			}
			sb.Draw(value, position + val2, (Rectangle?)null, val * num8, 0f, value.Size() / 2f, num2 * num8, (SpriteEffects)0, 0f);
			if (num7 != -1)
			{
				float inventoryScale = Main.inventoryScale;
				Main.inventoryScale = num2 * num8;
				Draw(sb, items, 14, num7, position + val2 + new Vector2(-26f * num2 * num8), Color.White);
				Main.inventoryScale = inventoryScale;
			}
		}
	}

	public static void DrawRadialQuicks(SpriteBatch sb, Vector2 position)
	{
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0198: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01af: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01de: Unknown result type (might be due to invalid IL or missing references)
		//IL_01df: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0209: Unknown result type (might be due to invalid IL or missing references)
		//IL_0238: Unknown result type (might be due to invalid IL or missing references)
		//IL_0239: Unknown result type (might be due to invalid IL or missing references)
		//IL_023b: Unknown result type (might be due to invalid IL or missing references)
		//IL_024b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0250: Unknown result type (might be due to invalid IL or missing references)
		//IL_0255: Unknown result type (might be due to invalid IL or missing references)
		//IL_0266: Unknown result type (might be due to invalid IL or missing references)
		//IL_0267: Unknown result type (might be due to invalid IL or missing references)
		//IL_0269: Unknown result type (might be due to invalid IL or missing references)
		//IL_0278: Unknown result type (might be due to invalid IL or missing references)
		//IL_0282: Unknown result type (might be due to invalid IL or missing references)
		//IL_0289: Unknown result type (might be due to invalid IL or missing references)
		//IL_0290: Unknown result type (might be due to invalid IL or missing references)
		//IL_0295: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bd: Unknown result type (might be due to invalid IL or missing references)
		QuicksRadialOpacity = MathHelper.Clamp(QuicksRadialOpacity + ((PlayerInput.UsingGamepad && PlayerInput.Triggers.Current.RadialQuickbar) ? 0.25f : (-0.15f)), 0f, 1f);
		if (QuicksRadialOpacity == 0f)
		{
			return;
		}
		Player player = Main.player[Main.myPlayer];
		Texture2D value = TextureAssets.HotbarRadial[2].Value;
		Texture2D value2 = TextureAssets.QuicksIcon.Value;
		float num = QuicksRadialOpacity * 0.9f;
		float num2 = QuicksRadialOpacity * 1f;
		float num3 = (float)(int)Main.mouseTextColor / 255f;
		float num4 = 1f - (1f - num3) * (1f - num3);
		num4 *= 0.785f;
		Color val = Color.White * num4 * num;
		float num5 = (float)Math.PI * 2f / (float)player.QuicksRadial.RadialCount;
		float num6 = -(float)Math.PI / 2f;
		Item item = player.QuickHeal_GetItemToUse();
		Item item2 = player.QuickMana_GetItemToUse();
		Item item3 = null;
		Item item4 = null;
		if (item == null)
		{
			item = new Item();
			item.SetDefaults(28);
		}
		if (item2 == null)
		{
			item2 = new Item();
			item2.SetDefaults(110);
		}
		if (item3 == null)
		{
			item3 = new Item();
			item3.SetDefaults(292);
		}
		if (item4 == null)
		{
			item4 = new Item();
			item4.SetDefaults(2428);
		}
		for (int i = 0; i < player.QuicksRadial.RadialCount; i++)
		{
			Item inv = item4;
			if (i == 1)
			{
				inv = item;
			}
			if (i == 2)
			{
				inv = item3;
			}
			if (i == 3)
			{
				inv = item2;
			}
			_ = player.QuicksRadial.Bindings[i];
			Vector2 val2 = new Vector2(120f, 0f).RotatedBy(num6 + num5 * (float)i) * num2;
			float num7 = 0.85f;
			if (player.QuicksRadial.SelectedBinding == i)
			{
				num7 = 1.7f;
			}
			sb.Draw(value, position + val2, (Rectangle?)null, val * num7, 0f, value.Size() / 2f, num2 * num7 * 1.3f, (SpriteEffects)0, 0f);
			float inventoryScale = Main.inventoryScale;
			Main.inventoryScale = num2 * num7;
			Draw(sb, ref inv, 14, position + val2 + new Vector2(-26f * num2 * num7), Color.White);
			Main.inventoryScale = inventoryScale;
			sb.Draw(value2, position + val2 + new Vector2(34f, 20f) * 0.85f * num2 * num7, (Rectangle?)null, val * num7, 0f, value.Size() / 2f, num2 * num7 * 1.3f, (SpriteEffects)0, 0f);
		}
	}

	public static void DrawRadialDpad(SpriteBatch sb, Vector2 position)
	{
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_010b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0110: Unknown result type (might be due to invalid IL or missing references)
		//IL_0115: Unknown result type (might be due to invalid IL or missing references)
		if (!PlayerInput.UsingGamepad || !PlayerInput.CurrentProfile.UsingDpadHotbar())
		{
			return;
		}
		Player player = Main.player[Main.myPlayer];
		if (player.chest != -1)
		{
			return;
		}
		Texture2D value = TextureAssets.HotbarRadial[0].Value;
		float num = (float)(int)Main.mouseTextColor / 255f;
		float num2 = 1f - (1f - num) * (1f - num);
		num2 *= 0.785f;
		Color val = Color.White * num2;
		sb.Draw(value, position, (Rectangle?)null, val, 0f, value.Size() / 2f, Main.inventoryScale, (SpriteEffects)0, 0f);
		for (int i = 0; i < 4; i++)
		{
			int num3 = player.DpadRadial.Bindings[i];
			if (num3 != -1)
			{
				Draw(sb, player.inventory, 14, num3, position + new Vector2((float)(value.Width / 3), 0f).RotatedBy(-(float)Math.PI / 2f + (float)Math.PI / 2f * (float)i) + new Vector2(-26f * Main.inventoryScale), Color.White);
			}
		}
	}

	public static string GetGamepadInstructions(int context = 0)
	{
		return GetGamepadInstructions(singleSlotArray, context);
	}

	public static string GetGamepadInstructions(ref Item inv, int context = 0)
	{
		singleSlotArray[0] = inv;
		string gamepadInstructions = GetGamepadInstructions(singleSlotArray, context);
		inv = singleSlotArray[0];
		return gamepadInstructions;
	}

	public static bool CanExecuteCommand()
	{
		return PlayerInput.AllowExecutionOfGamepadInstructions;
	}

	public static string GetGamepadInstructions(Item[] inv, int context = 0, int slot = 0)
	{
		Player player = Main.player[Main.myPlayer];
		string s = "";
		if (inv == null || inv[slot] == null || Main.mouseItem == null)
		{
			return s;
		}
		if (context == 0 || context == 1 || context == 2)
		{
			if (inv[slot].type > 0 && inv[slot].stack > 0)
			{
				if (Main.mouseItem.type > 0)
				{
					s += PlayerInput.BuildCommand(Lang.misc[65].Value, PlayerInput.ProfileGamepadUI.KeyStatus["MouseLeft"]);
					if (inv[slot].type == Main.mouseItem.type && Main.mouseItem.stack < inv[slot].maxStack && inv[slot].maxStack > 1)
					{
						s += PlayerInput.BuildCommand(Lang.misc[55].Value, PlayerInput.ProfileGamepadUI.KeyStatus["MouseRight"]);
					}
				}
				else
				{
					if (context == 0 && player.chest == -1 && PlayerInput.AllowExecutionOfGamepadInstructions)
					{
						player.DpadRadial.ChangeBinding(slot);
					}
					s += PlayerInput.BuildCommand(Lang.misc[54].Value, PlayerInput.ProfileGamepadUI.KeyStatus["MouseLeft"]);
					if (inv[slot].maxStack > 1)
					{
						s += PlayerInput.BuildCommand(Lang.misc[55].Value, PlayerInput.ProfileGamepadUI.KeyStatus["MouseRight"]);
					}
				}
				if (ItemID.Sets.OpenableBag[inv[slot].type])
				{
					s += PlayerInput.BuildCommand(Language.GetTextValue("UI.ActionOpen"), PlayerInput.ProfileGamepadUI.KeyStatus["Grapple"]);
					if (CanExecuteCommand() && PlayerInput.Triggers.JustPressed.Grapple)
					{
						TryOpenContainer(inv, context, slot, player);
						PlayerInput.LockGamepadButtons("Grapple");
						PlayerInput.SettingsForUI.TryRevertingToMouseMode();
					}
				}
				else if (ItemID.Sets.HasItemSwap[inv[slot].type])
				{
					s += PlayerInput.BuildCommand(Language.GetTextValue("UI.ActionChangeType"), PlayerInput.ProfileGamepadUI.KeyStatus["Grapple"]);
					if (CanExecuteCommand() && PlayerInput.Triggers.JustPressed.Grapple)
					{
						TryItemSwap(inv[slot]);
						PlayerInput.LockGamepadButtons("Grapple");
						PlayerInput.SettingsForUI.TryRevertingToMouseMode();
					}
				}
				else if (inv[slot].stack == 1 && inv[slot].CanBeEquipped())
				{
					s += PlayerInput.BuildCommand(Lang.misc[67].Value, PlayerInput.ProfileGamepadUI.KeyStatus["Grapple"]);
					if (CanExecuteCommand() && PlayerInput.Triggers.JustPressed.Grapple)
					{
						SwapEquip(inv, context, slot);
						PlayerInput.LockGamepadButtons("Grapple");
						PlayerInput.SettingsForUI.TryRevertingToMouseMode();
					}
				}
				s += PlayerInput.BuildCommand(Lang.misc[83].Value, PlayerInput.ProfileGamepadUI.KeyStatus["SmartCursor"]);
				if (CanExecuteCommand() && PlayerInput.Triggers.JustPressed.SmartCursor)
				{
					ToggleFavorited(inv[slot]);
					PlayerInput.LockGamepadButtons("SmartCursor");
					PlayerInput.SettingsForUI.TryRevertingToMouseMode();
				}
			}
			else if (Main.mouseItem.type > 0)
			{
				s += PlayerInput.BuildCommand(Lang.misc[65].Value, PlayerInput.ProfileGamepadUI.KeyStatus["MouseLeft"]);
			}
		}
		if (context == 3 || context == 4 || context == 32)
		{
			if (inv[slot].type > 0 && inv[slot].stack > 0)
			{
				if (Main.mouseItem.type > 0)
				{
					s += PlayerInput.BuildCommand(Lang.misc[65].Value, PlayerInput.ProfileGamepadUI.KeyStatus["MouseLeft"]);
					if (inv[slot].type == Main.mouseItem.type && Main.mouseItem.stack < inv[slot].maxStack && inv[slot].maxStack > 1)
					{
						s += PlayerInput.BuildCommand(Lang.misc[55].Value, PlayerInput.ProfileGamepadUI.KeyStatus["MouseRight"]);
					}
				}
				else
				{
					s += PlayerInput.BuildCommand(Lang.misc[54].Value, PlayerInput.ProfileGamepadUI.KeyStatus["MouseLeft"]);
					if (inv[slot].maxStack > 1)
					{
						s += PlayerInput.BuildCommand(Lang.misc[55].Value, PlayerInput.ProfileGamepadUI.KeyStatus["MouseRight"]);
					}
				}
				if (inv[slot].stack == 1 && inv[slot].CanBeEquipped())
				{
					s += PlayerInput.BuildCommand(Lang.misc[67].Value, PlayerInput.ProfileGamepadUI.KeyStatus["Grapple"]);
					if (CanExecuteCommand() && PlayerInput.Triggers.JustPressed.Grapple)
					{
						SwapEquip(inv, context, slot);
						PlayerInput.LockGamepadButtons("Grapple");
						PlayerInput.SettingsForUI.TryRevertingToMouseMode();
					}
				}
				if (context == 32)
				{
					s += PlayerInput.BuildCommand(Lang.misc[83].Value, PlayerInput.ProfileGamepadUI.KeyStatus["SmartCursor"]);
					if (CanExecuteCommand() && PlayerInput.Triggers.JustPressed.SmartCursor)
					{
						ToggleFavorited(inv[slot]);
						PlayerInput.LockGamepadButtons("SmartCursor");
						PlayerInput.SettingsForUI.TryRevertingToMouseMode();
					}
				}
			}
			else if (Main.mouseItem.type > 0)
			{
				s += PlayerInput.BuildCommand(Lang.misc[65].Value, PlayerInput.ProfileGamepadUI.KeyStatus["MouseLeft"]);
			}
		}
		if (context == 15)
		{
			if (inv[slot].type > 0 && inv[slot].stack > 0)
			{
				if (Main.mouseItem.type > 0)
				{
					if (inv[slot].type == Main.mouseItem.type && Main.mouseItem.stack < inv[slot].maxStack && inv[slot].maxStack > 1)
					{
						s += PlayerInput.BuildCommand(Lang.misc[91].Value, PlayerInput.ProfileGamepadUI.KeyStatus["MouseRight"]);
					}
				}
				else
				{
					s += PlayerInput.BuildCommand(Lang.misc[90].Value, PlayerInput.ProfileGamepadUI.KeyStatus["MouseLeft"], PlayerInput.ProfileGamepadUI.KeyStatus["MouseRight"]);
				}
			}
			else if (Main.mouseItem.type > 0)
			{
				s += PlayerInput.BuildCommand(Lang.misc[92].Value, PlayerInput.ProfileGamepadUI.KeyStatus["MouseLeft"]);
			}
		}
		if (context == 8 || context == 9 || context == 10 || context == 11 || context == 16 || context == 17 || context == 19 || context == 20)
		{
			if (inv[slot].type > 0 && inv[slot].stack > 0)
			{
				if (Main.mouseItem.type > 0)
				{
					if (Main.mouseItem.stack == 1 && Main.mouseItem.CanBeEquipped())
					{
						s += PlayerInput.BuildCommand(Lang.misc[65].Value, PlayerInput.ProfileGamepadUI.KeyStatus["MouseLeft"]);
					}
				}
				else
				{
					s += PlayerInput.BuildCommand(Lang.misc[54].Value, PlayerInput.ProfileGamepadUI.KeyStatus["MouseLeft"]);
				}
				if (context == 10)
				{
					bool flag = player.hideVisibleAccessory[slot];
					s += PlayerInput.BuildCommand(Lang.misc[flag ? 77 : 78].Value, PlayerInput.ProfileGamepadUI.KeyStatus["Grapple"]);
					if (CanExecuteCommand() && PlayerInput.Triggers.JustPressed.Grapple)
					{
						player.hideVisibleAccessory[slot] = !player.hideVisibleAccessory[slot];
						SoundEngine.PlaySound(12);
						if (Main.netMode == 1)
						{
							NetMessage.SendData(4, -1, -1, null, Main.myPlayer);
						}
						PlayerInput.LockGamepadButtons("Grapple");
						PlayerInput.SettingsForUI.TryRevertingToMouseMode();
					}
				}
				if ((context == 16 || context == 17 || context == 18 || context == 19 || context == 20) && slot < 2)
				{
					bool flag2 = player.hideMisc[slot];
					s += PlayerInput.BuildCommand(Lang.misc[flag2 ? 77 : 78].Value, PlayerInput.ProfileGamepadUI.KeyStatus["Grapple"]);
					if (CanExecuteCommand() && PlayerInput.Triggers.JustPressed.Grapple)
					{
						if (slot == 0)
						{
							player.TogglePet();
						}
						if (slot == 1)
						{
							player.ToggleLight();
						}
						SoundEngine.PlaySound(12);
						if (Main.netMode == 1)
						{
							NetMessage.SendData(4, -1, -1, null, Main.myPlayer);
						}
						PlayerInput.LockGamepadButtons("Grapple");
						PlayerInput.SettingsForUI.TryRevertingToMouseMode();
					}
				}
				if (canLoadoutShareAt[context])
				{
					s += PlayerInput.BuildCommand(Language.GetTextValue("UI.LoadoutShare"), PlayerInput.ProfileGamepadUI.KeyStatus["SmartCursor"]);
					if (CanExecuteCommand() && PlayerInput.Triggers.JustPressed.SmartCursor)
					{
						ToggleLoadoutShare(inv, context, slot);
						PlayerInput.LockGamepadButtons("SmartCursor");
						PlayerInput.SettingsForUI.TryRevertingToMouseMode();
					}
				}
			}
			else
			{
				if (Main.mouseItem.type > 0 && Main.mouseItem.CanBeEquipped())
				{
					s += PlayerInput.BuildCommand(Lang.misc[65].Value, PlayerInput.ProfileGamepadUI.KeyStatus["MouseLeft"]);
				}
				if (context == 10)
				{
					bool flag3 = player.hideVisibleAccessory[slot];
					s += PlayerInput.BuildCommand(Lang.misc[flag3 ? 77 : 78].Value, PlayerInput.ProfileGamepadUI.KeyStatus["Grapple"]);
					if (CanExecuteCommand() && PlayerInput.Triggers.JustPressed.Grapple)
					{
						player.hideVisibleAccessory[slot] = !player.hideVisibleAccessory[slot];
						SoundEngine.PlaySound(12);
						if (Main.netMode == 1)
						{
							NetMessage.SendData(4, -1, -1, null, Main.myPlayer);
						}
						PlayerInput.LockGamepadButtons("Grapple");
						PlayerInput.SettingsForUI.TryRevertingToMouseMode();
					}
				}
				if ((context == 16 || context == 17 || context == 18 || context == 19 || context == 20) && slot < 2)
				{
					bool flag4 = player.hideMisc[slot];
					s += PlayerInput.BuildCommand(Lang.misc[flag4 ? 77 : 78].Value, PlayerInput.ProfileGamepadUI.KeyStatus["Grapple"]);
					if (CanExecuteCommand() && PlayerInput.Triggers.JustPressed.Grapple)
					{
						if (slot == 0)
						{
							player.TogglePet();
						}
						if (slot == 1)
						{
							player.ToggleLight();
						}
						Main.mouseLeftRelease = false;
						SoundEngine.PlaySound(12);
						if (Main.netMode == 1)
						{
							NetMessage.SendData(4, -1, -1, null, Main.myPlayer);
						}
						PlayerInput.LockGamepadButtons("Grapple");
						PlayerInput.SettingsForUI.TryRevertingToMouseMode();
					}
				}
			}
		}
		switch (context)
		{
		case 12:
		case 25:
		case 27:
		case 33:
			if (inv[slot].type > 0 && inv[slot].stack > 0)
			{
				if (Main.mouseItem.type > 0)
				{
					if (Main.mouseItem.dye > 0)
					{
						s += PlayerInput.BuildCommand(Lang.misc[65].Value, PlayerInput.ProfileGamepadUI.KeyStatus["MouseLeft"]);
					}
				}
				else
				{
					s += PlayerInput.BuildCommand(Lang.misc[54].Value, PlayerInput.ProfileGamepadUI.KeyStatus["MouseLeft"]);
				}
				if (context == 12 || context == 25 || context == 27 || context == 33)
				{
					int num = -1;
					if (inv == player.dye)
					{
						num = slot;
					}
					if (inv == player.miscDyes)
					{
						num = 10 + slot;
					}
					if (num != -1)
					{
						if (num < 10)
						{
							bool flag6 = player.hideVisibleAccessory[slot];
							s += PlayerInput.BuildCommand(Lang.misc[flag6 ? 77 : 78].Value, PlayerInput.ProfileGamepadUI.KeyStatus["Grapple"]);
							if (CanExecuteCommand() && PlayerInput.Triggers.JustPressed.Grapple)
							{
								player.hideVisibleAccessory[slot] = !player.hideVisibleAccessory[slot];
								SoundEngine.PlaySound(12);
								if (Main.netMode == 1)
								{
									NetMessage.SendData(4, -1, -1, null, Main.myPlayer);
								}
								PlayerInput.LockGamepadButtons("Grapple");
								PlayerInput.SettingsForUI.TryRevertingToMouseMode();
							}
						}
						else
						{
							bool flag7 = player.hideMisc[slot];
							s += PlayerInput.BuildCommand(Lang.misc[flag7 ? 77 : 78].Value, PlayerInput.ProfileGamepadUI.KeyStatus["Grapple"]);
							if (CanExecuteCommand() && PlayerInput.Triggers.JustPressed.Grapple)
							{
								player.hideMisc[slot] = !player.hideMisc[slot];
								SoundEngine.PlaySound(12);
								if (Main.netMode == 1)
								{
									NetMessage.SendData(4, -1, -1, null, Main.myPlayer);
								}
								PlayerInput.LockGamepadButtons("Grapple");
								PlayerInput.SettingsForUI.TryRevertingToMouseMode();
							}
						}
					}
				}
			}
			else if (Main.mouseItem.type > 0 && Main.mouseItem.dye > 0)
			{
				s += PlayerInput.BuildCommand(Lang.misc[65].Value, PlayerInput.ProfileGamepadUI.KeyStatus["MouseLeft"]);
			}
			return s;
		case 18:
		{
			if (inv[slot].type > 0 && inv[slot].stack > 0)
			{
				if (Main.mouseItem.type > 0)
				{
					if (Main.mouseItem.dye > 0)
					{
						s += PlayerInput.BuildCommand(Lang.misc[65].Value, PlayerInput.ProfileGamepadUI.KeyStatus["MouseLeft"]);
					}
				}
				else
				{
					s += PlayerInput.BuildCommand(Lang.misc[54].Value, PlayerInput.ProfileGamepadUI.KeyStatus["MouseLeft"]);
				}
			}
			else if (Main.mouseItem.type > 0 && Main.mouseItem.dye > 0)
			{
				s += PlayerInput.BuildCommand(Lang.misc[65].Value, PlayerInput.ProfileGamepadUI.KeyStatus["MouseLeft"]);
			}
			bool enabledSuperCart = player.enabledSuperCart;
			s += PlayerInput.BuildCommand(Language.GetTextValue((!enabledSuperCart) ? "UI.EnableSuperCart" : "UI.DisableSuperCart"), PlayerInput.ProfileGamepadUI.KeyStatus["Grapple"]);
			if (CanExecuteCommand() && PlayerInput.Triggers.JustPressed.Grapple)
			{
				player.enabledSuperCart = !player.enabledSuperCart;
				SoundEngine.PlaySound(12);
				if (Main.netMode == 1)
				{
					NetMessage.SendData(4, -1, -1, null, Main.myPlayer);
				}
				PlayerInput.LockGamepadButtons("Grapple");
				PlayerInput.SettingsForUI.TryRevertingToMouseMode();
			}
			return s;
		}
		case 6:
			if (inv[slot].type > 0 && inv[slot].stack > 0)
			{
				s = ((Main.mouseItem.type <= 0) ? (s + PlayerInput.BuildCommand(Lang.misc[54].Value, PlayerInput.ProfileGamepadUI.KeyStatus["MouseLeft"])) : (s + PlayerInput.BuildCommand(Lang.misc[74].Value, PlayerInput.ProfileGamepadUI.KeyStatus["MouseLeft"])));
			}
			else if (Main.mouseItem.type > 0)
			{
				s += PlayerInput.BuildCommand(Lang.misc[74].Value, PlayerInput.ProfileGamepadUI.KeyStatus["MouseLeft"]);
			}
			return s;
		case 5:
		case 7:
		{
			bool flag5 = false;
			if (context == 5)
			{
				flag5 = Main.mouseItem.CanHavePrefixes() || Main.mouseItem.type == 0;
			}
			if (context == 7)
			{
				flag5 = Main.mouseItem.material;
			}
			if (inv[slot].type > 0 && inv[slot].stack > 0)
			{
				if (Main.mouseItem.type > 0)
				{
					if (flag5)
					{
						s += PlayerInput.BuildCommand(Lang.misc[65].Value, PlayerInput.ProfileGamepadUI.KeyStatus["MouseLeft"]);
					}
				}
				else
				{
					s += PlayerInput.BuildCommand(Lang.misc[54].Value, PlayerInput.ProfileGamepadUI.KeyStatus["MouseLeft"]);
				}
			}
			else if ((Main.mouseItem.type > 0) & flag5)
			{
				s += PlayerInput.BuildCommand(Lang.misc[65].Value, PlayerInput.ProfileGamepadUI.KeyStatus["MouseLeft"]);
			}
			break;
		}
		}
		_ = 41;
		if (!Main.mouseItem.IsAir)
		{
			if (canQuickDropAt[context])
			{
				s += PlayerInput.BuildCommand(Lang.inter[121].Value, PlayerInput.ProfileGamepadUI.KeyStatus["SmartSelect"]);
				if (CanExecuteCommand() && PlayerInput.Triggers.JustPressed.SmartSelect)
				{
					player.DropSelectedItem();
					PlayerInput.LockGamepadButtons("SmartSelect");
					PlayerInput.SettingsForUI.TryRevertingToMouseMode();
				}
			}
			else if (player.ItemSpace(Main.mouseItem).CanTakeItemToPersonalInventory)
			{
				s += PlayerInput.BuildCommand(Lang.misc[76].Value, PlayerInput.ProfileGamepadUI.KeyStatus["SmartSelect"]);
				if (CanExecuteCommand() && PlayerInput.Triggers.JustPressed.SmartSelect)
				{
					Main.mouseItem = player.GetItem(Main.mouseItem, GetItemSettings.ReturnItemShowAsNew);
					PlayerInput.LockGamepadButtons("SmartSelect");
					PlayerInput.SettingsForUI.TryRevertingToMouseMode();
				}
			}
		}
		else
		{
			switch (context)
			{
			case 22:
			case 41:
			case 42:
			case 43:
			{
				bool flag8 = Player.Settings.CraftingGridControl == Player.Settings.CraftingGridMode.Classic;
				s += PlayerInput.BuildCommand(Language.GetTextValue(CraftingUI.CraftingWindowTextKey), PlayerInput.ProfileGamepadUI.KeyStatus["SmartCursor"]);
				if (CanExecuteCommand() && PlayerInput.Triggers.JustPressed.SmartCursor)
				{
					if (flag8)
					{
						NewCraftingUI.Close(quiet: true, returnToInventory: true);
						if (UILinkPointNavigator.CurrentPage == 10)
						{
							UILinkPointNavigator.ChangePage(9);
						}
						else
						{
							UILinkPointNavigator.ChangePage(10);
						}
					}
					else
					{
						NewCraftingUI.ToggleInInventory();
					}
				}
				if (!Main.InGuideCraftMenu && Main.bannerUI.AnyAvailableBanners && context == 22)
				{
					s += PlayerInput.BuildCommand(Language.GetTextValue("UI.CyclePipsToBanners"), PlayerInput.ProfileGamepadUI.KeyStatus["SmartSelect"]);
					if (CanExecuteCommand() && PlayerInput.Triggers.JustPressed.SmartSelect)
					{
						Main.TryChangePipsPage(Main.PipPage.Banners);
						UILinkPointNavigator.ChangePage(22);
						PlayerInput.LockGamepadButtons("SmartSelect");
					}
				}
				break;
			}
			case 35:
				s += PlayerInput.BuildCommand(Language.GetTextValue("GameUI.BannersWindow"), PlayerInput.ProfileGamepadUI.KeyStatus["SmartCursor"]);
				if (CanExecuteCommand() && PlayerInput.Triggers.JustPressed.SmartCursor)
				{
					if (UILinkPointNavigator.CurrentPage == 23)
					{
						UILinkPointNavigator.ChangePage(22);
					}
					else
					{
						UILinkPointNavigator.ChangePage(23);
					}
				}
				s += PlayerInput.BuildCommand(Language.GetTextValue("UI.CyclePipsToCrafting"), PlayerInput.ProfileGamepadUI.KeyStatus["SmartSelect"]);
				if (CanExecuteCommand() && PlayerInput.Triggers.JustPressed.SmartSelect)
				{
					Main.TryChangePipsPage(Main.PipPage.Recipes);
					UILinkPointNavigator.ChangePage(9);
					PlayerInput.LockGamepadButtons("SmartSelect");
				}
				break;
			default:
			{
				ShiftForcedOn = true;
				AlternateClickAction? alternateClickAction = GetAlternateClickAction(inv, context, slot);
				if (alternateClickAction.HasValue)
				{
					s += PlayerInput.BuildCommand(alternateClickAction.Value.gamepadHintText.Value, PlayerInput.ProfileGamepadUI.KeyStatus["SmartSelect"]);
					if (CanDoSimulatedClickAction() && CanExecuteCommand() && PlayerInput.Triggers.JustPressed.SmartSelect)
					{
						bool mouseLeft = Main.mouseLeft;
						int cursorOverride = Main.cursorOverride;
						Main.mouseLeft = true;
						Main.cursorOverride = alternateClickAction.Value.cursorOverride;
						LeftClick(inv, context, slot);
						Main.cursorOverride = cursorOverride;
						Main.mouseLeft = mouseLeft;
						PlayerInput.LockGamepadButtons("SmartSelect");
						PlayerInput.SettingsForUI.TryRevertingToMouseMode();
					}
				}
				ShiftForcedOn = false;
				break;
			}
			}
		}
		if (!TryEnteringFastUseMode(inv, context, slot, player, ref s))
		{
			TryEnteringBuildingMode(inv, context, slot, player, ref s);
		}
		return s;
	}

	private static bool CanDoSimulatedClickAction()
	{
		if (PlayerInput.SteamDeckIsUsed)
		{
			return UILinkPointNavigator.InUse;
		}
		return true;
	}

	private static bool TryEnteringFastUseMode(Item[] inv, int context, int slot, Player player, ref string s)
	{
		int num = 0;
		if (Main.mouseItem.CanBeQuickUsed)
		{
			num = 1;
		}
		if (num == 0 && Main.mouseItem.stack <= 0 && context == 0 && inv[slot].CanBeQuickUsed)
		{
			num = 2;
		}
		if (num > 0)
		{
			s += PlayerInput.BuildCommand(Language.GetTextValue("UI.QuickUseItem"), PlayerInput.ProfileGamepadUI.KeyStatus["QuickMount"]);
			if (CanExecuteCommand() && PlayerInput.Triggers.JustPressed.QuickMount)
			{
				switch (num)
				{
				case 1:
					PlayerInput.TryEnteringFastUseModeForMouseItem();
					break;
				case 2:
					PlayerInput.TryEnteringFastUseModeForInventorySlot(slot);
					break;
				}
			}
			return true;
		}
		return false;
	}

	private static bool TryEnteringBuildingMode(Item[] inv, int context, int slot, Player player, ref string s)
	{
		int num = 0;
		if (IsABuildingItem(Main.mouseItem))
		{
			num = 1;
		}
		if (num == 0 && Main.mouseItem.stack <= 0 && context == 0 && IsABuildingItem(inv[slot]))
		{
			num = 2;
		}
		if (num > 0)
		{
			Item item = Main.mouseItem;
			if (num == 1)
			{
				item = Main.mouseItem;
			}
			if (num == 2)
			{
				item = inv[slot];
			}
			if (item.damage > 0 && item.ammo == 0)
			{
				s += PlayerInput.BuildCommand(Lang.misc[60].Value, PlayerInput.ProfileGamepadUI.KeyStatus["QuickMount"]);
			}
			else if (item.createTile >= 0 || item.createWall > 0)
			{
				s += PlayerInput.BuildCommand(Lang.misc[61].Value, PlayerInput.ProfileGamepadUI.KeyStatus["QuickMount"]);
			}
			else
			{
				s += PlayerInput.BuildCommand(Lang.misc[63].Value, PlayerInput.ProfileGamepadUI.KeyStatus["QuickMount"]);
			}
			if (CanExecuteCommand() && PlayerInput.Triggers.JustPressed.QuickMount)
			{
				PlayerInput.EnterBuildingMode();
			}
			return true;
		}
		return false;
	}

	public static string GetQuickCraftGamepadInstructions(Recipe recipe)
	{
		Player localPlayer = Main.LocalPlayer;
		if (!Main.mouseItem.IsAir || !localPlayer.ItemSpace(recipe.createItem).CanTakeItemToPersonalInventory || localPlayer.HasLockedInventory())
		{
			return null;
		}
		if (CanExecuteCommand() && PlayerInput.Triggers.Current.Grapple && (Main.stackSplit <= 1 || PlayerInput.Triggers.JustPressed.Grapple))
		{
			if (CanExecuteCommand() && PlayerInput.Triggers.JustPressed.Grapple)
			{
				UILinksInitializer.SomeVarsForUILinkers.SequencedCraftingCurrent = Main.recipe[Main.availableRecipe[Main.focusRecipe]];
			}
			RefreshStackSplitCooldown();
			Main.quickCraftStackSplit = true;
			if (UILinksInitializer.SomeVarsForUILinkers.SequencedCraftingCurrent == Main.recipe[Main.availableRecipe[Main.focusRecipe]])
			{
				CraftingRequests.CraftItem(recipe, 1, quickCraft: true);
			}
		}
		return PlayerInput.BuildCommand(Lang.misc[71].Value, PlayerInput.ProfileGamepadUI.KeyStatus["Grapple"]);
	}

	public static string GetCraftSlotGamepadInstructions()
	{
		if (Main.InGuideCraftMenu)
		{
			return "";
		}
		string text = "";
		Player localPlayer = Main.LocalPlayer;
		Recipe recipe = Main.recipe[Main.availableRecipe[Main.focusRecipe]];
		string quickCraftGamepadInstructions = GetQuickCraftGamepadInstructions(recipe);
		if (quickCraftGamepadInstructions == null && Main.mouseItem.stack == 1 && Main.mouseItem.CanBeEquipped())
		{
			text += PlayerInput.BuildCommand(Lang.misc[67].Value, PlayerInput.ProfileGamepadUI.KeyStatus["Grapple"]);
			if (CanExecuteCommand() && PlayerInput.Triggers.JustPressed.Grapple)
			{
				SwapEquip(ref Main.mouseItem);
				if (Main.player[Main.myPlayer].ItemSpace(Main.mouseItem).CanTakeItemToPersonalInventory)
				{
					Main.mouseItem = localPlayer.GetItem(Main.mouseItem, GetItemSettings.QuickTransferFromSlot);
				}
				PlayerInput.LockGamepadButtons("Grapple");
				PlayerInput.SettingsForUI.TryRevertingToMouseMode();
			}
		}
		if (Main.mouseItem.IsAir || (Main.mouseItem.CanHavePrefixes() && Item.CanStack(Main.mouseItem, recipe.createItem) && Main.mouseItem.stack + recipe.createItem.stack <= Main.mouseItem.maxStack))
		{
			text += PlayerInput.BuildCommand(Lang.misc[72].Value, PlayerInput.ProfileGamepadUI.KeyStatus["MouseLeft"], PlayerInput.ProfileGamepadUI.KeyStatus["MouseRight"]);
		}
		if (quickCraftGamepadInstructions != null)
		{
			text += quickCraftGamepadInstructions;
		}
		return text;
	}

	public static bool IsABuildingItem(Item item)
	{
		if (item.type > 0 && item.stack > 0 && item.useStyle != 0)
		{
			return item.useTime > 0;
		}
		return false;
	}

	public static void SelectEquipPage(Item item)
	{
		Main.EquipPage = -1;
		if (!item.IsAir)
		{
			if (Main.projHook[item.shoot])
			{
				Main.EquipPage = 2;
			}
			else if (item.mountType != -1)
			{
				Main.EquipPage = 2;
			}
			else if (item.buffType > 0 && Main.vanityPet[item.buffType])
			{
				Main.EquipPage = 2;
			}
			else if (item.buffType > 0 && Main.lightPet[item.buffType])
			{
				Main.EquipPage = 2;
			}
			else if (item.dye > 0 && Main.EquipPageSelected == 1)
			{
				Main.EquipPage = 0;
			}
			else if (item.legSlot != -1 || item.headSlot != -1 || item.bodySlot != -1 || item.accessory)
			{
				Main.EquipPage = 0;
			}
		}
	}
}
