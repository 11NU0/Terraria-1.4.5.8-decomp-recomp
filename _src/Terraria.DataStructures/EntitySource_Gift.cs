namespace Terraria.DataStructures;

public class EntitySource_Gift : IEntitySource
{
	public readonly IEntitySourceTarget Entity;

	public EntitySource_Gift(Entity entity)
	{
		Entity = entity;
	}
}
