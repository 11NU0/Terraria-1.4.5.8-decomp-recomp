using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ReLogic.Graphics;
using Terraria.GameContent;
using Terraria.Utilities;

namespace Terraria.Testing;

public static class DetailedFPS
{
	public enum OperationCategory
	{
		Idle,
		Update,
		Draw,
		Present,
		GC,
		End,
		Count
	}

	private struct Frame
	{
		public struct Event(OperationCategory category, long timestamp)
		{
			public OperationCategory category = category;

			public long timestamp = timestamp;
		}

		public List<Event> events;

		public int[] CollectionCount;

		public long Allocated;

		public void Init()
		{
			events = new List<Event>(16);
			CollectionCount = new int[GC.MaxGeneration + 1];
		}

		public void Start()
		{
			events.Clear();
			Begin(OperationCategory.Idle);
		}

		public void Begin(OperationCategory category)
		{
			if (events.Count >= 1000 || (events.Count > 0 && events.Last().category == category))
			{
				return;
			}
			long timestamp = Stopwatch.GetTimestamp();
			if (events.Count > 0)
			{
				Event obj = events.Last();
				if (obj.category == OperationCategory.Draw || obj.category == OperationCategory.Update)
				{
					TimeLogger.TotalDrawAndUpdate.Add((int)(timestamp - obj.timestamp));
				}
			}
			events.Add(new Event(category, timestamp));
			TimeSpan gCPauseTime = GetGCPauseTime();
			if (gCPauseTime > TimeSpan.Zero)
			{
				long num = Utils.TimeSpanToSWTicks(gCPauseTime);
				events.Insert(events.Count - 1, new Event(OperationCategory.GC, timestamp - num));
				TimeLogger.GCPause.Add((int)num);
			}
		}

		public void Finish()
		{
			Begin(OperationCategory.End);
			for (int i = 0; i <= GC.MaxGeneration; i++)
			{
				CollectionCount[i] = GetCollectionCount(i);
			}
			if (Main.CollectGen0EveryFrame)
			{
				CollectionCount[0]--;
			}
			Allocated = GetAllocatedBytes();
		}
	}

	public static readonly int FrameCount;

	private static Frame[] Frames;

	private static int oldest;

	private static int newest;

	private static TimeSpan LastGCPauseTime;

	private static int[] LastCollectionCount;

	private static long LastAllocatedBytes;

	private const int PixelsPerMs = 6;

	private const int FrameWidth = 2;

	private const int BoxHeight = 100;

	private static string[] _gcGenText;

	public static uint NonRepeatedFrameCount;

	public static TimeSpan CurrentFrameTime
	{
		get
		{
			Frame frame = Frames[newest];
			return Utils.SWTicksToTimeSpan(frame.events.Last().timestamp - frame.events[0].timestamp);
		}
	}

	static DetailedFPS()
	{
		FrameCount = 300;
		Frames = new Frame[FrameCount];
		LastCollectionCount = new int[GC.MaxGeneration + 1];
		_gcGenText = new string[3] { "G0", "G1", "G2" };
		NonRepeatedFrameCount = 0u;
		for (int i = 0; i < Frames.Length; i++)
		{
			Frames[i].Init();
		}
	}

	public static void StartNextFrame()
	{
		TimeLogger.StartNextFrame();
		Frames[newest].Finish();
		newest++;
		if (newest == Frames.Length)
		{
			newest = 0;
		}
		if (newest == oldest)
		{
			oldest++;
		}
		if (oldest == Frames.Length)
		{
			oldest = 0;
		}
		Frames[newest].Start();
	}

	public static void Begin(OperationCategory category)
	{
		Frames[newest].Begin(category);
	}

	public static void End()
	{
		TimeLogger.EndDrawFrame();
		Begin(OperationCategory.Idle);
	}

	public static float GetCPUUtilization(int numFrames)
	{
		long[] array = new long[6];
		int num = 0;
		foreach (Frame item in EnumerateFrames())
		{
			if (num++ == numFrames)
			{
				break;
			}
			if (item.events.Count < 2)
			{
				continue;
			}
			Frame.Event obj = item.events[0];
			foreach (Frame.Event @event in item.events)
			{
				array[(int)obj.category] += @event.timestamp - obj.timestamp;
				obj = @event;
			}
		}
		long num2 = array.Sum();
		return (float)((double)(array[2] + array[1]) / (double)num2);
	}

	public static bool VsyncAppearsActive()
	{
		long num = 0L;
		int num2 = 60;
		int num3 = 0;
		foreach (Frame item in EnumerateFrames())
		{
			if (num3++ == num2)
			{
				break;
			}
			if (item.events.Count < 2)
			{
				continue;
			}
			Frame.Event obj = item.events[0];
			foreach (Frame.Event @event in item.events)
			{
				if (obj.category == OperationCategory.Present)
				{
					num += @event.timestamp - obj.timestamp;
				}
				obj = @event;
			}
		}
		return Utils.SWTicksToTimeSpan(num / num2).TotalSeconds >= Main.TARGET_FRAME_TIME * 0.1;
	}

	private static TimeSpan GetGCPauseTime()
	{
		TimeSpan timeSpan = NewRuntimeMethods.GC_GetTotalPauseDuration();
		TimeSpan result = timeSpan - LastGCPauseTime;
		LastGCPauseTime = timeSpan;
		return result;
	}

	private static int GetCollectionCount(int gen)
	{
		int num = GC.CollectionCount(gen);
		int result = num - LastCollectionCount[gen];
		LastCollectionCount[gen] = num;
		return result;
	}

	private static int GetAllocatedBytes()
	{
		long num = NewRuntimeMethods.GC_GetTotalAllocatedBytes();
		long num2 = num - LastAllocatedBytes;
		LastAllocatedBytes = num;
		return (int)num2;
	}

	private static IEnumerable<Frame> EnumerateFrames()
	{
		int k = newest;
		while (k != oldest)
		{
			int num = k - 1;
			k = num;
			if (num < 0)
			{
				k = Frames.Length - 1;
			}
			yield return Frames[k];
		}
	}

	public static void Draw()
	{
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_0138: Unknown result type (might be due to invalid IL or missing references)
		//IL_013d: Unknown result type (might be due to invalid IL or missing references)
		Rectangle val = new Rectangle((Main.screenWidth - Frames.Length * 2) / 2, Main.screenHeight - 100, Frames.Length * 2, 100);
		DrawFlickerTests(val);
		DrawFPSBox(val);
		int num = 0;
		long num2 = 0L;
		foreach (Frame item in EnumerateFrames())
		{
			num++;
			DrawFrame(val.Right - num * 2, item);
			num2 += item.Allocated;
		}
		if (num2 > 0)
		{
			long num3 = num2 / num;
			DynamicSpriteFontExtensionMethods.DrawString(Main.spriteBatch, FontAssets.MouseText.Value, $"Avg Alloc: {num3,5} bytes/frame", val.TopRight() + new Vector2(10f, -20f), Color.White);
		}
		if (WindowsPerformanceDiagnostics.Supported)
		{
			DrawWindowsDisplayDiagnostics(val.Right + 100);
		}
		if (Main.keyState.PressingAlt())
		{
			DynamicSpriteFontExtensionMethods.DrawString(Main.spriteBatch, FontAssets.MouseText.Value, $"Time Acc: {Main.UpdateTimeAccumulator * 1000.0,5:0.0} ms", new Vector2((float)(Main.screenWidth - 200), (float)(Main.screenHeight - 24)), Color.White);
		}
	}

	private static void DrawFPSBox(Rectangle r)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Unknown result type (might be due to invalid IL or missing references)
		Color white = Color.White;
		DrawRect(new Rectangle(r.Left, r.Y, 2, r.Height), white);
		DrawRect(new Rectangle(r.Right, r.Y, 2, r.Height), white);
		DrawRect(new Rectangle(r.Left, r.Y, r.Width, 1), white);
		int num = 24;
		OperationCategory operationCategory = OperationCategory.Idle;
		while (operationCategory <= OperationCategory.GC)
		{
			if (operationCategory != OperationCategory.GC || !(LastGCPauseTime == TimeSpan.Zero))
			{
				DrawRect(new Rectangle(r.Right + 8, r.Bottom - num + 8, 8, 8), GetColor(operationCategory));
				DynamicSpriteFontExtensionMethods.DrawString(Main.spriteBatch, FontAssets.MouseText.Value, operationCategory.ToString(), new Vector2((float)(r.Right + 20), (float)(r.Bottom - num)), Color.White);
			}
			operationCategory++;
			num += 24;
		}
	}

	private static void DrawFrame(int x, Frame frame)
	{
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0101: Unknown result type (might be due to invalid IL or missing references)
		if (frame.events.Count < 2)
		{
			return;
		}
		int val = 0;
		Frame.Event obj = frame.events[0];
		long timestamp = obj.timestamp;
		for (int i = 1; i < frame.events.Count; i++)
		{
			Frame.Event obj2 = frame.events[i];
			int num = (int)(Utils.SWTicksToTimeSpan(obj.timestamp - timestamp).TotalMilliseconds * 6.0);
			int num2 = (int)(Utils.SWTicksToTimeSpan(obj2.timestamp - timestamp).TotalMilliseconds * 6.0);
			DrawRect(new Rectangle(x, Main.screenHeight - num2, 2, num2 - num), GetColor(obj.category));
			obj = obj2;
			val = num2;
		}
		val = Math.Max(val, 100);
		for (int j = 0; j <= GC.MaxGeneration; j++)
		{
			for (int k = 0; k < frame.CollectionCount[j]; k++)
			{
				DynamicSpriteFontExtensionMethods.DrawString(Main.spriteBatch, FontAssets.MouseText.Value, _gcGenText[j], new Vector2((float)(x - 10), (float)(Main.screenHeight - val - 15)), Color.White, 0f, Vector2.Zero, 0.75f, (SpriteEffects)0, 0f, (Vector2[])null, (Color[])null);
				val += 10;
			}
		}
	}

	private static Color GetColor(OperationCategory category)
	{
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		return category switch
		{
			OperationCategory.Update => Color.Orange, 
			OperationCategory.Draw => Color.Green, 
			OperationCategory.Present => Color.Magenta, 
			OperationCategory.Idle => Color.Gray, 
			OperationCategory.GC => Color.Blue, 
			_ => Color.Black, 
		};
	}

	private static void DrawFlickerTests(Rectangle fpsRect)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		int height = fpsRect.Height;
		Rectangle val = new Rectangle(fpsRect.X - height * 4, fpsRect.Y, height * 4, fpsRect.Height);
		DrawBoxFlickerTest(new Rectangle(val.X, val.Y, height, height));
		DrawScrollFlickerTest(new Rectangle(val.X + height, val.Y, val.Width - height, height));
		DynamicSpriteFontExtensionMethods.DrawString(Main.spriteBatch, FontAssets.MouseText.Value, "Frame Skip: " + Main.FrameSkipMode, new Vector2((float)val.Left, (float)(val.Top - 20)), Color.White);
		DynamicSpriteFontExtensionMethods.DrawString(Main.spriteBatch, FontAssets.MouseText.Value, "[R] to Render Now", new Vector2((float)(val.Left + 180), (float)(val.Top - 20)), Color.White);
		if (Main.keyState.IsKeyDown((Keys)82))
		{
			Main.renderNow = true;
		}
	}

	private static void DrawScrollFlickerTest(Rectangle r)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		DrawRect(r, Color.LightGray);
		Rectangle val = r;
		val.Inflate(-10, -10);
		int num = 100;
		int num2 = 51;
		int num3 = (int)(NonRepeatedFrameCount * num2 % num);
		for (int i = -1; i <= 3; i++)
		{
			DrawRect(Rectangle.Intersect(new Rectangle(r.X + num3 + i * num, r.Y, 12, r.Height), val), Color.Magenta);
		}
	}

	private static void DrawBoxFlickerTest(Rectangle r)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		DrawRect(r, Color.Black);
		for (int i = 0; i < 5; i++)
		{
			DrawRect(new Rectangle(r.X + i * r.Width / 4, r.Y, 1, r.Height), Color.White);
			DrawRect(new Rectangle(r.X, r.Y + i * r.Height / 4, r.Width, 1), Color.White);
		}
		int num = (int)(NonRepeatedFrameCount % 16);
		r.Width /= 4;
		r.Height /= 4;
		r.X += num / 4 * r.Width;
		r.Y += num % 4 * r.Width;
		DrawRect(r, Color.Magenta);
	}

	private static void DrawRect(Rectangle r, Color c)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		Main.spriteBatch.Draw(TextureAssets.MagicPixel.Value, r, c);
	}

	private static void DrawWindowsDisplayDiagnostics(int x)
	{
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		//IL_0112: Unknown result type (might be due to invalid IL or missing references)
		//IL_0117: Unknown result type (might be due to invalid IL or missing references)
		//IL_014e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0153: Unknown result type (might be due to invalid IL or missing references)
		int num = Main.screenHeight - 84;
		WindowsPerformanceDiagnostics.DisplayData displayData = WindowsPerformanceDiagnostics.GetDisplayData();
		string text = (displayData.RefreshRate.HasValue ? (displayData.RefreshRate.Value + " Hz") : "?");
		DynamicSpriteFontExtensionMethods.DrawString(Main.spriteBatch, FontAssets.MouseText.Value, "Refresh Rate: " + text, new Vector2((float)x, (float)num), Color.White);
		num += 16;
		DynamicSpriteFontExtensionMethods.DrawString(Main.spriteBatch, FontAssets.MouseText.Value, "VRR: " + OptStr(displayData.VRREnabled), new Vector2((float)x, (float)num), Color.White);
		num += 16;
		DynamicSpriteFontExtensionMethods.DrawString(Main.spriteBatch, FontAssets.MouseText.Value, "MPO: " + OptStr(displayData.MPOEnabled), new Vector2((float)x, (float)num), Color.White);
		num += 16;
		DynamicSpriteFontExtensionMethods.DrawString(Main.spriteBatch, FontAssets.MouseText.Value, "HAGS: " + OptStr(displayData.HagsEnabled), new Vector2((float)x, (float)num), Color.White);
		num += 16;
		DynamicSpriteFontExtensionMethods.DrawString(Main.spriteBatch, FontAssets.MouseText.Value, "Windowed Swap Opt: " + OptStr(displayData.WindowedGameOptimizationsEnabled), new Vector2((float)x, (float)num), Color.White);
		num += 16;
	}

	private static string OptStr(bool? v)
	{
		if (v.HasValue)
		{
			if (!v.Value)
			{
				return "Off";
			}
			return "On";
		}
		return "?";
	}
}
