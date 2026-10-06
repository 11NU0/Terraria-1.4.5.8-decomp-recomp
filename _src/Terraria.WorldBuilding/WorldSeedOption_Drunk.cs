namespace Terraria.WorldBuilding;

public class WorldSeedOption_Drunk : AWorldGenerationOption
{
	protected override string KeyName => "Seed_Drunk";

	public override string ServerConfigName => "drunk";

	public WorldSeedOption_Drunk()
	{
		SpecialSeedNames = new string[0];
		SpecialSeedValues = new int[1] { 5162020 };
	}
}
