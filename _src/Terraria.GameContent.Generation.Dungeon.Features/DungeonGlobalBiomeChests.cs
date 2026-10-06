using System.Collections.Generic;
using Terraria.GameContent.Generation.Dungeon.Rooms;
using Terraria.Utilities;

namespace Terraria.GameContent.Generation.Dungeon.Features;

public class DungeonGlobalBiomeChests : GlobalDungeonFeature
{
	public DungeonGlobalBiomeChests(DungeonFeatureSettings settings)
		: base(settings)
	{
		DungeonCrawler.CurrentDungeonData.dungeonFeatures.Add(this);
	}

	public override bool GenerateFeature(DungeonData data)
	{
		generated = false;
		if (data.Type == DungeonType.DualDungeon)
		{
			BiomeChests_DualDungeons(data);
		}
		else
		{
			BiomeChests(data);
		}
		generated = true;
		return true;
	}

	private void BiomeChests_DualDungeons(DungeonData data)
	{
		UnifiedRandom genRand = WorldGen.genRand;
		List<int> list = new List<int>();
		list.Add(8);
		list.Add(WorldGen.crimson ? 5 : 4);
		list.Add(6);
		list.Add(2);
		list.Add(3);
		if (WorldGen.drunkWorldGen)
		{
			list.Add(WorldGen.crimson ? 4 : 5);
		}
		List<DungeonRoom>[] array = new List<DungeonRoom>[16];
		DungeonRoom[] array2 = new DungeonRoom[16];
		for (int i = 0; i < data.dungeonRooms.Count; i++)
		{
			DungeonRoom dungeonRoom = data.dungeonRooms[i];
			if (dungeonRoom is BiomeDungeonRoom)
			{
				array2[dungeonRoom.settings.StyleData.Style] = dungeonRoom;
				continue;
			}
			if (array[dungeonRoom.settings.StyleData.Style] == null)
			{
				array[dungeonRoom.settings.StyleData.Style] = new List<DungeonRoom>();
			}
			array[dungeonRoom.settings.StyleData.Style].Add(dungeonRoom);
		}
		for (int j = 0; j < list.Count; j++)
		{
			int num = list[j];
			bool flag = false;
			DungeonRoom dungeonRoom2 = array2[num];
			if (dungeonRoom2 != null)
			{
				int num2 = 50;
				while (num2 > 0)
				{
					num2--;
					if (dungeonRoom2.DualDungeons_TryGenerateBiomeChestInRoom(data, this))
					{
						flag = true;
						break;
					}
				}
			}
			if (flag)
			{
				continue;
			}
			List<DungeonRoom> list2 = array[num];
			if (list2 == null)
			{
				continue;
			}
			int num3 = 30 * list2.Count;
			while (num3 > 0 && list2.Count > 0)
			{
				num3--;
				DungeonRoom dungeonRoom3 = list2[genRand.Next(list2.Count)];
				if (dungeonRoom3.DualDungeons_TryGenerateBiomeChestInRoom(data, this))
				{
					flag = true;
				}
				list2.Remove(dungeonRoom3);
				if (flag)
				{
					break;
				}
			}
		}
	}

	private void BiomeChests(DungeonData data)
	{
		UnifiedRandom genRand = WorldGen.genRand;
		int num = 5;
		if (WorldGen.drunkWorldGen)
		{
			num = 6;
		}
		int num2 = num;
		for (int i = 0; i < num2; i++)
		{
			bool flag = false;
			int num3 = 1000;
			while (!flag)
			{
				num3--;
				if (num3 <= 0)
				{
					break;
				}
				int num4 = genRand.Next(data.dungeonBounds.Left, data.dungeonBounds.Right);
				int num5 = genRand.Next((int)Main.worldSurface, data.dungeonBounds.Bottom);
				if ((data.dungeonEntrance != null && data.dungeonEntrance.Bounds.Contains(num4, num5)) || !DungeonUtils.IsConsideredDungeonWall(Main.tile[num4, num5].wall) || Main.tile[num4, num5].active())
				{
					continue;
				}
				ushort chestTileType = 21;
				int mainItemInChest = 0;
				int chestStyle = 0;
				switch (i % num)
				{
				case 0:
					chestStyle = 23;
					mainItemInChest = 1156;
					break;
				case 1:
					if (!WorldGen.crimson)
					{
						chestStyle = 24;
						mainItemInChest = 1571;
					}
					else
					{
						chestStyle = 25;
						mainItemInChest = 1569;
					}
					break;
				case 2:
					chestStyle = 26;
					mainItemInChest = 1260;
					break;
				case 3:
					chestStyle = 27;
					mainItemInChest = 1572;
					break;
				case 4:
					chestTileType = 467;
					chestStyle = 13;
					mainItemInChest = 4607;
					break;
				case 5:
					if (WorldGen.crimson)
					{
						chestStyle = 24;
						mainItemInChest = 1571;
					}
					else
					{
						chestStyle = 25;
						mainItemInChest = 1569;
					}
					break;
				}
				flag = WorldGen.AddBuriedChest(num4, num5, mainItemInChest, notNearOtherChests: false, chestStyle, trySlope: false, chestTileType);
			}
		}
	}
}
