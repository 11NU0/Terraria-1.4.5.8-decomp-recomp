namespace Terraria;

public static class NewProjectileModifiers
{
	public static void HardmodeDungeonSkeletonShot(Projectile projectile)
	{
		projectile.timeLeft = 300;
	}

	public static void RainHazard(Projectile projectile)
	{
		projectile.netImportant = true;
	}

	public static void IchorDartUpdatePenetrate(Projectile projectile)
	{
		if (Main.myPlayer == projectile.owner)
		{
			if (projectile.ai[1] >= 0f)
			{
				projectile.maxPenetrate = (projectile.penetrate = -1);
			}
			else if (projectile.penetrate < 0)
			{
				projectile.maxPenetrate = (projectile.penetrate = 1);
			}
		}
	}
}
