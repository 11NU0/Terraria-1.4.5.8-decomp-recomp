namespace Terraria.DataStructures;

public class EntitySource_Mount : IEntitySource
{
	public readonly IEntitySourceTarget Entity;

	public readonly int MountId;

	public EntitySource_Mount(IEntitySourceTarget entity, int mountId)
	{
		Entity = entity;
		MountId = mountId;
	}
}
