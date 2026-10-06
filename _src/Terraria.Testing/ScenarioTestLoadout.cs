using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Utilities;

namespace Terraria.Testing;

public class ScenarioTestLoadout
{
	public class ItemRecord
	{
		public int Index;

		public string Type;

		public int Prefix;

		public int Stack;
	}

	public class BuffRecord
	{
		public int Index;

		public string Type;

		public int Time;
	}

	public class MinionRecord
	{
		public int ItemType;

		public int ItemPrefix;
	}

	public bool DayTime;

	public int TimeOfDay;

	public int SelectedItem;

	public int Life;

	public int LifeMax;

	public int Mana;

	public int ManaMax;

	public List<MinionRecord> Minions = new List<MinionRecord>();

	public List<ItemRecord> Items_Inventory = new List<ItemRecord>();

	public List<ItemRecord> Items_Ammo = new List<ItemRecord>();

	public List<ItemRecord> Items_Armor = new List<ItemRecord>();

	public List<ItemRecord> Items_Equipment = new List<ItemRecord>();

	public List<BuffRecord> Buffs = new List<BuffRecord>();

	private static readonly string LoadoutsPath = Path.Combine(Main.SavePath, "Loadouts");

	private static readonly JsonSerializerSettings SerializerSettings = new JsonSerializerSettings
	{
		ContractResolver = (IContractResolver)(object)new EasyDeserializationJsonContractResolver(),
		TypeNameHandling = (TypeNameHandling)4,
		DefaultValueHandling = (DefaultValueHandling)1
	};

	public void Prepare(Player player)
	{
		DayTime = Main.dayTime;
		TimeOfDay = (int)Main.time;
		SelectedItem = player.selectedItem;
		Life = player.statLife;
		LifeMax = player.statLifeMax;
		Mana = player.statMana;
		ManaMax = player.statManaMax;
		Fill(Minions);
		Fill(player.inventory, Items_Inventory, 0, 10);
		Fill(player.inventory, Items_Inventory, 54, 58);
		Fill(player.armor, Items_Armor, 0, player.armor.Length);
		Fill(player.miscEquips, Items_Equipment, 0, player.miscEquips.Length);
		Fill(player, Buffs);
	}

	public void Apply(Player player)
	{
		if (Main.netMode != 1)
		{
			Main.dayTime = DayTime;
			Main.time = TimeOfDay;
		}
		player.statLife = Life;
		player.statLifeMax = LifeMax;
		player.statMana = Mana;
		player.statManaMax = ManaMax;
		player.selectedItemState.Select(SelectedItem);
		Apply(player.inventory, Items_Inventory);
		Apply(player.inventory, Items_Ammo);
		Apply(player.armor, Items_Armor);
		Apply(player.miscEquips, Items_Equipment);
		Apply(player, Minions);
		Apply(player, Buffs);
	}

	public void Fill(List<MinionRecord> records)
	{
		for (int i = 0; i < 1000; i++)
		{
			Projectile projectile = Main.projectile[i];
			if (projectile.active && projectile.owner == Main.myPlayer && projectile.MinionSpawnInfo != null && projectile.MinionSpawnInfo is MinionSpawnFromInventoryItem minionSpawnFromInventoryItem)
			{
				records.Add(new MinionRecord
				{
					ItemType = minionSpawnFromInventoryItem.ItemType,
					ItemPrefix = minionSpawnFromInventoryItem.ItemPrefix
				});
			}
		}
	}

	public void Apply(Player player, List<MinionRecord> records)
	{
		Projectile[] projectile = Main.projectile;
		foreach (Projectile projectile2 in projectile)
		{
			if (projectile2.active && projectile2.owner == Main.myPlayer && projectile2.minionSlots > 0f)
			{
				projectile2.Kill();
			}
		}
		MinionRespawner minionRespawner = new MinionRespawner();
		minionRespawner.Replace(records.Select((MinionRecord x) => new MinionSpawnFromInventoryItem(x.ItemType, x.ItemPrefix)));
		minionRespawner.RestoreMinionsFor(player);
	}

	public void Fill(Player player, List<BuffRecord> records)
	{
		for (int i = 0; i < Player.maxBuffs; i++)
		{
			if (player.buffType[i] != 0)
			{
				records.Add(new BuffRecord
				{
					Index = i,
					Type = BuffID.Search.GetName(player.buffType[i]),
					Time = player.buffTime[i]
				});
			}
		}
	}

	public void Apply(Player player, List<BuffRecord> records)
	{
		foreach (BuffRecord record in records)
		{
			player.buffType[record.Index] = BuffID.Search.GetId(record.Type);
			player.buffTime[record.Index] = record.Time;
		}
	}

	public void Fill(Item[] items, List<ItemRecord> records, int rangeStart, int rangeEndExclusive)
	{
		for (int i = rangeStart; i < rangeEndExclusive; i++)
		{
			Item item = items[i];
			records.Add(new ItemRecord
			{
				Index = i,
				Type = ItemID.Search.GetName(item.type),
				Prefix = item.prefix,
				Stack = item.stack
			});
		}
	}

	public void Apply(Item[] items, List<ItemRecord> records)
	{
		int type = default;
		foreach (ItemRecord record in records)
		{
			if (ItemID.Search.TryGetId(record.Type, out type))
			{
				Item item = new Item(type);
				item.Prefix(record.Prefix);
				item.stack = record.Stack;
				items[record.Index] = item;
			}
		}
	}

	public static ScenarioTestLoadout Deserialize(string json)
	{
		return JsonConvert.DeserializeObject<ScenarioTestLoadout>(json, SerializerSettings);
	}

	public static string Serialize(ScenarioTestLoadout loadout)
	{
		return JsonConvert.SerializeObject((object)loadout, typeof(ScenarioTestLoadout), (Formatting)1, SerializerSettings);
	}

	public static bool TryReadFromFriendlyName(out ScenarioTestLoadout config, string name)
	{
		config = null;
		string path = ToFilePath(name);
		try
		{
			if (!File.Exists(path))
			{
				return false;
			}
			config = Deserialize(File.ReadAllText(path));
			return true;
		}
		catch (Exception)
		{
			return false;
		}
	}

	public static bool TryReadFromFilePath(out ScenarioTestLoadout config, string FilePath)
	{
		config = null;
		try
		{
			if (!File.Exists(FilePath))
			{
				return false;
			}
			config = Deserialize(File.ReadAllText(FilePath));
			return true;
		}
		catch (Exception)
		{
			return false;
		}
	}

	public static void Set(ScenarioTestLoadout config, string name)
	{
		Directory.CreateDirectory(LoadoutsPath);
		File.WriteAllText(ToFilePath(name), Serialize(config));
	}

	private static string ToFilePath(string name)
	{
		return Path.Combine(LoadoutsPath, name + ".json");
	}

	public static void Clear(string name)
	{
		string path = ToFilePath(name);
		if (File.Exists(path))
		{
			File.Delete(path);
		}
	}

	public static string[] FindAll()
	{
		if (!Directory.Exists(LoadoutsPath))
		{
			return new string[0];
		}
		return Directory.GetFiles(LoadoutsPath, "*json");
	}

	static ScenarioTestLoadout()
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected Obj, but got Unknown
	}
}
