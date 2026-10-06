using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;

namespace Terraria.GameContent;

public class PlayerRainbowWingsTextureContent : ARenderTargetContentByRequest
{
	protected override void HandleUseRequest(GraphicsDevice device, SpriteBatch spriteBatch)
	{
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		Asset<Texture2D> val = TextureAssets.Extra[171];
		PrepareARenderTarget_AndListenToEvents(ref _target, device, val.Width(), val.Height(), (RenderTargetUsage)1);
		device.SetRenderTarget(_target);
		device.Clear(Color.Transparent);
		DrawData value = new DrawData(val.Value, Vector2.Zero, Color.White);
		spriteBatch.Begin((SpriteSortMode)1, BlendState.AlphaBlend);
		GameShaders.Misc["HallowBoss"].Apply(value);
		value.Draw(spriteBatch);
		spriteBatch.End();
		device.SetRenderTarget((RenderTarget2D)null);
		_wasPrepared = true;
	}
}
