using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;
using Terraria.GameInput;
using Terraria.Localization;
using Terraria.UI;

namespace Terraria.GameContent.UI.Elements;

public class UISearchBar : UIElement
{
	private readonly LocalizedText _textToShowWhenEmpty;

	private UITextBox _text;

	private string actualContents;

	private float _textScale;

	private int _maxInputLength;

	public bool HasContents => !string.IsNullOrWhiteSpace(actualContents);

	public bool IsWritingText { get; private set; }

	public int MaxInputLength
	{
		get
		{
			return _maxInputLength;
		}
		set
		{
			_maxInputLength = value;
			_text.SetTextMaxLength(_maxInputLength);
		}
	}

	public event Action<string> OnContentsChanged;

	public event Action OnStartTakingInput;

	public event Action OnEndTakingInput;

	public event Action OnNeedingVirtualKeyboard;

	public UISearchBar(LocalizedText emptyContentText, float scale)
	{
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		_textToShowWhenEmpty = emptyContentText;
		_textScale = scale;
		_text = new UITextBox("", scale)
		{
			HAlign = 0f,
			VAlign = 0.5f,
			BackgroundColor = Color.Transparent,
			BorderColor = Color.Transparent,
			Width = new StyleDimension(0f, 1f),
			Height = new StyleDimension(0f, 1f),
			TextHAlign = 0f,
			ShowInputTicker = false
		};
		MaxInputLength = 50;
		Append(_text);
	}

	public void SetContents(string contents, bool forced = false)
	{
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		if (!(actualContents == contents) || forced)
		{
			actualContents = contents;
			if (string.IsNullOrEmpty(actualContents))
			{
				_text.TextColor = Color.Gray;
				_text.SetText(_textToShowWhenEmpty.Value, _textScale, large: false);
			}
			else
			{
				_text.TextColor = Color.White;
				_text.SetText(actualContents);
				actualContents = _text.Text;
			}
			TrimDisplayIfOverElementDimensions(0);
			if (OnContentsChanged != null)
			{
				OnContentsChanged(contents);
			}
		}
	}

	public void TrimDisplayIfOverElementDimensions(int padding)
	{
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		//IL_010a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0112: Unknown result type (might be due to invalid IL or missing references)
		//IL_015c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0163: Unknown result type (might be due to invalid IL or missing references)
		//IL_017c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0195: Unknown result type (might be due to invalid IL or missing references)
		//IL_019c: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c8: Unknown result type (might be due to invalid IL or missing references)
		CalculatedStyle dimensions = GetDimensions();
		if (dimensions.Width != 0f || dimensions.Height != 0f)
		{
			Point val = new Point((int)dimensions.X, (int)dimensions.Y);
			Point val2 = new Point(val.X + (int)dimensions.Width, val.Y + (int)dimensions.Height);
			Rectangle val3 = new Rectangle(val.X, val.Y, val2.X - val.X, val2.Y - val.Y);
			CalculatedStyle dimensions2 = _text.GetDimensions();
			Point val4 = new Point((int)dimensions2.X, (int)dimensions2.Y);
			Point val5 = new Point(val4.X + (int)_text.MinWidth.Pixels, val4.Y + (int)_text.MinHeight.Pixels);
			Rectangle val6 = new Rectangle(val4.X, val4.Y, val5.X - val4.X, val5.Y - val4.Y);
			while (val6.Right > val3.Right - padding && _text.Text.Length > 0)
			{
				_text.SetText(Utils.TrimLastCharacter(_text.Text));
				RecalculateChildren();
				dimensions2 = _text.GetDimensions();
				val4 = new Point((int)dimensions2.X, (int)dimensions2.Y);
				val5 = new Point(val4.X + (int)_text.MinWidth.Pixels, val4.Y + (int)_text.MinHeight.Pixels);
				val6 = new Rectangle(val4.X, val4.Y, val5.X - val4.X, val5.Y - val4.Y);
				actualContents = _text.Text;
			}
		}
	}

	public override void MouseOver(UIMouseEvent evt)
	{
		base.MouseOver(evt);
		SoundEngine.PlaySound(12);
	}

	public override void Update(GameTime gameTime)
	{
		if (IsWritingText)
		{
			if (NeedsVirtualkeyboard())
			{
				if (OnNeedingVirtualKeyboard != null)
				{
					OnNeedingVirtualKeyboard();
				}
				return;
			}
			PlayerInput.WritingText = true;
			Main.CurrentInputTextTakerOverride = this;
		}
		base.Update(gameTime);
	}

	private bool NeedsVirtualkeyboard()
	{
		return PlayerInput.SettingsForUI.ShowGamepadHints;
	}

	protected override void DrawSelf(SpriteBatch spriteBatch)
	{
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		base.DrawSelf(spriteBatch);
		if (IsWritingText)
		{
			PlayerInput.WritingText = true;
			Main.instance.HandleIME();
			Rectangle val = _text.GetDimensions().ToRectangle();
			Vector2 position = new Vector2((float)val.Left, (float)(val.Bottom + 32));
			Main.instance.SetIMEPanelAnchor(position, 0f);
			string inputText = Main.GetInputText(actualContents);
			if (Main.inputTextEnter)
			{
				ToggleTakingText();
			}
			else if (Main.inputTextEscape)
			{
				Main.inputTextEscape = false;
				ToggleTakingText();
			}
			SetContents(inputText);
		}
	}

	public void ToggleTakingText()
	{
		IsWritingText = !IsWritingText;
		_text.ShowInputTicker = IsWritingText;
		Main.clrInput();
		if (IsWritingText)
		{
			if (OnStartTakingInput != null)
			{
				OnStartTakingInput();
			}
			return;
		}
		if (OnEndTakingInput != null)
		{
			OnEndTakingInput();
		}
		PlayerInput.WritingText = false;
		Main.instance.HandleIME();
	}

	public override void OnDeactivate()
	{
		if (IsWritingText)
		{
			ToggleTakingText();
		}
	}
}
