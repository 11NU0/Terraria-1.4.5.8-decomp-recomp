using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.GameContent.Shaders;
using Terraria.Graphics.Effects;
using Terraria.ID;

namespace Terraria.Graphics.Renderers;

public class FakeFishParticle : IPooledParticle, IParticle, IParticleRepel
{
	private enum State
	{
		FreeRoaming,
		InterestedInBobber,
		Latched,
		Jumping
	}

	public Vector2 Position;

	public Vector2 Velocity;

	public float Rotation;

	private int _latchedProjectileType;

	private Projectile _latchedProjectile;

	private Item _itemInstance;

	private int _lifeTimeCounted;

	private int _lifeTimeTotal;

	private float _totalScale;

	private int _waitTime;

	private State _state;

	private int _jumpTimeLeft;

	private bool _itemLooksDiagonal;

	private Vector2 _targetVelocity;

	private Vector2 _bobberLocation;

	private int _steerTime;

	private bool _isInanimate;

	private Color _sonarColor;

	private int _sonarTimeleft;

	private int _sonarStartTime;

	private bool _isSelectedSonarType;

	private bool _wasWet;

	private int _delayTime;

	private bool _gotRepelled;

	public bool ShouldBeRemovedFromRenderer { get; private set; }

	public bool IsRestingInPool { get; private set; }

	public FakeFishParticle()
	{
		_itemInstance = new Item();
	}

	public bool TryToMagnetizeTo(Projectile projectile, int requiredItemType = -1)
	{
		if (!CanHit(projectile, requiredItemType))
		{
			return false;
		}
		_latchedProjectile = projectile;
		_latchedProjectileType = projectile.type;
		_state = State.Latched;
		SetAndGetLatchDetails(out var _, out var _);
		Rotate(instant: true);
		return true;
	}

	public bool TryToPushAway(Projectile projectile, int requiredItemType = -1)
	{
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		if (_state == State.Jumping)
		{
			return false;
		}
		if (requiredItemType == _itemInstance.type)
		{
			return false;
		}
		if (!CanHit(projectile, -1, 160f))
		{
			return false;
		}
		Velocity += projectile.DirectionTo(Position).SafeNormalize(Vector2.UnitY) * (1f + 1f * Main.rand.NextFloat()) * 1f;
		return true;
	}

	public bool TryToBePinged(Projectile projectile, int requiredItemType = -1)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		if (!CanHit(projectile, -1, 1000f))
		{
			return false;
		}
		int num = (int)projectile.Center.Distance(Position) / 10;
		_sonarTimeleft = (_sonarStartTime = 60) + num;
		_isSelectedSonarType = requiredItemType == _itemInstance.type;
		_sonarColor = (Color)(_isSelectedSonarType ? new Color(255, 220, 30, 0) : (new Color(30, 200, 255, 0) * 0.05f));
		return true;
	}

	public bool CanHit(Projectile projectile, int requiredItemType = -1, float allowedRange = 80f)
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		if (requiredItemType != -1 && _itemInstance.type != requiredItemType)
		{
			return false;
		}
		Vector2 center = projectile.Center;
		int num = 80;
		if (center.Distance(Position) > (float)num)
		{
			return false;
		}
		if (!Collision.CanHitLine(center, 0, 0, Position, 0, 0))
		{
			return false;
		}
		return true;
	}

	private void CheckLatch()
	{
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		if (_state != State.Latched)
		{
			return;
		}
		int num = 80;
		if (!_latchedProjectile.active || _latchedProjectile.type != _latchedProjectileType || Position.Distance(_latchedProjectile.Center) > (float)num)
		{
			RemoveLatch();
		}
		else if (_latchedProjectile.ai[0] == 0f && _latchedProjectile.ai[1] >= 0f)
		{
			RemoveLatch();
		}
		else if (_latchedProjectile.ai[0] == 1f)
		{
			RemoveLatch();
			if (_latchedProjectile.ai[1] == (float)_itemInstance.type)
			{
				ShouldBeRemovedFromRenderer = true;
			}
		}
	}

	private void RemoveLatch()
	{
		_state = State.FreeRoaming;
		PickNewVelocity();
	}

	private void EmitWaterPulse()
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		if (Main.netMode != 2)
		{
			Vector2 position = Position - new Vector2(4f, 4f);
			int width = 8;
			int height = 8;
			bool lavaWet = Collision.LavaCollision(position, width, height);
			Collision.WetCollision(position, width, height);
			DoStandardWaterSplash(Position, Collision.shimmer, Collision.honey, lavaWet);
			WaterShaderData waterShaderData = (WaterShaderData)Filters.Scene["WaterDistortion"].GetShader();
			float value = 1.4f;
			waterShaderData.QueueRipple(Position, new Color(0.5f, 0.1f * (float)Math.Sign(value) + 0.5f, 0f, 1f) * Math.Abs(value), new Vector2(4f, 4f), RippleShape.Circle);
		}
	}

	private void DoStandardWaterSplash(Vector2 castPosition, bool shimmerWet, bool honeyWet, bool lavaWet)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_0275: Unknown result type (might be due to invalid IL or missing references)
		//IL_027c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_038e: Unknown result type (might be due to invalid IL or missing references)
		//IL_039a: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_044f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0456: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02af: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02da: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0367: Unknown result type (might be due to invalid IL or missing references)
		//IL_036e: Unknown result type (might be due to invalid IL or missing references)
		//IL_011b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0120: Unknown result type (might be due to invalid IL or missing references)
		//IL_0175: Unknown result type (might be due to invalid IL or missing references)
		//IL_017c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0166: Unknown result type (might be due to invalid IL or missing references)
		//IL_016b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0143: Unknown result type (might be due to invalid IL or missing references)
		//IL_0148: Unknown result type (might be due to invalid IL or missing references)
		Vector2 val = castPosition - new Vector2(4f, 4f);
		int num = 8;
		int num2 = 8;
		if (shimmerWet)
		{
			for (int i = 0; i < 10; i++)
			{
				int num3 = Dust.NewDust(new Vector2(val.X - 6f, val.Y + (float)(num2 / 2) - 8f), num + 12, 24, 308);
				Main.dust[num3].velocity.Y -= 4f;
				Main.dust[num3].velocity.X *= 2.5f;
				Main.dust[num3].scale = 1.3f;
				Main.dust[num3].noGravity = true;
				switch (Main.rand.Next(6))
				{
				case 0:
					Main.dust[num3].color = new Color(255, 255, 210);
					break;
				case 1:
					Main.dust[num3].color = new Color(190, 245, 255);
					break;
				case 2:
					Main.dust[num3].color = new Color(255, 150, 255);
					break;
				default:
					Main.dust[num3].color = new Color(190, 175, 255);
					break;
				}
				SoundEngine.PlaySound(SoundID.FishSplash, (int)val.X, (int)val.Y, 5f);
			}
		}
		else if (honeyWet)
		{
			for (int j = 0; j < 10; j++)
			{
				int num4 = Dust.NewDust(new Vector2(val.X - 6f, val.Y + (float)(num2 / 2) - 8f), num + 12, 24, 152);
				Main.dust[num4].velocity.Y--;
				Main.dust[num4].velocity.X *= 2.5f;
				Main.dust[num4].scale = 1.3f;
				Main.dust[num4].alpha = 100;
				Main.dust[num4].noGravity = true;
			}
			SoundEngine.PlaySound(SoundID.FishSplash, (int)val.X, (int)val.Y, 1f);
		}
		else if (lavaWet)
		{
			for (int k = 0; k < 10; k++)
			{
				int num5 = Dust.NewDust(new Vector2(val.X - 6f, val.Y + (float)(num2 / 2) - 8f), num + 12, 24, 35);
				Main.dust[num5].velocity.Y -= 1.5f;
				Main.dust[num5].velocity.X *= 2.5f;
				Main.dust[num5].scale = 1.3f;
				Main.dust[num5].alpha = 100;
				Main.dust[num5].noGravity = true;
			}
			SoundEngine.PlaySound(SoundID.FishSplash, (int)val.X, (int)val.Y, 1f);
		}
		else
		{
			for (int l = 0; l < 10; l++)
			{
				int num6 = Dust.NewDust(new Vector2(val.X - 6f, val.Y + (float)(num2 / 2)), num + 12, 24, Dust.dustWater());
				Main.dust[num6].velocity.Y -= 4f;
				Main.dust[num6].velocity.X *= 2.5f;
				Main.dust[num6].scale = 1.3f;
				Main.dust[num6].alpha = 100;
				Main.dust[num6].noGravity = true;
			}
			SoundEngine.PlaySound(SoundID.FishSplash, (int)val.X, (int)val.Y, 1f);
		}
	}

	public void Update(ref ParticleRendererSettings settings)
	{
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_0171: Unknown result type (might be due to invalid IL or missing references)
		//IL_017c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0186: Unknown result type (might be due to invalid IL or missing references)
		//IL_0190: Unknown result type (might be due to invalid IL or missing references)
		//IL_0195: Unknown result type (might be due to invalid IL or missing references)
		//IL_019c: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01db: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0283: Unknown result type (might be due to invalid IL or missing references)
		//IL_0288: Unknown result type (might be due to invalid IL or missing references)
		//IL_028a: Unknown result type (might be due to invalid IL or missing references)
		//IL_028c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0296: Unknown result type (might be due to invalid IL or missing references)
		//IL_029b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0203: Unknown result type (might be due to invalid IL or missing references)
		//IL_020d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0212: Unknown result type (might be due to invalid IL or missing references)
		//IL_0242: Unknown result type (might be due to invalid IL or missing references)
		//IL_0248: Unknown result type (might be due to invalid IL or missing references)
		//IL_024d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0252: Unknown result type (might be due to invalid IL or missing references)
		//IL_0257: Unknown result type (might be due to invalid IL or missing references)
		//IL_025e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0263: Unknown result type (might be due to invalid IL or missing references)
		//IL_0333: Unknown result type (might be due to invalid IL or missing references)
		//IL_0338: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0304: Unknown result type (might be due to invalid IL or missing references)
		//IL_0309: Unknown result type (might be due to invalid IL or missing references)
		//IL_0396: Unknown result type (might be due to invalid IL or missing references)
		//IL_039b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0345: Unknown result type (might be due to invalid IL or missing references)
		//IL_034b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0350: Unknown result type (might be due to invalid IL or missing references)
		//IL_0355: Unknown result type (might be due to invalid IL or missing references)
		//IL_035f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0364: Unknown result type (might be due to invalid IL or missing references)
		//IL_0381: Unknown result type (might be due to invalid IL or missing references)
		//IL_038b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0390: Unknown result type (might be due to invalid IL or missing references)
		//IL_03bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_03cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_03cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_03eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_040f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0419: Unknown result type (might be due to invalid IL or missing references)
		//IL_041e: Unknown result type (might be due to invalid IL or missing references)
		if (_delayTime-- >= 0)
		{
			return;
		}
		if (++_lifeTimeCounted >= _lifeTimeTotal)
		{
			ShouldBeRemovedFromRenderer = true;
		}
		if (--_sonarTimeleft < 0)
		{
			_sonarTimeleft = 0;
		}
		bool flag = Collision.WetCollision(Position, 2, 2);
		if (flag != _wasWet)
		{
			EmitWaterPulse();
		}
		_wasWet = flag;
		if (_state == State.Jumping)
		{
			Velocity.Y += 0.2f;
			Rotation = Velocity.ToRotation();
			Position += Velocity;
			if (--_jumpTimeLeft > 0)
			{
				return;
			}
			_state = State.FreeRoaming;
			PickNewVelocity();
			Velocity *= 0.15f;
		}
		if ((float)_lifeTimeCounted / (float)_lifeTimeTotal >= 0.15f && Velocity.Length() < 0.1f && --_waitTime <= 0)
		{
			_waitTime = Main.rand.Next(30, 121);
			PickNewVelocity();
			_steerTime = Main.rand.Next(30, 121);
		}
		if (--_steerTime > 0)
		{
			Velocity = Vector2.Lerp(Velocity, Rotation.ToRotationVector2() * 1f, 1f / 30f);
			Velocity = Vector2.Lerp(Velocity, _targetVelocity, 0.05f);
		}
		else if (Velocity.Length() > 0.3f)
		{
			Velocity *= 0.975f;
		}
		if (_state == State.InterestedInBobber && Position.Distance(_bobberLocation) < 16f)
		{
			Velocity *= 0.9f;
			if (Velocity.Length() < 0.02f)
			{
				float num = 0.3f + 1.4f * Main.rand.NextFloat();
				Velocity = (_bobberLocation - Position).SafeNormalize(Vector2.Zero) * (0f - num);
			}
		}
		CheckLatch();
		if (_state == State.Latched)
		{
			SetAndGetLatchDetails(out var idealBobberPosition, out var idealOffset);
			Position = Vector2.Lerp(Position, idealBobberPosition - idealOffset, 0.05f);
		}
		Position += Velocity;
		Rotate();
		int num2 = 20;
		if (_lifeTimeTotal - _lifeTimeCounted > num2 && !flag)
		{
			_lifeTimeCounted = _lifeTimeTotal - num2;
		}
		if (Velocity.Y < 0f && !Collision.WetCollision(Position + new Vector2(0f, -20f), 0, 0))
		{
			_steerTime = 0;
			Velocity.Y *= 0.92f;
		}
		if (Velocity != Vector2.Zero && !Collision.WetCollision(Position + Velocity.SafeNormalize(Vector2.Zero) * 32f, 0, 0))
		{
			_steerTime = 0;
			PickNewVelocity();
			Velocity *= 0.92f;
		}
		if (!(Velocity != Vector2.Zero))
		{
			return;
		}
		for (float num3 = 0f; num3 < 1f; num3 += 0.25f)
		{
			Vector2 val = ((float)Math.PI * 2f * num3).ToRotationVector2() * 16f;
			if (!(Vector2.Dot(val, Velocity.SafeNormalize(Vector2.UnitY)) < 0f) && !Collision.WetCollision(Position + val, 0, 0))
			{
				_steerTime = 0;
				PickNewVelocity();
				Velocity *= 0.92f;
				break;
			}
		}
	}

	private void SetAndGetLatchDetails(out Vector2 idealBobberPosition, out Vector2 idealOffset)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		idealBobberPosition = _latchedProjectile.Center + new Vector2(0f, 8f) + new Vector2(8f, 5f);
		Vector2 val = idealBobberPosition - Position;
		idealOffset = val.SafeNormalize(-Vector2.UnitY) * 6f;
		if (idealOffset.Y > 0f)
		{
			idealOffset.Y *= -1f;
		}
		idealOffset.Y -= 3f;
		idealOffset += new Vector2((float)(Math.Sign(val.X) * -4), 0f);
		_targetVelocity = idealOffset;
		_bobberLocation = idealBobberPosition;
		int num = (int)((float)_lifeTimeTotal * 0.85f);
		if (_lifeTimeCounted > num)
		{
			_lifeTimeCounted = num;
		}
	}

	private void Rotate(bool instant = false)
	{
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		float num = 0.1f;
		if (_isInanimate)
		{
			num *= 0.05f;
		}
		if (instant)
		{
			num = (float)Math.PI * 2f;
		}
		if (_state == State.InterestedInBobber || _state == State.Latched)
		{
			Rotation = Utils.rotateTowards(Position, Rotation.ToRotationVector2(), _bobberLocation, num).ToRotation();
		}
		else if (Velocity != Vector2.Zero)
		{
			Rotation = Utils.rotateTowards(Vector2.Zero, Rotation.ToRotationVector2(), Velocity, num).ToRotation();
		}
	}

	private void PickNewVelocity()
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		_targetVelocity = Main.rand.NextVector2Circular(1.5f, 0.6f);
		if (_state == State.InterestedInBobber)
		{
			_targetVelocity = (_bobberLocation - Position).SafeNormalize(-Vector2.UnitY) * (0.7f + 0.5f * Main.rand.NextFloat());
		}
		if (_state == State.Latched)
		{
			_targetVelocity = (_latchedProjectile.Center - Position).SafeNormalize(-Vector2.UnitY) * 0.7f;
		}
	}

	public void Prepare(int itemType, int lifeTimeTotal, Vector2 bobberLocation)
	{
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		_itemInstance.SetDefaults(itemType);
		_totalScale = 0.6f + 0.15f * Main.rand.NextFloat();
		_bobberLocation = bobberLocation + new Vector2(0f, 8f);
		_isInanimate = false;
		_lifeTimeTotal = lifeTimeTotal;
		_state = ((Main.rand.Next(3) != 0) ? State.InterestedInBobber : State.FreeRoaming);
		_itemLooksDiagonal = ItemID.Sets.ReceivesDiagonalCorrectionAsFakeFish[itemType];
		if (ItemID.Sets.IsFishingCrate[itemType] || ItemID.Sets.FakeFishInanimate[itemType])
		{
			_state = State.FreeRoaming;
			_isInanimate = true;
			_itemLooksDiagonal = false;
		}
		PickNewVelocity();
		_steerTime = Main.rand.Next(30, 121);
		_wasWet = true;
		_delayTime = 0;
	}

	public void TryJumping()
	{
		if (!_isInanimate)
		{
			int num = (int)(Velocity.Y / -0.2f);
			_jumpTimeLeft = num * 2;
			_state = State.Jumping;
			_delayTime = Main.rand.Next(0, 11);
		}
	}

	public void Draw(ref ParticleRendererSettings settings, SpriteBatch spritebatch)
	{
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_011b: Unknown result type (might be due to invalid IL or missing references)
		//IL_011e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0123: Unknown result type (might be due to invalid IL or missing references)
		//IL_0151: Unknown result type (might be due to invalid IL or missing references)
		//IL_018b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0190: Unknown result type (might be due to invalid IL or missing references)
		//IL_019a: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0160: Unknown result type (might be due to invalid IL or missing references)
		//IL_0137: Unknown result type (might be due to invalid IL or missing references)
		//IL_013c: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_02de: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_014a: Unknown result type (might be due to invalid IL or missing references)
		//IL_014f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0320: Unknown result type (might be due to invalid IL or missing references)
		//IL_0321: Unknown result type (might be due to invalid IL or missing references)
		//IL_0328: Unknown result type (might be due to invalid IL or missing references)
		//IL_032d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0337: Unknown result type (might be due to invalid IL or missing references)
		//IL_033d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0347: Unknown result type (might be due to invalid IL or missing references)
		//IL_034e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0350: Unknown result type (might be due to invalid IL or missing references)
		//IL_035a: Unknown result type (might be due to invalid IL or missing references)
		//IL_035f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0361: Unknown result type (might be due to invalid IL or missing references)
		//IL_0226: Unknown result type (might be due to invalid IL or missing references)
		//IL_0228: Unknown result type (might be due to invalid IL or missing references)
		//IL_0241: Unknown result type (might be due to invalid IL or missing references)
		//IL_0246: Unknown result type (might be due to invalid IL or missing references)
		//IL_0249: Unknown result type (might be due to invalid IL or missing references)
		//IL_0250: Unknown result type (might be due to invalid IL or missing references)
		//IL_0256: Unknown result type (might be due to invalid IL or missing references)
		//IL_025b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0269: Unknown result type (might be due to invalid IL or missing references)
		//IL_026f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0279: Unknown result type (might be due to invalid IL or missing references)
		//IL_027e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0280: Unknown result type (might be due to invalid IL or missing references)
		//IL_0285: Unknown result type (might be due to invalid IL or missing references)
		//IL_028a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0291: Unknown result type (might be due to invalid IL or missing references)
		//IL_0295: Unknown result type (might be due to invalid IL or missing references)
		//IL_0297: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a8: Unknown result type (might be due to invalid IL or missing references)
		Main.instance.LoadItem(_itemInstance.type);
		float num = (float)_lifeTimeCounted / (float)_lifeTimeTotal;
		float toMin = Utils.Remap(num, 0.1f, 0.5f, 0f, 0.85f);
		toMin = Utils.Remap(num, 0.5f, 0.9f, toMin, 1f);
		Vector2 val = settings.AnchorPosition + Position;
		float num2 = Utils.Remap(num, 0f, 0.25f, 0f, 1f) * Utils.Remap(num, 0.85f, 1f, 1f, 0f);
		Vector2 val2 = new Vector2(_itemInstance.scale * _totalScale);
		Vector2 val3 = ((float)Math.PI * 2f * num * 6f).ToRotationVector2();
		val2.X += val3.Y * 0.014f;
		val2.Y += val3.X * 0.012f;
		float num3 = Rotation;
		if (_itemLooksDiagonal)
		{
			num3 += (float)Math.PI / 4f;
		}
		SpriteEffects val4 = (SpriteEffects)0;
		Vector2 val5 = Velocity;
		if (_itemLooksDiagonal)
		{
			if (_state == State.InterestedInBobber)
			{
				val5 = _targetVelocity;
			}
			else if (_state == State.Latched)
			{
				val5 = _targetVelocity;
			}
		}
		if (val5.X < 0f)
		{
			val4 = (SpriteEffects)2;
			if (_itemLooksDiagonal)
			{
				num3 -= (float)Math.PI / 2f;
			}
		}
		Main.instance.DrawItem_GetBasics(_itemInstance, 0, out var texture, out var frame, out var glowmaskFrame);
		Color val6 = Color.Lerp(Color.White, Color.Black, 0.8f) * 0.7f;
		if (_sonarStartTime > 0 && _sonarTimeleft <= _sonarStartTime && _sonarTimeleft > 0)
		{
			float fromValue = 1f - (float)_sonarTimeleft / (float)_sonarStartTime;
			float num4 = Utils.Remap(fromValue, 0f, 0.2f, 0f, 1f) * Utils.Remap(fromValue, 0.2f, 1f, 1f, 0f);
			val6 = Color.Lerp(val6, Color.White, _isSelectedSonarType ? num4 : (num4 * 0.4f));
			Color val7 = _sonarColor * num4 * num2;
			for (float num5 = 0f; num5 < (float)Math.PI * 2f; num5 += (float)Math.PI / 2f)
			{
				spritebatch.Draw(texture, val + (num5 + num3).ToRotationVector2() * 2f * val2, (Rectangle?)frame, val7, num3, frame.Size() / 2f, val2, val4, 0f);
			}
		}
		spritebatch.Draw(texture, val, (Rectangle?)frame, val6 * num2, num3, frame.Size() / 2f, val2, val4, 0f);
		if (_itemInstance.glowMask != -1)
		{
			spritebatch.Draw(TextureAssets.GlowMask[_itemInstance.glowMask].Value, val, (Rectangle?)glowmaskFrame, Color.Lerp(Color.White, Color.Black, 0.8f) * num2 * 0.7f, num3, glowmaskFrame.Size() / 2f, val2, val4, 0f);
		}
	}

	public void RestInPool()
	{
		IsRestingInPool = true;
	}

	public virtual void FetchFromPool()
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		_lifeTimeCounted = 0;
		_lifeTimeTotal = 0;
		IsRestingInPool = false;
		ShouldBeRemovedFromRenderer = false;
		Position = (Velocity = Vector2.Zero);
		Rotation = 0f;
		_gotRepelled = false;
	}

	public void BeRepelled(ref ParticleRepelDetails details)
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0104: Unknown result type (might be due to invalid IL or missing references)
		//IL_010a: Unknown result type (might be due to invalid IL or missing references)
		//IL_010f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0114: Unknown result type (might be due to invalid IL or missing references)
		//IL_011e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0123: Unknown result type (might be due to invalid IL or missing references)
		//IL_0140: Unknown result type (might be due to invalid IL or missing references)
		//IL_014a: Unknown result type (might be due to invalid IL or missing references)
		//IL_014f: Unknown result type (might be due to invalid IL or missing references)
		if (_isInanimate || _gotRepelled || _state == State.Jumping || _state == State.Latched)
		{
			return;
		}
		float num = Position.Distance(details.SourcePosition) - details.Radius;
		if (num >= 100f)
		{
			return;
		}
		float toMin = 2f;
		if (!_gotRepelled || Main.rand.Next(10) == 0)
		{
			float num2 = Utils.Remap(num, 100f, 0f, toMin, 5f);
			Velocity = Position.DirectionFrom(details.SourcePosition).SafeNormalize(Velocity.ToRotation().ToRotationVector2()).RotatedByRandom(0.7853981852531433) * num2;
			Rotation = Velocity.ToRotation();
			Position -= Velocity;
			_targetVelocity = Velocity;
			if (Velocity != Vector2.Zero && !Collision.WetCollision(Position + Velocity.SafeNormalize(Vector2.Zero) * 32f, 0, 0))
			{
				_steerTime = 0;
				PickNewVelocity();
				Velocity = _targetVelocity * 0.1f;
			}
		}
		_delayTime = -1;
		_steerTime = Main.rand.Next(30, 121);
		_state = State.FreeRoaming;
		_gotRepelled = true;
		if ((float)_lifeTimeCounted < (float)_lifeTimeTotal * 0.7f)
		{
			float num3 = Utils.Remap((float)_lifeTimeCounted / (float)_lifeTimeTotal, 0f, 0.25f, 0f, 1f);
			float num4 = MathHelper.Lerp(1f, 0.85f, num3);
			if (num3 == 1f)
			{
				num4 = 0.7f;
			}
			_lifeTimeCounted = (int)((float)_lifeTimeTotal * num4);
		}
	}
}
