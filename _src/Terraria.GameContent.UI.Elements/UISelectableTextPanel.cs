using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.UI;

namespace Terraria.GameContent.UI.Elements;

public class UISelectableTextPanel<T> : UITextPanel<T>
{
	private readonly Asset<Texture2D> _BasePanelTexture;

	private readonly Asset<Texture2D> _hoveredBorderTexture;

	private Func<UISelectableTextPanel<T>, bool> _isSelected;

	public Func<UISelectableTextPanel<T>, bool> IsSelected
	{
		get
		{
			return _isSelected;
		}
		set
		{
			_isSelected = value;
		}
	}

	public UISelectableTextPanel(T text, float textScale = 1f, bool large = false)
		: base(text, textScale, large)
	{
		_BasePanelTexture = Main.Assets.Request<Texture2D>("Images/UI/CharCreation/PanelGrayscale", (AssetRequestMode)1);
		_hoveredBorderTexture = Main.Assets.Request<Texture2D>("Images/UI/CharCreation/CategoryPanelBorder", (AssetRequestMode)1);
	}

	protected override void DrawSelf(SpriteBatch spriteBatch)
	{
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0110: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		if (_drawPanel)
		{
			CalculatedStyle dimensions = GetDimensions();
			int num = 4;
			int num2 = 10;
			int num3 = 10;
			Utils.DrawSplicedPanel(spriteBatch, _BasePanelTexture.Value, (int)dimensions.X, (int)dimensions.Y, (int)dimensions.Width, (int)dimensions.Height, num2, num2, num3, num3, Color.Lerp(Color.Black, _color, 0.8f) * 0.5f);
			if (IsSelected != null && IsSelected(this))
			{
				Utils.DrawSplicedPanel(spriteBatch, _BasePanelTexture.Value, (int)dimensions.X + num, (int)dimensions.Y + num, (int)dimensions.Width - num * 2, (int)dimensions.Height - num * 2, num2, num2, num3, num3, Color.Lerp(_color, Color.White, 0.7f) * 0.5f);
			}
			if (IsMouseHovering)
			{
				Utils.DrawSplicedPanel(spriteBatch, _hoveredBorderTexture.Value, (int)dimensions.X, (int)dimensions.Y, (int)dimensions.Width, (int)dimensions.Height, num2, num2, num3, num3, Color.White);
			}
		}
		DrawText(spriteBatch);
	}
}
