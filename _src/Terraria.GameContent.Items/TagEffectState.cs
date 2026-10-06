using System;
using Terraria.ID;

namespace Terraria.GameContent.Items;

public class TagEffectState
{
	private readonly Player _owner;

	private UniqueTagEffect _effect;

	private readonly int[] TimeLeftOnNPC = new int[Main.maxNPCs];

	private readonly int[] ProcTimeLeftOnNPC = new int[Main.maxNPCs];

	public int Type { get; private set; }

	public TagEffectState(Player owner)
	{
		_owner = owner;
	}

	public bool IsNPCTagged(int npcIndex)
	{
		return TimeLeftOnNPC[npcIndex] > 0;
	}

	public bool CanProcOnNPC(int npcIndex)
	{
		return ProcTimeLeftOnNPC[npcIndex] > 0;
	}

	public void ClearProcOnNPC(int npcIndex)
	{
		ProcTimeLeftOnNPC[npcIndex] = 0;
	}

	public void RemoveAllProcs()
	{
		Array.Clear(ProcTimeLeftOnNPC, 0, ProcTimeLeftOnNPC.Length);
	}

	public void ResetNPCSlotData(int npcIndex)
	{
		TimeLeftOnNPC[npcIndex] = 0;
		ProcTimeLeftOnNPC[npcIndex] = 0;
	}

	public void Activate(int tagType)
	{
		Type = tagType;
		_effect = ItemID.Sets.UniqueTagEffects[tagType];
		_effect.OnSetToPlayer(_owner);
	}

	public void Deactivate()
	{
		_effect.OnRemovedFromPlayer(_owner);
		Clear();
	}

	public void ApplyTagToNPC(NPC npc)
	{
		TimeLeftOnNPC[npc.whoAmI] = (int)((float)_effect.TagDuration * _owner.tagEffectDuration);
		_effect.OnTagAppliedToNPC(_owner, npc);
	}

	public void EnableProcOnNPC(NPC npc)
	{
		ProcTimeLeftOnNPC[npc.whoAmI] = (int)((float)_effect.TagDuration * _owner.tagEffectDuration);
	}

	public void Update()
	{
		for (int i = 0; i < TimeLeftOnNPC.Length; i++)
		{
			if (TimeLeftOnNPC[i] > 0)
			{
				TimeLeftOnNPC[i]--;
			}
		}
		for (int j = 0; j < ProcTimeLeftOnNPC.Length; j++)
		{
			if (ProcTimeLeftOnNPC[j] > 0)
			{
				ProcTimeLeftOnNPC[j]--;
			}
		}
	}

	private void Clear()
	{
		Array.Clear(TimeLeftOnNPC, 0, TimeLeftOnNPC.Length);
		Array.Clear(ProcTimeLeftOnNPC, 0, ProcTimeLeftOnNPC.Length);
	}

	public void ModifyHit(Projectile optionalProjectile, NPC npcHit, ref TagDamageChanges changes)
	{
		if (IsNPCTagged(npcHit.whoAmI) && _effect.CanRunHitEffects(_owner, optionalProjectile, npcHit))
		{
			_effect.ModifyTaggedHit(_owner, optionalProjectile, npcHit, ref changes);
			if (CanProcOnNPC(npcHit.whoAmI))
			{
				_effect.ModifyProcHit(_owner, optionalProjectile, npcHit, ref changes);
			}
		}
	}

	public void OnHit(Projectile optionalProjectile, NPC npcHit, int calcDamage)
	{
		if (IsNPCTagged(npcHit.whoAmI) && _effect.CanRunHitEffects(_owner, optionalProjectile, npcHit))
		{
			_effect.OnTaggedHit(_owner, optionalProjectile, npcHit, calcDamage);
			if (CanProcOnNPC(npcHit.whoAmI) && _effect.OnProcHit(_owner, optionalProjectile, npcHit, calcDamage))
			{
				ClearProcOnNPC(npcHit.whoAmI);
			}
		}
	}
}
