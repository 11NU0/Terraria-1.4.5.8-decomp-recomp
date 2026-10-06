namespace Terraria.GameContent.Generation.Dungeon.Features;

public class DungeonPillarSettings : DungeonFeatureSettings
{
	public DungeonGenerationStyleData Style;

	public PillarType PillarType;

	public int Width;

	public int Height;

	public bool Wall;

	public int OverridePaintTile = -1;

	public int OverridePaintWall = -1;

	public bool CrowningOnTop;

	public bool CrowningOnBottom;

	public bool CrowningStopsAtPillar;

	public bool AlwaysPlaceEntirePillar = true;
}
