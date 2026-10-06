using System;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Terraria.Testing.Cloning;

internal static class DeepClassCloning<T> where T : class
{
	private static readonly ClassTypeInfo Info;

	private static readonly CloneClassChildren<T> _CloneChildren;

	private static readonly RestoreClassChildren<T> _RestoreChildren;

	private static ConditionalWeakTable<Type, Func<T, DeepCloneContext, bool, T>> _polymorphicClone;

	private static ConditionalWeakTable<Type, Func<T, T, DeepCloneContext, bool, T>> _polymorphicRestore;

	private static ConditionalWeakTable<Type, Action<T, DeepCloneContext>> _polymorphicCloneChildren;

	private static ConditionalWeakTable<Type, Action<T, T, DeepCloneContext>> _polymorphicRestoreChildren;

	static DeepClassCloning()
	{
		Info = DeepCloning.FreezeClassInfo(typeof(T));
		if (Info.CloneByReference || Info.CloneByValue)
		{
			_CloneChildren = (T clone, DeepCloneContext ctx) =>
			{
			};
			_RestoreChildren = (T clone, T current, DeepCloneContext ctx) =>
			{
			};
		}
		else if (typeof(T).IsArray)
		{
			DeepCloneCodegen.GetArrayDelegates(typeof(T).GetElementType(), out _CloneChildren, out _RestoreChildren);
		}
		else
		{
			DeepCloneCodegen.GenerateClassDelegates(Info, out _CloneChildren, out _RestoreChildren);
		}
	}

	public static T CloneRef(T src, DeepCloneContext ctx)
	{
		return CloneRef(src, ctx, unique: false);
	}

	public static T CloneUniqueRef(T src, DeepCloneContext ctx)
	{
		return CloneRef(src, ctx, unique: true);
	}

	private static T CloneRef(T src, DeepCloneContext ctx, bool unique)
	{
		if (src == null)
		{
			return null;
		}
		if (Info.CloneByReference)
		{
			return src;
		}
		if (Info.NeedsPolymorphicClone && src.GetType() != typeof(T))
		{
			return GetPolymorphicDispatch(ref _polymorphicClone, "PolymorphicClone", src.GetType())(src, ctx, unique);
		}
		if (ctx.TryQuickClone(src, out var clone))
		{
			return clone;
		}
		if (!unique && ctx.TryGetClone(src, out clone))
		{
			return clone;
		}
		clone = DeepCloning.ShallowClone(src);
		if (!unique)
		{
			ctx.AddClone(src, clone);
		}
		_CloneChildren(clone, ctx);
		return clone;
	}

	public static T RestoreRef(T backup, T current, DeepCloneContext ctx)
	{
		return RestoreRef(backup, current, ctx, false);
	}

	public static T RestoreUniqueRef(T backup, T current, DeepCloneContext ctx)
	{
		return RestoreRef(backup, current, ctx, unique: true);
	}

	private static T RestoreRef(T backup, T current, DeepCloneContext ctx, bool unique = false)
	{
		if (backup == null)
		{
			return null;
		}
		if (Info.CloneByReference)
		{
			return backup;
		}
		if (Info.NeedsPolymorphicClone && backup.GetType() != typeof(T))
		{
			return GetPolymorphicDispatch(ref _polymorphicRestore, "PolymorphicRestore", backup.GetType())(backup, current, ctx, unique);
		}
		if (ctx.TryQuickRestore(backup, current, out var clone))
		{
			return clone;
		}
		if (current == null)
		{
			return CloneRef(backup, ctx, unique);
		}
		if (!unique && ctx.TryGetClone(backup, out var clone2))
		{
			return clone2;
		}
		clone = DeepCloning.ShallowClone(backup);
		if (!unique)
		{
			ctx.AddClone(backup, clone);
		}
		_RestoreChildren(clone, current, ctx);
		return clone;
	}

	public static void CloneChildren(T clone, DeepCloneContext ctx)
	{
		if (!Info.NeedsPolymorphicClone || clone.GetType() == typeof(T))
		{
			_CloneChildren(clone, ctx);
		}
		else
		{
			GetPolymorphicDispatch(ref _polymorphicCloneChildren, "PolymorphicCloneChildren", clone.GetType())(clone, ctx);
		}
	}

	public static void RestoreChildren(T clone, T current, DeepCloneContext ctx)
	{
		if (!Info.NeedsPolymorphicClone || clone.GetType() == typeof(T))
		{
			_RestoreChildren(clone, current, ctx);
		}
		else
		{
			GetPolymorphicDispatch(ref _polymorphicRestoreChildren, "PolymorphicRestoreChildren", clone.GetType())(clone, current, ctx);
		}
	}

	private static F GetPolymorphicDispatch<F>(ref ConditionalWeakTable<Type, F> table, string methodName, Type u) where F : class
	{
		if (table == null)
		{
			table = new ConditionalWeakTable<Type, F>();
		}
		if (table.TryGetValue(u, out var value))
		{
			return value;
		}
		return table.GetValue(u, (Type key) => (F)typeof(DeepClassCloning<T>).GetMethod(key.IsValueType ? (methodName + "Boxed") : methodName, (BindingFlags)(-1)).MakeGenericMethod(key).Invoke(null, null));
	}

	private static Func<T, DeepCloneContext, bool, T> PolymorphicClone<U>() where U : class, T
	{
		return (T src, DeepCloneContext ctx, bool unique) => (T)DeepClassCloning<U>.CloneRef((U)src, ctx, unique);
	}

	private static Func<T, DeepCloneContext, bool, T> PolymorphicCloneBoxed<U>() where U : struct, T
	{
		return (T src, DeepCloneContext ctx, bool unique) => (T)(object)DeepStructCloning<U>.CloneStruct((U)src, ctx);
	}

	private static Func<T, T, DeepCloneContext, bool, T> PolymorphicRestore<U>() where U : class, T
	{
		return (T backup, T current, DeepCloneContext ctx, bool unique) => (T)DeepClassCloning<U>.RestoreRef((U)backup, current as U, ctx, unique);
	}

	private static Func<T, T, DeepCloneContext, bool, T> PolymorphicRestoreBoxed<U>() where U : struct, T
	{
		return (T backup, T current, DeepCloneContext ctx, bool unique) => (T)(object)DeepStructCloning<U>.RestoreStruct((U)backup, (current is U) ? ((U)current) : default(U), ctx);
	}

	private static Action<T, DeepCloneContext> PolymorphicCloneChildren<U>() where U : class, T
	{
		return (T clone, DeepCloneContext ctx) =>
		{
			DeepClassCloning<U>._CloneChildren((U)clone, ctx);
		};
	}

	private static Action<T, T, DeepCloneContext> PolymorphicRestoreChildren<U>() where U : class, T
	{
		return (T clone, T current, DeepCloneContext ctx) =>
		{
			DeepClassCloning<U>._RestoreChildren((U)clone, current as U, ctx);
		};
	}
}
