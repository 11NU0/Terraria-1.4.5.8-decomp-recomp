using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.Xna.Framework;
using Terraria.GameContent.Generation;
using Terraria.Utilities;
using Terraria.WorldBuilding;

namespace Terraria.GameContent.Biomes.CaveHouse;

public class HouseBuilder
{
	private const int VERTICAL_EXIT_WIDTH = 3;

	public static readonly HouseBuilder Invalid = new HouseBuilder();

	public readonly HouseType Type;

	public readonly bool IsValid;

	protected ushort[] SkipTilesDuringWallAging = new ushort[5] { 245, 246, 240, 241, 242 };

	public double ChestChance { get; set; }

	public ushort TileType { get; protected set; }

	public ushort WallType { get; protected set; }

	public ushort BeamType { get; protected set; }

	public byte BeamPaint { get; protected set; }

	public int PlatformStyle { get; protected set; }

	public int DoorStyle { get; protected set; }

	public int TableStyle { get; protected set; }

	public bool UsesTables2 { get; protected set; }

	public int WorkbenchStyle { get; protected set; }

	public int PianoStyle { get; protected set; }

	public int BookcaseStyle { get; protected set; }

	public int ChairStyle { get; protected set; }

	public int ChestStyle { get; protected set; }

	public bool UsesContainers2 { get; protected set; }

	public ReadOnlyCollection<Rectangle> Rooms { get; private set; }

	public Rectangle TopRoom
	{
		get
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			return Rooms.First();
		}
	}

	public Rectangle BottomRoom
	{
		get
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			return Rooms.Last();
		}
	}

	private UnifiedRandom _random => WorldGen.genRand;

	private Tile[,] _tiles => Main.tile;

	private HouseBuilder()
	{
		IsValid = false;
	}

	protected HouseBuilder(HouseType type, IEnumerable<Rectangle> rooms)
	{
		Type = type;
		IsValid = true;
		List<Rectangle> list = rooms.ToList();
		list.Sort((Rectangle lhs, Rectangle rhs) => lhs.Top.CompareTo(rhs.Top));
		Rooms = list.AsReadOnly();
	}

	protected virtual void AgeRoom(Rectangle room)
	{
	}

	public void PotentiallyConvertToRainbowMossBlock()
	{
		if (WorldGen.SecretSeed.rainbowStuff.Enabled && WorldGen.genRand.Next(2) == 0)
		{
			TileType = 692;
			WallType = 346;
			PlatformStyle = 43;
			DoorStyle = 44;
		}
	}

	public void PotentiallyConvertToRainbowBrick()
	{
		if (!Main.tenthAnniversaryWorld)
		{
			return;
		}
		if (Main.getGoodWorld)
		{
			if (WorldGen.genRand.Next(7) == 0)
			{
				TileType = 160;
				WallType = 44;
			}
		}
		else if (WorldGen.genRand.Next(2) == 0)
		{
			TileType = 160;
			WallType = 44;
		}
	}

	public void RainbowifyOnTenthAnniversaryWorlds()
	{
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		if (!Main.tenthAnniversaryWorld || (TileType == 160 && WorldGen.genRand.Next(2) == 0))
		{
			return;
		}
		foreach (Rectangle room in Rooms)
		{
			WorldUtils.Gen(new Point(room.X, room.Y), new Shapes.Rectangle(room.Width, room.Height), new Actions.SetTileAndWallRainbowPaint());
		}
	}

	public void PotentiallyConvertToSeedHouse()
	{
		if (WorldGen.SecretSeed.errorWorld.Enabled)
		{
			PlatformStyle = WorldGen.genRand.Next(49);
			DoorStyle = WorldGen.genRand.Next(49);
			TableStyle = WorldGen.genRand.Next(35);
			WorkbenchStyle = WorldGen.genRand.Next(44);
			PianoStyle = WorldGen.genRand.Next(39);
			BookcaseStyle = WorldGen.genRand.Next(40);
			ChairStyle = WorldGen.genRand.Next(44);
			switch (WorldGen.genRand.Next(20))
			{
			default:
				TileType = 159;
				WallType = 43;
				break;
			case 1:
				TileType = 422;
				WallType = 225;
				break;
			case 2:
				TileType = 194;
				WallType = 75;
				break;
			case 3:
				TileType = 541;
				WallType = 318;
				PlatformStyle = 48;
				break;
			case 4:
				TileType = 137;
				WallType = 147;
				break;
			case 5:
				TileType = 48;
				WallType = 245;
				break;
			case 6:
				TileType = 370;
				WallType = 182;
				break;
			case 7:
				TileType = 140;
				WallType = 33;
				break;
			case 8:
				TileType = 347;
				WallType = 174;
				break;
			case 9:
				TileType = 508;
				WallType = 243;
				break;
			case 10:
				TileType = 507;
				WallType = 242;
				break;
			case 11:
				TileType = 546;
				WallType = 167;
				break;
			case 12:
				TileType = 329;
				WallType = 169;
				break;
			case 13:
				TileType = 326;
				WallType = 136;
				break;
			case 14:
				TileType = 327;
				WallType = 137;
				break;
			case 15:
				TileType = 345;
				WallType = 172;
				break;
			case 16:
				TileType = 708;
				WallType = 347;
				break;
			case 17:
				TileType = 501;
				WallType = 238;
				break;
			case 18:
				TileType = 272;
				WallType = 225;
				break;
			case 19:
				TileType = 421;
				WallType = 225;
				break;
			}
		}
		else
		{
			if (WorldGen.genRand.NextFloat() > 0.4f)
			{
				return;
			}
			bool flag = Type == HouseType.Wood;
			bool flag2 = Type == HouseType.Desert;
			bool flag3 = Type == HouseType.Jungle;
			bool flag4 = Type == HouseType.Ice;
			List<ushort> list = new List<ushort>();
			if (flag3 && Main.notTheBeesWorld && Main.tenthAnniversaryWorld)
			{
				list.Add(562);
				list.Add(563);
				list.Add(229);
			}
			if ((flag | flag4) && Main.drunkWorld && Main.tenthAnniversaryWorld)
			{
				if (flag4)
				{
					list.Add(197);
				}
				else
				{
					list.Add(193);
				}
			}
			if (flag4 && WorldGen.SecretSeed.worldIsFrozen.Enabled && WorldGen.genRand.Next(3) == 0)
			{
				list.Add(145);
				list.Add(146);
			}
			if (flag2 && Main.remixWorld && Main.getGoodWorld)
			{
				list.Add(188);
			}
			if (list.Count > 0)
			{
				ushort num = list[WorldGen.genRand.Next(list.Count)];
				switch (num)
				{
				case 562:
					TileType = num;
					WallType = 312;
					BeamType = 575;
					BeamPaint = 16;
					PlatformStyle = 44;
					DoorStyle = 45;
					TableStyle = 8;
					UsesTables2 = true;
					WorkbenchStyle = 40;
					PianoStyle = 39;
					BookcaseStyle = 40;
					ChairStyle = 44;
					ChestStyle = 11;
					UsesContainers2 = true;
					break;
				case 563:
					TileType = num;
					WallType = 313;
					BeamType = 575;
					BeamPaint = 16;
					PlatformStyle = 44;
					DoorStyle = 45;
					TableStyle = 8;
					UsesTables2 = true;
					WorkbenchStyle = 40;
					PianoStyle = 39;
					BookcaseStyle = 40;
					ChairStyle = 44;
					ChestStyle = 11;
					UsesContainers2 = true;
					break;
				case 229:
					TileType = num;
					WallType = 86;
					BeamType = 575;
					BeamPaint = 15;
					PlatformStyle = 24;
					DoorStyle = 22;
					TableStyle = 19;
					UsesTables2 = false;
					WorkbenchStyle = 19;
					PianoStyle = 9;
					BookcaseStyle = 9;
					ChairStyle = 22;
					ChestStyle = 29;
					UsesContainers2 = false;
					break;
				case 188:
					TileType = num;
					WallType = 72;
					BeamType = 124;
					BeamPaint = 17;
					PlatformStyle = 25;
					DoorStyle = 4;
					TableStyle = 30;
					UsesTables2 = false;
					WorkbenchStyle = 5;
					PianoStyle = 17;
					BookcaseStyle = 6;
					ChairStyle = 6;
					ChestStyle = 42;
					UsesContainers2 = false;
					break;
				case 193:
					TileType = num;
					WallType = 76;
					BeamType = 124;
					BeamPaint = 19;
					PlatformStyle = 20;
					DoorStyle = 31;
					TableStyle = 29;
					UsesTables2 = false;
					WorkbenchStyle = 8;
					PianoStyle = 24;
					BookcaseStyle = 26;
					ChairStyle = 31;
					ChestStyle = 34;
					UsesContainers2 = false;
					break;
				case 197:
					TileType = num;
					WallType = 76;
					BeamType = 574;
					BeamPaint = 26;
					PlatformStyle = 20;
					DoorStyle = 31;
					TableStyle = 29;
					UsesTables2 = false;
					WorkbenchStyle = 8;
					PianoStyle = 24;
					BookcaseStyle = 26;
					ChairStyle = 31;
					ChestStyle = 34;
					UsesContainers2 = false;
					break;
				case 145:
					TileType = num;
					WallType = 29;
					BeamType = 574;
					BeamPaint = 26;
					break;
				case 146:
					TileType = num;
					WallType = 30;
					BeamType = 574;
					BeamPaint = 26;
					break;
				}
			}
		}
	}

	public void PaintSeedHouses()
	{
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		if (TileType != 197 || !Main.drunkWorld || !Main.tenthAnniversaryWorld)
		{
			return;
		}
		foreach (Rectangle room in Rooms)
		{
			WorldUtils.Gen(new Point(room.X, room.Y), new Shapes.Rectangle(room.Width, room.Height), Actions.Chain(new Modifiers.OnlyTiles(19, 10, 11, 14, 18, 87, 101, 15, 21), new Actions.SetTilePaint(7)));
			WorldUtils.Gen(new Point(room.X, room.Y), new Shapes.Rectangle(room.Width, room.Height), Actions.Chain(new Modifiers.OnlyWalls(WallType), new Actions.SetWallPaint(7)));
		}
	}

	public virtual void Place(HouseBuilderContext context, StructureMap structures)
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		PlaceEmptyRooms();
		foreach (Rectangle room in Rooms)
		{
			structures.AddProtectedStructure(room, 8);
		}
		PlaceStairs();
		PlaceDoors();
		PlacePlatforms();
		PlaceSupportBeams();
		PlaceBiomeSpecificPriorityTool(context);
		FillRooms();
		foreach (Rectangle room2 in Rooms)
		{
			AgeRoom(room2);
		}
		PlaceChests();
		PlaceBiomeSpecificTool(context);
		PaintSeedHouses();
	}

	private void PlaceEmptyRooms()
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		foreach (Rectangle room in Rooms)
		{
			WorldUtils.Gen(new Point(room.X, room.Y), new Shapes.Rectangle(room.Width, room.Height), Actions.Chain(new Actions.SetTileKeepWall(TileType), new Actions.SetFrames(frameNeighbors: true)));
			WorldUtils.Gen(new Point(room.X + 1, room.Y + 1), new Shapes.Rectangle(room.Width - 2, room.Height - 2), Actions.Chain(new Actions.ClearTile(frameNeighbors: true), new Actions.PlaceWall(WallType)));
		}
	}

	private void FillRooms()
	{
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_0100: Unknown result type (might be due to invalid IL or missing references)
		//IL_0106: Unknown result type (might be due to invalid IL or missing references)
		//IL_010e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0152: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0204: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fc: Unknown result type (might be due to invalid IL or missing references)
		int num = 14;
		if (UsesTables2)
		{
			num = 469;
		}
		Point[] choices = new Point[7]
		{
			new Point(num, TableStyle),
			new Point(16, 0),
			new Point(18, WorkbenchStyle),
			new Point(86, 0),
			new Point(87, PianoStyle),
			new Point(94, 0),
			new Point(101, BookcaseStyle)
		};
		foreach (Rectangle room in Rooms)
		{
			int num2 = room.Width / 8;
			int num3 = room.Width / (num2 + 1);
			int num4 = _random.Next(2);
			for (int i = 0; i < num2; i++)
			{
				int num5 = (i + 1) * num3 + room.X;
				switch (i + num4 % 2)
				{
				case 0:
				{
					int num6 = room.Y + Math.Min(room.Height / 2, room.Height - 5);
					PaintingEntry paintingEntry = ((Type == HouseType.Desert) ? WorldGen.RandHousePictureDesert() : WorldGen.RandHousePicture());
					WorldGen.PlaceTile(num5, num6, paintingEntry.tileType, mute: true, forced: false, -1, paintingEntry.style);
					break;
				}
				case 1:
				{
					int num6 = room.Y + 1;
					WorldGen.PlaceTile(num5, num6, 34, mute: true, forced: false, -1, _random.Next(6));
					for (int j = -1; j < 2; j++)
					{
						for (int k = 0; k < 3; k++)
						{
							_tiles[j + num5, k + num6].frameX += 54;
						}
					}
					break;
				}
				}
			}
			int num7 = room.Width / 8 + 3;
			WorldGen.SetupStatueList();
			while (num7 > 0)
			{
				int num8 = _random.Next(room.Width - 3) + 1 + room.X;
				int num9 = room.Y + room.Height - 2;
				switch (_random.Next(4))
				{
				case 0:
					WorldGen.PlaceSmallPile(num8, num9, _random.Next(31, 34), 1, 185);
					break;
				case 1:
					WorldGen.PlaceTile(num8, num9, 186, mute: true, forced: false, -1, _random.Next(22, 26));
					break;
				case 2:
				{
					int num10 = _random.Next(2, GenVars.statueList.Length);
					WorldGen.PlaceTile(num8, num9, GenVars.statueList[num10].X, mute: true, forced: false, -1, GenVars.statueList[num10].Y);
					if (GenVars.StatuesWithTraps.Contains(num10))
					{
						WorldGen.PlaceStatueTrap(num8, num9);
					}
					break;
				}
				case 3:
				{
					Point val = Utils.SelectRandom(_random, choices);
					WorldGen.PlaceTile(num8, num9, val.X, mute: true, forced: false, -1, val.Y);
					break;
				}
				}
				num7--;
			}
		}
	}

	private void PlaceStairs()
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		foreach (Tuple<Point, Point> item3 in CreateStairsList())
		{
			Point item = item3.Item1;
			Point item2 = item3.Item2;
			int num = ((item2.X > item.X) ? 1 : (-1));
			ShapeData shapeData = new ShapeData();
			for (int i = 0; i < item2.Y - item.Y; i++)
			{
				shapeData.Add(num * (i + 1), i);
			}
			WorldUtils.Gen(item, new ModShapes.All(shapeData), Actions.Chain(new Actions.PlaceTile(19, PlatformStyle), new Actions.SetSlope((num == 1) ? 1 : 2), new Actions.SetFrames(frameNeighbors: true)));
			WorldUtils.Gen(new Point(item.X + ((num == 1) ? 1 : (-4)), item.Y - 1), new Shapes.Rectangle(4, 1), Actions.Chain(new Actions.Clear(), new Actions.PlaceWall(WallType), new Actions.PlaceTile(19, PlatformStyle), new Actions.SetFrames(frameNeighbors: true)));
		}
	}

	private List<Tuple<Point, Point>> CreateStairsList()
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		List<Tuple<Point, Point>> list = new List<Tuple<Point, Point>>();
		for (int i = 1; i < Rooms.Count; i++)
		{
			Rectangle val = Rooms[i];
			Rectangle val2 = Rooms[i - 1];
			int num = val2.X - val.X;
			int num2 = val.X + val.Width - (val2.X + val2.Width);
			if (num > num2)
			{
				list.Add(new Tuple<Point, Point>(new Point(val.X + val.Width - 1, val.Y + 1), new Point(val.X + val.Width - val.Height + 1, val.Y + val.Height - 1)));
			}
			else
			{
				list.Add(new Tuple<Point, Point>(new Point(val.X, val.Y + 1), new Point(val.X + val.Height - 1, val.Y + val.Height - 1)));
			}
		}
		return list;
	}

	private void PlaceDoors()
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		foreach (Point item in CreateDoorList())
		{
			WorldUtils.Gen(item, new Shapes.Rectangle(1, 3), new Actions.ClearTile(frameNeighbors: true));
			WorldGen.PlaceTile(item.X, item.Y, 10, mute: true, forced: true, -1, DoorStyle);
		}
	}

	private List<Point> CreateDoorList()
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		List<Point> list = new List<Point>();
		foreach (Rectangle room in Rooms)
		{
			if (FindSideExit(new Rectangle(room.X + room.Width, room.Y + 1, 1, room.Height - 2), isLeft: false, out var exitY))
			{
				list.Add(new Point(room.X + room.Width - 1, exitY));
			}
			if (FindSideExit(new Rectangle(room.X, room.Y + 1, 1, room.Height - 2), isLeft: true, out exitY))
			{
				list.Add(new Point(room.X, exitY));
			}
		}
		return list;
	}

	private void PlacePlatforms()
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		foreach (Point item in CreatePlatformsList())
		{
			WorldUtils.Gen(item, new Shapes.Rectangle(3, 1), Actions.Chain(new Actions.ClearMetadata(), new Actions.PlaceTile(19, PlatformStyle), new Actions.SetFrames(frameNeighbors: true)));
		}
	}

	private List<Point> CreatePlatformsList()
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		List<Point> list = new List<Point>();
		Rectangle topRoom = TopRoom;
		Rectangle bottomRoom = BottomRoom;
		if (FindVerticalExit(new Rectangle(topRoom.X + 2, topRoom.Y, topRoom.Width - 4, 1), isUp: true, out var exitX))
		{
			list.Add(new Point(exitX, topRoom.Y));
		}
		if (FindVerticalExit(new Rectangle(bottomRoom.X + 2, bottomRoom.Y + bottomRoom.Height - 1, bottomRoom.Width - 4, 1), isUp: false, out exitX))
		{
			list.Add(new Point(exitX, bottomRoom.Y + bottomRoom.Height - 1));
		}
		return list;
	}

	private void PlaceSupportBeams()
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		foreach (Rectangle item in CreateSupportBeamList())
		{
			if (item.Height > 1 && _tiles[item.X, item.Y - 1].type != 19)
			{
				WorldUtils.Gen(new Point(item.X, item.Y), new Shapes.Rectangle(item.Width, item.Height), Actions.Chain(new Actions.SetTileKeepWall(BeamType), new Actions.SetFrames(frameNeighbors: true), new Actions.SetTilePaint(BeamPaint)));
				Tile tile = _tiles[item.X, item.Y + item.Height];
				tile.slope(0);
				tile.halfBrick(halfBrick: false);
			}
		}
	}

	private List<Rectangle> CreateSupportBeamList()
	{
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		//IL_014c: Unknown result type (might be due to invalid IL or missing references)
		//IL_011a: Unknown result type (might be due to invalid IL or missing references)
		//IL_019c: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_018d: Unknown result type (might be due to invalid IL or missing references)
		List<Rectangle> list = new List<Rectangle>();
		int num = Rooms.Min((Rectangle room) => room.Left);
		int num2 = Rooms.Max((Rectangle room) => room.Right) - 1;
		int num3 = 6;
		while (num3 > 4 && (num2 - num) % num3 != 0)
		{
			num3--;
		}
		for (int num4 = num; num4 <= num2; num4 += num3)
		{
			for (int num5 = 0; num5 < Rooms.Count; num5++)
			{
				Rectangle val = Rooms[num5];
				if (num4 < val.X || num4 >= val.X + val.Width)
				{
					continue;
				}
				int num6 = val.Y + val.Height;
				int num7 = 50;
				for (int num8 = num5 + 1; num8 < Rooms.Count; num8++)
				{
					if (num4 >= Rooms[num8].X && num4 < Rooms[num8].X + Rooms[num8].Width)
					{
						num7 = Math.Min(num7, Rooms[num8].Y - num6);
					}
				}
				if (num7 > 0)
				{
					bool flag = WorldUtils.Find(new Point(num4, num6), Searches.Chain(new Searches.Down(num7), new Conditions.IsSolid()), out var result);
					if (num7 < 50 && !WorldGen.SecretSeed.GenerateBiggerAbandonedHouses)
					{
						flag = true;
						result = new Point(num4, num6 + num7);
					}
					if (flag)
					{
						list.Add(new Rectangle(num4, num6, 1, result.Y - num6));
					}
				}
			}
		}
		return list;
	}

	private static bool FindVerticalExit(Rectangle wall, bool isUp, out int exitX)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		bool result = WorldUtils.Find(new Point(wall.X + wall.Width - 3, wall.Y + (isUp ? (-5) : 0)), Searches.Chain(new Searches.Left(wall.Width - 3), new Conditions.IsSolid().Not().AreaOr(3, 5)), out var result2);
		exitX = result2.X;
		return result;
	}

	private static bool FindSideExit(Rectangle wall, bool isLeft, out int exitY)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		bool result = WorldUtils.Find(new Point(wall.X + (isLeft ? (-4) : 0), wall.Y + wall.Height - 3), Searches.Chain(new Searches.Up(wall.Height - 3), new Conditions.IsSolid().Not().AreaOr(4, 3)), out var result2);
		exitY = result2.Y;
		return result;
	}

	private void PlaceChests()
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_020f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0223: Unknown result type (might be due to invalid IL or missing references)
		//IL_0242: Unknown result type (might be due to invalid IL or missing references)
		//IL_0256: Unknown result type (might be due to invalid IL or missing references)
		//IL_0118: Unknown result type (might be due to invalid IL or missing references)
		//IL_011d: Unknown result type (might be due to invalid IL or missing references)
		//IL_011f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0163: Unknown result type (might be due to invalid IL or missing references)
		//IL_0171: Unknown result type (might be due to invalid IL or missing references)
		//IL_019c: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cc: Unknown result type (might be due to invalid IL or missing references)
		if (_random.NextDouble() > ChestChance)
		{
			return;
		}
		bool flag = false;
		foreach (Rectangle room in Rooms)
		{
			int num = room.Height - 1 + room.Y;
			bool flag2 = num > (int)Main.worldSurface;
			ushort chestTileType = (ushort)((flag2 && UsesContainers2) ? 467 : 21);
			int chestStyle = (flag2 ? ChestStyle : 0);
			for (int i = 0; i < 10; i++)
			{
				if (flag = WorldGen.AddBuriedChest(_random.Next(2, room.Width - 2) + room.X, num, 0, notNearOtherChests: false, chestStyle, trySlope: false, chestTileType))
				{
					break;
				}
			}
			if (flag)
			{
				break;
			}
			for (int j = room.X + 2; j <= room.X + room.Width - 2; j++)
			{
				if (flag = WorldGen.AddBuriedChest(j, num, 0, notNearOtherChests: false, chestStyle, trySlope: false, chestTileType))
				{
					break;
				}
			}
			if (flag)
			{
				break;
			}
		}
		if (!flag)
		{
			foreach (Rectangle room2 in Rooms)
			{
				int num2 = room2.Y - 1;
				bool flag3 = num2 > (int)Main.worldSurface;
				ushort chestTileType2 = (ushort)((flag3 && UsesContainers2) ? 467 : 21);
				int chestStyle2 = (flag3 ? ChestStyle : 0);
				for (int k = 0; k < 10; k++)
				{
					if (flag = WorldGen.AddBuriedChest(_random.Next(2, room2.Width - 2) + room2.X, num2, 0, notNearOtherChests: false, chestStyle2, trySlope: false, chestTileType2))
					{
						break;
					}
				}
				if (flag)
				{
					break;
				}
				for (int l = room2.X + 2; l <= room2.X + room2.Width - 2; l++)
				{
					if (flag = WorldGen.AddBuriedChest(l, num2, 0, notNearOtherChests: false, chestStyle2, trySlope: false, chestTileType2))
					{
						break;
					}
				}
				if (flag)
				{
					break;
				}
			}
		}
		if (flag)
		{
			return;
		}
		for (int m = 0; m < 1000; m++)
		{
			int i2 = _random.Next(Rooms[0].X - 30, Rooms[0].X + 30);
			int num3 = _random.Next(Rooms[0].Y - 30, Rooms[0].Y + 30);
			bool flag4 = num3 > (int)Main.worldSurface;
			ushort chestTileType3 = (ushort)((flag4 && UsesContainers2) ? 467 : 21);
			int chestStyle3 = (flag4 ? ChestStyle : 0);
			if (flag = WorldGen.AddBuriedChest(i2, num3, 0, notNearOtherChests: false, chestStyle3, trySlope: false, chestTileType3))
			{
				break;
			}
		}
	}

	private void PlaceBiomeSpecificPriorityTool(HouseBuilderContext context)
	{
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0104: Unknown result type (might be due to invalid IL or missing references)
		//IL_0143: Unknown result type (might be due to invalid IL or missing references)
		//IL_0148: Unknown result type (might be due to invalid IL or missing references)
		//IL_014a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0161: Unknown result type (might be due to invalid IL or missing references)
		//IL_016f: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0200: Unknown result type (might be due to invalid IL or missing references)
		//IL_0207: Unknown result type (might be due to invalid IL or missing references)
		if (Type != HouseType.Desert || GenVars.extraBastStatueCount >= GenVars.extraBastStatueCountMax)
		{
			return;
		}
		bool flag = false;
		foreach (Rectangle room in Rooms)
		{
			int num = room.Height - 2 + room.Y;
			if (WorldGen.remixWorldGen && (double)num > Main.rockLayer)
			{
				return;
			}
			for (int i = 0; i < 10; i++)
			{
				int num2 = _random.Next(2, room.Width - 2) + room.X;
				WorldGen.PlaceTile(num2, num, 506, mute: true, forced: true);
				if (flag = _tiles[num2, num].active() && _tiles[num2, num].type == 506)
				{
					break;
				}
			}
			if (flag)
			{
				break;
			}
			for (int j = room.X + 2; j <= room.X + room.Width - 2; j++)
			{
				if (flag = WorldGen.PlaceTile(j, num, 506, mute: true, forced: true))
				{
					break;
				}
			}
			if (flag)
			{
				break;
			}
		}
		if (!flag)
		{
			foreach (Rectangle room2 in Rooms)
			{
				int num3 = room2.Y - 1;
				for (int k = 0; k < 10; k++)
				{
					int num4 = _random.Next(2, room2.Width - 2) + room2.X;
					WorldGen.PlaceTile(num4, num3, 506, mute: true, forced: true);
					if (flag = _tiles[num4, num3].active() && _tiles[num4, num3].type == 506)
					{
						break;
					}
				}
				if (flag)
				{
					break;
				}
				for (int l = room2.X + 2; l <= room2.X + room2.Width - 2; l++)
				{
					if (flag = WorldGen.PlaceTile(l, num3, 506, mute: true, forced: true))
					{
						break;
					}
				}
				if (flag)
				{
					break;
				}
			}
		}
		if (flag)
		{
			GenVars.extraBastStatueCount++;
		}
	}

	private void PlaceBiomeSpecificTool(HouseBuilderContext context)
	{
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_0183: Unknown result type (might be due to invalid IL or missing references)
		//IL_0188: Unknown result type (might be due to invalid IL or missing references)
		//IL_018a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0193: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0219: Unknown result type (might be due to invalid IL or missing references)
		//IL_0242: Unknown result type (might be due to invalid IL or missing references)
		//IL_0249: Unknown result type (might be due to invalid IL or missing references)
		if (Type == HouseType.Jungle && context.SharpenerCount < _random.Next(2, 5))
		{
			bool flag = false;
			foreach (Rectangle room in Rooms)
			{
				int num = room.Height - 2 + room.Y;
				for (int i = 0; i < 10; i++)
				{
					int num2 = _random.Next(2, room.Width - 2) + room.X;
					WorldGen.PlaceTile(num2, num, 377, mute: true, forced: true);
					if (flag = _tiles[num2, num].active() && _tiles[num2, num].type == 377)
					{
						break;
					}
				}
				if (flag)
				{
					break;
				}
				for (int j = room.X + 2; j <= room.X + room.Width - 2; j++)
				{
					if (flag = WorldGen.PlaceTile(j, num, 377, mute: true, forced: true))
					{
						break;
					}
				}
				if (flag)
				{
					break;
				}
			}
			if (flag)
			{
				context.SharpenerCount++;
			}
		}
		if (Type != HouseType.Desert || context.ExtractinatorCount >= _random.Next(2, 5))
		{
			return;
		}
		ushort num3 = 219;
		if (WorldGen.SecretSeed.errorWorld.Enabled)
		{
			num3 = 642;
		}
		bool flag2 = false;
		foreach (Rectangle room2 in Rooms)
		{
			int num4 = room2.Height - 2 + room2.Y;
			for (int k = 0; k < 10; k++)
			{
				int num5 = _random.Next(2, room2.Width - 2) + room2.X;
				WorldGen.PlaceTile(num5, num4, num3, mute: true, forced: true);
				if (flag2 = _tiles[num5, num4].active() && _tiles[num5, num4].type == num3)
				{
					break;
				}
			}
			if (flag2)
			{
				break;
			}
			for (int l = room2.X + 2; l <= room2.X + room2.Width - 2; l++)
			{
				if (flag2 = WorldGen.PlaceTile(l, num4, num3, mute: true, forced: true))
				{
					break;
				}
			}
			if (flag2)
			{
				break;
			}
		}
		if (flag2)
		{
			context.ExtractinatorCount++;
		}
	}
}
