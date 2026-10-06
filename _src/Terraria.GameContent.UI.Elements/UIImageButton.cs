using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.Audio;
using Terraria.UI;

namespace Terraria.GameContent.UI.Elements;

public class UIImageButton : UIElement
{
	private Asset<Texture2D> _texture;

	protected float _visibilityActive = 1f;

	protected float _visibilityInactive = 0.4f;

	private Asset<Texture2D> _borderTexture;

	private Rectangle? _frame;

	private Rectangle? _borderFrame;

	public Color Color = Color.White;

	public Color BorderColor = Color.White;

	public UIImageButton(Asset<Texture2D> texture, Rectangle? frame = null)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		_texture = texture;
		_frame = frame;
		Width.Set(frame.HasValue ? frame.Value.Width : _texture.Width(), 0f);
		Height.Set(frame.HasValue ? frame.Value.Height : _texture.Height(), 0f);
	}

	public void SetHoverImage(Asset<Texture2D> texture, Rectangle? frame = null)
	{
		_borderTexture = texture;
		_borderFrame = frame;
	}

	public void SetImage(Asset<Texture2D> texture, Rectangle? frame = null)
	{
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		_texture = texture;
		Width.Set(_frame.HasValue ? _frame.Value.Width : _texture.Width(), 0f);
		Height.Set(_frame.HasValue ? _frame.Value.Height : _texture.Height(), 0f);
	}

	protected override void DrawSelf(SpriteBatch spriteBatch)
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		CalculatedStyle dimensions = GetDimensions();
		spriteBatch.Draw(_texture.Value, dimensions.Position(), _frame, Color * (IsMouseHovering ? _visibilityActive : _visibilityInactive));
		if (_borderTexture != null && IsMouseHovering)
		{
			spriteBatch.Draw(_borderTexture.Value, dimensions.Position(), _borderFrame, BorderColor);
		}
	}

	public override void MouseOver(UIMouseEvent evt)
	{
		base.MouseOver(evt);
		SoundEngine.PlaySound(12);
	}

	public override void MouseOut(UIMouseEvent evt)
	{
		base.MouseOut(evt);
	}

	public void SetVisibility(float whenActive, float whenInactive)
	{
		_visibilityActive = MathHelper.Clamp(whenActive, 0f, 1f);
		_visibilityInactive = MathHelper.Clamp(whenInactive, 0f, 1f);
	}
}
