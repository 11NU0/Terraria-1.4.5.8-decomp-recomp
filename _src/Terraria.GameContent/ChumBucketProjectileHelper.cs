using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Terraria.GameContent;

public class ChumBucketProjectileHelper
{
	private Dictionary<Point, int> _chumCountsPendingForThisFrame = new Dictionary<Point, int>();

	private Dictionary<Point, int> _chumCountsFromLastFrame = new Dictionary<Point, int>();

	public void OnPreUpdateAllProjectiles()
	{
		Utils.Swap(ref _chumCountsPendingForThisFrame, ref _chumCountsFromLastFrame);
		_chumCountsPendingForThisFrame.Clear();
	}

	public void AddChumLocation(Vector2 spot)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		Point key = spot.ToTileCoordinates();
		int value = 0;
		_chumCountsPendingForThisFrame.TryGetValue(key, out value);
		value++;
		_chumCountsPendingForThisFrame[key] = value;
	}

	public int GetChumsInLocation(Point tileCoords)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		int value = 0;
		_chumCountsFromLastFrame.TryGetValue(tileCoords, out value);
		return value;
	}
}
