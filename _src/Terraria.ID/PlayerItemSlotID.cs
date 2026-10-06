using System;

namespace Terraria.ID;

public static class PlayerItemSlotID
{
	public struct SlotReference(Player player, int slot)
	{
		public readonly Player Player = player;

		public readonly int SlotId = slot;

		public Item Item
		{
			get
			{
				if (SlotId == TrashItem)
				{
					return Player.trashItem;
				}
				if (!TryGetArraySlot(out var arr, out var slot))
				{
					throw new IndexOutOfRangeException("SlotId: " + SlotId);
				}
				return arr[slot];
			}
			set
			{
				if (SlotId == TrashItem)
				{
					Player.trashItem = value;
					return;
				}
				if (!TryGetArraySlot(out var arr, out var slot))
				{
					throw new IndexOutOfRangeException("SlotId: " + SlotId);
				}
				arr[slot] = value;
			}
		}

		private bool TryGetArraySlot(out Item[] arr, out int slot)
		{
			if (SlotId >= Loadout3_Dye_0)
			{
				slot = SlotId - Loadout3_Dye_0;
				arr = Player.Loadouts[2].Dye;
			}
			else if (SlotId >= Loadout3_Armor_0)
			{
				slot = SlotId - Loadout3_Armor_0;
				arr = Player.Loadouts[2].Armor;
			}
			else if (SlotId >= Loadout2_Dye_0)
			{
				slot = SlotId - Loadout2_Dye_0;
				arr = Player.Loadouts[1].Dye;
			}
			else if (SlotId >= Loadout2_Armor_0)
			{
				slot = SlotId - Loadout2_Armor_0;
				arr = Player.Loadouts[1].Armor;
			}
			else if (SlotId >= Loadout1_Dye_0)
			{
				slot = SlotId - Loadout1_Dye_0;
				arr = Player.Loadouts[0].Dye;
			}
			else if (SlotId >= Loadout1_Armor_0)
			{
				slot = SlotId - Loadout1_Armor_0;
				arr = Player.Loadouts[0].Armor;
			}
			else if (SlotId >= Bank4_0)
			{
				slot = SlotId - Bank4_0;
				arr = Player.bank4.item;
			}
			else if (SlotId >= Bank3_0)
			{
				slot = SlotId - Bank3_0;
				arr = Player.bank3.item;
			}
			else
			{
				if (SlotId >= TrashItem)
				{
					slot = 0;
					arr = null;
					return false;
				}
				if (SlotId >= Bank2_0)
				{
					slot = SlotId - Bank2_0;
					arr = Player.bank2.item;
				}
				else if (SlotId >= Bank1_0)
				{
					slot = SlotId - Bank1_0;
					arr = Player.bank.item;
				}
				else if (SlotId >= MiscDye0)
				{
					slot = SlotId - MiscDye0;
					arr = Player.miscDyes;
				}
				else if (SlotId >= Misc0)
				{
					slot = SlotId - Misc0;
					arr = Player.miscEquips;
				}
				else if (SlotId >= Dye0)
				{
					slot = SlotId - Dye0;
					arr = Player.dye;
				}
				else if (SlotId >= Armor0)
				{
					slot = SlotId - Armor0;
					arr = Player.armor;
				}
				else
				{
					slot = SlotId - Inventory0;
					arr = Player.inventory;
				}
			}
			return true;
		}
	}

	public static readonly int Inventory0;

	public static readonly int InventoryMouseItem;

	public static readonly int Armor0;

	public static readonly int Dye0;

	public static readonly int Misc0;

	public static readonly int MiscDye0;

	public static readonly int Bank1_0;

	public static readonly int Bank2_0;

	public static readonly int TrashItem;

	public static readonly int Bank3_0;

	public static readonly int Bank4_0;

	public static readonly int Loadout1_Armor_0;

	public static readonly int Loadout1_Dye_0;

	public static readonly int Loadout2_Armor_0;

	public static readonly int Loadout2_Dye_0;

	public static readonly int Loadout3_Armor_0;

	public static readonly int Loadout3_Dye_0;

	public static readonly int Count;

	public static bool[] CanRelay;

	private static int _nextSlotId;

	static PlayerItemSlotID()
	{
		CanRelay = new bool[0];
		Inventory0 = AllocateSlots(58, canNetRelay: true);
		InventoryMouseItem = AllocateSlots(1, canNetRelay: true);
		Armor0 = AllocateSlots(20, canNetRelay: true);
		Dye0 = AllocateSlots(10, canNetRelay: true);
		Misc0 = AllocateSlots(5, canNetRelay: true);
		MiscDye0 = AllocateSlots(5, canNetRelay: true);
		Bank1_0 = AllocateSlots(200, canNetRelay: false);
		Bank2_0 = AllocateSlots(200, canNetRelay: false);
		TrashItem = AllocateSlots(1, canNetRelay: false);
		Bank3_0 = AllocateSlots(200, canNetRelay: false);
		Bank4_0 = AllocateSlots(200, canNetRelay: true);
		Loadout1_Armor_0 = AllocateSlots(20, canNetRelay: true);
		Loadout1_Dye_0 = AllocateSlots(10, canNetRelay: true);
		Loadout2_Armor_0 = AllocateSlots(20, canNetRelay: true);
		Loadout2_Dye_0 = AllocateSlots(10, canNetRelay: true);
		Loadout3_Armor_0 = AllocateSlots(20, canNetRelay: true);
		Loadout3_Dye_0 = AllocateSlots(10, canNetRelay: true);
		Count = _nextSlotId;
	}

	private static int AllocateSlots(int amount, bool canNetRelay)
	{
		int nextSlotId = _nextSlotId;
		_nextSlotId += amount;
		int num = CanRelay.Length;
		Array.Resize(ref CanRelay, num + amount);
		for (int i = num; i < _nextSlotId; i++)
		{
			CanRelay[i] = canNetRelay;
		}
		return nextSlotId;
	}
}
