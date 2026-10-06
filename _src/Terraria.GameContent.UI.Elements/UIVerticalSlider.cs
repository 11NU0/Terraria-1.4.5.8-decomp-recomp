using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;
using Terraria.GameInput;

namespace Terraria.GameContent.UI.Elements;

public class UIVerticalSlider : UISliderBase
{
	public float FillPercent;

	public Color FilledColor = Main.OurFavoriteColor;

	public Color EmptyColor = Color.Black;

	private Func<float> _getSliderValue;

	private Action<float> _slideKeyboardAction;

	private Action _slideGamepadAction;

	private bool _isReallyMouseOvered;

	private bool _soundedUsage;

	private bool _alreadyHovered;

	public UIVerticalSlider(Func<float> getStatus, Action<float> setStatusKeyboard, Action setStatusGamepad, Color color)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		_getSliderValue = ((getStatus != null) ? getStatus : ((Func<float>)(() => 0f)));
		_slideKeyboardAction = ((setStatusKeyboard != null) ? setStatusKeyboard : ((Action<float>)((float s) =>
		{
		})));
		_slideGamepadAction = setStatusGamepad;
		_isReallyMouseOvered = false;
	}

	protected override void DrawSelf(SpriteBatch spriteBatch)
	{
		UISliderBase.CurrentAimedSlider = null;
		if (!Main.mouseLeft || PlayerInput.IgnoreMouseInterface)
		{
			UISliderBase.CurrentLockedSlider = null;
		}
		GetUsageLevel();
		FillPercent = _getSliderValue();
		float sliderValueThatWasSet = FillPercent;
		bool flag = false;
		if (DrawValueBarDynamicWidth(spriteBatch, out sliderValueThatWasSet))
		{
			flag = true;
		}
		if ((UISliderBase.CurrentLockedSlider == this) | flag)
		{
			UISliderBase.CurrentAimedSlider = this;
			if (PlayerInput.Triggers.Current.MouseLeft && !PlayerInput.UsingGamepad && UISliderBase.CurrentLockedSlider == this)
			{
				_slideKeyboardAction(sliderValueThatWasSet);
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

	private bool DrawValueBarDynamicWidth(SpriteBatch spriteBatch, out float sliderValueThatWasSet)
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		//IL_0116: Unknown result type (might be due to invalid IL or missing references)
		//IL_0118: Unknown result type (might be due to invalid IL or missing references)
		//IL_0120: Unknown result type (might be due to invalid IL or missing references)
		//IL_012c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0141: Unknown result type (might be due to invalid IL or missing references)
		//IL_0148: Unknown result type (might be due to invalid IL or missing references)
		//IL_0158: Unknown result type (might be due to invalid IL or missing references)
		//IL_015d: Unknown result type (might be due to invalid IL or missing references)
		//IL_015f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0164: Unknown result type (might be due to invalid IL or missing references)
		//IL_0166: Unknown result type (might be due to invalid IL or missing references)
		//IL_0168: Unknown result type (might be due to invalid IL or missing references)
		//IL_0176: Unknown result type (might be due to invalid IL or missing references)
		//IL_0178: Unknown result type (might be due to invalid IL or missing references)
		//IL_017f: Unknown result type (might be due to invalid IL or missing references)
		//IL_018c: Unknown result type (might be due to invalid IL or missing references)
		//IL_018e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0195: Unknown result type (might be due to invalid IL or missing references)
		//IL_019f: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b2: Unknown result type (might be due to invalid IL or missing references)
		sliderValueThatWasSet = 0f;
		Texture2D value = TextureAssets.ColorBar.Value;
		Rectangle val = GetDimensions().ToRectangle();
		Rectangle val2 = new Rectangle(5, 4, 4, 4);
		Utils.DrawSplicedPanel(spriteBatch, value, val.X, val.Y, val.Width, val.Height, val2.X, val2.Width, val2.Y, val2.Height, Color.White);
		Rectangle val3 = val;
		val3.X += val2.Left;
		val3.Width -= val2.Right;
		val3.Y += val2.Top;
		val3.Height -= val2.Bottom;
		Texture2D value2 = TextureAssets.MagicPixel.Value;
		Rectangle value3 = new Rectangle(0, 0, 1, 1);
		spriteBatch.Draw(value2, val3, (Rectangle?)value3, EmptyColor);
		Rectangle val4 = val3;
		val4.Height = (int)((float)val4.Height * FillPercent);
		val4.Y += val3.Height - val4.Height;
		spriteBatch.Draw(value2, val4, (Rectangle?)value3, FilledColor);
		Vector2 center = new Vector2((float)(val4.Center.X + 1), (float)val4.Top);
		Vector2 size = new Vector2((float)(val4.Width + 16), 4f);
		Rectangle val5 = Utils.CenteredRectangle(center, size);
		Rectangle val6 = val5;
		val6.Inflate(2, 2);
		spriteBatch.Draw(value2, val6, (Rectangle?)value3, Color.Black);
		spriteBatch.Draw(value2, val5, (Rectangle?)value3, Color.White);
		Rectangle val7 = val3;
		val7.Inflate(4, 0);
		bool flag = (_isReallyMouseOvered = val7.Contains(Main.MouseScreen.ToPoint()) && !PlayerInput.IgnoreMouseInterface);
		if (IgnoresMouseInteraction)
		{
			flag = false;
		}
		int usageLevel = GetUsageLevel();
		if (usageLevel == 2)
		{
			flag = false;
		}
		if (usageLevel == 1)
		{
			flag = true;
		}
		if (flag || usageLevel == 1)
		{
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
		if (flag)
		{
			sliderValueThatWasSet = Utils.GetLerpValue(val3.Bottom, val3.Top, Main.mouseY, clamped: true);
			return true;
		}
		return false;
	}
}
