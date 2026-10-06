using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Terraria.Audio;
using Terraria.GameContent.UI.Elements;
using Terraria.Localization;
using Terraria.UI;
using Terraria.UI.Chat;

namespace Terraria.GameContent.UI.States;

public class UITextWrappingTest : UIState
{
	private enum Mode
	{
		UIText,
		SignsAndNPCChat,
		WordwrapStringLegacy,
		DrawColorCodedStringWithShadow,
		DrawColorCodedStringLegacy,
		MultilineChat
	}

	private class TestElement : UIElement
	{
		private readonly string text;

		private readonly float scale;

		private readonly Mode mode;

		public Action OnHeightUpdate;

		public TestElement(string text, float scale, Mode mode)
		{
			this.text = text;
			this.scale = scale;
			this.mode = mode;
		}

		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_026a: Unknown result type (might be due to invalid IL or missing references)
			//IL_026b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0275: Unknown result type (might be due to invalid IL or missing references)
			//IL_0280: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_02fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_031e: Unknown result type (might be due to invalid IL or missing references)
			//IL_031f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0329: Unknown result type (might be due to invalid IL or missing references)
			//IL_0334: Unknown result type (might be due to invalid IL or missing references)
			//IL_033b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0357: Unknown result type (might be due to invalid IL or missing references)
			//IL_035d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0362: Unknown result type (might be due to invalid IL or missing references)
			//IL_036a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0197: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_0157: Unknown result type (might be due to invalid IL or missing references)
			//IL_015d: Unknown result type (might be due to invalid IL or missing references)
			//IL_016a: Unknown result type (might be due to invalid IL or missing references)
			//IL_016f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0174: Unknown result type (might be due to invalid IL or missing references)
			//IL_020b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0217: Unknown result type (might be due to invalid IL or missing references)
			//IL_021c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0226: Unknown result type (might be due to invalid IL or missing references)
			//IL_0231: Unknown result type (might be due to invalid IL or missing references)
			Vector2 val = GetDimensions().Position();
			float num = GetInnerDimensions().Width;
			if (num <= 0f)
			{
				num = 1000f;
			}
			switch (mode)
			{
			case Mode.SignsAndNPCChat:
			{
				string[] array = Utils.WordwrapString(text, FontAssets.MouseText.Value, (int)(num / scale), 10, out var lineAmount);
				float num3 = 30f * scale;
				MinHeight.Set((float)lineAmount * num3, 0f);
				OnHeightUpdate();
				for (int j = 0; j < lineAmount; j++)
				{
					Utils.DrawBorderStringFourWay(spriteBatch, FontAssets.MouseText.Value, array[j], val.X, val.Y + (float)j * num3, Color.White, Color.Black, Vector2.Zero, scale);
				}
				break;
			}
			case Mode.WordwrapStringLegacy:
			{
				string[] array2 = Utils.WordwrapStringLegacy(text, FontAssets.MouseText.Value, (int)(num / scale), 10, out var lineAmount2);
				float num4 = 30f * scale;
				MinHeight.Set((float)lineAmount2 * num4, 0f);
				OnHeightUpdate();
				for (int k = 0; k < lineAmount2; k++)
				{
					Utils.DrawBorderStringFourWay(spriteBatch, FontAssets.MouseText.Value, array2[k], val.X, val.Y + (float)k * num4, Color.White, Color.Black, Vector2.Zero, scale);
				}
				break;
			}
			case Mode.MultilineChat:
			{
				List<List<TextSnippet>> list = Utils.WordwrapStringSmart(text, Color.White, FontAssets.MouseText.Value, (int)(num / scale), 10);
				float num2 = 30f * scale;
				MinHeight.Set((float)list.Count * num2, 0f);
				OnHeightUpdate();
				for (int i = 0; i < list.Count; i++)
				{
					ChatManager.DrawColorCodedStringWithShadow(spriteBatch, FontAssets.MouseText.Value, list[i].ToArray(), val + new Vector2(0f, (float)i * num2), 0f, Vector2.Zero, new Vector2(scale), out var _);
				}
				break;
			}
			case Mode.DrawColorCodedStringWithShadow:
			{
				ChatManager.DrawColorCodedStringWithShadow(spriteBatch, FontAssets.MouseText.Value, text, val, Color.White, 0f, Vector2.Zero, new Vector2(scale), num);
				Vector2 stringSize2 = ChatManager.GetStringSize(FontAssets.MouseText.Value, text, new Vector2(scale), num);
				MinHeight.Set(stringSize2.Y, 0f);
				OnHeightUpdate();
				break;
			}
			case Mode.DrawColorCodedStringLegacy:
			{
				ChatManager.DrawColorCodedStringShadow(spriteBatch, FontAssets.MouseText.Value, text, val, Color.Black, 0f, Vector2.Zero, new Vector2(scale), num);
				ChatManager.DrawColorCodedString(spriteBatch, FontAssets.MouseText.Value, text, val, Color.White, 0f, Vector2.Zero, new Vector2(scale), num);
				Vector2 stringSize = ChatManager.GetStringSize(FontAssets.MouseText.Value, text, new Vector2(scale), num);
				MinHeight.Set(stringSize.Y, 0f);
				OnHeightUpdate();
				break;
			}
			}
		}
	}

	private static readonly float TextPadding = 12f;

	private UIList list;

	private UIText modeText;

	private UIText scaleText;

	private UIText langText;

	private Mode mode;

	private int scale = 100;

	private string ScaleText => "Up/Down to change scale. Current: " + scale + "%";

	private string LangText => "Current Language: " + Language.ActiveCulture.CultureInfo.DisplayName;

	public UITextWrappingTest()
	{
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		UIPanel uIPanel = new UIPanel
		{
			Top = StyleDimension.FromPixels(100f),
			Left = StyleDimension.FromPixelsAndPercent(-400f, 0.5f),
			Width = StyleDimension.FromPixels(300f),
			Height = StyleDimension.FromPixels(40f),
			BackgroundColor = new Color(43, 56, 101),
			BorderColor = Color.Transparent
		};
		modeText = new UIText(mode.ToString(), 0.8f)
		{
			TextOriginX = 0f,
			Width = StyleDimension.FromPercent(1f),
			Height = StyleDimension.FromPercent(1f)
		};
		uIPanel.Append(modeText);
		uIPanel.OnLeftClick += (UIMouseEvent e, UIElement sender) =>
		{
			CycleMode(1);
		};
		uIPanel.OnRightClick += (UIMouseEvent e, UIElement sender) =>
		{
			CycleMode(-1);
		};
		Append(uIPanel);
		scaleText = new UIText(ScaleText, 0.8f)
		{
			TextOriginX = 0f,
			Top = StyleDimension.FromPixels(150f),
			Left = StyleDimension.FromPixelsAndPercent(-400f, 0.5f),
			Width = StyleDimension.FromPixels(300f),
			Height = StyleDimension.FromPixels(40f)
		};
		Append(scaleText);
		langText = new UIText(LangText, 0.8f)
		{
			TextOriginX = 1f,
			HAlign = 1f,
			Top = StyleDimension.FromPixels(150f),
			Left = StyleDimension.FromPixelsAndPercent(400f, -0.5f),
			Width = StyleDimension.FromPixels(300f),
			Height = StyleDimension.FromPixels(40f)
		};
		Append(langText);
		list = new UIList
		{
			Top = StyleDimension.FromPixels(200f),
			Left = StyleDimension.FromPixelsAndPercent(-400f, 0.5f),
			Width = StyleDimension.FromPixels(300f),
			Height = StyleDimension.FromPixelsAndPercent(-200f, 1f),
			ListPadding = 5f,
			ManualSortMethod = (List<UIElement> _) =>
			{
			}
		};
		list.SetPadding(0f);
		Append(list);
		UIScrollbar uIScrollbar = new UIScrollbar();
		uIScrollbar.SetView(100f, 1000f);
		uIScrollbar.Height.Set(-20f, 1f);
		uIScrollbar.HAlign = 1f;
		uIScrollbar.VAlign = 0.5f;
		uIScrollbar.Left.Set(6f, 0f);
		list.SetScrollbar(uIScrollbar);
		ResetList();
	}

	private void CycleMode(int offset)
	{
		int length = Enum.GetValues(typeof(Mode)).Length;
		mode = (Mode)((int)(mode + offset + length) % length);
		ResetList();
	}

	private void ResetList()
	{
		modeText.SetText(mode.ToString());
		list.Clear();
		list.Add(MakeElement("A test string in english.\nSecond line.\n\n^ Double line break\nLooooooooooooooonglinewithnospaces"));
		list.Add(MakeElement("Ends with newline\n"));
		list.Add(MakeElement("Non-breaking space: с\u00a0микротранзакциями\n"));
		list.Add(MakeElement("Thin\u2009Space\nHair\u200aSpace\nZero\u200bWidth\u200bSpace"));
		list.Add(NewSeparator());
		list.Add(MakeElement("せいなる スライムが がったいして できた 生き物。ごうまんで 力づよく きらめく けっしょうに おおわれている。つばさが 生える という うわさも ある。"));
		list.Add(MakeElement("정화된 슬라임들이 모두 통합되어, 눈부신 수정으로 장식된 거만하고 압도적인 힘이 되었습니다. 날개가 돋아난다는 소문도 있습니다. "));
		list.Add(MakeElement("Святые слизни объединяются в величественную всесокрушающую массу, украшенную превосходными кристаллами. Говорят, она даже может отрастить крылья."));
		list.Add(MakeElement("神圣史莱姆合并成了一种高傲的粉碎性力量，这种力量佩戴着闪耀的水晶。传说她会长出翅膀。"));
		list.Add(MakeElement("神聖史萊姆融合後，會點綴著閃耀的水晶，擁有傲視一切的粉碎性力量。傳說她會長出翅膀。"));
		list.Add(NewSeparator());
		list.Add(MakeElement("fullwidth terminators。bang！comma，fullstop。rcomma、colon：question？"));
		list.Add(MakeElement("Chinese separation〈聖聖聖聖〉《聖聖》「聖聖」『聖聖』【聖聖〔聖聖】〖聖聖〗!%),.:;?]}$100,25.24%"));
		list.Add(NewSeparator());
		list.Add(MakeElement(new LocalizedText("", "Keybind glyph support {InputTrigger_UseOrAttack} and {InputTrigger_InteractWithTile}").Value));
		list.Add(MakeElement("[c/FF0000:SomeRedText] [c/00FF00:SomeGreenText] [c/0000FF:SomeBlueText]"));
		list.Add(MakeElement("[c/FF0000:SomeRedText][c/00FF00:SomeGreenText][c/0000FF:SomeBlueText]"));
		list.Add(MakeElement("[c/0000FF:Long colored text, with escaped square brackets [\\] inside]"));
		list.Add(MakeElement("Items[i:1][i:2][i:3][i:4][i:5][i:6][i:7][i:100][i:1000]"));
		list.Add(MakeElement("ItemsOnSeparateLines\n[i:1]\n[i:2]\n[i:3]"));
		list.Add(MakeElement("Items and text [i:1] then stuff [i:2] and some more [i:3] etc"));
		list.Add(MakeElement("nospacebetweenitems[i:6]andtext[i:7]nospacebetweenitems[i:8]andtext[i:9]"));
		list.Add(MakeElement("[g:0][g:1][g:2][g:3][g:4][g:5][g:6][g:7][g:8][g:9][g:10][g:11][g:12][g:13][g:14][g:15][g:16][g:17][g:18][g:19][g:20][g:21][g:22][g:23][g:24][g:25]"));
		list.Add(MakeElement(Language.GetTextValue("Achievements.Completed", "[a:TRANSMUTE_ITEM]")));
		list.Add(MakeElement("[a:TO_INFINITY_AND_BEYOND][a:PURIFY_ENTIRE_WORLD][a:TO_INFINITY_AND_BEYOND][a:TRANSMUTE_ITEM][a:OBTAIN_HAMMER][a:BENCHED][a:HEAVY_METAL][a:GET_GOLDEN_DELIGHT][a:MINER_FOR_FIRE][a:HEAD_IN_THE_CLOUDS][a:GET_TERRASPARK_BOOTS]"));
	}

	private UIElement NewSeparator()
	{
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		return new UIHorizontalSeparator
		{
			Width = StyleDimension.FromPixelsAndPercent(0f, 1f),
			Color = new Color(89, 116, 213, 255) * 0.9f
		};
	}

	private UIElement MakeElement(string value)
	{
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		UIElement container = new UIPanel
		{
			Width = StyleDimension.FromPercent(1f),
			Height = StyleDimension.FromPixels(50 * scale),
			BackgroundColor = new Color(43, 56, 101),
			BorderColor = Color.Transparent
		};
		container.SetPadding(TextPadding);
		if (mode == Mode.UIText)
		{
			UIText text = new UIText(value, (float)scale / 100f)
			{
				TextOriginX = 0f,
				HAlign = 0f,
				VAlign = 0f,
				Width = StyleDimension.FromPercent(1f),
				Height = StyleDimension.FromPercent(1f),
				IsWrapped = true
			};
			text.OnInternalTextChange += () =>
			{
				container.Height = new StyleDimension(text.MinHeight.Pixels, 0f);
			};
			container.Append(text);
		}
		else
		{
			TestElement text2 = new TestElement(value, (float)scale / 100f, mode)
			{
				Width = StyleDimension.FromPercent(1f)
			};
			TestElement testElement = text2;
			testElement.OnHeightUpdate = (Action)Delegate.Combine(testElement.OnHeightUpdate, (Action)(() =>
			{
				container.Height = new StyleDimension(text2.MinHeight.Pixels + container.PaddingTop + container.PaddingBottom, 0f);
			}));
			container.Append(text2);
		}
		return container;
	}

	public override void Draw(SpriteBatch spriteBatch)
	{
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		base.Draw(spriteBatch);
		CalculatedStyle dimensions = list.GetDimensions();
		int num = (int)(dimensions.X + TextPadding);
		int num2 = (int)(dimensions.X + dimensions.Width - TextPadding);
		spriteBatch.Draw(TextureAssets.MagicPixel.Value, new Rectangle(num, (int)dimensions.Y, 1, (int)dimensions.Height), Color.Green);
		spriteBatch.Draw(TextureAssets.MagicPixel.Value, new Rectangle(num2, (int)dimensions.Y, 1, (int)dimensions.Height), Color.Green);
	}

	public override void Update(GameTime gameTime)
	{
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
		if (Main.keyState.IsKeyDown((Keys)27))
		{
			SoundEngine.PlaySound(11);
			Main.menuMode = 0;
		}
		int num = 0;
		if (Main.keyState.IsKeyDown((Keys)40) && Main.oldKeyState.IsKeyUp((Keys)40))
		{
			num = -10;
		}
		if (Main.keyState.IsKeyDown((Keys)38) && Main.oldKeyState.IsKeyUp((Keys)38))
		{
			num = 10;
		}
		if (num != 0)
		{
			scale = Utils.Clamp(scale + num, 50, 150);
			ResetList();
			scaleText.SetText(ScaleText);
		}
		langText.SetText(LangText);
		if (Main.mouseLeft)
		{
			Point val = Main.MouseScreen.ToPoint();
			CalculatedStyle dimensions = list.GetDimensions();
			if ((float)val.X > dimensions.X && (float)val.Y > dimensions.Y)
			{
				list.Width = StyleDimension.FromPixels((float)val.X - dimensions.X);
			}
		}
		base.Update(gameTime);
	}
}
