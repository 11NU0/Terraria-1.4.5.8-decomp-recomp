using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Terraria.Testing.Cloning;

public sealed class DeepCloneContext
{
	private readonly bool _isBackup;

	private readonly Dictionary<object, object> Map = new Dictionary<object, object>(RefEqualityComparer.Instance);

	public bool IsBackup => _isBackup;

	public DeepCloneContext(bool isBackup = false)
	{
		_isBackup = isBackup;
	}

	internal void AddClone<T>(T src, T clone) where T : class
	{
		Map.Add(src, clone);
	}

	internal bool TryGetClone<T>(T src, out T clone) where T : class
	{
		if (Map.TryGetValue(src, out var value))
		{
			clone = (T)value;
			return true;
		}
		clone = null;
		return false;
	}

	public void DumpMap()
	{
		if (DeepCloning.DiagnosticsEnabled)
		{
			File.WriteAllLines(Path.Combine(Main.SavePath, "dev", "clone-gen", "map.txt"), from k in Map.Keys
				select k.GetType() into t
				group t by t into g
				orderby g.Count() descending
				select g.Count() + ": " + DeepCloning.DebugTypeName(g.Key));
		}
	}

	internal bool TryQuickClone<T>(T src, out T clone) where T : class
	{
		BackupRestoreHelper<T> instance = BackupRestoreHelper<T>.Instance;
		clone = ((instance != null) ? instance.TryQuickBackup(src, this) : null);
		return clone != null;
	}

	internal bool TryQuickRestore<T>(T backup, T current, out T clone) where T : class
	{
		BackupRestoreHelper<T> instance = BackupRestoreHelper<T>.Instance;
		clone = ((instance != null) ? instance.TryQuickRestore(backup, current, this) : null);
		return clone != null;
	}
}
