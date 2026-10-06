namespace Terraria.DataStructures;

public class EntitySource_OnHit_ByProjectileSourceID : AEntitySource_OnHit
{
	public readonly int SourceId;

	public EntitySource_OnHit_ByProjectileSourceID(IEntitySourceTarget entityStriking, IEntitySourceTarget entityStruck, int projectileSourceId)
		: base(entityStriking, entityStruck)
	{
		SourceId = projectileSourceId;
	}
}
