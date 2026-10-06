namespace Terraria.Testing.Cloning;

public abstract class BackupRestoreHelper<T> where T : class
{
	public static BackupRestoreHelper<T> Instance { get; private set; }

	public static void Register(BackupRestoreHelper<T> helper)
	{
		Instance = helper;
	}

	public abstract T TryQuickBackup(T src, DeepCloneContext ctx);

	public abstract T TryQuickRestore(T backup, T current, DeepCloneContext ctx);
}
