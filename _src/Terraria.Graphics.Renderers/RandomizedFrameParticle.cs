using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Terraria.Graphics.Renderers;

public class RandomizedFrameParticle : ABasicParticle
{
	public float FadeInNormalizedTime;

	public float FadeOutNormalizedTime = 1f;

	public Color ColorTint = Color.White;

	public int AnimationFramesAmount;

	public int GameFramesPerAnimationFrame;

	private float _timeTolive;

	private float _timeSinceSpawn;

	private int _gameFramesCounted;

	public override void FetchFromPool()
	{
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		base.FetchFromPool();
		FadeInNormalizedTime = 0f;
		FadeOutNormalizedTime = 1f;
		ColorTint = Color.White;
		AnimationFramesAmount = 0;
		GameFramesPerAnimationFrame = 0;
		_timeTolive = 0f;
		_timeSinceSpawn = 0f;
		_gameFramesCounted = 0;
	}

	public void SetTypeInfo(int animationFramesAmount, int gameFramesPerAnimationFrame, float timeToLive)
	{
		_timeTolive = timeToLive;
		GameFramesPerAnimationFrame = gameFramesPerAnimationFrame;
		AnimationFramesAmount = animationFramesAmount;
		RandomizeFrame();
	}

	private void RandomizeFrame()
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		_frame = _texture.Frame(1, AnimationFramesAmount, 0, Main.rand.Next(AnimationFramesAmount));
		_origin = _frame.Size() / 2f;
	}

	public override void Update(ref ParticleRendererSettings settings)
	{
		base.Update(ref settings);
		_timeSinceSpawn++;
		if (_timeSinceSpawn >= _timeTolive)
		{
			ShouldBeRemovedFromRenderer = true;
		}
		if (++_gameFramesCounted >= GameFramesPerAnimationFrame)
		{
			_gameFramesCounted = 0;
			RandomizeFrame();
		}
	}

	public override void Draw(ref ParticleRendererSettings settings, SpriteBatch spritebatch)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		Color val = ColorTint * Utils.GetLerpValue(0f, FadeInNormalizedTime, _timeSinceSpawn / _timeTolive, clamped: true) * Utils.GetLerpValue(1f, FadeOutNormalizedTime, _timeSinceSpawn / _timeTolive, clamped: true);
		spritebatch.Draw(_texture.Value, settings.AnchorPosition + LocalPosition, (Rectangle?)_frame, val, Rotation, _origin, Scale, (SpriteEffects)0, 0f);
	}

	public RandomizedFrameParticle()
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
	}
}
