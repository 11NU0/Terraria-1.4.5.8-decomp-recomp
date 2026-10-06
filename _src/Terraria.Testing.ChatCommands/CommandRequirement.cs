using System;

namespace Terraria.Testing.ChatCommands;

[Flags]
public enum CommandRequirement
{
	SinglePlayer = 1,
	MultiplayerClient = 2,
	MultiplayerRPC = 4,
	LocalServer = 8,
	ClientAuthority = SinglePlayer | MultiplayerRPC,
	AnyAuthority = ClientAuthority | LocalServer,
	Client = SinglePlayer | MultiplayerClient,
	Local = Client | LocalServer,
	All = AnyAuthority | MultiplayerClient
}
