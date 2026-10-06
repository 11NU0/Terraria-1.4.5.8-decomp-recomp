using System;
using System.Collections.Generic;
using ReLogic.Utilities;
using Terraria.GameContent.Generation.Dungeon.Rooms;
using Terraria.Utilities;

namespace Terraria.GameContent.Generation.Dungeon.Halls;

public class StairwellDungeonHall : DungeonHall
{
	public StairwellDungeonHall(StairwellDungeonHallSettings settings)
		: base(settings)
	{
	}

	public override void CalculatePlatformsAndDoors(DungeonData data)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		if (Processed)
		{
			DungeonUtils.CalculatePlatformAndDoorsOnHallway(data, StartPosition, StartDirection.Y, settings.ForceStyleForDoorsAndPlatforms ? settings.StyleData : null);
			DungeonUtils.CalculatePlatformAndDoorsOnHallway(data, EndPosition, EndDirection.Y, settings.ForceStyleForDoorsAndPlatforms ? settings.StyleData : null);
		}
	}

	public override void CalculateHall(DungeonData data, Vector2D startPoint, Vector2D endPoint)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		calculated = false;
		Stairwell(data, startPoint, endPoint);
		calculated = true;
	}

	public override void GenerateHall(DungeonData data)
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		generated = false;
		Stairwell(data, StartPosition, EndPosition, generating: true);
		generated = true;
	}

	public void Stairwell(DungeonData data, Vector2D startPoint, Vector2D endPoint, bool generating = false)
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0106: Unknown result type (might be due to invalid IL or missing references)
		//IL_010b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0110: Unknown result type (might be due to invalid IL or missing references)
		//IL_0111: Unknown result type (might be due to invalid IL or missing references)
		//IL_0118: Unknown result type (might be due to invalid IL or missing references)
		//IL_011f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0124: Unknown result type (might be due to invalid IL or missing references)
		//IL_0129: Unknown result type (might be due to invalid IL or missing references)
		//IL_012b: Unknown result type (might be due to invalid IL or missing references)
		//IL_012d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0141: Unknown result type (might be due to invalid IL or missing references)
		//IL_0146: Unknown result type (might be due to invalid IL or missing references)
		//IL_014b: Unknown result type (might be due to invalid IL or missing references)
		//IL_014d: Unknown result type (might be due to invalid IL or missing references)
		//IL_014f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0151: Unknown result type (might be due to invalid IL or missing references)
		//IL_0165: Unknown result type (might be due to invalid IL or missing references)
		//IL_016a: Unknown result type (might be due to invalid IL or missing references)
		//IL_016f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0173: Unknown result type (might be due to invalid IL or missing references)
		//IL_0175: Unknown result type (might be due to invalid IL or missing references)
		//IL_0183: Unknown result type (might be due to invalid IL or missing references)
		//IL_0185: Unknown result type (might be due to invalid IL or missing references)
		//IL_0191: Unknown result type (might be due to invalid IL or missing references)
		//IL_0193: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01da: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0201: Unknown result type (might be due to invalid IL or missing references)
		//IL_0203: Unknown result type (might be due to invalid IL or missing references)
		//IL_0215: Unknown result type (might be due to invalid IL or missing references)
		StairwellDungeonHallSettings stairwellDungeonHallSettings = (StairwellDungeonHallSettings)settings;
		UnifiedRandom unifiedRandom = new UnifiedRandom(stairwellDungeonHallSettings.RandomSeed);
		Bounds.SetBounds((int)startPoint.X, (int)startPoint.Y, (int)startPoint.X, (int)startPoint.Y);
		bool flag = false;
		if (stairwellDungeonHallSettings.CrackedBrickChance > 0.0)
		{
			flag = unifiedRandom.NextDouble() <= stairwellDungeonHallSettings.CrackedBrickChance;
		}
		int innerBoundsSize = stairwellDungeonHallSettings.InnerBoundsSize;
		int outerBoundsSize = stairwellDungeonHallSettings.OuterBoundsSize;
		int num = innerBoundsSize + outerBoundsSize;
		List<DungeonRoom> allRoomsInSpots = DungeonUtils.GetAllRoomsInSpots(data.dungeonRooms, startPoint, endPoint, new DungeonRoomSearchSettings
		{
			Fluff = num + stairwellDungeonHallSettings.MaxDistFromLine
		});
		Vector2D val = endPoint - startPoint;
		Vector2D val2 = CalculateZigZagSlope(startPoint, endPoint, stairwellDungeonHallSettings.Gradient);
		double num2 = Math.Ceiling(Math.Abs(Vector2D.Cross(val2, val.SafeNormalize(default))) / (double)stairwellDungeonHallSettings.MaxDistFromLine);
		val2 /= num2;
		Vector2D startPoint2 = startPoint;
		for (int i = 0; (double)i < num2; i++)
		{
			Vector2D val3 = startPoint + val * (double)i / num2;
			Vector2D val4 = startPoint + val * (double)(i + 1) / num2;
			Vector2D val5 = val3 + val2 + unifiedRandom.NextVector2DCircular(stairwellDungeonHallSettings.PointVariance, stairwellDungeonHallSettings.PointVariance);
			Vector2D val6 = val4 - val2 + unifiedRandom.NextVector2DCircular(stairwellDungeonHallSettings.PointVariance, stairwellDungeonHallSettings.PointVariance);
			GenerateHallway(data, startPoint2, val5, allRoomsInSpots, flag, generating);
			GenerateHallway(data, val5, val6, allRoomsInSpots, flag, generating);
			startPoint2 = val6;
		}
		GenerateHallway(data, startPoint2, endPoint, allRoomsInSpots, flag, generating);
		data.genVars.generatingDungeonPositionX = (int)endPoint.X;
		data.genVars.generatingDungeonPositionY = (int)endPoint.Y;
		StartPosition = startPoint;
		EndPosition = endPoint;
		StartDirection = (EndDirection = val2.SafeNormalize(default));
		CrackedBrick = flag;
		Bounds.CalculateHitbox();
	}

	private Vector2D CalculateZigZagSlope(Vector2D startPoint, Vector2D endPoint, double gradient)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		Vector2D val = (endPoint - startPoint).SafeNormalize(Vector2D.UnitY);
		Vector2D val2 = new Vector2D((double)((val.X > 0.0) ? 1 : (-1)), gradient).SafeNormalize(default);
		double num = Vector2D.Distance(startPoint, endPoint) * (val.X / val2.X + val.Y / val2.Y) / 4.0;
		return val2 * num;
	}

	private void GenerateHallway(DungeonData data, Vector2D startPoint, Vector2D endPoint, List<DungeonRoom> roomsInArea, bool crackedBricks, bool generating)
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Unknown result type (might be due to invalid IL or missing references)
		StairwellDungeonHallSettings stairwellDungeonHallSettings = (StairwellDungeonHallSettings)settings;
		int innerBoundsSize = stairwellDungeonHallSettings.InnerBoundsSize;
		int outerBoundsSize = stairwellDungeonHallSettings.OuterBoundsSize;
		int num = innerBoundsSize + outerBoundsSize;
		Vector2D val = endPoint - startPoint;
		double num2 = Math.Ceiling(Math.Max(Math.Abs(val.X), Math.Abs(val.Y)));
		for (int i = 0; (double)i < num2; i++)
		{
			Vector2D val2 = startPoint + val * ((double)i / num2);
			if (!Processed)
			{
				data.dungeonBounds.UpdateBounds((int)val2.X - num, (int)val2.Y - num, (int)val2.Y + num, (int)val2.Y + num);
				Bounds.UpdateBounds((int)val2.X - num, (int)val2.Y - num, (int)val2.Y + num, (int)val2.Y + num);
			}
			if (generating)
			{
				GenerateDungeonSquareHall(data, roomsInArea, val2, settings.StyleData, innerBoundsSize, outerBoundsSize, stairwellDungeonHallSettings.PlaceOverProtectedBricks, crackedBricks, stairwellDungeonHallSettings.IsEntranceHall);
			}
		}
	}

	public override bool CanPlaceTileAt(DungeonData data, Tile tile, int tileType, int tileCrackedType)
	{
		if (((StairwellDungeonHallSettings)settings).IsEntranceHall && ((tile.active() && Main.tileDungeon[tile.type]) || Main.wallDungeon[tile.wall]))
		{
			return false;
		}
		return base.CanPlaceTileAt(data, tile, tileType, tileCrackedType);
	}
}
