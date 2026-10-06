namespace Terraria.GameContent.FishDropRules;

public abstract class FishRarityCondition
{
	public float FrequencyOfAppearanceForVisuals;

	public bool HackedIsAny;

	public abstract bool Matches(FishingContext context);
}
