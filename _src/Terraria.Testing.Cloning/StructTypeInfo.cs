using System;
using System.Linq;

namespace Terraria.Testing.Cloning;

internal class StructTypeInfo
{
	public readonly bool CloneByValue;

	public StructTypeInfo(Type type)
	{
		CloneByValue = type.IsPrimitive || !DeepCloning.GetAllInstanceFields(type).Any(DeepCloning.NeedsFieldClone);
	}
}
