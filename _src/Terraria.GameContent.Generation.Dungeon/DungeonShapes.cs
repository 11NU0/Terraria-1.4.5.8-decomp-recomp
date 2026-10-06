using System;
using Microsoft.Xna.Framework;
using Terraria.WorldBuilding;

namespace Terraria.GameContent.Generation.Dungeon;

public class DungeonShapes
{
	public class CircleRoom : GenShape
	{
		private int _verticalRadius;

		private int _horizontalRadius;

		public int VerticalRadius => _verticalRadius;

		public int HorizontalRadius => _horizontalRadius;

		public CircleRoom(int radius)
		{
			_verticalRadius = radius;
			_horizontalRadius = radius;
		}

		public CircleRoom(int horizontalRadius, int verticalRadius)
		{
			_horizontalRadius = horizontalRadius;
			_verticalRadius = verticalRadius;
		}

		public void SetRadius(int radius)
		{
			_verticalRadius = radius;
			_horizontalRadius = radius;
		}

		public override bool Perform(Point origin, GenAction action)
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0094: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_0085: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			int num = (_horizontalRadius + 1) * (_horizontalRadius + 1);
			for (int i = origin.Y - _verticalRadius; i <= origin.Y + _verticalRadius; i++)
			{
				double num2 = (double)_horizontalRadius / (double)_verticalRadius * (double)(i - origin.Y);
				int num3 = Math.Min(_horizontalRadius, (int)Math.Sqrt((double)num - num2 * num2));
				for (int j = origin.X - num3; j <= origin.X + num3; j++)
				{
					if (!UnitApply(action, origin, j, i) && _quitOnFail)
					{
						return false;
					}
				}
			}
			return true;
		}
	}

	public class MoundRoom : GenShape
	{
		private int _halfWidth;

		private int _height;

		public MoundRoom(int halfWidth, int height)
		{
			_halfWidth = halfWidth;
			_height = height;
		}

		public override bool Perform(Point origin, GenAction action)
		{
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			_ = _height;
			float num = _halfWidth;
			int num2 = _height / 2;
			for (int i = -_halfWidth; i <= _halfWidth; i++)
			{
				int num3 = Math.Min(_height, (int)((0f - (float)(_height + 1) / (num * num)) * ((float)i + num) * ((float)i - num)));
				for (int j = 0; j < num3; j++)
				{
					if (!UnitApply(action, origin, i + origin.X, origin.Y - j + num2) && _quitOnFail)
					{
						return false;
					}
				}
			}
			return true;
		}
	}

	public class HourglassRoom : GenShape
	{
		private int _width;

		private int _height;

		private float _percentileAddon;

		public HourglassRoom(int width, int height, float percentileAddon)
		{
			_width = width;
			_height = height;
			_percentileAddon = percentileAddon;
		}

		public override bool Perform(Point origin, GenAction action)
		{
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0081: Unknown result type (might be due to invalid IL or missing references)
			//IL_008e: Unknown result type (might be due to invalid IL or missing references)
			int num = _height / 2;
			for (int i = -num; i <= num; i++)
			{
				int y = origin.Y + i;
				float percent = ((float)i + (float)num) / (float)_height;
				float num2 = Math.Max(0f, Math.Min(1f, Utils.MultiLerp(Utils.WrappedLerp(0f, 1f, percent), 1f, 1f, 0.75f, 0.65f, 0.45f, 0.4f, 0.35f, 0.35f) + _percentileAddon));
				int num3 = (int)((float)_width * num2) / 2;
				for (int j = -num3; j <= num3; j++)
				{
					int x = origin.X + j;
					if (!UnitApply(action, origin, x, y) && _quitOnFail)
					{
						return false;
					}
				}
			}
			return true;
		}
	}

	public class QuadCircleRoom : GenShape
	{
		private int _radius;

		private int _distanceBetweenSpheres;

		public int Radius => _radius;

		public QuadCircleRoom(int radius, int distanceBetweenSpheres)
		{
			_radius = radius;
			_distanceBetweenSpheres = distanceBetweenSpheres;
		}

		public void SetRadius(int radius)
		{
			_radius = radius;
		}

		public override bool Perform(Point origin, GenAction action)
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_0072: Unknown result type (might be due to invalid IL or missing references)
			//IL_0077: Unknown result type (might be due to invalid IL or missing references)
			//IL_007c: Unknown result type (might be due to invalid IL or missing references)
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			//IL_008f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0096: Unknown result type (might be due to invalid IL or missing references)
			//IL_009b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0157: Unknown result type (might be due to invalid IL or missing references)
			//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0111: Unknown result type (might be due to invalid IL or missing references)
			//IL_0144: Unknown result type (might be due to invalid IL or missing references)
			//IL_0120: Unknown result type (might be due to invalid IL or missing references)
			int num = (_radius + 1) * (_radius + 1);
			Point val = origin;
			int num2 = 3;
			for (int i = 0; i < 5; i++)
			{
				val = i switch
				{
					1 => new Vector2((float)origin.X, (float)(origin.Y + _distanceBetweenSpheres - num2)).ToPoint(), 
					2 => new Vector2((float)(origin.X - _distanceBetweenSpheres + num2), (float)origin.Y).ToPoint(), 
					3 => new Vector2((float)(origin.X + _distanceBetweenSpheres - num2), (float)origin.Y).ToPoint(), 
					4 => origin, 
					_ => new Vector2((float)origin.X, (float)(origin.Y - _distanceBetweenSpheres + num2)).ToPoint(), 
				};
				for (int j = val.Y - _radius; j <= val.Y + _radius; j++)
				{
					double num3 = (double)_radius / (double)_radius * (double)(j - val.Y);
					int num4 = Math.Min(_radius, (int)Math.Sqrt((double)num - num3 * num3));
					for (int k = val.X - num4; k <= val.X + num4; k++)
					{
						if (!UnitApply(action, origin, k, j) && _quitOnFail)
						{
							return false;
						}
					}
				}
			}
			return true;
		}
	}
}
