using System;
using ReLogic.Utilities;
using Terraria.GameContent.Generation.Dungeon.Features;
using Terraria.Utilities;
using Terraria.WorldBuilding;

namespace Terraria.GameContent.Generation.Dungeon.Rooms;

public class LegacyDungeonRoom(DungeonRoomSettings settings) : DungeonRoom(settings)
{
	private ShapeData _innerShapeData = new ShapeData();

	private ShapeData _outerShapeData = new ShapeData();

	private int _floodedTileCount;

	public Vector2D StartPosition;

	public Vector2D EndPosition;

	public int Strength;

	public override void CalculateRoom(DungeonData data)
	{
		calculated = false;
		int x = settings.RoomPosition.X;
		int y = settings.RoomPosition.Y;
		LegacyRoom(data, x, y, generating: false);
		calculated = true;
	}

	public override bool GenerateRoom(DungeonData data)
	{
		generated = false;
		int x = settings.RoomPosition.X;
		int y = settings.RoomPosition.Y;
		LegacyRoom(data, x, y, generating: true);
		generated = true;
		return true;
	}

	public override int GetFloodedRoomTileCount()
	{
		return _floodedTileCount;
	}

	public override void FloodRoom(byte liquidType)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		if (generated && _innerShapeData != null)
		{
			WorldUtils.Gen(StartPosition.ToPoint(), new ModShapes.All(_innerShapeData), Actions.Chain(new Modifiers.IsBelowHeight(InnerBounds.Center.Y, inclusive: true), new Modifiers.IsNotSolid(), new Actions.SetLiquid(liquidType)));
		}
	}

	public override ProtectionType GetProtectionTypeFromPoint(int x, int y)
	{
		if (_innerShapeData == null || _outerShapeData == null || (calculated && !OuterBounds.Contains(x, y)))
		{
			return base.GetProtectionTypeFromPoint(x, y);
		}
		if (!_outerShapeData.Contains(x - (int)StartPosition.X, y - (int)StartPosition.Y))
		{
			return ProtectionType.None;
		}
		return ProtectionType.Walls;
	}

	public override bool IsInsideRoom(int x, int y)
	{
		if (base.IsInsideRoom(x, y))
		{
			return _innerShapeData.Contains(x - (int)StartPosition.X, y - (int)StartPosition.Y);
		}
		return false;
	}

	public override bool TryGenerateChestInRoom(DungeonData data, DungeonGlobalBasicChests feature)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		Vector2D endPosition = EndPosition;
		int num = (int)((float)Strength * 0.4f);
		return DungeonUtils.GenerateDungeonRegularChest(data, feature, settings.StyleData, (int)endPosition.X - num, (int)endPosition.Y - num, (int)endPosition.X + num, (int)endPosition.Y + num);
	}

	public override bool DualDungeons_TryGenerateBiomeChestInRoom(DungeonData data, DungeonGlobalBiomeChests feature)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		Vector2D endPosition = EndPosition;
		int num = (int)((float)Strength * 0.4f);
		return DungeonUtils.GenerateDungeonBiomeChest(data, feature, settings.StyleData, (int)endPosition.X - num, (int)endPosition.Y - num, (int)endPosition.X + num, (int)endPosition.Y + num);
	}

	public void LegacyRoom(DungeonData data, int i, int j, bool generating)
	{
		//IL_0114: Unknown result type (might be due to invalid IL or missing references)
		//IL_0126: Unknown result type (might be due to invalid IL or missing references)
		//IL_019d: Unknown result type (might be due to invalid IL or missing references)
		//IL_019f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0196: Unknown result type (might be due to invalid IL or missing references)
		//IL_019b: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_0250: Unknown result type (might be due to invalid IL or missing references)
		//IL_0257: Unknown result type (might be due to invalid IL or missing references)
		//IL_025d: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0205: Unknown result type (might be due to invalid IL or missing references)
		//IL_0267: Unknown result type (might be due to invalid IL or missing references)
		//IL_026c: Unknown result type (might be due to invalid IL or missing references)
		//IL_020f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0214: Unknown result type (might be due to invalid IL or missing references)
		//IL_0215: Unknown result type (might be due to invalid IL or missing references)
		//IL_0217: Unknown result type (might be due to invalid IL or missing references)
		//IL_021a: Unknown result type (might be due to invalid IL or missing references)
		//IL_021f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0221: Unknown result type (might be due to invalid IL or missing references)
		//IL_0226: Unknown result type (might be due to invalid IL or missing references)
		//IL_0228: Unknown result type (might be due to invalid IL or missing references)
		//IL_022a: Unknown result type (might be due to invalid IL or missing references)
		//IL_022f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0234: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_02de: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0316: Unknown result type (might be due to invalid IL or missing references)
		//IL_0349: Unknown result type (might be due to invalid IL or missing references)
		//IL_037c: Unknown result type (might be due to invalid IL or missing references)
		//IL_03af: Unknown result type (might be due to invalid IL or missing references)
		//IL_0792: Unknown result type (might be due to invalid IL or missing references)
		//IL_0794: Unknown result type (might be due to invalid IL or missing references)
		//IL_079a: Unknown result type (might be due to invalid IL or missing references)
		//IL_079c: Unknown result type (might be due to invalid IL or missing references)
		//IL_07b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_07bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0430: Unknown result type (might be due to invalid IL or missing references)
		//IL_0452: Unknown result type (might be due to invalid IL or missing references)
		//IL_0474: Unknown result type (might be due to invalid IL or missing references)
		//IL_0496: Unknown result type (might be due to invalid IL or missing references)
		//IL_06f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_06f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_06fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0715: Unknown result type (might be due to invalid IL or missing references)
		//IL_0759: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_052a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0535: Unknown result type (might be due to invalid IL or missing references)
		LegacyDungeonRoomSettings legacyDungeonRoomSettings = (LegacyDungeonRoomSettings)settings;
		UnifiedRandom unifiedRandom = new UnifiedRandom(legacyDungeonRoomSettings.RandomSeed);
		ushort brickTileType = settings.StyleData.BrickTileType;
		ushort brickWallType = settings.StyleData.BrickWallType;
		byte? b = ((settings.OverridePaintTile > -1) ? new byte?((byte)settings.OverridePaintTile) : settings.StyleData.TilePaintColor);
		byte? b2 = ((settings.OverridePaintWall > -1) ? new byte?((byte)settings.OverridePaintWall) : settings.StyleData.WallPaintColor);
		double num = data.roomStrengthScalar;
		if (legacyDungeonRoomSettings.StartingRoom)
		{
			num = 1.0;
		}
		double num2 = (int)(15.0 * num) + unifiedRandom.Next(15);
		Vector2D val = default;
		val.X = (double)((float)unifiedRandom.Next(-10, 11) * 0.1f) * data.roomSlantVariantScalar;
		val.Y = (double)((float)unifiedRandom.Next(-10, 11) * 0.1f) * data.roomSlantVariantScalar;
		if (val.X == 0.0 && val.Y == 0.0)
		{
			if (unifiedRandom.Next(2) == 0)
			{
				val.X = ((unifiedRandom.Next(2) != 0) ? 1 : (-1));
			}
			else
			{
				val.Y = ((unifiedRandom.Next(2) != 0) ? 1 : (-1));
			}
		}
		Vector2D val2 = default;
		val2.X = i;
		val2.Y = (double)j - num2 / 2.0;
		if (calculated)
		{
			val2 = StartPosition;
		}
		Vector2D val3 = val2;
		double num3 = data.roomStepScalar;
		if (legacyDungeonRoomSettings.StartingRoom)
		{
			num3 = 1.0;
		}
		int num4 = (int)(10.0 * num3) + unifiedRandom.Next(10);
		double num5 = num2;
		double num6 = data.roomInteriorToExteriorRatio;
		if (legacyDungeonRoomSettings.OverrideStartPosition != default(Vector2D) && legacyDungeonRoomSettings.OverrideEndPosition != default(Vector2D))
		{
			val2 = (val3 = legacyDungeonRoomSettings.OverrideStartPosition);
			Vector2D v = legacyDungeonRoomSettings.OverrideEndPosition - val2;
			val = v.SafeNormalize(Vector2D.UnitX);
			num4 = (int)Math.Ceiling(v.Length() / val.Length());
		}
		else if (legacyDungeonRoomSettings.OverrideVelocity != default(Vector2D))
		{
			val = legacyDungeonRoomSettings.OverrideVelocity;
		}
		if (legacyDungeonRoomSettings.OverrideStrength > 0)
		{
			num2 = (num5 = legacyDungeonRoomSettings.OverrideStrength);
		}
		if (legacyDungeonRoomSettings.OverrideSteps > 0)
		{
			num4 = legacyDungeonRoomSettings.OverrideSteps;
		}
		if (legacyDungeonRoomSettings.OverrideInteriorToExteriorRatio > 0.0)
		{
			num6 = legacyDungeonRoomSettings.OverrideInteriorToExteriorRatio;
		}
		InnerBounds.SetBounds((int)val2.X, (int)val2.Y, (int)val2.X, (int)val2.Y);
		OuterBounds.SetBounds((int)val2.X, (int)val2.Y, (int)val2.X, (int)val2.Y);
		while (num4 > 0)
		{
			num4--;
			int num7 = Math.Max(0, Math.Min(Main.maxTilesX - 1, (int)(val2.X - num2 * 0.800000011920929 - 5.0)));
			int num8 = Math.Max(0, Math.Min(Main.maxTilesX - 1, (int)(val2.X + num2 * 0.800000011920929 + 5.0)));
			int num9 = Math.Max(0, Math.Min(Main.maxTilesY - 1, (int)(val2.Y - num2 * 0.800000011920929 - 5.0)));
			int num10 = Math.Max(0, Math.Min(Main.maxTilesY - 1, (int)(val2.Y + num2 * 0.800000011920929 + 5.0)));
			if (legacyDungeonRoomSettings.IsEntranceRoom && data.Type == DungeonType.DualDungeon)
			{
				num10 = Math.Max(num10, DungeonUtils.GetDualDungeonBrickSupportCutoffY(data));
			}
			data.dungeonBounds.UpdateBounds(num7, num9, num8 - 1, num10 - 1);
			OuterBounds.UpdateBounds(num7, num9, num8 - 1, num10 - 1);
			int num11 = Math.Max(0, Math.Min(Main.maxTilesX - 1, (int)(val2.X - num2 * num6)));
			int num12 = Math.Max(0, Math.Min(Main.maxTilesX - 1, (int)(val2.X + num2 * num6)));
			int num13 = Math.Max(0, Math.Min(Main.maxTilesY - 1, (int)(val2.Y - num2 * num6)));
			int num14 = Math.Max(0, Math.Min(Main.maxTilesY - 1, (int)(val2.Y + num2 * num6)));
			InnerBounds.UpdateBounds(num11, num13, num12 - 1, num14 - 1);
			for (int k = num7; k < num8; k++)
			{
				for (int l = num9; l < num10; l++)
				{
					if (!generating)
					{
						_outerShapeData.Add(k - (int)val3.X, l - (int)val3.Y);
						if (k >= num11 && k <= num12 && l >= num13 && l <= num14)
						{
							_innerShapeData.Add(k - (int)val3.X, l - (int)val3.Y);
						}
						continue;
					}
					Main.tile[k, l].liquid = 0;
					bool resetTile = k >= num7 + 1 && k < num8 - 1 && l >= num9 + 1 && l < num10 - 1;
					if (CanPlaceTileAt(data, Main.tile[k, l], brickTileType, brickWallType, settings.StyleData.Style))
					{
						DungeonUtils.ChangeTileType(Main.tile[k, l], brickTileType, resetTile, b);
					}
				}
			}
			if (generating)
			{
				for (int m = num7 + 1; m < num8 - 1; m++)
				{
					for (int n = num9 + 1; n < num10 - 1; n++)
					{
						if (CanRemoveTileAt(data, Main.tile[m, n]))
						{
							DungeonUtils.ChangeWallType(Main.tile[m, n], brickWallType, resetTile: false, b2);
						}
					}
				}
			}
			num7 = num11;
			num8 = num12;
			num9 = num13;
			num10 = num14;
			if (generating)
			{
				for (int num15 = num7; num15 < num8; num15++)
				{
					for (int num16 = num9; num16 < num10; num16++)
					{
						DungeonUtils.ChangeWallType(Main.tile[num15, num16], brickWallType, resetTile: true, b2);
					}
				}
			}
			val2 += val;
			val.X = Math.Max(-1.0, Math.Min(1.0, val.X + (double)((float)unifiedRandom.Next(-10, 11) * 0.05f) * data.roomSlantVariantScalar));
			val.Y = Math.Max(-1.0, Math.Min(1.0, val.Y + (double)((float)unifiedRandom.Next(-10, 11) * 0.05f) * data.roomSlantVariantScalar));
		}
		StartPosition = val3;
		EndPosition = val2;
		Strength = (int)num5;
		InnerBounds.CalculateHitbox();
		OuterBounds.CalculateHitbox();
		_floodedTileCount = DungeonUtils.CalculateFloodedTileCountFromShapeData(InnerBounds, _innerShapeData);
	}
}
