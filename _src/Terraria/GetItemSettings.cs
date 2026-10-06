using System;

namespace Terraria;

public struct GetItemSettings(bool LongText = false, bool NoText = false, bool CanGoIntoVoidVault = false, bool NoSound = false, Action<Item> StepAfterHandlingSlotNormally = null, bool NoCoinMerge = false)
{
	public static GetItemSettings GiftRecieved = new GetItemSettings(LongText: true);

	public static GetItemSettings LootAllFromBank = default;

	public static GetItemSettings LootAllFromChest = new GetItemSettings(LongText: false, NoText: false, CanGoIntoVoidVault: true);

	public static GetItemSettings PickupItemFromWorld = new GetItemSettings(LongText: false, NoText: false, CanGoIntoVoidVault: true);

	public static GetItemSettings QuickTransferFromSlot = new GetItemSettings(LongText: false, NoText: true);

	public static GetItemSettings ReturnItemFromSlot = new GetItemSettings(LongText: false, NoText: true);

	public static GetItemSettings ReturnItemShowAsNew = new GetItemSettings(LongText: false, NoText: true, CanGoIntoVoidVault: false, NoSound: false, MakeNewAndShiny);

	public static GetItemSettings ItemCreatedFromItemUsage = default;

	public static GetItemSettings RefundConsumedItem = new GetItemSettings(LongText: false, NoText: true, CanGoIntoVoidVault: true, NoSound: true);

	public static GetItemSettings ReturnItemShowAsNewNoCoinMerge = new GetItemSettings(LongText: false, NoText: true, CanGoIntoVoidVault: false, NoSound: false, MakeNewAndShiny, NoCoinMerge: true);

	public readonly bool LongText = LongText;

	public readonly bool NoText = NoText;

	public readonly bool CanGoIntoVoidVault = CanGoIntoVoidVault;

	public readonly bool NoSound = NoSound;

	public readonly bool NoCoinMerge = NoCoinMerge;

	public readonly Action<Item> StepAfterHandlingSlotNormally = StepAfterHandlingSlotNormally;

	public void HandlePostAction(Item item)
	{
		if (StepAfterHandlingSlotNormally != null)
		{
			StepAfterHandlingSlotNormally(item);
		}
	}

	private static void MakeNewAndShiny(Item item)
	{
		item.newAndShiny = true;
	}
}
