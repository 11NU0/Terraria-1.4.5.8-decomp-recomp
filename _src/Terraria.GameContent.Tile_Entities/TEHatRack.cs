using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameInput;
using Terraria.UI;

namespace Terraria.GameContent.Tile_Entities;

public class TEHatRack : TileEntityType<TEHatRack>, IFixLoadedData
{
	private const int MyTileID = 475;

	public const int entityTileWidth = 3;

	public const int entityTileHeight = 4;

	private Player _dollPlayer;

	private Item[] _items;

	private Item[] _dyes;

	private static int hatTargetSlot;

	public TEHatRack()
	{
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		_items = new Item[2];
		for (int i = 0; i < _items.Length; i++)
		{
			_items[i] = new Item();
		}
		_dyes = new Item[2];
		for (int j = 0; j < _dyes.Length; j++)
		{
			_dyes[j] = new Item();
		}
		_dollPlayer = new Player();
		_dollPlayer.hair = 15;
		_dollPlayer.skinColor = Color.White;
		_dollPlayer.skinVariant = 10;
	}

	public static int Hook_AfterPlacement(int x, int y, int type = 475, int style = 0, int direction = 1, int alternate = 0)
	{
		if (Main.netMode == 1)
		{
			NetMessage.SendTileSquare(Main.myPlayer, x - 1, y - 3, 3, 4);
			NetMessage.SendData(87, -1, -1, null, x + -1, y + -3, (int)TileEntityType<TEHatRack>.EntityTypeID);
			return -1;
		}
		return TileEntityType<TEHatRack>.Place(x + -1, y + -3);
	}

	public override void WriteExtraData(BinaryWriter writer, bool networkSend)
	{
		BitsByte bitsByte = (byte)0;
		bitsByte[0] = !_items[0].IsAir;
		bitsByte[1] = !_items[1].IsAir;
		bitsByte[2] = !_dyes[0].IsAir;
		bitsByte[3] = !_dyes[1].IsAir;
		writer.Write(bitsByte);
		for (int i = 0; i < 2; i++)
		{
			Item item = _items[i];
			if (!item.IsAir)
			{
				writer.Write((short)item.type);
				writer.Write(item.prefix);
				writer.Write((short)item.stack);
			}
		}
		for (int j = 0; j < 2; j++)
		{
			Item item2 = _dyes[j];
			if (!item2.IsAir)
			{
				writer.Write((short)item2.type);
				writer.Write(item2.prefix);
				writer.Write((short)item2.stack);
			}
		}
	}

	public override void ReadExtraData(BinaryReader reader, int gameVersion, bool networkSend)
	{
		BitsByte bitsByte = reader.ReadByte();
		for (int i = 0; i < 2; i++)
		{
			_items[i] = new Item();
			Item item = _items[i];
			if (bitsByte[i])
			{
				item.netDefaults(reader.ReadInt16());
				item.Prefix(reader.ReadByte());
				item.stack = reader.ReadInt16();
			}
		}
		for (int j = 0; j < 2; j++)
		{
			_dyes[j] = new Item();
			Item item2 = _dyes[j];
			if (bitsByte[j + 2])
			{
				item2.netDefaults(reader.ReadInt16());
				item2.Prefix(reader.ReadByte());
				item2.stack = reader.ReadInt16();
			}
		}
	}

	public override string ToString()
	{
		return string.Concat(Position.X, "x  ", Position.Y, "y item: ", _items[0], " ", _items[1]);
	}

	public static void Framing_CheckTile(int callX, int callY)
	{
		if (WorldGen.destroyObject)
		{
			return;
		}
		int num = callX;
		int num2 = callY;
		Tile tileSafely = Framing.GetTileSafely(callX, callY);
		num -= tileSafely.frameX / 18 % 3;
		num2 -= tileSafely.frameY / 18 % 4;
		bool flag = false;
		for (int i = num; i < num + 3; i++)
		{
			for (int j = num2; j < num2 + 4; j++)
			{
				Tile tile = Main.tile[i, j];
				if (!tile.active() || tile.type != 475)
				{
					flag = true;
				}
			}
		}
		if (!WorldGen.SolidTileAllowBottomSlope(num, num2 + 4) || !WorldGen.SolidTileAllowBottomSlope(num + 1, num2 + 4) || !WorldGen.SolidTileAllowBottomSlope(num + 2, num2 + 4))
		{
			flag = true;
		}
		if (!flag)
		{
			return;
		}
		TileEntityType<TEHatRack>.Kill(num, num2);
		Item.NewItem(new EntitySource_TileBreak(num, num2), num * 16, num2 * 16, 48, 64, 3977);
		WorldGen.destroyObject = true;
		for (int k = num; k < num + 3; k++)
		{
			for (int l = num2; l < num2 + 4; l++)
			{
				if (Main.tile[k, l].active() && Main.tile[k, l].type == 475)
				{
					WorldGen.KillTile(k, l);
				}
			}
		}
		WorldGen.destroyObject = false;
	}

	public void Draw(int tileLeftX, int tileTopY)
	{
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0106: Unknown result type (might be due to invalid IL or missing references)
		//IL_018f: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_01af: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d2: Unknown result type (might be due to invalid IL or missing references)
		Player dollPlayer = _dollPlayer;
		dollPlayer.direction = -1;
		dollPlayer.Male = true;
		if (Framing.GetTileSafely(tileLeftX, tileTopY).frameX % 216 == 54)
		{
			dollPlayer.direction = 1;
		}
		dollPlayer.isDisplayDollOrInanimate = true;
		dollPlayer.isHatRackDoll = true;
		dollPlayer.armor[0] = _items[0];
		dollPlayer.dye[0] = _dyes[0];
		dollPlayer.ResetEffects();
		dollPlayer.ResetVisibleAccessories();
		dollPlayer.invis = true;
		dollPlayer.UpdateDyes();
		dollPlayer.DisplayDollUpdate();
		dollPlayer.PlayerFrame();
		Vector2 val = new Vector2((float)tileLeftX + 1.5f, (float)(tileTopY + 4)) * 16f;
		dollPlayer.direction *= -1;
		Vector2 val2 = new Vector2((float)(-dollPlayer.width / 2), (float)(-dollPlayer.height - 6)) + new Vector2((float)(dollPlayer.direction * 14), -2f);
		dollPlayer.position = val + val2;
		Main.PlayerRenderer.DrawPlayer(Main.Camera, dollPlayer, dollPlayer.position, 0f, dollPlayer.fullRotationOrigin);
		dollPlayer.armor[0] = _items[1];
		dollPlayer.dye[0] = _dyes[1];
		dollPlayer.ResetEffects();
		dollPlayer.ResetVisibleAccessories();
		dollPlayer.invis = true;
		dollPlayer.UpdateDyes();
		dollPlayer.DisplayDollUpdate();
		dollPlayer.skipAnimatingValuesInPlayerFrame = true;
		dollPlayer.PlayerFrame();
		dollPlayer.skipAnimatingValuesInPlayerFrame = false;
		dollPlayer.direction *= -1;
		val2 = new Vector2((float)(-dollPlayer.width / 2), (float)(-dollPlayer.height - 6)) + new Vector2((float)(dollPlayer.direction * 12), 16f);
		dollPlayer.position = val + val2;
		Main.PlayerRenderer.DrawPlayer(Main.Camera, dollPlayer, dollPlayer.position, 0f, dollPlayer.fullRotationOrigin);
	}

	public string GetItemGamepadInstructions(int slot = 0)
	{
		Item[] inv = _items;
		int num = slot;
		int context = 26;
		if (slot >= 2)
		{
			num -= 2;
			inv = _dyes;
			context = 27;
		}
		return ItemSlot.GetGamepadInstructions(inv, context, num);
	}

	public override void OnPlayerUpdate(Player player)
	{
		if (!player.InTileEntityInteractionRange(player.tileEntityAnchor.X, player.tileEntityAnchor.Y, 3, 4, TileReachCheckSettings.Simple) || player.chest != -1 || player.talkNPC != -1)
		{
			if (player.chest == -1 && player.talkNPC == -1)
			{
				SoundEngine.PlaySound(11);
			}
			player.tileEntityAnchor.Clear();
		}
	}

	public static void OnPlayerInteraction(Player player, int clickX, int clickY)
	{
		int num = clickX;
		int num2 = clickY;
		num -= Main.tile[num, num2].frameX % 54 / 18;
		num2 -= Main.tile[num, num2].frameY / 18;
		int num3 = TileEntityType<TEHatRack>.Find(num, num2);
		if (num3 != -1)
		{
			num2++;
			num++;
			hatTargetSlot = 0;
			TileEntity.BasicOpenCloseInteraction(player, num, num2, num3);
		}
	}

	public override void OnInventoryDraw(Player player, SpriteBatch spriteBatch)
	{
		if (Main.tile[player.tileEntityAnchor.X, player.tileEntityAnchor.Y].type != 475)
		{
			player.tileEntityAnchor.Clear();
		}
		else
		{
			DrawInner(player, spriteBatch);
		}
	}

	private void DrawInner(Player player, SpriteBatch spriteBatch)
	{
		Main.inventoryScale = 0.72f;
		DrawSlotPairSet(player, spriteBatch, 2, 0, 3.5f, 0.5f, 26);
	}

	private void DrawSlotPairSet(Player player, SpriteBatch spriteBatch, int slotsToShowLine, int slotsArrayOffset, float offsetX, float offsetY, int inventoryContextTarget)
	{
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		Item[] items = _items;
		int num = inventoryContextTarget;
		for (int i = 0; i < slotsToShowLine; i++)
		{
			for (int j = 0; j < 2; j++)
			{
				int num2 = (int)(73f + ((float)i + offsetX) * 56f * Main.inventoryScale);
				int num3 = (int)((float)Main.instance.invBottom + ((float)j + offsetY) * 56f * Main.inventoryScale);
				if (j == 0)
				{
					items = _items;
					num = inventoryContextTarget;
				}
				else
				{
					items = _dyes;
					num = 27;
				}
				if (Utils.FloatIntersect(Main.mouseX, Main.mouseY, 0f, 0f, num2, num3, (float)TextureAssets.InventoryBack.Width() * Main.inventoryScale, (float)TextureAssets.InventoryBack.Height() * Main.inventoryScale) && !PlayerInput.IgnoreMouseInterface)
				{
					player.mouseInterface = true;
					ItemSlot.Handle(items, num, i + slotsArrayOffset);
				}
				ItemSlot.Draw(spriteBatch, items, num, i + slotsArrayOffset, new Vector2((float)num2, (float)num3));
			}
		}
	}

	public override ItemSlot.AlternateClickAction? GetShiftClickAction(Item[] inv, int context = 0, int slot = 0)
	{
		Item item = inv[slot];
		if (context == 0 && CanQuickSwapIntoHatRack(item))
		{
			return ItemSlot.AlternateClickAction.TransferToChest;
		}
		if ((context == 26 || context == 27) && Main.LocalPlayer.ItemSpace(item).CanTakeItemToPersonalInventory)
		{
			return ItemSlot.AlternateClickAction.TransferFromChest;
		}
		return null;
	}

	public override bool PerformShiftClickAction(Item[] inv, int context = 0, int slot = 0)
	{
		Item item = inv[slot];
		if (Main.cursorOverride == 9 && context == 0)
		{
			if (Main.cursorOverride == 9 && !item.IsAir && !item.favorited && context == 0 && CanQuickSwapIntoHatRack(item))
			{
				return TryFitting(inv, slot);
			}
		}
		else if (Main.cursorOverride == 8 && (context == 26 || context == 27))
		{
			inv[slot] = Main.LocalPlayer.GetItem(inv[slot], GetItemSettings.QuickTransferFromSlot);
			if (Main.netMode == 1)
			{
				NetMessage.SendData(124, -1, -1, null, Main.myPlayer, ID, slot);
			}
			return true;
		}
		return false;
	}

	public static bool CanQuickSwapIntoHatRack(Item item)
	{
		return item.headSlot > 0;
	}

	private bool TryFitting(Item[] inv, int slot)
	{
		Item item = inv[slot];
		int num = -1;
		for (int i = 0; i < _items.Length; i++)
		{
			if (_items[i].IsAir)
			{
				num = i;
				hatTargetSlot = i;
				break;
			}
		}
		if (num == -1)
		{
			num = hatTargetSlot;
		}
		if (item.stack > 1 && !_items[num].IsAir)
		{
			return true;
		}
		SoundEngine.PlaySound(7);
		if (item.stack > 1)
		{
			_items[num] = item.Clone();
			_items[num].stack = 1;
			item.stack--;
		}
		else
		{
			Utils.Swap(ref _items[num], ref inv[slot]);
		}
		if (Main.netMode == 1)
		{
			NetMessage.SendData(124, -1, -1, null, Main.myPlayer, ID, num);
		}
		hatTargetSlot++;
		if (hatTargetSlot >= _items.Length)
		{
			hatTargetSlot = 0;
		}
		return true;
	}

	public void WriteItem(int itemIndex, BinaryWriter writer, bool dye)
	{
		Item item = _items[itemIndex];
		if (dye)
		{
			item = _dyes[itemIndex];
		}
		writer.Write((ushort)item.type);
		writer.Write((ushort)item.stack);
		writer.Write(item.prefix);
	}

	public void ReadItem(int itemIndex, BinaryReader reader, bool dye)
	{
		int num = reader.ReadUInt16();
		int stack = reader.ReadUInt16();
		int prefixWeWant = reader.ReadByte();
		Item item = _items[itemIndex];
		if (dye)
		{
			item = _dyes[itemIndex];
		}
		item.SetDefaults(num);
		item.stack = stack;
		item.Prefix(prefixWeWant);
	}

	public override bool IsTileValidForEntity(int x, int y)
	{
		if (!Main.tile[x, y].active() || Main.tile[x, y].type != 475 || Main.tile[x, y].frameY != 0 || Main.tile[x, y].frameX % 54 != 0)
		{
			return false;
		}
		return true;
	}

	public static bool IsBreakable(int clickX, int clickY)
	{
		int num = clickX;
		int num2 = clickY;
		num -= Main.tile[num, num2].frameX % 54 / 18;
		num2 -= Main.tile[num, num2].frameY / 18;
		if (TileEntity.TryGetAt<TEHatRack>(num, num2, out var result))
		{
			return !result.ContainsItems();
		}
		return true;
	}

	public bool ContainsItems()
	{
		for (int i = 0; i < 2; i++)
		{
			if (!_items[i].IsAir || !_dyes[i].IsAir)
			{
				return true;
			}
		}
		return false;
	}

	public void FixLoadedData()
	{
		Item[] items = _items;
		for (int i = 0; i < items.Length; i++)
		{
			items[i].FixAgainstExploit();
		}
		items = _dyes;
		for (int i = 0; i < items.Length; i++)
		{
			items[i].FixAgainstExploit();
		}
	}
}
