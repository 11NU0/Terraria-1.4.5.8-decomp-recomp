using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.Graphics.Shaders;

namespace Terraria.Graphics.Effects;

public class SimpleOverlay : Overlay
{
	private Asset<Texture2D> _texture;

	private ScreenShaderData _shader;

	public Vector2 TargetPosition = Vector2.Zero;

	public SimpleOverlay(string textureName, ScreenShaderData shader, EffectPriority priority = EffectPriority.VeryLow, RenderLayers layer = RenderLayers.All)
		: base(priority, layer)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		_texture = Main.Assets.Request<Texture2D>((textureName == null) ? "" : textureName, (AssetRequestMode)1);
		_shader = shader;
	}

	public SimpleOverlay(string textureName, string shaderName = "Default", EffectPriority priority = EffectPriority.VeryLow, RenderLayers layer = RenderLayers.All)
		: base(priority, layer)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		_texture = Main.Assets.Request<Texture2D>((textureName == null) ? "" : textureName, (AssetRequestMode)1);
		_shader = new ScreenShaderData(Main.ScreenShaderRef, shaderName);
	}

	public ScreenShaderData GetShader()
	{
		return _shader;
	}

	public override void Draw(SpriteBatch spriteBatch)
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		_shader.UseGlobalOpacity(Opacity);
		_shader.UseTargetPosition(TargetPosition);
		_shader.Apply();
		spriteBatch.Draw(_texture.Value, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), Main.ColorOfTheSkies);
	}

	public override void Update(GameTime gameTime)
	{
		_shader.Update(gameTime);
	}

	public override void Activate(Vector2 position, params object[] args)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		TargetPosition = position;
		Mode = OverlayMode.FadeIn;
	}

	public override void Deactivate(params object[] args)
	{
		Mode = OverlayMode.FadeOut;
	}

	public override bool IsVisible()
	{
		return _shader.CombinedOpacity > 0f;
	}
}
