using Microsoft.Xna.Framework;

namespace Terraria.DataStructures;

public abstract class AEntitySource_Tile : IEntitySource
{
	public readonly Point TileCoords;

	public AEntitySource_Tile(int tileCoordsX, int tileCoordsY)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		TileCoords = new Point(tileCoordsX, tileCoordsY);
	}
}
