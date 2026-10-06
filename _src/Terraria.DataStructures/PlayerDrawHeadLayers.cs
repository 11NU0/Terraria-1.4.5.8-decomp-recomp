using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using Terraria.Graphics;
using Terraria.ID;

namespace Terraria.DataStructures;

public static class PlayerDrawHeadLayers
{
	public static void DrawPlayer_0_(ref PlayerDrawHeadSet drawinfo)
	{
	}

	public static void DrawPlayer_00_BackHelmet(ref PlayerDrawHeadSet drawinfo)
	{
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0104: Unknown result type (might be due to invalid IL or missing references)
		//IL_0110: Unknown result type (might be due to invalid IL or missing references)
		if (drawinfo.drawPlayer.head >= 0 && drawinfo.drawPlayer.head < ArmorIDs.Head.Count)
		{
			int num = ArmorIDs.Head.Sets.FrontToBackID[drawinfo.drawPlayer.head];
			if (num >= 0)
			{
				Rectangle hairFrame = drawinfo.HairFrame;
				QuickCDD(drawinfo.DrawData, drawinfo.cHead, TextureAssets.ArmorHead[num].Value, drawinfo.helmetOffset + new Vector2(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.bodyFrameMemory.Width / 2) + (float)(drawinfo.drawPlayer.width / 2), drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.bodyFrameMemory.Height + 4f) + drawinfo.drawPlayer.headPosition + drawinfo.headVect, hairFrame, drawinfo.colorArmorHead, drawinfo.drawPlayer.headRotation, drawinfo.headVect, drawinfo.scale, drawinfo.playerEffect, 0f);
			}
		}
	}

	public static void DrawPlayer_01_FaceSkin(ref PlayerDrawHeadSet drawinfo)
	{
		//IL_0342: Unknown result type (might be due to invalid IL or missing references)
		//IL_034d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0352: Unknown result type (might be due to invalid IL or missing references)
		//IL_0358: Unknown result type (might be due to invalid IL or missing references)
		//IL_035d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0363: Unknown result type (might be due to invalid IL or missing references)
		//IL_036e: Unknown result type (might be due to invalid IL or missing references)
		//IL_037f: Unknown result type (might be due to invalid IL or missing references)
		//IL_038b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0420: Unknown result type (might be due to invalid IL or missing references)
		//IL_042b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0430: Unknown result type (might be due to invalid IL or missing references)
		//IL_0436: Unknown result type (might be due to invalid IL or missing references)
		//IL_043b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0441: Unknown result type (might be due to invalid IL or missing references)
		//IL_044c: Unknown result type (might be due to invalid IL or missing references)
		//IL_045d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0469: Unknown result type (might be due to invalid IL or missing references)
		//IL_04fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0509: Unknown result type (might be due to invalid IL or missing references)
		//IL_050e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0514: Unknown result type (might be due to invalid IL or missing references)
		//IL_0519: Unknown result type (might be due to invalid IL or missing references)
		//IL_051f: Unknown result type (might be due to invalid IL or missing references)
		//IL_052a: Unknown result type (might be due to invalid IL or missing references)
		//IL_053b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0547: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0102: Unknown result type (might be due to invalid IL or missing references)
		//IL_0107: Unknown result type (might be due to invalid IL or missing references)
		//IL_0108: Unknown result type (might be due to invalid IL or missing references)
		//IL_010e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0119: Unknown result type (might be due to invalid IL or missing references)
		//IL_012a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0136: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0604: Unknown result type (might be due to invalid IL or missing references)
		//IL_060a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0615: Unknown result type (might be due to invalid IL or missing references)
		//IL_0626: Unknown result type (might be due to invalid IL or missing references)
		//IL_0632: Unknown result type (might be due to invalid IL or missing references)
		//IL_070b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0716: Unknown result type (might be due to invalid IL or missing references)
		//IL_071b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0721: Unknown result type (might be due to invalid IL or missing references)
		//IL_0726: Unknown result type (might be due to invalid IL or missing references)
		//IL_072c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0737: Unknown result type (might be due to invalid IL or missing references)
		//IL_0748: Unknown result type (might be due to invalid IL or missing references)
		//IL_0754: Unknown result type (might be due to invalid IL or missing references)
		//IL_0247: Unknown result type (might be due to invalid IL or missing references)
		//IL_0252: Unknown result type (might be due to invalid IL or missing references)
		//IL_0257: Unknown result type (might be due to invalid IL or missing references)
		//IL_025d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0262: Unknown result type (might be due to invalid IL or missing references)
		//IL_0268: Unknown result type (might be due to invalid IL or missing references)
		//IL_0273: Unknown result type (might be due to invalid IL or missing references)
		//IL_0284: Unknown result type (might be due to invalid IL or missing references)
		//IL_0290: Unknown result type (might be due to invalid IL or missing references)
		bool flag = drawinfo.drawPlayer.head >= 0 && ArmorIDs.Head.Sets.HidesHead[drawinfo.drawPlayer.head];
		if (!flag && drawinfo.drawPlayer.faceHead > 0 && drawinfo.drawPlayer.faceHead < ArmorIDs.Face.Count)
		{
			Vector2 faceHeadOffsetFromHelmet = drawinfo.drawPlayer.GetFaceHeadOffsetFromHelmet();
			QuickCDD(drawinfo.DrawData, drawinfo.cFaceHead, TextureAssets.AccFace[drawinfo.drawPlayer.faceHead].Value, new Vector2(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.bodyFrameMemory.Width / 2) + (float)(drawinfo.drawPlayer.width / 2), drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.bodyFrameMemory.Height + 4f) + drawinfo.drawPlayer.headPosition + drawinfo.headVect + faceHeadOffsetFromHelmet, drawinfo.bodyFrameMemory, drawinfo.colorArmorHead, drawinfo.drawPlayer.headRotation, drawinfo.headVect, drawinfo.scale, drawinfo.playerEffect, 0f);
			if (drawinfo.drawPlayer.face <= 0 || drawinfo.drawPlayer.face >= ArmorIDs.Face.Count || !ArmorIDs.Face.Sets.DrawInFaceUnderHairLayer[drawinfo.drawPlayer.face])
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
			QuickCDD(drawinfo.DrawData, drawinfo.cFace, TextureAssets.AccFace[drawinfo.drawPlayer.face].Value, new Vector2((float)(int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.bodyFrameMemory.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)) + num, drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.bodyFrameMemory.Height + 4f) + drawinfo.drawPlayer.headPosition + drawinfo.headVect, drawinfo.bodyFrameMemory, drawinfo.colorArmorHead, drawinfo.drawPlayer.headRotation, drawinfo.headVect, drawinfo.scale, drawinfo.playerEffect, 0f);
		}
		else if (!flag && !drawinfo.drawPlayer.isHatRackDoll)
		{
			QuickCDD(drawinfo.DrawData, drawinfo.skinDyePacked, TextureAssets.Players[drawinfo.skinVar, 0].Value, new Vector2(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.bodyFrameMemory.Width / 2) + (float)(drawinfo.drawPlayer.width / 2), drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.bodyFrameMemory.Height + 4f) + drawinfo.drawPlayer.headPosition + drawinfo.headVect, drawinfo.bodyFrameMemory, drawinfo.colorHead, drawinfo.drawPlayer.headRotation, drawinfo.headVect, drawinfo.scale, drawinfo.playerEffect, 0f);
			QuickCDD(drawinfo.DrawData, TextureAssets.Players[drawinfo.skinVar, 1].Value, new Vector2(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.bodyFrameMemory.Width / 2) + (float)(drawinfo.drawPlayer.width / 2), drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.bodyFrameMemory.Height + 4f) + drawinfo.drawPlayer.headPosition + drawinfo.headVect, drawinfo.bodyFrameMemory, drawinfo.colorEyeWhites, drawinfo.drawPlayer.headRotation, drawinfo.headVect, drawinfo.scale, drawinfo.playerEffect, 0f);
			QuickCDD(drawinfo.DrawData, TextureAssets.Players[drawinfo.skinVar, 2].Value, new Vector2(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.bodyFrameMemory.Width / 2) + (float)(drawinfo.drawPlayer.width / 2), drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.bodyFrameMemory.Height + 4f) + drawinfo.drawPlayer.headPosition + drawinfo.headVect, drawinfo.bodyFrameMemory, drawinfo.colorEyes, drawinfo.drawPlayer.headRotation, drawinfo.headVect, drawinfo.scale, drawinfo.playerEffect, 0f);
			if (drawinfo.drawPlayer.yoraiz0rDarkness)
			{
				QuickCDD(drawinfo.DrawData, drawinfo.skinDyePacked, TextureAssets.Extra[67].Value, new Vector2(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.bodyFrameMemory.Width / 2) + (float)(drawinfo.drawPlayer.width / 2), drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.bodyFrameMemory.Height + 4f) + drawinfo.drawPlayer.headPosition + drawinfo.headVect, drawinfo.bodyFrameMemory, drawinfo.colorHead, drawinfo.drawPlayer.headRotation, drawinfo.headVect, drawinfo.scale, drawinfo.playerEffect, 0f);
			}
			if (drawinfo.drawPlayer.face > 0 && drawinfo.drawPlayer.face < ArmorIDs.Face.Count && ArmorIDs.Face.Sets.DrawInFaceUnderHairLayer[drawinfo.drawPlayer.face])
			{
				QuickCDD(drawinfo.DrawData, drawinfo.cFace, TextureAssets.AccFace[drawinfo.drawPlayer.face].Value, new Vector2((float)(int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.bodyFrameMemory.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.bodyFrameMemory.Height + 4f) + drawinfo.drawPlayer.headPosition + drawinfo.headVect, drawinfo.bodyFrameMemory, drawinfo.colorArmorHead, drawinfo.drawPlayer.headRotation, drawinfo.headVect, drawinfo.scale, drawinfo.playerEffect, 0f);
			}
		}
	}

	public static void DrawPlayer_02_DrawArmorWithFullHair(ref PlayerDrawHeadSet drawinfo)
	{
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_0118: Unknown result type (might be due to invalid IL or missing references)
		//IL_011d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0128: Unknown result type (might be due to invalid IL or missing references)
		//IL_012d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0133: Unknown result type (might be due to invalid IL or missing references)
		//IL_0138: Unknown result type (might be due to invalid IL or missing references)
		//IL_013e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0149: Unknown result type (might be due to invalid IL or missing references)
		//IL_015a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0166: Unknown result type (might be due to invalid IL or missing references)
		//IL_0181: Unknown result type (might be due to invalid IL or missing references)
		//IL_0186: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0252: Unknown result type (might be due to invalid IL or missing references)
		//IL_0257: Unknown result type (might be due to invalid IL or missing references)
		//IL_0262: Unknown result type (might be due to invalid IL or missing references)
		//IL_0267: Unknown result type (might be due to invalid IL or missing references)
		//IL_026d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0272: Unknown result type (might be due to invalid IL or missing references)
		//IL_0278: Unknown result type (might be due to invalid IL or missing references)
		//IL_0282: Unknown result type (might be due to invalid IL or missing references)
		//IL_028f: Unknown result type (might be due to invalid IL or missing references)
		//IL_029b: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01af: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_036b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0376: Unknown result type (might be due to invalid IL or missing references)
		//IL_037b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0381: Unknown result type (might be due to invalid IL or missing references)
		//IL_0386: Unknown result type (might be due to invalid IL or missing references)
		//IL_038c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0391: Unknown result type (might be due to invalid IL or missing references)
		//IL_0396: Unknown result type (might be due to invalid IL or missing references)
		//IL_039e: Unknown result type (might be due to invalid IL or missing references)
		//IL_03af: Unknown result type (might be due to invalid IL or missing references)
		//IL_03bb: Unknown result type (might be due to invalid IL or missing references)
		bool flag = drawinfo.drawPlayer.faceMask > 0 && drawinfo.drawPlayer.faceMask < ArmorIDs.Face.Count;
		if (flag && drawinfo.drawPlayer.head > 0 && drawinfo.drawPlayer.head < ArmorIDs.Head.Count && !ArmorIDs.Head.Sets.DrawFaceMaskUnderHeadLayer[drawinfo.drawPlayer.head])
		{
			flag = false;
		}
		if (flag)
		{
			Vector2 val = drawinfo.drawPlayer.GetFaceDrawOffset(drawinfo.drawPlayer.faceMask) * drawinfo.drawPlayer.Directions;
			QuickCDD(drawinfo.DrawData, drawinfo.cFaceMask, TextureAssets.AccFace[drawinfo.drawPlayer.faceMask].Value, val + new Vector2((float)(int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.bodyFrameMemory.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.bodyFrameMemory.Height + 4f) + drawinfo.drawPlayer.headPosition + drawinfo.headVect, drawinfo.bodyFrameMemory, drawinfo.colorArmorHead, drawinfo.drawPlayer.headRotation, drawinfo.headVect, drawinfo.scale, drawinfo.playerEffect, 0f);
		}
		if (!drawinfo.fullHair)
		{
			return;
		}
		Color color = drawinfo.colorArmorHead;
		int shaderTechnique = drawinfo.cHead;
		if (ArmorIDs.Head.Sets.UseSkinColor[drawinfo.drawPlayer.head])
		{
			color = ((!drawinfo.drawPlayer.isDisplayDollOrInanimate) ? drawinfo.colorHead : drawinfo.colorDisplayDollSkin);
			shaderTechnique = drawinfo.skinDyePacked;
		}
		QuickCDD(drawinfo.DrawData, shaderTechnique, TextureAssets.ArmorHead[drawinfo.drawPlayer.head].Value, drawinfo.helmetOffset + new Vector2(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.bodyFrameMemory.Width / 2) + (float)(drawinfo.drawPlayer.width / 2), drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.bodyFrameMemory.Height + 4f) + drawinfo.drawPlayer.headPosition + drawinfo.headVect, drawinfo.HairFrame, color, drawinfo.drawPlayer.headRotation, drawinfo.headVect, drawinfo.scale, drawinfo.playerEffect, 0f);
		if (!drawinfo.hideHair)
		{
			Rectangle hairFrame = drawinfo.HairFrame;
			hairFrame.Y -= 336;
			if (hairFrame.Y < 0)
			{
				hairFrame.Y = 0;
			}
			QuickCDD(drawinfo.DrawData, drawinfo.hairShaderPacked, TextureAssets.PlayerHair[drawinfo.drawPlayer.hair].Value, new Vector2(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.bodyFrameMemory.Width / 2) + (float)(drawinfo.drawPlayer.width / 2), drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.bodyFrameMemory.Height + 4f) + drawinfo.drawPlayer.headPosition + drawinfo.headVect + drawinfo.hairOffset, hairFrame, drawinfo.colorHair, drawinfo.drawPlayer.headRotation, drawinfo.headVect, drawinfo.scale, drawinfo.playerEffect, 0f);
		}
	}

	public static void DrawPlayer_03_HelmetHair(ref PlayerDrawHeadSet drawinfo)
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0110: Unknown result type (might be due to invalid IL or missing references)
		//IL_011c: Unknown result type (might be due to invalid IL or missing references)
		if (!drawinfo.hideHair && drawinfo.hatHair)
		{
			Rectangle hairFrame = drawinfo.HairFrame;
			hairFrame.Y -= 336;
			if (hairFrame.Y < 0)
			{
				hairFrame.Y = 0;
			}
			if (!drawinfo.drawPlayer.invis)
			{
				QuickCDD(drawinfo.DrawData, drawinfo.hairShaderPacked, TextureAssets.PlayerHairAlt[drawinfo.drawPlayer.hair].Value, new Vector2(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.bodyFrameMemory.Width / 2) + (float)(drawinfo.drawPlayer.width / 2), drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.bodyFrameMemory.Height + 4f) + drawinfo.drawPlayer.headPosition + drawinfo.headVect, hairFrame, drawinfo.colorHair, drawinfo.drawPlayer.headRotation, drawinfo.headVect, drawinfo.scale, drawinfo.playerEffect, 0f);
			}
		}
	}

	public static void DrawPlayer_04_CapricornMask(ref PlayerDrawHeadSet drawinfo)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		Rectangle hairFrame = drawinfo.HairFrame;
		hairFrame.Width += 2;
		QuickCDD(drawinfo.DrawData, drawinfo.cHead, TextureAssets.ArmorHead[drawinfo.drawPlayer.head].Value, drawinfo.helmetOffset + new Vector2(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.bodyFrameMemory.Width / 2) + (float)(drawinfo.drawPlayer.width / 2), drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.bodyFrameMemory.Height + 4f) + drawinfo.drawPlayer.headPosition + drawinfo.headVect, hairFrame, drawinfo.colorArmorHead, drawinfo.drawPlayer.headRotation, drawinfo.headVect, drawinfo.scale, drawinfo.playerEffect, 0f);
	}

	public static void DrawPlayer_04_RabbitOrder(ref PlayerDrawHeadSet drawinfo)
	{
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		//IL_0108: Unknown result type (might be due to invalid IL or missing references)
		//IL_010f: Unknown result type (might be due to invalid IL or missing references)
		//IL_011f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0127: Unknown result type (might be due to invalid IL or missing references)
		//IL_0150: Unknown result type (might be due to invalid IL or missing references)
		//IL_0155: Unknown result type (might be due to invalid IL or missing references)
		//IL_0167: Unknown result type (might be due to invalid IL or missing references)
		//IL_0205: Unknown result type (might be due to invalid IL or missing references)
		//IL_0210: Unknown result type (might be due to invalid IL or missing references)
		//IL_0215: Unknown result type (might be due to invalid IL or missing references)
		//IL_021b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0220: Unknown result type (might be due to invalid IL or missing references)
		//IL_0226: Unknown result type (might be due to invalid IL or missing references)
		//IL_022b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0230: Unknown result type (might be due to invalid IL or missing references)
		//IL_0238: Unknown result type (might be due to invalid IL or missing references)
		//IL_0249: Unknown result type (might be due to invalid IL or missing references)
		//IL_0255: Unknown result type (might be due to invalid IL or missing references)
		int verticalFrames = 27;
		Texture2D value = TextureAssets.ArmorHead[drawinfo.drawPlayer.head].Value;
		Rectangle val = value.Frame(1, verticalFrames, 0, drawinfo.drawPlayer.rabbitOrderFrame.DisplayFrame);
		Vector2 origin = val.Size() / 2f;
		int usedGravDir = 1;
		Vector2 val2 = DrawPlayer_04_GetHatDrawPosition(ref drawinfo, new Vector2(1f, -26f), usedGravDir);
		int hatStacks = GetHatStacks(ref drawinfo, 4955);
		float num = (float)Math.PI / 60f;
		float num2 = num * drawinfo.drawPlayer.position.X % ((float)Math.PI * 2f);
		for (int num3 = hatStacks - 1; num3 >= 0; num3--)
		{
			float num4 = Vector2.UnitY.RotatedBy(num2 + num * (float)num3).X * ((float)num3 / 30f) * 2f - (float)(num3 * 2 * drawinfo.drawPlayer.direction);
			QuickCDD(drawinfo.DrawData, drawinfo.cHead, value, val2 + new Vector2(num4, (float)(num3 * -14) * drawinfo.scale), val, drawinfo.colorArmorHead, drawinfo.drawPlayer.headRotation, origin, drawinfo.scale, drawinfo.playerEffect, 0f);
		}
		if (!drawinfo.hideHair)
		{
			Rectangle hairFrame = drawinfo.HairFrame;
			hairFrame.Y -= 336;
			if (hairFrame.Y < 0)
			{
				hairFrame.Y = 0;
			}
			QuickCDD(drawinfo.DrawData, drawinfo.hairShaderPacked, TextureAssets.PlayerHair[drawinfo.drawPlayer.hair].Value, new Vector2(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.bodyFrameMemory.Width / 2) + (float)(drawinfo.drawPlayer.width / 2), drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.bodyFrameMemory.Height + 4f) + drawinfo.drawPlayer.headPosition + drawinfo.headVect + drawinfo.hairOffset, hairFrame, drawinfo.colorHair, drawinfo.drawPlayer.headRotation, drawinfo.headVect, drawinfo.scale, drawinfo.playerEffect, 0f);
		}
	}

	public static void DrawPlayer_04_DeadCellsBeheadedHead(ref PlayerDrawHeadSet drawinfo)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0105: Unknown result type (might be due to invalid IL or missing references)
		//IL_0111: Unknown result type (might be due to invalid IL or missing references)
		Rectangle bodyFrame = drawinfo.drawPlayer.bodyFrame;
		int num = 9;
		int num2 = 4;
		int num3 = drawinfo.drawPlayer.miscCounter % (num * num2) / num2;
		bodyFrame.Y = bodyFrame.Height * num3;
		Vector2 position = drawinfo.helmetOffset + new Vector2(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.bodyFrameMemory.Width / 2) + (float)(drawinfo.drawPlayer.width / 2), drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.bodyFrameMemory.Height + 4f) + drawinfo.drawPlayer.headPosition + drawinfo.headVect;
		QuickCDD(drawinfo.DrawData, drawinfo.cHead, TextureAssets.ArmorHead[drawinfo.drawPlayer.head].Value, position, bodyFrame, drawinfo.colorArmorHead, drawinfo.drawPlayer.headRotation, drawinfo.headVect, drawinfo.scale, drawinfo.playerEffect, 0f);
	}

	public static void DrawPlayer_04_BadgersHat(ref PlayerDrawHeadSet drawinfo)
	{
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		//IL_011b: Unknown result type (might be due to invalid IL or missing references)
		//IL_012c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0132: Unknown result type (might be due to invalid IL or missing references)
		//IL_0134: Unknown result type (might be due to invalid IL or missing references)
		//IL_0174: Unknown result type (might be due to invalid IL or missing references)
		//IL_0184: Unknown result type (might be due to invalid IL or missing references)
		//IL_018f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0194: Unknown result type (might be due to invalid IL or missing references)
		//IL_0199: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b8: Unknown result type (might be due to invalid IL or missing references)
		int verticalFrames = 6;
		Texture2D value = TextureAssets.ArmorHead[drawinfo.drawPlayer.head].Value;
		Rectangle val = value.Frame(1, verticalFrames, 0, drawinfo.drawPlayer.rabbitOrderFrame.DisplayFrame);
		Vector2 origin = val.Size() / 2f;
		int num = 1;
		Vector2 val2 = DrawPlayer_04_GetHatDrawPosition(ref drawinfo, new Vector2(0f, -9f), num);
		int hatStacks = GetHatStacks(ref drawinfo, 5004);
		float num2 = (float)Math.PI / 60f;
		float num3 = num2 * drawinfo.drawPlayer.position.X % ((float)Math.PI * 2f);
		int num4 = hatStacks * 4 + 2;
		int num5 = 0;
		bool flag = (Main.GlobalTimeWrappedHourly + 180f) % 600f < 60f;
		for (int num6 = num4 - 1; num6 >= 0; num6--)
		{
			int num7 = 0;
			if (num6 == num4 - 1)
			{
				val.Y = 0;
				num7 = 2;
			}
			else if (num6 == 0)
			{
				val.Y = val.Height * 5;
			}
			else
			{
				val.Y = val.Height * (num5++ % 4 + 1);
			}
			if (!((val.Y == val.Height * 3) & flag))
			{
				float num8 = Vector2.UnitY.RotatedBy(num3 + num2 * (float)num6).X * ((float)num6 / 10f) * 4f - (float)num6 * 0.1f * (float)drawinfo.drawPlayer.direction;
				QuickCDD(drawinfo.DrawData, drawinfo.cHead, value, val2 + new Vector2(num8, (float)((num6 * -4 + num7) * num)) * drawinfo.scale, val, drawinfo.colorArmorHead, drawinfo.drawPlayer.headRotation, origin, drawinfo.scale, drawinfo.playerEffect, 0f);
			}
		}
	}

	private static Vector2 DrawPlayer_04_GetHatDrawPosition(ref PlayerDrawHeadSet drawinfo, Vector2 hatOffset, int usedGravDir)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		Vector2 val = new Vector2((float)drawinfo.drawPlayer.direction, (float)usedGravDir);
		return drawinfo.Position - Main.screenPosition + new Vector2((float)(-drawinfo.bodyFrameMemory.Width / 2 + drawinfo.drawPlayer.width / 2), (float)(drawinfo.drawPlayer.height - drawinfo.bodyFrameMemory.Height + 4)) + hatOffset * val * drawinfo.scale + (drawinfo.drawPlayer.headPosition + drawinfo.headVect);
	}

	private static int GetHatStacks(ref PlayerDrawHeadSet drawinfo, int hatItemId)
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

	public static void DrawPlayer_04_HatsWithFullHair(ref PlayerDrawHeadSet drawinfo)
	{
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_0152: Unknown result type (might be due to invalid IL or missing references)
		//IL_0157: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0104: Unknown result type (might be due to invalid IL or missing references)
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		//IL_010e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0115: Unknown result type (might be due to invalid IL or missing references)
		//IL_0126: Unknown result type (might be due to invalid IL or missing references)
		//IL_0132: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0223: Unknown result type (might be due to invalid IL or missing references)
		//IL_0228: Unknown result type (might be due to invalid IL or missing references)
		//IL_0233: Unknown result type (might be due to invalid IL or missing references)
		//IL_0238: Unknown result type (might be due to invalid IL or missing references)
		//IL_023e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0243: Unknown result type (might be due to invalid IL or missing references)
		//IL_0249: Unknown result type (might be due to invalid IL or missing references)
		//IL_0253: Unknown result type (might be due to invalid IL or missing references)
		//IL_0260: Unknown result type (might be due to invalid IL or missing references)
		//IL_026c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0189: Unknown result type (might be due to invalid IL or missing references)
		//IL_018e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0180: Unknown result type (might be due to invalid IL or missing references)
		//IL_0185: Unknown result type (might be due to invalid IL or missing references)
		if (drawinfo.drawPlayer.head == 259)
		{
			DrawPlayer_04_RabbitOrder(ref drawinfo);
		}
		else
		{
			if (!drawinfo.helmetIsOverFullHair)
			{
				return;
			}
			if (!drawinfo.hideHair)
			{
				Rectangle hairFrame = drawinfo.HairFrame;
				hairFrame.Y -= 336;
				if (hairFrame.Y < 0)
				{
					hairFrame.Y = 0;
				}
				QuickCDD(drawinfo.DrawData, drawinfo.hairShaderPacked, TextureAssets.PlayerHair[drawinfo.drawPlayer.hair].Value, new Vector2(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.bodyFrameMemory.Width / 2) + (float)(drawinfo.drawPlayer.width / 2), drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.bodyFrameMemory.Height + 4f) + drawinfo.drawPlayer.headPosition + drawinfo.headVect + drawinfo.hairOffset, hairFrame, drawinfo.colorHair, drawinfo.drawPlayer.headRotation, drawinfo.headVect, drawinfo.scale, drawinfo.playerEffect, 0f);
			}
			if (drawinfo.drawPlayer.head != 0)
			{
				Color color = drawinfo.colorArmorHead;
				int shaderTechnique = drawinfo.cHead;
				if (ArmorIDs.Head.Sets.UseSkinColor[drawinfo.drawPlayer.head])
				{
					color = ((!drawinfo.drawPlayer.isDisplayDollOrInanimate) ? drawinfo.colorHead : drawinfo.colorDisplayDollSkin);
					shaderTechnique = drawinfo.skinDyePacked;
				}
				QuickCDD(drawinfo.DrawData, shaderTechnique, TextureAssets.ArmorHead[drawinfo.drawPlayer.head].Value, drawinfo.helmetOffset + new Vector2(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.bodyFrameMemory.Width / 2) + (float)(drawinfo.drawPlayer.width / 2), drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.bodyFrameMemory.Height + 4f) + drawinfo.drawPlayer.headPosition + drawinfo.headVect, drawinfo.bodyFrameMemory, color, drawinfo.drawPlayer.headRotation, drawinfo.headVect, drawinfo.scale, drawinfo.playerEffect, 0f);
			}
		}
	}

	public static void DrawPlayer_05_TallHats(ref PlayerDrawHeadSet drawinfo)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_011b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0120: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_017d: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0204: Unknown result type (might be due to invalid IL or missing references)
		//IL_020a: Unknown result type (might be due to invalid IL or missing references)
		//IL_020f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0214: Unknown result type (might be due to invalid IL or missing references)
		//IL_021a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0227: Unknown result type (might be due to invalid IL or missing references)
		//IL_0233: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0152: Unknown result type (might be due to invalid IL or missing references)
		//IL_0157: Unknown result type (might be due to invalid IL or missing references)
		//IL_0149: Unknown result type (might be due to invalid IL or missing references)
		//IL_014e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0100: Unknown result type (might be due to invalid IL or missing references)
		if (drawinfo.helmetIsTall)
		{
			Rectangle hairFrame = drawinfo.HairFrame;
			if (drawinfo.drawPlayer.head == 158)
			{
				hairFrame.Height -= 2;
			}
			int num = 0;
			if (hairFrame.Y == hairFrame.Height * 6)
			{
				hairFrame.Height -= 2;
			}
			else if (hairFrame.Y == hairFrame.Height * 7)
			{
				num = -2;
			}
			else if (hairFrame.Y == hairFrame.Height * 8)
			{
				num = -2;
			}
			else if (hairFrame.Y == hairFrame.Height * 9)
			{
				num = -2;
			}
			else if (hairFrame.Y == hairFrame.Height * 10)
			{
				num = -2;
			}
			else if (hairFrame.Y == hairFrame.Height * 13)
			{
				hairFrame.Height -= 2;
			}
			else if (hairFrame.Y == hairFrame.Height * 14)
			{
				num = -2;
			}
			else if (hairFrame.Y == hairFrame.Height * 15)
			{
				num = -2;
			}
			else if (hairFrame.Y == hairFrame.Height * 16)
			{
				num = -2;
			}
			hairFrame.Y += num;
			Color color = drawinfo.colorArmorHead;
			int shaderTechnique = drawinfo.cHead;
			if (ArmorIDs.Head.Sets.UseSkinColor[drawinfo.drawPlayer.head])
			{
				color = ((!drawinfo.drawPlayer.isDisplayDollOrInanimate) ? drawinfo.colorHead : drawinfo.colorDisplayDollSkin);
				shaderTechnique = drawinfo.skinDyePacked;
			}
			QuickCDD(drawinfo.DrawData, shaderTechnique, TextureAssets.ArmorHead[drawinfo.drawPlayer.head].Value, drawinfo.helmetOffset + new Vector2(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.bodyFrameMemory.Width / 2) + (float)(drawinfo.drawPlayer.width / 2), drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.bodyFrameMemory.Height + 4f + (float)num) + drawinfo.drawPlayer.headPosition + drawinfo.headVect, hairFrame, color, drawinfo.drawPlayer.headRotation, drawinfo.headVect, drawinfo.scale, drawinfo.playerEffect, 0f);
		}
	}

	public static void DrawPlayer_06_NormalHats(ref PlayerDrawHeadSet drawinfo)
	{
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_012f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0134: Unknown result type (might be due to invalid IL or missing references)
		//IL_013f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0144: Unknown result type (might be due to invalid IL or missing references)
		//IL_014a: Unknown result type (might be due to invalid IL or missing references)
		//IL_014f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0154: Unknown result type (might be due to invalid IL or missing references)
		//IL_015a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0167: Unknown result type (might be due to invalid IL or missing references)
		//IL_0173: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_021d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0222: Unknown result type (might be due to invalid IL or missing references)
		//IL_022d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0232: Unknown result type (might be due to invalid IL or missing references)
		//IL_0238: Unknown result type (might be due to invalid IL or missing references)
		//IL_023d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0242: Unknown result type (might be due to invalid IL or missing references)
		//IL_0248: Unknown result type (might be due to invalid IL or missing references)
		//IL_0255: Unknown result type (might be due to invalid IL or missing references)
		//IL_0261: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_036b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0370: Unknown result type (might be due to invalid IL or missing references)
		//IL_037b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0380: Unknown result type (might be due to invalid IL or missing references)
		//IL_0386: Unknown result type (might be due to invalid IL or missing references)
		//IL_038b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0390: Unknown result type (might be due to invalid IL or missing references)
		//IL_0398: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b5: Unknown result type (might be due to invalid IL or missing references)
		if (drawinfo.drawPlayer.head == 270)
		{
			DrawPlayer_04_CapricornMask(ref drawinfo);
		}
		else if (drawinfo.drawPlayer.head == 265)
		{
			DrawPlayer_04_BadgersHat(ref drawinfo);
		}
		else if (drawinfo.drawPlayer.head == 282)
		{
			DrawPlayer_04_DeadCellsBeheadedHead(ref drawinfo);
		}
		else
		{
			if (!drawinfo.helmetIsNormal)
			{
				return;
			}
			Rectangle hairFrame = drawinfo.HairFrame;
			Color color = drawinfo.colorArmorHead;
			int shaderTechnique = drawinfo.cHead;
			if (ArmorIDs.Head.Sets.UseSkinColor[drawinfo.drawPlayer.head])
			{
				color = ((!drawinfo.drawPlayer.isDisplayDollOrInanimate) ? drawinfo.colorHead : drawinfo.colorDisplayDollSkin);
				shaderTechnique = drawinfo.skinDyePacked;
			}
			QuickCDD(drawinfo.DrawData, shaderTechnique, TextureAssets.ArmorHead[drawinfo.drawPlayer.head].Value, drawinfo.helmetOffset + new Vector2(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.bodyFrameMemory.Width / 2) + (float)(drawinfo.drawPlayer.width / 2), drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.bodyFrameMemory.Height + 4f) + drawinfo.drawPlayer.headPosition + drawinfo.headVect, hairFrame, color, drawinfo.drawPlayer.headRotation, drawinfo.headVect, drawinfo.scale, drawinfo.playerEffect, 0f);
			if (drawinfo.drawPlayer.head == 109)
			{
				Texture2D value = TextureAssets.Extra[276].Value;
				QuickCDD(drawinfo.DrawData, 0, value, drawinfo.helmetOffset + new Vector2(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.bodyFrameMemory.Width / 2) + (float)(drawinfo.drawPlayer.width / 2), drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.bodyFrameMemory.Height + 4f) + drawinfo.drawPlayer.headPosition + drawinfo.headVect, hairFrame, color, drawinfo.drawPlayer.headRotation, drawinfo.headVect, drawinfo.scale, drawinfo.playerEffect, 0f);
			}
			if (drawinfo.drawPlayer.head != 271)
			{
				return;
			}
			int num = PlayerDrawLayers.DrawPlayer_Head_GetTVScreen(drawinfo.drawPlayer);
			if (num == 0)
			{
				return;
			}
			Texture2D value2 = TextureAssets.GlowMask[309].Value;
			int frameY = drawinfo.drawPlayer.miscCounter % 20 / 5;
			if (num == 5)
			{
				frameY = 0;
				if (drawinfo.drawPlayer.eyeHelper.EyeFrameToShow > 0)
				{
					frameY = 2;
				}
			}
			Rectangle value3 = value2.Frame(6, 4, num, frameY, -2);
			QuickCDD(drawinfo.DrawData, drawinfo.cHead, value2, drawinfo.helmetOffset + new Vector2(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.bodyFrameMemory.Width / 2) + (float)(drawinfo.drawPlayer.width / 2), drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.bodyFrameMemory.Height + 4f) + drawinfo.drawPlayer.headPosition + drawinfo.headVect, value3, drawinfo.colorArmorHead, drawinfo.drawPlayer.headRotation, drawinfo.headVect, drawinfo.scale, drawinfo.playerEffect, 0f);
		}
	}

	public static void DrawPlayer_07_JustHair(ref PlayerDrawHeadSet drawinfo)
	{
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0101: Unknown result type (might be due to invalid IL or missing references)
		//IL_0106: Unknown result type (might be due to invalid IL or missing references)
		//IL_010b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0112: Unknown result type (might be due to invalid IL or missing references)
		//IL_0123: Unknown result type (might be due to invalid IL or missing references)
		//IL_012f: Unknown result type (might be due to invalid IL or missing references)
		if (!drawinfo.helmetIsNormal && !drawinfo.helmetIsOverFullHair && !drawinfo.helmetIsTall && !drawinfo.hideHair)
		{
			Rectangle hairFrame = drawinfo.HairFrame;
			hairFrame.Y -= 336;
			if (hairFrame.Y < 0)
			{
				hairFrame.Y = 0;
			}
			QuickCDD(drawinfo.DrawData, drawinfo.hairShaderPacked, TextureAssets.PlayerHair[drawinfo.drawPlayer.hair].Value, new Vector2(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.bodyFrameMemory.Width / 2) + (float)(drawinfo.drawPlayer.width / 2), drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.bodyFrameMemory.Height + 4f) + drawinfo.drawPlayer.headPosition + drawinfo.headVect + drawinfo.hairOffset, hairFrame, drawinfo.colorHair, drawinfo.drawPlayer.headRotation, drawinfo.headVect, drawinfo.scale, drawinfo.playerEffect, 0f);
		}
	}

	public static void DrawPlayer_08_FaceAcc(ref PlayerDrawHeadSet drawinfo)
	{
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_010a: Unknown result type (might be due to invalid IL or missing references)
		//IL_010f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0115: Unknown result type (might be due to invalid IL or missing references)
		//IL_011a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0120: Unknown result type (might be due to invalid IL or missing references)
		//IL_012a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0138: Unknown result type (might be due to invalid IL or missing references)
		//IL_0144: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0201: Unknown result type (might be due to invalid IL or missing references)
		//IL_026f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0274: Unknown result type (might be due to invalid IL or missing references)
		//IL_027f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0284: Unknown result type (might be due to invalid IL or missing references)
		//IL_028a: Unknown result type (might be due to invalid IL or missing references)
		//IL_028f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0295: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0352: Unknown result type (might be due to invalid IL or missing references)
		//IL_035d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0362: Unknown result type (might be due to invalid IL or missing references)
		//IL_0367: Unknown result type (might be due to invalid IL or missing references)
		//IL_038b: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_03fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0409: Unknown result type (might be due to invalid IL or missing references)
		//IL_040e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0414: Unknown result type (might be due to invalid IL or missing references)
		//IL_0419: Unknown result type (might be due to invalid IL or missing references)
		//IL_041f: Unknown result type (might be due to invalid IL or missing references)
		//IL_042a: Unknown result type (might be due to invalid IL or missing references)
		//IL_043b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0447: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_04de: Unknown result type (might be due to invalid IL or missing references)
		//IL_0502: Unknown result type (might be due to invalid IL or missing references)
		//IL_0570: Unknown result type (might be due to invalid IL or missing references)
		//IL_0575: Unknown result type (might be due to invalid IL or missing references)
		//IL_0580: Unknown result type (might be due to invalid IL or missing references)
		//IL_0585: Unknown result type (might be due to invalid IL or missing references)
		//IL_058b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0590: Unknown result type (might be due to invalid IL or missing references)
		//IL_0596: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_05be: Unknown result type (might be due to invalid IL or missing references)
		//IL_065e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0669: Unknown result type (might be due to invalid IL or missing references)
		//IL_066e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0674: Unknown result type (might be due to invalid IL or missing references)
		//IL_0679: Unknown result type (might be due to invalid IL or missing references)
		//IL_067f: Unknown result type (might be due to invalid IL or missing references)
		//IL_068a: Unknown result type (might be due to invalid IL or missing references)
		//IL_069b: Unknown result type (might be due to invalid IL or missing references)
		//IL_06a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0750: Unknown result type (might be due to invalid IL or missing references)
		//IL_075b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0760: Unknown result type (might be due to invalid IL or missing references)
		//IL_0766: Unknown result type (might be due to invalid IL or missing references)
		//IL_076b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0771: Unknown result type (might be due to invalid IL or missing references)
		//IL_078f: Unknown result type (might be due to invalid IL or missing references)
		//IL_07a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ac: Unknown result type (might be due to invalid IL or missing references)
		bool flag = drawinfo.drawPlayer.head < 0 || !ArmorIDs.Head.Sets.PreventBeardDraw[drawinfo.drawPlayer.head];
		if ((drawinfo.drawPlayer.beard > 0) & flag)
		{
			Vector2 beardDrawOffset = drawinfo.drawPlayer.GetBeardDrawOffset(minimap: true);
			Color color = drawinfo.colorArmorHead;
			if (ArmorIDs.Beard.Sets.UseHairColor[drawinfo.drawPlayer.beard])
			{
				color = drawinfo.colorHair;
			}
			QuickCDD(drawinfo.DrawData, drawinfo.cBeard, TextureAssets.AccBeard[drawinfo.drawPlayer.beard].Value, beardDrawOffset + new Vector2(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.bodyFrameMemory.Width / 2) + (float)(drawinfo.drawPlayer.width / 2), drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.bodyFrameMemory.Height + 4f) + drawinfo.drawPlayer.headPosition + drawinfo.headVect, drawinfo.bodyFrameMemory, color, drawinfo.drawPlayer.headRotation, drawinfo.headVect, drawinfo.scale, drawinfo.playerEffect, 0f);
		}
		bool flag2 = drawinfo.drawPlayer.head == 287;
		if (drawinfo.drawPlayer.face > 0 && drawinfo.drawPlayer.face < ArmorIDs.Face.Count && !ArmorIDs.Face.Sets.DrawInFaceUnderHairLayer[drawinfo.drawPlayer.face] && (drawinfo.drawPlayer.face != 23 || !flag2))
		{
			Vector2 val = drawinfo.drawPlayer.GetFaceDrawOffset(drawinfo.drawPlayer.face) * drawinfo.drawPlayer.Directions;
			QuickCDD(drawinfo.DrawData, drawinfo.cFace, TextureAssets.AccFace[drawinfo.drawPlayer.face].Value, val + new Vector2((float)(int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.bodyFrameMemory.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.bodyFrameMemory.Height + 4f) + drawinfo.drawPlayer.headPosition + drawinfo.headVect, drawinfo.bodyFrameMemory, drawinfo.colorArmorHead, drawinfo.drawPlayer.headRotation, drawinfo.headVect, drawinfo.scale, drawinfo.playerEffect, 0f);
		}
		bool flag3 = drawinfo.drawPlayer.faceMask > 0 && drawinfo.drawPlayer.faceMask < ArmorIDs.Face.Count;
		if (flag3 && drawinfo.drawPlayer.head > 0 && drawinfo.drawPlayer.head < ArmorIDs.Head.Count && (ArmorIDs.Head.Sets.PreventFaceMaskDraw[drawinfo.drawPlayer.head] || ArmorIDs.Head.Sets.DrawFaceMaskUnderHeadLayer[drawinfo.drawPlayer.head]))
		{
			flag3 = false;
		}
		if (flag3)
		{
			Vector2 val2 = drawinfo.drawPlayer.GetFaceDrawOffset(drawinfo.drawPlayer.faceMask) * drawinfo.drawPlayer.Directions;
			QuickCDD(drawinfo.DrawData, drawinfo.cFaceMask, TextureAssets.AccFace[drawinfo.drawPlayer.faceMask].Value, val2 + new Vector2((float)(int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.bodyFrameMemory.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.bodyFrameMemory.Height + 4f) + drawinfo.drawPlayer.headPosition + drawinfo.headVect, drawinfo.bodyFrameMemory, drawinfo.colorArmorHead, drawinfo.drawPlayer.headRotation, drawinfo.headVect, drawinfo.scale, drawinfo.playerEffect, 0f);
		}
		bool flag4 = drawinfo.drawPlayer.faceFlower > 0 && drawinfo.drawPlayer.faceFlower < ArmorIDs.Face.Count;
		if (flag4 && drawinfo.drawPlayer.head > 0 && drawinfo.drawPlayer.head < ArmorIDs.Head.Count && ArmorIDs.Head.Sets.PreventFaceFlowerDraw[drawinfo.drawPlayer.head])
		{
			flag4 = false;
		}
		if (flag4)
		{
			Vector2 val3 = drawinfo.drawPlayer.GetFaceDrawOffset(drawinfo.drawPlayer.faceFlower) * drawinfo.drawPlayer.Directions;
			QuickCDD(drawinfo.DrawData, drawinfo.cFaceFlower, TextureAssets.AccFace[drawinfo.drawPlayer.faceFlower].Value, val3 + new Vector2((float)(int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.bodyFrameMemory.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.bodyFrameMemory.Height + 4f) + drawinfo.drawPlayer.headPosition + drawinfo.headVect, drawinfo.bodyFrameMemory, drawinfo.colorArmorHead, drawinfo.drawPlayer.headRotation, drawinfo.headVect, drawinfo.scale, drawinfo.playerEffect, 0f);
		}
		if (drawinfo.drawUnicornHorn)
		{
			QuickCDD(drawinfo.DrawData, drawinfo.cUnicornHorn, TextureAssets.Extra[143].Value, new Vector2(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.bodyFrameMemory.Width / 2) + (float)(drawinfo.drawPlayer.width / 2), drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.bodyFrameMemory.Height + 4f) + drawinfo.drawPlayer.headPosition + drawinfo.headVect, drawinfo.bodyFrameMemory, drawinfo.colorArmorHead, drawinfo.drawPlayer.headRotation, drawinfo.headVect, drawinfo.scale, drawinfo.playerEffect, 0f);
		}
		if (drawinfo.drawAngelHalo)
		{
			Main.instance.LoadAccFace(7);
			QuickCDD(drawinfo.DrawData, drawinfo.cAngelHalo, TextureAssets.AccFace[7].Value, new Vector2((float)(int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.bodyFrameMemory.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.bodyFrameMemory.Height + 4f) + drawinfo.drawPlayer.headPosition + drawinfo.headVect, drawinfo.bodyFrameMemory, new Color(200, 200, 200, 200), drawinfo.drawPlayer.headRotation, drawinfo.headVect, drawinfo.scale, drawinfo.playerEffect, 0f);
		}
	}

	public static void DrawPlayer_RenderAllLayers(ref PlayerDrawHeadSet drawinfo)
	{
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		List<DrawData> drawData = drawinfo.DrawData;
		Effect pixelShader = Main.pixelShader;
		_ = Main.projectile;
		SpriteBatch spriteBatch = Main.spriteBatch;
		for (int i = 0; i < drawData.Count; i++)
		{
			DrawData cdd = drawData[i];
			if (!cdd.sourceRect.HasValue)
			{
				cdd.sourceRect = cdd.texture.Frame();
			}
			PlayerDrawHelper.SetShaderForData(drawinfo.drawPlayer, drawinfo.cHead, ref cdd);
			if (cdd.texture != null)
			{
				cdd.Draw(spriteBatch);
			}
		}
		pixelShader.CurrentTechnique.Passes[0].Apply();
	}

	public static void DrawPlayer_DrawSelectionRect(ref PlayerDrawHeadSet drawinfo)
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		SpriteRenderTargetHelper.GetDrawBoundary(drawinfo.DrawData, out var lowest, out var highest);
		Utils.DrawRect(Main.spriteBatch, lowest + Main.screenPosition, highest + Main.screenPosition, Color.White);
	}

	public static void QuickCDD(List<DrawData> drawData, Texture2D texture, Vector2 position, Rectangle? sourceRectangle, Color color, float rotation, Vector2 origin, float scale, SpriteEffects effects, float layerDepth)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0004: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		drawData.Add(new DrawData(texture, position, sourceRectangle, color, rotation, origin, scale, effects));
	}

	public static void QuickCDD(List<DrawData> drawData, int shaderTechnique, Texture2D texture, Vector2 position, Rectangle? sourceRectangle, Color color, float rotation, Vector2 origin, float scale, SpriteEffects effects, float layerDepth)
	{
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		DrawData item = new DrawData(texture, position, sourceRectangle, color, rotation, origin, scale, effects);
		item.shader = shaderTechnique;
		drawData.Add(item);
	}
}
