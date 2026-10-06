using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using Microsoft.Xna.Framework;
using ReLogic.Utilities;
using Terraria.Chat;
using Terraria.GameContent.NetModules;
using Terraria.Localization;
using Terraria.Net;
using Terraria.UI.Chat;

namespace Terraria.Testing.ChatCommands;

public class DebugCommandProcessor
{
	private readonly Dictionary<string, IDebugCommand> _commands = new Dictionary<string, IDebugCommand>();

	private static DebugMessage _lastCachedMessage;

	private static bool _infiniteRepeatLock;

	private static string MemoCommandsPath = Path.Combine(Main.SavePath, "MemoCommands");

	public IEnumerable<IDebugCommand> Commands => _commands.Values;

	public DebugCommandProcessor()
	{
		if (DebugOptions.enableDebugCommands)
		{
			AddAttributeCommandsFromType(typeof(ToolkitDebugCommands));
		}
	}

	public void AddAttributeCommandsFromType(Type type)
	{
		MethodInfo[] methods = type.GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
		foreach (MethodInfo methodInfo in methods)
		{
			DebugCommandAttribute attribute = AttributeUtilities.GetAttribute<DebugCommandAttribute>((MethodBase)methodInfo);
			if (attribute != null)
			{
				IDebugCommand debugCommand = attribute.ToDebugCommand(methodInfo);
				AddCommand(debugCommand);
			}
		}
	}

	public void AddCommand(IDebugCommand debugCommand)
	{
		_commands[debugCommand.Name.ToLower()] = debugCommand;
	}

	public bool Process(byte playerId, string message)
	{
		return Process(new DebugMessage(playerId, message));
	}

	public static void RepeatLatestMessage()
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		if (_infiniteRepeatLock)
		{
			Main.NewTextMultiline("infinite recursion command! " + _lastCachedMessage.CommandName, force: false, Color.Red);
			return;
		}
		_infiniteRepeatLock = true;
		if (_lastCachedMessage != null)
		{
			ChatManager.DebugCommands.Process(_lastCachedMessage);
		}
		_infiniteRepeatLock = false;
	}

	public bool Process(DebugMessage message)
	{
		//IL_0149: Unknown result type (might be due to invalid IL or missing references)
		if (!DebugOptions.enableDebugCommands && !message.CommandName.Equals("toggledebugcommands"))
		{
			return false;
		}
		if (message.CommandName != "=" && !message.CommandName.StartsWith("numpad"))
		{
			_lastCachedMessage = message;
		}
		if (!_commands.TryGetValue(message.CommandName, out var value))
		{
			if (message.Author == Main.myPlayer)
			{
				return TryProcessMemo(message);
			}
			return false;
		}
		if ((value.Requirements & CommandRequirement.MultiplayerRPC) != 0 && Main.netMode == 1)
		{
			NetPacket packet = NetDebugModule.Serialize(message);
			NetManager.Instance.SendToServer(packet);
			return true;
		}
		if (!CanRunCommandLocally(message.Author, value.Requirements))
		{
			return false;
		}
		bool flag = value.Process(message);
		if (!flag && value.HelpText != null)
		{
			message.Reply(value.HelpText);
		}
		if (((DebugOptions.Shared_ReportCommandUsage || value.Name == "showdebug") & flag) && Main.netMode != 0)
		{
			string arg = ((message.Author == byte.MaxValue) ? "server" : Main.player[message.Author].name);
			string text = $"{arg} debugged: /{message.CommandName} {message.Arguments}";
			if (Main.netMode != 1)
			{
				ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral(text), new Color(250, 250, 0));
			}
			else
			{
				ChatHelper.SendChatMessageFromClient(new ChatMessage(text));
			}
		}
		return true;
	}

	public bool ExecuteSubMessage(DebugMessage baseMessage, string newMessage)
	{
		return Process(baseMessage.CreateSubMessage(newMessage));
	}

	private static bool CanRunCommandLocally(int playerId, CommandRequirement requirements)
	{
		if ((Main.netMode != 0 || (requirements & CommandRequirement.SinglePlayer) == 0) && (Main.netMode != 1 || (requirements & CommandRequirement.MultiplayerClient) == 0) && (Main.netMode != 2 || (requirements & CommandRequirement.LocalServer) == 0 || playerId != 255))
		{
			if (Main.netMode == 2 && (requirements & CommandRequirement.MultiplayerRPC) != 0)
			{
				return playerId < 255;
			}
			return false;
		}
		return true;
	}

	private bool TryProcessMemo(DebugMessage message)
	{
		string path = Path.Combine(MemoCommandsPath, message.CommandName.ToLower() + ".txt");
		if (!File.Exists(path))
		{
			return false;
		}
		try
		{
			string[] array = message.Arguments.Split(' ');
			string[] array2 = File.ReadAllLines(path);
			foreach (string text in array2)
			{
				if (!string.IsNullOrWhiteSpace(text))
				{
					object[] args = array;
					ExecuteMemo(message, string.Format(text, args));
				}
			}
		}
		catch (FormatException)
		{
			message.ReplyError("Memo formatting error. Perhaps you forgot to pass arguments?");
		}
		return true;
	}

	private void ExecuteMemo(DebugMessage message, string cmd)
	{
		if (!ExecuteSubMessage(message, cmd))
		{
			ChatMessage message2 = ChatManager.Commands.CreateOutgoingMessage(cmd);
			if (Main.netMode == 1)
			{
				ChatHelper.SendChatMessageFromClient(message2);
			}
			else
			{
				ChatManager.Commands.ProcessIncomingMessage(message2, message.Author);
			}
		}
	}

	public static void OpenMemo(string name)
	{
		Utils.TryCreatingDirectory(MemoCommandsPath);
		string text = Path.Combine(MemoCommandsPath, name.ToLower() + ".txt");
		if (!File.Exists(text))
		{
			File.WriteAllBytes(text, new byte[0]);
		}
		System.Diagnostics.Process.Start(new ProcessStartInfo(text)
		{
			UseShellExecute = true
		});
	}
}
