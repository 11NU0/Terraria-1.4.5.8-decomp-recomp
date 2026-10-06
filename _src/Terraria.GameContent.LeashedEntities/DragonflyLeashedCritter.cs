namespace Terraria.GameContent.LeashedEntities;

public class DragonflyLeashedCritter : FlyerLeashedCritter
{
	public new static DragonflyLeashedCritter Prototype = new DragonflyLeashedCritter();

	public DragonflyLeashedCritter()
	{
		minWaitTime = 10;
		maxFlySpeed = 2.5f;
		acceleration = 0.4f;
		brakeDuration = 10;
	}
}
