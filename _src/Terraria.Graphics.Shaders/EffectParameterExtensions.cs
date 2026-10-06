using Microsoft.Xna.Framework.Graphics;

namespace Terraria.Graphics.Shaders;

public static class EffectParameterExtensions
{
	public static ShaderData.EffectParameter<T> GetParameter<T>(this Effect effect, string name)
	{
		return ShaderData.EffectParameter<T>.Get(effect.Parameters[name]);
	}
}
