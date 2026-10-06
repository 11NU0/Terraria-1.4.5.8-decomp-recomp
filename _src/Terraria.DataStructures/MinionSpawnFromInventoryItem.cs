namespace Terraria.DataStructures;

public class MinionSpawnFromInventoryItem : MinionSpawnInfo
{
	public int ItemType;

	public int ItemPrefix;

	public MinionSpawnFromInventoryItem(Item item)
	{
		ItemType = item.type;
		ItemPrefix = item.prefix;
	}

	public MinionSpawnFromInventoryItem(int itemType, int itemPrefix)
	{
		ItemType = itemType;
		ItemPrefix = itemPrefix;
	}

	protected virtual bool ItemMatches(Item item)
	{
		if (item.type == ItemType)
		{
			return item.prefix == ItemPrefix;
		}
		return false;
	}

	public override void TryRespawn(Player player)
	{
		Item item = FindMatchingItem(player);
		if (item == null)
		{
			return;
		}
		if (item.buffType > 0)
		{
			int num = item.buffTime;
			if (num == 0)
			{
				num = 3600;
			}
			player.AddBuff(item.buffType, num);
		}
		player.SilentlyShootItem(item);
	}

	protected Item FindMatchingItem(Player player)
	{
		Item[] inventory = player.inventory;
		for (int i = 0; i < 50; i++)
		{
			Item item = inventory[i];
			if (ItemMatches(item))
			{
				return item;
			}
		}
		return null;
	}
}
