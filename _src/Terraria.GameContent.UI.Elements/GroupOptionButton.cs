using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.Audio;
using Terraria.ID;
using Terraria.Localization;
using Terraria.UI;

namespace Terraria.GameContent.UI.Elements;

public class GroupOptionButton<T> : UIElement, IGroupOptionButton
{
	private T _currentOption;

	private readonly Asset<Texture2D> _BasePanelTexture;

	private readonly Asset<Texture2D> _selectedBorderTexture;

	private readonly Asset<Texture2D> _hoveredBorderTexture;

	private Asset<Texture2D> _iconTexture;

	private readonly T _myOption;

	private Color _color;

	private Color _borderColor;

	public float FadeFromBlack = 1f;

	public int InnerHighlightRim = 7;

	private float _whiteLerp = 0.7f;

	private float _opacity = 0.7f;

	private bool _hovered;

	private bool _soundedHover;

	public bool ShowHighlightWhenSelected = true;

	private bool _UseOverrideColors;

	private Color _overrideUnpickedColor = Color.White;

	private Color _overridePickedColor = Color.White;

	private float _overrideOpacityPicked;

	private float _overrideOpacityUnpicked;

	public readonly LocalizedText Description;

	private UIText _title;

	private float _iconScale = 1f;

	private Vector2 _iconOffset;

	private Rectangle? _iconFrame;

	private Color _iconColor = Color.White;

	public T OptionValue => _myOption;

	public bool IsSelected => EqualityComparer<T>.Default.Equals(_currentOption, _myOption);

	public Texture2D Icon
	{
		get
		{
			if (_iconTexture == null)
			{
				return null;
			}
			return _iconTexture.Value;
		}
	}

	public float IconScale
	{
		get
		{
			return _iconScale;
		}
		set
		{
			_iconScale = value;
		}
	}

	public Vector2 IconOffset
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return _iconOffset;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			_iconOffset = value;
		}
	}

	public Color IconColor
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return _iconColor;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			_iconColor = value;
		}
	}

	public GroupOptionButton(T option, LocalizedText title, LocalizedText description, Color textColor, string iconTexturePath, float textSize = 1f, float titleAlignmentX = 0.5f, float titleWidthReduction = 10f)
	{
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0100: Unknown result type (might be due to invalid IL or missing references)
		//IL_0149: Unknown result type (might be due to invalid IL or missing references)
		_borderColor = Color.White;
		_currentOption = option;
		_myOption = option;
		Description = description;
		Width = StyleDimension.FromPixels(44f);
		Height = StyleDimension.FromPixels(34f);
		_BasePanelTexture = Main.Assets.Request<Texture2D>("Images/UI/CharCreation/PanelGrayscale", (AssetRequestMode)1);
		_selectedBorderTexture = Main.Assets.Request<Texture2D>("Images/UI/CharCreation/CategoryPanelHighlight", (AssetRequestMode)1);
		_hoveredBorderTexture = Main.Assets.Request<Texture2D>("Images/UI/CharCreation/CategoryPanelBorder", (AssetRequestMode)1);
		if (iconTexturePath != null)
		{
			_iconTexture = Main.Assets.Request<Texture2D>(iconTexturePath, (AssetRequestMode)1);
		}
		_color = Colors.InventoryDefaultColor;
		if (title != null)
		{
			UIText uIText = new UIText(title, textSize)
			{
				HAlign = titleAlignmentX,
				VAlign = 0.5f,
				Width = StyleDimension.FromPixelsAndPercent(0f - titleWidthReduction, 1f),
				Top = StyleDimension.FromPixels(0f)
			};
			uIText.TextColor = textColor;
			Append(uIText);
			_title = uIText;
		}
	}

	public void SetText(LocalizedText text, float textSize, Color color)
	{
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		if (_title != null)
		{
			_title.Remove();
		}
		UIText uIText = new UIText(text, textSize)
		{
			HAlign = 0.5f,
			VAlign = 0.5f,
			Width = StyleDimension.FromPixelsAndPercent(-10f, 1f),
			Top = StyleDimension.FromPixels(0f)
		};
		uIText.TextColor = color;
		Append(uIText);
		_title = uIText;
	}

	public void SetTextWithoutLocalization(string text, float textSize, Color color, float hAlign, float left)
	{
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		if (_title != null)
		{
			_title.Remove();
		}
		UIText uIText = new UIText(text, textSize)
		{
			HAlign = 0.5f,
			VAlign = 0.5f,
			Width = StyleDimension.FromPixelsAndPercent(-10f, 1f),
			Top = StyleDimension.FromPixels(0f),
			IgnoresMouseInteraction = true
		};
		uIText.TextOriginX = hAlign;
		uIText.Left.Pixels = left;
		uIText.TextColor = color;
		Append(uIText);
		_title = uIText;
	}

	public void SetCurrentOption(T option)
	{
		_currentOption = option;
	}

	public void SetIcon(string iconTexturePath)
	{
		if (iconTexturePath != null)
		{
			_iconTexture = Main.Assets.Request<Texture2D>(iconTexturePath, (AssetRequestMode)1);
		}
		else
		{
			_iconTexture = null;
		}
	}

	public void SetIconFrame(Rectangle region)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		_iconFrame = region;
	}

	protected override void DrawSelf(SpriteBatch spriteBatch)
	{
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0179: Unknown result type (might be due to invalid IL or missing references)
		//IL_0124: Unknown result type (might be due to invalid IL or missing references)
		//IL_0125: Unknown result type (might be due to invalid IL or missing references)
		//IL_0130: Unknown result type (might be due to invalid IL or missing references)
		//IL_0136: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_018f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0194: Unknown result type (might be due to invalid IL or missing references)
		//IL_01db: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b5: Unknown result type (might be due to invalid IL or missing references)
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
		Color val = _color;
		float num = _opacity;
		bool isSelected = IsSelected;
		if (_UseOverrideColors)
		{
			val = (isSelected ? _overridePickedColor : _overrideUnpickedColor);
			num = (isSelected ? _overrideOpacityPicked : _overrideOpacityUnpicked);
		}
		Utils.DrawSplicedPanel(spriteBatch, _BasePanelTexture.Value, (int)dimensions.X, (int)dimensions.Y, (int)dimensions.Width, (int)dimensions.Height, 10, 10, 10, 10, Color.Lerp(Color.Black, val, FadeFromBlack) * num);
		if (isSelected && ShowHighlightWhenSelected)
		{
			Utils.DrawSplicedPanel(spriteBatch, _selectedBorderTexture.Value, (int)dimensions.X + InnerHighlightRim, (int)dimensions.Y + InnerHighlightRim, (int)dimensions.Width - InnerHighlightRim * 2, (int)dimensions.Height - InnerHighlightRim * 2, 10, 10, 10, 10, Color.Lerp(val, Color.White, _whiteLerp) * num);
		}
		if (_hovered)
		{
			Utils.DrawSplicedPanel(spriteBatch, _hoveredBorderTexture.Value, (int)dimensions.X, (int)dimensions.Y, (int)dimensions.Width, (int)dimensions.Height, 10, 10, 10, 10, _borderColor);
		}
		if (_iconTexture != null)
		{
			Color val2 = IconColor;
			if (!_hovered && !isSelected)
			{
				val2 = Color.Lerp(val, val2, _whiteLerp) * num;
			}
			spriteBatch.Draw(_iconTexture.Value, new Vector2(dimensions.X + 1f, dimensions.Y + 1f) + _iconOffset, _iconFrame, val2, 0f, Vector2.Zero, _iconScale, (SpriteEffects)0, 0f);
		}
	}

	public override void LeftMouseDown(UIMouseEvent evt)
	{
		SoundEngine.PlaySound(12);
		base.LeftMouseDown(evt);
	}

	public override void MouseOver(UIMouseEvent evt)
	{
		base.MouseOver(evt);
		_hovered = true;
	}

	public override void MouseOut(UIMouseEvent evt)
	{
		base.MouseOut(evt);
		_hovered = false;
	}

	public void SetColor(Color color, float opacity)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		_color = color;
		_opacity = opacity;
	}

	public void SetColorsBasedOnSelectionState(Color pickedColor, Color unpickedColor, float opacityPicked, float opacityNotPicked)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		_UseOverrideColors = true;
		_overridePickedColor = pickedColor;
		_overrideUnpickedColor = unpickedColor;
		_overrideOpacityPicked = opacityPicked;
		_overrideOpacityUnpicked = opacityNotPicked;
	}

	public void SetBorderColor(Color color)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		_borderColor = color;
	}
}
