using System.IO;
using Terraria.DataStructures;

namespace Terraria.GameContent.Tile_Entities;

public class TEDeadCellsDisplayJar : TileEntityType<TEDeadCellsDisplayJar>, IFixLoadedData
{
	public Item item;

	public TEDeadCellsDisplayJar()
	{
		item = new Item();
	}

	public override bool IsTileValidForEntity(int x, int y)
	{
		return ValidTile(x, y);
	}

	public static int Hook_AfterPlacement(int x, int y, int type = 698, int style = 0, int direction = 1, int alternate = 0)
	{
		if (Main.netMode == 1)
		{
			NetMessage.SendTileSquare(Main.myPlayer, x, y, 2, 2);
			NetMessage.SendData(87, -1, -1, null, x, y, (int)TileEntityType<TEDeadCellsDisplayJar>.EntityTypeID);
			return -1;
		}
		return TileEntityType<TEDeadCellsDisplayJar>.Place(x, y);
	}

	public static bool ValidTile(int x, int y)
	{
		if (!Main.tile[x, y].active() || Main.tile[x, y].type != 698 || Main.tile[x, y].frameY != 0 || Main.tile[x, y].frameX % 18 != 0)
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
			Item.NewItem(new EntitySource_TileBreak(Position.X, Position.Y), Position.X * 16, Position.Y * 16, 32, 32, item.type, 1, noBroadcast: false, item.prefix);
		}
		item = new Item();
	}

	public static void TryPlacing(int x, int y, int type, int prefix, int stack)
	{
		WorldGen.RangeFrame(x, y, x + 1, y + 2);
		if (!TileEntity.TryGetAt<TEDeadCellsDisplayJar>(x, y, out var result))
		{
			Item.NewItem(new EntitySource_TileBreak(x, y), x * 16, y * 16, 32, 32, type, stack, noBroadcast: false, prefix);
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
		if (FitsJar(player.inventory[player.selectedItem]) && !player.inventory[player.selectedItem].favorited)
		{
			player.GamepadEnableGrappleCooldown();
			PlaceItemInJar(player, clickX, clickY);
			return;
		}
		int num = clickX;
		int num2 = clickY;
		if (Main.tile[num, num2].frameX % 18 != 0)
		{
			num--;
		}
		if (Main.tile[num, num2].frameY % 36 != 0)
		{
			num2--;
		}
		if (TileEntity.TryGetAt<TEDeadCellsDisplayJar>(num, num2, out var result) && result.item.stack > 0)
		{
			player.GamepadEnableGrappleCooldown();
			WorldGen.KillTile(clickX, clickY, fail: true);
			if (Main.netMode == 1)
			{
				NetMessage.SendData(17, -1, -1, null, 0, num, num2, 1f);
			}
		}
	}

	public static bool FitsJar(Item i)
	{
		return i.stack > 0;
	}

	public static void PlaceItemInJar(Player player, int x, int y)
	{
		if (!player.ItemTimeIsZero)
		{
			return;
		}
		if (Main.tile[x, y].frameX % 18 != 0)
		{
			x--;
		}
		if (Main.tile[x, y].frameY % 36 != 0)
		{
			y--;
		}
		if (!TileEntity.TryGetAt<TEDeadCellsDisplayJar>(x, y, out var result))
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
			NetMessage.SendData(149, -1, -1, null, x, y, player.selectedItem, player.whoAmI, 1);
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
		player.PlayDroppedItemAnimation(20);
		WorldGen.RangeFrame(x, y, x + 1, y + 2);
	}

	public void FixLoadedData()
	{
		item.FixAgainstExploit();
	}
}
