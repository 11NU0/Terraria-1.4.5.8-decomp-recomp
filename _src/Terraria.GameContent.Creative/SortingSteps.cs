using System;
using System.Collections.Generic;
using Terraria.DataStructures;
using Terraria.ID;

namespace Terraria.GameContent.Creative;

public static class SortingSteps
{
	public abstract class ACreativeItemSortStep : ICreativeItemSortStep, IEntrySortStep<Item>, IComparer<Item>
	{
		public abstract string GetDisplayNameKey();

		public abstract int Compare(Item x, Item y);
	}

	public abstract class AStepByFittingFilter : ACreativeItemSortStep
	{
		public override int Compare(Item x, Item y)
		{
			int num = FitsFilter(x).CompareTo(FitsFilter(y));
			if (num == 0)
			{
				num = 1;
			}
			return num;
		}

		public abstract bool FitsFilter(Item item);

		public virtual int CompareWhenBothFit(Item x, Item y)
		{
			return string.Compare(x.Name, y.Name, StringComparison.OrdinalIgnoreCase);
		}
	}

	public class Blocks : AStepByFittingFilter
	{
		public override string GetDisplayNameKey()
		{
			return "CreativePowers.Sort_Blocks";
		}

		public override bool FitsFilter(Item item)
		{
			if (item.createTile >= 0)
			{
				return !Main.tileFrameImportant[item.createTile];
			}
			return false;
		}
	}

	public class Walls : AStepByFittingFilter
	{
		public override string GetDisplayNameKey()
		{
			return "CreativePowers.Sort_Walls";
		}

		public override bool FitsFilter(Item item)
		{
			return item.createWall >= 0;
		}
	}

	public class PlaceableObjects : AStepByFittingFilter
	{
		public override string GetDisplayNameKey()
		{
			return "CreativePowers.Sort_PlaceableObjects";
		}

		public override bool FitsFilter(Item item)
		{
			if (item.createTile >= 0)
			{
				return Main.tileFrameImportant[item.createTile];
			}
			return false;
		}
	}

	public class ByUnlockStatus : ACreativeItemSortStep
	{
		public override string GetDisplayNameKey()
		{
			return "CreativePowers.Sort_UnlockedFirst";
		}

		public override int Compare(Item x, Item y)
		{
			ItemsSacrificedUnlocksTracker itemSacrifices = Main.LocalPlayerCreativeTracker.ItemSacrifices;
			bool flag = itemSacrifices.IsNewlyResearched(x.type);
			bool flag2 = itemSacrifices.IsNewlyResearched(y.type);
			if (flag != flag2)
			{
				if (!flag)
				{
					return 1;
				}
				return -1;
			}
			bool flag3 = itemSacrifices.IsFullyResearched(x.type);
			bool flag4 = itemSacrifices.IsFullyResearched(y.type);
			if (flag3 != flag4)
			{
				if (!flag3)
				{
					return 1;
				}
				return -1;
			}
			return 0;
		}
	}

	public class ByCreativeSortingId : ACreativeItemSortStep
	{
		public override string GetDisplayNameKey()
		{
			return "CreativePowers.Sort_SortingID";
		}

		public override int Compare(Item x, Item y)
		{
			ContentSamples.CreativeHelper.ItemGroupAndOrderInGroup itemGroupAndOrderInGroup = ContentSamples.ItemCreativeSortingId[x.type];
			ContentSamples.CreativeHelper.ItemGroupAndOrderInGroup itemGroupAndOrderInGroup2 = ContentSamples.ItemCreativeSortingId[y.type];
			int num = itemGroupAndOrderInGroup.Group.CompareTo(itemGroupAndOrderInGroup2.Group);
			if (num == 0)
			{
				num = itemGroupAndOrderInGroup.OrderInGroup.CompareTo(itemGroupAndOrderInGroup2.OrderInGroup);
			}
			return num;
		}
	}

	public class Alphabetical : ACreativeItemSortStep
	{
		public override string GetDisplayNameKey()
		{
			return "CreativePowers.Sort_Alphabetical";
		}

		public override int Compare(Item x, Item y)
		{
			string name = x.Name;
			string name2 = y.Name;
			return name.CompareTo(name2);
		}
	}
}
