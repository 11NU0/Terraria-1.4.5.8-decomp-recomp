using Microsoft.Xna.Framework;
using ReLogic.Utilities;
using Terraria.WorldBuilding;

namespace Terraria.GameContent.Biomes.Desert;

public static class LarvaHoleEntrance
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
			int holeRadius = WorldGen.genRand.Next(13, 16);
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
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0135: Unknown result type (might be due to invalid IL or missing references)
		//IL_013c: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0157: Unknown result type (might be due to invalid IL or missing references)
		//IL_0166: Unknown result type (might be due to invalid IL or missing references)
		//IL_01dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_018a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0199: Unknown result type (might be due to invalid IL or missing references)
		ShapeData data = new ShapeData();
		WorldUtils.Gen(position, new Shapes.Rectangle(new Rectangle(-holeRadius, -holeRadius * 2, holeRadius * 2, holeRadius * 2)), new Actions.Clear().Output(data));
		WorldUtils.Gen(position, new Shapes.Tail(holeRadius * 2, new Vector2D(0.0, (double)holeRadius * 1.5)), Actions.Chain(new Actions.Clear().Output(data)));
		WorldUtils.Gen(position, new ModShapes.All(data), Actions.Chain(new Modifiers.Offset(0, 1), new Modifiers.Expand(1), new Modifiers.IsSolid(), new Actions.Smooth(applyToNeighbors: true)));
		GenShapeActionPair pair = new GenShapeActionPair(new Shapes.Rectangle(1, 1), Actions.Chain(new Modifiers.Blotches(), new Modifiers.IsSolid(), new Actions.Clear(), new Actions.PlaceWall(187)));
		GenShapeActionPair pair2 = new GenShapeActionPair(new Shapes.Circle(2, 3), Actions.Chain(new Modifiers.IsSolid(), new Actions.SetTile(397), new Actions.PlaceWall(187)));
		int num = position.X;
		int num2 = position.Y + (int)((double)holeRadius * 1.5);
		while (true)
		{
			int num3 = num2;
			Rectangle val = description.Hive;
			int top = val.Top;
			int y = position.Y;
			val = description.Desert;
			if (num3 >= top + (y - val.Top) * 2 + 12)
			{
				break;
			}
			WorldUtils.Gen(new Point(num, num2), pair);
			WorldUtils.Gen(new Point(num, num2), pair2);
			if (num2 % 3 == 0)
			{
				num += WorldGen.genRand.Next(-1, 2);
				WorldUtils.Gen(new Point(num, num2), pair);
				WorldUtils.Gen(new Point(num, num2), pair2);
			}
			num2++;
		}
		WorldUtils.Gen(new Point(position.X, position.Y + 2), new ModShapes.All(data), new Actions.PlaceWall(0));
	}
}
