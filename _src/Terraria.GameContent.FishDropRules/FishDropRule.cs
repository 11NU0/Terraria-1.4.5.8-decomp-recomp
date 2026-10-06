namespace Terraria.GameContent.FishDropRules;

public class FishDropRule
{
	public int[] PossibleItems;

	public int ChanceNumerator = 1;

	public int ChanceDenominator = 1;

	public AFishingCondition[] Conditions;

	public FishRarityCondition Rarity;

	public bool IsStopper
	{
		get
		{
			if (PossibleItems.Length != 0)
			{
				if (Rarity.HackedIsAny)
				{
					return ChanceDenominator == ChanceNumerator;
				}
				return false;
			}
			return true;
		}
	}

	public bool Attempt(FishingContext context, out int resultItemType)
	{
		resultItemType = 0;
		if (!MeetsConditions(context, forDisplay: false))
		{
			return false;
		}
		if (context.Random.Next(ChanceDenominator) >= ChanceNumerator)
		{
			return false;
		}
		if (!Rarity.Matches(context))
		{
			return false;
		}
		if (PossibleItems != null && PossibleItems.Length != 0)
		{
			resultItemType = context.Random.NextFromList(PossibleItems);
		}
		return true;
	}

	public bool MeetsConditions(FishingContext context, bool forDisplay)
	{
		AFishingCondition[] conditions = Conditions;
		for (int i = 0; i < conditions.Length; i++)
		{
			if (!conditions[i].Matches(context))
			{
				return false;
			}
		}
		return true;
	}
}
