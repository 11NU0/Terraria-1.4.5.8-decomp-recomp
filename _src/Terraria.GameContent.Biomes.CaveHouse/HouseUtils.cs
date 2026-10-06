using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.WorldBuilding;

namespace Terraria.GameContent.Biomes.CaveHouse;

public static class HouseUtils
{
	private static readonly bool[] BlacklistedTiles = TileID.Sets.Factory.CreateBoolSet(true, 225, 41, 43, 44, 226, 203, 112, 25, 151, 21, 467);

	private static readonly bool[] BeelistedTiles = TileID.Sets.Factory.CreateBoolSet(true, 41, 43, 44, 226, 203, 112, 25, 151, 21, 467);

	public static HouseBuilder CreateBuilder(Point origin, StructureMap structures)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		List<Rectangle> list = CreateRooms(origin);
		if (list.Count == 0 || !AreRoomLocationsValid(list))
		{
			return HouseBuilder.Invalid;
		}
		HouseType houseType = GetHouseType(list);
		if (!AreRoomsValid(list, structures, houseType))
		{
			return HouseBuilder.Invalid;
		}
		return houseType switch
		{
			HouseType.Wood => (HouseBuilder)new WoodHouseBuilder(list), 
			HouseType.Desert => new DesertHouseBuilder(list), 
			HouseType.Granite => new GraniteHouseBuilder(list), 
			HouseType.Ice => new IceHouseBuilder(list), 
			HouseType.Jungle => new JungleHouseBuilder(list), 
			HouseType.Marble => new MarbleHouseBuilder(list), 
			HouseType.Mushroom => new MushroomHouseBuilder(list), 
			_ => new WoodHouseBuilder(list), 
		};
	}

	public static int GetMaxPossibleRoomsInABigAbandonedHouse()
	{
		if (WorldGen.SecretSeed.errorWorld.Enabled)
		{
			return 30;
		}
		return 7;
	}

	public static int GetRandomizedRoomCountInABigAbandonedHouse()
	{
		int num = 7;
		if (WorldGen.SecretSeed.errorWorld.Enabled)
		{
			num = WorldGen.genRand.Next(7, 31);
		}
		return Math.Max(0, num - WorldGen.genRand.Next(4));
	}

	private static List<Rectangle> CreateRooms_BigAbandonedHouses(Point origin)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		List<Rectangle> list = new List<Rectangle>();
		if (!WorldUtils.Find(origin, Searches.Chain(new Searches.Down(200), new Conditions.IsSolid()), out var result) || result == origin)
		{
			return new List<Rectangle>();
		}
		Rectangle val = FindRoom(result);
		int randomizedRoomCountInABigAbandonedHouse = GetRandomizedRoomCountInABigAbandonedHouse();
		if (randomizedRoomCountInABigAbandonedHouse == 0)
		{
			return list;
		}
		for (int i = 0; i < randomizedRoomCountInABigAbandonedHouse; i++)
		{
			Rectangle val2 = FindRoom_BigAbandonedHouses(new Point(val.Center.X, val.Y + 1), val);
			list.Add(val2);
			val = val2;
		}
		for (int j = 0; j < list.Count; j++)
		{
			Rectangle val3 = list[j];
			val3.Y += 3;
		}
		return list;
	}

	private static List<Rectangle> CreateRooms(Point origin)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_0104: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0124: Unknown result type (might be due to invalid IL or missing references)
		if (WorldGen.SecretSeed.GenerateBiggerAbandonedHouses)
		{
			return CreateRooms_BigAbandonedHouses(origin);
		}
		if (!WorldUtils.Find(origin, Searches.Chain(new Searches.Down(200), new Conditions.IsSolid()), out var result) || result == origin)
		{
			return new List<Rectangle>();
		}
		Rectangle val = FindRoom(result);
		Rectangle val2 = FindRoom(new Point(val.Center.X, val.Y + 1));
		Rectangle val3 = FindRoom(new Point(val.Center.X, val.Y + val.Height + 10));
		val3.Y = val.Y + val.Height - 1;
		double roomSolidPrecentage = GetRoomSolidPrecentage(val2);
		double roomSolidPrecentage2 = GetRoomSolidPrecentage(val3);
		val.Y += 3;
		val2.Y += 3;
		val3.Y += 3;
		List<Rectangle> list = new List<Rectangle>();
		if (WorldGen.genRand.NextDouble() > roomSolidPrecentage + 0.2)
		{
			list.Add(val2);
		}
		list.Add(val);
		if (WorldGen.genRand.NextDouble() > roomSolidPrecentage2 + 0.2)
		{
			list.Add(val3);
		}
		return list;
	}

	private static Rectangle FindRoom(Point origin)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0104: Unknown result type (might be due to invalid IL or missing references)
		//IL_0128: Unknown result type (might be due to invalid IL or missing references)
		//IL_0150: Unknown result type (might be due to invalid IL or missing references)
		//IL_0156: Unknown result type (might be due to invalid IL or missing references)
		//IL_015f: Unknown result type (might be due to invalid IL or missing references)
		//IL_017e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0184: Unknown result type (might be due to invalid IL or missing references)
		//IL_018c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0192: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0168: Unknown result type (might be due to invalid IL or missing references)
		//IL_016e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0177: Unknown result type (might be due to invalid IL or missing references)
		bool flag = WorldUtils.Find(origin, Searches.Chain(new Searches.Left(25), new Conditions.IsSolid()), out var result);
		bool flag2 = WorldUtils.Find(origin, Searches.Chain(new Searches.Right(25), new Conditions.IsSolid()), out var result2);
		if (!flag)
		{
			result = new Point(origin.X - 25, origin.Y);
		}
		if (!flag2)
		{
			result2 = new Point(origin.X + 25, origin.Y);
		}
		Rectangle val = new Rectangle(origin.X, origin.Y, 0, 0);
		if (origin.X - result.X > result2.X - origin.X)
		{
			val.X = result.X;
			val.Width = Utils.Clamp(result2.X - result.X, 15, 30);
		}
		else
		{
			val.Width = Utils.Clamp(result2.X - result.X, 15, 30);
			val.X = result2.X - val.Width;
		}
		bool flag3 = WorldUtils.Find(result, Searches.Chain(new Searches.Up(10), new Conditions.IsSolid()), out var result3);
		bool flag4 = WorldUtils.Find(result2, Searches.Chain(new Searches.Up(10), new Conditions.IsSolid()), out var result4);
		if (!flag3)
		{
			result3 = new Point(origin.X, origin.Y - 10);
		}
		if (!flag4)
		{
			result4 = new Point(origin.X, origin.Y - 10);
		}
		val.Height = Utils.Clamp(Math.Max(origin.Y - result3.Y, origin.Y - result4.Y), 8, 12);
		val.Y -= val.Height;
		return val;
	}

	private static Rectangle FindRoom_BigAbandonedHouses(Point origin, Rectangle lastRoom)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_012d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0162: Unknown result type (might be due to invalid IL or missing references)
		//IL_0169: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_011b: Unknown result type (might be due to invalid IL or missing references)
		int minValue = 15;
		int num = 30;
		int minValue2 = 8;
		int num2 = 12;
		bool flag = WorldUtils.Find(origin, Searches.Chain(new Searches.Left(25), new Conditions.IsSolid()), out var result);
		bool flag2 = WorldUtils.Find(origin, Searches.Chain(new Searches.Right(25), new Conditions.IsSolid()), out var result2);
		if (!flag)
		{
			result = new Point(origin.X - 25, origin.Y);
		}
		if (!flag2)
		{
			result2 = new Point(origin.X + 25, origin.Y);
		}
		Rectangle val = new Rectangle(origin.X, origin.Y, 0, 0);
		if (WorldGen.genRand.Next(2) == 0)
		{
			if (result.X < lastRoom.Left)
			{
				result.X = lastRoom.Left;
			}
			val.X = result.X;
			val.Width = WorldGen.genRand.Next(minValue, num + 1);
			if (val.Left <= 10)
			{
				val.X = 10;
			}
			if (val.Right >= Main.maxTilesX - 10)
			{
				val.X = Main.maxTilesX - 10 - val.Width;
			}
		}
		else
		{
			if (result2.X > lastRoom.Right)
			{
				result2.X = lastRoom.Right;
			}
			val.Width = WorldGen.genRand.Next(minValue, num + 1);
			val.X = result2.X - val.Width;
			if (val.Left <= 10)
			{
				val.X = 10;
			}
			if (val.Right >= Main.maxTilesX - 10)
			{
				val.X = Main.maxTilesX - 10 - val.Width;
			}
		}
		val.Height = WorldGen.genRand.Next(minValue2, num2 + 1);
		val.Y -= val.Height;
		return val;
	}

	private static double GetRoomSolidPrecentage(Rectangle room)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		double num = room.Width * room.Height;
		Ref<int> obj = new Ref<int>(0);
		WorldUtils.Gen(new Point(room.X, room.Y), new Shapes.Rectangle(room.Width, room.Height), Actions.Chain(new Modifiers.IsSolid(), new Actions.Count(obj)));
		return (double)obj.Value / num;
	}

	private static bool AreRoomLocationsValid(IEnumerable<Rectangle> rooms)
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		foreach (Rectangle room in rooms)
		{
			if (!WorldGen.InWorld(room, 10))
			{
				return false;
			}
			if (room.Y + room.Height > Main.maxTilesY - 220)
			{
				return false;
			}
		}
		return true;
	}

	private static HouseType GetHouseType(IEnumerable<Rectangle> rooms)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		Dictionary<ushort, int> dictionary = new Dictionary<ushort, int>();
		foreach (Rectangle room in rooms)
		{
			WorldUtils.Gen(new Point(room.X - 10, room.Y - 10), new Shapes.Rectangle(room.Width + 20, room.Height + 20), new Actions.TileScanner(0, 59, 147, 1, 161, 53, 396, 397, 368, 367, 60, 70).Output(dictionary));
		}
		List<Tuple<HouseType, int>> list = new List<Tuple<HouseType, int>>
		{
			Tuple.Create(HouseType.Wood, dictionary[0] + dictionary[1]),
			Tuple.Create(HouseType.Jungle, dictionary[59] + dictionary[60] * 10),
			Tuple.Create(HouseType.Mushroom, dictionary[59] + dictionary[70] * 10),
			Tuple.Create(HouseType.Ice, dictionary[147] + dictionary[161]),
			Tuple.Create(HouseType.Desert, dictionary[397] + dictionary[396] + dictionary[53]),
			Tuple.Create(HouseType.Granite, dictionary[368]),
			Tuple.Create(HouseType.Marble, dictionary[367])
		};
		Tuple<HouseType, int> tuple = list[0];
		for (int i = 1; i < list.Count; i++)
		{
			if (tuple.Item2 < list[i].Item2)
			{
				tuple = list[i];
			}
		}
		return tuple.Item1;
	}

	private static bool AreRoomsValid(IEnumerable<Rectangle> rooms, StructureMap structures, HouseType style)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		foreach (Rectangle room in rooms)
		{
			if (style != HouseType.Granite && WorldUtils.Find(new Point(room.X - 2, room.Y - 2), Searches.Chain(new Searches.Rectangle(room.Width + 4, room.Height + 4).RequireAll(mode: false), new Conditions.HasLava()), out var _))
			{
				return false;
			}
			if (WorldGen.notTheBees)
			{
				if (!structures.CanPlace(room, BeelistedTiles, 5))
				{
					return false;
				}
			}
			else if (!structures.CanPlace(room, BlacklistedTiles, 5))
			{
				return false;
			}
		}
		return true;
	}
}
