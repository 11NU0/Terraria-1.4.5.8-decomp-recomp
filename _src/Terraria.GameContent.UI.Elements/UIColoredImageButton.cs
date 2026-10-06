using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.Audio;
using Terraria.UI;

namespace Terraria.GameContent.UI.Elements;

public class UIColoredImageButton : UIElement
{
	private Asset<Texture2D> _backPanelTexture;

	private Asset<Texture2D> _texture;

	private Asset<Texture2D> _middleTexture;

	private Asset<Texture2D> _backPanelHighlightTexture;

	private Asset<Texture2D> _backPanelBorderTexture;

	private Color _color;

	private float _visibilityActive = 1f;

	private float _visibilityInactive = 0.4f;

	private bool _selected;

	private bool _hovered;

	public UIColoredImageButton(Asset<Texture2D> texture, bool isSmall = false)
	{
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		_color = Color.White;
		_texture = texture;
		if (isSmall)
		{
			_backPanelTexture = Main.Assets.Request<Texture2D>("Images/UI/CharCreation/SmallPanel", (AssetRequestMode)1);
		}
		else
		{
			_backPanelTexture = Main.Assets.Request<Texture2D>("Images/UI/CharCreation/CategoryPanel", (AssetRequestMode)1);
		}
		Width.Set(_backPanelTexture.Width(), 0f);
		Height.Set(_backPanelTexture.Height(), 0f);
		_backPanelHighlightTexture = Main.Assets.Request<Texture2D>("Images/UI/CharCreation/CategoryPanelHighlight", (AssetRequestMode)1);
		if (isSmall)
		{
			_backPanelBorderTexture = Main.Assets.Request<Texture2D>("Images/UI/CharCreation/SmallPanelBorder", (AssetRequestMode)1);
		}
		else
		{
			_backPanelBorderTexture = Main.Assets.Request<Texture2D>("Images/UI/CharCreation/CategoryPanelBorder", (AssetRequestMode)1);
		}
	}

	public void SetImage(Asset<Texture2D> texture)
	{
		_texture = texture;
		Width.Set(_texture.Width(), 0f);
		Height.Set(_texture.Height(), 0f);
	}

	public void SetImageWithoutSettingSize(Asset<Texture2D> texture)
	{
		_texture = texture;
	}

	protected override void DrawSelf(SpriteBatch spriteBatch)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0100: Unknown result type (might be due to invalid IL or missing references)
		//IL_0110: Unknown result type (might be due to invalid IL or missing references)
		//IL_011a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0143: Unknown result type (might be due to invalid IL or missing references)
		//IL_014d: Unknown result type (might be due to invalid IL or missing references)
		//IL_015d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0167: Unknown result type (might be due to invalid IL or missing references)
		//IL_0190: Unknown result type (might be due to invalid IL or missing references)
		//IL_019b: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b5: Unknown result type (might be due to invalid IL or missing references)
		CalculatedStyle dimensions = GetDimensions();
		Vector2 val = dimensions.Position() + new Vector2(dimensions.Width, dimensions.Height) / 2f;
		spriteBatch.Draw(_backPanelTexture.Value, val, (Rectangle?)null, Color.White * (IsMouseHovering ? _visibilityActive : _visibilityInactive), 0f, _backPanelTexture.Size() / 2f, 1f, (SpriteEffects)0, 0f);
		_ = Color.White;
		if (_hovered)
		{
			spriteBatch.Draw(_backPanelBorderTexture.Value, val, (Rectangle?)null, Color.White, 0f, _backPanelBorderTexture.Size() / 2f, 1f, (SpriteEffects)0, 0f);
		}
		if (_selected)
		{
			spriteBatch.Draw(_backPanelHighlightTexture.Value, val, (Rectangle?)null, Color.White, 0f, _backPanelHighlightTexture.Size() / 2f, 1f, (SpriteEffects)0, 0f);
		}
		if (_middleTexture != null)
		{
			spriteBatch.Draw(_middleTexture.Value, val, (Rectangle?)null, Color.White, 0f, _middleTexture.Size() / 2f, 1f, (SpriteEffects)0, 0f);
		}
		if (_texture != null)
		{
			spriteBatch.Draw(_texture.Value, val, (Rectangle?)null, _color, 0f, _texture.Size() / 2f, 1f, (SpriteEffects)0, 0f);
		}
	}

	public override void MouseOver(UIMouseEvent evt)
	{
		base.MouseOver(evt);
		SoundEngine.PlaySound(12);
		_hovered = true;
	}

	public void SetVisibility(float whenActive, float whenInactive)
	{
		_visibilityActive = MathHelper.Clamp(whenActive, 0f, 1f);
		_visibilityInactive = MathHelper.Clamp(whenInactive, 0f, 1f);
	}

	public void SetColor(Color color)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		_color = color;
	}

	public void SetMiddleTexture(Asset<Texture2D> texAsset)
	{
		_middleTexture = texAsset;
	}

	public void SetSelected(bool selected)
	{
		_selected = selected;
	}

	public override void MouseOut(UIMouseEvent evt)
	{
		base.MouseOut(evt);
		_hovered = false;
	}
}
