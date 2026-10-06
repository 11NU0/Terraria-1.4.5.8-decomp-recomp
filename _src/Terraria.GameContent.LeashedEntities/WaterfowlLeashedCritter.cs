namespace Terraria.GameContent.LeashedEntities;

public class WaterfowlLeashedCritter : BirdLeashedCritter
{
	public new static WaterfowlLeashedCritter Prototype = new WaterfowlLeashedCritter();

	public WaterfowlLeashedCritter()
	{
		hasGroundBias = true;
	}

	protected override void CopyToDummy()
	{
		base.CopyToDummy();
		if (velocity.Y != 0f)
		{
			LeashedCritter._dummy.type++;
		}
	}
}
