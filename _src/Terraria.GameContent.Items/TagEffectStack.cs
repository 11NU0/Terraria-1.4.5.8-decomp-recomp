using System;
using Terraria.ID;

namespace Terraria.GameContent.Items;

public class TagEffectStack
{
	public static readonly int MaxEffects = 5;

	private TagEffectState[] _effectStates = new TagEffectState[MaxEffects];

	private int _activeEffectCount;

	private readonly Player _owner;

	public TagEffectStack(Player owner)
	{
		_owner = owner;
	}

	private int IndexOf(int tagType)
	{
		for (int i = 0; i < _activeEffectCount; i++)
		{
			if (_effectStates[i].Type == tagType)
			{
				return i;
			}
		}
		return -1;
	}

	private void LimitActiveEffects(int limit)
	{
		limit = Math.Min(limit, MaxEffects);
		while (_activeEffectCount > limit)
		{
			_effectStates[--_activeEffectCount].Deactivate();
		}
	}

	public void TrySetActiveEffect(int tagType)
	{
		int num = IndexOf(tagType);
		if (num < 0)
		{
			LimitActiveEffects(_owner.maxTagEffects - 1);
			num = _activeEffectCount;
			TagEffectState tagEffectState = _effectStates[num] ?? new TagEffectState(_owner);
			_effectStates[num] = tagEffectState;
			_activeEffectCount++;
			tagEffectState.Activate(tagType);
		}
		while (num > 0)
		{
			Utils.Swap(ref _effectStates[num], ref _effectStates[--num]);
		}
	}

	public bool IsNPCTagged(int tagType, int npcIndex)
	{
		int num = IndexOf(tagType);
		if (num < 0)
		{
			return false;
		}
		return _effectStates[num].IsNPCTagged(npcIndex);
	}

	public bool CanProcOnNPC(int tagType, int npcIndex)
	{
		int num = IndexOf(tagType);
		if (num < 0)
		{
			return false;
		}
		return _effectStates[num].CanProcOnNPC(npcIndex);
	}

	public void TryApplyTagToNPC(int tagType, NPC npc)
	{
		if (ItemID.Sets.UniqueTagEffects[tagType].CanApplyTagToNPC(npc.type))
		{
			TrySetActiveEffect(tagType);
			_effectStates[0].ApplyTagToNPC(npc);
		}
	}

	public void TryEnableProcOnNPC(int tagType, NPC npc)
	{
		int num = IndexOf(tagType);
		if (num >= 0)
		{
			_effectStates[num].EnableProcOnNPC(npc);
		}
	}

	public void RemoveAllProcs(int tagType)
	{
		int num = IndexOf(tagType);
		if (num >= 0)
		{
			_effectStates[num].RemoveAllProcs();
		}
	}

	public void ModifyHit(Projectile optionalProjectile, NPC npcHit, ref TagDamageChanges changes)
	{
		for (int i = 0; i < _activeEffectCount; i++)
		{
			_effectStates[i].ModifyHit(optionalProjectile, npcHit, ref changes);
		}
	}

	public void OnHit(Projectile optionalProjectile, NPC npcHit, int calcDamage)
	{
		for (int i = 0; i < _activeEffectCount; i++)
		{
			_effectStates[i].OnHit(optionalProjectile, npcHit, calcDamage);
		}
	}

	public void ResetNPCSlotData(int npcIndex)
	{
		for (int i = 0; i < _activeEffectCount; i++)
		{
			_effectStates[i].ResetNPCSlotData(npcIndex);
		}
	}

	public void Update()
	{
		LimitActiveEffects(_owner.maxTagEffects);
		for (int i = 0; i < _activeEffectCount; i++)
		{
			_effectStates[i].Update();
		}
	}
}
