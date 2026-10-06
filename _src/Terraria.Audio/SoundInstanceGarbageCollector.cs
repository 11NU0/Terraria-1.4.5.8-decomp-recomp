using System.Collections.Generic;
using Microsoft.Xna.Framework.Audio;

namespace Terraria.Audio;

public static class SoundInstanceGarbageCollector
{
	private static readonly List<SoundEffectInstance> _activeSounds = new List<SoundEffectInstance>(128);

	public static void Track(SoundEffectInstance sound)
	{
		if (Program.IsFna)
		{
			_activeSounds.Add(sound);
		}
	}

	public static void Update()
	{
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Invalid comparison between Unknown and I4
		for (int i = 0; i < _activeSounds.Count; i++)
		{
			if (_activeSounds[i] == null)
			{
				_activeSounds.RemoveAt(i);
				i--;
			}
			else if ((int)_activeSounds[i].State == 2)
			{
				_activeSounds[i].Dispose();
				_activeSounds.RemoveAt(i);
				i--;
			}
		}
	}
}
