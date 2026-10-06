using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.Testing;

namespace Terraria.Graphics.Renderers;

public class LegacyPlayerRenderer : IPlayerRenderer
{
	private readonly List<DrawData> _drawData = new List<DrawData>();

	private readonly List<int> _dust = new List<int>();

	private readonly List<int> _gore = new List<int>();

	public Projectile OverrideHeldProjectile;

	public static SamplerState MountedSamplerState
	{
		get
		{
			if (!Main.drawToScreen)
			{
				return SamplerState.AnisotropicClamp;
			}
			return SamplerState.LinearClamp;
		}
	}

	public void DrawPlayers(Camera camera, IEnumerable<Player> players)
	{
		foreach (Player player in players)
		{
			DrawPlayerFull(camera, player);
		}
	}

	public void DrawPlayerHead(Camera camera, Player drawPlayer, Vector2 position, float alpha = 1f, float scale = 1f, Color borderColor = default(Color))
	{
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		if (!drawPlayer.ShouldNotDraw)
		{
			_drawData.Clear();
			_dust.Clear();
			_gore.Clear();
			PlayerDrawHeadSet drawinfo = default;
			drawinfo.BoringSetup(drawPlayer, _drawData, _dust, _gore, position.X, position.Y, alpha, scale);
			PlayerDrawHeadLayers.DrawPlayer_00_BackHelmet(ref drawinfo);
			PlayerDrawHeadLayers.DrawPlayer_01_FaceSkin(ref drawinfo);
			PlayerDrawHeadLayers.DrawPlayer_02_DrawArmorWithFullHair(ref drawinfo);
			PlayerDrawHeadLayers.DrawPlayer_03_HelmetHair(ref drawinfo);
			PlayerDrawHeadLayers.DrawPlayer_04_HatsWithFullHair(ref drawinfo);
			PlayerDrawHeadLayers.DrawPlayer_05_TallHats(ref drawinfo);
			PlayerDrawHeadLayers.DrawPlayer_06_NormalHats(ref drawinfo);
			PlayerDrawHeadLayers.DrawPlayer_07_JustHair(ref drawinfo);
			PlayerDrawHeadLayers.DrawPlayer_08_FaceAcc(ref drawinfo);
			CreateOutlines(alpha, scale, borderColor);
			PlayerDrawHeadLayers.DrawPlayer_RenderAllLayers(ref drawinfo);
		}
	}

	private void CreateOutlines(float alpha, float scale, Color borderColor)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0143: Unknown result type (might be due to invalid IL or missing references)
		//IL_0144: Unknown result type (might be due to invalid IL or missing references)
		//IL_0163: Unknown result type (might be due to invalid IL or missing references)
		//IL_0168: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_019d: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01be: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c5: Unknown result type (might be due to invalid IL or missing references)
		if (!(borderColor != Color.Transparent))
		{
			return;
		}
		List<DrawData> collection = new List<DrawData>(_drawData);
		List<DrawData> list = new List<DrawData>(_drawData);
		float num = 2f * scale;
		Color val = borderColor;
		val *= alpha * alpha;
		Color black = Color.Black;
		black *= alpha * alpha;
		int colorOnlyShaderIndex = ContentSamples.DyeShaderIDs.ColorOnlyShaderIndex;
		for (int i = 0; i < list.Count; i++)
		{
			DrawData value = list[i];
			value.shader = colorOnlyShaderIndex;
			value.color = black;
			list[i] = value;
		}
		int num2 = 2;
		Vector2 val2;
		for (int j = -num2; j <= num2; j++)
		{
			for (int k = -num2; k <= num2; k++)
			{
				if (Math.Abs(j) + Math.Abs(k) == num2)
				{
					val2 = new Vector2((float)j * num, (float)k * num);
					for (int l = 0; l < list.Count; l++)
					{
						DrawData item = list[l];
						ref Vector2 position = ref item.position;
						position += val2;
						_drawData.Add(item);
					}
				}
			}
		}
		for (int m = 0; m < list.Count; m++)
		{
			DrawData value2 = list[m];
			value2.shader = colorOnlyShaderIndex;
			value2.color = val;
			list[m] = value2;
		}
		val2 = Vector2.Zero;
		num2 = 1;
		for (int n = -num2; n <= num2; n++)
		{
			for (int num3 = -num2; num3 <= num2; num3++)
			{
				if (Math.Abs(n) + Math.Abs(num3) == num2)
				{
					val2 = new Vector2((float)n * num, (float)num3 * num);
					for (int num4 = 0; num4 < list.Count; num4++)
					{
						DrawData item2 = list[num4];
						ref Vector2 position2 = ref item2.position;
						position2 += val2;
						_drawData.Add(item2);
					}
				}
			}
		}
		_drawData.AddRange(collection);
	}

	public void DrawPlayer(Camera camera, Player drawPlayer, Vector2 position, float rotation, Vector2 rotationOrigin, float shadow = 0f, float scale = 1f)
	{
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		if (drawPlayer.ShouldNotDraw)
		{
			return;
		}
		PlayerDrawSet drawInfo = default;
		_drawData.Clear();
		_dust.Clear();
		_gore.Clear();
		drawInfo.BoringSetup(drawPlayer, _drawData, _dust, _gore, position, shadow, rotation, rotationOrigin, OverrideHeldProjectile);
		DrawPlayer_UseNormalLayers(ref drawInfo);
		PlayerDrawLayers.DrawPlayer_TransformDrawData(ref drawInfo);
		if (scale != 1f)
		{
			PlayerDrawLayers.DrawPlayer_ScaleDrawData(ref drawInfo, scale);
		}
		PlayerDrawLayers.DrawPlayer_RenderAllLayers(ref drawInfo);
		if (!drawInfo.drawPlayer.mount.Active || !drawInfo.drawPlayer.UsingSuperCart || OverrideHeldProjectile != null)
		{
			return;
		}
		for (int i = 0; i < 1000; i++)
		{
			if (Main.projectile[i].active && Main.projectile[i].owner == drawInfo.drawPlayer.whoAmI && Main.projectile[i].type == 591)
			{
				Main.instance.DrawProj(i);
			}
		}
	}

	private static void DrawPlayer_UseNormalLayers(ref PlayerDrawSet drawInfo)
	{
		PlayerDrawLayers.DrawPlayer_extra_TorsoPlus(ref drawInfo);
		PlayerDrawLayers.DrawPlayer_01_2_JimsCloak(ref drawInfo);
		PlayerDrawLayers.DrawPlayer_extra_TorsoMinus(ref drawInfo);
		PlayerDrawLayers.DrawPlayer_02_MountBehindPlayer(ref drawInfo);
		PlayerDrawLayers.DrawPlayer_03_Carpet(ref drawInfo);
		PlayerDrawLayers.DrawPlayer_03_PortableStool(ref drawInfo);
		PlayerDrawLayers.DrawPlayer_extra_TorsoPlus(ref drawInfo);
		PlayerDrawLayers.DrawPlayer_04_ElectrifiedDebuffBack(ref drawInfo);
		PlayerDrawLayers.DrawPlayer_05_ForbiddenSetRing(ref drawInfo);
		PlayerDrawLayers.DrawPlayer_05_2_SafemanSun(ref drawInfo);
		PlayerDrawLayers.DrawPlayer_06_WebbedDebuffBack(ref drawInfo);
		PlayerDrawLayers.DrawPlayer_07_LeinforsHairShampoo(ref drawInfo);
		PlayerDrawLayers.DrawPlayer_extra_TorsoMinus(ref drawInfo);
		PlayerDrawLayers.DrawPlayer_08_Backpacks(ref drawInfo);
		PlayerDrawLayers.DrawPlayer_extra_TorsoPlus(ref drawInfo);
		PlayerDrawLayers.DrawPlayer_08_1_Tails(ref drawInfo);
		PlayerDrawLayers.DrawPlayer_extra_TorsoMinus(ref drawInfo);
		PlayerDrawLayers.DrawPlayer_09_Wings(ref drawInfo);
		PlayerDrawLayers.DrawPlayer_extra_TorsoPlus(ref drawInfo);
		PlayerDrawLayers.DrawPlayer_01_BackHair(ref drawInfo);
		PlayerDrawLayers.DrawPlayer_10_BackAcc(ref drawInfo);
		PlayerDrawLayers.DrawPlayer_01_3_BackHead(ref drawInfo);
		PlayerDrawLayers.DrawPlayer_extra_TorsoMinus(ref drawInfo);
		PlayerDrawLayers.DrawPlayer_11_Balloons(ref drawInfo);
		if (drawInfo.weaponDrawOrder == WeaponDrawOrder.BehindBackArm)
		{
			PlayerDrawLayers.DrawPlayer_27_HeldItem(ref drawInfo);
		}
		PlayerDrawLayers.DrawPlayer_13_ArmorBackCoat(ref drawInfo);
		PlayerDrawLayers.DrawPlayer_12_Skin(ref drawInfo);
		if (drawInfo.drawPlayer.wearsRobe && drawInfo.drawPlayer.body != 166)
		{
			PlayerDrawLayers.DrawPlayer_14_Shoes(ref drawInfo);
			PlayerDrawLayers.DrawPlayer_13_Leggings(ref drawInfo);
		}
		else
		{
			PlayerDrawLayers.DrawPlayer_13_Leggings(ref drawInfo);
			PlayerDrawLayers.DrawPlayer_14_Shoes(ref drawInfo);
		}
		PlayerDrawLayers.DrawPlayer_extra_TorsoPlus(ref drawInfo);
		PlayerDrawLayers.DrawPlayer_15_SkinLongCoat(ref drawInfo);
		PlayerDrawLayers.DrawPlayer_16_ArmorLongCoat(ref drawInfo);
		PlayerDrawLayers.DrawPlayer_17_Torso(ref drawInfo);
		PlayerDrawLayers.DrawPlayer_18_OffhandAcc(ref drawInfo);
		PlayerDrawLayers.DrawPlayer_19_WaistAcc(ref drawInfo);
		PlayerDrawLayers.DrawPlayer_20_NeckAcc(ref drawInfo);
		if (!drawInfo.mountHandlesHeadDraw)
		{
			PlayerDrawLayers.DrawPlayer_21_Head(ref drawInfo);
		}
		PlayerDrawLayers.DrawPlayer_21_1_Magiluminescence(ref drawInfo);
		if (!drawInfo.mountHandlesHeadDraw)
		{
			PlayerDrawLayers.DrawPlayer_22_FaceAcc(ref drawInfo);
			if (drawInfo.drawFrontAccInNeckAccLayer)
			{
				PlayerDrawLayers.DrawPlayer_extra_TorsoMinus(ref drawInfo);
				PlayerDrawLayers.DrawPlayer_32_FrontAcc_FrontPart(ref drawInfo);
				PlayerDrawLayers.DrawPlayer_extra_TorsoPlus(ref drawInfo);
			}
		}
		PlayerDrawLayers.DrawPlayer_23_MountFront(ref drawInfo);
		PlayerDrawLayers.DrawPlayer_24_Pulley(ref drawInfo);
		PlayerDrawLayers.DrawPlayer_JimsDroneRadio(ref drawInfo);
		PlayerDrawLayers.DrawPlayer_32_FrontAcc_BackPart(ref drawInfo);
		PlayerDrawLayers.DrawPlayer_25_Shield(ref drawInfo);
		PlayerDrawLayers.DrawPlayer_extra_MountPlus(ref drawInfo);
		PlayerDrawLayers.DrawPlayer_26_SolarShield(ref drawInfo);
		PlayerDrawLayers.DrawPlayer_extra_MountMinus(ref drawInfo);
		if (drawInfo.weaponDrawOrder == WeaponDrawOrder.BehindFrontArm)
		{
			PlayerDrawLayers.DrawPlayer_27_HeldItem(ref drawInfo);
		}
		PlayerDrawLayers.DrawPlayer_28_ArmOverItem(ref drawInfo);
		PlayerDrawLayers.DrawPlayer_29_OnhandAcc(ref drawInfo);
		PlayerDrawLayers.DrawPlayer_30_BladedGlove(ref drawInfo);
		if (!drawInfo.drawFrontAccInNeckAccLayer)
		{
			PlayerDrawLayers.DrawPlayer_32_FrontAcc_FrontPart(ref drawInfo);
		}
		PlayerDrawLayers.DrawPlayer_extra_TorsoMinus(ref drawInfo);
		if (drawInfo.weaponDrawOrder == WeaponDrawOrder.OverFrontArm)
		{
			PlayerDrawLayers.DrawPlayer_27_HeldItem(ref drawInfo);
		}
		PlayerDrawLayers.DrawPlayer_31_ProjectileOverArm(ref drawInfo);
		PlayerDrawLayers.DrawPlayer_33_FrozenOrWebbedDebuff(ref drawInfo);
		PlayerDrawLayers.DrawPlayer_34_ElectrifiedDebuffFront(ref drawInfo);
		PlayerDrawLayers.DrawPlayer_35_IceBarrier(ref drawInfo);
		PlayerDrawLayers.DrawPlayer_36_CTG(ref drawInfo);
		PlayerDrawLayers.DrawPlayer_37_BeetleBuff(ref drawInfo);
		PlayerDrawLayers.DrawPlayer_38_EyebrellaCloud(ref drawInfo);
		PlayerDrawLayers.DrawPlayer_MakeIntoFirstFractalAfterImage(ref drawInfo);
	}

	private void DrawPlayerFull(Camera camera, Player drawPlayer)
	{
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_017f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0185: Unknown result type (might be due to invalid IL or missing references)
		//IL_018a: Unknown result type (might be due to invalid IL or missing references)
		//IL_018f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_08e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_08e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0336: Unknown result type (might be due to invalid IL or missing references)
		//IL_0113: Unknown result type (might be due to invalid IL or missing references)
		//IL_0119: Unknown result type (might be due to invalid IL or missing references)
		//IL_011e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0134: Unknown result type (might be due to invalid IL or missing references)
		//IL_08ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0904: Unknown result type (might be due to invalid IL or missing references)
		//IL_091a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0920: Unknown result type (might be due to invalid IL or missing references)
		//IL_0925: Unknown result type (might be due to invalid IL or missing references)
		//IL_092b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0930: Unknown result type (might be due to invalid IL or missing references)
		//IL_0935: Unknown result type (might be due to invalid IL or missing references)
		//IL_048d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0493: Unknown result type (might be due to invalid IL or missing references)
		//IL_0498: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_04fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0513: Unknown result type (might be due to invalid IL or missing references)
		//IL_055a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0560: Unknown result type (might be due to invalid IL or missing references)
		//IL_0570: Unknown result type (might be due to invalid IL or missing references)
		//IL_0575: Unknown result type (might be due to invalid IL or missing references)
		//IL_057f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0585: Unknown result type (might be due to invalid IL or missing references)
		//IL_058a: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_062a: Unknown result type (might be due to invalid IL or missing references)
		//IL_062f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0634: Unknown result type (might be due to invalid IL or missing references)
		//IL_0639: Unknown result type (might be due to invalid IL or missing references)
		//IL_0642: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02da: Unknown result type (might be due to invalid IL or missing references)
		//IL_02df: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0307: Unknown result type (might be due to invalid IL or missing references)
		//IL_06d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_06d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_06dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_06e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_06f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_06f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_06fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0702: Unknown result type (might be due to invalid IL or missing references)
		//IL_070b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0736: Unknown result type (might be due to invalid IL or missing references)
		//IL_073c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0741: Unknown result type (might be due to invalid IL or missing references)
		//IL_0753: Unknown result type (might be due to invalid IL or missing references)
		//IL_0758: Unknown result type (might be due to invalid IL or missing references)
		//IL_075d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0762: Unknown result type (might be due to invalid IL or missing references)
		//IL_076b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0410: Unknown result type (might be due to invalid IL or missing references)
		//IL_0416: Unknown result type (might be due to invalid IL or missing references)
		//IL_041b: Unknown result type (might be due to invalid IL or missing references)
		//IL_042b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0430: Unknown result type (might be due to invalid IL or missing references)
		//IL_0435: Unknown result type (might be due to invalid IL or missing references)
		//IL_043a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0443: Unknown result type (might be due to invalid IL or missing references)
		//IL_0877: Unknown result type (might be due to invalid IL or missing references)
		//IL_087d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0882: Unknown result type (might be due to invalid IL or missing references)
		//IL_0892: Unknown result type (might be due to invalid IL or missing references)
		//IL_0897: Unknown result type (might be due to invalid IL or missing references)
		//IL_089c: Unknown result type (might be due to invalid IL or missing references)
		//IL_07a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_07a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_07bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_07c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_07c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_08a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_08bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_08c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0810: Unknown result type (might be due to invalid IL or missing references)
		//IL_0819: Unknown result type (might be due to invalid IL or missing references)
		//IL_0832: Unknown result type (might be due to invalid IL or missing references)
		//IL_0838: Unknown result type (might be due to invalid IL or missing references)
		//IL_083a: Unknown result type (might be due to invalid IL or missing references)
		//IL_083f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0844: Unknown result type (might be due to invalid IL or missing references)
		//IL_0849: Unknown result type (might be due to invalid IL or missing references)
		//IL_0852: Unknown result type (might be due to invalid IL or missing references)
		SpriteBatch spriteBatch = camera.SpriteBatch;
		SamplerState val = camera.Sampler;
		if (drawPlayer.mount.Active && drawPlayer.fullRotation != 0f)
		{
			val = MountedSamplerState;
		}
		spriteBatch.Begin((SpriteSortMode)1, BlendState.AlphaBlend, val, DepthStencilState.None, camera.Rasterizer, (Effect)null, camera.GameViewMatrix.TransformationMatrix);
		if (Main.gamePaused)
		{
			drawPlayer.PlayerFrame();
		}
		if (drawPlayer.ghost)
		{
			for (int i = 0; i < 3; i++)
			{
				Vector2 val2 = drawPlayer.shadowPos[i];
				val2 = drawPlayer.position + drawPlayer.netOffset - drawPlayer.velocity * (float)(2 + i * 2);
				DrawGhost(camera, drawPlayer, val2, 0.5f + 0.2f * (float)i);
			}
			DrawGhost(camera, drawPlayer, drawPlayer.position + drawPlayer.netOffset);
		}
		else
		{
			PrepareDrawForFrame(drawPlayer);
			if (drawPlayer.armorEffectDrawShadowEOCShield)
			{
				int num = drawPlayer.eocDash / 4;
				if (num > 3)
				{
					num = 3;
				}
				for (int j = 0; j < num; j++)
				{
					DrawPlayer(camera, drawPlayer, drawPlayer.shadowPos[j] + drawPlayer.netOffset, drawPlayer.shadowRotation[j], drawPlayer.shadowOrigin[j], 0.5f + 0.2f * (float)j);
				}
			}
			Vector2 position;
			if (drawPlayer.invis)
			{
				drawPlayer.armorEffectDrawOutlines = false;
				drawPlayer.armorEffectDrawShadow = false;
				drawPlayer.armorEffectDrawShadowSubtle = false;
				position = drawPlayer.position + drawPlayer.netOffset;
				if (drawPlayer.aggro <= -750)
				{
					DrawPlayer(camera, drawPlayer, position, drawPlayer.fullRotation, drawPlayer.fullRotationOrigin, 1f);
				}
				else
				{
					drawPlayer.invis = false;
					DrawPlayer(camera, drawPlayer, position, drawPlayer.fullRotation, drawPlayer.fullRotationOrigin);
					drawPlayer.invis = true;
				}
			}
			if (drawPlayer.armorEffectDrawOutlines)
			{
				_ = drawPlayer.position;
				if (!Main.gamePaused)
				{
					drawPlayer.ghostFade += drawPlayer.ghostDir * 0.075f;
				}
				if ((double)drawPlayer.ghostFade < 0.1)
				{
					drawPlayer.ghostDir = 1f;
					drawPlayer.ghostFade = 0.1f;
				}
				else if ((double)drawPlayer.ghostFade > 0.9)
				{
					drawPlayer.ghostDir = -1f;
					drawPlayer.ghostFade = 0.9f;
				}
				float num2 = drawPlayer.ghostFade * 5f;
				for (int k = 0; k < 4; k++)
				{
					float num3;
					float num4;
					switch (k)
					{
					default:
						num3 = num2;
						num4 = 0f;
						break;
					case 1:
						num3 = 0f - num2;
						num4 = 0f;
						break;
					case 2:
						num3 = 0f;
						num4 = num2;
						break;
					case 3:
						num3 = 0f;
						num4 = 0f - num2;
						break;
					}
					position = drawPlayer.position + drawPlayer.netOffset + new Vector2(num3, drawPlayer.gfxOffY + num4);
					DrawPlayer(camera, drawPlayer, position, drawPlayer.fullRotation, drawPlayer.fullRotationOrigin, drawPlayer.ghostFade);
				}
			}
			if (drawPlayer.armorEffectDrawOutlinesForbidden)
			{
				_ = drawPlayer.position;
				if (!Main.gamePaused)
				{
					drawPlayer.ghostFade += drawPlayer.ghostDir * 0.025f;
				}
				if ((double)drawPlayer.ghostFade < 0.1)
				{
					drawPlayer.ghostDir = 1f;
					drawPlayer.ghostFade = 0.1f;
				}
				else if ((double)drawPlayer.ghostFade > 0.9)
				{
					drawPlayer.ghostDir = -1f;
					drawPlayer.ghostFade = 0.9f;
				}
				float num5 = drawPlayer.ghostFade * 5f;
				for (int l = 0; l < 4; l++)
				{
					float num6;
					float num7;
					switch (l)
					{
					default:
						num6 = num5;
						num7 = 0f;
						break;
					case 1:
						num6 = 0f - num5;
						num7 = 0f;
						break;
					case 2:
						num6 = 0f;
						num7 = num5;
						break;
					case 3:
						num6 = 0f;
						num7 = 0f - num5;
						break;
					}
					position = drawPlayer.position + drawPlayer.netOffset + new Vector2(num6, drawPlayer.gfxOffY + num7);
					DrawPlayer(camera, drawPlayer, position, drawPlayer.fullRotation, drawPlayer.fullRotationOrigin, drawPlayer.ghostFade);
				}
			}
			if (drawPlayer.armorEffectDrawShadowBasilisk)
			{
				int num8 = (int)(drawPlayer.basiliskCharge * 3f);
				for (int m = 0; m < num8; m++)
				{
					DrawPlayer(camera, drawPlayer, drawPlayer.shadowPos[m] + drawPlayer.netOffset, drawPlayer.shadowRotation[m], drawPlayer.shadowOrigin[m], 0.5f + 0.2f * (float)m);
				}
			}
			else if (drawPlayer.armorEffectDrawShadow)
			{
				for (int n = 0; n < 3; n++)
				{
					DrawPlayer(camera, drawPlayer, drawPlayer.shadowPos[n] + drawPlayer.netOffset, drawPlayer.shadowRotation[n], drawPlayer.shadowOrigin[n], 0.5f + 0.2f * (float)n);
				}
			}
			if (drawPlayer.armorEffectDrawShadowLokis)
			{
				for (int num9 = 0; num9 < 3; num9++)
				{
					DrawPlayer(camera, drawPlayer, Vector2.Lerp(drawPlayer.shadowPos[num9], drawPlayer.position + new Vector2(0f, drawPlayer.gfxOffY), 0.5f) + drawPlayer.netOffset, drawPlayer.shadowRotation[num9], drawPlayer.shadowOrigin[num9], MathHelper.Lerp(1f, 0.5f + 0.2f * (float)num9, 0.5f));
				}
			}
			if (drawPlayer.armorEffectDrawShadowSubtle)
			{
				for (int num10 = 0; num10 < 4; num10++)
				{
					position = drawPlayer.position + drawPlayer.netOffset + new Vector2((float)Main.rand.Next(-20, 21) * 0.1f, (float)Main.rand.Next(-20, 21) * 0.1f + drawPlayer.gfxOffY);
					DrawPlayer(camera, drawPlayer, position, drawPlayer.fullRotation, drawPlayer.fullRotationOrigin, 0.9f);
				}
			}
			if (drawPlayer.shadowDodge)
			{
				drawPlayer.shadowDodgeCount++;
				if (drawPlayer.shadowDodgeCount > 30f)
				{
					drawPlayer.shadowDodgeCount = 30f;
				}
			}
			else
			{
				drawPlayer.shadowDodgeCount--;
				if (drawPlayer.shadowDodgeCount < 0f)
				{
					drawPlayer.shadowDodgeCount = 0f;
				}
			}
			if (drawPlayer.shadowDodgeCount > 0f)
			{
				_ = drawPlayer.position;
				position = drawPlayer.position + drawPlayer.netOffset + new Vector2(drawPlayer.shadowDodgeCount, drawPlayer.gfxOffY);
				DrawPlayer(camera, drawPlayer, position, drawPlayer.fullRotation, drawPlayer.fullRotationOrigin, 0.5f + (float)Main.rand.Next(-10, 11) * 0.005f);
				position = drawPlayer.position + drawPlayer.netOffset + new Vector2(0f - drawPlayer.shadowDodgeCount, drawPlayer.gfxOffY);
				DrawPlayer(camera, drawPlayer, position, drawPlayer.fullRotation, drawPlayer.fullRotationOrigin, 0.5f + (float)Main.rand.Next(-10, 11) * 0.005f);
			}
			if (drawPlayer.brainOfConfusionDodgeAnimationCounter > 0)
			{
				Vector2 val3 = drawPlayer.position + drawPlayer.netOffset + new Vector2(0f, drawPlayer.gfxOffY);
				float lerpValue = Utils.GetLerpValue(300f, 270f, drawPlayer.brainOfConfusionDodgeAnimationCounter);
				float num11 = MathHelper.Lerp(2f, 120f, lerpValue);
				if (lerpValue >= 0f && lerpValue <= 1f)
				{
					for (float num12 = 0f; num12 < (float)Math.PI * 2f; num12 += (float)Math.PI / 3f)
					{
						position = val3 + new Vector2(0f, num11).RotatedBy((float)Math.PI * 2f * lerpValue * 0.5f + num12);
						DrawPlayer(camera, drawPlayer, position, drawPlayer.fullRotation, drawPlayer.fullRotationOrigin, lerpValue);
					}
				}
			}
			position = drawPlayer.position + drawPlayer.netOffset + new Vector2(0f, drawPlayer.gfxOffY);
			if (drawPlayer.stoned)
			{
				DrawPlayerStoned(camera, drawPlayer, position);
			}
			else if (!drawPlayer.invis)
			{
				DrawPlayer(camera, drawPlayer, position, drawPlayer.fullRotation, drawPlayer.fullRotationOrigin);
			}
		}
		spriteBatch.End();
		if (drawPlayer.netOffset != Vector2.Zero && DebugOptions.ShowNetOffset)
		{
			DebugVisualizer.World.AddRectangle(drawPlayer.Hitbox, Color.YellowGreen, 1, 2f);
			DebugVisualizer.World.AddLine(drawPlayer.Center + drawPlayer.netOffset, drawPlayer.Center, Color.Red, Color.YellowGreen, 1, 2f);
		}
	}

	public void PrepareDrawForFrame(Player drawPlayer)
	{
		if (!drawPlayer.inventory[drawPlayer.selectedItem].flame && drawPlayer.head != 137 && drawPlayer.wings != 22)
		{
			return;
		}
		drawPlayer.itemFlameCount--;
		if (drawPlayer.itemFlameCount <= 0)
		{
			drawPlayer.itemFlameCount = 5;
			for (int i = 0; i < 7; i++)
			{
				drawPlayer.itemFlamePos[i].X = (float)Main.rand.Next(-10, 11) * 0.15f;
				drawPlayer.itemFlamePos[i].Y = (float)Main.rand.Next(-10, 1) * 0.35f;
			}
		}
	}

	private void DrawPlayerStoned(Camera camera, Player drawPlayer, Vector2 position)
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0102: Unknown result type (might be due to invalid IL or missing references)
		//IL_0107: Unknown result type (might be due to invalid IL or missing references)
		//IL_0131: Unknown result type (might be due to invalid IL or missing references)
		//IL_013b: Unknown result type (might be due to invalid IL or missing references)
		if (!drawPlayer.dead)
		{
			SpriteEffects val = (SpriteEffects)0;
			val = ((drawPlayer.direction != 1) ? ((SpriteEffects)1) : ((SpriteEffects)0));
			camera.SpriteBatch.Draw(TextureAssets.Extra[37].Value, new Vector2((float)(int)(position.X - camera.UnscaledPosition.X - (float)(drawPlayer.bodyFrame.Width / 2) + (float)(drawPlayer.width / 2)), (float)(int)(position.Y - camera.UnscaledPosition.Y + (float)drawPlayer.height - (float)drawPlayer.bodyFrame.Height + 8f)) + drawPlayer.bodyPosition + new Vector2((float)(drawPlayer.bodyFrame.Width / 2), (float)(drawPlayer.bodyFrame.Height / 2)), (Rectangle?)null, Lighting.GetColor((int)((double)position.X + (double)drawPlayer.width * 0.5) / 16, (int)((double)position.Y + (double)drawPlayer.height * 0.5) / 16, Color.White), 0f, new Vector2((float)(TextureAssets.Extra[37].Width() / 2), (float)(TextureAssets.Extra[37].Height() / 2)), 1f, val, 0f);
		}
	}

	private void DrawGhost(Camera camera, Player drawPlayer, Vector2 position, float shadow = 0f)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_010e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0115: Unknown result type (might be due to invalid IL or missing references)
		//IL_0120: Unknown result type (might be due to invalid IL or missing references)
		//IL_012c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0133: Unknown result type (might be due to invalid IL or missing references)
		//IL_013e: Unknown result type (might be due to invalid IL or missing references)
		//IL_014a: Unknown result type (might be due to invalid IL or missing references)
		//IL_014f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0155: Unknown result type (might be due to invalid IL or missing references)
		//IL_015b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0162: Unknown result type (might be due to invalid IL or missing references)
		byte mouseTextColor = Main.mouseTextColor;
		SpriteEffects val = (drawPlayer.direction != 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None);
		Color immuneAlpha = drawPlayer.GetImmuneAlpha(Lighting.GetColor((int)((double)drawPlayer.position.X + (double)drawPlayer.width * 0.5) / 16, (int)((double)drawPlayer.position.Y + (double)drawPlayer.height * 0.5) / 16, new Color(mouseTextColor / 2 + 100, mouseTextColor / 2 + 100, mouseTextColor / 2 + 100, mouseTextColor / 2 + 100)), shadow);
		immuneAlpha.A = (byte)((float)(int)immuneAlpha.A * (1f - Math.Max(0.5f, shadow - 0.5f)));
		Rectangle val2 = new Rectangle(0, TextureAssets.Ghost.Height() / 4 * drawPlayer.ghostFrame, TextureAssets.Ghost.Width(), TextureAssets.Ghost.Height() / 4);
		Vector2 val3 = new Vector2((float)val2.Width * 0.5f, (float)val2.Height * 0.5f);
		camera.SpriteBatch.Draw(TextureAssets.Ghost.Value, new Vector2((float)(int)(position.X - camera.UnscaledPosition.X + (float)(val2.Width / 2)), (float)(int)(position.Y - camera.UnscaledPosition.Y + (float)(val2.Height / 2))), (Rectangle?)val2, immuneAlpha, 0f, val3, 1f, val, 0f);
	}
}
