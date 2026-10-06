namespace Terraria.DataStructures;

public abstract class TileEntityType<T> : TileEntity where T : TileEntity, new()
{
	protected static byte EntityTypeID;

	public override void RegisterTileEntityID(int assignedID)
	{
		EntityTypeID = (byte)assignedID;
	}

	public override TileEntity GenerateInstance()
	{
		return new T();
	}

	public override void NetPlaceEntityAttempt(int x, int y)
	{
		int number = Place(x, y);
		NetMessage.SendData(86, -1, -1, null, number, x, y);
	}

	public static int Place(int x, int y)
	{
		return TileEntity.Place(x, y, EntityTypeID);
	}

	public static void Kill(int x, int y)
	{
		TileEntity.Kill(x, y, EntityTypeID);
	}

	public static int Find(int x, int y)
	{
		if (!TileEntity.TryGetAt<T>(x, y, out var result))
		{
			return -1;
		}
		return result.ID;
	}
}
