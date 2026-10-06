using System;
using System.Collections.Generic;

namespace Terraria.GameContent.FishDropRules;

public class FishDropRuleList
{
	private List<FishDropRule> _rules = new List<FishDropRule>();

	public int TryGetItemDropType(FishingContext context)
	{
		int resultItemType = 0;
		for (int i = 0; i < _rules.Count; i++)
		{
			if (_rules[i].Attempt(context, out resultItemType))
			{
				return resultItemType;
			}
		}
		return 0;
	}

	public void GetDisplayableDrops(FishingContext context, List<FishPossibilityEntry> resultTypes)
	{
		for (int i = 0; i < _rules.Count; i++)
		{
			FishDropRule fishDropRule = _rules[i];
			if (fishDropRule.MeetsConditions(context, forDisplay: true))
			{
				int itemType = 0;
				if (fishDropRule.PossibleItems.Length != 0)
				{
					itemType = context.Random.NextFromList(fishDropRule.PossibleItems);
				}
				resultTypes.Add(new FishPossibilityEntry
				{
					ItemType = itemType,
					Frequency = fishDropRule.Rarity.FrequencyOfAppearanceForVisuals
				});
				if (fishDropRule.IsStopper)
				{
					break;
				}
			}
		}
	}

	public void Add(FishDropRule rule)
	{
		Validate(rule);
		_rules.Add(rule);
	}

	private void Validate(FishDropRule rule)
	{
		if (rule.ChanceDenominator <= 0)
		{
			throw new ArgumentOutOfRangeException("FishDropRule.ChanceDenominator", "Chance Denominator must be positive non-zero number");
		}
	}
}
