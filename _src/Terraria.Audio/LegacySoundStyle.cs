using System;
using Microsoft.Xna.Framework.Audio;
using Terraria.Testing;
using Terraria.Utilities;

namespace Terraria.Audio;

public class LegacySoundStyle : SoundStyle
{
	private static readonly UnifiedRandom Random = new UnifiedRandom();

	private readonly int _style;

	public readonly int Variations;

	public readonly int SoundId;

	public readonly int _maxTrackedInstances;

	public int Style
	{
		get
		{
			if (Variations != 1)
			{
				return Random.Next(_style, _style + Variations);
			}
			return _style;
		}
	}

	public override bool IsTrackable => SoundId == 42;

	public override int MaxTrackedInstances => _maxTrackedInstances;

	public LegacySoundStyle(int soundId, int style, SoundType type = SoundType.Sound, int maxTrackedInstances = 0)
		: base(type)
	{
		_style = style;
		Variations = 1;
		SoundId = soundId;
		_maxTrackedInstances = maxTrackedInstances;
	}

	public LegacySoundStyle(int soundId, int style, int variations, SoundType type = SoundType.Sound, int maxTrackedInstances = 0)
		: base(type)
	{
		Invariant.Assert(variations > 0, "Sounds should always have at least one variation, as it is inclusive of the base sound.");
		_style = style;
		Variations = variations;
		SoundId = soundId;
		_maxTrackedInstances = maxTrackedInstances;
	}

	private LegacySoundStyle(int soundId, int style, int variations, SoundType type, float volume, float pitchVariance, int maxTrackedInstances)
		: base(volume, pitchVariance, type)
	{
		Invariant.Assert(variations > 0, "Sounds should always have at least one variation, as it is inclusive of the base sound.");
		_style = style;
		Variations = variations;
		SoundId = soundId;
		_maxTrackedInstances = maxTrackedInstances;
	}

	public LegacySoundStyle WithVolume(float volume)
	{
		return new LegacySoundStyle(SoundId, _style, Variations, Type, volume, PitchVariance, MaxTrackedInstances);
	}

	public LegacySoundStyle WithVolumePerceived(float volume)
	{
		float num = 1.6609f;
		float volume2 = (float)Math.Pow(volume, num);
		return new LegacySoundStyle(SoundId, _style, Variations, Type, volume2, PitchVariance, MaxTrackedInstances);
	}

	public LegacySoundStyle WithPitchVariance(float pitchVariance)
	{
		return new LegacySoundStyle(SoundId, _style, Variations, Type, Volume, pitchVariance, MaxTrackedInstances);
	}

	public LegacySoundStyle AsMusic()
	{
		return new LegacySoundStyle(SoundId, _style, Variations, SoundType.Music, Volume, PitchVariance, MaxTrackedInstances);
	}

	public LegacySoundStyle AsAmbient()
	{
		return new LegacySoundStyle(SoundId, _style, Variations, SoundType.Ambient, Volume, PitchVariance, MaxTrackedInstances);
	}

	public LegacySoundStyle AsSound()
	{
		return new LegacySoundStyle(SoundId, _style, Variations, SoundType.Sound, Volume, PitchVariance, MaxTrackedInstances);
	}

	public bool Includes(int soundId, int style)
	{
		if (SoundId == soundId && style >= _style)
		{
			return style < _style + Variations;
		}
		return false;
	}

	public override SoundEffect GetRandomSound()
	{
		Invariant.Assert(SoundId == 42, "GetRandomSound only works with \"trackable\" sounds.");
		if (IsTrackable)
		{
			return SoundEngine.GetTrackableSoundByStyleId(Style);
		}
		return null;
	}
}
