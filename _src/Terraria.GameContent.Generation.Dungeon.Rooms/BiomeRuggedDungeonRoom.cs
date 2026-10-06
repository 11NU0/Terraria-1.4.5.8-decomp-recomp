using System;
using Microsoft.Xna.Framework;
using Terraria.GameContent.Generation.Dungeon.Features;
using Terraria.Utilities;
using Terraria.WorldBuilding;

namespace Terraria.GameContent.Generation.Dungeon.Rooms;

public class BiomeRuggedDungeonRoom : BiomeDungeonRoom
{
	public Vector2 Position;

	public int RoomInnerSize;

	public int RoomOuterSize;

	public int WallDepth;

	public BiomeRuggedDungeonRoom(DungeonRoomSettings settings)
		: base(settings)
	{
		_innerShapeData = new ShapeData();
		_outerShapeData = new ShapeData();
	}

	public override void CalculateRoom(DungeonData data)
	{
		calculated = false;
		int x = settings.RoomPosition.X;
		int y = settings.RoomPosition.Y;
		BiomeRoom(data, x, y, generating: false);
		calculated = true;
	}

	public override bool GenerateRoom(DungeonData data)
	{
		generated = false;
		int x = settings.RoomPosition.X;
		int y = settings.RoomPosition.Y;
		BiomeRoom(data, x, y, generating: true);
		generated = true;
		return true;
	}

	public override bool CanGenerateFeatureAt(DungeonData data, IDungeonFeature feature, int x, int y)
	{
		if (feature is DungeonGlobalBanners || feature is DungeonGlobalPaintings)
		{
			return false;
		}
		return base.CanGenerateFeatureAt(data, feature, x, y);
	}

	public override void GenerateEarlyDungeonFeaturesInRoom(DungeonData data)
	{
		new UnifiedRandom(settings.RandomSeed);
		BiomeDungeonRoomSettings biomeDungeonRoomSettings = (BiomeDungeonRoomSettings)settings;
		ushort brickTileType = settings.StyleData.BrickTileType;
		_ = settings.StyleData.BrickWallType;
		byte? b = ((settings.OverridePaintTile > -1) ? new byte?((byte)settings.OverridePaintTile) : settings.StyleData.TilePaintColor);
		if (settings.OverridePaintWall <= -1)
		{
			_ = settings.StyleData.WallPaintColor;
		}
		else
		{
			_ = settings.OverridePaintWall;
		}
		DungeonUtils.GenerateSpeleothemsInArea(data, biomeDungeonRoomSettings.StyleData, InnerBounds, InnerBounds.Width / 6, brickTileType, b);
		DungeonUtils.GenerateFloatingRocksInArea(data, biomeDungeonRoomSettings.StyleData, InnerBounds, brickTileType, includePlatform: true, b);
	}

	public void BiomeRoom(DungeonData data, int i, int j, bool generating)
	{
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0106: Unknown result type (might be due to invalid IL or missing references)
		//IL_012d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0154: Unknown result type (might be due to invalid IL or missing references)
		//IL_017b: Unknown result type (might be due to invalid IL or missing references)
		//IL_019c: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01de: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_025c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0623: Unknown result type (might be due to invalid IL or missing references)
		//IL_0661: Unknown result type (might be due to invalid IL or missing references)
		//IL_0663: Unknown result type (might be due to invalid IL or missing references)
		//IL_066e: Unknown result type (might be due to invalid IL or missing references)
		//IL_067a: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_057c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0587: Unknown result type (might be due to invalid IL or missing references)
		//IL_04bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_04dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e8: Unknown result type (might be due to invalid IL or missing references)
		UnifiedRandom unifiedRandom = new UnifiedRandom(settings.RandomSeed);
		BiomeDungeonRoomSettings biomeDungeonRoomSettings = (BiomeDungeonRoomSettings)settings;
		ushort brickTileType = settings.StyleData.BrickTileType;
		ushort brickWallType = settings.StyleData.BrickWallType;
		byte? b = ((settings.OverridePaintTile > -1) ? new byte?((byte)settings.OverridePaintTile) : settings.StyleData.TilePaintColor);
		byte? b2 = ((settings.OverridePaintWall > -1) ? new byte?((byte)settings.OverridePaintWall) : settings.StyleData.WallPaintColor);
		Vector2 val = new Vector2((float)i, (float)j);
		int num = BiomeDungeonRoom.GetBiomeRoomInnerSize(biomeDungeonRoomSettings.StyleData);
		int num2 = 8;
		int num3 = BiomeDungeonRoom.GetBiomeRoomOuterSize(biomeDungeonRoomSettings.StyleData);
		if (calculated)
		{
			val = Position;
			num = RoomInnerSize;
			num3 = RoomOuterSize;
			num2 = WallDepth;
		}
		int num4 = 20;
		int num5 = Math.Max(num4 + num2, Math.Min(Main.maxTilesX - num4 - num2, (int)val.X - num));
		int num6 = Math.Max(num4 + num2, Math.Min(Main.maxTilesX - num4 - num2, (int)val.X + num));
		int num7 = Math.Max(num4 + num2, Math.Min(Main.maxTilesY - num4 - num2, (int)val.Y - num));
		int num8 = Math.Max(num4 + num2, Math.Min(Main.maxTilesY - num4 - num2, (int)val.Y + num));
		int num9 = Math.Max(num4, Math.Min(Main.maxTilesX - num4, (int)val.X - num3));
		int num10 = Math.Max(num4, Math.Min(Main.maxTilesX - num4, (int)val.X + num3));
		int num11 = Math.Max(num4, Math.Min(Main.maxTilesY - num4, (int)val.Y - num3));
		int num12 = Math.Max(num4, Math.Min(Main.maxTilesY - num4, (int)val.Y + num3));
		InnerBounds.SetBounds(num5, num7, num6, num8);
		OuterBounds.SetBounds(num9, num11, num10, num12);
		data.dungeonBounds.UpdateBounds(num9, num11, num10, num12);
		int num13 = num8 - num7;
		_ = InnerBounds.Center;
		int num14 = Math.Max(8, num2);
		for (int k = num9; k < num10; k++)
		{
			int num15 = k;
			float num16 = Utils.MultiLerp(Math.Max(0f, Math.Min(1f, (float)(k - num9) / Math.Max(1f, num10 - num9))), 1f, 0.95f, 0.85f, 0.25f, 0.1f, 0.05f, 0f, 0.05f, 0.1f, 0.25f, 0.85f, 0.95f, 1f);
			float num17 = (float)Math.Max(1, num13 / 16) + (float)Math.Max(1, num13 / 4) * num16;
			float num18 = (float)num7 + num17 + (float)unifiedRandom.Next(3);
			float num19 = num18 - (float)num14;
			float num20 = (float)num8 - num17 - 1f - (float)unifiedRandom.Next(3);
			float num21 = num20 + (float)num14;
			for (int l = num11; l < num12; l++)
			{
				bool flag = false;
				bool flag2 = false;
				int num22 = l;
				Tile tile = Main.tile[num15, num22];
				_ = Main.tile[num15, num22 - 1];
				_ = Main.tile[num15, num22 + 1];
				_ = Main.tile[num15, num22 + 2];
				if (generating && (tile.type == 484 || tile.type == 485))
				{
					tile.active(active: false);
				}
				if ((float)l < num19 || (float)l > num21)
				{
					continue;
				}
				if (k < num9 + num2 - 1 || k >= num10 - num2 + 1 || ((float)l >= num19 && (float)l <= num18) || ((float)l >= num20 && (float)l <= num21))
				{
					if (!generating)
					{
						_outerShapeData.Add(num15 - (int)val.X, num22 - (int)val.Y);
						continue;
					}
					DungeonUtils.ChangeTileType(tile, brickTileType, resetTile: false, b);
					if (tile.liquid > 0)
					{
						if (flag2)
						{
							tile.liquid = 0;
						}
						tile.liquidType(0);
					}
					if (flag)
					{
						DungeonUtils.ChangeWallType(tile, 0, resetTile: false, 0);
					}
					else
					{
						DungeonUtils.ChangeWallType(tile, brickWallType, resetTile: false, b2);
					}
				}
				else if (InnerBounds.Contains(num15, num22))
				{
					if (!generating)
					{
						_outerShapeData.Add(num15 - (int)val.X, num22 - (int)val.Y);
						_innerShapeData.Add(num15 - (int)val.X, num22 - (int)val.Y);
						continue;
					}
					if (tile.liquid > 0)
					{
						if (flag2)
						{
							tile.liquid = 0;
						}
						tile.liquidType(0);
					}
					if (flag)
					{
						DungeonUtils.ChangeWallType(tile, 0, resetTile: false, 0);
					}
					else
					{
						DungeonUtils.ChangeWallType(tile, brickWallType, resetTile: false, b2);
					}
					tile.active(active: false);
					tile.ClearBlockPaintAndCoating();
				}
				else if (!generating)
				{
					_outerShapeData.Add(num15 - (int)val.X, num22 - (int)val.Y);
				}
				else
				{
					if (tile.liquid > 0)
					{
						tile.liquid = 0;
					}
					DungeonUtils.ChangeTileType(tile, brickTileType, resetTile: true, b);
					DungeonUtils.ChangeWallType(tile, brickWallType, resetTile: false, b2);
				}
			}
		}
		BiomeRoom_AddHallwaySpace(val, num5, num6, num7, num8, 0, brickWallType, b2, generating);
		BiomeRoom_FinishRoom(unifiedRandom, num9, num10, num11, num12);
		RoomInnerSize = num;
		RoomOuterSize = num3;
		WallDepth = num2;
		Position = val;
		InnerBounds.CalculateHitbox();
		OuterBounds.CalculateHitbox();
	}
}
