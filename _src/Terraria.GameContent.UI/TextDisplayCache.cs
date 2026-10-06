using Terraria.GameInput;

namespace Terraria.GameContent.UI;

public class TextDisplayCache
{
	private string _originalText;

	private int _lastScreenWidth;

	private int _lastScreenHeight;

	private InputMode _lastInputMode;

	public string[] TextLines { get; private set; }

	public int AmountOfLines { get; private set; }

	public void PrepareCache(string text)
	{
		if ((0u | ((Main.screenWidth != _lastScreenWidth) ? 1u : 0u) | ((Main.screenHeight != _lastScreenHeight) ? 1u : 0u) | ((_originalText != text) ? 1u : 0u) | ((PlayerInput.CurrentInputMode != _lastInputMode) ? 1u : 0u)) != 0)
		{
			_lastScreenWidth = Main.screenWidth;
			_lastScreenHeight = Main.screenHeight;
			_originalText = text;
			_lastInputMode = PlayerInput.CurrentInputMode;
			TextLines = Utils.WordwrapString(text, FontAssets.MouseText.Value, 460, 10, out var lineAmount);
			AmountOfLines = lineAmount;
		}
	}
}
