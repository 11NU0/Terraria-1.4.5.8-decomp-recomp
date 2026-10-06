using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Terraria.Localization;

internal class VariableText
{
	private struct Condition
	{
		public bool RequiredValue;

		public string Name;
	}

	private readonly string _original;

	private readonly string _format;

	private readonly Condition[] _conditions;

	private readonly string[] _variables;

	private readonly object[] _formatArgBuffer;

	private readonly StringBuilder _formatBuffer = new StringBuilder();

	private static readonly Regex _substitutionRegex = new Regex("{(\\?!?)?([a-zA-Z][\\w\\.]*)}", RegexOptions.Compiled);

	private VariableText(string original, string format, Condition[] conditions, string[] variables)
	{
		_original = original;
		_format = format;
		_conditions = conditions;
		_variables = variables;
		_formatArgBuffer = new object[variables.Length];
	}

	public static bool TryCreate(string s, out VariableText text)
	{
		if (!_substitutionRegex.IsMatch(s))
		{
			text = null;
			return false;
		}
		List<string> variables = new List<string>();
		List<Condition> conditions = new List<Condition>();
		string format = _substitutionRegex.Replace(s, (Match match) =>
		{
			string text2 = match.Groups[2].ToString();
			string text3 = match.Groups[1].ToString();
			if (text3 != "")
			{
				conditions.Add(new Condition
				{
					Name = text2,
					RequiredValue = (text3 == "?")
				});
				return "";
			}
			int num = variables.IndexOf(text2);
			if (num < 0)
			{
				num = variables.Count;
				variables.Add(text2);
			}
			return "{" + num + "}";
		});
		text = new VariableText(s, format, conditions.ToArray(), variables.ToArray());
		return true;
	}

	private bool CheckConditionsAndLoadArgs(Func<string, object> lookup)
	{
		Condition[] conditions = _conditions;
		for (int i = 0; i < conditions.Length; i++)
		{
			Condition condition = conditions[i];
			if (((lookup(condition.Name) as bool?) ?? false) != condition.RequiredValue)
			{
				return false;
			}
		}
		for (int j = 0; j < _variables.Length; j++)
		{
			object obj = lookup(_variables[j]);
			if (obj == null)
			{
				return false;
			}
			_formatArgBuffer[j] = obj;
		}
		return true;
	}

	public bool ConditionsMet(Func<string, object> lookup)
	{
		return CheckConditionsAndLoadArgs(lookup);
	}

	public bool TryFormat(Func<string, object> lookup, out string formatted)
	{
		if (!CheckConditionsAndLoadArgs(lookup))
		{
			formatted = null;
			return false;
		}
		_formatBuffer.AppendFormat(_format, _formatArgBuffer);
		formatted = _formatBuffer.ToString();
		_formatBuffer.Clear();
		return true;
	}

	public override string ToString()
	{
		return _original;
	}
}
