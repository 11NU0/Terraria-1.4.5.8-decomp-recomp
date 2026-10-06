using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using ReLogic.Utilities;
using Terraria.GameContent.Generation.Dungeon.Rooms;
using Terraria.Utilities;

namespace Terraria.GameContent.Generation.Dungeon.Halls;

public class SineDungeonHall(DungeonHallSettings settings) : DungeonHall(settings)
{
	public List<Tuple<Vector2D, Vector2D>> PotentialPlatformPoints = new List<Tuple<Vector2D, Vector2D>>();

	public override void CalculatePlatformsAndDoors(DungeonData data)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		if (!Processed)
		{
			return;
		}
		DungeonUtils.CalculatePlatformAndDoorsOnHallway(data, StartPosition, StartDirection.Y, settings.ForceStyleForDoorsAndPlatforms ? settings.StyleData : null);
		DungeonUtils.CalculatePlatformAndDoorsOnHallway(data, EndPosition, EndDirection.Y, settings.ForceStyleForDoorsAndPlatforms ? settings.StyleData : null);
		float num = 0.65f;
		for (int i = 0; i < PotentialPlatformPoints.Count; i++)
		{
			Tuple<Vector2D, Vector2D> tuple = PotentialPlatformPoints[i];
			Vector2D item = tuple.Item1;
			Vector2D item2 = tuple.Item2;
			if (!(item2.Y < (double)num) || !(item2.Y > (double)(0f - num)))
			{
				DungeonUtils.CalculatePlatformAndDoorsOnHallway(data, item, item2.Y, settings.ForceStyleForDoorsAndPlatforms ? settings.StyleData : null);
			}
		}
	}

	public override void CalculateHall(DungeonData data, Vector2D startPoint, Vector2D endPoint)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		calculated = false;
		SineHall(data, startPoint, endPoint);
		calculated = true;
	}

	public override void GenerateHall(DungeonData data)
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		generated = false;
		SineHall(data, StartPosition, EndPosition, generating: true);
		generated = true;
	}

	public void SineHall(DungeonData data, Vector2D startPoint, Vector2D endPoint, bool generating = false)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0101: Unknown result type (might be due to invalid IL or missing references)
		//IL_0108: Unknown result type (might be due to invalid IL or missing references)
		//IL_010d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0112: Unknown result type (might be due to invalid IL or missing references)
		//IL_0117: Unknown result type (might be due to invalid IL or missing references)
		//IL_013f: Unknown result type (might be due to invalid IL or missing references)
		//IL_014b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0150: Unknown result type (might be due to invalid IL or missing references)
		//IL_028c: Unknown result type (might be due to invalid IL or missing references)
		//IL_029e: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0164: Unknown result type (might be due to invalid IL or missing references)
		//IL_0165: Unknown result type (might be due to invalid IL or missing references)
		//IL_0167: Unknown result type (might be due to invalid IL or missing references)
		//IL_015a: Unknown result type (might be due to invalid IL or missing references)
		//IL_015b: Unknown result type (might be due to invalid IL or missing references)
		//IL_015d: Unknown result type (might be due to invalid IL or missing references)
		//IL_016c: Unknown result type (might be due to invalid IL or missing references)
		//IL_016e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0175: Unknown result type (might be due to invalid IL or missing references)
		//IL_017e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0185: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01be: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01df: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0200: Unknown result type (might be due to invalid IL or missing references)
		//IL_0236: Unknown result type (might be due to invalid IL or missing references)
		//IL_0237: Unknown result type (might be due to invalid IL or missing references)
		//IL_0239: Unknown result type (might be due to invalid IL or missing references)
		//IL_023e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0218: Unknown result type (might be due to invalid IL or missing references)
		//IL_026b: Unknown result type (might be due to invalid IL or missing references)
		//IL_026c: Unknown result type (might be due to invalid IL or missing references)
		SineDungeonHallSettings sineDungeonHallSettings = (SineDungeonHallSettings)settings;
		UnifiedRandom unifiedRandom = new UnifiedRandom(sineDungeonHallSettings.RandomSeed);
		Vector2D val = startPoint;
		bool flag = false;
		if (sineDungeonHallSettings.CrackedBrickChance > 0.0)
		{
			flag = unifiedRandom.NextDouble() <= sineDungeonHallSettings.CrackedBrickChance;
		}
		int num = 3;
		int num2 = 8;
		int num3 = num + num2;
		Vector2D v = endPoint - startPoint;
		Vector2D val2 = v.SafeNormalize(Vector2D.UnitX);
		int num4 = (int)Math.Ceiling(v.Length() / val2.Length());
		int num5 = num4;
		Bounds.SetBounds((int)startPoint.X, (int)startPoint.Y, (int)startPoint.X, (int)startPoint.Y);
		DungeonRoomSearchSettings dungeonRoomSearchSettings = new DungeonRoomSearchSettings
		{
			Fluff = (int)((float)num * sineDungeonHallSettings.Magnitude) + num2
		};
		List<DungeonRoom> allRoomsInSpots = DungeonUtils.GetAllRoomsInSpots(data.dungeonRooms, startPoint, endPoint, dungeonRoomSearchSettings);
		Vector2D val3 = val2;
		Vector2D v2 = (Vector2.UnitY * sineDungeonHallSettings.Magnitude).ToVector2D();
		v2 = v2.ToVector2().RotatedBy(val3.ToRotation(), Vector2.Zero).ToVector2D();
		float num6 = 0f;
		float num7 = 0f;
		float num8 = (float)sineDungeonHallSettings.Iterations / (float)num5 * ((float)Math.PI * 2f);
		while (num4 > 0)
		{
			Vector2D val4 = v2 * (double)(float)Math.Sin(num7);
			Vector2D val5 = (sineDungeonHallSettings.FlipSine ? (val - val4) : (val + val4));
			if (!WorldGen.InWorld((int)(val5.X + val2.X), (int)(val5.Y + val2.Y), 10))
			{
				break;
			}
			if (!Processed)
			{
				data.dungeonBounds.UpdateBounds((int)val5.X - num3, (int)val5.Y - num3, (int)val5.Y + num3, (int)val5.Y + num3);
				Bounds.UpdateBounds((int)val5.X - num3, (int)val5.Y - num3, (int)val5.Y + num3, (int)val5.Y + num3);
			}
			if (generating)
			{
				GenerateDungeonSquareHall(data, allRoomsInSpots, val5, settings.StyleData, num, num2, sineDungeonHallSettings.PlaceOverProtectedBricks, flag);
			}
			val += val2;
			num6 += num8;
			num7 += num8;
			if (num6 >= 0.5f)
			{
				num6 = 0f;
				if (!Processed)
				{
					PotentialPlatformPoints.Add(new Tuple<Vector2D, Vector2D>(val, val4));
				}
			}
			num4--;
		}
		data.genVars.generatingDungeonPositionX = (int)endPoint.X;
		data.genVars.generatingDungeonPositionY = (int)endPoint.Y;
		StartPosition = startPoint;
		EndPosition = endPoint;
		StartDirection = new Vector2D(val3.X, val3.Y);
		EndDirection = new Vector2D(val2.X, val2.Y);
		CrackedBrick = flag;
		Bounds.CalculateHitbox();
	}
}
