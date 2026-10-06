using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameInput;

namespace Terraria.GameContent.UI;

public class WiresUI
{
	public static class Settings
	{
		[Flags]
		public enum MultiToolMode
		{
			Red = 1,
			Green = 2,
			Blue = 4,
			Yellow = 8,
			Actuator = 0x10,
			Cutter = 0x20
		}

		public static MultiToolMode ToolMode = MultiToolMode.Red;

		private static int _lastActuatorEnabled;

		public static bool DrawWires
		{
			get
			{
				if (Main.noTrapsWorld && !NPC.downedBoss3)
				{
					return false;
				}
				if (Main.LocalPlayer.HeldItem.IsAir || !Main.LocalPlayer.HeldItem.mech)
				{
					if (Main.LocalPlayer.InfoAccMechShowWires)
					{
						return Main.LocalPlayer.builderAccStatus[8] == 0;
					}
					return false;
				}
				return true;
			}
		}

		public static bool HideWires
		{
			get
			{
				if (!Main.LocalPlayer.HeldItem.IsAir)
				{
					return Main.LocalPlayer.HeldItem.type == 3620;
				}
				return false;
			}
		}

		public static bool DrawToolModeUI
		{
			get
			{
				if (!Main.LocalPlayer.HeldItem.IsAir)
				{
					if (Main.LocalPlayer.HeldItem.type != 3611)
					{
						return Main.LocalPlayer.HeldItem.type == 3625;
					}
					return true;
				}
				return false;
			}
		}

		public static bool DrawToolAllowActuators
		{
			get
			{
				int type = Main.player[Main.myPlayer].inventory[Main.player[Main.myPlayer].selectedItem].type;
				if (type == 3611)
				{
					_lastActuatorEnabled = 2;
				}
				if (type == 3625)
				{
					_lastActuatorEnabled = 1;
				}
				return _lastActuatorEnabled == 2;
			}
		}
	}

	public class WiresRadial
	{
		public Vector2 position;

		public bool active;

		public bool OnWiresMenu;

		private float _lineOpacity;

		public void Update()
		{
			FlowerUpdate();
			LineUpdate();
		}

		private void LineUpdate()
		{
			bool value = true;
			float min = 0.75f;
			Player player = Main.player[Main.myPlayer];
			if (!Settings.DrawToolModeUI || Main.drawingPlayerChat)
			{
				value = false;
				min = 0f;
			}
			if (player.dead || Main.mouseItem.type > 0)
			{
				value = false;
				_lineOpacity = 0f;
				return;
			}
			if (player.cursorItemIconEnabled && player.cursorItemIconID != 0 && player.cursorItemIconID != 3625)
			{
				value = false;
				_lineOpacity = 0f;
				return;
			}
			if ((!player.cursorItemIconEnabled && ((!PlayerInput.UsingGamepad && !Settings.DrawToolAllowActuators) || player.mouseInterface || player.lastMouseInterface)) || Main.ingameOptionsWindow || Main.InGameUI.IsVisible)
			{
				value = false;
				_lineOpacity = 0f;
				return;
			}
			float num = Utils.Clamp(_lineOpacity + 0.05f * (float)value.ToDirectionInt(), min, 1f);
			_lineOpacity += 0.05f * (float)Math.Sign(num - _lineOpacity);
			if (Math.Abs(_lineOpacity - num) < 0.05f)
			{
				_lineOpacity = num;
			}
		}

		private void FlowerUpdate()
		{
			//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
			Player player = Main.player[Main.myPlayer];
			if (!Settings.DrawToolModeUI)
			{
				active = false;
				return;
			}
			if ((player.mouseInterface || player.lastMouseInterface) && !OnWiresMenu)
			{
				active = false;
				return;
			}
			if (player.dead || Main.mouseItem.type > 0)
			{
				active = false;
				OnWiresMenu = false;
				return;
			}
			OnWiresMenu = false;
			if (!Main.mouseRight || !Main.mouseRightRelease || PlayerInput.LockGamepadTileUseButton || player.noThrow != 0 || Main.HoveringOverAnNPC || player.talkNPC != -1)
			{
				return;
			}
			if (active)
			{
				active = false;
			}
			else if (!Main.SmartInteractShowingGenuine)
			{
				active = true;
				position = Main.MouseScreen;
				if (PlayerInput.UsingGamepad && Main.SmartCursorWanted)
				{
					position = new Vector2((float)Main.screenWidth, (float)Main.screenHeight) / 2f;
				}
			}
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			DrawFlower(spriteBatch);
			DrawCursorArea(spriteBatch);
		}

		private void DrawLine(SpriteBatch spriteBatch)
		{
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00da: Unknown result type (might be due to invalid IL or missing references)
			//IL_0113: Unknown result type (might be due to invalid IL or missing references)
			//IL_014b: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0181: Unknown result type (might be due to invalid IL or missing references)
			//IL_018c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0197: Unknown result type (might be due to invalid IL or missing references)
			//IL_019c: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_012f: Unknown result type (might be due to invalid IL or missing references)
			//IL_013a: Unknown result type (might be due to invalid IL or missing references)
			//IL_013f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0144: Unknown result type (might be due to invalid IL or missing references)
			//IL_0168: Unknown result type (might be due to invalid IL or missing references)
			//IL_0173: Unknown result type (might be due to invalid IL or missing references)
			//IL_0178: Unknown result type (might be due to invalid IL or missing references)
			//IL_017d: Unknown result type (might be due to invalid IL or missing references)
			//IL_01be: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01df: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_028a: Unknown result type (might be due to invalid IL or missing references)
			//IL_028f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0291: Unknown result type (might be due to invalid IL or missing references)
			//IL_0296: Unknown result type (might be due to invalid IL or missing references)
			//IL_0307: Unknown result type (might be due to invalid IL or missing references)
			//IL_030e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0313: Unknown result type (might be due to invalid IL or missing references)
			//IL_03da: Unknown result type (might be due to invalid IL or missing references)
			//IL_03e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_03fa: Unknown result type (might be due to invalid IL or missing references)
			//IL_0404: Unknown result type (might be due to invalid IL or missing references)
			//IL_041d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0429: Unknown result type (might be due to invalid IL or missing references)
			//IL_0431: Unknown result type (might be due to invalid IL or missing references)
			//IL_043d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0447: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f5: Unknown result type (might be due to invalid IL or missing references)
			//IL_0302: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d0: Unknown result type (might be due to invalid IL or missing references)
			if (active || _lineOpacity == 0f)
			{
				return;
			}
			Vector2 val = Main.MouseScreen;
			Vector2 val2 = new Vector2((float)(Main.screenWidth / 2), (float)(Main.screenHeight - 70));
			if (PlayerInput.UsingGamepad)
			{
				val = Vector2.Zero;
			}
			Vector2 val3 = val - val2;
			Vector2.Dot(Vector2.Normalize(val3), Vector2.UnitX);
			Vector2.Dot(Vector2.Normalize(val3), Vector2.UnitY);
			val3.ToRotation();
			val3.Length();
			bool flag = false;
			bool drawToolAllowActuators = Settings.DrawToolAllowActuators;
			for (int i = 0; i < 6; i++)
			{
				if (!drawToolAllowActuators && i == 5)
				{
					continue;
				}
				bool flag2 = ((uint)Settings.ToolMode & (uint)(1 << i)) != 0;
				if (i == 5)
				{
					flag2 = (Settings.ToolMode & Settings.MultiToolMode.Actuator) != 0;
				}
				Vector2 val4 = val2 + Vector2.UnitX * (45f * ((float)i - 1.5f));
				int num = i;
				if (i == 0)
				{
					num = 3;
				}
				if (i == 3)
				{
					num = 0;
				}
				switch (num)
				{
				case 0:
				case 1:
					val4 = val2 + new Vector2((45f + (float)(drawToolAllowActuators ? 15 : 0)) * (float)(2 - num), 0f) * _lineOpacity;
					break;
				case 2:
				case 3:
					val4 = val2 + new Vector2((0f - (45f + (float)(drawToolAllowActuators ? 15 : 0))) * (float)(num - 1), 0f) * _lineOpacity;
					break;
				case 5:
					val4 = val2 + new Vector2(0f, 22f) * _lineOpacity;
					break;
				case 4:
					flag2 = false;
					val4 = val2 - new Vector2(0f, drawToolAllowActuators ? 22f : 0f) * _lineOpacity;
					break;
				}
				bool flag3 = false;
				if (!PlayerInput.UsingGamepad)
				{
					flag3 = Vector2.Distance(val4, val) < 19f * _lineOpacity;
				}
				if (flag)
				{
					flag3 = false;
				}
				if (flag3)
				{
					flag = true;
				}
				Texture2D value = TextureAssets.WireUi[(((Settings.ToolMode & Settings.MultiToolMode.Cutter) != 0) ? 8 : 0) + (flag3 ? 1 : 0)].Value;
				Texture2D val5 = null;
				switch (i)
				{
				case 0:
				case 1:
				case 2:
				case 3:
					val5 = TextureAssets.WireUi[2 + i].Value;
					break;
				case 4:
					val5 = TextureAssets.WireUi[((Settings.ToolMode & Settings.MultiToolMode.Cutter) != 0) ? 7 : 6].Value;
					break;
				case 5:
					val5 = TextureAssets.WireUi[10].Value;
					break;
				}
				Color val6 = Color.White;
				Color val7 = Color.White;
				if (!flag2 && i != 4)
				{
					if (flag3)
					{
						val7 = new Color(100, 100, 100);
						val7 = new Color(120, 120, 120);
						val6 = new Color(200, 200, 200);
					}
					else
					{
						val7 = new Color(150, 150, 150);
						val7 = new Color(80, 80, 80);
						val6 = new Color(100, 100, 100);
					}
				}
				Utils.CenteredRectangle(val4, new Vector2(40f));
				if (flag3)
				{
					if (Main.mouseLeft && Main.mouseLeftRelease)
					{
						switch (i)
						{
						case 0:
							Settings.ToolMode ^= Settings.MultiToolMode.Red;
							break;
						case 1:
							Settings.ToolMode ^= Settings.MultiToolMode.Green;
							break;
						case 2:
							Settings.ToolMode ^= Settings.MultiToolMode.Blue;
							break;
						case 3:
							Settings.ToolMode ^= Settings.MultiToolMode.Yellow;
							break;
						case 4:
							Settings.ToolMode ^= Settings.MultiToolMode.Cutter;
							break;
						case 5:
							Settings.ToolMode ^= Settings.MultiToolMode.Actuator;
							break;
						}
					}
					if (!Main.mouseLeft || Main.player[Main.myPlayer].mouseInterface)
					{
						Main.player[Main.myPlayer].mouseInterface = true;
					}
					OnWiresMenu = true;
				}
				spriteBatch.Draw(value, val4, (Rectangle?)null, val6 * _lineOpacity, 0f, value.Size() / 2f, _lineOpacity, (SpriteEffects)0, 0f);
				spriteBatch.Draw(val5, val4, (Rectangle?)null, val7 * _lineOpacity, 0f, val5.Size() / 2f, _lineOpacity, (SpriteEffects)0, 0f);
			}
			if (Main.mouseLeft && Main.mouseLeftRelease && !flag)
			{
				active = false;
			}
		}

		private void DrawFlower(SpriteBatch spriteBatch)
		{
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_0087: Unknown result type (might be due to invalid IL or missing references)
			//IL_0088: Unknown result type (might be due to invalid IL or missing references)
			//IL_0089: Unknown result type (might be due to invalid IL or missing references)
			//IL_008e: Unknown result type (might be due to invalid IL or missing references)
			//IL_008f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0090: Unknown result type (might be due to invalid IL or missing references)
			//IL_0095: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0081: Unknown result type (might be due to invalid IL or missing references)
			//IL_0086: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			//IL_0069: Unknown result type (might be due to invalid IL or missing references)
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			//IL_0078: Unknown result type (might be due to invalid IL or missing references)
			//IL_007d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0121: Unknown result type (might be due to invalid IL or missing references)
			//IL_0122: Unknown result type (might be due to invalid IL or missing references)
			//IL_0136: Unknown result type (might be due to invalid IL or missing references)
			//IL_013b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0140: Unknown result type (might be due to invalid IL or missing references)
			//IL_0202: Unknown result type (might be due to invalid IL or missing references)
			//IL_0203: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01df: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0182: Unknown result type (might be due to invalid IL or missing references)
			//IL_0183: Unknown result type (might be due to invalid IL or missing references)
			//IL_019f: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0246: Unknown result type (might be due to invalid IL or missing references)
			//IL_0248: Unknown result type (might be due to invalid IL or missing references)
			//IL_0249: Unknown result type (might be due to invalid IL or missing references)
			//IL_028c: Unknown result type (might be due to invalid IL or missing references)
			//IL_028e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0332: Unknown result type (might be due to invalid IL or missing references)
			//IL_0337: Unknown result type (might be due to invalid IL or missing references)
			//IL_0339: Unknown result type (might be due to invalid IL or missing references)
			//IL_033e: Unknown result type (might be due to invalid IL or missing references)
			//IL_03af: Unknown result type (might be due to invalid IL or missing references)
			//IL_03b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_03bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0469: Unknown result type (might be due to invalid IL or missing references)
			//IL_0475: Unknown result type (might be due to invalid IL or missing references)
			//IL_047e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0488: Unknown result type (might be due to invalid IL or missing references)
			//IL_04a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_04ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_04b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_04bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_0390: Unknown result type (might be due to invalid IL or missing references)
			//IL_039d: Unknown result type (might be due to invalid IL or missing references)
			//IL_03aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_0355: Unknown result type (might be due to invalid IL or missing references)
			//IL_0362: Unknown result type (might be due to invalid IL or missing references)
			//IL_0378: Unknown result type (might be due to invalid IL or missing references)
			if (!active)
			{
				return;
			}
			Vector2 val = Main.MouseScreen;
			Vector2 val2 = position;
			if (PlayerInput.UsingGamepad && Main.SmartCursorWanted)
			{
				if (PlayerInput.GamepadThumbstickRight != Vector2.Zero)
				{
					val = position + PlayerInput.GamepadThumbstickRight * 40f;
				}
				else
				{
					val = ((!(PlayerInput.GamepadThumbstickLeft != Vector2.Zero)) ? position : (position + PlayerInput.GamepadThumbstickLeft * 40f));
				}
			}
			Vector2 val3 = val - val2;
			Vector2.Dot(Vector2.Normalize(val3), Vector2.UnitX);
			Vector2.Dot(Vector2.Normalize(val3), Vector2.UnitY);
			float num = val3.ToRotation();
			float num2 = val3.Length();
			bool flag = false;
			bool drawToolAllowActuators = Settings.DrawToolAllowActuators;
			float num3 = 4 + drawToolAllowActuators.ToInt();
			float num4 = (drawToolAllowActuators ? 11f : (-0.5f));
			for (int i = 0; i < 6; i++)
			{
				if (!drawToolAllowActuators && i == 5)
				{
					continue;
				}
				bool flag2 = ((uint)Settings.ToolMode & (uint)(1 << i)) != 0;
				if (i == 5)
				{
					flag2 = (Settings.ToolMode & Settings.MultiToolMode.Actuator) != 0;
				}
				Vector2 val4 = val2 + Vector2.UnitX * (45f * ((float)i - 1.5f));
				switch (i)
				{
				case 0:
				case 1:
				case 2:
				case 3:
				{
					float num5 = i;
					if (i == 0)
					{
						num5 = 3f;
					}
					if (i == 3)
					{
						num5 = 0f;
					}
					val4 = val2 + Vector2.UnitX.RotatedBy(num5 * ((float)Math.PI * 2f) / num3 - (float)Math.PI / num4) * 45f;
					break;
				}
				case 5:
					val4 = val2 + Vector2.UnitX.RotatedBy((float)(i - 1) * ((float)Math.PI * 2f) / num3 - (float)Math.PI / num4) * 45f;
					break;
				case 4:
					flag2 = false;
					val4 = val2;
					break;
				}
				bool flag3 = false;
				if (i == 4)
				{
					flag3 = num2 < 20f;
				}
				switch (i)
				{
				case 4:
					flag3 = num2 < 20f;
					break;
				case 0:
				case 1:
				case 2:
				case 3:
				case 5:
				{
					float value = (val4 - val2).ToRotation().AngleTowards(num, (float)Math.PI * 2f / (num3 * 2f)) - num;
					if (num2 >= 20f && Math.Abs(value) < 0.01f)
					{
						flag3 = true;
					}
					break;
				}
				}
				if (!PlayerInput.UsingGamepad)
				{
					flag3 = Vector2.Distance(val4, val) < 19f;
				}
				if (flag)
				{
					flag3 = false;
				}
				if (flag3)
				{
					flag = true;
				}
				Texture2D value2 = TextureAssets.WireUi[(((Settings.ToolMode & Settings.MultiToolMode.Cutter) != 0) ? 8 : 0) + (flag3 ? 1 : 0)].Value;
				Texture2D val5 = null;
				switch (i)
				{
				case 0:
				case 1:
				case 2:
				case 3:
					val5 = TextureAssets.WireUi[2 + i].Value;
					break;
				case 4:
					val5 = TextureAssets.WireUi[((Settings.ToolMode & Settings.MultiToolMode.Cutter) != 0) ? 7 : 6].Value;
					break;
				case 5:
					val5 = TextureAssets.WireUi[10].Value;
					break;
				}
				Color val6 = Color.White;
				Color val7 = Color.White;
				if (!flag2 && i != 4)
				{
					if (flag3)
					{
						val7 = new Color(100, 100, 100);
						val7 = new Color(120, 120, 120);
						val6 = new Color(200, 200, 200);
					}
					else
					{
						val7 = new Color(150, 150, 150);
						val7 = new Color(80, 80, 80);
						val6 = new Color(100, 100, 100);
					}
				}
				Utils.CenteredRectangle(val4, new Vector2(40f));
				if (flag3)
				{
					if (Main.mouseLeft && Main.mouseLeftRelease)
					{
						switch (i)
						{
						case 0:
							Settings.ToolMode ^= Settings.MultiToolMode.Red;
							break;
						case 1:
							Settings.ToolMode ^= Settings.MultiToolMode.Green;
							break;
						case 2:
							Settings.ToolMode ^= Settings.MultiToolMode.Blue;
							break;
						case 3:
							Settings.ToolMode ^= Settings.MultiToolMode.Yellow;
							break;
						case 4:
							Settings.ToolMode ^= Settings.MultiToolMode.Cutter;
							break;
						case 5:
							Settings.ToolMode ^= Settings.MultiToolMode.Actuator;
							break;
						}
					}
					Main.player[Main.myPlayer].mouseInterface = true;
					OnWiresMenu = true;
				}
				spriteBatch.Draw(value2, val4, (Rectangle?)null, val6, 0f, value2.Size() / 2f, 1f, (SpriteEffects)0, 0f);
				spriteBatch.Draw(val5, val4, (Rectangle?)null, val7, 0f, val5.Size() / 2f, 1f, (SpriteEffects)0, 0f);
			}
			if (Main.mouseLeft && Main.mouseLeftRelease && !flag)
			{
				active = false;
			}
		}

		private void DrawCursorArea(SpriteBatch spriteBatch)
		{
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_009f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			//IL_007c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0081: Unknown result type (might be due to invalid IL or missing references)
			//IL_0086: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			//IL_0069: Unknown result type (might be due to invalid IL or missing references)
			//IL_006e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0116: Unknown result type (might be due to invalid IL or missing references)
			//IL_0117: Unknown result type (might be due to invalid IL or missing references)
			//IL_012b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0130: Unknown result type (might be due to invalid IL or missing references)
			//IL_0135: Unknown result type (might be due to invalid IL or missing references)
			//IL_019c: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_020e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0215: Unknown result type (might be due to invalid IL or missing references)
			//IL_021a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0220: Unknown result type (might be due to invalid IL or missing references)
			//IL_0222: Unknown result type (might be due to invalid IL or missing references)
			//IL_022c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0231: Unknown result type (might be due to invalid IL or missing references)
			//IL_0313: Unknown result type (might be due to invalid IL or missing references)
			//IL_0324: Unknown result type (might be due to invalid IL or missing references)
			//IL_032b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0330: Unknown result type (might be due to invalid IL or missing references)
			//IL_0335: Unknown result type (might be due to invalid IL or missing references)
			//IL_0438: Unknown result type (might be due to invalid IL or missing references)
			//IL_0443: Unknown result type (might be due to invalid IL or missing references)
			//IL_044a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0451: Unknown result type (might be due to invalid IL or missing references)
			//IL_0456: Unknown result type (might be due to invalid IL or missing references)
			//IL_045b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0486: Unknown result type (might be due to invalid IL or missing references)
			//IL_0491: Unknown result type (might be due to invalid IL or missing references)
			//IL_0498: Unknown result type (might be due to invalid IL or missing references)
			//IL_049f: Unknown result type (might be due to invalid IL or missing references)
			//IL_04a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_04a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_04ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_04b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_04bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_04c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_04cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_04d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_045f: Unknown result type (might be due to invalid IL or missing references)
			//IL_046a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0471: Unknown result type (might be due to invalid IL or missing references)
			//IL_0478: Unknown result type (might be due to invalid IL or missing references)
			//IL_047d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0482: Unknown result type (might be due to invalid IL or missing references)
			//IL_0366: Unknown result type (might be due to invalid IL or missing references)
			//IL_0371: Unknown result type (might be due to invalid IL or missing references)
			//IL_0378: Unknown result type (might be due to invalid IL or missing references)
			//IL_037d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0382: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_03b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_03be: Unknown result type (might be due to invalid IL or missing references)
			//IL_03c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_03c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_03cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_03da: Unknown result type (might be due to invalid IL or missing references)
			//IL_03e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_03e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_03eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0389: Unknown result type (might be due to invalid IL or missing references)
			//IL_0394: Unknown result type (might be due to invalid IL or missing references)
			//IL_039b: Unknown result type (might be due to invalid IL or missing references)
			//IL_03a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_03a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_04d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_04d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_04d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_04de: Unknown result type (might be due to invalid IL or missing references)
			//IL_04e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_04e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_04ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_04f4: Unknown result type (might be due to invalid IL or missing references)
			//IL_04f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_0500: Unknown result type (might be due to invalid IL or missing references)
			//IL_0514: Unknown result type (might be due to invalid IL or missing references)
			//IL_0516: Unknown result type (might be due to invalid IL or missing references)
			//IL_051d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0521: Unknown result type (might be due to invalid IL or missing references)
			//IL_052b: Unknown result type (might be due to invalid IL or missing references)
			//IL_052d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0537: Unknown result type (might be due to invalid IL or missing references)
			//IL_03f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_03fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0404: Unknown result type (might be due to invalid IL or missing references)
			//IL_0409: Unknown result type (might be due to invalid IL or missing references)
			//IL_040e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0264: Unknown result type (might be due to invalid IL or missing references)
			//IL_026f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0276: Unknown result type (might be due to invalid IL or missing references)
			//IL_027b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0280: Unknown result type (might be due to invalid IL or missing references)
			//IL_02aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_02bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_02cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_02df: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0287: Unknown result type (might be due to invalid IL or missing references)
			//IL_0292: Unknown result type (might be due to invalid IL or missing references)
			//IL_0299: Unknown result type (might be due to invalid IL or missing references)
			//IL_029e: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_02fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0302: Unknown result type (might be due to invalid IL or missing references)
			//IL_0307: Unknown result type (might be due to invalid IL or missing references)
			//IL_030c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0555: Unknown result type (might be due to invalid IL or missing references)
			//IL_0557: Unknown result type (might be due to invalid IL or missing references)
			//IL_055e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0561: Unknown result type (might be due to invalid IL or missing references)
			//IL_056b: Unknown result type (might be due to invalid IL or missing references)
			//IL_056d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0577: Unknown result type (might be due to invalid IL or missing references)
			if (active || _lineOpacity == 0f)
			{
				return;
			}
			Vector2 val = Main.MouseScreen + new Vector2((float)(10 - 9 * PlayerInput.UsingGamepad.ToInt()), 25f);
			Color val2 = new Color(50, 50, 50);
			bool drawToolAllowActuators = Settings.DrawToolAllowActuators;
			if (!drawToolAllowActuators)
			{
				val = (PlayerInput.UsingGamepad ? (val + new Vector2(0f, 10f)) : (val + new Vector2(-20f, 10f)));
			}
			Texture2D value = TextureAssets.BuilderAcc.Value;
			Texture2D val3 = value;
			Rectangle val4 = new Rectangle(140, 2, 6, 6);
			Rectangle val5 = new Rectangle(148, 2, 6, 6);
			Rectangle val6 = new Rectangle(128, 0, 10, 10);
			float num = 1f;
			float num2 = 1f;
			bool flag = false;
			if (flag && !drawToolAllowActuators)
			{
				num *= Main.cursorScale;
			}
			float num3 = _lineOpacity;
			if (PlayerInput.UsingGamepad)
			{
				num3 *= Main.GamepadCursorAlpha;
			}
			for (int i = 0; i < 5; i++)
			{
				if (!drawToolAllowActuators && i == 4)
				{
					continue;
				}
				float num4 = num3;
				Vector2 vec = val + Vector2.UnitX * (45f * ((float)i - 1.5f));
				int num5 = i;
				if (i == 0)
				{
					num5 = 3;
				}
				if (i == 1)
				{
					num5 = 2;
				}
				if (i == 2)
				{
					num5 = 1;
				}
				if (i == 3)
				{
					num5 = 0;
				}
				if (i == 4)
				{
					num5 = 5;
				}
				int num6 = num5;
				switch (num6)
				{
				case 2:
					num6 = 1;
					break;
				case 1:
					num6 = 2;
					break;
				}
				bool flag2 = ((uint)Settings.ToolMode & (uint)(1 << num6)) != 0;
				if (num6 == 5)
				{
					flag2 = (Settings.ToolMode & Settings.MultiToolMode.Actuator) != 0;
				}
				Color val7 = Color.HotPink;
				switch (num5)
				{
				case 0:
					val7 = new Color(253, 58, 61);
					break;
				case 1:
					val7 = new Color(83, 180, 253);
					break;
				case 2:
					val7 = new Color(83, 253, 153);
					break;
				case 3:
					val7 = new Color(253, 254, 83);
					break;
				case 5:
					val7 = Color.WhiteSmoke;
					break;
				}
				if (!flag2)
				{
					val7 = Color.Lerp(val7, Color.Black, 0.65f);
				}
				if (flag)
				{
					if (drawToolAllowActuators)
					{
						switch (num5)
						{
						case 0:
							vec = val + new Vector2(-12f, 0f) * num;
							break;
						case 3:
							vec = val + new Vector2(12f, 0f) * num;
							break;
						case 1:
							vec = val + new Vector2(-6f, 12f) * num;
							break;
						case 2:
							vec = val + new Vector2(6f, 12f) * num;
							break;
						case 5:
							vec = val + new Vector2(0f, 0f) * num;
							break;
						}
					}
					else
					{
						vec = val + new Vector2((float)(12 * (num5 + 1)), (float)(12 * (3 - num5))) * num;
					}
				}
				else if (drawToolAllowActuators)
				{
					switch (num5)
					{
					case 0:
						vec = val + new Vector2(-12f, 0f) * num;
						break;
					case 3:
						vec = val + new Vector2(12f, 0f) * num;
						break;
					case 1:
						vec = val + new Vector2(-6f, 12f) * num;
						break;
					case 2:
						vec = val + new Vector2(6f, 12f) * num;
						break;
					case 5:
						vec = val + new Vector2(0f, 0f) * num;
						break;
					}
				}
				else
				{
					float num7 = 0.7f;
					switch (num5)
					{
					case 0:
						vec = val + new Vector2(0f, -12f) * num * num7;
						break;
					case 3:
						vec = val + new Vector2(12f, 0f) * num * num7;
						break;
					case 1:
						vec = val + new Vector2(-12f, 0f) * num * num7;
						break;
					case 2:
						vec = val + new Vector2(0f, 12f) * num * num7;
						break;
					}
				}
				vec = vec.Floor();
				spriteBatch.Draw(val3, vec, (Rectangle?)val6, val2 * num4, 0f, val6.Size() / 2f, num2, (SpriteEffects)0, 0f);
				spriteBatch.Draw(value, vec, (Rectangle?)val4, val7 * num4, 0f, val4.Size() / 2f, num2, (SpriteEffects)0, 0f);
				if ((Settings.ToolMode & Settings.MultiToolMode.Cutter) != 0)
				{
					spriteBatch.Draw(value, vec, (Rectangle?)val5, val2 * num4, 0f, val5.Size() / 2f, num2, (SpriteEffects)0, 0f);
				}
			}
		}
	}

	private static WiresRadial radial = new WiresRadial();

	public static bool Open => radial.active;

	public static void HandleWiresUI(SpriteBatch spriteBatch)
	{
		radial.Update();
		radial.Draw(spriteBatch);
	}
}
