using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.UI;

namespace Terraria.GameContent.UI.Elements;

public class UIToggleImage : UIElement
{
	private Asset<Texture2D> _onTexture;

	private Asset<Texture2D> _offTexture;

	private int _drawWidth;

	private int _drawHeight;

	private Point _onTextureOffset = Point.Zero;

	private Point _offTextureOffset = Point.Zero;

	private bool _isOn;

	public bool IsOn => _isOn;

	public UIToggleImage(Asset<Texture2D> texture, int width, int height, Point onTextureOffset, Point offTextureOffset)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		_onTexture = texture;
		_offTexture = texture;
		_offTextureOffset = offTextureOffset;
		_onTextureOffset = onTextureOffset;
		_drawWidth = width;
		_drawHeight = height;
		Width.Set(width, 0f);
		Height.Set(height, 0f);
	}

	protected override void DrawSelf(SpriteBatch spriteBatch)
	{
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		CalculatedStyle dimensions = GetDimensions();
		Texture2D value;
		Point val;
		if (_isOn)
		{
			value = _onTexture.Value;
			val = _onTextureOffset;
		}
		else
		{
			value = _offTexture.Value;
			val = _offTextureOffset;
		}
		Color val2 = (IsMouseHovering ? Color.White : Color.Silver);
		spriteBatch.Draw(value, new Rectangle((int)dimensions.X, (int)dimensions.Y, _drawWidth, _drawHeight), (Rectangle?)new Rectangle(val.X, val.Y, _drawWidth, _drawHeight), val2);
	}

	public override void LeftClick(UIMouseEvent evt)
	{
		Toggle();
		base.LeftClick(evt);
	}

	public void SetState(bool value)
	{
		_isOn = value;
	}

	public void Toggle()
	{
		_isOn = !_isOn;
	}
}
