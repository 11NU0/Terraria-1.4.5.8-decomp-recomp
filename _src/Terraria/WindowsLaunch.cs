using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using Terraria.Social;

namespace Terraria;

public static class WindowsLaunch
{
	public delegate bool HandlerRoutine(CtrlTypes ctrlType);

	public enum CtrlTypes
	{
		CTRL_C_EVENT = 0,
		CTRL_BREAK_EVENT = 1,
		CTRL_CLOSE_EVENT = 2,
		CTRL_LOGOFF_EVENT = 5,
		CTRL_SHUTDOWN_EVENT = 6
	}

	private static HandlerRoutine _handleRoutine;

	private static bool ConsoleCtrlCheck(CtrlTypes ctrlType)
	{
		bool flag = false;
		switch (ctrlType)
		{
		case CtrlTypes.CTRL_C_EVENT:
			flag = true;
			break;
		case CtrlTypes.CTRL_BREAK_EVENT:
			flag = true;
			break;
		case CtrlTypes.CTRL_CLOSE_EVENT:
			flag = true;
			break;
		case CtrlTypes.CTRL_LOGOFF_EVENT:
		case CtrlTypes.CTRL_SHUTDOWN_EVENT:
			flag = true;
			break;
		}
		if (flag)
		{
			SocialAPI.Shutdown();
		}
		return true;
	}

	[DllImport("Kernel32")]
	public static extern bool SetConsoleCtrlHandler(HandlerRoutine handler, bool add);

	[STAThread]
	private static void Main(string[] args)
	{
		AppDomain.CurrentDomain.AssemblyResolve += (object sender, ResolveEventArgs sargs) =>
		{
			string resourceName = new AssemblyName(sargs.Name).Name + ".dll";
			string text = Array.Find(typeof(Program).Assembly.GetManifestResourceNames(), (string element) => element.EndsWith(resourceName));
			if (text == null)
			{
				return (Assembly)null;
			}
			using Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(text);
			byte[] array = new byte[stream.Length];
			stream.Read(array, 0, array.Length);
			return Assembly.Load(array);
		};
		Program.LaunchGame(args);
	}
}
