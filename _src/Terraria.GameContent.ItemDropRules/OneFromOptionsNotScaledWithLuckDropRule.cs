using System.Collections.Generic;

namespace Terraria.GameContent.ItemDropRules;

public class OneFromOptionsNotScaledWithLuckDropRule : IItemDropRule
{
	public int[] dropIds;

	public int chanceDenominator;

	public int chanceNumerator;

	public List<IItemDropRuleChainAttempt> ChainedRules { get; private set; }

	public OneFromOptionsNotScaledWithLuckDropRule(int chanceDenominator, int chanceNumerator, params int[] options)
	{
		this.chanceDenominator = chanceDenominator;
		dropIds = options;
		this.chanceNumerator = chanceNumerator;
		ChainedRules = new List<IItemDropRuleChainAttempt>();
	}

	public bool CanDrop(DropAttemptInfo info)
	{
		return true;
	}

	public ItemDropAttemptResult TryDroppingItem(DropAttemptInfo info)
	{
		if (info.rng.Next(chanceDenominator) < chanceNumerator)
		{
			CommonCode.DropItemFromNPC(info.npc, dropIds[info.rng.Next(dropIds.Length)], 1);
			return new ItemDropAttemptResult
			{
				State = ItemDropAttemptResultState.Success
			};
		}
		return new ItemDropAttemptResult
		{
			State = ItemDropAttemptResultState.FailedRandomRoll
		};
	}

	public void ReportDroprates(List<DropRateInfo> drops, DropRateInfoChainFeed ratesInfo)
	{
		float num = (float)chanceNumerator / (float)chanceDenominator;
		float num2 = num * ratesInfo.parentDroprateChance;
		float dropRate = 1f / (float)dropIds.Length * num2;
		for (int i = 0; i < dropIds.Length; i++)
		{
			drops.Add(new DropRateInfo(dropIds[i], 1, 1, dropRate, ratesInfo.conditions));
		}
		Chains.ReportDroprates(ChainedRules, num, drops, ratesInfo);
	}
}
