using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.GameContent;

namespace Terraria.Graphics.Renderers;

public class OutlinedTextureRenderer : INeedRenderTargetContent
{
	private OutlinedDrawRenderTargetContent[] _contents;

	private Asset<Texture2D>[] _matchingArray;

	public bool IsReady => false;

	public OutlinedTextureRenderer(Asset<Texture2D>[] matchingArray)
	{
		_matchingArray = matchingArray;
		Reset();
	}

	public void Reset()
	{
		_contents = new OutlinedDrawRenderTargetContent[_matchingArray.Length];
	}

	public void DrawWithOutlines(int textureIndex, Vector2 position, Color color, float rotation, float scale, SpriteEffects effects)
	{
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		if (_contents[textureIndex] == null)
		{
			_contents[textureIndex] = new OutlinedDrawRenderTargetContent();
			_contents[textureIndex].SetTexture(_matchingArray[textureIndex].Value);
		}
		OutlinedDrawRenderTargetContent outlinedDrawRenderTargetContent = _contents[textureIndex];
		if (outlinedDrawRenderTargetContent.IsReady)
		{
			RenderTarget2D target = outlinedDrawRenderTargetContent.GetTarget();
			Main.spriteBatch.Draw((Texture2D)(object)target, position, (Rectangle?)null, color, rotation, ((Texture2D)(object)target).Size() / 2f, scale, effects, 0f);
		}
		else
		{
			outlinedDrawRenderTargetContent.Request();
		}
	}

	public bool RequestAndTryGet(int textureIndex, out RenderTarget2D renderTarget)
	{
		renderTarget = null;
		if (_contents[textureIndex] == null)
		{
			_contents[textureIndex] = new OutlinedDrawRenderTargetContent();
			_contents[textureIndex].SetTexture(_matchingArray[textureIndex].Value);
		}
		OutlinedDrawRenderTargetContent outlinedDrawRenderTargetContent = _contents[textureIndex];
		if (!outlinedDrawRenderTargetContent.IsReady)
		{
			outlinedDrawRenderTargetContent.Request();
			return false;
		}
		renderTarget = outlinedDrawRenderTargetContent.GetTarget();
		return true;
	}

	public void PrepareRenderTarget(GraphicsDevice device, SpriteBatch spriteBatch)
	{
		for (int i = 0; i < _contents.Length; i++)
		{
			if (_contents[i] != null && !_contents[i].IsReady)
			{
				_contents[i].PrepareRenderTarget(device, spriteBatch);
			}
		}
	}
}
