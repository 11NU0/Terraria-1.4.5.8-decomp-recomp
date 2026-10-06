#define TRACE
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading;
using CsvHelper;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using ReLogic.Content;
using ReLogic.Content.Sources;
using ReLogic.Graphics;
using ReLogic.Utilities;
using Terraria.Testing;
using Terraria.Utilities;

namespace Terraria.Localization;

public class LanguageManager
{
	public static LanguageManager Instance = new LanguageManager();

	private readonly Dictionary<string, LocalizedText> _localizedTexts = new Dictionary<string, LocalizedText>();

	private readonly Dictionary<string, List<string>> _categoryGroupedKeys = new Dictionary<string, List<string>>();

	private readonly Dictionary<string, Dictionary<string, string>> _textVariations = new Dictionary<string, Dictionary<string, string>>();

	private GameCulture _fallbackCulture = GameCulture.DefaultCulture;

	private List<IContentSource> _contentSources = new List<IContentSource>();

	public const char VariationSeparatorSign = '$';

	public GameCulture ActiveCulture { get; private set; }

	public event LanguageChangeCallback OnLanguageChanged;

	private LanguageManager()
	{
		_localizedTexts[""] = LocalizedText.Empty;
	}

	public int GetCategorySize(string name)
	{
		if (_categoryGroupedKeys.TryGetValue(name, out var value))
		{
			return value.Count;
		}
		return 0;
	}

	public void SetLanguage(int legacyId)
	{
		GameCulture language = GameCulture.FromLegacyId(legacyId);
		SetLanguage(language);
	}

	public void SetLanguage(string cultureName)
	{
		GameCulture language = GameCulture.FromName(cultureName);
		SetLanguage(language);
	}

	public void EstimateWordCount()
	{
		string[] array = (from word in _localizedTexts.Values.Select((LocalizedText v) => v.UnformattedValue).SelectMany((string text) => text.Split(' ', '\n', '-', ','))
			where !string.IsNullOrWhiteSpace(word) && !word.StartsWith("{") && !word.EndsWith("}")
			select word).ToArray();
		(from w in array.Distinct()
			orderby w.Length
			select w).ToArray();
		Trace.WriteLine("Estimated word count: " + array.Length);
		Trace.WriteLine("Excluding one letter words: " + array.Where((string w) => w.Length > 1).Count());
	}

	private void SetAllTextValuesToKeys()
	{
		foreach (KeyValuePair<string, LocalizedText> localizedText in _localizedTexts)
		{
			localizedText.Value.SetValue(localizedText.Key);
		}
	}

	private string[] GetLanguageFilesForCulture(GameCulture culture)
	{
		Assembly.GetExecutingAssembly();
		return Array.FindAll(typeof(Program).Assembly.GetManifestResourceNames(), (string element) => element.StartsWith("Terraria.Localization.Content." + culture.CultureInfo.Name) && element.EndsWith(".json"));
	}

	public void SetLanguage(GameCulture culture)
	{
		if (ActiveCulture != culture)
		{
			Thread.CurrentThread.CurrentCulture = culture.CultureInfo;
			Thread.CurrentThread.CurrentUICulture = culture.CultureInfo;
			ReloadLanguage(culture);
		}
	}

	private void ReloadLanguage(GameCulture targetCulture)
	{
		if (ActiveCulture != _fallbackCulture)
		{
			SetAllTextValuesToKeys();
			if (targetCulture != _fallbackCulture)
			{
				LoadLanguage(_fallbackCulture);
			}
		}
		LoadLanguage(targetCulture);
		if (OnLanguageChanged != null)
		{
			OnLanguageChanged(this);
		}
	}

	private void LoadLanguage(GameCulture culture)
	{
		ActiveCulture = culture;
		_textVariations.Clear();
		LoadFilesForCulture(culture);
		LoadFromContentSources();
		ProcessCopyCommandsInTexts();
	}

	private void LoadFilesForCulture(GameCulture culture)
	{
		string[] languageFilesForCulture = GetLanguageFilesForCulture(culture);
		foreach (string text in languageFilesForCulture)
		{
			try
			{
				string text2 = null;
				if (text2 == null)
				{
					text2 = Utils.ReadEmbeddedResource(text);
				}
				if (text2 == null || text2.Length < 2)
				{
					throw new FormatException();
				}
				LoadLanguageFromFileTextJson(text2, canCreateCategories: true);
			}
			catch (Exception)
			{
				if (Debugger.IsAttached)
				{
					Debugger.Break();
				}
				Console.WriteLine("Failed to load language file: " + text);
				break;
			}
		}
	}

	private void ProcessCopyCommandsInTexts()
	{
		Regex regex = new Regex("{\\$(\\w+\\.\\w+)}", RegexOptions.Compiled);
		foreach (KeyValuePair<string, LocalizedText> localizedText in _localizedTexts)
		{
			bool condition = false;
			LocalizedText value = localizedText.Value;
			for (int i = 0; i < 100; i++)
			{
				string unformattedValue = value.UnformattedValue;
				string text = regex.Replace(unformattedValue, (Match match) =>
				{
					string text2 = match.Groups[1].ToString();
					LocalizedText value2;
					return (!_localizedTexts.TryGetValue(text2, out value2)) ? text2 : value2.UnformattedValue;
				});
				if (text == unformattedValue)
				{
					condition = true;
					break;
				}
				value.SetValue(text);
			}
			Invariant.Assert(condition, "Replacement loop created when attempts to parse: {0}", value.UnformattedValue);
		}
	}

	public void UseSources(List<IContentSource> sourcesFromLowestToHighest)
	{
		_contentSources = sourcesFromLowestToHighest;
		ReloadLanguage(ActiveCulture);
	}

	private void LoadFromContentSources()
	{
		string name = ActiveCulture.Name;
		string text = ("Localization" + Path.DirectorySeparatorChar + name).ToLower();
		foreach (IContentSource contentSource in _contentSources)
		{
			foreach (string item in contentSource.GetAllAssetsStartingWith(text))
			{
				string extension = contentSource.GetExtension(item);
				if (!(extension == ".json") && !(extension == ".csv"))
				{
					continue;
				}
				try
				{
					using Stream stream = contentSource.OpenStream(item);
					using StreamReader streamReader = new StreamReader(stream);
					string fileText = streamReader.ReadToEnd();
					if (extension == ".json")
					{
						LoadLanguageFromFileTextJson(fileText, canCreateCategories: false);
					}
					if (extension == ".csv")
					{
						LoadLanguageFromFileTextCsv(fileText);
					}
				}
				catch (Exception ex)
				{
					IAssetRepository val = XnaExtensions.Get<IAssetRepository>((IServiceProvider)((Game)Main.instance).Services);
					if (val != null && val.AssetLoadFailHandler != null)
					{
						string text2 = item + extension;
						val.AssetLoadFailHandler.Invoke(text2, (Exception)(object)AssetLoadException.FromAssetException(text2, ex));
					}
				}
			}
		}
	}

	public void LoadLanguageFromFileTextCsv(string fileText)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Expected Obj, but got Unknown
		using TextReader textReader = new StringReader(fileText);
		CsvReader val = new CsvReader(textReader);
		try
		{
			val.Configuration.HasHeaderRecord = true;
			if (!val.ReadHeader())
			{
				return;
			}
			string[] fieldHeaders = val.FieldHeaders;
			int num = -1;
			int num2 = -1;
			for (int i = 0; i < fieldHeaders.Length; i++)
			{
				string text = fieldHeaders[i].ToLower();
				if (text == "translation")
				{
					num2 = i;
				}
				if (text == "key")
				{
					num = i;
				}
			}
			if (num == -1 || num2 == -1)
			{
				return;
			}
			int num3 = Math.Max(num, num2) + 1;
			while (val.Read())
			{
				string[] currentRecord = val.CurrentRecord;
				if (currentRecord.Length >= num3)
				{
					string text2 = currentRecord[num];
					string value = currentRecord[num2];
					if (!string.IsNullOrWhiteSpace(text2) && !string.IsNullOrWhiteSpace(value))
					{
						UpdateTextValue(text2, value);
					}
				}
			}
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	public void LoadLanguageFromFileTextJson(string fileText, bool canCreateCategories)
	{
		foreach (KeyValuePair<string, Dictionary<string, string>> item in JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, string>>>(fileText))
		{
			_ = item.Key;
			foreach (KeyValuePair<string, string> item2 in item.Value)
			{
				string key = item.Key + "." + item2.Key;
				if (!UpdateTextValue(key, item2.Value) & canCreateCategories)
				{
					_localizedTexts.Add(key, new LocalizedText(key, item2.Value));
					if (!_categoryGroupedKeys.TryGetValue(item.Key, out var value))
					{
						_categoryGroupedKeys.Add(item.Key, value = new List<string>());
					}
					value.Add(item2.Key);
				}
			}
		}
	}

	private bool UpdateTextValue(string key, string value)
	{
		if (key.Contains('$'))
		{
			string[] array = key.Split('$');
			AddVariant(array[0], array[1], value);
			return true;
		}
		if (_localizedTexts.TryGetValue(key, out var value2))
		{
			value2.SetValue(value);
			return true;
		}
		return false;
	}

	public bool HotReloadContentFile(IContentSource contentSource, string path, string fullPath)
	{
		path = path.Replace('\\', '/');
		if (!path.StartsWith("Localization/") || !File.Exists(fullPath))
		{
			return false;
		}
		string text = File.ReadAllText(fullPath);
		if (path.EndsWith(".json"))
		{
			JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, string>>>(text);
		}
		else if (!path.EndsWith(".csv"))
		{
			return false;
		}
		if (contentSource == null)
		{
			return false;
		}
		ReloadLanguage(ActiveCulture);
		return true;
	}

	[Conditional("DEBUG")]
	private void ValidateAllCharactersContainedInFont(DynamicSpriteFont font)
	{
		if (font == null)
		{
			return;
		}
		string text = "";
		foreach (LocalizedText value2 in _localizedTexts.Values)
		{
			string value = value2.Value;
			for (int i = 0; i < value.Length; i++)
			{
				char c = value[i];
				if (!font.IsCharacterSupported(c))
				{
					text = text + value2.Key + ", " + c.ToString() + ", " + (int)c + "\n";
				}
			}
		}
	}

	public LocalizedText[] FindAll(Regex regex)
	{
		int num = 0;
		foreach (KeyValuePair<string, LocalizedText> localizedText in _localizedTexts)
		{
			if (regex.IsMatch(localizedText.Key))
			{
				num++;
			}
		}
		LocalizedText[] array = new LocalizedText[num];
		int num2 = 0;
		foreach (KeyValuePair<string, LocalizedText> localizedText2 in _localizedTexts)
		{
			if (regex.IsMatch(localizedText2.Key))
			{
				array[num2] = localizedText2.Value;
				num2++;
			}
		}
		return array;
	}

	public LocalizedText[] FindAll(LanguageSearchFilter filter)
	{
		LinkedList<LocalizedText> linkedList = new LinkedList<LocalizedText>();
		foreach (KeyValuePair<string, LocalizedText> localizedText in _localizedTexts)
		{
			if (filter(localizedText.Key, localizedText.Value))
			{
				linkedList.AddLast(localizedText.Value);
			}
		}
		return linkedList.ToArray();
	}

	public LocalizedText SelectRandom(LanguageSearchFilter filter, UnifiedRandom random = null)
	{
		int num = 0;
		foreach (KeyValuePair<string, LocalizedText> localizedText in _localizedTexts)
		{
			if (filter(localizedText.Key, localizedText.Value))
			{
				num++;
			}
		}
		int num2 = (random ?? Main.rand).Next(num);
		foreach (KeyValuePair<string, LocalizedText> localizedText2 in _localizedTexts)
		{
			if (filter(localizedText2.Key, localizedText2.Value) && --num == num2)
			{
				return localizedText2.Value;
			}
		}
		return LocalizedText.Empty;
	}

	public LocalizedText RandomFromCategory(string categoryName, UnifiedRandom random = null)
	{
		if (!_categoryGroupedKeys.TryGetValue(categoryName, out var value))
		{
			return new LocalizedText(categoryName + ".RANDOM", categoryName + ".RANDOM");
		}
		return GetText(categoryName + "." + value[(random ?? Main.rand).Next(value.Count)]);
	}

	public LocalizedText IndexedFromCategory(string categoryName, int index)
	{
		if (!_categoryGroupedKeys.TryGetValue(categoryName, out var value))
		{
			return new LocalizedText(categoryName + ".INDEXED", categoryName + ".INDEXED");
		}
		int index2 = index % value.Count;
		return GetText(categoryName + "." + value[index2]);
	}

	public bool Exists(string key)
	{
		return _localizedTexts.ContainsKey(key);
	}

	public LocalizedText GetText(string key)
	{
		if (_localizedTexts.TryGetValue(key, out var value))
		{
			return value;
		}
		return _localizedTexts[key] = new LocalizedText(key, key);
	}

	public string GetTextValue(string key)
	{
		if (_localizedTexts.TryGetValue(key, out var value))
		{
			return value.Value;
		}
		return key;
	}

	public string GetTextValue(string key, object arg0)
	{
		if (_localizedTexts.TryGetValue(key, out var value))
		{
			return value.Format(arg0);
		}
		return key;
	}

	public string GetTextValue(string key, object arg0, object arg1)
	{
		if (_localizedTexts.TryGetValue(key, out var value))
		{
			return value.Format(arg0, arg1);
		}
		return key;
	}

	public string GetTextValue(string key, object arg0, object arg1, object arg2)
	{
		if (_localizedTexts.TryGetValue(key, out var value))
		{
			return value.Format(arg0, arg1, arg2);
		}
		return key;
	}

	public string GetTextValue(string key, params object[] args)
	{
		if (_localizedTexts.TryGetValue(key, out var value))
		{
			return value.Format(args);
		}
		return key;
	}

	private void AddVariant(string key, string variant, string value)
	{
		if (!_textVariations.TryGetValue(key, out var value2))
		{
			value2 = (_textVariations[key] = new Dictionary<string, string>());
		}
		value2[variant] = value;
	}

	public bool TryGetVariation(string key, string variant, out string value)
	{
		value = null;
		if (_textVariations.TryGetValue(key, out var value2))
		{
			return value2.TryGetValue(variant, out value);
		}
		return false;
	}

	public void SetFallbackCulture(GameCulture culture)
	{
		_fallbackCulture = culture;
	}
}
