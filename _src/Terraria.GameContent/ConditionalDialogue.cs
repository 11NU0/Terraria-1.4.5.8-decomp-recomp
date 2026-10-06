using System;
using System.Collections.Generic;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;

namespace Terraria.GameContent;

public abstract class ConditionalDialogue
{
	public static class ItemGroups
	{
		public static RecipeGroup Ore = new RecipeGroup("RecipeGroups.Ore", 699, 12, 11, 700, 14, 701, 13, 702);

		public static RecipeGroup Bars = new RecipeGroup("RecipeGroups.Bar", 703, 20, 22, 704, 21, 705, 19, 706);

		public static RecipeGroup Anvils = new RecipeGroup("ItemName.IronAnvil", 35, 716);

		public static RecipeGroup Whips = new RecipeGroup("RecipeGroups.Whip");

		public static RecipeGroup Mounts = new RecipeGroup("RecipeGroups.Mount");

		internal static void PostSetupContent()
		{
			foreach (Item value in ContentSamples.ItemsByType.Values)
			{
				if (ProjectileID.Sets.IsAWhip[value.shoot])
				{
					Whips.Add(value.type);
				}
			}
			foreach (Item value2 in ContentSamples.ItemsByType.Values)
			{
				if (value2.mountType != -1)
				{
					Mounts.Add(value2.type);
				}
			}
		}
	}

	private class FreeCakeDialogue : ConditionalDialogue
	{
		public FreeCakeDialogue()
			: base((NPC _) => NPC.freeCake)
		{
		}

		public override string GetChatAndClearCondition(NPC npc)
		{
			NPC.freeCake = false;
			NetMessage.SendData(51, -1, -1, null, Main.myPlayer, 10f);
			Item item = new Item();
			item.SetDefaults(3750);
			Main.LocalPlayer.QuickSpawnItem(new EntitySource_Gift(npc), item, GetItemSettings.GiftRecieved);
			return Language.GetTextValue("PartyGirlSpecialText.Cake" + Main.rand.Next(1, 4));
		}
	}

	private static List<ConditionalDialogue>[] _registry = new List<ConditionalDialogue>[NPCID.Count];

	public readonly Predicate<NPC> ConditionsMet;

	public bool ShowIndicator { get; private set; }

	private static void Register(int npcType, ConditionalDialogue dialogue)
	{
		List<ConditionalDialogue> list = _registry[npcType];
		if (list == null)
		{
			list = (_registry[npcType] = new List<ConditionalDialogue>());
		}
		list.Add(dialogue);
	}

	public static bool TryGetPendingDialogue(NPC npc, out ConditionalDialogue dialogue)
	{
		dialogue = null;
		List<ConditionalDialogue> list = _registry[npc.type];
		if (list == null)
		{
			return false;
		}
		foreach (ConditionalDialogue item in list)
		{
			if (item.ConditionsMet(npc))
			{
				dialogue = item;
				return true;
			}
		}
		return false;
	}

	public ConditionalDialogue(Predicate<NPC> condition = null)
	{
		ShowIndicator = true;
		ConditionsMet = condition ?? ((Predicate<NPC>)((NPC _) => true));
	}

	public void HideIndicator()
	{
		ShowIndicator = false;
	}

	public abstract string GetChatAndClearCondition(NPC npc);

	public void Register(int npcType)
	{
		Register(npcType, this);
	}

	internal static void Init()
	{
		new FreeCakeDialogue().Register(208);
	}

	public static Predicate<NPC> CreateInventoryCondition(RecipeGroup item, int stack)
	{
		return CreateInventoryCondition(new Recipe.RequiredItemEntry(item, stack));
	}

	public static Predicate<NPC> CreateInventoryCondition(params Recipe.RequiredItemEntry[] requiredItems)
	{
		return (NPC _) => Recipe.CollectedEnoughItemsToCraft(requiredItems);
	}
}
