using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria.WorldBuilding;

namespace Terraria.GameContent.Generation;

public class ShapeFloodFill : GenShape
{
	private int _maximumActions;

	public ShapeFloodFill(int maximumActions = 100)
	{
		_maximumActions = maximumActions;
	}

	public override bool Perform(Point origin, GenAction action)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0121: Unknown result type (might be due to invalid IL or missing references)
		//IL_0126: Unknown result type (might be due to invalid IL or missing references)
		//IL_0129: Unknown result type (might be due to invalid IL or missing references)
		//IL_0130: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0144: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0104: Unknown result type (might be due to invalid IL or missing references)
		Queue<Point> queue = new Queue<Point>();
		HashSet<Point16> hashSet = new HashSet<Point16>();
		queue.Enqueue(origin);
		int num = _maximumActions;
		while (queue.Count > 0 && num > 0)
		{
			Point val = queue.Dequeue();
			if (!hashSet.Contains(new Point16(val.X, val.Y)) && UnitApply(action, origin, val.X, val.Y))
			{
				hashSet.Add(new Point16(val));
				num--;
				if (val.X + 1 < Main.maxTilesX - 1)
				{
					queue.Enqueue(new Point(val.X + 1, val.Y));
				}
				if (val.X - 1 >= 1)
				{
					queue.Enqueue(new Point(val.X - 1, val.Y));
				}
				if (val.Y + 1 < Main.maxTilesY - 1)
				{
					queue.Enqueue(new Point(val.X, val.Y + 1));
				}
				if (val.Y - 1 >= 1)
				{
					queue.Enqueue(new Point(val.X, val.Y - 1));
				}
			}
		}
		while (queue.Count > 0)
		{
			Point val2 = queue.Dequeue();
			if (!hashSet.Contains(new Point16(val2.X, val2.Y)))
			{
				queue.Enqueue(val2);
				break;
			}
		}
		return queue.Count == 0;
	}
}
