using System.IO;
using Microsoft.Xna.Framework;
using Terraria.Net;

namespace Terraria.GameContent.NetModules;

public class NetPingModule : NetModule
{
	public static NetPacket Serialize(Vector2 position)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		NetPacket result = NetModule.CreatePacket<NetPingModule>();
		result.Writer.WriteVector2(position);
		return result;
	}

	public override bool Deserialize(BinaryReader reader, int userId)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		Vector2 position = reader.ReadVector2();
		if (Main.dedServ)
		{
			NetManager.Instance.Broadcast(Serialize(position), userId);
		}
		else
		{
			Main.Pings.Add(position);
		}
		return true;
	}
}
