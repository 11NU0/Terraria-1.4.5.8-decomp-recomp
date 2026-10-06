using System.Collections.Generic;
using System.IO;
using System.Linq;
using Terraria.DataStructures;
using Terraria.Net;
using Terraria.Testing;

namespace Terraria.GameContent;

public static class CraftingRequests
{
	public struct RemoteCraftRequest
	{
		public Recipe recipe;

		public Item result;

		public List<Item> consumed;

		public List<Recipe.RequiredItemEntry> requested;

		public bool quickCraft;
	}

	public class NetCraftingRequestsModule : NetModule
	{
		public static NetPacket WriteRequest(List<Recipe.RequiredItemEntry> items, List<Chest> chests)
		{
			NetPacket result = NetModule.CreatePacket<NetCraftingRequestsModule>();
			result.Writer.Write7BitEncodedInt(items.Count);
			foreach (Recipe.RequiredItemEntry item in items)
			{
				result.Writer.Write(item.itemIdOrRecipeGroup);
				result.Writer.Write7BitEncodedInt(item.stack);
			}
			result.Writer.Write7BitEncodedInt(chests.Count);
			foreach (Chest chest in chests)
			{
				result.Writer.Write7BitEncodedInt(chest.index);
			}
			return result;
		}

		public static NetPacket WriteResponse(bool approved)
		{
			NetPacket result = NetModule.CreatePacket<NetCraftingRequestsModule>();
			result.Writer.Write(approved);
			return result;
		}

		public void DeserializeRequest(BinaryReader reader, int userId)
		{
			int num = reader.Read7BitEncodedInt();
			List<Recipe.RequiredItemEntry> list = new List<Recipe.RequiredItemEntry>(num);
			for (int i = 0; i < num; i++)
			{
				list.Add(new Recipe.RequiredItemEntry(reader.ReadInt32(), reader.Read7BitEncodedInt()));
			}
			int num2 = reader.Read7BitEncodedInt();
			List<Chest> list2 = new List<Chest>(num2);
			for (int j = 0; j < num2; j++)
			{
				int num3 = reader.Read7BitEncodedInt();
				list2.Add((num3 < 0) ? null : Main.chest[num3]);
			}
			HandleRequest(userId, list, list2);
		}

		public void DeserializeResponse(BinaryReader reader)
		{
			HandleResponse(reader.ReadBoolean());
		}

		public override bool Deserialize(BinaryReader reader, int userId)
		{
			if (Main.netMode == 2)
			{
				DeserializeRequest(reader, userId);
			}
			else
			{
				DeserializeResponse(reader);
			}
			return true;
		}
	}

	private static Queue<RemoteCraftRequest> _pendingCrafts = new Queue<RemoteCraftRequest>();

	public static bool HasPendingRequests => _pendingCrafts.Count > 0;

	public static void Clear()
	{
		_pendingCrafts.Clear();
	}

	public static void CraftItem(Recipe recipe, int qty = 1, bool quickCraft = false)
	{
		Player localPlayer = Main.LocalPlayer;
		List<Chest> chests = Recipe._recipeChests;
		List<Recipe.RequiredItemEntry> list = new List<Recipe.RequiredItemEntry>();
		for (int i = 0; i < qty && (i <= 0 || (Recipe.CollectedEnoughItemsToCraft(recipe) && Main.CursorHasSpaceToCraftRecipe(recipe))); i++)
		{
			list.Clear();
			recipe.GetIngredientsForOneCraft(localPlayer, list);
			if (Main.netMode == 0 || list.All((Recipe.RequiredItemEntry req) => CanCraftLocally(req, chests)))
			{
				CraftLocally(recipe, quickCraft, chests, list);
			}
			else
			{
				CraftViaRequest(recipe, quickCraft, chests, list);
			}
			foreach (Recipe.RequiredItemEntry item in list)
			{
				Recipe.SubtractOwnedItem(item);
			}
		}
		CraftingEffects.OnCraft(recipe, quickCraft);
	}

	private static Item CreateResult(Recipe recipe)
	{
		Item item = recipe.createItem.Clone();
		item.OnCreated(new RecipeItemCreationContext(recipe));
		if (item.stack <= 1)
		{
			item.Prefix(-1);
		}
		return item;
	}

	private static void CraftLocally(Recipe recipe, bool quickCraft, List<Chest> chests, List<Recipe.RequiredItemEntry> ingredients)
	{
		foreach (Recipe.RequiredItemEntry ingredient in ingredients)
		{
			Invariant.Assert(Consume(ingredient, chests, null, fromChests: true) == 0, "Not enough items for local craft");
		}
		Main.CraftItem_GrantItem(recipe, CreateResult(recipe), quickCraft);
	}

	private static void CraftViaRequest(Recipe recipe, bool quickCraft, List<Chest> chests, List<Recipe.RequiredItemEntry> ingredients)
	{
		List<Item> list = new List<Item>();
		List<Recipe.RequiredItemEntry> list2 = new List<Recipe.RequiredItemEntry>();
		foreach (Recipe.RequiredItemEntry ingredient in ingredients)
		{
			int num = Consume(ingredient, chests, list, fromChests: false);
			if (num > 0)
			{
				list2.Add(new Recipe.RequiredItemEntry
				{
					itemIdOrRecipeGroup = ingredient.itemIdOrRecipeGroup,
					stack = num
				});
			}
		}
		Invariant.Assert(list2.Any(), "CraftViaRequest with no items?");
		Item item = CreateResult(recipe);
		if (!quickCraft)
		{
			FakeCursorItem.Add(item);
		}
		_pendingCrafts.Enqueue(new RemoteCraftRequest
		{
			recipe = recipe,
			result = item,
			consumed = list,
			requested = list2,
			quickCraft = quickCraft
		});
		NetManager.Instance.SendToServer(NetCraftingRequestsModule.WriteRequest(list2, chests));
	}

	private static bool IsLocallyAccessible(Chest chest)
	{
		if (!chest.bankChest)
		{
			return chest.index == Main.LocalPlayer.chest;
		}
		return true;
	}

	private static bool CanCraftLocally(Recipe.RequiredItemEntry req, List<Chest> chests)
	{
		Invariant.Assert(chests.Take(chests.FindLastIndex(IsLocallyAccessible)).All(IsLocallyAccessible), "Locally accessible chests come first");
		int num = 0;
		num += CountMatches(req, Main.LocalPlayer.inventory, 58);
		foreach (Chest chest in chests)
		{
			if (IsLocallyAccessible(chest))
			{
				num += CountMatches(req, chest.item, chest.maxItems);
			}
		}
		return num >= req.stack;
	}

	private static int CountMatches(Recipe.RequiredItemEntry req, List<Chest> chests)
	{
		int num = 0;
		foreach (Chest chest in chests)
		{
			num += CountMatches(req, chest.item, chest.maxItems);
		}
		return num;
	}

	private static int CountMatches(Recipe.RequiredItemEntry req, Item[] inv, int maxItems)
	{
		int num = 0;
		for (int i = 0; i < maxItems; i++)
		{
			Item item = inv[i];
			if (req.Matches(item.type))
			{
				num += item.stack;
			}
		}
		return num;
	}

	private static int Consume(Recipe.RequiredItemEntry req, List<Chest> chests, List<Item> consumedItems, bool fromChests)
	{
		int toConsume = req.stack;
		if (Main.netMode != 2)
		{
			ConsumeItemsFrom(Main.LocalPlayer.inventory, 58, req, ref toConsume, consumedItems);
		}
		foreach (Chest chest in chests)
		{
			if (chest.bankChest | fromChests)
			{
				ConsumeItemsFrom(chest, req, ref toConsume, consumedItems);
			}
		}
		return toConsume;
	}

	private static void ConsumeItemsFrom(Chest chest, Recipe.RequiredItemEntry req, ref int toConsume, List<Item> consumedItems = null)
	{
		ConsumeItemsFrom(chest.item, chest.maxItems, req, ref toConsume, consumedItems, chest.bankChest ? (-1) : chest.index);
	}

	private static void ConsumeItemsFrom(Item[] inventory, int maxItems, Recipe.RequiredItemEntry req, ref int toConsume, List<Item> consumedItems = null, int chestIndex = -1)
	{
		if (toConsume <= 0)
		{
			return;
		}
		if (Main.netMode == 2)
		{
			Invariant.Assert(chestIndex >= 0, "ConsumeItemsFrom, no bank index on servers");
		}
		if (Main.netMode == 1)
		{
			Invariant.Assert(chestIndex < 0 || chestIndex == Main.LocalPlayer.chest, "ConsumeItemsFrom, bank or open chest on client");
		}
		for (int i = 0; i < maxItems; i++)
		{
			Item item = inventory[i];
			if (!req.Matches(item.type))
			{
				continue;
			}
			if (item.stack > toConsume)
			{
				if (consumedItems != null)
				{
					Item item2 = item.Clone();
					item2.stack = toConsume;
					consumedItems.Add(item2);
				}
				item.stack -= toConsume;
				toConsume = 0;
			}
			else
			{
				toConsume -= item.stack;
				consumedItems?.Add(item);
				inventory[i] = new Item();
			}
			if (chestIndex >= 0)
			{
				NetMessage.SendData(32, -1, -1, null, chestIndex, i);
			}
			if (toConsume <= 0)
			{
				break;
			}
		}
	}

	public static bool CanCraftFromChest(Chest chest, int whoAmI)
	{
		if (Chest.IsLocked(chest.x, chest.y))
		{
			return false;
		}
		int num = Chest.UsingChest(chest.index);
		if (num >= 0 && num != whoAmI)
		{
			return false;
		}
		return true;
	}

	private static void HandleRequest(int whoAmI, List<Recipe.RequiredItemEntry> items, List<Chest> chests)
	{
		chests.RemoveAll((Chest chest) => chest == null || !CanCraftFromChest(chest, whoAmI));
		if (!items.All((Recipe.RequiredItemEntry req) => CountMatches(req, chests) >= req.stack))
		{
			NetManager.Instance.SendToClient(NetCraftingRequestsModule.WriteResponse(approved: false), whoAmI);
			return;
		}
		foreach (Recipe.RequiredItemEntry item in items)
		{
			Invariant.Assert(Consume(item, chests, null, fromChests: true) == 0, "Not enough items for approved craft");
		}
		NetManager.Instance.SendToClient(NetCraftingRequestsModule.WriteResponse(approved: true), whoAmI);
	}

	private static void HandleResponse(bool approved)
	{
		RemoteCraftRequest remoteCraftRequest = _pendingCrafts.Dequeue();
		FakeCursorItem.Remove(remoteCraftRequest.result.type, remoteCraftRequest.result.stack);
		if (approved)
		{
			Main.CraftItem_GrantItem(remoteCraftRequest.recipe, remoteCraftRequest.result, remoteCraftRequest.quickCraft);
			return;
		}
		foreach (Item item in remoteCraftRequest.consumed)
		{
			Refund(item);
		}
	}

	public static void Refund(Item item)
	{
		Main.LocalPlayer.GetOrDropItem(item, GetItemSettings.RefundConsumedItem);
	}

	public static void SubtractPendingRequests()
	{
		foreach (RemoteCraftRequest pendingCraft in _pendingCrafts)
		{
			foreach (Recipe.RequiredItemEntry item in pendingCraft.requested)
			{
				Recipe.SubtractOwnedItem(item);
			}
		}
	}

	public static void SavePossibleRefunds(BinaryWriter writer)
	{
		int value = _pendingCrafts.Sum((RemoteCraftRequest c) => c.consumed.Count);
		writer.Write(value);
		foreach (RemoteCraftRequest pendingCraft in _pendingCrafts)
		{
			foreach (Item item in pendingCraft.consumed)
			{
				item.Serialize(writer);
			}
		}
	}
}
