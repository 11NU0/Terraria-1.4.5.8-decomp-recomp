using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using ReLogic.Utilities;
using Terraria.Utilities;
using Terraria.WorldBuilding;

namespace Terraria.GameContent.Biomes.Desert;

public static class ChambersEntrance
{
	private struct PathConnection(Point position, int direction)
	{
		public readonly Vector2D Position = new Vector2D((double)position.X, (double)position.Y);

		public readonly double Direction = direction;
	}

	public static void Place(DesertDescription description, GenerationProgress progress, float progressMin, float progressMax)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		Rectangle desert = description.Desert;
		int num = desert.Center.X + WorldGen.genRand.Next(-40, 41);
		Point position = new Point(num, (int)description.Surface[num]);
		PlaceAt(description, position, progress, progressMin, progressMax);
	}

	private static void PlaceAt(DesertDescription description, Point position, GenerationProgress progress, float progressMin, float progressMax)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0104: Unknown result type (might be due to invalid IL or missing references)
		//IL_0167: Unknown result type (might be due to invalid IL or missing references)
		//IL_0170: Unknown result type (might be due to invalid IL or missing references)
		//IL_0177: Unknown result type (might be due to invalid IL or missing references)
		//IL_019e: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02db: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0308: Unknown result type (might be due to invalid IL or missing references)
		//IL_032c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0336: Unknown result type (might be due to invalid IL or missing references)
		//IL_033d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0344: Unknown result type (might be due to invalid IL or missing references)
		//IL_034b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0350: Unknown result type (might be due to invalid IL or missing references)
		//IL_0352: Unknown result type (might be due to invalid IL or missing references)
		//IL_0356: Unknown result type (might be due to invalid IL or missing references)
		//IL_035b: Unknown result type (might be due to invalid IL or missing references)
		//IL_035d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0361: Unknown result type (might be due to invalid IL or missing references)
		//IL_0366: Unknown result type (might be due to invalid IL or missing references)
		//IL_0368: Unknown result type (might be due to invalid IL or missing references)
		//IL_036c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0371: Unknown result type (might be due to invalid IL or missing references)
		ShapeData shapeData = new ShapeData();
		Point origin = new Point(position.X, position.Y + 2);
		WorldUtils.Gen(origin, new Shapes.Circle(24, 12), Actions.Chain(new Modifiers.Blotches(), new Actions.SetTile(53).Output(shapeData)));
		UnifiedRandom genRand = WorldGen.genRand;
		ShapeData data = new ShapeData();
		Rectangle hive = description.Hive;
		int num = hive.Top - position.Y;
		int num2 = ((genRand.Next(2) != 0) ? 1 : (-1));
		List<PathConnection> list = new List<PathConnection>
		{
			new PathConnection(new Point(position.X + -num2 * 26, position.Y - 8), num2)
		};
		int num3 = genRand.Next(2, 4);
		for (int i = 0; i < num3; i++)
		{
			int num4 = (int)((double)(i + 1) / (double)num3 * (double)num) + genRand.Next(-8, 9);
			int num5 = num2 * genRand.Next(20, 41);
			int num6 = genRand.Next(18, 29);
			WorldUtils.Gen(position, new Shapes.Circle(num6 / 2, 3), Actions.Chain(new Modifiers.Offset(num5, num4), new Modifiers.Blotches(), new Actions.Clear().Output(data), new Actions.PlaceWall(187)));
			list.Add(new PathConnection(new Point(num5 + num6 / 2 * -num2 + position.X, num4 + position.Y), -num2));
			num2 *= -1;
		}
		WorldUtils.Gen(position, new ModShapes.OuterOutline(data), Actions.Chain(new Modifiers.Expand(1), new Modifiers.OnlyTiles(53), new Actions.SetTile(397), new Actions.PlaceWall(187)));
		GenShapeActionPair pair = new GenShapeActionPair(new Shapes.Rectangle(2, 4), Actions.Chain(new Modifiers.IsSolid(), new Modifiers.Blotches(), new Actions.Clear(), new Modifiers.Expand(1), new Actions.PlaceWall(187), new Modifiers.OnlyTiles(53), new Actions.SetTile(397)));
		for (int j = 1; j < list.Count; j++)
		{
			progress.Set((float)j / (float)list.Count, progressMin, progressMax);
			PathConnection pathConnection = list[j - 1];
			PathConnection pathConnection2 = list[j];
			double num7 = Math.Abs(pathConnection2.Position.X - pathConnection.Position.X) * 1.5;
			for (double num8 = 0.0; num8 <= 1.0; num8 += 0.02)
			{
				Vector2D val = new Vector2D(pathConnection.Position.X + pathConnection.Direction * num7 * num8, pathConnection.Position.Y);
				Vector2D val2 = new Vector2D(pathConnection2.Position.X + pathConnection2.Direction * num7 * (1.0 - num8), pathConnection2.Position.Y);
				Vector2D val3 = Vector2D.Lerp(pathConnection.Position, pathConnection2.Position, num8);
				Vector2D val4 = Vector2D.Lerp(val, val3, num8);
				Vector2D val5 = Vector2D.Lerp(val3, val2, num8);
				WorldUtils.Gen(Vector2D.Lerp(val4, val5, num8).ToPoint(), pair);
			}
		}
		WorldUtils.Gen(origin, new Shapes.Rectangle(new Rectangle(-29, -12, 58, 12)), Actions.Chain(new Modifiers.NotInShape(shapeData), new Modifiers.Expand(1), new Actions.PlaceWall(0)));
	}
}
