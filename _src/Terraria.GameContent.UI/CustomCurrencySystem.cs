using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Terraria.GameContent.UI;

public class CustomCurrencySystem
{
	protected Dictionary<int, int> _valuePerUnit = new Dictionary<int, int>();

	private long _currencyCap = 999999999L;

	public long CurrencyCap => _currencyCap;

	public void Include(int coin, int howMuchIsItWorth)
	{
		_valuePerUnit[coin] = howMuchIsItWorth;
	}

	public void SetCurrencyCap(long cap)
	{
		_currencyCap = cap;
	}

	public virtual long CountCurrency(out bool overFlowing, Item[] inv, params int[] ignoreSlots)
	{
		List<int> list = new List<int>(ignoreSlots);
		long num = 0L;
		for (int i = 0; i < inv.Length; i++)
		{
			if (!list.Contains(i))
			{
				if (_valuePerUnit.TryGetValue(inv[i].type, out var value))
				{
					num += value * inv[i].stack;
				}
				if (num >= CurrencyCap)
				{
					overFlowing = true;
					return CurrencyCap;
				}
			}
		}
		overFlowing = false;
		return num;
	}

	public virtual long CombineStacks(out bool overFlowing, params long[] coinCounts)
	{
		long num = 0L;
		foreach (long num2 in coinCounts)
		{
			num += num2;
			if (num >= CurrencyCap)
			{
				overFlowing = true;
				return CurrencyCap;
			}
		}
		overFlowing = false;
		return num;
	}

	public virtual bool TryPurchasing(long price, List<Item[]> inv, List<Point> slotCoins, List<Point> slotsEmpty, List<Point> slotEmptyBank, List<Point> slotEmptyBank2, List<Point> slotEmptyBank3, List<Point> slotEmptyBank4)
	{
		//IL_017a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_061d: Unknown result type (might be due to invalid IL or missing references)
		//IL_062e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0100: Unknown result type (might be due to invalid IL or missing references)
		//IL_010c: Unknown result type (might be due to invalid IL or missing references)
		//IL_035d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0367: Unknown result type (might be due to invalid IL or missing references)
		//IL_038d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0575: Unknown result type (might be due to invalid IL or missing references)
		//IL_039a: Unknown result type (might be due to invalid IL or missing references)
		//IL_057f: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_058a: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_03bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_03cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_03fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0256: Unknown result type (might be due to invalid IL or missing references)
		//IL_0262: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_0309: Unknown result type (might be due to invalid IL or missing references)
		//IL_031b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0330: Unknown result type (might be due to invalid IL or missing references)
		//IL_0335: Unknown result type (might be due to invalid IL or missing references)
		//IL_0281: Unknown result type (might be due to invalid IL or missing references)
		//IL_028d: Unknown result type (might be due to invalid IL or missing references)
		//IL_029e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0460: Unknown result type (might be due to invalid IL or missing references)
		//IL_046c: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_04bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0513: Unknown result type (might be due to invalid IL or missing references)
		//IL_0525: Unknown result type (might be due to invalid IL or missing references)
		//IL_053a: Unknown result type (might be due to invalid IL or missing references)
		//IL_053f: Unknown result type (might be due to invalid IL or missing references)
		//IL_048b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0497: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a8: Unknown result type (might be due to invalid IL or missing references)
		long num = price;
		Dictionary<Point, Item> dictionary = new Dictionary<Point, Item>();
		bool result = true;
		while (num > 0)
		{
			long num2 = 1000000L;
			for (int i = 0; i < 4; i++)
			{
				if (num >= num2)
				{
					foreach (Point slotCoin in slotCoins)
					{
						if (inv[slotCoin.X][slotCoin.Y].type == 74 - i)
						{
							long num3 = num / num2;
							dictionary[slotCoin] = inv[slotCoin.X][slotCoin.Y].Clone();
							if (num3 < inv[slotCoin.X][slotCoin.Y].stack)
							{
								inv[slotCoin.X][slotCoin.Y].stack -= (int)num3;
							}
							else
							{
								inv[slotCoin.X][slotCoin.Y].SetDefaults(0);
								slotsEmpty.Add(slotCoin);
							}
							num -= num2 * (dictionary[slotCoin].stack - inv[slotCoin.X][slotCoin.Y].stack);
						}
					}
				}
				num2 /= 100;
			}
			if (num <= 0)
			{
				continue;
			}
			if (slotsEmpty.Count > 0)
			{
				slotsEmpty.Sort(DelegateMethods.CompareYReverse);
				Point val = new Point(-1, -1);
				for (int j = 0; j < inv.Count; j++)
				{
					num2 = 10000L;
					for (int k = 0; k < 3; k++)
					{
						if (num >= num2)
						{
							foreach (Point slotCoin2 in slotCoins)
							{
								if (slotCoin2.X == j && inv[slotCoin2.X][slotCoin2.Y].type == 74 - k && inv[slotCoin2.X][slotCoin2.Y].stack >= 1)
								{
									List<Point> list = slotsEmpty;
									if (j == 1 && slotEmptyBank.Count > 0)
									{
										list = slotEmptyBank;
									}
									if (j == 2 && slotEmptyBank2.Count > 0)
									{
										list = slotEmptyBank2;
									}
									if (j == 3 && slotEmptyBank3.Count > 0)
									{
										list = slotEmptyBank3;
									}
									if (j == 4 && slotEmptyBank4.Count > 0)
									{
										list = slotEmptyBank4;
									}
									if (--inv[slotCoin2.X][slotCoin2.Y].stack <= 0)
									{
										inv[slotCoin2.X][slotCoin2.Y].SetDefaults(0);
										list.Add(slotCoin2);
									}
									dictionary[list[0]] = inv[list[0].X][list[0].Y].Clone();
									inv[list[0].X][list[0].Y].SetDefaults(73 - k);
									inv[list[0].X][list[0].Y].stack = 100;
									val = list[0];
									list.RemoveAt(0);
									break;
								}
							}
						}
						if (val.X != -1 || val.Y != -1)
						{
							break;
						}
						num2 /= 100;
					}
					for (int l = 0; l < 2; l++)
					{
						if (val.X != -1 || val.Y != -1)
						{
							continue;
						}
						foreach (Point slotCoin3 in slotCoins)
						{
							if (slotCoin3.X == j && inv[slotCoin3.X][slotCoin3.Y].type == 73 + l && inv[slotCoin3.X][slotCoin3.Y].stack >= 1)
							{
								List<Point> list2 = slotsEmpty;
								if (j == 1 && slotEmptyBank.Count > 0)
								{
									list2 = slotEmptyBank;
								}
								if (j == 2 && slotEmptyBank2.Count > 0)
								{
									list2 = slotEmptyBank2;
								}
								if (j == 3 && slotEmptyBank3.Count > 0)
								{
									list2 = slotEmptyBank3;
								}
								if (j == 4 && slotEmptyBank4.Count > 0)
								{
									list2 = slotEmptyBank4;
								}
								if (--inv[slotCoin3.X][slotCoin3.Y].stack <= 0)
								{
									inv[slotCoin3.X][slotCoin3.Y].SetDefaults(0);
									list2.Add(slotCoin3);
								}
								dictionary[list2[0]] = inv[list2[0].X][list2[0].Y].Clone();
								inv[list2[0].X][list2[0].Y].SetDefaults(72 + l);
								inv[list2[0].X][list2[0].Y].stack = 100;
								val = list2[0];
								list2.RemoveAt(0);
								break;
							}
						}
					}
					if (val.X != -1 && val.Y != -1)
					{
						slotCoins.Add(val);
						break;
					}
				}
				slotsEmpty.Sort(DelegateMethods.CompareYReverse);
				slotEmptyBank.Sort(DelegateMethods.CompareYReverse);
				slotEmptyBank2.Sort(DelegateMethods.CompareYReverse);
				slotEmptyBank3.Sort(DelegateMethods.CompareYReverse);
				slotEmptyBank4.Sort(DelegateMethods.CompareYReverse);
				continue;
			}
			foreach (KeyValuePair<Point, Item> item in dictionary)
			{
				inv[item.Key.X][item.Key.Y] = item.Value.Clone();
			}
			result = false;
			break;
		}
		return result;
	}

	public virtual bool Accepts(Item item)
	{
		return _valuePerUnit.ContainsKey(item.type);
	}

	public virtual void DrawSavingsMoney(SpriteBatch sb, string text, float shopx, float shopy, long totalCoins, bool horizontal = false)
	{
	}

	public virtual void GetPriceText(string[] lines, ref int currentLine, long price)
	{
	}

	protected int SortByHighest(Tuple<int, int> valueA, Tuple<int, int> valueB)
	{
		if (valueA.Item2 > valueB.Item2)
		{
			return -1;
		}
		if (valueA.Item2 == valueB.Item2)
		{
			return 0;
		}
		return -1;
	}

	protected List<Tuple<Point, Item>> ItemCacheCreate(List<Item[]> inventories)
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		List<Tuple<Point, Item>> list = new List<Tuple<Point, Item>>();
		for (int i = 0; i < inventories.Count; i++)
		{
			for (int j = 0; j < inventories[i].Length; j++)
			{
				Item item = inventories[i][j];
				list.Add(new Tuple<Point, Item>(new Point(i, j), item.DeepClone()));
			}
		}
		return list;
	}

	protected void ItemCacheRestore(List<Tuple<Point, Item>> cache, List<Item[]> inventories)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		foreach (Tuple<Point, Item> item in cache)
		{
			inventories[item.Item1.X][item.Item1.Y] = item.Item2;
		}
	}

	public virtual void GetItemExpectedPrice(Item item, out long calcForSelling, out long calcForBuying)
	{
		int storeValue = item.GetStoreValue();
		calcForSelling = storeValue;
		calcForBuying = storeValue;
	}
}
