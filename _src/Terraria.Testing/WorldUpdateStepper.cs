using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Terraria.GameInput;

namespace Terraria.Testing;

public static class WorldUpdateStepper
{
	public static bool DrawnThisUpdate;

	private static uint StartUpdateCount;

	private static int StepHeldCount;

	public static bool Paused { get; private set; }

	public static bool ShouldUpdateWorld()
	{
		GetStepKeyState(out var down, out var wasDown);
		if (!down)
		{
			StepHeldCount = 0;
		}
		if (!Paused)
		{
			return true;
		}
		if (down && ++StepHeldCount >= 12 && StepHeldCount % 2 == 0)
		{
			return true;
		}
		if (wasDown && !down)
		{
			return true;
		}
		return false;
	}

	private static void GetStepKeyState(out bool down, out bool wasDown)
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Invalid comparison between Unknown and I4
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Invalid comparison between Unknown and I4
		down = Main.keyState.IsKeyDown((Keys)35) || Main.keyState.IsKeyDown((Keys)107) || (int)PlayerInput.MouseInfo.XButton1 == 1;
		wasDown = Main.oldKeyState.IsKeyDown((Keys)35) || Main.oldKeyState.IsKeyDown((Keys)107) || (int)PlayerInput.MouseInfoOld.XButton1 == 1;
	}

	public static void TogglePaused()
	{
		Paused = !Paused;
		StartUpdateCount = Main.GameUpdateCount;
	}

	internal static void DrawHUD()
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		if (Paused && RecordReplay.Mode != RecordReplay.ReplayMode.Replaying)
		{
			Vector2 pos = Main.ScreenSize.ToVector2() - new Vector2(5f, 22f);
			Utils.DrawBorderString(Main.spriteBatch, "Updates Since Paused: " + (int)(Main.GameUpdateCount - StartUpdateCount), pos, Color.White, 1f, 1f);
		}
	}
}
