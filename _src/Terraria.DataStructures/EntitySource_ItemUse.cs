namespace Terraria.DataStructures;

public class EntitySource_ItemUse : IEntitySource
{
	public readonly IEntitySourceTarget Entity;

	public readonly Item Item;

	public EntitySource_ItemUse(IEntitySourceTarget entity, Item item)
	{
		Entity = entity;
		Item = item;
	}
}
