using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ReLogic.Utilities;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.Localization;
using Terraria.Utilities;
using Terraria.WorldBuilding;

namespace Terraria.IO;

public class WorldFileData : FileData
{
	private const ulong GUID_IN_WORLD_FILE_VERSION = 777389080577uL;

	public static readonly int MAX_USER_SEED_TEXT_LENGTH = 40;

	public DateTime CreationTime;

	public DateTime LastPlayed;

	public int WorldSizeX;

	public int WorldSizeY;

	public ulong WorldGeneratorVersion;

	private string _seedText = "";

	private int _seed;

	public int LoadStatus = StatusID.Ok;

	public Exception LoadException;

	public Guid UniqueId;

	public int WorldId;

	public LocalizedText _worldSizeName;

	public int GameMode;

	public bool DrunkWorld;

	public bool NotTheBees;

	public bool ForTheWorthy;

	public bool Anniversary;

	public bool DontStarve;

	public bool RemixWorld;

	public bool NoTrapsWorld;

	public bool ZenithWorld;

	public bool SkyblockWorld;

	public bool HasCorruption = true;

	public bool IsHardMode;

	public bool DefeatedMoonlord;

	private static List<AWorldGenerationOption> seedOptionsInOrder = new List<AWorldGenerationOption>
	{
		WorldGenerationOptions.Get<WorldSeedOption_Drunk>(),
		WorldGenerationOptions.Get<WorldSeedOption_NotTheBees>(),
		WorldGenerationOptions.Get<WorldSeedOption_ForTheWorthy>(),
		WorldGenerationOptions.Get<WorldSeedOption_Anniversary>(),
		WorldGenerationOptions.Get<WorldSeedOption_DontStarve>(),
		WorldGenerationOptions.Get<WorldSeedOption_Remix>(),
		WorldGenerationOptions.Get<WorldSeedOption_NoTraps>(),
		WorldGenerationOptions.Get<WorldSeedOption_Everything>(),
		WorldGenerationOptions.Get<WorldSeedOption_Skyblock>()
	};

	public string SeedText => _seedText;

	public int Seed => _seed;

	public bool IsValid => LoadStatus == StatusID.Ok;

	public string WorldSizeName => _worldSizeName.Value;

	public bool HasCrimson
	{
		get
		{
			return !HasCorruption;
		}
		set
		{
			HasCorruption = !value;
		}
	}

	public bool HasValidSeed => WorldGeneratorVersion != 0;

	public bool UseGuidAsMapName => WorldGeneratorVersion >= 777389080577L;

	public string MapFileName
	{
		get
		{
			if (!UseGuidAsMapName)
			{
				return WorldId.ToString();
			}
			return UniqueId.ToString();
		}
	}

	public string GetWorldName(bool allowCropping = false)
	{
		string text = Name;
		if (text == null)
		{
			return text;
		}
		if (allowCropping)
		{
			int num = 494;
			text = FontAssets.MouseText.Value.CreateCroppedText(text, (float)num);
		}
		return text;
	}

	public string GetFullSeedText(bool allowCropping = false)
	{
		int num = 0;
		if (WorldSizeX == 4200 && WorldSizeY == 1200)
		{
			num = 1;
		}
		if (WorldSizeX == 6400 && WorldSizeY == 1800)
		{
			num = 2;
		}
		if (WorldSizeX == 8400 && WorldSizeY == 2400)
		{
			num = 3;
		}
		int num2 = 0;
		if (HasCorruption)
		{
			num2 = 1;
		}
		if (HasCrimson)
		{
			num2 = 2;
		}
		int num3 = GameMode + 1;
		string text = _seedText;
		if (allowCropping)
		{
			int num4 = 340;
			text = FontAssets.MouseText.Value.CreateCroppedText(text, (float)num4);
		}
		int serializedSeedsSum = GetSerializedSeedsSum();
		return $"{num}.{num3}.{num2}.{serializedSeedsSum}.{text}";
	}

	public int GetSerializedSeedsSum()
	{
		int num = 0;
		if (DrunkWorld)
		{
			num++;
		}
		if (NotTheBees)
		{
			num += 2;
		}
		if (ForTheWorthy)
		{
			num += 4;
		}
		if (Anniversary)
		{
			num += 8;
		}
		if (DontStarve)
		{
			num += 16;
		}
		if (RemixWorld)
		{
			num += 32;
		}
		if (NoTrapsWorld)
		{
			num += 64;
		}
		if (ZenithWorld)
		{
			num += 128;
		}
		if (SkyblockWorld)
		{
			num += 256;
		}
		return num;
	}

	public List<string> GetSecretSeedCodes()
	{
		if (string.IsNullOrWhiteSpace(_seedText))
		{
			return new List<string>();
		}
		return _seedText.Split('|').ToList();
	}

	private static void EnableSeedOptions(int serializedSeedSum)
	{
		for (int i = 0; i < seedOptionsInOrder.Count; i++)
		{
			if (((serializedSeedSum >> i) & 1) == 1)
			{
				seedOptionsInOrder[i].Enabled = true;
			}
		}
	}

	public static bool TryApplyingCopiedSeed(string input, bool playSound, out string processedSeed, out string seedTextIncludingSecrets, out List<string> secretSeedTexts)
	{
		processedSeed = input;
		seedTextIncludingSecrets = input;
		secretSeedTexts = null;
		if (string.IsNullOrWhiteSpace(input))
		{
			return false;
		}
		if (!TryParseSeedOptionValue(ref processedSeed, out var value) || !TryParseSeedOptionValue(ref processedSeed, out var value2) || !TryParseSeedOptionValue(ref processedSeed, out var value3))
		{
			return false;
		}
		if (value <= 0 || value > 3)
		{
			return false;
		}
		if (value2 <= 0 || value2 > 4)
		{
			return false;
		}
		if (value3 <= 0 || value3 > 2)
		{
			return false;
		}
		if (!TryParseSeedOptionValue(ref processedSeed, out var value4))
		{
			value4 = 0;
		}
		seedTextIncludingSecrets = processedSeed;
		secretSeedTexts = new List<string>();
		List<WorldGen.SecretSeed> list = new List<WorldGen.SecretSeed>();
		string secretSeedText;
		WorldGen.SecretSeed secretSeed;
		while (TryParseSecretSeed(ref processedSeed, out secretSeedText, out secretSeed))
		{
			secretSeedTexts.Add(secretSeedText);
			list.Add(secretSeed);
		}
		if (processedSeed.Length > MAX_USER_SEED_TEXT_LENGTH)
		{
			return false;
		}
		WorldGen.SetWorldSize(value - 1);
		Main.GameMode = value2 - 1;
		WorldGen.WorldGenParam_Evil = value3 - 1;
		WorldGenerationOptions.Reset();
		EnableSeedOptions(value4);
		WorldGen.SecretSeed.ClearAllSeeds();
		foreach (WorldGen.SecretSeed item in list)
		{
			WorldGen.SecretSeed.Enable(item, playSound);
			playSound = false;
		}
		return true;
	}

	private static bool TryParseSeedOptionValue(ref string processedSeed, out int value)
	{
		int num = processedSeed.IndexOf('.');
		if (num < 0)
		{
			value = 0;
			return false;
		}
		if (!int.TryParse(processedSeed.Substring(0, num), out value))
		{
			return false;
		}
		processedSeed = processedSeed.Substring(num + 1);
		return true;
	}

	private static bool TryParseSecretSeed(ref string processedSeed, out string secretSeedText, out WorldGen.SecretSeed secretSeed)
	{
		int num = processedSeed.IndexOf('|');
		if (num < 0)
		{
			secretSeedText = null;
			secretSeed = null;
			return false;
		}
		secretSeedText = processedSeed.Substring(0, num);
		if (!WorldGen.SecretSeed.CheckInputForSecretSeed(secretSeedText, out secretSeed))
		{
			return false;
		}
		processedSeed = processedSeed.Substring(num + 1);
		return true;
	}

	public WorldFileData()
		: base("World")
	{
	}

	public WorldFileData(string path, bool cloudSave)
		: base("World", path, cloudSave)
	{
	}

	public override void SetAsActive()
	{
		if (LoadException != null)
		{
			throw LoadException;
		}
		Main.ActiveWorldFileData = this;
	}

	public void SetWorldSize(int x, int y)
	{
		WorldSizeX = x;
		WorldSizeY = y;
		switch (x)
		{
		case 4200:
			_worldSizeName = Language.GetText("UI.WorldSizeSmall");
			break;
		case 6400:
			_worldSizeName = Language.GetText("UI.WorldSizeMedium");
			break;
		case 8400:
			_worldSizeName = Language.GetText("UI.WorldSizeLarge");
			break;
		default:
			_worldSizeName = Language.GetText("UI.WorldSizeUnknown");
			break;
		}
	}

	public static WorldFileData FromInvalidWorld(string path, bool cloudSave, int statusCode, Exception exception)
	{
		WorldFileData worldFileData = new WorldFileData(path, cloudSave);
		worldFileData.GameMode = 0;
		worldFileData.SetSeedToEmpty();
		worldFileData.WorldGeneratorVersion = 0uL;
		worldFileData.Metadata = FileMetadata.FromCurrentSettings(FileType.World);
		worldFileData.SetWorldSize(1, 1);
		worldFileData.HasCorruption = true;
		worldFileData.IsHardMode = false;
		worldFileData.LoadStatus = statusCode;
		worldFileData.LoadException = exception;
		worldFileData.Name = FileUtilities.GetFileName(path, includeExtension: false);
		worldFileData.UniqueId = Guid.Empty;
		if (!cloudSave)
		{
			worldFileData.CreationTime = File.GetCreationTime(path);
		}
		else
		{
			worldFileData.CreationTime = DateTime.Now;
		}
		return worldFileData;
	}

	public void SetSeedToEmpty()
	{
		SetSeed("");
	}

	public void SetSeed(string seedText)
	{
		_seedText = seedText;
		_seed = TranslateSeed(seedText);
	}

	public static int TranslateSeed(string seedText)
	{
		if (!int.TryParse(seedText, out var result))
		{
			return Crc32.Calculate(seedText);
		}
		if (result != int.MinValue)
		{
			return Math.Abs(result);
		}
		return int.MaxValue;
	}

	public void SetSeedToRandom()
	{
		SetSeed(new UnifiedRandom().Next().ToString());
	}

	public void SetSeedToRandomWithCurrentEvents()
	{
		SetSeedToRandom();
		if (Main.isHalloweenDateNow())
		{
			WorldGen.SecretSeed.Enable(WorldGen.SecretSeed.halloweenGen, playSound: false);
			SetSeed("pumpkinseason|" + SeedText);
		}
	}

	public override void MoveToCloud()
	{
		if (!IsCloudSave)
		{
			string worldPathFromName = Main.GetWorldPathFromName(Name, cloudSave: true);
			if (FileUtilities.MoveToCloud(Path, worldPathFromName))
			{
				Main.LocalFavoriteData.ClearEntry(this);
				_isCloudSave = true;
				_path = worldPathFromName;
				Main.CloudFavoritesData.SaveFavorite(this);
			}
		}
	}

	public override void MoveToLocal()
	{
		if (IsCloudSave)
		{
			string worldPathFromName = Main.GetWorldPathFromName(Name, cloudSave: false);
			if (FileUtilities.MoveToLocal(Path, worldPathFromName))
			{
				Main.CloudFavoritesData.ClearEntry(this);
				_isCloudSave = false;
				_path = worldPathFromName;
				Main.LocalFavoriteData.SaveFavorite(this);
			}
		}
	}

	public void Rename(string newDisplayName)
	{
		if (newDisplayName != null)
		{
			WorldGen.RenameWorld(this, newDisplayName, GetRenameCallback(() =>
			{
				Main.menuMode = 6;
			}));
		}
	}

	public void CopyToLocal(string newDisplayName, Action onCompleted)
	{
		if (!IsCloudSave)
		{
			string worldPathFromName = Main.GetWorldPathFromName(Guid.NewGuid().ToString(), cloudSave: false);
			FileUtilities.Copy(Path, worldPathFromName, cloud: false);
			_path = worldPathFromName;
			WorldGen.RenameWorld(this, newDisplayName, GetRenameCallback(onCompleted));
		}
	}

	private Action<string> GetRenameCallback(Action returnToMenu)
	{
		return (string newWorldName) =>
		{
			Name = newWorldName;
			Main.QueueMainThreadAction(() =>
			{
				SoundEngine.PlaySound(10);
				returnToMenu();
			});
		};
	}
}
