using System;
using System.Reflection;

namespace Terraria.Utilities;

public static class NewRuntimeMethods
{
	private static bool IsNet45OrNewer = Type.GetType("System.Reflection.ReflectionContext", throwOnError: false) != null;

	private static MethodInfo _collect;

	public static void GC_Collect(int generation, GCCollectionMode mode, bool blocking)
	{
		if (IsNet45OrNewer)
		{
			_collect = _collect ?? typeof(GC).GetMethod("Collect", BindingFlags.Static | BindingFlags.Public, null, new Type[3]
			{
				typeof(int),
				typeof(GCCollectionMode),
				typeof(bool)
			}, null);
			_collect.Invoke(null, new object[3] { generation, mode, blocking });
		}
	}

	public static long GC_GetTotalAllocatedBytes()
	{
		return 0L;
	}

	public static TimeSpan GC_GetTotalPauseDuration()
	{
		return TimeSpan.Zero;
	}
}
