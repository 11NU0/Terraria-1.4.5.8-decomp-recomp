using Microsoft.Xna.Framework;
using Terraria.Localization;

namespace Terraria.Chat.Commands;

[ChatCommand("Roll")]
public class RollCommand : IChatCommand
{
	private static readonly Color RESPONSE_COLOR = ChatColors.ServerMessage;

	public void ProcessIncomingMessage(string text, byte clientId)
	{
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		int num = Main.rand.Next(1, 101);
		ChatHelper.BroadcastChatMessage(NetworkText.FromFormattable("*{0} {1} {2}", Main.player[clientId].name, Lang.mp[9].ToNetworkText(), num), RESPONSE_COLOR);
	}

	public void ProcessOutgoingMessage(ChatMessage message)
	{
	}

	static RollCommand()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
	}
}
