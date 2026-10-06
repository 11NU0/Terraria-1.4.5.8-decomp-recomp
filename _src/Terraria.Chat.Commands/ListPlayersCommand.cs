using System.Linq;
using Microsoft.Xna.Framework;
using Terraria.Localization;

namespace Terraria.Chat.Commands;

[ChatCommand("Playing")]
public class ListPlayersCommand : IChatCommand
{
	private static readonly Color RESPONSE_COLOR = ChatColors.ServerMessage;

	public void ProcessIncomingMessage(string text, byte clientId)
	{
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		ChatHelper.SendChatMessageToClient(NetworkText.FromLiteral(string.Join(", ", from player in Main.player
			where player.active
			select player.name)), RESPONSE_COLOR, clientId);
	}

	public void ProcessOutgoingMessage(ChatMessage message)
	{
	}

	static ListPlayersCommand()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
	}
}
