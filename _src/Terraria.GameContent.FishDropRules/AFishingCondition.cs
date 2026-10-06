namespace Terraria.GameContent.FishDropRules;

public abstract class AFishingCondition
{
	public bool CanBeSkippedForDisplay;

	public abstract bool Matches(FishingContext context);
}
