using System.Collections.Generic;

namespace Terraria.GameContent.ItemDropRules;

public class OneFromRulesRule : IItemDropRule, INestedItemDropRule
{
	public IItemDropRule[] options;

	public int chanceDenominator;

	public List<IItemDropRuleChainAttempt> ChainedRules { get; private set; }

	public OneFromRulesRule(int chanceDenominator, params IItemDropRule[] options)
	{
		this.chanceDenominator = chanceDenominator;
		this.options = options;
		ChainedRules = new List<IItemDropRuleChainAttempt>();
	}

	public bool CanDrop(DropAttemptInfo info)
	{
		return true;
	}

	public ItemDropAttemptResult TryDroppingItem(DropAttemptInfo info)
	{
		return new ItemDropAttemptResult
		{
			State = ItemDropAttemptResultState.DidNotRunCode
		};
	}

	public ItemDropAttemptResult TryDroppingItem(DropAttemptInfo info, ItemDropRuleResolveAction resolveAction)
	{
		int num = -1;
		if (info.rng.Next(chanceDenominator) == 0)
		{
			num = info.rng.Next(options.Length);
			resolveAction(options[num], info);
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
		float num = 1f / (float)chanceDenominator;
		float multiplier = 1f / (float)options.Length * num;
		for (int i = 0; i < options.Length; i++)
		{
			options[i].ReportDroprates(drops, ratesInfo.With(multiplier));
		}
		Chains.ReportDroprates(ChainedRules, num, drops, ratesInfo);
	}
}
