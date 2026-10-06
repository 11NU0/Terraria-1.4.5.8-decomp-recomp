using Microsoft.Xna.Framework;
using Terraria.GameContent.Drawing;
using Terraria.ID;

namespace Terraria.GameContent.Items;

public class WhipTagEffect_DarkHarvest : WhipTagEffect
{
	public override void OnTaggedHit(Player owner, Projectile optionalProjectile, NPC npcHit, int calcDamage)
	{
		SpawnBlackLightning(optionalProjectile, npcHit);
	}

	private void SpawnBlackLightning(Projectile projectile, NPC npcHit)
	{
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		int damage = (int)((float)TagDamage * ProjectileID.Sets.SummonTagDamageMultiplier[projectile.type]);
		int num = Projectile.NewProjectile(projectile.GetProjectileSource_FromThis(), npcHit.Center, Vector2.Zero, 916, damage, 0f, projectile.owner);
		Main.projectile[num].localNPCImmunity[npcHit.whoAmI] = -1;
		EmitBlackLightningParticles(npcHit);
	}

	private static void EmitBlackLightningParticles(NPC targetNPC)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		ParticleOrchestrator.RequestParticleSpawn(clientOnly: false, ParticleOrchestraType.BlackLightningHit, new ParticleOrchestraSettings
		{
			PositionInWorld = targetNPC.Center
		});
	}
}
