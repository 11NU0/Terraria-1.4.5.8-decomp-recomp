using System.Collections.Generic;
using System.IO;
using Terraria.IO;

namespace Terraria.Social.Base;

public abstract class CloudSocialModule : ISocialModule
{
	public bool EnabledByDefault;

	public virtual void BindTo(Preferences preferences)
	{
		preferences.OnSave += Configuration_OnSave;
		preferences.OnLoad += Configuration_OnLoad;
	}

	private void Configuration_OnLoad(Preferences preferences)
	{
		EnabledByDefault = preferences.Get("CloudSavingDefault", defaultValue: false);
	}

	private void Configuration_OnSave(Preferences preferences)
	{
		preferences.Put("CloudSavingDefault", EnabledByDefault);
	}

	public abstract void Initialize();

	public abstract void Shutdown();

	public abstract IEnumerable<string> GetFiles();

	public abstract bool Write(string path, byte[] data, int length);

	public abstract void Read(string path, byte[] buffer, int length);

	public abstract Stream OpenRead(string file);

	public abstract bool HasFile(string path);

	public abstract int GetFileSize(string path);

	public abstract bool Delete(string path);

	public abstract bool Forget(string path);

	public byte[] Read(string path)
	{
		byte[] array = new byte[GetFileSize(path)];
		Read(path, array, array.Length);
		return array;
	}

	public void Read(string path, byte[] buffer)
	{
		Read(path, buffer, buffer.Length);
	}

	public bool Write(string path, byte[] data)
	{
		return Write(path, data, data.Length);
	}
}
