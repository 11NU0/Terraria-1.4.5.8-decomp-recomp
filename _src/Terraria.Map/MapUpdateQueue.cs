using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria.WorldBuilding;

namespace Terraria.Map;

public class MapUpdateQueue
{
	private const int MAX_QUEUED_UPDATES = 262144;

	private static List<Rectangle> _areaUpdateQueue = new List<Rectangle>();

	private static Point16[] _updateQueue = new Point16[1024];

	private static int _updateCount = 0;

	private static readonly object _lock = new object();

	public static void Add(Rectangle area)
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		if (Main.dedServ || WorldGen.generatingWorld || !Main.mapEnabled)
		{
			return;
		}
		area = WorldUtils.ClampToWorld(area);
		lock (_lock)
		{
			_areaUpdateQueue.Add(area);
			for (int i = area.Left; i < area.Right; i++)
			{
				for (int j = area.Top; j < area.Bottom; j++)
				{
					Main.Map.QueueUpdate(i, j);
				}
			}
		}
	}

	public static void Add(int x, int y)
	{
		if (Main.dedServ || WorldGen.generatingWorld || !Main.mapEnabled || !Main.Map.QueueUpdate(x, y))
		{
			return;
		}
		lock (_lock)
		{
			if (_updateCount == _updateQueue.Length)
			{
				if (_updateCount >= 262144)
				{
					return;
				}
				Array.Resize(ref _updateQueue, _updateCount * 2);
			}
			_updateQueue[_updateCount++] = new Point16(x, y);
		}
	}

	public static void Update()
	{
		TimeLogger.StartTimestamp fromTimestamp = TimeLogger.Start();
		lock (_lock)
		{
			UpdateTiles();
			UpdateAreas();
		}
		TimeLogger.MapChanges.AddTime(fromTimestamp);
	}

	private static void UpdateAreas()
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		foreach (Rectangle item in _areaUpdateQueue)
		{
			Rectangle current = item;
			for (int i = current.Left; i < current.Right; i++)
			{
				for (int j = current.Top; j < current.Bottom; j++)
				{
					if (Main.Map.UpdateType(i, j))
					{
						MapRenderer.QueueChange(i, j);
					}
				}
			}
		}
		_areaUpdateQueue.Clear();
	}

	private static void UpdateTiles()
	{
		for (int i = 0; i < _updateCount; i++)
		{
			Point16 point = _updateQueue[i];
			if (Main.Map.UpdateType(point.X, point.Y))
			{
				MapRenderer.QueueChange(point.X, point.Y);
			}
		}
		_updateCount = 0;
	}
}
