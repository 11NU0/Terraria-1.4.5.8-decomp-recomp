using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;

namespace Terraria.Physics;

public static class BallCollision
{
	[Flags]
	private enum TileEdges : uint
	{
		None = 0u,
		Top = 1u,
		Bottom = 2u,
		Left = 4u,
		Right = 8u,
		TopLeftSlope = 0x10u,
		TopRightSlope = 0x20u,
		BottomLeftSlope = 0x40u,
		BottomRightSlope = 0x80u
	}

	public static BallStepResult Step(PhysicsProperties physicsProperties, Entity entity, ref float entityAngularVelocity, IBallContactListener listener)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0208: Unknown result type (might be due to invalid IL or missing references)
		//IL_020b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0210: Unknown result type (might be due to invalid IL or missing references)
		//IL_0132: Unknown result type (might be due to invalid IL or missing references)
		//IL_0133: Unknown result type (might be due to invalid IL or missing references)
		//IL_0134: Unknown result type (might be due to invalid IL or missing references)
		//IL_0139: Unknown result type (might be due to invalid IL or missing references)
		//IL_013a: Unknown result type (might be due to invalid IL or missing references)
		//IL_013b: Unknown result type (might be due to invalid IL or missing references)
		//IL_025f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0260: Unknown result type (might be due to invalid IL or missing references)
		//IL_0266: Unknown result type (might be due to invalid IL or missing references)
		//IL_0267: Unknown result type (might be due to invalid IL or missing references)
		//IL_0221: Unknown result type (might be due to invalid IL or missing references)
		//IL_0149: Unknown result type (might be due to invalid IL or missing references)
		//IL_014a: Unknown result type (might be due to invalid IL or missing references)
		//IL_014b: Unknown result type (might be due to invalid IL or missing references)
		//IL_022e: Unknown result type (might be due to invalid IL or missing references)
		//IL_015a: Unknown result type (might be due to invalid IL or missing references)
		//IL_015b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0161: Unknown result type (might be due to invalid IL or missing references)
		//IL_0166: Unknown result type (might be due to invalid IL or missing references)
		//IL_016b: Unknown result type (might be due to invalid IL or missing references)
		//IL_016d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0172: Unknown result type (might be due to invalid IL or missing references)
		//IL_0177: Unknown result type (might be due to invalid IL or missing references)
		//IL_0179: Unknown result type (might be due to invalid IL or missing references)
		//IL_017b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0185: Unknown result type (might be due to invalid IL or missing references)
		//IL_018a: Unknown result type (might be due to invalid IL or missing references)
		//IL_018f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0195: Unknown result type (might be due to invalid IL or missing references)
		//IL_019a: Unknown result type (might be due to invalid IL or missing references)
		//IL_019f: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01da: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_023b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0102: Unknown result type (might be due to invalid IL or missing references)
		//IL_0107: Unknown result type (might be due to invalid IL or missing references)
		//IL_0248: Unknown result type (might be due to invalid IL or missing references)
		Vector2 position = entity.position;
		Vector2 velocity = entity.velocity;
		Vector2 size = entity.Size;
		float num = entityAngularVelocity;
		float num2 = size.X * 0.5f;
		num *= physicsProperties.Drag;
		velocity *= physicsProperties.Drag;
		float num3 = velocity.Length();
		if (num3 > 1000f)
		{
			velocity = 1000f * Vector2.Normalize(velocity);
			num3 = 1000f;
		}
		int num4 = Math.Max(1, (int)Math.Ceiling(num3 / 2f));
		float num5 = 1f / (float)num4;
		velocity *= num5;
		num *= num5;
		float num6 = physicsProperties.Gravity / (float)(num4 * num4);
		bool flag = false;
		for (int i = 0; i < num4; i++)
		{
			velocity.Y += num6;
			if (CheckForPassThrough(position + size * 0.5f, out var type, out var contactTile))
			{
				if (type == BallPassThroughType.Tile && Main.tileSolid[contactTile.type] && !Main.tileSolidTop[contactTile.type])
				{
					velocity *= 0f;
					num *= 0f;
					flag = true;
				}
				else
				{
					BallPassThroughEvent passThrough = new BallPassThroughEvent(num5, contactTile, entity, type);
					listener.OnPassThrough(physicsProperties, ref position, ref velocity, ref num, ref passThrough);
				}
			}
			position += velocity;
			if (!IsBallInWorld(position, size))
			{
				return BallStepResult.OutOfBounds();
			}
			if (GetClosestEdgeToCircle(position, size, velocity, out var collisionPoint, out contactTile))
			{
				Vector2 val = Vector2.Normalize(position + size * 0.5f - collisionPoint);
				position = collisionPoint + val * (num2 + 0.0001f) - size * 0.5f;
				BallCollisionEvent collision = new BallCollisionEvent(num5, val, collisionPoint, contactTile, entity);
				flag = true;
				velocity = Vector2.Reflect(velocity, collision.Normal);
				listener.OnCollision(physicsProperties, ref position, ref velocity, ref collision);
				num = (collision.Normal.X * velocity.Y - collision.Normal.Y * velocity.X) / num2;
			}
		}
		velocity /= num5;
		num /= num5;
		BallStepResult result = BallStepResult.Moving();
		if (flag && velocity.X > -0.01f && velocity.X < 0.01f && velocity.Y <= 0f && velocity.Y > 0f - physicsProperties.Gravity)
		{
			result = BallStepResult.Resting();
		}
		entity.position = position;
		entity.velocity = velocity;
		entityAngularVelocity = num;
		return result;
	}

	private static bool CheckForPassThrough(Vector2 center, out BallPassThroughType type, out Tile contactTile)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		Point val = center.ToTileCoordinates();
		Tile tile = (contactTile = Main.tile[val.X, val.Y]);
		type = BallPassThroughType.None;
		if (tile == null)
		{
			return false;
		}
		if (tile.nactive())
		{
			type = BallPassThroughType.Tile;
			return IsPositionInsideTile(center, val, tile);
		}
		if (tile.liquid > 0)
		{
			float num = (float)(val.Y + 1) * 16f - (float)(int)tile.liquid / 255f * 16f;
			switch (tile.liquidType())
			{
			case 1:
				type = BallPassThroughType.Lava;
				break;
			case 2:
				type = BallPassThroughType.Honey;
				break;
			default:
				type = BallPassThroughType.Water;
				break;
			}
			return num < center.Y;
		}
		return false;
	}

	private static bool IsPositionInsideTile(Vector2 position, Point tileCoordinates, Tile tile)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		if (tile.slope() == 0 && !tile.halfBrick())
		{
			return true;
		}
		Vector2 val = position / 16f - new Vector2((float)tileCoordinates.X, (float)tileCoordinates.Y);
		return tile.slope() switch
		{
			0 => val.Y > 0.5f, 
			1 => val.Y > val.X, 
			2 => val.Y > 1f - val.X, 
			3 => val.Y < 1f - val.X, 
			4 => val.Y < val.X, 
			_ => false, 
		};
	}

	private static bool IsBallInWorld(Vector2 position, Vector2 size)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		if (position.X > 32f && position.Y > 32f && position.X + size.X < (float)Main.maxTilesX * 16f - 32f)
		{
			return position.Y + size.Y < (float)Main.maxTilesY * 16f - 32f;
		}
		return false;
	}

	private static bool GetClosestEdgeToCircle(Vector2 position, Vector2 size, Vector2 velocity, out Vector2 collisionPoint, out Tile collisionTile)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_0125: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		Rectangle tileBounds = GetTileBounds(position, size);
		Vector2 val = position + size * 0.5f;
		TileEdges tileEdges = TileEdges.None;
		tileEdges = ((!(velocity.Y < 0f)) ? (tileEdges | TileEdges.Top) : (tileEdges | TileEdges.Bottom));
		tileEdges = ((!(velocity.X < 0f)) ? (tileEdges | TileEdges.Left) : (tileEdges | TileEdges.Right));
		tileEdges = ((!(velocity.Y > velocity.X)) ? (tileEdges | TileEdges.TopRightSlope) : (tileEdges | TileEdges.BottomLeftSlope));
		tileEdges = ((!(velocity.Y > 0f - velocity.X)) ? (tileEdges | TileEdges.TopLeftSlope) : (tileEdges | TileEdges.BottomRightSlope));
		collisionPoint = Vector2.Zero;
		collisionTile = null;
		float num = float.MaxValue;
		Vector2 closestPointOut = default;
		float distanceSquaredOut = 0f;
		for (int i = tileBounds.Left; i < tileBounds.Right; i++)
		{
			for (int j = tileBounds.Top; j < tileBounds.Bottom; j++)
			{
				if (GetCollisionPointForTile(tileEdges, i, j, val, ref closestPointOut, ref distanceSquaredOut) && !(distanceSquaredOut >= num) && !(Vector2.Dot(velocity, val - closestPointOut) > 0f))
				{
					num = distanceSquaredOut;
					collisionPoint = closestPointOut;
					collisionTile = Main.tile[i, j];
				}
			}
		}
		float num2 = size.X / 2f;
		return num < num2 * num2;
	}

	private static bool GetCollisionPointForTile(TileEdges edgesToTest, int x, int y, Vector2 center, ref Vector2 closestPointOut, ref float distanceSquaredOut)
	{
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0108: Unknown result type (might be due to invalid IL or missing references)
		//IL_010a: Unknown result type (might be due to invalid IL or missing references)
		//IL_010f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0111: Unknown result type (might be due to invalid IL or missing references)
		//IL_0113: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_012c: Unknown result type (might be due to invalid IL or missing references)
		//IL_012e: Unknown result type (might be due to invalid IL or missing references)
		Tile tile = Main.tile[x, y];
		if (tile == null || !tile.nactive() || (!Main.tileSolid[tile.type] && !Main.tileSolidTop[tile.type]))
		{
			return false;
		}
		if (!Main.tileSolid[tile.type] && Main.tileSolidTop[tile.type] && tile.frameY != 0)
		{
			return false;
		}
		if (Main.tileSolidTop[tile.type])
		{
			edgesToTest &= TileEdges.Top | TileEdges.BottomLeftSlope | TileEdges.BottomRightSlope;
		}
		Vector2 tilePosition = new Vector2((float)x * 16f, (float)y * 16f);
		bool flag = false;
		LineSegment edge = default;
		if (GetSlopeEdge(ref edgesToTest, tile, tilePosition, ref edge))
		{
			closestPointOut = ClosestPointOnLineSegment(center, edge);
			distanceSquaredOut = Vector2.DistanceSquared(closestPointOut, center);
			flag = true;
		}
		if (GetTopOrBottomEdge(edgesToTest, x, y, tilePosition, ref edge))
		{
			Vector2 val = ClosestPointOnLineSegment(center, edge);
			float num = Vector2.DistanceSquared(val, center);
			if (!flag || num < distanceSquaredOut)
			{
				distanceSquaredOut = num;
				closestPointOut = val;
			}
			flag = true;
		}
		if (GetLeftOrRightEdge(edgesToTest, x, y, tilePosition, ref edge))
		{
			Vector2 val2 = ClosestPointOnLineSegment(center, edge);
			float num2 = Vector2.DistanceSquared(val2, center);
			if (!flag || num2 < distanceSquaredOut)
			{
				distanceSquaredOut = num2;
				closestPointOut = val2;
			}
			flag = true;
		}
		return flag;
	}

	private static bool GetSlopeEdge(ref TileEdges edgesToTest, Tile tile, Vector2 tilePosition, ref LineSegment edge)
	{
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0113: Unknown result type (might be due to invalid IL or missing references)
		//IL_0114: Unknown result type (might be due to invalid IL or missing references)
		//IL_011a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0126: Unknown result type (might be due to invalid IL or missing references)
		//IL_0132: Unknown result type (might be due to invalid IL or missing references)
		//IL_0137: Unknown result type (might be due to invalid IL or missing references)
		switch (tile.slope())
		{
		case 0:
			return false;
		case 1:
			edgesToTest &= TileEdges.Bottom | TileEdges.Left | TileEdges.BottomLeftSlope;
			if ((edgesToTest & TileEdges.BottomLeftSlope) == 0)
			{
				return false;
			}
			edge.Start = tilePosition;
			edge.End = new Vector2(tilePosition.X + 16f, tilePosition.Y + 16f);
			return true;
		case 2:
			edgesToTest &= TileEdges.Bottom | TileEdges.Right | TileEdges.BottomRightSlope;
			if ((edgesToTest & TileEdges.BottomRightSlope) == 0)
			{
				return false;
			}
			edge.Start = new Vector2(tilePosition.X, tilePosition.Y + 16f);
			edge.End = new Vector2(tilePosition.X + 16f, tilePosition.Y);
			return true;
		case 3:
			edgesToTest &= TileEdges.Top | TileEdges.Left | TileEdges.TopLeftSlope;
			if ((edgesToTest & TileEdges.TopLeftSlope) == 0)
			{
				return false;
			}
			edge.Start = new Vector2(tilePosition.X, tilePosition.Y + 16f);
			edge.End = new Vector2(tilePosition.X + 16f, tilePosition.Y);
			return true;
		case 4:
			edgesToTest &= TileEdges.Top | TileEdges.Right | TileEdges.TopRightSlope;
			if ((edgesToTest & TileEdges.TopRightSlope) == 0)
			{
				return false;
			}
			edge.Start = tilePosition;
			edge.End = new Vector2(tilePosition.X + 16f, tilePosition.Y + 16f);
			return true;
		default:
			return false;
		}
	}

	private static bool GetTopOrBottomEdge(TileEdges edgesToTest, int x, int y, Vector2 tilePosition, ref LineSegment edge)
	{
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0100: Unknown result type (might be due to invalid IL or missing references)
		//IL_0107: Unknown result type (might be due to invalid IL or missing references)
		//IL_0113: Unknown result type (might be due to invalid IL or missing references)
		//IL_0119: Unknown result type (might be due to invalid IL or missing references)
		//IL_011e: Unknown result type (might be due to invalid IL or missing references)
		if ((edgesToTest & TileEdges.Bottom) != 0)
		{
			Tile tile = Main.tile[x, y + 1];
			if (IsNeighborSolid(tile) && tile.slope() != 1 && tile.slope() != 2 && !tile.halfBrick())
			{
				return false;
			}
			edge.Start = new Vector2(tilePosition.X, tilePosition.Y + 16f);
			edge.End = new Vector2(tilePosition.X + 16f, tilePosition.Y + 16f);
			return true;
		}
		if ((edgesToTest & TileEdges.Top) != 0)
		{
			Tile tile2 = Main.tile[x, y - 1];
			if (!Main.tile[x, y].halfBrick() && IsNeighborSolid(tile2) && tile2.slope() != 3 && tile2.slope() != 4)
			{
				return false;
			}
			if (Main.tile[x, y].halfBrick())
			{
				tilePosition.Y += 8f;
			}
			edge.Start = new Vector2(tilePosition.X, tilePosition.Y);
			edge.End = new Vector2(tilePosition.X + 16f, tilePosition.Y);
			return true;
		}
		return false;
	}

	private static bool GetLeftOrRightEdge(TileEdges edgesToTest, int x, int y, Vector2 tilePosition, ref LineSegment edge)
	{
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		//IL_0115: Unknown result type (might be due to invalid IL or missing references)
		//IL_011b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0120: Unknown result type (might be due to invalid IL or missing references)
		//IL_0127: Unknown result type (might be due to invalid IL or missing references)
		//IL_0133: Unknown result type (might be due to invalid IL or missing references)
		//IL_013f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0144: Unknown result type (might be due to invalid IL or missing references)
		if ((edgesToTest & TileEdges.Left) != 0)
		{
			Tile tile = Main.tile[x, y];
			Tile tile2 = Main.tile[x - 1, y];
			if (IsNeighborSolid(tile2) && tile2.slope() != 1 && tile2.slope() != 3 && (!tile2.halfBrick() || tile.halfBrick()))
			{
				return false;
			}
			edge.Start = new Vector2(tilePosition.X, tilePosition.Y);
			edge.End = new Vector2(tilePosition.X, tilePosition.Y + 16f);
			if (tile.halfBrick())
			{
				edge.Start.Y += 8f;
			}
			return true;
		}
		if ((edgesToTest & TileEdges.Right) != 0)
		{
			Tile tile3 = Main.tile[x, y];
			Tile tile4 = Main.tile[x + 1, y];
			if (IsNeighborSolid(tile4) && tile4.slope() != 2 && tile4.slope() != 4 && (!tile4.halfBrick() || tile3.halfBrick()))
			{
				return false;
			}
			edge.Start = new Vector2(tilePosition.X + 16f, tilePosition.Y);
			edge.End = new Vector2(tilePosition.X + 16f, tilePosition.Y + 16f);
			if (tile3.halfBrick())
			{
				edge.Start.Y += 8f;
			}
			return true;
		}
		return false;
	}

	private static Rectangle GetTileBounds(Vector2 position, Vector2 size)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		int num = (int)Math.Floor(position.X / 16f);
		int num2 = (int)Math.Floor(position.Y / 16f);
		int num3 = (int)Math.Floor((position.X + size.X) / 16f);
		int num4 = (int)Math.Floor((position.Y + size.Y) / 16f);
		return new Rectangle(num, num2, num3 - num + 1, num4 - num2 + 1);
	}

	private static bool IsNeighborSolid(Tile tile)
	{
		if (tile != null && tile.nactive() && Main.tileSolid[tile.type])
		{
			return !Main.tileSolidTop[tile.type];
		}
		return false;
	}

	private static Vector2 ClosestPointOnLineSegment(Vector2 point, LineSegment lineSegment)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		Vector2 val = point - lineSegment.Start;
		Vector2 val2 = lineSegment.End - lineSegment.Start;
		float num = val2.LengthSquared();
		float num2 = Vector2.Dot(val, val2) / num;
		if (num2 < 0f)
		{
			return lineSegment.Start;
		}
		if (num2 > 1f)
		{
			return lineSegment.End;
		}
		return lineSegment.Start + val2 * num2;
	}

	[Conditional("DEBUG")]
	private static void DrawEdge(LineSegment edge)
	{
	}
}
