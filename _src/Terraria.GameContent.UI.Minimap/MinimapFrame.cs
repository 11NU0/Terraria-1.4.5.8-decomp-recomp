using System;
using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameInput;
using Terraria.Testing;

namespace Terraria.GameContent.UI.Minimap;

public class MinimapFrame : IConfigKeyHolder
{
	private class Button
	{
		public bool IsHighlighted;

		private readonly Vector2 _position;

		private readonly Asset<Texture2D> _hoverTexture;

		private readonly Action _onMouseDown;

		private Vector2 Size
		{
			get
			{
				//IL_0018: Unknown result type (might be due to invalid IL or missing references)
				return new Vector2((float)_hoverTexture.Width(), (float)_hoverTexture.Height());
			}
		}

		public Button(Asset<Texture2D> hoverTexture, Vector2 position, Action mouseDownCallback)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			_position = position;
			_hoverTexture = hoverTexture;
			_onMouseDown = mouseDownCallback;
		}

		public void Click()
		{
			_onMouseDown();
		}

		public void Draw(SpriteBatch spriteBatch, Vector2 parentPosition)
		{
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			if (IsHighlighted)
			{
				spriteBatch.Draw(_hoverTexture.Value, _position + parentPosition, Color.White);
			}
		}

		public bool IsTouchingPoint(Vector2 testPoint, Vector2 parentPosition)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_006d: Unknown result type (might be due to invalid IL or missing references)
			Vector2 val = _position + parentPosition + Size * 0.5f;
			Vector2 val2 = Vector2.Max(Size, new Vector2(22f, 22f)) * 0.5f;
			Vector2 val3 = testPoint - val;
			if (Math.Abs(val3.X) < val2.X)
			{
				return Math.Abs(val3.Y) < val2.Y;
			}
			return false;
		}
	}

	private const float DEFAULT_ZOOM = 1.05f;

	private const float ZOOM_OUT_MULTIPLIER = 0.975f;

	private const float ZOOM_IN_MULTIPLIER = 1.025f;

	private readonly Asset<Texture2D> _frameTexture;

	private readonly Vector2 _frameOffset;

	private Button _resetButton;

	private Button _zoomInButton;

	private Button _zoomOutButton;

	public string ConfigKey { get; set; }

	public string NameKey { get; set; }

	public Vector2 MinimapPosition
	{
		[CompilerGenerated]
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return field;
		}
		[CompilerGenerated]
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			field = value;
		}
	}

	private Vector2 FramePosition
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			return MinimapPosition + _frameOffset;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0003: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			MinimapPosition = value - _frameOffset;
		}
	}

	public MinimapFrame(Asset<Texture2D> frameTexture, Vector2 frameOffset)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		_frameTexture = frameTexture;
		_frameOffset = frameOffset;
	}

	public void SetResetButton(Asset<Texture2D> hoverTexture, Vector2 position)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		_resetButton = new Button(hoverTexture, position, () =>
		{
			ResetZoom();
		});
	}

	private void ResetZoom()
	{
		Main.mapMinimapScale = 1.05f;
	}

	public void SetZoomInButton(Asset<Texture2D> hoverTexture, Vector2 position)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		_zoomInButton = new Button(hoverTexture, position, () =>
		{
			ZoomInButton();
		});
	}

	private void ZoomInButton()
	{
		Main.mapMinimapScale *= 1.025f;
	}

	public void SetZoomOutButton(Asset<Texture2D> hoverTexture, Vector2 position)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		_zoomOutButton = new Button(hoverTexture, position, () =>
		{
			ZoomOutButton();
		});
	}

	private void ZoomOutButton()
	{
		Main.mapMinimapScale *= 0.975f;
	}

	public void Update()
	{
		ValidateState();
		Button button = null;
		if (_zoomInButton.IsHighlighted)
		{
			button = _zoomInButton;
		}
		if (_zoomOutButton.IsHighlighted)
		{
			button = _zoomOutButton;
		}
		if (_resetButton.IsHighlighted)
		{
			button = _resetButton;
		}
		_zoomInButton.IsHighlighted = false;
		_zoomOutButton.IsHighlighted = false;
		_resetButton.IsHighlighted = false;
		Button buttonUnderMouse = GetButtonUnderMouse();
		if (buttonUnderMouse == null || PlayerInput.IgnoreMouseInterface || Main.LocalPlayer.controlTorch)
		{
			return;
		}
		Main.instance.MouseTextNoOverride(string.Empty, 0, 0);
		buttonUnderMouse.IsHighlighted = true;
		Main.LocalPlayer.mouseInterface = true;
		if (button != buttonUnderMouse)
		{
			SoundEngine.PlaySound(12);
		}
		if (Main.mouseLeft)
		{
			buttonUnderMouse.Click();
			if (Main.mouseLeftRelease)
			{
				SoundEngine.PlaySound(12);
			}
		}
	}

	public void DrawBackground(SpriteBatch spriteBatch)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		ValidateState();
		spriteBatch.Draw(TextureAssets.MagicPixel.Value, new Rectangle((int)MinimapPosition.X - 6, (int)MinimapPosition.Y - 6, 244, 244), Color.Black * Main.mapMinimapAlpha);
	}

	public void DrawForeground(SpriteBatch spriteBatch)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		ValidateState();
		spriteBatch.Draw(_frameTexture.Value, FramePosition, Color.White);
		_zoomInButton.Draw(spriteBatch, FramePosition);
		_zoomOutButton.Draw(spriteBatch, FramePosition);
		_resetButton.Draw(spriteBatch, FramePosition);
	}

	private Button GetButtonUnderMouse()
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		Vector2 testPoint = new Vector2((float)Main.mouseX, (float)Main.mouseY);
		if (_zoomInButton.IsTouchingPoint(testPoint, FramePosition))
		{
			return _zoomInButton;
		}
		if (_zoomOutButton.IsTouchingPoint(testPoint, FramePosition))
		{
			return _zoomOutButton;
		}
		if (_resetButton.IsTouchingPoint(testPoint, FramePosition))
		{
			return _resetButton;
		}
		return null;
	}

	private void ValidateState()
	{
		Invariant.Assert(_zoomInButton != null, "Zoom In button texture must be set.");
		Invariant.Assert(_zoomOutButton != null, "Zoom Out button texture must be set.");
		Invariant.Assert(_resetButton != null, "Reset button texture must be set.");
	}
}
