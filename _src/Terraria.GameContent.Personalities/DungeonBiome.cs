namespace Terraria.GameContent.Personalities;

public class DungeonBiome : AShoppingBiome
{
	public DungeonBiome()
	{
		NameKey = "Dungeon";
	}

	public override bool IsInBiome(Player player)
	{
		return player.ZoneDungeon;
	}
}
