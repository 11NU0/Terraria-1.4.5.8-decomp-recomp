using Microsoft.Xna.Framework;
using Terraria.GameContent.Generation.Dungeon.Features;

namespace Terraria.GameContent.Generation.Dungeon.Entrances;

public abstract class DungeonEntrance
{
	public DungeonEntranceSettings settings;

	public bool calculated;

	public bool generated;

	public DungeonBounds Bounds = new DungeonBounds();

	public Point OldManSpawn;

	public bool Processed
	{
		get
		{
			if (!calculated)
			{
				return generated;
			}
			return true;
		}
	}

	public DungeonEntrance(DungeonEntranceSettings settings)
	{
		this.settings = settings;
	}

	public abstract void CalculateEntrance(DungeonData data, int x, int y);

	public abstract bool GenerateEntrance(DungeonData data, int x, int y);

	public virtual bool CanGenerateFeatureAt(DungeonData data, IDungeonFeature feature, int x, int y)
	{
		if (feature is DungeonGlobalBiomeChests)
		{
			return false;
		}
		return true;
	}
}
