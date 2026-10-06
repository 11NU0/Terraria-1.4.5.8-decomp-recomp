namespace Terraria.WorldBuilding;

public class WorldSeedOption_Remix : AWorldGenerationOption
{
	protected override string KeyName => "Seed_Remix";

	public override string ServerConfigName => "remix";

	public WorldSeedOption_Remix()
	{
		SpecialSeedNames = new string[1] { "dontdigup" };
		SpecialSeedValues = new int[0];
	}
}
