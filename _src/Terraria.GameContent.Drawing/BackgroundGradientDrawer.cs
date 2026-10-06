using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;

namespace Terraria.GameContent.Drawing;

public class BackgroundGradientDrawer
{
	private Color _color;

	private GetBackgroundDrawWeightMethod _weightGetter;

	private BackgroundArrayGetterMethod _textureGetter;

	private int[] _textureIndexesToCheck;

	private static Asset<Texture2D> _sunflareGradientDitherTexture;

	public BackgroundGradientDrawer(Color gradientColor, GetBackgroundDrawWeightMethod weightGetter, BackgroundArrayGetterMethod textureGetter, params int[] textureIndexesToCheck)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		_color = gradientColor;
		_weightGetter = weightGetter;
		_textureGetter = textureGetter;
		_textureIndexesToCheck = textureIndexesToCheck;
	}

	public void Draw()
	{
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		if (!Main.BackgroundEnabled)
		{
			return;
		}
		float num = _weightGetter();
		if (!(num <= 0f) && ShouldDrawForTextures() && Main.ShouldDrawSurfaceBackground())
		{
			if (_sunflareGradientDitherTexture == null)
			{
				_sunflareGradientDitherTexture = Main.Assets.Request<Texture2D>("Images/Misc/Sunflare/colorgradientdither", (AssetRequestMode)1);
			}
			SpriteBatch spriteBatch = Main.spriteBatch;
			Color val = new Color(_color.ToVector3() * Main.ColorOfSurfaceBackgroundsBase.ToVector3());
			spriteBatch.Draw(_sunflareGradientDitherTexture.Value, GetGradientRect(), (Rectangle?)null, val * num, 0f, Vector2.Zero, (SpriteEffects)0, 0f);
		}
	}

	private static Rectangle GetGradientRect()
	{
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		int num = 400;
		int num2 = Math.Max(0, (int)((Main.worldSurface * 16.0 - (double)Main.screenPosition.Y - 2400.0) * 0.10000000149011612)) - num;
		return new Rectangle(0, num2, Main.screenWidth, Main.screenHeight + num);
	}

	private bool ShouldDrawForTextures()
	{
		IEnumerable<int> enumerable = _textureGetter();
		int[] textureIndexesToCheck = _textureIndexesToCheck;
		foreach (int num in textureIndexesToCheck)
		{
			foreach (int item in enumerable)
			{
				if (num == item)
				{
					return true;
				}
			}
		}
		return false;
	}
}
