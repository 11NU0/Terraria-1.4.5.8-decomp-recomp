using System;

namespace Terraria.Testing.Cloning;

public class ChestCloneHelper : BackupRestoreHelper<Chest>
{
	private static readonly Item _dummy = new Item();

	public override Chest TryQuickBackup(Chest src, DeepCloneContext ctx)
	{
		Chest chest = DeepCloning.ShallowClone(src);
		if (chest.item != null)
		{
			Item[] array = (chest.item = DeepCloning.ShallowClone(chest.item));
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = Backup(array[i]);
			}
		}
		return chest;
	}

	private Item Backup(Item item)
	{
		if (!item.IsAir)
		{
			return DeepCloning.Clone(item);
		}
		return _dummy;
	}

	public override Chest TryQuickRestore(Chest backup, Chest current, DeepCloneContext ctx)
	{
		Chest chest = DeepCloning.ShallowClone(backup);
		Item[] item = backup.item;
		if (item != null)
		{
			Array.Resize(ref current.item, item.Length);
			Item[] array = (chest.item = current.item);
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = Restore(item[i], array[i]);
			}
		}
		return chest;
	}

	private Item Restore(Item backup, Item current)
	{
		if (current == null)
		{
			return DeepCloning.Clone(backup);
		}
		if (!current.IsNetStateDifferent(backup))
		{
			return current;
		}
		if (backup == _dummy)
		{
			current.TurnToAir();
		}
		else
		{
			current.SetDefaults(backup.type, backup.Variant);
			current.stack = backup.stack;
			current.Prefix(backup.prefix);
			current.favorited = backup.favorited;
		}
		return current;
	}
}
