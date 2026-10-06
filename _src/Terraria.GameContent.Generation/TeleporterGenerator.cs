using System;
using Terraria.GameContent.Generation.Dungeon;
using Terraria.ID;
using Terraria.Testing;
using Terraria.Utilities;

namespace Terraria.GameContent.Generation;

public class TeleporterGenerator
{
	[Flags]
	private enum PlacementFlags
	{
		None = 0,
		ForceSupports = 1,
		ClearTiles = 2
	}

	private UnifiedRandom _genRand;

	private int _pairsToPlace;

	private int _minDistanceBetweenTeleporters;

	private int _totalAttempts;

	private int _totalPlaced;

	private int _wireColor = 1;

	private PlacementFlags _placementFlags;

	public TeleporterGenerator(UnifiedRandom genRand, int pairsToPlace, int minDistanceBetweenTeleporters, int totalAttempts)
	{
		_genRand = genRand;
		_pairsToPlace = pairsToPlace;
		_minDistanceBetweenTeleporters = minDistanceBetweenTeleporters;
		_totalAttempts = totalAttempts;
	}

	public void PlaceTeleporters()
	{
		int i = 0;
		for (_placementFlags = PlacementFlags.None; (float)i < (float)_totalAttempts * 0.5f; i++)
		{
			if (TryPlaceTeleporterPair() && _totalPlaced >= _pairsToPlace)
			{
				return;
			}
		}
		_placementFlags |= PlacementFlags.ForceSupports;
		int minDistanceBetweenTeleporters = _minDistanceBetweenTeleporters;
		for (_minDistanceBetweenTeleporters = minDistanceBetweenTeleporters / 2; (float)i < (float)_totalAttempts * 0.75f; i++)
		{
			if (TryPlaceTeleporterPair() && _totalPlaced >= _pairsToPlace)
			{
				return;
			}
		}
		_placementFlags |= PlacementFlags.ClearTiles;
		for (_minDistanceBetweenTeleporters = minDistanceBetweenTeleporters / 3; i < _totalAttempts; i++)
		{
			if (TryPlaceTeleporterPair() && _totalPlaced >= _pairsToPlace)
			{
				break;
			}
		}
	}

	private bool TryPlaceTeleporterPair()
	{
		if (!TryFindTeleporterSpot(out var x, out var y))
		{
			return false;
		}
		int num = 500;
		int x2;
		int y2;
		while (!TryFindTeleporterSpot(out x2, out y2))
		{
			num--;
			if (num <= 0)
			{
				break;
			}
		}
		if (num == 0)
		{
			return false;
		}
		if (_placementFlags.HasFlag(PlacementFlags.ForceSupports))
		{
			CleanUpSupport(x, y);
			CleanUpSupport(x2, y2);
		}
		if (_placementFlags.HasFlag(PlacementFlags.ClearTiles))
		{
			ClearAreaAboveTeleporter(x, y);
			ClearAreaAboveTeleporter(x2, y2);
		}
		PlaceTeleporter(x, y);
		PlaceTeleporter(x2, y2);
		WorldGen.AddWireFromPointToPoint(x, y, x2, y2, _wireColor);
		_totalPlaced++;
		_wireColor++;
		if (_wireColor > 3)
		{
			_wireColor = 1;
		}
		return true;
	}

	private bool TryFindTeleporterSpot(out int x, out int y)
	{
		x = _genRand.Next(WorldGen.beachDistance, Main.maxTilesX - WorldGen.beachDistance);
		y = 0;
		if (WorldGen.skyblockWorldGen)
		{
			if (_totalPlaced == 0)
			{
				y = 50;
			}
			else
			{
				y = _genRand.Next(50, Main.UnderworldLayer - 100);
			}
		}
		else
		{
			y = _genRand.Next((int)Main.worldSurface, Main.UnderworldLayer - 100);
		}
		if (Main.tile[x, y].active())
		{
			return false;
		}
		bool flag = false;
		Tile tile = Main.tile[x, y + 1];
		while (!tile.active())
		{
			y++;
			if (y > Main.UnderworldLayer)
			{
				flag = true;
				break;
			}
			tile = Main.tile[x, y + 1];
		}
		if (flag || !Main.tileSolid[tile.type])
		{
			return false;
		}
		return CanPutTeleporterHere(x, y);
	}

	private bool CanPutTeleporterHere(int tileX, int tileY)
	{
		Tile tile = Main.tile[tileX - 1, tileY + 1];
		Tile tile2 = Main.tile[tileX, tileY + 1];
		Tile tile3 = Main.tile[tileX + 1, tileY + 1];
		if (Main.tileFrameImportant[tile.type] || Main.tileFrameImportant[tile2.type] || Main.tileFrameImportant[tile3.type])
		{
			return false;
		}
		if (!_placementFlags.HasFlag(PlacementFlags.ForceSupports) && (!WorldGen.SolidTile(tileX - 1, tileY + 1) || !WorldGen.SolidTile(tileX, tileY + 1) || !WorldGen.SolidTile(tileX + 1, tileY + 1)))
		{
			return false;
		}
		if (Main.wallDungeon[Main.tile[tileX, tileY].wall] || Main.tile[tileX, tileY].wall == 87 || Main.tile[tileX, tileY].wall == 86 || TileID.Sets.Clouds[Main.tile[tileX, tileY + 1].type])
		{
			return false;
		}
		if (WorldGen.SecretSeed.dualDungeons.Enabled && DungeonUtils.InAnyPotentialDungeonBounds(tileX, tileY))
		{
			return false;
		}
		if (Math.Abs(tileX - Main.spawnTileX) + Math.Abs(tileY - Main.spawnTileY) < 20)
		{
			return false;
		}
		if (WorldGen.IsTileNearby(tileX, tileY, 235, _minDistanceBetweenTeleporters))
		{
			return false;
		}
		for (int i = tileX - 1; i <= tileX + 1; i++)
		{
			for (int j = tileY - 3; j <= tileY; j++)
			{
				Tile tile4 = Main.tile[i, j];
				if (tile4.liquid > 0 && tile4.liquidType() > 0)
				{
					return false;
				}
				if (tile4.active() && (!_placementFlags.HasFlag(PlacementFlags.ClearTiles) || !WorldGen.CanKillTile(i, j)))
				{
					return false;
				}
			}
		}
		return true;
	}

	private void ClearAreaAboveTeleporter(int tileX, int tileY)
	{
		for (int i = tileX - 1; i <= tileX + 1; i++)
		{
			for (int j = tileY - 3; j <= tileY; j++)
			{
				ClearTile(i, j, out var _);
			}
		}
	}

	private void CleanUpSupport(int x, int y)
	{
		ClearTile(x - 1, y, out var _);
		ClearTile(x, y, out var _);
		ClearTile(x + 1, y, out var _);
		Tile tile4 = Main.tile[x - 1, y + 1];
		Tile tile5 = Main.tile[x, y + 1];
		Tile tile6 = Main.tile[x + 1, y + 1];
		ushort fallbackType = (ushort)((!tile5.active() || !Main.tileSolid[tile5.type] || Main.tileFrameImportant[tile5.type]) ? 1 : tile5.type);
		PlaceSupport(tile4, fallbackType);
		PlaceSupport(tile5, fallbackType);
		PlaceSupport(tile6, fallbackType);
	}

	private void PlaceSupport(Tile tile, ushort fallbackType)
	{
		bool flag = false;
		if (!tile.active())
		{
			flag = true;
			tile.active(active: true);
		}
		if (flag || !Main.tileSolid[tile.type])
		{
			tile.type = fallbackType;
		}
		tile.ClearSlope();
	}

	private void ClearTile(int x, int y, out Tile tile)
	{
		tile = Main.tile[x, y];
		if (tile.active())
		{
			Invariant.Assert(WorldGen.CanKillTile(x, y), "Trying to clear tile that can't be killed.");
			tile.ClearTile();
			tile.ClearBlockPaintAndCoating();
		}
	}

	private void PlaceTeleporter(int x, int y)
	{
		WorldGen.PlaceTile(x, y, 235);
		Invariant.Assert(Main.tile[x, y].active() && Main.tile[x, y].type == 235, "Failed to place teleporter.");
		WorldGen.PlaceTile(x, y - 1, 135, mute: true, forced: false, -1, 4);
		WorldGen.AddWire(x, y - 1, _wireColor);
		WorldGen.AddWire(x, y, _wireColor);
	}
}
