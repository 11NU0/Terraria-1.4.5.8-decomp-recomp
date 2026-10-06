namespace Terraria.DataStructures;

public class EntitySource_OnHit_ByItemSourceID : AEntitySource_OnHit
{
	public readonly int SourceId;

	public EntitySource_OnHit_ByItemSourceID(IEntitySourceTarget entityStriking, IEntitySourceTarget entityStruck, int itemSourceId)
		: base(entityStriking, entityStruck)
	{
		SourceId = itemSourceId;
	}
}
