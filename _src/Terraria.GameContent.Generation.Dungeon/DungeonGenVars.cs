using System.Collections.Generic;
using ReLogic.Utilities;
using Terraria.GameContent.Biomes;
using Terraria.GameContent.Generation.Dungeon.Entrances;

namespace Terraria.GameContent.Generation.Dungeon;

public class DungeonGenVars
{
	public int dungeonSide;

	public int dungeonLocation;

	public DungeonColor dungeonColor;

	public ushort brickTileType = 41;

	public ushort brickWallType = 7;

	public ushort brickCrackedTileType = 481;

	public ushort windowGlassWallType = 91;

	public ushort windowClosedGlassWallType = 149;

	public ushort windowEdgeWallType = 8;

	public int[] windowPlatformItemTypes;

	public int generatingDungeonPositionX;

	public int generatingDungeonPositionY;

	public int generatingDungeonTopX;

	public int dungeonLootStyle;

	public DungeonBounds outerPotentialDungeonBounds = new DungeonBounds();

	public DungeonBounds innerPotentialDungeonBounds = new DungeonBounds();

	public DungeonGenerationStyleData dungeonStyle;

	public List<DungeonGenerationStyleData> dungeonGenerationStyles = new List<DungeonGenerationStyleData>();

	public DitherSnake dungeonDitherSnake = new DitherSnake();

	public bool[] isCrackedBrick;

	public bool[] isPitTrapTile;

	public bool[] isDungeonTile;

	public bool[] isDungeonWall;

	public bool[] isDungeonWallEdge;

	public bool[] isDungeonWallGlass;

	public bool GeneratingDungeon;

	public PreGenDungeonEntranceSettings preGenDungeonEntranceSettings;

	public Vector2D dungeonEntrancePosition;

	public bool desertChestLootState;
}
