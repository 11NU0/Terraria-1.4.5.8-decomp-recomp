using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using ReLogic.OS;
using Terraria.GameContent.UI.States;
using Terraria.IO;
using Terraria.Localization;
using Terraria.Utilities;
using Terraria.WorldBuilding;

namespace Terraria.Testing;

public abstract class QuickLoad
{
	public class JoinWorld : QuickLoad
	{
		public Rectangle? WindowedBounds;

		public string PlayerPath;

		public string WorldPath;

		public string ServerIPText;

		public string ServerPassword;

		public bool IsHostAndPlay;

		public List<JoinWorld> ExtraClients = new List<JoinWorld>();

		public Vector2 PlayerPosition;

		public int MountType;

		public bool InDebugRegenUI;

		public string RegenTargetPassName;

		public WorldGenerator.SnapshotFrequency RegenSnapshotFrequency;

		public bool RegenPauseOnHashMismatch;

		protected override void Start()
		{
			if (Program.LaunchParameters.TryGetValue("-quickloadclient", out var value) && int.TryParse(value, out var result) && result < ExtraClients.Count)
			{
				ExtraClients[result].Start();
				return;
			}
			if (ExtraClients != null)
			{
				LaunchExtraClients();
			}
			RestoreWindowBounds();
			SelectPlayerAndWorld();
			PlayWorldOrJoinServer();
		}

		private void RestoreWindowBounds()
		{
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			if (WindowedBounds.HasValue)
			{
				Rectangle value = WindowedBounds.Value;
				if (Platform.Get<IWindowService>().IsSizeable(((Game)Main.instance).Window))
				{
					Main.SetResolution(value.Width, value.Height);
					Platform.Get<IWindowService>().SetPosition(((Game)Main.instance).Window, value.X, value.Y);
				}
			}
		}

		private void SaveWindowBounds()
		{
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			if (Platform.Get<IWindowService>().IsSizeable(((Game)Main.instance).Window))
			{
				WindowedBounds = Platform.Get<IWindowService>().GetBounds(((Game)Main.instance).Window);
			}
		}

		private void LaunchExtraClients()
		{
			for (int i = 0; i < ExtraClients.Count; i++)
			{
				Process.Start(Process.GetCurrentProcess().ProcessName, "-quickloadclient " + i);
			}
		}

		protected void SelectPlayerAndWorld()
		{
			Main.LoadPlayers();
			Main.SelectPlayer(Main.PlayerList.Single((PlayerFileData p) => p.Path == PlayerPath));
			if (!string.IsNullOrEmpty(WorldPath))
			{
				Main.LoadWorlds();
				Main.WorldList.Single((WorldFileData w) => w.Path == WorldPath).SetAsActive();
			}
		}

		protected void PlayWorldOrJoinServer()
		{
			WorldGen.Hooks.OnWorldLoad += OnWorldLoad;
			Main.menuMode = 10;
			if (ServerIPText != null)
			{
				Netplay.ServerPassword = ServerPassword;
				if (IsHostAndPlay)
				{
					Main.HostAndPlay();
					return;
				}
				Main.autoPass = true;
				Netplay.SetRemoteIP(ServerIPText);
				Netplay.StartTcpClient();
				Main.statusText = Language.GetTextValue("Net.ConnectingTo", ServerIPText);
			}
			else
			{
				WorldGen.playWorld();
			}
		}

		private void OnWorldLoad()
		{
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0070: Unknown result type (might be due to invalid IL or missing references)
			//IL_0082: Unknown result type (might be due to invalid IL or missing references)
			//IL_0087: Unknown result type (might be due to invalid IL or missing references)
			WorldGen.Hooks.OnWorldLoad -= OnWorldLoad;
			if (!(Main.ActiveWorldFileData.Path != WorldPath) && !(Main.ActivePlayerFileData.Path != PlayerPath) && (ServerIPText == null || Main.netMode == 1) && (Main.netMode != 1 || !(ServerIPText != Netplay.ServerIPText)))
			{
				if (PlayerPosition != Vector2.Zero)
				{
					Main.LocalPlayer.position = PlayerPosition;
					Main.LocalPlayer.ResetFallDamage();
				}
				if (MountType != 0)
				{
					Main.LocalPlayer.mount.SetMount(MountType, Main.LocalPlayer);
				}
				DebugUtils.QuickSPMessage("/onquickload");
			}
		}

		public virtual JoinWorld WithCurrentState()
		{
			//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
			SaveWindowBounds();
			Main.SaveSettings();
			if (!Main.gameMenu)
			{
				Player.SavePlayer(Main.ActivePlayerFileData);
			}
			if (Main.WorldFileMetadata == null)
			{
				Main.WorldFileMetadata = FileMetadata.FromCurrentSettings(FileType.World);
			}
			if (Main.netMode != 1)
			{
				WorldFile.SaveWorld();
			}
			PlayerPath = Main.ActivePlayerFileData.Path;
			WorldPath = Main.ActiveWorldFileData.Path;
			if (Main.netMode == 1)
			{
				ServerIPText = Netplay.ServerIPText;
				ServerPassword = Netplay.ServerPassword;
				IsHostAndPlay = Netplay.IsHostAndPlay;
				if (IsHostAndPlay)
				{
					NetMessage.SendData(94, -1, -1, NetworkText.FromLiteral("/quickload-clientprobe"));
				}
			}
			if (!Main.gameMenu)
			{
				PlayerPosition = Main.LocalPlayer.position;
				MountType = Main.LocalPlayer.mount.Type;
			}
			InDebugRegenUI = UIWorldGenDebug.ActiveInstance != null;
			if (InDebugRegenUI)
			{
				if (UIWorldGenDebug.CurrentTargetOrLatestPass != null)
				{
					RegenTargetPassName = UIWorldGenDebug.CurrentTargetOrLatestPass.Name;
				}
				RegenSnapshotFrequency = WorldGenerator.CurrentController.SnapshotFrequency;
				RegenPauseOnHashMismatch = WorldGenerator.CurrentController.PauseOnHashMismatch;
			}
			return this;
		}

		protected WorldGenerator.Controller CreateRegenController()
		{
			WorldGen.PrepForRegen();
			return new WorldGenerator.Controller(WorldGen.Manifest)
			{
				Paused = (InDebugRegenUI && RegenTargetPassName == null),
				SnapshotFrequency = RegenSnapshotFrequency,
				PauseOnHashMismatch = RegenPauseOnHashMismatch,
				OnPassesLoaded = (WorldGenerator.Controller c) =>
				{
					c.PauseAfterPass = c.Passes.FirstOrDefault((GenPass p) => p.Name == RegenTargetPassName);
					if (c.PauseAfterPass != null)
					{
						c.TryRunToEndOfPass(c.PauseAfterPass);
					}
				}
			};
		}
	}

	public class RegenWorld : JoinWorld
	{
		protected override void Start()
		{
			SelectPlayerAndWorld();
			GenerateWorld();
		}

		private void GenerateWorld()
		{
			WorldGen.CreateNewWorld(null, CreateRegenController(), OnGenerationFinished);
		}

		private void OnGenerationFinished(bool playable)
		{
			if (playable)
			{
				InDebugRegenUI = false;
				PlayWorldOrJoinServer();
			}
		}
	}

	private static readonly string FilePath = Path.Combine(Main.SavePath, "dev-quickload.json");

	private static readonly JsonSerializerSettings SerializerSettings = new JsonSerializerSettings
	{
		ContractResolver = (IContractResolver)(object)new EasyDeserializationJsonContractResolver(),
		TypeNameHandling = (TypeNameHandling)4
	};

	private static QuickLoad _loadedConfig;

	public static bool QuickLoading => _loadedConfig != null;

	protected abstract void Start();

	public static void Load()
	{
		if (TryRead(out _loadedConfig) && ShiftHeld())
		{
			_loadedConfig = null;
			Platform.Get<IWindowService>().Activate(((Game)Main.instance).Window);
			if (MessageBox.Show("Quick Load skipped. Do you want to delete the configuration?", "", MessageBoxButtons.YesNo) == DialogResult.Yes)
			{
				Clear();
			}
		}
	}

	public static void OnContentLoaded()
	{
		try
		{
			if (_loadedConfig != null)
			{
				_loadedConfig.Start();
			}
		}
		catch (Exception ex)
		{
			if (MessageBox.Show("Do you want to delete the configuration?\n\n" + ex, "Quickload Error", MessageBoxButtons.YesNo, MessageBoxIcon.Error) == DialogResult.Yes)
			{
				Clear();
			}
		}
	}

	public static QuickLoad Deserialize(string json)
	{
		return JsonConvert.DeserializeObject<QuickLoad>(json, SerializerSettings);
	}

	public static string Serialize(QuickLoad config)
	{
		return JsonConvert.SerializeObject((object)config, typeof(QuickLoad), SerializerSettings);
	}

	public static bool TryRead(out QuickLoad config)
	{
		config = null;
		try
		{
			if (!File.Exists(FilePath))
			{
				return false;
			}
			config = Deserialize(File.ReadAllText(FilePath));
			return true;
		}
		catch (Exception)
		{
			return false;
		}
	}

	public static void Set(QuickLoad config)
	{
		File.WriteAllText(FilePath, Serialize(config));
	}

	public static void Clear()
	{
		if (File.Exists(FilePath))
		{
			File.Delete(FilePath);
		}
	}

	private static bool ShiftHeld()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		if (Keyboard.GetState().PressingShift())
		{
			return true;
		}
		try
		{
			if (Platform.IsWindows)
			{
				return ShiftHeldWin();
			}
			if (Platform.IsOSX)
			{
				return ShiftHeldOSX();
			}
			if (Platform.IsLinux)
			{
				return ShiftHeldX11();
			}
		}
		catch (Exception)
		{
		}
		return false;
	}

	[DllImport("user32.dll")]
	private static extern short GetAsyncKeyState(int vKey);

	private static bool ShiftHeldWin()
	{
		if ((GetAsyncKeyState(160) & 0x8000) == 0)
		{
			return (GetAsyncKeyState(161) & 0x8000) != 0;
		}
		return true;
	}

	[DllImport("/System/Library/Frameworks/CoreGraphics.framework/CoreGraphics")]
	private static extern ulong CGEventSourceFlagsState(uint stateID);

	private static bool ShiftHeldOSX()
	{
		return (CGEventSourceFlagsState(1u) & 0x20000) != 0;
	}

	[DllImport("libX11")]
	private static extern IntPtr XOpenDisplay(IntPtr display);

	[DllImport("libX11")]
	private static extern void XCloseDisplay(IntPtr display);

	[DllImport("libX11")]
	private static extern int XQueryKeymap(IntPtr display, byte[] keys_return);

	[DllImport("libX11")]
	private static extern int XKeysymToKeycode(IntPtr display, ulong keysym);

	private static bool ShiftHeldX11()
	{
		IntPtr intPtr = XOpenDisplay(IntPtr.Zero);
		if (intPtr == IntPtr.Zero)
		{
			return false;
		}
		try
		{
			int num = XKeysymToKeycode(intPtr, 65505uL);
			int num2 = XKeysymToKeycode(intPtr, 65506uL);
			byte[] array = new byte[32];
			XQueryKeymap(intPtr, array);
			bool flag = (array[num / 8] & (1 << num % 8)) != 0;
			bool flag2 = (array[num2 / 8] & (1 << num2 % 8)) != 0;
			return flag | flag2;
		}
		finally
		{
			XCloseDisplay(intPtr);
		}
	}

	static QuickLoad()
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Expected Obj, but got Unknown
	}
}
