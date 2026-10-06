using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria.GameContent.Generation;
using Terraria.WorldBuilding;

namespace Terraria.GameContent.Biomes;

public class MahoganyTreeBiome : MicroBiome
{
	public override bool Place(Point origin, StructureMap structures, GenerationProgress progress)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01da: Unknown result type (might be due to invalid IL or missing references)
		//IL_026b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0277: Unknown result type (might be due to invalid IL or missing references)
		//IL_0284: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0307: Unknown result type (might be due to invalid IL or missing references)
		//IL_0314: Unknown result type (might be due to invalid IL or missing references)
		//IL_037c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0387: Unknown result type (might be due to invalid IL or missing references)
		//IL_0392: Unknown result type (might be due to invalid IL or missing references)
		//IL_056a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0575: Unknown result type (might be due to invalid IL or missing references)
		//IL_057d: Unknown result type (might be due to invalid IL or missing references)
		//IL_05fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0605: Unknown result type (might be due to invalid IL or missing references)
		//IL_060d: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_0699: Unknown result type (might be due to invalid IL or missing references)
		//IL_0759: Unknown result type (might be due to invalid IL or missing references)
		//IL_07dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_07e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_07fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0807: Unknown result type (might be due to invalid IL or missing references)
		//IL_0814: Unknown result type (might be due to invalid IL or missing references)
		if (!WorldUtils.Find(new Point(origin.X - 3, origin.Y), Searches.Chain(new Searches.Down(200), new Conditions.IsSolid().AreaAnd(6, 1)), out var result))
		{
			return false;
		}
		if (!WorldUtils.Find(new Point(result.X, result.Y - 5), Searches.Chain(new Searches.Up(120), new Conditions.IsSolid().AreaOr(6, 1)), out var result2) || result.Y - 5 - result2.Y > 60)
		{
			return false;
		}
		if (result.Y - result2.Y < 30)
		{
			return false;
		}
		if (!structures.CanPlace(new Rectangle(result.X - 30, result.Y - 60, 60, 90)))
		{
			return false;
		}
		if (!WorldGen.drunkWorldGen || WorldGen.genRand.Next(50) > 0)
		{
			Dictionary<ushort, int> dictionary = new Dictionary<ushort, int>();
			WorldUtils.Gen(new Point(result.X - 25, result.Y - 25), new Shapes.Rectangle(50, 50), new Actions.TileScanner(0, 59, 60, 147, 161, 163, 200, 164, 1, 25, 203, 117).Output(dictionary));
			int num = dictionary[1] + dictionary[25] + dictionary[203] + dictionary[117];
			int num2 = dictionary[0] + num;
			int num3 = dictionary[59] + dictionary[60];
			int num4 = dictionary[161] + dictionary[163] + dictionary[200] + dictionary[164];
			if (dictionary[147] + num4 > num3 || num2 > num3 || num3 < 50)
			{
				return false;
			}
		}
		int num5 = (result.Y - result2.Y - 9) / 5;
		int num6 = num5 * 5;
		int num7 = 0;
		double num8 = GenBase._random.NextDouble() + 1.0;
		double num9 = GenBase._random.NextDouble() + 2.0;
		if (GenBase._random.Next(2) == 0)
		{
			num9 = 0.0 - num9;
		}
		for (int i = 0; i < num5; i++)
		{
			int num10 = (int)(Math.Sin((double)(i + 1) / 12.0 * num8 * 3.1415927410125732) * num9);
			int num11 = ((num10 < num7) ? (num10 - num7) : 0);
			WorldUtils.Gen(new Point(result.X + num7 + num11, result.Y - (i + 1) * 5), new Shapes.Rectangle(6 + Math.Abs(num10 - num7), 7), Actions.Chain(new Modifiers.SkipTiles(21, 467, 226, 237), new Modifiers.SkipWalls(87), new Actions.RemoveWall(), new Actions.SetTile(383), new Actions.SetFrames()));
			WorldUtils.Gen(new Point(result.X + num7 + num11 + 2, result.Y - (i + 1) * 5), new Shapes.Rectangle(2 + Math.Abs(num10 - num7), 5), Actions.Chain(new Modifiers.SkipTiles(21, 467, 226, 237), new Modifiers.SkipWalls(87), new Actions.ClearTile(frameNeighbors: true), new Actions.PlaceWall(78)));
			WorldUtils.Gen(new Point(result.X + num7 + 2, result.Y - i * 5), new Shapes.Rectangle(2, 2), Actions.Chain(new Modifiers.SkipTiles(21, 467, 226, 237), new Modifiers.SkipWalls(87), new Actions.ClearTile(frameNeighbors: true), new Actions.PlaceWall(78)));
			num7 = num10;
		}
		int num12 = 6;
		if (num9 < 0.0)
		{
			num12 = 0;
		}
		List<Point> list = new List<Point>();
		for (int j = 0; j < 2; j++)
		{
			double num13 = ((double)j + 1.0) / 3.0;
			int num14 = num12 + (int)(Math.Sin((double)num5 * num13 / 12.0 * num8 * 3.1415927410125732) * num9);
			double num15 = GenBase._random.NextDouble() * 0.7853981852531433 - 0.7853981852531433 - 0.2;
			if (num12 == 0)
			{
				num15 -= 1.5707963705062866;
			}
			WorldUtils.Gen(new Point(result.X + num14, result.Y - (int)((double)(num5 * 5) * num13)), new ShapeBranch(num15, GenBase._random.Next(12, 16)).OutputEndpoints(list), Actions.Chain(new Modifiers.SkipTiles(21, 467, 226, 237), new Modifiers.SkipWalls(87), new Actions.SetTile(383), new Actions.SetFrames(frameNeighbors: true)));
			num12 = 6 - num12;
		}
		int num16 = (int)(Math.Sin((double)num5 / 12.0 * num8 * 3.1415927410125732) * num9);
		WorldUtils.Gen(new Point(result.X + 6 + num16, result.Y - num6), new ShapeBranch(-0.6853981852531433, GenBase._random.Next(16, 22)).OutputEndpoints(list), Actions.Chain(new Modifiers.SkipTiles(21, 467, 226, 237), new Modifiers.SkipWalls(87), new Actions.SetTile(383), new Actions.SetFrames(frameNeighbors: true)));
		WorldUtils.Gen(new Point(result.X + num16, result.Y - num6), new ShapeBranch(-2.45619455575943, GenBase._random.Next(16, 22)).OutputEndpoints(list), Actions.Chain(new Modifiers.SkipTiles(21, 467, 226, 237), new Modifiers.SkipWalls(87), new Actions.SetTile(383), new Actions.SetFrames(frameNeighbors: true)));
		foreach (Point item in list)
		{
			WorldUtils.Gen(item, new Shapes.Circle(4), Actions.Chain(new Modifiers.Blotches(4, 2), new Modifiers.SkipTiles(383, 21, 467, 226, 237), new Modifiers.SkipWalls(78, 87), new Actions.SetTile(384), new Actions.SetFrames(frameNeighbors: true)));
		}
		for (int k = 0; k < 4; k++)
		{
			double angle = (double)k / 3.0 * 2.0 + 0.57075;
			WorldUtils.Gen(result, new ShapeRoot(angle, GenBase._random.Next(40, 60)), Actions.Chain(new Modifiers.SkipTiles(21, 467, 226, 237), new Modifiers.SkipWalls(87), new Actions.SetTile(383, setSelfFrames: true)));
		}
		WorldGen.AddBuriedChest(result.X + 3, result.Y - 1, WorldGen.GetNextJungleChestItem(), notNearOtherChests: false, 10, trySlope: false, 0);
		structures.AddProtectedStructure(new Rectangle(result.X - 30, result.Y - 30, 60, 60));
		return true;
	}
}
