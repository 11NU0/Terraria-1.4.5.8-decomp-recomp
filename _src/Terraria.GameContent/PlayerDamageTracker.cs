using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria.DataStructures;
using Terraria.Localization;
using Terraria.Testing;

namespace Terraria.GameContent;

public class PlayerDamageTracker
{
	private class CreditEntry : IComparable<CreditEntry>
	{
		public readonly string Name;

		public int Damage { get; set; }

		public int HitCount { get; set; }

		public CreditEntry(string name)
		{
			Name = name;
		}

		public int CompareTo(CreditEntry other)
		{
			int num = 0;
			if (num == 0)
			{
				num = -Damage.CompareTo(other.Damage);
			}
			if (num == 0)
			{
				num = -HitCount.CompareTo(other.HitCount);
			}
			return num;
		}
	}

	private readonly List<CreditEntry> _list = new List<CreditEntry>();

	public void Reset()
	{
		_list.Clear();
	}

	public void AddDamage(PlayerDeathReason damageSource, int damage)
	{
		if (damage > 0 && DebugOptions.PracticeMode)
		{
			CreditEntry orAddEntry = GetOrAddEntry(GetSourceName(damageSource));
			orAddEntry.Damage += damage;
			orAddEntry.HitCount++;
		}
	}

	private CreditEntry GetOrAddEntry(string name)
	{
		foreach (CreditEntry item in _list)
		{
			if (item.Name == name)
			{
				return item;
			}
		}
		CreditEntry creditEntry = new CreditEntry(name);
		_list.Add(creditEntry);
		return creditEntry;
	}

	private static string GetSourceName(PlayerDeathReason source)
	{
		if (source.TryGetCausingEntity(out var entity))
		{
			if (entity is NPC)
			{
				return ((NPC)entity).GetGivenOrTypeNetName().ToString();
			}
			if (entity is Projectile)
			{
				return ((Projectile)entity).Name;
			}
			if (entity is Player)
			{
				return ((Player)entity).name;
			}
		}
		if (source.SourceOtherIndex.HasValue)
		{
			return "Other (" + source.SourceOtherIndex + ")";
		}
		if (source.CustomReason != null)
		{
			return source.CustomReason;
		}
		return "Unknown";
	}

	public NetworkText GetReport(PlayerDeathReason lethalDamageSource = null)
	{
		if (_list.Count == 0)
		{
			return NetworkText.Empty;
		}
		_list.Sort();
		int[] array = _list.Select((CreditEntry x) => x.Damage).ToArray();
		int[] array2 = NPCDamageTracker.CalculatePercentages(array);
		int length = array.Max().ToString().Length;
		List<string> list = new List<string>(_list.Count + 1);
		StringBuilder stringBuilder = new StringBuilder();
		if (lethalDamageSource != null)
		{
			stringBuilder.Append("Lethal damage dealt by {0}\n");
			list.Add(GetSourceName(lethalDamageSource));
		}
		stringBuilder.Append("Damage breakdown:");
		for (int num = 0; num < _list.Count; num++)
		{
			StringBuilder stringBuilder2 = new StringBuilder();
			stringBuilder2.Append(array2[num]).Append('%');
			while (stringBuilder2.Length < 6)
			{
				stringBuilder2.Append(' ');
			}
			stringBuilder2.Append(array[num]);
			while (stringBuilder2.Length < 8 + length)
			{
				stringBuilder2.Append(' ');
			}
			CreditEntry creditEntry = _list[num];
			stringBuilder2.Append('{').Append(list.Count).Append('}');
			list.Add(creditEntry.Name);
			stringBuilder2.Append(" - ").Append(creditEntry.HitCount).Append(" hits");
			stringBuilder.Append('\n').Append(stringBuilder2);
		}
		string text = stringBuilder.ToString();
		object[] substitutions = list.ToArray();
		return NetworkText.FromFormattable(text, substitutions);
	}
}
