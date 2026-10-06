using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.UI;

namespace Terraria.GameContent.UI.Elements;

public class UIImageWithBorder : UIImage
{
	private Asset<Texture2D> _borderTexture;

	private Texture2D _nonReloadingBorderTexture;

	public UIImageWithBorder(Asset<Texture2D> texture, Asset<Texture2D> borderTexture)
		: base(texture)
	{
		SetBorder(borderTexture);
	}

	public UIImageWithBorder(Texture2D nonReloadingTexture, Texture2D nonReloadingBorderTexture)
		: base(nonReloadingTexture)
	{
		SetBorder(nonReloadingBorderTexture);
	}

	public void SetBorder(Asset<Texture2D> texture)
	{
		_borderTexture = texture;
		_nonReloadingBorderTexture = null;
		Width.Set(_borderTexture.Width(), 0f);
		Height.Set(_borderTexture.Height(), 0f);
	}

	public void SetBorder(Texture2D nonReloadingTexture)
	{
		_borderTexture = null;
		_nonReloadingBorderTexture = nonReloadingTexture;
		Width.Set(_nonReloadingBorderTexture.Width, 0f);
		Height.Set(_nonReloadingBorderTexture.Height, 0f);
	}

	protected override void DrawSelf(SpriteBatch spriteBatch)
	{
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		base.DrawSelf(spriteBatch);
		CalculatedStyle dimensions = GetDimensions();
		Texture2D val = null;
		if (_borderTexture != null)
		{
			val = _borderTexture.Value;
		}
		if (_nonReloadingBorderTexture != null)
		{
			val = _nonReloadingBorderTexture;
		}
		if (ScaleToFit)
		{
			spriteBatch.Draw(val, dimensions.ToRectangle(), Color);
			return;
		}
		Vector2 val2 = val.Size();
		Vector2 val3 = dimensions.Position() + val2 * (1f - ImageScale) / 2f + val2 * NormalizedOrigin;
		if (RemoveFloatingPointsFromDrawPosition)
		{
			val3 = val3.Floor();
		}
		spriteBatch.Draw(val, val3, (Rectangle?)null, Color, Rotation, val2 * NormalizedOrigin, ImageScale, (SpriteEffects)0, 0f);
	}
}
