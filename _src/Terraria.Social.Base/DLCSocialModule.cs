namespace Terraria.Social.Base;

public abstract class DLCSocialModule : ISocialModule
{
	public abstract void Initialize();

	public abstract void Shutdown();

	public abstract bool HasDLC(DLCName dlc);
}
