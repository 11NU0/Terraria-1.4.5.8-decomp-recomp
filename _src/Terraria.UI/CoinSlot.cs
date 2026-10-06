using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using Terraria.ID;

namespace Terraria.UI;

public class CoinSlot
{
	private struct CoinEntry
	{
		public int Type;

		public int Stack;

		public int TextAnimFrame;

		public int JumpAnimFrame;

		public int SpinAnimFrame;

		public int DrawActive;

		public int JumpAnimHold;

		public int FadeItemType;

		public void ForceState(int itemType, int itemStack)
		{
			Type = itemType;
			Stack = itemStack;
			TextAnimFrame = 0;
			JumpAnimFrame = 0;
			JumpAnimHold = 0;
			SpinAnimFrame = 0;
			FadeItemType = 0;
		}

		public void UpdateState(int itemType, int itemStack, float jumpScale, out CoinDrawState drawState)
		{
			if (Type != itemType || DrawActive == 0)
			{
				bool flag = true;
				if (itemType != 0 && FadeItemType == itemType && DrawActive != 0)
				{
					flag = false;
				}
				if (itemType == 0 && DrawActive != 0 && ItemID.Sets.CommonCoin[Type])
				{
					FadeItemType = Type;
				}
				else
				{
					FadeItemType = 0;
				}
				if (FadeItemType != 0)
				{
					flag = false;
				}
				Type = itemType;
				if (DrawActive == 0)
				{
					Stack = itemStack;
				}
				if (flag)
				{
					TextAnimFrame = 0;
					JumpAnimFrame = 0;
					JumpAnimHold = 0;
					SpinAnimFrame = 0;
				}
			}
			DrawActive = 2;
			if (ItemID.Sets.CommonCoin[Type] || Type == 3817 || FadeItemType != 0)
			{
				if (Stack != itemStack)
				{
					Stack = itemStack;
					if (TextAnimFrame == 0)
					{
						TextAnimFrame = TextAnimKeys.Length - 1;
					}
				}
				if (TextAnimFrame >= JumpTrigger_TextAnimRangeStart && TextAnimFrame <= JumpTrigger_TextAnimRangeEnd)
				{
					JumpAnimHold = JumpAnimHoldTime;
					if (JumpAnimFrame == 0)
					{
						JumpAnimFrame = JumpAnimKeys.Length - 1;
					}
				}
			}
			drawState.stackTextScale = TextAnimKeys[TextAnimFrame];
			drawState.coinYOffset = JumpAnimKeys[JumpAnimFrame] * jumpScale;
			drawState.coinAnimFrame = SpinAnimFrame / 2;
			drawState.fadeItem = FadeItemType;
			drawState.fadeScale = 1f;
			if (FadeItemType != 0)
			{
				if (TextAnimFrame > 0 || JumpAnimFrame >= JumpApex || JumpAnimFrame >= FadeAnimKeys.Length)
				{
					drawState.stackTextDrawFadeOverload = 1f;
				}
				else
				{
					drawState.stackTextDrawFadeOverload = FadeAnimKeys[JumpAnimFrame];
				}
				drawState.fadeScale = drawState.stackTextDrawFadeOverload;
			}
			else if (Stack == 1 && (TextAnimFrame > 0 || JumpAnimFrame != 0))
			{
				if (TextAnimFrame > 0 || JumpAnimFrame >= JumpApex || JumpAnimFrame >= FadeAnimKeys.Length)
				{
					drawState.stackTextDrawFadeOverload = 1f;
				}
				else
				{
					drawState.stackTextDrawFadeOverload = FadeAnimKeys[JumpAnimFrame];
				}
			}
			else
			{
				drawState.stackTextDrawFadeOverload = -1f;
			}
		}

		public void UpdateAnim()
		{
			if (DrawActive > 0)
			{
				DrawActive--;
			}
			if (FadeItemType > 0 && JumpAnimFrame == 0 && TextAnimFrame == 0)
			{
				FadeItemType = 0;
			}
			if (TextAnimFrame > 0)
			{
				TextAnimFrame--;
			}
			if (JumpAnimHold > 0)
			{
				JumpAnimHold--;
			}
			if (JumpAnimFrame > 0)
			{
				if (JumpAnimHold > 0)
				{
					if (JumpAnimFrame != JumpApex)
					{
						if (JumpAnimFrame < JumpApex)
						{
							JumpAnimFrame = JumpApex + JumpApex - JumpAnimFrame;
						}
						JumpAnimFrame--;
					}
				}
				else
				{
					JumpAnimFrame--;
				}
			}
			if (JumpAnimFrame >= SpinAnimRangeStart && JumpAnimFrame <= SpinAnimRangeEnd)
			{
				SpinAnimFrame = (SpinAnimFrame + 1) % 14;
			}
			else if (SpinAnimFrame != 0)
			{
				SpinAnimFrame = (SpinAnimFrame + 1) % 14;
			}
		}
	}

	private class CoinEntryRef
	{
		public CoinEntry val;
	}

	public struct CoinDrawState
	{
		public int coinAnimFrame;

		public float coinYOffset;

		public float stackTextScale;

		public float stackTextDrawFadeOverload;

		public int fadeItem;

		public float fadeScale;
	}

	private static float[] FadeAnimKeys = new float[21]
	{
		0.3f, 0.4f, 0.5f, 0.6f, 0.7f, 0.8f, 0.9f, 1f, 1f, 1f,
		1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f,
		1f
	};

	private static float[] TextAnimKeys = new float[17]
	{
		1f, 1.0107f, 1.0391f, 1.0791f, 1.125f, 1.1709f, 1.2109f, 1.2393f, 1.25f, 1.2393f,
		1.2109f, 1.1709f, 1.125f, 1.0791f, 1.0391f, 1.0107f, 1f
	};

	private static float[] JumpAnimKeys = new float[25]
	{
		0f, 0.23748f, 0.43408f, 0.59366f, 0.72007f, 0.81717f, 0.88881f, 0.93885f, 0.97115f, 0.98955f,
		0.99793f, 1f, 1f, 1f, 0.99793f, 0.98955f, 0.97115f, 0.93885f, 0.88881f, 0.81717f,
		0.72007f, 0.59366f, 0.43408f, 0.23748f, 0f
	};

	private static int JumpApex = 12;

	private static int JumpTrigger_TextAnimRangeStart = 9;

	private static int JumpTrigger_TextAnimRangeEnd = 13;

	private static float ItemSlotCoinJumpScale = 10f;

	private static float SavingsCoinJumpScale = 10f;

	private static int JumpAnimHoldTime = 12;

	private static int SpinAnimRangeStart = 9;

	private static int SpinAnimRangeEnd = 13;

	private static CoinEntry[] Savings = new CoinEntry[4];

	private static CoinEntry[] ChestEntries = new CoinEntry[200];

	private static CoinEntry[] InventoryEntries = new CoinEntry[59];

	private static Dictionary<int, CoinEntryRef> Custom = new Dictionary<int, CoinEntryRef>();

	public static void UpdateSavings(int slot, int count, out CoinDrawState drawState)
	{
		Savings[slot].UpdateState(71 + slot, count, SavingsCoinJumpScale, out drawState);
	}

	public static void UpdateCustom(int customCurrencyID, int count, out CoinDrawState drawState)
	{
		if (!Custom.TryGetValue(customCurrencyID, out var value))
		{
			value = new CoinEntryRef();
			Custom[customCurrencyID] = value;
		}
		value.val.UpdateState(customCurrencyID, count, SavingsCoinJumpScale, out drawState);
	}

	public static float DrawItemCoin(SpriteBatch spriteBatch, Vector2 screenPositionForItemCenter, int coinType, int coinFrame, float scale, float sizeLimit, Color itemColor, float itemFade = 1f)
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		int num = coinType - 71;
		Texture2D value = TextureAssets.Coin[num].Value;
		Rectangle val = value.Frame(1, 8, 0, coinFrame);
		Color white = Color.White;
		_ = Color.White;
		float num2 = 1f;
		if ((float)val.Width > sizeLimit || (float)val.Height > sizeLimit)
		{
			num2 = ((val.Width <= val.Height) ? (sizeLimit / (float)val.Height) : (sizeLimit / (float)val.Width));
		}
		float num3 = scale * num2;
		SpriteEffects val2 = (SpriteEffects)0;
		Vector2 val3 = val.Size() / 2f;
		Color val4 = ContentSamples.ItemsByType[coinType].GetAlpha(itemColor).MultiplyRGBA(white);
		spriteBatch.Draw(value, screenPositionForItemCenter, (Rectangle?)val, val4 * itemFade, 0f, val3, num3, val2, 0f);
		return num3;
	}

	public static void UpdateSlotAnims()
	{
		for (int i = 0; i < Savings.Length; i++)
		{
			Savings[i].UpdateAnim();
		}
		for (int j = 0; j < ChestEntries.Length; j++)
		{
			ChestEntries[j].UpdateAnim();
		}
		for (int k = 0; k < InventoryEntries.Length; k++)
		{
			InventoryEntries[k].UpdateAnim();
		}
		foreach (KeyValuePair<int, CoinEntryRef> item in Custom)
		{
			item.Value.val.UpdateAnim();
		}
	}

	public static void ForceSlotState(int slot, int context, Item item)
	{
		switch (context)
		{
		case 0:
		case 1:
		case 2:
			InventoryEntries[slot].ForceState(item.type, item.stack);
			break;
		case 3:
		case 4:
			ChestEntries[slot].ForceState(item.type, item.stack);
			break;
		}
	}

	public static void UpdateDrawState(int slot, int context, Item item, out CoinDrawState drawState)
	{
		switch (context)
		{
		case 0:
		case 1:
		case 2:
			InventoryEntries[slot].UpdateState(item.type, item.stack, ItemSlotCoinJumpScale, out drawState);
			return;
		case 3:
		case 4:
			ChestEntries[slot].UpdateState(item.type, item.stack, ItemSlotCoinJumpScale, out drawState);
			return;
		}
		drawState.fadeItem = 0;
		drawState.fadeScale = 1f;
		drawState.coinAnimFrame = 0;
		drawState.coinYOffset = 0f;
		drawState.stackTextScale = 1f;
		drawState.stackTextDrawFadeOverload = -1f;
	}
}
