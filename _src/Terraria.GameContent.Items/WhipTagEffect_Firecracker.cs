using Microsoft.Xna.Framework;

namespace Terraria.GameContent.Items;

public class WhipTagEffect_Firecracker : WhipTagEffect
{
	private static float ProcDamageMultiplier = 2.75f;

	public override void ModifyProcHit(Player owner, Projectile optionalProjectile, NPC npcHit, ref TagDamageChanges changes)
	{
		changes.TotalDamageMultiplier *= ProcDamageMultiplier;
	}

	public override bool OnProcHit(Player owner, Projectile optionalProjectile, NPC npcHit, int calcDamage)
	{
		CreateExplosion(optionalProjectile, npcHit, (int)((float)calcDamage * ProcDamageMultiplier));
		return true;
	}

	private static void CreateExplosion(Projectile projectile, NPC npcHit, int procDamage)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		int num = Projectile.NewProjectile(projectile.GetProjectileSource_FromThis(), npcHit.Center, Vector2.Zero, 918, procDamage, 0f, projectile.owner);
		Main.projectile[num].localNPCImmunity[npcHit.whoAmI] = -1;
	}
}
