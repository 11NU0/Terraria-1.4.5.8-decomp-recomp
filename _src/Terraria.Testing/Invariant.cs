#define TRACE
using System;
using System.Diagnostics;
using Terraria.Chat;
using Terraria.Localization;
using Terraria.Utilities;

namespace Terraria.Testing;

public static class Invariant
{
	public static bool assertionsEnabled;

	public static void Assert(bool condition, string message)
	{
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		if (condition)
		{
			return;
		}
		if (assertionsEnabled)
		{
			if (!Main.dedServ || Debugger.IsAttached || !CrashWatcher.LogAllExceptions)
			{
				Trace.Assert(condition: false, message);
				return;
			}
			WorldGen.BroadcastText(NetworkText.FromLiteral("Invariant Failed: " + message), ChatColors.Error);
		}
		if (CrashWatcher.LogAllExceptions)
		{
			try
			{
				throw new Exception("Invariant Failed: " + message);
			}
			catch (Exception)
			{
				return;
			}
		}
		Console.Write("Invariant Failed: ");
		Console.WriteLine(message);
	}

	public static void Assert<T>(bool condition, string format, T arg1)
	{
		if (!condition)
		{
			Assert(condition: false, string.Format(format, arg1));
		}
	}

	public static void Assert<T1, T2>(bool condition, string format, T1 arg1, T2 arg2)
	{
		if (!condition)
		{
			Assert(condition: false, string.Format(format, arg1, arg2));
		}
	}

	public static void Assert<T1, T2, T3>(bool condition, string format, T1 arg1, T2 arg2, T3 arg3)
	{
		if (!condition)
		{
			Assert(condition: false, string.Format(format, arg1, arg2, arg3));
		}
	}
}
