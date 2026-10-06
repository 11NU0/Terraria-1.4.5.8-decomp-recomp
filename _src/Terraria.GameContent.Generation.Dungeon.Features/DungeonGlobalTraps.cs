using Terraria.GameContent.Generation.Dungeon.Halls;
using Terraria.GameContent.Generation.Dungeon.Rooms;
using Terraria.Utilities;
using Terraria.WorldBuilding;

namespace Terraria.GameContent.Generation.Dungeon.Features;

public class DungeonGlobalTraps : GlobalDungeonFeature
{
	public DungeonGlobalTraps(DungeonFeatureSettings settings)
		: base(settings)
	{
		DungeonCrawler.CurrentDungeonData.dungeonFeatures.Add(this);
	}

	public override bool GenerateFeature(DungeonData data)
	{
		generated = false;
		Traps(data);
		generated = true;
		return true;
	}

	public void Traps(DungeonData data)
	{
		UnifiedRandom genRand = WorldGen.genRand;
		float num = (float)Main.maxTilesX / 4200f;
		int num2 = 0;
		int num3 = 1000;
		int num4 = 0;
		int num5 = (int)((double)(8.4f * num) * data.globalFeatureScalar);
		Main.tileSolid[379] = false;
		GenVars.DualDungeon_AllowTrapGenerationInDungeon = true;
		while (num4 < num5)
		{
			num2++;
			int num6 = genRand.Next(data.dungeonBounds.Left, data.dungeonBounds.Right);
			int num7 = genRand.Next((int)Main.worldSurface, data.dungeonBounds.Bottom);
			if (data.Type == DungeonType.DualDungeon)
			{
				if (DungeonUtils.IsConsideredDungeonWall(Main.tile[num6, num7].wall) && data.CanGenerateFeatureAt(this, num6, num7) && !DungeonGenerationStyles.Temple.WallIsInStyle(Main.tile[num6, num7].wall) && WorldGen.placeTrap(num6, num7, 0))
				{
					num2 = num3;
				}
			}
			else if (DungeonUtils.IsConsideredDungeonWall(Main.tile[num6, num7].wall) && WorldGen.placeTrap(num6, num7, 0))
			{
				num2 = num3;
			}
			if (num2 > num3)
			{
				num4++;
				num2 = 0;
			}
		}
		if (data.Type == DungeonType.DualDungeon)
		{
			int num8 = 30;
			switch (WorldGen.GetWorldSize())
			{
			case 0:
				num8 = 30 + genRand.Next(11);
				break;
			case 1:
				num8 = 50 + genRand.Next(16);
				break;
			case 2:
				num8 = 70 + genRand.Next(21);
				break;
			}
			if (WorldGen.SecretSeed.Variations.actuallyNoTrapsForRealIMeanIt)
			{
				num8 = 0;
			}
			if (num8 > 0)
			{
				int num9 = num8;
				for (int i = 0; i < data.dungeonHalls.Count; i++)
				{
					DungeonHall dungeonHall = data.dungeonHalls[i];
					if (!dungeonHall.generated || dungeonHall.settings.StyleData.Style != 10)
					{
						continue;
					}
					DungeonBounds bounds = dungeonHall.Bounds;
					int num10 = bounds.Left + genRand.Next(bounds.Width);
					int num11 = bounds.Top + genRand.Next(bounds.Height);
					Tile tile = Main.tile[num10, num11];
					if (!tile.active() && DungeonGenerationStyles.Temple.WallIsInStyle(tile.wall) && WorldGen.mayanTrap(num10, num11))
					{
						num9--;
						if (num9 <= 0)
						{
							break;
						}
					}
				}
				if (num9 > 0)
				{
					for (int j = 0; j < data.dungeonRooms.Count; j++)
					{
						DungeonRoom dungeonRoom = data.dungeonRooms[j];
						if (!dungeonRoom.generated || dungeonRoom.settings.StyleData.Style != 10)
						{
							continue;
						}
						DungeonBounds innerBounds = dungeonRoom.InnerBounds;
						int num12 = innerBounds.Left + genRand.Next(innerBounds.Width);
						int num13 = innerBounds.Top + genRand.Next(innerBounds.Height);
						Tile tile2 = Main.tile[num12, num13];
						if (!tile2.active() && DungeonGenerationStyles.Temple.WallIsInStyle(tile2.wall) && WorldGen.mayanTrap(num12, num13))
						{
							num9--;
							if (num9 <= 0)
							{
								break;
							}
						}
					}
				}
				if (num9 > 0)
				{
					for (int k = 0; k < data.genVars.dungeonGenerationStyles.Count; k++)
					{
						DungeonGenerationStyleData dungeonGenerationStyleData = data.genVars.dungeonGenerationStyles[k];
						byte style = dungeonGenerationStyleData.Style;
						DungeonBounds dungeonBounds = data.outerProgressionBounds[k];
						if (style != 10)
						{
							continue;
						}
						int num14 = 1000;
						while (num9 > 0)
						{
							num14--;
							if (num14 <= 0)
							{
								break;
							}
							int num15 = dungeonBounds.Left + genRand.Next(dungeonBounds.Width);
							int num16 = dungeonBounds.Top + genRand.Next(dungeonBounds.Height);
							Tile tile3 = Main.tile[num15, num16];
							if (!tile3.active() && dungeonGenerationStyleData.WallIsInStyle(tile3.wall) && WorldGen.mayanTrap(num15, num16))
							{
								num9--;
							}
						}
						break;
					}
				}
			}
		}
		Main.tileSolid[379] = true;
		GenVars.DualDungeon_AllowTrapGenerationInDungeon = false;
	}
}
