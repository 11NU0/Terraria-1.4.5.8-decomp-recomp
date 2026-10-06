using Microsoft.Xna.Framework;
using Terraria.GameContent;

namespace Terraria.Chat.Commands;

[ChatCommand("BossDamage")]
public class BossDamageCommand : IChatCommand
{
	private static readonly Color RESPONSE_COLOR = ChatColors.World;

	public void ProcessIncomingMessage(string text, byte clientId)
	{
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		foreach (NPCDamageTracker item in NPCDamageTracker.RecentAttempts())
		{
			for (int i = 0; i < 255; i++)
			{
				if (Main.player[i].active)
				{
					ChatHelper.SendChatMessageToClient(item.GetReport(Main.player[i]), RESPONSE_COLOR, i);
				}
			}
		}
	}

	public void ProcessOutgoingMessage(ChatMessage message)
	{
	}

	static BossDamageCommand()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
	}
}
