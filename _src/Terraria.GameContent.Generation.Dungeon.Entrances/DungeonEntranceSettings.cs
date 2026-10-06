namespace Terraria.GameContent.Generation.Dungeon.Entrances;

public abstract class DungeonEntranceSettings
{
	public DungeonEntranceType EntranceType;

	public int RandomSeed;

	public DungeonGenerationStyleData StyleData;

	public bool PrecalculateEntrancePosition;
}
