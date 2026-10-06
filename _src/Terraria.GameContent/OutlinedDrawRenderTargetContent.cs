using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Terraria.GameContent;

public class OutlinedDrawRenderTargetContent : AnOutlinedDrawRenderTargetContent
{
	private Texture2D _theTexture;

	public void SetTexture(Texture2D texture)
	{
		if (_theTexture != texture)
		{
			_theTexture = texture;
			_wasPrepared = false;
			width = texture.Width + 8;
			height = texture.Height + 8;
		}
	}

	internal override void DrawTheContent(SpriteBatch spriteBatch)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		spriteBatch.Draw(_theTexture, new Vector2(4f, 4f), (Rectangle?)null, Color.White, 0f, Vector2.Zero, 1f, (SpriteEffects)0, 0f);
	}
}
