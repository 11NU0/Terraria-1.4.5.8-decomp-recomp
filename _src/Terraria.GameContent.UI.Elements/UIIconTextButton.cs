using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.Audio;
using Terraria.ID;
using Terraria.Localization;
using Terraria.UI;

namespace Terraria.GameContent.UI.Elements;

public class UIIconTextButton : UIElement
{
	private readonly Asset<Texture2D> _BasePanelTexture;

	private readonly Asset<Texture2D> _hoveredTexture;

	private readonly Asset<Texture2D> _iconTexture;

	private Color _color;

	private Color _hoverColor;

	public float FadeFromBlack = 1f;

	private float _whiteLerp = 0.7f;

	private float _opacity = 0.7f;

	private bool _hovered;

	private bool _soundedHover;

	private UIText _title;

	public UIIconTextButton(LocalizedText title, Color textColor, string iconTexturePath, float textSize = 1f, float titleAlignmentX = 0.5f, float titleWidthReduction = 10f)
	{
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		Width = StyleDimension.FromPixels(44f);
		Height = StyleDimension.FromPixels(34f);
		_hoverColor = Color.White;
		_BasePanelTexture = Main.Assets.Request<Texture2D>("Images/UI/CharCreation/PanelGrayscale", (AssetRequestMode)1);
		_hoveredTexture = Main.Assets.Request<Texture2D>("Images/UI/CharCreation/CategoryPanelHighlight", (AssetRequestMode)1);
		if (iconTexturePath != null)
		{
			_iconTexture = Main.Assets.Request<Texture2D>(iconTexturePath, (AssetRequestMode)1);
		}
		SetColor(Color.Lerp(Color.Black, Colors.InventoryDefaultColor, FadeFromBlack), 1f);
		if (title != null)
		{
			SetText(title, textSize, textColor);
		}
	}

	public void SetText(LocalizedText text, float textSize, Color color)
	{
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		if (_title != null)
		{
			_title.Remove();
		}
		UIText uIText = new UIText(text, textSize)
		{
			HAlign = 0f,
			VAlign = 0.5f,
			Top = StyleDimension.FromPixels(0f),
			Left = StyleDimension.FromPixelsAndPercent(10f, 0f),
			IgnoresMouseInteraction = true
		};
		uIText.TextColor = color;
		Append(uIText);
		_title = uIText;
		if (_iconTexture != null)
		{
			Width.Set(_title.GetDimensions().Width + (float)_iconTexture.Width() + 26f, 0f);
			Height.Set(Math.Max(_title.GetDimensions().Height, _iconTexture.Height()) + 16f, 0f);
		}
	}

	protected override void DrawSelf(SpriteBatch spriteBatch)
	{
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0102: Unknown result type (might be due to invalid IL or missing references)
		if (_hovered)
		{
			if (!_soundedHover)
			{
				SoundEngine.PlaySound(12);
			}
			_soundedHover = true;
		}
		else
		{
			_soundedHover = false;
		}
		CalculatedStyle dimensions = GetDimensions();
		Color color = _color;
		float opacity = _opacity;
		Utils.DrawSplicedPanel(spriteBatch, _BasePanelTexture.Value, (int)dimensions.X, (int)dimensions.Y, (int)dimensions.Width, (int)dimensions.Height, 10, 10, 10, 10, Color.Lerp(Color.Black, color, FadeFromBlack) * opacity);
		if (_iconTexture != null)
		{
			Color val = Color.Lerp(color, Color.White, _whiteLerp) * opacity;
			spriteBatch.Draw(_iconTexture.Value, new Vector2(dimensions.X + dimensions.Width - (float)_iconTexture.Width() - 5f, dimensions.Center().Y - (float)(_iconTexture.Height() / 2)), val);
		}
	}

	public override void LeftMouseDown(UIMouseEvent evt)
	{
		SoundEngine.PlaySound(12);
		base.LeftMouseDown(evt);
	}

	public override void MouseOver(UIMouseEvent evt)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		base.MouseOver(evt);
		SetColor(Color.Lerp(Colors.InventoryDefaultColor, Color.White, _whiteLerp), 0.7f);
		_hovered = true;
	}

	public override void MouseOut(UIMouseEvent evt)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		base.MouseOut(evt);
		SetColor(Color.Lerp(Color.Black, Colors.InventoryDefaultColor, FadeFromBlack), 1f);
		_hovered = false;
	}

	public void SetColor(Color color, float opacity)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		_color = color;
		_opacity = opacity;
	}

	public void SetHoverColor(Color color)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		_hoverColor = color;
	}
}
