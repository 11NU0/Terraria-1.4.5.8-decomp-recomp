using Terraria.GameContent.Generation.Dungeon;

namespace Terraria.GameContent.Events;

public class DangerousDungeonCurse
{
	public static int GetProgressPlayerNeedsToMatch(Player player)
	{
		if (player.ZoneLihzhardTemple)
		{
			return DualDungeonUnbreakableWallTiers.Temple;
		}
		if (player.ZoneHallow)
		{
			return DualDungeonUnbreakableWallTiers.Hallow;
		}
		if (player.ZoneDungeon)
		{
			return DualDungeonUnbreakableWallTiers.Dungeon;
		}
		if (player.ZoneJungle)
		{
			return DualDungeonUnbreakableWallTiers.JungleBoss;
		}
		if (player.ZoneCrimson || player.ZoneCorrupt)
		{
			return DualDungeonUnbreakableWallTiers.EvilBoss;
		}
		return DualDungeonUnbreakableWallTiers.EarlyGame;
	}

	public static int GetProgressPlayerCanSafelyMatch()
	{
		if (NPC.downedMechBossAny || NPC.downedQueenSlime)
		{
			return DualDungeonUnbreakableWallTiers.Temple;
		}
		if (NPC.downedBoss3 || Main.hardMode)
		{
			return DualDungeonUnbreakableWallTiers.Hallow;
		}
		if (NPC.downedQueenBee)
		{
			return DualDungeonUnbreakableWallTiers.Dungeon;
		}
		if (NPC.downedBoss2)
		{
			return DualDungeonUnbreakableWallTiers.JungleBoss;
		}
		if (NPC.downedSlimeKing || NPC.downedBoss1)
		{
			return DualDungeonUnbreakableWallTiers.EvilBoss;
		}
		return DualDungeonUnbreakableWallTiers.EarlyGame;
	}
}
