using System;
using Microsoft.Xna.Framework;
using Terraria.Utilities;

namespace Terraria.GameContent.Generation.Dungeon.Rooms;

public class BiomeSquareDungeonRoom : BiomeDungeonRoom
{
	public Vector2 Position;

	public int RoomInnerSize;

	public int RoomOuterSize;

	public int WallDepth;

	public BiomeSquareDungeonRoom(DungeonRoomSettings settings)
		: base(settings)
	{
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
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_043b: Unknown result type (might be due to invalid IL or missing references)
		//IL_043d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0448: Unknown result type (might be due to invalid IL or missing references)
		//IL_0454: Unknown result type (might be due to invalid IL or missing references)
		//IL_025c: Unknown result type (might be due to invalid IL or missing references)
		UnifiedRandom genRand = new UnifiedRandom(settings.RandomSeed);
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
		int minX = Math.Max(num4 + num2, Math.Min(Main.maxTilesX - num4 - num2, (int)val.X - num));
		int maxX = Math.Max(num4 + num2, Math.Min(Main.maxTilesX - num4 - num2, (int)val.X + num));
		int minY = Math.Max(num4 + num2, Math.Min(Main.maxTilesY - num4 - num2, (int)val.Y - num));
		int maxY = Math.Max(num4 + num2, Math.Min(Main.maxTilesY - num4 - num2, (int)val.Y + num));
		int num5 = Math.Max(num4, Math.Min(Main.maxTilesX - num4, (int)val.X - num3));
		int num6 = Math.Max(num4, Math.Min(Main.maxTilesX - num4, (int)val.X + num3));
		int num7 = Math.Max(num4, Math.Min(Main.maxTilesY - num4, (int)val.Y - num3));
		int num8 = Math.Max(num4, Math.Min(Main.maxTilesY - num4, (int)val.Y + num3));
		InnerBounds.SetBounds(minX, minY, maxX, maxY);
		OuterBounds.SetBounds(num5, num7, num6, num8);
		data.dungeonBounds.UpdateBounds(num5, num7, num6, num8);
		if (generating)
		{
			_ = OuterBounds.Center;
			for (int k = num5; k < num6; k++)
			{
				int num9 = k;
				for (int l = num7; l < num8; l++)
				{
					bool flag = false;
					bool flag2 = false;
					int num10 = l;
					Tile tile = Main.tile[num9, num10];
					_ = Main.tile[num9, num10 - 1];
					_ = Main.tile[num9, num10 + 1];
					_ = Main.tile[num9, num10 + 2];
					if (tile.type == 484 || tile.type == 485)
					{
						tile.active(active: false);
					}
					if (InnerBounds.Contains(num9, num10))
					{
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
							DungeonUtils.ChangeWallType(tile, 0, resetTile: false, b2);
						}
						else
						{
							DungeonUtils.ChangeWallType(tile, brickWallType, resetTile: false, b2);
						}
						tile.active(active: false);
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
			BiomeRoom_FinishRoom(genRand, num5, num6, num7, num8);
		}
		RoomInnerSize = num;
		RoomOuterSize = num3;
		WallDepth = num2;
		Position = val;
		InnerBounds.CalculateHitbox();
		OuterBounds.CalculateHitbox();
	}
}
