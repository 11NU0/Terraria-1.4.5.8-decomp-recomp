using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using ReLogic.Utilities;
using Terraria.Utilities;

namespace Terraria.GameContent.Generation.Dungeon;

public class DungeonBounds
{
	[JsonProperty]
	private Rectangle? _hitbox;

	private int _boundsLeft;

	private int _boundsRight;

	private int _boundsTop;

	private int _boundsBottom;

	public Rectangle Hitbox
	{
		get
		{
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			if (_hitbox.HasValue)
			{
				return _hitbox.Value;
			}
			return Rectangle.Empty;
		}
	}

	public int X => _boundsLeft;

	public int Y => _boundsTop;

	public int Width => _boundsRight - _boundsLeft;

	public int Height => _boundsBottom - _boundsTop;

	public int Size
	{
		get
		{
			if (Width <= Height)
			{
				return Height;
			}
			return Width;
		}
	}

	public int Left
	{
		get
		{
			return _boundsLeft;
		}
		set
		{
			_boundsLeft = (int)MathHelper.Clamp((float)value, 10f, (float)(Main.maxTilesX - 10));
		}
	}

	public int Right
	{
		get
		{
			return _boundsRight;
		}
		set
		{
			_boundsRight = (int)MathHelper.Clamp((float)value, 10f, (float)(Main.maxTilesX - 10));
		}
	}

	public int Top
	{
		get
		{
			return _boundsTop;
		}
		set
		{
			_boundsTop = (int)MathHelper.Clamp((float)value, 10f, (float)(Main.maxTilesY - 10));
		}
	}

	public int Bottom
	{
		get
		{
			return _boundsBottom;
		}
		set
		{
			_boundsBottom = (int)MathHelper.Clamp((float)value, 10f, (float)(Main.maxTilesY - 10));
		}
	}

	public Point Center
	{
		get
		{
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			return new Point((Left + Right) / 2, (Top + Bottom) / 2);
		}
	}

	public Point RandomPointInBounds(UnifiedRandom genRand)
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		return new Point(genRand.Next(Left, Right + 1), genRand.Next(Top, Bottom + 1));
	}

	public void Inflate(int amount)
	{
		SetBounds(Left - amount, Top - amount, Right + amount, Bottom + amount);
	}

	public void Shrink(int amount)
	{
		SetBounds(Left + amount, Top + amount, Right - amount, Bottom - amount);
	}

	public bool ContainsWithFluff(Vector2 point, int fluff)
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0004: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		if (fluff == 0)
		{
			return Contains((int)point.X, (int)point.Y);
		}
		return ContainsWithFluff((int)point.X, (int)point.Y, fluff);
	}

	public bool ContainsWithFluff(Vector2D point, int fluff)
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0004: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		if (fluff == 0)
		{
			return Contains((int)point.X, (int)point.Y);
		}
		return ContainsWithFluff((int)point.X, (int)point.Y, fluff);
	}

	public bool ContainsWithFluff(Point point, int fluff)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0004: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		if (fluff == 0)
		{
			return Contains(point.X, point.Y);
		}
		return ContainsWithFluff(point.X, point.Y, fluff);
	}

	public bool ContainsWithFluff(int x, int y, int fluff)
	{
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		if (fluff == 0)
		{
			return Contains(x, y);
		}
		if (!_hitbox.HasValue)
		{
			return false;
		}
		Rectangle value = _hitbox.Value;
		int num = value.Left - fluff;
		value = _hitbox.Value;
		Rectangle val = new Rectangle(num, value.Top - fluff, _hitbox.Value.Width + fluff * 2, _hitbox.Value.Height + fluff * 2);
		return val.Contains(x, y);
	}

	public bool Contains(Vector2D point)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		return Contains((int)point.X, (int)point.Y);
	}

	public bool Contains(Point point)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		return Contains(point.X, point.Y);
	}

	public bool Contains(int x, int y)
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		if (!_hitbox.HasValue)
		{
			return false;
		}
		Rectangle value = _hitbox.Value;
		return value.Contains(x, y);
	}

	public bool Intersects(DungeonBounds bounds)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		if (!bounds.HasHitbox())
		{
			return false;
		}
		return Intersects(bounds.Hitbox);
	}

	public bool Intersects(Rectangle hitbox)
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		if (!_hitbox.HasValue)
		{
			return false;
		}
		Rectangle value = _hitbox.Value;
		return value.Intersects(hitbox);
	}

	public bool IntersectsWithLineThreePointCheck(Point startPoint, Point endPoint)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		return IntersectsWithLineThreePointCheck(startPoint.ToVector2D(), endPoint.ToVector2D());
	}

	public bool IntersectsWithLineThreePointCheck(int startPointX, int startPointY, int endPointX, int endPointY)
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		return IntersectsWithLineThreePointCheck(new Vector2D((double)startPointX, (double)startPointY), new Vector2D((double)endPointX, (double)endPointY));
	}

	public bool IntersectsWithLineThreePointCheck(Vector2D startPoint, Vector2D endPoint)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		if (!_hitbox.HasValue)
		{
			return false;
		}
		if (Contains(startPoint) || Contains(endPoint) || Contains((startPoint + endPoint) / 2.0))
		{
			return true;
		}
		return false;
	}

	public bool HasHitbox()
	{
		return _hitbox.HasValue;
	}

	public void SetBoundsLeft(int minX)
	{
		Left = minX;
	}

	public void SetBoundsRight(int maxX)
	{
		Right = maxX;
	}

	public void SetBoundsTop(int minY)
	{
		Top = minY;
	}

	public void SetBoundsBottom(int maxY)
	{
		Bottom = maxY;
	}

	public void SetBounds(Rectangle rect)
	{
		SetBounds(rect.Left, rect.Top, rect.Right, rect.Bottom);
	}

	public void SetBounds(int minX, int minY, int maxX, int maxY)
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		Left = minX;
		Right = maxX;
		Top = minY;
		Bottom = maxY;
		CalculateHitbox();
	}

	public void UpdateBounds(int x, int y)
	{
		if (x < _boundsLeft)
		{
			Left = x;
		}
		if (x > _boundsRight)
		{
			Right = x;
		}
		if (y < _boundsTop)
		{
			Top = y;
		}
		if (y > _boundsBottom)
		{
			Bottom = y;
		}
	}

	public void UpdateBounds(DungeonBounds bounds)
	{
		if (Width == 0 || Height == 0)
		{
			SetBounds(bounds.Left, bounds.Top, bounds.Right, bounds.Bottom);
		}
		else
		{
			UpdateBounds(bounds.Left, bounds.Top, bounds.Right, bounds.Bottom);
		}
	}

	public void UpdateBounds(int minX, int minY, int maxX, int maxY)
	{
		if (minX < _boundsLeft)
		{
			Left = minX;
		}
		if (maxX > _boundsRight)
		{
			Right = maxX;
		}
		if (minY < _boundsTop)
		{
			Top = minY;
		}
		if (maxY > _boundsBottom)
		{
			Bottom = maxY;
		}
	}

	public Rectangle CalculateHitbox()
	{
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		if (Right <= Left)
		{
			Right = Left + 1;
		}
		if (Bottom <= Top)
		{
			Bottom = Top + 1;
		}
		_hitbox = new Rectangle(X, Y, Width, Height);
		return _hitbox.Value;
	}

	public void Reset()
	{
		_hitbox = null;
		Left = 0;
		Right = 0;
		Top = 0;
		Bottom = 0;
	}

	public override string ToString()
	{
		return $"Bounds: {Left}, {Top}, {Right}, {Bottom} | {Width} x {Height}";
	}
}
