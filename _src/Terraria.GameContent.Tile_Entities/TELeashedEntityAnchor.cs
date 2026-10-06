using Terraria.DataStructures;

namespace Terraria.GameContent.Tile_Entities;

public abstract class TELeashedEntityAnchor : TileEntity
{
	private LeashedEntity leashedEntity;

	public override void NetPlaceEntityAttempt(int x, int y)
	{
		int number = TileEntity.Place(x, y, type);
		NetMessage.SendData(86, -1, -1, null, number, x, y);
	}

	public override void OnRemoved()
	{
		DespawnLeashedEntity();
	}

	protected static int PlaceFromPlayerPlacementHook(int x, int y, int type)
	{
		if (Main.netMode == 1)
		{
			NetMessage.SendTileSquare(Main.myPlayer, x, y);
			NetMessage.SendData(87, -1, -1, null, x, y, type);
			return -1;
		}
		return TileEntity.Place(x, y, type);
	}

	public override void OnWorldLoaded()
	{
		RespawnLeashedEntity();
	}

	protected void DespawnLeashedEntity()
	{
		if (leashedEntity != null)
		{
			leashedEntity.active = false;
		}
	}

	protected void RespawnLeashedEntity()
	{
		DespawnLeashedEntity();
		leashedEntity = CreateLeashedEntity();
		LeashedEntity.AddNewEntity(leashedEntity, Position);
	}

	public abstract LeashedEntity CreateLeashedEntity();
}
