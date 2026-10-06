namespace Terraria.Testing.ChatCommands;

public interface IDebugCommand
{
	string Name { get; }

	string Description { get; }

	string HelpText { get; }

	CommandRequirement Requirements { get; }

	bool Process(DebugMessage message);
}
