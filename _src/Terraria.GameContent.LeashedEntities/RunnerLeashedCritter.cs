namespace Terraria.GameContent.LeashedEntities;

public class RunnerLeashedCritter : WalkerLeashedCritter
{
	public new static RunnerLeashedCritter Prototype = new RunnerLeashedCritter();

	public RunnerLeashedCritter()
	{
		anchorStyle = 1;
		walkingPace = 1.5f;
	}
}
