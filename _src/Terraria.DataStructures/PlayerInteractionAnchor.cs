namespace Terraria.DataStructures;

public struct PlayerInteractionAnchor(int entityID, int x = -1, int y = -1)
{
	public int interactEntityID = entityID;

	public int X = x;

	public int Y = y;

	public bool InUse => interactEntityID != -1;

	public void Clear()
	{
		interactEntityID = -1;
		X = -1;
		Y = -1;
	}

	public void Set(int entityID, int x, int y)
	{
		interactEntityID = entityID;
		X = x;
		Y = y;
	}

	public bool IsInValidUseTileEntity()
	{
		return GetTileEntity() != null;
	}

	public TileEntity GetTileEntity()
	{
		TileEntity result = null;
		if (InUse)
		{
			TileEntity.TryGet<TileEntity>(interactEntityID, out result);
		}
		return result;
	}
}
