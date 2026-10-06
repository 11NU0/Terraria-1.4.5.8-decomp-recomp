using System;
using Terraria.GameContent.UI.States;

namespace Terraria.Social.Base;

public class RichPresenceState : IEquatable<RichPresenceState>
{
	public enum GameModeState
	{
		InMainMenu,
		CreatingPlayer,
		CreatingWorld,
		PlayingSingle,
		PlayingMulti
	}

	public GameModeState GameMode;

	public bool Equals(RichPresenceState other)
	{
		if (GameMode != other.GameMode)
		{
			return false;
		}
		return true;
	}

	public static RichPresenceState GetCurrentState()
	{
		RichPresenceState richPresenceState = new RichPresenceState();
		if (Main.gameMenu)
		{
			bool flag = Main.MenuUI.CurrentState is UICharacterCreation;
			bool flag2 = Main.MenuUI.CurrentState is UIWorldCreation;
			if (flag)
			{
				richPresenceState.GameMode = GameModeState.CreatingPlayer;
			}
			else if (flag2)
			{
				richPresenceState.GameMode = GameModeState.CreatingWorld;
			}
			else
			{
				richPresenceState.GameMode = GameModeState.InMainMenu;
			}
		}
		else if (Main.netMode == 0)
		{
			richPresenceState.GameMode = GameModeState.PlayingSingle;
		}
		else
		{
			richPresenceState.GameMode = GameModeState.PlayingMulti;
		}
		return richPresenceState;
	}
}
