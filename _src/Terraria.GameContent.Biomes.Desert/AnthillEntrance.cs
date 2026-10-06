using Microsoft.Xna.Framework;
using ReLogic.Utilities;
using Terraria.WorldBuilding;

namespace Terraria.GameContent.Biomes.Desert;

public static class AnthillEntrance
{
	public static void Place(DesertDescription description, GenerationProgress progress, float progressMin, float progressMax)
	{
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		int num = WorldGen.genRand.Next(2, 4);
		for (int i = 0; i < num; i++)
		{
			progress.Set((float)i / (float)num, progressMin, progressMax);
			int holeRadius = WorldGen.genRand.Next(15, 18);
			int num2 = (int)((double)(i + 1) / (double)(num + 1) * (double)description.Surface.Width);
			int num3 = num2;
			Rectangle desert = description.Desert;
			num2 = num3 + desert.Left;
			int num4 = description.Surface[num2];
			PlaceAt(description, new Point(num2, num4), holeRadius);
		}
	}

	private static void PlaceAt(DesertDescription description, Point position, int holeRadius)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0188: Unknown result type (might be due to invalid IL or missing references)
		//IL_0190: Unknown result type (might be due to invalid IL or missing references)
		//IL_0248: Unknown result type (might be due to invalid IL or missing references)
		//IL_024d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0256: Unknown result type (might be due to invalid IL or missing references)
		//IL_025d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0262: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_0277: Unknown result type (might be due to invalid IL or missing references)
		//IL_027d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0293: Unknown result type (might be due to invalid IL or missing references)
		//IL_02de: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0202: Unknown result type (might be due to invalid IL or missing references)
		//IL_0232: Unknown result type (might be due to invalid IL or missing references)
		//IL_0210: Unknown result type (might be due to invalid IL or missing references)
		//IL_0221: Unknown result type (might be due to invalid IL or missing references)
		ShapeData data = new ShapeData();
		Point val = new Point(position.X, position.Y + 6);
		WorldUtils.Gen(val, new Shapes.Tail(holeRadius * 2, new Vector2D(0.0, (double)(-holeRadius) * 1.5)), Actions.Chain(new Actions.SetTile(53).Output(data)));
		GenShapeActionPair genShapeActionPair = new GenShapeActionPair(new Shapes.Rectangle(1, 1), Actions.Chain(new Modifiers.Blotches(), new Modifiers.IsSolid(), new Actions.Clear(), new Actions.PlaceWall(187)));
		GenShapeActionPair genShapeActionPair2 = new GenShapeActionPair(new Shapes.Rectangle(1, 1), Actions.Chain(new Modifiers.IsSolid(), new Actions.Clear(), new Actions.PlaceWall(187)));
		GenShapeActionPair pair = new GenShapeActionPair(new Shapes.Circle(2, 3), Actions.Chain(new Modifiers.IsSolid(), new Actions.SetTile(397), new Actions.PlaceWall(187)));
		GenShapeActionPair pair2 = new GenShapeActionPair(new Shapes.Circle(holeRadius, 3), Actions.Chain(new Modifiers.SkipWalls(187), new Actions.SetTile(53)));
		GenShapeActionPair pair3 = new GenShapeActionPair(new Shapes.Circle(holeRadius - 2, 3), Actions.Chain(new Actions.PlaceWall(187)));
		int num = position.X;
		int num2 = position.Y - holeRadius - 3;
		while (true)
		{
			int num3 = num2;
			Rectangle val2 = description.Hive;
			int top = val2.Top;
			int y = position.Y;
			val2 = description.Desert;
			if (num3 >= top + (y - val2.Top) * 2 + 12)
			{
				break;
			}
			WorldUtils.Gen(new Point(num, num2), (num2 < position.Y) ? genShapeActionPair2 : genShapeActionPair);
			WorldUtils.Gen(new Point(num, num2), pair);
			if (num2 % 3 == 0 && num2 >= position.Y)
			{
				num += WorldGen.genRand.Next(-1, 2);
				WorldUtils.Gen(new Point(num, num2), genShapeActionPair);
				if (num2 >= position.Y + 5)
				{
					WorldUtils.Gen(new Point(num, num2), pair2);
					WorldUtils.Gen(new Point(num, num2), pair3);
				}
				WorldUtils.Gen(new Point(num, num2), pair);
			}
			num2++;
		}
		WorldUtils.Gen(new Point(val.X, val.Y - (int)((double)holeRadius * 1.5) + 3), new Shapes.Circle(holeRadius / 2, holeRadius / 3), Actions.Chain(Actions.Chain(new Actions.ClearTile(), new Modifiers.Expand(1), new Actions.PlaceWall(0))));
		WorldUtils.Gen(val, new ModShapes.All(data), new Actions.Smooth());
	}
}
