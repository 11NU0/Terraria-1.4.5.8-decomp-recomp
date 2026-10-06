using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;
using Terraria.GameContent;

namespace Terraria.Graphics.Renderers;

public class ShockIconParticle : ABasicParticle
{
	public float FadeInNormalizedTime = 0.25f;

	public float FadeOutNormalizedTime = 0.75f;

	public float TimeToLive = 20f;

	public float Opacity;

	public float InitialScale = 1f;

	public Color ColorTint = Color.White;

	public ProjectileKey ParentProjectileKey;

	public Vector2 OffsetFromParent;

	private Vector2 initialPosition;

	private float _timeSinceSpawn;

	public override void FetchFromPool()
	{
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		base.FetchFromPool();
		_timeSinceSpawn = 0f;
		Opacity = 0f;
		FadeInNormalizedTime = 0.1f;
		FadeOutNormalizedTime = 0.9f;
		TimeToLive = 20f;
		InitialScale = 1f;
		ColorTint = Color.White;
	}

	public override void Update(ref ParticleRendererSettings settings)
	{
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0102: Unknown result type (might be due to invalid IL or missing references)
		//IL_0107: Unknown result type (might be due to invalid IL or missing references)
		//IL_010c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		if (_timeSinceSpawn == 0f)
		{
			initialPosition = LocalPosition;
		}
		base.Update(ref settings);
		_timeSinceSpawn++;
		float num = _timeSinceSpawn / TimeToLive;
		Scale = Vector2.One * InitialScale * Utils.MultiLerp(num, 0.2f, 0.9f, 1.3f, 0.9f);
		Opacity = MathHelper.Clamp(Utils.Remap(num, 0f, FadeInNormalizedTime, 0f, 1f) * Utils.Remap(num, FadeOutNormalizedTime, 1f, 1f, 0f), 0f, 1f) * 0.5f;
		if (ParentProjectileKey.TryGetActive(out var proj))
		{
			LocalPosition = proj.Top + num * OffsetFromParent;
		}
		else
		{
			LocalPosition = initialPosition + num * OffsetFromParent;
		}
		if (_timeSinceSpawn >= TimeToLive)
		{
			ShouldBeRemovedFromRenderer = true;
		}
	}

	public override void Draw(ref ParticleRendererSettings settings, SpriteBatch spritebatch)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		Vector2 val = settings.AnchorPosition + LocalPosition;
		Texture2D value = TextureAssets.Extra[268].Value;
		Vector2 val2 = new Vector2((float)(value.Width / 2), (float)(value.Height / 2));
		Vector2 scale = Scale;
		Color val3 = Color.Lerp(Lighting.GetColor(LocalPosition.ToTileCoordinates()).MultiplyRGBA(ColorTint), ColorTint, 0.75f) * Opacity;
		spritebatch.Draw(value, val, (Rectangle?)value.Frame(), val3, Rotation, val2, scale, (SpriteEffects)0, 0f);
	}

	public ShockIconParticle()
	{
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
	}
}
