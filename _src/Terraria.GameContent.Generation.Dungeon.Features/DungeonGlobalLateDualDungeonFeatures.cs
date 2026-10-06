using System.Collections.Generic;
using Terraria.GameContent.Generation.Dungeon.Rooms;

namespace Terraria.GameContent.Generation.Dungeon.Features;

public class DungeonGlobalLateDualDungeonFeatures : GlobalDungeonFeature
{
	public DungeonGlobalLateDualDungeonFeatures(DungeonFeatureSettings settings)
		: base(settings)
	{
		DungeonCrawler.CurrentDungeonData.dungeonFeatures.Add(this);
	}

	public override bool GenerateFeature(DungeonData data)
	{
		generated = false;
		LateDualDungeonFeatures(data);
		generated = true;
		return true;
	}

	public void LateDualDungeonFeatures(DungeonData data)
	{
		_ = WorldGen.genRand;
		for (int i = 0; i < data.genVars.dungeonGenerationStyles.Count; i++)
		{
			DungeonGenerationStyleData dungeonGenerationStyleData = data.genVars.dungeonGenerationStyles[i];
			byte style = dungeonGenerationStyleData.Style;
			DungeonBounds dungeonBounds = data.outerProgressionBounds[i];
			if (style == 8 || style == 7)
			{
				if (!dungeonGenerationStyleData.BrickGrassTileType.HasValue)
				{
					continue;
				}
				ushort value = dungeonGenerationStyleData.BrickGrassTileType.Value;
				for (int j = dungeonBounds.Left; j <= dungeonBounds.Right; j++)
				{
					for (int k = dungeonBounds.Top; k <= dungeonBounds.Bottom; k++)
					{
						Tile tile = Main.tile[j, k];
						if (tile.active() && tile.type == 59 && WorldGen.TileIsExposedToAir(j, k))
						{
							tile.type = value;
						}
					}
				}
			}
			if (style != 0)
			{
				continue;
			}
			for (int l = dungeonBounds.Left; l <= dungeonBounds.Right; l++)
			{
				for (int m = dungeonBounds.Top; m <= dungeonBounds.Bottom; m++)
				{
					Tile tile2 = Main.tile[l, m];
					Tile tile3 = Main.tile[l, m + 1];
					if (tile2.active() && tile2.type == 10 && (!tile3.active() || tile3.type != 10))
					{
						ushort wall = tile2.wall;
						if (wall >= 94 && wall <= 105)
						{
							WorldGen.KillTile(l, m);
							WorldGen.PlaceTile(l, m, 10, mute: true, forced: false, -1, 53);
						}
					}
				}
			}
		}
		List<BiomeDungeonRoom> list = new List<BiomeDungeonRoom>();
		for (int n = 0; n < data.dungeonRooms.Count; n++)
		{
			DungeonRoom dungeonRoom = data.dungeonRooms[n];
			if (dungeonRoom is BiomeDungeonRoom)
			{
				list.Add((BiomeDungeonRoom)dungeonRoom);
			}
		}
		while (list.Count > 0)
		{
			BiomeDungeonRoom biomeDungeonRoom = list[0];
			LateDualDungeonFeatures_CleanOutEntrancesOfLiquids(biomeDungeonRoom);
			if (biomeDungeonRoom.settings.StyleData.Style == 8)
			{
				LateDualDungeonFeatures_GrowGrass(biomeDungeonRoom);
			}
			list.Remove(biomeDungeonRoom);
		}
		for (int num = 0; num < data.dungeonRooms.Count; num++)
		{
			data.dungeonRooms[num].GenerateLateDungeonFeaturesInRoom(data);
		}
	}

	private void LateDualDungeonFeatures_CleanOutEntrancesOfLiquids(BiomeDungeonRoom room)
	{
		DungeonBounds innerBounds = room.InnerBounds;
		int num = innerBounds.Left - 1;
		int num2 = innerBounds.Right + 1;
		int num3 = innerBounds.Top - 1;
		int num4 = innerBounds.Bottom + 1;
		int num5 = -1;
		for (int i = 0; i < 2; i++)
		{
			int num6 = ((i == 0) ? num : num2);
			for (int j = num3; j <= num4; j++)
			{
				if (innerBounds.Contains(num6, j))
				{
					continue;
				}
				if (Main.tile[num6, j].active())
				{
					if (num5 != -1)
					{
						LateDualDungeonFeatures_CleanOutEntrancesOfLiquids_ActuallyClearArea(num6, (num5 + j - 1) / 2);
					}
					num5 = -1;
				}
				else if (num5 == -1)
				{
					num5 = j;
				}
			}
		}
		int num7 = -1;
		for (int k = 0; k < 2; k++)
		{
			int num8 = ((k == 0) ? num3 : num4);
			for (int l = num; l <= num2; l++)
			{
				if (innerBounds.Contains(l, num8))
				{
					continue;
				}
				if (Main.tile[l, num8].active())
				{
					if (num7 != -1)
					{
						LateDualDungeonFeatures_CleanOutEntrancesOfLiquids_ActuallyClearArea((num7 + l - 1) / 2, num8);
					}
					num7 = -1;
				}
				else if (num7 == -1)
				{
					num7 = l;
				}
			}
		}
	}

	private void LateDualDungeonFeatures_CleanOutEntrancesOfLiquids_ActuallyClearArea(int x, int y)
	{
		int num = 30;
		for (int i = -num; i <= num; i++)
		{
			for (int j = -num; j <= num; j++)
			{
				Tile tile = Main.tile[x + i, y + j];
				if (tile.liquid > 0)
				{
					tile.liquid = 0;
					tile.liquidType(0);
				}
			}
		}
	}

	private void LateDualDungeonFeatures_GrowGrass(DungeonRoom room)
	{
		if (room != null && room.settings.StyleData.BrickGrassTileType.HasValue)
		{
			LateDualDungeonFeatures_GrowGrass(room, room.settings.StyleData.BrickTileType, room.settings.StyleData.BrickGrassTileType.Value);
		}
	}

	private void LateDualDungeonFeatures_GrowGrass(DungeonRoom room, ushort dirtType, ushort grassType)
	{
		if (room == null)
		{
			return;
		}
		DungeonBounds outerBounds = room.OuterBounds;
		for (int i = outerBounds.Left; i <= outerBounds.Right; i++)
		{
			for (int j = outerBounds.Top; j <= outerBounds.Bottom; j++)
			{
				Tile tile = Main.tile[i, j];
				if (tile.active() && tile.type == dirtType && WorldGen.TileIsExposedToAir(i, j))
				{
					tile.type = grassType;
				}
			}
		}
	}
}
