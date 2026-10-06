using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.Testing;

namespace Terraria.GameContent;

public static class EmergencyStacking
{
	public class Group
	{
		public static readonly int DefaultStackDistanceStepSize = 160;

		public int DistanceStepSize = DefaultStackDistanceStepSize;

		private List<Predicate<Item>> Conditions = new List<Predicate<Item>>();

		internal int StackingPriority;

		public static Group FallenStars = new Group(75)
		{
			DistanceStepSize = DefaultStackDistanceStepSize * 4
		};

		public static Group CopperCoins = new Group(71);

		public static Group SilverCoins = new Group(72);

		public static Group Equipment = new Group((Item item) => item.OnlyNeedOneInInventory());

		public static Group RareCurrency = new Group
		{
			DistanceStepSize = DefaultStackDistanceStepSize / 4
		}.Add(73).Add(74).Add(3822);

		public static Group Default = new Group((Item item) => true);

		public Group()
		{
		}

		public Group(int type)
		{
			Add(type);
		}

		public Group(Predicate<Item> condition)
		{
			Add(condition);
		}

		public Group Add(Predicate<Item> condition)
		{
			Conditions.Add(condition);
			return this;
		}

		public Group Add(int type)
		{
			return Add((Item item) => item.type == type);
		}

		public bool Contains(Item item)
		{
			return Conditions.Any((Predicate<Item> p) => p(item));
		}
	}

	private struct StackableItem
	{
		public int type;

		public int age;

		public bool isOnScreen;

		public WorldItem item;

		public bool IsPreferredDestination(StackableItem other)
		{
			if (isOnScreen != other.isOnScreen)
			{
				return isOnScreen;
			}
			if (age != other.age)
			{
				return age < other.age;
			}
			return item.whoAmI < other.item.whoAmI;
		}
	}

	private struct Transfer : IComparable<Transfer>
	{
		public WorldItem src;

		public WorldItem dst;

		public int distanceOrder;

		public int preservationOrder;

		public int distance;

		public bool HasOwnership
		{
			get
			{
				if (src.playerIndexTheItemIsReservedFor == Main.myPlayer)
				{
					return dst.playerIndexTheItemIsReservedFor == Main.myPlayer;
				}
				return false;
			}
		}

		public int NumToTransfer
		{
			get
			{
				if (!Item.CanStack(src.inner, dst.inner))
				{
					return 0;
				}
				return Math.Min(src.stack, dst.maxStack - dst.stack);
			}
		}

		public int CompareTo(Transfer other)
		{
			int num = 0;
			if (num == 0)
			{
				num = distanceOrder.CompareTo(other.distanceOrder);
			}
			if (num == 0)
			{
				num = preservationOrder.CompareTo(other.preservationOrder);
			}
			if (num == 0)
			{
				num = distance.CompareTo(other.distance);
			}
			return num;
		}

		public override string ToString()
		{
			return $"({distanceOrder},{preservationOrder},{distance}) {src} -> {dst}";
		}
	}

	public static readonly List<Group> PreservationOrder = new List<Group>
	{
		Group.RareCurrency,
		Group.Equipment,
		Group.SilverCoins,
		Group.CopperCoins,
		Group.FallenStars,
		Group.Default
	};

	private static readonly Point PlayerViewRectSize = new Point(2320, 1600);

	private static readonly int ItemsToStackEachTime = 20;

	private static readonly int MaxTransferDistance = 2400;

	private static readonly int OnScreenDistancePriorityPenalty = 3;

	private static Group[] _groupLookup;

	private static readonly List<Transfer> PendingTransfers = new List<Transfer>(ItemsToStackEachTime);

	private static readonly StackableItem[] stackableItemsScratch = new StackableItem[400];

	private static readonly List<Rectangle> playerViewRectsScratch = new List<Rectangle>(255);

	private static Group[] GroupLookup
	{
		get
		{
			if (_groupLookup != null)
			{
				return _groupLookup;
			}
			int count = PreservationOrder.Count;
			foreach (Group item in PreservationOrder)
			{
				item.StackingPriority = count--;
			}
			_groupLookup = (from t in Enumerable.Range(0, ItemID.Count)
				select PreservationOrder.First((Group g) => g.Contains(ContentSamples.ItemsByType[t]))).ToArray();
			return _groupLookup;
		}
	}

	public static bool HasPendingTransferInvolving(WorldItem item)
	{
		if (PendingTransfers.Count == 0)
		{
			return false;
		}
		Invariant.Assert(Main.item[item.whoAmI] == item, "WorldItem whoAmI desync");
		return PendingTransfers.Any((Transfer t) => t.src == item || t.dst == item);
	}

	public static void ClearPendingTransfersInvolving(WorldItem item)
	{
		if (PendingTransfers.Count != 0)
		{
			Invariant.Assert(Main.item[item.whoAmI] == item, "WorldItem index desync");
			PendingTransfers.RemoveAll((Transfer t) => t.src == item || t.dst == item);
		}
	}

	public static bool EmergencyStackItemsToMakeSpace(out int freeSlot)
	{
		int limit = Math.Max(ItemsToStackEachTime, PendingTransfers.Count + 1);
		Invariant.Assert(PendingTransfers.Count == 0 || Main.netMode == 2, "PendingTransfers only allowed on server");
		PendingTransfers.Clear();
		FindBestTransfers(MemoStackableItems(), PendingTransfers, limit);
		ProcessPendingTransfers(out freeSlot);
		RequestOwnershipReleaseForPendingTransfers();
		return freeSlot < 400;
	}

	public static void ProcessPendingTransfers()
	{
		if (PendingTransfers.Count != 0)
		{
			ProcessPendingTransfers(out var _);
		}
	}

	private static void ProcessPendingTransfers(out int freeSlot)
	{
		freeSlot = 400;
		for (int i = 0; i < PendingTransfers.Count; i++)
		{
			UpdateDestinationFromPreviousTransfers(PendingTransfers, i);
			Transfer t = PendingTransfers[i];
			DoTransfer(t);
			if (t.src.IsAir)
			{
				freeSlot = Math.Min(freeSlot, t.src.whoAmI);
			}
		}
		if (Main.netMode != 2)
		{
			PendingTransfers.Clear();
			return;
		}
		PendingTransfers.RemoveAll((Transfer transfer) => transfer.NumToTransfer == 0);
	}

	private static void UpdateDestinationFromPreviousTransfers(List<Transfer> transfers, int i)
	{
		Transfer value = transfers[i];
		WorldItem dst = value.dst;
		int num = i - 1;
		while (dst.IsAir && num >= 0)
		{
			if (transfers[num].src == dst)
			{
				dst = transfers[num].dst;
			}
			num--;
		}
		if (dst != value.dst)
		{
			value.dst = dst;
			transfers[i] = value;
		}
	}

	private static StackableItem[] MemoStackableItems()
	{
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		List<Rectangle> playerViewRects = GetPlayerViewRects();
		StackableItem[] array = stackableItemsScratch;
		Array.Clear(array, 0, array.Length);
		for (int i = 0; i < 400; i++)
		{
			WorldItem worldItem = Main.item[i];
			Invariant.Assert(worldItem.whoAmI == i, "WorldItem whoAmI desync");
			if (!worldItem.IsAir && worldItem.stack < worldItem.maxStack && worldItem.shimmerTime == 0f && Main.timeItemSlotCannotBeReusedFor[i] == 0)
			{
				array[i] = new StackableItem
				{
					type = worldItem.type,
					age = worldItem.timeSinceItemSpawned,
					isOnScreen = AnyContains(playerViewRects, worldItem.Center.ToPoint()),
					item = worldItem
				};
			}
		}
		return array;
	}

	private static List<Rectangle> GetPlayerViewRects()
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		List<Rectangle> list = playerViewRectsScratch;
		list.Clear();
		for (int i = 0; i < 255; i++)
		{
			Player player = Main.player[i];
			if (player.active)
			{
				list.Add(Utils.CenteredRectangle(player.Center.ToPoint(), PlayerViewRectSize));
			}
		}
		return list;
	}

	private static void FindBestTransfers(StackableItem[] stackableItems, List<Transfer> transfers, int limit)
	{
		for (int i = 0; i < stackableItems.Length; i++)
		{
			StackableItem stackableItem = stackableItems[i];
			if (stackableItem.type == 0)
			{
				continue;
			}
			Transfer transfer = new Transfer
			{
				distanceOrder = int.MaxValue
			};
			for (int j = 0; j < stackableItems.Length; j++)
			{
				StackableItem other = stackableItems[j];
				if (stackableItem.type != other.type || stackableItem.item == other.item || stackableItem.IsPreferredDestination(other) || !Item.CanStack(stackableItem.item.inner, other.item.inner))
				{
					continue;
				}
				int num = DistanceBetween(stackableItem.item, other.item);
				if (num <= MaxTransferDistance)
				{
					Group obj = GroupLookup[stackableItem.type];
					Transfer transfer2 = new Transfer
					{
						src = stackableItem.item,
						dst = other.item,
						distanceOrder = num / obj.DistanceStepSize,
						preservationOrder = obj.StackingPriority,
						distance = num
					};
					if (stackableItem.isOnScreen)
					{
						transfer2.distanceOrder += OnScreenDistancePriorityPenalty;
					}
					if (transfer2.CompareTo(transfer) < 0)
					{
						transfer = transfer2;
					}
				}
			}
			if (transfer.src != null)
			{
				AddToOrderedList(transfers, limit, transfer);
			}
		}
	}

	private static void DoTransfer(Transfer t)
	{
		WorldItem src = t.src;
		WorldItem dst = t.dst;
		Invariant.Assert(Main.item[src.whoAmI] == src && Main.item[dst.whoAmI] == dst, "WorldItem index desync");
		if (!t.HasOwnership)
		{
			return;
		}
		int numToTransfer = t.NumToTransfer;
		if (numToTransfer != 0)
		{
			src.stack -= numToTransfer;
			dst.stack += numToTransfer;
			if (src.stack <= 0)
			{
				src.TurnToAir();
			}
			if (dst.stack == dst.maxStack)
			{
				OnReachingMaxStack(dst);
			}
			if (Main.netMode != 0)
			{
				dst.SyncItem();
				src.SyncItem();
			}
		}
	}

	private static void RequestOwnershipReleaseForPendingTransfers()
	{
		foreach (Transfer pendingTransfer in PendingTransfers)
		{
			pendingTransfer.src.FindOwner(forceAssignToServer: true);
			pendingTransfer.dst.FindOwner(forceAssignToServer: true);
		}
	}

	private static void AddToOrderedList(List<Transfer> list, int limit, Transfer item)
	{
		int i;
		for (i = 0; i < list.Count && item.CompareTo(list[i]) >= 0; i++)
		{
		}
		if (i != limit)
		{
			if (list.Count == limit)
			{
				list.RemoveAt(list.Count - 1);
			}
			list.Insert(i, item);
		}
	}

	private static bool AnyContains(List<Rectangle> rects, Point point)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		foreach (Rectangle rect in rects)
		{
			Rectangle current = rect;
			if (current.Contains(point))
			{
				return true;
			}
		}
		return false;
	}

	private static int DistanceBetween(WorldItem a, WorldItem b)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		Vector2 val = a.position - b.position;
		return Math.Abs((int)val.X) + Math.Abs((int)val.Y);
	}

	private static void OnReachingMaxStack(WorldItem item)
	{
		switch (item.type)
		{
		case 71:
			item.inner.SetDefaults(72);
			break;
		case 72:
			item.inner.SetDefaults(73);
			break;
		case 73:
			item.inner.SetDefaults(74);
			break;
		}
	}

	static EmergencyStacking()
	{
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
	}
}
