using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria.GameContent.NetModules;
using Terraria.GameContent.UI.Chat;
using Terraria.Localization;
using Terraria.Net;

namespace Terraria.Chat;

public static class ChatHelper
{
	private static List<Tuple<string, Color>> _cachedMessages = new List<Tuple<string, Color>>();

	public static void DisplayMessageOnClient(NetworkText text, Color color, int playerId)
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		if (Main.dedServ)
		{
			NetPacket packet = NetTextModule.SerializeServerMessage(text, color, byte.MaxValue);
			NetManager.Instance.SendToClient(packet, playerId);
		}
		else
		{
			DisplayMessage(text, color, byte.MaxValue);
		}
	}

	public static void SendChatMessageToClient(NetworkText text, Color color, int playerId)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		SendChatMessageToClientAs(byte.MaxValue, text, color, playerId);
	}

	public static void SendChatMessageToClientAs(byte messageAuthor, NetworkText text, Color color, int playerId)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		if (Main.dedServ)
		{
			NetPacket packet = NetTextModule.SerializeServerMessage(text, color, messageAuthor);
			NetManager.Instance.SendToClient(packet, playerId);
		}
		if (playerId == Main.myPlayer)
		{
			DisplayMessage(text, color, messageAuthor);
		}
	}

	public static void BroadcastChatMessage(NetworkText text, Color color, int excludedPlayer = -1)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		BroadcastChatMessageAs(byte.MaxValue, text, color, excludedPlayer);
	}

	public static void BroadcastChatMessageAs(byte messageAuthor, NetworkText text, Color color, int excludedPlayer = -1)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		if (Main.netMode != 1)
		{
			if (Main.dedServ)
			{
				NetPacket packet = NetTextModule.SerializeServerMessage(text, color, messageAuthor);
				NetManager.Instance.Broadcast(packet, OnlySendToPlayersWhoAreLoggedIn, excludedPlayer);
			}
			else if (excludedPlayer != Main.myPlayer)
			{
				DisplayMessage(text, color, messageAuthor);
			}
		}
	}

	public static bool OnlySendToPlayersWhoAreLoggedIn(int clientIndex)
	{
		return Netplay.Clients[clientIndex].State == 10;
	}

	public static void SendChatMessageFromClient(ChatMessage message)
	{
		if (!message.IsConsumed)
		{
			NetPacket packet = NetTextModule.SerializeClientMessage(message);
			NetManager.Instance.SendToServer(packet);
		}
	}

	public static void DisplayMessage(NetworkText text, Color color, byte messageAuthor)
	{
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		string text2 = text.ToString();
		if (messageAuthor < byte.MaxValue)
		{
			Main.player[messageAuthor].chatOverhead.NewMessage(text2, Main.PlayerOverheadChatMessageDisplayTime);
			Main.player[messageAuthor].chatOverhead.color = color;
			text2 = NameTagHandler.GenerateTag(Main.player[messageAuthor].name) + " " + text2;
		}
		if (ShouldCacheMessage())
		{
			CacheMessage(text2, color);
		}
		else
		{
			Main.NewTextMultiline(text2, force: false, color);
		}
	}

	private static void CacheMessage(string message, Color color)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		_cachedMessages.Add(new Tuple<string, Color>(message, color));
	}

	public static void ShowCachedMessages()
	{
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		lock (_cachedMessages)
		{
			foreach (Tuple<string, Color> cachedMessage in _cachedMessages)
			{
				Main.NewTextMultiline(cachedMessage.Item1, force: false, cachedMessage.Item2);
			}
		}
	}

	public static void ClearDelayedMessagesCache()
	{
		_cachedMessages.Clear();
	}

	private static bool ShouldCacheMessage()
	{
		if (Main.netMode == 1)
		{
			return Main.gameMenu;
		}
		return false;
	}
}
