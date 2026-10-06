using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using ReLogic.Utilities;
using Terraria.WorldBuilding;

namespace Terraria.GameContent.Generation;

public class ShapeBranch : GenShape
{
	private Point _offset;

	private List<Point> _endPoints;

	public ShapeBranch()
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		_offset = new Point(10, -5);
	}

	public ShapeBranch(Point offset)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		_offset = offset;
	}

	public ShapeBranch(double angle, double distance)
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		_offset = new Point((int)(Math.Cos(angle) * distance), (int)(Math.Sin(angle) * distance));
	}

	private bool PerformSegment(Point origin, GenAction action, Point start, Point end, int size)
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		size = Math.Max(1, size);
		for (int i = -(size >> 1); i < size - (size >> 1); i++)
		{
			for (int j = -(size >> 1); j < size - (size >> 1); j++)
			{
				if (!Utils.PlotLine(new Point(start.X + i, start.Y + j), end, (int tileX, int tileY) =>
				{
					//IL_000d: Unknown result type (might be due to invalid IL or missing references)
					return UnitApply(action, origin, tileX, tileY) || !_quitOnFail;
				}, jump: false))
				{
					return false;
				}
			}
		}
		return true;
	}

	public override bool Perform(Point origin, GenAction action)
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_010c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0120: Unknown result type (might be due to invalid IL or missing references)
		//IL_0129: Unknown result type (might be due to invalid IL or missing references)
		//IL_012e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0163: Unknown result type (might be due to invalid IL or missing references)
		//IL_0169: Unknown result type (might be due to invalid IL or missing references)
		//IL_016b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0179: Unknown result type (might be due to invalid IL or missing references)
		//IL_017e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0182: Unknown result type (might be due to invalid IL or missing references)
		//IL_018a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0192: Unknown result type (might be due to invalid IL or missing references)
		//IL_019a: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01de: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0206: Unknown result type (might be due to invalid IL or missing references)
		//IL_020d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0214: Unknown result type (might be due to invalid IL or missing references)
		//IL_021b: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d1: Unknown result type (might be due to invalid IL or missing references)
		Vector2D val = new Vector2D((double)_offset.X, (double)_offset.Y);
		double num = val.Length();
		int num2 = (int)(num / 6.0);
		if (_endPoints != null)
		{
			_endPoints.Add(new Point(origin.X + _offset.X, origin.Y + _offset.Y));
		}
		if (!PerformSegment(origin, action, origin, new Point(origin.X + _offset.X, origin.Y + _offset.Y), num2))
		{
			return false;
		}
		int num3 = (int)(num / 8.0);
		for (int i = 0; i < num3; i++)
		{
			double num4 = ((double)i + 1.0) / ((double)num3 + 1.0);
			Point val2 = new Point((int)(num4 * (double)_offset.X), (int)(num4 * (double)_offset.Y));
			Vector2D spinningpoint = new Vector2D((double)(_offset.X - val2.X), (double)(_offset.Y - val2.Y));
			spinningpoint = spinningpoint.RotatedBy((GenBase._random.NextDouble() * 0.5 + 1.0) * (double)((GenBase._random.Next(2) != 0) ? 1 : (-1))) * 0.75;
			Point val3 = new Point((int)spinningpoint.X + val2.X, (int)spinningpoint.Y + val2.Y);
			if (_endPoints != null)
			{
				_endPoints.Add(new Point(val3.X + origin.X, val3.Y + origin.Y));
			}
			if (!PerformSegment(origin, action, new Point(val2.X + origin.X, val2.Y + origin.Y), new Point(val3.X + origin.X, val3.Y + origin.Y), num2 - 1))
			{
				return false;
			}
		}
		return true;
	}

	public ShapeBranch OutputEndpoints(List<Point> endpoints)
	{
		_endPoints = endpoints;
		return this;
	}
}
