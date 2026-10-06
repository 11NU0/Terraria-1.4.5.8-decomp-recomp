using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Localization;
using Terraria.UI;
using Terraria.UI.Chat;

namespace Terraria.GameContent.UI;

public class CustomCurrencySingleCoin : CustomCurrencySystem
{
	public float CurrencyDrawScale = 0.8f;

	public string CurrencyTextKey = "Currency.DefenderMedals_PriceText";

	public Color CurrencyTextColor = new Color(240, 100, 120);

	public CustomCurrencySingleCoin(int coinItemID, long currencyCap)
	{
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		Include(coinItemID, 1);
		SetCurrencyCap(currencyCap);
	}

	public override bool TryPurchasing(long price, List<Item[]> inv, List<Point> slotCoins, List<Point> slotsEmpty, List<Point> slotEmptyBank, List<Point> slotEmptyBank2, List<Point> slotEmptyBank3, List<Point> slotEmptyBank4)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
		List<Tuple<Point, Item>> cache = ItemCacheCreate(inv);
		long num = price;
		for (int i = 0; i < slotCoins.Count; i++)
		{
			Point val = slotCoins[i];
			long num2 = num;
			if (inv[val.X][val.Y].stack < num2)
			{
				num2 = inv[val.X][val.Y].stack;
			}
			num -= num2;
			inv[val.X][val.Y].stack -= (int)num2;
			if (inv[val.X][val.Y].stack == 0)
			{
				switch (val.X)
				{
				case 0:
					slotsEmpty.Add(val);
					break;
				case 1:
					slotEmptyBank.Add(val);
					break;
				case 2:
					slotEmptyBank2.Add(val);
					break;
				case 3:
					slotEmptyBank3.Add(val);
					break;
				case 4:
					slotEmptyBank4.Add(val);
					break;
				}
				slotCoins.Remove(val);
				i--;
			}
			if (num == 0L)
			{
				break;
			}
		}
		if (num != 0L)
		{
			ItemCacheRestore(cache, inv);
			return false;
		}
		return true;
	}

	public override void DrawSavingsMoney(SpriteBatch sb, string text, float shopx, float shopy, long totalCoins, bool horizontal = false)
	{
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0104: Unknown result type (might be due to invalid IL or missing references)
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		//IL_0113: Unknown result type (might be due to invalid IL or missing references)
		//IL_0150: Unknown result type (might be due to invalid IL or missing references)
		//IL_015f: Unknown result type (might be due to invalid IL or missing references)
		//IL_016a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0174: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b8: Unknown result type (might be due to invalid IL or missing references)
		int num = _valuePerUnit.Keys.ElementAt(0);
		Main.instance.LoadItem(num);
		Texture2D value = TextureAssets.Item[num].Value;
		CoinSlot.CoinDrawState drawState = new CoinSlot.CoinDrawState
		{
			coinAnimFrame = 0,
			coinYOffset = 0f,
			stackTextScale = 1f
		};
		CoinSlot.UpdateCustom(num, (int)totalCoins, out drawState);
		if (horizontal)
		{
			_ = 99;
			Vector2 val = new Vector2(shopx + ChatManager.GetStringSize(FontAssets.MouseText.Value, text, Vector2.One).X + 45f, shopy + 50f - drawState.coinYOffset);
			sb.Draw(value, val, (Rectangle?)null, Color.White, 0f, value.Size() / 2f, CurrencyDrawScale, (SpriteEffects)0, 0f);
			Utils.DrawBorderStringFourWay(sb, FontAssets.ItemStack.Value, totalCoins.ToString(), val.X - 11f, val.Y, Color.White, Color.Black, new Vector2(0.3f), 0.75f * drawState.stackTextScale);
		}
		else
		{
			int num2 = ((totalCoins > 99) ? (-6) : 0);
			sb.Draw(value, new Vector2(shopx + 11f, shopy + 75f - drawState.coinYOffset), (Rectangle?)null, Color.White, 0f, value.Size() / 2f, CurrencyDrawScale, (SpriteEffects)0, 0f);
			Utils.DrawBorderStringFourWay(sb, FontAssets.ItemStack.Value, totalCoins.ToString(), shopx + (float)num2, shopy + 75f, Color.White, Color.Black, new Vector2(0.3f), 0.75f * drawState.stackTextScale);
		}
	}

	public override void GetPriceText(string[] lines, ref int currentLine, long price)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		Color val = CurrencyTextColor * ((float)(int)Main.mouseTextColor / 255f);
		lines[currentLine++] = $"[c/{val.R:X2}{val.G:X2}{val.B:X2}:{Lang.tip[50].Value} {price} {Language.GetTextValue(CurrencyTextKey)}]";
	}
}
