namespace Terraria.GameContent.LeashedEntities;

public class BirdLeashedCritter : FlyerLeashedCritter
{
	public new static BirdLeashedCritter Prototype = new BirdLeashedCritter();

	public BirdLeashedCritter()
	{
		anchorStyle = 2;
		minWaitTime = 120;
		maxWaitTime = 420;
		maxFlySpeed = 1.2f;
		acceleration = 0.1f;
		rotationScalar = 0.25f;
		brakeDuration = 10;
		hoverAmplitude = 3f;
		hoverPeriod = 0.005f;
	}
}
