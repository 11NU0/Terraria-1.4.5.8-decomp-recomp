using System.IO;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Terraria.IO;

namespace Terraria.WorldBuilding;

public class WorldGenConfiguration : GameConfiguration
{
	private readonly JObject _biomeRoot;

	private readonly JObject _passRoot;

	public WorldGenConfiguration(JObject configurationRoot)
		: base(configurationRoot)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Expected Obj, but got Unknown
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Expected Obj, but got Unknown
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		JObject val = (JObject)configurationRoot["Biomes"];
		if (val == null)
		{
			val = new JObject();
		}
		_biomeRoot = val;
		JObject val2 = (JObject)configurationRoot["Passes"];
		if (val2 == null)
		{
			val2 = new JObject();
		}
		_passRoot = val2;
	}

	public T CreateBiome<T>() where T : MicroBiome, new()
	{
		return CreateBiome<T>(typeof(T).Name);
	}

	public T CreateBiome<T>(string name) where T : MicroBiome, new()
	{
		JToken val = default;
		if (_biomeRoot.TryGetValue(name, out val))
		{
			return val.ToObject<T>();
		}
		return new T();
	}

	public GameConfiguration GetPassConfiguration(string name)
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Expected Obj, but got Unknown
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Expected Obj, but got Unknown
		JToken val = default;
		if (_passRoot.TryGetValue(name, out val))
		{
			return new GameConfiguration((JObject)val);
		}
		return new GameConfiguration(new JObject());
	}

	public static WorldGenConfiguration FromEmbeddedPath(string path)
	{
		using Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(path);
		using StreamReader streamReader = new StreamReader(stream);
		return new WorldGenConfiguration(JsonConvert.DeserializeObject<JObject>(streamReader.ReadToEnd()));
	}
}
