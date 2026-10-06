using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;

namespace Terraria.WorldBuilding;

public class ShapeData
{
	private HashSet<Point16> _points;

	public int Count => _points.Count;

	public ShapeData()
	{
		_points = new HashSet<Point16>();
	}

	public ShapeData(ShapeData original)
	{
		_points = new HashSet<Point16>(original._points);
	}

	public void Add(int x, int y)
	{
		Point16 item = new Point16(x, y);
		if (!_points.Contains(item))
		{
			_points.Add(item);
		}
	}

	public void AddBounds(int minX, int minY, int maxX, int maxY)
	{
		for (int i = minX; i <= maxX; i++)
		{
			for (int j = minY; j <= maxY; j++)
			{
				Add(i, j);
			}
		}
	}

	public void Remove(int x, int y)
	{
		Point16 item = new Point16(x, y);
		if (_points.Contains(item))
		{
			_points.Remove(item);
		}
	}

	public void RemoveBounds(int minX, int minY, int maxX, int maxY)
	{
		for (int i = minX; i <= maxX; i++)
		{
			for (int j = minY; j <= maxY; j++)
			{
				Remove(i, j);
			}
		}
	}

	public HashSet<Point16> GetData()
	{
		return _points;
	}

	public void Clear()
	{
		_points.Clear();
	}

	public bool Contains(int x, int y)
	{
		return _points.Contains(new Point16(x, y));
	}

	public void Add(ShapeData shapeData, Point localOrigin, Point remoteOrigin)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		foreach (Point16 datum in shapeData.GetData())
		{
			Add(remoteOrigin.X - localOrigin.X + datum.X, remoteOrigin.Y - localOrigin.Y + datum.Y);
		}
	}

	public void Subtract(ShapeData shapeData, Point localOrigin, Point remoteOrigin)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		foreach (Point16 datum in shapeData.GetData())
		{
			Remove(remoteOrigin.X - localOrigin.X + datum.X, remoteOrigin.Y - localOrigin.Y + datum.Y);
		}
	}

	public static Rectangle GetBounds(Point origin, params ShapeData[] shapes)
	{
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		int num = shapes[0]._points.First().X;
		int num2 = num;
		int num3 = shapes[0]._points.First().Y;
		int num4 = num3;
		for (int i = 0; i < shapes.Length; i++)
		{
			foreach (Point16 point in shapes[i]._points)
			{
				num = Math.Max(num, point.X);
				num2 = Math.Min(num2, point.X);
				num3 = Math.Max(num3, point.Y);
				num4 = Math.Min(num4, point.Y);
			}
		}
		return new Rectangle(num2 + origin.X, num4 + origin.Y, num - num2, num3 - num4);
	}
}
