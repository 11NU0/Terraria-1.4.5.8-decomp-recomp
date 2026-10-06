namespace Terraria.DataStructures;

public class EntitySource_TileInteraction : AEntitySource_Tile
{
	public readonly IEntitySourceTarget Entity;

	public EntitySource_TileInteraction(IEntitySourceTarget entity, int tileCoordsX, int tileCoordsY)
		: base(tileCoordsX, tileCoordsY)
	{
		Entity = entity;
	}
}
