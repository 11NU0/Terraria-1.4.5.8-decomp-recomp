using Steamworks;
using Terraria.Social.Base;

namespace Terraria.Social.Steam;

public class OverlaySocialModule : Terraria.Social.Base.OverlaySocialModule
{
	private Callback<GamepadTextInputDismissed_t> _gamepadTextInputDismissed;

	private bool _gamepadTextInputActive;

	public override void Initialize()
	{
		_gamepadTextInputDismissed = Callback<GamepadTextInputDismissed_t>.Create((Callback<GamepadTextInputDismissed_t>.DispatchDelegate)OnGamepadTextInputDismissed);
	}

	public override void Shutdown()
	{
	}

	public override bool IsGamepadTextInputActive()
	{
		return _gamepadTextInputActive;
	}

	public override bool ShowGamepadTextInput(string description, uint maxLength, bool multiLine = false, string existingText = "", bool password = false)
	{
		if (_gamepadTextInputActive)
		{
			return false;
		}
		bool flag = SteamUtils.ShowGamepadTextInput((EGamepadTextInputMode)(password ? 1 : 0), (EGamepadTextInputLineMode)(multiLine ? 1 : 0), description, maxLength, existingText);
		if (flag)
		{
			_gamepadTextInputActive = true;
		}
		return flag;
	}

	public override string GetGamepadText()
	{
		uint enteredGamepadTextLength = SteamUtils.GetEnteredGamepadTextLength();
		string result = default;
		SteamUtils.GetEnteredGamepadTextInput(out result, enteredGamepadTextLength);
		return result;
	}

	private void OnGamepadTextInputDismissed(GamepadTextInputDismissed_t result)
	{
		_gamepadTextInputActive = false;
	}
}
