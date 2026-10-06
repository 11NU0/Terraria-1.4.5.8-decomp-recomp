using System;
using System.Runtime.InteropServices;
using Terraria.Testing;

namespace Terraria.DataStructures;

[StructLayout(LayoutKind.Explicit)]
public struct ProjectileKey : IEquatable<ProjectileKey>
{
	[FieldOffset(0)]
	private readonly uint bits;

	[FieldOffset(0)]
	private readonly float floatBits;

	public int Spawner => (int)(bits & 0xFF);

	public int Index => (int)((bits >> 8) & 0x3FF);

	public int Generation => (int)((bits >> 18) & 0x3FFF);

	public override string ToString()
	{
		return $"spawner:{Spawner}, index:{Index}, gen:{Generation}";
	}

	private static uint Pack(int spawner, int index, int generation)
	{
		Invariant.Assert(spawner >= 0 && spawner <= 255, "Index out of range 'spawner': {0}", spawner);
		Invariant.Assert(index >= 0 && index <= 1000, "Index out of range 'index': {0}", index);
		return (uint)((spawner & 0xFF) | ((index & 0x3FF) << 8) | ((generation & 0x3FFF) << 18));
	}

	private ProjectileKey(uint bits)
	{
		this = default;
		this.bits = bits;
	}

	private ProjectileKey(float bits)
	{
		this = default;
		floatBits = bits;
	}

	public ProjectileKey(int spawner, int index, int generation)
		: this(Pack(spawner, index, generation))
	{
	}

	public static implicit operator uint(ProjectileKey p)
	{
		return p.bits;
	}

	public static implicit operator int(ProjectileKey p)
	{
		return (int)p.bits;
	}

	public static implicit operator float(ProjectileKey p)
	{
		return p.floatBits;
	}

	public static explicit operator ProjectileKey(int i)
	{
		return new ProjectileKey((uint)i);
	}

	public static explicit operator ProjectileKey(uint u)
	{
		return new ProjectileKey(u);
	}

	public static explicit operator ProjectileKey(float f)
	{
		return new ProjectileKey(f);
	}

	public bool Equals(ProjectileKey other)
	{
		return (int)this == (int)other;
	}

	public bool TryGet(out Projectile proj)
	{
		return Projectile.TryLookup(this, out proj);
	}

	public bool TryGetActive(out Projectile proj)
	{
		if (Projectile.TryLookup(this, out proj))
		{
			return proj.active;
		}
		return false;
	}

	public bool TryGetActive(int expectedType, out Projectile proj)
	{
		if (!TryGetActive(out proj))
		{
			return false;
		}
		Invariant.Assert(proj.type == expectedType, "Expected Type: {0}, got: {1}", expectedType, proj.type);
		return proj.type == expectedType;
	}

	public bool TryGetActive(bool[] expectedTypes, out Projectile proj)
	{
		if (!TryGetActive(out proj))
		{
			return false;
		}
		Invariant.Assert(proj.type >= 0 && expectedTypes[proj.type], "Expected Type invalid for bool array: {0}", proj.type);
		if (proj.type >= 0)
		{
			return expectedTypes[proj.type];
		}
		return false;
	}
}
