namespace Terraria.DataStructures;

public class EntitySource_FishedOut : IEntitySource
{
	public readonly IEntitySourceTarget Entity;

	public EntitySource_FishedOut(IEntitySourceTarget entity)
	{
		Entity = entity;
	}
}
