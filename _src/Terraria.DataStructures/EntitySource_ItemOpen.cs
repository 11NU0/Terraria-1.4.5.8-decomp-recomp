namespace Terraria.DataStructures;

public class EntitySource_ItemOpen : IEntitySource
{
	public readonly IEntitySourceTarget Entity;

	public readonly int ItemType;

	public EntitySource_ItemOpen(IEntitySourceTarget entity, int itemType)
	{
		Entity = entity;
		ItemType = itemType;
	}
}
