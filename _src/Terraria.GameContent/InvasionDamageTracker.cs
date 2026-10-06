using System.Collections.Generic;
using Terraria.Localization;

namespace Terraria.GameContent;

public class InvasionDamageTracker : NPCDamageTracker
{
	private static Dictionary<int, string> VanillaInvasionNameKeys = new Dictionary<int, string>
	{
		{ 1, "Bestiary_Invasions.Goblins" },
		{ 2, "Bestiary_Invasions.FrostLegion" },
		{ 3, "Bestiary_Invasions.Pirates" },
		{ 4, "Bestiary_Invasions.Martian" },
		{ -2, "Bestiary_Invasions.PumpkinMoon" },
		{ -1, "Bestiary_Invasions.FrostMoon" }
	};

	private readonly int _invasionGroup;

	private readonly LocalizedText _name;

	public override LocalizedText Name => _name;

	public override LocalizedText KillTimeMessage => null;

	public InvasionDamageTracker(int invasionGroup, LocalizedText name = null)
	{
		_invasionGroup = invasionGroup;
		_name = ((name != null) ? name : Language.GetText(VanillaInvasionNameKeys[invasionGroup]));
	}

	protected override bool IncludeDamageFor(NPC npc)
	{
		return NPC.GetNPCInvasionGroup(npc.type) == _invasionGroup;
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
		if (_invasionGroup == -2)
		{
			return Main.pumpkinMoon;
		}
		if (_invasionGroup == -1)
		{
			return Main.snowMoon;
		}
		return Main.invasionType == _invasionGroup;
	}
}
