using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;

namespace Terraria.GameContent.UI;

public class UIPopupTextManager
{
	public const int maxItemText = 20;

	public UIPopupText[] popupText = new UIPopupText[20];

	public int numActive;

	public void ResetText(UIPopupText text)
	{
		text.scale = 0f;
		text.rotation = 0f;
		text.alpha = 1f;
		text.alphaDir = -1;
		text.framesSinceSpawn = 0;
	}

	public int NewText(UIAdvancedPopupRequest request)
	{
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0115: Unknown result type (might be due to invalid IL or missing references)
		//IL_011a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
		if (!Main.showItemText)
		{
			return -1;
		}
		if (Main.netMode == 2)
		{
			return -1;
		}
		int num = FindNextItemTextSlot();
		if (num >= 0)
		{
			string text = request.Text;
			Vector2 val = FontAssets.MouseText.Value.MeasureString(text);
			UIPopupText uIPopupText = popupText[num];
			ResetText(uIPopupText);
			uIPopupText.active = true;
			uIPopupText.position = request.Position;
			if (request.Alignment >= UIPopupTextAlignment.BottomLeft)
			{
				uIPopupText.position.Y -= val.Y;
			}
			else if (request.Alignment >= UIPopupTextAlignment.MidLeft)
			{
				uIPopupText.position.Y -= val.Y / 2f;
			}
			switch ((int)request.Alignment % 3)
			{
			case 1:
				uIPopupText.position.X -= val.X / 2f;
				break;
			case 2:
				uIPopupText.position.X -= val.X;
				break;
			}
			uIPopupText.name = text;
			uIPopupText.velocity = request.Velocity;
			uIPopupText.lifeTime = request.DurationInFrames;
			uIPopupText.context = request.Context;
			uIPopupText.color = request.Color;
			uIPopupText.PrepareDisplayText();
		}
		return num;
	}

	private int FindNextItemTextSlot()
	{
		int num = -1;
		for (int i = 0; i < 20; i++)
		{
			if (!popupText[i].active)
			{
				num = i;
				break;
			}
		}
		if (num == -1)
		{
			double num2 = Main.bottomWorld;
			for (int j = 0; j < 20; j++)
			{
				if (num2 > (double)popupText[j].position.Y)
				{
					num = j;
					num2 = popupText[j].position.Y;
				}
			}
		}
		return num;
	}

	public void UpdateItemText()
	{
		int num = 0;
		for (int i = 0; i < 20; i++)
		{
			if (popupText[i].active)
			{
				num++;
				popupText[i].Update(i, this);
			}
		}
		numActive = num;
	}

	public void ClearAll()
	{
		for (int i = 0; i < 20; i++)
		{
			popupText[i] = new UIPopupText();
		}
		numActive = 0;
	}

	public void DrawItemTextPopups(float scaleTarget)
	{
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_011d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0127: Unknown result type (might be due to invalid IL or missing references)
		//IL_012c: Unknown result type (might be due to invalid IL or missing references)
		//IL_017c: Unknown result type (might be due to invalid IL or missing references)
		//IL_017e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0228: Unknown result type (might be due to invalid IL or missing references)
		//IL_022a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0223: Unknown result type (might be due to invalid IL or missing references)
		//IL_01de: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0281: Unknown result type (might be due to invalid IL or missing references)
		//IL_0283: Unknown result type (might be due to invalid IL or missing references)
		//IL_0367: Unknown result type (might be due to invalid IL or missing references)
		//IL_0371: Unknown result type (might be due to invalid IL or missing references)
		//IL_0379: Unknown result type (might be due to invalid IL or missing references)
		//IL_037e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0386: Unknown result type (might be due to invalid IL or missing references)
		//IL_029a: Unknown result type (might be due to invalid IL or missing references)
		//IL_029c: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02df: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0300: Unknown result type (might be due to invalid IL or missing references)
		//IL_0323: Unknown result type (might be due to invalid IL or missing references)
		//IL_032d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0335: Unknown result type (might be due to invalid IL or missing references)
		//IL_033a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0342: Unknown result type (might be due to invalid IL or missing references)
		//IL_03bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_03cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_03db: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0415: Unknown result type (might be due to invalid IL or missing references)
		//IL_0417: Unknown result type (might be due to invalid IL or missing references)
		//IL_042a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0440: Unknown result type (might be due to invalid IL or missing references)
		//IL_0445: Unknown result type (might be due to invalid IL or missing references)
		//IL_0454: Unknown result type (might be due to invalid IL or missing references)
		//IL_045c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0468: Unknown result type (might be due to invalid IL or missing references)
		//IL_0472: Unknown result type (might be due to invalid IL or missing references)
		//IL_0487: Unknown result type (might be due to invalid IL or missing references)
		//IL_0489: Unknown result type (might be due to invalid IL or missing references)
		//IL_049c: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_04da: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f6: Unknown result type (might be due to invalid IL or missing references)
		SpriteBatch spriteBatch = Main.spriteBatch;
		for (int i = 0; i < 20; i++)
		{
			UIPopupText uIPopupText = popupText[i];
			if (!uIPopupText.active)
			{
				continue;
			}
			string displayText = uIPopupText.displayText;
			Vector2 val = FontAssets.MouseText.Value.MeasureString(displayText);
			Vector2 val2 = new Vector2(val.X * 0.5f, val.Y * 0.5f);
			float num = scaleTarget;
			float num2 = uIPopupText.scale / num;
			int num3 = (int)(255f - 255f * num2);
			float num4 = (int)uIPopupText.color.R;
			_ = (float)(int)uIPopupText.color.G;
			_ = (float)(int)uIPopupText.color.B;
			float num5 = (int)uIPopupText.color.A;
			num4 *= num2 * uIPopupText.alpha * 0.3f;
			_ = uIPopupText.alpha;
			_ = uIPopupText.alpha;
			num5 *= num2 * uIPopupText.alpha;
			Color val3 = Color.Black;
			float num6 = 1f;
			Texture2D val4 = null;
			if (uIPopupText.context == UIPopupTextContext.SpecialSeed)
			{
				val3 = Main.hslToRgb(Main.GlobalTimeWrappedHourly * 0.6f % 1f, 1f, 0.6f) * 0.6f;
				num *= 0.5f;
				num6 = 0.8f;
			}
			int num7 = 40;
			Utils.EaseOutCirc(Utils.Remap(uIPopupText.framesSinceSpawn, 0f, num7, 0f, 1f));
			float num8 = (float)num3 / 255f;
			for (int j = 0; j < 5; j++)
			{
				Color val5 = val3;
				float num9 = 0f;
				float num10 = 0f;
				switch (j)
				{
				case 0:
					num9 -= num * 2f;
					break;
				case 1:
					num9 += num * 2f;
					break;
				case 2:
					num10 -= num * 2f;
					break;
				case 3:
					num10 += num * 2f;
					break;
				default:
					val5 = uIPopupText.color * num2 * uIPopupText.alpha * num6;
					break;
				}
				if (j < 4)
				{
					num5 = (float)(int)uIPopupText.color.A * num2 * uIPopupText.alpha;
					val5 = new Color(0, 0, 0, (int)num5);
				}
				if (val3 != Color.Black && j < 4)
				{
					num9 *= 1.3f + 1.3f * num8;
					num10 *= 1.3f + 1.3f * num8;
				}
				float num11 = uIPopupText.position.X + num9;
				float num12 = uIPopupText.position.Y + num10;
				if (val3 != Color.Black && j < 4)
				{
					Color val6 = val3;
					val6.A = (byte)MathHelper.Lerp(60f, 127f, Utils.GetLerpValue(0f, 255f, num5, clamped: true));
					DynamicSpriteFontExtensionMethods.DrawString(spriteBatch, FontAssets.MouseText.Value, displayText, new Vector2(num11 + val2.X, num12 + val2.Y), Color.Lerp(val5, val6, 0.5f), uIPopupText.rotation, val2, uIPopupText.scale, (SpriteEffects)0, 0f, (Vector2[])null, (Color[])null);
					DynamicSpriteFontExtensionMethods.DrawString(spriteBatch, FontAssets.MouseText.Value, displayText, new Vector2(num11 + val2.X, num12 + val2.Y), val6, uIPopupText.rotation, val2, uIPopupText.scale, (SpriteEffects)0, 0f, (Vector2[])null, (Color[])null);
				}
				else
				{
					DynamicSpriteFontExtensionMethods.DrawString(spriteBatch, FontAssets.MouseText.Value, displayText, new Vector2(num11 + val2.X, num12 + val2.Y), val5, uIPopupText.rotation, val2, uIPopupText.scale, (SpriteEffects)0, 0f, (Vector2[])null, (Color[])null);
				}
				if (val4 != null)
				{
					float num13 = (1.3f - num8) * uIPopupText.scale * 0.7f;
					Vector2 val7 = new Vector2(num11 + val2.X, num12 + val2.Y);
					Color val8 = val3 * 0.6f;
					if (j == 4)
					{
						val8 = Color.White * 0.6f;
					}
					val8.A = (byte)((float)(int)val8.A * 0.5f);
					int num14 = 25;
					spriteBatch.Draw(val4, val7 + new Vector2(val2.X * -0.5f - (float)num14 - val4.Size().X / 2f, 0f), (Rectangle?)null, val8 * uIPopupText.scale, 0f, val4.Size() / 2f, num13, (SpriteEffects)0, 0f);
					spriteBatch.Draw(val4, val7 + new Vector2(val2.X * 0.5f + (float)num14 + val4.Size().X / 2f, 0f), (Rectangle?)null, val8 * uIPopupText.scale, 0f, val4.Size() / 2f, num13, (SpriteEffects)0, 0f);
				}
			}
		}
	}
}
