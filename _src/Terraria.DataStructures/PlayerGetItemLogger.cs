using System.Collections.Generic;

namespace Terraria.DataStructures;

public class PlayerGetItemLogger
{
	public struct GetItemLoggerEntry
	{
		public Item[] TargetArray;

		public int TargetSlot;

		public int TargetItemSlotContext;

		public int Stack;
	}

	public List<GetItemLoggerEntry> Entries = new List<GetItemLoggerEntry>();

	private bool _enabled;

	public void Clear()
	{
		Entries.Clear();
	}

	public void Add(Item[] array, int slot, int itemSlotContext, int stack)
	{
		if (_enabled)
		{
			Entries.Add(new GetItemLoggerEntry
			{
				TargetArray = array,
				TargetSlot = slot,
				TargetItemSlotContext = itemSlotContext,
				Stack = stack
			});
		}
	}

	public void Start()
	{
		Clear();
		_enabled = true;
	}

	public void Stop()
	{
		_enabled = false;
	}
}
