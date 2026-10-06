namespace Terraria.GameContent.FishDropRules;

public class FishingConditions
{
	public class QuestFishCondition : AFishingCondition
	{
		public int CheckedType;

		public override bool Matches(FishingContext context)
		{
			return context.Fisher.questFish == CheckedType;
		}
	}

	public class QuestFishConditionRemix : AFishingCondition
	{
		public int CheckedType;

		public override bool Matches(FishingContext context)
		{
			if (context.Fisher.questFish == CheckedType)
			{
				return Main.remixWorld;
			}
			return false;
		}
	}
}
