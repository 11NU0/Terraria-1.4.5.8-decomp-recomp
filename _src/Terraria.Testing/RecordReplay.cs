using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria.Chat;

namespace Terraria.Testing;

public class RecordReplay
{
	public enum ReplayMode
	{
		None,
		Recording,
		Replaying
	}

	private static StateSnapshot GameplaySnapshot;

	private static List<StateSnapshot> RecordedInputs;

	private static int NextReplayInputIndex;

	public static ReplayMode Mode { get; private set; }

	public static void Reset()
	{
		GameplaySnapshot = null;
		RecordedInputs = null;
		Mode = ReplayMode.None;
	}

	public static void Checkpoint(out string reply)
	{
		switch (Mode)
		{
		default:
			GameplaySnapshot = StateSnapshot.Gameplay.Capture();
			reply = "Checkpoint saved";
			if (RecordedInputs != null)
			{
				RecordedInputs = null;
				reply += " and replay discarded";
			}
			break;
		case ReplayMode.Recording:
			Record(out reply);
			break;
		case ReplayMode.Replaying:
			if (NextReplayInputIndex >= RecordedInputs.Count)
			{
				throw new Exception("Cannot create checkpoint on last frame of replay");
			}
			reply = "Replay now starts at update " + NextReplayInputIndex + " / " + RecordedInputs.Count;
			GameplaySnapshot = StateSnapshot.Gameplay.Capture();
			RecordedInputs.RemoveRange(0, NextReplayInputIndex);
			NextReplayInputIndex = 0;
			break;
		}
	}

	public static void Return(out string reply)
	{
		if (GameplaySnapshot == null)
		{
			throw new Exception("Use /checkpoint or /record to create a checkpoint or start recording");
		}
		switch (Mode)
		{
		default:
			reply = "Checkpoint loaded";
			break;
		case ReplayMode.Recording:
			RecordedInputs = new List<StateSnapshot>();
			reply = "Recording restarted";
			break;
		case ReplayMode.Replaying:
			reply = "Replay restarted";
			break;
		}
		GameplaySnapshot.Restore();
		NextReplayInputIndex = 0;
	}

	public static void Record(out string reply)
	{
		if (Mode == ReplayMode.Replaying)
		{
			throw new Exception("Cannot record a new replay while one is playing. /replay stop to exit");
		}
		reply = ((Mode == ReplayMode.None) ? "Recording gameplay" : "Recording restarted");
		GameplaySnapshot = StateSnapshot.Gameplay.Capture();
		RecordedInputs = new List<StateSnapshot>();
		Mode = ReplayMode.Recording;
	}

	public static void Replay(out string reply)
	{
		ReplayMode mode = Mode;
		if (mode != ReplayMode.Replaying)
		{
			if (RecordedInputs == null)
			{
				throw new Exception("Use /record to create a replay first");
			}
			reply = "Replaying " + RecordedInputs.Count + " updates on loop. /replay stop to exit";
		}
		else
		{
			if (NextReplayInputIndex == 0)
			{
				throw new Exception("Already at start of replay");
			}
			reply = "Replay truncated " + RecordedInputs.Count + " -> " + NextReplayInputIndex + " updates";
			RecordedInputs.RemoveRange(NextReplayInputIndex, RecordedInputs.Count - NextReplayInputIndex);
		}
		GameplaySnapshot.Restore();
		NextReplayInputIndex = 0;
		Mode = ReplayMode.Replaying;
	}

	public static void Cancel(out string reply)
	{
		switch (Mode)
		{
		default:
			reply = "Nothing to cancel";
			break;
		case ReplayMode.Recording:
			reply = "Recording stopped";
			break;
		case ReplayMode.Replaying:
			reply = "Replay stopped";
			break;
		}
		Mode = ReplayMode.None;
	}

	internal static StateSnapshot RecordOrReplayInput()
	{
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		switch (Mode)
		{
		case ReplayMode.Recording:
			if (Main.playerInventory || Main.mapFullscreen || Main.InGameUI.IsVisible)
			{
				Cancel(out var _);
				Main.NewText("Recording cancelled. Cannot record UI interactions due to technical limitations", ChatColors.Error);
				return null;
			}
			RecordedInputs.Add(StateSnapshot.Input.Capture());
			return null;
		case ReplayMode.Replaying:
		{
			StateSnapshot result = StateSnapshot.Input.Capture();
			RecordedInputs[NextReplayInputIndex++].Restore();
			return result;
		}
		default:
			return null;
		}
	}

	internal static void ResetIfReplayEnded()
	{
		if (Mode == ReplayMode.Replaying && NextReplayInputIndex >= RecordedInputs.Count)
		{
			GameplaySnapshot.Restore();
			NextReplayInputIndex = 0;
		}
	}

	internal static void DrawHUD()
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		if (Mode == ReplayMode.Replaying)
		{
			Vector2 pos = Main.ScreenSize.ToVector2() - new Vector2(5f, 22f);
			Utils.DrawBorderString(Main.spriteBatch, "Replaying Update: " + NextReplayInputIndex + " / " + RecordedInputs.Count, pos, Color.White, 1f, 1f);
		}
	}
}
