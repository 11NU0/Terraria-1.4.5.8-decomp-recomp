using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using ReLogic.Utilities;
using Terraria.GameContent.Biomes;
using Terraria.GameContent.Generation.Dungeon.Halls;
using Terraria.GameContent.Generation.Dungeon.Rooms;
using Terraria.Localization;
using Terraria.Utilities;
using Terraria.WorldBuilding;

namespace Terraria.GameContent.Generation.Dungeon.LayoutProviders;

public class DualDungeonLayoutProvider : DungeonLayoutProvider
{
	private class HallwayCalculator
	{
		private class RoomEntry
		{
			public DungeonRoom room;

			public double progressAlongSnake;

			public List<RoomEntry> backLinks = new List<RoomEntry>();

			public List<RoomEntry> forwardLinks = new List<RoomEntry>();

			public Point Center
			{
				get
				{
					//IL_0006: Unknown result type (might be due to invalid IL or missing references)
					return room.Center;
				}
			}
		}

		private class HallLine
		{
			public RoomEntry source;

			public RoomEntry target;

			public Vector2D sourcePoint;

			public Vector2D targetPoint;
		}

		private readonly DungeonData data;

		private readonly List<RoomEntry> rooms;

		private readonly List<DungeonHall> halls = new List<DungeonHall>();

		private readonly List<HallLine> stairwells = new List<HallLine>();

		private readonly DitherSnake controlLines;

		private readonly double maxProgressDelta;

		private readonly double avgLineLength;

		private readonly double extraNearbyRoomSearchRadius = 50.0;

		public HallwayCalculator(DungeonData data, List<DungeonRoom> rooms)
		{
			this.data = data;
			this.rooms = (from r in rooms.Select((DungeonRoom r) =>
				{
					//IL_001e: Unknown result type (might be due to invalid IL or missing references)
					//IL_0023: Unknown result type (might be due to invalid IL or missing references)
					return new RoomEntry
					{
						room = r,
						progressAlongSnake = data.genVars.dungeonDitherSnake.GetPositionAlongSnake((Vector2D)(r.Center))
					};
				})
				orderby r.progressAlongSnake
				select r).ToList();
			controlLines = data.genVars.dungeonDitherSnake;
			avgLineLength = controlLines.Average((DungeonControlLine l) => l.LineLength);
			maxProgressDelta = 300.0 / avgLineLength;
		}

		public List<DungeonHall> Generate()
		{
			foreach (RoomEntry item in rooms.Skip(2))
			{
				if (WorldGen.genRand.Next(2) == 0)
				{
					HallLine hallLine = SelectGoodRoomForHallway(item, out var bestScore, ScoreStairwell, 25);
					if (hallLine != null && bestScore > 0.0)
					{
						MakeHall(hallLine, DungeonHallType.Stairwell);
					}
				}
			}
			foreach (RoomEntry item2 in rooms.Skip(1))
			{
				if (!item2.backLinks.Any())
				{
					int num = 0;
					while (true)
					{
						DungeonHallType hallType = DualDungeonLayout_GetGeneralHallType(WorldGen.genRand);
						int hallRadius = Math.Max(8, DualDungeonLayout_GetHallRadius(hallType));
						HallLine hallLine2 = SelectGoodRoomForHallway(item2, out var _, ScoreHallway, hallRadius);
						if (hallLine2 != null)
						{
							MakeHall(hallLine2, hallType);
							break;
						}
						if (num == 1000)
						{
							break;
						}
						num++;
					}
				}
				if (WorldGen.genRand.Next(2) == 0)
				{
					DungeonHallType hallType2 = DualDungeonLayout_GetGeneralHallType(WorldGen.genRand);
					int hallRadius2 = Math.Max(8, DualDungeonLayout_GetHallRadius(hallType2));
					HallLine hallLine3 = SelectGoodRoomForHallway(item2, out var bestScore3, ScoreHallway, hallRadius2);
					if (hallLine3 != null && bestScore3 > -2.0)
					{
						MakeHall(hallLine3, hallType2);
					}
				}
			}
			return halls;
		}

		private void MakeHall(HallLine line, DungeonHallType hallType)
		{
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0085: Unknown result type (might be due to invalid IL or missing references)
			//IL_008a: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
			bool flag = false;
			if (line.target.room.settings.StyleData.LiquidType >= 0 || line.source.room.settings.StyleData.LiquidType >= 0)
			{
				flag = true;
			}
			if (flag && hallType != DungeonHallType.LegacyEntrance && hallType != DungeonHallType.Legacy && hallType != DungeonHallType.Regular)
			{
				hallType = DungeonHallType.Regular;
			}
			DungeonGenerationStyleData style = data.genVars.dungeonDitherSnake.GetLineContaining(line.sourcePoint).Style;
			DungeonHallSettings dungeonHallSettings = DungeonCrawler.MakeDungeon_GetHallSettings(hallType, data, line.sourcePoint.ToVector2(), line.targetPoint.ToVector2(), style);
			dungeonHallSettings.CarveOnly = true;
			if (flag)
			{
				dungeonHallSettings.CrackedBrickChance = 1.0;
			}
			DungeonHall dungeonHall = DungeonCrawler.MakeDungeon_GetHall(dungeonHallSettings);
			dungeonHall.CalculateHall(data, line.sourcePoint, line.targetPoint);
			halls.Add(dungeonHall);
			line.source.backLinks.Add(line.target);
			line.target.forwardLinks.Add(line.source);
			if (hallType == DungeonHallType.Stairwell)
			{
				stairwells.Add(line);
			}
		}

		private double ScoreHallway(HallLine line)
		{
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_01cd: Unknown result type (might be due to invalid IL or missing references)
			double num = 0.0;
			double fromMin = Math.Sin(Math.PI * 7.0 / 36.0);
			Vector2D v = line.targetPoint - line.sourcePoint;
			Vector2D val = v.SafeNormalize(Vector2D.UnitX);
			num -= Utils.Remap(Math.Abs(val.Y), fromMin, 0.0, 0.0, 1.0);
			num -= Utils.Remap(Math.Abs(val.Y), fromMin, 1.0, 0.0, 5.0);
			num -= Utils.Remap(v.Length(), avgLineLength * 1.5, avgLineLength * 3.0, 0.0, 3.0);
			num -= (double)Utils.Remap(line.target.forwardLinks.Count, 1f, 3f, 0f, 2f);
			num -= (double)Utils.Remap(line.source.backLinks.Count, 1f, 3f, 0f, 2f);
			num += (double)Utils.Remap(DistanceFromCommonAncestor(line.source, line.target), 3f, 6f, 0f, 1f);
			if ((IsBiomeRoom(line.target) && line.target.forwardLinks.Any()) || (IsBiomeRoom(line.source) && line.source.backLinks.Any()))
			{
				num -= 5.0;
			}
			if (IsBiomeRoom(line.target) || IsBiomeRoom(line.source))
			{
				num -= Utils.Remap(Math.Abs(val.Y), 0.6, 0.8, 0.0, 5.0);
			}
			return num;
		}

		private double ScoreStairwell(HallLine line)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			Vector2D v = line.targetPoint - line.sourcePoint;
			Vector2D val = v.SafeNormalize(Vector2D.UnitX);
			double num = 0.0;
			num -= Utils.Remap(Math.Abs(val.Y), 0.6, 1.0, 1.0, 0.0);
			num -= Utils.Remap(v.Length(), avgLineLength * 0.5, avgLineLength * 1.5, 1.0, 0.0);
			if (line.target.backLinks.Any() || line.target.forwardLinks.Any())
			{
				num -= 5.0;
			}
			if (IsBiomeRoom(line.source) || IsBiomeRoom(line.target))
			{
				num -= 5.0;
			}
			return num;
		}

		private HallLine SelectGoodRoomForHallway(RoomEntry source, out double bestScore, Func<HallLine, double> scoreFunc, int hallRadius)
		{
			//IL_0129: Unknown result type (might be due to invalid IL or missing references)
			//IL_012e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0130: Unknown result type (might be due to invalid IL or missing references)
			//IL_0135: Unknown result type (might be due to invalid IL or missing references)
			//IL_0149: Unknown result type (might be due to invalid IL or missing references)
			//IL_014e: Unknown result type (might be due to invalid IL or missing references)
			//IL_016d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0172: Unknown result type (might be due to invalid IL or missing references)
			//IL_0183: Unknown result type (might be due to invalid IL or missing references)
			//IL_0195: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_01da: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_0291: Unknown result type (might be due to invalid IL or missing references)
			//IL_0296: Unknown result type (might be due to invalid IL or missing references)
			//IL_029d: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0333: Unknown result type (might be due to invalid IL or missing references)
			//IL_033e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0349: Unknown result type (might be due to invalid IL or missing references)
			//IL_0354: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_03b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_03bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_03c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_0404: Unknown result type (might be due to invalid IL or missing references)
			//IL_040b: Unknown result type (might be due to invalid IL or missing references)
			UnifiedRandom genRand = WorldGen.genRand;
			int val = rooms.FindIndex((RoomEntry r) => r.progressAlongSnake >= source.progressAlongSnake - maxProgressDelta);
			val = Math.Min(val, Math.Max(0, rooms.IndexOf(source) - 2));
			IEnumerable<RoomEntry> enumerable = rooms.Skip(val).TakeWhile((RoomEntry r) => r != source);
			double nearbyRoomSearchRadius = enumerable.Select((RoomEntry r) =>
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				//IL_0011: Unknown result type (might be due to invalid IL or missing references)
				//IL_0016: Unknown result type (might be due to invalid IL or missing references)
				return Vector2D.Distance((Vector2D)(r.Center), (Vector2D)(source.Center));
			}).Max() + extraNearbyRoomSearchRadius;
			List<RoomEntry> list = rooms.Where((RoomEntry r) =>
			{
				//IL_000a: Unknown result type (might be due to invalid IL or missing references)
				//IL_000f: Unknown result type (might be due to invalid IL or missing references)
				//IL_001a: Unknown result type (might be due to invalid IL or missing references)
				//IL_001f: Unknown result type (might be due to invalid IL or missing references)
				return r != source && Vector2D.Distance((Vector2D)(r.Center), (Vector2D)(source.Center)) <= nearbyRoomSearchRadius;
			}).ToList();
			bool flag = source.room.settings.StyleData.LiquidType >= 0;
			HallLine result = null;
			bestScore = double.MinValue;
			foreach (RoomEntry item in enumerable)
			{
				if (!CanConnect(source, item))
				{
					continue;
				}
				bool flag2 = item.room.settings.StyleData.LiquidType >= 0;
				Vector2D connectionPoint = Vector2D.Zero;
				Vector2D connectionPoint2 = Vector2D.Zero;
				source.room.GetHallwayConnectionPoint((Vector2D)(item.room.Center), out connectionPoint);
				item.room.GetHallwayConnectionPoint((Vector2D)(source.room.Center), out connectionPoint2);
				if ((flag && (connectionPoint.Y >= (double)source.room.Center.Y || connectionPoint.Y <= connectionPoint2.Y)) || (flag2 && (connectionPoint2.Y >= (double)item.room.Center.Y || connectionPoint.Y > connectionPoint2.Y)))
				{
					continue;
				}
				double num = genRand.NextDouble();
				HallLine hallLine = new HallLine
				{
					source = source,
					target = item
				};
				num -= (double)(GetHallwayConnectionPoints(source.room, item.room, out hallLine.sourcePoint, out hallLine.targetPoint) switch
				{
					ConnectionPointQuality.Okay => 2, 
					ConnectionPointQuality.Bad => 10, 
					ConnectionPointQuality.VeryBad => 1000, 
					_ => 0, 
				});
				num += scoreFunc(hallLine);
				if (num <= bestScore)
				{
					continue;
				}
				DungeonBounds dungeonBounds = new DungeonBounds();
				foreach (RoomEntry item2 in list)
				{
					Vector2D val2 = item2.Center.ToVector2().ClosestPointOnLine(hallLine.sourcePoint.ToVector2(), hallLine.targetPoint.ToVector2()).ToVector2D();
					double num2 = val2.Distance((Vector2D)(item2.Center)) - (double)item2.room.settings.GetBoundingRadius() - (double)hallRadius;
					if (item2 != item && num2 < 0.0)
					{
						num += num2 / 4.0 - 3.0;
					}
					if (item2 != item && num2 < -10.0)
					{
						num -= 1000.0;
					}
					if (item2 != item)
					{
						dungeonBounds.SetBounds((int)val2.X - hallRadius, (int)val2.Y - hallRadius, (int)val2.X + hallRadius, (int)val2.Y + hallRadius);
						item2.room.OuterBounds.Intersects(dungeonBounds);
					}
				}
				foreach (HallLine stairwell in stairwells)
				{
					if (Utils.LineSegmentsIntersect(hallLine.sourcePoint, hallLine.targetPoint, stairwell.sourcePoint, stairwell.targetPoint))
					{
						num -= 3.0;
					}
				}
				if (!(num <= bestScore) && controlLines.IsLineInsideBorder(hallLine.sourcePoint, hallLine.targetPoint, hallRadius))
				{
					result = hallLine;
					bestScore = num;
				}
			}
			return result;
		}

		private int DistanceFromCommonAncestor(RoomEntry a, RoomEntry b)
		{
			HashSet<RoomEntry> hashSet = new HashSet<RoomEntry> { a };
			HashSet<RoomEntry> hashSet2 = new HashSet<RoomEntry> { b };
			HashSet<RoomEntry> hashSet3 = new HashSet<RoomEntry> { a };
			HashSet<RoomEntry> hashSet4 = new HashSet<RoomEntry> { b };
			for (int i = 0; i < 8; i++)
			{
				if (hashSet.Any(hashSet2.Contains))
				{
					return i;
				}
				hashSet3 = new HashSet<RoomEntry>(hashSet3.SelectMany((RoomEntry e) => e.backLinks));
				foreach (RoomEntry item in hashSet3)
				{
					hashSet.Add(item);
				}
				hashSet4 = new HashSet<RoomEntry>(hashSet4.SelectMany((RoomEntry e) => e.backLinks));
				foreach (RoomEntry item2 in hashSet4)
				{
					hashSet2.Add(item2);
				}
			}
			return 8;
		}

		private bool CanConnect(RoomEntry a, RoomEntry b)
		{
			if (!a.backLinks.Contains(b) && !b.backLinks.Contains(a))
			{
				if (a.room.settings.ProgressionStage != b.room.settings.ProgressionStage && !IsBiomeRoom(a))
				{
					return IsBiomeRoom(b);
				}
				return true;
			}
			return false;
		}

		private static bool IsBiomeRoom(RoomEntry entry)
		{
			return IsBiomeRoom(entry.room.settings.RoomType);
		}

		private static bool IsBiomeRoom(DungeonRoomType roomType)
		{
			if ((uint)(roomType - 4) <= 2u)
			{
				return true;
			}
			return false;
		}
	}

	public DualDungeonLayoutProvider(DungeonLayoutProviderSettings settings)
		: base(settings)
	{
	}

	public override void ProvideLayout(DungeonData data, GenerationProgress progress, UnifiedRandom genRand, ref int roomDelay)
	{
		DungeonRoom entranceRoom = CalculateEntranceRoom(data);
		DungeonRoom dungeonRoom = CalculateStartingRoom(data);
		CalculateEntranceHall(data, entranceRoom, dungeonRoom);
		List<DungeonRoom> first = CalculateBiomeRooms(data);
		List<DungeonRoom> list = CalculateRooms(data);
		ConvertSpecializedRooms(data, list);
		List<DungeonHall> halls = new HallwayCalculator(data, first.Concat(list).Concat(new DungeonRoom[1] { dungeonRoom }).ToList()).Generate();
		ConvertSpecializedHalls(halls);
		CalculateDungeonBounds(data, list.Concat(new DungeonRoom[1] { dungeonRoom }), halls);
		for (int i = 0; i < data.dungeonRooms.Count; i++)
		{
			double num = Math.Round((float)(i + 1) / (float)data.dungeonRooms.Count * 100f);
			DungeonUtils.UpdateDungeonProgress(progress, Utils.Remap(i, 0f, data.dungeonRooms.Count, 0.35f, 0.4f), Language.GetTextValue("WorldGeneration.DungeonRooms", num), noFormatting: true);
			data.dungeonRooms[i].GenerateRoom(data);
		}
		for (int j = 0; j < data.dungeonRooms.Count; j++)
		{
			data.dungeonRooms[j].GeneratePreHallwaysDungeonFeaturesInRoom(data);
		}
		List<DungeonHall> list2 = data.dungeonHalls.FindAll(HallwayCheck_IsCrackedBrickHall);
		List<DungeonHall> list3 = data.dungeonHalls.FindAll(HallwayCheck_IsSpiderHall);
		int num2 = 0;
		for (int k = 0; k < list2.Count; k++)
		{
			DungeonHall dungeonHall = list2[k];
			double num3 = Math.Round((float)(num2 + 1) / (float)data.dungeonHalls.Count * 100f);
			DungeonUtils.UpdateDungeonProgress(progress, Utils.Remap(num2, 0f, data.dungeonHalls.Count, 0.4f, 0.6f), Language.GetTextValue("WorldGeneration.DungeonHalls", num3), noFormatting: true);
			if (dungeonHall.calculated)
			{
				dungeonHall.GenerateHall(data);
				num2++;
			}
		}
		for (int l = 0; l < list3.Count; l++)
		{
			DungeonHall dungeonHall2 = list3[l];
			double num4 = Math.Round((float)(num2 + 1) / (float)data.dungeonHalls.Count * 100f);
			DungeonUtils.UpdateDungeonProgress(progress, Utils.Remap(num2, 0f, data.dungeonHalls.Count, 0.4f, 0.6f), Language.GetTextValue("WorldGeneration.DungeonHalls", num4), noFormatting: true);
			if (dungeonHall2.calculated)
			{
				dungeonHall2.GenerateHall(data);
				num2++;
			}
		}
		for (int m = 0; m < data.dungeonHalls.Count; m++)
		{
			DungeonHall dungeonHall3 = data.dungeonHalls[m];
			if (!HallwayCheck_IsCrackedBrickHall(dungeonHall3) && !HallwayCheck_IsSpiderHall(dungeonHall3))
			{
				double num5 = Math.Round((float)(num2 + 1) / (float)data.dungeonHalls.Count * 100f);
				DungeonUtils.UpdateDungeonProgress(progress, Utils.Remap(num2, 0f, data.dungeonHalls.Count, 0.4f, 0.6f), Language.GetTextValue("WorldGeneration.DungeonHalls", num5), noFormatting: true);
				if (dungeonHall3.calculated)
				{
					dungeonHall3.GenerateHall(data);
					num2++;
				}
			}
		}
	}

	private static bool HallwayCheck_IsSpiderHall(DungeonHall hall)
	{
		return hall.settings.StyleData.Style == 12;
	}

	private static bool HallwayCheck_IsCrackedBrickHall(DungeonHall hall)
	{
		return hall.CrackedBrick;
	}

	private DungeonRoom CalculateStartingRoom(DungeonData data)
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		UnifiedRandom genRand = WorldGen.genRand;
		DungeonControlLine dungeonControlLine = data.genVars.dungeonDitherSnake[0];
		Vector2D start = dungeonControlLine.Start;
		DungeonRoom dungeonRoom = DungeonCrawler.MakeDungeon_GetRoom(new RegularDungeonRoomSettings
		{
			RoomType = DungeonRoomType.Regular,
			RoomPosition = new Point((int)start.X, (int)start.Y),
			RandomSeed = genRand.Next(),
			ProgressionStage = dungeonControlLine.ProgressionStage,
			StyleData = dungeonControlLine.Style,
			OverrideOuterBoundsSize = 8,
			OverrideInnerBoundsSize = (int)(20.0 * data.roomStrengthScalar)
		});
		dungeonRoom.CalculateRoom(data);
		return dungeonRoom;
	}

	private DungeonHall CalculateEntranceHall(DungeonData data, DungeonRoom entranceRoom, DungeonRoom startingRoom)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		StairwellDungeonHallSettings stairwellDungeonHallSettings = (StairwellDungeonHallSettings)DungeonCrawler.MakeDungeon_GetHallSettings(DungeonHallType.Stairwell, data, Vector2.Zero, Vector2.Zero, startingRoom.settings.StyleData);
		stairwellDungeonHallSettings.IsEntranceHall = true;
		DungeonHall dungeonHall = DungeonCrawler.MakeDungeon_GetHall(stairwellDungeonHallSettings);
		GetHallwayConnectionPoints(entranceRoom, startingRoom, out var point, out var point2);
		dungeonHall.CalculateHall(data, point, point2);
		return dungeonHall;
	}

	private List<DungeonRoom> CalculateBiomeRooms(DungeonData data)
	{
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		List<DungeonRoom> list = new List<DungeonRoom>();
		foreach (DungeonControlLine item in data.genVars.dungeonDitherSnake)
		{
			if (item.Next == null || item.Next.ProgressionStage != item.ProgressionStage)
			{
				Vector2D val = ((item.Next != null) ? item.Start : item.End);
				DungeonRoomSettings dungeonRoomSettings = DungeonCrawler.MakeDungeon_GetRoomSettings(item.Style.BiomeRoomType, data, item);
				dungeonRoomSettings.RoomPosition = new Point((int)val.X, (int)val.Y);
				DungeonRoom dungeonRoom = DungeonCrawler.MakeDungeon_GetRoom(dungeonRoomSettings);
				dungeonRoom.CalculateRoom(data);
				list.Add(dungeonRoom);
			}
		}
		return list;
	}

	private List<DungeonRoom> CalculateRooms(DungeonData data)
	{
		UnifiedRandom genRand = WorldGen.genRand;
		DitherSnake dungeonDitherSnake = data.genVars.dungeonDitherSnake;
		List<DungeonRoom> list = new List<DungeonRoom>();
		foreach (DungeonControlLine item in dungeonDitherSnake.Skip(1))
		{
			DungeonRoom room;
			if (item.ProgressionStage != item.Prev.ProgressionStage)
			{
				if (TryMakeRegularRoomOnLine(data, item, 0.8, genRand.NextDouble() - 0.5, out room))
				{
					list.Add(room);
				}
				continue;
			}
			for (int i = 0; i < 20; i++)
			{
				double value = genRand.NextDouble() * 2.0 - 1.0;
				value = (double)Math.Sign(value) * Math.Pow(Math.Abs(value), 0.5);
				if (TryMakeRegularRoomOnLine(data, item, genRand.NextDouble(), value, out room))
				{
					list.Add(room);
				}
			}
		}
		return list;
	}

	private bool TryMakeRegularRoomOnLine(DungeonData data, DungeonControlLine line, double normalizedDistanceAlong, double normalizedDistanceFrom, out DungeonRoom room)
	{
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		int num = 10;
		DungeonRoomSettings dungeonRoomSettings = DungeonCrawler.MakeDungeon_GetRoomSettings(DualDungeonLayout_GetGeneralRoomType(WorldGen.genRand), data, line);
		int boundingRadius = dungeonRoomSettings.GetBoundingRadius();
		SnakeOrientation orientation = dungeonRoomSettings.Orientation;
		Point val = data.genVars.dungeonDitherSnake.GetRoomPositionInsideBorder(line, normalizedDistanceAlong, normalizedDistanceFrom, boundingRadius, out orientation).ToPoint();
		dungeonRoomSettings.Orientation = orientation;
		dungeonRoomSettings.RoomPosition = new Point(val.X, val.Y);
		room = DungeonCrawler.MakeDungeon_TryRoom(data, val.X, val.Y, dungeonRoomSettings, addToData: true, boundingRadius + num);
		if (room == null)
		{
			return false;
		}
		room.CalculateRoom(data);
		return true;
	}

	private static void CalculateDungeonBounds(DungeonData data, IEnumerable<DungeonRoom> rooms, IEnumerable<DungeonHall> halls)
	{
		//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
		DungeonBounds outerPotentialDungeonBounds = data.genVars.outerPotentialDungeonBounds;
		int count = data.genVars.dungeonGenerationStyles.Count;
		data.outerProgressionBounds = new DungeonBounds[count];
		for (int i = 0; i < count; i++)
		{
			DungeonBounds dungeonBounds = (data.outerProgressionBounds[i] = new DungeonBounds());
			foreach (DungeonRoom room in rooms)
			{
				dungeonBounds.UpdateBounds(room.OuterBounds);
			}
			foreach (DungeonHall hall in halls)
			{
				dungeonBounds.UpdateBounds(hall.Bounds);
			}
			if (dungeonBounds.Top < outerPotentialDungeonBounds.Top)
			{
				dungeonBounds.Top = outerPotentialDungeonBounds.Top;
			}
			if (dungeonBounds.Bottom > outerPotentialDungeonBounds.Bottom)
			{
				dungeonBounds.Bottom = outerPotentialDungeonBounds.Bottom;
			}
			dungeonBounds.CalculateHitbox();
		}
	}

	private static DungeonRoom CalculateEntranceRoom(DungeonData data)
	{
		//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
		DungeonBounds outerPotentialDungeonBounds = data.genVars.outerPotentialDungeonBounds;
		if (data.genVars.generatingDungeonPositionY > outerPotentialDungeonBounds.Top - 10)
		{
			data.genVars.generatingDungeonPositionY = outerPotentialDungeonBounds.Top - 10;
		}
		if (data.genVars.preGenDungeonEntranceSettings.PrecalculateEntrancePosition)
		{
			data.genVars.generatingDungeonPositionX = -10 + (int)data.genVars.dungeonEntrancePosition.X + WorldGen.genRand.Next(20);
			data.genVars.generatingDungeonPositionY = (int)data.genVars.dungeonEntrancePosition.Y + 30;
		}
		if (SpecialSeedFeatures.DungeonEntranceIsBuried)
		{
			data.genVars.generatingDungeonPositionY = (int)Main.worldSurface + 15;
		}
		if (SpecialSeedFeatures.DungeonEntranceIsUnderground)
		{
			data.genVars.generatingDungeonPositionY = (int)GenVars.worldSurfaceHigh + 15;
		}
		DungeonRoom dungeonRoom = DungeonCrawler.MakeDungeon_GetRoom(new LegacyDungeonRoomSettings
		{
			StyleData = data.genVars.dungeonStyle,
			RoomPosition = new Point(data.genVars.generatingDungeonPositionX, data.genVars.generatingDungeonPositionY),
			IsEntranceRoom = true
		});
		dungeonRoom.CalculateRoom(data);
		return dungeonRoom;
	}

	public static DungeonRoomType DualDungeonLayout_GetGeneralRoomType(UnifiedRandom genRand)
	{
		return genRand.Next(8) switch
		{
			1 => DungeonRoomType.Regular, 
			2 => DungeonRoomType.Wormlike, 
			3 => DungeonRoomType.GenShapeCircle, 
			4 => DungeonRoomType.GenShapeMound, 
			5 => DungeonRoomType.GenShapeHourglass, 
			6 => DungeonRoomType.GenShapeDoughnut, 
			7 => DungeonRoomType.GenShapeQuadCircle, 
			_ => DungeonRoomType.Legacy, 
		};
	}

	public static DungeonHallType DualDungeonLayout_GetGeneralHallType(UnifiedRandom genRand)
	{
		return genRand.Next(3) switch
		{
			1 => DungeonHallType.Regular, 
			2 => DungeonHallType.Sine, 
			_ => DungeonHallType.Legacy, 
		};
	}

	public static int DualDungeonLayout_GetHallRadius(DungeonHallType hallType)
	{
		if ((uint)hallType <= 2u || hallType != DungeonHallType.Sine)
		{
			return 8;
		}
		return 14;
	}

	public void ConvertSpecializedRooms(DungeonData data, List<DungeonRoom> rooms)
	{
		//IL_03c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_06f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_06fe: Unknown result type (might be due to invalid IL or missing references)
		UnifiedRandom genRand = WorldGen.genRand;
		DitherSnake dungeonDitherSnake = data.genVars.dungeonDitherSnake;
		int num = 2;
		int num2 = 2;
		int num3 = 2;
		int num4 = 2;
		int num5 = 5;
		int num6 = 6;
		switch (WorldGen.GetWorldSize())
		{
		case 0:
			num = 2;
			num2 = 2;
			num3 = 2;
			num4 = 2;
			num5 = 5;
			num6 = 6;
			break;
		case 1:
			num = 4;
			num2 = 6;
			num3 = 6;
			num4 = 6;
			num5 = 8;
			num6 = 10;
			break;
		case 2:
			num = 6;
			num2 = 10;
			num3 = 10;
			num4 = 8;
			num5 = 11;
			num6 = 14;
			break;
		}
		List<DungeonRoom> list = new List<DungeonRoom>();
		List<DungeonRoom> list2 = new List<DungeonRoom>();
		List<DungeonRoom> list3 = new List<DungeonRoom>();
		foreach (DungeonRoom room in rooms)
		{
			switch (room.settings.StyleData.Style)
			{
			case 1:
				list.Add(room);
				break;
			case 8:
				list2.Add(room);
				break;
			case 6:
				list3.Add(room);
				break;
			}
		}
		int num7 = 2000;
		List<DungeonRoom> list4 = list.ToList();
		_ = list4.Count;
		num7 = 2000;
		while (num7 > 0 && list4.Count > 0 && num > 0)
		{
			num7--;
			if (num7 <= 0)
			{
				break;
			}
			DungeonRoom dungeonRoom = list4[genRand.Next(list4.Count)];
			if (dungeonRoom.settings.OnCurvedLine || dungeonRoom.settings.Orientation != SnakeOrientation.Bottom)
			{
				list4.Remove(dungeonRoom);
				continue;
			}
			if (dungeonRoom.settings is GenShapeDungeonRoomSettings && ((GenShapeDungeonRoomSettings)dungeonRoom.settings).ShapeType == GenShapeType.Doughnut)
			{
				list4.Remove(dungeonRoom);
				continue;
			}
			if (dungeonRoom.GetFloodedRoomTileCount() < SceneMetrics.ShimmerTileThreshold)
			{
				list4.Remove(dungeonRoom);
				continue;
			}
			dungeonRoom.settings.StyleData = DungeonGenerationStyles.Shimmer;
			dungeonRoom.settings.HallwayConnectionPointOverride = ConnectToTopOfRoomOnly;
			num--;
			list.Remove(dungeonRoom);
			list4.Remove(dungeonRoom);
		}
		if (num > 0)
		{
			list4 = list.ToList();
			_ = list4.Count;
			num7 = 2000;
			while (num7 > 0 && list4.Count > 0 && num > 0)
			{
				num7--;
				if (num7 <= 0)
				{
					break;
				}
				DungeonRoom dungeonRoom2 = list4[genRand.Next(list4.Count)];
				if (dungeonRoom2.settings.OnCurvedLine || dungeonRoom2.settings.Orientation != SnakeOrientation.Bottom)
				{
					list4.Remove(dungeonRoom2);
					continue;
				}
				if (dungeonRoom2.settings is GenShapeDungeonRoomSettings && ((GenShapeDungeonRoomSettings)dungeonRoom2.settings).ShapeType == GenShapeType.Doughnut)
				{
					list4.Remove(dungeonRoom2);
					continue;
				}
				dungeonRoom2.settings.StyleData = DungeonGenerationStyles.Shimmer;
				dungeonRoom2.settings.HallwayConnectionPointOverride = ConnectToTopOfRoomOnly;
				num--;
				list.Remove(dungeonRoom2);
				list4.Remove(dungeonRoom2);
			}
		}
		list4 = list.ToList();
		num7 = 2000;
		while (num7 > 0 && list4.Count > 0 && num2 > 0)
		{
			num7--;
			if (num7 <= 0)
			{
				break;
			}
			DungeonRoom dungeonRoom3 = list4[genRand.Next(list4.Count)];
			if (dungeonRoom3.settings.OnCurvedLine || dungeonRoom3.settings.Orientation == SnakeOrientation.Bottom)
			{
				list4.Remove(dungeonRoom3);
				continue;
			}
			DungeonControlLine controlLine = dungeonRoom3.settings.ControlLine;
			DungeonRoomSettings dungeonRoomSettings = DungeonCrawler.MakeDungeon_GetRoomSettings(DungeonRoomType.LivingTree, data, controlLine);
			dungeonRoomSettings.StyleData = DungeonGenerationStyles.LivingWood;
			dungeonRoomSettings.HallwayConnectionPointOverride = ConnectToBottomOfRoomOnly;
			dungeonRoomSettings.RoomPosition = new Point(dungeonRoom3.settings.RoomPosition.X, dungeonRoom3.settings.RoomPosition.Y);
			if (!dungeonDitherSnake.IsCircleInsideBorderWithMatchingStyle(controlLine, (Vector2D)(dungeonRoomSettings.RoomPosition), dungeonRoomSettings.GetBoundingRadius()))
			{
				continue;
			}
			DungeonRoom dungeonRoom4 = DungeonCrawler.MakeDungeon_GetRoom(dungeonRoomSettings, addToData: false);
			dungeonRoom4.CalculateRoom(data);
			DungeonRoom roomFound = null;
			if (!data.IsAnyRoomInBounds(out roomFound, dungeonRoom4.OuterBounds, new DungeonRoomSearchSettings
			{
				UseOuterBounds = true,
				ExcludedRoom = dungeonRoom3
			}))
			{
				int num8 = data.dungeonRooms.IndexOf(dungeonRoom3);
				int num9 = rooms.IndexOf(dungeonRoom3);
				list.Remove(dungeonRoom3);
				list4.Remove(dungeonRoom3);
				if (num8 > -1)
				{
					data.dungeonRooms[num8] = dungeonRoom4;
				}
				else
				{
					data.dungeonRooms.Add(dungeonRoom4);
				}
				if (num9 > -1)
				{
					rooms[num9] = dungeonRoom4;
				}
				else
				{
					rooms.Add(dungeonRoom4);
				}
				num2--;
			}
		}
		list4 = list.ToList();
		num7 = 2000;
		while (num7 > 0 && list4.Count > 0 && num4 > 0)
		{
			num7--;
			if (num7 <= 0)
			{
				break;
			}
			DungeonRoom dungeonRoom5 = list4[genRand.Next(list4.Count)];
			if (dungeonRoom5.settings.OnCurvedLine)
			{
				list4.Remove(dungeonRoom5);
				continue;
			}
			dungeonRoom5.settings.StyleData = DungeonGenerationStyles.Spider;
			num4--;
			list.Remove(dungeonRoom5);
			list4.Remove(dungeonRoom5);
		}
		list4 = list2.ToList();
		num7 = 2000;
		while (num7 > 0 && list4.Count > 0 && num5 > 0)
		{
			num7--;
			if (num7 <= 0)
			{
				break;
			}
			DungeonRoom dungeonRoom6 = list4[genRand.Next(list4.Count)];
			if (dungeonRoom6.settings.OnCurvedLine || dungeonRoom6.settings.Orientation != SnakeOrientation.Bottom)
			{
				list4.Remove(dungeonRoom6);
				continue;
			}
			if (dungeonRoom6.settings is GenShapeDungeonRoomSettings && ((GenShapeDungeonRoomSettings)dungeonRoom6.settings).ShapeType == GenShapeType.Doughnut)
			{
				list4.Remove(dungeonRoom6);
				continue;
			}
			dungeonRoom6.settings.StyleData = DungeonGenerationStyles.Beehive;
			dungeonRoom6.settings.HallwayConnectionPointOverride = ConnectToTopOfRoomOnly;
			num5--;
			list2.Remove(dungeonRoom6);
			list4.Remove(dungeonRoom6);
		}
		list4 = list2.ToList();
		num7 = 2000;
		while (num7 > 0 && list4.Count > 0 && num3 > 0)
		{
			num7--;
			if (num7 <= 0)
			{
				break;
			}
			DungeonRoom dungeonRoom7 = list4[genRand.Next(list4.Count)];
			if (dungeonRoom7.settings.OnCurvedLine || dungeonRoom7.settings.Orientation == SnakeOrientation.Bottom)
			{
				list4.Remove(dungeonRoom7);
				continue;
			}
			DungeonControlLine controlLine2 = dungeonRoom7.settings.ControlLine;
			DungeonRoomSettings dungeonRoomSettings2 = DungeonCrawler.MakeDungeon_GetRoomSettings(DungeonRoomType.LivingTree, data, controlLine2);
			dungeonRoomSettings2.StyleData = DungeonGenerationStyles.LivingMahogany;
			dungeonRoomSettings2.HallwayConnectionPointOverride = ConnectToBottomOfRoomOnly;
			dungeonRoomSettings2.RoomPosition = new Point(dungeonRoom7.settings.RoomPosition.X, dungeonRoom7.settings.RoomPosition.Y);
			if (!dungeonDitherSnake.IsCircleInsideBorderWithMatchingStyle(controlLine2, (Vector2D)(dungeonRoomSettings2.RoomPosition), dungeonRoomSettings2.GetBoundingRadius()))
			{
				continue;
			}
			DungeonRoom dungeonRoom8 = DungeonCrawler.MakeDungeon_GetRoom(dungeonRoomSettings2, addToData: false);
			dungeonRoom8.CalculateRoom(data);
			DungeonRoom roomFound2 = null;
			if (!data.IsAnyRoomInBounds(out roomFound2, dungeonRoom8.OuterBounds, new DungeonRoomSearchSettings
			{
				UseOuterBounds = true,
				ExcludedRoom = dungeonRoom7
			}))
			{
				int num10 = data.dungeonRooms.IndexOf(dungeonRoom7);
				int num11 = rooms.IndexOf(dungeonRoom7);
				list.Remove(dungeonRoom7);
				list4.Remove(dungeonRoom7);
				if (num10 > -1)
				{
					data.dungeonRooms[num10] = dungeonRoom8;
				}
				else
				{
					data.dungeonRooms.Add(dungeonRoom8);
				}
				if (num11 > -1)
				{
					rooms[num11] = dungeonRoom8;
				}
				else
				{
					rooms.Add(dungeonRoom8);
				}
				num3--;
			}
		}
		list4 = list3.ToList();
		num7 = 2000;
		while (num7 > 0 && list4.Count > 0 && num6 > 0)
		{
			num7--;
			if (num7 > 0)
			{
				DungeonRoom dungeonRoom9 = list4[genRand.Next(list4.Count)];
				if (dungeonRoom9.settings.OnCurvedLine)
				{
					list4.Remove(dungeonRoom9);
					continue;
				}
				dungeonRoom9.settings.StyleData = DungeonGenerationStyles.Crystal;
				num6--;
				list3.Remove(dungeonRoom9);
				list4.Remove(dungeonRoom9);
				continue;
			}
			break;
		}
	}

	public void ConvertSpecializedHalls(List<DungeonHall> halls)
	{
		UnifiedRandom genRand = WorldGen.genRand;
		int num = 3;
		switch (WorldGen.GetWorldSize())
		{
		case 0:
			num = 3;
			break;
		case 1:
			num = 4;
			break;
		case 2:
			num = 5;
			break;
		}
		List<DungeonHall> list = new List<DungeonHall>();
		foreach (DungeonHall hall in halls)
		{
			byte style = hall.settings.StyleData.Style;
			if (style == 1)
			{
				list.Add(hall);
			}
		}
		int num2 = 2000;
		List<DungeonHall> list2 = list.ToList();
		num2 = 2000;
		while (num2 > 0 && list2.Count > 0 && num > 0)
		{
			num2--;
			if (num2 > 0)
			{
				DungeonHall dungeonHall = list2[genRand.Next(list2.Count)];
				if (dungeonHall.CrackedBrick)
				{
					list2.Remove(dungeonHall);
					continue;
				}
				dungeonHall.settings.StyleData = DungeonGenerationStyles.Spider;
				dungeonHall.settings.CarveOnly = false;
				num--;
				list.Remove(dungeonHall);
				list2.Remove(dungeonHall);
				continue;
			}
			break;
		}
	}

	public static ConnectionPointQuality ConnectToTopOfRoomOnly(DungeonRoom room, Vector2D otherRoomPos, out Vector2D connectionPoint)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		connectionPoint = (Vector2D)(room.GetRoomCenterForHallway(otherRoomPos));
		while (room.IsInsideRoom(connectionPoint.ToPoint()))
		{
			connectionPoint.Y--;
		}
		connectionPoint.Y += 3.0;
		Vector2D val = (otherRoomPos - connectionPoint).SafeNormalize(default);
		if (!(val.Y < -0.5))
		{
			if (!(val.Y < -0.2))
			{
				if (!(val.Y < 0.0))
				{
					return ConnectionPointQuality.VeryBad;
				}
				return ConnectionPointQuality.Bad;
			}
			return ConnectionPointQuality.Okay;
		}
		return ConnectionPointQuality.Good;
	}

	public static ConnectionPointQuality ConnectToBottomOfRoomOnly(DungeonRoom room, Vector2D otherRoomPos, out Vector2D connectionPoint)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		connectionPoint = (Vector2D)(room.GetRoomCenterForHallway(otherRoomPos));
		while (room.IsInsideRoom(connectionPoint.ToPoint()))
		{
			connectionPoint.Y++;
		}
		connectionPoint.Y -= 3.0;
		Vector2D val = (otherRoomPos - connectionPoint).SafeNormalize(default);
		if (!(val.Y > 0.5))
		{
			if (!(val.Y > 0.2))
			{
				if (!(val.Y > 0.0))
				{
					return ConnectionPointQuality.VeryBad;
				}
				return ConnectionPointQuality.Bad;
			}
			return ConnectionPointQuality.Okay;
		}
		return ConnectionPointQuality.Good;
	}

	public static ConnectionPointQuality GetHallwayConnectionPoints(DungeonRoom room1, DungeonRoom room2, out Vector2D point1, out Vector2D point2)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		ConnectionPointQuality hallwayConnectionPoint = room1.GetHallwayConnectionPoint((Vector2D)(room2.Center), out point1);
		ConnectionPointQuality hallwayConnectionPoint2 = room2.GetHallwayConnectionPoint((Vector2D)(room1.Center), out point2);
		return (ConnectionPointQuality)Math.Max((int)hallwayConnectionPoint, (int)hallwayConnectionPoint2);
	}
}
