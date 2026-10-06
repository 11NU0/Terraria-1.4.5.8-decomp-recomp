using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.Testing;
using Terraria.UI;

namespace Terraria.GameContent;

public class QuickStacking
{
	private class DestinationHelper
	{
		private PositionedChest _chest;

		public Item[] items;

		public int itemCount;

		public bool locked;

		public bool transferBlocked;

		private int[] freeSlots = new int[200];

		private int freeSlotStart;

		private int freeSlotCount;

		private int[] categoryScores = new int[ItemSorting.LayerCount];

		public Vector2 position
		{
			get
			{
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				return _chest.position;
			}
		}

		public int ChestIndex
		{
			get
			{
				Invariant.Assert(!_chest.chest.bankChest, "PositionedChest is a bank");
				return _chest.chest.index;
			}
		}

		public bool IsEmpty => freeSlotCount == itemCount;

		public bool HasFreeSlots => freeSlotStart < freeSlotCount;

		public void Reset(PositionedChest inventory)
		{
			_chest = inventory;
			items = inventory.chest.item;
			itemCount = inventory.chest.maxItems;
			locked = inventory.chest.IsLockedOrInUse();
			transferBlocked = false;
			if (freeSlots.Length < itemCount)
			{
				Array.Resize(ref freeSlots, itemCount);
			}
			Array.Clear(freeSlots, 0, freeSlots.Length);
			freeSlotStart = 0;
			freeSlotCount = 0;
			Array.Clear(categoryScores, 0, categoryScores.Length);
		}

		public void AddCategoryScore(int category)
		{
			categoryScores[category]++;
		}

		public void AddFreeSlot(int i)
		{
			AddToListyArray(ref freeSlots, ref freeSlotCount, i);
		}

		public bool TryGetCategoryScore(int category, out int score)
		{
			score = categoryScores[category];
			if (score == 0)
			{
				return false;
			}
			return true;
		}

		public int ConsumeFreeSlot()
		{
			return freeSlots[freeSlotStart++];
		}

		public void SyncSlot(int slot)
		{
			if (_chest.chest.index >= 0)
			{
				NetMessage.SendData(32, -1, -1, null, _chest.chest.index, slot);
			}
		}
	}

	private class MatchingItemTypeDestinationList
	{
		private struct LinkedEntry
		{
			public DestinationHelper value;

			public int next;
		}

		private LinkedEntry[] entries = new LinkedEntry[1000];

		private int numEntries;

		private int[] firstEntryForType = new int[ItemID.Count];

		public MatchingItemTypeDestinationList()
		{
			Reset();
		}

		public void Reset()
		{
			Array.Clear(firstEntryForType, 0, firstEntryForType.Length);
			Array.Clear(entries, 0, entries.Length);
			numEntries = 1;
		}

		private int Tail(int type)
		{
			int num = firstEntryForType[type];
			if (num == 0)
			{
				return 0;
			}
			while (entries[num].next != 0)
			{
				num = entries[num].next;
			}
			return num;
		}

		internal void Add(int type, DestinationHelper value)
		{
			int num = Tail(type);
			if (num == 0)
			{
				firstEntryForType[type] = AddEntry(value);
			}
			else if (entries[num].value != value)
			{
				entries[num].next = AddEntry(value);
			}
		}

		private int AddEntry(DestinationHelper value)
		{
			if (numEntries == entries.Length)
			{
				Array.Resize(ref entries, entries.Length * 2);
			}
			int result = numEntries;
			AddToListyArray(ref entries, ref numEntries, new LinkedEntry
			{
				value = value
			});
			return result;
		}

		public bool Lookup(int type, out DestinationHelper value)
		{
			value = null;
			int num = firstEntryForType[type];
			while (num > 0)
			{
				LinkedEntry linkedEntry = entries[num];
				value = linkedEntry.value;
				if (value.HasFreeSlots)
				{
					return true;
				}
				num = linkedEntry.next;
				firstEntryForType[type] = num;
			}
			return false;
		}
	}

	internal struct SourceInventory
	{
		public Item[] items;

		public int numItems;

		public PlayerItemSlotID.SlotReference[] slots;

		public bool[] transferBlocked;

		public Vector2 position;
	}

	private static SourceInventory netInv;

	private static Item[] inventoryItemsScratch = new Item[400];

	private static PlayerItemSlotID.SlotReference[] slotsScratch = new PlayerItemSlotID.SlotReference[400];

	private static bool[] blockedSlotsScratch = new bool[400];

	private static List<DestinationHelper> destHelperListScratch = new List<DestinationHelper>();

	private static MatchingItemTypeDestinationList matchingItemTypeScratch = new MatchingItemTypeDestinationList();

	private static List<int> _blockedChests = new List<int>();

	private static DestinationHelper[] destHelperPool = new DestinationHelper[100];

	private static int nextDestHelper = 0;

	private static void AddToListyArray<T>(ref T[] arr, ref int count, T elem)
	{
		if (count == arr.Length)
		{
			Array.Resize(ref arr, arr.Length * 2);
		}
		arr[count++] = elem;
	}

	private static int GetCategory(int type)
	{
		return ItemSorting.GetSortingLayerIndex(type);
	}

	public static void QuickStackToNearbyInventories(Player player, bool smartStack = false)
	{
		QuickStackToNearbyBanks(player);
		QuickStackToNearbyChests(player, smartStack);
	}

	private static void QuickStackToNearbyBanks(Player player)
	{
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		List<PositionedChest> banksInRangeOf = NearbyChests.GetBanksInRangeOf(player);
		foreach (PositionedChest item in banksInRangeOf)
		{
			long coinsMoved = ChestUI.MoveCoins(player.inventory, item.chest);
			Chest.VisualizeChestTransfer_CoinsBatch(player.Center, item.position, coinsMoved, Chest.ItemTransferVisualizationSettings.PlayerToChest);
		}
		SourceInventory sourceInventory = PackQuickStackableItems(player, includeVoidBag: false);
		Transfer(sourceInventory, banksInRangeOf, out var blockedChests);
		Invariant.Assert(!blockedChests.Any(), "blockedChests");
		RestoreToPlayer(player, sourceInventory);
	}

	public static void QuickStackToNearbyChests(Player player, bool smartStack = false)
	{
		QuickStackToNearbyChests(player, PackQuickStackableItems(player, includeVoidBag: true), smartStack);
	}

	internal static void QuickStackToNearbyChests(Player player, SourceInventory inventory, bool smartStack)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		if (Main.netMode == 1)
		{
			SendQuickStackToNearbyChests(player, inventory, smartStack);
			return;
		}
		List<PositionedChest> chestsInRangeOf = NearbyChests.GetChestsInRangeOf(player.position);
		Transfer(inventory, chestsInRangeOf, out var blockedChests, smartStack);
		IndicateBlockedChests(player, blockedChests);
		RestoreToPlayer(player, inventory);
	}

	internal static void IndicateBlockedChests(Player player, List<int> chests)
	{
		if (!chests.Any())
		{
			return;
		}
		if (Main.netMode == 2)
		{
			Invariant.Assert(chests == _blockedChests, "chests == _blockedChests");
			NetMessage.SendData(85, player.whoAmI, -1, null, player.whoAmI);
			return;
		}
		Invariant.Assert(player == Main.LocalPlayer, "IndicateBlockedChests for remote client");
		foreach (int chest in chests)
		{
			Chest.IndicateBlockedChest(chest);
		}
	}

	private static void SendQuickStackToNearbyChests(Player player, SourceInventory inventory, bool smartStack)
	{
		netInv = inventory;
		for (int i = 0; i < inventory.numItems; i++)
		{
			int slotId = inventory.slots[i].SlotId;
			player.LockNetSlot(slotId);
			NetMessage.SendData(5, -1, -1, null, player.whoAmI, slotId);
		}
		NetMessage.SendData(85, -1, -1, null, smartStack ? 1 : 0);
	}

	internal static void WriteNetInventorySlots(BinaryWriter writer)
	{
		writer.Write(netInv.numItems);
		for (int i = 0; i < netInv.numItems; i++)
		{
			writer.Write((short)netInv.slots[i].SlotId);
		}
	}

	internal static SourceInventory ReadNetInventory(Player player, BinaryReader reader)
	{
		SourceInventory scratchInventory = GetScratchInventory(player);
		Array.Clear(scratchInventory.transferBlocked, 0, scratchInventory.transferBlocked.Length);
		scratchInventory.numItems = reader.ReadInt32();
		for (int i = 0; i < scratchInventory.numItems; i++)
		{
			PlayerItemSlotID.SlotReference slotReference = new PlayerItemSlotID.SlotReference(player, reader.ReadInt16());
			scratchInventory.slots[i] = slotReference;
			Item item = slotReference.Item;
			scratchInventory.items[i] = item;
		}
		return scratchInventory;
	}

	internal static void WriteBlockedChestList(BinaryWriter writer)
	{
		writer.Write(_blockedChests.Count);
		for (int i = 0; i < _blockedChests.Count; i++)
		{
			writer.Write((ushort)_blockedChests[i]);
		}
	}

	internal static List<int> ReadBlockedChestList(BinaryReader reader)
	{
		_blockedChests.Clear();
		int num = reader.ReadInt32();
		for (int i = 0; i < num; i++)
		{
			_blockedChests.Add(reader.ReadUInt16());
		}
		return _blockedChests;
	}

	private static void RestoreToPlayer(Player player, SourceInventory inventory)
	{
		for (int i = 0; i < inventory.numItems; i++)
		{
			Item item = inventory.items[i];
			PlayerItemSlotID.SlotReference slot = inventory.slots[i];
			bool flag = inventory.transferBlocked[i];
			if (0 == 0)
			{
				slot.Item = item;
			}
			if (Main.netMode == 2)
			{
				NetMessage.SendData(5, -1, -1, null, player.whoAmI, slot.SlotId, flag ? 1 : 0);
			}
			else if (flag)
			{
				ItemSlot.IndicateBlockedSlot(slot);
			}
		}
	}

	private static SourceInventory GetScratchInventory(Player player)
	{
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		return new SourceInventory
		{
			items = inventoryItemsScratch,
			numItems = 0,
			slots = slotsScratch,
			transferBlocked = blockedSlotsScratch,
			position = player.Center
		};
	}

	private static SourceInventory PackQuickStackableItems(Player player, bool includeVoidBag)
	{
		SourceInventory inventory = GetScratchInventory(player);
		Array.Clear(inventory.transferBlocked, 0, inventory.transferBlocked.Length);
		AddQuickStackableItems(player, ref inventory, PlayerItemSlotID.Inventory0 + 10, 40);
		if (player.useVoidBag() & includeVoidBag)
		{
			AddQuickStackableItems(player, ref inventory, PlayerItemSlotID.Bank4_0, player.bank4.maxItems);
		}
		return inventory;
	}

	private static void AddQuickStackableItems(Player player, ref SourceInventory inventory, int startSlot, int count)
	{
		for (int i = 0; i < count; i++)
		{
			PlayerItemSlotID.SlotReference slotReference = new PlayerItemSlotID.SlotReference(player, startSlot + i);
			Item item = slotReference.Item;
			if (!item.IsAir && !item.favorited && !item.IsACoin)
			{
				int num = inventory.numItems++;
				inventory.slots[num] = slotReference;
				inventory.items[num] = item;
			}
		}
	}

	private static void Transfer(SourceInventory source, List<PositionedChest> destinations, out List<int> blockedChests, bool smartStack = false)
	{
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0146: Unknown result type (might be due to invalid IL or missing references)
		nextDestHelper = 0;
		List<DestinationHelper> list = destHelperListScratch;
		list.Clear();
		MatchingItemTypeDestinationList matchingItemTypeDestinationList = matchingItemTypeScratch;
		matchingItemTypeDestinationList.Reset();
		foreach (PositionedChest destination in destinations)
		{
			if (!destination.chest.IsEmpty())
			{
				DestinationHelper destHelperFromPool = GetDestHelperFromPool();
				destHelperFromPool.Reset(destination);
				list.Add(destHelperFromPool);
				BuildDestinationMetricsAndStackItems(source, destHelperFromPool, matchingItemTypeDestinationList);
			}
		}
		for (int i = 0; i < source.numItems; i++)
		{
			Item item = source.items[i];
			if (!item.IsAir && matchingItemTypeDestinationList.Lookup(item.type, out var value))
			{
				Consolidate(source, i);
				InsertIntoFreeSlot(ref source.items[i], value, source.position);
			}
		}
		if (smartStack)
		{
			for (int j = 0; j < source.numItems; j++)
			{
				Item item2 = source.items[j];
				if (!item2.IsAir && !source.transferBlocked[j] && TryGetBestDestinationForCategory(GetCategory(item2.type), list, out var dest))
				{
					if (dest.locked)
					{
						source.transferBlocked[j] = true;
						dest.transferBlocked = true;
					}
					else
					{
						Consolidate(source, j);
						InsertIntoFreeSlot(ref source.items[j], dest, source.position);
					}
				}
			}
		}
		blockedChests = _blockedChests;
		blockedChests.Clear();
		foreach (DestinationHelper item3 in list)
		{
			if (item3.transferBlocked)
			{
				blockedChests.Add(item3.ChestIndex);
			}
		}
	}

	private static DestinationHelper GetDestHelperFromPool()
	{
		if (nextDestHelper == destHelperPool.Length)
		{
			Array.Resize(ref destHelperPool, destHelperPool.Length * 2);
		}
		if (destHelperPool[nextDestHelper] == null)
		{
			destHelperPool[nextDestHelper] = new DestinationHelper();
		}
		return destHelperPool[nextDestHelper++];
	}

	private static void BuildDestinationMetricsAndStackItems(SourceInventory source, DestinationHelper dest, MatchingItemTypeDestinationList destinationsForItemTypes)
	{
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		for (int i = 0; i < dest.itemCount; i++)
		{
			Item item = dest.items[i];
			if (item.IsAir)
			{
				dest.AddFreeSlot(i);
				continue;
			}
			dest.AddCategoryScore(GetCategory(item.type));
			for (int j = 0; j < source.numItems; j++)
			{
				Item item2 = source.items[j];
				if (item2.type != item.type)
				{
					continue;
				}
				if (dest.locked)
				{
					source.transferBlocked[j] = true;
					dest.transferBlocked = true;
					continue;
				}
				if (Item.CanStack(item2, item) && item.stack < item.maxStack)
				{
					FillStack(item2, dest, i, source.position);
					if (item2.IsAir)
					{
						continue;
					}
				}
				destinationsForItemTypes.Add(item.type, dest);
			}
		}
	}

	private static bool TryGetBestDestinationForCategory(int category, List<DestinationHelper> destinations, out DestinationHelper dest)
	{
		dest = null;
		int num = int.MinValue;
		foreach (DestinationHelper destination in destinations)
		{
			if (destination.HasFreeSlots && destination.TryGetCategoryScore(category, out var score) && score > num)
			{
				dest = destination;
				num = score;
			}
		}
		return dest != null;
	}

	private static void FillStack(Item item, DestinationHelper dest, int slotIndex, Vector2 srcPosition)
	{
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		Item item2 = dest.items[slotIndex];
		int num = Math.Min(item2.maxStack - item2.stack, item.stack);
		Invariant.Assert(Item.CanStack(item, item2) && num > 0 && item2.stack + num <= item2.maxStack && num <= item.stack, "FillStack allowed");
		Chest.VisualizeChestTransfer(srcPosition, dest.position, item.type, Chest.ItemTransferVisualizationSettings.PlayerToChest);
		item2.stack += num;
		item.stack -= num;
		if (item.stack == 0)
		{
			item.TurnToAir();
		}
		dest.SyncSlot(slotIndex);
	}

	private static void Consolidate(SourceInventory source, int i)
	{
		Item item = source.items[i++];
		while (i < source.numItems)
		{
			Item item2 = source.items[i++];
			if (Item.CanStack(item, item2))
			{
				int num = Math.Min(item.maxStack - item.stack, item2.stack);
				item.stack += num;
				item2.stack -= num;
				if (item2.stack == 0)
				{
					item2.TurnToAir();
				}
			}
		}
	}

	private static void InsertIntoFreeSlot(ref Item item, DestinationHelper dest, Vector2 srcPosition)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		Chest.VisualizeChestTransfer(srcPosition, dest.position, item.type, Chest.ItemTransferVisualizationSettings.PlayerToChest);
		int num = dest.ConsumeFreeSlot();
		Utils.Swap(ref item, ref dest.items[num]);
		dest.SyncSlot(num);
	}
}
