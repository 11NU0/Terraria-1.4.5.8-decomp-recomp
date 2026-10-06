using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ObjectData;

namespace Terraria;

public struct TileObject
{
	public int xCoord;

	public int yCoord;

	public int type;

	public int style;

	public int alternate;

	public int random;

	public static TileObject Empty = default;

	public static TileObjectPreviewData objectPreview = new TileObjectPreviewData();

	public static bool Place(TileObject toBePlaced)
	{
		TileObjectData tileData = TileObjectData.GetTileData(toBePlaced.type, toBePlaced.style, toBePlaced.alternate);
		if (tileData == null)
		{
			return false;
		}
		if (tileData.HookPlaceOverride.hook != null)
		{
			int x;
			int y;
			if (tileData.HookPlaceOverride.processedCoordinates)
			{
				x = toBePlaced.xCoord;
				y = toBePlaced.yCoord;
			}
			else
			{
				x = toBePlaced.xCoord + tileData.Origin.X;
				y = toBePlaced.yCoord + tileData.Origin.Y;
			}
			if (tileData.HookPlaceOverride.hook(x, y, toBePlaced.type, toBePlaced.style, 1, toBePlaced.alternate) == tileData.HookPlaceOverride.badReturn)
			{
				return false;
			}
		}
		else
		{
			ushort num = (ushort)toBePlaced.type;
			int num2 = 0;
			int num3 = 0;
			int num4 = tileData.CalculatePlacementStyle(toBePlaced.style, toBePlaced.alternate, toBePlaced.random);
			int num5 = 0;
			if (tileData.StyleWrapLimit > 0)
			{
				num5 = num4 / tileData.StyleWrapLimit * tileData.StyleLineSkip;
				num4 %= tileData.StyleWrapLimit;
			}
			if (tileData.StyleHorizontal)
			{
				num2 = tileData.CoordinateFullWidth * num4;
				num3 = tileData.CoordinateFullHeight * num5;
			}
			else
			{
				num2 = tileData.CoordinateFullWidth * num5;
				num3 = tileData.CoordinateFullHeight * num4;
			}
			int num6 = toBePlaced.xCoord;
			int num7 = toBePlaced.yCoord;
			for (int i = 0; i < tileData.Width; i++)
			{
				for (int j = 0; j < tileData.Height; j++)
				{
					Tile tileSafely = Framing.GetTileSafely(num6 + i, num7 + j);
					if (tileSafely.active() && tileSafely.type != 484 && (Main.tileCut[tileSafely.type] || TileID.Sets.BreakableWhenPlacing[tileSafely.type]))
					{
						WorldGen.KillTile(num6 + i, num7 + j);
						if (!Main.tile[num6 + i, num7 + j].active() && Main.netMode != 0)
						{
							NetMessage.SendData(17, -1, -1, null, 0, num6 + i, num7 + j);
						}
					}
				}
			}
			for (int k = 0; k < tileData.Width; k++)
			{
				int num8 = num2 + k * (tileData.CoordinateWidth + tileData.CoordinatePadding);
				int num9 = num3;
				for (int l = 0; l < tileData.Height; l++)
				{
					Tile tileSafely2 = Framing.GetTileSafely(num6 + k, num7 + l);
					if (!tileSafely2.active())
					{
						tileSafely2.active(active: true);
						tileSafely2.frameX = (short)num8;
						tileSafely2.frameY = (short)num9;
						tileSafely2.type = num;
					}
					num9 += tileData.CoordinateHeights[l] + tileData.CoordinatePadding;
				}
			}
		}
		if (tileData.FlattenAnchors)
		{
			AnchorData anchorBottom = tileData.AnchorBottom;
			if (anchorBottom.tileCount != 0 && (anchorBottom.type & AnchorType.SolidTile) == AnchorType.SolidTile)
			{
				int num10 = toBePlaced.xCoord + anchorBottom.checkStart;
				int j2 = toBePlaced.yCoord + tileData.Height;
				for (int m = 0; m < anchorBottom.tileCount; m++)
				{
					Tile tileSafely3 = Framing.GetTileSafely(num10 + m, j2);
					if (Main.tileSolid[tileSafely3.type] && !Main.tileSolidTop[tileSafely3.type] && tileSafely3.blockType() != 0)
					{
						WorldGen.SlopeTile(num10 + m, j2);
					}
				}
			}
			anchorBottom = tileData.AnchorTop;
			if (anchorBottom.tileCount != 0 && (anchorBottom.type & AnchorType.SolidTile) == AnchorType.SolidTile)
			{
				int num11 = toBePlaced.xCoord + anchorBottom.checkStart;
				int j3 = toBePlaced.yCoord - 1;
				for (int n = 0; n < anchorBottom.tileCount; n++)
				{
					Tile tileSafely4 = Framing.GetTileSafely(num11 + n, j3);
					if (Main.tileSolid[tileSafely4.type] && !Main.tileSolidTop[tileSafely4.type] && tileSafely4.blockType() != 0)
					{
						WorldGen.SlopeTile(num11 + n, j3);
					}
				}
			}
			anchorBottom = tileData.AnchorRight;
			if (anchorBottom.tileCount != 0 && (anchorBottom.type & AnchorType.SolidTile) == AnchorType.SolidTile)
			{
				int i2 = toBePlaced.xCoord + tileData.Width;
				int num12 = toBePlaced.yCoord + anchorBottom.checkStart;
				for (int num13 = 0; num13 < anchorBottom.tileCount; num13++)
				{
					Tile tileSafely5 = Framing.GetTileSafely(i2, num12 + num13);
					if (Main.tileSolid[tileSafely5.type] && !Main.tileSolidTop[tileSafely5.type] && tileSafely5.blockType() != 0)
					{
						WorldGen.SlopeTile(i2, num12 + num13);
					}
				}
			}
			anchorBottom = tileData.AnchorLeft;
			if (anchorBottom.tileCount != 0 && (anchorBottom.type & AnchorType.SolidTile) == AnchorType.SolidTile)
			{
				int i3 = toBePlaced.xCoord - 1;
				int num14 = toBePlaced.yCoord + anchorBottom.checkStart;
				for (int num15 = 0; num15 < anchorBottom.tileCount; num15++)
				{
					Tile tileSafely6 = Framing.GetTileSafely(i3, num14 + num15);
					if (Main.tileSolid[tileSafely6.type] && !Main.tileSolidTop[tileSafely6.type] && tileSafely6.blockType() != 0)
					{
						WorldGen.SlopeTile(i3, num14 + num15);
					}
				}
			}
		}
		return true;
	}

	public static bool CanPlace(int x, int y, int type, int style, int dir, out TileObject objectData, bool onlyCheck = false, int? forcedRandom = null)
	{
		//IL_015c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0178: Unknown result type (might be due to invalid IL or missing references)
		//IL_022e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0235: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0262: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_03cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0274: Unknown result type (might be due to invalid IL or missing references)
		//IL_0282: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0331: Unknown result type (might be due to invalid IL or missing references)
		//IL_041a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0343: Unknown result type (might be due to invalid IL or missing references)
		//IL_0351: Unknown result type (might be due to invalid IL or missing references)
		//IL_042c: Unknown result type (might be due to invalid IL or missing references)
		//IL_043a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0393: Unknown result type (might be due to invalid IL or missing references)
		//IL_039a: Unknown result type (might be due to invalid IL or missing references)
		//IL_047c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0483: Unknown result type (might be due to invalid IL or missing references)
		TileObjectData tileData = TileObjectData.GetTileData(type, style);
		objectData = Empty;
		if (tileData == null)
		{
			return false;
		}
		int num = x - tileData.Origin.X;
		int num2 = y - tileData.Origin.Y;
		if (num < 0 || num + tileData.Width >= Main.maxTilesX || num2 < 0 || num2 + tileData.Height >= Main.maxTilesY)
		{
			return false;
		}
		bool flag = tileData.RandomStyleRange > 0;
		if (TileObjectPreviewData.placementCache == null)
		{
			TileObjectPreviewData.placementCache = new TileObjectPreviewData();
		}
		TileObjectPreviewData.placementCache.Reset();
		int num3 = 0;
		if (tileData.AlternatesCount != 0)
		{
			num3 = tileData.AlternatesCount;
		}
		float num4 = -1f;
		float num5 = -1f;
		int num6 = 0;
		TileObjectData tileObjectData = null;
		int num7 = -1;
		bool flag2 = false;
		while (num7 < num3)
		{
			num7++;
			TileObjectData tileData2 = TileObjectData.GetTileData(type, style, num7);
			if (tileData2.Direction != TileObjectDirection.None && ((tileData2.Direction == TileObjectDirection.PlaceLeft && dir == 1) || (tileData2.Direction == TileObjectDirection.PlaceRight && dir == -1)))
			{
				continue;
			}
			int num8 = x - tileData2.Origin.X;
			int num9 = y - tileData2.Origin.Y;
			if (num8 < 5 || num8 + tileData2.Width > Main.maxTilesX - 5 || num9 < 5 || num9 + tileData2.Height > Main.maxTilesY - 5)
			{
				return false;
			}
			Rectangle val = new Rectangle(0, 0, tileData2.Width, tileData2.Height);
			int num10 = 0;
			int num11 = 0;
			if (tileData2.AnchorTop.tileCount != 0)
			{
				if (val.Y == 0)
				{
					val.Y = -1;
					val.Height++;
					num11++;
				}
				int checkStart = tileData2.AnchorTop.checkStart;
				if (checkStart < val.X)
				{
					val.Width += val.X - checkStart;
					num10 += val.X - checkStart;
					val.X = checkStart;
				}
				int num12 = checkStart + tileData2.AnchorTop.tileCount - 1;
				int num13 = val.X + val.Width - 1;
				if (num12 > num13)
				{
					val.Width += num12 - num13;
				}
			}
			if (tileData2.AnchorBottom.tileCount != 0)
			{
				if (val.Y + val.Height == tileData2.Height)
				{
					val.Height++;
				}
				int checkStart2 = tileData2.AnchorBottom.checkStart;
				if (checkStart2 < val.X)
				{
					val.Width += val.X - checkStart2;
					num10 += val.X - checkStart2;
					val.X = checkStart2;
				}
				int num14 = checkStart2 + tileData2.AnchorBottom.tileCount - 1;
				int num15 = val.X + val.Width - 1;
				if (num14 > num15)
				{
					val.Width += num14 - num15;
				}
			}
			if (tileData2.AnchorLeft.tileCount != 0)
			{
				if (val.X == 0)
				{
					val.X = -1;
					val.Width++;
					num10++;
				}
				int num16 = tileData2.AnchorLeft.checkStart;
				if ((tileData2.AnchorLeft.type & AnchorType.Tree) == AnchorType.Tree)
				{
					num16--;
				}
				if (num16 < val.Y)
				{
					val.Width += val.Y - num16;
					num11 += val.Y - num16;
					val.Y = num16;
				}
				int num17 = num16 + tileData2.AnchorLeft.tileCount - 1;
				if ((tileData2.AnchorLeft.type & AnchorType.Tree) == AnchorType.Tree)
				{
					num17 += 2;
				}
				int num18 = val.Y + val.Height - 1;
				if (num17 > num18)
				{
					val.Height += num17 - num18;
				}
			}
			if (tileData2.AnchorRight.tileCount != 0)
			{
				if (val.X + val.Width == tileData2.Width)
				{
					val.Width++;
				}
				int num19 = tileData2.AnchorLeft.checkStart;
				if ((tileData2.AnchorRight.type & AnchorType.Tree) == AnchorType.Tree)
				{
					num19--;
				}
				if (num19 < val.Y)
				{
					val.Width += val.Y - num19;
					num11 += val.Y - num19;
					val.Y = num19;
				}
				int num20 = num19 + tileData2.AnchorRight.tileCount - 1;
				if ((tileData2.AnchorRight.type & AnchorType.Tree) == AnchorType.Tree)
				{
					num20 += 2;
				}
				int num21 = val.Y + val.Height - 1;
				if (num20 > num21)
				{
					val.Height += num20 - num21;
				}
			}
			if (onlyCheck)
			{
				objectPreview.Reset();
				objectPreview.Active = true;
				objectPreview.Type = (ushort)type;
				objectPreview.Style = (short)style;
				objectPreview.Alternate = num7;
				objectPreview.Size = new Point16(val.Width, val.Height);
				objectPreview.ObjectStart = new Point16(num10, num11);
				objectPreview.Coordinates = new Point16(num8 - num10, num9 - num11);
			}
			float num22 = 0f;
			float num23 = tileData2.Width * tileData2.Height;
			float num24 = 0f;
			float num25 = 0f;
			for (int i = 0; i < tileData2.Width; i++)
			{
				for (int j = 0; j < tileData2.Height; j++)
				{
					Tile tileSafely = Framing.GetTileSafely(num8 + i, num9 + j);
					bool flag3 = !tileData2.LiquidPlace(tileSafely);
					bool flag4 = false;
					if (tileData2.AnchorWall)
					{
						num25++;
						if (!tileData2.isValidWallAnchor(tileSafely.wall))
						{
							flag4 = true;
						}
						else
						{
							num24++;
						}
					}
					bool flag5 = false;
					if (tileSafely.active() && (!Main.tileCut[tileSafely.type] || tileSafely.type == 484 || tileSafely.type == 654) && !TileID.Sets.BreakableWhenPlacing[tileSafely.type])
					{
						flag5 = true;
					}
					if (flag5 | flag3 | flag4)
					{
						if (onlyCheck)
						{
							objectPreview[i + num10, j + num11] = 2;
						}
						continue;
					}
					if (onlyCheck)
					{
						objectPreview[i + num10, j + num11] = 1;
					}
					num22++;
				}
			}
			AnchorData anchorBottom = tileData2.AnchorBottom;
			if (anchorBottom.tileCount != 0)
			{
				num25 += (float)anchorBottom.tileCount;
				int height = tileData2.Height;
				for (int k = 0; k < anchorBottom.tileCount; k++)
				{
					int num26 = anchorBottom.checkStart + k;
					Tile tileSafely = Framing.GetTileSafely(num8 + num26, num9 + height);
					bool flag6 = false;
					if (tileSafely.nactive())
					{
						if ((anchorBottom.type & AnchorType.SolidTile) == AnchorType.SolidTile && Main.tileSolid[tileSafely.type] && !Main.tileSolidTop[tileSafely.type] && !Main.tileNoAttach[tileSafely.type] && (tileData2.FlattenAnchors || tileSafely.blockType() == 0))
						{
							flag6 = tileData2.isValidTileAnchor(tileSafely.type);
						}
						if (!flag6 && ((anchorBottom.type & AnchorType.SolidWithTop) == AnchorType.SolidWithTop || (anchorBottom.type & AnchorType.Table) == AnchorType.Table))
						{
							if (TileID.Sets.Platforms[tileSafely.type])
							{
								_ = tileSafely.frameX / TileObjectData.PlatformFrameWidth();
								if (!tileSafely.halfBrick() && WorldGen.PlatformProperTopFrame(tileSafely.frameX))
								{
									flag6 = true;
								}
							}
							else if (Main.tileSolid[tileSafely.type] && Main.tileSolidTop[tileSafely.type])
							{
								flag6 = true;
							}
						}
						if (!flag6 && (anchorBottom.type & AnchorType.Table) == AnchorType.Table && !TileID.Sets.Platforms[tileSafely.type] && Main.tileTable[tileSafely.type] && tileSafely.blockType() == 0)
						{
							flag6 = true;
						}
						if (!flag6 && (anchorBottom.type & AnchorType.SolidSide) == AnchorType.SolidSide && Main.tileSolid[tileSafely.type] && !Main.tileSolidTop[tileSafely.type])
						{
							int num27 = tileSafely.blockType();
							if ((uint)(num27 - 4) <= 1u)
							{
								flag6 = tileData2.isValidTileAnchor(tileSafely.type);
							}
						}
						if (!flag6 && (anchorBottom.type & AnchorType.AlternateTile) == AnchorType.AlternateTile && tileData2.isValidAlternateAnchor(tileSafely.type))
						{
							flag6 = true;
						}
					}
					else if (!flag6 && (anchorBottom.type & AnchorType.EmptyTile) == AnchorType.EmptyTile)
					{
						flag6 = true;
					}
					if (!flag6)
					{
						if (onlyCheck)
						{
							objectPreview[num26 + num10, height + num11] = 2;
						}
						continue;
					}
					if (onlyCheck)
					{
						objectPreview[num26 + num10, height + num11] = 1;
					}
					num24++;
				}
			}
			int num28 = -1;
			anchorBottom = tileData2.AnchorTop;
			if (anchorBottom.tileCount != 0)
			{
				num25 += (float)anchorBottom.tileCount;
				int num29 = -1;
				for (int l = 0; l < anchorBottom.tileCount; l++)
				{
					int num30 = -1;
					int num31 = anchorBottom.checkStart + l;
					Tile tileSafely = Framing.GetTileSafely(num8 + num31, num9 + num29);
					bool flag7 = false;
					if (tileSafely.nactive())
					{
						if (Main.tileSolid[tileSafely.type] && !Main.tileSolidTop[tileSafely.type] && !Main.tileNoAttach[tileSafely.type] && (tileData2.FlattenAnchors || tileSafely.blockType() == 0))
						{
							flag7 = tileData2.isValidTileAnchor(tileSafely.type);
							if (flag7)
							{
								num30 = 0;
							}
						}
						if (!flag7 && (anchorBottom.type & AnchorType.SolidBottom) == AnchorType.SolidBottom && ((Main.tileSolid[tileSafely.type] && (!Main.tileSolidTop[tileSafely.type] || (TileID.Sets.Platforms[tileSafely.type] && (tileSafely.halfBrick() || tileSafely.topSlope())))) || tileSafely.halfBrick() || tileSafely.topSlope()) && !TileID.Sets.NotReallySolid[tileSafely.type] && !tileSafely.bottomSlope())
						{
							flag7 = tileData2.isValidTileAnchor(tileSafely.type);
							if (flag7)
							{
								num30 = 0;
							}
						}
						if (!flag7 && (anchorBottom.type & AnchorType.Platform) == AnchorType.Platform && TileID.Sets.Platforms[tileSafely.type])
						{
							flag7 = tileData2.isValidTileAnchor(tileSafely.type);
							if (flag7)
							{
								num30 = ((!tileSafely.halfBrick() && !tileSafely.topSlope()) ? 8 : 0);
							}
						}
						if (!flag7 && (anchorBottom.type & AnchorType.PlatformNonHammered) == AnchorType.PlatformNonHammered && TileID.Sets.Platforms[tileSafely.type] && tileSafely.slope() == 0 && !tileSafely.halfBrick())
						{
							flag7 = tileData2.isValidTileAnchor(tileSafely.type);
							if (flag7)
							{
								num30 = 8;
							}
						}
						if (!flag7 && (anchorBottom.type & AnchorType.PlanterBox) == AnchorType.PlanterBox && tileSafely.type == 380)
						{
							flag7 = tileData2.isValidTileAnchor(tileSafely.type);
							if (flag7)
							{
								num30 = 0;
							}
						}
						if (!flag7 && (anchorBottom.type & AnchorType.SolidSide) == AnchorType.SolidSide && Main.tileSolid[tileSafely.type] && !Main.tileSolidTop[tileSafely.type])
						{
							int num27 = tileSafely.blockType();
							if ((uint)(num27 - 2) <= 1u)
							{
								flag7 = tileData2.isValidTileAnchor(tileSafely.type);
							}
							if (flag7)
							{
								num30 = 0;
							}
						}
						if (!flag7 && (anchorBottom.type & AnchorType.AlternateTile) == AnchorType.AlternateTile && tileData2.isValidAlternateAnchor(tileSafely.type))
						{
							flag7 = true;
							if (flag7)
							{
								num30 = 0;
							}
						}
					}
					else if (!flag7 && (anchorBottom.type & AnchorType.EmptyTile) == AnchorType.EmptyTile)
					{
						flag7 = true;
						num30 = 0;
					}
					if (flag7 && (anchorBottom.type & AnchorType.AllFlatHeight) == AnchorType.AllFlatHeight)
					{
						if (num28 == -1)
						{
							num28 = num30;
						}
						if (num28 != num30)
						{
							flag7 = false;
						}
					}
					if (!flag7)
					{
						if (onlyCheck)
						{
							objectPreview[num31 + num10, num29 + num11] = 2;
						}
						continue;
					}
					if (onlyCheck)
					{
						objectPreview[num31 + num10, num29 + num11] = 1;
					}
					num24++;
				}
			}
			anchorBottom = tileData2.AnchorRight;
			if (anchorBottom.tileCount != 0)
			{
				num25 += (float)anchorBottom.tileCount;
				int width = tileData2.Width;
				for (int m = 0; m < anchorBottom.tileCount; m++)
				{
					int num32 = anchorBottom.checkStart + m;
					Tile tileSafely = Framing.GetTileSafely(num8 + width, num9 + num32);
					bool flag8 = false;
					if (tileSafely.nactive())
					{
						if (Main.tileSolid[tileSafely.type] && !Main.tileSolidTop[tileSafely.type] && !Main.tileNoAttach[tileSafely.type] && (tileData2.FlattenAnchors || tileSafely.blockType() == 0))
						{
							flag8 = tileData2.isValidTileAnchor(tileSafely.type);
						}
						if (!flag8 && (anchorBottom.type & AnchorType.SolidSide) == AnchorType.SolidSide && Main.tileSolid[tileSafely.type] && !Main.tileSolidTop[tileSafely.type])
						{
							int num27 = tileSafely.blockType();
							if (num27 == 2 || num27 == 4)
							{
								flag8 = tileData2.isValidTileAnchor(tileSafely.type);
							}
						}
						if (!flag8 && (anchorBottom.type & AnchorType.Tree) == AnchorType.Tree && TileID.Sets.IsATreeTrunk[tileSafely.type])
						{
							flag8 = true;
							if (m == 0)
							{
								num25++;
								Tile tileSafely2 = Framing.GetTileSafely(num8 + width, num9 + num32 - 1);
								if (tileSafely2.nactive() && TileID.Sets.IsATreeTrunk[tileSafely2.type])
								{
									num24++;
									if (onlyCheck)
									{
										objectPreview[width + num10, num32 + num11 - 1] = 1;
									}
								}
								else if (onlyCheck)
								{
									objectPreview[width + num10, num32 + num11 - 1] = 2;
								}
							}
							if (m == anchorBottom.tileCount - 1)
							{
								num25++;
								Tile tileSafely3 = Framing.GetTileSafely(num8 + width, num9 + num32 + 1);
								if (tileSafely3.nactive() && TileID.Sets.IsATreeTrunk[tileSafely3.type])
								{
									num24++;
									if (onlyCheck)
									{
										objectPreview[width + num10, num32 + num11 + 1] = 1;
									}
								}
								else if (onlyCheck)
								{
									objectPreview[width + num10, num32 + num11 + 1] = 2;
								}
							}
						}
						if (!flag8 && (anchorBottom.type & AnchorType.AlternateTile) == AnchorType.AlternateTile && tileData2.isValidAlternateAnchor(tileSafely.type))
						{
							flag8 = true;
						}
					}
					else if (!flag8 && (anchorBottom.type & AnchorType.EmptyTile) == AnchorType.EmptyTile)
					{
						flag8 = true;
					}
					if (!flag8)
					{
						if (onlyCheck)
						{
							objectPreview[width + num10, num32 + num11] = 2;
						}
						continue;
					}
					if (onlyCheck)
					{
						objectPreview[width + num10, num32 + num11] = 1;
					}
					num24++;
				}
			}
			anchorBottom = tileData2.AnchorLeft;
			if (anchorBottom.tileCount != 0)
			{
				num25 += (float)anchorBottom.tileCount;
				int num33 = -1;
				for (int n = 0; n < anchorBottom.tileCount; n++)
				{
					int num34 = anchorBottom.checkStart + n;
					Tile tileSafely = Framing.GetTileSafely(num8 + num33, num9 + num34);
					bool flag9 = false;
					if (tileSafely.nactive())
					{
						if (Main.tileSolid[tileSafely.type] && !Main.tileSolidTop[tileSafely.type] && !Main.tileNoAttach[tileSafely.type] && (tileData2.FlattenAnchors || tileSafely.blockType() == 0))
						{
							flag9 = tileData2.isValidTileAnchor(tileSafely.type);
						}
						if (!flag9 && (anchorBottom.type & AnchorType.SolidSide) == AnchorType.SolidSide && Main.tileSolid[tileSafely.type] && !Main.tileSolidTop[tileSafely.type])
						{
							int num27 = tileSafely.blockType();
							if (num27 == 3 || num27 == 5)
							{
								flag9 = tileData2.isValidTileAnchor(tileSafely.type);
							}
						}
						if (!flag9 && (anchorBottom.type & AnchorType.Tree) == AnchorType.Tree && TileID.Sets.IsATreeTrunk[tileSafely.type])
						{
							flag9 = true;
							if (n == 0)
							{
								num25++;
								Tile tileSafely4 = Framing.GetTileSafely(num8 + num33, num9 + num34 - 1);
								if (tileSafely4.nactive() && TileID.Sets.IsATreeTrunk[tileSafely4.type])
								{
									num24++;
									if (onlyCheck)
									{
										objectPreview[num33 + num10, num34 + num11 - 1] = 1;
									}
								}
								else if (onlyCheck)
								{
									objectPreview[num33 + num10, num34 + num11 - 1] = 2;
								}
							}
							if (n == anchorBottom.tileCount - 1)
							{
								num25++;
								Tile tileSafely5 = Framing.GetTileSafely(num8 + num33, num9 + num34 + 1);
								if (tileSafely5.nactive() && TileID.Sets.IsATreeTrunk[tileSafely5.type])
								{
									num24++;
									if (onlyCheck)
									{
										objectPreview[num33 + num10, num34 + num11 + 1] = 1;
									}
								}
								else if (onlyCheck)
								{
									objectPreview[num33 + num10, num34 + num11 + 1] = 2;
								}
							}
						}
						if (!flag9 && (anchorBottom.type & AnchorType.AlternateTile) == AnchorType.AlternateTile && tileData2.isValidAlternateAnchor(tileSafely.type))
						{
							flag9 = true;
						}
					}
					else if (!flag9 && (anchorBottom.type & AnchorType.EmptyTile) == AnchorType.EmptyTile)
					{
						flag9 = true;
					}
					if (!flag9)
					{
						if (onlyCheck)
						{
							objectPreview[num33 + num10, num34 + num11] = 2;
						}
						continue;
					}
					if (onlyCheck)
					{
						objectPreview[num33 + num10, num34 + num11] = 1;
					}
					num24++;
				}
			}
			if (tileData2.HookCheckIfCanPlace.hook != null)
			{
				if (tileData2.HookCheckIfCanPlace.processedCoordinates)
				{
					_ = tileData2.Origin;
					_ = tileData2.Origin;
				}
				if (tileData2.HookCheckIfCanPlace.hook(x, y, type, style, dir, num7) == tileData2.HookCheckIfCanPlace.badReturn && tileData2.HookCheckIfCanPlace.badResponse == 0)
				{
					num24 = 0f;
					num22 = 0f;
					objectPreview.AllInvalid();
				}
			}
			float num35 = num24 / num25;
			if (num25 == 0f)
			{
				num35 = 1f;
			}
			float num36 = num22 / num23;
			if (num36 == 1f && num25 == 0f)
			{
				num23 = 1f;
				num25 = 1f;
				num35 = 1f;
				num36 = 1f;
			}
			if (num35 == 1f && num36 == 1f)
			{
				num4 = 1f;
				num5 = 1f;
				num6 = num7;
				tileObjectData = tileData2;
				break;
			}
			if (!((num35 < 1f) & flag2) && (num35 > num4 || (num35 == num4 && num36 > num5)))
			{
				flag2 = true;
				TileObjectPreviewData.placementCache.CopyFrom(objectPreview);
				num4 = num35;
				num5 = num36;
				tileObjectData = tileData2;
				num6 = num7;
			}
		}
		int num37 = -1;
		if (flag)
		{
			if (TileObjectPreviewData.randomCache == null)
			{
				TileObjectPreviewData.randomCache = new TileObjectPreviewData();
			}
			bool flag10 = false;
			if (TileObjectPreviewData.randomCache.Type == type)
			{
				Point16 coordinates = TileObjectPreviewData.randomCache.Coordinates;
				Point16 objectStart = TileObjectPreviewData.randomCache.ObjectStart;
				int num38 = coordinates.X + objectStart.X;
				int num39 = coordinates.Y + objectStart.Y;
				int num40 = x - tileData.Origin.X;
				int num41 = y - tileData.Origin.Y;
				if (num38 != num40 || num39 != num41)
				{
					flag10 = true;
				}
			}
			else
			{
				flag10 = true;
			}
			int randomStyleRange = tileData.RandomStyleRange;
			int num42 = Main.rand.Next(tileData.RandomStyleRange);
			if (forcedRandom.HasValue)
			{
				num42 = (forcedRandom.Value % randomStyleRange + randomStyleRange) % randomStyleRange;
			}
			num37 = ((!flag10 && !forcedRandom.HasValue) ? TileObjectPreviewData.randomCache.Random : num42);
		}
		if (tileData.SpecificRandomStyles != null)
		{
			if (TileObjectPreviewData.randomCache == null)
			{
				TileObjectPreviewData.randomCache = new TileObjectPreviewData();
			}
			bool flag11 = false;
			if (TileObjectPreviewData.randomCache.Type == type)
			{
				Point16 coordinates2 = TileObjectPreviewData.randomCache.Coordinates;
				Point16 objectStart2 = TileObjectPreviewData.randomCache.ObjectStart;
				int num43 = coordinates2.X + objectStart2.X;
				int num44 = coordinates2.Y + objectStart2.Y;
				int num45 = x - tileData.Origin.X;
				int num46 = y - tileData.Origin.Y;
				if (num43 != num45 || num44 != num46)
				{
					flag11 = true;
				}
			}
			else
			{
				flag11 = true;
			}
			int num47 = tileData.SpecificRandomStyles.Length;
			int num48 = Main.rand.Next(num47);
			if (forcedRandom.HasValue)
			{
				num48 = (forcedRandom.Value % num47 + num47) % num47;
			}
			num37 = ((!flag11 && !forcedRandom.HasValue) ? TileObjectPreviewData.randomCache.Random : (tileData.SpecificRandomStyles[num48] - style));
		}
		if (onlyCheck)
		{
			if (num4 != 1f || num5 != 1f)
			{
				objectPreview.CopyFrom(TileObjectPreviewData.placementCache);
				num7 = num6;
			}
			objectPreview.Random = num37;
			if (tileData.RandomStyleRange > 0 || tileData.SpecificRandomStyles != null)
			{
				TileObjectPreviewData.randomCache.CopyFrom(objectPreview);
			}
		}
		if (!onlyCheck)
		{
			objectData.xCoord = x - tileObjectData.Origin.X;
			objectData.yCoord = y - tileObjectData.Origin.Y;
			objectData.type = type;
			objectData.style = style;
			objectData.alternate = num7;
			objectData.random = num37;
		}
		if (num4 == 1f)
		{
			return num5 == 1f;
		}
		return false;
	}

	public static void DrawPreview(SpriteBatch sb, TileObjectPreviewData op, Vector2 position, float opacity)
	{
		//IL_01d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01de: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0246: Unknown result type (might be due to invalid IL or missing references)
		//IL_0256: Unknown result type (might be due to invalid IL or missing references)
		//IL_0259: Unknown result type (might be due to invalid IL or missing references)
		//IL_025a: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_026a: Unknown result type (might be due to invalid IL or missing references)
		//IL_026d: Unknown result type (might be due to invalid IL or missing references)
		//IL_026e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0321: Unknown result type (might be due to invalid IL or missing references)
		//IL_0326: Unknown result type (might be due to invalid IL or missing references)
		//IL_0331: Unknown result type (might be due to invalid IL or missing references)
		//IL_0343: Unknown result type (might be due to invalid IL or missing references)
		//IL_0355: Unknown result type (might be due to invalid IL or missing references)
		//IL_0367: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0395: Unknown result type (might be due to invalid IL or missing references)
		//IL_048e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0490: Unknown result type (might be due to invalid IL or missing references)
		//IL_0497: Unknown result type (might be due to invalid IL or missing references)
		//IL_049e: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_03cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_03fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0408: Unknown result type (might be due to invalid IL or missing references)
		//IL_0414: Unknown result type (might be due to invalid IL or missing references)
		//IL_0416: Unknown result type (might be due to invalid IL or missing references)
		//IL_0428: Unknown result type (might be due to invalid IL or missing references)
		//IL_042a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0464: Unknown result type (might be due to invalid IL or missing references)
		//IL_0466: Unknown result type (might be due to invalid IL or missing references)
		//IL_046d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0474: Unknown result type (might be due to invalid IL or missing references)
		//IL_047e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0432: Unknown result type (might be due to invalid IL or missing references)
		Point16 coordinates = op.Coordinates;
		Texture2D value = TextureAssets.Tile[op.Type].Value;
		TileObjectData tileData = TileObjectData.GetTileData(op.Type, op.Style, op.Alternate);
		int num = 0;
		int num2 = 0;
		int num3 = tileData.CalculatePlacementStyle(op.Style, op.Alternate, op.Random);
		int num4 = 0;
		int num5 = tileData.DrawYOffset;
		int drawXOffset = tileData.DrawXOffset;
		num3 += tileData.DrawStyleOffset;
		int num6 = tileData.StyleWrapLimit;
		int num7 = tileData.StyleLineSkip;
		if (tileData.StyleWrapLimitVisualOverride.HasValue)
		{
			num6 = tileData.StyleWrapLimitVisualOverride.Value;
		}
		if (tileData.styleLineSkipVisualOverride.HasValue)
		{
			num7 = tileData.styleLineSkipVisualOverride.Value;
		}
		if (num6 > 0)
		{
			num4 = num3 / num6 * num7;
			num3 %= num6;
		}
		if (tileData.StyleHorizontal)
		{
			num = tileData.CoordinateFullWidth * num3;
			num2 = tileData.CoordinateFullHeight * num4;
		}
		else
		{
			num = tileData.CoordinateFullWidth * num4;
			num2 = tileData.CoordinateFullHeight * num3;
		}
		for (int i = 0; i < op.Size.X; i++)
		{
			int num8 = num + (i - op.ObjectStart.X) * (tileData.CoordinateWidth + tileData.CoordinatePadding);
			int num9 = num2;
			for (int j = 0; j < op.Size.Y; j++)
			{
				int num10 = coordinates.X + i;
				int num11 = coordinates.Y + j;
				if (j == 0 && tileData.DrawStepDown != 0 && WorldGen.SolidTile(Framing.GetTileSafely(num10, num11 - 1)))
				{
					num5 += tileData.DrawStepDown;
				}
				if (op.Type == 567)
				{
					num5 = ((j != 0) ? tileData.DrawYOffset : (tileData.DrawYOffset - 2));
				}
				int num12 = op[i, j];
				Color val;
				if (num12 != 1)
				{
					if (num12 != 2)
					{
						continue;
					}
					val = Color.Red * 0.7f;
				}
				else
				{
					val = Color.White;
				}
				val *= 0.5f;
				val *= opacity;
				if (i < op.ObjectStart.X || i >= op.ObjectStart.X + tileData.Width || j < op.ObjectStart.Y || j >= op.ObjectStart.Y + tileData.Height)
				{
					continue;
				}
				SpriteEffects val2 = (SpriteEffects)0;
				if (tileData.DrawFlipHorizontal && num10 % 2 == 0)
				{
					val2 = (SpriteEffects)(val2 | (SpriteEffects)1);
				}
				if (tileData.DrawFlipVertical && num11 % 2 == 0)
				{
					val2 = (SpriteEffects)(val2 | (SpriteEffects)2);
				}
				int coordinateWidth = tileData.CoordinateWidth;
				int num13 = tileData.CoordinateHeights[j - op.ObjectStart.Y];
				float num14 = (float)(coordinateWidth - 16) / 2f;
				if (op.Type >= 0 && TileID.Sets.DoNotAdjustDrawPositionBasedOnTileWidth[op.Type])
				{
					num14 = 0f;
				}
				Vector2 val3 = new Vector2((float)(num10 * 16 - (int)(position.X + num14 + (float)drawXOffset)), (float)(num11 * 16 - (int)position.Y + num5));
				Rectangle val4 = new Rectangle(num8, num9, coordinateWidth, num13);
				if (tileData.DrawFrameOffsets != null)
				{
					Rectangle val5 = tileData.DrawFrameOffsets[i - op.ObjectStart.X, j - op.ObjectStart.Y];
					val4.X += val5.X;
					val4.Y += val5.Y;
					val4.Width += val5.Width;
					val4.Height += val5.Height;
				}
				if (TileID.Sets.CritterCageLidStyle[op.Type] >= 0)
				{
					int num15 = TileID.Sets.CritterCageLidStyle[op.Type];
					if ((num15 < 3 && val4.Y % 54 == 0) || (num15 >= 3 && val4.Y % 36 == 0))
					{
						Vector2 val6 = val3;
						val6.Y += 8f;
						Rectangle value2 = val4;
						value2.Y += 8;
						value2.Height -= 8;
						Main.spriteBatch.Draw(value, val6, (Rectangle?)value2, val, 0f, Vector2.Zero, 1f, val2, 0f);
						val6 = val3;
						val6.Y -= 2f;
						value2 = val4;
						if (num15 == 0)
						{
							value2.X = val4.X % 108;
						}
						value2.Y = 0;
						value2.Height = 10;
						Main.spriteBatch.Draw(TextureAssets.CageTop[num15].Value, val6, (Rectangle?)value2, val, 0f, Vector2.Zero, 1f, val2, 0f);
					}
					else
					{
						sb.Draw(value, val3, (Rectangle?)val4, val, 0f, Vector2.Zero, 1f, val2, 0f);
					}
				}
				else
				{
					sb.Draw(value, val3, (Rectangle?)val4, val, 0f, Vector2.Zero, 1f, val2, 0f);
				}
				num9 += num13 + tileData.CoordinatePadding;
			}
		}
	}
}
