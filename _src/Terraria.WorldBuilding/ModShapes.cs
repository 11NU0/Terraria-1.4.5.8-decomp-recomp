using Microsoft.Xna.Framework;
using Terraria.DataStructures;

namespace Terraria.WorldBuilding;

public static class ModShapes
{
	public class All : GenModShape
	{
		public All(ShapeData data)
			: base(data)
		{
		}

		public override bool Perform(Point origin, GenAction action)
		{
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			foreach (Point16 datum in _data.GetData())
			{
				if (!UnitApply(action, origin, datum.X + origin.X, datum.Y + origin.Y) && _quitOnFail)
				{
					return false;
				}
			}
			return true;
		}
	}

	public class OuterOutline : GenModShape
	{
		private static readonly int[] POINT_OFFSETS = new int[16]
		{
			1, 0, -1, 0, 0, 1, 0, -1, 1, 1,
			1, -1, -1, 1, -1, -1
		};

		private bool _useDiagonals;

		private bool _useInterior;

		public OuterOutline(ShapeData data, bool useDiagonals = true, bool useInterior = false)
			: base(data)
		{
			_useDiagonals = useDiagonals;
			_useInterior = useInterior;
		}

		public override bool Perform(Point origin, GenAction action)
		{
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
			int num = (_useDiagonals ? 16 : 8);
			foreach (Point16 datum in _data.GetData())
			{
				if (_useInterior && !UnitApply(action, origin, datum.X + origin.X, datum.Y + origin.Y) && _quitOnFail)
				{
					return false;
				}
				for (int i = 0; i < num; i += 2)
				{
					if (!_data.Contains(datum.X + POINT_OFFSETS[i], datum.Y + POINT_OFFSETS[i + 1]) && !UnitApply(action, origin, origin.X + datum.X + POINT_OFFSETS[i], origin.Y + datum.Y + POINT_OFFSETS[i + 1]) && _quitOnFail)
					{
						return false;
					}
				}
			}
			return true;
		}
	}

	public class InnerOutline : GenModShape
	{
		private static readonly int[] POINT_OFFSETS = new int[16]
		{
			1, 0, -1, 0, 0, 1, 0, -1, 1, 1,
			1, -1, -1, 1, -1, -1
		};

		private bool _useDiagonals;

		public InnerOutline(ShapeData data, bool useDiagonals = true)
			: base(data)
		{
			_useDiagonals = useDiagonals;
		}

		public override bool Perform(Point origin, GenAction action)
		{
			//IL_0074: Unknown result type (might be due to invalid IL or missing references)
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0088: Unknown result type (might be due to invalid IL or missing references)
			int num = (_useDiagonals ? 16 : 8);
			foreach (Point16 datum in _data.GetData())
			{
				bool flag = false;
				for (int i = 0; i < num; i += 2)
				{
					if (!_data.Contains(datum.X + POINT_OFFSETS[i], datum.Y + POINT_OFFSETS[i + 1]))
					{
						flag = true;
						break;
					}
				}
				if (flag && !UnitApply(action, origin, datum.X + origin.X, datum.Y + origin.Y) && _quitOnFail)
				{
					return false;
				}
			}
			return true;
		}
	}
}
