namespace Terraria.WorldBuilding;

public class WorldSeedOption_NotTheBees : AWorldGenerationOption
{
	protected override string KeyName => "Seed_NotTheBees";

	public override string ServerConfigName => "notthebees";

	public WorldSeedOption_NotTheBees()
	{
		SpecialSeedNames = new string[1] { "notthebees" };
		SpecialSeedValues = new int[0];
	}
}
