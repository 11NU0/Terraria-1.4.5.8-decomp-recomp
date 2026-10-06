using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Microsoft.Xna.Framework;
using ReLogic.OS;
using Terraria.Chat;
using Terraria.GameContent;
using Terraria.GameContent.UI.States;
using Terraria.ID;
using Terraria.IO;
using Terraria.Localization;
using Terraria.Map;
using Terraria.Net.Sockets;
using Terraria.UI;
using Terraria.Utilities;
using Terraria.WorldBuilding;

namespace Terraria.Testing.ChatCommands;

public static class ToolkitDebugCommands
{
	private delegate void CommandImpl(out string reply);

	private static IEnumerable<Point> _findNextEnumerable;

	private static IEnumerator<Point> FindNextEnumerator;

	public static IEnumerable<Point> FindNextEnumerable
	{
		get
		{
			return _findNextEnumerable;
		}
		set
		{
			_findNextEnumerable = value;
			FindNextEnumerator = null;
		}
	}

	[DebugCommand("hh", "Opens a list of all the debug commands", CommandRequirement.Client)]
	public static bool HelpCommand(DebugMessage message)
	{
		IngameFancyUI.OpenUIState(new UIDebugCommandsList());
		return true;
	}

	[DebugCommand("memo", "Creates a shortcut command with a given name. Opens the file to write in. One command per line. Accepts arg substitions ({0}, {1}, etc)", CommandRequirement.Client, HelpText = "Usage: /memo <custom-command-name>")]
	public static bool MemoCommand(DebugMessage message)
	{
		if (string.IsNullOrWhiteSpace(message.Arguments) || message.Arguments.Contains(" "))
		{
			return false;
		}
		DebugCommandProcessor.OpenMemo(message.Arguments);
		return true;
	}

	[DebugCommand("memonum", "Creates a memo for a numpad key (0-9). Shorthand for /memo numpad{i}", CommandRequirement.Client, HelpText = "Usage: /memonum <0-9>")]
	public static bool MemoNumCommand(DebugMessage message)
	{
		if (!int.TryParse(message.Arguments, out var result) || result < 0 || result > 9)
		{
			message.ReplyError("Invalid numpad key number");
			return false;
		}
		DebugCommandProcessor.OpenMemo("numpad" + result);
		return true;
	}

	[DebugCommand("setserverping", "Sets a target ping for all players on the server. Clients will automatically adjust /latency to achieve it.", CommandRequirement.MultiplayerRPC, HelpText = "Usage: /setserverping <ms>")]
	public static bool SetServerPingCommand(DebugMessage message)
	{
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		if (!int.TryParse(message.Arguments, out var result) || result < 0 || result > 10000)
		{
			return false;
		}
		DebugOptions.Shared_ServerPing = result;
		ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral($"Target ping set {result}ms"), new Color(250, 250, 0));
		NetMessage.SendData(94, -1, -1, NetworkText.FromLiteral("/setserverping"), 0, DebugOptions.Shared_ServerPing);
		return true;
	}

	[DebugCommand("latency", "Adds latency to incoming and outgoing packets sent by this client.", CommandRequirement.MultiplayerClient, HelpText = "Usage: /latency <ms>")]
	public static bool LatencyCommand(DebugMessage message)
	{
		uint result = 0u;
		if (!uint.TryParse(message.Arguments, out result))
		{
			return false;
		}
		DebugNetworkStream.Latency = result;
		message.Reply($"Latency set to {result}ms");
		return true;
	}

	[DebugCommand("setdrawwait", "Sets a fixed waiting period to occur during each engine draw call.", CommandRequirement.Client, HelpText = "Usage: /setdrawwait <delay in ms>")]
	public static bool SetDrawWaitCommand(DebugMessage message)
	{
		if (!double.TryParse(message.Arguments, out var result) || result < 0.0 || result > 100.0)
		{
			return false;
		}
		DebugOptions.DrawWaitInMs = result;
		message.Reply($"Draw wait time set to {result}ms");
		return true;
	}

	[DebugCommand("setupdatewait", "Sets a fixed waiting period to occur during each engine update call.", CommandRequirement.Client, HelpText = "Usage: /setupdatewait <delay in ms>")]
	public static bool SetUpdateWaitCommand(DebugMessage message)
	{
		if (!double.TryParse(message.Arguments, out var result) || result < 0.0 || result > 100.0)
		{
			return false;
		}
		DebugOptions.UpdateWaitInMs = result;
		message.Reply($"Update wait time set to {result}ms");
		return true;
	}

	[DebugCommand("toggleinactivewait", "Toggles main thread sleeping when window is unfocused (this setting is saved).", CommandRequirement.Client)]
	public static bool ToggleInactiveWait(DebugMessage message)
	{
		Main.ThrottleWhenInactive = !Main.ThrottleWhenInactive;
		Main.SaveSettings();
		message.Reply("Inactive CPU throttling " + (Main.ThrottleWhenInactive ? "enabled" : "disabled"));
		return true;
	}

	[DebugCommand("togglecore0pinning", "Toggles core 0 unpinning on load (this setting is saved).", CommandRequirement.Client)]
	public static bool ToggleCore0Pinning(DebugMessage message)
	{
		if (!Platform.IsWindows)
		{
			message.ReplyError("Only supported on windows");
			return false;
		}
		Main.UnpinFromCore0 = !Main.UnpinFromCore0;
		Main.SaveSettings();
		message.Reply("Core 0 pinning " + ((!Main.UnpinFromCore0) ? "enabled" : "disabled"));
		WindowsPerformanceDiagnostics.SetCore0Affinity(!Main.UnpinFromCore0);
		return true;
	}

	[DebugCommand("quickload", "Automatically rejoin this world/server with this player whenever the game is launched. Executes /onquickload memo when joining the world. Will relaunch all local clients when used with  Host & Play", CommandRequirement.Client, HelpText = "/quickload  [stop]")]
	public static bool QuickLoadRejoinCommand(DebugMessage message)
	{
		switch (message.Arguments.ToLowerInvariant().Trim())
		{
		case "stop":
		case "disable":
		case "clear":
		case "cancel":
		case "exit":
			QuickLoad.Clear();
			message.Reply("Quick Load configuration cleared.");
			return true;
		default:
			QuickLoad.Set(new QuickLoad.JoinWorld().WithCurrentState());
			message.Reply("Quick Load configuration set. Hold shift while loading to clear it.");
			return true;
		}
	}

	[DebugCommand("quickload-regen", "Automatically regenerate this world whenever the game is launched. Executes /onquickload memo when joining the world", CommandRequirement.SinglePlayer)]
	public static bool QuickLoadRegenCommand(DebugMessage message)
	{
		QuickLoad.Set(new QuickLoad.RegenWorld().WithCurrentState());
		message.Reply("Quick Load configuration set. Hold shift while loading to clear it.");
		return true;
	}

	[DebugCommand("light", "Toggles the lighting system between active and fullbright.", CommandRequirement.Client)]
	public static bool LightCommand(DebugMessage message)
	{
		if (DebugOptions.devLightTilesCheat)
		{
			DebugOptions.devLightTilesCheat = false;
			message.Reply("Lighting enabled");
		}
		else
		{
			DebugOptions.devLightTilesCheat = true;
			message.Reply("Lighting disabled");
		}
		return true;
	}

	[DebugCommand("nolimits", "No border restrictions", CommandRequirement.Client)]
	public static bool NoLimitsCommand(DebugMessage message)
	{
		if (DebugOptions.noLimits)
		{
			DebugOptions.noLimits = false;
			message.Reply("No limits disabled");
		}
		else
		{
			DebugOptions.noLimits = true;
			message.Reply("No limits enabled");
		}
		return true;
	}

	[DebugCommand("save", "Save the player (and the world if single player)", CommandRequirement.Client)]
	public static bool SaveCommand(DebugMessage message)
	{
		Player.SavePlayer(Main.ActivePlayerFileData);
		if (Main.netMode == 0)
		{
			WorldFile.SaveWorld();
			message.Reply("Player and world saved!");
		}
		else
		{
			message.Reply("Player saved!");
		}
		return true;
	}

	[DebugCommand("reload", "Reloads the last save", CommandRequirement.SinglePlayer)]
	public static bool ReloadCommand(DebugMessage message)
	{
		Stopwatch stopwatch = Stopwatch.StartNew();
		WorldFile.LoadWorld();
		Main.sectionManager.SetAllSectionsLoaded();
		message.Reply($"Reloaded in {stopwatch.ElapsedMilliseconds}ms");
		return true;
	}

	[DebugCommand("quit", "Exit world without saving.", CommandRequirement.Client)]
	public static bool QuitCommand(DebugMessage message)
	{
		WorldGen.JustQuit();
		return true;
	}

	[DebugCommand("reloadpacks", "Reloads resource packs.", CommandRequirement.Client)]
	public static bool ReloadPacksCommand(DebugMessage message)
	{
		Main.instance.ResetAllContentBasedRenderTargets();
		Main.AssetSourceController.Refresh();
		message.Reply("Resource Packs Reloaded.");
		return true;
	}

	[DebugCommand("frame", "Resets all frame data", CommandRequirement.Client)]
	public static bool FrameCommand(DebugMessage message)
	{
		Main.sectionManager.SetAllSectionsLoaded();
		message.Reply("World frame data cleared");
		return true;
	}

	[DebugCommand("hash", "Prints out the hash of all saved (non-volatile) tile data", CommandRequirement.AnyAuthority)]
	public static bool HashCommand(DebugMessage message)
	{
		message.Reply($"Tile data hash: {WorldGenerator.HashWorld():X8}");
		return true;
	}

	[DebugCommand("snapshot", "Creates a snapshot of the current tile state for the world.", CommandRequirement.AnyAuthority)]
	public static bool SnapshotCommand(DebugMessage message)
	{
		TileSnapshot.Create();
		message.Reply("Tile Snapshot Created.");
		return true;
	}

	[DebugCommand("snapclear", "Clears previously created snapshot.", CommandRequirement.AnyAuthority)]
	public static bool SnapshotClearCommand(DebugMessage message)
	{
		TileSnapshot.Clear();
		message.Reply("Tile Snapshot Cleared.");
		return true;
	}

	[DebugCommand("snapsave", "Saves a snapshot in dev-snapshots.", CommandRequirement.AnyAuthority, HelpText = "Usage: /snapsave <name>")]
	public static bool SnapshotSaveCommand(DebugMessage message)
	{
		if (string.IsNullOrWhiteSpace(message.Arguments))
		{
			message.ReplyError("Snapshot name required");
			return true;
		}
		if (!TileSnapshot.IsCreated)
		{
			TileSnapshot.Create();
		}
		Directory.CreateDirectory(Path.Combine(Main.SavePath, "dev-snapshots"));
		TileSnapshot.Save(Path.Combine(Main.SavePath, "dev-snapshots", message.Arguments + ".gensnapshot"));
		message.Reply("Tile Snapshot Saved to dev-snapshots/" + message.Arguments + ".gensnapshot");
		return true;
	}

	[DebugCommand("snapload", "Loads a snapshot in dev-snapshots.", CommandRequirement.AnyAuthority, HelpText = "Usage: /snapsave <name>")]
	public static bool SnapshotLoadCommand(DebugMessage message)
	{
		if (string.IsNullOrWhiteSpace(message.Arguments))
		{
			message.ReplyError("Snapshot name required");
			return true;
		}
		if (!TileSnapshot.IsCreated)
		{
			TileSnapshot.Create();
		}
		string path = Path.Combine(Main.SavePath, "dev-snapshots", message.Arguments + ".gensnapshot");
		if (!File.Exists(path))
		{
			message.ReplyError("File not found: dev-snapshots/" + message.Arguments + ".gensnapshot");
			return true;
		}
		TileSnapshot.Load(path);
		message.Reply("Tile Snapshot Loaded. Use /swap or /restore to apply.");
		return true;
	}

	[DebugCommand("restore", "Restores the world's tiles to the previously created snapshot.", CommandRequirement.AnyAuthority)]
	public static bool RestoreCommand(DebugMessage message)
	{
		if (!TileSnapshot.IsCreated)
		{
			message.ReplyError("No snapshot to restore");
		}
		else if (!TileSnapshot.SizeMatches)
		{
			message.ReplyError("Tile snapshot does not match current world size");
		}
		else
		{
			Stopwatch stopwatch = Stopwatch.StartNew();
			TileSnapshot.Restore();
			message.Reply($"Tile snapshot restored in {stopwatch.ElapsedMilliseconds}ms");
		}
		return true;
	}

	[DebugCommand("swap", "Swaps the world's tiles with the previously created snapshot.", CommandRequirement.AnyAuthority)]
	public static bool SwapCommand(DebugMessage message)
	{
		if (!TileSnapshot.IsCreated)
		{
			message.ReplyError("No snapshot to restore");
		}
		else if (!TileSnapshot.SizeMatches)
		{
			message.ReplyError("Tile snapshot does not match current world size");
		}
		else
		{
			Stopwatch stopwatch = Stopwatch.StartNew();
			TileSnapshot.Swap();
			message.Reply($"Tile snapshot swapped in {stopwatch.ElapsedMilliseconds}ms");
		}
		return true;
	}

	[DebugCommand("snapshotdiff", "Finds differences between the current map and saved snapshot. Use /next to skip through them.", CommandRequirement.SinglePlayer)]
	public static bool SnapshotDiffCommand(DebugMessage message)
	{
		if (!TileSnapshot.IsCreated)
		{
			message.ReplyError("No snapshot to compare");
		}
		else if (!TileSnapshot.SizeMatches)
		{
			message.ReplyError("Tile snapshot does not match current world size");
		}
		else
		{
			FindNextEnumerable = TileSnapshot.Compare();
			FindNext();
		}
		return true;
	}

	[DebugCommand("find", "Iterates through all instances of a tile in the world. Use /next to skip through them", CommandRequirement.Client, HelpText = "Usage: /find <id>")]
	public static bool FindCommand(DebugMessage message)
	{
		if (!int.TryParse(message.Arguments, out var tileType))
		{
			return false;
		}
		if (tileType < 0 || tileType >= TileID.Count)
		{
			return false;
		}
		string text = string.Empty;
		if (MapHelper.HasOption(tileType, 0))
		{
			text = Lang.GetMapObjectName(MapHelper.TileToLookup(tileType, 0));
		}
		if (text == string.Empty)
		{
			text = "#" + tileType;
		}
		FindNextEnumerable = FindTiles(message, (Tile t) => t.type == tileType, "Tile " + text, 3);
		FindNext();
		return true;
	}

	[DebugCommand("findwall", "Iterates through all instances of a wall in the world. Use /next to skip through them", CommandRequirement.Client, HelpText = "Usage: /findwall <id>")]
	public static bool FindWallCommand(DebugMessage message)
	{
		if (!int.TryParse(message.Arguments, out var wallType))
		{
			return false;
		}
		if (wallType <= 0 || wallType >= WallID.Count)
		{
			return false;
		}
		FindNextEnumerable = FindTiles(message, (Tile t) => t.wall == wallType, "Wall #" + wallType, 10);
		FindNext();
		return true;
	}

	[DebugCommand("next", "Finds the next instance of a tile/wall/object. Requires calling /find, /findwall or /snapshotdiff first", CommandRequirement.Client)]
	public static bool NextCommand(DebugMessage message)
	{
		if (FindNextEnumerable == null)
		{
			message.ReplyError("Scan not started. Nothing to find.");
			return true;
		}
		FindNext();
		return true;
	}

	public static void FindNext()
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		if (FindNextEnumerator == null)
		{
			FindNextEnumerator = FindNextEnumerable.GetEnumerator();
		}
		if (FindNextEnumerator.MoveNext())
		{
			GoToTile(FindNextEnumerator.Current);
		}
		else
		{
			FindNextEnumerator = null;
		}
	}

	private static IEnumerable<Point> FindTiles(DebugMessage message, Func<Tile, bool> predicate, string descriptor, int skipDistance)
	{
		Point lastPoint = Point.Zero;
		for (int x = 0; x < Main.maxTilesX; x++)
		{
			for (int y = 0; y < Main.maxTilesY; y++)
			{
				Tile tile = Main.tile[x, y];
				if (tile != null && predicate(tile) && (x - lastPoint.X >= skipDistance || Math.Abs(y - lastPoint.Y) >= skipDistance))
				{
					lastPoint = new Point(x, y);
					message.Reply(descriptor + " found at " + lastPoint);
					yield return lastPoint;
				}
			}
		}
		message.Reply(descriptor + " scan complete.");
	}

	private static void GoToTile(Point coordinates)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		Main.mapFullscreenPos = coordinates.ToVector2() + new Vector2(0.5f, 0.5f);
		Main.Pings.Add(Main.mapFullscreenPos);
	}

	[DebugCommand("showsections", "Toggles net section overlay.", CommandRequirement.Client)]
	public static bool ShowNetSectionsCommand(DebugMessage message)
	{
		DebugOptions.ShowSections = !DebugOptions.ShowSections;
		return true;
	}

	[DebugCommand("map", "Reveals the full map for the world.", CommandRequirement.Client, HelpText = "Usage: /map [pretty]")]
	public static bool MapCommand(DebugMessage message)
	{
		Main.clearMap = true;
		if (DebugOptions.unlockMap == 0)
		{
			DebugOptions.unlockMap = ((!(message.Arguments.ToLower().Trim() == "pretty")) ? 1 : 2);
			Main.refreshMap = true;
		}
		else
		{
			DebugOptions.unlockMap = 0;
		}
		return true;
	}

	[DebugCommand("clearmap", "Deletes the full map for the world.", CommandRequirement.Client)]
	public static bool ClearMapCommand(DebugMessage message)
	{
		Main.clearMap = true;
		Main.Map.Clear();
		Main.refreshMap = true;
		return true;
	}

	[DebugCommand("hideall", "Stops tiles, walls, and water from drawing", CommandRequirement.Client)]
	public static bool HideAll(DebugMessage message)
	{
		int num = 0;
		bool flag = false;
		if (!DebugOptions.hideTiles)
		{
			num++;
		}
		if (!DebugOptions.hideTiles2)
		{
			num++;
		}
		if (!DebugOptions.hideWalls)
		{
			num++;
		}
		if (!DebugOptions.hideWater)
		{
			num++;
		}
		if (num >= 2)
		{
			flag = true;
		}
		DebugOptions.hideTiles = flag;
		DebugOptions.hideTiles2 = flag;
		DebugOptions.hideWalls = flag;
		DebugOptions.hideWater = flag;
		if (flag)
		{
			message.Reply("Everything is hidden");
		}
		else
		{
			message.Reply("Everything is shown");
		}
		return true;
	}

	[DebugCommand("hidetiles", "Stops tiles from drawing on the screen", CommandRequirement.Client)]
	public static bool HideTiles(DebugMessage message)
	{
		DebugOptions.hideTiles = !DebugOptions.hideTiles;
		if (DebugOptions.hideTiles)
		{
			message.Reply("Tiles are hidden");
		}
		else
		{
			message.Reply("Tiles are shown");
		}
		return true;
	}

	[DebugCommand("hidetiles2", "Stops non-solid tiles from drawing on the screen", CommandRequirement.Client)]
	public static bool HideTiles2(DebugMessage message)
	{
		DebugOptions.hideTiles2 = !DebugOptions.hideTiles2;
		if (DebugOptions.hideTiles2)
		{
			message.Reply("Secondary tiles are hidden");
		}
		else
		{
			message.Reply("Secondary tiles are shown");
		}
		return true;
	}

	[DebugCommand("hidewalls", "Stops walls from drawing on the screen", CommandRequirement.Client)]
	public static bool HideWalls(DebugMessage message)
	{
		DebugOptions.hideWalls = !DebugOptions.hideWalls;
		if (DebugOptions.hideWalls)
		{
			message.Reply("Walls are hidden");
		}
		else
		{
			message.Reply("Walls are shown");
		}
		return true;
	}

	[DebugCommand("=", "repeats latest command", CommandRequirement.Client)]
	public static bool Repeat(DebugMessage message)
	{
		DebugCommandProcessor.RepeatLatestMessage();
		return true;
	}

	[DebugCommand("drawhitboxes", "Draws hitboxes on the screen", CommandRequirement.Client)]
	public static bool DrawHitboxes(DebugMessage message)
	{
		DebugOptions.DrawHitboxes = !DebugOptions.DrawHitboxes;
		if (DebugOptions.DrawHitboxes)
		{
			message.Reply("Hitboxes are on");
		}
		else
		{
			message.Reply("Hitboxes are hidden");
		}
		return true;
	}

	[DebugCommand("drawaggro", "Draws aggro on the screen", CommandRequirement.Client)]
	public static bool DrawAggro(DebugMessage message)
	{
		DebugOptions.DrawAggro = !DebugOptions.DrawAggro;
		if (DebugOptions.DrawAggro)
		{
			message.Reply("Aggro is on");
		}
		else
		{
			message.Reply("Aggro is hidden");
		}
		return true;
	}

	[DebugCommand("hidewater", "Stops water from drawing on the screen", CommandRequirement.Client)]
	public static bool HideWater(DebugMessage message)
	{
		DebugOptions.hideWater = !DebugOptions.hideWater;
		if (DebugOptions.hideWater)
		{
			message.Reply("Water is hidden");
		}
		else
		{
			message.Reply("Water is shown");
		}
		return true;
	}

	[DebugCommand("showunbreakablewalls", "Forces unbreakable walls to be visible even when covered by tiles", CommandRequirement.Client)]
	public static bool ShowUnbreakableWall(DebugMessage message)
	{
		DebugOptions.ShowUnbreakableWall = !DebugOptions.ShowUnbreakableWall;
		if (DebugOptions.ShowUnbreakableWall)
		{
			message.Reply("Unbreakable walls are shown");
		}
		else
		{
			message.Reply("Unbreakable walls are hidden");
		}
		return true;
	}

	[DebugCommand("showlinks", "Draws gamepad link points as an interface overlay", CommandRequirement.Client)]
	public static bool DrawLinkPoints(DebugMessage message)
	{
		DebugOptions.DrawLinkPoints = !DebugOptions.DrawLinkPoints;
		message.Reply("Gamepad link points are " + (DebugOptions.DrawLinkPoints ? "shown" : "hidden"));
		return true;
	}

	[DebugCommand("forcegamepad", "Prevents keyboard and mouse input from overriding gamepad input state. Keyboard can still use commands and chat", CommandRequirement.Client)]
	public static bool ForceGamepad(DebugMessage message)
	{
		DebugOptions.ForceGamepad = !DebugOptions.ForceGamepad;
		message.Reply("Gamepad input " + (DebugOptions.ForceGamepad ? "forced on" : "normal"));
		return true;
	}

	[DebugCommand("logitemuse", "Reports Player.ApplyItem/AnimTime calls in local chat.", CommandRequirement.Client)]
	public static bool LogItemUseCommand(DebugMessage message)
	{
		DebugOptions.LogItemUse = !DebugOptions.LogItemUse;
		return true;
	}

	[DebugCommand("shownetoffset", "Draws indicators for debugging netOffset", CommandRequirement.Client)]
	public static bool ShowNetOffset(DebugMessage message)
	{
		DebugOptions.ShowNetOffset = !DebugOptions.ShowNetOffset;
		message.Reply("netOffset debug " + (DebugOptions.ShowNetOffset ? "shown" : "hidden"));
		return true;
	}

	[DebugCommand("fakenetoffset", "Sets the netOffset for all entities to a given value (in pixels).", CommandRequirement.Client, HelpText = "Usage: /fakenetoffset <dx> <dy>")]
	public static bool FakeNetOffset(DebugMessage message)
	{
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		string[] array = message.Arguments.Split(new char[2] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries);
		if (array.Length < 2 || !float.TryParse(array[0], out var result) || !float.TryParse(array[1], out var result2))
		{
			return false;
		}
		DebugOptions.FakeNetOffset = new Vector2(result, result2);
		message.Reply("netOffset set to (" + result + ", " + result2 + ")");
		return true;
	}

	[DebugCommand("nodamagevar", "Removes damage variation (the inherent +/- 15% from damage). Useful for gathering specific data on true damage.", CommandRequirement.Client)]
	public static bool NoDamageVarCommand(DebugMessage message)
	{
		DebugOptions.NoDamageVar = !DebugOptions.NoDamageVar;
		message.Reply("No Damage Vars: " + (DebugOptions.NoDamageVar ? "On" : "Off"));
		return true;
	}

	[DebugCommand("hurtdummies", "Allows projectiles to aim at target dummies.", CommandRequirement.Client)]
	public static bool HurtDummiesCommand(DebugMessage message)
	{
		DebugOptions.LetProjectilesAimAtTargetDummies = !DebugOptions.LetProjectilesAimAtTargetDummies;
		message.Reply("Aim At Dummies: " + (DebugOptions.LetProjectilesAimAtTargetDummies ? "Enabled" : "Disabled"));
		return true;
	}

	[DebugCommand("practice", "Toggles practice mode, which returns you to the last checkpoint when you take lethal damage.", CommandRequirement.SinglePlayer)]
	public static bool PracticeCommand(DebugMessage message)
	{
		DebugOptions.PracticeMode = !DebugOptions.PracticeMode;
		if (DebugOptions.PracticeMode)
		{
			if (Main.LocalPlayer.dead)
			{
				Main.LocalPlayer.Spawn(PlayerSpawnContext.ReviveFromDeath);
			}
			Main.LocalPlayer.DamageTracker.Reset();
			Checkpoint(message);
		}
		message.Reply("Practice Mode " + (DebugOptions.PracticeMode ? "enabled" : "disabled"));
		return true;
	}

	[DebugCommand("expandedbossdamage", "Toggles expanded boss damage, which separates damage, and shows hit count per source.", CommandRequirement.SinglePlayer)]
	public static bool ExpandedBossDamage(DebugMessage message)
	{
		NPCDamageTracker.UseExpandedSources = !NPCDamageTracker.UseExpandedSources;
		message.Reply("Boss Damage Expanded Sources " + (NPCDamageTracker.UseExpandedSources ? "enabled" : "disabled"));
		return true;
	}

	[DebugCommand("pauseworld", "Pauses world updates. Use Numpad+/End/Mouse4 to step", CommandRequirement.Client)]
	public static bool PauseWorld(DebugMessage message)
	{
		WorldUpdateStepper.TogglePaused();
		message.Reply("World updates " + (WorldUpdateStepper.Paused ? "paused" : "resumed"));
		return true;
	}

	[DebugCommand("checkpoint", "Saves a gameplay checkpoint", CommandRequirement.SinglePlayer)]
	public static bool Checkpoint(DebugMessage message)
	{
		return UseCommandEndpoint(message, RecordReplay.Checkpoint);
	}

	[DebugCommand("return", "Returns to the last checkpoint", CommandRequirement.SinglePlayer)]
	public static bool Return(DebugMessage message)
	{
		return UseCommandEndpoint(message, RecordReplay.Return);
	}

	[DebugCommand("record", "Creates a checkpoint and starts recording inputs", CommandRequirement.SinglePlayer, HelpText = "Usage: /record [stop]")]
	public static bool Record(DebugMessage message)
	{
		if (message.Arguments.Equals("stop", StringComparison.OrdinalIgnoreCase))
		{
			return UseCommandEndpoint(message, RecordReplay.Cancel);
		}
		return UseCommandEndpoint(message, RecordReplay.Record);
	}

	[DebugCommand("replay", "Replays the recorded input capture on loop", CommandRequirement.SinglePlayer, HelpText = "Usage: /replay [stop]")]
	public static bool Replay(DebugMessage message)
	{
		if (message.Arguments.Equals("stop", StringComparison.OrdinalIgnoreCase))
		{
			return UseCommandEndpoint(message, RecordReplay.Cancel);
		}
		return UseCommandEndpoint(message, RecordReplay.Replay);
	}

	private static bool UseCommandEndpoint(DebugMessage message, CommandImpl impl)
	{
		try
		{
			impl(out var reply);
			message.Reply(reply);
		}
		catch (Exception ex)
		{
			message.ReplyError(ex.Message);
		}
		return true;
	}

	[DebugCommand("notimeouts-server", "Toggles server-side timeouts", CommandRequirement.MultiplayerRPC)]
	public static bool NoTimeoutsServer(DebugMessage message)
	{
		Main.stopTimeOuts = !Main.stopTimeOuts;
		message.Reply("Server timeouts for inactive clients: " + (Main.stopTimeOuts ? "Off" : "On"));
		return true;
	}

	[DebugCommand("notimeouts-client", "Toggles client-side timeouts", CommandRequirement.MultiplayerClient)]
	public static bool NoTimeoutsClient(DebugMessage message)
	{
		Main.stopTimeOuts = !Main.stopTimeOuts;
		message.Reply("Client timeouts for inactive server: " + (Main.stopTimeOuts ? "Off" : "On"));
		return true;
	}

	[DebugCommand("showdebug", "Toggles command reporting.", CommandRequirement.MultiplayerRPC | CommandRequirement.LocalServer)]
	public static bool ShowDebugCommand(DebugMessage message)
	{
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		if (message.Author != byte.MaxValue && !Main.player[message.Author].host)
		{
			message.ReplyError("/showdebug can only be toggled by the host or server console.");
			return true;
		}
		if (DebugOptions.Shared_ReportCommandUsage)
		{
			DebugOptions.Shared_ReportCommandUsage = false;
			ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral("Command reporting disabled"), new Color(250, 250, 0));
		}
		else
		{
			DebugOptions.Shared_ReportCommandUsage = true;
			ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral("Command reporting enabled"), new Color(250, 250, 0));
		}
		NetMessage.SendData(94, -1, -1, NetworkText.FromLiteral("/showdebug"), 0, DebugOptions.Shared_ReportCommandUsage ? 1 : 0);
		return true;
	}

	[DebugCommand("randomizeprojslots", "Toggles randomization of projectile slots (whoAmI).", CommandRequirement.AnyAuthority)]
	public static bool RandomizeProjSlotsCommand(DebugMessage message)
	{
		if (Main.netMode != 0 && message.Author != byte.MaxValue && !Main.player[message.Author].host)
		{
			message.ReplyError("/randomizeprojslots can only be toggled by the host or server console.");
			return true;
		}
		DebugOptions.Shared_RandomizeProjectileSlots = !DebugOptions.Shared_RandomizeProjectileSlots;
		message.Reply("Projectiles now spawn " + (DebugOptions.Shared_RandomizeProjectileSlots ? "in random slots" : "in order"));
		NetMessage.SendData(94, -1, -1, NetworkText.FromLiteral("/randomizeprojslots"), 0, DebugOptions.Shared_RandomizeProjectileSlots ? 1 : 0);
		return true;
	}

	[DebugCommand("saveloadout", "saves a loadout to the loadouts menu.", CommandRequirement.Client, HelpText = "Usage: /saveloadout (name)")]
	public static bool LoadoutSaveCommand(DebugMessage message)
	{
		if (message.Arguments.Length < 1)
		{
			return false;
		}
		ScenarioTestLoadout scenarioTestLoadout = new ScenarioTestLoadout();
		scenarioTestLoadout.Prepare(Main.LocalPlayer);
		ScenarioTestLoadout.Set(scenarioTestLoadout, message.Arguments);
		message.Reply("Loadout saved! " + message.Arguments);
		return true;
	}

	[DebugCommand("loadout", "creates a loadout.", CommandRequirement.Client, HelpText = "Usage: /loadout (name)")]
	public static bool LoadoutLoadCommand(DebugMessage message)
	{
		if (message.Arguments.Length == 0)
		{
			string[] array = ScenarioTestLoadout.FindAll();
			message.Reply("Available loadouts:");
			if (array.Length != 0)
			{
				string[] array2 = array;
				foreach (string fileName in array2)
				{
					message.Reply("    " + new FileInfo(fileName).Name.Replace(".json", ""));
				}
			}
			return false;
		}
		ScenarioTestLoadout.TryReadFromFriendlyName(out var config, message.Arguments);
		config?.Apply(Main.LocalPlayer);
		return true;
	}

	[DebugCommand("loadouts", "Opens a list of all the loadouts", CommandRequirement.Client)]
	public static bool LoadoutsCommand(DebugMessage message)
	{
		IngameFancyUI.OpenUIState(new UIDebugLoadoutsList());
		return true;
	}
}
