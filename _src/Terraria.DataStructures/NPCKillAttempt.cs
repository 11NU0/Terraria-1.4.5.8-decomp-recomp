namespace Terraria.DataStructures;

public struct NPCKillAttempt(NPC target)
{
	public readonly NPC npc = target;

	public readonly int netId = target.netID;

	public readonly bool active = target.active;

	public bool DidNPCDie()
	{
		return !npc.active;
	}

	public bool DidNPCDieOrTransform()
	{
		if (!DidNPCDie())
		{
			return npc.netID != netId;
		}
		return true;
	}
}
