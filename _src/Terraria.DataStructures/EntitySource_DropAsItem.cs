namespace Terraria.DataStructures;

public class EntitySource_DropAsItem : IEntitySource
{
	public readonly IEntitySourceTarget Entity;

	public EntitySource_DropAsItem(IEntitySourceTarget entity)
	{
		Entity = entity;
	}
}
