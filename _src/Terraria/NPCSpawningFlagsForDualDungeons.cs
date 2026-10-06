using Terraria.GameContent.Generation.Dungeon;
using Terraria.ID;

namespace Terraria;

public struct NPCSpawningFlagsForDualDungeons
{
	public bool ZoneDungeon;

	public bool ZoneSnow;

	public bool ZoneGlowshroom;

	public bool ZoneCorrupt;

	public bool ZoneCrimson;

	public bool ZoneJungle;

	public bool ZoneHallow;

	public bool ZoneLihzhardTemple;

	public bool ZoneUndergroundDesert;

	public bool CanScan(int x, int y)
	{
		if (!WorldGen.SolidTile3(x, y))
		{
			return false;
		}
		ushort type = Main.tile[x, y].type;
		if (TileID.Sets.Boulders[type])
		{
			return false;
		}
		if (type == 48 || type == 137 || type == 232)
		{
			return false;
		}
		return true;
	}

	public bool ScanZonesFor(int x, int y)
	{
		ushort type = Main.tile[x, y].type;
		ushort wall = Framing.GetTileSafely(x, y - 1).wall;
		bool result = false;
		switch (type)
		{
		case 109:
		case 110:
		case 113:
		case 116:
		case 117:
		case 118:
		case 164:
		case 385:
		case 402:
		case 403:
		case 492:
			ZoneHallow = true;
			result = true;
			break;
		case 147:
		case 148:
		case 161:
		case 162:
		case 206:
		case 224:
			ZoneSnow = true;
			result = true;
			break;
		case 60:
		case 61:
		case 62:
		case 74:
		case 225:
		case 383:
		case 384:
			ZoneJungle = true;
			result = true;
			break;
		case 59:
		case 120:
			if (wall > 0 && WallID.Sets.DualDungeonsJungleBiomeWalls[wall])
			{
				ZoneJungle = true;
				result = true;
			}
			break;
		case 1:
		case 38:
			if (wall > 0 && DungeonGenerationStyles.Cavern.WallIsInStyle(wall))
			{
				result = true;
			}
			break;
		case 191:
			result = true;
			break;
		case 23:
		case 24:
		case 25:
		case 32:
		case 112:
		case 398:
		case 400:
		case 474:
		case 661:
			ZoneCorrupt = true;
			result = true;
			break;
		case 163:
			ZoneSnow = (ZoneCorrupt = true);
			result = true;
			break;
		case 200:
			ZoneSnow = (ZoneCrimson = true);
			result = true;
			break;
		case 195:
		case 199:
		case 201:
		case 203:
		case 234:
		case 352:
		case 399:
		case 401:
		case 662:
			ZoneCrimson = true;
			result = true;
			break;
		case 41:
		case 43:
		case 44:
		case 481:
		case 482:
		case 483:
			if ((double)y > Main.rockLayer)
			{
				ZoneDungeon = true;
			}
			result = true;
			break;
		case 226:
			ZoneLihzhardTemple = true;
			result = true;
			break;
		case 70:
		case 71:
		case 72:
		case 528:
			ZoneGlowshroom = true;
			result = true;
			break;
		}
		if (type == 123 && wall > 0)
		{
			if (wall > 0 && DungeonGenerationStyles.Cavern.WallIsInStyle(wall))
			{
				result = true;
			}
			else if (WallID.Sets.DualDungeonsJungleBiomeWalls[wall])
			{
				ZoneJungle = true;
				result = true;
			}
			else if (wall == DungeonGenerationStyles.Temple.BrickWallType)
			{
				ZoneLihzhardTemple = true;
				result = true;
			}
		}
		switch (type)
		{
		case 22:
		case 140:
			if (wall == 69 || wall == 217 || wall == 220 || wall == 3 || wall == 233)
			{
				ZoneCorrupt = true;
				result = true;
			}
			break;
		case 204:
		case 347:
			if (wall == 81 || wall == 218 || wall == 221 || wall == 83 || wall == 77)
			{
				ZoneCrimson = true;
				result = true;
			}
			break;
		case 53:
		case 112:
		case 116:
		case 234:
		case 396:
		case 397:
		case 398:
		case 399:
		case 400:
		case 401:
		case 402:
		case 403:
			if (WallID.Sets.Conversion.HardenedSand[wall] || WallID.Sets.Conversion.Sandstone[wall] || wall == 223)
			{
				ZoneUndergroundDesert = true;
				result = true;
			}
			break;
		}
		return result;
	}
}
