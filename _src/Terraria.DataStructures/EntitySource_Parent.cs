namespace Terraria.DataStructures;

public class EntitySource_Parent : IEntitySource
{
	public readonly IEntitySourceTarget Entity;

	public EntitySource_Parent(IEntitySourceTarget entity)
	{
		Entity = entity;
	}
}
