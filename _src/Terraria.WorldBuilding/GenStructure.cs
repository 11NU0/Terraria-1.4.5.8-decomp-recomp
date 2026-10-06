using Microsoft.Xna.Framework;

namespace Terraria.WorldBuilding;

public abstract class GenStructure : GenBase
{
	public virtual bool Place(Point origin, StructureMap structures)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		return Place(origin, structures, null);
	}

	public abstract bool Place(Point origin, StructureMap structures, GenerationProgress progress);
}
