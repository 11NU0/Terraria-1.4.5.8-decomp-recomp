using Terraria.GameContent.Drawing;

namespace Terraria.GameContent.Items;

public class WhipTagEffect_Kaleidoscope : WhipTagEffect
{
	public override void OnTaggedHit(Player owner, Projectile optionalProjectile, NPC npcHit, int calcDamage)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		ParticleOrchestrator.RequestParticleSpawn(clientOnly: false, ParticleOrchestraType.RainbowRodHit, new ParticleOrchestraSettings
		{
			PositionInWorld = optionalProjectile.Center
		});
	}
}
