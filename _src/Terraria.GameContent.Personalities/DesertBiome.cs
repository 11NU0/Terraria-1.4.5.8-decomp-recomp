namespace Terraria.GameContent.Personalities;

public class DesertBiome : AShoppingBiome
{
	public DesertBiome()
	{
		NameKey = "Desert";
	}

	public override bool IsInBiome(Player player)
	{
		return player.ZoneDesert;
	}
}
