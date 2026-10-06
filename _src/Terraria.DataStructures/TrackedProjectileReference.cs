using System.IO;

namespace Terraria.DataStructures;

public struct TrackedProjectileReference
{
	public ProjectileKey Key { get; private set; }

	public int ProjectileType { get; private set; }

	public void Set(Projectile proj)
	{
		Key = proj.key;
		ProjectileType = proj.type;
	}

	public void Clear()
	{
		this = default;
	}

	public void Write(BinaryWriter writer)
	{
		writer.Write(Key);
		writer.Write((short)ProjectileType);
	}

	public bool IsTracking(Projectile proj)
	{
		return (int)proj.key == (int)Key;
	}

	public void Read(BinaryReader reader)
	{
		Key = (ProjectileKey)reader.ReadInt32();
		ProjectileType = reader.ReadInt16();
	}

	public override bool Equals(object obj)
	{
		if (!(obj is TrackedProjectileReference other))
		{
			return false;
		}
		return Equals(other);
	}

	public bool Equals(TrackedProjectileReference other)
	{
		if ((int)Key == (int)other.Key)
		{
			return ProjectileType == other.ProjectileType;
		}
		return false;
	}

	public override int GetHashCode()
	{
		return Key.GetHashCode();
	}

	public static bool operator ==(TrackedProjectileReference c1, TrackedProjectileReference c2)
	{
		return c1.Equals(c2);
	}

	public static bool operator !=(TrackedProjectileReference c1, TrackedProjectileReference c2)
	{
		return !c1.Equals(c2);
	}
}
