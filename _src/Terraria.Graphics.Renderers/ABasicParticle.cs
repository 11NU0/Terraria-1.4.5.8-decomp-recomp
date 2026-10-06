using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;

namespace Terraria.Graphics.Renderers;

public abstract class ABasicParticle : IPooledParticle, IParticle
{
	public Vector2 AccelerationPerFrame;

	public Vector2 Velocity;

	public Vector2 LocalPosition;

	protected Asset<Texture2D> _texture;

	protected Rectangle _frame;

	protected Vector2 _origin;

	public float Rotation;

	public float RotationVelocity;

	public float RotationAcceleration;

	public Vector2 Scale;

	public Vector2 ScaleVelocity;

	public Vector2 ScaleAcceleration;

	public bool ShouldBeRemovedFromRenderer { get; protected set; }

	public bool IsRestingInPool { get; private set; }

	public ABasicParticle()
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		_texture = null;
		_frame = Rectangle.Empty;
		_origin = Vector2.Zero;
		Velocity = Vector2.Zero;
		LocalPosition = Vector2.Zero;
		ShouldBeRemovedFromRenderer = false;
	}

	public virtual void SetBasicInfo(Asset<Texture2D> textureAsset, Rectangle? frame, Vector2 initialVelocity, Vector2 initialLocalPosition)
	{
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		_texture = textureAsset;
		_frame = (frame.HasValue ? frame.Value : _texture.Frame());
		_origin = _frame.Size() / 2f;
		Velocity = initialVelocity;
		LocalPosition = initialLocalPosition;
		ShouldBeRemovedFromRenderer = false;
	}

	public virtual void Update(ref ParticleRendererSettings settings)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		Velocity += AccelerationPerFrame;
		LocalPosition += Velocity;
		RotationVelocity += RotationAcceleration;
		Rotation += RotationVelocity;
		ScaleVelocity += ScaleAcceleration;
		Scale += ScaleVelocity;
	}

	public abstract void Draw(ref ParticleRendererSettings settings, SpriteBatch spritebatch);

	public void RestInPool()
	{
		IsRestingInPool = true;
	}

	public virtual void FetchFromPool()
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		IsRestingInPool = false;
		ShouldBeRemovedFromRenderer = false;
		AccelerationPerFrame = Vector2.Zero;
		Velocity = Vector2.Zero;
		LocalPosition = Vector2.Zero;
		_texture = null;
		_frame = Rectangle.Empty;
		_origin = Vector2.Zero;
		Rotation = 0f;
		RotationVelocity = 0f;
		RotationAcceleration = 0f;
		Scale = Vector2.Zero;
		ScaleVelocity = Vector2.Zero;
		ScaleAcceleration = Vector2.Zero;
	}
}
