using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Localization.IME;
using ReLogic.OS;

namespace Terraria.GameContent.UI.Elements;

internal class UITextBox(string text, float textScale = 1f, bool large = false) : UITextPanel<string>(text, textScale, large)
{
	private int _cursor;

	private int _frameCount;

	private int _maxLength = 20;

	public bool ShowInputTicker = true;

	public bool HideSelf;

	protected override Vector2 TextDrawPosition
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			Vector2 textDrawPosition = base.TextDrawPosition;
			if (ShowInputTicker)
			{
				string compositionString = Platform.Get<IImeService>().CompositionString;
				if (!string.IsNullOrEmpty(compositionString))
				{
					textDrawPosition.X -= Font.MeasureString(compositionString).X * TextScale * TextHAlign;
				}
			}
			return textDrawPosition;
		}
	}

	public void Write(string text)
	{
		SetText(Text.Insert(_cursor, text));
		_cursor += text.Length;
	}

	public override void SetText(string text, float textScale, bool large)
	{
		text = Utils.TrimUserString(text ?? "", _maxLength);
		base.SetText(text, textScale, large);
		_cursor = Math.Min(Text.Length, _cursor);
	}

	public void SetTextMaxLength(int maxLength)
	{
		_maxLength = maxLength;
	}

	public void Backspace()
	{
		if (_cursor != 0)
		{
			SetText(Utils.TrimLastCharacter(Text));
		}
	}

	public void CursorLeft()
	{
		if (_cursor != 0)
		{
			_cursor--;
		}
	}

	public void CursorRight()
	{
		if (_cursor < Text.Length)
		{
			_cursor++;
		}
	}

	protected override void DrawSelf(SpriteBatch spriteBatch)
	{
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		//IL_013f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0141: Unknown result type (might be due to invalid IL or missing references)
		if (HideSelf)
		{
			return;
		}
		_cursor = Text.Length;
		base.DrawSelf(spriteBatch);
		if (!ShowInputTicker)
		{
			return;
		}
		Vector2 textDrawPosition = TextDrawPosition;
		string compositionString = Platform.Get<IImeService>().CompositionString;
		if (!string.IsNullOrEmpty(compositionString))
		{
			textDrawPosition.X += Font.MeasureString(compositionString).X * TextScale;
			DrawText(spriteBatch, compositionString, TextDrawPosition + new Vector2(TextSize.X, 0f), Main.imeCompositionStringColor);
		}
		_frameCount++;
		if ((_frameCount %= 40) <= 20)
		{
			textDrawPosition.X += Font.MeasureString(Text.Substring(0, _cursor)).X * TextScale;
			textDrawPosition.X += 6f - (IsLarge ? 8f : 4f) * TextScale;
			if (IsLarge)
			{
				textDrawPosition.Y += 2f * TextScale;
			}
			DrawText(spriteBatch, "|", textDrawPosition, TextColor);
		}
	}
}
