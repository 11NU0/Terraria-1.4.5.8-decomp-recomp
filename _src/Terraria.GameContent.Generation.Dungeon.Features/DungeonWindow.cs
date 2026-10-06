namespace Terraria.GameContent.Generation.Dungeon.Features;

public abstract class DungeonWindow : DungeonFeature
{
	public DungeonWindow(DungeonFeatureSettings settings)
		: base(settings)
	{
		DungeonCrawler.CurrentDungeonData.dungeonFeatures.Add(this);
	}

	public override bool CanGenerateFeatureAt(DungeonData data, IDungeonFeature feature, int x, int y)
	{
		if (feature is DungeonGlobalWallVariants)
		{
			return true;
		}
		return false;
	}
}
