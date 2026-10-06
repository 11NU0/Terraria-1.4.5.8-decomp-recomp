using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameInput;
using Terraria.Graphics.Renderers;
using Terraria.ID;
using Terraria.UI;

namespace Terraria.GameContent.Tile_Entities;

public class TEDisplayDoll : TileEntityType<TEDisplayDoll>, IFixLoadedData
{
	public struct DisplayDollPose
	{
		public DisplayDollPoseID Pose;

		public float ItemAnimationPercent;

		public float? ItemAimRadians;
	}

	private const int MyTileID = 470;

	public const int entityTileWidth = 2;

	public const int entityTileHeight = 3;

	private Player _dollPlayer;

	private Item[] _equip;

	private Item[] _dyes;

	private Item[] _misc;

	private byte _pose;

	public static Dictionary<int, List<DisplayDollPose>> SupportedUseStylePoses;

	private static Projectile _projectileDummy;

	private static LegacyPlayerRenderer _playerRenderer;

	public ArraySegment<Item> AccSlots => new ArraySegment<Item>(_equip, 3, 5);

	static TEDisplayDoll()
	{
		SupportedUseStylePoses = new Dictionary<int, List<DisplayDollPose>>();
		_projectileDummy = new Projectile();
		_playerRenderer = new LegacyPlayerRenderer();
		SupportedUseStylePoses.Clear();
		RegisterUsePose(1, DisplayDollPoseID.Use1, 1f);
		RegisterUsePose(1, DisplayDollPoseID.Use2, 0.8f);
		RegisterUsePose(1, DisplayDollPoseID.Use3, 0.6f);
		RegisterUsePose(1, DisplayDollPoseID.Use4, 0.4143f);
		RegisterUsePose(1, DisplayDollPoseID.Use5, 0.2f);
		RegisterUsePose(7, DisplayDollPoseID.Use1, 0.5f);
		RegisterUsePose(3, DisplayDollPoseID.Use1, 0.5f);
		RegisterUsePose(4, DisplayDollPoseID.Use1, 0.5f);
		RegisterUsePose(5, DisplayDollPoseID.Use1, 0.5f, -(float)Math.PI / 2f);
		RegisterUsePose(5, DisplayDollPoseID.Use2, 0.5f, -(float)Math.PI / 4f);
		RegisterUsePose(5, DisplayDollPoseID.Use3, 0.5f, 0f);
		RegisterUsePose(5, DisplayDollPoseID.Use4, 0.5f, 0.7853981f);
		RegisterUsePose(5, DisplayDollPoseID.Use5, 0.5f, (float)Math.PI / 2f);
		RegisterUsePose(6, DisplayDollPoseID.Use1, 0.5f);
		RegisterUsePose(2, DisplayDollPoseID.Use1, 0.5f);
		RegisterUsePose(8, DisplayDollPoseID.Use1, 0.5f);
		RegisterUsePose(9, DisplayDollPoseID.Use1, 0.5f);
		RegisterUsePose(11, DisplayDollPoseID.Use1, 0.5f);
		RegisterUsePose(12, DisplayDollPoseID.Use1, 0.75f);
		RegisterUsePose(12, DisplayDollPoseID.Use2, 0.5f);
		RegisterUsePose(12, DisplayDollPoseID.Use3, 0.25f);
		RegisterUsePose(13, DisplayDollPoseID.Use1, 0.5f, -(float)Math.PI / 2f);
		RegisterUsePose(13, DisplayDollPoseID.Use2, 0.5f, -(float)Math.PI / 4f);
		RegisterUsePose(13, DisplayDollPoseID.Use3, 0.5f, 0f);
		RegisterUsePose(13, DisplayDollPoseID.Use4, 0.5f, 0.7853981f);
		RegisterUsePose(13, DisplayDollPoseID.Use5, 0.5f, (float)Math.PI / 2f);
		RegisterUsePose(14, DisplayDollPoseID.Use1, 0.5f);
		RegisterUsePose(15, DisplayDollPoseID.Use1, 0.5f);
	}

	private static void RegisterUsePose(int useStyle, DisplayDollPoseID pose, float usePercent, float? useAim = null)
	{
		if (!SupportedUseStylePoses.TryGetValue(useStyle, out var value))
		{
			value = new List<DisplayDollPose>();
			SupportedUseStylePoses[useStyle] = value;
		}
		value.Add(new DisplayDollPose
		{
			Pose = pose,
			ItemAnimationPercent = usePercent,
			ItemAimRadians = useAim
		});
	}

	public TEDisplayDoll()
	{
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		_equip = new Item[9];
		for (int i = 0; i < _equip.Length; i++)
		{
			_equip[i] = new Item();
		}
		_dyes = new Item[9];
		for (int j = 0; j < _dyes.Length; j++)
		{
			_dyes[j] = new Item();
		}
		_misc = new Item[1];
		for (int k = 0; k < _misc.Length; k++)
		{
			_misc[k] = new Item();
		}
		_dollPlayer = new Player();
		_dollPlayer.hair = 15;
		_dollPlayer.skinColor = Color.White;
		_dollPlayer.skinVariant = 10;
	}

	public static int Hook_AfterPlacement(int x, int y, int type = 470, int style = 0, int direction = 1, int alternate = 0)
	{
		if (Main.netMode == 1)
		{
			NetMessage.SendTileSquare(Main.myPlayer, x, y - 2, 2, 3);
			NetMessage.SendData(87, -1, -1, null, x, y - 2, (int)TileEntityType<TEDisplayDoll>.EntityTypeID);
			return -1;
		}
		return TileEntityType<TEDisplayDoll>.Place(x, y - 2);
	}

	private bool IsValidPose(int testedPose)
	{
		bool flag = false;
		if ((uint)testedPose <= 3u)
		{
			flag = true;
		}
		Item item = _misc[0];
		if (!flag && item != null && !item.IsAir && SupportedUseStylePoses.TryGetValue(item.useStyle, out var value))
		{
			foreach (DisplayDollPose item2 in value)
			{
				if ((DisplayDollPoseID)_pose == item2.Pose)
				{
					flag = true;
					break;
				}
			}
		}
		return flag;
	}

	public override void WriteExtraData(BinaryWriter writer, bool networkSend)
	{
		BitsByte bitsByte = (byte)0;
		bitsByte[0] = !_equip[0].IsAir;
		bitsByte[1] = !_equip[1].IsAir;
		bitsByte[2] = !_equip[2].IsAir;
		bitsByte[3] = !_equip[3].IsAir;
		bitsByte[4] = !_equip[4].IsAir;
		bitsByte[5] = !_equip[5].IsAir;
		bitsByte[6] = !_equip[6].IsAir;
		bitsByte[7] = !_equip[7].IsAir;
		BitsByte bitsByte2 = (byte)0;
		bitsByte2[0] = !_dyes[0].IsAir;
		bitsByte2[1] = !_dyes[1].IsAir;
		bitsByte2[2] = !_dyes[2].IsAir;
		bitsByte2[3] = !_dyes[3].IsAir;
		bitsByte2[4] = !_dyes[4].IsAir;
		bitsByte2[5] = !_dyes[5].IsAir;
		bitsByte2[6] = !_dyes[6].IsAir;
		bitsByte2[7] = !_dyes[7].IsAir;
		BitsByte bitsByte3 = (byte)0;
		bitsByte3[0] = !_misc[0].IsAir;
		bitsByte3[1] = !_equip[8].IsAir;
		bitsByte3[2] = !_dyes[8].IsAir;
		writer.Write(bitsByte);
		writer.Write(bitsByte2);
		writer.Write(_pose);
		writer.Write(bitsByte3);
		Item[] equip = _equip;
		foreach (Item item in equip)
		{
			if (!item.IsAir)
			{
				writer.Write((short)item.type);
				writer.Write(item.prefix);
				writer.Write((short)item.stack);
			}
		}
		equip = _dyes;
		foreach (Item item2 in equip)
		{
			if (!item2.IsAir)
			{
				writer.Write((short)item2.type);
				writer.Write(item2.prefix);
				writer.Write((short)item2.stack);
			}
		}
		equip = _misc;
		foreach (Item item3 in equip)
		{
			if (!item3.IsAir)
			{
				writer.Write((short)item3.type);
				writer.Write(item3.prefix);
				writer.Write((short)item3.stack);
			}
		}
	}

	public override void ReadExtraData(BinaryReader reader, int gameVersion, bool networkSend)
	{
		BitsByte bitsByte = reader.ReadByte();
		BitsByte bitsByte2 = reader.ReadByte();
		if (gameVersion >= 307)
		{
			_pose = reader.ReadByte();
		}
		BitsByte bitsByte3 = (byte)0;
		if (gameVersion >= 308)
		{
			bitsByte3 = reader.ReadByte();
		}
		bool flag = false;
		if (gameVersion == 311)
		{
			flag = bitsByte3[1];
			bitsByte3[1] = false;
		}
		int num = (byte)bitsByte | (bitsByte3[1] ? 256 : 0);
		for (int i = 0; i < _equip.Length; i++)
		{
			_equip[i] = new Item();
			Item item = _equip[i];
			if ((num & (1 << i)) != 0)
			{
				item.netDefaults(reader.ReadInt16());
				item.Prefix(reader.ReadByte());
				item.stack = reader.ReadInt16();
			}
		}
		long num2 = (byte)bitsByte2 | (bitsByte3[2] ? 256 : 0);
		for (int j = 0; j < _dyes.Length; j++)
		{
			_dyes[j] = new Item();
			Item item2 = _dyes[j];
			if ((num2 & (1 << j)) != 0L)
			{
				item2.netDefaults(reader.ReadInt16());
				item2.Prefix(reader.ReadByte());
				item2.stack = reader.ReadInt16();
			}
		}
		for (int k = 0; k < _misc.Length; k++)
		{
			_misc[k] = new Item();
			Item item3 = _misc[k];
			if (bitsByte3[k])
			{
				item3.netDefaults(reader.ReadInt16());
				item3.Prefix(reader.ReadByte());
				item3.stack = reader.ReadInt16();
			}
		}
		if (flag)
		{
			Item obj = _equip[8];
			obj.netDefaults(reader.ReadInt16());
			obj.Prefix(reader.ReadByte());
			obj.stack = reader.ReadInt16();
		}
	}

	public override string ToString()
	{
		return string.Concat(Position.X, "x  ", Position.Y, "y item: ", _equip[0], " ", _equip[1], " ", _equip[2]);
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
		num -= tileSafely.frameX / 18 % 2;
		num2 -= tileSafely.frameY / 18 % 3;
		bool flag = false;
		for (int i = num; i < num + 2; i++)
		{
			for (int j = num2; j < num2 + 3; j++)
			{
				Tile tile = Main.tile[i, j];
				if (!tile.active() || tile.type != 470)
				{
					flag = true;
				}
			}
		}
		if (!WorldGen.SolidTileAllowBottomSlope(num, num2 + 3) || !WorldGen.SolidTileAllowBottomSlope(num + 1, num2 + 3))
		{
			flag = true;
		}
		if (!flag)
		{
			return;
		}
		TileEntityType<TEDisplayDoll>.Kill(num, num2);
		if (Main.tile[callX, callY].frameX / 72 != 1)
		{
			Item.NewItem(new EntitySource_TileBreak(num, num2), num * 16, num2 * 16, 32, 48, 498);
		}
		else
		{
			Item.NewItem(new EntitySource_TileBreak(num, num2), num * 16, num2 * 16, 32, 48, 1989);
		}
		WorldGen.destroyObject = true;
		for (int k = num; k < num + 2; k++)
		{
			for (int l = num2; l < num2 + 3; l++)
			{
				if (Main.tile[k, l].active() && Main.tile[k, l].type == 470)
				{
					WorldGen.KillTile(k, l);
				}
			}
		}
		WorldGen.destroyObject = false;
	}

	public void Draw(int tileLeftX, int tileTopY)
	{
		//IL_01c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_021b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0220: Unknown result type (might be due to invalid IL or missing references)
		//IL_023b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0240: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_032b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0335: Unknown result type (might be due to invalid IL or missing references)
		//IL_0351: Unknown result type (might be due to invalid IL or missing references)
		//IL_0356: Unknown result type (might be due to invalid IL or missing references)
		//IL_035b: Unknown result type (might be due to invalid IL or missing references)
		//IL_035e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0360: Unknown result type (might be due to invalid IL or missing references)
		//IL_037a: Unknown result type (might be due to invalid IL or missing references)
		//IL_024b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0250: Unknown result type (might be due to invalid IL or missing references)
		//IL_04af: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ba: Unknown result type (might be due to invalid IL or missing references)
		Player dollPlayer = _dollPlayer;
		for (int i = 0; i < 8; i++)
		{
			dollPlayer.armor[i] = _equip[i];
			dollPlayer.dye[i] = _dyes[i];
		}
		Item item = _misc[0];
		dollPlayer.inventory[0] = item;
		dollPlayer.direction = -1;
		dollPlayer.Male = true;
		Tile tileSafely = Framing.GetTileSafely(tileLeftX, tileTopY);
		if (tileSafely.frameX % 72 == 36)
		{
			dollPlayer.direction = 1;
		}
		if (tileSafely.frameX / 72 == 1)
		{
			dollPlayer.Male = false;
		}
		dollPlayer.isDisplayDollOrInanimate = true;
		dollPlayer.ResetEffects();
		dollPlayer.ResetVisibleAccessories();
		dollPlayer.UpdateDyes();
		dollPlayer.DisplayDollUpdate();
		dollPlayer.UpdateSocialShadow();
		dollPlayer.bodyFrameCounter = 0.0;
		dollPlayer.headFrameCounter = 0.0;
		dollPlayer.legFrameCounter = 0.0;
		dollPlayer.wingFrameCounter = 0;
		dollPlayer.sitting.isSitting = false;
		dollPlayer.itemAnimationMax = 0;
		dollPlayer.itemAnimation = 0;
		Item item2 = _equip[8];
		int num = -1;
		if (!item2.IsAir)
		{
			num = item2.mountType;
		}
		if (dollPlayer.mount.Type != num)
		{
			if (num == -1)
			{
				dollPlayer.mount.Dismount(dollPlayer);
			}
			else
			{
				dollPlayer.mount.SetMount(num, dollPlayer);
			}
		}
		dollPlayer.miscDyes[3] = _dyes[8];
		dollPlayer.miscDyes[2] = _dyes[8];
		int num2 = 0;
		DisplayDollPoseID displayDollPoseID = (DisplayDollPoseID)_pose;
		if (!IsValidPose(_pose))
		{
			displayDollPoseID = DisplayDollPoseID.Standing;
		}
		if (num != -1)
		{
			dollPlayer.mount.ApplyDummyFrameCounters();
			if (displayDollPoseID == DisplayDollPoseID.Sitting || displayDollPoseID == DisplayDollPoseID.Jumping)
			{
				displayDollPoseID = DisplayDollPoseID.Standing;
			}
		}
		switch (displayDollPoseID)
		{
		case DisplayDollPoseID.Standing:
			dollPlayer.velocity = Vector2.Zero;
			break;
		case DisplayDollPoseID.Walking:
			dollPlayer.velocity = Vector2.UnitX * (float)dollPlayer.direction;
			dollPlayer.legFrame.Y = dollPlayer.legFrame.Height * 9;
			dollPlayer.bodyFrame.Y = dollPlayer.legFrame.Y;
			break;
		case DisplayDollPoseID.Sitting:
			dollPlayer.velocity = Vector2.Zero;
			dollPlayer.sitting.isSitting = true;
			num2 = 14;
			break;
		case DisplayDollPoseID.Jumping:
			dollPlayer.velocity = Vector2.UnitY;
			break;
		default:
		{
			dollPlayer.velocity = Vector2.Zero;
			if (!SupportedUseStylePoses.TryGetValue(item.useStyle, out var value))
			{
				break;
			}
			foreach (DisplayDollPose item3 in value)
			{
				if ((DisplayDollPoseID)_pose != item3.Pose)
				{
					continue;
				}
				dollPlayer.itemAnimationMax = 1000;
				dollPlayer.itemAnimation = (int)(1000f * item3.ItemAnimationPercent);
				dollPlayer.itemRotation = 0f;
				float? itemAimRadians = item3.ItemAimRadians;
				if (itemAimRadians.HasValue)
				{
					itemAimRadians = item3.ItemAimRadians;
					dollPlayer.itemRotation = itemAimRadians.Value;
					if (dollPlayer.direction == -1)
					{
						dollPlayer.itemRotation *= -1f;
					}
				}
				break;
			}
			break;
		}
		}
		dollPlayer.PlayerFrame();
		Vector2 position = new Vector2((float)(tileLeftX + 1), (float)(tileTopY + 3)) * 16f + new Vector2((float)(-dollPlayer.width / 2), (float)(-dollPlayer.height - 6 + num2));
		dollPlayer.position = position;
		dollPlayer.lastVisualizedSelectedItem = item;
		dollPlayer.ItemCheck_EmitHeldItemLight(item);
		dollPlayer.AnimatePlayerAndGetItemFrame(0f, item);
		_playerRenderer.OverrideHeldProjectile = null;
		if (item != null && !item.IsAir && item.shoot > 0)
		{
			Projectile projectileDummy = _projectileDummy;
			projectileDummy.SetDefaults(item.shoot);
			projectileDummy.isAPreviewDisplayDoll = true;
			bool botherDrawing = false;
			int key = item.useStyle;
			int num3 = ((projectileDummy.type >= 0) ? ProjectileID.Sets.DisplayDollDrawUseStyle[projectileDummy.type] : (-1));
			if (num3 > -1)
			{
				key = num3;
			}
			if (SupportedUseStylePoses.TryGetValue(key, out var value2))
			{
				foreach (DisplayDollPose item4 in value2)
				{
					if ((DisplayDollPoseID)_pose == item4.Pose)
					{
						projectileDummy.AI_DisplayDoll(dollPlayer, item, item4, out botherDrawing);
						break;
					}
				}
			}
			if (botherDrawing)
			{
				_playerRenderer.OverrideHeldProjectile = projectileDummy;
				int drawLayer = projectileDummy.drawLayer;
				if ((uint)drawLayer <= 3u)
				{
					Main.instance.DrawProjDirect(projectileDummy);
				}
			}
		}
		dollPlayer.isFullbright = tileSafely.fullbrightBlock();
		dollPlayer.skinDyePacked = PlayerDrawHelper.PackShader(tileSafely.color(), PlayerDrawHelper.ShaderConfiguration.TilePaintID);
		_playerRenderer.PrepareDrawForFrame(dollPlayer);
		_playerRenderer.DrawPlayer(Main.Camera, dollPlayer, dollPlayer.position, 0f, dollPlayer.fullRotationOrigin);
	}

	public override void OnPlayerUpdate(Player player)
	{
		if (!player.InTileEntityInteractionRange(player.tileEntityAnchor.X, player.tileEntityAnchor.Y, 2, 3, TileReachCheckSettings.Simple) || player.chest != -1 || player.talkNPC != -1)
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
		if (Main.tile[num, num2].frameX % 36 != 0)
		{
			num--;
		}
		num2 -= Main.tile[num, num2].frameY / 18;
		int num3 = TileEntityType<TEDisplayDoll>.Find(num, num2);
		if (num3 != -1)
		{
			num2++;
			TileEntity.BasicOpenCloseInteraction(player, num, num2, num3);
		}
	}

	public override void OnInventoryDraw(Player player, SpriteBatch spriteBatch)
	{
		if (Main.tile[player.tileEntityAnchor.X, player.tileEntityAnchor.Y].type != 470)
		{
			player.tileEntityAnchor.Clear();
		}
		else
		{
			DrawUI(player, spriteBatch);
		}
	}

	public string GetItemGamepadInstructions(int slot = 0)
	{
		Item[] inv = _equip;
		int num = slot;
		int context;
		if (slot >= 18)
		{
			inv = _misc;
			num = 0;
			context = 38;
		}
		else if (slot < 9)
		{
			if (slot == 8)
			{
				context = 39;
			}
			else
			{
				context = ((slot < 3) ? 23 : 24);
			}
		}
		else
		{
			num -= 9;
			inv = _dyes;
			context = 25;
		}
		return ItemSlot.GetGamepadInstructions(inv, context, num);
	}

	private void DrawUI(Player player, SpriteBatch spriteBatch)
	{
		Main.inventoryScale = 0.755f;
		DrawSlotMisc(player, spriteBatch, 1, 0, 0f, 0.5f, 38);
		DrawSlotPairSet(player, spriteBatch, 3, 0, 1f, 0.5f, 23);
		DrawSlotPairSet(player, spriteBatch, 5, 3, 4f, 0.5f, 24);
		DrawSlotPairSet(player, spriteBatch, 1, 8, 9f, 0.5f, 39);
	}

	private void DrawSlotMisc(Player player, SpriteBatch spriteBatch, int slotsToShowLine, int slotsArrayOffset, float offsetX, float offsetY, int inventoryContextTarget)
	{
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		Item[] misc = _misc;
		int context = inventoryContextTarget;
		for (int i = 0; i < slotsToShowLine; i++)
		{
			for (int j = 0; j < 1; j++)
			{
				int num = (int)(22f + ((float)i + offsetX) * 56f * Main.inventoryScale);
				int num2 = (int)((float)Main.instance.invBottom + ((float)j + offsetY) * 56f * Main.inventoryScale);
				if (j == 0)
				{
					misc = _misc;
					context = inventoryContextTarget;
				}
				if (Utils.FloatIntersect(Main.mouseX, Main.mouseY, 0f, 0f, num, num2, (float)TextureAssets.InventoryBack.Width() * Main.inventoryScale, (float)TextureAssets.InventoryBack.Height() * Main.inventoryScale) && !PlayerInput.IgnoreMouseInterface)
				{
					player.mouseInterface = true;
					ItemSlot.Handle(misc, context, i + slotsArrayOffset);
				}
				ItemSlot.Draw(spriteBatch, misc, context, i + slotsArrayOffset, new Vector2((float)num, (float)num2));
			}
		}
	}

	private void DrawSlotPairSet(Player player, SpriteBatch spriteBatch, int slotsToShowLine, int slotsArrayOffset, float offsetX, float offsetY, int inventoryContextTarget)
	{
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		Item[] equip = _equip;
		int num = inventoryContextTarget;
		for (int i = 0; i < slotsToShowLine; i++)
		{
			for (int j = 0; j < 2; j++)
			{
				int num2 = (int)(22f + ((float)i + offsetX) * 56f * Main.inventoryScale);
				int num3 = (int)((float)Main.instance.invBottom + ((float)j + offsetY) * 56f * Main.inventoryScale);
				if (j == 0)
				{
					equip = _equip;
					num = inventoryContextTarget;
				}
				else
				{
					equip = _dyes;
					num = 25;
				}
				if (Utils.FloatIntersect(Main.mouseX, Main.mouseY, 0f, 0f, num2, num3, (float)TextureAssets.InventoryBack.Width() * Main.inventoryScale, (float)TextureAssets.InventoryBack.Height() * Main.inventoryScale) && !PlayerInput.IgnoreMouseInterface)
				{
					player.mouseInterface = true;
					ItemSlot.Handle(equip, num, i + slotsArrayOffset);
				}
				ItemSlot.Draw(spriteBatch, equip, num, i + slotsArrayOffset, new Vector2((float)num2, (float)num3));
			}
		}
	}

	public override ItemSlot.AlternateClickAction? GetShiftClickAction(Item[] inv, int context = 0, int slot = 0)
	{
		Item item = inv[slot];
		if (context == 0 && CanQuickSwapIntoDisplayDoll(item))
		{
			return ItemSlot.AlternateClickAction.TransferToChest;
		}
		if ((context == 23 || context == 24 || context == 39 || context == 25 || context == 38) && Main.LocalPlayer.ItemSpace(item).CanTakeItemToPersonalInventory)
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
			if (!item.IsAir && !item.favorited && CanQuickSwapIntoDisplayDoll(item))
			{
				return TryFitting(inv, slot);
			}
		}
		else if (Main.cursorOverride == 8 && (context == 23 || context == 24 || context == 39 || context == 25 || context == 38))
		{
			inv[slot] = Main.LocalPlayer.GetItem(item, GetItemSettings.QuickTransferFromSlot);
			if (Main.netMode == 1)
			{
				NetMessage.SendData(121, -1, -1, null, Main.myPlayer, ID, slot, context switch
				{
					25 => 1, 
					38 => 3, 
					_ => 0, 
				});
			}
			return true;
		}
		return false;
	}

	public static bool CanQuickSwapIntoDisplayDoll(Item item)
	{
		if (item.headSlot <= 0 && item.bodySlot <= 0 && item.legSlot <= 0 && !item.accessory && item.mountType < 0)
		{
			return AcceptedInWeaponSlot(item);
		}
		return true;
	}

	public static bool AcceptedInWeaponSlot(Item item)
	{
		if (item.useStyle == 0 || item.mountType != -1)
		{
			return item.holdStyle != 0;
		}
		return true;
	}

	private bool TryFitting(Item[] inv, int slot)
	{
		Item item = inv[slot];
		Item[] array = _equip;
		int num = -1;
		if (item.headSlot > 0)
		{
			num = 0;
		}
		else if (item.bodySlot > 0)
		{
			num = 1;
		}
		else if (item.legSlot > 0)
		{
			num = 2;
		}
		else if (item.accessory)
		{
			num = GetAccessoryTargetSlot(item);
		}
		else if (item.mountType >= 0)
		{
			num = 8;
		}
		else if (AcceptedInWeaponSlot(item))
		{
			array = _misc;
			num = 0;
		}
		if (num == -1)
		{
			return false;
		}
		if (item.stack > 1 && !array[num].IsAir)
		{
			return true;
		}
		SoundEngine.PlaySound(7);
		if (item.stack > 1)
		{
			item.favorited = false;
			array[num] = item.Clone();
			array[num].stack = 1;
			item.stack--;
		}
		else
		{
			inv[slot].favorited = false;
			Utils.Swap(ref array[num], ref inv[slot]);
		}
		if (Main.netMode == 1)
		{
			NetMessage.SendData(121, -1, -1, null, Main.myPlayer, ID, num, (array == _misc) ? 3 : 0);
		}
		return true;
	}

	private int GetAccessoryTargetSlot(Item item)
	{
		if (ItemSlot.HasIncompatibleAccessory(item, vanity: true, AccSlots, out var collisionSlot))
		{
			return collisionSlot;
		}
		for (int i = 3; i < 8; i++)
		{
			if (_equip[i].IsAir)
			{
				return i;
			}
		}
		return 3;
	}

	public void WriteItem(int itemIndex, BinaryWriter writer, Item[] collection)
	{
		Item item = collection[itemIndex];
		writer.Write((ushort)item.type);
		writer.Write((ushort)item.stack);
		writer.Write(item.prefix);
	}

	public void ReadItem(int itemIndex, BinaryReader reader, Item[] collection)
	{
		int num = reader.ReadUInt16();
		int stack = reader.ReadUInt16();
		int prefixWeWant = reader.ReadByte();
		if (itemIndex < collection.Length)
		{
			Item obj = collection[itemIndex];
			obj.SetDefaults(num);
			obj.stack = stack;
			obj.Prefix(prefixWeWant);
		}
	}

	public void WriteData(int itemIndex, int command, BinaryWriter writer)
	{
		bool flag = command == 1;
		bool flag2 = command == 2;
		bool flag3 = command == 3;
		if (flag2)
		{
			writer.Write(_pose);
			return;
		}
		Item[] collection = _equip;
		if (flag)
		{
			collection = _dyes;
		}
		if (flag3)
		{
			collection = _misc;
		}
		WriteItem(itemIndex, writer, collection);
	}

	public void ReadData(int itemIndex, int command, BinaryReader reader)
	{
		bool flag = command == 1;
		bool flag2 = command == 2;
		bool flag3 = command == 3;
		if (flag2)
		{
			ReadPose(reader);
			return;
		}
		Item[] collection = _equip;
		if (flag)
		{
			collection = _dyes;
		}
		if (flag3)
		{
			collection = _misc;
		}
		ReadItem(itemIndex, reader, collection);
	}

	public static void WriteDummySync(int itemIndex, int command, BinaryWriter writer)
	{
		if (command == 2)
		{
			writer.Write((byte)0);
			return;
		}
		writer.Write(0);
		writer.Write((byte)0);
	}

	public static void ReadDummySync(int itemIndex, int command, BinaryReader reader)
	{
		if (command == 2)
		{
			reader.ReadByte();
			return;
		}
		reader.ReadInt32();
		reader.ReadByte();
	}

	public void ReadPose(BinaryReader reader)
	{
		_pose = reader.ReadByte();
	}

	public override bool IsTileValidForEntity(int x, int y)
	{
		if (!Main.tile[x, y].active() || Main.tile[x, y].type != 470 || Main.tile[x, y].frameY != 0 || Main.tile[x, y].frameX % 36 != 0)
		{
			return false;
		}
		return true;
	}

	public void SetInventoryFromMannequin(int headFrame, int shirtFrame, int legFrame)
	{
		headFrame /= 100;
		shirtFrame /= 100;
		legFrame /= 100;
		if (headFrame >= 0 && headFrame < Item.headType.Length)
		{
			_equip[0].SetDefaults(Item.headType[headFrame]);
		}
		if (shirtFrame >= 0 && shirtFrame < Item.bodyType.Length)
		{
			_equip[1].SetDefaults(Item.bodyType[shirtFrame]);
		}
		if (legFrame >= 0 && legFrame < Item.legType.Length)
		{
			_equip[2].SetDefaults(Item.legType[legFrame]);
		}
	}

	public static bool IsBreakable(int clickX, int clickY)
	{
		int num = clickX;
		int num2 = clickY;
		if (Main.tile[num, num2].frameX % 36 != 0)
		{
			num--;
		}
		num2 -= Main.tile[num, num2].frameY / 18;
		if (TileEntity.TryGetAt<TEDisplayDoll>(num, num2, out var result))
		{
			return !result.ContainsItems();
		}
		return true;
	}

	public static bool TryChangePose(int clickX, int clickY)
	{
		int num = clickX;
		int num2 = clickY;
		if (Main.tile[num, num2].frameX % 36 != 0)
		{
			num--;
		}
		num2 -= Main.tile[num, num2].frameY / 18;
		if (TileEntity.TryGetAt<TEDisplayDoll>(num, num2, out var result))
		{
			result.ChangePose();
			if (Main.netMode == 1)
			{
				NetMessage.SendData(121, -1, -1, null, Main.myPlayer, result.ID, (int)result._pose, 2f);
			}
			return true;
		}
		return false;
	}

	public void ChangePose()
	{
		_pose++;
		if (!IsValidPose(_pose))
		{
			_pose = 0;
		}
	}

	public bool ContainsItems()
	{
		Item[] equip = _equip;
		for (int i = 0; i < equip.Length; i++)
		{
			if (!equip[i].IsAir)
			{
				return true;
			}
		}
		equip = _dyes;
		for (int i = 0; i < equip.Length; i++)
		{
			if (!equip[i].IsAir)
			{
				return true;
			}
		}
		equip = _misc;
		for (int i = 0; i < equip.Length; i++)
		{
			if (!equip[i].IsAir)
			{
				return true;
			}
		}
		return false;
	}

	public void FixLoadedData()
	{
		Item[] equip = _equip;
		for (int i = 0; i < equip.Length; i++)
		{
			equip[i].FixAgainstExploit();
		}
		equip = _dyes;
		for (int i = 0; i < equip.Length; i++)
		{
			equip[i].FixAgainstExploit();
		}
		equip = _misc;
		for (int i = 0; i < equip.Length; i++)
		{
			equip[i].FixAgainstExploit();
		}
	}
}
