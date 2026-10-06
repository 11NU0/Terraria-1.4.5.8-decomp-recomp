using Terraria.Localization;

namespace Terraria.UI;

public class ItemTooltip
{
	public static readonly ItemTooltip None = new ItemTooltip();

	private static ulong _globalValidatorKey = 1uL;

	private static readonly ulong _neverUpdateHack = ulong.MaxValue;

	private string[] _tooltipLines;

	private ulong _validatorKey;

	private readonly LocalizedText _text;

	private string _processedText;

	public int Lines
	{
		get
		{
			ValidateTooltip();
			if (_tooltipLines == null)
			{
				return 0;
			}
			return _tooltipLines.Length;
		}
	}

	private ItemTooltip()
	{
	}

	private ItemTooltip(string key)
	{
		_text = Language.GetText(key);
	}

	public static ItemTooltip FromLanguageKey(string key)
	{
		return new ItemTooltip(key);
	}

	public string GetLine(int line)
	{
		ValidateTooltip();
		return _tooltipLines[line];
	}

	private ItemTooltip(string[] hardcodedLines)
	{
		_validatorKey = _neverUpdateHack;
		_tooltipLines = hardcodedLines;
		_processedText = string.Join("\n", hardcodedLines);
	}

	public static ItemTooltip FromHardcodedText(params string[] text)
	{
		return new ItemTooltip(text);
	}

	private void ValidateTooltip()
	{
		if (_validatorKey != _neverUpdateHack && _validatorKey != _globalValidatorKey)
		{
			_validatorKey = _globalValidatorKey;
			if (_text == null || !_text.HasValue)
			{
				_tooltipLines = null;
				_processedText = string.Empty;
				return;
			}
			string value = _text.Value;
			_tooltipLines = value.Split('\n');
			_processedText = value;
		}
	}

	public static void InvalidateTooltips()
	{
		_globalValidatorKey++;
		if (_globalValidatorKey == ulong.MaxValue)
		{
			_globalValidatorKey = 0uL;
		}
	}
}
