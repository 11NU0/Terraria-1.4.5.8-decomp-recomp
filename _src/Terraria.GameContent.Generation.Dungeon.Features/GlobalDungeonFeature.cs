namespace Terraria.GameContent.Generation.Dungeon.Features;

public abstract class GlobalDungeonFeature : IDungeonFeature
{
	public DungeonFeatureSettings settings;

	public bool generated;

	public GlobalDungeonFeature(DungeonFeatureSettings settings)
	{
		this.settings = settings;
	}

	public abstract bool GenerateFeature(DungeonData data);
}
