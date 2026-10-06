namespace Terraria.Chat.Commands;

public interface ICommandAliasProvider
{
	void PrepareAliases(ChatCommandProcessor commandProcessor);
}
