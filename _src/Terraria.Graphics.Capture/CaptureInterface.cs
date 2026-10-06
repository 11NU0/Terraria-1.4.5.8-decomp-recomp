using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.GameContent.UI;
using Terraria.GameInput;
using Terraria.Localization;
using Terraria.UI.Chat;

namespace Terraria.Graphics.Capture;

public class CaptureInterface
{
	public static class Settings
	{
		public static bool PackImage = true;

		public static bool IncludeEntities = true;

		public static bool TransparentBackground;

		public static int BiomeChoiceIndex = -1;

		public static int ScreenAnchor = 0;

		public static Color MarkedAreaColor = new Color(0.8f, 0.8f, 0.8f, 0f) * 0.3f;

		static Settings()
		{
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		}
	}

	private abstract class CaptureInterfaceMode
	{
		public bool Selected;

		public abstract void Update();

		public abstract void Draw(SpriteBatch sb);

		public abstract void ToggleActive(bool tickedOn);

		public abstract bool UsingMap();

		protected void StartDrawingSelection(SpriteBatch sb)
		{
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			sb.End();
			sb.Begin((SpriteSortMode)0, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, (Effect)null, SelectionZoomMatrix);
			SetZoom_Context();
		}

		protected void EndDrawingSelection(SpriteBatch sb)
		{
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			sb.End();
			sb.Begin((SpriteSortMode)0, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, (Effect)null, Main.UIScaleMatrix);
			PlayerInput.SetZoom_UI();
		}
	}

	public enum SelectionContext
	{
		World,
		Map
	}

	private class ModeEdgeSelection : CaptureInterfaceMode
	{
		public override void Update()
		{
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			if (Selected)
			{
				SetZoom_Context();
				Vector2 mouse = new Vector2((float)Main.mouseX, (float)Main.mouseY);
				EdgePlacement(mouse);
			}
		}

		public override void Draw(SpriteBatch sb)
		{
			if (Selected)
			{
				StartDrawingSelection(sb);
				DrawMarkedArea(sb);
				DrawCursors(sb);
				EndDrawingSelection(sb);
			}
		}

		public override void ToggleActive(bool tickedOn)
		{
		}

		public override bool UsingMap()
		{
			return true;
		}

		private void EdgePlacement(Vector2 mouse)
		{
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_006e: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_0081: Unknown result type (might be due to invalid IL or missing references)
			//IL_0082: Unknown result type (might be due to invalid IL or missing references)
			if (JustActivated)
			{
				return;
			}
			Point result;
			if (!Main.mapFullscreen)
			{
				if (Main.mouseLeft)
				{
					EdgeAPinned = true;
					EdgeA = Main.MouseWorld.ToTileCoordinates();
				}
				if (Main.mouseRight)
				{
					EdgeBPinned = true;
					EdgeB = Main.MouseWorld.ToTileCoordinates();
				}
			}
			else if (GetMapCoords((int)mouse.X, (int)mouse.Y, 0, out result))
			{
				if (Main.mouseLeft)
				{
					EdgeAPinned = true;
					EdgeA = result;
				}
				if (Main.mouseRight)
				{
					EdgeBPinned = true;
					EdgeB = result;
				}
			}
			ConstraintPoints();
		}

		private void DrawMarkedArea(SpriteBatch sb)
		{
			//IL_01d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01df: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0204: Unknown result type (might be due to invalid IL or missing references)
			//IL_021b: Unknown result type (might be due to invalid IL or missing references)
			//IL_022b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0097: Unknown result type (might be due to invalid IL or missing references)
			//IL_009c: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00de: Unknown result type (might be due to invalid IL or missing references)
			//IL_0234: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_0240: Unknown result type (might be due to invalid IL or missing references)
			//IL_0248: Unknown result type (might be due to invalid IL or missing references)
			//IL_0260: Unknown result type (might be due to invalid IL or missing references)
			//IL_0262: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0113: Unknown result type (might be due to invalid IL or missing references)
			//IL_0115: Unknown result type (might be due to invalid IL or missing references)
			//IL_027f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0286: Unknown result type (might be due to invalid IL or missing references)
			//IL_0132: Unknown result type (might be due to invalid IL or missing references)
			//IL_0139: Unknown result type (might be due to invalid IL or missing references)
			//IL_0296: Unknown result type (might be due to invalid IL or missing references)
			//IL_0149: Unknown result type (might be due to invalid IL or missing references)
			//IL_029e: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_0151: Unknown result type (might be due to invalid IL or missing references)
			//IL_0159: Unknown result type (might be due to invalid IL or missing references)
			//IL_015e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0173: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_0183: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_018b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0193: Unknown result type (might be due to invalid IL or missing references)
			//IL_019a: Unknown result type (might be due to invalid IL or missing references)
			//IL_019f: Unknown result type (might be due to invalid IL or missing references)
			if (!EdgeAPinned || !EdgeBPinned)
			{
				return;
			}
			int num = Math.Min(EdgeA.X, EdgeB.X);
			int num2 = Math.Min(EdgeA.Y, EdgeB.Y);
			int num3 = Math.Abs(EdgeA.X - EdgeB.X);
			int num4 = Math.Abs(EdgeA.Y - EdgeB.Y);
			if (!Main.mapFullscreen)
			{
				Rectangle val = Main.ReverseGravitySupport(new Rectangle(num * 16, num2 * 16, (num3 + 1) * 16, (num4 + 1) * 16));
				Rectangle val2 = Main.ReverseGravitySupport(new Rectangle((int)Main.screenPosition.X, (int)Main.screenPosition.Y, Main.screenWidth + 1, Main.screenHeight + 1));
				Rectangle val3 = default;
				Rectangle.Intersect(ref val2, ref val, out val3);
				if (val3.Width != 0 && val3.Height != 0)
				{
					val3.Offset(-val2.X, -val2.Y);
					sb.Draw(TextureAssets.MagicPixel.Value, val3, Settings.MarkedAreaColor);
					for (int i = 0; i < 2; i++)
					{
						sb.Draw(TextureAssets.MagicPixel.Value, new Rectangle(val3.X, val3.Y + ((i == 1) ? val3.Height : (-2)), val3.Width, 2), Color.White);
						sb.Draw(TextureAssets.MagicPixel.Value, new Rectangle(val3.X + ((i == 1) ? val3.Width : (-2)), val3.Y, 2, val3.Height), Color.White);
					}
				}
				return;
			}
			GetMapCoords(num, num2, 1, out var result);
			GetMapCoords(num + num3 + 1, num2 + num4 + 1, 1, out var result2);
			Rectangle val4 = new Rectangle(result.X, result.Y, result2.X - result.X, result2.Y - result.Y);
			Rectangle val5 = new Rectangle(0, 0, Main.screenWidth + 1, Main.screenHeight + 1);
			Rectangle val6 = default;
			Rectangle.Intersect(ref val5, ref val4, out val6);
			if (val6.Width != 0 && val6.Height != 0)
			{
				val6.Offset(-val5.X, -val5.Y);
				sb.Draw(TextureAssets.MagicPixel.Value, val6, Settings.MarkedAreaColor);
				for (int j = 0; j < 2; j++)
				{
					sb.Draw(TextureAssets.MagicPixel.Value, new Rectangle(val6.X, val6.Y + ((j == 1) ? val6.Height : (-2)), val6.Width, 2), Color.White);
					sb.Draw(TextureAssets.MagicPixel.Value, new Rectangle(val6.X + ((j == 1) ? val6.Width : (-2)), val6.Y, 2, val6.Height), Color.White);
				}
			}
		}

		private void DrawCursors(SpriteBatch sb)
		{
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0074: Unknown result type (might be due to invalid IL or missing references)
			//IL_0079: Unknown result type (might be due to invalid IL or missing references)
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
			//IL_008e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0095: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00da: Unknown result type (might be due to invalid IL or missing references)
			//IL_00db: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			//IL_0069: Unknown result type (might be due to invalid IL or missing references)
			//IL_006e: Unknown result type (might be due to invalid IL or missing references)
			//IL_014f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0154: Unknown result type (might be due to invalid IL or missing references)
			//IL_0100: Unknown result type (might be due to invalid IL or missing references)
			//IL_0134: Unknown result type (might be due to invalid IL or missing references)
			//IL_0302: Unknown result type (might be due to invalid IL or missing references)
			//IL_0307: Unknown result type (might be due to invalid IL or missing references)
			//IL_0160: Unknown result type (might be due to invalid IL or missing references)
			//IL_0165: Unknown result type (might be due to invalid IL or missing references)
			//IL_016f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0174: Unknown result type (might be due to invalid IL or missing references)
			//IL_03e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_0367: Unknown result type (might be due to invalid IL or missing references)
			//IL_036e: Unknown result type (might be due to invalid IL or missing references)
			//IL_037e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0383: Unknown result type (might be due to invalid IL or missing references)
			//IL_03a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_03aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_03bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_03c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_03c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_03c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_03c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_03c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_03cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_03d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_03d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_03d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_024e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0253: Unknown result type (might be due to invalid IL or missing references)
			//IL_0255: Unknown result type (might be due to invalid IL or missing references)
			//IL_0257: Unknown result type (might be due to invalid IL or missing references)
			//IL_025c: Unknown result type (might be due to invalid IL or missing references)
			//IL_025e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0260: Unknown result type (might be due to invalid IL or missing references)
			//IL_0261: Unknown result type (might be due to invalid IL or missing references)
			//IL_0262: Unknown result type (might be due to invalid IL or missing references)
			//IL_0267: Unknown result type (might be due to invalid IL or missing references)
			//IL_0269: Unknown result type (might be due to invalid IL or missing references)
			//IL_026e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0278: Unknown result type (might be due to invalid IL or missing references)
			//IL_0282: Unknown result type (might be due to invalid IL or missing references)
			//IL_0287: Unknown result type (might be due to invalid IL or missing references)
			//IL_028c: Unknown result type (might be due to invalid IL or missing references)
			//IL_028e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0293: Unknown result type (might be due to invalid IL or missing references)
			//IL_0295: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_0183: Unknown result type (might be due to invalid IL or missing references)
			//IL_0185: Unknown result type (might be due to invalid IL or missing references)
			//IL_018f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0194: Unknown result type (might be due to invalid IL or missing references)
			//IL_0199: Unknown result type (might be due to invalid IL or missing references)
			//IL_019b: Unknown result type (might be due to invalid IL or missing references)
			//IL_019d: Unknown result type (might be due to invalid IL or missing references)
			//IL_019f: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_046f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0474: Unknown result type (might be due to invalid IL or missing references)
			//IL_0424: Unknown result type (might be due to invalid IL or missing references)
			//IL_0458: Unknown result type (might be due to invalid IL or missing references)
			//IL_03fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_040e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0410: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_0624: Unknown result type (might be due to invalid IL or missing references)
			//IL_0629: Unknown result type (might be due to invalid IL or missing references)
			//IL_0480: Unknown result type (might be due to invalid IL or missing references)
			//IL_0485: Unknown result type (might be due to invalid IL or missing references)
			//IL_048f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0494: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_02df: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_070d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0714: Unknown result type (might be due to invalid IL or missing references)
			//IL_068f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0696: Unknown result type (might be due to invalid IL or missing references)
			//IL_06a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_06ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_06cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_06d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_06e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_06e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_06e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_06eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_06ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_06ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_06ef: Unknown result type (might be due to invalid IL or missing references)
			//IL_06f4: Unknown result type (might be due to invalid IL or missing references)
			//IL_06f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_06f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_06fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_06ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_0574: Unknown result type (might be due to invalid IL or missing references)
			//IL_0579: Unknown result type (might be due to invalid IL or missing references)
			//IL_057b: Unknown result type (might be due to invalid IL or missing references)
			//IL_057d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0582: Unknown result type (might be due to invalid IL or missing references)
			//IL_0584: Unknown result type (might be due to invalid IL or missing references)
			//IL_0586: Unknown result type (might be due to invalid IL or missing references)
			//IL_0587: Unknown result type (might be due to invalid IL or missing references)
			//IL_0588: Unknown result type (might be due to invalid IL or missing references)
			//IL_058d: Unknown result type (might be due to invalid IL or missing references)
			//IL_058f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0594: Unknown result type (might be due to invalid IL or missing references)
			//IL_059e: Unknown result type (might be due to invalid IL or missing references)
			//IL_05a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_05ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_05b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_05b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_05b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_05bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_05c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_05c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_04a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_04a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_04af: Unknown result type (might be due to invalid IL or missing references)
			//IL_04b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_04b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_04bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_04bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_04bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_04c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_04d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_04dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_04e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_04e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_04eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0725: Unknown result type (might be due to invalid IL or missing references)
			//IL_0736: Unknown result type (might be due to invalid IL or missing references)
			//IL_0738: Unknown result type (might be due to invalid IL or missing references)
			//IL_05d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_05d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_05d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_0500: Unknown result type (might be due to invalid IL or missing references)
			//IL_0502: Unknown result type (might be due to invalid IL or missing references)
			//IL_0503: Unknown result type (might be due to invalid IL or missing references)
			//IL_0504: Unknown result type (might be due to invalid IL or missing references)
			//IL_0509: Unknown result type (might be due to invalid IL or missing references)
			//IL_050b: Unknown result type (might be due to invalid IL or missing references)
			//IL_050d: Unknown result type (might be due to invalid IL or missing references)
			//IL_05f4: Unknown result type (might be due to invalid IL or missing references)
			//IL_0605: Unknown result type (might be due to invalid IL or missing references)
			//IL_0607: Unknown result type (might be due to invalid IL or missing references)
			//IL_060c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0616: Unknown result type (might be due to invalid IL or missing references)
			//IL_0519: Unknown result type (might be due to invalid IL or missing references)
			//IL_051b: Unknown result type (might be due to invalid IL or missing references)
			//IL_051d: Unknown result type (might be due to invalid IL or missing references)
			float num = 1f / Main.cursorScale;
			float num2 = 0.8f / num;
			Vector2 val = Main.screenPosition + new Vector2(30f);
			Vector2 val2 = val + new Vector2((float)Main.screenWidth, (float)Main.screenHeight) - new Vector2(60f);
			if (Main.mapFullscreen)
			{
				val -= Main.screenPosition;
				val2 -= Main.screenPosition;
			}
			Vector3 val3 = Main.rgbToHsl(Main.cursorColor);
			Color val4 = Main.hslToRgb((val3.X + 0.33f) % 1f, val3.Y, val3.Z);
			Color val5 = Main.hslToRgb((val3.X - 0.33f) % 1f, val3.Y, val3.Z);
			val4 = (val5 = Color.White);
			bool flag = Main.player[Main.myPlayer].gravDir == -1f;
			if (!EdgeAPinned)
			{
				Utils.DrawCursorSingle(sb, val4, 3.926991f, Main.cursorScale * num * num2, new Vector2((float)Main.mouseX - 5f + 12f, (float)Main.mouseY + 2.5f + 12f), 4);
			}
			else
			{
				int specialMode = 0;
				float num3 = 0f;
				Vector2 zero = Vector2.Zero;
				if (!Main.mapFullscreen)
				{
					Vector2 val6 = EdgeA.ToVector2() * 16f;
					if (!EdgeBPinned)
					{
						specialMode = 1;
						val6 += Vector2.One * 8f;
						zero = val6;
						num3 = (-val6 + Main.ReverseGravitySupport(new Vector2((float)Main.mouseX, (float)Main.mouseY)) + Main.screenPosition).ToRotation();
						if (flag)
						{
							num3 = 0f - num3;
						}
						zero = Vector2.Clamp(val6, val, val2);
						if (zero != val6)
						{
							num3 = (val6 - zero).ToRotation();
						}
					}
					else
					{
						Vector2 val7 = new Vector2((float)((EdgeA.X > EdgeB.X).ToInt() * 16), (float)((EdgeA.Y > EdgeB.Y).ToInt() * 16));
						val6 += val7;
						zero = Vector2.Clamp(val6, val, val2);
						num3 = (EdgeB.ToVector2() * 16f + new Vector2(16f) - val7 - zero).ToRotation();
						if (zero != val6)
						{
							num3 = (val6 - zero).ToRotation();
							specialMode = 1;
						}
						if (flag)
						{
							num3 *= -1f;
						}
					}
					Utils.DrawCursorSingle(sb, val4, num3 - (float)Math.PI / 2f, Main.cursorScale * num, Main.ReverseGravitySupport(zero - Main.screenPosition), 4, specialMode);
				}
				else
				{
					Point result = EdgeA;
					if (EdgeBPinned)
					{
						int num4 = (EdgeA.X > EdgeB.X).ToInt();
						int num5 = (EdgeA.Y > EdgeB.Y).ToInt();
						result.X += num4;
						result.Y += num5;
						GetMapCoords(result.X, result.Y, 1, out result);
						Point result2 = EdgeB;
						result2.X += 1 - num4;
						result2.Y += 1 - num5;
						GetMapCoords(result2.X, result2.Y, 1, out result2);
						zero = result.ToVector2();
						zero = Vector2.Clamp(zero, val, val2);
						num3 = (result2.ToVector2() - zero).ToRotation();
					}
					else
					{
						GetMapCoords(result.X, result.Y, 1, out result);
					}
					Utils.DrawCursorSingle(sb, val4, num3 - (float)Math.PI / 2f, Main.cursorScale * num, result.ToVector2(), 4);
				}
			}
			if (!EdgeBPinned)
			{
				Utils.DrawCursorSingle(sb, val5, 0.7853981f, Main.cursorScale * num * num2, new Vector2((float)Main.mouseX + 2.5f + 12f, (float)Main.mouseY - 5f + 12f), 5);
				return;
			}
			int specialMode2 = 0;
			float num6 = 0f;
			Vector2 zero2 = Vector2.Zero;
			if (!Main.mapFullscreen)
			{
				Vector2 val8 = EdgeB.ToVector2() * 16f;
				if (!EdgeAPinned)
				{
					specialMode2 = 1;
					val8 += Vector2.One * 8f;
					zero2 = val8;
					num6 = (-val8 + Main.ReverseGravitySupport(new Vector2((float)Main.mouseX, (float)Main.mouseY)) + Main.screenPosition).ToRotation();
					if (flag)
					{
						num6 = 0f - num6;
					}
					zero2 = Vector2.Clamp(val8, val, val2);
					if (zero2 != val8)
					{
						num6 = (val8 - zero2).ToRotation();
					}
				}
				else
				{
					Vector2 val9 = new Vector2((float)((EdgeB.X >= EdgeA.X).ToInt() * 16), (float)((EdgeB.Y >= EdgeA.Y).ToInt() * 16));
					val8 += val9;
					zero2 = Vector2.Clamp(val8, val, val2);
					num6 = (EdgeA.ToVector2() * 16f + new Vector2(16f) - val9 - zero2).ToRotation();
					if (zero2 != val8)
					{
						num6 = (val8 - zero2).ToRotation();
						specialMode2 = 1;
					}
					if (flag)
					{
						num6 *= -1f;
					}
				}
				Utils.DrawCursorSingle(sb, val5, num6 - (float)Math.PI / 2f, Main.cursorScale * num, Main.ReverseGravitySupport(zero2 - Main.screenPosition), 5, specialMode2);
			}
			else
			{
				Point result3 = EdgeB;
				if (EdgeAPinned)
				{
					int num7 = (EdgeB.X >= EdgeA.X).ToInt();
					int num8 = (EdgeB.Y >= EdgeA.Y).ToInt();
					result3.X += num7;
					result3.Y += num8;
					GetMapCoords(result3.X, result3.Y, 1, out result3);
					Point result4 = EdgeA;
					result4.X += 1 - num7;
					result4.Y += 1 - num8;
					GetMapCoords(result4.X, result4.Y, 1, out result4);
					zero2 = result3.ToVector2();
					zero2 = Vector2.Clamp(zero2, val, val2);
					num6 = (result4.ToVector2() - zero2).ToRotation();
				}
				else
				{
					GetMapCoords(result3.X, result3.Y, 1, out result3);
				}
				Utils.DrawCursorSingle(sb, val5, num6 - (float)Math.PI / 2f, Main.cursorScale * num, result3.ToVector2(), 5);
			}
		}
	}

	private class ModeDragBounds : CaptureInterfaceMode
	{
		public int currentAim = -1;

		private bool dragging;

		private int caughtEdge = -1;

		private bool inMap;

		public override void Update()
		{
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			if (Selected && !JustActivated)
			{
				SetZoom_Context();
				Vector2 mouse = new Vector2((float)Main.mouseX, (float)Main.mouseY);
				DragBounds(mouse);
			}
		}

		public override void Draw(SpriteBatch sb)
		{
			if (Selected)
			{
				StartDrawingSelection(sb);
				DrawMarkedArea(sb);
				EndDrawingSelection(sb);
			}
		}

		public override void ToggleActive(bool tickedOn)
		{
			if (!tickedOn)
			{
				currentAim = -1;
			}
		}

		public override bool UsingMap()
		{
			return caughtEdge != -1;
		}

		private void DragBounds(Vector2 mouse)
		{
			//IL_01dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fa: Unknown result type (might be due to invalid IL or missing references)
			//IL_0201: Unknown result type (might be due to invalid IL or missing references)
			//IL_0209: Unknown result type (might be due to invalid IL or missing references)
			//IL_0220: Unknown result type (might be due to invalid IL or missing references)
			//IL_0230: Unknown result type (might be due to invalid IL or missing references)
			//IL_0147: Unknown result type (might be due to invalid IL or missing references)
			//IL_014c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0151: Unknown result type (might be due to invalid IL or missing references)
			//IL_0177: Unknown result type (might be due to invalid IL or missing references)
			//IL_017c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0181: Unknown result type (might be due to invalid IL or missing references)
			//IL_018e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0239: Unknown result type (might be due to invalid IL or missing references)
			//IL_0197: Unknown result type (might be due to invalid IL or missing references)
			//IL_0245: Unknown result type (might be due to invalid IL or missing references)
			//IL_024d: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_0069: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			//IL_007d: Unknown result type (might be due to invalid IL or missing references)
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0354: Unknown result type (might be due to invalid IL or missing references)
			//IL_0356: Unknown result type (might be due to invalid IL or missing references)
			//IL_035a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0362: Unknown result type (might be due to invalid IL or missing references)
			//IL_0372: Unknown result type (might be due to invalid IL or missing references)
			//IL_0373: Unknown result type (might be due to invalid IL or missing references)
			//IL_0284: Unknown result type (might be due to invalid IL or missing references)
			//IL_029f: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_0291: Unknown result type (might be due to invalid IL or missing references)
			//IL_0296: Unknown result type (might be due to invalid IL or missing references)
			//IL_029b: Unknown result type (might be due to invalid IL or missing references)
			//IL_038b: Unknown result type (might be due to invalid IL or missing references)
			//IL_038f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0394: Unknown result type (might be due to invalid IL or missing references)
			//IL_03a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_03a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_0301: Unknown result type (might be due to invalid IL or missing references)
			//IL_031f: Unknown result type (might be due to invalid IL or missing references)
			//IL_033c: Unknown result type (might be due to invalid IL or missing references)
			if (!EdgeAPinned || !EdgeBPinned)
			{
				bool flag = false;
				if (Main.mouseLeft)
				{
					flag = true;
				}
				if (flag)
				{
					bool flag2 = true;
					Point result;
					if (!Main.mapFullscreen)
					{
						result = (Main.screenPosition + mouse).ToTileCoordinates();
					}
					else
					{
						flag2 = GetMapCoords((int)mouse.X, (int)mouse.Y, 0, out result);
					}
					if (flag2)
					{
						if (!EdgeAPinned)
						{
							EdgeAPinned = true;
							EdgeA = result;
						}
						if (!EdgeBPinned)
						{
							EdgeBPinned = true;
							EdgeB = result;
						}
					}
					currentAim = 3;
					caughtEdge = 1;
				}
			}
			int num = Math.Min(EdgeA.X, EdgeB.X);
			int num2 = Math.Min(EdgeA.Y, EdgeB.Y);
			int num3 = Math.Abs(EdgeA.X - EdgeB.X);
			int num4 = Math.Abs(EdgeA.Y - EdgeB.Y);
			bool value = Main.player[Main.myPlayer].gravDir == -1f;
			int num5 = 1 - value.ToInt();
			int num6 = value.ToInt();
			Rectangle val;
			Rectangle val2;
			Rectangle val3 = default;
			if (!Main.mapFullscreen)
			{
				val = Main.ReverseGravitySupport(new Rectangle(num * 16, num2 * 16, (num3 + 1) * 16, (num4 + 1) * 16));
				val2 = Main.ReverseGravitySupport(new Rectangle((int)Main.screenPosition.X, (int)Main.screenPosition.Y, Main.screenWidth + 1, Main.screenHeight + 1));
				Rectangle.Intersect(ref val2, ref val, out val3);
				if (val3.Width == 0 || val3.Height == 0)
				{
					return;
				}
				val3.Offset(-val2.X, -val2.Y);
			}
			else
			{
				GetMapCoords(num, num2, 1, out var result2);
				GetMapCoords(num + num3 + 1, num2 + num4 + 1, 1, out var result3);
				val = new Rectangle(result2.X, result2.Y, result3.X - result2.X, result3.Y - result2.Y);
				val2 = new Rectangle(0, 0, Main.screenWidth + 1, Main.screenHeight + 1);
				Rectangle.Intersect(ref val2, ref val, out val3);
				if (val3.Width == 0 || val3.Height == 0)
				{
					return;
				}
				val3.Offset(-val2.X, -val2.Y);
			}
			dragging = false;
			if (!Main.mouseLeft)
			{
				currentAim = -1;
			}
			if (currentAim != -1)
			{
				dragging = true;
				Point val4 = default;
				if (!Main.mapFullscreen)
				{
					val4 = Main.MouseWorld.ToTileCoordinates();
				}
				else
				{
					if (!GetMapCoords((int)mouse.X, (int)mouse.Y, 0, out var result4))
					{
						return;
					}
					val4 = result4;
				}
				switch (currentAim)
				{
				case 0:
				case 1:
					if (caughtEdge == 0)
					{
						EdgeA.Y = val4.Y;
					}
					if (caughtEdge == 1)
					{
						EdgeB.Y = val4.Y;
					}
					break;
				case 2:
				case 3:
					if (caughtEdge == 0)
					{
						EdgeA.X = val4.X;
					}
					if (caughtEdge == 1)
					{
						EdgeB.X = val4.X;
					}
					break;
				}
			}
			else
			{
				caughtEdge = -1;
				Rectangle drawbox = val;
				drawbox.Offset(-val2.X, -val2.Y);
				inMap = drawbox.Contains(mouse.ToPoint());
				for (int i = 0; i < 4; i++)
				{
					Rectangle bound = GetBound(drawbox, i);
					bound.Inflate(8, 8);
					if (!bound.Contains(mouse.ToPoint()))
					{
						continue;
					}
					currentAim = i;
					switch (i)
					{
					case 0:
						if (EdgeA.Y < EdgeB.Y)
						{
							caughtEdge = num6;
						}
						else
						{
							caughtEdge = num5;
						}
						break;
					case 1:
						if (EdgeA.Y >= EdgeB.Y)
						{
							caughtEdge = num6;
						}
						else
						{
							caughtEdge = num5;
						}
						break;
					case 2:
						if (EdgeA.X < EdgeB.X)
						{
							caughtEdge = 0;
						}
						else
						{
							caughtEdge = 1;
						}
						break;
					case 3:
						if (EdgeA.X >= EdgeB.X)
						{
							caughtEdge = 0;
						}
						else
						{
							caughtEdge = 1;
						}
						break;
					}
					break;
				}
			}
			ConstraintPoints();
		}

		private Rectangle GetBound(Rectangle drawbox, int boundIndex)
		{
			//IL_0003: Unknown result type (might be due to invalid IL or missing references)
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0085: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0072: Unknown result type (might be due to invalid IL or missing references)
			//IL_0079: Unknown result type (might be due to invalid IL or missing references)
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			return boundIndex switch
			{
				0 => new Rectangle(drawbox.X, drawbox.Y - 2, drawbox.Width, 2), 
				1 => new Rectangle(drawbox.X, drawbox.Y + drawbox.Height, drawbox.Width, 2), 
				2 => new Rectangle(drawbox.X - 2, drawbox.Y, 2, drawbox.Height), 
				3 => new Rectangle(drawbox.X + drawbox.Width, drawbox.Y, 2, drawbox.Height), 
				_ => Rectangle.Empty, 
			};
		}

		public void DrawMarkedArea(SpriteBatch sb)
		{
			//IL_012d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0134: Unknown result type (might be due to invalid IL or missing references)
			//IL_013b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0142: Unknown result type (might be due to invalid IL or missing references)
			//IL_014a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0151: Unknown result type (might be due to invalid IL or missing references)
			//IL_0159: Unknown result type (might be due to invalid IL or missing references)
			//IL_0170: Unknown result type (might be due to invalid IL or missing references)
			//IL_0180: Unknown result type (might be due to invalid IL or missing references)
			//IL_0097: Unknown result type (might be due to invalid IL or missing references)
			//IL_009c: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00de: Unknown result type (might be due to invalid IL or missing references)
			//IL_0189: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_0195: Unknown result type (might be due to invalid IL or missing references)
			//IL_019d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_02bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_02be: Unknown result type (might be due to invalid IL or missing references)
			//IL_0212: Unknown result type (might be due to invalid IL or missing references)
			//IL_0219: Unknown result type (might be due to invalid IL or missing references)
			//IL_01dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_02cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0229: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_0231: Unknown result type (might be due to invalid IL or missing references)
			//IL_0239: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0203: Unknown result type (might be due to invalid IL or missing references)
			//IL_0282: Unknown result type (might be due to invalid IL or missing references)
			//IL_024c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0292: Unknown result type (might be due to invalid IL or missing references)
			//IL_025c: Unknown result type (might be due to invalid IL or missing references)
			//IL_029a: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0264: Unknown result type (might be due to invalid IL or missing references)
			//IL_026c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0273: Unknown result type (might be due to invalid IL or missing references)
			if (!EdgeAPinned || !EdgeBPinned)
			{
				return;
			}
			int num = Math.Min(EdgeA.X, EdgeB.X);
			int num2 = Math.Min(EdgeA.Y, EdgeB.Y);
			int num3 = Math.Abs(EdgeA.X - EdgeB.X);
			int num4 = Math.Abs(EdgeA.Y - EdgeB.Y);
			Rectangle val3 = default;
			if (!Main.mapFullscreen)
			{
				Rectangle val = Main.ReverseGravitySupport(new Rectangle(num * 16, num2 * 16, (num3 + 1) * 16, (num4 + 1) * 16));
				Rectangle val2 = Main.ReverseGravitySupport(new Rectangle((int)Main.screenPosition.X, (int)Main.screenPosition.Y, Main.screenWidth + 1, Main.screenHeight + 1));
				Rectangle.Intersect(ref val2, ref val, out val3);
				if (val3.Width == 0 || val3.Height == 0)
				{
					return;
				}
				val3.Offset(-val2.X, -val2.Y);
			}
			else
			{
				GetMapCoords(num, num2, 1, out var result);
				GetMapCoords(num + num3 + 1, num2 + num4 + 1, 1, out var result2);
				Rectangle val = new Rectangle(result.X, result.Y, result2.X - result.X, result2.Y - result.Y);
				Rectangle val2 = new Rectangle(0, 0, Main.screenWidth + 1, Main.screenHeight + 1);
				Rectangle.Intersect(ref val2, ref val, out val3);
				if (val3.Width == 0 || val3.Height == 0)
				{
					return;
				}
				val3.Offset(-val2.X, -val2.Y);
			}
			sb.Draw(TextureAssets.MagicPixel.Value, val3, Settings.MarkedAreaColor);
			Rectangle val4 = Rectangle.Empty;
			for (int i = 0; i < 2; i++)
			{
				if (currentAim != i)
				{
					DrawBound(sb, new Rectangle(val3.X, val3.Y + ((i == 1) ? val3.Height : (-2)), val3.Width, 2), 0);
				}
				else
				{
					val4 = new Rectangle(val3.X, val3.Y + ((i == 1) ? val3.Height : (-2)), val3.Width, 2);
				}
				if (currentAim != i + 2)
				{
					DrawBound(sb, new Rectangle(val3.X + ((i == 1) ? val3.Width : (-2)), val3.Y, 2, val3.Height), 0);
				}
				else
				{
					val4 = new Rectangle(val3.X + ((i == 1) ? val3.Width : (-2)), val3.Y, 2, val3.Height);
				}
			}
			if (val4 != Rectangle.Empty)
			{
				DrawBound(sb, val4, 1 + dragging.ToInt());
			}
		}

		private void DrawBound(SpriteBatch sb, Rectangle r, int mode)
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0070: Unknown result type (might be due to invalid IL or missing references)
			//IL_0078: Unknown result type (might be due to invalid IL or missing references)
			//IL_0088: Unknown result type (might be due to invalid IL or missing references)
			//IL_0089: Unknown result type (might be due to invalid IL or missing references)
			//IL_009e: Unknown result type (might be due to invalid IL or missing references)
			//IL_009f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00df: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
			//IL_0100: Unknown result type (might be due to invalid IL or missing references)
			//IL_0108: Unknown result type (might be due to invalid IL or missing references)
			//IL_0118: Unknown result type (might be due to invalid IL or missing references)
			//IL_0119: Unknown result type (might be due to invalid IL or missing references)
			//IL_012e: Unknown result type (might be due to invalid IL or missing references)
			//IL_012f: Unknown result type (might be due to invalid IL or missing references)
			switch (mode)
			{
			case 0:
				sb.Draw(TextureAssets.MagicPixel.Value, r, Color.Silver);
				break;
			case 1:
			{
				Rectangle val2 = new Rectangle(r.X - 2, r.Y, r.Width + 4, r.Height);
				sb.Draw(TextureAssets.MagicPixel.Value, val2, Color.White);
				val2 = new Rectangle(r.X, r.Y - 2, r.Width, r.Height + 4);
				sb.Draw(TextureAssets.MagicPixel.Value, val2, Color.White);
				sb.Draw(TextureAssets.MagicPixel.Value, r, Color.White);
				break;
			}
			case 2:
			{
				Rectangle val = new Rectangle(r.X - 2, r.Y, r.Width + 4, r.Height);
				sb.Draw(TextureAssets.MagicPixel.Value, val, Color.Gold);
				val = new Rectangle(r.X, r.Y - 2, r.Width, r.Height + 4);
				sb.Draw(TextureAssets.MagicPixel.Value, val, Color.Gold);
				sb.Draw(TextureAssets.MagicPixel.Value, r, Color.Gold);
				break;
			}
			}
		}
	}

	private class ModeChangeSettings : CaptureInterfaceMode
	{
		private const int ButtonsCount = 7;

		private int hoveredButton = -1;

		private bool inUI;

		private Rectangle GetRect()
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			Rectangle val = new Rectangle(0, 0, 224, 170);
			if (Settings.ScreenAnchor == 0)
			{
				val.X = 227 - val.Width / 2;
				val.Y = 80;
			}
			return val;
		}

		private void ButtonDraw(int button, ref string key, ref string value)
		{
			switch (button)
			{
			case 0:
				key = Lang.inter[74].Value;
				value = Lang.inter[73 - Settings.PackImage.ToInt()].Value;
				break;
			case 1:
				key = Lang.inter[75].Value;
				value = Lang.inter[73 - Settings.IncludeEntities.ToInt()].Value;
				break;
			case 2:
				key = Lang.inter[76].Value;
				value = Lang.inter[73 - (!Settings.TransparentBackground).ToInt()].Value;
				break;
			case 6:
				key = "      " + Lang.menu[86].Value;
				value = "";
				break;
			case 3:
			case 4:
			case 5:
				break;
			}
		}

		private void PressButton(int button)
		{
			bool flag = false;
			switch (button)
			{
			case 0:
				Settings.PackImage = !Settings.PackImage;
				flag = true;
				break;
			case 1:
				Settings.IncludeEntities = !Settings.IncludeEntities;
				flag = true;
				break;
			case 2:
				Settings.TransparentBackground = !Settings.TransparentBackground;
				flag = true;
				break;
			case 6:
				Settings.PackImage = true;
				Settings.IncludeEntities = true;
				Settings.TransparentBackground = false;
				Settings.BiomeChoiceIndex = -1;
				flag = true;
				break;
			}
			if (flag)
			{
				SoundEngine.PlaySound(12);
			}
		}

		private void DrawWaterChoices(SpriteBatch spritebatch, Point start, Point mouse)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0150: Unknown result type (might be due to invalid IL or missing references)
			//IL_0151: Unknown result type (might be due to invalid IL or missing references)
			//IL_015a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0164: Unknown result type (might be due to invalid IL or missing references)
			//IL_016b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0175: Unknown result type (might be due to invalid IL or missing references)
			//IL_017f: Unknown result type (might be due to invalid IL or missing references)
			//IL_010b: Unknown result type (might be due to invalid IL or missing references)
			//IL_010c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0115: Unknown result type (might be due to invalid IL or missing references)
			//IL_011f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0129: Unknown result type (might be due to invalid IL or missing references)
			//IL_0133: Unknown result type (might be due to invalid IL or missing references)
			//IL_0192: Unknown result type (might be due to invalid IL or missing references)
			//IL_0193: Unknown result type (might be due to invalid IL or missing references)
			//IL_019d: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bf: Unknown result type (might be due to invalid IL or missing references)
			Rectangle r = new Rectangle(0, 0, 20, 20);
			for (int i = 0; i < 2; i++)
			{
				for (int j = 0; j < 7; j++)
				{
					if (i == 1 && j == 6)
					{
						continue;
					}
					int num = j + i * 7;
					r.X = start.X + 24 * j + 12 * i;
					r.Y = start.Y + 24 * i;
					int num2 = num;
					int num3 = 0;
					if (r.Contains(mouse))
					{
						if (Main.mouseLeft && Main.mouseLeftRelease)
						{
							SoundEngine.PlaySound(12);
							Settings.BiomeChoiceIndex = num2;
						}
						Main.instance.MouseText(Language.GetTextValue("CaptureBiomeChoice." + num2), 0, 0);
						num3++;
					}
					if (Settings.BiomeChoiceIndex == num2)
					{
						num3 += 2;
					}
					Texture2D value = TextureAssets.Extra[130].Value;
					int num4 = num * 18;
					_ = Color.White;
					float num5 = 1f;
					if (num3 < 2)
					{
						num5 *= 0.5f;
					}
					if (num3 % 2 == 1)
					{
						spritebatch.Draw(TextureAssets.MagicPixel.Value, r.TopLeft(), (Rectangle?)new Rectangle(0, 0, 1, 1), Color.Gold, 0f, Vector2.Zero, new Vector2(20f), (SpriteEffects)0, 0f);
					}
					else
					{
						spritebatch.Draw(TextureAssets.MagicPixel.Value, r.TopLeft(), (Rectangle?)new Rectangle(0, 0, 1, 1), Color.White * num5, 0f, Vector2.Zero, new Vector2(20f), (SpriteEffects)0, 0f);
					}
					spritebatch.Draw(value, r.TopLeft() + new Vector2(2f), (Rectangle?)new Rectangle(num4, 0, 16, 16), Color.White * num5);
				}
			}
		}

		private int UnnecessaryBiomeSelectionTypeConversion(int index)
		{
			switch (index)
			{
			case 0:
				return -1;
			case 1:
				return 0;
			case 2:
				return 2;
			case 3:
			case 4:
			case 5:
			case 6:
			case 7:
			case 8:
				return index;
			case 9:
				return 10;
			case 10:
				return 12;
			case 11:
				return 13;
			case 12:
				return 14;
			default:
				return 0;
			}
		}

		public override void Update()
		{
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			if (!Selected || JustActivated)
			{
				return;
			}
			PlayerInput.SetZoom_UI();
			Point val = new Point(Main.mouseX, Main.mouseY);
			hoveredButton = -1;
			Rectangle rect = GetRect();
			inUI = rect.Contains(val);
			rect.Inflate(-20, -20);
			rect.Height = 16;
			int y = rect.Y;
			for (int i = 0; i < 7; i++)
			{
				rect.Y = y + i * 20;
				if (rect.Contains(val))
				{
					hoveredButton = i;
					break;
				}
			}
			if (Main.mouseLeft && Main.mouseLeftRelease && hoveredButton != -1)
			{
				PressButton(hoveredButton);
			}
		}

		public override void Draw(SpriteBatch sb)
		{
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			//IL_008c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0091: Unknown result type (might be due to invalid IL or missing references)
			//IL_0157: Unknown result type (might be due to invalid IL or missing references)
			//IL_0158: Unknown result type (might be due to invalid IL or missing references)
			//IL_015d: Unknown result type (might be due to invalid IL or missing references)
			//IL_016e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0173: Unknown result type (might be due to invalid IL or missing references)
			//IL_0178: Unknown result type (might be due to invalid IL or missing references)
			//IL_017d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0182: Unknown result type (might be due to invalid IL or missing references)
			//IL_00af: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_010b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0110: Unknown result type (might be due to invalid IL or missing references)
			//IL_0115: Unknown result type (might be due to invalid IL or missing references)
			//IL_0127: Unknown result type (might be due to invalid IL or missing references)
			//IL_012c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0131: Unknown result type (might be due to invalid IL or missing references)
			//IL_0136: Unknown result type (might be due to invalid IL or missing references)
			//IL_009c: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
			if (!Selected)
			{
				return;
			}
			StartDrawingSelection(sb);
			((ModeDragBounds)Modes[1]).currentAim = -1;
			((ModeDragBounds)Modes[1]).DrawMarkedArea(sb);
			EndDrawingSelection(sb);
			Rectangle rect = GetRect();
			Utils.DrawInvBG(sb, rect, new Color(63, 65, 151, 255) * 0.485f);
			for (int i = 0; i < 7; i++)
			{
				string key = "";
				string value = "";
				ButtonDraw(i, ref key, ref value);
				Color baseColor = Color.White;
				if (i == hoveredButton)
				{
					baseColor = Color.Gold;
				}
				ChatManager.DrawColorCodedStringWithShadow(sb, FontAssets.ItemStack.Value, key, rect.TopLeft() + new Vector2(20f, (float)(20 + 20 * i)), baseColor, 0f, Vector2.Zero, Vector2.One);
				ChatManager.DrawColorCodedStringWithShadow(sb, FontAssets.ItemStack.Value, value, rect.TopRight() + new Vector2(-20f, (float)(20 + 20 * i)), baseColor, 0f, FontAssets.ItemStack.Value.MeasureString(value) * Vector2.UnitX, Vector2.One);
			}
			DrawWaterChoices(sb, (rect.TopLeft() + new Vector2((float)(rect.Width / 2 - 84), 90f)).ToPoint(), Main.MouseScreen.ToPoint());
		}

		public override void ToggleActive(bool tickedOn)
		{
			if (tickedOn)
			{
				hoveredButton = -1;
			}
		}

		public override bool UsingMap()
		{
			return inUI;
		}
	}

	private static SelectionContext _selectionContext;

	private static Dictionary<int, CaptureInterfaceMode> Modes = FillModes();

	public bool Active;

	public static bool JustActivated;

	private bool KeyToggleActiveHeld;

	public int SelectedMode;

	public int HoveredMode;

	public static bool EdgeAPinned;

	public static bool EdgeBPinned;

	public static Point EdgeA;

	public static Point EdgeB;

	public static bool CameraLock;

	private static float CameraFrame;

	private static float CameraWaiting;

	private const float CameraMaxFrame = 5f;

	private const float CameraMaxWait = 60f;

	private static CaptureSettings CameraSettings;

	private static Matrix SelectionZoomMatrix
	{
		get
		{
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			if (_selectionContext != SelectionContext.World)
			{
				_ = 1;
				return Matrix.Identity;
			}
			return Main.GameViewMatrix.ZoomMatrix;
		}
	}

	private static void SetZoom_Context()
	{
		switch (_selectionContext)
		{
		case SelectionContext.Map:
			PlayerInput.SetZoom_Unscaled();
			break;
		case SelectionContext.World:
			PlayerInput.SetZoom_World();
			break;
		}
	}

	private static Dictionary<int, CaptureInterfaceMode> FillModes()
	{
		return new Dictionary<int, CaptureInterfaceMode>
		{
			{
				0,
				new ModeEdgeSelection()
			},
			{
				1,
				new ModeDragBounds()
			},
			{
				2,
				new ModeChangeSettings()
			}
		};
	}

	public static Rectangle GetArea()
	{
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		int num = Math.Min(EdgeA.X, EdgeB.X);
		int num2 = Math.Min(EdgeA.Y, EdgeB.Y);
		int num3 = Math.Abs(EdgeA.X - EdgeB.X);
		int num4 = Math.Abs(EdgeA.Y - EdgeB.Y);
		return new Rectangle(num, num2, num3 + 1, num4 + 1);
	}

	public void Update(SelectionContext context)
	{
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		_selectionContext = context;
		if (CameraLock)
		{
			return;
		}
		PlayerInput.SetZoom_UI();
		bool toggleCameraMode = PlayerInput.Triggers.Current.ToggleCameraMode;
		if (toggleCameraMode && !KeyToggleActiveHeld && (Main.mouseItem.type == 0 || Active) && !Main.CaptureModeDisabled && !Main.player[Main.myPlayer].dead && !Main.player[Main.myPlayer].ghost)
		{
			ToggleCamera(!Active);
		}
		KeyToggleActiveHeld = toggleCameraMode;
		if (!Active)
		{
			return;
		}
		Main.blockMouse = true;
		if (JustActivated && Main.mouseLeftRelease && !Main.mouseLeft)
		{
			JustActivated = false;
		}
		Vector2 mouse = new Vector2((float)Main.mouseX, (float)Main.mouseY);
		if (UpdateButtons(mouse) && Main.mouseLeft)
		{
			return;
		}
		foreach (KeyValuePair<int, CaptureInterfaceMode> mode in Modes)
		{
			mode.Value.Selected = mode.Key == SelectedMode;
			mode.Value.Update();
		}
		PlayerInput.SetZoom_Unscaled();
	}

	public void Draw(SpriteBatch sb)
	{
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0100: Unknown result type (might be due to invalid IL or missing references)
		//IL_0105: Unknown result type (might be due to invalid IL or missing references)
		//IL_011a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0121: Unknown result type (might be due to invalid IL or missing references)
		//IL_0132: Unknown result type (might be due to invalid IL or missing references)
		//IL_0138: Unknown result type (might be due to invalid IL or missing references)
		if (!Active)
		{
			return;
		}
		sb.End();
		sb.Begin((SpriteSortMode)0, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, (Effect)null, Main.UIScaleMatrix);
		PlayerInput.SetZoom_UI();
		foreach (CaptureInterfaceMode value in Modes.Values)
		{
			value.Draw(sb);
		}
		sb.End();
		sb.Begin((SpriteSortMode)0, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, (Effect)null, Main.UIScaleMatrix);
		PlayerInput.SetZoom_UI();
		Main.mouseText = false;
		Main.instance.GUIBarsDraw();
		DrawButtons(sb);
		Main.instance.DrawMouseOver();
		sb.End();
		sb.Begin((SpriteSortMode)0, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, (Effect)null, Main.UIScaleMatrix);
		Utils.DrawBorderStringBig(sb, Lang.inter[81].Value, new Vector2((float)Main.screenWidth * 0.5f, 100f), Color.White, 1f, 0.5f, 0.5f);
		Utils.DrawCursorSingle(sb, Main.cursorColor, float.NaN, Main.cursorScale);
		DrawCameraLock(sb);
		sb.End();
		sb.Begin();
	}

	public void ToggleCamera(bool On = true)
	{
		if (CameraLock)
		{
			return;
		}
		bool active = Active;
		Active = Modes.ContainsKey(SelectedMode) & On;
		if (active != Active)
		{
			SoundEngine.PlaySound(On ? 10 : 11);
		}
		foreach (KeyValuePair<int, CaptureInterfaceMode> mode in Modes)
		{
			mode.Value.ToggleActive(Active && mode.Key == SelectedMode);
		}
		if (On && !active)
		{
			JustActivated = true;
		}
	}

	private bool UpdateButtons(Vector2 mouse)
	{
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		HoveredMode = -1;
		bool flag = !Main.graphics.IsFullScreen;
		int num = 9;
		for (int i = 0; i < num; i++)
		{
			Rectangle val = new Rectangle(24 + 46 * i, 24, 42, 42);
			if (!val.Contains(mouse.ToPoint()))
			{
				continue;
			}
			HoveredMode = i;
			bool flag2 = Main.mouseLeft && Main.mouseLeftRelease;
			int num2 = 0;
			if (i == num2++ && flag2)
			{
				QuickScreenshot();
			}
			if (i == num2++ && flag2 && EdgeAPinned && EdgeBPinned)
			{
				CaptureSettings captureSettings = new CaptureSettings
				{
					Area = GetArea(),
					Biome = CaptureBiome.GetCaptureBiome(Settings.BiomeChoiceIndex),
					CaptureBackground = !Settings.TransparentBackground,
					CaptureEntities = Settings.IncludeEntities,
					UseScaling = Settings.PackImage,
					CaptureMech = WiresUI.Settings.DrawWires
				};
				if (captureSettings.Biome.WaterStyle != 13)
				{
					Main.liquidAlpha[13] = 0f;
				}
				StartCamera(captureSettings);
			}
			if (i == num2++ && flag2 && SelectedMode != 0)
			{
				SoundEngine.PlaySound(12);
				SelectedMode = 0;
				ToggleCamera();
			}
			if (i == num2++ && flag2 && SelectedMode != 1)
			{
				SoundEngine.PlaySound(12);
				SelectedMode = 1;
				ToggleCamera();
			}
			if (i == num2++ && flag2)
			{
				SoundEngine.PlaySound(12);
				ResetFocus();
			}
			if (i == num2++ && flag2 && Main.mapEnabled)
			{
				SoundEngine.PlaySound(12);
				Main.mapFullscreen = !Main.mapFullscreen;
			}
			if (i == num2++ && flag2 && SelectedMode != 2)
			{
				SoundEngine.PlaySound(12);
				SelectedMode = 2;
				ToggleCamera();
			}
			if (i == num2++ && (flag2 & flag))
			{
				SoundEngine.PlaySound(12);
				Utils.OpenFolder(Path.Combine(Main.SavePath, "Captures"));
			}
			if (i == num2++ && flag2)
			{
				ToggleCamera(On: false);
				Main.blockMouse = true;
				Main.mouseLeftRelease = false;
			}
			return true;
		}
		return false;
	}

	public static Rectangle FullScreenArea()
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		int screenWidth = Main.screenWidth;
		int screenHeight = Main.screenHeight;
		Main.screenWidth = PlayerInput.RealScreenWidth;
		Main.screenHeight = PlayerInput.RealScreenHeight;
		try
		{
			Vector2 val = Main.Camera.ScaledPosition / 16f;
			Vector2 val2 = (Main.Camera.ScaledPosition + Main.Camera.ScaledSize) / 16f;
			Point val3 = new Point((int)Math.Ceiling(val.X), (int)Math.Ceiling(val.Y));
			Point val4 = new Point((int)Math.Floor(val2.X), (int)Math.Floor(val2.Y));
			return new Rectangle(val3.X, val3.Y, val4.X - val3.X, val4.Y - val3.Y);
		}
		finally
		{
			Main.screenWidth = screenWidth;
			Main.screenHeight = screenHeight;
		}
	}

	public static void QuickScreenshot()
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		StartCamera(new CaptureSettings
		{
			Area = FullScreenArea(),
			Biome = CaptureBiome.GetCaptureBiome(Settings.BiomeChoiceIndex),
			CaptureBackground = !Settings.TransparentBackground,
			CaptureEntities = Settings.IncludeEntities,
			UseScaling = Settings.PackImage,
			CaptureMech = WiresUI.Settings.DrawWires
		});
	}

	private void DrawButtons(SpriteBatch sb)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0189: Unknown result type (might be due to invalid IL or missing references)
		//IL_0190: Unknown result type (might be due to invalid IL or missing references)
		//IL_0197: Unknown result type (might be due to invalid IL or missing references)
		//IL_019c: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0223: Unknown result type (might be due to invalid IL or missing references)
		//IL_022a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0231: Unknown result type (might be due to invalid IL or missing references)
		//IL_0236: Unknown result type (might be due to invalid IL or missing references)
		//IL_0245: Unknown result type (might be due to invalid IL or missing references)
		//IL_024f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0263: Unknown result type (might be due to invalid IL or missing references)
		//IL_026d: Unknown result type (might be due to invalid IL or missing references)
		new Vector2((float)Main.mouseX, (float)Main.mouseY);
		int num = 9;
		for (int i = 0; i < num; i++)
		{
			Texture2D val = TextureAssets.InventoryBack.Value;
			float num2 = 0.8f;
			Vector2 val2 = new Vector2((float)(24 + 46 * i), 24f);
			Color val3 = Main.inventoryBack * 0.8f;
			if (SelectedMode == 0 && i == 2)
			{
				val = TextureAssets.InventoryBack14.Value;
			}
			else if (SelectedMode == 1 && i == 3)
			{
				val = TextureAssets.InventoryBack14.Value;
			}
			else if (SelectedMode == 2 && i == 6)
			{
				val = TextureAssets.InventoryBack14.Value;
			}
			else if (i >= 2 && i <= 3)
			{
				val = TextureAssets.InventoryBack2.Value;
			}
			sb.Draw(val, val2, (Rectangle?)null, val3, 0f, default(Vector2), num2, (SpriteEffects)0, 0f);
			switch (i)
			{
			case 0:
				val = TextureAssets.Camera[7].Value;
				break;
			case 1:
				val = TextureAssets.Camera[0].Value;
				break;
			case 2:
			case 3:
			case 4:
				val = TextureAssets.Camera[i].Value;
				break;
			case 5:
				val = (Main.mapFullscreen ? TextureAssets.MapIcon[0].Value : TextureAssets.MapIcon[4].Value);
				break;
			case 6:
				val = TextureAssets.Camera[1].Value;
				break;
			case 7:
				val = TextureAssets.Camera[6].Value;
				break;
			case 8:
				val = TextureAssets.Camera[5].Value;
				break;
			}
			sb.Draw(val, val2 + new Vector2(26f) * num2, (Rectangle?)null, Color.White, 0f, val.Size() / 2f, 1f, (SpriteEffects)0, 0f);
			bool flag = false;
			switch (i)
			{
			case 1:
				if (!EdgeAPinned || !EdgeBPinned)
				{
					flag = true;
				}
				break;
			case 7:
				if (Main.graphics.IsFullScreen)
				{
					flag = true;
				}
				break;
			case 5:
				if (!Main.mapEnabled)
				{
					flag = true;
				}
				break;
			}
			if (flag)
			{
				sb.Draw(TextureAssets.Cd.Value, val2 + new Vector2(26f) * num2, (Rectangle?)null, Color.White * 0.65f, 0f, TextureAssets.Cd.Value.Size() / 2f, 1f, (SpriteEffects)0, 0f);
			}
		}
		string text = "";
		switch (HoveredMode)
		{
		case 0:
			text = Lang.inter[111].Value;
			break;
		case 1:
			text = Lang.inter[67].Value;
			break;
		case 2:
			text = Lang.inter[69].Value;
			break;
		case 3:
			text = Lang.inter[70].Value;
			break;
		case 4:
			text = Lang.inter[78].Value;
			break;
		case 5:
			text = (Main.mapFullscreen ? Lang.inter[109].Value : Lang.inter[108].Value);
			break;
		case 6:
			text = Lang.inter[68].Value;
			break;
		case 7:
			text = Lang.inter[110].Value;
			break;
		case 8:
			text = Lang.inter[71].Value;
			break;
		default:
			text = "???";
			break;
		case -1:
			break;
		}
		switch (HoveredMode)
		{
		case 1:
			if (!EdgeAPinned || !EdgeBPinned)
			{
				text = text + "\n" + Lang.inter[112].Value;
			}
			break;
		case 7:
			if (Main.graphics.IsFullScreen)
			{
				text = text + "\n" + Lang.inter[113].Value;
			}
			break;
		case 5:
			if (!Main.mapEnabled)
			{
				text = text + "\n" + Lang.inter[114].Value;
			}
			break;
		}
		if (text != "")
		{
			Main.instance.MouseText(text, 0, 0);
		}
	}

	private static bool GetMapCoords(int PinX, int PinY, int Goal, out Point result)
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_03cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_03de: Unknown result type (might be due to invalid IL or missing references)
		//IL_0394: Unknown result type (might be due to invalid IL or missing references)
		//IL_0399: Unknown result type (might be due to invalid IL or missing references)
		//IL_0385: Unknown result type (might be due to invalid IL or missing references)
		//IL_038a: Unknown result type (might be due to invalid IL or missing references)
		if (!Main.mapFullscreen)
		{
			result = new Point(-1, -1);
			return false;
		}
		float num = 0f;
		float num2 = 0f;
		float num3 = 2f;
		_ = Main.maxTilesX / MapRenderer.textureMaxWidth;
		_ = Main.maxTilesY / MapRenderer.textureMaxHeight;
		float num4 = 10f;
		float num5 = 10f;
		float num6 = Main.maxTilesX - 10;
		float num7 = Main.maxTilesY - 10;
		num = 200f;
		num2 = 300f;
		num3 = Main.mapFullscreenScale;
		float num8 = (float)Main.screenWidth / (float)Main.maxTilesX * 0.8f;
		if (Main.mapFullscreenScale < num8)
		{
			Main.mapFullscreenScale = num8;
		}
		if (Main.mapFullscreenScale > 16f)
		{
			Main.mapFullscreenScale = 16f;
		}
		num3 = Main.mapFullscreenScale;
		if (Main.mapFullscreenPos.X < num4)
		{
			Main.mapFullscreenPos.X = num4;
		}
		if (Main.mapFullscreenPos.X > num6)
		{
			Main.mapFullscreenPos.X = num6;
		}
		if (Main.mapFullscreenPos.Y < num5)
		{
			Main.mapFullscreenPos.Y = num5;
		}
		if (Main.mapFullscreenPos.Y > num7)
		{
			Main.mapFullscreenPos.Y = num7;
		}
		float x = Main.mapFullscreenPos.X;
		float y = Main.mapFullscreenPos.Y;
		float num9 = x * num3;
		y *= num3;
		num = 0f - num9 + (float)(Main.screenWidth / 2);
		num2 = 0f - y + (float)(Main.screenHeight / 2);
		num += num4 * num3;
		num2 += num5 * num3;
		float num10 = Main.maxTilesX / 840;
		num10 *= Main.mapFullscreenScale;
		float num11 = num;
		float num12 = num2;
		float num13 = TextureAssets.Map.Width();
		float num14 = TextureAssets.Map.Height();
		if (Main.maxTilesX == 8400)
		{
			num10 *= 0.999f;
			num11 -= 40.6f * num10;
			num12 = num2 - 5f * num10;
			num13 -= 8.045f;
			num13 *= num10;
			num14 += 0.12f;
			num14 *= num10;
			if ((double)num10 < 1.2)
			{
				num14++;
			}
		}
		else if (Main.maxTilesX == 6400)
		{
			num10 *= 1.09f;
			num11 -= 38.8f * num10;
			num12 = num2 - 3.85f * num10;
			num13 -= 13.6f;
			num13 *= num10;
			num14 -= 6.92f;
			num14 *= num10;
			if ((double)num10 < 1.2)
			{
				num14 += 2f;
			}
		}
		else if (Main.maxTilesX == 6300)
		{
			num10 *= 1.09f;
			num11 -= 39.8f * num10;
			num12 = num2 - 4.08f * num10;
			num13 -= 26.69f;
			num13 *= num10;
			num14 -= 6.92f;
			num14 *= num10;
			if ((double)num10 < 1.2)
			{
				num14 += 2f;
			}
		}
		else if (Main.maxTilesX == 4200)
		{
			num10 *= 0.998f;
			num11 -= 37.3f * num10;
			num12 -= 1.7f * num10;
			num13 -= 16f;
			num13 *= num10;
			num14 -= 8.31f;
			num14 *= num10;
		}
		switch (Goal)
		{
		case 0:
		{
			int num15 = (int)((0f - num + (float)PinX) / num3 + num4);
			int num16 = (int)((0f - num2 + (float)PinY) / num3 + num5);
			bool flag = false;
			if ((float)num15 < num4)
			{
				flag = true;
			}
			if ((float)num15 >= num6)
			{
				flag = true;
			}
			if ((float)num16 < num5)
			{
				flag = true;
			}
			if ((float)num16 >= num7)
			{
				flag = true;
			}
			if (!flag)
			{
				result = new Point(num15, num16);
				return true;
			}
			result = new Point(-1, -1);
			return false;
		}
		case 1:
		{
			Vector2 val = new Vector2(num, num2);
			Vector2 val2 = new Vector2((float)PinX, (float)PinY) * num3 - new Vector2(10f * num3);
			result = (val + val2).ToPoint();
			return true;
		}
		default:
			result = new Point(-1, -1);
			return false;
		}
	}

	private static void ConstraintPoints()
	{
		int fluff = 40;
		if (EdgeAPinned)
		{
			PointWorldClamp(ref EdgeA, fluff);
		}
		if (EdgeBPinned)
		{
			PointWorldClamp(ref EdgeB, fluff);
		}
	}

	private static void PointWorldClamp(ref Point point, int fluff)
	{
		if (point.X < fluff)
		{
			point.X = fluff;
		}
		if (point.X > Main.maxTilesX - 1 - fluff)
		{
			point.X = Main.maxTilesX - 1 - fluff;
		}
		if (point.Y < fluff)
		{
			point.Y = fluff;
		}
		if (point.Y > Main.maxTilesY - 1 - fluff)
		{
			point.Y = Main.maxTilesY - 1 - fluff;
		}
	}

	public bool UsingMap()
	{
		if (CameraLock)
		{
			return true;
		}
		return Modes[SelectedMode].UsingMap();
	}

	public static void ResetFocus()
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		EdgeAPinned = false;
		EdgeBPinned = false;
		EdgeA = new Point(-1, -1);
		EdgeB = new Point(-1, -1);
	}

	public void Scrolling()
	{
		int num = PlayerInput.ScrollWheelDelta / 120;
		num %= 30;
		if (num < 0)
		{
			num += 30;
		}
		int selectedMode = SelectedMode;
		SelectedMode -= num;
		while (SelectedMode < 0)
		{
			SelectedMode += 2;
		}
		while (SelectedMode > 2)
		{
			SelectedMode -= 2;
		}
		if (SelectedMode != selectedMode)
		{
			SoundEngine.PlaySound(12);
		}
	}

	public void UpdateCameraCountdown()
	{
		if (CameraLock && CameraFrame == 4f)
		{
			CaptureManager.Instance.Capture(CameraSettings);
		}
		CameraFrame += CameraLock.ToDirectionInt();
		if (CameraFrame < 0f)
		{
			CameraFrame = 0f;
		}
		if (CameraFrame > 5f)
		{
			CameraFrame = 5f;
		}
		if (CameraFrame == 5f)
		{
			CameraWaiting++;
		}
		if (CameraWaiting > 60f)
		{
			CameraWaiting = 60f;
		}
	}

	private void DrawCameraLock(SpriteBatch sb)
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0105: Unknown result type (might be due to invalid IL or missing references)
		//IL_0113: Unknown result type (might be due to invalid IL or missing references)
		//IL_0130: Unknown result type (might be due to invalid IL or missing references)
		//IL_013a: Unknown result type (might be due to invalid IL or missing references)
		//IL_013f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0141: Unknown result type (might be due to invalid IL or missing references)
		//IL_0146: Unknown result type (might be due to invalid IL or missing references)
		//IL_014c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0156: Unknown result type (might be due to invalid IL or missing references)
		//IL_015b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0187: Unknown result type (might be due to invalid IL or missing references)
		//IL_0191: Unknown result type (might be due to invalid IL or missing references)
		//IL_0196: Unknown result type (might be due to invalid IL or missing references)
		//IL_0198: Unknown result type (might be due to invalid IL or missing references)
		//IL_019d: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b2: Unknown result type (might be due to invalid IL or missing references)
		if (CameraFrame == 0f)
		{
			return;
		}
		sb.Draw(TextureAssets.MagicPixel.Value, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), (Rectangle?)new Rectangle(0, 0, 1, 1), Color.Black * (CameraFrame / 5f));
		if (CameraFrame != 5f)
		{
			return;
		}
		float num = CameraWaiting - 60f + 5f;
		if (!(num <= 0f))
		{
			num /= 5f;
			float num2 = CaptureManager.Instance.GetProgress() * 100f;
			if (num2 > 100f)
			{
				num2 = 100f;
			}
			string text = num2.ToString("##") + " ";
			string text2 = "/ 100%";
			Vector2 val = FontAssets.DeathText.Value.MeasureString(text);
			Vector2 val2 = FontAssets.DeathText.Value.MeasureString(text2);
			Vector2 val3 = new Vector2(0f - val.X, (0f - val.Y) / 2f);
			Vector2 val4 = new Vector2(0f, (0f - val2.Y) / 2f);
			ChatManager.DrawColorCodedStringWithShadow(sb, FontAssets.DeathText.Value, text, new Vector2((float)Main.screenWidth, (float)Main.screenHeight) / 2f + val3, Color.White * num, 0f, Vector2.Zero, Vector2.One);
			ChatManager.DrawColorCodedStringWithShadow(sb, FontAssets.DeathText.Value, text2, new Vector2((float)Main.screenWidth, (float)Main.screenHeight) / 2f + val4, Color.White * num, 0f, Vector2.Zero, Vector2.One);
		}
	}

	public static void StartCamera(CaptureSettings settings)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		Rectangle val = FullScreenArea();
		settings.CameraSpaceEffects = val.Contains(settings.Area);
		SoundEngine.PlaySound(40);
		CameraSettings = settings;
		CameraLock = true;
		CameraWaiting = 0f;
	}

	public static void EndCamera()
	{
		CameraLock = false;
	}
}
