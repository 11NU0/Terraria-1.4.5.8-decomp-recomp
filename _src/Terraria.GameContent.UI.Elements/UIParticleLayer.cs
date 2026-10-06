using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Graphics.Renderers;
using Terraria.UI;

namespace Terraria.GameContent.UI.Elements;

public class UIParticleLayer : UIElement
{
	public ParticleRenderer ParticleSystem;

	public Vector2 AnchorPositionOffsetByPercents;

	public Vector2 AnchorPositionOffsetByPixels;

	public UIParticleLayer()
	{
		IgnoresMouseInteraction = true;
		ParticleSystem = new ParticleRenderer();
		OnUpdate += ParticleSystemUpdate;
	}

	private void ParticleSystemUpdate(UIElement affectedElement)
	{
		ParticleSystem.Update();
	}

	public override void Recalculate()
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		base.Recalculate();
		Rectangle r = GetDimensions().ToRectangle();
		ParticleSystem.Settings.AnchorPosition = r.TopLeft() + AnchorPositionOffsetByPercents * r.Size() + AnchorPositionOffsetByPixels;
	}

	protected override void DrawSelf(SpriteBatch spriteBatch)
	{
		ParticleSystem.Draw(spriteBatch);
	}

	public void AddParticle(IParticle particle)
	{
		ParticleSystem.Add(particle);
	}

	public void ClearParticles()
	{
		ParticleSystem.Clear();
	}
}
