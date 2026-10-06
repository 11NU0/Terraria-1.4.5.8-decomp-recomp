using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Terraria.Testing;

namespace Terraria.Audio;

public class ActiveSound
{
	public delegate bool LoopedPlayCondition();

	public readonly bool IsGlobal;

	public Vector2 Position;

	public float Volume;

	public float Pitch;

	public LoopedPlayCondition Condition;

	public SoundEffectInstance Sound { get; private set; }

	public SoundStyle Style { get; private set; }

	public bool IsPlaying
	{
		get
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Invalid comparison between Unknown and I4
			if (Sound != null)
			{
				return (int)Sound.State == 0;
			}
			return false;
		}
	}

	private void UseOverrides(SoundPlayOverrides overrides)
	{
		if (overrides.Volume.HasValue)
		{
			Volume = overrides.Volume.Value;
		}
	}

	public ActiveSound(SoundStyle style, Vector2 position, SoundPlayOverrides overrides)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		Invariant.Assert(style.IsTrackable, "Only trackable sounds may be used here.");
		Position = position;
		Volume = 1f;
		Pitch = style.PitchVariance;
		IsGlobal = false;
		Style = style;
		UseOverrides(overrides);
		Play();
	}

	public ActiveSound(SoundStyle style)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		Invariant.Assert(style.IsTrackable, "Only trackable sounds may be used here.");
		Position = Vector2.Zero;
		Volume = 1f;
		Pitch = style.PitchVariance;
		IsGlobal = true;
		Style = style;
		Play();
	}

	public ActiveSound(SoundStyle style, Vector2 position, LoopedPlayCondition condition, SoundPlayOverrides overrides)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		Invariant.Assert(style.IsTrackable, "Only trackable sounds may be used here.");
		Position = position;
		Volume = 1f;
		Pitch = style.PitchVariance;
		IsGlobal = false;
		Style = style;
		UseOverrides(overrides);
		PlayLooped(condition);
	}

	private void Play()
	{
		SoundEffectInstance val = (Sound = Style.GetRandomSound().CreateInstance());
		val.Pitch += Style.GetRandomPitch();
		Pitch = val.Pitch;
		val.Volume = DetermineIntendedVolume();
		val.Play();
		SoundInstanceGarbageCollector.Track(val);
		Update();
	}

	private void PlayLooped(LoopedPlayCondition condition)
	{
		SoundEffectInstance val = (Sound = Style.GetRandomSound().CreateInstance());
		val.Pitch += Style.GetRandomPitch();
		Pitch = val.Pitch;
		val.IsLooped = true;
		Condition = condition;
		val.Play();
		SoundInstanceGarbageCollector.Track(val);
		Update();
	}

	public void Stop()
	{
		if (Sound != null)
		{
			Sound.Stop();
		}
	}

	public void Pause()
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (Sound != null && (int)Sound.State == 0)
		{
			Sound.Pause();
		}
	}

	public void Resume()
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Invalid comparison between Unknown and I4
		if (Sound != null && (int)Sound.State == 1)
		{
			Sound.Resume();
		}
	}

	public void Update()
	{
		if (Sound != null)
		{
			if (Condition != null && !Condition())
			{
				Sound.Stop(true);
				return;
			}
			float volume = DetermineIntendedVolume();
			Sound.Volume = volume;
			Sound.Pitch = Pitch;
		}
	}

	private float DetermineIntendedVolume()
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		float num = 1f;
		if (!IsGlobal)
		{
			Vector2 val = Position - Main.Camera.Center;
			Sound.Pan = MathHelper.Clamp(val.X / ((float)Main.MaxWorldViewSize.X * 0.5f), -1f, 1f);
			num = MathHelper.Clamp(1f - val.Length() / LegacySoundPlayer.SoundAttenuationDistance, 0f, 1f);
		}
		num *= Style.Volume * Volume;
		switch (Style.Type)
		{
		case SoundType.Sound:
			num *= Main.soundVolume;
			break;
		case SoundType.Ambient:
			num *= Main.ambientVolume;
			break;
		case SoundType.Music:
			num *= Main.musicVolume;
			break;
		}
		return MathHelper.Clamp(num, 0f, 1f);
	}
}
