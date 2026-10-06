using System;
using System.Collections.Generic;
using System.Linq;
using Terraria.ID;
using Terraria.Localization;

namespace Terraria;

public class RecipeGroup
{
	public static readonly int FakeItemIdOffset = 1000000;

	public static LocalizedText DefaultCombineFormat = Language.GetText("CombineFormat.RecipeGroup");

	public Func<string> GetText;

	public HashSet<int> ValidItems = new HashSet<int>();

	public List<int> Items = new List<int>();

	public int DecraftItemId;

	public static Dictionary<int, RecipeGroup> recipeGroups = new Dictionary<int, RecipeGroup>();

	public static int nextRecipeGroupIndex;

	public int RegisteredId { get; private set; }

	private static Func<string> WithDefaultCombineFormat(string key)
	{
		LocalizedText text = Language.GetText(key);
		return () => DefaultCombineFormat.Format(text);
	}

	public RecipeGroup(string groupDescriptorKey, params int[] validItems)
		: this(WithDefaultCombineFormat(groupDescriptorKey), validItems)
	{
	}

	public RecipeGroup(Func<string> getName, params int[] validItems)
	{
		RegisteredId = -1;
		GetText = getName;
		foreach (int itemID in validItems)
		{
			Add(itemID);
		}
	}

	public RecipeGroup Add(int itemID, Func<bool> isPreferred = null)
	{
		ValidItems.Add(itemID);
		Items.Add(itemID);
		return this;
	}

	internal void SortDecraftingEntries()
	{
		DecraftItemId = Items.OrderBy((int e) => ContentSamples.ItemsByType[e].value).First();
	}

	public override string ToString()
	{
		return GetText();
	}

	public RecipeGroup Register()
	{
		if (RegisteredId >= 0)
		{
			throw new Exception("Already registered");
		}
		int key = (RegisteredId = nextRecipeGroupIndex++);
		recipeGroups.Add(key, this);
		return this;
	}

	public int CountUsableItems(Dictionary<int, int> itemStacksAvailable)
	{
		int num = 0;
		foreach (int validItem in ValidItems)
		{
			if (itemStacksAvailable.TryGetValue(validItem, out var value))
			{
				num += value;
			}
		}
		return num;
	}

	public int GetGroupFakeItemId()
	{
		return RegisteredId + FakeItemIdOffset;
	}

	public bool Contains(int itemType)
	{
		return ValidItems.Contains(itemType);
	}

	public int GetPlaceholderItemType()
	{
		return Items[0];
	}
}
