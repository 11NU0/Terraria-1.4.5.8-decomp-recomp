using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;

namespace Terraria.Graphics.Renderers;

public class GasParticle : ABasicParticle
{
	public float FadeInNormalizedTime = 0.25f;

	public float FadeOutNormalizedTime = 0.75f;

	public float TimeToLive = 80f;

	public Color ColorTint;

	public float Opacity;

	public float AdditiveAmount = 1f;

	public float FadeInEnd = 20f;

	public float FadeOutStart = 30f;

	public float FadeOutEnd = 45f;

	public float SlowdownScalar = 0.95f;

	private float _timeSinceSpawn;

	public Color LightColorTint;

	private int _internalIndentifier;

	public float InitialScale = 1f;

	public override void FetchFromPool()
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		base.FetchFromPool();
		ColorTint = Color.Transparent;
		_timeSinceSpawn = 0f;
		Opacity = 0f;
		FadeInNormalizedTime = 0.25f;
		FadeOutNormalizedTime = 0.75f;
		TimeToLive = 80f;
		_internalIndentifier = Main.rand.Next(255);
		SlowdownScalar = 0.95f;
		LightColorTint = Color.Transparent;
		InitialScale = 1f;
	}

	public override void Update(ref ParticleRendererSettings settings)
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0107: Unknown result type (might be due to invalid IL or missing references)
		//IL_0112: Unknown result type (might be due to invalid IL or missing references)
		//IL_0117: Unknown result type (might be due to invalid IL or missing references)
		//IL_0119: Unknown result type (might be due to invalid IL or missing references)
		base.Update(ref settings);
		_timeSinceSpawn++;
		float fromValue = _timeSinceSpawn / TimeToLive;
		Scale = Vector2.One * InitialScale * Utils.Remap(fromValue, 0f, 0.95f, 1f, 1.3f);
		Opacity = MathHelper.Clamp(Utils.Remap(fromValue, 0f, FadeInNormalizedTime, 0f, 1f) * Utils.Remap(fromValue, FadeOutNormalizedTime, 1f, 1f, 0f), 0f, 1f) * 0.85f;
		Rotation = (float)_internalIndentifier * 0.4002029f + _timeSinceSpawn * ((float)Math.PI * 2f) / 480f * 0.5f;
		Velocity *= SlowdownScalar;
		if (LightColorTint != Color.Transparent)
		{
			Color val = LightColorTint * Opacity;
			Lighting.AddLight(LocalPosition, (float)(int)val.R / 255f, (float)(int)val.G / 255f, (float)(int)val.B / 255f);
		}
		if (_timeSinceSpawn >= TimeToLive)
		{
			ShouldBeRemovedFromRenderer = true;
		}
	}

	public override void Draw(ref ParticleRendererSettings settings, SpriteBatch spritebatch)
	{
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
		Main.instance.LoadProjectile(1007);
		Texture2D value = TextureAssets.Projectile[1007].Value;
		Vector2 val = new Vector2((float)(value.Width / 2), (float)(value.Height / 2));
		Vector2 val2 = settings.AnchorPosition + LocalPosition;
		Color val3 = Color.Lerp(Lighting.GetColor(LocalPosition.ToTileCoordinates()), ColorTint, 0.2f) * Opacity;
		Vector2 scale = Scale;
		spritebatch.Draw(value, val2, (Rectangle?)value.Frame(), val3, Rotation, val, scale, (SpriteEffects)0, 0f);
		spritebatch.Draw(value, val2, (Rectangle?)value.Frame(), val3 * 0.25f, Rotation, val, scale * (1f + Opacity * 1.5f), (SpriteEffects)0, 0f);
	}
}
