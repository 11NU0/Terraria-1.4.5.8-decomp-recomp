using System;
using Microsoft.Xna.Framework;

namespace Terraria.DataStructures;

public struct TileReachCheckSettings
{
	public int TileRangeMultiplier;

	public int? TileReachLimit;

	public int? OverrideXReach;

	public int? OverrideYReach;

	public static readonly TileReachCheckSettings Simple = new TileReachCheckSettings
	{
		TileRangeMultiplier = 1,
		TileReachLimit = 20
	};

	public static readonly TileReachCheckSettings Pylons = new TileReachCheckSettings
	{
		OverrideXReach = 60,
		OverrideYReach = 60
	};

	public void GetRanges(out int x, out int y)
	{
		x = Player.tileRangeX * TileRangeMultiplier;
		y = Player.tileRangeY * TileRangeMultiplier;
		if (TileReachLimit.HasValue)
		{
			if (x > TileReachLimit.Value)
			{
				x = TileReachLimit.Value;
			}
			if (y > TileReachLimit.Value)
			{
				y = TileReachLimit.Value;
			}
		}
		if (OverrideXReach.HasValue)
		{
			x = OverrideXReach.Value;
		}
		if (OverrideYReach.HasValue)
		{
			y = OverrideYReach.Value;
		}
	}

	public void GetTileRegion(Player player, out int LX, out int LY, out int HX, out int HY, int TB = 0)
	{
		GetRanges(out var x, out var y);
		x += TB;
		y += TB;
		LX = (int)(player.position.X / 16f) - x;
		HX = (int)Math.Ceiling((player.position.X + (float)player.width) / 16f) - 1 + x;
		LY = (int)(player.position.Y / 16f) - y;
		HY = (int)Math.Ceiling((player.position.Y + (float)player.height) / 16f) - 1 + y;
	}

	public Rectangle GetTileRegion(Player player, int TB = 0)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		GetTileRegion(player, out var LX, out var LY, out var HX, out var HY, TB);
		return new Rectangle(LX, LY, HX - LX, HY - LY);
	}

	public void GetWorldRegion(Player player, out int LX, out int LY, out int HX, out int HY, int TB = 0)
	{
		GetTileRegion(player, out LX, out LY, out HX, out HY, TB);
		LX *= 16;
		LY *= 16;
		HX *= 16;
		HY *= 16;
		HX += 15;
		HY += 15;
	}

	public Rectangle GetWorldRegion(Player player, int TB = 0)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		GetWorldRegion(player, out var LX, out var LY, out var HX, out var HY, TB);
		return new Rectangle(LX, LY, HX - LX, HY - LY);
	}
}
