namespace Terraria.GameContent.Items;

public struct TagDamageChanges
{
	public bool AddProjectileTagDamage;

	public int AddedBaseDamage;

	public int AddedFlatDamage;

	public float TotalDamageMultiplier;

	public bool? Crit;

	public int HighestAddedBaseDamage;

	public static TagDamageChanges None => new TagDamageChanges
	{
		TotalDamageMultiplier = 1f
	};
}
