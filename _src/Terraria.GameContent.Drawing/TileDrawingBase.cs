using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;
using Terraria.Graphics;

namespace Terraria.GameContent.Drawing;

public class TileDrawingBase
{
	public static bool DrawOwnBlacks = true;

	protected TimeLogger.TimeLogData FlushLogData;

	protected TimeLogger.TimeLogData DrawCallLogData;

	private SpriteBatchBeginner batchBeginner;

	public void Begin(RasterizerState rasterizer, Matrix transformation)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		batchBeginner = new SpriteBatchBeginner((SpriteSortMode)0, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, rasterizer, null, transformation);
		batchBeginner.Begin(Main.tileBatch);
		batchBeginner.Begin(Main.spriteBatch);
	}

	public void End()
	{
		TimeLogger.StartTimestamp fromTimestamp = TimeLogger.Start();
		int num = Main.tileBatch.End();
		num += Main.spriteBatch.PendingDrawCallCount();
		Main.spriteBatch.End();
		DrawCallLogData.Add(num);
		FlushLogData.AddTime(fromTimestamp);
	}

	public void RestartLayeredBatch()
	{
		TimeLogger.StartTimestamp fromTimestamp = TimeLogger.Start();
		int value = Main.tileBatch.Restart();
		DrawCallLogData.Add(value);
		FlushLogData.AddTime(fromTimestamp);
	}

	public void RestartSpriteBatch()
	{
		TimeLogger.StartTimestamp fromTimestamp = TimeLogger.Start();
		int value = Main.spriteBatch.PendingDrawCallCount();
		Main.spriteBatch.End();
		batchBeginner.Begin(Main.spriteBatch);
		DrawCallLogData.Add(value);
		FlushLogData.AddTime(fromTimestamp);
	}
}
