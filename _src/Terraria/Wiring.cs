using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Events;
using Terraria.GameContent.UI;
using Terraria.ID;
using Terraria.Localization;
using Terraria.UI;

namespace Terraria;

public static class Wiring
{
	public static bool blockPlayerTeleportationForOneIteration;

	public static bool running;

	private static Dictionary<Point16, bool> _wireSkip;

	private static DoubleStack<Point16> _wireList;

	private static DoubleStack<byte> _wireDirectionList;

	private static Dictionary<Point16, byte> _toProcess;

	private static Queue<Point16> _GatesCurrent;

	private static Queue<Point16> _LampsToCheck;

	private static Queue<Point16> _GatesNext;

	private static Dictionary<Point16, bool> _GatesDone;

	private static Dictionary<Point16, byte> _PixelBoxTriggers;

	private static Vector2[] _teleport;

	private const int MaxPump = 20;

	private static int[] _inPumpX;

	private static int[] _inPumpY;

	private static int _numInPump;

	private static int[] _outPumpX;

	private static int[] _outPumpY;

	private static int _numOutPump;

	private const int MaxMech = 1000;

	private static int[] _mechX;

	private static int[] _mechY;

	private static int _numMechs;

	private static int[] _mechTime;

	private static int _currentWireColor;

	private static int CurrentUser = 255;

	private static int cannonCoolDown = 0;

	private static int bunnyCannonCoolDown = 0;

	private static int snowballCannonCoolDown = 0;

	public static readonly Vector2 HopperGrabHitboxSize = new Vector2(192f);

	public static void SetCurrentUser(int plr = -1)
	{
		if (plr < 0 || plr > 255)
		{
			plr = 255;
		}
		if (Main.netMode == 0)
		{
			plr = Main.myPlayer;
		}
		CurrentUser = plr;
	}

	public static void Initialize()
	{
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		_wireSkip = new Dictionary<Point16, bool>();
		_wireList = new DoubleStack<Point16>();
		_wireDirectionList = new DoubleStack<byte>();
		_toProcess = new Dictionary<Point16, byte>();
		_GatesCurrent = new Queue<Point16>();
		_GatesNext = new Queue<Point16>();
		_GatesDone = new Dictionary<Point16, bool>();
		_LampsToCheck = new Queue<Point16>();
		_PixelBoxTriggers = new Dictionary<Point16, byte>();
		_inPumpX = new int[20];
		_inPumpY = new int[20];
		_outPumpX = new int[20];
		_outPumpY = new int[20];
		_teleport = new Vector2[2]
		{
			Vector2.One * -1f,
			Vector2.One * -1f
		};
		_mechX = new int[1000];
		_mechY = new int[1000];
		_mechTime = new int[1000];
	}

	public static void SkipWire(int x, int y)
	{
		_wireSkip[new Point16(x, y)] = true;
	}

	public static void SkipWire(Point16 point)
	{
		_wireSkip[point] = true;
	}

	public static void ClearAll()
	{
		for (int i = 0; i < 20; i++)
		{
			_inPumpX[i] = 0;
			_inPumpY[i] = 0;
			_outPumpX[i] = 0;
			_outPumpY[i] = 0;
		}
		_numInPump = 0;
		_numOutPump = 0;
		for (int j = 0; j < 1000; j++)
		{
			_mechTime[j] = 0;
			_mechX[j] = 0;
			_mechY[j] = 0;
		}
		_numMechs = 0;
	}

	public static void UpdateMech()
	{
		if (cannonCoolDown > 0)
		{
			cannonCoolDown--;
		}
		if (bunnyCannonCoolDown > 0)
		{
			bunnyCannonCoolDown--;
		}
		if (snowballCannonCoolDown > 0)
		{
			snowballCannonCoolDown--;
		}
		SetCurrentUser();
		for (int num = _numMechs - 1; num >= 0; num--)
		{
			_mechTime[num]--;
			int num2 = _mechX[num];
			int num3 = _mechY[num];
			if (!WorldGen.InWorld(num2, num3, 1))
			{
				_numMechs--;
			}
			else
			{
				Tile tile = Main.tile[num2, num3];
				if (tile == null)
				{
					_numMechs--;
				}
				else
				{
					if (tile.active() && tile.type == 144)
					{
						if (tile.frameY == 0)
						{
							_mechTime[num] = 0;
						}
						else
						{
							int num4 = tile.frameX / 18;
							switch (num4)
							{
							case 0:
								num4 = 60;
								break;
							case 1:
								num4 = 180;
								break;
							case 2:
								num4 = 300;
								break;
							case 3:
								num4 = 30;
								break;
							case 4:
								num4 = 15;
								break;
							}
							if (Math.IEEERemainder(_mechTime[num], num4) == 0.0)
							{
								_mechTime[num] = 18000;
								TripWire(_mechX[num], _mechY[num], 1, 1);
							}
						}
					}
					if (_mechTime[num] <= 0)
					{
						if (tile.active() && tile.type == 144)
						{
							tile.frameY = 0;
							NetMessage.SendTileSquare(-1, _mechX[num], _mechY[num]);
						}
						if (tile.active() && tile.type == 411)
						{
							int num5 = tile.frameX % 36 / 18;
							int num6 = tile.frameY % 36 / 18;
							int num7 = _mechX[num] - num5;
							int num8 = _mechY[num] - num6;
							int num9 = 36;
							if (Main.tile[num7, num8].frameX >= 36)
							{
								num9 = -36;
							}
							for (int i = num7; i < num7 + 2; i++)
							{
								for (int j = num8; j < num8 + 2; j++)
								{
									if (WorldGen.InWorld(i, j, 1))
									{
										Tile tile2 = Main.tile[i, j];
										if (tile2 != null)
										{
											tile2.frameX = (short)(tile2.frameX + num9);
										}
									}
								}
							}
							NetMessage.SendTileSquare(-1, num7, num8, 2, 2);
						}
						for (int k = num; k < _numMechs; k++)
						{
							_mechX[k] = _mechX[k + 1];
							_mechY[k] = _mechY[k + 1];
							_mechTime[k] = _mechTime[k + 1];
						}
						_numMechs--;
					}
				}
			}
		}
	}

	public static void HitSwitch(int i, int j)
	{
		if (!WorldGen.InWorld(i, j) || Main.tile[i, j] == null)
		{
			return;
		}
		if (Main.tile[i, j].type == 135 || Main.tile[i, j].type == 314 || Main.tile[i, j].type == 423 || Main.tile[i, j].type == 428 || Main.tile[i, j].type == 442 || Main.tile[i, j].type == 476)
		{
			SoundEngine.PlaySound(28, i * 16, j * 16, 0);
			TripWire(i, j, 1, 1);
		}
		else if (Main.tile[i, j].type == 440)
		{
			SoundEngine.PlaySound(28, i * 16 + 16, j * 16 + 16, 0);
			TripWire(i, j, 3, 3);
		}
		else if (Main.tile[i, j].type == 136)
		{
			if (Main.tile[i, j].frameY == 0)
			{
				Main.tile[i, j].frameY = 18;
			}
			else
			{
				Main.tile[i, j].frameY = 0;
			}
			SoundEngine.PlaySound(28, i * 16, j * 16, 0);
			TripWire(i, j, 1, 1);
		}
		else if (Main.tile[i, j].type == 210)
		{
			ExplodeMine(i, j);
		}
		else if (Main.tile[i, j].type == 443)
		{
			GeyserTrap(i, j);
		}
		else if (Main.tile[i, j].type == 144)
		{
			if (Main.tile[i, j].frameY == 0)
			{
				Main.tile[i, j].frameY = 18;
				if (Main.netMode != 1)
				{
					CheckMech(i, j, 18000);
				}
			}
			else
			{
				Main.tile[i, j].frameY = 0;
			}
			SoundEngine.PlaySound(28, i * 16, j * 16, 0);
		}
		else if (Main.tile[i, j].type == 441 || Main.tile[i, j].type == 468)
		{
			int num = Main.tile[i, j].frameX / 18 * -1;
			int num2 = Main.tile[i, j].frameY / 18 * -1;
			num %= 4;
			if (num < -1)
			{
				num += 2;
			}
			num += i;
			num2 += j;
			SoundEngine.PlaySound(28, i * 16, j * 16, 0);
			TripWire(num, num2, 2, 2);
		}
		else if (Main.tile[i, j].type == 467)
		{
			if (Main.tile[i, j].frameX / 36 == 4)
			{
				int num3 = Main.tile[i, j].frameX / 18 * -1;
				int num4 = Main.tile[i, j].frameY / 18 * -1;
				num3 %= 4;
				if (num3 < -1)
				{
					num3 += 2;
				}
				num3 += i;
				num4 += j;
				SoundEngine.PlaySound(28, i * 16, j * 16, 0);
				TripWire(num3, num4, 2, 2);
			}
		}
		else
		{
			if (Main.tile[i, j].type != 132 && Main.tile[i, j].type != 411)
			{
				return;
			}
			short num5 = 36;
			int num6 = Main.tile[i, j].frameX / 18 * -1;
			int num7 = Main.tile[i, j].frameY / 18 * -1;
			num6 %= 4;
			if (num6 < -1)
			{
				num6 += 2;
				num5 = -36;
			}
			num6 += i;
			num7 += j;
			if (Main.netMode != 1 && Main.tile[num6, num7].type == 411)
			{
				CheckMech(num6, num7, 60);
			}
			for (int k = num6; k < num6 + 2; k++)
			{
				for (int l = num7; l < num7 + 2; l++)
				{
					if (Main.tile[k, l].type == 132 || Main.tile[k, l].type == 411)
					{
						Main.tile[k, l].frameX += num5;
					}
				}
			}
			WorldGen.TileFrame(num6, num7);
			SoundEngine.PlaySound(28, i * 16, j * 16, 0);
			TripWire(num6, num7, 2, 2);
		}
	}

	public static void PokeLogicGate(int lampX, int lampY)
	{
		if (Main.netMode != 1)
		{
			_LampsToCheck.Enqueue(new Point16(lampX, lampY));
			LogicGatePass();
		}
	}

	public static bool Actuate(int i, int j)
	{
		Tile tile = Main.tile[i, j];
		if (!tile.actuator())
		{
			return false;
		}
		if (tile.inActive())
		{
			ReActive(i, j);
		}
		else
		{
			DeActive(i, j);
		}
		return true;
	}

	public static void ActuateForced(int i, int j)
	{
		if (Main.tile[i, j].inActive())
		{
			ReActive(i, j);
		}
		else
		{
			DeActive(i, j);
		}
	}

	public static void MassWireOperation(Point ps, Point pe, Player master)
	{
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		int wireCount = 0;
		int actuatorCount = 0;
		for (int i = 0; i < 58; i++)
		{
			if (master.inventory[i].type == 530)
			{
				wireCount += master.inventory[i].stack;
			}
			if (master.inventory[i].type == 849)
			{
				actuatorCount += master.inventory[i].stack;
			}
		}
		int num = wireCount;
		int num2 = actuatorCount;
		MassWireOperationInner(master, ps, pe, master.Center, master.direction == 1, ref wireCount, ref actuatorCount);
		int num3 = num - wireCount;
		int num4 = num2 - actuatorCount;
		if (Main.netMode == 2)
		{
			NetMessage.SendData(110, master.whoAmI, -1, null, 530, num3, master.whoAmI);
			NetMessage.SendData(110, master.whoAmI, -1, null, 849, num4, master.whoAmI);
			return;
		}
		for (int j = 0; j < num3; j++)
		{
			master.ConsumeItem(530);
		}
		for (int k = 0; k < num4; k++)
		{
			master.ConsumeItem(849);
		}
	}

	private static bool CheckMech(int i, int j, int time)
	{
		for (int k = 0; k < _numMechs; k++)
		{
			if (_mechX[k] == i && _mechY[k] == j)
			{
				return false;
			}
		}
		if (_numMechs < 999)
		{
			_mechX[_numMechs] = i;
			_mechY[_numMechs] = j;
			_mechTime[_numMechs] = time;
			_numMechs++;
			return true;
		}
		return false;
	}

	private static void XferWater()
	{
		for (int i = 0; i < _numInPump; i++)
		{
			int num = _inPumpX[i];
			int num2 = _inPumpY[i];
			int liquid = Main.tile[num, num2].liquid;
			if (liquid <= 0)
			{
				continue;
			}
			byte b = Main.tile[num, num2].liquidType();
			for (int j = 0; j < _numOutPump; j++)
			{
				int num3 = _outPumpX[j];
				int num4 = _outPumpY[j];
				int liquid2 = Main.tile[num3, num4].liquid;
				if (liquid2 >= 255)
				{
					continue;
				}
				byte b2 = Main.tile[num3, num4].liquidType();
				if (liquid2 == 0)
				{
					b2 = b;
				}
				if (b2 == b)
				{
					int num5 = liquid;
					if (num5 + liquid2 > 255)
					{
						num5 = 255 - liquid2;
					}
					Main.tile[num3, num4].liquid += (byte)num5;
					Main.tile[num, num2].liquid -= (byte)num5;
					liquid = Main.tile[num, num2].liquid;
					Main.tile[num3, num4].liquidType(b);
					WorldGen.SquareTileFrame(num3, num4);
					if (Main.tile[num, num2].liquid == 0)
					{
						Main.tile[num, num2].liquidType(0);
						WorldGen.SquareTileFrame(num, num2);
						break;
					}
				}
			}
			WorldGen.SquareTileFrame(num, num2);
		}
	}

	private static void TripWire(int left, int top, int width, int height)
	{
		//IL_0130: Unknown result type (might be due to invalid IL or missing references)
		//IL_0135: Unknown result type (might be due to invalid IL or missing references)
		//IL_0146: Unknown result type (might be due to invalid IL or missing references)
		//IL_014b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0240: Unknown result type (might be due to invalid IL or missing references)
		//IL_0245: Unknown result type (might be due to invalid IL or missing references)
		//IL_0256: Unknown result type (might be due to invalid IL or missing references)
		//IL_025b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0350: Unknown result type (might be due to invalid IL or missing references)
		//IL_0355: Unknown result type (might be due to invalid IL or missing references)
		//IL_0366: Unknown result type (might be due to invalid IL or missing references)
		//IL_036b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0460: Unknown result type (might be due to invalid IL or missing references)
		//IL_0465: Unknown result type (might be due to invalid IL or missing references)
		//IL_0476: Unknown result type (might be due to invalid IL or missing references)
		//IL_047b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0494: Unknown result type (might be due to invalid IL or missing references)
		//IL_0499: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ae: Unknown result type (might be due to invalid IL or missing references)
		if (Main.netMode == 1)
		{
			return;
		}
		running = true;
		if (_wireList.Count != 0)
		{
			_wireList.Clear(quickClear: true);
		}
		if (_wireDirectionList.Count != 0)
		{
			_wireDirectionList.Clear(quickClear: true);
		}
		Vector2[] array = new Vector2[8];
		int num = 0;
		for (int i = left; i < left + width; i++)
		{
			for (int j = top; j < top + height; j++)
			{
				Point16 back = new Point16(i, j);
				Tile tile = Main.tile[i, j];
				if (tile != null && tile.wire())
				{
					_wireList.PushBack(back);
				}
			}
		}
		_teleport[0].X = -1f;
		_teleport[0].Y = -1f;
		_teleport[1].X = -1f;
		_teleport[1].Y = -1f;
		if (_wireList.Count > 0)
		{
			_numInPump = 0;
			_numOutPump = 0;
			HitWire(_wireList, 1);
			if (_numInPump > 0 && _numOutPump > 0)
			{
				XferWater();
			}
		}
		array[num++] = _teleport[0];
		array[num++] = _teleport[1];
		for (int k = left; k < left + width; k++)
		{
			for (int l = top; l < top + height; l++)
			{
				Point16 back = new Point16(k, l);
				Tile tile2 = Main.tile[k, l];
				if (tile2 != null && tile2.wire2())
				{
					_wireList.PushBack(back);
				}
			}
		}
		_teleport[0].X = -1f;
		_teleport[0].Y = -1f;
		_teleport[1].X = -1f;
		_teleport[1].Y = -1f;
		if (_wireList.Count > 0)
		{
			_numInPump = 0;
			_numOutPump = 0;
			HitWire(_wireList, 2);
			if (_numInPump > 0 && _numOutPump > 0)
			{
				XferWater();
			}
		}
		array[num++] = _teleport[0];
		array[num++] = _teleport[1];
		_teleport[0].X = -1f;
		_teleport[0].Y = -1f;
		_teleport[1].X = -1f;
		_teleport[1].Y = -1f;
		for (int m = left; m < left + width; m++)
		{
			for (int n = top; n < top + height; n++)
			{
				Point16 back = new Point16(m, n);
				Tile tile3 = Main.tile[m, n];
				if (tile3 != null && tile3.wire3())
				{
					_wireList.PushBack(back);
				}
			}
		}
		if (_wireList.Count > 0)
		{
			_numInPump = 0;
			_numOutPump = 0;
			HitWire(_wireList, 3);
			if (_numInPump > 0 && _numOutPump > 0)
			{
				XferWater();
			}
		}
		array[num++] = _teleport[0];
		array[num++] = _teleport[1];
		_teleport[0].X = -1f;
		_teleport[0].Y = -1f;
		_teleport[1].X = -1f;
		_teleport[1].Y = -1f;
		for (int num2 = left; num2 < left + width; num2++)
		{
			for (int num3 = top; num3 < top + height; num3++)
			{
				Point16 back = new Point16(num2, num3);
				Tile tile4 = Main.tile[num2, num3];
				if (tile4 != null && tile4.wire4())
				{
					_wireList.PushBack(back);
				}
			}
		}
		if (_wireList.Count > 0)
		{
			_numInPump = 0;
			_numOutPump = 0;
			HitWire(_wireList, 4);
			if (_numInPump > 0 && _numOutPump > 0)
			{
				XferWater();
			}
		}
		array[num++] = _teleport[0];
		array[num++] = _teleport[1];
		running = false;
		for (int num4 = 0; num4 < 8; num4 += 2)
		{
			_teleport[0] = array[num4];
			_teleport[1] = array[num4 + 1];
			if (_teleport[0].X >= 0f && _teleport[1].X >= 0f)
			{
				Teleport();
			}
		}
		PixelBoxPass();
		LogicGatePass();
	}

	private static void PixelBoxPass()
	{
		foreach (KeyValuePair<Point16, byte> pixelBoxTrigger in _PixelBoxTriggers)
		{
			if (pixelBoxTrigger.Value == 3)
			{
				Tile tile = Main.tile[pixelBoxTrigger.Key.X, pixelBoxTrigger.Key.Y];
				tile.frameX = (short)((tile.frameX != 18) ? 18 : 0);
				NetMessage.SendTileSquare(-1, pixelBoxTrigger.Key.X, pixelBoxTrigger.Key.Y);
			}
		}
		_PixelBoxTriggers.Clear();
	}

	private static void LogicGatePass()
	{
		if (_GatesCurrent.Count != 0)
		{
			return;
		}
		_GatesDone.Clear();
		while (_LampsToCheck.Count > 0)
		{
			while (_LampsToCheck.Count > 0)
			{
				Point16 point = _LampsToCheck.Dequeue();
				CheckLogicGate(point.X, point.Y);
			}
			while (_GatesNext.Count > 0)
			{
				Utils.Swap(ref _GatesCurrent, ref _GatesNext);
				while (_GatesCurrent.Count > 0)
				{
					Point16 key = _GatesCurrent.Peek();
					if (_GatesDone.TryGetValue(key, out var value) && value)
					{
						_GatesCurrent.Dequeue();
						continue;
					}
					_GatesDone.Add(key, value: true);
					TripWire(key.X, key.Y, 1, 1);
					_GatesCurrent.Dequeue();
				}
			}
		}
		_GatesDone.Clear();
		if (blockPlayerTeleportationForOneIteration)
		{
			blockPlayerTeleportationForOneIteration = false;
		}
	}

	private static void CheckLogicGate(int lampX, int lampY)
	{
		//IL_0206: Unknown result type (might be due to invalid IL or missing references)
		//IL_0210: Unknown result type (might be due to invalid IL or missing references)
		//IL_021a: Unknown result type (might be due to invalid IL or missing references)
		//IL_021f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0224: Unknown result type (might be due to invalid IL or missing references)
		//IL_0226: Unknown result type (might be due to invalid IL or missing references)
		//IL_0232: Unknown result type (might be due to invalid IL or missing references)
		//IL_023a: Unknown result type (might be due to invalid IL or missing references)
		if (!WorldGen.InWorld(lampX, lampY, 1))
		{
			return;
		}
		for (int i = lampY; i < Main.maxTilesY; i++)
		{
			Tile tile = Main.tile[lampX, i];
			if (!tile.active())
			{
				break;
			}
			if (tile.type == 420)
			{
				_GatesDone.TryGetValue(new Point16(lampX, i), out var value);
				int num = tile.frameY / 18;
				bool flag = tile.frameX == 18;
				bool flag2 = tile.frameX == 36;
				if (num < 0)
				{
					break;
				}
				int num2 = 0;
				int num3 = 0;
				bool flag3 = false;
				for (int num4 = i - 1; num4 > 0; num4--)
				{
					Tile tile2 = Main.tile[lampX, num4];
					if (!tile2.active() || tile2.type != 419)
					{
						break;
					}
					if (tile2.frameX == 36)
					{
						flag3 = true;
						break;
					}
					num2++;
					num3 += (tile2.frameX == 18).ToInt();
				}
				bool flag4 = false;
				switch (num)
				{
				default:
					return;
				case 0:
					flag4 = num2 == num3;
					break;
				case 2:
					flag4 = num2 != num3;
					break;
				case 1:
					flag4 = num3 > 0;
					break;
				case 3:
					flag4 = num3 == 0;
					break;
				case 4:
					flag4 = num3 == 1;
					break;
				case 5:
					flag4 = num3 != 1;
					break;
				}
				bool flag5 = !flag3 & flag2;
				bool flag6 = false;
				if (flag3 && Framing.GetTileSafely(lampX, lampY).frameX == 36)
				{
					flag6 = true;
				}
				if (!((flag4 != flag) | flag5 | flag6))
				{
					break;
				}
				_ = tile.frameX % 18 / 18;
				tile.frameX = (short)(18 * flag4.ToInt());
				if (flag3)
				{
					tile.frameX = 36;
				}
				SkipWire(lampX, i);
				WorldGen.SquareTileFrame(lampX, i);
				NetMessage.SendTileSquare(-1, lampX, i);
				bool flag7 = !flag3 | flag6;
				if (flag6)
				{
					if (num3 == 0 || num2 == 0)
					{
						flag7 = false;
					}
					flag7 = Main.rand.Next(num2) < num3;
				}
				if (flag5)
				{
					flag7 = false;
				}
				if (flag7)
				{
					if (!value)
					{
						_GatesNext.Enqueue(new Point16(lampX, i));
						break;
					}
					Vector2 val = new Vector2((float)lampX, (float)i) * 16f - new Vector2(10f);
					Utils.PoofOfSmoke(val);
					NetMessage.SendData(106, -1, -1, null, (int)val.X, val.Y);
				}
				break;
			}
			if (tile.type != 419)
			{
				break;
			}
		}
	}

	private static void HitWire(DoubleStack<Point16> next, int wireType)
	{
		_wireDirectionList.Clear(quickClear: true);
		for (int i = 0; i < next.Count; i++)
		{
			Point16 point = next.PopFront();
			SkipWire(point);
			_toProcess.Add(point, 4);
			next.PushBack(point);
			_wireDirectionList.PushBack(0);
		}
		_currentWireColor = wireType;
		while (next.Count > 0)
		{
			Point16 key = next.PopFront();
			int num = _wireDirectionList.PopFront();
			int x = key.X;
			int y = key.Y;
			if (!_wireSkip.ContainsKey(key))
			{
				HitWireSingle(x, y);
			}
			for (int j = 0; j < 4; j++)
			{
				int num2;
				int num3;
				switch (j)
				{
				case 0:
					num2 = x;
					num3 = y + 1;
					break;
				case 1:
					num2 = x;
					num3 = y - 1;
					break;
				case 2:
					num2 = x + 1;
					num3 = y;
					break;
				case 3:
					num2 = x - 1;
					num3 = y;
					break;
				default:
					num2 = x;
					num3 = y + 1;
					break;
				}
				if (num2 < 2 || num2 >= Main.maxTilesX - 2 || num3 < 2 || num3 >= Main.maxTilesY - 2)
				{
					continue;
				}
				Tile tile = Main.tile[num2, num3];
				if (tile == null)
				{
					continue;
				}
				Tile tile2 = Main.tile[x, y];
				if (tile2 == null)
				{
					continue;
				}
				byte b = 3;
				if (tile.type == 424 || tile.type == 445)
				{
					b = 0;
				}
				if (tile2.type == 424)
				{
					switch (tile2.frameX / 18)
					{
					case 0:
						if (j != num)
						{
							continue;
						}
						break;
					case 1:
						if ((num != 0 || j != 3) && (num != 3 || j != 0) && (num != 1 || j != 2) && (num != 2 || j != 1))
						{
							continue;
						}
						break;
					case 2:
						if ((num != 0 || j != 2) && (num != 2 || j != 0) && (num != 1 || j != 3) && (num != 3 || j != 1))
						{
							continue;
						}
						break;
					}
				}
				if (tile2.type == 445)
				{
					if (j != num)
					{
						continue;
					}
					if (_PixelBoxTriggers.ContainsKey(key))
					{
						_PixelBoxTriggers[key] |= (byte)((!((j == 0) | (j == 1))) ? 1 : 2);
					}
					else
					{
						_PixelBoxTriggers[key] = (byte)((!((j == 0) | (j == 1))) ? 1u : 2u);
					}
				}
				if (!(wireType switch
				{
					1 => tile.wire(), 
					2 => tile.wire2(), 
					3 => tile.wire3(), 
					4 => tile.wire4(), 
					_ => false, 
				}))
				{
					continue;
				}
				Point16 point2 = new Point16(num2, num3);
				if (_toProcess.TryGetValue(point2, out var value))
				{
					value--;
					if (value == 0)
					{
						_toProcess.Remove(point2);
					}
					else
					{
						_toProcess[point2] = value;
					}
					continue;
				}
				next.PushBack(point2);
				_wireDirectionList.PushBack((byte)j);
				if (b > 0)
				{
					_toProcess.Add(point2, b);
				}
			}
		}
		_wireSkip.Clear();
		_toProcess.Clear();
	}

	public static IEntitySource GetProjectileSource(int sourceTileX, int sourceTileY)
	{
		return new EntitySource_Wiring(sourceTileX, sourceTileY);
	}

	public static IEntitySource GetNPCSource(int sourceTileX, int sourceTileY)
	{
		return new EntitySource_Wiring(sourceTileX, sourceTileY);
	}

	public static IEntitySource GetItemSource(int sourceTileX, int sourceTileY)
	{
		return new EntitySource_Wiring(sourceTileX, sourceTileY);
	}

	private static void HitWireSingle(int i, int j)
	{
		//IL_0514: Unknown result type (might be due to invalid IL or missing references)
		//IL_0519: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_0570: Unknown result type (might be due to invalid IL or missing references)
		//IL_060f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0658: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a7b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aed: Unknown result type (might be due to invalid IL or missing references)
		//IL_0af4: Unknown result type (might be due to invalid IL or missing references)
		//IL_155e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1563: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ca2: Unknown result type (might be due to invalid IL or missing references)
		//IL_1922: Unknown result type (might be due to invalid IL or missing references)
		//IL_1cc5: Unknown result type (might be due to invalid IL or missing references)
		//IL_1cce: Unknown result type (might be due to invalid IL or missing references)
		//IL_198d: Unknown result type (might be due to invalid IL or missing references)
		//IL_19fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a66: Unknown result type (might be due to invalid IL or missing references)
		//IL_1602: Unknown result type (might be due to invalid IL or missing references)
		//IL_16cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_16dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_16e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_16e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d9f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1da9: Unknown result type (might be due to invalid IL or missing references)
		//IL_1dae: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c15: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c31: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c3a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ddf: Unknown result type (might be due to invalid IL or missing references)
		//IL_1de8: Unknown result type (might be due to invalid IL or missing references)
		//IL_1aec: Unknown result type (might be due to invalid IL or missing references)
		//IL_1af9: Unknown result type (might be due to invalid IL or missing references)
		//IL_1afe: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b03: Unknown result type (might be due to invalid IL or missing references)
		//IL_1889: Unknown result type (might be due to invalid IL or missing references)
		//IL_258d: Unknown result type (might be due to invalid IL or missing references)
		//IL_2597: Unknown result type (might be due to invalid IL or missing references)
		//IL_259c: Unknown result type (might be due to invalid IL or missing references)
		//IL_25a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_25a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_25af: Unknown result type (might be due to invalid IL or missing references)
		//IL_25b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_2846: Unknown result type (might be due to invalid IL or missing references)
		//IL_2850: Unknown result type (might be due to invalid IL or missing references)
		//IL_2855: Unknown result type (might be due to invalid IL or missing references)
		//IL_285a: Unknown result type (might be due to invalid IL or missing references)
		//IL_285c: Unknown result type (might be due to invalid IL or missing references)
		//IL_2868: Unknown result type (might be due to invalid IL or missing references)
		//IL_2870: Unknown result type (might be due to invalid IL or missing references)
		//IL_2c1e: Unknown result type (might be due to invalid IL or missing references)
		//IL_2c28: Unknown result type (might be due to invalid IL or missing references)
		//IL_2c2d: Unknown result type (might be due to invalid IL or missing references)
		//IL_2c32: Unknown result type (might be due to invalid IL or missing references)
		//IL_2c34: Unknown result type (might be due to invalid IL or missing references)
		//IL_2c40: Unknown result type (might be due to invalid IL or missing references)
		//IL_2c48: Unknown result type (might be due to invalid IL or missing references)
		//IL_2f8d: Unknown result type (might be due to invalid IL or missing references)
		//IL_30e8: Unknown result type (might be due to invalid IL or missing references)
		Tile tile = Main.tile[i, j];
		bool? forcedStateWhereTrueIsOn = null;
		bool doSkipWires = true;
		int type = tile.type;
		if (tile.actuator())
		{
			ActuateForced(i, j);
		}
		if (!tile.active())
		{
			return;
		}
		switch (type)
		{
		case 144:
			HitSwitch(i, j);
			WorldGen.SquareTileFrame(i, j);
			NetMessage.SendTileSquare(-1, i, j);
			break;
		case 421:
			if (!tile.actuator())
			{
				tile.type = 422;
				WorldGen.SquareTileFrame(i, j);
				NetMessage.SendTileSquare(-1, i, j);
			}
			break;
		case 422:
			if (!tile.actuator())
			{
				tile.type = 421;
				WorldGen.SquareTileFrame(i, j);
				NetMessage.SendTileSquare(-1, i, j);
			}
			break;
		}
		if (type >= 255 && type <= 268)
		{
			if (!tile.actuator())
			{
				if (type >= 262)
				{
					tile.type -= 7;
				}
				else
				{
					tile.type += 7;
				}
				WorldGen.SquareTileFrame(i, j);
				NetMessage.SendTileSquare(-1, i, j);
			}
			return;
		}
		switch (type)
		{
		case 419:
		{
			int num50 = 18;
			if (tile.frameX >= num50)
			{
				num50 = -num50;
			}
			if (tile.frameX == 36)
			{
				num50 = 0;
			}
			SkipWire(i, j);
			tile.frameX = (short)(tile.frameX + num50);
			WorldGen.SquareTileFrame(i, j);
			NetMessage.SendTileSquare(-1, i, j);
			_LampsToCheck.Enqueue(new Point16(i, j));
			return;
		}
		case 406:
		{
			int num18 = tile.frameX % 54 / 18;
			int num19 = tile.frameY % 54 / 18;
			int num20 = i - num18;
			int num21 = j - num19;
			int num22 = 54;
			if (Main.tile[num20, num21].frameY >= 108)
			{
				num22 = -108;
			}
			for (int num23 = num20; num23 < num20 + 3; num23++)
			{
				for (int num24 = num21; num24 < num21 + 3; num24++)
				{
					SkipWire(num23, num24);
					Main.tile[num23, num24].frameY = (short)(Main.tile[num23, num24].frameY + num22);
				}
			}
			NetMessage.SendTileSquare(-1, num20 + 1, num21 + 1, 3);
			return;
		}
		case 452:
		{
			int num11 = tile.frameX % 54 / 18;
			int num12 = tile.frameY % 54 / 18;
			int num13 = i - num11;
			int num14 = j - num12;
			int num15 = 54;
			if (Main.tile[num13, num14].frameX >= 54)
			{
				num15 = -54;
			}
			for (int num16 = num13; num16 < num13 + 3; num16++)
			{
				for (int num17 = num14; num17 < num14 + 3; num17++)
				{
					SkipWire(num16, num17);
					Main.tile[num16, num17].frameX = (short)(Main.tile[num16, num17].frameX + num15);
				}
			}
			NetMessage.SendTileSquare(-1, num13 + 1, num14 + 1, 3);
			return;
		}
		case 411:
		{
			int num43 = tile.frameX % 36 / 18;
			int num44 = tile.frameY % 36 / 18;
			int num45 = i - num43;
			int num46 = j - num44;
			int num47 = 36;
			if (Main.tile[num45, num46].frameX >= 36)
			{
				num47 = -36;
			}
			for (int num48 = num45; num48 < num45 + 2; num48++)
			{
				for (int num49 = num46; num49 < num46 + 2; num49++)
				{
					SkipWire(num48, num49);
					Main.tile[num48, num49].frameX = (short)(Main.tile[num48, num49].frameX + num47);
				}
			}
			NetMessage.SendTileSquare(-1, num45, num46, 2, 2);
			return;
		}
		case 356:
		{
			int num = tile.frameX % 36 / 18;
			int num2 = tile.frameY % 54 / 18;
			int num3 = i - num;
			int num4 = j - num2;
			for (int k = num3; k < num3 + 2; k++)
			{
				for (int l = num4; l < num4 + 3; l++)
				{
					SkipWire(k, l);
				}
			}
			if (!Main.fastForwardTimeToDawn && Main.sundialCooldown == 0)
			{
				Main.Sundialing();
			}
			NetMessage.SendTileSquare(-1, num3, num4, 2, 2);
			return;
		}
		case 663:
		{
			int num25 = tile.frameX % 36 / 18;
			int num26 = tile.frameY % 54 / 18;
			int num27 = i - num25;
			int num28 = j - num26;
			for (int num29 = num27; num29 < num27 + 2; num29++)
			{
				for (int num30 = num28; num30 < num28 + 3; num30++)
				{
					SkipWire(num29, num30);
				}
			}
			if (!Main.fastForwardTimeToDusk && Main.moondialCooldown == 0)
			{
				Main.Moondialing();
			}
			NetMessage.SendTileSquare(-1, num27, num28, 2, 2);
			return;
		}
		case 425:
		{
			int num5 = tile.frameX % 36 / 18;
			int num6 = tile.frameY % 36 / 18;
			int num7 = i - num5;
			int num8 = j - num6;
			for (int m = num7; m < num7 + 2; m++)
			{
				for (int n = num8; n < num8 + 2; n++)
				{
					SkipWire(m, n);
				}
			}
			if (Main.AnnouncementBoxDisabled)
			{
				return;
			}
			Color pink = Color.Pink;
			int num9 = Sign.ReadSign(num7, num8, CreateIfMissing: false);
			if (num9 == -1 || Main.sign[num9] == null || string.IsNullOrWhiteSpace(Main.sign[num9].text))
			{
				return;
			}
			if (Main.AnnouncementBoxRange == -1)
			{
				if (Main.netMode == 0)
				{
					Main.NewTextMultiline(Main.sign[num9].text, force: false, pink, 460);
				}
				else if (Main.netMode == 2)
				{
					NetMessage.SendData(107, -1, -1, NetworkText.FromLiteral(Main.sign[num9].text), 255, (int)pink.R, (int)pink.G, (int)pink.B, 460);
				}
			}
			else if (Main.netMode == 0)
			{
				if (Main.player[Main.myPlayer].Distance(new Vector2((float)(num7 * 16 + 16), (float)(num8 * 16 + 16))) <= (float)Main.AnnouncementBoxRange)
				{
					Main.NewTextMultiline(Main.sign[num9].text, force: false, pink, 460);
				}
			}
			else
			{
				if (Main.netMode != 2)
				{
					return;
				}
				for (int num10 = 0; num10 < 255; num10++)
				{
					if (Main.player[num10].active && Main.player[num10].Distance(new Vector2((float)(num7 * 16 + 16), (float)(num8 * 16 + 16))) <= (float)Main.AnnouncementBoxRange)
					{
						NetMessage.SendData(107, num10, -1, NetworkText.FromLiteral(Main.sign[num9].text), 255, (int)pink.R, (int)pink.G, (int)pink.B, 460);
					}
				}
			}
			return;
		}
		case 405:
			ToggleFirePlace(i, j, tile, forcedStateWhereTrueIsOn, doSkipWires);
			return;
		case 209:
		{
			int num31 = tile.frameX % 72 / 18;
			int num32 = tile.frameY % 54 / 18;
			int num33 = i - num31;
			int num34 = j - num32;
			int num35 = tile.frameY / 54;
			int num36 = tile.frameX / 72;
			int num37 = -1;
			if (num31 == 1 || num31 == 2)
			{
				num37 = num32;
			}
			int num38 = 0;
			if (num31 == 3)
			{
				num38 = -54;
			}
			if (num31 == 0)
			{
				num38 = 54;
			}
			if (num35 >= 8 && num38 > 0)
			{
				num38 = 0;
			}
			if (num35 == 0 && num38 < 0)
			{
				num38 = 0;
			}
			bool flag = false;
			if (num38 != 0)
			{
				for (int num39 = num33; num39 < num33 + 4; num39++)
				{
					for (int num40 = num34; num40 < num34 + 3; num40++)
					{
						SkipWire(num39, num40);
						Main.tile[num39, num40].frameY = (short)(Main.tile[num39, num40].frameY + num38);
					}
				}
				flag = true;
			}
			if ((num36 == 3 || num36 == 4) && (num37 == 0 || num37 == 1))
			{
				num38 = ((num36 == 3) ? 72 : (-72));
				for (int num41 = num33; num41 < num33 + 4; num41++)
				{
					for (int num42 = num34; num42 < num34 + 3; num42++)
					{
						SkipWire(num41, num42);
						Main.tile[num41, num42].frameX = (short)(Main.tile[num41, num42].frameX + num38);
					}
				}
				flag = true;
			}
			if (flag)
			{
				NetMessage.SendTileSquare(-1, num33, num34, 4, 3);
			}
			if (num37 == -1)
			{
				return;
			}
			bool flag2 = true;
			if ((num36 == 3 || num36 == 4) && num37 < 2)
			{
				flag2 = false;
			}
			int damage = 0;
			float knockBack = 0f;
			int time = 30;
			switch (num36)
			{
			case 0:
				if (cannonCoolDown > 0)
				{
					return;
				}
				damage = 300;
				knockBack = 8f;
				time = 480;
				break;
			case 1:
				if (bunnyCannonCoolDown > 0)
				{
					return;
				}
				damage = 350;
				knockBack = 8f;
				time = 3600;
				break;
			}
			if (CheckMech(num33, num34, time) & flag2)
			{
				switch (num36)
				{
				case 0:
					cannonCoolDown = 120;
					break;
				case 1:
					bunnyCannonCoolDown = 480;
					break;
				}
				WorldGen.ShootFromCannon(num33, num34, num35, num36 + 1, damage, knockBack, CurrentUser, fromWire: true);
			}
			return;
		}
		case 212:
		{
			int num51 = tile.frameX % 54 / 18;
			int num52 = tile.frameY % 54 / 18;
			int num53 = i - num51;
			int num54 = j - num52;
			int num55 = tile.frameX / 54;
			int num56 = -1;
			if (num51 == 1)
			{
				num56 = num52;
			}
			int num57 = 0;
			if (num51 == 0)
			{
				num57 = -54;
			}
			if (num51 == 2)
			{
				num57 = 54;
			}
			if (num55 >= 1 && num57 > 0)
			{
				num57 = 0;
			}
			if (num55 == 0 && num57 < 0)
			{
				num57 = 0;
			}
			bool flag3 = false;
			if (num57 != 0)
			{
				for (int num58 = num53; num58 < num53 + 3; num58++)
				{
					for (int num59 = num54; num59 < num54 + 3; num59++)
					{
						SkipWire(num58, num59);
						Main.tile[num58, num59].frameX = (short)(Main.tile[num58, num59].frameX + num57);
					}
				}
				flag3 = true;
			}
			if (flag3)
			{
				NetMessage.SendTileSquare(-1, num53, num54, 3, 3);
			}
			if (num56 != -1 && snowballCannonCoolDown == 0 && CheckMech(num53, num54, 60))
			{
				snowballCannonCoolDown = 15;
				float num60 = 12f + (float)Main.rand.Next(450) * 0.01f;
				float num61 = Main.rand.Next(85, 105);
				float num62 = Main.rand.Next(-35, 11);
				int type2 = 166;
				int damage2 = 35;
				float knockBack2 = 3.5f;
				Vector2 val = new Vector2((float)((num53 + 2) * 16 - 8), (float)((num54 + 2) * 16 - 8));
				if (tile.frameX / 54 == 0)
				{
					num61 *= -1f;
					val.X -= 12f;
				}
				else
				{
					val.X += 12f;
				}
				float num63 = num61;
				float num64 = num62;
				float num65 = (float)Math.Sqrt(num63 * num63 + num64 * num64);
				num65 = num60 / num65;
				num63 *= num65;
				num64 *= num65;
				Projectile.NewProjectile(GetProjectileSource(num53, num54), val.X, val.Y, num63, num64, type2, damage2, knockBack2, Main.myPlayer);
			}
			return;
		}
		}
		if (TileID.Sets.Campfires[type])
		{
			ToggleCampFire(i, j, tile, forcedStateWhereTrueIsOn, doSkipWires);
			return;
		}
		if (type == 130)
		{
			if (Main.tile[i, j - 1] != null && (!Main.tile[i, j - 1].active() || !TileID.Sets.PreventsActuationUnder[Main.tile[i, j - 1].type]) && WorldGen.CanKillTile(i, j))
			{
				tile.type = 131;
				WorldGen.SquareTileFrame(i, j);
				NetMessage.SendTileSquare(-1, i, j);
			}
			return;
		}
		if (type == 131)
		{
			tile.type = 130;
			WorldGen.SquareTileFrame(i, j);
			NetMessage.SendTileSquare(-1, i, j);
			return;
		}
		if (type == 387 || type == 386)
		{
			bool value = type == 387;
			int num66 = WorldGen.ShiftTrapdoor(i, j, playerAbove: true).ToInt();
			if (num66 == 0)
			{
				num66 = -WorldGen.ShiftTrapdoor(i, j, playerAbove: false).ToInt();
			}
			if (num66 != 0)
			{
				NetMessage.SendData(19, -1, -1, null, 3 - value.ToInt(), i, j, num66);
			}
			return;
		}
		if (type == 389 || type == 388)
		{
			bool flag4 = type == 389;
			WorldGen.ShiftTallGate(i, j, flag4);
			NetMessage.SendData(19, -1, -1, null, 4 + flag4.ToInt(), i, j);
			return;
		}
		if (type == 11)
		{
			if (WorldGen.CloseDoor(i, j, forced: true))
			{
				NetMessage.SendData(19, -1, -1, null, 1, i, j);
			}
			return;
		}
		if (type == 10)
		{
			int num67 = 1;
			if (Main.rand.Next(2) == 0)
			{
				num67 = -1;
			}
			if (!WorldGen.OpenDoor(i, j, num67))
			{
				if (WorldGen.OpenDoor(i, j, -num67))
				{
					NetMessage.SendData(19, -1, -1, null, 0, i, j, -num67);
				}
			}
			else
			{
				NetMessage.SendData(19, -1, -1, null, 0, i, j, num67);
			}
			return;
		}
		if (type == 216)
		{
			WorldGen.LaunchRocket(i, j, fromWiring: true);
			SkipWire(i, j);
			return;
		}
		if (type == 497 || (type == 15 && tile.frameY / 40 == 1) || (type == 15 && tile.frameY / 40 == 20))
		{
			int num68 = j - tile.frameY % 40 / 18;
			SkipWire(i, num68);
			SkipWire(i, num68 + 1);
			if (CheckMech(i, num68, 60))
			{
				Projectile.NewProjectile(GetProjectileSource(i, num68), i * 16 + 8, num68 * 16 + 12, 0f, 0f, 733, 0, 0f, Main.myPlayer);
			}
			return;
		}
		switch (type)
		{
		case 335:
		{
			int num72 = j - tile.frameY / 18;
			int num73 = i - tile.frameX / 18;
			SkipWire(num73, num72);
			SkipWire(num73, num72 + 1);
			SkipWire(num73 + 1, num72);
			SkipWire(num73 + 1, num72 + 1);
			if (CheckMech(num73, num72, 30))
			{
				WorldGen.LaunchRocketSmall(num73, num72, fromWiring: true);
			}
			return;
		}
		case 338:
		{
			int num69 = j - tile.frameY / 18;
			int num70 = i - tile.frameX / 18;
			SkipWire(num70, num69);
			SkipWire(num70, num69 + 1);
			if (!CheckMech(num70, num69, 30))
			{
				return;
			}
			bool flag5 = false;
			for (int num71 = 0; num71 < 1000; num71++)
			{
				if (Main.projectile[num71].active && Main.projectile[num71].aiStyle == 73 && Main.projectile[num71].ai[0] == (float)num70 && Main.projectile[num71].ai[1] == (float)num69)
				{
					flag5 = true;
					break;
				}
			}
			if (!flag5)
			{
				int type3 = 419 + Main.rand.Next(4);
				Projectile.NewProjectile(GetProjectileSource(num70, num69), num70 * 16 + 8, num69 * 16 + 2, 0f, 0f, type3, 0, 0f, Main.myPlayer, num70, num69);
			}
			return;
		}
		case 235:
		{
			int num74 = i - tile.frameX / 18;
			if (tile.wall == 87 && (double)j > Main.worldSurface && !NPC.downedPlantBoss)
			{
				return;
			}
			if (_teleport[0].X == -1f)
			{
				_teleport[0].X = num74;
				_teleport[0].Y = j;
				if (tile.halfBrick())
				{
					_teleport[0].Y += 0.5f;
				}
			}
			else if (_teleport[0].X != (float)num74 || _teleport[0].Y != (float)j)
			{
				_teleport[1].X = num74;
				_teleport[1].Y = j;
				if (tile.halfBrick())
				{
					_teleport[1].Y += 0.5f;
				}
			}
			return;
		}
		}
		if (TileID.Sets.Torches[type])
		{
			ToggleTorch(i, j, tile, forcedStateWhereTrueIsOn);
			return;
		}
		switch (type)
		{
		case 429:
		{
			int num156 = Main.tile[i, j].frameX / 18;
			bool flag8 = num156 % 2 >= 1;
			bool flag9 = num156 % 4 >= 2;
			bool flag10 = num156 % 8 >= 4;
			bool flag11 = num156 % 16 >= 8;
			bool flag12 = false;
			short num157 = 0;
			switch (_currentWireColor)
			{
			case 1:
				num157 = 18;
				flag12 = !flag8;
				break;
			case 2:
				num157 = 72;
				flag12 = !flag10;
				break;
			case 3:
				num157 = 36;
				flag12 = !flag9;
				break;
			case 4:
				num157 = 144;
				flag12 = !flag11;
				break;
			}
			if (flag12)
			{
				tile.frameX += num157;
			}
			else
			{
				tile.frameX -= num157;
			}
			NetMessage.SendTileSquare(-1, i, j);
			break;
		}
		case 149:
			ToggleHolidayLight(i, j, tile, forcedStateWhereTrueIsOn);
			break;
		case 244:
		{
			int num81;
			for (num81 = tile.frameX / 18; num81 >= 3; num81 -= 3)
			{
			}
			int num82;
			for (num82 = tile.frameY / 18; num82 >= 3; num82 -= 3)
			{
			}
			int num83 = i - num81;
			int num84 = j - num82;
			int num85 = 54;
			if (Main.tile[num83, num84].frameX >= 54)
			{
				num85 = -54;
			}
			for (int num86 = num83; num86 < num83 + 3; num86++)
			{
				for (int num87 = num84; num87 < num84 + 2; num87++)
				{
					SkipWire(num86, num87);
					Main.tile[num86, num87].frameX = (short)(Main.tile[num86, num87].frameX + num85);
				}
			}
			NetMessage.SendTileSquare(-1, num83, num84, 3, 2);
			break;
		}
		case 565:
		{
			int num125;
			for (num125 = tile.frameX / 18; num125 >= 2; num125 -= 2)
			{
			}
			int num126;
			for (num126 = tile.frameY / 18; num126 >= 2; num126 -= 2)
			{
			}
			int num127 = i - num125;
			int num128 = j - num126;
			int num129 = 36;
			if (Main.tile[num127, num128].frameX >= 36)
			{
				num129 = -36;
			}
			for (int num130 = num127; num130 < num127 + 2; num130++)
			{
				for (int num131 = num128; num131 < num128 + 2; num131++)
				{
					SkipWire(num130, num131);
					Main.tile[num130, num131].frameX = (short)(Main.tile[num130, num131].frameX + num129);
				}
			}
			NetMessage.SendTileSquare(-1, num127, num128, 2, 2);
			break;
		}
		case 42:
			ToggleHangingLantern(i, j, tile, forcedStateWhereTrueIsOn, doSkipWires);
			break;
		case 93:
			ToggleLamp(i, j, tile, forcedStateWhereTrueIsOn, doSkipWires);
			break;
		case 95:
		case 100:
		case 126:
		case 173:
		case 564:
			Toggle2x2Light(i, j, tile, forcedStateWhereTrueIsOn, doSkipWires);
			break;
		case 593:
		{
			SkipWire(i, j);
			short num93 = (short)((Main.tile[i, j].frameX != 0) ? (-18) : 18);
			Main.tile[i, j].frameX += num93;
			if (Main.netMode == 2)
			{
				NetMessage.SendTileSquare(-1, i, j, 1, 1);
			}
			int num94 = ((num93 > 0) ? 4 : 3);
			Animation.NewTemporaryAnimation(num94, 593, i, j);
			NetMessage.SendTemporaryAnimation(-1, num94, 593, i, j);
			break;
		}
		case 594:
		{
			int num132;
			for (num132 = tile.frameY / 18; num132 >= 2; num132 -= 2)
			{
			}
			num132 = j - num132;
			int num133 = tile.frameX / 18;
			if (num133 > 1)
			{
				num133 -= 2;
			}
			num133 = i - num133;
			SkipWire(num133, num132);
			SkipWire(num133, num132 + 1);
			SkipWire(num133 + 1, num132);
			SkipWire(num133 + 1, num132 + 1);
			short num134 = (short)((Main.tile[num133, num132].frameX != 0) ? (-36) : 36);
			for (int num135 = 0; num135 < 2; num135++)
			{
				for (int num136 = 0; num136 < 2; num136++)
				{
					Main.tile[num133 + num135, num132 + num136].frameX += num134;
				}
			}
			if (Main.netMode == 2)
			{
				NetMessage.SendTileSquare(-1, num133, num132, 2, 2);
			}
			int num137 = ((num134 > 0) ? 4 : 3);
			Animation.NewTemporaryAnimation(num137, 594, num133, num132);
			NetMessage.SendTemporaryAnimation(-1, num137, 594, num133, num132);
			break;
		}
		case 34:
			ToggleChandelier(i, j, tile, forcedStateWhereTrueIsOn, doSkipWires);
			break;
		case 314:
			if (CheckMech(i, j, 5))
			{
				Minecart.FlipSwitchTrack(i, j);
			}
			break;
		case 33:
		case 49:
		case 174:
		case 372:
		case 646:
			ToggleCandle(i, j, tile, forcedStateWhereTrueIsOn);
			break;
		case 92:
			ToggleLampPost(i, j, tile, forcedStateWhereTrueIsOn, doSkipWires);
			break;
		case 137:
		{
			int num95 = tile.frameY / 18;
			Vector2 val3 = Vector2.Zero;
			float speedX = 0f;
			float speedY = 0f;
			int num96 = 0;
			int damage4 = 0;
			switch (num95)
			{
			case 0:
			case 1:
			case 2:
			case 5:
				if (CheckMech(i, j, 200))
				{
					int num104 = ((tile.frameX == 0) ? (-1) : ((tile.frameX == 18) ? 1 : 0));
					int num105 = ((tile.frameX >= 36) ? ((tile.frameX >= 72) ? 1 : (-1)) : 0);
					val3 = new Vector2((float)(i * 16 + 8 + 10 * num104), (float)(j * 16 + 8 + 10 * num105));
					float num106 = 3f;
					if (num95 == 0)
					{
						num96 = 98;
						damage4 = 20;
						num106 = 12f;
					}
					if (num95 == 1)
					{
						num96 = 184;
						damage4 = 40;
						num106 = 12f;
					}
					if (num95 == 2)
					{
						num96 = 187;
						damage4 = 40;
						num106 = 5f;
					}
					if (num95 == 5)
					{
						num96 = 980;
						damage4 = 30;
						num106 = 12f;
					}
					speedX = (float)num104 * num106;
					speedY = (float)num105 * num106;
				}
				break;
			case 3:
			{
				if (!CheckMech(i, j, 300))
				{
					break;
				}
				int num99 = 200;
				for (int num100 = 0; num100 < 1000; num100++)
				{
					if (Main.projectile[num100].active && Main.projectile[num100].type == num96)
					{
						Vector2 val4 = new Vector2((float)(i * 16 + 8), (float)(j * 18 + 8)) - Main.projectile[num100].Center;
						float num101 = val4.Length();
						if (num101 < 50f)
						{
							num99 -= 50;
						}
						else if (num101 < 100f)
						{
							num99 -= 15;
						}
						else if (num101 < 200f)
						{
							num99 -= 10;
						}
						else if (num101 < 300f)
						{
							num99 -= 8;
						}
						else if (num101 < 400f)
						{
							num99 -= 6;
						}
						else if (num101 < 500f)
						{
							num99 -= 5;
						}
						else if (num101 < 700f)
						{
							num99 -= 4;
						}
						else if (num101 < 900f)
						{
							num99 -= 3;
						}
						else
						{
							num99 = ((!(num101 < 1200f)) ? (num99 - 1) : (num99 - 2));
						}
					}
				}
				if (num99 > 0)
				{
					num96 = 185;
					damage4 = 40;
					int num102 = 0;
					int num103 = 0;
					switch (tile.frameX / 18)
					{
					case 0:
					case 1:
						num102 = 0;
						num103 = 1;
						break;
					case 2:
						num102 = 0;
						num103 = -1;
						break;
					case 3:
						num102 = -1;
						num103 = 0;
						break;
					case 4:
						num102 = 1;
						num103 = 0;
						break;
					}
					speedX = (float)(4 * num102) + (float)Main.rand.Next(-20 + ((num102 == 1) ? 20 : 0), 21 - ((num102 == -1) ? 20 : 0)) * 0.05f;
					speedY = (float)(4 * num103) + (float)Main.rand.Next(-20 + ((num103 == 1) ? 20 : 0), 21 - ((num103 == -1) ? 20 : 0)) * 0.05f;
					val3 = new Vector2((float)(i * 16 + 8 + 14 * num102), (float)(j * 16 + 8 + 14 * num103));
				}
				break;
			}
			case 4:
				if (CheckMech(i, j, 90))
				{
					int num97 = 0;
					int num98 = 0;
					switch (tile.frameX / 18)
					{
					case 0:
					case 1:
						num97 = 0;
						num98 = 1;
						break;
					case 2:
						num97 = 0;
						num98 = -1;
						break;
					case 3:
						num97 = -1;
						num98 = 0;
						break;
					case 4:
						num97 = 1;
						num98 = 0;
						break;
					}
					speedX = 8 * num97;
					speedY = 8 * num98;
					damage4 = 60;
					num96 = 186;
					val3 = new Vector2((float)(i * 16 + 8 + 18 * num97), (float)(j * 16 + 8 + 18 * num98));
				}
				break;
			}
			switch (num95)
			{
			case -10:
				if (CheckMech(i, j, 200))
				{
					int num111 = -1;
					if (tile.frameX != 0)
					{
						num111 = 1;
					}
					speedX = 12 * num111;
					damage4 = 20;
					num96 = 98;
					val3 = new Vector2((float)(i * 16 + 8), (float)(j * 16 + 7));
					val3.X += 10 * num111;
					val3.Y += 2f;
				}
				break;
			case -9:
				if (CheckMech(i, j, 200))
				{
					int num107 = -1;
					if (tile.frameX != 0)
					{
						num107 = 1;
					}
					speedX = 12 * num107;
					damage4 = 40;
					num96 = 184;
					val3 = new Vector2((float)(i * 16 + 8), (float)(j * 16 + 7));
					val3.X += 10 * num107;
					val3.Y += 2f;
				}
				break;
			case -8:
				if (CheckMech(i, j, 200))
				{
					int num112 = -1;
					if (tile.frameX != 0)
					{
						num112 = 1;
					}
					speedX = 5 * num112;
					damage4 = 40;
					num96 = 187;
					val3 = new Vector2((float)(i * 16 + 8), (float)(j * 16 + 7));
					val3.X += 10 * num112;
					val3.Y += 2f;
				}
				break;
			case -7:
			{
				if (!CheckMech(i, j, 300))
				{
					break;
				}
				num96 = 185;
				int num108 = 200;
				for (int num109 = 0; num109 < 1000; num109++)
				{
					if (Main.projectile[num109].active && Main.projectile[num109].type == num96)
					{
						Vector2 val5 = new Vector2((float)(i * 16 + 8), (float)(j * 18 + 8)) - Main.projectile[num109].Center;
						float num110 = val5.Length();
						if (num110 < 50f)
						{
							num108 -= 50;
						}
						else if (num110 < 100f)
						{
							num108 -= 15;
						}
						else if (num110 < 200f)
						{
							num108 -= 10;
						}
						else if (num110 < 300f)
						{
							num108 -= 8;
						}
						else if (num110 < 400f)
						{
							num108 -= 6;
						}
						else if (num110 < 500f)
						{
							num108 -= 5;
						}
						else if (num110 < 700f)
						{
							num108 -= 4;
						}
						else if (num110 < 900f)
						{
							num108 -= 3;
						}
						else
						{
							num108 = ((!(num110 < 1200f)) ? (num108 - 1) : (num108 - 2));
						}
					}
				}
				if (num108 > 0)
				{
					speedX = (float)Main.rand.Next(-20, 21) * 0.05f;
					speedY = 4f + (float)Main.rand.Next(0, 21) * 0.05f;
					damage4 = 40;
					val3 = new Vector2((float)(i * 16 + 8), (float)(j * 16 + 16));
					val3.Y += 6f;
					Projectile.NewProjectile(GetProjectileSource(i, j), (int)val3.X, (int)val3.Y, speedX, speedY, num96, damage4, 2f, Main.myPlayer);
				}
				break;
			}
			case -6:
				if (CheckMech(i, j, 90))
				{
					speedX = 0f;
					speedY = 8f;
					damage4 = 60;
					num96 = 186;
					val3 = new Vector2((float)(i * 16 + 8), (float)(j * 16 + 16));
					val3.Y += 10f;
				}
				break;
			}
			if (num96 != 0)
			{
				Projectile.NewProjectile(GetProjectileSource(i, j), (int)val3.X, (int)val3.Y, speedX, speedY, num96, damage4, 2f, Main.myPlayer);
			}
			break;
		}
		case 443:
			GeyserTrap(i, j);
			break;
		case 21:
		case 467:
			Hopper(i, j);
			break;
		case 219:
		case 642:
			Extractinator(i, j);
			break;
		case 531:
		{
			int num88 = tile.frameX / 36;
			int num89 = tile.frameY / 54;
			int num90 = i - (tile.frameX - num88 * 36) / 18;
			int num91 = j - (tile.frameY - num89 * 54) / 18;
			if (CheckMech(num90, num91, 900))
			{
				Vector2 val2 = new Vector2((float)(num90 + 1), (float)num91) * 16f;
				val2.Y += 28f;
				int num92 = 99;
				int damage3 = 70;
				float knockBack3 = 10f;
				if (num92 != 0)
				{
					Projectile.NewProjectile(GetProjectileSource(num90, num91), (int)val2.X, (int)val2.Y, 0f, 0f, num92, damage3, knockBack3, Main.myPlayer);
				}
			}
			break;
		}
		case 35:
		case 139:
			WorldGen.SwitchMB(i, j);
			break;
		case 207:
			WorldGen.SwitchFountain(i, j);
			break;
		case 410:
		case 480:
		case 509:
		case 657:
		case 658:
		case 720:
		case 721:
		case 725:
		case 733:
			WorldGen.SwitchMonolith(i, j);
			break;
		case 455:
			BirthdayParty.ToggleManualParty();
			break;
		case 141:
			WorldGen.KillTile(i, j, fail: false, effectOnly: false, noItem: true);
			NetMessage.SendTileSquare(-1, i, j);
			Projectile.NewProjectile(GetProjectileSource(i, j), i * 16 + 8, j * 16 + 8, 0f, 0f, 108, 500, 10f, Main.myPlayer);
			break;
		case 210:
			ExplodeMine(i, j);
			break;
		case 142:
		case 143:
		{
			int num119 = j - tile.frameY / 18;
			int num120 = tile.frameX / 18;
			if (num120 > 1)
			{
				num120 -= 2;
			}
			num120 = i - num120;
			SkipWire(num120, num119);
			SkipWire(num120, num119 + 1);
			SkipWire(num120 + 1, num119);
			SkipWire(num120 + 1, num119 + 1);
			if (type == 142)
			{
				for (int num121 = 0; num121 < 4; num121++)
				{
					if (_numInPump >= 19)
					{
						break;
					}
					int num122;
					int num123;
					switch (num121)
					{
					case 0:
						num122 = num120;
						num123 = num119 + 1;
						break;
					case 1:
						num122 = num120 + 1;
						num123 = num119 + 1;
						break;
					case 2:
						num122 = num120;
						num123 = num119;
						break;
					default:
						num122 = num120 + 1;
						num123 = num119;
						break;
					}
					_inPumpX[_numInPump] = num122;
					_inPumpY[_numInPump] = num123;
					_numInPump++;
				}
				break;
			}
			for (int num124 = 0; num124 < 4; num124++)
			{
				if (_numOutPump >= 19)
				{
					break;
				}
				int num122;
				int num123;
				switch (num124)
				{
				case 0:
					num122 = num120;
					num123 = num119 + 1;
					break;
				case 1:
					num122 = num120 + 1;
					num123 = num119 + 1;
					break;
				case 2:
					num122 = num120;
					num123 = num119;
					break;
				default:
					num122 = num120 + 1;
					num123 = num119;
					break;
				}
				_outPumpX[_numOutPump] = num122;
				_outPumpY[_numOutPump] = num123;
				_numOutPump++;
			}
			break;
		}
		case 105:
		{
			int num138 = j - tile.frameY / 18;
			int num139 = tile.frameX / 18;
			int num140 = 0;
			while (num139 >= 2)
			{
				num139 -= 2;
				num140++;
			}
			num139 = i - num139;
			num139 = i - tile.frameX % 36 / 18;
			num138 = j - tile.frameY % 54 / 18;
			int num141 = tile.frameY / 54;
			num141 %= 3;
			num140 = tile.frameX / 36 + num141 * 55;
			SkipWire(num139, num138);
			SkipWire(num139, num138 + 1);
			SkipWire(num139, num138 + 2);
			SkipWire(num139 + 1, num138);
			SkipWire(num139 + 1, num138 + 1);
			SkipWire(num139 + 1, num138 + 2);
			int num142 = num139 * 16 + 16;
			int num143 = (num138 + 3) * 16;
			int num144 = -1;
			int num145 = -1;
			bool flag6 = true;
			bool flag7 = false;
			switch (num140)
			{
			case 5:
				num145 = 73;
				break;
			case 13:
				num145 = 24;
				break;
			case 30:
				num145 = 6;
				break;
			case 35:
				num145 = 2;
				break;
			case 51:
				num145 = Utils.SelectRandom(Main.rand, new short[2] { 299, 538 });
				break;
			case 52:
				num145 = 356;
				break;
			case 53:
				num145 = 357;
				break;
			case 54:
				num145 = Utils.SelectRandom(Main.rand, new short[2] { 355, 358 });
				break;
			case 55:
				num145 = Utils.SelectRandom(Main.rand, new short[2] { 367, 366 });
				break;
			case 56:
				num145 = Utils.SelectRandom(Main.rand, new short[5] { 359, 359, 359, 359, 360 });
				break;
			case 57:
				num145 = 377;
				break;
			case 58:
				num145 = 300;
				break;
			case 59:
				num145 = Utils.SelectRandom(Main.rand, new short[2] { 364, 362 });
				break;
			case 60:
				num145 = 148;
				break;
			case 61:
				num145 = 361;
				break;
			case 62:
				num145 = Utils.SelectRandom(Main.rand, new short[3] { 487, 486, 485 });
				break;
			case 63:
				num145 = 164;
				flag6 &= NPC.MechSpawn(num142, num143, 165);
				break;
			case 64:
				num145 = 86;
				flag7 = true;
				break;
			case 65:
				num145 = 490;
				break;
			case 66:
				num145 = 82;
				break;
			case 67:
				num145 = 449;
				break;
			case 68:
				num145 = 167;
				break;
			case 69:
				num145 = 480;
				break;
			case 70:
				num145 = 48;
				break;
			case 71:
				num145 = Utils.SelectRandom(Main.rand, new short[3] { 170, 180, 171 });
				flag7 = true;
				break;
			case 72:
				num145 = 481;
				break;
			case 73:
				num145 = 482;
				break;
			case 74:
				num145 = 430;
				break;
			case 75:
				num145 = 489;
				break;
			case 76:
				num145 = 611;
				break;
			case 77:
				num145 = 602;
				break;
			case 78:
				num145 = Utils.SelectRandom(Main.rand, new short[6] { 595, 596, 599, 597, 600, 598 });
				break;
			case 79:
				num145 = Utils.SelectRandom(Main.rand, new short[2] { 616, 617 });
				break;
			case 80:
				num145 = Utils.SelectRandom(Main.rand, new short[2] { 671, 672 });
				break;
			case 81:
				num145 = 673;
				break;
			case 82:
				num145 = Utils.SelectRandom(Main.rand, new short[2] { 674, 675 });
				break;
			}
			if ((num145 != -1 && CheckMech(num139, num138, 30) && NPC.MechSpawn(num142, num143, num145)) & flag6)
			{
				if (!flag7 || !Collision.SolidTiles(num139 - 2, num139 + 3, num138, num138 + 2))
				{
					num144 = NPC.NewNPC(GetNPCSource(num139, num138), num142, num143, num145);
				}
				else
				{
					Vector2 val6 = new Vector2((float)(num142 - 4), (float)(num143 - 22)) - new Vector2(10f);
					Utils.PoofOfSmoke(val6);
					NetMessage.SendData(106, -1, -1, null, (int)val6.X, val6.Y);
				}
			}
			if (num144 <= -1)
			{
				switch (num140)
				{
				case 4:
					if (CheckMech(num139, num138, 30) && NPC.MechSpawn(num142, num143, 1))
					{
						num144 = NPC.NewNPC(GetNPCSource(num139, num138), num142, num143 - 12, 1);
					}
					break;
				case 7:
					if (CheckMech(num139, num138, 30) && NPC.MechSpawn(num142, num143, 49))
					{
						num144 = NPC.NewNPC(GetNPCSource(num139, num138), num142 - 4, num143 - 6, 49);
					}
					break;
				case 8:
					if (CheckMech(num139, num138, 30) && NPC.MechSpawn(num142, num143, 55))
					{
						num144 = NPC.NewNPC(GetNPCSource(num139, num138), num142, num143 - 12, 55);
					}
					break;
				case 9:
				{
					int type4 = 46;
					if (BirthdayParty.PartyIsUp)
					{
						type4 = 540;
					}
					if (CheckMech(num139, num138, 30) && NPC.MechSpawn(num142, num143, type4))
					{
						num144 = NPC.NewNPC(GetNPCSource(num139, num138), num142, num143 - 12, type4);
					}
					break;
				}
				case 10:
					if (CheckMech(num139, num138, 30) && NPC.MechSpawn(num142, num143, 21))
					{
						num144 = NPC.NewNPC(GetNPCSource(num139, num138), num142, num143, 21);
					}
					break;
				case 16:
					if (CheckMech(num139, num138, 30) && NPC.MechSpawn(num142, num143, 42))
					{
						if (!Collision.SolidTiles(num139 - 1, num139 + 1, num138, num138 + 1))
						{
							num144 = NPC.NewNPC(GetNPCSource(num139, num138), num142, num143 - 12, 42);
							break;
						}
						Vector2 val8 = new Vector2((float)(num142 - 4), (float)(num143 - 22)) - new Vector2(10f);
						Utils.PoofOfSmoke(val8);
						NetMessage.SendData(106, -1, -1, null, (int)val8.X, val8.Y);
					}
					break;
				case 18:
					if (CheckMech(num139, num138, 30) && NPC.MechSpawn(num142, num143, 67))
					{
						num144 = NPC.NewNPC(GetNPCSource(num139, num138), num142, num143 - 12, 67);
					}
					break;
				case 23:
					if (CheckMech(num139, num138, 30) && NPC.MechSpawn(num142, num143, 63))
					{
						num144 = NPC.NewNPC(GetNPCSource(num139, num138), num142, num143 - 12, 63);
					}
					break;
				case 27:
					if (CheckMech(num139, num138, 30) && NPC.MechSpawn(num142, num143, 85))
					{
						num144 = NPC.NewNPC(GetNPCSource(num139, num138), num142 - 9, num143, 85);
					}
					break;
				case 28:
					if (CheckMech(num139, num138, 30) && NPC.MechSpawn(num142, num143, 74))
					{
						num144 = NPC.NewNPC(GetNPCSource(num139, num138), num142, num143 - 12, Utils.SelectRandom(Main.rand, new short[3] { 74, 297, 298 }));
					}
					break;
				case 34:
				{
					for (int num154 = 0; num154 < 2; num154++)
					{
						for (int num155 = 0; num155 < 3; num155++)
						{
							Tile tile2 = Main.tile[num139 + num154, num138 + num155];
							tile2.type = 349;
							tile2.frameX = (short)(num154 * 18 + 216);
							tile2.frameY = (short)(num155 * 18);
						}
					}
					Animation.NewTemporaryAnimation(0, 349, num139, num138);
					if (Main.netMode == 2)
					{
						NetMessage.SendTileSquare(-1, num139, num138, 2, 3);
					}
					break;
				}
				case 42:
					if (CheckMech(num139, num138, 30) && NPC.MechSpawn(num142, num143, 58))
					{
						num144 = NPC.NewNPC(GetNPCSource(num139, num138), num142, num143 - 12, 58);
					}
					break;
				case 37:
					if (CheckMech(num139, num138, 600) && Item.MechSpawn(num142, num143, 58) && Item.MechSpawn(num142, num143, 1734) && Item.MechSpawn(num142, num143, 1867))
					{
						Item.NewItem(GetItemSource(num142, num143), num142, num143 - 16, 0, 0, 58);
					}
					break;
				case 50:
					if (CheckMech(num139, num138, 30) && NPC.MechSpawn(num142, num143, 65))
					{
						if (!Collision.SolidTiles(num139 - 2, num139 + 3, num138, num138 + 2))
						{
							num144 = NPC.NewNPC(GetNPCSource(num139, num138), num142, num143 - 12, 65);
							break;
						}
						Vector2 val7 = new Vector2((float)(num142 - 4), (float)(num143 - 22)) - new Vector2(10f);
						Utils.PoofOfSmoke(val7);
						NetMessage.SendData(106, -1, -1, null, (int)val7.X, val7.Y);
					}
					break;
				case 2:
					if (CheckMech(num139, num138, 600) && Item.MechSpawn(num142, num143, 184) && Item.MechSpawn(num142, num143, 1735) && Item.MechSpawn(num142, num143, 1868))
					{
						Item.NewItem(GetItemSource(num142, num143), num142, num143 - 16, 0, 0, 184);
					}
					break;
				case 17:
					if (CheckMech(num139, num138, 600) && Item.MechSpawn(num142, num143, 166))
					{
						Item.NewItem(GetItemSource(num142, num143), num142, num143 - 20, 0, 0, 166);
					}
					break;
				case 40:
				{
					if (!CheckMech(num139, num138, 300))
					{
						break;
					}
					int num150 = 50;
					int[] array2 = new int[num150];
					int num151 = 0;
					for (int num152 = 0; num152 < Main.maxNPCs; num152++)
					{
						if (Main.npc[num152].active && (Main.npc[num152].type == 17 || Main.npc[num152].type == 19 || Main.npc[num152].type == 22 || Main.npc[num152].type == 38 || Main.npc[num152].type == 54 || Main.npc[num152].type == 107 || Main.npc[num152].type == 108 || Main.npc[num152].type == 142 || Main.npc[num152].type == 160 || Main.npc[num152].type == 207 || Main.npc[num152].type == 209 || Main.npc[num152].type == 227 || Main.npc[num152].type == 228 || Main.npc[num152].type == 229 || Main.npc[num152].type == 368 || Main.npc[num152].type == 369 || Main.npc[num152].type == 550 || Main.npc[num152].type == 441 || Main.npc[num152].type == 588))
						{
							array2[num151] = num152;
							num151++;
							if (num151 >= num150)
							{
								break;
							}
						}
					}
					if (num151 > 0)
					{
						int num153 = array2[Main.rand.Next(num151)];
						Main.npc[num153].Teleport(new Vector2((float)(num142 - Main.npc[num153].width / 2), (float)(num143 - Main.npc[num153].height - 1)), 14);
					}
					break;
				}
				case 41:
				{
					if (!CheckMech(num139, num138, 300))
					{
						break;
					}
					int num146 = 50;
					int[] array = new int[num146];
					int num147 = 0;
					for (int num148 = 0; num148 < Main.maxNPCs; num148++)
					{
						if (Main.npc[num148].active && (Main.npc[num148].type == 18 || Main.npc[num148].type == 20 || Main.npc[num148].type == 124 || Main.npc[num148].type == 178 || Main.npc[num148].type == 208 || Main.npc[num148].type == 353 || Main.npc[num148].type == 633 || Main.npc[num148].type == 663))
						{
							array[num147] = num148;
							num147++;
							if (num147 >= num146)
							{
								break;
							}
						}
					}
					if (num147 > 0)
					{
						int num149 = array[Main.rand.Next(num147)];
						Main.npc[num149].Teleport(new Vector2((float)(num142 - Main.npc[num149].width / 2), (float)(num143 - Main.npc[num149].height - 1)), 14);
					}
					break;
				}
				}
			}
			if (num144 >= 0)
			{
				Main.npc[num144].value = 0f;
				Main.npc[num144].npcSlots = 0f;
				Main.npc[num144].SpawnedFromStatue = true;
				Main.npc[num144].CanBeReplacedByOtherNPCs = true;
			}
			break;
		}
		case 349:
		{
			int num113 = tile.frameY / 18;
			num113 %= 3;
			int num114 = j - num113;
			int num115;
			for (num115 = tile.frameX / 18; num115 >= 2; num115 -= 2)
			{
			}
			num115 = i - num115;
			SkipWire(num115, num114);
			SkipWire(num115, num114 + 1);
			SkipWire(num115, num114 + 2);
			SkipWire(num115 + 1, num114);
			SkipWire(num115 + 1, num114 + 1);
			SkipWire(num115 + 1, num114 + 2);
			short num116 = (short)((Main.tile[num115, num114].frameX != 0) ? (-216) : 216);
			for (int num117 = 0; num117 < 2; num117++)
			{
				for (int num118 = 0; num118 < 3; num118++)
				{
					Main.tile[num115 + num117, num114 + num118].frameX += num116;
				}
			}
			if (Main.netMode == 2)
			{
				NetMessage.SendTileSquare(-1, num115, num114, 2, 3);
			}
			Animation.NewTemporaryAnimation((num116 <= 0) ? 1 : 0, 349, num115, num114);
			break;
		}
		case 506:
		{
			int num75 = tile.frameY / 18;
			num75 %= 3;
			int num76 = j - num75;
			int num77;
			for (num77 = tile.frameX / 18; num77 >= 2; num77 -= 2)
			{
			}
			num77 = i - num77;
			if (!WorldGen.ValidateTileSquareIsActiveAndOfType(num77, num76, 2, 3, type))
			{
				break;
			}
			SkipWire(num77, num76);
			SkipWire(num77, num76 + 1);
			SkipWire(num77, num76 + 2);
			SkipWire(num77 + 1, num76);
			SkipWire(num77 + 1, num76 + 1);
			SkipWire(num77 + 1, num76 + 2);
			short num78 = (short)((Main.tile[num77, num76].frameX >= 72) ? (-72) : 72);
			for (int num79 = 0; num79 < 2; num79++)
			{
				for (int num80 = 0; num80 < 3; num80++)
				{
					Main.tile[num77 + num79, num76 + num80].frameX += num78;
				}
			}
			if (Main.netMode == 2)
			{
				NetMessage.SendTileSquare(-1, num77, num76, 2, 3);
			}
			break;
		}
		case 546:
			tile.type = 557;
			WorldGen.SquareTileFrame(i, j);
			NetMessage.SendTileSquare(-1, i, j);
			break;
		case 557:
			tile.type = 546;
			WorldGen.SquareTileFrame(i, j);
			NetMessage.SendTileSquare(-1, i, j);
			break;
		}
	}

	private static void Extractinator(int sourceX, int sourceY)
	{
		Tile tile = Main.tile[sourceX, sourceY];
		int num = sourceX;
		int num2 = sourceY;
		num -= tile.frameX % 54 / 18;
		num2 -= tile.frameY % 54 / 18;
		int time = 60;
		if (!CheckMech(num, num2, time) || !TryFindChestForExtractinator(num, num2, out var chestIndex) || Chest.UsingChest(chestIndex) != -1)
		{
			return;
		}
		int type = tile.type;
		Item[] item = Main.chest[chestIndex].item;
		for (int num3 = Main.chest[chestIndex].maxItems - 1; num3 >= 0; num3--)
		{
			Item item2 = item[num3];
			if (!item2.IsAir)
			{
				ExtractinatorHelper.RollExtractinatorDrop(ItemID.Sets.ExtractinatorMode[item2.type], type, out var itemType, out var stack);
				if (itemType > 0)
				{
					if (--item2.stack <= 0)
					{
						item2.TurnToAir();
					}
					Item.NewItem(new EntitySource_Wiring(num, num2), num * 16, num2 * 16, 32, 32, itemType, stack, noBroadcast: false, -1);
					break;
				}
			}
		}
	}

	private static bool TryFindChestForExtractinator(int lookupX, int lookupY, out int chestIndex)
	{
		chestIndex = 0;
		int num = 3;
		int num2 = 3;
		int num3 = 2;
		for (int i = lookupX - num3; i <= lookupX + num + num3; i++)
		{
			for (int j = lookupY - num3; j <= lookupY + num2 + num3; j++)
			{
				if (!Chest.IsLocked(i, j))
				{
					int num4 = Chest.FindChest(i, j);
					if (num4 != -1)
					{
						chestIndex = num4;
						return true;
					}
				}
			}
		}
		return false;
	}

	public static bool IsHopperInRangeOf(WorldItem item)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		if (ItemID.Sets.ItemsThatShouldNotBeInInventory[item.type])
		{
			return false;
		}
		Rectangle hitbox = item.Hitbox;
		hitbox.Inflate((int)(HopperGrabHitboxSize.X / 2f), (int)(HopperGrabHitboxSize.Y / 2f));
		Point val = hitbox.TopLeft().ToTileCoordinates().ClampedInWorld();
		Point val2 = hitbox.BottomRight().ToTileCoordinates().ClampedInWorld();
		for (int i = val.X; i <= val2.X; i++)
		{
			for (int j = val.Y; j <= val2.Y; j++)
			{
				Tile tile = Main.tile[i, j];
				if (tile != null && tile.active())
				{
					int type = tile.type;
					if ((type == 21 || type == 467) && tile.anyWire())
					{
						return true;
					}
				}
			}
		}
		return false;
	}

	private static void Hopper(int sourceX, int sourceY)
	{
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0101: Unknown result type (might be due to invalid IL or missing references)
		Tile tile = Main.tile[sourceX, sourceY];
		int num = sourceX;
		int num2 = sourceY;
		if (tile.frameX % 36 != 0)
		{
			num--;
		}
		if (tile.frameY % 36 != 0)
		{
			num2--;
		}
		int time = 60;
		if (!CheckMech(num, num2, time) || Chest.IsLocked(num, num2))
		{
			return;
		}
		int num3 = Chest.FindChest(num, num2);
		if (num3 == -1 || Chest.UsingChest(num3) != -1)
		{
			return;
		}
		Rectangle val = Utils.CenteredRectangle(new Vector2((float)(num * 16 + 16), (float)(num2 * 16 + 16)), HopperGrabHitboxSize);
		Chest chest = Main.chest[num3];
		bool flag = false;
		for (int i = 0; i < 400; i++)
		{
			WorldItem worldItem = Main.item[i];
			int type = worldItem.type;
			if (worldItem.active && worldItem.playerIndexTheItemIsReservedFor == Main.myPlayer && !ItemID.Sets.ItemsThatShouldNotBeInInventory[worldItem.type])
			{
				Rectangle hitbox = worldItem.Hitbox;
				if (hitbox.Intersects(val) && TryToPutItemInChest(worldItem, chest))
				{
					flag = true;
					worldItem.SyncItem();
					Chest.VisualizeChestTransfer(worldItem.Center, val.Center.ToVector2(), type, Chest.ItemTransferVisualizationSettings.Hopper);
				}
			}
		}
		if (flag)
		{
			ItemSorting.SortInventory(chest, withSync: false, withFeedback: false);
		}
	}

	private static bool TryToPutItemInChest(WorldItem worldItem, Chest chest)
	{
		if (worldItem.IsACoin)
		{
			return TryMoveCoinsInChest(worldItem, chest);
		}
		for (int i = 0; i < chest.maxItems; i++)
		{
			if (TryAddingToStack(worldItem, ref chest.item[i]))
			{
				return true;
			}
		}
		for (int j = 0; j < chest.maxItems; j++)
		{
			if (TryAddingToEmptySlot(worldItem, ref chest.item[j]))
			{
				return true;
			}
		}
		return false;
	}

	private static bool TryMoveCoinsInChest(WorldItem worldItem, Chest chest)
	{
		if (!worldItem.IsACoin)
		{
			return false;
		}
		int maxItems = chest.maxItems;
		Item[] item = chest.item;
		long num = Utils.CoinsCount(out var _, item);
		int num2 = worldItem.value / 5;
		int[] array = Utils.CoinsSplit(num + num2 * worldItem.stack);
		int[] array2 = new int[array.Length];
		int num3 = 0;
		for (int num4 = array.Length - 1; num4 >= 0; num4--)
		{
			if (array[num4] != 0)
			{
				while (true)
				{
					if (num3 >= maxItems)
					{
						return false;
					}
					if (item[num3].IsAir || item[num3].IsACoin)
					{
						break;
					}
					num3++;
				}
				array2[num4] = num3++;
			}
		}
		Item[] array3 = item;
		foreach (Item item2 in array3)
		{
			if (item2.IsACoin)
			{
				item2.TurnToAir();
			}
		}
		for (int num5 = array.Length - 1; num5 >= 0; num5--)
		{
			if (array[num5] != 0)
			{
				int num6 = 71 + num5;
				int num7 = Math.Min(ContentSamples.ItemsByType[num6].maxStack, array[num5]);
				Item obj = item[array2[num5]];
				obj.SetDefaults(num6);
				obj.stack = num7;
				array[num5] -= num7;
				_ = array[num5];
				_ = 0;
			}
		}
		worldItem.TurnToAir();
		return true;
	}

	private static bool TryAddingToEmptySlot(WorldItem worldItem, ref Item chestItem)
	{
		if (chestItem.stack != 0)
		{
			return false;
		}
		SoundEngine.PlaySound(7);
		chestItem = worldItem.inner.Clone();
		chestItem.newAndShiny = false;
		worldItem.TurnToAir();
		return true;
	}

	private static bool TryAddingToStack(WorldItem worldItem, ref Item chestItem)
	{
		if (chestItem.stack >= chestItem.maxStack || !Item.CanStack(worldItem.inner, chestItem))
		{
			return false;
		}
		int num = worldItem.stack;
		if (worldItem.stack + chestItem.stack > chestItem.maxStack)
		{
			num = chestItem.maxStack - chestItem.stack;
		}
		worldItem.stack -= num;
		chestItem.stack += num;
		if (worldItem.stack <= 0)
		{
			worldItem.TurnToAir();
			return true;
		}
		if (chestItem.type == 0)
		{
			chestItem = worldItem.inner.Clone();
			chestItem.newAndShiny = false;
			worldItem.TurnToAir();
			return true;
		}
		return num > 0;
	}

	public static void ToggleHolidayLight(int i, int j, Tile tileCache, bool? forcedStateWhereTrueIsOn)
	{
		bool flag = tileCache.frameX >= 54;
		if (!forcedStateWhereTrueIsOn.HasValue || !forcedStateWhereTrueIsOn.Value != flag)
		{
			if (tileCache.frameX < 54)
			{
				tileCache.frameX += 54;
			}
			else
			{
				tileCache.frameX -= 54;
			}
			NetMessage.SendTileSquare(-1, i, j);
		}
	}

	public static void ToggleHangingLantern(int i, int j, Tile tileCache, bool? forcedStateWhereTrueIsOn, bool doSkipWires)
	{
		int num;
		for (num = tileCache.frameY / 18; num >= 2; num -= 2)
		{
		}
		int num2 = j - num;
		short num3 = 18;
		if (tileCache.frameX > 0)
		{
			num3 = -18;
		}
		bool flag = tileCache.frameX > 0;
		if (!forcedStateWhereTrueIsOn.HasValue || !forcedStateWhereTrueIsOn.Value != flag)
		{
			Main.tile[i, num2].frameX += num3;
			Main.tile[i, num2 + 1].frameX += num3;
			if (doSkipWires)
			{
				SkipWire(i, num2);
				SkipWire(i, num2 + 1);
			}
			NetMessage.SendTileSquare(-1, i, num2, 1, 2);
		}
	}

	public static void Toggle2x2Light(int i, int j, Tile tileCache, bool? forcedStateWhereTrueIsOn, bool doSkipWires)
	{
		int num;
		for (num = tileCache.frameY / 18; num >= 2; num -= 2)
		{
		}
		num = j - num;
		int num2 = tileCache.frameX / 18;
		if (num2 > 1)
		{
			num2 -= 2;
		}
		num2 = i - num2;
		short num3 = 36;
		if (Main.tile[num2, num].frameX > 0)
		{
			num3 = -36;
		}
		bool flag = Main.tile[num2, num].frameX > 0;
		if (!forcedStateWhereTrueIsOn.HasValue || !forcedStateWhereTrueIsOn.Value != flag)
		{
			Main.tile[num2, num].frameX += num3;
			Main.tile[num2, num + 1].frameX += num3;
			Main.tile[num2 + 1, num].frameX += num3;
			Main.tile[num2 + 1, num + 1].frameX += num3;
			if (doSkipWires)
			{
				SkipWire(num2, num);
				SkipWire(num2 + 1, num);
				SkipWire(num2, num + 1);
				SkipWire(num2 + 1, num + 1);
			}
			NetMessage.SendTileSquare(-1, num2, num, 2, 2);
		}
	}

	public static void ToggleLampPost(int i, int j, Tile tileCache, bool? forcedStateWhereTrueIsOn, bool doSkipWires)
	{
		int num = j - tileCache.frameY / 18;
		short num2 = 18;
		if (tileCache.frameX > 0)
		{
			num2 = -18;
		}
		bool flag = tileCache.frameX > 0;
		if (forcedStateWhereTrueIsOn.HasValue && !forcedStateWhereTrueIsOn.Value == flag)
		{
			return;
		}
		for (int k = num; k < num + 6; k++)
		{
			Main.tile[i, k].frameX += num2;
			if (doSkipWires)
			{
				SkipWire(i, k);
			}
		}
		NetMessage.SendTileSquare(-1, i, num, 1, 6);
	}

	public static void ToggleTorch(int i, int j, Tile tileCache, bool? forcedStateWhereTrueIsOn)
	{
		bool flag = tileCache.frameX >= 66;
		if (!forcedStateWhereTrueIsOn.HasValue || !forcedStateWhereTrueIsOn.Value != flag)
		{
			if (tileCache.frameX < 66)
			{
				tileCache.frameX += 66;
			}
			else
			{
				tileCache.frameX -= 66;
			}
			NetMessage.SendTileSquare(-1, i, j);
		}
	}

	public static void ToggleCandle(int i, int j, Tile tileCache, bool? forcedStateWhereTrueIsOn)
	{
		short num = 18;
		if (tileCache.frameX > 0)
		{
			num = -18;
		}
		bool flag = tileCache.frameX > 0;
		if (!forcedStateWhereTrueIsOn.HasValue || !forcedStateWhereTrueIsOn.Value != flag)
		{
			tileCache.frameX += num;
			NetMessage.SendTileSquare(-1, i, j, 3);
		}
	}

	public static void ToggleLamp(int i, int j, Tile tileCache, bool? forcedStateWhereTrueIsOn, bool doSkipWires)
	{
		int num;
		for (num = tileCache.frameY / 18; num >= 3; num -= 3)
		{
		}
		num = j - num;
		short num2 = 18;
		if (tileCache.frameX > 0)
		{
			num2 = -18;
		}
		bool flag = tileCache.frameX > 0;
		if (!forcedStateWhereTrueIsOn.HasValue || !forcedStateWhereTrueIsOn.Value != flag)
		{
			Main.tile[i, num].frameX += num2;
			Main.tile[i, num + 1].frameX += num2;
			Main.tile[i, num + 2].frameX += num2;
			if (doSkipWires)
			{
				SkipWire(i, num);
				SkipWire(i, num + 1);
				SkipWire(i, num + 2);
			}
			NetMessage.SendTileSquare(-1, i, num, 1, 3);
		}
	}

	public static void ToggleChandelier(int i, int j, Tile tileCache, bool? forcedStateWhereTrueIsOn, bool doSkipWires)
	{
		int num;
		for (num = tileCache.frameY / 18; num >= 3; num -= 3)
		{
		}
		int num2 = j - num;
		int num3 = tileCache.frameX % 108 / 18;
		if (num3 > 2)
		{
			num3 -= 3;
		}
		num3 = i - num3;
		short num4 = 54;
		if (Main.tile[num3, num2].frameX % 108 > 0)
		{
			num4 = -54;
		}
		bool flag = Main.tile[num3, num2].frameX % 108 > 0;
		if (forcedStateWhereTrueIsOn.HasValue && !forcedStateWhereTrueIsOn.Value == flag)
		{
			return;
		}
		for (int k = num3; k < num3 + 3; k++)
		{
			for (int l = num2; l < num2 + 3; l++)
			{
				Main.tile[k, l].frameX += num4;
				if (doSkipWires)
				{
					SkipWire(k, l);
				}
			}
		}
		NetMessage.SendTileSquare(-1, num3 + 1, num2 + 1, 3);
	}

	public static void ToggleCampFire(int i, int j, Tile tileCache, bool? forcedStateWhereTrueIsOn, bool doSkipWires)
	{
		if (!TileID.Sets.Campfires[tileCache.type])
		{
			return;
		}
		int num = 3;
		int num2 = 2;
		int num3 = tileCache.frameX % (num * 18) / 18;
		int num4 = tileCache.frameY % (num2 * 18) / 18;
		int num5 = i - num3;
		int num6 = j - num4;
		if (!WorldGen.ValidateTileSquareIsActiveAndOfType(num5, num6, num, num2, tileCache.type))
		{
			return;
		}
		bool flag = Main.tile[num5, num6].frameY >= 36;
		if (forcedStateWhereTrueIsOn.HasValue && !forcedStateWhereTrueIsOn.Value == flag)
		{
			return;
		}
		short num7 = 36;
		if (Main.tile[num5, num6].frameY >= 36)
		{
			num7 = -36;
		}
		for (int k = num5; k < num5 + num; k++)
		{
			for (int l = num6; l < num6 + num2; l++)
			{
				if (doSkipWires)
				{
					SkipWire(k, l);
				}
				Tile tile = Main.tile[k, l];
				if (tile.active() && tile.type == tileCache.type)
				{
					tile.frameY += num7;
				}
			}
		}
		NetMessage.SendTileSquare(-1, num5, num6, num, num2);
	}

	public static void ToggleFirePlace(int i, int j, Tile theBlock, bool? forcedStateWhereTrueIsOn, bool doSkipWires)
	{
		int num = theBlock.frameX % 54 / 18;
		int num2 = theBlock.frameY % 36 / 18;
		int num3 = i - num;
		int num4 = j - num2;
		bool flag = Main.tile[num3, num4].frameX >= 54;
		if (forcedStateWhereTrueIsOn.HasValue && !forcedStateWhereTrueIsOn.Value == flag)
		{
			return;
		}
		int num5 = 54;
		if (Main.tile[num3, num4].frameX >= 54)
		{
			num5 = -54;
		}
		for (int k = num3; k < num3 + 3; k++)
		{
			for (int l = num4; l < num4 + 2; l++)
			{
				if (doSkipWires)
				{
					SkipWire(k, l);
				}
				Main.tile[k, l].frameX = (short)(Main.tile[k, l].frameX + num5);
			}
		}
		NetMessage.SendTileSquare(-1, num3, num4, 3, 2);
	}

	public static void ExplodeMine(int i, int j)
	{
		if (Main.netMode != 1)
		{
			WorldGen.KillTile(i, j, fail: false, effectOnly: false, noItem: true);
			NetMessage.SendTileSquare(-1, i, j);
			Projectile.NewProjectile(GetProjectileSource(i, j), i * 16 + 8, j * 16 + 8, 0f, 0f, 164, 250, 10f, Main.myPlayer);
		}
	}

	private static void GeyserTrap(int i, int j)
	{
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
		if (Main.netMode == 1)
		{
			return;
		}
		Tile tile = Main.tile[i, j];
		if (tile.type != 443)
		{
			return;
		}
		int num = tile.frameX / 36;
		int num2 = i - (tile.frameX - num * 36) / 18;
		if (CheckMech(num2, j, 200))
		{
			Vector2 zero = Vector2.Zero;
			Vector2 zero2 = Vector2.Zero;
			int num3 = 654;
			int damage = 20;
			if (num < 2)
			{
				zero = new Vector2((float)(num2 + 1), (float)j) * 16f;
				zero2 = new Vector2(0f, -8f);
			}
			else
			{
				zero = new Vector2((float)(num2 + 1), (float)(j + 1)) * 16f;
				zero2 = new Vector2(0f, 8f);
			}
			if (num3 != 0)
			{
				Projectile.NewProjectile(GetProjectileSource(num2, j), (int)zero.X, (int)zero.Y, zero2.X, zero2.Y, num3, damage, 2f, Main.myPlayer);
			}
		}
	}

	private static void Teleport()
	{
		//IL_01bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0249: Unknown result type (might be due to invalid IL or missing references)
		//IL_0255: Unknown result type (might be due to invalid IL or missing references)
		//IL_0268: Unknown result type (might be due to invalid IL or missing references)
		//IL_026d: Unknown result type (might be due to invalid IL or missing references)
		//IL_026e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0273: Unknown result type (might be due to invalid IL or missing references)
		//IL_029a: Unknown result type (might be due to invalid IL or missing references)
		//IL_028b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0350: Unknown result type (might be due to invalid IL or missing references)
		//IL_035d: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_0387: Unknown result type (might be due to invalid IL or missing references)
		//IL_038c: Unknown result type (might be due to invalid IL or missing references)
		//IL_038d: Unknown result type (might be due to invalid IL or missing references)
		if (_teleport[0].X < _teleport[1].X + 3f && _teleport[0].X > _teleport[1].X - 3f && _teleport[0].Y > _teleport[1].Y - 3f && _teleport[0].Y < _teleport[1].Y)
		{
			return;
		}
		Rectangle[] array = new Rectangle[2];
		array[0].X = (int)(_teleport[0].X * 16f);
		array[0].Width = 48;
		array[0].Height = 48;
		array[0].Y = (int)(_teleport[0].Y * 16f - (float)array[0].Height);
		array[1].X = (int)(_teleport[1].X * 16f);
		array[1].Width = 48;
		array[1].Height = 48;
		array[1].Y = (int)(_teleport[1].Y * 16f - (float)array[1].Height);
		for (int i = 0; i < 2; i++)
		{
			Vector2 val = new Vector2((float)(array[1].X - array[0].X), (float)(array[1].Y - array[0].Y));
			if (i == 1)
			{
				val = new Vector2((float)(array[0].X - array[1].X), (float)(array[0].Y - array[1].Y));
			}
			if (!blockPlayerTeleportationForOneIteration)
			{
				for (int j = 0; j < 255; j++)
				{
					if (Main.player[j].active && !Main.player[j].dead && !Main.player[j].teleporting && TeleporterHitboxIntersects(array[i], Main.player[j].Hitbox))
					{
						Vector2 val2 = Main.player[j].position + val;
						Main.player[j].teleporting = true;
						if (Main.netMode == 2)
						{
							RemoteClient.CheckSection(j, val2);
						}
						Main.player[j].Teleport(val2);
						if (Main.netMode == 2)
						{
							NetMessage.SendData(65, -1, -1, null, 0, j, val2.X, val2.Y);
						}
					}
				}
			}
			for (int k = 0; k < Main.maxNPCs; k++)
			{
				if (Main.npc[k].active && !Main.npc[k].teleporting && Main.npc[k].lifeMax > 5 && !Main.npc[k].boss && !Main.npc[k].noTileCollide)
				{
					int type = Main.npc[k].type;
					if (!NPCID.Sets.TeleportationImmune[type] && TeleporterHitboxIntersects(array[i], Main.npc[k].Hitbox))
					{
						Main.npc[k].teleporting = true;
						Main.npc[k].Teleport(Main.npc[k].position + val);
					}
				}
			}
		}
		for (int l = 0; l < 255; l++)
		{
			Main.player[l].teleporting = false;
		}
		for (int m = 0; m < Main.maxNPCs; m++)
		{
			Main.npc[m].teleporting = false;
		}
	}

	private static bool TeleporterHitboxIntersects(Rectangle teleporter, Rectangle entity)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		Rectangle val = Rectangle.Union(teleporter, entity);
		if (val.Width <= teleporter.Width + entity.Width)
		{
			return val.Height <= teleporter.Height + entity.Height;
		}
		return false;
	}

	private static void DeActive(int i, int j)
	{
		if (!Main.tile[i, j].active() || (Main.tile[i, j].type == 226 && (double)j > Main.worldSurface && !NPC.downedPlantBoss))
		{
			return;
		}
		bool flag = Main.tileSolid[Main.tile[i, j].type] && !TileID.Sets.NotReallySolid[Main.tile[i, j].type];
		switch (Main.tile[i, j].type)
		{
		case 314:
		case 379:
		case 386:
		case 387:
		case 388:
		case 389:
		case 476:
			flag = false;
			break;
		}
		if (flag && (!Main.tile[i, j - 1].active() || (!TileID.Sets.PreventsActuationUnder[Main.tile[i, j - 1].type] && WorldGen.CanKillTile(i, j))))
		{
			Main.tile[i, j].inActive(inActive: true);
			WorldGen.SquareTileFrame(i, j, resetFrame: false);
			if (Main.netMode != 1)
			{
				NetMessage.SendTileSquare(-1, i, j);
			}
		}
	}

	private static void ReActive(int i, int j)
	{
		Main.tile[i, j].inActive(inActive: false);
		WorldGen.SquareTileFrame(i, j, resetFrame: false);
		if (Main.netMode != 1)
		{
			NetMessage.SendTileSquare(-1, i, j);
		}
	}

	private static void MassWireOperationInner(Player user, Point ps, Point pe, Vector2 dropPoint, bool dir, ref int wireCount, ref int actuatorCount)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0137: Unknown result type (might be due to invalid IL or missing references)
		//IL_0142: Unknown result type (might be due to invalid IL or missing references)
		//IL_014a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0115: Unknown result type (might be due to invalid IL or missing references)
		//IL_0120: Unknown result type (might be due to invalid IL or missing references)
		//IL_0128: Unknown result type (might be due to invalid IL or missing references)
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_0174: Unknown result type (might be due to invalid IL or missing references)
		Math.Abs(ps.X - pe.X);
		Math.Abs(ps.Y - pe.Y);
		int num = Math.Sign(pe.X - ps.X);
		int num2 = Math.Sign(pe.Y - ps.Y);
		WiresUI.Settings.MultiToolMode toolMode = WiresUI.Settings.ToolMode;
		Point pt = default;
		bool flag = false;
		Item.StartCachingType(530);
		Item.StartCachingType(849);
		bool flag2 = dir;
		int num3;
		int num4;
		int num5;
		if (flag2)
		{
			pt.X = ps.X;
			num3 = ps.Y;
			num4 = pe.Y;
			num5 = num2;
		}
		else
		{
			pt.Y = ps.Y;
			num3 = ps.X;
			num4 = pe.X;
			num5 = num;
		}
		for (int i = num3; i != num4; i += num5)
		{
			if (flag)
			{
				break;
			}
			if (flag2)
			{
				pt.Y = i;
			}
			else
			{
				pt.X = i;
			}
			bool? flag3 = MassWireOperationStep(user, pt, toolMode, ref wireCount, ref actuatorCount);
			if (flag3.HasValue && !flag3.Value)
			{
				flag = true;
				break;
			}
		}
		if (flag2)
		{
			pt.Y = pe.Y;
			num3 = ps.X;
			num4 = pe.X;
			num5 = num;
		}
		else
		{
			pt.X = pe.X;
			num3 = ps.Y;
			num4 = pe.Y;
			num5 = num2;
		}
		for (int j = num3; j != num4; j += num5)
		{
			if (flag)
			{
				break;
			}
			if (!flag2)
			{
				pt.Y = j;
			}
			else
			{
				pt.X = j;
			}
			bool? flag4 = MassWireOperationStep(user, pt, toolMode, ref wireCount, ref actuatorCount);
			if (flag4.HasValue && !flag4.Value)
			{
				flag = true;
				break;
			}
		}
		if (!flag)
		{
			MassWireOperationStep(user, pe, toolMode, ref wireCount, ref actuatorCount);
		}
		EntitySource_ByItemSourceId reason = new EntitySource_ByItemSourceId(user, ItemSourceID.GrandDesignOrMultiColorWrench);
		Item.DropCache(reason, dropPoint, Vector2.Zero, 530);
		Item.DropCache(reason, dropPoint, Vector2.Zero, 849);
	}

	private static bool? MassWireOperationStep(Player user, Point pt, WiresUI.Settings.MultiToolMode mode, ref int wiresLeftToConsume, ref int actuatorsLeftToConsume)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_022d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0233: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_026e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0274: Unknown result type (might be due to invalid IL or missing references)
		//IL_0246: Unknown result type (might be due to invalid IL or missing references)
		//IL_024d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0288: Unknown result type (might be due to invalid IL or missing references)
		//IL_028f: Unknown result type (might be due to invalid IL or missing references)
		//IL_012d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0133: Unknown result type (might be due to invalid IL or missing references)
		//IL_0146: Unknown result type (might be due to invalid IL or missing references)
		//IL_014d: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0182: Unknown result type (might be due to invalid IL or missing references)
		//IL_0188: Unknown result type (might be due to invalid IL or missing references)
		//IL_019b: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0335: Unknown result type (might be due to invalid IL or missing references)
		//IL_033b: Unknown result type (might be due to invalid IL or missing references)
		//IL_030c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0313: Unknown result type (might be due to invalid IL or missing references)
		//IL_01db: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_034f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0356: Unknown result type (might be due to invalid IL or missing references)
		if (!WorldGen.InWorld(pt.X, pt.Y, 1))
		{
			return null;
		}
		Tile tile = Main.tile[pt.X, pt.Y];
		if (tile == null)
		{
			return null;
		}
		if (user != null && !user.CanDoWireStuffHere(pt.X, pt.Y))
		{
			return null;
		}
		if ((mode & WiresUI.Settings.MultiToolMode.Cutter) == 0)
		{
			if ((mode & WiresUI.Settings.MultiToolMode.Red) != 0 && !tile.wire())
			{
				if (wiresLeftToConsume <= 0)
				{
					return false;
				}
				wiresLeftToConsume--;
				WorldGen.PlaceWire(pt.X, pt.Y);
				NetMessage.SendData(17, -1, -1, null, 5, pt.X, pt.Y);
			}
			if ((mode & WiresUI.Settings.MultiToolMode.Green) != 0 && !tile.wire3())
			{
				if (wiresLeftToConsume <= 0)
				{
					return false;
				}
				wiresLeftToConsume--;
				WorldGen.PlaceWire3(pt.X, pt.Y);
				NetMessage.SendData(17, -1, -1, null, 12, pt.X, pt.Y);
			}
			if ((mode & WiresUI.Settings.MultiToolMode.Blue) != 0 && !tile.wire2())
			{
				if (wiresLeftToConsume <= 0)
				{
					return false;
				}
				wiresLeftToConsume--;
				WorldGen.PlaceWire2(pt.X, pt.Y);
				NetMessage.SendData(17, -1, -1, null, 10, pt.X, pt.Y);
			}
			if ((mode & WiresUI.Settings.MultiToolMode.Yellow) != 0 && !tile.wire4())
			{
				if (wiresLeftToConsume <= 0)
				{
					return false;
				}
				wiresLeftToConsume--;
				WorldGen.PlaceWire4(pt.X, pt.Y);
				NetMessage.SendData(17, -1, -1, null, 16, pt.X, pt.Y);
			}
			if ((mode & WiresUI.Settings.MultiToolMode.Actuator) != 0 && !tile.actuator())
			{
				if (actuatorsLeftToConsume <= 0)
				{
					return false;
				}
				actuatorsLeftToConsume--;
				WorldGen.PlaceActuator(pt.X, pt.Y);
				NetMessage.SendData(17, -1, -1, null, 8, pt.X, pt.Y);
			}
		}
		if ((mode & WiresUI.Settings.MultiToolMode.Cutter) != 0)
		{
			if ((mode & WiresUI.Settings.MultiToolMode.Red) != 0 && tile.wire() && WorldGen.KillWire(pt.X, pt.Y))
			{
				NetMessage.SendData(17, -1, -1, null, 6, pt.X, pt.Y);
			}
			if ((mode & WiresUI.Settings.MultiToolMode.Green) != 0 && tile.wire3() && WorldGen.KillWire3(pt.X, pt.Y))
			{
				NetMessage.SendData(17, -1, -1, null, 13, pt.X, pt.Y);
			}
			if ((mode & WiresUI.Settings.MultiToolMode.Blue) != 0 && tile.wire2() && WorldGen.KillWire2(pt.X, pt.Y))
			{
				NetMessage.SendData(17, -1, -1, null, 11, pt.X, pt.Y);
			}
			if ((mode & WiresUI.Settings.MultiToolMode.Yellow) != 0 && tile.wire4() && WorldGen.KillWire4(pt.X, pt.Y))
			{
				NetMessage.SendData(17, -1, -1, null, 17, pt.X, pt.Y);
			}
			if ((mode & WiresUI.Settings.MultiToolMode.Actuator) != 0 && tile.actuator() && WorldGen.KillActuator(pt.X, pt.Y))
			{
				NetMessage.SendData(17, -1, -1, null, 9, pt.X, pt.Y);
			}
		}
		return true;
	}

	static Wiring()
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
	}
}
