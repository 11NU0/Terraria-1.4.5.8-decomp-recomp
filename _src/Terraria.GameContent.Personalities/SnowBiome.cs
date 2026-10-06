namespace Terraria.GameContent.Personalities;

public class SnowBiome : AShoppingBiome
{
	public SnowBiome()
	{
		NameKey = "Snow";
	}

	public override bool IsInBiome(Player player)
	{
		return player.ZoneSnow;
	}
}
