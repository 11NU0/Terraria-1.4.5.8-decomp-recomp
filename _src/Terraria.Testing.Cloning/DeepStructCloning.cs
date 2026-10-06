namespace Terraria.Testing.Cloning;

internal static class DeepStructCloning<T> where T : struct
{
	private static readonly StructTypeInfo Info;

	public static readonly CloneStructChildren<T> CloneChildren;

	public static readonly RestoreStructChildren<T> RestoreChildren;

	static DeepStructCloning()
	{
		Info = DeepCloning.FreezeStructInfo(typeof(T));
		if (Info.CloneByValue)
		{
			CloneChildren = delegate
			{
			};
			RestoreChildren = delegate
			{
			};
		}
		else
		{
			DeepCloneCodegen.GenerateStructDelegates(out CloneChildren, out RestoreChildren);
		}
	}

	public static T CloneStruct(T source, DeepCloneContext ctx)
	{
		if (Info.CloneByValue)
		{
			return source;
		}
		T clone = source;
		CloneChildren(ref clone, ctx);
		return clone;
	}

	public static T RestoreStruct(T backup, T current, DeepCloneContext ctx)
	{
		if (Info.CloneByValue)
		{
			return backup;
		}
		RestoreChildren(ref backup, ref current, ctx);
		return backup;
	}
}
