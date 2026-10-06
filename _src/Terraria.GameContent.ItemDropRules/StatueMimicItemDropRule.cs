using System.Collections.Generic;
using Terraria.ID;

namespace Terraria.GameContent.ItemDropRules;

public class StatueMimicItemDropRule : IItemDropRule
{
	public List<IItemDropRuleChainAttempt> ChainedRules { get; private set; }

	public StatueMimicItemDropRule()
	{
		ChainedRules = new List<IItemDropRuleChainAttempt>();
	}

	public bool CanDrop(DropAttemptInfo info)
	{
		if (info.npc.ai[1] > 0f)
		{
			return info.npc.ai[1] < (float)ItemID.Count;
		}
		return false;
	}

	public void ReportDroprates(List<DropRateInfo> drops, DropRateInfoChainFeed ratesInfo)
	{
		Chains.ReportDroprates(ChainedRules, 1f, drops, ratesInfo);
	}

	public ItemDropAttemptResult TryDroppingItem(DropAttemptInfo info)
	{
		int itemId = WorldGen.StatueStyleToItem((int)info.npc.ai[1]);
		CommonCode.DropItemFromNPC(info.npc, itemId, 1);
		return new ItemDropAttemptResult
		{
			State = ItemDropAttemptResultState.Success
		};
	}
}
