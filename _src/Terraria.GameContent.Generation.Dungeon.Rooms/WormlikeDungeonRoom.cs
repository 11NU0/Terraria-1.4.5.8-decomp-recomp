using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Terraria.Utilities;
using Terraria.WorldBuilding;

namespace Terraria.GameContent.Generation.Dungeon.Rooms;

public class WormlikeDungeonRoom(DungeonRoomSettings settings) : DungeonRoom(settings)
{
	private ShapeData _innerShapeData = new ShapeData();

	private ShapeData _outerShapeData = new ShapeData();

	private int _floodedTileCount;

	public int InnerBoundsSizeMin;

	public int InnerBoundsSizeMax;

	public Vector2[] Positions;

	public override void CalculateRoom(DungeonData data)
	{
		calculated = false;
		int x = settings.RoomPosition.X;
		int y = settings.RoomPosition.Y;
		WormlikeRoom(data, x, y, generating: false);
		calculated = true;
	}

	public override bool GenerateRoom(DungeonData data)
	{
		generated = false;
		int x = settings.RoomPosition.X;
		int y = settings.RoomPosition.Y;
		WormlikeRoom(data, x, y, generating: true);
		generated = true;
		return true;
	}

	public override int GetFloodedRoomTileCount()
	{
		return _floodedTileCount;
	}

	public override void FloodRoom(byte liquidType)
	{
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		if (_innerShapeData == null || Positions == null)
		{
			base.FloodRoom(liquidType);
			return;
		}
		_ = (WormlikeDungeonRoomSettings)settings;
		WorldUtils.Gen(Positions[0].ToPoint(), new ModShapes.All(_innerShapeData), Actions.Chain(new Modifiers.IsBelowHeight(Center.Y, inclusive: true), new Modifiers.IsNotSolid(), new Actions.SetLiquid(liquidType)));
	}

	public override ProtectionType GetProtectionTypeFromPoint(int x, int y)
	{
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		if (_innerShapeData == null || _outerShapeData == null || Positions == null || (calculated && !OuterBounds.Contains(x, y)))
		{
			return base.GetProtectionTypeFromPoint(x, y);
		}
		Point val = Positions[0].ToPoint();
		if (!_outerShapeData.Contains(x - val.X, y - val.Y))
		{
			return ProtectionType.None;
		}
		return ProtectionType.Walls;
	}

	public override bool IsInsideRoom(int x, int y)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		if (Positions == null)
		{
			return base.IsInsideRoom(x, y);
		}
		Point val = Positions[0].ToPoint();
		if (base.IsInsideRoom(x, y))
		{
			return _innerShapeData.Contains(x - val.X, y - val.Y);
		}
		return false;
	}

	public void WormlikeRoom(DungeonData data, int i, int j, bool generating)
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0100: Unknown result type (might be due to invalid IL or missing references)
		//IL_0135: Unknown result type (might be due to invalid IL or missing references)
		//IL_0137: Unknown result type (might be due to invalid IL or missing references)
		//IL_013c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0144: Unknown result type (might be due to invalid IL or missing references)
		//IL_014e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0157: Unknown result type (might be due to invalid IL or missing references)
		//IL_0161: Unknown result type (might be due to invalid IL or missing references)
		//IL_016a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0174: Unknown result type (might be due to invalid IL or missing references)
		//IL_017d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0187: Unknown result type (might be due to invalid IL or missing references)
		//IL_019b: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01de: Unknown result type (might be due to invalid IL or missing references)
		//IL_029c: Unknown result type (might be due to invalid IL or missing references)
		//IL_029e: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02be: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_020b: Unknown result type (might be due to invalid IL or missing references)
		//IL_020d: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0255: Unknown result type (might be due to invalid IL or missing references)
		//IL_0257: Unknown result type (might be due to invalid IL or missing references)
		//IL_0259: Unknown result type (might be due to invalid IL or missing references)
		//IL_025e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0260: Unknown result type (might be due to invalid IL or missing references)
		//IL_027e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0284: Unknown result type (might be due to invalid IL or missing references)
		//IL_0286: Unknown result type (might be due to invalid IL or missing references)
		//IL_028b: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0300: Unknown result type (might be due to invalid IL or missing references)
		//IL_0308: Unknown result type (might be due to invalid IL or missing references)
		//IL_0312: Unknown result type (might be due to invalid IL or missing references)
		//IL_031b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0325: Unknown result type (might be due to invalid IL or missing references)
		//IL_032e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0338: Unknown result type (might be due to invalid IL or missing references)
		//IL_0341: Unknown result type (might be due to invalid IL or missing references)
		//IL_034b: Unknown result type (might be due to invalid IL or missing references)
		//IL_035f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0369: Unknown result type (might be due to invalid IL or missing references)
		//IL_0372: Unknown result type (might be due to invalid IL or missing references)
		//IL_037c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0385: Unknown result type (might be due to invalid IL or missing references)
		//IL_038f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0398: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_024c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0251: Unknown result type (might be due to invalid IL or missing references)
		//IL_03cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_048a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0496: Unknown result type (might be due to invalid IL or missing references)
		//IL_0419: Unknown result type (might be due to invalid IL or missing references)
		//IL_041b: Unknown result type (might be due to invalid IL or missing references)
		//IL_041d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0422: Unknown result type (might be due to invalid IL or missing references)
		//IL_0424: Unknown result type (might be due to invalid IL or missing references)
		//IL_0442: Unknown result type (might be due to invalid IL or missing references)
		//IL_0448: Unknown result type (might be due to invalid IL or missing references)
		//IL_044a: Unknown result type (might be due to invalid IL or missing references)
		//IL_044f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0410: Unknown result type (might be due to invalid IL or missing references)
		//IL_0415: Unknown result type (might be due to invalid IL or missing references)
		UnifiedRandom unifiedRandom = new UnifiedRandom(settings.RandomSeed);
		WormlikeDungeonRoomSettings wormlikeDungeonRoomSettings = (WormlikeDungeonRoomSettings)settings;
		Point val = new Point(i, j);
		if (Processed)
		{
			val = Positions[0].ToPoint();
		}
		int num = 9 + unifiedRandom.Next(3);
		int num2 = Math.Max(4, num / 5);
		if (Processed)
		{
			num = InnerBoundsSizeMax;
			num2 = InnerBoundsSizeMin;
		}
		int num3 = num;
		int num4 = 8;
		int num5 = num + num4;
		InnerBounds.SetBounds(val.X, val.Y, val.X, val.Y);
		OuterBounds.SetBounds(val.X, val.Y, val.X, val.Y);
		_outerShapeData.Clear();
		_innerShapeData.Clear();
		Vector2 val2 = val.ToVector2();
		Vector2 val3 = val2;
		List<Vector2> list = new List<Vector2>();
		val2 = val3;
		Vector2 val4 = unifiedRandom.NextVector2CircularEdge(1f, 1f);
		Vector2 spinningpoint = val4;
		int firstSideIterations = wormlikeDungeonRoomSettings.FirstSideIterations;
		int num6 = 0;
		for (int k = 0; k < firstSideIterations; k++)
		{
			float num7 = (float)k / (float)firstSideIterations;
			num3 = (int)Utils.Lerp(num, num2, num7);
			num5 = num3 + num4;
			Point val5 = val2.ToPoint();
			_outerShapeData.AddBounds(val5.X - num5 - (int)val3.X, val5.Y - num5 - (int)val3.Y, val5.X + num5 - (int)val3.X, val5.Y + num5 - (int)val3.Y);
			_innerShapeData.AddBounds(val5.X - num3 - (int)val3.X, val5.Y - num3 - (int)val3.Y, val5.X + num3 - (int)val3.X, val5.Y + num3 - (int)val3.Y);
			if (!Processed)
			{
				list.Add(val2);
			}
			GenerateDungeonSquareRoom(data, InnerBounds, OuterBounds, val2.ToVector2D(), settings.StyleData, num3, num5, generating, generating);
			if (Processed)
			{
				num6++;
				if (num6 < Positions.Length)
				{
					val2 = Positions[num6];
				}
			}
			else
			{
				val2 += val4;
				val4 = spinningpoint.RotatedBy(Utils.Lerp(0.0, 1.5707963705062866, num7));
			}
		}
		val2 = val3;
		val4 = spinningpoint.RotatedBy(3.1415927410125732, Vector2.Zero).RotatedByRandom(0.7853981852531433);
		spinningpoint = val4;
		firstSideIterations = wormlikeDungeonRoomSettings.SecondSideIterations;
		for (int l = 0; l < firstSideIterations; l++)
		{
			float num8 = (float)l / (float)firstSideIterations;
			num3 = (int)Utils.Lerp(num, num2, num8);
			num5 = num3 + num4;
			Point val6 = val2.ToPoint();
			_outerShapeData.AddBounds(val6.X - num5 - (int)val3.X, val6.Y - num5 - (int)val3.Y, val6.X + num5 - (int)val3.X, val6.Y + num5 - (int)val3.Y);
			_innerShapeData.AddBounds(val6.X - num3 - (int)val3.X, val6.Y - num3 - (int)val3.Y, val6.X + num3 - (int)val3.X, val6.Y + num3 - (int)val3.Y);
			if (!Processed)
			{
				list.Add(val2);
			}
			GenerateDungeonSquareRoom(data, InnerBounds, OuterBounds, val2.ToVector2D(), settings.StyleData, num3, num5, generating, generating);
			if (Processed)
			{
				num6++;
				if (num6 < Positions.Length)
				{
					val2 = Positions[num6];
				}
			}
			else
			{
				val2 += val4;
				val4 = spinningpoint.RotatedBy(Utils.Lerp(0.0, 1.5707963705062866, num8));
			}
		}
		if (!Processed)
		{
			Positions = Enumerable.ToArray(list);
		}
		InnerBoundsSizeMin = num2;
		InnerBoundsSizeMax = num;
		InnerBounds.CalculateHitbox();
		OuterBounds.CalculateHitbox();
		_floodedTileCount = DungeonUtils.CalculateFloodedTileCountFromShapeData(InnerBounds, _innerShapeData);
	}
}
