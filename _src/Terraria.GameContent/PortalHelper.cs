using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria.ID;

namespace Terraria.GameContent;

public class PortalHelper
{
	public const int PORTALS_PER_PERSON = 2;

	private static int[,] FoundPortals;

	private static int[] PortalCooldownForPlayers;

	private static int[] PortalCooldownForNPCs;

	private static readonly Vector2[] EDGES;

	private static readonly Vector2[] SLOPE_EDGES;

	private static readonly Point[] SLOPE_OFFSETS;

	private static bool anyPortalAtAll;

	static PortalHelper()
	{
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		//IL_010c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0111: Unknown result type (might be due to invalid IL or missing references)
		//IL_011a: Unknown result type (might be due to invalid IL or missing references)
		//IL_011f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0128: Unknown result type (might be due to invalid IL or missing references)
		//IL_012d: Unknown result type (might be due to invalid IL or missing references)
		FoundPortals = new int[256, 2];
		PortalCooldownForPlayers = new int[256];
		PortalCooldownForNPCs = new int[Main.maxNPCs];
		EDGES = new Vector2[4]
		{
			new Vector2(0f, 1f),
			new Vector2(0f, -1f),
			new Vector2(1f, 0f),
			new Vector2(-1f, 0f)
		};
		SLOPE_EDGES = new Vector2[4]
		{
			new Vector2(1f, -1f),
			new Vector2(-1f, -1f),
			new Vector2(1f, 1f),
			new Vector2(-1f, 1f)
		};
		SLOPE_OFFSETS = new Point[4]
		{
			new Point(1, -1),
			new Point(-1, -1),
			new Point(1, 1),
			new Point(-1, 1)
		};
		anyPortalAtAll = false;
		for (int i = 0; i < SLOPE_EDGES.Length; i++)
		{
			SLOPE_EDGES[i].Normalize();
		}
		for (int j = 0; j < FoundPortals.GetLength(0); j++)
		{
			FoundPortals[j, 0] = -1;
			FoundPortals[j, 1] = -1;
		}
	}

	public static void UpdatePortalPoints()
	{
		anyPortalAtAll = false;
		for (int i = 0; i < FoundPortals.GetLength(0); i++)
		{
			FoundPortals[i, 0] = -1;
			FoundPortals[i, 1] = -1;
		}
		for (int j = 0; j < PortalCooldownForPlayers.Length; j++)
		{
			if (PortalCooldownForPlayers[j] > 0)
			{
				PortalCooldownForPlayers[j]--;
			}
		}
		for (int k = 0; k < PortalCooldownForNPCs.Length; k++)
		{
			if (PortalCooldownForNPCs[k] > 0)
			{
				PortalCooldownForNPCs[k]--;
			}
		}
		for (int l = 0; l < 1000; l++)
		{
			Projectile projectile = Main.projectile[l];
			if (projectile.active && projectile.type == 602 && projectile.ai[1] >= 0f && projectile.ai[1] <= 1f && projectile.owner >= 0 && projectile.owner <= 255)
			{
				FoundPortals[projectile.owner, (int)projectile.ai[1]] = l;
				if (FoundPortals[projectile.owner, 0] != -1 && FoundPortals[projectile.owner, 1] != -1)
				{
					anyPortalAtAll = true;
				}
			}
		}
	}

	public static void ResetNPCSlotData(int npcIndex)
	{
		PortalCooldownForNPCs[npcIndex] = 0;
	}

	public static void TryGoingThroughPortals(Entity ent)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0101: Unknown result type (might be due to invalid IL or missing references)
		//IL_0102: Unknown result type (might be due to invalid IL or missing references)
		//IL_012d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0134: Unknown result type (might be due to invalid IL or missing references)
		//IL_0141: Unknown result type (might be due to invalid IL or missing references)
		//IL_0148: Unknown result type (might be due to invalid IL or missing references)
		//IL_015a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0165: Unknown result type (might be due to invalid IL or missing references)
		//IL_016a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0171: Unknown result type (might be due to invalid IL or missing references)
		//IL_0176: Unknown result type (might be due to invalid IL or missing references)
		//IL_017b: Unknown result type (might be due to invalid IL or missing references)
		//IL_017d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0187: Unknown result type (might be due to invalid IL or missing references)
		//IL_018c: Unknown result type (might be due to invalid IL or missing references)
		//IL_018e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0190: Unknown result type (might be due to invalid IL or missing references)
		//IL_0192: Unknown result type (might be due to invalid IL or missing references)
		//IL_0197: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01df: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0201: Unknown result type (might be due to invalid IL or missing references)
		//IL_0203: Unknown result type (might be due to invalid IL or missing references)
		//IL_0205: Unknown result type (might be due to invalid IL or missing references)
		//IL_020a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0216: Unknown result type (might be due to invalid IL or missing references)
		//IL_021b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0227: Unknown result type (might be due to invalid IL or missing references)
		//IL_022c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0236: Unknown result type (might be due to invalid IL or missing references)
		//IL_023b: Unknown result type (might be due to invalid IL or missing references)
		//IL_023d: Unknown result type (might be due to invalid IL or missing references)
		//IL_023f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0241: Unknown result type (might be due to invalid IL or missing references)
		//IL_0246: Unknown result type (might be due to invalid IL or missing references)
		//IL_0252: Unknown result type (might be due to invalid IL or missing references)
		//IL_0257: Unknown result type (might be due to invalid IL or missing references)
		//IL_0279: Unknown result type (might be due to invalid IL or missing references)
		//IL_027e: Unknown result type (might be due to invalid IL or missing references)
		//IL_029a: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02de: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0301: Unknown result type (might be due to invalid IL or missing references)
		//IL_030d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0312: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0315: Unknown result type (might be due to invalid IL or missing references)
		//IL_0322: Unknown result type (might be due to invalid IL or missing references)
		//IL_0327: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0438: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03da: Unknown result type (might be due to invalid IL or missing references)
		//IL_0456: Unknown result type (might be due to invalid IL or missing references)
		//IL_045d: Unknown result type (might be due to invalid IL or missing references)
		if (!anyPortalAtAll)
		{
			return;
		}
		float collisionPoint = 0f;
		_ = ent.velocity;
		int width = ent.width;
		int height = ent.height;
		int num = 1;
		if (ent is Player)
		{
			num = (int)((Player)ent).gravDir;
		}
		for (int i = 0; i < FoundPortals.GetLength(0); i++)
		{
			if (FoundPortals[i, 0] == -1 || FoundPortals[i, 1] == -1 || (ent is Player && (i >= PortalCooldownForPlayers.Length || PortalCooldownForPlayers[i] > 0)) || (ent is NPC && (i >= PortalCooldownForNPCs.Length || PortalCooldownForNPCs[i] > 0)))
			{
				continue;
			}
			for (int j = 0; j < 2; j++)
			{
				Projectile projectile = Main.projectile[FoundPortals[i, j]];
				GetPortalEdges(projectile.Center, projectile.ai[0], out var start, out var end);
				if (!Collision.CheckAABBvLineCollision(ent.position + ent.velocity, ent.Size, start, end, 2f, ref collisionPoint))
				{
					continue;
				}
				Projectile projectile2 = Main.projectile[FoundPortals[i, 1 - j]];
				float num2 = ent.Hitbox.Distance(projectile.Center);
				Vector2 val = GetPortalOutingPoint(ent.Size, projectile2.Center, projectile2.ai[0], out var bonusX, out var bonusY) + Vector2.Normalize(new Vector2((float)bonusX, (float)bonusY)) * num2;
				Vector2 val2 = Vector2.UnitX * 16f;
				if (Collision.TileCollision(val - val2, val2, width, height, fallThrough: true, fall2: true, num) != val2)
				{
					continue;
				}
				val2 = -Vector2.UnitX * 16f;
				if (Collision.TileCollision(val - val2, val2, width, height, fallThrough: true, fall2: true, num) != val2)
				{
					continue;
				}
				val2 = Vector2.UnitY * 16f;
				if (Collision.TileCollision(val - val2, val2, width, height, fallThrough: true, fall2: true, num) != val2)
				{
					continue;
				}
				val2 = -Vector2.UnitY * 16f;
				if (Collision.TileCollision(val - val2, val2, width, height, fallThrough: true, fall2: true, num) != val2)
				{
					continue;
				}
				float num3 = 0.1f;
				if (bonusY == -num)
				{
					num3 = 0.1f;
				}
				if (ent.velocity == Vector2.Zero)
				{
					ent.velocity = (projectile.ai[0] - (float)Math.PI / 2f).ToRotationVector2() * num3;
				}
				if (ent.velocity.Length() < num3)
				{
					ent.velocity.Normalize();
					ent.velocity *= num3;
				}
				Vector2 val3 = Vector2.Normalize(new Vector2((float)bonusX, (float)bonusY));
				if (val3.HasNaNs() || val3 == Vector2.Zero)
				{
					val3 = Vector2.UnitX * (float)ent.direction;
				}
				ent.velocity = val3 * ent.velocity.Length();
				if ((bonusY == -num && Math.Sign(ent.velocity.Y) != -num) || Math.Abs(ent.velocity.Y) < 0.1f)
				{
					ent.velocity.Y = (float)(-num) * 0.1f;
				}
				int num4 = (int)((float)(projectile2.owner * 2) + projectile2.ai[1]);
				int lastPortalColorIndex = num4 + ((num4 % 2 == 0) ? 1 : (-1));
				if (ent is Player)
				{
					Player player = (Player)ent;
					player.lastPortalColorIndex = lastPortalColorIndex;
					player.Teleport(val, 4, num4);
					if (Main.netMode == 1)
					{
						NetMessage.SendData(96, -1, -1, null, player.whoAmI, val.X, val.Y, num4);
						NetMessage.SendData(13, -1, -1, null, player.whoAmI);
					}
					PortalCooldownForPlayers[i] = 10;
				}
				else if (ent is NPC)
				{
					NPC nPC = (NPC)ent;
					nPC.lastPortalColorIndex = lastPortalColorIndex;
					nPC.Teleport(val, 4, num4);
					if (Main.netMode == 2)
					{
						NetMessage.SendData(100, -1, -1, null, nPC.whoAmI, val.X, val.Y, num4);
						NetMessage.SendData(23, -1, -1, null, nPC.whoAmI);
					}
					PortalCooldownForPlayers[i] = 10;
					if (bonusY == -1 && ent.velocity.Y > -3f)
					{
						ent.velocity.Y = -3f;
					}
				}
				return;
			}
		}
	}

	public static int TryPlacingPortal(Projectile theBolt, Vector2 velocity, Vector2 theCrashVelocity)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_019b: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0104: Unknown result type (might be due to invalid IL or missing references)
		//IL_010c: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0206: Unknown result type (might be due to invalid IL or missing references)
		//IL_020c: Unknown result type (might be due to invalid IL or missing references)
		//IL_020d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0215: Unknown result type (might be due to invalid IL or missing references)
		//IL_0121: Unknown result type (might be due to invalid IL or missing references)
		//IL_0126: Unknown result type (might be due to invalid IL or missing references)
		Vector2 val = velocity / velocity.Length();
		Point val2 = FindCollision(theBolt.position, theBolt.position + velocity + val * 32f).ToTileCoordinates();
		Tile tile = Main.tile[val2.X, val2.Y];
		Vector2 val3 = new Vector2((float)(val2.X * 16 + 8), (float)(val2.Y * 16 + 8));
		if (!WorldGen.SolidOrSlopedTile(tile))
		{
			return -1;
		}
		int num = tile.slope();
		bool flag = tile.halfBrick();
		for (int i = 0; i < (flag ? 2 : EDGES.Length); i++)
		{
			if (Vector2.Dot(EDGES[i], val) > 0f && FindValidLine(val2, (int)EDGES[i].Y, (int)(0f - EDGES[i].X), out var bestPosition))
			{
				val3 = new Vector2((float)(bestPosition.X * 16 + 8), (float)(bestPosition.Y * 16 + 8));
				return AddPortal(theBolt, val3 - EDGES[i] * (flag ? 0f : 8f), (float)Math.Atan2(EDGES[i].Y, EDGES[i].X) + (float)Math.PI / 2f, (int)theBolt.ai[0], theBolt.direction);
			}
		}
		if (num != 0)
		{
			Vector2 val4 = SLOPE_EDGES[num - 1];
			if (Vector2.Dot(val4, -val) > 0f && FindValidLine(val2, -SLOPE_OFFSETS[num - 1].Y, SLOPE_OFFSETS[num - 1].X, out var bestPosition2))
			{
				val3 = new Vector2((float)(bestPosition2.X * 16 + 8), (float)(bestPosition2.Y * 16 + 8));
				return AddPortal(theBolt, val3, (float)Math.Atan2(val4.Y, val4.X) - (float)Math.PI / 2f, (int)theBolt.ai[0], theBolt.direction);
			}
		}
		return -1;
	}

	private static bool FindValidLine(Point position, int xOffset, int yOffset, out Point bestPosition)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		bestPosition = position;
		if (IsValidLine(position, xOffset, yOffset))
		{
			return true;
		}
		Point val = new Point(position.X - xOffset, position.Y - yOffset);
		if (IsValidLine(val, xOffset, yOffset))
		{
			bestPosition = val;
			return true;
		}
		Point val2 = new Point(position.X + xOffset, position.Y + yOffset);
		if (IsValidLine(val2, xOffset, yOffset))
		{
			bestPosition = val2;
			return true;
		}
		return false;
	}

	private static bool IsValidLine(Point position, int xOffset, int yOffset)
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		Tile tile = Main.tile[position.X, position.Y];
		Tile tile2 = Main.tile[position.X - xOffset, position.Y - yOffset];
		Tile tile3 = Main.tile[position.X + xOffset, position.Y + yOffset];
		if (BlockPortals(Main.tile[position.X + yOffset, position.Y - xOffset]) || BlockPortals(Main.tile[position.X + yOffset - xOffset, position.Y - xOffset - yOffset]) || BlockPortals(Main.tile[position.X + yOffset + xOffset, position.Y - xOffset + yOffset]))
		{
			return false;
		}
		if (CanPlacePortalOn(tile) && CanPlacePortalOn(tile2) && CanPlacePortalOn(tile3) && tile2.HasSameSlope(tile) && tile3.HasSameSlope(tile))
		{
			return true;
		}
		return false;
	}

	private static bool CanPlacePortalOn(Tile t)
	{
		if (!DoesTileTypeSupportPortals(t.type))
		{
			return false;
		}
		return WorldGen.SolidOrSlopedTile(t);
	}

	private static bool DoesTileTypeSupportPortals(ushort tileType)
	{
		if (tileType == 496)
		{
			return false;
		}
		return true;
	}

	private static bool BlockPortals(Tile t)
	{
		if (t.active() && !Main.tileCut[t.type] && !TileID.Sets.BreakableWhenPlacing[t.type] && Main.tileSolid[t.type])
		{
			return true;
		}
		return false;
	}

	private static Vector2 FindCollision(Vector2 startPosition, Vector2 stopPosition)
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		int lastX = 0;
		int lastY = 0;
		Utils.PlotLine(startPosition.ToTileCoordinates(), stopPosition.ToTileCoordinates(), (int x, int y) =>
		{
			lastX = x;
			lastY = y;
			return !WorldGen.SolidOrSlopedTile(x, y);
		}, jump: false);
		return new Vector2((float)lastX * 16f, (float)lastY * 16f);
	}

	private static int AddPortal(Projectile sourceProjectile, Vector2 position, float angle, int form, int direction)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		if (!SupportedTilesAreFine(position, angle))
		{
			return -1;
		}
		RemoveMyOldPortal(form);
		RemoveIntersectingPortals(position, angle);
		int num = Projectile.NewProjectile(Projectile.InheritSource(sourceProjectile), position.X, position.Y, 0f, 0f, 602, 0, 0f, Main.myPlayer, angle, form);
		Main.projectile[num].direction = direction;
		Main.projectile[num].netUpdate = true;
		return num;
	}

	private static void RemoveMyOldPortal(int form)
	{
		for (int i = 0; i < 1000; i++)
		{
			Projectile projectile = Main.projectile[i];
			if (projectile.active && projectile.type == 602 && projectile.owner == Main.myPlayer && projectile.ai[1] == (float)form)
			{
				projectile.Kill();
				break;
			}
		}
	}

	private static void RemoveIntersectingPortals(Vector2 position, float angle)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		GetPortalEdges(position, angle, out var start, out var end);
		for (int i = 0; i < 1000; i++)
		{
			Projectile projectile = Main.projectile[i];
			if (!projectile.active || projectile.type != 602)
			{
				continue;
			}
			GetPortalEdges(projectile.Center, projectile.ai[0], out var start2, out var end2);
			if (Collision.CheckLinevLine(start, end, start2, end2).Length != 0)
			{
				if (projectile.owner != Main.myPlayer && Main.netMode != 2)
				{
					NetMessage.SendData(95, -1, -1, null, projectile.owner, (int)projectile.ai[1]);
				}
				projectile.Kill();
			}
		}
	}

	public static Color GetPortalColor(int colorIndex)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		return GetPortalColor(colorIndex / 2, colorIndex % 2);
	}

	public static Color GetPortalColor(int player, int portal)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		Color result = Color.White;
		if (Main.netMode == 0)
		{
			result = ((portal != 0) ? Main.hslToRgb(0.52f, 1f, 0.6f) : Main.hslToRgb(0.12f, 1f, 0.5f));
		}
		else
		{
			float num = 0.08f;
			result = Main.hslToRgb((0.5f + (float)player * (num * 2f) + (float)portal * num) % 1f, 1f, 0.5f);
		}
		result.A = 66;
		return result;
	}

	private static void GetPortalEdges(Vector2 position, float angle, out Vector2 start, out Vector2 end)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		Vector2 val = angle.ToRotationVector2();
		start = position + val * -22f;
		end = position + val * 22f;
	}

	private static Vector2 GetPortalOutingPoint(Vector2 objectSize, Vector2 portalPosition, float portalAngle, out int bonusX, out int bonusY)
	{
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0112: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_0100: Unknown result type (might be due to invalid IL or missing references)
		//IL_0105: Unknown result type (might be due to invalid IL or missing references)
		int num = (int)Math.Round(MathHelper.WrapAngle(portalAngle) / ((float)Math.PI / 4f));
		switch (num)
		{
		case -2:
		case 2:
			bonusX = ((num != 2) ? 1 : (-1));
			bonusY = 0;
			return portalPosition + new Vector2((num == 2) ? (0f - objectSize.X) : 0f, (0f - objectSize.Y) / 2f);
		case 0:
		case 4:
			bonusX = 0;
			bonusY = ((num == 0) ? 1 : (-1));
			return portalPosition + new Vector2((0f - objectSize.X) / 2f, (num == 0) ? 0f : (0f - objectSize.Y));
		case -3:
		case 3:
			bonusX = ((num == -3) ? 1 : (-1));
			bonusY = -1;
			return portalPosition + new Vector2((num == -3) ? 0f : (0f - objectSize.X), 0f - objectSize.Y);
		case -1:
		case 1:
			bonusX = ((num == -1) ? 1 : (-1));
			bonusY = 1;
			return portalPosition + new Vector2((num == -1) ? 0f : (0f - objectSize.X), 0f);
		default:
			bonusX = 0;
			bonusY = 0;
			return portalPosition;
		}
	}

	public static void SyncPortalsOnPlayerJoin(int plr, int fluff, List<Point> dontInclude, out List<Point> portalSections)
	{
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		portalSections = new List<Point>();
		for (int i = 0; i < 1000; i++)
		{
			Projectile projectile = Main.projectile[i];
			if (!projectile.active || (projectile.type != 602 && projectile.type != 601))
			{
				continue;
			}
			Vector2 center = projectile.Center;
			int sectionX = Netplay.GetSectionX((int)(center.X / 16f));
			int sectionY = Netplay.GetSectionY((int)(center.Y / 16f));
			for (int j = sectionX - fluff; j < sectionX + fluff + 1; j++)
			{
				for (int k = sectionY - fluff; k < sectionY + fluff + 1; k++)
				{
					if (j >= 0 && j < Main.maxSectionsX && k >= 0 && k < Main.maxSectionsY && !Netplay.Clients[plr].TileSections[j, k] && !dontInclude.Contains(new Point(j, k)))
					{
						portalSections.Add(new Point(j, k));
					}
				}
			}
		}
	}

	public static void SyncPortalSections(Vector2 portalPosition, int fluff)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		for (int i = 0; i < 255; i++)
		{
			if (Main.player[i].active)
			{
				RemoteClient.CheckSection(i, portalPosition, fluff);
			}
		}
	}

	public static bool SupportedTilesAreFine(Vector2 portalCenter, float portalAngle)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0149: Unknown result type (might be due to invalid IL or missing references)
		//IL_014f: Unknown result type (might be due to invalid IL or missing references)
		//IL_019d: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_015c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0162: Unknown result type (might be due to invalid IL or missing references)
		//IL_01da: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0171: Unknown result type (might be due to invalid IL or missing references)
		//IL_0177: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		//IL_010b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0202: Unknown result type (might be due to invalid IL or missing references)
		//IL_020a: Unknown result type (might be due to invalid IL or missing references)
		//IL_011c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0124: Unknown result type (might be due to invalid IL or missing references)
		Point val = portalCenter.ToTileCoordinates();
		int num = (int)Math.Round(MathHelper.WrapAngle(portalAngle) / ((float)Math.PI / 4f));
		int num2;
		int num3;
		switch (num)
		{
		case -2:
		case 2:
			num2 = ((num != 2) ? 1 : (-1));
			num3 = 0;
			break;
		case 0:
		case 4:
			num2 = 0;
			num3 = ((num == 0) ? 1 : (-1));
			break;
		case -3:
		case 3:
			num2 = ((num == -3) ? 1 : (-1));
			num3 = -1;
			break;
		case -1:
		case 1:
			num2 = ((num == -1) ? 1 : (-1));
			num3 = 1;
			break;
		default:
			Main.NewText("Broken portal! (over4s = " + num + " , " + portalAngle + ")");
			return false;
		}
		if (num2 != 0 && num3 != 0)
		{
			int num4 = 3;
			if (num2 == -1 && num3 == 1)
			{
				num4 = 5;
			}
			if (num2 == 1 && num3 == -1)
			{
				num4 = 2;
			}
			if (num2 == 1 && num3 == 1)
			{
				num4 = 4;
			}
			num4--;
			if (SupportedSlope(val.X, val.Y, num4) && SupportedSlope(val.X + num2, val.Y - num3, num4))
			{
				return SupportedSlope(val.X - num2, val.Y + num3, num4);
			}
			return false;
		}
		if (num2 != 0)
		{
			if (num2 == 1)
			{
				val.X--;
			}
			if (SupportedNormal(val.X, val.Y) && SupportedNormal(val.X, val.Y - 1))
			{
				return SupportedNormal(val.X, val.Y + 1);
			}
			return false;
		}
		if (num3 != 0)
		{
			if (num3 == 1)
			{
				val.Y--;
			}
			if (!SupportedNormal(val.X, val.Y) || !SupportedNormal(val.X + 1, val.Y) || !SupportedNormal(val.X - 1, val.Y))
			{
				if (SupportedHalfbrick(val.X, val.Y) && SupportedHalfbrick(val.X + 1, val.Y))
				{
					return SupportedHalfbrick(val.X - 1, val.Y);
				}
				return false;
			}
			return true;
		}
		return true;
	}

	private static bool SupportedSlope(int x, int y, int slope)
	{
		Tile tile = Main.tile[x, y];
		if (tile != null && tile.nactive() && !Main.tileCut[tile.type] && !TileID.Sets.BreakableWhenPlacing[tile.type] && Main.tileSolid[tile.type] && tile.slope() == slope)
		{
			return DoesTileTypeSupportPortals(tile.type);
		}
		return false;
	}

	private static bool SupportedHalfbrick(int x, int y)
	{
		Tile tile = Main.tile[x, y];
		if (tile != null && tile.nactive() && !Main.tileCut[tile.type] && !TileID.Sets.BreakableWhenPlacing[tile.type] && Main.tileSolid[tile.type] && tile.halfBrick())
		{
			return DoesTileTypeSupportPortals(tile.type);
		}
		return false;
	}

	private static bool SupportedNormal(int x, int y)
	{
		Tile tile = Main.tile[x, y];
		if (tile != null && tile.nactive() && !Main.tileCut[tile.type] && !TileID.Sets.BreakableWhenPlacing[tile.type] && Main.tileSolid[tile.type] && !TileID.Sets.NotReallySolid[tile.type] && !tile.halfBrick() && tile.slope() == 0)
		{
			return DoesTileTypeSupportPortals(tile.type);
		}
		return false;
	}
}
