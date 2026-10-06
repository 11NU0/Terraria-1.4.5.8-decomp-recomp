using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent.Events;

namespace Terraria.GameContent.UI;

public class UIDust
{
	public const int maxDust = 200;

	public Dust[] dust = new Dust[201];

	public void Clear()
	{
		for (int i = 0; i < 201; i++)
		{
			dust[i] = new Dust();
		}
	}

	public Dust NewDustPerfect(Vector2 Position, int Type, Vector2? Velocity = null, int Alpha = 0, Color newColor = default(Color), float Scale = 1f)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		Dust dust = NewDust(Position, 0, 0, Type, 0f, 0f, Alpha, newColor, Scale);
		dust.position = Position;
		if (Velocity.HasValue)
		{
			dust.velocity = Velocity.Value;
		}
		return dust;
	}

	public Dust NewDustDirect(Vector2 Position, int Width, int Height, int Type, float SpeedX = 0f, float SpeedY = 0f, int Alpha = 0, Color newColor = default(Color), float Scale = 1f)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		Dust dust = NewDust(Position, Width, Height, Type, SpeedX, SpeedY, Alpha, newColor, Scale);
		if (dust.velocity.HasNaNs())
		{
			dust.velocity = Vector2.Zero;
		}
		return dust;
	}

	public Dust NewDust(Vector2 Position, int Width, int Height, int Type, float SpeedX = 0f, float SpeedY = 0f, int Alpha = 0, Color newColor = default(Color), float Scale = 1f)
	{
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		if (Type != 6 && Type != 267)
		{
			throw new Exception();
		}
		int num = 200;
		for (int i = 0; i < 200; i++)
		{
			Dust dust = this.dust[i];
			if (!dust.active)
			{
				int num2 = Width;
				int num3 = Height;
				if (num2 < 5)
				{
					num2 = 5;
				}
				if (num3 < 5)
				{
					num3 = 5;
				}
				num = i;
				dust.fadeIn = 0f;
				dust.active = true;
				dust.type = Type;
				dust.noGravity = false;
				dust.color = newColor;
				dust.alpha = Alpha;
				dust.position.X = Position.X + (float)Main.rand.Next(num2 - 4) + 4f;
				dust.position.Y = Position.Y + (float)Main.rand.Next(num3 - 4) + 4f;
				dust.velocity.X = (float)Main.rand.Next(-20, 21) * 0.1f + SpeedX;
				dust.velocity.Y = (float)Main.rand.Next(-20, 21) * 0.1f + SpeedY;
				dust.frame.X = 10 * Type;
				dust.frame.Y = 10 * Main.rand.Next(3);
				dust.shader = null;
				dust.customData = null;
				dust.noLightEmittance = false;
				dust.fullBright = false;
				int num4 = Type;
				while (num4 >= 100)
				{
					num4 -= 100;
					dust.frame.X -= 1000;
					dust.frame.Y += 30;
				}
				dust.frame.Width = 8;
				dust.frame.Height = 8;
				dust.rotation = 0f;
				dust.scale = 1f + (float)Main.rand.Next(-20, 21) * 0.01f;
				dust.scale *= Scale;
				dust.noLight = false;
				dust.firstFrame = true;
				if (dust.type == 6)
				{
					dust.velocity.Y = (float)Main.rand.Next(-10, 6) * 0.1f;
					dust.velocity.X *= 0.3f;
					dust.scale *= 0.7f;
				}
				break;
			}
		}
		return this.dust[num];
	}

	public Dust CloneDust(int dustIndex)
	{
		Dust rf = dust[dustIndex];
		return CloneDust(rf);
	}

	public Dust CloneDust(Dust rf)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		Dust dust = NewDust(rf.position, 0, 0, rf.type);
		dust.position = rf.position;
		dust.velocity = rf.velocity;
		dust.fadeIn = rf.fadeIn;
		dust.noGravity = rf.noGravity;
		dust.scale = rf.scale;
		dust.rotation = rf.rotation;
		dust.noLight = rf.noLight;
		dust.active = rf.active;
		dust.type = rf.type;
		dust.color = rf.color;
		dust.alpha = rf.alpha;
		dust.frame = rf.frame;
		dust.shader = rf.shader;
		dust.customData = rf.customData;
		return dust;
	}

	public void QuickBox(Vector2 topLeft, Vector2 bottomRight, int divisions, Color color, Action<Dust> manipulator)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		float num = divisions + 2;
		for (float num2 = 0f; num2 <= (float)(divisions + 2); num2++)
		{
			Dust obj = QuickDust(new Vector2(MathHelper.Lerp(topLeft.X, bottomRight.X, num2 / num), topLeft.Y), color);
			manipulator?.Invoke(obj);
			obj = QuickDust(new Vector2(MathHelper.Lerp(topLeft.X, bottomRight.X, num2 / num), bottomRight.Y), color);
			manipulator?.Invoke(obj);
			obj = QuickDust(new Vector2(topLeft.X, MathHelper.Lerp(topLeft.Y, bottomRight.Y, num2 / num)), color);
			manipulator?.Invoke(obj);
			obj = QuickDust(new Vector2(bottomRight.X, MathHelper.Lerp(topLeft.Y, bottomRight.Y, num2 / num)), color);
			manipulator?.Invoke(obj);
		}
	}

	public void QuickCircle(Vector2 center, float radius, int divisions, Color color, Action<Dust> manipulator)
	{
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		float num = 1f / Math.Max(1f, divisions);
		for (float num2 = 0f; num2 < 1f; num2 += num)
		{
			float num3 = num2 * ((float)Math.PI * 2f);
			Vector2 val = center;
			val += new Vector2(radius, 0f).RotatedBy(num3, Vector2.Zero);
			Dust obj = QuickDust(val, color);
			manipulator?.Invoke(obj);
		}
	}

	public Dust QuickDust(Vector2 pos, Color color)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		Dust dust = NewDust(pos, 0, 0, 267);
		dust.position = pos;
		dust.velocity = Vector2.Zero;
		dust.fadeIn = 1f;
		dust.noLight = true;
		dust.noGravity = true;
		dust.color = color;
		return dust;
	}

	public Dust QuickDustSmall(Vector2 pos, Color color, bool floorPositionValues = false)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		Dust dust = QuickDust(pos, color);
		dust.fadeIn = 0f;
		dust.scale = 0.35f;
		if (floorPositionValues)
		{
			dust.position = dust.position.Floor();
		}
		return dust;
	}

	public void QuickDustLine(Vector2 start, Vector2 end, float splits, Color color)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		QuickDust(start, color).scale = 0.3f;
		QuickDust(end, color).scale = 0.3f;
		float num = 1f / splits;
		for (float num2 = 0f; num2 < 1f; num2 += num)
		{
			QuickDust(Vector2.Lerp(start, end, num2), color).scale = 0.3f;
		}
	}

	public void UpdateDust()
	{
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_018a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0194: Unknown result type (might be due to invalid IL or missing references)
		//IL_0199: Unknown result type (might be due to invalid IL or missing references)
		Sandstorm.ShowSandstormVisuals();
		for (int i = 0; i < 200; i++)
		{
			Dust dust = this.dust[i];
			if (!dust.active)
			{
				continue;
			}
			if (dust.scale > 10f)
			{
				dust.active = false;
			}
			dust.position += dust.velocity;
			if (dust.type == 6)
			{
				if (!dust.noGravity)
				{
					dust.velocity.Y += 0.05f;
				}
			}
			else if (dust.type == 267)
			{
				if (dust.velocity.X < 0f)
				{
					dust.rotation--;
				}
				else
				{
					dust.rotation++;
				}
				dust.velocity.Y *= 0.98f;
				dust.velocity.X *= 0.98f;
				dust.scale += 0.02f;
			}
			dust.velocity.X *= 0.99f;
			dust.rotation += dust.velocity.X * 0.5f;
			if (dust.fadeIn > 0f && dust.fadeIn < 100f)
			{
				dust.scale += 0.03f;
				if (dust.scale > dust.fadeIn)
				{
					dust.fadeIn = 0f;
				}
			}
			else
			{
				dust.scale -= 0.01f;
			}
			if (dust.noGravity)
			{
				dust.velocity *= 0.92f;
				if (dust.fadeIn == 0f)
				{
					dust.scale -= 0.04f;
				}
			}
			float num = 0.1f;
			if (dust.scale < num)
			{
				dust.active = false;
			}
		}
	}

	internal void DrawDust()
	{
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_011b: Unknown result type (might be due to invalid IL or missing references)
		//IL_011d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_010a: Unknown result type (might be due to invalid IL or missing references)
		SpriteBatch spriteBatch = Main.spriteBatch;
		for (int i = 0; i < 200; i++)
		{
			Dust dust = this.dust[i];
			if (!dust.active)
			{
				continue;
			}
			float visualScale = dust.GetVisualScale();
			Color newColor = Lighting.GetColor((int)((double)dust.position.X + 4.0) / 16, (int)((double)dust.position.Y + 4.0) / 16);
			if (dust.type == 6)
			{
				newColor = Color.White;
			}
			newColor = dust.GetAlpha(newColor);
			spriteBatch.Draw(TextureAssets.Dust.Value, dust.position, (Rectangle?)dust.frame, newColor, dust.GetVisualRotation(), new Vector2(4f, 4f), visualScale, (SpriteEffects)0, 0f);
			if (dust.color.PackedValue != 0)
			{
				Color color = dust.GetColor(newColor);
				if (color.PackedValue != 0)
				{
					spriteBatch.Draw(TextureAssets.Dust.Value, dust.position, (Rectangle?)dust.frame, color, dust.GetVisualRotation(), new Vector2(4f, 4f), visualScale, (SpriteEffects)0, 0f);
				}
			}
			if (newColor == Color.Black)
			{
				dust.active = false;
			}
		}
	}
}
