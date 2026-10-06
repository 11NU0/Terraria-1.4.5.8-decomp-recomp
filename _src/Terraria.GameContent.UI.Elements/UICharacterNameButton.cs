using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.Audio;
using Terraria.Localization;
using Terraria.UI;

namespace Terraria.GameContent.UI.Elements;

public class UICharacterNameButton : UIElement
{
	private readonly Asset<Texture2D> _BasePanelTexture;

	private readonly Asset<Texture2D> _selectedBorderTexture;

	private readonly Asset<Texture2D> _hoveredBorderTexture;

	private bool _hovered;

	private bool _soundedHover;

	private readonly LocalizedText _textToShowWhenEmpty;

	private string actualContents;

	private UIText _text;

	private UIText _title;

	public readonly LocalizedText Description;

	public float DistanceFromTitleToOption = 20f;

	public UICharacterNameButton(LocalizedText titleText, LocalizedText emptyContentText, LocalizedText description = null)
	{
		Width = StyleDimension.FromPixels(400f);
		Height = StyleDimension.FromPixels(40f);
		Description = description;
		_BasePanelTexture = Main.Assets.Request<Texture2D>("Images/UI/CharCreation/CategoryPanel", (AssetRequestMode)1);
		_selectedBorderTexture = Main.Assets.Request<Texture2D>("Images/UI/CharCreation/CategoryPanelHighlight", (AssetRequestMode)1);
		_hoveredBorderTexture = Main.Assets.Request<Texture2D>("Images/UI/CharCreation/CategoryPanelBorder", (AssetRequestMode)1);
		_textToShowWhenEmpty = emptyContentText;
		float textScale = 1f;
		UIText uIText = new UIText(titleText, textScale)
		{
			HAlign = 0f,
			VAlign = 0.5f,
			Left = StyleDimension.FromPixels(10f)
		};
		Append(uIText);
		_title = uIText;
		UIText uIText2 = new UIText(Language.GetText("UI.PlayerNameSlot"), textScale)
		{
			HAlign = 0f,
			VAlign = 0.5f,
			Left = StyleDimension.FromPixels(150f)
		};
		Append(uIText2);
		_text = uIText2;
		SetContents(null);
	}

	protected override void DrawSelf(SpriteBatch spriteBatch)
	{
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
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
		Utils.DrawSplicedPanel(spriteBatch, _BasePanelTexture.Value, (int)dimensions.X, (int)dimensions.Y, (int)dimensions.Width, (int)dimensions.Height, 10, 10, 10, 10, Color.White * 0.5f);
		if (_hovered)
		{
			Utils.DrawSplicedPanel(spriteBatch, _hoveredBorderTexture.Value, (int)dimensions.X, (int)dimensions.Y, (int)dimensions.Width, (int)dimensions.Height, 10, 10, 10, 10, Color.White);
		}
	}

	public void SetContents(string name)
	{
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		actualContents = name;
		if (string.IsNullOrEmpty(actualContents))
		{
			_text.TextColor = Color.Gray;
			_text.SetText(_textToShowWhenEmpty);
		}
		else
		{
			_text.TextColor = Color.White;
			_text.SetText(actualContents);
		}
		_text.Left = StyleDimension.FromPixels(_title.GetInnerDimensions().Width + DistanceFromTitleToOption);
	}

	public CalculatedStyle GetTextDimensions()
	{
		return _text.GetDimensions();
	}

	public void TrimDisplayIfOverElementDimensions(int padding)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0135: Unknown result type (might be due to invalid IL or missing references)
		//IL_013c: Unknown result type (might be due to invalid IL or missing references)
		//IL_014c: Unknown result type (might be due to invalid IL or missing references)
		//IL_015c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0163: Unknown result type (might be due to invalid IL or missing references)
		//IL_016a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0171: Unknown result type (might be due to invalid IL or missing references)
		//IL_0178: Unknown result type (might be due to invalid IL or missing references)
		//IL_0180: Unknown result type (might be due to invalid IL or missing references)
		//IL_0187: Unknown result type (might be due to invalid IL or missing references)
		//IL_018f: Unknown result type (might be due to invalid IL or missing references)
		CalculatedStyle dimensions = GetDimensions();
		Point val = new Point((int)dimensions.X, (int)dimensions.Y);
		Point val2 = new Point(val.X + (int)dimensions.Width, val.Y + (int)dimensions.Height);
		Rectangle val3 = new Rectangle(val.X, val.Y, val2.X - val.X, val2.Y - val.Y);
		CalculatedStyle dimensions2 = _text.GetDimensions();
		Point val4 = new Point((int)dimensions2.X, (int)dimensions2.Y);
		Point val5 = new Point(val4.X + (int)dimensions2.Width, val4.Y + (int)dimensions2.Height);
		Rectangle val6 = new Rectangle(val4.X, val4.Y, val5.X - val4.X, val5.Y - val4.Y);
		bool flag = false;
		while (val6.Right > val3.Right - padding)
		{
			_text.SetText(Utils.TrimLastCharacter(_text.Text));
			flag = true;
			RecalculateChildren();
			dimensions2 = _text.GetDimensions();
			val4 = new Point((int)dimensions2.X, (int)dimensions2.Y);
			val5 = new Point(val4.X + (int)dimensions2.Width, val4.Y + (int)dimensions2.Height);
			val6 = new Rectangle(val4.X, val4.Y, val5.X - val4.X, val5.Y - val4.Y);
		}
		if (flag)
		{
			_text.SetText(Utils.TrimLastCharacter(_text.Text) + "…");
		}
	}

	public override void LeftMouseDown(UIMouseEvent evt)
	{
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
}
