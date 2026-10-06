using System;
using System.Collections.Generic;
using ReLogic.Utilities;
using Terraria.Chat.Commands;
using Terraria.Localization;
using Terraria.Testing;

namespace Terraria.Chat;

public class ChatCommandProcessor : IChatProcessor
{
	private readonly Dictionary<LocalizedText, ChatCommandId> _localizedCommands = new Dictionary<LocalizedText, ChatCommandId>();

	private readonly Dictionary<ChatCommandId, IChatCommand> _commands = new Dictionary<ChatCommandId, IChatCommand>();

	private Dictionary<LocalizedText, Func<string>> _aliases = new Dictionary<LocalizedText, Func<string>>();

	private IChatCommand _defaultCommand;

	public ChatCommandProcessor AddCommand<T>() where T : IChatCommand, new()
	{
		ChatCommandAttribute cacheableAttribute = AttributeUtilities.GetCacheableAttribute<T, ChatCommandAttribute>();
		Invariant.Assert(cacheableAttribute != null, "AddCommand<T>(): {0} must have a ChatCommand attribute.", typeof(T));
		string commandKey = "ChatCommand." + cacheableAttribute.Name;
		ChatCommandId chatCommandId = ChatCommandId.FromType<T>();
		_commands[chatCommandId] = new T();
		if (Language.Exists(commandKey))
		{
			_localizedCommands.Add(Language.GetText(commandKey), chatCommandId);
		}
		else
		{
			commandKey += "_";
			LocalizedText[] array = Language.FindAll((string text2, LocalizedText text) => text2.StartsWith(commandKey));
			foreach (LocalizedText key in array)
			{
				_localizedCommands.Add(key, chatCommandId);
			}
		}
		return this;
	}

	public void AddAlias(LocalizedText alias, Func<string> result)
	{
		_aliases[alias] = result;
	}

	public void PrepareAliases()
	{
		foreach (IChatCommand value in _commands.Values)
		{
			if (value is ICommandAliasProvider)
			{
				((ICommandAliasProvider)value).PrepareAliases(this);
			}
		}
	}

	public ChatCommandProcessor AddDefaultCommand<T>() where T : IChatCommand, new()
	{
		AddCommand<T>();
		ChatCommandId key = ChatCommandId.FromType<T>();
		_defaultCommand = _commands[key];
		return this;
	}

	private static bool ParseCommandPrefix<T>(string text, Dictionary<LocalizedText, T> commands, out string remainder, out T value)
	{
		foreach (KeyValuePair<LocalizedText, T> command in commands)
		{
			if (command.Key.ParseCommandPrefix(text, out remainder))
			{
				value = command.Value;
				return true;
			}
		}
		remainder = "";
		value = default;
		return false;
	}

	public ChatMessage CreateOutgoingMessage(string text)
	{
		ChatMessage chatMessage = new ChatMessage(text);
		if (ParseCommandPrefix(chatMessage.Text, _localizedCommands, out var remainder, out var value))
		{
			chatMessage.Text = remainder;
			chatMessage.SetCommand(value);
			_commands[value].ProcessOutgoingMessage(chatMessage);
			return chatMessage;
		}
		if (ParseCommandPrefix(chatMessage.Text, _aliases, out remainder, out var value2))
		{
			return CreateOutgoingMessage(value2());
		}
		return chatMessage;
	}

	public void ProcessIncomingMessage(ChatMessage message, int clientId)
	{
		if (_commands.TryGetValue(message.CommandId, out var value))
		{
			value.ProcessIncomingMessage(message.Text, (byte)clientId);
			message.Consume();
		}
		else if (_defaultCommand != null)
		{
			_defaultCommand.ProcessIncomingMessage(message.Text, (byte)clientId);
			message.Consume();
		}
	}
}
