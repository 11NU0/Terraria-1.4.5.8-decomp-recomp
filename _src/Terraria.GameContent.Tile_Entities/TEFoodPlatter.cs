using System.IO;
using Terraria.DataStructures;
using Terraria.ID;

namespace Terraria.GameContent.Tile_Entities;

public class TEFoodPlatter : TileEntityType<TEFoodPlatter>, IFixLoadedData
{
	public Item item;

	public TEFoodPlatter()
	{
		item = new Item();
	}

	public override bool IsTileValidForEntity(int x, int y)
	{
		return ValidTile(x, y);
	}

	public static int Hook_AfterPlacement(int x, int y, int type = 520, int style = 0, int direction = 1, int alternate = 0)
	{
		if (Main.netMode == 1)
		{
			NetMessage.SendTileSquare(Main.myPlayer, x, y);
			NetMessage.SendData(87, -1, -1, null, x, y, (int)TileEntityType<TEFoodPlatter>.EntityTypeID);
			return -1;
		}
		return TileEntityType<TEFoodPlatter>.Place(x, y);
	}

	public static bool ValidTile(int x, int y)
	{
		if (!Main.tile[x, y].active() || Main.tile[x, y].type != 520 || Main.tile[x, y].frameY != 0)
		{
			return false;
		}
		return true;
	}

	public override void WriteExtraData(BinaryWriter writer, bool networkSend)
	{
		writer.Write((short)item.type);
		writer.Write(item.prefix);
		writer.Write((short)item.stack);
	}

	public override void ReadExtraData(BinaryReader reader, int gameVersion, bool networkSend)
	{
		item = new Item();
		item.netDefaults(reader.ReadInt16());
		item.Prefix(reader.ReadByte());
		item.stack = reader.ReadInt16();
	}

	public override string ToString()
	{
		return Position.X + "x  " + Position.Y + "y item: " + item;
	}

	public void DropItem()
	{
		if (Main.netMode != 1)
		{
			Item.NewItem(new EntitySource_TileBreak(Position.X, Position.Y), Position.X * 16, Position.Y * 16, 16, 16, item.type, 1, noBroadcast: false, item.prefix);
		}
		item = new Item();
	}

	public static void TryPlacing(int x, int y, int type, int prefix, int stack)
	{
		WorldGen.RangeFrame(x, y, x + 1, y + 1);
		if (!TileEntity.TryGetAt<TEFoodPlatter>(x, y, out var result))
		{
			Item.NewItem(new EntitySource_TileBreak(x, y), x * 16, y * 16, 16, 16, type, stack, noBroadcast: false, prefix);
			return;
		}
		if (result.item.stack > 0)
		{
			result.DropItem();
		}
		result.item = new Item();
		result.item.SetDefaults(type);
		result.item.Prefix(prefix);
		result.item.stack = stack;
		NetMessage.SendData(86, -1, -1, null, result.ID, x, y);
	}

	public static void OnPlayerInteraction(Player player, int clickX, int clickY)
	{
		TEFoodPlatter result;
		if (FitsFoodPlatter(player.inventory[player.selectedItem]) && !player.inventory[player.selectedItem].favorited)
		{
			player.GamepadEnableGrappleCooldown();
			PlaceItemInFrame(player, clickX, clickY);
		}
		else if (TileEntity.TryGetAt<TEFoodPlatter>(clickX, clickY, out result) && result.item.stack > 0)
		{
			player.GamepadEnableGrappleCooldown();
			WorldGen.KillTile(clickX, clickY, fail: true);
			if (Main.netMode == 1)
			{
				NetMessage.SendData(17, -1, -1, null, 0, clickX, clickY, 1f);
			}
		}
	}

	public static bool FitsFoodPlatter(Item i)
	{
		if (i.stack > 0)
		{
			return ItemID.Sets.IsFood[i.type];
		}
		return false;
	}

	public static void PlaceItemInFrame(Player player, int x, int y)
	{
		if (!player.ItemTimeIsZero || !TileEntity.TryGetAt<TEFoodPlatter>(x, y, out var result))
		{
			return;
		}
		if (result.item.stack > 0)
		{
			WorldGen.KillTile(x, y, fail: true);
			if (Main.netMode == 1)
			{
				NetMessage.SendData(17, -1, -1, null, 0, Player.tileTargetX, y, 1f);
			}
		}
		if (Main.netMode == 1)
		{
			NetMessage.SendData(133, -1, -1, null, x, y, player.selectedItem, player.whoAmI, 1);
		}
		else
		{
			TryPlacing(x, y, player.inventory[player.selectedItem].type, player.inventory[player.selectedItem].prefix, 1);
		}
		player.inventory[player.selectedItem].stack--;
		if (player.inventory[player.selectedItem].stack <= 0)
		{
			player.inventory[player.selectedItem].SetDefaults(0);
			Main.mouseItem.SetDefaults(0);
		}
		if (player.selectedItem == 58)
		{
			Main.mouseItem = player.inventory[player.selectedItem].Clone();
		}
		player.releaseUseItem = false;
		player.mouseInterface = true;
		WorldGen.RangeFrame(x, y, x + 1, y + 1);
	}

	public void FixLoadedData()
	{
		item.FixAgainstExploit();
	}
}
