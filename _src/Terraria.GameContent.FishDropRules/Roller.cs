using Terraria.DataStructures;

namespace Terraria.GameContent.FishDropRules;

public class Roller
{
	private FishingContext _context = new FishingContext();

	private FishDropRuleList _ruleList = new FishDropRuleList();

	public void Roll(Projectile projectile, FishingAttempt fisher)
	{
		FishingContext context = _context;
		context.Player = Main.player[projectile.owner];
		context.Fisher = fisher;
	}
}
