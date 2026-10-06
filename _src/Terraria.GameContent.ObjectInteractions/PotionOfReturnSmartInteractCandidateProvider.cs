using Microsoft.Xna.Framework;

namespace Terraria.GameContent.ObjectInteractions;

public class PotionOfReturnSmartInteractCandidateProvider : ISmartInteractCandidateProvider
{
	private class ReusableCandidate : ISmartInteractCandidate
	{
		public float DistanceFromCursor { get; private set; }

		public void WinCandidacy()
		{
			Main.SmartInteractPotionOfReturn = true;
			Main.SmartInteractShowingGenuine = true;
		}

		public void Reuse(float distanceFromCursor)
		{
			DistanceFromCursor = distanceFromCursor;
		}
	}

	private ReusableCandidate _candidate = new ReusableCandidate();

	public void ClearSelfAndPrepareForCheck()
	{
		Main.SmartInteractPotionOfReturn = false;
	}

	public bool ProvideCandidate(SmartInteractScanSettings settings, out ISmartInteractCandidate candidate)
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		candidate = null;
		if (!PotionOfReturnHelper.TryGetGateHitbox(settings.player, out var homeHitbox))
		{
			return false;
		}
		Vector2 val = homeHitbox.ClosestPointInRect(settings.mousevec);
		float distanceFromCursor = val.Distance(settings.mousevec);
		Point val2 = val.ToTileCoordinates();
		if (val2.X < settings.LX || val2.X > settings.HX || val2.Y < settings.LY || val2.Y > settings.HY)
		{
			return false;
		}
		_candidate.Reuse(distanceFromCursor);
		candidate = _candidate;
		return true;
	}
}
