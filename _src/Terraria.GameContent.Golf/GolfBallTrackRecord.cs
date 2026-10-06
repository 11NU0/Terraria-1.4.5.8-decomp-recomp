using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Terraria.GameContent.Golf;

public class GolfBallTrackRecord
{
	private List<Vector2> _hitLocations = new List<Vector2>();

	public void RecordHit(Vector2 position)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		_hitLocations.Add(position);
	}

	public int GetAccumulatedScore()
	{
		GetTrackInfo(out var totalDistancePassed, out var hitsMade);
		int num = (int)(totalDistancePassed / 16.0);
		int num2 = hitsMade + 2;
		return num / num2;
	}

	private void GetTrackInfo(out double totalDistancePassed, out int hitsMade)
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		hitsMade = 0;
		totalDistancePassed = 0.0;
		int num = 0;
		while (num < _hitLocations.Count - 1)
		{
			totalDistancePassed += Vector2.Distance(_hitLocations[num], _hitLocations[num + 1]);
			num++;
			hitsMade++;
		}
	}
}
