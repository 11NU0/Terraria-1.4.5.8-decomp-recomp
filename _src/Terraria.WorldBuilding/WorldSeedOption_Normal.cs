using System.Linq;

namespace Terraria.WorldBuilding;

public class WorldSeedOption_Normal : AWorldGenerationOption
{
	protected override string KeyName => "Seed_Normal";

	public override string ServerConfigName => null;

	public WorldSeedOption_Normal()
	{
		SpecialSeedNames = new string[0];
		SpecialSeedValues = new int[0];
		AWorldGenerationOption.OnOptionStateChanged += UpdateDependentState;
	}

	private void UpdateDependentState(AWorldGenerationOption changed)
	{
		Enabled = WorldGenerationOptions.Options.All((AWorldGenerationOption x) => x == this || !x.Enabled);
	}

	protected override void OnEnabledStateChanged()
	{
		if (!Enabled)
		{
			return;
		}
		foreach (AWorldGenerationOption option in WorldGenerationOptions.Options)
		{
			if (option != this)
			{
				option.Enabled = false;
			}
		}
	}
}
