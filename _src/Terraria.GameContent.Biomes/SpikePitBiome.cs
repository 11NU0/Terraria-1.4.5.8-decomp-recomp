using Microsoft.Xna.Framework;
using Terraria.WorldBuilding;

namespace Terraria.GameContent.Biomes;

public class SpikePitBiome : MicroBiome
{
	public override bool Place(Point origin, StructureMap structures, GenerationProgress progress)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0127: Unknown result type (might be due to invalid IL or missing references)
		//IL_0185: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0200: Unknown result type (might be due to invalid IL or missing references)
		//IL_0201: Unknown result type (might be due to invalid IL or missing references)
		//IL_0207: Unknown result type (might be due to invalid IL or missing references)
		//IL_0217: Unknown result type (might be due to invalid IL or missing references)
		//IL_021c: Unknown result type (might be due to invalid IL or missing references)
		//IL_021f: Unknown result type (might be due to invalid IL or missing references)
		//IL_022b: Unknown result type (might be due to invalid IL or missing references)
		//IL_024f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0263: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f2: Unknown result type (might be due to invalid IL or missing references)
		if (WorldGen.SolidTile(origin.X, origin.Y) && GenBase._tiles[origin.X, origin.Y].wall == 3)
		{
			return false;
		}
		if (!WorldUtils.Find(origin, Searches.Chain(new Searches.Down(100), new Conditions.IsSolid()), out origin))
		{
			return false;
		}
		ushort num = 1;
		if (WorldGen.notTheBees)
		{
			num = 225;
		}
		if (!WorldUtils.Find(new Point(origin.X - 4, origin.Y), Searches.Chain(new Searches.Down(5), new Conditions.IsTile(num).AreaAnd(8, 1)), out var _))
		{
			return false;
		}
		ShapeData shapeData = new ShapeData();
		ShapeData shapeData2 = new ShapeData();
		ShapeData shapeData3 = new ShapeData();
		for (int i = 0; i < 4; i++)
		{
			WorldUtils.Gen(origin, new Shapes.Circle(GenBase._random.Next(8, 10) + i), Actions.Chain(new Modifiers.Offset(0, 5 * i + 5), new Modifiers.Blotches(3).Output(shapeData)));
		}
		for (int j = 0; j < 4; j++)
		{
			WorldUtils.Gen(origin, new Shapes.Circle(GenBase._random.Next(6, 7) + j), Actions.Chain(new Modifiers.Offset(0, 2 * j + 12), new Modifiers.Blotches(3).Output(shapeData2)));
		}
		for (int k = 0; k < 4; k++)
		{
			WorldUtils.Gen(origin, new Shapes.Circle(GenBase._random.Next(4, 5) + k / 2), Actions.Chain(new Modifiers.Offset(0, (int)(7.5 * (double)k) - 10), new Modifiers.Blotches(3).Output(shapeData3)));
		}
		ShapeData shapeData4 = new ShapeData(shapeData2);
		shapeData2.Subtract(shapeData3, origin, origin);
		shapeData4.Subtract(shapeData2, origin, origin);
		Rectangle bounds = ShapeData.GetBounds(origin, shapeData, shapeData3);
		if (!structures.CanPlace(bounds, 2))
		{
			return false;
		}
		WorldUtils.Gen(origin, new ModShapes.All(shapeData), Actions.Chain(new Actions.SetTile(num, setSelfFrames: true)));
		WorldUtils.Gen(origin, new ModShapes.All(shapeData3), new Actions.ClearTile(frameNeighbors: true));
		WorldUtils.Gen(origin, new ModShapes.All(shapeData4), Actions.Chain(new Modifiers.IsTouchingAir(useDiagonals: true), new Modifiers.IsTouching(true, num), new Actions.SetTile(48, setSelfFrames: true)));
		WorldUtils.Gen(origin, new ModShapes.All(shapeData4), Actions.Chain(new Modifiers.Checkerboard(2), new Modifiers.IsTouchingAir(useDiagonals: true), new Modifiers.IsTouching(false, 48), new Actions.SetTile(48, setSelfFrames: true)));
		structures.AddProtectedStructure(bounds, 2);
		return true;
	}
}
