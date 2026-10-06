using System;
using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using ReLogic.Utilities;
using Terraria.GameContent.Biomes.Desert;
using Terraria.WorldBuilding;

namespace Terraria.GameContent.Biomes;

public class DunesBiome : MicroBiome
{
	private class DunesDescription
	{
		public bool IsValid { get; private set; }

		public SurfaceMap Surface { get; private set; }

		public Rectangle Area
		{
			[CompilerGenerated]
			get
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				return field;
			}
			[CompilerGenerated]
			private set
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				//IL_0002: Unknown result type (might be due to invalid IL or missing references)
				field = value;
			}
		}

		public WindDirection WindDirection { get; private set; }

		private DunesDescription()
		{
		}

		public static DunesDescription CreateFromPlacement(Point origin, int width, int height)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			Rectangle val = new Rectangle(origin.X - width / 2, origin.Y - height / 2, width, height);
			return new DunesDescription
			{
				Area = val,
				IsValid = true,
				Surface = SurfaceMap.FromArea(val.Left - 20, val.Width + 40),
				WindDirection = ((WorldGen.genRand.Next(2) != 0) ? WindDirection.Right : WindDirection.Left)
			};
		}
	}

	private enum WindDirection
	{
		Left,
		Right
	}

	[JsonProperty("SingleDunesWidth")]
	private WorldGenRange _singleDunesWidth = WorldGenRange.Empty;

	[JsonProperty("HeightScale")]
	private double _heightScale = 1.0;

	public int MaximumWidth => _singleDunesWidth.ScaledMaximum * 2;

	public override bool Place(Point origin, StructureMap structures, GenerationProgress progress)
	{
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		int height = (int)((double)GenBase._random.Next(60, 100) * _heightScale);
		int height2 = (int)((double)GenBase._random.Next(60, 100) * _heightScale);
		int random = _singleDunesWidth.GetRandom(GenBase._random);
		int random2 = _singleDunesWidth.GetRandom(GenBase._random);
		DunesDescription description = DunesDescription.CreateFromPlacement(new Point(origin.X - random / 2 + 30, origin.Y), random, height);
		DunesDescription description2 = DunesDescription.CreateFromPlacement(new Point(origin.X + random2 / 2 - 30, origin.Y), random2, height2);
		PlaceSingle(description, structures);
		PlaceSingle(description2, structures);
		return true;
	}

	private void PlaceSingle(DunesDescription description, StructureMap structures)
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0102: Unknown result type (might be due to invalid IL or missing references)
		//IL_0106: Unknown result type (might be due to invalid IL or missing references)
		//IL_0156: Unknown result type (might be due to invalid IL or missing references)
		int num = GenBase._random.Next(3) + 8;
		Rectangle area;
		for (int i = 0; i < num - 1; i++)
		{
			int num2 = (int)(2.0 / (double)num * (double)description.Area.Width);
			double num3 = (double)i / (double)num * (double)description.Area.Width;
			area = description.Area;
			int num4 = (int)(num3 + (double)area.Left) + num2 * 2 / 5;
			num4 += GenBase._random.Next(-5, 6);
			double num5 = (double)i / (double)(num - 2);
			double num6 = 1.0 - Math.Abs(num5 - 0.5) * 2.0;
			PlaceHill(num4 - num2 / 2, num4 + num2 / 2, (num6 * 0.3 + 0.2) * _heightScale, description);
		}
		int num7 = GenBase._random.Next(2) + 1;
		for (int j = 0; j < num7; j++)
		{
			int num8 = description.Area.Width / 2;
			area = description.Area;
			int x = area.Center.X;
			x += GenBase._random.Next(-10, 11);
			PlaceHill(x - num8 / 2, x + num8 / 2, 0.8 * _heightScale, description);
		}
		structures.AddStructure(description.Area, 20);
	}

	private static void PlaceHill(int startX, int endX, double scale, DunesDescription description)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0100: Unknown result type (might be due to invalid IL or missing references)
		//IL_010d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		Point val = new Point(startX, (int)description.Surface[startX]);
		Point val2 = new Point(endX, (int)description.Surface[endX]);
		Point val3 = new Point((val.X + val2.X) / 2, (val.Y + val2.Y) / 2 - (int)(35.0 * scale));
		int num = (val2.X - val3.X) / 4;
		int minValue = (val2.X - val3.X) / 16;
		if (description.WindDirection == WindDirection.Left)
		{
			val3.X -= WorldGen.genRand.Next(minValue, num + 1);
		}
		else
		{
			val3.X += WorldGen.genRand.Next(minValue, num + 1);
		}
		Point val4 = new Point(0, (int)(scale * 12.0));
		Point val5 = new Point(val4.X / -2, val4.Y / -2);
		PlaceCurvedLine(val, val3, (description.WindDirection != WindDirection.Left) ? val5 : val4, description);
		PlaceCurvedLine(val3, val2, (description.WindDirection == WindDirection.Left) ? val5 : val4, description);
	}

	private static void PlaceCurvedLine(Point startPoint, Point endPoint, Point anchorOffset, DunesDescription description)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_011d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0181: Unknown result type (might be due to invalid IL or missing references)
		//IL_0130: Unknown result type (might be due to invalid IL or missing references)
		//IL_018a: Unknown result type (might be due to invalid IL or missing references)
		//IL_014a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0166: Unknown result type (might be due to invalid IL or missing references)
		//IL_019a: Unknown result type (might be due to invalid IL or missing references)
		Point p = new Point((startPoint.X + endPoint.X) / 2, (startPoint.Y + endPoint.Y) / 2);
		p.X += anchorOffset.X;
		p.Y += anchorOffset.Y;
		Vector2D val = startPoint.ToVector2D();
		Vector2D val2 = endPoint.ToVector2D();
		Vector2D val3 = p.ToVector2D();
		double num = 0.5 / (val2.X - val.X);
		Point val4 = new Point(-1, -1);
		for (double num2 = 0.0; num2 <= 1.0; num2 += num)
		{
			Vector2D val5 = Vector2D.Lerp(val, val3, num2);
			Vector2D val6 = Vector2D.Lerp(val3, val2, num2);
			Point val7 = Vector2D.Lerp(val5, val6, num2).ToPoint();
			if (val7 == val4)
			{
				continue;
			}
			val4 = val7;
			int num3 = description.Area.Width / 2;
			int x = val7.X;
			Rectangle area = description.Area;
			int num4 = num3 - Math.Abs(x - area.Center.X);
			int num5 = description.Surface[val7.X] + (int)(Math.Sqrt(num4) * 3.0);
			for (int i = val7.Y - 10; i < val7.Y; i++)
			{
				if (GenBase._tiles[val7.X, i].active() && GenBase._tiles[val7.X, i].type != 53)
				{
					GenBase._tiles[val7.X, i].ClearEverything();
				}
			}
			for (int j = val7.Y; j < num5; j++)
			{
				GenBase._tiles[val7.X, j].ResetToType(53);
			}
		}
	}
}
