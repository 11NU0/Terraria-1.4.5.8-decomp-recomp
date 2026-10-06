using System;
using Microsoft.Xna.Framework;

namespace Terraria.GameContent.UI;

public class WorldUIAnchor
{
	public enum AnchorType
	{
		Entity,
		Tile,
		Pos,
		None
	}

	public AnchorType type;

	public Entity entity;

	public Vector2 pos = Vector2.Zero;

	public Vector2 size = Vector2.Zero;

	public WorldUIAnchor()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		type = AnchorType.None;
	}

	public WorldUIAnchor(Entity anchor)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		type = AnchorType.Entity;
		entity = anchor;
	}

	public WorldUIAnchor(Vector2 anchor)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		type = AnchorType.Pos;
		pos = anchor;
	}

	public WorldUIAnchor(int topLeftX, int topLeftY, int width, int height)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		type = AnchorType.Tile;
		pos = new Vector2((float)topLeftX + (float)width / 2f, (float)topLeftY + (float)height / 2f) * 16f;
		size = new Vector2((float)width, (float)height) * 16f;
	}

	public bool InRange(Vector2 target, float tileRangeX, float tileRangeY)
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_010c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		switch (type)
		{
		case AnchorType.Entity:
			if (Math.Abs(target.X - entity.Center.X) <= tileRangeX * 16f + (float)entity.width / 2f)
			{
				return Math.Abs(target.Y - entity.Center.Y) <= tileRangeY * 16f + (float)entity.height / 2f;
			}
			return false;
		case AnchorType.Pos:
			if (Math.Abs(target.X - pos.X) <= tileRangeX * 16f)
			{
				return Math.Abs(target.Y - pos.Y) <= tileRangeY * 16f;
			}
			return false;
		case AnchorType.Tile:
			if (Math.Abs(target.X - pos.X) <= tileRangeX * 16f + size.X / 2f)
			{
				return Math.Abs(target.Y - pos.Y) <= tileRangeY * 16f + size.Y / 2f;
			}
			return false;
		default:
			return true;
		}
	}
}
