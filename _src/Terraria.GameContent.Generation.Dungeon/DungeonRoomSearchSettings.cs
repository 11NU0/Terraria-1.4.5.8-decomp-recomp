using Terraria.GameContent.Generation.Dungeon.Rooms;

namespace Terraria.GameContent.Generation.Dungeon;

public struct DungeonRoomSearchSettings
{
	public int Fluff;

	public DungeonRoom ExcludedRoom;

	public ProgressionStageCheck ProgressionStageCheck;

	public int? ProgressionStage;

	public int? MaximumDistance;

	public bool UseOuterBounds;
}
