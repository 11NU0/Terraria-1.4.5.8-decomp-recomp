using System.Collections.Generic;
using System.Linq;

namespace Terraria.GameContent;

public static class SecretSeedsTracker
{
	private static List<string> _seedsForConfig = new List<string>();

	private static List<WorldGen.SecretSeed> _seedsForInterface = new List<WorldGen.SecretSeed>();

	private static bool _processedConfig = false;

	public static List<WorldGen.SecretSeed> SeedsForInterface => _seedsForInterface;

	public static void SetstringsFromConfig(ICollection<string> seedStrings)
	{
		_seedsForConfig.AddRange(seedStrings);
	}

	public static void PrepareInterface()
	{
		if (!_processedConfig)
		{
			_processedConfig = true;
			_seedsForInterface.Clear();
			foreach (string item in _seedsForConfig)
			{
				if (SeedHasSecret(item, out var seed))
				{
					_seedsForInterface.Add(seed);
				}
			}
			_seedsForInterface = _seedsForInterface.Distinct().ToList();
			_seedsForConfig.Clear();
			_seedsForConfig.AddRange(_seedsForInterface.Select((WorldGen.SecretSeed x) => x.TextThatWasUsedToUnlock));
		}
		_seedsForConfig.Sort();
		_seedsForInterface.Sort((WorldGen.SecretSeed a, WorldGen.SecretSeed b) => a.TextThatWasUsedToUnlock.CompareTo(b.TextThatWasUsedToUnlock));
	}

	private static bool SeedHasSecret(string seedString, out WorldGen.SecretSeed seed)
	{
		return WorldGen.SecretSeed.CheckInputForSecretSeed(seedString, out seed);
	}

	public static void AddSeedToTrack(string seedString)
	{
		if (SeedHasSecret(seedString, out var seed) && !_seedsForInterface.Contains(seed))
		{
			_seedsForConfig.Add(seed.TextThatWasUsedToUnlock);
			_seedsForInterface.Add(seed);
			Main.SaveSettings();
		}
	}

	public static List<string> GetStringsToSave()
	{
		return _seedsForConfig;
	}
}
