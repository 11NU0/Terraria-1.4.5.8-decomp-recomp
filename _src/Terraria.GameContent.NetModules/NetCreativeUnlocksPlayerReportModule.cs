using System.IO;
using Terraria.Net;

namespace Terraria.GameContent.NetModules;

public class NetCreativeUnlocksPlayerReportModule : NetModule
{
	public static NetPacket SerializeSacrificeRequest(int userId, int itemId, int amount)
	{
		NetPacket result = NetModule.CreatePacket<NetCreativeUnlocksPlayerReportModule>();
		result.Writer.Write((byte)userId);
		result.Writer.Write((ushort)itemId);
		result.Writer.Write((ushort)amount);
		return result;
	}

	public override bool Deserialize(BinaryReader reader, int userId)
	{
		int num = reader.ReadByte();
		int itemId = reader.ReadUInt16();
		int amount = reader.ReadUInt16();
		if (Main.dedServ)
		{
			num = userId;
			NetManager.Instance.Broadcast(SerializeSacrificeRequest(num, itemId, amount), num);
			return true;
		}
		Player player = Main.player[num];
		if (Main.LocalPlayer.team > 0 && Main.LocalPlayer.team == player.team)
		{
			Main.LocalPlayerCreativeTracker.ItemSacrifices.RegisterItemSacrifice(itemId, amount, player.name);
		}
		return true;
	}
}
