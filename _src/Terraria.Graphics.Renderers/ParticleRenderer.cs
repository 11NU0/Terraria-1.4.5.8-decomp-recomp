using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

namespace Terraria.Graphics.Renderers;

public class ParticleRenderer
{
	public ParticleRendererSettings Settings;

	public List<IParticle> Particles = new List<IParticle>();

	public ParticleRenderer()
	{
		Settings = default;
	}

	public void Add(IParticle particle)
	{
		Particles.Add(particle);
	}

	public void Clear()
	{
		for (int i = 0; i < Particles.Count; i++)
		{
			if (Particles[i] is IPooledParticle pooledParticle)
			{
				pooledParticle.RestInPool();
			}
		}
		Particles.Clear();
	}

	public void Update()
	{
		for (int i = 0; i < Particles.Count; i++)
		{
			if (Particles[i].ShouldBeRemovedFromRenderer)
			{
				if (Particles[i] is IPooledParticle pooledParticle)
				{
					pooledParticle.RestInPool();
				}
				Particles.RemoveAt(i);
				i--;
			}
			else
			{
				Particles[i].Update(ref Settings);
			}
		}
	}

	public void Draw(SpriteBatch spriteBatch)
	{
		TimeLogger.StartTimestamp fromTimestamp = TimeLogger.Start();
		for (int i = 0; i < Particles.Count; i++)
		{
			if (!Particles[i].ShouldBeRemovedFromRenderer)
			{
				Particles[i].Draw(ref Settings, spriteBatch);
			}
		}
		TimeLogger.Particles.AddTime(fromTimestamp);
	}
}
