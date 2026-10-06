using System;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;

namespace Terraria.GameContent.LeashedEntities;

public class WalkerLeashedCritter : LeashedCritter
{
	public static WalkerLeashedCritter Prototype = new WalkerLeashedCritter();

	private const int State_Standing = 0;

	private const int State_PickDirection = 1;

	private const int State_Walking = 2;

	private const int State_Falling = 3;

	private const int State_Recalling = 4;

	protected float walkingPace;

	public WalkerLeashedCritter()
	{
		walkingPace = 0.8f;
		strayingRangeInBlocksX = (strayingRangeInBlocksY = 3);
	}

	protected bool AdvanceTargetPosition()
	{
		if (Math.Abs(TargetPosition.X - AnchorPosition.X) >= strayingRangeInBlocksX)
		{
			direction = Math.Sign(AnchorPosition.X - TargetPosition.X);
		}
		if (!WorldGen.InWorld(TargetPosition.X + direction, TargetPosition.Y))
		{
			direction *= -1;
		}
		spriteDirection = direction;
		int num = TargetPosition.X + direction;
		short y = TargetPosition.Y;
		bool flag = !WorldGen.SolidTileAllowTopSlope(num, y - 1);
		bool flag2 = !WorldGen.SolidTileNoPlatforms(num, y);
		bool flag3 = !WorldGen.SolidTileAllowBottomSlope(num, y + 1);
		bool flag4 = WorldGen.AnyLiquidAt(num, y + 1);
		bool flag5 = !WorldGen.SolidTileAllowBottomSlope(num, y + 2);
		bool flag6 = flag && !flag2;
		bool flag7 = (flag2 & flag3) && !flag4 && !flag5;
		bool flag8 = flag2 && !flag3;
		if (flag6)
		{
			TargetPosition = new Point16(num, y - 1);
		}
		else if (flag7)
		{
			TargetPosition = new Point16(num, y + 1);
		}
		else
		{
			if (!flag8)
			{
				return false;
			}
			TargetPosition = new Point16(num, y);
		}
		return true;
	}

	public override void Update()
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0105: Unknown result type (might be due to invalid IL or missing references)
		//IL_010a: Unknown result type (might be due to invalid IL or missing references)
		base.Update();
		Point16 tilePosition = Center.ToTileCoordinates16();
		HandleFalling(tilePosition);
		WaitTime--;
		if (WaitTime <= 0)
		{
			if (State == 4)
			{
				Recall();
			}
			WaitTime = (short)rand.Next(60, 61);
			State = (byte)rand.Next(2);
		}
		HandleWalking();
		int value = TargetPosition.X - tilePosition.X;
		int num = TargetPosition.Y - tilePosition.Y;
		if (Math.Abs(value) == 1 && Math.Abs(num) == 1)
		{
			velocity.Y = num * 2;
		}
		float maxAmountAllowedToMove = velocity.Length();
		Vector2 val = TargetPosition.ToWorldCoordinates();
		Center = Center.MoveTowards(val, maxAmountAllowedToMove);
		if (Center == val && State == 0)
		{
			velocity = Vector2.Zero;
		}
		if (Main.netMode != 2)
		{
			VisualEffects();
		}
		CopyToDummy();
		LeashedCritter._dummy.FindFrame();
		CopyFromDummy();
	}

	private void HandleFalling(Point16 tilePosition)
	{
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		if (WorldGen.SolidTileAllowPlatformTopFrame(tilePosition.X, tilePosition.Y + 1))
		{
			velocity.Y = 0f;
			if (State == 3 || State == 4)
			{
				Center = TargetPosition.ToWorldCoordinates();
			}
			if (State == 3)
			{
				State = 0;
				WaitTime = 0;
			}
			return;
		}
		velocity.Y += LeashedCritter.gravity;
		if (velocity.Y > LeashedCritter.maxFallSpeed)
		{
			velocity.Y = LeashedCritter.maxFallSpeed;
		}
		TargetPosition.X = tilePosition.X;
		TargetPosition.Y = (short)Math.Min(tilePosition.Y + 1, Main.maxTilesY - 1);
		if (State != 4)
		{
			if (TargetPosition.Y - AnchorPosition.Y > strayingRangeInBlocksY)
			{
				State = 4;
				WaitTime = 20;
			}
			else
			{
				State = 3;
			}
		}
	}

	private void HandleWalking()
	{
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		if (State == 3 || State == 4)
		{
			return;
		}
		velocity.X = walkingPace * (float)direction;
		if (State != 0 && !(Center.Distance(TargetPosition.ToWorldCoordinates()) >= 1f))
		{
			if (State == 1)
			{
				direction = rand.Next(2) * 2 - 1;
				State = 2;
			}
			if (!AdvanceTargetPosition())
			{
				WaitTime = 30;
				State = 0;
			}
		}
	}

	protected override void CopyToDummy()
	{
		base.CopyToDummy();
		if (State == 4)
		{
			LeashedCritter._dummy.Opacity = (float)WaitTime / 20f;
		}
	}

	public override Vector2 GetDrawOffset()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		Point16 point = Center.ToTileCoordinates16();
		if (Framing.GetTileSafely(point.X, point.Y + 1).halfBrick())
		{
			return new Vector2(0f, 8f);
		}
		return base.GetDrawOffset();
	}
}
