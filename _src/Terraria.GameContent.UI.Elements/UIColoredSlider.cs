using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;
using Terraria.GameInput;
using Terraria.Localization;
using Terraria.UI;

namespace Terraria.GameContent.UI.Elements;

public class UIColoredSlider : UISliderBase
{
	private Color _color;

	private LocalizedText _textKey;

	private Func<float> _getStatusTextAct;

	private Action<float> _slideKeyboardAction;

	private Func<float, Color> _blipFunc;

	private Action _slideGamepadAction;

	private const bool BOTHER_WITH_TEXT = false;

	private bool _isReallyMouseOvered;

	private bool _alreadyHovered;

	private bool _soundedUsage;

	public UIColoredSlider(LocalizedText textKey, Func<float> getStatus, Action<float> setStatusKeyboard, Action setStatusGamepad, Func<float, Color> blipColorFunction, Color color)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		_color = color;
		_textKey = textKey;
		_getStatusTextAct = ((getStatus != null) ? getStatus : ((Func<float>)(() => 0f)));
		_slideKeyboardAction = ((setStatusKeyboard != null) ? setStatusKeyboard : ((Action<float>)((float s) =>
		{
		})));
		_blipFunc = ((blipColorFunction != null) ? blipColorFunction : ((Func<float, Color>)((float s) =>
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			return Color.Lerp(Color.Black, Color.White, s);
		})));
		_slideGamepadAction = setStatusGamepad;
		_isReallyMouseOvered = false;
	}

	protected override void DrawSelf(SpriteBatch spriteBatch)
	{
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0123: Unknown result type (might be due to invalid IL or missing references)
		//IL_012a: Unknown result type (might be due to invalid IL or missing references)
		UISliderBase.CurrentAimedSlider = null;
		if (!Main.mouseLeft)
		{
			UISliderBase.CurrentLockedSlider = null;
		}
		int usageLevel = GetUsageLevel();
		float num = 8f;
		base.DrawSelf(spriteBatch);
		CalculatedStyle dimensions = GetDimensions();
		float num2 = dimensions.Width + 1f;
		Vector2 val = new Vector2(dimensions.X, dimensions.Y);
		bool flag = false;
		bool flag2 = IsMouseHovering;
		if (usageLevel == 2)
		{
			flag2 = false;
		}
		if (usageLevel == 1)
		{
			flag2 = true;
		}
		Vector2 val2 = val + new Vector2(0f, 2f);
		Color val3;
		if (flag)
		{
			val3 = Color.Gold;
		}
		else
		{
			val3 = (flag2 ? Color.White : Color.Silver);
		}
		val3 = Color.Lerp(val3, Color.White, flag2 ? 0.5f : 0f);
		Vector2 val4 = new Vector2(0.8f);
		val2.X += 8f;
		val2.Y += num;
		val2.X -= 17f;
		TextureAssets.ColorBar.Frame();
		val2 = new Vector2(dimensions.X + dimensions.Width - 10f, dimensions.Y + 10f + num);
		float obj = DrawValueBar(spriteBatch, val2, 1f, _getStatusTextAct(), usageLevel, out var wasInBar, _blipFunc);
		if ((UISliderBase.CurrentLockedSlider == this) | wasInBar)
		{
			UISliderBase.CurrentAimedSlider = this;
			if (PlayerInput.Triggers.Current.MouseLeft && !PlayerInput.UsingGamepad && UISliderBase.CurrentLockedSlider == this)
			{
				_slideKeyboardAction(obj);
				if (!_soundedUsage)
				{
					SoundEngine.PlaySound(12);
				}
				_soundedUsage = true;
			}
			else
			{
				_soundedUsage = false;
			}
		}
		if (UISliderBase.CurrentAimedSlider != null && UISliderBase.CurrentLockedSlider == null)
		{
			UISliderBase.CurrentLockedSlider = UISliderBase.CurrentAimedSlider;
		}
		if (_isReallyMouseOvered)
		{
			_slideGamepadAction();
		}
	}

	private float DrawValueBar(SpriteBatch sb, Vector2 drawPosition, float drawScale, float sliderPosition, int lockMode, out bool wasInBar, Func<float, Color> blipColorFunc)
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_013d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0178: Unknown result type (might be due to invalid IL or missing references)
		//IL_0179: Unknown result type (might be due to invalid IL or missing references)
		//IL_0273: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0226: Unknown result type (might be due to invalid IL or missing references)
		//IL_023c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0249: Unknown result type (might be due to invalid IL or missing references)
		//IL_024f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0262: Unknown result type (might be due to invalid IL or missing references)
		//IL_026a: Unknown result type (might be due to invalid IL or missing references)
		Texture2D value = TextureAssets.ColorBar.Value;
		Vector2 val = new Vector2((float)value.Width, (float)value.Height) * drawScale;
		drawPosition.X -= (int)val.X;
		Rectangle val2 = new Rectangle((int)drawPosition.X, (int)drawPosition.Y - (int)val.Y / 2, (int)val.X, (int)val.Y);
		Rectangle val3 = val2;
		sb.Draw(value, val2, Color.White);
		float num = (float)val2.X + 5f * drawScale;
		float num2 = (float)val2.Y + 4f * drawScale;
		for (float num3 = 0f; num3 < 167f; num3++)
		{
			float arg = num3 / 167f;
			Color val4 = blipColorFunc(arg);
			sb.Draw(TextureAssets.ColorBlip.Value, new Vector2(num + num3 * drawScale, num2), (Rectangle?)null, val4, 0f, Vector2.Zero, drawScale, (SpriteEffects)0, 0f);
		}
		val2.X = (int)num - 2;
		val2.Y = (int)num2;
		val2.Width -= 4;
		val2.Height -= 8;
		bool flag = (_isReallyMouseOvered = val2.Contains(new Point(Main.mouseX, Main.mouseY)));
		if (IgnoresMouseInteraction)
		{
			flag = false;
		}
		if (lockMode == 2)
		{
			flag = false;
		}
		if (flag || lockMode == 1)
		{
			sb.Draw(TextureAssets.ColorHighlight.Value, val3, Main.OurFavoriteColor);
			if (!_alreadyHovered)
			{
				SoundEngine.PlaySound(12);
			}
			_alreadyHovered = true;
		}
		else
		{
			_alreadyHovered = false;
		}
		wasInBar = false;
		if (!IgnoresMouseInteraction)
		{
			sb.Draw(TextureAssets.ColorSlider.Value, new Vector2(num + 167f * drawScale * sliderPosition, num2 + 4f * drawScale), (Rectangle?)null, Color.White, 0f, new Vector2(0.5f * (float)TextureAssets.ColorSlider.Value.Width, 0.5f * (float)TextureAssets.ColorSlider.Value.Height), drawScale, (SpriteEffects)0, 0f);
			if (Main.mouseX >= val2.X && Main.mouseX <= val2.X + val2.Width)
			{
				wasInBar = flag;
				return (float)(Main.mouseX - val2.X) / (float)val2.Width;
			}
		}
		if (val2.X >= Main.mouseX)
		{
			return 0f;
		}
		return 1f;
	}
}
