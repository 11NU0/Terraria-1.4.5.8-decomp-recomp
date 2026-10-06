using System;
using System.Reflection;

namespace Terraria.Testing.ChatCommands;

[AttributeUsage(AttributeTargets.Method, Inherited = false)]
public sealed class DebugCommandAttribute : Attribute
{
	private class InternalDebugCommand : IDebugCommand
	{
		private delegate bool ProcessMethod(DebugMessage message);

		private readonly ProcessMethod _processMethod;

		public string Name { get; private set; }

		public string Description { get; private set; }

		public string HelpText { get; private set; }

		public CommandRequirement Requirements { get; private set; }

		public InternalDebugCommand(DebugCommandAttribute attribute, MethodInfo method)
		{
			Name = attribute.Name;
			Description = attribute.Description;
			HelpText = attribute.HelpText;
			Requirements = attribute.Requirements;
			_processMethod = (ProcessMethod)Delegate.CreateDelegate(typeof(ProcessMethod), method);
		}

		public bool Process(DebugMessage message)
		{
			return _processMethod(message);
		}
	}

	public readonly string Name;

	public readonly string Description;

	public readonly CommandRequirement Requirements;

	public string HelpText;

	public DebugCommandAttribute(string name, string description, CommandRequirement requirements)
	{
		Name = name;
		Description = description;
		Requirements = requirements;
	}

	public IDebugCommand ToDebugCommand(MethodInfo method)
	{
		return new InternalDebugCommand(this, method);
	}
}
