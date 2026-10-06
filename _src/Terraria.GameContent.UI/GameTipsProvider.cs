using System.Collections.Generic;
using Terraria.GameInput;
using Terraria.Localization;

namespace Terraria.GameContent.UI;

public class GameTipsProvider : ITipProvider
{
	private LocalizedText[] _tipsDefault;

	private LocalizedText[] _tipsGamepad;

	private LocalizedText[] _tipsKeyboard;

	private LocalizedText _lastTip;

	public GameTipsProvider()
	{
		_tipsDefault = Language.FindAll(Lang.CreateDialogFilter("LoadingTips_Default.", checkConditions: false));
		_tipsGamepad = Language.FindAll(Lang.CreateDialogFilter("LoadingTips_GamePad.", checkConditions: false));
		_tipsKeyboard = Language.FindAll(Lang.CreateDialogFilter("LoadingTips_Keyboard.", checkConditions: false));
		_lastTip = null;
	}

	public LocalizedText RollAvailableTip()
	{
		List<LocalizedText> list = new List<LocalizedText>();
		list.AddRange(_tipsDefault);
		if (PlayerInput.UsingGamepad)
		{
			list.AddRange(_tipsGamepad);
		}
		else
		{
			list.AddRange(_tipsKeyboard);
		}
		do
		{
			list.Remove(_lastTip);
			if (list.Count == 0)
			{
				_lastTip = LocalizedText.Empty;
			}
			else
			{
				_lastTip = list[Main.rand.Next(list.Count)];
			}
		}
		while (!_lastTip.ConditionsMet);
		return _lastTip;
	}
}
