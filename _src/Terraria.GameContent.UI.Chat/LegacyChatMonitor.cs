using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using Terraria.GameInput;
using Terraria.UI.Chat;

namespace Terraria.GameContent.UI.Chat;

public class LegacyChatMonitor : IChatMonitor
{
	private int numChatLines;

	private ChatLine[] chatLine;

	private int chatLength;

	private int showCount;

	private int startChatLine;

	public LegacyChatMonitor()
	{
		showCount = 10;
		numChatLines = 500;
		chatLength = 600;
		chatLine = new ChatLine[numChatLines];
		for (int i = 0; i < numChatLines; i++)
		{
			chatLine[i] = new ChatLine();
		}
	}

	public void Clear()
	{
		for (int i = 0; i < numChatLines; i++)
		{
			chatLine[i] = new ChatLine();
		}
	}

	public void ResetOffset()
	{
		startChatLine = 0;
	}

	public void Update()
	{
		for (int i = 0; i < numChatLines; i++)
		{
			chatLine[i].UpdateTimeLeft();
		}
	}

	public void Offset(int linesOffset)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		showCount = (int)((float)(Main.screenHeight / 3) / FontAssets.MouseText.Value.MeasureString("1").Y) - 1;
		switch (linesOffset)
		{
		case 1:
			startChatLine++;
			if (startChatLine + showCount >= numChatLines - 1)
			{
				startChatLine = numChatLines - showCount - 1;
			}
			if (chatLine[startChatLine + showCount].originalText == "")
			{
				startChatLine--;
			}
			break;
		case -1:
			startChatLine--;
			if (startChatLine < 0)
			{
				startChatLine = 0;
			}
			break;
		}
	}

	public void NewText(string newText, byte R = byte.MaxValue, byte G = byte.MaxValue, byte B = byte.MaxValue)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		NewTextMultiline(newText, force: false, new Color((int)R, (int)G, (int)B));
	}

	public void NewTextInternal(string newText, byte R = byte.MaxValue, byte G = byte.MaxValue, byte B = byte.MaxValue, bool force = false)
	{
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		int num = 80;
		if (!force && newText.Length > num)
		{
			string oldText = newText;
			oldText = TrimIntoMultipleLines(R, G, B, num, oldText);
			if (oldText.Length > 0)
			{
				NewTextInternal(oldText, R, G, B, force: true);
			}
			return;
		}
		for (int num2 = numChatLines - 1; num2 > 0; num2--)
		{
			chatLine[num2].Copy(chatLine[num2 - 1]);
		}
		chatLine[0].color = new Color((int)R, (int)G, (int)B);
		chatLine[0].originalText = newText;
		chatLine[0].parsedText = ChatManager.ParseMessage(chatLine[0].originalText, chatLine[0].color).ToArray();
		chatLine[0].showTime = chatLength;
		SoundEngine.PlaySound(12);
	}

	private string TrimIntoMultipleLines(byte R, byte G, byte B, int maxTextSize, string oldText)
	{
		while (oldText.Length > maxTextSize)
		{
			int num = maxTextSize;
			int num2 = num;
			while (oldText.Substring(num2, 1) != " ")
			{
				num2--;
				if (num2 < 1)
				{
					break;
				}
			}
			if (num2 == 0)
			{
				while (oldText.Substring(num, 1) != " ")
				{
					num++;
					if (num >= oldText.Length - 1)
					{
						break;
					}
				}
			}
			else
			{
				num = num2;
			}
			if (num >= oldText.Length - 1)
			{
				num = oldText.Length;
			}
			string text = Utils.TrimUserString(oldText, num);
			NewTextInternal(text, R, G, B, force: true);
			oldText = oldText.Substring(text.Length);
			if (oldText.Length > 0)
			{
				while (oldText.Substring(0, 1) == " ")
				{
					oldText = oldText.Substring(1);
				}
			}
		}
		return oldText;
	}

	public void NewTextMultiline(string text, bool force = false, Color c = default(Color), int WidthLimit = -1)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		if (c == default(Color))
		{
			c = Color.White;
		}
		List<List<TextSnippet>> list = ((WidthLimit == -1) ? Utils.WordwrapStringSmart(text, c, FontAssets.MouseText.Value, Main.ChatLineWidthLimit, 10) : Utils.WordwrapStringSmart(text, c, FontAssets.MouseText.Value, WidthLimit, 10));
		for (int i = 0; i < list.Count; i++)
		{
			NewText(list[i]);
		}
	}

	public void NewText(List<TextSnippet> snippets)
	{
		for (int num = numChatLines - 1; num > 0; num--)
		{
			chatLine[num].Copy(chatLine[num - 1]);
		}
		chatLine[0].originalText = "this is a hack because draw checks length is higher than 0";
		chatLine[0].parsedText = snippets.ToArray();
		chatLine[0].showTime = chatLength;
		SoundEngine.PlaySound(12);
	}

	public void DrawChat(bool drawingPlayerChat)
	{
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		int num = startChatLine;
		int num2 = startChatLine + showCount;
		if (num2 >= numChatLines)
		{
			num2 = --numChatLines;
			num = num2 - showCount;
		}
		int num3 = 0;
		int num4 = -1;
		int num5 = -1;
		for (int i = num; i < num2; i++)
		{
			if (drawingPlayerChat || (chatLine[i].showTime > 0 && chatLine[i].parsedText.Length != 0))
			{
				int hoveredSnippet = -1;
				ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, FontAssets.MouseText.Value, chatLine[i].parsedText, new Vector2(88f, (float)(Main.screenHeight - 30 - 28 - num3 * 21)), 0f, Vector2.Zero, Vector2.One, out hoveredSnippet);
				if (hoveredSnippet >= 0)
				{
					num4 = i;
					num5 = hoveredSnippet;
				}
			}
			num3++;
		}
		if (num4 > -1 && !PlayerInput.IgnoreMouseInterface)
		{
			chatLine[num4].parsedText[num5].OnHover();
			Main.LocalPlayer.mouseInterface = true;
			if (Main.mouseLeft && Main.mouseLeftRelease)
			{
				chatLine[num4].parsedText[num5].OnClick();
			}
		}
	}
}
