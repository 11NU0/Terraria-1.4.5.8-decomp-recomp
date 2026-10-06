using System.IO;
using Terraria.Net;
using Terraria.Testing.ChatCommands;
using Terraria.UI.Chat;

namespace Terraria.GameContent.NetModules;

public class NetDebugModule : NetModule
{
	public static NetPacket Serialize(DebugMessage message)
	{
		NetPacket result = NetModule.CreatePacket<NetDebugModule>();
		message.Serialize(result.Writer);
		return result;
	}

	public override bool Deserialize(BinaryReader reader, int senderPlayerId)
	{
		DebugMessage message = DebugMessage.Deserialize((byte)senderPlayerId, reader);
		ChatManager.DebugCommands.Process(message);
		return true;
	}
}
