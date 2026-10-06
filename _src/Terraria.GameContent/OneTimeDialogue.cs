using System;
using System.Collections.Generic;
using Terraria.DataStructures;
using Terraria.Localization;

namespace Terraria.GameContent;

public class OneTimeDialogue : ConditionalDialogue
{
	public readonly LocalizedText ChatText;

	public readonly List<Item> Rewards = new List<Item>();

	public OneTimeDialogue(string key, Predicate<NPC> condition = null)
		: base((NPC npc) => !Main.LocalPlayer.oneTimeDialoguesSeen.Contains(key) && (condition == null || condition(npc)))
	{
		ChatText = Language.GetText(key);
	}

	public override string GetChatAndClearCondition(NPC npc)
	{
		Player localPlayer = Main.LocalPlayer;
		localPlayer.oneTimeDialoguesSeen.Add(ChatText.Key);
		foreach (Item reward in Rewards)
		{
			localPlayer.QuickSpawnItem(new EntitySource_Gift(npc), reward, GetItemSettings.GiftRecieved);
		}
		return ChatText.Value;
	}

	public OneTimeDialogue WithReward(int itemId, int stack = 1)
	{
		Item item = new Item();
		item.SetDefaults(itemId);
		item.stack = stack;
		return WithRewards(item);
	}

	public OneTimeDialogue WithRewards(params Item[] rewards)
	{
		Rewards.AddRange(rewards);
		return this;
	}
}
