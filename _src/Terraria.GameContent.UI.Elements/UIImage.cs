using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.UI;

namespace Terraria.GameContent.UI.Elements;

public class UIImage : UIElement
{
	private Asset<Texture2D> _texture;

	public float ImageScale = 1f;

	public float Rotation;

	public bool ScaleToFit;

	public bool AllowResizingDimensions = true;

	public Color Color = Color.White;

	public Vector2 NormalizedOrigin = Vector2.Zero;

	public Rectangle? Frame;

	public bool RemoveFloatingPointsFromDrawPosition;

	public bool UseTextureSizeForOrigin = true;

	private Texture2D _nonReloadingTexture;

	public Asset<Texture2D> Texture => _texture;

	protected UIImage()
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
	}

	public UIImage(Asset<Texture2D> texture)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		SetImage(texture);
	}

	public UIImage(Texture2D nonReloadingTexture)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		SetImage(nonReloadingTexture);
	}

	public void SetImage(Asset<Texture2D> texture)
	{
		_texture = texture;
		_nonReloadingTexture = null;
		if (AllowResizingDimensions)
		{
			Width.Set(_texture.Width(), 0f);
			Height.Set(_texture.Height(), 0f);
		}
	}

	public void SetImage(Texture2D nonReloadingTexture)
	{
		_texture = null;
		_nonReloadingTexture = nonReloadingTexture;
		if (AllowResizingDimensions)
		{
			Width.Set(_nonReloadingTexture.Width, 0f);
			Height.Set(_nonReloadingTexture.Height, 0f);
		}
	}

	protected override void DrawSelf(SpriteBatch spriteBatch)
	{
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		CalculatedStyle dimensions = GetDimensions();
		Texture2D val = null;
		if (_texture != null)
		{
			val = _texture.Value;
		}
		if (_nonReloadingTexture != null)
		{
			val = _nonReloadingTexture;
		}
		if (ScaleToFit)
		{
			spriteBatch.Draw(val, dimensions.ToRectangle(), Frame, Color);
			return;
		}
		Vector2 val2 = val.Size();
		Vector2 val3 = new Vector2(dimensions.Width, dimensions.Height);
		if (UseTextureSizeForOrigin)
		{
			val3 = val2;
		}
		Vector2 val4 = dimensions.Position() + val3 * (1f - ImageScale) / 2f + val3 * NormalizedOrigin;
		if (RemoveFloatingPointsFromDrawPosition)
		{
			val4 = val4.Floor();
		}
		spriteBatch.Draw(val, val4, Frame, Color, Rotation, val2 * NormalizedOrigin, ImageScale, (SpriteEffects)0, 0f);
	}
}
