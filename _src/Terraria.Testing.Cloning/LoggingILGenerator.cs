using System;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;

namespace Terraria.Testing.Cloning;

internal sealed class LoggingILGenerator
{
	private readonly ILGenerator il;

	public readonly StringBuilder log;

	public LoggingILGenerator(ILGenerator il, string header)
	{
		this.il = il;
		log = new StringBuilder();
		log.AppendLine(header);
	}

	public LocalBuilder DeclareLocal(Type type)
	{
		LocalBuilder localBuilder = il.DeclareLocal(type);
		log.AppendLine(".local [" + localBuilder.LocalIndex + "] " + type.FullName);
		return localBuilder;
	}

	public void Emit(OpCode opcode)
	{
		log.AppendLine("  " + opcode);
		il.Emit(opcode);
	}

	public void Emit(OpCode opcode, MethodInfo method)
	{
		log.AppendLine(string.Concat("  ", opcode, " ", (method.DeclaringType != null) ? method.DeclaringType.Name : "dynamic", "::", method.Name));
		il.Emit(opcode, method);
	}

	public void Emit(OpCode opcode, FieldInfo field)
	{
		log.AppendLine(string.Concat("  ", opcode, " ", field.FieldType.Name, " ", field.DeclaringType.Name, "::", field.Name));
		il.Emit(opcode, field);
	}

	public void Emit(OpCode opcode, LocalBuilder local)
	{
		log.AppendLine(string.Concat("  ", opcode, " [", local.LocalIndex, "]"));
		il.Emit(opcode, local);
	}

	public void Emit(OpCode opcode, Type type)
	{
		log.AppendLine(string.Concat("  ", opcode, " ", type.FullName));
		il.Emit(opcode, type);
	}

	public Label DefineLabel()
	{
		return il.DefineLabel();
	}

	public void MarkLabel(Label label)
	{
		log.AppendLine("label_" + label.GetHashCode() + ":");
		il.MarkLabel(label);
	}

	public void Emit(OpCode opcode, Label label)
	{
		log.AppendLine(string.Concat("  ", opcode, " label_", label.GetHashCode()));
		il.Emit(opcode, label);
	}

	public string GetLog()
	{
		return log.ToString();
	}
}
