namespace Terraria.GameContent.Generation.Dungeon.Features;

public abstract class DungeonWindowSettings : DungeonFeatureSettings
{
	public DungeonGenerationStyleData Style;

	public int OverrideGlassPaint = -1;

	public int OverrideGlassType = -1;

	public bool Closed;
}
