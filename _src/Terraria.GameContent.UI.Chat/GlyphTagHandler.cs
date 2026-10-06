using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Steamworks;
using Terraria.Social;
using Terraria.UI.Chat;

namespace Terraria.GameContent.UI.Chat;

public class GlyphTagHandler : ITagHandler
{
	public class GlyphXboxTagHandler : ITagHandler
	{
		TextSnippet ITagHandler.Parse(string text, Color baseColor, string options)
		{
			if (!int.TryParse(text, out var result) || result >= 26)
			{
				return new TextSnippet(text);
			}
			return new GlyphSnippet(result)
			{
				ForcedStyle = 0,
				DeleteWhole = true,
				Text = "[gx:" + result + "]"
			};
		}
	}

	public class GlyphPSTagHandler : ITagHandler
	{
		TextSnippet ITagHandler.Parse(string text, Color baseColor, string options)
		{
			if (!int.TryParse(text, out var result) || result >= 26)
			{
				return new TextSnippet(text);
			}
			return new GlyphSnippet(result)
			{
				ForcedStyle = 1,
				DeleteWhole = true,
				Text = "[gp:" + result + "]"
			};
		}
	}

	public class GlyphSwitchTagHandler : ITagHandler
	{
		TextSnippet ITagHandler.Parse(string text, Color baseColor, string options)
		{
			if (!int.TryParse(text, out var result) || result >= 26)
			{
				return new TextSnippet(text);
			}
			return new GlyphSnippet(result)
			{
				ForcedStyle = 2,
				DeleteWhole = true,
				Text = "[gn:" + result + "]"
			};
		}
	}

	public class GlyphSnippet : TextSnippet
	{
		public int ForcedStyle = -1;

		private int _glyphIndex;

		public GlyphSnippet(int index)
		{
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			_glyphIndex = index;
			Color = Color.White;
		}

		public GlyphSnippet(string keyName)
		{
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			GlyphIndexes.TryGetValue(keyName, out _glyphIndex);
			Color = Color.White;
		}

		private static int GetAutoRow()
		{
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Expected I4, but got Unknown
			if (SocialAPI.Mode != SocialMode.Steam)
			{
				return 0;
			}
			SteamInput.RunFrame(true);
			int result = 0;
			InputHandle_t controllerForGamepadIndex = SteamInput.GetControllerForGamepadIndex(0);
			if (controllerForGamepadIndex.m_InputHandle != 0L)
			{
				ESteamInputType inputTypeForHandle = SteamInput.GetInputTypeForHandle(controllerForGamepadIndex);
				switch ((int)inputTypeForHandle - 5)
				{
				case 0:
				case 7:
				case 8:
					result = 1;
					break;
				case 3:
				case 4:
				case 5:
					result = 2;
					break;
				}
			}
			return result;
		}

		public override bool UniqueDraw(bool justCheckingString, out Vector2 size, SpriteBatch spriteBatch, Vector2 position = default(Vector2), Color color = default(Color), float scale = 1f)
		{
			//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0077: Unknown result type (might be due to invalid IL or missing references)
			//IL_0079: Unknown result type (might be due to invalid IL or missing references)
			//IL_007e: Unknown result type (might be due to invalid IL or missing references)
			//IL_008b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0095: Unknown result type (might be due to invalid IL or missing references)
			//IL_009c: Unknown result type (might be due to invalid IL or missing references)
			scale *= GlyphsScale;
			if (!justCheckingString && color != Color.Black)
			{
				int num = ForcedStyle;
				if (num == -1)
				{
					num = GlyphStyle;
				}
				if (num == -1)
				{
					num = GetAutoRow();
				}
				int frameX = _glyphIndex;
				int glyphIndex = _glyphIndex;
				if (glyphIndex == 25)
				{
					frameX = ((Main.GlobalTimeWrappedHourly % 0.6f < 0.3f) ? 17 : 18);
				}
				Texture2D value = TextureAssets.TextGlyph[0].Value;
				spriteBatch.Draw(value, position + GlyphsOffset, (Rectangle?)value.Frame(25, 3, frameX, num), color, 0f, Vector2.Zero, scale, (SpriteEffects)0, 0f);
			}
			size = new Vector2(26f) * scale;
			return true;
		}
	}

	private const int GlyphsPerLine = 25;

	private const int MaxGlyphs = 26;

	public static float GlyphsScale = 1f;

	public static Vector2 GlyphsOffset = Vector2.Zero;

	public const int DefaultGlyphStyle = -1;

	public static int GlyphStyle = -1;

	private static Dictionary<string, int> GlyphIndexes = new Dictionary<string, int>
	{
		{
			((object)(Buttons)4096/*cast due to constrained. prefix*/).ToString(),
			0
		},
		{
			((object)(Buttons)8192/*cast due to constrained. prefix*/).ToString(),
			1
		},
		{
			((object)(Buttons)32/*cast due to constrained. prefix*/).ToString(),
			4
		},
		{
			((object)(Buttons)2/*cast due to constrained. prefix*/).ToString(),
			15
		},
		{
			((object)(Buttons)4/*cast due to constrained. prefix*/).ToString(),
			14
		},
		{
			((object)(Buttons)8/*cast due to constrained. prefix*/).ToString(),
			13
		},
		{
			((object)(Buttons)1/*cast due to constrained. prefix*/).ToString(),
			16
		},
		{
			((object)(Buttons)256/*cast due to constrained. prefix*/).ToString(),
			6
		},
		{
			((object)(Buttons)64/*cast due to constrained. prefix*/).ToString(),
			10
		},
		{
			((object)(Buttons)536870912/*cast due to constrained. prefix*/).ToString(),
			20
		},
		{
			((object)(Buttons)2097152/*cast due to constrained. prefix*/).ToString(),
			17
		},
		{
			((object)(Buttons)1073741824/*cast due to constrained. prefix*/).ToString(),
			18
		},
		{
			((object)(Buttons)268435456/*cast due to constrained. prefix*/).ToString(),
			19
		},
		{
			((object)(Buttons)8388608/*cast due to constrained. prefix*/).ToString(),
			8
		},
		{
			((object)(Buttons)512/*cast due to constrained. prefix*/).ToString(),
			7
		},
		{
			((object)(Buttons)128/*cast due to constrained. prefix*/).ToString(),
			11
		},
		{
			((object)(Buttons)33554432/*cast due to constrained. prefix*/).ToString(),
			24
		},
		{
			((object)(Buttons)134217728/*cast due to constrained. prefix*/).ToString(),
			21
		},
		{
			((object)(Buttons)67108864/*cast due to constrained. prefix*/).ToString(),
			22
		},
		{
			((object)(Buttons)16777216/*cast due to constrained. prefix*/).ToString(),
			23
		},
		{
			((object)(Buttons)4194304/*cast due to constrained. prefix*/).ToString(),
			9
		},
		{
			((object)(Buttons)16/*cast due to constrained. prefix*/).ToString(),
			5
		},
		{
			((object)(Buttons)16384/*cast due to constrained. prefix*/).ToString(),
			2
		},
		{
			((object)(Buttons)32768/*cast due to constrained. prefix*/).ToString(),
			3
		},
		{ "RightStickAxis", 12 },
		{ "LR", 25 }
	};

	public static TextSnippet GetGlyph(string keyName)
	{
		return new GlyphSnippet(keyName);
	}

	TextSnippet ITagHandler.Parse(string text, Color baseColor, string options)
	{
		if (!int.TryParse(text, out var result) || result >= 26)
		{
			return new TextSnippet(text);
		}
		return new GlyphSnippet(result)
		{
			DeleteWhole = true,
			Text = "[g:" + result + "]"
		};
	}

	public static string GenerateTag(int index)
	{
		string text = "[g";
		return text + ":" + index + "]";
	}

	public static string GenerateTag(string keyname)
	{
		if (GlyphIndexes.TryGetValue(keyname, out var value))
		{
			return GenerateTag(value);
		}
		return keyname;
	}

	static GlyphTagHandler()
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0119: Unknown result type (might be due to invalid IL or missing references)
		//IL_0134: Unknown result type (might be due to invalid IL or missing references)
		//IL_014f: Unknown result type (might be due to invalid IL or missing references)
		//IL_016a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0184: Unknown result type (might be due to invalid IL or missing references)
		//IL_019e: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_020a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0225: Unknown result type (might be due to invalid IL or missing references)
		//IL_023d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0257: Unknown result type (might be due to invalid IL or missing references)
		//IL_0271: Unknown result type (might be due to invalid IL or missing references)
	}
}
