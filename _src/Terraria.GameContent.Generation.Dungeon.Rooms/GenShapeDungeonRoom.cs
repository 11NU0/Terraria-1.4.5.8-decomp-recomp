using System;
using Microsoft.Xna.Framework;
using ReLogic.Utilities;
using Terraria.GameContent.Generation.Dungeon.Features;
using Terraria.Utilities;
using Terraria.WorldBuilding;

namespace Terraria.GameContent.Generation.Dungeon.Rooms;

public class GenShapeDungeonRoom : DungeonRoom
{
	private ShapeData _innerShapeData = new ShapeData();

	private ShapeData _outerShapeData = new ShapeData();

	private int _floodedTileCount;

	public GenShapeDungeonRoom(DungeonRoomSettings settings)
		: base(settings)
	{
		_ = (GenShapeDungeonRoomSettings)settings;
	}

	public override void CalculateRoom(DungeonData data)
	{
		calculated = false;
		int x = settings.RoomPosition.X;
		int y = settings.RoomPosition.Y;
		GenShapeRoom(data, x, y, generating: false);
		calculated = true;
	}

	public override bool GenerateRoom(DungeonData data)
	{
		generated = false;
		int x = settings.RoomPosition.X;
		int y = settings.RoomPosition.Y;
		GenShapeRoom(data, x, y, generating: true);
		generated = true;
		return true;
	}

	public override bool CanGenerateFeatureAt(DungeonData data, IDungeonFeature feature, int x, int y)
	{
		GenShapeType shapeType = ((GenShapeDungeonRoomSettings)settings).ShapeType;
		if ((uint)(shapeType - 2) <= 1u && feature is DungeonWindow)
		{
			return false;
		}
		return base.CanGenerateFeatureAt(data, feature, x, y);
	}

	public override void GenerateEarlyDungeonFeaturesInRoom(DungeonData data)
	{
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		GenShapeDungeonRoomSettings genShapeDungeonRoomSettings = (GenShapeDungeonRoomSettings)settings;
		if (genShapeDungeonRoomSettings.ShapeType == GenShapeType.Doughnut)
		{
			byte? paintID = ((settings.OverridePaintTile > -1) ? new byte?((byte)settings.OverridePaintTile) : settings.StyleData.TilePaintColor);
			DungeonShapes.CircleRoom circleRoom = (DungeonShapes.CircleRoom)genShapeDungeonRoomSettings.InnerShape;
			Actions.SetTilePaint setTilePaint = new Actions.SetTilePaint(paintID);
			DungeonShapes.CircleRoom shape = new DungeonShapes.CircleRoom(Math.Max(1, circleRoom.HorizontalRadius / 2), Math.Max(1, circleRoom.VerticalRadius / 2));
			WorldUtils.Gen(InnerBounds.Center, shape, Actions.Chain(new Actions.ClearTile(), new Actions.SetTile(genShapeDungeonRoomSettings.StyleData.BrickTileType, setSelfFrames: false, setNeighborFrames: false, clearTile: false), setTilePaint));
		}
		base.GenerateEarlyDungeonFeaturesInRoom(data);
	}

	public override Point GetRoomCenterForDungeonFeature(DungeonData data, DungeonFeature feature)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		GenShapeDungeonRoomSettings genShapeDungeonRoomSettings = (GenShapeDungeonRoomSettings)settings;
		Point roomCenterForDungeonFeature = base.GetRoomCenterForDungeonFeature(data, feature);
		if (feature is DungeonWindow && genShapeDungeonRoomSettings.ShapeType == GenShapeType.Mound)
		{
			roomCenterForDungeonFeature.Y += InnerBounds.Height / 5;
		}
		return roomCenterForDungeonFeature;
	}

	public override Point GetRoomCenterForHallway(Vector2D otherRoomPos)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		GenShapeDungeonRoomSettings genShapeDungeonRoomSettings = (GenShapeDungeonRoomSettings)settings;
		Vector2D val = base.GetRoomCenterForHallway(otherRoomPos).ToVector2D();
		Vector2D unitX = Vector2D.UnitX;
		DungeonRoomType roomType = genShapeDungeonRoomSettings.RoomType;
		if (roomType != DungeonRoomType.GenShapeDoughnut)
		{
			return val.ToPoint();
		}
		unitX = (otherRoomPos - val).SafeNormalize(Vector2D.UnitX);
		Point result = (val + unitX * (double)(InnerBounds.Size / 2)).ToPoint();
		result.X = (int)(val.X + unitX.X * (double)(InnerBounds.Width / 2));
		result.Y = (int)(val.Y + unitX.Y * (double)(InnerBounds.Height / 2));
		return result;
	}

	public override int GetFloodedRoomTileCount()
	{
		return _floodedTileCount;
	}

	public override void FloodRoom(byte liquidType)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		GenShapeDungeonRoomSettings genShapeDungeonRoomSettings = (GenShapeDungeonRoomSettings)settings;
		WorldUtils.Gen(InnerBounds.Center, genShapeDungeonRoomSettings.InnerShape, Actions.Chain(new Modifiers.IsBelowHeight(InnerBounds.Center.Y, inclusive: true), new Modifiers.IsNotSolid(), new Actions.SetLiquid(liquidType)));
	}

	public override ProtectionType GetProtectionTypeFromPoint(int x, int y)
	{
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		if (_innerShapeData == null || _outerShapeData == null || (calculated && !OuterBounds.Contains(x, y)))
		{
			return base.GetProtectionTypeFromPoint(x, y);
		}
		if (!_outerShapeData.Contains(x - Center.X, y - Center.Y))
		{
			return ProtectionType.None;
		}
		return ProtectionType.Walls;
	}

	public override bool IsInsideRoom(int x, int y)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		if (base.IsInsideRoom(x, y))
		{
			return _innerShapeData.Contains(x - Center.X, y - Center.Y);
		}
		return false;
	}

	public void GenShapeRoom(DungeonData data, int i, int j, bool generating)
	{
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		//IL_010b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0127: Unknown result type (might be due to invalid IL or missing references)
		//IL_0129: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01da: Unknown result type (might be due to invalid IL or missing references)
		//IL_01dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0256: Unknown result type (might be due to invalid IL or missing references)
		//IL_0262: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ad: Unknown result type (might be due to invalid IL or missing references)
		new UnifiedRandom(settings.RandomSeed);
		GenShapeDungeonRoomSettings genShapeDungeonRoomSettings = (GenShapeDungeonRoomSettings)settings;
		ushort brickTileType = settings.StyleData.BrickTileType;
		ushort brickWallType = settings.StyleData.BrickWallType;
		byte? paintID = ((settings.OverridePaintTile > -1) ? new byte?((byte)settings.OverridePaintTile) : settings.StyleData.TilePaintColor);
		byte? paintID2 = ((settings.OverridePaintWall > -1) ? new byte?((byte)settings.OverridePaintWall) : settings.StyleData.WallPaintColor);
		Vector2D val = new Vector2D((double)i, (double)j);
		if (Processed)
		{
			val = (Vector2D)(Center);
		}
		InnerBounds.SetBounds((int)val.X, (int)val.Y, (int)val.X, (int)val.Y);
		OuterBounds.SetBounds((int)val.X, (int)val.Y, (int)val.X, (int)val.Y);
		Actions.SetTilePaint setTilePaint = new Actions.SetTilePaint(paintID);
		Actions.SetWallPaint setWallPaint = new Actions.SetWallPaint(paintID2);
		WorldUtils.Gen(val.ToPoint(), genShapeDungeonRoomSettings.OuterShape, Actions.Chain(new Modifiers.Expand(1), new Actions.UpdateBounds(data.dungeonBounds).Output(_outerShapeData), new Actions.UpdateBounds(OuterBounds), new Modifiers.Conditions(new Conditions.BoolCheck(generating)), new Actions.ClearTile(), new Actions.SetTile(brickTileType, setSelfFrames: false, setNeighborFrames: false, clearTile: false), setTilePaint));
		if (generating)
		{
			WorldUtils.Gen(val.ToPoint(), genShapeDungeonRoomSettings.OuterShape, Actions.Chain(new Actions.SetWall(brickWallType, setSelfFrames: false, setNeighborFrames: false, clearTile: false), setWallPaint));
		}
		WorldUtils.Gen(val.ToPoint(), genShapeDungeonRoomSettings.InnerShape, Actions.Chain(new Actions.UpdateBounds(data.dungeonBounds).Output(_innerShapeData), new Actions.UpdateBounds(InnerBounds), new Modifiers.Conditions(new Conditions.BoolCheck(generating)), new Actions.Clear(), new Actions.SetWall(brickWallType, setSelfFrames: false, setNeighborFrames: false, clearTile: false), setWallPaint));
		InnerBounds.CalculateHitbox();
		OuterBounds.CalculateHitbox();
		_floodedTileCount = DungeonUtils.CalculateFloodedTileCountFromShapeData(InnerBounds, _innerShapeData);
	}
}
