using System;

namespace Terraria.DataStructures;

[Flags]
public enum TileDataType
{
	Tile = 1,
	TilePaint = 2,
	Wall = 4,
	WallPaint = 8,
	Liquid = 0x10,
	Wiring = 0x20,
	Actuator = 0x40,
	Slope = 0x80,
	All = Tile | TilePaint | Wall | WallPaint | Liquid | Wiring | Actuator | Slope
}
