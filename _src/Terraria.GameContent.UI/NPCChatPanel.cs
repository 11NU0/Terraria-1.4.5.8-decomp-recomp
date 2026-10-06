using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using ReLogic.Localization.IME;
using ReLogic.OS;
using Terraria.Audio;
using Terraria.GameContent.UI.States;
using Terraria.GameInput;
using Terraria.Localization;
using Terraria.UI;
using Terraria.UI.Chat;
using Terraria.UI.Gamepad;

namespace Terraria.GameContent.UI;

public class NPCChatPanel
{
	private int textBlinkerCount;

	private int textBlinkerState;

	private List<NPCInteraction> _interactions = new List<NPCInteraction>();

	private TextDisplayCache _textDisplayCache = new TextDisplayCache();

	private int _neededInteractionLines;

	public const int AllowedInteractionsPerLine = 4;

	private int _lastHovered = -1;

	private Player LocalPlayer => Main.LocalPlayer;

	private byte mouseTextColor => Main.mouseTextColor;

	public bool allowRichText => LocalPlayer.talkNPC != -1;

	public bool InVirtualKeyboard
	{
		get
		{
			if (Main.InGameUI.CurrentState is UIVirtualKeyboard)
			{
				return PlayerInput.UsingGamepad;
			}
			return false;
		}
	}

	public void Draw()
	{
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0104: Unknown result type (might be due to invalid IL or missing references)
		//IL_010c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0112: Unknown result type (might be due to invalid IL or missing references)
		//IL_0133: Unknown result type (might be due to invalid IL or missing references)
		//IL_0135: Unknown result type (might be due to invalid IL or missing references)
		//IL_013f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0146: Unknown result type (might be due to invalid IL or missing references)
		//IL_0152: Unknown result type (might be due to invalid IL or missing references)
		//IL_0171: Unknown result type (might be due to invalid IL or missing references)
		//IL_017b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0183: Unknown result type (might be due to invalid IL or missing references)
		//IL_0189: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0201: Unknown result type (might be due to invalid IL or missing references)
		//IL_0203: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e8: Unknown result type (might be due to invalid IL or missing references)
		if (!CanHoldConversation())
		{
			Close();
			return;
		}
		PrepareText();
		PrepareInteractions();
		PrepareVirtualKeyboard();
		Color val = new Color(200, 200, 200, 200);
		int num = (mouseTextColor * 2 + 255) / 3;
		Color val2 = new Color(num, num, num, num);
		Point val3 = new Point(500, 500);
		Rectangle val4 = new Rectangle(Main.screenWidth / 2 - val3.X / 2, 100, val3.X, 30);
		val4.Height += 30 * _textDisplayCache.AmountOfLines;
		val4.Height += 30 * _neededInteractionLines + Math.Max(0, 2 * (_neededInteractionLines - 1));
		if (true)
		{
			Main.spriteBatch.Draw(TextureAssets.ChatBack.Value, val4.TopLeft(), (Rectangle?)new Rectangle(0, 0, TextureAssets.ChatBack.Width(), val4.Height - 30), val, 0f, default(Vector2), 1f, (SpriteEffects)0, 0f);
			Main.spriteBatch.Draw(TextureAssets.ChatBack.Value, new Vector2(val4.TopLeft().X, (float)(val4.Y + val4.Height - 30)), (Rectangle?)new Rectangle(0, TextureAssets.ChatBack.Height() - 30, TextureAssets.ChatBack.Width(), 30), val, 0f, default(Vector2), 1f, (SpriteEffects)0, 0f);
		}
		DrawText(val2, val4);
		Main.DrawNPCPortrait(val, val4.TopLeft());
		Main.DrawNPCChatBottomRightItem(val4.BottomRight());
		if (!PlayerInput.IgnoreMouseInterface && val4.Contains(new Point(Main.mouseX, Main.mouseY)))
		{
			LocalPlayer.mouseInterface = true;
		}
		DrawButtons(val4, val2);
	}

	private void DrawButtons(Rectangle panelArea, Color chatColor)
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_0107: Unknown result type (might be due to invalid IL or missing references)
		//IL_0111: Unknown result type (might be due to invalid IL or missing references)
		//IL_0116: Unknown result type (might be due to invalid IL or missing references)
		//IL_0118: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_013a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0156: Unknown result type (might be due to invalid IL or missing references)
		//IL_012d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0171: Unknown result type (might be due to invalid IL or missing references)
		//IL_017d: Unknown result type (might be due to invalid IL or missing references)
		//IL_018a: Unknown result type (might be due to invalid IL or missing references)
		//IL_019a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0193: Unknown result type (might be due to invalid IL or missing references)
		//IL_019f: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0204: Unknown result type (might be due to invalid IL or missing references)
		//IL_0206: Unknown result type (might be due to invalid IL or missing references)
		//IL_020d: Unknown result type (might be due to invalid IL or missing references)
		//IL_020f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0213: Unknown result type (might be due to invalid IL or missing references)
		//IL_022f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0231: Unknown result type (might be due to invalid IL or missing references)
		//IL_0237: Unknown result type (might be due to invalid IL or missing references)
		//IL_0239: Unknown result type (might be due to invalid IL or missing references)
		//IL_023d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0248: Unknown result type (might be due to invalid IL or missing references)
		//IL_0257: Unknown result type (might be due to invalid IL or missing references)
		//IL_025c: Unknown result type (might be due to invalid IL or missing references)
		//IL_026f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0289: Unknown result type (might be due to invalid IL or missing references)
		//IL_028b: Unknown result type (might be due to invalid IL or missing references)
		//IL_029a: Unknown result type (might be due to invalid IL or missing references)
		//IL_029f: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02df: Unknown result type (might be due to invalid IL or missing references)
		UILinkPointNavigator.Shortcuts.NPCCHAT_ButtonsNew = true;
		UILinkPointNavigator.Shortcuts.NPCCHAT_ButtonsCount = _interactions.Count;
		DynamicSpriteFont value = FontAssets.MouseText.Value;
		Vector2 val = panelArea.BottomLeft() + new Vector2(30f, (float)(-22 * _neededInteractionLines + Math.Max(0, 2 * (_neededInteractionLines - 1)) - 4));
		int num = -1;
		int num2 = -1;
		float num3 = 0.9f;
		Rectangle val2 = new Rectangle((int)val.X, (int)val.Y, 100, 22);
		foreach (NPCInteraction interaction in _interactions)
		{
			num++;
			byte b = mouseTextColor;
			chatColor = new Color((int)b, (int)((double)(int)b / 1.1), b / 2, (int)b);
			if (num % 4 == 0)
			{
				val2.X = (int)val.X;
				val2.Y = num / 4 * 22 + (int)val.Y;
			}
			string text = interaction.GetText();
			int coinValue = 0;
			bool flag = interaction.TryAddCoins(ref chatColor, out coinValue);
			float num4 = 1f;
			Vector2 val3 = ChatManager.GetStringSize(value, text, new Vector2(num3));
			if (val3.X > 260f)
			{
				num4 *= 260f / val3.X;
			}
			val2.Width = (int)(val3.X * num4);
			bool flag2 = val2.Contains(new Point(Main.mouseX, Main.mouseY));
			Vector2 val4 = new Vector2(flag2 ? 1.2f : num3);
			Vector2 origin = new Vector2(0f, val3.Y * 0.5f);
			Color baseColor = (flag2 ? Color.Brown : Color.Black);
			Vector2 val5 = new Vector2((float)val2.Left, (float)val2.Center.Y);
			if (flag2)
			{
				val5.X -= (int)((1.2f - num3) * (float)val2.Width * 0.5f);
				val3 *= 1.2f / num3;
			}
			if (flag2)
			{
				num2 = num;
			}
			ChatManager.DrawColorCodedStringShadow(Main.spriteBatch, value, text, val5, baseColor, 0f, origin, val4 * num4);
			ChatManager.DrawColorCodedString(Main.spriteBatch, value, text, val5, chatColor, 0f, origin, val4 * num4);
			UILinkPointNavigator.SetPosition(2500 + num, val2.Center.ToVector2());
			val2.X += val2.Width + 30;
			if (interaction.ShowExclamation)
			{
				Utils.DrawNotificationIcon(Main.spriteBatch, val5 + new Vector2(val3.X * num4, 0f) + new Vector2(8f, 0f), 0f);
			}
			if (flag)
			{
				ItemSlot.DrawMoney(Main.spriteBatch, "", val2.X - 45, val2.Y - 44, Utils.CoinsSplit(coinValue), horizontal: true);
				val2.X += 106;
			}
			if (!PlayerInput.IgnoreMouseInterface & flag2)
			{
				LocalPlayer.mouseInterface = true;
				LocalPlayer.releaseUseItem = false;
				num2 = num;
				if (Main.mouseLeft && Main.mouseLeftRelease)
				{
					Main.mouseLeftRelease = false;
					interaction.Interact();
				}
			}
		}
		if (_lastHovered != num2 && (!PlayerInput.UsingGamepad || num2 != -1))
		{
			SoundEngine.PlaySound(12);
		}
		_lastHovered = num2;
	}

	private void PrepareInteractions()
	{
		_interactions.Clear();
		foreach (NPCInteraction item in NPCInteractions.All)
		{
			if (item.Condition())
			{
				_interactions.Add(item);
			}
		}
		int count = _interactions.Count;
		_neededInteractionLines = (int)Math.Ceiling((float)count / 4f);
	}

	private void PrepareVirtualKeyboard()
	{
		int num = 120 + _textDisplayCache.AmountOfLines * 30 + 30;
		num -= 235;
		UIVirtualKeyboard.ShouldHideText = !PlayerInput.SettingsForUI.ShowGamepadHints;
		if (!PlayerInput.UsingGamepad)
		{
			num = 9999;
		}
		UIVirtualKeyboard.OffsetDown = num;
	}

	private void PrepareText()
	{
		string chatTextToShow = Main.npcChatText;
		OverrideChatTextWithShenanigans(ref chatTextToShow);
		_textDisplayCache.PrepareCache(chatTextToShow);
	}

	private void OverrideChatTextWithShenanigans(ref string chatTextToShow)
	{
		bool num = LocalPlayer.talkNPC != -1 && Main.CanDryadPlayStardewAnimation(LocalPlayer, Main.npc[LocalPlayer.talkNPC]);
		int num2 = 24;
		if (LocalPlayer.talkNPC != -1 && Main.npc[LocalPlayer.talkNPC].ai[0] == (float)num2 && NPC.RerollDryadText == 2)
		{
			NPC.RerollDryadText = 1;
		}
		if (num && NPC.RerollDryadText == 1 && Main.npc[LocalPlayer.talkNPC].ai[0] != (float)num2 && LocalPlayer.talkNPC != -1 && Main.npc[LocalPlayer.talkNPC].active && Main.npc[LocalPlayer.talkNPC].type == 20)
		{
			NPC.RerollDryadText = 0;
			chatTextToShow = (Main.npcChatText = Main.npc[LocalPlayer.talkNPC].GetChat());
			NPC.PreventJojaColaDialog = true;
		}
		if (num && !NPC.PreventJojaColaDialog)
		{
			chatTextToShow = Language.GetTextValue("StardewTalk.PlayerHasColaAndIsHoldingIt");
		}
	}

	private void DrawText(Color textColor, Rectangle textArea)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0101: Unknown result type (might be due to invalid IL or missing references)
		//IL_0112: Unknown result type (might be due to invalid IL or missing references)
		//IL_0141: Unknown result type (might be due to invalid IL or missing references)
		//IL_014f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0157: Unknown result type (might be due to invalid IL or missing references)
		//IL_0220: Unknown result type (might be due to invalid IL or missing references)
		//IL_0222: Unknown result type (might be due to invalid IL or missing references)
		//IL_0228: Unknown result type (might be due to invalid IL or missing references)
		//IL_022d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0184: Unknown result type (might be due to invalid IL or missing references)
		//IL_0186: Unknown result type (might be due to invalid IL or missing references)
		//IL_0190: Unknown result type (might be due to invalid IL or missing references)
		//IL_0195: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0165: Unknown result type (might be due to invalid IL or missing references)
		//IL_0170: Unknown result type (might be due to invalid IL or missing references)
		//IL_0175: Unknown result type (might be due to invalid IL or missing references)
		//IL_017a: Unknown result type (might be due to invalid IL or missing references)
		Vector2 val = textArea.TopLeft() + new Vector2(20f, 20f);
		DynamicSpriteFont value = FontAssets.MouseText.Value;
		string[] textLines = _textDisplayCache.TextLines;
		int amountOfLines = _textDisplayCache.AmountOfLines;
		for (int i = 0; i < amountOfLines; i++)
		{
			string text = textLines[i];
			if (text != null)
			{
				if (allowRichText)
				{
					ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, value, text, val + new Vector2(0f, (float)(i * 30)), textColor, 0f, Vector2.Zero, Vector2.One);
				}
				else
				{
					Utils.DrawBorderStringFourWay(Main.spriteBatch, value, text, val.X, val.Y + (float)(i * 30), textColor, Color.Black, Vector2.Zero);
				}
			}
		}
		if (!Main.editSign || textLines[amountOfLines - 1] == null)
		{
			return;
		}
		Vector2 val2 = val + new Vector2(0f, (float)((amountOfLines - 1) * 30));
		val2.X += value.MeasureString(textLines[amountOfLines - 1]).X;
		string compositionString = Platform.Get<IImeService>().CompositionString;
		if (compositionString != null && compositionString.Length > 0)
		{
			float x = value.MeasureString(compositionString).X;
			if (x + val2.X - val.X > 460f)
			{
				val2 = val + new Vector2(0f, (float)(amountOfLines * 30));
			}
			ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, value, compositionString, val2, Main.imeCompositionStringColor, 0f, Vector2.Zero, Vector2.One);
			Main.instance.SetIMEPanelAnchor(val2 + new Vector2(0f, 54f), 0f);
			val2.X += x;
		}
		if (++textBlinkerCount >= 20)
		{
			textBlinkerState = ((textBlinkerState == 0) ? 1 : 0);
			textBlinkerCount = 0;
		}
		if (textBlinkerState == 1)
		{
			ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, value, "|", val2, textColor, 0f, Vector2.Zero, Vector2.One);
		}
	}

	public void Close()
	{
		_lastHovered = -1;
		ClearNPCChatText();
	}

	private void ClearNPCChatText()
	{
		Main.npcChatText = "";
	}

	public bool CanHoldConversation()
	{
		if (LocalPlayer.talkNPC < 0)
		{
			return LocalPlayer.sign != -1;
		}
		return true;
	}
}
