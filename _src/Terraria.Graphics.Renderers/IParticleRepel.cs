namespace Terraria.Graphics.Renderers;

public interface IParticleRepel : IParticle
{
	void BeRepelled(ref ParticleRepelDetails details);
}
