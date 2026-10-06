namespace Terraria.DataStructures;

public abstract class AEntitySource_OnHit : IEntitySource
{
	public readonly IEntitySourceTarget EntityStriking;

	public readonly IEntitySourceTarget EntityStruck;

	public AEntitySource_OnHit(IEntitySourceTarget entityStriking, IEntitySourceTarget entityStruck)
	{
		EntityStriking = entityStriking;
		EntityStruck = entityStruck;
	}
}
