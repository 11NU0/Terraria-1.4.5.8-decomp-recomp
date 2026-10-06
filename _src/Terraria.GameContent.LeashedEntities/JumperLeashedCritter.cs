using System;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;

namespace Terraria.GameContent.LeashedEntities;

public class JumperLeashedCritter : LeashedCritter
{
	public static JumperLeashedCritter Prototype = new JumperLeashedCritter();

	private const int State_Normal = 0;

	private const int State_Recalling = 1;

	protected int minWaitTime;

	protected int maxWaitTime;

	protected float maxJumpWidth;

	protected float minJumpWidth;

	protected float maxJumpHeight;

	protected float maxJumpDuration;

	protected int jumpCooldown;

	protected bool canStandOnWater;

	protected static readonly float buoyancy = 0.4f;

	public JumperLeashedCritter()
	{
		strayingRangeInBlocksX = (strayingRangeInBlocksY = 12);
		minWaitTime = 180;
		maxWaitTime = 300;
		maxJumpWidth = 112f;
		minJumpWidth = 48f;
		maxJumpHeight = 64f;
		maxJumpDuration = 30f;
		jumpCooldown = 60;
		canStandOnWater = false;
	}

	public override void Spawn(bool newlyAdded)
	{
		base.Spawn(newlyAdded);
		PickNewTarget();
		SetJumpCooldown();
	}

	public override void Update()
	{
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
		base.Update();
		WaitTime--;
		if (WaitTime <= 0)
		{
			switch (State)
			{
			case 0:
				if (!TryStartJump())
				{
					PickNewTarget();
					SetJumpCooldown();
				}
				break;
			case 1:
				Recall();
				PickNewTarget();
				SetJumpCooldown();
				State = 0;
				break;
			}
		}
		Move(out var hitSomething);
		if (hitSomething && State != 1)
		{
			PickNewTarget();
			SetJumpCooldown();
		}
		Vector2 val = TargetPosition.ToWorldCoordinates() - Center;
		if (val.Length() < 8f && CanStandOnTile(TargetPosition))
		{
			Center = TargetPosition.ToWorldCoordinates();
			velocity = Vector2.Zero;
			PickNewTarget();
			SetJumpCooldown();
		}
		spriteDirection = direction;
		if (Main.netMode != 2)
		{
			VisualEffects();
		}
		CopyToDummy();
		LeashedCritter._dummy.FindFrame();
		CopyFromDummy();
	}

	private void SetJumpCooldown()
	{
		WaitTime = (short)rand.Next(minWaitTime, maxWaitTime + 1);
	}

	private bool TryStartJump()
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		Vector2 val = TargetPosition.ToWorldCoordinates() - Center;
		if (val.Y * -1f > maxJumpHeight)
		{
			return false;
		}
		float num = Math.Min(Math.Abs(val.X), maxJumpWidth);
		if (num <= minJumpWidth)
		{
			return false;
		}
		float num2 = num / maxJumpWidth;
		float num3 = maxJumpDuration * num2;
		float num4 = val.Y * num2 / num3 - 0.5f * LeashedCritter.gravity * num3;
		if (!(num4 < LeashedCritter.gravity * -1f))
		{
			return false;
		}
		direction = Math.Sign(val.X);
		velocity.X = num / num3 * (float)direction;
		velocity.Y = num4;
		WaitTime = (short)(num3 + (float)jumpCooldown);
		return true;
	}

	private void Move(out bool hitSomething)
	{
		//IL_0004: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0108: Unknown result type (might be due to invalid IL or missing references)
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		//IL_0117: Unknown result type (might be due to invalid IL or missing references)
		//IL_011e: Unknown result type (might be due to invalid IL or missing references)
		//IL_015d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0162: Unknown result type (might be due to invalid IL or missing references)
		hitSomething = false;
		Point val = Center.ToTileCoordinates();
		int num = Math.Sign((int)velocity.X);
		if (num != 0)
		{
			direction = num;
		}
		int num2 = Math.Sign((int)velocity.Y);
		Vector2 val2 = new Vector2((float)num, (float)num2) * Size * 0.5f;
		Vector2 val3 = Center + val2 + velocity;
		Point val4 = val3.ToTileCoordinates();
		if (WorldGen.AnyLiquidAt(val, 0))
		{
			velocity.Y -= buoyancy;
			if (State != 1)
			{
				WaitTime = (short)minWaitTime;
			}
		}
		if (!WorldGen.SolidTileAllowPlatformTopFrame(val4.X, val4.Y))
		{
			Move_NoObstruction(val, val3.Y);
			return;
		}
		hitSomething = true;
		bool flag = false;
		if (num2 != 0)
		{
			Point val5 = val;
			val5.Y += num2;
			flag = WorldGen.SolidTileAllowPlatformTopFrame(val5.X, val5.Y);
		}
		bool flag2 = false;
		if (num != 0)
		{
			Point val6 = val;
			val6.X += num;
			flag2 = WorldGen.SolidTileNoPlatforms(val6.X, val6.Y);
		}
		if (flag)
		{
			velocity.Y = 0f;
		}
		if (flag2)
		{
			velocity.X = 0f;
		}
		if (!flag && !flag2)
		{
			velocity = Vector2.Zero;
		}
	}

	private void Move_NoObstruction(Point currentTile, float nextY)
	{
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		if (velocity.Y >= 0f && nextY % 16f >= 8f)
		{
			Point tile = currentTile;
			tile.Y++;
			if (CanStandOnTile(tile))
			{
				Center = currentTile.ToWorldCoordinates();
				velocity = Vector2.Zero;
				return;
			}
		}
		Center += velocity;
		velocity.Y = MathHelper.Clamp(velocity.Y + LeashedCritter.gravity, 0f - LeashedCritter.maxFallSpeed, LeashedCritter.maxFallSpeed);
		if (State != 1 && Math.Abs(currentTile.Y - AnchorPosition.Y) > strayingRangeInBlocksY)
		{
			State = 1;
			WaitTime = 20;
		}
	}

	private void PickNewTarget()
	{
		int num = (int)(maxJumpWidth / 16f);
		int num2 = (int)(minJumpWidth / 16f);
		int num3 = TargetPosition.X - (AnchorPosition.X - strayingRangeInBlocksX);
		int num4 = AnchorPosition.X + strayingRangeInBlocksX - TargetPosition.X;
		bool flag = num3 >= num2;
		bool flag2 = num4 >= num2;
		if (flag || flag2)
		{
			int num5 = ((!(flag & flag2)) ? ((!flag) ? 1 : (-1)) : (rand.Next(2) * 2 - 1));
			int num6 = ((num5 < 1) ? num3 : num4);
			int num7 = rand.Next(1, num6 / num + 1);
			int num8 = num6 % num;
			if (num8 < num2)
			{
				num8 = 0;
			}
			int startX = TargetPosition.X + (num7 * num + num8) * num5;
			if (TryGetReachableTile(startX, out var tile))
			{
				TargetPosition = tile;
			}
		}
	}

	private bool TryGetReachableTile(int startX, out Point16 tile)
	{
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
		tile = Point16.Zero;
		int num = Math.Sign(AnchorPosition.X - startX);
		if (num == 0)
		{
			return false;
		}
		short num2 = (short)Math.Min(AnchorPosition.Y, (int)(Center.Y / 16f));
		for (int i = startX; i != AnchorPosition.X; i += num)
		{
			tile = new Point16(i, num2);
			if (WorldGen.SolidTileAllowPlatformTopFrame(tile.X, tile.Y) || WorldGen.AnyLiquidAt(tile, 0))
			{
				float num3 = maxJumpHeight / 16f;
				for (int j = 0; (float)j < num3; j++)
				{
					tile.Y--;
					if (!WorldGen.SolidTileAllowPlatformTopFrame(tile.X, tile.Y) && !WorldGen.AnyLiquidAt(tile, 0))
					{
						return true;
					}
				}
				continue;
			}
			int num4 = AnchorPosition.Y + strayingRangeInBlocksY;
			tile.Y = (short)(num2 + 1);
			while (tile.Y < num4)
			{
				if (CanStandOnTile(tile))
				{
					tile.Y--;
					return true;
				}
				tile.Y++;
			}
		}
		return false;
	}

	private bool CanStandOnTile(Point tile)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		if (WorldGen.SolidTileAllowPlatformTopFrame(tile.X, tile.Y))
		{
			return true;
		}
		if (canStandOnWater && WorldGen.AnyLiquidAt(tile, 0))
		{
			return !WorldGen.AnyLiquidAt(tile.X, tile.Y - 1, 0);
		}
		return false;
	}

	protected override void CopyToDummy()
	{
		base.CopyToDummy();
		if (State == 1)
		{
			LeashedCritter._dummy.Opacity = (float)WaitTime / 20f;
		}
	}

	public override Vector2 GetDrawOffset()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		Point16 point = Center.ToTileCoordinates16();
		if (Framing.GetTileSafely(point.X, point.Y + 1).halfBrick())
		{
			return new Vector2(0f, Center.Y % 16f);
		}
		return base.GetDrawOffset();
	}
}
