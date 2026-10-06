using Terraria.IO;

namespace Terraria.WorldBuilding;

public abstract class GenPass : GenBase
{
	public string Name;

	public double Weight;

	public bool Enabled { get; private set; }

	public void Disable()
	{
		Enabled = false;
	}

	internal void Enable()
	{
		Enabled = true;
	}

	public GenPass(string name, double loadWeight)
	{
		Name = name;
		Weight = loadWeight;
		Enabled = true;
	}

	protected abstract void ApplyPass(GenerationProgress progress, GameConfiguration configuration);

	public void Apply(GenerationProgress progress, GameConfiguration configuration)
	{
		ApplyPass(progress, configuration);
	}
}
