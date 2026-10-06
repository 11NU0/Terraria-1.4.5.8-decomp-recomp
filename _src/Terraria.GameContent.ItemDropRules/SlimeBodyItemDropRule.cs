using System.Collections.Generic;
using Terraria.ID;

namespace Terraria.GameContent.ItemDropRules;

public class SlimeBodyItemDropRule : IItemDropRule
{
	public List<IItemDropRuleChainAttempt> ChainedRules { get; private set; }

	public SlimeBodyItemDropRule()
	{
		ChainedRules = new List<IItemDropRuleChainAttempt>();
	}

	public bool CanDrop(DropAttemptInfo info)
	{
		if (NPCID.Sets.SlimeCanContainItems[info.npc.type] && info.npc.ai[1] > 0f)
		{
			return info.npc.ai[1] < (float)ItemID.Count;
		}
		return false;
	}

	public ItemDropAttemptResult TryDroppingItem(DropAttemptInfo info)
	{
		int itemId = (int)info.npc.ai[1];
		GetDropInfo(itemId, out var amountDroppedMinimum, out var amountDroppedMaximum);
		CommonCode.DropItemFromNPC(info.npc, itemId, info.rng.Next(amountDroppedMinimum, amountDroppedMaximum + 1));
		return new ItemDropAttemptResult
		{
			State = ItemDropAttemptResultState.Success
		};
	}

	public void GetDropInfo(int itemId, out int amountDroppedMinimum, out int amountDroppedMaximum)
	{
		amountDroppedMinimum = 1;
		amountDroppedMaximum = 1;
		switch (itemId)
		{
		case 8:
			amountDroppedMinimum = 5;
			amountDroppedMaximum = 10;
			break;
		case 166:
			amountDroppedMinimum = 2;
			amountDroppedMaximum = 6;
			break;
		case 965:
			amountDroppedMinimum = 20;
			amountDroppedMaximum = 45;
			break;
		case 11:
		case 12:
		case 13:
		case 14:
		case 174:
		case 364:
		case 365:
		case 366:
		case 699:
		case 700:
		case 701:
		case 702:
		case 1104:
		case 1105:
		case 1106:
		case 3347:
			amountDroppedMinimum = 3;
			amountDroppedMaximum = 13;
			break;
		case 71:
			amountDroppedMinimum = 50;
			amountDroppedMaximum = 99;
			break;
		case 72:
			amountDroppedMinimum = 20;
			amountDroppedMaximum = 99;
			break;
		case 73:
			amountDroppedMinimum = 1;
			amountDroppedMaximum = 2;
			break;
		case 4343:
		case 4344:
			amountDroppedMinimum = 2;
			amountDroppedMaximum = 5;
			break;
		case 2:
		case 3:
		case 9:
		case 150:
		case 593:
		case 751:
		case 1103:
		case 3081:
		case 3086:
		case 3609:
		case 3610:
		case 5395:
			amountDroppedMinimum = 10;
			amountDroppedMaximum = 25;
			break;
		case 147:
		case 314:
		case 1124:
		case 1125:
		case 1345:
		case 3736:
		case 3737:
		case 3738:
			amountDroppedMinimum = 2;
			amountDroppedMaximum = 5;
			break;
		}
	}

	public void ReportDroprates(List<DropRateInfo> drops, DropRateInfoChainFeed ratesInfo)
	{
		Chains.ReportDroprates(ChainedRules, 1f, drops, ratesInfo);
	}
}
