using Microsoft.Xna.Framework;

namespace Terraria.DataStructures;

public class MultiPointHitbox
{
	public readonly Point PointSize;

	public readonly Vector2[] Points;

	public readonly Rectangle BoundingRect;

	public MultiPointHitbox(Point pointSize, Vector2[] points)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		PointSize = pointSize;
		Points = points;
		Rectangle val = Utils.CenteredRectangle(points[0], Vector2.Zero);
		foreach (Vector2 v in points)
		{
			val = val.Including(v.ToPoint());
		}
		BoundingRect = val;
	}

	public bool Intersects(Rectangle targetRect)
	{
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		targetRect.Inflate(PointSize.X / 2, PointSize.Y / 2);
		Rectangle boundingRect = BoundingRect;
		if (!boundingRect.Intersects(targetRect))
		{
			return false;
		}
		Vector2[] points = Points;
		foreach (Vector2 v in points)
		{
			if (targetRect.Contains(v.ToPoint()))
			{
				return true;
			}
		}
		return false;
	}
}
