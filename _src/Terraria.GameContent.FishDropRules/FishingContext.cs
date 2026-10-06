using Terraria.DataStructures;
using Terraria.Utilities;

namespace Terraria.GameContent.FishDropRules;

public class FishingContext
{
	public UnifiedRandom Random = new UnifiedRandom();

	public FishingAttempt Fisher;

	public Player Player;

	public bool RolledCorruption;

	public bool RolledCrimson;

	public bool RolledJungle;

	public bool RolledSnow;

	public bool RolledDesert;

	public bool RolledInfectedDesert;

	public bool RolledRemixOcean;
}
