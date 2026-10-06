using Microsoft.Xna.Framework;
using Terraria.GameContent.Biomes;

namespace Terraria.GameContent.Generation.Dungeon.Rooms;

public abstract class DungeonRoomSettings
{
	public DungeonControlLine ControlLine;

	public Point RoomPosition;

	public DungeonRoomType RoomType;

	public int RandomSeed;

	public DungeonGenerationStyleData StyleData;

	public int ProgressionStage;

	public bool StartingRoom;

	public int OverridePaintTile = -1;

	public int OverridePaintWall = -1;

	public bool ForceStyleForDoorsAndPlatforms;

	public bool OnCurvedLine;

	public SnakeOrientation Orientation;

	public DungeonUtils.GetHallwayConnectionPoint HallwayConnectionPointOverride;

	public int? HallwayPointAdjuster;

	public abstract int GetBoundingRadius();
}
