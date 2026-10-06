namespace Terraria.GameContent.LeashedEntities;

public class CrawlingFlyLeashedCritter : FlyerLeashedCritter
{
	public new static CrawlingFlyLeashedCritter Prototype = new CrawlingFlyLeashedCritter();

	public CrawlingFlyLeashedCritter()
	{
		hasGroundBias = true;
	}

	protected override void SetDefaults(Item sample)
	{
		base.SetDefaults(sample);
		scale = Main.rand.NextFloat() * 0.2f + 0.7f;
	}
}
