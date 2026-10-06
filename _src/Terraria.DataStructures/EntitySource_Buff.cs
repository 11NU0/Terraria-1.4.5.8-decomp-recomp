namespace Terraria.DataStructures;

public class EntitySource_Buff : IEntitySource
{
	public readonly IEntitySourceTarget Entity;

	public readonly int BuffId;

	public readonly int BuffIndex;

	public EntitySource_Buff(IEntitySourceTarget entity, int buffId, int buffIndex)
	{
		Entity = entity;
		BuffId = buffId;
		BuffIndex = buffIndex;
	}
}
