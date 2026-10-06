using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Terraria.Graphics;

public class SpriteViewMatrix
{
	private Vector2 _zoom = Vector2.One;

	private Vector2 _translation = Vector2.Zero;

	private Matrix _zoomMatrix = Matrix.Identity;

	private Matrix _transformationMatrix = Matrix.Identity;

	private Matrix _normalizedTransformationMatrix = Matrix.Identity;

	private SpriteEffects _effects;

	private Matrix _effectMatrix;

	private GraphicsDevice _graphicsDevice;

	private Viewport _viewport;

	private bool _overrideSystemViewport;

	private bool _needsRebuild = true;

	private const float PixelPerfectOffset = 1f / 256f;

	private const float PixelPerfectSafeZoomLevelStep = 1f / 128f;

	public Vector2 Zoom
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return _zoom;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			if (_zoom != value)
			{
				_zoom = value;
				_needsRebuild = true;
			}
		}
	}

	public Vector2 Translation
	{
		get
		{
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			if (ShouldRebuild())
			{
				Rebuild();
			}
			return _translation;
		}
	}

	public Matrix ZoomMatrix
	{
		get
		{
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			if (ShouldRebuild())
			{
				Rebuild();
			}
			return _zoomMatrix;
		}
	}

	public Matrix TransformationMatrix
	{
		get
		{
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			if (ShouldRebuild())
			{
				Rebuild();
			}
			return _transformationMatrix;
		}
	}

	public Matrix NormalizedTransformationMatrix
	{
		get
		{
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			if (ShouldRebuild())
			{
				Rebuild();
			}
			return _normalizedTransformationMatrix;
		}
	}

	public Vector2 RenderZoom
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			return new Vector2(ZoomMatrix.M11, ZoomMatrix.M22);
		}
	}

	public SpriteEffects Effects
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return _effects;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			if (_effects != value)
			{
				_effects = value;
				_needsRebuild = true;
			}
		}
	}

	public Matrix EffectMatrix
	{
		get
		{
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			if (ShouldRebuild())
			{
				Rebuild();
			}
			return _effectMatrix;
		}
	}

	public SpriteViewMatrix(GraphicsDevice graphicsDevice)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		_graphicsDevice = graphicsDevice;
	}

	private void Rebuild()
	{
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0104: Unknown result type (might be due to invalid IL or missing references)
		//IL_0119: Unknown result type (might be due to invalid IL or missing references)
		//IL_011e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0121: Unknown result type (might be due to invalid IL or missing references)
		//IL_0122: Unknown result type (might be due to invalid IL or missing references)
		//IL_0128: Unknown result type (might be due to invalid IL or missing references)
		//IL_012f: Unknown result type (might be due to invalid IL or missing references)
		//IL_013b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0140: Unknown result type (might be due to invalid IL or missing references)
		//IL_0146: Unknown result type (might be due to invalid IL or missing references)
		//IL_0151: Unknown result type (might be due to invalid IL or missing references)
		//IL_0156: Unknown result type (might be due to invalid IL or missing references)
		//IL_015b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0161: Unknown result type (might be due to invalid IL or missing references)
		//IL_0162: Unknown result type (might be due to invalid IL or missing references)
		//IL_0168: Unknown result type (might be due to invalid IL or missing references)
		//IL_016a: Unknown result type (might be due to invalid IL or missing references)
		//IL_016f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0174: Unknown result type (might be due to invalid IL or missing references)
		//IL_0188: Unknown result type (might be due to invalid IL or missing references)
		//IL_018d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0191: Unknown result type (might be due to invalid IL or missing references)
		//IL_0196: Unknown result type (might be due to invalid IL or missing references)
		//IL_0198: Unknown result type (might be due to invalid IL or missing references)
		//IL_019d: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01da: Unknown result type (might be due to invalid IL or missing references)
		//IL_01df: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		if (!_overrideSystemViewport)
		{
			_viewport = _graphicsDevice.Viewport;
		}
		Vector2 val = new Vector2((float)_viewport.Width, (float)_viewport.Height);
		Matrix val2 = Matrix.Identity;
		if (((int)_effects & 1) != 0)
		{
			val2 *= Matrix.CreateScale(-1f, 1f, 1f) * Matrix.CreateTranslation(val.X, 0f, 0f);
		}
		if (((int)_effects & 2) != 0)
		{
			val2 *= Matrix.CreateScale(1f, -1f, 1f) * Matrix.CreateTranslation(0f, val.Y, 0f);
		}
		Vector2 val3 = Utils.Round(_zoom / (1f / 128f)) * (1f / 128f);
		Vector2 val4 = val * 0.5f;
		Vector2 val5 = Utils.Round(val4 - val4 / val3);
		Matrix val6 = Matrix.CreateOrthographicOffCenter(0f, val.X, val.Y, 0f, 0f, 1f);
		_translation = val5;
		_zoomMatrix = Matrix.CreateTranslation(0f - val5.X, 0f - val5.Y, 0f) * Matrix.CreateScale(val3.X, val3.Y, 1f);
		_effectMatrix = val2;
		_transformationMatrix = val2 * _zoomMatrix;
		Matrix val7 = Matrix.CreateTranslation(1f / 256f, 1f / 256f, 0f);
		_transformationMatrix *= val7;
		Matrix val8 = Matrix.CreateTranslation(-0.5f, -0.5f, 0f);
		_normalizedTransformationMatrix = Matrix.Invert(val2) * _zoomMatrix * val7 * val8 * val6;
		_needsRebuild = false;
	}

	public void SetViewportOverride(Viewport viewport)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		_viewport = viewport;
		_overrideSystemViewport = true;
	}

	public void ClearViewportOverride()
	{
		_overrideSystemViewport = false;
	}

	private bool ShouldRebuild()
	{
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		if (!_needsRebuild)
		{
			if (!_overrideSystemViewport && !_graphicsDevice.IsDisposed)
			{
				Viewport viewport = _graphicsDevice.Viewport;
				if (viewport.Width == _viewport.Width)
				{
					viewport = _graphicsDevice.Viewport;
					return viewport.Height != _viewport.Height;
				}
				return true;
			}
			return false;
		}
		return true;
	}
}
