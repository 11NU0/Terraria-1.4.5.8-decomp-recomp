namespace Terraria.DataStructures;

public class EntitySource_ItemUse_WithAmmo : EntitySource_ItemUse
{
	public readonly int AmmoItemIdUsed;

	public EntitySource_ItemUse_WithAmmo(IEntitySourceTarget entity, Item item, int ammoItemIdUsed)
		: base(entity, item)
	{
		AmmoItemIdUsed = ammoItemIdUsed;
	}
}
