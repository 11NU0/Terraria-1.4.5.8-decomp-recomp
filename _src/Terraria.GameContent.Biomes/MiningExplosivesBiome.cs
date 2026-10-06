using Microsoft.Xna.Framework;
using ReLogic.Utilities;
using Terraria.GameContent.Generation;
using Terraria.WorldBuilding;

namespace Terraria.GameContent.Biomes;

public class MiningExplosivesBiome : MicroBiome
{
	public override bool Place(Point origin, StructureMap structures, GenerationProgress progress)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0118: Unknown result type (might be due to invalid IL or missing references)
		//IL_0153: Unknown result type (might be due to invalid IL or missing references)
		//IL_0169: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_020a: Unknown result type (might be due to invalid IL or missing references)
		//IL_021d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0225: Unknown result type (might be due to invalid IL or missing references)
		//IL_025d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0275: Unknown result type (might be due to invalid IL or missing references)
		//IL_027d: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0321: Unknown result type (might be due to invalid IL or missing references)
		//IL_0328: Unknown result type (might be due to invalid IL or missing references)
		//IL_034b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0355: Unknown result type (might be due to invalid IL or missing references)
		//IL_0362: Unknown result type (might be due to invalid IL or missing references)
		//IL_036c: Unknown result type (might be due to invalid IL or missing references)
		//IL_037b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0385: Unknown result type (might be due to invalid IL or missing references)
		//IL_0394: Unknown result type (might be due to invalid IL or missing references)
		//IL_039e: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_03cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_043b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0442: Unknown result type (might be due to invalid IL or missing references)
		//IL_040f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0419: Unknown result type (might be due to invalid IL or missing references)
		//IL_0474: Unknown result type (might be due to invalid IL or missing references)
		//IL_047b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0491: Unknown result type (might be due to invalid IL or missing references)
		//IL_0498: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_045d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0464: Unknown result type (might be due to invalid IL or missing references)
		if (WorldGen.SolidTile(origin.X, origin.Y))
		{
			return false;
		}
		if (Main.tile[origin.X, origin.Y].wall == 216 || Main.tile[origin.X, origin.Y].wall == 187)
		{
			return false;
		}
		ushort type = Utils.SelectRandom<ushort>(GenBase._random, (ushort)((GenVars.goldBar == 19) ? 8 : 169), (ushort)((GenVars.silverBar == 21) ? 9 : 168), (ushort)((GenVars.ironBar == 22) ? 6 : 167), (ushort)((GenVars.copperBar == 20) ? 7 : 166));
		double num = GenBase._random.NextDouble() * 2.0 - 1.0;
		if (!WorldUtils.Find(origin, Searches.Chain((num > 0.0) ? ((GenSearch)new Searches.Right(40)) : ((GenSearch)new Searches.Left(40)), new Conditions.IsSolid()), out origin))
		{
			return false;
		}
		if (!WorldUtils.Find(origin, Searches.Chain(new Searches.Down(80), new Conditions.IsSolid()), out origin))
		{
			return false;
		}
		ShapeData shapeData = new ShapeData();
		Ref<int> obj = new Ref<int>(0);
		Ref<int> obj2 = new Ref<int>(0);
		WorldUtils.Gen(origin, new ShapeRunner(10.0, 20, new Vector2D(num, 1.0)).Output(shapeData), Actions.Chain(new Modifiers.Blotches(), new Actions.Scanner(obj), new Modifiers.IsSolid(), new Actions.Scanner(obj2)));
		if (obj2.Value < obj.Value / 2)
		{
			return false;
		}
		Rectangle area = new Rectangle(origin.X - 15, origin.Y - 10, 30, 20);
		if (!structures.CanPlace(area))
		{
			return false;
		}
		WorldUtils.Gen(origin, new ModShapes.All(shapeData), new Actions.SetTile(type, setSelfFrames: true));
		WorldUtils.Gen(new Point(origin.X - (int)(num * -5.0), origin.Y - 5), new Shapes.Circle(5), Actions.Chain(new Modifiers.Blotches(), new Actions.ClearTile(frameNeighbors: true)));
		int num2 = 1 & (WorldUtils.Find(new Point(origin.X - ((num > 0.0) ? 3 : (-3)), origin.Y - 3), Searches.Chain(new Searches.Down(10), new Conditions.IsSolid()), out var result) ? 1 : 0);
		int num3 = ((GenBase._random.Next(4) == 0) ? 3 : 7);
		if (((uint)num2 & (WorldUtils.Find(new Point(origin.X - ((num > 0.0) ? (-num3) : num3), origin.Y - 3), Searches.Chain(new Searches.Down(10), new Conditions.IsSolid()), out var result2) ? 1u : 0u)) == 0)
		{
			return false;
		}
		result.Y--;
		result2.Y--;
		Tile tile = GenBase._tiles[result.X, result.Y + 1];
		tile.slope(0);
		tile.halfBrick(halfBrick: false);
		for (int i = -1; i <= 1; i++)
		{
			WorldUtils.ClearTile(result2.X + i, result2.Y);
			WorldUtils.ClearTile(result2.X + i, result2.Y - 1);
			WorldUtils.ClearTile(result2.X + i, result2.Y - 2);
			WorldUtils.ClearTile(result2.X + i, result2.Y - 3);
			WorldUtils.ClearTile(result2.X + i, result2.Y - 4);
			Tile tile2 = GenBase._tiles[result2.X + i, result2.Y + 1];
			if (!WorldGen.SolidOrSlopedTile(tile2))
			{
				tile2.ResetToType(1);
				tile2.active(active: true);
			}
			tile2.slope(0);
			tile2.halfBrick(halfBrick: false);
			WorldUtils.TileFrame(result2.X + i, result2.Y + 1, frameNeighbors: true);
		}
		Tile tile3 = Main.tile[result.X, result.Y];
		if (tile3 != null && tile3.active() && !WorldGen.CanKillTile(result.X, result.Y))
		{
			return false;
		}
		WorldGen.PlaceTile(result.X, result.Y, 141, mute: true, forced: true);
		WorldGen.PlaceTile(result2.X, result2.Y, 411, mute: true, forced: true);
		WorldUtils.WireLine(result, result2);
		structures.AddProtectedStructure(area, 5);
		return true;
	}
}
