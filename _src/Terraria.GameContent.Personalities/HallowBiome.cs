namespace Terraria.GameContent.Personalities;

public class HallowBiome : AShoppingBiome
{
	public HallowBiome()
	{
		NameKey = "Hallow";
	}

	public override bool IsInBiome(Player player)
	{
		return player.ZoneHallow;
	}
}
