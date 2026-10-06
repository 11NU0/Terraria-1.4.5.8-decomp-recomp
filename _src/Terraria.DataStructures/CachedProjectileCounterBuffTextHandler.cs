namespace Terraria.DataStructures;

public class CachedProjectileCounterBuffTextHandler : IBuffTextHandler
{
	private int[] projectilesToLookFor;

	public CachedProjectileCounterBuffTextHandler(params int[] projectileTypesToLookFor)
	{
		projectilesToLookFor = projectileTypesToLookFor;
	}

	public string HandleBuffText()
	{
		if (projectilesToLookFor == null)
		{
			return null;
		}
		int[] ownedProjectileCounts = Main.LocalPlayer.ownedProjectileCounts;
		float num = 0f;
		int[] array = projectilesToLookFor;
		foreach (int num2 in array)
		{
			num += (float)ownedProjectileCounts[num2];
		}
		if (num > 0f)
		{
			return "x" + num;
		}
		return null;
	}
}
