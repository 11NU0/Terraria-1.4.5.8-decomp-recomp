using System;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.Graphics.Shaders;
using Terraria.ID;

namespace Terraria;

public static class DelegateMethods
{
	public static class CharacterPreview
	{
		public static void EtsyPet(Projectile proj, bool walking)
		{
			Float(proj, walking);
			if (walking)
			{
				float num = (float)Main.timeForVisualEffects % 90f / 90f;
				proj.localAI[1] = (float)Math.PI * 2f * num;
			}
			else
			{
				proj.localAI[1] = 0f;
			}
		}

		public static void CompanionCubePet(Projectile proj, bool walking)
		{
			if (walking)
			{
				float num = (float)Main.timeForVisualEffects % 120f;
				int num2 = (int)(num / 30f);
				float percent = num % 30f / 30f;
				float num3 = Utils.MultiLerp(percent, 0f, 0f, 16f, 20f, 20f, 16f, 0f, 0f);
				float num4 = Utils.MultiLerp(percent, 0f, 0f, 0.2f, 0.5f, 0.75f, 0.8f, 1f, 1f);
				proj.position.Y -= num3;
				proj.rotation = (float)num2 * ((float)Math.PI / 2f) + num4 * ((float)Math.PI / 2f);
			}
			else
			{
				proj.rotation = 0f;
			}
		}

		public static void BerniePet(Projectile proj, bool walking)
		{
			if (walking)
			{
				proj.position.X += 6f;
			}
		}

		public static void SlimePet(Projectile proj, bool walking)
		{
			if (walking)
			{
				float percent = (float)Main.timeForVisualEffects % 30f / 30f;
				proj.position.Y -= Utils.MultiLerp(percent, 0f, 0f, 16f, 20f, 20f, 16f, 0f, 0f);
			}
		}

		public static void WormPet(Projectile proj, bool walking)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0093: Unknown result type (might be due to invalid IL or missing references)
			//IL_009f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_0089: Unknown result type (might be due to invalid IL or missing references)
			//IL_008e: Unknown result type (might be due to invalid IL or missing references)
			float num = -0.3985988f;
			Vector2 val = (Vector2.UnitY * 2f).RotatedBy(num);
			Vector2 val2 = proj.position;
			int num2 = proj.oldPos.Length;
			if (proj.type == 893)
			{
				num2 = proj.oldPos.Length - 30;
			}
			for (int i = 0; i < proj.oldPos.Length; i++)
			{
				val2 -= val;
				if (i < num2)
				{
					proj.oldPos[i] = val2;
				}
				else if (i > 0)
				{
					proj.oldPos[i] = proj.oldPos[i - 1];
				}
				val = val.RotatedBy(-0.05235987901687622);
			}
			proj.rotation = val.ToRotation() + (float)Math.PI / 10f + (float)Math.PI;
			if (proj.type == 887)
			{
				proj.rotation += (float)Math.PI / 8f;
			}
			if (proj.type == 893)
			{
				proj.rotation += (float)Math.PI / 2f;
			}
		}

		public static void FloatAndSpinWhenWalking(Projectile proj, bool walking)
		{
			Float(proj, walking);
			if (walking)
			{
				proj.rotation = (float)Math.PI * 2f * ((float)Main.timeForVisualEffects % 20f / 20f);
			}
			else
			{
				proj.rotation = 0f;
			}
		}

		public static void FloatAndRotateForwardWhenWalking(Projectile proj, bool walking)
		{
			Float(proj, walking);
			RotateForwardWhenWalking(proj, walking);
		}

		public static void Float(Projectile proj, bool walking)
		{
			float num = 0.5f;
			float num2 = (float)Main.timeForVisualEffects % 60f / 60f;
			proj.position.Y += 0f - num + (float)(Math.Cos(num2 * ((float)Math.PI * 2f) * 2f) * (double)(num * 2f));
		}

		public static void RotateForwardWhenWalking(Projectile proj, bool walking)
		{
			if (walking)
			{
				proj.rotation = (float)Math.PI / 6f;
			}
			else
			{
				proj.rotation = 0f;
			}
		}

		public static void SpinWhenWalking(Projectile proj, bool walking)
		{
			if (walking)
			{
				proj.rotation = (float)Math.PI * 2f * ((float)Main.timeForVisualEffects % 20f / 20f);
			}
			else
			{
				proj.rotation = 0f;
			}
		}
	}

	public static class Mount
	{
		public static Dust BatDashDust(Player player, int currentDustCount, Dust dust)
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			if (currentDustCount % 2 == 0)
			{
				dust.active = false;
			}
			else
			{
				dust.position = Main.rand.NextVector2FromRectangle(player.Hitbox);
				dust.scale *= 0.75f;
			}
			return dust;
		}

		public static bool BatPlayerSize(Player player, out Vector2? size)
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			size = new Vector2(20f, 18f);
			return true;
		}

		public static bool PixiePlayerSize(Player player, out Vector2? size)
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			size = new Vector2(8f, 14f);
			return true;
		}

		public static Dust PixieDashDust(Player player, int currentDustCount, Dust dust)
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			if (currentDustCount % 2 == 0)
			{
				dust.active = false;
			}
			else
			{
				dust.position = Main.rand.NextVector2FromRectangle(player.Hitbox);
				dust.scale *= 0.75f;
			}
			return dust;
		}

		public static bool RatPlayerSize(Player player, out Vector2? size)
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			size = new Vector2(14f, 14f);
			return true;
		}

		public static bool NoPosition(Player player, out Vector2? position)
		{
			position = null;
			return true;
		}

		public static bool WolfMouthPosition(Player player, out Vector2? position)
		{
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			Vector2 spinningpoint = new Vector2((float)(player.direction * 22), player.gravDir * -6f);
			position = player.RotatedRelativePoint(player.MountedCenter, reverseRotation: false, addGfxOffY: false) + spinningpoint.RotatedBy(player.fullRotation);
			return true;
		}

		public static bool VelociraptorMouthPosition(Player player, out Vector2? position)
		{
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			Vector2 spinningpoint = new Vector2((float)(player.direction * 24), player.gravDir * -12f);
			position = player.RotatedRelativePoint(player.MountedCenter, reverseRotation: false, addGfxOffY: false) + spinningpoint.RotatedBy(player.fullRotation);
			return true;
		}
	}

	public static class Minecart
	{
		public static Vector2 rotationOrigin;

		public static float rotation;

		public static void Sparks(Vector2 dustPosition)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
			dustPosition += new Vector2((float)((Main.rand.Next(2) == 0) ? 13 : (-13)), 0f).RotatedBy(rotation);
			int num = Dust.NewDust(dustPosition, 1, 1, 213, Main.rand.Next(-2, 3), Main.rand.Next(-2, 3));
			Main.dust[num].noGravity = true;
			Main.dust[num].fadeIn = Main.dust[num].scale + 1f + 0.01f * (float)Main.rand.Next(0, 51);
			Main.dust[num].noGravity = true;
			Dust obj = Main.dust[num];
			obj.velocity *= (float)Main.rand.Next(15, 51) * 0.01f;
			Main.dust[num].velocity.X *= (float)Main.rand.Next(25, 101) * 0.01f;
			Main.dust[num].velocity.Y -= (float)Main.rand.Next(15, 31) * 0.1f;
			Main.dust[num].position.Y -= 4f;
			if (Main.rand.Next(3) != 0)
			{
				Main.dust[num].noGravity = false;
			}
			else
			{
				Main.dust[num].scale *= 0.6f;
			}
		}

		public static void JumpingSound(Player Player, Vector2 Position, int Width, int Height)
		{
		}

		public static void LandingSound(Player Player, Vector2 Position, int Width, int Height)
		{
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			SoundEngine.PlaySound(SoundID.Item53, (int)Position.X + Width / 2, (int)Position.Y + Height / 2);
		}

		public static void BumperSound(Player Player, Vector2 Position, int Width, int Height)
		{
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			SoundEngine.PlaySound(SoundID.Item56, (int)Position.X + Width / 2, (int)Position.Y + Height / 2);
		}

		public static void SpawnFartCloud(Player Player, Vector2 Position, int Width, int Height, bool useDelay = true)
		{
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_006e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_012b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0135: Unknown result type (might be due to invalid IL or missing references)
			//IL_013a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0147: Unknown result type (might be due to invalid IL or missing references)
			//IL_014c: Unknown result type (might be due to invalid IL or missing references)
			//IL_014d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0152: Unknown result type (might be due to invalid IL or missing references)
			//IL_0173: Unknown result type (might be due to invalid IL or missing references)
			//IL_0178: Unknown result type (might be due to invalid IL or missing references)
			//IL_017d: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_01cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_006d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0211: Unknown result type (might be due to invalid IL or missing references)
			//IL_021b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0220: Unknown result type (might be due to invalid IL or missing references)
			//IL_0225: Unknown result type (might be due to invalid IL or missing references)
			//IL_023d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0263: Unknown result type (might be due to invalid IL or missing references)
			//IL_0268: Unknown result type (might be due to invalid IL or missing references)
			//IL_026f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0284: Unknown result type (might be due to invalid IL or missing references)
			//IL_0289: Unknown result type (might be due to invalid IL or missing references)
			//IL_029e: Unknown result type (might be due to invalid IL or missing references)
			//IL_02af: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_031d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0322: Unknown result type (might be due to invalid IL or missing references)
			//IL_0323: Unknown result type (might be due to invalid IL or missing references)
			//IL_0328: Unknown result type (might be due to invalid IL or missing references)
			if (useDelay)
			{
				if (Player.fartKartCloudDelay > 0)
				{
					return;
				}
				Player.fartKartCloudDelay = 20;
			}
			float num = 10f;
			float num2 = -4f;
			Vector2 val = Position + new Vector2((float)(Width / 2 - 18), (float)(Height - 16));
			Vector2 val2 = Player.velocity * 0.1f;
			if (val2.Length() > 2f)
			{
				val2 = val2.SafeNormalize(Vector2.Zero) * 2f;
			}
			int num3 = Gore.NewGore(val + new Vector2(0f, num2), Vector2.Zero, Main.rand.Next(435, 438));
			Gore obj = Main.gore[num3];
			obj.velocity *= 0.2f;
			Gore obj2 = Main.gore[num3];
			obj2.velocity += val2;
			Main.gore[num3].velocity.Y *= 0.75f;
			num3 = Gore.NewGore(val + new Vector2(0f - num, num2), Vector2.Zero, Main.rand.Next(435, 438));
			Gore obj3 = Main.gore[num3];
			obj3.velocity *= 0.2f;
			Gore obj4 = Main.gore[num3];
			obj4.velocity += val2;
			Main.gore[num3].velocity.Y *= 0.75f;
			num3 = Gore.NewGore(val + new Vector2(num, num2), Vector2.Zero, Main.rand.Next(435, 438));
			Gore obj5 = Main.gore[num3];
			obj5.velocity *= 0.2f;
			Gore obj6 = Main.gore[num3];
			obj6.velocity += val2;
			Main.gore[num3].velocity.Y *= 0.75f;
			if (Player.mount.Active && Player.mount.Type == 53)
			{
				Vector2 val3 = Position + new Vector2((float)(Width / 2), (float)(Height + 10));
				float num4 = 30f;
				float num5 = -16f;
				for (int i = 0; i < 15; i++)
				{
					Dust dust = Dust.NewDustPerfect(val3 + new Vector2(0f - num4 + num4 * 2f * Main.rand.NextFloat(), num5 * Main.rand.NextFloat()), 107, Vector2.Zero, 100, Color.Lerp(new Color(64, 220, 96), Color.White, Main.rand.NextFloat() * 0.3f), 0.6f);
					dust.velocity *= (float)Main.rand.Next(15, 51) * 0.01f;
					dust.velocity.X *= (float)Main.rand.Next(25, 101) * 0.01f;
					dust.velocity.Y -= (float)Main.rand.Next(15, 31) * 0.1f;
					dust.velocity += val2;
					dust.velocity.Y *= 0.75f;
					dust.fadeIn = 0.2f + Main.rand.NextFloat() * 0.1f;
					dust.noGravity = Main.rand.Next(3) == 0;
					dust.noLightEmittance = true;
				}
			}
		}

		public static void JumpingSoundFart(Player Player, Vector2 Position, int Width, int Height)
		{
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			SoundEngine.PlaySound(SoundID.Item16, (int)Position.X + Width / 2, (int)Position.Y + Height / 2);
			SpawnFartCloud(Player, Position, Width, Height, useDelay: false);
		}

		public static void LandingSoundFart(Player Player, Vector2 Position, int Width, int Height)
		{
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			SoundEngine.PlaySound(SoundID.Item16, (int)Position.X + Width / 2, (int)Position.Y + Height / 2);
			SoundEngine.PlaySound(SoundID.Item53, (int)Position.X + Width / 2, (int)Position.Y + Height / 2);
			SpawnFartCloud(Player, Position, Width, Height, useDelay: false);
		}

		public static void BumperSoundFart(Player Player, Vector2 Position, int Width, int Height)
		{
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			SoundEngine.PlaySound(SoundID.Item16, (int)Position.X + Width / 2, (int)Position.Y + Height / 2);
			SoundEngine.PlaySound(SoundID.Item56, (int)Position.X + Width / 2, (int)Position.Y + Height / 2);
			SpawnFartCloud(Player, Position, Width, Height);
		}

		public static void SparksFart(Vector2 dustPosition)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_0101: Unknown result type (might be due to invalid IL or missing references)
			//IL_0106: Unknown result type (might be due to invalid IL or missing references)
			dustPosition += new Vector2((float)((Main.rand.Next(2) == 0) ? 13 : (-13)), 0f).RotatedBy(rotation);
			int num = Dust.NewDust(dustPosition, 1, 1, 211, Main.rand.Next(-2, 3), Main.rand.Next(-2, 3), 50, default, 0.8f);
			if (Main.rand.Next(2) == 0)
			{
				Main.dust[num].alpha += 25;
			}
			if (Main.rand.Next(2) == 0)
			{
				Main.dust[num].alpha += 25;
			}
			Main.dust[num].noLight = true;
			Main.dust[num].noGravity = Main.rand.Next(3) == 0;
			Dust obj = Main.dust[num];
			obj.velocity *= (float)Main.rand.Next(15, 51) * 0.01f;
			Main.dust[num].velocity.X *= (float)Main.rand.Next(25, 101) * 0.01f;
			Main.dust[num].velocity.Y -= (float)Main.rand.Next(15, 31) * 0.1f;
			Main.dust[num].position.Y -= 4f;
		}

		public static void SparksTerraFart(Vector2 dustPosition)
		{
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0094: Unknown result type (might be due to invalid IL or missing references)
			//IL_0117: Unknown result type (might be due to invalid IL or missing references)
			//IL_0131: Unknown result type (might be due to invalid IL or missing references)
			//IL_0136: Unknown result type (might be due to invalid IL or missing references)
			if (Main.rand.Next(2) == 0)
			{
				SparksFart(dustPosition);
				return;
			}
			dustPosition += new Vector2((float)((Main.rand.Next(2) == 0) ? 13 : (-13)), 0f).RotatedBy(rotation);
			int num = Dust.NewDust(dustPosition, 1, 1, 107, Main.rand.Next(-2, 3), Main.rand.Next(-2, 3), 100, Color.Lerp(new Color(64, 220, 96), Color.White, Main.rand.NextFloat() * 0.3f), 0.8f);
			if (Main.rand.Next(2) == 0)
			{
				Main.dust[num].alpha += 25;
			}
			if (Main.rand.Next(2) == 0)
			{
				Main.dust[num].alpha += 25;
			}
			Main.dust[num].noLightEmittance = true;
			Main.dust[num].noGravity = Main.rand.Next(3) == 0;
			Dust obj = Main.dust[num];
			obj.velocity *= (float)Main.rand.Next(15, 51) * 0.01f;
			Main.dust[num].velocity.X *= (float)Main.rand.Next(25, 101) * 0.01f;
			Main.dust[num].velocity.Y -= (float)Main.rand.Next(15, 31) * 0.1f;
			Main.dust[num].position.Y -= 4f;
		}

		public static void SparksMech(Vector2 dustPosition)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
			dustPosition += new Vector2((float)((Main.rand.Next(2) == 0) ? 13 : (-13)), 0f).RotatedBy(rotation);
			int num = Dust.NewDust(dustPosition, 1, 1, 260, Main.rand.Next(-2, 3), Main.rand.Next(-2, 3));
			Main.dust[num].noGravity = true;
			Main.dust[num].fadeIn = Main.dust[num].scale + 0.5f + 0.01f * (float)Main.rand.Next(0, 51);
			Main.dust[num].noGravity = true;
			Dust obj = Main.dust[num];
			obj.velocity *= (float)Main.rand.Next(15, 51) * 0.01f;
			Main.dust[num].velocity.X *= (float)Main.rand.Next(25, 101) * 0.01f;
			Main.dust[num].velocity.Y -= (float)Main.rand.Next(15, 31) * 0.1f;
			Main.dust[num].position.Y -= 4f;
			if (Main.rand.Next(3) != 0)
			{
				Main.dust[num].noGravity = false;
			}
			else
			{
				Main.dust[num].scale *= 0.6f;
			}
		}

		public static void SparksMeow(Vector2 dustPosition)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0102: Unknown result type (might be due to invalid IL or missing references)
			dustPosition += new Vector2((float)((Main.rand.Next(2) == 0) ? 13 : (-13)), 0f).RotatedBy(rotation);
			int num = Dust.NewDust(dustPosition, 1, 1, 213, Main.rand.Next(-2, 3), Main.rand.Next(-2, 3));
			Main.dust[num].shader = GameShaders.Armor.GetShaderFromItemId(2870);
			Main.dust[num].noGravity = true;
			Main.dust[num].fadeIn = Main.dust[num].scale + 1f + 0.01f * (float)Main.rand.Next(0, 51);
			Main.dust[num].noGravity = true;
			Dust obj = Main.dust[num];
			obj.velocity *= (float)Main.rand.Next(15, 51) * 0.01f;
			Main.dust[num].velocity.X *= (float)Main.rand.Next(25, 101) * 0.01f;
			Main.dust[num].velocity.Y -= (float)Main.rand.Next(15, 31) * 0.1f;
			Main.dust[num].position.Y -= 4f;
			if (Main.rand.Next(3) != 0)
			{
				Main.dust[num].noGravity = false;
			}
			else
			{
				Main.dust[num].scale *= 0.6f;
			}
		}
	}

	public static Vector3 v3_1 = Vector3.Zero;

	public static Vector2 v2_1 = Vector2.Zero;

	public static float f_1 = 0f;

	public static Color c_1 = Color.Transparent;

	public static int i_1;

	public static bool CheckResultOut;

	public static TileCuttingContext tilecut_0 = TileCuttingContext.Unknown;

	public static bool[] tileCutIgnore = null;

	public static Color ColorLerp_BlackToWhite(float percent)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		return Color.Lerp(Color.Black, Color.White, percent);
	}

	public static Color ColorLerp_HSL_H(float percent)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		return Main.hslToRgb(percent, 1f, 0.5f);
	}

	public static Color ColorLerp_HSL_S(float percent)
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		return Main.hslToRgb(v3_1.X, percent, v3_1.Z);
	}

	public static Color ColorLerp_HSL_L(float percent)
	{
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		return Main.hslToRgb(v3_1.X, v3_1.Y, 0.15f + 0.85f * percent);
	}

	public static Color ColorLerp_HSL_O(float percent)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		return Color.Lerp(Color.White, Main.hslToRgb(v3_1.X, v3_1.Y, v3_1.Z), percent);
	}

	public static bool SpreadIceBlocksOverWater(int x, int y)
	{
		return SpreadTile(x, y, 161, 80, pyramidShape: false, 0);
	}

	public static bool SpreadDirt(int x, int y)
	{
		return SpreadTile(x, y, 0, 0);
	}

	public static bool SpreadPoopPyramid(int x, int y)
	{
		return SpreadTile(x, y, 666, 322, pyramidShape: true);
	}

	public static bool SpreadTile(int x, int y, int tileType, int dustType, bool pyramidShape = false, int overLiquidType = -1)
	{
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_014c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0159: Unknown result type (might be due to invalid IL or missing references)
		//IL_016c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0197: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_01be: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f1: Unknown result type (might be due to invalid IL or missing references)
		float num = Math.Max(0.1f, f_1);
		if (pyramidShape)
		{
			float num2 = num;
			float num3 = v2_1.Y + num / 2f;
			float num4 = Math.Abs((float)y - num3) / num;
			num2 *= 1f - num4;
			if (Math.Abs((float)x - v2_1.X) > num2 || Math.Abs((float)y - v2_1.Y) > num)
			{
				return false;
			}
		}
		else if (Vector2.Distance(v2_1, new Vector2((float)x, (float)y)) > num)
		{
			return false;
		}
		if (overLiquidType >= 0 && (Main.tile[x, y].liquid == 0 || Main.tile[x, y].liquidType() != overLiquidType))
		{
			if (WorldGen.SolidTile(x, y) && Main.tile[x, y].type != tileType)
			{
				return false;
			}
			return true;
		}
		WorldGen.TryKillingReplaceableTile(x, y, tileType);
		if (WorldGen.PlaceTile(x, y, tileType))
		{
			if (overLiquidType >= 0)
			{
				Main.tile[x, y].Clear(TileDataType.Liquid);
				WorldGen.SquareTileFrame(x, y, resetFrame: false);
				if (Main.netMode != 0)
				{
					NetMessage.sendWater(x, y);
				}
				else
				{
					Liquid.AddWater(x, y);
				}
			}
			if (Main.netMode != 0)
			{
				NetMessage.SendData(17, -1, -1, null, 1, x, y, tileType);
			}
			Vector2 position = new Vector2((float)(x * 16), (float)(y * 16));
			for (int i = 0; i < 3; i++)
			{
				Dust dust = Dust.NewDustDirect(position, 16, 16, dustType, 0f, 0f, 100, Color.Transparent, 2.2f);
				dust.noGravity = true;
				dust.velocity.Y -= 1.2f;
				dust.velocity *= 4f;
				Dust dust2 = Dust.NewDustDirect(position, 16, 16, dustType, 0f, 0f, 100, Color.Transparent, 1.3f);
				dust2.velocity.Y -= 1.2f;
				dust2.velocity *= 2f;
			}
			int num5 = y + 1;
			if (Main.tile[x, num5] != null && !TileID.Sets.Platforms[Main.tile[x, num5].type] && (Main.tile[x, num5].topSlope() || Main.tile[x, num5].halfBrick()))
			{
				WorldGen.SlopeTile(x, num5);
				if (Main.netMode != 0)
				{
					NetMessage.SendData(17, -1, -1, null, 14, x, num5);
				}
			}
			num5 = y - 1;
			if (Main.tile[x, num5] != null && !TileID.Sets.Platforms[Main.tile[x, num5].type] && Main.tile[x, num5].bottomSlope())
			{
				WorldGen.SlopeTile(x, num5);
				if (Main.netMode != 0)
				{
					NetMessage.SendData(17, -1, -1, null, 14, x, num5);
				}
			}
			for (int j = x - 1; j <= x + 1; j++)
			{
				for (int k = y - 1; k <= y + 1; k++)
				{
					Tile tile = Main.tile[j, k];
					if (!tile.active() || tileType == tile.type || (tile.type != 2 && tile.type != 23 && tile.type != 60 && tile.type != 70 && tile.type != 109 && tile.type != 199 && tile.type != 477 && tile.type != 492))
					{
						continue;
					}
					bool flag = true;
					for (int l = j - 1; l <= j + 1; l++)
					{
						for (int m = k - 1; m <= k + 1; m++)
						{
							if (!WorldGen.SolidTile(l, m))
							{
								flag = false;
							}
						}
					}
					if (flag)
					{
						WorldGen.KillTile(j, k, fail: true);
						if (Main.netMode != 0)
						{
							NetMessage.SendData(17, -1, -1, null, 0, j, k, 1f);
						}
					}
				}
			}
			return true;
		}
		if (overLiquidType >= 0 && !Collision.EmptyTile(x, y, ignoreTiles: true))
		{
			return true;
		}
		Tile tile2 = Main.tile[x, y];
		if (tile2 == null)
		{
			return false;
		}
		if (tile2.type < 0 || tile2.type >= TileID.Count)
		{
			return false;
		}
		if (Main.tileSolid[tile2.type] && !TileID.Sets.Platforms[tile2.type])
		{
			return tile2.type == 380;
		}
		return true;
	}

	public static bool SpreadWater(int x, int y)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
		if (Vector2.Distance(v2_1, new Vector2((float)x, (float)y)) > f_1)
		{
			return false;
		}
		if (WorldGen.PlaceLiquid(x, y, 0, byte.MaxValue))
		{
			Vector2 position = new Vector2((float)(x * 16), (float)(y * 16));
			int type = Dust.dustWater();
			for (int i = 0; i < 3; i++)
			{
				Dust dust = Dust.NewDustDirect(position, 16, 16, type, 0f, 0f, 100, Color.Transparent, 2.2f);
				dust.noGravity = true;
				dust.velocity.Y -= 1.2f;
				dust.velocity *= 7f;
				Dust dust2 = Dust.NewDustDirect(position, 16, 16, type, 0f, 0f, 100, Color.Transparent, 1.3f);
				dust2.velocity.Y -= 1.2f;
				dust2.velocity *= 4f;
			}
			return true;
		}
		return false;
	}

	public static bool SpreadHoney(int x, int y)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		if (Vector2.Distance(v2_1, new Vector2((float)x, (float)y)) > f_1)
		{
			return false;
		}
		if (WorldGen.PlaceLiquid(x, y, 2, byte.MaxValue))
		{
			Vector2 position = new Vector2((float)(x * 16), (float)(y * 16));
			int type = 152;
			for (int i = 0; i < 3; i++)
			{
				Dust dust = Dust.NewDustDirect(position, 16, 16, type, 0f, 0f, 100, Color.Transparent, 2.2f);
				dust.velocity.Y -= 1.2f;
				dust.velocity *= 7f;
				Dust dust2 = Dust.NewDustDirect(position, 16, 16, type, 0f, 0f, 100, Color.Transparent, 1.3f);
				dust2.velocity.Y -= 1.2f;
				dust2.velocity *= 4f;
			}
			return true;
		}
		return false;
	}

	public static bool SpreadLava(int x, int y)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		if (Vector2.Distance(v2_1, new Vector2((float)x, (float)y)) > f_1)
		{
			return false;
		}
		if (WorldGen.PlaceLiquid(x, y, 1, byte.MaxValue))
		{
			Vector2 position = new Vector2((float)(x * 16), (float)(y * 16));
			int type = 35;
			for (int i = 0; i < 3; i++)
			{
				Dust dust = Dust.NewDustDirect(position, 16, 16, type, 0f, 0f, 100, Color.Transparent, 1.2f);
				dust.velocity *= 7f;
				Dust dust2 = Dust.NewDustDirect(position, 16, 16, type, 0f, 0f, 100, Color.Transparent, 0.8f);
				dust2.velocity *= 4f;
			}
			return true;
		}
		return false;
	}

	public static bool SpreadDry(int x, int y)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		if (Vector2.Distance(v2_1, new Vector2((float)x, (float)y)) > f_1)
		{
			return false;
		}
		if (WorldGen.EmptyLiquid(x, y))
		{
			Vector2 position = new Vector2((float)(x * 16), (float)(y * 16));
			int type = 31;
			for (int i = 0; i < 3; i++)
			{
				Dust dust = Dust.NewDustDirect(position, 16, 16, type, 0f, 0f, 100, Color.Transparent, 1.2f);
				dust.noGravity = true;
				dust.velocity *= 7f;
				Dust dust2 = Dust.NewDustDirect(position, 16, 16, type, 0f, 0f, 100, Color.Transparent, 0.8f);
				dust2.velocity *= 4f;
			}
			return true;
		}
		return false;
	}

	public static bool SpreadTest(int x, int y)
	{
		Tile tile = Main.tile[x, y];
		if (WorldGen.SolidTile(x, y) || tile.wall != 0)
		{
			tile.active();
			return false;
		}
		return true;
	}

	public static bool TestDust(int x, int y)
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		if (x < 0 || x >= Main.maxTilesX || y < 0 || y >= Main.maxTilesY)
		{
			return false;
		}
		int num = Dust.NewDust(new Vector2((float)x, (float)y) * 16f + new Vector2(8f), 0, 0, 6);
		Main.dust[num].noGravity = true;
		Main.dust[num].noLight = true;
		return true;
	}

	public static bool CastLight(int x, int y)
	{
		if (x < 0 || x >= Main.maxTilesX || y < 0 || y >= Main.maxTilesY)
		{
			return false;
		}
		if (Main.tile[x, y] == null)
		{
			return false;
		}
		Lighting.AddLight(x, y, v3_1.X, v3_1.Y, v3_1.Z);
		return true;
	}

	public static bool CastLightOpen(int x, int y)
	{
		if (x < 0 || x >= Main.maxTilesX || y < 0 || y >= Main.maxTilesY)
		{
			return false;
		}
		if (Main.tile[x, y] == null)
		{
			return false;
		}
		if (!Main.tile[x, y].active() || Main.tile[x, y].inActive() || Main.tileSolidTop[Main.tile[x, y].type] || !Main.tileSolid[Main.tile[x, y].type])
		{
			Lighting.AddLight(x, y, v3_1.X, v3_1.Y, v3_1.Z);
		}
		return true;
	}

	public static bool CheckStopForSolids(int x, int y)
	{
		if (x < 0 || x >= Main.maxTilesX || y < 0 || y >= Main.maxTilesY)
		{
			return false;
		}
		if (Main.tile[x, y] == null)
		{
			return false;
		}
		if (Main.tile[x, y].active() && !Main.tile[x, y].inActive() && !Main.tileSolidTop[Main.tile[x, y].type] && Main.tileSolid[Main.tile[x, y].type])
		{
			CheckResultOut = true;
			return false;
		}
		return true;
	}

	public static bool CastLightOpen_StopForSolids_ScaleWithDistance(int x, int y)
	{
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		if (x < 0 || x >= Main.maxTilesX || y < 0 || y >= Main.maxTilesY)
		{
			return false;
		}
		if (Main.tile[x, y] == null)
		{
			return false;
		}
		if (!Main.tile[x, y].active() || Main.tile[x, y].inActive() || Main.tileSolidTop[Main.tile[x, y].type] || !Main.tileSolid[Main.tile[x, y].type])
		{
			Vector3 val = v3_1;
			Vector2 val2 = new Vector2((float)x, (float)y);
			float num = Vector2.Distance(v2_1, val2);
			val *= MathHelper.Lerp(0.65f, 1f, num / f_1);
			Lighting.AddLight(x, y, val.X, val.Y, val.Z);
			return true;
		}
		return false;
	}

	public static bool CastLightOpen_StopForSolids(int x, int y)
	{
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		if (x < 0 || x >= Main.maxTilesX || y < 0 || y >= Main.maxTilesY)
		{
			return false;
		}
		if (Main.tile[x, y] == null)
		{
			return false;
		}
		if (!Main.tile[x, y].active() || Main.tile[x, y].inActive() || Main.tileSolidTop[Main.tile[x, y].type] || !Main.tileSolid[Main.tile[x, y].type])
		{
			Vector3 val = v3_1;
			new Vector2((float)x, (float)y);
			Lighting.AddLight(x, y, val.X, val.Y, val.Z);
			return true;
		}
		return false;
	}

	public static bool SpreadLightOpen_StopForSolids(int x, int y)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		if (Vector2.Distance(v2_1, new Vector2((float)x, (float)y)) > f_1)
		{
			return false;
		}
		Tile tile = Main.tile[x, y];
		if (tile == null || tile.active() || tile.inActive() || Main.tileSolidTop[tile.type] || !Main.tileSolid[tile.type])
		{
			Vector3 val = v3_1;
			new Vector2((float)x, (float)y);
			Lighting.AddLight(x, y, val.X, val.Y, val.Z);
			return true;
		}
		return false;
	}

	public static bool EmitGolfCartDust_StopForSolids(int x, int y)
	{
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		if (x < 0 || x >= Main.maxTilesX || y < 0 || y >= Main.maxTilesY)
		{
			return false;
		}
		if (Main.tile[x, y] == null)
		{
			return false;
		}
		if (!Main.tile[x, y].active() || Main.tile[x, y].inActive() || Main.tileSolidTop[Main.tile[x, y].type] || !Main.tileSolid[Main.tile[x, y].type])
		{
			Dust.NewDustPerfect(new Vector2((float)(x * 16 + 8), (float)(y * 16 + 8)), 260, Vector2.UnitY * -0.2f);
			return true;
		}
		return false;
	}

	public static bool NotSolidOrPlatforms(int x, int y)
	{
		Tile tile = Main.tile[x, y];
		if (tile == null)
		{
			return false;
		}
		if (!tile.active())
		{
			return true;
		}
		if (Main.tileSolid[tile.type] || TileID.Sets.Platforms[tile.type])
		{
			return false;
		}
		return true;
	}

	public static bool NotDoorStand(int x, int y)
	{
		if (Main.tile[x, y] != null && Main.tile[x, y].active() && Main.tile[x, y].type == 11)
		{
			if (Main.tile[x, y].frameX >= 18)
			{
				return Main.tile[x, y].frameX < 54;
			}
			return false;
		}
		return true;
	}

	public static bool CutTiles(int x, int y)
	{
		if (!WorldGen.InWorld(x, y, 1))
		{
			return false;
		}
		if (Main.tile[x, y] == null)
		{
			return false;
		}
		if (!Main.tileCut[Main.tile[x, y].type])
		{
			return true;
		}
		if (tileCutIgnore[Main.tile[x, y].type])
		{
			return true;
		}
		if (WorldGen.CanCutTile(x, y, tilecut_0))
		{
			WorldGen.KillTile(x, y);
			if (Main.netMode != 0)
			{
				NetMessage.SendData(17, -1, -1, null, 0, x, y);
			}
		}
		return true;
	}

	public static bool SearchAvoidedByNPCs(int x, int y)
	{
		if (!WorldGen.InWorld(x, y, 1))
		{
			return false;
		}
		if (Main.tile[x, y] == null)
		{
			return false;
		}
		if (!Main.tile[x, y].active() || !TileID.Sets.AvoidedByNPCs[Main.tile[x, y].type])
		{
			return true;
		}
		return false;
	}

	public static void RainbowLaserDraw(int stage, Vector2 currentPosition, float distanceLeft, Rectangle lastFrame, out float distCovered, out Rectangle frame, out Vector2 origin, out Color color)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		color = c_1;
		switch (stage)
		{
		case 0:
			distCovered = 33f;
			frame = new Rectangle(0, 0, 26, 22);
			origin = frame.Size() / 2f;
			break;
		case 1:
			frame = new Rectangle(0, 25, 26, 28);
			distCovered = frame.Height;
			origin = new Vector2((float)(frame.Width / 2), 0f);
			break;
		case 2:
			distCovered = 22f;
			frame = new Rectangle(0, 56, 26, 22);
			origin = new Vector2((float)(frame.Width / 2), 1f);
			break;
		default:
			distCovered = 9999f;
			frame = Rectangle.Empty;
			origin = Vector2.Zero;
			color = Color.Transparent;
			break;
		}
	}

	public static void TurretLaserDraw(int stage, Vector2 currentPosition, float distanceLeft, Rectangle lastFrame, out float distCovered, out Rectangle frame, out Vector2 origin, out Color color)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0102: Unknown result type (might be due to invalid IL or missing references)
		//IL_0107: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		color = c_1;
		switch (stage)
		{
		case 0:
			distCovered = 32f;
			frame = new Rectangle(0, 0, 22, 20);
			origin = frame.Size() / 2f;
			break;
		case 1:
		{
			i_1++;
			int num = i_1 % 5;
			frame = new Rectangle(0, 22 * (num + 1), 22, 20);
			distCovered = frame.Height - 1;
			origin = new Vector2((float)(frame.Width / 2), 0f);
			break;
		}
		case 2:
			frame = new Rectangle(0, 154, 22, 30);
			distCovered = frame.Height;
			origin = new Vector2((float)(frame.Width / 2), 1f);
			break;
		default:
			distCovered = 9999f;
			frame = Rectangle.Empty;
			origin = Vector2.Zero;
			color = Color.Transparent;
			break;
		}
	}

	public static void LightningLaserDraw(int stage, Vector2 currentPosition, float distanceLeft, Rectangle lastFrame, out float distCovered, out Rectangle frame, out Vector2 origin, out Color color)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		color = c_1 * f_1;
		switch (stage)
		{
		case 0:
			distCovered = 0f;
			frame = new Rectangle(0, 0, 21, 8);
			origin = frame.Size() / 2f;
			break;
		case 1:
			frame = new Rectangle(0, 8, 21, 6);
			distCovered = frame.Height;
			origin = new Vector2((float)(frame.Width / 2), 0f);
			break;
		case 2:
			distCovered = 8f;
			frame = new Rectangle(0, 14, 21, 8);
			origin = new Vector2((float)(frame.Width / 2), 2f);
			break;
		default:
			distCovered = 9999f;
			frame = Rectangle.Empty;
			origin = Vector2.Zero;
			color = Color.Transparent;
			break;
		}
	}

	public static int CompareYReverse(Point a, Point b)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		return b.Y.CompareTo(a.Y);
	}

	public static int CompareDrawSorterByYScale(DrawData a, DrawData b)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		return a.scale.Y.CompareTo(b.scale.Y);
	}

	static DelegateMethods()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
	}
}
