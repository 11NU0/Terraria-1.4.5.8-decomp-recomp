using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;
using Terraria.UI;

namespace Terraria.Map;

public struct MapOverlayDrawContext(Vector2 mapPosition, Vector2 mapOffset, Rectangle? clippingRect, float mapScale, float drawScale, float opacity)
{
	public struct DrawResult(bool isMouseOver)
	{
		public static readonly DrawResult Culled = new DrawResult(isMouseOver: false);

		public readonly bool IsMouseOver = isMouseOver;
	}

	private readonly Vector2 _mapPosition = mapPosition;

	private readonly Vector2 _mapOffset = mapOffset;

	private readonly Rectangle? _clippingRect = clippingRect;

	private readonly float _mapScale = mapScale;

	private readonly float _drawScale = drawScale;

	private readonly float _opacity = opacity;

	public DrawResult Draw(Texture2D texture, Vector2 position, Alignment alignment)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		return Draw(texture, position, new SpriteFrame(1, 1), alignment);
	}

	public DrawResult Draw(Texture2D texture, Vector2 position, SpriteFrame frame, Alignment alignment)
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0106: Unknown result type (might be due to invalid IL or missing references)
		//IL_010b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		if (_opacity == 0f)
		{
			return new DrawResult(isMouseOver: false);
		}
		position = (position - _mapPosition) * _mapScale + _mapOffset;
		if (_clippingRect.HasValue)
		{
			Rectangle value = _clippingRect.Value;
			if (!value.Contains(position.ToPoint()))
			{
				return DrawResult.Culled;
			}
		}
		Rectangle sourceRectangle = frame.GetSourceRectangle(texture);
		Vector2 val = sourceRectangle.Size() * alignment.OffsetMultiplier;
		Main.spriteBatch.Draw(texture, position, (Rectangle?)sourceRectangle, Color.White * _opacity, 0f, val, _drawScale, (SpriteEffects)0, 0f);
		position -= val * _drawScale;
		Rectangle val2 = new Rectangle((int)position.X, (int)position.Y, (int)((float)sourceRectangle.Width * _drawScale), (int)((float)sourceRectangle.Height * _drawScale));
		return new DrawResult(val2.Contains(Main.MouseScreen.ToPoint()));
	}

	public Rectangle GetUnclampedDrawRegion(Texture2D texture, Vector2 position, SpriteFrame frame, float scale, Alignment alignment)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		position = (position - _mapPosition) * _mapScale + _mapOffset;
		Rectangle sourceRectangle = frame.GetSourceRectangle(texture);
		Vector2 val = sourceRectangle.Size() * alignment.OffsetMultiplier;
		float num = _drawScale * scale;
		Vector2 val2 = position - val * num;
		return new Rectangle((int)val2.X, (int)val2.Y, (int)((float)sourceRectangle.Width * num), (int)((float)sourceRectangle.Height * num));
	}

	public Rectangle GetClampedDrawRegion(Texture2D texture, Vector2 position, SpriteFrame frame, float scale, Alignment alignment, int screenBorderRegion)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_0108: Unknown result type (might be due to invalid IL or missing references)
		//IL_010f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_016f: Unknown result type (might be due to invalid IL or missing references)
		//IL_011b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0128: Unknown result type (might be due to invalid IL or missing references)
		//IL_0135: Unknown result type (might be due to invalid IL or missing references)
		//IL_013c: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_017a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0187: Unknown result type (might be due to invalid IL or missing references)
		//IL_0194: Unknown result type (might be due to invalid IL or missing references)
		//IL_022f: Unknown result type (might be due to invalid IL or missing references)
		//IL_01db: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fc: Unknown result type (might be due to invalid IL or missing references)
		position = (position - _mapPosition) * _mapScale + _mapOffset;
		Rectangle sourceRectangle = frame.GetSourceRectangle(texture);
		Vector2 val = sourceRectangle.Size() * alignment.OffsetMultiplier;
		float num = _drawScale * scale;
		Vector2 val2 = position - val * num;
		Rectangle val3 = new Rectangle((int)val2.X, (int)val2.Y, (int)((float)sourceRectangle.Width * num), (int)((float)sourceRectangle.Height * num));
		int num2 = Main.screenWidth - screenBorderRegion;
		int num3 = Main.screenHeight - screenBorderRegion;
		int num4 = (screenBorderRegion + num2) / 2;
		int num5 = (screenBorderRegion + num3) / 2;
		if (val3.X < screenBorderRegion)
		{
			float num6 = val3.X - num4;
			float num7 = val3.Y - num5;
			int num8 = val3.X - screenBorderRegion;
			int num9 = (int)((float)num8 / num6 * num7);
			val3.X -= num8;
			val3.Y -= num9;
		}
		else if (val3.X + val3.Width > num2)
		{
			float num10 = val3.X - num4;
			float num11 = val3.Y - num5;
			int num12 = val3.X + val3.Width - num2;
			int num13 = (int)((float)num12 / num10 * num11);
			val3.X -= num12;
			val3.Y -= num13;
		}
		if (val3.Y < screenBorderRegion)
		{
			float num14 = val3.X - num4;
			float num15 = val3.Y - num5;
			int num16 = val3.Y - screenBorderRegion;
			int num17 = (int)((float)num16 / num15 * num14);
			val3.X -= num17;
			val3.Y -= num16;
		}
		else if (val3.Y + val3.Height > num3)
		{
			float num18 = val3.X - num4;
			float num19 = val3.Y - num5;
			int num20 = val3.Y + val3.Height - num3;
			int num21 = (int)((float)num20 / num19 * num18);
			val3.X -= num21;
			val3.Y -= num20;
		}
		return val3;
	}

	public DrawResult DrawClamped(Texture2D texture, Texture2D offscreenTexture, Vector2 position, Color color, SpriteFrame frame, float scaleIfNotSelected, float scaleIfSelected, float scaleIfOffscreen, Alignment alignment, int screenBorderRegion, out bool onScreen)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0153: Unknown result type (might be due to invalid IL or missing references)
		//IL_015a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01da: Unknown result type (might be due to invalid IL or missing references)
		//IL_016a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0177: Unknown result type (might be due to invalid IL or missing references)
		//IL_0184: Unknown result type (might be due to invalid IL or missing references)
		//IL_018b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0256: Unknown result type (might be due to invalid IL or missing references)
		//IL_025d: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0203: Unknown result type (might be due to invalid IL or missing references)
		//IL_02df: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_026d: Unknown result type (might be due to invalid IL or missing references)
		//IL_027a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0287: Unknown result type (might be due to invalid IL or missing references)
		//IL_028e: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_03fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_033f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0340: Unknown result type (might be due to invalid IL or missing references)
		//IL_0341: Unknown result type (might be due to invalid IL or missing references)
		//IL_0346: Unknown result type (might be due to invalid IL or missing references)
		//IL_0348: Unknown result type (might be due to invalid IL or missing references)
		//IL_035a: Unknown result type (might be due to invalid IL or missing references)
		//IL_035f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0364: Unknown result type (might be due to invalid IL or missing references)
		//IL_0366: Unknown result type (might be due to invalid IL or missing references)
		//IL_0368: Unknown result type (might be due to invalid IL or missing references)
		//IL_036a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0376: Unknown result type (might be due to invalid IL or missing references)
		//IL_037b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0380: Unknown result type (might be due to invalid IL or missing references)
		//IL_038b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0390: Unknown result type (might be due to invalid IL or missing references)
		//IL_0399: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_03bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03cd: Unknown result type (might be due to invalid IL or missing references)
		onScreen = true;
		if (_opacity == 0f)
		{
			return new DrawResult(isMouseOver: false);
		}
		position = (position - _mapPosition) * _mapScale + _mapOffset;
		Rectangle sourceRectangle = frame.GetSourceRectangle(texture);
		Vector2 val = sourceRectangle.Size() * alignment.OffsetMultiplier;
		Vector2 val2 = position;
		float num = _drawScale * scaleIfNotSelected;
		float num2 = _drawScale * scaleIfOffscreen;
		Vector2 val3 = position - val * num;
		Rectangle val4 = new Rectangle((int)val3.X, (int)val3.Y, (int)((float)sourceRectangle.Width * num), (int)((float)sourceRectangle.Height * num));
		int num3 = Main.screenWidth - screenBorderRegion;
		int num4 = Main.screenHeight - screenBorderRegion;
		int num5 = (screenBorderRegion + num3) / 2;
		int num6 = (screenBorderRegion + num4) / 2;
		if (val4.X < screenBorderRegion)
		{
			float num7 = val4.X - num5;
			float num8 = val4.Y - num6;
			int num9 = val4.X - screenBorderRegion;
			int num10 = (int)((float)num9 / num7 * num8);
			val2.X -= num9;
			val4.X -= num9;
			val2.Y -= num10;
			val4.Y -= num10;
			onScreen = false;
		}
		else if (val4.X + val4.Width > num3)
		{
			onScreen = false;
			float num11 = val4.X - num5;
			float num12 = val4.Y - num6;
			int num13 = val4.X + val4.Width - num3;
			int num14 = (int)((float)num13 / num11 * num12);
			val2.X -= num13;
			val4.X -= num13;
			val2.Y -= num14;
			val4.Y -= num14;
		}
		if (val4.Y < screenBorderRegion)
		{
			onScreen = false;
			float num15 = val4.X - num5;
			float num16 = val4.Y - num6;
			int num17 = val4.Y - screenBorderRegion;
			int num18 = (int)((float)num17 / num16 * num15);
			val2.X -= num18;
			val4.X -= num18;
			val2.Y -= num17;
			val4.Y -= num17;
		}
		else if (val4.Y + val4.Height > num4)
		{
			onScreen = false;
			float num19 = val4.X - num5;
			float num20 = val4.Y - num6;
			int num21 = val4.Y + val4.Height - num4;
			int num22 = (int)((float)num21 / num20 * num19);
			val2.X -= num22;
			val4.X -= num22;
			val2.Y -= num21;
			val4.Y -= num21;
		}
		bool flag = val4.Contains(Main.MouseScreen.ToPoint());
		float num23 = num;
		if (!onScreen)
		{
			num23 = num2;
			if (flag)
			{
				num23 *= scaleIfSelected;
			}
		}
		else if (flag)
		{
			num23 = _drawScale * scaleIfSelected;
		}
		if (!onScreen && !flag)
		{
			int frameX = 2;
			int frameY = 1;
			_ = offscreenTexture.Width / 3;
			_ = offscreenTexture.Height / 3;
			Vector2 val5 = position - val2;
			float num24 = val5.ToRotation();
			val5.Normalize();
			Vector2 val6 = val4.Center.ToVector2();
			val6 += val5 * ((float)(sourceRectangle.Height / 4) * num2);
			Rectangle val7 = offscreenTexture.Frame(3, 3, frameX, frameY);
			Vector2 val8 = new Vector2(0f, (float)val7.Height * 0.5f);
			Main.spriteBatch.Draw(offscreenTexture, val6, (Rectangle?)val7, Color.White * _opacity, num24, val8, num2, (SpriteEffects)0, 0f);
		}
		Main.spriteBatch.Draw(texture, val2, (Rectangle?)sourceRectangle, color * _opacity, 0f, val, num23, (SpriteEffects)0, 0f);
		return new DrawResult(flag);
	}

	public DrawResult Draw(Texture2D texture, Vector2 position, Color color, SpriteFrame frame, float scaleIfNotSelected, float scaleIfSelected, Alignment alignment)
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0100: Unknown result type (might be due to invalid IL or missing references)
		//IL_0107: Unknown result type (might be due to invalid IL or missing references)
		//IL_0111: Unknown result type (might be due to invalid IL or missing references)
		if (_opacity == 0f)
		{
			return new DrawResult(isMouseOver: false);
		}
		position = (position - _mapPosition) * _mapScale + _mapOffset;
		if (_clippingRect.HasValue)
		{
			Rectangle value = _clippingRect.Value;
			if (!value.Contains(position.ToPoint()))
			{
				return DrawResult.Culled;
			}
		}
		Rectangle sourceRectangle = frame.GetSourceRectangle(texture);
		Vector2 val = sourceRectangle.Size() * alignment.OffsetMultiplier;
		Vector2 val2 = position;
		float num = _drawScale * scaleIfNotSelected;
		Vector2 val3 = position - val * num;
		Rectangle val4 = new Rectangle((int)val3.X, (int)val3.Y, (int)((float)sourceRectangle.Width * num), (int)((float)sourceRectangle.Height * num));
		bool flag = val4.Contains(Main.MouseScreen.ToPoint());
		float num2 = num;
		if (flag)
		{
			num2 = _drawScale * scaleIfSelected;
		}
		Main.spriteBatch.Draw(texture, val2, (Rectangle?)sourceRectangle, color * _opacity, 0f, val, num2, (SpriteEffects)0, 0f);
		return new DrawResult(flag);
	}
}
