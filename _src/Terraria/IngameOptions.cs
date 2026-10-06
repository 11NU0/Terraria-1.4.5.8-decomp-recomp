using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.GameContent.UI.BigProgressBar;
using Terraria.GameInput;
using Terraria.Localization;
using Terraria.Social;
using Terraria.UI;
using Terraria.UI.Gamepad;

namespace Terraria;

public static class IngameOptions
{
	public const int width = 670;

	public const int height = 480;

	public static float[] leftScale = new float[10] { 0.7f, 0.7f, 0.7f, 0.7f, 0.7f, 0.7f, 0.7f, 0.7f, 0.7f, 0.7f };

	public static float[] rightScale = new float[20]
	{
		0.7f, 0.7f, 0.7f, 0.7f, 0.7f, 0.7f, 0.7f, 0.7f, 0.7f, 0.7f,
		0.7f, 0.7f, 0.7f, 0.7f, 0.7f, 0.7f, 0.7f, 0.7f, 0.7f, 0.7f
	};

	private static Dictionary<int, int> _leftSideCategoryMapping = new Dictionary<int, int>
	{
		{ 0, 0 },
		{ 1, 1 },
		{ 2, 2 },
		{ 3, 3 }
	};

	public static bool[] skipRightSlot = new bool[20];

	public static int leftHover = -1;

	public static int rightHover = -1;

	public static int oldLeftHover = -1;

	public static int oldRightHover = -1;

	public static int rightLock = -1;

	public static bool inBar;

	public static bool notBar;

	public static bool noSound;

	private static Rectangle _GUIHover;

	public static int category;

	public static Vector2 valuePosition = Vector2.Zero;

	private static string _mouseOverText;

	private static bool _canConsumeHover;

	public static void Open()
	{
		Main.ClosePlayerChat();
		Main.chatText = "";
		Main.playerInventory = false;
		Main.editChest = false;
		Main.npcChatText = "";
		SoundEngine.PlaySound(10);
		Main.ingameOptionsWindow = true;
		category = 0;
		for (int i = 0; i < leftScale.Length; i++)
		{
			leftScale[i] = 0f;
		}
		for (int j = 0; j < rightScale.Length; j++)
		{
			rightScale[j] = 0f;
		}
		leftHover = -1;
		rightHover = -1;
		oldLeftHover = -1;
		oldRightHover = -1;
		rightLock = -1;
		inBar = false;
		notBar = false;
		noSound = false;
	}

	public static void Close()
	{
		Close(false);
	}

	public static void Close(bool quiet = false)
	{
		if (Main.setKey == -1)
		{
			Main.ingameOptionsWindow = false;
			if (!quiet)
			{
				SoundEngine.PlaySound(11);
			}
			Main.playerInventory = true;
			Main.SaveSettings();
		}
	}

	public static void Draw(Main mainInstance, SpriteBatch sb)
	{
		//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0107: Unknown result type (might be due to invalid IL or missing references)
		//IL_010c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0113: Unknown result type (might be due to invalid IL or missing references)
		//IL_0118: Unknown result type (might be due to invalid IL or missing references)
		//IL_011d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0123: Unknown result type (might be due to invalid IL or missing references)
		//IL_012f: Unknown result type (might be due to invalid IL or missing references)
		//IL_013b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0149: Unknown result type (might be due to invalid IL or missing references)
		//IL_0157: Unknown result type (might be due to invalid IL or missing references)
		//IL_015c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0162: Unknown result type (might be due to invalid IL or missing references)
		//IL_016d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0178: Unknown result type (might be due to invalid IL or missing references)
		//IL_0185: Unknown result type (might be due to invalid IL or missing references)
		//IL_019d: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_021c: Unknown result type (might be due to invalid IL or missing references)
		//IL_021e: Unknown result type (might be due to invalid IL or missing references)
		//IL_022a: Unknown result type (might be due to invalid IL or missing references)
		//IL_022f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0234: Unknown result type (might be due to invalid IL or missing references)
		//IL_0239: Unknown result type (might be due to invalid IL or missing references)
		//IL_0309: Unknown result type (might be due to invalid IL or missing references)
		//IL_0316: Unknown result type (might be due to invalid IL or missing references)
		//IL_0325: Unknown result type (might be due to invalid IL or missing references)
		//IL_0336: Unknown result type (might be due to invalid IL or missing references)
		//IL_0345: Unknown result type (might be due to invalid IL or missing references)
		//IL_034b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0353: Unknown result type (might be due to invalid IL or missing references)
		//IL_035a: Unknown result type (might be due to invalid IL or missing references)
		//IL_036c: Unknown result type (might be due to invalid IL or missing references)
		//IL_037b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0390: Unknown result type (might be due to invalid IL or missing references)
		//IL_039f: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_025a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0267: Unknown result type (might be due to invalid IL or missing references)
		//IL_0276: Unknown result type (might be due to invalid IL or missing references)
		//IL_0287: Unknown result type (might be due to invalid IL or missing references)
		//IL_0296: Unknown result type (might be due to invalid IL or missing references)
		//IL_029c: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_04cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_04dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_04eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_05bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_05be: Unknown result type (might be due to invalid IL or missing references)
		//IL_061a: Unknown result type (might be due to invalid IL or missing references)
		//IL_061c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0675: Unknown result type (might be due to invalid IL or missing references)
		//IL_0677: Unknown result type (might be due to invalid IL or missing references)
		//IL_06d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_06d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_072e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0730: Unknown result type (might be due to invalid IL or missing references)
		//IL_077f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0781: Unknown result type (might be due to invalid IL or missing references)
		//IL_07d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_07d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_081f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0821: Unknown result type (might be due to invalid IL or missing references)
		//IL_0864: Unknown result type (might be due to invalid IL or missing references)
		//IL_0866: Unknown result type (might be due to invalid IL or missing references)
		//IL_09cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_09d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_09e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_09f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_09ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a0c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a16: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a1b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a3f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a4a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a51: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b30: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b32: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b43: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b49: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bb5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bb7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bd4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bda: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c03: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c0a: Unknown result type (might be due to invalid IL or missing references)
		//IL_192f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1931: Unknown result type (might be due to invalid IL or missing references)
		//IL_194e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1954: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c82: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c89: Unknown result type (might be due to invalid IL or missing references)
		//IL_2a23: Unknown result type (might be due to invalid IL or missing references)
		//IL_2a28: Unknown result type (might be due to invalid IL or missing references)
		//IL_2a44: Unknown result type (might be due to invalid IL or missing references)
		//IL_2a46: Unknown result type (might be due to invalid IL or missing references)
		//IL_2a57: Unknown result type (might be due to invalid IL or missing references)
		//IL_2a5d: Unknown result type (might be due to invalid IL or missing references)
		//IL_2a89: Unknown result type (might be due to invalid IL or missing references)
		//IL_2a8b: Unknown result type (might be due to invalid IL or missing references)
		//IL_2aa8: Unknown result type (might be due to invalid IL or missing references)
		//IL_2aae: Unknown result type (might be due to invalid IL or missing references)
		//IL_2167: Unknown result type (might be due to invalid IL or missing references)
		//IL_2169: Unknown result type (might be due to invalid IL or missing references)
		//IL_2186: Unknown result type (might be due to invalid IL or missing references)
		//IL_218c: Unknown result type (might be due to invalid IL or missing references)
		//IL_19a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_19a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_19c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_19c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_2ad1: Unknown result type (might be due to invalid IL or missing references)
		//IL_2ad8: Unknown result type (might be due to invalid IL or missing references)
		//IL_2b14: Unknown result type (might be due to invalid IL or missing references)
		//IL_2b16: Unknown result type (might be due to invalid IL or missing references)
		//IL_2b1e: Unknown result type (might be due to invalid IL or missing references)
		//IL_21f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_21f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_2212: Unknown result type (might be due to invalid IL or missing references)
		//IL_2218: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d5c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d5e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d7b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d81: Unknown result type (might be due to invalid IL or missing references)
		//IL_3971: Unknown result type (might be due to invalid IL or missing references)
		//IL_3973: Unknown result type (might be due to invalid IL or missing references)
		//IL_397a: Unknown result type (might be due to invalid IL or missing references)
		//IL_397f: Unknown result type (might be due to invalid IL or missing references)
		//IL_3997: Unknown result type (might be due to invalid IL or missing references)
		//IL_399c: Unknown result type (might be due to invalid IL or missing references)
		//IL_22f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_22f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_2313: Unknown result type (might be due to invalid IL or missing references)
		//IL_2319: Unknown result type (might be due to invalid IL or missing references)
		//IL_0da4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dab: Unknown result type (might be due to invalid IL or missing references)
		//IL_2b7c: Unknown result type (might be due to invalid IL or missing references)
		//IL_2b83: Unknown result type (might be due to invalid IL or missing references)
		//IL_2342: Unknown result type (might be due to invalid IL or missing references)
		//IL_2349: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a28: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a2a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a47: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a4d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e29: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e30: Unknown result type (might be due to invalid IL or missing references)
		//IL_3a45: Unknown result type (might be due to invalid IL or missing references)
		//IL_3a50: Unknown result type (might be due to invalid IL or missing references)
		//IL_2c1e: Unknown result type (might be due to invalid IL or missing references)
		//IL_2c20: Unknown result type (might be due to invalid IL or missing references)
		//IL_2c3d: Unknown result type (might be due to invalid IL or missing references)
		//IL_2c43: Unknown result type (might be due to invalid IL or missing references)
		//IL_23e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_23ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_39c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_39c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_39c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_39cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_39d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_39d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_2c66: Unknown result type (might be due to invalid IL or missing references)
		//IL_2c6d: Unknown result type (might be due to invalid IL or missing references)
		//IL_2ca9: Unknown result type (might be due to invalid IL or missing references)
		//IL_2cab: Unknown result type (might be due to invalid IL or missing references)
		//IL_2cb3: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a9a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a9c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ab9: Unknown result type (might be due to invalid IL or missing references)
		//IL_1abf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f03: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f05: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f22: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f28: Unknown result type (might be due to invalid IL or missing references)
		//IL_24a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_24a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_24bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_24c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f4b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f52: Unknown result type (might be due to invalid IL or missing references)
		//IL_2d11: Unknown result type (might be due to invalid IL or missing references)
		//IL_2d18: Unknown result type (might be due to invalid IL or missing references)
		//IL_2500: Unknown result type (might be due to invalid IL or missing references)
		//IL_2502: Unknown result type (might be due to invalid IL or missing references)
		//IL_251f: Unknown result type (might be due to invalid IL or missing references)
		//IL_2525: Unknown result type (might be due to invalid IL or missing references)
		//IL_255a: Unknown result type (might be due to invalid IL or missing references)
		//IL_255c: Unknown result type (might be due to invalid IL or missing references)
		//IL_2579: Unknown result type (might be due to invalid IL or missing references)
		//IL_257f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b12: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b14: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b31: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b37: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fd0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fd7: Unknown result type (might be due to invalid IL or missing references)
		//IL_2db3: Unknown result type (might be due to invalid IL or missing references)
		//IL_2db5: Unknown result type (might be due to invalid IL or missing references)
		//IL_2dd2: Unknown result type (might be due to invalid IL or missing references)
		//IL_2dd8: Unknown result type (might be due to invalid IL or missing references)
		//IL_2dfb: Unknown result type (might be due to invalid IL or missing references)
		//IL_2e02: Unknown result type (might be due to invalid IL or missing references)
		//IL_2e3e: Unknown result type (might be due to invalid IL or missing references)
		//IL_2e40: Unknown result type (might be due to invalid IL or missing references)
		//IL_1079: Unknown result type (might be due to invalid IL or missing references)
		//IL_107b: Unknown result type (might be due to invalid IL or missing references)
		//IL_108c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1092: Unknown result type (might be due to invalid IL or missing references)
		//IL_10b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_10b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_10c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_10cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_1114: Unknown result type (might be due to invalid IL or missing references)
		//IL_25d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_25db: Unknown result type (might be due to invalid IL or missing references)
		//IL_25f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_25fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b8a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b8c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ba9: Unknown result type (might be due to invalid IL or missing references)
		//IL_1baf: Unknown result type (might be due to invalid IL or missing references)
		//IL_115e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1160: Unknown result type (might be due to invalid IL or missing references)
		//IL_1183: Unknown result type (might be due to invalid IL or missing references)
		//IL_1189: Unknown result type (might be due to invalid IL or missing references)
		//IL_2ed9: Unknown result type (might be due to invalid IL or missing references)
		//IL_2ee0: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c00: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c02: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c1f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c25: Unknown result type (might be due to invalid IL or missing references)
		//IL_11ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_11b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_2f73: Unknown result type (might be due to invalid IL or missing references)
		//IL_2651: Unknown result type (might be due to invalid IL or missing references)
		//IL_2653: Unknown result type (might be due to invalid IL or missing references)
		//IL_2670: Unknown result type (might be due to invalid IL or missing references)
		//IL_2676: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ca1: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ca3: Unknown result type (might be due to invalid IL or missing references)
		//IL_1cc6: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ccc: Unknown result type (might be due to invalid IL or missing references)
		//IL_1237: Unknown result type (might be due to invalid IL or missing references)
		//IL_123e: Unknown result type (might be due to invalid IL or missing references)
		//IL_2f92: Unknown result type (might be due to invalid IL or missing references)
		//IL_2f9e: Unknown result type (might be due to invalid IL or missing references)
		//IL_2fa3: Unknown result type (might be due to invalid IL or missing references)
		//IL_2fbd: Unknown result type (might be due to invalid IL or missing references)
		//IL_2fbf: Unknown result type (might be due to invalid IL or missing references)
		//IL_2fd0: Unknown result type (might be due to invalid IL or missing references)
		//IL_2fd6: Unknown result type (might be due to invalid IL or missing references)
		//IL_2ff2: Unknown result type (might be due to invalid IL or missing references)
		//IL_2ff7: Unknown result type (might be due to invalid IL or missing references)
		//IL_1cef: Unknown result type (might be due to invalid IL or missing references)
		//IL_1cf6: Unknown result type (might be due to invalid IL or missing references)
		//IL_3025: Unknown result type (might be due to invalid IL or missing references)
		//IL_3027: Unknown result type (might be due to invalid IL or missing references)
		//IL_3038: Unknown result type (might be due to invalid IL or missing references)
		//IL_303e: Unknown result type (might be due to invalid IL or missing references)
		//IL_306a: Unknown result type (might be due to invalid IL or missing references)
		//IL_306c: Unknown result type (might be due to invalid IL or missing references)
		//IL_3089: Unknown result type (might be due to invalid IL or missing references)
		//IL_308f: Unknown result type (might be due to invalid IL or missing references)
		//IL_30b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_30b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_30f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_30f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_30ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_26c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_26c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_26e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_26ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d86: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d8d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1344: Unknown result type (might be due to invalid IL or missing references)
		//IL_1346: Unknown result type (might be due to invalid IL or missing references)
		//IL_1369: Unknown result type (might be due to invalid IL or missing references)
		//IL_136f: Unknown result type (might be due to invalid IL or missing references)
		//IL_315d: Unknown result type (might be due to invalid IL or missing references)
		//IL_3164: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e56: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e58: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e75: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e7b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1392: Unknown result type (might be due to invalid IL or missing references)
		//IL_1399: Unknown result type (might be due to invalid IL or missing references)
		//IL_273d: Unknown result type (might be due to invalid IL or missing references)
		//IL_273f: Unknown result type (might be due to invalid IL or missing references)
		//IL_275c: Unknown result type (might be due to invalid IL or missing references)
		//IL_2762: Unknown result type (might be due to invalid IL or missing references)
		//IL_3202: Unknown result type (might be due to invalid IL or missing references)
		//IL_3204: Unknown result type (might be due to invalid IL or missing references)
		//IL_3221: Unknown result type (might be due to invalid IL or missing references)
		//IL_3227: Unknown result type (might be due to invalid IL or missing references)
		//IL_324a: Unknown result type (might be due to invalid IL or missing references)
		//IL_3251: Unknown result type (might be due to invalid IL or missing references)
		//IL_328d: Unknown result type (might be due to invalid IL or missing references)
		//IL_328f: Unknown result type (might be due to invalid IL or missing references)
		//IL_3297: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ef8: Unknown result type (might be due to invalid IL or missing references)
		//IL_1efa: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f17: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f1d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1480: Unknown result type (might be due to invalid IL or missing references)
		//IL_1487: Unknown result type (might be due to invalid IL or missing references)
		//IL_27b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_27b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_27d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_27d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_32f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_32fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_152a: Unknown result type (might be due to invalid IL or missing references)
		//IL_152c: Unknown result type (might be due to invalid IL or missing references)
		//IL_153d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1543: Unknown result type (might be due to invalid IL or missing references)
		//IL_1567: Unknown result type (might be due to invalid IL or missing references)
		//IL_1569: Unknown result type (might be due to invalid IL or missing references)
		//IL_157a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1580: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f5b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f5d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f7a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f80: Unknown result type (might be due to invalid IL or missing references)
		//IL_339a: Unknown result type (might be due to invalid IL or missing references)
		//IL_339c: Unknown result type (might be due to invalid IL or missing references)
		//IL_33b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_33bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_2862: Unknown result type (might be due to invalid IL or missing references)
		//IL_2864: Unknown result type (might be due to invalid IL or missing references)
		//IL_2881: Unknown result type (might be due to invalid IL or missing references)
		//IL_2887: Unknown result type (might be due to invalid IL or missing references)
		//IL_15bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_15bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_15dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_15e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_33e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_33e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_3425: Unknown result type (might be due to invalid IL or missing references)
		//IL_3427: Unknown result type (might be due to invalid IL or missing references)
		//IL_342f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1fbf: Unknown result type (might be due to invalid IL or missing references)
		//IL_1fc1: Unknown result type (might be due to invalid IL or missing references)
		//IL_1fde: Unknown result type (might be due to invalid IL or missing references)
		//IL_1fe4: Unknown result type (might be due to invalid IL or missing references)
		//IL_348d: Unknown result type (might be due to invalid IL or missing references)
		//IL_3494: Unknown result type (might be due to invalid IL or missing references)
		//IL_28d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_28d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_28f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_28f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_162f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1631: Unknown result type (might be due to invalid IL or missing references)
		//IL_164e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1654: Unknown result type (might be due to invalid IL or missing references)
		//IL_3532: Unknown result type (might be due to invalid IL or missing references)
		//IL_3534: Unknown result type (might be due to invalid IL or missing references)
		//IL_3551: Unknown result type (might be due to invalid IL or missing references)
		//IL_3557: Unknown result type (might be due to invalid IL or missing references)
		//IL_2054: Unknown result type (might be due to invalid IL or missing references)
		//IL_2056: Unknown result type (might be due to invalid IL or missing references)
		//IL_2073: Unknown result type (might be due to invalid IL or missing references)
		//IL_2079: Unknown result type (might be due to invalid IL or missing references)
		//IL_357a: Unknown result type (might be due to invalid IL or missing references)
		//IL_3581: Unknown result type (might be due to invalid IL or missing references)
		//IL_35bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_35bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_294a: Unknown result type (might be due to invalid IL or missing references)
		//IL_294c: Unknown result type (might be due to invalid IL or missing references)
		//IL_2969: Unknown result type (might be due to invalid IL or missing references)
		//IL_296f: Unknown result type (might be due to invalid IL or missing references)
		//IL_169a: Unknown result type (might be due to invalid IL or missing references)
		//IL_169c: Unknown result type (might be due to invalid IL or missing references)
		//IL_16b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_16bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_3627: Unknown result type (might be due to invalid IL or missing references)
		//IL_362e: Unknown result type (might be due to invalid IL or missing references)
		//IL_20e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_20e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_2101: Unknown result type (might be due to invalid IL or missing references)
		//IL_2107: Unknown result type (might be due to invalid IL or missing references)
		//IL_29b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_29b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_29d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_29da: Unknown result type (might be due to invalid IL or missing references)
		//IL_1712: Unknown result type (might be due to invalid IL or missing references)
		//IL_1714: Unknown result type (might be due to invalid IL or missing references)
		//IL_1731: Unknown result type (might be due to invalid IL or missing references)
		//IL_1737: Unknown result type (might be due to invalid IL or missing references)
		//IL_36c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_36e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_36e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_3700: Unknown result type (might be due to invalid IL or missing references)
		//IL_3702: Unknown result type (might be due to invalid IL or missing references)
		//IL_3713: Unknown result type (might be due to invalid IL or missing references)
		//IL_3719: Unknown result type (might be due to invalid IL or missing references)
		//IL_3792: Unknown result type (might be due to invalid IL or missing references)
		//IL_3794: Unknown result type (might be due to invalid IL or missing references)
		//IL_37b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_37bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_17a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_17a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_17c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_17ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_3808: Unknown result type (might be due to invalid IL or missing references)
		//IL_380a: Unknown result type (might be due to invalid IL or missing references)
		//IL_382d: Unknown result type (might be due to invalid IL or missing references)
		//IL_3833: Unknown result type (might be due to invalid IL or missing references)
		//IL_1818: Unknown result type (might be due to invalid IL or missing references)
		//IL_181a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1837: Unknown result type (might be due to invalid IL or missing references)
		//IL_183d: Unknown result type (might be due to invalid IL or missing references)
		//IL_3880: Unknown result type (might be due to invalid IL or missing references)
		//IL_3882: Unknown result type (might be due to invalid IL or missing references)
		//IL_389f: Unknown result type (might be due to invalid IL or missing references)
		//IL_38a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_1877: Unknown result type (might be due to invalid IL or missing references)
		//IL_1879: Unknown result type (might be due to invalid IL or missing references)
		//IL_1896: Unknown result type (might be due to invalid IL or missing references)
		//IL_189c: Unknown result type (might be due to invalid IL or missing references)
		//IL_18cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_18cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_18de: Unknown result type (might be due to invalid IL or missing references)
		//IL_18e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_38f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_38fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_391d: Unknown result type (might be due to invalid IL or missing references)
		//IL_3923: Unknown result type (might be due to invalid IL or missing references)
		_canConsumeHover = true;
		for (int i = 0; i < skipRightSlot.Length; i++)
		{
			skipRightSlot[i] = false;
		}
		bool flag = GameCulture.FromCultureName(GameCulture.CultureName.Russian).IsActive || GameCulture.FromCultureName(GameCulture.CultureName.Portuguese).IsActive || GameCulture.FromCultureName(GameCulture.CultureName.Polish).IsActive || GameCulture.FromCultureName(GameCulture.CultureName.French).IsActive;
		bool isActive = GameCulture.FromCultureName(GameCulture.CultureName.Polish).IsActive;
		bool isActive2 = GameCulture.FromCultureName(GameCulture.CultureName.German).IsActive;
		bool flag2 = GameCulture.FromCultureName(GameCulture.CultureName.Italian).IsActive || GameCulture.FromCultureName(GameCulture.CultureName.Spanish).IsActive;
		bool flag3 = false;
		int num = 70;
		float scale = 0.75f;
		float num2 = 60f;
		float num3 = 300f;
		if (flag)
		{
			flag3 = true;
		}
		if (isActive)
		{
			num3 = 200f;
		}
		new Vector2((float)Main.mouseX, (float)Main.mouseY);
		bool flag4 = Main.mouseLeft && Main.mouseLeftRelease;
		Vector2 val = new Vector2((float)Main.screenWidth, (float)Main.screenHeight);
		Vector2 val2 = new Vector2(670f, 480f);
		Vector2 val3 = val / 2f - val2 / 2f;
		int num4 = 20;
		_GUIHover = new Rectangle((int)(val3.X - (float)num4), (int)(val3.Y - (float)num4), (int)(val2.X + (float)(num4 * 2)), (int)(val2.Y + (float)(num4 * 2)));
		Utils.DrawInvBG(sb, val3.X - (float)num4, val3.Y - (float)num4, val2.X + (float)(num4 * 2), val2.Y + (float)(num4 * 2), new Color(33, 15, 91, 255) * 0.685f);
		Rectangle val4 = new Rectangle((int)val3.X - num4, (int)val3.Y - num4, (int)val2.X + num4 * 2, (int)val2.Y + num4 * 2);
		if (val4.Contains(new Point(Main.mouseX, Main.mouseY)))
		{
			Main.player[Main.myPlayer].mouseInterface = true;
		}
		Utils.DrawBorderString(sb, Language.GetTextValue("GameUI.SettingsMenu"), val3 + val2 * new Vector2(0.5f, 0f), Color.White, 1f, 0.5f);
		if (flag)
		{
			Utils.DrawInvBG(sb, val3.X + (float)(num4 / 2), val3.Y + (float)(num4 * 5 / 2), val2.X / 3f - (float)num4, val2.Y - (float)(num4 * 3));
			Utils.DrawInvBG(sb, val3.X + val2.X / 3f + (float)num4, val3.Y + (float)(num4 * 5 / 2), val2.X * 2f / 3f - (float)(num4 * 3 / 2), val2.Y - (float)(num4 * 3));
		}
		else
		{
			Utils.DrawInvBG(sb, val3.X + (float)(num4 / 2), val3.Y + (float)(num4 * 5 / 2), val2.X / 2f - (float)num4, val2.Y - (float)(num4 * 3));
			Utils.DrawInvBG(sb, val3.X + val2.X / 2f + (float)num4, val3.Y + (float)(num4 * 5 / 2), val2.X / 2f - (float)(num4 * 3 / 2), val2.Y - (float)(num4 * 3));
		}
		float num5 = 0.7f;
		float num6 = 0.8f;
		float num7 = 0.01f;
		if (flag)
		{
			num5 = 0.4f;
			num6 = 0.44f;
		}
		if (isActive2)
		{
			num5 = 0.55f;
			num6 = 0.6f;
		}
		if (oldLeftHover != leftHover && leftHover != -1)
		{
			SoundEngine.PlaySound(12);
		}
		if (oldRightHover != rightHover && rightHover != -1)
		{
			SoundEngine.PlaySound(12);
		}
		if (flag4 && rightHover != -1 && !noSound)
		{
			SoundEngine.PlaySound(12);
		}
		oldLeftHover = leftHover;
		oldRightHover = rightHover;
		noSound = false;
		bool flag5 = SocialAPI.Network != null && SocialAPI.Network.CanInvite();
		int num8 = (flag5 ? 1 : 0);
		int num9 = 5 + num8 + 2;
		Vector2 val5 = new Vector2(val3.X + val2.X / 4f, val3.Y + (float)(num4 * 5 / 2));
		Vector2 val6 = new Vector2(0f, val2.Y - (float)(num4 * 5)) / (float)(num9 + 1);
		if (flag)
		{
			val5.X -= 55f;
		}
		UILinkPointNavigator.Shortcuts.INGAMEOPTIONS_BUTTONS_LEFT = num9 + 1;
		for (int j = 0; j <= num9; j++)
		{
			bool flag6 = false;
			if (_leftSideCategoryMapping.TryGetValue(j, out var value))
			{
				flag6 = category == value;
			}
			if ((leftHover == j) | flag6)
			{
				leftScale[j] += num7;
			}
			else
			{
				leftScale[j] -= num7;
			}
			if (leftScale[j] < num5)
			{
				leftScale[j] = num5;
			}
			if (leftScale[j] > num6)
			{
				leftScale[j] = num6;
			}
		}
		leftHover = -1;
		int num10 = category;
		int num11 = 0;
		if (DrawLeftSide(sb, Lang.menu[114].Value, num11, val5, val6, leftScale))
		{
			leftHover = num11;
			if (flag4)
			{
				category = 0;
				SoundEngine.PlaySound(10);
			}
		}
		num11++;
		if (DrawLeftSide(sb, Lang.menu[210].Value, num11, val5, val6, leftScale))
		{
			leftHover = num11;
			if (flag4)
			{
				category = 1;
				SoundEngine.PlaySound(10);
			}
		}
		num11++;
		if (DrawLeftSide(sb, Lang.menu[63].Value, num11, val5, val6, leftScale))
		{
			leftHover = num11;
			if (flag4)
			{
				category = 2;
				SoundEngine.PlaySound(10);
			}
		}
		num11++;
		if (DrawLeftSide(sb, Lang.menu[218].Value, num11, val5, val6, leftScale))
		{
			leftHover = num11;
			if (flag4)
			{
				category = 3;
				SoundEngine.PlaySound(10);
			}
		}
		num11++;
		if (DrawLeftSide(sb, Lang.menu[66].Value, num11, val5, val6, leftScale))
		{
			leftHover = num11;
			if (flag4)
			{
				Close();
				IngameFancyUI.OpenKeybinds();
			}
		}
		num11++;
		if (flag5 && DrawLeftSide(sb, Lang.menu[147].Value, num11, val5, val6, leftScale))
		{
			leftHover = num11;
			if (flag4)
			{
				Close();
				SocialAPI.Network.OpenInviteInterface();
			}
		}
		if (flag5)
		{
			num11++;
		}
		if (DrawLeftSide(sb, Lang.menu[131].Value, num11, val5, val6, leftScale))
		{
			leftHover = num11;
			if (flag4)
			{
				Close();
				IngameFancyUI.OpenAchievements();
			}
		}
		num11++;
		if (DrawLeftSide(sb, Lang.menu[118].Value, num11, val5, val6, leftScale))
		{
			leftHover = num11;
			if (flag4)
			{
				Close();
			}
		}
		num11++;
		if (DrawLeftSide(sb, Lang.inter[35].Value, num11, val5, val6, leftScale))
		{
			leftHover = num11;
			if (flag4)
			{
				Close();
				Main.menuMode = 10;
				Main.gameMenu = true;
				WorldGen.SaveAndQuit();
			}
		}
		num11++;
		if (num10 != category)
		{
			for (int k = 0; k < rightScale.Length; k++)
			{
				rightScale[k] = 0f;
			}
		}
		int num12 = 0;
		int num13 = 0;
		switch (category)
		{
		case 0:
			num13 = 17;
			num5 = 1f;
			num6 = 1.001f;
			num7 = 0.001f;
			break;
		case 1:
			num13 = 14;
			num5 = 1f;
			num6 = 1.001f;
			num7 = 0.001f;
			break;
		case 2:
			num13 = 14;
			num5 = 1f;
			num6 = 1.001f;
			num7 = 0.001f;
			break;
		case 3:
			num13 = 15;
			num5 = 1f;
			num6 = 1.001f;
			num7 = 0.001f;
			break;
		}
		if (flag)
		{
			num5 -= 0.1f;
			num6 -= 0.1f;
		}
		if (isActive2 && category == 3)
		{
			num5 -= 0.15f;
			num6 -= 0.15f;
		}
		if (flag2 && (category == 0 || category == 3))
		{
			num5 -= 0.2f;
			num6 -= 0.2f;
		}
		UILinkPointNavigator.Shortcuts.INGAMEOPTIONS_BUTTONS_RIGHT = num13;
		Vector2 val7 = new Vector2(val3.X + val2.X * 3f / 4f, val3.Y + (float)(num4 * 5 / 2));
		Vector2 val8 = new Vector2(0f, val2.Y - (float)(num4 * 3)) / (float)(num13 + 1);
		if (category == 2)
		{
			val8.Y -= 2f;
		}
		new Vector2(8f, 0f);
		if (flag)
		{
			val7.X = val3.X + val2.X * 2f / 3f;
		}
		for (int l = 0; l < rightScale.Length; l++)
		{
			if (rightLock == l || (rightHover == l && rightLock == -1))
			{
				rightScale[l] += num7;
			}
			else
			{
				rightScale[l] -= num7;
			}
			if (rightScale[l] < num5)
			{
				rightScale[l] = num5;
			}
			if (rightScale[l] > num6)
			{
				rightScale[l] = num6;
			}
		}
		inBar = false;
		rightHover = -1;
		if (!Main.mouseLeft)
		{
			rightLock = -1;
		}
		if (rightLock == -1)
		{
			notBar = false;
		}
		if (category == 0)
		{
			int num14 = 0;
			DrawRightSide(sb, Lang.menu[65].Value, num14, val7, val8, rightScale[num14], 1f);
			skipRightSlot[num14] = true;
			num14++;
			val7.X -= num;
			if (DrawRightSide(sb, Lang.menu[99].Value + " " + Math.Round(Main.musicVolume * 100f) + "%", num14, val7, val8, rightScale[num14], (rightScale[num14] - num5) / (num6 - num5)))
			{
				if (rightLock == -1)
				{
					notBar = true;
				}
				noSound = true;
				rightHover = num14;
			}
			valuePosition.X = val3.X + val2.X - (float)(num4 / 2) - 20f;
			valuePosition.Y -= 3f;
			float musicVolume = DrawValueBar(sb, scale, Main.musicVolume);
			if ((inBar || rightLock == num14) && !notBar)
			{
				rightHover = num14;
				if (Main.mouseLeft && rightLock == num14)
				{
					Main.musicVolume = musicVolume;
				}
			}
			if ((float)Main.mouseX > val3.X + val2.X * 2f / 3f + (float)num4 && (float)Main.mouseX < valuePosition.X + 3.75f && (float)Main.mouseY > valuePosition.Y - 10f && (float)Main.mouseY <= valuePosition.Y + 10f)
			{
				if (rightLock == -1)
				{
					notBar = true;
				}
				rightHover = num14;
			}
			if (rightHover == num14)
			{
				UILinkPointNavigator.Shortcuts.OPTIONS_BUTTON_SPECIALFEATURE = 2;
			}
			num14++;
			if (DrawRightSide(sb, Lang.menu[98].Value + " " + Math.Round(Main.soundVolume * 100f) + "%", num14, val7, val8, rightScale[num14], (rightScale[num14] - num5) / (num6 - num5)))
			{
				if (rightLock == -1)
				{
					notBar = true;
				}
				rightHover = num14;
			}
			valuePosition.X = val3.X + val2.X - (float)(num4 / 2) - 20f;
			valuePosition.Y -= 3f;
			float soundVolume = DrawValueBar(sb, scale, Main.soundVolume);
			if ((inBar || rightLock == num14) && !notBar)
			{
				rightHover = num14;
				if (Main.mouseLeft && rightLock == num14)
				{
					Main.soundVolume = soundVolume;
					noSound = true;
				}
			}
			if ((float)Main.mouseX > val3.X + val2.X * 2f / 3f + (float)num4 && (float)Main.mouseX < valuePosition.X + 3.75f && (float)Main.mouseY > valuePosition.Y - 10f && (float)Main.mouseY <= valuePosition.Y + 10f)
			{
				if (rightLock == -1)
				{
					notBar = true;
				}
				rightHover = num14;
			}
			if (rightHover == num14)
			{
				UILinkPointNavigator.Shortcuts.OPTIONS_BUTTON_SPECIALFEATURE = 3;
			}
			num14++;
			if (DrawRightSide(sb, Lang.menu[119].Value + " " + Math.Round(Main.ambientVolume * 100f) + "%", num14, val7, val8, rightScale[num14], (rightScale[num14] - num5) / (num6 - num5)))
			{
				if (rightLock == -1)
				{
					notBar = true;
				}
				rightHover = num14;
			}
			valuePosition.X = val3.X + val2.X - (float)(num4 / 2) - 20f;
			valuePosition.Y -= 3f;
			float ambientVolume = DrawValueBar(sb, scale, Main.ambientVolume);
			if ((inBar || rightLock == num14) && !notBar)
			{
				rightHover = num14;
				if (Main.mouseLeft && rightLock == num14)
				{
					Main.ambientVolume = ambientVolume;
					noSound = true;
				}
			}
			if ((float)Main.mouseX > val3.X + val2.X * 2f / 3f + (float)num4 && (float)Main.mouseX < valuePosition.X + 3.75f && (float)Main.mouseY > valuePosition.Y - 10f && (float)Main.mouseY <= valuePosition.Y + 10f)
			{
				if (rightLock == -1)
				{
					notBar = true;
				}
				rightHover = num14;
			}
			if (rightHover == num14)
			{
				UILinkPointNavigator.Shortcuts.OPTIONS_BUTTON_SPECIALFEATURE = 4;
			}
			num14++;
			val7.X += num;
			DrawRightSide(sb, "", num14, val7, val8, rightScale[num14], 1f);
			skipRightSlot[num14] = true;
			num14++;
			DrawRightSide(sb, Language.GetTextValue("GameUI.ZoomCategory"), num14, val7, val8, rightScale[num14], 1f);
			skipRightSlot[num14] = true;
			num14++;
			val7.X -= num;
			string text = Language.GetTextValue("GameUI.GameZoom", Math.Round(Main.GameZoomTarget * 100f), Math.Round(Main.GameViewMatrix.Zoom.X * 100f));
			if (flag3)
			{
				text = FontAssets.ItemStack.Value.CreateWrappedText(text, num3, Language.ActiveCulture.CultureInfo);
			}
			if (DrawRightSide(sb, text, num14, val7, val8, rightScale[num14] * 0.85f, (rightScale[num14] - num5) / (num6 - num5)))
			{
				if (rightLock == -1)
				{
					notBar = true;
				}
				rightHover = num14;
			}
			valuePosition.X = val3.X + val2.X - (float)(num4 / 2) - 20f;
			valuePosition.Y -= 3f;
			float num15 = DrawValueBar(sb, scale, Main.GameZoomTarget - 1f);
			if ((inBar || rightLock == num14) && !notBar)
			{
				rightHover = num14;
				if (Main.mouseLeft && rightLock == num14)
				{
					Main.GameZoomTarget = num15 + 1f;
				}
			}
			if ((float)Main.mouseX > val3.X + val2.X * 2f / 3f + (float)num4 && (float)Main.mouseX < valuePosition.X + 3.75f && (float)Main.mouseY > valuePosition.Y - 10f && (float)Main.mouseY <= valuePosition.Y + 10f)
			{
				if (rightLock == -1)
				{
					notBar = true;
				}
				rightHover = num14;
			}
			if (rightHover == num14)
			{
				UILinkPointNavigator.Shortcuts.OPTIONS_BUTTON_SPECIALFEATURE = 10;
			}
			num14++;
			bool flag7 = false;
			if (Main.temporaryGUIScaleSlider == -1f)
			{
				Main.temporaryGUIScaleSlider = Main.UIScaleWanted;
			}
			string text2 = Language.GetTextValue("GameUI.UIScale", Math.Round(Main.temporaryGUIScaleSlider * 100f), Math.Round(Main.UIScale * 100f));
			if (flag3)
			{
				text2 = FontAssets.ItemStack.Value.CreateWrappedText(text2, num3, Language.ActiveCulture.CultureInfo);
			}
			if (DrawRightSide(sb, text2, num14, val7, val8, rightScale[num14] * 0.75f, (rightScale[num14] - num5) / (num6 - num5)))
			{
				if (rightLock == -1)
				{
					notBar = true;
				}
				rightHover = num14;
			}
			valuePosition.X = val3.X + val2.X - (float)(num4 / 2) - 20f;
			valuePosition.Y -= 3f;
			float num16 = DrawValueBar(sb, scale, MathHelper.Clamp((Main.temporaryGUIScaleSlider - 0.5f) / 1.5f, 0f, 1f));
			if ((inBar || rightLock == num14) && !notBar)
			{
				rightHover = num14;
				if (Main.mouseLeft && rightLock == num14)
				{
					Main.temporaryGUIScaleSlider = num16 * 1.5f + 0.5f;
					Main.temporaryGUIScaleSlider = (float)(int)(Main.temporaryGUIScaleSlider * 100f) / 100f;
					Main.temporaryGUIScaleSliderUpdate = true;
					flag7 = true;
				}
			}
			if (!flag7 && Main.temporaryGUIScaleSliderUpdate && Main.temporaryGUIScaleSlider != -1f)
			{
				Main.UIScale = Main.temporaryGUIScaleSlider;
				Main.temporaryGUIScaleSliderUpdate = false;
			}
			if ((float)Main.mouseX > val3.X + val2.X * 2f / 3f + (float)num4 && (float)Main.mouseX < valuePosition.X + 3.75f && (float)Main.mouseY > valuePosition.Y - 10f && (float)Main.mouseY <= valuePosition.Y + 10f)
			{
				if (rightLock == -1)
				{
					notBar = true;
				}
				rightHover = num14;
			}
			if (rightHover == num14)
			{
				UILinkPointNavigator.Shortcuts.OPTIONS_BUTTON_SPECIALFEATURE = 11;
			}
			num14++;
			val7.X += num;
			DrawRightSide(sb, "", num14, val7, val8, rightScale[num14], 1f);
			skipRightSlot[num14] = true;
			num14++;
			DrawRightSide(sb, Language.GetTextValue("GameUI.Gameplay"), num14, val7, val8, rightScale[num14], 1f);
			skipRightSlot[num14] = true;
			num14++;
			if (DrawRightSide(sb, Main.autoSave ? Lang.menu[67].Value : Lang.menu[68].Value, num14, val7, val8, rightScale[num14], (rightScale[num14] - num5) / (num6 - num5)))
			{
				rightHover = num14;
				if (flag4)
				{
					Main.autoSave = !Main.autoSave;
				}
			}
			num14++;
			if (DrawRightSide(sb, Main.autoPause ? Lang.menu[69].Value : Lang.menu[70].Value, num14, val7, val8, rightScale[num14], (rightScale[num14] - num5) / (num6 - num5)))
			{
				rightHover = num14;
				if (flag4)
				{
					Main.autoPause = !Main.autoPause;
				}
			}
			num14++;
			string textValue = Language.GetTextValue(Main.SettingPlayWhenUnfocused ? "UI.PlayWhenUnfocusedOn" : "UI.PlayWhenUnfocusedOff");
			if (DrawRightSide(sb, textValue, num14, val7, val8, rightScale[num14], (rightScale[num14] - num5) / (num6 - num5)))
			{
				rightHover = num14;
				if (flag4)
				{
					Main.SettingPlayWhenUnfocused = !Main.SettingPlayWhenUnfocused;
				}
			}
			num14++;
			if (DrawRightSide(sb, Main.ReversedUpDownArmorSetBonuses ? Lang.menu[220].Value : Lang.menu[221].Value, num14, val7, val8, rightScale[num14], (rightScale[num14] - num5) / (num6 - num5)))
			{
				rightHover = num14;
				if (flag4)
				{
					Main.ReversedUpDownArmorSetBonuses = !Main.ReversedUpDownArmorSetBonuses;
				}
			}
			num14++;
			if (DrawRightSide(sb, DoorOpeningHelper.PreferenceSettings switch
			{
				DoorOpeningHelper.DoorAutoOpeningPreference.EnabledForEverything => Language.GetTextValue("UI.SmartDoorsEnabled"), 
				DoorOpeningHelper.DoorAutoOpeningPreference.EnabledForGamepadOnly => Language.GetTextValue("UI.SmartDoorsGamepad"), 
				_ => Language.GetTextValue("UI.SmartDoorsDisabled"), 
			}, num14, val7, val8, rightScale[num14], (rightScale[num14] - num5) / (num6 - num5)))
			{
				rightHover = num14;
				if (flag4)
				{
					DoorOpeningHelper.CyclePreferences();
				}
			}
			num14++;
			string textValue2;
			if (Player.Settings.HoverControl != ButtonControlMode.Hold)
			{
				_ = 1;
				textValue2 = Language.GetTextValue("UI.HoverControlSettingIsClick");
			}
			else
			{
				textValue2 = Language.GetTextValue("UI.HoverControlSettingIsHold");
			}
			if (DrawRightSide(sb, textValue2, num14, val7, val8, rightScale[num14], (rightScale[num14] - num5) / (num6 - num5)))
			{
				rightHover = num14;
				if (flag4)
				{
					Player.Settings.CycleHoverControl();
				}
			}
			num14++;
			if (DrawRightSide(sb, Language.GetTextValue(Main.SettingsEnabled_AutoReuseAllItems ? "UI.AutoReuseAllOn" : "UI.AutoReuseAllOff"), num14, val7, val8, rightScale[num14], (rightScale[num14] - num5) / (num6 - num5)))
			{
				rightHover = num14;
				if (flag4)
				{
					Main.SettingsEnabled_AutoReuseAllItems = !Main.SettingsEnabled_AutoReuseAllItems;
				}
			}
			num14++;
			DrawRightSide(sb, "", num14, val7, val8, rightScale[num14], 1f);
			skipRightSlot[num14] = true;
			num14++;
		}
		if (category == 1)
		{
			int num17 = 0;
			if (DrawRightSide(sb, Main.showItemText ? Lang.menu[71].Value : Lang.menu[72].Value, num17, val7, val8, rightScale[num17], (rightScale[num17] - num5) / (num6 - num5)))
			{
				rightHover = num17;
				if (flag4)
				{
					Main.showItemText = !Main.showItemText;
				}
			}
			num17++;
			if (DrawRightSide(sb, Lang.menu[123].Value + " " + Lang.menu[124 + Main.invasionProgressMode], num17, val7, val8, rightScale[num17], (rightScale[num17] - num5) / (num6 - num5)))
			{
				rightHover = num17;
				if (flag4)
				{
					Main.invasionProgressMode++;
					if (Main.invasionProgressMode >= 3)
					{
						Main.invasionProgressMode = 0;
					}
				}
			}
			num17++;
			if (DrawRightSide(sb, Main.placementPreview ? Lang.menu[128].Value : Lang.menu[129].Value, num17, val7, val8, rightScale[num17], (rightScale[num17] - num5) / (num6 - num5)))
			{
				rightHover = num17;
				if (flag4)
				{
					Main.placementPreview = !Main.placementPreview;
				}
			}
			num17++;
			if (DrawRightSide(sb, ItemSlot.Options.HighlightNewItems ? Lang.inter[117].Value : Lang.inter[116].Value, num17, val7, val8, rightScale[num17], (rightScale[num17] - num5) / (num6 - num5)))
			{
				rightHover = num17;
				if (flag4)
				{
					ItemSlot.Options.HighlightNewItems = !ItemSlot.Options.HighlightNewItems;
				}
			}
			num17++;
			if (DrawRightSide(sb, Main.MouseShowBuildingGrid ? Lang.menu[229].Value : Lang.menu[230].Value, num17, val7, val8, rightScale[num17], (rightScale[num17] - num5) / (num6 - num5)))
			{
				rightHover = num17;
				if (flag4)
				{
					Main.MouseShowBuildingGrid = !Main.MouseShowBuildingGrid;
				}
			}
			num17++;
			if (DrawRightSide(sb, Main.GamepadDisableInstructionsDisplay ? Lang.menu[241].Value : Lang.menu[242].Value, num17, val7, val8, rightScale[num17], (rightScale[num17] - num5) / (num6 - num5)))
			{
				rightHover = num17;
				if (flag4)
				{
					Main.GamepadDisableInstructionsDisplay = !Main.GamepadDisableInstructionsDisplay;
				}
			}
			num17++;
			string textValue3 = Language.GetTextValue("UI.MinimapFrame_" + Main.MinimapFrameManagerInstance.ActiveSelectionKeyName);
			if (DrawRightSide(sb, Language.GetTextValue("UI.SelectMapBorder", textValue3), num17, val7, val8, rightScale[num17], (rightScale[num17] - num5) / (num6 - num5)))
			{
				rightHover = num17;
				if (flag4)
				{
					Main.MinimapFrameManagerInstance.CycleSelection();
				}
			}
			num17++;
			val7.X -= num;
			string text3 = Language.GetTextValue("GameUI.MapScale", Math.Round(Main.MapScale * 100f));
			if (flag3)
			{
				text3 = FontAssets.ItemStack.Value.CreateWrappedText(text3, num3, Language.ActiveCulture.CultureInfo);
			}
			if (DrawRightSide(sb, text3, num17, val7, val8, rightScale[num17] * 0.85f, (rightScale[num17] - num5) / (num6 - num5)))
			{
				if (rightLock == -1)
				{
					notBar = true;
				}
				rightHover = num17;
			}
			valuePosition.X = val3.X + val2.X - (float)(num4 / 2) - 20f;
			valuePosition.Y -= 3f;
			float num18 = DrawValueBar(sb, scale, (Main.MapScale - 0.5f) / 0.5f);
			if ((inBar || rightLock == num17) && !notBar)
			{
				rightHover = num17;
				if (Main.mouseLeft && rightLock == num17)
				{
					Main.MapScale = num18 * 0.5f + 0.5f;
				}
			}
			if ((float)Main.mouseX > val3.X + val2.X * 2f / 3f + (float)num4 && (float)Main.mouseX < valuePosition.X + 3.75f && (float)Main.mouseY > valuePosition.Y - 10f && (float)Main.mouseY <= valuePosition.Y + 10f)
			{
				if (rightLock == -1)
				{
					notBar = true;
				}
				rightHover = num17;
			}
			if (rightHover == num17)
			{
				UILinkPointNavigator.Shortcuts.OPTIONS_BUTTON_SPECIALFEATURE = 12;
			}
			num17++;
			val7.X += num;
			string activeSetKeyName = Main.ResourceSetsManager.ActiveSetKeyName;
			string textValue4 = Language.GetTextValue("UI.HealthManaStyle_" + activeSetKeyName);
			if (DrawRightSide(sb, Language.GetTextValue("UI.SelectHealthStyle", textValue4), num17, val7, val8, rightScale[num17], (rightScale[num17] - num5) / (num6 - num5)))
			{
				rightHover = num17;
				if (flag4)
				{
					Main.ResourceSetsManager.CycleResourceSet();
				}
			}
			num17++;
			if (DrawRightSide(sb, Main.DialoguePortraitPreference switch
			{
				Main.DialoguePortraitDrawOption.CloseUp => Language.GetTextValue("UI.PortraitsCloseUp"), 
				Main.DialoguePortraitDrawOption.FullBodyRetro => Language.GetTextValue("UI.PortraitsFullBody"), 
				Main.DialoguePortraitDrawOption.Disabled => Language.GetTextValue("UI.PortraitsDisabled"), 
				_ => Language.GetTextValue("UI.PortraitsDetailed"), 
			}, num17, val7, val8, rightScale[num17], (rightScale[num17] - num5) / (num6 - num5)))
			{
				rightHover = num17;
				if (flag4)
				{
					Main.CycleNPCPortraitMode();
				}
			}
			num17++;
			string textValue5 = Language.GetTextValue(BigProgressBarSystem.ShowText ? "UI.ShowBossLifeTextOn" : "UI.ShowBossLifeTextOff");
			if (DrawRightSide(sb, textValue5, num17, val7, val8, rightScale[num17], (rightScale[num17] - num5) / (num6 - num5)))
			{
				rightHover = num17;
				if (flag4)
				{
					BigProgressBarSystem.ToggleShowText();
				}
			}
			num17++;
			if (DrawRightSide(sb, Main.SettingsEnabled_OpaqueBoxBehindTooltips ? Language.GetTextValue("GameUI.HoverTextBoxesOn") : Language.GetTextValue("GameUI.HoverTextBoxesOff"), num17, val7, val8, rightScale[num17], (rightScale[num17] - num5) / (num6 - num5)))
			{
				rightHover = num17;
				if (flag4)
				{
					Main.SettingsEnabled_OpaqueBoxBehindTooltips = !Main.SettingsEnabled_OpaqueBoxBehindTooltips;
				}
			}
			num17++;
			string txt;
			if (ItemSlot.Options.DisableQuickTrash)
			{
				txt = Lang.menu[253].Value;
			}
			else
			{
				txt = (ItemSlot.Options.DisableLeftShiftTrashCan ? Lang.menu[224].Value : Lang.menu[223].Value);
			}
			if (DrawRightSide(sb, txt, num17, val7, val8, rightScale[num17], (rightScale[num17] - num5) / (num6 - num5)))
			{
				rightHover = num17;
				if (flag4)
				{
					if (ItemSlot.Options.DisableQuickTrash)
					{
						ItemSlot.Options.DisableQuickTrash = false;
						ItemSlot.Options.DisableLeftShiftTrashCan = true;
					}
					else if (ItemSlot.Options.DisableLeftShiftTrashCan)
					{
						ItemSlot.Options.DisableLeftShiftTrashCan = false;
					}
					else
					{
						ItemSlot.Options.DisableQuickTrash = true;
						ItemSlot.Options.DisableLeftShiftTrashCan = false;
					}
				}
			}
			num17++;
			string textValue6 = Language.GetTextValue(Main.FlashyEffectsInterface ? "UI.FlashyEffectsInterfaceOn" : "UI.FlashyEffectsInterfaceOff");
			if (DrawRightSide(sb, textValue6, num17, val7, val8, rightScale[num17], (rightScale[num17] - num5) / (num6 - num5)))
			{
				rightHover = num17;
				if (flag4)
				{
					Main.FlashyEffectsInterface = !Main.FlashyEffectsInterface;
				}
			}
			num17++;
		}
		if (category == 2)
		{
			int num19 = 0;
			if (DrawRightSide(sb, Main.graphics.IsFullScreen ? Lang.menu[49].Value : Lang.menu[50].Value, num19, val7, val8, rightScale[num19], (rightScale[num19] - num5) / (num6 - num5)))
			{
				rightHover = num19;
				if (flag4)
				{
					Main.ToggleFullScreen();
				}
			}
			num19++;
			if (DrawRightSide(sb, Lang.menu[51].Value + ": " + Main.PendingResolutionWidth + "x" + Main.PendingResolutionHeight, num19, val7, val8, rightScale[num19], (rightScale[num19] - num5) / (num6 - num5)))
			{
				rightHover = num19;
				if (flag4)
				{
					int num20 = 0;
					for (int m = 0; m < Main.numDisplayModes; m++)
					{
						if (Main.displayWidth[m] == Main.PendingResolutionWidth && Main.displayHeight[m] == Main.PendingResolutionHeight)
						{
							num20 = m;
							break;
						}
					}
					num20++;
					if (num20 >= Main.numDisplayModes)
					{
						num20 = 0;
					}
					Main.PendingResolutionWidth = Main.displayWidth[num20];
					Main.PendingResolutionHeight = Main.displayHeight[num20];
					Main.SetResolution(Main.PendingResolutionWidth, Main.PendingResolutionHeight);
				}
			}
			num19++;
			val7.X -= num;
			if (DrawRightSide(sb, Lang.menu[52].Value + ": " + Main.bgScroll + "%", num19, val7, val8, rightScale[num19], (rightScale[num19] - num5) / (num6 - num5)))
			{
				if (rightLock == -1)
				{
					notBar = true;
				}
				noSound = true;
				rightHover = num19;
			}
			valuePosition.X = val3.X + val2.X - (float)(num4 / 2) - 20f;
			valuePosition.Y -= 3f;
			float num21 = DrawValueBar(sb, scale, (float)Main.bgScroll / 100f);
			if ((inBar || rightLock == num19) && !notBar)
			{
				rightHover = num19;
				if (Main.mouseLeft && rightLock == num19)
				{
					Main.bgScroll = (int)(num21 * 100f);
					Main.caveParallax = 1f - (float)Main.bgScroll / 500f;
				}
			}
			if ((float)Main.mouseX > val3.X + val2.X * 2f / 3f + (float)num4 && (float)Main.mouseX < valuePosition.X + 3.75f && (float)Main.mouseY > valuePosition.Y - 10f && (float)Main.mouseY <= valuePosition.Y + 10f)
			{
				if (rightLock == -1)
				{
					notBar = true;
				}
				rightHover = num19;
			}
			if (rightHover == num19)
			{
				UILinkPointNavigator.Shortcuts.OPTIONS_BUTTON_SPECIALFEATURE = 1;
			}
			num19++;
			val7.X += num;
			if (DrawRightSide(sb, Lang.menu[(int)(247 + Main.FrameSkipMode)].Value, num19, val7, val8, rightScale[num19], (rightScale[num19] - num5) / (num6 - num5)))
			{
				rightHover = num19;
				if (flag4)
				{
					Main.CycleFrameSkipMode();
				}
			}
			num19++;
			if (DrawRightSide(sb, Language.GetTextValue("UI.LightMode_" + Lighting.Mode), num19, val7, val8, rightScale[num19], (rightScale[num19] - num5) / (num6 - num5)))
			{
				rightHover = num19;
				if (flag4)
				{
					Lighting.NextLightMode();
				}
			}
			num19++;
			if (DrawRightSide(sb, Lang.menu[59 + Main.qaStyle].Value, num19, val7, val8, rightScale[num19], (rightScale[num19] - num5) / (num6 - num5)))
			{
				rightHover = num19;
				if (flag4)
				{
					Main.qaStyle++;
					if (Main.qaStyle > 3)
					{
						Main.qaStyle = 0;
					}
				}
			}
			num19++;
			if (DrawRightSide(sb, Main.BackgroundEnabled ? Lang.menu[100].Value : Lang.menu[101].Value, num19, val7, val8, rightScale[num19], (rightScale[num19] - num5) / (num6 - num5)))
			{
				rightHover = num19;
				if (flag4)
				{
					Main.BackgroundEnabled = !Main.BackgroundEnabled;
				}
			}
			num19++;
			if (DrawRightSide(sb, ChildSafety.Disabled ? Lang.menu[132].Value : Lang.menu[133].Value, num19, val7, val8, rightScale[num19], (rightScale[num19] - num5) / (num6 - num5)))
			{
				rightHover = num19;
				if (flag4)
				{
					ChildSafety.Disabled = !ChildSafety.Disabled;
				}
			}
			num19++;
			if (DrawRightSide(sb, Language.GetTextValue("GameUI.ForegroundSunlightEffects", Main.ForegroundSunlightEffects ? Language.GetTextValue("GameUI.Enabled") : Language.GetTextValue("GameUI.Disabled")), num19, val7, val8, rightScale[num19], (rightScale[num19] - num5) / (num6 - num5)))
			{
				rightHover = num19;
				if (flag4)
				{
					Main.ForegroundSunlightEffects = !Main.ForegroundSunlightEffects;
				}
			}
			num19++;
			if (DrawRightSide(sb, Language.GetTextValue("GameUI.HeatDistortion", Main.UseHeatDistortion ? Language.GetTextValue("GameUI.Enabled") : Language.GetTextValue("GameUI.Disabled")), num19, val7, val8, rightScale[num19], (rightScale[num19] - num5) / (num6 - num5)))
			{
				rightHover = num19;
				if (flag4)
				{
					Main.UseHeatDistortion = !Main.UseHeatDistortion;
				}
			}
			num19++;
			if (DrawRightSide(sb, Language.GetTextValue("GameUI.StormEffects", Main.UseStormEffects ? Language.GetTextValue("GameUI.Enabled") : Language.GetTextValue("GameUI.Disabled")), num19, val7, val8, rightScale[num19], (rightScale[num19] - num5) / (num6 - num5)))
			{
				rightHover = num19;
				if (flag4)
				{
					Main.UseStormEffects = !Main.UseStormEffects;
				}
			}
			num19++;
			if (DrawRightSide(sb, Language.GetTextValue("GameUI.WaveQuality", Main.WaveQuality switch
			{
				1 => (object)Language.GetTextValue("GameUI.QualityLow"), 
				2 => Language.GetTextValue("GameUI.QualityMedium"), 
				3 => Language.GetTextValue("GameUI.QualityHigh"), 
				_ => Language.GetTextValue("GameUI.QualityOff"), 
			}), num19, val7, val8, rightScale[num19], (rightScale[num19] - num5) / (num6 - num5)))
			{
				rightHover = num19;
				if (flag4)
				{
					Main.WaveQuality = (Main.WaveQuality + 1) % 4;
				}
			}
			num19++;
			if (DrawRightSide(sb, Language.GetTextValue("UI.TilesSwayInWind" + (Main.SettingsEnabled_TilesSwayInWind ? "On" : "Off")), num19, val7, val8, rightScale[num19], (rightScale[num19] - num5) / (num6 - num5)))
			{
				rightHover = num19;
				if (flag4)
				{
					Main.SettingsEnabled_TilesSwayInWind = !Main.SettingsEnabled_TilesSwayInWind;
				}
			}
			num19++;
			if (DrawRightSide(sb, Language.GetTextValue("GameUI.ScreenShake", Main.UseScreenShake ? Language.GetTextValue("GameUI.Enabled") : Language.GetTextValue("GameUI.Disabled")), num19, val7, val8, rightScale[num19], (rightScale[num19] - num5) / (num6 - num5)))
			{
				rightHover = num19;
				if (flag4)
				{
					Main.UseScreenShake = !Main.UseScreenShake;
				}
			}
			num19++;
			string textValue7 = Language.GetTextValue(Main.FlashyEffectsWorld ? "UI.FlashyEffectsWorldOn" : "UI.FlashyEffectsWorldOff");
			if (DrawRightSide(sb, textValue7, num19, val7, val8, rightScale[num19], (rightScale[num19] - num5) / (num6 - num5)))
			{
				rightHover = num19;
				if (flag4)
				{
					Main.FlashyEffectsWorld = !Main.FlashyEffectsWorld;
				}
			}
			num19++;
		}
		if (category == 3)
		{
			int num22 = 0;
			float num23 = num;
			if (flag)
			{
				num2 = 126f;
			}
			Vector3 hSLVector = Main.mouseColorSlider.GetHSLVector();
			Main.mouseColorSlider.ApplyToMainLegacyBars();
			DrawRightSide(sb, Lang.menu[64].Value, num22, val7, val8, rightScale[num22], 1f);
			skipRightSlot[num22] = true;
			num22++;
			val7.X -= num23;
			if (DrawRightSide(sb, "", num22, val7, val8, rightScale[num22], (rightScale[num22] - num5) / (num6 - num5)))
			{
				if (rightLock == -1)
				{
					notBar = true;
				}
				rightHover = num22;
			}
			valuePosition.X = val3.X + val2.X - (float)(num4 / 2) - 20f;
			valuePosition.Y -= 3f;
			valuePosition.X -= num2;
			DelegateMethods.v3_1 = hSLVector;
			float x = DrawValueBar(sb, scale, hSLVector.X, 0, DelegateMethods.ColorLerp_HSL_H);
			if ((inBar || rightLock == num22) && !notBar)
			{
				rightHover = num22;
				if (Main.mouseLeft && rightLock == num22)
				{
					hSLVector.X = x;
					noSound = true;
				}
			}
			if ((float)Main.mouseX > val3.X + val2.X * 2f / 3f + (float)num4 && (float)Main.mouseX < valuePosition.X + 3.75f && (float)Main.mouseY > valuePosition.Y - 10f && (float)Main.mouseY <= valuePosition.Y + 10f)
			{
				if (rightLock == -1)
				{
					notBar = true;
				}
				rightHover = num22;
			}
			if (rightHover == num22)
			{
				UILinkPointNavigator.Shortcuts.OPTIONS_BUTTON_SPECIALFEATURE = 5;
				Main.menuMode = 25;
			}
			num22++;
			if (DrawRightSide(sb, "", num22, val7, val8, rightScale[num22], (rightScale[num22] - num5) / (num6 - num5)))
			{
				if (rightLock == -1)
				{
					notBar = true;
				}
				rightHover = num22;
			}
			valuePosition.X = val3.X + val2.X - (float)(num4 / 2) - 20f;
			valuePosition.Y -= 3f;
			valuePosition.X -= num2;
			DelegateMethods.v3_1 = hSLVector;
			x = DrawValueBar(sb, scale, hSLVector.Y, 0, DelegateMethods.ColorLerp_HSL_S);
			if ((inBar || rightLock == num22) && !notBar)
			{
				rightHover = num22;
				if (Main.mouseLeft && rightLock == num22)
				{
					hSLVector.Y = x;
					noSound = true;
				}
			}
			if ((float)Main.mouseX > val3.X + val2.X * 2f / 3f + (float)num4 && (float)Main.mouseX < valuePosition.X + 3.75f && (float)Main.mouseY > valuePosition.Y - 10f && (float)Main.mouseY <= valuePosition.Y + 10f)
			{
				if (rightLock == -1)
				{
					notBar = true;
				}
				rightHover = num22;
			}
			if (rightHover == num22)
			{
				UILinkPointNavigator.Shortcuts.OPTIONS_BUTTON_SPECIALFEATURE = 6;
				Main.menuMode = 25;
			}
			num22++;
			if (DrawRightSide(sb, "", num22, val7, val8, rightScale[num22], (rightScale[num22] - num5) / (num6 - num5)))
			{
				if (rightLock == -1)
				{
					notBar = true;
				}
				rightHover = num22;
			}
			valuePosition.X = val3.X + val2.X - (float)(num4 / 2) - 20f;
			valuePosition.Y -= 3f;
			valuePosition.X -= num2;
			DelegateMethods.v3_1 = hSLVector;
			DelegateMethods.v3_1.Z = Utils.GetLerpValue(0.15f, 1f, DelegateMethods.v3_1.Z, clamped: true);
			x = DrawValueBar(sb, scale, DelegateMethods.v3_1.Z, 0, DelegateMethods.ColorLerp_HSL_L);
			if ((inBar || rightLock == num22) && !notBar)
			{
				rightHover = num22;
				if (Main.mouseLeft && rightLock == num22)
				{
					hSLVector.Z = x * 0.85f + 0.15f;
					noSound = true;
				}
			}
			if ((float)Main.mouseX > val3.X + val2.X * 2f / 3f + (float)num4 && (float)Main.mouseX < valuePosition.X + 3.75f && (float)Main.mouseY > valuePosition.Y - 10f && (float)Main.mouseY <= valuePosition.Y + 10f)
			{
				if (rightLock == -1)
				{
					notBar = true;
				}
				rightHover = num22;
			}
			if (rightHover == num22)
			{
				UILinkPointNavigator.Shortcuts.OPTIONS_BUTTON_SPECIALFEATURE = 7;
				Main.menuMode = 25;
			}
			num22++;
			if (hSLVector.Z < 0.15f)
			{
				hSLVector.Z = 0.15f;
			}
			Main.mouseColorSlider.SetHSL(hSLVector);
			Main.mouseColor = Main.mouseColorSlider.GetColor();
			val7.X += num23;
			DrawRightSide(sb, "", num22, val7, val8, rightScale[num22], 1f);
			skipRightSlot[num22] = true;
			num22++;
			hSLVector = Main.mouseBorderColorSlider.GetHSLVector();
			if (PlayerInput.UsingGamepad && rightHover == -1)
			{
				Main.mouseBorderColorSlider.ApplyToMainLegacyBars();
			}
			DrawRightSide(sb, Lang.menu[217].Value, num22, val7, val8, rightScale[num22], 1f);
			skipRightSlot[num22] = true;
			num22++;
			val7.X -= num23;
			if (DrawRightSide(sb, "", num22, val7, val8, rightScale[num22], (rightScale[num22] - num5) / (num6 - num5)))
			{
				if (rightLock == -1)
				{
					notBar = true;
				}
				rightHover = num22;
			}
			valuePosition.X = val3.X + val2.X - (float)(num4 / 2) - 20f;
			valuePosition.Y -= 3f;
			valuePosition.X -= num2;
			DelegateMethods.v3_1 = hSLVector;
			x = DrawValueBar(sb, scale, hSLVector.X, 0, DelegateMethods.ColorLerp_HSL_H);
			if ((inBar || rightLock == num22) && !notBar)
			{
				rightHover = num22;
				if (Main.mouseLeft && rightLock == num22)
				{
					hSLVector.X = x;
					noSound = true;
				}
			}
			if ((float)Main.mouseX > val3.X + val2.X * 2f / 3f + (float)num4 && (float)Main.mouseX < valuePosition.X + 3.75f && (float)Main.mouseY > valuePosition.Y - 10f && (float)Main.mouseY <= valuePosition.Y + 10f)
			{
				if (rightLock == -1)
				{
					notBar = true;
				}
				rightHover = num22;
			}
			if (rightHover == num22)
			{
				UILinkPointNavigator.Shortcuts.OPTIONS_BUTTON_SPECIALFEATURE = 5;
				Main.menuMode = 252;
			}
			num22++;
			if (DrawRightSide(sb, "", num22, val7, val8, rightScale[num22], (rightScale[num22] - num5) / (num6 - num5)))
			{
				if (rightLock == -1)
				{
					notBar = true;
				}
				rightHover = num22;
			}
			valuePosition.X = val3.X + val2.X - (float)(num4 / 2) - 20f;
			valuePosition.Y -= 3f;
			valuePosition.X -= num2;
			DelegateMethods.v3_1 = hSLVector;
			x = DrawValueBar(sb, scale, hSLVector.Y, 0, DelegateMethods.ColorLerp_HSL_S);
			if ((inBar || rightLock == num22) && !notBar)
			{
				rightHover = num22;
				if (Main.mouseLeft && rightLock == num22)
				{
					hSLVector.Y = x;
					noSound = true;
				}
			}
			if ((float)Main.mouseX > val3.X + val2.X * 2f / 3f + (float)num4 && (float)Main.mouseX < valuePosition.X + 3.75f && (float)Main.mouseY > valuePosition.Y - 10f && (float)Main.mouseY <= valuePosition.Y + 10f)
			{
				if (rightLock == -1)
				{
					notBar = true;
				}
				rightHover = num22;
			}
			if (rightHover == num22)
			{
				UILinkPointNavigator.Shortcuts.OPTIONS_BUTTON_SPECIALFEATURE = 6;
				Main.menuMode = 252;
			}
			num22++;
			if (DrawRightSide(sb, "", num22, val7, val8, rightScale[num22], (rightScale[num22] - num5) / (num6 - num5)))
			{
				if (rightLock == -1)
				{
					notBar = true;
				}
				rightHover = num22;
			}
			valuePosition.X = val3.X + val2.X - (float)(num4 / 2) - 20f;
			valuePosition.Y -= 3f;
			valuePosition.X -= num2;
			DelegateMethods.v3_1 = hSLVector;
			x = DrawValueBar(sb, scale, hSLVector.Z, 0, DelegateMethods.ColorLerp_HSL_L);
			if ((inBar || rightLock == num22) && !notBar)
			{
				rightHover = num22;
				if (Main.mouseLeft && rightLock == num22)
				{
					hSLVector.Z = x;
					noSound = true;
				}
			}
			if ((float)Main.mouseX > val3.X + val2.X * 2f / 3f + (float)num4 && (float)Main.mouseX < valuePosition.X + 3.75f && (float)Main.mouseY > valuePosition.Y - 10f && (float)Main.mouseY <= valuePosition.Y + 10f)
			{
				if (rightLock == -1)
				{
					notBar = true;
				}
				rightHover = num22;
			}
			if (rightHover == num22)
			{
				UILinkPointNavigator.Shortcuts.OPTIONS_BUTTON_SPECIALFEATURE = 7;
				Main.menuMode = 252;
			}
			num22++;
			if (DrawRightSide(sb, "", num22, val7, val8, rightScale[num22], (rightScale[num22] - num5) / (num6 - num5)))
			{
				if (rightLock == -1)
				{
					notBar = true;
				}
				rightHover = num22;
			}
			valuePosition.X = val3.X + val2.X - (float)(num4 / 2) - 20f;
			valuePosition.Y -= 3f;
			valuePosition.X -= num2;
			DelegateMethods.v3_1 = hSLVector;
			float num24 = Main.mouseBorderColorSlider.Alpha;
			x = DrawValueBar(sb, scale, num24, 0, DelegateMethods.ColorLerp_HSL_O);
			if ((inBar || rightLock == num22) && !notBar)
			{
				rightHover = num22;
				if (Main.mouseLeft && rightLock == num22)
				{
					num24 = x;
					noSound = true;
				}
			}
			if ((float)Main.mouseX > val3.X + val2.X * 2f / 3f + (float)num4 && (float)Main.mouseX < valuePosition.X + 3.75f && (float)Main.mouseY > valuePosition.Y - 10f && (float)Main.mouseY <= valuePosition.Y + 10f)
			{
				if (rightLock == -1)
				{
					notBar = true;
				}
				rightHover = num22;
			}
			if (rightHover == num22)
			{
				UILinkPointNavigator.Shortcuts.OPTIONS_BUTTON_SPECIALFEATURE = 8;
				Main.menuMode = 252;
			}
			num22++;
			Main.mouseBorderColorSlider.SetHSL(hSLVector);
			Main.mouseBorderColorSlider.Alpha = num24;
			Main.MouseBorderColor = Main.mouseBorderColorSlider.GetColor();
			val7.X += num23;
			DrawRightSide(sb, "", num22, val7, val8, rightScale[num22], 1f);
			skipRightSlot[num22] = true;
			num22++;
			string txt2 = "";
			switch (LockOnHelper.UseMode)
			{
			case LockOnHelper.LockOnMode.FocusTarget:
				txt2 = Lang.menu[232].Value;
				break;
			case LockOnHelper.LockOnMode.TargetClosest:
				txt2 = Lang.menu[233].Value;
				break;
			case LockOnHelper.LockOnMode.ThreeDS:
				txt2 = Lang.menu[234].Value;
				break;
			}
			if (DrawRightSide(sb, txt2, num22, val7, val8, rightScale[num22] * 0.9f, (rightScale[num22] - num5) / (num6 - num5)))
			{
				rightHover = num22;
				if (flag4)
				{
					LockOnHelper.CycleUseModes();
				}
			}
			num22++;
			if (DrawRightSide(sb, Player.SmartCursorSettings.SmartBlocksEnabled ? Lang.menu[215].Value : Lang.menu[216].Value, num22, val7, val8, rightScale[num22] * 0.9f, (rightScale[num22] - num5) / (num6 - num5)))
			{
				rightHover = num22;
				if (flag4)
				{
					Player.SmartCursorSettings.SmartBlocksEnabled = !Player.SmartCursorSettings.SmartBlocksEnabled;
				}
			}
			num22++;
			if (DrawRightSide(sb, Main.cSmartCursorModeIsToggleAndNotHold ? Lang.menu[121].Value : Lang.menu[122].Value, num22, val7, val8, rightScale[num22], (rightScale[num22] - num5) / (num6 - num5)))
			{
				rightHover = num22;
				if (flag4)
				{
					Main.cSmartCursorModeIsToggleAndNotHold = !Main.cSmartCursorModeIsToggleAndNotHold;
				}
			}
			num22++;
			if (DrawRightSide(sb, Player.SmartCursorSettings.SmartAxeAfterPickaxe ? Lang.menu[214].Value : Lang.menu[213].Value, num22, val7, val8, rightScale[num22] * 0.9f, (rightScale[num22] - num5) / (num6 - num5)))
			{
				rightHover = num22;
				if (flag4)
				{
					Player.SmartCursorSettings.SmartAxeAfterPickaxe = !Player.SmartCursorSettings.SmartAxeAfterPickaxe;
				}
			}
			num22++;
		}
		if (rightHover != -1 && rightLock == -1)
		{
			rightLock = rightHover;
		}
		for (int n = 0; n < num9 + 1; n++)
		{
			UILinkPointNavigator.SetPosition(2900 + n, val5 + val6 * (float)(n + 1));
		}
		Vector2 zero = Vector2.Zero;
		if (flag)
		{
			zero.X = -40f;
		}
		for (int num25 = 0; num25 < num13; num25++)
		{
			if (!skipRightSlot[num25])
			{
				UILinkPointNavigator.SetPosition(2930 + num12, val7 + zero + val8 * (float)(num25 + 1));
				num12++;
			}
		}
		UILinkPointNavigator.Shortcuts.INGAMEOPTIONS_BUTTONS_RIGHT = num12;
		Main.DrawInterface_29_SettingsButton();
		Main.DrawGamepadInstructions();
		Main.mouseText = false;
		Main.instance.GUIBarsDraw();
		Main.instance.DrawMouseOver();
		Main.spriteBatch.End();
		Main.spriteBatch.Begin((SpriteSortMode)0, BlendState.AlphaBlend, Main.SamplerStateForCursor, DepthStencilState.None, RasterizerState.CullCounterClockwise, (Effect)null, Main.UIScaleMatrix);
		Main.DrawCursor(Main.DrawThickCursor());
	}

	public static void MouseOver()
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		if (Main.ingameOptionsWindow)
		{
			if (_GUIHover.Contains(Main.MouseScreen.ToPoint()))
			{
				Main.mouseText = true;
			}
			if (_mouseOverText != null)
			{
				Main.instance.MouseText(_mouseOverText, 0, 0);
			}
			_mouseOverText = null;
		}
	}

	public static bool DrawLeftSide(SpriteBatch sb, string txt, int i, Vector2 anchor, Vector2 offset, float[] scales, float minscale = 0.7f, float maxscale = 0.8f, float scalespeed = 0.01f)
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		bool flag = false;
		if (_leftSideCategoryMapping.TryGetValue(i, out var value))
		{
			flag = category == value;
		}
		Color color = Color.Lerp(Color.Gray, Color.White, (scales[i] - minscale) / (maxscale - minscale));
		if (flag)
		{
			color = Color.Gold;
		}
		Vector2 val = Utils.DrawBorderStringBig(sb, txt, anchor + offset * (float)(1 + i), color, scales[i], 0.5f, 0.5f);
		Rectangle val2 = new Rectangle((int)anchor.X - (int)val.X / 2, (int)anchor.Y + (int)(offset.Y * (float)(1 + i)) - (int)val.Y / 2, (int)val.X, (int)val.Y);
		bool flag2 = val2.Contains(new Point(Main.mouseX, Main.mouseY));
		if (!_canConsumeHover)
		{
			return false;
		}
		if (flag2)
		{
			_canConsumeHover = false;
			return true;
		}
		return false;
	}

	public static bool DrawRightSide(SpriteBatch sb, string txt, int i, Vector2 anchor, Vector2 offset, float scale, float colorScale, Color over = default(Color))
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		Color color = Color.Lerp(Color.Gray, Color.White, colorScale);
		if (over != default(Color))
		{
			color = over;
		}
		Vector2 val = Utils.DrawBorderStringMeasured(sb, txt, anchor + offset * (float)(1 + i), color, scale, 0.5f, 0.5f);
		valuePosition = anchor + offset * (float)(1 + i) + val * new Vector2(0.5f, 0f);
		Rectangle val2 = new Rectangle((int)anchor.X - (int)val.X / 2, (int)anchor.Y + (int)(offset.Y * (float)(1 + i)) - (int)val.Y / 2, (int)val.X, (int)val.Y);
		bool flag = val2.Contains(new Point(Main.mouseX, Main.mouseY));
		if (!_canConsumeHover)
		{
			return false;
		}
		if (flag)
		{
			_canConsumeHover = false;
			return true;
		}
		return false;
	}

	public static Rectangle GetExpectedRectangleForNotification(int itemIndex, Vector2 anchor, Vector2 offset, int areaWidth)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		return Utils.CenteredRectangle(anchor + offset * (float)(1 + itemIndex), new Vector2((float)areaWidth, offset.Y - 4f));
	}

	public static bool DrawValue(SpriteBatch sb, string txt, int i, float scale, Color over = default(Color))
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		Color color = Color.Gray;
		Vector2 val = FontAssets.MouseText.Value.MeasureString(txt) * scale;
		Rectangle val2 = new Rectangle((int)valuePosition.X, (int)valuePosition.Y - (int)val.Y / 2, (int)val.X, (int)val.Y);
		bool flag = val2.Contains(new Point(Main.mouseX, Main.mouseY));
		if (flag)
		{
			color = Color.White;
		}
		if (over != default(Color))
		{
			color = over;
		}
		Utils.DrawBorderString(sb, txt, valuePosition, color, scale, 0f, 0.5f);
		valuePosition.X += val.X;
		if (!_canConsumeHover)
		{
			return false;
		}
		if (flag)
		{
			_canConsumeHover = false;
			return true;
		}
		return false;
	}

	public static float DrawValueBar(SpriteBatch sb, float scale, float perc, int lockState = 0, Utils.ColorLerpMethod colorMethod = null)
	{
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_013a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0160: Unknown result type (might be due to invalid IL or missing references)
		//IL_0161: Unknown result type (might be due to invalid IL or missing references)
		//IL_018c: Unknown result type (might be due to invalid IL or missing references)
		//IL_019b: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_021c: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0205: Unknown result type (might be due to invalid IL or missing references)
		//IL_020d: Unknown result type (might be due to invalid IL or missing references)
		if (colorMethod == null)
		{
			colorMethod = Utils.ColorLerp_BlackToWhite;
		}
		Texture2D value = TextureAssets.ColorBar.Value;
		Vector2 val = new Vector2((float)value.Width, (float)value.Height) * scale;
		valuePosition.X -= (int)val.X;
		Rectangle val2 = new Rectangle((int)valuePosition.X, (int)valuePosition.Y - (int)val.Y / 2, (int)val.X, (int)val.Y);
		Rectangle val3 = val2;
		sb.Draw(value, val2, Color.White);
		int num = 167;
		float num2 = (float)val2.X + 5f * scale;
		float num3 = (float)val2.Y + 4f * scale;
		for (float num4 = 0f; num4 < (float)num; num4++)
		{
			float percent = num4 / (float)num;
			sb.Draw(TextureAssets.ColorBlip.Value, new Vector2(num2 + num4 * scale, num3), (Rectangle?)null, colorMethod(percent), 0f, Vector2.Zero, scale, (SpriteEffects)0, 0f);
		}
		val2.Inflate((int)(-5f * scale), 0);
		bool flag = val2.Contains(new Point(Main.mouseX, Main.mouseY));
		if (lockState == 2)
		{
			flag = false;
		}
		if (flag || lockState == 1)
		{
			sb.Draw(TextureAssets.ColorHighlight.Value, val3, Main.OurFavoriteColor);
		}
		sb.Draw(TextureAssets.ColorSlider.Value, new Vector2(num2 + 167f * scale * perc, num3 + 4f * scale), (Rectangle?)null, Color.White, 0f, new Vector2(0.5f * (float)TextureAssets.ColorSlider.Width(), 0.5f * (float)TextureAssets.ColorSlider.Height()), scale, (SpriteEffects)0, 0f);
		if (Main.mouseX >= val2.X && Main.mouseX <= val2.X + val2.Width)
		{
			inBar = flag;
			return (float)(Main.mouseX - val2.X) / (float)val2.Width;
		}
		inBar = false;
		if (val2.X >= Main.mouseX)
		{
			return 0f;
		}
		return 1f;
	}

	static IngameOptions()
	{
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
	}
}
