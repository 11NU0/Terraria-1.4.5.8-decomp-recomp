using System;
using Terraria.Localization;

namespace Terraria.Chat.Commands;

[ChatCommand("Say")]
public class SayChatCommand : IChatCommand
{
	public void ProcessIncomingMessage(string text, byte clientId)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		ChatHelper.BroadcastChatMessageAs(clientId, NetworkText.FromLiteral(text), Main.player[clientId].ChatColor());
		if (Main.dedServ)
		{
			Console.WriteLine("<{0}> {1}", Main.player[clientId].name, text);
		}
	}

	public void ProcessOutgoingMessage(ChatMessage message)
	{
	}
}
