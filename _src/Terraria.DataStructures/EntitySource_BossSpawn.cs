namespace Terraria.DataStructures;

public class EntitySource_BossSpawn : IEntitySource
{
	public readonly IEntitySourceTarget Entity;

	public EntitySource_BossSpawn(IEntitySourceTarget entity)
	{
		Entity = entity;
	}
}
