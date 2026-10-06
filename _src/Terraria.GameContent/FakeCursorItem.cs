namespace Terraria.GameContent;

public static class FakeCursorItem
{
	private static int _type;

	private static int _stack;

	private static int _prefix;

	private static Item _item = new Item();

	public static Item Item
	{
		get
		{
			int num = ((!Main.mouseItem.IsAir) ? Main.mouseItem.stack : 0);
			if (_type != _item.type)
			{
				_item.SetDefaults(_type);
			}
			else
			{
				_item.Refresh();
			}
			if (_prefix != _item.prefix)
			{
				_item.Prefix(_prefix);
			}
			_item.stack = _stack + num;
			return _item;
		}
	}

	public static void Reset()
	{
		_type = 0;
		_stack = 0;
	}

	public static void Add(int itemType, int itemStack, int itemPrefix = 0)
	{
		if (itemStack != 0)
		{
			if (_type == itemType)
			{
				_stack += itemStack;
			}
			else
			{
				_stack = itemStack;
			}
			_type = itemType;
			_prefix = itemPrefix;
		}
	}

	public static void Add(Item item)
	{
		Add(item.type, item.stack, item.prefix);
	}

	public static void Remove(int itemType, int itemStack)
	{
		if (itemStack != 0 && _type == itemType)
		{
			_stack -= itemStack;
			if (_stack <= 0)
			{
				_type = 0;
			}
		}
	}
}
