namespace Terraria.GameContent.LeashedEntities;

public class CrawlerLeashedCritter : WalkerLeashedCritter
{
	public new static CrawlerLeashedCritter Prototype = new CrawlerLeashedCritter();

	public CrawlerLeashedCritter()
	{
		anchorStyle = 1;
		walkingPace = 0.4f;
	}
}
