using System;

namespace Terraria.Testing.Cloning;

public class ActiveCloneHelper<T> : BackupRestoreHelper<T> where T : class, new()
{
	private static readonly T _dummy = new T();

	private readonly Predicate<T> Active;

	private readonly Action<T> SetInactive;

	public ActiveCloneHelper(Predicate<T> active, Action<T> setInactive)
	{
		Active = active;
		SetInactive = setInactive;
	}

	public override T TryQuickBackup(T src, DeepCloneContext ctx)
	{
		if (Active(src))
		{
			return null;
		}
		return _dummy;
	}

	public override T TryQuickRestore(T backup, T current, DeepCloneContext ctx)
	{
		if (backup != _dummy)
		{
			return null;
		}
		if (current == null)
		{
			current = new T();
		}
		SetInactive(current);
		return current;
	}
}
