using System.Linq;
using Microsoft.Xna.Framework;
using Terraria.Localization;

namespace Terraria.Chat.Commands;

[ChatCommand("AllDeath")]
public class AllDeathCommand : IChatCommand
{
	private static readonly Color RESPONSE_COLOR = ChatColors.Death;

	public void ProcessIncomingMessage(string text, byte clientId)
	{
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		foreach (Player item in from x in Main.player
			where x?.active ?? false
			orderby x.numberOfDeathsPVE descending
			select x)
		{
			NetworkText text2 = NetworkText.FromKey("LegacyMultiplayer.23", item.name, item.numberOfDeathsPVE);
			if (item.numberOfDeathsPVE == 1)
			{
				text2 = NetworkText.FromKey("LegacyMultiplayer.25", item.name, item.numberOfDeathsPVE);
			}
			ChatHelper.BroadcastChatMessage(text2, RESPONSE_COLOR);
		}
	}

	public void ProcessOutgoingMessage(ChatMessage message)
	{
	}

	static AllDeathCommand()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
	}
}
