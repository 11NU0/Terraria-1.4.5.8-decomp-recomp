namespace Terraria.DataStructures;

public class EntitySource_Loot : IEntitySource
{
	public readonly IEntitySourceTarget Entity;

	public EntitySource_Loot(IEntitySourceTarget entity)
	{
		Entity = entity;
	}
}
