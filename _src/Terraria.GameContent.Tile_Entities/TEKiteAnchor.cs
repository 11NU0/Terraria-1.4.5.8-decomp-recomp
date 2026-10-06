using Terraria.DataStructures;
using Terraria.GameContent.LeashedEntities;
using Terraria.ID;

namespace Terraria.GameContent.Tile_Entities;

public class TEKiteAnchor : TELeashedEntityAnchorWithItem
{
	private static byte _myEntityID;

	public TEKiteAnchor()
	{
		type = _myEntityID;
	}

	public override void RegisterTileEntityID(int assignedID)
	{
		type = (_myEntityID = (byte)assignedID);
	}

	public override bool IsTileValidForEntity(int x, int y)
	{
		Tile tile = Main.tile[x, y];
		if (tile.active())
		{
			return tile.type == 723;
		}
		return false;
	}

	public override TileEntity GenerateInstance()
	{
		return new TEKiteAnchor();
	}

	public static void Kill(int x, int y)
	{
		TileEntity.Kill(x, y, _myEntityID);
	}

	public static int Hook_AfterPlacement(int x, int y, int type, int style, int direction, int alternate)
	{
		return TELeashedEntityAnchorWithItem.PlaceFromPlayerPlacementHook(x, y, _myEntityID);
	}

	public override bool FitsItem(int itemType)
	{
		return ItemID.Sets.IsAKite[itemType];
	}

	public override LeashedEntity CreateLeashedEntity()
	{
		if (itemType <= 0)
		{
			return null;
		}
		LeashedKite leashedKite = (LeashedKite)LeashedKite.Prototype.NewInstance();
		leashedKite.SetDefaults(ContentSamples.ItemsByType[itemType].shoot);
		return leashedKite;
	}

	public static void DebugPlace(int x, int y, int itemType)
	{
		int key = TileEntity.Place(x, y, _myEntityID);
		((TEKiteAnchor)TileEntity.ByID[key]).InsertItem(itemType);
		NetMessage.SendData(156, -1, -1, null, x, y, itemType);
	}
}
