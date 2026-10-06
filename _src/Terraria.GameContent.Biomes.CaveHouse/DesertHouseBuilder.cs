using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria.GameContent.Generation;
using Terraria.WorldBuilding;

namespace Terraria.GameContent.Biomes.CaveHouse;

public class DesertHouseBuilder : HouseBuilder
{
	public DesertHouseBuilder(IEnumerable<Rectangle> rooms)
		: base(HouseType.Desert, rooms)
	{
		TileType = 396;
		WallType = 187;
		BeamType = 577;
		PlatformStyle = 42;
		DoorStyle = 43;
		TableStyle = 7;
		UsesTables2 = true;
		WorkbenchStyle = 39;
		PianoStyle = 38;
		BookcaseStyle = 39;
		ChairStyle = 43;
		ChestStyle = 10;
		UsesContainers2 = true;
		PotentiallyConvertToSeedHouse();
	}

	protected override void AgeRoom(Rectangle room)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_010f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0117: Unknown result type (might be due to invalid IL or missing references)
		//IL_011d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0126: Unknown result type (might be due to invalid IL or missing references)
		//IL_012b: Unknown result type (might be due to invalid IL or missing references)
		//IL_018b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0191: Unknown result type (might be due to invalid IL or missing references)
		//IL_0197: Unknown result type (might be due to invalid IL or missing references)
		//IL_019c: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a2: Unknown result type (might be due to invalid IL or missing references)
		WorldUtils.Gen(new Point(room.X, room.Y), new Shapes.Rectangle(room.Width, room.Height), Actions.Chain(new Modifiers.Dither(0.8), new Modifiers.Blotches(2, 0.2), new Modifiers.OnlyTiles(TileType), new Actions.SetTileKeepWall(396, setSelfFrames: true), new Modifiers.Dither(), new Actions.SetTileKeepWall(397, setSelfFrames: true)));
		WorldUtils.Gen(new Point(room.X + 1, room.Y), new Shapes.Rectangle(room.Width - 2, 1), Actions.Chain(new Modifiers.Dither(), new Modifiers.OnlyTiles(397, 396), new Modifiers.Offset(0, 1), new ActionStalagtite()));
		WorldUtils.Gen(new Point(room.X + 1, room.Y + room.Height - 1), new Shapes.Rectangle(room.Width - 2, 1), Actions.Chain(new Modifiers.Dither(), new Modifiers.OnlyTiles(397, 396), new Modifiers.Offset(0, 1), new ActionStalagtite()));
		WorldUtils.Gen(new Point(room.X, room.Y), new Shapes.Rectangle(room.Width, room.Height), Actions.Chain(new Modifiers.Dither(0.8), new Modifiers.Blotches(), new Modifiers.OnlyWalls(WallType), new Actions.PlaceWall(216)));
	}
}
