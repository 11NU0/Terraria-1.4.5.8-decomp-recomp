using System;
using System.Linq;
using System.Reflection;

namespace Terraria.Testing.Cloning;

internal class ClassTypeInfo
{
	public readonly Type Type;

	public readonly bool CloneByReference;

	public readonly bool PrivateFieldsUnique;

	public readonly bool NeedsPolymorphicClone;

	public readonly bool CloneByValue;

	public ClassTypeInfo(Type type, ClassTypeConfig config, ref ClassTypeInfo _out)
	{
		Type = config.Type;
		ClassTypeInfo[] source = DeepCloning.AncestorConfigTypes(type).Select(DeepCloning.FreezeClassInfo).ToArray();
		CloneByReference = Attribute.IsDefined(type, typeof(CloneByReference)) || config.CloneByReference || source.Any((ClassTypeInfo c) => c.CloneByReference);
		if (!CloneByReference)
		{
			PrivateFieldsUnique = config.PrivateFieldsUnique || source.Any((ClassTypeInfo c) => c.PrivateFieldsUnique);
			NeedsPolymorphicClone = ((!type.IsArray) ? (!type.IsSealed) : (!type.GetElementType().IsValueType && !type.GetElementType().IsSealed));
		}
		_out = this;
		if (!CloneByReference && !type.IsGenericTypeDefinition)
		{
			CloneByValue = (type.IsArray ? (!DeepCloning.NeedsDynamicClone(type.GetElementType())) : (!DeepCloning.GetAllInstanceFields(type).Any(DeepCloning.NeedsFieldClone)));
		}
	}

	public MethodInfo GetHelperMethod(string name)
	{
		return typeof(DeepClassCloning<>).MakeGenericType(Type).GetMethod(name, BindingFlags.Static | BindingFlags.Public);
	}
}
