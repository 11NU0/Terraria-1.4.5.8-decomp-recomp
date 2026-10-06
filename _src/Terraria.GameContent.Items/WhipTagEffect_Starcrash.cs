using System;
using Microsoft.Xna.Framework;

namespace Terraria.GameContent.Items;

public class WhipTagEffect_Starcrash : WhipTagEffect
{
	public override bool OnProcHit(Player owner, Projectile optionalProjectile, NPC npcHit, int calcDamage)
	{
		SpawnMeteorWhipMeteorOn(optionalProjectile, npcHit, calcDamage);
		return true;
	}

	private void SpawnMeteorWhipMeteorOn(Projectile projectile, NPC targetNPC, int calcDamage)
	{
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		int num = 200;
		int num2 = 600;
		int damage = (int)((float)calcDamage * 1.33f);
		Vector2 val = new Vector2((float)(-num + Main.rand.Next(num * 2)), (float)(-num2));
		Vector2 val2 = targetNPC.Center + val;
		Vector2 val3 = val.SafeNormalize(Vector2.Zero) * -12f;
		int num3 = 8;
		int num4 = 35;
		val2 = targetNPC.Center + new Vector2(0f, (float)(-num3 * num4)).RotatedBy(Main.rand.NextFloatDirection() * ((float)Math.PI * 2f) * 0.125f);
		val3 = targetNPC.DirectionFrom(val2) * (float)num3;
		Projectile.NewProjectile(projectile.GetProjectileSource_FromThis(), val2, val3, 1037, damage, projectile.knockBack, projectile.owner, Main.rand.Next(3), targetNPC.position.Y);
	}
}
