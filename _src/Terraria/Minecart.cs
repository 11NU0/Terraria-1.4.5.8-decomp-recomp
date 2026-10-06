using System;
using Microsoft.Xna.Framework;
using Terraria.GameContent;

namespace Terraria;

public static class Minecart
{
	private enum TrackState
	{
		NoTrack = -1,
		AboveTrack,
		OnTrack,
		BelowTrack,
		AboveFront,
		AboveBack,
		OnFront,
		OnBack
	}

	public struct Customization
	{
		public float MinecartTextureWidth;

		public Vector2 WheelOffset;

		public Vector2 MagnetOffset;

		public static Customization Default
		{
			get
			{
				//IL_0020: Unknown result type (might be due to invalid IL or missing references)
				//IL_0025: Unknown result type (might be due to invalid IL or missing references)
				//IL_0036: Unknown result type (might be due to invalid IL or missing references)
				//IL_003b: Unknown result type (might be due to invalid IL or missing references)
				return new Customization
				{
					MinecartTextureWidth = 50f,
					MagnetOffset = new Vector2(25f, 26f),
					WheelOffset = new Vector2(12f, 0f)
				};
			}
		}
	}

	private const int TotalFrames = 36;

	public const int LeftDownDecoration = 36;

	public const int RightDownDecoration = 37;

	public const int BouncyBumperDecoration = 38;

	public const int RegularBumperDecoration = 39;

	public const int Flag_OnTrack = 0;

	public const int Flag_BouncyBumper = 1;

	public const int Flag_UsedRamp = 2;

	public const int Flag_HitSwitch = 3;

	public const int Flag_BoostLeft = 4;

	public const int Flag_BoostRight = 5;

	private const int NoConnection = -1;

	private const int TopConnection = 0;

	private const int MiddleConnection = 1;

	private const int BottomConnection = 2;

	private const int BumperEnd = -1;

	private const int BouncyEnd = -2;

	private const int RampEnd = -3;

	private const int OpenEnd = -4;

	public const float BoosterSpeed = 4f;

	private const int Type_Normal = 0;

	private const int Type_Pressure = 1;

	private const int Type_Booster = 2;

	private static int[] _leftSideConnection;

	private static int[] _rightSideConnection;

	private static int[] _trackType;

	private static bool[] _boostLeft;

	private static Vector2[] _texturePosition;

	private static short _firstPressureFrame;

	private static short _firstLeftBoostFrame;

	private static short _firstRightBoostFrame;

	private static int[][] _trackSwitchOptions;

	private static int[][] _tileHeight;

	public static void Initialize()
	{
		//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0123: Unknown result type (might be due to invalid IL or missing references)
		//IL_0128: Unknown result type (might be due to invalid IL or missing references)
		//IL_016c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0171: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_021f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0224: Unknown result type (might be due to invalid IL or missing references)
		//IL_0289: Unknown result type (might be due to invalid IL or missing references)
		//IL_028e: Unknown result type (might be due to invalid IL or missing references)
		//IL_02df: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0335: Unknown result type (might be due to invalid IL or missing references)
		//IL_033a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0381: Unknown result type (might be due to invalid IL or missing references)
		//IL_0386: Unknown result type (might be due to invalid IL or missing references)
		//IL_03cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_042a: Unknown result type (might be due to invalid IL or missing references)
		//IL_042f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0487: Unknown result type (might be due to invalid IL or missing references)
		//IL_048c: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0555: Unknown result type (might be due to invalid IL or missing references)
		//IL_055a: Unknown result type (might be due to invalid IL or missing references)
		//IL_058e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0593: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_05cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0629: Unknown result type (might be due to invalid IL or missing references)
		//IL_062e: Unknown result type (might be due to invalid IL or missing references)
		//IL_068b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0690: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_06f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_074f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0754: Unknown result type (might be due to invalid IL or missing references)
		//IL_079b: Unknown result type (might be due to invalid IL or missing references)
		//IL_07a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_07d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_07d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0812: Unknown result type (might be due to invalid IL or missing references)
		//IL_0817: Unknown result type (might be due to invalid IL or missing references)
		//IL_0853: Unknown result type (might be due to invalid IL or missing references)
		//IL_0858: Unknown result type (might be due to invalid IL or missing references)
		//IL_089d: Unknown result type (might be due to invalid IL or missing references)
		//IL_08a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_08e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_08ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_0945: Unknown result type (might be due to invalid IL or missing references)
		//IL_094a: Unknown result type (might be due to invalid IL or missing references)
		//IL_09a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_09a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a0b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a10: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a73: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a78: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ab1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ab6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aef: Unknown result type (might be due to invalid IL or missing references)
		//IL_0af4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b4b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b50: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ba7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bac: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c03: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c08: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c5f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c64: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c7e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c83: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c99: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c9e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cb4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cb9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ccf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cd4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cec: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cf6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cfb: Unknown result type (might be due to invalid IL or missing references)
		if (!Main.dedServ && (float)TextureAssets.MinecartMount.Width() != Customization.Default.MinecartTextureWidth)
		{
			throw new Exception("Be sure to update Minecart.textureWidth to match the actual texture size of " + Customization.Default.MinecartTextureWidth + ".");
		}
		_rightSideConnection = new int[36];
		_leftSideConnection = new int[36];
		_trackType = new int[36];
		_boostLeft = new bool[36];
		_texturePosition = new Vector2[40];
		_tileHeight = new int[36][];
		for (int i = 0; i < 36; i++)
		{
			int[] array = new int[8];
			for (int j = 0; j < array.Length; j++)
			{
				array[j] = 5;
			}
			_tileHeight[i] = array;
		}
		int num = 0;
		_leftSideConnection[num] = -1;
		_rightSideConnection[num] = -1;
		_tileHeight[num][0] = -4;
		_tileHeight[num][7] = -4;
		_texturePosition[num] = new Vector2(0f, 0f);
		num++;
		_leftSideConnection[num] = 1;
		_rightSideConnection[num] = 1;
		_texturePosition[num] = new Vector2(1f, 0f);
		num++;
		_leftSideConnection[num] = -1;
		_rightSideConnection[num] = 1;
		for (int k = 0; k < 4; k++)
		{
			_tileHeight[num][k] = -1;
		}
		_texturePosition[num] = new Vector2(2f, 1f);
		num++;
		_leftSideConnection[num] = 1;
		_rightSideConnection[num] = -1;
		for (int l = 4; l < 8; l++)
		{
			_tileHeight[num][l] = -1;
		}
		_texturePosition[num] = new Vector2(3f, 1f);
		num++;
		_leftSideConnection[num] = 2;
		_rightSideConnection[num] = 1;
		_tileHeight[num][0] = 1;
		_tileHeight[num][1] = 2;
		_tileHeight[num][2] = 3;
		_tileHeight[num][3] = 3;
		_tileHeight[num][4] = 4;
		_tileHeight[num][5] = 4;
		_texturePosition[num] = new Vector2(0f, 2f);
		num++;
		_leftSideConnection[num] = 1;
		_rightSideConnection[num] = 2;
		_tileHeight[num][2] = 4;
		_tileHeight[num][3] = 4;
		_tileHeight[num][4] = 3;
		_tileHeight[num][5] = 3;
		_tileHeight[num][6] = 2;
		_tileHeight[num][7] = 1;
		_texturePosition[num] = new Vector2(1f, 2f);
		num++;
		_leftSideConnection[num] = 1;
		_rightSideConnection[num] = 0;
		_tileHeight[num][4] = 6;
		_tileHeight[num][5] = 6;
		_tileHeight[num][6] = 7;
		_tileHeight[num][7] = 8;
		_texturePosition[num] = new Vector2(0f, 1f);
		num++;
		_leftSideConnection[num] = 0;
		_rightSideConnection[num] = 1;
		_tileHeight[num][0] = 8;
		_tileHeight[num][1] = 7;
		_tileHeight[num][2] = 6;
		_tileHeight[num][3] = 6;
		_texturePosition[num] = new Vector2(1f, 1f);
		num++;
		_leftSideConnection[num] = 0;
		_rightSideConnection[num] = 2;
		for (int m = 0; m < 8; m++)
		{
			_tileHeight[num][m] = 8 - m;
		}
		_texturePosition[num] = new Vector2(0f, 3f);
		num++;
		_leftSideConnection[num] = 2;
		_rightSideConnection[num] = 0;
		for (int n = 0; n < 8; n++)
		{
			_tileHeight[num][n] = n + 1;
		}
		_texturePosition[num] = new Vector2(1f, 3f);
		num++;
		_leftSideConnection[num] = 2;
		_rightSideConnection[num] = -1;
		_tileHeight[num][0] = 1;
		_tileHeight[num][1] = 2;
		for (int num2 = 2; num2 < 8; num2++)
		{
			_tileHeight[num][num2] = -1;
		}
		_texturePosition[num] = new Vector2(4f, 1f);
		num++;
		_leftSideConnection[num] = -1;
		_rightSideConnection[num] = 2;
		_tileHeight[num][6] = 2;
		_tileHeight[num][7] = 1;
		for (int num3 = 0; num3 < 6; num3++)
		{
			_tileHeight[num][num3] = -1;
		}
		_texturePosition[num] = new Vector2(5f, 1f);
		num++;
		_leftSideConnection[num] = 0;
		_rightSideConnection[num] = -1;
		_tileHeight[num][0] = 8;
		_tileHeight[num][1] = 7;
		_tileHeight[num][2] = 6;
		for (int num4 = 3; num4 < 8; num4++)
		{
			_tileHeight[num][num4] = -1;
		}
		_texturePosition[num] = new Vector2(6f, 1f);
		num++;
		_leftSideConnection[num] = -1;
		_rightSideConnection[num] = 0;
		_tileHeight[num][5] = 6;
		_tileHeight[num][6] = 7;
		_tileHeight[num][7] = 8;
		for (int num5 = 0; num5 < 5; num5++)
		{
			_tileHeight[num][num5] = -1;
		}
		_texturePosition[num] = new Vector2(7f, 1f);
		num++;
		_leftSideConnection[num] = -1;
		_rightSideConnection[num] = 1;
		_tileHeight[num][0] = -4;
		_texturePosition[num] = new Vector2(2f, 0f);
		num++;
		_leftSideConnection[num] = 1;
		_rightSideConnection[num] = -1;
		_tileHeight[num][7] = -4;
		_texturePosition[num] = new Vector2(3f, 0f);
		num++;
		_leftSideConnection[num] = 2;
		_rightSideConnection[num] = -1;
		for (int num6 = 0; num6 < 6; num6++)
		{
			_tileHeight[num][num6] = num6 + 1;
		}
		_tileHeight[num][6] = -3;
		_tileHeight[num][7] = -3;
		_texturePosition[num] = new Vector2(4f, 0f);
		num++;
		_leftSideConnection[num] = -1;
		_rightSideConnection[num] = 2;
		_tileHeight[num][0] = -3;
		_tileHeight[num][1] = -3;
		for (int num7 = 2; num7 < 8; num7++)
		{
			_tileHeight[num][num7] = 8 - num7;
		}
		_texturePosition[num] = new Vector2(5f, 0f);
		num++;
		_leftSideConnection[num] = 0;
		_rightSideConnection[num] = -1;
		for (int num8 = 0; num8 < 6; num8++)
		{
			_tileHeight[num][num8] = 8 - num8;
		}
		_tileHeight[num][6] = -3;
		_tileHeight[num][7] = -3;
		_texturePosition[num] = new Vector2(6f, 0f);
		num++;
		_leftSideConnection[num] = -1;
		_rightSideConnection[num] = 0;
		_tileHeight[num][0] = -3;
		_tileHeight[num][1] = -3;
		for (int num9 = 2; num9 < 8; num9++)
		{
			_tileHeight[num][num9] = num9 + 1;
		}
		_texturePosition[num] = new Vector2(7f, 0f);
		num++;
		_leftSideConnection[num] = -1;
		_rightSideConnection[num] = -1;
		_tileHeight[num][0] = -4;
		_tileHeight[num][7] = -4;
		_trackType[num] = 1;
		_texturePosition[num] = new Vector2(0f, 4f);
		num++;
		_leftSideConnection[num] = 1;
		_rightSideConnection[num] = 1;
		_trackType[num] = 1;
		_texturePosition[num] = new Vector2(1f, 4f);
		num++;
		_leftSideConnection[num] = -1;
		_rightSideConnection[num] = 1;
		_tileHeight[num][0] = -4;
		_trackType[num] = 1;
		_texturePosition[num] = new Vector2(0f, 5f);
		num++;
		_leftSideConnection[num] = 1;
		_rightSideConnection[num] = -1;
		_tileHeight[num][7] = -4;
		_trackType[num] = 1;
		_texturePosition[num] = new Vector2(1f, 5f);
		num++;
		_leftSideConnection[num] = -1;
		_rightSideConnection[num] = 1;
		for (int num10 = 0; num10 < 6; num10++)
		{
			_tileHeight[num][num10] = -2;
		}
		_texturePosition[num] = new Vector2(2f, 2f);
		num++;
		_leftSideConnection[num] = 1;
		_rightSideConnection[num] = -1;
		for (int num11 = 2; num11 < 8; num11++)
		{
			_tileHeight[num][num11] = -2;
		}
		_texturePosition[num] = new Vector2(3f, 2f);
		num++;
		_leftSideConnection[num] = 2;
		_rightSideConnection[num] = -1;
		_tileHeight[num][0] = 1;
		_tileHeight[num][1] = 2;
		for (int num12 = 2; num12 < 8; num12++)
		{
			_tileHeight[num][num12] = -2;
		}
		_texturePosition[num] = new Vector2(4f, 2f);
		num++;
		_leftSideConnection[num] = -1;
		_rightSideConnection[num] = 2;
		_tileHeight[num][6] = 2;
		_tileHeight[num][7] = 1;
		for (int num13 = 0; num13 < 6; num13++)
		{
			_tileHeight[num][num13] = -2;
		}
		_texturePosition[num] = new Vector2(5f, 2f);
		num++;
		_leftSideConnection[num] = 0;
		_rightSideConnection[num] = -1;
		_tileHeight[num][0] = 8;
		_tileHeight[num][1] = 7;
		_tileHeight[num][2] = 6;
		for (int num14 = 3; num14 < 8; num14++)
		{
			_tileHeight[num][num14] = -2;
		}
		_texturePosition[num] = new Vector2(6f, 2f);
		num++;
		_leftSideConnection[num] = -1;
		_rightSideConnection[num] = 0;
		_tileHeight[num][5] = 6;
		_tileHeight[num][6] = 7;
		_tileHeight[num][7] = 8;
		for (int num15 = 0; num15 < 5; num15++)
		{
			_tileHeight[num][num15] = -2;
		}
		_texturePosition[num] = new Vector2(7f, 2f);
		num++;
		_leftSideConnection[num] = 1;
		_rightSideConnection[num] = 1;
		_trackType[num] = 2;
		_boostLeft[num] = false;
		_texturePosition[num] = new Vector2(2f, 3f);
		num++;
		_leftSideConnection[num] = 1;
		_rightSideConnection[num] = 1;
		_trackType[num] = 2;
		_boostLeft[num] = true;
		_texturePosition[num] = new Vector2(3f, 3f);
		num++;
		_leftSideConnection[num] = 0;
		_rightSideConnection[num] = 2;
		for (int num16 = 0; num16 < 8; num16++)
		{
			_tileHeight[num][num16] = 8 - num16;
		}
		_trackType[num] = 2;
		_boostLeft[num] = false;
		_texturePosition[num] = new Vector2(4f, 3f);
		num++;
		_leftSideConnection[num] = 2;
		_rightSideConnection[num] = 0;
		for (int num17 = 0; num17 < 8; num17++)
		{
			_tileHeight[num][num17] = num17 + 1;
		}
		_trackType[num] = 2;
		_boostLeft[num] = true;
		_texturePosition[num] = new Vector2(5f, 3f);
		num++;
		_leftSideConnection[num] = 0;
		_rightSideConnection[num] = 2;
		for (int num18 = 0; num18 < 8; num18++)
		{
			_tileHeight[num][num18] = 8 - num18;
		}
		_trackType[num] = 2;
		_boostLeft[num] = true;
		_texturePosition[num] = new Vector2(6f, 3f);
		num++;
		_leftSideConnection[num] = 2;
		_rightSideConnection[num] = 0;
		for (int num19 = 0; num19 < 8; num19++)
		{
			_tileHeight[num][num19] = num19 + 1;
		}
		_trackType[num] = 2;
		_boostLeft[num] = false;
		_texturePosition[num] = new Vector2(7f, 3f);
		num++;
		_texturePosition[36] = new Vector2(0f, 6f);
		_texturePosition[37] = new Vector2(1f, 6f);
		_texturePosition[39] = new Vector2(0f, 7f);
		_texturePosition[38] = new Vector2(1f, 7f);
		for (int num20 = 0; num20 < _texturePosition.Length; num20++)
		{
			_texturePosition[num20] *= 18f;
		}
		for (int num21 = 0; num21 < _tileHeight.Length; num21++)
		{
			int[] array2 = _tileHeight[num21];
			for (int num22 = 0; num22 < array2.Length; num22++)
			{
				if (array2[num22] >= 0)
				{
					array2[num22] = (8 - array2[num22]) * 2;
				}
			}
		}
		int[] array3 = new int[36];
		_trackSwitchOptions = new int[64][];
		for (int num23 = 0; num23 < 64; num23++)
		{
			int num24 = 0;
			for (int num25 = 1; num25 < 256; num25 <<= 1)
			{
				if ((num23 & num25) == num25)
				{
					num24++;
				}
			}
			int num26 = 0;
			for (int num27 = 0; num27 < 36; num27++)
			{
				array3[num27] = -1;
				int num28 = 0;
				switch (_leftSideConnection[num27])
				{
				case 0:
					num28 |= 1;
					break;
				case 1:
					num28 |= 2;
					break;
				case 2:
					num28 |= 4;
					break;
				}
				switch (_rightSideConnection[num27])
				{
				case 0:
					num28 |= 8;
					break;
				case 1:
					num28 |= 0x10;
					break;
				case 2:
					num28 |= 0x20;
					break;
				}
				if (num24 < 2)
				{
					if (num23 != num28)
					{
						continue;
					}
				}
				else if (num28 == 0 || (num23 & num28) != num28)
				{
					continue;
				}
				array3[num27] = num27;
				num26++;
			}
			if (num26 == 0)
			{
				continue;
			}
			int[] array4 = new int[num26];
			int num29 = 0;
			for (int num30 = 0; num30 < 36; num30++)
			{
				if (array3[num30] != -1)
				{
					array4[num29] = array3[num30];
					num29++;
				}
			}
			_trackSwitchOptions[num23] = array4;
		}
		_firstPressureFrame = -1;
		_firstLeftBoostFrame = -1;
		_firstRightBoostFrame = -1;
		for (int num31 = 0; num31 < _trackType.Length; num31++)
		{
			switch (_trackType[num31])
			{
			case 1:
				if (_firstPressureFrame == -1)
				{
					_firstPressureFrame = (short)num31;
				}
				break;
			case 2:
				if (_boostLeft[num31])
				{
					if (_firstLeftBoostFrame == -1)
					{
						_firstLeftBoostFrame = (short)num31;
					}
				}
				else if (_firstRightBoostFrame == -1)
				{
					_firstRightBoostFrame = (short)num31;
				}
				break;
			}
		}
	}

	public static bool IsPressurePlate(Tile tile)
	{
		if (tile == null)
		{
			return false;
		}
		if (tile.active() && tile.type == 314 && (tile.frameX == 20 || tile.frameX == 21))
		{
			return true;
		}
		return false;
	}

	public static BitsByte TrackCollision(Player Player, ref Vector2 Position, ref Vector2 Velocity, ref Vector2 lastBoost, int Width, int Height, bool followDown, bool followUp, int fallStart, bool trackOnly, Mount.MountDelegatesData delegatesData)
	{
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_012c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0284: Unknown result type (might be due to invalid IL or missing references)
		//IL_0286: Unknown result type (might be due to invalid IL or missing references)
		//IL_0288: Unknown result type (might be due to invalid IL or missing references)
		//IL_028d: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0254: Unknown result type (might be due to invalid IL or missing references)
		//IL_0256: Unknown result type (might be due to invalid IL or missing references)
		//IL_0258: Unknown result type (might be due to invalid IL or missing references)
		//IL_025d: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_02af: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0735: Unknown result type (might be due to invalid IL or missing references)
		//IL_0744: Unknown result type (might be due to invalid IL or missing references)
		//IL_0761: Unknown result type (might be due to invalid IL or missing references)
		//IL_0385: Unknown result type (might be due to invalid IL or missing references)
		//IL_038a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0377: Unknown result type (might be due to invalid IL or missing references)
		//IL_037c: Unknown result type (might be due to invalid IL or missing references)
		//IL_035e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0363: Unknown result type (might be due to invalid IL or missing references)
		//IL_0350: Unknown result type (might be due to invalid IL or missing references)
		//IL_0355: Unknown result type (might be due to invalid IL or missing references)
		//IL_030b: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0409: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0855: Unknown result type (might be due to invalid IL or missing references)
		//IL_085a: Unknown result type (might be due to invalid IL or missing references)
		//IL_077a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0397: Unknown result type (might be due to invalid IL or missing references)
		//IL_039c: Unknown result type (might be due to invalid IL or missing references)
		//IL_039e: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0791: Unknown result type (might be due to invalid IL or missing references)
		//IL_06f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_06f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0435: Unknown result type (might be due to invalid IL or missing references)
		//IL_07aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0445: Unknown result type (might be due to invalid IL or missing references)
		//IL_0871: Unknown result type (might be due to invalid IL or missing references)
		//IL_0878: Unknown result type (might be due to invalid IL or missing references)
		//IL_08a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_08a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_093e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0945: Unknown result type (might be due to invalid IL or missing references)
		//IL_07d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_07e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_08c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_08ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_08d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_08ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_0624: Unknown result type (might be due to invalid IL or missing references)
		//IL_063a: Unknown result type (might be due to invalid IL or missing references)
		//IL_091e: Unknown result type (might be due to invalid IL or missing references)
		//IL_090b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0912: Unknown result type (might be due to invalid IL or missing references)
		//IL_081d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0820: Unknown result type (might be due to invalid IL or missing references)
		//IL_0825: Unknown result type (might be due to invalid IL or missing references)
		//IL_080a: Unknown result type (might be due to invalid IL or missing references)
		//IL_080d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0812: Unknown result type (might be due to invalid IL or missing references)
		//IL_082e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0830: Unknown result type (might be due to invalid IL or missing references)
		//IL_0832: Unknown result type (might be due to invalid IL or missing references)
		//IL_0837: Unknown result type (might be due to invalid IL or missing references)
		if (followDown & followUp)
		{
			followDown = false;
			followUp = false;
		}
		Customization minecartSettings = Player.MinecartSettings;
		Vector2 val = new Vector2((float)(Width / 2) - minecartSettings.MinecartTextureWidth / 2f, (float)(Height / 2));
		Vector2 val2 = Position + new Vector2((float)(Width / 2) - minecartSettings.MinecartTextureWidth / 2f, (float)(Height / 2)) + minecartSettings.MagnetOffset;
		Vector2 val3 = Velocity;
		float num = val3.Length();
		val3.Normalize();
		Vector2 val4 = val2;
		Tile tile = null;
		bool flag = false;
		bool flag2 = true;
		int num2 = -1;
		int num3 = -1;
		int num4 = -1;
		TrackState trackState = TrackState.NoTrack;
		bool flag3 = false;
		bool flag4 = false;
		bool flag5 = false;
		bool flag6 = false;
		Vector2 val5 = Vector2.Zero;
		Vector2 val6 = Vector2.Zero;
		BitsByte result = default;
		while (true)
		{
			int num5 = (int)(val4.X / 16f);
			int num6 = (int)(val4.Y / 16f);
			int num7 = (int)val4.X % 16 / 2;
			if (flag2)
			{
				num4 = num7;
			}
			bool flag7 = num7 != num4;
			if ((trackState == TrackState.OnBack || trackState == TrackState.OnTrack || trackState == TrackState.OnFront) && num5 != num2)
			{
				int num8 = ((trackState != TrackState.OnBack) ? tile.FrontTrack() : tile.BackTrack());
				switch ((!(val3.X < 0f)) ? _rightSideConnection[num8] : _leftSideConnection[num8])
				{
				case 0:
					num6--;
					val4.Y -= 2f;
					break;
				case 2:
					num6++;
					val4.Y += 2f;
					break;
				}
			}
			TrackState trackState2 = TrackState.NoTrack;
			bool flag8 = false;
			if (num5 != num2 || num6 != num3)
			{
				if (flag2)
				{
					flag2 = false;
				}
				else
				{
					flag8 = true;
				}
				tile = Main.tile[num5, num6];
				if (tile == null)
				{
					tile = new Tile();
					Main.tile[num5, num6] = tile;
				}
				flag = ((tile.nactive() && tile.type == 314) ? true : false);
			}
			if (flag)
			{
				TrackState trackState3 = TrackState.NoTrack;
				int num9 = tile.FrontTrack();
				int num10 = tile.BackTrack();
				int num11 = _tileHeight[num9][num7];
				switch (num11)
				{
				case -4:
					if (trackState == TrackState.OnFront)
					{
						if (trackOnly)
						{
							val4 -= val6;
							num = 0f;
							trackState2 = TrackState.OnFront;
							flag6 = true;
						}
						else
						{
							trackState2 = TrackState.NoTrack;
							flag5 = true;
						}
					}
					break;
				case -1:
					if (trackState == TrackState.OnFront)
					{
						val4 -= val6;
						num = 0f;
						trackState2 = TrackState.OnFront;
						flag6 = true;
					}
					break;
				case -2:
					if (trackState != TrackState.OnFront)
					{
						break;
					}
					if (trackOnly)
					{
						val4 -= val6;
						num = 0f;
						trackState2 = TrackState.OnFront;
						flag6 = true;
						break;
					}
					if (val3.X < 0f)
					{
						float num14 = (float)(num5 * 16 + (num7 + 1) * 2) - val4.X;
						val4.X += num14;
						num += num14 / val3.X;
					}
					val3.X = 0f - val3.X;
					result[1] = true;
					trackState2 = TrackState.OnFront;
					break;
				case -3:
					if (trackState == TrackState.OnFront)
					{
						trackState = TrackState.NoTrack;
						Matrix val7;
						if (Velocity.X > 0f)
						{
							val7 = ((_leftSideConnection[num9] != 2) ? Matrix.CreateRotationZ((float)Math.PI / 4f) : Matrix.CreateRotationZ(-(float)Math.PI / 4f));
						}
						else
						{
							val7 = ((_rightSideConnection[num9] != 2) ? Matrix.CreateRotationZ(-(float)Math.PI / 4f) : Matrix.CreateRotationZ((float)Math.PI / 4f));
						}
						val5 = Vector2.Transform(new Vector2(Velocity.X, 0f), val7);
						val5.X = Velocity.X;
						flag4 = true;
						num = 0f;
					}
					break;
				default:
				{
					float num12 = num6 * 16 + num11;
					if (num5 != num2 && trackState == TrackState.NoTrack && val4.Y > num12 && val4.Y - num12 < 2f)
					{
						flag8 = false;
						trackState = TrackState.AboveFront;
					}
					TrackState trackState4 = ((!(val4.Y < num12)) ? ((!(val4.Y > num12)) ? TrackState.OnTrack : TrackState.BelowTrack) : TrackState.AboveTrack);
					if (num10 != -1)
					{
						float num13 = num6 * 16 + _tileHeight[num10][num7];
						trackState3 = ((!(val4.Y < num13)) ? ((!(val4.Y > num13)) ? TrackState.OnTrack : TrackState.BelowTrack) : TrackState.AboveTrack);
					}
					switch (trackState4)
					{
					case TrackState.OnTrack:
						trackState2 = ((trackState3 == TrackState.OnTrack) ? TrackState.OnTrack : TrackState.OnFront);
						break;
					case TrackState.AboveTrack:
						trackState2 = trackState3 switch
						{
							TrackState.OnTrack => TrackState.OnBack, 
							TrackState.BelowTrack => TrackState.AboveFront, 
							TrackState.AboveTrack => TrackState.AboveTrack, 
							_ => TrackState.AboveFront, 
						};
						break;
					case TrackState.BelowTrack:
						trackState2 = trackState3 switch
						{
							TrackState.OnTrack => TrackState.OnBack, 
							TrackState.AboveTrack => TrackState.AboveBack, 
							TrackState.BelowTrack => TrackState.BelowTrack, 
							_ => TrackState.BelowTrack, 
						};
						break;
					}
					break;
				}
				}
			}
			if (!flag8)
			{
				if (trackState != trackState2)
				{
					bool flag9 = false;
					if (flag7 || val3.Y > 0f)
					{
						switch (trackState)
						{
						case TrackState.AboveTrack:
							switch (trackState2)
							{
							case TrackState.AboveFront:
								trackState2 = TrackState.OnBack;
								break;
							case TrackState.AboveBack:
								trackState2 = TrackState.OnFront;
								break;
							case TrackState.AboveTrack:
								trackState2 = TrackState.OnTrack;
								break;
							}
							break;
						case TrackState.AboveFront:
							if (trackState2 == TrackState.BelowTrack)
							{
								trackState2 = TrackState.OnFront;
							}
							break;
						case TrackState.AboveBack:
							if (trackState2 == TrackState.BelowTrack)
							{
								trackState2 = TrackState.OnBack;
							}
							break;
						case TrackState.OnFront:
							trackState2 = TrackState.OnFront;
							flag9 = true;
							break;
						case TrackState.OnBack:
							trackState2 = TrackState.OnBack;
							flag9 = true;
							break;
						case TrackState.OnTrack:
						{
							int num15 = _tileHeight[tile.FrontTrack()][num7];
							int num16 = _tileHeight[tile.BackTrack()][num7];
							if (followDown)
							{
								trackState2 = ((num15 >= num16) ? TrackState.OnFront : TrackState.OnBack);
							}
							else if (followUp)
							{
								trackState2 = ((num15 >= num16) ? TrackState.OnBack : TrackState.OnFront);
							}
							else
							{
								trackState2 = TrackState.OnFront;
							}
							flag9 = true;
							break;
						}
						}
						int num17 = -1;
						switch (trackState2)
						{
						case TrackState.OnTrack:
						case TrackState.OnFront:
							num17 = tile.FrontTrack();
							break;
						case TrackState.OnBack:
							num17 = tile.BackTrack();
							break;
						}
						if (num17 != -1)
						{
							if (!flag9 && Velocity.Y > Player.defaultGravity)
							{
								int num18 = (int)(Position.Y / 16f);
								if (fallStart < num18 - 1)
								{
									delegatesData.MinecartLandingSound(Player, Position, Width, Height);
									WheelSparks(delegatesData.MinecartDust, Position, Width, Height, 10, minecartSettings);
								}
							}
							if (trackState == TrackState.AboveFront && _trackType[num17] == 1)
							{
								flag3 = true;
							}
							val3.Y = 0f;
							val4.Y = num6 * 16 + _tileHeight[num17][num7];
						}
					}
				}
			}
			else if (trackState2 == TrackState.OnFront || trackState2 == TrackState.OnBack || trackState2 == TrackState.OnTrack)
			{
				if (flag && _trackType[tile.FrontTrack()] == 1)
				{
					flag3 = true;
				}
				val3.Y = 0f;
			}
			if (trackState2 == TrackState.OnFront)
			{
				int num19 = tile.FrontTrack();
				if (_trackType[num19] == 2 && lastBoost.X == 0f && lastBoost.Y == 0f)
				{
					lastBoost = new Vector2((float)num5, (float)num6);
					if (_boostLeft[num19])
					{
						result[4] = true;
					}
					else
					{
						result[5] = true;
					}
				}
			}
			num4 = num7;
			trackState = trackState2;
			num2 = num5;
			num3 = num6;
			if (num > 0f)
			{
				float num20 = val4.X % 2f;
				float num21 = val4.Y % 2f;
				float num22 = 3f;
				float num23 = 3f;
				if (val3.X < 0f)
				{
					num22 = num20 + 0.125f;
				}
				else if (val3.X > 0f)
				{
					num22 = 2f - num20;
				}
				if (val3.Y < 0f)
				{
					num23 = num21 + 0.125f;
				}
				else if (val3.Y > 0f)
				{
					num23 = 2f - num21;
				}
				if (num22 == 3f && num23 == 3f)
				{
					break;
				}
				float num24 = Math.Abs(num22 / val3.X);
				float num25 = Math.Abs(num23 / val3.Y);
				float num26 = ((num24 < num25) ? num24 : num25);
				if (num26 > num)
				{
					val6 = val3 * num;
					num = 0f;
				}
				else
				{
					val6 = val3 * num26;
					num -= num26;
				}
				val4 += val6;
				continue;
			}
			if (lastBoost.X != (float)num2 || lastBoost.Y != (float)num3)
			{
				lastBoost = Vector2.Zero;
			}
			break;
		}
		if (flag3)
		{
			result[3] = true;
		}
		if (flag5)
		{
			Velocity.X = val4.X - val2.X;
			Velocity.Y = Player.defaultGravity;
		}
		else if (flag4)
		{
			result[2] = true;
			Velocity = val5;
		}
		else if (result[1])
		{
			Velocity.X = 0f - Velocity.X;
			Position.X = val4.X - minecartSettings.MagnetOffset.X - val.X - Velocity.X;
			if (val3.Y == 0f)
			{
				Velocity.Y = 0f;
			}
		}
		else
		{
			if (flag6)
			{
				Velocity.X = val4.X - val2.X;
			}
			if (val3.Y == 0f)
			{
				Velocity.Y = 0f;
			}
		}
		Position.Y += val4.Y - val2.Y - Velocity.Y;
		Position.Y = (float)Math.Round(Position.Y, 2);
		if (trackState == TrackState.OnTrack || (uint)(trackState - 5) <= 1u)
		{
			result[0] = true;
		}
		return result;
	}

	public static bool FrameTrack(int i, int j, bool pound, bool mute = false)
	{
		if (_trackType == null)
		{
			return false;
		}
		Tile tile = Main.tile[i, j];
		if (tile == null)
		{
			tile = new Tile();
			Main.tile[i, j] = tile;
		}
		if (mute && tile.type != 314)
		{
			return false;
		}
		int nearbyTilesSetLookupIndex = GetNearbyTilesSetLookupIndex(i, j);
		int num = tile.FrontTrack();
		int num2 = tile.BackTrack();
		int num3 = ((num >= 0 && num < _trackType.Length) ? _trackType[num] : 0);
		int num4 = -1;
		int num5 = -1;
		int[] array = _trackSwitchOptions[nearbyTilesSetLookupIndex];
		if (array == null)
		{
			if (pound)
			{
				return false;
			}
			tile.FrontTrack(0);
			tile.BackTrack(-1);
			return false;
		}
		if (!pound)
		{
			int num6 = -1;
			int num7 = -1;
			bool flag = false;
			for (int k = 0; k < array.Length; k++)
			{
				int num8 = array[k];
				if (num2 == array[k])
				{
					num5 = k;
				}
				if (_trackType[num8] != num3)
				{
					continue;
				}
				if (_leftSideConnection[num8] == -1 || _rightSideConnection[num8] == -1)
				{
					if (num == array[k])
					{
						num4 = k;
						flag = true;
					}
					if (num6 == -1)
					{
						num6 = k;
					}
				}
				else
				{
					if (num == array[k])
					{
						num4 = k;
						flag = false;
					}
					if (num7 == -1)
					{
						num7 = k;
					}
				}
			}
			if (num7 != -1)
			{
				if ((num4 == -1) | flag)
				{
					num4 = num7;
				}
			}
			else
			{
				if (num4 == -1)
				{
					switch (num3)
					{
					case 2:
						return false;
					case 1:
						return false;
					}
					num4 = num6;
				}
				num5 = -1;
			}
		}
		else
		{
			for (int l = 0; l < array.Length; l++)
			{
				if (num == array[l])
				{
					num4 = l;
				}
				if (num2 == array[l])
				{
					num5 = l;
				}
			}
			int num9 = 0;
			int num10 = 0;
			for (int m = 0; m < array.Length; m++)
			{
				if (_trackType[array[m]] == num3)
				{
					if (_leftSideConnection[array[m]] == -1 || _rightSideConnection[array[m]] == -1)
					{
						num10++;
					}
					else
					{
						num9++;
					}
				}
			}
			if (num9 < 2 && num10 < 2)
			{
				return false;
			}
			bool flag2 = num9 == 0;
			bool flag3 = false;
			if (!flag2)
			{
				while (!flag3)
				{
					num5++;
					if (num5 >= array.Length)
					{
						num5 = -1;
						break;
					}
					if ((_leftSideConnection[array[num5]] != _leftSideConnection[array[num4]] || _rightSideConnection[array[num5]] != _rightSideConnection[array[num4]]) && _trackType[array[num5]] == num3 && _leftSideConnection[array[num5]] != -1 && _rightSideConnection[array[num5]] != -1)
					{
						flag3 = true;
					}
				}
			}
			if (!flag3)
			{
				do
				{
					num4++;
					if (num4 >= array.Length)
					{
						num4 = -1;
						do
						{
							num4++;
						}
						while (_trackType[array[num4]] != num3 || (_leftSideConnection[array[num4]] == -1 || _rightSideConnection[array[num4]] == -1) != flag2);
						break;
					}
				}
				while (_trackType[array[num4]] != num3 || (_leftSideConnection[array[num4]] == -1 || _rightSideConnection[array[num4]] == -1) != flag2);
			}
		}
		bool flag4 = false;
		switch (num4)
		{
		case -2:
			if (tile.FrontTrack() != _firstPressureFrame)
			{
				flag4 = true;
			}
			break;
		case -1:
			if (tile.FrontTrack() != 0)
			{
				flag4 = true;
			}
			break;
		default:
			if (tile.FrontTrack() != array[num4])
			{
				flag4 = true;
			}
			break;
		}
		if (num5 == -1)
		{
			if (tile.BackTrack() != -1)
			{
				flag4 = true;
			}
		}
		else if (tile.BackTrack() != array[num5])
		{
			flag4 = true;
		}
		switch (num4)
		{
		case -2:
			tile.FrontTrack(_firstPressureFrame);
			break;
		case -1:
			tile.FrontTrack(0);
			break;
		default:
			tile.FrontTrack((short)array[num4]);
			break;
		}
		if (num5 == -1)
		{
			tile.BackTrack(-1);
		}
		else
		{
			tile.BackTrack((short)array[num5]);
		}
		if ((pound & flag4) && !mute)
		{
			WorldGen.KillTile(i, j, fail: true);
		}
		return true;
	}

	private static int GetNearbyTilesSetLookupIndex(int i, int j)
	{
		int num = 0;
		if (Main.tile[i - 1, j - 1] != null && Main.tile[i - 1, j - 1].type == 314)
		{
			num++;
		}
		if (Main.tile[i - 1, j] != null && Main.tile[i - 1, j].type == 314)
		{
			num += 2;
		}
		if (Main.tile[i - 1, j + 1] != null && Main.tile[i - 1, j + 1].type == 314)
		{
			num += 4;
		}
		if (Main.tile[i + 1, j - 1] != null && Main.tile[i + 1, j - 1].type == 314)
		{
			num += 8;
		}
		if (Main.tile[i + 1, j] != null && Main.tile[i + 1, j].type == 314)
		{
			num += 16;
		}
		if (Main.tile[i + 1, j + 1] != null && Main.tile[i + 1, j + 1].type == 314)
		{
			num += 32;
		}
		return num;
	}

	public static bool GetOnTrack(int tileX, int tileY, ref Vector2 Position, int Width, int Height, Customization settings)
	{
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0101: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		//IL_0104: Unknown result type (might be due to invalid IL or missing references)
		Tile tile = Main.tile[tileX, tileY];
		if (tile.type != 314)
		{
			return false;
		}
		Vector2 val = new Vector2((float)(Width / 2) - settings.MinecartTextureWidth / 2f, (float)(Height / 2));
		Vector2 val2 = Position + val + settings.MagnetOffset;
		int num = (int)val2.X % 16 / 2;
		int num2 = -1;
		int num3 = 0;
		for (int i = num; i < 8; i++)
		{
			num3 = _tileHeight[tile.frameX][i];
			if (num3 >= 0)
			{
				num2 = i;
				break;
			}
		}
		if (num2 == -1)
		{
			for (int num4 = num - 1; num4 >= 0; num4--)
			{
				num3 = _tileHeight[tile.frameX][num4];
				if (num3 >= 0)
				{
					num2 = num4;
					break;
				}
			}
		}
		if (num2 == -1)
		{
			return false;
		}
		val2.X = tileX * 16 + num2 * 2;
		val2.Y = tileY * 16 + num3;
		val2 -= settings.MagnetOffset;
		val2 -= val;
		Position = val2;
		return true;
	}

	public static bool OnTrack(Vector2 Position, int Width, int Height, Customization settings)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		Vector2 val = Position + new Vector2((float)(Width / 2) - settings.MinecartTextureWidth / 2f, (float)(Height / 2)) + settings.MagnetOffset;
		int num = (int)(val.X / 16f);
		int num2 = (int)(val.Y / 16f);
		if (Main.tile[num, num2] == null)
		{
			return false;
		}
		return Main.tile[num, num2].type == 314;
	}

	public static float TrackRotation(Player player, ref float rotation, Vector2 Position, int Width, int Height, bool followDown, bool followUp, Mount.MountDelegatesData delegatesData)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		GetWheelsPositions(player, Position, Width, Height, followDown, followUp, delegatesData, out var leftWheel, out var rightWheel);
		float num = rightWheel.Y - leftWheel.Y;
		float num2 = rightWheel.X - leftWheel.X;
		float num3 = num / num2;
		float num4 = leftWheel.Y + (Position.X - leftWheel.X) * num3;
		float num5 = (Position.X - (float)(int)Position.X) * num3;
		rotation = (float)Math.Atan2(num, num2);
		return num4 - Position.Y + num5;
	}

	public static void GetWheelsPositions(Player player, Vector2 Position, int Width, int Height, bool followDown, bool followUp, Mount.MountDelegatesData delegatesData, out Vector2 leftWheel, out Vector2 rightWheel)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		leftWheel = Position;
		rightWheel = Position;
		Vector2 lastBoost = Vector2.Zero;
		Customization minecartSettings = player.MinecartSettings;
		Vector2 Velocity = minecartSettings.WheelOffset * new Vector2(-1f, 1f);
		TrackCollision(player, ref leftWheel, ref Velocity, ref lastBoost, Width, Height, followDown, followUp, 0, trackOnly: true, delegatesData);
		leftWheel += Velocity;
		Velocity = minecartSettings.WheelOffset;
		TrackCollision(player, ref rightWheel, ref Velocity, ref lastBoost, Width, Height, followDown, followUp, 0, trackOnly: true, delegatesData);
		rightWheel += Velocity;
	}

	public static void HitTrackSwitch(Vector2 Position, int Width, int Height, Customization settings)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0004: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		Vector2 magnetPosition = GetMagnetPosition(Position, Width, Height, settings);
		int num = (int)(magnetPosition.X / 16f);
		int num2 = (int)(magnetPosition.Y / 16f);
		Wiring.HitSwitch(num, num2);
		NetMessage.SendData(59, -1, -1, null, num, num2);
	}

	public static Vector2 GetMagnetPosition(Vector2 Position, int Width, int Height, Customization settings)
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		new Vector2((float)(Width / 2) - settings.MinecartTextureWidth / 2f, (float)(Height / 2));
		return Position + new Vector2((float)(Width / 2) - settings.MinecartTextureWidth / 2f, (float)(Height / 2)) + settings.MagnetOffset;
	}

	public static void FlipSwitchTrack(int i, int j)
	{
		Tile tileTrack = Main.tile[i, j];
		short num = tileTrack.FrontTrack();
		if (num == -1)
		{
			return;
		}
		switch (_trackType[num])
		{
		case 0:
			if (tileTrack.BackTrack() != -1)
			{
				tileTrack.FrontTrack(tileTrack.BackTrack());
				tileTrack.BackTrack(num);
				NetMessage.SendTileSquare(-1, i, j);
			}
			break;
		case 2:
			FrameTrack(i, j, pound: true, mute: true);
			NetMessage.SendTileSquare(-1, i, j);
			break;
		}
	}

	public static void TrackColors(int i, int j, Tile trackTile, out int frontColor, out int backColor)
	{
		if (trackTile.type == 314)
		{
			frontColor = trackTile.color();
			backColor = frontColor;
			if (trackTile.frameY == -1)
			{
				return;
			}
			int num = _leftSideConnection[trackTile.frameX];
			int num2 = _rightSideConnection[trackTile.frameX];
			int num3 = _leftSideConnection[trackTile.frameY];
			int num4 = _rightSideConnection[trackTile.frameY];
			int num5 = 0;
			int num6 = 0;
			int num7 = 0;
			int num8 = 0;
			for (int k = 0; k < 4; k++)
			{
				int num9 = (k switch
				{
					1 => num2, 
					2 => num3, 
					3 => num4, 
					_ => num, 
				}) switch
				{
					0 => -1, 
					1 => 0, 
					2 => 1, 
					_ => 0, 
				};
				Tile tile = ((k % 2 != 0) ? Main.tile[i + 1, j + num9] : Main.tile[i - 1, j + num9]);
				int num10 = ((tile != null && tile.active() && tile.type == 314) ? tile.color() : 0);
				switch (k)
				{
				default:
					num5 = num10;
					break;
				case 1:
					num6 = num10;
					break;
				case 2:
					num7 = num10;
					break;
				case 3:
					num8 = num10;
					break;
				}
			}
			if (num == num3)
			{
				if (num6 != 0)
				{
					frontColor = num6;
				}
				else if (num5 != 0)
				{
					frontColor = num5;
				}
				if (num8 != 0)
				{
					backColor = num8;
				}
				else if (num7 != 0)
				{
					backColor = num7;
				}
				return;
			}
			if (num2 == num4)
			{
				if (num5 != 0)
				{
					frontColor = num5;
				}
				else if (num6 != 0)
				{
					frontColor = num6;
				}
				if (num7 != 0)
				{
					backColor = num7;
				}
				else if (num8 != 0)
				{
					backColor = num8;
				}
				return;
			}
			if (num6 == 0)
			{
				if (num5 != 0)
				{
					frontColor = num5;
				}
			}
			else if (num5 != 0)
			{
				frontColor = ((num2 <= num) ? num6 : num5);
			}
			if (num8 == 0)
			{
				if (num7 != 0)
				{
					backColor = num7;
				}
			}
			else if (num7 != 0)
			{
				backColor = ((num4 <= num3) ? num8 : num7);
			}
		}
		else
		{
			frontColor = 0;
			backColor = 0;
		}
	}

	public static bool DrawLeftDecoration(int frameID)
	{
		if (frameID < 0 || frameID >= 36)
		{
			return false;
		}
		return _leftSideConnection[frameID] == 2;
	}

	public static bool DrawRightDecoration(int frameID)
	{
		if (frameID < 0 || frameID >= 36)
		{
			return false;
		}
		return _rightSideConnection[frameID] == 2;
	}

	public static bool DrawBumper(int frameID)
	{
		if (frameID < 0 || frameID >= 36)
		{
			return false;
		}
		if (_tileHeight[frameID][0] != -1)
		{
			return _tileHeight[frameID][7] == -1;
		}
		return true;
	}

	public static bool DrawBouncyBumper(int frameID)
	{
		if (frameID < 0 || frameID >= 36)
		{
			return false;
		}
		if (_tileHeight[frameID][0] != -2)
		{
			return _tileHeight[frameID][7] == -2;
		}
		return true;
	}

	public static void PlaceTrack(Tile trackCache, int style)
	{
		trackCache.active(active: true);
		trackCache.type = 314;
		trackCache.frameY = -1;
		switch (style)
		{
		case 0:
			trackCache.frameX = -1;
			break;
		case 1:
			trackCache.frameX = _firstPressureFrame;
			break;
		case 2:
			trackCache.frameX = _firstLeftBoostFrame;
			break;
		case 3:
			trackCache.frameX = _firstRightBoostFrame;
			break;
		}
	}

	public static int GetTrackItem(Tile trackCache)
	{
		return _trackType[trackCache.frameX] switch
		{
			0 => 2340, 
			1 => 2492, 
			2 => 2739, 
			_ => 0, 
		};
	}

	public static Rectangle GetSourceRect(int frameID, int animationFrame = 0)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		if (frameID < 0 || frameID >= 40)
		{
			return new Rectangle(0, 0, 0, 0);
		}
		Vector2 val = _texturePosition[frameID];
		Rectangle result = new Rectangle((int)val.X, (int)val.Y, 16, 16);
		if (frameID < 36 && _trackType[frameID] == 2)
		{
			result.Y += 18 * animationFrame;
		}
		return result;
	}

	public static bool GetAreExpectationsForSidesMet(Point tileCoords, int? expectedYOffsetForLeft, int? expectedYOffsetForRight)
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		Tile tileTrack = Main.tile[tileCoords.X, tileCoords.Y];
		if (expectedYOffsetForLeft.HasValue)
		{
			short num = tileTrack.FrontTrack();
			int num2 = ConvertOffsetYToTrackConnectionValue(expectedYOffsetForLeft.Value);
			if (_leftSideConnection[num] != num2)
			{
				return false;
			}
		}
		if (expectedYOffsetForRight.HasValue)
		{
			short num3 = tileTrack.FrontTrack();
			int num4 = ConvertOffsetYToTrackConnectionValue(expectedYOffsetForRight.Value);
			if (_rightSideConnection[num3] != num4)
			{
				return false;
			}
		}
		return true;
	}

	public static void TryFittingTileOrientation(Point tileCoords, int? expectedYOffsetForLeft, int? expectedYOffsetForRight)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		int nearbyTilesSetLookupIndex = GetNearbyTilesSetLookupIndex(tileCoords.X, tileCoords.Y);
		int[] array = _trackSwitchOptions[nearbyTilesSetLookupIndex];
		if (array == null)
		{
			return;
		}
		Tile tileSafely = Framing.GetTileSafely(tileCoords);
		int num = _trackType[tileSafely.FrontTrack()];
		int? num2 = null;
		foreach (int num3 in array)
		{
			_ = _leftSideConnection[num3];
			_ = _rightSideConnection[num3];
			_ = _trackType[num3];
			if (expectedYOffsetForLeft.HasValue)
			{
				int num4 = ConvertOffsetYToTrackConnectionValue(expectedYOffsetForLeft.Value);
				if (_leftSideConnection[num3] != num4)
				{
					continue;
				}
			}
			if (expectedYOffsetForRight.HasValue)
			{
				int num5 = ConvertOffsetYToTrackConnectionValue(expectedYOffsetForRight.Value);
				if (_rightSideConnection[num3] != num5)
				{
					continue;
				}
			}
			if (_trackType[num3] == num)
			{
				num2 = num3;
				break;
			}
		}
		if (num2.HasValue)
		{
			tileSafely.FrontTrack((short)num2.Value);
			NetMessage.SendTileSquare(-1, tileCoords.X, tileCoords.Y);
		}
	}

	private static int ConvertOffsetYToTrackConnectionValue(int offsetY)
	{
		return offsetY switch
		{
			-1 => 0, 
			1 => 2, 
			_ => 1, 
		};
	}

	private static int ConvertTrackConnectionValueToOffsetY(int trackConnectionValue)
	{
		return trackConnectionValue switch
		{
			0 => -1, 
			2 => 1, 
			_ => 0, 
		};
	}

	public static void WheelSparks(Action<Vector2> DustAction, Vector2 Position, int Width, int Height, int sparkCount, Customization settings)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		Vector2 val = new Vector2((float)(Width / 2) - settings.MinecartTextureWidth / 2f, (float)(Height / 2));
		Vector2 obj = Position + val + settings.MagnetOffset;
		for (int i = 0; i < sparkCount; i++)
		{
			DustAction(obj);
		}
	}

	private static short FrontTrack(this Tile tileTrack)
	{
		return tileTrack.frameX;
	}

	private static void FrontTrack(this Tile tileTrack, short trackID)
	{
		tileTrack.frameX = trackID;
	}

	private static short BackTrack(this Tile tileTrack)
	{
		return tileTrack.frameY;
	}

	private static void BackTrack(this Tile tileTrack, short trackID)
	{
		tileTrack.frameY = trackID;
	}
}
