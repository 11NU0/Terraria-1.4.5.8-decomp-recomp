#define TRACE
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Newtonsoft.Json;

namespace Terraria.WorldBuilding;

public class WorldManifest
{
	public List<GenPassResult> GenPassResults = new List<GenPassResult>();

	public static readonly JsonSerializerSettings SerializerSettings = new JsonSerializerSettings
	{
		TypeNameHandling = (TypeNameHandling)4
	};

	public string Version { get; set; }

	public string GitSHA { get; set; }

	public uint? FinalHash
	{
		get
		{
			if (GenPassResults.Count <= 0)
			{
				return null;
			}
			return GenPassResults[GenPassResults.Count - 1].Hash;
		}
	}

	public static WorldManifest Deserialize(string json)
	{
		try
		{
			if (!string.IsNullOrEmpty(json))
			{
				return JsonConvert.DeserializeObject<WorldManifest>(json, SerializerSettings);
			}
		}
		catch (Exception value)
		{
			Trace.WriteLine(value);
		}
		return new WorldManifest();
	}

	public string Serialize()
	{
		return JsonConvert.SerializeObject((object)this, SerializerSettings);
	}

	public WorldManifest Clone()
	{
		return JsonConvert.DeserializeObject<WorldManifest>(JsonConvert.SerializeObject((object)this, SerializerSettings), SerializerSettings);
	}

	static WorldManifest()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected Obj, but got Unknown
	}
}
