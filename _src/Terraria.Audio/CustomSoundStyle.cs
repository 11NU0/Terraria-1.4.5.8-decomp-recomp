using Microsoft.Xna.Framework.Audio;
using Terraria.Testing;
using Terraria.Utilities;

namespace Terraria.Audio;

public class CustomSoundStyle : SoundStyle
{
	private static readonly UnifiedRandom Random = new UnifiedRandom();

	private readonly SoundEffect[] _soundEffects;

	public override bool IsTrackable => true;

	public override int MaxTrackedInstances => 0;

	public CustomSoundStyle(SoundEffect soundEffect, SoundType type = SoundType.Sound, float volume = 1f, float pitchVariance = 0f)
		: base(volume, pitchVariance, type)
	{
		_soundEffects = new SoundEffect[1] { soundEffect };
	}

	public CustomSoundStyle(SoundEffect[] soundEffects, SoundType type = SoundType.Sound, float volume = 1f, float pitchVariance = 0f)
		: base(volume, pitchVariance, type)
	{
		Invariant.Assert(soundEffects != null && soundEffects.Length != 0, "CustomSoundStyle with no effects");
		_soundEffects = soundEffects;
	}

	public override SoundEffect GetRandomSound()
	{
		return _soundEffects[Random.Next(_soundEffects.Length)];
	}
}
