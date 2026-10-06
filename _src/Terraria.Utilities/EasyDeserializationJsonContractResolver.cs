using System;
using System.ComponentModel;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Terraria.Utilities;

public class EasyDeserializationJsonContractResolver : DefaultContractResolver
{
	protected override JsonContract CreateContract(Type objectType)
	{
		JsonContract val = base.CreateContract(objectType);
		if (val is JsonStringContract && objectType != typeof(string))
		{
			TypeConverter converter = TypeDescriptor.GetConverter(objectType);
			if (converter != null && converter.CanConvertTo(typeof(string)) && !converter.CanConvertFrom(typeof(string)))
			{
				val = (JsonContract)(object)base.CreateObjectContract(objectType);
			}
		}
		if (objectType.IsArray || objectType.IsValueType)
		{
			val.IsReference = false;
		}
		return val;
	}

	protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		JsonProperty val = base.CreateProperty(member, memberSerialization);
		if (!val.Writable)
		{
			val.Ignored = true;
		}
		return val;
	}
}
