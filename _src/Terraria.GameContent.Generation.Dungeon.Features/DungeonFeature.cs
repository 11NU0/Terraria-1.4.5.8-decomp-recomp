namespace Terraria.GameContent.Generation.Dungeon.Features;

public abstract class DungeonFeature : IDungeonFeature
{
	public DungeonFeatureSettings settings;

	public DungeonBounds Bounds = new DungeonBounds();

	public bool generated;

	public DungeonFeature(DungeonFeatureSettings settings)
	{
		this.settings = settings;
	}

	public abstract bool GenerateFeature(DungeonData data, int x, int y);

	public virtual bool CanGenerateFeatureAt(DungeonData data, IDungeonFeature feature, int x, int y)
	{
		return true;
	}
}
