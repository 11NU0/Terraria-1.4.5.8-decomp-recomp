namespace Terraria.GameContent.Items;

public class WhipTagEffect_Constellation : WhipTagEffect
{
	public override void OnTaggedHit(Player owner, Projectile optionalProjectile, NPC npcHit, int calcDamage)
	{
		owner.TagEffectStack.TryEnableProcOnNPC(5479, npcHit);
	}

	public override bool OnProcHit(Player owner, Projectile optionalProjectile, NPC npcHit, int calcDamage)
	{
		if (optionalProjectile != null)
		{
			return optionalProjectile.type == 1034;
		}
		return false;
	}
}
