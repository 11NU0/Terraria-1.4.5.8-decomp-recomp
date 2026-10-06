using Microsoft.Xna.Framework;

namespace Terraria.WorldBuilding;

public static class Searches
{
	public class Left : GenSearch
	{
		private int _maxDistance;

		public Left(int maxDistance)
		{
			_maxDistance = maxDistance;
		}

		public override Point Find(Point origin)
		{
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			for (int i = 0; i < _maxDistance; i++)
			{
				if (Check(origin.X - i, origin.Y))
				{
					return new Point(origin.X - i, origin.Y);
				}
			}
			return GenSearch.NOT_FOUND;
		}
	}

	public class Right : GenSearch
	{
		private int _maxDistance;

		public Right(int maxDistance)
		{
			_maxDistance = maxDistance;
		}

		public override Point Find(Point origin)
		{
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			for (int i = 0; i < _maxDistance; i++)
			{
				if (Check(origin.X + i, origin.Y))
				{
					return new Point(origin.X + i, origin.Y);
				}
			}
			return GenSearch.NOT_FOUND;
		}
	}

	public class Down : GenSearch
	{
		private int _maxDistance;

		public Down(int maxDistance)
		{
			_maxDistance = maxDistance;
		}

		public override Point Find(Point origin)
		{
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			for (int i = 0; i < _maxDistance && origin.Y + i < Main.maxTilesY; i++)
			{
				if (Check(origin.X, origin.Y + i))
				{
					return new Point(origin.X, origin.Y + i);
				}
			}
			return GenSearch.NOT_FOUND;
		}
	}

	public class Up : GenSearch
	{
		private int _maxDistance;

		public Up(int maxDistance)
		{
			_maxDistance = maxDistance;
		}

		public override Point Find(Point origin)
		{
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			for (int i = 0; i < _maxDistance; i++)
			{
				if (Check(origin.X, origin.Y - i))
				{
					return new Point(origin.X, origin.Y - i);
				}
			}
			return GenSearch.NOT_FOUND;
		}
	}

	public class Rectangle : GenSearch
	{
		private int _width;

		private int _height;

		public Rectangle(int width, int height)
		{
			_width = width;
			_height = height;
		}

		public override Point Find(Point origin)
		{
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			for (int i = 0; i < _width; i++)
			{
				for (int j = 0; j < _height; j++)
				{
					if (Check(origin.X + i, origin.Y + j))
					{
						return new Point(origin.X + i, origin.Y + j);
					}
				}
			}
			return GenSearch.NOT_FOUND;
		}
	}

	public static GenSearch Chain(GenSearch search, params GenCondition[] conditions)
	{
		return search.Conditions(conditions);
	}
}
