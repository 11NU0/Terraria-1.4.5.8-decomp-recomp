using System.Collections.Generic;
using Microsoft.Xna.Framework;
using ReLogic.Utilities;
using Terraria.GameContent.Generation.Dungeon.Features;
using Terraria.Utilities;
using Terraria.WorldBuilding;

namespace Terraria.GameContent.Generation.Dungeon.Rooms;

public abstract class BiomeDungeonRoom : DungeonRoom
{
	protected const int BIOMEROOM_INNER_SIZE_BASE = 32;

	protected const int BIOMEROOM_INNER_SIZE_BASE_TEMPLE = 50;

	protected const int BIOMEROOM_WALL_DEPTH = 8;

	protected ShapeData _innerShapeData;

	protected ShapeData _outerShapeData;

	public BiomeDungeonRoom(DungeonRoomSettings settings)
		: base(settings)
	{
	}

	public override bool CanGenerateFeatureAt(DungeonData data, IDungeonFeature feature, int x, int y)
	{
		if (feature is DungeonDropTrap || feature is DungeonPitTrap || feature is DungeonGlobalSpikes || feature is DungeonGlobalTraps || feature is DungeonGlobalBookshelves)
		{
			return false;
		}
		return base.CanGenerateFeatureAt(data, feature, x, y);
	}

	public override void GeneratePreHallwaysDungeonFeaturesInRoom(DungeonData data)
	{
	}

	public override void GenerateEarlyDungeonFeaturesInRoom(DungeonData data)
	{
	}

	public override void CalculatePlatformsAndDoors(DungeonData data)
	{
		DungeonUtils.CalculatePlatformsAndDoorsOnEdgesOfRoom(data, InnerBounds, settings.ForceStyleForDoorsAndPlatforms ? settings.StyleData : null, 0, 0);
	}

	public override ConnectionPointQuality GetHallwayConnectionPoint(Vector2D otherRoomPos, out Vector2D connectionPoint)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		connectionPoint = (Vector2D)(Center);
		int num = ((!((otherRoomPos - connectionPoint).SafeNormalize(Vector2D.UnitX).X < 0.0)) ? 1 : (-1));
		while (IsInsideRoom(connectionPoint.ToPoint()))
		{
			connectionPoint.X += num;
		}
		return ConnectionPointQuality.Good;
	}

	public override ProtectionType GetProtectionTypeFromPoint(int x, int y)
	{
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		if (settings.StyleData.Style == 0 || settings.StyleData.Style == 10)
		{
			return ProtectionType.None;
		}
		if (_innerShapeData != null && _outerShapeData != null)
		{
			if (!_outerShapeData.Contains(x - Center.X, y - Center.Y))
			{
				return ProtectionType.None;
			}
			if (!_innerShapeData.Contains(x - Center.X, y - Center.Y))
			{
				return ProtectionType.Walls;
			}
			return ProtectionType.TilesAndWalls;
		}
		if (!OuterBounds.Contains(x, y))
		{
			return ProtectionType.None;
		}
		return ProtectionType.Walls;
	}

	public override bool IsInsideRoom(int x, int y)
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		if (base.IsInsideRoom(x, y))
		{
			if (_innerShapeData != null)
			{
				return _innerShapeData.Contains(x - Center.X, y - Center.Y);
			}
			return true;
		}
		return false;
	}

	public static int GetBiomeRoomInnerSize(DungeonGenerationStyleData styleData)
	{
		float num = (float)Main.maxTilesX / 4200f;
		if (styleData.Style == 10)
		{
			return (int)(50f * num);
		}
		return (int)(32f * num);
	}

	public static int GetBiomeRoomOuterSize(DungeonGenerationStyleData styleData)
	{
		return GetBiomeRoomInnerSize(styleData) + 8;
	}

	public void BiomeRoom_AddHallwaySpace(Vector2 position, int edgeLeft, int edgeRight, int edgeTop, int edgeBottom, int shapeOffset, ushort wallType, byte? wallPaint, bool generating)
	{
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		int num = (edgeTop + edgeBottom) / 2 - 3;
		int num2 = (edgeTop + edgeBottom) / 2 + 3;
		for (int i = edgeLeft; i < edgeRight; i++)
		{
			int num3 = i;
			for (int j = num; j < num2; j++)
			{
				int num4 = j;
				Tile tile = Main.tile[num3, num4];
				if (!generating && _innerShapeData != null)
				{
					_innerShapeData.Add(num3 - (int)position.X + shapeOffset, num4 - (int)position.Y + shapeOffset);
					_outerShapeData.Add(num3 - (int)position.X + shapeOffset, num4 - (int)position.Y + shapeOffset);
					continue;
				}
				tile.active(active: false);
				if (tile.wall != wallType)
				{
					DungeonUtils.ChangeWallType(tile, wallType, resetTile: false, wallPaint);
				}
			}
		}
	}

	public void BiomeRoom_FinishRoom(UnifiedRandom genRand, int outerRoomLeft, int outerRoomRight, int outerRoomTop, int outerRoomBottom, bool treatAsSurface = false)
	{
		List<int> heightsToSkip = new List<int>();
		for (int i = outerRoomLeft; i < outerRoomRight; i++)
		{
			heightsToSkip.Clear();
			int num = i;
			for (int j = outerRoomTop; j < outerRoomBottom; j++)
			{
				int num2 = j;
				Tile tile = Main.tile[num, num2];
				if (heightsToSkip.Contains(num2))
				{
					heightsToSkip.Remove(num2);
				}
				else
				{
					if (!InnerBounds.Contains(num, num2))
					{
						continue;
					}
					switch (settings.StyleData.Style)
					{
					default:
						if (tile.liquid > 0)
						{
							tile.liquidType(0);
						}
						break;
					case 4:
						if (treatAsSurface)
						{
							BiomeRoom_GrassySurface(genRand, ref heightsToSkip, num, num2, tile, 25, 23);
						}
						if (tile.liquid > 0)
						{
							tile.liquidType(0);
						}
						break;
					case 5:
						if (treatAsSurface)
						{
							BiomeRoom_GrassySurface(genRand, ref heightsToSkip, num, num2, tile, 203, 199);
						}
						if (tile.liquid > 0)
						{
							tile.liquidType(0);
						}
						break;
					case 6:
						if (treatAsSurface)
						{
							BiomeRoom_GrassySurface(genRand, ref heightsToSkip, num, num2, tile, 117, 109);
						}
						if (tile.liquid > 0)
						{
							tile.liquidType(0);
						}
						break;
					case 2:
						if (treatAsSurface)
						{
							BiomeRoom_SnowySurface(genRand, ref heightsToSkip, num, num2, tile);
						}
						if (tile.liquid > 0)
						{
							tile.liquidType(0);
						}
						break;
					case 3:
						if (treatAsSurface)
						{
							BiomeRoom_SandySurface(genRand, ref heightsToSkip, num, num2, tile);
						}
						if (tile.liquid > 0)
						{
							tile.liquidType(1);
						}
						break;
					}
				}
			}
		}
	}

	public void BiomeRoom_GrassySurface(UnifiedRandom genRand, ref List<int> heightsToSkip, int tileX, int tileY, Tile tile, ushort tileTypeToReplace, ushort grassTileType)
	{
		if (Main.tile[tileX, tileY - 1].active() || !tile.active() || tile.type != tileTypeToReplace)
		{
			return;
		}
		if (WorldGen.TileIsExposedToAir(tileX, tileY))
		{
			DungeonUtils.ChangeTileType(tile, grassTileType, resetTile: false, -1);
		}
		else
		{
			DungeonUtils.ChangeTileType(tile, 0, resetTile: false, -1);
		}
		for (int i = 0; i < 5 + genRand.Next(4); i++)
		{
			int num = tileY + i;
			Tile tile2 = Main.tile[tileX, num];
			if (tile2.active() && tile2.type == tileTypeToReplace)
			{
				heightsToSkip.Add(num);
				tile2.liquid = 0;
				if (WorldGen.TileIsExposedToAir(tileX, num))
				{
					DungeonUtils.ChangeTileType(tile2, grassTileType, resetTile: false, -1);
				}
				else
				{
					DungeonUtils.ChangeTileType(tile2, 0, resetTile: false, -1);
				}
			}
		}
	}

	public void BiomeRoom_SandySurface(UnifiedRandom genRand, ref List<int> heightsToSkip, int tileX, int tileY, Tile tile)
	{
		if (Main.tile[tileX, tileY - 1].active() || !tile.active() || tile.type != 396)
		{
			return;
		}
		if (WorldGen.BlockBelowMakesSandConvertIntoHardenedSand(tileX, tileY))
		{
			DungeonUtils.ChangeTileType(tile, 397, resetTile: false, -1);
		}
		else
		{
			DungeonUtils.ChangeTileType(tile, 53, resetTile: false, -1);
		}
		for (int i = 0; i < 5 + genRand.Next(4); i++)
		{
			int num = tileY + i;
			Tile tile2 = Main.tile[tileX, num];
			if (tile2.active() && tile2.type == 396)
			{
				heightsToSkip.Add(num);
				tile2.liquid = 0;
				if (WorldGen.BlockBelowMakesSandConvertIntoHardenedSand(tileX, num))
				{
					DungeonUtils.ChangeTileType(tile2, 397, resetTile: false, -1);
				}
				else
				{
					DungeonUtils.ChangeTileType(tile2, 53, resetTile: false, -1);
				}
			}
		}
	}

	public void BiomeRoom_SnowySurface(UnifiedRandom genRand, ref List<int> heightsToSkip, int tileX, int tileY, Tile tile)
	{
		if (Main.tile[tileX, tileY - 1].active() || !tile.active() || tile.type != 161)
		{
			return;
		}
		DungeonUtils.ChangeTileType(tile, 147, resetTile: false, -1);
		for (int i = 0; i < 5 + genRand.Next(4); i++)
		{
			int num = tileY + i;
			Tile tile2 = Main.tile[tileX, num];
			if (tile2.active() && tile2.type == 161)
			{
				heightsToSkip.Add(num);
				tile2.liquid = 0;
				DungeonUtils.ChangeTileType(tile2, 147, resetTile: false, -1);
			}
		}
	}
}
