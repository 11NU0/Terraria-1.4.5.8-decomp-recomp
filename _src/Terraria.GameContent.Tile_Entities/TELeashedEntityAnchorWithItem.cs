using System.IO;
using Terraria.DataStructures;

namespace Terraria.GameContent.Tile_Entities;

public abstract class TELeashedEntityAnchorWithItem : TELeashedEntityAnchor
{
	protected int itemType;

	public override void WriteExtraData(BinaryWriter writer, bool networkSend)
	{
		writer.Write((short)itemType);
	}

	public override void ReadExtraData(BinaryReader reader, int gameVersion, bool networkSend)
	{
		itemType = reader.ReadInt16();
	}

	public void DropItemForTileBreak()
	{
		if (itemType > 0)
		{
			if (Main.netMode != 1)
			{
				Item.NewItem(new EntitySource_TileBreak(Position.X, Position.Y), Position.X * 16, Position.Y * 16, 16, 16, itemType);
			}
			itemType = 0;
		}
	}

	public void InsertItem(int itemType)
	{
		this.itemType = itemType;
		RespawnLeashedEntity();
	}

	public override void OnWorldLoaded()
	{
		if (!FitsItem(itemType))
		{
			itemType = 0;
		}
		base.OnWorldLoaded();
	}

	public abstract bool FitsItem(int itemType);

	protected new static int PlaceFromPlayerPlacementHook(int x, int y, int type)
	{
		int num = TELeashedEntityAnchor.PlaceFromPlayerPlacementHook(x, y, type);
		Item heldItem = Main.LocalPlayer.HeldItem;
		int num2 = heldItem.type;
		if (!heldItem.consumable && --heldItem.stack <= 0)
		{
			heldItem.TurnToAir();
		}
		if (Main.netMode == 1)
		{
			NetMessage.SendData(156, -1, -1, null, x, y, num2);
		}
		else
		{
			((TELeashedEntityAnchorWithItem)TileEntity.ByID[num]).InsertItem(num2);
		}
		return num;
	}
}
