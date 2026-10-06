using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace Terraria.Testing.Cloning;

internal static class DeepCloneCodegen
{
	private static readonly MethodInfo _memberwiseCloneMethod = typeof(object).GetMethod("MemberwiseClone", BindingFlags.Instance | BindingFlags.NonPublic);

	internal static void GenerateClassDelegates<T>(ClassTypeInfo selfInfo, out CloneClassChildren<T> cloneChildren, out RestoreClassChildren<T> restoreChildren) where T : class
	{
		Type typeFromHandle = typeof(T);
		DynamicMethod dynamicMethod = BuildCloneChildrenDynamicMethod(typeFromHandle, new Type[2]
		{
			typeFromHandle,
			typeof(DeepCloneContext)
		}, selfInfo.PrivateFieldsUnique);
		cloneChildren = (CloneClassChildren<T>)dynamicMethod.CreateDelegate(typeof(CloneClassChildren<T>));
		DynamicMethod dynamicMethod2 = BuildRestoreChildrenDynamicMethod(typeFromHandle, new Type[3]
		{
			typeFromHandle,
			typeFromHandle,
			typeof(DeepCloneContext)
		}, selfInfo.PrivateFieldsUnique);
		restoreChildren = (RestoreClassChildren<T>)dynamicMethod2.CreateDelegate(typeof(RestoreClassChildren<T>));
	}

	internal static void GenerateStructDelegates<T>(out CloneStructChildren<T> cloneChildren, out RestoreStructChildren<T> restoreChildren) where T : struct
	{
		Type typeFromHandle = typeof(T);
		Type type = typeFromHandle.MakeByRefType();
		DynamicMethod dynamicMethod = BuildCloneChildrenDynamicMethod(typeFromHandle, new Type[2]
		{
			type,
			typeof(DeepCloneContext)
		}, uniqueRef: false);
		cloneChildren = (CloneStructChildren<T>)dynamicMethod.CreateDelegate(typeof(CloneStructChildren<T>));
		DynamicMethod dynamicMethod2 = BuildRestoreChildrenDynamicMethod(typeFromHandle, new Type[3]
		{
			type,
			type,
			typeof(DeepCloneContext)
		}, uniqueRef: false);
		restoreChildren = (RestoreStructChildren<T>)dynamicMethod2.CreateDelegate(typeof(RestoreStructChildren<T>));
	}

	internal static void GetArrayDelegates<T>(Type elemType, out CloneClassChildren<T> cloneChildren, out RestoreClassChildren<T> restoreChildren) where T : class
	{
		if (typeof(T).GetArrayRank() > 2)
		{
			throw new Exception("Only 1d and 2d arrays are supported due to runtime/codegen limitations");
		}
		string text = ((typeof(T).GetArrayRank() == 2) ? "2D" : "");
		string name = (elemType.IsValueType ? "CloneStructElements" : "CloneClassElements") + text;
		string name2 = (elemType.IsValueType ? "RestoreStructElements" : "RestoreClassElements") + text;
		cloneChildren = (CloneClassChildren<T>)Delegate.CreateDelegate(typeof(CloneClassChildren<T>), typeof(DeepCloneCodegen).GetMethod(name, BindingFlags.Static | BindingFlags.NonPublic).MakeGenericMethod(elemType));
		restoreChildren = (RestoreClassChildren<T>)Delegate.CreateDelegate(typeof(RestoreClassChildren<T>), typeof(DeepCloneCodegen).GetMethod(name2, BindingFlags.Static | BindingFlags.NonPublic).MakeGenericMethod(elemType));
	}

	private static DynamicMethod BuildCloneChildrenDynamicMethod(Type type, Type[] paramTypes, bool uniqueRef)
	{
		string text = "CloneChildren_" + type.Name;
		DynamicMethod dynamicMethod = new DynamicMethod(text, null, paramTypes, type, skipVisibility: true);
		LoggingILGenerator loggingILGenerator = new LoggingILGenerator(dynamicMethod.GetILGenerator(), text);
		EmitCloneChildrenForType(loggingILGenerator, type, uniqueRef);
		loggingILGenerator.Emit(OpCodes.Ret);
		WriteILLog(type, "CloneChildren", loggingILGenerator.GetLog());
		return dynamicMethod;
	}

	private static void EmitCloneChildrenForType(LoggingILGenerator il, Type type, bool uniqueRef, List<FieldInfo> path = null)
	{
		path = path ?? new List<FieldInfo>();
		foreach (FieldInfo allInstanceField in DeepCloning.GetAllInstanceFields(type))
		{
			if (!DeepCloning.NeedsFieldClone(allInstanceField))
			{
				continue;
			}
			Type fieldType = allInstanceField.FieldType;
			if (fieldType.IsValueType)
			{
				path.Add(allInstanceField);
				EmitCloneChildrenForType(il, fieldType, uniqueRef: false, path);
				path.RemoveAt(path.Count - 1);
				continue;
			}
			il.log.AppendLine("// " + fieldType.Name + " " + ((path.Count > 0) ? (string.Join(".", path.Select((FieldInfo f) => f.Name)) + ".") : "") + allInstanceField.Name);
			EmitFieldCloneChildren(il, allInstanceField, uniqueRef, path);
		}
	}

	private static void EmitFieldCloneChildren(LoggingILGenerator il, FieldInfo leaf, bool uniqueRef, List<FieldInfo> path)
	{
		il.Emit(OpCodes.Ldarg_0);
		foreach (FieldInfo item in path)
		{
			il.Emit(OpCodes.Ldflda, item);
		}
		il.Emit(OpCodes.Dup);
		il.Emit(OpCodes.Ldfld, leaf);
		EmitCloneCall(il, leaf.FieldType, uniqueRef);
		il.Emit(OpCodes.Stfld, leaf);
	}

	private static void EmitCloneCall(LoggingILGenerator il, Type ft, bool uniqueRef)
	{
		ClassTypeInfo classTypeInfo = DeepCloning.FreezeClassInfo(ft);
		if (uniqueRef && !classTypeInfo.NeedsPolymorphicClone && classTypeInfo.CloneByValue)
		{
			Label label = il.DefineLabel();
			il.Emit(OpCodes.Dup);
			il.Emit(OpCodes.Brfalse, label);
			il.Emit(OpCodes.Callvirt, _memberwiseCloneMethod);
			il.Emit(OpCodes.Castclass, ft);
			il.MarkLabel(label);
		}
		else
		{
			il.Emit(OpCodes.Ldarg_1);
			il.Emit(OpCodes.Call, classTypeInfo.GetHelperMethod(uniqueRef ? "CloneUniqueRef" : "CloneRef"));
		}
	}

	private static DynamicMethod BuildRestoreChildrenDynamicMethod(Type type, Type[] paramTypes, bool uniqueRef)
	{
		string text = "RestoreChildren_" + type.Name;
		DynamicMethod dynamicMethod = new DynamicMethod(text, null, paramTypes, type, skipVisibility: true);
		LoggingILGenerator loggingILGenerator = new LoggingILGenerator(dynamicMethod.GetILGenerator(), text);
		EmitRestoreChildrenForType(loggingILGenerator, type, uniqueRef);
		loggingILGenerator.Emit(OpCodes.Ret);
		WriteILLog(type, "RestoreChildren", loggingILGenerator.GetLog());
		return dynamicMethod;
	}

	private static void EmitRestoreChildrenForType(LoggingILGenerator il, Type type, bool uniqueRef, List<FieldInfo> path = null)
	{
		path = path ?? new List<FieldInfo>();
		foreach (FieldInfo allInstanceField in DeepCloning.GetAllInstanceFields(type))
		{
			if (!DeepCloning.NeedsFieldClone(allInstanceField))
			{
				continue;
			}
			Type fieldType = allInstanceField.FieldType;
			if (fieldType.IsValueType)
			{
				path.Add(allInstanceField);
				EmitRestoreChildrenForType(il, fieldType, uniqueRef: false, path);
				path.RemoveAt(path.Count - 1);
				continue;
			}
			il.log.AppendLine("// " + DeepCloning.DebugTypeName(fieldType) + " " + string.Join(".", path.Select((FieldInfo f) => f.Name).Concat(new string[1] { allInstanceField.Name })));
			EmitFieldRestoreChildren(il, allInstanceField, uniqueRef, path);
		}
	}

	private static void EmitFieldRestoreChildren(LoggingILGenerator il, FieldInfo leaf, bool uniqueRef, List<FieldInfo> path)
	{
		il.Emit(OpCodes.Ldarg_0);
		foreach (FieldInfo item in path)
		{
			il.Emit(OpCodes.Ldflda, item);
		}
		il.Emit(OpCodes.Dup);
		il.Emit(OpCodes.Ldfld, leaf);
		il.Emit(OpCodes.Ldarg_1);
		foreach (FieldInfo item2 in path)
		{
			il.Emit(OpCodes.Ldfld, item2);
		}
		il.Emit(OpCodes.Ldfld, leaf);
		EmitRestoreCall(il, leaf.FieldType, uniqueRef);
		il.Emit(OpCodes.Stfld, leaf);
	}

	private static void EmitRestoreCall(LoggingILGenerator il, Type ft, bool uniqueRef)
	{
		ClassTypeInfo classTypeInfo = DeepCloning.FreezeClassInfo(ft);
		if (uniqueRef && !classTypeInfo.NeedsPolymorphicClone && classTypeInfo.CloneByValue)
		{
			il.Emit(OpCodes.Pop);
			Label label = il.DefineLabel();
			il.Emit(OpCodes.Dup);
			il.Emit(OpCodes.Brfalse, label);
			il.Emit(OpCodes.Callvirt, _memberwiseCloneMethod);
			il.Emit(OpCodes.Castclass, ft);
			il.MarkLabel(label);
		}
		else
		{
			il.Emit(OpCodes.Ldarg_2);
			il.Emit(OpCodes.Call, classTypeInfo.GetHelperMethod(uniqueRef ? "RestoreUniqueRef" : "RestoreRef"));
		}
	}

	private static void CloneClassElements<E>(E[] clone, DeepCloneContext ctx) where E : class
	{
		for (int i = 0; i < clone.Length; i++)
		{
			clone[i] = DeepClassCloning<E>.CloneRef(clone[i], ctx);
		}
	}

	private static void CloneStructElements<E>(E[] clone, DeepCloneContext ctx) where E : struct
	{
		CloneStructChildren<E> cloneChildren = DeepStructCloning<E>.CloneChildren;
		for (int i = 0; i < clone.Length; i++)
		{
			cloneChildren(ref clone[i], ctx);
		}
	}

	private static void RestoreClassElements<E>(E[] clone, E[] current, DeepCloneContext ctx) where E : class
	{
		int num = Math.Min(current?.Length ?? 0, clone.Length);
		int i;
		for (i = 0; i < num; i++)
		{
			clone[i] = DeepClassCloning<E>.RestoreRef(clone[i], current[i], ctx);
		}
		for (; i < clone.Length; i++)
		{
			clone[i] = DeepClassCloning<E>.CloneRef(clone[i], ctx);
		}
	}

	private static void RestoreStructElements<E>(E[] clone, E[] current, DeepCloneContext ctx) where E : struct
	{
		RestoreStructChildren<E> restoreChildren = DeepStructCloning<E>.RestoreChildren;
		int num = Math.Min(current?.Length ?? 0, clone.Length);
		int i;
		for (i = 0; i < num; i++)
		{
			restoreChildren(ref clone[i], ref current[i], ctx);
		}
		CloneStructChildren<E> cloneChildren = DeepStructCloning<E>.CloneChildren;
		for (; i < clone.Length; i++)
		{
			cloneChildren(ref clone[i], ctx);
		}
	}

	private static void CloneClassElements2D<E>(E[,] clone, DeepCloneContext ctx) where E : class
	{
		//IL_001e: Expected VT, but got Obj
		for (int i = 0; i < clone.GetLength(0); i++)
		{
			for (int j = 0; j < clone.GetLength(1); j++)
			{
				clone[i, j] = DeepClassCloning<E>.CloneRef(clone[i, j], ctx);
			}
		}
	}

	private static void CloneStructElements2D<E>(E[,] clone, DeepCloneContext ctx) where E : struct
	{
		CloneStructChildren<E> cloneChildren = DeepStructCloning<E>.CloneChildren;
		for (int i = 0; i < clone.GetLength(0); i++)
		{
			for (int j = 0; j < clone.GetLength(1); j++)
			{
				cloneChildren(ref clone[i, j], ctx);
			}
		}
	}

	private static void RestoreClassElements2D<E>(E[,] clone, E[,] current, DeepCloneContext ctx) where E : class
	{
		//IL_0034: Expected VT, but got Obj
		for (int i = 0; i < clone.GetLength(0); i++)
		{
			for (int j = 0; j < clone.GetLength(1); j++)
			{
				clone[i, j] = DeepClassCloning<E>.RestoreRef(clone[i, j], (current != null) ? current[i, j] : null, ctx);
			}
		}
	}

	private static void RestoreStructElements2D<E>(E[,] clone, E[,] current, DeepCloneContext ctx) where E : struct
	{
		RestoreStructChildren<E> restoreChildren = DeepStructCloning<E>.RestoreChildren;
		for (int i = 0; i < clone.GetLength(0); i++)
		{
			for (int j = 0; j < clone.GetLength(1); j++)
			{
				restoreChildren(ref clone[i, j], ref current[i, j], ctx);
			}
		}
	}

	private static void WriteILLog(Type type, string variant, string log)
	{
		if (!DeepCloning.DiagnosticsEnabled)
		{
			return;
		}
		try
		{
			string text = Path.Combine(Main.SavePath, "dev", "clone-gen");
			Directory.CreateDirectory(text);
			File.WriteAllText(Path.Combine(text, DeepCloning.DebugTypeName(type).Replace('<', '[').Replace('>', ']') + "." + variant + ".il"), log);
		}
		catch
		{
		}
	}
}
