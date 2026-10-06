using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using Terraria.Chat;
using Terraria.GameContent.UI.Chat;
using Terraria.Graphics;
using Terraria.Localization;
using Terraria.Testing.ChatCommands;

namespace Terraria.UI.Chat;

public static class ChatManager
{
	public static class Regexes
	{
		public static readonly Regex Format = new Regex("(?<!\\\\)\\[(?<tag>[a-zA-Z]{1,10})(\\/(?<options>[^:]+))?:(?<text>.+?)(?<!\\\\)\\]", RegexOptions.Compiled | RegexOptions.Singleline);
	}

	public static readonly DebugCommandProcessor DebugCommands = new DebugCommandProcessor();

	public static readonly ChatCommandProcessor Commands = new ChatCommandProcessor();

	private static ConcurrentDictionary<string, ITagHandler> _handlers = new ConcurrentDictionary<string, ITagHandler>();

	public static readonly Vector2[] ShadowDirections = new Vector2[4]
	{
		-Vector2.UnitX,
		Vector2.UnitX,
		-Vector2.UnitY,
		Vector2.UnitY
	};

	public static Color WaveColor(Color color)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		float num = (float)(int)Main.mouseTextColor / 255f;
		color = Color.Lerp(color, Color.Black, 1f - num);
		color.A = Main.mouseTextColor;
		return color;
	}

	public static void ConvertNormalSnippets(List<TextSnippet> snippets)
	{
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		for (int i = 0; i < snippets.Count; i++)
		{
			TextSnippet textSnippet = snippets[i];
			if (textSnippet.GetType() == typeof(TextSnippet))
			{
				snippets[i] = new PlainTagHandler.PlainSnippet(textSnippet.Text, textSnippet.Color);
			}
		}
	}

	public static void Register<T>(params string[] names) where T : ITagHandler, new()
	{
		T val = new T();
		for (int i = 0; i < names.Length; i++)
		{
			_handlers[names[i].ToLower()] = val;
		}
	}

	private static ITagHandler GetHandler(string tagName)
	{
		string key = tagName.ToLower();
		if (_handlers.ContainsKey(key))
		{
			return _handlers[key];
		}
		return null;
	}

	public static bool MayNeedParsing(string text)
	{
		if (text.IndexOf('\r') < 0)
		{
			return Regexes.Format.IsMatch(text);
		}
		return true;
	}

	public static List<TextSnippet> ParseMessage(string text, Color baseColor)
	{
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0140: Unknown result type (might be due to invalid IL or missing references)
		text = text.Replace("\r", "");
		MatchCollection matchCollection = Regexes.Format.Matches(text);
		List<TextSnippet> list = new List<TextSnippet>();
		int num = 0;
		foreach (Match item in matchCollection)
		{
			if (item.Index > num)
			{
				list.Add(new TextSnippet(text.Substring(num, item.Index - num), baseColor));
			}
			num = item.Index + item.Length;
			string value = item.Groups["tag"].Value;
			string text2 = item.Groups["text"].Value.Replace("\\]", "]");
			string value2 = item.Groups["options"].Value;
			ITagHandler handler = GetHandler(value);
			if (handler != null)
			{
				list.Add(handler.Parse(text2, baseColor, value2));
				list[list.Count - 1].TextOriginal = item.ToString();
			}
			else
			{
				list.Add(new TextSnippet(text2, baseColor));
			}
		}
		if (text.Length > num)
		{
			list.Add(new TextSnippet(text.Substring(num, text.Length - num), baseColor));
		}
		return list;
	}

	public static bool AddChatText(DynamicSpriteFont font, string text, Vector2 baseScale)
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		int num = 470;
		num = Main.screenWidth - 330;
		if (GetStringSize(font, Main.chatText + text, baseScale).X > (float)num)
		{
			return false;
		}
		Main.chatText += text;
		return true;
	}

	public static IEnumerable<PositionedSnippet> LayoutSnippets(DynamicSpriteFont font, IEnumerable<TextSnippet> snippets, Vector2 scale, float maxWidth = -1f)
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		int line = 0;
		Vector2 pos = Vector2.Zero;
		float uniqueDrawScale = Math.Min(scale.X, scale.Y);
		int i = 0;
		foreach (TextSnippet snippet in snippets)
		{
			if (snippet.UniqueDraw(justCheckingSize: true, out var size, null, default, default, uniqueDrawScale))
			{
				if (maxWidth >= 0f && pos.X + size.X > maxWidth)
				{
					pos.X = 0f;
					pos.Y += (float)font.LineSpacing * scale.Y;
					line++;
				}
				yield return new PositionedSnippet(snippet, i, line, pos, size);
				pos.X += size.X;
			}
			else
			{
				string text = font.CreateWrappedText(snippet.Text, scale.X, maxWidth, pos.X, Language.ActiveCulture.CultureInfo);
				int num = 0;
				while (true)
				{
					int sep = text.IndexOf('\n', num);
					int num2 = ((sep < 0) ? text.Length : sep) - num;
					if (num2 > 0)
					{
						string text2 = text.Substring(num, num2);
						size = font.MeasureString(text2) * scale;
						yield return new PositionedSnippet(snippet.CopyMorph(text2), i, line, pos, size);
						pos.X += size.X;
					}
					if (sep < 0)
					{
						break;
					}
					pos.X = 0f;
					pos.Y += (float)font.LineSpacing * scale.Y;
					line++;
					num = sep + 1;
				}
			}
			i++;
			size = default;
		}
	}

	public static Vector2 GetStringSize(DynamicSpriteFont font, string text, Vector2 baseScale, float maxWidth = -1f)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		return GetStringSize(font, ParseMessage(text, Color.White), baseScale, maxWidth);
	}

	public static Vector2 GetStringSize(DynamicSpriteFont font, IEnumerable<TextSnippet> snippets, Vector2 scale, float maxWidth = -1f)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		return GetStringSize(LayoutSnippets(font, snippets, scale, maxWidth));
	}

	public static Vector2 GetStringSize(IEnumerable<PositionedSnippet> snippets)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		Vector2 zero = Vector2.Zero;
		foreach (PositionedSnippet snippet in snippets)
		{
			zero.X = Math.Max(zero.X, snippet.Position.X + snippet.Size.X);
			zero.Y = Math.Max(zero.Y, snippet.Position.Y + snippet.Size.Y);
		}
		return zero;
	}

	public static void DrawColorCodedStringShadow(SpriteBatch spriteBatch, DynamicSpriteFont font, List<PositionedSnippet> snippets, Vector2 position, Color shadowColor, float rotation, Vector2 origin, Vector2 scale, float spread = 2f)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		for (int i = 0; i < ShadowDirections.Length; i++)
		{
			DrawColorCodedString(spriteBatch, font, snippets, position + ShadowDirections[i] * spread, rotation, origin, scale, out var _, shadowColor);
		}
	}

	public static void DrawColorCodedString(SpriteBatch spriteBatch, DynamicSpriteFont font, IEnumerable<TextSnippet> snippets, Vector2 position, Color baseColor, float rotation, Vector2 origin, Vector2 scale, out int hoveredSnippet, float maxWidth = -1f, bool ignoreColors = false)
	{
		//IL_0004: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		DrawColorCodedString(spriteBatch, font, LayoutSnippets(font, snippets, scale, maxWidth), position, rotation, origin, scale, out hoveredSnippet, ignoreColors ? new Color?(baseColor) : ((Color?)null));
	}

	public static void DrawColorCodedString(SpriteBatch spriteBatch, DynamicSpriteFont font, IEnumerable<TextSnippet> snippets, Vector2 position, float rotation, Vector2 origin, Vector2 scale, out int hoveredSnippet, float maxWidth = -1f)
	{
		//IL_0004: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		DrawColorCodedString(spriteBatch, font, LayoutSnippets(font, snippets, scale, maxWidth), position, rotation, origin, scale, out hoveredSnippet);
	}

	public static void DrawColorCodedString(SpriteBatch spriteBatch, DynamicSpriteFont font, IEnumerable<PositionedSnippet> snippets, Vector2 position, float rotation, Vector2 origin, Vector2 scale, out int hoveredSnippet, Color? colorOverride = null)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		hoveredSnippet = -1;
		Vector2 vec = new Vector2((float)Main.mouseX, (float)Main.mouseY);
		float scale2 = Math.Min(scale.X, scale.Y);
		foreach (PositionedSnippet snippet2 in snippets)
		{
			Vector2 val = position + snippet2.Position;
			TextSnippet snippet = snippet2.Snippet;
			Color val2 = (colorOverride.HasValue ? colorOverride.Value : snippet.GetVisibleColor());
			if (!snippet.UniqueDraw(justCheckingSize: false, out var _, spriteBatch, val, val2, scale2))
			{
				DynamicSpriteFontExtensionMethods.DrawString(spriteBatch, font, snippet.Text, val, val2, rotation, origin, scale, (SpriteEffects)0, 0f);
			}
			if (snippet2.Snippet.CheckForHover && vec.Between(val, val + snippet2.Size))
			{
				hoveredSnippet = snippet2.OrigIndex;
			}
		}
	}

	public static void DrawColorCodedStringWithShadow(SpriteBatch spriteBatch, DynamicSpriteFont font, TextSnippet[] snippets, Vector2 position, float rotation, Vector2 origin, Vector2 baseScale, out int hoveredSnippet, float maxWidth = -1f, float spread = 2f)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		List<PositionedSnippet> snippets2 = LayoutSnippets(font, snippets, baseScale, maxWidth).ToList();
		DrawColorCodedStringShadow(spriteBatch, font, snippets2, position, Color.Black, rotation, origin, baseScale, spread);
		DrawColorCodedString(spriteBatch, font, snippets2, position, rotation, origin, baseScale, out hoveredSnippet);
	}

	public static void DrawColorCodedStringWithShadow(SpriteBatch spriteBatch, DynamicSpriteFont font, TextSnippet[] snippets, Vector2 position, Color color, float rotation, Vector2 origin, Vector2 baseScale, out int hoveredSnippet, float maxWidth = -1f, float spread = 2f)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		List<PositionedSnippet> snippets2 = LayoutSnippets(font, snippets, baseScale, maxWidth).ToList();
		DrawColorCodedStringShadow(spriteBatch, font, snippets2, position, color.MultiplyRGBA(Color.Black), rotation, origin, baseScale, spread);
		DrawColorCodedString(spriteBatch, font, snippets2, position, rotation, origin, baseScale, out hoveredSnippet, color);
	}

	public static void DrawColorCodedStringShadow(SpriteBatch spriteBatch, DynamicSpriteFont font, string text, Vector2 position, Color baseColor, float rotation, Vector2 origin, Vector2 baseScale, float maxWidth = -1f, float spread = 2f)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		for (int i = 0; i < ShadowDirections.Length; i++)
		{
			DrawColorCodedString(spriteBatch, font, text, position + ShadowDirections[i] * spread, baseColor, rotation, origin, baseScale, maxWidth, ignoreColors: true);
		}
	}

	public static Vector2 DrawColorCodedString(SpriteBatch spriteBatch, DynamicSpriteFont font, string text, Vector2 position, Color baseColor, float rotation, Vector2 origin, Vector2 baseScale, float maxWidth = -1f, bool ignoreColors = false)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0277: Unknown result type (might be due to invalid IL or missing references)
		//IL_021f: Unknown result type (might be due to invalid IL or missing references)
		//IL_023d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0249: Unknown result type (might be due to invalid IL or missing references)
		//IL_024f: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0102: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_011e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0128: Unknown result type (might be due to invalid IL or missing references)
		//IL_0135: Unknown result type (might be due to invalid IL or missing references)
		//IL_013b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_014b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0169: Unknown result type (might be due to invalid IL or missing references)
		//IL_0175: Unknown result type (might be due to invalid IL or missing references)
		//IL_017b: Unknown result type (might be due to invalid IL or missing references)
		Vector2 val = position;
		Vector2 val2 = val;
		string[] array = text.Split('\n');
		float x = font.MeasureString(" ").X;
		Color val3 = baseColor;
		float num = 1f;
		float num2 = 0f;
		string[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			string[] array3 = array2[i].Split(':');
			foreach (string text2 in array3)
			{
				if (text2.StartsWith("sss"))
				{
					if (text2.StartsWith("sss1"))
					{
						if (!ignoreColors)
						{
							val3 = Color.Red;
						}
					}
					else if (text2.StartsWith("sss2"))
					{
						if (!ignoreColors)
						{
							val3 = Color.Blue;
						}
					}
					else if (text2.StartsWith("sssr") && !ignoreColors)
					{
						val3 = Color.White;
					}
					continue;
				}
				string[] array4 = text2.Split(' ');
				for (int k = 0; k < array4.Length; k++)
				{
					if (k != 0)
					{
						val.X += x * baseScale.X * num;
					}
					if (maxWidth > 0f)
					{
						float num3 = font.MeasureString(array4[k]).X * baseScale.X * num;
						if (val.X - position.X + num3 > maxWidth)
						{
							val.X = position.X;
							val.Y += (float)font.LineSpacing * num2 * baseScale.Y;
							val2.Y = Math.Max(val2.Y, val.Y);
							num2 = 0f;
						}
					}
					if (num2 < num)
					{
						num2 = num;
					}
					DynamicSpriteFontExtensionMethods.DrawString(spriteBatch, font, array4[k], val, val3, rotation, origin, baseScale * num, (SpriteEffects)0, 0f);
					val.X += font.MeasureString(array4[k]).X * baseScale.X * num;
					val2.X = Math.Max(val2.X, val.X);
				}
			}
			val.X = position.X;
			val.Y += (float)font.LineSpacing * num2 * baseScale.Y;
			val2.Y = Math.Max(val2.Y, val.Y);
			num2 = 0f;
		}
		return val2;
	}

	public static void DrawColorCodedStringWithShadow(SpriteBatch spriteBatch, DynamicSpriteFont font, string text, Vector2 position, Color baseColor, float rotation, Vector2 origin, Vector2 scale, float maxWidth = -1f, float spread = 2f)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		Color val = baseColor.MultiplyRGBA(Color.Black);
		if (maxWidth < 0f && !MayNeedParsing(text))
		{
			Vector2[] shadowDirections = ShadowDirections;
			foreach (Vector2 val2 in shadowDirections)
			{
				DynamicSpriteFontExtensionMethods.DrawString(spriteBatch, font, text, position + val2 * spread, val, rotation, origin, scale, (SpriteEffects)0, 0f);
			}
			DynamicSpriteFontExtensionMethods.DrawString(spriteBatch, font, text, position, baseColor, rotation, origin, scale, (SpriteEffects)0, 0f);
		}
		else
		{
			List<TextSnippet> snippets = ParseMessage(text, baseColor);
			ConvertNormalSnippets(snippets);
			List<PositionedSnippet> snippets2 = LayoutSnippets(font, snippets, scale, maxWidth).ToList();
			DrawColorCodedStringShadow(spriteBatch, font, snippets2, position, val, rotation, origin, scale, spread);
			DrawColorCodedString(spriteBatch, font, snippets2, position, rotation, origin, scale, out var _);
		}
	}

	public static void DrawStringWithShadowFast(TileBatch tileBatch, DynamicSpriteFont font, string text, Vector2 position, Color baseColor, Vector2 origin, float scale, uint layer = 0u, float spread = 2f)
	{
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Expected Obj, but got Unknown
		Color shadowColor = baseColor.MultiplyRGBA(Color.Black);
		font.DrawCustomFast((DynamicSpriteFont.DrawCharacter)((Texture2D tex, Vector2 pos, Rectangle rect, Vector2 _) =>
		{
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0080: Unknown result type (might be due to invalid IL or missing references)
			//IL_0082: Unknown result type (might be due to invalid IL or missing references)
			//IL_008c: Unknown result type (might be due to invalid IL or missing references)
			tileBatch.SetLayer(layer, 0);
			Vector2[] shadowDirections = ShadowDirections;
			foreach (Vector2 val in shadowDirections)
			{
				tileBatch.Draw(tex, pos + val * spread, rect, shadowColor, Vector2.Zero, scale, (SpriteEffects)0);
			}
			tileBatch.SetLayer(layer + 1, 0);
			tileBatch.Draw(tex, pos, rect, baseColor, Vector2.Zero, scale, (SpriteEffects)0);
		}), text, position - origin * scale, new Vector2(scale));
	}

	static ChatManager()
	{
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
	}
}
