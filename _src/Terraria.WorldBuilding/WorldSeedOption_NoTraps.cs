namespace Terraria.WorldBuilding;

public class WorldSeedOption_NoTraps : AWorldGenerationOption
{
	protected override string KeyName => "Seed_NoTraps";

	public override string ServerConfigName => "notraps";

	public WorldSeedOption_NoTraps()
	{
		SpecialSeedNames = new string[1] { "notraps" };
		SpecialSeedValues = new int[0];
	}
}
