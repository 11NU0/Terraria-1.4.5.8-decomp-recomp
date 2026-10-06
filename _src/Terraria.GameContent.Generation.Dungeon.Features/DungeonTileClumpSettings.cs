namespace Terraria.GameContent.Generation.Dungeon.Features;

public class DungeonTileClumpSettings : DungeonFeatureSettings
{
	public int RandomSeed;

	public double Strength;

	public int Steps;

	public ushort TileType;

	public ushort WallType;

	public DungeonBounds AreaToGenerateIn;

	public ushort? OnlyReplaceThisTileType;

	public ushort? OnlyReplaceThisWallType;
}
