using System;
using Microsoft.Xna.Framework;

namespace Terraria;

public abstract class Entity : IEntitySourceTarget
{
	public int whoAmI;

	public Vector2 position;

	public Vector2 velocity;

	public Vector2 oldPosition;

	public Vector2 oldVelocity;

	public int oldDirection;

	public int direction = 1;

	public int width;

	public int height;

	public bool wet;

	public bool shimmerWet;

	public bool honeyWet;

	public byte wetCount;

	public bool lavaWet;

	public bool AnyWet
	{
		get
		{
			if (!wet && !lavaWet && !honeyWet)
			{
				return shimmerWet;
			}
			return true;
		}
	}

	public virtual Vector2 VisualPosition
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return position;
		}
	}

	public Vector2 Center
	{
		get
		{
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			return new Vector2(position.X + (float)width / 2f, position.Y + (float)height / 2f);
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			position = new Vector2(value.X - (float)width / 2f, value.Y - (float)height / 2f);
		}
	}

	public Vector2 Left
	{
		get
		{
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			return new Vector2(position.X, position.Y + (float)height / 2f);
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			position = new Vector2(value.X, value.Y - (float)height / 2f);
		}
	}

	public Vector2 Right
	{
		get
		{
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			return new Vector2(position.X + (float)width, position.Y + (float)height / 2f);
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			position = new Vector2(value.X - (float)width, value.Y - (float)height / 2f);
		}
	}

	public Vector2 Top
	{
		get
		{
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			return new Vector2(position.X + (float)width / 2f, position.Y);
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			position = new Vector2(value.X - (float)width / 2f, value.Y);
		}
	}

	public Vector2 TopLeft
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return position;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			position = value;
		}
	}

	public Vector2 TopRight
	{
		get
		{
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			return new Vector2(position.X + (float)width, position.Y);
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			position = new Vector2(value.X - (float)width, value.Y);
		}
	}

	public Vector2 Bottom
	{
		get
		{
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			return new Vector2(position.X + (float)width / 2f, position.Y + (float)height);
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			position = new Vector2(value.X - (float)width / 2f, value.Y - (float)height);
		}
	}

	public Vector2 BottomLeft
	{
		get
		{
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			return new Vector2(position.X, position.Y + (float)height);
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			position = new Vector2(value.X, value.Y - (float)height);
		}
	}

	public Vector2 BottomRight
	{
		get
		{
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			return new Vector2(position.X + (float)width, position.Y + (float)height);
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			position = new Vector2(value.X - (float)width, value.Y - (float)height);
		}
	}

	public Vector2 Size
	{
		get
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			return new Vector2((float)width, (float)height);
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			width = (int)value.X;
			height = (int)value.Y;
		}
	}

	public Rectangle Hitbox
	{
		get
		{
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			return new Rectangle((int)position.X, (int)position.Y, width, height);
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			position = new Vector2((float)value.X, (float)value.Y);
			width = value.Width;
			height = value.Height;
		}
	}

	public float AngleTo(Vector2 Destination)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		return (float)Math.Atan2(Destination.Y - Center.Y, Destination.X - Center.X);
	}

	public float AngleFrom(Vector2 Source)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		return (float)Math.Atan2(Center.Y - Source.Y, Center.X - Source.X);
	}

	public float Distance(Vector2 Other)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		return Vector2.Distance(Center, Other);
	}

	public float DistanceSQ(Vector2 Other)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		return Vector2.DistanceSquared(Center, Other);
	}

	public Vector2 DirectionTo(Vector2 Destination)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		return Vector2.Normalize(Destination - Center);
	}

	public Vector2 DirectionFrom(Vector2 Source)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		return Vector2.Normalize(Center - Source);
	}

	public bool WithinRange(Vector2 Target, float MaxRange)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		return Vector2.DistanceSquared(Center, Target) <= MaxRange * MaxRange;
	}
}
