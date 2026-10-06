using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Text;
using Microsoft.Xna.Framework;
using ReLogic.OS;
using Terraria.GameContent;
using Terraria.GameContent.Drawing;
using Terraria.GameInput;
using Terraria.Testing;
using Terraria.UI.Chat;

namespace Terraria;

public static class TimeLogger
{
	public class DataSeries
	{
		private int[] values = new int[FrameCount];

		private bool[] used = new bool[FrameCount];

		private int next;

		private int count;

		private int usedCount;

		public int previous;

		public int median;

		public int p90;

		public int max;

		private static int[] _sort = new int[FrameCount];

		public bool HasData => usedCount > 0;

		public float frequency
		{
			get
			{
				if (count <= 0)
				{
					return 0f;
				}
				return (float)usedCount / (float)count;
			}
		}

		public void Add(int value, out bool newMax)
		{
			values[next] += value;
			used[next] = true;
			newMax = values[next] > max;
		}

		public void StartNextFrame()
		{
			if (used[next])
			{
				previous = values[next];
			}
			if (count < values.Length)
			{
				count++;
			}
			next = (next + 1) % values.Length;
			usedCount = 0;
			for (int i = 0; i < count; i++)
			{
				if (used[i])
				{
					_sort[usedCount++] = values[i];
				}
			}
			if (usedCount > 0)
			{
				Array.Sort(_sort, 0, usedCount);
				median = Quantile(_sort, usedCount, 0.5f);
				p90 = Quantile(_sort, usedCount, 0.9f);
				max = Quantile(_sort, usedCount, 1f);
			}
			else
			{
				previous = (median = (p90 = (max = 0)));
			}
			values[next] = 0;
			used[next] = false;
		}

		public void Reset()
		{
			next = (count = (usedCount = 0));
			previous = (median = (p90 = (max = 0)));
		}

		private static int Quantile(int[] sorted, int len, float quantile)
		{
			quantile *= (float)(len - 1);
			return (int)MathHelper.Lerp((float)sorted[(int)Math.Floor(quantile)], (float)sorted[(int)Math.Ceiling(quantile)], quantile % 1f);
		}
	}

	public class TimeLogData
	{
		public string name;

		public Func<int, string> format;

		public int budget;

		public bool pendingDisplay;

		public DataSeries[] data = new DataSeries[2]
		{
			new DataSeries(),
			new DataSeries()
		};

		public bool ShouldDrawByDefault
		{
			get
			{
				DataSeries[] array = data;
				foreach (DataSeries dataSeries in array)
				{
					if (dataSeries.HasData && (float)dataSeries.p90 >= (float)budget * 0.05f)
					{
						return true;
					}
				}
				return false;
			}
		}

		public void AddTime(StartTimestamp fromTimestamp)
		{
			Add(fromTimestamp.ElapsedTicks);
		}

		public void Add(int value)
		{
			data[activeDataSeries].Add(value, out var newMax);
			LogAdd(name, value, newMax);
		}

		public void Reset()
		{
			DataSeries[] array = data;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].Reset();
			}
		}

		public void StartNextFrame()
		{
			data[activeDataSeries].StartNextFrame();
		}
	}

	public struct StartTimestamp
	{
		public long ticks;

		public int ElapsedTicks => (int)(Stopwatch.GetTimestamp() - ticks);

		public TimeSpan Elapsed => Utils.SWTicksToTimeSpan(ElapsedTicks);
	}

	private class FormatPool
	{
		private readonly string _format;

		private readonly double _minValue;

		private readonly double _rounding;

		private readonly string[] _strings;

		private string _nullString;

		public FormatPool(string format, double maxValue, double minValue = 0.0, double rounding = 1.0)
		{
			_format = format;
			_minValue = minValue;
			_rounding = rounding;
			_strings = new string[(int)((maxValue - minValue) / rounding) + 1];
			_nullString = string.Format(_format, (object)null);
		}

		public string Format(double value)
		{
			int num = (int)Math.Round((value - _minValue) / _rounding);
			if (num < 0 || num >= _strings.Length)
			{
				return string.Format(_format, value);
			}
			string text = _strings[num];
			if (text == null)
			{
				text = (_strings[num] = string.Format(_format, value));
			}
			return text;
		}

		public string Format(double? value)
		{
			if (value.HasValue)
			{
				return Format(value.Value);
			}
			return _nullString;
		}
	}

	public static readonly int FrameCount;

	private static StreamWriter logWriter;

	private static StringBuilder logBuilder;

	private static int framesToLog;

	private static int currentFrame;

	private static bool startLoggingNextFrame;

	private static bool endLoggingThisFrame;

	private static bool currentlyLogging;

	private static string[] DataSeriesHeaders;

	private static int activeDataSeries;

	private static List<TimeLogData> entries;

	public static TimeLogData TotalDrawAndUpdate;

	public static TimeLogData DrawSolidTiles;

	public static TimeLogData FlushSolidTiles;

	public static TimeLogData SolidDrawCalls;

	public static TimeLogData DrawNonSolidTiles;

	public static TimeLogData FlushNonSolidTiles;

	public static TimeLogData NonSolidDrawCalls;

	public static TimeLogData DrawBlackTiles;

	public static TimeLogData DrawWallTiles;

	public static TimeLogData FlushWallTiles;

	public static TimeLogData WallDrawCalls;

	public static TimeLogData DrawWaterTiles;

	public static TimeLogData LiquidDrawCalls;

	public static TimeLogData DrawBackgroundWaterTiles;

	public static TimeLogData LiquidBackgroundDrawCalls;

	public static TimeLogData DrawUndergroundBackground;

	public static TimeLogData DrawOldUndergroundBackground;

	public static TimeLogData DrawWireTiles;

	public static TimeLogData ClothingRacks;

	public static TimeLogData TileExtras;

	public static TimeLogData Nature;

	public static TimeLogData RenderSolidTiles;

	public static TimeLogData RenderNonSolidTiles;

	public static TimeLogData RenderBlacksAndWalls;

	public static TimeLogData RenderUndergroundBackground;

	public static TimeLogData RenderBackgroundLiquid;

	public static TimeLogData RenderLiquid;

	public static TimeLogData[] TotalDrawByRenderCount;

	public static TimeLogData TotalDrawRenderNow;

	public static TimeLogData TotalDraw;

	public static TimeLogData Lighting;

	public static TimeLogData LightingInit;

	public static TimeLogData[] LightingByPass;

	public static TimeLogData FindPaintedTiles;

	public static TimeLogData PrepareRequests;

	public static TimeLogData FindingWaterfalls;

	public static TimeLogData MapChanges;

	public static TimeLogData MapSectionUpdate;

	public static TimeLogData MapUpdate;

	public static TimeLogData SectionFraming;

	public static TimeLogData SectionRefresh;

	public static TimeLogData SkyBackground;

	public static TimeLogData SunMoonStars;

	public static TimeLogData SurfaceBackground;

	public static TimeLogData Map;

	public static TimeLogData PlayerChat;

	public static TimeLogData Waterfalls;

	public static TimeLogData NPCs;

	public static TimeLogData Projectiles;

	public static TimeLogData Players;

	public static TimeLogData Items;

	public static TimeLogData Rain;

	public static TimeLogData Gore;

	public static TimeLogData Dust;

	public static TimeLogData Particles;

	public static TimeLogData LeashedEntities;

	public static TimeLogData Interface;

	public static TimeLogData DrawFPSGraph;

	public static TimeLogData DrawTimeLogger;

	public static TimeLogData Overlays;

	public static TimeLogData Filters;

	public static TimeLogData SunVisibility;

	public static TimeLogData MenuDrawTime;

	public static TimeLogData SplashDrawTime;

	public static TimeLogData DrawFullscreenMap;

	public static TimeLogData GCPause;

	public static TimeLogData TotalUpdate;

	public static TimeLogData UpdatesInWorld;

	public static TimeLogData UpdatePlayers;

	public static TimeLogData SpawnNPCs;

	public static TimeLogData UpdateNPCs;

	public static TimeLogData UpdateProjectiles;

	public static TimeLogData UpdateItems;

	public static TimeLogData UpdateDust;

	public static TimeLogData UpdateGore;

	public static TimeLogData UpdateWorld;

	public static TimeLogData UpdateTime;

	public static TimeLogData UpdateLeashedEntities;

	public static TimeLogData UpdateFloatingText;

	private static Queue<Action> _onNextFrame;

	public static int ABTestMode;

	public static readonly string ABTestName;

	private static int TableWidth;

	private static int ColumnSpacing;

	private static int DrawnEntryNumber;

	private static readonly uint DrawLayer_Background;

	private static readonly uint DrawLayer_Text;

	private static Queue<TimeLogData> _entriesToDraw;

	private static FormatPool _PinnedCPUFormat;

	private static FormatPool _AssignedCPUFormat;

	private static FormatPool _procThrottleFormat;

	private static FormatPool _expectedCPUFormat;

	private static FormatPool _terrariaCPUFormat;

	private static FormatPool _pendingCPUFormat;

	private static FormatPool _percentFormat;

	private static FormatPool _msFormat;

	private static FormatPool _intFormat;

	public static bool ABTestFlag
	{
		get
		{
			return TileDrawingBase.DrawOwnBlacks;
		}
		set
		{
			TileDrawingBase.DrawOwnBlacks = value;
		}
	}

	private static TimeLogData NewEntry(string name, TimeSpan? budget = null)
	{
		TimeLogData timeLogData = new TimeLogData
		{
			name = name,
			format = FormatTicks,
			budget = (int)Utils.TimeSpanToSWTicks(budget ?? TimeSpan.FromMilliseconds(2.0))
		};
		entries.Add(timeLogData);
		return timeLogData;
	}

	private static TimeLogData NewCounterEntry(string name, int budget)
	{
		TimeLogData timeLogData = new TimeLogData
		{
			name = name,
			format = (int i) => _intFormat.Format(i),
			budget = budget
		};
		entries.Add(timeLogData);
		return timeLogData;
	}

	static TimeLogger()
	{
		FrameCount = DetailedFPS.FrameCount;
		framesToLog = -1;
		DataSeriesHeaders = new string[2];
		activeDataSeries = 0;
		entries = new List<TimeLogData>();
		TotalDrawAndUpdate = NewEntry("Total Draw+Update", TimeSpan.FromSeconds(Main.TARGET_FRAME_TIME));
		DrawSolidTiles = NewEntry("Draw Solid Tiles");
		FlushSolidTiles = NewEntry("Flush Solid Tiles");
		SolidDrawCalls = NewCounterEntry("Solid Draw Calls", 200);
		DrawNonSolidTiles = NewEntry("Draw Non-Solid Tiles");
		FlushNonSolidTiles = NewEntry("Flush Non-Solid Tiles");
		NonSolidDrawCalls = NewCounterEntry("Non-Solid Draw Calls", 200);
		DrawBlackTiles = NewEntry("Draw Black Tiles");
		DrawWallTiles = NewEntry("Draw Wall Tiles");
		FlushWallTiles = NewEntry("Flush Wall Tiles");
		WallDrawCalls = NewCounterEntry("Wall Draw Calls", 200);
		DrawWaterTiles = NewEntry("Draw Liquid Tiles");
		LiquidDrawCalls = NewCounterEntry("Liquid Draw Calls", 200);
		DrawBackgroundWaterTiles = NewEntry("Draw Bg Liquid Tiles");
		LiquidBackgroundDrawCalls = NewCounterEntry("Bg Liquid Draw Calls", 200);
		DrawUndergroundBackground = NewEntry("Draw Underground Bg");
		DrawOldUndergroundBackground = NewEntry("Draw Old Underground Bg");
		DrawWireTiles = NewEntry("Draw Wire Tiles");
		ClothingRacks = NewEntry("Hat Racks & Display Dolls");
		TileExtras = NewEntry("Tile Extras");
		Nature = NewEntry("Nature Renderer");
		RenderSolidTiles = NewEntry("Render Solid Tiles", TimeSpan.FromMilliseconds(3.0));
		RenderNonSolidTiles = NewEntry("Render Non-Solid Tiles", TimeSpan.FromMilliseconds(3.0));
		RenderBlacksAndWalls = NewEntry("Render Black Tiles & Walls", TimeSpan.FromMilliseconds(3.0));
		RenderUndergroundBackground = NewEntry("Render Underground Bg", TimeSpan.FromMilliseconds(3.0));
		RenderBackgroundLiquid = NewEntry("Render Bg Water Tiles", TimeSpan.FromMilliseconds(3.0));
		RenderLiquid = NewEntry("Render Water Tiles", TimeSpan.FromMilliseconds(3.0));
		TotalDrawRenderNow = NewEntry("Total Draw, Render Now", TimeSpan.FromMilliseconds(12.0));
		TotalDraw = NewEntry("Total Draw", TimeSpan.FromMilliseconds(12.0));
		Lighting = NewEntry("Lighting");
		LightingInit = NewEntry("Lighting Init");
		FindPaintedTiles = NewEntry("Find Painted Tiles");
		PrepareRequests = NewEntry("Prep Render Target Content");
		FindingWaterfalls = NewEntry("Find Waterfalls");
		MapChanges = NewEntry("Queued Map Updates");
		MapSectionUpdate = NewEntry("Map Section Update");
		MapUpdate = NewEntry("Map Update");
		SectionFraming = NewEntry("Section Framing");
		SectionRefresh = NewEntry("Section Refresh");
		SkyBackground = NewEntry("Sky Background");
		SunMoonStars = NewEntry("Sun, Moon & Stars");
		SurfaceBackground = NewEntry("Surface Background");
		Map = NewEntry("Map");
		PlayerChat = NewEntry("Player Chat");
		Waterfalls = NewEntry("Waterfalls");
		NPCs = NewEntry("NPC");
		Projectiles = NewEntry("Projectiles");
		Players = NewEntry("Players");
		Items = NewEntry("Items");
		Rain = NewEntry("Rain");
		Gore = NewEntry("Gore");
		Dust = NewEntry("Dust");
		Particles = NewEntry("Particles");
		LeashedEntities = NewEntry("Leashed Entities");
		Interface = NewEntry("Interface");
		DrawFPSGraph = NewEntry("Draw FPS Graph");
		DrawTimeLogger = NewEntry("Draw Render Timings");
		Overlays = NewEntry("Overlays");
		Filters = NewEntry("Screen Filters/Blit");
		SunVisibility = NewEntry("Sun Visibility");
		MenuDrawTime = NewEntry("Menu");
		SplashDrawTime = NewEntry("Splash");
		DrawFullscreenMap = NewEntry("Full Screen Map");
		GCPause = NewEntry("GC Pause", TimeSpan.FromMilliseconds(1.0));
		TotalUpdate = NewEntry("Total Update", TimeSpan.FromMilliseconds(4.0));
		UpdatesInWorld = NewEntry("Updates in World", TimeSpan.FromMilliseconds(4.0));
		UpdatePlayers = NewEntry("Update Players");
		SpawnNPCs = NewEntry("Spawn NPCs");
		UpdateNPCs = NewEntry("Update NPCs");
		UpdateProjectiles = NewEntry("Update Projectiles");
		UpdateItems = NewEntry("Update Items");
		UpdateDust = NewEntry("Update Dust");
		UpdateGore = NewEntry("Update Gore");
		UpdateWorld = NewEntry("Update World (tiles)");
		UpdateTime = NewEntry("Update Time");
		UpdateLeashedEntities = NewEntry("Update Leashed Entities");
		UpdateFloatingText = NewEntry("Update Floating Text");
		_onNextFrame = new Queue<Action>();
		ABTestMode = (ABTestFlag ? 1 : 0);
		ABTestName = "New Draw Blacks";
		ColumnSpacing = 220;
		DrawnEntryNumber = 0;
		DrawLayer_Background = 0u;
		DrawLayer_Text = 1u;
		_entriesToDraw = new Queue<TimeLogData>();
		_PinnedCPUFormat = new FormatPool("Pinned to CPU #{0}", 64.0);
		_AssignedCPUFormat = new FormatPool("Assigned CPU #{0}", 64.0);
		_procThrottleFormat = new FormatPool("CPU Throttle/Boost {0:0}%", 200.0);
		_expectedCPUFormat = new FormatPool("Expected CPU Usage {0:0}%", 200.0);
		_terrariaCPUFormat = new FormatPool("Terraria CPU Usage {0:0}%", 200.0);
		_pendingCPUFormat = new FormatPool("#Threads pending CPU {0}", 100.0);
		_percentFormat = new FormatPool("{0,3:00}%", 100.0);
		_msFormat = new FormatPool("{0,5:F2}", 20.0, -10.0, 0.009999999776482582);
		_intFormat = new FormatPool("{0,5}", 5000.0);
		TotalDrawByRenderCount = new TimeLogData[9];
		for (int i = 0; i < TotalDrawByRenderCount.Length; i++)
		{
			TotalDrawByRenderCount[i] = NewEntry("Total Draw (Render #" + i + ")", TimeSpan.FromMilliseconds(15.0));
		}
		LightingByPass = new TimeLogData[4];
		for (int j = 0; j < LightingByPass.Length; j++)
		{
			LightingByPass[j] = NewEntry("Lighting Pass #" + j, TimeSpan.FromMilliseconds(1.0));
		}
	}

	public static void Reset()
	{
		OnNextFrame(() =>
		{
			foreach (TimeLogData entry in entries)
			{
				entry.Reset();
			}
		});
	}

	public static void ToggleLogging()
	{
		if (currentlyLogging)
		{
			endLoggingThisFrame = true;
			startLoggingNextFrame = false;
		}
		else
		{
			startLoggingNextFrame = true;
			endLoggingThisFrame = false;
			Main.NewText("Detailed logging started", 250, 250, 0);
		}
	}

	public static void OnNextFrame(Action action)
	{
		_onNextFrame.Enqueue(action);
	}

	public static void StartNextFrame()
	{
		while (_onNextFrame.Count > 0)
		{
			_onNextFrame.Dequeue()();
		}
		ABTest();
		foreach (TimeLogData entry in entries)
		{
			entry.StartNextFrame();
		}
		if (startLoggingNextFrame)
		{
			startLoggingNextFrame = false;
			_ = DateTime.Now;
			string path = Main.SavePath + Path.DirectorySeparatorChar + "TerrariaDrawLog.7z";
			try
			{
				logWriter = new StreamWriter(new GZipStream(new FileStream(path, FileMode.Create), CompressionMode.Compress));
				logBuilder = new StringBuilder(5000);
				framesToLog = 600;
				currentFrame = 1;
				currentlyLogging = true;
			}
			catch
			{
				Main.NewText("Detailed logging could not be started.", 250, 250, 0);
			}
		}
		if (currentlyLogging)
		{
			logBuilder.AppendLine($"Start of Frame #{currentFrame}");
		}
	}

	public static void EndDrawFrame()
	{
		if (currentFrame <= framesToLog)
		{
			logBuilder.AppendLine($"End of Frame #{currentFrame}");
			logBuilder.AppendLine();
			if (endLoggingThisFrame)
			{
				endLoggingThisFrame = false;
				logBuilder.AppendLine("Logging ended early");
				currentFrame = framesToLog;
			}
			if (logBuilder.Length > 4000)
			{
				logWriter.Write(logBuilder.ToString());
				logBuilder.Clear();
			}
			currentFrame++;
			if (currentFrame > framesToLog)
			{
				Main.NewText("Detailed logging ended.", 250, 250, 0);
				logWriter.Write(logBuilder.ToString());
				logBuilder.Clear();
				logBuilder = null;
				logWriter.Flush();
				logWriter.Close();
				logWriter = null;
				framesToLog = -1;
				currentFrame = 0;
				currentlyLogging = false;
			}
		}
	}

	private static void LogAdd(string logText, int ticks, bool newMax)
	{
		if (currentFrame != 0)
		{
			logBuilder.AppendLine(string.Format("    {0} : {1:F4}ms {2}", logText, Utils.SWTicksToTimeSpan(ticks).TotalMilliseconds, newMax ? " - New Maximum" : string.Empty));
		}
	}

	public static StartTimestamp Start()
	{
		return new StartTimestamp
		{
			ticks = Stopwatch.GetTimestamp()
		};
	}

	public static void ABTest()
	{
		if (Main.renderCount == 0 && ABTestMode == 2)
		{
			ABTestFlag = !ABTestFlag;
		}
		if (ABTestFlag)
		{
			DataSeriesHeaders[0] = "Baseline";
			DataSeriesHeaders[1] = ABTestName;
		}
		activeDataSeries = (ABTestFlag ? 1 : 0);
	}

	public static void DrawException(Exception e)
	{
		if (currentlyLogging)
		{
			logBuilder.AppendLine(e.ToString());
		}
	}

	public static void Draw()
	{
		DrawTimes();
		DrawExtras();
	}

	private static void DrawTimes()
	{
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_0102: Unknown result type (might be due to invalid IL or missing references)
		//IL_0107: Unknown result type (might be due to invalid IL or missing references)
		//IL_011d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0122: Unknown result type (might be due to invalid IL or missing references)
		//IL_0149: Unknown result type (might be due to invalid IL or missing references)
		//IL_015c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0155: Unknown result type (might be due to invalid IL or missing references)
		//IL_017e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0192: Unknown result type (might be due to invalid IL or missing references)
		//IL_018b: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01db: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_021a: Unknown result type (might be due to invalid IL or missing references)
		//IL_021f: Unknown result type (might be due to invalid IL or missing references)
		//IL_023c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0241: Unknown result type (might be due to invalid IL or missing references)
		//IL_0260: Unknown result type (might be due to invalid IL or missing references)
		//IL_0265: Unknown result type (might be due to invalid IL or missing references)
		//IL_0284: Unknown result type (might be due to invalid IL or missing references)
		//IL_0289: Unknown result type (might be due to invalid IL or missing references)
		DrawnEntryNumber = 0;
		foreach (TimeLogData entry in entries)
		{
			entry.pendingDisplay = entry.ShouldDrawByDefault;
		}
		int num = 100;
		TableWidth = ((DataSeriesHeaders[1] != null) ? 900 : 440);
		Main.tileBatch.SetLayer(DrawLayer_Background, 0);
		Main.tileBatch.Draw(TextureAssets.MagicPixel.Value, new Rectangle(0, num, TableWidth, 800), new Color(60, 60, 60, 80));
		DrawString("Render Time (ms) F7 to hide", new Vector2(80f, (float)(num - 16)), Color.White);
		DrawString("Median", new Vector2(273f, (float)num), Color.White);
		DrawString("P90", new Vector2(325f, (float)num), Color.White);
		DrawString("Max", new Vector2(365f, (float)num), Color.White);
		DrawString("Freq", new Vector2(400f, (float)num), Color.White);
		if (DataSeriesHeaders[1] != null)
		{
			DrawString(DataSeriesHeaders[0], new Vector2(280f, (float)(num - 16)), (activeDataSeries == 0) ? Color.LightGreen : Color.White);
			DrawString(DataSeriesHeaders[1], new Vector2((float)(280 + ColumnSpacing), (float)(num - 16)), (activeDataSeries == 1) ? Color.LightGreen : Color.White);
			DrawString("Delta", new Vector2((float)(320 + 2 * ColumnSpacing), (float)(num - 16)), Color.White);
			DrawString("Median", new Vector2((float)(273 + ColumnSpacing), (float)num), Color.White);
			DrawString("P90", new Vector2((float)(325 + ColumnSpacing), (float)num), Color.White);
			DrawString("Max", new Vector2((float)(365 + ColumnSpacing), (float)num), Color.White);
			DrawString("Freq", new Vector2((float)(400 + ColumnSpacing), (float)num), Color.White);
			DrawString("Median", new Vector2((float)(250 + 2 * ColumnSpacing), (float)num), Color.White);
			DrawString("P90", new Vector2((float)(360 + 2 * ColumnSpacing), (float)num), Color.White);
		}
		num += 16;
		DrawEntry(TotalDrawAndUpdate, ref num, -1);
		DrawEntry(TotalDraw, ref num, -1);
		if (!Main.drawToScreen)
		{
			DrawEntry(TotalDrawByRenderCount[0], ref num);
			DrawEntry(TotalDrawByRenderCount[1], ref num);
			DrawEntry(RenderLiquid, ref num, 1);
			DrawEntry(LiquidDrawCalls, ref num, 2);
			DrawEntry(TotalDrawByRenderCount[2], ref num);
			if (RenderUndergroundBackground.pendingDisplay)
			{
				DrawEntry(RenderUndergroundBackground, ref num, 1);
				DrawEntry(DrawUndergroundBackground, ref num, 2);
			}
			DrawEntry(RenderBackgroundLiquid, ref num, 1);
			DrawEntry(LiquidBackgroundDrawCalls, ref num, 2);
			DrawEntry(TotalDrawByRenderCount[3], ref num);
			DrawEntry(RenderBlacksAndWalls, ref num, 1);
			DrawEntry(DrawBlackTiles, ref num, 2);
			DrawEntry(DrawWallTiles, ref num, 2);
			DrawEntry(FlushWallTiles, ref num, 3);
			DrawEntry(WallDrawCalls, ref num, 4);
			DrawEntry(RenderSolidTiles, ref num, 1);
			DrawEntry(FlushSolidTiles, ref num, 2);
			DrawEntry(SolidDrawCalls, ref num, 3);
			DrawEntry(RenderNonSolidTiles, ref num, 1);
			DrawEntry(FlushNonSolidTiles, ref num, 2);
			DrawEntry(NonSolidDrawCalls, ref num, 3);
			DrawWaterTiles.pendingDisplay = false;
			DrawBackgroundWaterTiles.pendingDisplay = false;
			DrawSolidTiles.pendingDisplay = false;
			DrawNonSolidTiles.pendingDisplay = false;
		}
		TimeLogData[] totalDrawByRenderCount = TotalDrawByRenderCount;
		foreach (TimeLogData timeLogData in totalDrawByRenderCount)
		{
			if (timeLogData.pendingDisplay)
			{
				DrawEntry(timeLogData, ref num);
			}
		}
		if (TotalDrawRenderNow.pendingDisplay)
		{
			DrawEntry(TotalDrawRenderNow, ref num);
		}
		num += 12;
		DrawEntry(Lighting, ref num);
		if (LightingInit.pendingDisplay)
		{
			DrawEntry(LightingInit, ref num, 1);
		}
		totalDrawByRenderCount = LightingByPass;
		foreach (TimeLogData timeLogData2 in totalDrawByRenderCount)
		{
			if (timeLogData2.pendingDisplay)
			{
				DrawEntry(timeLogData2, ref num, 1);
			}
		}
		num += 12;
		DrawEntry(TotalUpdate, ref num);
		DrawEntry(UpdatesInWorld, ref num, 1);
		num += 12;
		foreach (TimeLogData entry2 in entries)
		{
			if (entry2.pendingDisplay)
			{
				DrawEntry(entry2, ref num);
			}
		}
	}

	private static void DrawExtras()
	{
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_020c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0211: Unknown result type (might be due to invalid IL or missing references)
		//IL_0119: Unknown result type (might be due to invalid IL or missing references)
		//IL_011e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0142: Unknown result type (might be due to invalid IL or missing references)
		//IL_0147: Unknown result type (might be due to invalid IL or missing references)
		//IL_015f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0164: Unknown result type (might be due to invalid IL or missing references)
		if (Platform.IsWindows)
		{
			WindowsPerformanceDiagnostics.Data data = WindowsPerformanceDiagnostics.GetData();
			int num = Main.screenWidth - 180;
			int num2 = Main.screenHeight - 100;
			DrawString((data.PinnedToProcessor ? _PinnedCPUFormat : _AssignedCPUFormat).Format(data.CurrentProcessor), new Vector2((float)num, (float)(num2 += 12)), Color.White);
			DrawString(_procThrottleFormat.Format(data.ProcessorThrottlePercent), new Vector2((float)num, (float)(num2 += 12)), Color.White);
			DrawString(_expectedCPUFormat.Format(data.ExpectedCPUPercent), new Vector2((float)num, (float)(num2 += 12)), Color.White);
			if (data.PinnedToProcessor)
			{
				DrawString(_terrariaCPUFormat.Format(data.MainThreadCPUPercent), new Vector2((float)num, (float)(num2 += 12)), Color.White);
			}
			DrawString(_pendingCPUFormat.Format(data.ContentionQueueLength), new Vector2((float)num, (float)(num2 += 12)), Color.White);
			num2 += 2;
			if (data.RecommendUnpinning)
			{
				DrawString("Core pinning may be", new Vector2((float)num, (float)(num2 += 11)), Color.Orange);
				DrawString("reducing performance", new Vector2((float)num, (float)(num2 += 12)), Color.Orange);
			}
		}
		if (!PlayerInput.UsingGamepad)
		{
			return;
		}
		for (int i = 0; i < 2; i++)
		{
			string text = "";
			int num3 = Main.screenWidth - 220;
			int num4 = Main.screenHeight - 140 + i * 12;
			if (i == 0)
			{
				text = $"Thumbstick (L): {PlayerInput.GamepadThumbstickLeft.X,7:P2} ,{PlayerInput.GamepadThumbstickLeft.Y,7:P2}";
			}
			if (i == 1)
			{
				text = $"Thumbstick (R): {PlayerInput.GamepadThumbstickRight.X,7:P2} ,{PlayerInput.GamepadThumbstickRight.Y,7:P2}";
			}
			DrawString(text, new Vector2((float)num3, (float)num4), Color.White);
		}
	}

	private static void DrawEntry(TimeLogData e, ref int y, int indent = 0)
	{
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		e.pendingDisplay = false;
		if (DrawnEntryNumber++ % 2 == 1)
		{
			Main.tileBatch.SetLayer(DrawLayer_Background, 0);
			Main.tileBatch.Draw(TextureAssets.MagicPixel.Value, new Rectangle(0, y, TableWidth, 12), new Color(0, 0, 0, 80));
		}
		int num = 0;
		DrawString(e.name, new Vector2((float)(num + 20 + indent * 12), (float)y), PerformanceColor(e.data[0].median, e.budget));
		DrawEntryData(num, y, e.data[0], e.format, e.budget);
		num += ColumnSpacing;
		DrawEntryData(num, y, e.data[1], e.format, e.budget);
		num += ColumnSpacing;
		DrawDelta(num, y, e.data[0], e.data[1], e.format, e.budget);
		y += 12;
	}

	private static void DrawEntryData(int x, int y, DataSeries data, Func<int, string> format, int budget)
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		if (data.HasData)
		{
			DrawValue(data.previous, new Vector2((float)(x + 240), (float)y), format, budget);
			DrawValue(data.median, new Vector2((float)(x + 280), (float)y), format, budget);
			DrawValue(data.p90, new Vector2((float)(x + 320), (float)y), format, budget);
			DrawValue(data.max, new Vector2((float)(x + 360), (float)y), format, budget);
			if (data.frequency < 1f)
			{
				DrawString(_percentFormat.Format(data.frequency * 100f), new Vector2((float)(x + 400), (float)y), Color.White);
			}
		}
	}

	private static void DrawDelta(int x, int y, DataSeries data0, DataSeries data1, Func<int, string> format, int budget)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		if (data0.HasData && data1.HasData)
		{
			DrawDelta(data0.median, data1.median, new Vector2((float)(x + 240), (float)y), format, budget);
			DrawDelta(data0.p90, data1.p90, new Vector2((float)(x + 340), (float)y), format, budget);
		}
	}

	private static string FormatTicks(int ticks)
	{
		TimeSpan timeSpan = Utils.SWTicksToTimeSpan(ticks);
		string text = _msFormat.Format((float)timeSpan.TotalMilliseconds);
		if (text.Length > 5)
		{
			text = text.Substring(0, 5);
		}
		return text;
	}

	private static void DrawValue(int value, Vector2 pos, Func<int, string> format, int budget)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		DrawString(format(value), pos, PerformanceColor(value, budget));
	}

	private static void DrawDelta(int value0, int value1, Vector2 pos, Func<int, string> format, int budget)
	{
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		int num = value1 - value0;
		int num2 = (int)Math.Round((double)value1 * 100.0 / (double)value0 - 100.0);
		if (value0 != 0 && Math.Abs(num2) > 1 && Math.Abs(num * 100) > budget)
		{
			Color color = ((num2 < 0) ? Color.Lerp(Color.White, new Color(0, 200, 0), (float)(-num2) / 100f) : Color.Lerp(Color.White, new Color(200, 0, 0), (float)num2 / 100f));
			DrawString(format(num), pos, color);
			pos.X += 40f;
			DrawString(_percentFormat.Format(num2), pos, color);
		}
	}

	private static void DrawString(string text, Vector2 pos, Color color)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		ChatManager.DrawStringWithShadowFast(Main.tileBatch, FontAssets.MouseText.Value, text, pos, color, Vector2.Zero, 0.75f, DrawLayer_Text, 1.5f);
	}

	private static Color PerformanceColor(long value, long budget)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		Color white = Color.White;
		Color val = new Color(255, 255, 170);
		Color val2 = new Color(255, 160, 50);
		Color val3 = new Color(255, 50, 50);
		Color val4 = new Color(200, 0, 0);
		float num = (float)((double)value / (double)budget);
		if ((double)num <= 0.33)
		{
			return Color.Lerp(white, val, num / 0.33f);
		}
		if ((double)num <= 0.67)
		{
			return Color.Lerp(val, val2, (num - 0.33f) / 0.34f);
		}
		if (num <= 1f)
		{
			return Color.Lerp(val2, val3, (num - 0.67f) / 0.32999998f);
		}
		return Color.Lerp(val3, val4, Utils.Clamp(num - 1f, 0f, 1f));
	}
}
