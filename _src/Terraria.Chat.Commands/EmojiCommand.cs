using System;
using System.Collections.Generic;
using Terraria.GameContent.UI;
using Terraria.Localization;

namespace Terraria.Chat.Commands;

[ChatCommand("Emoji")]
public class EmojiCommand : IChatCommand, ICommandAliasProvider
{
	public const int PlayerEmojiDuration = 360;

	private readonly Dictionary<LocalizedText, int> _byName = new Dictionary<LocalizedText, int>();

	public EmojiCommand()
	{
		Initialize();
	}

	public void Initialize()
	{
		_byName.Clear();
		for (int i = 0; i < EmoteID.Count; i++)
		{
			LocalizedText emojiName = Lang.GetEmojiName(i);
			if (emojiName != LocalizedText.Empty)
			{
				_byName[emojiName] = i;
			}
		}
	}

	public void PrepareAliases(ChatCommandProcessor commandProcessor)
	{
		for (int i = 0; i < EmoteID.Count; i++)
		{
			string name = EmoteID.Search.GetName(i);
			commandProcessor.AddAlias(Language.GetText("EmojiCommand." + name), () => string.Format("{0} {1}", Language.GetTextValue("ChatCommand.Emoji_1"), Language.GetTextValue("EmojiName." + name)));
		}
	}

	public void ProcessIncomingMessage(string text, byte clientId)
	{
	}

	public void ProcessOutgoingMessage(ChatMessage message)
	{
		if (Main.netMode != 2 && Main.LocalPlayer.dead)
		{
			message.Consume();
			return;
		}
		int result = -1;
		if (int.TryParse(message.Text, out result))
		{
			if (result < 0 || result >= EmoteID.Count)
			{
				return;
			}
		}
		else
		{
			result = -1;
		}
		if (result == -1)
		{
			foreach (LocalizedText key in _byName.Keys)
			{
				if (key.EqualsCommand(message.Text))
				{
					result = _byName[key];
					break;
				}
			}
		}
		if (result != -1)
		{
			if (Main.netMode == 0)
			{
				EmoteBubble.NewBubble(result, new WorldUIAnchor((Entity)Main.LocalPlayer), 360);
				EmoteBubble.CheckForNPCsToReactToEmoteBubble(result, Main.LocalPlayer);
			}
			else
			{
				NetMessage.SendData(120, -1, -1, null, Main.myPlayer, result);
			}
		}
		message.Consume();
	}

	public void PrintWarning(string text)
	{
		throw new Exception("This needs localized text!");
	}
}
