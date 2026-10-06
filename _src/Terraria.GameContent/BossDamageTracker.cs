using Terraria.Localization;

namespace Terraria.GameContent;

public class BossDamageTracker : NPCDamageTracker
{
	private readonly int _type;

	private readonly CustomDefinition _overrides;

	private bool _killed;

	public override LocalizedText Name
	{
		get
		{
			if (_overrides == null || _overrides.Name == null)
			{
				return Lang.GetNPCName(_type);
			}
			return _overrides.Name;
		}
	}

	public override LocalizedText KillTimeMessage => Language.GetText(_killed ? "BossDamageCommand.KillTime" : "BossDamageCommand.KillTimeEscaped");

	public BossDamageTracker(int type, CustomDefinition definition)
	{
		if (definition != null && definition.NPCTypes != null)
		{
			type = definition.NPCTypes[0];
		}
		_type = type;
		_overrides = definition;
	}

	protected override bool IncludeDamageFor(NPC npc)
	{
		if (NPCDamageTracker.BossTypeForMob[npc.type] == _type)
		{
			return true;
		}
		if (_overrides == null || _overrides.NPCTypes == null)
		{
			return npc.type == _type;
		}
		return _overrides.NPCTypes.Contains(npc.type);
	}

	protected override void CheckActive()
	{
		if (!IsActive())
		{
			Stop();
		}
	}

	private bool IsActive()
	{
		if (_overrides != null && _overrides.NPCTypes != null)
		{
			foreach (int nPCType in _overrides.NPCTypes)
			{
				if (NPC.npcsFoundForCheckActive[nPCType])
				{
					return true;
				}
			}
			return false;
		}
		return NPC.npcsFoundForCheckActive[_type];
	}

	protected override void OnBossKilled(NPC npc)
	{
		_killed = true;
	}
}
