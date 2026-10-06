using System;
using Microsoft.Xna.Framework;

namespace Terraria.Utilities;

public class BitSet2D
{
	private Point offset;

	private int size;

	private Bits64[] bits;

	public bool this[Point p]
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			int num = Coord(p);
			return bits[num >> 6][num & 0x3F];
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			int num = Coord(p);
			bits[num >> 6][num & 0x3F] = value;
		}
	}

	public void Reset(Point center, int maxDist)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		size = maxDist * 2 + 1;
		offset = new Point(center.X - maxDist, center.Y - maxDist);
		int num = size * size + 63 >> 6;
		if (bits == null || bits.Length < num)
		{
			Array.Resize(ref bits, num);
		}
		Array.Clear(bits, 0, bits.Length);
	}

	private int Coord(Point p)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		int num = p.X - offset.X;
		return (p.Y - offset.Y) * size + num;
	}

	public bool InBounds(Point p)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		int num = p.X - offset.X;
		int num2 = p.Y - offset.Y;
		if (num >= 0 && num < size && num2 >= 0)
		{
			return num2 < size;
		}
		return false;
	}

	public bool Add(Point p)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		int num = Coord(p);
		if (bits[num >> 6][num & 0x3F])
		{
			return false;
		}
		bits[num >> 6][num & 0x3F] = true;
		return true;
	}

	public bool Remove(Point p)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		int num = Coord(p);
		if (!bits[num >> 6][num & 0x3F])
		{
			return false;
		}
		bits[num >> 6][num & 0x3F] = false;
		return true;
	}
}
