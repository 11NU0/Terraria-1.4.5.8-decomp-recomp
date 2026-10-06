using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;

namespace Terraria.Graphics.Renderers;

public class BloodyExplosionParticle : ABasicParticle
{
	public float FadeInNormalizedTime = 0.25f;

	public float FadeOutNormalizedTime = 0.75f;

	public float TimeToLive = 20f;

	public float Opacity;

	public float InnerOpacity;

	public float InitialScale = 1f;

	public Color ColorTint = Color.White;

	public Color LightColorTint = Color.Transparent;

	private float _timeSinceSpawn;

	public override void FetchFromPool()
	{
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		base.FetchFromPool();
		_timeSinceSpawn = 0f;
		Opacity = 0f;
		InnerOpacity = 0f;
		FadeInNormalizedTime = 0.1f;
		FadeOutNormalizedTime = 0.9f;
		TimeToLive = 20f;
		InitialScale = 1f;
		ColorTint = Color.White;
		LightColorTint = Color.Transparent;
	}

	public override void Update(ref ParticleRendererSettings settings)
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0119: Unknown result type (might be due to invalid IL or missing references)
		//IL_0128: Unknown result type (might be due to invalid IL or missing references)
		//IL_012d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0132: Unknown result type (might be due to invalid IL or missing references)
		//IL_01df: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0149: Unknown result type (might be due to invalid IL or missing references)
		//IL_014e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0168: Unknown result type (might be due to invalid IL or missing references)
		//IL_0169: Unknown result type (might be due to invalid IL or missing references)
		//IL_016e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0174: Unknown result type (might be due to invalid IL or missing references)
		//IL_0189: Unknown result type (might be due to invalid IL or missing references)
		//IL_018f: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0155: Unknown result type (might be due to invalid IL or missing references)
		//IL_015c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0161: Unknown result type (might be due to invalid IL or missing references)
		base.Update(ref settings);
		_timeSinceSpawn++;
		float fromValue = _timeSinceSpawn / TimeToLive;
		Scale = Vector2.One * InitialScale * Utils.Remap(fromValue, 0f, 0.3f, 0.5f, 1f);
		float num = 0.4f;
		Opacity = MathHelper.Clamp(Utils.Remap(fromValue, 0f, FadeInNormalizedTime, 0f, 1f) * Utils.Remap(fromValue, FadeOutNormalizedTime, 1f, 1f, 0f), 0f, 1f) * num;
		InnerOpacity = MathHelper.Clamp(Utils.Remap(fromValue, 0f, FadeInNormalizedTime * 0.75f, 0f, 1f) * Utils.Remap(fromValue, 0.3f, 0.45f, 1f, 0f), 0f, 1f) * num;
		if (_timeSinceSpawn == 3f)
		{
			Rectangle val = Utils.CenteredRectangle(LocalPosition, new Vector2(16f, 16f));
			for (int i = 0; i < 50; i++)
			{
				Vector2 val2 = Main.rand.NextVector2CircularEdge(4f, 4f);
				if (i % 2 == 0)
				{
					val2 *= 0.5f;
				}
				Dust obj = Main.dust[Dust.NewDust(val.TopLeft(), val.Width, val.Height, 5, 0f, 0f, 100, default, 1.25f + Main.rand.NextFloat() * 0.5f)];
				obj.velocity = val2;
				obj.noGravity = i % 3 == 0;
			}
		}
		if (LightColorTint != Color.Transparent)
		{
			Color val3 = LightColorTint * Opacity;
			Lighting.AddLight(LocalPosition, (float)(int)val3.R / 255f, (float)(int)val3.G / 255f, (float)(int)val3.B / 255f);
		}
		if (_timeSinceSpawn >= TimeToLive)
		{
			ShouldBeRemovedFromRenderer = true;
		}
	}

	public override void Draw(ref ParticleRendererSettings settings, SpriteBatch spritebatch)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0105: Unknown result type (might be due to invalid IL or missing references)
		//IL_010d: Unknown result type (might be due to invalid IL or missing references)
		//IL_010e: Unknown result type (might be due to invalid IL or missing references)
		//IL_011e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0127: Unknown result type (might be due to invalid IL or missing references)
		//IL_0131: Unknown result type (might be due to invalid IL or missing references)
		//IL_0139: Unknown result type (might be due to invalid IL or missing references)
		//IL_013b: Unknown result type (might be due to invalid IL or missing references)
		float num = _timeSinceSpawn / TimeToLive;
		Vector2 val = settings.AnchorPosition + LocalPosition;
		Color val2 = Color.Lerp(Lighting.GetColor(LocalPosition.ToTileCoordinates()).MultiplyRGBA(ColorTint), ColorTint, 0.65f);
		Texture2D value = TextureAssets.Extra[174].Value;
		Vector2 val3 = new Vector2((float)(value.Width / 2), (float)(value.Height / 2));
		Vector2 val4 = Scale * (1.1f + 0.15f * num);
		Color val5 = val2 * Opacity;
		Texture2D value2 = TextureAssets.Extra[267].Value;
		Vector2 val6 = new Vector2((float)(value2.Width / 2), (float)(value2.Height / 2));
		Vector2 val7 = Scale * (1f + 0.05f * num);
		Color val8 = val2 * InnerOpacity;
		spritebatch.Draw(value, val, (Rectangle?)value.Frame(), val5, Rotation, val3, val4, (SpriteEffects)0, 0f);
		spritebatch.Draw(value2, val, (Rectangle?)value2.Frame(), val8, Rotation, val6, val7, (SpriteEffects)0, 0f);
	}

	public BloodyExplosionParticle()
	{
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
	}
}
