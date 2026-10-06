using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.Audio;
using Terraria.UI;

namespace Terraria.GameContent.UI.Elements;

public class UIClothStyleButton : UIElement
{
	private readonly Player _player;

	public readonly int ClothStyleId;

	private readonly Asset<Texture2D> _BasePanelTexture;

	private readonly Asset<Texture2D> _selectedBorderTexture;

	private readonly Asset<Texture2D> _hoveredBorderTexture;

	private readonly UICharacter _char;

	private bool _hovered;

	private bool _soundedHover;

	private int _realSkinVariant;

	private Action PrepareAction;

	public UIClothStyleButton(Player player, int clothStyleId, Action prepareAction = null)
	{
		_player = player;
		ClothStyleId = clothStyleId;
		PrepareAction = prepareAction;
		Width = StyleDimension.FromPixels(44f);
		Height = StyleDimension.FromPixels(80f);
		_BasePanelTexture = Main.Assets.Request<Texture2D>("Images/UI/CharCreation/CategoryPanel", (AssetRequestMode)1);
		_selectedBorderTexture = Main.Assets.Request<Texture2D>("Images/UI/CharCreation/CategoryPanelHighlight", (AssetRequestMode)1);
		_hoveredBorderTexture = Main.Assets.Request<Texture2D>("Images/UI/CharCreation/CategoryPanelBorder", (AssetRequestMode)1);
		_char = new UICharacter(_player, animated: false, hasBackPanel: false)
		{
			HAlign = 0.5f,
			VAlign = 0.5f
		};
		Append(_char);
	}

	public override void Draw(SpriteBatch spriteBatch)
	{
		_realSkinVariant = _player.skinVariant;
		_player.skinVariant = ClothStyleId;
		int hair = _player.hair;
		if (PrepareAction != null)
		{
			PrepareAction();
		}
		_player.PlayerFrame();
		base.Draw(spriteBatch);
		_player.skinVariant = _realSkinVariant;
		_player.hair = hair;
	}

	protected override void DrawSelf(SpriteBatch spriteBatch)
	{
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0108: Unknown result type (might be due to invalid IL or missing references)
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
		if (_realSkinVariant == ClothStyleId)
		{
			Utils.DrawSplicedPanel(spriteBatch, _selectedBorderTexture.Value, (int)dimensions.X + 3, (int)dimensions.Y + 3, (int)dimensions.Width - 6, (int)dimensions.Height - 6, 10, 10, 10, 10, Color.White);
		}
		if (_hovered)
		{
			Utils.DrawSplicedPanel(spriteBatch, _hoveredBorderTexture.Value, (int)dimensions.X, (int)dimensions.Y, (int)dimensions.Width, (int)dimensions.Height, 10, 10, 10, 10, Color.White);
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
		_char.SetAnimated(animated: true);
	}

	public override void MouseOut(UIMouseEvent evt)
	{
		base.MouseOut(evt);
		_hovered = false;
		_char.SetAnimated(animated: false);
	}
}
