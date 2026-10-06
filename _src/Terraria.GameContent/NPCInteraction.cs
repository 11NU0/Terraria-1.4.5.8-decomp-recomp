using Microsoft.Xna.Framework;

namespace Terraria.GameContent;

public abstract class NPCInteraction
{
	public virtual bool ShowExclamation => false;

	public Player LocalPlayer => Main.LocalPlayer;

	public NPC TalkNPC => Main.npc[LocalPlayer.talkNPC];

	public int TalkNPCType
	{
		get
		{
			if (LocalPlayer.talkNPC == -1)
			{
				return 0;
			}
			return TalkNPC.type;
		}
	}

	public abstract bool Condition();

	public abstract string GetText();

	public abstract void Interact();

	public virtual bool TryAddCoins(ref Color chatColor, out int coinValue)
	{
		coinValue = 0;
		return false;
	}
}
