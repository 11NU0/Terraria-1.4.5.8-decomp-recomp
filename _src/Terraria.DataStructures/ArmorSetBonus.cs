using System.Collections.Generic;
using Terraria.Localization;

namespace Terraria.DataStructures;

public class ArmorSetBonus
{
	public delegate void ArmorSetEffect(Player player);

	public enum PartType
	{
		None,
		Head,
		Body,
		Legs
	}

	public struct QueryContext(Player player)
	{
		public int HeadItem = TryGetType(player.GetEffectiveArmor(0));

		public int BodyItem = TryGetType(player.GetEffectiveArmor(1));

		public int LegItem = TryGetType(player.GetEffectiveArmor(2));

		private static int TryGetType(Item item)
		{
			return item?.type ?? 0;
		}

		public int GetPart(PartType part)
		{
			return part switch
			{
				PartType.Head => HeadItem, 
				PartType.Body => BodyItem, 
				PartType.Legs => LegItem, 
				_ => 0, 
			};
		}
	}

	public struct QueryResult
	{
		public int ItemsNeeded;

		public int ItemsFound;

		public bool Complete => ItemsNeeded == ItemsFound;
	}

	private class SetBonusDisplayStringSubstitutes
	{
		public int Numerator { get; set; }

		public int Denominator { get; set; }

		public LocalizedText Description { get; set; }
	}

	public class Builder
	{
		private struct Parts
		{
			public int Head;

			public int Body;

			public int Legs;
		}

		private ArmorSetEffect Effect;

		private string TextKey;

		private PartType PrimaryPart;

		private List<Parts> _sets = new List<Parts>();

		public Builder(ArmorSetEffect effect, string textKey, PartType primaryPart)
		{
			Effect = effect;
			TextKey = textKey;
			PrimaryPart = primaryPart;
		}

		public Builder Set(int head, int body, int legs)
		{
			_sets.Add(new Parts
			{
				Head = head,
				Body = body,
				Legs = legs
			});
			return this;
		}

		public Builder Set(int[] headOptions, int[] bodyOptions, int[] legsOptions)
		{
			if (headOptions == null)
			{
				headOptions = new int[1];
			}
			if (bodyOptions == null)
			{
				bodyOptions = new int[1];
			}
			if (legsOptions == null)
			{
				legsOptions = new int[1];
			}
			int[] array = headOptions;
			foreach (int head in array)
			{
				int[] array2 = bodyOptions;
				foreach (int body in array2)
				{
					int[] array3 = legsOptions;
					foreach (int legs in array3)
					{
						Set(head, body, legs);
					}
				}
			}
			return this;
		}

		public void Add()
		{
			foreach (Parts set in _sets)
			{
				ArmorSetBonuses.All.Add(new ArmorSetBonus
				{
					Effect = Effect,
					Description = Language.GetText(TextKey),
					Head = set.Head,
					Body = set.Body,
					Legs = set.Legs,
					PrimaryPart = PrimaryPart
				});
			}
		}
	}

	public ArmorSetEffect Effect;

	public LocalizedText Description;

	public int Head;

	public int Body;

	public int Legs;

	public PartType PrimaryPart;

	private static LocalizedText ItemSetBonusEquipped = Language.GetText("UI.ItemSetBonusEquipped");

	private static LocalizedText ItemSetBonusGeneral = Language.GetText("UI.ItemSetBonusGeneral");

	private static LocalizedText[] ItemSetBonusDecidedBy = new LocalizedText[4]
	{
		null,
		Language.GetText("UI.ItemSetBonusDecidedByHead"),
		Language.GetText("UI.ItemSetBonusDecidedByBody"),
		Language.GetText("UI.ItemSetBonusDecidedByLegs")
	};

	public int GetPart(PartType part)
	{
		return part switch
		{
			PartType.Head => Head, 
			PartType.Body => Body, 
			PartType.Legs => Legs, 
			_ => 0, 
		};
	}

	public QueryResult QueryCount(QueryContext context)
	{
		QueryResult result = default;
		TryCounting(context.HeadItem, Head, ref result.ItemsFound, ref result.ItemsNeeded);
		TryCounting(context.BodyItem, Body, ref result.ItemsFound, ref result.ItemsNeeded);
		TryCounting(context.LegItem, Legs, ref result.ItemsFound, ref result.ItemsNeeded);
		return result;
	}

	private void TryCounting(int testedItem, int neededItem, ref int foundItemCount, ref int neededItemCount)
	{
		if (neededItem != 0)
		{
			neededItemCount++;
			if (testedItem == neededItem)
			{
				foundItemCount++;
			}
		}
	}

	public string GetTooltipForSinglePiece(int itemType)
	{
		LocalizedText description = Description;
		if (PrimaryPart != PartType.None && GetPart(PrimaryPart) != itemType)
		{
			description = ItemSetBonusDecidedBy[(int)PrimaryPart];
		}
		return ItemSetBonusGeneral.FormatWith(new SetBonusDisplayStringSubstitutes
		{
			Description = description
		});
	}

	public string GetTooltipForWornArmor(QueryContext context, QueryResult result)
	{
		LocalizedText description = Description;
		if (PrimaryPart != PartType.None && context.GetPart(PrimaryPart) != GetPart(PrimaryPart))
		{
			description = ItemSetBonusDecidedBy[(int)PrimaryPart];
		}
		return ItemSetBonusEquipped.FormatWith(new SetBonusDisplayStringSubstitutes
		{
			Description = description,
			Numerator = result.ItemsFound,
			Denominator = result.ItemsNeeded
		});
	}

	public static Builder Create(ArmorSetEffect effect, string textKey, PartType primaryPart = PartType.None)
	{
		return new Builder(effect, textKey, primaryPart);
	}
}
