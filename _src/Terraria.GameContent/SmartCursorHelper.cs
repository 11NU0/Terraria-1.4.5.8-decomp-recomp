using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.GameContent.UI;
using Terraria.GameInput;
using Terraria.ID;

namespace Terraria.GameContent;

public class SmartCursorHelper
{
	public class SmartCursorUsageInfo
	{
		public Player player;

		public Item item;

		public Vector2 mouse;

		public Vector2 position;

		public Vector2 Center;

		public int screenTargetX;

		public int screenTargetY;

		public int reachableStartX;

		public int reachableEndX;

		public int reachableStartY;

		public int reachableEndY;

		public int paintLookup;

		public int paintCoatingLookup;
	}

	private static List<Point> _targets = new List<Point>();

	private static List<Point> _grappleTargets = new List<Point>();

	private static List<Point> _points = new List<Point>();

	private static List<Point> _endpoints = new List<Point>();

	private static List<Point> _toRemove = new List<Point>();

	private static List<Point> _targets2 = new List<Point>();

	private static Point? _lockedDesiredDirection;

	private static Point? _lockedContinuityCoords;

	public static Point? LockedDesiredDirection => _lockedDesiredDirection;

	public static void SmartCursorLookup(Player player)
	{
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_022c: Unknown result type (might be due to invalid IL or missing references)
		//IL_023c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0255: Unknown result type (might be due to invalid IL or missing references)
		//IL_0298: Unknown result type (might be due to invalid IL or missing references)
		//IL_029e: Unknown result type (might be due to invalid IL or missing references)
		Main.SmartCursorShowing = false;
		if (!player.controlUseItem || !Main.SmartCursorIsUsed || !UsingSmartCursorWithContinuity(player))
		{
			_lockedDesiredDirection = null;
			_lockedContinuityCoords = null;
		}
		if (!Main.SmartCursorIsUsed)
		{
			return;
		}
		SmartCursorUsageInfo smartCursorUsageInfo = new SmartCursorUsageInfo
		{
			player = player,
			item = player.HeldItem,
			mouse = Main.MouseWorld,
			position = player.position,
			Center = player.Center
		};
		_ = player.gravDir;
		int tileTargetX = Player.tileTargetX;
		int tileTargetY = Player.tileTargetY;
		_ = Player.tileRangeX;
		_ = Player.tileRangeY;
		smartCursorUsageInfo.screenTargetX = Utils.Clamp(tileTargetX, 10, Main.maxTilesX - 10);
		smartCursorUsageInfo.screenTargetY = Utils.Clamp(tileTargetY, 10, Main.maxTilesY - 10);
		if (Main.tile[smartCursorUsageInfo.screenTargetX, smartCursorUsageInfo.screenTargetY] == null)
		{
			return;
		}
		bool flag = IsHoveringOverAnInteractableTileThatBlocksSmartCursor(smartCursorUsageInfo);
		TryFindingPaintInplayerInventory(smartCursorUsageInfo, out smartCursorUsageInfo.paintLookup, out smartCursorUsageInfo.paintCoatingLookup);
		int num = smartCursorUsageInfo.item.tileBoost;
		if (smartCursorUsageInfo.item.createWall > 0 || smartCursorUsageInfo.item.createTile > 0 || smartCursorUsageInfo.item.tileWand > 0)
		{
			num += player.blockRange;
		}
		TileReachCheckSettings.Simple.GetTileRegion(player, out smartCursorUsageInfo.reachableStartX, out smartCursorUsageInfo.reachableStartY, out smartCursorUsageInfo.reachableEndX, out smartCursorUsageInfo.reachableEndY, num);
		smartCursorUsageInfo.reachableStartX = Utils.Clamp(smartCursorUsageInfo.reachableStartX, 10, Main.maxTilesX - 10);
		smartCursorUsageInfo.reachableEndX = Utils.Clamp(smartCursorUsageInfo.reachableEndX, 10, Main.maxTilesX - 10);
		smartCursorUsageInfo.reachableStartY = Utils.Clamp(smartCursorUsageInfo.reachableStartY, 10, Main.maxTilesY - 10);
		smartCursorUsageInfo.reachableEndY = Utils.Clamp(smartCursorUsageInfo.reachableEndY, 10, Main.maxTilesY - 10);
		if (!flag || smartCursorUsageInfo.screenTargetX < smartCursorUsageInfo.reachableStartX || smartCursorUsageInfo.screenTargetX > smartCursorUsageInfo.reachableEndX || smartCursorUsageInfo.screenTargetY < smartCursorUsageInfo.reachableStartY || smartCursorUsageInfo.screenTargetY > smartCursorUsageInfo.reachableEndY)
		{
			_grappleTargets.Clear();
			int[] grappling = player.grappling;
			int grapCount = player.grapCount;
			for (int i = 0; i < grapCount; i++)
			{
				Projectile obj = Main.projectile[grappling[i]];
				int num2 = (int)obj.Center.X / 16;
				int num3 = (int)obj.Center.Y / 16;
				_grappleTargets.Add(new Point(num2, num3));
			}
			int fX = -1;
			int fY = -1;
			if (!Player.SmartCursorSettings.SmartAxeAfterPickaxe)
			{
				Step_Axe(smartCursorUsageInfo, ref fX, ref fY);
			}
			Step_ForceCursorToAnyMinableThing(smartCursorUsageInfo, ref fX, ref fY);
			Step_Pickaxe_MineShinies(smartCursorUsageInfo, ref fX, ref fY);
			Step_Pickaxe_MineSolids(player, player.position, player.Center, player.width, player.direction, smartCursorUsageInfo, _grappleTargets, ref fX, ref fY);
			if (Player.SmartCursorSettings.SmartAxeAfterPickaxe)
			{
				Step_Axe(smartCursorUsageInfo, ref fX, ref fY);
			}
			Step_ColoredWrenches(smartCursorUsageInfo, ref fX, ref fY);
			Step_MulticolorWrench(smartCursorUsageInfo, ref fX, ref fY);
			Step_Hammers(smartCursorUsageInfo, ref fX, ref fY);
			Step_ActuationRod(smartCursorUsageInfo, ref fX, ref fY);
			Step_WireCutter(smartCursorUsageInfo, ref fX, ref fY);
			Step_Platforms(smartCursorUsageInfo, ref fX, ref fY);
			Step_MinecartTracks(smartCursorUsageInfo, ref fX, ref fY);
			Step_Walls(smartCursorUsageInfo, ref fX, ref fY);
			Step_PumpkinSeeds(smartCursorUsageInfo, ref fX, ref fY);
			Step_GrassSeeds(smartCursorUsageInfo, ref fX, ref fY);
			Step_Moss(smartCursorUsageInfo, ref fX, ref fY);
			Step_Pigronata(smartCursorUsageInfo, ref fX, ref fY);
			Step_Boulders(smartCursorUsageInfo, ref fX, ref fY);
			Step_Torch(smartCursorUsageInfo, ref fX, ref fY);
			Step_LawnMower(smartCursorUsageInfo, ref fX, ref fY);
			Step_BlocksFilling(smartCursorUsageInfo, ref fX, ref fY);
			Step_BlocksLines(smartCursorUsageInfo, ref fX, ref fY);
			Step_PaintRoller(smartCursorUsageInfo, ref fX, ref fY);
			Step_PaintBrush(smartCursorUsageInfo, ref fX, ref fY);
			Step_PaintScrapper(smartCursorUsageInfo, ref fX, ref fY);
			Step_Acorns(smartCursorUsageInfo, ref fX, ref fY);
			Step_GemCorns(smartCursorUsageInfo, ref fX, ref fY);
			Step_EmptyBuckets(smartCursorUsageInfo, ref fX, ref fY);
			Step_Actuators(smartCursorUsageInfo, ref fX, ref fY);
			Step_AlchemySeeds(smartCursorUsageInfo, ref fX, ref fY);
			Step_PlanterBox(smartCursorUsageInfo, ref fX, ref fY);
			Step_ClayPots(smartCursorUsageInfo, ref fX, ref fY);
			Step_StaffOfRegrowth(smartCursorUsageInfo, ref fX, ref fY);
			if (fX != -1 && fY != -1)
			{
				Main.SmartCursorX = (Player.tileTargetX = fX);
				Main.SmartCursorY = (Player.tileTargetY = fY);
				Main.SmartCursorShowing = true;
			}
			_grappleTargets.Clear();
		}
	}

	private static bool UsingSmartCursorWithContinuity(Player player)
	{
		Item heldItem = player.HeldItem;
		if (heldItem.createTile >= 0)
		{
			return TileID.Sets.Platforms[heldItem.createTile];
		}
		return false;
	}

	private static void TryFindingPaintInplayerInventory(SmartCursorUsageInfo providedInfo, out int paintLookup, out int coatingLookup)
	{
		_ = providedInfo.player.inventory;
		paintLookup = 0;
		coatingLookup = 0;
		if (providedInfo.item.type == 1071 || providedInfo.item.type == 1543 || providedInfo.item.type == 1072 || providedInfo.item.type == 1544)
		{
			Item item = providedInfo.player.FindPaintOrCoating();
			if (item != null)
			{
				coatingLookup = item.paintCoating;
				paintLookup = item.paint;
			}
		}
	}

	private static bool IsHoveringOverAnInteractableTileThatBlocksSmartCursor(SmartCursorUsageInfo providedInfo)
	{
		bool result = false;
		Tile tile = Main.tile[providedInfo.screenTargetX, providedInfo.screenTargetY];
		if (tile.active())
		{
			if (TileID.Sets.DisableSmartCursor[tile.type])
			{
				result = true;
			}
			if (tile.type == 314 && providedInfo.player.gravDir == 1f)
			{
				result = true;
			}
		}
		return result;
	}

	private static bool AllowNormalBlockPlacementBehaviourForItemType(int itemType)
	{
		if (itemType < 0 || itemType >= ItemID.Count)
		{
			return false;
		}
		if (itemType == 213 || itemType == 5295 || ItemID.Sets.GrassSeeds[itemType] || ItemID.Sets.Moss[itemType])
		{
			return false;
		}
		return true;
	}

	private static void Step_StaffOfRegrowth(SmartCursorUsageInfo providedInfo, ref int focusedX, ref int focusedY)
	{
		//IL_019d: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0235: Unknown result type (might be due to invalid IL or missing references)
		//IL_023c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0217: Unknown result type (might be due to invalid IL or missing references)
		//IL_021c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0263: Unknown result type (might be due to invalid IL or missing references)
		//IL_026c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0156: Unknown result type (might be due to invalid IL or missing references)
		if ((providedInfo.item.type != 213 && providedInfo.item.type != 5295) || focusedX != -1 || focusedY != -1)
		{
			return;
		}
		_targets.Clear();
		for (int i = providedInfo.reachableStartX; i <= providedInfo.reachableEndX; i++)
		{
			for (int j = providedInfo.reachableStartY; j <= providedInfo.reachableEndY; j++)
			{
				Tile tile = Main.tile[i, j];
				bool flag = !Main.tile[i - 1, j].active() || !Main.tile[i, j + 1].active() || !Main.tile[i + 1, j].active() || !Main.tile[i, j - 1].active();
				bool flag2 = !Main.tile[i - 1, j - 1].active() || !Main.tile[i - 1, j + 1].active() || !Main.tile[i + 1, j + 1].active() || !Main.tile[i + 1, j - 1].active();
				if (tile.active() && !tile.inActive() && tile.type == 0 && (flag || ((tile.type == 0) & flag2)))
				{
					_targets.Add(new Point(i, j));
				}
			}
		}
		if (_targets.Count > 0)
		{
			float num = -1f;
			Point val = _targets[0];
			for (int k = 0; k < _targets.Count; k++)
			{
				float num2 = Vector2.Distance(new Vector2((float)_targets[k].X, (float)_targets[k].Y) * 16f + Vector2.One * 8f, providedInfo.mouse);
				if (num == -1f || num2 < num)
				{
					num = num2;
					val = _targets[k];
				}
			}
			if (Collision.InTileBounds(val.X, val.Y, providedInfo.reachableStartX, providedInfo.reachableStartY, providedInfo.reachableEndX, providedInfo.reachableEndY))
			{
				focusedX = val.X;
				focusedY = val.Y;
			}
		}
		_targets.Clear();
	}

	private static void Step_GrassSeeds(SmartCursorUsageInfo providedInfo, ref int focusedX, ref int focusedY)
	{
		//IL_01f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_020a: Unknown result type (might be due to invalid IL or missing references)
		//IL_021c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0227: Unknown result type (might be due to invalid IL or missing references)
		//IL_0231: Unknown result type (might be due to invalid IL or missing references)
		//IL_0236: Unknown result type (might be due to invalid IL or missing references)
		//IL_0240: Unknown result type (might be due to invalid IL or missing references)
		//IL_0245: Unknown result type (might be due to invalid IL or missing references)
		//IL_024b: Unknown result type (might be due to invalid IL or missing references)
		//IL_028f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0296: Unknown result type (might be due to invalid IL or missing references)
		//IL_0271: Unknown result type (might be due to invalid IL or missing references)
		//IL_0276: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b0: Unknown result type (might be due to invalid IL or missing references)
		if (focusedX > -1 || focusedY > -1)
		{
			return;
		}
		int type = providedInfo.item.type;
		if (type < 0 || type >= ItemID.Count || !ItemID.Sets.GrassSeeds[type])
		{
			return;
		}
		_targets.Clear();
		for (int i = providedInfo.reachableStartX; i <= providedInfo.reachableEndX; i++)
		{
			for (int j = providedInfo.reachableStartY; j <= providedInfo.reachableEndY; j++)
			{
				Tile tile = Main.tile[i, j];
				bool flag = !Main.tile[i - 1, j].active() || !Main.tile[i, j + 1].active() || !Main.tile[i + 1, j].active() || !Main.tile[i, j - 1].active();
				bool flag2 = !Main.tile[i - 1, j - 1].active() || !Main.tile[i - 1, j + 1].active() || !Main.tile[i + 1, j + 1].active() || !Main.tile[i + 1, j - 1].active();
				if (tile.active() && !tile.inActive() && (flag || flag2))
				{
					bool flag3 = false;
					switch (type)
					{
					default:
						flag3 = tile.type == 0;
						break;
					case 59:
					case 2171:
						flag3 = tile.type == 0 || tile.type == 59;
						break;
					case 194:
					case 195:
						flag3 = tile.type == 59;
						break;
					case 5214:
						flag3 = tile.type == 57;
						break;
					}
					if (flag3)
					{
						_targets.Add(new Point(i, j));
					}
				}
			}
		}
		if (_targets.Count > 0)
		{
			float num = -1f;
			Point val = _targets[0];
			for (int k = 0; k < _targets.Count; k++)
			{
				float num2 = Vector2.Distance(new Vector2((float)_targets[k].X, (float)_targets[k].Y) * 16f + Vector2.One * 8f, providedInfo.mouse);
				if (num == -1f || num2 < num)
				{
					num = num2;
					val = _targets[k];
				}
			}
			if (Collision.InTileBounds(val.X, val.Y, providedInfo.reachableStartX, providedInfo.reachableStartY, providedInfo.reachableEndX, providedInfo.reachableEndY))
			{
				focusedX = val.X;
				focusedY = val.Y;
			}
		}
		_targets.Clear();
	}

	private static void Step_Moss(SmartCursorUsageInfo providedInfo, ref int focusedX, ref int focusedY)
	{
		//IL_0196: Unknown result type (might be due to invalid IL or missing references)
		//IL_019b: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01df: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_022e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0235: Unknown result type (might be due to invalid IL or missing references)
		//IL_0210: Unknown result type (might be due to invalid IL or missing references)
		//IL_0215: Unknown result type (might be due to invalid IL or missing references)
		//IL_025c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0265: Unknown result type (might be due to invalid IL or missing references)
		//IL_014f: Unknown result type (might be due to invalid IL or missing references)
		if (focusedX > -1 || focusedY > -1)
		{
			return;
		}
		int type = providedInfo.item.type;
		if (type < 0 || type >= ItemID.Count || !ItemID.Sets.Moss[type])
		{
			return;
		}
		_targets.Clear();
		for (int i = providedInfo.reachableStartX; i <= providedInfo.reachableEndX; i++)
		{
			for (int j = providedInfo.reachableStartY; j <= providedInfo.reachableEndY; j++)
			{
				Tile tile = Main.tile[i, j];
				bool flag = !Main.tile[i - 1, j].active() || !Main.tile[i, j + 1].active() || !Main.tile[i + 1, j].active() || !Main.tile[i, j - 1].active();
				bool flag2 = !Main.tile[i - 1, j - 1].active() || !Main.tile[i - 1, j + 1].active() || !Main.tile[i + 1, j + 1].active() || !Main.tile[i + 1, j - 1].active();
				if (tile.active() && !tile.inActive() && (flag || flag2) && (tile.type == 1 || tile.type == 38))
				{
					_targets.Add(new Point(i, j));
				}
			}
		}
		if (_targets.Count > 0)
		{
			float num = -1f;
			Point val = _targets[0];
			for (int k = 0; k < _targets.Count; k++)
			{
				float num2 = Vector2.Distance(new Vector2((float)_targets[k].X, (float)_targets[k].Y) * 16f + Vector2.One * 8f, providedInfo.mouse);
				if (num == -1f || num2 < num)
				{
					num = num2;
					val = _targets[k];
				}
			}
			if (Collision.InTileBounds(val.X, val.Y, providedInfo.reachableStartX, providedInfo.reachableStartY, providedInfo.reachableEndX, providedInfo.reachableEndY))
			{
				focusedX = val.X;
				focusedY = val.Y;
			}
		}
		_targets.Clear();
	}

	private static void Step_ClayPots(SmartCursorUsageInfo providedInfo, ref int focusedX, ref int focusedY)
	{
		//IL_0151: Unknown result type (might be due to invalid IL or missing references)
		//IL_0156: Unknown result type (might be due to invalid IL or missing references)
		//IL_0167: Unknown result type (might be due to invalid IL or missing references)
		//IL_0178: Unknown result type (might be due to invalid IL or missing references)
		//IL_0216: Unknown result type (might be due to invalid IL or missing references)
		//IL_021d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0191: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_024d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0256: Unknown result type (might be due to invalid IL or missing references)
		//IL_010a: Unknown result type (might be due to invalid IL or missing references)
		if (providedInfo.item.createTile != 78 || focusedX != -1 || focusedY != -1)
		{
			return;
		}
		_targets.Clear();
		bool flag = false;
		if (Main.tile[providedInfo.screenTargetX, providedInfo.screenTargetY].active())
		{
			flag = true;
		}
		if (!Collision.InTileBounds(providedInfo.screenTargetX, providedInfo.screenTargetY, providedInfo.reachableStartX, providedInfo.reachableStartY, providedInfo.reachableEndX, providedInfo.reachableEndY))
		{
			flag = true;
		}
		if (!flag)
		{
			for (int i = providedInfo.reachableStartX; i <= providedInfo.reachableEndX; i++)
			{
				for (int j = providedInfo.reachableStartY; j <= providedInfo.reachableEndY; j++)
				{
					Tile tile = Main.tile[i, j];
					Tile tile2 = Main.tile[i, j + 1];
					if ((!tile.active() || Main.tileCut[tile.type] || TileID.Sets.BreakableWhenPlacing[tile.type]) && tile2.nactive() && !tile2.halfBrick() && tile2.slope() == 0 && Main.tileSolid[tile2.type])
					{
						_targets.Add(new Point(i, j));
					}
				}
			}
		}
		if (_targets.Count > 0)
		{
			float num = -1f;
			Point val = _targets[0];
			for (int k = 0; k < _targets.Count; k++)
			{
				if (Collision.EmptyTile(_targets[k].X, _targets[k].Y, ignoreTiles: true))
				{
					float num2 = Vector2.Distance(new Vector2((float)_targets[k].X, (float)_targets[k].Y) * 16f + Vector2.One * 8f, providedInfo.mouse);
					if (num == -1f || num2 < num)
					{
						num = num2;
						val = _targets[k];
					}
				}
			}
			if (Collision.InTileBounds(val.X, val.Y, providedInfo.reachableStartX, providedInfo.reachableStartY, providedInfo.reachableEndX, providedInfo.reachableEndY) && num != -1f)
			{
				focusedX = val.X;
				focusedY = val.Y;
			}
		}
		_targets.Clear();
	}

	private static void Step_PlanterBox(SmartCursorUsageInfo providedInfo, ref int focusedX, ref int focusedY)
	{
		//IL_01b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0205: Unknown result type (might be due to invalid IL or missing references)
		//IL_0249: Unknown result type (might be due to invalid IL or missing references)
		//IL_0250: Unknown result type (might be due to invalid IL or missing references)
		//IL_022b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0230: Unknown result type (might be due to invalid IL or missing references)
		//IL_0280: Unknown result type (might be due to invalid IL or missing references)
		//IL_0289: Unknown result type (might be due to invalid IL or missing references)
		//IL_010c: Unknown result type (might be due to invalid IL or missing references)
		//IL_016a: Unknown result type (might be due to invalid IL or missing references)
		if (providedInfo.item.createTile != 380 || focusedX != -1 || focusedY != -1)
		{
			return;
		}
		_targets.Clear();
		bool flag = false;
		if (Main.tile[providedInfo.screenTargetX, providedInfo.screenTargetY].active() && Main.tile[providedInfo.screenTargetX, providedInfo.screenTargetY].type == 380)
		{
			flag = true;
		}
		if (!flag)
		{
			for (int i = providedInfo.reachableStartX; i <= providedInfo.reachableEndX; i++)
			{
				for (int j = providedInfo.reachableStartY; j <= providedInfo.reachableEndY; j++)
				{
					Tile tile = Main.tile[i, j];
					if (tile.active() && tile.type == 380)
					{
						if (!Main.tile[i - 1, j].active() || Main.tileCut[Main.tile[i - 1, j].type] || TileID.Sets.BreakableWhenPlacing[Main.tile[i - 1, j].type])
						{
							_targets.Add(new Point(i - 1, j));
						}
						if (!Main.tile[i + 1, j].active() || Main.tileCut[Main.tile[i + 1, j].type] || TileID.Sets.BreakableWhenPlacing[Main.tile[i + 1, j].type])
						{
							_targets.Add(new Point(i + 1, j));
						}
					}
				}
			}
		}
		if (_targets.Count > 0)
		{
			float num = -1f;
			Point val = _targets[0];
			for (int k = 0; k < _targets.Count; k++)
			{
				float num2 = Vector2.Distance(new Vector2((float)_targets[k].X, (float)_targets[k].Y) * 16f + Vector2.One * 8f, providedInfo.mouse);
				if (num == -1f || num2 < num)
				{
					num = num2;
					val = _targets[k];
				}
			}
			if (Collision.InTileBounds(val.X, val.Y, providedInfo.reachableStartX, providedInfo.reachableStartY, providedInfo.reachableEndX, providedInfo.reachableEndY) && num != -1f)
			{
				focusedX = val.X;
				focusedY = val.Y;
			}
		}
		_targets.Clear();
	}

	private static void Step_AlchemySeeds(SmartCursorUsageInfo providedInfo, ref int focusedX, ref int focusedY)
	{
		//IL_0395: Unknown result type (might be due to invalid IL or missing references)
		//IL_039a: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_03de: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_042d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0434: Unknown result type (might be due to invalid IL or missing references)
		//IL_040f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0414: Unknown result type (might be due to invalid IL or missing references)
		//IL_045b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0464: Unknown result type (might be due to invalid IL or missing references)
		//IL_034e: Unknown result type (might be due to invalid IL or missing references)
		if (providedInfo.item.createTile != 82 || focusedX != -1 || focusedY != -1)
		{
			return;
		}
		int placeStyle = providedInfo.item.placeStyle;
		_targets.Clear();
		for (int i = providedInfo.reachableStartX; i <= providedInfo.reachableEndX; i++)
		{
			for (int j = providedInfo.reachableStartY; j <= providedInfo.reachableEndY; j++)
			{
				Tile tile = Main.tile[i, j];
				Tile tile2 = Main.tile[i, j + 1];
				bool num = !tile.active() || TileID.Sets.BreakableWhenPlacing[tile.type] || (Main.tileCut[tile.type] && tile.type != 82 && tile.type != 83) || WorldGen.IsHarvestableHerbWithSeed(tile.type, tile.frameX / 18, j);
				bool flag = tile2.nactive() && !tile2.halfBrick() && tile2.slope() == 0;
				if (!num || !flag)
				{
					continue;
				}
				switch (placeStyle)
				{
				case 0:
					if ((tile2.type != 78 && tile2.type != 380 && tile2.type != 2 && tile2.type != 477 && tile2.type != 109 && tile2.type != 492) || tile.liquid > 0)
					{
						continue;
					}
					break;
				case 1:
					if ((tile2.type != 78 && tile2.type != 380 && tile2.type != 60) || tile.liquid > 0)
					{
						continue;
					}
					break;
				case 2:
					if ((tile2.type != 78 && tile2.type != 380 && tile2.type != 0 && tile2.type != 59) || tile.liquid > 0)
					{
						continue;
					}
					break;
				case 3:
					if ((tile2.type != 78 && tile2.type != 380 && tile2.type != 203 && tile2.type != 199 && tile2.type != 23 && tile2.type != 25) || tile.liquid > 0)
					{
						continue;
					}
					break;
				case 4:
					if ((tile2.type != 78 && tile2.type != 380 && tile2.type != 53 && tile2.type != 116) || (tile.liquid > 0 && tile.lava()))
					{
						continue;
					}
					break;
				case 5:
					if ((tile2.type != 78 && tile2.type != 380 && tile2.type != 57 && tile2.type != 633) || (tile.liquid > 0 && !tile.lava()))
					{
						continue;
					}
					break;
				case 6:
					if ((tile2.type != 78 && tile2.type != 380 && tile2.type != 147 && tile2.type != 161 && tile2.type != 163 && tile2.type != 164 && tile2.type != 200) || (tile.liquid > 0 && tile.lava()))
					{
						continue;
					}
					break;
				}
				_targets.Add(new Point(i, j));
			}
		}
		if (_targets.Count > 0)
		{
			float num2 = -1f;
			Point val = _targets[0];
			for (int k = 0; k < _targets.Count; k++)
			{
				float num3 = Vector2.Distance(new Vector2((float)_targets[k].X, (float)_targets[k].Y) * 16f + Vector2.One * 8f, providedInfo.mouse);
				if (num2 == -1f || num3 < num2)
				{
					num2 = num3;
					val = _targets[k];
				}
			}
			if (Collision.InTileBounds(val.X, val.Y, providedInfo.reachableStartX, providedInfo.reachableStartY, providedInfo.reachableEndX, providedInfo.reachableEndY))
			{
				focusedX = val.X;
				focusedY = val.Y;
			}
		}
		_targets.Clear();
	}

	private static void Step_Actuators(SmartCursorUsageInfo providedInfo, ref int focusedX, ref int focusedY)
	{
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0104: Unknown result type (might be due to invalid IL or missing references)
		//IL_010e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0113: Unknown result type (might be due to invalid IL or missing references)
		//IL_0119: Unknown result type (might be due to invalid IL or missing references)
		//IL_015a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0161: Unknown result type (might be due to invalid IL or missing references)
		//IL_013c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0141: Unknown result type (might be due to invalid IL or missing references)
		//IL_0188: Unknown result type (might be due to invalid IL or missing references)
		//IL_0191: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		if (providedInfo.item.type != 849 || focusedX != -1 || focusedY != -1)
		{
			return;
		}
		_targets.Clear();
		for (int i = providedInfo.reachableStartX; i <= providedInfo.reachableEndX; i++)
		{
			for (int j = providedInfo.reachableStartY; j <= providedInfo.reachableEndY; j++)
			{
				Tile tile = Main.tile[i, j];
				if ((tile.wire() || tile.wire2() || tile.wire3() || tile.wire4()) && !tile.actuator() && tile.active())
				{
					_targets.Add(new Point(i, j));
				}
			}
		}
		if (_targets.Count > 0)
		{
			float num = -1f;
			Point val = _targets[0];
			for (int k = 0; k < _targets.Count; k++)
			{
				float num2 = Vector2.Distance(new Vector2((float)_targets[k].X, (float)_targets[k].Y) * 16f + Vector2.One * 8f, providedInfo.mouse);
				if (num == -1f || num2 < num)
				{
					num = num2;
					val = _targets[k];
				}
			}
			if (Collision.InTileBounds(val.X, val.Y, providedInfo.reachableStartX, providedInfo.reachableStartY, providedInfo.reachableEndX, providedInfo.reachableEndY))
			{
				focusedX = val.X;
				focusedY = val.Y;
			}
		}
		_targets.Clear();
	}

	private static void Step_EmptyBuckets(SmartCursorUsageInfo providedInfo, ref int focusedX, ref int focusedY)
	{
		//IL_0111: Unknown result type (might be due to invalid IL or missing references)
		//IL_0116: Unknown result type (might be due to invalid IL or missing references)
		//IL_0124: Unknown result type (might be due to invalid IL or missing references)
		//IL_0136: Unknown result type (might be due to invalid IL or missing references)
		//IL_0141: Unknown result type (might be due to invalid IL or missing references)
		//IL_014b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0150: Unknown result type (might be due to invalid IL or missing references)
		//IL_015a: Unknown result type (might be due to invalid IL or missing references)
		//IL_015f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0165: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_018b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0190: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		if (providedInfo.item.type != 205 || focusedX != -1 || focusedY != -1)
		{
			return;
		}
		_targets.Clear();
		for (int i = providedInfo.reachableStartX; i <= providedInfo.reachableEndX; i++)
		{
			for (int j = providedInfo.reachableStartY; j <= providedInfo.reachableEndY; j++)
			{
				Tile tile = Main.tile[i, j];
				if (tile.liquid <= 0)
				{
					continue;
				}
				int num = tile.liquidType();
				int num2 = 0;
				for (int k = i - 1; k <= i + 1; k++)
				{
					for (int l = j - 1; l <= j + 1; l++)
					{
						if (Main.tile[k, l].liquidType() == num)
						{
							num2 += Main.tile[k, l].liquid;
						}
					}
				}
				if (num2 > 100)
				{
					_targets.Add(new Point(i, j));
				}
			}
		}
		if (_targets.Count > 0)
		{
			float num3 = -1f;
			Point val = _targets[0];
			for (int m = 0; m < _targets.Count; m++)
			{
				float num4 = Vector2.Distance(new Vector2((float)_targets[m].X, (float)_targets[m].Y) * 16f + Vector2.One * 8f, providedInfo.mouse);
				if (num3 == -1f || num4 < num3)
				{
					num3 = num4;
					val = _targets[m];
				}
			}
			if (Collision.InTileBounds(val.X, val.Y, providedInfo.reachableStartX, providedInfo.reachableStartY, providedInfo.reachableEndX, providedInfo.reachableEndY))
			{
				focusedX = val.X;
				focusedY = val.Y;
			}
		}
		_targets.Clear();
	}

	private static void Step_PaintScrapper(SmartCursorUsageInfo providedInfo, ref int focusedX, ref int focusedY)
	{
		//IL_0105: Unknown result type (might be due to invalid IL or missing references)
		//IL_010a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0118: Unknown result type (might be due to invalid IL or missing references)
		//IL_012a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0135: Unknown result type (might be due to invalid IL or missing references)
		//IL_013f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0144: Unknown result type (might be due to invalid IL or missing references)
		//IL_014e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0153: Unknown result type (might be due to invalid IL or missing references)
		//IL_0159: Unknown result type (might be due to invalid IL or missing references)
		//IL_019d: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_017f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0184: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		if (!ItemID.Sets.IsPaintScraper[providedInfo.item.type] || focusedX != -1 || focusedY != -1)
		{
			return;
		}
		_targets.Clear();
		for (int i = providedInfo.reachableStartX; i <= providedInfo.reachableEndX; i++)
		{
			for (int j = providedInfo.reachableStartY; j <= providedInfo.reachableEndY; j++)
			{
				Tile tile = Main.tile[i, j];
				bool flag = false;
				if (tile.active())
				{
					flag |= tile.color() > 0;
					flag |= tile.type == 184;
					flag |= tile.fullbrightBlock();
					flag |= tile.invisibleBlock();
				}
				if (tile.wall > 0)
				{
					flag |= tile.wallColor() > 0;
					flag |= tile.fullbrightWall();
					flag |= tile.invisibleWall();
				}
				if (flag)
				{
					_targets.Add(new Point(i, j));
				}
			}
		}
		if (_targets.Count > 0)
		{
			float num = -1f;
			Point val = _targets[0];
			for (int k = 0; k < _targets.Count; k++)
			{
				float num2 = Vector2.Distance(new Vector2((float)_targets[k].X, (float)_targets[k].Y) * 16f + Vector2.One * 8f, providedInfo.mouse);
				if (num == -1f || num2 < num)
				{
					num = num2;
					val = _targets[k];
				}
			}
			if (Collision.InTileBounds(val.X, val.Y, providedInfo.reachableStartX, providedInfo.reachableStartY, providedInfo.reachableEndX, providedInfo.reachableEndY))
			{
				focusedX = val.X;
				focusedY = val.Y;
			}
		}
		_targets.Clear();
	}

	private static void Step_PaintBrush(SmartCursorUsageInfo providedInfo, ref int focusedX, ref int focusedY)
	{
		//IL_011a: Unknown result type (might be due to invalid IL or missing references)
		//IL_011f: Unknown result type (might be due to invalid IL or missing references)
		//IL_012d: Unknown result type (might be due to invalid IL or missing references)
		//IL_013f: Unknown result type (might be due to invalid IL or missing references)
		//IL_014a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0154: Unknown result type (might be due to invalid IL or missing references)
		//IL_0159: Unknown result type (might be due to invalid IL or missing references)
		//IL_0163: Unknown result type (might be due to invalid IL or missing references)
		//IL_0168: Unknown result type (might be due to invalid IL or missing references)
		//IL_016e: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0194: Unknown result type (might be due to invalid IL or missing references)
		//IL_0199: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		if ((providedInfo.item.type != 1071 && providedInfo.item.type != 1543) || (providedInfo.paintLookup == 0 && providedInfo.paintCoatingLookup == 0) || focusedX != -1 || focusedY != -1)
		{
			return;
		}
		_targets.Clear();
		int paintLookup = providedInfo.paintLookup;
		int paintCoatingLookup = providedInfo.paintCoatingLookup;
		if (paintLookup != 0 || paintCoatingLookup != 0)
		{
			for (int i = providedInfo.reachableStartX; i <= providedInfo.reachableEndX; i++)
			{
				for (int j = providedInfo.reachableStartY; j <= providedInfo.reachableEndY; j++)
				{
					Tile tile = Main.tile[i, j];
					if (tile.active() && (0u | ((paintLookup != 0 && tile.color() != paintLookup) ? 1u : 0u) | ((paintCoatingLookup == 1 && !tile.fullbrightBlock()) ? 1u : 0u) | ((paintCoatingLookup == 2 && !tile.invisibleBlock()) ? 1u : 0u)) != 0)
					{
						_targets.Add(new Point(i, j));
					}
				}
			}
		}
		if (_targets.Count > 0)
		{
			float num = -1f;
			Point val = _targets[0];
			for (int k = 0; k < _targets.Count; k++)
			{
				float num2 = Vector2.Distance(new Vector2((float)_targets[k].X, (float)_targets[k].Y) * 16f + Vector2.One * 8f, providedInfo.mouse);
				if (num == -1f || num2 < num)
				{
					num = num2;
					val = _targets[k];
				}
			}
			if (Collision.InTileBounds(val.X, val.Y, providedInfo.reachableStartX, providedInfo.reachableStartY, providedInfo.reachableEndX, providedInfo.reachableEndY))
			{
				focusedX = val.X;
				focusedY = val.Y;
			}
		}
		_targets.Clear();
	}

	private static void Step_PaintRoller(SmartCursorUsageInfo providedInfo, ref int focusedX, ref int focusedY)
	{
		//IL_0142: Unknown result type (might be due to invalid IL or missing references)
		//IL_0147: Unknown result type (might be due to invalid IL or missing references)
		//IL_0155: Unknown result type (might be due to invalid IL or missing references)
		//IL_0167: Unknown result type (might be due to invalid IL or missing references)
		//IL_0172: Unknown result type (might be due to invalid IL or missing references)
		//IL_017c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0181: Unknown result type (might be due to invalid IL or missing references)
		//IL_018b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0190: Unknown result type (might be due to invalid IL or missing references)
		//IL_0196: Unknown result type (might be due to invalid IL or missing references)
		//IL_01da: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0208: Unknown result type (might be due to invalid IL or missing references)
		//IL_0211: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
		if ((providedInfo.item.type != 1072 && providedInfo.item.type != 1544) || (providedInfo.paintLookup == 0 && providedInfo.paintCoatingLookup == 0) || focusedX != -1 || focusedY != -1)
		{
			return;
		}
		_targets.Clear();
		int paintLookup = providedInfo.paintLookup;
		int paintCoatingLookup = providedInfo.paintCoatingLookup;
		for (int i = providedInfo.reachableStartX; i <= providedInfo.reachableEndX; i++)
		{
			for (int j = providedInfo.reachableStartY; j <= providedInfo.reachableEndY; j++)
			{
				Tile tile = Main.tile[i, j];
				if (tile.wall > 0 && (!tile.active() || !Main.tileSolid[tile.type] || Main.tileSolidTop[tile.type]) && (0u | ((paintLookup != 0 && tile.wallColor() != paintLookup) ? 1u : 0u) | ((paintCoatingLookup == 1 && !tile.fullbrightWall()) ? 1u : 0u) | ((paintCoatingLookup == 2 && !tile.invisibleWall()) ? 1u : 0u)) != 0)
				{
					_targets.Add(new Point(i, j));
				}
			}
		}
		if (_targets.Count > 0)
		{
			float num = -1f;
			Point val = _targets[0];
			for (int k = 0; k < _targets.Count; k++)
			{
				float num2 = Vector2.Distance(new Vector2((float)_targets[k].X, (float)_targets[k].Y) * 16f + Vector2.One * 8f, providedInfo.mouse);
				if (num == -1f || num2 < num)
				{
					num = num2;
					val = _targets[k];
				}
			}
			if (Collision.InTileBounds(val.X, val.Y, providedInfo.reachableStartX, providedInfo.reachableStartY, providedInfo.reachableEndX, providedInfo.reachableEndY))
			{
				focusedX = val.X;
				focusedY = val.Y;
			}
		}
		_targets.Clear();
	}

	private static void Step_BlocksLines(SmartCursorUsageInfo providedInfo, ref int focusedX, ref int focusedY)
	{
		//IL_02cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0391: Unknown result type (might be due to invalid IL or missing references)
		//IL_0398: Unknown result type (might be due to invalid IL or missing references)
		//IL_030c: Unknown result type (might be due to invalid IL or missing references)
		//IL_031e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0329: Unknown result type (might be due to invalid IL or missing references)
		//IL_0333: Unknown result type (might be due to invalid IL or missing references)
		//IL_0338: Unknown result type (might be due to invalid IL or missing references)
		//IL_0342: Unknown result type (might be due to invalid IL or missing references)
		//IL_0347: Unknown result type (might be due to invalid IL or missing references)
		//IL_034d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0373: Unknown result type (might be due to invalid IL or missing references)
		//IL_0378: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0285: Unknown result type (might be due to invalid IL or missing references)
		int type = providedInfo.item.type;
		if (type < 0 || type >= ItemID.Count || !Player.SmartCursorSettings.SmartBlocksEnabled || providedInfo.item.createTile <= -1 || !AllowNormalBlockPlacementBehaviourForItemType(type) || !Main.tileSolid[providedInfo.item.createTile] || Main.tileSolidTop[providedInfo.item.createTile] || Main.tileFrameImportant[providedInfo.item.createTile] || focusedX != -1 || focusedY != -1)
		{
			return;
		}
		_targets.Clear();
		bool flag = false;
		if (Main.tile[providedInfo.screenTargetX, providedInfo.screenTargetY].active())
		{
			flag = true;
		}
		if (!Collision.InTileBounds(providedInfo.screenTargetX, providedInfo.screenTargetY, providedInfo.reachableStartX, providedInfo.reachableStartY, providedInfo.reachableEndX, providedInfo.reachableEndY))
		{
			flag = true;
		}
		if (!flag)
		{
			for (int i = providedInfo.reachableStartX; i <= providedInfo.reachableEndX; i++)
			{
				for (int j = providedInfo.reachableStartY; j <= providedInfo.reachableEndY; j++)
				{
					Tile tile = Main.tile[i, j];
					if (!tile.active() || Main.tileCut[tile.type] || TileID.Sets.BreakableWhenPlacing[tile.type])
					{
						bool flag2 = false;
						if (Main.tile[i - 1, j].active() && Main.tileSolid[Main.tile[i - 1, j].type] && !Main.tileSolidTop[Main.tile[i - 1, j].type])
						{
							flag2 = true;
						}
						if (Main.tile[i + 1, j].active() && Main.tileSolid[Main.tile[i + 1, j].type] && !Main.tileSolidTop[Main.tile[i + 1, j].type])
						{
							flag2 = true;
						}
						if (Main.tile[i, j - 1].active() && Main.tileSolid[Main.tile[i, j - 1].type] && !Main.tileSolidTop[Main.tile[i, j - 1].type])
						{
							flag2 = true;
						}
						if (Main.tile[i, j + 1].active() && Main.tileSolid[Main.tile[i, j + 1].type] && !Main.tileSolidTop[Main.tile[i, j + 1].type])
						{
							flag2 = true;
						}
						if (flag2)
						{
							_targets.Add(new Point(i, j));
						}
					}
				}
			}
		}
		if (_targets.Count > 0)
		{
			float num = -1f;
			Point val = _targets[0];
			for (int k = 0; k < _targets.Count; k++)
			{
				if (Collision.EmptyTile(_targets[k].X, _targets[k].Y))
				{
					float num2 = Vector2.Distance(new Vector2((float)_targets[k].X, (float)_targets[k].Y) * 16f + Vector2.One * 8f, providedInfo.mouse);
					if (num == -1f || num2 < num)
					{
						num = num2;
						val = _targets[k];
					}
				}
			}
			if (Collision.InTileBounds(val.X, val.Y, providedInfo.reachableStartX, providedInfo.reachableStartY, providedInfo.reachableEndX, providedInfo.reachableEndY) && num != -1f)
			{
				focusedX = val.X;
				focusedY = val.Y;
			}
		}
		_targets.Clear();
	}

	private static void Step_Boulders(SmartCursorUsageInfo providedInfo, ref int focusedX, ref int focusedY)
	{
		//IL_0233: Unknown result type (might be due to invalid IL or missing references)
		//IL_0238: Unknown result type (might be due to invalid IL or missing references)
		//IL_0246: Unknown result type (might be due to invalid IL or missing references)
		//IL_0258: Unknown result type (might be due to invalid IL or missing references)
		//IL_0263: Unknown result type (might be due to invalid IL or missing references)
		//IL_026d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0272: Unknown result type (might be due to invalid IL or missing references)
		//IL_027c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0281: Unknown result type (might be due to invalid IL or missing references)
		//IL_0287: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0302: Unknown result type (might be due to invalid IL or missing references)
		//IL_0193: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c4: Unknown result type (might be due to invalid IL or missing references)
		if (providedInfo.item.createTile <= -1 || providedInfo.item.createTile >= TileID.Count || !TileID.Sets.Boulders[providedInfo.item.createTile] || focusedX != -1 || focusedY != -1)
		{
			return;
		}
		_targets.Clear();
		for (int i = providedInfo.reachableStartX; i <= providedInfo.reachableEndX; i++)
		{
			for (int j = providedInfo.reachableStartY; j <= providedInfo.reachableEndY; j++)
			{
				Tile tile = Main.tile[i, j + 1];
				Tile tile2 = Main.tile[i + 1, j + 1];
				bool flag = true;
				if (!tile2.nactive() || !tile.nactive())
				{
					flag = false;
				}
				if (tile2.slope() > 0 || tile.slope() > 0 || tile2.halfBrick() || tile.halfBrick())
				{
					flag = false;
				}
				if ((!Main.tileSolid[tile2.type] && !Main.tileTable[tile2.type]) || (!Main.tileSolid[tile.type] && !Main.tileTable[tile.type]))
				{
					flag = false;
				}
				if (Main.tileNoAttach[tile2.type] || Main.tileNoAttach[tile.type])
				{
					flag = false;
				}
				for (int k = i; k <= i + 1; k++)
				{
					for (int l = j - 1; l <= j; l++)
					{
						Tile tile3 = Main.tile[k, l];
						if (tile3.active() && !Main.tileCut[tile3.type])
						{
							flag = false;
						}
					}
				}
				int num = i * 16;
				int num2 = j * 16 - 16;
				int num3 = 32;
				int num4 = 32;
				Rectangle val = new Rectangle(num, num2, num3, num4);
				for (int m = 0; m < 255; m++)
				{
					Player player = Main.player[m];
					if (player.active && !player.dead)
					{
						Rectangle hitbox = player.Hitbox;
						if (hitbox.Intersects(val))
						{
							flag = false;
							break;
						}
					}
				}
				if (flag)
				{
					_targets.Add(new Point(i, j));
				}
			}
		}
		if (_targets.Count > 0)
		{
			float num5 = -1f;
			Point val2 = _targets[0];
			for (int n = 0; n < _targets.Count; n++)
			{
				float num6 = Vector2.Distance(new Vector2((float)_targets[n].X, (float)_targets[n].Y) * 16f + Vector2.One * 8f, providedInfo.mouse);
				if (num5 == -1f || num6 < num5)
				{
					num5 = num6;
					val2 = _targets[n];
				}
			}
			if (Collision.InTileBounds(val2.X, val2.Y, providedInfo.reachableStartX, providedInfo.reachableStartY, providedInfo.reachableEndX, providedInfo.reachableEndY))
			{
				focusedX = val2.X;
				focusedY = val2.Y;
			}
		}
		_targets.Clear();
	}

	private static void Step_Pigronata(SmartCursorUsageInfo providedInfo, ref int focusedX, ref int focusedY)
	{
		//IL_011f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0124: Unknown result type (might be due to invalid IL or missing references)
		//IL_0132: Unknown result type (might be due to invalid IL or missing references)
		//IL_0144: Unknown result type (might be due to invalid IL or missing references)
		//IL_014f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0159: Unknown result type (might be due to invalid IL or missing references)
		//IL_015e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0168: Unknown result type (might be due to invalid IL or missing references)
		//IL_016d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0173: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01be: Unknown result type (might be due to invalid IL or missing references)
		//IL_0199: Unknown result type (might be due to invalid IL or missing references)
		//IL_019e: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		if (providedInfo.item.createTile != 454 || focusedX != -1 || focusedY != -1)
		{
			return;
		}
		_targets.Clear();
		for (int i = providedInfo.reachableStartX; i <= providedInfo.reachableEndX; i++)
		{
			for (int j = providedInfo.reachableStartY; j <= providedInfo.reachableEndY && !((double)j > Main.worldSurface - 2.0); j++)
			{
				bool flag = true;
				for (int k = i - 2; k <= i + 1; k++)
				{
					for (int l = j - 1; l <= j + 2; l++)
					{
						Tile tile = Main.tile[k, l];
						if (l == j - 1)
						{
							if (!WorldGen.SolidTile(tile))
							{
								flag = false;
							}
						}
						else if (tile.active() && (!Main.tileCut[tile.type] || tile.type == 454))
						{
							flag = false;
						}
					}
				}
				if (flag)
				{
					_targets.Add(new Point(i, j));
				}
			}
		}
		if (_targets.Count > 0)
		{
			float num = -1f;
			Point val = _targets[0];
			for (int m = 0; m < _targets.Count; m++)
			{
				float num2 = Vector2.Distance(new Vector2((float)_targets[m].X, (float)_targets[m].Y) * 16f + Vector2.One * 8f, providedInfo.mouse);
				if (num == -1f || num2 < num)
				{
					num = num2;
					val = _targets[m];
				}
			}
			if (Collision.InTileBounds(val.X, val.Y, providedInfo.reachableStartX, providedInfo.reachableStartY, providedInfo.reachableEndX, providedInfo.reachableEndY))
			{
				focusedX = val.X;
				focusedY = val.Y;
			}
		}
		_targets.Clear();
	}

	private static void Step_PumpkinSeeds(SmartCursorUsageInfo providedInfo, ref int focusedX, ref int focusedY)
	{
		//IL_01dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0202: Unknown result type (might be due to invalid IL or missing references)
		//IL_020d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0217: Unknown result type (might be due to invalid IL or missing references)
		//IL_021c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0226: Unknown result type (might be due to invalid IL or missing references)
		//IL_022b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0231: Unknown result type (might be due to invalid IL or missing references)
		//IL_0275: Unknown result type (might be due to invalid IL or missing references)
		//IL_027c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0257: Unknown result type (might be due to invalid IL or missing references)
		//IL_025c: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_0196: Unknown result type (might be due to invalid IL or missing references)
		if (providedInfo.item.createTile != 254 || focusedX != -1 || focusedY != -1)
		{
			return;
		}
		_targets.Clear();
		for (int i = providedInfo.reachableStartX; i <= providedInfo.reachableEndX; i++)
		{
			for (int j = providedInfo.reachableStartY; j <= providedInfo.reachableEndY; j++)
			{
				Tile tile = Main.tile[i, j + 1];
				Tile tile2 = Main.tile[i + 1, j + 1];
				if ((double)j > Main.worldSurface - 2.0)
				{
					break;
				}
				bool flag = true;
				if (!tile2.active() || !tile.active())
				{
					flag = false;
				}
				if (tile2.slope() > 0 || tile.slope() > 0 || tile2.halfBrick() || tile.halfBrick())
				{
					flag = false;
				}
				if (tile2.type != 2 && tile2.type != 477 && tile2.type != 109 && tile2.type != 492)
				{
					flag = false;
				}
				if (tile.type != 2 && tile.type != 477 && tile.type != 109 && tile.type != 492)
				{
					flag = false;
				}
				for (int k = i; k <= i + 1; k++)
				{
					for (int l = j - 1; l <= j; l++)
					{
						Tile tile3 = Main.tile[k, l];
						if (tile3.active() && (tile3.type < 0 || tile3.type >= TileID.Count || Main.tileSolid[tile3.type] || !WorldGen.CanCutTile(k, l, TileCuttingContext.TilePlacement)))
						{
							flag = false;
						}
					}
				}
				if (flag)
				{
					_targets.Add(new Point(i, j));
				}
			}
		}
		if (_targets.Count > 0)
		{
			float num = -1f;
			Point val = _targets[0];
			for (int m = 0; m < _targets.Count; m++)
			{
				float num2 = Vector2.Distance(new Vector2((float)_targets[m].X, (float)_targets[m].Y) * 16f + Vector2.One * 8f, providedInfo.mouse);
				if (num == -1f || num2 < num)
				{
					num = num2;
					val = _targets[m];
				}
			}
			if (Collision.InTileBounds(val.X, val.Y, providedInfo.reachableStartX, providedInfo.reachableStartY, providedInfo.reachableEndX, providedInfo.reachableEndY))
			{
				focusedX = val.X;
				focusedY = val.Y;
			}
		}
		_targets.Clear();
	}

	private static void Step_Walls(SmartCursorUsageInfo providedInfo, ref int focusedX, ref int focusedY)
	{
		//IL_01ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0200: Unknown result type (might be due to invalid IL or missing references)
		//IL_0212: Unknown result type (might be due to invalid IL or missing references)
		//IL_021d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0227: Unknown result type (might be due to invalid IL or missing references)
		//IL_022c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0236: Unknown result type (might be due to invalid IL or missing references)
		//IL_023b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0241: Unknown result type (might be due to invalid IL or missing references)
		//IL_0285: Unknown result type (might be due to invalid IL or missing references)
		//IL_028c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0267: Unknown result type (might be due to invalid IL or missing references)
		//IL_026c: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a6: Unknown result type (might be due to invalid IL or missing references)
		int width = providedInfo.player.width;
		int height = providedInfo.player.height;
		if (providedInfo.item.createWall <= 0 || focusedX != -1 || focusedY != -1)
		{
			return;
		}
		_targets.Clear();
		for (int i = providedInfo.reachableStartX; i <= providedInfo.reachableEndX; i++)
		{
			for (int j = providedInfo.reachableStartY; j <= providedInfo.reachableEndY; j++)
			{
				Tile tile = Main.tile[i, j];
				if (tile.wall == 0 && (!tile.active() || !Main.tileSolid[tile.type] || Main.tileSolidTop[tile.type]) && Collision.CanHitWithCheck(providedInfo.position, width, height, new Vector2((float)i, (float)j) * 16f, 16, 16, DelegateMethods.NotDoorStand))
				{
					bool flag = false;
					if (Main.tile[i - 1, j].active() || Main.tile[i - 1, j].wall > 0)
					{
						flag = true;
					}
					if (Main.tile[i + 1, j].active() || Main.tile[i + 1, j].wall > 0)
					{
						flag = true;
					}
					if (Main.tile[i, j - 1].active() || Main.tile[i, j - 1].wall > 0)
					{
						flag = true;
					}
					if (Main.tile[i, j + 1].active() || Main.tile[i, j + 1].wall > 0)
					{
						flag = true;
					}
					if (WorldGen.IsOpenDoorAnchorFrame(i, j))
					{
						flag = false;
					}
					if (flag)
					{
						_targets.Add(new Point(i, j));
					}
				}
			}
		}
		if (_targets.Count > 0)
		{
			float num = -1f;
			Point val = _targets[0];
			for (int k = 0; k < _targets.Count; k++)
			{
				float num2 = Vector2.Distance(new Vector2((float)_targets[k].X, (float)_targets[k].Y) * 16f + Vector2.One * 8f, providedInfo.mouse);
				if (num == -1f || num2 < num)
				{
					num = num2;
					val = _targets[k];
				}
			}
			if (Collision.InTileBounds(val.X, val.Y, providedInfo.reachableStartX, providedInfo.reachableStartY, providedInfo.reachableEndX, providedInfo.reachableEndY))
			{
				focusedX = val.X;
				focusedY = val.Y;
			}
		}
		_targets.Clear();
	}

	private static void Step_MinecartTracks(SmartCursorUsageInfo providedInfo, ref int focusedX, ref int focusedY)
	{
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_090e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0913: Unknown result type (might be due to invalid IL or missing references)
		//IL_0929: Unknown result type (might be due to invalid IL or missing references)
		//IL_093a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a8a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a91: Unknown result type (might be due to invalid IL or missing references)
		//IL_056f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0574: Unknown result type (might be due to invalid IL or missing references)
		//IL_099b: Unknown result type (might be due to invalid IL or missing references)
		//IL_09ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_095e: Unknown result type (might be due to invalid IL or missing references)
		//IL_096f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a05: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a17: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a22: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a2c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a31: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a3b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a40: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a46: Unknown result type (might be due to invalid IL or missing references)
		//IL_09d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_09e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ac1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aca: Unknown result type (might be due to invalid IL or missing references)
		//IL_058a: Unknown result type (might be due to invalid IL or missing references)
		//IL_059b: Unknown result type (might be due to invalid IL or missing references)
		//IL_06eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_06f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a6c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a71: Unknown result type (might be due to invalid IL or missing references)
		//IL_05fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_060d: Unknown result type (might be due to invalid IL or missing references)
		//IL_05bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_085b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0666: Unknown result type (might be due to invalid IL or missing references)
		//IL_0678: Unknown result type (might be due to invalid IL or missing references)
		//IL_0683: Unknown result type (might be due to invalid IL or missing references)
		//IL_068d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0692: Unknown result type (might be due to invalid IL or missing references)
		//IL_069c: Unknown result type (might be due to invalid IL or missing references)
		//IL_06a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_06a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0631: Unknown result type (might be due to invalid IL or missing references)
		//IL_0642: Unknown result type (might be due to invalid IL or missing references)
		//IL_0722: Unknown result type (might be due to invalid IL or missing references)
		//IL_072b: Unknown result type (might be due to invalid IL or missing references)
		//IL_06cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_06d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_08c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0339: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0439: Unknown result type (might be due to invalid IL or missing references)
		//IL_0522: Unknown result type (might be due to invalid IL or missing references)
		if ((providedInfo.item.type == 2340 || providedInfo.item.type == 2739) && focusedX == -1 && focusedY == -1)
		{
			_targets.Clear();
			Vector2 val = (Main.MouseWorld - providedInfo.Center).SafeNormalize(Vector2.UnitY);
			float num = Vector2.Dot(val, -Vector2.UnitY);
			bool flag = num >= 0.5f;
			bool flag2 = num <= -0.5f;
			float num2 = Vector2.Dot(val, Vector2.UnitX);
			bool flag3 = num2 >= 0.5f;
			bool flag4 = num2 <= -0.5f;
			bool flag5 = flag & flag4;
			bool flag6 = flag & flag3;
			bool flag7 = flag2 & flag4;
			bool flag8 = flag2 & flag3;
			if (flag5)
			{
				flag4 = false;
			}
			if (flag6)
			{
				flag3 = false;
			}
			if (flag7)
			{
				flag4 = false;
			}
			if (flag8)
			{
				flag3 = false;
			}
			bool flag9 = false;
			if (Main.tile[providedInfo.screenTargetX, providedInfo.screenTargetY].active() && Main.tile[providedInfo.screenTargetX, providedInfo.screenTargetY].type == 314)
			{
				flag9 = true;
			}
			if (!flag9)
			{
				for (int i = providedInfo.reachableStartX; i <= providedInfo.reachableEndX; i++)
				{
					for (int j = providedInfo.reachableStartY; j <= providedInfo.reachableEndY; j++)
					{
						Tile tile = Main.tile[i, j];
						if (tile.active() && tile.type == 314)
						{
							bool flag10 = Main.tile[i + 1, j + 1].active() && Main.tile[i + 1, j + 1].type == 314;
							bool flag11 = Main.tile[i + 1, j - 1].active() && Main.tile[i + 1, j - 1].type == 314;
							bool flag12 = Main.tile[i - 1, j + 1].active() && Main.tile[i - 1, j + 1].type == 314;
							bool flag13 = Main.tile[i - 1, j - 1].active() && Main.tile[i - 1, j - 1].type == 314;
							if (flag5 && (!Main.tile[i - 1, j - 1].active() || Main.tileCut[Main.tile[i - 1, j - 1].type] || TileID.Sets.BreakableWhenPlacing[Main.tile[i - 1, j - 1].type]) && !(!flag10 & flag11) && !flag12)
							{
								_targets.Add(new Point(i - 1, j - 1));
							}
							if (flag4 && (!Main.tile[i - 1, j].active() || Main.tileCut[Main.tile[i - 1, j].type] || TileID.Sets.BreakableWhenPlacing[Main.tile[i - 1, j].type]))
							{
								_targets.Add(new Point(i - 1, j));
							}
							if (flag7 && (!Main.tile[i - 1, j + 1].active() || Main.tileCut[Main.tile[i - 1, j + 1].type] || TileID.Sets.BreakableWhenPlacing[Main.tile[i - 1, j + 1].type]) && !(!flag11 & flag10) && !flag13)
							{
								_targets.Add(new Point(i - 1, j + 1));
							}
							if (flag6 && (!Main.tile[i + 1, j - 1].active() || Main.tileCut[Main.tile[i + 1, j - 1].type] || TileID.Sets.BreakableWhenPlacing[Main.tile[i + 1, j - 1].type]) && !(!flag12 & flag13) && !flag10)
							{
								_targets.Add(new Point(i + 1, j - 1));
							}
							if (flag3 && (!Main.tile[i + 1, j].active() || Main.tileCut[Main.tile[i + 1, j].type] || TileID.Sets.BreakableWhenPlacing[Main.tile[i + 1, j].type]))
							{
								_targets.Add(new Point(i + 1, j));
							}
							if (flag8 && (!Main.tile[i + 1, j + 1].active() || Main.tileCut[Main.tile[i + 1, j + 1].type] || TileID.Sets.BreakableWhenPlacing[Main.tile[i + 1, j + 1].type]) && !(!flag13 & flag12) && !flag11)
							{
								_targets.Add(new Point(i + 1, j + 1));
							}
						}
					}
				}
			}
			if (_targets.Count > 0)
			{
				float num3 = -1f;
				Point val2 = _targets[0];
				for (int k = 0; k < _targets.Count; k++)
				{
					if ((!Main.tile[_targets[k].X, _targets[k].Y - 1].active() || Main.tile[_targets[k].X, _targets[k].Y - 1].type != 314) && (!Main.tile[_targets[k].X, _targets[k].Y + 1].active() || Main.tile[_targets[k].X, _targets[k].Y + 1].type != 314))
					{
						float num4 = Vector2.Distance(new Vector2((float)_targets[k].X, (float)_targets[k].Y) * 16f + Vector2.One * 8f, providedInfo.mouse);
						if (num3 == -1f || num4 < num3)
						{
							num3 = num4;
							val2 = _targets[k];
						}
					}
				}
				if (Collision.InTileBounds(val2.X, val2.Y, providedInfo.reachableStartX, providedInfo.reachableStartY, providedInfo.reachableEndX, providedInfo.reachableEndY) && num3 != -1f)
				{
					focusedX = val2.X;
					focusedY = val2.Y;
				}
			}
			_targets.Clear();
		}
		if (providedInfo.item.type != 2492 || focusedX != -1 || focusedY != -1)
		{
			return;
		}
		_targets.Clear();
		bool flag14 = false;
		if (Main.tile[providedInfo.screenTargetX, providedInfo.screenTargetY].active() && Main.tile[providedInfo.screenTargetX, providedInfo.screenTargetY].type == 314)
		{
			flag14 = true;
		}
		if (!flag14)
		{
			for (int l = providedInfo.reachableStartX; l <= providedInfo.reachableEndX; l++)
			{
				for (int m = providedInfo.reachableStartY; m <= providedInfo.reachableEndY; m++)
				{
					Tile tile2 = Main.tile[l, m];
					if (tile2.active() && tile2.type == 314)
					{
						if (!Main.tile[l - 1, m].active() || Main.tileCut[Main.tile[l - 1, m].type] || TileID.Sets.BreakableWhenPlacing[Main.tile[l - 1, m].type])
						{
							_targets.Add(new Point(l - 1, m));
						}
						if (!Main.tile[l + 1, m].active() || Main.tileCut[Main.tile[l + 1, m].type] || TileID.Sets.BreakableWhenPlacing[Main.tile[l + 1, m].type])
						{
							_targets.Add(new Point(l + 1, m));
						}
					}
				}
			}
		}
		if (_targets.Count > 0)
		{
			float num5 = -1f;
			Point val3 = _targets[0];
			for (int n = 0; n < _targets.Count; n++)
			{
				if ((!Main.tile[_targets[n].X, _targets[n].Y - 1].active() || Main.tile[_targets[n].X, _targets[n].Y - 1].type != 314) && (!Main.tile[_targets[n].X, _targets[n].Y + 1].active() || Main.tile[_targets[n].X, _targets[n].Y + 1].type != 314))
				{
					float num6 = Vector2.Distance(new Vector2((float)_targets[n].X, (float)_targets[n].Y) * 16f + Vector2.One * 8f, providedInfo.mouse);
					if (num5 == -1f || num6 < num5)
					{
						num5 = num6;
						val3 = _targets[n];
					}
				}
			}
			if (Collision.InTileBounds(val3.X, val3.Y, providedInfo.reachableStartX, providedInfo.reachableStartY, providedInfo.reachableEndX, providedInfo.reachableEndY) && num5 != -1f)
			{
				focusedX = val3.X;
				focusedY = val3.Y;
			}
		}
		_targets.Clear();
	}

	private static void Step_Platforms(SmartCursorUsageInfo providedInfo, ref int focusedX, ref int focusedY)
	{
		//IL_022f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0234: Unknown result type (might be due to invalid IL or missing references)
		//IL_023c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0241: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_0252: Unknown result type (might be due to invalid IL or missing references)
		//IL_0257: Unknown result type (might be due to invalid IL or missing references)
		//IL_0260: Unknown result type (might be due to invalid IL or missing references)
		//IL_0265: Unknown result type (might be due to invalid IL or missing references)
		//IL_0268: Unknown result type (might be due to invalid IL or missing references)
		//IL_0274: Unknown result type (might be due to invalid IL or missing references)
		//IL_0283: Unknown result type (might be due to invalid IL or missing references)
		//IL_0288: Unknown result type (might be due to invalid IL or missing references)
		//IL_028d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0298: Unknown result type (might be due to invalid IL or missing references)
		//IL_029a: Unknown result type (might be due to invalid IL or missing references)
		//IL_029c: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0318: Unknown result type (might be due to invalid IL or missing references)
		//IL_0321: Unknown result type (might be due to invalid IL or missing references)
		//IL_0329: Unknown result type (might be due to invalid IL or missing references)
		//IL_0330: Unknown result type (might be due to invalid IL or missing references)
		//IL_0337: Unknown result type (might be due to invalid IL or missing references)
		//IL_034a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0105: Unknown result type (might be due to invalid IL or missing references)
		//IL_0133: Unknown result type (might be due to invalid IL or missing references)
		//IL_016b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0173: Unknown result type (might be due to invalid IL or missing references)
		//IL_013f: Unknown result type (might be due to invalid IL or missing references)
		//IL_019a: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0154: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01db: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e1: Unknown result type (might be due to invalid IL or missing references)
		if (providedInfo.item.createTile < 0 || !TileID.Sets.Platforms[providedInfo.item.createTile] || focusedX != -1 || focusedY != -1 || IsPlatform(providedInfo.screenTargetX, providedInfo.screenTargetY))
		{
			return;
		}
		_targets.Clear();
		_points.Clear();
		for (int i = providedInfo.reachableStartX; i <= providedInfo.reachableEndX; i++)
		{
			for (int j = providedInfo.reachableStartY; j <= providedInfo.reachableEndY; j++)
			{
				Point desiredDirectionFrom = GetDesiredDirectionFrom(providedInfo.mouse - new Point(i, j).ToWorldCoordinates());
				bool flag = !IsPlatform(i, j);
				if (flag && desiredDirectionFrom.Y == 0 && Main.tile[i, j].active() && !WorldGen.SolidTile(i, j) && (IsPlatform(i - 1, j) || IsPlatform(i + 1, j)))
				{
					flag = false;
				}
				if (flag)
				{
					continue;
				}
				int num = ((desiredDirectionFrom.X == desiredDirectionFrom.Y) ? 2 : ((desiredDirectionFrom.X == -desiredDirectionFrom.Y) ? 1 : 0));
				if ((num == 0 || Main.tile[i, j].slope() != num) && (desiredDirectionFrom.X != 0 || (!IsPlatform(i - 1, j + desiredDirectionFrom.Y) && !IsPlatform(i + 1, j + desiredDirectionFrom.Y))))
				{
					Tile tile = Main.tile[i + desiredDirectionFrom.X, j + desiredDirectionFrom.Y];
					if ((!tile.active() || Main.tileCut[tile.type]) && AllowedForContinuity(i + desiredDirectionFrom.X, j + desiredDirectionFrom.Y, 2))
					{
						_targets.Add(new Point(i + desiredDirectionFrom.X, j + desiredDirectionFrom.Y));
						_points.Add(new Point(desiredDirectionFrom.X, desiredDirectionFrom.Y));
					}
				}
			}
		}
		if (_targets.Count > 0)
		{
			float num2 = -1f;
			float num3 = -1f;
			Point val = _targets[0];
			Point val2 = _points[0];
			for (int k = 0; k < _targets.Count; k++)
			{
				Point val3 = _targets[k];
				Point val4 = _points[k];
				Vector2 val5 = providedInfo.mouse - _targets[k].ToWorldCoordinates();
				float num4 = val5.Length();
				float num5 = Vector2.Dot(val5, val4.ToVector2());
				if (num2 == -1f || num4 < num2 || (num4 == num2 && num5 > num3))
				{
					num2 = num4;
					num3 = num5;
					val = val3;
					val2 = val4;
				}
			}
			if (Collision.InTileBounds(val.X, val.Y, providedInfo.reachableStartX, providedInfo.reachableStartY, providedInfo.reachableEndX, providedInfo.reachableEndY))
			{
				focusedX = val.X;
				focusedY = val.Y;
				_lockedDesiredDirection = new Point(val2.X, val2.Y);
				_lockedContinuityCoords = new Point(focusedX, focusedY);
			}
		}
		_targets.Clear();
		_points.Clear();
	}

	public static bool TileTargetDesired()
	{
		if (_lockedContinuityCoords.HasValue)
		{
			if (Main.SmartCursorShowing && Player.tileTargetX == Main.SmartCursorX)
			{
				return Player.tileTargetY == Main.SmartCursorY;
			}
			return false;
		}
		return true;
	}

	private static bool AllowedForContinuity(int x, int y, int skipsAllowed)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		if (!_lockedContinuityCoords.HasValue)
		{
			return true;
		}
		Point value = _lockedContinuityCoords.Value;
		if (x == value.X && y == value.Y)
		{
			return true;
		}
		if (!_lockedDesiredDirection.HasValue)
		{
			return false;
		}
		for (int i = 0; i < skipsAllowed; i++)
		{
			value.X += _lockedDesiredDirection.Value.X;
			value.Y += _lockedDesiredDirection.Value.Y;
			if (x == value.X && y == value.Y)
			{
				return true;
			}
		}
		return false;
	}

	private static Point GetDesiredDirectionFrom(Vector2 offset)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		if (_lockedDesiredDirection.HasValue)
		{
			return _lockedDesiredDirection.Value;
		}
		float num = offset.ToRotation();
		if (num < 0f)
		{
			num += (float)Math.PI * 2f;
		}
		float num2 = (float)Math.PI / 4f;
		return (((float)(int)((num + num2 / 2f) % ((float)Math.PI * 2f) / num2) * num2).ToRotationVector2() * 1.5f).ToPoint();
	}

	private static bool IsPlatform(int x, int y)
	{
		if (Main.tile[x, y].active())
		{
			return TileID.Sets.Platforms[Main.tile[x, y].type];
		}
		return false;
	}

	private static void Step_WireCutter(SmartCursorUsageInfo providedInfo, ref int focusedX, ref int focusedY)
	{
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0106: Unknown result type (might be due to invalid IL or missing references)
		//IL_010b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0111: Unknown result type (might be due to invalid IL or missing references)
		//IL_0152: Unknown result type (might be due to invalid IL or missing references)
		//IL_0159: Unknown result type (might be due to invalid IL or missing references)
		//IL_0134: Unknown result type (might be due to invalid IL or missing references)
		//IL_0139: Unknown result type (might be due to invalid IL or missing references)
		//IL_0180: Unknown result type (might be due to invalid IL or missing references)
		//IL_0189: Unknown result type (might be due to invalid IL or missing references)
		if (providedInfo.item.type != 510 || focusedX != -1 || focusedY != -1)
		{
			return;
		}
		_targets.Clear();
		for (int i = providedInfo.reachableStartX; i <= providedInfo.reachableEndX; i++)
		{
			for (int j = providedInfo.reachableStartY; j <= providedInfo.reachableEndY; j++)
			{
				Tile tile = Main.tile[i, j];
				if (tile.wire() || tile.wire2() || tile.wire3() || tile.wire4() || tile.actuator())
				{
					_targets.Add(new Point(i, j));
				}
			}
		}
		if (_targets.Count > 0)
		{
			float num = -1f;
			Point val = _targets[0];
			for (int k = 0; k < _targets.Count; k++)
			{
				float num2 = Vector2.Distance(new Vector2((float)_targets[k].X, (float)_targets[k].Y) * 16f + Vector2.One * 8f, providedInfo.mouse);
				if (num == -1f || num2 < num)
				{
					num = num2;
					val = _targets[k];
				}
			}
			if (Collision.InTileBounds(val.X, val.Y, providedInfo.reachableStartX, providedInfo.reachableStartY, providedInfo.reachableEndX, providedInfo.reachableEndY))
			{
				focusedX = val.X;
				focusedY = val.Y;
			}
		}
		_targets.Clear();
	}

	private static void Step_ActuationRod(SmartCursorUsageInfo providedInfo, ref int focusedX, ref int focusedY)
	{
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0108: Unknown result type (might be due to invalid IL or missing references)
		//IL_010d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0117: Unknown result type (might be due to invalid IL or missing references)
		//IL_011c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0122: Unknown result type (might be due to invalid IL or missing references)
		//IL_0166: Unknown result type (might be due to invalid IL or missing references)
		//IL_016d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0148: Unknown result type (might be due to invalid IL or missing references)
		//IL_014d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0194: Unknown result type (might be due to invalid IL or missing references)
		//IL_019d: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		bool actuationRodLock = providedInfo.player.ActuationRodLock;
		bool actuationRodLockSetting = providedInfo.player.ActuationRodLockSetting;
		if (providedInfo.item.type != 3620 || focusedX != -1 || focusedY != -1)
		{
			return;
		}
		_targets.Clear();
		for (int i = providedInfo.reachableStartX; i <= providedInfo.reachableEndX; i++)
		{
			for (int j = providedInfo.reachableStartY; j <= providedInfo.reachableEndY; j++)
			{
				Tile tile = Main.tile[i, j];
				if (tile.active() && tile.actuator() && (!actuationRodLock || actuationRodLockSetting == tile.inActive()))
				{
					_targets.Add(new Point(i, j));
				}
			}
		}
		if (_targets.Count > 0)
		{
			float num = -1f;
			Point val = _targets[0];
			for (int k = 0; k < _targets.Count; k++)
			{
				float num2 = Vector2.Distance(new Vector2((float)_targets[k].X, (float)_targets[k].Y) * 16f + Vector2.One * 8f, providedInfo.mouse);
				if (num == -1f || num2 < num)
				{
					num = num2;
					val = _targets[k];
				}
			}
			if (Collision.InTileBounds(val.X, val.Y, providedInfo.reachableStartX, providedInfo.reachableStartY, providedInfo.reachableEndX, providedInfo.reachableEndY))
			{
				focusedX = val.X;
				focusedY = val.Y;
			}
		}
		_targets.Clear();
	}

	private static void Step_Hammers(SmartCursorUsageInfo providedInfo, ref int focusedX, ref int focusedY)
	{
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_06fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0716: Unknown result type (might be due to invalid IL or missing references)
		//IL_0727: Unknown result type (might be due to invalid IL or missing references)
		//IL_07fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_06b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0779: Unknown result type (might be due to invalid IL or missing references)
		//IL_078b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0796: Unknown result type (might be due to invalid IL or missing references)
		//IL_07a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_07a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_07af: Unknown result type (might be due to invalid IL or missing references)
		//IL_07b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_0749: Unknown result type (might be due to invalid IL or missing references)
		//IL_075a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0808: Unknown result type (might be due to invalid IL or missing references)
		//IL_080f: Unknown result type (might be due to invalid IL or missing references)
		//IL_07e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_07e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0842: Unknown result type (might be due to invalid IL or missing references)
		//IL_084b: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0227: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_03da: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_03fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0333: Unknown result type (might be due to invalid IL or missing references)
		//IL_0416: Unknown result type (might be due to invalid IL or missing references)
		//IL_0423: Unknown result type (might be due to invalid IL or missing references)
		//IL_0407: Unknown result type (might be due to invalid IL or missing references)
		//IL_04db: Unknown result type (might be due to invalid IL or missing references)
		//IL_039b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0430: Unknown result type (might be due to invalid IL or missing references)
		//IL_0437: Unknown result type (might be due to invalid IL or missing references)
		//IL_0455: Unknown result type (might be due to invalid IL or missing references)
		//IL_0440: Unknown result type (might be due to invalid IL or missing references)
		//IL_0447: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0505: Unknown result type (might be due to invalid IL or missing references)
		//IL_05dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0461: Unknown result type (might be due to invalid IL or missing references)
		//IL_0468: Unknown result type (might be due to invalid IL or missing references)
		//IL_0475: Unknown result type (might be due to invalid IL or missing references)
		//IL_047c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0557: Unknown result type (might be due to invalid IL or missing references)
		//IL_0569: Unknown result type (might be due to invalid IL or missing references)
		//IL_0574: Unknown result type (might be due to invalid IL or missing references)
		//IL_057e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0583: Unknown result type (might be due to invalid IL or missing references)
		//IL_058d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0592: Unknown result type (might be due to invalid IL or missing references)
		//IL_0598: Unknown result type (might be due to invalid IL or missing references)
		//IL_0527: Unknown result type (might be due to invalid IL or missing references)
		//IL_0538: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_049b: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_048f: Unknown result type (might be due to invalid IL or missing references)
		//IL_05be: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0620: Unknown result type (might be due to invalid IL or missing references)
		//IL_0629: Unknown result type (might be due to invalid IL or missing references)
		int width = providedInfo.player.width;
		int height = providedInfo.player.height;
		if (providedInfo.item.hammer > 0 && focusedX == -1 && focusedY == -1)
		{
			Vector2 val = providedInfo.mouse - providedInfo.Center;
			int num = Math.Sign(val.X);
			int num2 = Math.Sign(val.Y);
			if (Math.Abs(val.X) > Math.Abs(val.Y) * 3f)
			{
				num2 = 0;
				providedInfo.mouse.Y = providedInfo.Center.Y;
			}
			if (Math.Abs(val.Y) > Math.Abs(val.X) * 3f)
			{
				num = 0;
				providedInfo.mouse.X = providedInfo.Center.X;
			}
			_ = (int)providedInfo.Center.X / 16;
			_ = (int)providedInfo.Center.Y / 16;
			_points.Clear();
			_endpoints.Clear();
			int num3 = 1;
			if (num2 == -1 && num != 0)
			{
				num3 = -1;
			}
			int num4 = (int)((providedInfo.position.X + (float)(width / 2) + (float)((width / 2 - 1) * num)) / 16f);
			int num5 = (int)(((double)providedInfo.position.Y + 0.1) / 16.0);
			if (num3 == -1)
			{
				num5 = (int)((providedInfo.position.Y + (float)height - 1f) / 16f);
			}
			int num6 = width / 16 + ((width % 16 != 0) ? 1 : 0);
			int num7 = height / 16 + ((height % 16 != 0) ? 1 : 0);
			if (num != 0)
			{
				for (int i = 0; i < num7; i++)
				{
					if (Main.tile[num4, num5 + i * num3] != null)
					{
						_points.Add(new Point(num4, num5 + i * num3));
					}
				}
			}
			if (num2 != 0)
			{
				for (int j = 0; j < num6; j++)
				{
					if (Main.tile[(int)(providedInfo.position.X / 16f) + j, num5] != null)
					{
						_points.Add(new Point((int)(providedInfo.position.X / 16f) + j, num5));
					}
				}
			}
			int num8 = (int)((providedInfo.mouse.X + (float)((width / 2 - 1) * num)) / 16f);
			int num9 = (int)(((double)providedInfo.mouse.Y + 0.1 - (double)(height / 2 + 1)) / 16.0);
			if (num3 == -1)
			{
				num9 = (int)((providedInfo.mouse.Y + (float)(height / 2) - 1f) / 16f);
			}
			if (providedInfo.player.gravDir == -1f && num2 == 0)
			{
				num9++;
			}
			if (num9 < 10)
			{
				num9 = 10;
			}
			if (num9 > Main.maxTilesY - 10)
			{
				num9 = Main.maxTilesY - 10;
			}
			int num10 = width / 16 + ((width % 16 != 0) ? 1 : 0);
			int num11 = height / 16 + ((height % 16 != 0) ? 1 : 0);
			if (num != 0)
			{
				for (int k = 0; k < num11; k++)
				{
					if (Main.tile[num8, num9 + k * num3] != null)
					{
						_endpoints.Add(new Point(num8, num9 + k * num3));
					}
				}
			}
			if (num2 != 0)
			{
				for (int l = 0; l < num10; l++)
				{
					if (Main.tile[(int)((providedInfo.mouse.X - (float)(width / 2)) / 16f) + l, num9] != null)
					{
						_endpoints.Add(new Point((int)((providedInfo.mouse.X - (float)(width / 2)) / 16f) + l, num9));
					}
				}
			}
			_targets.Clear();
			while (_points.Count > 0)
			{
				Point val2 = _points[0];
				Point val3 = _endpoints[0];
				Point val4 = Collision.HitLineWall(val2.X, val2.Y, val3.X, val3.Y);
				if (val4.X == -1 || val4.Y == -1)
				{
					_points.Remove(val2);
					_endpoints.Remove(val3);
					continue;
				}
				if (val4.X != val3.X || val4.Y != val3.Y)
				{
					_targets.Add(val4);
				}
				_ = Main.tile[val4.X, val4.Y];
				if (Collision.HitWallSubstep(val4.X, val4.Y))
				{
					_targets.Add(val4);
				}
				_points.Remove(val2);
				_endpoints.Remove(val3);
			}
			if (_targets.Count > 0)
			{
				float num12 = -1f;
				Point val5 = new Point(-1, -1);
				for (int m = 0; m < _targets.Count; m++)
				{
					if (!Main.tile[_targets[m].X, _targets[m].Y].active() || Main.tile[_targets[m].X, _targets[m].Y].type != 26)
					{
						float num13 = Vector2.Distance(new Vector2((float)_targets[m].X, (float)_targets[m].Y) * 16f + Vector2.One * 8f, providedInfo.Center);
						if (num12 == -1f || num13 < num12)
						{
							num12 = num13;
							val5 = _targets[m];
						}
					}
				}
				if (val5.X != -1 && Collision.InTileBounds(val5.X, val5.Y, providedInfo.reachableStartX, providedInfo.reachableStartY, providedInfo.reachableEndX, providedInfo.reachableEndY))
				{
					providedInfo.player.poundRelease = false;
					focusedX = val5.X;
					focusedY = val5.Y;
				}
			}
			_targets.Clear();
			_points.Clear();
			_endpoints.Clear();
		}
		if (providedInfo.item.hammer <= 0 || focusedX != -1 || focusedY != -1)
		{
			return;
		}
		_targets.Clear();
		for (int n = providedInfo.reachableStartX; n <= providedInfo.reachableEndX; n++)
		{
			for (int num14 = providedInfo.reachableStartY; num14 <= providedInfo.reachableEndY; num14++)
			{
				if (Main.tile[n, num14].wall > 0 && Collision.HitWallSubstep(n, num14))
				{
					_targets.Add(new Point(n, num14));
				}
			}
		}
		if (_targets.Count > 0)
		{
			float num15 = -1f;
			Point val6 = new Point(-1, -1);
			for (int num16 = 0; num16 < _targets.Count; num16++)
			{
				if (!Main.tile[_targets[num16].X, _targets[num16].Y].active() || Main.tile[_targets[num16].X, _targets[num16].Y].type != 26)
				{
					float num17 = Vector2.Distance(new Vector2((float)_targets[num16].X, (float)_targets[num16].Y) * 16f + Vector2.One * 8f, providedInfo.mouse);
					if (num15 == -1f || num17 < num15)
					{
						num15 = num17;
						val6 = _targets[num16];
					}
				}
			}
			if (val6.X != -1 && Collision.InTileBounds(val6.X, val6.Y, providedInfo.reachableStartX, providedInfo.reachableStartY, providedInfo.reachableEndX, providedInfo.reachableEndY))
			{
				providedInfo.player.poundRelease = false;
				focusedX = val6.X;
				focusedY = val6.Y;
			}
		}
		_targets.Clear();
	}

	private static void Step_MulticolorWrench(SmartCursorUsageInfo providedInfo, ref int focusedX, ref int focusedY)
	{
		//IL_04bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_04eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_04fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0504: Unknown result type (might be due to invalid IL or missing references)
		//IL_0509: Unknown result type (might be due to invalid IL or missing references)
		//IL_050f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0553: Unknown result type (might be due to invalid IL or missing references)
		//IL_055a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0535: Unknown result type (might be due to invalid IL or missing references)
		//IL_053a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0581: Unknown result type (might be due to invalid IL or missing references)
		//IL_058a: Unknown result type (might be due to invalid IL or missing references)
		//IL_015e: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_027c: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0333: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_021e: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_035f: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_024a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0416: Unknown result type (might be due to invalid IL or missing references)
		//IL_038b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0300: Unknown result type (might be due to invalid IL or missing references)
		//IL_0442: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_046e: Unknown result type (might be due to invalid IL or missing references)
		if (providedInfo.item.type != 3625 || focusedX != -1 || focusedY != -1)
		{
			return;
		}
		_targets.Clear();
		WiresUI.Settings.MultiToolMode toolMode = WiresUI.Settings.ToolMode;
		WiresUI.Settings.MultiToolMode multiToolMode = (WiresUI.Settings.MultiToolMode)0;
		if (Main.tile[providedInfo.screenTargetX, providedInfo.screenTargetY].wire())
		{
			multiToolMode |= WiresUI.Settings.MultiToolMode.Red;
		}
		if (Main.tile[providedInfo.screenTargetX, providedInfo.screenTargetY].wire2())
		{
			multiToolMode |= WiresUI.Settings.MultiToolMode.Blue;
		}
		if (Main.tile[providedInfo.screenTargetX, providedInfo.screenTargetY].wire3())
		{
			multiToolMode |= WiresUI.Settings.MultiToolMode.Green;
		}
		if (Main.tile[providedInfo.screenTargetX, providedInfo.screenTargetY].wire4())
		{
			multiToolMode |= WiresUI.Settings.MultiToolMode.Yellow;
		}
		toolMode &= ~WiresUI.Settings.MultiToolMode.Cutter;
		bool flag = toolMode == multiToolMode;
		toolMode = WiresUI.Settings.ToolMode;
		if (!flag)
		{
			bool flag2 = (toolMode & WiresUI.Settings.MultiToolMode.Red) != 0;
			bool flag3 = (toolMode & WiresUI.Settings.MultiToolMode.Blue) != 0;
			bool flag4 = (toolMode & WiresUI.Settings.MultiToolMode.Green) != 0;
			bool flag5 = (toolMode & WiresUI.Settings.MultiToolMode.Yellow) != 0;
			bool flag6 = (toolMode & WiresUI.Settings.MultiToolMode.Cutter) != 0;
			for (int i = providedInfo.reachableStartX; i <= providedInfo.reachableEndX; i++)
			{
				for (int j = providedInfo.reachableStartY; j <= providedInfo.reachableEndY; j++)
				{
					Tile tile = Main.tile[i, j];
					if (flag6)
					{
						if ((tile.wire() & flag2) || (tile.wire2() & flag3) || (tile.wire3() & flag4) || (tile.wire4() & flag5))
						{
							_targets.Add(new Point(i, j));
						}
					}
					else
					{
						if (!(tile.wire() & flag2) && !(tile.wire2() & flag3) && !(tile.wire3() & flag4) && !(tile.wire4() & flag5))
						{
							continue;
						}
						if (flag2)
						{
							if (!Main.tile[i - 1, j].wire())
							{
								_targets.Add(new Point(i - 1, j));
							}
							if (!Main.tile[i + 1, j].wire())
							{
								_targets.Add(new Point(i + 1, j));
							}
							if (!Main.tile[i, j - 1].wire())
							{
								_targets.Add(new Point(i, j - 1));
							}
							if (!Main.tile[i, j + 1].wire())
							{
								_targets.Add(new Point(i, j + 1));
							}
						}
						if (flag3)
						{
							if (!Main.tile[i - 1, j].wire2())
							{
								_targets.Add(new Point(i - 1, j));
							}
							if (!Main.tile[i + 1, j].wire2())
							{
								_targets.Add(new Point(i + 1, j));
							}
							if (!Main.tile[i, j - 1].wire2())
							{
								_targets.Add(new Point(i, j - 1));
							}
							if (!Main.tile[i, j + 1].wire2())
							{
								_targets.Add(new Point(i, j + 1));
							}
						}
						if (flag4)
						{
							if (!Main.tile[i - 1, j].wire3())
							{
								_targets.Add(new Point(i - 1, j));
							}
							if (!Main.tile[i + 1, j].wire3())
							{
								_targets.Add(new Point(i + 1, j));
							}
							if (!Main.tile[i, j - 1].wire3())
							{
								_targets.Add(new Point(i, j - 1));
							}
							if (!Main.tile[i, j + 1].wire3())
							{
								_targets.Add(new Point(i, j + 1));
							}
						}
						if (flag5)
						{
							if (!Main.tile[i - 1, j].wire4())
							{
								_targets.Add(new Point(i - 1, j));
							}
							if (!Main.tile[i + 1, j].wire4())
							{
								_targets.Add(new Point(i + 1, j));
							}
							if (!Main.tile[i, j - 1].wire4())
							{
								_targets.Add(new Point(i, j - 1));
							}
							if (!Main.tile[i, j + 1].wire4())
							{
								_targets.Add(new Point(i, j + 1));
							}
						}
					}
				}
			}
		}
		if (_targets.Count > 0)
		{
			float num = -1f;
			Point val = _targets[0];
			for (int k = 0; k < _targets.Count; k++)
			{
				float num2 = Vector2.Distance(new Vector2((float)_targets[k].X, (float)_targets[k].Y) * 16f + Vector2.One * 8f, providedInfo.mouse);
				if (num == -1f || num2 < num)
				{
					num = num2;
					val = _targets[k];
				}
			}
			if (Collision.InTileBounds(val.X, val.Y, providedInfo.reachableStartX, providedInfo.reachableStartY, providedInfo.reachableEndX, providedInfo.reachableEndY))
			{
				focusedX = val.X;
				focusedY = val.Y;
			}
		}
		_targets.Clear();
	}

	private static void Step_ColoredWrenches(SmartCursorUsageInfo providedInfo, ref int focusedX, ref int focusedY)
	{
		//IL_0484: Unknown result type (might be due to invalid IL or missing references)
		//IL_0489: Unknown result type (might be due to invalid IL or missing references)
		//IL_0497: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_04be: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_04cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_051c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0523: Unknown result type (might be due to invalid IL or missing references)
		//IL_04fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0503: Unknown result type (might be due to invalid IL or missing references)
		//IL_054a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0553: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0277: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_031e: Unknown result type (might be due to invalid IL or missing references)
		//IL_029f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0220: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0346: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0248: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_036e: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_0415: Unknown result type (might be due to invalid IL or missing references)
		//IL_0396: Unknown result type (might be due to invalid IL or missing references)
		//IL_043d: Unknown result type (might be due to invalid IL or missing references)
		if ((providedInfo.item.type != 509 && providedInfo.item.type != 850 && providedInfo.item.type != 851 && providedInfo.item.type != 3612) || focusedX != -1 || focusedY != -1)
		{
			return;
		}
		_targets.Clear();
		int num = 0;
		if (providedInfo.item.type == 509)
		{
			num = 1;
		}
		if (providedInfo.item.type == 850)
		{
			num = 2;
		}
		if (providedInfo.item.type == 851)
		{
			num = 3;
		}
		if (providedInfo.item.type == 3612)
		{
			num = 4;
		}
		bool flag = false;
		if (Main.tile[providedInfo.screenTargetX, providedInfo.screenTargetY].wire() && num == 1)
		{
			flag = true;
		}
		if (Main.tile[providedInfo.screenTargetX, providedInfo.screenTargetY].wire2() && num == 2)
		{
			flag = true;
		}
		if (Main.tile[providedInfo.screenTargetX, providedInfo.screenTargetY].wire3() && num == 3)
		{
			flag = true;
		}
		if (Main.tile[providedInfo.screenTargetX, providedInfo.screenTargetY].wire4() && num == 4)
		{
			flag = true;
		}
		if (!flag)
		{
			for (int i = providedInfo.reachableStartX; i <= providedInfo.reachableEndX; i++)
			{
				for (int j = providedInfo.reachableStartY; j <= providedInfo.reachableEndY; j++)
				{
					Tile tile = Main.tile[i, j];
					if ((!tile.wire() || num != 1) && (!tile.wire2() || num != 2) && (!tile.wire3() || num != 3) && (!tile.wire4() || num != 4))
					{
						continue;
					}
					if (num == 1)
					{
						if (!Main.tile[i - 1, j].wire())
						{
							_targets.Add(new Point(i - 1, j));
						}
						if (!Main.tile[i + 1, j].wire())
						{
							_targets.Add(new Point(i + 1, j));
						}
						if (!Main.tile[i, j - 1].wire())
						{
							_targets.Add(new Point(i, j - 1));
						}
						if (!Main.tile[i, j + 1].wire())
						{
							_targets.Add(new Point(i, j + 1));
						}
					}
					if (num == 2)
					{
						if (!Main.tile[i - 1, j].wire2())
						{
							_targets.Add(new Point(i - 1, j));
						}
						if (!Main.tile[i + 1, j].wire2())
						{
							_targets.Add(new Point(i + 1, j));
						}
						if (!Main.tile[i, j - 1].wire2())
						{
							_targets.Add(new Point(i, j - 1));
						}
						if (!Main.tile[i, j + 1].wire2())
						{
							_targets.Add(new Point(i, j + 1));
						}
					}
					if (num == 3)
					{
						if (!Main.tile[i - 1, j].wire3())
						{
							_targets.Add(new Point(i - 1, j));
						}
						if (!Main.tile[i + 1, j].wire3())
						{
							_targets.Add(new Point(i + 1, j));
						}
						if (!Main.tile[i, j - 1].wire3())
						{
							_targets.Add(new Point(i, j - 1));
						}
						if (!Main.tile[i, j + 1].wire3())
						{
							_targets.Add(new Point(i, j + 1));
						}
					}
					if (num == 4)
					{
						if (!Main.tile[i - 1, j].wire4())
						{
							_targets.Add(new Point(i - 1, j));
						}
						if (!Main.tile[i + 1, j].wire4())
						{
							_targets.Add(new Point(i + 1, j));
						}
						if (!Main.tile[i, j - 1].wire4())
						{
							_targets.Add(new Point(i, j - 1));
						}
						if (!Main.tile[i, j + 1].wire4())
						{
							_targets.Add(new Point(i, j + 1));
						}
					}
				}
			}
		}
		if (_targets.Count > 0)
		{
			float num2 = -1f;
			Point val = _targets[0];
			for (int k = 0; k < _targets.Count; k++)
			{
				float num3 = Vector2.Distance(new Vector2((float)_targets[k].X, (float)_targets[k].Y) * 16f + Vector2.One * 8f, providedInfo.mouse);
				if (num2 == -1f || num3 < num2)
				{
					num2 = num3;
					val = _targets[k];
				}
			}
			if (Collision.InTileBounds(val.X, val.Y, providedInfo.reachableStartX, providedInfo.reachableStartY, providedInfo.reachableEndX, providedInfo.reachableEndY))
			{
				focusedX = val.X;
				focusedY = val.Y;
			}
		}
		_targets.Clear();
	}

	private static void Step_Acorns(SmartCursorUsageInfo providedInfo, ref int focusedX, ref int focusedY)
	{
		//IL_03c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_03dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_053c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0541: Unknown result type (might be due to invalid IL or missing references)
		//IL_054f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0561: Unknown result type (might be due to invalid IL or missing references)
		//IL_056c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0576: Unknown result type (might be due to invalid IL or missing references)
		//IL_057b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0585: Unknown result type (might be due to invalid IL or missing references)
		//IL_058a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0590: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_05db: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_05bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0602: Unknown result type (might be due to invalid IL or missing references)
		//IL_060b: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0373: Unknown result type (might be due to invalid IL or missing references)
		//IL_0341: Unknown result type (might be due to invalid IL or missing references)
		int num = 9;
		int num2 = 14;
		int num3 = 20;
		if (providedInfo.item.type != 27 || focusedX != -1 || focusedY != -1 || providedInfo.reachableStartY <= 20)
		{
			return;
		}
		_targets.Clear();
		for (int i = providedInfo.reachableStartX; i <= providedInfo.reachableEndX; i++)
		{
			for (int j = providedInfo.reachableStartY; j <= providedInfo.reachableEndY; j++)
			{
				Tile tile = Main.tile[i, j];
				Tile tile2 = Main.tile[i, j - 1];
				Tile tile3 = Main.tile[i, j + 1];
				Tile tile4 = Main.tile[i - 1, j];
				Tile tile5 = Main.tile[i + 1, j];
				Tile tile6 = Main.tile[i - 2, j];
				Tile tile7 = Main.tile[i + 2, j];
				Tile tile8 = Main.tile[i - 3, j];
				Tile tile9 = Main.tile[i + 3, j];
				if ((tile.active() && !Main.tileCut[tile.type] && !TileID.Sets.BreakableWhenPlacing[tile.type]) || (tile2.active() && !Main.tileCut[tile2.type] && !TileID.Sets.BreakableWhenPlacing[tile2.type]) || !tile3.active() || !WorldGen.SolidTile2(tile3))
				{
					continue;
				}
				bool flag = (tile4.active() && TileID.Sets.CommonSapling[tile4.type]) || (tile5.active() && TileID.Sets.CommonSapling[tile5.type]);
				bool flag2 = flag || (tile6.active() && TileID.Sets.CommonSapling[tile6.type]) || (tile7.active() && TileID.Sets.CommonSapling[tile7.type]) || (tile8.active() && TileID.Sets.CommonSapling[tile8.type]) || (tile9.active() && TileID.Sets.CommonSapling[tile9.type]);
				switch (tile3.type)
				{
				case 60:
					if (!flag2 && WorldGen.EmptyTileCheck(i - 2, i + 2, j - num2 + 1, j, 20))
					{
						_targets.Add(new Point(i, j));
					}
					break;
				case 2:
				case 23:
				case 109:
				case 147:
				case 199:
				case 477:
				case 492:
				case 633:
				case 661:
				case 662:
					if (!flag2 && tile4.liquid == 0 && tile.liquid == 0 && tile5.liquid == 0 && WorldGen.EmptyTileCheck(i - 2, i + 2, j - num + 1, j, 20))
					{
						_targets.Add(new Point(i, j));
					}
					break;
				case 53:
				case 112:
				case 116:
				case 234:
					if (!flag && tile.liquid == 0 && WorldGen.EmptyTileCheck(i, i, j - num3, j, 20))
					{
						_targets.Add(new Point(i, j));
					}
					break;
				}
			}
		}
		_toRemove.Clear();
		for (int k = 0; k < _targets.Count; k++)
		{
			bool flag3 = false;
			for (int l = -1; l < 2; l += 2)
			{
				Tile tile10 = Main.tile[_targets[k].X + l, _targets[k].Y + 1];
				if (tile10.active())
				{
					switch (tile10.type)
					{
					case 2:
					case 23:
					case 53:
					case 60:
					case 109:
					case 112:
					case 116:
					case 147:
					case 199:
					case 234:
					case 477:
					case 492:
					case 633:
					case 661:
					case 662:
						flag3 = true;
						break;
					}
				}
			}
			if (!flag3)
			{
				_toRemove.Add(_targets[k]);
			}
		}
		for (int m = 0; m < _toRemove.Count; m++)
		{
			_targets.Remove(_toRemove[m]);
		}
		_toRemove.Clear();
		if (_targets.Count > 0)
		{
			float num4 = -1f;
			Point val = _targets[0];
			for (int n = 0; n < _targets.Count; n++)
			{
				float num5 = Vector2.Distance(new Vector2((float)_targets[n].X, (float)_targets[n].Y) * 16f + Vector2.One * 8f, providedInfo.mouse);
				if (num4 == -1f || num5 < num4)
				{
					num4 = num5;
					val = _targets[n];
				}
			}
			if (Collision.InTileBounds(val.X, val.Y, providedInfo.reachableStartX, providedInfo.reachableStartY, providedInfo.reachableEndX, providedInfo.reachableEndY))
			{
				focusedX = val.X;
				focusedY = val.Y;
			}
		}
		_targets.Clear();
	}

	private static void Step_GemCorns(SmartCursorUsageInfo providedInfo, ref int focusedX, ref int focusedY)
	{
		//IL_0284: Unknown result type (might be due to invalid IL or missing references)
		//IL_0298: Unknown result type (might be due to invalid IL or missing references)
		//IL_0318: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_035e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0363: Unknown result type (might be due to invalid IL or missing references)
		//IL_0371: Unknown result type (might be due to invalid IL or missing references)
		//IL_0383: Unknown result type (might be due to invalid IL or missing references)
		//IL_038e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0398: Unknown result type (might be due to invalid IL or missing references)
		//IL_039d: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_03dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0424: Unknown result type (might be due to invalid IL or missing references)
		//IL_042d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0234: Unknown result type (might be due to invalid IL or missing references)
		if (!WorldGen.GrowTreeSettings.Profiles.TryGetFromItemId(providedInfo.item.type, out var profile) || focusedX != -1 || focusedY != -1 || providedInfo.reachableStartY <= 20)
		{
			return;
		}
		_targets.Clear();
		for (int i = providedInfo.reachableStartX; i <= providedInfo.reachableEndX; i++)
		{
			for (int j = providedInfo.reachableStartY; j <= providedInfo.reachableEndY; j++)
			{
				Tile tile = Main.tile[i, j];
				Tile tile2 = Main.tile[i, j - 1];
				Tile tile3 = Main.tile[i, j + 1];
				Tile tile4 = Main.tile[i - 1, j];
				Tile tile5 = Main.tile[i + 1, j];
				Tile tile6 = Main.tile[i - 2, j];
				Tile tile7 = Main.tile[i + 2, j];
				Tile tile8 = Main.tile[i - 3, j];
				Tile tile9 = Main.tile[i + 3, j];
				if (profile.GroundTest(tile3.type) && (!tile.active() || Main.tileCut[tile.type] || TileID.Sets.BreakableWhenPlacing[tile.type]) && (!tile2.active() || Main.tileCut[tile2.type] || TileID.Sets.BreakableWhenPlacing[tile2.type]) && (!tile4.active() || !TileID.Sets.CommonSapling[tile4.type]) && (!tile5.active() || !TileID.Sets.CommonSapling[tile5.type]) && (!tile6.active() || !TileID.Sets.CommonSapling[tile6.type]) && (!tile7.active() || !TileID.Sets.CommonSapling[tile7.type]) && (!tile8.active() || !TileID.Sets.CommonSapling[tile8.type]) && (!tile9.active() || !TileID.Sets.CommonSapling[tile9.type]) && tile3.active() && WorldGen.SolidTile2(tile3) && tile4.liquid == 0 && tile.liquid == 0 && tile5.liquid == 0 && WorldGen.EmptyTileCheck(i - 2, i + 2, j - profile.TreeHeightMax, j, profile.SaplingTileType))
				{
					_targets.Add(new Point(i, j));
				}
			}
		}
		_toRemove.Clear();
		for (int k = 0; k < _targets.Count; k++)
		{
			bool flag = false;
			for (int l = -1; l < 2; l += 2)
			{
				Tile tile10 = Main.tile[_targets[k].X + l, _targets[k].Y + 1];
				if (tile10.active() && profile.GroundTest(tile10.type))
				{
					flag = true;
				}
			}
			if (!flag)
			{
				_toRemove.Add(_targets[k]);
			}
		}
		for (int m = 0; m < _toRemove.Count; m++)
		{
			_targets.Remove(_toRemove[m]);
		}
		_toRemove.Clear();
		if (_targets.Count > 0)
		{
			float num = -1f;
			Point val = _targets[0];
			for (int n = 0; n < _targets.Count; n++)
			{
				float num2 = Vector2.Distance(new Vector2((float)_targets[n].X, (float)_targets[n].Y) * 16f + Vector2.One * 8f, providedInfo.mouse);
				if (num == -1f || num2 < num)
				{
					num = num2;
					val = _targets[n];
				}
			}
			if (Collision.InTileBounds(val.X, val.Y, providedInfo.reachableStartX, providedInfo.reachableStartY, providedInfo.reachableEndX, providedInfo.reachableEndY))
			{
				focusedX = val.X;
				focusedY = val.Y;
			}
		}
		_targets.Clear();
	}

	private static void Step_ForceCursorToAnyMinableThing(SmartCursorUsageInfo providedInfo, ref int fX, ref int fY)
	{
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		int reachableStartX = providedInfo.reachableStartX;
		int reachableStartY = providedInfo.reachableStartY;
		int reachableEndX = providedInfo.reachableEndX;
		int reachableEndY = providedInfo.reachableEndY;
		_ = providedInfo.screenTargetX;
		_ = providedInfo.screenTargetY;
		Vector2 mouse = providedInfo.mouse;
		Item item = providedInfo.item;
		if (fX != -1 || fY != -1 || PlayerInput.UsingGamepad)
		{
			return;
		}
		Point val = mouse.ToTileCoordinates();
		int x = val.X;
		int y = val.Y;
		if (Collision.InTileBounds(x, y, reachableStartX, reachableStartY, reachableEndX, reachableEndY))
		{
			Tile tile = Main.tile[x, y];
			bool flag = tile.active() && WorldGen.CanKillTile(x, y) && (!Main.tileSolid[tile.type] || Main.tileSolidTop[tile.type]);
			if (flag && Main.tileAxe[tile.type] && item.axe < 1)
			{
				flag = false;
			}
			if (flag && Main.tileHammer[tile.type] && item.hammer < 1)
			{
				flag = false;
			}
			if (flag && !Main.tileHammer[tile.type] && !Main.tileAxe[tile.type] && item.pick < 1)
			{
				flag = false;
			}
			if (flag)
			{
				fX = x;
				fY = y;
			}
		}
	}

	public static void Step_Pickaxe_MineShinies(SmartCursorUsageInfo providedInfo, ref int fX, ref int fY)
	{
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0153: Unknown result type (might be due to invalid IL or missing references)
		//IL_0158: Unknown result type (might be due to invalid IL or missing references)
		//IL_015f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0166: Unknown result type (might be due to invalid IL or missing references)
		//IL_018a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0113: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0204: Unknown result type (might be due to invalid IL or missing references)
		//IL_0209: Unknown result type (might be due to invalid IL or missing references)
		//IL_0217: Unknown result type (might be due to invalid IL or missing references)
		//IL_0229: Unknown result type (might be due to invalid IL or missing references)
		//IL_0234: Unknown result type (might be due to invalid IL or missing references)
		//IL_023e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0243: Unknown result type (might be due to invalid IL or missing references)
		//IL_024d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0252: Unknown result type (might be due to invalid IL or missing references)
		//IL_0257: Unknown result type (might be due to invalid IL or missing references)
		//IL_0298: Unknown result type (might be due to invalid IL or missing references)
		//IL_029f: Unknown result type (might be due to invalid IL or missing references)
		//IL_027a: Unknown result type (might be due to invalid IL or missing references)
		//IL_027f: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bb: Unknown result type (might be due to invalid IL or missing references)
		int reachableStartX = providedInfo.reachableStartX;
		int reachableStartY = providedInfo.reachableStartY;
		int reachableEndX = providedInfo.reachableEndX;
		int reachableEndY = providedInfo.reachableEndY;
		_ = providedInfo.screenTargetX;
		_ = providedInfo.screenTargetY;
		Item item = providedInfo.item;
		Vector2 mouse = providedInfo.mouse;
		if (item.pick <= 0 || fX != -1 || fY != -1)
		{
			return;
		}
		_targets.Clear();
		if (item.type != 1333 && item.type != 523)
		{
			_ = item.type != 4384;
		}
		else
			_ = 0;
		int num = 0;
		for (int i = reachableStartX; i <= reachableEndX; i++)
		{
			for (int j = reachableStartY; j <= reachableEndY; j++)
			{
				Tile tile = Main.tile[i, j];
				_ = Main.tile[i - 1, j];
				_ = Main.tile[i + 1, j];
				_ = Main.tile[i, j + 1];
				if (!tile.active())
				{
					continue;
				}
				int num2 = (num2 = TileID.Sets.SmartCursorPickaxePriorityOverride[tile.type]);
				if (num2 > 0)
				{
					if (num < num2)
					{
						num = num2;
					}
					_targets.Add(new Point(i, j));
				}
			}
		}
		_targets2.Clear();
		foreach (Point item2 in _targets2)
		{
			Tile tile2 = Main.tile[item2.X, item2.Y];
			if (TileID.Sets.SmartCursorPickaxePriorityOverride[tile2.type] < num)
			{
				_targets2.Add(item2);
			}
		}
		foreach (Point item3 in _targets2)
		{
			_targets.Remove(item3);
		}
		if (_targets.Count > 0)
		{
			float num3 = -1f;
			Point val = _targets[0];
			for (int k = 0; k < _targets.Count; k++)
			{
				float num4 = Vector2.Distance(new Vector2((float)_targets[k].X, (float)_targets[k].Y) * 16f + Vector2.One * 8f, mouse);
				if (num3 == -1f || num4 < num3)
				{
					num3 = num4;
					val = _targets[k];
				}
			}
			if (Collision.InTileBounds(val.X, val.Y, reachableStartX, reachableStartY, reachableEndX, reachableEndY))
			{
				fX = val.X;
				fY = val.Y;
			}
		}
		_targets.Clear();
	}

	public static void Step_Pickaxe_MineSolids(Player player, Vector2 position, Vector2 Center, int width, int direction, SmartCursorUsageInfo providedInfo, List<Point> grappleTargets, ref int focusedX, ref int focusedY)
	{
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0100: Unknown result type (might be due to invalid IL or missing references)
		//IL_010b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0133: Unknown result type (might be due to invalid IL or missing references)
		//IL_013e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0128: Unknown result type (might be due to invalid IL or missing references)
		//IL_016c: Unknown result type (might be due to invalid IL or missing references)
		//IL_018a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_0239: Unknown result type (might be due to invalid IL or missing references)
		//IL_0215: Unknown result type (might be due to invalid IL or missing references)
		//IL_0257: Unknown result type (might be due to invalid IL or missing references)
		//IL_0269: Unknown result type (might be due to invalid IL or missing references)
		//IL_0427: Unknown result type (might be due to invalid IL or missing references)
		//IL_042c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0434: Unknown result type (might be due to invalid IL or missing references)
		//IL_0439: Unknown result type (might be due to invalid IL or missing references)
		//IL_043b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0442: Unknown result type (might be due to invalid IL or missing references)
		//IL_0449: Unknown result type (might be due to invalid IL or missing references)
		//IL_0450: Unknown result type (might be due to invalid IL or missing references)
		//IL_0566: Unknown result type (might be due to invalid IL or missing references)
		//IL_0577: Unknown result type (might be due to invalid IL or missing references)
		//IL_048c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0493: Unknown result type (might be due to invalid IL or missing references)
		//IL_0472: Unknown result type (might be due to invalid IL or missing references)
		//IL_047f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0392: Unknown result type (might be due to invalid IL or missing references)
		//IL_0594: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_049c: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_04bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0609: Unknown result type (might be due to invalid IL or missing references)
		//IL_060e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0610: Unknown result type (might be due to invalid IL or missing references)
		//IL_0611: Unknown result type (might be due to invalid IL or missing references)
		//IL_051e: Unknown result type (might be due to invalid IL or missing references)
		//IL_052b: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_07bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_07d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_07d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_07e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_07e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_082e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0835: Unknown result type (might be due to invalid IL or missing references)
		//IL_0504: Unknown result type (might be due to invalid IL or missing references)
		//IL_0810: Unknown result type (might be due to invalid IL or missing references)
		//IL_0815: Unknown result type (might be due to invalid IL or missing references)
		//IL_0861: Unknown result type (might be due to invalid IL or missing references)
		//IL_086b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0512: Unknown result type (might be due to invalid IL or missing references)
		//IL_0667: Unknown result type (might be due to invalid IL or missing references)
		//IL_0681: Unknown result type (might be due to invalid IL or missing references)
		//IL_0694: Unknown result type (might be due to invalid IL or missing references)
		//IL_069e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0704: Unknown result type (might be due to invalid IL or missing references)
		int height = player.height;
		float gravDir = player.gravDir;
		int whoAmI = player.whoAmI;
		if (providedInfo.item.pick <= 0 || focusedX != -1 || focusedY != -1)
		{
			return;
		}
		if (PlayerInput.UsingGamepad)
		{
			Vector2 navigatorDirections = PlayerInput.Triggers.Current.GetNavigatorDirections();
			Vector2 gamepadThumbstickLeft = PlayerInput.GamepadThumbstickLeft;
			Vector2 gamepadThumbstickRight = PlayerInput.GamepadThumbstickRight;
			if (navigatorDirections == Vector2.Zero && gamepadThumbstickLeft.Length() < 0.05f && gamepadThumbstickRight.Length() < 0.05f)
			{
				providedInfo.mouse = Center + new Vector2((float)(direction * 1000), 0f);
			}
		}
		Vector2 val = providedInfo.mouse - Center;
		int num = Math.Sign(val.X);
		int num2 = Math.Sign(val.Y);
		if (Math.Abs(val.X) > Math.Abs(val.Y) * 3f)
		{
			num2 = 0;
			providedInfo.mouse.Y = Center.Y;
		}
		if (Math.Abs(val.Y) > Math.Abs(val.X) * 3f)
		{
			num = 0;
			providedInfo.mouse.X = Center.X;
		}
		_ = (int)Center.X / 16;
		_ = (int)Center.Y / 16;
		_points.Clear();
		_endpoints.Clear();
		int num3 = 1;
		if (num2 == -1 && num != 0)
		{
			num3 = -1;
		}
		int num4 = (int)((position.X + (float)(width / 2) + (float)((width / 2 - 1) * num)) / 16f);
		int num5 = (int)(((double)position.Y + 0.1) / 16.0);
		if (num3 == -1)
		{
			num5 = (int)((position.Y + (float)height - 1f) / 16f);
		}
		int num6 = width / 16 + ((width % 16 != 0) ? 1 : 0);
		int num7 = height / 16 + ((height % 16 != 0) ? 1 : 0);
		if (num != 0)
		{
			for (int i = 0; i < num7; i++)
			{
				if (Main.tile[num4, num5 + i * num3] != null)
				{
					_points.Add(new Point(num4, num5 + i * num3));
				}
			}
		}
		if (num2 != 0)
		{
			for (int j = 0; j < num6; j++)
			{
				if (Main.tile[(int)(position.X / 16f) + j, num5] != null)
				{
					_points.Add(new Point((int)(position.X / 16f) + j, num5));
				}
			}
		}
		int num8 = (int)((providedInfo.mouse.X + (float)((width / 2 - 1) * num)) / 16f);
		int num9 = (int)(((double)providedInfo.mouse.Y + 0.1 - (double)(height / 2 + 1)) / 16.0);
		if (num3 == -1)
		{
			num9 = (int)((providedInfo.mouse.Y + (float)(height / 2) - 1f) / 16f);
		}
		if (gravDir == -1f && num2 == 0)
		{
			num9++;
		}
		if (gravDir == 1f && num == 0)
		{
			num9++;
		}
		if (num9 < 10)
		{
			num9 = 10;
		}
		if (num9 > Main.maxTilesY - 10)
		{
			num9 = Main.maxTilesY - 10;
		}
		int num10 = width / 16 + ((width % 16 != 0) ? 1 : 0);
		int num11 = height / 16 + ((height % 16 != 0) ? 1 : 0);
		if (WorldGen.InWorld(num8, num9, 40))
		{
			if (num != 0)
			{
				for (int k = 0; k < num11; k++)
				{
					if (Main.tile[num8, num9 + k * num3] != null)
					{
						_endpoints.Add(new Point(num8, num9 + k * num3));
					}
				}
			}
			if (num2 != 0)
			{
				for (int l = 0; l < num10; l++)
				{
					if (Main.tile[(int)((providedInfo.mouse.X - (float)(width / 2)) / 16f) + l, num9] != null)
					{
						_endpoints.Add(new Point((int)((providedInfo.mouse.X - (float)(width / 2)) / 16f) + l, num9));
					}
				}
			}
		}
		_targets.Clear();
		while (_points.Count > 0 && _endpoints.Count > 0)
		{
			Point val2 = _points[0];
			Point val3 = _endpoints[0];
			if (!Collision.HitLine(val2.X, val2.Y, val3.X, val3.Y, num * (int)gravDir, -num2 * (int)gravDir, grappleTargets, out var col))
			{
				_points.Remove(val2);
				_endpoints.Remove(val3);
				continue;
			}
			if (col.X != val3.X || col.Y != val3.Y)
			{
				_targets.Add(col);
			}
			Tile tile = Main.tile[col.X, col.Y];
			if (!tile.inActive() && tile.active() && Main.tileSolid[tile.type] && !Main.tileSolidTop[tile.type] && !grappleTargets.Contains(col))
			{
				_targets.Add(col);
			}
			_points.Remove(val2);
			_endpoints.Remove(val3);
		}
		_toRemove.Clear();
		for (int m = 0; m < _targets.Count; m++)
		{
			if (!WorldGen.CanKillTile(_targets[m].X, _targets[m].Y))
			{
				_toRemove.Add(_targets[m]);
			}
		}
		for (int n = 0; n < _toRemove.Count; n++)
		{
			_targets.Remove(_toRemove[n]);
		}
		_toRemove.Clear();
		if (_targets.Count > 0)
		{
			float num12 = -1f;
			Point val4 = _targets[0];
			Vector2 val5 = Center;
			if (Main.netMode == 1)
			{
				int num13 = 0;
				int num14 = 0;
				int num15 = 0;
				for (int num16 = 0; num16 < whoAmI; num16++)
				{
					Player player2 = Main.player[num16];
					if (player2.active && !player2.dead && player2.HeldItem.pick > 0 && player2.itemAnimation > 0)
					{
						if (player.Distance(player2.Center) <= 8f)
						{
							num13++;
						}
						if (player.Distance(player2.Center) <= 80f && Math.Abs(player2.Center.Y - Center.Y) <= 12f)
						{
							num14++;
						}
					}
				}
				for (int num17 = whoAmI + 1; num17 < 255; num17++)
				{
					Player player3 = Main.player[num17];
					if (player3.active && !player3.dead && player3.HeldItem.pick > 0 && player3.itemAnimation > 0 && player.Distance(player3.Center) <= 8f)
					{
						num15++;
					}
				}
				if (num13 > 0)
				{
					if (num13 % 2 == 1)
					{
						val5.X += 12f;
					}
					else
					{
						val5.X -= 12f;
					}
					if (num14 % 2 == 1)
					{
						val5.Y -= 12f;
					}
				}
				if (num15 > 0 && num13 == 0)
				{
					if (num15 % 2 == 1)
					{
						val5.X -= 12f;
					}
					else
					{
						val5.X += 12f;
					}
				}
			}
			for (int num18 = 0; num18 < _targets.Count; num18++)
			{
				float num19 = Vector2.Distance(new Vector2((float)_targets[num18].X, (float)_targets[num18].Y) * 16f + Vector2.One * 8f, val5);
				if (num12 == -1f || num19 < num12)
				{
					num12 = num19;
					val4 = _targets[num18];
				}
			}
			if (Collision.InTileBounds(val4.X, val4.Y, providedInfo.reachableStartX, providedInfo.reachableStartY, providedInfo.reachableEndX, providedInfo.reachableEndY))
			{
				focusedX = val4.X;
				focusedY = val4.Y;
			}
		}
		_points.Clear();
		_endpoints.Clear();
		_targets.Clear();
	}

	public static void Step_Axe(SmartCursorUsageInfo providedInfo, ref int fX, ref int fY)
	{
		//IL_04ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ee: Unknown result type (might be due to invalid IL or missing references)
		int reachableStartX = providedInfo.reachableStartX;
		int reachableStartY = providedInfo.reachableStartY;
		int reachableEndX = providedInfo.reachableEndX;
		int reachableEndY = providedInfo.reachableEndY;
		_ = providedInfo.screenTargetX;
		_ = providedInfo.screenTargetY;
		if (providedInfo.item.axe <= 0 || fX != -1 || fY != -1)
		{
			return;
		}
		float num = -1f;
		for (int i = reachableStartX; i <= reachableEndX; i++)
		{
			for (int j = reachableStartY; j <= reachableEndY; j++)
			{
				if (!Main.tile[i, j].active())
				{
					continue;
				}
				Tile tile = Main.tile[i, j];
				if (!Main.tileAxe[tile.type] || TileID.Sets.IgnoreSmartCursorPriorityAxe[tile.type])
				{
					continue;
				}
				int num2 = i;
				int k = j;
				int type = tile.type;
				if (TileID.Sets.IsATreeTrunk[type])
				{
					if (Collision.InTileBounds(num2 + 1, k, reachableStartX, reachableStartY, reachableEndX, reachableEndY))
					{
						if (Main.tile[num2, k].frameY >= 198 && Main.tile[num2, k].frameX == 44)
						{
							num2++;
						}
						if (Main.tile[num2, k].frameX == 66 && Main.tile[num2, k].frameY <= 44)
						{
							num2++;
						}
						if (Main.tile[num2, k].frameX == 44 && Main.tile[num2, k].frameY >= 132 && Main.tile[num2, k].frameY <= 176)
						{
							num2++;
						}
					}
					if (Collision.InTileBounds(num2 - 1, k, reachableStartX, reachableStartY, reachableEndX, reachableEndY))
					{
						if (Main.tile[num2, k].frameY >= 198 && Main.tile[num2, k].frameX == 66)
						{
							num2--;
						}
						if (Main.tile[num2, k].frameX == 88 && Main.tile[num2, k].frameY >= 66 && Main.tile[num2, k].frameY <= 110)
						{
							num2--;
						}
						if (Main.tile[num2, k].frameX == 22 && Main.tile[num2, k].frameY >= 132 && Main.tile[num2, k].frameY <= 176)
						{
							num2--;
						}
					}
					for (; Main.tile[num2, k].active() && Main.tile[num2, k].type == type && Main.tile[num2, k + 1].type == type && Collision.InTileBounds(num2, k + 1, reachableStartX, reachableStartY, reachableEndX, reachableEndY); k++)
					{
					}
				}
				if (tile.type == 80)
				{
					if (Collision.InTileBounds(num2 + 1, k, reachableStartX, reachableStartY, reachableEndX, reachableEndY))
					{
						if (Main.tile[num2, k].frameX == 54)
						{
							num2++;
						}
						if (Main.tile[num2, k].frameX == 108 && Main.tile[num2, k].frameY == 36)
						{
							num2++;
						}
					}
					if (Collision.InTileBounds(num2 - 1, k, reachableStartX, reachableStartY, reachableEndX, reachableEndY))
					{
						if (Main.tile[num2, k].frameX == 36)
						{
							num2--;
						}
						if (Main.tile[num2, k].frameX == 108 && Main.tile[num2, k].frameY == 18)
						{
							num2--;
						}
					}
					for (; Main.tile[num2, k].active() && Main.tile[num2, k].type == 80 && Main.tile[num2, k + 1].type == 80 && Collision.InTileBounds(num2, k + 1, reachableStartX, reachableStartY, reachableEndX, reachableEndY); k++)
					{
					}
				}
				if (tile.type == 323 || tile.type == 72)
				{
					for (; Main.tile[num2, k].active() && ((Main.tile[num2, k].type == 323 && Main.tile[num2, k + 1].type == 323) || (Main.tile[num2, k].type == 72 && Main.tile[num2, k + 1].type == 72)) && Collision.InTileBounds(num2, k + 1, reachableStartX, reachableStartY, reachableEndX, reachableEndY); k++)
					{
					}
				}
				float num3 = Vector2.Distance(new Vector2((float)num2, (float)k) * 16f + Vector2.One * 8f, providedInfo.mouse);
				if (num == -1f || num3 < num)
				{
					num = num3;
					fX = num2;
					fY = k;
				}
			}
		}
	}

	private static void Step_BlocksFilling(SmartCursorUsageInfo providedInfo, ref int fX, ref int fY)
	{
		//IL_0307: Unknown result type (might be due to invalid IL or missing references)
		//IL_030c: Unknown result type (might be due to invalid IL or missing references)
		//IL_031d: Unknown result type (might be due to invalid IL or missing references)
		//IL_032e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0403: Unknown result type (might be due to invalid IL or missing references)
		//IL_040a: Unknown result type (might be due to invalid IL or missing references)
		//IL_034a: Unknown result type (might be due to invalid IL or missing references)
		//IL_035c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0367: Unknown result type (might be due to invalid IL or missing references)
		//IL_0371: Unknown result type (might be due to invalid IL or missing references)
		//IL_0376: Unknown result type (might be due to invalid IL or missing references)
		//IL_0380: Unknown result type (might be due to invalid IL or missing references)
		//IL_0385: Unknown result type (might be due to invalid IL or missing references)
		//IL_038b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0390: Unknown result type (might be due to invalid IL or missing references)
		//IL_0395: Unknown result type (might be due to invalid IL or missing references)
		//IL_039a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0427: Unknown result type (might be due to invalid IL or missing references)
		//IL_0430: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bc: Unknown result type (might be due to invalid IL or missing references)
		if (!Player.SmartCursorSettings.SmartBlocksEnabled)
		{
			return;
		}
		int type = providedInfo.item.type;
		if (type < 0 || type >= ItemID.Count)
		{
			return;
		}
		int reachableStartX = providedInfo.reachableStartX;
		int reachableStartY = providedInfo.reachableStartY;
		int reachableEndX = providedInfo.reachableEndX;
		int reachableEndY = providedInfo.reachableEndY;
		int screenTargetX = providedInfo.screenTargetX;
		int screenTargetY = providedInfo.screenTargetY;
		if (Player.SmartCursorSettings.SmartBlocksEnabled || providedInfo.item.createTile <= -1 || !AllowNormalBlockPlacementBehaviourForItemType(type) || !Main.tileSolid[providedInfo.item.createTile] || Main.tileSolidTop[providedInfo.item.createTile] || Main.tileFrameImportant[providedInfo.item.createTile] || fX != -1 || fY != -1)
		{
			return;
		}
		_targets.Clear();
		bool flag = false;
		if (Main.tile[screenTargetX, screenTargetY].active())
		{
			flag = true;
		}
		if (!Collision.InTileBounds(screenTargetX, screenTargetY, reachableStartX, reachableStartY, reachableEndX, reachableEndY))
		{
			flag = true;
		}
		if (!flag)
		{
			for (int i = reachableStartX; i <= reachableEndX; i++)
			{
				for (int j = reachableStartY; j <= reachableEndY; j++)
				{
					Tile tile = Main.tile[i, j];
					if (!tile.active() || Main.tileCut[tile.type] || TileID.Sets.BreakableWhenPlacing[tile.type])
					{
						int num = 0;
						if (Main.tile[i - 1, j].active() && Main.tileSolid[Main.tile[i - 1, j].type] && !Main.tileSolidTop[Main.tile[i - 1, j].type])
						{
							num++;
						}
						if (Main.tile[i + 1, j].active() && Main.tileSolid[Main.tile[i + 1, j].type] && !Main.tileSolidTop[Main.tile[i + 1, j].type])
						{
							num++;
						}
						if (Main.tile[i, j - 1].active() && Main.tileSolid[Main.tile[i, j - 1].type] && !Main.tileSolidTop[Main.tile[i, j - 1].type])
						{
							num++;
						}
						if (Main.tile[i, j + 1].active() && Main.tileSolid[Main.tile[i, j + 1].type] && !Main.tileSolidTop[Main.tile[i, j + 1].type])
						{
							num++;
						}
						if (num >= 2)
						{
							_targets.Add(new Point(i, j));
						}
					}
				}
			}
		}
		if (_targets.Count > 0)
		{
			float num2 = -1f;
			float num3 = float.PositiveInfinity;
			Point val = _targets[0];
			for (int k = 0; k < _targets.Count; k++)
			{
				if (Collision.EmptyTile(_targets[k].X, _targets[k].Y, ignoreTiles: true))
				{
					Vector2 val2 = new Vector2((float)_targets[k].X, (float)_targets[k].Y) * 16f + Vector2.One * 8f - providedInfo.mouse;
					bool flag2 = false;
					float num4 = Math.Abs(val2.X);
					float num5 = val2.Length();
					if (num4 < num3)
					{
						flag2 = true;
					}
					if (num4 == num3 && (num2 == -1f || num5 < num2))
					{
						flag2 = true;
					}
					if (flag2)
					{
						num2 = num5;
						num3 = num4;
						val = _targets[k];
					}
				}
			}
			if (Collision.InTileBounds(val.X, val.Y, reachableStartX, reachableStartY, reachableEndX, reachableEndY) && num2 != -1f)
			{
				fX = val.X;
				fY = val.Y;
			}
		}
		_targets.Clear();
	}

	private static void Step_Torch(SmartCursorUsageInfo providedInfo, ref int fX, ref int fY)
	{
		//IL_01ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01df: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0203: Unknown result type (might be due to invalid IL or missing references)
		//IL_0208: Unknown result type (might be due to invalid IL or missing references)
		//IL_020e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0252: Unknown result type (might be due to invalid IL or missing references)
		//IL_0259: Unknown result type (might be due to invalid IL or missing references)
		//IL_0234: Unknown result type (might be due to invalid IL or missing references)
		//IL_0239: Unknown result type (might be due to invalid IL or missing references)
		//IL_026c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0275: Unknown result type (might be due to invalid IL or missing references)
		//IL_0177: Unknown result type (might be due to invalid IL or missing references)
		int reachableStartX = providedInfo.reachableStartX;
		int reachableStartY = providedInfo.reachableStartY;
		int reachableEndX = providedInfo.reachableEndX;
		int reachableEndY = providedInfo.reachableEndY;
		_ = providedInfo.screenTargetX;
		_ = providedInfo.screenTargetY;
		int type = providedInfo.item.type;
		if (type < 0 || type >= ItemID.Count || !ItemID.Sets.Torches[type] || fX != -1 || fY != -1)
		{
			return;
		}
		_targets.Clear();
		bool flag = !ItemID.Sets.WaterTorches[type];
		for (int i = reachableStartX; i <= reachableEndX; i++)
		{
			for (int j = reachableStartY; j <= reachableEndY; j++)
			{
				Tile tile = Main.tile[i, j];
				if ((flag && tile.liquid > 0) || (tile.active() && !TileID.Sets.BreakableWhenPlacing[tile.type] && (!Main.tileCut[tile.type] || tile.type == 82 || tile.type == 83)))
				{
					continue;
				}
				bool flag2 = false;
				for (int k = i - 8; k <= i + 8; k++)
				{
					for (int l = j - 8; l <= j + 8; l++)
					{
						if (Main.tile[k, l] != null)
						{
							Tile tile2 = Main.tile[k, l];
							if (TileID.Sets.Torches[tile2.type])
							{
								flag2 = true;
								break;
							}
						}
					}
					if (flag2)
					{
						break;
					}
				}
				if (!flag2 && IsValidSpotForTorch(i, j, tile))
				{
					_targets.Add(new Point(i, j));
				}
			}
		}
		if (_targets.Count > 0)
		{
			float num = -1f;
			Point val = _targets[0];
			for (int m = 0; m < _targets.Count; m++)
			{
				float num2 = Vector2.Distance(new Vector2((float)_targets[m].X, (float)_targets[m].Y) * 16f + Vector2.One * 8f, providedInfo.mouse);
				if (num == -1f || num2 < num)
				{
					num = num2;
					val = _targets[m];
				}
			}
			if (Collision.InTileBounds(val.X, val.Y, reachableStartX, reachableStartY, reachableEndX, reachableEndY))
			{
				fX = val.X;
				fY = val.Y;
			}
		}
		_targets.Clear();
	}

	private static bool IsValidSpotForTorch(int x, int y, Tile tileCache)
	{
		if (tileCache.wall > 0)
		{
			return true;
		}
		if (TileID.Sets.Torches[tileCache.type])
		{
			return false;
		}
		Tile tile = Main.tile[x - 1, y];
		if (tile.active() && (tile.slope() == 0 || tile.slope() % 2 != 1) && ((Main.tileSolid[tile.type] && !Main.tileNoAttach[tile.type] && !Main.tileSolidTop[tile.type] && !TileID.Sets.NotReallySolid[tile.type]) || TileID.Sets.IsBeam[tile.type] || (WorldGen.IsTreeType(tile.type) && WorldGen.IsTreeType(Main.tile[x - 1, y - 1].type) && WorldGen.IsTreeType(Main.tile[x - 1, y + 1].type))))
		{
			return true;
		}
		Tile tile2 = Main.tile[x + 1, y];
		if (tile2.active() && (tile2.slope() == 0 || tile2.slope() % 2 != 0) && ((Main.tileSolid[tile2.type] && !Main.tileNoAttach[tile2.type] && !Main.tileSolidTop[tile2.type] && !TileID.Sets.NotReallySolid[tile2.type]) || TileID.Sets.IsBeam[tile2.type] || (WorldGen.IsTreeType(tile2.type) && WorldGen.IsTreeType(Main.tile[x + 1, y - 1].type) && WorldGen.IsTreeType(Main.tile[x + 1, y + 1].type))))
		{
			return true;
		}
		Tile tile3 = Main.tile[x, y + 1];
		if (tile3.active() && tile3.slope() == 0 && !tile3.halfBrick() && ((Main.tileSolid[tile3.type] && !Main.tileSolidTop[tile3.type]) || TileID.Sets.Platforms[tile3.type]) && !TileID.Sets.NotReallySolid[tile3.type])
		{
			return true;
		}
		return false;
	}

	private static void Step_LawnMower(SmartCursorUsageInfo providedInfo, ref int fX, ref int fY)
	{
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0107: Unknown result type (might be due to invalid IL or missing references)
		//IL_0111: Unknown result type (might be due to invalid IL or missing references)
		//IL_0116: Unknown result type (might be due to invalid IL or missing references)
		//IL_0120: Unknown result type (might be due to invalid IL or missing references)
		//IL_0125: Unknown result type (might be due to invalid IL or missing references)
		//IL_012b: Unknown result type (might be due to invalid IL or missing references)
		//IL_016f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0176: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0151: Unknown result type (might be due to invalid IL or missing references)
		//IL_0156: Unknown result type (might be due to invalid IL or missing references)
		//IL_0189: Unknown result type (might be due to invalid IL or missing references)
		//IL_0192: Unknown result type (might be due to invalid IL or missing references)
		int reachableStartX = providedInfo.reachableStartX;
		int reachableStartY = providedInfo.reachableStartY;
		int reachableEndX = providedInfo.reachableEndX;
		int reachableEndY = providedInfo.reachableEndY;
		_ = providedInfo.screenTargetX;
		_ = providedInfo.screenTargetY;
		if (providedInfo.item.type != 4049 || fX != -1 || fY != -1)
		{
			return;
		}
		_targets.Clear();
		for (int i = reachableStartX; i <= reachableEndX; i++)
		{
			for (int j = reachableStartY; j <= reachableEndY; j++)
			{
				Tile tile = Main.tile[i, j];
				if (tile.active() && (tile.type == 2 || tile.type == 109))
				{
					_targets.Add(new Point(i, j));
				}
			}
		}
		if (_targets.Count > 0)
		{
			float num = -1f;
			Point val = _targets[0];
			for (int k = 0; k < _targets.Count; k++)
			{
				float num2 = Vector2.Distance(new Vector2((float)_targets[k].X, (float)_targets[k].Y) * 16f + Vector2.One * 8f, providedInfo.mouse);
				if (num == -1f || num2 < num)
				{
					num = num2;
					val = _targets[k];
				}
			}
			if (Collision.InTileBounds(val.X, val.Y, reachableStartX, reachableStartY, reachableEndX, reachableEndY))
			{
				fX = val.X;
				fY = val.Y;
			}
		}
		_targets.Clear();
	}
}
