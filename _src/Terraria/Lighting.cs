using System;
using Microsoft.Xna.Framework;
using Terraria.Graphics;
using Terraria.Graphics.Light;
using Terraria.ID;
using Terraria.Utilities;

namespace Terraria;

public class Lighting
{
	private const float DEFAULT_GLOBAL_BRIGHTNESS = 1.2f;

	private const float BLIND_GLOBAL_BRIGHTNESS = 1f;

	[Old]
	public static int OffScreenTiles = 45;

	private static LightMode _mode = LightMode.Color;

	private static readonly LightingEngine NewEngine = new LightingEngine();

	private static readonly LegacyLighting LegacyEngine = new LegacyLighting(Main.Camera);

	private static ILightingEngine _activeEngine;

	public static float GlobalBrightness { get; set; }

	public static LightMode Mode
	{
		get
		{
			return _mode;
		}
		set
		{
			_mode = value;
			switch (_mode)
			{
			case LightMode.Color:
				_activeEngine = NewEngine;
				LegacyEngine.Mode = 0;
				OffScreenTiles = 35;
				break;
			case LightMode.White:
				_activeEngine = LegacyEngine;
				LegacyEngine.Mode = 1;
				break;
			case LightMode.Retro:
				_activeEngine = LegacyEngine;
				LegacyEngine.Mode = 2;
				break;
			case LightMode.Trippy:
				_activeEngine = LegacyEngine;
				LegacyEngine.Mode = 3;
				break;
			}
			Main.renderCount = 0;
			Main.renderNow = false;
		}
	}

	public static bool NotRetro
	{
		get
		{
			if (Mode != LightMode.Retro)
			{
				return Mode != LightMode.Trippy;
			}
			return false;
		}
	}

	public static bool UsingNewLighting => Mode == LightMode.Color;

	public static bool UpdateEveryFrame
	{
		get
		{
			if (!Main.RenderTargetsRequired)
			{
				return !NotRetro;
			}
			return false;
		}
	}

	public static void Initialize()
	{
		GlobalBrightness = 1.2f;
		NewEngine.Rebuild();
		LegacyEngine.Rebuild();
		if (_activeEngine == null)
		{
			Mode = LightMode.Color;
		}
	}

	public static void LightTiles(Rectangle area)
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		TimeLogger.StartTimestamp fromTimestamp = TimeLogger.Start();
		Main.render = true;
		UpdateGlobalBrightness();
		_activeEngine.ProcessArea(area);
		TimeLogger.Lighting.AddTime(fromTimestamp);
	}

	private static void UpdateGlobalBrightness()
	{
		GlobalBrightness = 1.2f;
		if (Main.player[Main.myPlayer].blind)
		{
			GlobalBrightness = 1f;
		}
	}

	public static float Brightness(int x, int y)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		Vector3 color = _activeEngine.GetColor(x, y);
		return GlobalBrightness * (color.X + color.Y + color.Z) / 3f;
	}

	public static Vector3 GetSubLight(Vector2 position)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		Vector2 val = position / 16f - new Vector2(0.5f, 0.5f);
		Vector2 val2 = new Vector2(val.X % 1f, val.Y % 1f);
		int num = (int)val.X;
		int num2 = (int)val.Y;
		Vector3 color = _activeEngine.GetColor(num, num2);
		Vector3 color2 = _activeEngine.GetColor(num + 1, num2);
		Vector3 color3 = _activeEngine.GetColor(num, num2 + 1);
		Vector3 color4 = _activeEngine.GetColor(num + 1, num2 + 1);
		Vector3 val3 = Vector3.Lerp(color, color2, val2.X);
		Vector3 val4 = Vector3.Lerp(color3, color4, val2.X);
		return Vector3.Lerp(val3, val4, val2.Y);
	}

	public static void AddLight(Vector2 position, Vector3 rgb)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		AddLight((int)(position.X / 16f), (int)(position.Y / 16f), rgb.X, rgb.Y, rgb.Z);
	}

	public static void AddLight(Vector2 position, float r, float g, float b)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		AddLight((int)(position.X / 16f), (int)(position.Y / 16f), r, g, b);
	}

	public static void AddLight(int i, int j, int torchID, float lightAmount)
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		TorchID.TorchColor(torchID, out var R, out var G, out var B);
		_activeEngine.AddLight(i, j, new Vector3(R * lightAmount, G * lightAmount, B * lightAmount));
	}

	public static void AddLight(Vector2 position, int torchID)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		TorchID.TorchColor(torchID, out var R, out var G, out var B);
		AddLight((int)position.X / 16, (int)position.Y / 16, R, G, B);
	}

	public static void AddLight(int i, int j, float r, float g, float b)
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		if (!Main.gamePaused && Main.netMode != 2)
		{
			_activeEngine.AddLight(i, j, new Vector3(r, g, b));
		}
	}

	public static void NextLightMode()
	{
		Mode++;
		if (!Enum.IsDefined(typeof(LightMode), Mode))
		{
			Mode = LightMode.White;
		}
		Clear();
	}

	public static void Clear()
	{
		_activeEngine.Clear();
	}

	public static Color GetColor(Point tileCoords)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		if (Main.gameMenu)
		{
			return Color.White;
		}
		return new Color(_activeEngine.GetColor(tileCoords.X, tileCoords.Y) * GlobalBrightness);
	}

	public static Color GetColor(Point tileCoords, Color originalColor)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		if (Main.gameMenu)
		{
			return originalColor;
		}
		return new Color(_activeEngine.GetColor(tileCoords.X, tileCoords.Y) * originalColor.ToVector3());
	}

	public static Color GetColor(int x, int y, Color oldColor)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		if (Main.gameMenu)
		{
			return oldColor;
		}
		return new Color(_activeEngine.GetColor(x, y) * oldColor.ToVector3());
	}

	public static Color GetColorClamped(int x, int y, Color oldColor)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		if (Main.gameMenu)
		{
			return oldColor;
		}
		Vector3 color = _activeEngine.GetColor(x, y);
		color = Vector3.Min(Vector3.One, color);
		return new Color(color * oldColor.ToVector3());
	}

	public static Color GetColor(int x, int y)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		if (Main.gameMenu)
		{
			return Color.White;
		}
		Color result = default;
		Vector3 color = _activeEngine.GetColor(x, y);
		float num = GlobalBrightness * 255f;
		int num2 = (int)(color.X * num);
		int num3 = (int)(color.Y * num);
		int num4 = (int)(color.Z * num);
		if (num2 > 255)
		{
			num2 = 255;
		}
		if (num3 > 255)
		{
			num3 = 255;
		}
		if (num4 > 255)
		{
			num4 = 255;
		}
		num4 <<= 16;
		num3 <<= 8;
		result.PackedValue = (uint)(num2 | num3 | num4 | -16777216);
		return result;
	}

	public static void GetColor9Slice(int centerX, int centerY, ref Color[] slices)
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		int num = 0;
		for (int i = centerX - 1; i <= centerX + 1; i++)
		{
			for (int j = centerY - 1; j <= centerY + 1; j++)
			{
				Vector3 color = _activeEngine.GetColor(i, j);
				int num2 = (int)(255f * color.X * GlobalBrightness);
				int num3 = (int)(255f * color.Y * GlobalBrightness);
				int num4 = (int)(255f * color.Z * GlobalBrightness);
				if (num2 > 255)
				{
					num2 = 255;
				}
				if (num3 > 255)
				{
					num3 = 255;
				}
				if (num4 > 255)
				{
					num4 = 255;
				}
				num4 <<= 16;
				num3 <<= 8;
				slices[num].PackedValue = (uint)(num2 | num3 | num4 | -16777216);
				num += 3;
			}
			num -= 8;
		}
	}

	public static void GetColor9Slice(int x, int y, ref Vector3[] slices)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0112: Unknown result type (might be due to invalid IL or missing references)
		//IL_011c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0121: Unknown result type (might be due to invalid IL or missing references)
		slices[0] = _activeEngine.GetColor(x - 1, y - 1) * GlobalBrightness;
		slices[3] = _activeEngine.GetColor(x - 1, y) * GlobalBrightness;
		slices[6] = _activeEngine.GetColor(x - 1, y + 1) * GlobalBrightness;
		slices[1] = _activeEngine.GetColor(x, y - 1) * GlobalBrightness;
		slices[4] = _activeEngine.GetColor(x, y) * GlobalBrightness;
		slices[7] = _activeEngine.GetColor(x, y + 1) * GlobalBrightness;
		slices[2] = _activeEngine.GetColor(x + 1, y - 1) * GlobalBrightness;
		slices[5] = _activeEngine.GetColor(x + 1, y) * GlobalBrightness;
		slices[8] = _activeEngine.GetColor(x + 1, y + 1) * GlobalBrightness;
	}

	public static void GetCornerColors(int centerX, int centerY, out VertexColors vertices, float scale = 1f)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0100: Unknown result type (might be due to invalid IL or missing references)
		//IL_0108: Unknown result type (might be due to invalid IL or missing references)
		//IL_016b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0171: Unknown result type (might be due to invalid IL or missing references)
		//IL_0179: Unknown result type (might be due to invalid IL or missing references)
		//IL_0181: Unknown result type (might be due to invalid IL or missing references)
		//IL_018e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0194: Unknown result type (might be due to invalid IL or missing references)
		//IL_019c: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01be: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0229: Unknown result type (might be due to invalid IL or missing references)
		//IL_022f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0237: Unknown result type (might be due to invalid IL or missing references)
		//IL_023f: Unknown result type (might be due to invalid IL or missing references)
		//IL_024c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0252: Unknown result type (might be due to invalid IL or missing references)
		//IL_025a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0262: Unknown result type (might be due to invalid IL or missing references)
		//IL_026f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0275: Unknown result type (might be due to invalid IL or missing references)
		//IL_027d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0285: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_030b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0311: Unknown result type (might be due to invalid IL or missing references)
		//IL_0319: Unknown result type (might be due to invalid IL or missing references)
		//IL_0321: Unknown result type (might be due to invalid IL or missing references)
		//IL_032e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0334: Unknown result type (might be due to invalid IL or missing references)
		//IL_033c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0344: Unknown result type (might be due to invalid IL or missing references)
		vertices = default;
		Vector3 color = _activeEngine.GetColor(centerX, centerY);
		Vector3 color2 = _activeEngine.GetColor(centerX, centerY - 1);
		Vector3 color3 = _activeEngine.GetColor(centerX, centerY + 1);
		Vector3 color4 = _activeEngine.GetColor(centerX - 1, centerY);
		Vector3 color5 = _activeEngine.GetColor(centerX + 1, centerY);
		Vector3 color6 = _activeEngine.GetColor(centerX - 1, centerY - 1);
		Vector3 color7 = _activeEngine.GetColor(centerX + 1, centerY - 1);
		Vector3 color8 = _activeEngine.GetColor(centerX - 1, centerY + 1);
		Vector3 color9 = _activeEngine.GetColor(centerX + 1, centerY + 1);
		float num = GlobalBrightness * scale * 63.75f;
		int num2 = (int)((color2.X + color6.X + color4.X + color.X) * num);
		int num3 = (int)((color2.Y + color6.Y + color4.Y + color.Y) * num);
		int num4 = (int)((color2.Z + color6.Z + color4.Z + color.Z) * num);
		if (num2 > 255)
		{
			num2 = 255;
		}
		if (num3 > 255)
		{
			num3 = 255;
		}
		if (num4 > 255)
		{
			num4 = 255;
		}
		num3 <<= 8;
		num4 <<= 16;
		vertices.TopLeftColor.PackedValue = (uint)(num2 | num3 | num4 | -16777216);
		num2 = (int)((color2.X + color7.X + color5.X + color.X) * num);
		num3 = (int)((color2.Y + color7.Y + color5.Y + color.Y) * num);
		num4 = (int)((color2.Z + color7.Z + color5.Z + color.Z) * num);
		if (num2 > 255)
		{
			num2 = 255;
		}
		if (num3 > 255)
		{
			num3 = 255;
		}
		if (num4 > 255)
		{
			num4 = 255;
		}
		num3 <<= 8;
		num4 <<= 16;
		vertices.TopRightColor.PackedValue = (uint)(num2 | num3 | num4 | -16777216);
		num2 = (int)((color3.X + color8.X + color4.X + color.X) * num);
		num3 = (int)((color3.Y + color8.Y + color4.Y + color.Y) * num);
		num4 = (int)((color3.Z + color8.Z + color4.Z + color.Z) * num);
		if (num2 > 255)
		{
			num2 = 255;
		}
		if (num3 > 255)
		{
			num3 = 255;
		}
		if (num4 > 255)
		{
			num4 = 255;
		}
		num3 <<= 8;
		num4 <<= 16;
		vertices.BottomLeftColor.PackedValue = (uint)(num2 | num3 | num4 | -16777216);
		num2 = (int)((color3.X + color9.X + color5.X + color.X) * num);
		num3 = (int)((color3.Y + color9.Y + color5.Y + color.Y) * num);
		num4 = (int)((color3.Z + color9.Z + color5.Z + color.Z) * num);
		if (num2 > 255)
		{
			num2 = 255;
		}
		if (num3 > 255)
		{
			num3 = 255;
		}
		if (num4 > 255)
		{
			num4 = 255;
		}
		num3 <<= 8;
		num4 <<= 16;
		vertices.BottomRightColor.PackedValue = (uint)(num2 | num3 | num4 | -16777216);
	}

	public static void GetColor4Slice(int centerX, int centerY, ref Color[] slices)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_013d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0152: Unknown result type (might be due to invalid IL or missing references)
		//IL_0167: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0129: Unknown result type (might be due to invalid IL or missing references)
		//IL_012e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0260: Unknown result type (might be due to invalid IL or missing references)
		//IL_0275: Unknown result type (might be due to invalid IL or missing references)
		//IL_028a: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_02db: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_024c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0251: Unknown result type (might be due to invalid IL or missing references)
		//IL_0385: Unknown result type (might be due to invalid IL or missing references)
		//IL_039a: Unknown result type (might be due to invalid IL or missing references)
		//IL_03af: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0309: Unknown result type (might be due to invalid IL or missing references)
		//IL_031f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0400: Unknown result type (might be due to invalid IL or missing references)
		//IL_0405: Unknown result type (might be due to invalid IL or missing references)
		//IL_0371: Unknown result type (might be due to invalid IL or missing references)
		//IL_0376: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_04bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0418: Unknown result type (might be due to invalid IL or missing references)
		//IL_042e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0444: Unknown result type (might be due to invalid IL or missing references)
		//IL_0521: Unknown result type (might be due to invalid IL or missing references)
		//IL_0526: Unknown result type (might be due to invalid IL or missing references)
		//IL_0496: Unknown result type (might be due to invalid IL or missing references)
		//IL_049b: Unknown result type (might be due to invalid IL or missing references)
		Vector3 color = _activeEngine.GetColor(centerX, centerY - 1);
		Vector3 color2 = _activeEngine.GetColor(centerX, centerY + 1);
		Vector3 color3 = _activeEngine.GetColor(centerX - 1, centerY);
		Vector3 color4 = _activeEngine.GetColor(centerX + 1, centerY);
		float num = color.X + color.Y + color.Z;
		float num2 = color2.X + color2.Y + color2.Z;
		float num3 = color4.X + color4.Y + color4.Z;
		float num4 = color3.X + color3.Y + color3.Z;
		if (num >= num4)
		{
			int num5 = (int)(255f * color3.X * GlobalBrightness);
			int num6 = (int)(255f * color3.Y * GlobalBrightness);
			int num7 = (int)(255f * color3.Z * GlobalBrightness);
			if (num5 > 255)
			{
				num5 = 255;
			}
			if (num6 > 255)
			{
				num6 = 255;
			}
			if (num7 > 255)
			{
				num7 = 255;
			}
			slices[0] = new Color((int)(byte)num5, (int)(byte)num6, (int)(byte)num7, 255);
		}
		else
		{
			int num8 = (int)(255f * color.X * GlobalBrightness);
			int num9 = (int)(255f * color.Y * GlobalBrightness);
			int num10 = (int)(255f * color.Z * GlobalBrightness);
			if (num8 > 255)
			{
				num8 = 255;
			}
			if (num9 > 255)
			{
				num9 = 255;
			}
			if (num10 > 255)
			{
				num10 = 255;
			}
			slices[0] = new Color((int)(byte)num8, (int)(byte)num9, (int)(byte)num10, 255);
		}
		if (num >= num3)
		{
			int num11 = (int)(255f * color4.X * GlobalBrightness);
			int num12 = (int)(255f * color4.Y * GlobalBrightness);
			int num13 = (int)(255f * color4.Z * GlobalBrightness);
			if (num11 > 255)
			{
				num11 = 255;
			}
			if (num12 > 255)
			{
				num12 = 255;
			}
			if (num13 > 255)
			{
				num13 = 255;
			}
			slices[1] = new Color((int)(byte)num11, (int)(byte)num12, (int)(byte)num13, 255);
		}
		else
		{
			int num14 = (int)(255f * color.X * GlobalBrightness);
			int num15 = (int)(255f * color.Y * GlobalBrightness);
			int num16 = (int)(255f * color.Z * GlobalBrightness);
			if (num14 > 255)
			{
				num14 = 255;
			}
			if (num15 > 255)
			{
				num15 = 255;
			}
			if (num16 > 255)
			{
				num16 = 255;
			}
			slices[1] = new Color((int)(byte)num14, (int)(byte)num15, (int)(byte)num16, 255);
		}
		if (num2 >= num4)
		{
			int num17 = (int)(255f * color3.X * GlobalBrightness);
			int num18 = (int)(255f * color3.Y * GlobalBrightness);
			int num19 = (int)(255f * color3.Z * GlobalBrightness);
			if (num17 > 255)
			{
				num17 = 255;
			}
			if (num18 > 255)
			{
				num18 = 255;
			}
			if (num19 > 255)
			{
				num19 = 255;
			}
			slices[2] = new Color((int)(byte)num17, (int)(byte)num18, (int)(byte)num19, 255);
		}
		else
		{
			int num20 = (int)(255f * color2.X * GlobalBrightness);
			int num21 = (int)(255f * color2.Y * GlobalBrightness);
			int num22 = (int)(255f * color2.Z * GlobalBrightness);
			if (num20 > 255)
			{
				num20 = 255;
			}
			if (num21 > 255)
			{
				num21 = 255;
			}
			if (num22 > 255)
			{
				num22 = 255;
			}
			slices[2] = new Color((int)(byte)num20, (int)(byte)num21, (int)(byte)num22, 255);
		}
		if (num2 >= num3)
		{
			int num23 = (int)(255f * color4.X * GlobalBrightness);
			int num24 = (int)(255f * color4.Y * GlobalBrightness);
			int num25 = (int)(255f * color4.Z * GlobalBrightness);
			if (num23 > 255)
			{
				num23 = 255;
			}
			if (num24 > 255)
			{
				num24 = 255;
			}
			if (num25 > 255)
			{
				num25 = 255;
			}
			slices[3] = new Color((int)(byte)num23, (int)(byte)num24, (int)(byte)num25, 255);
		}
		else
		{
			int num26 = (int)(255f * color2.X * GlobalBrightness);
			int num27 = (int)(255f * color2.Y * GlobalBrightness);
			int num28 = (int)(255f * color2.Z * GlobalBrightness);
			if (num26 > 255)
			{
				num26 = 255;
			}
			if (num27 > 255)
			{
				num27 = 255;
			}
			if (num28 > 255)
			{
				num28 = 255;
			}
			slices[3] = new Color((int)(byte)num26, (int)(byte)num27, (int)(byte)num28, 255);
		}
	}

	public static void GetColor4Slice(int x, int y, ref Vector3[] slices)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		//IL_010f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0114: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0136: Unknown result type (might be due to invalid IL or missing references)
		//IL_013c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0141: Unknown result type (might be due to invalid IL or missing references)
		//IL_0122: Unknown result type (might be due to invalid IL or missing references)
		//IL_0128: Unknown result type (might be due to invalid IL or missing references)
		//IL_012d: Unknown result type (might be due to invalid IL or missing references)
		Vector3 color = _activeEngine.GetColor(x, y - 1);
		Vector3 color2 = _activeEngine.GetColor(x, y + 1);
		Vector3 color3 = _activeEngine.GetColor(x - 1, y);
		Vector3 color4 = _activeEngine.GetColor(x + 1, y);
		float num = color.X + color.Y + color.Z;
		float num2 = color2.X + color2.Y + color2.Z;
		float num3 = color4.X + color4.Y + color4.Z;
		float num4 = color3.X + color3.Y + color3.Z;
		if (num >= num4)
		{
			slices[0] = color3 * GlobalBrightness;
		}
		else
		{
			slices[0] = color * GlobalBrightness;
		}
		if (num >= num3)
		{
			slices[1] = color4 * GlobalBrightness;
		}
		else
		{
			slices[1] = color * GlobalBrightness;
		}
		if (num2 >= num4)
		{
			slices[2] = color3 * GlobalBrightness;
		}
		else
		{
			slices[2] = color2 * GlobalBrightness;
		}
		if (num2 >= num3)
		{
			slices[3] = color4 * GlobalBrightness;
		}
		else
		{
			slices[3] = color2 * GlobalBrightness;
		}
	}

	internal static void AddGameplaySnapshotComponents()
	{
		NewEngine.AddGameplaySnapshotComponents();
	}
}
