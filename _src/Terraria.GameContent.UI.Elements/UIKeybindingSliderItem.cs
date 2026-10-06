using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.GameInput;
using Terraria.UI;
using Terraria.UI.Chat;

namespace Terraria.GameContent.UI.Elements;

public class UIKeybindingSliderItem : UIElement
{
	private Color _color;

	private Func<string> _TextDisplayFunction;

	private Func<float> _GetStatusFunction;

	private Action<float> _SlideKeyboardAction;

	private Action _SlideGamepadAction;

	private int _sliderIDInPage;

	private Asset<Texture2D> _toggleTexture;

	public UIKeybindingSliderItem(Func<string> getText, Func<float> getStatus, Action<float> setStatusKeyboard, Action setStatusGamepad, int sliderIDInPage, Color color)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		_color = color;
		_toggleTexture = Main.Assets.Request<Texture2D>("Images/UI/Settings_Toggle", (AssetRequestMode)1);
		_TextDisplayFunction = ((getText != null) ? getText : ((Func<string>)(() => "???")));
		_GetStatusFunction = ((getStatus != null) ? getStatus : ((Func<float>)(() => 0f)));
		_SlideKeyboardAction = ((setStatusKeyboard != null) ? setStatusKeyboard : ((Action<float>)((float s) =>
		{
		})));
		_SlideGamepadAction = ((setStatusGamepad != null) ? setStatusGamepad : ((Action)(() =>
		{
		})));
		_sliderIDInPage = sliderIDInPage;
	}

	protected override void DrawSelf(SpriteBatch spriteBatch)
	{
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0130: Unknown result type (might be due to invalid IL or missing references)
		//IL_0132: Unknown result type (might be due to invalid IL or missing references)
		//IL_0139: Unknown result type (might be due to invalid IL or missing references)
		//IL_013e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0166: Unknown result type (might be due to invalid IL or missing references)
		//IL_018f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0194: Unknown result type (might be due to invalid IL or missing references)
		//IL_0196: Unknown result type (might be due to invalid IL or missing references)
		float num = 6f;
		base.DrawSelf(spriteBatch);
		int num2 = 0;
		IngameOptions.rightHover = -1;
		if (!Main.mouseLeft)
		{
			IngameOptions.rightLock = -1;
		}
		if (IngameOptions.rightLock == _sliderIDInPage)
		{
			num2 = 1;
		}
		else if (IngameOptions.rightLock != -1)
		{
			num2 = 2;
		}
		CalculatedStyle dimensions = GetDimensions();
		float num3 = dimensions.Width + 1f;
		Vector2 val = new Vector2(dimensions.X, dimensions.Y);
		bool flag = IsMouseHovering;
		if (num2 == 1)
		{
			flag = true;
		}
		if (num2 == 2)
		{
			flag = false;
		}
		Vector2 scale = new Vector2(0.8f);
		Color val2;
			val2 = (flag ? Color.White : Color.Silver);
		val2 = Color.Lerp(val2, Color.White, flag ? 0.5f : 0f);
		Color color = (flag ? _color : _color.MultiplyRGBA(new Color(180, 180, 180)));
		Vector2 position = val;
		Utils.DrawSettingsPanel(spriteBatch, position, num3, color);
		position.X += 8f;
		position.Y += 2f + num;
		ChatManager.DrawColorCodedStringWithShadow(spriteBatch, FontAssets.ItemStack.Value, _TextDisplayFunction(), position, val2, 0f, Vector2.Zero, scale, num3);
		position.X -= 17f;
		TextureAssets.ColorBar.Frame();
		position = new Vector2(dimensions.X + dimensions.Width - 10f, dimensions.Y + 10f + num);
		IngameOptions.valuePosition = position;
		float obj = IngameOptions.DrawValueBar(spriteBatch, 1f, _GetStatusFunction(), num2);
		if (IngameOptions.inBar || IngameOptions.rightLock == _sliderIDInPage)
		{
			IngameOptions.rightHover = _sliderIDInPage;
			if (PlayerInput.Triggers.Current.MouseLeft && PlayerInput.CurrentProfile.AllowEditing && !PlayerInput.UsingGamepad && IngameOptions.rightLock == _sliderIDInPage)
			{
				_SlideKeyboardAction(obj);
			}
		}
		if (IngameOptions.rightHover != -1 && IngameOptions.rightLock == -1)
		{
			IngameOptions.rightLock = IngameOptions.rightHover;
		}
		if (IsMouseHovering && PlayerInput.CurrentProfile.AllowEditing)
		{
			_SlideGamepadAction();
		}
	}
}
