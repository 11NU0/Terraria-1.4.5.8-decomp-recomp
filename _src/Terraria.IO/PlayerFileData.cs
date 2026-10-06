using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Terraria.Social;
using Terraria.Utilities;

namespace Terraria.IO;

public class PlayerFileData : FileData
{
	private Player _player;

	private TimeSpan _playTime = TimeSpan.Zero;

	private readonly Stopwatch _timer = new Stopwatch();

	private bool _isTimerActive;

	public Player Player
	{
		get
		{
			return _player;
		}
		set
		{
			_player = value;
			if (value != null)
			{
				Name = _player.name;
			}
		}
	}

	public bool ServerSideCharacter { get; private set; }

	public DateTime LastPlayed => DateTime.FromBinary(Player.lastTimePlayerWasSaved);

	public PlayerFileData()
		: base("Player")
	{
	}

	public PlayerFileData(string path, bool cloudSave)
		: base("Player", path, cloudSave)
	{
	}

	public static PlayerFileData CreateAndSave(Player player)
	{
		PlayerFileData playerFileData = new PlayerFileData();
		playerFileData.Metadata = FileMetadata.FromCurrentSettings(FileType.Player);
		playerFileData.Player = player;
		playerFileData._isCloudSave = SocialAPI.Cloud != null && SocialAPI.Cloud.EnabledByDefault;
		playerFileData._path = Main.GetPlayerPathFromName(player.name, playerFileData.IsCloudSave);
		(playerFileData.IsCloudSave ? Main.CloudFavoritesData : Main.LocalFavoriteData).ClearEntry(playerFileData);
		Player.SavePlayer(playerFileData, skipMapSave: true);
		return playerFileData;
	}

	public override void SetAsActive()
	{
		Main.ActivePlayerFileData = this;
		Main.player[Main.myPlayer] = Player;
	}

	public void MarkAsServerSide()
	{
		ServerSideCharacter = true;
	}

	public override void MoveToCloud()
	{
		if (IsCloudSave || SocialAPI.Cloud == null)
		{
			return;
		}
		string playerPathFromName = Main.GetPlayerPathFromName(Name, cloudSave: true);
		if (!FileUtilities.MoveToCloud(Path, playerPathFromName))
		{
			return;
		}
		string fileName = GetFileName(includeExtension: false);
		string path = Main.PlayerPath + System.IO.Path.DirectorySeparatorChar + fileName + System.IO.Path.DirectorySeparatorChar;
		string text = playerPathFromName.Substring(0, playerPathFromName.Length - 4);
		if (Directory.Exists(path))
		{
			string[] files = Directory.GetFiles(path);
			for (int i = 0; i < files.Length; i++)
			{
				string cloudPath = text + "/" + FileUtilities.GetFileName(files[i]);
				FileUtilities.MoveToCloud(files[i], cloudPath);
			}
		}
		Main.LocalFavoriteData.ClearEntry(this);
		_isCloudSave = true;
		_path = playerPathFromName;
		Main.CloudFavoritesData.SaveFavorite(this);
	}

	public override void MoveToLocal()
	{
		if (!IsCloudSave || SocialAPI.Cloud == null)
		{
			return;
		}
		string playerPathFromName = Main.GetPlayerPathFromName(Name, cloudSave: false);
		if (!FileUtilities.MoveToLocal(Path, playerPathFromName))
		{
			return;
		}
		string fileName = GetFileName(includeExtension: false);
		string fileName2 = FileUtilities.GetFileName(playerPathFromName, includeExtension: false);
		string mapPath = System.IO.Path.Combine(Main.CloudPlayerPath, fileName);
		foreach (string item in (from path in SocialAPI.Cloud.GetFiles().ToList()
			where MapBelongsToPath(mapPath, path)
			select path).ToList())
		{
			string localPath = System.IO.Path.Combine(Main.PlayerPath, fileName2, FileUtilities.GetFileName(item));
			FileUtilities.MoveToLocal(item, localPath);
		}
		Main.CloudFavoritesData.ClearEntry(this);
		_isCloudSave = false;
		_path = playerPathFromName;
		Main.LocalFavoriteData.SaveFavorite(this);
	}

	private bool MapBelongsToPath(string mapPath, string filePath)
	{
		if (!filePath.EndsWith(".map", StringComparison.CurrentCultureIgnoreCase))
		{
			return false;
		}
		string text = mapPath.Replace('\\', '/');
		if (!text.EndsWith("/"))
		{
			text += "/";
		}
		return filePath.StartsWith(text, StringComparison.CurrentCultureIgnoreCase);
	}

	public void UpdatePlayTimer()
	{
		if (FocusHelper.AllowCountingPlayerTime)
		{
			StartPlayTimer();
		}
		else
		{
			PausePlayTimer();
		}
	}

	public void StartPlayTimer()
	{
		if (!_isTimerActive)
		{
			_isTimerActive = true;
			if (!_timer.IsRunning)
			{
				_timer.Start();
			}
		}
	}

	public void PausePlayTimer()
	{
		StopPlayTimer();
	}

	public TimeSpan GetPlayTime()
	{
		if (_timer.IsRunning)
		{
			return _playTime + _timer.Elapsed;
		}
		return _playTime;
	}

	public void UpdatePlayTimerAndKeepState()
	{
		bool isRunning = _timer.IsRunning;
		_playTime += _timer.Elapsed;
		_timer.Reset();
		if (isRunning)
		{
			_timer.Start();
		}
	}

	public void StopPlayTimer()
	{
		if (_isTimerActive)
		{
			_isTimerActive = false;
			if (_timer.IsRunning)
			{
				_playTime += _timer.Elapsed;
				_timer.Reset();
			}
		}
	}

	public void SetPlayTime(TimeSpan time)
	{
		_playTime = time;
	}

	public void Rename(string newName)
	{
		if (Player != null)
		{
			Player.name = newName.Trim();
		}
		Player.SavePlayer(this);
	}
}
