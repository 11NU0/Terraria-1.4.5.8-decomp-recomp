using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria.WorldBuilding;

namespace Terraria.GameContent.Biomes.CaveHouse;

public class WoodHouseBuilder : HouseBuilder
{
	public WoodHouseBuilder(IEnumerable<Rectangle> rooms)
		: base(HouseType.Wood, rooms)
	{
		TileType = 30;
		WallType = 27;
		BeamType = 124;
		PlatformStyle = 0;
		DoorStyle = 0;
		TableStyle = 0;
		WorkbenchStyle = 0;
		PianoStyle = 0;
		BookcaseStyle = 0;
		ChairStyle = 0;
		ChestStyle = 1;
		PotentiallyConvertToSeedHouse();
		PotentiallyConvertToRainbowBrick();
		PotentiallyConvertToRainbowMossBlock();
	}

	protected override void AgeRoom(Rectangle room)
	{
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_011f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0148: Unknown result type (might be due to invalid IL or missing references)
		//IL_014e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0154: Unknown result type (might be due to invalid IL or missing references)
		//IL_0159: Unknown result type (might be due to invalid IL or missing references)
		//IL_015f: Unknown result type (might be due to invalid IL or missing references)
		for (int i = 0; i < room.Width * room.Height / 16; i++)
		{
			int num = WorldGen.genRand.Next(1, room.Width - 1) + room.X;
			int num2 = WorldGen.genRand.Next(1, room.Height - 1) + room.Y;
			WorldUtils.Gen(new Point(num, num2), new Shapes.Rectangle(2, 2), Actions.Chain(new Modifiers.Dither(), new Modifiers.Blotches(2, 2), new Modifiers.IsEmpty(), new Actions.SetTile(51, setSelfFrames: true)));
		}
		WorldUtils.Gen(new Point(room.X, room.Y), new Shapes.Rectangle(room.Width, room.Height), Actions.Chain(new Modifiers.Dither(0.85), new Modifiers.Blotches(), new Modifiers.OnlyWalls(WallType), new Modifiers.SkipTiles(SkipTilesDuringWallAging), ((double)room.Y > Main.worldSurface) ? ((GenAction)new Actions.ClearWall(frameNeighbors: true)) : ((GenAction)new Actions.PlaceWall(2))));
		WorldUtils.Gen(new Point(room.X, room.Y), new Shapes.Rectangle(room.Width, room.Height), Actions.Chain(new Modifiers.Dither(0.95), new Modifiers.OnlyTiles(30, 321, 158), new Actions.ClearTile(frameNeighbors: true)));
	}

	public override void Place(HouseBuilderContext context, StructureMap structures)
	{
		base.Place(context, structures);
		RainbowifyOnTenthAnniversaryWorlds();
	}
}
