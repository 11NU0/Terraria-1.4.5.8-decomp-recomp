using System;
using Microsoft.Xna.Framework;

namespace Terraria.GameContent.Items;

public class WhipTagEffect_ViolentDisplayOfFlower : WhipTagEffect
{
	public override bool OnProcHit(Player owner, Projectile optionalProjectile, NPC npcHit, int calcDamage)
	{
		if (optionalProjectile != null)
		{
			SpawnFlowerExplosionOn(optionalProjectile, npcHit, calcDamage);
		}
		return true;
	}

	private void SpawnFlowerExplosionOn(Projectile projectile, NPC targetNPC, int calcDamage)
	{
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		int num = calcDamage + projectile.bonusTagDamage;
		float num2 = Main.rand.NextFloat() * ((float)Math.PI * 2f);
		int num3 = 3;
		int num4 = 3;
		float num5 = 2.25f;
		num3 += Main.rand.Next(0, num4 + 1);
		int damage = (int)Math.Ceiling((float)(int)((float)num * num5) / (float)num3);
		for (int i = 0; i < num3; i++)
		{
			float num6 = (float)i / (float)num3 * ((float)Math.PI * 2f) + num2;
			float num7 = (float)((targetNPC.width > targetNPC.height) ? targetNPC.width : targetNPC.height) / 8f;
			Vector2 velocity = Vector2.UnitX.RotatedBy(num6).RotatedByRandom(0.39269909262657166) * num7;
			int num8 = Projectile.NewProjectile(projectile.GetProjectileSource_FromThis(), targetNPC.Center, velocity, 1038, damage, 0f, projectile.owner, Main.rand.NextFloat() * -20f);
			Main.projectile[num8].localNPCImmunity[targetNPC.whoAmI] = 30;
		}
	}
}
