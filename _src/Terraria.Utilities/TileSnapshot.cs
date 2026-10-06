using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Microsoft.Xna.Framework;
using ReLogic.Threading;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.IO;

namespace Terraria.Utilities;

public static class TileSnapshot
{
	[StructLayout(LayoutKind.Explicit)]
	public struct TileStruct
	{
		[FieldOffset(0)]
		private ushort _type;

		[FieldOffset(2)]
		private ushort _wall_bTileHeader3_packed;

		[FieldOffset(4)]
		private ushort _sTileHeader;

		[FieldOffset(6)]
		private short _frameX;

		[FieldOffset(8)]
		private short _frameY;

		[FieldOffset(10)]
		private byte _liquid;

		[FieldOffset(11)]
		private byte _bTileHeader;

		[FieldOffset(0)]
		private int _i0;

		[FieldOffset(4)]
		private int _i1;

		[FieldOffset(8)]
		private int _i2;

		private static string[] _liquidNames = new string[4] { "water", "lava", "honey", "shimmer" };

		public static TileStruct From(Tile tile)
		{
			TileStruct result = new TileStruct
			{
				_type = tile.type
			};
			ushort wall = tile.wall;
			result._sTileHeader = tile.sTileHeader;
			result._liquid = tile.liquid;
			result._bTileHeader = tile.bTileHeader;
			result._wall_bTileHeader3_packed = (ushort)(wall | ((tile.bTileHeader3 & 0xE0) << 8));
			if ((result._sTileHeader & 0x20) == 0)
			{
				result._type = 0;
				result._sTileHeader &= 35808;
			}
			else
			{
				if (Main.tileFrameImportant[result._type])
				{
					result._frameX = tile.frameX;
					result._frameY = tile.frameY;
				}
				if ((result._sTileHeader & 0x7400) != 0 && !TileID.Sets.SaveSlopes[result._type])
				{
					result._sTileHeader &= 35839;
				}
			}
			if (wall == 0)
			{
				result._bTileHeader &= 224;
			}
			if (result._liquid == 0)
			{
				result._bTileHeader &= 159;
			}
			return result;
		}

		public void Apply(Tile tile)
		{
			tile.type = _type;
			tile.wall = (ushort)(_wall_bTileHeader3_packed & 0x1FFF);
			tile.sTileHeader = _sTileHeader;
			tile.frameX = _frameX;
			tile.frameY = _frameY;
			tile.liquid = _liquid;
			tile.bTileHeader = _bTileHeader;
			tile.bTileHeader2 = 0;
			tile.bTileHeader3 = (byte)((_wall_bTileHeader3_packed >> 8) & 0xE0);
		}

		public static bool operator ==(TileStruct lhs, TileStruct rhs)
		{
			if (lhs._i0 == rhs._i0 && lhs._i1 == rhs._i1)
			{
				return lhs._i2 == rhs._i2;
			}
			return false;
		}

		public static bool operator !=(TileStruct lhs, TileStruct rhs)
		{
			return !(lhs == rhs);
		}

		public override bool Equals(object obj)
		{
			if (!(obj is TileStruct))
			{
				return false;
			}
			return (TileStruct)obj == this;
		}

		public override int GetHashCode()
		{
			return _i0 ^ _i1 ^ _i2;
		}

		public override string ToString()
		{
			bool flag = (_sTileHeader & 0x20) != 0;
			int num = _sTileHeader & 0x1F;
			bool flag2 = (_sTileHeader & 0x400) != 0;
			int num2 = (_sTileHeader & 0x7000) >> 12;
			int num3 = _wall_bTileHeader3_packed & 0x1FFF;
			int num4 = _bTileHeader & 0x1F;
			int num5 = (_bTileHeader & 0x60) >> 5;
			bool flag3 = (_sTileHeader & 0x80) != 0;
			bool flag4 = (_sTileHeader & 0x100) != 0;
			bool flag5 = (_sTileHeader & 0x200) != 0;
			bool flag6 = (_bTileHeader & 0x80) != 0;
			bool flag7 = (_sTileHeader & 0x800) != 0;
			bool flag8 = (_sTileHeader & 0x40) != 0;
			bool flag9 = (_wall_bTileHeader3_packed & 0x2000) != 0;
			bool flag10 = (_wall_bTileHeader3_packed & 0x4000) != 0;
			bool flag11 = (_sTileHeader & 0x8000) != 0;
			bool flag12 = (_wall_bTileHeader3_packed & 0x8000) != 0;
			string text = "!tile";
			if (flag)
			{
				text = "tile:" + _type;
				if (num > 0)
				{
					text = text + "c" + num;
				}
				if (flag2)
				{
					text += "h";
				}
				if (num2 != 0)
				{
					text = text + "s" + num2;
				}
				if (Main.tileFrameImportant[_type])
				{
					text = text + " f" + _frameX + "," + _frameY;
				}
			}
			string text2 = "!wall";
			if (num3 > 0)
			{
				text2 = "wall:" + num3;
				if (num4 > 0)
				{
					text2 = text2 + "c" + num4;
				}
			}
			string text3 = "!liquid";
			if (_liquid > 0)
			{
				text3 = _liquidNames[num5] + ":" + _liquid;
			}
			return $"{text} {text2} {text3} flags:{(flag7 ? 'a' : '0')}{(flag8 ? 'x' : '0')} {(flag9 ? 'E' : '0')}{(flag10 ? 'e' : '0')} {(flag12 ? 'F' : '0')}{(flag11 ? 'f' : '0')} {(flag3 ? 'r' : '0')}{(flag4 ? 'b' : '0')}{(flag5 ? 'g' : '0')}{(flag6 ? 'y' : '0')}";
		}

		public void Write(BinaryWriter writer)
		{
			writer.Write(_i0);
			writer.Write(_i1);
			writer.Write(_i2);
		}

		public static TileStruct Read(BinaryReader reader)
		{
			return new TileStruct
			{
				_i0 = reader.ReadInt32(),
				_i1 = reader.ReadInt32(),
				_i2 = reader.ReadInt32()
			};
		}
	}

	[Serializable]
	[CompilerGenerated]
	private sealed class _003C_003Ec
	{
		public static readonly _003C_003Ec _003C_003E9 = new _003C_003Ec();

		public static ParallelForAction _003C_003E9__10_0;

		public static ParallelForAction _003C_003E9__23_0;

		public static ParallelForAction _003C_003E9__28_0;

		internal void _003CSaveTiles_003Eb__10_0(int x0, int x1, object _)
		{
			Tile[,] tile = Main.tile;
			TileStruct[] tiles = _tiles;
			int maxTilesY = Main.maxTilesY;
			for (int i = x0; i < x1; i++)
			{
				for (int j = 0; j < maxTilesY; j++)
				{
					tiles[i * maxTilesY + j] = TileStruct.From(tile[i, j]);
				}
			}
		}

		internal void _003CRestoreTiles_003Eb__23_0(int x0, int x1, object _)
		{
			TileStruct[] tiles = _tiles;
			Tile[,] tile = Main.tile;
			int maxTilesY = Main.maxTilesY;
			for (int i = x0; i < x1; i++)
			{
				for (int j = 0; j < maxTilesY; j++)
				{
					tiles[i * maxTilesY + j].Apply(tile[i, j]);
				}
			}
			Liquid.numLiquid = 0;
			LiquidBuffer.numLiquidBuffer = 0;
		}

		internal void _003CSwapTiles_003Eb__28_0(int x0, int x1, object _)
		{
			Tile[,] tile = Main.tile;
			TileStruct[] tiles = _tiles;
			int maxTilesY = Main.maxTilesY;
			for (int i = x0; i < x1; i++)
			{
				for (int j = 0; j < maxTilesY; j++)
				{
					Tile tile2 = tile[i, j];
					TileStruct t = TileStruct.From(tile2);
					Utils.Swap(ref tiles[i * maxTilesY + j], ref t);
					t.Apply(tile2);
				}
			}
		}
	}

	private static WorldFileData _worldFile;

	private static TileStruct[] _tiles;

	private static List<TileEntity> _tileEntities;

	private static List<Chest> _chests;

	private static MemoryStream _tempStream = new MemoryStream();

	private static BinaryWriter _tempWriter = new BinaryWriter(_tempStream);

	private static BinaryReader _tempReader = new BinaryReader(_tempStream);

	public static object Context { get; private set; }

	public static bool IsCreated => _tiles != null;

	public static bool SizeMatches
	{
		get
		{
			if (_worldFile.WorldSizeX == Main.ActiveWorldFileData.WorldSizeX)
			{
				return _worldFile.WorldSizeY == Main.ActiveWorldFileData.WorldSizeY;
			}
			return false;
		}
	}

	public static void Create(object context = null)
	{
		Context = context;
		_worldFile = Main.ActiveWorldFileData;
		SaveTiles();
		SaveTileEntities(clone: true);
		SaveChests(clone: true);
	}

	private static void SaveTiles()
	{
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Expected Obj, but got Unknown
		Array.Resize(ref _tiles, Main.maxTilesX * Main.maxTilesY);
		int maxTilesX = Main.maxTilesX;
		ParallelForAction val = _003C_003Ec._003C_003E9__10_0;
		if (val == null)
		{
			ParallelForAction val2 = (int x0, int x1, object _) =>
			{
				Tile[,] tile = Main.tile;
				TileStruct[] tiles = _tiles;
				int maxTilesY = Main.maxTilesY;
				for (int i = x0; i < x1; i++)
				{
					for (int j = 0; j < maxTilesY; j++)
					{
						tiles[i * maxTilesY + j] = TileStruct.From(tile[i, j]);
					}
				}
			};
			_003C_003Ec._003C_003E9__10_0 = val2;
			val = val2;
		}
		FastParallel.For(0, maxTilesX, val, (object)null);
	}

	private static void SaveTileEntities(bool clone)
	{
		if (_tileEntities == null)
		{
			_tileEntities = new List<TileEntity>(TileEntity.ByID.Count);
		}
		_tileEntities.Clear();
		lock (TileEntity.EntityCreationLock)
		{
			foreach (TileEntity value in TileEntity.ByID.Values)
			{
				_tileEntities.Add(clone ? Clone(value) : value);
			}
		}
	}

	private static void SaveChests(bool clone)
	{
		if (_chests == null)
		{
			_chests = new List<Chest>(8000);
		}
		_chests.Clear();
		Chest[] chest = Main.chest;
		foreach (Chest chest2 in chest)
		{
			if (chest2 != null)
			{
				_chests.Add(clone ? chest2.CloneWithSeparateItems() : chest2);
			}
		}
	}

	public static IEnumerable<Point> Compare()
	{
		bool any = false;
		for (int x = 0; x < Main.maxTilesX; x++)
		{
			for (int y = 0; y < Main.maxTilesY; y++)
			{
				TileStruct tileStruct = TileStruct.From(Main.tile[x, y]);
				TileStruct tileStruct2 = _tiles[x * Main.maxTilesY + y];
				if (!(tileStruct == tileStruct2))
				{
					any = true;
					Main.NewText("Mismatch at " + x + ", " + y + " world vs snap");
					Main.NewText(tileStruct.ToString());
					Main.NewText(tileStruct2.ToString());
					yield return new Point(x, y);
				}
			}
		}
		Main.NewText(any ? "No more differences" : "Snapshot matches identically");
	}

	private static TileEntity Clone(TileEntity ent)
	{
		_tempStream.Position = 0L;
		TileEntity.Write(_tempWriter, ent);
		_tempStream.Position = 0L;
		return TileEntity.Read(_tempReader, 326);
	}

	public static void Restore()
	{
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		RestoreTiles();
		RestoreTileEntities(_tileEntities, clone: true);
		RestoreChests(_chests, clone: true);
		if (Main.dedServ)
		{
			NetMessage.ResyncTiles(new Rectangle(0, 0, Main.maxTilesX, Main.maxTilesY));
		}
		else
		{
			Main.sectionManager.SetAllSectionsLoaded();
		}
	}

	private static void RestoreTiles()
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Expected Obj, but got Unknown
		int maxTilesX = Main.maxTilesX;
		ParallelForAction val = _003C_003Ec._003C_003E9__23_0;
		if (val == null)
		{
			ParallelForAction val2 = (int x0, int x1, object _) =>
			{
				TileStruct[] tiles = _tiles;
				Tile[,] tile = Main.tile;
				int maxTilesY = Main.maxTilesY;
				for (int i = x0; i < x1; i++)
				{
					for (int j = 0; j < maxTilesY; j++)
					{
						tiles[i * maxTilesY + j].Apply(tile[i, j]);
					}
				}
				Liquid.numLiquid = 0;
				LiquidBuffer.numLiquidBuffer = 0;
			};
			_003C_003Ec._003C_003E9__23_0 = val2;
			val = val2;
		}
		FastParallel.For(0, maxTilesX, val, (object)null);
	}

	private static void RestoreTileEntities(List<TileEntity> entities, bool clone)
	{
		lock (TileEntity.EntityCreationLock)
		{
			LeashedEntity.Clear(keepActiveSections: true);
			TileEntity.Clear();
			foreach (TileEntity entity in entities)
			{
				Restore(clone ? Clone(entity) : entity);
			}
		}
	}

	private static void Restore(TileEntity ent)
	{
		ent.ID = TileEntity.TileEntitiesNextID++;
		TileEntity.Add(ent);
		ent.OnWorldLoaded();
	}

	private static void RestoreChests(List<Chest> chests, bool clone)
	{
		Chest.Clear();
		foreach (Chest chest in chests)
		{
			Chest.Assign(clone ? chest.CloneWithSeparateItems() : chest);
		}
	}

	public static void Swap()
	{
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		_worldFile = Main.ActiveWorldFileData;
		SwapTiles();
		List<TileEntity> tileEntities = _tileEntities;
		_tileEntities = null;
		SaveTileEntities(clone: false);
		RestoreTileEntities(tileEntities, clone: false);
		List<Chest> chests = _chests;
		_chests = null;
		SaveChests(clone: false);
		RestoreChests(chests, clone: false);
		if (Main.dedServ)
		{
			NetMessage.ResyncTiles(new Rectangle(0, 0, Main.maxTilesX, Main.maxTilesY));
		}
		else
		{
			Main.sectionManager.SetAllSectionsLoaded();
		}
	}

	private static void SwapTiles()
	{
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Expected Obj, but got Unknown
		Array.Resize(ref _tiles, Main.maxTilesX * Main.maxTilesY);
		int maxTilesX = Main.maxTilesX;
		ParallelForAction val = _003C_003Ec._003C_003E9__28_0;
		if (val == null)
		{
			ParallelForAction val2 = (int x0, int x1, object _) =>
			{
				Tile[,] tile = Main.tile;
				TileStruct[] tiles = _tiles;
				int maxTilesY = Main.maxTilesY;
				for (int i = x0; i < x1; i++)
				{
					for (int j = 0; j < maxTilesY; j++)
					{
						Tile tile2 = tile[i, j];
						TileStruct t = TileStruct.From(tile2);
						Utils.Swap(ref tiles[i * maxTilesY + j], ref t);
						t.Apply(tile2);
					}
				}
			};
			_003C_003Ec._003C_003E9__28_0 = val2;
			val = val2;
		}
		FastParallel.For(0, maxTilesX, val, (object)null);
	}

	public static void Clear()
	{
		_tiles = null;
		_tileEntities = null;
		_chests = null;
		GC.Collect();
	}

	public static void Save(BinaryWriter writer)
	{
		writer.Write(Marshal.SizeOf(typeof(TileStruct)));
		TileStruct[] tiles = _tiles;
		foreach (TileStruct tileStruct in tiles)
		{
			tileStruct.Write(writer);
		}
		writer.Write(_tileEntities.Count);
		foreach (TileEntity tileEntity in _tileEntities)
		{
			TileEntity.Write(writer, tileEntity);
		}
		writer.Write(_chests.Count);
		foreach (Chest chest in _chests)
		{
			writer.Write(chest.index);
			writer.Write(chest.x);
			writer.Write(chest.y);
			writer.Write(chest.maxItems);
			writer.Write(chest.name);
			for (int j = 0; j < chest.maxItems; j++)
			{
				Item item = chest.item[j];
				if (item.IsAir)
				{
					writer.Write((ushort)0);
					continue;
				}
				writer.Write((ushort)item.type);
				writer.Write((ushort)item.stack);
				writer.Write(item.prefix);
			}
		}
	}

	public static void Load(BinaryReader reader, object context = null)
	{
		if (reader.ReadInt32() != Marshal.SizeOf(typeof(TileStruct)))
		{
			throw new Exception("TileSnapshot was saved with a different value of #define SNAPSHOT_RUNTIME_DATA");
		}
		Context = context;
		_worldFile = Main.ActiveWorldFileData;
		Array.Resize(ref _tiles, Main.maxTilesX * Main.maxTilesY);
		for (int i = 0; i < _tiles.Length; i++)
		{
			_tiles[i] = TileStruct.Read(reader);
		}
		if (_tileEntities == null)
		{
			_tileEntities = new List<TileEntity>();
		}
		_tileEntities.Clear();
		int num = reader.ReadInt32();
		for (int j = 0; j < num; j++)
		{
			_tileEntities.Add(TileEntity.Read(reader, 326));
		}
		if (_chests == null)
		{
			_chests = new List<Chest>();
		}
		_chests.Clear();
		num = reader.ReadInt32();
		for (int k = 0; k < num; k++)
		{
			Chest chest = Chest.CreateOutOfArray(reader.ReadInt32(), reader.ReadInt32(), reader.ReadInt32(), reader.ReadInt32());
			chest.name = reader.ReadString();
			chest.FillWithEmptyInstances();
			for (int l = 0; l < chest.maxItems; l++)
			{
				int num2 = reader.ReadUInt16();
				if (num2 != 0)
				{
					Item obj = chest.item[l];
					obj.SetDefaults(num2);
					obj.stack = reader.ReadUInt16();
					obj.Prefix(reader.ReadByte());
				}
			}
			_chests.Add(chest);
		}
	}

	public static void Save(string path)
	{
		using BinaryWriter writer = new BinaryWriter(File.Create(path));
		Save(writer);
	}

	public static void Load(string path, object context = null)
	{
		using BinaryReader reader = new BinaryReader(File.OpenRead(path));
		Load(reader, context);
	}
}
