using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using ReLogic.Utilities;
using Terraria.GameContent.Generation.Dungeon.Rooms;
using Terraria.Utilities;

namespace Terraria.GameContent.Generation.Dungeon.Halls;

public class LegacyEntranceDungeonHall : LegacyDungeonHall
{
	public int Direction;

	public LegacyEntranceDungeonHall(DungeonHallSettings settings)
		: base(settings)
	{
	}

	public override void CalculatePlatformsAndDoors(DungeonData data)
	{
	}

	public override void LegacyHall(DungeonData dungeonData, int i, int j, bool generating = false)
	{
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_016e: Unknown result type (might be due to invalid IL or missing references)
		//IL_022d: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0180: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0285: Unknown result type (might be due to invalid IL or missing references)
		//IL_028a: Unknown result type (might be due to invalid IL or missing references)
		//IL_028b: Unknown result type (might be due to invalid IL or missing references)
		//IL_028d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0290: Unknown result type (might be due to invalid IL or missing references)
		//IL_0296: Unknown result type (might be due to invalid IL or missing references)
		//IL_029b: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_031e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0325: Unknown result type (might be due to invalid IL or missing references)
		//IL_032b: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03be: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_03db: Unknown result type (might be due to invalid IL or missing references)
		//IL_03dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_03df: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0412: Unknown result type (might be due to invalid IL or missing references)
		//IL_0414: Unknown result type (might be due to invalid IL or missing references)
		//IL_041f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0421: Unknown result type (might be due to invalid IL or missing references)
		//IL_0335: Unknown result type (might be due to invalid IL or missing references)
		//IL_033c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0342: Unknown result type (might be due to invalid IL or missing references)
		//IL_034f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0354: Unknown result type (might be due to invalid IL or missing references)
		//IL_0357: Unknown result type (might be due to invalid IL or missing references)
		//IL_035c: Unknown result type (might be due to invalid IL or missing references)
		//IL_035e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0363: Unknown result type (might be due to invalid IL or missing references)
		//IL_0365: Unknown result type (might be due to invalid IL or missing references)
		//IL_0367: Unknown result type (might be due to invalid IL or missing references)
		//IL_036c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0371: Unknown result type (might be due to invalid IL or missing references)
		//IL_038d: Unknown result type (might be due to invalid IL or missing references)
		//IL_038f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0390: Unknown result type (might be due to invalid IL or missing references)
		//IL_0392: Unknown result type (might be due to invalid IL or missing references)
		//IL_0394: Unknown result type (might be due to invalid IL or missing references)
		//IL_0396: Unknown result type (might be due to invalid IL or missing references)
		//IL_0398: Unknown result type (might be due to invalid IL or missing references)
		//IL_0439: Unknown result type (might be due to invalid IL or missing references)
		//IL_0441: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a31: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a44: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a52: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a54: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a5a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a5c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a62: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a69: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a70: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a75: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a7b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a82: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a89: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a8e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0463: Unknown result type (might be due to invalid IL or missing references)
		//IL_049a: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ab9: Unknown result type (might be due to invalid IL or missing references)
		//IL_055b: Unknown result type (might be due to invalid IL or missing references)
		//IL_056a: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_082b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0856: Unknown result type (might be due to invalid IL or missing references)
		//IL_0881: Unknown result type (might be due to invalid IL or missing references)
		//IL_08ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_0601: Unknown result type (might be due to invalid IL or missing references)
		//IL_062e: Unknown result type (might be due to invalid IL or missing references)
		//IL_09f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_09f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_09f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_09fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_066d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a01: Unknown result type (might be due to invalid IL or missing references)
		//IL_08ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_08f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_08ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0904: Unknown result type (might be due to invalid IL or missing references)
		LegacyEntranceDungeonHallSettings legacyEntranceDungeonHallSettings = (LegacyEntranceDungeonHallSettings)settings;
		UnifiedRandom unifiedRandom = new UnifiedRandom(legacyEntranceDungeonHallSettings.RandomSeed);
		ushort brickTileType = settings.StyleData.BrickTileType;
		ushort brickCrackedTileType = settings.StyleData.BrickCrackedTileType;
		ushort brickWallType = settings.StyleData.BrickWallType;
		Vector2D val = Vector2D.Zero;
		Vector2D val2 = Vector2D.Zero;
		int num = Main.maxTilesX / 2;
		int num2 = unifiedRandom.Next(5, 9);
		int num3 = 1;
		val.X = i;
		val.Y = j;
		Vector2D startPosition = val;
		int num4 = unifiedRandom.Next(10, 30);
		num3 = ((i <= dungeonData.genVars.generatingDungeonTopX) ? 1 : (-1));
		if (i > Main.maxTilesX - 400)
		{
			num3 = -1;
		}
		else if (i < 400)
		{
			num3 = 1;
		}
		val2.Y = -1.0;
		val2.X = num3;
		if (unifiedRandom.Next(3) != 0)
		{
			val2.X *= 1f + (float)unifiedRandom.Next(0, 200) * 0.01f;
		}
		else if (unifiedRandom.Next(3) == 0)
		{
			val2.X *= (float)unifiedRandom.Next(50, 76) * 0.01f;
		}
		else if (unifiedRandom.Next(6) == 0)
		{
			val2.Y *= 2.0;
		}
		if (dungeonData.useSkewedDungeonEntranceHalls)
		{
			if (dungeonData.genVars.generatingDungeonPositionX < num && val2.X < 0.0 && val2.X < -0.5)
			{
				val2.X = 0.5;
			}
			if (dungeonData.genVars.generatingDungeonPositionX > num && val2.X > 0.0 && val2.X > 0.5)
			{
				val2.X = -0.5;
			}
		}
		else
		{
			if (dungeonData.genVars.generatingDungeonPositionX < num && val2.X < -0.5)
			{
				val2.X = -0.5;
			}
			if (dungeonData.genVars.generatingDungeonPositionX > num && val2.X > 0.5)
			{
				val2.X = 0.5;
			}
		}
		if (WorldGen.drunkWorldGen || WorldGen.SecretSeed.noSurface.Enabled)
		{
			num3 *= -1;
			val2.X *= -1.0;
		}
		if (calculated)
		{
			val = (startPosition = StartPosition);
			val2 = (EndPosition - StartPosition).SafeNormalize(Vector2D.UnitX);
			num3 = Direction;
			num2 = Strength;
			num4 = Steps;
		}
		int strength = num2;
		int steps = num4;
		double num5 = dungeonData.hallInteriorToExteriorRatio;
		if ((float)legacyEntranceDungeonHallSettings.OverrideStrength > 0f)
		{
			num2 = (strength = legacyEntranceDungeonHallSettings.OverrideStrength);
		}
		if (legacyEntranceDungeonHallSettings.OverrideSteps > 0)
		{
			num4 = (steps = legacyEntranceDungeonHallSettings.OverrideSteps);
		}
		if (legacyEntranceDungeonHallSettings.OverrideInteriorToExteriorRatio > 0.0)
		{
			num5 = legacyEntranceDungeonHallSettings.OverrideInteriorToExteriorRatio;
		}
		bool flag = false;
		if (OverrideStartPosition != default(Vector2D) && OverrideEndPosition != default(Vector2D))
		{
			flag = true;
			Vector2D overrideStartPosition = OverrideStartPosition;
			Vector2D v = OverrideEndPosition - overrideStartPosition;
			Vector2D val3 = v.SafeNormalize(Vector2D.UnitX);
			num4 = (steps = (int)Math.Ceiling(v.Length() / val3.Length()));
			val = (startPosition = overrideStartPosition);
			val2 = val3;
			num3 = ((val3.X > 0.0) ? 1 : (-1));
		}
		Bounds.SetBounds((int)val.X, (int)val.Y, (int)val.X, (int)val.Y);
		Vector2D startPos = val;
		Vector2D endPos = val + val2 * (double)num4;
		DungeonRoomSearchSettings dungeonRoomSearchSettings = new DungeonRoomSearchSettings
		{
			Fluff = num4 / 2 + num2
		};
		List<DungeonRoom> allRoomsInSpots = DungeonUtils.GetAllRoomsInSpots(dungeonData.dungeonRooms, startPos, endPos, dungeonRoomSearchSettings);
		Vector2D val4 = val2;
		int num6 = 30;
		int num7 = 10;
		int num8 = 0;
		while (num4 > 0)
		{
			num4--;
			if (!WorldGen.InWorld((int)val.X, (int)val.Y, num6 + 5))
			{
				break;
			}
			int num9 = Math.Max(num6, Math.Min(Main.maxTilesX - num6 - 1, (int)(val.X - (double)num2 - 4.0 - (double)unifiedRandom.Next(6))));
			int num10 = Math.Max(num6, Math.Min(Main.maxTilesX - num6 - 1, (int)(val.X + (double)num2 + 4.0 + (double)unifiedRandom.Next(6))));
			int num11 = Math.Max(num6, Math.Min(Main.maxTilesY - num6 - 1, (int)(val.Y - (double)num2 - 4.0)));
			int num12 = Math.Max(num6, Math.Min(Main.maxTilesY - num6 - 1, (int)(val.Y + (double)num2 + 4.0 + (double)unifiedRandom.Next(6))));
			if (!Processed)
			{
				dungeonData.dungeonBounds.UpdateBounds(num9, num11, num10, num12);
				Bounds.UpdateBounds(num9, num11, num10, num12);
			}
			int num13 = 1;
			if (val.X > (double)num)
			{
				num13 = -1;
			}
			int num14 = (int)(val.X + dungeonData.dungeonEntranceStrengthX * 0.6 * (double)num13 + dungeonData.dungeonEntranceStrengthX2 * (double)num13);
			int num15 = (int)(dungeonData.dungeonEntranceStrengthY2 * 0.5);
			if (!legacyEntranceDungeonHallSettings.UsePrecalculatedEntrance && val.Y < Main.worldSurface - 5.0 && ((Main.tile[num14, (int)(val.Y - (double)num2 - 6.0 + (double)num15)].wall == 0 && Main.tile[num14, (int)(val.Y - (double)num2 - 7.0 + (double)num15)].wall == 0 && Main.tile[num14, (int)(val.Y - (double)num2 - 8.0 + (double)num15)].wall == 0) || WorldGen.SecretSeed.surfaceIsDesert.Enabled))
			{
				dungeonData.createdDungeonEntranceOnSurface = true;
				if (generating)
				{
					WorldGen.TileRunner(num14, (int)(val.Y - (double)num2 - 6.0 + (double)num15), unifiedRandom.Next(25, 35), unifiedRandom.Next(10, 20), -1, addTile: false, 0.0, -1.0);
				}
			}
			if (generating && !settings.CarveOnly)
			{
				for (int k = num9; k < num10; k++)
				{
					for (int l = num11; l < num12; l++)
					{
						bool flag2 = true;
						ProtectionType highestProtectionTypeFromPoint = DungeonUtils.GetHighestProtectionTypeFromPoint(k, l, allRoomsInSpots);
						if (highestProtectionTypeFromPoint != ProtectionType.TilesAndWalls)
						{
							if (highestProtectionTypeFromPoint == ProtectionType.Tiles)
							{
								flag2 = false;
							}
							Tile tile = Main.tile[k, l];
							tile.liquid = 0;
							if (flag2 && CanPlaceTileAt(dungeonData, tile, brickTileType, brickCrackedTileType))
							{
								WorldGen.paintTile(k, l, 0, broadCast: false, paintEffects: false);
								DungeonUtils.ChangeTileType(Main.tile[k, l], brickTileType, resetTile: true, legacyEntranceDungeonHallSettings.OverridePaintTile);
							}
						}
					}
				}
				for (int m = num9 + 1; m < num10 - 1; m++)
				{
					for (int n = num11 + 1; n < num12 - 1; n++)
					{
						bool flag3 = true;
						ProtectionType highestProtectionTypeFromPoint2 = DungeonUtils.GetHighestProtectionTypeFromPoint(m, n, allRoomsInSpots);
						if (highestProtectionTypeFromPoint2 != ProtectionType.TilesAndWalls)
						{
							if (highestProtectionTypeFromPoint2 == ProtectionType.Walls && DungeonUtils.IsConsideredDungeonWall(Main.tile[m, n].wall))
							{
								flag3 = false;
							}
							if (flag3)
							{
								WorldGen.paintWall(m, n, 0, broadCast: false, paintEffects: false);
								DungeonUtils.ChangeWallType(Main.tile[m, n], brickWallType, resetTile: false, legacyEntranceDungeonHallSettings.OverridePaintWall);
							}
						}
					}
				}
			}
			int num16 = 0;
			if (unifiedRandom.Next(num2) == 0)
			{
				num16 = unifiedRandom.Next(1, 3);
			}
			num9 = Math.Max(num6, Math.Min(Main.maxTilesX - num6 - 1, (int)(val.X - (double)num2 * num5 - (double)num16)));
			num10 = Math.Max(num6, Math.Min(Main.maxTilesX - num6 - 1, (int)(val.X + (double)num2 * num5 + (double)num16)));
			num11 = Math.Max(num6, Math.Min(Main.maxTilesY - num6 - 1, (int)(val.Y - (double)num2 * num5 - (double)num16)));
			num12 = Math.Max(num6, Math.Min(Main.maxTilesY - num6 - 1, (int)(val.Y + (double)num2 * num5 + (double)num16)));
			if (generating)
			{
				if (flag)
				{
					num8--;
					if (num8 <= 0)
					{
						num8 = num7;
						DungeonPlatformData item = new DungeonPlatformData
						{
							Position = new Point((int)val.X, (int)val.Y),
							PlacePotsChance = 0.25,
							InAHallway = true
						};
						dungeonData.dungeonPlatformData.Add(item);
					}
				}
				for (int num17 = num9; num17 < num10; num17++)
				{
					for (int num18 = num11; num18 < num12; num18++)
					{
						bool flag4 = true;
						ProtectionType highestProtectionTypeFromPoint3 = DungeonUtils.GetHighestProtectionTypeFromPoint(num17, num18, allRoomsInSpots);
						if (highestProtectionTypeFromPoint3 != ProtectionType.TilesAndWalls)
						{
							if (highestProtectionTypeFromPoint3 == ProtectionType.Walls && DungeonUtils.IsConsideredDungeonWall(Main.tile[num17, num18].wall))
							{
								flag4 = false;
							}
							Main.tile[num17, num18].ClearTile();
							if (flag4 && !settings.CarveOnly)
							{
								DungeonUtils.ChangeWallType(Main.tile[num17, num18], brickWallType, resetTile: false, legacyEntranceDungeonHallSettings.OverridePaintWall);
							}
						}
					}
				}
			}
			if (!legacyEntranceDungeonHallSettings.UsePrecalculatedEntrance && dungeonData.createdDungeonEntranceOnSurface)
			{
				num4 = 0;
			}
			val += val2;
			if (!flag && val.Y < Main.worldSurface)
			{
				val2.Y *= 0.9800000190734863;
			}
		}
		dungeonData.genVars.generatingDungeonPositionX = (int)val.X;
		dungeonData.genVars.generatingDungeonPositionY = (int)val.Y;
		StartPosition = startPosition;
		EndPosition = val;
		StartDirection = new Vector2D(val4.X, val4.Y);
		EndDirection = new Vector2D(val2.X, val2.Y);
		Strength = strength;
		Steps = steps;
		Direction = num3;
		if (!Processed)
		{
			Bounds.CalculateHitbox();
		}
	}
}
