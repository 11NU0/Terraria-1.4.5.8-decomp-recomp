using System;
using System.Collections.Generic;
using System.Linq;
using Terraria.ID;
using Terraria.Testing;

namespace Terraria.UI;

public class ItemSorting
{
	private class ItemSortingLayer
	{
		public readonly string Name;

		public readonly Func<ItemSortingLayer, Item[], List<int>, List<int>> SortingMethod;

		public ItemSortingLayer(string name, Func<ItemSortingLayer, Item[], List<int>, List<int>> method)
		{
			Name = name;
			SortingMethod = method;
		}

		public void Validate(ref List<int> indexesSortable, Item[] inv)
		{
			if (_layerWhiteLists.TryGetValue(Name, out var list))
			{
				indexesSortable = indexesSortable.Where((int i) => list.Contains(inv[i].type)).ToList();
			}
		}

		public override string ToString()
		{
			return Name;
		}
	}

	private class ItemSortingLayers
	{
		public static ItemSortingLayer WeaponsMelee = new ItemSortingLayer("Weapons - Melee", (ItemSortingLayer layer, Item[] inv, List<int> itemsToSort) =>
		{
			List<int> indexesSortable = itemsToSort.Where((int i) => inv[i].damage > 0 && !inv[i].consumable && inv[i].ammo == 0 && inv[i].melee && inv[i].pick < 1 && inv[i].hammer < 1 && inv[i].axe < 1).ToList();
			layer.Validate(ref indexesSortable, inv);
			foreach (int item in indexesSortable)
			{
				itemsToSort.Remove(item);
			}
			SortIndicesStable(indexesSortable, (int x, int y) =>
			{
				int num = inv[y].OriginalRarity.CompareTo(inv[x].OriginalRarity);
				if (num == 0)
				{
					num = inv[y].OriginalDamage.CompareTo(inv[x].OriginalDamage);
				}
				return num;
			});
			return indexesSortable;
		});

		public static ItemSortingLayer WeaponsRanged = new ItemSortingLayer("Weapons - Ranged", (ItemSortingLayer layer, Item[] inv, List<int> itemsToSort) =>
		{
			List<int> indexesSortable = itemsToSort.Where((int i) => (inv[i].damage > 0 && !inv[i].consumable && inv[i].ammo == 0 && inv[i].ranged) || (inv[i].type >= 0 && ItemID.Sets.SortingPriorityWeaponsRanged[inv[i].type] > -1)).ToList();
			layer.Validate(ref indexesSortable, inv);
			foreach (int item2 in indexesSortable)
			{
				itemsToSort.Remove(item2);
			}
			SortIndicesStable(indexesSortable, (int x, int y) =>
			{
				int num = CompareWithPrioritySet(ItemID.Sets.SortingPriorityWeaponsRanged, inv[x].type, inv[y].type);
				if (num == 0)
				{
					num = inv[y].OriginalRarity.CompareTo(inv[x].OriginalRarity);
				}
				if (num == 0)
				{
					num = inv[y].OriginalDamage.CompareTo(inv[x].OriginalDamage);
				}
				return num;
			});
			return indexesSortable;
		});

		public static ItemSortingLayer WeaponsMagic = new ItemSortingLayer("Weapons - Magic", (ItemSortingLayer layer, Item[] inv, List<int> itemsToSort) =>
		{
			List<int> indexesSortable = itemsToSort.Where((int i) => inv[i].damage > 0 && !inv[i].consumable && inv[i].ammo == 0 && inv[i].magic).ToList();
			layer.Validate(ref indexesSortable, inv);
			foreach (int item3 in indexesSortable)
			{
				itemsToSort.Remove(item3);
			}
			SortIndicesStable(indexesSortable, (int x, int y) =>
			{
				int num = inv[y].OriginalRarity.CompareTo(inv[x].OriginalRarity);
				if (num == 0)
				{
					num = inv[y].OriginalDamage.CompareTo(inv[x].OriginalDamage);
				}
				return num;
			});
			return indexesSortable;
		});

		public static ItemSortingLayer WeaponsMinions = new ItemSortingLayer("Weapons - Minions", (ItemSortingLayer layer, Item[] inv, List<int> itemsToSort) =>
		{
			List<int> indexesSortable = itemsToSort.Where((int i) => inv[i].damage > 0 && !inv[i].consumable && inv[i].summon).ToList();
			layer.Validate(ref indexesSortable, inv);
			foreach (int item4 in indexesSortable)
			{
				itemsToSort.Remove(item4);
			}
			SortIndicesStable(indexesSortable, (int x, int y) =>
			{
				int num = inv[y].OriginalRarity.CompareTo(inv[x].OriginalRarity);
				if (num == 0)
				{
					num = inv[y].OriginalDamage.CompareTo(inv[x].OriginalDamage);
				}
				return num;
			});
			return indexesSortable;
		});

		public static ItemSortingLayer WeaponsAssorted = new ItemSortingLayer("Weapons - Assorted", (ItemSortingLayer layer, Item[] inv, List<int> itemsToSort) =>
		{
			List<int> indexesSortable = itemsToSort.Where((int i) => inv[i].damage > 0 && inv[i].ammo == 0 && inv[i].pick == 0 && inv[i].axe == 0 && inv[i].hammer == 0).ToList();
			layer.Validate(ref indexesSortable, inv);
			foreach (int item5 in indexesSortable)
			{
				itemsToSort.Remove(item5);
			}
			SortIndicesStable(indexesSortable, (int x, int y) =>
			{
				int num = inv[y].OriginalRarity.CompareTo(inv[x].OriginalRarity);
				if (num == 0)
				{
					num = inv[y].OriginalDamage.CompareTo(inv[x].OriginalDamage);
				}
				return num;
			});
			return indexesSortable;
		});

		public static ItemSortingLayer WeaponsAmmo = new ItemSortingLayer("Weapons - Ammo", (ItemSortingLayer layer, Item[] inv, List<int> itemsToSort) =>
		{
			List<int> indexesSortable = itemsToSort.Where((int i) => inv[i].ammo > 0 && inv[i].damage > 0).ToList();
			layer.Validate(ref indexesSortable, inv);
			foreach (int item6 in indexesSortable)
			{
				itemsToSort.Remove(item6);
			}
			SortIndicesStable(indexesSortable, (int x, int y) =>
			{
				int num = inv[y].OriginalRarity.CompareTo(inv[x].OriginalRarity);
				if (num == 0)
				{
					num = inv[y].OriginalDamage.CompareTo(inv[x].OriginalDamage);
				}
				return num;
			});
			return indexesSortable;
		});

		public static ItemSortingLayer ToolsPicksaws = new ItemSortingLayer("Tools - Picksaws", (ItemSortingLayer layer, Item[] inv, List<int> itemsToSort) =>
		{
			List<int> indexesSortable = itemsToSort.Where((int i) => inv[i].pick > 0 && inv[i].axe > 0).ToList();
			layer.Validate(ref indexesSortable, inv);
			foreach (int item7 in indexesSortable)
			{
				itemsToSort.Remove(item7);
			}
			SortIndicesStable(indexesSortable, (int x, int y) => inv[x].pick.CompareTo(inv[y].pick));
			return indexesSortable;
		});

		public static ItemSortingLayer ToolsHamaxes = new ItemSortingLayer("Tools - Hamaxes", (ItemSortingLayer layer, Item[] inv, List<int> itemsToSort) =>
		{
			List<int> indexesSortable = itemsToSort.Where((int i) => inv[i].hammer > 0 && inv[i].axe > 0).ToList();
			layer.Validate(ref indexesSortable, inv);
			foreach (int item8 in indexesSortable)
			{
				itemsToSort.Remove(item8);
			}
			SortIndicesStable(indexesSortable, (int x, int y) => inv[x].axe.CompareTo(inv[y].axe));
			return indexesSortable;
		});

		public static ItemSortingLayer ToolsPickaxes = new ItemSortingLayer("Tools - Pickaxes", (ItemSortingLayer layer, Item[] inv, List<int> itemsToSort) =>
		{
			List<int> indexesSortable = itemsToSort.Where((int i) => inv[i].pick > 0).ToList();
			layer.Validate(ref indexesSortable, inv);
			foreach (int item9 in indexesSortable)
			{
				itemsToSort.Remove(item9);
			}
			SortIndicesStable(indexesSortable, (int x, int y) => inv[x].pick.CompareTo(inv[y].pick));
			return indexesSortable;
		});

		public static ItemSortingLayer ToolsAxes = new ItemSortingLayer("Tools - Axes", (ItemSortingLayer layer, Item[] inv, List<int> itemsToSort) =>
		{
			List<int> indexesSortable = itemsToSort.Where((int i) => inv[i].axe > 0).ToList();
			layer.Validate(ref indexesSortable, inv);
			foreach (int item10 in indexesSortable)
			{
				itemsToSort.Remove(item10);
			}
			SortIndicesStable(indexesSortable, (int x, int y) => inv[x].axe.CompareTo(inv[y].axe));
			return indexesSortable;
		});

		public static ItemSortingLayer ToolsHammers = new ItemSortingLayer("Tools - Hammers", (ItemSortingLayer layer, Item[] inv, List<int> itemsToSort) =>
		{
			List<int> indexesSortable = itemsToSort.Where((int i) => inv[i].hammer > 0).ToList();
			layer.Validate(ref indexesSortable, inv);
			foreach (int item11 in indexesSortable)
			{
				itemsToSort.Remove(item11);
			}
			SortIndicesStable(indexesSortable, (int x, int y) => inv[x].hammer.CompareTo(inv[y].hammer));
			return indexesSortable;
		});

		public static ItemSortingLayer ToolsTerraforming = new ItemSortingLayer("Tools - Terraforming", (ItemSortingLayer layer, Item[] inv, List<int> itemsToSort) =>
		{
			List<int> indexesSortable = itemsToSort.Where((int i) => inv[i].type > 0 && ItemID.Sets.SortingPriorityTerraforming[inv[i].type] > -1).ToList();
			layer.Validate(ref indexesSortable, inv);
			foreach (int item12 in indexesSortable)
			{
				itemsToSort.Remove(item12);
			}
			SortIndicesStable(indexesSortable, (int x, int y) =>
			{
				int num = ItemID.Sets.SortingPriorityTerraforming[inv[x].type].CompareTo(ItemID.Sets.SortingPriorityTerraforming[inv[y].type]);
				if (num == 0)
				{
					num = inv[y].stack.CompareTo(inv[x].stack);
				}
				return num;
			});
			return indexesSortable;
		});

		public static ItemSortingLayer ToolsFishing = new ItemSortingLayer("Tools - Fishing", (ItemSortingLayer layer, Item[] inv, List<int> itemsToSort) =>
		{
			List<int> indexesSortable = itemsToSort.Where((int i) => inv[i].fishingPole > 0 || inv[i].bait > 0 || inv[i].questItem || (inv[i].type > 0 && (ItemID.Sets.IsFishingCrate[inv[i].type] || ItemID.Sets.IsBasicFish[inv[i].type] || ItemID.Sets.SortingPriorityToolsFishing[inv[i].type] > -1))).ToList();
			layer.Validate(ref indexesSortable, inv);
			foreach (int item13 in indexesSortable)
			{
				itemsToSort.Remove(item13);
			}
			SortIndicesStable(indexesSortable, (int x, int y) =>
			{
				int num = CompareWithPrioritySet(ItemID.Sets.SortingPriorityToolsFishing, inv[x].type, inv[y].type);
				if (num == 0)
				{
					num = inv[y].fishingPole.CompareTo(inv[x].fishingPole);
				}
				if (num == 0)
				{
					num = inv[y].bait.CompareTo(inv[x].bait);
				}
				if (num == 0)
				{
					num = inv[y].questItem.CompareTo(inv[x].questItem);
				}
				if (num == 0 && inv[y].type >= 0 && inv[x].type >= 0)
				{
					if (num == 0)
					{
						num = ItemID.Sets.IsFishingCrate[inv[y].type].CompareTo(ItemID.Sets.IsFishingCrate[inv[x].type]);
					}
					if (num == 0)
					{
						num = ItemID.Sets.IsBasicFish[inv[y].type].CompareTo(ItemID.Sets.IsBasicFish[inv[x].type]);
					}
				}
				if (num == 0)
				{
					num = inv[y].OriginalRarity.CompareTo(inv[x].OriginalRarity);
				}
				if (num == 0)
				{
					num = inv[y].stack.CompareTo(inv[x].stack);
				}
				return num;
			});
			return indexesSortable;
		});

		public static ItemSortingLayer ToolsGolf = new ItemSortingLayer("Tools - Golf", (ItemSortingLayer layer, Item[] inv, List<int> itemsToSort) =>
		{
			List<int> indexesSortable = itemsToSort.Where((int i) => inv[i].type > 0 && ItemID.Sets.SortingPriorityToolsGolf[inv[i].type] > -1).ToList();
			layer.Validate(ref indexesSortable, inv);
			foreach (int item14 in indexesSortable)
			{
				itemsToSort.Remove(item14);
			}
			SortIndicesStable(indexesSortable, (int x, int y) =>
			{
				int num = ItemID.Sets.SortingPriorityToolsGolf[inv[x].type].CompareTo(ItemID.Sets.SortingPriorityToolsGolf[inv[y].type]);
				if (num == 0)
				{
					num = inv[y].OriginalRarity.CompareTo(inv[x].OriginalRarity);
				}
				if (num == 0)
				{
					num = inv[y].stack.CompareTo(inv[x].stack);
				}
				return num;
			});
			return indexesSortable;
		});

		public static ItemSortingLayer ToolsInstruments = new ItemSortingLayer("Tools - Instruments", (ItemSortingLayer layer, Item[] inv, List<int> itemsToSort) =>
		{
			List<int> indexesSortable = itemsToSort.Where((int i) => inv[i].type > 0 && ItemID.Sets.SortingPriorityToolsInstruments[inv[i].type] > -1).ToList();
			layer.Validate(ref indexesSortable, inv);
			foreach (int item15 in indexesSortable)
			{
				itemsToSort.Remove(item15);
			}
			SortIndicesStable(indexesSortable, (int x, int y) =>
			{
				int num = ItemID.Sets.SortingPriorityToolsInstruments[inv[x].type].CompareTo(ItemID.Sets.SortingPriorityToolsInstruments[inv[y].type]);
				if (num == 0)
				{
					num = inv[y].OriginalRarity.CompareTo(inv[x].OriginalRarity);
				}
				if (num == 0)
				{
					num = inv[y].stack.CompareTo(inv[x].stack);
				}
				return num;
			});
			return indexesSortable;
		});

		public static ItemSortingLayer ToolsKeys = new ItemSortingLayer("Tools - Keys", (ItemSortingLayer layer, Item[] inv, List<int> itemsToSort) =>
		{
			List<int> indexesSortable = itemsToSort.Where((int i) => inv[i].type > 0 && ItemID.Sets.SortingPriorityToolsKeys[inv[i].type] > -1).ToList();
			layer.Validate(ref indexesSortable, inv);
			foreach (int item16 in indexesSortable)
			{
				itemsToSort.Remove(item16);
			}
			SortIndicesStable(indexesSortable, (int x, int y) =>
			{
				int num = ItemID.Sets.SortingPriorityToolsKeys[inv[x].type].CompareTo(ItemID.Sets.SortingPriorityToolsKeys[inv[y].type]);
				if (num == 0)
				{
					num = inv[y].OriginalRarity.CompareTo(inv[x].OriginalRarity);
				}
				if (num == 0)
				{
					num = inv[y].stack.CompareTo(inv[x].stack);
				}
				return num;
			});
			return indexesSortable;
		});

		public static ItemSortingLayer ToolsKites = new ItemSortingLayer("Tools - Kites", (ItemSortingLayer layer, Item[] inv, List<int> itemsToSort) =>
		{
			List<int> indexesSortable = itemsToSort.Where((int i) => inv[i].type > 0 && ItemID.Sets.SortingPriorityToolsKites[inv[i].type] > -1).ToList();
			layer.Validate(ref indexesSortable, inv);
			foreach (int item17 in indexesSortable)
			{
				itemsToSort.Remove(item17);
			}
			SortIndicesStable(indexesSortable, (int x, int y) =>
			{
				int num = ItemID.Sets.SortingPriorityToolsKites[inv[x].type].CompareTo(ItemID.Sets.SortingPriorityToolsKites[inv[y].type]);
				if (num == 0)
				{
					num = inv[y].OriginalRarity.CompareTo(inv[x].OriginalRarity);
				}
				if (num == 0)
				{
					num = inv[y].stack.CompareTo(inv[x].stack);
				}
				return num;
			});
			return indexesSortable;
		});

		public static ItemSortingLayer ToolsAmmoLeftovers = new ItemSortingLayer("Weapons - Ammo Leftovers", (ItemSortingLayer layer, Item[] inv, List<int> itemsToSort) =>
		{
			List<int> indexesSortable = itemsToSort.Where((int i) => inv[i].ammo > 0 && inv[i].type >= 0 && inv[i].type < ItemID.Count && !ItemID.Sets.IsFood[inv[i].type] && ItemID.Sets.SortingPriorityMiscAcorns[inv[i].type] == -1).ToList();
			layer.Validate(ref indexesSortable, inv);
			foreach (int item18 in indexesSortable)
			{
				itemsToSort.Remove(item18);
			}
			SortIndicesStable(indexesSortable, (int x, int y) =>
			{
				int num = inv[y].OriginalRarity.CompareTo(inv[x].OriginalRarity);
				if (num == 0)
				{
					num = inv[y].OriginalDamage.CompareTo(inv[x].OriginalDamage);
				}
				return num;
			});
			return indexesSortable;
		});

		public static ItemSortingLayer ToolsMisc = new ItemSortingLayer("Tools - Misc", (ItemSortingLayer layer, Item[] inv, List<int> itemsToSort) =>
		{
			List<int> indexesSortable = itemsToSort.Where((int i) => inv[i].type > 0 && ItemID.Sets.SortingPriorityToolsMisc[inv[i].type] > -1).ToList();
			layer.Validate(ref indexesSortable, inv);
			foreach (int item19 in indexesSortable)
			{
				itemsToSort.Remove(item19);
			}
			SortIndicesStable(indexesSortable, (int x, int y) =>
			{
				int num = ItemID.Sets.SortingPriorityToolsMisc[inv[x].type].CompareTo(ItemID.Sets.SortingPriorityToolsMisc[inv[y].type]);
				if (num == 0)
				{
					num = inv[y].OriginalRarity.CompareTo(inv[x].OriginalRarity);
				}
				if (num == 0)
				{
					num = inv[y].stack.CompareTo(inv[x].stack);
				}
				return num;
			});
			return indexesSortable;
		});

		public static ItemSortingLayer ArmorCombat = new ItemSortingLayer("Armor - Combat", (ItemSortingLayer layer, Item[] inv, List<int> itemsToSort) =>
		{
			List<int> indexesSortable = itemsToSort.Where((int i) => (inv[i].bodySlot >= 0 || inv[i].headSlot >= 0 || inv[i].legSlot >= 0) && !inv[i].vanity).ToList();
			layer.Validate(ref indexesSortable, inv);
			foreach (int item20 in indexesSortable)
			{
				itemsToSort.Remove(item20);
			}
			SortIndicesStable(indexesSortable, (int x, int y) =>
			{
				int num = inv[y].OriginalRarity.CompareTo(inv[x].OriginalRarity);
				if (num == 0)
				{
					num = inv[y].OriginalDefense.CompareTo(inv[x].OriginalDefense);
				}
				if (num == 0)
				{
					num = inv[x].type.CompareTo(inv[y].type);
				}
				return num;
			});
			return indexesSortable;
		});

		public static ItemSortingLayer ArmorVanity = new ItemSortingLayer("Armor - Vanity", (ItemSortingLayer layer, Item[] inv, List<int> itemsToSort) =>
		{
			List<int> indexesSortable = itemsToSort.Where((int i) => (inv[i].bodySlot >= 0 || inv[i].headSlot >= 0 || inv[i].legSlot >= 0) && inv[i].vanity).ToList();
			layer.Validate(ref indexesSortable, inv);
			foreach (int item21 in indexesSortable)
			{
				itemsToSort.Remove(item21);
			}
			SortIndicesStable(indexesSortable, (int x, int y) =>
			{
				int num = inv[y].OriginalRarity.CompareTo(inv[x].OriginalRarity);
				if (num == 0)
				{
					num = inv[x].type.CompareTo(inv[y].type);
				}
				return num;
			});
			return indexesSortable;
		});

		public static ItemSortingLayer ArmorAccessories = new ItemSortingLayer("Armor - Accessories", (ItemSortingLayer layer, Item[] inv, List<int> itemsToSort) =>
		{
			List<int> indexesSortable = itemsToSort.Where((int i) => inv[i].accessory).ToList();
			layer.Validate(ref indexesSortable, inv);
			foreach (int item22 in indexesSortable)
			{
				itemsToSort.Remove(item22);
			}
			SortIndicesStable(indexesSortable, (int x, int y) =>
			{
				int num = inv[x].vanity.CompareTo(inv[y].vanity);
				if (num == 0)
				{
					num = inv[y].OriginalRarity.CompareTo(inv[x].OriginalRarity);
				}
				if (num == 0)
				{
					num = inv[y].OriginalDefense.CompareTo(inv[x].OriginalDefense);
				}
				if (num == 0)
				{
					num = inv[x].type.CompareTo(inv[y].type);
				}
				return num;
			});
			return indexesSortable;
		});

		public static ItemSortingLayer EquipGrapple = new ItemSortingLayer("Equip - Grapple", (ItemSortingLayer layer, Item[] inv, List<int> itemsToSort) =>
		{
			List<int> indexesSortable = itemsToSort.Where((int i) => Main.projHook[inv[i].shoot]).ToList();
			layer.Validate(ref indexesSortable, inv);
			foreach (int item23 in indexesSortable)
			{
				itemsToSort.Remove(item23);
			}
			SortIndicesStable(indexesSortable, (int x, int y) =>
			{
				int num = inv[y].OriginalRarity.CompareTo(inv[x].OriginalRarity);
				if (num == 0)
				{
					num = inv[x].type.CompareTo(inv[y].type);
				}
				return num;
			});
			return indexesSortable;
		});

		public static ItemSortingLayer EquipMount = new ItemSortingLayer("Equip - Mount", (ItemSortingLayer layer, Item[] inv, List<int> itemsToSort) =>
		{
			List<int> indexesSortable = itemsToSort.Where((int i) => inv[i].mountType != -1 && !MountID.Sets.Cart[inv[i].mountType]).ToList();
			layer.Validate(ref indexesSortable, inv);
			foreach (int item24 in indexesSortable)
			{
				itemsToSort.Remove(item24);
			}
			SortIndicesStable(indexesSortable, (int x, int y) =>
			{
				int num = inv[y].OriginalRarity.CompareTo(inv[x].OriginalRarity);
				if (num == 0)
				{
					num = inv[x].type.CompareTo(inv[y].type);
				}
				return num;
			});
			return indexesSortable;
		});

		public static ItemSortingLayer EquipCart = new ItemSortingLayer("Equip - Cart", (ItemSortingLayer layer, Item[] inv, List<int> itemsToSort) =>
		{
			List<int> indexesSortable = itemsToSort.Where((int i) => inv[i].mountType != -1 && MountID.Sets.Cart[inv[i].mountType]).ToList();
			layer.Validate(ref indexesSortable, inv);
			foreach (int item25 in indexesSortable)
			{
				itemsToSort.Remove(item25);
			}
			SortIndicesStable(indexesSortable, (int x, int y) =>
			{
				int num = inv[y].OriginalRarity.CompareTo(inv[x].OriginalRarity);
				if (num == 0)
				{
					num = inv[x].type.CompareTo(inv[y].type);
				}
				return num;
			});
			return indexesSortable;
		});

		public static ItemSortingLayer EquipLightPet = new ItemSortingLayer("Equip - Light Pet", (ItemSortingLayer layer, Item[] inv, List<int> itemsToSort) =>
		{
			List<int> indexesSortable = itemsToSort.Where((int i) => inv[i].buffType > 0 && Main.lightPet[inv[i].buffType]).ToList();
			layer.Validate(ref indexesSortable, inv);
			foreach (int item26 in indexesSortable)
			{
				itemsToSort.Remove(item26);
			}
			SortIndicesStable(indexesSortable, (int x, int y) =>
			{
				int num = inv[y].OriginalRarity.CompareTo(inv[x].OriginalRarity);
				if (num == 0)
				{
					num = inv[x].type.CompareTo(inv[y].type);
				}
				return num;
			});
			return indexesSortable;
		});

		public static ItemSortingLayer EquipVanityPet = new ItemSortingLayer("Equip - Vanity Pet", (ItemSortingLayer layer, Item[] inv, List<int> itemsToSort) =>
		{
			List<int> indexesSortable = itemsToSort.Where((int i) => inv[i].buffType > 0 && Main.vanityPet[inv[i].buffType]).ToList();
			layer.Validate(ref indexesSortable, inv);
			foreach (int item27 in indexesSortable)
			{
				itemsToSort.Remove(item27);
			}
			SortIndicesStable(indexesSortable, (int x, int y) =>
			{
				int num = inv[y].OriginalRarity.CompareTo(inv[x].OriginalRarity);
				if (num == 0)
				{
					num = inv[x].type.CompareTo(inv[y].type);
				}
				return num;
			});
			return indexesSortable;
		});

		public static ItemSortingLayer PotionsLife = new ItemSortingLayer("Potions - Life", (ItemSortingLayer layer, Item[] inv, List<int> itemsToSort) =>
		{
			List<int> indexesSortable = itemsToSort.Where((int i) => inv[i].consumable && inv[i].healLife > 0 && inv[i].healMana < 1 && inv[i].type != 5).ToList();
			layer.Validate(ref indexesSortable, inv);
			foreach (int item28 in indexesSortable)
			{
				itemsToSort.Remove(item28);
			}
			SortIndicesStable(indexesSortable, (int x, int y) =>
			{
				int num = inv[y].healLife.CompareTo(inv[x].healLife);
				if (num == 0)
				{
					num = inv[y].stack.CompareTo(inv[x].stack);
				}
				return num;
			});
			return indexesSortable;
		});

		public static ItemSortingLayer PotionsJustTheMushroom = new ItemSortingLayer("Potions - Just The Mushroom", (ItemSortingLayer layer, Item[] inv, List<int> itemsToSort) =>
		{
			List<int> indexesSortable = itemsToSort.Where((int i) => inv[i].type == 5).ToList();
			layer.Validate(ref indexesSortable, inv);
			foreach (int item29 in indexesSortable)
			{
				itemsToSort.Remove(item29);
			}
			SortIndicesStable(indexesSortable, (int x, int y) => inv[y].stack.CompareTo(inv[x].stack));
			return indexesSortable;
		});

		public static ItemSortingLayer PotionsMana = new ItemSortingLayer("Potions - Mana", (ItemSortingLayer layer, Item[] inv, List<int> itemsToSort) =>
		{
			List<int> indexesSortable = itemsToSort.Where((int i) => inv[i].consumable && inv[i].healLife < 1 && inv[i].healMana > 0).ToList();
			layer.Validate(ref indexesSortable, inv);
			foreach (int item30 in indexesSortable)
			{
				itemsToSort.Remove(item30);
			}
			SortIndicesStable(indexesSortable, (int x, int y) =>
			{
				int num = inv[y].healMana.CompareTo(inv[x].healMana);
				if (num == 0)
				{
					num = inv[y].stack.CompareTo(inv[x].stack);
				}
				return num;
			});
			return indexesSortable;
		});

		public static ItemSortingLayer PotionsElixirs = new ItemSortingLayer("Potions - Elixirs", (ItemSortingLayer layer, Item[] inv, List<int> itemsToSort) =>
		{
			List<int> indexesSortable = itemsToSort.Where((int i) => inv[i].consumable && inv[i].healLife > 0 && inv[i].healMana > 0).ToList();
			layer.Validate(ref indexesSortable, inv);
			foreach (int item31 in indexesSortable)
			{
				itemsToSort.Remove(item31);
			}
			SortIndicesStable(indexesSortable, (int x, int y) =>
			{
				int num = inv[y].healLife.CompareTo(inv[x].healLife);
				if (num == 0)
				{
					num = inv[y].stack.CompareTo(inv[x].stack);
				}
				return num;
			});
			return indexesSortable;
		});

		public static ItemSortingLayer PotionsBuffs = new ItemSortingLayer("Potions - Buffs", (ItemSortingLayer layer, Item[] inv, List<int> itemsToSort) =>
		{
			List<int> indexesSortable = itemsToSort.Where((int i) => (inv[i].consumable && inv[i].buffType > 0 && inv[i].type >= 0 && inv[i].type < ItemID.Count && !ItemID.Sets.IsFood[inv[i].type]) || (inv[i].type >= 0 && ItemID.Sets.SortingPriorityPotionsBuffs[inv[i].type] > -1)).ToList();
			layer.Validate(ref indexesSortable, inv);
			foreach (int item32 in indexesSortable)
			{
				itemsToSort.Remove(item32);
			}
			SortIndicesStable(indexesSortable, (int x, int y) =>
			{
				int num = CompareWithPrioritySet(ItemID.Sets.SortingPriorityPotionsBuffs, inv[x].type, inv[y].type);
				if (num == 0)
				{
					num = inv[y].OriginalRarity.CompareTo(inv[x].OriginalRarity);
				}
				if (num == 0)
				{
					num = inv[x].type.CompareTo(inv[y].type);
				}
				if (num == 0)
				{
					num = inv[y].stack.CompareTo(inv[x].stack);
				}
				return num;
			});
			return indexesSortable;
		});

		public static ItemSortingLayer PotionsFood = new ItemSortingLayer("Potions - Food", (ItemSortingLayer layer, Item[] inv, List<int> itemsToSort) =>
		{
			List<int> indexesSortable = itemsToSort.Where((int i) => inv[i].consumable && inv[i].buffType > 0 && inv[i].type >= 0 && inv[i].type < ItemID.Count && ItemID.Sets.IsFood[inv[i].type]).ToList();
			layer.Validate(ref indexesSortable, inv);
			foreach (int item33 in indexesSortable)
			{
				itemsToSort.Remove(item33);
			}
			SortIndicesStable(indexesSortable, (int x, int y) =>
			{
				int num = ((inv[y].buffType >= 0 && inv[y].buffType < BuffID.Count) ? BuffID.Sets.SortingPriorityFoodBuffs[inv[y].buffType].CompareTo(BuffID.Sets.SortingPriorityFoodBuffs[inv[x].buffType]) : 0);
				if (num == 0)
				{
					num = inv[y].OriginalRarity.CompareTo(inv[x].OriginalRarity);
				}
				if (num == 0)
				{
					num = inv[x].type.CompareTo(inv[y].type);
				}
				if (num == 0)
				{
					num = inv[y].stack.CompareTo(inv[x].stack);
				}
				return num;
			});
			return indexesSortable;
		});

		public static ItemSortingLayer PotionsDyes = new ItemSortingLayer("Potions - Dyes", (ItemSortingLayer layer, Item[] inv, List<int> itemsToSort) =>
		{
			List<int> indexesSortable = itemsToSort.Where((int i) => inv[i].dye > 0 || (inv[i].type >= 0 && ItemID.Sets.SortingPriorityPotionsDyeMaterial[inv[i].type] > -1)).ToList();
			layer.Validate(ref indexesSortable, inv);
			foreach (int item34 in indexesSortable)
			{
				itemsToSort.Remove(item34);
			}
			SortIndicesStable(indexesSortable, (int x, int y) =>
			{
				int num = inv[y].OriginalRarity.CompareTo(inv[x].OriginalRarity);
				if (num == 0)
				{
					num = inv[y].dye.CompareTo(inv[x].dye);
				}
				if (num == 0)
				{
					num = CompareWithPrioritySet(ItemID.Sets.SortingPriorityPotionsDyeMaterial, inv[x].type, inv[y].type);
				}
				if (num == 0)
				{
					num = inv[y].stack.CompareTo(inv[x].stack);
				}
				return num;
			});
			return indexesSortable;
		});

		public static ItemSortingLayer PotionsHairDyes = new ItemSortingLayer("Potions - Hair Dyes", (ItemSortingLayer layer, Item[] inv, List<int> itemsToSort) =>
		{
			List<int> indexesSortable = itemsToSort.Where((int i) => inv[i].hairDye >= 0).ToList();
			layer.Validate(ref indexesSortable, inv);
			foreach (int item35 in indexesSortable)
			{
				itemsToSort.Remove(item35);
			}
			SortIndicesStable(indexesSortable, (int x, int y) =>
			{
				int num = inv[y].OriginalRarity.CompareTo(inv[x].OriginalRarity);
				if (num == 0)
				{
					num = inv[y].hairDye.CompareTo(inv[x].hairDye);
				}
				if (num == 0)
				{
					num = inv[y].stack.CompareTo(inv[x].stack);
				}
				return num;
			});
			return indexesSortable;
		});

		public static ItemSortingLayer MiscValuables = new ItemSortingLayer("Misc - Importants", (ItemSortingLayer layer, Item[] inv, List<int> itemsToSort) =>
		{
			List<int> indexesSortable = itemsToSort.Where((int i) => inv[i].type > 0 && ItemID.Sets.SortingPriorityMiscImportants[inv[i].type] > -1).ToList();
			layer.Validate(ref indexesSortable, inv);
			foreach (int item36 in indexesSortable)
			{
				itemsToSort.Remove(item36);
			}
			SortIndicesStable(indexesSortable, (int x, int y) =>
			{
				int num = ItemID.Sets.SortingPriorityMiscImportants[inv[x].type].CompareTo(ItemID.Sets.SortingPriorityMiscImportants[inv[y].type]);
				if (num == 0)
				{
					num = inv[y].stack.CompareTo(inv[x].stack);
				}
				return num;
			});
			return indexesSortable;
		});

		public static ItemSortingLayer MiscWiring = new ItemSortingLayer("Misc - Wiring", (ItemSortingLayer layer, Item[] inv, List<int> itemsToSort) =>
		{
			List<int> indexesSortable = itemsToSort.Where((int i) => (inv[i].type > 0 && ItemID.Sets.SortingPriorityWiring[inv[i].type] > -1) || inv[i].mech).ToList();
			layer.Validate(ref indexesSortable, inv);
			foreach (int item37 in indexesSortable)
			{
				itemsToSort.Remove(item37);
			}
			SortIndicesStable(indexesSortable, (int x, int y) =>
			{
				int num = ItemID.Sets.SortingPriorityWiring[inv[y].type].CompareTo(ItemID.Sets.SortingPriorityWiring[inv[x].type]);
				if (num == 0)
				{
					num = inv[y].OriginalRarity.CompareTo(inv[x].OriginalRarity);
				}
				if (num == 0)
				{
					num = inv[y].type.CompareTo(inv[x].type);
				}
				if (num == 0)
				{
					num = inv[y].stack.CompareTo(inv[x].stack);
				}
				return num;
			});
			return indexesSortable;
		});

		public static ItemSortingLayer MiscMaterials = new ItemSortingLayer("Misc - Materials", (ItemSortingLayer layer, Item[] inv, List<int> itemsToSort) =>
		{
			List<int> indexesSortable = itemsToSort.Where((int i) => inv[i].type > 0 && ItemID.Sets.SortingPriorityMaterials[inv[i].type] > -1).ToList();
			layer.Validate(ref indexesSortable, inv);
			foreach (int item38 in indexesSortable)
			{
				itemsToSort.Remove(item38);
			}
			SortIndicesStable(indexesSortable, (int x, int y) =>
			{
				int num = ItemID.Sets.SortingPriorityMaterials[inv[y].type].CompareTo(ItemID.Sets.SortingPriorityMaterials[inv[x].type]);
				if (num == 0)
				{
					num = inv[y].stack.CompareTo(inv[x].stack);
				}
				return num;
			});
			return indexesSortable;
		});

		public static ItemSortingLayer MiscJustTheGlowingMushroom = new ItemSortingLayer("Misc - Just The Glowing Mushroom", (ItemSortingLayer layer, Item[] inv, List<int> itemsToSort) =>
		{
			List<int> indexesSortable = itemsToSort.Where((int i) => inv[i].type == 183).ToList();
			layer.Validate(ref indexesSortable, inv);
			foreach (int item39 in indexesSortable)
			{
				itemsToSort.Remove(item39);
			}
			SortIndicesStable(indexesSortable, (int x, int y) => inv[y].stack.CompareTo(inv[x].stack));
			return indexesSortable;
		});

		public static ItemSortingLayer MiscExtractinator = new ItemSortingLayer("Misc - Extractinator", (ItemSortingLayer layer, Item[] inv, List<int> itemsToSort) =>
		{
			List<int> indexesSortable = itemsToSort.Where((int i) => inv[i].type > 0 && ItemID.Sets.SortingPriorityExtractibles[inv[i].type] > -1).ToList();
			layer.Validate(ref indexesSortable, inv);
			foreach (int item40 in indexesSortable)
			{
				itemsToSort.Remove(item40);
			}
			SortIndicesStable(indexesSortable, (int x, int y) =>
			{
				int num = ItemID.Sets.SortingPriorityExtractibles[inv[y].type].CompareTo(ItemID.Sets.SortingPriorityExtractibles[inv[x].type]);
				if (num == 0)
				{
					num = inv[y].stack.CompareTo(inv[x].stack);
				}
				return num;
			});
			return indexesSortable;
		});

		public static ItemSortingLayer MiscPainting = new ItemSortingLayer("Misc - Painting", (ItemSortingLayer layer, Item[] inv, List<int> itemsToSort) =>
		{
			List<int> indexesSortable = itemsToSort.Where((int i) => (inv[i].type > 0 && ItemID.Sets.SortingPriorityPainting[inv[i].type] > -1) || inv[i].paint > 0).ToList();
			layer.Validate(ref indexesSortable, inv);
			foreach (int item41 in indexesSortable)
			{
				itemsToSort.Remove(item41);
			}
			SortIndicesStable(indexesSortable, (int x, int y) =>
			{
				int num = ItemID.Sets.SortingPriorityPainting[inv[y].type].CompareTo(ItemID.Sets.SortingPriorityPainting[inv[x].type]);
				if (num == 0)
				{
					num = inv[x].paint.CompareTo(inv[y].paint);
				}
				if (num == 0)
				{
					num = inv[x].paintCoating.CompareTo(inv[y].paintCoating);
				}
				if (num == 0)
				{
					num = inv[y].stack.CompareTo(inv[x].stack);
				}
				return num;
			});
			return indexesSortable;
		});

		public static ItemSortingLayer MiscRopes = new ItemSortingLayer("Misc - Ropes", (ItemSortingLayer layer, Item[] inv, List<int> itemsToSort) =>
		{
			List<int> indexesSortable = itemsToSort.Where((int i) => inv[i].type > 0 && ItemID.Sets.SortingPriorityRopes[inv[i].type] > -1).ToList();
			layer.Validate(ref indexesSortable, inv);
			foreach (int item42 in indexesSortable)
			{
				itemsToSort.Remove(item42);
			}
			SortIndicesStable(indexesSortable, (int x, int y) =>
			{
				int num = ItemID.Sets.SortingPriorityRopes[inv[y].type].CompareTo(ItemID.Sets.SortingPriorityRopes[inv[x].type]);
				if (num == 0)
				{
					num = inv[y].stack.CompareTo(inv[x].stack);
				}
				return num;
			});
			return indexesSortable;
		});

		public static ItemSortingLayer MiscHerbsAndSeeds = new ItemSortingLayer("Misc - Herbs And Seeds", (ItemSortingLayer layer, Item[] inv, List<int> itemsToSort) =>
		{
			List<int> indexesSortable = itemsToSort.Where((int i) => inv[i].type > 0 && ItemID.Sets.SortingPriorityMiscHerbsAndSeeds[inv[i].type] > -1).ToList();
			layer.Validate(ref indexesSortable, inv);
			foreach (int item43 in indexesSortable)
			{
				itemsToSort.Remove(item43);
			}
			SortIndicesStable(indexesSortable, (int x, int y) =>
			{
				int num = ItemID.Sets.SortingPriorityMiscHerbsAndSeeds[inv[y].type].CompareTo(ItemID.Sets.SortingPriorityMiscHerbsAndSeeds[inv[x].type]);
				if (num == 0)
				{
					num = inv[y].stack.CompareTo(inv[x].stack);
				}
				return num;
			});
			return indexesSortable;
		});

		public static ItemSortingLayer MiscGems = new ItemSortingLayer("Misc - Gems", (ItemSortingLayer layer, Item[] inv, List<int> itemsToSort) =>
		{
			List<int> indexesSortable = itemsToSort.Where((int i) => inv[i].type > 0 && ItemID.Sets.SortingPriorityMiscGems[inv[i].type] > -1).ToList();
			layer.Validate(ref indexesSortable, inv);
			foreach (int item44 in indexesSortable)
			{
				itemsToSort.Remove(item44);
			}
			SortIndicesStable(indexesSortable, (int x, int y) =>
			{
				int num = ItemID.Sets.SortingPriorityMiscGems[inv[y].type].CompareTo(ItemID.Sets.SortingPriorityMiscGems[inv[x].type]);
				if (num == 0)
				{
					num = inv[y].stack.CompareTo(inv[x].stack);
				}
				return num;
			});
			return indexesSortable;
		});

		public static ItemSortingLayer MiscAcorns = new ItemSortingLayer("Misc - Acorns", (ItemSortingLayer layer, Item[] inv, List<int> itemsToSort) =>
		{
			List<int> indexesSortable = itemsToSort.Where((int i) => inv[i].type > 0 && ItemID.Sets.SortingPriorityMiscAcorns[inv[i].type] > -1).ToList();
			layer.Validate(ref indexesSortable, inv);
			foreach (int item45 in indexesSortable)
			{
				itemsToSort.Remove(item45);
			}
			SortIndicesStable(indexesSortable, (int x, int y) =>
			{
				int num = ItemID.Sets.SortingPriorityMiscAcorns[inv[y].type].CompareTo(ItemID.Sets.SortingPriorityMiscAcorns[inv[x].type]);
				if (num == 0)
				{
					num = inv[y].stack.CompareTo(inv[x].stack);
				}
				return num;
			});
			return indexesSortable;
		});

		public static ItemSortingLayer MiscBossBags = new ItemSortingLayer("Misc - Boss Bags", (ItemSortingLayer layer, Item[] inv, List<int> itemsToSort) =>
		{
			List<int> indexesSortable = itemsToSort.Where((int i) => inv[i].type > 0 && ItemID.Sets.SortingPriorityMiscBossBags[inv[i].type] > -1).ToList();
			layer.Validate(ref indexesSortable, inv);
			foreach (int item46 in indexesSortable)
			{
				itemsToSort.Remove(item46);
			}
			SortIndicesStable(indexesSortable, (int x, int y) =>
			{
				int num = ItemID.Sets.SortingPriorityMiscBossBags[inv[x].type].CompareTo(ItemID.Sets.SortingPriorityMiscBossBags[inv[y].type]);
				if (num == 0)
				{
					num = inv[y].OriginalRarity.CompareTo(inv[x].OriginalRarity);
				}
				if (num == 0)
				{
					num = inv[y].stack.CompareTo(inv[x].stack);
				}
				return num;
			});
			return indexesSortable;
		});

		public static ItemSortingLayer MiscCritters = new ItemSortingLayer("Misc - Critters", (ItemSortingLayer layer, Item[] inv, List<int> itemsToSort) =>
		{
			List<int> indexesSortable = itemsToSort.Where((int i) => inv[i].makeNPC > 0).ToList();
			layer.Validate(ref indexesSortable, inv);
			foreach (int item47 in indexesSortable)
			{
				itemsToSort.Remove(item47);
			}
			SortIndicesStable(indexesSortable, (int x, int y) =>
			{
				int num = inv[x].makeNPC.CompareTo(inv[y].makeNPC);
				if (num == 0)
				{
					num = inv[y].stack.CompareTo(inv[x].stack);
				}
				return num;
			});
			return indexesSortable;
		});

		public static ItemSortingLayer LastMaterials = new ItemSortingLayer("Last - Materials", (ItemSortingLayer layer, Item[] inv, List<int> itemsToSort) =>
		{
			List<int> indexesSortable = itemsToSort.Where((int i) => inv[i].createTile < 0 && inv[i].createWall < 1 && inv[i].rare != -1).ToList();
			layer.Validate(ref indexesSortable, inv);
			foreach (int item48 in indexesSortable)
			{
				itemsToSort.Remove(item48);
			}
			SortIndicesStable(indexesSortable, (int x, int y) =>
			{
				int num = inv[y].OriginalRarity.CompareTo(inv[x].OriginalRarity);
				if (num == 0)
				{
					num = inv[y].value.CompareTo(inv[x].value);
				}
				if (num == 0)
				{
					num = inv[y].stack.CompareTo(inv[x].stack);
				}
				return num;
			});
			return indexesSortable;
		});

		public static ItemSortingLayer LastTilesImportant = new ItemSortingLayer("Last - Tiles (Frame Important)", (ItemSortingLayer layer, Item[] inv, List<int> itemsToSort) =>
		{
			List<int> indexesSortable = itemsToSort.Where((int i) => inv[i].createTile >= 0 && Main.tileFrameImportant[inv[i].createTile]).ToList();
			layer.Validate(ref indexesSortable, inv);
			foreach (int item49 in indexesSortable)
			{
				itemsToSort.Remove(item49);
			}
			SortIndicesStable(indexesSortable, (int x, int y) =>
			{
				int num = string.Compare(inv[x].Name, inv[y].Name, StringComparison.OrdinalIgnoreCase);
				if (num == 0)
				{
					num = inv[y].stack.CompareTo(inv[x].stack);
				}
				return num;
			});
			return indexesSortable;
		});

		public static ItemSortingLayer LastTilesCommon = new ItemSortingLayer("Last - Tiles (Common), Walls", (ItemSortingLayer layer, Item[] inv, List<int> itemsToSort) =>
		{
			List<int> indexesSortable = itemsToSort.Where((int i) => inv[i].createWall > 0 || inv[i].createTile >= 0).ToList();
			layer.Validate(ref indexesSortable, inv);
			foreach (int item50 in indexesSortable)
			{
				itemsToSort.Remove(item50);
			}
			SortIndicesStable(indexesSortable, (int x, int y) =>
			{
				int num = string.Compare(inv[x].Name, inv[y].Name, StringComparison.OrdinalIgnoreCase);
				if (num == 0)
				{
					num = inv[y].stack.CompareTo(inv[x].stack);
				}
				return num;
			});
			return indexesSortable;
		});

		public static ItemSortingLayer LastNotTrash = new ItemSortingLayer("Last - Not Trash", (ItemSortingLayer layer, Item[] inv, List<int> itemsToSort) =>
		{
			List<int> indexesSortable = itemsToSort.Where((int i) => inv[i].OriginalRarity >= 0).ToList();
			layer.Validate(ref indexesSortable, inv);
			foreach (int item51 in indexesSortable)
			{
				itemsToSort.Remove(item51);
			}
			SortIndicesStable(indexesSortable, (int x, int y) =>
			{
				int num = inv[y].OriginalRarity.CompareTo(inv[x].OriginalRarity);
				if (num == 0)
				{
					num = string.Compare(inv[x].Name, inv[y].Name, StringComparison.OrdinalIgnoreCase);
				}
				if (num == 0)
				{
					num = inv[y].stack.CompareTo(inv[x].stack);
				}
				return num;
			});
			return indexesSortable;
		});

		public static ItemSortingLayer LastTrash = new ItemSortingLayer("Last - Trash", (ItemSortingLayer layer, Item[] inv, List<int> itemsToSort) =>
		{
			List<int> indexesSortable = new List<int>(itemsToSort);
			layer.Validate(ref indexesSortable, inv);
			foreach (int item52 in indexesSortable)
			{
				itemsToSort.Remove(item52);
			}
			SortIndicesStable(indexesSortable, (int x, int y) =>
			{
				int num = inv[y].value.CompareTo(inv[x].value);
				if (num == 0)
				{
					num = inv[y].stack.CompareTo(inv[x].stack);
				}
				return num;
			});
			return indexesSortable;
		});

		private static void SortIndicesStable(List<int> list, Comparison<int> comparison)
		{
			list.Sort((int x, int y) =>
			{
				int num = comparison(x, y);
				if (num == 0)
				{
					num = x.CompareTo(y);
				}
				return num;
			});
		}

		public static int CompareWithPrioritySet(int[] prioritySet, int typeOne, int typeTwo)
		{
			if (typeOne < 0 || typeTwo < 0)
			{
				return 0;
			}
			if (prioritySet[typeOne] >= 0 && prioritySet[typeTwo] < 0)
			{
				return -1;
			}
			if (prioritySet[typeOne] < 0 && prioritySet[typeTwo] >= 0)
			{
				return 1;
			}
			if (prioritySet[typeOne] < 0 && prioritySet[typeTwo] < 0)
			{
				return 0;
			}
			return prioritySet[typeOne].CompareTo(prioritySet[typeTwo]);
		}
	}

	private struct DamageTypeSortingLayerEntry(float multiplier, ItemSortingLayer layer, int index)
	{
		public float Multiplier = multiplier;

		public ItemSortingLayer Layer = layer;

		public int Index = index;
	}

	private struct MemoryStamp
	{
		public int ItemType;

		public int Stack;

		public int Prefix;

		public MemoryStamp(int itemType, int stack, int prefix)
		{
			ItemType = itemType;
			Stack = stack;
			Prefix = prefix;
		}

		public MemoryStamp(Item item)
		{
			ItemType = item.type;
			Stack = item.stack;
			Prefix = item.prefix;
		}

		public override bool Equals(object obj)
		{
			if (obj == null || !(obj is MemoryStamp))
			{
				return false;
			}
			return Equals((MemoryStamp)obj);
		}

		public bool Equals(MemoryStamp other)
		{
			if (ItemType == other.ItemType && Stack == other.Stack)
			{
				return Prefix == other.Prefix;
			}
			return false;
		}

		public override int GetHashCode()
		{
			return (((ItemType * 397) ^ Stack) * 397) ^ Prefix;
		}

		public static bool operator ==(MemoryStamp left, MemoryStamp right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(MemoryStamp left, MemoryStamp right)
		{
			return !left.Equals(right);
		}
	}

	private static List<ItemSortingLayer> _layerList = new List<ItemSortingLayer>();

	private static Dictionary<string, List<int>> _layerWhiteLists = new Dictionary<string, List<int>>();

	private static int[] _layerIndexForItemType;

	private static int _layerCount;

	private static List<DamageTypeSortingLayerEntry> _damageRankings = new List<DamageTypeSortingLayerEntry>();

	private static readonly List<int> _sort_itemsToSort = new List<int>();

	private static readonly List<int> _sort_sortedItemIndexes = new List<int>();

	private static readonly List<int> _sort_counts = new List<int>();

	private static readonly List<Item> _sort_itemsCache = new List<Item>();

	private static readonly List<int> _sort_availableSortingSlots = new List<int>();

	private static MemoryStamp[] _sortInventory_preStamps = new MemoryStamp[0];

	private static MemoryStamp[] _sortInventory_postStamps = new MemoryStamp[0];

	private static readonly List<int> _fillAmmoFromInventory_acceptedAmmoTypes = new List<int>();

	private static readonly List<int> _fillAmmoFromInventory_emptyAmmoSlots = new List<int>();

	public static int LayerCount => _layerCount;

	public static void SetupWhiteLists()
	{
		_layerWhiteLists.Clear();
		List<ItemSortingLayer> list = new List<ItemSortingLayer>();
		List<Item> list2 = new List<Item>();
		List<int> list3 = new List<int>();
		list.Add(ItemSortingLayers.WeaponsMelee);
		list.Add(ItemSortingLayers.WeaponsRanged);
		list.Add(ItemSortingLayers.WeaponsMagic);
		list.Add(ItemSortingLayers.WeaponsMinions);
		list.Add(ItemSortingLayers.WeaponsAssorted);
		list.Add(ItemSortingLayers.WeaponsAmmo);
		list.Add(ItemSortingLayers.ToolsPicksaws);
		list.Add(ItemSortingLayers.ToolsHamaxes);
		list.Add(ItemSortingLayers.ToolsPickaxes);
		list.Add(ItemSortingLayers.ToolsAxes);
		list.Add(ItemSortingLayers.ToolsHammers);
		list.Add(ItemSortingLayers.ToolsTerraforming);
		list.Add(ItemSortingLayers.ToolsFishing);
		list.Add(ItemSortingLayers.ToolsGolf);
		list.Add(ItemSortingLayers.ToolsInstruments);
		list.Add(ItemSortingLayers.ToolsKeys);
		list.Add(ItemSortingLayers.ToolsKites);
		list.Add(ItemSortingLayers.ToolsAmmoLeftovers);
		list.Add(ItemSortingLayers.ToolsMisc);
		list.Add(ItemSortingLayers.ArmorCombat);
		list.Add(ItemSortingLayers.ArmorVanity);
		list.Add(ItemSortingLayers.ArmorAccessories);
		list.Add(ItemSortingLayers.EquipGrapple);
		list.Add(ItemSortingLayers.EquipMount);
		list.Add(ItemSortingLayers.EquipCart);
		list.Add(ItemSortingLayers.EquipLightPet);
		list.Add(ItemSortingLayers.EquipVanityPet);
		list.Add(ItemSortingLayers.PotionsDyes);
		list.Add(ItemSortingLayers.PotionsHairDyes);
		list.Add(ItemSortingLayers.PotionsLife);
		list.Add(ItemSortingLayers.PotionsJustTheMushroom);
		list.Add(ItemSortingLayers.PotionsMana);
		list.Add(ItemSortingLayers.PotionsElixirs);
		list.Add(ItemSortingLayers.PotionsBuffs);
		list.Add(ItemSortingLayers.PotionsFood);
		list.Add(ItemSortingLayers.MiscValuables);
		list.Add(ItemSortingLayers.MiscPainting);
		list.Add(ItemSortingLayers.MiscWiring);
		list.Add(ItemSortingLayers.MiscMaterials);
		list.Add(ItemSortingLayers.MiscJustTheGlowingMushroom);
		list.Add(ItemSortingLayers.MiscRopes);
		list.Add(ItemSortingLayers.MiscHerbsAndSeeds);
		list.Add(ItemSortingLayers.MiscAcorns);
		list.Add(ItemSortingLayers.MiscGems);
		list.Add(ItemSortingLayers.MiscBossBags);
		list.Add(ItemSortingLayers.MiscCritters);
		list.Add(ItemSortingLayers.MiscExtractinator);
		list.Add(ItemSortingLayers.LastMaterials);
		list.Add(ItemSortingLayers.LastTilesImportant);
		list.Add(ItemSortingLayers.LastTilesCommon);
		list.Add(ItemSortingLayers.LastNotTrash);
		list.Add(ItemSortingLayers.LastTrash);
		for (int i = -48; i < ItemID.Count; i++)
		{
			Item item = new Item();
			item.netDefaults(i);
			list2.Add(item);
			list3.Add(i + 48);
		}
		Item[] array = list2.ToArray();
		_layerCount = list.Count;
		_layerIndexForItemType = new int[ItemID.Count];
		for (int j = 0; j < list.Count; j++)
		{
			ItemSortingLayer itemSortingLayer = list[j];
			List<int> list4 = itemSortingLayer.SortingMethod(itemSortingLayer, array, list3);
			List<int> list5 = new List<int>();
			for (int k = 0; k < list4.Count; k++)
			{
				Item item2 = array[list4[k]];
				list5.Add(item2.type);
				_layerIndexForItemType[item2.type] = j;
			}
			_layerWhiteLists.Add(itemSortingLayer.Name, list5);
		}
		Invariant.Assert(!_layerWhiteLists[ItemSortingLayers.LastNotTrash.Name].Any(), "An item has been added which doesn't fit into any sorting layer");
	}

	private static void AddSortingPrioritiesBasedOnPlayerDamage(List<ItemSortingLayer> list)
	{
		Player player = Main.player[Main.myPlayer];
		_damageRankings.Clear();
		_damageRankings.Add(new DamageTypeSortingLayerEntry(player.meleeDamage, ItemSortingLayers.WeaponsMelee, 0));
		_damageRankings.Add(new DamageTypeSortingLayerEntry(player.rangedDamage, ItemSortingLayers.WeaponsRanged, 1));
		_damageRankings.Add(new DamageTypeSortingLayerEntry(player.magicDamage, ItemSortingLayers.WeaponsMagic, 2));
		_damageRankings.Add(new DamageTypeSortingLayerEntry(player.minionDamage, ItemSortingLayers.WeaponsMinions, 3));
		_damageRankings.Sort(Descending);
		foreach (DamageTypeSortingLayerEntry damageRanking in _damageRankings)
		{
			list.Add(damageRanking.Layer);
		}
	}

	private static int Descending(DamageTypeSortingLayerEntry x, DamageTypeSortingLayerEntry y)
	{
		int num = y.Multiplier.CompareTo(x.Multiplier);
		if (num == 0)
		{
			num = x.Index.CompareTo(y.Index);
		}
		return num;
	}

	private static void SetupSortingPriorities()
	{
		_ = Main.player[Main.myPlayer];
		_layerList.Clear();
		AddSortingPrioritiesBasedOnPlayerDamage(_layerList);
		_layerList.Add(ItemSortingLayers.WeaponsAssorted);
		_layerList.Add(ItemSortingLayers.WeaponsAmmo);
		_layerList.Add(ItemSortingLayers.ToolsPicksaws);
		_layerList.Add(ItemSortingLayers.ToolsHamaxes);
		_layerList.Add(ItemSortingLayers.ToolsPickaxes);
		_layerList.Add(ItemSortingLayers.ToolsAxes);
		_layerList.Add(ItemSortingLayers.ToolsHammers);
		_layerList.Add(ItemSortingLayers.ToolsTerraforming);
		_layerList.Add(ItemSortingLayers.ToolsFishing);
		_layerList.Add(ItemSortingLayers.ToolsGolf);
		_layerList.Add(ItemSortingLayers.ToolsInstruments);
		_layerList.Add(ItemSortingLayers.ToolsKeys);
		_layerList.Add(ItemSortingLayers.ToolsKites);
		_layerList.Add(ItemSortingLayers.ToolsAmmoLeftovers);
		_layerList.Add(ItemSortingLayers.ToolsMisc);
		_layerList.Add(ItemSortingLayers.ArmorCombat);
		_layerList.Add(ItemSortingLayers.ArmorVanity);
		_layerList.Add(ItemSortingLayers.ArmorAccessories);
		_layerList.Add(ItemSortingLayers.EquipGrapple);
		_layerList.Add(ItemSortingLayers.EquipMount);
		_layerList.Add(ItemSortingLayers.EquipCart);
		_layerList.Add(ItemSortingLayers.EquipLightPet);
		_layerList.Add(ItemSortingLayers.EquipVanityPet);
		_layerList.Add(ItemSortingLayers.PotionsDyes);
		_layerList.Add(ItemSortingLayers.PotionsHairDyes);
		_layerList.Add(ItemSortingLayers.PotionsLife);
		_layerList.Add(ItemSortingLayers.PotionsJustTheMushroom);
		_layerList.Add(ItemSortingLayers.PotionsMana);
		_layerList.Add(ItemSortingLayers.PotionsElixirs);
		_layerList.Add(ItemSortingLayers.PotionsBuffs);
		_layerList.Add(ItemSortingLayers.PotionsFood);
		_layerList.Add(ItemSortingLayers.MiscValuables);
		_layerList.Add(ItemSortingLayers.MiscPainting);
		_layerList.Add(ItemSortingLayers.MiscWiring);
		_layerList.Add(ItemSortingLayers.MiscMaterials);
		_layerList.Add(ItemSortingLayers.MiscJustTheGlowingMushroom);
		_layerList.Add(ItemSortingLayers.MiscRopes);
		_layerList.Add(ItemSortingLayers.MiscHerbsAndSeeds);
		_layerList.Add(ItemSortingLayers.MiscAcorns);
		_layerList.Add(ItemSortingLayers.MiscGems);
		_layerList.Add(ItemSortingLayers.MiscBossBags);
		_layerList.Add(ItemSortingLayers.MiscCritters);
		_layerList.Add(ItemSortingLayers.MiscExtractinator);
		_layerList.Add(ItemSortingLayers.LastMaterials);
		_layerList.Add(ItemSortingLayers.LastTilesImportant);
		_layerList.Add(ItemSortingLayers.LastTilesCommon);
		_layerList.Add(ItemSortingLayers.LastNotTrash);
		_layerList.Add(ItemSortingLayers.LastTrash);
	}

	private static void Sort(bool withFeedback, Item[] inv, params int[] ignoreSlots)
	{
		SetupSortingPriorities();
		_sort_itemsToSort.Clear();
		_sort_sortedItemIndexes.Clear();
		_sort_counts.Clear();
		_sort_itemsCache.Clear();
		_sort_availableSortingSlots.Clear();
		for (int i = 0; i < inv.Length; i++)
		{
			if (!ignoreSlots.Contains(i))
			{
				Item item = inv[i];
				if (item != null && item.stack != 0 && item.type != 0 && !item.favorited)
				{
					_sort_itemsToSort.Add(i);
				}
			}
		}
		for (int j = 0; j < _sort_itemsToSort.Count; j++)
		{
			Item item2 = inv[_sort_itemsToSort[j]];
			if (item2.stack >= item2.maxStack)
			{
				continue;
			}
			int num = item2.maxStack - item2.stack;
			for (int k = j; k < _sort_itemsToSort.Count; k++)
			{
				if (j == k)
				{
					continue;
				}
				Item item3 = inv[_sort_itemsToSort[k]];
				if (Item.CanStack(item2, item3) && item3.stack != item3.maxStack)
				{
					int num2 = item3.stack;
					if (num < num2)
					{
						num2 = num;
					}
					item2.stack += num2;
					item3.stack -= num2;
					num -= num2;
					if (item3.stack == 0)
					{
						inv[_sort_itemsToSort[k]] = new Item();
						_sort_itemsToSort.Remove(_sort_itemsToSort[k]);
						j--;
						k--;
						break;
					}
					if (num == 0)
					{
						break;
					}
				}
			}
		}
		_sort_availableSortingSlots.AddRange(_sort_itemsToSort);
		for (int l = 0; l < inv.Length; l++)
		{
			if (!ignoreSlots.Contains(l) && !_sort_availableSortingSlots.Contains(l))
			{
				Item item4 = inv[l];
				if (item4 == null || item4.stack == 0 || item4.type == 0)
				{
					_sort_availableSortingSlots.Add(l);
				}
			}
		}
		_sort_availableSortingSlots.Sort();
		foreach (ItemSortingLayer layer in _layerList)
		{
			List<int> list = layer.SortingMethod(layer, inv, _sort_itemsToSort);
			if (list.Count > 0)
			{
				_sort_counts.Add(list.Count);
			}
			_sort_sortedItemIndexes.AddRange(list);
		}
		_sort_sortedItemIndexes.AddRange(_sort_itemsToSort);
		foreach (int sort_sortedItemIndex in _sort_sortedItemIndexes)
		{
			_sort_itemsCache.Add(inv[sort_sortedItemIndex]);
			inv[sort_sortedItemIndex] = new Item();
		}
		float num3 = 1f / (float)_sort_counts.Count;
		float num4 = num3 / 2f;
		for (int m = 0; m < _sort_itemsCache.Count; m++)
		{
			int num5 = _sort_availableSortingSlots[0];
			if (withFeedback)
			{
				ItemSlot.SetGlow(num5, num4, Main.player[Main.myPlayer].chest != -1);
			}
			_sort_counts[0]--;
			if (_sort_counts[0] == 0)
			{
				_sort_counts.RemoveAt(0);
				num4 += num3;
			}
			inv[num5] = _sort_itemsCache[m];
			_sort_availableSortingSlots.Remove(num5);
		}
	}

	public static string GetSortingLayer(int itemType)
	{
		foreach (KeyValuePair<string, List<int>> layerWhiteList in _layerWhiteLists)
		{
			if (layerWhiteList.Value.Contains(itemType))
			{
				return layerWhiteList.Key;
			}
		}
		return null;
	}

	public static int GetSortingLayerIndex(int itemType)
	{
		return _layerIndexForItemType[itemType];
	}

	public static void SortInventory()
	{
		if (!Main.LocalPlayer.HasLockedInventory())
		{
			if (!Main.LocalPlayer.HasItem(905))
			{
				SortCoins();
			}
			SortAmmo();
			Sort(true, Main.player[Main.myPlayer].inventory, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 50, 51, 52, 53, 54, 55, 56, 57, 58);
		}
	}

	public static void SortChest()
	{
		int chest = Main.player[Main.myPlayer].chest;
		if (chest != -1)
		{
			_ = Main.player[Main.myPlayer].bank.item;
			if (chest == -3)
			{
				_ = Main.player[Main.myPlayer].bank2.item;
			}
			if (chest == -4)
			{
				_ = Main.player[Main.myPlayer].bank3.item;
			}
			if (chest == -5)
			{
				_ = Main.player[Main.myPlayer].bank4.item;
			}
			if (chest > -1)
			{
				_ = Main.chest[chest].item;
			}
			SortInventory(Main.LocalPlayer.GetCurrentContainer(), withSync: true, withFeedback: true);
		}
	}

	public static void SortInventory(Chest chest, bool withSync, bool withFeedback)
	{
		Item[] item = chest.item;
		Array.Resize(ref _sortInventory_preStamps, chest.maxItems);
		Array.Resize(ref _sortInventory_postStamps, chest.maxItems);
		for (int i = 0; i < chest.maxItems; i++)
		{
			_sortInventory_preStamps[i] = new MemoryStamp(item[i]);
		}
		Sort(withFeedback, item);
		for (int j = 0; j < chest.maxItems; j++)
		{
			_sortInventory_postStamps[j] = new MemoryStamp(item[j]);
		}
		if (!withSync || Main.netMode != 1 || Main.player[Main.myPlayer].chest <= -1)
		{
			return;
		}
		for (int k = 0; k < chest.maxItems; k++)
		{
			if (_sortInventory_postStamps[k] != _sortInventory_preStamps[k])
			{
				NetMessage.SendData(32, -1, -1, null, Main.player[Main.myPlayer].chest, k);
			}
		}
	}

	public static void SortAmmo()
	{
		ClearAmmoSlotSpaces();
		FillAmmoFromInventory();
	}

	public static void FillAmmoFromInventory()
	{
		_fillAmmoFromInventory_acceptedAmmoTypes.Clear();
		_fillAmmoFromInventory_emptyAmmoSlots.Clear();
		Item[] inventory = Main.player[Main.myPlayer].inventory;
		for (int i = 54; i < 58; i++)
		{
			ItemSlot.SetGlow(i, 0.31f, chest: false);
			Item item = inventory[i];
			if (item.IsAir)
			{
				_fillAmmoFromInventory_emptyAmmoSlots.Add(i);
			}
			else if (item.ammo != AmmoID.None)
			{
				if (!_fillAmmoFromInventory_acceptedAmmoTypes.Contains(item.type))
				{
					_fillAmmoFromInventory_acceptedAmmoTypes.Add(item.type);
				}
				RefillItemStack(inventory, inventory[i], 0, 50);
			}
		}
		if (_fillAmmoFromInventory_emptyAmmoSlots.Count < 1)
		{
			return;
		}
		for (int j = 0; j < 50; j++)
		{
			Item item2 = inventory[j];
			if (item2.stack >= 1 && item2.CanFillEmptyAmmoSlot() && _fillAmmoFromInventory_acceptedAmmoTypes.Contains(item2.type) && !item2.favorited)
			{
				int num = _fillAmmoFromInventory_emptyAmmoSlots[0];
				_fillAmmoFromInventory_emptyAmmoSlots.Remove(num);
				Utils.Swap(ref inventory[j], ref inventory[num]);
				RefillItemStack(inventory, inventory[num], 0, 50);
				if (_fillAmmoFromInventory_emptyAmmoSlots.Count == 0)
				{
					break;
				}
			}
		}
		if (_fillAmmoFromInventory_emptyAmmoSlots.Count < 1)
		{
			return;
		}
		for (int k = 0; k < 50; k++)
		{
			Item item3 = inventory[k];
			if (item3.stack >= 1 && item3.CanFillEmptyAmmoSlot() && item3.FitsAmmoSlot() && !item3.favorited)
			{
				int num2 = _fillAmmoFromInventory_emptyAmmoSlots[0];
				_fillAmmoFromInventory_emptyAmmoSlots.Remove(num2);
				Utils.Swap(ref inventory[k], ref inventory[num2]);
				RefillItemStack(inventory, inventory[num2], 0, 50);
				if (_fillAmmoFromInventory_emptyAmmoSlots.Count == 0)
				{
					break;
				}
			}
		}
	}

	public static void ClearAmmoSlotSpaces()
	{
		Item[] inventory = Main.player[Main.myPlayer].inventory;
		for (int i = 54; i < 58; i++)
		{
			Item item = inventory[i];
			if (!item.IsAir && item.ammo != AmmoID.None && item.stack < item.maxStack)
			{
				int loopStartIndex = (item.favorited ? 54 : (i + 1));
				RefillItemStack(inventory, item, loopStartIndex, 58);
			}
		}
		for (int j = 54; j < 58; j++)
		{
			if (inventory[j].type > 0 && !inventory[j].favorited)
			{
				TrySlidingUp(inventory, j, 54);
			}
		}
	}

	private static void SortCoins()
	{
		Item[] inventory = Main.LocalPlayer.inventory;
		long count = Utils.CoinsCount(out var overFlowing, inventory, 58);
		int commonMaxStack = Item.CommonMaxStack;
		if (overFlowing)
		{
			return;
		}
		int[] array = Utils.CoinsSplit(count);
		int num = 0;
		for (int i = 0; i < 3; i++)
		{
			int num2 = array[i];
			while (num2 > 0)
			{
				num2 -= 99;
				num++;
			}
		}
		int num3 = array[3];
		while (num3 > commonMaxStack)
		{
			num3 -= commonMaxStack;
			num++;
		}
		int num4 = 0;
		for (int j = 0; j < 58; j++)
		{
			if (inventory[j].type >= 71 && inventory[j].type <= 74 && inventory[j].stack > 0)
			{
				num4++;
			}
		}
		if (num4 < num)
		{
			return;
		}
		for (int k = 0; k < 58; k++)
		{
			if (inventory[k].type >= 71 && inventory[k].type <= 74 && inventory[k].stack > 0)
			{
				inventory[k].TurnToAir();
			}
		}
		int num5 = 100;
		while (true)
		{
			int num6 = -1;
			for (int num7 = 3; num7 >= 0; num7--)
			{
				if (array[num7] > 0)
				{
					num6 = num7;
					break;
				}
			}
			if (num6 == -1)
			{
				break;
			}
			int num8 = array[num6];
			if (num6 == 3 && num8 > commonMaxStack)
			{
				num8 = commonMaxStack;
			}
			bool flag = false;
			if (!flag)
			{
				for (int l = 50; l < 54; l++)
				{
					if (inventory[l].IsAir)
					{
						inventory[l].SetDefaults(71 + num6);
						inventory[l].stack = num8;
						array[num6] -= num8;
						flag = true;
						break;
					}
				}
			}
			if (!flag)
			{
				for (int m = 0; m < 50; m++)
				{
					if (inventory[m].IsAir)
					{
						inventory[m].SetDefaults(71 + num6);
						inventory[m].stack = num8;
						array[num6] -= num8;
						flag = true;
						break;
					}
				}
			}
			num5--;
			if (num5 > 0)
			{
				continue;
			}
			for (int num9 = 3; num9 >= 0; num9--)
			{
				if (array[num9] > 0)
				{
					Main.LocalPlayer.QuickSpawnItem(Main.LocalPlayer.GetItemSource_InventoryOverflow(), 71 + num9, array[num9]);
				}
			}
			break;
		}
	}

	private static void RefillItemStack(Item[] inv, Item itemToRefill, int loopStartIndex, int loopEndIndex)
	{
		int num = itemToRefill.maxStack - itemToRefill.stack;
		if (num <= 0)
		{
			return;
		}
		for (int i = loopStartIndex; i < loopEndIndex; i++)
		{
			Item item = inv[i];
			if (item.stack >= 1 && item.type == itemToRefill.type && !item.favorited)
			{
				int num2 = item.stack;
				if (num2 > num)
				{
					num2 = num;
				}
				num -= num2;
				itemToRefill.stack += num2;
				item.stack -= num2;
				if (item.stack <= 0)
				{
					item.TurnToAir();
				}
				if (num <= 0)
				{
					break;
				}
			}
		}
	}

	private static void TrySlidingUp(Item[] inv, int slot, int minimumIndex)
	{
		for (int i = minimumIndex; i < slot; i++)
		{
			if (inv[i].IsAir)
			{
				Utils.Swap(ref inv[i], ref inv[slot]);
				break;
			}
		}
	}
}
