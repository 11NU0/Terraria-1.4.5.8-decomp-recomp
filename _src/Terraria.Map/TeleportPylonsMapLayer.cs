using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Tile_Entities;
using Terraria.GameInput;
using Terraria.UI;

namespace Terraria.Map;

public class TeleportPylonsMapLayer : IMapLayer
{
	public const int BorderSize = 10;

	public static void DrawLine(SpriteBatch spriteBatch, Vector2 start, Vector2 end, Color col, float width)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		float num = Vector2.Distance(start, end);
		float num2 = (end - start).ToRotation();
		int num3 = Math.Min(5, (int)num);
		Rectangle val = TextureAssets.BlackTile.Value.Frame();
		for (int i = 0; i < num3; i++)
		{
			spriteBatch.Draw(TextureAssets.BlackTile.Value, Vector2.Lerp(start, end, (float)i / (float)num3), (Rectangle?)val, col, num2, new Vector2(0f, (float)val.Width * 0.5f), new Vector2(num / (float)num3 / 16f, width / 16f), (SpriteEffects)0, 0f);
		}
	}

	public void Draw(ref MapOverlayDrawContext context, ref string text)
	{
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_024a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0259: Unknown result type (might be due to invalid IL or missing references)
		//IL_025e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0263: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0203: Unknown result type (might be due to invalid IL or missing references)
		//IL_0208: Unknown result type (might be due to invalid IL or missing references)
		//IL_020d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0124: Unknown result type (might be due to invalid IL or missing references)
		//IL_0133: Unknown result type (might be due to invalid IL or missing references)
		//IL_0138: Unknown result type (might be due to invalid IL or missing references)
		//IL_0159: Unknown result type (might be due to invalid IL or missing references)
		//IL_015e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0162: Unknown result type (might be due to invalid IL or missing references)
		//IL_0167: Unknown result type (might be due to invalid IL or missing references)
		//IL_016e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0170: Unknown result type (might be due to invalid IL or missing references)
		//IL_0175: Unknown result type (might be due to invalid IL or missing references)
		//IL_0177: Unknown result type (might be due to invalid IL or missing references)
		//IL_017c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0190: Unknown result type (might be due to invalid IL or missing references)
		//IL_0192: Unknown result type (might be due to invalid IL or missing references)
		//IL_0197: Unknown result type (might be due to invalid IL or missing references)
		//IL_0199: Unknown result type (might be due to invalid IL or missing references)
		//IL_019e: Unknown result type (might be due to invalid IL or missing references)
		List<TeleportPylonInfo> pylons = Main.PylonSystem.Pylons;
		float num = 1f;
		float scaleIfSelected = num * 2f;
		float scaleIfOffscreen = num * 0.5f;
		Texture2D value = TextureAssets.Extra[182].Value;
		Texture2D value2 = TextureAssets.Extra[299].Value;
		Color color = Color.White;
		if (!TeleportPylonsSystem.IsPlayerNearAPylon(Main.LocalPlayer))
		{
			color = Color.Gray * 0.5f;
		}
		bool flag = false;
		int num2 = -1;
		if (Main.mapFullscreen && Main.MapPylonTile.X != -1 && Main.MapPylonTile.Y != -1)
		{
			Rectangle unclampedDrawRegion = context.GetUnclampedDrawRegion(value, Main.MapPylonTile.ToVector2() + new Vector2(1.5f, 2f), new SpriteFrame(11, 1, 0, 0)
			{
				PaddingY = 0
			}, num, Alignment.Center);
			Point center = unclampedDrawRegion.Center;
			for (int i = 0; i < pylons.Count; i++)
			{
				TeleportPylonInfo info = pylons[i];
				if (IsRevealed(info) && !(info.PositionInTiles == Main.MapPylonTile))
				{
					unclampedDrawRegion = context.GetUnclampedDrawRegion(value, info.PositionInTiles.ToVector2() + new Vector2(1.5f, 2f), new SpriteFrame(11, 1, 0, 0)
					{
						PaddingY = 0
					}, num, Alignment.Center);
					Point center2 = unclampedDrawRegion.Center;
					DrawLine(Main.spriteBatch, center.ToVector2(), center2.ToVector2(), Color.Black, 6f);
					DrawLine(Main.spriteBatch, center.ToVector2(), center2.ToVector2(), Color.White, 2f);
				}
			}
		}
		for (int j = 0; j < pylons.Count; j++)
		{
			TeleportPylonInfo info2 = pylons[j];
			if (!IsRevealed(info2))
			{
				continue;
			}
			bool onScreen = true;
			if (((!Main.mapFullscreen) ? context.Draw(value, info2.PositionInTiles.ToVector2() + new Vector2(1.5f, 2f), color, new SpriteFrame(11, 1, (byte)info2.TypeOfPylon, 0)
			{
				PaddingY = 0
			}, num, scaleIfSelected, Alignment.Center) : context.DrawClamped(value, value2, info2.PositionInTiles.ToVector2() + new Vector2(1.5f, 2f), color, new SpriteFrame(11, 1, (byte)info2.TypeOfPylon, 0)
			{
				PaddingY = 0
			}, num, scaleIfSelected, scaleIfOffscreen, Alignment.Center, 10, out onScreen)).IsMouseOver)
			{
				Main.cancelWormHole = true;
				string itemNameValue = Lang.GetItemNameValue(TETeleportationPylon.GetPylonItemTypeFromTileStyle((int)info2.TypeOfPylon));
				text = itemNameValue;
				if (Main.mouseLeft && Main.mouseLeftRelease)
				{
					flag = onScreen;
					num2 = j;
				}
			}
		}
		if (num2 != -1 && Main.mouseLeft && Main.mouseLeftRelease)
		{
			TeleportPylonInfo info3 = pylons[num2];
			if (flag)
			{
				Main.mouseLeftRelease = false;
				Main.mapFullscreen = false;
				PlayerInput.LockGamepadButtons("MouseLeft");
				Main.PylonSystem.RequestTeleportation(info3, Main.LocalPlayer);
				SoundEngine.PlaySound(11);
			}
			else
			{
				Main.mouseLeftRelease = false;
				PlayerInput.LockGamepadButtons("MouseLeft");
				Main.PanTargetMapFullscreen = true;
				Main.PanTargetMapFullscreenEnd.X = info3.PositionInTiles.X;
				Main.PanTargetMapFullscreenEnd.Y = info3.PositionInTiles.Y;
			}
		}
	}

	public static bool IsRevealed(TeleportPylonInfo info)
	{
		if (!Main.teamBasedSpawnsSeed)
		{
			return true;
		}
		return Main.Map.IsRevealed(info.PositionInTiles.X, info.PositionInTiles.Y);
	}
}
