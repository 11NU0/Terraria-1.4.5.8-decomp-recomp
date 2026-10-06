using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using Microsoft.Xna.Framework.Audio;
using ReLogic.Content.Sources;

namespace Terraria.Audio;

public class LegacyAudioSystem : IAudioSystem, IDisposable
{
	public IAudioTrack[] AudioTracks;

	public int MusicReplayDelay;

	public AudioEngine Engine;

	public SoundBank SoundBank;

	public WaveBank WaveBank;

	public Dictionary<int, string> TrackNamesByIndex;

	public Dictionary<int, IAudioTrack> DefaultTrackByIndex;

	public List<IContentSource> FileSources;

	public int[] TrackLoopCounts;

	public AudioTrackPlayCallback[] PlayCallbacks;

	public void LoadFromSources()
	{
		List<IContentSource> fileSources = FileSources;
		for (int i = 0; i < AudioTracks.Length; i++)
		{
			if (TrackNamesByIndex.TryGetValue(i, out var value))
			{
				string assetPath = "Music" + Path.DirectorySeparatorChar + value;
				IAudioTrack audioTrack = DefaultTrackByIndex[i];
				IAudioTrack audioTrack2 = audioTrack;
				IAudioTrack audioTrack3 = FindReplacementTrack(fileSources, assetPath);
				if (audioTrack3 != null)
				{
					audioTrack2 = audioTrack3;
				}
				if (AudioTracks[i] != audioTrack2)
				{
					AudioTracks[i].Stop((AudioStopOptions)1);
				}
				if (AudioTracks[i] != audioTrack)
				{
					AudioTracks[i].Dispose();
				}
				AudioTracks[i] = audioTrack2;
			}
		}
	}

	public void UseSources(List<IContentSource> sourcesFromLowestToHighest)
	{
		FileSources = sourcesFromLowestToHighest;
		LoadFromSources();
	}

	public void Update()
	{
		if (!WaveBank.IsPrepared)
		{
			return;
		}
		for (int i = 0; i < AudioTracks.Length; i++)
		{
			if (AudioTracks[i] != null)
			{
				AudioTracks[i].Update();
			}
		}
	}

	private IAudioTrack FindReplacementTrack(List<IContentSource> sources, string assetPath)
	{
		IAudioTrack audioTrack = null;
		for (int i = 0; i < sources.Count; i++)
		{
			IContentSource val = sources[i];
			if (!val.HasAsset(assetPath))
			{
				continue;
			}
			string extension = val.GetExtension(assetPath);
			try
			{
				IAudioTrack audioTrack2 = null;
				switch (extension)
				{
				case ".ogg":
					audioTrack2 = new OGGAudioTrack(val.OpenStream(assetPath));
					break;
				case ".wav":
					audioTrack2 = new WAVAudioTrack(val.OpenStream(assetPath));
					break;
				case ".mp3":
					audioTrack2 = new MP3AudioTrack(val.OpenStream(assetPath));
					break;
				}
				if (audioTrack2 != null)
				{
					audioTrack?.Dispose();
					audioTrack = audioTrack2;
				}
			}
			catch
			{
				string textToShow = "A resource pack failed to load " + assetPath + "!";
				Main.IssueReporter.AddReport(textToShow);
				Main.IssueReporterIndicator.AttemptLettingPlayerKnow();
			}
		}
		return audioTrack;
	}

	public ReadOnlyCollection<RendererDetail> VeryExpensiveWayToGetAllAudioRenderers()
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		AudioEngine val = new AudioEngine("Content\\TerrariaMusic.xgs");
		ReadOnlyCollection<RendererDetail> rendererDetails = val.RendererDetails;
		val.Dispose();
		return rendererDetails;
	}

	public LegacyAudioSystem()
	{
		Init();
	}

	public void Init(string renderer = "")
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Expected Obj, but got Unknown
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Expected Obj, but got Unknown
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Expected Obj, but got Unknown
		Engine = new AudioEngine("Content\\TerrariaMusic.xgs", TimeSpan.FromMilliseconds(250.0), renderer);
		SoundBank = new SoundBank(Engine, "Content\\Sound Bank.xsb");
		Engine.Update();
		WaveBank = new WaveBank(Engine, "Content\\Wave Bank.xwb", 0, (short)512);
		Engine.Update();
		AudioTracks = new IAudioTrack[Main.maxMusic];
		TrackNamesByIndex = new Dictionary<int, string>();
		DefaultTrackByIndex = new Dictionary<int, IAudioTrack>();
		TrackLoopCounts = new int[Main.maxMusic];
		PlayCallbacks = new AudioTrackPlayCallback[Main.maxMusic];
	}

	public IEnumerator PrepareWaveBank()
	{
		while (!WaveBank.IsPrepared)
		{
			Engine.Update();
			yield return null;
		}
	}

	public void LoadCue(int cueIndex, string cueName)
	{
		CueAudioTrack cueAudioTrack = new CueAudioTrack(SoundBank, cueName);
		TrackNamesByIndex[cueIndex] = cueName;
		DefaultTrackByIndex[cueIndex] = cueAudioTrack;
		AudioTracks[cueIndex] = cueAudioTrack;
	}

	public void UpdateMisc()
	{
	}

	public void PauseAll()
	{
		if (!WaveBank.IsPrepared)
		{
			return;
		}
		float[] musicFade = Main.musicFade;
		for (int i = 0; i < AudioTracks.Length; i++)
		{
			if (AudioTracks[i] != null && !AudioTracks[i].IsPaused && AudioTracks[i].IsPlaying && musicFade[i] > 0f)
			{
				try
				{
					AudioTracks[i].Pause();
				}
				catch (Exception)
				{
				}
			}
		}
	}

	public void ResumeAll()
	{
		if (!WaveBank.IsPrepared)
		{
			return;
		}
		float[] musicFade = Main.musicFade;
		for (int i = 0; i < AudioTracks.Length; i++)
		{
			if (AudioTracks[i] != null && AudioTracks[i].IsPaused && musicFade[i] > 0f)
			{
				try
				{
					AudioTracks[i].Resume();
				}
				catch (Exception)
				{
				}
			}
		}
	}

	public void UpdateAmbientCueState(int i, bool gameIsActive, ref float trackVolume, float systemVolume)
	{
		if (!WaveBank.IsPrepared || AudioTracks[i] == null)
		{
			return;
		}
		if (systemVolume == 0f)
		{
			if (AudioTracks[i].IsPlaying)
			{
				AudioTracks[i].Stop((AudioStopOptions)1);
			}
			return;
		}
		if (!AudioTracks[i].IsPlaying)
		{
			AudioTracks[i].Reuse();
			AudioTracks[i].Play();
			AudioTracks[i].SetVariable("Volume", trackVolume * systemVolume);
			return;
		}
		if (AudioTracks[i].IsPaused & gameIsActive)
		{
			AudioTracks[i].Resume();
			return;
		}
		trackVolume += 0.005f;
		if (trackVolume > 1f)
		{
			trackVolume = 1f;
		}
		AudioTracks[i].SetVariable("Volume", trackVolume * systemVolume);
	}

	public void UpdateAmbientCueTowardStopping(int i, float stoppingSpeed, ref float trackVolume, float systemVolume)
	{
		if (!WaveBank.IsPrepared || AudioTracks[i] == null)
		{
			return;
		}
		if (!AudioTracks[i].IsPlaying)
		{
			trackVolume = 0f;
			return;
		}
		if (trackVolume > 0f)
		{
			trackVolume -= stoppingSpeed;
			if (trackVolume < 0f)
			{
				trackVolume = 0f;
			}
		}
		if (trackVolume <= 0f)
		{
			AudioTracks[i].Stop((AudioStopOptions)1);
		}
		else
		{
			AudioTracks[i].SetVariable("Volume", trackVolume * systemVolume);
		}
	}

	public bool IsTrackPlaying(int trackIndex)
	{
		if (!WaveBank.IsPrepared)
		{
			return false;
		}
		if (AudioTracks[trackIndex] != null)
		{
			return AudioTracks[trackIndex].IsPlaying;
		}
		return false;
	}

	public void UpdateCommonTrack(bool active, int i, float totalVolume, ref float tempFade)
	{
		if (!WaveBank.IsPrepared || AudioTracks[i] == null)
		{
			return;
		}
		tempFade += 0.005f;
		if (tempFade > 1f)
		{
			tempFade = 1f;
		}
		if (!AudioTracks[i].IsPlaying & active)
		{
			AudioTracks[i].Reuse();
			AudioTracks[i].SetVariable("Volume", totalVolume);
			AudioTracks[i].Play();
			if (PlayCallbacks[i] != null)
			{
				PlayCallbacks[i](i, TrackLoopCounts[i]);
			}
			TrackLoopCounts[i]++;
		}
		else
		{
			AudioTracks[i].SetVariable("Volume", totalVolume);
		}
	}

	public void UpdateCommonTrackTowardStopping(int i, float totalVolume, ref float tempFade, bool isMainTrackAudible)
	{
		if (!WaveBank.IsPrepared || AudioTracks[i] == null)
		{
			return;
		}
		if (AudioTracks[i].IsPlaying || !AudioTracks[i].IsStopped)
		{
			if (isMainTrackAudible)
			{
				tempFade -= 0.005f;
			}
			else if (Main.curMusic == 0)
			{
				tempFade = 0f;
			}
			if (tempFade <= 0f)
			{
				tempFade = 0f;
				AudioTracks[i].SetVariable("Volume", 0f);
				AudioTracks[i].Stop((AudioStopOptions)1);
				TrackLoopCounts[i] = 0;
			}
			else
			{
				AudioTracks[i].SetVariable("Volume", totalVolume);
			}
		}
		else
		{
			tempFade = 0f;
		}
	}

	public void UpdateAudioEngine()
	{
		Engine.Update();
	}

	public void SetPlayCallback(int trackIndex, AudioTrackPlayCallback callback)
	{
		PlayCallbacks[trackIndex] = callback;
	}

	public void Dispose()
	{
		SoundBank.Dispose();
		WaveBank.Dispose();
		Engine.Dispose();
	}

	public List<RendererDetail> GetAvailableAudioRenderers()
	{
		List<RendererDetail> list = new List<RendererDetail>();
		list.AddRange(VeryExpensiveWayToGetAllAudioRenderers());
		return list;
	}

	public void ChangeAudioDevice(string deviceId)
	{
		Dispose();
		Init(deviceId);
		for (int i = 1; i < Main.maxMusic; i++)
		{
			LoadCue(i, "Music_" + i);
		}
		LoadFromSources();
		SoundEngine.Reload();
	}
}
