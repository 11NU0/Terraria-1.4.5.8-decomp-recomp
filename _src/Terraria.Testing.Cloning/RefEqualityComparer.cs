using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Terraria.Testing.Cloning;

internal sealed class RefEqualityComparer : EqualityComparer<object>
{
	public static readonly RefEqualityComparer Instance = new RefEqualityComparer();

	private RefEqualityComparer()
	{
	}

	public override bool Equals(object x, object y)
	{
		return x == y;
	}

	public override int GetHashCode(object obj)
	{
		return RuntimeHelpers.GetHashCode(obj);
	}
}
