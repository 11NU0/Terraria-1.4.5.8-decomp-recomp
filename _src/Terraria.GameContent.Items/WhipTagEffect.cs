using Terraria.ID;

namespace Terraria.GameContent.Items;

public class WhipTagEffect : UniqueTagEffect
{
	public int PlayerBuffId;

	public int PlayerBuffTime;

	public bool PlayerBuffAppliedManually;

	public int CritChance;

	public int TagDamage;

	private const int generalWhipMarkDuration = 240;

	public WhipTagEffect()
	{
		TagDuration = 240;
	}

	public override bool CanApplyTagToNPC(int npcType)
	{
		if (!NPCID.Sets.DebuffImmunitySets.TryGetValue(npcType, out var value))
		{
			return true;
		}
		if (value != null)
		{
			return !value.ImmuneToWhips;
		}
		return true;
	}

	public override void OnRemovedFromPlayer(Player player)
	{
		if (player == Main.LocalPlayer)
		{
			player.ClearBuff(PlayerBuffId);
		}
	}

	public override void OnTagAppliedToNPC(Player player, NPC npc)
	{
		if (player == Main.LocalPlayer)
		{
			AddTheBuff(player);
		}
	}

	protected void AddTheBuff(Player player)
	{
		if (!PlayerBuffAppliedManually && PlayerBuffId > 0)
		{
			int time = (int)((float)PlayerBuffTime * player.tagEffectDuration);
			player.AddBuff(PlayerBuffId, time);
		}
	}

	public override void ModifyTaggedHit(Player owner, Projectile optionalProjectile, NPC npcHit, ref TagDamageChanges changes)
	{
		changes.AddProjectileTagDamage = true;
		changes.AddedBaseDamage += TagDamage;
		if (changes.HighestAddedBaseDamage < TagDamage)
		{
			changes.HighestAddedBaseDamage = TagDamage;
		}
		if (Main.rand.Next(100) < CritChance)
		{
			changes.Crit = true;
		}
	}

	public override bool CanRunHitEffects(Player owner, Projectile optionalProjectile, NPC npcHit)
	{
		if (optionalProjectile == null || !optionalProjectile.OwnedBySomeone)
		{
			return false;
		}
		if (!optionalProjectile.minion && !ProjectileID.Sets.MinionShot[optionalProjectile.type] && !optionalProjectile.sentry)
		{
			return ProjectileID.Sets.SentryShot[optionalProjectile.type];
		}
		return true;
	}
}
