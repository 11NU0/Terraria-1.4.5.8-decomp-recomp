namespace Terraria.WorldBuilding;

public class WorldSeedOption_Skyblock : AWorldGenerationOption
{
	protected override string KeyName => "Seed_Skyblock";

	public override string ServerConfigName => "skyblock";

	public WorldSeedOption_Skyblock()
	{
		SpecialSeedNames = new string[1] { "skyblock" };
		SpecialSeedValues = new int[0];
	}
}
