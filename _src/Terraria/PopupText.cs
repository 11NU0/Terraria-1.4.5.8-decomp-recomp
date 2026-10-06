using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using Terraria.GameContent;
using Terraria.Localization;

namespace Terraria;

public class PopupText
{
	public const int maxItemText = 20;

	public static PopupText[] popupText = new PopupText[20];

	public Vector2 position;

	public Vector2 velocity;

	public float alpha;

	public int alphaDir = 1;

	public string name;

	public string displayText;

	public long stack;

	public float scale = 1f;

	public float rotation;

	public Color color;

	public bool active;

	public int lifeTime;

	public int framesSinceSpawn;

	public static int activeTime = 60;

	public static int numActive;

	public bool NoStack;

	public bool coinText;

	public long coinValue;

	public static int sonarText = -1;

	public bool expert;

	public bool master;

	public bool sonar;

	public PopupTextContext context;

	public int npcNetID;

	public bool freeAdvanced;

	public Vector2[] charOffsets;

	public Color[] charColors;

	public PopupEffectStyle effectStyle;

	public int effectIntensity;

	public bool AnyEffect => effectStyle != PopupEffectStyle.None;

	public bool notActuallyAnItem
	{
		get
		{
			if (npcNetID == 0)
			{
				return freeAdvanced;
			}
			return true;
		}
	}

	public static float TargetScale
	{
		get
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			return Main.UIScale / Main.GameViewMatrix.RenderZoom.X;
		}
	}

	public static void ClearSonarText()
	{
		if (sonarText >= 0 && popupText[sonarText].sonar)
		{
			popupText[sonarText].active = false;
			sonarText = -1;
		}
	}

	public static void ResetText(PopupText text)
	{
		text.NoStack = false;
		text.coinText = false;
		text.coinValue = 0L;
		text.sonar = false;
		text.npcNetID = 0;
		text.expert = false;
		text.master = false;
		text.freeAdvanced = false;
		text.scale = 0f;
		text.rotation = 0f;
		text.alpha = 1f;
		text.alphaDir = -1;
		text.framesSinceSpawn = 0;
		text.effectStyle = PopupEffectStyle.None;
		text.effectIntensity = 0;
	}

	public static int NewText(AdvancedPopupRequest request, Vector2 position)
	{
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
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
			PopupText obj = popupText[num];
			ResetText(obj);
			obj.active = true;
			obj.position = position - val / 2f;
			obj.name = text;
			obj.stack = 1L;
			obj.velocity = request.Velocity;
			obj.lifeTime = request.DurationInFrames;
			obj.context = PopupTextContext.Advanced;
			obj.freeAdvanced = true;
			obj.color = request.Color;
			obj.PrepareDisplayText();
		}
		return num;
	}

	public static int NewText(PopupTextContext context, int npcNetID, Vector2 position, bool stay5TimesLonger)
	{
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		if (!Main.showItemText)
		{
			return -1;
		}
		if (npcNetID == 0)
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
			NPC nPC = new NPC();
			nPC.SetDefaults(npcNetID);
			string typeName = nPC.TypeName;
			Vector2 val = FontAssets.MouseText.Value.MeasureString(typeName);
			PopupText popupText = PopupText.popupText[num];
			ResetText(popupText);
			popupText.active = true;
			popupText.position = position - val / 2f;
			popupText.name = typeName;
			popupText.stack = 1L;
			popupText.velocity.Y = -7f;
			popupText.lifeTime = 60;
			popupText.context = context;
			if (stay5TimesLonger)
			{
				popupText.lifeTime *= 5;
			}
			popupText.npcNetID = npcNetID;
			popupText.color = Color.White;
			if (context == PopupTextContext.SonarAlert)
			{
				popupText.color = Color.Lerp(Color.White, Color.Crimson, 0.5f);
			}
			popupText.PrepareDisplayText();
		}
		return num;
	}

	public static int NewText(PopupTextContext context, Item newItem, Vector2 position, int stack, bool noStack = false, bool longText = false)
	{
		//IL_041b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0420: Unknown result type (might be due to invalid IL or missing references)
		//IL_0441: Unknown result type (might be due to invalid IL or missing references)
		//IL_0447: Unknown result type (might be due to invalid IL or missing references)
		//IL_0461: Unknown result type (might be due to invalid IL or missing references)
		//IL_0467: Unknown result type (might be due to invalid IL or missing references)
		//IL_0118: Unknown result type (might be due to invalid IL or missing references)
		//IL_011d: Unknown result type (might be due to invalid IL or missing references)
		//IL_012b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0130: Unknown result type (might be due to invalid IL or missing references)
		//IL_0317: Unknown result type (might be due to invalid IL or missing references)
		//IL_032b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0344: Unknown result type (might be due to invalid IL or missing references)
		//IL_0358: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0608: Unknown result type (might be due to invalid IL or missing references)
		//IL_060d: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_064b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0650: Unknown result type (might be due to invalid IL or missing references)
		//IL_0684: Unknown result type (might be due to invalid IL or missing references)
		//IL_0689: Unknown result type (might be due to invalid IL or missing references)
		//IL_022c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0231: Unknown result type (might be due to invalid IL or missing references)
		//IL_026e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0273: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e7: Unknown result type (might be due to invalid IL or missing references)
		if (!Main.showItemText)
		{
			return -1;
		}
		if (newItem.Name == null)
		{
			return -1;
		}
		if (Main.netMode == 2)
		{
			return -1;
		}
		bool flag = newItem.type >= 71 && newItem.type <= 74;
		for (int i = 0; i < 20; i++)
		{
			PopupText popupText = PopupText.popupText[i];
			if (!popupText.active || popupText.notActuallyAnItem || (!(popupText.name == newItem.AffixName()) && (!flag || !popupText.coinText)) || popupText.NoStack || noStack)
			{
				continue;
			}
			string text = newItem.Name + " (" + (popupText.stack + stack) + ")";
			string text2 = newItem.Name;
			if (popupText.stack > 1)
			{
				text2 = text2 + " (" + popupText.stack + ")";
			}
			Vector2 val = FontAssets.MouseText.Value.MeasureString(text2);
			val = FontAssets.MouseText.Value.MeasureString(text);
			if (popupText.lifeTime < 0)
			{
				popupText.scale = 1f;
			}
			if (popupText.lifeTime < 60)
			{
				popupText.lifeTime = 60;
			}
			if (flag && popupText.coinText)
			{
				long num = 0L;
				if (newItem.type == 71)
				{
					num += stack;
				}
				else if (newItem.type == 72)
				{
					num += 100 * stack;
				}
				else if (newItem.type == 73)
				{
					num += 10000 * stack;
				}
				else if (newItem.type == 74)
				{
					num += 1000000 * stack;
				}
				popupText.AddToCoinValue(num);
				text = ValueToName(popupText.coinValue);
				val = FontAssets.MouseText.Value.MeasureString(text);
				popupText.name = text;
				if (popupText.coinValue >= 1000000)
				{
					if (popupText.lifeTime < 300)
					{
						popupText.lifeTime = 300;
					}
					popupText.color = new Color(220, 220, 198);
				}
				else if (popupText.coinValue >= 10000)
				{
					if (popupText.lifeTime < 240)
					{
						popupText.lifeTime = 240;
					}
					popupText.color = new Color(224, 201, 92);
				}
				else if (popupText.coinValue >= 100)
				{
					if (popupText.lifeTime < 180)
					{
						popupText.lifeTime = 180;
					}
					popupText.color = new Color(181, 192, 193);
				}
				else if (popupText.coinValue >= 1)
				{
					if (popupText.lifeTime < 120)
					{
						popupText.lifeTime = 120;
					}
					popupText.color = new Color(246, 138, 96);
				}
			}
			popupText.stack += stack;
			popupText.scale = 0f;
			popupText.rotation = 0f;
			popupText.position.X = position.X + (float)newItem.width * 0.5f - val.X * 0.5f;
			popupText.position.Y = position.Y + (float)newItem.height * 0.25f - val.Y * 0.5f;
			popupText.velocity.Y = -7f;
			popupText.context = context;
			popupText.npcNetID = 0;
			popupText.effectStyle = PopupEffectStyle.None;
			if (popupText.coinText)
			{
				popupText.stack = 1L;
			}
			PrepareEffects(context, newItem, popupText);
			if (popupText.AnyEffect)
			{
				popupText.framesSinceSpawn = 0;
			}
			popupText.PrepareDisplayText();
			return i;
		}
		int num2 = FindNextItemTextSlot();
		if (num2 >= 0)
		{
			string text3 = newItem.AffixName();
			if (stack > 1)
			{
				text3 = text3 + " (" + stack + ")";
			}
			Vector2 val2 = FontAssets.MouseText.Value.MeasureString(text3);
			PopupText popupText2 = PopupText.popupText[num2];
			ResetText(popupText2);
			popupText2.active = true;
			popupText2.position.X = position.X - val2.X * 0.5f;
			popupText2.position.Y = position.Y - val2.Y * 0.5f;
			popupText2.name = newItem.AffixName();
			popupText2.stack = stack;
			popupText2.velocity.Y = -7f;
			popupText2.lifeTime = 60;
			popupText2.context = context;
			if (longText)
			{
				popupText2.lifeTime *= 5;
			}
			popupText2.coinValue = 0L;
			popupText2.coinText = newItem.type >= 71 && newItem.type <= 74;
			if (popupText2.coinText)
			{
				long num3 = 0L;
				if (newItem.type == 71)
				{
					num3 += popupText2.stack;
				}
				else if (newItem.type == 72)
				{
					num3 += 100 * popupText2.stack;
				}
				else if (newItem.type == 73)
				{
					num3 += 10000 * popupText2.stack;
				}
				else if (newItem.type == 74)
				{
					num3 += 1000000 * popupText2.stack;
				}
				popupText2.AddToCoinValue(num3);
				popupText2.ValueToName();
				popupText2.stack = 1L;
				if (popupText2.coinValue >= 1000000)
				{
					if (popupText2.lifeTime < 300)
					{
						popupText2.lifeTime = 300;
					}
					popupText2.color = new Color(220, 220, 198);
				}
				else if (popupText2.coinValue >= 10000)
				{
					if (popupText2.lifeTime < 240)
					{
						popupText2.lifeTime = 240;
					}
					popupText2.color = new Color(224, 201, 92);
				}
				else if (popupText2.coinValue >= 100)
				{
					if (popupText2.lifeTime < 180)
					{
						popupText2.lifeTime = 180;
					}
					popupText2.color = new Color(181, 192, 193);
				}
				else if (popupText2.coinValue >= 1)
				{
					if (popupText2.lifeTime < 120)
					{
						popupText2.lifeTime = 120;
					}
					popupText2.color = new Color(246, 138, 96);
				}
			}
			PrepareEffects(context, newItem, popupText2);
			popupText2.PrepareDisplayText();
		}
		return num2;
	}

	private static void PrepareEffects(PopupTextContext context, Item newItem, PopupText somePopup)
	{
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		if (newItem.rare == -13)
		{
			somePopup.master = true;
		}
		somePopup.expert = newItem.expert;
		CraftingEffectDetails effectDetails = CraftingEffects.GetEffectDetails(newItem);
		if (!somePopup.coinText)
		{
			somePopup.color = Item.GetPopupRarityColor(effectDetails.Rarity);
		}
		if (context == PopupTextContext.ItemCraft)
		{
			somePopup.effectIntensity = effectDetails.Intensity;
			somePopup.effectStyle = effectDetails.Style;
		}
	}

	private void PrepareDisplayText()
	{
		displayText = name;
		if (stack > 1)
		{
			displayText = displayText + " (" + stack + ")";
		}
		if (AnyEffect)
		{
			PrepareTextEffects();
		}
	}

	private void PrepareTextEffects()
	{
		int length = displayText.Length;
		if (charOffsets == null)
		{
			charOffsets = new Vector2[length];
		}
		Array.Resize(ref charOffsets, length);
		if (charColors == null)
		{
			charColors = new Color[length];
		}
		Array.Resize(ref charColors, length);
	}

	private static void EmitFancyFlashDust(PopupText somePopup)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0180: Unknown result type (might be due to invalid IL or missing references)
		//IL_0185: Unknown result type (might be due to invalid IL or missing references)
		//IL_01df: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_021b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0234: Unknown result type (might be due to invalid IL or missing references)
		//IL_0240: Unknown result type (might be due to invalid IL or missing references)
		//IL_026a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0282: Unknown result type (might be due to invalid IL or missing references)
		//IL_0288: Unknown result type (might be due to invalid IL or missing references)
		//IL_028a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0318: Unknown result type (might be due to invalid IL or missing references)
		//IL_031d: Unknown result type (might be due to invalid IL or missing references)
		Vector2 textHitbox = somePopup.GetTextHitbox();
		float num = 1f / somePopup.scale;
		textHitbox *= num;
		int num2 = 6 + somePopup.effectIntensity / 2;
		int num3 = -3 + somePopup.effectIntensity;
		num3 *= 4;
		if (num3 < 0)
		{
			num3 = 0;
		}
		num2 -= num3;
		if (somePopup.effectStyle == PopupEffectStyle.Potion)
		{
			num2 = 0;
			num3 = 0;
		}
		for (int i = 0; i < num2; i++)
		{
			float num4 = -0.1f + 1.2f * Main.rand.NextFloat();
			float num5 = somePopup.position.X + textHitbox.X * num4;
			float num6 = somePopup.position.Y + textHitbox.Y * (1f + 0.4f * (float)Math.Sin(num4 * (float)Math.PI));
			Dust dust = Dust.NewDustPerfect(new Vector2(num5, num6), 306, new Vector2(0f, Main.rand.NextFloatDirection()), 0, somePopup.color, 2f);
			dust.noGravity = true;
			dust.noLight = true;
			dust.noLightEmittance = true;
			dust.velocity.Y += -2f;
			dust.fadeIn = 1.4f * (1f + 0.4f * Main.rand.NextFloat());
			dust.scale = 0.6f + 0.4f * Main.rand.NextFloat();
			if (dust.scale >= 0.9f)
			{
				Dust dust2 = Dust.CloneDust(dust);
				dust2.color = new Color(255, 255, 255, 255);
				dust2.scale *= 0.65f;
				dust2.fadeIn = 1.1f;
			}
		}
		for (int j = 0; j < num3; j++)
		{
			float num7 = -0.1f + 1.2f * Main.rand.NextFloat();
			float num8 = somePopup.position.X + textHitbox.X * num7;
			float num9 = somePopup.position.Y + textHitbox.Y * (0.6f + 0.4f * (float)Math.Sin(num7 * (float)Math.PI));
			Dust dust3 = Dust.NewDustPerfect(new Vector2(num8, num9), 306, new Vector2(0f, Main.rand.NextFloatDirection()), 0, somePopup.color, 2f);
			dust3.noLight = true;
			dust3.noLightEmittance = true;
			dust3.velocity.X = dust3.velocity.RotatedBy((float)Math.PI * 2f * Main.rand.NextFloatDirection()).X;
			dust3.velocity.Y += -2f;
			dust3.fadeIn = 2.4f * (1f + 0.4f * Main.rand.NextFloat());
			dust3.scale = 0.6f + 0.4f * Main.rand.NextFloat();
			if (dust3.scale >= 0.9f)
			{
				Dust dust4 = Dust.CloneDust(dust3);
				dust4.color = new Color(255, 255, 255, 255);
				dust4.scale *= 0.65f;
				dust4.fadeIn = 1.1f;
			}
		}
	}

	private void AddToCoinValue(long addedValue)
	{
		long val = coinValue + addedValue;
		coinValue = Math.Min(9999999999L, Math.Max(0L, val));
	}

	private static int FindNextItemTextSlot()
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

	public static void AssignAsSonarText(int sonarTextIndex)
	{
		sonarText = sonarTextIndex;
		if (sonarText > -1)
		{
			popupText[sonarText].sonar = true;
		}
	}

	public static string ValueToName(long coinValue)
	{
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		int num4 = 0;
		string text = "";
		long num5 = coinValue;
		while (num5 > 0)
		{
			if (num5 >= 1000000)
			{
				num5 -= 1000000;
				num++;
			}
			else if (num5 >= 10000)
			{
				num5 -= 10000;
				num2++;
			}
			else if (num5 >= 100)
			{
				num5 -= 100;
				num3++;
			}
			else if (num5 >= 1)
			{
				num5--;
				num4++;
			}
		}
		text = "";
		if (num > 0)
		{
			text = text + num + string.Format(" {0} ", Language.GetTextValue("Currency.Platinum"));
		}
		if (num2 > 0)
		{
			text = text + num2 + string.Format(" {0} ", Language.GetTextValue("Currency.Gold"));
		}
		if (num3 > 0)
		{
			text = text + num3 + string.Format(" {0} ", Language.GetTextValue("Currency.Silver"));
		}
		if (num4 > 0)
		{
			text = text + num4 + string.Format(" {0} ", Language.GetTextValue("Currency.Copper"));
		}
		if (text.Length > 1)
		{
			text = text.Substring(0, text.Length - 1);
		}
		return text;
	}

	private void ValueToName()
	{
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		int num4 = 0;
		long num5 = coinValue;
		while (num5 > 0)
		{
			if (num5 >= 1000000)
			{
				num5 -= 1000000;
				num++;
			}
			else if (num5 >= 10000)
			{
				num5 -= 10000;
				num2++;
			}
			else if (num5 >= 100)
			{
				num5 -= 100;
				num3++;
			}
			else if (num5 >= 1)
			{
				num5--;
				num4++;
			}
		}
		name = "";
		if (num > 0)
		{
			name = name + num + string.Format(" {0} ", Language.GetTextValue("Currency.Platinum"));
		}
		if (num2 > 0)
		{
			name = name + num2 + string.Format(" {0} ", Language.GetTextValue("Currency.Gold"));
		}
		if (num3 > 0)
		{
			name = name + num3 + string.Format(" {0} ", Language.GetTextValue("Currency.Silver"));
		}
		if (num4 > 0)
		{
			name = name + num4 + string.Format(" {0} ", Language.GetTextValue("Currency.Copper"));
		}
		if (name.Length > 1)
		{
			name = name.Substring(0, name.Length - 1);
		}
	}

	public void Update(int whoAmI)
	{
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0101: Unknown result type (might be due to invalid IL or missing references)
		//IL_010f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0116: Unknown result type (might be due to invalid IL or missing references)
		//IL_011d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0161: Unknown result type (might be due to invalid IL or missing references)
		//IL_0166: Unknown result type (might be due to invalid IL or missing references)
		//IL_0176: Unknown result type (might be due to invalid IL or missing references)
		//IL_0191: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d2: Unknown result type (might be due to invalid IL or missing references)
		if (!active)
		{
			return;
		}
		framesSinceSpawn++;
		float targetScale = TargetScale;
		alpha += (float)alphaDir * 0.01f;
		if ((double)alpha <= 0.7)
		{
			alpha = 0.7f;
			alphaDir = 1;
		}
		if (alpha >= 1f)
		{
			alpha = 1f;
			alphaDir = -1;
		}
		if (expert)
		{
			color = new Color((int)(byte)Main.DiscoR, (int)(byte)Main.DiscoG, (int)(byte)Main.DiscoB, (int)Main.mouseTextColor);
		}
		else if (master)
		{
			color = new Color(255, (int)(byte)(Main.masterColor * 200f), 0, (int)Main.mouseTextColor);
		}
		bool flag = false;
		Vector2 textHitbox = GetTextHitbox();
		Rectangle val = new Rectangle((int)(position.X - textHitbox.X / 2f), (int)(position.Y - textHitbox.Y / 2f), (int)textHitbox.X, (int)textHitbox.Y);
		if (AnyEffect && framesSinceSpawn == 8)
		{
			EmitFancyFlashDust(this);
		}
		for (int i = 0; i < 20; i++)
		{
			PopupText popupText = PopupText.popupText[i];
			if (!popupText.active || i == whoAmI)
			{
				continue;
			}
			Vector2 textHitbox2 = popupText.GetTextHitbox();
			Rectangle val2 = new Rectangle((int)(popupText.position.X - textHitbox2.X / 2f), (int)(popupText.position.Y - textHitbox2.Y / 2f), (int)textHitbox2.X, (int)textHitbox2.Y);
			if (val.Intersects(val2) && (position.Y < popupText.position.Y || (position.Y == popupText.position.Y && whoAmI < i)))
			{
				flag = true;
				int num = numActive;
				if (num > 3)
				{
					num = 3;
				}
				popupText.lifeTime = activeTime + 15 * num;
				lifeTime = activeTime + 15 * num;
			}
		}
		if (!flag)
		{
			velocity.Y *= 0.86f;
			if (scale == targetScale)
			{
				velocity.Y *= 0.4f;
			}
		}
		else if (velocity.Y > -6f)
		{
			velocity.Y -= 0.2f;
		}
		else
		{
			velocity.Y *= 0.86f;
		}
		velocity.X *= 0.93f;
		position += velocity;
		lifeTime--;
		if (lifeTime <= 0)
		{
			scale -= 0.03f * targetScale;
			if ((double)scale < 0.1 * (double)targetScale)
			{
				active = false;
				if (sonarText == whoAmI)
				{
					sonarText = -1;
				}
			}
			lifeTime = 0;
		}
		else
		{
			if (scale < targetScale)
			{
				scale += 0.1f * targetScale;
			}
			if (scale > targetScale)
			{
				scale = targetScale;
			}
		}
	}

	private Vector2 GetTextHitbox()
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		string text = displayText;
		Vector2 val = FontAssets.MouseText.Value.MeasureString(text);
		val *= scale;
		val.Y *= 0.8f;
		return val;
	}

	public static void UpdateItemText()
	{
		int num = 0;
		for (int i = 0; i < 20; i++)
		{
			if (popupText[i].active)
			{
				num++;
				popupText[i].Update(i);
			}
		}
		numActive = num;
	}

	public static void ClearAll()
	{
		for (int i = 0; i < 20; i++)
		{
			popupText[i] = new PopupText();
		}
		numActive = 0;
	}

	public static void DrawItemTextPopups(float scaleTarget)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0136: Unknown result type (might be due to invalid IL or missing references)
		//IL_0140: Unknown result type (might be due to invalid IL or missing references)
		//IL_0145: Unknown result type (might be due to invalid IL or missing references)
		//IL_0150: Unknown result type (might be due to invalid IL or missing references)
		//IL_015a: Unknown result type (might be due to invalid IL or missing references)
		//IL_015f: Unknown result type (might be due to invalid IL or missing references)
		//IL_016a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0174: Unknown result type (might be due to invalid IL or missing references)
		//IL_0179: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_023d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0244: Unknown result type (might be due to invalid IL or missing references)
		//IL_0249: Unknown result type (might be due to invalid IL or missing references)
		//IL_024a: Unknown result type (might be due to invalid IL or missing references)
		//IL_024f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0251: Unknown result type (might be due to invalid IL or missing references)
		//IL_0256: Unknown result type (might be due to invalid IL or missing references)
		//IL_0280: Unknown result type (might be due to invalid IL or missing references)
		//IL_0285: Unknown result type (might be due to invalid IL or missing references)
		//IL_028a: Unknown result type (might be due to invalid IL or missing references)
		//IL_028c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0293: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02df: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0373: Unknown result type (might be due to invalid IL or missing references)
		//IL_0378: Unknown result type (might be due to invalid IL or missing references)
		//IL_0382: Unknown result type (might be due to invalid IL or missing references)
		//IL_0387: Unknown result type (might be due to invalid IL or missing references)
		//IL_0390: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_056e: Unknown result type (might be due to invalid IL or missing references)
		//IL_057a: Unknown result type (might be due to invalid IL or missing references)
		//IL_059b: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_05bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_05cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0607: Unknown result type (might be due to invalid IL or missing references)
		//IL_060e: Unknown result type (might be due to invalid IL or missing references)
		//IL_061b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0633: Unknown result type (might be due to invalid IL or missing references)
		//IL_0635: Unknown result type (might be due to invalid IL or missing references)
		//IL_0637: Unknown result type (might be due to invalid IL or missing references)
		//IL_0656: Unknown result type (might be due to invalid IL or missing references)
		//IL_065d: Unknown result type (might be due to invalid IL or missing references)
		//IL_066a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0676: Unknown result type (might be due to invalid IL or missing references)
		//IL_067d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0687: Unknown result type (might be due to invalid IL or missing references)
		//IL_069a: Unknown result type (might be due to invalid IL or missing references)
		//IL_069c: Unknown result type (might be due to invalid IL or missing references)
		//IL_069e: Unknown result type (might be due to invalid IL or missing references)
		//IL_06bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_06c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_06d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_06dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_06e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_06fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0700: Unknown result type (might be due to invalid IL or missing references)
		//IL_0702: Unknown result type (might be due to invalid IL or missing references)
		//IL_0713: Unknown result type (might be due to invalid IL or missing references)
		//IL_071a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0727: Unknown result type (might be due to invalid IL or missing references)
		//IL_0739: Unknown result type (might be due to invalid IL or missing references)
		//IL_073b: Unknown result type (might be due to invalid IL or missing references)
		//IL_073d: Unknown result type (might be due to invalid IL or missing references)
		//IL_074e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0755: Unknown result type (might be due to invalid IL or missing references)
		//IL_0762: Unknown result type (might be due to invalid IL or missing references)
		//IL_077a: Unknown result type (might be due to invalid IL or missing references)
		//IL_077c: Unknown result type (might be due to invalid IL or missing references)
		//IL_077e: Unknown result type (might be due to invalid IL or missing references)
		//IL_079d: Unknown result type (might be due to invalid IL or missing references)
		//IL_07a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_07b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_07bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_07c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_07e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_07e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_07e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0804: Unknown result type (might be due to invalid IL or missing references)
		//IL_080b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0818: Unknown result type (might be due to invalid IL or missing references)
		//IL_0824: Unknown result type (might be due to invalid IL or missing references)
		//IL_082b: Unknown result type (might be due to invalid IL or missing references)
		//IL_03dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_03eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0402: Unknown result type (might be due to invalid IL or missing references)
		//IL_0971: Unknown result type (might be due to invalid IL or missing references)
		//IL_0976: Unknown result type (might be due to invalid IL or missing references)
		//IL_045f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0923: Unknown result type (might be due to invalid IL or missing references)
		//IL_092c: Unknown result type (might be due to invalid IL or missing references)
		//IL_092e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0935: Unknown result type (might be due to invalid IL or missing references)
		//IL_093a: Unknown result type (might be due to invalid IL or missing references)
		//IL_087f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0886: Unknown result type (might be due to invalid IL or missing references)
		//IL_088d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0892: Unknown result type (might be due to invalid IL or missing references)
		//IL_040c: Unknown result type (might be due to invalid IL or missing references)
		//IL_040e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0410: Unknown result type (might be due to invalid IL or missing references)
		//IL_0415: Unknown result type (might be due to invalid IL or missing references)
		//IL_0419: Unknown result type (might be due to invalid IL or missing references)
		//IL_041e: Unknown result type (might be due to invalid IL or missing references)
		//IL_042f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0436: Unknown result type (might be due to invalid IL or missing references)
		//IL_0440: Unknown result type (might be due to invalid IL or missing references)
		//IL_0442: Unknown result type (might be due to invalid IL or missing references)
		//IL_0479: Unknown result type (might be due to invalid IL or missing references)
		//IL_09ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_09f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_09fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_09ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a4b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a54: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a56: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a5d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a62: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b32: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b34: Unknown result type (might be due to invalid IL or missing references)
		//IL_0af2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0afb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0afd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b04: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b09: Unknown result type (might be due to invalid IL or missing references)
		//IL_0483: Unknown result type (might be due to invalid IL or missing references)
		//IL_0485: Unknown result type (might be due to invalid IL or missing references)
		//IL_0487: Unknown result type (might be due to invalid IL or missing references)
		//IL_048c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0490: Unknown result type (might be due to invalid IL or missing references)
		//IL_0495: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_04be: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_04cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0be2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0be4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bdd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b95: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b9c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ba8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0baf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bb4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c27: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c51: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c53: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d72: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d7c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d86: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d8e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d93: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d9c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c6a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c6c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cb0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cba: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cc4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ccc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cd1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cd3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cda: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ce6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d16: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d20: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d2a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d32: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d37: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d40: Unknown result type (might be due to invalid IL or missing references)
		//IL_0de9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0df3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dfd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e05: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e0a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e11: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e16: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e4b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e4d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e60: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e76: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e7b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e8a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e93: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e9f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ea9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ebe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ec0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ed3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ee9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0eee: Unknown result type (might be due to invalid IL or missing references)
		//IL_0efd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f06: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f12: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f1c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e1d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e27: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e2c: Unknown result type (might be due to invalid IL or missing references)
		SpriteBatch spriteBatch = Main.spriteBatch;
		Vector2 screenPosition = Main.screenPosition;
		int screenHeight = Main.screenHeight;
		for (int i = 0; i < 20; i++)
		{
			PopupText popupText = PopupText.popupText[i];
			if (!popupText.active)
			{
				continue;
			}
			string text = popupText.displayText;
			Vector2 val = FontAssets.MouseText.Value.MeasureString(text);
			Vector2 val2 = new Vector2(val.X * 0.5f, val.Y * 0.5f);
			float num = scaleTarget;
			float num2 = popupText.scale / num;
			int num3 = (int)(255f - 255f * num2);
			float num4 = (int)popupText.color.R;
			_ = (float)(int)popupText.color.G;
			_ = (float)(int)popupText.color.B;
			float num5 = (int)popupText.color.A;
			num4 *= num2 * popupText.alpha * 0.3f;
			_ = popupText.alpha;
			_ = popupText.alpha;
			num5 *= num2 * popupText.alpha;
			Color val3 = Color.Black;
			float num6 = 1f;
			Texture2D val4 = null;
			Vector2[] array = null;
			Color[] array2 = null;
			switch (popupText.context)
			{
			case PopupTextContext.ItemPickupToVoidContainer:
				val3 = new Color(127, 20, 255) * 0.4f;
				num6 = 0.8f;
				break;
			case PopupTextContext.SonarAlert:
				val3 = Color.Blue * 0.4f;
				if (popupText.npcNetID != 0)
				{
					val3 = Color.Red * 0.4f;
				}
				num6 = 1f;
				break;
			case PopupTextContext.ItemReforge_Best:
				val3 = Main.hslToRgb(Main.GlobalTimeWrappedHourly * 0.6f % 1f, 1f, 0.6f) * 0.6f;
				num *= 0.5f;
				num6 = 0.8f;
				break;
			}
			int num7 = 40;
			float num8 = Utils.Remap(popupText.framesSinceSpawn, 0f, num7, 0f, 1f);
			float num9 = (float)Utils.EaseOutCirc(num8);
			if (popupText.effectStyle == PopupEffectStyle.Metal || popupText.effectStyle == PopupEffectStyle.MagicWeapon || popupText.effectStyle == PopupEffectStyle.RangedWeapon || popupText.effectStyle == PopupEffectStyle.SummonWeapon || popupText.effectStyle == PopupEffectStyle.MeleeWeapon)
			{
				Vector2 val5 = new Vector2(0f, -4f);
				Vector2 val6 = popupText.position - screenPosition + val2;
				float num10 = popupText.scale;
				num6 = (float)Utils.Lerp(0.6000000238418579, 1.0, num8);
				Vector3 val7 = Main.rgbToHsl(popupText.color);
				Color val8 = Main.hslToRgb(val7.X, val7.Y, 1f - num8);
				val8.A = 0;
				float num11 = (float)Utils.EaseInCirc(Utils.Clamp(num8 * 1.25f, 0f, 1f));
				val3 = Color.Lerp(val8, Color.Black, num11);
				float num12 = Utils.Remap(num8, 0f, 0.1f, 0f, 1f) * Utils.Remap(num8, 0.1f, 1f, 1f, 0f);
				float num13 = Utils.Remap(num8, 0f, 0.2f, 0f, 1f) * Utils.Remap(num8, 0.2f, 0.8f, 1f, 0f);
				Texture2D value = TextureAssets.Extra[98].Value;
				Vector2 val9 = value.Frame().Size() / 2f;
				Vector2 val10 = new Vector2(1f, val.X / (float)value.Width);
				val10 *= num10;
				if (num12 > 0f)
				{
					Vector2 val11 = new Vector2(Utils.Remap(num9, 0f, 1f, -20f, 20f), 0f) + val5;
					val11 *= num10;
					Vector2 val12 = new Vector2(-60f, 0f);
					while (val12.X <= 40f)
					{
						spriteBatch.Draw(value, val6 + val11 + val12 * num10, (Rectangle?)null, popupText.color * num12, (float)Math.PI / 2f, val9, val10, (SpriteEffects)0, 0f);
						val12.X += 40f;
					}
					Vector2 val13 = new Vector2(-20f, 0f);
					while (val13.X <= 20f)
					{
						spriteBatch.Draw(value, val6 + val11 + val13 * num10, (Rectangle?)null, new Color(255, 255, 255, 0) * 0.5f * num13, (float)Math.PI / 2f, val9, val10 * 0.5f, (SpriteEffects)0, 0f);
						val13.X += 20f;
					}
				}
				float num14 = (float)Math.PI * 2f * num8;
				float fromValue = (float)Utils.EaseOutCirc(Utils.Clamp(num8 * 2f, 0f, 1f));
				float num15 = Utils.Remap(num8, 0.1f, 0.3f, 0f, 1f) * Utils.Remap(num8, 0.3f, 0.6f, 1f, 0f);
				Vector2 val14 = new Vector2(val.X, 0f) * Utils.Remap(fromValue, 0f, 1f, -0.8f, 0.4f) + val5;
				val14 *= num10;
				spriteBatch.Draw(value, val6 + val14, (Rectangle?)null, popupText.color * num15, 0f + num14, val9, num10, (SpriteEffects)0, 0f);
				spriteBatch.Draw(value, val6 + val14, (Rectangle?)null, popupText.color * num15, (float)Math.PI / 2f + num14, val9, num10 * 1.3f, (SpriteEffects)0, 0f);
				spriteBatch.Draw(value, val6 + val14, (Rectangle?)null, new Color(255, 255, 255, 0) * num15, 0f + num14, val9, new Vector2(0.5f, 0.5f) * num10 * 1.3f, (SpriteEffects)0, 0f);
				spriteBatch.Draw(value, val6 + val14, (Rectangle?)null, new Color(255, 255, 255, 0) * num15, (float)Math.PI / 2f + num14, val9, new Vector2(0.5f, 0.5f) * num10, (SpriteEffects)0, 0f);
				num14 = 0f;
				spriteBatch.Draw(value, val6 + val14, (Rectangle?)null, popupText.color * num15, 0f + num14, val9, num10, (SpriteEffects)0, 0f);
				spriteBatch.Draw(value, val6 + val14, (Rectangle?)null, popupText.color * num15, (float)Math.PI / 2f + num14, val9, num10 * 1.3f, (SpriteEffects)0, 0f);
				spriteBatch.Draw(value, val6 + val14, (Rectangle?)null, new Color(255, 255, 255, 0) * num15, 0f + num14, val9, new Vector2(0.5f, 0.5f) * num10 * 1.3f, (SpriteEffects)0, 0f);
				spriteBatch.Draw(value, val6 + val14, (Rectangle?)null, new Color(255, 255, 255, 0) * num15, (float)Math.PI / 2f + num14, val9, new Vector2(0.5f, 0.5f) * num10, (SpriteEffects)0, 0f);
				array2 = popupText.charColors;
				float num16 = 1f / (float)text.Length;
				for (int j = 0; j < text.Length; j++)
				{
					float num17 = Utils.Remap(num9, num16 * (float)j, num16 * (float)(j + 1), 0f, 1f);
					array2[j] = Color.Lerp(Color.White, popupText.color, num17);
				}
			}
			if (popupText.effectStyle == PopupEffectStyle.MagicWeapon)
			{
				array = popupText.charOffsets;
				float num18 = 1f / (float)text.Length;
				for (int k = 0; k < text.Length; k++)
				{
					Utils.Remap(num9 * 1.25f, num18 * (float)k, num18 * (float)(k + 1), 0f, 1f);
					Vector2 val15 = new Vector2((0f - (float)Math.Sin((float)Math.PI * 2f * ((float)(k * 31) / 12f))) * 144f, 0f);
					array[k] = Vector2.Lerp(val15, Vector2.Zero, num9);
				}
			}
			if (popupText.effectStyle == PopupEffectStyle.RangedWeapon)
			{
				array = popupText.charOffsets;
				array2 = popupText.charColors;
				val3 = Color.Transparent;
				float num19 = 1f / (float)text.Length;
				float num20 = Utils.Clamp(num9 * 3f, 0f, 1f);
				float num21 = Utils.Clamp(num9 * 1.5f, 0f, 1f);
				for (int l = 0; l < text.Length; l++)
				{
					float num22 = Utils.Remap(num9, num19 * (float)l, num19 * (float)(l + 1), 0f, 1f);
					array2[l] = Color.Lerp(new Color(0, 0, 0, 1), popupText.color, num22);
					Vector2 val16 = new Vector2(60f * num22 - 120f * num20, ((float)Math.Sin((float)Math.PI * 2f * ((float)(l * 31) / 12f)) * 0.5f + 0.5f) * -204f * (1f - num21));
					array[l] = Vector2.Lerp(val16, Vector2.Zero, num9);
				}
			}
			if (popupText.effectStyle == PopupEffectStyle.Potion)
			{
				array = popupText.charOffsets;
				float num23 = 1f / (float)text.Length;
				for (int m = 0; m < text.Length; m++)
				{
					Utils.Remap(num9 * 1.25f, num23 * (float)m, num23 * (float)(m + 1), 0f, 1f);
					Vector2 val17 = new Vector2(0f, (float)Math.Sin((float)Math.PI * 2f * ((float)m / 12f)) * 20f);
					array[m] = Vector2.Lerp(val17, Vector2.Zero, num9);
				}
			}
			float num24 = (float)num3 / 255f;
			for (int n = 0; n < 5; n++)
			{
				Color val18 = val3;
				float num25 = 0f;
				float num26 = 0f;
				switch (n)
				{
				case 0:
					num25 -= num * 2f;
					break;
				case 1:
					num25 += num * 2f;
					break;
				case 2:
					num26 -= num * 2f;
					break;
				case 3:
					num26 += num * 2f;
					break;
				default:
					val18 = popupText.color * num2 * popupText.alpha * num6;
					break;
				}
				if (n < 4)
				{
					num5 = (float)(int)popupText.color.A * num2 * popupText.alpha;
					val18 = new Color(0, 0, 0, (int)num5);
				}
				if (val3 != Color.Black && n < 4)
				{
					num25 *= 1.3f + 1.3f * num24;
					num26 *= 1.3f + 1.3f * num24;
				}
				float num27 = popupText.position.Y - screenPosition.Y + num26;
				if (Main.player[Main.myPlayer].gravDir == -1f)
				{
					num27 = (float)screenHeight - num27;
				}
				if (val3 != Color.Black && n < 4)
				{
					Color val19 = val3;
					val19.A = (byte)MathHelper.Lerp(60f, 127f, Utils.GetLerpValue(0f, 255f, num5, clamped: true));
					DynamicSpriteFontExtensionMethods.DrawString(spriteBatch, FontAssets.MouseText.Value, text, new Vector2(popupText.position.X - screenPosition.X + num25 + val2.X, num27 + val2.Y), Color.Lerp(val18, val19, 0.5f), popupText.rotation, val2, popupText.scale, (SpriteEffects)0, 0f, array, (Color[])null);
					DynamicSpriteFontExtensionMethods.DrawString(spriteBatch, FontAssets.MouseText.Value, text, new Vector2(popupText.position.X - screenPosition.X + num25 + val2.X, num27 + val2.Y), val19, popupText.rotation, val2, popupText.scale, (SpriteEffects)0, 0f, array, (Color[])null);
				}
				else
				{
					DynamicSpriteFontExtensionMethods.DrawString(spriteBatch, FontAssets.MouseText.Value, text, new Vector2(popupText.position.X - screenPosition.X + num25 + val2.X, num27 + val2.Y), val18, popupText.rotation, val2, popupText.scale, (SpriteEffects)0, 0f, array, (n == 4) ? array2 : null);
				}
				if (val4 != null)
				{
					float num28 = (1.3f - num24) * popupText.scale * 0.7f;
					Vector2 val20 = new Vector2(popupText.position.X - screenPosition.X + num25 + val2.X, num27 + val2.Y);
					Color val21 = val3 * 0.6f;
					if (n == 4)
					{
						val21 = Color.White * 0.6f;
					}
					val21.A = (byte)((float)(int)val21.A * 0.5f);
					int num29 = 25;
					spriteBatch.Draw(val4, val20 + new Vector2(val2.X * -0.5f - (float)num29 - val4.Size().X / 2f, 0f), (Rectangle?)null, val21 * popupText.scale, 0f, val4.Size() / 2f, num28, (SpriteEffects)0, 0f);
					spriteBatch.Draw(val4, val20 + new Vector2(val2.X * 0.5f + (float)num29 + val4.Size().X / 2f, 0f), (Rectangle?)null, val21 * popupText.scale, 0f, val4.Size() / 2f, num28, (SpriteEffects)0, 0f);
				}
			}
		}
	}
}
