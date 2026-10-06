using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Terraria.Graphics.Renderers;

public class FadingParticle : ABasicParticle
{
	public float FadeInNormalizedTime;

	public float FadeOutNormalizedTime = 1f;

	public Color ColorTint = Color.White;

	public int Delay;

	protected float timeTolive;

	protected float timeSinceSpawn;

	protected bool fullbright = true;

	public int followPlayerIndex = -1;

	public override void FetchFromPool()
	{
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		base.FetchFromPool();
		FadeInNormalizedTime = 0f;
		FadeOutNormalizedTime = 1f;
		ColorTint = Color.White;
		timeTolive = 0f;
		timeSinceSpawn = 0f;
		followPlayerIndex = -1;
		Delay = 0;
	}

	public void SetTypeInfo(float timeToLive, bool fullbright = true)
	{
		timeTolive = timeToLive;
		this.fullbright = fullbright;
	}

	public override void Update(ref ParticleRendererSettings settings)
	{
		if (Delay > 0)
		{
			Delay--;
			return;
		}
		base.Update(ref settings);
		timeSinceSpawn++;
		if (timeSinceSpawn >= timeTolive)
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
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		Vector2 val = settings.AnchorPosition + LocalPosition;
		if (followPlayerIndex != -1)
		{
			val += Main.player[followPlayerIndex].MountedCenter;
		}
		Color val2 = (fullbright ? ColorTint : ColorTint.MultiplyRGB(Lighting.GetColor(LocalPosition.ToTileCoordinates()))) * Utils.GetLerpValue(0f, FadeInNormalizedTime, timeSinceSpawn / timeTolive, clamped: true) * Utils.GetLerpValue(1f, FadeOutNormalizedTime, timeSinceSpawn / timeTolive, clamped: true);
		spritebatch.Draw(_texture.Value, val, (Rectangle?)_frame, val2, Rotation, _origin, Scale, (SpriteEffects)0, 0f);
	}

	public FadingParticle()
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
	}
}
