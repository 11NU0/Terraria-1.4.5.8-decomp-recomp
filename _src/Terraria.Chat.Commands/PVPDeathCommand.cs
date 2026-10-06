using Microsoft.Xna.Framework;
using Terraria.Localization;

namespace Terraria.Chat.Commands;

[ChatCommand("PVPDeath")]
public class PVPDeathCommand : IChatCommand
{
	private static readonly Color RESPONSE_COLOR = ChatColors.Death;

	public void ProcessIncomingMessage(string text, byte clientId)
	{
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		NetworkText text2 = NetworkText.FromKey("LegacyMultiplayer.24", Main.player[clientId].name, Main.player[clientId].numberOfDeathsPVP);
		if (Main.player[clientId].numberOfDeathsPVP == 1)
		{
			text2 = NetworkText.FromKey("LegacyMultiplayer.26", Main.player[clientId].name, Main.player[clientId].numberOfDeathsPVP);
		}
		ChatHelper.BroadcastChatMessage(text2, RESPONSE_COLOR);
	}

	public void ProcessOutgoingMessage(ChatMessage message)
	{
	}

	static PVPDeathCommand()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
	}
}
