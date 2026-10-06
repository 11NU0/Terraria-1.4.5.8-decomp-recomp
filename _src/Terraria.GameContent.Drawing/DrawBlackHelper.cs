using Microsoft.Xna.Framework;

namespace Terraria.GameContent.Drawing;

public struct DrawBlackHelper(uint layer, Vector2 drawOffset)
{
	private readonly uint layer = layer;

	private readonly Vector2 drawOffset = drawOffset;

	private int y = 0;

	private int startX = 0;

	private int endX = 0;

	public void DrawBlack(int x, int y)
	{
		if (y == this.y && x == endX)
		{
			endX++;
			return;
		}
		EndStrip();
		this.y = y;
		startX = x;
		endX = x + 1;
	}

	public void EndStrip()
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		if (startX != endX)
		{
			Vector2 val = new Vector2((float)(startX << 4), (float)(y << 4)) - Main.screenPosition + drawOffset;
			Main.tileBatch.SetLayer(layer, 0);
			Main.tileBatch.Draw(TextureAssets.BlackTile.Value, new Vector4(val.X, val.Y, (float)(endX - startX << 4), 16f), Color.Black);
		}
	}
}
