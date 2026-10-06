using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria.Localization;

namespace Terraria.Chat.Commands;

[ChatCommand("Help")]
public class HelpCommand : IChatCommand
{
	private static readonly Color RESPONSE_COLOR = ChatColors.ServerMessage;

	public void ProcessIncomingMessage(string text, byte clientId)
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		ChatHelper.SendChatMessageToClient(ComposeMessage(GetCommandAliasesByID()), RESPONSE_COLOR, clientId);
	}

	private static Dictionary<string, List<LocalizedText>> GetCommandAliasesByID()
	{
		LocalizedText[] array = Language.FindAll(Lang.CreateDialogFilter("ChatCommandDescription."));
		Dictionary<string, List<LocalizedText>> dictionary = new Dictionary<string, List<LocalizedText>>();
		LocalizedText[] array2 = array;
		foreach (LocalizedText localizedText in array2)
		{
			string key = localizedText.Key;
			key = key.Replace("ChatCommandDescription.", "");
			int num = key.IndexOf('_');
			if (num != -1)
			{
				key = key.Substring(0, num);
			}
			if (!dictionary.TryGetValue(key, out var value))
			{
				value = (dictionary[key] = new List<LocalizedText>());
			}
			value.Add(localizedText);
		}
		return dictionary;
	}

	private static NetworkText ComposeMessage(Dictionary<string, List<LocalizedText>> aliases)
	{
		string text = "";
		for (int i = 0; i < aliases.Count; i++)
		{
			text = text + "{" + i + "}\n";
		}
		List<NetworkText> list = new List<NetworkText>();
		foreach (KeyValuePair<string, List<LocalizedText>> alias in aliases)
		{
			list.Add(Language.GetText("ChatCommandDescription." + alias.Key).ToNetworkText());
		}
		string text2 = text;
		object[] substitutions = list.ToArray();
		return NetworkText.FromFormattable(text2, substitutions);
	}

	public void ProcessOutgoingMessage(ChatMessage message)
	{
	}

	static HelpCommand()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
	}
}
