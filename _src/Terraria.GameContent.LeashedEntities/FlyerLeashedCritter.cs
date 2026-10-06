using System;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;

namespace Terraria.GameContent.LeashedEntities;

public class FlyerLeashedCritter : LeashedCritter
{
	public static FlyerLeashedCritter Prototype = new FlyerLeashedCritter();

	protected int minWaitTime;

	protected int maxWaitTime;

	protected float maxFlySpeed;

	protected float acceleration;

	protected int brakeDuration;

	protected float rotationScalar;

	protected float hoverAmplitude;

	protected float hoverPeriod;

	protected bool hasGroundBias;

	private const float HoverYVelocity = 0.0001f;

	public FlyerLeashedCritter()
	{
		anchorStyle = 4;
		strayingRangeInBlocksX = (strayingRangeInBlocksY = 7);
		minWaitTime = 60;
		maxWaitTime = 300;
		maxFlySpeed = 1f;
		acceleration = 0.2f;
		brakeDuration = 10;
	}

	public override void Spawn(bool newlyAdded)
	{
		base.Spawn(newlyAdded);
		if (!WorldGen.SolidTileAllowPlatformTopFrame(AnchorPosition.X, AnchorPosition.Y + 1))
		{
			velocity.Y = 0.0001f;
		}
		PickNewTarget();
	}

	protected void PickNewTarget()
	{
		bool num = hasGroundBias && AnchorPosition.Y == TargetPosition.Y && rand.Next(4) != 0;
		TargetPosition = new Point16(AnchorPosition.X + rand.Next(-strayingRangeInBlocksX, strayingRangeInBlocksX + 1), AnchorPosition.Y + rand.Next(-strayingRangeInBlocksY, 1));
		if (num)
		{
			TargetPosition.Y = AnchorPosition.Y;
		}
	}

	protected override void CopyToDummy()
	{
		base.CopyToDummy();
		if (velocity.Y != 0f)
		{
			LeashedCritter._dummy.rotation = velocity.X * rotationScalar;
		}
	}

	public override void Update()
	{
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0102: Unknown result type (might be due to invalid IL or missing references)
		//IL_0107: Unknown result type (might be due to invalid IL or missing references)
		//IL_0124: Unknown result type (might be due to invalid IL or missing references)
		//IL_012a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0134: Unknown result type (might be due to invalid IL or missing references)
		//IL_0139: Unknown result type (might be due to invalid IL or missing references)
		//IL_013b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0140: Unknown result type (might be due to invalid IL or missing references)
		//IL_0146: Unknown result type (might be due to invalid IL or missing references)
		//IL_014b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0150: Unknown result type (might be due to invalid IL or missing references)
		//IL_0155: Unknown result type (might be due to invalid IL or missing references)
		//IL_0157: Unknown result type (might be due to invalid IL or missing references)
		//IL_015e: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01de: Unknown result type (might be due to invalid IL or missing references)
		//IL_016d: Unknown result type (might be due to invalid IL or missing references)
		//IL_016e: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0200: Unknown result type (might be due to invalid IL or missing references)
		//IL_0186: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01af: Unknown result type (might be due to invalid IL or missing references)
		base.Update();
		WaitTime--;
		if (WaitTime <= 0)
		{
			WaitTime = (short)rand.Next(minWaitTime, maxWaitTime + 1);
			PickNewTarget();
		}
		Point val = Center.ToTileCoordinates();
		Vector2 val2 = TargetPosition.ToWorldCoordinates();
		Vector2 val3 = val2 - Center;
		float num = val3.Length();
		Vector2 val4 = val3 / num;
		if (val4.HasNaNs())
		{
			val4 = Vector2.Zero;
		}
		velocity += val4 * acceleration;
		float num2 = velocity.Length();
		float val5 = Math.Min(1f, num / ((float)brakeDuration * maxFlySpeed));
		float num3 = maxFlySpeed * Math.Max(val5, 0.25f);
		if (num2 > num3)
		{
			velocity *= num3 / num2;
			num2 = num3;
		}
		bool flag = num < maxFlySpeed;
		bool flag2 = flag;
		if (!flag2)
		{
			Point val6 = (Center + Size * 0.5f * val4 + velocity).ToTileCoordinates();
			flag2 = WorldGen.SolidTileAllowPlatformTopFrame(val6.X, val6.Y) || IsBlockedByMedium(val, val6);
		}
		if (flag2)
		{
			bool flag3 = false;
			if (flag)
			{
				Center = val2;
				flag3 = true;
			}
			velocity.X = 0f;
			velocity.Y = ((flag3 && WorldGen.SolidTileAllowPlatformTopFrame(val.X, val.Y + 1)) ? 0f : 0.0001f);
		}
		else
		{
			Center += velocity;
			if (velocity.Y == 0f && !WorldGen.SolidTileAllowPlatformTopFrame(val.X, val.Y + 1))
			{
				velocity.Y = 0.0001f;
			}
		}
		int num4 = Math.Sign(velocity.X);
		if (num4 != 0 && num4 != direction)
		{
			direction = num4;
			spriteDirection = -direction;
		}
		if (Main.netMode != 2)
		{
			VisualEffects();
		}
		CopyToDummy();
		LeashedCritter._dummy.FindFrame();
		CopyFromDummy();
	}

	private bool IsBlockedByMedium(Point origin, Point destination)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		if (origin == destination)
		{
			return false;
		}
		int num = (isAquatic ? 255 : 0);
		bool flag = IsLiquidBoundary(origin, out var liquidAmount, out var liquidType);
		bool flag2 = liquidType > 0;
		bool flag3 = liquidAmount == num;
		if (!((!flag && !flag2) & flag3))
		{
			return false;
		}
		bool flag4 = IsLiquidBoundary(destination, out var liquidAmount2, out var liquidType2);
		bool flag5 = liquidType2 > 0;
		bool flag6 = liquidAmount2 == num;
		return !((!flag4 && !flag5) & flag6);
	}

	private bool IsLiquidBoundary(Point coordinates, out int liquidAmount, out int liquidType)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		Tile tileSafely = Framing.GetTileSafely(coordinates);
		liquidAmount = tileSafely.liquid;
		liquidType = ((tileSafely.liquid > 0) ? tileSafely.liquidType() : (-1));
		if (tileSafely.nactive() && Main.tileSolid[tileSafely.type] && !Main.tileSolidTop[tileSafely.type])
		{
			return false;
		}
		if (liquidAmount > 0 && liquidAmount < 255)
		{
			return true;
		}
		if (liquidAmount == 0)
		{
			coordinates.Y++;
			Tile tileSafely2 = Framing.GetTileSafely(coordinates);
			if (tileSafely2.liquid == byte.MaxValue)
			{
				if (tileSafely2.nactive() && Main.tileSolid[tileSafely2.type])
				{
					return Main.tileSolidTop[tileSafely2.type];
				}
				return true;
			}
			return false;
		}
		coordinates.Y--;
		Tile tileSafely3 = Framing.GetTileSafely(coordinates);
		if (tileSafely3.liquid == 0)
		{
			if (tileSafely3.nactive() && Main.tileSolid[tileSafely3.type])
			{
				return Main.tileSolidTop[tileSafely3.type];
			}
			return true;
		}
		return false;
	}

	public override Vector2 GetDrawOffset()
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		if (velocity.Y == 0f)
		{
			Point16 point = Center.ToTileCoordinates16();
			if (Framing.GetTileSafely(point.X, point.Y + 1).halfBrick())
			{
				return new Vector2(0f, 8f);
			}
			return Vector2.Zero;
		}
		if (hoverPeriod == 0f || hoverAmplitude == 0f)
		{
			return Vector2.Zero;
		}
		return GetBobbingOffset();
	}

	protected Vector2 GetBobbingOffset()
	{
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		double num = Main.timeForVisualEffects + (double)(whoAmI * npcType);
		num *= (double)(hoverPeriod * ((float)Math.PI * 2f));
		return new Vector2(0f, (float)Math.Sin(num) * hoverAmplitude);
	}
}
