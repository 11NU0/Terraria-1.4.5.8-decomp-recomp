using System;
using System.Collections.Generic;
using ReLogic.Utilities;
using Terraria.GameContent.Generation.Dungeon.Rooms;
using Terraria.Utilities;

namespace Terraria.GameContent.Generation.Dungeon.Halls;

public class RegularDungeonHall : DungeonHall
{
	public RegularDungeonHall(DungeonHallSettings settings)
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
			AddExtraPlatformsIfNeeded(data);
		}
	}

	public override void CalculateHall(DungeonData data, Vector2D startPoint, Vector2D endPoint)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		calculated = false;
		RegularHall(data, startPoint, endPoint);
		calculated = true;
	}

	public override void GenerateHall(DungeonData data)
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		generated = false;
		RegularHall(data, StartPosition, EndPosition, generating: true);
		generated = true;
	}

	public void RegularHall(DungeonData data, Vector2D startPoint, Vector2D endPoint, bool generating = false)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0100: Unknown result type (might be due to invalid IL or missing references)
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		//IL_010f: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0201: Unknown result type (might be due to invalid IL or missing references)
		//IL_0208: Unknown result type (might be due to invalid IL or missing references)
		//IL_020f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0214: Unknown result type (might be due to invalid IL or missing references)
		//IL_021a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0221: Unknown result type (might be due to invalid IL or missing references)
		//IL_0228: Unknown result type (might be due to invalid IL or missing references)
		//IL_022d: Unknown result type (might be due to invalid IL or missing references)
		//IL_023f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0132: Unknown result type (might be due to invalid IL or missing references)
		//IL_013c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0146: Unknown result type (might be due to invalid IL or missing references)
		//IL_0150: Unknown result type (might be due to invalid IL or missing references)
		//IL_0165: Unknown result type (might be due to invalid IL or missing references)
		//IL_016f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0179: Unknown result type (might be due to invalid IL or missing references)
		//IL_0183: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_019a: Unknown result type (might be due to invalid IL or missing references)
		RegularDungeonHallSettings regularDungeonHallSettings = (RegularDungeonHallSettings)settings;
		UnifiedRandom unifiedRandom = new UnifiedRandom(regularDungeonHallSettings.RandomSeed);
		Vector2D val = startPoint;
		bool flag = false;
		if (regularDungeonHallSettings.CrackedBrickChance > 0.0)
		{
			flag = unifiedRandom.NextDouble() <= regularDungeonHallSettings.CrackedBrickChance;
		}
		int num = 3;
		int num2 = 8;
		if (regularDungeonHallSettings.OverrideInnerBoundsSize > 0)
		{
			num = regularDungeonHallSettings.OverrideInnerBoundsSize;
		}
		if (regularDungeonHallSettings.OverrideOuterBoundsSize > 0)
		{
			num2 = regularDungeonHallSettings.OverrideOuterBoundsSize;
		}
		int num3 = num + num2;
		Vector2D v = endPoint - startPoint;
		Vector2D val2 = v.SafeNormalize(Vector2D.UnitX);
		int num4 = (int)Math.Ceiling(v.Length() / val2.Length());
		Bounds.SetBounds((int)startPoint.X, (int)startPoint.Y, (int)startPoint.X, (int)startPoint.Y);
		DungeonRoomSearchSettings dungeonRoomSearchSettings = new DungeonRoomSearchSettings
		{
			Fluff = num3
		};
		List<DungeonRoom> allRoomsInSpots = DungeonUtils.GetAllRoomsInSpots(data.dungeonRooms, startPoint, endPoint, dungeonRoomSearchSettings);
		Vector2D val3 = val2;
		while (num4 > 0 && WorldGen.InWorld((int)(val.X + val2.X), (int)(val.Y + val2.Y), 10))
		{
			if (!Processed)
			{
				data.dungeonBounds.UpdateBounds((int)val.X - num3, (int)val.Y - num3, (int)val.Y + num3, (int)val.Y + num3);
				Bounds.UpdateBounds((int)val.X - num3, (int)val.Y - num3, (int)val.Y + num3, (int)val.Y + num3);
			}
			if (generating)
			{
				GenerateDungeonSquareHall(data, allRoomsInSpots, val, settings.StyleData, num, num2, regularDungeonHallSettings.PlaceOverProtectedBricks, flag);
			}
			val += val2;
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
