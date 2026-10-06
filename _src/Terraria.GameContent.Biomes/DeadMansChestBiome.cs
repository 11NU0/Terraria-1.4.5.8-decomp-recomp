using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using Terraria.ID;
using Terraria.Utilities;
using Terraria.WorldBuilding;

namespace Terraria.GameContent.Biomes;

public class DeadMansChestBiome : MicroBiome
{
	private class DartTrapPlacementAttempt
	{
		public int directionX;

		public int xPush;

		public int x;

		public int y;

		public Point position;

		public Tile t;

		public DartTrapPlacementAttempt(Point position, int directionX, int x, int y, int xPush, Tile t)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			this.position = position;
			this.directionX = directionX;
			this.x = x;
			this.y = y;
			this.xPush = xPush;
			this.t = t;
		}
	}

	private class BoulderPlacementAttempt
	{
		public Point position;

		public int yPush;

		public int requiredHeight;

		public int bestType;

		public BoulderPlacementAttempt(Point position, int yPush, int requiredHeight, int bestType)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			this.position = position;
			this.yPush = yPush;
			this.requiredHeight = requiredHeight;
			this.bestType = bestType;
		}
	}

	private class WirePlacementAttempt
	{
		public Point position;

		public int dirX;

		public int dirY;

		public int steps;

		public WirePlacementAttempt(Point position, int dirX, int dirY, int steps)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			this.position = position;
			this.dirX = dirX;
			this.dirY = dirY;
			this.steps = steps;
		}
	}

	private class ExplosivePlacementAttempt
	{
		public Point position;

		public ExplosivePlacementAttempt(Point position)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			this.position = position;
		}
	}

	private List<DartTrapPlacementAttempt> _dartTrapPlacementSpots = new List<DartTrapPlacementAttempt>();

	private List<WirePlacementAttempt> _wirePlacementSpots = new List<WirePlacementAttempt>();

	private List<BoulderPlacementAttempt> _boulderPlacementSpots = new List<BoulderPlacementAttempt>();

	private List<ExplosivePlacementAttempt> _explosivePlacementAttempt = new List<ExplosivePlacementAttempt>();

	[JsonProperty("NumberOfDartTraps")]
	private IntRange _numberOfDartTraps = new IntRange(3, 6);

	[JsonProperty("NumberOfBoulderTraps")]
	private IntRange _numberOfBoulderTraps = new IntRange(2, 4);

	[JsonProperty("NumberOfStepsBetweenBoulderTraps")]
	private IntRange _numberOfStepsBetweenBoulderTraps = new IntRange(2, 4);

	public override bool Place(Point origin, StructureMap structures, GenerationProgress progress)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		//IL_0111: Unknown result type (might be due to invalid IL or missing references)
		//IL_0164: Unknown result type (might be due to invalid IL or missing references)
		//IL_0188: Unknown result type (might be due to invalid IL or missing references)
		if (!IsAGoodSpot(origin))
		{
			return false;
		}
		ClearCaches();
		Point position = new Point(origin.X, origin.Y + 1);
		FindBoulderTrapSpots(position);
		FindDartTrapSpots(position);
		FindExplosiveTrapSpots(position);
		if (!AreThereEnoughTraps())
		{
			return false;
		}
		TurnGoldChestIntoDeadMansChest(origin);
		foreach (DartTrapPlacementAttempt dartTrapPlacementSpot in _dartTrapPlacementSpots)
		{
			ActuallyPlaceDartTrap(dartTrapPlacementSpot.position, dartTrapPlacementSpot.directionX, dartTrapPlacementSpot.x, dartTrapPlacementSpot.y, dartTrapPlacementSpot.xPush, dartTrapPlacementSpot.t);
		}
		foreach (WirePlacementAttempt wirePlacementSpot in _wirePlacementSpots)
		{
			PlaceWireLine(wirePlacementSpot.position, wirePlacementSpot.dirX, wirePlacementSpot.dirY, wirePlacementSpot.steps);
		}
		foreach (BoulderPlacementAttempt boulderPlacementSpot in _boulderPlacementSpots)
		{
			ActuallyPlaceBoulderTrap(boulderPlacementSpot.position, boulderPlacementSpot.yPush, boulderPlacementSpot.requiredHeight, boulderPlacementSpot.bestType);
		}
		foreach (ExplosivePlacementAttempt item in _explosivePlacementAttempt)
		{
			ActuallyPlaceExplosive(item.position);
		}
		PlaceWiresForExplosives(origin);
		return true;
	}

	private void PlaceWiresForExplosives(Point origin)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		if (_explosivePlacementAttempt.Count <= 0)
		{
			return;
		}
		PlaceWireLine(origin, 0, 1, _explosivePlacementAttempt[0].position.Y - origin.Y);
		int num = _explosivePlacementAttempt[0].position.X;
		int num2 = _explosivePlacementAttempt[0].position.X;
		int y = _explosivePlacementAttempt[0].position.Y;
		for (int i = 1; i < _explosivePlacementAttempt.Count; i++)
		{
			int x = _explosivePlacementAttempt[i].position.X;
			if (num > x)
			{
				num = x;
			}
			if (num2 < x)
			{
				num2 = x;
			}
		}
		PlaceWireLine(new Point(num, y), 1, 0, num2 - num);
	}

	private bool AreThereEnoughTraps()
	{
		if (_boulderPlacementSpots.Count >= 1 || _explosivePlacementAttempt.Count >= 1)
		{
			return _dartTrapPlacementSpots.Count >= 1;
		}
		return false;
	}

	private void ClearCaches()
	{
		_dartTrapPlacementSpots.Clear();
		_wirePlacementSpots.Clear();
		_boulderPlacementSpots.Clear();
		_explosivePlacementAttempt.Clear();
	}

	private void FindBoulderTrapSpots(Point position)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_0112: Unknown result type (might be due to invalid IL or missing references)
		//IL_012e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
		int x = position.X;
		int num = GenBase._random.Next(_numberOfBoulderTraps);
		int num2 = GenBase._random.Next(_numberOfStepsBetweenBoulderTraps);
		x -= num / 2 * num2;
		int num3 = position.Y - 6;
		for (int i = 0; i <= num; i++)
		{
			FindBoulderTrapSpot(new Point(x, num3));
			x += num2;
		}
		if (_boulderPlacementSpots.Count <= 0)
		{
			return;
		}
		int num4 = _boulderPlacementSpots[0].position.X;
		int num5 = _boulderPlacementSpots[0].position.X;
		for (int j = 1; j < _boulderPlacementSpots.Count; j++)
		{
			int x2 = _boulderPlacementSpots[j].position.X;
			if (num4 > x2)
			{
				num4 = x2;
			}
			if (num5 < x2)
			{
				num5 = x2;
			}
		}
		if (num4 > position.X)
		{
			num4 = position.X;
		}
		if (num5 < position.X)
		{
			num5 = position.X;
		}
		_wirePlacementSpots.Add(new WirePlacementAttempt(new Point(num4, num3 - 1), 1, 0, num5 - num4));
		_wirePlacementSpots.Add(new WirePlacementAttempt(position, 0, -1, 7));
	}

	private void FindBoulderTrapSpot(Point position)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		int x = position.X;
		int y = position.Y;
		for (int i = 0; i < 50; i++)
		{
			if (Main.tile[x, y - i].active())
			{
				FindBoulderTrapSpot_CheckSpot(new Point(x, y - i), i);
				break;
			}
		}
	}

	private void FindBoulderTrapSpot_CheckSpot(Point position, int yPush)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_012e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		//IL_013f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0145: Unknown result type (might be due to invalid IL or missing references)
		//IL_0157: Unknown result type (might be due to invalid IL or missing references)
		//IL_015f: Unknown result type (might be due to invalid IL or missing references)
		//IL_011a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0181: Unknown result type (might be due to invalid IL or missing references)
		//IL_0189: Unknown result type (might be due to invalid IL or missing references)
		//IL_0194: Unknown result type (might be due to invalid IL or missing references)
		//IL_019f: Unknown result type (might be due to invalid IL or missing references)
		//IL_024b: Unknown result type (might be due to invalid IL or missing references)
		int[] array = new int[TileID.Count];
		for (int i = position.X; i < position.X + 2; i++)
		{
			for (int j = position.Y - 4; j <= position.Y; j++)
			{
				Tile tile = Main.tile[i, j];
				if (tile.active() && !Main.tileFrameImportant[tile.type] && Main.tileSolid[tile.type])
				{
					array[tile.type]++;
				}
				if ((tile.active() && !WorldGen.CanBeClearedDuringGeneration(tile.type, i, j)) || (tile.active() && TileID.Sets.IsAContainer[tile.type]))
				{
					return;
				}
			}
		}
		for (int k = position.X - 1; k < position.X + 2 + 1; k++)
		{
			for (int l = position.Y - 4 - 1; l <= position.Y - 4 + 2; l++)
			{
				Tile tile2 = Main.tile[k, l];
				if (!tile2.active() || TileID.Sets.IsAContainer[tile2.type])
				{
					return;
				}
			}
		}
		Tile tile3 = Main.tile[position.X, position.Y + 1];
		Tile tile4 = Main.tile[position.X + 1, position.Y + 1];
		if (tile3.active() && tile4.active())
		{
			return;
		}
		int num = 2;
		int num2 = position.X - num;
		int num3 = position.Y - 4 - num;
		int num4 = position.X + num + 1;
		int num5 = position.Y - 4 + num + 1;
		for (int m = num2; m <= num4; m++)
		{
			for (int n = num3; n <= num5; n++)
			{
				Tile tile5 = Main.tile[m, n];
				if (tile5.active() && (TileID.Sets.IsAContainer[tile5.type] || tile5.type == 12 || tile5.type == 665 || tile5.type == 639))
				{
					return;
				}
			}
		}
		int num6 = -1;
		for (int num7 = 0; num7 < array.Length; num7++)
		{
			if (num6 == -1 || array[num6] < array[num7])
			{
				num6 = num7;
			}
		}
		_boulderPlacementSpots.Add(new BoulderPlacementAttempt(position, yPush - 1, 4, num6));
	}

	private void FindDartTrapSpots(Point position)
	{
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		int num = GenBase._random.Next(_numberOfDartTraps);
		int num2 = ((GenBase._random.Next(2) != 0) ? 1 : (-1));
		int steps = -1;
		for (int i = 0; i < num; i++)
		{
			bool flag = FindDartTrapSpotSingle(position, num2);
			num2 *= -1;
			position.Y--;
			if (flag)
			{
				steps = i;
			}
		}
		_wirePlacementSpots.Add(new WirePlacementAttempt(new Point(position.X, position.Y + num), 0, -1, steps));
	}

	private bool FindDartTrapSpotSingle(Point position, int directionX)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		int x = position.X;
		int y = position.Y;
		for (int i = 0; i < 20; i++)
		{
			Tile tile = Main.tile[x + i * directionX, y];
			if ((!tile.active() || tile.type < 0 || tile.type >= TileID.Count || !TileID.Sets.IsAContainer[tile.type]) && tile.active() && Main.tileSolid[tile.type])
			{
				if (i >= 5 && !tile.actuator() && !Main.tileFrameImportant[tile.type] && WorldGen.CanBeClearedDuringGeneration(tile.type, x + i * directionX, y))
				{
					_dartTrapPlacementSpots.Add(new DartTrapPlacementAttempt(position, directionX, x, y, i, tile));
					return true;
				}
				return false;
			}
		}
		return false;
	}

	private void FindExplosiveTrapSpots(Point position)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_012f: Unknown result type (might be due to invalid IL or missing references)
		//IL_014a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0167: Unknown result type (might be due to invalid IL or missing references)
		int x = position.X;
		int num = position.Y + 3;
		List<int> list = new List<int>();
		if (IsGoodSpotsForExplosive(x, num))
		{
			list.Add(x);
		}
		x++;
		if (IsGoodSpotsForExplosive(x, num))
		{
			list.Add(x);
		}
		int num2 = -1;
		if (list.Count > 0)
		{
			num2 = list[GenBase._random.Next(list.Count)];
		}
		list.Clear();
		x += GenBase._random.Next(2, 6);
		int num3 = 4;
		for (int i = x; i < x + num3; i++)
		{
			if (IsGoodSpotsForExplosive(i, num))
			{
				list.Add(i);
			}
		}
		int num4 = -1;
		if (list.Count > 0)
		{
			num4 = list[GenBase._random.Next(list.Count)];
		}
		x = position.X - num3 - GenBase._random.Next(2, 6);
		for (int j = x; j < x + num3; j++)
		{
			if (IsGoodSpotsForExplosive(j, num))
			{
				list.Add(j);
			}
		}
		int num5 = -1;
		if (list.Count > 0)
		{
			num5 = list[GenBase._random.Next(list.Count)];
		}
		if (num5 != -1)
		{
			_explosivePlacementAttempt.Add(new ExplosivePlacementAttempt(new Point(num5, num)));
		}
		if (num2 != -1)
		{
			_explosivePlacementAttempt.Add(new ExplosivePlacementAttempt(new Point(num2, num)));
		}
		if (num4 != -1)
		{
			_explosivePlacementAttempt.Add(new ExplosivePlacementAttempt(new Point(num4, num)));
		}
	}

	private bool IsGoodSpotsForExplosive(int x, int y)
	{
		Tile tile = Main.tile[x, y];
		if (tile.active() && tile.type >= 0 && tile.type < TileID.Count && TileID.Sets.IsAContainer[tile.type])
		{
			return false;
		}
		if (tile.active() && Main.tileSolid[tile.type] && !Main.tileFrameImportant[tile.type] && !Main.tileSolidTop[tile.type])
		{
			return true;
		}
		return false;
	}

	public List<int> GetPossibleChestsToTrapify(StructureMap structures)
	{
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		List<int> list = new List<int>();
		bool[] array = new bool[TileID.Sets.GeneralPlacementTiles.Length];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = TileID.Sets.GeneralPlacementTiles[i];
		}
		array[21] = true;
		array[467] = true;
		array[138] = true;
		array[664] = true;
		array[712] = true;
		array[713] = true;
		array[714] = true;
		array[715] = true;
		for (int j = 0; j < 8000; j++)
		{
			Chest chest = Main.chest[j];
			if (chest == null)
			{
				continue;
			}
			Point val = new Point(chest.x, chest.y);
			if (IsAGoodSpot(val))
			{
				ClearCaches();
				Point position = new Point(val.X, val.Y + 1);
				FindBoulderTrapSpots(position);
				FindDartTrapSpots(position);
				if (AreThereEnoughTraps() && (structures == null || structures.CanPlace(new Rectangle(val.X, val.Y, 1, 1), array, 10)))
				{
					list.Add(j);
				}
			}
		}
		return list;
	}

	private static bool IsAGoodSpot(Point position)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		if (!WorldGen.InWorld(position.X, position.Y, 50))
		{
			return false;
		}
		if (WorldGen.oceanDepths(position.X, position.Y))
		{
			return false;
		}
		Tile tile = Main.tile[position.X, position.Y];
		if (tile.type != 21)
		{
			return false;
		}
		if (tile.frameX / 36 != 1)
		{
			return false;
		}
		tile = Main.tile[position.X, position.Y + 2];
		if (!WorldGen.CanBeClearedDuringGeneration(tile.type, position.X, position.Y + 2))
		{
			return false;
		}
		if (WorldGen.countWires(position.X, position.Y, 20) > 0)
		{
			return false;
		}
		if (WorldGen.countTiles(position.X, position.Y, jungle: false, lavaOk: true) < 40)
		{
			return false;
		}
		return true;
	}

	private void TurnGoldChestIntoDeadMansChest(Point position)
	{
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		for (int i = 0; i < 2; i++)
		{
			for (int j = 0; j < 2; j++)
			{
				int num = position.X + i;
				int num2 = position.Y + j;
				Tile tile = Main.tile[num, num2];
				tile.type = 467;
				tile.frameX = (short)(144 + i * 18);
				tile.frameY = (short)(j * 18);
			}
		}
		if (GenBase._random.Next(3) != 0)
		{
			return;
		}
		int num3 = Chest.FindChest(position.X, position.Y);
		if (num3 <= -1)
		{
			return;
		}
		Item[] item = Main.chest[num3].item;
		for (int num4 = item.Length - 2; num4 > 0; num4--)
		{
			Item item2 = item[num4];
			if (item2.stack != 0)
			{
				item[num4 + 1] = item2.DeepClone();
			}
		}
		item[1] = new Item();
		item[1].SetDefaults(5007);
		Main.chest[num3].item = item;
	}

	private void ActuallyPlaceDartTrap(Point position, int directionX, int x, int y, int xPush, Tile t)
	{
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		t.type = 137;
		t.frameY = 0;
		if (directionX == -1)
		{
			t.frameX = 18;
		}
		else
		{
			t.frameX = 0;
		}
		t.slope(0);
		t.halfBrick(halfBrick: false);
		WorldGen.TileFrame(x, y, resetFrame: true);
		PlaceWireLine(position, directionX, 0, xPush);
	}

	private void PlaceWireLine(Point start, int offsetX, int offsetY, int steps)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		for (int i = 0; i <= steps; i++)
		{
			Main.tile[start.X + offsetX * i, start.Y + offsetY * i].wire(wire: true);
		}
	}

	private void ActuallyPlaceBoulderTrap(Point position, int yPush, int requiredHeight, int bestType)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_018c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		for (int i = position.X; i < position.X + 2; i++)
		{
			for (int j = position.Y - requiredHeight; j <= position.Y + 2; j++)
			{
				Tile tile = Main.tile[i, j];
				if (j < position.Y - requiredHeight + 2)
				{
					tile.ClearTile();
				}
				else if (j <= position.Y)
				{
					bool flag = false;
					do
					{
						if (!tile.active())
						{
							tile.active(active: true);
							tile.type = (ushort)bestType;
						}
						tile.slope(0);
						tile.halfBrick(halfBrick: false);
						WorldGen.TileFrame(i, j, resetFrame: true);
						if (flag)
						{
							break;
						}
						flag = true;
					}
					while (!tile.active());
					tile.wire(wire: true);
					if (Main.tileSolid[tile.type])
					{
						tile.actuator(actuator: true);
					}
				}
				else
				{
					tile.ClearTile();
				}
			}
		}
		int num = position.X + 1;
		int num2 = position.Y - requiredHeight + 1;
		int num3 = 3;
		int num4 = num - num3;
		int num5 = num2 - num3;
		int num6 = num + num3 - 1;
		int num7 = num2 + num3 - 1;
		for (int k = num4; k <= num6; k++)
		{
			for (int l = num5; l <= num7; l++)
			{
				Tile tile2 = Main.tile[k, l];
				if (tile2.type >= 0 && !TileID.Sets.Boulders[tile2.type])
				{
					tile2.type = 1;
					if (tile2.wire())
					{
						tile2.actuator(actuator: true);
					}
				}
			}
		}
		WorldGen.PlaceTile(num, num2, 138);
		PlaceWireLine(position, 0, 1, yPush);
	}

	private void ActuallyPlaceExplosive(Point position)
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		Tile tile = Main.tile[position.X, position.Y];
		tile.type = 141;
		tile.frameX = (tile.frameY = 0);
		tile.slope(0);
		tile.halfBrick(halfBrick: false);
		WorldGen.TileFrame(position.X, position.Y, resetFrame: true);
	}
}
