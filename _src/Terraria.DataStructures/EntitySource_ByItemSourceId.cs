namespace Terraria.DataStructures;

public class EntitySource_ByItemSourceId : IEntitySource
{
	public readonly IEntitySourceTarget Entity;

	public readonly int SourceId;

	public EntitySource_ByItemSourceId(IEntitySourceTarget entity, int itemSourceId)
	{
		Entity = entity;
		SourceId = itemSourceId;
	}
}
