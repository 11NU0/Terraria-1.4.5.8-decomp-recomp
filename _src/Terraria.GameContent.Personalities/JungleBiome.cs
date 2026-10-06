namespace Terraria.GameContent.Personalities;

public class JungleBiome : AShoppingBiome
{
	public JungleBiome()
	{
		NameKey = "Jungle";
	}

	public override bool IsInBiome(Player player)
	{
		return player.ZoneJungle;
	}
}
