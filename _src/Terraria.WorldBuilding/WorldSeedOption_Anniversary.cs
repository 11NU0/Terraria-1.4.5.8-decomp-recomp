namespace Terraria.WorldBuilding;

public class WorldSeedOption_Anniversary : AWorldGenerationOption
{
	protected override string KeyName => "Seed_Celebration";

	public override string ServerConfigName => "celebration";

	public WorldSeedOption_Anniversary()
	{
		SpecialSeedNames = new string[1] { "celebrationmk10" };
		SpecialSeedValues = new int[2] { 5162021, 5162011 };
	}
}
