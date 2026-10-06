using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using Terraria.UI;

namespace Terraria.GameContent.UI.Elements;

public class UITextPanel<T> : UIPanel
{
	protected T _text;

	protected float _textScale = 1f;

	protected Vector2 _textSize = Vector2.Zero;

	protected bool _isLarge;

	protected Color _color = Color.White;

	protected bool _drawPanel = true;

	public float TextHAlign = 0.5f;

	public bool HideContents;

	private string _asterisks;

	public bool IsLarge => _isLarge;

	public bool DrawPanel
	{
		get
		{
			return _drawPanel;
		}
		set
		{
			_drawPanel = value;
		}
	}

	public float TextScale
	{
		get
		{
			return _textScale;
		}
		set
		{
			_textScale = value;
		}
	}

	public Vector2 TextSize
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return _textSize;
		}
	}

	public string Text
	{
		get
		{
			if (_text != null)
			{
				return _text.ToString();
			}
			return "";
		}
	}

	public Color TextColor
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return _color;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			_color = value;
		}
	}

	protected DynamicSpriteFont Font
	{
		get
		{
			if (!_isLarge)
			{
				return FontAssets.MouseText.Value;
			}
			return FontAssets.DeathText.Value;
		}
	}

	protected virtual Vector2 TextDrawPosition
	{
		get
		{
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0072: Unknown result type (might be due to invalid IL or missing references)
			CalculatedStyle innerDimensions = GetInnerDimensions();
			Vector2 result = innerDimensions.Position();
			result.X += (innerDimensions.Width - _textSize.X) * TextHAlign;
			if (_isLarge)
			{
				result.Y -= 10f * _textScale * _textScale;
			}
			else
			{
				result.Y -= 2f * _textScale;
			}
			return result;
		}
	}

	public UITextPanel(T text, float textScale = 1f, bool large = false)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		SetText(text, textScale, large);
	}

	public override void Recalculate()
	{
		SetText(_text, _textScale, _isLarge);
		base.Recalculate();
	}

	public void SetText(T text)
	{
		SetText(text, _textScale, _isLarge);
	}

	public virtual void SetText(T text, float textScale, bool large)
	{
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		_text = text;
		_textScale = textScale;
		_isLarge = large;
		_textSize = new Vector2(Font.MeasureString(text.ToString()).X, large ? 32f : 16f) * textScale;
		MinWidth.Set(_textSize.X + PaddingLeft + PaddingRight, 0f);
		MinHeight.Set(_textSize.Y + PaddingTop + PaddingBottom, 0f);
	}

	protected override void DrawSelf(SpriteBatch spriteBatch)
	{
		if (_drawPanel)
		{
			base.DrawSelf(spriteBatch);
		}
		DrawText(spriteBatch);
	}

	protected void DrawText(SpriteBatch spriteBatch)
	{
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		string text = Text;
		if (HideContents)
		{
			if (_asterisks == null || _asterisks.Length != text.Length)
			{
				_asterisks = new string('*', text.Length);
			}
			text = _asterisks;
		}
		DrawText(spriteBatch, text, TextDrawPosition, _color);
	}

	protected void DrawText(SpriteBatch spriteBatch, string text, Vector2 position, Color color)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		if (_isLarge)
		{
			Utils.DrawBorderStringBig(spriteBatch, text, position, color, _textScale);
		}
		else
		{
			Utils.DrawBorderString(spriteBatch, text, position, color, _textScale);
		}
	}
}
