using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using Microsoft.Xna.Framework;
using ReLogic.IO;
using ReLogic.OS;
using Terraria.Initializers;
using Terraria.Localization;
using Terraria.Social;
using Terraria.Utilities;

namespace Terraria;

public static class Program
{
	public static bool IsXna = true;

	public static bool IsFna = false;

	public static bool IsMono = Type.GetType("Mono.Runtime") != null;

	public static Dictionary<string, string> LaunchParameters = new Dictionary<string, string>();

	public static string SavePath;

	public const string TerrariaSaveFolderPath = "Terraria";

	private static int ThingsToLoad;

	private static int ThingsLoaded;

	public static bool LoadedEverything;

	public static IntPtr JitForcedMethodCache;

	public static float LoadedPercentage
	{
		get
		{
			if (ThingsToLoad == 0)
			{
				return 1f;
			}
			return (float)ThingsLoaded / (float)ThingsToLoad;
		}
	}

	public static void StartForceLoad()
	{
		if (!Main.SkipAssemblyLoad)
		{
			Thread thread = new Thread(ForceLoadThread);
			thread.IsBackground = true;
			thread.Start();
		}
		else
		{
			LoadedEverything = true;
		}
	}

	public static void ForceLoadThread(object threadContext)
	{
		ForceLoadAssembly(Assembly.GetExecutingAssembly(), initializeStaticMembers: true);
		LoadedEverything = true;
	}

	private static void ForceJITOnAssembly(Assembly assembly)
	{
		Type[] types = assembly.GetTypes();
		foreach (Type type in types)
		{
			MethodInfo[] array = (IsMono ? type.GetMethods() : type.GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic));
			foreach (MethodInfo methodInfo in array)
			{
				if (!methodInfo.IsAbstract && !methodInfo.ContainsGenericParameters && methodInfo.GetMethodBody() != null)
				{
					if (IsMono)
					{
						JitForcedMethodCache = methodInfo.MethodHandle.GetFunctionPointer();
					}
					else
					{
						RuntimeHelpers.PrepareMethod(methodInfo.MethodHandle);
					}
				}
			}
			ThingsLoaded++;
		}
	}

	private static void ForceStaticInitializers(Assembly assembly)
	{
		Type[] types = assembly.GetTypes();
		foreach (Type type in types)
		{
			if (!type.IsGenericType)
			{
				RuntimeHelpers.RunClassConstructor(type.TypeHandle);
			}
		}
	}

	private static void ForceLoadAssembly(Assembly assembly, bool initializeStaticMembers)
	{
		ThingsToLoad = assembly.GetTypes().Length;
		ForceJITOnAssembly(assembly);
		if (initializeStaticMembers)
		{
			ForceStaticInitializers(assembly);
		}
	}

	private static void ForceLoadAssembly(string name, bool initializeStaticMembers)
	{
		Assembly assembly = null;
		Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
		for (int i = 0; i < assemblies.Length; i++)
		{
			if (assemblies[i].GetName().Name.Equals(name))
			{
				assembly = assemblies[i];
				break;
			}
		}
		if (assembly == null)
		{
			assembly = Assembly.Load(name);
		}
		ForceLoadAssembly(assembly, initializeStaticMembers);
	}

	private static void SetupLogging()
	{
		if (LaunchParameters.ContainsKey("-logfile"))
		{
			string text = LaunchParameters["-logfile"];
			text = ((text != null && !(text.Trim() == "")) ? Path.Combine(text, $"Log_{DateTime.Now:yyyyMMddHHmmssfff}.log") : Path.Combine(SavePath, "Logs", $"Log_{DateTime.Now:yyyyMMddHHmmssfff}.log"));
			ConsoleOutputMirror.ToFile(text);
		}
		CrashWatcher.Inititialize();
		CrashWatcher.DumpOnException = LaunchParameters.ContainsKey("-minidump");
		CrashWatcher.LogAllExceptions = LaunchParameters.ContainsKey("-logerrors");
		if (LaunchParameters.ContainsKey("-fulldump"))
		{
			Console.WriteLine("Full Dump logs enabled.");
			CrashWatcher.EnableCrashDumps(CrashDump.Options.WithFullMemory);
		}
	}

	private static void InitializeConsoleOutput()
	{
		if (Debugger.IsAttached)
		{
			return;
		}
		try
		{
			Console.OutputEncoding = Encoding.UTF8;
			if (Platform.IsWindows)
			{
				Console.InputEncoding = Encoding.Unicode;
			}
			else
			{
				Console.InputEncoding = Encoding.UTF8;
			}
		}
		catch
		{
		}
	}

	public static void LaunchGame(string[] args, bool monoArgs = false)
	{
		Thread.CurrentThread.Name = "Main Thread";
		if (monoArgs)
		{
			args = Utils.ConvertMonoArgsToDotNet(args);
		}
		LogFNANativeLibVersions();
		LaunchParameters = Utils.ParseArguements(args);
		SavePath = (LaunchParameters.ContainsKey("-savedirectory") ? LaunchParameters["-savedirectory"] : Platform.Get<IPathService>().GetStoragePath("Terraria"));
		ThreadPool.SetMinThreads(8, 8);
		InitializeConsoleOutput();
		SetupLogging();
		Platform.Get<IWindowService>().SetQuickEditEnabled(false);
		RunGame();
	}

	public static void RunGame()
	{
		LanguageManager.Instance.SetLanguage(GameCulture.DefaultCulture);
		if (Platform.IsOSX)
		{
			Main.OnEngineLoad += () =>
			{
				((Game)Main.instance).IsMouseVisible = false;
			};
		}
		else if (Platform.IsWindows)
		{
			Main.OnEngineLoad += () =>
			{
				IMouseNotifier val = Platform.Get<IMouseNotifier>();
				if (val != null)
				{
					val.AddMouseHandler((Action<bool>)((bool connected) =>
					{
						if (connected)
						{
							((Game)Main.instance).IsMouseVisible = true;
							Main.instance.ReHideCursor = true;
						}
					}));
				}
			};
		}
		Main main = new Main();
		try
		{
			Lang.InitializeLegacyLocalization();
			SocialAPI.Initialize();
			LaunchInitializer.LoadParameters(main);
			Main.OnEnginePreload += StartForceLoad;
			if (Main.dedServ)
			{
				main.DedServ();
			}
			((Game)main).Run();
		}
		catch (Exception e)
		{
			DisplayException(e);
		}
		finally
		{
			((IDisposable)main)?.Dispose();
		}
	}

	private static void LogFNANativeLibVersions()
	{
	}

	private static void DisplayException(Exception e)
	{
		try
		{
			string text = e.ToString();
			if (WorldGen.isGeneratingOrLoadingWorld)
			{
				try
				{
					text = $"Creating world - Seed: {Main.ActiveWorldFileData.SeedText} Width: {Main.maxTilesX}, Height: {Main.maxTilesY}, Evil: {WorldGen.WorldGenParam_Evil}, IsExpert: {Main.expertMode}\n{text}";
				}
				catch
				{
				}
			}
			using (StreamWriter streamWriter = new StreamWriter("client-crashlog.txt", append: true))
			{
				streamWriter.WriteLine(DateTime.Now);
				streamWriter.WriteLine(text);
				streamWriter.WriteLine("");
			}
			if (Main.dedServ)
			{
				Console.WriteLine(Language.GetTextValue("Error.ServerCrash"), DateTime.Now, text);
			}
			MessageBox.Show(text, "Terraria: Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
		}
		catch
		{
		}
	}
}
