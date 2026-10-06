namespace Terraria.DataStructures;

public class BackgroundVariantSet
{
	public BackgroundVariant Pure = new BackgroundVariant();

	public BackgroundVariant Corrupt = new BackgroundVariant();

	public BackgroundVariant Crimson = new BackgroundVariant();

	public BackgroundVariant Hallow = new BackgroundVariant();

	public void Clear()
	{
		Pure.Clear();
		Corrupt.Clear();
		Crimson.Clear();
		Hallow.Clear();
	}
}
