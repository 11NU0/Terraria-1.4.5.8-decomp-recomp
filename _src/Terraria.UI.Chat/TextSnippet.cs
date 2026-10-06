using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Terraria.UI.Chat;

public class TextSnippet
{
	public string Text;

	public string TextOriginal;

	public Color Color = Color.White;

	public bool CheckForHover;

	public bool DeleteWhole;

	public bool UseRawColor;

	public TextSnippet(string text = "")
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		Text = text;
		TextOriginal = text;
	}

	public TextSnippet(string text, Color color)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		Text = text;
		TextOriginal = text;
		Color = color;
	}

	public virtual void OnHover()
	{
	}

	public virtual void OnClick()
	{
	}

	public virtual Color GetVisibleColor()
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		if (UseRawColor)
		{
			return Color;
		}
		return ChatManager.WaveColor(Color);
	}

	public virtual bool UniqueDraw(bool justCheckingSize, out Vector2 size, SpriteBatch spriteBatch, Vector2 position = default(Vector2), Color color = default(Color), float scale = 1f)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		size = Vector2.Zero;
		return false;
	}

	public virtual TextSnippet CopyMorph(string newText)
	{
		TextSnippet textSnippet = (TextSnippet)MemberwiseClone();
		textSnippet.Text = newText;
		return textSnippet;
	}

	public override string ToString()
	{
		return "Text: " + Text + " | OriginalText: " + TextOriginal;
	}
}
