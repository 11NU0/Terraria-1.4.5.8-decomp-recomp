namespace Terraria.DataStructures;

public struct WingStats(int flyTime = 100, float flySpeedOverride = -1f, float accelerationMultiplier = 1f, bool hasHoldDownHoverFeatures = false, float hoverFlySpeedOverride = -1f, float hoverAccelerationMultiplier = 1f)
{
	public static readonly WingStats Default;

	public int FlyTime = flyTime;

	public float AccRunSpeedOverride = flySpeedOverride;

	public float AccRunAccelerationMult = accelerationMultiplier;

	public bool HasDownHoverStats = hasHoldDownHoverFeatures;

	public float DownHoverSpeedOverride = hoverFlySpeedOverride;

	public float DownHoverAccelerationMult = hoverAccelerationMultiplier;

	public WingStats WithSpeedBoost(float multiplier)
	{
		return new WingStats(FlyTime, AccRunSpeedOverride * multiplier, AccRunAccelerationMult, HasDownHoverStats, DownHoverSpeedOverride * multiplier, DownHoverAccelerationMult);
	}
}
