using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;

namespace Terraria.GameContent;

public class PlayerTitaniumStormBuffTextureContent : ARenderTargetContentByRequest
{
	private MiscShaderData _shaderData;

	public PlayerTitaniumStormBuffTextureContent()
	{
		_shaderData = new MiscShaderData(Main.PixelShaderRef, "TitaniumStorm");
		_shaderData.UseImage1("Images/Extra_" + (short)156);
	}

	protected override void HandleUseRequest(GraphicsDevice device, SpriteBatch spriteBatch)
	{
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		Main.instance.LoadProjectile(908);
		Asset<Texture2D> val = TextureAssets.Projectile[908];
		UpdateSettingsForRendering(0.6f, 0f, Main.GlobalTimeWrappedHourly, 0.3f);
		PrepareARenderTarget_AndListenToEvents(ref _target, device, val.Width(), val.Height(), (RenderTargetUsage)1);
		device.SetRenderTarget(_target);
		device.Clear(Color.Transparent);
		DrawData value = new DrawData(val.Value, Vector2.Zero, Color.White);
		spriteBatch.Begin((SpriteSortMode)1, BlendState.AlphaBlend);
		_shaderData.Apply(value);
		value.Draw(spriteBatch);
		spriteBatch.End();
		device.SetRenderTarget((RenderTarget2D)null);
		_wasPrepared = true;
	}

	public void UpdateSettingsForRendering(float gradientContributionFromOriginalTexture, float gradientScrollingSpeed, float flatGradientOffset, float gradientColorDominance)
	{
		_shaderData.UseColor(gradientScrollingSpeed, gradientContributionFromOriginalTexture, gradientColorDominance);
		_shaderData.UseOpacity(flatGradientOffset);
	}
}
