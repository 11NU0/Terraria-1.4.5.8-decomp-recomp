#define TRACE
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Terraria.Testing;
using Terraria.Utilities;

namespace Terraria.WorldBuilding;

public class WorldGenSnapshot
{
	[JsonConverter(typeof(SnapshotGenVars))]
	private class SnapshotGenVars : JsonConverter
	{
		public static JsonSerializerSettings SerializerSettings = new JsonSerializerSettings
		{
			ContractResolver = (IContractResolver)(object)new EasyDeserializationJsonContractResolver(),
			PreserveReferencesHandling = (PreserveReferencesHandling)1,
			ReferenceLoopHandling = (ReferenceLoopHandling)2,
			TypeNameHandling = (TypeNameHandling)4
		};

		private static Dictionary<string, MemberInfo> fieldsAndProperties = (from m in ((IEnumerable<MemberInfo>)typeof(GenVars).GetFields(BindingFlags.Static | BindingFlags.Public)).Concat((IEnumerable<MemberInfo>)typeof(GenVars).GetProperties(BindingFlags.Static | BindingFlags.Public))
			where !(m is PropertyInfo) || ((PropertyInfo)m).CanWrite
			where !(m is FieldInfo) || !((FieldInfo)m).IsInitOnly
			where !m.GetCustomAttributes(typeof(JsonIgnoreAttribute), inherit: true).Any()
			select m).ToDictionary((MemberInfo m) => m.Name);

		public static string Serialize()
		{
			return JsonConvert.SerializeObject((object)new SnapshotGenVars(), SerializerSettings);
		}

		public static void DeserializeAndApply(string json)
		{
			JsonConvert.DeserializeObject<SnapshotGenVars>(json, SerializerSettings);
		}

		public override bool CanConvert(Type objectType)
		{
			return objectType == typeof(SnapshotGenVars);
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Invalid comparison between Unknown and I4
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Invalid comparison between Unknown and I4
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Invalid comparison between Unknown and I4
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			if ((int)reader.TokenType != 1)
			{
				throw new JsonReaderException();
			}
			while (reader.Read() && (int)reader.TokenType != 13)
			{
				if ((int)reader.TokenType != 4)
				{
					throw new JsonReaderException("Expected PropertyName");
				}
				string key = (string)reader.Value;
				if (!reader.Read())
				{
					throw new JsonReaderException();
				}
				if (fieldsAndProperties.TryGetValue(key, out var value))
				{
					SetValue(value, serializer.Deserialize(reader, GetType(value)));
				}
				else
				{
					reader.Skip();
				}
			}
			return null;
		}

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			writer.WriteStartObject();
			foreach (MemberInfo value2 in fieldsAndProperties.Values)
			{
				writer.WritePropertyName(value2.Name);
				serializer.Serialize(writer, GetValue(value2), GetType(value2));
			}
			writer.WriteEndObject();
		}

		private Type GetType(MemberInfo member)
		{
			if (member is PropertyInfo)
			{
				return ((PropertyInfo)member).PropertyType;
			}
			if (member is FieldInfo)
			{
				return ((FieldInfo)member).FieldType;
			}
			throw new ArgumentException(member.GetType().ToString());
		}

		private object GetValue(MemberInfo member)
		{
			if (member is PropertyInfo)
			{
				return ((PropertyInfo)member).GetGetMethod().Invoke(null, null);
			}
			if (member is FieldInfo)
			{
				return ((FieldInfo)member).GetValue(null);
			}
			throw new ArgumentException(member.GetType().ToString());
		}

		private void SetValue(MemberInfo member, object v)
		{
			if (member is PropertyInfo)
			{
				((PropertyInfo)member).GetSetMethod().Invoke(null, new object[1] { v });
				return;
			}
			if (member is FieldInfo)
			{
				((FieldInfo)member).SetValue(null, v);
				return;
			}
			throw new ArgumentException(member.GetType().ToString());
		}

		static SnapshotGenVars()
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Expected Obj, but got Unknown
		}
	}

	private int _dataOffset;

	private List<GenPass> _matchingPasses;

	private static string SnapshotFolderSuffix = "_gensnapshots";

	private static string Extension = ".gensnapshot";

	private static IDictionary<string, long> _snapshotSizeCache = new Dictionary<string, long>();

	public WorldManifest Manifest { get; private set; }

	private string Path { get; set; }

	private string GenVarsJson { get; set; }

	public List<GenPassResult> GenPassResults => Manifest.GenPassResults;

	public bool Outdated
	{
		get
		{
			if (!(Manifest.GitSHA != GitStatus.GitSHA) && !(Manifest.Version != Main.versionNumber))
			{
				return !_matchingPasses.Zip(GenPassResults, (GenPass p, GenPassResult r) => p.Enabled == !r.Skipped).All((bool x) => x);
			}
			return true;
		}
	}

	private static string PathForActiveWorld => System.IO.Path.ChangeExtension(Main.ActiveWorldFileData.Path, null) + SnapshotFolderSuffix;

	public static long EstimatedDiskUsage => _snapshotSizeCache.Values.Sum();

	public override string ToString()
	{
		GenPassResult genPassResult = GenPassResults.Last();
		return $"Pass - {genPassResult.Name}, rand - {genPassResult.RandNext:X8}, hash - {genPassResult.Hash:X8}";
	}

	public static void DeleteAllForCurrentWorld()
	{
		if (Directory.Exists(PathForActiveWorld))
		{
			try
			{
				Directory.Delete(PathForActiveWorld, recursive: true);
			}
			catch (Exception)
			{
			}
		}
		_snapshotSizeCache.Clear();
	}

	public static WorldGenSnapshot Create()
	{
		WorldGenSnapshot worldGenSnapshot = new WorldGenSnapshot
		{
			Manifest = WorldGen.Manifest.Clone(),
			GenVarsJson = SnapshotGenVars.Serialize()
		};
		worldGenSnapshot._matchingPasses = WorldGenerator.CurrentController.Passes.GetRange(0, worldGenSnapshot.GenPassResults.Count);
		worldGenSnapshot.Path = System.IO.Path.Combine(PathForActiveWorld, string.Concat(worldGenSnapshot, Extension));
		if (!Directory.Exists(PathForActiveWorld))
		{
			Directory.CreateDirectory(PathForActiveWorld);
		}
		TileSnapshot.Create(worldGenSnapshot);
		using BinaryWriter binaryWriter = new BinaryWriter(File.Create(worldGenSnapshot.Path));
		binaryWriter.Write(worldGenSnapshot.Manifest.Serialize());
		binaryWriter.Write(worldGenSnapshot.GenVarsJson);
		worldGenSnapshot._dataOffset = (int)binaryWriter.BaseStream.Position;
		TileSnapshot.Save(binaryWriter);
		_snapshotSizeCache[worldGenSnapshot.Path] = binaryWriter.BaseStream.Length;
		return worldGenSnapshot;
	}

	public static void Delete(WorldGenSnapshot snap)
	{
		try
		{
			File.Delete(snap.Path);
		}
		catch (Exception)
		{
		}
		_snapshotSizeCache.Remove(snap.Path);
	}

	public void ResaveForCurrentVersion()
	{
		Manifest = WorldGen.Manifest.Clone();
		GenVarsJson = SnapshotGenVars.Serialize();
		using MemoryStream memoryStream = new MemoryStream();
		using (FileStream fileStream = File.OpenRead(Path))
		{
			fileStream.Seek(_dataOffset, SeekOrigin.Current);
			fileStream.CopyTo(memoryStream);
		}
		memoryStream.Position = 0L;
		using BinaryWriter binaryWriter = new BinaryWriter(File.Create(Path));
		binaryWriter.Write(Manifest.Serialize());
		binaryWriter.Write(GenVarsJson);
		_dataOffset = (int)binaryWriter.BaseStream.Position;
		memoryStream.CopyTo(binaryWriter.BaseStream);
		_snapshotSizeCache[Path] = binaryWriter.BaseStream.Length;
	}

	public static Dictionary<GenPass, WorldGenSnapshot> LoadSnapshots(WorldManifest worldManifest, List<GenPass> passes)
	{
		_snapshotSizeCache.Clear();
		Dictionary<GenPass, WorldGenSnapshot> dictionary = new Dictionary<GenPass, WorldGenSnapshot>();
		Task.Factory.StartNew(() =>
		{
			DeleteSnapshotsForOtherWorlds(PathForActiveWorld);
		});
		if (!Directory.Exists(PathForActiveWorld))
		{
			return dictionary;
		}
		if (worldManifest == null)
		{
			Trace.WriteLine("Deleting old snapshots because a new world is being created (/regen was not used)");
			DeleteAllForCurrentWorld();
			return dictionary;
		}
		foreach (string item in Directory.EnumerateFiles(PathForActiveWorld, "*" + Extension))
		{
			if (ReadSnapshot(item, out var snap) && snap.IsValidHistoryOf(worldManifest) && FindMatchingGenPass(snap.Manifest, passes, out var pass))
			{
				snap._matchingPasses = passes.GetRange(0, snap.GenPassResults.Count);
				dictionary[pass] = snap;
			}
			else
			{
				Trace.WriteLine($"Deleting snapshot ({snap}) due to manifest mismatch. A change to the codebase has probably invalidated it.");
				Delete(snap);
			}
		}
		return dictionary;
	}

	private static void DeleteSnapshotsForOtherWorlds(string snapshotPathForActiveWorld)
	{
		string directoryName = System.IO.Path.GetDirectoryName(snapshotPathForActiveWorld);
		string fileName = System.IO.Path.GetFileName(snapshotPathForActiveWorld);
		foreach (string item in Directory.EnumerateDirectories(directoryName))
		{
			if (item.EndsWith(SnapshotFolderSuffix) && !(System.IO.Path.GetFileName(item) == fileName))
			{
				Trace.WriteLine("Deleting snapshot directory: " + item);
				try
				{
					Directory.Delete(item, recursive: true);
				}
				catch (Exception)
				{
				}
			}
		}
	}

	private static bool FindMatchingGenPass(WorldManifest manifest, List<GenPass> passes, out GenPass pass)
	{
		pass = null;
		List<GenPassResult> genPassResults = manifest.GenPassResults;
		if (genPassResults.Count > passes.Count || !genPassResults.Zip(passes, (GenPassResult r, GenPass p) => r.Name == p.Name).All((bool x) => x))
		{
			return false;
		}
		pass = passes[genPassResults.Count - 1];
		return true;
	}

	public bool IsValidHistoryOf(WorldManifest target)
	{
		return StartsWith(target.GenPassResults, Manifest.GenPassResults);
	}

	private static bool StartsWith(List<GenPassResult> list, List<GenPassResult> prefix)
	{
		if (prefix.Count <= list.Count)
		{
			return list.Zip(prefix, (GenPassResult a, GenPassResult b) => a.Matches(b)).All((bool x) => x);
		}
		return false;
	}

	private static bool ReadSnapshot(string path, out WorldGenSnapshot snap)
	{
		try
		{
			using BinaryReader binaryReader = new BinaryReader(File.OpenRead(path));
			snap = new WorldGenSnapshot
			{
				Path = path,
				Manifest = WorldManifest.Deserialize(binaryReader.ReadString()),
				GenVarsJson = binaryReader.ReadString(),
				_dataOffset = (int)binaryReader.BaseStream.Position
			};
			_snapshotSizeCache[path] = binaryReader.BaseStream.Length;
			return true;
		}
		catch (Exception ex)
		{
			Trace.WriteLine("Failed to read snapshot: " + path + ", " + ex);
			snap = null;
			return false;
		}
	}

	public void Load()
	{
		if (TileSnapshot.Context == this)
		{
			return;
		}
		using BinaryReader binaryReader = new BinaryReader(File.OpenRead(Path));
		binaryReader.BaseStream.Seek(_dataOffset, SeekOrigin.Current);
		TileSnapshot.Load(binaryReader, this);
	}

	public void Restore()
	{
		Load();
		WorldGen.RestoreTemporaryStateChanges();
		WorldGen.Reset();
		WorldGen.Manifest = Manifest.Clone();
		SnapshotGenVars.DeserializeAndApply(GenVarsJson);
		TileSnapshot.Restore();
		NPC[] npc = Main.npc;
		for (int i = 0; i < npc.Length; i++)
		{
			npc[i].active = false;
		}
		Main.NewText("Restored " + this, byte.MaxValue, byte.MaxValue, 0);
	}
}
