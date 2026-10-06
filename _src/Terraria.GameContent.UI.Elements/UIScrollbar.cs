using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.Audio;
using Terraria.UI;

namespace Terraria.GameContent.UI.Elements;

public class UIScrollbar : UIElement
{
	public enum ColorTheme
	{
		Blue,
		Cyan
	}

	private float _viewPosition;

	private float _viewSize = 1f;

	private float _maxViewSize = 20f;

	private bool _isDragging;

	private bool _isHoveringOverHandle;

	private float _dragYOffset;

	public bool AutoHide;

	private Asset<Texture2D> _texture;

	private Asset<Texture2D> _innerTexture;

	private ColorTheme _theme;

	public float ViewPosition
	{
		get
		{
			return _viewPosition;
		}
		set
		{
			_viewPosition = MathHelper.Clamp(value, 0f, _maxViewSize - _viewSize);
		}
	}

	public bool CanScroll => _maxViewSize != _viewSize;

	public void GoToBottom()
	{
		ViewPosition = _maxViewSize - _viewSize;
	}

	public UIScrollbar(ColorTheme theme = ColorTheme.Blue)
	{
		_theme = theme;
		Width.Set(20f, 0f);
		MaxWidth.Set(20f, 0f);
		string text = "Images/UI/Scrollbar";
		if (_theme == ColorTheme.Cyan)
		{
			text = "Images/UI/Scrollbar2";
		}
		_texture = Main.Assets.Request<Texture2D>(text, (AssetRequestMode)1);
		_innerTexture = Main.Assets.Request<Texture2D>("Images/UI/ScrollbarInner", (AssetRequestMode)1);
		PaddingTop = 5f;
		PaddingBottom = 5f;
	}

	public void SetView(float viewSize, float maxViewSize)
	{
		viewSize = MathHelper.Clamp(viewSize, 0f, maxViewSize);
		_viewPosition = MathHelper.Clamp(_viewPosition, 0f, maxViewSize - viewSize);
		_viewSize = viewSize;
		_maxViewSize = maxViewSize;
	}

	public float GetValue()
	{
		return _viewPosition;
	}

	private Rectangle GetHandleRectangle()
	{
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		CalculatedStyle innerDimensions = GetInnerDimensions();
		if (_maxViewSize == 0f && _viewSize == 0f)
		{
			_viewSize = 1f;
			_maxViewSize = 1f;
		}
		return new Rectangle((int)innerDimensions.X, (int)(innerDimensions.Y + innerDimensions.Height * (_viewPosition / _maxViewSize)) - 3, 20, (int)(innerDimensions.Height * (_viewSize / _maxViewSize)) + 7);
	}

	private void DrawBar(SpriteBatch spriteBatch, Texture2D texture, Rectangle dimensions, Color color)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		spriteBatch.Draw(texture, new Rectangle(dimensions.X, dimensions.Y - 6, dimensions.Width, 6), (Rectangle?)new Rectangle(0, 0, texture.Width, 6), color);
		spriteBatch.Draw(texture, new Rectangle(dimensions.X, dimensions.Y, dimensions.Width, dimensions.Height), (Rectangle?)new Rectangle(0, 6, texture.Width, 4), color);
		spriteBatch.Draw(texture, new Rectangle(dimensions.X, dimensions.Y + dimensions.Height, dimensions.Width, 6), (Rectangle?)new Rectangle(0, texture.Height - 6, texture.Width, 6), color);
	}

	protected override void DrawSelf(SpriteBatch spriteBatch)
	{
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0117: Unknown result type (might be due to invalid IL or missing references)
		if (!AutoHide || CanScroll)
		{
			CalculatedStyle dimensions = GetDimensions();
			CalculatedStyle innerDimensions = GetInnerDimensions();
			if (_isDragging)
			{
				float num = UserInterface.ActiveInstance.MousePosition.Y - innerDimensions.Y - _dragYOffset;
				_viewPosition = MathHelper.Clamp(num / innerDimensions.Height * _maxViewSize, 0f, _maxViewSize - _viewSize);
			}
			Rectangle handleRectangle = GetHandleRectangle();
			Vector2 mousePosition = UserInterface.ActiveInstance.MousePosition;
			bool isHoveringOverHandle = _isHoveringOverHandle;
			_isHoveringOverHandle = handleRectangle.Contains(new Point((int)mousePosition.X, (int)mousePosition.Y));
			if (!isHoveringOverHandle && _isHoveringOverHandle)
			{
				SoundEngine.PlaySound(12);
			}
			DrawBar(spriteBatch, _texture.Value, dimensions.ToRectangle(), Color.White);
			DrawBar(spriteBatch, _innerTexture.Value, handleRectangle, Color.White * ((_isDragging || _isHoveringOverHandle) ? 1f : 0.85f));
		}
	}

	public override void LeftMouseDown(UIMouseEvent evt)
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		base.LeftMouseDown(evt);
		if (evt.Target == this)
		{
			Rectangle handleRectangle = GetHandleRectangle();
			if (handleRectangle.Contains(new Point((int)evt.MousePosition.X, (int)evt.MousePosition.Y)))
			{
				_isDragging = true;
				_dragYOffset = evt.MousePosition.Y - (float)handleRectangle.Y;
			}
			else
			{
				CalculatedStyle innerDimensions = GetInnerDimensions();
				float num = UserInterface.ActiveInstance.MousePosition.Y - innerDimensions.Y - (float)(handleRectangle.Height >> 1);
				_viewPosition = MathHelper.Clamp(num / innerDimensions.Height * _maxViewSize, 0f, _maxViewSize - _viewSize);
			}
		}
	}

	public override void LeftMouseUp(UIMouseEvent evt)
	{
		base.LeftMouseUp(evt);
		_isDragging = false;
	}
}
