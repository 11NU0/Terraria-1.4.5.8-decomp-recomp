using System.Collections.Generic;

namespace Terraria.GameContent.ItemDropRules;

public class DropBasedOnExtraGel : IItemDropRule, INestedItemDropRule
{
	public IItemDropRule ruleForNormal;

	public IItemDropRule ruleForExtraGel;

	public List<IItemDropRuleChainAttempt> ChainedRules { get; private set; }

	public DropBasedOnExtraGel(IItemDropRule ruleForNormal, IItemDropRule ruleForExtraGel)
	{
		this.ruleForNormal = ruleForNormal;
		this.ruleForExtraGel = ruleForExtraGel;
		ChainedRules = new List<IItemDropRuleChainAttempt>();
	}

	public bool CanDrop(DropAttemptInfo info)
	{
		if (SpecialSeedFeatures.ShouldDropExtraGel)
		{
			return ruleForExtraGel.CanDrop(info);
		}
		return ruleForNormal.CanDrop(info);
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
		if (SpecialSeedFeatures.ShouldDropExtraGel)
		{
			return resolveAction(ruleForExtraGel, info);
		}
		return resolveAction(ruleForNormal, info);
	}

	public void ReportDroprates(List<DropRateInfo> drops, DropRateInfoChainFeed ratesInfo)
	{
		DropRateInfoChainFeed ratesInfo2 = ratesInfo.With(1f);
		ratesInfo2.AddCondition(new Conditions.DropExtraGel());
		ruleForExtraGel.ReportDroprates(drops, ratesInfo2);
		DropRateInfoChainFeed ratesInfo3 = ratesInfo.With(1f);
		ratesInfo3.AddCondition(new Conditions.NotDropExtraGel());
		ruleForNormal.ReportDroprates(drops, ratesInfo3);
		Chains.ReportDroprates(ChainedRules, 1f, drops, ratesInfo);
	}
}
