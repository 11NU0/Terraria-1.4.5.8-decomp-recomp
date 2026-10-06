using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.GameContent;
using Terraria.GameContent.Events;
using Terraria.GameContent.Liquid;
using Terraria.Graphics;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.UI;

namespace Terraria.DataStructures;

public static class PlayerDrawLayers
{
	public static void DrawPlayer_extra_TorsoPlus(ref PlayerDrawSet drawinfo)
	{
		drawinfo.Position.Y += drawinfo.torsoOffset;
		drawinfo.ItemLocation.Y += drawinfo.torsoOffset;
	}

	public static void DrawPlayer_extra_TorsoMinus(ref PlayerDrawSet drawinfo)
	{
		drawinfo.Position.Y -= drawinfo.torsoOffset;
		drawinfo.ItemLocation.Y -= drawinfo.torsoOffset;
	}

	public static void DrawPlayer_extra_MountPlus(ref PlayerDrawSet drawinfo)
	{
		drawinfo.Position.Y += (int)drawinfo.mountOffSet / 2;
	}

	public static void DrawPlayer_extra_MountMinus(ref PlayerDrawSet drawinfo)
	{
		drawinfo.Position.Y -= (int)drawinfo.mountOffSet / 2;
	}

	public static void DrawCompositeArmorPiece(ref PlayerDrawSet drawinfo, CompositePlayerDrawContext context, DrawData data, int bodyIndex)
	{
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0118: Unknown result type (might be due to invalid IL or missing references)
		//IL_011d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0129: Unknown result type (might be due to invalid IL or missing references)
		//IL_012e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0142: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_0158: Unknown result type (might be due to invalid IL or missing references)
		//IL_015d: Unknown result type (might be due to invalid IL or missing references)
		//IL_01eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0268: Unknown result type (might be due to invalid IL or missing references)
		//IL_026d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0278: Unknown result type (might be due to invalid IL or missing references)
		//IL_027d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0283: Unknown result type (might be due to invalid IL or missing references)
		//IL_0288: Unknown result type (might be due to invalid IL or missing references)
		//IL_028d: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0318: Unknown result type (might be due to invalid IL or missing references)
		//IL_0320: Unknown result type (might be due to invalid IL or missing references)
		//IL_032b: Unknown result type (might be due to invalid IL or missing references)
		//IL_033c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0347: Unknown result type (might be due to invalid IL or missing references)
		//IL_0482: Unknown result type (might be due to invalid IL or missing references)
		//IL_0487: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_04bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_038f: Unknown result type (might be due to invalid IL or missing references)
		//IL_039a: Unknown result type (might be due to invalid IL or missing references)
		//IL_039f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0190: Unknown result type (might be due to invalid IL or missing references)
		//IL_0197: Unknown result type (might be due to invalid IL or missing references)
		//IL_0199: Unknown result type (might be due to invalid IL or missing references)
		//IL_019b: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0504: Unknown result type (might be due to invalid IL or missing references)
		//IL_0509: Unknown result type (might be due to invalid IL or missing references)
		//IL_0532: Unknown result type (might be due to invalid IL or missing references)
		//IL_0539: Unknown result type (might be due to invalid IL or missing references)
		//IL_053e: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0406: Unknown result type (might be due to invalid IL or missing references)
		//IL_0438: Unknown result type (might be due to invalid IL or missing references)
		//IL_043a: Unknown result type (might be due to invalid IL or missing references)
		drawinfo.DrawDataCache.Add(data);
		if (drawinfo.hideEntirePlayer || drawinfo.hideEntirePlayerExceptHelmetsAndFaceAccessories)
		{
			return;
		}
		switch (context)
		{
		case CompositePlayerDrawContext.BackShoulder:
		case CompositePlayerDrawContext.BackArm:
		case CompositePlayerDrawContext.FrontArm:
		case CompositePlayerDrawContext.FrontShoulder:
		{
			if (drawinfo.armGlowColor.PackedValue == 0)
			{
				break;
			}
			DrawData item2 = data;
			item2.color = drawinfo.armGlowColor;
			Rectangle value2 = item2.sourceRect.Value;
			value2.Y += 224;
			item2.sourceRect = value2;
			if (bodyIndex == 227)
			{
				Vector2 position2 = item2.position;
				for (int j = 0; j < 2; j++)
				{
					Vector2 val2 = new Vector2((float)Main.rand.Next(-10, 10) * 0.125f, (float)Main.rand.Next(-10, 10) * 0.125f);
					item2.position = position2 + val2;
					if (j == 0)
					{
						drawinfo.DrawDataCache.Add(item2);
					}
				}
			}
			drawinfo.DrawDataCache.Add(item2);
			break;
		}
		case CompositePlayerDrawContext.Torso:
		{
			if (drawinfo.bodyGlowColor.PackedValue == 0)
			{
				break;
			}
			DrawData item = data;
			item.color = drawinfo.bodyGlowColor;
			Rectangle value = item.sourceRect.Value;
			value.Y += 224;
			item.sourceRect = value;
			if (bodyIndex == 227)
			{
				Vector2 position = item.position;
				for (int i = 0; i < 2; i++)
				{
					Vector2 val = new Vector2((float)Main.rand.Next(-10, 10) * 0.125f, (float)Main.rand.Next(-10, 10) * 0.125f);
					item.position = position + val;
					if (i == 0)
					{
						drawinfo.DrawDataCache.Add(item);
					}
				}
			}
			drawinfo.DrawDataCache.Add(item);
			break;
		}
		}
		if (context == CompositePlayerDrawContext.FrontShoulder && drawinfo.drawPlayer.head == 269)
		{
			Vector2 pos = drawinfo.helmetOffset + new Vector2((float)(int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.headPosition + drawinfo.headVect;
			drawinfo.drawPlayer.ApplyHeadOffsetFromMount(ref pos);
			DrawData item3 = new DrawData(TextureAssets.Extra[214].Value, pos, drawinfo.drawPlayer.bodyFrame, drawinfo.colorArmorHead, drawinfo.drawPlayer.headRotation, drawinfo.headVect, 1f, drawinfo.playerEffect);
			item3.shader = drawinfo.cHead;
			drawinfo.DrawDataCache.Add(item3);
			item3 = new DrawData(TextureAssets.GlowMask[308].Value, pos, drawinfo.drawPlayer.bodyFrame, drawinfo.headGlowColor, drawinfo.drawPlayer.headRotation, drawinfo.headVect, 1f, drawinfo.playerEffect);
			item3.shader = drawinfo.cHead;
			drawinfo.DrawDataCache.Add(item3);
		}
		if (context == CompositePlayerDrawContext.FrontArm && bodyIndex == 205)
		{
			Color immuneAlphaPure = drawinfo.drawPlayer.GetImmuneAlphaPure(new Color(100, 100, 100, 0), drawinfo.shadow);
			ulong seed = (ulong)(drawinfo.drawPlayer.miscCounter / 4);
			int num = 4;
			for (int k = 0; k < num; k++)
			{
				float num2 = (float)Utils.RandomInt(ref seed, -10, 11) * 0.2f;
				float num3 = (float)Utils.RandomInt(ref seed, -10, 1) * 0.15f;
				DrawData item4 = data;
				Rectangle value3 = item4.sourceRect.Value;
				value3.Y += 224;
				item4.sourceRect = value3;
				item4.position.X += num2;
				item4.position.Y += num3;
				item4.color = immuneAlphaPure;
				drawinfo.DrawDataCache.Add(item4);
			}
		}
		switch (bodyIndex)
		{
		case 251:
		{
			DrawData item6 = data;
			item6.texture = TextureAssets.GlowMask[364].Value;
			item6.color = GetChickenBonesGlowColor(ref drawinfo, scaleByShadow: true);
			float num5 = drawinfo.stealth * drawinfo.stealth;
			num5 *= 1f - drawinfo.shadow;
			item6.color = Color.Multiply(item6.color, num5);
			drawinfo.DrawDataCache.Add(item6);
			break;
		}
		case 259:
		{
			DrawData item5 = data;
			item5.texture = TextureAssets.GlowMask[376].Value;
			item5.color = drawinfo.drawPlayer.GetImmuneAlphaPure(Color.White, drawinfo.shadow);
			float num4 = drawinfo.stealth * drawinfo.stealth;
			num4 *= 1f - drawinfo.shadow;
			item5.color = Color.Multiply(item5.color, num4);
			drawinfo.DrawDataCache.Add(item5);
			break;
		}
		}
	}

	public static Color GetChickenBonesGlowColor(ref PlayerDrawSet drawinfo, bool scaleByShadow, bool wings = false)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		if (drawinfo.hideEntirePlayer || drawinfo.hideEntirePlayerExceptHelmetsAndFaceAccessories)
		{
			return Color.Transparent;
		}
		Color val = new Color(255, 255, 255, 0);
		if (!wings)
		{
			float num = Utils.Remap(Utils.WrappedLerp(0f, 1f, (float)(drawinfo.drawPlayer.miscCounter % 100) / 100f), 0f, 1f, 0.8f, 1f);
			val *= num;
		}
		if (!scaleByShadow)
		{
			return val;
		}
		return drawinfo.drawPlayer.GetImmuneAlphaPure(val, drawinfo.shadow);
	}

	public static Color GetLunaGlowColor(ref PlayerDrawSet drawinfo, bool scaleByShadow)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		if (drawinfo.hideEntirePlayer || drawinfo.hideEntirePlayerExceptHelmetsAndFaceAccessories)
		{
			return Color.Transparent;
		}
		Color val = new Color(255, 255, 255, 100);
		float num = Utils.Remap(Utils.WrappedLerp(0f, 1f, (float)(drawinfo.drawPlayer.miscCounter % 100) / 100f), 0f, 1f, 0.85f, 1f);
		val *= num;
		if (!scaleByShadow)
		{
			return val;
		}
		return drawinfo.drawPlayer.GetImmuneAlphaPure(val, drawinfo.shadow);
	}

	public static void DrawPlayer_01_BackHair(ref PlayerDrawSet drawinfo)
	{
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_010e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0119: Unknown result type (might be due to invalid IL or missing references)
		//IL_0162: Unknown result type (might be due to invalid IL or missing references)
		//IL_0164: Unknown result type (might be due to invalid IL or missing references)
		//IL_016f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0180: Unknown result type (might be due to invalid IL or missing references)
		//IL_018b: Unknown result type (might be due to invalid IL or missing references)
		if (!drawinfo.hideHair && drawinfo.backHairDraw)
		{
			Vector2 position = new Vector2((float)(int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.headPosition + drawinfo.headVect + drawinfo.hairOffset;
			if (drawinfo.drawPlayer.head == -1 || drawinfo.fullHair || drawinfo.drawsBackHairWithoutHeadgear)
			{
				DrawData item = new DrawData(TextureAssets.PlayerHair[drawinfo.drawPlayer.hair].Value, position, drawinfo.hairBackFrame, drawinfo.colorHair, drawinfo.drawPlayer.headRotation, drawinfo.headVect, 1f, drawinfo.playerEffect);
				item.shader = drawinfo.hairDyePacked;
				drawinfo.DrawDataCache.Add(item);
			}
			else if (drawinfo.hatHair)
			{
				DrawData item = new DrawData(TextureAssets.PlayerHairAlt[drawinfo.drawPlayer.hair].Value, position, drawinfo.hairBackFrame, drawinfo.colorHair, drawinfo.drawPlayer.headRotation, drawinfo.headVect, 1f, drawinfo.playerEffect);
				item.shader = drawinfo.hairDyePacked;
				drawinfo.DrawDataCache.Add(item);
			}
		}
	}

	public static void DrawPlayer_02_MountBehindPlayer(ref PlayerDrawSet drawinfo)
	{
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		if (drawinfo.drawPlayer.mount.Active)
		{
			DrawMeowcartTrail(ref drawinfo);
			DrawTiedBalloons(ref drawinfo);
			drawinfo.drawPlayer.mount.Draw(drawinfo.DrawDataCache, 0, drawinfo.drawPlayer, drawinfo.Position, drawinfo.colorMount, drawinfo.playerEffect, drawinfo.shadow);
			drawinfo.drawPlayer.mount.Draw(drawinfo.DrawDataCache, 1, drawinfo.drawPlayer, drawinfo.Position, drawinfo.colorMount, drawinfo.playerEffect, drawinfo.shadow);
		}
	}

	public static void DrawPlayer_03_Carpet(ref PlayerDrawSet drawinfo)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0108: Unknown result type (might be due to invalid IL or missing references)
		//IL_0113: Unknown result type (might be due to invalid IL or missing references)
		if (drawinfo.drawPlayer.carpetFrame >= 0)
		{
			Color colorArmorLegs = drawinfo.colorArmorLegs;
			float num = 0f;
			if (drawinfo.drawPlayer.gravDir == -1f)
			{
				num = 10f;
			}
			DrawData item = new DrawData(TextureAssets.FlyingCarpet.Value, new Vector2((float)(int)(drawinfo.Position.X - Main.screenPosition.X + (float)(drawinfo.drawPlayer.width / 2)), (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)(drawinfo.drawPlayer.height / 2) + 28f * drawinfo.drawPlayer.gravDir + num)), new Rectangle(0, TextureAssets.FlyingCarpet.Height() / 6 * drawinfo.drawPlayer.carpetFrame, TextureAssets.FlyingCarpet.Width(), TextureAssets.FlyingCarpet.Height() / 6), colorArmorLegs, drawinfo.drawPlayer.bodyRotation, new Vector2((float)(TextureAssets.FlyingCarpet.Width() / 2), (float)(TextureAssets.FlyingCarpet.Height() / 8)), 1f, drawinfo.playerEffect);
			item.shader = drawinfo.cCarpet;
			drawinfo.DrawDataCache.Add(item);
		}
	}

	public static void DrawPlayer_03_PortableStool(ref PlayerDrawSet drawinfo)
	{
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		if (drawinfo.drawPlayer.portableStoolInfo.IsInUse && drawinfo.shadow == 0f)
		{
			Texture2D value = TextureAssets.Extra[102].Value;
			Vector2 position = new Vector2((float)(int)(drawinfo.Position.X - Main.screenPosition.X + (float)(drawinfo.drawPlayer.width / 2)), (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height + 28f));
			Rectangle val = value.Frame();
			Vector2 origin = val.Size() * new Vector2(0.5f, 1f);
			DrawData item = new DrawData(value, position, val, drawinfo.colorArmorLegs, drawinfo.drawPlayer.bodyRotation, origin, 1f, drawinfo.playerEffect);
			item.shader = drawinfo.cPortableStool;
			drawinfo.DrawDataCache.Add(item);
		}
	}

	public static void DrawPlayer_04_ElectrifiedDebuffBack(ref PlayerDrawSet drawinfo)
	{
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0108: Unknown result type (might be due to invalid IL or missing references)
		//IL_010d: Unknown result type (might be due to invalid IL or missing references)
		//IL_012b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0136: Unknown result type (might be due to invalid IL or missing references)
		//IL_0159: Unknown result type (might be due to invalid IL or missing references)
		//IL_0164: Unknown result type (might be due to invalid IL or missing references)
		if (!drawinfo.drawPlayer.electrified || drawinfo.shadow != 0f)
		{
			return;
		}
		Texture2D value = TextureAssets.GlowMask[25].Value;
		int num = drawinfo.drawPlayer.miscCounter / 5;
		for (int i = 0; i < 2; i++)
		{
			num %= 7;
			if (num <= 1 || num >= 5)
			{
				DrawData item = new DrawData(value, new Vector2((float)(int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.bodyPosition + new Vector2((float)(drawinfo.drawPlayer.bodyFrame.Width / 2), (float)(drawinfo.drawPlayer.bodyFrame.Height / 2)), new Rectangle(0, num * value.Height / 7, value.Width, value.Height / 7), drawinfo.colorElectricity, drawinfo.drawPlayer.bodyRotation, new Vector2((float)(value.Width / 2), (float)(value.Height / 14)), 1f, drawinfo.playerEffect);
				drawinfo.DrawDataCache.Add(item);
			}
			num += 3;
		}
	}

	public static void DrawPlayer_05_ForbiddenSetRing(ref PlayerDrawSet drawinfo)
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_019d: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		//IL_0108: Unknown result type (might be due to invalid IL or missing references)
		//IL_010a: Unknown result type (might be due to invalid IL or missing references)
		//IL_011a: Unknown result type (might be due to invalid IL or missing references)
		//IL_011f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0124: Unknown result type (might be due to invalid IL or missing references)
		//IL_020a: Unknown result type (might be due to invalid IL or missing references)
		//IL_023c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0241: Unknown result type (might be due to invalid IL or missing references)
		//IL_0246: Unknown result type (might be due to invalid IL or missing references)
		//IL_024b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0257: Unknown result type (might be due to invalid IL or missing references)
		//IL_0264: Unknown result type (might be due to invalid IL or missing references)
		//IL_026e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0279: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02be: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f5: Unknown result type (might be due to invalid IL or missing references)
		if (drawinfo.drawPlayer.setForbidden && drawinfo.shadow == 0f)
		{
			Color val = Color.Lerp(drawinfo.colorArmorBody, Color.White, 0.7f);
			Texture2D value = TextureAssets.Extra[74].Value;
			Texture2D value2 = TextureAssets.GlowMask[217].Value;
			bool flag = !drawinfo.drawPlayer.setForbiddenCooldownLocked;
			int num = 0;
			num = (int)(((float)drawinfo.drawPlayer.miscCounter / 300f * ((float)Math.PI * 2f)).ToRotationVector2().Y * 6f);
			float num2 = ((float)drawinfo.drawPlayer.miscCounter / 75f * ((float)Math.PI * 2f)).ToRotationVector2().X * 4f;
			Color color = new Color(80, 70, 40, 0) * (num2 / 8f + 0.5f) * 0.8f;
			if (!flag)
			{
				num = 0;
				num2 = 2f;
				color = new Color(80, 70, 40, 0) * 0.3f;
				val = val.MultiplyRGB(new Color(0.5f, 0.5f, 1f));
			}
			Vector2 val2 = new Vector2((float)(int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.bodyPosition + new Vector2((float)(drawinfo.drawPlayer.bodyFrame.Width / 2), (float)(drawinfo.drawPlayer.bodyFrame.Height / 2));
			int num3 = 10;
			int num4 = 20;
			if (drawinfo.drawPlayer.head == 238)
			{
				num3 += 4;
				num4 += 4;
			}
			val2 += new Vector2((float)(-drawinfo.drawPlayer.direction * num3), (float)(-num4) * drawinfo.drawPlayer.gravDir + (float)num * drawinfo.drawPlayer.gravDir);
			DrawData item = new DrawData(value, val2, null, val, drawinfo.drawPlayer.bodyRotation, value.Size() / 2f, 1f, drawinfo.playerEffect);
			item.shader = drawinfo.cBody;
			drawinfo.DrawDataCache.Add(item);
			for (float num5 = 0f; num5 < 4f; num5++)
			{
				item = new DrawData(value2, val2 + (num5 * ((float)Math.PI / 2f)).ToRotationVector2() * num2, null, color, drawinfo.drawPlayer.bodyRotation, value.Size() / 2f, 1f, drawinfo.playerEffect);
				drawinfo.DrawDataCache.Add(item);
			}
		}
	}

	public static void DrawPlayer_01_3_BackHead(ref PlayerDrawSet drawinfo)
	{
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0105: Unknown result type (might be due to invalid IL or missing references)
		//IL_0110: Unknown result type (might be due to invalid IL or missing references)
		//IL_0121: Unknown result type (might be due to invalid IL or missing references)
		//IL_012c: Unknown result type (might be due to invalid IL or missing references)
		if (drawinfo.drawPlayer.head >= 0 && drawinfo.drawPlayer.head < ArmorIDs.Head.Count)
		{
			int num = ArmorIDs.Head.Sets.FrontToBackID[drawinfo.drawPlayer.head];
			if (num >= 0)
			{
				Vector2 pos = drawinfo.helmetOffset;
				drawinfo.drawPlayer.ApplyHeadOffsetFromMount(ref pos);
				DrawData item = new DrawData(TextureAssets.ArmorHead[num].Value, pos + new Vector2((float)(int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.headPosition + drawinfo.headVect, drawinfo.drawPlayer.bodyFrame, drawinfo.colorArmorHead, drawinfo.drawPlayer.headRotation, drawinfo.headVect, 1f, drawinfo.playerEffect);
				item.shader = drawinfo.cHead;
				drawinfo.DrawDataCache.Add(item);
			}
		}
		if (drawinfo.drawPlayer.face > 0 && drawinfo.drawPlayer.face == 23)
		{
			DrawPlayer_ChippysHeadband(ref drawinfo);
		}
	}

	public static void DrawPlayer_01_2_JimsCloak(ref PlayerDrawSet drawinfo)
	{
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0100: Unknown result type (might be due to invalid IL or missing references)
		//IL_0111: Unknown result type (might be due to invalid IL or missing references)
		//IL_011c: Unknown result type (might be due to invalid IL or missing references)
		if (drawinfo.drawPlayer.legs == 60 && !drawinfo.isSitting && !drawinfo.drawPlayer.invis && (!ShouldOverrideLegs_CheckShoes(ref drawinfo) || drawinfo.drawPlayer.wearsRobe))
		{
			DrawData item = new DrawData(TextureAssets.Extra[153].Value, new Vector2((float)(int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.legFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.legFrame.Height + 4f)) + drawinfo.drawPlayer.legPosition + drawinfo.legVect, drawinfo.drawPlayer.legFrame, drawinfo.colorArmorLegs, drawinfo.drawPlayer.legRotation, drawinfo.legVect, 1f, drawinfo.playerEffect);
			item.shader = drawinfo.cLegs;
			drawinfo.DrawDataCache.Add(item);
		}
	}

	public static void DrawPlayer_05_2_SafemanSun(ref PlayerDrawSet drawinfo)
	{
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Unknown result type (might be due to invalid IL or missing references)
		//IL_0158: Unknown result type (might be due to invalid IL or missing references)
		//IL_0163: Unknown result type (might be due to invalid IL or missing references)
		//IL_0168: Unknown result type (might be due to invalid IL or missing references)
		//IL_0193: Unknown result type (might be due to invalid IL or missing references)
		//IL_0198: Unknown result type (might be due to invalid IL or missing references)
		//IL_019d: Unknown result type (might be due to invalid IL or missing references)
		//IL_01eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_021d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0222: Unknown result type (might be due to invalid IL or missing references)
		//IL_0227: Unknown result type (might be due to invalid IL or missing references)
		//IL_022c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0238: Unknown result type (might be due to invalid IL or missing references)
		//IL_0245: Unknown result type (might be due to invalid IL or missing references)
		//IL_024f: Unknown result type (might be due to invalid IL or missing references)
		//IL_025a: Unknown result type (might be due to invalid IL or missing references)
		//IL_028e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0298: Unknown result type (might be due to invalid IL or missing references)
		//IL_029f: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d6: Unknown result type (might be due to invalid IL or missing references)
		if (drawinfo.drawPlayer.head != 238 || drawinfo.shadow != 0f)
		{
			return;
		}
		Color color = Color.Lerp(drawinfo.colorArmorBody, Color.White, 0.7f);
		Texture2D value = TextureAssets.Extra[152].Value;
		Texture2D value2 = TextureAssets.Extra[152].Value;
		int num = 0;
		num = (int)(((float)drawinfo.drawPlayer.miscCounter / 300f * ((float)Math.PI * 2f)).ToRotationVector2().Y * 6f);
		float num2 = ((float)drawinfo.drawPlayer.miscCounter / 75f * ((float)Math.PI * 2f)).ToRotationVector2().X * 4f;
		Color color2 = new Color(80, 70, 40, 0) * (num2 / 8f + 0.5f) * 0.8f;
		Vector2 val = new Vector2((float)(int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.bodyPosition + new Vector2((float)(drawinfo.drawPlayer.bodyFrame.Width / 2), (float)(drawinfo.drawPlayer.bodyFrame.Height / 2));
		int num3 = 8;
		int num4 = 20;
		num3 += 4;
		num4 += 4;
		if (drawinfo.drawPlayer.mount.Active)
		{
			int type = drawinfo.drawPlayer.mount.Type;
			if ((uint)(type - 55) <= 1u || type == 61)
			{
				num4 -= 22;
			}
		}
		val += new Vector2((float)(-drawinfo.drawPlayer.direction * num3), (float)(-num4) * drawinfo.drawPlayer.gravDir + (float)num * drawinfo.drawPlayer.gravDir);
		DrawData item = new DrawData(value, val, null, color, drawinfo.drawPlayer.bodyRotation, value.Size() / 2f, 1f, drawinfo.playerEffect);
		item.shader = drawinfo.cHead;
		drawinfo.DrawDataCache.Add(item);
		for (float num5 = 0f; num5 < 4f; num5++)
		{
			item = new DrawData(value2, val + (num5 * ((float)Math.PI / 2f)).ToRotationVector2() * num2, null, color2, drawinfo.drawPlayer.bodyRotation, value.Size() / 2f, 1f, drawinfo.playerEffect);
			item.shader = drawinfo.cHead;
			drawinfo.DrawDataCache.Add(item);
		}
	}

	public static void DrawPlayer_06_WebbedDebuffBack(ref PlayerDrawSet drawinfo)
	{
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_010f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0114: Unknown result type (might be due to invalid IL or missing references)
		//IL_0122: Unknown result type (might be due to invalid IL or missing references)
		//IL_012f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0139: Unknown result type (might be due to invalid IL or missing references)
		//IL_0144: Unknown result type (might be due to invalid IL or missing references)
		if (drawinfo.drawPlayer.webbed && drawinfo.shadow == 0f && drawinfo.drawPlayer.velocity.Y != 0f)
		{
			Color color = drawinfo.colorArmorBody * 0.75f;
			Texture2D value = TextureAssets.Extra[32].Value;
			DrawData item = new DrawData(value, new Vector2((float)(int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.bodyPosition + new Vector2((float)(drawinfo.drawPlayer.bodyFrame.Width / 2), (float)(drawinfo.drawPlayer.bodyFrame.Height / 2)), null, color, drawinfo.drawPlayer.bodyRotation, value.Size() / 2f, 1f, drawinfo.playerEffect);
			drawinfo.DrawDataCache.Add(item);
		}
	}

	public static void DrawPlayer_07_LeinforsHairShampoo(ref PlayerDrawSet drawinfo)
	{
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_010d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0113: Unknown result type (might be due to invalid IL or missing references)
		//IL_0138: Unknown result type (might be due to invalid IL or missing references)
		//IL_0142: Unknown result type (might be due to invalid IL or missing references)
		//IL_0147: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01be: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0201: Unknown result type (might be due to invalid IL or missing references)
		//IL_0206: Unknown result type (might be due to invalid IL or missing references)
		//IL_0207: Unknown result type (might be due to invalid IL or missing references)
		//IL_0208: Unknown result type (might be due to invalid IL or missing references)
		//IL_020d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0213: Unknown result type (might be due to invalid IL or missing references)
		//IL_022f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0235: Unknown result type (might be due to invalid IL or missing references)
		//IL_025d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0267: Unknown result type (might be due to invalid IL or missing references)
		//IL_026c: Unknown result type (might be due to invalid IL or missing references)
		//IL_02eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0300: Unknown result type (might be due to invalid IL or missing references)
		//IL_0305: Unknown result type (might be due to invalid IL or missing references)
		//IL_031e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0323: Unknown result type (might be due to invalid IL or missing references)
		//IL_0332: Unknown result type (might be due to invalid IL or missing references)
		//IL_0337: Unknown result type (might be due to invalid IL or missing references)
		//IL_033c: Unknown result type (might be due to invalid IL or missing references)
		//IL_033e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0340: Unknown result type (might be due to invalid IL or missing references)
		//IL_0345: Unknown result type (might be due to invalid IL or missing references)
		//IL_034c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0369: Unknown result type (might be due to invalid IL or missing references)
		//IL_036f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0397: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a6: Unknown result type (might be due to invalid IL or missing references)
		if (drawinfo.drawPlayer.leinforsHair && (drawinfo.fullHair || drawinfo.hatHair || drawinfo.drawsBackHairWithoutHeadgear || drawinfo.drawPlayer.head == -1 || drawinfo.drawPlayer.head == 0) && drawinfo.drawPlayer.hair != 12 && drawinfo.shadow == 0f && Main.rgbToHsl(drawinfo.colorHead).Z > 0.2f)
		{
			if (Main.rand.Next(20) == 0 && !drawinfo.hatHair)
			{
				Rectangle val = Utils.CenteredRectangle(drawinfo.Position + drawinfo.drawPlayer.Size / 2f + new Vector2(0f, drawinfo.drawPlayer.gravDir * -20f), new Vector2(20f, 14f));
				int num = Dust.NewDust(val.TopLeft(), val.Width, val.Height, 204, 0f, 0f, 150, default, 0.3f);
				Main.dust[num].fadeIn = 1f;
				Dust obj = Main.dust[num];
				obj.velocity *= 0.1f;
				Main.dust[num].noLight = true;
				Main.dust[num].shader = GameShaders.Armor.GetSecondaryShader(drawinfo.drawPlayer.cLeinShampoo, drawinfo.drawPlayer);
				drawinfo.DustCache.Add(num);
			}
			if (Main.rand.Next(40) == 0 && drawinfo.hatHair)
			{
				Rectangle val2 = Utils.CenteredRectangle(drawinfo.Position + drawinfo.drawPlayer.Size / 2f + new Vector2((float)(drawinfo.drawPlayer.direction * -10), drawinfo.drawPlayer.gravDir * -10f), new Vector2(5f, 5f));
				int num2 = Dust.NewDust(val2.TopLeft(), val2.Width, val2.Height, 204, 0f, 0f, 150, default, 0.3f);
				Main.dust[num2].fadeIn = 1f;
				Dust obj2 = Main.dust[num2];
				obj2.velocity *= 0.1f;
				Main.dust[num2].noLight = true;
				Main.dust[num2].shader = GameShaders.Armor.GetSecondaryShader(drawinfo.drawPlayer.cLeinShampoo, drawinfo.drawPlayer);
				drawinfo.DustCache.Add(num2);
			}
			if (drawinfo.drawPlayer.velocity.X != 0f && drawinfo.backHairDraw && Main.rand.Next(15) == 0)
			{
				Rectangle val3 = Utils.CenteredRectangle(drawinfo.Position + drawinfo.drawPlayer.Size / 2f + new Vector2((float)(drawinfo.drawPlayer.direction * -14), 0f), new Vector2(4f, 30f));
				int num3 = Dust.NewDust(val3.TopLeft(), val3.Width, val3.Height, 204, 0f, 0f, 150, default, 0.3f);
				Main.dust[num3].fadeIn = 1f;
				Dust obj3 = Main.dust[num3];
				obj3.velocity *= 0.1f;
				Main.dust[num3].noLight = true;
				Main.dust[num3].shader = GameShaders.Armor.GetSecondaryShader(drawinfo.drawPlayer.cLeinShampoo, drawinfo.drawPlayer);
				drawinfo.DustCache.Add(num3);
			}
		}
	}

	public static bool DrawPlayer_08_PlayerVisuallyHasFullArmorSet(PlayerDrawSet drawinfo, int head, int body, int legs)
	{
		if (drawinfo.drawPlayer.head == head && drawinfo.drawPlayer.body == body)
		{
			return drawinfo.drawPlayer.legs == legs;
		}
		return false;
	}

	public static void DrawPlayer_08_Backpacks(ref PlayerDrawSet drawinfo)
	{
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0100: Unknown result type (might be due to invalid IL or missing references)
		//IL_0102: Unknown result type (might be due to invalid IL or missing references)
		//IL_010f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0114: Unknown result type (might be due to invalid IL or missing references)
		//IL_0119: Unknown result type (might be due to invalid IL or missing references)
		//IL_011a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0120: Unknown result type (might be due to invalid IL or missing references)
		//IL_012e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0139: Unknown result type (might be due to invalid IL or missing references)
		//IL_0190: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0209: Unknown result type (might be due to invalid IL or missing references)
		//IL_020e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0213: Unknown result type (might be due to invalid IL or missing references)
		//IL_0215: Unknown result type (might be due to invalid IL or missing references)
		//IL_0217: Unknown result type (might be due to invalid IL or missing references)
		//IL_021c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0247: Unknown result type (might be due to invalid IL or missing references)
		//IL_024c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0268: Unknown result type (might be due to invalid IL or missing references)
		//IL_0273: Unknown result type (might be due to invalid IL or missing references)
		//IL_0278: Unknown result type (might be due to invalid IL or missing references)
		//IL_027a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0287: Unknown result type (might be due to invalid IL or missing references)
		//IL_028c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0292: Unknown result type (might be due to invalid IL or missing references)
		//IL_0294: Unknown result type (might be due to invalid IL or missing references)
		//IL_029b: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0316: Unknown result type (might be due to invalid IL or missing references)
		//IL_031c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0321: Unknown result type (might be due to invalid IL or missing references)
		//IL_0326: Unknown result type (might be due to invalid IL or missing references)
		//IL_0331: Unknown result type (might be due to invalid IL or missing references)
		//IL_0336: Unknown result type (might be due to invalid IL or missing references)
		//IL_0368: Unknown result type (might be due to invalid IL or missing references)
		//IL_036d: Unknown result type (might be due to invalid IL or missing references)
		//IL_037c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0381: Unknown result type (might be due to invalid IL or missing references)
		//IL_0386: Unknown result type (might be due to invalid IL or missing references)
		//IL_0388: Unknown result type (might be due to invalid IL or missing references)
		//IL_038d: Unknown result type (might be due to invalid IL or missing references)
		//IL_038f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0391: Unknown result type (might be due to invalid IL or missing references)
		//IL_0396: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_03af: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_03cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0466: Unknown result type (might be due to invalid IL or missing references)
		//IL_046c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0471: Unknown result type (might be due to invalid IL or missing references)
		//IL_0476: Unknown result type (might be due to invalid IL or missing references)
		//IL_0481: Unknown result type (might be due to invalid IL or missing references)
		//IL_0486: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_04bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_04cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_04dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_04df: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0500: Unknown result type (might be due to invalid IL or missing references)
		//IL_0508: Unknown result type (might be due to invalid IL or missing references)
		//IL_0513: Unknown result type (might be due to invalid IL or missing references)
		//IL_0524: Unknown result type (might be due to invalid IL or missing references)
		//IL_052f: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_06b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_06b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_06bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_06c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_06cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0704: Unknown result type (might be due to invalid IL or missing references)
		//IL_0713: Unknown result type (might be due to invalid IL or missing references)
		//IL_0718: Unknown result type (might be due to invalid IL or missing references)
		//IL_071d: Unknown result type (might be due to invalid IL or missing references)
		//IL_071f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0724: Unknown result type (might be due to invalid IL or missing references)
		//IL_0726: Unknown result type (might be due to invalid IL or missing references)
		//IL_0728: Unknown result type (might be due to invalid IL or missing references)
		//IL_072d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0730: Unknown result type (might be due to invalid IL or missing references)
		//IL_0735: Unknown result type (might be due to invalid IL or missing references)
		//IL_073a: Unknown result type (might be due to invalid IL or missing references)
		//IL_076c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0771: Unknown result type (might be due to invalid IL or missing references)
		//IL_079f: Unknown result type (might be due to invalid IL or missing references)
		//IL_07a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_07a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_07b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_07b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_07b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_07b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_07d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0802: Unknown result type (might be due to invalid IL or missing references)
		//IL_080d: Unknown result type (might be due to invalid IL or missing references)
		//IL_083c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0847: Unknown result type (might be due to invalid IL or missing references)
		//IL_0885: Unknown result type (might be due to invalid IL or missing references)
		//IL_088d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0898: Unknown result type (might be due to invalid IL or missing references)
		//IL_08a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_08b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_08e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0906: Unknown result type (might be due to invalid IL or missing references)
		//IL_0911: Unknown result type (might be due to invalid IL or missing references)
		//IL_0941: Unknown result type (might be due to invalid IL or missing references)
		//IL_094c: Unknown result type (might be due to invalid IL or missing references)
		if (DrawPlayer_08_PlayerVisuallyHasFullArmorSet(drawinfo, 266, 235, 218))
		{
			Vector2 vec = new Vector2(-2f + -2f * drawinfo.drawPlayer.Directions.X, 0f) + drawinfo.Position - Main.screenPosition + drawinfo.drawPlayer.bodyPosition + new Vector2((float)(drawinfo.drawPlayer.width / 2), (float)(drawinfo.drawPlayer.height - drawinfo.drawPlayer.bodyFrame.Height / 2));
			vec = vec.Floor();
			Texture2D value = TextureAssets.Extra[212].Value;
			Rectangle value2 = value.Frame(1, 5, 0, drawinfo.drawPlayer.miscCounter % 25 / 5);
			Color immuneAlphaPure = drawinfo.drawPlayer.GetImmuneAlphaPure(new Color(250, 250, 250, 200), drawinfo.shadow);
			immuneAlphaPure *= drawinfo.drawPlayer.stealth;
			DrawData item = new DrawData(value, vec, value2, immuneAlphaPure, drawinfo.drawPlayer.bodyRotation, drawinfo.bodyVect, 1f, drawinfo.playerEffect);
			item.shader = drawinfo.cBody;
			drawinfo.DrawDataCache.Add(item);
		}
		if (DrawPlayer_08_PlayerVisuallyHasFullArmorSet(drawinfo, 268, 237, 222))
		{
			Vector2 vec2 = new Vector2(-9f + 1f * drawinfo.drawPlayer.Directions.X, -4f * drawinfo.drawPlayer.Directions.Y) + drawinfo.Position - Main.screenPosition + drawinfo.drawPlayer.bodyPosition + new Vector2((float)(drawinfo.drawPlayer.width / 2), (float)(drawinfo.drawPlayer.height - drawinfo.drawPlayer.bodyFrame.Height / 2));
			vec2 = vec2.Floor();
			Texture2D value3 = TextureAssets.Extra[213].Value;
			Rectangle value4 = value3.Frame(1, 5, 0, drawinfo.drawPlayer.miscCounter % 25 / 5);
			Color immuneAlphaPure2 = drawinfo.drawPlayer.GetImmuneAlphaPure(new Color(250, 250, 250, 200), drawinfo.shadow);
			immuneAlphaPure2 *= drawinfo.drawPlayer.stealth;
			DrawData item = new DrawData(value3, vec2, value4, immuneAlphaPure2, drawinfo.drawPlayer.bodyRotation, drawinfo.bodyVect, 1f, drawinfo.playerEffect);
			item.shader = drawinfo.cBody;
			drawinfo.DrawDataCache.Add(item);
		}
		if (drawinfo.heldItem.type == 4818 && drawinfo.drawPlayer.ownedProjectileCounts[902] == 0)
		{
			int num = 8;
			Vector2 val = new Vector2(0f, 8f);
			Vector2 vec3 = drawinfo.Position - Main.screenPosition + drawinfo.drawPlayer.bodyPosition + new Vector2((float)(drawinfo.drawPlayer.width / 2), (float)(drawinfo.drawPlayer.height - drawinfo.drawPlayer.bodyFrame.Height / 2)) + new Vector2(0f, -4f) + val;
			vec3 = vec3.Floor();
			DrawData item = new DrawData(TextureAssets.BackPack[num].Value, vec3, drawinfo.drawPlayer.bodyFrame, drawinfo.colorArmorBody, drawinfo.drawPlayer.bodyRotation, drawinfo.bodyVect, 1f, drawinfo.playerEffect);
			drawinfo.DrawDataCache.Add(item);
		}
		if (drawinfo.drawPlayer.backpack > 0 && drawinfo.drawPlayer.backpack < ArmorIDs.Back.Count && (!drawinfo.drawPlayer.mount.Active || (drawinfo.drawPlayer.mount.Type >= 0 && MountID.Sets.DoesNotOverrideBackpackDraw[drawinfo.drawPlayer.mount.Type])))
		{
			Vector2 val2 = new Vector2(0f, 8f);
			Vector2 vec4 = drawinfo.Position - Main.screenPosition + drawinfo.drawPlayer.bodyPosition + new Vector2((float)(drawinfo.drawPlayer.width / 2), (float)(drawinfo.drawPlayer.height - drawinfo.drawPlayer.bodyFrame.Height / 2)) + new Vector2(0f, -4f) + val2;
			vec4 = vec4.Floor();
			DrawData item = new DrawData(TextureAssets.AccBack[drawinfo.drawPlayer.backpack].Value, vec4, drawinfo.drawPlayer.bodyFrame, drawinfo.colorArmorBody, drawinfo.drawPlayer.bodyRotation, drawinfo.bodyVect, 1f, drawinfo.playerEffect);
			item.shader = drawinfo.cBackpack;
			drawinfo.DrawDataCache.Add(item);
		}
		else
		{
			if (drawinfo.heldItem.type != 1178 && drawinfo.heldItem.type != 779 && drawinfo.heldItem.type != 5134 && drawinfo.heldItem.type != 1295 && drawinfo.heldItem.type != 1910 && !drawinfo.drawPlayer.turtleArmor && drawinfo.drawPlayer.body != 106 && drawinfo.drawPlayer.body != 170)
			{
				return;
			}
			int type = drawinfo.heldItem.type;
			int num2 = 1;
			float num3 = -4f;
			float num4 = -8f;
			int shader = 0;
			if (drawinfo.drawPlayer.turtleArmor)
			{
				num2 = 4;
				shader = drawinfo.cBody;
			}
			else if (drawinfo.drawPlayer.body == 106)
			{
				num2 = 6;
				shader = drawinfo.cBody;
			}
			else if (drawinfo.drawPlayer.body == 170)
			{
				num2 = 7;
				shader = drawinfo.cBody;
			}
			else
			{
				switch (type)
				{
				case 1178:
					num2 = 1;
					break;
				case 779:
					num2 = 2;
					break;
				case 5134:
					num2 = 9;
					break;
				case 1295:
					num2 = 3;
					break;
				case 1910:
					num2 = 5;
					break;
				}
			}
			Vector2 val3 = new Vector2(0f, 8f);
			Vector2 vec5 = drawinfo.Position - Main.screenPosition + drawinfo.drawPlayer.bodyPosition + new Vector2((float)(drawinfo.drawPlayer.width / 2), (float)(drawinfo.drawPlayer.height - drawinfo.drawPlayer.bodyFrame.Height / 2)) + new Vector2(0f, -4f) + val3;
			vec5 = vec5.Floor();
			Vector2 vec6 = drawinfo.Position - Main.screenPosition + new Vector2((float)(drawinfo.drawPlayer.width / 2), (float)(drawinfo.drawPlayer.height - drawinfo.drawPlayer.bodyFrame.Height / 2)) + new Vector2((-9f + num3) * (float)drawinfo.drawPlayer.direction, (2f + num4) * drawinfo.drawPlayer.gravDir) + val3;
			vec6 = vec6.Floor();
			switch (num2)
			{
			case 7:
			{
				DrawData item = new DrawData(TextureAssets.BackPack[num2].Value, vec5, new Rectangle(0, drawinfo.drawPlayer.bodyFrame.Y, TextureAssets.BackPack[num2].Width(), drawinfo.drawPlayer.bodyFrame.Height), drawinfo.colorArmorBody, drawinfo.drawPlayer.bodyRotation, new Vector2((float)TextureAssets.BackPack[num2].Width() * 0.5f, drawinfo.bodyVect.Y), 1f, drawinfo.playerEffect);
				item.shader = shader;
				drawinfo.DrawDataCache.Add(item);
				break;
			}
			case 4:
			case 6:
			{
				DrawData item = new DrawData(TextureAssets.BackPack[num2].Value, vec5, drawinfo.drawPlayer.bodyFrame, drawinfo.colorArmorBody, drawinfo.drawPlayer.bodyRotation, drawinfo.bodyVect, 1f, drawinfo.playerEffect);
				item.shader = shader;
				drawinfo.DrawDataCache.Add(item);
				break;
			}
			default:
			{
				DrawData item = new DrawData(TextureAssets.BackPack[num2].Value, vec6, new Rectangle(0, 0, TextureAssets.BackPack[num2].Width(), TextureAssets.BackPack[num2].Height()), drawinfo.colorArmorBody, drawinfo.drawPlayer.bodyRotation, new Vector2((float)(TextureAssets.BackPack[num2].Width() / 2), (float)(TextureAssets.BackPack[num2].Height() / 2)), 1f, drawinfo.playerEffect);
				item.shader = shader;
				drawinfo.DrawDataCache.Add(item);
				break;
			}
			}
		}
	}

	public static void DrawPlayer_08_1_Tails(ref PlayerDrawSet drawinfo)
	{
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0101: Unknown result type (might be due to invalid IL or missing references)
		//IL_0102: Unknown result type (might be due to invalid IL or missing references)
		//IL_0107: Unknown result type (might be due to invalid IL or missing references)
		//IL_0108: Unknown result type (might be due to invalid IL or missing references)
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		//IL_010e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0127: Unknown result type (might be due to invalid IL or missing references)
		//IL_012e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0139: Unknown result type (might be due to invalid IL or missing references)
		//IL_014a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0155: Unknown result type (might be due to invalid IL or missing references)
		if (drawinfo.drawPlayer.tail > 0 && drawinfo.drawPlayer.tail < ArmorIDs.Back.Count && !drawinfo.drawPlayer.mount.Active)
		{
			Vector2 zero = Vector2.Zero;
			if (drawinfo.isSitting)
			{
				zero.Y += -2f;
			}
			if (!drawinfo.drawPlayer.Male)
			{
				zero.X += 2 * drawinfo.drawPlayer.direction;
			}
			Vector2 val = new Vector2(0f, 8f);
			Vector2 vec = zero + drawinfo.Position - Main.screenPosition + drawinfo.drawPlayer.bodyPosition + new Vector2((float)(drawinfo.drawPlayer.width / 2), (float)(drawinfo.drawPlayer.height - drawinfo.drawPlayer.bodyFrame.Height / 2)) + new Vector2(0f, -4f) + val;
			vec = vec.Floor();
			DrawData item = new DrawData(TextureAssets.AccBack[drawinfo.drawPlayer.tail].Value, vec, drawinfo.drawPlayer.bodyFrame, drawinfo.colorArmorBody, drawinfo.drawPlayer.bodyRotation, drawinfo.bodyVect, 1f, drawinfo.playerEffect);
			item.shader = drawinfo.cTail;
			drawinfo.DrawDataCache.Add(item);
		}
	}

	public static void DrawPlayer_10_BackAcc(ref PlayerDrawSet drawinfo)
	{
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0127: Unknown result type (might be due to invalid IL or missing references)
		//IL_012c: Unknown result type (might be due to invalid IL or missing references)
		//IL_013b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0140: Unknown result type (might be due to invalid IL or missing references)
		//IL_0145: Unknown result type (might be due to invalid IL or missing references)
		//IL_0146: Unknown result type (might be due to invalid IL or missing references)
		//IL_014b: Unknown result type (might be due to invalid IL or missing references)
		//IL_014c: Unknown result type (might be due to invalid IL or missing references)
		//IL_014d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0152: Unknown result type (might be due to invalid IL or missing references)
		//IL_016b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0172: Unknown result type (might be due to invalid IL or missing references)
		//IL_017d: Unknown result type (might be due to invalid IL or missing references)
		//IL_018e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0199: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01de: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0204: Unknown result type (might be due to invalid IL or missing references)
		//IL_0207: Unknown result type (might be due to invalid IL or missing references)
		//IL_0220: Unknown result type (might be due to invalid IL or missing references)
		//IL_0246: Unknown result type (might be due to invalid IL or missing references)
		//IL_0251: Unknown result type (might be due to invalid IL or missing references)
		//IL_0256: Unknown result type (might be due to invalid IL or missing references)
		//IL_0258: Unknown result type (might be due to invalid IL or missing references)
		//IL_026c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0271: Unknown result type (might be due to invalid IL or missing references)
		//IL_0285: Unknown result type (might be due to invalid IL or missing references)
		//IL_0294: Unknown result type (might be due to invalid IL or missing references)
		//IL_0299: Unknown result type (might be due to invalid IL or missing references)
		//IL_029e: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02be: Unknown result type (might be due to invalid IL or missing references)
		if (drawinfo.drawPlayer.back <= 0 || drawinfo.drawPlayer.back >= ArmorIDs.Back.Count)
		{
			return;
		}
		if (drawinfo.drawPlayer.front >= 1 && drawinfo.drawPlayer.front <= 4)
		{
			int num = drawinfo.drawPlayer.bodyFrame.Y / 56;
			if (num < 1 || num > 5)
			{
				drawinfo.armorAdjust = 10;
			}
			else
			{
				if (drawinfo.drawPlayer.front == 1)
				{
					drawinfo.armorAdjust = 0;
				}
				if (drawinfo.drawPlayer.front == 2)
				{
					drawinfo.armorAdjust = 8;
				}
				if (drawinfo.drawPlayer.front == 3)
				{
					drawinfo.armorAdjust = 0;
				}
				if (drawinfo.drawPlayer.front == 4)
				{
					drawinfo.armorAdjust = 8;
				}
			}
		}
		Vector2 zero = Vector2.Zero;
		Vector2 val = new Vector2(0f, 8f);
		Vector2 vec = zero + drawinfo.Position - Main.screenPosition + drawinfo.drawPlayer.bodyPosition + new Vector2((float)(drawinfo.drawPlayer.width / 2), (float)(drawinfo.drawPlayer.height - drawinfo.drawPlayer.bodyFrame.Height / 2)) + new Vector2(0f, -4f) + val;
		vec = vec.Floor();
		DrawData item = new DrawData(TextureAssets.AccBack[drawinfo.drawPlayer.back].Value, vec, drawinfo.drawPlayer.bodyFrame, drawinfo.colorArmorBody, drawinfo.drawPlayer.bodyRotation, drawinfo.bodyVect, 1f, drawinfo.playerEffect);
		item.shader = drawinfo.cBack;
		drawinfo.DrawDataCache.Add(item);
		if (drawinfo.drawPlayer.back == 36)
		{
			Rectangle bodyFrame = drawinfo.drawPlayer.bodyFrame;
			Rectangle value = bodyFrame;
			value.Width = 2;
			int num2 = 0;
			int num3 = bodyFrame.Width / 2;
			int num4 = 2;
			if (((int)drawinfo.playerEffect & 1) != 0)
			{
				num2 = bodyFrame.Width - 2;
				num4 = -2;
			}
			for (int i = 0; i < num3; i++)
			{
				value.X = bodyFrame.X + 2 * i;
				Color immuneAlpha = drawinfo.drawPlayer.GetImmuneAlpha(LiquidRenderer.GetShimmerGlitterColor(top: true, (float)i / 16f, 0f), drawinfo.shadow);
				immuneAlpha *= (float)(int)drawinfo.colorArmorBody.A / 255f;
				item = new DrawData(TextureAssets.GlowMask[332].Value, vec + new Vector2((float)(num2 + i * num4), 0f), value, immuneAlpha, drawinfo.drawPlayer.bodyRotation, drawinfo.bodyVect, 1f, drawinfo.playerEffect);
				item.shader = drawinfo.cBack;
				drawinfo.DrawDataCache.Add(item);
			}
		}
	}

	public static void DrawPlayer_09_Wings(ref PlayerDrawSet drawinfo)
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_010d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0112: Unknown result type (might be due to invalid IL or missing references)
		//IL_011c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0123: Unknown result type (might be due to invalid IL or missing references)
		//IL_0128: Unknown result type (might be due to invalid IL or missing references)
		//IL_0129: Unknown result type (might be due to invalid IL or missing references)
		//IL_012e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0133: Unknown result type (might be due to invalid IL or missing references)
		//IL_0494: Unknown result type (might be due to invalid IL or missing references)
		//IL_0499: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_04af: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_050d: Unknown result type (might be due to invalid IL or missing references)
		//IL_050f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0514: Unknown result type (might be due to invalid IL or missing references)
		//IL_051b: Unknown result type (might be due to invalid IL or missing references)
		//IL_051d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0527: Unknown result type (might be due to invalid IL or missing references)
		//IL_0537: Unknown result type (might be due to invalid IL or missing references)
		//IL_0539: Unknown result type (might be due to invalid IL or missing references)
		//IL_0543: Unknown result type (might be due to invalid IL or missing references)
		//IL_054e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0589: Unknown result type (might be due to invalid IL or missing references)
		//IL_058b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0590: Unknown result type (might be due to invalid IL or missing references)
		//IL_0597: Unknown result type (might be due to invalid IL or missing references)
		//IL_0599: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_05bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_0398: Unknown result type (might be due to invalid IL or missing references)
		//IL_039a: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_043c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0447: Unknown result type (might be due to invalid IL or missing references)
		//IL_0617: Unknown result type (might be due to invalid IL or missing references)
		//IL_0618: Unknown result type (might be due to invalid IL or missing references)
		//IL_0632: Unknown result type (might be due to invalid IL or missing references)
		//IL_063e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0645: Unknown result type (might be due to invalid IL or missing references)
		//IL_064a: Unknown result type (might be due to invalid IL or missing references)
		//IL_064b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0650: Unknown result type (might be due to invalid IL or missing references)
		//IL_0655: Unknown result type (might be due to invalid IL or missing references)
		//IL_0663: Unknown result type (might be due to invalid IL or missing references)
		//IL_0668: Unknown result type (might be due to invalid IL or missing references)
		//IL_0682: Unknown result type (might be due to invalid IL or missing references)
		//IL_0684: Unknown result type (might be due to invalid IL or missing references)
		//IL_06dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_06e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0726: Unknown result type (might be due to invalid IL or missing references)
		//IL_0731: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ac2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0acd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ad2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ae1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0962: Unknown result type (might be due to invalid IL or missing references)
		//IL_0973: Unknown result type (might be due to invalid IL or missing references)
		//IL_0991: Unknown result type (might be due to invalid IL or missing references)
		//IL_099c: Unknown result type (might be due to invalid IL or missing references)
		//IL_09a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_09ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_09b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_09b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_09ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_09c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_09c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_09cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_09d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_09e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_09ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_09f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_09f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a0a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a0f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a2d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a2f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a34: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a3b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a48: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a4a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a54: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a5f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0781: Unknown result type (might be due to invalid IL or missing references)
		//IL_079a: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_07b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_07bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c2c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c31: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c36: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c3b: Unknown result type (might be due to invalid IL or missing references)
		//IL_018e: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_022c: Unknown result type (might be due to invalid IL or missing references)
		//IL_026e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0275: Unknown result type (might be due to invalid IL or missing references)
		//IL_027a: Unknown result type (might be due to invalid IL or missing references)
		//IL_027c: Unknown result type (might be due to invalid IL or missing references)
		//IL_027e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0280: Unknown result type (might be due to invalid IL or missing references)
		//IL_0285: Unknown result type (might be due to invalid IL or missing references)
		//IL_028a: Unknown result type (might be due to invalid IL or missing references)
		//IL_029e: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_033f: Unknown result type (might be due to invalid IL or missing references)
		//IL_034a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e47: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e4c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e74: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e79: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e95: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e9a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e9c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ea1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ebb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ebc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ec4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ec9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ece: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ed3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ee4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0eee: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ef3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ef8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f0d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f12: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f30: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f32: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f37: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f3e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f4b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f4d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f57: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f62: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c7c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c84: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c89: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cb1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cb6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cd2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cd7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cd9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cde: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cf8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cf9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d01: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d06: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d0b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d10: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d21: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d2b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d30: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d35: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d4a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d4f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d6d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d6f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d74: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d7b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d88: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d8a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d94: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d9f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dd9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ddb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0de0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0de7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0df4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0df6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e00: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e0b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0af7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b15: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b47: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b4c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b51: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b56: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b5b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b5d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b62: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b73: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b7d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b82: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b87: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b9b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ba0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bbe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bc0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bc5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bcc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bd9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bdb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0be5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bf0: Unknown result type (might be due to invalid IL or missing references)
		//IL_07e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_07e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_07eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_07f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_07f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_084f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0859: Unknown result type (might be due to invalid IL or missing references)
		//IL_0899: Unknown result type (might be due to invalid IL or missing references)
		//IL_08a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_1145: Unknown result type (might be due to invalid IL or missing references)
		//IL_1156: Unknown result type (might be due to invalid IL or missing references)
		//IL_1173: Unknown result type (might be due to invalid IL or missing references)
		//IL_1174: Unknown result type (might be due to invalid IL or missing references)
		//IL_1176: Unknown result type (might be due to invalid IL or missing references)
		//IL_1177: Unknown result type (might be due to invalid IL or missing references)
		//IL_117c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1181: Unknown result type (might be due to invalid IL or missing references)
		//IL_0faf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fb4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fc2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fe0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0feb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ff5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ffa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fff: Unknown result type (might be due to invalid IL or missing references)
		//IL_1004: Unknown result type (might be due to invalid IL or missing references)
		//IL_1009: Unknown result type (might be due to invalid IL or missing references)
		//IL_1011: Unknown result type (might be due to invalid IL or missing references)
		//IL_1016: Unknown result type (might be due to invalid IL or missing references)
		//IL_101b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1020: Unknown result type (might be due to invalid IL or missing references)
		//IL_1031: Unknown result type (might be due to invalid IL or missing references)
		//IL_103b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1040: Unknown result type (might be due to invalid IL or missing references)
		//IL_1045: Unknown result type (might be due to invalid IL or missing references)
		//IL_1059: Unknown result type (might be due to invalid IL or missing references)
		//IL_105e: Unknown result type (might be due to invalid IL or missing references)
		//IL_107c: Unknown result type (might be due to invalid IL or missing references)
		//IL_107e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1083: Unknown result type (might be due to invalid IL or missing references)
		//IL_108a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1097: Unknown result type (might be due to invalid IL or missing references)
		//IL_1099: Unknown result type (might be due to invalid IL or missing references)
		//IL_10a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_10ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_14be: Unknown result type (might be due to invalid IL or missing references)
		//IL_14c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_14c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_14c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_14ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_14d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_14d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_14dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_14ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_14f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_14fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_1502: Unknown result type (might be due to invalid IL or missing references)
		//IL_151b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1520: Unknown result type (might be due to invalid IL or missing references)
		//IL_1526: Unknown result type (might be due to invalid IL or missing references)
		//IL_1528: Unknown result type (might be due to invalid IL or missing references)
		//IL_152d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1535: Unknown result type (might be due to invalid IL or missing references)
		//IL_157a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1585: Unknown result type (might be due to invalid IL or missing references)
		//IL_15b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_15be: Unknown result type (might be due to invalid IL or missing references)
		//IL_15d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_15e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_15e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_1600: Unknown result type (might be due to invalid IL or missing references)
		//IL_1602: Unknown result type (might be due to invalid IL or missing references)
		//IL_1607: Unknown result type (might be due to invalid IL or missing references)
		//IL_160e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1650: Unknown result type (might be due to invalid IL or missing references)
		//IL_165b: Unknown result type (might be due to invalid IL or missing references)
		//IL_13c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_13c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_13d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_13f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_13f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_13f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_13f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_13fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_1402: Unknown result type (might be due to invalid IL or missing references)
		//IL_1416: Unknown result type (might be due to invalid IL or missing references)
		//IL_141b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1439: Unknown result type (might be due to invalid IL or missing references)
		//IL_143b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1440: Unknown result type (might be due to invalid IL or missing references)
		//IL_1447: Unknown result type (might be due to invalid IL or missing references)
		//IL_1454: Unknown result type (might be due to invalid IL or missing references)
		//IL_1456: Unknown result type (might be due to invalid IL or missing references)
		//IL_1460: Unknown result type (might be due to invalid IL or missing references)
		//IL_146b: Unknown result type (might be due to invalid IL or missing references)
		//IL_118c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1191: Unknown result type (might be due to invalid IL or missing references)
		//IL_119a: Unknown result type (might be due to invalid IL or missing references)
		//IL_119f: Unknown result type (might be due to invalid IL or missing references)
		//IL_11a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_11fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_1205: Unknown result type (might be due to invalid IL or missing references)
		//IL_120a: Unknown result type (might be due to invalid IL or missing references)
		//IL_11c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_11c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_11c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_11d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_1714: Unknown result type (might be due to invalid IL or missing references)
		//IL_1719: Unknown result type (might be due to invalid IL or missing references)
		//IL_1795: Unknown result type (might be due to invalid IL or missing references)
		//IL_1247: Unknown result type (might be due to invalid IL or missing references)
		//IL_1249: Unknown result type (might be due to invalid IL or missing references)
		//IL_1255: Unknown result type (might be due to invalid IL or missing references)
		//IL_125a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1289: Unknown result type (might be due to invalid IL or missing references)
		//IL_12c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_12c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_12c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_12d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_12d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_12d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_12db: Unknown result type (might be due to invalid IL or missing references)
		//IL_12dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_12e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_12e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_12e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_12ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_12f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_12f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_12fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_12fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_12fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_1303: Unknown result type (might be due to invalid IL or missing references)
		//IL_1309: Unknown result type (might be due to invalid IL or missing references)
		//IL_130b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1310: Unknown result type (might be due to invalid IL or missing references)
		//IL_1317: Unknown result type (might be due to invalid IL or missing references)
		//IL_1324: Unknown result type (might be due to invalid IL or missing references)
		//IL_1326: Unknown result type (might be due to invalid IL or missing references)
		//IL_1330: Unknown result type (might be due to invalid IL or missing references)
		//IL_1335: Unknown result type (might be due to invalid IL or missing references)
		//IL_1337: Unknown result type (might be due to invalid IL or missing references)
		//IL_1805: Unknown result type (might be due to invalid IL or missing references)
		//IL_18a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_18b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_18ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_18bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_18c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_18c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_18df: Unknown result type (might be due to invalid IL or missing references)
		//IL_18e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_193b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1945: Unknown result type (might be due to invalid IL or missing references)
		//IL_1987: Unknown result type (might be due to invalid IL or missing references)
		//IL_1992: Unknown result type (might be due to invalid IL or missing references)
		//IL_189b: Unknown result type (might be due to invalid IL or missing references)
		//IL_18a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_18a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_1bb6: Unknown result type (might be due to invalid IL or missing references)
		//IL_1bc9: Unknown result type (might be due to invalid IL or missing references)
		//IL_1bcb: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c23: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c2d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c6c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c77: Unknown result type (might be due to invalid IL or missing references)
		//IL_19eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_19ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a26: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a82: Unknown result type (might be due to invalid IL or missing references)
		//IL_1cc1: Unknown result type (might be due to invalid IL or missing references)
		//IL_1cc3: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d1b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d36: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d41: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d52: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d94: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d9f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1abb: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ad2: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ad4: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ad6: Unknown result type (might be due to invalid IL or missing references)
		//IL_1adb: Unknown result type (might be due to invalid IL or missing references)
		//IL_1af3: Unknown result type (might be due to invalid IL or missing references)
		//IL_1afa: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b0a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b12: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f30: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f32: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f8a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1fa5: Unknown result type (might be due to invalid IL or missing references)
		//IL_1fb0: Unknown result type (might be due to invalid IL or missing references)
		//IL_1fc1: Unknown result type (might be due to invalid IL or missing references)
		//IL_2003: Unknown result type (might be due to invalid IL or missing references)
		//IL_200e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e01: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e03: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e5b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e79: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e84: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e95: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ed8: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ee3: Unknown result type (might be due to invalid IL or missing references)
		//IL_204a: Unknown result type (might be due to invalid IL or missing references)
		//IL_2055: Unknown result type (might be due to invalid IL or missing references)
		//IL_2066: Unknown result type (might be due to invalid IL or missing references)
		//IL_206b: Unknown result type (might be due to invalid IL or missing references)
		//IL_207f: Unknown result type (might be due to invalid IL or missing references)
		//IL_2081: Unknown result type (might be due to invalid IL or missing references)
		//IL_20d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_20e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_2122: Unknown result type (might be due to invalid IL or missing references)
		//IL_212d: Unknown result type (might be due to invalid IL or missing references)
		//IL_2359: Unknown result type (might be due to invalid IL or missing references)
		//IL_235b: Unknown result type (might be due to invalid IL or missing references)
		//IL_23b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_23cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_23d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_23e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_23f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_2435: Unknown result type (might be due to invalid IL or missing references)
		//IL_2440: Unknown result type (might be due to invalid IL or missing references)
		//IL_248d: Unknown result type (might be due to invalid IL or missing references)
		//IL_248f: Unknown result type (might be due to invalid IL or missing references)
		//IL_24e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_2501: Unknown result type (might be due to invalid IL or missing references)
		//IL_250c: Unknown result type (might be due to invalid IL or missing references)
		//IL_251d: Unknown result type (might be due to invalid IL or missing references)
		//IL_255f: Unknown result type (might be due to invalid IL or missing references)
		//IL_256a: Unknown result type (might be due to invalid IL or missing references)
		//IL_25ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_216b: Unknown result type (might be due to invalid IL or missing references)
		//IL_216d: Unknown result type (might be due to invalid IL or missing references)
		//IL_2177: Unknown result type (might be due to invalid IL or missing references)
		//IL_2191: Unknown result type (might be due to invalid IL or missing references)
		//IL_2196: Unknown result type (might be due to invalid IL or missing references)
		//IL_2198: Unknown result type (might be due to invalid IL or missing references)
		//IL_219f: Unknown result type (might be due to invalid IL or missing references)
		//IL_21a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_21b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_21be: Unknown result type (might be due to invalid IL or missing references)
		//IL_21c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_21c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_271a: Unknown result type (might be due to invalid IL or missing references)
		//IL_271f: Unknown result type (might be due to invalid IL or missing references)
		//IL_2724: Unknown result type (might be due to invalid IL or missing references)
		//IL_2754: Unknown result type (might be due to invalid IL or missing references)
		//IL_2759: Unknown result type (might be due to invalid IL or missing references)
		//IL_2a27: Unknown result type (might be due to invalid IL or missing references)
		//IL_2a29: Unknown result type (might be due to invalid IL or missing references)
		//IL_2a81: Unknown result type (might be due to invalid IL or missing references)
		//IL_2a9b: Unknown result type (might be due to invalid IL or missing references)
		//IL_2aa6: Unknown result type (might be due to invalid IL or missing references)
		//IL_2ab7: Unknown result type (might be due to invalid IL or missing references)
		//IL_2af9: Unknown result type (might be due to invalid IL or missing references)
		//IL_2b04: Unknown result type (might be due to invalid IL or missing references)
		//IL_25ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_25d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_25de: Unknown result type (might be due to invalid IL or missing references)
		//IL_25e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_25e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_25e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_25ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_2644: Unknown result type (might be due to invalid IL or missing references)
		//IL_2656: Unknown result type (might be due to invalid IL or missing references)
		//IL_2661: Unknown result type (might be due to invalid IL or missing references)
		//IL_2672: Unknown result type (might be due to invalid IL or missing references)
		//IL_26b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_26bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_21e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_21f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_21fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_21fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_2203: Unknown result type (might be due to invalid IL or missing references)
		//IL_2217: Unknown result type (might be due to invalid IL or missing references)
		//IL_2219: Unknown result type (might be due to invalid IL or missing references)
		//IL_221d: Unknown result type (might be due to invalid IL or missing references)
		//IL_2222: Unknown result type (might be due to invalid IL or missing references)
		//IL_2227: Unknown result type (might be due to invalid IL or missing references)
		//IL_2229: Unknown result type (might be due to invalid IL or missing references)
		//IL_2281: Unknown result type (might be due to invalid IL or missing references)
		//IL_228b: Unknown result type (might be due to invalid IL or missing references)
		//IL_2295: Unknown result type (might be due to invalid IL or missing references)
		//IL_22d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_22e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_279b: Unknown result type (might be due to invalid IL or missing references)
		//IL_27ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_27b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_27b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_27be: Unknown result type (might be due to invalid IL or missing references)
		//IL_27c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_27dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_27df: Unknown result type (might be due to invalid IL or missing references)
		//IL_27e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_27e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_283e: Unknown result type (might be due to invalid IL or missing references)
		//IL_2848: Unknown result type (might be due to invalid IL or missing references)
		//IL_2850: Unknown result type (might be due to invalid IL or missing references)
		//IL_2861: Unknown result type (might be due to invalid IL or missing references)
		//IL_286b: Unknown result type (might be due to invalid IL or missing references)
		//IL_28ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_28b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_2906: Unknown result type (might be due to invalid IL or missing references)
		//IL_2908: Unknown result type (might be due to invalid IL or missing references)
		//IL_2960: Unknown result type (might be due to invalid IL or missing references)
		//IL_296a: Unknown result type (might be due to invalid IL or missing references)
		//IL_2972: Unknown result type (might be due to invalid IL or missing references)
		//IL_2983: Unknown result type (might be due to invalid IL or missing references)
		//IL_298d: Unknown result type (might be due to invalid IL or missing references)
		//IL_29cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_29da: Unknown result type (might be due to invalid IL or missing references)
		if (drawinfo.drawPlayer.dead || drawinfo.hideEntirePlayer || drawinfo.hideEntirePlayerExceptHelmetsAndFaceAccessories)
		{
			return;
		}
		Vector2 directions = drawinfo.drawPlayer.Directions;
		Vector2 val = drawinfo.Position - Main.screenPosition + drawinfo.drawPlayer.Size / 2f;
		Vector2 val2 = new Vector2(0f, 7f);
		val = drawinfo.Position - Main.screenPosition + new Vector2((float)(drawinfo.drawPlayer.width / 2), (float)(drawinfo.drawPlayer.height - drawinfo.drawPlayer.bodyFrame.Height / 2)) + val2;
		if (drawinfo.drawPlayer.wings <= 0)
		{
			return;
		}
		Main.instance.LoadWings(drawinfo.drawPlayer.wings);
		DrawData item;
		if (drawinfo.drawPlayer.wings == 22)
		{
			if (!drawinfo.drawPlayer.ShouldDrawWingsThatAreAlwaysAnimated())
			{
				return;
			}
			Main.instance.LoadItemFlames(1866);
			Color colorArmorBody = drawinfo.colorArmorBody;
			int num = 26;
			int num2 = -9;
			Vector2 val3 = val + new Vector2((float)num2, (float)num) * directions;
			if (drawinfo.shadow == 0f && drawinfo.drawPlayer.grappling[0] == -1)
			{
				for (int i = 0; i < 7; i++)
				{
					Color color = new Color(250 - i * 10, 250 - i * 10, 250 - i * 10, 150 - i * 10);
					Vector2 val4 = new Vector2((float)Main.rand.Next(-10, 11) * 0.2f, (float)Main.rand.Next(-10, 11) * 0.2f);
					drawinfo.stealth *= drawinfo.stealth;
					drawinfo.stealth *= 1f - drawinfo.shadow;
					color = new Color((int)((float)(int)color.R * drawinfo.stealth), (int)((float)(int)color.G * drawinfo.stealth), (int)((float)(int)color.B * drawinfo.stealth), (int)((float)(int)color.A * drawinfo.stealth));
					val4.X = drawinfo.drawPlayer.itemFlamePos[i].X;
					val4.Y = 0f - drawinfo.drawPlayer.itemFlamePos[i].Y;
					val4 *= 0.5f;
					Vector2 position = (val3 + val4).Floor();
					item = new DrawData(TextureAssets.ItemFlame[1866].Value, position, new Rectangle(0, TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / 7 * drawinfo.drawPlayer.wingFrame, TextureAssets.Wings[drawinfo.drawPlayer.wings].Width(), TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / 7 - 2), color, drawinfo.drawPlayer.bodyRotation, new Vector2((float)(TextureAssets.Wings[drawinfo.drawPlayer.wings].Width() / 2), (float)(TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / 14)), 1f, drawinfo.playerEffect);
					item.shader = drawinfo.cWings;
					drawinfo.DrawDataCache.Add(item);
				}
			}
			item = new DrawData(TextureAssets.Wings[drawinfo.drawPlayer.wings].Value, val3.Floor(), new Rectangle(0, TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / 7 * drawinfo.drawPlayer.wingFrame, TextureAssets.Wings[drawinfo.drawPlayer.wings].Width(), TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / 7), colorArmorBody, drawinfo.drawPlayer.bodyRotation, new Vector2((float)(TextureAssets.Wings[drawinfo.drawPlayer.wings].Width() / 2), (float)(TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / 14)), 1f, drawinfo.playerEffect);
			item.shader = drawinfo.cWings;
			drawinfo.DrawDataCache.Add(item);
			return;
		}
		if (drawinfo.drawPlayer.wings == 28)
		{
			if (drawinfo.drawPlayer.ShouldDrawWingsThatAreAlwaysAnimated())
			{
				Color colorArmorBody2 = drawinfo.colorArmorBody;
				Vector2 val5 = new Vector2(0f, 19f);
				Vector2 vec = val + val5 * directions;
				Texture2D value = TextureAssets.Wings[drawinfo.drawPlayer.wings].Value;
				Rectangle val6 = value.Frame(1, 4, 0, drawinfo.drawPlayer.miscCounter / 5 % 4);
				val6.Width -= 2;
				val6.Height -= 2;
				item = new DrawData(value, vec.Floor(), val6, Color.Lerp(colorArmorBody2, Color.White, 1f), drawinfo.drawPlayer.bodyRotation, val6.Size() / 2f, 1f, drawinfo.playerEffect);
				item.shader = drawinfo.cWings;
				drawinfo.DrawDataCache.Add(item);
				value = TextureAssets.Extra[38].Value;
				item = new DrawData(value, vec.Floor(), val6, Color.Lerp(colorArmorBody2, Color.White, 0.5f), drawinfo.drawPlayer.bodyRotation, val6.Size() / 2f, 1f, drawinfo.playerEffect);
				item.shader = drawinfo.cWings;
				drawinfo.DrawDataCache.Add(item);
			}
			return;
		}
		if (drawinfo.drawPlayer.wings == 45)
		{
			if (!drawinfo.drawPlayer.ShouldDrawWingsThatAreAlwaysAnimated())
			{
				return;
			}
			DrawStarboardRainbowTrail(ref drawinfo, val, directions);
			Color val7 = new Color(255, 255, 255, 255);
			int num3 = 22;
			int num4 = 0;
			Vector2 vec2 = val + new Vector2((float)num4, (float)num3) * directions;
			Color color2 = val7 * (1f - drawinfo.shadow);
			item = new DrawData(TextureAssets.Wings[drawinfo.drawPlayer.wings].Value, vec2.Floor(), new Rectangle(0, TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / 6 * drawinfo.drawPlayer.wingFrame, TextureAssets.Wings[drawinfo.drawPlayer.wings].Width(), TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / 6), color2, drawinfo.drawPlayer.bodyRotation, new Vector2((float)(TextureAssets.Wings[drawinfo.drawPlayer.wings].Width() / 2), (float)(TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / 12)), 1f, drawinfo.playerEffect);
			item.shader = drawinfo.cWings;
			drawinfo.DrawDataCache.Add(item);
			if (drawinfo.shadow == 0f)
			{
				float num5 = ((float)drawinfo.drawPlayer.miscCounter / 75f * ((float)Math.PI * 2f)).ToRotationVector2().X * 4f;
				Color color3 = new Color(70, 70, 70, 0) * (num5 / 8f + 0.5f) * 0.4f;
				for (float num6 = 0f; num6 < (float)Math.PI * 2f; num6 += (float)Math.PI / 2f)
				{
					item = new DrawData(TextureAssets.Wings[drawinfo.drawPlayer.wings].Value, vec2.Floor() + num6.ToRotationVector2() * num5, new Rectangle(0, TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / 6 * drawinfo.drawPlayer.wingFrame, TextureAssets.Wings[drawinfo.drawPlayer.wings].Width(), TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / 6), color3, drawinfo.drawPlayer.bodyRotation, new Vector2((float)(TextureAssets.Wings[drawinfo.drawPlayer.wings].Width() / 2), (float)(TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / 12)), 1f, drawinfo.playerEffect);
					item.shader = drawinfo.cWings;
					drawinfo.DrawDataCache.Add(item);
				}
			}
			return;
		}
		if (drawinfo.drawPlayer.wings == 34)
		{
			if (drawinfo.drawPlayer.ShouldDrawWingsThatAreAlwaysAnimated())
			{
				drawinfo.stealth *= drawinfo.stealth;
				drawinfo.stealth *= 1f - drawinfo.shadow;
				Color color4 = new Color((int)(250f * drawinfo.stealth), (int)(250f * drawinfo.stealth), (int)(250f * drawinfo.stealth), (int)(100f * drawinfo.stealth));
				Vector2 val8 = new Vector2(0f, 0f);
				Texture2D value2 = TextureAssets.Wings[drawinfo.drawPlayer.wings].Value;
				Vector2 vec3 = drawinfo.Position + drawinfo.drawPlayer.Size / 2f - Main.screenPosition + val8 * drawinfo.drawPlayer.Directions - Vector2.UnitX * (float)drawinfo.drawPlayer.direction * 4f;
				Rectangle val9 = value2.Frame(1, 6, 0, drawinfo.drawPlayer.wingFrame);
				val9.Width -= 2;
				val9.Height -= 2;
				item = new DrawData(value2, vec3.Floor(), val9, color4, drawinfo.drawPlayer.bodyRotation, val9.Size() / 2f, 1f, drawinfo.playerEffect);
				item.shader = drawinfo.cWings;
				drawinfo.DrawDataCache.Add(item);
			}
			return;
		}
		if (drawinfo.drawPlayer.wings == 51)
		{
			drawinfo.stealth *= drawinfo.stealth;
			drawinfo.stealth *= 1f - drawinfo.shadow;
			Color color5 = GetLunaGlowColor(ref drawinfo, scaleByShadow: true) * drawinfo.stealth;
			Vector2 val10 = new Vector2(0f, (float)((drawinfo.drawPlayer.Directions.Y < 0f) ? 8 : 6));
			Texture2D value3 = TextureAssets.Wings[drawinfo.drawPlayer.wings].Value;
			Vector2 vec4 = drawinfo.Position + new Vector2((float)(drawinfo.drawPlayer.width / 2), (float)(drawinfo.drawPlayer.height - drawinfo.drawPlayer.bodyFrame.Height / 2)) - Main.screenPosition + val10 - Vector2.UnitX * (float)drawinfo.drawPlayer.direction * 4f;
			Rectangle val11 = value3.Frame(1, 8, 0, drawinfo.drawPlayer.wingFrame);
			val11.Width -= 2;
			val11.Height -= 2;
			item = new DrawData(value3, vec4.Floor(), val11, color5, drawinfo.drawPlayer.bodyRotation, val11.Size() / 2f, 1f, drawinfo.playerEffect);
			item.shader = drawinfo.cWings;
			drawinfo.DrawDataCache.Add(item);
			return;
		}
		if (drawinfo.drawPlayer.wings == 47)
		{
			Color colorArmorBody3 = drawinfo.colorArmorBody;
			Color val12 = GetChickenBonesGlowColor(ref drawinfo, scaleByShadow: true, wings: true);
			drawinfo.stealth *= drawinfo.stealth;
			drawinfo.stealth *= 1f - drawinfo.shadow;
			if (drawinfo.stealth == 1f)
			{
				val12.A = 180;
			}
			val12 = Color.Multiply(val12, drawinfo.stealth);
			Vector2 val13 = Main.OffsetsPlayerHeadgear[drawinfo.drawPlayer.bodyFrame.Y / drawinfo.drawPlayer.bodyFrame.Height];
			val13.Y -= 2f;
			Vector2 val14 = new Vector2(1f, 1f) + val13;
			Texture2D value4 = TextureAssets.Wings[drawinfo.drawPlayer.wings].Value;
			Vector2 vec5 = val + val14 * drawinfo.drawPlayer.Directions - Vector2.UnitX * (float)drawinfo.drawPlayer.direction * 4f;
			Rectangle val15 = value4.Frame(1, 11, 0, drawinfo.drawPlayer.wingFrame);
			val15.Width -= 2;
			val15.Height -= 2;
			item = new DrawData(value4, vec5.Floor(), val15, colorArmorBody3, drawinfo.drawPlayer.bodyRotation, val15.Size() / 2f, 1f, drawinfo.playerEffect);
			item.shader = drawinfo.cWings;
			drawinfo.DrawDataCache.Add(item);
			item = new DrawData(TextureAssets.GlowMask[366].Value, vec5.Floor(), val15, val12, drawinfo.drawPlayer.bodyRotation, val15.Size() / 2f, 1f, drawinfo.playerEffect);
			item.shader = drawinfo.cWings;
			drawinfo.DrawDataCache.Add(item);
			return;
		}
		if (drawinfo.drawPlayer.wings == 49)
		{
			Color colorArmorBody4 = drawinfo.colorArmorBody;
			Vector2 val16 = Main.OffsetsPlayerHeadgear[drawinfo.drawPlayer.bodyFrame.Y / drawinfo.drawPlayer.bodyFrame.Height];
			val16.Y -= 2f;
			Vector2 val17 = new Vector2(1f, 1f) + val16;
			Texture2D value5 = TextureAssets.Wings[drawinfo.drawPlayer.wings].Value;
			Vector2 vec6 = val + val17 * drawinfo.drawPlayer.Directions - Vector2.UnitX * (float)drawinfo.drawPlayer.direction * 4f;
			Rectangle val18 = value5.Frame(1, 11, 0, drawinfo.drawPlayer.wingFrame);
			val18.Width -= 2;
			val18.Height -= 2;
			item = new DrawData(value5, vec6.Floor(), val18, colorArmorBody4, drawinfo.drawPlayer.bodyRotation, val18.Size() / 2f, 1f, drawinfo.playerEffect);
			item.shader = drawinfo.cWings;
			drawinfo.DrawDataCache.Add(item);
			return;
		}
		if (drawinfo.drawPlayer.wings == 48)
		{
			if (drawinfo.drawPlayer.ShouldDrawWingsThatAreAlwaysAnimated())
			{
				Color colorArmorBody5 = drawinfo.colorArmorBody;
				Vector2 val19 = new Vector2(4f, 0f);
				Texture2D value6 = TextureAssets.Wings[drawinfo.drawPlayer.wings].Value;
				Vector2 vec7 = drawinfo.Position + drawinfo.drawPlayer.Size / 2f - Main.screenPosition + val19 * drawinfo.drawPlayer.Directions - Vector2.UnitX * (float)drawinfo.drawPlayer.direction * 4f;
				Rectangle val20 = value6.Frame(1, 8, 0, drawinfo.drawPlayer.wingFrame);
				val20.Width -= 2;
				val20.Height -= 2;
				item = new DrawData(value6, vec7.Floor(), val20, colorArmorBody5, drawinfo.drawPlayer.bodyRotation, val20.Size() / 2f, 1f, drawinfo.playerEffect);
				item.shader = drawinfo.cWings;
				drawinfo.DrawDataCache.Add(item);
			}
			return;
		}
		if (drawinfo.drawPlayer.wings == 40)
		{
			drawinfo.stealth *= drawinfo.stealth;
			drawinfo.stealth *= 1f - drawinfo.shadow;
			Color color6 = new Color((int)(250f * drawinfo.stealth), (int)(250f * drawinfo.stealth), (int)(250f * drawinfo.stealth), (int)(100f * drawinfo.stealth));
			Vector2 val21 = new Vector2(-4f, 0f);
			Texture2D value7 = TextureAssets.Wings[drawinfo.drawPlayer.wings].Value;
			Vector2 val22 = val + val21 * directions;
			for (int j = 0; j < 1; j++)
			{
				SpriteEffects val23 = drawinfo.playerEffect;
				Vector2 scale = new Vector2(1f);
				Vector2 zero = Vector2.Zero;
				zero.X = drawinfo.drawPlayer.direction * 3;
				if (j == 1)
				{
					val23 = (SpriteEffects)(val23 ^ (SpriteEffects)1);
					scale = new Vector2(0.7f, 1f);
					zero.X += (float)(-drawinfo.drawPlayer.direction) * 6f;
				}
				Vector2 val24 = drawinfo.drawPlayer.velocity * -1.5f;
				int num7 = 0;
				int num8 = 8;
				float num9 = 4f;
				if (drawinfo.drawPlayer.velocity.Y == 0f)
				{
					num7 = 8;
					num8 = 14;
					num9 = 3f;
				}
				for (int k = num7; k < num8; k++)
				{
					Vector2 val25 = val22;
					Rectangle val26 = value7.Frame(1, 14, 0, k);
					val26.Width -= 2;
					val26.Height -= 2;
					int num10 = (k - num7) % (int)num9;
					Vector2 val27 = new Vector2(0f, 0.5f).RotatedBy((drawinfo.drawPlayer.miscCounterNormalized * (2f + (float)num10) + (float)num10 * 0.5f + (float)j * 1.3f) * ((float)Math.PI * 2f)) * (float)(num10 + 1);
					val25 += val27;
					val25 += val24 * ((float)num10 / num9);
					val25 += zero;
					item = new DrawData(value7, val25.Floor(), val26, color6, drawinfo.drawPlayer.bodyRotation, val26.Size() / 2f, scale, val23);
					item.shader = drawinfo.cWings;
					drawinfo.DrawDataCache.Add(item);
				}
			}
			return;
		}
		if (drawinfo.drawPlayer.wings == 39)
		{
			if (drawinfo.drawPlayer.ShouldDrawWingsThatAreAlwaysAnimated())
			{
				drawinfo.stealth *= drawinfo.stealth;
				drawinfo.stealth *= 1f - drawinfo.shadow;
				Color colorArmorBody6 = drawinfo.colorArmorBody;
				Vector2 val28 = new Vector2(-6f, -7f);
				Texture2D value8 = TextureAssets.Wings[drawinfo.drawPlayer.wings].Value;
				Vector2 vec8 = val + val28 * directions;
				Rectangle val29 = value8.Frame(1, 6, 0, drawinfo.drawPlayer.wingFrame);
				val29.Width -= 2;
				val29.Height -= 2;
				item = new DrawData(value8, vec8.Floor(), val29, colorArmorBody6, drawinfo.drawPlayer.bodyRotation, val29.Size() / 2f, 1f, drawinfo.playerEffect);
				item.shader = drawinfo.cWings;
				drawinfo.DrawDataCache.Add(item);
			}
			return;
		}
		if (drawinfo.drawPlayer.wings == 50)
		{
			Texture2D value9 = TextureAssets.Wings[drawinfo.drawPlayer.wings].Value;
			Vector2 zero2 = Vector2.Zero;
			Vector2 vec9 = val + zero2 * drawinfo.drawPlayer.Directions - Vector2.UnitX * (float)drawinfo.drawPlayer.direction * 4f;
			int num11 = 11;
			Rectangle value10 = value9.Frame(1, num11, 0, drawinfo.drawPlayer.wingFrame);
			item = new DrawData(value9, vec9.Floor(), value10, drawinfo.colorArmorBody, drawinfo.drawPlayer.bodyRotation, new Vector2((float)(TextureAssets.Wings[drawinfo.drawPlayer.wings].Width() / 2), (float)(TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / num11 / 2)), 1f, drawinfo.playerEffect);
			item.shader = drawinfo.cWings;
			drawinfo.DrawDataCache.Add(item);
			Color color7 = drawinfo.drawPlayer.GetImmuneAlphaPure(Color.White, drawinfo.shadow) * (drawinfo.stealth * drawinfo.stealth) * (1f - drawinfo.shadow);
			item = new DrawData(TextureAssets.Wings[drawinfo.drawPlayer.wings].Value, vec9.Floor(), value10, color7, drawinfo.drawPlayer.bodyRotation, new Vector2((float)(TextureAssets.Wings[drawinfo.drawPlayer.wings].Width() / 2), (float)(TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / num11 / 2)), 1f, drawinfo.playerEffect);
			item.shader = drawinfo.cWings;
			drawinfo.DrawDataCache.Add(item);
			return;
		}
		int num12 = 0;
		int num13 = 0;
		int num14 = 4;
		if (drawinfo.drawPlayer.wings == 43)
		{
			num13 = -5;
			num12 = -7;
			num14 = 7;
		}
		else if (drawinfo.drawPlayer.wings == 44)
		{
			num14 = 7;
		}
		else if (drawinfo.drawPlayer.wings == 5)
		{
			num13 = 4;
			num12 -= 4;
		}
		else if (drawinfo.drawPlayer.wings == 27)
		{
			num13 = 3;
		}
		else if (drawinfo.drawPlayer.wings == 41)
		{
			num13 = -1;
		}
		else if (drawinfo.drawPlayer.wings == 12)
		{
			num13 = -1;
			num12 = -1;
		}
		Color val30 = drawinfo.colorArmorBody;
		if (drawinfo.drawPlayer.wings == 9 || drawinfo.drawPlayer.wings == 29)
		{
			drawinfo.stealth *= drawinfo.stealth;
			drawinfo.stealth *= 1f - drawinfo.shadow;
			val30 = new Color((int)(250f * drawinfo.stealth), (int)(250f * drawinfo.stealth), (int)(250f * drawinfo.stealth), (int)(100f * drawinfo.stealth));
		}
		if (drawinfo.drawPlayer.wings == 10)
		{
			drawinfo.stealth *= drawinfo.stealth;
			drawinfo.stealth *= 1f - drawinfo.shadow;
			val30 = new Color((int)(250f * drawinfo.stealth), (int)(250f * drawinfo.stealth), (int)(250f * drawinfo.stealth), (int)(175f * drawinfo.stealth));
		}
		if (drawinfo.drawPlayer.wings == 11 && val30.A > Main.gFade)
		{
			val30.A = Main.gFade;
		}
		if (drawinfo.drawPlayer.wings == 31)
		{
			val30.A = (byte)(220f * drawinfo.stealth);
		}
		if (drawinfo.drawPlayer.wings == 32)
		{
			val30.A = (byte)(127f * drawinfo.stealth);
		}
		if (drawinfo.drawPlayer.wings == 6)
		{
			val30.A = (byte)(160f * drawinfo.stealth);
			val30 *= 0.9f;
		}
		Vector2 val31 = val + new Vector2((float)(num13 - 9), (float)(num12 + 2)) * directions;
		item = new DrawData(TextureAssets.Wings[drawinfo.drawPlayer.wings].Value, val31.Floor(), new Rectangle(0, TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / num14 * drawinfo.drawPlayer.wingFrame, TextureAssets.Wings[drawinfo.drawPlayer.wings].Width(), TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / num14), val30, drawinfo.drawPlayer.bodyRotation, new Vector2((float)(TextureAssets.Wings[drawinfo.drawPlayer.wings].Width() / 2), (float)(TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / num14 / 2)), 1f, drawinfo.playerEffect);
		item.shader = drawinfo.cWings;
		drawinfo.DrawDataCache.Add(item);
		if (drawinfo.drawPlayer.wings == 43 && drawinfo.shadow == 0f)
		{
			float num15 = drawinfo.stealth * drawinfo.stealth;
			Vector2 val32 = val31;
			Vector2 origin = new Vector2((float)(TextureAssets.Wings[drawinfo.drawPlayer.wings].Width() / 2), (float)(TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / num14 / 2));
			Rectangle value11 = new Rectangle(0, TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / num14 * drawinfo.drawPlayer.wingFrame, TextureAssets.Wings[drawinfo.drawPlayer.wings].Width(), TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / num14);
			for (int l = 0; l < 2; l++)
			{
				item = new DrawData(position: val32 + new Vector2((float)Main.rand.Next(-10, 10) * 0.125f, (float)Main.rand.Next(-10, 10) * 0.125f), texture: TextureAssets.GlowMask[272].Value, sourceRect: value11, color: Color.Multiply(new Color(230, 230, 230, 60), num15), rotation: drawinfo.drawPlayer.bodyRotation, origin: origin, scale: 1f, effect: drawinfo.playerEffect);
				item.shader = drawinfo.cWings;
				drawinfo.DrawDataCache.Add(item);
			}
		}
		if (drawinfo.drawPlayer.wings == 23)
		{
			drawinfo.stealth *= drawinfo.stealth;
			drawinfo.stealth *= 1f - drawinfo.shadow;
			val30 = new Color((int)(200f * drawinfo.stealth), (int)(200f * drawinfo.stealth), (int)(200f * drawinfo.stealth), (int)(200f * drawinfo.stealth));
			item = new DrawData(TextureAssets.Flames[8].Value, val31.Floor(), new Rectangle(0, TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / 4 * drawinfo.drawPlayer.wingFrame, TextureAssets.Wings[drawinfo.drawPlayer.wings].Width(), TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / 4), val30, drawinfo.drawPlayer.bodyRotation, new Vector2((float)(TextureAssets.Wings[drawinfo.drawPlayer.wings].Width() / 2), (float)(TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / 8)), 1f, drawinfo.playerEffect);
			item.shader = drawinfo.cWings;
			drawinfo.DrawDataCache.Add(item);
		}
		else if (drawinfo.drawPlayer.wings == 27)
		{
			item = new DrawData(TextureAssets.GlowMask[92].Value, val31.Floor(), new Rectangle(0, TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / 4 * drawinfo.drawPlayer.wingFrame, TextureAssets.Wings[drawinfo.drawPlayer.wings].Width(), TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / 4), new Color(255, 255, 255, 127) * drawinfo.stealth * (1f - drawinfo.shadow), drawinfo.drawPlayer.bodyRotation, new Vector2((float)(TextureAssets.Wings[drawinfo.drawPlayer.wings].Width() / 2), (float)(TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / 8)), 1f, drawinfo.playerEffect);
			item.shader = drawinfo.cWings;
			drawinfo.DrawDataCache.Add(item);
		}
		else if (drawinfo.drawPlayer.wings == 44)
		{
			PlayerRainbowWingsTextureContent playerRainbowWings = TextureAssets.RenderTargets.PlayerRainbowWings;
			playerRainbowWings.Request();
			if (playerRainbowWings.IsReady)
			{
				RenderTarget2D target = playerRainbowWings.GetTarget();
				item = new DrawData((Texture2D)(object)target, val31.Floor(), new Rectangle(0, TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / 7 * drawinfo.drawPlayer.wingFrame, TextureAssets.Wings[drawinfo.drawPlayer.wings].Width(), TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / 7), new Color(255, 255, 255, 255) * drawinfo.stealth * (1f - drawinfo.shadow), drawinfo.drawPlayer.bodyRotation, new Vector2((float)(TextureAssets.Wings[drawinfo.drawPlayer.wings].Width() / 2), (float)(TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / 14)), 1f, drawinfo.playerEffect);
				item.shader = drawinfo.cWings;
				drawinfo.DrawDataCache.Add(item);
			}
		}
		else if (drawinfo.drawPlayer.wings == 30)
		{
			item = new DrawData(TextureAssets.GlowMask[181].Value, val31.Floor(), new Rectangle(0, TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / 4 * drawinfo.drawPlayer.wingFrame, TextureAssets.Wings[drawinfo.drawPlayer.wings].Width(), TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / 4), new Color(255, 255, 255, 127) * drawinfo.stealth * (1f - drawinfo.shadow), drawinfo.drawPlayer.bodyRotation, new Vector2((float)(TextureAssets.Wings[drawinfo.drawPlayer.wings].Width() / 2), (float)(TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / 8)), 1f, drawinfo.playerEffect);
			item.shader = drawinfo.cWings;
			drawinfo.DrawDataCache.Add(item);
		}
		else if (drawinfo.drawPlayer.wings == 38)
		{
			Color val33 = drawinfo.ArkhalisColor * drawinfo.stealth * (1f - drawinfo.shadow);
			item = new DrawData(TextureAssets.GlowMask[251].Value, val31.Floor(), new Rectangle(0, TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / 4 * drawinfo.drawPlayer.wingFrame, TextureAssets.Wings[drawinfo.drawPlayer.wings].Width(), TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / 4), val33, drawinfo.drawPlayer.bodyRotation, new Vector2((float)(TextureAssets.Wings[drawinfo.drawPlayer.wings].Width() / 2), (float)(TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / 8)), 1f, drawinfo.playerEffect);
			item.shader = drawinfo.cWings;
			drawinfo.DrawDataCache.Add(item);
			for (int num16 = drawinfo.drawPlayer.shadowPos.Length - 2; num16 >= 0; num16--)
			{
				Color val34 = val33;
				val34.A = 0;
				val34 *= MathHelper.Lerp(1f, 0f, (float)num16 / 3f);
				val34 *= 0.1f;
				Vector2 val35 = drawinfo.drawPlayer.shadowPos[num16] - drawinfo.drawPlayer.position;
				for (float num17 = 0f; num17 < 1f; num17 += 0.01f)
				{
					Vector2 val36 = new Vector2(2f, 0f).RotatedBy(num17 / 0.04f * ((float)Math.PI * 2f));
					item = new DrawData(TextureAssets.GlowMask[251].Value, val36 + val35 * num17 + val31, new Rectangle(0, TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / 4 * drawinfo.drawPlayer.wingFrame, TextureAssets.Wings[drawinfo.drawPlayer.wings].Width(), TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / 4), val34 * (1f - num17), drawinfo.drawPlayer.bodyRotation, new Vector2((float)(TextureAssets.Wings[drawinfo.drawPlayer.wings].Width() / 2), (float)(TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / 8)), 1f, drawinfo.playerEffect);
					item.shader = drawinfo.cWings;
					drawinfo.DrawDataCache.Add(item);
				}
			}
		}
		else if (drawinfo.drawPlayer.wings == 29)
		{
			item = new DrawData(TextureAssets.Wings[drawinfo.drawPlayer.wings].Value, val31.Floor(), new Rectangle(0, TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / 4 * drawinfo.drawPlayer.wingFrame, TextureAssets.Wings[drawinfo.drawPlayer.wings].Width(), TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / 4), new Color(255, 255, 255, 0) * drawinfo.stealth * (1f - drawinfo.shadow) * 0.5f, drawinfo.drawPlayer.bodyRotation, new Vector2((float)(TextureAssets.Wings[drawinfo.drawPlayer.wings].Width() / 2), (float)(TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / 8)), 1.06f, drawinfo.playerEffect);
			item.shader = drawinfo.cWings;
			drawinfo.DrawDataCache.Add(item);
		}
		else if (drawinfo.drawPlayer.wings == 36)
		{
			item = new DrawData(TextureAssets.GlowMask[213].Value, val31.Floor(), new Rectangle(0, TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / 4 * drawinfo.drawPlayer.wingFrame, TextureAssets.Wings[drawinfo.drawPlayer.wings].Width(), TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / 4), new Color(255, 255, 255, 0) * drawinfo.stealth * (1f - drawinfo.shadow), drawinfo.drawPlayer.bodyRotation, new Vector2((float)(TextureAssets.Wings[drawinfo.drawPlayer.wings].Width() / 2), (float)(TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / 8)), 1.06f, drawinfo.playerEffect);
			item.shader = drawinfo.cWings;
			drawinfo.DrawDataCache.Add(item);
			Vector2 spinningpoint = new Vector2(0f, 2f - drawinfo.shadow * 2f);
			for (int m = 0; m < 4; m++)
			{
				item = new DrawData(TextureAssets.GlowMask[213].Value, spinningpoint.RotatedBy((float)Math.PI / 2f * (float)m) + val31.Floor(), new Rectangle(0, TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / 4 * drawinfo.drawPlayer.wingFrame, TextureAssets.Wings[drawinfo.drawPlayer.wings].Width(), TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / 4), new Color(127, 127, 127, 127) * drawinfo.stealth * (1f - drawinfo.shadow), drawinfo.drawPlayer.bodyRotation, new Vector2((float)(TextureAssets.Wings[drawinfo.drawPlayer.wings].Width() / 2), (float)(TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / 8)), 1f, drawinfo.playerEffect);
				item.shader = drawinfo.cWings;
				drawinfo.DrawDataCache.Add(item);
			}
		}
		else if (drawinfo.drawPlayer.wings == 31)
		{
			Color val37 = new Color(255, 255, 255, 0);
			val37 = Color.Lerp(Color.HotPink, Color.Crimson, (float)Math.Cos((float)Math.PI * 2f * ((float)drawinfo.drawPlayer.miscCounter / 100f)) * 0.4f + 0.5f);
			val37.A = 0;
			for (int n = 0; n < 4; n++)
			{
				Vector2 val38 = new Vector2((float)Math.Cos((float)Math.PI * 2f * ((float)drawinfo.drawPlayer.miscCounter / 60f)) * 0.5f + 0.5f, 0f).RotatedBy((float)n * ((float)Math.PI / 2f)) * 1f;
				item = new DrawData(TextureAssets.Wings[drawinfo.drawPlayer.wings].Value, val31.Floor() + val38, new Rectangle(0, TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / 4 * drawinfo.drawPlayer.wingFrame, TextureAssets.Wings[drawinfo.drawPlayer.wings].Width(), TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / 4), val37 * drawinfo.stealth * (1f - drawinfo.shadow) * 0.5f, drawinfo.drawPlayer.bodyRotation, new Vector2((float)(TextureAssets.Wings[drawinfo.drawPlayer.wings].Width() / 2), (float)(TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / 8)), 1f, drawinfo.playerEffect);
				item.shader = drawinfo.cWings;
				drawinfo.DrawDataCache.Add(item);
			}
			item = new DrawData(TextureAssets.Wings[drawinfo.drawPlayer.wings].Value, val31.Floor(), new Rectangle(0, TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / 4 * drawinfo.drawPlayer.wingFrame, TextureAssets.Wings[drawinfo.drawPlayer.wings].Width(), TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / 4), val37 * drawinfo.stealth * (1f - drawinfo.shadow) * 1f, drawinfo.drawPlayer.bodyRotation, new Vector2((float)(TextureAssets.Wings[drawinfo.drawPlayer.wings].Width() / 2), (float)(TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / 8)), 1f, drawinfo.playerEffect);
			item.shader = drawinfo.cWings;
			drawinfo.DrawDataCache.Add(item);
		}
		else if (drawinfo.drawPlayer.wings == 32)
		{
			item = new DrawData(TextureAssets.GlowMask[183].Value, val31.Floor(), new Rectangle(0, TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / 4 * drawinfo.drawPlayer.wingFrame, TextureAssets.Wings[drawinfo.drawPlayer.wings].Width(), TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / 4), new Color(255, 255, 255, 0) * drawinfo.stealth * (1f - drawinfo.shadow), drawinfo.drawPlayer.bodyRotation, new Vector2((float)(TextureAssets.Wings[drawinfo.drawPlayer.wings].Width() / 2), (float)(TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / 8)), 1.06f, drawinfo.playerEffect);
			item.shader = drawinfo.cWings;
			drawinfo.DrawDataCache.Add(item);
		}
	}

	public static void DrawPlayer_12_1_BalloonFronts(ref PlayerDrawSet drawinfo)
	{
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_010e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0119: Unknown result type (might be due to invalid IL or missing references)
		//IL_017f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0184: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0212: Unknown result type (might be due to invalid IL or missing references)
		//IL_0231: Unknown result type (might be due to invalid IL or missing references)
		//IL_0246: Unknown result type (might be due to invalid IL or missing references)
		//IL_024c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0251: Unknown result type (might be due to invalid IL or missing references)
		//IL_0256: Unknown result type (might be due to invalid IL or missing references)
		//IL_025b: Unknown result type (might be due to invalid IL or missing references)
		//IL_026c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0271: Unknown result type (might be due to invalid IL or missing references)
		//IL_0276: Unknown result type (might be due to invalid IL or missing references)
		//IL_029d: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_02af: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_031b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0326: Unknown result type (might be due to invalid IL or missing references)
		//IL_035e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0369: Unknown result type (might be due to invalid IL or missing references)
		if (drawinfo.drawPlayer.balloonFront <= 0 || drawinfo.drawPlayer.balloonFront >= ArmorIDs.Balloon.Count)
		{
			return;
		}
		DrawData item;
		if (ArmorIDs.Balloon.Sets.UsesTorsoFraming[drawinfo.drawPlayer.balloonFront])
		{
			item = new DrawData(TextureAssets.AccBalloon[drawinfo.drawPlayer.balloonFront].Value, new Vector2((float)(int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.bodyPosition + drawinfo.bodyVect, drawinfo.drawPlayer.bodyFrame, drawinfo.colorArmorBody, drawinfo.drawPlayer.bodyRotation, drawinfo.bodyVect, 1f, drawinfo.playerEffect);
			item.shader = drawinfo.cBalloonFront;
			drawinfo.DrawDataCache.Add(item);
			return;
		}
		int num = ((!FocusHelper.PausePlayerBalloonAnimations) ? (DateTime.Now.Millisecond % 800 / 200) : 0);
		Vector2 val = Main.OffsetsPlayerOffhand[drawinfo.drawPlayer.bodyFrame.Y / 56];
		if (drawinfo.drawPlayer.direction != 1)
		{
			val.X = (float)drawinfo.drawPlayer.width - val.X;
		}
		if (drawinfo.drawPlayer.gravDir != 1f)
		{
			val.Y -= drawinfo.drawPlayer.height;
		}
		Vector2 val2 = new Vector2(0f, 8f) + new Vector2(0f, 6f);
		Vector2 val3 = new Vector2((float)(int)(drawinfo.Position.X - Main.screenPosition.X + val.X), (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y + val.Y * drawinfo.drawPlayer.gravDir));
		val3 = drawinfo.Position - Main.screenPosition + val * new Vector2(1f, drawinfo.drawPlayer.gravDir) + new Vector2(0f, (float)(drawinfo.drawPlayer.height - drawinfo.drawPlayer.bodyFrame.Height)) + val2;
		val3 = val3.Floor();
		item = new DrawData(TextureAssets.AccBalloon[drawinfo.drawPlayer.balloonFront].Value, val3, new Rectangle(0, TextureAssets.AccBalloon[drawinfo.drawPlayer.balloonFront].Height() / 4 * num, TextureAssets.AccBalloon[drawinfo.drawPlayer.balloonFront].Width(), TextureAssets.AccBalloon[drawinfo.drawPlayer.balloonFront].Height() / 4), drawinfo.colorArmorBody, drawinfo.drawPlayer.bodyRotation, new Vector2((float)(26 + drawinfo.drawPlayer.direction * 4), 28f + drawinfo.drawPlayer.gravDir * 6f), 1f, drawinfo.playerEffect);
		item.shader = drawinfo.cBalloonFront;
		drawinfo.DrawDataCache.Add(item);
	}

	public static void DrawPlayer_11_Balloons(ref PlayerDrawSet drawinfo)
	{
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_010e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0119: Unknown result type (might be due to invalid IL or missing references)
		//IL_017f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0184: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0212: Unknown result type (might be due to invalid IL or missing references)
		//IL_0231: Unknown result type (might be due to invalid IL or missing references)
		//IL_0246: Unknown result type (might be due to invalid IL or missing references)
		//IL_024c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0251: Unknown result type (might be due to invalid IL or missing references)
		//IL_0256: Unknown result type (might be due to invalid IL or missing references)
		//IL_025b: Unknown result type (might be due to invalid IL or missing references)
		//IL_026c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0271: Unknown result type (might be due to invalid IL or missing references)
		//IL_0276: Unknown result type (might be due to invalid IL or missing references)
		//IL_029d: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_02af: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_031b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0326: Unknown result type (might be due to invalid IL or missing references)
		//IL_035e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0369: Unknown result type (might be due to invalid IL or missing references)
		if (drawinfo.drawPlayer.balloon <= 0 || drawinfo.drawPlayer.balloon >= ArmorIDs.Balloon.Count)
		{
			return;
		}
		DrawData item;
		if (ArmorIDs.Balloon.Sets.UsesTorsoFraming[drawinfo.drawPlayer.balloon])
		{
			item = new DrawData(TextureAssets.AccBalloon[drawinfo.drawPlayer.balloon].Value, new Vector2((float)(int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.bodyPosition + drawinfo.bodyVect, drawinfo.drawPlayer.bodyFrame, drawinfo.colorArmorBody, drawinfo.drawPlayer.bodyRotation, drawinfo.bodyVect, 1f, drawinfo.playerEffect);
			item.shader = drawinfo.cBalloon;
			drawinfo.DrawDataCache.Add(item);
			return;
		}
		int num = ((!FocusHelper.PausePlayerBalloonAnimations) ? (DateTime.Now.Millisecond % 800 / 200) : 0);
		Vector2 val = Main.OffsetsPlayerOffhand[drawinfo.drawPlayer.bodyFrame.Y / 56];
		if (drawinfo.drawPlayer.direction != 1)
		{
			val.X = (float)drawinfo.drawPlayer.width - val.X;
		}
		if (drawinfo.drawPlayer.gravDir != 1f)
		{
			val.Y -= drawinfo.drawPlayer.height;
		}
		Vector2 val2 = new Vector2(0f, 8f) + new Vector2(0f, 6f);
		Vector2 val3 = new Vector2((float)(int)(drawinfo.Position.X - Main.screenPosition.X + val.X), (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y + val.Y * drawinfo.drawPlayer.gravDir));
		val3 = drawinfo.Position - Main.screenPosition + val * new Vector2(1f, drawinfo.drawPlayer.gravDir) + new Vector2(0f, (float)(drawinfo.drawPlayer.height - drawinfo.drawPlayer.bodyFrame.Height)) + val2;
		val3 = val3.Floor();
		item = new DrawData(TextureAssets.AccBalloon[drawinfo.drawPlayer.balloon].Value, val3, new Rectangle(0, TextureAssets.AccBalloon[drawinfo.drawPlayer.balloon].Height() / 4 * num, TextureAssets.AccBalloon[drawinfo.drawPlayer.balloon].Width(), TextureAssets.AccBalloon[drawinfo.drawPlayer.balloon].Height() / 4), drawinfo.colorArmorBody, drawinfo.drawPlayer.bodyRotation, new Vector2((float)(26 + drawinfo.drawPlayer.direction * 4), 28f + drawinfo.drawPlayer.gravDir * 6f), 1f, drawinfo.playerEffect);
		item.shader = drawinfo.cBalloon;
		drawinfo.DrawDataCache.Add(item);
	}

	public static void DrawPlayer_12_Skin(ref PlayerDrawSet drawinfo)
	{
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Unknown result type (might be due to invalid IL or missing references)
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		//IL_010e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0119: Unknown result type (might be due to invalid IL or missing references)
		//IL_0124: Unknown result type (might be due to invalid IL or missing references)
		//IL_0135: Unknown result type (might be due to invalid IL or missing references)
		//IL_0140: Unknown result type (might be due to invalid IL or missing references)
		//IL_0268: Unknown result type (might be due to invalid IL or missing references)
		//IL_0273: Unknown result type (might be due to invalid IL or missing references)
		//IL_0278: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02be: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_02da: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cf: Unknown result type (might be due to invalid IL or missing references)
		if (drawinfo.usesCompositeTorso)
		{
			DrawPlayer_12_Skin_Composite(ref drawinfo);
			return;
		}
		if (drawinfo.isSitting)
		{
			drawinfo.hidesBottomSkin = true;
		}
		if (!drawinfo.hidesTopSkin)
		{
			drawinfo.Position.Y += drawinfo.torsoOffset;
			DrawData drawData = new DrawData(TextureAssets.Players[drawinfo.skinVar, 3].Value, new Vector2((float)(int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.bodyPosition + new Vector2((float)(drawinfo.drawPlayer.bodyFrame.Width / 2), (float)(drawinfo.drawPlayer.bodyFrame.Height / 2)), drawinfo.drawPlayer.bodyFrame, drawinfo.colorBodySkin, drawinfo.drawPlayer.bodyRotation, drawinfo.bodyVect, 1f, drawinfo.playerEffect);
			drawData.shader = drawinfo.skinDyePacked;
			DrawData item = drawData;
			drawinfo.DrawDataCache.Add(item);
			drawinfo.Position.Y -= drawinfo.torsoOffset;
		}
		if (!drawinfo.hidesBottomSkin && !IsBottomOverridden(ref drawinfo))
		{
			if (drawinfo.isSitting)
			{
				DrawSittingLegs(ref drawinfo, TextureAssets.Players[drawinfo.skinVar, 10].Value, drawinfo.colorLegs, 0, drawinfo.drawPlayer.legs, default, skin: true);
				return;
			}
			DrawData item = new DrawData(TextureAssets.Players[drawinfo.skinVar, 10].Value, new Vector2((float)(int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.bodyPosition + new Vector2((float)(drawinfo.drawPlayer.bodyFrame.Width / 2), (float)(drawinfo.drawPlayer.bodyFrame.Height / 2)), drawinfo.drawPlayer.legFrame, drawinfo.colorLegs, drawinfo.drawPlayer.legRotation, drawinfo.bodyVect, 1f, drawinfo.playerEffect);
			drawinfo.DrawDataCache.Add(item);
		}
	}

	public static bool IsBottomOverridden(ref PlayerDrawSet drawinfo)
	{
		if (ShouldOverrideLegs_CheckPants(ref drawinfo))
		{
			return true;
		}
		if (ShouldOverrideLegs_CheckShoes(ref drawinfo))
		{
			return true;
		}
		return false;
	}

	public static bool ShouldOverrideLegs_CheckPants(ref PlayerDrawSet drawinfo)
	{
		if (ShouldOverrideLegs_CheckShoes(ref drawinfo))
		{
			return false;
		}
		switch (drawinfo.drawPlayer.legs)
		{
		case 55:
		case 63:
		case 67:
		case 106:
		case 138:
		case 140:
		case 143:
		case 217:
		case 222:
		case 226:
		case 228:
			return true;
		default:
			return false;
		}
	}

	public static bool ShouldOverrideLegs_CheckShoes(ref PlayerDrawSet drawinfo)
	{
		sbyte shoe = drawinfo.drawPlayer.shoe;
		if (shoe == 15)
		{
			return true;
		}
		return false;
	}

	public static void DrawPlayer_12_Skin_Composite(ref PlayerDrawSet drawinfo)
	{
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0110: Unknown result type (might be due to invalid IL or missing references)
		//IL_0115: Unknown result type (might be due to invalid IL or missing references)
		//IL_0126: Unknown result type (might be due to invalid IL or missing references)
		//IL_0127: Unknown result type (might be due to invalid IL or missing references)
		//IL_0129: Unknown result type (might be due to invalid IL or missing references)
		//IL_012f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0131: Invalid comparison between Unknown and I4
		//IL_013a: Unknown result type (might be due to invalid IL or missing references)
		//IL_013f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0144: Unknown result type (might be due to invalid IL or missing references)
		//IL_0151: Unknown result type (might be due to invalid IL or missing references)
		//IL_0152: Unknown result type (might be due to invalid IL or missing references)
		//IL_0154: Unknown result type (might be due to invalid IL or missing references)
		//IL_0156: Unknown result type (might be due to invalid IL or missing references)
		//IL_015c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0161: Unknown result type (might be due to invalid IL or missing references)
		//IL_0164: Unknown result type (might be due to invalid IL or missing references)
		//IL_0169: Unknown result type (might be due to invalid IL or missing references)
		//IL_016b: Unknown result type (might be due to invalid IL or missing references)
		//IL_016d: Unknown result type (might be due to invalid IL or missing references)
		//IL_016f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0174: Unknown result type (might be due to invalid IL or missing references)
		//IL_0176: Unknown result type (might be due to invalid IL or missing references)
		//IL_0178: Unknown result type (might be due to invalid IL or missing references)
		//IL_017f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0184: Unknown result type (might be due to invalid IL or missing references)
		//IL_0186: Unknown result type (might be due to invalid IL or missing references)
		//IL_0188: Unknown result type (might be due to invalid IL or missing references)
		//IL_018a: Unknown result type (might be due to invalid IL or missing references)
		//IL_018f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0191: Unknown result type (might be due to invalid IL or missing references)
		//IL_0193: Unknown result type (might be due to invalid IL or missing references)
		//IL_021b: Unknown result type (might be due to invalid IL or missing references)
		//IL_021d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0228: Unknown result type (might be due to invalid IL or missing references)
		//IL_022f: Unknown result type (might be due to invalid IL or missing references)
		//IL_023a: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_01da: Unknown result type (might be due to invalid IL or missing references)
		//IL_0360: Unknown result type (might be due to invalid IL or missing references)
		//IL_036b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0370: Unknown result type (might be due to invalid IL or missing references)
		//IL_039b: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c2: Unknown result type (might be due to invalid IL or missing references)
		if (!drawinfo.hidesTopSkin && !drawinfo.drawPlayer.invis)
		{
			Vector2 val = new Vector2((float)(int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.bodyPosition + new Vector2((float)(drawinfo.drawPlayer.bodyFrame.Width / 2), (float)(drawinfo.drawPlayer.bodyFrame.Height / 2));
			val.Y += drawinfo.torsoOffset;
			Vector2 val2 = Main.OffsetsPlayerHeadgear[drawinfo.drawPlayer.bodyFrame.Y / drawinfo.drawPlayer.bodyFrame.Height];
			val2.Y -= 2f;
			val += val2 * (float)(-(((int)drawinfo.playerEffect & 2) > 0).ToDirectionInt());
			float bodyRotation = drawinfo.drawPlayer.bodyRotation;
			Vector2 val3 = val;
			Vector2 val4 = val;
			Vector2 bodyVect = drawinfo.bodyVect;
			Vector2 bodyVect2 = drawinfo.bodyVect;
			Vector2 compositeOffset_BackArm = GetCompositeOffset_BackArm(ref drawinfo);
			val3 += compositeOffset_BackArm;
			_ = bodyVect + compositeOffset_BackArm;
			Vector2 compositeOffset_FrontArm = GetCompositeOffset_FrontArm(ref drawinfo);
			bodyVect2 += compositeOffset_FrontArm;
			_ = val4 + compositeOffset_FrontArm;
			if (drawinfo.drawFloatingTube)
			{
				drawinfo.DrawDataCache.Add(new DrawData(TextureAssets.Extra[105].Value, val, new Rectangle(0, 0, 40, 56), drawinfo.floatingTubeColor, bodyRotation, drawinfo.bodyVect, 1f, drawinfo.playerEffect)
				{
					shader = drawinfo.cFloatingTube
				});
			}
			drawinfo.DrawDataCache.Add(new DrawData(TextureAssets.Players[drawinfo.skinVar, 3].Value, val, drawinfo.compTorsoFrame, drawinfo.colorBodySkin, bodyRotation, drawinfo.bodyVect, 1f, drawinfo.playerEffect)
			{
				shader = drawinfo.skinDyePacked
			});
		}
		if (!drawinfo.hidesBottomSkin && !drawinfo.drawPlayer.invis && !IsBottomOverridden(ref drawinfo))
		{
			if (drawinfo.isSitting)
			{
				DrawSittingLegs(ref drawinfo, TextureAssets.Players[drawinfo.skinVar, 10].Value, drawinfo.colorLegs, drawinfo.skinDyePacked, drawinfo.drawPlayer.legs, default, skin: true);
			}
			else
			{
				DrawData drawData = new DrawData(TextureAssets.Players[drawinfo.skinVar, 10].Value, new Vector2((float)(int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.bodyPosition + new Vector2((float)(drawinfo.drawPlayer.bodyFrame.Width / 2), (float)(drawinfo.drawPlayer.bodyFrame.Height / 2)), drawinfo.drawPlayer.legFrame, drawinfo.colorLegs, drawinfo.drawPlayer.legRotation, drawinfo.bodyVect, 1f, drawinfo.playerEffect);
				drawData.shader = drawinfo.skinDyePacked;
				DrawData item = drawData;
				drawinfo.DrawDataCache.Add(item);
			}
		}
		DrawPlayer_12_SkinComposite_BackArmShirt(ref drawinfo);
	}

	public static void DrawPlayer_12_SkinComposite_BackArmShirt(ref PlayerDrawSet drawinfo)
	{
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		//IL_0105: Invalid comparison between Unknown and I4
		//IL_010e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0113: Unknown result type (might be due to invalid IL or missing references)
		//IL_0118: Unknown result type (might be due to invalid IL or missing references)
		//IL_0136: Unknown result type (might be due to invalid IL or missing references)
		//IL_0137: Unknown result type (might be due to invalid IL or missing references)
		//IL_0138: Unknown result type (might be due to invalid IL or missing references)
		//IL_0139: Unknown result type (might be due to invalid IL or missing references)
		//IL_013c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0141: Unknown result type (might be due to invalid IL or missing references)
		//IL_0144: Unknown result type (might be due to invalid IL or missing references)
		//IL_0149: Unknown result type (might be due to invalid IL or missing references)
		//IL_014b: Unknown result type (might be due to invalid IL or missing references)
		//IL_014c: Unknown result type (might be due to invalid IL or missing references)
		//IL_014e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0153: Unknown result type (might be due to invalid IL or missing references)
		//IL_0154: Unknown result type (might be due to invalid IL or missing references)
		//IL_0157: Unknown result type (might be due to invalid IL or missing references)
		//IL_015c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0161: Unknown result type (might be due to invalid IL or missing references)
		//IL_0163: Unknown result type (might be due to invalid IL or missing references)
		//IL_0165: Unknown result type (might be due to invalid IL or missing references)
		//IL_0167: Unknown result type (might be due to invalid IL or missing references)
		//IL_016c: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_05bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_05fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_05fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0609: Unknown result type (might be due to invalid IL or missing references)
		//IL_0610: Unknown result type (might be due to invalid IL or missing references)
		//IL_0618: Unknown result type (might be due to invalid IL or missing references)
		//IL_04dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_04df: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0404: Unknown result type (might be due to invalid IL or missing references)
		//IL_040b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0413: Unknown result type (might be due to invalid IL or missing references)
		//IL_032d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0330: Unknown result type (might be due to invalid IL or missing references)
		//IL_033b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0342: Unknown result type (might be due to invalid IL or missing references)
		//IL_034d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0236: Unknown result type (might be due to invalid IL or missing references)
		//IL_0238: Unknown result type (might be due to invalid IL or missing references)
		//IL_0243: Unknown result type (might be due to invalid IL or missing references)
		//IL_024a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0252: Unknown result type (might be due to invalid IL or missing references)
		//IL_07d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_07d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_07d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_07d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_07e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_07e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_080b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0810: Unknown result type (might be due to invalid IL or missing references)
		//IL_0815: Unknown result type (might be due to invalid IL or missing references)
		//IL_081f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0821: Unknown result type (might be due to invalid IL or missing references)
		//IL_0829: Unknown result type (might be due to invalid IL or missing references)
		//IL_0842: Unknown result type (might be due to invalid IL or missing references)
		//IL_084a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0757: Unknown result type (might be due to invalid IL or missing references)
		//IL_0759: Unknown result type (might be due to invalid IL or missing references)
		//IL_0764: Unknown result type (might be due to invalid IL or missing references)
		//IL_076b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0773: Unknown result type (might be due to invalid IL or missing references)
		//IL_06cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_06d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_06dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_06e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_06eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_067b: Unknown result type (might be due to invalid IL or missing references)
		//IL_067e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0689: Unknown result type (might be due to invalid IL or missing references)
		//IL_0690: Unknown result type (might be due to invalid IL or missing references)
		//IL_069b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0544: Unknown result type (might be due to invalid IL or missing references)
		//IL_0546: Unknown result type (might be due to invalid IL or missing references)
		//IL_0551: Unknown result type (might be due to invalid IL or missing references)
		//IL_0558: Unknown result type (might be due to invalid IL or missing references)
		//IL_0560: Unknown result type (might be due to invalid IL or missing references)
		//IL_0468: Unknown result type (might be due to invalid IL or missing references)
		//IL_046a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0475: Unknown result type (might be due to invalid IL or missing references)
		//IL_047c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0484: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_029d: Unknown result type (might be due to invalid IL or missing references)
		//IL_029f: Unknown result type (might be due to invalid IL or missing references)
		//IL_02aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b9: Unknown result type (might be due to invalid IL or missing references)
		Vector2 val = new Vector2((float)(int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.bodyPosition + new Vector2((float)(drawinfo.drawPlayer.bodyFrame.Width / 2), (float)(drawinfo.drawPlayer.bodyFrame.Height / 2));
		Vector2 val2 = Main.OffsetsPlayerHeadgear[drawinfo.drawPlayer.bodyFrame.Y / drawinfo.drawPlayer.bodyFrame.Height];
		val2.Y -= 2f;
		val += val2 * (float)(-(((int)drawinfo.playerEffect & 2) > 0).ToDirectionInt());
		val.Y += drawinfo.torsoOffset;
		float bodyRotation = drawinfo.drawPlayer.bodyRotation;
		Vector2 val3 = val;
		Vector2 val4 = val;
		Vector2 bodyVect = drawinfo.bodyVect;
		Vector2 compositeOffset_BackArm = GetCompositeOffset_BackArm(ref drawinfo);
		val3 += compositeOffset_BackArm;
		val4 += drawinfo.backShoulderOffset;
		bodyVect += compositeOffset_BackArm;
		float rotation = bodyRotation + drawinfo.compositeBackArmRotation;
		bool flag = !drawinfo.drawPlayer.invis;
		bool flag2 = !drawinfo.drawPlayer.invis;
		bool flag3 = drawinfo.drawPlayer.body > 0 && drawinfo.drawPlayer.body < ArmorIDs.Body.Count;
		bool flag4 = drawinfo.drawPlayer.coat > 0 && drawinfo.drawPlayer.coat < ArmorIDs.Body.Count;
		bool flag5 = !drawinfo.hidesTopSkin;
		bool flag6 = false;
		if (flag3)
		{
			flag &= drawinfo.missingHand;
			if (flag2 && drawinfo.missingArm)
			{
				if (flag5)
				{
					drawinfo.DrawDataCache.Add(new DrawData(TextureAssets.Players[drawinfo.skinVar, 7].Value, val3, drawinfo.compBackArmFrame, drawinfo.colorBodySkin, rotation, bodyVect, 1f, drawinfo.playerEffect)
					{
						shader = drawinfo.skinDyePacked
					});
				}
				if (!flag6 & flag5)
				{
					drawinfo.DrawDataCache.Add(new DrawData(TextureAssets.Players[drawinfo.skinVar, 5].Value, val3, drawinfo.compBackArmFrame, drawinfo.colorBodySkin, rotation, bodyVect, 1f, drawinfo.playerEffect)
					{
						shader = drawinfo.skinDyePacked
					});
					flag6 = true;
				}
				flag2 = false;
			}
			if (!drawinfo.drawPlayer.invis || IsArmorDrawnWhenInvisible(drawinfo.drawPlayer.body))
			{
				Texture2D value = TextureAssets.ArmorBodyComposite[drawinfo.drawPlayer.body].Value;
				if (!drawinfo.hideCompositeShoulders)
				{
					DrawCompositeArmorPiece(ref drawinfo, CompositePlayerDrawContext.BackShoulder, new DrawData(value, val4, drawinfo.compBackShoulderFrame, drawinfo.colorArmorBody, bodyRotation, drawinfo.bodyVect, 1f, drawinfo.playerEffect)
					{
						shader = drawinfo.cBody
					}, drawinfo.drawPlayer.body);
					if (drawinfo.drawPlayer.body == 71)
					{
						Texture2D value2 = TextureAssets.Extra[277].Value;
						DrawCompositeArmorPiece(ref drawinfo, CompositePlayerDrawContext.BackShoulder, new DrawData(value2, val4, drawinfo.compBackShoulderFrame, drawinfo.colorArmorBody, bodyRotation, drawinfo.bodyVect, 1f, drawinfo.playerEffect)
						{
							shader = 0
						}, drawinfo.drawPlayer.body);
					}
				}
				DrawPlayer_12_1_BalloonFronts(ref drawinfo);
				DrawCompositeArmorPiece(ref drawinfo, CompositePlayerDrawContext.BackArm, new DrawData(value, val3, drawinfo.compBackArmFrame, drawinfo.colorArmorBody, rotation, bodyVect, 1f, drawinfo.playerEffect)
				{
					shader = drawinfo.cBody
				}, drawinfo.drawPlayer.body);
				if (drawinfo.drawPlayer.body == 71)
				{
					Texture2D value3 = TextureAssets.Extra[277].Value;
					DrawCompositeArmorPiece(ref drawinfo, CompositePlayerDrawContext.BackArm, new DrawData(value3, val3, drawinfo.compBackArmFrame, drawinfo.colorArmorBody, rotation, bodyVect, 1f, drawinfo.playerEffect)
					{
						shader = 0
					}, drawinfo.drawPlayer.body);
				}
			}
		}
		if (flag)
		{
			if (flag5)
			{
				if (flag2)
				{
					drawinfo.DrawDataCache.Add(new DrawData(TextureAssets.Players[drawinfo.skinVar, 7].Value, val3, drawinfo.compBackArmFrame, drawinfo.colorBodySkin, rotation, bodyVect, 1f, drawinfo.playerEffect)
					{
						shader = drawinfo.skinDyePacked
					});
				}
				if (!flag6 & flag5)
				{
					drawinfo.DrawDataCache.Add(new DrawData(TextureAssets.Players[drawinfo.skinVar, 5].Value, val3, drawinfo.compBackArmFrame, drawinfo.colorBodySkin, rotation, bodyVect, 1f, drawinfo.playerEffect)
					{
						shader = drawinfo.skinDyePacked
					});
					flag6 = true;
				}
			}
			if (!flag3)
			{
				drawinfo.DrawDataCache.Add(new DrawData(TextureAssets.Players[drawinfo.skinVar, 8].Value, val3, drawinfo.compBackArmFrame, drawinfo.colorUnderShirt, rotation, bodyVect, 1f, drawinfo.playerEffect));
				DrawPlayer_12_1_BalloonFronts(ref drawinfo);
				drawinfo.DrawDataCache.Add(new DrawData(TextureAssets.Players[drawinfo.skinVar, 13].Value, val3, drawinfo.compBackArmFrame, drawinfo.colorShirt, rotation, bodyVect, 1f, drawinfo.playerEffect));
			}
		}
		if (flag4 && (!drawinfo.drawPlayer.invis || IsArmorDrawnWhenInvisible(drawinfo.drawPlayer.coat)))
		{
			Texture2D value4 = TextureAssets.ArmorBodyComposite[drawinfo.drawPlayer.coat].Value;
			if (!drawinfo.hideCompositeShoulders)
			{
				DrawCompositeArmorPiece(ref drawinfo, CompositePlayerDrawContext.BackShoulder, new DrawData(value4, val4, drawinfo.compBackShoulderFrame, drawinfo.colorArmorBody, bodyRotation, drawinfo.bodyVect, 1f, drawinfo.playerEffect)
				{
					shader = drawinfo.cCoat
				}, drawinfo.drawPlayer.coat);
			}
			DrawCompositeArmorPiece(ref drawinfo, CompositePlayerDrawContext.BackArm, new DrawData(value4, val3, drawinfo.compBackArmFrame, drawinfo.colorArmorBody, rotation, bodyVect, 1f, drawinfo.playerEffect)
			{
				shader = drawinfo.cCoat
			}, drawinfo.drawPlayer.coat);
		}
		if (drawinfo.drawPlayer.handoff > 0 && drawinfo.drawPlayer.handoff < ArmorIDs.HandOff.Count)
		{
			Texture2D value5 = TextureAssets.AccHandsOffComposite[drawinfo.drawPlayer.handoff].Value;
			DrawCompositeArmorPiece(ref drawinfo, CompositePlayerDrawContext.BackArmAccessory, new DrawData(value5, val3, drawinfo.compBackArmFrame, drawinfo.colorArmorBody, rotation, bodyVect, 1f, drawinfo.playerEffect)
			{
				shader = drawinfo.cHandOff
			}, -1);
		}
		if (drawinfo.drawPlayer.drawingFootball)
		{
			Main.instance.LoadProjectile(861);
			Texture2D value6 = TextureAssets.Projectile[861].Value;
			Rectangle val5 = value6.Frame(1, 4);
			Vector2 origin = val5.Size() / 2f;
			Vector2 position = val3 + new Vector2((float)(drawinfo.drawPlayer.direction * -2), drawinfo.drawPlayer.gravDir * 4f);
			drawinfo.DrawDataCache.Add(new DrawData(value6, position, val5, drawinfo.colorArmorBody, bodyRotation + (float)Math.PI / 4f * (float)drawinfo.drawPlayer.direction, origin, 0.8f, drawinfo.playerEffect));
		}
	}

	public static void DrawPlayer_13_ArmorBackCoat(ref PlayerDrawSet drawinfo)
	{
		//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0102: Unknown result type (might be due to invalid IL or missing references)
		//IL_0107: Unknown result type (might be due to invalid IL or missing references)
		//IL_0112: Unknown result type (might be due to invalid IL or missing references)
		//IL_011d: Unknown result type (might be due to invalid IL or missing references)
		//IL_012e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0139: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		int matchingBodyExtensionBack = GetMatchingBodyExtensionBack(ref drawinfo, drawinfo.drawPlayer.coat);
		if (matchingBodyExtensionBack != -1)
		{
			Main.instance.LoadArmorLegs(matchingBodyExtensionBack);
			if (drawinfo.isSitting && !ArmorIDs.Legs.Sets.DoesNotSupportSittingDraw[matchingBodyExtensionBack])
			{
				DrawSittingLegs(ref drawinfo, TextureAssets.ArmorLeg[matchingBodyExtensionBack].Value, drawinfo.colorArmorBody, drawinfo.cCoat, matchingBodyExtensionBack, new Vector2(0f, drawinfo.seatYOffset));
				return;
			}
			DrawData cdd = new DrawData(TextureAssets.ArmorLeg[matchingBodyExtensionBack].Value, new Vector2((float)(int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.legFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.legFrame.Height + 4f)) + drawinfo.drawPlayer.legPosition + drawinfo.legVect, drawinfo.drawPlayer.legFrame, drawinfo.colorArmorBody, drawinfo.drawPlayer.legRotation, drawinfo.legVect, 1f, drawinfo.playerEffect);
			cdd.shader = drawinfo.cCoat;
			DrawLongCoat(ref drawinfo, ref cdd, matchingBodyExtensionBack);
		}
	}

	public static void DrawPlayer_13_Leggings(ref PlayerDrawSet drawinfo)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a7a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a85: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a8a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a90: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a95: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aa0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aab: Unknown result type (might be due to invalid IL or missing references)
		//IL_0abc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ac7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b73: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b7e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b83: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b89: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b8e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b99: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ba4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bb5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bc0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0233: Unknown result type (might be due to invalid IL or missing references)
		//IL_0246: Unknown result type (might be due to invalid IL or missing references)
		//IL_024c: Unknown result type (might be due to invalid IL or missing references)
		//IL_026c: Unknown result type (might be due to invalid IL or missing references)
		//IL_027f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0285: Unknown result type (might be due to invalid IL or missing references)
		//IL_054d: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_05cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_05db: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_05fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_060d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0618: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		//IL_0668: Unknown result type (might be due to invalid IL or missing references)
		//IL_06e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_06e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_06f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_06f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_06fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0701: Unknown result type (might be due to invalid IL or missing references)
		//IL_070c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0717: Unknown result type (might be due to invalid IL or missing references)
		//IL_0728: Unknown result type (might be due to invalid IL or missing references)
		//IL_0733: Unknown result type (might be due to invalid IL or missing references)
		//IL_033d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0101: Unknown result type (might be due to invalid IL or missing references)
		//IL_0399: Unknown result type (might be due to invalid IL or missing references)
		//IL_08da: Unknown result type (might be due to invalid IL or missing references)
		//IL_0953: Unknown result type (might be due to invalid IL or missing references)
		//IL_0958: Unknown result type (might be due to invalid IL or missing references)
		//IL_0963: Unknown result type (might be due to invalid IL or missing references)
		//IL_0968: Unknown result type (might be due to invalid IL or missing references)
		//IL_096e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0973: Unknown result type (might be due to invalid IL or missing references)
		//IL_097e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0989: Unknown result type (might be due to invalid IL or missing references)
		//IL_099a: Unknown result type (might be due to invalid IL or missing references)
		//IL_09a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_03fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_041e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0423: Unknown result type (might be due to invalid IL or missing references)
		//IL_0428: Unknown result type (might be due to invalid IL or missing references)
		//IL_042d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0438: Unknown result type (might be due to invalid IL or missing references)
		//IL_043d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0442: Unknown result type (might be due to invalid IL or missing references)
		//IL_01df: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0125: Unknown result type (might be due to invalid IL or missing references)
		//IL_012a: Unknown result type (might be due to invalid IL or missing references)
		//IL_045d: Unknown result type (might be due to invalid IL or missing references)
		//IL_045f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0460: Unknown result type (might be due to invalid IL or missing references)
		//IL_0465: Unknown result type (might be due to invalid IL or missing references)
		//IL_0467: Unknown result type (might be due to invalid IL or missing references)
		//IL_0469: Unknown result type (might be due to invalid IL or missing references)
		//IL_046e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0474: Unknown result type (might be due to invalid IL or missing references)
		//IL_0476: Unknown result type (might be due to invalid IL or missing references)
		//IL_047e: Unknown result type (might be due to invalid IL or missing references)
		//IL_048e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0490: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_04bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_04be: Unknown result type (might be due to invalid IL or missing references)
		//IL_07a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_07be: Unknown result type (might be due to invalid IL or missing references)
		//IL_07bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_07c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_083e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0843: Unknown result type (might be due to invalid IL or missing references)
		//IL_084e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0853: Unknown result type (might be due to invalid IL or missing references)
		//IL_0859: Unknown result type (might be due to invalid IL or missing references)
		//IL_085e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0869: Unknown result type (might be due to invalid IL or missing references)
		//IL_0874: Unknown result type (might be due to invalid IL or missing references)
		//IL_0885: Unknown result type (might be due to invalid IL or missing references)
		//IL_0890: Unknown result type (might be due to invalid IL or missing references)
		//IL_0160: Unknown result type (might be due to invalid IL or missing references)
		//IL_016c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0171: Unknown result type (might be due to invalid IL or missing references)
		//IL_0173: Unknown result type (might be due to invalid IL or missing references)
		//IL_0178: Unknown result type (might be due to invalid IL or missing references)
		//IL_0190: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b8: Unknown result type (might be due to invalid IL or missing references)
		Vector2 legsOffset = drawinfo.legsOffset;
		if (drawinfo.drawPlayer.legs == 169)
		{
			return;
		}
		if (drawinfo.isSitting && drawinfo.drawPlayer.legs != 140)
		{
			if (drawinfo.drawPlayer.legs > 0 && drawinfo.drawPlayer.legs < ArmorIDs.Legs.Count && (!ShouldOverrideLegs_CheckShoes(ref drawinfo) || drawinfo.drawPlayer.wearsRobe))
			{
				if (drawinfo.drawPlayer.invis)
				{
					return;
				}
				DrawSittingLegs(ref drawinfo, TextureAssets.ArmorLeg[drawinfo.drawPlayer.legs].Value, drawinfo.colorArmorLegs, drawinfo.cLegs, drawinfo.drawPlayer.legs);
				if (drawinfo.drawPlayer.legs == 60)
				{
					Texture2D value = TextureAssets.Extra[278].Value;
					DrawSittingLegs(ref drawinfo, value, drawinfo.colorArmorLegs, 0, drawinfo.drawPlayer.legs);
				}
				if (drawinfo.legsGlowMask == -1)
				{
					return;
				}
				if (drawinfo.legsGlowMask == 274)
				{
					Vector2 legsOffset2 = drawinfo.legsOffset;
					for (int i = 0; i < 2; i++)
					{
						Vector2 val = new Vector2((float)Main.rand.Next(-10, 10) * 0.125f, (float)Main.rand.Next(-10, 10) * 0.125f);
						ref Vector2 legsOffset3 = ref drawinfo.legsOffset;
						legsOffset3 += val;
						DrawSittingLegs(ref drawinfo, TextureAssets.GlowMask[drawinfo.legsGlowMask].Value, drawinfo.legsGlowColor, drawinfo.cLegs, drawinfo.drawPlayer.legs);
						drawinfo.legsOffset = legsOffset2;
					}
				}
				else
				{
					DrawSittingLegs(ref drawinfo, TextureAssets.GlowMask[drawinfo.legsGlowMask].Value, drawinfo.legsGlowColor, drawinfo.cLegs, drawinfo.drawPlayer.legs);
				}
			}
			else if (!drawinfo.drawPlayer.invis && !ShouldOverrideLegs_CheckShoes(ref drawinfo))
			{
				DrawSittingLegs(ref drawinfo, TextureAssets.Players[drawinfo.skinVar, 11].Value, drawinfo.colorPants, 0, drawinfo.drawPlayer.legs, default, skin: true);
				DrawSittingLegs(ref drawinfo, TextureAssets.Players[drawinfo.skinVar, 12].Value, drawinfo.colorShoes, 0, drawinfo.drawPlayer.legs, default, skin: true);
			}
		}
		else if (drawinfo.drawPlayer.legs == 140)
		{
			if (!drawinfo.drawPlayer.invis && !drawinfo.drawPlayer.mount.Active)
			{
				Texture2D value2 = TextureAssets.Extra[73].Value;
				bool flag = drawinfo.drawPlayer.legFrame.Y != drawinfo.drawPlayer.legFrame.Height || Main.gameMenu;
				int num = drawinfo.drawPlayer.miscCounter / 3 % 8;
				if (flag)
				{
					num = drawinfo.drawPlayer.miscCounter / 4 % 8;
				}
				Rectangle val2 = new Rectangle(18 * flag.ToInt(), num * 26, 16, 24);
				float num2 = 12f;
				if (drawinfo.drawPlayer.bodyFrame.Height != 0)
				{
					num2 = 12f - Main.OffsetsPlayerHeadgear[drawinfo.drawPlayer.bodyFrame.Y / drawinfo.drawPlayer.bodyFrame.Height].Y;
				}
				if (drawinfo.drawPlayer.Directions.Y == -1f)
				{
					num2 -= 6f;
				}
				Vector2 scale = new Vector2(1f, 1f);
				Vector2 val3 = drawinfo.Position + drawinfo.drawPlayer.Size * new Vector2(0.5f, 0.5f + 0.5f * drawinfo.drawPlayer.gravDir);
				_ = drawinfo.drawPlayer.direction;
				Vector2 val4 = val3 + new Vector2(0f, (0f - num2) * drawinfo.drawPlayer.gravDir) - Main.screenPosition + drawinfo.drawPlayer.legPosition;
				if (drawinfo.isSitting)
				{
					val4.Y += drawinfo.seatYOffset;
				}
				val4 += legsOffset;
				val4 = val4.Floor();
				DrawData item = new DrawData(value2, val4, val2, drawinfo.colorArmorLegs, drawinfo.drawPlayer.legRotation, val2.Size() * new Vector2(0.5f, 0.5f - drawinfo.drawPlayer.gravDir * 0.5f), scale, drawinfo.playerEffect);
				item.shader = drawinfo.cLegs;
				drawinfo.DrawDataCache.Add(item);
			}
		}
		else if (drawinfo.drawPlayer.legs > 0 && drawinfo.drawPlayer.legs < ArmorIDs.Legs.Count && (!ShouldOverrideLegs_CheckShoes(ref drawinfo) || drawinfo.drawPlayer.wearsRobe))
		{
			if (drawinfo.drawPlayer.invis)
			{
				return;
			}
			DrawData item = new DrawData(TextureAssets.ArmorLeg[drawinfo.drawPlayer.legs].Value, legsOffset + new Vector2((float)(int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.legFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.legFrame.Height + 4f)) + drawinfo.drawPlayer.legPosition + drawinfo.legVect, drawinfo.drawPlayer.legFrame, drawinfo.colorArmorLegs, drawinfo.drawPlayer.legRotation, drawinfo.legVect, 1f, drawinfo.playerEffect);
			item.shader = drawinfo.cLegs;
			drawinfo.DrawDataCache.Add(item);
			if (drawinfo.drawPlayer.legs == 60)
			{
				Texture2D value3 = TextureAssets.Extra[278].Value;
				item = new DrawData(value3, legsOffset + new Vector2((float)(int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.legFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.legFrame.Height + 4f)) + drawinfo.drawPlayer.legPosition + drawinfo.legVect, drawinfo.drawPlayer.legFrame, drawinfo.colorArmorLegs, drawinfo.drawPlayer.legRotation, drawinfo.legVect, 1f, drawinfo.playerEffect);
				item.shader = 0;
				drawinfo.DrawDataCache.Add(item);
			}
			if (drawinfo.legsGlowMask == -1)
			{
				return;
			}
			if (drawinfo.legsGlowMask == 274)
			{
				for (int j = 0; j < 2; j++)
				{
					item = new DrawData(position: legsOffset + new Vector2((float)Main.rand.Next(-10, 10) * 0.125f, (float)Main.rand.Next(-10, 10) * 0.125f) + new Vector2((float)(int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.legFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.legFrame.Height + 4f)) + drawinfo.drawPlayer.legPosition + drawinfo.legVect, texture: TextureAssets.GlowMask[drawinfo.legsGlowMask].Value, sourceRect: drawinfo.drawPlayer.legFrame, color: drawinfo.legsGlowColor, rotation: drawinfo.drawPlayer.legRotation, origin: drawinfo.legVect, scale: 1f, effect: drawinfo.playerEffect);
					item.shader = drawinfo.cLegs;
					drawinfo.DrawDataCache.Add(item);
				}
			}
			else
			{
				item = new DrawData(TextureAssets.GlowMask[drawinfo.legsGlowMask].Value, legsOffset + new Vector2((float)(int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.legFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.legFrame.Height + 4f)) + drawinfo.drawPlayer.legPosition + drawinfo.legVect, drawinfo.drawPlayer.legFrame, drawinfo.legsGlowColor, drawinfo.drawPlayer.legRotation, drawinfo.legVect, 1f, drawinfo.playerEffect);
				item.shader = drawinfo.cLegs;
				drawinfo.DrawDataCache.Add(item);
			}
		}
		else if (!drawinfo.drawPlayer.invis && !ShouldOverrideLegs_CheckShoes(ref drawinfo))
		{
			DrawData item = new DrawData(TextureAssets.Players[drawinfo.skinVar, 11].Value, new Vector2((float)(int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.legFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.legFrame.Height + 4f)) + drawinfo.drawPlayer.legPosition + drawinfo.legVect, drawinfo.drawPlayer.legFrame, drawinfo.colorPants, drawinfo.drawPlayer.legRotation, drawinfo.legVect, 1f, drawinfo.playerEffect);
			drawinfo.DrawDataCache.Add(item);
			item = new DrawData(TextureAssets.Players[drawinfo.skinVar, 12].Value, new Vector2((float)(int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.legFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.legFrame.Height + 4f)) + drawinfo.drawPlayer.legPosition + drawinfo.legVect, drawinfo.drawPlayer.legFrame, drawinfo.colorShoes, drawinfo.drawPlayer.legRotation, drawinfo.legVect, 1f, drawinfo.playerEffect);
			drawinfo.DrawDataCache.Add(item);
		}
	}

	private static void DrawSittingLegs(ref PlayerDrawSet drawinfo, Texture2D textureToDraw, Color matchingColor, int shaderIndex = 0, int legIndex = -1, Vector2 offset = default(Vector2), bool skin = false)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0441: Unknown result type (might be due to invalid IL or missing references)
		//IL_044a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0460: Unknown result type (might be due to invalid IL or missing references)
		//IL_0465: Unknown result type (might be due to invalid IL or missing references)
		//IL_046a: Unknown result type (might be due to invalid IL or missing references)
		//IL_046f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0471: Unknown result type (might be due to invalid IL or missing references)
		//IL_0472: Unknown result type (might be due to invalid IL or missing references)
		//IL_042c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0431: Unknown result type (might be due to invalid IL or missing references)
		//IL_026f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0274: Unknown result type (might be due to invalid IL or missing references)
		//IL_0377: Unknown result type (might be due to invalid IL or missing references)
		//IL_054f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0551: Unknown result type (might be due to invalid IL or missing references)
		//IL_0558: Unknown result type (might be due to invalid IL or missing references)
		//IL_0565: Unknown result type (might be due to invalid IL or missing references)
		//IL_0570: Unknown result type (might be due to invalid IL or missing references)
		//IL_0341: Unknown result type (might be due to invalid IL or missing references)
		//IL_0346: Unknown result type (might be due to invalid IL or missing references)
		//IL_024b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0250: Unknown result type (might be due to invalid IL or missing references)
		//IL_039d: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ba: Unknown result type (might be due to invalid IL or missing references)
		Vector2 legsOffset = drawinfo.legsOffset;
		Vector2 val = new Vector2((float)(int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.legFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.legFrame.Height + 4f)) + drawinfo.drawPlayer.legPosition + drawinfo.legVect;
		Rectangle legFrame = drawinfo.drawPlayer.legFrame;
		val.Y -= 2f;
		val.Y += drawinfo.seatYOffset;
		val += legsOffset;
		val += offset;
		int num = 2;
		int num2 = 42;
		int num3 = 2;
		int num4 = 2;
		int num5 = 0;
		int num6 = 0;
		int num7 = 0;
		int num8 = 0;
		int num9 = 0;
		bool flag = false;
		bool flag3 = false;
		if (legIndex == 169 || !skin)
		{
			switch (legIndex)
			{
			case 217:
				num = 0;
				num4 = 0;
				num5 = 0;
				num3 = 1;
				num2 = 0;
				legFrame = drawinfo.drawPlayer.legFrame;
				flag3 = true;
				break;
			case 149:
			case 171:
			case 172:
				num = -6;
				num4 = 2;
				num5 = 2;
				num3 = 4;
				num2 = 6;
				legFrame = drawinfo.drawPlayer.legFrame;
				val.Y += 6f;
				val.Y -= drawinfo.seatYOffset;
				break;
			case 169:
				if (skin)
				{
					num = -6;
					num4 = 2;
					num5 = 2;
					num3 = 4;
					num2 = 6;
					legFrame = drawinfo.drawPlayer.legFrame;
					val.Y += 6f;
				}
				else
				{
					num = 0;
					num4 = 0;
					num5 = 0;
					num3 = 1;
					num2 = 0;
					legFrame = drawinfo.drawPlayer.legFrame;
					val.Y -= drawinfo.seatYOffset;
					flag = true;
				}
				break;
			case 238:
			case 239:
				num = 2;
				num4 = 2;
				num5 = -2;
				num2 = 42;
				val.Y -= drawinfo.seatYOffset;
				flag = true;
				break;
			case 214:
			case 215:
			case 216:
				num = -6;
				num4 = 2;
				num5 = 2;
				num3 = 4;
				num2 = 6;
				legFrame = drawinfo.drawPlayer.legFrame;
				val.Y += 6f;
				break;
			case 106:
			case 143:
			case 226:
				num = 0;
				num4 = 0;
				num2 = 6;
				val.Y += 4f;
				legFrame.Y = legFrame.Height * 5;
				break;
			case 222:
				val.X -= 2f * drawinfo.drawPlayer.Directions.X;
				break;
			case 223:
				val.X -= 2f * drawinfo.drawPlayer.Directions.X;
				val.Y -= drawinfo.seatYOffset;
				break;
			case 132:
				num = -2;
				num7 = 2;
				break;
			case 193:
			case 194:
				if (drawinfo.drawPlayer.body == 218)
				{
					num = -2;
					num7 = 2;
					val.Y += 2f;
				}
				break;
			case 177:
			case 178:
			case 181:
			case 182:
			case 200:
			case 201:
			case 206:
				num = 0;
				num4 = 0;
				num5 = 0;
				num3 = 1;
				num2 = 0;
				legFrame = drawinfo.drawPlayer.legFrame;
				num8 = 4;
				num9 = 6;
				break;
			}
		}
		for (int num10 = num3; num10 >= 0; num10--)
		{
			Vector2 position = val + new Vector2((float)num, 2f) * new Vector2((float)drawinfo.drawPlayer.direction, 1f);
			Rectangle value = legFrame;
			if (!flag3)
			{
				value.Y += num10 * 2;
				value.Y += num2;
				value.Height -= num2;
				value.Height -= num10 * 2;
				if (num10 != num3)
				{
					value.Height = 2;
				}
			}
			position.X += drawinfo.drawPlayer.direction * num4 * num10 + num6 * drawinfo.drawPlayer.direction;
			if (num10 != 0)
			{
				position.X += num7 * drawinfo.drawPlayer.direction;
			}
			position.Y += num2;
			position.Y += num5;
			position.X += num8 * drawinfo.drawPlayer.direction;
			position.Y += num9;
			DrawData cdd = new DrawData(textureToDraw, position, value, matchingColor, drawinfo.drawPlayer.legRotation, drawinfo.legVect, 1f, drawinfo.playerEffect);
			cdd.shader = shaderIndex;
			if (flag)
			{
				DrawLongCoat(ref drawinfo, ref cdd, legIndex);
			}
			else
			{
				drawinfo.DrawDataCache.Add(cdd);
			}
		}
	}

	public static void DrawPlayer_14_Shoes(ref PlayerDrawSet drawinfo)
	{
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_0133: Unknown result type (might be due to invalid IL or missing references)
		//IL_0138: Unknown result type (might be due to invalid IL or missing references)
		//IL_0143: Unknown result type (might be due to invalid IL or missing references)
		//IL_0148: Unknown result type (might be due to invalid IL or missing references)
		//IL_014e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0153: Unknown result type (might be due to invalid IL or missing references)
		//IL_015e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0169: Unknown result type (might be due to invalid IL or missing references)
		//IL_017a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0185: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		if (drawinfo.drawPlayer.shoe > 0 && drawinfo.drawPlayer.shoe < ArmorIDs.Shoe.Count && !ShouldOverrideLegs_CheckPants(ref drawinfo))
		{
			Vector2 shoeDrawOffset = drawinfo.drawPlayer.GetShoeDrawOffset();
			int num = drawinfo.cShoe;
			if (drawinfo.drawPlayer.shoe == 22 || drawinfo.drawPlayer.shoe == 23)
			{
				num = drawinfo.cFlameWaker;
			}
			if (drawinfo.isSitting)
			{
				DrawSittingLegs(ref drawinfo, TextureAssets.AccShoes[drawinfo.drawPlayer.shoe].Value, drawinfo.colorArmorLegs, num, drawinfo.drawPlayer.legs, shoeDrawOffset);
				return;
			}
			DrawData item = new DrawData(TextureAssets.AccShoes[drawinfo.drawPlayer.shoe].Value, shoeDrawOffset + new Vector2((float)(int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.legFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.legFrame.Height + 4f)) + drawinfo.drawPlayer.legPosition + drawinfo.legVect, drawinfo.drawPlayer.legFrame, drawinfo.colorArmorLegs, drawinfo.drawPlayer.legRotation, drawinfo.legVect, 1f, drawinfo.playerEffect);
			item.shader = num;
			drawinfo.DrawDataCache.Add(item);
		}
	}

	public static void DrawPlayer_15_SkinLongCoat(ref PlayerDrawSet drawinfo)
	{
		//IL_0129: Unknown result type (might be due to invalid IL or missing references)
		//IL_0134: Unknown result type (might be due to invalid IL or missing references)
		//IL_0139: Unknown result type (might be due to invalid IL or missing references)
		//IL_013f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0144: Unknown result type (might be due to invalid IL or missing references)
		//IL_014f: Unknown result type (might be due to invalid IL or missing references)
		//IL_015a: Unknown result type (might be due to invalid IL or missing references)
		//IL_016b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0176: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		if ((drawinfo.skinVar == 3 || drawinfo.skinVar == 8 || drawinfo.skinVar == 7) && (drawinfo.drawPlayer.body <= 0 || drawinfo.drawPlayer.body >= ArmorIDs.Body.Count) && !drawinfo.drawPlayer.invis)
		{
			if (drawinfo.isSitting)
			{
				DrawSittingLegs(ref drawinfo, TextureAssets.Players[drawinfo.skinVar, 14].Value, drawinfo.colorShirt, 0, drawinfo.drawPlayer.legs, default, skin: true);
				return;
			}
			DrawData item = new DrawData(TextureAssets.Players[drawinfo.skinVar, 14].Value, new Vector2((float)(int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.legFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.legFrame.Height + 4f)) + drawinfo.drawPlayer.legPosition + drawinfo.legVect, drawinfo.drawPlayer.legFrame, drawinfo.colorShirt, drawinfo.drawPlayer.legRotation, drawinfo.legVect, 1f, drawinfo.playerEffect);
			drawinfo.DrawDataCache.Add(item);
		}
	}

	public static void DrawPlayer_16_ArmorLongCoat(ref PlayerDrawSet drawinfo)
	{
		//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0104: Unknown result type (might be due to invalid IL or missing references)
		//IL_010f: Unknown result type (might be due to invalid IL or missing references)
		//IL_011a: Unknown result type (might be due to invalid IL or missing references)
		//IL_012b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0136: Unknown result type (might be due to invalid IL or missing references)
		//IL_0240: Unknown result type (might be due to invalid IL or missing references)
		//IL_024b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0250: Unknown result type (might be due to invalid IL or missing references)
		//IL_0256: Unknown result type (might be due to invalid IL or missing references)
		//IL_025b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0266: Unknown result type (might be due to invalid IL or missing references)
		//IL_0271: Unknown result type (might be due to invalid IL or missing references)
		//IL_0282: Unknown result type (might be due to invalid IL or missing references)
		//IL_028d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_019e: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b2: Unknown result type (might be due to invalid IL or missing references)
		int matchingBodyExtension = GetMatchingBodyExtension(ref drawinfo, drawinfo.drawPlayer.body);
		if (matchingBodyExtension != -1)
		{
			Main.instance.LoadArmorLegs(matchingBodyExtension);
			if (drawinfo.isSitting && !ArmorIDs.Legs.Sets.DoesNotSupportSittingDraw[matchingBodyExtension])
			{
				DrawSittingLegs(ref drawinfo, TextureAssets.ArmorLeg[matchingBodyExtension].Value, drawinfo.colorArmorBody, drawinfo.cBody, matchingBodyExtension);
			}
			else
			{
				DrawData cdd = new DrawData(TextureAssets.ArmorLeg[matchingBodyExtension].Value, new Vector2((float)(int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.legFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.legFrame.Height + 4f)) + drawinfo.drawPlayer.legPosition + drawinfo.legVect, drawinfo.drawPlayer.legFrame, drawinfo.colorArmorBody, drawinfo.drawPlayer.legRotation, drawinfo.legVect, 1f, drawinfo.playerEffect);
				cdd.shader = drawinfo.cBody;
				DrawLongCoat(ref drawinfo, ref cdd, matchingBodyExtension);
			}
		}
		int matchingBodyExtension2 = GetMatchingBodyExtension(ref drawinfo, drawinfo.drawPlayer.coat);
		if (matchingBodyExtension2 != -1)
		{
			Main.instance.LoadArmorLegs(matchingBodyExtension2);
			if (drawinfo.isSitting && !ArmorIDs.Legs.Sets.DoesNotSupportSittingDraw[matchingBodyExtension2])
			{
				DrawSittingLegs(ref drawinfo, TextureAssets.ArmorLeg[matchingBodyExtension2].Value, drawinfo.colorArmorBody, drawinfo.cCoat, matchingBodyExtension2);
				return;
			}
			DrawData cdd = new DrawData(TextureAssets.ArmorLeg[matchingBodyExtension2].Value, new Vector2((float)(int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.legFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.legFrame.Height + 4f)) + drawinfo.drawPlayer.legPosition + drawinfo.legVect, drawinfo.drawPlayer.legFrame, drawinfo.colorArmorBody, drawinfo.drawPlayer.legRotation, drawinfo.legVect, 1f, drawinfo.playerEffect);
			cdd.shader = drawinfo.cCoat;
			DrawLongCoat(ref drawinfo, ref cdd, matchingBodyExtension2);
		}
	}

	private static void DrawLongCoat(ref PlayerDrawSet drawinfo, ref DrawData cdd, int specialLegCoat)
	{
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		drawinfo.DrawDataCache.Add(cdd);
		if (specialLegCoat == 238)
		{
			DrawData item = cdd;
			item.texture = TextureAssets.GlowMask[363].Value;
			item.color = GetChickenBonesGlowColor(ref drawinfo, scaleByShadow: true);
			float num = drawinfo.stealth * drawinfo.stealth;
			num *= 1f - drawinfo.shadow;
			item.color = Color.Multiply(item.color, num);
			drawinfo.DrawDataCache.Add(item);
		}
	}

	public static int GetMatchingBodyExtensionBack(ref PlayerDrawSet drawinfo, int bodyValue)
	{
		int result = -1;
		if (bodyValue == 251)
		{
			result = 239;
		}
		return result;
	}

	public static int GetMatchingBodyExtension(ref PlayerDrawSet drawinfo, int bodyValue)
	{
		int result = -1;
		switch (bodyValue)
		{
		case 200:
			result = 149;
			break;
		case 202:
			result = 151;
			break;
		case 201:
			result = 150;
			break;
		case 209:
			result = 160;
			break;
		case 207:
			result = 161;
			break;
		case 198:
			result = 162;
			break;
		case 182:
			result = 163;
			break;
		case 168:
			result = 164;
			break;
		case 73:
			result = 170;
			break;
		case 52:
			result = ((!drawinfo.drawPlayer.Male) ? 172 : 171);
			break;
		case 187:
			result = 173;
			break;
		case 205:
			result = 174;
			break;
		case 53:
			result = ((!drawinfo.drawPlayer.Male) ? 176 : 175);
			break;
		case 210:
			result = ((!drawinfo.drawPlayer.Male) ? 177 : 178);
			break;
		case 211:
			result = ((!drawinfo.drawPlayer.Male) ? 181 : 182);
			break;
		case 218:
			result = 195;
			break;
		case 222:
			result = ((!drawinfo.drawPlayer.Male) ? 200 : 201);
			break;
		case 225:
			result = 206;
			break;
		case 236:
			result = 221;
			break;
		case 237:
			result = 223;
			break;
		case 89:
			result = 186;
			break;
		case 81:
			result = 169;
			break;
		case 251:
			result = 238;
			break;
		}
		return result;
	}

	public static void DrawPlayer_17_Torso(ref PlayerDrawSet drawinfo)
	{
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_074c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0757: Unknown result type (might be due to invalid IL or missing references)
		//IL_075c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0787: Unknown result type (might be due to invalid IL or missing references)
		//IL_078c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0797: Unknown result type (might be due to invalid IL or missing references)
		//IL_07a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_07b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_07be: Unknown result type (might be due to invalid IL or missing references)
		//IL_0869: Unknown result type (might be due to invalid IL or missing references)
		//IL_0874: Unknown result type (might be due to invalid IL or missing references)
		//IL_0879: Unknown result type (might be due to invalid IL or missing references)
		//IL_08a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_08a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_08b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_08bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_08d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_08db: Unknown result type (might be due to invalid IL or missing references)
		//IL_050d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0518: Unknown result type (might be due to invalid IL or missing references)
		//IL_051d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0548: Unknown result type (might be due to invalid IL or missing references)
		//IL_054d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0558: Unknown result type (might be due to invalid IL or missing references)
		//IL_0563: Unknown result type (might be due to invalid IL or missing references)
		//IL_0574: Unknown result type (might be due to invalid IL or missing references)
		//IL_057f: Unknown result type (might be due to invalid IL or missing references)
		//IL_062a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0635: Unknown result type (might be due to invalid IL or missing references)
		//IL_063a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0665: Unknown result type (might be due to invalid IL or missing references)
		//IL_066a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0675: Unknown result type (might be due to invalid IL or missing references)
		//IL_0680: Unknown result type (might be due to invalid IL or missing references)
		//IL_0691: Unknown result type (might be due to invalid IL or missing references)
		//IL_069c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0986: Unknown result type (might be due to invalid IL or missing references)
		//IL_0991: Unknown result type (might be due to invalid IL or missing references)
		//IL_0996: Unknown result type (might be due to invalid IL or missing references)
		//IL_09c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_09c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_09d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_09dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_09ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_09f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_015b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0166: Unknown result type (might be due to invalid IL or missing references)
		//IL_016b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0196: Unknown result type (might be due to invalid IL or missing references)
		//IL_019b: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0284: Unknown result type (might be due to invalid IL or missing references)
		//IL_028f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0294: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_03bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_03cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_03fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_040a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0415: Unknown result type (might be due to invalid IL or missing references)
		//IL_0426: Unknown result type (might be due to invalid IL or missing references)
		//IL_0431: Unknown result type (might be due to invalid IL or missing references)
		if (drawinfo.usesCompositeTorso)
		{
			DrawPlayer_17_TorsoComposite(ref drawinfo);
		}
		else if (drawinfo.drawPlayer.body > 0 && drawinfo.drawPlayer.body < ArmorIDs.Body.Count)
		{
			Rectangle bodyFrame = drawinfo.drawPlayer.bodyFrame;
			int num = drawinfo.armorAdjust;
			bodyFrame.X += num;
			bodyFrame.Width -= num;
			if (drawinfo.drawPlayer.direction == -1)
			{
				num = 0;
			}
			if (!drawinfo.drawPlayer.invis || (drawinfo.drawPlayer.body != 21 && drawinfo.drawPlayer.body != 22))
			{
				Texture2D texture = (drawinfo.drawPlayer.Male ? TextureAssets.ArmorBody[drawinfo.drawPlayer.body].Value : TextureAssets.FemaleBody[drawinfo.drawPlayer.body].Value);
				DrawData item = new DrawData(texture, new Vector2((float)((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)) + num), (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.bodyPosition + new Vector2((float)(drawinfo.drawPlayer.bodyFrame.Width / 2), (float)(drawinfo.drawPlayer.bodyFrame.Height / 2)), bodyFrame, drawinfo.colorArmorBody, drawinfo.drawPlayer.bodyRotation, drawinfo.bodyVect, 1f, drawinfo.playerEffect);
				item.shader = drawinfo.cBody;
				drawinfo.DrawDataCache.Add(item);
				if (drawinfo.bodyGlowMask != -1)
				{
					item = new DrawData(TextureAssets.GlowMask[drawinfo.bodyGlowMask].Value, new Vector2((float)((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)) + num), (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.bodyPosition + new Vector2((float)(drawinfo.drawPlayer.bodyFrame.Width / 2), (float)(drawinfo.drawPlayer.bodyFrame.Height / 2)), bodyFrame, drawinfo.bodyGlowColor, drawinfo.drawPlayer.bodyRotation, drawinfo.bodyVect, 1f, drawinfo.playerEffect);
					item.shader = drawinfo.cBody;
					drawinfo.DrawDataCache.Add(item);
				}
			}
			if (drawinfo.missingHand && !drawinfo.drawPlayer.invis)
			{
				DrawData drawData = new DrawData(TextureAssets.Players[drawinfo.skinVar, 5].Value, new Vector2((float)(int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.bodyPosition + new Vector2((float)(drawinfo.drawPlayer.bodyFrame.Width / 2), (float)(drawinfo.drawPlayer.bodyFrame.Height / 2)), drawinfo.drawPlayer.bodyFrame, drawinfo.colorBodySkin, drawinfo.drawPlayer.bodyRotation, drawinfo.bodyVect, 1f, drawinfo.playerEffect);
				drawData.shader = drawinfo.skinDyePacked;
				DrawData item = drawData;
				drawinfo.DrawDataCache.Add(item);
			}
		}
		else if (!drawinfo.drawPlayer.invis)
		{
			DrawData item;
			if (!drawinfo.drawPlayer.Male)
			{
				item = new DrawData(TextureAssets.Players[drawinfo.skinVar, 4].Value, new Vector2((float)(int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.bodyPosition + new Vector2((float)(drawinfo.drawPlayer.bodyFrame.Width / 2), (float)(drawinfo.drawPlayer.bodyFrame.Height / 2)), drawinfo.drawPlayer.bodyFrame, drawinfo.colorUnderShirt, drawinfo.drawPlayer.bodyRotation, drawinfo.bodyVect, 1f, drawinfo.playerEffect);
				drawinfo.DrawDataCache.Add(item);
				item = new DrawData(TextureAssets.Players[drawinfo.skinVar, 6].Value, new Vector2((float)(int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.bodyPosition + new Vector2((float)(drawinfo.drawPlayer.bodyFrame.Width / 2), (float)(drawinfo.drawPlayer.bodyFrame.Height / 2)), drawinfo.drawPlayer.bodyFrame, drawinfo.colorShirt, drawinfo.drawPlayer.bodyRotation, drawinfo.bodyVect, 1f, drawinfo.playerEffect);
				drawinfo.DrawDataCache.Add(item);
			}
			else
			{
				item = new DrawData(TextureAssets.Players[drawinfo.skinVar, 4].Value, new Vector2((float)(int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.bodyPosition + new Vector2((float)(drawinfo.drawPlayer.bodyFrame.Width / 2), (float)(drawinfo.drawPlayer.bodyFrame.Height / 2)), drawinfo.drawPlayer.bodyFrame, drawinfo.colorUnderShirt, drawinfo.drawPlayer.bodyRotation, drawinfo.bodyVect, 1f, drawinfo.playerEffect);
				drawinfo.DrawDataCache.Add(item);
				item = new DrawData(TextureAssets.Players[drawinfo.skinVar, 6].Value, new Vector2((float)(int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.bodyPosition + new Vector2((float)(drawinfo.drawPlayer.bodyFrame.Width / 2), (float)(drawinfo.drawPlayer.bodyFrame.Height / 2)), drawinfo.drawPlayer.bodyFrame, drawinfo.colorShirt, drawinfo.drawPlayer.bodyRotation, drawinfo.bodyVect, 1f, drawinfo.playerEffect);
				drawinfo.DrawDataCache.Add(item);
			}
			DrawData drawData = new DrawData(TextureAssets.Players[drawinfo.skinVar, 5].Value, new Vector2((float)(int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.bodyPosition + new Vector2((float)(drawinfo.drawPlayer.bodyFrame.Width / 2), (float)(drawinfo.drawPlayer.bodyFrame.Height / 2)), drawinfo.drawPlayer.bodyFrame, drawinfo.colorBodySkin, drawinfo.drawPlayer.bodyRotation, drawinfo.bodyVect, 1f, drawinfo.playerEffect);
			drawData.shader = drawinfo.skinDyePacked;
			item = drawData;
			drawinfo.DrawDataCache.Add(item);
		}
	}

	public static void DrawPlayer_17_TorsoComposite(ref PlayerDrawSet drawinfo)
	{
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		//IL_0105: Invalid comparison between Unknown and I4
		//IL_010e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0113: Unknown result type (might be due to invalid IL or missing references)
		//IL_0118: Unknown result type (might be due to invalid IL or missing references)
		//IL_0125: Unknown result type (might be due to invalid IL or missing references)
		//IL_0127: Unknown result type (might be due to invalid IL or missing references)
		//IL_012c: Unknown result type (might be due to invalid IL or missing references)
		//IL_012e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0133: Unknown result type (might be due to invalid IL or missing references)
		//IL_0135: Unknown result type (might be due to invalid IL or missing references)
		//IL_0137: Unknown result type (might be due to invalid IL or missing references)
		//IL_013d: Unknown result type (might be due to invalid IL or missing references)
		//IL_013e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0140: Unknown result type (might be due to invalid IL or missing references)
		//IL_0145: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0304: Unknown result type (might be due to invalid IL or missing references)
		//IL_030f: Unknown result type (might be due to invalid IL or missing references)
		//IL_033f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0341: Unknown result type (might be due to invalid IL or missing references)
		//IL_034c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0353: Unknown result type (might be due to invalid IL or missing references)
		//IL_035e: Unknown result type (might be due to invalid IL or missing references)
		//IL_038e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0390: Unknown result type (might be due to invalid IL or missing references)
		//IL_039b: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_048e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0496: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0226: Unknown result type (might be due to invalid IL or missing references)
		//IL_0228: Unknown result type (might be due to invalid IL or missing references)
		//IL_0233: Unknown result type (might be due to invalid IL or missing references)
		//IL_023a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0245: Unknown result type (might be due to invalid IL or missing references)
		//IL_0424: Unknown result type (might be due to invalid IL or missing references)
		//IL_0426: Unknown result type (might be due to invalid IL or missing references)
		//IL_0431: Unknown result type (might be due to invalid IL or missing references)
		//IL_0438: Unknown result type (might be due to invalid IL or missing references)
		//IL_0443: Unknown result type (might be due to invalid IL or missing references)
		Vector2 val = new Vector2((float)(int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.bodyPosition + new Vector2((float)(drawinfo.drawPlayer.bodyFrame.Width / 2), (float)(drawinfo.drawPlayer.bodyFrame.Height / 2));
		Vector2 val2 = Main.OffsetsPlayerHeadgear[drawinfo.drawPlayer.bodyFrame.Y / drawinfo.drawPlayer.bodyFrame.Height];
		val2.Y -= 2f;
		val += val2 * (float)(-(((int)drawinfo.playerEffect & 2) > 0).ToDirectionInt());
		float bodyRotation = drawinfo.drawPlayer.bodyRotation;
		Vector2 val3 = val;
		Vector2 bodyVect = drawinfo.bodyVect;
		Vector2 compositeOffset_BackArm = GetCompositeOffset_BackArm(ref drawinfo);
		_ = val3 + compositeOffset_BackArm;
		bodyVect += compositeOffset_BackArm;
		bool flag = false;
		if (drawinfo.drawPlayer.body > 0 && drawinfo.drawPlayer.body < ArmorIDs.Body.Count)
		{
			flag = true;
			if (!drawinfo.drawPlayer.invis || IsArmorDrawnWhenInvisible(drawinfo.drawPlayer.body))
			{
				Texture2D value = TextureAssets.ArmorBodyComposite[drawinfo.drawPlayer.body].Value;
				DrawCompositeArmorPiece(ref drawinfo, CompositePlayerDrawContext.Torso, new DrawData(value, val, drawinfo.compTorsoFrame, drawinfo.colorArmorBody, bodyRotation, drawinfo.bodyVect, 1f, drawinfo.playerEffect)
				{
					shader = drawinfo.cBody
				}, drawinfo.drawPlayer.body);
				if (drawinfo.drawPlayer.body == 71)
				{
					Texture2D value2 = TextureAssets.Extra[277].Value;
					DrawCompositeArmorPiece(ref drawinfo, CompositePlayerDrawContext.Torso, new DrawData(value2, val, drawinfo.compTorsoFrame, drawinfo.colorArmorBody, bodyRotation, drawinfo.bodyVect, 1f, drawinfo.playerEffect)
					{
						shader = 0
					}, drawinfo.drawPlayer.body);
				}
			}
		}
		if (!flag && !drawinfo.drawPlayer.invis)
		{
			drawinfo.DrawDataCache.Add(new DrawData(TextureAssets.Players[drawinfo.skinVar, 4].Value, val, drawinfo.compBackShoulderFrame, drawinfo.colorUnderShirt, bodyRotation, drawinfo.bodyVect, 1f, drawinfo.playerEffect));
			drawinfo.DrawDataCache.Add(new DrawData(TextureAssets.Players[drawinfo.skinVar, 6].Value, val, drawinfo.compBackShoulderFrame, drawinfo.colorShirt, bodyRotation, drawinfo.bodyVect, 1f, drawinfo.playerEffect));
			drawinfo.DrawDataCache.Add(new DrawData(TextureAssets.Players[drawinfo.skinVar, 4].Value, val, drawinfo.compTorsoFrame, drawinfo.colorUnderShirt, bodyRotation, drawinfo.bodyVect, 1f, drawinfo.playerEffect));
			drawinfo.DrawDataCache.Add(new DrawData(TextureAssets.Players[drawinfo.skinVar, 6].Value, val, drawinfo.compTorsoFrame, drawinfo.colorShirt, bodyRotation, drawinfo.bodyVect, 1f, drawinfo.playerEffect));
		}
		if (drawinfo.drawPlayer.coat > 0 && drawinfo.drawPlayer.coat < ArmorIDs.Body.Count && (!drawinfo.drawPlayer.invis || IsArmorDrawnWhenInvisible(drawinfo.drawPlayer.coat)))
		{
			Texture2D value3 = TextureAssets.ArmorBodyComposite[drawinfo.drawPlayer.coat].Value;
			DrawCompositeArmorPiece(ref drawinfo, CompositePlayerDrawContext.Torso, new DrawData(value3, val, drawinfo.compTorsoFrame, drawinfo.colorArmorBody, bodyRotation, drawinfo.bodyVect, 1f, drawinfo.playerEffect)
			{
				shader = drawinfo.cCoat
			}, drawinfo.drawPlayer.coat);
		}
		if (drawinfo.drawFloatingTube)
		{
			drawinfo.DrawDataCache.Add(new DrawData(TextureAssets.Extra[105].Value, val, new Rectangle(0, 56, 40, 56), drawinfo.floatingTubeColor, bodyRotation, drawinfo.bodyVect, 1f, drawinfo.playerEffect)
			{
				shader = drawinfo.cFloatingTube
			});
		}
	}

	public static void DrawPlayer_18_OffhandAcc(ref PlayerDrawSet drawinfo)
	{
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_010a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0115: Unknown result type (might be due to invalid IL or missing references)
		//IL_0126: Unknown result type (might be due to invalid IL or missing references)
		//IL_0131: Unknown result type (might be due to invalid IL or missing references)
		if (!drawinfo.usesCompositeBackHandAcc && drawinfo.drawPlayer.handoff > 0 && drawinfo.drawPlayer.handoff < ArmorIDs.HandOff.Count)
		{
			DrawData item = new DrawData(TextureAssets.AccHandsOff[drawinfo.drawPlayer.handoff].Value, new Vector2((float)(int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.bodyPosition + new Vector2((float)(drawinfo.drawPlayer.bodyFrame.Width / 2), (float)(drawinfo.drawPlayer.bodyFrame.Height / 2)), drawinfo.drawPlayer.bodyFrame, drawinfo.colorArmorBody, drawinfo.drawPlayer.bodyRotation, drawinfo.bodyVect, 1f, drawinfo.playerEffect);
			item.shader = drawinfo.cHandOff;
			drawinfo.DrawDataCache.Add(item);
		}
	}

	public static void DrawPlayer_JimsDroneRadio(ref PlayerDrawSet drawinfo)
	{
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0128: Unknown result type (might be due to invalid IL or missing references)
		//IL_012d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0132: Unknown result type (might be due to invalid IL or missing references)
		//IL_0139: Unknown result type (might be due to invalid IL or missing references)
		//IL_014a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0155: Unknown result type (might be due to invalid IL or missing references)
		if ((drawinfo.drawPlayer.HeldItem.type == 5451 || drawinfo.drawPlayer.HeldItem.type == 5738) && drawinfo.drawPlayer.itemAnimation == 0)
		{
			Rectangle bodyFrame = drawinfo.drawPlayer.bodyFrame;
			Texture2D value = TextureAssets.Extra[261].Value;
			DrawData item = new DrawData(value, new Vector2((float)((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)) + drawinfo.drawPlayer.direction * 2), (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f + 14f)) + drawinfo.drawPlayer.bodyPosition + new Vector2((float)(drawinfo.drawPlayer.bodyFrame.Width / 2), (float)(drawinfo.drawPlayer.bodyFrame.Height / 2)), bodyFrame, drawinfo.colorArmorLegs, drawinfo.drawPlayer.legRotation, drawinfo.legVect, 1f, drawinfo.playerEffect);
			item.shader = drawinfo.cWaist;
			drawinfo.DrawDataCache.Add(item);
		}
	}

	public static void DrawPlayer_19_WaistAcc(ref PlayerDrawSet drawinfo)
	{
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0101: Unknown result type (might be due to invalid IL or missing references)
		//IL_0108: Unknown result type (might be due to invalid IL or missing references)
		//IL_0119: Unknown result type (might be due to invalid IL or missing references)
		//IL_0124: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		if (drawinfo.drawPlayer.waist > 0 && drawinfo.drawPlayer.waist < ArmorIDs.Waist.Count)
		{
			Rectangle value = drawinfo.drawPlayer.legFrame;
			if (ArmorIDs.Waist.Sets.UsesTorsoFraming[drawinfo.drawPlayer.waist])
			{
				value = drawinfo.drawPlayer.bodyFrame;
			}
			DrawData item = new DrawData(TextureAssets.AccWaist[drawinfo.drawPlayer.waist].Value, new Vector2((float)(int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.legFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.legFrame.Height + 4f)) + drawinfo.drawPlayer.legPosition + drawinfo.legVect, value, drawinfo.colorArmorLegs, drawinfo.drawPlayer.legRotation, drawinfo.legVect, 1f, drawinfo.playerEffect);
			item.shader = drawinfo.cWaist;
			drawinfo.DrawDataCache.Add(item);
		}
	}

	public static void DrawPlayer_20_NeckAcc(ref PlayerDrawSet drawinfo)
	{
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0101: Unknown result type (might be due to invalid IL or missing references)
		//IL_010c: Unknown result type (might be due to invalid IL or missing references)
		//IL_011d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0128: Unknown result type (might be due to invalid IL or missing references)
		if (drawinfo.drawPlayer.neck > 0 && drawinfo.drawPlayer.neck < ArmorIDs.Neck.Count)
		{
			DrawData item = new DrawData(TextureAssets.AccNeck[drawinfo.drawPlayer.neck].Value, new Vector2((float)(int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.bodyPosition + new Vector2((float)(drawinfo.drawPlayer.bodyFrame.Width / 2), (float)(drawinfo.drawPlayer.bodyFrame.Height / 2)), drawinfo.drawPlayer.bodyFrame, drawinfo.colorArmorBody, drawinfo.drawPlayer.bodyRotation, drawinfo.bodyVect, 1f, drawinfo.playerEffect);
			item.shader = drawinfo.cNeck;
			drawinfo.DrawDataCache.Add(item);
		}
	}

	public static void DrawPlayer_21_Head(ref PlayerDrawSet drawinfo)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0166: Unknown result type (might be due to invalid IL or missing references)
		//IL_016c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0171: Unknown result type (might be due to invalid IL or missing references)
		//IL_0176: Unknown result type (might be due to invalid IL or missing references)
		//IL_017b: Unknown result type (might be due to invalid IL or missing references)
		//IL_017d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0182: Unknown result type (might be due to invalid IL or missing references)
		//IL_018d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0192: Unknown result type (might be due to invalid IL or missing references)
		//IL_0198: Unknown result type (might be due to invalid IL or missing references)
		//IL_019d: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0352: Unknown result type (might be due to invalid IL or missing references)
		//IL_0357: Unknown result type (might be due to invalid IL or missing references)
		//IL_0362: Unknown result type (might be due to invalid IL or missing references)
		//IL_0367: Unknown result type (might be due to invalid IL or missing references)
		//IL_036d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0372: Unknown result type (might be due to invalid IL or missing references)
		//IL_037d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0388: Unknown result type (might be due to invalid IL or missing references)
		//IL_0399: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_03df: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0440: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_04be: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_04fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0507: Unknown result type (might be due to invalid IL or missing references)
		//IL_0647: Unknown result type (might be due to invalid IL or missing references)
		//IL_064c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0672: Unknown result type (might be due to invalid IL or missing references)
		//IL_06eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_06f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_06fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0700: Unknown result type (might be due to invalid IL or missing references)
		//IL_0706: Unknown result type (might be due to invalid IL or missing references)
		//IL_070b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0710: Unknown result type (might be due to invalid IL or missing references)
		//IL_0718: Unknown result type (might be due to invalid IL or missing references)
		//IL_0729: Unknown result type (might be due to invalid IL or missing references)
		//IL_0734: Unknown result type (might be due to invalid IL or missing references)
		//IL_076f: Unknown result type (might be due to invalid IL or missing references)
		//IL_07e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_07f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_07fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0803: Unknown result type (might be due to invalid IL or missing references)
		//IL_0808: Unknown result type (might be due to invalid IL or missing references)
		//IL_080d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0815: Unknown result type (might be due to invalid IL or missing references)
		//IL_0826: Unknown result type (might be due to invalid IL or missing references)
		//IL_0831: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_05fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0550: Unknown result type (might be due to invalid IL or missing references)
		//IL_0553: Unknown result type (might be due to invalid IL or missing references)
		//IL_055e: Unknown result type (might be due to invalid IL or missing references)
		//IL_056f: Unknown result type (might be due to invalid IL or missing references)
		//IL_057a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0419: Unknown result type (might be due to invalid IL or missing references)
		//IL_041e: Unknown result type (might be due to invalid IL or missing references)
		//IL_040f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0414: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b59: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b5e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b61: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b66: Unknown result type (might be due to invalid IL or missing references)
		//IL_0880: Unknown result type (might be due to invalid IL or missing references)
		//IL_0885: Unknown result type (might be due to invalid IL or missing references)
		//IL_088d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0892: Unknown result type (might be due to invalid IL or missing references)
		//IL_08c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bad: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b7a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0906: Unknown result type (might be due to invalid IL or missing references)
		//IL_090b: Unknown result type (might be due to invalid IL or missing references)
		//IL_091d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0920: Unknown result type (might be due to invalid IL or missing references)
		//IL_0926: Unknown result type (might be due to invalid IL or missing references)
		//IL_0928: Invalid comparison between Unknown and I4
		//IL_0931: Unknown result type (might be due to invalid IL or missing references)
		//IL_0936: Unknown result type (might be due to invalid IL or missing references)
		//IL_0950: Unknown result type (might be due to invalid IL or missing references)
		//IL_0952: Unknown result type (might be due to invalid IL or missing references)
		//IL_0953: Unknown result type (might be due to invalid IL or missing references)
		//IL_09d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_09d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_09e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_09e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_09eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_09f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_09f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_09fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a0e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a19: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a54: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a56: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a57: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ad4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ad9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ae4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ae9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aef: Unknown result type (might be due to invalid IL or missing references)
		//IL_0af4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0af9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b01: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b12: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b1d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d73: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d78: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d7a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d7c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d86: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d8b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dc0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dc5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dca: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bdf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0be4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c40: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cb9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cbe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cc9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cce: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cd4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cd9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cde: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ce5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cf2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cfa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e02: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e13: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e19: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e1b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e4d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e63: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e68: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e6d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e75: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e85: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e8d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c19: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c1e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c0f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c14: Unknown result type (might be due to invalid IL or missing references)
		//IL_1de8: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ded: Unknown result type (might be due to invalid IL or missing references)
		//IL_1dee: Unknown result type (might be due to invalid IL or missing references)
		//IL_1df3: Unknown result type (might be due to invalid IL or missing references)
		//IL_1df6: Unknown result type (might be due to invalid IL or missing references)
		//IL_1dfb: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d29: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d2c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d37: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d48: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d53: Unknown result type (might be due to invalid IL or missing references)
		//IL_0eeb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0eee: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ef9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f0a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f15: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e30: Unknown result type (might be due to invalid IL or missing references)
		//IL_1eaa: Unknown result type (might be due to invalid IL or missing references)
		//IL_1eaf: Unknown result type (might be due to invalid IL or missing references)
		//IL_1eba: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ebf: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ec5: Unknown result type (might be due to invalid IL or missing references)
		//IL_1eca: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ed5: Unknown result type (might be due to invalid IL or missing references)
		//IL_1edf: Unknown result type (might be due to invalid IL or missing references)
		//IL_1eed: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ef8: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e11: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e16: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f4b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1fc4: Unknown result type (might be due to invalid IL or missing references)
		//IL_1fc9: Unknown result type (might be due to invalid IL or missing references)
		//IL_1fd4: Unknown result type (might be due to invalid IL or missing references)
		//IL_1fd9: Unknown result type (might be due to invalid IL or missing references)
		//IL_1fdf: Unknown result type (might be due to invalid IL or missing references)
		//IL_1fe4: Unknown result type (might be due to invalid IL or missing references)
		//IL_1fef: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ffa: Unknown result type (might be due to invalid IL or missing references)
		//IL_200b: Unknown result type (might be due to invalid IL or missing references)
		//IL_2016: Unknown result type (might be due to invalid IL or missing references)
		//IL_133b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1340: Unknown result type (might be due to invalid IL or missing references)
		//IL_1343: Unknown result type (might be due to invalid IL or missing references)
		//IL_1348: Unknown result type (might be due to invalid IL or missing references)
		//IL_1177: Unknown result type (might be due to invalid IL or missing references)
		//IL_117c: Unknown result type (might be due to invalid IL or missing references)
		//IL_117e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1180: Unknown result type (might be due to invalid IL or missing references)
		//IL_118a: Unknown result type (might be due to invalid IL or missing references)
		//IL_118f: Unknown result type (might be due to invalid IL or missing references)
		//IL_119e: Unknown result type (might be due to invalid IL or missing references)
		//IL_11a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_11a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_2530: Unknown result type (might be due to invalid IL or missing references)
		//IL_2535: Unknown result type (might be due to invalid IL or missing references)
		//IL_253f: Unknown result type (might be due to invalid IL or missing references)
		//IL_2544: Unknown result type (might be due to invalid IL or missing references)
		//IL_2547: Unknown result type (might be due to invalid IL or missing references)
		//IL_254c: Unknown result type (might be due to invalid IL or missing references)
		//IL_2274: Unknown result type (might be due to invalid IL or missing references)
		//IL_22ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_22f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_22fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_2302: Unknown result type (might be due to invalid IL or missing references)
		//IL_2308: Unknown result type (might be due to invalid IL or missing references)
		//IL_230d: Unknown result type (might be due to invalid IL or missing references)
		//IL_2318: Unknown result type (might be due to invalid IL or missing references)
		//IL_2336: Unknown result type (might be due to invalid IL or missing references)
		//IL_2347: Unknown result type (might be due to invalid IL or missing references)
		//IL_2352: Unknown result type (might be due to invalid IL or missing references)
		//IL_2073: Unknown result type (might be due to invalid IL or missing references)
		//IL_2078: Unknown result type (might be due to invalid IL or missing references)
		//IL_209c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fd2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fe3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fe9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0feb: Unknown result type (might be due to invalid IL or missing references)
		//IL_101f: Unknown result type (might be due to invalid IL or missing references)
		//IL_10a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_10a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_10b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_10b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_10bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_10c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_10cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_10d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_10e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_10f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_2661: Unknown result type (might be due to invalid IL or missing references)
		//IL_26db: Unknown result type (might be due to invalid IL or missing references)
		//IL_26e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_26eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_26f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_26f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_26fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_2726: Unknown result type (might be due to invalid IL or missing references)
		//IL_2736: Unknown result type (might be due to invalid IL or missing references)
		//IL_273b: Unknown result type (might be due to invalid IL or missing references)
		//IL_2740: Unknown result type (might be due to invalid IL or missing references)
		//IL_2747: Unknown result type (might be due to invalid IL or missing references)
		//IL_2755: Unknown result type (might be due to invalid IL or missing references)
		//IL_2760: Unknown result type (might be due to invalid IL or missing references)
		//IL_20b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_20ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_20db: Unknown result type (might be due to invalid IL or missing references)
		//IL_20e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_1387: Unknown result type (might be due to invalid IL or missing references)
		//IL_138c: Unknown result type (might be due to invalid IL or missing references)
		//IL_257e: Unknown result type (might be due to invalid IL or missing references)
		//IL_2583: Unknown result type (might be due to invalid IL or missing references)
		//IL_258b: Unknown result type (might be due to invalid IL or missing references)
		//IL_2596: Unknown result type (might be due to invalid IL or missing references)
		//IL_25a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_25a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_25b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_25b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_25c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_25cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_25d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_25d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_2397: Unknown result type (might be due to invalid IL or missing references)
		//IL_23c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_2409: Unknown result type (might be due to invalid IL or missing references)
		//IL_2410: Unknown result type (might be due to invalid IL or missing references)
		//IL_2415: Unknown result type (might be due to invalid IL or missing references)
		//IL_2423: Unknown result type (might be due to invalid IL or missing references)
		//IL_249c: Unknown result type (might be due to invalid IL or missing references)
		//IL_24a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_24ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_24b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_24b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_24bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_24c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_24c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_24ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_24d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_24e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_24f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_2118: Unknown result type (might be due to invalid IL or missing references)
		//IL_212d: Unknown result type (might be due to invalid IL or missing references)
		//IL_2132: Unknown result type (might be due to invalid IL or missing references)
		//IL_2143: Unknown result type (might be due to invalid IL or missing references)
		//IL_21bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_21c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_21cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_21d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_21d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_21dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_2207: Unknown result type (might be due to invalid IL or missing references)
		//IL_220c: Unknown result type (might be due to invalid IL or missing references)
		//IL_2211: Unknown result type (might be due to invalid IL or missing references)
		//IL_2218: Unknown result type (might be due to invalid IL or missing references)
		//IL_2226: Unknown result type (might be due to invalid IL or missing references)
		//IL_2231: Unknown result type (might be due to invalid IL or missing references)
		//IL_20e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_20fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_2111: Unknown result type (might be due to invalid IL or missing references)
		//IL_2116: Unknown result type (might be due to invalid IL or missing references)
		//IL_13c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_13bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_25e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_25e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_25f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_25fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_2606: Unknown result type (might be due to invalid IL or missing references)
		//IL_260b: Unknown result type (might be due to invalid IL or missing references)
		//IL_2616: Unknown result type (might be due to invalid IL or missing references)
		//IL_261b: Unknown result type (might be due to invalid IL or missing references)
		//IL_262a: Unknown result type (might be due to invalid IL or missing references)
		//IL_2636: Unknown result type (might be due to invalid IL or missing references)
		//IL_2641: Unknown result type (might be due to invalid IL or missing references)
		//IL_2646: Unknown result type (might be due to invalid IL or missing references)
		//IL_264b: Unknown result type (might be due to invalid IL or missing references)
		//IL_2650: Unknown result type (might be due to invalid IL or missing references)
		//IL_13c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_1235: Unknown result type (might be due to invalid IL or missing references)
		//IL_1223: Unknown result type (might be due to invalid IL or missing references)
		//IL_124d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1254: Unknown result type (might be due to invalid IL or missing references)
		//IL_13e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_13e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_1267: Unknown result type (might be due to invalid IL or missing references)
		//IL_1278: Unknown result type (might be due to invalid IL or missing references)
		//IL_127e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1280: Unknown result type (might be due to invalid IL or missing references)
		//IL_12b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_12d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_12d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_12da: Unknown result type (might be due to invalid IL or missing references)
		//IL_12e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_12f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_12fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_1430: Unknown result type (might be due to invalid IL or missing references)
		//IL_14a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_14ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_14b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_14be: Unknown result type (might be due to invalid IL or missing references)
		//IL_14c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_14c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_14ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_14d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_14e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_14ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_1535: Unknown result type (might be due to invalid IL or missing references)
		//IL_15ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_15b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_15be: Unknown result type (might be due to invalid IL or missing references)
		//IL_15c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_15c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_15ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_15d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_15da: Unknown result type (might be due to invalid IL or missing references)
		//IL_15e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_15ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_163b: Unknown result type (might be due to invalid IL or missing references)
		//IL_16b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_16b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_16c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_16c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_16cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_16d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_16d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_16e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_16ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_16f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b6c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a68: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ae1: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ae6: Unknown result type (might be due to invalid IL or missing references)
		//IL_1af1: Unknown result type (might be due to invalid IL or missing references)
		//IL_1af6: Unknown result type (might be due to invalid IL or missing references)
		//IL_1afc: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b01: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b06: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b0e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b1e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b26: Unknown result type (might be due to invalid IL or missing references)
		//IL_1bc8: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c41: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c46: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c51: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c56: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c5c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c61: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c6a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c6f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c7a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c84: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c92: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c9d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1778: Unknown result type (might be due to invalid IL or missing references)
		//IL_177d: Unknown result type (might be due to invalid IL or missing references)
		//IL_178f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1792: Unknown result type (might be due to invalid IL or missing references)
		//IL_1798: Unknown result type (might be due to invalid IL or missing references)
		//IL_179a: Invalid comparison between Unknown and I4
		//IL_17a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_17a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_193d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1955: Unknown result type (might be due to invalid IL or missing references)
		//IL_1957: Unknown result type (might be due to invalid IL or missing references)
		//IL_1958: Unknown result type (might be due to invalid IL or missing references)
		//IL_19d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_19da: Unknown result type (might be due to invalid IL or missing references)
		//IL_19e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_19ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_19f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_19f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_19fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a02: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a12: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a1a: Unknown result type (might be due to invalid IL or missing references)
		//IL_17f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_17fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_1803: Unknown result type (might be due to invalid IL or missing references)
		//IL_1805: Unknown result type (might be due to invalid IL or missing references)
		//IL_1806: Unknown result type (might be due to invalid IL or missing references)
		//IL_1883: Unknown result type (might be due to invalid IL or missing references)
		//IL_1888: Unknown result type (might be due to invalid IL or missing references)
		//IL_1893: Unknown result type (might be due to invalid IL or missing references)
		//IL_1898: Unknown result type (might be due to invalid IL or missing references)
		//IL_189e: Unknown result type (might be due to invalid IL or missing references)
		//IL_18a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_18a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_18b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_18c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_18cc: Unknown result type (might be due to invalid IL or missing references)
		Vector2 pos = Vector2.Zero;
		drawinfo.drawPlayer.ApplyHeadOffsetFromMount(ref pos);
		Vector2 helmetOffset = drawinfo.helmetOffset + pos;
		DrawPlayer_21_Head_TheFace(ref drawinfo);
		bool flag = drawinfo.drawPlayer.head == 14 || drawinfo.drawPlayer.head == 56 || drawinfo.drawPlayer.head == 114 || drawinfo.drawPlayer.head == 158 || drawinfo.drawPlayer.head == 69 || drawinfo.drawPlayer.head == 180;
		bool flag2 = drawinfo.drawPlayer.head == 28;
		bool flag3 = drawinfo.drawPlayer.head == 39 || drawinfo.drawPlayer.head == 38;
		bool flag4 = true;
		if (drawinfo.drawPlayer.mount.Active)
		{
			int type = drawinfo.drawPlayer.mount.Type;
			if (type == 54)
			{
				if (drawinfo.drawPlayer.head >= 0 && !ArmorIDs.Head.Sets.CanDrawOnVelociraptorMount[drawinfo.drawPlayer.head])
				{
					flag4 = false;
				}
			}
			else if (type >= 0 && MountID.Sets.PlayerIsHidden[type])
			{
				flag4 = false;
			}
		}
		Vector2 val = new Vector2((float)(-drawinfo.drawPlayer.bodyFrame.Width / 2 + drawinfo.drawPlayer.width / 2), (float)(drawinfo.drawPlayer.height - drawinfo.drawPlayer.bodyFrame.Height + 4));
		Vector2 val2 = (drawinfo.Position - Main.screenPosition + val).Floor() + drawinfo.drawPlayer.headPosition + drawinfo.headVect + pos;
		if (((int)drawinfo.playerEffect & 2) != 0)
		{
			int num = drawinfo.drawPlayer.bodyFrame.Height - drawinfo.hairFrontFrame.Height;
			val2.Y += num;
		}
		val2 += drawinfo.hairOffset;
		bool flag5 = drawinfo.drawPlayer.faceMask > 0 && drawinfo.drawPlayer.faceMask < ArmorIDs.Face.Count;
		if (flag5 && drawinfo.drawPlayer.head > 0 && drawinfo.drawPlayer.head < ArmorIDs.Head.Count && !ArmorIDs.Head.Sets.DrawFaceMaskUnderHeadLayer[drawinfo.drawPlayer.head])
		{
			flag5 = false;
		}
		else if (flag5 && drawinfo.drawPlayer.mount.Active && drawinfo.drawPlayer.mount.Type == 54 && !ArmorIDs.Face.Sets.CanDrawOnVelociraptorMount[drawinfo.drawPlayer.faceMask])
		{
			flag5 = false;
		}
		DrawData item;
		if (flag5)
		{
			Vector2 faceDrawOffset = drawinfo.drawPlayer.GetFaceDrawOffset(drawinfo.drawPlayer.faceMask);
			item = new DrawData(TextureAssets.AccFace[drawinfo.drawPlayer.faceMask].Value, faceDrawOffset + pos + drawinfo.helmetOffset + new Vector2((float)(int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.headPosition + drawinfo.headVect, drawinfo.drawPlayer.bodyFrame, drawinfo.colorArmorHead, drawinfo.drawPlayer.headRotation, drawinfo.headVect, 1f, drawinfo.playerEffect);
			item.shader = drawinfo.cFaceMask;
			drawinfo.DrawDataCache.Add(item);
		}
		if (flag4 && drawinfo.fullHair)
		{
			Color color = drawinfo.colorArmorHead;
			int shader = drawinfo.cHead;
			if (ArmorIDs.Head.Sets.UseSkinColor[drawinfo.drawPlayer.head])
			{
				color = ((!drawinfo.drawPlayer.isDisplayDollOrInanimate) ? drawinfo.colorHead : drawinfo.colorDisplayDollSkin);
				shader = drawinfo.skinDyePacked;
			}
			item = new DrawData(TextureAssets.ArmorHead[drawinfo.drawPlayer.head].Value, helmetOffset + new Vector2((float)(int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.headPosition + drawinfo.headVect, drawinfo.drawPlayer.bodyFrame, color, drawinfo.drawPlayer.headRotation, drawinfo.headVect, 1f, drawinfo.playerEffect);
			item.shader = shader;
			drawinfo.DrawDataCache.Add(item);
			if (!drawinfo.drawPlayer.invis)
			{
				item = new DrawData(TextureAssets.PlayerHair[drawinfo.drawPlayer.hair].Value, val2, drawinfo.hairFrontFrame, drawinfo.colorHair, drawinfo.drawPlayer.headRotation, drawinfo.headVect, 1f, drawinfo.playerEffect);
				item.shader = drawinfo.hairDyePacked;
				drawinfo.DrawDataCache.Add(item);
			}
		}
		if (flag4 && drawinfo.hatHair && !drawinfo.drawPlayer.invis)
		{
			item = new DrawData(TextureAssets.PlayerHairAlt[drawinfo.drawPlayer.hair].Value, val2, drawinfo.hairFrontFrame, drawinfo.colorHair, drawinfo.drawPlayer.headRotation, drawinfo.headVect, 1f, drawinfo.playerEffect);
			item.shader = drawinfo.hairDyePacked;
			drawinfo.DrawDataCache.Add(item);
		}
		if (flag4 && drawinfo.drawPlayer.head == 270)
		{
			Rectangle bodyFrame = drawinfo.drawPlayer.bodyFrame;
			bodyFrame.Width += 2;
			item = new DrawData(TextureAssets.ArmorHead[drawinfo.drawPlayer.head].Value, helmetOffset + new Vector2((float)(int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.headPosition + drawinfo.headVect, bodyFrame, drawinfo.colorArmorHead, drawinfo.drawPlayer.headRotation, drawinfo.headVect, 1f, drawinfo.playerEffect);
			item.shader = drawinfo.cHead;
			drawinfo.DrawDataCache.Add(item);
			item = new DrawData(TextureAssets.GlowMask[drawinfo.headGlowMask].Value, helmetOffset + new Vector2((float)(int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.headPosition + drawinfo.headVect, bodyFrame, drawinfo.headGlowColor, drawinfo.drawPlayer.headRotation, drawinfo.headVect, 1f, drawinfo.playerEffect);
			item.shader = drawinfo.cHead;
			drawinfo.DrawDataCache.Add(item);
		}
		else if (flag4 && drawinfo.drawPlayer.head == 282)
		{
			Rectangle bodyFrame2 = drawinfo.drawPlayer.bodyFrame;
			Rectangle bodyFrame3 = drawinfo.drawPlayer.bodyFrame;
			bodyFrame3.X = (bodyFrame3.Y = 0);
			int num2 = 9;
			int num3 = 4;
			int num4 = drawinfo.drawPlayer.miscCounter % (num2 * num3) / num3;
			bodyFrame2.Y = bodyFrame2.Height * num4;
			int num5 = 0;
			num5 += drawinfo.drawPlayer.bodyFrame.Y / 56;
			if (num5 >= Main.OffsetsPlayerHeadgear.Length)
			{
				num5 = 0;
			}
			Vector2 val3 = Main.OffsetsPlayerHeadgear[num5];
			val3.Y -= 2f;
			val3 *= (float)(-(((int)drawinfo.playerEffect & 2) > 0).ToDirectionInt());
			item = new DrawData(TextureAssets.ArmorHead[drawinfo.drawPlayer.head].Value, val3 + helmetOffset + new Vector2((float)(int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.headPosition + drawinfo.headVect, bodyFrame2, drawinfo.colorArmorHead, drawinfo.drawPlayer.headRotation, drawinfo.headVect, 1f, drawinfo.playerEffect);
			item.shader = drawinfo.cHead;
			drawinfo.DrawDataCache.Add(item);
			item = new DrawData(TextureAssets.GlowMask[drawinfo.headGlowMask].Value, val3 + helmetOffset + new Vector2((float)(int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.headPosition + drawinfo.headVect, bodyFrame3, drawinfo.headGlowColor, drawinfo.drawPlayer.headRotation, drawinfo.headVect, 1f, drawinfo.playerEffect);
			item.shader = drawinfo.cHead;
			drawinfo.DrawDataCache.Add(item);
		}
		else if (flag4 & flag)
		{
			Rectangle bodyFrame4 = drawinfo.drawPlayer.bodyFrame;
			Vector2 headVect = drawinfo.headVect;
			if (drawinfo.drawPlayer.gravDir == 1f)
			{
				if (bodyFrame4.Y != 0)
				{
					bodyFrame4.Y -= 2;
					headVect.Y += 2f;
				}
				bodyFrame4.Height -= 8;
			}
			else if (bodyFrame4.Y != 0)
			{
				bodyFrame4.Y -= 2;
				headVect.Y -= 10f;
				bodyFrame4.Height -= 8;
			}
			Color color2 = drawinfo.colorArmorHead;
			int shader2 = drawinfo.cHead;
			if (ArmorIDs.Head.Sets.UseSkinColor[drawinfo.drawPlayer.head])
			{
				color2 = ((!drawinfo.drawPlayer.isDisplayDollOrInanimate) ? drawinfo.colorHead : drawinfo.colorDisplayDollSkin);
				shader2 = drawinfo.skinDyePacked;
			}
			item = new DrawData(TextureAssets.ArmorHead[drawinfo.drawPlayer.head].Value, helmetOffset + new Vector2((float)(int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.headPosition + drawinfo.headVect, bodyFrame4, color2, drawinfo.drawPlayer.headRotation, headVect, 1f, drawinfo.playerEffect);
			item.shader = shader2;
			drawinfo.DrawDataCache.Add(item);
		}
		else if (flag4 && drawinfo.drawPlayer.head == 259)
		{
			int verticalFrames = 27;
			Texture2D value = TextureAssets.ArmorHead[drawinfo.drawPlayer.head].Value;
			Rectangle val4 = value.Frame(1, verticalFrames, 0, drawinfo.drawPlayer.rabbitOrderFrame.DisplayFrame);
			Vector2 origin = val4.Size() / 2f;
			int num6 = drawinfo.drawPlayer.babyBird.ToInt();
			Vector2 val5 = DrawPlayer_21_Head_GetSpecialHatDrawPosition(ref drawinfo, ref helmetOffset, new Vector2((float)(1 + num6 * 2), (float)(-26 + drawinfo.drawPlayer.babyBird.ToInt() * -6)));
			int hatStacks = GetHatStacks(ref drawinfo, 4955);
			float num7 = (float)Math.PI / 60f;
			float num8 = num7 * drawinfo.drawPlayer.position.X % ((float)Math.PI * 2f);
			for (int num9 = hatStacks - 1; num9 >= 0; num9--)
			{
				float num10 = Vector2.UnitY.RotatedBy(num8 + num7 * (float)num9).X * ((float)num9 / 30f) * 2f - (float)(num9 * 2 * drawinfo.drawPlayer.direction);
				item = new DrawData(value, val5 + new Vector2(num10, (float)(num9 * -14) * drawinfo.drawPlayer.gravDir), val4, drawinfo.colorArmorHead, drawinfo.drawPlayer.headRotation, origin, 1f, drawinfo.playerEffect);
				item.shader = drawinfo.cHead;
				drawinfo.DrawDataCache.Add(item);
			}
			if (!drawinfo.drawPlayer.invis)
			{
				item = new DrawData(TextureAssets.PlayerHair[drawinfo.drawPlayer.hair].Value, val2, drawinfo.hairFrontFrame, drawinfo.colorHair, drawinfo.drawPlayer.headRotation, drawinfo.headVect, 1f, drawinfo.playerEffect);
				item.shader = drawinfo.hairDyePacked;
				drawinfo.DrawDataCache.Add(item);
			}
		}
		else if (flag4 && drawinfo.drawPlayer.head > 0 && drawinfo.drawPlayer.head < ArmorIDs.Head.Count && !flag2)
		{
			if (!(drawinfo.drawPlayer.invis & flag3))
			{
				if (drawinfo.drawPlayer.head == 13)
				{
					int hatStacks2 = GetHatStacks(ref drawinfo, 205);
					float num11 = (float)Math.PI / 60f;
					float num12 = num11 * drawinfo.drawPlayer.position.X % ((float)Math.PI * 2f);
					for (int i = 0; i < hatStacks2; i++)
					{
						float num13 = Vector2.UnitY.RotatedBy(num12 + num11 * (float)i).X * ((float)i / 30f) * 2f;
						item = new DrawData(TextureAssets.ArmorHead[drawinfo.drawPlayer.head].Value, helmetOffset + new Vector2((float)(int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)) + num13, (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f - (float)(4 * i))) + drawinfo.drawPlayer.headPosition + drawinfo.headVect, drawinfo.drawPlayer.bodyFrame, drawinfo.colorArmorHead, drawinfo.drawPlayer.headRotation, drawinfo.headVect, 1f, drawinfo.playerEffect);
						item.shader = drawinfo.cHead;
						drawinfo.DrawDataCache.Add(item);
					}
				}
				else if (drawinfo.drawPlayer.head == 265)
				{
					int verticalFrames2 = 6;
					Texture2D value2 = TextureAssets.ArmorHead[drawinfo.drawPlayer.head].Value;
					Rectangle val6 = value2.Frame(1, verticalFrames2, 0, drawinfo.drawPlayer.rabbitOrderFrame.DisplayFrame);
					Vector2 origin2 = val6.Size() / 2f;
					Vector2 val7 = DrawPlayer_21_Head_GetSpecialHatDrawPosition(ref drawinfo, ref helmetOffset, new Vector2(0f, -9f));
					int hatStacks3 = GetHatStacks(ref drawinfo, 5004);
					float num14 = (float)Math.PI / 60f;
					float num15 = num14 * drawinfo.drawPlayer.position.X % ((float)Math.PI * 2f);
					int num16 = hatStacks3 * 4 + 2;
					int num17 = 0;
					bool flag6 = (Main.GlobalTimeWrappedHourly + 180f) % 600f < 60f;
					for (int num18 = num16 - 1; num18 >= 0; num18--)
					{
						int num19 = 0;
						if (num18 == num16 - 1)
						{
							val6.Y = 0;
							num19 = 2;
						}
						else if (num18 == 0)
						{
							val6.Y = val6.Height * 5;
						}
						else
						{
							val6.Y = val6.Height * (num17++ % 4 + 1);
						}
						if (!((val6.Y == val6.Height * 3) & flag6))
						{
							float num20 = Vector2.UnitY.RotatedBy(num15 + num14 * (float)num18).X * ((float)num18 / 10f) * 4f - (float)num18 * 0.1f * (float)drawinfo.drawPlayer.direction;
							item = new DrawData(value2, val7 + new Vector2(num20, (float)(num18 * -4 + num19) * drawinfo.drawPlayer.gravDir), val6, drawinfo.colorArmorHead, drawinfo.drawPlayer.headRotation, origin2, 1f, drawinfo.playerEffect);
							item.shader = drawinfo.cHead;
							drawinfo.DrawDataCache.Add(item);
						}
					}
				}
				else
				{
					Rectangle bodyFrame5 = drawinfo.drawPlayer.bodyFrame;
					Vector2 headVect2 = drawinfo.headVect;
					if (drawinfo.drawPlayer.gravDir == 1f)
					{
						bodyFrame5.Height -= 4;
					}
					else
					{
						headVect2.Y -= 4f;
						bodyFrame5.Height -= 4;
					}
					Color color3 = drawinfo.colorArmorHead;
					int shader3 = drawinfo.cHead;
					Texture2D value3 = TextureAssets.ArmorHead[drawinfo.drawPlayer.head].Value;
					Color val8 = (drawinfo.drawPlayer.isDisplayDollOrInanimate ? drawinfo.colorDisplayDollSkin : drawinfo.colorHead);
					int skinDyePacked = drawinfo.skinDyePacked;
					if (ArmorIDs.Head.Sets.UseSkinColor[drawinfo.drawPlayer.head])
					{
						color3 = val8;
						shader3 = skinDyePacked;
					}
					if (drawinfo.drawPlayer.mount.Active && drawinfo.mountHandlesHeadDraw && drawinfo.drawPlayer.head == 288)
					{
						value3 = TextureAssets.Extra[284].Value;
					}
					item = new DrawData(value3, helmetOffset + new Vector2((float)(int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.headPosition + drawinfo.headVect, bodyFrame5, color3, drawinfo.drawPlayer.headRotation, headVect2, 1f, drawinfo.playerEffect);
					item.shader = shader3;
					drawinfo.DrawDataCache.Add(item);
					if (drawinfo.drawPlayer.head == 292)
					{
						item = new DrawData(TextureAssets.Extra[300].Value, helmetOffset + new Vector2((float)(int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.headPosition + drawinfo.headVect, bodyFrame5, val8, drawinfo.drawPlayer.headRotation, headVect2, 1f, drawinfo.playerEffect);
						item.shader = skinDyePacked;
						drawinfo.DrawDataCache.Add(item);
					}
					if (drawinfo.drawPlayer.head == 109)
					{
						Texture2D value4 = TextureAssets.Extra[276].Value;
						item = new DrawData(value4, helmetOffset + new Vector2((float)(int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.headPosition + drawinfo.headVect, bodyFrame5, color3, drawinfo.drawPlayer.headRotation, headVect2, 1f, drawinfo.playerEffect);
						item.shader = 0;
						drawinfo.DrawDataCache.Add(item);
					}
					if (drawinfo.headGlowMask != -1)
					{
						if (drawinfo.headGlowMask == 309)
						{
							int num21 = DrawPlayer_Head_GetTVScreen(drawinfo.drawPlayer);
							if (num21 != 0)
							{
								int num22 = 0;
								num22 += drawinfo.drawPlayer.bodyFrame.Y / 56;
								if (num22 >= Main.OffsetsPlayerHeadgear.Length)
								{
									num22 = 0;
								}
								Vector2 val9 = Main.OffsetsPlayerHeadgear[num22];
								val9.Y -= 2f;
								val9 *= (float)(-(((int)drawinfo.playerEffect & 2) > 0).ToDirectionInt());
								Texture2D value5 = TextureAssets.GlowMask[drawinfo.headGlowMask].Value;
								int frameY = drawinfo.drawPlayer.miscCounter % 20 / 5;
								if (num21 == 5)
								{
									frameY = 0;
									if (drawinfo.drawPlayer.eyeHelper.EyeFrameToShow > 0)
									{
										frameY = 2;
									}
								}
								Rectangle value6 = value5.Frame(6, 4, num21, frameY, -2);
								item = new DrawData(value5, val9 + helmetOffset + new Vector2((float)(int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.headPosition + drawinfo.headVect, value6, drawinfo.headGlowColor, drawinfo.drawPlayer.headRotation, drawinfo.headVect, 1f, drawinfo.playerEffect);
								item.shader = drawinfo.cHead;
								drawinfo.DrawDataCache.Add(item);
							}
						}
						else if (drawinfo.headGlowMask == 273)
						{
							for (int j = 0; j < 2; j++)
							{
								item = new DrawData(position: new Vector2((float)Main.rand.Next(-10, 10) * 0.125f, (float)Main.rand.Next(-10, 10) * 0.125f) + helmetOffset + new Vector2((float)(int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.headPosition + drawinfo.headVect, texture: TextureAssets.GlowMask[drawinfo.headGlowMask].Value, sourceRect: bodyFrame5, color: drawinfo.headGlowColor, rotation: drawinfo.drawPlayer.headRotation, origin: headVect2, scale: 1f, effect: drawinfo.playerEffect);
								item.shader = drawinfo.cHead;
								drawinfo.DrawDataCache.Add(item);
							}
						}
						else
						{
							item = new DrawData(TextureAssets.GlowMask[drawinfo.headGlowMask].Value, helmetOffset + new Vector2((float)(int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.headPosition + drawinfo.headVect, bodyFrame5, drawinfo.headGlowColor, drawinfo.drawPlayer.headRotation, headVect2, 1f, drawinfo.playerEffect);
							item.shader = drawinfo.cHead;
							drawinfo.DrawDataCache.Add(item);
						}
					}
					if (drawinfo.drawPlayer.head == 211)
					{
						Color color4 = new Color(100, 100, 100, 0);
						ulong seed = (ulong)(drawinfo.drawPlayer.miscCounter / 4 + 100);
						int num23 = 4;
						for (int k = 0; k < num23; k++)
						{
							float num24 = (float)Utils.RandomInt(ref seed, -10, 11) * 0.2f;
							float num25 = (float)Utils.RandomInt(ref seed, -14, 1) * 0.15f;
							item = new DrawData(TextureAssets.GlowMask[241].Value, helmetOffset + new Vector2((float)(int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.headPosition + drawinfo.headVect + new Vector2(num24, num25), drawinfo.drawPlayer.bodyFrame, color4, drawinfo.drawPlayer.headRotation, drawinfo.headVect, 1f, drawinfo.playerEffect);
							item.shader = drawinfo.cHead;
							drawinfo.DrawDataCache.Add(item);
						}
					}
				}
			}
		}
		else if (flag4 && !drawinfo.drawPlayer.invis && (drawinfo.drawPlayer.face < 0 || !ArmorIDs.Face.Sets.PreventHairDraw[drawinfo.drawPlayer.face]))
		{
			item = new DrawData(TextureAssets.PlayerHair[drawinfo.drawPlayer.hair].Value, val2, drawinfo.hairFrontFrame, drawinfo.colorHair, drawinfo.drawPlayer.headRotation, drawinfo.headVect, 1f, drawinfo.playerEffect);
			item.shader = drawinfo.hairDyePacked;
			drawinfo.DrawDataCache.Add(item);
		}
		bool flag7 = drawinfo.drawPlayer.head < 0 || !ArmorIDs.Head.Sets.PreventBeardDraw[drawinfo.drawPlayer.head];
		if (drawinfo.drawPlayer.mount.Active && drawinfo.drawPlayer.mount.Type == 54)
		{
			flag7 = true;
		}
		if ((drawinfo.drawPlayer.beard > 0) & flag7)
		{
			Vector2 val10 = drawinfo.drawPlayer.GetBeardDrawOffset() + pos;
			Color color5 = drawinfo.colorArmorHead;
			if (ArmorIDs.Beard.Sets.UseHairColor[drawinfo.drawPlayer.beard])
			{
				color5 = drawinfo.colorHair;
			}
			item = new DrawData(TextureAssets.AccBeard[drawinfo.drawPlayer.beard].Value, val10 + new Vector2((float)(int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.headPosition + drawinfo.headVect, drawinfo.drawPlayer.bodyFrame, color5, drawinfo.drawPlayer.headRotation, drawinfo.headVect, 1f, drawinfo.playerEffect);
			item.shader = drawinfo.cBeard;
			drawinfo.DrawDataCache.Add(item);
		}
		if (flag4 && drawinfo.drawPlayer.head == 205)
		{
			DrawData drawData = new DrawData(TextureAssets.Extra[77].Value, helmetOffset + new Vector2((float)(int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.headPosition + drawinfo.headVect, drawinfo.drawPlayer.bodyFrame, drawinfo.colorHead, drawinfo.drawPlayer.headRotation, drawinfo.headVect, 1f, drawinfo.playerEffect);
			drawData.shader = drawinfo.skinDyePacked;
			item = drawData;
			drawinfo.DrawDataCache.Add(item);
		}
		if (flag4 && drawinfo.drawPlayer.head == 214 && !drawinfo.drawPlayer.invis)
		{
			Rectangle bodyFrame6 = drawinfo.drawPlayer.bodyFrame;
			bodyFrame6.Y = 0;
			float num26 = (float)drawinfo.drawPlayer.miscCounter / 300f;
			Color val11 = new Color(0, 0, 0, 0);
			float num27 = 0.8f;
			float num28 = 0.9f;
			if (num26 >= num27)
			{
				val11 = Color.Lerp(Color.Transparent, new Color(200, 200, 200, 0), Utils.GetLerpValue(num27, num28, num26, clamped: true));
			}
			if (num26 >= num28)
			{
				val11 = Color.Lerp(Color.Transparent, new Color(200, 200, 200, 0), Utils.GetLerpValue(1f, num28, num26, clamped: true));
			}
			val11 *= drawinfo.stealth * (1f - drawinfo.shadow);
			item = new DrawData(TextureAssets.Extra[90].Value, helmetOffset + new Vector2((float)(int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.headPosition + drawinfo.headVect - Main.OffsetsPlayerHeadgear[drawinfo.drawPlayer.bodyFrame.Y / drawinfo.drawPlayer.bodyFrame.Height], bodyFrame6, val11, drawinfo.drawPlayer.headRotation, drawinfo.headVect, 1f, drawinfo.playerEffect);
			drawinfo.DrawDataCache.Add(item);
		}
		if (flag4 && drawinfo.drawPlayer.head == 137)
		{
			item = new DrawData(TextureAssets.JackHat.Value, helmetOffset + new Vector2((float)(int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.headPosition + drawinfo.headVect, drawinfo.drawPlayer.bodyFrame, new Color(255, 255, 255, 255), drawinfo.drawPlayer.headRotation, drawinfo.headVect, 1f, drawinfo.playerEffect);
			drawinfo.DrawDataCache.Add(item);
			for (int l = 0; l < 7; l++)
			{
				Color color6 = new Color(110 - l * 10, 110 - l * 10, 110 - l * 10, 110 - l * 10);
				Vector2 val12 = new Vector2((float)Main.rand.Next(-10, 11) * 0.2f, (float)Main.rand.Next(-10, 11) * 0.2f);
				val12.X = drawinfo.drawPlayer.itemFlamePos[l].X;
				val12.Y = drawinfo.drawPlayer.itemFlamePos[l].Y;
				val12 *= 0.5f;
				item = new DrawData(TextureAssets.JackHat.Value, helmetOffset + new Vector2((float)(int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.headPosition + drawinfo.headVect + val12, drawinfo.drawPlayer.bodyFrame, color6, drawinfo.drawPlayer.headRotation, drawinfo.headVect, 1f, drawinfo.playerEffect);
				drawinfo.DrawDataCache.Add(item);
			}
		}
		if (!drawinfo.drawPlayer.babyBird)
		{
			return;
		}
		Rectangle bodyFrame7 = drawinfo.drawPlayer.bodyFrame;
		bodyFrame7.Y = 0;
		Vector2 pos2 = Vector2.Zero;
		Color color7 = drawinfo.colorArmorHead;
		if (drawinfo.drawPlayer.mount.Active)
		{
			int type2 = drawinfo.drawPlayer.mount.Type;
			if (type2 == 52)
			{
				Vector2 mountedCenter = drawinfo.drawPlayer.MountedCenter;
				color7 = drawinfo.drawPlayer.GetImmuneAlphaPure(Lighting.GetColorClamped((int)mountedCenter.X / 16, (int)mountedCenter.Y / 16, Color.White), drawinfo.shadow);
				pos2 = new Vector2(0f, 6f) * drawinfo.drawPlayer.Directions;
			}
			if (type2 == 54)
			{
				Vector2 mountedCenter2 = drawinfo.drawPlayer.MountedCenter;
				color7 = drawinfo.drawPlayer.GetImmuneAlphaPure(Lighting.GetColorClamped((int)mountedCenter2.X / 16, (int)mountedCenter2.Y / 16, Color.White), drawinfo.shadow);
				drawinfo.drawPlayer.ApplyHeadOffsetFromMount(ref pos2);
				pos2 += new Vector2(-2f, 2f) * drawinfo.drawPlayer.Directions;
			}
		}
		item = new DrawData(TextureAssets.Extra[100].Value, pos2 + new Vector2((float)(int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.headPosition + drawinfo.headVect + Main.OffsetsPlayerHeadgear[drawinfo.drawPlayer.bodyFrame.Y / drawinfo.drawPlayer.bodyFrame.Height] * drawinfo.drawPlayer.gravDir, bodyFrame7, color7, drawinfo.drawPlayer.headRotation, drawinfo.headVect, 1f, drawinfo.playerEffect);
		drawinfo.DrawDataCache.Add(item);
	}

	public static int DrawPlayer_Head_GetTVScreen(Player plr)
	{
		if (NPC.AnyDanger())
		{
			return 4;
		}
		if (plr.statLife <= plr.statLifeMax2 / 4)
		{
			return 1;
		}
		if (plr.ZoneCorrupt || plr.ZoneCrimson || plr.ZoneGraveyard)
		{
			return 0;
		}
		if (plr.wet)
		{
			return 2;
		}
		if (plr.townNPCs >= 3 || BirthdayParty.PartyIsUp || LanternNight.LanternsUp)
		{
			return 5;
		}
		return 3;
	}

	private static int GetHatStacks(ref PlayerDrawSet drawinfo, int hatItemId)
	{
		int num = 0;
		Item effectiveArmor = drawinfo.drawPlayer.GetEffectiveArmor(0);
		if (effectiveArmor != null && effectiveArmor.type == hatItemId && effectiveArmor.stack > 0)
		{
			num += effectiveArmor.stack;
		}
		effectiveArmor = drawinfo.drawPlayer.GetEffectiveArmor(10);
		if (effectiveArmor != null && effectiveArmor.type == hatItemId && effectiveArmor.stack > 0)
		{
			num += effectiveArmor.stack;
		}
		if (num > 2)
		{
			num = 2;
		}
		return num;
	}

	private static Vector2 DrawPlayer_21_Head_GetSpecialHatDrawPosition(ref PlayerDrawSet drawinfo, ref Vector2 helmetOffset, Vector2 hatOffset)
	{
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0105: Unknown result type (might be due to invalid IL or missing references)
		//IL_0106: Unknown result type (might be due to invalid IL or missing references)
		//IL_010b: Unknown result type (might be due to invalid IL or missing references)
		//IL_010c: Unknown result type (might be due to invalid IL or missing references)
		Vector2 val = Main.OffsetsPlayerHeadgear[drawinfo.drawPlayer.bodyFrame.Y / drawinfo.drawPlayer.bodyFrame.Height] * drawinfo.drawPlayer.Directions;
		Vector2 vec = drawinfo.Position - Main.screenPosition + helmetOffset + new Vector2((float)(-drawinfo.drawPlayer.bodyFrame.Width / 2 + drawinfo.drawPlayer.width / 2), (float)(drawinfo.drawPlayer.height - drawinfo.drawPlayer.bodyFrame.Height + 4)) + hatOffset * drawinfo.drawPlayer.Directions + val;
		vec = vec.Floor();
		vec += drawinfo.drawPlayer.headPosition + drawinfo.headVect;
		if (drawinfo.drawPlayer.gravDir == -1f)
		{
			vec.Y += 12f;
		}
		return vec.Floor();
	}

	private static void DrawPlayer_21_Head_TheFace(ref PlayerDrawSet drawinfo)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0559: Unknown result type (might be due to invalid IL or missing references)
		//IL_0564: Unknown result type (might be due to invalid IL or missing references)
		//IL_0569: Unknown result type (might be due to invalid IL or missing references)
		//IL_056f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0574: Unknown result type (might be due to invalid IL or missing references)
		//IL_057f: Unknown result type (might be due to invalid IL or missing references)
		//IL_058a: Unknown result type (might be due to invalid IL or missing references)
		//IL_059b: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0661: Unknown result type (might be due to invalid IL or missing references)
		//IL_066c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0671: Unknown result type (might be due to invalid IL or missing references)
		//IL_0677: Unknown result type (might be due to invalid IL or missing references)
		//IL_067c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0687: Unknown result type (might be due to invalid IL or missing references)
		//IL_0692: Unknown result type (might be due to invalid IL or missing references)
		//IL_06a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_0759: Unknown result type (might be due to invalid IL or missing references)
		//IL_0764: Unknown result type (might be due to invalid IL or missing references)
		//IL_0769: Unknown result type (might be due to invalid IL or missing references)
		//IL_076f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0774: Unknown result type (might be due to invalid IL or missing references)
		//IL_077f: Unknown result type (might be due to invalid IL or missing references)
		//IL_078a: Unknown result type (might be due to invalid IL or missing references)
		//IL_079b: Unknown result type (might be due to invalid IL or missing references)
		//IL_07a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_028c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0297: Unknown result type (might be due to invalid IL or missing references)
		//IL_029c: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02df: Unknown result type (might be due to invalid IL or missing references)
		//IL_07e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_085f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0864: Unknown result type (might be due to invalid IL or missing references)
		//IL_086f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0874: Unknown result type (might be due to invalid IL or missing references)
		//IL_087a: Unknown result type (might be due to invalid IL or missing references)
		//IL_087f: Unknown result type (might be due to invalid IL or missing references)
		//IL_088a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0895: Unknown result type (might be due to invalid IL or missing references)
		//IL_08a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_08b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_0143: Unknown result type (might be due to invalid IL or missing references)
		//IL_0148: Unknown result type (might be due to invalid IL or missing references)
		//IL_0153: Unknown result type (might be due to invalid IL or missing references)
		//IL_0158: Unknown result type (might be due to invalid IL or missing references)
		//IL_015e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0163: Unknown result type (might be due to invalid IL or missing references)
		//IL_016e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0179: Unknown result type (might be due to invalid IL or missing references)
		//IL_018a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0195: Unknown result type (might be due to invalid IL or missing references)
		//IL_0957: Unknown result type (might be due to invalid IL or missing references)
		//IL_09d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_09d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_09e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_09e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_09eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_09f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_09fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a06: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a17: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a22: Unknown result type (might be due to invalid IL or missing references)
		//IL_03bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0438: Unknown result type (might be due to invalid IL or missing references)
		//IL_043d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0448: Unknown result type (might be due to invalid IL or missing references)
		//IL_044d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0453: Unknown result type (might be due to invalid IL or missing references)
		//IL_0458: Unknown result type (might be due to invalid IL or missing references)
		//IL_0463: Unknown result type (might be due to invalid IL or missing references)
		//IL_046e: Unknown result type (might be due to invalid IL or missing references)
		//IL_047f: Unknown result type (might be due to invalid IL or missing references)
		//IL_048a: Unknown result type (might be due to invalid IL or missing references)
		Vector2 pos = Vector2.Zero;
		drawinfo.drawPlayer.ApplyHeadOffsetFromMount(ref pos);
		bool flag = drawinfo.drawPlayer.head >= 0 && ArmorIDs.Head.Sets.HidesHead[drawinfo.drawPlayer.head];
		if (drawinfo.mountHandlesHeadDraw)
		{
			if (drawinfo.mountDrawsEyelid)
			{
				DrawPlayer_21_Head_TheFace_Eyelid(ref drawinfo);
			}
			if (drawinfo.drawPlayer.face > 0 && ArmorIDs.Face.Sets.DrawInFaceUnderHairLayer[drawinfo.drawPlayer.face] && (!drawinfo.drawPlayer.mount.Active || drawinfo.drawPlayer.mount.Type != 54 || ArmorIDs.Face.Sets.CanDrawOnVelociraptorMount[drawinfo.drawPlayer.face]))
			{
				DrawData item = new DrawData(TextureAssets.AccFace[drawinfo.drawPlayer.face].Value, pos + new Vector2((float)(int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.headPosition + drawinfo.headVect, drawinfo.drawPlayer.bodyFrame, drawinfo.colorArmorHead, drawinfo.drawPlayer.headRotation, drawinfo.headVect, 1f, drawinfo.playerEffect);
				item.shader = drawinfo.cFace;
				drawinfo.DrawDataCache.Add(item);
			}
		}
		else if (!flag && drawinfo.drawPlayer.faceHead > 0 && drawinfo.drawPlayer.faceHead < ArmorIDs.Face.Count)
		{
			Vector2 val = drawinfo.drawPlayer.GetFaceHeadOffsetFromHelmet() + pos;
			DrawData item = new DrawData(TextureAssets.AccFace[drawinfo.drawPlayer.faceHead].Value, new Vector2((float)(int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.headPosition + drawinfo.headVect + val, drawinfo.drawPlayer.bodyFrame, drawinfo.colorArmorHead, drawinfo.drawPlayer.headRotation, drawinfo.headVect, 1f, drawinfo.playerEffect);
			item.shader = drawinfo.cFaceHead;
			drawinfo.DrawDataCache.Add(item);
			if (drawinfo.drawPlayer.face <= 0 || !ArmorIDs.Face.Sets.DrawInFaceUnderHairLayer[drawinfo.drawPlayer.face] || (drawinfo.drawPlayer.mount.Active && drawinfo.drawPlayer.mount.Type == 54 && !ArmorIDs.Face.Sets.CanDrawOnVelociraptorMount[drawinfo.drawPlayer.face]))
			{
				return;
			}
			float num = 0f;
			if (drawinfo.drawPlayer.face == 5)
			{
				sbyte faceHead = drawinfo.drawPlayer.faceHead;
				if ((uint)(faceHead - 10) <= 3u)
				{
					num = 2 * drawinfo.drawPlayer.direction;
				}
			}
			item = new DrawData(TextureAssets.AccFace[drawinfo.drawPlayer.face].Value, pos + new Vector2((float)(int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)) + num, (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.headPosition + drawinfo.headVect, drawinfo.drawPlayer.bodyFrame, drawinfo.colorArmorHead, drawinfo.drawPlayer.headRotation, drawinfo.headVect, 1f, drawinfo.playerEffect);
			item.shader = drawinfo.cFace;
			drawinfo.DrawDataCache.Add(item);
		}
		else if (!drawinfo.drawPlayer.invis && !flag)
		{
			DrawData drawData = new DrawData(TextureAssets.Players[drawinfo.skinVar, 0].Value, new Vector2((float)(int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.headPosition + drawinfo.headVect, drawinfo.drawPlayer.bodyFrame, drawinfo.colorHead, drawinfo.drawPlayer.headRotation, drawinfo.headVect, 1f, drawinfo.playerEffect);
			drawData.shader = drawinfo.skinDyePacked;
			DrawData item = drawData;
			drawinfo.DrawDataCache.Add(item);
			item = new DrawData(TextureAssets.Players[drawinfo.skinVar, 1].Value, new Vector2((float)(int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.headPosition + drawinfo.headVect, drawinfo.drawPlayer.bodyFrame, drawinfo.colorEyeWhites, drawinfo.drawPlayer.headRotation, drawinfo.headVect, 1f, drawinfo.playerEffect);
			drawinfo.DrawDataCache.Add(item);
			item = new DrawData(TextureAssets.Players[drawinfo.skinVar, 2].Value, new Vector2((float)(int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.headPosition + drawinfo.headVect, drawinfo.drawPlayer.bodyFrame, drawinfo.colorEyes, drawinfo.drawPlayer.headRotation, drawinfo.headVect, 1f, drawinfo.playerEffect);
			drawinfo.DrawDataCache.Add(item);
			DrawPlayer_21_Head_TheFace_Eyelid(ref drawinfo);
			if (drawinfo.drawPlayer.yoraiz0rDarkness)
			{
				drawData = new DrawData(TextureAssets.Extra[67].Value, pos + new Vector2((float)(int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.headPosition + drawinfo.headVect, drawinfo.drawPlayer.bodyFrame, drawinfo.colorHead, drawinfo.drawPlayer.headRotation, drawinfo.headVect, 1f, drawinfo.playerEffect);
				drawData.shader = drawinfo.skinDyePacked;
				item = drawData;
				drawinfo.DrawDataCache.Add(item);
			}
			if (drawinfo.drawPlayer.face > 0 && ArmorIDs.Face.Sets.DrawInFaceUnderHairLayer[drawinfo.drawPlayer.face] && (!drawinfo.drawPlayer.mount.Active || drawinfo.drawPlayer.mount.Type != 54 || ArmorIDs.Face.Sets.CanDrawOnVelociraptorMount[drawinfo.drawPlayer.face]))
			{
				item = new DrawData(TextureAssets.AccFace[drawinfo.drawPlayer.face].Value, pos + new Vector2((float)(int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.headPosition + drawinfo.headVect, drawinfo.drawPlayer.bodyFrame, drawinfo.colorArmorHead, drawinfo.drawPlayer.headRotation, drawinfo.headVect, 1f, drawinfo.playerEffect);
				item.shader = drawinfo.cFace;
				drawinfo.DrawDataCache.Add(item);
			}
		}
	}

	private static void DrawPlayer_21_Head_TheFace_Eyelid(ref PlayerDrawSet drawinfo)
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Invalid comparison between Unknown and I4
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_016f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0174: Unknown result type (might be due to invalid IL or missing references)
		//IL_017e: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0207: Unknown result type (might be due to invalid IL or missing references)
		//IL_020c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0212: Unknown result type (might be due to invalid IL or missing references)
		//IL_0217: Unknown result type (might be due to invalid IL or missing references)
		//IL_021c: Unknown result type (might be due to invalid IL or missing references)
		//IL_021d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0222: Unknown result type (might be due to invalid IL or missing references)
		//IL_0229: Unknown result type (might be due to invalid IL or missing references)
		//IL_0237: Unknown result type (might be due to invalid IL or missing references)
		//IL_0242: Unknown result type (might be due to invalid IL or missing references)
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_0105: Unknown result type (might be due to invalid IL or missing references)
		//IL_010a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0115: Unknown result type (might be due to invalid IL or missing references)
		//IL_011a: Unknown result type (might be due to invalid IL or missing references)
		//IL_015d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0162: Unknown result type (might be due to invalid IL or missing references)
		Asset<Texture2D> val = TextureAssets.Players[drawinfo.skinVar, 15];
		if (val.IsLoaded)
		{
			Vector2 pos = Vector2.Zero;
			drawinfo.drawPlayer.ApplyHeadOffsetFromMount(ref pos);
			Vector2 val2 = Main.OffsetsPlayerHeadgear[drawinfo.drawPlayer.bodyFrame.Y / drawinfo.drawPlayer.bodyFrame.Height];
			val2.Y -= 2f;
			val2 *= (float)(-(((int)drawinfo.playerEffect & 2) > 0).ToDirectionInt());
			Color color = drawinfo.colorHead;
			int shader = drawinfo.skinDyePacked;
			int frameY = drawinfo.drawPlayer.eyeHelper.EyeFrameToShow;
			if (drawinfo.drawPlayer.mount.Active && drawinfo.drawPlayer.mount.Type == 54)
			{
				color = drawinfo.drawPlayer.GetImmuneAlpha(Lighting.GetColorClamped((int)drawinfo.drawPlayer.MountedCenter.X / 16, (int)drawinfo.drawPlayer.MountedCenter.Y / 16, new Color(158, 92, 67)), drawinfo.shadow);
				shader = drawinfo.drawPlayer.cMount;
			}
			if (drawinfo.drawPlayer.mount.Active && drawinfo.mountHandlesHeadDraw && drawinfo.mountDrawsEyelid && drawinfo.drawPlayer.head == 288)
			{
				color = Color.Black;
				frameY = 2;
			}
			Rectangle value = val.Frame(1, 3, 0, frameY);
			DrawData drawData = new DrawData(val.Value, pos + new Vector2((float)(int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.headPosition + drawinfo.headVect + val2, value, color, drawinfo.drawPlayer.headRotation, drawinfo.headVect, 1f, drawinfo.playerEffect);
			drawData.shader = shader;
			DrawData item = drawData;
			drawinfo.DrawDataCache.Add(item);
		}
	}

	public static void DrawPlayer_21_1_Magiluminescence(ref PlayerDrawSet drawinfo)
	{
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_011e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0129: Unknown result type (might be due to invalid IL or missing references)
		//IL_012e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0159: Unknown result type (might be due to invalid IL or missing references)
		//IL_015e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0169: Unknown result type (might be due to invalid IL or missing references)
		//IL_0173: Unknown result type (might be due to invalid IL or missing references)
		//IL_0180: Unknown result type (might be due to invalid IL or missing references)
		//IL_018b: Unknown result type (might be due to invalid IL or missing references)
		if (drawinfo.shadow == 0f && drawinfo.drawPlayer.neck == 11 && !drawinfo.hideEntirePlayer && !drawinfo.hideEntirePlayerExceptHelmetsAndFaceAccessories)
		{
			Color colorArmorBody = drawinfo.colorArmorBody;
			Color val = new Color(140, 140, 35, 12);
			float num = (float)(colorArmorBody.R + colorArmorBody.G + colorArmorBody.B) / 3f / 255f;
			val = Color.Lerp(val, Color.Transparent, num);
			if (!(val == Color.Transparent))
			{
				DrawData item = new DrawData(TextureAssets.GlowMask[310].Value, new Vector2((float)(int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.bodyPosition + new Vector2((float)(drawinfo.drawPlayer.bodyFrame.Width / 2), (float)(drawinfo.drawPlayer.bodyFrame.Height / 2)), drawinfo.drawPlayer.bodyFrame, val, drawinfo.drawPlayer.bodyRotation, drawinfo.bodyVect, 1f, drawinfo.playerEffect);
				item.shader = drawinfo.cNeck;
				drawinfo.DrawDataCache.Add(item);
			}
		}
	}

	public static void DrawPlayer_ChippysHeadband(ref PlayerDrawSet drawinfo)
	{
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_014a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0104: Unknown result type (might be due to invalid IL or missing references)
		//IL_010b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_027a: Unknown result type (might be due to invalid IL or missing references)
		//IL_027f: Unknown result type (might be due to invalid IL or missing references)
		//IL_028d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0298: Unknown result type (might be due to invalid IL or missing references)
		//IL_0245: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0335: Unknown result type (might be due to invalid IL or missing references)
		//IL_0346: Unknown result type (might be due to invalid IL or missing references)
		//IL_0351: Unknown result type (might be due to invalid IL or missing references)
		//IL_0356: Unknown result type (might be due to invalid IL or missing references)
		//IL_035b: Unknown result type (might be due to invalid IL or missing references)
		//IL_035d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0362: Unknown result type (might be due to invalid IL or missing references)
		//IL_0364: Unknown result type (might be due to invalid IL or missing references)
		//IL_0370: Unknown result type (might be due to invalid IL or missing references)
		//IL_037b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0380: Unknown result type (might be due to invalid IL or missing references)
		//IL_0385: Unknown result type (might be due to invalid IL or missing references)
		//IL_038a: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_03db: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_03fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0404: Unknown result type (might be due to invalid IL or missing references)
		//IL_040b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0413: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d3: Unknown result type (might be due to invalid IL or missing references)
		Texture2D value = TextureAssets.Extra[279].Value;
		float num = 0.4f;
		float num2 = 0.8f;
		float num3 = 160f;
		int num4 = 0;
		float num5 = 0f;
		Vector2 zero = Vector2.Zero;
		float num6 = 0f;
		float num7 = 0f;
		float num8 = 0f;
		float num10 = 0f;
		bool isVelociraptor = false;
		bool hasRockGolemHead = false;
		Vector2 val = DrawPlayer_GetMountOffsetForFaceAcc(ref drawinfo, ref isVelociraptor, ref hasRockGolemHead);
		Vector2 val2 = drawinfo.drawPlayer.GetFaceDrawOffset(drawinfo.drawPlayer.face);
		if (isVelociraptor & hasRockGolemHead)
		{
			val2 += drawinfo.drawPlayer.GetHelmetOffsetAddonFromMount();
		}
		val2 += val;
		int num11 = Math.Min(drawinfo.drawPlayer.availableAdvancedShadowsCount - 1, 30);
		float num12 = 0f;
		for (int num13 = num11; num13 > 0; num13--)
		{
			EntityShadowInfo advancedShadow = drawinfo.drawPlayer.GetAdvancedShadow(num13);
			EntityShadowInfo advancedShadow2 = drawinfo.drawPlayer.GetAdvancedShadow(num13 - 1);
			if (num13 == 1)
			{
				num10 = drawinfo.drawPlayer.position.AngleFrom(advancedShadow.Position);
			}
			num12 += Vector2.Distance(advancedShadow.Position, advancedShadow2.Position);
		}
		num6 = MathHelper.Clamp(num12 / num3, 0f, 1f);
		num7 = (0f - Main.WindForVisuals) * 0.45f * drawinfo.drawPlayer.Directions.Y;
		float num14 = Utils.MultiLerp((float)drawinfo.drawPlayer.miscCounter % 100f / 100f, 0.3f, 0.5f, 0.8f, 1f, 0.8f, 1f, 0.7f, 0.6f, 0.8f, 0.3f);
		num7 *= num14;
		num8 = num6;
		switch ((num8 >= num2) ? 2 : ((num8 >= num) ? 1 : 0))
		{
		default:
			num4 = 0;
			break;
		case 1:
		{
			float num15 = (float)drawinfo.drawPlayer.miscCounter % 24f / 24f;
			num4 = (int)Utils.Lerp(1.0, 6.0, num15);
			break;
		}
		case 2:
		{
			float num15 = (float)drawinfo.drawPlayer.miscCounter % 18f / 18f;
			num4 = (int)Utils.Lerp(7.0, 12.0, num15);
			break;
		}
		}
		num5 = num10;
		if (num5 != 0f && drawinfo.drawPlayer.Directions.X == -1f)
		{
			num5 += (float)Math.PI;
		}
		num5 += num7;
		num5 %= (float)Math.PI * 2f;
		Rectangle val3 = value.Frame(1, 13, 0, num4);
		Vector2 val4 = new Vector2(26f, 22f);
		if (drawinfo.drawPlayer.Directions.X == -1f)
		{
			val4.X = 46f;
		}
		if (drawinfo.drawPlayer.Directions.Y == -1f)
		{
			val4.Y = 56f - val4.Y;
		}
		Vector2 val5 = new Vector2((float)(int)(drawinfo.Position.X - Main.screenPosition.X - (float)(val3.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)val3.Height + 4f)) + drawinfo.drawPlayer.headPosition + val4;
		val2 += new Vector2(1f, -2f) * drawinfo.drawPlayer.Directions;
		Vector2 val6 = Main.OffsetsPlayerHeadgear[drawinfo.drawPlayer.bodyFrame.Y / drawinfo.drawPlayer.bodyFrame.Height];
		val6.Y -= 2f;
		val6 *= drawinfo.drawPlayer.Directions.Y;
		DrawData item = new DrawData(value, val2 + zero + val5 + val6, val3, drawinfo.colorArmorHead, num5, val4, 1f, drawinfo.playerEffect);
		item.shader = drawinfo.cFace;
		drawinfo.DrawDataCache.Add(item);
	}

	public static Vector2 DrawPlayer_GetMountOffsetForFaceAcc(ref PlayerDrawSet drawinfo, ref bool isVelociraptor, ref bool hasRockGolemHead)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		Vector2 pos = Vector2.Zero;
		isVelociraptor = false;
		hasRockGolemHead = drawinfo.drawPlayer.head == 241;
		if (drawinfo.drawPlayer.mount.Active)
		{
			switch (drawinfo.drawPlayer.mount.Type)
			{
			case 52:
				pos = new Vector2(28f, -2f) * drawinfo.drawPlayer.Directions;
				break;
			case 54:
				drawinfo.drawPlayer.ApplyHeadOffsetFromMount(ref pos);
				pos -= drawinfo.drawPlayer.GetHelmetOffsetAddonFromMount();
				isVelociraptor = true;
				break;
			case 55:
			case 56:
			case 61:
				drawinfo.drawPlayer.ApplyHeadOffsetFromMount(ref pos);
				pos -= drawinfo.drawPlayer.GetHelmetOffsetAddonFromMount();
				break;
			}
		}
		return pos;
	}

	public static void DrawPlayer_22_FaceAcc(ref PlayerDrawSet drawinfo)
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_029c: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0155: Unknown result type (might be due to invalid IL or missing references)
		//IL_015a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0165: Unknown result type (might be due to invalid IL or missing references)
		//IL_016a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0170: Unknown result type (might be due to invalid IL or missing references)
		//IL_0175: Unknown result type (might be due to invalid IL or missing references)
		//IL_0180: Unknown result type (might be due to invalid IL or missing references)
		//IL_018b: Unknown result type (might be due to invalid IL or missing references)
		//IL_019c: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_035f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0364: Unknown result type (might be due to invalid IL or missing references)
		//IL_036f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0374: Unknown result type (might be due to invalid IL or missing references)
		//IL_037a: Unknown result type (might be due to invalid IL or missing references)
		//IL_037f: Unknown result type (might be due to invalid IL or missing references)
		//IL_038a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0395: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_0493: Unknown result type (might be due to invalid IL or missing references)
		//IL_0498: Unknown result type (might be due to invalid IL or missing references)
		//IL_05dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_04cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_04cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0556: Unknown result type (might be due to invalid IL or missing references)
		//IL_055b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0566: Unknown result type (might be due to invalid IL or missing references)
		//IL_056b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0571: Unknown result type (might be due to invalid IL or missing references)
		//IL_0576: Unknown result type (might be due to invalid IL or missing references)
		//IL_0581: Unknown result type (might be due to invalid IL or missing references)
		//IL_058c: Unknown result type (might be due to invalid IL or missing references)
		//IL_059d: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_049f: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0714: Unknown result type (might be due to invalid IL or missing references)
		//IL_0719: Unknown result type (might be due to invalid IL or missing references)
		//IL_060e: Unknown result type (might be due to invalid IL or missing references)
		//IL_060f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0611: Unknown result type (might be due to invalid IL or missing references)
		//IL_068e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0693: Unknown result type (might be due to invalid IL or missing references)
		//IL_069e: Unknown result type (might be due to invalid IL or missing references)
		//IL_06a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_06a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_06b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_06c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_06d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_06e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_05fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0772: Unknown result type (might be due to invalid IL or missing references)
		//IL_077d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0782: Unknown result type (might be due to invalid IL or missing references)
		//IL_0784: Unknown result type (might be due to invalid IL or missing references)
		//IL_0791: Unknown result type (might be due to invalid IL or missing references)
		//IL_0796: Unknown result type (might be due to invalid IL or missing references)
		//IL_07b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_07b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_07b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0831: Unknown result type (might be due to invalid IL or missing references)
		//IL_0836: Unknown result type (might be due to invalid IL or missing references)
		//IL_0841: Unknown result type (might be due to invalid IL or missing references)
		//IL_0846: Unknown result type (might be due to invalid IL or missing references)
		//IL_084c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0851: Unknown result type (might be due to invalid IL or missing references)
		//IL_085c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0866: Unknown result type (might be due to invalid IL or missing references)
		//IL_0874: Unknown result type (might be due to invalid IL or missing references)
		//IL_087f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0720: Unknown result type (might be due to invalid IL or missing references)
		//IL_0728: Unknown result type (might be due to invalid IL or missing references)
		//IL_0737: Unknown result type (might be due to invalid IL or missing references)
		//IL_0742: Unknown result type (might be due to invalid IL or missing references)
		//IL_0747: Unknown result type (might be due to invalid IL or missing references)
		//IL_074c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0751: Unknown result type (might be due to invalid IL or missing references)
		//IL_0756: Unknown result type (might be due to invalid IL or missing references)
		bool isVelociraptor = false;
		bool hasRockGolemHead = false;
		bool flag = drawinfo.drawPlayer.head == 287;
		Vector2 val = DrawPlayer_GetMountOffsetForFaceAcc(ref drawinfo, ref isVelociraptor, ref hasRockGolemHead);
		if (drawinfo.drawPlayer.face > 0 && drawinfo.drawPlayer.face < ArmorIDs.Face.Count && !ArmorIDs.Face.Sets.DrawInFaceUnderHairLayer[drawinfo.drawPlayer.face] && (!isVelociraptor || ArmorIDs.Face.Sets.CanDrawOnVelociraptorMount[drawinfo.drawPlayer.face]) && (drawinfo.drawPlayer.face != 23 || !flag))
		{
			Vector2 val2 = drawinfo.drawPlayer.GetFaceDrawOffset(drawinfo.drawPlayer.face);
			if (isVelociraptor & hasRockGolemHead)
			{
				val2 += drawinfo.drawPlayer.GetHelmetOffsetAddonFromMount();
			}
			DrawData item = new DrawData(TextureAssets.AccFace[drawinfo.drawPlayer.face].Value, val2 + val + new Vector2((float)(int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.headPosition + drawinfo.headVect, drawinfo.drawPlayer.bodyFrame, drawinfo.colorArmorHead, drawinfo.drawPlayer.headRotation, drawinfo.headVect, 1f, drawinfo.playerEffect);
			item.shader = drawinfo.cFace;
			drawinfo.DrawDataCache.Add(item);
		}
		bool flag2 = drawinfo.drawPlayer.faceMask > 0 && drawinfo.drawPlayer.faceMask < ArmorIDs.Face.Count;
		if (flag2 && drawinfo.drawPlayer.head > 0 && drawinfo.drawPlayer.head < ArmorIDs.Head.Count && (ArmorIDs.Head.Sets.PreventFaceMaskDraw[drawinfo.drawPlayer.head] || ArmorIDs.Head.Sets.DrawFaceMaskUnderHeadLayer[drawinfo.drawPlayer.head]))
		{
			flag2 = false;
		}
		else if (flag2 && drawinfo.drawPlayer.mount.Active && drawinfo.drawPlayer.mount.Type == 54 && !ArmorIDs.Face.Sets.CanDrawOnVelociraptorMount[drawinfo.drawPlayer.faceMask])
		{
			flag2 = false;
		}
		if (flag2)
		{
			Vector2 val3 = drawinfo.drawPlayer.GetFaceDrawOffset(drawinfo.drawPlayer.faceMask);
			if (isVelociraptor & hasRockGolemHead)
			{
				val3 += drawinfo.drawPlayer.GetHelmetOffsetAddonFromMount();
			}
			DrawData item = new DrawData(TextureAssets.AccFace[drawinfo.drawPlayer.faceMask].Value, val3 + val + drawinfo.helmetOffset + new Vector2((float)(int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.headPosition + drawinfo.headVect, drawinfo.drawPlayer.bodyFrame, drawinfo.colorArmorHead, drawinfo.drawPlayer.headRotation, drawinfo.headVect, 1f, drawinfo.playerEffect);
			item.shader = drawinfo.cFaceMask;
			drawinfo.DrawDataCache.Add(item);
		}
		bool flag3 = drawinfo.drawPlayer.faceFlower > 0 && drawinfo.drawPlayer.faceFlower < ArmorIDs.Face.Count;
		if (flag3 && drawinfo.drawPlayer.head > 0 && drawinfo.drawPlayer.head < ArmorIDs.Head.Count && ArmorIDs.Head.Sets.PreventFaceFlowerDraw[drawinfo.drawPlayer.head])
		{
			flag3 = false;
		}
		else if (flag3 && drawinfo.drawPlayer.mount.Active && drawinfo.drawPlayer.mount.Type == 54 && !ArmorIDs.Face.Sets.CanDrawOnVelociraptorMount[drawinfo.drawPlayer.faceFlower])
		{
			flag3 = false;
		}
		if (flag3)
		{
			Vector2 val4 = drawinfo.drawPlayer.GetFaceDrawOffset(drawinfo.drawPlayer.faceFlower);
			if (isVelociraptor & hasRockGolemHead)
			{
				val4 += drawinfo.drawPlayer.GetHelmetOffsetAddonFromMount();
			}
			DrawData item = new DrawData(TextureAssets.AccFace[drawinfo.drawPlayer.faceFlower].Value, val4 + val + drawinfo.helmetOffset + new Vector2((float)(int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.headPosition + drawinfo.headVect, drawinfo.drawPlayer.bodyFrame, drawinfo.colorArmorHead, drawinfo.drawPlayer.headRotation, drawinfo.headVect, 1f, drawinfo.playerEffect);
			item.shader = drawinfo.cFaceFlower;
			drawinfo.DrawDataCache.Add(item);
		}
		if (drawinfo.drawUnicornHorn)
		{
			Vector2 val5 = Vector2.Zero;
			if (isVelociraptor & hasRockGolemHead)
			{
				val5 += drawinfo.drawPlayer.GetHelmetOffsetAddonFromMount();
			}
			DrawData item = new DrawData(TextureAssets.Extra[143].Value, val + val5 + new Vector2((float)(int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.headPosition + drawinfo.headVect, drawinfo.drawPlayer.bodyFrame, drawinfo.colorArmorHead, drawinfo.drawPlayer.headRotation, drawinfo.headVect, 1f, drawinfo.playerEffect);
			item.shader = drawinfo.cUnicornHorn;
			drawinfo.DrawDataCache.Add(item);
		}
		if (drawinfo.drawAngelHalo)
		{
			Vector2 val6 = Vector2.Zero;
			if (isVelociraptor & hasRockGolemHead)
			{
				val6 += drawinfo.drawPlayer.GetHelmetOffsetAddonFromMount() + new Vector2(-4f, -14f) * drawinfo.drawPlayer.Directions;
			}
			Color immuneAlphaPure = drawinfo.drawPlayer.GetImmuneAlphaPure(new Color(200, 200, 200, 150), drawinfo.shadow);
			immuneAlphaPure *= drawinfo.drawPlayer.stealth;
			Main.instance.LoadAccFace(7);
			DrawData item = new DrawData(TextureAssets.AccFace[7].Value, val + val6 + new Vector2((float)(int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.headPosition + drawinfo.headVect, drawinfo.drawPlayer.bodyFrame, immuneAlphaPure, drawinfo.drawPlayer.headRotation, drawinfo.headVect, 1f, drawinfo.playerEffect);
			item.shader = drawinfo.cAngelHalo;
			drawinfo.DrawDataCache.Add(item);
		}
	}

	public static void DrawTiedBalloons(ref PlayerDrawSet drawinfo)
	{
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
		if (drawinfo.drawPlayer.mount.Type == 34)
		{
			Texture2D value = TextureAssets.Extra[141].Value;
			Vector2 val = new Vector2(0f, 4f);
			Color colorMount = drawinfo.colorMount;
			int frameY = (int)(Main.GlobalTimeWrappedHourly * 3f + drawinfo.drawPlayer.position.X / 50f) % 3;
			Rectangle val2 = value.Frame(1, 3, 0, frameY);
			Vector2 origin = new Vector2((float)(val2.Width / 2), (float)val2.Height);
			float rotation = (0f - drawinfo.drawPlayer.velocity.X) * 0.1f - drawinfo.drawPlayer.fullRotation;
			DrawData item = new DrawData(value, drawinfo.drawPlayer.MountedCenter + val - Main.screenPosition, val2, colorMount, rotation, origin, 1f, drawinfo.playerEffect);
			drawinfo.DrawDataCache.Add(item);
		}
	}

	public static void DrawStarboardRainbowTrail(ref PlayerDrawSet drawinfo, Vector2 commonWingPosPreFloor, Vector2 dirsVec)
	{
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0105: Unknown result type (might be due to invalid IL or missing references)
		//IL_0114: Unknown result type (might be due to invalid IL or missing references)
		//IL_0119: Unknown result type (might be due to invalid IL or missing references)
		//IL_0128: Unknown result type (might be due to invalid IL or missing references)
		//IL_012d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0132: Unknown result type (might be due to invalid IL or missing references)
		//IL_0134: Unknown result type (might be due to invalid IL or missing references)
		//IL_0147: Unknown result type (might be due to invalid IL or missing references)
		//IL_0156: Unknown result type (might be due to invalid IL or missing references)
		//IL_015b: Unknown result type (might be due to invalid IL or missing references)
		//IL_016a: Unknown result type (might be due to invalid IL or missing references)
		//IL_016f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0174: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0224: Unknown result type (might be due to invalid IL or missing references)
		//IL_022b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0248: Unknown result type (might be due to invalid IL or missing references)
		//IL_028a: Unknown result type (might be due to invalid IL or missing references)
		//IL_028e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0294: Unknown result type (might be due to invalid IL or missing references)
		//IL_0299: Unknown result type (might be due to invalid IL or missing references)
		//IL_029b: Unknown result type (might be due to invalid IL or missing references)
		//IL_029d: Unknown result type (might be due to invalid IL or missing references)
		//IL_02af: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_0303: Unknown result type (might be due to invalid IL or missing references)
		//IL_0305: Unknown result type (might be due to invalid IL or missing references)
		//IL_0309: Unknown result type (might be due to invalid IL or missing references)
		//IL_030e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0313: Unknown result type (might be due to invalid IL or missing references)
		//IL_0322: Unknown result type (might be due to invalid IL or missing references)
		//IL_0326: Unknown result type (might be due to invalid IL or missing references)
		//IL_0328: Unknown result type (might be due to invalid IL or missing references)
		//IL_032b: Unknown result type (might be due to invalid IL or missing references)
		if (drawinfo.shadow != 0f)
		{
			return;
		}
		int num = Math.Min(drawinfo.drawPlayer.availableAdvancedShadowsCount - 1, 30);
		float num2 = 0f;
		for (int num3 = num; num3 > 0; num3--)
		{
			EntityShadowInfo advancedShadow = drawinfo.drawPlayer.GetAdvancedShadow(num3);
			EntityShadowInfo advancedShadow2 = drawinfo.drawPlayer.GetAdvancedShadow(num3 - 1);
			num2 += Vector2.Distance(advancedShadow.Position, advancedShadow2.Position);
		}
		float num4 = MathHelper.Clamp(num2 / 160f, 0f, 1f);
		Main.instance.LoadProjectile(250);
		Texture2D value = TextureAssets.Projectile[250].Value;
		float num5 = 1.7f;
		Vector2 origin = new Vector2((float)(value.Width / 2), (float)(value.Height / 2));
		Vector2 val = new Vector2((float)drawinfo.drawPlayer.width, (float)drawinfo.drawPlayer.height) / 2f;
		Color white = Color.White;
		white.A = 64;
		Vector2 val2 = val;
		val2 = drawinfo.drawPlayer.DefaultSize * new Vector2(0.5f, 1f) + new Vector2(0f, -4f);
		if (dirsVec.Y < 0f)
		{
			val2 = drawinfo.drawPlayer.DefaultSize * new Vector2(0.5f, 0f) + new Vector2(0f, 4f);
		}
		for (int num6 = num; num6 > 0; num6--)
		{
			EntityShadowInfo advancedShadow3 = drawinfo.drawPlayer.GetAdvancedShadow(num6);
			EntityShadowInfo advancedShadow4 = drawinfo.drawPlayer.GetAdvancedShadow(num6 - 1);
			Vector2 pos = advancedShadow3.Position + val2 + advancedShadow3.HeadgearOffset;
			Vector2 pos2 = advancedShadow4.Position + val2 + advancedShadow4.HeadgearOffset;
			pos = drawinfo.drawPlayer.RotatedRelativePoint(pos, reverseRotation: true, addGfxOffY: false);
			pos2 = drawinfo.drawPlayer.RotatedRelativePoint(pos2, reverseRotation: true, addGfxOffY: false);
			float num7 = (pos2 - pos).ToRotation() - (float)Math.PI / 2f;
			num7 = (float)Math.PI / 2f * (float)drawinfo.drawPlayer.direction;
			float num8 = Math.Abs(pos2.X - pos.X);
			Vector2 scale = new Vector2(num5, num8 / (float)value.Height);
			float num9 = 1f - (float)num6 / (float)num;
			num9 *= num9;
			num9 *= Utils.GetLerpValue(0f, 4f, num8, clamped: true);
			num9 *= 0.5f;
			num9 *= num9;
			Color val3 = white * num9 * num4;
			if (!(val3 == Color.Transparent))
			{
				DrawData item = new DrawData(value, pos - Main.screenPosition, null, val3, num7, origin, scale, drawinfo.playerEffect);
				item.shader = drawinfo.cWings;
				drawinfo.DrawDataCache.Add(item);
				for (float num10 = 0.25f; num10 < 1f; num10 += 0.25f)
				{
					item = new DrawData(value, Vector2.Lerp(pos, pos2, num10) - Main.screenPosition, null, val3, num7, origin, scale, drawinfo.playerEffect);
					item.shader = drawinfo.cWings;
					drawinfo.DrawDataCache.Add(item);
				}
			}
		}
	}

	public static void DrawMeowcartTrail(ref PlayerDrawSet drawinfo)
	{
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0115: Unknown result type (might be due to invalid IL or missing references)
		//IL_011a: Unknown result type (might be due to invalid IL or missing references)
		//IL_011f: Unknown result type (might be due to invalid IL or missing references)
		//IL_012a: Unknown result type (might be due to invalid IL or missing references)
		//IL_012c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0131: Unknown result type (might be due to invalid IL or missing references)
		//IL_0133: Unknown result type (might be due to invalid IL or missing references)
		//IL_0138: Unknown result type (might be due to invalid IL or missing references)
		//IL_0146: Unknown result type (might be due to invalid IL or missing references)
		//IL_014b: Unknown result type (might be due to invalid IL or missing references)
		//IL_014d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0152: Unknown result type (might be due to invalid IL or missing references)
		//IL_0154: Unknown result type (might be due to invalid IL or missing references)
		//IL_015c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0167: Unknown result type (might be due to invalid IL or missing references)
		//IL_016c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0171: Unknown result type (might be due to invalid IL or missing references)
		//IL_019b: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0207: Unknown result type (might be due to invalid IL or missing references)
		//IL_0209: Unknown result type (might be due to invalid IL or missing references)
		//IL_0220: Unknown result type (might be due to invalid IL or missing references)
		//IL_023a: Unknown result type (might be due to invalid IL or missing references)
		//IL_023e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0244: Unknown result type (might be due to invalid IL or missing references)
		//IL_0249: Unknown result type (might be due to invalid IL or missing references)
		//IL_024e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0250: Unknown result type (might be due to invalid IL or missing references)
		//IL_0255: Unknown result type (might be due to invalid IL or missing references)
		//IL_0264: Unknown result type (might be due to invalid IL or missing references)
		//IL_0268: Unknown result type (might be due to invalid IL or missing references)
		//IL_026a: Unknown result type (might be due to invalid IL or missing references)
		//IL_026d: Unknown result type (might be due to invalid IL or missing references)
		if (drawinfo.drawPlayer.mount.Type == 33 && !(drawinfo.shadow > 0f))
		{
			int num = Math.Min(drawinfo.drawPlayer.availableAdvancedShadowsCount - 1, 20);
			float num2 = 0f;
			for (int num3 = num; num3 > 0; num3--)
			{
				EntityShadowInfo advancedShadow = drawinfo.drawPlayer.GetAdvancedShadow(num3);
				EntityShadowInfo advancedShadow2 = drawinfo.drawPlayer.GetAdvancedShadow(num3 - 1);
				num2 += Vector2.Distance(advancedShadow.Position, advancedShadow2.Position);
			}
			float num4 = MathHelper.Clamp(num2 / 160f, 0f, 1f);
			Main.instance.LoadProjectile(250);
			Texture2D value = TextureAssets.Projectile[250].Value;
			float num5 = 1.5f;
			Vector2 origin = new Vector2((float)(value.Width / 2), 0f);
			Vector2 val = new Vector2((float)drawinfo.drawPlayer.width, (float)drawinfo.drawPlayer.height) / 2f;
			Vector2 val2 = new Vector2((float)(-drawinfo.drawPlayer.direction * 10), 15f);
			Color white = Color.White;
			white.A = 127;
			Vector2 val3 = val + val2;
			val3 = Vector2.Zero;
			Vector2 val4 = drawinfo.drawPlayer.RotatedRelativePoint(drawinfo.drawPlayer.Center + val3 + val2) - drawinfo.drawPlayer.position;
			for (int num6 = num; num6 > 0; num6--)
			{
				EntityShadowInfo advancedShadow3 = drawinfo.drawPlayer.GetAdvancedShadow(num6);
				EntityShadowInfo advancedShadow4 = drawinfo.drawPlayer.GetAdvancedShadow(num6 - 1);
				Vector2 val5 = advancedShadow3.Position + val3;
				Vector2 val6 = advancedShadow4.Position + val3;
				val5 += val4;
				val6 += val4;
				val5 = drawinfo.drawPlayer.RotatedRelativePoint(val5, reverseRotation: true, addGfxOffY: false);
				val6 = drawinfo.drawPlayer.RotatedRelativePoint(val6, reverseRotation: true, addGfxOffY: false);
				float rotation = (val6 - val5).ToRotation() - (float)Math.PI / 2f;
				float num7 = Vector2.Distance(val5, val6);
				Vector2 scale = new Vector2(num5, num7 / (float)value.Height);
				float num8 = 1f - (float)num6 / (float)num;
				num8 *= num8;
				Color color = white * num8 * num4;
				DrawData item = new DrawData(value, val5 - Main.screenPosition, null, color, rotation, origin, scale, drawinfo.playerEffect);
				drawinfo.DrawDataCache.Add(item);
			}
		}
	}

	public static void DrawPlayer_23_MountFront(ref PlayerDrawSet drawinfo)
	{
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		if (!drawinfo.drawPlayer.mount.Active)
		{
			return;
		}
		drawinfo.drawPlayer.mount.Draw(drawinfo.DrawDataCache, 2, drawinfo.drawPlayer, drawinfo.Position, drawinfo.colorMount, drawinfo.playerEffect, drawinfo.shadow);
		if (drawinfo.mountHandlesHeadDraw)
		{
			DrawPlayer_21_Head(ref drawinfo);
			DrawPlayer_22_FaceAcc(ref drawinfo);
			if (drawinfo.drawFrontAccInNeckAccLayer)
			{
				DrawPlayer_extra_TorsoMinus(ref drawinfo);
				DrawPlayer_32_FrontAcc_FrontPart(ref drawinfo);
				DrawPlayer_extra_TorsoPlus(ref drawinfo);
			}
		}
		if (drawinfo.drawPlayer.mount.Type != 54)
		{
			drawinfo.drawPlayer.mount.Draw(drawinfo.DrawDataCache, 3, drawinfo.drawPlayer, drawinfo.Position, drawinfo.colorMount, drawinfo.playerEffect, drawinfo.shadow);
		}
	}

	public static void DrawPlayer_24_Pulley(ref PlayerDrawSet drawinfo)
	{
		//IL_0213: Unknown result type (might be due to invalid IL or missing references)
		//IL_0247: Unknown result type (might be due to invalid IL or missing references)
		//IL_0252: Unknown result type (might be due to invalid IL or missing references)
		//IL_0273: Unknown result type (might be due to invalid IL or missing references)
		//IL_027e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		//IL_0114: Unknown result type (might be due to invalid IL or missing references)
		//IL_0134: Unknown result type (might be due to invalid IL or missing references)
		//IL_013f: Unknown result type (might be due to invalid IL or missing references)
		if (drawinfo.drawPlayer.pulley && drawinfo.drawPlayer.itemAnimation == 0)
		{
			if (drawinfo.drawPlayer.pulleyDir == 2)
			{
				int num = -25;
				int num2 = 0;
				float rotation = 0f;
				DrawData item = new DrawData(TextureAssets.Pulley.Value, new Vector2((float)((int)(drawinfo.Position.X - Main.screenPosition.X + (float)(drawinfo.drawPlayer.width / 2) - (float)(9 * drawinfo.drawPlayer.direction)) + num2 * drawinfo.drawPlayer.direction), (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)(drawinfo.drawPlayer.height / 2) + 2f * drawinfo.drawPlayer.gravDir + (float)num * drawinfo.drawPlayer.gravDir)), new Rectangle(0, TextureAssets.Pulley.Height() / 2 * drawinfo.drawPlayer.pulleyFrame, TextureAssets.Pulley.Width(), TextureAssets.Pulley.Height() / 2), drawinfo.colorArmorHead, rotation, new Vector2((float)(TextureAssets.Pulley.Width() / 2), (float)(TextureAssets.Pulley.Height() / 4)), 1f, drawinfo.playerEffect);
				drawinfo.DrawDataCache.Add(item);
			}
			else
			{
				int num3 = -26;
				int num4 = 10;
				float rotation2 = 0.35f * (float)(-drawinfo.drawPlayer.direction);
				DrawData item = new DrawData(TextureAssets.Pulley.Value, new Vector2((float)((int)(drawinfo.Position.X - Main.screenPosition.X + (float)(drawinfo.drawPlayer.width / 2) - (float)(9 * drawinfo.drawPlayer.direction)) + num4 * drawinfo.drawPlayer.direction), (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)(drawinfo.drawPlayer.height / 2) + 2f * drawinfo.drawPlayer.gravDir + (float)num3 * drawinfo.drawPlayer.gravDir)), new Rectangle(0, TextureAssets.Pulley.Height() / 2 * drawinfo.drawPlayer.pulleyFrame, TextureAssets.Pulley.Width(), TextureAssets.Pulley.Height() / 2), drawinfo.colorArmorHead, rotation2, new Vector2((float)(TextureAssets.Pulley.Width() / 2), (float)(TextureAssets.Pulley.Height() / 4)), 1f, drawinfo.playerEffect);
				drawinfo.DrawDataCache.Add(item);
			}
		}
	}

	public static void DrawPlayer_25_Shield(ref PlayerDrawSet drawinfo)
	{
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_0345: Unknown result type (might be due to invalid IL or missing references)
		//IL_034a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0355: Unknown result type (might be due to invalid IL or missing references)
		//IL_035a: Unknown result type (might be due to invalid IL or missing references)
		//IL_035f: Unknown result type (might be due to invalid IL or missing references)
		//IL_037b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0380: Unknown result type (might be due to invalid IL or missing references)
		//IL_0385: Unknown result type (might be due to invalid IL or missing references)
		//IL_0386: Unknown result type (might be due to invalid IL or missing references)
		//IL_038b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0392: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0137: Unknown result type (might be due to invalid IL or missing references)
		//IL_013c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0146: Unknown result type (might be due to invalid IL or missing references)
		//IL_0156: Unknown result type (might be due to invalid IL or missing references)
		//IL_015b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_041d: Unknown result type (might be due to invalid IL or missing references)
		//IL_042d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0432: Unknown result type (might be due to invalid IL or missing references)
		//IL_044c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0463: Unknown result type (might be due to invalid IL or missing references)
		//IL_04bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_04cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_04fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_04fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0501: Unknown result type (might be due to invalid IL or missing references)
		//IL_0507: Unknown result type (might be due to invalid IL or missing references)
		//IL_0514: Unknown result type (might be due to invalid IL or missing references)
		//IL_051c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0181: Unknown result type (might be due to invalid IL or missing references)
		//IL_0198: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0200: Unknown result type (might be due to invalid IL or missing references)
		//IL_0205: Unknown result type (might be due to invalid IL or missing references)
		//IL_020a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0226: Unknown result type (might be due to invalid IL or missing references)
		//IL_022b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0230: Unknown result type (might be due to invalid IL or missing references)
		//IL_0231: Unknown result type (might be due to invalid IL or missing references)
		//IL_023d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0253: Unknown result type (might be due to invalid IL or missing references)
		//IL_0259: Unknown result type (might be due to invalid IL or missing references)
		//IL_025b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0260: Unknown result type (might be due to invalid IL or missing references)
		//IL_0265: Unknown result type (might be due to invalid IL or missing references)
		//IL_026b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0278: Unknown result type (might be due to invalid IL or missing references)
		//IL_0280: Unknown result type (might be due to invalid IL or missing references)
		//IL_0577: Unknown result type (might be due to invalid IL or missing references)
		//IL_058e: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_05eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_05fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0600: Unknown result type (might be due to invalid IL or missing references)
		//IL_061c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0621: Unknown result type (might be due to invalid IL or missing references)
		//IL_0626: Unknown result type (might be due to invalid IL or missing references)
		//IL_0627: Unknown result type (might be due to invalid IL or missing references)
		//IL_062c: Unknown result type (might be due to invalid IL or missing references)
		//IL_062f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0634: Unknown result type (might be due to invalid IL or missing references)
		//IL_063e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0649: Unknown result type (might be due to invalid IL or missing references)
		//IL_0653: Unknown result type (might be due to invalid IL or missing references)
		//IL_0658: Unknown result type (might be due to invalid IL or missing references)
		//IL_065d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0662: Unknown result type (might be due to invalid IL or missing references)
		//IL_0667: Unknown result type (might be due to invalid IL or missing references)
		//IL_0669: Unknown result type (might be due to invalid IL or missing references)
		//IL_066b: Unknown result type (might be due to invalid IL or missing references)
		//IL_066d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0672: Unknown result type (might be due to invalid IL or missing references)
		//IL_0674: Unknown result type (might be due to invalid IL or missing references)
		//IL_0676: Unknown result type (might be due to invalid IL or missing references)
		//IL_067a: Unknown result type (might be due to invalid IL or missing references)
		//IL_067f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0684: Unknown result type (might be due to invalid IL or missing references)
		//IL_06a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_06b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_06b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_06cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_06d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_06d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_06e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_06e9: Unknown result type (might be due to invalid IL or missing references)
		if (drawinfo.drawPlayer.shield <= 0 || drawinfo.drawPlayer.shield >= ArmorIDs.Shield.Count)
		{
			return;
		}
		Vector2 zero = Vector2.Zero;
		if (drawinfo.drawPlayer.shieldRaised)
		{
			zero.Y -= 4f * drawinfo.drawPlayer.gravDir;
		}
		Rectangle bodyFrame = drawinfo.drawPlayer.bodyFrame;
		Vector2 zero2 = Vector2.Zero;
		Vector2 bodyVect = drawinfo.bodyVect;
		if (bodyFrame.Width != TextureAssets.AccShield[drawinfo.drawPlayer.shield].Value.Width)
		{
			bodyFrame.Width = TextureAssets.AccShield[drawinfo.drawPlayer.shield].Value.Width;
			bodyVect.X += bodyFrame.Width - TextureAssets.AccShield[drawinfo.drawPlayer.shield].Value.Width;
			if (((int)drawinfo.playerEffect & 1) != 0)
			{
				bodyVect.X = (float)bodyFrame.Width - bodyVect.X;
			}
		}
		DrawData item;
		if (drawinfo.drawPlayer.shieldRaised)
		{
			float num = (float)Math.Sin(Main.GlobalTimeWrappedHourly * ((float)Math.PI * 2f));
			float num2 = 2.5f + 1.5f * num;
			Color val = drawinfo.colorArmorBody;
			val.A = 0;
			val *= 0.45f - num * 0.15f;
			for (float num3 = 0f; num3 < 4f; num3++)
			{
				item = new DrawData(TextureAssets.AccShield[drawinfo.drawPlayer.shield].Value, zero2 + new Vector2((float)(int)(drawinfo.Position.X - Main.screenPosition.X - (float)(bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.bodyPosition + new Vector2((float)(bodyFrame.Width / 2), (float)(drawinfo.drawPlayer.bodyFrame.Height / 2)) + zero + new Vector2(num2, 0f).RotatedBy(num3 / 4f * ((float)Math.PI * 2f)), bodyFrame, val, drawinfo.drawPlayer.bodyRotation, bodyVect, 1f, drawinfo.playerEffect);
				item.shader = drawinfo.cShield;
				drawinfo.DrawDataCache.Add(item);
			}
		}
		item = new DrawData(TextureAssets.AccShield[drawinfo.drawPlayer.shield].Value, zero2 + new Vector2((float)(int)(drawinfo.Position.X - Main.screenPosition.X - (float)(bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.bodyPosition + new Vector2((float)(bodyFrame.Width / 2), (float)(drawinfo.drawPlayer.bodyFrame.Height / 2)) + zero, bodyFrame, drawinfo.colorArmorBody, drawinfo.drawPlayer.bodyRotation, bodyVect, 1f, drawinfo.playerEffect);
		item.shader = drawinfo.cShield;
		drawinfo.DrawDataCache.Add(item);
		if (drawinfo.drawPlayer.shieldRaised)
		{
			Color val2 = drawinfo.colorArmorBody;
			float num4 = (float)Math.Sin(Main.GlobalTimeWrappedHourly * (float)Math.PI);
			val2.A = (byte)((float)(int)val2.A * (0.5f + 0.5f * num4));
			val2 *= 0.5f + 0.5f * num4;
			item = new DrawData(TextureAssets.AccShield[drawinfo.drawPlayer.shield].Value, zero2 + new Vector2((float)(int)(drawinfo.Position.X - Main.screenPosition.X - (float)(bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.bodyPosition + new Vector2((float)(bodyFrame.Width / 2), (float)(drawinfo.drawPlayer.bodyFrame.Height / 2)) + zero, bodyFrame, val2, drawinfo.drawPlayer.bodyRotation, bodyVect, 1f, drawinfo.playerEffect);
			item.shader = drawinfo.cShield;
		}
		if (drawinfo.drawPlayer.shieldRaised && drawinfo.drawPlayer.shieldParryTimeLeft > 0)
		{
			float num5 = (float)drawinfo.drawPlayer.shieldParryTimeLeft / 20f;
			float num6 = 1.5f * num5;
			Vector2 val3 = zero2 + new Vector2((float)(int)(drawinfo.Position.X - Main.screenPosition.X - (float)(bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.bodyPosition + new Vector2((float)(bodyFrame.Width / 2), (float)(drawinfo.drawPlayer.bodyFrame.Height / 2)) + zero;
			Color val4 = drawinfo.colorArmorBody;
			float num7 = 1f;
			Vector2 val5 = drawinfo.Position + drawinfo.drawPlayer.Size / 2f - Main.screenPosition;
			Vector2 val6 = val3 - val5;
			val3 += val6 * num6;
			num7 += num6;
			val4.A = (byte)((float)(int)val4.A * (1f - num5));
			val4 *= 1f - num5;
			item = new DrawData(TextureAssets.AccShield[drawinfo.drawPlayer.shield].Value, val3, bodyFrame, val4, drawinfo.drawPlayer.bodyRotation, bodyVect, num7, drawinfo.playerEffect);
			item.shader = drawinfo.cShield;
			drawinfo.DrawDataCache.Add(item);
		}
		if (drawinfo.drawPlayer.mount.Cart)
		{
			drawinfo.DrawDataCache.Reverse(drawinfo.DrawDataCache.Count - 2, 2);
		}
	}

	public static void DrawPlayer_26_SolarShield(ref PlayerDrawSet drawinfo)
	{
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_010c: Unknown result type (might be due to invalid IL or missing references)
		//IL_011d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0122: Unknown result type (might be due to invalid IL or missing references)
		//IL_0131: Unknown result type (might be due to invalid IL or missing references)
		//IL_0134: Unknown result type (might be due to invalid IL or missing references)
		//IL_013e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0149: Unknown result type (might be due to invalid IL or missing references)
		if (drawinfo.drawPlayer.solarShields > 0 && drawinfo.shadow == 0f && !drawinfo.drawPlayer.dead)
		{
			Texture2D value = TextureAssets.Extra[61 + drawinfo.drawPlayer.solarShields - 1].Value;
			Color color = new Color(255, 255, 255, 127);
			float num = (drawinfo.drawPlayer.solarShieldPos[0] * new Vector2(1f, 0.5f)).ToRotation();
			if (drawinfo.drawPlayer.direction == -1)
			{
				num += (float)Math.PI;
			}
			num += (float)Math.PI / 50f * (float)drawinfo.drawPlayer.direction;
			DrawData item = new DrawData(value, new Vector2((float)(int)(drawinfo.Position.X - Main.screenPosition.X + (float)(drawinfo.drawPlayer.width / 2)), (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)(drawinfo.drawPlayer.height / 2))) + drawinfo.drawPlayer.solarShieldPos[0], null, color, num, value.Size() / 2f, 1f, drawinfo.playerEffect);
			item.shader = drawinfo.cBody;
			drawinfo.DrawDataCache.Add(item);
		}
	}

	public static void DrawPlayer_27_HeldItem(ref PlayerDrawSet drawinfo)
	{
		//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0121: Unknown result type (might be due to invalid IL or missing references)
		//IL_0126: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01de: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_05cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_05df: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0531: Unknown result type (might be due to invalid IL or missing references)
		//IL_053c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0541: Unknown result type (might be due to invalid IL or missing references)
		//IL_0546: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_050a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0514: Unknown result type (might be due to invalid IL or missing references)
		//IL_051b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0520: Unknown result type (might be due to invalid IL or missing references)
		//IL_0433: Unknown result type (might be due to invalid IL or missing references)
		//IL_043e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0445: Unknown result type (might be due to invalid IL or missing references)
		//IL_044a: Unknown result type (might be due to invalid IL or missing references)
		//IL_06e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_06f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_070c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0714: Unknown result type (might be due to invalid IL or missing references)
		//IL_05fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0609: Unknown result type (might be due to invalid IL or missing references)
		//IL_060e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0613: Unknown result type (might be due to invalid IL or missing references)
		//IL_0623: Unknown result type (might be due to invalid IL or missing references)
		//IL_0628: Unknown result type (might be due to invalid IL or missing references)
		//IL_062d: Unknown result type (might be due to invalid IL or missing references)
		//IL_047c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0487: Unknown result type (might be due to invalid IL or missing references)
		//IL_048c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0491: Unknown result type (might be due to invalid IL or missing references)
		//IL_0594: Unknown result type (might be due to invalid IL or missing references)
		//IL_0557: Unknown result type (might be due to invalid IL or missing references)
		//IL_0562: Unknown result type (might be due to invalid IL or missing references)
		//IL_0567: Unknown result type (might be due to invalid IL or missing references)
		//IL_056c: Unknown result type (might be due to invalid IL or missing references)
		//IL_06c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0466: Unknown result type (might be due to invalid IL or missing references)
		//IL_046b: Unknown result type (might be due to invalid IL or missing references)
		//IL_07d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_07da: Unknown result type (might be due to invalid IL or missing references)
		//IL_07dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_07e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_07c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_07cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0740: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0778: Unknown result type (might be due to invalid IL or missing references)
		//IL_066a: Unknown result type (might be due to invalid IL or missing references)
		//IL_066f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0674: Unknown result type (might be due to invalid IL or missing references)
		//IL_0699: Unknown result type (might be due to invalid IL or missing references)
		//IL_069e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0859: Unknown result type (might be due to invalid IL or missing references)
		//IL_085e: Unknown result type (might be due to invalid IL or missing references)
		//IL_079f: Unknown result type (might be due to invalid IL or missing references)
		//IL_07a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_07a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_07a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_0792: Unknown result type (might be due to invalid IL or missing references)
		//IL_08bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_08b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_089b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0892: Unknown result type (might be due to invalid IL or missing references)
		//IL_0960: Unknown result type (might be due to invalid IL or missing references)
		//IL_0965: Unknown result type (might be due to invalid IL or missing references)
		//IL_0969: Unknown result type (might be due to invalid IL or missing references)
		//IL_0984: Unknown result type (might be due to invalid IL or missing references)
		//IL_0989: Unknown result type (might be due to invalid IL or missing references)
		//IL_098b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0995: Unknown result type (might be due to invalid IL or missing references)
		//IL_099a: Unknown result type (might be due to invalid IL or missing references)
		//IL_09a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_09a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_09ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_09b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_09b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_09b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_09bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_09c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_09ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_09e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_09e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_09ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_09fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a01: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a33: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a38: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a3d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a42: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a44: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a49: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a4e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a6a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a74: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a84: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a88: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ad5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ada: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ade: Unknown result type (might be due to invalid IL or missing references)
		//IL_0af9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0afe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b20: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b25: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b2a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b2c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b2e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b38: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b3d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b44: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b49: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b4e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b53: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b55: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b5a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b5f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b68: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b6d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b7d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b81: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bb4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bb9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bbd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bd8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bdd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bdf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0be9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bee: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c08: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c2a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c43: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c4d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c52: Unknown result type (might be due to invalid IL or missing references)
		//IL_15a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_15aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_15b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_15b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_15bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_15c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_15df: Unknown result type (might be due to invalid IL or missing references)
		//IL_15e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_15ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_15f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_15fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_1604: Unknown result type (might be due to invalid IL or missing references)
		//IL_1609: Unknown result type (might be due to invalid IL or missing references)
		//IL_1610: Unknown result type (might be due to invalid IL or missing references)
		//IL_1614: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dc5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dcd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c7a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c7f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c84: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c89: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c8b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c90: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c9d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ca4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ca9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cae: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cb5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cc2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cc6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cfc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d01: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d06: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d0b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d0d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d12: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d17: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d20: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d25: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d3e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d43: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d48: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d58: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d5c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1209: Unknown result type (might be due to invalid IL or missing references)
		//IL_1213: Unknown result type (might be due to invalid IL or missing references)
		//IL_1224: Unknown result type (might be due to invalid IL or missing references)
		//IL_1229: Unknown result type (might be due to invalid IL or missing references)
		//IL_122b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1237: Unknown result type (might be due to invalid IL or missing references)
		//IL_1249: Unknown result type (might be due to invalid IL or missing references)
		//IL_1253: Unknown result type (might be due to invalid IL or missing references)
		//IL_110c: Unknown result type (might be due to invalid IL or missing references)
		//IL_111a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1122: Unknown result type (might be due to invalid IL or missing references)
		//IL_1131: Unknown result type (might be due to invalid IL or missing references)
		//IL_113c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1141: Unknown result type (might be due to invalid IL or missing references)
		//IL_1146: Unknown result type (might be due to invalid IL or missing references)
		//IL_1148: Unknown result type (might be due to invalid IL or missing references)
		//IL_1158: Unknown result type (might be due to invalid IL or missing references)
		//IL_115e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1160: Unknown result type (might be due to invalid IL or missing references)
		//IL_1165: Unknown result type (might be due to invalid IL or missing references)
		//IL_1170: Unknown result type (might be due to invalid IL or missing references)
		//IL_119a: Unknown result type (might be due to invalid IL or missing references)
		//IL_11ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_11c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_11c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_11d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_11d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_11de: Unknown result type (might be due to invalid IL or missing references)
		//IL_11e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_165e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1660: Unknown result type (might be due to invalid IL or missing references)
		//IL_1667: Unknown result type (might be due to invalid IL or missing references)
		//IL_166b: Unknown result type (might be due to invalid IL or missing references)
		//IL_166f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1691: Unknown result type (might be due to invalid IL or missing references)
		//IL_1696: Unknown result type (might be due to invalid IL or missing references)
		//IL_129c: Unknown result type (might be due to invalid IL or missing references)
		//IL_12bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_12c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_12cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_12d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_12d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_12e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_12ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_1309: Unknown result type (might be due to invalid IL or missing references)
		//IL_1310: Unknown result type (might be due to invalid IL or missing references)
		//IL_1316: Unknown result type (might be due to invalid IL or missing references)
		//IL_1268: Unknown result type (might be due to invalid IL or missing references)
		//IL_1273: Unknown result type (might be due to invalid IL or missing references)
		//IL_127d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e22: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e45: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e4a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e54: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e59: Unknown result type (might be due to invalid IL or missing references)
		//IL_16b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_16ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_16c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_16c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_16c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_133c: Unknown result type (might be due to invalid IL or missing references)
		//IL_135c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1366: Unknown result type (might be due to invalid IL or missing references)
		//IL_136b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1374: Unknown result type (might be due to invalid IL or missing references)
		//IL_1379: Unknown result type (might be due to invalid IL or missing references)
		//IL_1389: Unknown result type (might be due to invalid IL or missing references)
		//IL_138d: Unknown result type (might be due to invalid IL or missing references)
		//IL_13b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_13b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_13f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_1411: Unknown result type (might be due to invalid IL or missing references)
		//IL_141b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1420: Unknown result type (might be due to invalid IL or missing references)
		//IL_1427: Unknown result type (might be due to invalid IL or missing references)
		//IL_1434: Unknown result type (might be due to invalid IL or missing references)
		//IL_1438: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ea3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0eae: Unknown result type (might be due to invalid IL or missing references)
		//IL_0eb3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ec6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ecc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ece: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ed3: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a24: Unknown result type (might be due to invalid IL or missing references)
		//IL_173e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1743: Unknown result type (might be due to invalid IL or missing references)
		//IL_1748: Unknown result type (might be due to invalid IL or missing references)
		//IL_176d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1772: Unknown result type (might be due to invalid IL or missing references)
		//IL_17fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_1816: Unknown result type (might be due to invalid IL or missing references)
		//IL_1829: Unknown result type (might be due to invalid IL or missing references)
		//IL_182f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1831: Unknown result type (might be due to invalid IL or missing references)
		//IL_1844: Unknown result type (might be due to invalid IL or missing references)
		//IL_1849: Unknown result type (might be due to invalid IL or missing references)
		//IL_184e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1476: Unknown result type (might be due to invalid IL or missing references)
		//IL_1498: Unknown result type (might be due to invalid IL or missing references)
		//IL_14b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_14bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_14c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ef4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0eff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f04: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f17: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f1d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f1f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f24: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a97: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a86: Unknown result type (might be due to invalid IL or missing references)
		//IL_186c: Unknown result type (might be due to invalid IL or missing references)
		//IL_186e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1875: Unknown result type (might be due to invalid IL or missing references)
		//IL_1879: Unknown result type (might be due to invalid IL or missing references)
		//IL_1880: Unknown result type (might be due to invalid IL or missing references)
		//IL_1885: Unknown result type (might be due to invalid IL or missing references)
		//IL_18b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_18d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_18eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_18f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_18f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_18f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_18fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_1903: Unknown result type (might be due to invalid IL or missing references)
		//IL_192f: Unknown result type (might be due to invalid IL or missing references)
		//IL_193b: Unknown result type (might be due to invalid IL or missing references)
		//IL_193f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1951: Unknown result type (might be due to invalid IL or missing references)
		//IL_1956: Unknown result type (might be due to invalid IL or missing references)
		//IL_1960: Unknown result type (might be due to invalid IL or missing references)
		//IL_1968: Unknown result type (might be due to invalid IL or missing references)
		//IL_1987: Unknown result type (might be due to invalid IL or missing references)
		//IL_1993: Unknown result type (might be due to invalid IL or missing references)
		//IL_199a: Unknown result type (might be due to invalid IL or missing references)
		//IL_19ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_19b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_19bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_19c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_1503: Unknown result type (might be due to invalid IL or missing references)
		//IL_1523: Unknown result type (might be due to invalid IL or missing references)
		//IL_152d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1546: Unknown result type (might be due to invalid IL or missing references)
		//IL_154d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1552: Unknown result type (might be due to invalid IL or missing references)
		//IL_1561: Unknown result type (might be due to invalid IL or missing references)
		//IL_156e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1572: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fdf: Unknown result type (might be due to invalid IL or missing references)
		//IL_1007: Unknown result type (might be due to invalid IL or missing references)
		//IL_100c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1015: Unknown result type (might be due to invalid IL or missing references)
		//IL_101a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1021: Unknown result type (might be due to invalid IL or missing references)
		//IL_1025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fa3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fab: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fb3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fba: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f8a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f8f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f5f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f6c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f73: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ae2: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ace: Unknown result type (might be due to invalid IL or missing references)
		//IL_1073: Unknown result type (might be due to invalid IL or missing references)
		//IL_109b: Unknown result type (might be due to invalid IL or missing references)
		//IL_10a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_10b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_10bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_10c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ab4: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b55: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b64: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b70: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b75: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b7c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b80: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b84: Unknown result type (might be due to invalid IL or missing references)
		if (drawinfo.drawPlayer.JustDroppedAnItem || !drawinfo.drawPlayer.IsAllowedToHoldItems)
		{
			return;
		}
		if (drawinfo.SelectedDrawnProjectile != null && drawinfo.shadow == 0f && drawinfo.SelectedDrawnProjectile.drawLayer == 7)
		{
			drawinfo.projectileDrawPosition = drawinfo.DrawDataCache.Count;
		}
		Item heldItem = drawinfo.heldItem;
		int num = heldItem.type;
		if (drawinfo.drawPlayer.UsingBiomeTorches)
		{
			switch (num)
			{
			case 8:
				num = drawinfo.drawPlayer.BiomeTorchHoldStyle(num);
				break;
			case 966:
				num = drawinfo.drawPlayer.BiomeCampfireHoldStyle(num);
				break;
			}
		}
		float adjustedItemScale = drawinfo.drawPlayer.GetAdjustedItemScale(heldItem);
		Main.instance.LoadItem(num);
		Texture2D value = TextureAssets.Item[num].Value;
		Vector2 val = new Vector2((float)(int)(drawinfo.ItemLocation.X - Main.screenPosition.X), (float)(int)(drawinfo.ItemLocation.Y - Main.screenPosition.Y));
		Rectangle val2 = drawinfo.drawPlayer.GetItemDrawFrame(num);
		if (num == 5629)
		{
			value = TextureAssets.Extra[285].Value;
			val2 = value.Frame();
		}
		bool flag = drawinfo.drawPlayer.itemAnimation > 0 && heldItem.useStyle != 0;
		bool flag2 = heldItem.holdStyle != 0 && !drawinfo.drawPlayer.pulley;
		if (!drawinfo.drawPlayer.CanVisuallyHoldItem(heldItem))
		{
			flag2 = false;
		}
		drawinfo.itemColor = Lighting.GetColor((int)((double)drawinfo.Position.X + (double)drawinfo.drawPlayer.width * 0.5) / 16, (int)(((double)drawinfo.Position.Y + (double)drawinfo.drawPlayer.height * 0.5) / 16.0));
		if (num == 678)
		{
			drawinfo.itemColor = Color.White;
		}
		DrawPlayer_27_HeldItem_ApplyStealthToColor(ref drawinfo, heldItem, flag, flag2, ref drawinfo.itemColor);
		if (drawinfo.shadow != 0f || drawinfo.drawPlayer.frozen || !(flag | flag2) || num <= 0 || drawinfo.drawPlayer.dead || heldItem.noUseGraphic || (drawinfo.drawPlayer.wet && heldItem.noWet) || (drawinfo.drawPlayer.happyFunTorchTime && drawinfo.drawPlayer.inventory[drawinfo.drawPlayer.selectedItem].createTile == 4 && drawinfo.drawPlayer.itemAnimation == 0))
		{
			return;
		}
		_ = drawinfo.drawPlayer.name;
		Color color = Color.White;
		Vector2 val3 = Vector2.Zero;
		Vector2? val4 = null;
		switch (num)
		{
		case 1827:
		case 3352:
			if (drawinfo.drawPlayer.isDisplayDollOrInanimate)
			{
				val4 = new Vector2((float)((drawinfo.drawPlayer.direction == -1) ? (value.Width - 6) : 6), (float)(value.Height - 6));
			}
			break;
		case 5669:
		{
			float num3 = Utils.WrappedLerp(0.5f, 1f, (float)(Main.LocalPlayer.miscCounter % 100) / 100f);
			color = Color.Lerp(color, new Color(180, 85, 30), num3);
			color.A = (byte)heldItem.alpha;
			break;
		}
		case 5670:
		case 5671:
			color = Item.GetPhaseColor(heldItem.shoot);
			break;
		case 104:
		case 5094:
		case 5095:
			val3 = new Vector2(4f, -4f) * drawinfo.drawPlayer.Directions;
			break;
		case 426:
		case 797:
		case 1506:
		case 5096:
		case 5097:
			val3 = new Vector2(6f, -6f) * drawinfo.drawPlayer.Directions;
			break;
		case 46:
		{
			Vector3 val5 = drawinfo.itemColor.ToVector3();
			float num2 = Utils.Remap(val5.Length() / 1.731f, 0.3f, 0.5f, 1f, 0f);
			color = Color.Lerp(Color.Transparent, new Color(255, 255, 255, 127) * 0.7f, num2);
			break;
		}
		case 204:
			val3 = new Vector2(4f, -6f) * drawinfo.drawPlayer.Directions;
			break;
		case 3349:
			val3 = new Vector2(2f, -2f) * drawinfo.drawPlayer.Directions;
			break;
		case 3823:
			val3 = new Vector2((float)(7 * drawinfo.drawPlayer.direction), -7f * drawinfo.drawPlayer.gravDir);
			break;
		case 3827:
			val3 = new Vector2((float)(13 * drawinfo.drawPlayer.direction), -13f * drawinfo.drawPlayer.gravDir);
			color = heldItem.GetAlpha(drawinfo.itemColor);
			color = Color.Lerp(color, Color.White, 0.6f);
			color.A = 66;
			break;
		case 5462:
			val3 = new Vector2(12f, -14f) * drawinfo.drawPlayer.Directions;
			color = new Color(255, 140, 0, 5);
			color = Color.Transparent;
			if (drawinfo.SelectedDrawnProjectile != null)
			{
				Projectile selectedDrawnProjectile = drawinfo.SelectedDrawnProjectile;
				if (selectedDrawnProjectile.active && selectedDrawnProjectile.type == 1040)
				{
					Color val6 = new Color(255, 140, 0, 5);
					color = Color.Lerp(Color.Transparent, val6, Utils.Remap(selectedDrawnProjectile.ai[1], 0f, 30f, 0f, 1f));
				}
			}
			break;
		case 5466:
			val4 = new Vector2((float)((drawinfo.drawPlayer.direction == -1) ? (value.Width - 6) : 6), (float)(value.Height - 6));
			break;
		}
		DrawPlayer_27_HeldItem_ApplyStealthToColor(ref drawinfo, heldItem, flag, flag2, ref color);
		Vector2 val7 = new Vector2((float)val2.Width * 0.5f - (float)val2.Width * 0.5f * (float)drawinfo.drawPlayer.direction, (float)val2.Height);
		if (heldItem.useStyle == 9 && drawinfo.drawPlayer.itemAnimation > 0)
		{
			Vector2 val8 = new Vector2(0.5f, 0.4f);
			if (heldItem.type == 5009 || heldItem.type == 5042 || heldItem.type == 5645)
			{
				val8 = new Vector2(0.26f, 0.5f);
				if (drawinfo.drawPlayer.direction == -1)
				{
					val8.X = 1f - val8.X;
				}
			}
			val7 = val2.Size() * val8;
		}
		if (drawinfo.drawPlayer.gravDir == -1f)
		{
			val7.Y = (float)val2.Height - val7.Y;
		}
		val7 += val3;
		float num4 = drawinfo.drawPlayer.itemRotation;
		if (heldItem.useStyle == 8)
		{
			ref float x = ref val.X;
			float num5 = x;
			_ = drawinfo.drawPlayer.direction;
			x = num5 - 0f;
			num4 -= (float)Math.PI / 2f * (float)drawinfo.drawPlayer.direction;
			val7.Y = 2f;
			val7.X += 2 * drawinfo.drawPlayer.direction;
		}
		if (val4.HasValue)
		{
			val7 = val4.Value;
		}
		if (num == 425 || num == 507)
		{
			if (drawinfo.drawPlayer.gravDir == 1f)
			{
				if (drawinfo.drawPlayer.direction == 1)
				{
					drawinfo.itemEffect = (SpriteEffects)2;
				}
				else
				{
					drawinfo.itemEffect = (SpriteEffects)3;
				}
			}
			else if (drawinfo.drawPlayer.direction == 1)
			{
				drawinfo.itemEffect = (SpriteEffects)0;
			}
			else
			{
				drawinfo.itemEffect = (SpriteEffects)1;
			}
		}
		if ((num == 946 || num == 4707) && num4 != 0f)
		{
			val.Y -= 22f * drawinfo.drawPlayer.gravDir;
			num4 = -1.57f * (float)(-drawinfo.drawPlayer.direction) * drawinfo.drawPlayer.gravDir;
		}
		ItemSlot.GetItemLight(ref drawinfo.itemColor, heldItem, outInTheWorld: false, drawinfo.drawPlayer.stealth);
		DrawData item;
		switch (num)
		{
		case 3476:
		{
			Texture2D value2 = TextureAssets.Extra[64].Value;
			Rectangle val12 = value2.Frame(1, 9, 0, drawinfo.drawPlayer.miscCounter % 54 / 6);
			Vector2 val13 = new Vector2((float)(val12.Width / 2 * drawinfo.drawPlayer.direction), 0f);
			Vector2 origin2 = val12.Size() / 2f;
			item = new DrawData(value2, (drawinfo.ItemLocation - Main.screenPosition + val13).Floor(), val12, heldItem.GetAlpha(drawinfo.itemColor).MultiplyRGBA(new Color(new Vector4(0.5f, 0.5f, 0.5f, 0.8f))), drawinfo.drawPlayer.itemRotation, origin2, adjustedItemScale, drawinfo.itemEffect);
			drawinfo.DrawDataCache.Add(item);
			value2 = TextureAssets.GlowMask[195].Value;
			item = new DrawData(value2, (drawinfo.ItemLocation - Main.screenPosition + val13).Floor(), val12, new Color(250, 250, 250, heldItem.alpha) * 0.5f, drawinfo.drawPlayer.itemRotation, origin2, adjustedItemScale, drawinfo.itemEffect);
			drawinfo.DrawDataCache.Add(item);
			return;
		}
		case 4049:
		{
			Texture2D value3 = TextureAssets.Extra[92].Value;
			Rectangle val14 = value3.Frame(1, 4, 0, drawinfo.drawPlayer.miscCounter % 20 / 5);
			Vector2 val15 = new Vector2((float)(val14.Width / 2 * drawinfo.drawPlayer.direction), 0f);
			val15 += new Vector2((float)(-10 * drawinfo.drawPlayer.direction), 8f * drawinfo.drawPlayer.gravDir);
			Vector2 origin3 = val14.Size() / 2f;
			item = new DrawData(value3, (drawinfo.ItemLocation - Main.screenPosition + val15).Floor(), val14, heldItem.GetAlpha(drawinfo.itemColor), drawinfo.drawPlayer.itemRotation, origin3, adjustedItemScale, drawinfo.itemEffect);
			drawinfo.DrawDataCache.Add(item);
			return;
		}
		case 3779:
		{
			Texture2D val9 = value;
			Rectangle val10 = val9.Frame();
			Vector2 val11 = new Vector2((float)(val10.Width / 2 * drawinfo.drawPlayer.direction), 0f);
			Vector2 origin = val10.Size() / 2f;
			float num6 = ((float)drawinfo.drawPlayer.miscCounter / 75f * ((float)Math.PI * 2f)).ToRotationVector2().X * 1f + 0f;
			Color color2 = new Color(120, 40, 222, 0) * (num6 / 2f * 0.3f + 0.85f) * 0.5f;
			num6 = 2f;
			for (float num7 = 0f; num7 < 4f; num7++)
			{
				item = new DrawData(TextureAssets.GlowMask[218].Value, (drawinfo.ItemLocation - Main.screenPosition + val11).Floor() + (num7 * ((float)Math.PI / 2f)).ToRotationVector2() * num6, val10, color2, drawinfo.drawPlayer.itemRotation, origin, adjustedItemScale, drawinfo.itemEffect);
				drawinfo.DrawDataCache.Add(item);
			}
			item = new DrawData(val9, (drawinfo.ItemLocation - Main.screenPosition + val11).Floor(), val10, heldItem.GetAlpha(drawinfo.itemColor).MultiplyRGBA(new Color(new Vector4(0.5f, 0.5f, 0.5f, 0.8f))), drawinfo.drawPlayer.itemRotation, origin, adjustedItemScale, drawinfo.itemEffect);
			drawinfo.DrawDataCache.Add(item);
			return;
		}
		}
		if (heldItem.useStyle == 5)
		{
			if (Item.staff[num])
			{
				float num8 = drawinfo.drawPlayer.itemRotation + 0.785f * (float)drawinfo.drawPlayer.direction;
				float num9 = 0f;
				float num10 = 0f;
				Vector2 val16 = new Vector2(0f, (float)val2.Height);
				if (num == 3210)
				{
					num9 = 8 * -drawinfo.drawPlayer.direction;
					num10 = 2 * (int)drawinfo.drawPlayer.gravDir;
				}
				if (num == 3870)
				{
					Vector2 val17 = (drawinfo.drawPlayer.itemRotation + (float)Math.PI / 4f * (float)drawinfo.drawPlayer.direction).ToRotationVector2() * new Vector2((float)(-drawinfo.drawPlayer.direction) * 1.5f, drawinfo.drawPlayer.gravDir) * 3f;
					num9 = (int)val17.X;
					num10 = (int)val17.Y;
				}
				if (num == 3787)
				{
					num10 = (int)((float)(8 * (int)drawinfo.drawPlayer.gravDir) * (float)Math.Cos(num8));
				}
				if (num == 6152)
				{
					Vector2 val18 = (new Vector2(-16f, -4f) * drawinfo.drawPlayer.Directions).RotatedBy(drawinfo.drawPlayer.itemRotation);
					num9 = val18.X;
					num10 = val18.Y;
				}
				if (num == 3209)
				{
					Vector2 val19 = (new Vector2(-8f, 0f) * drawinfo.drawPlayer.Directions).RotatedBy(drawinfo.drawPlayer.itemRotation);
					num9 = val19.X;
					num10 = val19.Y;
				}
				if (drawinfo.drawPlayer.gravDir == -1f)
				{
					if (drawinfo.drawPlayer.direction == -1)
					{
						num8 += 1.57f;
						val16 = new Vector2((float)val2.Width, 0f);
						num9 -= (float)val2.Width;
					}
					else
					{
						num8 -= 1.57f;
						val16 = Vector2.Zero;
					}
				}
				else if (drawinfo.drawPlayer.direction == -1)
				{
					val16 = new Vector2((float)val2.Width, (float)val2.Height);
					num9 -= (float)val2.Width;
				}
				item = new DrawData(value, new Vector2((float)(int)(drawinfo.ItemLocation.X - Main.screenPosition.X + val16.X + num9), (float)(int)(drawinfo.ItemLocation.Y - Main.screenPosition.Y + num10)), val2, heldItem.GetAlpha(drawinfo.itemColor), num8, val16, adjustedItemScale, drawinfo.itemEffect);
				drawinfo.DrawDataCache.Add(item);
				if (num == 3870)
				{
					item = new DrawData(TextureAssets.GlowMask[238].Value, new Vector2((float)(int)(drawinfo.ItemLocation.X - Main.screenPosition.X + val16.X + num9), (float)(int)(drawinfo.ItemLocation.Y - Main.screenPosition.Y + num10)), val2, new Color(255, 255, 255, 127), num8, val16, adjustedItemScale, drawinfo.itemEffect);
					drawinfo.DrawDataCache.Add(item);
				}
				return;
			}
			if (num == 5118)
			{
				float rotation = drawinfo.drawPlayer.itemRotation + 1.57f * (float)drawinfo.drawPlayer.direction;
				Vector2 origin4 = new Vector2((float)val2.Width * 0.5f, (float)val2.Height);
				Vector2 spinningpoint = new Vector2(10f, 4f) * drawinfo.drawPlayer.Directions;
				spinningpoint = spinningpoint.RotatedBy(drawinfo.drawPlayer.itemRotation);
				spinningpoint.Y += (float)val2.Height * 0.5f;
				item = new DrawData(value, new Vector2((float)(int)(drawinfo.ItemLocation.X - Main.screenPosition.X + spinningpoint.X), (float)(int)(drawinfo.ItemLocation.Y - Main.screenPosition.Y + spinningpoint.Y)), val2, heldItem.GetAlpha(drawinfo.itemColor), rotation, origin4, adjustedItemScale, drawinfo.itemEffect);
				drawinfo.DrawDataCache.Add(item);
				return;
			}
			int num11 = 10;
			Vector2 val20 = new Vector2(0f, (float)(val2.Height / 2));
			Vector2 val21 = Main.DrawPlayerItemPos(drawinfo.drawPlayer.gravDir, num);
			num11 = (int)val21.X;
			val20.Y = val21.Y;
			Vector2 origin5 = new Vector2((float)(-num11), (float)(val2.Height / 2));
			if (drawinfo.drawPlayer.direction == -1)
			{
				origin5 = new Vector2((float)(val2.Width + num11), (float)(val2.Height / 2));
			}
			item = new DrawData(value, new Vector2((float)(int)(drawinfo.ItemLocation.X - Main.screenPosition.X + val20.X), (float)(int)(drawinfo.ItemLocation.Y - Main.screenPosition.Y + val20.Y)), val2, heldItem.GetAlpha(drawinfo.itemColor), drawinfo.drawPlayer.itemRotation, origin5, adjustedItemScale, drawinfo.itemEffect);
			drawinfo.DrawDataCache.Add(item);
			if (heldItem.color != default(Color))
			{
				item = new DrawData(value, new Vector2((float)(int)(drawinfo.ItemLocation.X - Main.screenPosition.X + val20.X), (float)(int)(drawinfo.ItemLocation.Y - Main.screenPosition.Y + val20.Y)), val2, heldItem.GetColor(drawinfo.itemColor), drawinfo.drawPlayer.itemRotation, origin5, adjustedItemScale, drawinfo.itemEffect);
				drawinfo.DrawDataCache.Add(item);
			}
			if (heldItem.glowMask != -1)
			{
				Color color3 = Color.White;
				DrawPlayer_27_HeldItem_ApplyStealthToColor(ref drawinfo, heldItem, flag, flag2, ref color3);
				item = new DrawData(TextureAssets.GlowMask[heldItem.glowMask].Value, new Vector2((float)(int)(drawinfo.ItemLocation.X - Main.screenPosition.X + val20.X), (float)(int)(drawinfo.ItemLocation.Y - Main.screenPosition.Y + val20.Y)), val2, color3, drawinfo.drawPlayer.itemRotation, origin5, adjustedItemScale, drawinfo.itemEffect);
				drawinfo.DrawDataCache.Add(item);
			}
			if (num == 3788)
			{
				float num12 = ((float)drawinfo.drawPlayer.miscCounter / 75f * ((float)Math.PI * 2f)).ToRotationVector2().X * 1f + 0f;
				Color color4 = new Color(80, 40, 252, 0) * (num12 / 2f * 0.3f + 0.85f) * 0.5f;
				DrawPlayer_27_HeldItem_ApplyStealthToColor(ref drawinfo, heldItem, flag, flag2, ref color4);
				for (float num13 = 0f; num13 < 4f; num13++)
				{
					item = new DrawData(TextureAssets.GlowMask[220].Value, new Vector2((float)(int)(drawinfo.ItemLocation.X - Main.screenPosition.X + val20.X), (float)(int)(drawinfo.ItemLocation.Y - Main.screenPosition.Y + val20.Y)) + (num13 * ((float)Math.PI / 2f) + drawinfo.drawPlayer.itemRotation).ToRotationVector2() * num12, null, color4, drawinfo.drawPlayer.itemRotation, origin5, adjustedItemScale, drawinfo.itemEffect);
					drawinfo.DrawDataCache.Add(item);
				}
			}
			return;
		}
		item = new DrawData(value, val, val2, heldItem.GetAlpha(drawinfo.itemColor), num4, val7, adjustedItemScale, drawinfo.itemEffect);
		drawinfo.DrawDataCache.Add(item);
		if (heldItem.color != default(Color))
		{
			item = new DrawData(value, val, val2, heldItem.GetColor(drawinfo.itemColor), num4, val7, adjustedItemScale, drawinfo.itemEffect);
			drawinfo.DrawDataCache.Add(item);
		}
		if (heldItem.glowMask != -1)
		{
			if (num == 5670 || num == 5671)
			{
				item = new DrawData(TextureAssets.GlowMask[heldItem.glowMask].Value, val, val2, color, num4, val7, adjustedItemScale, drawinfo.itemEffect);
				drawinfo.DrawDataCache.Add(item);
				color = Item.GetPhaseColor(heldItem.shoot, drawColor: true);
				DrawPlayer_27_HeldItem_ApplyStealthToColor(ref drawinfo, heldItem, flag, flag2, ref color);
			}
			item = new DrawData(TextureAssets.GlowMask[heldItem.glowMask].Value, val, val2, color, num4, val7, adjustedItemScale, drawinfo.itemEffect);
			drawinfo.DrawDataCache.Add(item);
		}
		if (heldItem.type == 5462 && drawinfo.SelectedDrawnProjectile != null)
		{
			Projectile selectedDrawnProjectile2 = drawinfo.SelectedDrawnProjectile;
			if (selectedDrawnProjectile2.active && selectedDrawnProjectile2.type == 1040)
			{
				float fromValue = selectedDrawnProjectile2.ai[1];
				Color val22 = new Color(255, 180, 60, 0);
				color = Color.Lerp(Color.Transparent, val22, Utils.Remap(selectedDrawnProjectile2.ai[1], 0f, 30f, 0f, 1f));
				float num14 = Utils.Remap(fromValue, 20f, 26f, 0f, 1f) * Utils.Remap(fromValue, 26f, 32f, 1f, 0f);
				float num15 = Utils.Remap(fromValue, 23f, 29f, 0f, 1f);
				num15 = 1f - (1f - num15) * (1f - num15);
				float num16 = num15;
				float num17 = adjustedItemScale * (1f + num16 * 0.3f);
				Vector2 position = val - new Vector2((float)drawinfo.drawPlayer.direction, 0f - drawinfo.drawPlayer.gravDir).RotatedBy(drawinfo.drawPlayer.itemRotation) * (num17 * 4f + 3f);
				for (float num18 = 0f; num18 < (float)Math.PI * 2f; num18 += (float)Math.PI / 2f)
				{
					item = new DrawData(TextureAssets.GlowMask[heldItem.glowMask].Value, position, val2, color * num14, num4, val7, num17, drawinfo.itemEffect);
					drawinfo.DrawDataCache.Add(item);
				}
				int num19 = 37;
				Vector2 position2 = val + new Vector2((float)(num19 * drawinfo.drawPlayer.direction), (float)(-num19) * drawinfo.drawPlayer.gravDir).RotatedBy(drawinfo.drawPlayer.itemRotation) * adjustedItemScale;
				Texture2D value4 = TextureAssets.Extra[174].Value;
				float num20 = 1f - num16;
				num20 *= 0.85f;
				item = new DrawData(value4, position2, null, color * num14, 0f, value4.Frame().Size() / 2f, num20, drawinfo.itemEffect);
				drawinfo.DrawDataCache.Add(item);
				item = new DrawData(value4, position2, null, Color.White * num14, 0f, value4.Frame().Size() / 2f, num20 * 0.92f, drawinfo.itemEffect);
				drawinfo.DrawDataCache.Add(item);
			}
		}
		if (!heldItem.flame || drawinfo.shadow != 0f)
		{
			return;
		}
		try
		{
			Main.instance.LoadItemFlames(num);
			if (TextureAssets.ItemFlame[num].IsLoaded)
			{
				Color color5 = new Color(100, 100, 100, 0);
				int num21 = 7;
				float num22 = 1f;
				float num23 = 0f;
				switch (num)
				{
				case 3045:
					color5 = new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB, 0);
					break;
				case 5293:
					color5 = new Color(50, 50, 100, 20);
					break;
				case 5353:
					color5 = new Color(255, 255, 255, 200);
					break;
				case 4952:
					num21 = 3;
					num22 = 0.6f;
					color5 = new Color(50, 50, 50, 0);
					break;
				case 5322:
					color5 = new Color(100, 100, 100, 150);
					num23 = -2 * drawinfo.drawPlayer.direction;
					break;
				}
				DrawPlayer_27_HeldItem_ApplyStealthToColor(ref drawinfo, heldItem, flag, flag2, ref color5);
				for (int i = 0; i < num21; i++)
				{
					float num24 = drawinfo.drawPlayer.itemFlamePos[i].X * adjustedItemScale * num22;
					float num25 = drawinfo.drawPlayer.itemFlamePos[i].Y * adjustedItemScale * num22;
					item = new DrawData(TextureAssets.ItemFlame[num].Value, new Vector2((float)(int)(val.X + num24 + num23), (float)(int)(val.Y + num25)), val2, color5, num4, val7, adjustedItemScale, drawinfo.itemEffect);
					drawinfo.DrawDataCache.Add(item);
				}
			}
		}
		catch
		{
		}
	}

	private static void DrawPlayer_27_HeldItem_ApplyStealthToColor(ref PlayerDrawSet drawinfo, Item playerItem, bool drawUseStyle, bool drawHoldStyle, ref Color color)
	{
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0104: Unknown result type (might be due to invalid IL or missing references)
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		//IL_010e: Unknown result type (might be due to invalid IL or missing references)
		bool flag = drawUseStyle && playerItem.ranged;
		bool flag2 = !drawUseStyle & drawHoldStyle;
		if (drawinfo.drawPlayer.shroomiteStealth && (flag | flag2))
		{
			float num = drawinfo.drawPlayer.stealth;
			if (num < 0.03f)
			{
				num = 0.03f;
			}
			float num2 = (1f + num * 10f) / 11f;
			color = new Color((int)(byte)((float)(int)color.R * num), (int)(byte)((float)(int)color.G * num), (int)(byte)((float)(int)color.B * num2), (int)(byte)((float)(int)color.A * num));
		}
		if (drawinfo.drawPlayer.setVortex && (flag | flag2))
		{
			float num3 = drawinfo.drawPlayer.stealth;
			if (num3 < 0.03f)
			{
				num3 = 0.03f;
			}
			_ = (1f + num3 * 10f) / 11f;
			color = color.MultiplyRGBA(new Color(Vector4.Lerp(Vector4.One, new Vector4(0f, 0.12f, 0.16f, 0f), 1f - num3)));
		}
	}

	public static void DrawPlayer_28_ArmOverItem(ref PlayerDrawSet drawinfo)
	{
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0806: Unknown result type (might be due to invalid IL or missing references)
		//IL_0811: Unknown result type (might be due to invalid IL or missing references)
		//IL_0816: Unknown result type (might be due to invalid IL or missing references)
		//IL_0841: Unknown result type (might be due to invalid IL or missing references)
		//IL_0846: Unknown result type (might be due to invalid IL or missing references)
		//IL_0851: Unknown result type (might be due to invalid IL or missing references)
		//IL_085c: Unknown result type (might be due to invalid IL or missing references)
		//IL_086d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0878: Unknown result type (might be due to invalid IL or missing references)
		//IL_0932: Unknown result type (might be due to invalid IL or missing references)
		//IL_093d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0942: Unknown result type (might be due to invalid IL or missing references)
		//IL_096d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0972: Unknown result type (might be due to invalid IL or missing references)
		//IL_097d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0988: Unknown result type (might be due to invalid IL or missing references)
		//IL_0999: Unknown result type (might be due to invalid IL or missing references)
		//IL_09a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a50: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a5b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a60: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a8b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a90: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a9b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aa6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ab7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ac2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_041a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0425: Unknown result type (might be due to invalid IL or missing references)
		//IL_042a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0455: Unknown result type (might be due to invalid IL or missing references)
		//IL_045a: Unknown result type (might be due to invalid IL or missing references)
		//IL_045f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0466: Unknown result type (might be due to invalid IL or missing references)
		//IL_0477: Unknown result type (might be due to invalid IL or missing references)
		//IL_0482: Unknown result type (might be due to invalid IL or missing references)
		//IL_0543: Unknown result type (might be due to invalid IL or missing references)
		//IL_054e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0553: Unknown result type (might be due to invalid IL or missing references)
		//IL_057e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0583: Unknown result type (might be due to invalid IL or missing references)
		//IL_0588: Unknown result type (might be due to invalid IL or missing references)
		//IL_058f: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0327: Unknown result type (might be due to invalid IL or missing references)
		//IL_032c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0337: Unknown result type (might be due to invalid IL or missing references)
		//IL_0342: Unknown result type (might be due to invalid IL or missing references)
		//IL_0353: Unknown result type (might be due to invalid IL or missing references)
		//IL_035e: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_020a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0215: Unknown result type (might be due to invalid IL or missing references)
		//IL_0226: Unknown result type (might be due to invalid IL or missing references)
		//IL_0231: Unknown result type (might be due to invalid IL or missing references)
		//IL_06c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_06cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_06d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0705: Unknown result type (might be due to invalid IL or missing references)
		//IL_070a: Unknown result type (might be due to invalid IL or missing references)
		//IL_070f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0715: Unknown result type (might be due to invalid IL or missing references)
		//IL_0723: Unknown result type (might be due to invalid IL or missing references)
		//IL_072e: Unknown result type (might be due to invalid IL or missing references)
		if (drawinfo.drawPlayer.mount.Active && drawinfo.drawPlayer.mount.Type == 54)
		{
			drawinfo.drawPlayer.mount.Draw(drawinfo.DrawDataCache, 3, drawinfo.drawPlayer, drawinfo.Position, drawinfo.colorMount, drawinfo.playerEffect, drawinfo.shadow);
		}
		else if (drawinfo.usesCompositeTorso)
		{
			DrawPlayer_28_ArmOverItemComposite(ref drawinfo);
		}
		else if (drawinfo.drawPlayer.body > 0 && drawinfo.drawPlayer.body < ArmorIDs.Body.Count)
		{
			Rectangle bodyFrame = drawinfo.drawPlayer.bodyFrame;
			int num = drawinfo.armorAdjust;
			bodyFrame.X += num;
			bodyFrame.Width -= num;
			if (drawinfo.drawPlayer.direction == -1)
			{
				num = 0;
			}
			if (drawinfo.drawPlayer.invis && (drawinfo.drawPlayer.body == 21 || drawinfo.drawPlayer.body == 22))
			{
				return;
			}
			DrawData item;
			if (drawinfo.missingHand && !drawinfo.drawPlayer.invis)
			{
				_ = drawinfo.drawPlayer.body;
				DrawData drawData;
				if (drawinfo.missingArm)
				{
					drawData = new DrawData(TextureAssets.Players[drawinfo.skinVar, 7].Value, new Vector2((float)(int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.bodyPosition + new Vector2((float)(drawinfo.drawPlayer.bodyFrame.Width / 2), (float)(drawinfo.drawPlayer.bodyFrame.Height / 2)), drawinfo.drawPlayer.bodyFrame, drawinfo.colorBodySkin, drawinfo.drawPlayer.bodyRotation, drawinfo.bodyVect, 1f, drawinfo.playerEffect);
					drawData.shader = drawinfo.skinDyePacked;
					item = drawData;
					drawinfo.DrawDataCache.Add(item);
				}
				drawData = new DrawData(TextureAssets.Players[drawinfo.skinVar, 9].Value, new Vector2((float)(int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.bodyPosition + new Vector2((float)(drawinfo.drawPlayer.bodyFrame.Width / 2), (float)(drawinfo.drawPlayer.bodyFrame.Height / 2)), drawinfo.drawPlayer.bodyFrame, drawinfo.colorBodySkin, drawinfo.drawPlayer.bodyRotation, drawinfo.bodyVect, 1f, drawinfo.playerEffect);
				drawData.shader = drawinfo.skinDyePacked;
				item = drawData;
				drawinfo.DrawDataCache.Add(item);
			}
			item = new DrawData(TextureAssets.ArmorArm[drawinfo.drawPlayer.body].Value, new Vector2((float)((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)) + num), (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.bodyPosition + new Vector2((float)(drawinfo.drawPlayer.bodyFrame.Width / 2), (float)(drawinfo.drawPlayer.bodyFrame.Height / 2)), bodyFrame, drawinfo.colorArmorBody, drawinfo.drawPlayer.bodyRotation, drawinfo.bodyVect, 1f, drawinfo.playerEffect);
			item.shader = drawinfo.cBody;
			drawinfo.DrawDataCache.Add(item);
			if (drawinfo.armGlowMask != -1)
			{
				item = new DrawData(TextureAssets.GlowMask[drawinfo.armGlowMask].Value, new Vector2((float)((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)) + num), (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.bodyPosition + new Vector2((float)(drawinfo.drawPlayer.bodyFrame.Width / 2), (float)(drawinfo.drawPlayer.bodyFrame.Height / 2)), bodyFrame, drawinfo.armGlowColor, drawinfo.drawPlayer.bodyRotation, drawinfo.bodyVect, 1f, drawinfo.playerEffect);
				item.shader = drawinfo.cBody;
				drawinfo.DrawDataCache.Add(item);
			}
			if (drawinfo.drawPlayer.body == 205)
			{
				Color color = new Color(100, 100, 100, 0);
				ulong seed = (ulong)(drawinfo.drawPlayer.miscCounter / 4);
				int num2 = 4;
				for (int i = 0; i < num2; i++)
				{
					float num3 = (float)Utils.RandomInt(ref seed, -10, 11) * 0.2f;
					float num4 = (float)Utils.RandomInt(ref seed, -10, 1) * 0.15f;
					item = new DrawData(TextureAssets.GlowMask[240].Value, new Vector2((float)((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)) + num), (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.bodyPosition + new Vector2((float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + num3, (float)(drawinfo.drawPlayer.bodyFrame.Height / 2) + num4), bodyFrame, color, drawinfo.drawPlayer.bodyRotation, drawinfo.bodyVect, 1f, drawinfo.playerEffect);
					item.shader = drawinfo.cBody;
					drawinfo.DrawDataCache.Add(item);
				}
			}
		}
		else if (!drawinfo.drawPlayer.invis)
		{
			DrawData drawData = new DrawData(TextureAssets.Players[drawinfo.skinVar, 7].Value, new Vector2((float)(int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.bodyPosition + new Vector2((float)(drawinfo.drawPlayer.bodyFrame.Width / 2), (float)(drawinfo.drawPlayer.bodyFrame.Height / 2)), drawinfo.drawPlayer.bodyFrame, drawinfo.colorBodySkin, drawinfo.drawPlayer.bodyRotation, drawinfo.bodyVect, 1f, drawinfo.playerEffect);
			drawData.shader = drawinfo.skinDyePacked;
			DrawData item = drawData;
			drawinfo.DrawDataCache.Add(item);
			item = new DrawData(TextureAssets.Players[drawinfo.skinVar, 8].Value, new Vector2((float)(int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.bodyPosition + new Vector2((float)(drawinfo.drawPlayer.bodyFrame.Width / 2), (float)(drawinfo.drawPlayer.bodyFrame.Height / 2)), drawinfo.drawPlayer.bodyFrame, drawinfo.colorUnderShirt, drawinfo.drawPlayer.bodyRotation, drawinfo.bodyVect, 1f, drawinfo.playerEffect);
			drawinfo.DrawDataCache.Add(item);
			item = new DrawData(TextureAssets.Players[drawinfo.skinVar, 13].Value, new Vector2((float)(int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.bodyPosition + new Vector2((float)(drawinfo.drawPlayer.bodyFrame.Width / 2), (float)(drawinfo.drawPlayer.bodyFrame.Height / 2)), drawinfo.drawPlayer.bodyFrame, drawinfo.colorShirt, drawinfo.drawPlayer.bodyRotation, drawinfo.bodyVect, 1f, drawinfo.playerEffect);
			drawinfo.DrawDataCache.Add(item);
		}
	}

	public static void DrawPlayer_28_ArmOverItemComposite(ref PlayerDrawSet drawinfo)
	{
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		//IL_0105: Invalid comparison between Unknown and I4
		//IL_010e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0113: Unknown result type (might be due to invalid IL or missing references)
		//IL_0118: Unknown result type (might be due to invalid IL or missing references)
		//IL_0139: Unknown result type (might be due to invalid IL or missing references)
		//IL_013e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0141: Unknown result type (might be due to invalid IL or missing references)
		//IL_0146: Unknown result type (might be due to invalid IL or missing references)
		//IL_0148: Unknown result type (might be due to invalid IL or missing references)
		//IL_014a: Unknown result type (might be due to invalid IL or missing references)
		//IL_014c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0151: Unknown result type (might be due to invalid IL or missing references)
		//IL_0153: Unknown result type (might be due to invalid IL or missing references)
		//IL_0154: Unknown result type (might be due to invalid IL or missing references)
		//IL_0156: Unknown result type (might be due to invalid IL or missing references)
		//IL_015b: Unknown result type (might be due to invalid IL or missing references)
		//IL_015c: Unknown result type (might be due to invalid IL or missing references)
		//IL_015e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0163: Unknown result type (might be due to invalid IL or missing references)
		//IL_0168: Unknown result type (might be due to invalid IL or missing references)
		//IL_0184: Unknown result type (might be due to invalid IL or missing references)
		//IL_0186: Unknown result type (might be due to invalid IL or missing references)
		//IL_018c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0195: Unknown result type (might be due to invalid IL or missing references)
		//IL_019b: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b2d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b2f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b3a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b40: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b48: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_05de: Unknown result type (might be due to invalid IL or missing references)
		//IL_0911: Unknown result type (might be due to invalid IL or missing references)
		//IL_0913: Unknown result type (might be due to invalid IL or missing references)
		//IL_091e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0924: Unknown result type (might be due to invalid IL or missing references)
		//IL_092c: Unknown result type (might be due to invalid IL or missing references)
		//IL_095d: Unknown result type (might be due to invalid IL or missing references)
		//IL_095f: Unknown result type (might be due to invalid IL or missing references)
		//IL_096a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0970: Unknown result type (might be due to invalid IL or missing references)
		//IL_0978: Unknown result type (might be due to invalid IL or missing references)
		//IL_09a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_09aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_09b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_09bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_09c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_08b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_08b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_08c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_08ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_08d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0628: Unknown result type (might be due to invalid IL or missing references)
		//IL_062b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0636: Unknown result type (might be due to invalid IL or missing references)
		//IL_063c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0644: Unknown result type (might be due to invalid IL or missing references)
		//IL_0675: Unknown result type (might be due to invalid IL or missing references)
		//IL_0678: Unknown result type (might be due to invalid IL or missing references)
		//IL_0683: Unknown result type (might be due to invalid IL or missing references)
		//IL_0689: Unknown result type (might be due to invalid IL or missing references)
		//IL_0691: Unknown result type (might be due to invalid IL or missing references)
		//IL_06c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_06c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_06cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_06d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_06dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a98: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a9a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aa5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aab: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ab3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a42: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a45: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a50: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a56: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a5e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0707: Unknown result type (might be due to invalid IL or missing references)
		//IL_0784: Unknown result type (might be due to invalid IL or missing references)
		//IL_0789: Unknown result type (might be due to invalid IL or missing references)
		//IL_0794: Unknown result type (might be due to invalid IL or missing references)
		//IL_0799: Unknown result type (might be due to invalid IL or missing references)
		//IL_079f: Unknown result type (might be due to invalid IL or missing references)
		//IL_07a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_07a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_07d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_07dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_07f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0834: Unknown result type (might be due to invalid IL or missing references)
		//IL_083c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0847: Unknown result type (might be due to invalid IL or missing references)
		//IL_0858: Unknown result type (might be due to invalid IL or missing references)
		//IL_0863: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_04cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_037f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0382: Unknown result type (might be due to invalid IL or missing references)
		//IL_038d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0393: Unknown result type (might be due to invalid IL or missing references)
		//IL_039b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0327: Unknown result type (might be due to invalid IL or missing references)
		//IL_0329: Unknown result type (might be due to invalid IL or missing references)
		//IL_0334: Unknown result type (might be due to invalid IL or missing references)
		//IL_033a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0342: Unknown result type (might be due to invalid IL or missing references)
		//IL_0528: Unknown result type (might be due to invalid IL or missing references)
		//IL_052a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0535: Unknown result type (might be due to invalid IL or missing references)
		//IL_053b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0543: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0401: Unknown result type (might be due to invalid IL or missing references)
		//IL_0407: Unknown result type (might be due to invalid IL or missing references)
		//IL_040f: Unknown result type (might be due to invalid IL or missing references)
		//IL_045f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0462: Unknown result type (might be due to invalid IL or missing references)
		//IL_046d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0473: Unknown result type (might be due to invalid IL or missing references)
		//IL_047b: Unknown result type (might be due to invalid IL or missing references)
		Vector2 val = new Vector2((float)(int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.bodyPosition + new Vector2((float)(drawinfo.drawPlayer.bodyFrame.Width / 2), (float)(drawinfo.drawPlayer.bodyFrame.Height / 2));
		Vector2 val2 = Main.OffsetsPlayerHeadgear[drawinfo.drawPlayer.bodyFrame.Y / drawinfo.drawPlayer.bodyFrame.Height];
		val2.Y -= 2f;
		val += val2 * (float)(-(((int)drawinfo.playerEffect & 2) > 0).ToDirectionInt());
		float bodyRotation = drawinfo.drawPlayer.bodyRotation;
		float rotation = drawinfo.drawPlayer.bodyRotation + drawinfo.compositeFrontArmRotation;
		Vector2 bodyVect = drawinfo.bodyVect;
		Vector2 compositeOffset_FrontArm = GetCompositeOffset_FrontArm(ref drawinfo);
		bodyVect += compositeOffset_FrontArm;
		val += compositeOffset_FrontArm;
		Vector2 position = val + drawinfo.frontShoulderOffset;
		if (drawinfo.compFrontArmFrame.X / drawinfo.compFrontArmFrame.Width >= 7)
		{
			val += new Vector2((float)((((int)drawinfo.playerEffect & 1) == 0) ? 1 : (-1)), (float)((((int)drawinfo.playerEffect & 2) == 0) ? 1 : (-1)));
		}
		_ = drawinfo.drawPlayer.invis;
		bool num = drawinfo.drawPlayer.body > 0 && drawinfo.drawPlayer.body < ArmorIDs.Body.Count;
		bool flag = drawinfo.drawPlayer.coat > 0 && drawinfo.drawPlayer.coat < ArmorIDs.Body.Count;
		int num2 = (drawinfo.compShoulderOverFrontArm ? 1 : 0);
		int num3 = ((!drawinfo.compShoulderOverFrontArm) ? 1 : 0);
		int num4 = ((!drawinfo.compShoulderOverFrontArm) ? 1 : 0);
		bool flag2 = !drawinfo.hidesTopSkin;
		if (num)
		{
			if (!drawinfo.drawPlayer.invis || IsArmorDrawnWhenInvisible(drawinfo.drawPlayer.body))
			{
				Texture2D value = TextureAssets.ArmorBodyComposite[drawinfo.drawPlayer.body].Value;
				for (int i = 0; i < 2; i++)
				{
					if ((!drawinfo.drawPlayer.invis && i == num4) & flag2)
					{
						if (drawinfo.missingArm)
						{
							drawinfo.DrawDataCache.Add(new DrawData(TextureAssets.Players[drawinfo.skinVar, 7].Value, val, drawinfo.compFrontArmFrame, drawinfo.colorBodySkin, rotation, bodyVect, 1f, drawinfo.playerEffect)
							{
								shader = drawinfo.skinDyePacked
							});
						}
						if (drawinfo.missingHand)
						{
							drawinfo.DrawDataCache.Add(new DrawData(TextureAssets.Players[drawinfo.skinVar, 9].Value, val, drawinfo.compFrontArmFrame, drawinfo.colorBodySkin, rotation, bodyVect, 1f, drawinfo.playerEffect)
							{
								shader = drawinfo.skinDyePacked
							});
						}
					}
					if (i == num2 && !drawinfo.hideCompositeShoulders)
					{
						DrawCompositeArmorPiece(ref drawinfo, CompositePlayerDrawContext.FrontShoulder, new DrawData(value, position, drawinfo.compFrontShoulderFrame, drawinfo.colorArmorBody, bodyRotation, bodyVect, 1f, drawinfo.playerEffect)
						{
							shader = drawinfo.cBody
						}, drawinfo.drawPlayer.body);
						if (drawinfo.drawPlayer.body == 71)
						{
							Texture2D value2 = TextureAssets.Extra[277].Value;
							DrawCompositeArmorPiece(ref drawinfo, CompositePlayerDrawContext.FrontShoulder, new DrawData(value2, position, drawinfo.compFrontShoulderFrame, drawinfo.colorArmorBody, bodyRotation, bodyVect, 1f, drawinfo.playerEffect)
							{
								shader = 0
							}, drawinfo.drawPlayer.body);
							if (drawinfo.drawPlayer.legs == 60)
							{
								Texture2D value3 = TextureAssets.Extra[275].Value;
								DrawCompositeArmorPiece(ref drawinfo, CompositePlayerDrawContext.FrontShoulder, new DrawData(value3, position, drawinfo.compFrontShoulderFrame, drawinfo.colorArmorBody, bodyRotation, bodyVect, 1f, drawinfo.playerEffect)
								{
									shader = drawinfo.cLegs
								}, drawinfo.drawPlayer.body);
							}
						}
					}
					if (i == num3)
					{
						DrawCompositeArmorPiece(ref drawinfo, CompositePlayerDrawContext.FrontArm, new DrawData(value, val, drawinfo.compFrontArmFrame, drawinfo.colorArmorBody, rotation, bodyVect, 1f, drawinfo.playerEffect)
						{
							shader = drawinfo.cBody
						}, drawinfo.drawPlayer.body);
						if (drawinfo.drawPlayer.body == 71)
						{
							Texture2D value4 = TextureAssets.Extra[277].Value;
							DrawCompositeArmorPiece(ref drawinfo, CompositePlayerDrawContext.FrontArm, new DrawData(value4, val, drawinfo.compFrontArmFrame, drawinfo.colorArmorBody, rotation, bodyVect, 1f, drawinfo.playerEffect)
							{
								shader = 0
							}, drawinfo.drawPlayer.body);
						}
					}
				}
			}
		}
		else if (!drawinfo.drawPlayer.invis)
		{
			for (int j = 0; j < 2; j++)
			{
				if (j == num2)
				{
					if (flag2)
					{
						drawinfo.DrawDataCache.Add(new DrawData(TextureAssets.Players[drawinfo.skinVar, 7].Value, position, drawinfo.compFrontShoulderFrame, drawinfo.colorBodySkin, bodyRotation, bodyVect, 1f, drawinfo.playerEffect)
						{
							shader = drawinfo.skinDyePacked
						});
					}
					if (!drawinfo.hideCompositeShoulders)
					{
						drawinfo.DrawDataCache.Add(new DrawData(TextureAssets.Players[drawinfo.skinVar, 8].Value, position, drawinfo.compFrontShoulderFrame, drawinfo.colorUnderShirt, bodyRotation, bodyVect, 1f, drawinfo.playerEffect));
						drawinfo.DrawDataCache.Add(new DrawData(TextureAssets.Players[drawinfo.skinVar, 13].Value, position, drawinfo.compFrontShoulderFrame, drawinfo.colorShirt, bodyRotation, bodyVect, 1f, drawinfo.playerEffect));
						drawinfo.DrawDataCache.Add(new DrawData(TextureAssets.Players[drawinfo.skinVar, 6].Value, position, drawinfo.compFrontShoulderFrame, drawinfo.colorShirt, bodyRotation, bodyVect, 1f, drawinfo.playerEffect));
						if (drawinfo.drawPlayer.head == 269)
						{
							Vector2 pos = drawinfo.helmetOffset + new Vector2((float)(int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.headPosition + drawinfo.headVect;
							drawinfo.drawPlayer.ApplyHeadOffsetFromMount(ref pos);
							DrawData item = new DrawData(TextureAssets.Extra[214].Value, pos, drawinfo.drawPlayer.bodyFrame, drawinfo.colorArmorHead, drawinfo.drawPlayer.headRotation, drawinfo.headVect, 1f, drawinfo.playerEffect);
							item.shader = drawinfo.cHead;
							drawinfo.DrawDataCache.Add(item);
							item = new DrawData(TextureAssets.GlowMask[308].Value, pos, drawinfo.drawPlayer.bodyFrame, drawinfo.headGlowColor, drawinfo.drawPlayer.headRotation, drawinfo.headVect, 1f, drawinfo.playerEffect);
							item.shader = drawinfo.cHead;
							drawinfo.DrawDataCache.Add(item);
						}
					}
				}
				if (j == num3)
				{
					if (flag2)
					{
						drawinfo.DrawDataCache.Add(new DrawData(TextureAssets.Players[drawinfo.skinVar, 7].Value, val, drawinfo.compFrontArmFrame, drawinfo.colorBodySkin, rotation, bodyVect, 1f, drawinfo.playerEffect)
						{
							shader = drawinfo.skinDyePacked
						});
					}
					drawinfo.DrawDataCache.Add(new DrawData(TextureAssets.Players[drawinfo.skinVar, 8].Value, val, drawinfo.compFrontArmFrame, drawinfo.colorUnderShirt, rotation, bodyVect, 1f, drawinfo.playerEffect));
					drawinfo.DrawDataCache.Add(new DrawData(TextureAssets.Players[drawinfo.skinVar, 13].Value, val, drawinfo.compFrontArmFrame, drawinfo.colorShirt, rotation, bodyVect, 1f, drawinfo.playerEffect));
					drawinfo.DrawDataCache.Add(new DrawData(TextureAssets.Players[drawinfo.skinVar, 6].Value, val, drawinfo.compFrontArmFrame, drawinfo.colorShirt, rotation, bodyVect, 1f, drawinfo.playerEffect));
				}
			}
		}
		if (flag && (!drawinfo.drawPlayer.invis || IsArmorDrawnWhenInvisible(drawinfo.drawPlayer.coat)))
		{
			Texture2D value5 = TextureAssets.ArmorBodyComposite[drawinfo.drawPlayer.coat].Value;
			for (int k = 0; k < 2; k++)
			{
				if (k == num2 && !drawinfo.hideCompositeShoulders)
				{
					DrawCompositeArmorPiece(ref drawinfo, CompositePlayerDrawContext.FrontShoulder, new DrawData(value5, position, drawinfo.compFrontShoulderFrame, drawinfo.colorArmorBody, bodyRotation, bodyVect, 1f, drawinfo.playerEffect)
					{
						shader = drawinfo.cCoat
					}, drawinfo.drawPlayer.coat);
				}
				if (k == num3)
				{
					DrawCompositeArmorPiece(ref drawinfo, CompositePlayerDrawContext.FrontArm, new DrawData(value5, val, drawinfo.compFrontArmFrame, drawinfo.colorArmorBody, rotation, bodyVect, 1f, drawinfo.playerEffect)
					{
						shader = drawinfo.cCoat
					}, drawinfo.drawPlayer.coat);
				}
			}
		}
		if (drawinfo.drawPlayer.handon > 0 && drawinfo.drawPlayer.handon < ArmorIDs.HandOn.Count)
		{
			Texture2D value6 = TextureAssets.AccHandsOnComposite[drawinfo.drawPlayer.handon].Value;
			DrawCompositeArmorPiece(ref drawinfo, CompositePlayerDrawContext.FrontArmAccessory, new DrawData(value6, val, drawinfo.compFrontArmFrame, drawinfo.colorArmorBody, rotation, bodyVect, 1f, drawinfo.playerEffect)
			{
				shader = drawinfo.cHandOn
			}, -1);
		}
	}

	public static void DrawPlayer_29_OnhandAcc(ref PlayerDrawSet drawinfo)
	{
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_010a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0115: Unknown result type (might be due to invalid IL or missing references)
		//IL_0126: Unknown result type (might be due to invalid IL or missing references)
		//IL_0131: Unknown result type (might be due to invalid IL or missing references)
		if (!drawinfo.usesCompositeFrontHandAcc && drawinfo.drawPlayer.handon > 0 && drawinfo.drawPlayer.handon < ArmorIDs.HandOn.Count)
		{
			DrawData item = new DrawData(TextureAssets.AccHandsOn[drawinfo.drawPlayer.handon].Value, new Vector2((float)(int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.bodyPosition + new Vector2((float)(drawinfo.drawPlayer.bodyFrame.Width / 2), (float)(drawinfo.drawPlayer.bodyFrame.Height / 2)), drawinfo.drawPlayer.bodyFrame, drawinfo.colorArmorBody, drawinfo.drawPlayer.bodyRotation, drawinfo.bodyVect, 1f, drawinfo.playerEffect);
			item.shader = drawinfo.cHandOn;
			drawinfo.DrawDataCache.Add(item);
		}
	}

	public static void DrawPlayer_30_BladedGlove(ref PlayerDrawSet drawinfo)
	{
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_0183: Unknown result type (might be due to invalid IL or missing references)
		//IL_0196: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c5: Unknown result type (might be due to invalid IL or missing references)
		Item heldItem = drawinfo.heldItem;
		if (heldItem.type > -1 && Item.claw[heldItem.type] && drawinfo.shadow == 0f)
		{
			Main.instance.LoadItem(heldItem.type);
			Asset<Texture2D> val = TextureAssets.Item[heldItem.type];
			Vector2 origin = new Vector2((float)val.Width() * 0.5f - (float)val.Width() * 0.5f * (float)drawinfo.drawPlayer.direction, (float)((drawinfo.drawPlayer.gravDir != -1f) ? val.Height() : 0));
			if (drawinfo.drawPlayer.isDisplayDollOrInanimate)
			{
				origin = new Vector2((float)((drawinfo.drawPlayer.direction == -1) ? (val.Width() - 6) : 6), (float)(val.Height() - 6));
			}
			if (!drawinfo.drawPlayer.frozen && (drawinfo.drawPlayer.itemAnimation > 0 || (heldItem.holdStyle != 0 && !drawinfo.drawPlayer.pulley)) && heldItem.type > 0 && !drawinfo.drawPlayer.dead && !heldItem.noUseGraphic && (!drawinfo.drawPlayer.wet || !heldItem.noWet))
			{
				DrawData item = new DrawData(val.Value, new Vector2((float)(int)(drawinfo.ItemLocation.X - Main.screenPosition.X), (float)(int)(drawinfo.ItemLocation.Y - Main.screenPosition.Y)), new Rectangle(0, 0, val.Width(), val.Height()), heldItem.GetAlpha(drawinfo.itemColor), drawinfo.drawPlayer.itemRotation, origin, drawinfo.drawPlayer.GetAdjustedItemScale(heldItem), drawinfo.itemEffect);
				drawinfo.DrawDataCache.Add(item);
			}
		}
	}

	public static void DrawPlayer_31_ProjectileOverArm(ref PlayerDrawSet drawinfo)
	{
		if (drawinfo.SelectedDrawnProjectile != null && drawinfo.shadow == 0f && drawinfo.SelectedDrawnProjectile.drawLayer == 8)
		{
			drawinfo.projectileDrawPosition = drawinfo.DrawDataCache.Count;
		}
	}

	public static void DrawPlayer_32_FrontAcc(ref PlayerDrawSet drawinfo)
	{
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0112: Unknown result type (might be due to invalid IL or missing references)
		//IL_0117: Unknown result type (might be due to invalid IL or missing references)
		//IL_0122: Unknown result type (might be due to invalid IL or missing references)
		//IL_012d: Unknown result type (might be due to invalid IL or missing references)
		//IL_013e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0149: Unknown result type (might be due to invalid IL or missing references)
		if (drawinfo.drawPlayer.front > 0 && drawinfo.drawPlayer.front < ArmorIDs.Front.Count && !drawinfo.drawPlayer.mount.Active)
		{
			Vector2 zero = Vector2.Zero;
			DrawData item = new DrawData(TextureAssets.AccFront[drawinfo.drawPlayer.front].Value, zero + new Vector2((float)(int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.bodyPosition + new Vector2((float)(drawinfo.drawPlayer.bodyFrame.Width / 2), (float)(drawinfo.drawPlayer.bodyFrame.Height / 2)), drawinfo.drawPlayer.bodyFrame, drawinfo.colorArmorBody, drawinfo.drawPlayer.bodyRotation, drawinfo.bodyVect, 1f, drawinfo.playerEffect);
			item.shader = drawinfo.cFront;
			drawinfo.DrawDataCache.Add(item);
		}
	}

	public static void DrawPlayer_32_FrontAcc_FrontPart(ref PlayerDrawSet drawinfo)
	{
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_013e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0143: Unknown result type (might be due to invalid IL or missing references)
		//IL_014e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0153: Unknown result type (might be due to invalid IL or missing references)
		//IL_017e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0183: Unknown result type (might be due to invalid IL or missing references)
		//IL_0188: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0201: Unknown result type (might be due to invalid IL or missing references)
		//IL_020e: Unknown result type (might be due to invalid IL or missing references)
		//IL_021d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0223: Unknown result type (might be due to invalid IL or missing references)
		//IL_0226: Unknown result type (might be due to invalid IL or missing references)
		//IL_023f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0265: Unknown result type (might be due to invalid IL or missing references)
		//IL_0270: Unknown result type (might be due to invalid IL or missing references)
		//IL_0275: Unknown result type (might be due to invalid IL or missing references)
		//IL_0277: Unknown result type (might be due to invalid IL or missing references)
		//IL_028b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0290: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02be: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d9: Unknown result type (might be due to invalid IL or missing references)
		if (drawinfo.drawPlayer.front <= 0 || drawinfo.drawPlayer.front >= ArmorIDs.Front.Count || (ArmorIDs.Front.Sets.DontDrawIfWearingAScarfOrCape[drawinfo.drawPlayer.front] && ((drawinfo.drawPlayer.neck > 0 && ArmorIDs.Neck.Sets.IsAScarf[drawinfo.drawPlayer.neck]) || (drawinfo.drawPlayer.back > 0 && ArmorIDs.Back.Sets.IsACape[drawinfo.drawPlayer.back]))))
		{
			return;
		}
		Rectangle bodyFrame = drawinfo.drawPlayer.bodyFrame;
		int num = bodyFrame.Width / 2;
		bodyFrame.Width -= num;
		Vector2 bodyVect = drawinfo.bodyVect;
		if (((int)drawinfo.playerEffect & 1) != 0)
		{
			bodyVect.X -= num;
		}
		Vector2 val = drawinfo.drawPlayer.GetFrontDrawOffset() + new Vector2((float)(int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.bodyPosition + new Vector2((float)(drawinfo.drawPlayer.bodyFrame.Width / 2), (float)(drawinfo.drawPlayer.bodyFrame.Height / 2));
		DrawData item = new DrawData(TextureAssets.AccFront[drawinfo.drawPlayer.front].Value, val, bodyFrame, drawinfo.colorArmorBody, drawinfo.drawPlayer.bodyRotation, bodyVect, 1f, drawinfo.playerEffect);
		item.shader = drawinfo.cFront;
		drawinfo.DrawDataCache.Add(item);
		if (drawinfo.drawPlayer.front == 12)
		{
			Rectangle val2 = bodyFrame;
			Rectangle value = val2;
			value.Width = 2;
			int num2 = 0;
			int num3 = val2.Width / 2;
			int num4 = 2;
			if (((int)drawinfo.playerEffect & 1) != 0)
			{
				num2 = val2.Width - 2;
				num4 = -2;
			}
			for (int i = 0; i < num3; i++)
			{
				value.X = val2.X + 2 * i;
				Color immuneAlpha = drawinfo.drawPlayer.GetImmuneAlpha(LiquidRenderer.GetShimmerGlitterColor(top: true, (float)i / 16f, 0f), drawinfo.shadow);
				immuneAlpha *= (float)(int)drawinfo.colorArmorBody.A / 255f;
				item = new DrawData(TextureAssets.GlowMask[331].Value, val + new Vector2((float)(num2 + i * num4), 0f), value, immuneAlpha, drawinfo.drawPlayer.bodyRotation, bodyVect, 1f, drawinfo.playerEffect);
				item.shader = drawinfo.cFront;
				drawinfo.DrawDataCache.Add(item);
			}
		}
	}

	public static void DrawPlayer_32_FrontAcc_BackPart(ref PlayerDrawSet drawinfo)
	{
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_014a: Unknown result type (might be due to invalid IL or missing references)
		//IL_014f: Unknown result type (might be due to invalid IL or missing references)
		//IL_015a: Unknown result type (might be due to invalid IL or missing references)
		//IL_015f: Unknown result type (might be due to invalid IL or missing references)
		//IL_018a: Unknown result type (might be due to invalid IL or missing references)
		//IL_018f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0194: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_0208: Unknown result type (might be due to invalid IL or missing references)
		//IL_0209: Unknown result type (might be due to invalid IL or missing references)
		//IL_020b: Unknown result type (might be due to invalid IL or missing references)
		//IL_020d: Unknown result type (might be due to invalid IL or missing references)
		//IL_021a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0229: Unknown result type (might be due to invalid IL or missing references)
		//IL_022f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0232: Unknown result type (might be due to invalid IL or missing references)
		//IL_024b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0271: Unknown result type (might be due to invalid IL or missing references)
		//IL_027c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0281: Unknown result type (might be due to invalid IL or missing references)
		//IL_0283: Unknown result type (might be due to invalid IL or missing references)
		//IL_0297: Unknown result type (might be due to invalid IL or missing references)
		//IL_029c: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02de: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e5: Unknown result type (might be due to invalid IL or missing references)
		if (drawinfo.drawPlayer.front <= 0 || drawinfo.drawPlayer.front >= ArmorIDs.Front.Count || (ArmorIDs.Front.Sets.DontDrawIfWearingAScarfOrCape[drawinfo.drawPlayer.front] && ((drawinfo.drawPlayer.neck > 0 && ArmorIDs.Neck.Sets.IsAScarf[drawinfo.drawPlayer.neck]) || (drawinfo.drawPlayer.back > 0 && ArmorIDs.Back.Sets.IsACape[drawinfo.drawPlayer.back]))))
		{
			return;
		}
		Rectangle bodyFrame = drawinfo.drawPlayer.bodyFrame;
		int num = bodyFrame.Width / 2;
		bodyFrame.Width -= num;
		bodyFrame.X += num;
		Vector2 bodyVect = drawinfo.bodyVect;
		if (((int)drawinfo.playerEffect & 1) == 0)
		{
			bodyVect.X -= num;
		}
		Vector2 val = drawinfo.drawPlayer.GetFrontDrawOffset() + new Vector2((float)(int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.bodyPosition + new Vector2((float)(drawinfo.drawPlayer.bodyFrame.Width / 2), (float)(drawinfo.drawPlayer.bodyFrame.Height / 2));
		DrawData item = new DrawData(TextureAssets.AccFront[drawinfo.drawPlayer.front].Value, val, bodyFrame, drawinfo.colorArmorBody, drawinfo.drawPlayer.bodyRotation, bodyVect, 1f, drawinfo.playerEffect);
		item.shader = drawinfo.cFront;
		drawinfo.DrawDataCache.Add(item);
		if (drawinfo.drawPlayer.front == 12)
		{
			Rectangle val2 = bodyFrame;
			Rectangle value = val2;
			value.Width = 2;
			int num2 = 0;
			int num3 = val2.Width / 2;
			int num4 = 2;
			if (((int)drawinfo.playerEffect & 1) != 0)
			{
				num2 = val2.Width - 2;
				num4 = -2;
			}
			for (int i = 0; i < num3; i++)
			{
				value.X = val2.X + 2 * i;
				Color immuneAlpha = drawinfo.drawPlayer.GetImmuneAlpha(LiquidRenderer.GetShimmerGlitterColor(top: true, (float)i / 16f, 0f), drawinfo.shadow);
				immuneAlpha *= (float)(int)drawinfo.colorArmorBody.A / 255f;
				item = new DrawData(TextureAssets.GlowMask[331].Value, val + new Vector2((float)(num2 + i * num4), 0f), value, immuneAlpha, drawinfo.drawPlayer.bodyRotation, bodyVect, 1f, drawinfo.playerEffect);
				item.shader = drawinfo.cFront;
				drawinfo.DrawDataCache.Add(item);
			}
		}
	}

	public static void DrawPlayer_33_FrozenOrWebbedDebuff(ref PlayerDrawSet drawinfo)
	{
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0121: Unknown result type (might be due to invalid IL or missing references)
		//IL_012c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0131: Unknown result type (might be due to invalid IL or missing references)
		//IL_015c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0161: Unknown result type (might be due to invalid IL or missing references)
		//IL_017c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0186: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_020e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0218: Unknown result type (might be due to invalid IL or missing references)
		//IL_021d: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0309: Unknown result type (might be due to invalid IL or missing references)
		//IL_0316: Unknown result type (might be due to invalid IL or missing references)
		//IL_0320: Unknown result type (might be due to invalid IL or missing references)
		//IL_032b: Unknown result type (might be due to invalid IL or missing references)
		if (!drawinfo.drawPlayer.shimmering)
		{
			if (drawinfo.drawPlayer.frozen && drawinfo.shadow == 0f)
			{
				Color colorArmorBody = drawinfo.colorArmorBody;
				colorArmorBody.R = (byte)((double)(int)colorArmorBody.R * 0.55);
				colorArmorBody.G = (byte)((double)(int)colorArmorBody.G * 0.55);
				colorArmorBody.B = (byte)((double)(int)colorArmorBody.B * 0.55);
				colorArmorBody.A = (byte)((double)(int)colorArmorBody.A * 0.55);
				DrawData item = new DrawData(TextureAssets.Frozen.Value, new Vector2((float)(int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.bodyPosition + new Vector2((float)(drawinfo.drawPlayer.bodyFrame.Width / 2), (float)(drawinfo.drawPlayer.bodyFrame.Height / 2)), new Rectangle(0, 0, TextureAssets.Frozen.Width(), TextureAssets.Frozen.Height()), colorArmorBody, drawinfo.drawPlayer.bodyRotation, new Vector2((float)(TextureAssets.Frozen.Width() / 2), (float)(TextureAssets.Frozen.Height() / 2)), 1f, drawinfo.playerEffect);
				drawinfo.DrawDataCache.Add(item);
			}
			else if (drawinfo.drawPlayer.webbed && drawinfo.shadow == 0f && drawinfo.drawPlayer.velocity.Y == 0f)
			{
				Color color = drawinfo.colorArmorBody * 0.75f;
				Texture2D value = TextureAssets.Extra[31].Value;
				int num = drawinfo.drawPlayer.height / 2;
				DrawData item = new DrawData(value, new Vector2((float)(int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f + (float)num)) + drawinfo.drawPlayer.bodyPosition + new Vector2((float)(drawinfo.drawPlayer.bodyFrame.Width / 2), (float)(drawinfo.drawPlayer.bodyFrame.Height / 2)), null, color, drawinfo.drawPlayer.bodyRotation, value.Size() / 2f, 1f, drawinfo.playerEffect);
				drawinfo.DrawDataCache.Add(item);
			}
		}
	}

	public static void DrawPlayer_34_ElectrifiedDebuffFront(ref PlayerDrawSet drawinfo)
	{
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_010b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0110: Unknown result type (might be due to invalid IL or missing references)
		//IL_012e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0139: Unknown result type (might be due to invalid IL or missing references)
		//IL_015c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0167: Unknown result type (might be due to invalid IL or missing references)
		if (!drawinfo.drawPlayer.electrified || drawinfo.shadow != 0f)
		{
			return;
		}
		Texture2D value = TextureAssets.GlowMask[25].Value;
		int num = drawinfo.drawPlayer.miscCounter / 5;
		for (int i = 0; i < 2; i++)
		{
			num %= 7;
			if (num > 1 && num < 5)
			{
				DrawData item = new DrawData(value, new Vector2((float)(int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.bodyPosition + new Vector2((float)(drawinfo.drawPlayer.bodyFrame.Width / 2), (float)(drawinfo.drawPlayer.bodyFrame.Height / 2)), new Rectangle(0, num * value.Height / 7, value.Width, value.Height / 7), drawinfo.colorElectricity, drawinfo.drawPlayer.bodyRotation, new Vector2((float)(value.Width / 2), (float)(value.Height / 14)), 1f, drawinfo.playerEffect);
				drawinfo.DrawDataCache.Add(item);
			}
			num += 3;
		}
	}

	public static void DrawPlayer_35_IceBarrier(ref PlayerDrawSet drawinfo)
	{
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0116: Unknown result type (might be due to invalid IL or missing references)
		//IL_0120: Unknown result type (might be due to invalid IL or missing references)
		//IL_0140: Unknown result type (might be due to invalid IL or missing references)
		//IL_014b: Unknown result type (might be due to invalid IL or missing references)
		if (drawinfo.drawPlayer.iceBarrier && drawinfo.shadow == 0f)
		{
			int num = TextureAssets.IceBarrier.Height() / 12;
			Color white = Color.White;
			DrawData item = new DrawData(TextureAssets.IceBarrier.Value, new Vector2((float)(int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.bodyPosition + new Vector2((float)(drawinfo.drawPlayer.bodyFrame.Width / 2), (float)(drawinfo.drawPlayer.bodyFrame.Height / 2)), new Rectangle(0, num * drawinfo.drawPlayer.iceBarrierFrame, TextureAssets.IceBarrier.Width(), num), white, 0f, new Vector2((float)(TextureAssets.Frozen.Width() / 2), (float)(TextureAssets.Frozen.Height() / 2)), 1f, drawinfo.playerEffect);
			drawinfo.DrawDataCache.Add(item);
		}
	}

	public static void DrawPlayer_36_CTG(ref PlayerDrawSet drawinfo)
	{
		//IL_0129: Unknown result type (might be due to invalid IL or missing references)
		//IL_014e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0180: Unknown result type (might be due to invalid IL or missing references)
		//IL_0185: Unknown result type (might be due to invalid IL or missing references)
		//IL_0188: Unknown result type (might be due to invalid IL or missing references)
		//IL_018d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0192: Unknown result type (might be due to invalid IL or missing references)
		//IL_0131: Unknown result type (might be due to invalid IL or missing references)
		//IL_0136: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_0211: Unknown result type (might be due to invalid IL or missing references)
		//IL_0213: Unknown result type (might be due to invalid IL or missing references)
		//IL_0215: Unknown result type (might be due to invalid IL or missing references)
		//IL_0217: Unknown result type (might be due to invalid IL or missing references)
		//IL_021e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0223: Unknown result type (might be due to invalid IL or missing references)
		//IL_0248: Unknown result type (might be due to invalid IL or missing references)
		//IL_0254: Unknown result type (might be due to invalid IL or missing references)
		//IL_025e: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e4: Unknown result type (might be due to invalid IL or missing references)
		if (drawinfo.shadow != 0f || (byte)drawinfo.drawPlayer.ownedLargeGems <= 0)
		{
			return;
		}
		bool flag = false;
		BitsByte ownedLargeGems = drawinfo.drawPlayer.ownedLargeGems;
		float num = 0f;
		for (int i = 0; i < 7; i++)
		{
			if (ownedLargeGems[i])
			{
				num++;
			}
		}
		float num2 = 1f - num * 0.06f;
		float num3 = (num - 1f) * 4f;
		switch ((int)num)
		{
		case 2:
			num3 += 10f;
			break;
		case 3:
			num3 += 8f;
			break;
		case 4:
			num3 += 6f;
			break;
		case 5:
			num3 += 6f;
			break;
		case 6:
			num3 += 2f;
			break;
		case 7:
			num3 += 0f;
			break;
		}
		float num4 = (float)drawinfo.drawPlayer.miscCounter / 300f * ((float)Math.PI * 2f);
		if (!(num > 0f))
		{
			return;
		}
		float num5 = (float)Math.PI * 2f / num;
		float num6 = 0f;
		Vector2 val = new Vector2(1.3f, 0.65f);
		if (!flag)
		{
			val = Vector2.One;
		}
		float num7 = 38f - drawinfo.drawPlayer.HeightOffsetVisual;
		Vector2 val2 = new Vector2((float)(int)(drawinfo.Center.X - Main.screenPosition.X), (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y - num7));
		val2 -= drawinfo.VisualPositionOffset;
		List<DrawData> list = new List<DrawData>();
		for (int j = 0; j < 7; j++)
		{
			if (!ownedLargeGems[j])
			{
				num6++;
				continue;
			}
			Vector2 val3 = (num4 + num5 * ((float)j - num6)).ToRotationVector2();
			float num8 = num2;
			if (flag)
			{
				num8 = MathHelper.Lerp(num2 * 0.7f, 1f, val3.Y / 2f + 0.5f);
			}
			Texture2D value = TextureAssets.Gem[j].Value;
			DrawData item = new DrawData(value, val2 + val3 * val * num3, null, new Color(250, 250, 250, Main.mouseTextColor / 2), 0f, value.Size() / 2f, ((float)(int)Main.mouseTextColor / 1000f + 0.8f) * num8, (SpriteEffects)0);
			list.Add(item);
		}
		if (flag)
		{
			list.Sort(DelegateMethods.CompareDrawSorterByYScale);
		}
		drawinfo.DrawDataCache.AddRange(list);
	}

	public static void DrawPlayer_37_BeetleBuff(ref PlayerDrawSet drawinfo)
	{
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_011b: Unknown result type (might be due to invalid IL or missing references)
		//IL_012c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0131: Unknown result type (might be due to invalid IL or missing references)
		//IL_0136: Unknown result type (might be due to invalid IL or missing references)
		//IL_0138: Unknown result type (might be due to invalid IL or missing references)
		//IL_0170: Unknown result type (might be due to invalid IL or missing references)
		//IL_017a: Unknown result type (might be due to invalid IL or missing references)
		//IL_019a: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0225: Unknown result type (might be due to invalid IL or missing references)
		//IL_0236: Unknown result type (might be due to invalid IL or missing references)
		//IL_023b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0273: Unknown result type (might be due to invalid IL or missing references)
		//IL_027e: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ad: Unknown result type (might be due to invalid IL or missing references)
		if ((!drawinfo.drawPlayer.beetleOffense && !drawinfo.drawPlayer.beetleDefense) || drawinfo.shadow != 0f)
		{
			return;
		}
		for (int i = 0; i < drawinfo.drawPlayer.beetleOrbs; i++)
		{
			DrawData item;
			for (int j = 0; j < 5; j++)
			{
				Color colorArmorBody = drawinfo.colorArmorBody;
				float num = (float)j * 0.1f;
				num = 0.5f - num;
				colorArmorBody.R = (byte)((float)(int)colorArmorBody.R * num);
				colorArmorBody.G = (byte)((float)(int)colorArmorBody.G * num);
				colorArmorBody.B = (byte)((float)(int)colorArmorBody.B * num);
				colorArmorBody.A = (byte)((float)(int)colorArmorBody.A * num);
				Vector2 val = -drawinfo.drawPlayer.beetleVel[i] * (float)j;
				item = new DrawData(TextureAssets.Beetle.Value, new Vector2((float)(int)(drawinfo.Position.X - Main.screenPosition.X + (float)(drawinfo.drawPlayer.width / 2)), (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)(drawinfo.drawPlayer.height / 2))) + drawinfo.drawPlayer.beetlePos[i] + val, new Rectangle(0, TextureAssets.Beetle.Height() / 3 * drawinfo.drawPlayer.beetleFrame + 1, TextureAssets.Beetle.Width(), TextureAssets.Beetle.Height() / 3 - 2), colorArmorBody, 0f, new Vector2((float)(TextureAssets.Beetle.Width() / 2), (float)(TextureAssets.Beetle.Height() / 6)), 1f, drawinfo.playerEffect);
				drawinfo.DrawDataCache.Add(item);
			}
			item = new DrawData(TextureAssets.Beetle.Value, new Vector2((float)(int)(drawinfo.Position.X - Main.screenPosition.X + (float)(drawinfo.drawPlayer.width / 2)), (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)(drawinfo.drawPlayer.height / 2))) + drawinfo.drawPlayer.beetlePos[i], new Rectangle(0, TextureAssets.Beetle.Height() / 3 * drawinfo.drawPlayer.beetleFrame + 1, TextureAssets.Beetle.Width(), TextureAssets.Beetle.Height() / 3 - 2), drawinfo.colorArmorBody, 0f, new Vector2((float)(TextureAssets.Beetle.Width() / 2), (float)(TextureAssets.Beetle.Height() / 6)), 1f, drawinfo.playerEffect);
			drawinfo.DrawDataCache.Add(item);
		}
	}

	public static void DrawPlayer_38_EyebrellaCloud(ref PlayerDrawSet drawinfo)
	{
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0100: Unknown result type (might be due to invalid IL or missing references)
		//IL_0102: Unknown result type (might be due to invalid IL or missing references)
		//IL_0107: Unknown result type (might be due to invalid IL or missing references)
		//IL_010c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0111: Unknown result type (might be due to invalid IL or missing references)
		//IL_0119: Unknown result type (might be due to invalid IL or missing references)
		//IL_0128: Unknown result type (might be due to invalid IL or missing references)
		//IL_012d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0132: Unknown result type (might be due to invalid IL or missing references)
		//IL_0137: Unknown result type (might be due to invalid IL or missing references)
		//IL_013c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0195: Unknown result type (might be due to invalid IL or missing references)
		//IL_027c: Unknown result type (might be due to invalid IL or missing references)
		//IL_027e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0284: Unknown result type (might be due to invalid IL or missing references)
		//IL_028b: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0203: Unknown result type (might be due to invalid IL or missing references)
		//IL_0205: Unknown result type (might be due to invalid IL or missing references)
		//IL_0207: Unknown result type (might be due to invalid IL or missing references)
		//IL_020c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0212: Unknown result type (might be due to invalid IL or missing references)
		//IL_0219: Unknown result type (might be due to invalid IL or missing references)
		//IL_0223: Unknown result type (might be due to invalid IL or missing references)
		if (drawinfo.drawPlayer.eyebrellaCloud && drawinfo.shadow == 0f)
		{
			Texture2D value = TextureAssets.Projectile[238].Value;
			int frameY = drawinfo.drawPlayer.miscCounter % 18 / 6;
			Rectangle val = value.Frame(1, 6, 0, frameY);
			Vector2 origin = new Vector2((float)(val.Width / 2), (float)(val.Height / 2));
			Vector2 pos = new Vector2(0f, -70f);
			drawinfo.drawPlayer.ApplyHeadOffsetFromMount(ref pos);
			if (drawinfo.drawPlayer.mount.Active && drawinfo.drawPlayer.mount.Type == 54)
			{
				pos += new Vector2(-14f, 0f) * drawinfo.drawPlayer.Directions;
			}
			Vector2 val2 = drawinfo.drawPlayer.MountedCenter - new Vector2(0f, (float)drawinfo.drawPlayer.height * 0.5f) + pos - Main.screenPosition;
			Color color = Lighting.GetColor((drawinfo.drawPlayer.Top + new Vector2(0f, -30f)).ToTileCoordinates());
			int num = 170;
			int num3;
			int num4;
			int num2 = (num3 = (num4 = num));
			if (color.R < num)
			{
				num2 = color.R;
			}
			if (color.G < num)
			{
				num3 = color.G;
			}
			if (color.B < num)
			{
				num4 = color.B;
			}
			Color val3 = new Color(num2, num3, num4, 100);
			float num5 = (float)(drawinfo.drawPlayer.miscCounter % 50) / 50f;
			float num6 = 3f;
			DrawData item;
			for (int i = 0; i < 2; i++)
			{
				Vector2 val4 = new Vector2((i == 0) ? (0f - num6) : num6, 0f).RotatedBy(num5 * ((float)Math.PI * 2f) * ((i == 0) ? 1f : (-1f)));
				item = new DrawData(value, val2 + val4, val, val3 * 0.65f, 0f, origin, 1f, (SpriteEffects)((drawinfo.drawPlayer.gravDir == -1f) ? 2 : 0));
				item.shader = drawinfo.cHead;
				item.ignorePlayerRotation = true;
				drawinfo.DrawDataCache.Add(item);
			}
			item = new DrawData(value, val2, val, val3, 0f, origin, 1f, (SpriteEffects)((drawinfo.drawPlayer.gravDir == -1f) ? 2 : 0));
			item.shader = drawinfo.cHead;
			item.ignorePlayerRotation = true;
			drawinfo.DrawDataCache.Add(item);
		}
	}

	private static Vector2 GetCompositeOffset_BackArm(ref PlayerDrawSet drawinfo)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		return new Vector2((float)(6 * ((((int)drawinfo.playerEffect & 1) == 0) ? 1 : (-1))), (float)(2 * ((((int)drawinfo.playerEffect & 2) == 0) ? 1 : (-1))));
	}

	private static Vector2 GetCompositeOffset_FrontArm(ref PlayerDrawSet drawinfo)
	{
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		return new Vector2((float)(-5 * ((((int)drawinfo.playerEffect & 1) == 0) ? 1 : (-1))), 0f);
	}

	public static void DrawPlayer_TransformDrawData(ref PlayerDrawSet drawinfo)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0130: Unknown result type (might be due to invalid IL or missing references)
		//IL_0135: Unknown result type (might be due to invalid IL or missing references)
		//IL_0136: Unknown result type (might be due to invalid IL or missing references)
		//IL_013b: Unknown result type (might be due to invalid IL or missing references)
		//IL_013d: Unknown result type (might be due to invalid IL or missing references)
		//IL_013f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0140: Unknown result type (might be due to invalid IL or missing references)
		//IL_0145: Unknown result type (might be due to invalid IL or missing references)
		//IL_0149: Unknown result type (might be due to invalid IL or missing references)
		//IL_014b: Unknown result type (might be due to invalid IL or missing references)
		//IL_014c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0151: Unknown result type (might be due to invalid IL or missing references)
		_ = drawinfo.rotation;
		_ = 0f;
		Vector2 val = drawinfo.Position - Main.screenPosition + drawinfo.rotationOrigin;
		Vector2 val2 = drawinfo.drawPlayer.position + drawinfo.rotationOrigin;
		Matrix val3 = Matrix.CreateRotationZ(drawinfo.rotation);
		for (int i = 0; i < drawinfo.DustCache.Count; i++)
		{
			Vector2 val4 = Main.dust[drawinfo.DustCache[i]].position - val2;
			val4 = Vector2.Transform(val4, val3);
			Main.dust[drawinfo.DustCache[i]].position = val4 + val2;
		}
		for (int j = 0; j < drawinfo.GoreCache.Count; j++)
		{
			Vector2 val5 = Main.gore[drawinfo.GoreCache[j]].position - val2;
			val5 = Vector2.Transform(val5, val3);
			Main.gore[drawinfo.GoreCache[j]].position = val5 + val2;
		}
		for (int k = 0; k < drawinfo.DrawDataCache.Count; k++)
		{
			DrawData value = drawinfo.DrawDataCache[k];
			if (!value.ignorePlayerRotation)
			{
				Vector2 val6 = value.position - val;
				val6 = Vector2.Transform(val6, val3);
				value.position = val6 + val;
				value.rotation += drawinfo.rotation;
				drawinfo.DrawDataCache[k] = value;
			}
		}
	}

	public static void DrawPlayer_ScaleDrawData(ref PlayerDrawSet drawinfo, float scale)
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		if (scale != 1f)
		{
			Vector2 val = drawinfo.Position + drawinfo.drawPlayer.Size * new Vector2(0.5f, 1f) - Main.screenPosition;
			for (int i = 0; i < drawinfo.DrawDataCache.Count; i++)
			{
				DrawData value = drawinfo.DrawDataCache[i];
				Vector2 val2 = value.position - val;
				value.position = val + val2 * scale;
				ref Vector2 scale2 = ref value.scale;
				scale2 *= scale;
				drawinfo.DrawDataCache[i] = value;
			}
		}
	}

	public static void DrawPlayer_AddSelectionGlow(ref PlayerDrawSet drawinfo)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		if (!(drawinfo.selectionGlowColor == Color.Transparent))
		{
			Color selectionGlowColor = drawinfo.selectionGlowColor;
			List<DrawData> list = new List<DrawData>();
			list.AddRange(GetFlatColoredCloneData(ref drawinfo, new Vector2(0f, -2f), selectionGlowColor));
			list.AddRange(GetFlatColoredCloneData(ref drawinfo, new Vector2(0f, 2f), selectionGlowColor));
			list.AddRange(GetFlatColoredCloneData(ref drawinfo, new Vector2(2f, 0f), selectionGlowColor));
			list.AddRange(GetFlatColoredCloneData(ref drawinfo, new Vector2(-2f, 0f), selectionGlowColor));
			list.AddRange(drawinfo.DrawDataCache);
			drawinfo.DrawDataCache = list;
		}
	}

	public static void DrawPlayer_MakeIntoFirstFractalAfterImage(ref PlayerDrawSet drawinfo)
	{
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		if (!drawinfo.drawPlayer.isFirstFractalAfterImage)
		{
			if (drawinfo.drawPlayer.HeldItem.type == 4722)
			{
				_ = drawinfo.drawPlayer.itemAnimation > 0;
			}
			else
				_ = 0;
			return;
		}
		for (int i = 0; i < drawinfo.DrawDataCache.Count; i++)
		{
			DrawData value = drawinfo.DrawDataCache[i];
			ref Color color = ref value.color;
			color *= drawinfo.drawPlayer.firstFractalAfterImageOpacity;
			value.color.A = (byte)((float)(int)value.color.A * 0.8f);
			drawinfo.DrawDataCache[i] = value;
		}
	}

	public static void DrawPlayer_RenderAllLayers(ref PlayerDrawSet drawinfo)
	{
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		SpriteDrawBuffer spriteBuffer = Main.spriteBuffer;
		List<DrawData> drawDataCache = drawinfo.DrawDataCache;
		foreach (DrawData item in drawDataCache)
		{
			if (item.texture != null)
			{
				item.Draw(spriteBuffer);
			}
		}
		DrawData cdd = default;
		int num = 0;
		for (int i = 0; i <= drawDataCache.Count; i++)
		{
			if (drawinfo.projectileDrawPosition == i)
			{
				if (cdd.shader != 0)
				{
					Main.pixelShader.CurrentTechnique.Passes[0].Apply();
				}
				spriteBuffer.Unbind();
				DrawHeldProj(drawinfo, drawinfo.SelectedDrawnProjectile);
			}
			if (i != drawDataCache.Count)
			{
				cdd = drawDataCache[i];
				if (!cdd.sourceRect.HasValue)
				{
					cdd.sourceRect = cdd.texture.Frame();
				}
				PlayerDrawHelper.SetShaderForData(drawinfo.drawPlayer, drawinfo.cHead, ref cdd);
				if (cdd.texture != null)
				{
					spriteBuffer.DrawSingle(num++);
				}
			}
		}
		spriteBuffer.Unbind();
		Main.pixelShader.CurrentTechnique.Passes[0].Apply();
	}

	private static void DrawHeldProj(PlayerDrawSet drawinfo, Projectile proj)
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		SamplerState val = Main.graphics.GraphicsDevice.SamplerStates[0];
		Player player = Main.player[proj.owner];
		player.position += player.netOffset;
		proj.position += player.netOffset;
		try
		{
			Main.instance.DrawProjDirect(proj, drawinfo.drawPlayer);
			if (proj.type == 595 || proj.type == 735 || proj.type == 927)
			{
				Main.instance.DrawProjDirect(proj, drawinfo.drawPlayer);
			}
		}
		catch
		{
			proj.active = false;
		}
		player.position -= player.netOffset;
		proj.position -= player.netOffset;
		Main.graphics.GraphicsDevice.SamplerStates[0] = val;
	}

	public static void DrawPlayer_RenderAllLayersSlow(ref PlayerDrawSet drawinfo)
	{
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		int num = -1;
		List<DrawData> drawDataCache = drawinfo.DrawDataCache;
		Effect pixelShader = Main.pixelShader;
		_ = Main.projectile;
		SpriteBatch spriteBatch = Main.spriteBatch;
		for (int i = 0; i <= drawDataCache.Count; i++)
		{
			if (drawinfo.projectileDrawPosition == i)
			{
				if (num != 0)
				{
					pixelShader.CurrentTechnique.Passes[0].Apply();
					num = 0;
				}
				try
				{
					Main.instance.DrawProjDirect(drawinfo.SelectedDrawnProjectile, drawinfo.drawPlayer);
				}
				catch
				{
					drawinfo.SelectedDrawnProjectile.active = false;
				}
			}
			if (i != drawDataCache.Count)
			{
				DrawData cdd = drawDataCache[i];
				if (!cdd.sourceRect.HasValue)
				{
					cdd.sourceRect = cdd.texture.Frame();
				}
				PlayerDrawHelper.SetShaderForData(drawinfo.drawPlayer, drawinfo.cHead, ref cdd);
				num = cdd.shader;
				if (cdd.texture != null)
				{
					cdd.Draw(spriteBatch);
				}
			}
		}
		pixelShader.CurrentTechnique.Passes[0].Apply();
	}

	public static void DrawPlayer_DrawSelectionRect(ref PlayerDrawSet drawinfo)
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		SpriteRenderTargetHelper.GetDrawBoundary(drawinfo.DrawDataCache, out var lowest, out var highest);
		Utils.DrawRect(Main.spriteBatch, lowest + Main.screenPosition, highest + Main.screenPosition, Color.White);
	}

	private static bool IsArmorDrawnWhenInvisible(int torsoID)
	{
		if ((uint)(torsoID - 21) <= 1u)
		{
			return false;
		}
		return true;
	}

	private static DrawData[] GetFlatColoredCloneData(ref PlayerDrawSet drawinfo, Vector2 offset, Color color)
	{
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		int colorOnlyShaderIndex = ContentSamples.DyeShaderIDs.ColorOnlyShaderIndex;
		DrawData[] array = new DrawData[drawinfo.DrawDataCache.Count];
		for (int i = 0; i < drawinfo.DrawDataCache.Count; i++)
		{
			DrawData drawData = drawinfo.DrawDataCache[i];
			ref Vector2 position = ref drawData.position;
			position += offset;
			drawData.shader = colorOnlyShaderIndex;
			drawData.color = color;
			array[i] = drawData;
		}
		return array;
	}
}
