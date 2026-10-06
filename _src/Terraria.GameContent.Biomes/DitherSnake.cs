using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using ReLogic.Utilities;
using Terraria.GameContent.Generation.Dungeon;

namespace Terraria.GameContent.Biomes;

public class DitherSnake : List<DungeonControlLine>
{
	private static readonly Vector2D[] CircleTestPoints = Enumerable.Range(0, 12).Select((int i) =>
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		return Vector2D.UnitX.RotatedBy(Math.PI * 2.0 * (double)i / 12.0);
	}).ToArray();

	private static readonly double ExtraBuffer = 1.0 / Math.Cos(Math.PI / 6.0);

	public new void Add(DungeonControlLine line)
	{
		if (Count > 0)
		{
			DungeonControlLine dungeonControlLine = this.Last();
			dungeonControlLine.Next = line;
			line.Prev = dungeonControlLine;
		}
		line.Index = Count;
		base.Add(line);
	}

	public DungeonControlLine GetClosestLineTo(Vector2D pos)
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		DungeonControlLine result = null;
		double num = double.MaxValue;
		using Enumerator enumerator = GetEnumerator();
		while (enumerator.MoveNext())
		{
			DungeonControlLine current = enumerator.Current;
			double num2 = current.Center.Distance(pos);
			if (num2 < num)
			{
				result = current;
				num = num2;
			}
		}
		return result;
	}

	public DungeonControlLine GetLineContaining(Vector2D pos, DungeonControlLine initialGuess = null, int depth = 0)
	{
		//IL_0004: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		if (initialGuess == null)
		{
			initialGuess = GetClosestLineTo(pos);
		}
		if (depth == 3)
		{
			return null;
		}
		if (Vector2D.Dot(pos - initialGuess.Start, initialGuess.StartTangent) < 0.0 && initialGuess.Prev != null)
		{
			return GetLineContaining(pos, initialGuess.Prev, depth + 1);
		}
		if (Vector2D.Dot(pos - initialGuess.End, initialGuess.EndTangent) < 0.0 && initialGuess.Next != null)
		{
			return GetLineContaining(pos, initialGuess.Next, depth + 1);
		}
		return initialGuess;
	}

	public double GetPositionAlongSnake(Vector2D pos)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		DungeonControlLine lineContaining = GetLineContaining(pos);
		if (!lineContaining.CanPaint((int)pos.X, (int)pos.Y, out var _, out var normalizedLineProgress))
		{
			normalizedLineProgress = 0.5;
		}
		return (double)lineContaining.Index + normalizedLineProgress;
	}

	public bool IsCircleInsideBorderWithStyle(DungeonGenerationStyleData style, Vector2D center, int radius)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		DungeonControlLine closestLineTo = GetClosestLineTo(center);
		if (closestLineTo.Style == style)
		{
			return IsCircleInsideBorderWithMatchingStyle(closestLineTo, center, radius);
		}
		return false;
	}

	public bool IsCircleInsideBorderWithMatchingStyle(DungeonControlLine nearbyLine, Vector2D center, int radius)
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		double num = (double)radius * ExtraBuffer;
		Vector2D[] circleTestPoints = CircleTestPoints;
		foreach (Vector2D val in circleTestPoints)
		{
			Vector2D val2 = center + val * num;
			DungeonControlLine lineContaining = GetLineContaining(val2, nearbyLine);
			if (lineContaining == null || lineContaining.Style != nearbyLine.Style)
			{
				return false;
			}
			if (!lineContaining.IsInsideBorder(val2.ToPoint()))
			{
				return false;
			}
		}
		return true;
	}

	public Vector2D GetRoomPositionInsideBorder(DungeonControlLine line, double normalizedDistanceAlong, double normalizedDistanceFrom, int roomRadius, out SnakeOrientation orientation)
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		orientation = SnakeOrientation.Unknown;
		Vector2D target = Vector2D.Lerp(line.Start, line.End, normalizedDistanceAlong);
		Vector2D potentialRoomPosition = line.GetPotentialRoomPosition(normalizedDistanceAlong, -1.0, roomRadius);
		Vector2D potentialRoomPosition2 = line.GetPotentialRoomPosition(normalizedDistanceAlong, 1.0, roomRadius);
		Vector2D target2 = ((potentialRoomPosition.Y < potentialRoomPosition2.Y) ? potentialRoomPosition : potentialRoomPosition2);
		Vector2D target3 = ((potentialRoomPosition.Y > potentialRoomPosition2.Y) ? potentialRoomPosition : potentialRoomPosition2);
		for (int i = 0; i < 4; i++)
		{
			Vector2D potentialRoomPosition3 = line.GetPotentialRoomPosition(normalizedDistanceAlong, normalizedDistanceFrom, roomRadius);
			if (IsCircleInsideBorderWithMatchingStyle(line, potentialRoomPosition3, roomRadius))
			{
				double num = potentialRoomPosition3.Distance(target);
				double num2 = potentialRoomPosition3.Distance(target2);
				double num3 = potentialRoomPosition3.Distance(target3);
				if (num < num2 && num < num3)
				{
					orientation = SnakeOrientation.Center;
				}
				else if (num2 < num3)
				{
					orientation = SnakeOrientation.Top;
				}
				else
				{
					orientation = SnakeOrientation.Bottom;
				}
				return potentialRoomPosition3;
			}
			normalizedDistanceFrom *= 0.8;
		}
		orientation = SnakeOrientation.Center;
		return line.Center;
	}

	public void SetTangents()
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		DungeonControlLine dungeonControlLine = base[0];
		dungeonControlLine.StartTangent = dungeonControlLine.NormalizedLineDirection;
		while (dungeonControlLine.Next != null)
		{
			DungeonControlLine next = dungeonControlLine.Next;
			dungeonControlLine.EndTangent = -(next.StartTangent = (dungeonControlLine.NormalizedLineDirection + next.NormalizedLineDirection).SafeNormalize(default));
			dungeonControlLine = next;
		}
		dungeonControlLine.EndTangent = -dungeonControlLine.NormalizedLineDirection;
	}

	public void AdjustTangentsToPreventSelfIntersection()
	{
		for (int i = 0; i < 100; i++)
		{
			bool flag = false;
			using (Enumerator enumerator = GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.AdjustTangentsToPreventSelfIntersection())
					{
						flag = true;
					}
				}
			}
			if (!flag)
			{
				break;
			}
		}
	}

	public bool IsLineInsideBorder(Vector2D from, Vector2D to, int halfWidth)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		Vector2D val = (to - from).SafeNormalize(Vector2D.UnitX).RotatedBy(Math.PI / 2.0) * (double)halfWidth;
		if (IsLineInsideBorder(from + val, to + val))
		{
			return IsLineInsideBorder(from - val, to - val);
		}
		return false;
	}

	public bool IsLineInsideBorder(Vector2D from, Vector2D to)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		DungeonControlLine line = GetClosestLineTo(from);
		return Utils.PlotLine(from.ToPoint(), to.ToPoint(), (int x, int y) =>
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			line = GetLineContaining(new Vector2D((double)x, (double)y), line);
			return (line != null && line.IsInsideBorder(new Point(x, y))) ? true : false;
		});
	}
}
