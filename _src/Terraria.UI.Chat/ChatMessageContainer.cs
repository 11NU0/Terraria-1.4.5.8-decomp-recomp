using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria.GameContent;

namespace Terraria.UI.Chat;

public class ChatMessageContainer
{
	public string OriginalText;

	private bool _prepared;

	private List<TextSnippet[]> _parsedText;

	private Color _color;

	private int _widthLimitInPixels;

	private int _timeLeft;

	public int LineCount => _parsedText.Count;

	public bool CanBeShownWhenChatIsClosed => _timeLeft > 0;

	public bool Prepared => _prepared;

	public void SetContents(string text, Color color, int widthLimitInPixels)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		OriginalText = text;
		_color = color;
		_widthLimitInPixels = widthLimitInPixels;
		_parsedText = new List<TextSnippet[]>();
		_timeLeft = 600;
		Refresh();
	}

	public void OnWidthLimitChanged()
	{
		if (_widthLimitInPixels == -1)
		{
			_prepared = false;
		}
	}

	public void Update()
	{
		if (_timeLeft > 0)
		{
			_timeLeft--;
		}
		Refresh();
	}

	public TextSnippet[] GetSnippetWithInversedIndex(int snippetIndex)
	{
		int index = _parsedText.Count - 1 - snippetIndex;
		return _parsedText[index];
	}

	private void Refresh()
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		if (!_prepared)
		{
			_prepared = true;
			int num = _widthLimitInPixels;
			if (num == -1)
			{
				num = Main.ChatLineWidthLimit;
			}
			List<List<TextSnippet>> list = Utils.WordwrapStringSmart(OriginalText, _color, FontAssets.MouseText.Value, num, 10);
			_parsedText.Clear();
			for (int i = 0; i < list.Count; i++)
			{
				_parsedText.Add(list[i].ToArray());
			}
		}
	}
}
