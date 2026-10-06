namespace Terraria.GameContent.Items;

public class WhipTagEffect_Possession : WhipTagEffect
{
	public override bool OnProcHit(Player owner, Projectile optionalProjectile, NPC npcHit, int calcDamage)
	{
		Projectile.SpawnMoonLordWhipProc(optionalProjectile, npcHit, 20, 0);
		return true;
	}
}
