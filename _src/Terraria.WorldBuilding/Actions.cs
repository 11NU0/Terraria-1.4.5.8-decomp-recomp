using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Generation.Dungeon;

namespace Terraria.WorldBuilding;

public static class Actions
{
	public class ContinueWrapper : GenAction
	{
		private GenAction _action;

		public ContinueWrapper(GenAction action)
		{
			_action = action;
		}

		public override bool Apply(Point origin, int x, int y, params object[] args)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			_action.Apply(origin, x, y, args);
			return UnitApply(origin, x, y, args);
		}
	}

	public class Count : GenAction
	{
		private Ref<int> _count;

		public Count(Ref<int> count)
		{
			_count = count;
		}

		public override bool Apply(Point origin, int x, int y, params object[] args)
		{
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			_count.Value++;
			return UnitApply(origin, x, y, args);
		}
	}

	public class Scanner : GenAction
	{
		private Ref<int> _count;

		public Scanner(Ref<int> count)
		{
			_count = count;
		}

		public override bool Apply(Point origin, int x, int y, params object[] args)
		{
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			_count.Value++;
			return UnitApply(origin, x, y, args);
		}
	}

	public class TileScanner : GenAction
	{
		private ushort[] _tileIds;

		private Dictionary<ushort, int> _tileCounts;

		public TileScanner(params ushort[] tiles)
		{
			_tileIds = tiles;
			_tileCounts = new Dictionary<ushort, int>();
			for (int i = 0; i < tiles.Length; i++)
			{
				_tileCounts[_tileIds[i]] = 0;
			}
		}

		public override bool Apply(Point origin, int x, int y, params object[] args)
		{
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			Tile tile = GenBase._tiles[x, y];
			if (tile.active() && _tileCounts.ContainsKey(tile.type))
			{
				_tileCounts[tile.type]++;
			}
			return UnitApply(origin, x, y, args);
		}

		public TileScanner Output(Dictionary<ushort, int> resultsOutput)
		{
			_tileCounts = resultsOutput;
			for (int i = 0; i < _tileIds.Length; i++)
			{
				if (!_tileCounts.ContainsKey(_tileIds[i]))
				{
					_tileCounts[_tileIds[i]] = 0;
				}
			}
			return this;
		}

		public Dictionary<ushort, int> GetResults()
		{
			return _tileCounts;
		}

		public int GetCount(ushort tileId)
		{
			if (!_tileCounts.ContainsKey(tileId))
			{
				return -1;
			}
			return _tileCounts[tileId];
		}
	}

	public class Blank : GenAction
	{
		public override bool Apply(Point origin, int x, int y, params object[] args)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return UnitApply(origin, x, y, args);
		}
	}

	public class Custom : GenAction
	{
		private CustomPerUnitAction _perUnit;

		public Custom(CustomPerUnitAction perUnit)
		{
			_perUnit = perUnit;
		}

		public override bool Apply(Point origin, int x, int y, params object[] args)
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			return _perUnit(x, y, args) | UnitApply(origin, x, y, args);
		}
	}

	public class ClearMetadata : GenAction
	{
		public override bool Apply(Point origin, int x, int y, params object[] args)
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			GenBase._tiles[x, y].ClearMetadata();
			return UnitApply(origin, x, y, args);
		}
	}

	public class Clear : GenAction
	{
		public override bool Apply(Point origin, int x, int y, params object[] args)
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			GenBase._tiles[x, y].ClearEverything();
			return UnitApply(origin, x, y, args);
		}
	}

	public class ClearTile : GenAction
	{
		private bool _frameNeighbors;

		public ClearTile(bool frameNeighbors = false)
		{
			_frameNeighbors = frameNeighbors;
		}

		public override bool Apply(Point origin, int x, int y, params object[] args)
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			WorldUtils.ClearTile(x, y, _frameNeighbors);
			return UnitApply(origin, x, y, args);
		}
	}

	public class ClearWall : GenAction
	{
		private bool _frameNeighbors;

		public ClearWall(bool frameNeighbors = false)
		{
			_frameNeighbors = frameNeighbors;
		}

		public override bool Apply(Point origin, int x, int y, params object[] args)
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			WorldUtils.ClearWall(x, y, _frameNeighbors);
			return UnitApply(origin, x, y, args);
		}
	}

	public class HalfBlock : GenAction
	{
		private bool _value;

		public HalfBlock(bool value = true)
		{
			_value = value;
		}

		public override bool Apply(Point origin, int x, int y, params object[] args)
		{
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			GenBase._tiles[x, y].halfBrick(_value);
			return UnitApply(origin, x, y, args);
		}
	}

	public class SetTile : GenAction
	{
		private ushort _type;

		private bool _doFraming;

		private bool _doNeighborFraming;

		private bool _clearTile;

		public SetTile(ushort type, bool setSelfFrames = false, bool setNeighborFrames = true, bool clearTile = true)
		{
			_type = type;
			_doFraming = setSelfFrames;
			_doNeighborFraming = setNeighborFrames;
			_clearTile = clearTile;
		}

		public override bool Apply(Point origin, int x, int y, params object[] args)
		{
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			Tile tile = GenBase._tiles[x, y];
			if (_clearTile)
			{
				tile.Clear(~(TileDataType.Wiring | TileDataType.Actuator));
			}
			tile.type = _type;
			tile.active(active: true);
			if (_doFraming)
			{
				WorldUtils.TileFrame(x, y, _doNeighborFraming);
			}
			return UnitApply(origin, x, y, args);
		}
	}

	public class SetWall : GenAction
	{
		private ushort _type;

		private bool _doFraming;

		private bool _doNeighborFraming;

		private bool _clearTile;

		public SetWall(ushort type, bool setSelfFrames = false, bool setNeighborFrames = true, bool clearTile = true)
		{
			_type = type;
			_doFraming = setSelfFrames;
			_doNeighborFraming = setNeighborFrames;
			_clearTile = clearTile;
		}

		public override bool Apply(Point origin, int x, int y, params object[] args)
		{
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			Tile tile = GenBase._tiles[x, y];
			if (_clearTile)
			{
				tile.Clear(~(TileDataType.Wiring | TileDataType.Actuator));
			}
			tile.wall = _type;
			if (_doFraming)
			{
				WorldUtils.WallFrame(x, y, _doNeighborFraming);
			}
			return UnitApply(origin, x, y, args);
		}
	}

	public class SetTileKeepWall : GenAction
	{
		private ushort _type;

		private bool _doFraming;

		private bool _doNeighborFraming;

		public SetTileKeepWall(ushort type, bool setSelfFrames = false, bool setNeighborFrames = true)
		{
			_type = type;
			_doFraming = setSelfFrames;
			_doNeighborFraming = setNeighborFrames;
		}

		public override bool Apply(Point origin, int x, int y, params object[] args)
		{
			//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
			ushort wall = GenBase._tiles[x, y].wall;
			int wallFrameX = GenBase._tiles[x, y].wallFrameX();
			int wallFrameY = GenBase._tiles[x, y].wallFrameY();
			GenBase._tiles[x, y].Clear(~(TileDataType.Wiring | TileDataType.Actuator));
			GenBase._tiles[x, y].type = _type;
			GenBase._tiles[x, y].active(active: true);
			if (wall > 0)
			{
				GenBase._tiles[x, y].wall = wall;
				GenBase._tiles[x, y].wallFrameX(wallFrameX);
				GenBase._tiles[x, y].wallFrameY(wallFrameY);
			}
			if (_doFraming)
			{
				WorldUtils.TileFrame(x, y, _doNeighborFraming);
			}
			return UnitApply(origin, x, y, args);
		}
	}

	public class UpdateBounds : GenAction
	{
		private DungeonBounds _bounds;

		public UpdateBounds(DungeonBounds bounds)
		{
			_bounds = bounds;
		}

		public override bool Apply(Point origin, int x, int y, params object[] args)
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			_bounds.UpdateBounds(x, y);
			return UnitApply(origin, x, y, args);
		}
	}

	public class DebugDraw : GenAction
	{
		private Color _color;

		private SpriteBatch _spriteBatch;

		public DebugDraw(SpriteBatch spriteBatch, Color color = default(Color))
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			_spriteBatch = spriteBatch;
			_color = color;
		}

		public override bool Apply(Point origin, int x, int y, params object[] args)
		{
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			_spriteBatch.Draw(TextureAssets.MagicPixel.Value, new Rectangle((x << 4) - (int)Main.screenPosition.X, (y << 4) - (int)Main.screenPosition.Y, 16, 16), _color);
			return UnitApply(origin, x, y, args);
		}
	}

	public class SetSlope : GenAction
	{
		private int _slope;

		public SetSlope(int slope)
		{
			_slope = slope;
		}

		public override bool Apply(Point origin, int x, int y, params object[] args)
		{
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			WorldGen.SlopeTile(x, y, _slope);
			return UnitApply(origin, x, y, args);
		}
	}

	public class SetHalfTile : GenAction
	{
		private bool _halfTile;

		public SetHalfTile(bool halfTile)
		{
			_halfTile = halfTile;
		}

		public override bool Apply(Point origin, int x, int y, params object[] args)
		{
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			GenBase._tiles[x, y].halfBrick(_halfTile);
			return UnitApply(origin, x, y, args);
		}
	}

	public class SetTilePaint : GenAction
	{
		private byte paintID;

		public SetTilePaint(byte? paintID)
			: this((byte)(paintID.HasValue ? paintID.Value : 0))
		{
		}

		public SetTilePaint(byte paintID)
		{
			this.paintID = paintID;
		}

		public override bool Apply(Point origin, int x, int y, params object[] args)
		{
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			if (paintID != 0)
			{
				GenBase._tiles[x, y].color(paintID);
			}
			return UnitApply(origin, x, y, args);
		}
	}

	public class ClearTilePaint : GenAction
	{
		public override bool Apply(Point origin, int x, int y, params object[] args)
		{
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			GenBase._tiles[x, y].color(0);
			return UnitApply(origin, x, y, args);
		}
	}

	public class SetWallPaint : GenAction
	{
		private byte paintID;

		public SetWallPaint(byte? paintID)
			: this((byte)(paintID.HasValue ? paintID.Value : 0))
		{
		}

		public SetWallPaint(byte paintID)
		{
			this.paintID = paintID;
		}

		public override bool Apply(Point origin, int x, int y, params object[] args)
		{
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			if (paintID != 0)
			{
				GenBase._tiles[x, y].wallColor(paintID);
			}
			return UnitApply(origin, x, y, args);
		}
	}

	public class ClearWallPaint : GenAction
	{
		public override bool Apply(Point origin, int x, int y, params object[] args)
		{
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			GenBase._tiles[x, y].wallColor(0);
			return UnitApply(origin, x, y, args);
		}
	}

	public class SetTileAndWallPaint : GenAction
	{
		private byte paintID;

		public SetTileAndWallPaint(byte paintID)
		{
			this.paintID = paintID;
		}

		public override bool Apply(Point origin, int x, int y, params object[] args)
		{
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			if (paintID == 0)
			{
				return Fail();
			}
			if (GenBase._tiles[x, y].active())
			{
				GenBase._tiles[x, y].color(paintID);
			}
			if (GenBase._tiles[x, y].wall != 0)
			{
				GenBase._tiles[x, y].wallColor(paintID);
			}
			return UnitApply(origin, x, y, args);
		}
	}

	public class ClearTileAndWallPaint : GenAction
	{
		public override bool Apply(Point origin, int x, int y, params object[] args)
		{
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			GenBase._tiles[x, y].color(0);
			GenBase._tiles[x, y].wallColor(0);
			return UnitApply(origin, x, y, args);
		}
	}

	public class SetTileAndWallRainbowPaint : GenAction
	{
		public override bool Apply(Point origin, int x, int y, params object[] args)
		{
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			byte rainbowPaintIDForPosition = WorldGen.GetRainbowPaintIDForPosition(x, y);
			if (GenBase._tiles[x, y].active())
			{
				GenBase._tiles[x, y].color(rainbowPaintIDForPosition);
			}
			if (GenBase._tiles[x, y].wall != 0)
			{
				GenBase._tiles[x, y].wallColor(rainbowPaintIDForPosition);
			}
			return UnitApply(origin, x, y, args);
		}
	}

	public class PlaceTile : GenAction
	{
		private ushort _type;

		private int _style;

		public PlaceTile(ushort type, int style = 0)
		{
			_type = type;
			_style = style;
		}

		public override bool Apply(Point origin, int x, int y, params object[] args)
		{
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			WorldGen.PlaceTile(x, y, _type, mute: true, forced: false, -1, _style);
			return UnitApply(origin, x, y, args);
		}
	}

	public class RemoveWall : GenAction
	{
		public override bool Apply(Point origin, int x, int y, params object[] args)
		{
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			GenBase._tiles[x, y].wall = 0;
			return UnitApply(origin, x, y, args);
		}
	}

	public class PlaceWall : GenAction
	{
		private ushort _type;

		private bool _neighbors;

		public PlaceWall(ushort type, bool neighbors = true)
		{
			_type = type;
			_neighbors = neighbors;
		}

		public override bool Apply(Point origin, int x, int y, params object[] args)
		{
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			GenBase._tiles[x, y].wall = _type;
			WorldGen.SquareWallFrame(x, y);
			if (_neighbors)
			{
				WorldGen.SquareWallFrame(x + 1, y);
				WorldGen.SquareWallFrame(x - 1, y);
				WorldGen.SquareWallFrame(x, y - 1);
				WorldGen.SquareWallFrame(x, y + 1);
			}
			return UnitApply(origin, x, y, args);
		}
	}

	public class SetLiquid : GenAction
	{
		private int _type;

		private byte _value;

		public SetLiquid(int type = 0, byte value = byte.MaxValue)
		{
			_value = value;
			_type = type;
		}

		public override bool Apply(Point origin, int x, int y, params object[] args)
		{
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			GenBase._tiles[x, y].liquidType(_type);
			GenBase._tiles[x, y].liquid = _value;
			return UnitApply(origin, x, y, args);
		}
	}

	public class SwapSolidTile : GenAction
	{
		private ushort _type;

		public SwapSolidTile(ushort type)
		{
			_type = type;
		}

		public override bool Apply(Point origin, int x, int y, params object[] args)
		{
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			Tile tile = GenBase._tiles[x, y];
			if (WorldGen.SolidTile(tile))
			{
				tile.ResetToType(_type);
				return UnitApply(origin, x, y, args);
			}
			return Fail();
		}
	}

	public class SetFrames : GenAction
	{
		private bool _frameNeighbors;

		public SetFrames(bool frameNeighbors = false)
		{
			_frameNeighbors = frameNeighbors;
		}

		public override bool Apply(Point origin, int x, int y, params object[] args)
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			WorldUtils.TileFrame(x, y, _frameNeighbors);
			return UnitApply(origin, x, y, args);
		}
	}

	public class Smooth : GenAction
	{
		private bool _applyToNeighbors;

		public Smooth(bool applyToNeighbors = false)
		{
			_applyToNeighbors = applyToNeighbors;
		}

		public override bool Apply(Point origin, int x, int y, params object[] args)
		{
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			Tile.SmoothSlope(x, y, _applyToNeighbors);
			return UnitApply(origin, x, y, args);
		}
	}

	public static GenAction Chain(params GenAction[] actions)
	{
		for (int i = 0; i < actions.Length - 1; i++)
		{
			actions[i].NextAction = actions[i + 1];
		}
		return actions[0];
	}

	public static GenAction Continue(GenAction action)
	{
		return new ContinueWrapper(action);
	}
}
