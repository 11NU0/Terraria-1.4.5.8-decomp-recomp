using System;
using Microsoft.Xna.Framework.Audio;

namespace Terraria.Audio;

public abstract class ASoundEffectBasedAudioTrack : IAudioTrack, IDisposable
{
	protected const int bufferLength = 4096;

	protected const int bufferCountPerSubmit = 2;

	protected const int buffersToCoverFor = 8;

	protected byte[] _bufferToSubmit = new byte[4096];

	protected float[] _temporaryBuffer = new float[2048];

	private int _sampleRate;

	private AudioChannels _channels;

	protected DynamicSoundEffectInstance _soundEffectInstance;

	public bool IsPlaying
	{
		get
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Invalid comparison between Unknown and I4
			return (int)((SoundEffectInstance)_soundEffectInstance).State == 0;
		}
	}

	public bool IsStopped
	{
		get
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Invalid comparison between Unknown and I4
			return (int)((SoundEffectInstance)_soundEffectInstance).State == 2;
		}
	}

	public bool IsPaused
	{
		get
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Invalid comparison between Unknown and I4
			return (int)((SoundEffectInstance)_soundEffectInstance).State == 1;
		}
	}

	public ASoundEffectBasedAudioTrack()
	{
	}

	protected void CreateSoundEffect(int sampleRate, AudioChannels channels)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Expected Obj, but got Unknown
		_sampleRate = sampleRate;
		_channels = channels;
		_soundEffectInstance = new DynamicSoundEffectInstance(_sampleRate, _channels);
	}

	private void _soundEffectInstance_BufferNeeded(object sender, EventArgs e)
	{
		PrepareBuffer();
	}

	public void Update()
	{
		if (IsPlaying && _soundEffectInstance.PendingBufferCount < 8)
		{
			PrepareBuffer();
		}
	}

	protected void ResetBuffer()
	{
		for (int i = 0; i < _bufferToSubmit.Length; i++)
		{
			_bufferToSubmit[i] = 0;
		}
	}

	protected void PrepareBuffer()
	{
		for (int i = 0; i < 2; i++)
		{
			ReadAheadPutAChunkIntoTheBuffer();
		}
	}

	public void Stop(AudioStopOptions options)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		((SoundEffectInstance)_soundEffectInstance).Stop((int)options == 1);
	}

	public void Play()
	{
		PrepareToPlay();
		((SoundEffectInstance)_soundEffectInstance).Play();
	}

	public void Pause()
	{
		((SoundEffectInstance)_soundEffectInstance).Pause();
	}

	public void SetVariable(string variableName, float value)
	{
		switch (variableName)
		{
		case "Volume":
		{
			float volume = ReMapVolumeToMatchXact(value);
			((SoundEffectInstance)_soundEffectInstance).Volume = volume;
			break;
		}
		case "Pitch":
			((SoundEffectInstance)_soundEffectInstance).Pitch = value;
			break;
		case "Pan":
			((SoundEffectInstance)_soundEffectInstance).Pan = value;
			break;
		}
	}

	private float ReMapVolumeToMatchXact(float musicVolume)
	{
		double num = 31.0 * (double)musicVolume - 25.0 - 11.94;
		return (float)Math.Pow(10.0, num / 20.0);
	}

	public void Resume()
	{
		((SoundEffectInstance)_soundEffectInstance).Resume();
	}

	protected virtual void PrepareToPlay()
	{
		ResetBuffer();
	}

	public abstract void Reuse();

	protected virtual void ReadAheadPutAChunkIntoTheBuffer()
	{
	}

	public abstract void Dispose();
}
