using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Terraria.GameContent.Drawing;

public class HorizonHelper
{
	public static bool DebugSunVisibility = false;

	private readonly int SampleAreaSize = 128;

	private readonly int SmallTextureSize = 64;

	private RenderTarget2D _tinyTarget;

	private RenderTarget2D _pixelTarget;

	private bool _targetUpToDate;

	private BlendState _horizonBlendState = new BlendState
	{
		AlphaSourceBlend = (Blend)1,
		AlphaDestinationBlend = (Blend)5,
		ColorSourceBlend = (Blend)1,
		ColorDestinationBlend = (Blend)5
	};

	private static Color[] MoonColors = new Color[9]
	{
		new Color(230, 235, 255),
		new Color(250, 235, 160),
		new Color(230, 255, 230),
		new Color(160, 240, 255),
		new Color(180, 255, 255),
		new Color(230, 255, 230),
		new Color(255, 180, 255),
		new Color(255, 200, 180),
		new Color(225, 180, 255)
	};

	public bool SunVisibilityEnabled => _targetUpToDate;

	public Texture2D SunVisibilityPixelTexture => (Texture2D)(object)_pixelTarget;

	public HorizonHelper()
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Expected Obj, but got Unknown
	}

	public void UpdateSunVisibility(RenderTarget2D bigTarget)
	{
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Expected Obj, but got Unknown
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Expected Obj, but got Unknown
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_010b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0110: Unknown result type (might be due to invalid IL or missing references)
		//IL_0116: Unknown result type (might be due to invalid IL or missing references)
		//IL_0137: Unknown result type (might be due to invalid IL or missing references)
		//IL_0172: Unknown result type (might be due to invalid IL or missing references)
		//IL_0177: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		_targetUpToDate = false;
		if (Main.ForegroundSunlightEffects && bigTarget != null)
		{
			TimeLogger.StartTimestamp fromTimestamp = TimeLogger.Start();
			GraphicsDevice graphicsDevice = ((Game)Main.instance).GraphicsDevice;
			if (_tinyTarget == null || _tinyTarget.IsContentLost)
			{
				_tinyTarget = new RenderTarget2D(graphicsDevice, SmallTextureSize, SmallTextureSize, true, (SurfaceFormat)12, (DepthFormat)0);
			}
			if (_pixelTarget == null || _pixelTarget.IsContentLost)
			{
				_pixelTarget = new RenderTarget2D(graphicsDevice, 1, 1, false, (SurfaceFormat)12, (DepthFormat)0);
			}
			Rectangle val = Utils.CenteredRectangle(Main.ReverseGravitySupport(Main.LastCelestialBodyPosition * Main.ScreenSize.ToVector2()), new Vector2((float)SampleAreaSize) * Main.BackgroundViewMatrix.RenderZoom);
			if (DebugSunVisibility)
			{
				Test_DrawSmallTarget(bigTarget, val);
			}
			graphicsDevice.SetRenderTarget(_tinyTarget);
			graphicsDevice.Clear(Color.Transparent);
			Main.spriteBatch.Begin((SpriteSortMode)1, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullCounterClockwise);
			Main.spriteBatch.Draw((Texture2D)(object)bigTarget, ((Texture2D)_tinyTarget).Bounds, (Rectangle?)val, Color.White);
			Main.spriteBatch.End();
			graphicsDevice.SetRenderTarget(_pixelTarget);
			graphicsDevice.Clear(Color.White);
			Main.spriteBatch.Begin((SpriteSortMode)1, _horizonBlendState, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullCounterClockwise);
			Main.spriteBatch.Draw((Texture2D)(object)_tinyTarget, ((Texture2D)_pixelTarget).Bounds, Color.White);
			Main.spriteBatch.End();
			graphicsDevice.SetRenderTarget((RenderTarget2D)null);
			_targetUpToDate = true;
			TimeLogger.SunVisibility.AddTime(fromTimestamp);
		}
	}

	private void Test_DrawSmallTarget(RenderTarget2D bigTarget, Rectangle sunSampleRect)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Expected Obj, but got Unknown
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_010b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0112: Unknown result type (might be due to invalid IL or missing references)
		//IL_0117: Unknown result type (might be due to invalid IL or missing references)
		//IL_013e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0145: Unknown result type (might be due to invalid IL or missing references)
		//IL_014a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0163: Unknown result type (might be due to invalid IL or missing references)
		//IL_016b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0171: Unknown result type (might be due to invalid IL or missing references)
		//IL_0176: Unknown result type (might be due to invalid IL or missing references)
		//IL_0190: Unknown result type (might be due to invalid IL or missing references)
		//IL_0196: Unknown result type (might be due to invalid IL or missing references)
		//IL_019d: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f3: Unknown result type (might be due to invalid IL or missing references)
		GraphicsDevice graphicsDevice = ((Game)Main.instance).GraphicsDevice;
		graphicsDevice.SetRenderTarget(bigTarget);
		Main.spriteBatch.Begin((SpriteSortMode)1, new BlendState
		{
			ColorDestinationBlend = (Blend)1,
			ColorSourceBlend = (Blend)4,
			AlphaDestinationBlend = (Blend)1,
			AlphaSourceBlend = (Blend)4
		}, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullCounterClockwise);
		Main.spriteBatch.Draw((Texture2D)(object)_tinyTarget, new Rectangle(0, 0, sunSampleRect.Width, sunSampleRect.Height), Color.White);
		Main.spriteBatch.End();
		Main.spriteBatch.Begin();
		Main.spriteBatch.Draw(TextureAssets.MagicPixel.Value, new Rectangle(sunSampleRect.Left, sunSampleRect.Top, 1, sunSampleRect.Height), Color.Red);
		Main.spriteBatch.Draw(TextureAssets.MagicPixel.Value, new Rectangle(sunSampleRect.Right, sunSampleRect.Top, 1, sunSampleRect.Height), Color.Red);
		Main.spriteBatch.Draw(TextureAssets.MagicPixel.Value, new Rectangle(sunSampleRect.Left, sunSampleRect.Top, sunSampleRect.Width, 1), Color.Red);
		Main.spriteBatch.Draw(TextureAssets.MagicPixel.Value, new Rectangle(sunSampleRect.Left, sunSampleRect.Bottom, sunSampleRect.Width, 1), Color.Red);
		Main.spriteBatch.Draw(TextureAssets.MagicPixel.Value, new Rectangle(sunSampleRect.Width, 0, 1, sunSampleRect.Height), Color.Red);
		Main.spriteBatch.Draw(TextureAssets.MagicPixel.Value, new Rectangle(0, sunSampleRect.Height, sunSampleRect.Width, 1), Color.Red);
		byte[] array = new byte[1];
		((Texture2D)_pixelTarget).GetData<byte>(array);
		Utils.DrawBorderString(text: $"{(float)(int)array[0] / 255f:F3}", sb: Main.spriteBatch, pos: new Vector2(10f, (float)(sunSampleRect.Height + 20)), color: Color.White);
		Main.spriteBatch.End();
		graphicsDevice.SetRenderTarget((RenderTarget2D)null);
	}

	public static void GetCelestialBodyColors(out Color sunColor, out Color moonColor)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		sunColor = new Color(255, 246, 204);
		moonColor = GetMoonColor() * GetMoonStrength();
	}

	private static Color GetMoonColor()
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		Color val = new Color(230, 235, 255);
		int num = Main.moonType;
		if (!TextureAssets.Moon.IndexInRange(num))
		{
			num = Utils.Clamp(num, 0, 8);
		}
		val = MoonColors[num];
		if (Main.pumpkinMoon)
		{
			val = new Color(255, 225, 180);
		}
		if (Main.snowMoon)
		{
			val = new Color(220, 220, 255);
		}
		if (WorldGen.drunkWorldGen)
		{
			val = new Color(255, 255, 255);
		}
		return val;
	}

	public static float GetMoonStrength()
	{
		return Utils.Remap(Math.Abs(4 - Main.moonPhase), 0f, 4f, 0f, 1f);
	}

	static HorizonHelper()
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
	}
}
