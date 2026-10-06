using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;

namespace Terraria.GameContent.UI.ResourceSets;

public struct ResourceDrawSettings
{
	public delegate void TextureGetter(int elementIndex, int firstElementIndex, int lastElementIndex, out Asset<Texture2D> texture, out Vector2 drawOffset, out float drawScale, out Rectangle? sourceRect);

	public Vector2 TopLeftAnchor;

	public int ElementCount;

	public int ElementIndexOffset;

	public TextureGetter GetTextureMethod;

	public Vector2 OffsetPerDraw;

	public Vector2 OffsetPerDrawByTexturePercentile;

	public Vector2 OffsetSpriteAnchor;

	public Vector2 OffsetSpriteAnchorByTexturePercentile;

	public void Draw(SpriteBatch spriteBatch, ref bool isHovered)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_010b: Unknown result type (might be due to invalid IL or missing references)
		//IL_010d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0112: Unknown result type (might be due to invalid IL or missing references)
		//IL_0114: Unknown result type (might be due to invalid IL or missing references)
		//IL_011a: Unknown result type (might be due to invalid IL or missing references)
		//IL_011f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0124: Unknown result type (might be due to invalid IL or missing references)
		//IL_0129: Unknown result type (might be due to invalid IL or missing references)
		//IL_012e: Unknown result type (might be due to invalid IL or missing references)
		int elementCount = ElementCount;
		Vector2 val = TopLeftAnchor;
		Point val2 = Main.MouseScreen.ToPoint();
		for (int i = 0; i < elementCount; i++)
		{
			int elementIndex = i + ElementIndexOffset;
			GetTextureMethod(elementIndex, ElementIndexOffset, ElementIndexOffset + elementCount - 1, out var texture, out var drawOffset, out var drawScale, out var sourceRect);
			Rectangle val3 = texture.Frame();
			if (sourceRect.HasValue)
			{
				val3 = sourceRect.Value;
			}
			Vector2 val4 = val + drawOffset;
			Vector2 val5 = OffsetSpriteAnchor + val3.Size() * OffsetSpriteAnchorByTexturePercentile;
			Rectangle val6 = val3;
			val6.X += (int)(val4.X - val5.X);
			val6.Y += (int)(val4.Y - val5.Y);
			if (val6.Contains(val2))
			{
				isHovered = true;
			}
			spriteBatch.Draw(texture.Value, val4, (Rectangle?)val3, Color.White, 0f, val5, drawScale, (SpriteEffects)0, 0f);
			val += OffsetPerDraw + val3.Size() * OffsetPerDrawByTexturePercentile;
		}
	}
}
