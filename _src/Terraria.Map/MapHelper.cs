using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using Ionic.Zlib;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.IO;
using Terraria.Social;
using Terraria.Testing;
using Terraria.Utilities;

namespace Terraria.Map;

public static class MapHelper
{
	private struct OldMapHelper
	{
		public byte misc;

		public byte misc2;

		public bool active()
		{
			if ((misc & 1) == 1)
			{
				return true;
			}
			return false;
		}

		public bool water()
		{
			if ((misc & 2) == 2)
			{
				return true;
			}
			return false;
		}

		public bool lava()
		{
			if ((misc & 4) == 4)
			{
				return true;
			}
			return false;
		}

		public bool honey()
		{
			if ((misc2 & 0x40) == 64)
			{
				return true;
			}
			return false;
		}

		public bool changed()
		{
			if ((misc & 8) == 8)
			{
				return true;
			}
			return false;
		}

		public bool wall()
		{
			if ((misc & 0x10) == 16)
			{
				return true;
			}
			return false;
		}

		public byte option()
		{
			byte b = 0;
			if ((misc & 0x20) == 32)
			{
				b++;
			}
			if ((misc & 0x40) == 64)
			{
				b += 2;
			}
			if ((misc & 0x80) == 128)
			{
				b += 4;
			}
			if ((misc2 & 1) == 1)
			{
				b += 8;
			}
			return b;
		}

		public byte color()
		{
			return (byte)((misc2 & 0x1E) >> 1);
		}
	}

	public const int drawLoopMilliseconds = 5;

	private const int HeaderEmpty = 0;

	private const int HeaderTile = 1;

	private const int HeaderWall = 2;

	private const int HeaderWater = 3;

	private const int HeaderLava = 4;

	private const int HeaderHoney = 5;

	private const int HeaderHeavenAndHell = 6;

	private const int HeaderBackground = 7;

	private const int Header2_ReadHeader3Bit = 1;

	private const int Header2Color1 = 2;

	private const int Header2Color2 = 4;

	private const int Header2Color3 = 8;

	private const int Header2Color4 = 16;

	private const int Header2Color5 = 32;

	private const int Header2ShimmerBit = 64;

	private const int Header2_UnusedBit8 = 128;

	private const int Header3_ReservedForHeader4Bit = 1;

	private const int Header3_UnusudBit2 = 2;

	private const int Header3_UnusudBit3 = 4;

	private const int Header3_UnusudBit4 = 8;

	private const int Header3_UnusudBit5 = 16;

	private const int Header3_UnusudBit6 = 32;

	private const int Header3_UnusudBit7 = 64;

	private const int Header3_UnusudBit8 = 128;

	public const int maxTileOptions = 13;

	public const int maxWallOptions = 2;

	private const int maxLiquidTypes = 4;

	private const int maxSkyGradients = 256;

	private const int maxDirtGradients = 256;

	private const int maxRockGradients = 256;

	private static object IOLock = new object();

	public static int[] tileOptionCounts;

	public static int[] wallOptionCounts;

	public static ushort[] tileLookup;

	public static ushort[] wallLookup;

	private static ushort tilePosition;

	private static ushort wallPosition;

	private static ushort liquidPosition;

	private static ushort skyPosition;

	private static ushort dirtPosition;

	private static ushort rockPosition;

	private static ushort hellPosition;

	private static Color[] colorLookup;

	private static ushort[] snowTypes;

	private static ushort wallRangeStart;

	private static ushort wallRangeEnd;

	public static Rectangle sceneArea;

	public static float sceneSnowiness;

	public static bool noStatusText = false;

	public const int MapChunkSize = 64;

	private static ZlibCodec zlibDecompress = new ZlibCodec((CompressionMode)1);

	public static void Initialize()
	{
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_0105: Unknown result type (might be due to invalid IL or missing references)
		//IL_010a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0123: Unknown result type (might be due to invalid IL or missing references)
		//IL_0128: Unknown result type (might be due to invalid IL or missing references)
		//IL_013e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0143: Unknown result type (might be due to invalid IL or missing references)
		//IL_0158: Unknown result type (might be due to invalid IL or missing references)
		//IL_015d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0176: Unknown result type (might be due to invalid IL or missing references)
		//IL_017b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0191: Unknown result type (might be due to invalid IL or missing references)
		//IL_0196: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_020c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0211: Unknown result type (might be due to invalid IL or missing references)
		//IL_0226: Unknown result type (might be due to invalid IL or missing references)
		//IL_022b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0244: Unknown result type (might be due to invalid IL or missing references)
		//IL_0249: Unknown result type (might be due to invalid IL or missing references)
		//IL_0262: Unknown result type (might be due to invalid IL or missing references)
		//IL_0267: Unknown result type (might be due to invalid IL or missing references)
		//IL_027d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0282: Unknown result type (might be due to invalid IL or missing references)
		//IL_0298: Unknown result type (might be due to invalid IL or missing references)
		//IL_029d: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0309: Unknown result type (might be due to invalid IL or missing references)
		//IL_030b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0318: Unknown result type (might be due to invalid IL or missing references)
		//IL_031a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0327: Unknown result type (might be due to invalid IL or missing references)
		//IL_0329: Unknown result type (might be due to invalid IL or missing references)
		//IL_0336: Unknown result type (might be due to invalid IL or missing references)
		//IL_0338: Unknown result type (might be due to invalid IL or missing references)
		//IL_0345: Unknown result type (might be due to invalid IL or missing references)
		//IL_0347: Unknown result type (might be due to invalid IL or missing references)
		//IL_0354: Unknown result type (might be due to invalid IL or missing references)
		//IL_0356: Unknown result type (might be due to invalid IL or missing references)
		//IL_0363: Unknown result type (might be due to invalid IL or missing references)
		//IL_0365: Unknown result type (might be due to invalid IL or missing references)
		//IL_0372: Unknown result type (might be due to invalid IL or missing references)
		//IL_0374: Unknown result type (might be due to invalid IL or missing references)
		//IL_0381: Unknown result type (might be due to invalid IL or missing references)
		//IL_0383: Unknown result type (might be due to invalid IL or missing references)
		//IL_0390: Unknown result type (might be due to invalid IL or missing references)
		//IL_0392: Unknown result type (might be due to invalid IL or missing references)
		//IL_039f: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0404: Unknown result type (might be due to invalid IL or missing references)
		//IL_041a: Unknown result type (might be due to invalid IL or missing references)
		//IL_041f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0429: Unknown result type (might be due to invalid IL or missing references)
		//IL_042b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0438: Unknown result type (might be due to invalid IL or missing references)
		//IL_043a: Unknown result type (might be due to invalid IL or missing references)
		//IL_044d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0452: Unknown result type (might be due to invalid IL or missing references)
		//IL_0468: Unknown result type (might be due to invalid IL or missing references)
		//IL_0471: Unknown result type (might be due to invalid IL or missing references)
		//IL_0473: Unknown result type (might be due to invalid IL or missing references)
		//IL_047d: Unknown result type (might be due to invalid IL or missing references)
		//IL_047f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0489: Unknown result type (might be due to invalid IL or missing references)
		//IL_048b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0498: Unknown result type (might be due to invalid IL or missing references)
		//IL_049a: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0501: Unknown result type (might be due to invalid IL or missing references)
		//IL_0503: Unknown result type (might be due to invalid IL or missing references)
		//IL_0510: Unknown result type (might be due to invalid IL or missing references)
		//IL_0512: Unknown result type (might be due to invalid IL or missing references)
		//IL_051f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0521: Unknown result type (might be due to invalid IL or missing references)
		//IL_052e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0530: Unknown result type (might be due to invalid IL or missing references)
		//IL_053d: Unknown result type (might be due to invalid IL or missing references)
		//IL_053f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0555: Unknown result type (might be due to invalid IL or missing references)
		//IL_055a: Unknown result type (might be due to invalid IL or missing references)
		//IL_056c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0571: Unknown result type (might be due to invalid IL or missing references)
		//IL_0587: Unknown result type (might be due to invalid IL or missing references)
		//IL_058c: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_05aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0604: Unknown result type (might be due to invalid IL or missing references)
		//IL_061a: Unknown result type (might be due to invalid IL or missing references)
		//IL_061f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0632: Unknown result type (might be due to invalid IL or missing references)
		//IL_0637: Unknown result type (might be due to invalid IL or missing references)
		//IL_0649: Unknown result type (might be due to invalid IL or missing references)
		//IL_064e: Unknown result type (might be due to invalid IL or missing references)
		//IL_065e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0667: Unknown result type (might be due to invalid IL or missing references)
		//IL_0669: Unknown result type (might be due to invalid IL or missing references)
		//IL_0673: Unknown result type (might be due to invalid IL or missing references)
		//IL_0675: Unknown result type (might be due to invalid IL or missing references)
		//IL_0682: Unknown result type (might be due to invalid IL or missing references)
		//IL_0684: Unknown result type (might be due to invalid IL or missing references)
		//IL_0691: Unknown result type (might be due to invalid IL or missing references)
		//IL_0693: Unknown result type (might be due to invalid IL or missing references)
		//IL_06a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_06a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_06b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_06be: Unknown result type (might be due to invalid IL or missing references)
		//IL_06c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_06cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_06d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_06db: Unknown result type (might be due to invalid IL or missing references)
		//IL_06e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_0700: Unknown result type (might be due to invalid IL or missing references)
		//IL_070a: Unknown result type (might be due to invalid IL or missing references)
		//IL_070c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0716: Unknown result type (might be due to invalid IL or missing references)
		//IL_0718: Unknown result type (might be due to invalid IL or missing references)
		//IL_0725: Unknown result type (might be due to invalid IL or missing references)
		//IL_0727: Unknown result type (might be due to invalid IL or missing references)
		//IL_0734: Unknown result type (might be due to invalid IL or missing references)
		//IL_0736: Unknown result type (might be due to invalid IL or missing references)
		//IL_0746: Unknown result type (might be due to invalid IL or missing references)
		//IL_0750: Unknown result type (might be due to invalid IL or missing references)
		//IL_0752: Unknown result type (might be due to invalid IL or missing references)
		//IL_075f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0761: Unknown result type (might be due to invalid IL or missing references)
		//IL_077a: Unknown result type (might be due to invalid IL or missing references)
		//IL_077f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0792: Unknown result type (might be due to invalid IL or missing references)
		//IL_0797: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_07b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_07c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_07da: Unknown result type (might be due to invalid IL or missing references)
		//IL_07df: Unknown result type (might be due to invalid IL or missing references)
		//IL_07f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_07f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_080a: Unknown result type (might be due to invalid IL or missing references)
		//IL_080f: Unknown result type (might be due to invalid IL or missing references)
		//IL_081f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0824: Unknown result type (might be due to invalid IL or missing references)
		//IL_0837: Unknown result type (might be due to invalid IL or missing references)
		//IL_083c: Unknown result type (might be due to invalid IL or missing references)
		//IL_084f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0854: Unknown result type (might be due to invalid IL or missing references)
		//IL_0864: Unknown result type (might be due to invalid IL or missing references)
		//IL_0869: Unknown result type (might be due to invalid IL or missing references)
		//IL_087c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0881: Unknown result type (might be due to invalid IL or missing references)
		//IL_0894: Unknown result type (might be due to invalid IL or missing references)
		//IL_0899: Unknown result type (might be due to invalid IL or missing references)
		//IL_08a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_08ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_08c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_08c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_08d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_08de: Unknown result type (might be due to invalid IL or missing references)
		//IL_08f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_08f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_090c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0919: Unknown result type (might be due to invalid IL or missing references)
		//IL_091b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0928: Unknown result type (might be due to invalid IL or missing references)
		//IL_092a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0937: Unknown result type (might be due to invalid IL or missing references)
		//IL_0939: Unknown result type (might be due to invalid IL or missing references)
		//IL_0955: Unknown result type (might be due to invalid IL or missing references)
		//IL_095a: Unknown result type (might be due to invalid IL or missing references)
		//IL_096d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0972: Unknown result type (might be due to invalid IL or missing references)
		//IL_098b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0990: Unknown result type (might be due to invalid IL or missing references)
		//IL_09a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_09a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_09b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_09ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_09d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_09d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_09e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_09ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_09fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a04: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a06: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a10: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a12: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a1f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a21: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a2e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a38: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a3a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a44: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a46: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a5c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a61: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a77: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a7c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a8e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a93: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aa6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aab: Unknown result type (might be due to invalid IL or missing references)
		//IL_0abe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ac3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ad9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ade: Unknown result type (might be due to invalid IL or missing references)
		//IL_0af3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0af8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b0d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b12: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b28: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b2d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b40: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b45: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b58: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b5d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b70: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b75: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b8b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b90: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ba6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bab: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bc4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bc9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bdc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0be1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bf7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c01: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c03: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c0d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c0f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c1c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c1e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c37: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c3c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c49: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c53: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c55: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c62: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c64: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c71: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c73: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c86: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c8b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ca1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ca6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cb9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cbe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cce: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cd8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cda: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ce4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ce6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cf3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cf5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d08: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d12: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d14: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d1e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d20: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d2d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d2f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d3f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d49: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d4b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d58: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d5a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d70: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d75: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d8b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d90: Unknown result type (might be due to invalid IL or missing references)
		//IL_0da6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dab: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dc1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dc6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dd6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ddb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0df1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dfb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dfd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e07: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e09: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e22: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e27: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e37: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e3c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e4c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e51: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e61: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e66: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e79: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e7e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e94: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e99: Unknown result type (might be due to invalid IL or missing references)
		//IL_0eaf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0eb4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0eca: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ed7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ed9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ee6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ee8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f04: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f09: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f22: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f27: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f43: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f48: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f64: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f69: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f7c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f81: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f91: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f9e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fa0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fad: Unknown result type (might be due to invalid IL or missing references)
		//IL_0faf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fc2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fc7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fe0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fe5: Unknown result type (might be due to invalid IL or missing references)
		//IL_1001: Unknown result type (might be due to invalid IL or missing references)
		//IL_1006: Unknown result type (might be due to invalid IL or missing references)
		//IL_1022: Unknown result type (might be due to invalid IL or missing references)
		//IL_1027: Unknown result type (might be due to invalid IL or missing references)
		//IL_1043: Unknown result type (might be due to invalid IL or missing references)
		//IL_1048: Unknown result type (might be due to invalid IL or missing references)
		//IL_1064: Unknown result type (might be due to invalid IL or missing references)
		//IL_1069: Unknown result type (might be due to invalid IL or missing references)
		//IL_107c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1081: Unknown result type (might be due to invalid IL or missing references)
		//IL_1097: Unknown result type (might be due to invalid IL or missing references)
		//IL_109c: Unknown result type (might be due to invalid IL or missing references)
		//IL_10ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_10b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_10bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_10c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_10ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_10dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_10e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_10f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_1102: Unknown result type (might be due to invalid IL or missing references)
		//IL_1104: Unknown result type (might be due to invalid IL or missing references)
		//IL_1111: Unknown result type (might be due to invalid IL or missing references)
		//IL_1113: Unknown result type (might be due to invalid IL or missing references)
		//IL_1129: Unknown result type (might be due to invalid IL or missing references)
		//IL_1136: Unknown result type (might be due to invalid IL or missing references)
		//IL_1138: Unknown result type (might be due to invalid IL or missing references)
		//IL_1145: Unknown result type (might be due to invalid IL or missing references)
		//IL_1147: Unknown result type (might be due to invalid IL or missing references)
		//IL_115d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1162: Unknown result type (might be due to invalid IL or missing references)
		//IL_1178: Unknown result type (might be due to invalid IL or missing references)
		//IL_117d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1193: Unknown result type (might be due to invalid IL or missing references)
		//IL_1198: Unknown result type (might be due to invalid IL or missing references)
		//IL_11ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_11b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_11c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_11ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_11e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_11e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_11fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_1202: Unknown result type (might be due to invalid IL or missing references)
		//IL_1217: Unknown result type (might be due to invalid IL or missing references)
		//IL_121c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1231: Unknown result type (might be due to invalid IL or missing references)
		//IL_1236: Unknown result type (might be due to invalid IL or missing references)
		//IL_124e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1253: Unknown result type (might be due to invalid IL or missing references)
		//IL_126b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1270: Unknown result type (might be due to invalid IL or missing references)
		//IL_1285: Unknown result type (might be due to invalid IL or missing references)
		//IL_128a: Unknown result type (might be due to invalid IL or missing references)
		//IL_129f: Unknown result type (might be due to invalid IL or missing references)
		//IL_12a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_12bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_12c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_12db: Unknown result type (might be due to invalid IL or missing references)
		//IL_12e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_12f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_12fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_1314: Unknown result type (might be due to invalid IL or missing references)
		//IL_1319: Unknown result type (might be due to invalid IL or missing references)
		//IL_1332: Unknown result type (might be due to invalid IL or missing references)
		//IL_1337: Unknown result type (might be due to invalid IL or missing references)
		//IL_134d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1352: Unknown result type (might be due to invalid IL or missing references)
		//IL_136b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1370: Unknown result type (might be due to invalid IL or missing references)
		//IL_1389: Unknown result type (might be due to invalid IL or missing references)
		//IL_138e: Unknown result type (might be due to invalid IL or missing references)
		//IL_13a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_13a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_13c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_13c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_13e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_13e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_13fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_1400: Unknown result type (might be due to invalid IL or missing references)
		//IL_1416: Unknown result type (might be due to invalid IL or missing references)
		//IL_141b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1431: Unknown result type (might be due to invalid IL or missing references)
		//IL_1436: Unknown result type (might be due to invalid IL or missing references)
		//IL_144c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1451: Unknown result type (might be due to invalid IL or missing references)
		//IL_1467: Unknown result type (might be due to invalid IL or missing references)
		//IL_146c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1481: Unknown result type (might be due to invalid IL or missing references)
		//IL_1486: Unknown result type (might be due to invalid IL or missing references)
		//IL_149b: Unknown result type (might be due to invalid IL or missing references)
		//IL_14a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_14b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_14bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_14d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_14d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_14f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_14f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_1511: Unknown result type (might be due to invalid IL or missing references)
		//IL_1516: Unknown result type (might be due to invalid IL or missing references)
		//IL_1545: Unknown result type (might be due to invalid IL or missing references)
		//IL_154a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1579: Unknown result type (might be due to invalid IL or missing references)
		//IL_157e: Unknown result type (might be due to invalid IL or missing references)
		//IL_15a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_15a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_15bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_15c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_15d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_15d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_15ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_15f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_1604: Unknown result type (might be due to invalid IL or missing references)
		//IL_1609: Unknown result type (might be due to invalid IL or missing references)
		//IL_161e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1623: Unknown result type (might be due to invalid IL or missing references)
		//IL_1636: Unknown result type (might be due to invalid IL or missing references)
		//IL_163b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1650: Unknown result type (might be due to invalid IL or missing references)
		//IL_1655: Unknown result type (might be due to invalid IL or missing references)
		//IL_166d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1672: Unknown result type (might be due to invalid IL or missing references)
		//IL_1689: Unknown result type (might be due to invalid IL or missing references)
		//IL_168e: Unknown result type (might be due to invalid IL or missing references)
		//IL_16a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_16aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_16c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_16cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_16e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_16e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_16ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_1704: Unknown result type (might be due to invalid IL or missing references)
		//IL_171a: Unknown result type (might be due to invalid IL or missing references)
		//IL_171f: Unknown result type (might be due to invalid IL or missing references)
		//IL_173b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1740: Unknown result type (might be due to invalid IL or missing references)
		//IL_1759: Unknown result type (might be due to invalid IL or missing references)
		//IL_175e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1771: Unknown result type (might be due to invalid IL or missing references)
		//IL_1776: Unknown result type (might be due to invalid IL or missing references)
		//IL_178c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1791: Unknown result type (might be due to invalid IL or missing references)
		//IL_17a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_17ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_17c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_17c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_17e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_17e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_1801: Unknown result type (might be due to invalid IL or missing references)
		//IL_1806: Unknown result type (might be due to invalid IL or missing references)
		//IL_181c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1821: Unknown result type (might be due to invalid IL or missing references)
		//IL_1834: Unknown result type (might be due to invalid IL or missing references)
		//IL_1839: Unknown result type (might be due to invalid IL or missing references)
		//IL_1852: Unknown result type (might be due to invalid IL or missing references)
		//IL_1857: Unknown result type (might be due to invalid IL or missing references)
		//IL_186a: Unknown result type (might be due to invalid IL or missing references)
		//IL_186f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1888: Unknown result type (might be due to invalid IL or missing references)
		//IL_188d: Unknown result type (might be due to invalid IL or missing references)
		//IL_18a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_18a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_18bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_18c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_18d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_18db: Unknown result type (might be due to invalid IL or missing references)
		//IL_18f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_18f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_190c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1911: Unknown result type (might be due to invalid IL or missing references)
		//IL_192a: Unknown result type (might be due to invalid IL or missing references)
		//IL_192f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1945: Unknown result type (might be due to invalid IL or missing references)
		//IL_194a: Unknown result type (might be due to invalid IL or missing references)
		//IL_195f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1964: Unknown result type (might be due to invalid IL or missing references)
		//IL_197d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1982: Unknown result type (might be due to invalid IL or missing references)
		//IL_199e: Unknown result type (might be due to invalid IL or missing references)
		//IL_19a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_19b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_19be: Unknown result type (might be due to invalid IL or missing references)
		//IL_19d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_19d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_19ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_19f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a0d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a12: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a27: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a2c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a45: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a4a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a66: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a6b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a87: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a8c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1aa8: Unknown result type (might be due to invalid IL or missing references)
		//IL_1aad: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ac3: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ac8: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ae1: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ae6: Unknown result type (might be due to invalid IL or missing references)
		//IL_1af9: Unknown result type (might be due to invalid IL or missing references)
		//IL_1afe: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b17: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b1c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b35: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b3a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b4d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b52: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b6b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b70: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b86: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b8b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ba4: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ba9: Unknown result type (might be due to invalid IL or missing references)
		//IL_1bbf: Unknown result type (might be due to invalid IL or missing references)
		//IL_1bc4: Unknown result type (might be due to invalid IL or missing references)
		//IL_1bda: Unknown result type (might be due to invalid IL or missing references)
		//IL_1bdf: Unknown result type (might be due to invalid IL or missing references)
		//IL_1bf5: Unknown result type (might be due to invalid IL or missing references)
		//IL_1bfa: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c0d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c12: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c28: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c2d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c46: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c4b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c64: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c69: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c7f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c84: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c9a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c9f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1cb5: Unknown result type (might be due to invalid IL or missing references)
		//IL_1cba: Unknown result type (might be due to invalid IL or missing references)
		//IL_1cd6: Unknown result type (might be due to invalid IL or missing references)
		//IL_1cdb: Unknown result type (might be due to invalid IL or missing references)
		//IL_1cf4: Unknown result type (might be due to invalid IL or missing references)
		//IL_1cf9: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d0f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d14: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d27: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d34: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d36: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d43: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d45: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d52: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d54: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d61: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d63: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d70: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d72: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d7f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d81: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d8e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d90: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d9d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d9f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1dac: Unknown result type (might be due to invalid IL or missing references)
		//IL_1dae: Unknown result type (might be due to invalid IL or missing references)
		//IL_1dbb: Unknown result type (might be due to invalid IL or missing references)
		//IL_1dbd: Unknown result type (might be due to invalid IL or missing references)
		//IL_1dca: Unknown result type (might be due to invalid IL or missing references)
		//IL_1dcc: Unknown result type (might be due to invalid IL or missing references)
		//IL_1dd9: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ddb: Unknown result type (might be due to invalid IL or missing references)
		//IL_1de8: Unknown result type (might be due to invalid IL or missing references)
		//IL_1dea: Unknown result type (might be due to invalid IL or missing references)
		//IL_1df7: Unknown result type (might be due to invalid IL or missing references)
		//IL_1df9: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e06: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e08: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e15: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e17: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e24: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e26: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e33: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e35: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e42: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e44: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e51: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e53: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e60: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e62: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e6f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e71: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e7e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e80: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e8d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e8f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e9c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e9e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1eab: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ead: Unknown result type (might be due to invalid IL or missing references)
		//IL_1eba: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ebc: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ec9: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ecb: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ed8: Unknown result type (might be due to invalid IL or missing references)
		//IL_1eda: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ee7: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ee9: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ef6: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ef8: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f05: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f07: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f14: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f16: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f23: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f25: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f32: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f34: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f41: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f43: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f50: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f52: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f5f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f61: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f6e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f70: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f7d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f7f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f8c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f8e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f9b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f9d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1faa: Unknown result type (might be due to invalid IL or missing references)
		//IL_1fac: Unknown result type (might be due to invalid IL or missing references)
		//IL_1fb9: Unknown result type (might be due to invalid IL or missing references)
		//IL_1fbb: Unknown result type (might be due to invalid IL or missing references)
		//IL_1fc8: Unknown result type (might be due to invalid IL or missing references)
		//IL_1fca: Unknown result type (might be due to invalid IL or missing references)
		//IL_1fd7: Unknown result type (might be due to invalid IL or missing references)
		//IL_1fd9: Unknown result type (might be due to invalid IL or missing references)
		//IL_1fe6: Unknown result type (might be due to invalid IL or missing references)
		//IL_1fe8: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ff5: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ff7: Unknown result type (might be due to invalid IL or missing references)
		//IL_2004: Unknown result type (might be due to invalid IL or missing references)
		//IL_2006: Unknown result type (might be due to invalid IL or missing references)
		//IL_2013: Unknown result type (might be due to invalid IL or missing references)
		//IL_2015: Unknown result type (might be due to invalid IL or missing references)
		//IL_2022: Unknown result type (might be due to invalid IL or missing references)
		//IL_2024: Unknown result type (might be due to invalid IL or missing references)
		//IL_2031: Unknown result type (might be due to invalid IL or missing references)
		//IL_2033: Unknown result type (might be due to invalid IL or missing references)
		//IL_2040: Unknown result type (might be due to invalid IL or missing references)
		//IL_2042: Unknown result type (might be due to invalid IL or missing references)
		//IL_204f: Unknown result type (might be due to invalid IL or missing references)
		//IL_2051: Unknown result type (might be due to invalid IL or missing references)
		//IL_205e: Unknown result type (might be due to invalid IL or missing references)
		//IL_2060: Unknown result type (might be due to invalid IL or missing references)
		//IL_206d: Unknown result type (might be due to invalid IL or missing references)
		//IL_206f: Unknown result type (might be due to invalid IL or missing references)
		//IL_207c: Unknown result type (might be due to invalid IL or missing references)
		//IL_207e: Unknown result type (might be due to invalid IL or missing references)
		//IL_208b: Unknown result type (might be due to invalid IL or missing references)
		//IL_208d: Unknown result type (might be due to invalid IL or missing references)
		//IL_209a: Unknown result type (might be due to invalid IL or missing references)
		//IL_209c: Unknown result type (might be due to invalid IL or missing references)
		//IL_20a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_20ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_20b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_20ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_20c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_20c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_20d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_20d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_20e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_20e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_20f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_20f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_2103: Unknown result type (might be due to invalid IL or missing references)
		//IL_2105: Unknown result type (might be due to invalid IL or missing references)
		//IL_2112: Unknown result type (might be due to invalid IL or missing references)
		//IL_2114: Unknown result type (might be due to invalid IL or missing references)
		//IL_2121: Unknown result type (might be due to invalid IL or missing references)
		//IL_2123: Unknown result type (might be due to invalid IL or missing references)
		//IL_2130: Unknown result type (might be due to invalid IL or missing references)
		//IL_2132: Unknown result type (might be due to invalid IL or missing references)
		//IL_213f: Unknown result type (might be due to invalid IL or missing references)
		//IL_2141: Unknown result type (might be due to invalid IL or missing references)
		//IL_214e: Unknown result type (might be due to invalid IL or missing references)
		//IL_2150: Unknown result type (might be due to invalid IL or missing references)
		//IL_215d: Unknown result type (might be due to invalid IL or missing references)
		//IL_215f: Unknown result type (might be due to invalid IL or missing references)
		//IL_216c: Unknown result type (might be due to invalid IL or missing references)
		//IL_216e: Unknown result type (might be due to invalid IL or missing references)
		//IL_217b: Unknown result type (might be due to invalid IL or missing references)
		//IL_217d: Unknown result type (might be due to invalid IL or missing references)
		//IL_218a: Unknown result type (might be due to invalid IL or missing references)
		//IL_218c: Unknown result type (might be due to invalid IL or missing references)
		//IL_2199: Unknown result type (might be due to invalid IL or missing references)
		//IL_219b: Unknown result type (might be due to invalid IL or missing references)
		//IL_21a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_21aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_21b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_21b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_21c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_21c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_21d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_21d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_21e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_21e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_21f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_21f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_2202: Unknown result type (might be due to invalid IL or missing references)
		//IL_2204: Unknown result type (might be due to invalid IL or missing references)
		//IL_2211: Unknown result type (might be due to invalid IL or missing references)
		//IL_2213: Unknown result type (might be due to invalid IL or missing references)
		//IL_2220: Unknown result type (might be due to invalid IL or missing references)
		//IL_2222: Unknown result type (might be due to invalid IL or missing references)
		//IL_222f: Unknown result type (might be due to invalid IL or missing references)
		//IL_2231: Unknown result type (might be due to invalid IL or missing references)
		//IL_2247: Unknown result type (might be due to invalid IL or missing references)
		//IL_224c: Unknown result type (might be due to invalid IL or missing references)
		//IL_2265: Unknown result type (might be due to invalid IL or missing references)
		//IL_226a: Unknown result type (might be due to invalid IL or missing references)
		//IL_227c: Unknown result type (might be due to invalid IL or missing references)
		//IL_2281: Unknown result type (might be due to invalid IL or missing references)
		//IL_2297: Unknown result type (might be due to invalid IL or missing references)
		//IL_229c: Unknown result type (might be due to invalid IL or missing references)
		//IL_22af: Unknown result type (might be due to invalid IL or missing references)
		//IL_22b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_22c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_22cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_22df: Unknown result type (might be due to invalid IL or missing references)
		//IL_22e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_22fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_22ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_2312: Unknown result type (might be due to invalid IL or missing references)
		//IL_2317: Unknown result type (might be due to invalid IL or missing references)
		//IL_232d: Unknown result type (might be due to invalid IL or missing references)
		//IL_2332: Unknown result type (might be due to invalid IL or missing references)
		//IL_2345: Unknown result type (might be due to invalid IL or missing references)
		//IL_234a: Unknown result type (might be due to invalid IL or missing references)
		//IL_235d: Unknown result type (might be due to invalid IL or missing references)
		//IL_2362: Unknown result type (might be due to invalid IL or missing references)
		//IL_2375: Unknown result type (might be due to invalid IL or missing references)
		//IL_237a: Unknown result type (might be due to invalid IL or missing references)
		//IL_2396: Unknown result type (might be due to invalid IL or missing references)
		//IL_239b: Unknown result type (might be due to invalid IL or missing references)
		//IL_23b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_23b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_23cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_23d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_23eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_23f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_2409: Unknown result type (might be due to invalid IL or missing references)
		//IL_240e: Unknown result type (might be due to invalid IL or missing references)
		//IL_2427: Unknown result type (might be due to invalid IL or missing references)
		//IL_242c: Unknown result type (might be due to invalid IL or missing references)
		//IL_2448: Unknown result type (might be due to invalid IL or missing references)
		//IL_244d: Unknown result type (might be due to invalid IL or missing references)
		//IL_2460: Unknown result type (might be due to invalid IL or missing references)
		//IL_2465: Unknown result type (might be due to invalid IL or missing references)
		//IL_247b: Unknown result type (might be due to invalid IL or missing references)
		//IL_2480: Unknown result type (might be due to invalid IL or missing references)
		//IL_2499: Unknown result type (might be due to invalid IL or missing references)
		//IL_249e: Unknown result type (might be due to invalid IL or missing references)
		//IL_24b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_24b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_24cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_24d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_24e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_24ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_24ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_24f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_24fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_2507: Unknown result type (might be due to invalid IL or missing references)
		//IL_2509: Unknown result type (might be due to invalid IL or missing references)
		//IL_2513: Unknown result type (might be due to invalid IL or missing references)
		//IL_2515: Unknown result type (might be due to invalid IL or missing references)
		//IL_251f: Unknown result type (might be due to invalid IL or missing references)
		//IL_2521: Unknown result type (might be due to invalid IL or missing references)
		//IL_252e: Unknown result type (might be due to invalid IL or missing references)
		//IL_2530: Unknown result type (might be due to invalid IL or missing references)
		//IL_253d: Unknown result type (might be due to invalid IL or missing references)
		//IL_2547: Unknown result type (might be due to invalid IL or missing references)
		//IL_2549: Unknown result type (might be due to invalid IL or missing references)
		//IL_2553: Unknown result type (might be due to invalid IL or missing references)
		//IL_2555: Unknown result type (might be due to invalid IL or missing references)
		//IL_2562: Unknown result type (might be due to invalid IL or missing references)
		//IL_2564: Unknown result type (might be due to invalid IL or missing references)
		//IL_2571: Unknown result type (might be due to invalid IL or missing references)
		//IL_2573: Unknown result type (might be due to invalid IL or missing references)
		//IL_2586: Unknown result type (might be due to invalid IL or missing references)
		//IL_2590: Unknown result type (might be due to invalid IL or missing references)
		//IL_2592: Unknown result type (might be due to invalid IL or missing references)
		//IL_259f: Unknown result type (might be due to invalid IL or missing references)
		//IL_25a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_25ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_25b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_25c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_25c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_25dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_25e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_25ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_25f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_25fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_2600: Unknown result type (might be due to invalid IL or missing references)
		//IL_260a: Unknown result type (might be due to invalid IL or missing references)
		//IL_260c: Unknown result type (might be due to invalid IL or missing references)
		//IL_2616: Unknown result type (might be due to invalid IL or missing references)
		//IL_2618: Unknown result type (might be due to invalid IL or missing references)
		//IL_2625: Unknown result type (might be due to invalid IL or missing references)
		//IL_2627: Unknown result type (might be due to invalid IL or missing references)
		//IL_2631: Unknown result type (might be due to invalid IL or missing references)
		//IL_2633: Unknown result type (might be due to invalid IL or missing references)
		//IL_263d: Unknown result type (might be due to invalid IL or missing references)
		//IL_263f: Unknown result type (might be due to invalid IL or missing references)
		//IL_2649: Unknown result type (might be due to invalid IL or missing references)
		//IL_264e: Unknown result type (might be due to invalid IL or missing references)
		//IL_2658: Unknown result type (might be due to invalid IL or missing references)
		//IL_265a: Unknown result type (might be due to invalid IL or missing references)
		//IL_2664: Unknown result type (might be due to invalid IL or missing references)
		//IL_2666: Unknown result type (might be due to invalid IL or missing references)
		//IL_2670: Unknown result type (might be due to invalid IL or missing references)
		//IL_2672: Unknown result type (might be due to invalid IL or missing references)
		//IL_267c: Unknown result type (might be due to invalid IL or missing references)
		//IL_267e: Unknown result type (might be due to invalid IL or missing references)
		//IL_2688: Unknown result type (might be due to invalid IL or missing references)
		//IL_268a: Unknown result type (might be due to invalid IL or missing references)
		//IL_2694: Unknown result type (might be due to invalid IL or missing references)
		//IL_2696: Unknown result type (might be due to invalid IL or missing references)
		//IL_26a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_26a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_26b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_26b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_26c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_26c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_26cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_26cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_26d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_26db: Unknown result type (might be due to invalid IL or missing references)
		//IL_26e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_26e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_26f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_26f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_2700: Unknown result type (might be due to invalid IL or missing references)
		//IL_2702: Unknown result type (might be due to invalid IL or missing references)
		//IL_270f: Unknown result type (might be due to invalid IL or missing references)
		//IL_2711: Unknown result type (might be due to invalid IL or missing references)
		//IL_271e: Unknown result type (might be due to invalid IL or missing references)
		//IL_2720: Unknown result type (might be due to invalid IL or missing references)
		//IL_272d: Unknown result type (might be due to invalid IL or missing references)
		//IL_272f: Unknown result type (might be due to invalid IL or missing references)
		//IL_273c: Unknown result type (might be due to invalid IL or missing references)
		//IL_273e: Unknown result type (might be due to invalid IL or missing references)
		//IL_274b: Unknown result type (might be due to invalid IL or missing references)
		//IL_274d: Unknown result type (might be due to invalid IL or missing references)
		//IL_275a: Unknown result type (might be due to invalid IL or missing references)
		//IL_275c: Unknown result type (might be due to invalid IL or missing references)
		//IL_2769: Unknown result type (might be due to invalid IL or missing references)
		//IL_276b: Unknown result type (might be due to invalid IL or missing references)
		//IL_2778: Unknown result type (might be due to invalid IL or missing references)
		//IL_277a: Unknown result type (might be due to invalid IL or missing references)
		//IL_2787: Unknown result type (might be due to invalid IL or missing references)
		//IL_2789: Unknown result type (might be due to invalid IL or missing references)
		//IL_2796: Unknown result type (might be due to invalid IL or missing references)
		//IL_2798: Unknown result type (might be due to invalid IL or missing references)
		//IL_27a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_27a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_27b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_27b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_27c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_27ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_27e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_27e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_27ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_2804: Unknown result type (might be due to invalid IL or missing references)
		//IL_281d: Unknown result type (might be due to invalid IL or missing references)
		//IL_2822: Unknown result type (might be due to invalid IL or missing references)
		//IL_2838: Unknown result type (might be due to invalid IL or missing references)
		//IL_2842: Unknown result type (might be due to invalid IL or missing references)
		//IL_2844: Unknown result type (might be due to invalid IL or missing references)
		//IL_284e: Unknown result type (might be due to invalid IL or missing references)
		//IL_2850: Unknown result type (might be due to invalid IL or missing references)
		//IL_285a: Unknown result type (might be due to invalid IL or missing references)
		//IL_285c: Unknown result type (might be due to invalid IL or missing references)
		//IL_2866: Unknown result type (might be due to invalid IL or missing references)
		//IL_2868: Unknown result type (might be due to invalid IL or missing references)
		//IL_2872: Unknown result type (might be due to invalid IL or missing references)
		//IL_2874: Unknown result type (might be due to invalid IL or missing references)
		//IL_2881: Unknown result type (might be due to invalid IL or missing references)
		//IL_2883: Unknown result type (might be due to invalid IL or missing references)
		//IL_2890: Unknown result type (might be due to invalid IL or missing references)
		//IL_2892: Unknown result type (might be due to invalid IL or missing references)
		//IL_289f: Unknown result type (might be due to invalid IL or missing references)
		//IL_28a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_28ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_28b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_28bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_28bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_28cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_28ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_28db: Unknown result type (might be due to invalid IL or missing references)
		//IL_28dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_28ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_28ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_28f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_28fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_2908: Unknown result type (might be due to invalid IL or missing references)
		//IL_290a: Unknown result type (might be due to invalid IL or missing references)
		//IL_2917: Unknown result type (might be due to invalid IL or missing references)
		//IL_2919: Unknown result type (might be due to invalid IL or missing references)
		//IL_2926: Unknown result type (might be due to invalid IL or missing references)
		//IL_2928: Unknown result type (might be due to invalid IL or missing references)
		//IL_2935: Unknown result type (might be due to invalid IL or missing references)
		//IL_2937: Unknown result type (might be due to invalid IL or missing references)
		//IL_2944: Unknown result type (might be due to invalid IL or missing references)
		//IL_2946: Unknown result type (might be due to invalid IL or missing references)
		//IL_2953: Unknown result type (might be due to invalid IL or missing references)
		//IL_2955: Unknown result type (might be due to invalid IL or missing references)
		//IL_2962: Unknown result type (might be due to invalid IL or missing references)
		//IL_2964: Unknown result type (might be due to invalid IL or missing references)
		//IL_2971: Unknown result type (might be due to invalid IL or missing references)
		//IL_2973: Unknown result type (might be due to invalid IL or missing references)
		//IL_2980: Unknown result type (might be due to invalid IL or missing references)
		//IL_2982: Unknown result type (might be due to invalid IL or missing references)
		//IL_298f: Unknown result type (might be due to invalid IL or missing references)
		//IL_2991: Unknown result type (might be due to invalid IL or missing references)
		//IL_299e: Unknown result type (might be due to invalid IL or missing references)
		//IL_29a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_29ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_29af: Unknown result type (might be due to invalid IL or missing references)
		//IL_29bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_29be: Unknown result type (might be due to invalid IL or missing references)
		//IL_29da: Unknown result type (might be due to invalid IL or missing references)
		//IL_29df: Unknown result type (might be due to invalid IL or missing references)
		//IL_29fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_2a00: Unknown result type (might be due to invalid IL or missing references)
		//IL_2a19: Unknown result type (might be due to invalid IL or missing references)
		//IL_2a1e: Unknown result type (might be due to invalid IL or missing references)
		//IL_2a31: Unknown result type (might be due to invalid IL or missing references)
		//IL_2a36: Unknown result type (might be due to invalid IL or missing references)
		//IL_2a4f: Unknown result type (might be due to invalid IL or missing references)
		//IL_2a54: Unknown result type (might be due to invalid IL or missing references)
		//IL_2a70: Unknown result type (might be due to invalid IL or missing references)
		//IL_2a75: Unknown result type (might be due to invalid IL or missing references)
		//IL_2a8a: Unknown result type (might be due to invalid IL or missing references)
		//IL_2a8f: Unknown result type (might be due to invalid IL or missing references)
		//IL_2aab: Unknown result type (might be due to invalid IL or missing references)
		//IL_2ab0: Unknown result type (might be due to invalid IL or missing references)
		//IL_2ac6: Unknown result type (might be due to invalid IL or missing references)
		//IL_2acb: Unknown result type (might be due to invalid IL or missing references)
		//IL_2adb: Unknown result type (might be due to invalid IL or missing references)
		//IL_2ae0: Unknown result type (might be due to invalid IL or missing references)
		//IL_2af6: Unknown result type (might be due to invalid IL or missing references)
		//IL_2afb: Unknown result type (might be due to invalid IL or missing references)
		//IL_2b0d: Unknown result type (might be due to invalid IL or missing references)
		//IL_2b12: Unknown result type (might be due to invalid IL or missing references)
		//IL_2b24: Unknown result type (might be due to invalid IL or missing references)
		//IL_2b29: Unknown result type (might be due to invalid IL or missing references)
		//IL_2b3c: Unknown result type (might be due to invalid IL or missing references)
		//IL_2b41: Unknown result type (might be due to invalid IL or missing references)
		//IL_2b54: Unknown result type (might be due to invalid IL or missing references)
		//IL_2b59: Unknown result type (might be due to invalid IL or missing references)
		//IL_2b6f: Unknown result type (might be due to invalid IL or missing references)
		//IL_2b74: Unknown result type (might be due to invalid IL or missing references)
		//IL_2b8a: Unknown result type (might be due to invalid IL or missing references)
		//IL_2b8f: Unknown result type (might be due to invalid IL or missing references)
		//IL_2ba8: Unknown result type (might be due to invalid IL or missing references)
		//IL_2bad: Unknown result type (might be due to invalid IL or missing references)
		//IL_2bbd: Unknown result type (might be due to invalid IL or missing references)
		//IL_2bc2: Unknown result type (might be due to invalid IL or missing references)
		//IL_2bd8: Unknown result type (might be due to invalid IL or missing references)
		//IL_2bdd: Unknown result type (might be due to invalid IL or missing references)
		//IL_2bf3: Unknown result type (might be due to invalid IL or missing references)
		//IL_2bf8: Unknown result type (might be due to invalid IL or missing references)
		//IL_2c0b: Unknown result type (might be due to invalid IL or missing references)
		//IL_2c10: Unknown result type (might be due to invalid IL or missing references)
		//IL_2c23: Unknown result type (might be due to invalid IL or missing references)
		//IL_2c28: Unknown result type (might be due to invalid IL or missing references)
		//IL_2c41: Unknown result type (might be due to invalid IL or missing references)
		//IL_2c46: Unknown result type (might be due to invalid IL or missing references)
		//IL_2c6f: Unknown result type (might be due to invalid IL or missing references)
		//IL_2c74: Unknown result type (might be due to invalid IL or missing references)
		//IL_2c8d: Unknown result type (might be due to invalid IL or missing references)
		//IL_2c92: Unknown result type (might be due to invalid IL or missing references)
		//IL_2ca4: Unknown result type (might be due to invalid IL or missing references)
		//IL_2ca9: Unknown result type (might be due to invalid IL or missing references)
		//IL_2cc5: Unknown result type (might be due to invalid IL or missing references)
		//IL_2cca: Unknown result type (might be due to invalid IL or missing references)
		//IL_2ce0: Unknown result type (might be due to invalid IL or missing references)
		//IL_2ce5: Unknown result type (might be due to invalid IL or missing references)
		//IL_2cf8: Unknown result type (might be due to invalid IL or missing references)
		//IL_2cfd: Unknown result type (might be due to invalid IL or missing references)
		//IL_2d0f: Unknown result type (might be due to invalid IL or missing references)
		//IL_2d14: Unknown result type (might be due to invalid IL or missing references)
		//IL_2d26: Unknown result type (might be due to invalid IL or missing references)
		//IL_2d2b: Unknown result type (might be due to invalid IL or missing references)
		//IL_2d44: Unknown result type (might be due to invalid IL or missing references)
		//IL_2d49: Unknown result type (might be due to invalid IL or missing references)
		//IL_2d5f: Unknown result type (might be due to invalid IL or missing references)
		//IL_2d64: Unknown result type (might be due to invalid IL or missing references)
		//IL_2d77: Unknown result type (might be due to invalid IL or missing references)
		//IL_2d7c: Unknown result type (might be due to invalid IL or missing references)
		//IL_2d8e: Unknown result type (might be due to invalid IL or missing references)
		//IL_2d93: Unknown result type (might be due to invalid IL or missing references)
		//IL_2dac: Unknown result type (might be due to invalid IL or missing references)
		//IL_2db1: Unknown result type (might be due to invalid IL or missing references)
		//IL_2dc7: Unknown result type (might be due to invalid IL or missing references)
		//IL_2dcc: Unknown result type (might be due to invalid IL or missing references)
		//IL_2ddf: Unknown result type (might be due to invalid IL or missing references)
		//IL_2de4: Unknown result type (might be due to invalid IL or missing references)
		//IL_2dfd: Unknown result type (might be due to invalid IL or missing references)
		//IL_2e02: Unknown result type (might be due to invalid IL or missing references)
		//IL_2e12: Unknown result type (might be due to invalid IL or missing references)
		//IL_2e17: Unknown result type (might be due to invalid IL or missing references)
		//IL_2e2d: Unknown result type (might be due to invalid IL or missing references)
		//IL_2e32: Unknown result type (might be due to invalid IL or missing references)
		//IL_2e4b: Unknown result type (might be due to invalid IL or missing references)
		//IL_2e50: Unknown result type (might be due to invalid IL or missing references)
		//IL_2e69: Unknown result type (might be due to invalid IL or missing references)
		//IL_2e6e: Unknown result type (might be due to invalid IL or missing references)
		//IL_2e87: Unknown result type (might be due to invalid IL or missing references)
		//IL_2e8c: Unknown result type (might be due to invalid IL or missing references)
		//IL_2ea5: Unknown result type (might be due to invalid IL or missing references)
		//IL_2eaa: Unknown result type (might be due to invalid IL or missing references)
		//IL_2ebd: Unknown result type (might be due to invalid IL or missing references)
		//IL_2ec2: Unknown result type (might be due to invalid IL or missing references)
		//IL_2ed8: Unknown result type (might be due to invalid IL or missing references)
		//IL_2edd: Unknown result type (might be due to invalid IL or missing references)
		//IL_2ef6: Unknown result type (might be due to invalid IL or missing references)
		//IL_2efb: Unknown result type (might be due to invalid IL or missing references)
		//IL_2f0e: Unknown result type (might be due to invalid IL or missing references)
		//IL_2f13: Unknown result type (might be due to invalid IL or missing references)
		//IL_2f28: Unknown result type (might be due to invalid IL or missing references)
		//IL_2f2d: Unknown result type (might be due to invalid IL or missing references)
		//IL_2f42: Unknown result type (might be due to invalid IL or missing references)
		//IL_2f47: Unknown result type (might be due to invalid IL or missing references)
		//IL_2f63: Unknown result type (might be due to invalid IL or missing references)
		//IL_2f68: Unknown result type (might be due to invalid IL or missing references)
		//IL_2f81: Unknown result type (might be due to invalid IL or missing references)
		//IL_2f86: Unknown result type (might be due to invalid IL or missing references)
		//IL_2f9f: Unknown result type (might be due to invalid IL or missing references)
		//IL_2fa4: Unknown result type (might be due to invalid IL or missing references)
		//IL_2fb7: Unknown result type (might be due to invalid IL or missing references)
		//IL_2fbc: Unknown result type (might be due to invalid IL or missing references)
		//IL_2fd8: Unknown result type (might be due to invalid IL or missing references)
		//IL_2fdd: Unknown result type (might be due to invalid IL or missing references)
		//IL_2ff0: Unknown result type (might be due to invalid IL or missing references)
		//IL_2ff5: Unknown result type (might be due to invalid IL or missing references)
		//IL_3008: Unknown result type (might be due to invalid IL or missing references)
		//IL_300d: Unknown result type (might be due to invalid IL or missing references)
		//IL_3026: Unknown result type (might be due to invalid IL or missing references)
		//IL_302b: Unknown result type (might be due to invalid IL or missing references)
		//IL_3041: Unknown result type (might be due to invalid IL or missing references)
		//IL_3046: Unknown result type (might be due to invalid IL or missing references)
		//IL_305c: Unknown result type (might be due to invalid IL or missing references)
		//IL_3061: Unknown result type (might be due to invalid IL or missing references)
		//IL_3076: Unknown result type (might be due to invalid IL or missing references)
		//IL_307b: Unknown result type (might be due to invalid IL or missing references)
		//IL_3091: Unknown result type (might be due to invalid IL or missing references)
		//IL_3096: Unknown result type (might be due to invalid IL or missing references)
		//IL_30ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_30b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_30cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_30d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_30ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_30f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_310f: Unknown result type (might be due to invalid IL or missing references)
		//IL_3114: Unknown result type (might be due to invalid IL or missing references)
		//IL_312a: Unknown result type (might be due to invalid IL or missing references)
		//IL_312f: Unknown result type (might be due to invalid IL or missing references)
		//IL_3145: Unknown result type (might be due to invalid IL or missing references)
		//IL_314a: Unknown result type (might be due to invalid IL or missing references)
		//IL_3163: Unknown result type (might be due to invalid IL or missing references)
		//IL_3168: Unknown result type (might be due to invalid IL or missing references)
		//IL_3184: Unknown result type (might be due to invalid IL or missing references)
		//IL_3189: Unknown result type (might be due to invalid IL or missing references)
		//IL_31a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_31a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_31c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_31c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_31d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_31dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_31f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_31f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_320e: Unknown result type (might be due to invalid IL or missing references)
		//IL_3213: Unknown result type (might be due to invalid IL or missing references)
		//IL_3226: Unknown result type (might be due to invalid IL or missing references)
		//IL_322b: Unknown result type (might be due to invalid IL or missing references)
		//IL_3247: Unknown result type (might be due to invalid IL or missing references)
		//IL_324c: Unknown result type (might be due to invalid IL or missing references)
		//IL_325f: Unknown result type (might be due to invalid IL or missing references)
		//IL_3264: Unknown result type (might be due to invalid IL or missing references)
		//IL_3280: Unknown result type (might be due to invalid IL or missing references)
		//IL_3285: Unknown result type (might be due to invalid IL or missing references)
		//IL_3292: Unknown result type (might be due to invalid IL or missing references)
		//IL_329f: Unknown result type (might be due to invalid IL or missing references)
		//IL_32a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_32ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_32b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_32bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_32bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_32d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_32dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_32f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_32fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_3317: Unknown result type (might be due to invalid IL or missing references)
		//IL_331c: Unknown result type (might be due to invalid IL or missing references)
		//IL_3335: Unknown result type (might be due to invalid IL or missing references)
		//IL_333a: Unknown result type (might be due to invalid IL or missing references)
		//IL_3356: Unknown result type (might be due to invalid IL or missing references)
		//IL_335b: Unknown result type (might be due to invalid IL or missing references)
		//IL_3377: Unknown result type (might be due to invalid IL or missing references)
		//IL_337c: Unknown result type (might be due to invalid IL or missing references)
		//IL_3398: Unknown result type (might be due to invalid IL or missing references)
		//IL_339d: Unknown result type (might be due to invalid IL or missing references)
		//IL_33b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_33be: Unknown result type (might be due to invalid IL or missing references)
		//IL_33da: Unknown result type (might be due to invalid IL or missing references)
		//IL_33df: Unknown result type (might be due to invalid IL or missing references)
		//IL_33fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_3400: Unknown result type (might be due to invalid IL or missing references)
		//IL_341c: Unknown result type (might be due to invalid IL or missing references)
		//IL_3421: Unknown result type (might be due to invalid IL or missing references)
		//IL_343d: Unknown result type (might be due to invalid IL or missing references)
		//IL_3442: Unknown result type (might be due to invalid IL or missing references)
		//IL_345b: Unknown result type (might be due to invalid IL or missing references)
		//IL_3460: Unknown result type (might be due to invalid IL or missing references)
		//IL_347c: Unknown result type (might be due to invalid IL or missing references)
		//IL_3481: Unknown result type (might be due to invalid IL or missing references)
		//IL_3494: Unknown result type (might be due to invalid IL or missing references)
		//IL_3499: Unknown result type (might be due to invalid IL or missing references)
		//IL_34b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_34ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_34d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_34db: Unknown result type (might be due to invalid IL or missing references)
		//IL_34f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_34fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_3512: Unknown result type (might be due to invalid IL or missing references)
		//IL_3517: Unknown result type (might be due to invalid IL or missing references)
		//IL_352d: Unknown result type (might be due to invalid IL or missing references)
		//IL_3532: Unknown result type (might be due to invalid IL or missing references)
		//IL_354b: Unknown result type (might be due to invalid IL or missing references)
		//IL_3550: Unknown result type (might be due to invalid IL or missing references)
		//IL_3569: Unknown result type (might be due to invalid IL or missing references)
		//IL_356e: Unknown result type (might be due to invalid IL or missing references)
		//IL_3584: Unknown result type (might be due to invalid IL or missing references)
		//IL_3589: Unknown result type (might be due to invalid IL or missing references)
		//IL_359b: Unknown result type (might be due to invalid IL or missing references)
		//IL_35a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_35b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_35b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_35c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_35ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_35e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_35e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_35f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_35fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_360f: Unknown result type (might be due to invalid IL or missing references)
		//IL_3614: Unknown result type (might be due to invalid IL or missing references)
		//IL_362a: Unknown result type (might be due to invalid IL or missing references)
		//IL_362f: Unknown result type (might be due to invalid IL or missing references)
		//IL_3642: Unknown result type (might be due to invalid IL or missing references)
		//IL_364c: Unknown result type (might be due to invalid IL or missing references)
		//IL_364e: Unknown result type (might be due to invalid IL or missing references)
		//IL_3658: Unknown result type (might be due to invalid IL or missing references)
		//IL_365a: Unknown result type (might be due to invalid IL or missing references)
		//IL_3664: Unknown result type (might be due to invalid IL or missing references)
		//IL_3666: Unknown result type (might be due to invalid IL or missing references)
		//IL_3679: Unknown result type (might be due to invalid IL or missing references)
		//IL_3683: Unknown result type (might be due to invalid IL or missing references)
		//IL_3685: Unknown result type (might be due to invalid IL or missing references)
		//IL_368f: Unknown result type (might be due to invalid IL or missing references)
		//IL_3691: Unknown result type (might be due to invalid IL or missing references)
		//IL_369b: Unknown result type (might be due to invalid IL or missing references)
		//IL_369d: Unknown result type (might be due to invalid IL or missing references)
		//IL_36b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_36ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_36bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_36c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_36c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_36d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_36d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_36e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_36f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_36f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_36fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_36ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_3709: Unknown result type (might be due to invalid IL or missing references)
		//IL_370b: Unknown result type (might be due to invalid IL or missing references)
		//IL_3718: Unknown result type (might be due to invalid IL or missing references)
		//IL_371a: Unknown result type (might be due to invalid IL or missing references)
		//IL_372a: Unknown result type (might be due to invalid IL or missing references)
		//IL_3734: Unknown result type (might be due to invalid IL or missing references)
		//IL_3736: Unknown result type (might be due to invalid IL or missing references)
		//IL_3740: Unknown result type (might be due to invalid IL or missing references)
		//IL_3742: Unknown result type (might be due to invalid IL or missing references)
		//IL_374c: Unknown result type (might be due to invalid IL or missing references)
		//IL_374e: Unknown result type (might be due to invalid IL or missing references)
		//IL_375e: Unknown result type (might be due to invalid IL or missing references)
		//IL_3768: Unknown result type (might be due to invalid IL or missing references)
		//IL_376a: Unknown result type (might be due to invalid IL or missing references)
		//IL_3774: Unknown result type (might be due to invalid IL or missing references)
		//IL_3776: Unknown result type (might be due to invalid IL or missing references)
		//IL_3780: Unknown result type (might be due to invalid IL or missing references)
		//IL_3782: Unknown result type (might be due to invalid IL or missing references)
		//IL_3795: Unknown result type (might be due to invalid IL or missing references)
		//IL_379f: Unknown result type (might be due to invalid IL or missing references)
		//IL_37a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_37ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_37ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_37b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_37b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_37d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_37d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_37f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_37f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_380b: Unknown result type (might be due to invalid IL or missing references)
		//IL_3810: Unknown result type (might be due to invalid IL or missing references)
		//IL_3829: Unknown result type (might be due to invalid IL or missing references)
		//IL_382e: Unknown result type (might be due to invalid IL or missing references)
		//IL_3841: Unknown result type (might be due to invalid IL or missing references)
		//IL_3846: Unknown result type (might be due to invalid IL or missing references)
		//IL_3862: Unknown result type (might be due to invalid IL or missing references)
		//IL_3867: Unknown result type (might be due to invalid IL or missing references)
		//IL_387d: Unknown result type (might be due to invalid IL or missing references)
		//IL_3882: Unknown result type (might be due to invalid IL or missing references)
		//IL_3898: Unknown result type (might be due to invalid IL or missing references)
		//IL_389d: Unknown result type (might be due to invalid IL or missing references)
		//IL_38b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_38bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_38ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_38d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_38ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_38f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_390a: Unknown result type (might be due to invalid IL or missing references)
		//IL_390f: Unknown result type (might be due to invalid IL or missing references)
		//IL_392b: Unknown result type (might be due to invalid IL or missing references)
		//IL_3930: Unknown result type (might be due to invalid IL or missing references)
		//IL_3947: Unknown result type (might be due to invalid IL or missing references)
		//IL_394c: Unknown result type (might be due to invalid IL or missing references)
		//IL_3978: Unknown result type (might be due to invalid IL or missing references)
		//IL_397d: Unknown result type (might be due to invalid IL or missing references)
		//IL_3996: Unknown result type (might be due to invalid IL or missing references)
		//IL_399b: Unknown result type (might be due to invalid IL or missing references)
		//IL_39b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_39b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_39d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_39d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_39ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_39f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_3a0e: Unknown result type (might be due to invalid IL or missing references)
		//IL_3a13: Unknown result type (might be due to invalid IL or missing references)
		//IL_3a2c: Unknown result type (might be due to invalid IL or missing references)
		//IL_3a31: Unknown result type (might be due to invalid IL or missing references)
		//IL_3a3e: Unknown result type (might be due to invalid IL or missing references)
		//IL_3a4b: Unknown result type (might be due to invalid IL or missing references)
		//IL_3a4d: Unknown result type (might be due to invalid IL or missing references)
		//IL_3a5a: Unknown result type (might be due to invalid IL or missing references)
		//IL_3a5c: Unknown result type (might be due to invalid IL or missing references)
		//IL_3a69: Unknown result type (might be due to invalid IL or missing references)
		//IL_3a6b: Unknown result type (might be due to invalid IL or missing references)
		//IL_3a78: Unknown result type (might be due to invalid IL or missing references)
		//IL_3a7a: Unknown result type (might be due to invalid IL or missing references)
		//IL_3a87: Unknown result type (might be due to invalid IL or missing references)
		//IL_3a89: Unknown result type (might be due to invalid IL or missing references)
		//IL_3a96: Unknown result type (might be due to invalid IL or missing references)
		//IL_3aa3: Unknown result type (might be due to invalid IL or missing references)
		//IL_3aa5: Unknown result type (might be due to invalid IL or missing references)
		//IL_3ab2: Unknown result type (might be due to invalid IL or missing references)
		//IL_3ab4: Unknown result type (might be due to invalid IL or missing references)
		//IL_3ac1: Unknown result type (might be due to invalid IL or missing references)
		//IL_3ac3: Unknown result type (might be due to invalid IL or missing references)
		//IL_3ad0: Unknown result type (might be due to invalid IL or missing references)
		//IL_3ad2: Unknown result type (might be due to invalid IL or missing references)
		//IL_3ae5: Unknown result type (might be due to invalid IL or missing references)
		//IL_3af2: Unknown result type (might be due to invalid IL or missing references)
		//IL_3af4: Unknown result type (might be due to invalid IL or missing references)
		//IL_3b01: Unknown result type (might be due to invalid IL or missing references)
		//IL_3b03: Unknown result type (might be due to invalid IL or missing references)
		//IL_3b10: Unknown result type (might be due to invalid IL or missing references)
		//IL_3b12: Unknown result type (might be due to invalid IL or missing references)
		//IL_3b28: Unknown result type (might be due to invalid IL or missing references)
		//IL_3b35: Unknown result type (might be due to invalid IL or missing references)
		//IL_3b37: Unknown result type (might be due to invalid IL or missing references)
		//IL_3b44: Unknown result type (might be due to invalid IL or missing references)
		//IL_3b46: Unknown result type (might be due to invalid IL or missing references)
		//IL_3b53: Unknown result type (might be due to invalid IL or missing references)
		//IL_3b55: Unknown result type (might be due to invalid IL or missing references)
		//IL_3b6b: Unknown result type (might be due to invalid IL or missing references)
		//IL_3b78: Unknown result type (might be due to invalid IL or missing references)
		//IL_3b7a: Unknown result type (might be due to invalid IL or missing references)
		//IL_3b87: Unknown result type (might be due to invalid IL or missing references)
		//IL_3b89: Unknown result type (might be due to invalid IL or missing references)
		//IL_3b96: Unknown result type (might be due to invalid IL or missing references)
		//IL_3b98: Unknown result type (might be due to invalid IL or missing references)
		//IL_3ba8: Unknown result type (might be due to invalid IL or missing references)
		//IL_3bb5: Unknown result type (might be due to invalid IL or missing references)
		//IL_3bb7: Unknown result type (might be due to invalid IL or missing references)
		//IL_3bc4: Unknown result type (might be due to invalid IL or missing references)
		//IL_3bc6: Unknown result type (might be due to invalid IL or missing references)
		//IL_3bd3: Unknown result type (might be due to invalid IL or missing references)
		//IL_3bd5: Unknown result type (might be due to invalid IL or missing references)
		//IL_3be2: Unknown result type (might be due to invalid IL or missing references)
		//IL_3bef: Unknown result type (might be due to invalid IL or missing references)
		//IL_3bf1: Unknown result type (might be due to invalid IL or missing references)
		//IL_3bfe: Unknown result type (might be due to invalid IL or missing references)
		//IL_3c00: Unknown result type (might be due to invalid IL or missing references)
		//IL_3c0d: Unknown result type (might be due to invalid IL or missing references)
		//IL_3c0f: Unknown result type (might be due to invalid IL or missing references)
		//IL_3c1f: Unknown result type (might be due to invalid IL or missing references)
		//IL_3c2c: Unknown result type (might be due to invalid IL or missing references)
		//IL_3c2e: Unknown result type (might be due to invalid IL or missing references)
		//IL_3c3b: Unknown result type (might be due to invalid IL or missing references)
		//IL_3c3d: Unknown result type (might be due to invalid IL or missing references)
		//IL_3c4a: Unknown result type (might be due to invalid IL or missing references)
		//IL_3c4c: Unknown result type (might be due to invalid IL or missing references)
		//IL_3c5c: Unknown result type (might be due to invalid IL or missing references)
		//IL_3c69: Unknown result type (might be due to invalid IL or missing references)
		//IL_3c6b: Unknown result type (might be due to invalid IL or missing references)
		//IL_3c78: Unknown result type (might be due to invalid IL or missing references)
		//IL_3c7a: Unknown result type (might be due to invalid IL or missing references)
		//IL_3c87: Unknown result type (might be due to invalid IL or missing references)
		//IL_3c89: Unknown result type (might be due to invalid IL or missing references)
		//IL_3c96: Unknown result type (might be due to invalid IL or missing references)
		//IL_3ca4: Unknown result type (might be due to invalid IL or missing references)
		//IL_3ca6: Unknown result type (might be due to invalid IL or missing references)
		//IL_3cb4: Unknown result type (might be due to invalid IL or missing references)
		//IL_3cb6: Unknown result type (might be due to invalid IL or missing references)
		//IL_3cc4: Unknown result type (might be due to invalid IL or missing references)
		//IL_3cc6: Unknown result type (might be due to invalid IL or missing references)
		//IL_3cd6: Unknown result type (might be due to invalid IL or missing references)
		//IL_3ce4: Unknown result type (might be due to invalid IL or missing references)
		//IL_3ce6: Unknown result type (might be due to invalid IL or missing references)
		//IL_3cf4: Unknown result type (might be due to invalid IL or missing references)
		//IL_3cf6: Unknown result type (might be due to invalid IL or missing references)
		//IL_3d04: Unknown result type (might be due to invalid IL or missing references)
		//IL_3d06: Unknown result type (might be due to invalid IL or missing references)
		//IL_3d10: Unknown result type (might be due to invalid IL or missing references)
		//IL_3d15: Unknown result type (might be due to invalid IL or missing references)
		//IL_3d20: Unknown result type (might be due to invalid IL or missing references)
		//IL_3d22: Unknown result type (might be due to invalid IL or missing references)
		//IL_3d30: Unknown result type (might be due to invalid IL or missing references)
		//IL_3d32: Unknown result type (might be due to invalid IL or missing references)
		//IL_3d40: Unknown result type (might be due to invalid IL or missing references)
		//IL_3d42: Unknown result type (might be due to invalid IL or missing references)
		//IL_3d62: Unknown result type (might be due to invalid IL or missing references)
		//IL_3d67: Unknown result type (might be due to invalid IL or missing references)
		//IL_3d95: Unknown result type (might be due to invalid IL or missing references)
		//IL_3d9a: Unknown result type (might be due to invalid IL or missing references)
		//IL_3dbd: Unknown result type (might be due to invalid IL or missing references)
		//IL_3dc2: Unknown result type (might be due to invalid IL or missing references)
		//IL_3de2: Unknown result type (might be due to invalid IL or missing references)
		//IL_3de7: Unknown result type (might be due to invalid IL or missing references)
		//IL_3e15: Unknown result type (might be due to invalid IL or missing references)
		//IL_3e1a: Unknown result type (might be due to invalid IL or missing references)
		//IL_3e41: Unknown result type (might be due to invalid IL or missing references)
		//IL_3e46: Unknown result type (might be due to invalid IL or missing references)
		//IL_3e5c: Unknown result type (might be due to invalid IL or missing references)
		//IL_3e61: Unknown result type (might be due to invalid IL or missing references)
		//IL_3e77: Unknown result type (might be due to invalid IL or missing references)
		//IL_3e7c: Unknown result type (might be due to invalid IL or missing references)
		//IL_3e94: Unknown result type (might be due to invalid IL or missing references)
		//IL_3e99: Unknown result type (might be due to invalid IL or missing references)
		//IL_3eaf: Unknown result type (might be due to invalid IL or missing references)
		//IL_3eb4: Unknown result type (might be due to invalid IL or missing references)
		//IL_3ec6: Unknown result type (might be due to invalid IL or missing references)
		//IL_3ecb: Unknown result type (might be due to invalid IL or missing references)
		//IL_3ee4: Unknown result type (might be due to invalid IL or missing references)
		//IL_3ee9: Unknown result type (might be due to invalid IL or missing references)
		//IL_3eff: Unknown result type (might be due to invalid IL or missing references)
		//IL_3f04: Unknown result type (might be due to invalid IL or missing references)
		//IL_3f1a: Unknown result type (might be due to invalid IL or missing references)
		//IL_3f1f: Unknown result type (might be due to invalid IL or missing references)
		//IL_3f38: Unknown result type (might be due to invalid IL or missing references)
		//IL_3f3d: Unknown result type (might be due to invalid IL or missing references)
		//IL_3f57: Unknown result type (might be due to invalid IL or missing references)
		//IL_3f5c: Unknown result type (might be due to invalid IL or missing references)
		//IL_3f73: Unknown result type (might be due to invalid IL or missing references)
		//IL_3f78: Unknown result type (might be due to invalid IL or missing references)
		//IL_3f8e: Unknown result type (might be due to invalid IL or missing references)
		//IL_3f93: Unknown result type (might be due to invalid IL or missing references)
		//IL_3fa8: Unknown result type (might be due to invalid IL or missing references)
		//IL_3fad: Unknown result type (might be due to invalid IL or missing references)
		//IL_3fc6: Unknown result type (might be due to invalid IL or missing references)
		//IL_3fcb: Unknown result type (might be due to invalid IL or missing references)
		//IL_3fe4: Unknown result type (might be due to invalid IL or missing references)
		//IL_3fe9: Unknown result type (might be due to invalid IL or missing references)
		//IL_4005: Unknown result type (might be due to invalid IL or missing references)
		//IL_400a: Unknown result type (might be due to invalid IL or missing references)
		//IL_4020: Unknown result type (might be due to invalid IL or missing references)
		//IL_4025: Unknown result type (might be due to invalid IL or missing references)
		//IL_4041: Unknown result type (might be due to invalid IL or missing references)
		//IL_4046: Unknown result type (might be due to invalid IL or missing references)
		//IL_405f: Unknown result type (might be due to invalid IL or missing references)
		//IL_4064: Unknown result type (might be due to invalid IL or missing references)
		//IL_407a: Unknown result type (might be due to invalid IL or missing references)
		//IL_407f: Unknown result type (might be due to invalid IL or missing references)
		//IL_4095: Unknown result type (might be due to invalid IL or missing references)
		//IL_409a: Unknown result type (might be due to invalid IL or missing references)
		//IL_40af: Unknown result type (might be due to invalid IL or missing references)
		//IL_40b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_40c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_40c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_40d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_40de: Unknown result type (might be due to invalid IL or missing references)
		//IL_40f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_40f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_4115: Unknown result type (might be due to invalid IL or missing references)
		//IL_411a: Unknown result type (might be due to invalid IL or missing references)
		//IL_4133: Unknown result type (might be due to invalid IL or missing references)
		//IL_4138: Unknown result type (might be due to invalid IL or missing references)
		//IL_4154: Unknown result type (might be due to invalid IL or missing references)
		//IL_4159: Unknown result type (might be due to invalid IL or missing references)
		//IL_4175: Unknown result type (might be due to invalid IL or missing references)
		//IL_417a: Unknown result type (might be due to invalid IL or missing references)
		//IL_418f: Unknown result type (might be due to invalid IL or missing references)
		//IL_4194: Unknown result type (might be due to invalid IL or missing references)
		//IL_41b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_41b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_41ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_41d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_41e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_41ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_4207: Unknown result type (might be due to invalid IL or missing references)
		//IL_420c: Unknown result type (might be due to invalid IL or missing references)
		//IL_4221: Unknown result type (might be due to invalid IL or missing references)
		//IL_4226: Unknown result type (might be due to invalid IL or missing references)
		//IL_423c: Unknown result type (might be due to invalid IL or missing references)
		//IL_4241: Unknown result type (might be due to invalid IL or missing references)
		//IL_425a: Unknown result type (might be due to invalid IL or missing references)
		//IL_425f: Unknown result type (might be due to invalid IL or missing references)
		//IL_4278: Unknown result type (might be due to invalid IL or missing references)
		//IL_427d: Unknown result type (might be due to invalid IL or missing references)
		//IL_4299: Unknown result type (might be due to invalid IL or missing references)
		//IL_429e: Unknown result type (might be due to invalid IL or missing references)
		//IL_42b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_42bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_42d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_42da: Unknown result type (might be due to invalid IL or missing references)
		//IL_42f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_42f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_430e: Unknown result type (might be due to invalid IL or missing references)
		//IL_4313: Unknown result type (might be due to invalid IL or missing references)
		//IL_4329: Unknown result type (might be due to invalid IL or missing references)
		//IL_432e: Unknown result type (might be due to invalid IL or missing references)
		//IL_4344: Unknown result type (might be due to invalid IL or missing references)
		//IL_4349: Unknown result type (might be due to invalid IL or missing references)
		//IL_435f: Unknown result type (might be due to invalid IL or missing references)
		//IL_4364: Unknown result type (might be due to invalid IL or missing references)
		//IL_437a: Unknown result type (might be due to invalid IL or missing references)
		//IL_437f: Unknown result type (might be due to invalid IL or missing references)
		//IL_4395: Unknown result type (might be due to invalid IL or missing references)
		//IL_439a: Unknown result type (might be due to invalid IL or missing references)
		//IL_43b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_43b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_43d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_43d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_43ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_43f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_440d: Unknown result type (might be due to invalid IL or missing references)
		//IL_4412: Unknown result type (might be due to invalid IL or missing references)
		//IL_4428: Unknown result type (might be due to invalid IL or missing references)
		//IL_442d: Unknown result type (might be due to invalid IL or missing references)
		//IL_4446: Unknown result type (might be due to invalid IL or missing references)
		//IL_444b: Unknown result type (might be due to invalid IL or missing references)
		//IL_4464: Unknown result type (might be due to invalid IL or missing references)
		//IL_4469: Unknown result type (might be due to invalid IL or missing references)
		//IL_4482: Unknown result type (might be due to invalid IL or missing references)
		//IL_4487: Unknown result type (might be due to invalid IL or missing references)
		//IL_44a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_44a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_44c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_44c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_44e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_44ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_4506: Unknown result type (might be due to invalid IL or missing references)
		//IL_450b: Unknown result type (might be due to invalid IL or missing references)
		//IL_4527: Unknown result type (might be due to invalid IL or missing references)
		//IL_452c: Unknown result type (might be due to invalid IL or missing references)
		//IL_453f: Unknown result type (might be due to invalid IL or missing references)
		//IL_4544: Unknown result type (might be due to invalid IL or missing references)
		//IL_4557: Unknown result type (might be due to invalid IL or missing references)
		//IL_455c: Unknown result type (might be due to invalid IL or missing references)
		//IL_456f: Unknown result type (might be due to invalid IL or missing references)
		//IL_4574: Unknown result type (might be due to invalid IL or missing references)
		//IL_458a: Unknown result type (might be due to invalid IL or missing references)
		//IL_458f: Unknown result type (might be due to invalid IL or missing references)
		//IL_45a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_45aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_45c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_45cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_45e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_45e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_4602: Unknown result type (might be due to invalid IL or missing references)
		//IL_4607: Unknown result type (might be due to invalid IL or missing references)
		//IL_461d: Unknown result type (might be due to invalid IL or missing references)
		//IL_4622: Unknown result type (might be due to invalid IL or missing references)
		//IL_4638: Unknown result type (might be due to invalid IL or missing references)
		//IL_463d: Unknown result type (might be due to invalid IL or missing references)
		//IL_4659: Unknown result type (might be due to invalid IL or missing references)
		//IL_465e: Unknown result type (might be due to invalid IL or missing references)
		//IL_4674: Unknown result type (might be due to invalid IL or missing references)
		//IL_4679: Unknown result type (might be due to invalid IL or missing references)
		//IL_4695: Unknown result type (might be due to invalid IL or missing references)
		//IL_469a: Unknown result type (might be due to invalid IL or missing references)
		//IL_46b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_46b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_46ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_46d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_46e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_46eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_46fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_4703: Unknown result type (might be due to invalid IL or missing references)
		//IL_4716: Unknown result type (might be due to invalid IL or missing references)
		//IL_471b: Unknown result type (might be due to invalid IL or missing references)
		//IL_472e: Unknown result type (might be due to invalid IL or missing references)
		//IL_4733: Unknown result type (might be due to invalid IL or missing references)
		//IL_4748: Unknown result type (might be due to invalid IL or missing references)
		//IL_474d: Unknown result type (might be due to invalid IL or missing references)
		//IL_4766: Unknown result type (might be due to invalid IL or missing references)
		//IL_476b: Unknown result type (might be due to invalid IL or missing references)
		//IL_4781: Unknown result type (might be due to invalid IL or missing references)
		//IL_4786: Unknown result type (might be due to invalid IL or missing references)
		//IL_479c: Unknown result type (might be due to invalid IL or missing references)
		//IL_47a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_47b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_47b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_47cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_47d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_47e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_47e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_47fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_4801: Unknown result type (might be due to invalid IL or missing references)
		//IL_481d: Unknown result type (might be due to invalid IL or missing references)
		//IL_4822: Unknown result type (might be due to invalid IL or missing references)
		//IL_483e: Unknown result type (might be due to invalid IL or missing references)
		//IL_4843: Unknown result type (might be due to invalid IL or missing references)
		//IL_485c: Unknown result type (might be due to invalid IL or missing references)
		//IL_4861: Unknown result type (might be due to invalid IL or missing references)
		//IL_487d: Unknown result type (might be due to invalid IL or missing references)
		//IL_4882: Unknown result type (might be due to invalid IL or missing references)
		//IL_4895: Unknown result type (might be due to invalid IL or missing references)
		//IL_489a: Unknown result type (might be due to invalid IL or missing references)
		//IL_48b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_48bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_48d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_48d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_48f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_48f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_490a: Unknown result type (might be due to invalid IL or missing references)
		//IL_490f: Unknown result type (might be due to invalid IL or missing references)
		//IL_4922: Unknown result type (might be due to invalid IL or missing references)
		//IL_4927: Unknown result type (might be due to invalid IL or missing references)
		//IL_4943: Unknown result type (might be due to invalid IL or missing references)
		//IL_4948: Unknown result type (might be due to invalid IL or missing references)
		//IL_495e: Unknown result type (might be due to invalid IL or missing references)
		//IL_4963: Unknown result type (might be due to invalid IL or missing references)
		//IL_4979: Unknown result type (might be due to invalid IL or missing references)
		//IL_497e: Unknown result type (might be due to invalid IL or missing references)
		//IL_4994: Unknown result type (might be due to invalid IL or missing references)
		//IL_4999: Unknown result type (might be due to invalid IL or missing references)
		//IL_49b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_49b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_49d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_49d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_49f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_49f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_4a09: Unknown result type (might be due to invalid IL or missing references)
		//IL_4a0e: Unknown result type (might be due to invalid IL or missing references)
		//IL_4a21: Unknown result type (might be due to invalid IL or missing references)
		//IL_4a26: Unknown result type (might be due to invalid IL or missing references)
		//IL_4a39: Unknown result type (might be due to invalid IL or missing references)
		//IL_4a3e: Unknown result type (might be due to invalid IL or missing references)
		//IL_4a51: Unknown result type (might be due to invalid IL or missing references)
		//IL_4a56: Unknown result type (might be due to invalid IL or missing references)
		//IL_4a69: Unknown result type (might be due to invalid IL or missing references)
		//IL_4a6e: Unknown result type (might be due to invalid IL or missing references)
		//IL_4a81: Unknown result type (might be due to invalid IL or missing references)
		//IL_4a86: Unknown result type (might be due to invalid IL or missing references)
		//IL_4a9c: Unknown result type (might be due to invalid IL or missing references)
		//IL_4aa1: Unknown result type (might be due to invalid IL or missing references)
		//IL_4ab7: Unknown result type (might be due to invalid IL or missing references)
		//IL_4abc: Unknown result type (might be due to invalid IL or missing references)
		//IL_4ad2: Unknown result type (might be due to invalid IL or missing references)
		//IL_4ad7: Unknown result type (might be due to invalid IL or missing references)
		//IL_4af0: Unknown result type (might be due to invalid IL or missing references)
		//IL_4af5: Unknown result type (might be due to invalid IL or missing references)
		//IL_4b0e: Unknown result type (might be due to invalid IL or missing references)
		//IL_4b13: Unknown result type (might be due to invalid IL or missing references)
		//IL_4b2f: Unknown result type (might be due to invalid IL or missing references)
		//IL_4b34: Unknown result type (might be due to invalid IL or missing references)
		//IL_4b4a: Unknown result type (might be due to invalid IL or missing references)
		//IL_4b4f: Unknown result type (might be due to invalid IL or missing references)
		//IL_4b65: Unknown result type (might be due to invalid IL or missing references)
		//IL_4b6a: Unknown result type (might be due to invalid IL or missing references)
		//IL_4b80: Unknown result type (might be due to invalid IL or missing references)
		//IL_4b85: Unknown result type (might be due to invalid IL or missing references)
		//IL_4b9e: Unknown result type (might be due to invalid IL or missing references)
		//IL_4ba3: Unknown result type (might be due to invalid IL or missing references)
		//IL_4bbb: Unknown result type (might be due to invalid IL or missing references)
		//IL_4bc0: Unknown result type (might be due to invalid IL or missing references)
		//IL_4bdc: Unknown result type (might be due to invalid IL or missing references)
		//IL_4be1: Unknown result type (might be due to invalid IL or missing references)
		//IL_4bf6: Unknown result type (might be due to invalid IL or missing references)
		//IL_4bfb: Unknown result type (might be due to invalid IL or missing references)
		//IL_4c0e: Unknown result type (might be due to invalid IL or missing references)
		//IL_4c13: Unknown result type (might be due to invalid IL or missing references)
		//IL_4c2f: Unknown result type (might be due to invalid IL or missing references)
		//IL_4c34: Unknown result type (might be due to invalid IL or missing references)
		//IL_4c4d: Unknown result type (might be due to invalid IL or missing references)
		//IL_4c52: Unknown result type (might be due to invalid IL or missing references)
		//IL_4c6a: Unknown result type (might be due to invalid IL or missing references)
		//IL_4c6f: Unknown result type (might be due to invalid IL or missing references)
		//IL_4c87: Unknown result type (might be due to invalid IL or missing references)
		//IL_4c8c: Unknown result type (might be due to invalid IL or missing references)
		//IL_4ca2: Unknown result type (might be due to invalid IL or missing references)
		//IL_4ca7: Unknown result type (might be due to invalid IL or missing references)
		//IL_4cbd: Unknown result type (might be due to invalid IL or missing references)
		//IL_4cc2: Unknown result type (might be due to invalid IL or missing references)
		//IL_4cd7: Unknown result type (might be due to invalid IL or missing references)
		//IL_4cdc: Unknown result type (might be due to invalid IL or missing references)
		//IL_4cf5: Unknown result type (might be due to invalid IL or missing references)
		//IL_4cfa: Unknown result type (might be due to invalid IL or missing references)
		//IL_4d16: Unknown result type (might be due to invalid IL or missing references)
		//IL_4d1b: Unknown result type (might be due to invalid IL or missing references)
		//IL_4d31: Unknown result type (might be due to invalid IL or missing references)
		//IL_4d36: Unknown result type (might be due to invalid IL or missing references)
		//IL_4d4c: Unknown result type (might be due to invalid IL or missing references)
		//IL_4d51: Unknown result type (might be due to invalid IL or missing references)
		//IL_4d67: Unknown result type (might be due to invalid IL or missing references)
		//IL_4d6c: Unknown result type (might be due to invalid IL or missing references)
		//IL_4d80: Unknown result type (might be due to invalid IL or missing references)
		//IL_4d85: Unknown result type (might be due to invalid IL or missing references)
		//IL_4d99: Unknown result type (might be due to invalid IL or missing references)
		//IL_4d9e: Unknown result type (might be due to invalid IL or missing references)
		//IL_4db2: Unknown result type (might be due to invalid IL or missing references)
		//IL_4db7: Unknown result type (might be due to invalid IL or missing references)
		//IL_4dd3: Unknown result type (might be due to invalid IL or missing references)
		//IL_4dd8: Unknown result type (might be due to invalid IL or missing references)
		//IL_4df1: Unknown result type (might be due to invalid IL or missing references)
		//IL_4df6: Unknown result type (might be due to invalid IL or missing references)
		//IL_4e09: Unknown result type (might be due to invalid IL or missing references)
		//IL_4e0e: Unknown result type (might be due to invalid IL or missing references)
		//IL_4e21: Unknown result type (might be due to invalid IL or missing references)
		//IL_4e26: Unknown result type (might be due to invalid IL or missing references)
		//IL_4e3f: Unknown result type (might be due to invalid IL or missing references)
		//IL_4e44: Unknown result type (might be due to invalid IL or missing references)
		//IL_4e5d: Unknown result type (might be due to invalid IL or missing references)
		//IL_4e62: Unknown result type (might be due to invalid IL or missing references)
		//IL_4e7b: Unknown result type (might be due to invalid IL or missing references)
		//IL_4e80: Unknown result type (might be due to invalid IL or missing references)
		//IL_4e9c: Unknown result type (might be due to invalid IL or missing references)
		//IL_4ea1: Unknown result type (might be due to invalid IL or missing references)
		//IL_4eba: Unknown result type (might be due to invalid IL or missing references)
		//IL_4ebf: Unknown result type (might be due to invalid IL or missing references)
		//IL_4ed5: Unknown result type (might be due to invalid IL or missing references)
		//IL_4eda: Unknown result type (might be due to invalid IL or missing references)
		//IL_4eed: Unknown result type (might be due to invalid IL or missing references)
		//IL_4ef2: Unknown result type (might be due to invalid IL or missing references)
		//IL_4eff: Unknown result type (might be due to invalid IL or missing references)
		//IL_4f04: Unknown result type (might be due to invalid IL or missing references)
		//IL_4f11: Unknown result type (might be due to invalid IL or missing references)
		//IL_4f16: Unknown result type (might be due to invalid IL or missing references)
		//IL_4f32: Unknown result type (might be due to invalid IL or missing references)
		//IL_4f37: Unknown result type (might be due to invalid IL or missing references)
		//IL_4f4f: Unknown result type (might be due to invalid IL or missing references)
		//IL_4f54: Unknown result type (might be due to invalid IL or missing references)
		//IL_4f6d: Unknown result type (might be due to invalid IL or missing references)
		//IL_4f72: Unknown result type (might be due to invalid IL or missing references)
		//IL_4f8b: Unknown result type (might be due to invalid IL or missing references)
		//IL_4f90: Unknown result type (might be due to invalid IL or missing references)
		//IL_4fac: Unknown result type (might be due to invalid IL or missing references)
		//IL_4fb1: Unknown result type (might be due to invalid IL or missing references)
		//IL_4fc9: Unknown result type (might be due to invalid IL or missing references)
		//IL_4fce: Unknown result type (might be due to invalid IL or missing references)
		//IL_4fe7: Unknown result type (might be due to invalid IL or missing references)
		//IL_4fec: Unknown result type (might be due to invalid IL or missing references)
		//IL_5002: Unknown result type (might be due to invalid IL or missing references)
		//IL_5007: Unknown result type (might be due to invalid IL or missing references)
		//IL_5020: Unknown result type (might be due to invalid IL or missing references)
		//IL_5025: Unknown result type (might be due to invalid IL or missing references)
		//IL_5041: Unknown result type (might be due to invalid IL or missing references)
		//IL_5046: Unknown result type (might be due to invalid IL or missing references)
		//IL_5062: Unknown result type (might be due to invalid IL or missing references)
		//IL_5067: Unknown result type (might be due to invalid IL or missing references)
		//IL_507f: Unknown result type (might be due to invalid IL or missing references)
		//IL_5084: Unknown result type (might be due to invalid IL or missing references)
		//IL_509d: Unknown result type (might be due to invalid IL or missing references)
		//IL_50a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_50bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_50c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_50d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_50db: Unknown result type (might be due to invalid IL or missing references)
		//IL_50f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_50fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_5118: Unknown result type (might be due to invalid IL or missing references)
		//IL_511d: Unknown result type (might be due to invalid IL or missing references)
		//IL_5139: Unknown result type (might be due to invalid IL or missing references)
		//IL_513e: Unknown result type (might be due to invalid IL or missing references)
		//IL_5157: Unknown result type (might be due to invalid IL or missing references)
		//IL_515c: Unknown result type (might be due to invalid IL or missing references)
		//IL_5178: Unknown result type (might be due to invalid IL or missing references)
		//IL_517d: Unknown result type (might be due to invalid IL or missing references)
		//IL_5199: Unknown result type (might be due to invalid IL or missing references)
		//IL_519e: Unknown result type (might be due to invalid IL or missing references)
		//IL_51b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_51bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_51d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_51dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_51f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_51fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_5211: Unknown result type (might be due to invalid IL or missing references)
		//IL_5216: Unknown result type (might be due to invalid IL or missing references)
		//IL_5232: Unknown result type (might be due to invalid IL or missing references)
		//IL_5237: Unknown result type (might be due to invalid IL or missing references)
		//IL_5253: Unknown result type (might be due to invalid IL or missing references)
		//IL_5258: Unknown result type (might be due to invalid IL or missing references)
		//IL_5271: Unknown result type (might be due to invalid IL or missing references)
		//IL_5276: Unknown result type (might be due to invalid IL or missing references)
		//IL_528c: Unknown result type (might be due to invalid IL or missing references)
		//IL_5291: Unknown result type (might be due to invalid IL or missing references)
		//IL_52aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_52af: Unknown result type (might be due to invalid IL or missing references)
		//IL_52c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_52ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_52dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_52e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_52fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_5300: Unknown result type (might be due to invalid IL or missing references)
		//IL_5319: Unknown result type (might be due to invalid IL or missing references)
		//IL_531e: Unknown result type (might be due to invalid IL or missing references)
		//IL_5337: Unknown result type (might be due to invalid IL or missing references)
		//IL_533c: Unknown result type (might be due to invalid IL or missing references)
		//IL_5358: Unknown result type (might be due to invalid IL or missing references)
		//IL_535d: Unknown result type (might be due to invalid IL or missing references)
		//IL_5376: Unknown result type (might be due to invalid IL or missing references)
		//IL_537b: Unknown result type (might be due to invalid IL or missing references)
		//IL_5398: Unknown result type (might be due to invalid IL or missing references)
		//IL_539d: Unknown result type (might be due to invalid IL or missing references)
		//IL_53b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_53bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_53d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_53db: Unknown result type (might be due to invalid IL or missing references)
		//IL_53ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_53f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_540f: Unknown result type (might be due to invalid IL or missing references)
		//IL_5414: Unknown result type (might be due to invalid IL or missing references)
		//IL_543c: Unknown result type (might be due to invalid IL or missing references)
		//IL_5441: Unknown result type (might be due to invalid IL or missing references)
		//IL_5454: Unknown result type (might be due to invalid IL or missing references)
		//IL_5459: Unknown result type (might be due to invalid IL or missing references)
		//IL_546c: Unknown result type (might be due to invalid IL or missing references)
		//IL_5471: Unknown result type (might be due to invalid IL or missing references)
		//IL_5484: Unknown result type (might be due to invalid IL or missing references)
		//IL_5489: Unknown result type (might be due to invalid IL or missing references)
		//IL_549c: Unknown result type (might be due to invalid IL or missing references)
		//IL_54a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_54b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_54bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_54d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_54da: Unknown result type (might be due to invalid IL or missing references)
		//IL_54f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_54f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_5511: Unknown result type (might be due to invalid IL or missing references)
		//IL_5516: Unknown result type (might be due to invalid IL or missing references)
		//IL_5532: Unknown result type (might be due to invalid IL or missing references)
		//IL_5537: Unknown result type (might be due to invalid IL or missing references)
		//IL_554d: Unknown result type (might be due to invalid IL or missing references)
		//IL_5552: Unknown result type (might be due to invalid IL or missing references)
		//IL_5568: Unknown result type (might be due to invalid IL or missing references)
		//IL_556d: Unknown result type (might be due to invalid IL or missing references)
		//IL_5586: Unknown result type (might be due to invalid IL or missing references)
		//IL_558b: Unknown result type (might be due to invalid IL or missing references)
		//IL_55a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_55a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_55c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_55c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_55e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_55e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_5604: Unknown result type (might be due to invalid IL or missing references)
		//IL_5609: Unknown result type (might be due to invalid IL or missing references)
		//IL_5625: Unknown result type (might be due to invalid IL or missing references)
		//IL_562a: Unknown result type (might be due to invalid IL or missing references)
		//IL_5646: Unknown result type (might be due to invalid IL or missing references)
		//IL_564b: Unknown result type (might be due to invalid IL or missing references)
		//IL_5667: Unknown result type (might be due to invalid IL or missing references)
		//IL_566c: Unknown result type (might be due to invalid IL or missing references)
		//IL_5685: Unknown result type (might be due to invalid IL or missing references)
		//IL_568a: Unknown result type (might be due to invalid IL or missing references)
		//IL_56a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_56a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_56c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_56c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_56e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_56e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_56fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_56ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_5718: Unknown result type (might be due to invalid IL or missing references)
		//IL_571d: Unknown result type (might be due to invalid IL or missing references)
		//IL_5733: Unknown result type (might be due to invalid IL or missing references)
		//IL_5738: Unknown result type (might be due to invalid IL or missing references)
		//IL_5754: Unknown result type (might be due to invalid IL or missing references)
		//IL_5759: Unknown result type (might be due to invalid IL or missing references)
		//IL_5772: Unknown result type (might be due to invalid IL or missing references)
		//IL_5777: Unknown result type (might be due to invalid IL or missing references)
		//IL_578c: Unknown result type (might be due to invalid IL or missing references)
		//IL_5791: Unknown result type (might be due to invalid IL or missing references)
		//IL_57aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_57af: Unknown result type (might be due to invalid IL or missing references)
		//IL_57c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_57ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_57e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_57e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_57fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_57ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_5814: Unknown result type (might be due to invalid IL or missing references)
		//IL_5819: Unknown result type (might be due to invalid IL or missing references)
		//IL_582e: Unknown result type (might be due to invalid IL or missing references)
		//IL_5833: Unknown result type (might be due to invalid IL or missing references)
		//IL_5848: Unknown result type (might be due to invalid IL or missing references)
		//IL_584d: Unknown result type (might be due to invalid IL or missing references)
		//IL_5860: Unknown result type (might be due to invalid IL or missing references)
		//IL_5865: Unknown result type (might be due to invalid IL or missing references)
		//IL_5878: Unknown result type (might be due to invalid IL or missing references)
		//IL_587d: Unknown result type (might be due to invalid IL or missing references)
		//IL_5890: Unknown result type (might be due to invalid IL or missing references)
		//IL_5895: Unknown result type (might be due to invalid IL or missing references)
		//IL_58a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_58ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_58c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_58cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_58e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_58e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_5902: Unknown result type (might be due to invalid IL or missing references)
		//IL_5907: Unknown result type (might be due to invalid IL or missing references)
		//IL_5923: Unknown result type (might be due to invalid IL or missing references)
		//IL_5928: Unknown result type (might be due to invalid IL or missing references)
		//IL_5944: Unknown result type (might be due to invalid IL or missing references)
		//IL_5949: Unknown result type (might be due to invalid IL or missing references)
		//IL_5965: Unknown result type (might be due to invalid IL or missing references)
		//IL_596a: Unknown result type (might be due to invalid IL or missing references)
		//IL_5980: Unknown result type (might be due to invalid IL or missing references)
		//IL_5985: Unknown result type (might be due to invalid IL or missing references)
		//IL_599e: Unknown result type (might be due to invalid IL or missing references)
		//IL_59a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_59bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_59c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_59dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_59e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_59fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_5a00: Unknown result type (might be due to invalid IL or missing references)
		//IL_5a19: Unknown result type (might be due to invalid IL or missing references)
		//IL_5a1e: Unknown result type (might be due to invalid IL or missing references)
		//IL_5a3a: Unknown result type (might be due to invalid IL or missing references)
		//IL_5a3f: Unknown result type (might be due to invalid IL or missing references)
		//IL_5a55: Unknown result type (might be due to invalid IL or missing references)
		//IL_5a5a: Unknown result type (might be due to invalid IL or missing references)
		//IL_5a76: Unknown result type (might be due to invalid IL or missing references)
		//IL_5a7b: Unknown result type (might be due to invalid IL or missing references)
		//IL_5a92: Unknown result type (might be due to invalid IL or missing references)
		//IL_5a97: Unknown result type (might be due to invalid IL or missing references)
		//IL_5ab4: Unknown result type (might be due to invalid IL or missing references)
		//IL_5ab9: Unknown result type (might be due to invalid IL or missing references)
		//IL_5ad2: Unknown result type (might be due to invalid IL or missing references)
		//IL_5ad7: Unknown result type (might be due to invalid IL or missing references)
		//IL_5ae7: Unknown result type (might be due to invalid IL or missing references)
		//IL_5af4: Unknown result type (might be due to invalid IL or missing references)
		//IL_5af6: Unknown result type (might be due to invalid IL or missing references)
		//IL_5b03: Unknown result type (might be due to invalid IL or missing references)
		//IL_5b05: Unknown result type (might be due to invalid IL or missing references)
		//IL_5b12: Unknown result type (might be due to invalid IL or missing references)
		//IL_5b14: Unknown result type (might be due to invalid IL or missing references)
		//IL_5b30: Unknown result type (might be due to invalid IL or missing references)
		//IL_5b35: Unknown result type (might be due to invalid IL or missing references)
		//IL_5b51: Unknown result type (might be due to invalid IL or missing references)
		//IL_5b56: Unknown result type (might be due to invalid IL or missing references)
		//IL_5b6c: Unknown result type (might be due to invalid IL or missing references)
		//IL_5b71: Unknown result type (might be due to invalid IL or missing references)
		//IL_5b84: Unknown result type (might be due to invalid IL or missing references)
		//IL_5b89: Unknown result type (might be due to invalid IL or missing references)
		//IL_5ba5: Unknown result type (might be due to invalid IL or missing references)
		//IL_5baa: Unknown result type (might be due to invalid IL or missing references)
		//IL_5bc6: Unknown result type (might be due to invalid IL or missing references)
		//IL_5bcb: Unknown result type (might be due to invalid IL or missing references)
		//IL_5be7: Unknown result type (might be due to invalid IL or missing references)
		//IL_5bec: Unknown result type (might be due to invalid IL or missing references)
		//IL_5c05: Unknown result type (might be due to invalid IL or missing references)
		//IL_5c0a: Unknown result type (might be due to invalid IL or missing references)
		//IL_5c23: Unknown result type (might be due to invalid IL or missing references)
		//IL_5c28: Unknown result type (might be due to invalid IL or missing references)
		//IL_5c41: Unknown result type (might be due to invalid IL or missing references)
		//IL_5c46: Unknown result type (might be due to invalid IL or missing references)
		//IL_5c5c: Unknown result type (might be due to invalid IL or missing references)
		//IL_5c61: Unknown result type (might be due to invalid IL or missing references)
		//IL_5c7a: Unknown result type (might be due to invalid IL or missing references)
		//IL_5c7f: Unknown result type (might be due to invalid IL or missing references)
		//IL_5c9b: Unknown result type (might be due to invalid IL or missing references)
		//IL_5ca0: Unknown result type (might be due to invalid IL or missing references)
		//IL_5cb3: Unknown result type (might be due to invalid IL or missing references)
		//IL_5cb8: Unknown result type (might be due to invalid IL or missing references)
		//IL_5cd4: Unknown result type (might be due to invalid IL or missing references)
		//IL_5cd9: Unknown result type (might be due to invalid IL or missing references)
		//IL_5cef: Unknown result type (might be due to invalid IL or missing references)
		//IL_5cf4: Unknown result type (might be due to invalid IL or missing references)
		//IL_5d01: Unknown result type (might be due to invalid IL or missing references)
		//IL_5d06: Unknown result type (might be due to invalid IL or missing references)
		//IL_5d1c: Unknown result type (might be due to invalid IL or missing references)
		//IL_5d21: Unknown result type (might be due to invalid IL or missing references)
		//IL_5d3a: Unknown result type (might be due to invalid IL or missing references)
		//IL_5d3f: Unknown result type (might be due to invalid IL or missing references)
		//IL_5d5b: Unknown result type (might be due to invalid IL or missing references)
		//IL_5d60: Unknown result type (might be due to invalid IL or missing references)
		//IL_5d73: Unknown result type (might be due to invalid IL or missing references)
		//IL_5d78: Unknown result type (might be due to invalid IL or missing references)
		//IL_5d8b: Unknown result type (might be due to invalid IL or missing references)
		//IL_5d90: Unknown result type (might be due to invalid IL or missing references)
		//IL_5da3: Unknown result type (might be due to invalid IL or missing references)
		//IL_5da8: Unknown result type (might be due to invalid IL or missing references)
		//IL_5dc1: Unknown result type (might be due to invalid IL or missing references)
		//IL_5dc6: Unknown result type (might be due to invalid IL or missing references)
		//IL_5ddb: Unknown result type (might be due to invalid IL or missing references)
		//IL_5de0: Unknown result type (might be due to invalid IL or missing references)
		//IL_5df5: Unknown result type (might be due to invalid IL or missing references)
		//IL_5dff: Unknown result type (might be due to invalid IL or missing references)
		//IL_5e04: Unknown result type (might be due to invalid IL or missing references)
		//IL_5e16: Unknown result type (might be due to invalid IL or missing references)
		//IL_5e20: Unknown result type (might be due to invalid IL or missing references)
		//IL_5e25: Unknown result type (might be due to invalid IL or missing references)
		//IL_5e3a: Unknown result type (might be due to invalid IL or missing references)
		//IL_5e44: Unknown result type (might be due to invalid IL or missing references)
		//IL_5e49: Unknown result type (might be due to invalid IL or missing references)
		//IL_5e60: Unknown result type (might be due to invalid IL or missing references)
		//IL_5e65: Unknown result type (might be due to invalid IL or missing references)
		//IL_5e74: Unknown result type (might be due to invalid IL or missing references)
		//IL_5e79: Unknown result type (might be due to invalid IL or missing references)
		//IL_5e8c: Unknown result type (might be due to invalid IL or missing references)
		//IL_5e91: Unknown result type (might be due to invalid IL or missing references)
		//IL_5ea4: Unknown result type (might be due to invalid IL or missing references)
		//IL_5ea9: Unknown result type (might be due to invalid IL or missing references)
		//IL_5ee8: Unknown result type (might be due to invalid IL or missing references)
		//IL_5eed: Unknown result type (might be due to invalid IL or missing references)
		//IL_5f06: Unknown result type (might be due to invalid IL or missing references)
		//IL_5f0b: Unknown result type (might be due to invalid IL or missing references)
		//IL_5f21: Unknown result type (might be due to invalid IL or missing references)
		//IL_5f26: Unknown result type (might be due to invalid IL or missing references)
		//IL_5f3c: Unknown result type (might be due to invalid IL or missing references)
		//IL_5f41: Unknown result type (might be due to invalid IL or missing references)
		//IL_5f57: Unknown result type (might be due to invalid IL or missing references)
		//IL_5f5c: Unknown result type (might be due to invalid IL or missing references)
		//IL_5f6f: Unknown result type (might be due to invalid IL or missing references)
		//IL_5f74: Unknown result type (might be due to invalid IL or missing references)
		//IL_5f8a: Unknown result type (might be due to invalid IL or missing references)
		//IL_5f8f: Unknown result type (might be due to invalid IL or missing references)
		//IL_5fa8: Unknown result type (might be due to invalid IL or missing references)
		//IL_5fad: Unknown result type (might be due to invalid IL or missing references)
		//IL_5fc6: Unknown result type (might be due to invalid IL or missing references)
		//IL_5fcb: Unknown result type (might be due to invalid IL or missing references)
		//IL_5fe1: Unknown result type (might be due to invalid IL or missing references)
		//IL_5fe6: Unknown result type (might be due to invalid IL or missing references)
		//IL_5ffc: Unknown result type (might be due to invalid IL or missing references)
		//IL_6001: Unknown result type (might be due to invalid IL or missing references)
		//IL_6017: Unknown result type (might be due to invalid IL or missing references)
		//IL_601c: Unknown result type (might be due to invalid IL or missing references)
		//IL_6038: Unknown result type (might be due to invalid IL or missing references)
		//IL_603d: Unknown result type (might be due to invalid IL or missing references)
		//IL_6056: Unknown result type (might be due to invalid IL or missing references)
		//IL_605b: Unknown result type (might be due to invalid IL or missing references)
		//IL_606b: Unknown result type (might be due to invalid IL or missing references)
		//IL_6070: Unknown result type (might be due to invalid IL or missing references)
		//IL_6083: Unknown result type (might be due to invalid IL or missing references)
		//IL_6088: Unknown result type (might be due to invalid IL or missing references)
		//IL_609b: Unknown result type (might be due to invalid IL or missing references)
		//IL_60a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_60b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_60bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_60cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_60d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_60e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_60ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_60fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_6101: Unknown result type (might be due to invalid IL or missing references)
		//IL_6114: Unknown result type (might be due to invalid IL or missing references)
		//IL_6119: Unknown result type (might be due to invalid IL or missing references)
		//IL_612c: Unknown result type (might be due to invalid IL or missing references)
		//IL_6131: Unknown result type (might be due to invalid IL or missing references)
		//IL_614d: Unknown result type (might be due to invalid IL or missing references)
		//IL_6152: Unknown result type (might be due to invalid IL or missing references)
		//IL_6165: Unknown result type (might be due to invalid IL or missing references)
		//IL_616a: Unknown result type (might be due to invalid IL or missing references)
		//IL_6183: Unknown result type (might be due to invalid IL or missing references)
		//IL_6188: Unknown result type (might be due to invalid IL or missing references)
		//IL_61a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_61a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_61b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_61be: Unknown result type (might be due to invalid IL or missing references)
		//IL_61d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_61d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_61e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_61ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_61ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_61f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_61fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_6204: Unknown result type (might be due to invalid IL or missing references)
		//IL_6206: Unknown result type (might be due to invalid IL or missing references)
		//IL_6210: Unknown result type (might be due to invalid IL or missing references)
		//IL_6212: Unknown result type (might be due to invalid IL or missing references)
		//IL_621c: Unknown result type (might be due to invalid IL or missing references)
		//IL_621e: Unknown result type (might be due to invalid IL or missing references)
		//IL_6228: Unknown result type (might be due to invalid IL or missing references)
		//IL_622a: Unknown result type (might be due to invalid IL or missing references)
		//IL_6234: Unknown result type (might be due to invalid IL or missing references)
		//IL_6236: Unknown result type (might be due to invalid IL or missing references)
		//IL_6240: Unknown result type (might be due to invalid IL or missing references)
		//IL_6242: Unknown result type (might be due to invalid IL or missing references)
		//IL_624f: Unknown result type (might be due to invalid IL or missing references)
		//IL_6251: Unknown result type (might be due to invalid IL or missing references)
		//IL_625a: Unknown result type (might be due to invalid IL or missing references)
		//IL_625c: Unknown result type (might be due to invalid IL or missing references)
		//IL_6269: Unknown result type (might be due to invalid IL or missing references)
		//IL_626b: Unknown result type (might be due to invalid IL or missing references)
		//IL_627f: Unknown result type (might be due to invalid IL or missing references)
		//IL_6284: Unknown result type (might be due to invalid IL or missing references)
		//IL_6291: Unknown result type (might be due to invalid IL or missing references)
		//IL_629a: Unknown result type (might be due to invalid IL or missing references)
		//IL_629c: Unknown result type (might be due to invalid IL or missing references)
		//IL_62a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_62a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_62b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_62b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_62c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_62c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_62d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_62dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_62ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_62f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_6303: Unknown result type (might be due to invalid IL or missing references)
		//IL_6308: Unknown result type (might be due to invalid IL or missing references)
		//IL_6315: Unknown result type (might be due to invalid IL or missing references)
		//IL_631e: Unknown result type (might be due to invalid IL or missing references)
		//IL_6320: Unknown result type (might be due to invalid IL or missing references)
		//IL_632a: Unknown result type (might be due to invalid IL or missing references)
		//IL_632c: Unknown result type (might be due to invalid IL or missing references)
		//IL_6339: Unknown result type (might be due to invalid IL or missing references)
		//IL_633b: Unknown result type (might be due to invalid IL or missing references)
		//IL_6348: Unknown result type (might be due to invalid IL or missing references)
		//IL_6352: Unknown result type (might be due to invalid IL or missing references)
		//IL_6354: Unknown result type (might be due to invalid IL or missing references)
		//IL_635e: Unknown result type (might be due to invalid IL or missing references)
		//IL_6360: Unknown result type (might be due to invalid IL or missing references)
		//IL_636d: Unknown result type (might be due to invalid IL or missing references)
		//IL_6377: Unknown result type (might be due to invalid IL or missing references)
		//IL_6379: Unknown result type (might be due to invalid IL or missing references)
		//IL_6383: Unknown result type (might be due to invalid IL or missing references)
		//IL_6385: Unknown result type (might be due to invalid IL or missing references)
		//IL_6392: Unknown result type (might be due to invalid IL or missing references)
		//IL_639b: Unknown result type (might be due to invalid IL or missing references)
		//IL_639d: Unknown result type (might be due to invalid IL or missing references)
		//IL_63a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_63a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_63b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_63b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_63c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_63cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_63d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_63db: Unknown result type (might be due to invalid IL or missing references)
		//IL_63dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_63ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_63f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_63f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_6400: Unknown result type (might be due to invalid IL or missing references)
		//IL_6402: Unknown result type (might be due to invalid IL or missing references)
		//IL_640f: Unknown result type (might be due to invalid IL or missing references)
		//IL_6419: Unknown result type (might be due to invalid IL or missing references)
		//IL_641b: Unknown result type (might be due to invalid IL or missing references)
		//IL_6425: Unknown result type (might be due to invalid IL or missing references)
		//IL_6427: Unknown result type (might be due to invalid IL or missing references)
		//IL_6434: Unknown result type (might be due to invalid IL or missing references)
		//IL_6436: Unknown result type (might be due to invalid IL or missing references)
		//IL_6443: Unknown result type (might be due to invalid IL or missing references)
		//IL_644d: Unknown result type (might be due to invalid IL or missing references)
		//IL_644f: Unknown result type (might be due to invalid IL or missing references)
		//IL_6459: Unknown result type (might be due to invalid IL or missing references)
		//IL_645b: Unknown result type (might be due to invalid IL or missing references)
		//IL_6468: Unknown result type (might be due to invalid IL or missing references)
		//IL_6472: Unknown result type (might be due to invalid IL or missing references)
		//IL_6474: Unknown result type (might be due to invalid IL or missing references)
		//IL_647e: Unknown result type (might be due to invalid IL or missing references)
		//IL_6480: Unknown result type (might be due to invalid IL or missing references)
		//IL_6490: Unknown result type (might be due to invalid IL or missing references)
		//IL_6495: Unknown result type (might be due to invalid IL or missing references)
		//IL_64a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_64ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_64bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_64c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_64d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_64da: Unknown result type (might be due to invalid IL or missing references)
		//IL_64ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_64ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_6502: Unknown result type (might be due to invalid IL or missing references)
		//IL_6507: Unknown result type (might be due to invalid IL or missing references)
		//IL_6517: Unknown result type (might be due to invalid IL or missing references)
		//IL_651c: Unknown result type (might be due to invalid IL or missing references)
		//IL_652f: Unknown result type (might be due to invalid IL or missing references)
		//IL_6534: Unknown result type (might be due to invalid IL or missing references)
		//IL_6541: Unknown result type (might be due to invalid IL or missing references)
		//IL_654b: Unknown result type (might be due to invalid IL or missing references)
		//IL_654d: Unknown result type (might be due to invalid IL or missing references)
		//IL_655a: Unknown result type (might be due to invalid IL or missing references)
		//IL_655c: Unknown result type (might be due to invalid IL or missing references)
		//IL_6566: Unknown result type (might be due to invalid IL or missing references)
		//IL_6568: Unknown result type (might be due to invalid IL or missing references)
		//IL_6578: Unknown result type (might be due to invalid IL or missing references)
		//IL_657d: Unknown result type (might be due to invalid IL or missing references)
		//IL_658d: Unknown result type (might be due to invalid IL or missing references)
		//IL_6592: Unknown result type (might be due to invalid IL or missing references)
		//IL_65a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_65a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_65b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_65bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_65cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_65d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_65e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_65e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_65f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_65fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_6611: Unknown result type (might be due to invalid IL or missing references)
		//IL_6616: Unknown result type (might be due to invalid IL or missing references)
		//IL_6626: Unknown result type (might be due to invalid IL or missing references)
		//IL_662b: Unknown result type (might be due to invalid IL or missing references)
		//IL_663b: Unknown result type (might be due to invalid IL or missing references)
		//IL_6640: Unknown result type (might be due to invalid IL or missing references)
		//IL_6650: Unknown result type (might be due to invalid IL or missing references)
		//IL_6655: Unknown result type (might be due to invalid IL or missing references)
		//IL_6665: Unknown result type (might be due to invalid IL or missing references)
		//IL_666a: Unknown result type (might be due to invalid IL or missing references)
		//IL_667a: Unknown result type (might be due to invalid IL or missing references)
		//IL_667f: Unknown result type (might be due to invalid IL or missing references)
		//IL_668f: Unknown result type (might be due to invalid IL or missing references)
		//IL_6694: Unknown result type (might be due to invalid IL or missing references)
		//IL_66a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_66ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_66ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_66b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_66b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_66c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_66ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_66de: Unknown result type (might be due to invalid IL or missing references)
		//IL_66e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_66f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_66f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_6708: Unknown result type (might be due to invalid IL or missing references)
		//IL_670d: Unknown result type (might be due to invalid IL or missing references)
		//IL_671d: Unknown result type (might be due to invalid IL or missing references)
		//IL_6722: Unknown result type (might be due to invalid IL or missing references)
		//IL_6732: Unknown result type (might be due to invalid IL or missing references)
		//IL_6737: Unknown result type (might be due to invalid IL or missing references)
		//IL_6747: Unknown result type (might be due to invalid IL or missing references)
		//IL_674c: Unknown result type (might be due to invalid IL or missing references)
		//IL_675c: Unknown result type (might be due to invalid IL or missing references)
		//IL_6761: Unknown result type (might be due to invalid IL or missing references)
		//IL_6771: Unknown result type (might be due to invalid IL or missing references)
		//IL_6776: Unknown result type (might be due to invalid IL or missing references)
		//IL_6786: Unknown result type (might be due to invalid IL or missing references)
		//IL_678b: Unknown result type (might be due to invalid IL or missing references)
		//IL_679b: Unknown result type (might be due to invalid IL or missing references)
		//IL_67a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_67b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_67b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_67c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_67ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_67da: Unknown result type (might be due to invalid IL or missing references)
		//IL_67df: Unknown result type (might be due to invalid IL or missing references)
		//IL_67ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_67f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_6804: Unknown result type (might be due to invalid IL or missing references)
		//IL_6809: Unknown result type (might be due to invalid IL or missing references)
		//IL_6819: Unknown result type (might be due to invalid IL or missing references)
		//IL_681e: Unknown result type (might be due to invalid IL or missing references)
		//IL_682e: Unknown result type (might be due to invalid IL or missing references)
		//IL_6833: Unknown result type (might be due to invalid IL or missing references)
		//IL_6842: Unknown result type (might be due to invalid IL or missing references)
		//IL_6847: Unknown result type (might be due to invalid IL or missing references)
		//IL_6857: Unknown result type (might be due to invalid IL or missing references)
		//IL_685c: Unknown result type (might be due to invalid IL or missing references)
		//IL_686c: Unknown result type (might be due to invalid IL or missing references)
		//IL_6871: Unknown result type (might be due to invalid IL or missing references)
		//IL_6881: Unknown result type (might be due to invalid IL or missing references)
		//IL_6886: Unknown result type (might be due to invalid IL or missing references)
		//IL_6896: Unknown result type (might be due to invalid IL or missing references)
		//IL_689b: Unknown result type (might be due to invalid IL or missing references)
		//IL_68ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_68b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_68c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_68ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_68dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_68e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_68f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_68f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_690b: Unknown result type (might be due to invalid IL or missing references)
		//IL_6910: Unknown result type (might be due to invalid IL or missing references)
		//IL_691d: Unknown result type (might be due to invalid IL or missing references)
		//IL_6927: Unknown result type (might be due to invalid IL or missing references)
		//IL_6929: Unknown result type (might be due to invalid IL or missing references)
		//IL_6933: Unknown result type (might be due to invalid IL or missing references)
		//IL_6935: Unknown result type (might be due to invalid IL or missing references)
		//IL_693f: Unknown result type (might be due to invalid IL or missing references)
		//IL_6941: Unknown result type (might be due to invalid IL or missing references)
		//IL_694b: Unknown result type (might be due to invalid IL or missing references)
		//IL_694d: Unknown result type (might be due to invalid IL or missing references)
		//IL_695a: Unknown result type (might be due to invalid IL or missing references)
		//IL_6964: Unknown result type (might be due to invalid IL or missing references)
		//IL_6966: Unknown result type (might be due to invalid IL or missing references)
		//IL_6970: Unknown result type (might be due to invalid IL or missing references)
		//IL_6972: Unknown result type (might be due to invalid IL or missing references)
		//IL_6982: Unknown result type (might be due to invalid IL or missing references)
		//IL_6987: Unknown result type (might be due to invalid IL or missing references)
		//IL_699a: Unknown result type (might be due to invalid IL or missing references)
		//IL_699f: Unknown result type (might be due to invalid IL or missing references)
		//IL_69b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_69b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_69c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_69cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_69e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_69ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_69f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_6a01: Unknown result type (might be due to invalid IL or missing references)
		//IL_6a03: Unknown result type (might be due to invalid IL or missing references)
		//IL_6a0d: Unknown result type (might be due to invalid IL or missing references)
		//IL_6a0f: Unknown result type (might be due to invalid IL or missing references)
		//IL_6a1f: Unknown result type (might be due to invalid IL or missing references)
		//IL_6a24: Unknown result type (might be due to invalid IL or missing references)
		//IL_6a34: Unknown result type (might be due to invalid IL or missing references)
		//IL_6a39: Unknown result type (might be due to invalid IL or missing references)
		//IL_6a49: Unknown result type (might be due to invalid IL or missing references)
		//IL_6a4e: Unknown result type (might be due to invalid IL or missing references)
		//IL_6a5e: Unknown result type (might be due to invalid IL or missing references)
		//IL_6a63: Unknown result type (might be due to invalid IL or missing references)
		//IL_6a73: Unknown result type (might be due to invalid IL or missing references)
		//IL_6a78: Unknown result type (might be due to invalid IL or missing references)
		//IL_6a88: Unknown result type (might be due to invalid IL or missing references)
		//IL_6a8d: Unknown result type (might be due to invalid IL or missing references)
		//IL_6a9d: Unknown result type (might be due to invalid IL or missing references)
		//IL_6aa2: Unknown result type (might be due to invalid IL or missing references)
		//IL_6ab5: Unknown result type (might be due to invalid IL or missing references)
		//IL_6aba: Unknown result type (might be due to invalid IL or missing references)
		//IL_6aca: Unknown result type (might be due to invalid IL or missing references)
		//IL_6acf: Unknown result type (might be due to invalid IL or missing references)
		//IL_6adf: Unknown result type (might be due to invalid IL or missing references)
		//IL_6ae4: Unknown result type (might be due to invalid IL or missing references)
		//IL_6af4: Unknown result type (might be due to invalid IL or missing references)
		//IL_6afe: Unknown result type (might be due to invalid IL or missing references)
		//IL_6b00: Unknown result type (might be due to invalid IL or missing references)
		//IL_6b0a: Unknown result type (might be due to invalid IL or missing references)
		//IL_6b0c: Unknown result type (might be due to invalid IL or missing references)
		//IL_6b18: Unknown result type (might be due to invalid IL or missing references)
		//IL_6b22: Unknown result type (might be due to invalid IL or missing references)
		//IL_6b24: Unknown result type (might be due to invalid IL or missing references)
		//IL_6b2e: Unknown result type (might be due to invalid IL or missing references)
		//IL_6b30: Unknown result type (might be due to invalid IL or missing references)
		//IL_6b40: Unknown result type (might be due to invalid IL or missing references)
		//IL_6b45: Unknown result type (might be due to invalid IL or missing references)
		//IL_6b55: Unknown result type (might be due to invalid IL or missing references)
		//IL_6b5a: Unknown result type (might be due to invalid IL or missing references)
		//IL_6b6a: Unknown result type (might be due to invalid IL or missing references)
		//IL_6b6f: Unknown result type (might be due to invalid IL or missing references)
		//IL_6b81: Unknown result type (might be due to invalid IL or missing references)
		//IL_6b86: Unknown result type (might be due to invalid IL or missing references)
		//IL_6b96: Unknown result type (might be due to invalid IL or missing references)
		//IL_6b9b: Unknown result type (might be due to invalid IL or missing references)
		//IL_6bab: Unknown result type (might be due to invalid IL or missing references)
		//IL_6bb0: Unknown result type (might be due to invalid IL or missing references)
		//IL_6bc0: Unknown result type (might be due to invalid IL or missing references)
		//IL_6bc5: Unknown result type (might be due to invalid IL or missing references)
		//IL_6bd5: Unknown result type (might be due to invalid IL or missing references)
		//IL_6bda: Unknown result type (might be due to invalid IL or missing references)
		//IL_6be9: Unknown result type (might be due to invalid IL or missing references)
		//IL_6bee: Unknown result type (might be due to invalid IL or missing references)
		//IL_6bfe: Unknown result type (might be due to invalid IL or missing references)
		//IL_6c03: Unknown result type (might be due to invalid IL or missing references)
		//IL_6c13: Unknown result type (might be due to invalid IL or missing references)
		//IL_6c18: Unknown result type (might be due to invalid IL or missing references)
		//IL_6c31: Unknown result type (might be due to invalid IL or missing references)
		//IL_6c36: Unknown result type (might be due to invalid IL or missing references)
		//IL_6c49: Unknown result type (might be due to invalid IL or missing references)
		//IL_6c4e: Unknown result type (might be due to invalid IL or missing references)
		//IL_6c60: Unknown result type (might be due to invalid IL or missing references)
		//IL_6c65: Unknown result type (might be due to invalid IL or missing references)
		//IL_6c73: Unknown result type (might be due to invalid IL or missing references)
		//IL_6c78: Unknown result type (might be due to invalid IL or missing references)
		//IL_6c8b: Unknown result type (might be due to invalid IL or missing references)
		//IL_6c90: Unknown result type (might be due to invalid IL or missing references)
		//IL_6ca0: Unknown result type (might be due to invalid IL or missing references)
		//IL_6ca5: Unknown result type (might be due to invalid IL or missing references)
		//IL_6cb8: Unknown result type (might be due to invalid IL or missing references)
		//IL_6cbd: Unknown result type (might be due to invalid IL or missing references)
		//IL_6ccf: Unknown result type (might be due to invalid IL or missing references)
		//IL_6cd4: Unknown result type (might be due to invalid IL or missing references)
		//IL_6cea: Unknown result type (might be due to invalid IL or missing references)
		//IL_6cef: Unknown result type (might be due to invalid IL or missing references)
		//IL_6d05: Unknown result type (might be due to invalid IL or missing references)
		//IL_6d0a: Unknown result type (might be due to invalid IL or missing references)
		//IL_6d1d: Unknown result type (might be due to invalid IL or missing references)
		//IL_6d22: Unknown result type (might be due to invalid IL or missing references)
		//IL_6d32: Unknown result type (might be due to invalid IL or missing references)
		//IL_6d37: Unknown result type (might be due to invalid IL or missing references)
		//IL_6d4d: Unknown result type (might be due to invalid IL or missing references)
		//IL_6d52: Unknown result type (might be due to invalid IL or missing references)
		//IL_6d68: Unknown result type (might be due to invalid IL or missing references)
		//IL_6d6d: Unknown result type (might be due to invalid IL or missing references)
		//IL_6d83: Unknown result type (might be due to invalid IL or missing references)
		//IL_6d88: Unknown result type (might be due to invalid IL or missing references)
		//IL_6d9e: Unknown result type (might be due to invalid IL or missing references)
		//IL_6da3: Unknown result type (might be due to invalid IL or missing references)
		//IL_6db9: Unknown result type (might be due to invalid IL or missing references)
		//IL_6dbe: Unknown result type (might be due to invalid IL or missing references)
		//IL_6dd1: Unknown result type (might be due to invalid IL or missing references)
		//IL_6dd6: Unknown result type (might be due to invalid IL or missing references)
		//IL_6de9: Unknown result type (might be due to invalid IL or missing references)
		//IL_6dee: Unknown result type (might be due to invalid IL or missing references)
		//IL_6e01: Unknown result type (might be due to invalid IL or missing references)
		//IL_6e06: Unknown result type (might be due to invalid IL or missing references)
		//IL_6e19: Unknown result type (might be due to invalid IL or missing references)
		//IL_6e1e: Unknown result type (might be due to invalid IL or missing references)
		//IL_6e3a: Unknown result type (might be due to invalid IL or missing references)
		//IL_6e3f: Unknown result type (might be due to invalid IL or missing references)
		//IL_6e52: Unknown result type (might be due to invalid IL or missing references)
		//IL_6e57: Unknown result type (might be due to invalid IL or missing references)
		//IL_6e6a: Unknown result type (might be due to invalid IL or missing references)
		//IL_6e6f: Unknown result type (might be due to invalid IL or missing references)
		//IL_6e82: Unknown result type (might be due to invalid IL or missing references)
		//IL_6e87: Unknown result type (might be due to invalid IL or missing references)
		//IL_6e9a: Unknown result type (might be due to invalid IL or missing references)
		//IL_6e9f: Unknown result type (might be due to invalid IL or missing references)
		//IL_6ebb: Unknown result type (might be due to invalid IL or missing references)
		//IL_6ec0: Unknown result type (might be due to invalid IL or missing references)
		//IL_6ecd: Unknown result type (might be due to invalid IL or missing references)
		//IL_6eda: Unknown result type (might be due to invalid IL or missing references)
		//IL_6edc: Unknown result type (might be due to invalid IL or missing references)
		//IL_6ee9: Unknown result type (might be due to invalid IL or missing references)
		//IL_6eeb: Unknown result type (might be due to invalid IL or missing references)
		//IL_6ef8: Unknown result type (might be due to invalid IL or missing references)
		//IL_6f05: Unknown result type (might be due to invalid IL or missing references)
		//IL_6f07: Unknown result type (might be due to invalid IL or missing references)
		//IL_6f14: Unknown result type (might be due to invalid IL or missing references)
		//IL_6f16: Unknown result type (might be due to invalid IL or missing references)
		//IL_6f29: Unknown result type (might be due to invalid IL or missing references)
		//IL_6f2e: Unknown result type (might be due to invalid IL or missing references)
		//IL_6f41: Unknown result type (might be due to invalid IL or missing references)
		//IL_6f46: Unknown result type (might be due to invalid IL or missing references)
		//IL_6f5c: Unknown result type (might be due to invalid IL or missing references)
		//IL_6f61: Unknown result type (might be due to invalid IL or missing references)
		//IL_6f76: Unknown result type (might be due to invalid IL or missing references)
		//IL_6f7b: Unknown result type (might be due to invalid IL or missing references)
		//IL_6f8b: Unknown result type (might be due to invalid IL or missing references)
		//IL_6f90: Unknown result type (might be due to invalid IL or missing references)
		//IL_6fa0: Unknown result type (might be due to invalid IL or missing references)
		//IL_6fa5: Unknown result type (might be due to invalid IL or missing references)
		//IL_6fbb: Unknown result type (might be due to invalid IL or missing references)
		//IL_6fc0: Unknown result type (might be due to invalid IL or missing references)
		//IL_6fd3: Unknown result type (might be due to invalid IL or missing references)
		//IL_6fd8: Unknown result type (might be due to invalid IL or missing references)
		//IL_6fee: Unknown result type (might be due to invalid IL or missing references)
		//IL_6ff3: Unknown result type (might be due to invalid IL or missing references)
		//IL_7006: Unknown result type (might be due to invalid IL or missing references)
		//IL_700b: Unknown result type (might be due to invalid IL or missing references)
		//IL_701e: Unknown result type (might be due to invalid IL or missing references)
		//IL_7023: Unknown result type (might be due to invalid IL or missing references)
		//IL_7039: Unknown result type (might be due to invalid IL or missing references)
		//IL_703e: Unknown result type (might be due to invalid IL or missing references)
		//IL_7054: Unknown result type (might be due to invalid IL or missing references)
		//IL_7059: Unknown result type (might be due to invalid IL or missing references)
		//IL_706f: Unknown result type (might be due to invalid IL or missing references)
		//IL_7074: Unknown result type (might be due to invalid IL or missing references)
		//IL_7087: Unknown result type (might be due to invalid IL or missing references)
		//IL_708c: Unknown result type (might be due to invalid IL or missing references)
		//IL_709f: Unknown result type (might be due to invalid IL or missing references)
		//IL_70a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_70b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_70bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_70d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_70d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_70ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_70ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_7102: Unknown result type (might be due to invalid IL or missing references)
		//IL_7107: Unknown result type (might be due to invalid IL or missing references)
		//IL_711d: Unknown result type (might be due to invalid IL or missing references)
		//IL_7122: Unknown result type (might be due to invalid IL or missing references)
		//IL_7135: Unknown result type (might be due to invalid IL or missing references)
		//IL_713a: Unknown result type (might be due to invalid IL or missing references)
		//IL_714d: Unknown result type (might be due to invalid IL or missing references)
		//IL_7152: Unknown result type (might be due to invalid IL or missing references)
		//IL_7168: Unknown result type (might be due to invalid IL or missing references)
		//IL_716d: Unknown result type (might be due to invalid IL or missing references)
		//IL_7183: Unknown result type (might be due to invalid IL or missing references)
		//IL_7188: Unknown result type (might be due to invalid IL or missing references)
		//IL_719e: Unknown result type (might be due to invalid IL or missing references)
		//IL_71a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_71b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_71bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_71ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_71d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_71e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_71eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_71fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_7203: Unknown result type (might be due to invalid IL or missing references)
		//IL_7216: Unknown result type (might be due to invalid IL or missing references)
		//IL_721b: Unknown result type (might be due to invalid IL or missing references)
		//IL_722e: Unknown result type (might be due to invalid IL or missing references)
		//IL_7233: Unknown result type (might be due to invalid IL or missing references)
		//IL_7246: Unknown result type (might be due to invalid IL or missing references)
		//IL_724b: Unknown result type (might be due to invalid IL or missing references)
		//IL_7261: Unknown result type (might be due to invalid IL or missing references)
		//IL_7266: Unknown result type (might be due to invalid IL or missing references)
		//IL_727c: Unknown result type (might be due to invalid IL or missing references)
		//IL_7281: Unknown result type (might be due to invalid IL or missing references)
		//IL_7294: Unknown result type (might be due to invalid IL or missing references)
		//IL_7299: Unknown result type (might be due to invalid IL or missing references)
		//IL_72af: Unknown result type (might be due to invalid IL or missing references)
		//IL_72b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_72c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_72cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_72df: Unknown result type (might be due to invalid IL or missing references)
		//IL_72e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_72f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_72fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_730f: Unknown result type (might be due to invalid IL or missing references)
		//IL_7314: Unknown result type (might be due to invalid IL or missing references)
		//IL_732a: Unknown result type (might be due to invalid IL or missing references)
		//IL_732f: Unknown result type (might be due to invalid IL or missing references)
		//IL_7348: Unknown result type (might be due to invalid IL or missing references)
		//IL_734d: Unknown result type (might be due to invalid IL or missing references)
		//IL_7366: Unknown result type (might be due to invalid IL or missing references)
		//IL_736b: Unknown result type (might be due to invalid IL or missing references)
		//IL_7381: Unknown result type (might be due to invalid IL or missing references)
		//IL_7386: Unknown result type (might be due to invalid IL or missing references)
		//IL_7399: Unknown result type (might be due to invalid IL or missing references)
		//IL_739e: Unknown result type (might be due to invalid IL or missing references)
		//IL_73b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_73b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_73c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_73cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_73e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_73e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_73f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_73fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_7410: Unknown result type (might be due to invalid IL or missing references)
		//IL_7415: Unknown result type (might be due to invalid IL or missing references)
		//IL_7426: Unknown result type (might be due to invalid IL or missing references)
		//IL_742b: Unknown result type (might be due to invalid IL or missing references)
		//IL_743e: Unknown result type (might be due to invalid IL or missing references)
		//IL_7443: Unknown result type (might be due to invalid IL or missing references)
		//IL_7456: Unknown result type (might be due to invalid IL or missing references)
		//IL_745b: Unknown result type (might be due to invalid IL or missing references)
		//IL_746e: Unknown result type (might be due to invalid IL or missing references)
		//IL_7473: Unknown result type (might be due to invalid IL or missing references)
		//IL_7486: Unknown result type (might be due to invalid IL or missing references)
		//IL_748b: Unknown result type (might be due to invalid IL or missing references)
		//IL_749e: Unknown result type (might be due to invalid IL or missing references)
		//IL_74a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_74b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_74ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_74cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_74d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_74ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_74ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_7501: Unknown result type (might be due to invalid IL or missing references)
		//IL_7506: Unknown result type (might be due to invalid IL or missing references)
		//IL_7519: Unknown result type (might be due to invalid IL or missing references)
		//IL_751e: Unknown result type (might be due to invalid IL or missing references)
		//IL_7531: Unknown result type (might be due to invalid IL or missing references)
		//IL_7536: Unknown result type (might be due to invalid IL or missing references)
		//IL_7549: Unknown result type (might be due to invalid IL or missing references)
		//IL_754e: Unknown result type (might be due to invalid IL or missing references)
		//IL_756a: Unknown result type (might be due to invalid IL or missing references)
		//IL_756f: Unknown result type (might be due to invalid IL or missing references)
		//IL_757f: Unknown result type (might be due to invalid IL or missing references)
		//IL_7584: Unknown result type (might be due to invalid IL or missing references)
		//IL_7597: Unknown result type (might be due to invalid IL or missing references)
		//IL_759c: Unknown result type (might be due to invalid IL or missing references)
		//IL_75b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_75b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_75ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_75cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_75e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_75ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_7602: Unknown result type (might be due to invalid IL or missing references)
		//IL_7607: Unknown result type (might be due to invalid IL or missing references)
		//IL_761a: Unknown result type (might be due to invalid IL or missing references)
		//IL_761f: Unknown result type (might be due to invalid IL or missing references)
		//IL_7638: Unknown result type (might be due to invalid IL or missing references)
		//IL_763d: Unknown result type (might be due to invalid IL or missing references)
		//IL_7650: Unknown result type (might be due to invalid IL or missing references)
		//IL_7655: Unknown result type (might be due to invalid IL or missing references)
		//IL_7668: Unknown result type (might be due to invalid IL or missing references)
		//IL_766d: Unknown result type (might be due to invalid IL or missing references)
		//IL_7680: Unknown result type (might be due to invalid IL or missing references)
		//IL_7685: Unknown result type (might be due to invalid IL or missing references)
		//IL_7698: Unknown result type (might be due to invalid IL or missing references)
		//IL_769d: Unknown result type (might be due to invalid IL or missing references)
		//IL_76b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_76b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_76c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_76cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_76e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_76e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_76f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_76fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_7710: Unknown result type (might be due to invalid IL or missing references)
		//IL_7715: Unknown result type (might be due to invalid IL or missing references)
		//IL_7728: Unknown result type (might be due to invalid IL or missing references)
		//IL_772d: Unknown result type (might be due to invalid IL or missing references)
		//IL_7740: Unknown result type (might be due to invalid IL or missing references)
		//IL_7745: Unknown result type (might be due to invalid IL or missing references)
		//IL_7758: Unknown result type (might be due to invalid IL or missing references)
		//IL_775d: Unknown result type (might be due to invalid IL or missing references)
		//IL_7770: Unknown result type (might be due to invalid IL or missing references)
		//IL_7775: Unknown result type (might be due to invalid IL or missing references)
		//IL_7788: Unknown result type (might be due to invalid IL or missing references)
		//IL_778d: Unknown result type (might be due to invalid IL or missing references)
		//IL_77a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_77a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_77b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_77bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_77d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_77d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_77e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_77ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_7800: Unknown result type (might be due to invalid IL or missing references)
		//IL_7805: Unknown result type (might be due to invalid IL or missing references)
		//IL_7818: Unknown result type (might be due to invalid IL or missing references)
		//IL_781d: Unknown result type (might be due to invalid IL or missing references)
		//IL_7830: Unknown result type (might be due to invalid IL or missing references)
		//IL_7835: Unknown result type (might be due to invalid IL or missing references)
		//IL_784b: Unknown result type (might be due to invalid IL or missing references)
		//IL_7850: Unknown result type (might be due to invalid IL or missing references)
		//IL_7863: Unknown result type (might be due to invalid IL or missing references)
		//IL_7868: Unknown result type (might be due to invalid IL or missing references)
		//IL_787b: Unknown result type (might be due to invalid IL or missing references)
		//IL_7880: Unknown result type (might be due to invalid IL or missing references)
		//IL_7893: Unknown result type (might be due to invalid IL or missing references)
		//IL_7898: Unknown result type (might be due to invalid IL or missing references)
		//IL_78ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_78b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_78c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_78c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_78de: Unknown result type (might be due to invalid IL or missing references)
		//IL_78e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_78f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_78fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_790e: Unknown result type (might be due to invalid IL or missing references)
		//IL_7913: Unknown result type (might be due to invalid IL or missing references)
		//IL_7929: Unknown result type (might be due to invalid IL or missing references)
		//IL_792e: Unknown result type (might be due to invalid IL or missing references)
		//IL_7941: Unknown result type (might be due to invalid IL or missing references)
		//IL_7946: Unknown result type (might be due to invalid IL or missing references)
		//IL_7959: Unknown result type (might be due to invalid IL or missing references)
		//IL_795e: Unknown result type (might be due to invalid IL or missing references)
		//IL_7971: Unknown result type (might be due to invalid IL or missing references)
		//IL_7976: Unknown result type (might be due to invalid IL or missing references)
		//IL_7989: Unknown result type (might be due to invalid IL or missing references)
		//IL_798e: Unknown result type (might be due to invalid IL or missing references)
		//IL_79a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_79a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_79bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_79c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_79d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_79dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_79f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_79f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_7a0a: Unknown result type (might be due to invalid IL or missing references)
		//IL_7a0f: Unknown result type (might be due to invalid IL or missing references)
		//IL_7a22: Unknown result type (might be due to invalid IL or missing references)
		//IL_7a27: Unknown result type (might be due to invalid IL or missing references)
		//IL_7a3a: Unknown result type (might be due to invalid IL or missing references)
		//IL_7a3f: Unknown result type (might be due to invalid IL or missing references)
		//IL_7a52: Unknown result type (might be due to invalid IL or missing references)
		//IL_7a57: Unknown result type (might be due to invalid IL or missing references)
		//IL_7a6d: Unknown result type (might be due to invalid IL or missing references)
		//IL_7a72: Unknown result type (might be due to invalid IL or missing references)
		//IL_7a8b: Unknown result type (might be due to invalid IL or missing references)
		//IL_7a90: Unknown result type (might be due to invalid IL or missing references)
		//IL_7aa9: Unknown result type (might be due to invalid IL or missing references)
		//IL_7aae: Unknown result type (might be due to invalid IL or missing references)
		//IL_7ac4: Unknown result type (might be due to invalid IL or missing references)
		//IL_7ac9: Unknown result type (might be due to invalid IL or missing references)
		//IL_7adc: Unknown result type (might be due to invalid IL or missing references)
		//IL_7ae1: Unknown result type (might be due to invalid IL or missing references)
		//IL_7af4: Unknown result type (might be due to invalid IL or missing references)
		//IL_7af9: Unknown result type (might be due to invalid IL or missing references)
		//IL_7b0b: Unknown result type (might be due to invalid IL or missing references)
		//IL_7b10: Unknown result type (might be due to invalid IL or missing references)
		//IL_7b23: Unknown result type (might be due to invalid IL or missing references)
		//IL_7b28: Unknown result type (might be due to invalid IL or missing references)
		//IL_7b3b: Unknown result type (might be due to invalid IL or missing references)
		//IL_7b40: Unknown result type (might be due to invalid IL or missing references)
		//IL_7b53: Unknown result type (might be due to invalid IL or missing references)
		//IL_7b58: Unknown result type (might be due to invalid IL or missing references)
		//IL_7b69: Unknown result type (might be due to invalid IL or missing references)
		//IL_7b6e: Unknown result type (might be due to invalid IL or missing references)
		//IL_7b81: Unknown result type (might be due to invalid IL or missing references)
		//IL_7b86: Unknown result type (might be due to invalid IL or missing references)
		//IL_7b99: Unknown result type (might be due to invalid IL or missing references)
		//IL_7b9e: Unknown result type (might be due to invalid IL or missing references)
		//IL_7bb1: Unknown result type (might be due to invalid IL or missing references)
		//IL_7bb6: Unknown result type (might be due to invalid IL or missing references)
		//IL_7bc9: Unknown result type (might be due to invalid IL or missing references)
		//IL_7bce: Unknown result type (might be due to invalid IL or missing references)
		//IL_7be1: Unknown result type (might be due to invalid IL or missing references)
		//IL_7be6: Unknown result type (might be due to invalid IL or missing references)
		//IL_7bfc: Unknown result type (might be due to invalid IL or missing references)
		//IL_7c01: Unknown result type (might be due to invalid IL or missing references)
		//IL_7c14: Unknown result type (might be due to invalid IL or missing references)
		//IL_7c19: Unknown result type (might be due to invalid IL or missing references)
		//IL_7c2c: Unknown result type (might be due to invalid IL or missing references)
		//IL_7c31: Unknown result type (might be due to invalid IL or missing references)
		//IL_7c47: Unknown result type (might be due to invalid IL or missing references)
		//IL_7c4c: Unknown result type (might be due to invalid IL or missing references)
		//IL_7c5f: Unknown result type (might be due to invalid IL or missing references)
		//IL_7c64: Unknown result type (might be due to invalid IL or missing references)
		//IL_7c77: Unknown result type (might be due to invalid IL or missing references)
		//IL_7c7c: Unknown result type (might be due to invalid IL or missing references)
		//IL_7c8f: Unknown result type (might be due to invalid IL or missing references)
		//IL_7c94: Unknown result type (might be due to invalid IL or missing references)
		//IL_7ca6: Unknown result type (might be due to invalid IL or missing references)
		//IL_7cab: Unknown result type (might be due to invalid IL or missing references)
		//IL_7cbe: Unknown result type (might be due to invalid IL or missing references)
		//IL_7cc3: Unknown result type (might be due to invalid IL or missing references)
		//IL_7cd6: Unknown result type (might be due to invalid IL or missing references)
		//IL_7cdb: Unknown result type (might be due to invalid IL or missing references)
		//IL_7cee: Unknown result type (might be due to invalid IL or missing references)
		//IL_7cf3: Unknown result type (might be due to invalid IL or missing references)
		//IL_7d06: Unknown result type (might be due to invalid IL or missing references)
		//IL_7d0b: Unknown result type (might be due to invalid IL or missing references)
		//IL_7d21: Unknown result type (might be due to invalid IL or missing references)
		//IL_7d26: Unknown result type (might be due to invalid IL or missing references)
		//IL_7d3c: Unknown result type (might be due to invalid IL or missing references)
		//IL_7d41: Unknown result type (might be due to invalid IL or missing references)
		//IL_7d57: Unknown result type (might be due to invalid IL or missing references)
		//IL_7d5c: Unknown result type (might be due to invalid IL or missing references)
		//IL_7d6f: Unknown result type (might be due to invalid IL or missing references)
		//IL_7d74: Unknown result type (might be due to invalid IL or missing references)
		//IL_7d90: Unknown result type (might be due to invalid IL or missing references)
		//IL_7d97: Unknown result type (might be due to invalid IL or missing references)
		//IL_7d9c: Unknown result type (might be due to invalid IL or missing references)
		//IL_7db1: Unknown result type (might be due to invalid IL or missing references)
		//IL_7db8: Unknown result type (might be due to invalid IL or missing references)
		//IL_7dbd: Unknown result type (might be due to invalid IL or missing references)
		//IL_7dd2: Unknown result type (might be due to invalid IL or missing references)
		//IL_7dd9: Unknown result type (might be due to invalid IL or missing references)
		//IL_7dde: Unknown result type (might be due to invalid IL or missing references)
		//IL_7df3: Unknown result type (might be due to invalid IL or missing references)
		//IL_7dfa: Unknown result type (might be due to invalid IL or missing references)
		//IL_7dff: Unknown result type (might be due to invalid IL or missing references)
		//IL_7e14: Unknown result type (might be due to invalid IL or missing references)
		//IL_7e1b: Unknown result type (might be due to invalid IL or missing references)
		//IL_7e20: Unknown result type (might be due to invalid IL or missing references)
		//IL_7e35: Unknown result type (might be due to invalid IL or missing references)
		//IL_7e3c: Unknown result type (might be due to invalid IL or missing references)
		//IL_7e41: Unknown result type (might be due to invalid IL or missing references)
		//IL_7e56: Unknown result type (might be due to invalid IL or missing references)
		//IL_7e5d: Unknown result type (might be due to invalid IL or missing references)
		//IL_7e62: Unknown result type (might be due to invalid IL or missing references)
		//IL_7e77: Unknown result type (might be due to invalid IL or missing references)
		//IL_7e7e: Unknown result type (might be due to invalid IL or missing references)
		//IL_7e83: Unknown result type (might be due to invalid IL or missing references)
		//IL_7e98: Unknown result type (might be due to invalid IL or missing references)
		//IL_7e9f: Unknown result type (might be due to invalid IL or missing references)
		//IL_7ea4: Unknown result type (might be due to invalid IL or missing references)
		//IL_7eb7: Unknown result type (might be due to invalid IL or missing references)
		//IL_7ebc: Unknown result type (might be due to invalid IL or missing references)
		//IL_7ed1: Unknown result type (might be due to invalid IL or missing references)
		//IL_7ed8: Unknown result type (might be due to invalid IL or missing references)
		//IL_7edd: Unknown result type (might be due to invalid IL or missing references)
		//IL_7ef2: Unknown result type (might be due to invalid IL or missing references)
		//IL_7ef9: Unknown result type (might be due to invalid IL or missing references)
		//IL_7efe: Unknown result type (might be due to invalid IL or missing references)
		//IL_7f13: Unknown result type (might be due to invalid IL or missing references)
		//IL_7f1a: Unknown result type (might be due to invalid IL or missing references)
		//IL_7f1f: Unknown result type (might be due to invalid IL or missing references)
		//IL_7f34: Unknown result type (might be due to invalid IL or missing references)
		//IL_7f3b: Unknown result type (might be due to invalid IL or missing references)
		//IL_7f40: Unknown result type (might be due to invalid IL or missing references)
		//IL_7f55: Unknown result type (might be due to invalid IL or missing references)
		//IL_7f5c: Unknown result type (might be due to invalid IL or missing references)
		//IL_7f61: Unknown result type (might be due to invalid IL or missing references)
		//IL_7f76: Unknown result type (might be due to invalid IL or missing references)
		//IL_7f7d: Unknown result type (might be due to invalid IL or missing references)
		//IL_7f82: Unknown result type (might be due to invalid IL or missing references)
		//IL_7f9d: Unknown result type (might be due to invalid IL or missing references)
		//IL_7fb3: Unknown result type (might be due to invalid IL or missing references)
		//IL_801c: Unknown result type (might be due to invalid IL or missing references)
		//IL_8021: Unknown result type (might be due to invalid IL or missing references)
		//IL_8047: Unknown result type (might be due to invalid IL or missing references)
		//IL_8054: Unknown result type (might be due to invalid IL or missing references)
		//IL_80bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_80c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_80eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_80f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_8163: Unknown result type (might be due to invalid IL or missing references)
		//IL_8168: Unknown result type (might be due to invalid IL or missing references)
		//IL_8183: Unknown result type (might be due to invalid IL or missing references)
		//IL_81ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_81b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_8266: Unknown result type (might be due to invalid IL or missing references)
		//IL_826b: Unknown result type (might be due to invalid IL or missing references)
		//IL_820e: Unknown result type (might be due to invalid IL or missing references)
		//IL_8213: Unknown result type (might be due to invalid IL or missing references)
		//IL_82ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_82bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_839f: Unknown result type (might be due to invalid IL or missing references)
		//IL_83a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_8345: Unknown result type (might be due to invalid IL or missing references)
		//IL_834a: Unknown result type (might be due to invalid IL or missing references)
		//IL_83d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_83d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_8408: Unknown result type (might be due to invalid IL or missing references)
		//IL_840d: Unknown result type (might be due to invalid IL or missing references)
		//IL_843f: Unknown result type (might be due to invalid IL or missing references)
		//IL_8444: Unknown result type (might be due to invalid IL or missing references)
		//IL_846d: Unknown result type (might be due to invalid IL or missing references)
		//IL_846f: Unknown result type (might be due to invalid IL or missing references)
		Color[][] array = new Color[TileID.Count][];
		for (int i = 0; i < TileID.Count; i++)
		{
			array[i] = new Color[13];
		}
		array[656][0] = new Color(21, 124, 212);
		array[701][0] = array[656][0];
		array[624][0] = new Color(210, 91, 77);
		array[700][0] = array[624][0];
		array[621][0] = new Color(250, 250, 250);
		array[622][0] = new Color(235, 235, 249);
		array[518][0] = new Color(26, 196, 84);
		array[518][1] = new Color(48, 208, 234);
		array[518][2] = new Color(135, 196, 26);
		array[519][0] = new Color(28, 216, 109);
		array[519][1] = new Color(107, 182, 0);
		array[519][2] = new Color(75, 184, 230);
		array[519][3] = new Color(208, 80, 80);
		array[519][4] = new Color(141, 137, 223);
		array[519][5] = new Color(182, 175, 130);
		array[549][0] = new Color(54, 83, 20);
		array[528][0] = new Color(182, 175, 130);
		array[529][0] = new Color(99, 150, 8);
		array[529][1] = new Color(139, 154, 64);
		array[529][2] = new Color(34, 129, 168);
		array[529][3] = new Color(180, 82, 82);
		array[529][4] = new Color(113, 108, 205);
		Color val = new Color(151, 107, 75);
		array[0][0] = val;
		array[668][0] = val;
		array[5][0] = val;
		array[5][1] = new Color(182, 175, 130);
		Color val2 = new Color(127, 127, 127);
		array[583][0] = val2;
		array[584][0] = val2;
		array[585][0] = val2;
		array[586][0] = val2;
		array[587][0] = val2;
		array[588][0] = val2;
		array[589][0] = val2;
		array[590][0] = val2;
		array[595][0] = val;
		array[596][0] = val;
		array[615][0] = val;
		array[616][0] = val;
		array[634][0] = new Color(145, 120, 120);
		array[633][0] = new Color(210, 140, 100);
		array[637][0] = new Color(200, 120, 75);
		array[638][0] = new Color(200, 120, 75);
		array[30][0] = val;
		array[191][0] = val;
		array[272][0] = new Color(121, 119, 101);
		val = new Color(128, 128, 128);
		array[1][0] = val;
		array[38][0] = val;
		array[48][0] = val;
		array[130][0] = val;
		array[138][0] = val;
		array[664][0] = val;
		array[711][0] = val;
		array[712][0] = val;
		array[713][0] = val;
		array[714][0] = val;
		array[715][0] = val;
		array[716][0] = val;
		array[273][0] = val;
		array[283][0] = val;
		array[618][0] = val;
		array[654][0] = new Color(200, 44, 28);
		array[2][0] = new Color(28, 216, 94);
		array[477][0] = new Color(28, 216, 94);
		array[492][0] = new Color(78, 193, 227);
		val = new Color(26, 196, 84);
		array[3][0] = val;
		array[192][0] = val;
		array[73][0] = new Color(27, 197, 109);
		array[52][0] = new Color(23, 177, 76);
		array[353][0] = new Color(28, 216, 94);
		array[20][0] = new Color(163, 116, 81);
		array[6][0] = new Color(140, 101, 80);
		val = new Color(150, 67, 22);
		array[7][0] = val;
		array[47][0] = val;
		array[284][0] = val;
		array[682][0] = val;
		array[560][0] = val;
		val = new Color(185, 164, 23);
		array[8][0] = val;
		array[45][0] = val;
		array[680][0] = val;
		array[560][2] = val;
		val = new Color(185, 194, 195);
		array[9][0] = val;
		array[46][0] = val;
		array[681][0] = val;
		array[560][1] = val;
		val = new Color(98, 95, 167);
		array[22][0] = val;
		array[140][0] = val;
		array[23][0] = new Color(141, 137, 223);
		array[24][0] = new Color(122, 116, 218);
		array[636][0] = new Color(122, 116, 218);
		array[25][0] = new Color(109, 90, 128);
		array[37][0] = new Color(104, 86, 84);
		array[39][0] = new Color(181, 62, 59);
		array[40][0] = new Color(146, 81, 68);
		array[41][0] = new Color(66, 84, 109);
		array[677][0] = new Color(66, 84, 109);
		array[481][0] = new Color(66, 84, 109);
		array[43][0] = new Color(84, 100, 63);
		array[678][0] = new Color(84, 100, 63);
		array[482][0] = new Color(84, 100, 63);
		array[44][0] = new Color(107, 68, 99);
		array[679][0] = new Color(107, 68, 99);
		array[483][0] = new Color(107, 68, 99);
		array[53][0] = new Color(186, 168, 84);
		val = new Color(190, 171, 94);
		array[151][0] = val;
		array[154][0] = val;
		array[274][0] = val;
		array[328][0] = new Color(200, 246, 254);
		array[329][0] = new Color(15, 15, 15);
		array[54][0] = new Color(200, 246, 254);
		array[56][0] = new Color(43, 40, 84);
		array[75][0] = new Color(26, 26, 26);
		array[683][0] = new Color(100, 90, 190);
		array[57][0] = new Color(68, 68, 76);
		val = new Color(142, 66, 66);
		array[58][0] = val;
		array[76][0] = val;
		array[684][0] = val;
		val = new Color(92, 68, 73);
		array[59][0] = val;
		array[120][0] = val;
		array[60][0] = new Color(143, 215, 29);
		array[61][0] = new Color(135, 196, 26);
		array[703][0] = array[61][0];
		array[74][0] = new Color(96, 197, 27);
		array[62][0] = new Color(121, 176, 24);
		array[233][0] = new Color(107, 182, 29);
		array[652][0] = array[233][0];
		array[651][0] = array[233][0];
		array[63][0] = new Color(110, 140, 182);
		array[64][0] = new Color(196, 96, 114);
		array[65][0] = new Color(56, 150, 97);
		array[66][0] = new Color(160, 118, 58);
		array[67][0] = new Color(140, 58, 166);
		array[68][0] = new Color(125, 191, 197);
		array[566][0] = new Color(233, 180, 90);
		array[70][0] = new Color(93, 127, 255);
		val = new Color(182, 175, 130);
		array[71][0] = val;
		array[72][0] = val;
		array[190][0] = val;
		array[578][0] = new Color(172, 155, 110);
		val = new Color(73, 120, 17);
		array[80][0] = val;
		array[484][0] = val;
		array[188][0] = val;
		array[80][1] = new Color(87, 84, 151);
		array[80][2] = new Color(34, 129, 168);
		array[80][3] = new Color(130, 56, 55);
		val = new Color(11, 80, 143);
		array[107][0] = val;
		array[121][0] = val;
		array[685][0] = val;
		val = new Color(91, 169, 169);
		array[108][0] = val;
		array[122][0] = val;
		array[686][0] = val;
		val = new Color(128, 26, 52);
		array[111][0] = val;
		array[150][0] = val;
		array[109][0] = new Color(78, 193, 227);
		array[110][0] = new Color(48, 186, 135);
		array[113][0] = new Color(48, 208, 234);
		array[115][0] = new Color(33, 171, 207);
		array[112][0] = new Color(103, 98, 122);
		val = new Color(238, 225, 218);
		array[116][0] = val;
		array[118][0] = val;
		array[117][0] = new Color(181, 172, 190);
		array[119][0] = new Color(107, 92, 108);
		array[123][0] = new Color(106, 107, 118);
		array[124][0] = new Color(73, 51, 36);
		array[131][0] = new Color(52, 52, 52);
		array[145][0] = new Color(192, 30, 30);
		array[146][0] = new Color(43, 192, 30);
		val = new Color(211, 236, 241);
		array[147][0] = val;
		array[148][0] = val;
		array[152][0] = new Color(128, 133, 184);
		array[153][0] = new Color(239, 141, 126);
		array[155][0] = new Color(131, 162, 161);
		array[156][0] = new Color(170, 171, 157);
		array[157][0] = new Color(104, 100, 126);
		val = new Color(145, 81, 85);
		array[158][0] = val;
		array[232][0] = val;
		array[575][0] = new Color(125, 61, 65);
		array[159][0] = new Color(148, 133, 98);
		array[161][0] = new Color(144, 195, 232);
		array[162][0] = new Color(184, 219, 240);
		array[163][0] = new Color(174, 145, 214);
		array[164][0] = new Color(218, 182, 204);
		array[170][0] = new Color(27, 109, 69);
		array[171][0] = new Color(33, 135, 85);
		val = new Color(129, 125, 93);
		array[166][0] = val;
		array[175][0] = val;
		array[167][0] = new Color(62, 82, 114);
		val = new Color(132, 157, 127);
		array[168][0] = val;
		array[176][0] = val;
		val = new Color(152, 171, 198);
		array[169][0] = val;
		array[177][0] = val;
		array[179][0] = new Color(49, 134, 114);
		array[180][0] = new Color(126, 134, 49);
		array[181][0] = new Color(134, 59, 49);
		array[182][0] = new Color(43, 86, 140);
		array[183][0] = new Color(121, 49, 134);
		array[381][0] = new Color(254, 121, 2);
		array[687][0] = new Color(254, 121, 2);
		array[534][0] = new Color(114, 254, 2);
		array[689][0] = new Color(114, 254, 2);
		array[536][0] = new Color(0, 197, 208);
		array[690][0] = new Color(0, 197, 208);
		array[539][0] = new Color(208, 0, 126);
		array[688][0] = new Color(208, 0, 126);
		array[625][0] = new Color(220, 12, 237);
		array[691][0] = new Color(220, 12, 237);
		array[627][0] = new Color(255, 76, 76);
		array[627][1] = new Color(255, 195, 76);
		array[627][2] = new Color(195, 255, 76);
		array[627][3] = new Color(76, 255, 76);
		array[627][4] = new Color(76, 255, 195);
		array[627][5] = new Color(76, 195, 255);
		array[627][6] = new Color(77, 76, 255);
		array[627][7] = new Color(196, 76, 255);
		array[627][8] = new Color(255, 76, 195);
		array[512][0] = new Color(49, 134, 114);
		array[513][0] = new Color(126, 134, 49);
		array[514][0] = new Color(134, 59, 49);
		array[515][0] = new Color(43, 86, 140);
		array[516][0] = new Color(121, 49, 134);
		array[517][0] = new Color(254, 121, 2);
		array[535][0] = new Color(114, 254, 2);
		array[537][0] = new Color(0, 197, 208);
		array[540][0] = new Color(208, 0, 126);
		array[626][0] = new Color(220, 12, 237);
		for (int j = 0; j < array[628].Length; j++)
		{
			array[628][j] = array[627][j];
		}
		for (int k = 0; k < array[692].Length; k++)
		{
			array[692][k] = array[627][k];
		}
		for (int l = 0; l < array[160].Length; l++)
		{
			array[160][l] = array[627][l];
		}
		array[184][0] = new Color(29, 106, 88);
		array[184][1] = new Color(94, 100, 36);
		array[184][2] = new Color(96, 44, 40);
		array[184][3] = new Color(34, 63, 102);
		array[184][4] = new Color(79, 35, 95);
		array[184][5] = new Color(253, 62, 3);
		array[184][6] = new Color(22, 123, 62);
		array[184][7] = new Color(0, 106, 148);
		array[184][8] = new Color(148, 0, 132);
		array[184][9] = new Color(122, 24, 168);
		array[184][10] = new Color(220, 20, 20);
		array[189][0] = new Color(223, 255, 255);
		array[193][0] = new Color(56, 121, 255);
		array[194][0] = new Color(157, 157, 107);
		array[195][0] = new Color(134, 22, 34);
		array[196][0] = new Color(147, 144, 178);
		array[197][0] = new Color(97, 200, 225);
		array[198][0] = new Color(62, 61, 52);
		array[199][0] = new Color(208, 80, 80);
		array[201][0] = new Color(203, 61, 64);
		array[205][0] = new Color(186, 50, 52);
		array[200][0] = new Color(216, 152, 144);
		array[202][0] = new Color(213, 178, 28);
		array[203][0] = new Color(128, 44, 45);
		array[204][0] = new Color(125, 55, 65);
		array[206][0] = new Color(124, 175, 201);
		array[208][0] = new Color(88, 105, 118);
		array[211][0] = new Color(191, 233, 115);
		array[213][0] = new Color(137, 120, 67);
		array[214][0] = new Color(103, 103, 103);
		array[221][0] = new Color(239, 90, 50);
		array[222][0] = new Color(231, 96, 228);
		array[223][0] = new Color(57, 85, 101);
		array[224][0] = new Color(107, 132, 139);
		array[225][0] = new Color(227, 125, 22);
		array[226][0] = new Color(141, 56, 0);
		array[229][0] = new Color(255, 156, 12);
		array[659][0] = new Color(247, 228, 254);
		array[230][0] = new Color(131, 79, 13);
		array[234][0] = new Color(53, 44, 41);
		array[235][0] = new Color(214, 184, 46);
		array[236][0] = new Color(149, 232, 87);
		array[702][0] = array[236][0];
		array[237][0] = new Color(255, 241, 51);
		array[238][0] = new Color(225, 128, 206);
		array[655][0] = new Color(225, 128, 206);
		array[243][0] = new Color(198, 196, 170);
		array[248][0] = new Color(219, 71, 38);
		array[249][0] = new Color(235, 38, 231);
		array[250][0] = new Color(86, 85, 92);
		array[251][0] = new Color(235, 150, 23);
		array[252][0] = new Color(153, 131, 44);
		array[253][0] = new Color(57, 48, 97);
		array[254][0] = new Color(248, 158, 92);
		array[255][0] = new Color(107, 49, 154);
		array[256][0] = new Color(154, 148, 49);
		array[257][0] = new Color(49, 49, 154);
		array[258][0] = new Color(49, 154, 68);
		array[259][0] = new Color(154, 49, 77);
		array[260][0] = new Color(85, 89, 118);
		array[261][0] = new Color(154, 83, 49);
		array[262][0] = new Color(221, 79, 255);
		array[263][0] = new Color(250, 255, 79);
		array[264][0] = new Color(79, 102, 255);
		array[265][0] = new Color(79, 255, 89);
		array[266][0] = new Color(255, 79, 79);
		array[267][0] = new Color(240, 240, 247);
		array[268][0] = new Color(255, 145, 79);
		array[287][0] = new Color(79, 128, 17);
		val = new Color(122, 217, 232);
		array[275][0] = val;
		array[276][0] = val;
		array[277][0] = val;
		array[278][0] = val;
		array[279][0] = val;
		array[280][0] = val;
		array[281][0] = val;
		array[282][0] = val;
		array[285][0] = val;
		array[286][0] = val;
		array[288][0] = val;
		array[289][0] = val;
		array[290][0] = val;
		array[291][0] = val;
		array[292][0] = val;
		array[293][0] = val;
		array[294][0] = val;
		array[295][0] = val;
		array[296][0] = val;
		array[297][0] = val;
		array[298][0] = val;
		array[299][0] = val;
		array[309][0] = val;
		array[310][0] = val;
		array[413][0] = val;
		array[339][0] = val;
		array[542][0] = val;
		array[632][0] = val;
		array[640][0] = val;
		array[643][0] = val;
		array[644][0] = val;
		array[645][0] = val;
		array[710][0] = val;
		array[358][0] = val;
		array[359][0] = val;
		array[360][0] = val;
		array[361][0] = val;
		array[362][0] = val;
		array[363][0] = val;
		array[364][0] = val;
		array[391][0] = val;
		array[392][0] = val;
		array[393][0] = val;
		array[394][0] = val;
		array[414][0] = val;
		array[505][0] = val;
		array[543][0] = val;
		array[598][0] = val;
		array[521][0] = val;
		array[522][0] = val;
		array[523][0] = val;
		array[524][0] = val;
		array[525][0] = val;
		array[526][0] = val;
		array[527][0] = val;
		array[532][0] = val;
		array[533][0] = val;
		array[538][0] = val;
		array[544][0] = val;
		array[629][0] = val;
		array[550][0] = val;
		array[551][0] = val;
		array[553][0] = val;
		array[554][0] = val;
		array[555][0] = val;
		array[556][0] = val;
		array[558][0] = val;
		array[559][0] = val;
		array[580][0] = val;
		array[582][0] = val;
		array[599][0] = val;
		array[600][0] = val;
		array[601][0] = val;
		array[602][0] = val;
		array[603][0] = val;
		array[604][0] = val;
		array[605][0] = val;
		array[606][0] = val;
		array[607][0] = val;
		array[608][0] = val;
		array[609][0] = val;
		array[610][0] = val;
		array[611][0] = val;
		array[612][0] = val;
		array[619][0] = val;
		array[620][0] = val;
		array[630][0] = new Color(117, 145, 73);
		array[631][0] = new Color(122, 234, 225);
		array[552][0] = array[53][0];
		array[564][0] = new Color(87, 127, 220);
		array[408][0] = new Color(85, 83, 82);
		array[409][0] = new Color(85, 83, 82);
		array[669][0] = new Color(83, 46, 57);
		array[670][0] = new Color(91, 87, 167);
		array[671][0] = new Color(23, 33, 81);
		array[672][0] = new Color(53, 133, 103);
		array[673][0] = new Color(11, 67, 80);
		array[674][0] = new Color(40, 49, 60);
		array[675][0] = new Color(21, 13, 77);
		array[676][0] = new Color(195, 201, 215);
		array[415][0] = new Color(249, 75, 7);
		array[416][0] = new Color(0, 160, 170);
		array[417][0] = new Color(160, 87, 234);
		array[418][0] = new Color(22, 173, 254);
		array[489][0] = new Color(255, 29, 136);
		array[490][0] = new Color(211, 211, 211);
		array[311][0] = new Color(117, 61, 25);
		array[312][0] = new Color(204, 93, 73);
		array[313][0] = new Color(87, 150, 154);
		array[4][0] = new Color(253, 221, 3);
		array[4][1] = new Color(253, 221, 3);
		val = new Color(253, 221, 3);
		array[93][0] = val;
		array[33][0] = val;
		array[174][0] = val;
		array[100][0] = val;
		array[98][0] = val;
		array[173][0] = val;
		val = new Color(119, 105, 79);
		array[11][0] = val;
		array[10][0] = val;
		array[593][0] = val;
		array[594][0] = val;
		val = new Color(191, 142, 111);
		array[14][0] = val;
		array[469][0] = val;
		array[486][0] = val;
		array[488][0] = new Color(127, 92, 69);
		array[704][0] = array[488][0];
		array[487][0] = val;
		array[487][1] = val;
		array[15][0] = val;
		array[15][1] = val;
		array[497][0] = val;
		array[18][0] = val;
		array[19][0] = val;
		array[19][1] = Color.Black;
		array[55][0] = val;
		array[79][0] = val;
		array[86][0] = val;
		array[87][0] = val;
		array[88][0] = val;
		array[89][0] = val;
		array[89][1] = val;
		array[89][2] = new Color(105, 107, 125);
		array[94][0] = val;
		array[101][0] = val;
		array[104][0] = val;
		array[106][0] = val;
		array[114][0] = val;
		array[128][0] = val;
		array[139][0] = val;
		array[172][0] = val;
		array[216][0] = val;
		array[269][0] = val;
		array[334][0] = val;
		array[471][0] = val;
		array[470][0] = val;
		array[475][0] = val;
		array[377][0] = val;
		array[380][0] = val;
		array[395][0] = val;
		array[573][0] = val;
		array[12][0] = new Color(174, 24, 69);
		array[665][0] = new Color(174, 24, 69);
		array[639][0] = new Color(110, 105, 255);
		array[13][0] = new Color(133, 213, 247);
		val = new Color(144, 148, 144);
		array[17][0] = val;
		array[90][0] = val;
		array[96][0] = val;
		array[97][0] = val;
		array[99][0] = val;
		array[132][0] = val;
		array[142][0] = val;
		array[143][0] = val;
		array[144][0] = val;
		array[207][0] = val;
		array[209][0] = val;
		array[212][0] = val;
		array[217][0] = val;
		array[218][0] = val;
		array[219][0] = val;
		array[220][0] = val;
		array[228][0] = val;
		array[300][0] = val;
		array[301][0] = val;
		array[302][0] = val;
		array[303][0] = val;
		array[304][0] = val;
		array[305][0] = val;
		array[306][0] = val;
		array[307][0] = val;
		array[308][0] = val;
		array[567][0] = val;
		array[349][0] = new Color(144, 148, 144);
		array[531][0] = new Color(144, 148, 144);
		array[105][0] = new Color(144, 148, 144);
		array[105][1] = new Color(177, 92, 31);
		array[105][2] = new Color(201, 188, 170);
		array[137][0] = new Color(144, 148, 144);
		array[137][1] = new Color(141, 56, 0);
		array[137][2] = new Color(144, 148, 144);
		array[16][0] = new Color(140, 130, 116);
		array[26][0] = new Color(119, 101, 125);
		array[26][1] = new Color(214, 127, 133);
		array[695][0] = array[26][0];
		array[695][1] = array[26][1];
		array[36][0] = new Color(230, 89, 92);
		array[28][0] = new Color(151, 79, 80);
		array[28][1] = new Color(90, 139, 140);
		array[28][2] = new Color(192, 136, 70);
		array[28][3] = new Color(203, 185, 151);
		array[28][4] = new Color(73, 56, 41);
		array[28][5] = new Color(148, 159, 67);
		array[28][6] = new Color(138, 172, 67);
		array[28][7] = new Color(226, 122, 47);
		array[28][8] = new Color(198, 87, 93);
		for (int m = 0; m < array[653].Length; m++)
		{
			array[653][m] = array[28][m];
		}
		array[29][0] = new Color(175, 105, 128);
		array[51][0] = new Color(192, 202, 203);
		array[697][0] = array[51][0];
		array[698][0] = new Color(200, 200, 200);
		array[31][0] = new Color(141, 120, 168);
		array[31][1] = new Color(212, 105, 105);
		array[696][0] = array[31][0];
		array[696][1] = array[31][1];
		array[32][0] = new Color(151, 135, 183);
		array[42][0] = new Color(251, 235, 127);
		array[50][0] = new Color(170, 48, 114);
		array[707][0] = array[50][0];
		array[85][0] = new Color(192, 192, 192);
		array[69][0] = new Color(190, 150, 92);
		array[77][0] = new Color(238, 85, 70);
		array[81][0] = new Color(245, 133, 191);
		array[78][0] = new Color(121, 110, 97);
		array[141][0] = new Color(192, 59, 59);
		array[129][0] = new Color(255, 117, 224);
		array[129][1] = new Color(255, 117, 224);
		array[126][0] = new Color(159, 209, 229);
		array[125][0] = new Color(141, 175, 255);
		array[103][0] = new Color(141, 98, 77);
		array[95][0] = new Color(255, 162, 31);
		array[92][0] = new Color(213, 229, 237);
		array[91][0] = new Color(13, 88, 130);
		array[215][0] = new Color(254, 121, 2);
		array[592][0] = new Color(254, 121, 2);
		array[316][0] = new Color(157, 176, 226);
		array[317][0] = new Color(118, 227, 129);
		array[318][0] = new Color(227, 118, 215);
		array[319][0] = new Color(96, 68, 48);
		array[320][0] = new Color(203, 185, 151);
		array[321][0] = new Color(96, 77, 64);
		array[574][0] = new Color(76, 57, 44);
		array[322][0] = new Color(198, 170, 104);
		array[635][0] = new Color(145, 120, 120);
		array[149][0] = new Color(220, 50, 50);
		array[149][1] = new Color(0, 220, 50);
		array[149][2] = new Color(50, 50, 220);
		array[133][0] = new Color(231, 53, 56);
		array[133][1] = new Color(192, 189, 221);
		array[134][0] = new Color(166, 187, 153);
		array[134][1] = new Color(241, 129, 249);
		array[102][0] = new Color(229, 212, 73);
		array[35][0] = new Color(226, 145, 30);
		array[34][0] = new Color(235, 166, 135);
		array[136][0] = new Color(213, 203, 204);
		array[231][0] = new Color(224, 194, 101);
		array[239][0] = new Color(224, 194, 101);
		array[240][0] = new Color(120, 85, 60);
		array[240][1] = new Color(99, 50, 30);
		array[240][2] = new Color(153, 153, 117);
		array[240][3] = new Color(112, 84, 56);
		array[240][4] = new Color(234, 231, 226);
		array[241][0] = new Color(77, 74, 72);
		array[244][0] = new Color(200, 245, 253);
		val = new Color(99, 50, 30);
		array[242][0] = val;
		array[245][0] = val;
		array[246][0] = val;
		array[242][1] = new Color(185, 142, 97);
		array[247][0] = new Color(140, 150, 150);
		array[271][0] = new Color(107, 250, 255);
		array[270][0] = new Color(187, 255, 107);
		array[581][0] = new Color(255, 150, 150);
		array[660][0] = new Color(255, 150, 150);
		array[572][0] = new Color(255, 186, 212);
		array[572][1] = new Color(209, 201, 255);
		array[572][2] = new Color(200, 254, 255);
		array[572][3] = new Color(199, 255, 211);
		array[572][4] = new Color(180, 209, 255);
		array[572][5] = new Color(255, 220, 214);
		array[314][0] = new Color(181, 164, 125);
		array[324][0] = new Color(228, 213, 173);
		array[351][0] = new Color(31, 31, 31);
		array[424][0] = new Color(146, 155, 187);
		array[429][0] = new Color(220, 220, 220);
		array[445][0] = new Color(240, 240, 240);
		array[21][0] = new Color(174, 129, 92);
		array[21][1] = new Color(233, 207, 94);
		array[21][2] = new Color(137, 128, 200);
		array[21][3] = new Color(160, 160, 160);
		array[21][4] = new Color(106, 210, 255);
		array[441][0] = array[21][0];
		array[441][1] = array[21][1];
		array[441][2] = array[21][2];
		array[441][3] = array[21][3];
		array[441][4] = array[21][4];
		array[27][0] = new Color(54, 154, 54);
		array[27][1] = new Color(226, 196, 49);
		val = new Color(246, 197, 26);
		array[82][0] = val;
		array[83][0] = val;
		array[84][0] = val;
		val = new Color(76, 150, 216);
		array[82][1] = val;
		array[83][1] = val;
		array[84][1] = val;
		val = new Color(185, 214, 42);
		array[82][2] = val;
		array[83][2] = val;
		array[84][2] = val;
		val = new Color(167, 203, 37);
		array[82][3] = val;
		array[83][3] = val;
		array[84][3] = val;
		array[591][6] = val;
		val = new Color(32, 168, 117);
		array[82][4] = val;
		array[83][4] = val;
		array[84][4] = val;
		val = new Color(177, 69, 49);
		array[82][5] = val;
		array[83][5] = val;
		array[84][5] = val;
		val = new Color(40, 152, 240);
		array[82][6] = val;
		array[83][6] = val;
		array[84][6] = val;
		array[591][1] = new Color(246, 197, 26);
		array[591][2] = new Color(76, 150, 216);
		array[591][3] = new Color(32, 168, 117);
		array[591][4] = new Color(40, 152, 240);
		array[591][5] = new Color(114, 81, 56);
		array[591][6] = new Color(141, 137, 223);
		array[591][7] = new Color(208, 80, 80);
		array[591][8] = new Color(177, 69, 49);
		array[165][0] = new Color(115, 173, 229);
		array[165][1] = new Color(100, 100, 100);
		array[165][2] = new Color(152, 152, 152);
		array[165][3] = new Color(227, 125, 22);
		for (int n = 0; n < array[693].Length; n++)
		{
			array[693][n] = array[165][n];
			array[694][n] = array[165][n];
		}
		array[178][0] = new Color(208, 94, 201);
		array[178][1] = new Color(233, 146, 69);
		array[178][2] = new Color(71, 146, 251);
		array[178][3] = new Color(60, 226, 133);
		array[178][4] = new Color(250, 30, 71);
		array[178][5] = new Color(166, 176, 204);
		array[178][6] = new Color(255, 217, 120);
		val = new Color(99, 99, 99);
		array[185][0] = val;
		array[186][0] = val;
		array[187][0] = val;
		array[565][0] = val;
		array[579][0] = val;
		val = new Color(114, 81, 56);
		array[185][1] = val;
		array[186][1] = val;
		array[187][1] = val;
		array[591][0] = val;
		val = new Color(133, 133, 101);
		array[185][2] = val;
		array[186][2] = val;
		array[187][2] = val;
		val = new Color(151, 200, 211);
		array[185][3] = val;
		array[186][3] = val;
		array[187][3] = val;
		val = new Color(177, 183, 161);
		array[185][4] = val;
		array[186][4] = val;
		array[187][4] = val;
		val = new Color(134, 114, 38);
		array[185][5] = val;
		array[186][5] = val;
		array[187][5] = val;
		val = new Color(82, 62, 66);
		array[185][6] = val;
		array[186][6] = val;
		array[187][6] = val;
		val = new Color(143, 117, 121);
		array[185][7] = val;
		array[186][7] = val;
		array[187][7] = val;
		val = new Color(177, 92, 31);
		array[185][8] = val;
		array[186][8] = val;
		array[187][8] = val;
		val = new Color(85, 73, 87);
		array[185][9] = val;
		array[186][9] = val;
		array[187][9] = val;
		val = new Color(26, 196, 84);
		array[185][10] = val;
		array[186][10] = val;
		array[187][10] = val;
		val = array[53][0];
		array[185][11] = val;
		array[186][11] = val;
		array[187][11] = val;
		Color[] array2 = array[647];
		for (int num = 0; num < array2.Length; num++)
		{
			array2[num] = array[186][num];
		}
		array2 = array[648];
		for (int num2 = 0; num2 < array2.Length; num2++)
		{
			array2[num2] = array[187][num2];
		}
		array[706][0] = array[648][4];
		array2 = array[650];
		for (int num3 = 0; num3 < array2.Length; num3++)
		{
			array2[num3] = array[185][num3];
		}
		array2 = array[649];
		for (int num4 = 0; num4 < array2.Length; num4++)
		{
			array2[num4] = array[185][num4];
		}
		array[227][0] = new Color(74, 197, 155);
		array[227][1] = new Color(54, 153, 88);
		array[227][2] = new Color(63, 126, 207);
		array[227][3] = new Color(240, 180, 4);
		array[227][4] = new Color(45, 68, 168);
		array[227][5] = new Color(61, 92, 0);
		array[227][6] = new Color(216, 112, 152);
		array[227][7] = new Color(200, 40, 24);
		array[227][8] = new Color(113, 45, 133);
		array[227][9] = new Color(235, 137, 2);
		array[227][10] = new Color(41, 152, 135);
		array[227][11] = new Color(198, 19, 78);
		array[373][0] = new Color(9, 61, 191);
		array[374][0] = new Color(253, 32, 3);
		array[375][0] = new Color(255, 156, 12);
		array[461][0] = new Color(212, 192, 100);
		array[461][1] = new Color(137, 132, 156);
		array[461][2] = new Color(148, 122, 112);
		array[461][3] = new Color(221, 201, 206);
		array[323][0] = new Color(182, 141, 86);
		array[325][0] = new Color(129, 125, 93);
		array[326][0] = new Color(9, 61, 191);
		array[327][0] = new Color(253, 32, 3);
		array[507][0] = new Color(5, 5, 5);
		array[508][0] = new Color(5, 5, 5);
		array[330][0] = new Color(226, 118, 76);
		array[331][0] = new Color(161, 172, 173);
		array[332][0] = new Color(204, 181, 72);
		array[333][0] = new Color(190, 190, 178);
		array[335][0] = new Color(217, 174, 137);
		array[336][0] = new Color(253, 62, 3);
		array[337][0] = new Color(144, 148, 144);
		array[338][0] = new Color(85, 255, 160);
		array[315][0] = new Color(235, 114, 80);
		array[641][0] = new Color(235, 125, 150);
		array[340][0] = new Color(96, 248, 2);
		array[341][0] = new Color(105, 74, 202);
		array[342][0] = new Color(29, 240, 255);
		array[343][0] = new Color(254, 202, 80);
		array[344][0] = new Color(131, 252, 245);
		array[345][0] = new Color(255, 156, 12);
		array[346][0] = new Color(149, 212, 89);
		array[642][0] = new Color(149, 212, 89);
		array[347][0] = new Color(236, 74, 79);
		array[348][0] = new Color(44, 26, 233);
		array[350][0] = new Color(55, 97, 155);
		array[352][0] = new Color(238, 97, 94);
		array[354][0] = new Color(141, 107, 89);
		array[355][0] = new Color(141, 107, 89);
		array[699][0] = new Color(141, 107, 89);
		array[463][0] = new Color(155, 214, 240);
		array[491][0] = new Color(60, 20, 160);
		array[464][0] = new Color(233, 183, 128);
		array[465][0] = new Color(51, 84, 195);
		array[466][0] = new Color(205, 153, 73);
		array[356][0] = new Color(233, 203, 24);
		array[663][0] = new Color(24, 203, 233);
		array[357][0] = new Color(168, 178, 204);
		array[367][0] = new Color(168, 178, 204);
		array[561][0] = new Color(148, 158, 184);
		array[365][0] = new Color(146, 136, 205);
		array[366][0] = new Color(223, 232, 233);
		array[368][0] = new Color(50, 46, 104);
		array[369][0] = new Color(50, 46, 104);
		array[576][0] = new Color(30, 26, 84);
		array[370][0] = new Color(127, 116, 194);
		array[49][0] = new Color(89, 201, 255);
		array[372][0] = new Color(252, 128, 201);
		array[646][0] = new Color(108, 133, 140);
		array[371][0] = new Color(249, 101, 189);
		array[376][0] = new Color(160, 120, 92);
		array[378][0] = new Color(160, 120, 100);
		array[379][0] = new Color(251, 209, 240);
		array[382][0] = new Color(28, 216, 94);
		array[383][0] = new Color(221, 136, 144);
		array[384][0] = new Color(131, 206, 12);
		array[385][0] = new Color(87, 21, 144);
		array[386][0] = new Color(127, 92, 69);
		array[387][0] = new Color(127, 92, 69);
		array[388][0] = new Color(127, 92, 69);
		array[389][0] = new Color(127, 92, 69);
		array[390][0] = new Color(253, 32, 3);
		array[397][0] = new Color(212, 192, 100);
		array[396][0] = new Color(198, 124, 78);
		array[577][0] = new Color(178, 104, 58);
		array[398][0] = new Color(100, 82, 126);
		array[399][0] = new Color(77, 76, 66);
		array[400][0] = new Color(96, 68, 117);
		array[401][0] = new Color(68, 60, 51);
		array[402][0] = new Color(174, 168, 186);
		array[403][0] = new Color(205, 152, 186);
		array[404][0] = new Color(212, 148, 88);
		array[405][0] = new Color(140, 140, 140);
		array[406][0] = new Color(120, 120, 120);
		array[407][0] = new Color(255, 227, 132);
		array[411][0] = new Color(227, 46, 46);
		array[494][0] = new Color(227, 227, 227);
		array[421][0] = new Color(65, 75, 90);
		array[422][0] = new Color(65, 75, 90);
		array[425][0] = new Color(146, 155, 187);
		array[426][0] = new Color(168, 38, 47);
		array[430][0] = new Color(39, 168, 96);
		array[431][0] = new Color(39, 94, 168);
		array[432][0] = new Color(242, 221, 100);
		array[433][0] = new Color(224, 100, 242);
		array[434][0] = new Color(197, 193, 216);
		array[727][0] = new Color(119, 22, 52);
		array[728][0] = new Color(23, 119, 79);
		array[729][0] = new Color(23, 54, 119);
		array[730][0] = new Color(119, 68, 23);
		array[731][0] = new Color(74, 23, 119);
		array[732][0] = new Color(78, 82, 109);
		array[427][0] = new Color(183, 53, 62);
		array[435][0] = new Color(54, 183, 111);
		array[436][0] = new Color(54, 109, 183);
		array[437][0] = new Color(255, 236, 115);
		array[438][0] = new Color(239, 115, 255);
		array[439][0] = new Color(212, 208, 231);
		array[440][0] = new Color(238, 51, 53);
		array[440][1] = new Color(13, 107, 216);
		array[440][2] = new Color(33, 184, 115);
		array[440][3] = new Color(255, 221, 62);
		array[440][4] = new Color(165, 0, 236);
		array[440][5] = new Color(223, 230, 238);
		array[440][6] = new Color(207, 101, 0);
		array[419][0] = new Color(88, 95, 114);
		array[419][1] = new Color(214, 225, 236);
		array[419][2] = new Color(25, 131, 205);
		array[423][0] = new Color(245, 197, 1);
		array[423][1] = new Color(185, 0, 224);
		array[423][2] = new Color(58, 240, 111);
		array[423][3] = new Color(50, 107, 197);
		array[423][4] = new Color(253, 91, 3);
		array[423][5] = new Color(254, 194, 20);
		array[423][6] = new Color(174, 195, 215);
		array[420][0] = new Color(99, 255, 107);
		array[420][1] = new Color(99, 255, 107);
		array[420][4] = new Color(99, 255, 107);
		array[420][2] = new Color(218, 2, 5);
		array[420][3] = new Color(218, 2, 5);
		array[420][5] = new Color(218, 2, 5);
		array[476][0] = new Color(160, 160, 160);
		array[410][0] = new Color(75, 139, 166);
		array[480][0] = new Color(120, 50, 50);
		array[509][0] = new Color(50, 50, 60);
		array[657][0] = new Color(35, 205, 215);
		array[658][0] = new Color(200, 105, 230);
		array[412][0] = new Color(75, 139, 166);
		array[720][0] = new Color(164, 175, 175);
		array[721][0] = new Color(77, 176, 144);
		array[725][0] = new Color(229, 65, 65);
		array[733][0] = new Color(115, 69, 27);
		array[751][0] = Color.Gray;
		array[752][0] = Color.Gray;
		array[443][0] = new Color(144, 148, 144);
		array[442][0] = new Color(3, 144, 201);
		array[444][0] = new Color(191, 176, 124);
		array[446][0] = new Color(255, 66, 152);
		array[447][0] = new Color(179, 132, 255);
		array[448][0] = new Color(0, 206, 180);
		array[449][0] = new Color(91, 186, 240);
		array[450][0] = new Color(92, 240, 91);
		array[451][0] = new Color(240, 91, 147);
		array[452][0] = new Color(255, 150, 181);
		array[453][0] = new Color(179, 132, 255);
		array[453][1] = new Color(0, 206, 180);
		array[453][2] = new Color(255, 66, 152);
		array[454][0] = new Color(174, 16, 176);
		array[455][0] = new Color(48, 225, 110);
		array[456][0] = new Color(179, 132, 255);
		array[457][0] = new Color(150, 164, 206);
		array[457][1] = new Color(255, 132, 184);
		array[457][2] = new Color(74, 255, 232);
		array[457][3] = new Color(215, 159, 255);
		array[457][4] = new Color(229, 219, 234);
		array[458][0] = new Color(211, 198, 111);
		array[459][0] = new Color(190, 223, 232);
		array[460][0] = new Color(141, 163, 181);
		array[717][0] = new Color(126, 102, 118);
		array[718][0] = new Color(245, 215, 196);
		array[719][0] = new Color(223, 255, 255);
		array[462][0] = new Color(231, 178, 28);
		array[467][0] = new Color(129, 56, 121);
		array[467][1] = new Color(255, 249, 59);
		array[467][2] = new Color(161, 67, 24);
		array[467][3] = new Color(89, 70, 72);
		array[467][4] = new Color(233, 207, 94);
		array[467][5] = new Color(254, 158, 35);
		array[467][6] = new Color(34, 221, 151);
		array[467][7] = new Color(249, 170, 236);
		array[467][8] = new Color(35, 200, 254);
		array[467][9] = new Color(190, 200, 200);
		array[467][10] = new Color(230, 170, 100);
		array[467][11] = new Color(165, 168, 26);
		array[467][12] = array[21][0];
		for (int num5 = 0; num5 < 13; num5++)
		{
			array[468][num5] = array[467][num5];
		}
		array[472][0] = new Color(190, 160, 140);
		array[726][0] = new Color(30, 30, 36);
		array[473][0] = new Color(85, 114, 123);
		array[474][0] = new Color(116, 94, 97);
		array[478][0] = new Color(108, 34, 35);
		array[479][0] = new Color(178, 114, 68);
		array[485][0] = new Color(198, 134, 88);
		array[492][0] = new Color(78, 193, 227);
		array[492][0] = new Color(78, 193, 227);
		array[493][0] = new Color(250, 249, 252);
		array[493][1] = new Color(240, 90, 90);
		array[493][2] = new Color(98, 230, 92);
		array[493][3] = new Color(95, 197, 238);
		array[493][4] = new Color(241, 221, 100);
		array[493][5] = new Color(213, 92, 237);
		array[494][0] = new Color(224, 219, 236);
		array[495][0] = new Color(253, 227, 215);
		array[496][0] = new Color(165, 159, 153);
		array[498][0] = new Color(202, 174, 165);
		array[499][0] = new Color(160, 187, 142);
		array[500][0] = new Color(254, 158, 35);
		array[501][0] = new Color(34, 221, 151);
		array[502][0] = new Color(249, 170, 236);
		array[503][0] = new Color(35, 200, 254);
		array[506][0] = new Color(61, 61, 61);
		array[510][0] = new Color(191, 142, 111);
		array[511][0] = new Color(187, 68, 74);
		array[520][0] = new Color(224, 219, 236);
		array[545][0] = new Color(255, 126, 145);
		array[530][0] = new Color(107, 182, 0);
		array[530][1] = new Color(23, 154, 209);
		array[530][2] = new Color(238, 97, 94);
		array[530][3] = new Color(113, 108, 205);
		array[705][0] = array[530][0];
		array[705][1] = array[530][1];
		array[705][2] = array[530][2];
		array[705][3] = array[530][3];
		array[546][0] = new Color(60, 60, 60);
		array[557][0] = new Color(60, 60, 60);
		array[547][0] = new Color(120, 110, 100);
		array[548][0] = new Color(120, 110, 100);
		array[562][0] = new Color(165, 168, 26);
		array[563][0] = new Color(165, 168, 26);
		array[571][0] = new Color(165, 168, 26);
		array[568][0] = new Color(248, 203, 233);
		array[569][0] = new Color(203, 248, 218);
		array[570][0] = new Color(160, 242, 255);
		array[597][0] = new Color(28, 216, 94);
		array[597][1] = new Color(183, 237, 20);
		array[597][2] = new Color(185, 83, 200);
		array[597][3] = new Color(131, 128, 168);
		array[597][4] = new Color(38, 142, 214);
		array[597][5] = new Color(229, 154, 9);
		array[597][6] = new Color(142, 227, 234);
		array[597][7] = new Color(98, 111, 223);
		array[597][8] = new Color(241, 233, 158);
		array[597][9] = new Color(238, 85, 70);
		array[597][10] = new Color(247, 228, 254);
		array[617][0] = new Color(233, 207, 94);
		Color val3 = new Color(250, 100, 50);
		array[548][1] = val3;
		array[613][0] = val3;
		array[614][0] = val3;
		array[623][0] = new Color(220, 210, 245);
		array[661][0] = new Color(141, 137, 223);
		array[662][0] = new Color(208, 80, 80);
		array[666][0] = new Color(115, 60, 40);
		array[667][0] = new Color(247, 228, 254);
		array[708][0] = new Color(237, 218, 244);
		array[709][0] = new Color(247, 228, 254);
		array[722][0] = new Color(185, 190, 20);
		array[723][0] = new Color(185, 190, 20);
		array[724][0] = new Color(185, 190, 20);
		array[734][0] = new Color(160, 115, 80);
		array[735][0] = new Color(235, 200, 20);
		array[736][0] = new Color(240, 130, 160);
		array[737][0] = new Color(65, 75, 65);
		array[738][0] = new Color(210, 170, 215);
		array[739][0] = new Color(90, 125, 235);
		array[740][0] = Color.LightGray;
		array[741][0] = new Color(160, 90, 50);
		array[742][0] = new Color(26, 141, 204);
		array[743][0] = new Color(227, 227, 227);
		array[744][0] = new Color(106, 103, 96);
		array[745][0] = new Color(120, 114, 108);
		array[746][0] = new Color(122, 78, 21);
		array[747][0] = new Color(231, 178, 28);
		array[748][0] = new Color(2, 114, 206);
		array[749][0] = MultiplyMapColor(array[138][0], 0.95f);
		array[750][0] = MultiplyMapColor(array[48][0], 0.95f);
		array[753][0] = MultiplyMapColor(array[158][0], 0.95f);
		Color[] array3 = new Color[4]
		{
			new Color(9, 61, 191),
			new Color(253, 32, 3),
			new Color(254, 194, 20),
			new Color(161, 127, 255)
		};
		Color[][] array4 = new Color[WallID.Count][];
		for (int num6 = 0; num6 < WallID.Count; num6++)
		{
			array4[num6] = new Color[2];
		}
		array4[158][0] = new Color(107, 49, 154);
		array4[163][0] = new Color(154, 148, 49);
		array4[162][0] = new Color(49, 49, 154);
		array4[160][0] = new Color(49, 154, 68);
		array4[161][0] = new Color(154, 49, 77);
		array4[159][0] = new Color(85, 89, 118);
		array4[157][0] = new Color(154, 83, 49);
		array4[154][0] = new Color(221, 79, 255);
		array4[166][0] = new Color(250, 255, 79);
		array4[165][0] = new Color(79, 102, 255);
		array4[156][0] = new Color(79, 255, 89);
		array4[164][0] = new Color(255, 79, 79);
		array4[155][0] = new Color(240, 240, 247);
		array4[153][0] = new Color(255, 145, 79);
		array4[169][0] = new Color(5, 5, 5);
		array4[224][0] = new Color(57, 55, 52);
		array4[323][0] = new Color(55, 25, 33);
		array4[324][0] = new Color(60, 55, 145);
		array4[325][0] = new Color(10, 5, 50);
		array4[326][0] = new Color(30, 105, 75);
		array4[327][0] = new Color(5, 45, 55);
		array4[328][0] = new Color(20, 25, 35);
		array4[329][0] = new Color(15, 10, 50);
		array4[330][0] = new Color(153, 164, 187);
		array4[225][0] = new Color(68, 68, 68);
		array4[226][0] = new Color(148, 138, 74);
		array4[227][0] = new Color(95, 137, 191);
		array4[170][0] = new Color(59, 39, 22);
		array4[171][0] = new Color(59, 39, 22);
		val = new Color(52, 52, 52);
		array4[1][0] = val;
		array4[53][0] = val;
		array4[52][0] = val;
		array4[51][0] = val;
		array4[50][0] = val;
		array4[49][0] = val;
		array4[48][0] = val;
		array4[44][0] = val;
		array4[346][0] = val;
		array4[5][0] = val;
		array4[349][0] = val;
		array4[350][0] = new Color(0, 255, 0);
		val = new Color(88, 61, 46);
		array4[2][0] = val;
		array4[16][0] = val;
		array4[59][0] = val;
		array4[3][0] = new Color(61, 58, 78);
		array4[4][0] = new Color(73, 51, 36);
		array4[106][0] = new Color(148, 116, 74);
		array4[6][0] = new Color(91, 30, 30);
		val = new Color(27, 31, 42);
		array4[7][0] = val;
		array4[17][0] = val;
		array4[331][0] = val;
		val = new Color(32, 40, 45);
		array4[94][0] = val;
		array4[100][0] = val;
		val = new Color(44, 41, 50);
		array4[95][0] = val;
		array4[101][0] = val;
		val = new Color(31, 39, 26);
		array4[8][0] = val;
		array4[18][0] = val;
		array4[332][0] = val;
		val = new Color(36, 45, 44);
		array4[98][0] = val;
		array4[104][0] = val;
		val = new Color(38, 49, 50);
		array4[99][0] = val;
		array4[105][0] = val;
		val = new Color(41, 28, 36);
		array4[9][0] = val;
		array4[19][0] = val;
		array4[333][0] = val;
		val = new Color(72, 50, 77);
		array4[96][0] = val;
		array4[102][0] = val;
		val = new Color(78, 50, 69);
		array4[97][0] = val;
		array4[103][0] = val;
		array4[10][0] = new Color(74, 62, 12);
		array4[334][0] = new Color(74, 62, 12);
		array4[11][0] = new Color(46, 56, 59);
		array4[335][0] = new Color(46, 56, 59);
		array4[12][0] = new Color(75, 32, 11);
		array4[336][0] = new Color(75, 32, 11);
		array4[13][0] = new Color(67, 37, 37);
		array4[338][0] = new Color(67, 37, 37);
		val = new Color(15, 15, 15);
		array4[14][0] = val;
		array4[337][0] = val;
		array4[20][0] = val;
		array4[15][0] = new Color(52, 43, 45);
		array4[22][0] = new Color(113, 99, 99);
		array4[23][0] = new Color(38, 38, 43);
		array4[24][0] = new Color(53, 39, 41);
		array4[25][0] = new Color(11, 35, 62);
		array4[339][0] = new Color(11, 35, 62);
		array4[26][0] = new Color(21, 63, 70);
		array4[340][0] = new Color(21, 63, 70);
		array4[27][0] = new Color(88, 61, 46);
		array4[27][1] = new Color(52, 52, 52);
		array4[28][0] = new Color(81, 84, 101);
		array4[29][0] = new Color(88, 23, 23);
		array4[30][0] = new Color(28, 88, 23);
		array4[31][0] = new Color(78, 87, 99);
		val = new Color(69, 67, 41);
		array4[34][0] = val;
		array4[37][0] = val;
		array4[32][0] = new Color(86, 17, 40);
		array4[33][0] = new Color(49, 47, 83);
		array4[35][0] = new Color(51, 51, 70);
		array4[36][0] = new Color(87, 59, 55);
		array4[38][0] = new Color(49, 57, 49);
		array4[39][0] = new Color(78, 79, 73);
		array4[45][0] = new Color(60, 59, 51);
		array4[46][0] = new Color(48, 57, 47);
		array4[47][0] = new Color(71, 77, 85);
		array4[40][0] = new Color(85, 102, 103);
		array4[41][0] = new Color(52, 50, 62);
		array4[42][0] = new Color(71, 42, 44);
		array4[43][0] = new Color(73, 66, 50);
		array4[54][0] = new Color(40, 56, 50);
		array4[55][0] = new Color(49, 48, 36);
		array4[56][0] = new Color(43, 33, 32);
		array4[57][0] = new Color(31, 40, 49);
		array4[58][0] = new Color(48, 35, 52);
		array4[60][0] = new Color(1, 52, 20);
		array4[61][0] = new Color(55, 39, 26);
		array4[62][0] = new Color(39, 33, 26);
		array4[69][0] = new Color(43, 42, 68);
		array4[70][0] = new Color(30, 70, 80);
		array4[341][0] = new Color(100, 40, 1);
		array4[342][0] = new Color(92, 30, 72);
		array4[343][0] = new Color(42, 81, 1);
		array4[344][0] = new Color(1, 81, 109);
		array4[345][0] = new Color(56, 22, 97);
		val = new Color(30, 80, 48);
		array4[63][0] = val;
		array4[65][0] = val;
		array4[66][0] = val;
		array4[68][0] = val;
		val = new Color(53, 80, 30);
		array4[64][0] = val;
		array4[67][0] = val;
		array4[78][0] = new Color(63, 39, 26);
		array4[244][0] = new Color(63, 39, 26);
		array4[71][0] = new Color(78, 105, 135);
		array4[72][0] = new Color(52, 84, 12);
		array4[73][0] = new Color(190, 204, 223);
		val = new Color(64, 62, 80);
		array4[74][0] = val;
		array4[80][0] = val;
		array4[75][0] = new Color(65, 65, 35);
		array4[76][0] = new Color(20, 46, 104);
		array4[77][0] = new Color(61, 13, 16);
		array4[79][0] = new Color(51, 47, 96);
		array4[81][0] = new Color(101, 51, 51);
		array4[82][0] = new Color(77, 64, 34);
		array4[83][0] = new Color(62, 38, 41);
		array4[234][0] = new Color(60, 36, 39);
		array4[84][0] = new Color(48, 78, 93);
		array4[85][0] = new Color(54, 63, 69);
		val = new Color(138, 73, 38);
		array4[86][0] = val;
		array4[108][0] = val;
		val = new Color(50, 15, 8);
		array4[87][0] = val;
		array4[112][0] = val;
		array4[109][0] = new Color(94, 25, 17);
		array4[110][0] = new Color(125, 36, 122);
		array4[111][0] = new Color(51, 35, 27);
		array4[113][0] = new Color(135, 58, 0);
		array4[114][0] = new Color(65, 52, 15);
		array4[115][0] = new Color(39, 42, 51);
		array4[116][0] = new Color(89, 26, 27);
		array4[117][0] = new Color(126, 123, 115);
		array4[118][0] = new Color(8, 50, 19);
		array4[119][0] = new Color(95, 21, 24);
		array4[120][0] = new Color(17, 31, 65);
		array4[121][0] = new Color(192, 173, 143);
		array4[122][0] = new Color(114, 114, 131);
		array4[123][0] = new Color(136, 119, 7);
		array4[124][0] = new Color(8, 72, 3);
		array4[125][0] = new Color(117, 132, 82);
		array4[126][0] = new Color(100, 102, 114);
		array4[127][0] = new Color(30, 118, 226);
		array4[128][0] = new Color(93, 6, 102);
		array4[129][0] = new Color(64, 40, 169);
		array4[130][0] = new Color(39, 34, 180);
		array4[131][0] = new Color(87, 94, 125);
		array4[132][0] = new Color(6, 6, 6);
		array4[133][0] = new Color(69, 72, 186);
		array4[134][0] = new Color(130, 62, 16);
		array4[135][0] = new Color(22, 123, 163);
		array4[136][0] = new Color(40, 86, 151);
		array4[137][0] = new Color(183, 75, 15);
		array4[138][0] = new Color(83, 80, 100);
		array4[139][0] = new Color(115, 65, 68);
		array4[140][0] = new Color(119, 108, 81);
		array4[141][0] = new Color(59, 67, 71);
		array4[142][0] = new Color(222, 216, 202);
		array4[143][0] = new Color(90, 112, 105);
		array4[144][0] = new Color(62, 28, 87);
		array4[146][0] = new Color(120, 59, 19);
		array4[147][0] = new Color(59, 59, 59);
		array4[148][0] = new Color(229, 218, 161);
		Color val4 = new Color(73, 59, 50);
		array4[149][0] = val4;
		array4[150][0] = val4;
		Color val5 = new Color(102, 75, 34);
		array4[151][0] = val5;
		array4[152][0] = val5;
		array4[167][0] = new Color(70, 68, 51);
		array4[316][0] = new Color(125, 100, 100);
		array4[317][0] = new Color(143, 117, 121);
		array4[172][0] = new Color(163, 96, 0);
		array4[242][0] = new Color(5, 5, 5);
		array4[243][0] = new Color(5, 5, 5);
		array4[173][0] = new Color(94, 163, 46);
		array4[174][0] = new Color(117, 32, 59);
		array4[175][0] = new Color(20, 11, 203);
		array4[176][0] = new Color(74, 69, 88);
		array4[177][0] = new Color(60, 30, 30);
		array4[183][0] = new Color(111, 117, 135);
		array4[179][0] = new Color(111, 117, 135);
		array4[178][0] = new Color(111, 117, 135);
		array4[184][0] = new Color(25, 23, 54);
		array4[181][0] = new Color(25, 23, 54);
		array4[180][0] = new Color(25, 23, 54);
		array4[182][0] = new Color(74, 71, 129);
		array4[185][0] = new Color(52, 52, 52);
		array4[186][0] = new Color(38, 9, 66);
		array4[216][0] = new Color(158, 100, 64);
		array4[217][0] = new Color(62, 45, 75);
		array4[218][0] = new Color(57, 14, 12);
		array4[219][0] = new Color(96, 72, 133);
		array4[187][0] = new Color(149, 80, 51);
		array4[235][0] = new Color(140, 75, 48);
		array4[220][0] = new Color(67, 55, 80);
		array4[221][0] = new Color(64, 37, 29);
		array4[222][0] = new Color(70, 51, 91);
		array4[188][0] = new Color(82, 63, 80);
		array4[189][0] = new Color(65, 61, 77);
		array4[190][0] = new Color(64, 65, 92);
		array4[191][0] = new Color(76, 53, 84);
		array4[192][0] = new Color(144, 67, 52);
		array4[193][0] = new Color(149, 48, 48);
		array4[194][0] = new Color(111, 32, 36);
		array4[195][0] = new Color(147, 48, 55);
		array4[196][0] = new Color(97, 67, 51);
		array4[197][0] = new Color(112, 80, 62);
		array4[198][0] = new Color(88, 61, 46);
		array4[199][0] = new Color(127, 94, 76);
		array4[200][0] = new Color(143, 50, 123);
		array4[201][0] = new Color(136, 120, 131);
		array4[202][0] = new Color(219, 92, 143);
		array4[203][0] = new Color(113, 64, 150);
		array4[204][0] = new Color(74, 67, 60);
		array4[205][0] = new Color(60, 78, 59);
		array4[206][0] = new Color(0, 54, 21);
		array4[207][0] = new Color(74, 97, 72);
		array4[208][0] = new Color(40, 37, 35);
		array4[209][0] = new Color(77, 63, 66);
		array4[210][0] = new Color(111, 6, 6);
		array4[211][0] = new Color(88, 67, 59);
		array4[212][0] = new Color(88, 87, 80);
		array4[213][0] = new Color(71, 71, 67);
		array4[214][0] = new Color(76, 52, 60);
		array4[215][0] = new Color(89, 48, 59);
		array4[223][0] = new Color(51, 18, 4);
		array4[228][0] = new Color(160, 2, 75);
		array4[229][0] = new Color(100, 55, 164);
		array4[230][0] = new Color(0, 117, 101);
		array4[236][0] = new Color(127, 49, 44);
		array4[231][0] = new Color(110, 90, 78);
		array4[232][0] = new Color(47, 69, 75);
		array4[145][0] = new Color(144, 144, 144);
		array4[107][0] = new Color(60, 60, 60);
		array4[233][0] = new Color(91, 67, 70);
		array4[237][0] = new Color(200, 44, 18);
		array4[238][0] = new Color(24, 93, 66);
		array4[239][0] = new Color(160, 87, 234);
		array4[240][0] = new Color(6, 106, 255);
		array4[245][0] = new Color(102, 102, 102);
		array4[315][0] = new Color(181, 230, 29);
		array4[246][0] = new Color(61, 58, 78);
		array4[247][0] = new Color(52, 43, 45);
		array4[248][0] = new Color(81, 84, 101);
		array4[249][0] = new Color(85, 102, 103);
		array4[250][0] = new Color(52, 52, 52);
		array4[251][0] = new Color(52, 52, 52);
		array4[252][0] = new Color(52, 52, 52);
		array4[253][0] = new Color(52, 52, 52);
		array4[254][0] = new Color(52, 52, 52);
		array4[255][0] = new Color(52, 52, 52);
		array4[314][0] = new Color(52, 52, 52);
		array4[256][0] = new Color(40, 56, 50);
		array4[257][0] = new Color(49, 48, 36);
		array4[258][0] = new Color(43, 33, 32);
		array4[259][0] = new Color(31, 40, 49);
		array4[260][0] = new Color(48, 35, 52);
		array4[261][0] = new Color(88, 61, 46);
		array4[262][0] = new Color(55, 39, 26);
		array4[263][0] = new Color(39, 33, 26);
		array4[264][0] = new Color(43, 42, 68);
		array4[265][0] = new Color(30, 70, 80);
		array4[266][0] = new Color(78, 105, 135);
		array4[267][0] = new Color(51, 47, 96);
		array4[268][0] = new Color(101, 51, 51);
		array4[269][0] = new Color(62, 38, 41);
		array4[270][0] = new Color(59, 39, 22);
		array4[271][0] = new Color(59, 39, 22);
		array4[272][0] = new Color(111, 117, 135);
		array4[273][0] = new Color(25, 23, 54);
		array4[274][0] = new Color(52, 52, 52);
		array4[275][0] = new Color(149, 80, 51);
		array4[276][0] = new Color(82, 63, 80);
		array4[277][0] = new Color(65, 61, 77);
		array4[278][0] = new Color(64, 65, 92);
		array4[279][0] = new Color(76, 53, 84);
		array4[280][0] = new Color(144, 67, 52);
		array4[281][0] = new Color(149, 48, 48);
		array4[282][0] = new Color(111, 32, 36);
		array4[283][0] = new Color(147, 48, 55);
		array4[284][0] = new Color(97, 67, 51);
		array4[285][0] = new Color(112, 80, 62);
		array4[286][0] = new Color(88, 61, 46);
		array4[287][0] = new Color(127, 94, 76);
		array4[288][0] = new Color(143, 50, 123);
		array4[289][0] = new Color(136, 120, 131);
		array4[290][0] = new Color(219, 92, 143);
		array4[291][0] = new Color(113, 64, 150);
		array4[292][0] = new Color(74, 67, 60);
		array4[293][0] = new Color(60, 78, 59);
		array4[294][0] = new Color(0, 54, 21);
		array4[295][0] = new Color(74, 97, 72);
		array4[296][0] = new Color(40, 37, 35);
		array4[297][0] = new Color(77, 63, 66);
		array4[298][0] = new Color(111, 6, 6);
		array4[299][0] = new Color(88, 67, 59);
		array4[300][0] = new Color(88, 87, 80);
		array4[301][0] = new Color(71, 71, 67);
		array4[302][0] = new Color(76, 52, 60);
		array4[303][0] = new Color(89, 48, 59);
		array4[304][0] = new Color(158, 100, 64);
		array4[305][0] = new Color(62, 45, 75);
		array4[306][0] = new Color(57, 14, 12);
		array4[307][0] = new Color(96, 72, 133);
		array4[308][0] = new Color(67, 55, 80);
		array4[309][0] = new Color(64, 37, 29);
		array4[310][0] = new Color(70, 51, 91);
		array4[311][0] = new Color(51, 18, 4);
		array4[312][0] = new Color(78, 110, 51);
		array4[313][0] = new Color(78, 110, 51);
		array4[319][0] = new Color(105, 51, 108);
		array4[320][0] = new Color(75, 30, 15);
		array4[321][0] = new Color(91, 108, 130);
		array4[322][0] = new Color(91, 108, 130);
		array4[347][0] = new Color(100, 65, 130);
		array4[348][0] = new Color(120, 75, 75);
		float multiplier = 0.5f;
		array4[351][0] = MultiplyMapColor(array[734][0], multiplier);
		array4[352][0] = MultiplyMapColor(array[735][0], multiplier);
		array4[353][0] = MultiplyMapColor(array[170][0], multiplier);
		array4[354][0] = MultiplyMapColor(array[736][0], multiplier);
		array4[355][0] = MultiplyMapColor(array[737][0], multiplier);
		array4[356][0] = MultiplyMapColor(array[738][0], multiplier);
		array4[357][0] = MultiplyMapColor(array[739][0], multiplier);
		array4[358][0] = MultiplyMapColor(array[741][0], multiplier);
		array4[359][0] = MultiplyMapColor(array[742][0], multiplier);
		array4[360][0] = new Color(73, 93, 116);
		array4[361][0] = MultiplyMapColor(array[744][0], multiplier);
		array4[362][0] = MultiplyMapColor(array[745][0], multiplier);
		array4[363][0] = MultiplyMapColor(array[746][0], multiplier);
		array4[364][0] = MultiplyMapColor(array[747][0], multiplier);
		array4[365][0] = MultiplyMapColor(array[748][0], multiplier);
		array4[366][0] = MultiplyMapColor(array[749][0], multiplier);
		Color[] array5 = new Color[256];
		Color val6 = new Color(50, 40, 255);
		Color val7 = new Color(145, 185, 255);
		for (int num7 = 0; num7 < array5.Length; num7++)
		{
			float num8 = (float)num7 / (float)array5.Length;
			float num9 = 1f - num8;
			array5[num7] = new Color((int)(byte)((float)(int)val6.R * num9 + (float)(int)val7.R * num8), (int)(byte)((float)(int)val6.G * num9 + (float)(int)val7.G * num8), (int)(byte)((float)(int)val6.B * num9 + (float)(int)val7.B * num8));
		}
		Color[] array6 = new Color[256];
		Color val8 = new Color(88, 61, 46);
		Color val9 = new Color(37, 78, 123);
		for (int num10 = 0; num10 < array6.Length; num10++)
		{
			float num11 = (float)num10 / 255f;
			float num12 = 1f - num11;
			array6[num10] = new Color((int)(byte)((float)(int)val8.R * num12 + (float)(int)val9.R * num11), (int)(byte)((float)(int)val8.G * num12 + (float)(int)val9.G * num11), (int)(byte)((float)(int)val8.B * num12 + (float)(int)val9.B * num11));
		}
		Color[] array7 = new Color[256];
		Color val10 = new Color(74, 67, 60);
		val9 = new Color(53, 70, 97);
		for (int num13 = 0; num13 < array7.Length; num13++)
		{
			float num14 = (float)num13 / 255f;
			float num15 = 1f - num14;
			array7[num13] = new Color((int)(byte)((float)(int)val10.R * num15 + (float)(int)val9.R * num14), (int)(byte)((float)(int)val10.G * num15 + (float)(int)val9.G * num14), (int)(byte)((float)(int)val10.B * num15 + (float)(int)val9.B * num14));
		}
		Color val11 = new Color(50, 44, 38);
		int num16 = 0;
		tileOptionCounts = new int[TileID.Count];
		for (int num17 = 0; num17 < TileID.Count; num17++)
		{
			Color[] array8 = array[num17];
			int num18;
			for (num18 = 0; num18 < 13 && !(array8[num18] == Color.Transparent); num18++)
			{
			}
			tileOptionCounts[num17] = num18;
			num16 += num18;
		}
		wallOptionCounts = new int[WallID.Count];
		for (int num19 = 0; num19 < WallID.Count; num19++)
		{
			Color[] array9 = array4[num19];
			int num20;
			for (num20 = 0; num20 < 2 && !(array9[num20] == Color.Transparent); num20++)
			{
			}
			wallOptionCounts[num19] = num20;
			num16 += num20;
		}
		num16 += 774;
		colorLookup = new Color[num16];
		colorLookup[0] = Color.Transparent;
		ushort num21 = (tilePosition = 1);
		tileLookup = new ushort[TileID.Count];
		for (int num22 = 0; num22 < TileID.Count; num22++)
		{
			if (tileOptionCounts[num22] > 0)
			{
				_ = array[num22];
				tileLookup[num22] = num21;
				for (int num23 = 0; num23 < tileOptionCounts[num22]; num23++)
				{
					colorLookup[num21] = array[num22][num23];
					num21++;
				}
			}
			else
			{
				tileLookup[num22] = 0;
			}
		}
		wallPosition = num21;
		wallLookup = new ushort[WallID.Count];
		wallRangeStart = num21;
		for (int num24 = 0; num24 < WallID.Count; num24++)
		{
			if (wallOptionCounts[num24] > 0)
			{
				_ = array4[num24];
				wallLookup[num24] = num21;
				for (int num25 = 0; num25 < wallOptionCounts[num24]; num25++)
				{
					colorLookup[num21] = array4[num24][num25];
					num21++;
				}
			}
			else
			{
				wallLookup[num24] = 0;
			}
		}
		wallRangeEnd = num21;
		liquidPosition = num21;
		for (int num26 = 0; num26 < 4; num26++)
		{
			colorLookup[num21] = array3[num26];
			num21++;
		}
		skyPosition = num21;
		for (int num27 = 0; num27 < 256; num27++)
		{
			colorLookup[num21] = array5[num27];
			num21++;
		}
		dirtPosition = num21;
		for (int num28 = 0; num28 < 256; num28++)
		{
			colorLookup[num21] = array6[num28];
			num21++;
		}
		rockPosition = num21;
		for (int num29 = 0; num29 < 256; num29++)
		{
			colorLookup[num21] = array7[num29];
			num21++;
		}
		hellPosition = num21;
		colorLookup[num21] = val11;
		snowTypes = new ushort[6];
		snowTypes[0] = tileLookup[147];
		snowTypes[1] = tileLookup[161];
		snowTypes[2] = tileLookup[162];
		snowTypes[3] = tileLookup[163];
		snowTypes[4] = tileLookup[164];
		snowTypes[5] = tileLookup[200];
		Lang.BuildMapAtlas();
	}

	public static Color MultiplyMapColor(Color color, float multiplier)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		byte a = color.A;
		color *= multiplier;
		color.A = a;
		return color;
	}

	public static bool HasOption(int tileType, int option)
	{
		return option < tileOptionCounts[tileType];
	}

	public static int TileToLookup(int tileType, int option)
	{
		Invariant.Assert(tileType >= 0 && tileType <= TileID.Count, "MapHelper.TileToLookup passed a bad tileType: {0}", tileType);
		Invariant.Assert(option >= 0, "MapHelper.TileToLookup passed a bad option: {0}", option);
		Invariant.Assert(option < tileOptionCounts[tileType], "MapHelper.TileToLookup passed an option for tileType {0} that does not exist: {1}", tileType, option);
		return tileLookup[tileType] + option;
	}

	public static int LookupCount()
	{
		return colorLookup.Length;
	}

	public static void CaptureSceneState(SceneMetrics metrics)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		sceneArea = Utils.CenteredRectangle(metrics.TileCenter, SceneMetrics.ZoneScanSize);
		sceneSnowiness = (float)metrics.SnowTileCount / (float)SceneMetrics.SnowTileMax;
	}

	private static void MapColor(ushort type, ref Color oldColor, byte colorType)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		Color val = WorldGen.paintColor(colorType);
		float num = (float)(int)oldColor.R / 255f;
		float num2 = (float)(int)oldColor.G / 255f;
		float num3 = (float)(int)oldColor.B / 255f;
		if (num2 > num)
		{
			float num4 = num;
			num = num2;
			num2 = num4;
		}
		if (num3 > num)
		{
			float num5 = num;
			num = num3;
			num3 = num5;
		}
		switch (colorType)
		{
		case 29:
		{
			float num7 = num3 * 0.3f;
			oldColor.R = (byte)((float)(int)val.R * num7);
			oldColor.G = (byte)((float)(int)val.G * num7);
			oldColor.B = (byte)((float)(int)val.B * num7);
			break;
		}
		case 30:
			if (type >= wallRangeStart && type <= wallRangeEnd)
			{
				oldColor.R = (byte)((float)(255 - oldColor.R) * 0.5f);
				oldColor.G = (byte)((float)(255 - oldColor.G) * 0.5f);
				oldColor.B = (byte)((float)(255 - oldColor.B) * 0.5f);
			}
			else
			{
				oldColor.R = (byte)(255 - oldColor.R);
				oldColor.G = (byte)(255 - oldColor.G);
				oldColor.B = (byte)(255 - oldColor.B);
			}
			break;
		default:
		{
			float num6 = num;
			oldColor.R = (byte)((float)(int)val.R * num6);
			oldColor.G = (byte)((float)(int)val.G * num6);
			oldColor.B = (byte)((float)(int)val.B * num6);
			break;
		}
		}
	}

	public static Color GetMapTileXnaColor(MapTile tile, int i, int j)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		Color oldColor = colorLookup[tile.Type];
		byte color = tile.Color;
		if (color > 0)
		{
			MapColor(tile.Type, ref oldColor, color);
		}
		if (tile.Light == byte.MaxValue)
		{
			return oldColor;
		}
		float num = (float)(int)tile.Light / 255f;
		oldColor.R = (byte)((float)(int)oldColor.R * num);
		oldColor.G = (byte)((float)(int)oldColor.G * num);
		oldColor.B = (byte)((float)(int)oldColor.B * num);
		return oldColor;
	}

	public static bool IsBackground(int mapType)
	{
		return mapType >= skyPosition;
	}

	public static MapTile CreateMapTile(int i, int j, byte Light, int backgroundOverride = 0)
	{
		Tile tile = Main.tile[i, j];
		if (tile == null)
		{
			return default;
		}
		int newColor = 0;
		int newLight = Light;
		int baseType = 0;
		int baseOption = 0;
		GetTileType(i, j, tile, ref newColor, ref newLight, ref baseType, ref baseOption);
		if (baseType == 0)
		{
			GetWallType(i, j, tile, ref newColor, ref newLight, ref baseType, ref baseOption);
		}
		if (baseType == 0)
		{
			newColor = 0;
			newLight = Light;
			baseType = ((backgroundOverride == 0) ? GetBackgroundType(i, j, ref newLight) : backgroundOverride);
		}
		return MapTile.Create((ushort)(baseType + baseOption), (byte)newLight, (byte)newColor);
	}

	private static void GetTileType(int i, int j, Tile tileCache, ref int newColor, ref int newLight, ref int baseType, ref int baseOption)
	{
		if ((DebugOptions.ShowUnbreakableWall && tileCache.wall == 350) || !tileCache.active())
		{
			return;
		}
		int num = tileCache.type;
		baseType = tileLookup[num];
		bool flag = tileCache.invisibleBlock();
		if (tileCache.fullbrightBlock() && !flag)
		{
			newLight = 255;
		}
		if (flag)
		{
			baseType = 0;
			return;
		}
		switch (num)
		{
		case 5:
			if (WorldGen.IsThisAMushroomTree(i, j))
			{
				baseOption = 1;
			}
			newColor = tileCache.color();
			return;
		case 51:
		case 697:
			if ((i + j) % 2 == 0)
			{
				baseType = 0;
			}
			break;
		case 19:
			if (tileCache.frameY == 864)
			{
				baseType = 0;
			}
			break;
		case 184:
			if (tileCache.frameX / 22 == 10)
			{
				num = 627;
				baseType = tileLookup[num];
			}
			break;
		}
		if (baseType != 0)
		{
			GetTileBaseOption(i, j, num, tileCache, ref baseOption);
			if (num == 160)
			{
				newColor = 0;
			}
			else
			{
				newColor = tileCache.color();
			}
		}
	}

	private static void GetWallType(int i, int j, Tile tileCache, ref int newColor, ref int newLight, ref int baseType, ref int baseOption)
	{
		bool flag = tileCache.invisibleWall();
		if (tileCache.wall > 0 && tileCache.fullbrightWall() && !flag)
		{
			newLight = 255;
		}
		if (tileCache.liquid > 32)
		{
			int num = tileCache.liquidType();
			baseType = liquidPosition + num;
		}
		else if (!tileCache.invisibleWall() && tileCache.wall > 0 && tileCache.wall < WallID.Count)
		{
			int wall = tileCache.wall;
			baseType = wallLookup[wall];
			newColor = tileCache.wallColor();
			switch (wall)
			{
			case 21:
			case 88:
			case 89:
			case 90:
			case 91:
			case 92:
			case 93:
			case 168:
			case 241:
				newColor = 0;
				break;
			case 27:
				baseOption = i % 2;
				break;
			default:
				baseOption = 0;
				break;
			}
		}
	}

	private static int GetBackgroundType(int i, int j, ref int newLight)
	{
		if ((double)j < Main.worldSurface)
		{
			if (Main.remixWorld)
			{
				newLight = 5;
				return 100;
			}
			newLight = 255;
			return CalcSkyGradient(skyPosition, 256, j);
		}
		if (j < Main.UnderworldLayer)
		{
			byte b = 0;
			if (!WorldGen.generatingWorld)
			{
				if (!sceneArea.Contains(i, j))
				{
					for (int k = i - 36; k <= i + 30; k += 10)
					{
						for (int l = j - 36; l <= j + 30; l += 10)
						{
							int type = Main.Map[k, l].Type;
							for (int m = 0; m < snowTypes.Length; m++)
							{
								if (snowTypes[m] == type)
								{
									b = byte.MaxValue;
									k = i + 31;
									l = j + 31;
									break;
								}
							}
						}
					}
				}
				else
				{
					b = (byte)(Utils.Clamp(sceneSnowiness, 0f, 1f) * 255f);
				}
			}
			if ((double)j < Main.rockLayer)
			{
				return dirtPosition + b;
			}
			return rockPosition + b;
		}
		return hellPosition;
	}

	public static int CalcSkyGradient(ushort skyPosition, int maxSkyGradients, int j)
	{
		int num = (byte)((double)(maxSkyGradients - 1) * ((double)j / Main.worldSurface));
		return skyPosition + num;
	}

	public static void GetTileBaseOption(int x, int y, int tileType, Tile tileCache, ref int baseOption)
	{
		switch (tileType)
		{
		case 89:
			switch (tileCache.frameX / 54)
			{
			case 0:
			case 21:
			case 23:
				baseOption = 0;
				break;
			case 43:
				baseOption = 2;
				break;
			default:
				baseOption = 1;
				break;
			}
			break;
		case 160:
		case 627:
		case 628:
		case 692:
			baseOption = (x + y) % 9;
			break;
		case 461:
			if (Main.SceneMetrics.ZoneCorrupt)
			{
				baseOption = 1;
			}
			else if (Main.SceneMetrics.ZoneCrimson)
			{
				baseOption = 2;
			}
			else if (Main.SceneMetrics.ZoneHallow)
			{
				baseOption = 3;
			}
			break;
		case 80:
		{
			WorldGen.GetCactusType(x, y, tileCache.frameX, tileCache.frameY, out var evil, out var good, out var crimson);
			if (evil)
			{
				baseOption = 1;
			}
			else if (good)
			{
				baseOption = 2;
			}
			else if (crimson)
			{
				baseOption = 3;
			}
			else
			{
				baseOption = 0;
			}
			break;
		}
		case 529:
		{
			int num9 = y + 1;
			WorldGen.GetBiomeInfluence(x, x, num9, num9, out var corruptCount2, out var crimsonCount2, out var hallowedCount2);
			int num10 = corruptCount2;
			if (num10 < crimsonCount2)
			{
				num10 = crimsonCount2;
			}
			if (num10 < hallowedCount2)
			{
				num10 = hallowedCount2;
			}
			int num11 = 0;
			if (corruptCount2 == 0 && crimsonCount2 == 0 && hallowedCount2 == 0)
			{
				num11 = ((x < WorldGen.beachDistance || x > Main.maxTilesX - WorldGen.beachDistance) ? 1 : 0);
			}
			else if (hallowedCount2 == num10)
			{
				num11 = 2;
			}
			else
			{
				num11 = ((crimsonCount2 != num10) ? 4 : 3);
			}
			baseOption = num11;
			break;
		}
		case 530:
		{
			int num2 = y - tileCache.frameY % 36 / 18 + 2;
			int num3 = x - tileCache.frameX % 54 / 18;
			WorldGen.GetBiomeInfluence(num3, num3 + 3, num2, num2, out var corruptCount, out var crimsonCount, out var hallowedCount);
			int num4 = corruptCount;
			if (num4 < crimsonCount)
			{
				num4 = crimsonCount;
			}
			if (num4 < hallowedCount)
			{
				num4 = hallowedCount;
			}
			int num5 = 0;
			num5 = ((corruptCount != 0 || crimsonCount != 0 || hallowedCount != 0) ? ((hallowedCount == num4) ? 1 : ((crimsonCount != num4) ? 3 : 2)) : 0);
			baseOption = num5;
			break;
		}
		case 19:
		{
			int num13 = tileCache.frameY / 18;
			baseOption = 0;
			if (num13 == 48)
			{
				baseOption = 1;
			}
			break;
		}
		case 15:
		{
			int num8 = tileCache.frameY / 40;
			baseOption = 0;
			if (num8 == 1 || num8 == 20)
			{
				baseOption = 1;
			}
			break;
		}
		case 518:
		case 519:
			baseOption = tileCache.frameY / 18;
			break;
		case 4:
			if (tileCache.frameX < 66)
			{
				baseOption = 1;
			}
			baseOption = 0;
			break;
		case 572:
			baseOption = tileCache.frameY / 36;
			break;
		case 21:
		case 441:
			switch (tileCache.frameX / 36)
			{
			case 1:
			case 2:
			case 10:
			case 13:
			case 15:
				baseOption = 1;
				break;
			case 3:
			case 4:
				baseOption = 2;
				break;
			case 6:
				baseOption = 3;
				break;
			case 11:
			case 17:
				baseOption = 4;
				break;
			default:
				baseOption = 0;
				break;
			}
			break;
		case 467:
		case 468:
		{
			int num = tileCache.frameX / 36;
			switch (num)
			{
			case 0:
			case 1:
			case 2:
			case 3:
			case 4:
			case 5:
			case 6:
			case 7:
			case 8:
			case 9:
			case 10:
			case 11:
				baseOption = num;
				break;
			case 12:
			case 13:
			case 15:
			case 18:
				baseOption = 10;
				break;
			case 14:
			case 17:
				baseOption = 7;
				break;
			case 16:
				baseOption = 3;
				break;
			default:
				baseOption = 12;
				break;
			}
			break;
		}
		case 560:
		{
			int num = tileCache.frameX / 36;
			if ((uint)num <= 2u)
			{
				baseOption = num;
			}
			else
			{
				baseOption = 0;
			}
			break;
		}
		case 28:
		case 653:
			if (tileCache.frameY < 144)
			{
				baseOption = 0;
			}
			else if (tileCache.frameY < 252)
			{
				baseOption = 1;
			}
			else if (tileCache.frameY < 360 || (tileCache.frameY >= 900 && tileCache.frameY < 1008))
			{
				baseOption = 2;
			}
			else if (tileCache.frameY < 468)
			{
				baseOption = 3;
			}
			else if (tileCache.frameY < 576)
			{
				baseOption = 4;
			}
			else if (tileCache.frameY < 684)
			{
				baseOption = 5;
			}
			else if (tileCache.frameY < 792)
			{
				baseOption = 6;
			}
			else if (tileCache.frameY < 900)
			{
				baseOption = 8;
			}
			else if (tileCache.frameY < 1008)
			{
				baseOption = 7;
			}
			else if (tileCache.frameY < 1116)
			{
				baseOption = 0;
			}
			else if (tileCache.frameY < 1224)
			{
				baseOption = 3;
			}
			else
			{
				baseOption = 7;
			}
			break;
		case 27:
			if (tileCache.frameY < 34)
			{
				baseOption = 1;
			}
			else
			{
				baseOption = 0;
			}
			break;
		case 31:
		case 696:
			if (tileCache.frameX >= 36)
			{
				baseOption = 1;
			}
			else
			{
				baseOption = 0;
			}
			break;
		case 26:
		case 695:
			if (tileCache.frameX >= 54)
			{
				baseOption = 1;
			}
			else
			{
				baseOption = 0;
			}
			break;
		case 137:
			switch (tileCache.frameY / 18)
			{
			default:
				baseOption = 0;
				break;
			case 1:
			case 2:
			case 3:
			case 4:
				baseOption = 1;
				break;
			case 5:
				baseOption = 2;
				break;
			}
			break;
		case 82:
		case 83:
		case 84:
			if (tileCache.frameX < 18)
			{
				baseOption = 0;
			}
			else if (tileCache.frameX < 36)
			{
				baseOption = 1;
			}
			else if (tileCache.frameX < 54)
			{
				baseOption = 2;
			}
			else if (tileCache.frameX < 72)
			{
				baseOption = 3;
			}
			else if (tileCache.frameX < 90)
			{
				baseOption = 4;
			}
			else if (tileCache.frameX < 108)
			{
				baseOption = 5;
			}
			else
			{
				baseOption = 6;
			}
			break;
		case 591:
			baseOption = tileCache.frameX / 36;
			break;
		case 105:
			if (tileCache.frameX >= 1548 && tileCache.frameX <= 1654)
			{
				baseOption = 1;
			}
			else if (tileCache.frameX >= 1656 && tileCache.frameX <= 1798)
			{
				baseOption = 2;
			}
			else
			{
				baseOption = 0;
			}
			break;
		case 133:
			if (tileCache.frameX < 52)
			{
				baseOption = 0;
			}
			else
			{
				baseOption = 1;
			}
			break;
		case 134:
			if (tileCache.frameX < 28)
			{
				baseOption = 0;
			}
			else
			{
				baseOption = 1;
			}
			break;
		case 149:
			baseOption = y % 3;
			break;
		case 165:
		case 693:
		case 694:
			if (tileCache.frameX < 54)
			{
				baseOption = 0;
			}
			else if (tileCache.frameX < 106)
			{
				baseOption = 1;
			}
			else if (tileCache.frameX >= 216)
			{
				baseOption = 1;
			}
			else if (tileCache.frameX < 162)
			{
				baseOption = 2;
			}
			else
			{
				baseOption = 3;
			}
			break;
		case 178:
			if (tileCache.frameX < 18)
			{
				baseOption = 0;
			}
			else if (tileCache.frameX < 36)
			{
				baseOption = 1;
			}
			else if (tileCache.frameX < 54)
			{
				baseOption = 2;
			}
			else if (tileCache.frameX < 72)
			{
				baseOption = 3;
			}
			else if (tileCache.frameX < 90)
			{
				baseOption = 4;
			}
			else if (tileCache.frameX < 108)
			{
				baseOption = 5;
			}
			else
			{
				baseOption = 6;
			}
			break;
		case 184:
			if (tileCache.frameX < 22)
			{
				baseOption = 0;
			}
			else if (tileCache.frameX < 44)
			{
				baseOption = 1;
			}
			else if (tileCache.frameX < 66)
			{
				baseOption = 2;
			}
			else if (tileCache.frameX < 88)
			{
				baseOption = 3;
			}
			else if (tileCache.frameX < 110)
			{
				baseOption = 4;
			}
			else if (tileCache.frameX < 132)
			{
				baseOption = 5;
			}
			else if (tileCache.frameX < 154)
			{
				baseOption = 6;
			}
			else if (tileCache.frameX < 176)
			{
				baseOption = 7;
			}
			else if (tileCache.frameX < 198)
			{
				baseOption = 8;
			}
			else if (tileCache.frameX < 220)
			{
				baseOption = 9;
			}
			else if (tileCache.frameX < 242)
			{
				baseOption = 10;
			}
			break;
		case 650:
		{
			int num = tileCache.frameX / 18;
			if (num < 6 || num == 28 || num == 29 || num == 30 || num == 31 || num == 32)
			{
				baseOption = 0;
			}
			else if (num < 12 || num == 33 || num == 34 || num == 35)
			{
				baseOption = 1;
			}
			else if (num < 28)
			{
				baseOption = 2;
			}
			else if (num < 48)
			{
				baseOption = 3;
			}
			else if (num < 54)
			{
				baseOption = 4;
			}
			else if (num < 72)
			{
				baseOption = 0;
			}
			else if (num == 72)
			{
				baseOption = 1;
			}
			else if (num < 78)
			{
				baseOption = 11;
			}
			break;
		}
		case 649:
		{
			int num = tileCache.frameX / 36;
			int num6 = tileCache.frameY / 18 - 1;
			num += num6 * 18;
			if (num < 6 || num == 19 || num == 20 || num == 21 || num == 22 || num == 23 || num == 24 || num == 33 || num == 38 || num == 39 || num == 40)
			{
				baseOption = 0;
			}
			else if (num < 16)
			{
				baseOption = 2;
			}
			else if (num < 19 || num == 31 || num == 32)
			{
				baseOption = 1;
			}
			else if (num < 31)
			{
				baseOption = 3;
			}
			else if (num < 38)
			{
				baseOption = 4;
			}
			else if (num < 59)
			{
				baseOption = 0;
			}
			else if (num < 62)
			{
				baseOption = 1;
			}
			else if (num < 65)
			{
				baseOption = 11;
			}
			break;
		}
		case 185:
		{
			int num;
			if (tileCache.frameY < 18)
			{
				num = tileCache.frameX / 18;
				if (num < 6 || num == 28 || num == 29 || num == 30 || num == 31 || num == 32)
				{
					baseOption = 0;
				}
				else if (num < 12 || num == 33 || num == 34 || num == 35)
				{
					baseOption = 1;
				}
				else if (num < 28)
				{
					baseOption = 2;
				}
				else if (num < 48)
				{
					baseOption = 3;
				}
				else if (num < 54)
				{
					baseOption = 4;
				}
				else if (num < 72)
				{
					baseOption = 0;
				}
				else if (num == 72)
				{
					baseOption = 1;
				}
				else if (num < 78)
				{
					baseOption = 11;
				}
				break;
			}
			num = tileCache.frameX / 36;
			int num12 = tileCache.frameY / 18 - 1;
			num += num12 * 18;
			if (num < 6 || num == 19 || num == 20 || num == 21 || num == 22 || num == 23 || num == 24 || num == 33 || num == 38 || num == 39 || num == 40)
			{
				baseOption = 0;
			}
			else if (num < 16)
			{
				baseOption = 2;
			}
			else if (num < 19 || num == 31 || num == 32)
			{
				baseOption = 1;
			}
			else if (num < 31)
			{
				baseOption = 3;
			}
			else if (num < 38)
			{
				baseOption = 4;
			}
			else if (num < 59)
			{
				baseOption = 0;
			}
			else if (num < 62)
			{
				baseOption = 1;
			}
			else if (num < 65)
			{
				baseOption = 11;
			}
			break;
		}
		case 186:
		case 647:
		{
			int num = tileCache.frameX / 54;
			if (num < 7)
			{
				baseOption = 2;
			}
			else if (num < 22 || num == 33 || num == 34 || num == 35)
			{
				baseOption = 0;
			}
			else if (num < 25)
			{
				baseOption = 1;
			}
			else if (num == 25)
			{
				baseOption = 5;
			}
			else if (num < 32)
			{
				baseOption = 3;
			}
			break;
		}
		case 187:
		case 648:
		{
			int num = tileCache.frameX / 54;
			int num7 = tileCache.frameY / 36;
			num += num7 * 36;
			if (num < 3 || num == 14 || num == 15 || num == 16)
			{
				baseOption = 0;
			}
			else if (num < 6)
			{
				baseOption = 6;
			}
			else if (num < 9)
			{
				baseOption = 7;
			}
			else if (num < 14)
			{
				baseOption = 4;
			}
			else if (num < 18)
			{
				baseOption = 4;
			}
			else if (num < 23)
			{
				baseOption = 8;
			}
			else if (num < 25)
			{
				baseOption = 0;
			}
			else if (num < 29)
			{
				baseOption = 1;
			}
			else if (num < 47)
			{
				baseOption = 0;
			}
			else if (num < 50)
			{
				baseOption = 1;
			}
			else if (num < 52)
			{
				baseOption = 10;
			}
			else if (num < 55)
			{
				baseOption = 2;
			}
			break;
		}
		case 227:
			baseOption = tileCache.frameX / 34;
			break;
		case 129:
			if (tileCache.frameX >= 324)
			{
				baseOption = 1;
			}
			else
			{
				baseOption = 0;
			}
			break;
		case 240:
		{
			int num = tileCache.frameX / 54;
			int num14 = tileCache.frameY / 54;
			num += num14 * 36;
			if ((num >= 0 && num <= 11) || (num >= 47 && num <= 53) || num == 72 || num == 73 || num == 75)
			{
				baseOption = 0;
				break;
			}
			if ((num < 12 || num > 15) && (num < 18 || num > 35) && (num < 63 || num > 71))
			{
				switch (num)
				{
				case 74:
				case 76:
				case 77:
				case 78:
				case 79:
				case 80:
				case 81:
				case 82:
				case 83:
				case 84:
				case 85:
				case 86:
				case 87:
				case 88:
				case 89:
				case 90:
				case 91:
				case 92:
				case 93:
				case 94:
				case 95:
				case 96:
				case 97:
				case 98:
				case 99:
				case 100:
					break;
				default:
					switch (num)
					{
					case 16:
					case 17:
						baseOption = 2;
						return;
					case 41:
					case 42:
					case 43:
					case 44:
					case 45:
						baseOption = 3;
						return;
					}
					if (num == 46)
					{
						baseOption = 4;
					}
					return;
				}
			}
			baseOption = 1;
			break;
		}
		case 242:
		{
			int num = tileCache.frameY / 72;
			if (tileCache.frameX / 106 == 0 && num >= 22 && num <= 24)
			{
				baseOption = 1;
			}
			else
			{
				baseOption = 0;
			}
			break;
		}
		case 440:
		{
			int num = tileCache.frameX / 54;
			if (num > 6)
			{
				num = 6;
			}
			baseOption = num;
			break;
		}
		case 457:
		{
			int num = tileCache.frameX / 36;
			if (num > 4)
			{
				num = 4;
			}
			baseOption = num;
			break;
		}
		case 453:
		{
			int num = tileCache.frameX / 36;
			if (num > 2)
			{
				num = 2;
			}
			baseOption = num;
			break;
		}
		case 419:
		{
			int num = tileCache.frameX / 18;
			if (num > 2)
			{
				num = 2;
			}
			baseOption = num;
			break;
		}
		case 428:
		{
			int num = tileCache.frameY / 18;
			if (num > 3)
			{
				num = 3;
			}
			baseOption = num;
			break;
		}
		case 420:
		{
			int num = tileCache.frameY / 18;
			if (num > 5)
			{
				num = 5;
			}
			baseOption = num;
			break;
		}
		case 423:
		{
			int num = tileCache.frameY / 18;
			if (num > 6)
			{
				num = 6;
			}
			baseOption = num;
			break;
		}
		case 493:
			if (tileCache.frameX < 18)
			{
				baseOption = 0;
			}
			else if (tileCache.frameX < 36)
			{
				baseOption = 1;
			}
			else if (tileCache.frameX < 54)
			{
				baseOption = 2;
			}
			else if (tileCache.frameX < 72)
			{
				baseOption = 3;
			}
			else if (tileCache.frameX < 90)
			{
				baseOption = 4;
			}
			else
			{
				baseOption = 5;
			}
			break;
		case 548:
			if (tileCache.frameX / 54 < 7)
			{
				baseOption = 0;
			}
			else
			{
				baseOption = 1;
			}
			break;
		case 597:
		{
			int num = tileCache.frameX / 54;
			if ((uint)num <= 10u)
			{
				baseOption = num;
			}
			else
			{
				baseOption = 0;
			}
			break;
		}
		default:
			baseOption = 0;
			break;
		}
	}

	public static void SaveMap()
	{
		if ((Main.ActivePlayerFileData.IsCloudSave && SocialAPI.Cloud == null) || !Main.mapEnabled || !Monitor.TryEnter(IOLock))
		{
			return;
		}
		try
		{
			FileUtilities.ProtectedInvoke(InternalSaveMap);
		}
		catch (Exception value)
		{
			using StreamWriter streamWriter = new StreamWriter("client-crashlog.txt", append: true);
			streamWriter.WriteLine(DateTime.Now);
			streamWriter.WriteLine(value);
			streamWriter.WriteLine("");
		}
		finally
		{
			Monitor.Exit(IOLock);
		}
	}

	private static void InternalSaveMap()
	{
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Expected Obj, but got Unknown
		bool isCloudSave = Main.ActivePlayerFileData.IsCloudSave;
		string text = Main.playerPathName.Substring(0, Main.playerPathName.Length - 4);
		if (!isCloudSave)
		{
			Utils.TryCreatingDirectory(text);
		}
		text = text + Path.DirectorySeparatorChar + Main.ActiveWorldFileData.MapFileName + ".map";
		new Stopwatch().Start();
		if (!Main.gameMenu)
		{
			noStatusText = true;
		}
		using (MemoryStream memoryStream = new MemoryStream(4000))
		{
			using BinaryWriter binaryWriter = new BinaryWriter(memoryStream);
			DeflateStream val = new DeflateStream((Stream)memoryStream, (CompressionMode)0);
			try
			{
				int num = 0;
				byte[] array = new byte[16384];
				binaryWriter.Write(326);
				Main.MapFileMetadata.IncrementAndWrite(binaryWriter);
				binaryWriter.Write(Main.worldName);
				binaryWriter.Write(Main.worldID);
				binaryWriter.Write(Main.maxTilesY);
				binaryWriter.Write(Main.maxTilesX);
				binaryWriter.Write((short)TileID.Count);
				binaryWriter.Write((short)WallID.Count);
				binaryWriter.Write((short)4);
				binaryWriter.Write((short)256);
				binaryWriter.Write((short)256);
				binaryWriter.Write((short)256);
				byte b = 1;
				byte b2 = 0;
				int i;
				for (i = 0; i < TileID.Count; i++)
				{
					if (tileOptionCounts[i] != 1)
					{
						b2 |= b;
					}
					if (b == 128)
					{
						binaryWriter.Write(b2);
						b2 = 0;
						b = 1;
					}
					else
					{
						b <<= 1;
					}
				}
				if (b != 1)
				{
					binaryWriter.Write(b2);
				}
				i = 0;
				b = 1;
				b2 = 0;
				for (; i < WallID.Count; i++)
				{
					if (wallOptionCounts[i] != 1)
					{
						b2 |= b;
					}
					if (b == 128)
					{
						binaryWriter.Write(b2);
						b2 = 0;
						b = 1;
					}
					else
					{
						b <<= 1;
					}
				}
				if (b != 1)
				{
					binaryWriter.Write(b2);
				}
				for (i = 0; i < TileID.Count; i++)
				{
					if (tileOptionCounts[i] != 1)
					{
						binaryWriter.Write((byte)tileOptionCounts[i]);
					}
				}
				for (i = 0; i < WallID.Count; i++)
				{
					if (wallOptionCounts[i] != 1)
					{
						binaryWriter.Write((byte)wallOptionCounts[i]);
					}
				}
				binaryWriter.Flush();
				for (int j = 0; j < Main.maxTilesY; j++)
				{
					if (!noStatusText)
					{
						float num2 = (float)j / (float)Main.maxTilesY;
						Main.statusText = Lang.gen[66].Value + " " + (int)(num2 * 100f + 1f) + "%";
					}
					int num3;
					for (num3 = 0; num3 < Main.maxTilesX; num3++)
					{
						MapTile mapTile = Main.Map[num3, j];
						byte b4;
						byte b5;
						byte b3 = (b4 = (b5 = 0));
						int num4 = 0;
						bool flag = true;
						bool flag2 = true;
						int num5 = 0;
						int num6 = 0;
						byte b6 = 0;
						int num7;
						ushort num8;
						if (mapTile.Light <= 18)
						{
							flag2 = false;
							flag = false;
							num7 = 0;
							num8 = 0;
							num4 = 0;
							int num9 = num3 + 1;
							int num10 = Main.maxTilesX - num3 - 1;
							while (num10 > 0 && Main.Map[num9, j].Light <= 18)
							{
								num4++;
								num10--;
								num9++;
							}
						}
						else
						{
							b6 = mapTile.Color;
							num8 = mapTile.Type;
							if (num8 < wallPosition)
							{
								num7 = 1;
								num8 -= tilePosition;
							}
							else if (num8 < liquidPosition)
							{
								num7 = 2;
								num8 -= wallPosition;
							}
							else if (num8 < skyPosition)
							{
								int num11 = num8 - liquidPosition;
								if (num11 == 3)
								{
									b4 |= 0x40;
									num11 = 0;
								}
								num7 = 3 + num11;
								flag = false;
							}
							else if (num8 < dirtPosition)
							{
								num7 = 6;
								flag2 = false;
								flag = false;
							}
							else if (num8 < hellPosition)
							{
								num7 = 7;
								num8 = ((num8 >= rockPosition) ? ((ushort)(num8 - rockPosition)) : ((ushort)(num8 - dirtPosition)));
							}
							else
							{
								num7 = 6;
								flag = false;
							}
							if (mapTile.Light == byte.MaxValue)
							{
								flag2 = false;
							}
							if (flag2)
							{
								num4 = 0;
								int num9 = num3 + 1;
								int num10 = Main.maxTilesX - num3 - 1;
								num5 = num9;
								while (num10 > 0)
								{
									if (mapTile.EqualsWithoutLight(Main.Map[num9, j]))
									{
										num10--;
										num4++;
										num9++;
										continue;
									}
									num6 = num9;
									break;
								}
							}
							else
							{
								num4 = 0;
								int num9 = num3 + 1;
								int num10 = Main.maxTilesX - num3 - 1;
								while (num10 > 0 && mapTile.Equals(Main.Map[num9, j]))
								{
									num10--;
									num4++;
									num9++;
								}
							}
						}
						if (b6 > 0)
						{
							b4 |= (byte)(b6 << 1);
						}
						if (b5 != 0)
						{
							b4 |= 1;
						}
						if (b4 != 0)
						{
							b3 |= 1;
						}
						b3 |= (byte)(num7 << 1);
						if (flag && num8 > 255)
						{
							b3 |= 0x10;
						}
						if (flag2)
						{
							b3 |= 0x20;
						}
						if (num4 > 0)
						{
							b3 = ((num4 <= 255) ? ((byte)(b3 | 0x40)) : ((byte)(b3 | 0x80)));
						}
						array[num] = b3;
						num++;
						if (b4 != 0)
						{
							array[num] = b4;
							num++;
						}
						if (b5 != 0)
						{
							array[num] = b5;
							num++;
						}
						if (flag)
						{
							array[num] = (byte)num8;
							num++;
							if (num8 > 255)
							{
								array[num] = (byte)(num8 >> 8);
								num++;
							}
						}
						if (flag2)
						{
							array[num] = mapTile.Light;
							num++;
						}
						if (num4 > 0)
						{
							array[num] = (byte)num4;
							num++;
							if (num4 > 255)
							{
								array[num] = (byte)(num4 >> 8);
								num++;
							}
						}
						for (int k = num5; k < num6; k++)
						{
							array[num] = Main.Map[k, j].Light;
							num++;
						}
						num3 += num4;
						if (num >= 4096)
						{
							((Stream)(object)val).Write(array, 0, num);
							num = 0;
						}
					}
				}
				if (num > 0)
				{
					((Stream)(object)val).Write(array, 0, num);
				}
				((Stream)(object)val).Dispose();
				FileUtilities.WriteAllBytes(text, memoryStream.ToArray(), isCloudSave);
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
		noStatusText = false;
	}

	public static void DecompressChunk(byte[] srcData, long srcLength, byte[] dstData, long dstLength, out long resultLength)
	{
		resultLength = 0L;
		ZlibCodec val = zlibDecompress;
		val.InitializeInflate();
		val.InputBuffer = srcData;
		val.OutputBuffer = dstData;
		val.NextIn = 0;
		val.AvailableBytesIn = (int)srcLength;
		val.NextOut = 0;
		val.AvailableBytesOut = dstData.Length;
		int num = val.Inflate((FlushType)4);
		if ((num == 0 || 1 == num) && val.TotalBytesOut > 0)
		{
			resultLength = val.TotalBytesOut;
		}
		for (int i = (int)resultLength; i < dstLength; i++)
		{
			dstData[i] = 0;
		}
	}

	public unsafe static void LoadMapVersionCompressed(BinaryReader fileIO, int release)
	{
		if (release >= 135)
		{
			Main.MapFileMetadata = FileMetadata.Read(fileIO, FileType.Map);
		}
		else
		{
			Main.MapFileMetadata = FileMetadata.FromCurrentSettings(FileType.Map);
		}
		_ = DateTime.Now;
		fileIO.ReadString();
		int num = fileIO.ReadInt32();
		int num2 = fileIO.ReadInt32();
		int num3 = fileIO.ReadInt32();
		if (num != Main.worldID || num3 != Main.maxTilesX || num2 != Main.maxTilesY)
		{
			throw new Exception("Map meta-data is invalid.");
		}
		short num4 = fileIO.ReadInt16();
		short num5 = fileIO.ReadInt16();
		short num6 = fileIO.ReadInt16();
		short num7 = fileIO.ReadInt16();
		short num8 = fileIO.ReadInt16();
		short num9 = fileIO.ReadInt16();
		byte[] array = new byte[2048];
		byte[] array2 = new byte[sizeof(MapTile) * 64 * 64];
		bool[] array3 = new bool[num4];
		byte b = 0;
		byte b2 = 128;
		for (int i = 0; i < num4; i++)
		{
			if (b2 == 128)
			{
				b = fileIO.ReadByte();
				b2 = 1;
			}
			else
			{
				b2 <<= 1;
			}
			if ((b & b2) == b2)
			{
				array3[i] = true;
			}
		}
		bool[] array4 = new bool[num5];
		b = 0;
		b2 = 128;
		for (int i = 0; i < num5; i++)
		{
			if (b2 == 128)
			{
				b = fileIO.ReadByte();
				b2 = 1;
			}
			else
			{
				b2 <<= 1;
			}
			if ((b & b2) == b2)
			{
				array4[i] = true;
			}
		}
		byte[] array5 = new byte[num4];
		ushort num10 = 0;
		for (int i = 0; i < num4; i++)
		{
			if (array3[i])
			{
				array5[i] = fileIO.ReadByte();
			}
			else
			{
				array5[i] = 1;
			}
			num10 += array5[i];
		}
		byte[] array6 = new byte[num5];
		ushort num11 = 0;
		for (int i = 0; i < num5; i++)
		{
			if (array4[i])
			{
				array6[i] = fileIO.ReadByte();
			}
			else
			{
				array6[i] = 1;
			}
			num11 += array6[i];
		}
		ushort[] array7 = new ushort[num10 + num11 + num6 + num7 + num8 + num9 + 2];
		array7[0] = 0;
		ushort num12 = 1;
		ushort num13 = 1;
		for (int i = 0; i < TileID.Count; i++)
		{
			if (i < num4)
			{
				int num14 = array5[i];
				int num15 = tileOptionCounts[i];
				for (int j = 0; j < num15; j++)
				{
					if (j < num14)
					{
						array7[num13] = num12;
						num13++;
					}
					num12++;
				}
			}
			else
			{
				num12 += (ushort)tileOptionCounts[i];
			}
		}
		for (int i = 0; i < WallID.Count; i++)
		{
			if (i < num5)
			{
				int num16 = array6[i];
				int num17 = wallOptionCounts[i];
				for (int k = 0; k < num17; k++)
				{
					if (k < num16)
					{
						array7[num13] = num12;
						num13++;
					}
					num12++;
				}
			}
			else
			{
				num12 += (ushort)wallOptionCounts[i];
			}
		}
		for (int i = 0; i < 4; i++)
		{
			if (i < num6)
			{
				array7[num13] = num12;
				num13++;
			}
			num12++;
		}
		for (int i = 0; i < 256; i++)
		{
			if (i < num7)
			{
				array7[num13] = num12;
				num13++;
			}
			num12++;
		}
		for (int i = 0; i < 256; i++)
		{
			if (i < num8)
			{
				array7[num13] = num12;
				num13++;
			}
			num12++;
		}
		for (int i = 0; i < 256; i++)
		{
			if (i < num9)
			{
				array7[num13] = num12;
				num13++;
			}
			num12++;
		}
		array7[num13] = num12;
		bool flag = num13 != num12;
		int num18 = (Main.maxTilesX + 63) / 64;
		int num19 = (Main.maxTilesY + 63) / 64;
		int num20 = num18 * num19;
		int num21 = 4096;
		for (int l = 0; l < num20; l++)
		{
			float num22 = (float)l / (float)num20;
			Main.statusText = Lang.gen[66].Value + " " + (int)(num22 * 100f + 1f) + "%";
			int num23 = fileIO.ReadInt32();
			int num24 = l % num18;
			int num25 = l / num18;
			if (num23 > 0)
			{
				if (array.Length < num23)
				{
					array = new byte[(num23 + 1023) / 1024 * 1024];
				}
				for (int m = 0; m < num23; m++)
				{
					array[m] = fileIO.ReadByte();
				}
				DecompressChunk(array, num23, array2, array2.Length, out var _);
				fixed (byte* ptr = array2)
				{
					MapTile* ptr2 = (MapTile*)ptr;
					if (flag)
					{
						for (int n = 0; n < num21; n++)
						{
							int num26 = n % 64 + num24 * 64;
							int num27 = n / 64 + num25 * 64;
							ptr2[n].Type = array7[ptr2[n].Type];
							if (num26 < Main.maxTilesX && num27 < Main.maxTilesY)
							{
								Main.Map.SetTile(num26, num27, ref ptr2[n]);
							}
						}
						continue;
					}
					for (int num28 = 0; num28 < num21; num28++)
					{
						int num29 = num28 % 64 + num24 * 64;
						int num30 = num28 / 64 + num25 * 64;
						if (num29 < Main.maxTilesX && num30 < Main.maxTilesY)
						{
							Main.Map.SetTile(num29, num30, ref ptr2[num28]);
						}
					}
				}
				continue;
			}
			MapTile tile = default;
			for (int num31 = 0; num31 < num21; num31++)
			{
				int num32 = num31 % 64 + num24 * 64;
				int num33 = num31 / 64 + num25 * 64;
				if (num32 < Main.maxTilesX && num33 < Main.maxTilesY)
				{
					Main.Map.SetTile(num32, num33, ref tile);
				}
			}
		}
	}

	public static void LoadMapVersion1(BinaryReader fileIO, int release)
	{
		string text = fileIO.ReadString();
		int num = fileIO.ReadInt32();
		int num2 = fileIO.ReadInt32();
		int num3 = fileIO.ReadInt32();
		if (text != Main.worldName || num != Main.worldID || num3 != Main.maxTilesX || num2 != Main.maxTilesY)
		{
			throw new Exception("Map meta-data is invalid.");
		}
		OldMapHelper oldMapHelper = default;
		for (int i = 0; i < Main.maxTilesX; i++)
		{
			float num4 = (float)i / (float)Main.maxTilesX;
			Main.statusText = Lang.gen[67].Value + " " + (int)(num4 * 100f + 1f) + "%";
			for (int j = 0; j < Main.maxTilesY; j++)
			{
				if (fileIO.ReadBoolean())
				{
					int num5 = ((release <= 77) ? fileIO.ReadByte() : fileIO.ReadUInt16());
					byte b = fileIO.ReadByte();
					oldMapHelper.misc = fileIO.ReadByte();
					if (release >= 50)
					{
						oldMapHelper.misc2 = fileIO.ReadByte();
					}
					else
					{
						oldMapHelper.misc2 = 0;
					}
					bool flag = false;
					int num6 = oldMapHelper.option();
					int num7;
					if (oldMapHelper.active())
					{
						num7 = num6 + tileLookup[num5];
					}
					else if (oldMapHelper.water())
					{
						num7 = liquidPosition;
					}
					else if (oldMapHelper.lava())
					{
						num7 = liquidPosition + 1;
					}
					else if (oldMapHelper.honey())
					{
						num7 = liquidPosition + 2;
					}
					else if (oldMapHelper.wall())
					{
						num7 = num6 + wallLookup[num5];
					}
					else if ((double)j < Main.worldSurface)
					{
						flag = true;
						num7 = CalcSkyGradient(skyPosition, 256, j);
					}
					else if ((double)j < Main.rockLayer)
					{
						flag = true;
						if (num5 > 255)
						{
							num5 = 255;
						}
						num7 = num5 + dirtPosition;
					}
					else if (j < Main.UnderworldLayer)
					{
						flag = true;
						if (num5 > 255)
						{
							num5 = 255;
						}
						num7 = num5 + rockPosition;
					}
					else
					{
						num7 = hellPosition;
					}
					MapTile tile = MapTile.Create((ushort)num7, b, 0);
					Main.Map.SetTile(i, j, ref tile);
					int num8 = fileIO.ReadInt16();
					if (b == byte.MaxValue)
					{
						while (num8 > 0)
						{
							num8--;
							j++;
							if (flag)
							{
								if ((double)j < Main.worldSurface)
								{
									flag = true;
									num7 = CalcSkyGradient(skyPosition, 256, j);
								}
								else if ((double)j < Main.rockLayer)
								{
									flag = true;
									num7 = num5 + dirtPosition;
								}
								else if (j < Main.UnderworldLayer)
								{
									flag = true;
									num7 = num5 + rockPosition;
								}
								else
								{
									flag = true;
									num7 = hellPosition;
								}
								tile.Type = (ushort)num7;
							}
							Main.Map.SetTile(i, j, ref tile);
						}
						continue;
					}
					while (num8 > 0)
					{
						j++;
						num8--;
						b = fileIO.ReadByte();
						if (b <= 18)
						{
							continue;
						}
						tile.Light = b;
						if (flag)
						{
							if ((double)j < Main.worldSurface)
							{
								flag = true;
								num7 = CalcSkyGradient(skyPosition, 256, j);
							}
							else if ((double)j < Main.rockLayer)
							{
								flag = true;
								num7 = num5 + dirtPosition;
							}
							else if (j < Main.UnderworldLayer)
							{
								flag = true;
								num7 = num5 + rockPosition;
							}
							else
							{
								flag = true;
								num7 = hellPosition;
							}
							tile.Type = (ushort)num7;
						}
						Main.Map.SetTile(i, j, ref tile);
					}
				}
				else
				{
					int num9 = fileIO.ReadInt16();
					j += num9;
				}
			}
		}
	}

	public static void LoadMapVersion2(BinaryReader fileIO, int release)
	{
		//IL_0388: Unknown result type (might be due to invalid IL or missing references)
		//IL_0392: Expected Obj, but got Unknown
		if (release >= 135)
		{
			Main.MapFileMetadata = FileMetadata.Read(fileIO, FileType.Map);
		}
		else
		{
			Main.MapFileMetadata = FileMetadata.FromCurrentSettings(FileType.Map);
		}
		fileIO.ReadString();
		int num = fileIO.ReadInt32();
		int num2 = fileIO.ReadInt32();
		int num3 = fileIO.ReadInt32();
		if (num != Main.worldID || num3 != Main.maxTilesX || num2 != Main.maxTilesY)
		{
			throw new Exception("Map meta-data is invalid.");
		}
		short num4 = fileIO.ReadInt16();
		short num5 = fileIO.ReadInt16();
		short num6 = fileIO.ReadInt16();
		short num7 = fileIO.ReadInt16();
		short num8 = fileIO.ReadInt16();
		short num9 = fileIO.ReadInt16();
		bool[] array = new bool[num4];
		byte b = 0;
		byte b2 = 128;
		for (int i = 0; i < num4; i++)
		{
			if (b2 == 128)
			{
				b = fileIO.ReadByte();
				b2 = 1;
			}
			else
			{
				b2 <<= 1;
			}
			if ((b & b2) == b2)
			{
				array[i] = true;
			}
		}
		bool[] array2 = new bool[num5];
		b = 0;
		b2 = 128;
		for (int i = 0; i < num5; i++)
		{
			if (b2 == 128)
			{
				b = fileIO.ReadByte();
				b2 = 1;
			}
			else
			{
				b2 <<= 1;
			}
			if ((b & b2) == b2)
			{
				array2[i] = true;
			}
		}
		byte[] array3 = new byte[num4];
		ushort num10 = 0;
		for (int i = 0; i < num4; i++)
		{
			if (array[i])
			{
				array3[i] = fileIO.ReadByte();
			}
			else
			{
				array3[i] = 1;
			}
			num10 += array3[i];
		}
		byte[] array4 = new byte[num5];
		ushort num11 = 0;
		for (int i = 0; i < num5; i++)
		{
			if (array2[i])
			{
				array4[i] = fileIO.ReadByte();
			}
			else
			{
				array4[i] = 1;
			}
			num11 += array4[i];
		}
		ushort[] array5 = new ushort[num10 + num11 + num6 + num7 + num8 + num9 + 2];
		array5[0] = 0;
		ushort num12 = 1;
		ushort num13 = 1;
		ushort num14 = num13;
		for (int i = 0; i < TileID.Count; i++)
		{
			if (i < num4)
			{
				int num15 = array3[i];
				int num16 = tileOptionCounts[i];
				for (int j = 0; j < num16; j++)
				{
					if (j < num15)
					{
						array5[num13] = num12;
						num13++;
					}
					num12++;
				}
			}
			else
			{
				num12 += (ushort)tileOptionCounts[i];
			}
		}
		ushort num17 = num13;
		for (int i = 0; i < WallID.Count; i++)
		{
			if (i < num5)
			{
				int num18 = array4[i];
				int num19 = wallOptionCounts[i];
				for (int k = 0; k < num19; k++)
				{
					if (k < num18)
					{
						array5[num13] = num12;
						num13++;
					}
					num12++;
				}
			}
			else
			{
				num12 += (ushort)wallOptionCounts[i];
			}
		}
		ushort num20 = num13;
		for (int i = 0; i < 4; i++)
		{
			if (i < num6)
			{
				array5[num13] = num12;
				num13++;
			}
			num12++;
		}
		ushort num21 = num13;
		for (int i = 0; i < 256; i++)
		{
			if (i < num7)
			{
				array5[num13] = num12;
				num13++;
			}
			num12++;
		}
		ushort num22 = num13;
		for (int i = 0; i < 256; i++)
		{
			if (i < num8)
			{
				array5[num13] = num12;
				num13++;
			}
			num12++;
		}
		ushort num23 = num13;
		for (int i = 0; i < 256; i++)
		{
			if (i < num9)
			{
				array5[num13] = num12;
				num13++;
			}
			num12++;
		}
		ushort num24 = num13;
		array5[num13] = num12;
		BinaryReader binaryReader = ((release < 93) ? new BinaryReader(fileIO.BaseStream) : new BinaryReader((Stream)new DeflateStream(fileIO.BaseStream, (CompressionMode)1)));
		for (int l = 0; l < Main.maxTilesY; l++)
		{
			float num25 = (float)l / (float)Main.maxTilesY;
			Main.statusText = Lang.gen[67].Value + " " + (int)(num25 * 100f + 1f) + "%";
			for (int m = 0; m < Main.maxTilesX; m++)
			{
				byte b3 = binaryReader.ReadByte();
				byte b4 = (byte)(((b3 & 1) == 1) ? binaryReader.ReadByte() : 0);
				if ((b4 & 1) == 1)
				{
					binaryReader.ReadByte();
				}
				byte b5 = (byte)((b3 & 0xE) >> 1);
				bool flag;
				switch (b5)
				{
				case 0:
					flag = false;
					break;
				case 1:
				case 2:
				case 7:
					flag = true;
					break;
				case 3:
				case 4:
				case 5:
					flag = false;
					break;
				case 6:
					flag = false;
					break;
				default:
					flag = false;
					break;
				}
				ushort num26 = (ushort)(flag ? (((b3 & 0x10) != 16) ? binaryReader.ReadByte() : binaryReader.ReadUInt16()) : 0);
				byte b6 = (((b3 & 0x20) != 32) ? byte.MaxValue : binaryReader.ReadByte());
				int num27 = (byte)((b3 & 0xC0) >> 6) switch
				{
					0 => 0, 
					1 => binaryReader.ReadByte(), 
					2 => binaryReader.ReadInt16(), 
					_ => 0, 
				};
				switch (b5)
				{
				case 0:
					m += num27;
					continue;
				case 1:
					num26 += num14;
					break;
				case 2:
					num26 += num17;
					break;
				case 3:
				case 4:
				case 5:
				{
					int num28 = b5 - 3;
					if ((b4 & 0x40) == 64)
					{
						num28 = 3;
					}
					num26 += (ushort)(num20 + num28);
					break;
				}
				case 6:
					num26 = ((!((double)l < Main.worldSurface)) ? num24 : ((ushort)(num26 + (ushort)CalcSkyGradient(num21, num7, l))));
					break;
				case 7:
					num26 = ((!((double)l < Main.rockLayer)) ? ((ushort)(num26 + num23)) : ((ushort)(num26 + num22)));
					break;
				}
				MapTile tile = MapTile.Create(array5[num26], b6, (byte)((b4 >> 1) & 0x1F));
				Main.Map.SetTile(m, l, ref tile);
				if (b6 == byte.MaxValue)
				{
					while (num27 > 0)
					{
						m++;
						Main.Map.SetTile(m, l, ref tile);
						num27--;
					}
					continue;
				}
				while (num27 > 0)
				{
					m++;
					tile = tile.WithLight(binaryReader.ReadByte());
					Main.Map.SetTile(m, l, ref tile);
					num27--;
				}
			}
		}
		binaryReader.Close();
	}

	static MapHelper()
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Expected Obj, but got Unknown
	}
}
