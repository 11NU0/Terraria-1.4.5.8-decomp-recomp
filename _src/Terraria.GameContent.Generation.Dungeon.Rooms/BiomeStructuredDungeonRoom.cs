using System;
using Microsoft.Xna.Framework;
using Terraria.GameContent.Generation.Dungeon.Features;
using Terraria.Utilities;
using Terraria.WorldBuilding;

namespace Terraria.GameContent.Generation.Dungeon.Rooms;

public class BiomeStructuredDungeonRoom : BiomeDungeonRoom
{
	public const int VARIANT_DOUBLEDIAMOND = 0;

	public const int VARIANT_ROUNDED = 1;

	public const int VARIANT_CANDY = 2;

	public const int VARIANT_WIGGLED = 3;

	public const int MAX_VARIANTS = 4;

	public Vector2 Position;

	public int RoomInnerSize;

	public int RoomOuterSize;

	public int WallDepth;

	public BiomeStructuredDungeonRoom(DungeonRoomSettings settings)
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

	public override void GenerateEarlyDungeonFeaturesInRoom(DungeonData data)
	{
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		UnifiedRandom unifiedRandom = new UnifiedRandom(settings.RandomSeed);
		BiomeDungeonRoomSettings biomeDungeonRoomSettings = (BiomeDungeonRoomSettings)settings;
		_ = settings.StyleData.Style;
		int x = InnerBounds.Center.X;
		int y = InnerBounds.Center.Y;
		int width = InnerBounds.Width;
		int height = InnerBounds.Height;
		int bottom = InnerBounds.Bottom;
		int num = (int)((float)width * 0.25f);
		int num2 = unifiedRandom.Next(2);
		if (num2 == 1 && !biomeDungeonRoomSettings.StyleData.CanGenerateFeatureAt(data, this, null, x, y))
		{
			num2 = 0;
		}
		if (num2 == 0 || num2 != 1)
		{
			DungeonWindowBasicSettings dungeonWindowBasicSettings = new DungeonWindowBasicSettings
			{
				Style = settings.StyleData,
				Width = 9,
				Height = height / 5,
				Closed = (unifiedRandom.Next(3) != 0)
			};
			new DungeonWindowBasic(dungeonWindowBasicSettings).GenerateFeature(data, x, y);
			dungeonWindowBasicSettings.Width = 7;
			dungeonWindowBasicSettings.Height = height / 5 - 4;
			new DungeonWindowBasic(dungeonWindowBasicSettings).GenerateFeature(data, x - num, y + 3);
			new DungeonWindowBasic(dungeonWindowBasicSettings).GenerateFeature(data, x + num, y + 3);
		}
		else
		{
			int num3 = 4;
			DungeonPillarSettings dungeonPillarSettings = new DungeonPillarSettings();
			dungeonPillarSettings.Style = settings.StyleData;
			dungeonPillarSettings.OverridePaintTile = settings.OverridePaintTile;
			dungeonPillarSettings.OverridePaintWall = settings.OverridePaintWall;
			dungeonPillarSettings.PillarType = PillarType.BlockActuatedSolidTop;
			dungeonPillarSettings.Width = 10;
			dungeonPillarSettings.Height = bottom - y + 5;
			dungeonPillarSettings.CrowningOnTop = true;
			dungeonPillarSettings.CrowningOnBottom = false;
			dungeonPillarSettings.CrowningStopsAtPillar = true;
			dungeonPillarSettings.AlwaysPlaceEntirePillar = false;
			new DungeonPillar(dungeonPillarSettings).GenerateFeature(data, x + 1, bottom + num3);
			dungeonPillarSettings.Width = 7;
			dungeonPillarSettings.Height = bottom - y;
			new DungeonPillar(dungeonPillarSettings).GenerateFeature(data, x - num + 1, bottom + num3);
			new DungeonPillar(dungeonPillarSettings).GenerateFeature(data, x + num, bottom + num3);
		}
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
		//IL_026f: Unknown result type (might be due to invalid IL or missing references)
		//IL_027b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_06b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_06f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_06f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_06fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0709: Unknown result type (might be due to invalid IL or missing references)
		//IL_048b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0499: Unknown result type (might be due to invalid IL or missing references)
		//IL_05fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_060a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0549: Unknown result type (might be due to invalid IL or missing references)
		//IL_0557: Unknown result type (might be due to invalid IL or missing references)
		//IL_0570: Unknown result type (might be due to invalid IL or missing references)
		//IL_057e: Unknown result type (might be due to invalid IL or missing references)
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
		InnerBounds.SetBounds(num5, num7, num6 - 1, num8 - 1);
		OuterBounds.SetBounds(num9, num11, num10 - 1, num12 - 1);
		data.dungeonBounds.UpdateBounds(num9, num11, num10 - 1, num12 - 1);
		int num13 = num10 - num9;
		int num14 = num12 - num11;
		_ = OuterBounds.Center;
		_ = OuterBounds.Center;
		int variant = unifiedRandom.Next(4);
		int num15 = 1;
		for (int k = num9; k < num10; k++)
		{
			int num16 = k;
			float percentile = Math.Max(0f, Math.Min(1f, (float)(k - num9) / Math.Max(1f, num10 - 1 - num9)));
			float num17 = BiomeRoom_GetYPercent(unifiedRandom, variant, percentile);
			float num18 = (float)Math.Max(1, num14 / 16) + (float)Math.Max(1, num14 / 4) * num17;
			float num19 = (float)num7 + num18;
			float num20 = num19 - (float)num2;
			float num21 = (float)num8 - num18 - 1f;
			float num22 = num21 + (float)num2;
			for (int l = num11; l < num12; l++)
			{
				int num23 = l;
				float percentile2 = Math.Max(0f, Math.Min(1f, (float)(l - num11) / Math.Max(1f, num12 - 1 - num11)));
				float num24 = BiomeRoom_GetXPercent(unifiedRandom, variant, percentile2);
				float num25 = (float)Math.Max(1, num13 / 4) * num24;
				float num26 = (float)num5 + num25;
				float num27 = num26 - (float)num2;
				float num28 = (float)num6 - num25 - 1f;
				float num29 = num28 + (float)num2;
				Tile tile = Main.tile[num16, num23];
				_ = Main.tile[num16, num23 - 1];
				_ = Main.tile[num16, num23 + 1];
				_ = Main.tile[num16, num23 + 2];
				if (generating && (tile.type == 484 || tile.type == 485))
				{
					tile.active(active: false);
				}
				if ((float)l < num20 || (float)l > num22 || (float)k < num27 || (float)k > num29)
				{
					continue;
				}
				if (k <= num9 + num2 - 1 || k >= num10 - num2 + 1 || ((float)l >= num20 && (float)l <= num19) || ((float)l >= num21 && (float)l <= num22) || ((float)k >= num27 && (float)k <= num26) || ((float)k >= num28 && (float)k <= num29))
				{
					if (!generating)
					{
						_outerShapeData.Add(num16 - (int)val.X + num15, num23 - (int)val.Y + num15);
						continue;
					}
					DungeonUtils.ChangeTileType(tile, brickTileType, resetTile: false, b);
					if (tile.liquid > 0)
					{
						tile.liquid = 0;
						tile.liquidType(0);
					}
					DungeonUtils.ChangeWallType(tile, brickWallType, resetTile: false, b2);
				}
				else if (InnerBounds.Contains(num16, num23))
				{
					if (!generating)
					{
						_innerShapeData.Add(num16 - (int)val.X + num15, num23 - (int)val.Y + num15);
						_outerShapeData.Add(num16 - (int)val.X + num15, num23 - (int)val.Y + num15);
						continue;
					}
					if (tile.liquid > 0)
					{
						tile.liquid = 0;
						tile.liquidType(0);
					}
					DungeonUtils.ChangeWallType(tile, brickWallType, resetTile: false, b2);
					tile.active(active: false);
					tile.ClearBlockPaintAndCoating();
				}
				else if (!generating)
				{
					_outerShapeData.Add(num16 - (int)val.X + num15, num23 - (int)val.Y + num15);
				}
				else
				{
					if (tile.liquid > 0)
					{
						tile.liquid = 0;
						tile.liquidType(0);
					}
					DungeonUtils.ChangeTileType(tile, brickTileType, resetTile: true, b);
					DungeonUtils.ChangeWallType(tile, brickWallType, resetTile: false, b2);
				}
			}
		}
		BiomeRoom_AddHallwaySpace(val, num5, num6, num7, num8, num15, brickWallType, b2, generating);
		BiomeRoom_FinishRoom(unifiedRandom, num9, num10, num11, num12);
		RoomInnerSize = num;
		RoomOuterSize = num3;
		WallDepth = num2;
		Position = val;
		InnerBounds.CalculateHitbox();
		OuterBounds.CalculateHitbox();
	}

	public float BiomeRoom_GetXPercent(UnifiedRandom genRand, int variant, float percentile)
	{
		return variant switch
		{
			1 => Utils.MultiLerp(Utils.WrappedLerp(0f, 1f, percentile), 0f, 0.9f, 0.25f, 0f), 
			2 => 0f, 
			3 => 0f, 
			_ => Utils.MultiLerp(Utils.WrappedLerp(0f, 1f, percentile), 0.1f, 0.5f, 0.1f), 
		};
	}

	public float BiomeRoom_GetYPercent(UnifiedRandom genRand, int variant, float percentile)
	{
		return variant switch
		{
			1 => 0f, 
			2 => Utils.MultiLerp(Utils.WrappedLerp(0f, 1f, percentile), 1f, 1f, 0.5f, 0f, 0.5f, 0.25f, 0.1f), 
			3 => Utils.MultiLerp(Utils.WrappedLerp(0f, 1f, percentile), 0f, 0.25f, 0.6f, 0.25f, 0f, 0f, 0.3f, 0.4f, 0.3f, 0f, 0f), 
			_ => Utils.MultiLerp(Utils.WrappedLerp(0f, 1f, percentile), 1f, 0f, 0.5f, 0.5f, 0f, 0f, 0.5f), 
		};
	}
}
