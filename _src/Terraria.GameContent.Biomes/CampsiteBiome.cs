using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.WorldBuilding;

namespace Terraria.GameContent.Biomes;

public class CampsiteBiome : MicroBiome
{
	public override bool Place(Point origin, StructureMap structures, GenerationProgress progress)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_0123: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0145: Unknown result type (might be due to invalid IL or missing references)
		//IL_0110: Unknown result type (might be due to invalid IL or missing references)
		//IL_0225: Unknown result type (might be due to invalid IL or missing references)
		//IL_0154: Unknown result type (might be due to invalid IL or missing references)
		//IL_0239: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0302: Unknown result type (might be due to invalid IL or missing references)
		//IL_0210: Unknown result type (might be due to invalid IL or missing references)
		//IL_0328: Unknown result type (might be due to invalid IL or missing references)
		//IL_03db: Unknown result type (might be due to invalid IL or missing references)
		//IL_0555: Unknown result type (might be due to invalid IL or missing references)
		//IL_055d: Unknown result type (might be due to invalid IL or missing references)
		//IL_056b: Unknown result type (might be due to invalid IL or missing references)
		//IL_042c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0413: Unknown result type (might be due to invalid IL or missing references)
		//IL_0448: Unknown result type (might be due to invalid IL or missing references)
		//IL_0386: Unknown result type (might be due to invalid IL or missing references)
		//IL_0464: Unknown result type (might be due to invalid IL or missing references)
		//IL_0485: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_050f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0534: Unknown result type (might be due to invalid IL or missing references)
		Ref<int> obj = new Ref<int>(0);
		Ref<int> obj2 = new Ref<int>(0);
		WorldUtils.Gen(origin, new Shapes.Circle(10), Actions.Chain(new Actions.Scanner(obj2), new Modifiers.IsSolid(), new Actions.Scanner(obj)));
		if (obj.Value < obj2.Value - 5)
		{
			return false;
		}
		int num = GenBase._random.Next(6, 10);
		int num2 = GenBase._random.Next(1, 5);
		if (!structures.CanPlace(new Rectangle(origin.X - num, origin.Y - num, num * 2, num * 2)))
		{
			return false;
		}
		int num3 = num + 3;
		for (int i = origin.X - num3; i <= origin.X + num3; i++)
		{
			for (int j = origin.Y - num3; j <= origin.Y + num3; j++)
			{
				Tile tile = Main.tile[i, j];
				if (tile.active() && (Main.tileDungeon[tile.type] || TileID.Sets.IsAContainer[tile.type] || tile.type == 226 || tile.type == 237))
				{
					return false;
				}
			}
		}
		ushort type = (byte)(196 + WorldGen.genRand.Next(4));
		for (int k = origin.X - num; k <= origin.X + num; k++)
		{
			for (int l = origin.Y - num; l <= origin.Y + num; l++)
			{
				if (Main.tile[k, l].active())
				{
					int type2 = Main.tile[k, l].type;
					if (type2 == 53 || type2 == 396 || type2 == 397 || type2 == 404)
					{
						type = 171;
					}
					if (type2 == 161 || type2 == 147)
					{
						type = 40;
					}
					if (type2 == 60)
					{
						type = (byte)(204 + WorldGen.genRand.Next(4));
					}
					if (type2 == 367)
					{
						type = 178;
					}
					if (type2 == 368)
					{
						type = 180;
					}
				}
			}
		}
		ShapeData data = new ShapeData();
		WorldUtils.Gen(origin, new Shapes.Slime(num), Actions.Chain(new Modifiers.Blotches(num2, num2, num2, 1, 1.0).Output(data), new Modifiers.Offset(0, -2), new Modifiers.OnlyTiles(53), new Actions.SetTile(397, setSelfFrames: true), new Modifiers.OnlyWalls(default(ushort)), new Actions.PlaceWall(type)));
		WorldUtils.Gen(origin, new ModShapes.All(data), Actions.Chain(new Actions.ClearTile(), new Actions.SetLiquid(0, 0), new Actions.SetFrames(frameNeighbors: true), new Modifiers.OnlyWalls(default(ushort)), new Actions.PlaceWall(type)));
		if (!WorldUtils.Find(origin, Searches.Chain(new Searches.Down(10), new Conditions.IsSolid()), out var result))
		{
			return false;
		}
		int num4 = result.Y - 1;
		bool flag = GenBase._random.Next() % 2 == 0;
		if (GenBase._random.Next() % 10 != 0)
		{
			int num5 = GenBase._random.Next(1, 4);
			int num6 = (flag ? 4 : (-(num >> 1)));
			for (int m = 0; m < num5; m++)
			{
				int num7 = GenBase._random.Next(1, 3);
				for (int n = 0; n < num7; n++)
				{
					WorldGen.PlaceTile(origin.X + num6 - m, num4 - n, 332, mute: true);
				}
			}
		}
		int num8 = (num - 3) * ((!flag) ? 1 : (-1));
		if (GenBase._random.Next() % 10 != 0)
		{
			WorldGen.PlaceTile(origin.X + num8, num4, 186);
		}
		if (GenBase._random.Next() % 10 != 0)
		{
			if (WorldGen.SecretSeed.rainbowStuff.Enabled)
			{
				WorldGen.PlaceTile(origin.X, num4, 215, mute: true, forced: false, -1, 5);
			}
			else
			{
				WorldGen.PlaceTile(origin.X, num4, 215, mute: true);
			}
			if (GenBase._tiles[origin.X, num4].active() && GenBase._tiles[origin.X, num4].type == 215)
			{
				GenBase._tiles[origin.X, num4].frameY += 36;
				GenBase._tiles[origin.X - 1, num4].frameY += 36;
				GenBase._tiles[origin.X + 1, num4].frameY += 36;
				GenBase._tiles[origin.X, num4 - 1].frameY += 36;
				GenBase._tiles[origin.X - 1, num4 - 1].frameY += 36;
				GenBase._tiles[origin.X + 1, num4 - 1].frameY += 36;
			}
		}
		structures.AddProtectedStructure(new Rectangle(origin.X - num, origin.Y - num, num * 2, num * 2), 4);
		return true;
	}
}
