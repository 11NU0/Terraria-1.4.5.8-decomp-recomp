using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Terraria.Testing.Cloning;

public static class DeepCloning
{
	public static bool DiagnosticsEnabled;

	private static readonly object _lock;

	private static readonly Dictionary<Type, ClassTypeConfig> _classConfig;

	private static readonly Dictionary<Type, StructTypeInfo> _structInfo;

	private static readonly Func<object, object> _memberwiseClone;

	static DeepCloning()
	{
		DiagnosticsEnabled = Program.LaunchParameters.ContainsKey("-diagclone");
		_lock = new object();
		_classConfig = new Dictionary<Type, ClassTypeConfig>();
		_structInfo = new Dictionary<Type, StructTypeInfo>();
		_memberwiseClone = (Func<object, object>)Delegate.CreateDelegate(typeof(Func<object, object>), typeof(object).GetMethod("MemberwiseClone", BindingFlags.Instance | BindingFlags.NonPublic));
		AddImmutableType<string>();
		AddImmutableType<Delegate>();
		AddImmutableType(typeof(IEqualityComparer<>));
		AddPrivateFieldsUniqueType(typeof(List<>));
		AddPrivateFieldsUniqueType(typeof(Dictionary<, >));
		AddPrivateFieldsUniqueType(typeof(HashSet<>));
	}

	public static T Clone<T>(T source, DeepCloneContext ctx = null) where T : class
	{
		return DeepClassCloning<T>.CloneRef(source, ctx ?? new DeepCloneContext());
	}

	public static T Restore<T>(T backup, T current, DeepCloneContext ctx = null) where T : class
	{
		return DeepClassCloning<T>.RestoreRef(backup, current, ctx ?? new DeepCloneContext());
	}

	public static T CloneStruct<T>(T source, DeepCloneContext ctx = null) where T : struct
	{
		return DeepStructCloning<T>.CloneStruct(source, ctx ?? new DeepCloneContext());
	}

	public static T RestoreStruct<T>(T backup, T current, DeepCloneContext ctx = null) where T : struct
	{
		return DeepStructCloning<T>.RestoreStruct(backup, current, ctx ?? new DeepCloneContext());
	}

	public static void CloneChildren<T>(T clone, DeepCloneContext ctx = null) where T : class
	{
		DeepClassCloning<T>.CloneChildren(clone, ctx ?? new DeepCloneContext());
	}

	public static void CloneChildren<T>(ref T clone, DeepCloneContext ctx = null) where T : struct
	{
		DeepStructCloning<T>.CloneChildren(ref clone, ctx ?? new DeepCloneContext());
	}

	public static void RestoreChildren<T>(T clone, T current, DeepCloneContext ctx = null) where T : class
	{
		DeepClassCloning<T>.RestoreChildren(clone, current, ctx ?? new DeepCloneContext());
	}

	public static void RestoreChildren<T>(ref T clone, ref T current, DeepCloneContext ctx = null) where T : struct
	{
		DeepStructCloning<T>.RestoreChildren(ref clone, ref current, ctx ?? new DeepCloneContext());
	}

	public static void AddImmutableType<T>()
	{
		AddImmutableType(typeof(T));
	}

	public static void AddImmutableType(Type type)
	{
		lock (_lock)
		{
			GetClassConfig(type).CloneByReference = true;
		}
	}

	public static void AddPrivateFieldsUniqueType<T>()
	{
		AddPrivateFieldsUniqueType(typeof(T));
	}

	public static void AddPrivateFieldsUniqueType(Type type)
	{
		lock (_lock)
		{
			GetClassConfig(type).PrivateFieldsUnique = true;
		}
	}

	internal static ClassTypeInfo FreezeClassInfo(Type t)
	{
		lock (_lock)
		{
			return GetClassConfig(t, unfrozenOnly: false).Freeze();
		}
	}

	internal static ClassTypeConfig GetClassConfig(Type t, bool unfrozenOnly = true)
	{
		lock (_lock)
		{
			if (!_classConfig.TryGetValue(t, out var value))
			{
				value = (_classConfig[t] = new ClassTypeConfig(t));
			}
			if (unfrozenOnly && value.IsFrozen)
			{
				throw new Exception(string.Concat("Cannot configure ", t, " after it has been frozen"));
			}
			return value;
		}
	}

	internal static bool NeedsFieldClone(FieldInfo field)
	{
		if (Attribute.GetCustomAttribute(field, typeof(CloneByReference)) == null)
		{
			return NeedsDynamicClone(field.FieldType);
		}
		return false;
	}

	internal static bool NeedsDynamicClone(Type type)
	{
		Type underlyingType = Nullable.GetUnderlyingType(type);
		if (underlyingType != null)
		{
			return NeedsDynamicClone(underlyingType);
		}
		if (type.IsValueType)
		{
			return !FreezeStructInfo(type).CloneByValue;
		}
		return !FreezeClassInfo(type).CloneByReference;
	}

	internal static StructTypeInfo FreezeStructInfo(Type t)
	{
		lock (_lock)
		{
			if (!_structInfo.TryGetValue(t, out var value))
			{
				value = (_structInfo[t] = new StructTypeInfo(t));
			}
			return value;
		}
	}

	private static IEnumerable<Type> DirectAncestors(Type type)
	{
		if (type.BaseType != null)
		{
			yield return type.BaseType;
		}
		Type[] interfaces = type.GetInterfaces();
		for (int i = 0; i < interfaces.Length; i++)
		{
			yield return interfaces[i];
		}
	}

	internal static IEnumerable<Type> AncestorConfigTypes(Type type)
	{
		if (type.IsGenericType && !type.IsGenericTypeDefinition)
		{
			yield return type.GetGenericTypeDefinition();
		}
		foreach (Type item in DirectAncestors(type))
		{
			yield return item.ContainsGenericParameters ? item.GetGenericTypeDefinition() : item;
		}
	}

	internal static IEnumerable<FieldInfo> GetAllInstanceFields(Type type)
	{
		BindingFlags flags = BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
		Type t = type;
		while (t != null && t != typeof(object))
		{
			FieldInfo[] fields = t.GetFields(flags);
			for (int i = 0; i < fields.Length; i++)
			{
				yield return fields[i];
			}
			t = t.BaseType;
		}
	}

	public static string DebugTypeName(Type type)
	{
		if (type.IsGenericType)
		{
			return type.Name + "<" + string.Join(", ", from t in type.GetGenericArguments()
				select DebugTypeName(t)) + ">";
		}
		return type.Name;
	}

	public static T ShallowClone<T>(T obj) where T : class
	{
		return (T)_memberwiseClone(obj);
	}
}
