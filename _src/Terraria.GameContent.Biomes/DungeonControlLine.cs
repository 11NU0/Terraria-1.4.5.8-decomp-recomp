using System;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using ReLogic.Utilities;
using Terraria.GameContent.Generation.Dungeon;

namespace Terraria.GameContent.Biomes;

public class DungeonControlLine
{
	public int Index;

	public DungeonControlLine Next;

	public DungeonControlLine Prev;

	public Vector2D Start;

	public Vector2D End;

	public Vector2D StartTangent;

	public Vector2D EndTangent;

	public Vector2D StartNormal;

	public Vector2D EndNormal;

	public double CrossTangent;

	public double StartRadius;

	public double EndRadius;

	public static double NormalizedDistanceSafeFromDither;

	private const double StyleTransitionDitherWidth = 0.5;

	private const int BorderWidth = 4;

	public Vector2D NormalizedLineDirection;

	public double LineLength;

	public DungeonGenerationStyleData Style;

	public int ProgressionStage;

	public bool CurveLine;

	public Vector2D Center
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			return (End + Start) / 2.0;
		}
	}

	[JsonConstructor]
	private DungeonControlLine()
	{
	}

	public DungeonControlLine(Vector2D start, Vector2D end, double startRadius, double endRadius, int progressionStage, DungeonGenerationStyleData style)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		Start = start;
		End = end;
		StartRadius = startRadius;
		EndRadius = endRadius;
		ProgressionStage = progressionStage;
		Style = style;
		Vector2D v = End - Start;
		LineLength = v.Length();
		NormalizedLineDirection = v.SafeNormalize(Vector2D.UnitX);
	}

	private void CacheNormals()
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		StartNormal = new Vector2D(StartTangent.Y, 0.0 - StartTangent.X);
		EndNormal = new Vector2D(0.0 - EndTangent.Y, EndTangent.X);
		CrossTangent = Vector2D.Cross(StartTangent, EndTangent);
	}

	public bool CanPaint(int x, int y, out double distance, out double normalizedLineProgress)
	{
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		distance = 0.0;
		normalizedLineProgress = 0.0;
		Vector2D val = new Vector2D((double)x, (double)y);
		Vector2D val2 = val - Start;
		double num = Vector2D.Dot(val2, StartTangent);
		if (num < 0.0)
		{
			if (Prev != null)
			{
				return false;
			}
			normalizedLineProgress = 0.0;
			distance = val2.Length();
			return true;
		}
		Vector2D val3 = val - End;
		double num2 = Vector2D.Dot(val3, EndTangent);
		if (num2 < 0.0)
		{
			if (Next != null)
			{
				return false;
			}
			normalizedLineProgress = 1.0;
			distance = val3.Length();
			return true;
		}
		double num3 = Vector2D.Dot(val2, StartNormal);
		double num4 = Vector2D.Dot(val3, EndNormal);
		double num5 = (num + num2) / 2.0;
		num *= num;
		num2 *= num2;
		double num6 = num / (num + num2);
		double value = num3 * (1.0 - num6) + num4 * num6 - num5 * CrossTangent * num6 * (1.0 - num6);
		distance = Math.Abs(value);
		normalizedLineProgress = num6;
		return true;
	}

	public void Paint(int x, int y)
	{
		if (!CanPaint(x, y, out var distance, out var normalizedLineProgress))
		{
			return;
		}
		double num = Utils.Lerp(StartRadius, EndRadius, normalizedLineProgress);
		double num2 = distance / num;
		if (num2 > 1.0)
		{
			return;
		}
		DungeonGenerationStyleData styleWithDitheredTransition = GetStyleWithDitheredTransition(x, y, normalizedLineProgress);
		if (SkipPaintForEdge(x, y, styleWithDitheredTransition, num2))
		{
			return;
		}
		Tile tile = Main.tile[x, y];
		tile.ClearEverything();
		tile.active(active: true);
		tile.type = styleWithDitheredTransition.BrickTileType;
		tile.wall = styleWithDitheredTransition.BrickWallType;
		if (styleWithDitheredTransition.UnbreakableWallProgressionTier <= DualDungeonUnbreakableWallTiers.EarlyGame)
		{
			return;
		}
		int num3 = (int)(num * NormalizedDistanceSafeFromDither);
		double num4 = distance - (double)num3;
		if (num4 >= -4.0 && num4 <= 0.0)
		{
			int num5 = styleWithDitheredTransition.UnbreakableWallProgressionTier;
			if (num4 <= -2.0)
			{
				num5 += 16;
			}
			tile.wall = 350;
			tile.wallColor((byte)num5);
		}
	}

	public DungeonGenerationStyleData GetStyleWithDitheredTransition(int x, int y, double normalizedLineProgress)
	{
		if (normalizedLineProgress < 0.25)
		{
			if (Prev != null && Prev.Style != Style && Utils.Remap(normalizedLineProgress, 0.0, 0.25, 0.5, 1.0) <= DitherSnakePass._bayerDither[x % 4, y % 4])
			{
				return Prev.Style;
			}
		}
		else if (normalizedLineProgress > 0.75 && Next != null && Next.Style != Style && Utils.Remap(normalizedLineProgress, 0.75, 1.0, 0.0, 0.5) >= DitherSnakePass._bayerDither[x % 4, y % 4])
		{
			return Next.Style;
		}
		return Style;
	}

	public static bool SkipPaintForEdge(int x, int y, DungeonGenerationStyleData style, double normalizedDistanceForPoint)
	{
		if (normalizedDistanceForPoint <= NormalizedDistanceSafeFromDither)
		{
			return false;
		}
		double num = 1.0 - (normalizedDistanceForPoint - NormalizedDistanceSafeFromDither) / (1.0 - NormalizedDistanceSafeFromDither);
		if (!style.EdgeDither)
		{
			return num < 0.25;
		}
		if (!WorldGen.InWorld(x, y, 5))
		{
			return false;
		}
		Tile tile = Main.tile[x, y];
		if (tile != null && !tile.active())
		{
			return true;
		}
		if (num <= DitherSnakePass._bayerDither[x % 4, y % 4])
		{
			return true;
		}
		double num2 = Utils.Lerp(0.0, 0.949999988079071, 1.0 - num);
		if ((double)WorldGen.genRand.NextFloat() <= num2)
		{
			return true;
		}
		if (num <= 3.0 / 32.0 && WorldGen.genRand.Next(3) != 0)
		{
			return true;
		}
		if (num <= 0.125 && WorldGen.genRand.Next(2) == 0)
		{
			return true;
		}
		if (num <= 5.0 / 32.0 && WorldGen.genRand.Next(4) == 0)
		{
			return true;
		}
		return false;
	}

	public void Paint(Rectangle dungeonBounds)
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		CacheNormals();
		double num = Utils.Max<double>(StartRadius, EndRadius);
		Point val = Start.ToPoint();
		Point val2 = End.ToPoint();
		Rectangle val3 = Rectangle.Union(new Rectangle(val.X, val.Y, 1, 1), new Rectangle(val2.X, val2.Y, 1, 1));
		val3.Inflate((int)num, (int)num);
		Rectangle val4 = Rectangle.Intersect(val3, dungeonBounds);
		for (int i = val4.Left; i <= val4.Right; i++)
		{
			for (int j = val4.Top; j <= val4.Bottom; j++)
			{
				Paint(i, j);
			}
		}
	}

	public bool IsSelfIntersecting()
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		CacheNormals();
		double num = Vector2D.Cross(StartNormal, EndNormal);
		Vector2D val = End - Start;
		double value = Vector2D.Cross(val, EndNormal) / num;
		double value2 = Vector2D.Cross(val, StartNormal) / num;
		if (!(Math.Abs(value) < StartRadius))
		{
			return Math.Abs(value2) < EndRadius;
		}
		return true;
	}

	public bool AdjustTangentsToPreventSelfIntersection()
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		if (!IsSelfIntersecting())
		{
			return false;
		}
		Vector2D startTangent = (StartTangent - EndTangent / 2.0).SafeNormalize(default);
		Vector2D endTangent = (EndTangent - StartTangent / 2.0).SafeNormalize(default);
		if (Prev != null)
		{
			StartTangent = startTangent;
			Prev.EndTangent = -StartTangent;
		}
		if (Next != null)
		{
			EndTangent = endTangent;
			Next.StartTangent = -EndTangent;
		}
		return true;
	}

	public bool IsInsideBorder(Point point)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		if (CanPaint(point.X, point.Y, out var distance, out var normalizedLineProgress))
		{
			return distance < Utils.Lerp(StartRadius, EndRadius, normalizedLineProgress) * NormalizedDistanceSafeFromDither - 4.0;
		}
		return false;
	}

	public Vector2D GetPotentialRoomPosition(double normalizedDistanceAlong, double normalizedOffset, int roomRadius)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		Vector2D val = Vector2D.Lerp(StartNormal, EndNormal, normalizedDistanceAlong).SafeNormalize(default);
		double num = Utils.Lerp(StartRadius, EndRadius, normalizedDistanceAlong) * NormalizedDistanceSafeFromDither - 4.0 - (double)roomRadius;
		return Vector2D.Lerp(Start, End, normalizedDistanceAlong) + val * num * normalizedOffset;
	}
}
