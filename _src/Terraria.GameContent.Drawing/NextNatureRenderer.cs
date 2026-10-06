using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;

namespace Terraria.GameContent.Drawing;

public class NextNatureRenderer : INatureRenderer
{
	private struct Entry
	{
		public DrawData Data;

		public SideFlags Seams;

		public bool IsGlowMask;
	}

	private readonly List<Entry> _entries = new List<Entry>();

	public void DrawNature(Texture2D texture, Vector2 position, Rectangle sourceRectangle, Color color, float rotation, Vector2 origin, float scale, SpriteEffects effects, float layerDepth, SideFlags seams = SideFlags.None)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		seams |= GetOriginSides(sourceRectangle, origin);
		Entry item = new Entry
		{
			Data = new DrawData(texture, position, sourceRectangle, color, rotation, origin, scale, effects),
			IsGlowMask = false,
			Seams = seams
		};
		_entries.Add(item);
	}

	private static SideFlags GetOriginSides(Rectangle sourceRectangle, Vector2 origin)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		float num = origin.X / (float)sourceRectangle.Width;
		float num2 = 1f - num;
		float num3 = origin.Y / (float)sourceRectangle.Height;
		float num4 = 1f - num3;
		SideFlags sideFlags = SideFlags.None;
		if ((double)num < 0.25)
		{
			sideFlags |= SideFlags.Left;
		}
		if ((double)num2 < 0.25)
		{
			sideFlags |= SideFlags.Right;
		}
		if ((double)num3 < 0.25)
		{
			sideFlags |= SideFlags.Top;
		}
		if ((double)num4 < 0.25)
		{
			sideFlags |= SideFlags.Bottom;
		}
		return sideFlags;
	}

	public void DrawGlowmask(Texture2D texture, Vector2 position, Rectangle? sourceRectangle, Color color, float rotation, Vector2 origin, float scale, SpriteEffects effects, float layerDepth)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		Entry item = new Entry
		{
			Data = new DrawData(texture, position, sourceRectangle, color, rotation, origin, scale, effects),
			IsGlowMask = true
		};
		_entries.Add(item);
	}

	public void DrawAfterAllObjects(SpriteBatchBeginner beginner)
	{
		//IL_012b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0130: Unknown result type (might be due to invalid IL or missing references)
		//IL_0131: Unknown result type (might be due to invalid IL or missing references)
		//IL_013c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0199: Unknown result type (might be due to invalid IL or missing references)
		if (_entries.Count == 0)
		{
			return;
		}
		TimeLogger.StartTimestamp fromTimestamp = TimeLogger.Start();
		float num = 0f;
		if (Main.dayTime)
		{
			float fromValue = (float)Main.time;
			float num2 = 54000f;
			float val = Utils.Remap(fromValue, 1200f, 5400f, 0f, 1f) * Utils.Remap(fromValue, 1200f, 7200f, 1f, 0f) * 0.3f;
			float num3 = Utils.Remap(fromValue, num2 - 10800f, num2 - 4200f, 0f, 1f) * Utils.Remap(fromValue, num2 - 1800f, num2 - 600f, 1f, 0f) * 0.4f;
			num3 *= num3;
			num = Math.Max(val2: Utils.Remap(fromValue, 0f, 7200f, 0f, 1f) * Utils.Remap(fromValue, num2 - 7200f, num2, 1f, 0f) * 0f, val1: Math.Max(val, num3));
			if (Main.eclipse)
			{
				num = 0f;
			}
		}
		num *= 0.4f;
		Vector2 lastCelestialBodyPosition = Main.LastCelestialBodyPosition;
		float num4 = Utils.Remap(Math.Min(lastCelestialBodyPosition.X, 1f - lastCelestialBodyPosition.X), 0f, 1f / 96f, 0f, 1f);
		num *= num4;
		if (!Main.ShouldDrawSurfaceBackground() || !Main.HorizonHelper.SunVisibilityEnabled)
		{
			num = 0f;
		}
		if (num == 0f)
		{
			DrawWithoutShader(beginner, Main.spriteBatch);
		}
		else
		{
			DrawWithLitNatureShader(beginner, num, lastCelestialBodyPosition);
		}
		_entries.Clear();
		TimeLogger.Nature.AddTime(fromTimestamp);
	}

	private void DrawWithoutShader(SpriteBatchBeginner beginner, SpriteBatch spriteBatch)
	{
		beginner.Begin(spriteBatch);
		foreach (Entry entry in _entries)
		{
			entry.Data.Draw(spriteBatch);
		}
		spriteBatch.End();
	}

	private void DrawWithLitNatureShader(SpriteBatchBeginner beginner, float visibility, Vector2 sunPosition)
	{
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0102: Unknown result type (might be due to invalid IL or missing references)
		//IL_0114: Unknown result type (might be due to invalid IL or missing references)
		//IL_011e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0123: Unknown result type (might be due to invalid IL or missing references)
		//IL_0126: Unknown result type (might be due to invalid IL or missing references)
		//IL_0133: Unknown result type (might be due to invalid IL or missing references)
		//IL_013d: Unknown result type (might be due to invalid IL or missing references)
		//IL_017f: Unknown result type (might be due to invalid IL or missing references)
		SpriteDrawBuffer spriteBuffer = Main.spriteBuffer;
		foreach (Entry entry in _entries)
		{
			entry.Data.Draw(spriteBuffer);
		}
		MiscShaderData miscShaderData = GameShaders.Misc["LitNature"];
		Vector2 val = Vector2.Transform(Main.ReverseGravitySupport(sunPosition * Main.ScreenSize.ToVector2()), Matrix.Invert(Main.Transform));
		Vector4 specificData = new Vector4(val.X, val.Y, visibility, 0f);
		miscShaderData.UseImage1((Texture)(object)Main.HorizonHelper.SunVisibilityPixelTexture);
		miscShaderData.UseSpriteTransformMatrix(beginner.transformMatrix);
		HorizonHelper.GetCelestialBodyColors(out var sunColor, out var moonColor);
		Color newColor = (Main.dayTime ? sunColor : moonColor);
		Vector3 val2 = Main.rgbToHsl(newColor);
		newColor = Main.hslToRgb(val2.X, Utils.Clamp(val2.Y * 8f, 0f, 1f), val2.Z * 1f) * 0.5f;
		miscShaderData.UseColor(Color.Lerp(newColor, new Color(255, 200, 0), 0.8f));
		int num = 0;
		foreach (Entry entry2 in _entries)
		{
			specificData.W = (float)(entry2.IsGlowMask ? ((SideFlags)(-1)) : entry2.Seams);
			miscShaderData.UseShaderSpecificData(specificData);
			miscShaderData.Apply(entry2.Data);
			spriteBuffer.DrawSingle(num++);
		}
		spriteBuffer.Unbind();
	}
}
