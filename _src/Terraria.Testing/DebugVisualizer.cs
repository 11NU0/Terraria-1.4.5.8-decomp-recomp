using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Terraria.Testing;

public class DebugVisualizer
{
	public enum UpdatePhase
	{
		Update,
		UpdateInWorld,
		Draw
	}

	private class DrawItem
	{
		public readonly UpdatePhase Phase = CurrentPhase;

		public readonly Action<SpriteBatch> Draw;

		public int TimeLeft;

		public DrawItem(Action<SpriteBatch> draw, int lifetime)
		{
			Draw = draw;
			TimeLeft = lifetime;
		}
	}

	private class PhaseOverrideHandle : IDisposable
	{
		private readonly UpdatePhase _prev;

		public PhaseOverrideHandle(UpdatePhase phase)
		{
			UpdatePhase currentPhase = CurrentPhase;
			CurrentPhase = phase;
			_prev = currentPhase;
		}

		public void Dispose()
		{
			CurrentPhase = _prev;
		}
	}

	public static readonly DebugVisualizer UI = new DebugVisualizer(ui: true);

	public static readonly DebugVisualizer World = new DebugVisualizer(ui: false);

	private static UpdatePhase CurrentPhase;

	private readonly List<DrawItem> items = new List<DrawItem>();

	private readonly bool _ui;

	private DebugVisualizer(bool ui)
	{
		_ui = ui;
	}

	public void Add(Action<SpriteBatch> draw = null, int lifetime = 1)
	{
		items.Add(new DrawItem(draw, lifetime));
	}

	public void AddLine(Vector2 start, Vector2 end, Color colorStart, Color colorEnd = default(Color), int lifetime = 1, float width = 1f)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		if (colorEnd == default(Color))
		{
			colorEnd = colorStart;
		}
		Add((SpriteBatch sb) =>
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			Utils.DrawLine(sb, start, end, colorStart, colorEnd, width);
		}, lifetime);
	}

	public void AddLine(Point start, Point end, Color colorStart, Color colorEnd = default(Color), int lifetime = 1, float width = 1f)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		AddLine(start.ToVector2(), end.ToVector2(), colorStart, colorEnd, lifetime, width);
	}

	public void AddRectangle(Rectangle rect, Color color, int lifetime = 1, float width = 1f)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		AddRectangle(rect.TopLeft(), rect.BottomRight(), color, color, lifetime, width);
	}

	public void AddRectangle(Vector2 start, Vector2 end, Color colorStart, Color colorEnd = default(Color), int lifetime = 1, float width = 1f)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		AddLine(start, new Vector2(start.X, end.Y), colorStart, colorEnd, lifetime, width);
		AddLine(start, new Vector2(end.X, start.Y), colorStart, colorEnd, lifetime, width);
		AddLine(end, new Vector2(start.X, end.Y), colorStart, colorEnd, lifetime, width);
		AddLine(end, new Vector2(end.X, start.Y), colorStart, colorEnd, lifetime, width);
	}

	public void AddFilledRectangle(Rectangle rect, Color color, int lifetime = 1)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		Vector2 val = rect.TopLeft();
		Vector2 val2 = rect.BottomRight();
		AddLine(new Vector2(val.X, (val.Y + val2.Y) / 2f), new Vector2(val2.X, (val.Y + val2.Y) / 2f), color, color, lifetime, rect.Height);
	}

	public static void PreUpdate()
	{
		SetPhase(UpdatePhase.Update);
	}

	public static void PreWorldUpdate()
	{
		SetPhase(UpdatePhase.UpdateInWorld);
	}

	public static void PreDraw()
	{
		SetPhase(UpdatePhase.Draw);
	}

	private static void SetPhase(UpdatePhase phase)
	{
		CurrentPhase = phase;
		UI.Tick();
		World.Tick();
	}

	public void Tick()
	{
		int num = 0;
		for (int i = 0; i < items.Count; i++)
		{
			DrawItem drawItem = items[i];
			if (drawItem.Phase == CurrentPhase)
			{
				drawItem.TimeLeft--;
			}
			if (drawItem.TimeLeft > 0)
			{
				items[num++] = drawItem;
			}
		}
		items.RemoveRange(num, items.Count - num);
	}

	public void Draw(SpriteBatch spriteBatch)
	{
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		if (items.Count == 0)
		{
			return;
		}
		spriteBatch.Begin((SpriteSortMode)0, BlendState.AlphaBlend, Main.DefaultSamplerState, (DepthStencilState)null, (RasterizerState)null, (Effect)null, _ui ? (Matrix.CreateTranslation(Main.screenPosition.X, Main.screenPosition.Y, 0f) * Main.UIScaleMatrix) : Main.GameViewMatrix.TransformationMatrix);
		foreach (DrawItem item in items)
		{
			item.Draw(spriteBatch);
		}
		spriteBatch.End();
	}

	public static IDisposable InPhase(UpdatePhase phase)
	{
		return new PhaseOverrideHandle(phase);
	}
}
