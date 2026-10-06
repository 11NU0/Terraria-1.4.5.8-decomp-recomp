using Microsoft.Xna.Framework;
using Terraria.DataStructures;

namespace Terraria.GameContent.ObjectInteractions;

public class NPCSmartInteractCandidateProvider : ISmartInteractCandidateProvider
{
	private class ReusableCandidate : ISmartInteractCandidate
	{
		private int _npcIndexToTarget;

		public float DistanceFromCursor { get; private set; }

		public void WinCandidacy()
		{
			Main.SmartInteractNPC = _npcIndexToTarget;
			Main.SmartInteractShowingGenuine = true;
		}

		public void Reuse(int npcIndex, float npcDistanceFromCursor)
		{
			_npcIndexToTarget = npcIndex;
			DistanceFromCursor = npcDistanceFromCursor;
		}
	}

	private ReusableCandidate _candidate = new ReusableCandidate();

	public void ClearSelfAndPrepareForCheck()
	{
		Main.SmartInteractNPC = -1;
	}

	public bool ProvideCandidate(SmartInteractScanSettings settings, out ISmartInteractCandidate candidate)
	{
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		candidate = null;
		if (!settings.FullInteraction)
		{
			return false;
		}
		Rectangle worldRegion = TileReachCheckSettings.Simple.GetWorldRegion(settings.player);
		Vector2 mousevec = settings.mousevec;
		mousevec.ToPoint();
		bool flag = false;
		int num = -1;
		float npcDistanceFromCursor = -1f;
		for (int i = 0; i < Main.maxNPCs; i++)
		{
			NPC nPC = Main.npc[i];
			if (!nPC.active || !nPC.townNPC)
			{
				continue;
			}
			Rectangle hitbox = nPC.Hitbox;
			if (hitbox.Intersects(worldRegion) && !flag)
			{
				float num2 = nPC.Hitbox.Distance(mousevec);
				if (num == -1 || Main.npc[num].Hitbox.Distance(mousevec) > num2)
				{
					num = i;
					npcDistanceFromCursor = num2;
				}
				if (num2 == 0f)
				{
					flag = true;
					num = i;
					npcDistanceFromCursor = num2;
					break;
				}
			}
		}
		if (settings.DemandOnlyZeroDistanceTargets && !flag)
		{
			return false;
		}
		if (num != -1)
		{
			_candidate.Reuse(num, npcDistanceFromCursor);
			candidate = _candidate;
			return true;
		}
		return false;
	}
}
