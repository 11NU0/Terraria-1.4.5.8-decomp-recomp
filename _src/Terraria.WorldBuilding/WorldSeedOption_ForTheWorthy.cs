namespace Terraria.WorldBuilding;

public class WorldSeedOption_ForTheWorthy : AWorldGenerationOption
{
	protected override string KeyName => "Seed_ForTheWorthy";

	public override string ServerConfigName => "fortheworthy";

	public WorldSeedOption_ForTheWorthy()
	{
		SpecialSeedNames = new string[1] { "fortheworthy" };
		SpecialSeedValues = new int[0];
	}
}
