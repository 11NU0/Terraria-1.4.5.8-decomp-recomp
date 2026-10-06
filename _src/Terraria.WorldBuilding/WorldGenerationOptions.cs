using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Terraria.IO;

namespace Terraria.WorldBuilding;

public class WorldGenerationOptions
{
	private class OptionStorage<T> where T : AWorldGenerationOption
	{
		public static T Instance;
	}

	private static List<AWorldGenerationOption> _options;

	private const string _powerPermissionsLineHeader = "seed_";

	public static IEnumerable<AWorldGenerationOption> Options => _options;

	public static T Get<T>() where T : AWorldGenerationOption
	{
		return OptionStorage<T>.Instance;
	}

	static WorldGenerationOptions()
	{
		_options = new List<AWorldGenerationOption>();
		Register<WorldSeedOption_Normal>();
		Register<WorldSeedOption_NotTheBees>();
		Register<WorldSeedOption_Drunk>();
		Register<WorldSeedOption_Anniversary>();
		Register<WorldSeedOption_DontStarve>();
		Register<WorldSeedOption_ForTheWorthy>();
		Register<WorldSeedOption_NoTraps>();
		Register<WorldSeedOption_Remix>();
		Register<WorldSeedOption_Everything>();
		Register<WorldSeedOption_Skyblock>();
	}

	public static void Register<T>() where T : AWorldGenerationOption, new()
	{
		if (OptionStorage<T>.Instance != null)
		{
			throw new ArgumentException(string.Concat(typeof(T), " has already been registered"));
		}
		T item = (OptionStorage<T>.Instance = new T());
		_options.Add(item);
	}

	public static void Reset()
	{
		Get<WorldSeedOption_Normal>().Enabled = true;
	}

	public static void SelectOption(AWorldGenerationOption option)
	{
		Reset();
		if (option != null)
		{
			option.Enabled = true;
		}
	}

	public static AWorldGenerationOption GetOptionFromSeedText(string processedSeed)
	{
		int num = WorldFileData.TranslateSeed(processedSeed);
		string text = Regex.Replace(processedSeed.ToLower(), "[^a-z0-9]+", "");
		foreach (AWorldGenerationOption option in Options)
		{
			int[] specialSeedValues = option.SpecialSeedValues;
			foreach (int num2 in specialSeedValues)
			{
				if (num == num2)
				{
					return option;
				}
			}
			string[] specialSeedNames = option.SpecialSeedNames;
			foreach (string text2 in specialSeedNames)
			{
				if (text == text2)
				{
					return option;
				}
			}
		}
		return null;
	}

	public static void TryEnablingFlagFrom(string line)
	{
		int length = "seed_".Length;
		if (line.Length < length || !line.ToLower().StartsWith("seed_"))
		{
			return;
		}
		string[] array = line.Substring(length).Split('=');
		if (array.Length == 2 && int.TryParse(array[1].Trim(), out var result))
		{
			bool autoGenEnabled = Utils.Clamp(result, 0, 1) == 1;
			string namePiece = array[0].Trim().ToLower();
			AWorldGenerationOption aWorldGenerationOption = _options.FirstOrDefault((AWorldGenerationOption x) => x.ServerConfigName != null && x.ServerConfigName == namePiece);
			if (aWorldGenerationOption != null)
			{
				aWorldGenerationOption.AutoGenEnabled = autoGenEnabled;
			}
		}
	}
}
