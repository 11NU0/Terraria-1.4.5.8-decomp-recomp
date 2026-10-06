using Terraria.GameContent.Drawing;

namespace Terraria.GameContent;

public struct ShimmerUnstuckHelper
{
	public int TimeLeftUnstuck;

	public bool IndefiniteProtectionActive;

	public bool ShouldUnstuck
	{
		get
		{
			if (!IndefiniteProtectionActive)
			{
				return TimeLeftUnstuck > 0;
			}
			return true;
		}
	}

	public void Update(Player player)
	{
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		bool flag = !player.shimmering && !player.shimmerWet;
		if (flag)
		{
			IndefiniteProtectionActive = false;
		}
		if (TimeLeftUnstuck > 0 && !flag)
		{
			StartUnstuck();
		}
		if (!IndefiniteProtectionActive && TimeLeftUnstuck > 0)
		{
			TimeLeftUnstuck--;
			if (TimeLeftUnstuck == 0)
			{
				ParticleOrchestrator.BroadcastOrRequestParticleSpawn(ParticleOrchestraType.ShimmerTownNPC, new ParticleOrchestraSettings
				{
					PositionInWorld = player.Bottom
				});
			}
		}
	}

	public void StartUnstuck()
	{
		IndefiniteProtectionActive = true;
		TimeLeftUnstuck = 120;
	}

	public void Clear()
	{
		IndefiniteProtectionActive = false;
		TimeLeftUnstuck = 0;
	}
}
