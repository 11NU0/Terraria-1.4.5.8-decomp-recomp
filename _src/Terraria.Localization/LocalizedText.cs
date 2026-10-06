using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Terraria.Localization;

public class LocalizedText
{
	public static readonly LocalizedText Empty = new LocalizedText("", "");

	public readonly string Key;

	private object _value;

	private static Dictionary<Type, PropertyDescriptorCollection> _propertyLookupCache = new Dictionary<Type, PropertyDescriptorCollection>();

	public string Value
	{
		get
		{
			if (_value is VariableText variableText && variableText.TryFormat(Lang.GetGlobalSubstitution, out var formatted))
			{
				return formatted;
			}
			return _value.ToString();
		}
	}

	public string UnformattedValue => _value.ToString();

	public string EnglishValue { get; private set; }

	public bool HasValue => EnglishValue != Key;

	public bool ConditionsMet
	{
		get
		{
			if (_value is VariableText variableText)
			{
				return variableText.ConditionsMet(Lang.GetGlobalSubstitution);
			}
			return true;
		}
	}

	internal LocalizedText(string key, string text)
	{
		Key = key;
		SetValue(text);
	}

	internal void SetValue(string value)
	{
		if (value == null)
		{
			throw new ArgumentNullException("value");
		}
		_value = value;
		if (LanguageManager.Instance != null && LanguageManager.Instance.ActiveCulture == GameCulture.DefaultCulture)
		{
			EnglishValue = value;
		}
		if (VariableText.TryCreate(value, out var text))
		{
			_value = text;
		}
	}

	public bool GetValueIfConditionsMet(out string formatted)
	{
		if (_value is VariableText variableText)
		{
			return variableText.TryFormat(Lang.GetGlobalSubstitution, out formatted);
		}
		formatted = Value;
		return true;
	}

	public bool TryFormatWith(object obj, out string formatted)
	{
		if (_value is VariableText variableText)
		{
			return variableText.TryFormat(GetPropertyLookupFunc(obj), out formatted);
		}
		formatted = Value;
		return true;
	}

	public bool TryFormatWith(Func<string, object> lookup, out string formatted)
	{
		if (_value is VariableText variableText)
		{
			return variableText.TryFormat(lookup, out formatted);
		}
		formatted = Value;
		return true;
	}

	public string FormatWith(object obj)
	{
		if (!TryFormatWith(obj, out var formatted))
		{
			return Value;
		}
		return formatted;
	}

	public string FormatWith(Func<string, object> lookup)
	{
		if (!TryFormatWith(lookup, out var formatted))
		{
			return Value;
		}
		return formatted;
	}

	public bool ConditionsMetWith(object obj)
	{
		if (_value is VariableText variableText)
		{
			return variableText.ConditionsMet(GetPropertyLookupFunc(obj));
		}
		return true;
	}

	public bool ConditionsMetWith(Func<string, object> lookup)
	{
		if (_value is VariableText variableText)
		{
			return variableText.ConditionsMet(lookup);
		}
		return true;
	}

	public NetworkText ToNetworkText()
	{
		return NetworkText.FromKey(Key);
	}

	public NetworkText ToNetworkText(params object[] substitutions)
	{
		return NetworkText.FromKey(Key, substitutions);
	}

	public static explicit operator string(LocalizedText text)
	{
		return text.Value;
	}

	public string Format(object arg0)
	{
		return string.Format(Value, arg0);
	}

	public string Format(object arg0, object arg1)
	{
		return string.Format(Value, arg0, arg1);
	}

	public string Format(object arg0, object arg1, object arg2)
	{
		return string.Format(Value, arg0, arg1, arg2);
	}

	public string Format(params object[] args)
	{
		return string.Format(Value, args);
	}

	public override string ToString()
	{
		return Value;
	}

	public bool EqualsCommand(string text)
	{
		text = text.ToLower();
		if (!(text == Value))
		{
			return text == EnglishValue;
		}
		return true;
	}

	public bool ParseCommandPrefix(string text, out string remainder)
	{
		if (!Utils.ParseCommandPrefix(text, Value, out remainder))
		{
			return Utils.ParseCommandPrefix(text, EnglishValue, out remainder);
		}
		return true;
	}

	private static Func<string, object> GetPropertyLookupFunc(object inst)
	{
		Type type = inst.GetType();
		if (!_propertyLookupCache.TryGetValue(type, out var properties))
		{
			_propertyLookupCache[type] = (properties = TypeDescriptor.GetProperties(type));
		}
		return (string name) =>
		{
			PropertyDescriptor propertyDescriptor = properties[name];
			return (propertyDescriptor != null) ? propertyDescriptor.GetValue(inst) : Lang.GetGlobalSubstitution(name);
		};
	}
}
