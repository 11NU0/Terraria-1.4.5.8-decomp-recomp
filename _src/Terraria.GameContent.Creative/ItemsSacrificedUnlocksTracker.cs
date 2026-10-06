using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using Terraria.ID;

namespace Terraria.GameContent.Creative;

public class ItemsSacrificedUnlocksTracker : IPersistentPerWorldContent, IOnPlayerJoining
{
	public const int POSITIVE_SACRIFICE_COUNT_CAP = 9999;

	private ConcurrentDictionary<string, int> _sacrificeCountByItemPersistentId;

	private Dictionary<int, int> _sacrificesCountByItemIdCache;

	private Dictionary<int, string> _unlockedByTeammate;

	private HashSet<int> _newlyUnlocked;

	public bool AnyNewUnlocksFromTeammates;

	public int LastEditId { get; private set; }

	public void DismissNewlyUnlockedFromTeamMatesIcon()
	{
		AnyNewUnlocksFromTeammates = false;
	}

	public ItemsSacrificedUnlocksTracker()
	{
		_sacrificeCountByItemPersistentId = new ConcurrentDictionary<string, int>();
		_sacrificesCountByItemIdCache = new Dictionary<int, int>();
		_unlockedByTeammate = new Dictionary<int, string>();
		_newlyUnlocked = new HashSet<int>();
		LastEditId = 0;
	}

	public int GetSacrificeCount(int itemId)
	{
		if (ContentSamples.CreativeResearchItemPersistentIdOverride.TryGetValue(itemId, out var value))
		{
			itemId = value;
		}
		_sacrificesCountByItemIdCache.TryGetValue(itemId, out var value2);
		return value2;
	}

	public void ForEachItemWithResearchProgress(Action<int> action)
	{
		foreach (KeyValuePair<int, int> item in _sacrificesCountByItemIdCache)
		{
			if (item.Value > 0)
			{
				action(item.Key);
			}
		}
	}

	public void CountFullyResearchedItems(out int fullyResearchedItems, out int allItems)
	{
		fullyResearchedItems = 0;
		allItems = 0;
		for (int i = 0; i < ItemID.Count; i++)
		{
			if (TryGetSacrificeNumbers(i, out var amountWeHave, out var amountNeededTotal))
			{
				allItems++;
				if (amountWeHave >= amountNeededTotal)
				{
					fullyResearchedItems++;
				}
			}
		}
	}

	public bool TryGetSacrificeNumbers(int itemId, out int amountWeHave, out int amountNeededTotal)
	{
		if (ContentSamples.CreativeResearchItemPersistentIdOverride.TryGetValue(itemId, out var value))
		{
			itemId = value;
		}
		amountWeHave = (amountNeededTotal = 0);
		if (!CreativeItemSacrificesCatalog.Instance.TryGetSacrificeCountCapToUnlockInfiniteItems(itemId, out amountNeededTotal))
		{
			return false;
		}
		_sacrificesCountByItemIdCache.TryGetValue(itemId, out amountWeHave);
		return true;
	}

	public bool IsFullyResearched(int itemId)
	{
		if (TryGetSacrificeNumbers(itemId, out var amountWeHave, out var amountNeededTotal))
		{
			return amountWeHave >= amountNeededTotal;
		}
		return false;
	}

	public bool IsNewlyResearched(int itemId)
	{
		return _newlyUnlocked.Contains(itemId);
	}

	public void ClearNewlyResearchedStatus(int itemId)
	{
		_newlyUnlocked.Remove(itemId);
	}

	public bool TryGetTeammateUnlockCredit(int itemId, out string teammateName)
	{
		return _unlockedByTeammate.TryGetValue(itemId, out teammateName);
	}

	public void RegisterItemSacrifice(int itemId, int amount, string teammateName = null)
	{
		if (ContentSamples.CreativeResearchItemPersistentIdOverride.TryGetValue(itemId, out var value))
		{
			itemId = value;
		}
		if (!ContentSamples.ItemPersistentIdsByNetIds.TryGetValue(itemId, out var value2) || !CreativeItemSacrificesCatalog.Instance.TryGetSacrificeCountCapToUnlockInfiniteItems(itemId, out var amountNeeded))
		{
			return;
		}
		_sacrificeCountByItemPersistentId.TryGetValue(value2, out var value3);
		if (value3 >= amountNeeded)
		{
			return;
		}
		value3 = Math.Min(value3 + amount, amountNeeded);
		_sacrificeCountByItemPersistentId[value2] = value3;
		_sacrificesCountByItemIdCache[itemId] = value3;
		MarkContentsDirty();
		if (value3 >= amountNeeded)
		{
			_newlyUnlocked.Add(itemId);
			if (teammateName != null)
			{
				AnyNewUnlocksFromTeammates = true;
				_unlockedByTeammate[itemId] = teammateName;
			}
		}
	}

	public void SetSacrificeCountDirectly(string persistentId, int sacrificeCount)
	{
		int value = Utils.Clamp(sacrificeCount, 0, 9999);
		_sacrificeCountByItemPersistentId[persistentId] = value;
		if (ContentSamples.ItemNetIdsByPersistentIds.TryGetValue(persistentId, out var value2))
		{
			_sacrificesCountByItemIdCache[value2] = value;
			MarkContentsDirty();
		}
	}

	public void Save(BinaryWriter writer)
	{
		writer.Write(value: false);
		KeyValuePair<string, int>[] array = _sacrificeCountByItemPersistentId.ToArray();
		writer.Write(array.Length);
		KeyValuePair<string, int>[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			KeyValuePair<string, int> keyValuePair = array2[i];
			writer.Write(keyValuePair.Key);
			writer.Write(keyValuePair.Value);
		}
	}

	public void Load(BinaryReader reader, int gameVersionSaveWasMadeOn)
	{
		if (gameVersionSaveWasMadeOn >= 282)
		{
			reader.ReadBoolean();
		}
		int num = reader.ReadInt32();
		for (int i = 0; i < num; i++)
		{
			string key = reader.ReadString();
			int value = reader.ReadInt32();
			if (ContentSamples.ItemNetIdsByPersistentIds.TryGetValue(key, out var value2))
			{
				if (ContentSamples.CreativeResearchItemPersistentIdOverride.TryGetValue(value2, out var value3))
				{
					value2 = value3;
				}
				_sacrificesCountByItemIdCache[value2] = value;
				if (ContentSamples.ItemPersistentIdsByNetIds.TryGetValue(value2, out var value4))
				{
					key = value4;
				}
			}
			_sacrificeCountByItemPersistentId[key] = value;
		}
	}

	public void ValidateWorld(BinaryReader reader, int gameVersionSaveWasMadeOn)
	{
		int num = reader.ReadInt32();
		for (int i = 0; i < num; i++)
		{
			reader.ReadString();
			reader.ReadInt32();
		}
	}

	public void Reset()
	{
		_sacrificeCountByItemPersistentId.Clear();
		_sacrificesCountByItemIdCache.Clear();
		AnyNewUnlocksFromTeammates = false;
		_unlockedByTeammate.Clear();
		_newlyUnlocked.Clear();
		MarkContentsDirty();
	}

	public void OnPlayerJoining(int playerIndex)
	{
	}

	public void MarkContentsDirty()
	{
		LastEditId++;
	}
}
