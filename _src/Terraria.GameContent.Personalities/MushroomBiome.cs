namespace Terraria.GameContent.Personalities;

public class MushroomBiome : AShoppingBiome
{
	public MushroomBiome()
	{
		NameKey = "Mushroom";
	}

	public override bool IsInBiome(Player player)
	{
		return player.ZoneGlowshroom;
	}
}
