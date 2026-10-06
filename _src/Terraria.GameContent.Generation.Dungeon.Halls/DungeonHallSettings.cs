namespace Terraria.GameContent.Generation.Dungeon.Halls;

public abstract class DungeonHallSettings
{
	public DungeonHallType HallType;

	public int RandomSeed;

	public DungeonGenerationStyleData StyleData;

	public int OverridePaintTile = -1;

	public int OverridePaintWall = -1;

	public double CrackedBrickChance = 0.166;

	public bool PlaceOverProtectedBricks;

	public double ZigzagChance = 0.66;

	public bool ForceStyleForDoorsAndPlatforms;

	public bool CarveOnly;
}
