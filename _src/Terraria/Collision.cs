using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria.ID;

namespace Terraria;

public class Collision
{
	public enum TileContactSide
	{
		Left,
		Right,
		Top,
		Bottom,
		BottomLeft,
		BottomRight
	}

	public struct TileContact(TileContactSide side, int x, int y, int type, int slope, int overlap)
	{
		public TileContactSide Side = side;

		public int Overlap = overlap;

		public int X = x;

		public int Y = y;

		public int Slope = slope;

		public int Type = type;
	}

	public struct HurtTile
	{
		public int type;

		public int x;

		public int y;
	}

	public static bool stair;

	public static bool stairFall;

	public static bool honey;

	public static bool shimmer;

	public static bool sloping;

	public static bool landMine = false;

	public static bool up;

	public static bool down;

	public static float Epsilon = (float)Math.E;

	private const int bottomFluff = 40;

	private static List<TileContact> contacts = new List<TileContact>();

	private static List<Point> _cacheForConveyorBelts = new List<Point>();

	public static Vector2[] CheckLinevLine(Vector2 a1, Vector2 a2, Vector2 b1, Vector2 b2)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0105: Unknown result type (might be due to invalid IL or missing references)
		//IL_010b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0113: Unknown result type (might be due to invalid IL or missing references)
		//IL_0119: Unknown result type (might be due to invalid IL or missing references)
		//IL_0120: Unknown result type (might be due to invalid IL or missing references)
		//IL_0126: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0174: Unknown result type (might be due to invalid IL or missing references)
		//IL_017b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0181: Unknown result type (might be due to invalid IL or missing references)
		//IL_018a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0191: Unknown result type (might be due to invalid IL or missing references)
		//IL_0197: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_01eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01de: Unknown result type (might be due to invalid IL or missing references)
		//IL_01df: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e1: Unknown result type (might be due to invalid IL or missing references)
		if (a1.Equals(a2) && b1.Equals(b2))
		{
			if (a1.Equals(b1))
			{
				return new Vector2[1] { a1 };
			}
			return new Vector2[0];
		}
		if (b1.Equals(b2))
		{
			if (PointOnLine(b1, a1, a2))
			{
				return new Vector2[1] { b1 };
			}
			return new Vector2[0];
		}
		if (a1.Equals(a2))
		{
			if (PointOnLine(a1, b1, b2))
			{
				return new Vector2[1] { a1 };
			}
			return new Vector2[0];
		}
		float num = (b2.X - b1.X) * (a1.Y - b1.Y) - (b2.Y - b1.Y) * (a1.X - b1.X);
		float num2 = (a2.X - a1.X) * (a1.Y - b1.Y) - (a2.Y - a1.Y) * (a1.X - b1.X);
		float num3 = (b2.Y - b1.Y) * (a2.X - a1.X) - (b2.X - b1.X) * (a2.Y - a1.Y);
		if (!(0f - Epsilon < num3) || !(num3 < Epsilon))
		{
			float num4 = num / num3;
			float num5 = num2 / num3;
			if (0f <= num4 && num4 <= 1f && 0f <= num5 && num5 <= 1f)
			{
				return new Vector2[1]
				{
					new Vector2(a1.X + num4 * (a2.X - a1.X), a1.Y + num4 * (a2.Y - a1.Y))
				};
			}
			return new Vector2[0];
		}
		if ((0f - Epsilon < num && num < Epsilon) || (0f - Epsilon < num2 && num2 < Epsilon))
		{
			if (a1.Equals(a2))
			{
				return OneDimensionalIntersection(b1, b2, a1, a2);
			}
			return OneDimensionalIntersection(a1, a2, b1, b2);
		}
		return new Vector2[0];
	}

	private static double DistFromSeg(Vector2 p, Vector2 q0, Vector2 q1, double radius, ref float u)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		double num = q1.X - q0.X;
		double num2 = q1.Y - q0.Y;
		double num3 = q0.X - p.X;
		double num4 = q0.Y - p.Y;
		double num5 = Math.Sqrt(num * num + num2 * num2);
		if (num5 < (double)Epsilon)
		{
			throw new Exception("Expected line segment, not point.");
		}
		return Math.Abs(num * num4 - num3 * num2) / num5;
	}

	private static bool PointOnLine(Vector2 p, Vector2 a1, Vector2 a2)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		float u = 0f;
		return DistFromSeg(p, a1, a2, Epsilon, ref u) < (double)Epsilon;
	}

	private static Vector2[] OneDimensionalIntersection(Vector2 a1, Vector2 a2, Vector2 b1, Vector2 b2)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		float num = a2.X - a1.X;
		float num2 = a2.Y - a1.Y;
		float relativePoint;
		float relativePoint2;
		if (Math.Abs(num) > Math.Abs(num2))
		{
			relativePoint = (b1.X - a1.X) / num;
			relativePoint2 = (b2.X - a1.X) / num;
		}
		else
		{
			relativePoint = (b1.Y - a1.Y) / num2;
			relativePoint2 = (b2.Y - a1.Y) / num2;
		}
		List<Vector2> list = new List<Vector2>();
		float[] array = FindOverlapPoints(relativePoint, relativePoint2);
		foreach (float num3 in array)
		{
			float num4 = a2.X * num3 + a1.X * (1f - num3);
			float num5 = a2.Y * num3 + a1.Y * (1f - num3);
			list.Add(new Vector2(num4, num5));
		}
		return list.ToArray();
	}

	private static float[] FindOverlapPoints(float relativePoint1, float relativePoint2)
	{
		float val = Math.Min(relativePoint1, relativePoint2);
		float val2 = Math.Max(relativePoint1, relativePoint2);
		float num = Math.Max(0f, val);
		float num2 = Math.Min(1f, val2);
		if (num > num2)
		{
			return new float[0];
		}
		if (num != num2)
		{
			return new float[2] { num, num2 };
		}
		return new float[1] { num };
	}

	public static bool CheckAABBvAABBCollision(Vector2 position1, Vector2 dimensions1, Vector2 position2, Vector2 dimensions2)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		if (position1.X < position2.X + dimensions2.X && position1.Y < position2.Y + dimensions2.Y && position1.X + dimensions1.X > position2.X)
		{
			return position1.Y + dimensions1.Y > position2.Y;
		}
		return false;
	}

	private static int collisionOutcode(Vector2 aabbPosition, Vector2 aabbDimensions, Vector2 point)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		float num = aabbPosition.X + aabbDimensions.X;
		float num2 = aabbPosition.Y + aabbDimensions.Y;
		int num3 = 0;
		if (aabbDimensions.X <= 0f)
		{
			num3 |= 5;
		}
		else if (point.X < aabbPosition.X)
		{
			num3 |= 1;
		}
		else if (point.X - num > 0f)
		{
			num3 |= 4;
		}
		if (aabbDimensions.Y <= 0f)
		{
			num3 |= 0xA;
		}
		else if (point.Y < aabbPosition.Y)
		{
			num3 |= 2;
		}
		else if (point.Y - num2 > 0f)
		{
			num3 |= 8;
		}
		return num3;
	}

	public static bool CheckAABBvLineCollision(Vector2 aabbPosition, Vector2 aabbDimensions, Vector2 lineStart, Vector2 lineEnd)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		int num;
		if ((num = collisionOutcode(aabbPosition, aabbDimensions, lineEnd)) == 0)
		{
			return true;
		}
		int num2;
		while ((num2 = collisionOutcode(aabbPosition, aabbDimensions, lineStart)) != 0)
		{
			if ((num2 & num) != 0)
			{
				return false;
			}
			if ((num2 & 5) != 0)
			{
				float num3 = aabbPosition.X;
				if ((num2 & 4) != 0)
				{
					num3 += aabbDimensions.X;
				}
				lineStart.Y += (num3 - lineStart.X) * (lineEnd.Y - lineStart.Y) / (lineEnd.X - lineStart.X);
				lineStart.X = num3;
			}
			else
			{
				float num4 = aabbPosition.Y;
				if ((num2 & 8) != 0)
				{
					num4 += aabbDimensions.Y;
				}
				lineStart.X += (num4 - lineStart.Y) * (lineEnd.X - lineStart.X) / (lineEnd.Y - lineStart.Y);
				lineStart.Y = num4;
			}
		}
		return true;
	}

	public static bool CheckAABBvLineCollision2(Vector2 aabbPosition, Vector2 aabbDimensions, Vector2 lineStart, Vector2 lineEnd)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		float collisionPoint = 0f;
		if (!Utils.RectangleLineCollision(aabbPosition, aabbPosition + aabbDimensions, lineStart, lineEnd))
		{
			return CheckAABBvLineCollision(aabbPosition, aabbDimensions, lineStart, lineEnd, 0.0001f, ref collisionPoint);
		}
		return true;
	}

	public static bool CheckAABBvLineCollision(Vector2 objectPosition, Vector2 objectDimensions, Vector2 lineStart, Vector2 lineEnd, float lineWidth, ref float collisionPoint)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		//IL_010b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0126: Unknown result type (might be due to invalid IL or missing references)
		//IL_012d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0133: Unknown result type (might be due to invalid IL or missing references)
		//IL_0135: Unknown result type (might be due to invalid IL or missing references)
		//IL_013a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0142: Unknown result type (might be due to invalid IL or missing references)
		//IL_014a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0150: Unknown result type (might be due to invalid IL or missing references)
		//IL_0152: Unknown result type (might be due to invalid IL or missing references)
		//IL_0157: Unknown result type (might be due to invalid IL or missing references)
		//IL_015f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0167: Unknown result type (might be due to invalid IL or missing references)
		//IL_016d: Unknown result type (might be due to invalid IL or missing references)
		//IL_016f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0174: Unknown result type (might be due to invalid IL or missing references)
		//IL_017c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0184: Unknown result type (might be due to invalid IL or missing references)
		//IL_018a: Unknown result type (might be due to invalid IL or missing references)
		//IL_018c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0191: Unknown result type (might be due to invalid IL or missing references)
		//IL_020b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0215: Unknown result type (might be due to invalid IL or missing references)
		//IL_0223: Unknown result type (might be due to invalid IL or missing references)
		//IL_022e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0246: Unknown result type (might be due to invalid IL or missing references)
		//IL_0248: Unknown result type (might be due to invalid IL or missing references)
		//IL_024a: Unknown result type (might be due to invalid IL or missing references)
		//IL_024f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0255: Unknown result type (might be due to invalid IL or missing references)
		//IL_025e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0263: Unknown result type (might be due to invalid IL or missing references)
		//IL_0268: Unknown result type (might be due to invalid IL or missing references)
		//IL_026a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0271: Unknown result type (might be due to invalid IL or missing references)
		//IL_0279: Unknown result type (might be due to invalid IL or missing references)
		//IL_0280: Unknown result type (might be due to invalid IL or missing references)
		//IL_0335: Unknown result type (might be due to invalid IL or missing references)
		//IL_0337: Unknown result type (might be due to invalid IL or missing references)
		//IL_0339: Unknown result type (might be due to invalid IL or missing references)
		//IL_033e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0340: Unknown result type (might be due to invalid IL or missing references)
		//IL_0347: Unknown result type (might be due to invalid IL or missing references)
		//IL_034f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0356: Unknown result type (might be due to invalid IL or missing references)
		//IL_029b: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0371: Unknown result type (might be due to invalid IL or missing references)
		//IL_0376: Unknown result type (might be due to invalid IL or missing references)
		//IL_0378: Unknown result type (might be due to invalid IL or missing references)
		//IL_037d: Unknown result type (might be due to invalid IL or missing references)
		//IL_037f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0386: Unknown result type (might be due to invalid IL or missing references)
		//IL_038e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0395: Unknown result type (might be due to invalid IL or missing references)
		//IL_02df: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_03cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_031d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0326: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03fc: Unknown result type (might be due to invalid IL or missing references)
		float num = lineWidth * 0.5f;
		Vector2 position = lineStart;
		Vector2 val = lineEnd - lineStart;
		if (val.X > 0f)
		{
			val.X += lineWidth;
			position.X -= num;
		}
		else
		{
			position.X += val.X - num;
			val.X = 0f - val.X + lineWidth;
		}
		if (val.Y > 0f)
		{
			val.Y += lineWidth;
			position.Y -= num;
		}
		else
		{
			position.Y += val.Y - num;
			val.Y = 0f - val.Y + lineWidth;
		}
		if (!CheckAABBvAABBCollision(objectPosition, objectDimensions, position, val))
		{
			return false;
		}
		Vector2 val2 = objectPosition - lineStart;
		Vector2 val3 = val2 + objectDimensions;
		Vector2 spinningpoint = new Vector2(val2.X, val3.Y);
		Vector2 spinningpoint2 = new Vector2(val3.X, val2.Y);
		Vector2 val4 = lineEnd - lineStart;
		float num2 = val4.Length();
		float num3 = (float)Math.Atan2(val4.Y, val4.X);
		Vector2[] array = new Vector2[4]
		{
			val2.RotatedBy(0f - num3),
			spinningpoint2.RotatedBy(0f - num3),
			val3.RotatedBy(0f - num3),
			spinningpoint.RotatedBy(0f - num3)
		};
		collisionPoint = num2;
		bool result = false;
		for (int i = 0; i < array.Length; i++)
		{
			if (Math.Abs(array[i].Y) < num && array[i].X < collisionPoint && array[i].X >= 0f)
			{
				collisionPoint = array[i].X;
				result = true;
			}
		}
		Vector2 val5 = new Vector2(0f, num);
		Vector2 val6 = new Vector2(num2, num);
		Vector2 val7 = new Vector2(0f, 0f - num);
		Vector2 val8 = new Vector2(num2, 0f - num);
		for (int j = 0; j < array.Length; j++)
		{
			int num4 = (j + 1) % array.Length;
			Vector2 val9 = val6 - val5;
			Vector2 val10 = array[num4] - array[j];
			float num5 = val9.X * val10.Y - val9.Y * val10.X;
			if (num5 != 0f)
			{
				Vector2 val11 = array[j] - val5;
				float num6 = (val11.X * val10.Y - val11.Y * val10.X) / num5;
				if (num6 >= 0f && num6 <= 1f)
				{
					float num7 = (val11.X * val9.Y - val11.Y * val9.X) / num5;
					if (num7 >= 0f && num7 <= 1f)
					{
						result = true;
						collisionPoint = Math.Min(collisionPoint, val5.X + num6 * val9.X);
					}
				}
			}
			val9 = val8 - val7;
			num5 = val9.X * val10.Y - val9.Y * val10.X;
			if (num5 == 0f)
			{
				continue;
			}
			Vector2 val12 = array[j] - val7;
			float num8 = (val12.X * val10.Y - val12.Y * val10.X) / num5;
			if (num8 >= 0f && num8 <= 1f)
			{
				float num9 = (val12.X * val9.Y - val12.Y * val9.X) / num5;
				if (num9 >= 0f && num9 <= 1f)
				{
					result = true;
					collisionPoint = Math.Min(collisionPoint, val7.X + num8 * val9.X);
				}
			}
		}
		return result;
	}

	public static bool CanHit(Entity source, Entity target)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		return CanHit(source.position, source.width, source.height, target.position, target.width, target.height);
	}

	public static bool CanHit(Entity source, NPCAimedTarget target)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		return CanHit(source.position, source.width, source.height, target.Position, target.Width, target.Height);
	}

	public static bool CanHit(Vector2 Position1, int Width1, int Height1, Vector2 Position2, int Width2, int Height2)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		return CanHit(Position1.ToPoint(), Width1, Height1, Position2.ToPoint(), Width2, Height2);
	}

	public static bool CanHit(Point Position1, int Width1, int Height1, Point Position2, int Width2, int Height2)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		int num = (Position1.X + Width1 / 2) / 16;
		int num2 = (Position1.Y + Height1 / 2) / 16;
		int num3 = (Position2.X + Width2 / 2) / 16;
		int num4 = (Position2.Y + Height2 / 2) / 16;
		if (num <= 1)
		{
			num = 1;
		}
		if (num >= Main.maxTilesX)
		{
			num = Main.maxTilesX - 1;
		}
		if (num3 <= 1)
		{
			num3 = 1;
		}
		if (num3 >= Main.maxTilesX)
		{
			num3 = Main.maxTilesX - 1;
		}
		if (num2 <= 1)
		{
			num2 = 1;
		}
		if (num2 >= Main.maxTilesY - 40)
		{
			num2 = Main.maxTilesY - 40;
		}
		if (num4 <= 1)
		{
			num4 = 1;
		}
		if (num4 >= Main.maxTilesY - 40)
		{
			num4 = Main.maxTilesY - 40;
		}
		try
		{
			do
			{
				int num5 = Math.Abs(num - num3);
				int num6 = Math.Abs(num2 - num4);
				if (num == num3 && num2 == num4)
				{
					return true;
				}
				if (num5 > num6)
				{
					num = ((num >= num3) ? (num - 1) : (num + 1));
					if (Main.tile[num, num2 - 1] == null)
					{
						return false;
					}
					if (Main.tile[num, num2 + 1] == null)
					{
						return false;
					}
					if (!Main.tile[num, num2 - 1].inActive() && Main.tile[num, num2 - 1].active() && Main.tileSolid[Main.tile[num, num2 - 1].type] && !Main.tileSolidTop[Main.tile[num, num2 - 1].type] && Main.tile[num, num2 - 1].slope() == 0 && !Main.tile[num, num2 - 1].halfBrick() && !Main.tile[num, num2 + 1].inActive() && Main.tile[num, num2 + 1].active() && Main.tileSolid[Main.tile[num, num2 + 1].type] && !Main.tileSolidTop[Main.tile[num, num2 + 1].type] && Main.tile[num, num2 + 1].slope() == 0 && !Main.tile[num, num2 + 1].halfBrick())
					{
						return false;
					}
				}
				else
				{
					num2 = ((num2 >= num4) ? (num2 - 1) : (num2 + 1));
					if (Main.tile[num - 1, num2] == null)
					{
						return false;
					}
					if (Main.tile[num + 1, num2] == null)
					{
						return false;
					}
					if (!Main.tile[num - 1, num2].inActive() && Main.tile[num - 1, num2].active() && Main.tileSolid[Main.tile[num - 1, num2].type] && !Main.tileSolidTop[Main.tile[num - 1, num2].type] && Main.tile[num - 1, num2].slope() == 0 && !Main.tile[num - 1, num2].halfBrick() && !Main.tile[num + 1, num2].inActive() && Main.tile[num + 1, num2].active() && Main.tileSolid[Main.tile[num + 1, num2].type] && !Main.tileSolidTop[Main.tile[num + 1, num2].type] && Main.tile[num + 1, num2].slope() == 0 && !Main.tile[num + 1, num2].halfBrick())
					{
						return false;
					}
				}
				if (Main.tile[num, num2] == null)
				{
					return false;
				}
			}
			while (Main.tile[num, num2].inActive() || !Main.tile[num, num2].active() || !Main.tileSolid[Main.tile[num, num2].type] || Main.tileSolidTop[Main.tile[num, num2].type]);
			return false;
		}
		catch
		{
			return false;
		}
	}

	public static bool CanHitWithCheck(Entity source, Entity target, Utils.TileActionAttempt check)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		return CanHitWithCheck(source.position, source.width, source.height, target.position, target.width, target.height, check);
	}

	public static bool CanHit(Entity source, NPCAimedTarget target, Utils.TileActionAttempt check)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		return CanHitWithCheck(source.position, source.width, source.height, target.Position, target.Width, target.Height, check);
	}

	public static bool CanHitWithCheck(Vector2 Position1, int Width1, int Height1, Vector2 Position2, int Width2, int Height2, Utils.TileActionAttempt check)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		int num = (int)((Position1.X + (float)(Width1 / 2)) / 16f);
		int num2 = (int)((Position1.Y + (float)(Height1 / 2)) / 16f);
		int num3 = (int)((Position2.X + (float)(Width2 / 2)) / 16f);
		int num4 = (int)((Position2.Y + (float)(Height2 / 2)) / 16f);
		if (num <= 1)
		{
			num = 1;
		}
		if (num >= Main.maxTilesX)
		{
			num = Main.maxTilesX - 1;
		}
		if (num3 <= 1)
		{
			num3 = 1;
		}
		if (num3 >= Main.maxTilesX)
		{
			num3 = Main.maxTilesX - 1;
		}
		if (num2 <= 1)
		{
			num2 = 1;
		}
		if (num2 >= Main.maxTilesY - 40)
		{
			num2 = Main.maxTilesY - 40;
		}
		if (num4 <= 1)
		{
			num4 = 1;
		}
		if (num4 >= Main.maxTilesY - 40)
		{
			num4 = Main.maxTilesY - 40;
		}
		try
		{
			do
			{
				int num5 = Math.Abs(num - num3);
				int num6 = Math.Abs(num2 - num4);
				if (num == num3 && num2 == num4)
				{
					return true;
				}
				if (num5 > num6)
				{
					num = ((num >= num3) ? (num - 1) : (num + 1));
					if (Main.tile[num, num2 - 1] == null)
					{
						return false;
					}
					if (Main.tile[num, num2 + 1] == null)
					{
						return false;
					}
					if (!Main.tile[num, num2 - 1].inActive() && Main.tile[num, num2 - 1].active() && Main.tileSolid[Main.tile[num, num2 - 1].type] && !Main.tileSolidTop[Main.tile[num, num2 - 1].type] && Main.tile[num, num2 - 1].slope() == 0 && !Main.tile[num, num2 - 1].halfBrick() && !Main.tile[num, num2 + 1].inActive() && Main.tile[num, num2 + 1].active() && Main.tileSolid[Main.tile[num, num2 + 1].type] && !Main.tileSolidTop[Main.tile[num, num2 + 1].type] && Main.tile[num, num2 + 1].slope() == 0 && !Main.tile[num, num2 + 1].halfBrick())
					{
						return false;
					}
				}
				else
				{
					num2 = ((num2 >= num4) ? (num2 - 1) : (num2 + 1));
					if (Main.tile[num - 1, num2] == null)
					{
						return false;
					}
					if (Main.tile[num + 1, num2] == null)
					{
						return false;
					}
					if (!Main.tile[num - 1, num2].inActive() && Main.tile[num - 1, num2].active() && Main.tileSolid[Main.tile[num - 1, num2].type] && !Main.tileSolidTop[Main.tile[num - 1, num2].type] && Main.tile[num - 1, num2].slope() == 0 && !Main.tile[num - 1, num2].halfBrick() && !Main.tile[num + 1, num2].inActive() && Main.tile[num + 1, num2].active() && Main.tileSolid[Main.tile[num + 1, num2].type] && !Main.tileSolidTop[Main.tile[num + 1, num2].type] && Main.tile[num + 1, num2].slope() == 0 && !Main.tile[num + 1, num2].halfBrick())
					{
						return false;
					}
				}
				if (Main.tile[num, num2] == null)
				{
					return false;
				}
				if (!Main.tile[num, num2].inActive() && Main.tile[num, num2].active() && Main.tileSolid[Main.tile[num, num2].type] && !Main.tileSolidTop[Main.tile[num, num2].type])
				{
					return false;
				}
			}
			while (check(num, num2));
			return false;
		}
		catch
		{
			return false;
		}
	}

	public static bool CanHitLine(Vector2 Position1, int Width1, int Height1, Vector2 Position2, int Width2, int Height2)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		int num = (int)((Position1.X + (float)(Width1 / 2)) / 16f);
		int num2 = (int)((Position1.Y + (float)(Height1 / 2)) / 16f);
		int num3 = (int)((Position2.X + (float)(Width2 / 2)) / 16f);
		int num4 = (int)((Position2.Y + (float)(Height2 / 2)) / 16f);
		if (num <= 1)
		{
			num = 1;
		}
		if (num >= Main.maxTilesX)
		{
			num = Main.maxTilesX - 1;
		}
		if (num3 <= 1)
		{
			num3 = 1;
		}
		if (num3 >= Main.maxTilesX)
		{
			num3 = Main.maxTilesX - 1;
		}
		if (num2 <= 1)
		{
			num2 = 1;
		}
		if (num2 >= Main.maxTilesY - 40)
		{
			num2 = Main.maxTilesY - 40;
		}
		if (num4 <= 1)
		{
			num4 = 1;
		}
		if (num4 >= Main.maxTilesY - 40)
		{
			num4 = Main.maxTilesY - 40;
		}
		float num5 = Math.Abs(num - num3);
		float num6 = Math.Abs(num2 - num4);
		if (num5 == 0f && num6 == 0f)
		{
			return true;
		}
		float num7 = 1f;
		float num8 = 1f;
		if (num5 == 0f || num6 == 0f)
		{
			if (num5 == 0f)
			{
				num7 = 0f;
			}
			if (num6 == 0f)
			{
				num8 = 0f;
			}
		}
		else if (num5 > num6)
		{
			num7 = num5 / num6;
		}
		else
		{
			num8 = num6 / num5;
		}
		float num9 = 0f;
		float num10 = 0f;
		int num11 = 1;
		if (num2 < num4)
		{
			num11 = 2;
		}
		int num12 = (int)num5;
		int num13 = (int)num6;
		int num14 = Math.Sign(num3 - num);
		int num15 = Math.Sign(num4 - num2);
		bool flag = false;
		bool flag2 = false;
		try
		{
			do
			{
				switch (num11)
				{
				case 2:
				{
					num9 += num7;
					int num17 = (int)num9;
					num9 -= (float)num17;
					for (int j = 0; j < num17; j++)
					{
						if (Main.tile[num, num2 - 1] == null)
						{
							return false;
						}
						if (Main.tile[num, num2] == null)
						{
							return false;
						}
						if (Main.tile[num, num2 + 1] == null)
						{
							return false;
						}
						Tile tile4 = Main.tile[num, num2 - 1];
						Tile tile5 = Main.tile[num, num2 + 1];
						Tile tile6 = Main.tile[num, num2];
						if ((!tile4.inActive() && tile4.active() && Main.tileSolid[tile4.type] && !Main.tileSolidTop[tile4.type]) || (!tile5.inActive() && tile5.active() && Main.tileSolid[tile5.type] && !Main.tileSolidTop[tile5.type]) || (!tile6.inActive() && tile6.active() && Main.tileSolid[tile6.type] && !Main.tileSolidTop[tile6.type]))
						{
							return false;
						}
						if (num12 == 0 && num13 == 0)
						{
							flag = true;
							break;
						}
						num += num14;
						num12--;
						if (num12 == 0 && num13 == 0 && num17 == 1)
						{
							flag2 = true;
						}
					}
					if (num13 != 0)
					{
						num11 = 1;
					}
					break;
				}
				case 1:
				{
					num10 += num8;
					int num16 = (int)num10;
					num10 -= (float)num16;
					for (int i = 0; i < num16; i++)
					{
						if (Main.tile[num - 1, num2] == null)
						{
							return false;
						}
						if (Main.tile[num, num2] == null)
						{
							return false;
						}
						if (Main.tile[num + 1, num2] == null)
						{
							return false;
						}
						Tile tile = Main.tile[num - 1, num2];
						Tile tile2 = Main.tile[num + 1, num2];
						Tile tile3 = Main.tile[num, num2];
						if ((!tile.inActive() && tile.active() && Main.tileSolid[tile.type] && !Main.tileSolidTop[tile.type]) || (!tile2.inActive() && tile2.active() && Main.tileSolid[tile2.type] && !Main.tileSolidTop[tile2.type]) || (!tile3.inActive() && tile3.active() && Main.tileSolid[tile3.type] && !Main.tileSolidTop[tile3.type]))
						{
							return false;
						}
						if (num12 == 0 && num13 == 0)
						{
							flag = true;
							break;
						}
						num2 += num15;
						num13--;
						if (num12 == 0 && num13 == 0 && num16 == 1)
						{
							flag2 = true;
						}
					}
					if (num12 != 0)
					{
						num11 = 2;
					}
					break;
				}
				}
				if (Main.tile[num, num2] == null)
				{
					return false;
				}
				Tile tile7 = Main.tile[num, num2];
				if (!tile7.inActive() && tile7.active() && Main.tileSolid[tile7.type] && !Main.tileSolidTop[tile7.type])
				{
					return false;
				}
			}
			while (!(flag | flag2));
			return true;
		}
		catch
		{
			return false;
		}
	}

	public static bool HitLine(int x1, int y1, int x2, int y2, int ignoreX, int ignoreY, List<Point> ignoreTargets, out Point col)
	{
		//IL_05cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_055e: Unknown result type (might be due to invalid IL or missing references)
		//IL_053a: Unknown result type (might be due to invalid IL or missing references)
		//IL_053f: Unknown result type (might be due to invalid IL or missing references)
		//IL_014c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0151: Unknown result type (might be due to invalid IL or missing references)
		//IL_05bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_035b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0360: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0174: Unknown result type (might be due to invalid IL or missing references)
		//IL_0179: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0383: Unknown result type (might be due to invalid IL or missing references)
		//IL_0388: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_03dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_059e: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_0231: Unknown result type (might be due to invalid IL or missing references)
		//IL_0236: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_04dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0440: Unknown result type (might be due to invalid IL or missing references)
		//IL_0445: Unknown result type (might be due to invalid IL or missing references)
		//IL_0283: Unknown result type (might be due to invalid IL or missing references)
		//IL_0288: Unknown result type (might be due to invalid IL or missing references)
		//IL_0492: Unknown result type (might be due to invalid IL or missing references)
		//IL_0497: Unknown result type (might be due to invalid IL or missing references)
		int value = x1;
		int value2 = y1;
		int value3 = x2;
		int value4 = y2;
		value = Utils.Clamp(value, 1, Main.maxTilesX - 1);
		value3 = Utils.Clamp(value3, 1, Main.maxTilesX - 1);
		value2 = Utils.Clamp(value2, 1, Main.maxTilesY - 40);
		value4 = Utils.Clamp(value4, 1, Main.maxTilesY - 40);
		float num = Math.Abs(value - value3);
		float num2 = Math.Abs(value2 - value4);
		if (num == 0f && num2 == 0f)
		{
			col = new Point(value, value2);
			return true;
		}
		float num3 = 1f;
		float num4 = 1f;
		if (num == 0f || num2 == 0f)
		{
			if (num == 0f)
			{
				num3 = 0f;
			}
			if (num2 == 0f)
			{
				num4 = 0f;
			}
		}
		else if (num > num2)
		{
			num3 = num / num2;
		}
		else
		{
			num4 = num2 / num;
		}
		float num5 = 0f;
		float num6 = 0f;
		int num7 = 1;
		if (value2 < value4)
		{
			num7 = 2;
		}
		int num8 = (int)num;
		int num9 = (int)num2;
		int num10 = Math.Sign(value3 - value);
		int num11 = Math.Sign(value4 - value2);
		bool flag = false;
		bool flag2 = false;
		try
		{
			do
			{
				switch (num7)
				{
				case 2:
				{
					num5 += num3;
					int num13 = (int)num5;
					num5 -= (float)num13;
					for (int j = 0; j < num13; j++)
					{
						if (Main.tile[value, value2 - 1] == null)
						{
							col = new Point(value, value2 - 1);
							return false;
						}
						if (Main.tile[value, value2 + 1] == null)
						{
							col = new Point(value, value2 + 1);
							return false;
						}
						Tile tile4 = Main.tile[value, value2 - 1];
						Tile tile5 = Main.tile[value, value2 + 1];
						Tile tile6 = Main.tile[value, value2];
						if (!ignoreTargets.Contains(new Point(value, value2)) && !ignoreTargets.Contains(new Point(value, value2 - 1)) && !ignoreTargets.Contains(new Point(value, value2 + 1)))
						{
							if (ignoreY != -1 && num11 < 0 && !tile4.inActive() && tile4.active() && Main.tileSolid[tile4.type] && !Main.tileSolidTop[tile4.type])
							{
								col = new Point(value, value2 - 1);
								return true;
							}
							if (ignoreY != 1 && num11 > 0 && !tile5.inActive() && tile5.active() && Main.tileSolid[tile5.type] && !Main.tileSolidTop[tile5.type])
							{
								col = new Point(value, value2 + 1);
								return true;
							}
							if (!tile6.inActive() && tile6.active() && Main.tileSolid[tile6.type] && !Main.tileSolidTop[tile6.type])
							{
								col = new Point(value, value2);
								return true;
							}
						}
						if (num8 == 0 && num9 == 0)
						{
							flag = true;
							break;
						}
						value += num10;
						num8--;
						if (num8 == 0 && num9 == 0 && num13 == 1)
						{
							flag2 = true;
						}
					}
					if (num9 != 0)
					{
						num7 = 1;
					}
					break;
				}
				case 1:
				{
					num6 += num4;
					int num12 = (int)num6;
					num6 -= (float)num12;
					for (int i = 0; i < num12; i++)
					{
						if (Main.tile[value - 1, value2] == null)
						{
							col = new Point(value - 1, value2);
							return false;
						}
						if (Main.tile[value + 1, value2] == null)
						{
							col = new Point(value + 1, value2);
							return false;
						}
						Tile tile = Main.tile[value - 1, value2];
						Tile tile2 = Main.tile[value + 1, value2];
						Tile tile3 = Main.tile[value, value2];
						if (!ignoreTargets.Contains(new Point(value, value2)) && !ignoreTargets.Contains(new Point(value - 1, value2)) && !ignoreTargets.Contains(new Point(value + 1, value2)))
						{
							if (ignoreX != -1 && num10 < 0 && !tile.inActive() && tile.active() && Main.tileSolid[tile.type] && !Main.tileSolidTop[tile.type])
							{
								col = new Point(value - 1, value2);
								return true;
							}
							if (ignoreX != 1 && num10 > 0 && !tile2.inActive() && tile2.active() && Main.tileSolid[tile2.type] && !Main.tileSolidTop[tile2.type])
							{
								col = new Point(value + 1, value2);
								return true;
							}
							if (!tile3.inActive() && tile3.active() && Main.tileSolid[tile3.type] && !Main.tileSolidTop[tile3.type])
							{
								col = new Point(value, value2);
								return true;
							}
						}
						if (num8 == 0 && num9 == 0)
						{
							flag = true;
							break;
						}
						value2 += num11;
						num9--;
						if (num8 == 0 && num9 == 0 && num12 == 1)
						{
							flag2 = true;
						}
					}
					if (num8 != 0)
					{
						num7 = 2;
					}
					break;
				}
				}
				if (Main.tile[value, value2] == null)
				{
					col = new Point(value, value2);
					return false;
				}
				Tile tile7 = Main.tile[value, value2];
				if (!ignoreTargets.Contains(new Point(value, value2)) && !tile7.inActive() && tile7.active() && Main.tileSolid[tile7.type] && !Main.tileSolidTop[tile7.type])
				{
					col = new Point(value, value2);
					return true;
				}
			}
			while (!(flag | flag2));
			col = new Point(value, value2);
			return true;
		}
		catch
		{
			col = new Point(x1, y1);
			return false;
		}
	}

	public static bool AnyWallOfTypeOnLine(int x1, int y1, int x2, int y2, ushort wallId)
	{
		int num = x1;
		int num2 = y1;
		int num3 = x2;
		int num4 = y2;
		if (num <= 1)
		{
			num = 1;
		}
		if (num >= Main.maxTilesX)
		{
			num = Main.maxTilesX - 1;
		}
		if (num3 <= 1)
		{
			num3 = 1;
		}
		if (num3 >= Main.maxTilesX)
		{
			num3 = Main.maxTilesX - 1;
		}
		if (num2 <= 1)
		{
			num2 = 1;
		}
		if (num2 >= Main.maxTilesY - 40)
		{
			num2 = Main.maxTilesY - 40;
		}
		if (num4 <= 1)
		{
			num4 = 1;
		}
		if (num4 >= Main.maxTilesY - 40)
		{
			num4 = Main.maxTilesY - 40;
		}
		float num5 = Math.Abs(num - num3);
		float num6 = Math.Abs(num2 - num4);
		if (num5 == 0f && num6 == 0f)
		{
			return false;
		}
		float num7 = 1f;
		float num8 = 1f;
		if (num5 == 0f || num6 == 0f)
		{
			if (num5 == 0f)
			{
				num7 = 0f;
			}
			if (num6 == 0f)
			{
				num8 = 0f;
			}
		}
		else if (num5 > num6)
		{
			num7 = num5 / num6;
		}
		else
		{
			num8 = num6 / num5;
		}
		float num9 = 0f;
		float num10 = 0f;
		int num11 = 1;
		if (num2 < num4)
		{
			num11 = 2;
		}
		int num12 = (int)num5;
		int num13 = (int)num6;
		int num14 = Math.Sign(num3 - num);
		int num15 = Math.Sign(num4 - num2);
		bool flag = false;
		bool flag2 = false;
		try
		{
			do
			{
				switch (num11)
				{
				case 2:
				{
					num9 += num7;
					int num17 = (int)num9;
					num9 -= (float)num17;
					for (int j = 0; j < num17; j++)
					{
						_ = Main.tile[num, num2];
						if (HitSpecificWallSubstep(num, num2, wallId))
						{
							return true;
						}
						if (num12 == 0 && num13 == 0)
						{
							flag = true;
							break;
						}
						num += num14;
						num12--;
						if (num12 == 0 && num13 == 0 && num17 == 1)
						{
							flag2 = true;
						}
					}
					if (num13 != 0)
					{
						num11 = 1;
					}
					break;
				}
				case 1:
				{
					num10 += num8;
					int num16 = (int)num10;
					num10 -= (float)num16;
					for (int i = 0; i < num16; i++)
					{
						_ = Main.tile[num, num2];
						if (HitSpecificWallSubstep(num, num2, wallId))
						{
							return true;
						}
						if (num12 == 0 && num13 == 0)
						{
							flag = true;
							break;
						}
						num2 += num15;
						num13--;
						if (num12 == 0 && num13 == 0 && num16 == 1)
						{
							flag2 = true;
						}
					}
					if (num12 != 0)
					{
						num11 = 2;
					}
					break;
				}
				}
				if (Main.tile[num, num2] == null)
				{
					return false;
				}
				_ = Main.tile[num, num2];
				if (HitSpecificWallSubstep(num, num2, wallId))
				{
					return true;
				}
			}
			while (!(flag | flag2));
			return false;
		}
		catch
		{
			return false;
		}
	}

	public static bool HitSpecificWallSubstep(int x, int y, ushort wallId)
	{
		if (Main.tile[x, y].wall == wallId)
		{
			return true;
		}
		return false;
	}

	public static Point HitLineWall(int x1, int y1, int x2, int y2)
	{
		//IL_027d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0282: Unknown result type (might be due to invalid IL or missing references)
		//IL_0286: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_023b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0240: Unknown result type (might be due to invalid IL or missing references)
		//IL_025c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0261: Unknown result type (might be due to invalid IL or missing references)
		//IL_0162: Unknown result type (might be due to invalid IL or missing references)
		//IL_0167: Unknown result type (might be due to invalid IL or missing references)
		//IL_0271: Unknown result type (might be due to invalid IL or missing references)
		//IL_0276: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e9: Unknown result type (might be due to invalid IL or missing references)
		int num = x1;
		int num2 = y1;
		int num3 = x2;
		int num4 = y2;
		if (num <= 1)
		{
			num = 1;
		}
		if (num >= Main.maxTilesX)
		{
			num = Main.maxTilesX - 1;
		}
		if (num3 <= 1)
		{
			num3 = 1;
		}
		if (num3 >= Main.maxTilesX)
		{
			num3 = Main.maxTilesX - 1;
		}
		if (num2 <= 1)
		{
			num2 = 1;
		}
		if (num2 >= Main.maxTilesY - 40)
		{
			num2 = Main.maxTilesY - 40;
		}
		if (num4 <= 1)
		{
			num4 = 1;
		}
		if (num4 >= Main.maxTilesY - 40)
		{
			num4 = Main.maxTilesY - 40;
		}
		float num5 = Math.Abs(num - num3);
		float num6 = Math.Abs(num2 - num4);
		if (num5 == 0f && num6 == 0f)
		{
			return new Point(num, num2);
		}
		float num7 = 1f;
		float num8 = 1f;
		if (num5 == 0f || num6 == 0f)
		{
			if (num5 == 0f)
			{
				num7 = 0f;
			}
			if (num6 == 0f)
			{
				num8 = 0f;
			}
		}
		else if (num5 > num6)
		{
			num7 = num5 / num6;
		}
		else
		{
			num8 = num6 / num5;
		}
		float num9 = 0f;
		float num10 = 0f;
		int num11 = 1;
		if (num2 < num4)
		{
			num11 = 2;
		}
		int num12 = (int)num5;
		int num13 = (int)num6;
		int num14 = Math.Sign(num3 - num);
		int num15 = Math.Sign(num4 - num2);
		bool flag = false;
		bool flag2 = false;
		try
		{
			do
			{
				switch (num11)
				{
				case 2:
				{
					num9 += num7;
					int num17 = (int)num9;
					num9 -= (float)num17;
					for (int j = 0; j < num17; j++)
					{
						_ = Main.tile[num, num2];
						if (HitWallSubstep(num, num2))
						{
							return new Point(num, num2);
						}
						if (num12 == 0 && num13 == 0)
						{
							flag = true;
							break;
						}
						num += num14;
						num12--;
						if (num12 == 0 && num13 == 0 && num17 == 1)
						{
							flag2 = true;
						}
					}
					if (num13 != 0)
					{
						num11 = 1;
					}
					break;
				}
				case 1:
				{
					num10 += num8;
					int num16 = (int)num10;
					num10 -= (float)num16;
					for (int i = 0; i < num16; i++)
					{
						_ = Main.tile[num, num2];
						if (HitWallSubstep(num, num2))
						{
							return new Point(num, num2);
						}
						if (num12 == 0 && num13 == 0)
						{
							flag = true;
							break;
						}
						num2 += num15;
						num13--;
						if (num12 == 0 && num13 == 0 && num16 == 1)
						{
							flag2 = true;
						}
					}
					if (num12 != 0)
					{
						num11 = 2;
					}
					break;
				}
				}
				if (Main.tile[num, num2] == null)
				{
					return new Point(-1, -1);
				}
				_ = Main.tile[num, num2];
				if (HitWallSubstep(num, num2))
				{
					return new Point(num, num2);
				}
			}
			while (!(flag | flag2));
			return new Point(num, num2);
		}
		catch
		{
			return new Point(-1, -1);
		}
	}

	public static bool HitWallSubstep(int x, int y)
	{
		if (Main.tile[x, y].wall == 0)
		{
			return false;
		}
		bool flag = false;
		if (Main.wallHouse[Main.tile[x, y].wall])
		{
			flag = true;
		}
		if (!flag)
		{
			for (int i = -1; i < 2; i++)
			{
				for (int j = -1; j < 2; j++)
				{
					if ((i != 0 || j != 0) && Main.tile[x + i, y + j].wall == 0)
					{
						flag = true;
					}
				}
			}
		}
		if (Main.tile[x, y].active() & flag)
		{
			bool flag2 = true;
			for (int k = -1; k < 2; k++)
			{
				for (int l = -1; l < 2; l++)
				{
					if (k != 0 || l != 0)
					{
						Tile tile = Main.tile[x + k, y + l];
						if (!tile.active() || !Main.tileSolid[tile.type] || Main.tileSolidTop[tile.type])
						{
							flag2 = false;
						}
					}
				}
			}
			if (flag2)
			{
				flag = false;
			}
		}
		return flag;
	}

	public static bool EmptyTile(int i, int j, bool ignoreTiles = false)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_0104: Unknown result type (might be due to invalid IL or missing references)
		Rectangle val = new Rectangle(i * 16, j * 16, 16, 16);
		if (Main.tile[i, j].active() && !ignoreTiles)
		{
			return false;
		}
		for (int k = 0; k < 255; k++)
		{
			if (Main.player[k].active && !Main.player[k].dead && !Main.player[k].ghost && val.Intersects(new Rectangle((int)Main.player[k].position.X, (int)Main.player[k].position.Y, Main.player[k].width, Main.player[k].height)))
			{
				return false;
			}
		}
		for (int l = 0; l < Main.maxNPCs; l++)
		{
			if (Main.npc[l].active && val.Intersects(new Rectangle((int)Main.npc[l].position.X, (int)Main.npc[l].position.Y, Main.npc[l].width, Main.npc[l].height)))
			{
				return false;
			}
		}
		return true;
	}

	public static bool DrownCollision(Vector2 Position, int Width, int Height, float gravDir = -1f, bool includeSlopes = false)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0204: Unknown result type (might be due to invalid IL or missing references)
		//IL_020a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0218: Unknown result type (might be due to invalid IL or missing references)
		//IL_0221: Unknown result type (might be due to invalid IL or missing references)
		//IL_0229: Unknown result type (might be due to invalid IL or missing references)
		//IL_022f: Unknown result type (might be due to invalid IL or missing references)
		Vector2 val = new Vector2(Position.X + (float)(Width / 2), Position.Y + (float)(Height / 2));
		int num = 10;
		int num2 = 12;
		if (num > Width)
		{
			num = Width;
		}
		if (num2 > Height)
		{
			num2 = Height;
		}
		val = new Vector2(val.X - (float)(num / 2), Position.Y + -2f);
		if (gravDir == -1f)
		{
			val.Y += Height / 2 - 6;
		}
		int value = (int)(Position.X / 16f) - 1;
		int value2 = (int)((Position.X + (float)Width) / 16f) + 2;
		int value3 = (int)(Position.Y / 16f) - 1;
		int value4 = (int)((Position.Y + (float)Height) / 16f) + 2;
		int num3 = Utils.Clamp(value, 0, Main.maxTilesX - 1);
		value2 = Utils.Clamp(value2, 0, Main.maxTilesX - 1);
		value3 = Utils.Clamp(value3, 0, Main.maxTilesY - 40);
		value4 = Utils.Clamp(value4, 0, Main.maxTilesY - 40);
		int num4 = ((gravDir == 1f) ? value3 : (value4 - 1));
		Vector2 val2 = default;
		for (int i = num3; i < value2; i++)
		{
			for (int j = value3; j < value4; j++)
			{
				Tile tile = Main.tile[i, j];
				if (tile != null && tile.liquid > 0 && !tile.lava() && !tile.shimmer() && (j != num4 || !tile.active() || !Main.tileSolid[tile.type] || Main.tileSolidTop[tile.type] || (includeSlopes && tile.blockType() != 0)))
				{
					val2.X = i * 16;
					val2.Y = j * 16;
					int num5 = 16;
					float num6 = 256 - Main.tile[i, j].liquid;
					num6 /= 32f;
					val2.Y += num6 * 2f;
					num5 -= (int)(num6 * 2f);
					if (val.X + (float)num > val2.X && val.X < val2.X + 16f && val.Y + (float)num2 > val2.Y && val.Y < val2.Y + (float)num5)
					{
						return true;
					}
				}
			}
		}
		return false;
	}

	public static bool IsWorldPointSolid(Vector2 pos, bool treatPlatformsAsNonSolid = false)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_010b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0111: Unknown result type (might be due to invalid IL or missing references)
		//IL_0161: Unknown result type (might be due to invalid IL or missing references)
		//IL_0167: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_011d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0123: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01be: Unknown result type (might be due to invalid IL or missing references)
		//IL_0173: Unknown result type (might be due to invalid IL or missing references)
		//IL_0179: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0132: Unknown result type (might be due to invalid IL or missing references)
		//IL_0138: Unknown result type (might be due to invalid IL or missing references)
		//IL_0188: Unknown result type (might be due to invalid IL or missing references)
		//IL_018e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0146: Unknown result type (might be due to invalid IL or missing references)
		//IL_014c: Unknown result type (might be due to invalid IL or missing references)
		//IL_019a: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a0: Unknown result type (might be due to invalid IL or missing references)
		Point val = pos.ToTileCoordinates();
		if (!WorldGen.InWorld(val.X, val.Y, 1))
		{
			return false;
		}
		Tile tile = Main.tile[val.X, val.Y];
		if (tile == null || !tile.active() || tile.inActive() || !Main.tileSolid[tile.type])
		{
			return false;
		}
		if (treatPlatformsAsNonSolid && tile.type > 0 && tile.type <= TileID.Count && (TileID.Sets.Platforms[tile.type] || tile.type == 380))
		{
			return false;
		}
		int num = tile.blockType();
		switch (num)
		{
		case 0:
			if (pos.X >= (float)(val.X * 16) && pos.X <= (float)(val.X * 16 + 16) && pos.Y >= (float)(val.Y * 16))
			{
				return pos.Y <= (float)(val.Y * 16 + 16);
			}
			return false;
		case 1:
			if (pos.X >= (float)(val.X * 16) && pos.X <= (float)(val.X * 16 + 16) && pos.Y >= (float)(val.Y * 16 + 8))
			{
				return pos.Y <= (float)(val.Y * 16 + 16);
			}
			return false;
		case 2:
		case 3:
		case 4:
		case 5:
		{
			if (pos.X < (float)(val.X * 16) && pos.X > (float)(val.X * 16 + 16) && pos.Y < (float)(val.Y * 16) && pos.Y > (float)(val.Y * 16 + 16))
			{
				return false;
			}
			float num2 = pos.X % 16f;
			float num3 = pos.Y % 16f;
			switch (num)
			{
			case 3:
				return num2 + num3 >= 16f;
			case 2:
				return num3 >= num2;
			case 5:
				return num3 <= num2;
			case 4:
				return num2 + num3 <= 16f;
			}
			break;
		}
		}
		return false;
	}

	public static bool GetWaterLine(Point pt, out float waterLineHeight)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		return GetWaterLine(pt.X, pt.Y, out waterLineHeight);
	}

	public static bool GetWaterLine(int X, int Y, out float waterLineHeight)
	{
		waterLineHeight = 0f;
		if (!WorldGen.InWorld(X, Y, 10))
		{
			return false;
		}
		if (Main.tile[X, Y - 2] == null)
		{
			Main.tile[X, Y - 2] = new Tile();
		}
		if (Main.tile[X, Y - 1] == null)
		{
			Main.tile[X, Y - 1] = new Tile();
		}
		if (Main.tile[X, Y] == null)
		{
			Main.tile[X, Y] = new Tile();
		}
		if (Main.tile[X, Y + 1] == null)
		{
			Main.tile[X, Y + 1] = new Tile();
		}
		if (Main.tile[X, Y - 2].liquid > 0)
		{
			return false;
		}
		if (Main.tile[X, Y - 1].liquid > 0)
		{
			waterLineHeight = Y * 16;
			waterLineHeight -= Main.tile[X, Y - 1].liquid / 16;
			return true;
		}
		if (Main.tile[X, Y].liquid > 0)
		{
			waterLineHeight = (Y + 1) * 16;
			waterLineHeight -= Main.tile[X, Y].liquid / 16;
			return true;
		}
		if (Main.tile[X, Y + 1].liquid > 0)
		{
			waterLineHeight = (Y + 2) * 16;
			waterLineHeight -= Main.tile[X, Y + 1].liquid / 16;
			return true;
		}
		return false;
	}

	public static bool GetWaterLineIterate(Point pt, out float waterLineHeight)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		return GetWaterLineIterate(pt.X, pt.Y, out waterLineHeight);
	}

	public static bool GetWaterLineIterate(int X, int Y, out float waterLineHeight)
	{
		waterLineHeight = 0f;
		while (Y > 0 && Framing.GetTileSafely(X, Y).liquid > 0)
		{
			Y--;
		}
		Y++;
		if (Main.tile[X, Y] == null)
		{
			Main.tile[X, Y] = new Tile();
		}
		if (Main.tile[X, Y].liquid > 0)
		{
			waterLineHeight = Y * 16;
			waterLineHeight -= Main.tile[X, Y - 1].liquid / 16;
			return true;
		}
		return false;
	}

	public static bool WetCollision(Vector2 Position, int Width, int Height)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0184: Unknown result type (might be due to invalid IL or missing references)
		//IL_018d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0198: Unknown result type (might be due to invalid IL or missing references)
		//IL_019e: Unknown result type (might be due to invalid IL or missing references)
		//IL_01af: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0296: Unknown result type (might be due to invalid IL or missing references)
		//IL_029f: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d2: Unknown result type (might be due to invalid IL or missing references)
		honey = false;
		shimmer = false;
		Vector2 val = new Vector2(Position.X + (float)(Width / 2), Position.Y + (float)(Height / 2));
		int num = 10;
		int num2 = Height / 2;
		if (num > Width)
		{
			num = Width;
		}
		if (num2 > Height)
		{
			num2 = Height;
		}
		val = new Vector2(val.X - (float)(num / 2), val.Y - (float)(num2 / 2));
		int value = (int)(Position.X / 16f) - 1;
		int value2 = (int)((Position.X + (float)Width) / 16f) + 2;
		int value3 = (int)(Position.Y / 16f) - 1;
		int value4 = (int)((Position.Y + (float)Height) / 16f) + 2;
		int num3 = Utils.Clamp(value, 0, Main.maxTilesX - 1);
		value2 = Utils.Clamp(value2, 0, Main.maxTilesX - 1);
		value3 = Utils.Clamp(value3, 0, Main.maxTilesY - 40);
		value4 = Utils.Clamp(value4, 0, Main.maxTilesY - 40);
		Vector2 val2 = default;
		for (int i = num3; i < value2; i++)
		{
			for (int j = value3; j < value4; j++)
			{
				if (Main.tile[i, j] == null)
				{
					continue;
				}
				if (Main.tile[i, j].liquid > 0)
				{
					val2.X = i * 16;
					val2.Y = j * 16;
					int num4 = 16;
					float num5 = 256 - Main.tile[i, j].liquid;
					num5 /= 32f;
					val2.Y += num5 * 2f;
					num4 -= (int)(num5 * 2f);
					if (val.X + (float)num > val2.X && val.X < val2.X + 16f && val.Y + (float)num2 > val2.Y && val.Y < val2.Y + (float)num4)
					{
						if (Main.tile[i, j].honey())
						{
							honey = true;
						}
						if (Main.tile[i, j].shimmer())
						{
							shimmer = true;
						}
						return true;
					}
				}
				else
				{
					if (!Main.tile[i, j].active() || Main.tile[i, j].slope() == 0 || j <= 0 || Main.tile[i, j - 1] == null || Main.tile[i, j - 1].liquid <= 0)
					{
						continue;
					}
					val2.X = i * 16;
					val2.Y = j * 16;
					int num6 = 16;
					if (val.X + (float)num > val2.X && val.X < val2.X + 16f && val.Y + (float)num2 > val2.Y && val.Y < val2.Y + (float)num6)
					{
						if (Main.tile[i, j - 1].honey())
						{
							honey = true;
						}
						else if (Main.tile[i, j - 1].shimmer())
						{
							shimmer = true;
						}
						return true;
					}
				}
			}
		}
		return false;
	}

	public static bool LavaCollision(Vector2 Position, int Width, int Height)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_013e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0147: Unknown result type (might be due to invalid IL or missing references)
		//IL_014f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0155: Unknown result type (might be due to invalid IL or missing references)
		//IL_0163: Unknown result type (might be due to invalid IL or missing references)
		//IL_016c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0174: Unknown result type (might be due to invalid IL or missing references)
		//IL_017a: Unknown result type (might be due to invalid IL or missing references)
		int value = (int)(Position.X / 16f) - 1;
		int value2 = (int)((Position.X + (float)Width) / 16f) + 2;
		int value3 = (int)(Position.Y / 16f) - 1;
		int value4 = (int)((Position.Y + (float)Height) / 16f) + 2;
		int num = Utils.Clamp(value, 0, Main.maxTilesX - 1);
		value2 = Utils.Clamp(value2, 0, Main.maxTilesX - 1);
		value3 = Utils.Clamp(value3, 0, Main.maxTilesY - 40);
		value4 = Utils.Clamp(value4, 0, Main.maxTilesY - 40);
		Vector2 val = default;
		for (int i = num; i < value2; i++)
		{
			for (int j = value3; j < value4; j++)
			{
				if (Main.tile[i, j] != null && Main.tile[i, j].liquid > 0 && Main.tile[i, j].lava())
				{
					val.X = i * 16;
					val.Y = j * 16;
					int num2 = 16;
					float num3 = 256 - Main.tile[i, j].liquid;
					num3 /= 32f;
					val.Y += num3 * 2f;
					num2 -= (int)(num3 * 2f);
					if (Position.X + (float)Width > val.X && Position.X < val.X + 16f && Position.Y + (float)Height > val.Y && Position.Y < val.Y + (float)num2)
					{
						return true;
					}
				}
			}
		}
		return false;
	}

	public static Vector4 WalkDownSlope(Vector2 Position, Vector2 Velocity, int Width, int Height, float gravity = 0f)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_034b: Unknown result type (might be due to invalid IL or missing references)
		//IL_034c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0352: Unknown result type (might be due to invalid IL or missing references)
		//IL_0358: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_015d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0164: Unknown result type (might be due to invalid IL or missing references)
		//IL_016b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0174: Unknown result type (might be due to invalid IL or missing references)
		//IL_0303: Unknown result type (might be due to invalid IL or missing references)
		//IL_0309: Unknown result type (might be due to invalid IL or missing references)
		//IL_0313: Unknown result type (might be due to invalid IL or missing references)
		//IL_031c: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0328: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_033e: Unknown result type (might be due to invalid IL or missing references)
		if (Velocity.Y != gravity)
		{
			return new Vector4(Position, Velocity.X, Velocity.Y);
		}
		int value = (int)(Position.X / 16f);
		int value2 = (int)((Position.X + (float)Width) / 16f);
		int value3 = (int)((Position.Y + (float)Height + 4f) / 16f);
		value = Utils.Clamp(value, 0, Main.maxTilesX - 1);
		value2 = Utils.Clamp(value2, 0, Main.maxTilesX - 1);
		value3 = Utils.Clamp(value3, 0, Main.maxTilesY - 2 - 40);
		float num = (value3 + 3) * 16;
		int num2 = -1;
		int num3 = -1;
		int num4 = 1;
		if (Velocity.X < 0f)
		{
			num4 = 2;
		}
		for (int i = value; i <= value2; i++)
		{
			for (int j = value3; j <= value3 + 1; j++)
			{
				if (Main.tile[i, j] == null)
				{
					Main.tile[i, j] = new Tile();
				}
				if (!Main.tile[i, j].nactive() || (!Main.tileSolid[Main.tile[i, j].type] && !Main.tileSolidTop[Main.tile[i, j].type]))
				{
					continue;
				}
				int num5 = j * 16;
				if (Main.tile[i, j].halfBrick())
				{
					num5 += 8;
				}
				Rectangle val = new Rectangle(i * 16, j * 16 - 17, 16, 16);
				if (!val.Intersects(new Rectangle((int)Position.X, (int)Position.Y, Width, Height)) || !((float)num5 <= num))
				{
					continue;
				}
				if (num == (float)num5)
				{
					if (Main.tile[i, j].slope() == 0)
					{
						continue;
					}
					if (num2 != -1 && num3 != -1 && Main.tile[num2, num3] != null && Main.tile[num2, num3].slope() != 0)
					{
						if (Main.tile[i, j].slope() == num4)
						{
							num = num5;
							num2 = i;
							num3 = j;
						}
					}
					else
					{
						num = num5;
						num2 = i;
						num3 = j;
					}
				}
				else
				{
					num = num5;
					num2 = i;
					num3 = j;
				}
			}
		}
		int num6 = num2;
		int num7 = num3;
		if (num2 != -1 && num3 != -1 && Main.tile[num6, num7] != null && Main.tile[num6, num7].slope() > 0)
		{
			int num8 = Main.tile[num6, num7].slope();
			Vector2 val2 = default;
			val2.X = num6 * 16;
			val2.Y = num7 * 16;
			switch (num8)
			{
			case 2:
			{
				float num9 = val2.X + 16f - (Position.X + (float)Width);
				if (Position.Y + (float)Height >= val2.Y + num9 && Velocity.X < 0f)
				{
					Velocity.Y += Math.Abs(Velocity.X);
				}
				break;
			}
			case 1:
			{
				float num9 = Position.X - val2.X;
				if (Position.Y + (float)Height >= val2.Y + num9 && Velocity.X > 0f)
				{
					Velocity.Y += Math.Abs(Velocity.X);
				}
				break;
			}
			}
		}
		return new Vector4(Position, Velocity.X, Velocity.Y);
	}

	public static Vector4 SlopeCollision(Vector2 Position, Vector2 Velocity, int Width, int Height, float gravity = 0f, bool fall = false, bool ignoreAetheriumPlatforms = false)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0667: Unknown result type (might be due to invalid IL or missing references)
		//IL_0668: Unknown result type (might be due to invalid IL or missing references)
		//IL_066a: Unknown result type (might be due to invalid IL or missing references)
		//IL_066b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0670: Unknown result type (might be due to invalid IL or missing references)
		//IL_0672: Unknown result type (might be due to invalid IL or missing references)
		//IL_067c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0681: Unknown result type (might be due to invalid IL or missing references)
		//IL_0683: Unknown result type (might be due to invalid IL or missing references)
		//IL_068a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0710: Unknown result type (might be due to invalid IL or missing references)
		//IL_0717: Unknown result type (might be due to invalid IL or missing references)
		//IL_0693: Unknown result type (might be due to invalid IL or missing references)
		//IL_069a: Unknown result type (might be due to invalid IL or missing references)
		//IL_06a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_0792: Unknown result type (might be due to invalid IL or missing references)
		//IL_0794: Unknown result type (might be due to invalid IL or missing references)
		//IL_079b: Unknown result type (might be due to invalid IL or missing references)
		//IL_07a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0720: Unknown result type (might be due to invalid IL or missing references)
		//IL_0727: Unknown result type (might be due to invalid IL or missing references)
		//IL_0733: Unknown result type (might be due to invalid IL or missing references)
		//IL_0739: Unknown result type (might be due to invalid IL or missing references)
		//IL_06c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0752: Unknown result type (might be due to invalid IL or missing references)
		//IL_06df: Unknown result type (might be due to invalid IL or missing references)
		//IL_076c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0192: Unknown result type (might be due to invalid IL or missing references)
		//IL_019b: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01da: Unknown result type (might be due to invalid IL or missing references)
		//IL_020b: Unknown result type (might be due to invalid IL or missing references)
		//IL_021b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0313: Unknown result type (might be due to invalid IL or missing references)
		//IL_031c: Unknown result type (might be due to invalid IL or missing references)
		//IL_022c: Unknown result type (might be due to invalid IL or missing references)
		//IL_023a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0328: Unknown result type (might be due to invalid IL or missing references)
		//IL_032e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0340: Unknown result type (might be due to invalid IL or missing references)
		//IL_0349: Unknown result type (might be due to invalid IL or missing references)
		//IL_026b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0355: Unknown result type (might be due to invalid IL or missing references)
		//IL_035b: Unknown result type (might be due to invalid IL or missing references)
		//IL_028e: Unknown result type (might be due to invalid IL or missing references)
		//IL_029b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0386: Unknown result type (might be due to invalid IL or missing references)
		//IL_038c: Unknown result type (might be due to invalid IL or missing references)
		//IL_039b: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0441: Unknown result type (might be due to invalid IL or missing references)
		//IL_0447: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_049f: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0450: Unknown result type (might be due to invalid IL or missing references)
		//IL_045f: Unknown result type (might be due to invalid IL or missing references)
		//IL_03db: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0473: Unknown result type (might be due to invalid IL or missing references)
		//IL_0405: Unknown result type (might be due to invalid IL or missing references)
		//IL_0413: Unknown result type (might be due to invalid IL or missing references)
		//IL_041b: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_05da: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_05aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0507: Unknown result type (might be due to invalid IL or missing references)
		//IL_062f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0550: Unknown result type (might be due to invalid IL or missing references)
		//IL_055e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0566: Unknown result type (might be due to invalid IL or missing references)
		stair = false;
		stairFall = false;
		BitsByte bitsByte = (byte)0;
		float y = Position.Y;
		float y2 = Position.Y;
		sloping = false;
		Vector2 val = Position;
		Vector2 val2 = Velocity;
		int value = (int)(Position.X / 16f) - 1;
		int value2 = (int)((Position.X + (float)Width) / 16f) + 2;
		int value3 = (int)(Position.Y / 16f) - 1;
		int value4 = (int)((Position.Y + (float)Height) / 16f) + 2;
		int num = Utils.Clamp(value, 0, Main.maxTilesX - 1);
		value2 = Utils.Clamp(value2, 0, Main.maxTilesX - 1);
		value3 = Utils.Clamp(value3, 0, Main.maxTilesY - 40);
		value4 = Utils.Clamp(value4, 0, Main.maxTilesY - 40);
		Vector2 val3 = default;
		for (int i = num; i < value2; i++)
		{
			for (int j = value3; j < value4; j++)
			{
				Tile tile = Main.tile[i, j];
				if (tile == null || !tile.active() || tile.inActive())
				{
					continue;
				}
				bool flag = Main.tileSolid[tile.type];
				if (Main.tileSolidTop[tile.type] && tile.frameY == 0)
				{
					flag = true;
				}
				if (ignoreAetheriumPlatforms && tile.type == 19 && tile.frameY / 18 == 50)
				{
					flag = false;
				}
				if (!flag)
				{
					continue;
				}
				val3.X = i * 16;
				val3.Y = j * 16;
				int num2 = 16;
				if (Main.tile[i, j].halfBrick())
				{
					val3.Y += 8f;
					num2 -= 8;
				}
				if (!(Position.X + (float)Width > val3.X) || !(Position.X < val3.X + 16f) || !(Position.Y + (float)Height > val3.Y) || !(Position.Y < val3.Y + (float)num2))
				{
					continue;
				}
				bool flag2 = true;
				if (TileID.Sets.Platforms[Main.tile[i, j].type])
				{
					if (Velocity.Y < 0f)
					{
						flag2 = false;
					}
					if (Position.Y + (float)Height < (float)(j * 16) || Position.Y + (float)Height - (1f + Math.Abs(Velocity.X)) > (float)(j * 16 + 16))
					{
						flag2 = false;
					}
					if (((Main.tile[i, j].slope() == 1 && Velocity.X >= 0f) || (Main.tile[i, j].slope() == 2 && Velocity.X <= 0f)) && (Position.Y + (float)Height) / 16f - 1f == (float)j)
					{
						flag2 = false;
					}
				}
				if (!flag2)
				{
					continue;
				}
				bool flag3 = false;
				if (fall && TileID.Sets.Platforms[Main.tile[i, j].type])
				{
					flag3 = true;
				}
				int num3 = Main.tile[i, j].slope();
				val3.X = i * 16;
				val3.Y = j * 16;
				if (!(Position.X + (float)Width > val3.X) || !(Position.X < val3.X + 16f) || !(Position.Y + (float)Height > val3.Y) || !(Position.Y < val3.Y + 16f))
				{
					continue;
				}
				float num4 = 0f;
				if (num3 == 3 || num3 == 4)
				{
					if (num3 == 3)
					{
						num4 = Position.X - val3.X;
					}
					if (num3 == 4)
					{
						num4 = val3.X + 16f - (Position.X + (float)Width);
					}
					if (num4 >= 0f)
					{
						if (Position.Y <= val3.Y + 16f - num4)
						{
							float num5 = val3.Y + 16f - Position.Y - num4;
							if (Position.Y + num5 > y2)
							{
								val.Y = Position.Y + num5;
								y2 = val.Y;
								if (val2.Y < 0.0101f)
								{
									val2.Y = 0.0101f;
								}
								bitsByte[num3] = true;
							}
						}
					}
					else if (Position.Y > val3.Y)
					{
						float num6 = val3.Y + 16f;
						if (val.Y < num6)
						{
							val.Y = num6;
							if (val2.Y < 0.0101f)
							{
								val2.Y = 0.0101f;
							}
						}
					}
				}
				if (num3 != 1 && num3 != 2)
				{
					continue;
				}
				if (num3 == 1)
				{
					num4 = Position.X - val3.X;
				}
				if (num3 == 2)
				{
					num4 = val3.X + 16f - (Position.X + (float)Width);
				}
				if (num4 >= 0f)
				{
					if (!(Position.Y + (float)Height >= val3.Y + num4))
					{
						continue;
					}
					float num7 = val3.Y - (Position.Y + (float)Height) + num4;
					if (!(Position.Y + num7 < y))
					{
						continue;
					}
					if (flag3)
					{
						stairFall = true;
						continue;
					}
					if (TileID.Sets.Platforms[Main.tile[i, j].type])
					{
						stair = true;
					}
					else
					{
						stair = false;
					}
					val.Y = Position.Y + num7;
					y = val.Y;
					if (val2.Y > 0f)
					{
						val2.Y = 0f;
					}
					bitsByte[num3] = true;
					continue;
				}
				if (TileID.Sets.Platforms[Main.tile[i, j].type] && !(Position.Y + (float)Height - 4f - Math.Abs(Velocity.X) <= val3.Y))
				{
					if (flag3)
					{
						stairFall = true;
					}
					continue;
				}
				float num8 = val3.Y - (float)Height;
				if (!(val.Y > num8))
				{
					continue;
				}
				if (flag3)
				{
					stairFall = true;
					continue;
				}
				if (TileID.Sets.Platforms[Main.tile[i, j].type])
				{
					stair = true;
				}
				else
				{
					stair = false;
				}
				val.Y = num8;
				if (val2.Y > 0f)
				{
					val2.Y = 0f;
				}
			}
		}
		Vector2 val4 = val - Position;
		Vector2 val5 = TileCollision(Position, val4, Width, Height);
		if (val5.Y > val4.Y)
		{
			float num9 = val4.Y - val5.Y;
			val.Y = Position.Y + val5.Y;
			if (bitsByte[1])
			{
				val.X = Position.X - num9;
			}
			if (bitsByte[2])
			{
				val.X = Position.X + num9;
			}
			val2.X = 0f;
			val2.Y = 0f;
			up = false;
		}
		else if (val5.Y < val4.Y)
		{
			float num10 = val5.Y - val4.Y;
			val.Y = Position.Y + val5.Y;
			if (bitsByte[3])
			{
				val.X = Position.X - num10;
			}
			if (bitsByte[4])
			{
				val.X = Position.X + num10;
			}
			val2.X = 0f;
			val2.Y = 0f;
		}
		return new Vector4(val, val2.X, val2.Y);
	}

	public static Vector2 noSlopeCollision(Vector2 Position, Vector2 Velocity, int Width, int Height, bool fallThrough = false, bool fall2 = false)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_0190: Unknown result type (might be due to invalid IL or missing references)
		//IL_0199: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_0284: Unknown result type (might be due to invalid IL or missing references)
		//IL_028e: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0237: Unknown result type (might be due to invalid IL or missing references)
		//IL_021d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0360: Unknown result type (might be due to invalid IL or missing references)
		//IL_0367: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_039e: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0332: Unknown result type (might be due to invalid IL or missing references)
		//IL_033e: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0261: Unknown result type (might be due to invalid IL or missing references)
		//IL_0267: Unknown result type (might be due to invalid IL or missing references)
		//IL_0277: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0353: Unknown result type (might be due to invalid IL or missing references)
		up = false;
		down = false;
		Vector2 result = Velocity;
		Vector2 val = Position + Velocity;
		int value = (int)(Position.X / 16f) - 1;
		int value2 = (int)((Position.X + (float)Width) / 16f) + 2;
		int value3 = (int)(Position.Y / 16f) - 1;
		int value4 = (int)((Position.Y + (float)Height) / 16f) + 2;
		int num = -1;
		int num2 = -1;
		int num3 = -1;
		int num4 = -1;
		int num5 = Utils.Clamp(value, 0, Main.maxTilesX - 1);
		value2 = Utils.Clamp(value2, 0, Main.maxTilesX - 1);
		value3 = Utils.Clamp(value3, 0, Main.maxTilesY - 40);
		value4 = Utils.Clamp(value4, 0, Main.maxTilesY - 40);
		float num6 = (value4 + 3) * 16;
		Vector2 val2 = default;
		for (int i = num5; i < value2; i++)
		{
			for (int j = value3; j < value4; j++)
			{
				if (Main.tile[i, j] == null || !Main.tile[i, j].active() || (!Main.tileSolid[Main.tile[i, j].type] && (!Main.tileSolidTop[Main.tile[i, j].type] || Main.tile[i, j].frameY != 0)))
				{
					continue;
				}
				val2.X = i * 16;
				val2.Y = j * 16;
				int num7 = 16;
				if (Main.tile[i, j].halfBrick())
				{
					val2.Y += 8f;
					num7 -= 8;
				}
				if (!(val.X + (float)Width > val2.X) || !(val.X < val2.X + 16f) || !(val.Y + (float)Height > val2.Y) || !(val.Y < val2.Y + (float)num7))
				{
					continue;
				}
				if (Position.Y + (float)Height <= val2.Y)
				{
					down = true;
					if ((!(Main.tileSolidTop[Main.tile[i, j].type] & fallThrough) || !((Velocity.Y <= 1f) | fall2)) && num6 > val2.Y)
					{
						num3 = i;
						num4 = j;
						if (num7 < 16)
						{
							num4++;
						}
						if (num3 != num)
						{
							result.Y = val2.Y - (Position.Y + (float)Height);
							num6 = val2.Y;
						}
					}
				}
				else if (Position.X + (float)Width <= val2.X && !Main.tileSolidTop[Main.tile[i, j].type])
				{
					num = i;
					num2 = j;
					if (num2 != num4)
					{
						result.X = val2.X - (Position.X + (float)Width);
					}
					if (num3 == num)
					{
						result.Y = Velocity.Y;
					}
				}
				else if (Position.X >= val2.X + 16f && !Main.tileSolidTop[Main.tile[i, j].type])
				{
					num = i;
					num2 = j;
					if (num2 != num4)
					{
						result.X = val2.X + 16f - Position.X;
					}
					if (num3 == num)
					{
						result.Y = Velocity.Y;
					}
				}
				else if (Position.Y >= val2.Y + (float)num7 && !Main.tileSolidTop[Main.tile[i, j].type])
				{
					up = true;
					num3 = i;
					num4 = j;
					result.Y = val2.Y + (float)num7 - Position.Y + 0.01f;
					if (num4 == num2)
					{
						result.X = Velocity.X;
					}
				}
			}
		}
		return result;
	}

	public static void BuildTileContacts(Vector2 Position, int Width, int Height, List<TileContact> contactTiles)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_0171: Unknown result type (might be due to invalid IL or missing references)
		//IL_0177: Unknown result type (might be due to invalid IL or missing references)
		//IL_0213: Unknown result type (might be due to invalid IL or missing references)
		//IL_021c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0193: Unknown result type (might be due to invalid IL or missing references)
		//IL_019c: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0232: Unknown result type (might be due to invalid IL or missing references)
		//IL_023b: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0243: Unknown result type (might be due to invalid IL or missing references)
		//IL_0249: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0641: Unknown result type (might be due to invalid IL or missing references)
		//IL_0650: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0301: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01de: Unknown result type (might be due to invalid IL or missing references)
		//IL_065b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0667: Unknown result type (might be due to invalid IL or missing references)
		//IL_0312: Unknown result type (might be due to invalid IL or missing references)
		//IL_031b: Unknown result type (might be due to invalid IL or missing references)
		//IL_032c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0332: Unknown result type (might be due to invalid IL or missing references)
		//IL_025f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0268: Unknown result type (might be due to invalid IL or missing references)
		//IL_0277: Unknown result type (might be due to invalid IL or missing references)
		//IL_027d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0678: Unknown result type (might be due to invalid IL or missing references)
		//IL_067e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0370: Unknown result type (might be due to invalid IL or missing references)
		//IL_0376: Unknown result type (might be due to invalid IL or missing references)
		//IL_03df: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_047c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0482: Unknown result type (might be due to invalid IL or missing references)
		//IL_0522: Unknown result type (might be due to invalid IL or missing references)
		//IL_0528: Unknown result type (might be due to invalid IL or missing references)
		//IL_053b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0541: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_05cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0689: Unknown result type (might be due to invalid IL or missing references)
		//IL_068f: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_0413: Unknown result type (might be due to invalid IL or missing references)
		//IL_0419: Unknown result type (might be due to invalid IL or missing references)
		//IL_042c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0439: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_04bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_04df: Unknown result type (might be due to invalid IL or missing references)
		//IL_0572: Unknown result type (might be due to invalid IL or missing references)
		//IL_057b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0606: Unknown result type (might be due to invalid IL or missing references)
		//IL_060f: Unknown result type (might be due to invalid IL or missing references)
		//IL_044c: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f2: Unknown result type (might be due to invalid IL or missing references)
		Position.X = (int)Position.X;
		Position.Y = (int)Position.Y;
		contactTiles.Clear();
		int value = (int)((Position.X - 1f) / 16f) - 1;
		int value2 = (int)((Position.X + 1f + (float)Width) / 16f) + 1;
		int value3 = (int)((Position.Y - 1f) / 16f) - 1;
		int value4 = (int)((Position.Y + 3f + (float)Height) / 16f) + 1;
		int num = Utils.Clamp(value, 0, Main.maxTilesX - 1);
		value2 = Utils.Clamp(value2, 0, Main.maxTilesX - 1);
		value3 = Utils.Clamp(value3, 0, Main.maxTilesY - 40);
		value4 = Utils.Clamp(value4, 0, Main.maxTilesY - 40);
		Vector2 val = default;
		for (int i = num; i < value2; i++)
		{
			for (int j = value3; j < value4; j++)
			{
				Tile tile = Main.tile[i, j];
				if (tile == null || !tile.active() || tile.inActive())
				{
					continue;
				}
				ushort type = tile.type;
				bool flag = Main.tileSolid[type];
				if (Main.tileSolidTop[type] && tile.frameY == 0)
				{
					flag = true;
				}
				if (!flag)
				{
					continue;
				}
				val.X = i * 16;
				val.Y = j * 16;
				int num2 = 16;
				if (tile.halfBrick())
				{
					val.Y += 8f;
					num2 -= 8;
				}
				byte b = tile.slope();
				if (Math.Abs(Position.X - (val.X + 16f)) < 0.1f && Position.Y + (float)Height > val.Y && Position.Y < val.Y + (float)num2 && b != 3 && b != 1)
				{
					float num3 = Math.Max(Math.Min(Position.Y + (float)Height, val.Y + (float)num2) - Math.Max(Position.Y, val.Y) + 0.5f, 1f);
					contactTiles.Add(new TileContact(TileContactSide.Left, i, j, type, b, (int)num3));
				}
				if (Math.Abs(Position.X + (float)Width - val.X) < 0.1f && Position.Y + (float)Height > val.Y && Position.Y < val.Y + (float)num2 && b != 4 && b != 2)
				{
					float num4 = Math.Max(Math.Min(Position.Y + (float)Height, val.Y + (float)num2) - Math.Max(Position.Y, val.Y) + 0.5f, 1f);
					contactTiles.Add(new TileContact(TileContactSide.Right, i, j, type, b, (int)num4));
				}
				if (!(Position.Y + 3f + (float)Height > val.Y) || !(Position.Y - 1f < val.Y + (float)num2))
				{
					continue;
				}
				if (Position.X + (float)Width > val.X && Position.X < val.X + 16f)
				{
					float num5 = Math.Max(Math.Min(Position.X + (float)Width, val.X + 16f) - Math.Max(Position.X, val.X) + 0.5f, 1f);
					switch (b)
					{
					case 0:
						if (Math.Abs(Position.Y - (val.Y + (float)num2)) < 0.1f)
						{
							contactTiles.Add(new TileContact(TileContactSide.Top, i, j, type, b, (int)num5));
						}
						if (Math.Abs(Position.Y + (float)Height - val.Y) < 0.1f)
						{
							contactTiles.Add(new TileContact(TileContactSide.Bottom, i, j, type, b, (int)num5));
						}
						break;
					case 1:
					{
						if (Math.Abs(Position.Y - (val.Y + (float)num2)) < 0.1f)
						{
							contactTiles.Add(new TileContact(TileContactSide.Top, i, j, type, b, (int)num5));
						}
						float num9 = Math.Max(Position.X - val.X, 0f);
						float num10 = Position.Y + (float)Height;
						if (num10 - val.Y > -0.1f && num10 - (val.Y + num9) < 0.1f)
						{
							contactTiles.Add(new TileContact(TileContactSide.Bottom, i, j, type, b, (int)num5));
						}
						break;
					}
					case 2:
					{
						if (Math.Abs(Position.Y - (val.Y + (float)num2)) < 0.1f)
						{
							contactTiles.Add(new TileContact(TileContactSide.Top, i, j, type, b, (int)num5));
						}
						float num7 = Math.Max(val.X + 16f - (Position.X + (float)Width), 0f);
						float num8 = Position.Y + (float)Height;
						if (num8 - val.Y > -0.1f && num8 - (val.Y + num7) < 0.1f)
						{
							contactTiles.Add(new TileContact(TileContactSide.Bottom, i, j, type, b, (int)num5));
						}
						break;
					}
					case 3:
					{
						float num11 = Math.Max(Position.X - val.X, 0f);
						if (Math.Abs(Position.Y - (val.Y + (float)num2 - num11)) < 0.1f)
						{
							contactTiles.Add(new TileContact(TileContactSide.Top, i, j, type, b, (int)num5));
						}
						if (Math.Abs(Position.Y + (float)Height - val.Y) < 0.1f)
						{
							contactTiles.Add(new TileContact(TileContactSide.Bottom, i, j, type, b, (int)num5));
						}
						break;
					}
					case 4:
					{
						float num6 = Math.Max(val.X + 16f - (Position.X + (float)Width), 0f);
						if (Math.Abs(Position.Y - (val.Y + (float)num2 - num6)) < 0.1f)
						{
							contactTiles.Add(new TileContact(TileContactSide.Top, i, j, type, b, (int)num5));
						}
						if (Math.Abs(Position.Y + (float)Height - val.Y) < 0.1f)
						{
							contactTiles.Add(new TileContact(TileContactSide.Bottom, i, j, type, b, (int)num5));
						}
						break;
					}
					}
				}
				else
				{
					if (!(Position.X + 3f + (float)Width > val.X) || !(Position.X - 3f < val.X + 16f) || !(Position.Y < val.Y))
					{
						continue;
					}
					TileContactSide tileContactSide = ((val.X < Position.X) ? TileContactSide.BottomLeft : TileContactSide.BottomRight);
					switch (b)
					{
					case 0:
					case 3:
					case 4:
						contactTiles.Add(new TileContact(tileContactSide, i, j, type, b, 0));
						break;
					case 1:
						if (tileContactSide == TileContactSide.BottomRight)
						{
							contactTiles.Add(new TileContact(tileContactSide, i, j, type, b, 0));
						}
						break;
					case 2:
						if (tileContactSide == TileContactSide.BottomLeft)
						{
							contactTiles.Add(new TileContact(tileContactSide, i, j, type, b, 0));
						}
						break;
					}
				}
			}
		}
	}

	public static Vector2 TileCollision(Vector2 Position, Vector2 Velocity, int Width, int Height, bool fallThrough = false, bool fall2 = false, int gravDir = 1, bool ignoreDoors = false, bool ignoreAetheriumPlatforms = false, bool hoik = true)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0682: Unknown result type (might be due to invalid IL or missing references)
		//IL_01aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01be: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01de: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_0233: Unknown result type (might be due to invalid IL or missing references)
		//IL_023a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0246: Unknown result type (might be due to invalid IL or missing references)
		//IL_037a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0384: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0302: Unknown result type (might be due to invalid IL or missing references)
		//IL_0279: Unknown result type (might be due to invalid IL or missing references)
		//IL_0280: Unknown result type (might be due to invalid IL or missing references)
		//IL_028c: Unknown result type (might be due to invalid IL or missing references)
		//IL_024e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0255: Unknown result type (might be due to invalid IL or missing references)
		//IL_0433: Unknown result type (might be due to invalid IL or missing references)
		//IL_043d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0336: Unknown result type (might be due to invalid IL or missing references)
		//IL_0340: Unknown result type (might be due to invalid IL or missing references)
		//IL_034c: Unknown result type (might be due to invalid IL or missing references)
		//IL_030e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0315: Unknown result type (might be due to invalid IL or missing references)
		//IL_0297: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0512: Unknown result type (might be due to invalid IL or missing references)
		//IL_0519: Unknown result type (might be due to invalid IL or missing references)
		//IL_03cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0358: Unknown result type (might be due to invalid IL or missing references)
		//IL_0362: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0628: Unknown result type (might be due to invalid IL or missing references)
		//IL_0632: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_03fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0404: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0502: Unknown result type (might be due to invalid IL or missing references)
		//IL_0659: Unknown result type (might be due to invalid IL or missing references)
		//IL_05dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0426: Unknown result type (might be due to invalid IL or missing references)
		up = false;
		down = false;
		Vector2 result = Velocity;
		Vector2 val = Position + Velocity;
		int value = (int)(Position.X / 16f) - 1;
		int value2 = (int)((Position.X + (float)Width) / 16f) + 2;
		int value3 = (int)(Position.Y / 16f) - 1;
		int value4 = (int)((Position.Y + (float)Height) / 16f) + 2;
		int num = -1;
		int num2 = -1;
		int num3 = -1;
		int num4 = -1;
		int num5 = Utils.Clamp(value, 0, Main.maxTilesX - 1);
		value2 = Utils.Clamp(value2, 0, Main.maxTilesX - 1);
		value3 = Utils.Clamp(value3, 0, Main.maxTilesY - 40);
		value4 = Utils.Clamp(value4, 0, Main.maxTilesY - 40);
		float num6 = (value4 + 3) * 16;
		Vector2 val2 = default;
		for (int i = num5; i < value2; i++)
		{
			for (int j = value3; j < value4; j++)
			{
				Tile tile = Main.tile[i, j];
				if (tile == null || !tile.active() || tile.inActive())
				{
					continue;
				}
				bool flag = Main.tileSolid[tile.type];
				if (Main.tileSolidTop[tile.type] && tile.frameY == 0)
				{
					flag = true;
				}
				if (ignoreDoors && TileID.Sets.ForAdvancedCollision.ClosedDoors[tile.type])
				{
					flag = false;
				}
				if (ignoreAetheriumPlatforms && tile.type == 19 && tile.frameY / 18 == 50)
				{
					flag = false;
				}
				if (!flag)
				{
					continue;
				}
				val2.X = i * 16;
				val2.Y = j * 16;
				int num7 = 16;
				if (Main.tile[i, j].halfBrick())
				{
					val2.Y += 8f;
					num7 -= 8;
				}
				if (!(val.X + (float)Width > val2.X) || !(val.X < val2.X + 16f) || !(val.Y + (float)Height > val2.Y) || !(val.Y < val2.Y + (float)num7))
				{
					continue;
				}
				bool flag2 = false;
				bool flag3 = false;
				if (Main.tile[i, j].slope() > 2)
				{
					if (Main.tile[i, j].slope() == 3 && Position.Y + Math.Abs(Velocity.X) >= val2.Y && Position.X >= val2.X)
					{
						flag3 = true;
					}
					if (Main.tile[i, j].slope() == 4 && Position.Y + Math.Abs(Velocity.X) >= val2.Y && Position.X + (float)Width <= val2.X + 16f)
					{
						flag3 = true;
					}
				}
				else if (Main.tile[i, j].slope() > 0)
				{
					flag2 = true;
					if (Main.tile[i, j].slope() == 1 && Position.Y + (float)Height - Math.Abs(Velocity.X) <= val2.Y + (float)num7 && Position.X >= val2.X)
					{
						flag3 = true;
					}
					if (Main.tile[i, j].slope() == 2 && Position.Y + (float)Height - Math.Abs(Velocity.X) <= val2.Y + (float)num7 && Position.X + (float)Width <= val2.X + 16f)
					{
						flag3 = true;
					}
				}
				if (flag3)
				{
					continue;
				}
				if (Position.Y + (float)Height <= val2.Y)
				{
					down = true;
					if ((!(Main.tileSolidTop[Main.tile[i, j].type] & fallThrough) || !((Velocity.Y <= 1f) | fall2)) && num6 > val2.Y)
					{
						num3 = i;
						num4 = j;
						if (num7 < 16)
						{
							num4++;
						}
						if (num3 != num && !flag2)
						{
							result.Y = val2.Y - (Position.Y + (float)Height) + ((gravDir == -1) ? (-0.01f) : 0f);
							num6 = val2.Y;
						}
					}
				}
				else if (Position.X + (float)Width <= val2.X && !Main.tileSolidTop[Main.tile[i, j].type])
				{
					if (i >= 1 && Main.tile[i - 1, j] == null)
					{
						Main.tile[i - 1, j] = new Tile();
					}
					if (!hoik || i < 1 || (Main.tile[i - 1, j].slope() != 2 && Main.tile[i - 1, j].slope() != 4))
					{
						num = i;
						num2 = j;
						if (num2 != num4)
						{
							result.X = val2.X - (Position.X + (float)Width);
						}
						if (num3 == num)
						{
							result.Y = Velocity.Y;
						}
					}
				}
				else if (Position.X >= val2.X + 16f && !Main.tileSolidTop[Main.tile[i, j].type])
				{
					if (Main.tile[i + 1, j] == null)
					{
						Main.tile[i + 1, j] = new Tile();
					}
					if (!hoik || (Main.tile[i + 1, j].slope() != 1 && Main.tile[i + 1, j].slope() != 3))
					{
						num = i;
						num2 = j;
						if (num2 != num4)
						{
							result.X = val2.X + 16f - Position.X;
						}
						if (num3 == num)
						{
							result.Y = Velocity.Y;
						}
					}
				}
				else if (Position.Y >= val2.Y + (float)num7 && !Main.tileSolidTop[Main.tile[i, j].type])
				{
					up = true;
					num3 = i;
					num4 = j;
					result.Y = val2.Y + (float)num7 - Position.Y + ((gravDir == 1) ? 0.01f : 0f);
					if (num4 == num2)
					{
						result.X = Velocity.X;
					}
				}
			}
		}
		return result;
	}

	public static float TileCollisionInStepsOf16(Vector2 Position, Vector2 normalizedDirection, float amount, int Width, int Height)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0104: Unknown result type (might be due to invalid IL or missing references)
		//IL_0106: Unknown result type (might be due to invalid IL or missing references)
		//IL_010b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_011e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0120: Unknown result type (might be due to invalid IL or missing references)
		//IL_012b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0130: Unknown result type (might be due to invalid IL or missing references)
		//IL_0132: Unknown result type (might be due to invalid IL or missing references)
		//IL_0134: Unknown result type (might be due to invalid IL or missing references)
		float num = 16f;
		Vector2 val = new Vector2(Position.X + 1f, Position.Y);
		int width = Width - 1;
		for (float num2 = 0f; num2 < amount; num2 += num)
		{
			Vector2 val2 = normalizedDirection * num2;
			Vector2 position = Position + val2;
			Vector2 val3 = val + val2;
			float num3 = Math.Min(num, amount - num2);
			Vector2 val4 = normalizedDirection * num3;
			Vector4 vec = SlopeCollision(val3, val4, width, Height);
			Vector2 val5;
			if (vec.XY() != val3 || vec.ZW() != val4)
			{
				val5 = vec.XY() - val3;
				float num4 = val5.Length();
				return num2 - num4;
			}
			vec = SlopeCollision(val3 + val4, val4, width, Height);
			if (vec.XY() != val3 + val4 || vec.ZW() != val4)
			{
				val5 = vec.XY() - val3 - val4;
				float num5 = val5.Length();
				return num2 + num3 - num5;
			}
			Vector2 val6 = TileCollision(position, val4, Width, Height);
			if (val6 != val4)
			{
				return num2 + val6.Length();
			}
		}
		return amount;
	}

	public static bool TryChangingSizeFromBottomCenter(Rectangle hitbox, int targetWidth, int targetHeight, out Rectangle changedHitbox)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_010e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0113: Unknown result type (might be due to invalid IL or missing references)
		//IL_0118: Unknown result type (might be due to invalid IL or missing references)
		//IL_011d: Unknown result type (might be due to invalid IL or missing references)
		changedHitbox = hitbox;
		int num = targetHeight - hitbox.Height;
		if (num > 0)
		{
			int num2 = (int)TileCollisionInStepsOf16(changedHitbox.TopLeft(), Vector2.UnitY, num, changedHitbox.Width, changedHitbox.Height);
			int num3 = (int)TileCollisionInStepsOf16(changedHitbox.TopLeft(), -Vector2.UnitY, num, changedHitbox.Width, changedHitbox.Height);
			if (num3 + num2 < num)
			{
				return false;
			}
			int num4 = Math.Min(num, num3);
			int num5 = 0;
			int val = num - num4 - num5;
			num4 += Math.Min(val, num3 - num4);
			val = num - num4 - num5;
			num5 += Math.Min(val, num2 - num5);
			changedHitbox.Offset(0, -num4);
			changedHitbox.Height += num4 + num5;
		}
		else
		{
			changedHitbox.Offset(0, -num);
			changedHitbox.Height = targetHeight;
		}
		int num6 = targetWidth - hitbox.Width;
		if (num6 > 0)
		{
			int num7 = (int)TileCollisionInStepsOf16(changedHitbox.TopLeft(), Vector2.UnitX, num6, changedHitbox.Width, changedHitbox.Height);
			int num8 = (int)TileCollisionInStepsOf16(changedHitbox.TopLeft(), -Vector2.UnitX, num6, changedHitbox.Width, changedHitbox.Height);
			if (num8 + num7 < num6)
			{
				return false;
			}
			int num10;
			int num9 = (num10 = Math.Min(num6 / 2, Math.Min(num8, num7)));
			int val2 = num6 - num10 - num9;
			num10 += Math.Min(val2, num8 - num10);
			val2 = num6 - num10 - num9;
			num9 += Math.Min(val2, num7 - num9);
			changedHitbox.Offset(-num10, 0);
			changedHitbox.Width += num10 + num9;
		}
		else
		{
			changedHitbox.Offset(num6 / 2, 0);
			changedHitbox.Width = targetWidth;
		}
		return true;
	}

	public static bool IsClearSpotTest(Vector2 position, float testMagnitude, int Width, int Height, bool fallThrough = false, bool fall2 = false, int gravDir = 1, bool checkCardinals = true, bool checkSlopes = false)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		//IL_0108: Unknown result type (might be due to invalid IL or missing references)
		//IL_010e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0113: Unknown result type (might be due to invalid IL or missing references)
		//IL_0116: Unknown result type (might be due to invalid IL or missing references)
		//IL_011e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0123: Unknown result type (might be due to invalid IL or missing references)
		//IL_0124: Unknown result type (might be due to invalid IL or missing references)
		//IL_012d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0132: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_013c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0142: Unknown result type (might be due to invalid IL or missing references)
		//IL_0147: Unknown result type (might be due to invalid IL or missing references)
		//IL_014a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0151: Unknown result type (might be due to invalid IL or missing references)
		//IL_0156: Unknown result type (might be due to invalid IL or missing references)
		//IL_0157: Unknown result type (might be due to invalid IL or missing references)
		//IL_0160: Unknown result type (might be due to invalid IL or missing references)
		//IL_0165: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_016f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0174: Unknown result type (might be due to invalid IL or missing references)
		//IL_017a: Unknown result type (might be due to invalid IL or missing references)
		//IL_017f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0182: Unknown result type (might be due to invalid IL or missing references)
		//IL_018a: Unknown result type (might be due to invalid IL or missing references)
		//IL_018f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0190: Unknown result type (might be due to invalid IL or missing references)
		//IL_0199: Unknown result type (might be due to invalid IL or missing references)
		//IL_019e: Unknown result type (might be due to invalid IL or missing references)
		if (checkCardinals)
		{
			Vector2 val = Vector2.UnitX * testMagnitude;
			if (TileCollision(position - val, val, Width, Height, fallThrough, fall2, gravDir) != val)
			{
				return false;
			}
			val = -Vector2.UnitX * testMagnitude;
			if (TileCollision(position - val, val, Width, Height, fallThrough, fall2, gravDir) != val)
			{
				return false;
			}
			val = Vector2.UnitY * testMagnitude;
			if (TileCollision(position - val, val, Width, Height, fallThrough, fall2, gravDir) != val)
			{
				return false;
			}
			val = -Vector2.UnitY * testMagnitude;
			if (TileCollision(position - val, val, Width, Height, fallThrough, fall2, gravDir) != val)
			{
				return false;
			}
		}
		if (checkSlopes)
		{
			Vector2 val = Vector2.UnitX * testMagnitude;
			Vector4 val2 = new Vector4(position, testMagnitude, 0f);
			if (SlopeCollision(position, val, Width, Height, gravDir, fallThrough) != val2)
			{
				return false;
			}
			val = -Vector2.UnitX * testMagnitude;
			val2 = new Vector4(position, 0f - testMagnitude, 0f);
			if (SlopeCollision(position, val, Width, Height, gravDir, fallThrough) != val2)
			{
				return false;
			}
			val = Vector2.UnitY * testMagnitude;
			val2 = new Vector4(position, 0f, testMagnitude);
			if (SlopeCollision(position, val, Width, Height, gravDir, fallThrough) != val2)
			{
				return false;
			}
			val = -Vector2.UnitY * testMagnitude;
			val2 = new Vector4(position, 0f, 0f - testMagnitude);
			if (SlopeCollision(position, val, Width, Height, gravDir, fallThrough) != val2)
			{
				return false;
			}
		}
		return true;
	}

	public static List<Point> FindCollisionTile(int Direction, Vector2 position, float testMagnitude, int Width, int Height, bool fallThrough = false, bool fall2 = false, int gravDir = 1, bool checkCardinals = true, bool checkSlopes = false)
	{
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0295: Unknown result type (might be due to invalid IL or missing references)
		//IL_029b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0283: Unknown result type (might be due to invalid IL or missing references)
		//IL_0288: Unknown result type (might be due to invalid IL or missing references)
		//IL_028e: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0304: Unknown result type (might be due to invalid IL or missing references)
		//IL_0309: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0333: Unknown result type (might be due to invalid IL or missing references)
		//IL_0334: Unknown result type (might be due to invalid IL or missing references)
		//IL_0340: Unknown result type (might be due to invalid IL or missing references)
		//IL_0345: Unknown result type (might be due to invalid IL or missing references)
		//IL_034a: Unknown result type (might be due to invalid IL or missing references)
		//IL_034b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0312: Unknown result type (might be due to invalid IL or missing references)
		//IL_031d: Unknown result type (might be due to invalid IL or missing references)
		//IL_010e: Unknown result type (might be due to invalid IL or missing references)
		//IL_010f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0110: Unknown result type (might be due to invalid IL or missing references)
		//IL_0115: Unknown result type (might be due to invalid IL or missing references)
		//IL_011c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0121: Unknown result type (might be due to invalid IL or missing references)
		//IL_0126: Unknown result type (might be due to invalid IL or missing references)
		//IL_0133: Unknown result type (might be due to invalid IL or missing references)
		//IL_0138: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01db: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_035b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0366: Unknown result type (might be due to invalid IL or missing references)
		//IL_015f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0160: Unknown result type (might be due to invalid IL or missing references)
		//IL_0167: Unknown result type (might be due to invalid IL or missing references)
		//IL_016c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0171: Unknown result type (might be due to invalid IL or missing references)
		//IL_017b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0180: Unknown result type (might be due to invalid IL or missing references)
		//IL_0185: Unknown result type (might be due to invalid IL or missing references)
		//IL_0186: Unknown result type (might be due to invalid IL or missing references)
		//IL_0142: Unknown result type (might be due to invalid IL or missing references)
		//IL_014f: Unknown result type (might be due to invalid IL or missing references)
		//IL_022b: Unknown result type (might be due to invalid IL or missing references)
		//IL_022c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0233: Unknown result type (might be due to invalid IL or missing references)
		//IL_0238: Unknown result type (might be due to invalid IL or missing references)
		//IL_023d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0247: Unknown result type (might be due to invalid IL or missing references)
		//IL_024c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0251: Unknown result type (might be due to invalid IL or missing references)
		//IL_0252: Unknown result type (might be due to invalid IL or missing references)
		//IL_0208: Unknown result type (might be due to invalid IL or missing references)
		//IL_0215: Unknown result type (might be due to invalid IL or missing references)
		//IL_0379: Unknown result type (might be due to invalid IL or missing references)
		//IL_037a: Unknown result type (might be due to invalid IL or missing references)
		//IL_037b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0380: Unknown result type (might be due to invalid IL or missing references)
		//IL_0387: Unknown result type (might be due to invalid IL or missing references)
		//IL_038c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0391: Unknown result type (might be due to invalid IL or missing references)
		//IL_039f: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_043f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0440: Unknown result type (might be due to invalid IL or missing references)
		//IL_0441: Unknown result type (might be due to invalid IL or missing references)
		//IL_0446: Unknown result type (might be due to invalid IL or missing references)
		//IL_044d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0452: Unknown result type (might be due to invalid IL or missing references)
		//IL_0457: Unknown result type (might be due to invalid IL or missing references)
		//IL_0465: Unknown result type (might be due to invalid IL or missing references)
		//IL_046a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0194: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0263: Unknown result type (might be due to invalid IL or missing references)
		//IL_0270: Unknown result type (might be due to invalid IL or missing references)
		//IL_03cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_03cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_03dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_03bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0491: Unknown result type (might be due to invalid IL or missing references)
		//IL_0492: Unknown result type (might be due to invalid IL or missing references)
		//IL_0499: Unknown result type (might be due to invalid IL or missing references)
		//IL_049e: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0473: Unknown result type (might be due to invalid IL or missing references)
		//IL_0481: Unknown result type (might be due to invalid IL or missing references)
		//IL_0400: Unknown result type (might be due to invalid IL or missing references)
		//IL_040e: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d4: Unknown result type (might be due to invalid IL or missing references)
		List<Point> list = new List<Point>();
		if ((uint)Direction > 1u)
		{
			if ((uint)(Direction - 2) <= 1u)
			{
				Vector2 val = ((Direction == 2) ? (Vector2.UnitY * testMagnitude) : (-Vector2.UnitY * testMagnitude));
				Vector4 vec = new Vector4(position, val.X, val.Y);
				int num = (int)(position.Y + (float)((Direction == 2) ? Height : 0)) / 16;
				float num2 = Math.Min(16f - position.X % 16f, Width);
				float num3 = num2;
				if (checkCardinals && TileCollision(position - val, val, (int)num2, Height, fallThrough, fall2, gravDir) != val)
				{
					list.Add(new Point((int)position.X / 16, num));
				}
				else if (checkSlopes && SlopeCollision(position, val, (int)num2, Height, gravDir, fallThrough).YZW() != vec.YZW())
				{
					list.Add(new Point((int)position.X / 16, num));
				}
				for (; num3 + 16f <= (float)(Width - 16); num3 += 16f)
				{
					if (checkCardinals && TileCollision(position - val + Vector2.UnitX * num3, val, 16, Height, fallThrough, fall2, gravDir) != val)
					{
						list.Add(new Point((int)(position.X + num3) / 16, num));
					}
					else if (checkSlopes && SlopeCollision(position + Vector2.UnitX * num3, val, 16, Height, gravDir, fallThrough).YZW() != vec.YZW())
					{
						list.Add(new Point((int)(position.X + num3) / 16, num));
					}
				}
				int width = Width - (int)num3;
				if (checkCardinals && TileCollision(position - val + Vector2.UnitX * num3, val, width, Height, fallThrough, fall2, gravDir) != val)
				{
					list.Add(new Point((int)(position.X + num3) / 16, num));
				}
				else if (checkSlopes && SlopeCollision(position + Vector2.UnitX * num3, val, width, Height, gravDir, fallThrough).YZW() != vec.YZW())
				{
					list.Add(new Point((int)(position.X + num3) / 16, num));
				}
			}
		}
		else
		{
			Vector2 val = ((Direction == 0) ? (Vector2.UnitX * testMagnitude) : (-Vector2.UnitX * testMagnitude));
			Vector4 vec = new Vector4(position, val.X, val.Y);
			int num = (int)(position.X + (float)((Direction == 0) ? Width : 0)) / 16;
			float num4 = Math.Min(16f - position.Y % 16f, Height);
			float num5 = num4;
			if (checkCardinals && TileCollision(position - val, val, Width, (int)num4, fallThrough, fall2, gravDir) != val)
			{
				list.Add(new Point(num, (int)position.Y / 16));
			}
			else if (checkSlopes && SlopeCollision(position, val, Width, (int)num4, gravDir, fallThrough).XZW() != vec.XZW())
			{
				list.Add(new Point(num, (int)position.Y / 16));
			}
			for (; num5 + 16f <= (float)(Height - 16); num5 += 16f)
			{
				if (checkCardinals && TileCollision(position - val + Vector2.UnitY * num5, val, Width, 16, fallThrough, fall2, gravDir) != val)
				{
					list.Add(new Point(num, (int)(position.Y + num5) / 16));
				}
				else if (checkSlopes && SlopeCollision(position + Vector2.UnitY * num5, val, Width, 16, gravDir, fallThrough).XZW() != vec.XZW())
				{
					list.Add(new Point(num, (int)(position.Y + num5) / 16));
				}
			}
			int height = Height - (int)num5;
			if (checkCardinals && TileCollision(position - val + Vector2.UnitY * num5, val, Width, height, fallThrough, fall2, gravDir) != val)
			{
				list.Add(new Point(num, (int)(position.Y + num5) / 16));
			}
			else if (checkSlopes && SlopeCollision(position + Vector2.UnitY * num5, val, Width, height, gravDir, fallThrough).XZW() != vec.XZW())
			{
				list.Add(new Point(num, (int)(position.Y + num5) / 16));
			}
		}
		return list;
	}

	public static bool FindCollisionDirection(out int Direction, Vector2 position, int Width, int Height, bool fallThrough = false, bool fall2 = false, int gravDir = 1)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		Vector2 val = Vector2.UnitX * 16f;
		if (TileCollision(position - val, val, Width, Height, fallThrough, fall2, gravDir) != val)
		{
			Direction = 0;
			return true;
		}
		val = -Vector2.UnitX * 16f;
		if (TileCollision(position - val, val, Width, Height, fallThrough, fall2, gravDir) != val)
		{
			Direction = 1;
			return true;
		}
		val = Vector2.UnitY * 16f;
		if (TileCollision(position - val, val, Width, Height, fallThrough, fall2, gravDir) != val)
		{
			Direction = 2;
			return true;
		}
		val = -Vector2.UnitY * 16f;
		if (TileCollision(position - val, val, Width, Height, fallThrough, fall2, gravDir) != val)
		{
			Direction = 3;
			return true;
		}
		Direction = -1;
		return false;
	}

	public static bool SolidCollision(Vector2 Position, int Width, int Height)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0158: Unknown result type (might be due to invalid IL or missing references)
		//IL_0161: Unknown result type (might be due to invalid IL or missing references)
		//IL_0169: Unknown result type (might be due to invalid IL or missing references)
		//IL_016f: Unknown result type (might be due to invalid IL or missing references)
		//IL_017d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0186: Unknown result type (might be due to invalid IL or missing references)
		//IL_018e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0194: Unknown result type (might be due to invalid IL or missing references)
		int value = (int)(Position.X / 16f) - 1;
		int value2 = (int)((Position.X + (float)Width) / 16f) + 2;
		int value3 = (int)(Position.Y / 16f) - 1;
		int value4 = (int)((Position.Y + (float)Height) / 16f) + 2;
		int num = Utils.Clamp(value, 0, Main.maxTilesX - 1);
		value2 = Utils.Clamp(value2, 0, Main.maxTilesX - 1);
		value3 = Utils.Clamp(value3, 0, Main.maxTilesY - 40);
		value4 = Utils.Clamp(value4, 0, Main.maxTilesY - 40);
		Vector2 val = default;
		for (int i = num; i < value2; i++)
		{
			for (int j = value3; j < value4; j++)
			{
				if (Main.tile[i, j] != null && !Main.tile[i, j].inActive() && Main.tile[i, j].active() && Main.tileSolid[Main.tile[i, j].type] && !Main.tileSolidTop[Main.tile[i, j].type])
				{
					val.X = i * 16;
					val.Y = j * 16;
					int num2 = 16;
					if (Main.tile[i, j].halfBrick())
					{
						val.Y += 8f;
						num2 -= 8;
					}
					if (Position.X + (float)Width > val.X && Position.X < val.X + 16f && Position.Y + (float)Height > val.Y && Position.Y < val.Y + (float)num2)
					{
						return true;
					}
				}
			}
		}
		return false;
	}

	public static bool SolidCollision(Vector2 Position, int Width, int Height, bool acceptTopSurfaces)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_016d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0176: Unknown result type (might be due to invalid IL or missing references)
		//IL_017e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0184: Unknown result type (might be due to invalid IL or missing references)
		//IL_0192: Unknown result type (might be due to invalid IL or missing references)
		//IL_019b: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a9: Unknown result type (might be due to invalid IL or missing references)
		int value = (int)(Position.X / 16f) - 1;
		int value2 = (int)((Position.X + (float)Width) / 16f) + 2;
		int value3 = (int)(Position.Y / 16f) - 1;
		int value4 = (int)((Position.Y + (float)Height) / 16f) + 2;
		int num = Utils.Clamp(value, 0, Main.maxTilesX - 1);
		value2 = Utils.Clamp(value2, 0, Main.maxTilesX - 1);
		value3 = Utils.Clamp(value3, 0, Main.maxTilesY - 40);
		value4 = Utils.Clamp(value4, 0, Main.maxTilesY - 40);
		Vector2 val = default;
		for (int i = num; i < value2; i++)
		{
			for (int j = value3; j < value4; j++)
			{
				Tile tile = Main.tile[i, j];
				if (tile == null || !tile.active() || tile.inActive())
				{
					continue;
				}
				bool flag = Main.tileSolid[tile.type] && !Main.tileSolidTop[tile.type];
				if (acceptTopSurfaces)
				{
					flag = ((!TileID.Sets.Platforms[tile.type]) ? (flag | (Main.tileSolidTop[tile.type] && tile.frameY == 0)) : (flag | WorldGen.PlatformProperTopFrame(tile.frameX)));
				}
				if (flag)
				{
					val.X = i * 16;
					val.Y = j * 16;
					int num2 = 16;
					if (tile.halfBrick())
					{
						val.Y += 8f;
						num2 -= 8;
					}
					if (Position.X + (float)Width > val.X && Position.X < val.X + 16f && Position.Y + (float)Height > val.Y && Position.Y < val.Y + (float)num2)
					{
						return true;
					}
				}
			}
		}
		return false;
	}

	public static Vector2 WaterCollision(Vector2 Position, Vector2 Velocity, int Width, int Height, bool fallThrough = false, bool fall2 = false, bool lavaWalk = true)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		//IL_0004: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0144: Unknown result type (might be due to invalid IL or missing references)
		//IL_014d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0155: Unknown result type (might be due to invalid IL or missing references)
		//IL_015b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0169: Unknown result type (might be due to invalid IL or missing references)
		//IL_0172: Unknown result type (might be due to invalid IL or missing references)
		//IL_017a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0180: Unknown result type (might be due to invalid IL or missing references)
		//IL_018c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0195: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a9: Unknown result type (might be due to invalid IL or missing references)
		Vector2 result = Velocity;
		Vector2 val = Position + Velocity;
		int value = (int)(Position.X / 16f) - 1;
		int value2 = (int)((Position.X + (float)Width) / 16f) + 2;
		int value3 = (int)(Position.Y / 16f) - 1;
		int value4 = (int)((Position.Y + (float)Height) / 16f) + 2;
		int num = Utils.Clamp(value, 0, Main.maxTilesX - 1);
		value2 = Utils.Clamp(value2, 0, Main.maxTilesX - 1);
		value3 = Utils.Clamp(value3, 0, Main.maxTilesY - 40);
		value4 = Utils.Clamp(value4, 0, Main.maxTilesY - 40);
		Vector2 val2 = default;
		for (int i = num; i < value2; i++)
		{
			for (int j = value3; j < value4; j++)
			{
				if (Main.tile[i, j] != null && Main.tile[i, j].liquid > 0 && Main.tile[i, j - 1].liquid == 0 && (!Main.tile[i, j].lava() || lavaWalk))
				{
					int num2 = Main.tile[i, j].liquid / 32 * 2 + 2;
					val2.X = i * 16;
					val2.Y = j * 16 + 16 - num2;
					if (val.X + (float)Width > val2.X && val.X < val2.X + 16f && val.Y + (float)Height > val2.Y && val.Y < val2.Y + (float)num2 && Position.Y + (float)Height <= val2.Y && !fallThrough)
					{
						result.Y = val2.Y - (Position.Y + (float)Height);
					}
				}
			}
		}
		return result;
	}

	public static Vector2 AnyCollisionWithSpecificTiles(Vector2 Position, Vector2 Velocity, int Width, int Height, bool[] tilesWeCanCollideWithByType, bool evenActuated = false)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		//IL_0004: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_0304: Unknown result type (might be due to invalid IL or missing references)
		//IL_012c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0135: Unknown result type (might be due to invalid IL or missing references)
		//IL_0140: Unknown result type (might be due to invalid IL or missing references)
		//IL_0146: Unknown result type (might be due to invalid IL or missing references)
		//IL_0157: Unknown result type (might be due to invalid IL or missing references)
		//IL_0160: Unknown result type (might be due to invalid IL or missing references)
		//IL_016b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0171: Unknown result type (might be due to invalid IL or missing references)
		//IL_0180: Unknown result type (might be due to invalid IL or missing references)
		//IL_018a: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_0222: Unknown result type (might be due to invalid IL or missing references)
		//IL_0229: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_0284: Unknown result type (might be due to invalid IL or missing references)
		//IL_028b: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_0256: Unknown result type (might be due to invalid IL or missing references)
		//IL_0262: Unknown result type (might be due to invalid IL or missing references)
		//IL_0212: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0277: Unknown result type (might be due to invalid IL or missing references)
		Vector2 result = Velocity;
		Vector2 val = Position + Velocity;
		int num = (int)(Position.X / 16f) - 1;
		int num2 = (int)((Position.X + (float)Width) / 16f) + 2;
		int num3 = (int)(Position.Y / 16f) - 1;
		int num4 = (int)((Position.Y + (float)Height) / 16f) + 2;
		int num5 = -1;
		int num6 = -1;
		int num7 = -1;
		int num8 = -1;
		if (num < 0)
		{
			num = 0;
		}
		if (num2 > Main.maxTilesX)
		{
			num2 = Main.maxTilesX;
		}
		if (num3 < 0)
		{
			num3 = 0;
		}
		if (num4 > Main.maxTilesY - 40)
		{
			num4 = Main.maxTilesY - 40;
		}
		Vector2 val2 = default;
		for (int i = num; i < num2; i++)
		{
			for (int j = num3; j < num4; j++)
			{
				Tile tile = Main.tile[i, j];
				if (tile == null || !tile.active() || (!evenActuated && tile.inActive()) || !tilesWeCanCollideWithByType[tile.type])
				{
					continue;
				}
				val2.X = i * 16;
				val2.Y = j * 16;
				int num9 = 16;
				if (tile.halfBrick())
				{
					val2.Y += 8f;
					num9 -= 8;
				}
				if (!(val.X + (float)Width > val2.X) || !(val.X < val2.X + 16f) || !(val.Y + (float)Height > val2.Y) || !(val.Y < val2.Y + (float)num9))
				{
					continue;
				}
				if (Position.Y + (float)Height <= val2.Y)
				{
					num7 = i;
					num8 = j;
					if (num7 != num5)
					{
						result.Y = val2.Y - (Position.Y + (float)Height);
					}
				}
				else if (Position.X + (float)Width <= val2.X && !Main.tileSolidTop[tile.type])
				{
					num5 = i;
					num6 = j;
					if (num6 != num8)
					{
						result.X = val2.X - (Position.X + (float)Width);
					}
					if (num7 == num5)
					{
						result.Y = Velocity.Y;
					}
				}
				else if (Position.X >= val2.X + 16f && !Main.tileSolidTop[tile.type])
				{
					num5 = i;
					num6 = j;
					if (num6 != num8)
					{
						result.X = val2.X + 16f - Position.X;
					}
					if (num7 == num5)
					{
						result.Y = Velocity.Y;
					}
				}
				else if (Position.Y >= val2.Y + (float)num9 && !Main.tileSolidTop[tile.type])
				{
					num7 = i;
					num8 = j;
					result.Y = val2.Y + (float)num9 - Position.Y + 0.01f;
					if (num8 == num6)
					{
						result.X = Velocity.X + 0.01f;
					}
				}
			}
		}
		return result;
	}

	public static Vector2 AnyCollision(Vector2 Position, Vector2 Velocity, int Width, int Height, bool evenActuated = false)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		//IL_0004: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_0339: Unknown result type (might be due to invalid IL or missing references)
		//IL_013d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0146: Unknown result type (might be due to invalid IL or missing references)
		//IL_0151: Unknown result type (might be due to invalid IL or missing references)
		//IL_0157: Unknown result type (might be due to invalid IL or missing references)
		//IL_0168: Unknown result type (might be due to invalid IL or missing references)
		//IL_0171: Unknown result type (might be due to invalid IL or missing references)
		//IL_017c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0182: Unknown result type (might be due to invalid IL or missing references)
		//IL_0191: Unknown result type (might be due to invalid IL or missing references)
		//IL_019b: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01db: Unknown result type (might be due to invalid IL or missing references)
		//IL_023f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0246: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_020e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0214: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_027f: Unknown result type (might be due to invalid IL or missing references)
		//IL_028b: Unknown result type (might be due to invalid IL or missing references)
		//IL_022f: Unknown result type (might be due to invalid IL or missing references)
		//IL_030a: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a0: Unknown result type (might be due to invalid IL or missing references)
		Vector2 result = Velocity;
		Vector2 val = Position + Velocity;
		int num = (int)(Position.X / 16f) - 1;
		int num2 = (int)((Position.X + (float)Width) / 16f) + 2;
		int num3 = (int)(Position.Y / 16f) - 1;
		int num4 = (int)((Position.Y + (float)Height) / 16f) + 2;
		int num5 = -1;
		int num6 = -1;
		int num7 = -1;
		int num8 = -1;
		if (num < 0)
		{
			num = 0;
		}
		if (num2 > Main.maxTilesX)
		{
			num2 = Main.maxTilesX;
		}
		if (num3 < 0)
		{
			num3 = 0;
		}
		if (num4 > Main.maxTilesY - 40)
		{
			num4 = Main.maxTilesY - 40;
		}
		Vector2 val2 = default;
		for (int i = num; i < num2; i++)
		{
			for (int j = num3; j < num4; j++)
			{
				if (Main.tile[i, j] == null || !Main.tile[i, j].active() || (!evenActuated && Main.tile[i, j].inActive()))
				{
					continue;
				}
				val2.X = i * 16;
				val2.Y = j * 16;
				int num9 = 16;
				if (Main.tile[i, j].halfBrick())
				{
					val2.Y += 8f;
					num9 -= 8;
				}
				if (!(val.X + (float)Width > val2.X) || !(val.X < val2.X + 16f) || !(val.Y + (float)Height > val2.Y) || !(val.Y < val2.Y + (float)num9))
				{
					continue;
				}
				if (Position.Y + (float)Height <= val2.Y)
				{
					num7 = i;
					num8 = j;
					if (num7 != num5)
					{
						result.Y = val2.Y - (Position.Y + (float)Height);
					}
				}
				else if (Position.X + (float)Width <= val2.X && !Main.tileSolidTop[Main.tile[i, j].type])
				{
					num5 = i;
					num6 = j;
					if (num6 != num8)
					{
						result.X = val2.X - (Position.X + (float)Width);
					}
					if (num7 == num5)
					{
						result.Y = Velocity.Y;
					}
				}
				else if (Position.X >= val2.X + 16f && !Main.tileSolidTop[Main.tile[i, j].type])
				{
					num5 = i;
					num6 = j;
					if (num6 != num8)
					{
						result.X = val2.X + 16f - Position.X;
					}
					if (num7 == num5)
					{
						result.Y = Velocity.Y;
					}
				}
				else if (Position.Y >= val2.Y + (float)num9 && !Main.tileSolidTop[Main.tile[i, j].type])
				{
					num7 = i;
					num8 = j;
					result.Y = val2.Y + (float)num9 - Position.Y + 0.01f;
					if (num8 == num6)
					{
						result.X = Velocity.X + 0.01f;
					}
				}
			}
		}
		return result;
	}

	public static void HitTilesInACircle(Vector2 Position, Vector2 Velocity, int Width, int Height)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0170: Unknown result type (might be due to invalid IL or missing references)
		//IL_0179: Unknown result type (might be due to invalid IL or missing references)
		//IL_0184: Unknown result type (might be due to invalid IL or missing references)
		//IL_018a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0198: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01af: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f8: Unknown result type (might be due to invalid IL or missing references)
		Vector2 val = Position + Velocity;
		int num = (int)(Position.X / 16f) - 1;
		int num2 = (int)((Position.X + (float)Width) / 16f) + 2;
		int num3 = (int)(Position.Y / 16f) - 1;
		int num4 = (int)((Position.Y + (float)Height) / 16f) + 2;
		if (num < 0)
		{
			num = 0;
		}
		if (num2 > Main.maxTilesX)
		{
			num2 = Main.maxTilesX;
		}
		if (num3 < 0)
		{
			num3 = 0;
		}
		if (num4 > Main.maxTilesY - 40)
		{
			num4 = Main.maxTilesY - 40;
		}
		Vector2 val2 = default;
		for (int i = num; i < num2; i++)
		{
			for (int j = num3; j < num4; j++)
			{
				if (Main.tile[i, j] == null || Main.tile[i, j].inActive() || !Main.tile[i, j].active() || (!Main.tileSolid[Main.tile[i, j].type] && (!Main.tileSolidTop[Main.tile[i, j].type] || Main.tile[i, j].frameY != 0)))
				{
					continue;
				}
				val2.X = i * 16;
				val2.Y = j * 16;
				int num5 = 16;
				if (Main.tile[i, j].halfBrick())
				{
					val2.Y += 8f;
					num5 -= 8;
				}
				if (val.X + (float)Width >= val2.X && val.X <= val2.X + 16f && val.Y + (float)Height >= val2.Y && val.Y <= val2.Y + (float)num5)
				{
					Vector2 val3 = new Vector2(val.X + (float)(Width / 2), val.Y + (float)(Height / 2)) - new Vector2(val2.X + 8f, val2.Y + 8f);
					if (val3.Length() < (float)((Width + Height) / 4))
					{
						WorldGen.KillTile(i, j, fail: true, effectOnly: true);
					}
				}
			}
		}
	}

	public static void HitTiles(Vector2 Position, Vector2 Velocity, int Width, int Height)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0170: Unknown result type (might be due to invalid IL or missing references)
		//IL_0179: Unknown result type (might be due to invalid IL or missing references)
		//IL_0181: Unknown result type (might be due to invalid IL or missing references)
		//IL_0187: Unknown result type (might be due to invalid IL or missing references)
		//IL_0195: Unknown result type (might be due to invalid IL or missing references)
		//IL_019e: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ac: Unknown result type (might be due to invalid IL or missing references)
		Vector2 val = Position + Velocity;
		int num = (int)(Position.X / 16f) - 1;
		int num2 = (int)((Position.X + (float)Width) / 16f) + 2;
		int num3 = (int)(Position.Y / 16f) - 1;
		int num4 = (int)((Position.Y + (float)Height) / 16f) + 2;
		if (num < 0)
		{
			num = 0;
		}
		if (num2 > Main.maxTilesX)
		{
			num2 = Main.maxTilesX;
		}
		if (num3 < 0)
		{
			num3 = 0;
		}
		if (num4 > Main.maxTilesY - 40)
		{
			num4 = Main.maxTilesY - 40;
		}
		Vector2 val2 = default;
		for (int i = num; i < num2; i++)
		{
			for (int j = num3; j < num4; j++)
			{
				if (Main.tile[i, j] != null && !Main.tile[i, j].inActive() && Main.tile[i, j].active() && (Main.tileSolid[Main.tile[i, j].type] || (Main.tileSolidTop[Main.tile[i, j].type] && Main.tile[i, j].frameY == 0)))
				{
					val2.X = i * 16;
					val2.Y = j * 16;
					int num5 = 16;
					if (Main.tile[i, j].halfBrick())
					{
						val2.Y += 8f;
						num5 -= 8;
					}
					if (val.X + (float)Width >= val2.X && val.X <= val2.X + 16f && val.Y + (float)Height >= val2.Y && val.Y <= val2.Y + (float)num5)
					{
						WorldGen.KillTile(i, j, fail: true, effectOnly: true);
					}
				}
			}
		}
	}

	public static bool AnyHurtingTiles(Vector2 Position, int Width, int Height)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		return HurtTiles(Position, Width, Height, null).type >= 0;
	}

	public static HurtTile HurtTiles(Vector2 Position, int Width, int Height, Player player)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0105: Unknown result type (might be due to invalid IL or missing references)
		//IL_0112: Unknown result type (might be due to invalid IL or missing references)
		//IL_011e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0128: Unknown result type (might be due to invalid IL or missing references)
		//IL_013a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0147: Unknown result type (might be due to invalid IL or missing references)
		//IL_0159: Unknown result type (might be due to invalid IL or missing references)
		//IL_0163: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0213: Unknown result type (might be due to invalid IL or missing references)
		//IL_021c: Unknown result type (might be due to invalid IL or missing references)
		int num = (int)(Position.X / 16f) - 1;
		int num2 = (int)((Position.X + (float)Width) / 16f) + 2;
		int num3 = (int)(Position.Y / 16f) - 1;
		int num4 = (int)((Position.Y + (float)Height) / 16f) + 2;
		if (num < 0)
		{
			num = 0;
		}
		if (num2 > Main.maxTilesX)
		{
			num2 = Main.maxTilesX;
		}
		if (num3 < 0)
		{
			num3 = 0;
		}
		if (num4 > Main.maxTilesY - 40)
		{
			num4 = Main.maxTilesY - 40;
		}
		Vector2 val = default;
		for (int i = num; i < num2; i++)
		{
			for (int j = num3; j < num4; j++)
			{
				Tile tile = Main.tile[i, j];
				if (tile == null || tile.inActive() || !tile.active())
				{
					continue;
				}
				val.X = i * 16;
				val.Y = j * 16;
				int num5 = 16;
				if (tile.halfBrick())
				{
					val.Y += 8f;
					num5 -= 8;
				}
				int num6 = 0;
				if (TileID.Sets.Suffocate[tile.type])
				{
					num6 = 2;
				}
				if (Position.X + (float)Width - (float)num6 < val.X || Position.X + (float)num6 > val.X + 16f || Position.Y + (float)Height - (float)num6 < val.Y - 0.5f || Position.Y + (float)num6 > val.Y + (float)num5 + 0.5f || !CanTileHurt(tile.type, i, j, player))
				{
					continue;
				}
				if (tile.slope() > 0)
				{
					if (num6 > 0)
					{
						continue;
					}
					int num7 = 0;
					if (tile.rightSlope() && Position.X > val.X)
					{
						num7++;
					}
					if (tile.leftSlope() && Position.X + (float)Width < val.X + 16f)
					{
						num7++;
					}
					if (tile.bottomSlope() && Position.Y > val.Y)
					{
						num7++;
					}
					if (tile.topSlope() && Position.Y + (float)Height < val.Y + (float)num5)
					{
						num7++;
					}
					if (num7 == 2)
					{
						continue;
					}
				}
				return new HurtTile
				{
					type = tile.type,
					x = i,
					y = j
				};
			}
		}
		return new HurtTile
		{
			type = -1
		};
	}

	public static bool CanTileHurt(ushort type, int i, int j, Player player)
	{
		if (type == 230 && !Main.getGoodWorld)
		{
			return false;
		}
		if (type == 80 && !Main.dontStarveWorld)
		{
			return false;
		}
		if (TileID.Sets.TouchDamageBleeding[type] || TileID.Sets.Suffocate[type] || TileID.Sets.TouchDamageImmediate[type] > 0)
		{
			return true;
		}
		if (TileID.Sets.TouchDamageHot[type] && (player == null || !player.fireWalk))
		{
			return true;
		}
		return false;
	}

	public static bool SwitchTiles(Entity entity, Vector2 Position, int Width, int Height, Vector2 oldPosition, int objType)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_026d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0276: Unknown result type (might be due to invalid IL or missing references)
		//IL_0281: Unknown result type (might be due to invalid IL or missing references)
		//IL_0287: Unknown result type (might be due to invalid IL or missing references)
		//IL_0298: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0204: Unknown result type (might be due to invalid IL or missing references)
		//IL_020a: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0223: Unknown result type (might be due to invalid IL or missing references)
		//IL_022a: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02db: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0302: Unknown result type (might be due to invalid IL or missing references)
		//IL_030a: Unknown result type (might be due to invalid IL or missing references)
		int num = (int)(Position.X / 16f) - 1;
		int num2 = (int)((Position.X + (float)Width) / 16f) + 2;
		int num3 = (int)(Position.Y / 16f) - 1;
		int num4 = (int)((Position.Y + (float)Height) / 16f) + 2;
		if (num < 0)
		{
			num = 0;
		}
		if (num2 > Main.maxTilesX)
		{
			num2 = Main.maxTilesX;
		}
		if (num3 < 0)
		{
			num3 = 0;
		}
		if (num4 > Main.maxTilesY - 40)
		{
			num4 = Main.maxTilesY - 40;
		}
		Vector2 val = default;
		for (int i = num; i < num2; i++)
		{
			for (int j = num3; j < num4; j++)
			{
				if (Main.tile[i, j] == null)
				{
					continue;
				}
				int type = Main.tile[i, j].type;
				if (!Main.tile[i, j].active() || (type != 135 && type != 210 && type != 443 && type != 442))
				{
					continue;
				}
				val.X = i * 16;
				val.Y = j * 16 + 12;
				bool flag = false;
				if (type == 442)
				{
					if (objType == 4)
					{
						float r1StartX = 0f;
						float r1StartY = 0f;
						float r1Width = 0f;
						float r1Height = 0f;
						switch (Main.tile[i, j].frameX / 22)
						{
						case 0:
							r1StartX = i * 16;
							r1StartY = j * 16 + 16 - 10;
							r1Width = 16f;
							r1Height = 10f;
							break;
						case 1:
							r1StartX = i * 16;
							r1StartY = j * 16;
							r1Width = 16f;
							r1Height = 10f;
							break;
						case 2:
							r1StartX = i * 16;
							r1StartY = j * 16;
							r1Width = 10f;
							r1Height = 16f;
							break;
						case 3:
							r1StartX = i * 16 + 16 - 10;
							r1StartY = j * 16;
							r1Width = 10f;
							r1Height = 16f;
							break;
						}
						if (Utils.FloatIntersect(r1StartX, r1StartY, r1Width, r1Height, Position.X, Position.Y, Width, Height) && !Utils.FloatIntersect(r1StartX, r1StartY, r1Width, r1Height, oldPosition.X, oldPosition.Y, Width, Height))
						{
							Wiring.HitSwitch(i, j);
							NetMessage.SendData(59, -1, -1, null, i, j);
							return true;
						}
					}
					flag = true;
				}
				if (flag || !(Position.X + (float)Width > val.X) || !(Position.X < val.X + 16f) || !(Position.Y + (float)Height > val.Y) || !((double)Position.Y < (double)val.Y + 4.01) || (oldPosition.X + (float)Width > val.X && oldPosition.X < val.X + 16f && oldPosition.Y + (float)Height > val.Y && (double)oldPosition.Y < (double)val.Y + 16.01))
				{
					continue;
				}
				switch (type)
				{
				case 210:
					Wiring.HitSwitch(i, j);
					NetMessage.SendData(59, -1, -1, null, i, j);
					continue;
				case 443:
					if (objType == 1 || objType == 5)
					{
						Wiring.HitSwitch(i, j);
						NetMessage.SendData(59, -1, -1, null, i, j);
					}
					continue;
				}
				int num5 = Main.tile[i, j].frameY / 18;
				bool flag2 = true;
				if ((num5 == 4 || num5 == 2 || num5 == 3 || num5 == 6 || num5 == 7) && objType != 5)
				{
					flag2 = false;
				}
				if (num5 == 5 && (objType == 1 || objType == 4 || objType == 5))
				{
					flag2 = false;
				}
				if (!flag2)
				{
					continue;
				}
				if (Main.netMode == 1 && objType == 5)
				{
					NetMessage.SendData(13, -1, -1, null, Main.myPlayer);
				}
				Wiring.HitSwitch(i, j);
				NetMessage.SendData(59, -1, -1, null, i, j);
				if (num5 == 7)
				{
					WorldGen.KillTile(i, j);
					if (Main.netMode == 1)
					{
						NetMessage.SendData(17, -1, -1, null, 0, i, j);
					}
				}
				return true;
			}
		}
		return false;
	}

	public static Vector2 StickyTiles(Vector2 Position, Vector2 Velocity, int Width, int Height)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_038b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0107: Unknown result type (might be due to invalid IL or missing references)
		//IL_0110: Unknown result type (might be due to invalid IL or missing references)
		//IL_011f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0125: Unknown result type (might be due to invalid IL or missing references)
		//IL_013a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0143: Unknown result type (might be due to invalid IL or missing references)
		//IL_027e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0287: Unknown result type (might be due to invalid IL or missing references)
		//IL_014e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0155: Unknown result type (might be due to invalid IL or missing references)
		//IL_0296: Unknown result type (might be due to invalid IL or missing references)
		//IL_029c: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0182: Unknown result type (might be due to invalid IL or missing references)
		//IL_018d: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_035e: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_032d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0345: Unknown result type (might be due to invalid IL or missing references)
		//IL_034b: Unknown result type (might be due to invalid IL or missing references)
		int num = (int)(Position.X / 16f) - 1;
		int num2 = (int)((Position.X + (float)Width) / 16f) + 2;
		int num3 = (int)(Position.Y / 16f) - 1;
		int num4 = (int)((Position.Y + (float)Height) / 16f) + 2;
		if (num < 0)
		{
			num = 0;
		}
		if (num2 > Main.maxTilesX)
		{
			num2 = Main.maxTilesX;
		}
		if (num3 < 0)
		{
			num3 = 0;
		}
		if (num4 > Main.maxTilesY - 40)
		{
			num4 = Main.maxTilesY - 40;
		}
		Vector2 val = default;
		for (int i = num; i < num2; i++)
		{
			for (int j = num3; j < num4; j++)
			{
				if (Main.tile[i, j] == null || !Main.tile[i, j].active() || Main.tile[i, j].inActive())
				{
					continue;
				}
				if (Main.tile[i, j].type == 51)
				{
					int num5 = 0;
					val.X = i * 16;
					val.Y = j * 16;
					if (Position.X + (float)Width > val.X - (float)num5 && Position.X < val.X + 16f + (float)num5 && Position.Y + (float)Height > val.Y && (double)Position.Y < (double)val.Y + 16.01)
					{
						if (Main.tile[i, j].type == 51 && (double)(Math.Abs(Velocity.X) + Math.Abs(Velocity.Y)) > 0.7 && Main.rand.Next(30) == 0)
						{
							Dust.NewDust(new Vector2((float)(i * 16), (float)(j * 16)), 16, 16, 30);
						}
						return new Vector2((float)i, (float)j);
					}
				}
				else
				{
					if (Main.tile[i, j].type != 229 || Main.tile[i, j].slope() != 0)
					{
						continue;
					}
					int num6 = 1;
					val.X = i * 16;
					val.Y = j * 16;
					float num7 = 16.01f;
					if (Main.tile[i, j].halfBrick())
					{
						val.Y += 8f;
						num7 -= 8f;
					}
					if (Position.X + (float)Width > val.X - (float)num6 && Position.X < val.X + 16f + (float)num6 && Position.Y + (float)Height > val.Y && Position.Y < val.Y + num7)
					{
						if (Main.tile[i, j].type == 51 && (double)(Math.Abs(Velocity.X) + Math.Abs(Velocity.Y)) > 0.7 && Main.rand.Next(30) == 0)
						{
							Dust.NewDust(new Vector2((float)(i * 16), (float)(j * 16)), 16, 16, 30);
						}
						return new Vector2((float)i, (float)j);
					}
				}
			}
		}
		return new Vector2(-1f, -1f);
	}

	public static bool SolidTilesVersatile(int startX, int endX, int startY, int endY)
	{
		if (startX > endX)
		{
			Utils.Swap(ref startX, ref endX);
		}
		if (startY > endY)
		{
			Utils.Swap(ref startY, ref endY);
		}
		return SolidTiles(startX, endX, startY, endY);
	}

	public static bool SolidTiles(Vector2 position, int width, int height)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		return SolidTiles((int)(position.X / 16f), (int)((position.X + (float)width) / 16f), (int)(position.Y / 16f), (int)((position.Y + (float)height) / 16f));
	}

	public static bool SolidTiles(int startX, int endX, int startY, int endY)
	{
		if (startX < 0)
		{
			return true;
		}
		if (endX >= Main.maxTilesX)
		{
			return true;
		}
		if (startY < 0)
		{
			return true;
		}
		if (endY >= Main.maxTilesY - 40)
		{
			return true;
		}
		for (int i = startX; i < endX + 1; i++)
		{
			for (int j = startY; j < endY + 1; j++)
			{
				if (Main.tile[i, j] == null)
				{
					return false;
				}
				if (Main.tile[i, j].active() && !Main.tile[i, j].inActive() && Main.tileSolid[Main.tile[i, j].type] && !Main.tileSolidTop[Main.tile[i, j].type])
				{
					return true;
				}
			}
		}
		return false;
	}

	public static bool SolidTiles(Vector2 position, int width, int height, bool allowTopSurfaces)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		return SolidTiles((int)(position.X / 16f), (int)((position.X + (float)width) / 16f), (int)(position.Y / 16f), (int)((position.Y + (float)height) / 16f), allowTopSurfaces);
	}

	public static bool SolidTiles(int startX, int endX, int startY, int endY, bool allowTopSurfaces)
	{
		if (startX < 0)
		{
			return true;
		}
		if (endX >= Main.maxTilesX)
		{
			return true;
		}
		if (startY < 0)
		{
			return true;
		}
		if (endY >= Main.maxTilesY - 40)
		{
			return true;
		}
		for (int i = startX; i < endX + 1; i++)
		{
			for (int j = startY; j < endY + 1; j++)
			{
				Tile tile = Main.tile[i, j];
				if (tile == null)
				{
					return false;
				}
				if (tile.active() && !Main.tile[i, j].inActive())
				{
					ushort type = tile.type;
					bool flag = Main.tileSolid[type] && !Main.tileSolidTop[type];
					if (allowTopSurfaces)
					{
						flag |= Main.tileSolidTop[type] && tile.frameY == 0;
					}
					if (flag)
					{
						return true;
					}
				}
			}
		}
		return false;
	}

	public static bool SolidFullTiles(Vector2 pos, Vector2 size)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		int val = (int)(pos.X / 16f);
		int val2 = (int)(pos.Y / 16f);
		int val3 = (int)Math.Ceiling((pos.X + size.X) / 16f);
		int val4 = (int)Math.Ceiling((pos.Y + size.Y) / 16f);
		int num = Math.Max(val, 0);
		val2 = Math.Max(val2, 0);
		val3 = Math.Min(val3, Main.maxTilesX - 1);
		val4 = Math.Min(val4, Main.maxTilesY - 1);
		for (int i = num; i < val3; i++)
		{
			for (int j = val2; j < val4; j++)
			{
				Tile tile = Main.tile[i, j];
				if (tile != null && tile.nactive() && tile.blockType() == 0 && Main.tileSolid[tile.type] && !Main.tileSolidTop[tile.type])
				{
					return true;
				}
			}
		}
		return false;
	}

	public static void StepDown(ref Vector2 position, ref Vector2 velocity, int width, int height, ref float stepSpeed, ref float gfxOffY, int gravDir = 1, bool waterWalk = false)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_017e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0195: Unknown result type (might be due to invalid IL or missing references)
		Vector2 val = position;
		val.X += velocity.X;
		val.Y = (float)Math.Floor((val.Y + (float)height) / 16f) * 16f - (float)height;
		bool flag = false;
		int num = (int)(val.X / 16f);
		int num2 = (int)((val.X + (float)width) / 16f);
		int num3 = (int)((val.Y + (float)height + 4f) / 16f);
		int num4 = height / 16 + ((height % 16 != 0) ? 1 : 0);
		float num5 = (num3 + num4) * 16;
		float num6 = Main.bottomWorld / 16f - 42f;
		for (int i = num; i <= num2; i++)
		{
			for (int j = num3; j <= num3 + 1; j++)
			{
				if (!WorldGen.InWorld(i, j, 1))
				{
					continue;
				}
				if (Main.tile[i, j] == null)
				{
					Main.tile[i, j] = new Tile();
				}
				if (Main.tile[i, j - 1] == null)
				{
					Main.tile[i, j - 1] = new Tile();
				}
				if (waterWalk && Main.tile[i, j].liquid > 0 && Main.tile[i, j - 1].liquid == 0)
				{
					int num7 = Main.tile[i, j].liquid / 32 * 2 + 2;
					int num8 = j * 16 + 16 - num7;
					Rectangle val2 = new Rectangle(i * 16, j * 16 - 17, 16, 16);
					if (val2.Intersects(new Rectangle((int)position.X, (int)position.Y, width, height)) && (float)num8 < num5)
					{
						num5 = num8;
					}
				}
				if ((float)j >= num6 || (Main.tile[i, j].nactive() && (Main.tileSolid[Main.tile[i, j].type] || Main.tileSolidTop[Main.tile[i, j].type])))
				{
					int num9 = j * 16;
					if (Main.tile[i, j].halfBrick())
					{
						num9 += 8;
					}
					if (Utils.FloatIntersect(i * 16, j * 16 - 17, 16f, 16f, position.X, position.Y, width, height) && (float)num9 < num5)
					{
						num5 = num9;
					}
				}
			}
		}
		float num10 = num5 - (position.Y + (float)height);
		if (num10 > 7f && num10 < 17f && !flag)
		{
			stepSpeed = 1.5f;
			if (num10 > 9f)
			{
				stepSpeed = 2.5f;
			}
			gfxOffY += position.Y + (float)height - num5;
			position.Y = num5 - (float)height;
		}
	}

	public static void StepUp(ref Vector2 position, ref Vector2 velocity, int width, int height, ref float stepSpeed, ref float gfxOffY, int gravDir = 1, bool holdsMatching = false, int specialChecksMode = 0)
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_059d: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_06c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_061d: Unknown result type (might be due to invalid IL or missing references)
		//IL_06cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_062b: Unknown result type (might be due to invalid IL or missing references)
		int num = 0;
		if (velocity.X < 0f)
		{
			num = -1;
		}
		if (velocity.X > 0f)
		{
			num = 1;
		}
		Vector2 val = position;
		val.X += velocity.X;
		int num2 = (int)((val.X + (float)(width / 2) + (float)((width / 2 + 1) * num)) / 16f);
		int num3 = (int)(((double)val.Y + 0.1) / 16.0);
		if (gravDir == 1)
		{
			num3 = (int)((val.Y + (float)height - 1f) / 16f);
		}
		int num4 = height / 16 + ((height % 16 != 0) ? 1 : 0);
		if (!WorldGen.InWorld(num2, num3, 1) || num3 >= Main.maxTilesY - 40)
		{
			return;
		}
		bool flag = true;
		bool flag2 = true;
		if (Main.tile[num2, num3] == null)
		{
			return;
		}
		for (int i = 1; i < num4 + 2; i++)
		{
			if (!WorldGen.InWorld(num2, num3 - i * gravDir) || Main.tile[num2, num3 - i * gravDir] == null)
			{
				return;
			}
		}
		if (!WorldGen.InWorld(num2 - num, num3 - num4 * gravDir) || Main.tile[num2 - num, num3 - num4 * gravDir] == null)
		{
			return;
		}
		Tile tile;
		for (int j = 2; j < num4 + 1; j++)
		{
			if (!WorldGen.InWorld(num2, num3 - j * gravDir) || Main.tile[num2, num3 - j * gravDir] == null)
			{
				return;
			}
			tile = Main.tile[num2, num3 - j * gravDir];
			flag = flag && (!tile.nactive() || !Main.tileSolid[tile.type] || Main.tileSolidTop[tile.type]);
		}
		tile = Main.tile[num2 - num, num3 - num4 * gravDir];
		flag2 = flag2 && (!tile.nactive() || !Main.tileSolid[tile.type] || Main.tileSolidTop[tile.type]);
		bool flag3 = true;
		bool flag4 = true;
		bool flag5 = true;
		Tile tile2;
		if (gravDir == 1)
		{
			if (Main.tile[num2, num3 - gravDir] == null || Main.tile[num2, num3 - (num4 + 1) * gravDir] == null)
			{
				return;
			}
			tile = Main.tile[num2, num3 - gravDir];
			tile2 = Main.tile[num2, num3 - (num4 + 1) * gravDir];
			flag3 = flag3 && (!tile.nactive() || !Main.tileSolid[tile.type] || Main.tileSolidTop[tile.type] || (tile.slope() == 1 && position.X + (float)(width / 2) > (float)(num2 * 16)) || (tile.slope() == 2 && position.X + (float)(width / 2) < (float)(num2 * 16 + 16)) || (tile.halfBrick() && (!tile2.nactive() || !Main.tileSolid[tile2.type] || Main.tileSolidTop[tile2.type])));
			tile = Main.tile[num2, num3];
			tile2 = Main.tile[num2, num3 - 1];
			if (specialChecksMode == 1)
			{
				flag5 = !TileID.Sets.IgnoredByNpcStepUp[tile.type];
			}
			flag4 = flag4 && ((tile.nactive() && (!tile.topSlope() || (tile.slope() == 1 && position.X + (float)(width / 2) < (float)(num2 * 16)) || (tile.slope() == 2 && position.X + (float)(width / 2) > (float)(num2 * 16 + 16))) && (!tile.topSlope() || position.Y + (float)height > (float)(num3 * 16)) && ((Main.tileSolid[tile.type] && !Main.tileSolidTop[tile.type]) || ((holdsMatching && ((Main.tileSolidTop[tile.type] && tile.frameY == 0) || TileID.Sets.Platforms[tile.type] || tile.type == 380) && (!Main.tileSolid[tile2.type] || !tile2.nactive())) & flag5))) || (tile2.halfBrick() && tile2.nactive()));
			flag4 &= !Main.tileSolidTop[tile.type] || !Main.tileSolidTop[tile2.type];
		}
		else
		{
			tile = Main.tile[num2, num3 - gravDir];
			tile2 = Main.tile[num2, num3 - (num4 + 1) * gravDir];
			flag3 = flag3 && (!tile.nactive() || !Main.tileSolid[tile.type] || Main.tileSolidTop[tile.type] || tile.slope() != 0 || (tile.halfBrick() && (!tile2.nactive() || !Main.tileSolid[tile2.type] || Main.tileSolidTop[tile2.type])));
			tile = Main.tile[num2, num3];
			tile2 = Main.tile[num2, num3 + 1];
			flag4 = flag4 && ((tile.nactive() && ((Main.tileSolid[tile.type] && !Main.tileSolidTop[tile.type]) || (holdsMatching && Main.tileSolidTop[tile.type] && tile.frameY == 0 && (!Main.tileSolid[tile2.type] || !tile2.nactive())))) || (tile2.halfBrick() && tile2.nactive()));
		}
		if (!((float)(num2 * 16) < val.X + (float)width) || !((float)(num2 * 16 + 16) > val.X))
		{
			return;
		}
		if (gravDir == 1)
		{
			if (!(flag4 & flag3 & flag & flag2))
			{
				return;
			}
			float num5 = num3 * 16;
			if (Main.tile[num2, num3 - 1].halfBrick())
			{
				num5 -= 8f;
			}
			else if (Main.tile[num2, num3].halfBrick())
			{
				num5 += 8f;
			}
			if (!(num5 < val.Y + (float)height))
			{
				return;
			}
			float num6 = val.Y + (float)height - num5;
			if ((double)num6 <= 16.1)
			{
				gfxOffY += position.Y + (float)height - num5;
				position.Y = num5 - (float)height;
				if (num6 < 9f)
				{
					stepSpeed = 1f;
				}
				else
				{
					stepSpeed = 2f;
				}
			}
		}
		else
		{
			if (!(flag4 & flag3 & flag & flag2) || Main.tile[num2, num3].bottomSlope() || TileID.Sets.Platforms[tile2.type])
			{
				return;
			}
			float num7 = num3 * 16 + 16;
			if (!(num7 > val.Y))
			{
				return;
			}
			float num8 = num7 - val.Y;
			if ((double)num8 <= 16.1)
			{
				gfxOffY -= num7 - position.Y;
				position.Y = num7;
				velocity.Y = 0f;
				if (num8 < 9f)
				{
					stepSpeed = 1f;
				}
				else
				{
					stepSpeed = 2f;
				}
			}
		}
	}

	public static bool InTileBounds(int x, int y, int lx, int ly, int hx, int hy)
	{
		if (x < lx || x > hx || y < ly || y > hy)
		{
			return false;
		}
		return true;
	}

	public static float GetTileRotation(Vector2 position)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		float num = position.Y % 16f;
		int num2 = (int)(position.X / 16f);
		int num3 = (int)(position.Y / 16f);
		Tile tile = Main.tile[num2, num3];
		if (tile == null)
		{
			return 0f;
		}
		bool flag = false;
		for (int num4 = 2; num4 >= 0; num4--)
		{
			if (tile.active())
			{
				if (Main.tileSolid[tile.type])
				{
					int num5 = tile.blockType();
					if (tile.type != 19)
					{
						return num5 switch
						{
							1 => 0f, 
							2 => (float)Math.PI / 4f, 
							3 => -(float)Math.PI / 4f, 
							_ => 0f, 
						};
					}
					int num6 = tile.frameX / 18;
					if (((num6 >= 0 && num6 <= 7) || (num6 >= 12 && num6 <= 16)) && ((num == 0f) | flag))
					{
						return 0f;
					}
					switch (num6)
					{
					case 8:
					case 19:
					case 21:
					case 23:
						return -(float)Math.PI / 4f;
					case 10:
					case 20:
					case 22:
					case 24:
						return (float)Math.PI / 4f;
					case 25:
					case 26:
						if (!flag)
						{
							switch (num5)
							{
							case 2:
								return (float)Math.PI / 4f;
							case 3:
								return -(float)Math.PI / 4f;
							}
							break;
						}
						return 0f;
					}
				}
				else if ((Main.tileSolidTop[tile.type] && tile.frameY == 0) & flag)
				{
					return 0f;
				}
			}
			num3++;
			if (num3 >= Main.maxTilesY)
			{
				return 0f;
			}
			tile = Main.tile[num2, num3];
			if (tile == null)
			{
				return 0f;
			}
			flag = true;
		}
		return 0f;
	}

	public static void GetEntityEdgeTiles(List<Point> p, Entity entity, bool left = true, bool right = true, bool up = true, bool down = true)
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
		int num = (int)entity.position.X;
		int num2 = (int)entity.position.Y;
		_ = num % 16;
		_ = num2 % 16;
		int num3 = (int)entity.Right.X;
		int num4 = (int)entity.Bottom.Y;
		if (num % 16 == 0)
		{
			num--;
		}
		if (num2 % 16 == 0)
		{
			num2--;
		}
		if (num3 % 16 == 0)
		{
			num3++;
		}
		if (num4 % 16 == 0)
		{
			num4++;
		}
		int num5 = num3 / 16 - num / 16;
		int num6 = num4 / 16 - num2 / 16;
		num /= 16;
		num2 /= 16;
		for (int i = num; i <= num + num5; i++)
		{
			if (up)
			{
				p.Add(new Point(i, num2));
			}
			if (down)
			{
				p.Add(new Point(i, num2 + num6));
			}
		}
		for (int j = num2; j < num2 + num6; j++)
		{
			if (left)
			{
				p.Add(new Point(num, j));
			}
			if (right)
			{
				p.Add(new Point(num + num5, j));
			}
		}
	}

	public static bool ApplyConveyorBeltMovementToVelocity(WorldItem item, ref Vector2 velocity)
	{
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_044b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0407: Unknown result type (might be due to invalid IL or missing references)
		//IL_03cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0219: Unknown result type (might be due to invalid IL or missing references)
		//IL_0483: Unknown result type (might be due to invalid IL or missing references)
		//IL_0459: Unknown result type (might be due to invalid IL or missing references)
		//IL_0423: Unknown result type (might be due to invalid IL or missing references)
		//IL_0491: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_06f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0734: Unknown result type (might be due to invalid IL or missing references)
		//IL_070d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0721: Unknown result type (might be due to invalid IL or missing references)
		//IL_076e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0749: Unknown result type (might be due to invalid IL or missing references)
		//IL_075d: Unknown result type (might be due to invalid IL or missing references)
		//IL_07b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0782: Unknown result type (might be due to invalid IL or missing references)
		//IL_07cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0792: Unknown result type (might be due to invalid IL or missing references)
		//IL_07a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_050f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0514: Unknown result type (might be due to invalid IL or missing references)
		//IL_0516: Unknown result type (might be due to invalid IL or missing references)
		//IL_07db: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_0578: Unknown result type (might be due to invalid IL or missing references)
		//IL_057d: Unknown result type (might be due to invalid IL or missing references)
		//IL_057f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0544: Unknown result type (might be due to invalid IL or missing references)
		//IL_083c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0841: Unknown result type (might be due to invalid IL or missing references)
		//IL_0843: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_08cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_08d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_08d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0856: Unknown result type (might be due to invalid IL or missing references)
		//IL_08e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0885: Unknown result type (might be due to invalid IL or missing references)
		//IL_0878: Unknown result type (might be due to invalid IL or missing references)
		//IL_069b: Unknown result type (might be due to invalid IL or missing references)
		//IL_06a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_06a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_06c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0915: Unknown result type (might be due to invalid IL or missing references)
		//IL_0908: Unknown result type (might be due to invalid IL or missing references)
		//IL_090a: Unknown result type (might be due to invalid IL or missing references)
		bool flag = false;
		BuildTileContacts(item.position, item.width, item.height, contacts);
		if (contacts.Count > 0)
		{
			int num = -1;
			int num2 = -1;
			bool flag2 = false;
			TileContactSide tileContactSide = TileContactSide.Top;
			TileContactSide tileContactSide2 = TileContactSide.Top;
			Vector2 zero = Vector2.Zero;
			bool flag3 = false;
			bool flag4 = false;
			bool flag5 = false;
			for (int i = 0; i < contacts.Count; i++)
			{
				int num3 = TileID.Sets.ConveyorDirection[contacts[i].Type];
				switch (contacts[i].Side)
				{
				case TileContactSide.Left:
					zero.Y += num3 * contacts[i].Overlap;
					flag3 |= num3 != 0;
					break;
				case TileContactSide.Right:
					zero.Y += -num3 * contacts[i].Overlap;
					flag3 |= num3 != 0;
					break;
				case TileContactSide.Top:
					zero.X += -num3 * contacts[i].Overlap;
					flag4 = num3 != 0;
					break;
				case TileContactSide.Bottom:
					flag5 = true;
					zero.X += num3 * contacts[i].Overlap;
					flag4 = num3 != 0;
					if (contacts[i].Slope == 1)
					{
						if ((float)(contacts[i].X * 16) < item.position.X)
						{
							Tile tile3 = Main.tile[contacts[i].X, contacts[i].Y - 1];
							if (tile3 != null && tile3.active() && tile3.slope() == 3)
							{
								num2 = i;
							}
						}
					}
					else if (contacts[i].Slope == 2 && (float)(contacts[i].X * 16 + 16) > item.Right.X)
					{
						Tile tile4 = Main.tile[contacts[i].X, contacts[i].Y - 1];
						if (tile4 != null && tile4.active() && tile4.slope() == 4)
						{
							num = i;
						}
					}
					break;
				case TileContactSide.BottomLeft:
					if (num3 == -1)
					{
						int x2 = contacts[i].X;
						int y2 = contacts[i].Y;
						Tile tile2 = Main.tile[x2, y2 - 1];
						byte b2 = tile2.slope();
						if (!tile2.active() || (b2 == 1 && TileID.Sets.ConveyorDirection[tile2.type] == -1))
						{
							tileContactSide = TileContactSide.BottomLeft;
							flag2 = Main.tile[x2, y2].halfBrick();
						}
					}
					if (num3 == 1)
					{
						tileContactSide2 = TileContactSide.BottomLeft;
					}
					break;
				case TileContactSide.BottomRight:
					if (num3 == 1)
					{
						int x = contacts[i].X;
						int y = contacts[i].Y;
						Tile tile = Main.tile[x, y - 1];
						byte b = tile.slope();
						if (!tile.active() || (b == 2 && TileID.Sets.ConveyorDirection[tile.type] == 1))
						{
							tileContactSide = TileContactSide.BottomRight;
							flag2 = Main.tile[x, y].halfBrick();
						}
					}
					if (num3 == -1)
					{
						tileContactSide2 = TileContactSide.BottomRight;
					}
					break;
				}
			}
			if (zero.X < 0f)
			{
				if (zero.X < -8f)
				{
					zero.X = -2.5f;
				}
				else if (zero.X < -4f)
				{
					zero.X = -1.25f;
				}
				else
				{
					zero.X = -0.75f;
				}
			}
			else if (zero.X > 0f)
			{
				if (zero.X > 8f)
				{
					zero.X = 2.5f;
				}
				else if (zero.X > 4f)
				{
					zero.X = 1.25f;
				}
				else
				{
					zero.X = 0.75f;
				}
			}
			if (zero.Y < 0f)
			{
				if (zero.Y < 8f)
				{
					zero.Y = -2.5f;
				}
				else
				{
					zero.Y = -1.25f;
				}
			}
			else if (zero.Y > 0f)
			{
				if (zero.Y > 8f)
				{
					zero.Y = 2.5f;
				}
				else
				{
					zero.Y = 1.25f;
				}
			}
			else if (flag3 && velocity.Y <= 1f)
			{
				velocity.Y = 0f;
			}
			if (zero.Y == 0f)
			{
				if (velocity.Y < 0.11f && tileContactSide != TileContactSide.Top && !flag5)
				{
					if (tileContactSide == TileContactSide.BottomLeft)
					{
						flag = true;
						Vector2 bottom = item.Bottom;
						int num4 = (int)bottom.Y;
						num4 = (num4 + 8) / 16;
						num4 *= 16;
						if (flag2)
						{
							num4 += 8;
						}
						bottom.Y = num4;
						item.Bottom = bottom;
						velocity.Y = 0f;
						velocity.X = -1.25f;
						zero.X = -2.5f;
					}
					if (tileContactSide == TileContactSide.BottomRight)
					{
						flag = true;
						Vector2 bottom2 = item.Bottom;
						int num5 = (int)bottom2.Y;
						num5 = (num5 + 8) / 16;
						num5 *= 16;
						if (flag2)
						{
							num5 += 8;
						}
						bottom2.Y = num5;
						item.Bottom = bottom2;
						velocity.Y = 0f;
						velocity.X = 1.25f;
						zero.X = 2.5f;
					}
				}
				else if ((double)velocity.Y < 0.3 && !flag5)
				{
					switch (tileContactSide2)
					{
					case TileContactSide.BottomRight:
						if (velocity.X <= 0.75f && velocity.X >= -2.5f && !flag3)
						{
							flag = true;
							int num7 = (int)item.position.X;
							num7 = (num7 + 8) / 16;
							num7 *= 16;
							item.position.X = num7;
							velocity.X = 0f;
							velocity.Y = 0.75f;
							zero.Y = 2.5f;
						}
						break;
					case TileContactSide.BottomLeft:
						if (velocity.X >= -0.75f && velocity.X <= 2.5f && !flag3)
						{
							flag = true;
							Vector2 right = item.Right;
							int num6 = (int)right.X;
							num6 = (num6 + 8) / 16;
							num6 *= 16;
							right.X = num6;
							item.Right = right;
							velocity.X = 0f;
							velocity.Y = 0.75f;
							zero.Y = 2.5f;
						}
						break;
					}
				}
			}
			flag |= flag4 | flag3;
			if (zero.Y < 0f)
			{
				velocity.Y = Math.Max(velocity.Y + zero.Y * 6f / 60f, zero.Y);
			}
			else if (zero.Y > 0f)
			{
				velocity.Y = Math.Min(velocity.Y + zero.Y * 6f / 60f, zero.Y);
			}
			if (zero.X < 0f && velocity.X > zero.X)
			{
				velocity.X = Math.Max(velocity.X + zero.X * 6f / 60f, zero.X);
			}
			if (zero.X > 0f && velocity.X < zero.X)
			{
				velocity.X = Math.Min(velocity.X + zero.X * 6f / 60f, zero.X);
			}
			if (num != -1 && velocity.X > 0f)
			{
				int num8 = contacts[num].X * 16 + Math.Max(16 - item.height / 2, 0);
				Vector2 right2 = item.Right;
				if (right2.X + velocity.X > (float)num8)
				{
					if (right2.X >= (float)num8)
					{
						right2.X = num8;
						velocity.X = 0f;
						item.Right = right2;
					}
					else
					{
						velocity.X = (float)num8 - right2.X;
					}
				}
			}
			if (num2 != -1 && velocity.X < 0f)
			{
				int num9 = contacts[num2].X * 16 + Math.Min(item.height / 2, 16);
				Vector2 position = item.position;
				if (position.X + velocity.X < (float)num9)
				{
					if (position.X <= (float)num9)
					{
						position.X = num9;
						velocity.X = 0f;
						item.position = position;
					}
					else
					{
						velocity.X = (float)num9 - position.X;
					}
				}
			}
		}
		return flag;
	}

	public static void StepConveyorBelt(Entity entity, float gravDir, bool artificialRising = false)
	{
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_010f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0116: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_044d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0452: Unknown result type (might be due to invalid IL or missing references)
		//IL_045c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0461: Unknown result type (might be due to invalid IL or missing references)
		//IL_0464: Unknown result type (might be due to invalid IL or missing references)
		//IL_0469: Unknown result type (might be due to invalid IL or missing references)
		//IL_047e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0483: Unknown result type (might be due to invalid IL or missing references)
		//IL_0487: Unknown result type (might be due to invalid IL or missing references)
		//IL_048c: Unknown result type (might be due to invalid IL or missing references)
		//IL_048e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0493: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_04af: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_04de: Unknown result type (might be due to invalid IL or missing references)
		//IL_015d: Unknown result type (might be due to invalid IL or missing references)
		//IL_017b: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_04fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0513: Unknown result type (might be due to invalid IL or missing references)
		//IL_0518: Unknown result type (might be due to invalid IL or missing references)
		//IL_051c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0521: Unknown result type (might be due to invalid IL or missing references)
		//IL_0523: Unknown result type (might be due to invalid IL or missing references)
		//IL_0528: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0205: Unknown result type (might be due to invalid IL or missing references)
		//IL_021b: Unknown result type (might be due to invalid IL or missing references)
		//IL_024d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0273: Unknown result type (might be due to invalid IL or missing references)
		//IL_0291: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_037e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0383: Unknown result type (might be due to invalid IL or missing references)
		//IL_0385: Unknown result type (might be due to invalid IL or missing references)
		//IL_038b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0390: Unknown result type (might be due to invalid IL or missing references)
		//IL_0397: Unknown result type (might be due to invalid IL or missing references)
		//IL_039c: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0349: Unknown result type (might be due to invalid IL or missing references)
		//IL_034e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0350: Unknown result type (might be due to invalid IL or missing references)
		//IL_0356: Unknown result type (might be due to invalid IL or missing references)
		//IL_035b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0362: Unknown result type (might be due to invalid IL or missing references)
		//IL_0367: Unknown result type (might be due to invalid IL or missing references)
		//IL_036c: Unknown result type (might be due to invalid IL or missing references)
		//IL_036e: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0319: Unknown result type (might be due to invalid IL or missing references)
		Player player = null;
		if (entity is Player)
		{
			player = (Player)entity;
			if (Math.Abs(player.gfxOffY) > 2f || player.grapCount > 0 || player.pulley)
			{
				return;
			}
			entity.height -= 5;
			entity.position.Y += 5f;
		}
		int num = 0;
		int num2 = 0;
		bool flag = false;
		int num3 = (int)entity.position.Y + entity.height;
		Rectangle hitbox = entity.Hitbox;
		hitbox.Inflate(2, 2);
		_ = entity.TopLeft;
		_ = entity.TopRight;
		_ = entity.BottomLeft;
		_ = entity.BottomRight;
		List<Point> cacheForConveyorBelts = _cacheForConveyorBelts;
		cacheForConveyorBelts.Clear();
		GetEntityEdgeTiles(cacheForConveyorBelts, entity, left: false, right: false);
		Vector2 val = new Vector2(0.0001f);
		Vector2 lineStart = default;
		Vector2 lineStart2 = default;
		Vector2 lineEnd = default;
		Vector2 lineEnd2 = default;
		for (int i = 0; i < cacheForConveyorBelts.Count; i++)
		{
			Point val2 = cacheForConveyorBelts[i];
			if (!WorldGen.InWorld(val2.X, val2.Y) || (player != null && player.onTrack && val2.Y < num3))
			{
				continue;
			}
			Tile tile = Main.tile[val2.X, val2.Y];
			if (tile == null || !tile.active() || !tile.nactive())
			{
				continue;
			}
			int num4 = TileID.Sets.ConveyorDirection[tile.type];
			if (num4 == 0)
			{
				continue;
			}
			lineStart.X = (lineStart2.X = val2.X * 16);
			lineEnd.X = (lineEnd2.X = val2.X * 16 + 16);
			switch (tile.slope())
			{
			case 1:
				lineStart2.Y = val2.Y * 16;
				lineEnd2.Y = (lineEnd.Y = (lineStart.Y = val2.Y * 16 + 16));
				break;
			case 2:
				lineEnd2.Y = val2.Y * 16;
				lineStart2.Y = (lineEnd.Y = (lineStart.Y = val2.Y * 16 + 16));
				break;
			case 3:
				lineEnd.Y = (lineStart2.Y = (lineEnd2.Y = val2.Y * 16));
				lineStart.Y = val2.Y * 16 + 16;
				break;
			case 4:
				lineStart.Y = (lineStart2.Y = (lineEnd2.Y = val2.Y * 16));
				lineEnd.Y = val2.Y * 16 + 16;
				break;
			default:
				if (tile.halfBrick())
				{
					lineStart2.Y = (lineEnd2.Y = val2.Y * 16 + 8);
				}
				else
				{
					lineStart2.Y = (lineEnd2.Y = val2.Y * 16);
				}
				lineStart.Y = (lineEnd.Y = val2.Y * 16 + 16);
				break;
			}
			int num5 = 0;
			if (!TileID.Sets.Platforms[tile.type] && CheckAABBvLineCollision2(entity.position - val, entity.Size + val * 2f, lineStart, lineEnd))
			{
				num5--;
			}
			if (CheckAABBvLineCollision2(entity.position - val, entity.Size + val * 2f, lineStart2, lineEnd2))
			{
				num5++;
			}
			if (num5 != 0)
			{
				flag = true;
				num += num4 * num5 * (int)gravDir;
				if (tile.leftSlope())
				{
					num2 += (int)gravDir * -num4;
				}
				if (tile.rightSlope())
				{
					num2 -= (int)gravDir * -num4;
				}
			}
		}
		if (entity is Player)
		{
			entity.height += 5;
			entity.position.Y -= 5f;
		}
		if (!flag)
		{
			return;
		}
		if (artificialRising)
		{
			num2 = -1;
		}
		if ((num != 0) | artificialRising)
		{
			num = Math.Sign(num);
			num2 = Math.Sign(num2);
			Vector2 velocity = Vector2.Normalize(new Vector2((float)num * gravDir, (float)num2)) * 2.5f;
			Vector2 val3 = TileCollision(entity.position, velocity, entity.width, entity.height, fallThrough: false, fall2: false, (int)gravDir);
			entity.position += val3;
			if (!artificialRising)
			{
				val3 = TileCollision(Velocity: new Vector2(0f, 2.5f * gravDir), Position: entity.position, Width: entity.width, Height: entity.height, fallThrough: false, fall2: false, gravDir: (int)gravDir);
				entity.position += val3;
			}
			if (artificialRising)
			{
				velocity = new Vector2((float)num, (float)num2);
				val3 = TileCollision(entity.position - velocity, velocity, entity.width, entity.height, fallThrough: false, fall2: false, (int)gravDir);
				entity.position += val3;
			}
		}
	}

	public static bool TryFindingConveyorBeltRising(Entity entity, float gravDir)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		if (entity is WorldItem)
		{
			Point val = (entity.BottomLeft + new Vector2(-0.5f, -0.01f)).ToTileCoordinates();
			float num = 0f;
			if (WorldGen.InWorld(val))
			{
				Tile tile = Main.tile[val.X, val.Y];
				if (tile != null && tile.active() && tile.nactive() && tile.slope() == 0)
				{
					int num2 = TileID.Sets.ConveyorDirection[tile.type];
					num += (float)num2 * gravDir;
				}
			}
			Point val2 = (entity.BottomRight + new Vector2(0.5f, -0.01f)).ToTileCoordinates();
			if (WorldGen.InWorld(val2))
			{
				Tile tile2 = Main.tile[val2.X, val2.Y];
				if (tile2 != null && tile2.active() && tile2.nactive() && tile2.slope() == 0)
				{
					int num3 = TileID.Sets.ConveyorDirection[tile2.type];
					num += (float)num3 * gravDir * -1f;
				}
			}
			return num < 0f;
		}
		return false;
	}

	public static List<Point> GetTilesIn(Vector2 TopLeft, Vector2 BottomRight)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		List<Point> list = new List<Point>();
		Point val = TopLeft.ToTileCoordinates();
		Point val2 = BottomRight.ToTileCoordinates();
		int num = Utils.Clamp(val.X, 0, Main.maxTilesX - 1);
		int num2 = Utils.Clamp(val.Y, 0, Main.maxTilesY - 40);
		int num3 = Utils.Clamp(val2.X, 0, Main.maxTilesX - 1);
		int num4 = Utils.Clamp(val2.Y, 0, Main.maxTilesY - 40);
		for (int i = num; i <= num3; i++)
		{
			for (int j = num2; j <= num4; j++)
			{
				if (Main.tile[i, j] != null)
				{
					list.Add(new Point(i, j));
				}
			}
		}
		return list;
	}

	public static void ExpandVertically(int startX, int startY, out int topY, out int bottomY, int maxExpandUp = 100, int maxExpandDown = 100)
	{
		topY = startY;
		bottomY = startY;
		if (!WorldGen.InWorld(startX, startY, 10))
		{
			return;
		}
		for (int i = 0; i < maxExpandUp; i++)
		{
			if (topY <= 0)
			{
				break;
			}
			if (topY < 10)
			{
				break;
			}
			if (Main.tile[startX, topY] == null)
			{
				break;
			}
			if (WorldGen.SolidTile3(startX, topY))
			{
				break;
			}
			topY--;
		}
		for (int j = 0; j < maxExpandDown; j++)
		{
			if (bottomY >= Main.maxTilesY - 10)
			{
				break;
			}
			if (bottomY > Main.maxTilesY - 10)
			{
				break;
			}
			if (Main.tile[startX, bottomY] == null)
			{
				break;
			}
			if (WorldGen.SolidTile3(startX, bottomY))
			{
				break;
			}
			bottomY++;
		}
	}

	public static Vector2 AdvancedTileCollision(bool[] forcedIgnoredTiles, Vector2 Position, Vector2 Velocity, int Width, int Height, bool fallThrough = false, bool fall2 = false, int gravDir = 1)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0173: Unknown result type (might be due to invalid IL or missing references)
		//IL_017c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0187: Unknown result type (might be due to invalid IL or missing references)
		//IL_018d: Unknown result type (might be due to invalid IL or missing references)
		//IL_019e: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0309: Unknown result type (might be due to invalid IL or missing references)
		//IL_027a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0285: Unknown result type (might be due to invalid IL or missing references)
		//IL_0291: Unknown result type (might be due to invalid IL or missing references)
		//IL_021f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0226: Unknown result type (might be due to invalid IL or missing references)
		//IL_0232: Unknown result type (might be due to invalid IL or missing references)
		//IL_0200: Unknown result type (might be due to invalid IL or missing references)
		//IL_0207: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_029d: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_023d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0247: Unknown result type (might be due to invalid IL or missing references)
		//IL_0472: Unknown result type (might be due to invalid IL or missing references)
		//IL_0479: Unknown result type (might be due to invalid IL or missing references)
		//IL_0346: Unknown result type (might be due to invalid IL or missing references)
		//IL_032c: Unknown result type (might be due to invalid IL or missing references)
		//IL_02dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0537: Unknown result type (might be due to invalid IL or missing references)
		//IL_053e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0569: Unknown result type (might be due to invalid IL or missing references)
		//IL_0573: Unknown result type (might be due to invalid IL or missing references)
		//IL_0377: Unknown result type (might be due to invalid IL or missing references)
		//IL_037d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0441: Unknown result type (might be due to invalid IL or missing references)
		//IL_0447: Unknown result type (might be due to invalid IL or missing references)
		//IL_059a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0509: Unknown result type (might be due to invalid IL or missing references)
		//IL_0515: Unknown result type (might be due to invalid IL or missing references)
		//IL_0462: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_052a: Unknown result type (might be due to invalid IL or missing references)
		up = false;
		down = false;
		Vector2 result = Velocity;
		Vector2 val = Position + Velocity;
		int value = (int)(Position.X / 16f) - 1;
		int value2 = (int)((Position.X + (float)Width) / 16f) + 2;
		int value3 = (int)(Position.Y / 16f) - 1;
		int value4 = (int)((Position.Y + (float)Height) / 16f) + 2;
		int num = -1;
		int num2 = -1;
		int num3 = -1;
		int num4 = -1;
		int num5 = Utils.Clamp(value, 0, Main.maxTilesX - 1);
		value2 = Utils.Clamp(value2, 0, Main.maxTilesX - 1);
		value3 = Utils.Clamp(value3, 0, Main.maxTilesY - 40);
		value4 = Utils.Clamp(value4, 0, Main.maxTilesY - 40);
		float num6 = (value4 + 3) * 16;
		Vector2 val2 = default;
		for (int i = num5; i < value2; i++)
		{
			for (int j = value3; j < value4; j++)
			{
				Tile tile = Main.tile[i, j];
				if (tile == null || !tile.active() || tile.inActive() || forcedIgnoredTiles[tile.type] || (!Main.tileSolid[tile.type] && (!Main.tileSolidTop[tile.type] || tile.frameY != 0)))
				{
					continue;
				}
				val2.X = i * 16;
				val2.Y = j * 16;
				int num7 = 16;
				if (tile.halfBrick())
				{
					val2.Y += 8f;
					num7 -= 8;
				}
				if (!(val.X + (float)Width > val2.X) || !(val.X < val2.X + 16f) || !(val.Y + (float)Height > val2.Y) || !(val.Y < val2.Y + (float)num7))
				{
					continue;
				}
				bool flag = false;
				bool flag2 = false;
				if (tile.slope() > 2)
				{
					if (tile.slope() == 3 && Position.Y + Math.Abs(Velocity.X) >= val2.Y && Position.X >= val2.X)
					{
						flag2 = true;
					}
					if (tile.slope() == 4 && Position.Y + Math.Abs(Velocity.X) >= val2.Y && Position.X + (float)Width <= val2.X + 16f)
					{
						flag2 = true;
					}
				}
				else if (tile.slope() > 0)
				{
					flag = true;
					if (tile.slope() == 1 && Position.Y + (float)Height - Math.Abs(Velocity.X) <= val2.Y + (float)num7 && Position.X >= val2.X)
					{
						flag2 = true;
					}
					if (tile.slope() == 2 && Position.Y + (float)Height - Math.Abs(Velocity.X) <= val2.Y + (float)num7 && Position.X + (float)Width <= val2.X + 16f)
					{
						flag2 = true;
					}
				}
				if (flag2)
				{
					continue;
				}
				if (Position.Y + (float)Height <= val2.Y)
				{
					down = true;
					if ((!(Main.tileSolidTop[tile.type] & fallThrough) || !((Velocity.Y <= 1f) | fall2)) && num6 > val2.Y)
					{
						num3 = i;
						num4 = j;
						if (num7 < 16)
						{
							num4++;
						}
						if (num3 != num && !flag)
						{
							result.Y = val2.Y - (Position.Y + (float)Height) + ((gravDir == -1) ? (-0.01f) : 0f);
							num6 = val2.Y;
						}
					}
				}
				else if (Position.X + (float)Width <= val2.X && !Main.tileSolidTop[tile.type])
				{
					if (Main.tile[i - 1, j] == null)
					{
						Main.tile[i - 1, j] = new Tile();
					}
					if (Main.tile[i - 1, j].slope() != 2 && Main.tile[i - 1, j].slope() != 4)
					{
						num = i;
						num2 = j;
						if (num2 != num4)
						{
							result.X = val2.X - (Position.X + (float)Width);
						}
						if (num3 == num)
						{
							result.Y = Velocity.Y;
						}
					}
				}
				else if (Position.X >= val2.X + 16f && !Main.tileSolidTop[tile.type])
				{
					if (Main.tile[i + 1, j] == null)
					{
						Main.tile[i + 1, j] = new Tile();
					}
					if (Main.tile[i + 1, j].slope() != 1 && Main.tile[i + 1, j].slope() != 3)
					{
						num = i;
						num2 = j;
						if (num2 != num4)
						{
							result.X = val2.X + 16f - Position.X;
						}
						if (num3 == num)
						{
							result.Y = Velocity.Y;
						}
					}
				}
				else if (Position.Y >= val2.Y + (float)num7 && !Main.tileSolidTop[tile.type])
				{
					up = true;
					num3 = i;
					num4 = j;
					result.Y = val2.Y + (float)num7 - Position.Y + ((gravDir == 1) ? 0.01f : 0f);
					if (num4 == num2)
					{
						result.X = Velocity.X;
					}
				}
			}
		}
		return result;
	}

	public static void LaserScan(Vector2 samplingPoint, Vector2 directionUnit, float samplingWidth, float maxDistance, float[] samples)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0108: Unknown result type (might be due to invalid IL or missing references)
		//IL_010d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		for (int i = 0; i < samples.Length; i++)
		{
			float num = (float)i / (float)(samples.Length - 1);
			Vector2 center = default;
			Vector2 val = samplingPoint + directionUnit.RotatedBy(1.5707963705062866, center) * (num - 0.5f) * samplingWidth;
			int num2 = (int)val.X / 16;
			int num3 = (int)val.Y / 16;
			Vector2 val2 = val + directionUnit * maxDistance;
			int num4 = (int)val2.X / 16;
			int num5 = (int)val2.Y / 16;
			float num6 = 0f;
			if (!HitLine(num2, num3, num4, num5, 0, 0, new List<Point>(), out var col))
			{
				center = new Vector2((float)Math.Abs(num2 - col.X), (float)Math.Abs(num3 - col.Y));
				num6 = center.Length() * 16f;
			}
			else if (col.X == num4 && col.Y == num5)
			{
				num6 = maxDistance;
			}
			else
			{
				center = new Vector2((float)Math.Abs(num2 - col.X), (float)Math.Abs(num3 - col.Y));
				num6 = center.Length() * 16f;
			}
			samples[i] = num6;
		}
	}

	public static void AimingLaserScan(Vector2 startPoint, Vector2 endPoint, float samplingWidth, int samplesToTake, out Vector2 vectorTowardsTarget, out float[] samples)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		samples = new float[samplesToTake];
		vectorTowardsTarget = endPoint - startPoint;
		LaserScan(startPoint, vectorTowardsTarget.SafeNormalize(Vector2.Zero), samplingWidth, vectorTowardsTarget.Length(), samples);
	}
}
