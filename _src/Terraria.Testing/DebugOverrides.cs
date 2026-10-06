using System.Collections.Generic;
using System.Diagnostics;
using ReLogic.Content;

namespace Terraria.Testing;

internal class DebugOverrides
{
	public static Dictionary<string, double> Overrides;

	private static readonly FileChangeWatcher<int> _changeWatcher = new FileChangeWatcher<int>();

	private static uint lastWatcherCheck = Main.GameUpdateCount;

	[Conditional("DEBUG")]
	public static void Replace(string key, ref int value)
	{
		double num = value;
		value = (int)num;
	}

	[Conditional("DEBUG")]
	public static void Replace(string key, ref float value)
	{
		double num = value;
		value = (float)num;
	}

	[Conditional("DEBUG")]
	public static void Replace(string key, ref double value)
	{
		Update();
		if (Overrides.TryGetValue(key, out var value2))
		{
			value = value2;
		}
	}

	[Conditional("DEBUG")]
	public static void Set(string key, double value)
	{
		Init();
		Overrides[key] = value;
		Save();
	}

	private static void Init()
	{
	}

	private static void Update()
	{
		Init();
	}

	private static void Save()
	{
	}
}
