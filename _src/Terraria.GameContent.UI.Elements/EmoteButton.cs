using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.Audio;
using Terraria.Localization;
using Terraria.UI;

namespace Terraria.GameContent.UI.Elements;

public class EmoteButton : UIElement
{
	private Asset<Texture2D> _texture;

	private Asset<Texture2D> _textureBorder;

	private int _emoteIndex;

	private bool _hovered;

	private int _frameCounter;

	public EmoteButton(int emoteIndex)
	{
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		_texture = Main.Assets.Request<Texture2D>("Images/Extra_" + (short)48, (AssetRequestMode)1);
		_textureBorder = Main.Assets.Request<Texture2D>("Images/UI/EmoteBubbleBorder", (AssetRequestMode)1);
		_emoteIndex = emoteIndex;
		Rectangle frame = GetFrame();
		Width.Set(frame.Width, 0f);
		Height.Set(frame.Height, 0f);
	}

	private Rectangle GetFrame()
	{
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		int num = ((_frameCounter >= 10) ? 1 : 0);
		return _texture.Frame(8, EmoteBubble.EMOTE_SHEET_VERTICAL_FRAMES, _emoteIndex % 4 * 2 + num, _emoteIndex / 4 + 1);
	}

	private void UpdateFrame()
	{
		if (++_frameCounter >= 20)
		{
			_frameCounter = 0;
		}
	}

	public override void Update(GameTime gameTime)
	{
		UpdateFrame();
		base.Update(gameTime);
	}

	protected override void DrawSelf(SpriteBatch spriteBatch)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0106: Unknown result type (might be due to invalid IL or missing references)
		//IL_010d: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		CalculatedStyle dimensions = GetDimensions();
		Vector2 val = dimensions.Position() + new Vector2(dimensions.Width, dimensions.Height) / 2f;
		Rectangle frame = GetFrame();
		Rectangle value = frame;
		value.X = _texture.Width() / 8;
		value.Y = 0;
		Vector2 val2 = frame.Size() / 2f;
		Color white = Color.White;
		Color val3 = Color.Black;
		if (_hovered)
		{
			val3 = Main.OurFavoriteColor;
		}
		spriteBatch.Draw(_texture.Value, val, (Rectangle?)value, white, 0f, val2, 1f, (SpriteEffects)0, 0f);
		spriteBatch.Draw(_texture.Value, val, (Rectangle?)frame, white, 0f, val2, 1f, (SpriteEffects)0, 0f);
		spriteBatch.Draw(_textureBorder.Value, val - Vector2.One * 2f, (Rectangle?)null, val3, 0f, val2, 1f, (SpriteEffects)0, 0f);
		if (_hovered)
		{
			string name = EmoteID.Search.GetName(_emoteIndex);
			string cursorText = "/" + Language.GetTextValue("EmojiName." + name);
			Main.instance.MouseText(cursorText, 0, 0);
		}
	}

	public override void MouseOver(UIMouseEvent evt)
	{
		base.MouseOver(evt);
		SoundEngine.PlaySound(12);
		_hovered = true;
	}

	public override void MouseOut(UIMouseEvent evt)
	{
		base.MouseOut(evt);
		_hovered = false;
	}

	public override void LeftClick(UIMouseEvent evt)
	{
		base.LeftClick(evt);
		EmoteBubble.MakeLocalPlayerEmote(_emoteIndex);
		IngameFancyUI.Close();
	}
}
