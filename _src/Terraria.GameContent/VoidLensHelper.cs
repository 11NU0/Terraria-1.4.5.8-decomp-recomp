using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.DataStructures;
using Terraria.ID;

namespace Terraria.GameContent;

public struct VoidLensHelper
{
	private readonly Vector2 _position;

	private readonly float _opacity;

	private readonly int _frameNumber;

	public VoidLensHelper(Projectile proj)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		_position = proj.Center;
		_opacity = proj.Opacity;
		_frameNumber = proj.frame;
	}

	public VoidLensHelper(Vector2 worldPosition, float opacity)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		worldPosition.Y -= 2f;
		_position = worldPosition;
		_opacity = opacity;
		_frameNumber = (int)(((float)Main.tileFrameCounter[491] + _position.X + _position.Y) % 40f) / 5;
	}

	public void Update()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		Lighting.AddLight(_position, 0.4f, 0.2f, 0.9f);
		SpawnVoidLensDust();
	}

	public void SpawnVoidLensDust()
	{
		//IL_016b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0179: Unknown result type (might be due to invalid IL or missing references)
		//IL_017e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0180: Unknown result type (might be due to invalid IL or missing references)
		//IL_018c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0191: Unknown result type (might be due to invalid IL or missing references)
		//IL_0196: Unknown result type (might be due to invalid IL or missing references)
		//IL_0199: Unknown result type (might be due to invalid IL or missing references)
		//IL_019e: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01de: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0203: Unknown result type (might be due to invalid IL or missing references)
		//IL_0205: Unknown result type (might be due to invalid IL or missing references)
		//IL_020a: Unknown result type (might be due to invalid IL or missing references)
		//IL_021d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0222: Unknown result type (might be due to invalid IL or missing references)
		//IL_0227: Unknown result type (might be due to invalid IL or missing references)
		//IL_022e: Unknown result type (might be due to invalid IL or missing references)
		//IL_023b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0241: Unknown result type (might be due to invalid IL or missing references)
		//IL_0242: Unknown result type (might be due to invalid IL or missing references)
		//IL_024c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0251: Unknown result type (might be due to invalid IL or missing references)
		//IL_028e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0295: Unknown result type (might be due to invalid IL or missing references)
		//IL_029f: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0135: Unknown result type (might be due to invalid IL or missing references)
		//IL_013b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0145: Unknown result type (might be due to invalid IL or missing references)
		//IL_014a: Unknown result type (might be due to invalid IL or missing references)
		//IL_014f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0156: Unknown result type (might be due to invalid IL or missing references)
		//IL_0160: Unknown result type (might be due to invalid IL or missing references)
		//IL_0165: Unknown result type (might be due to invalid IL or missing references)
		if (Main.rand.Next(3) == 0)
		{
			if (Main.rand.Next(2) == 0)
			{
				Vector2 val = Vector2.UnitY.RotatedByRandom(6.2831854820251465);
				val *= new Vector2(0.5f, 1f);
				Dust dust = Dust.NewDustDirect(_position - val * 30f, 0, 0, Utils.SelectRandom<int>(Main.rand, 86, 88));
				dust.noGravity = true;
				dust.noLightEmittance = true;
				dust.position = _position - val.SafeNormalize(Vector2.Zero) * (float)Main.rand.Next(10, 21);
				dust.velocity = val.RotatedBy(1.5707963705062866) * 2f;
				dust.scale = 0.5f + Main.rand.NextFloat();
				dust.fadeIn = 0.5f;
				dust.customData = this;
				dust.position += dust.velocity * 10f;
				dust.velocity *= -1f;
			}
			else
			{
				Vector2 val2 = Vector2.UnitY.RotatedByRandom(6.2831854820251465);
				val2 *= new Vector2(0.5f, 1f);
				Dust dust2 = Dust.NewDustDirect(_position - val2 * 30f, 0, 0, Utils.SelectRandom<int>(Main.rand, 86, 88));
				dust2.noGravity = true;
				dust2.noLightEmittance = true;
				dust2.position = _position - val2.SafeNormalize(Vector2.Zero) * (float)Main.rand.Next(5, 10);
				dust2.velocity = val2.RotatedBy(-1.5707963705062866) * 3f;
				dust2.scale = 0.5f + Main.rand.NextFloat();
				dust2.fadeIn = 0.5f;
				dust2.customData = this;
				dust2.position += dust2.velocity * 10f;
				dust2.velocity *= -1f;
			}
		}
	}

	public void DrawToDrawData(List<DrawData> drawDataList, int selectionMode)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0101: Unknown result type (might be due to invalid IL or missing references)
		//IL_0115: Unknown result type (might be due to invalid IL or missing references)
		//IL_0128: Unknown result type (might be due to invalid IL or missing references)
		//IL_012d: Unknown result type (might be due to invalid IL or missing references)
		//IL_013a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0153: Unknown result type (might be due to invalid IL or missing references)
		//IL_0166: Unknown result type (might be due to invalid IL or missing references)
		//IL_016b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0170: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fe: Unknown result type (might be due to invalid IL or missing references)
		Main.instance.LoadProjectile(734);
		Asset<Texture2D> val = TextureAssets.Projectile[734];
		Rectangle val2 = val.Frame(1, 8, 0, _frameNumber);
		Color val3 = Lighting.GetColor(_position.ToTileCoordinates());
		val3 = Color.Lerp(val3, Color.White, 0.5f);
		val3 *= _opacity;
		DrawData drawData = new DrawData(val.Value, _position - Main.screenPosition, val2, val3, 0f, val2.Size() / 2f, 1f, (SpriteEffects)0);
		drawDataList.Add(drawData);
		for (float num = 0f; num < 1f; num += 0.34f)
		{
			DrawData item = drawData;
			item.color = new Color(127, 50, 127, 0) * _opacity;
			ref Vector2 scale = ref item.scale;
			scale *= 1.1f;
			float x = (Main.GlobalTimeWrappedHourly / 5f * ((float)Math.PI * 2f)).ToRotationVector2().X;
			ref Color color = ref item.color;
			color *= x * 0.1f + 0.3f;
			ref Vector2 position = ref item.position;
			position += ((Main.GlobalTimeWrappedHourly / 5f + num) * ((float)Math.PI * 2f)).ToRotationVector2() * (x * 1f + 2f);
			drawDataList.Add(item);
		}
		if (selectionMode != 0)
		{
			int num2 = (val3.R + val3.G + val3.B) / 3;
			if (num2 > 10)
			{
				Color selectionGlowColor = Colors.GetSelectionGlowColor(selectionMode == 2, num2);
				drawData = new DrawData(TextureAssets.Extra[93].Value, _position - Main.screenPosition, val2, selectionGlowColor, 0f, val2.Size() / 2f, 1f, (SpriteEffects)0);
				drawDataList.Add(drawData);
			}
		}
	}
}
