using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;

namespace Terraria.Graphics.Renderers;

public class CreativeSacrificeParticle : IParticle
{
	public Vector2 AccelerationPerFrame;

	public Vector2 Velocity;

	public Vector2 LocalPosition;

	public float ScaleOffsetPerFrame;

	public float StopWhenBelowXScale;

	private Asset<Texture2D> _texture;

	private Rectangle _frame;

	private Vector2 _origin;

	private float _scale;

	public bool ShouldBeRemovedFromRenderer { get; private set; }

	public CreativeSacrificeParticle(Asset<Texture2D> textureAsset, Rectangle? frame, Vector2 initialVelocity, Vector2 initialLocalPosition)
	{
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		_texture = textureAsset;
		_frame = (frame.HasValue ? frame.Value : _texture.Frame());
		_origin = _frame.Size() / 2f;
		Velocity = initialVelocity;
		LocalPosition = initialLocalPosition;
		StopWhenBelowXScale = 0f;
		ShouldBeRemovedFromRenderer = false;
		_scale = 0.6f;
	}

	public void Update(ref ParticleRendererSettings settings)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		Velocity += AccelerationPerFrame;
		LocalPosition += Velocity;
		_scale += ScaleOffsetPerFrame;
		if (_scale <= StopWhenBelowXScale)
		{
			ShouldBeRemovedFromRenderer = true;
		}
	}

	public void Draw(ref ParticleRendererSettings settings, SpriteBatch spritebatch)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		Color val = Color.Lerp(Color.White, new Color(255, 255, 255, 0), Utils.GetLerpValue(0.1f, 0.5f, _scale));
		spritebatch.Draw(_texture.Value, settings.AnchorPosition + LocalPosition, (Rectangle?)_frame, val, 0f, _origin, _scale, (SpriteEffects)0, 0f);
	}
}
