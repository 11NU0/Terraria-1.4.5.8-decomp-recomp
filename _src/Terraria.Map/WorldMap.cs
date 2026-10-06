using System;
using System.IO;
using Terraria.IO;
using Terraria.Social;
using Terraria.Testing;
using Terraria.Utilities;

namespace Terraria.Map;

public class WorldMap
{
	public readonly int MaxWidth;

	public readonly int MaxHeight;

	public const int BlackEdgeWidth = 40;

	private MapTile[,] _tiles;

	public MapTile this[int x, int y] => _tiles[x, y];

	public WorldMap(int maxWidth, int maxHeight)
	{
		MaxWidth = maxWidth;
		MaxHeight = maxHeight;
		_tiles = new MapTile[MaxWidth, MaxHeight];
	}

	public void ConsumeUpdate(int x, int y)
	{
		_tiles[x, y].IsChanged = false;
	}

	public void Update(int x, int y, byte light)
	{
		_tiles[x, y] = MapHelper.CreateMapTile(x, y, light);
	}

	public void SetTile(int x, int y, ref MapTile tile)
	{
		_tiles[x, y] = tile;
	}

	public bool IsRevealed(int x, int y)
	{
		return _tiles[x, y].Light > 0;
	}

	public bool UpdateLighting(int x, int y, byte light)
	{
		MapTile other = _tiles[x, y];
		if (light == 0 && other.Light == 0)
		{
			return false;
		}
		MapTile mapTile = MapHelper.CreateMapTile(x, y, Math.Max(other.Light, light));
		if (mapTile.Equals(other))
		{
			return false;
		}
		_tiles[x, y] = mapTile;
		return true;
	}

	public bool UpdateType(int x, int y)
	{
		return UpdateType(x, y, ref _tiles[x, y]);
	}

	private bool UpdateType(int x, int y, ref MapTile mapTile)
	{
		if (!mapTile.UpdateQueued)
		{
			return false;
		}
		mapTile.UpdateQueued = false;
		if (mapTile.Light == 0)
		{
			return false;
		}
		if (!Main.sectionManager.TileLoaded(x, y))
		{
			return false;
		}
		bool flag = MapHelper.IsBackground(mapTile.Type);
		MapTile mapTile2 = MapHelper.CreateMapTile(x, y, mapTile.Light, flag ? mapTile.Type : 0);
		if (mapTile2.Equals(mapTile))
		{
			return false;
		}
		mapTile = mapTile2;
		return true;
	}

	internal bool QueueUpdate(int x, int y)
	{
		return QueueUpdate(ref _tiles[x, y]);
	}

	private bool QueueUpdate(ref MapTile mapTile)
	{
		if (mapTile.Light == 0 || mapTile.UpdateQueued)
		{
			return false;
		}
		mapTile.UpdateQueued = true;
		return true;
	}

	public void UnlockMapSection(int sectionX, int sectionY)
	{
		int num = sectionX * 200;
		int value = num + 200;
		int num2 = sectionY * 150;
		int value2 = num2 + 150;
		int num3 = 40;
		num = Utils.Clamp(num, num3, Main.maxTilesX - num3);
		value = Utils.Clamp(value, num3, Main.maxTilesX - num3);
		num2 = Utils.Clamp(num2, num3, Main.maxTilesY - num3);
		value2 = Utils.Clamp(value2, num3, Main.maxTilesY - num3);
		if (DebugOptions.unlockMap == 2)
		{
			for (int i = num; i < value; i++)
			{
				for (int j = num2; j < value2; j++)
				{
					UnlockMapTilePretty(i, j);
				}
			}
			return;
		}
		for (int k = num; k < value; k++)
		{
			for (int l = num2; l < value2; l++)
			{
				UpdateLighting(k, l, byte.MaxValue);
			}
		}
	}

	public void UnlockMapTilePretty(int x, int y)
	{
		if (!WorldGen.InWorld(x, y, 12) || WorldGen.SolidTile(x, y))
		{
			return;
		}
		int num = 5;
		float num2 = 255f;
		Tile tileSafely = Framing.GetTileSafely(x, y);
		if (tileSafely.liquid > 0 && !tileSafely.lava())
		{
			return;
		}
		if (tileSafely.wall > 0)
		{
			num2 *= 0.8f;
		}
		if ((double)y >= Main.worldSurface)
		{
			num2 *= 0.7f;
		}
		for (int i = -num; i <= num; i++)
		{
			for (int j = -num; j <= num; j++)
			{
				int x2 = x + i;
				int y2 = y + j;
				float num3 = num - Math.Abs(i) - Math.Abs(j);
				if (num3 >= 0f)
				{
					UpdateLighting(x2, y2, (byte)(num2 * (num3 / (float)num)));
				}
			}
		}
	}

	public void Load()
	{
		Lighting.Clear();
		bool isCloudSave = Main.ActivePlayerFileData.IsCloudSave;
		if ((isCloudSave && SocialAPI.Cloud == null) || !Main.mapEnabled)
		{
			return;
		}
		if (!TryGetMapPath(Main.ActivePlayerFileData, Main.ActiveWorldFileData, out var mapPath))
		{
			Main.MapFileMetadata = FileMetadata.FromCurrentSettings(FileType.Map);
			return;
		}
		using MemoryStream input = new MemoryStream(FileUtilities.ReadAllBytes(mapPath, isCloudSave));
		using BinaryReader binaryReader = new BinaryReader(input);
		try
		{
			int num = binaryReader.ReadInt32();
			bool flag = (num & 0x8000) == 32768;
			num &= -32769;
			if (num <= 326)
			{
				if (flag)
				{
					MapHelper.LoadMapVersionCompressed(binaryReader, num);
				}
				else if (num <= 91)
				{
					MapHelper.LoadMapVersion1(binaryReader, num);
				}
				else
				{
					MapHelper.LoadMapVersion2(binaryReader, num);
				}
				ClearEdges();
				Main.clearMap = true;
				Main.loadMap = true;
				Main.loadMapLock = true;
				Main.refreshMap = false;
			}
		}
		catch (Exception value)
		{
			using (StreamWriter streamWriter = new StreamWriter("client-crashlog.txt", append: true))
			{
				streamWriter.WriteLine(DateTime.Now);
				streamWriter.WriteLine(value);
				streamWriter.WriteLine("");
			}
			if (!isCloudSave)
			{
				File.Copy(mapPath, mapPath + ".bad", overwrite: true);
			}
			Clear();
		}
	}

	public static bool TryGetMapPath(PlayerFileData playerFileData, WorldFileData worldFileData, out string mapPath)
	{
		string text = playerFileData.Path.Substring(0, playerFileData.Path.Length - 4);
		mapPath = text + Path.DirectorySeparatorChar + worldFileData.MapFileName + ".map";
		if (worldFileData.UseGuidAsMapName && !FileUtilities.Exists(mapPath, playerFileData.IsCloudSave))
		{
			mapPath = text + Path.DirectorySeparatorChar.ToString() + worldFileData.WorldId + ".map";
		}
		return FileUtilities.Exists(mapPath, playerFileData.IsCloudSave);
	}

	public void Save()
	{
		MapHelper.SaveMap();
	}

	public void Clear()
	{
		for (int i = 0; i < MaxWidth; i++)
		{
			for (int j = 0; j < MaxHeight; j++)
			{
				_tiles[i, j].Clear();
			}
		}
	}

	public void ClearEdges()
	{
		for (int i = 0; i < MaxWidth; i++)
		{
			for (int j = 0; j < 40; j++)
			{
				_tiles[i, j].Clear();
			}
		}
		for (int k = 0; k < MaxWidth; k++)
		{
			for (int l = MaxHeight - 40; l < MaxHeight; l++)
			{
				_tiles[k, l].Clear();
			}
		}
		for (int m = 0; m < 40; m++)
		{
			for (int n = 40; n < MaxHeight - 40; n++)
			{
				_tiles[m, n].Clear();
			}
		}
		for (int num = MaxWidth - 40; num < MaxWidth; num++)
		{
			for (int num2 = 40; num2 < MaxHeight - 40; num2++)
			{
				_tiles[num, num2].Clear();
			}
		}
	}
}
