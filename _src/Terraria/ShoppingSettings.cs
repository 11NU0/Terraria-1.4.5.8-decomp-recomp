namespace Terraria;

public struct ShoppingSettings
{
	public float PriceAdjustment;

	public string HappinessReport;

	public static ShoppingSettings NotInShop => new ShoppingSettings
	{
		PriceAdjustment = 1f,
		HappinessReport = ""
	};
}
