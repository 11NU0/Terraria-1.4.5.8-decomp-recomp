using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using Terraria.GameInput;
using Terraria.Graphics;
using Terraria.Graphics.Renderers;
using Terraria.Localization;
using Terraria.UI.Chat;

namespace Terraria.GameContent.UI;

public class LegacyMultiplayerClosePlayersOverlay : IMultiplayerClosePlayersOverlay
{
	public void Draw()
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_0105: Unknown result type (might be due to invalid IL or missing references)
		//IL_010a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0125: Unknown result type (might be due to invalid IL or missing references)
		//IL_0154: Unknown result type (might be due to invalid IL or missing references)
		//IL_0160: Unknown result type (might be due to invalid IL or missing references)
		//IL_0168: Unknown result type (might be due to invalid IL or missing references)
		//IL_0172: Unknown result type (might be due to invalid IL or missing references)
		//IL_0177: Unknown result type (might be due to invalid IL or missing references)
		//IL_0179: Unknown result type (might be due to invalid IL or missing references)
		//IL_017b: Unknown result type (might be due to invalid IL or missing references)
		//IL_017d: Unknown result type (might be due to invalid IL or missing references)
		//IL_017f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0186: Unknown result type (might be due to invalid IL or missing references)
		//IL_018b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0190: Unknown result type (might be due to invalid IL or missing references)
		//IL_0195: Unknown result type (might be due to invalid IL or missing references)
		//IL_019a: Unknown result type (might be due to invalid IL or missing references)
		//IL_019f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0208: Unknown result type (might be due to invalid IL or missing references)
		//IL_020d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0222: Unknown result type (might be due to invalid IL or missing references)
		//IL_022c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0233: Unknown result type (might be due to invalid IL or missing references)
		//IL_0244: Unknown result type (might be due to invalid IL or missing references)
		//IL_0141: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0288: Unknown result type (might be due to invalid IL or missing references)
		//IL_029d: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0332: Unknown result type (might be due to invalid IL or missing references)
		//IL_0337: Unknown result type (might be due to invalid IL or missing references)
		//IL_0339: Unknown result type (might be due to invalid IL or missing references)
		//IL_033b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0342: Unknown result type (might be due to invalid IL or missing references)
		//IL_0347: Unknown result type (might be due to invalid IL or missing references)
		//IL_034c: Unknown result type (might be due to invalid IL or missing references)
		//IL_034e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0358: Unknown result type (might be due to invalid IL or missing references)
		//IL_035d: Unknown result type (might be due to invalid IL or missing references)
		//IL_035f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0361: Unknown result type (might be due to invalid IL or missing references)
		//IL_0368: Unknown result type (might be due to invalid IL or missing references)
		//IL_036d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0372: Unknown result type (might be due to invalid IL or missing references)
		//IL_0389: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_03dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_03fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0454: Unknown result type (might be due to invalid IL or missing references)
		//IL_0459: Unknown result type (might be due to invalid IL or missing references)
		//IL_045d: Unknown result type (might be due to invalid IL or missing references)
		//IL_046e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0475: Unknown result type (might be due to invalid IL or missing references)
		//IL_0483: Unknown result type (might be due to invalid IL or missing references)
		//IL_04aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_04af: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0501: Unknown result type (might be due to invalid IL or missing references)
		//IL_0508: Unknown result type (might be due to invalid IL or missing references)
		//IL_050a: Unknown result type (might be due to invalid IL or missing references)
		//IL_050c: Unknown result type (might be due to invalid IL or missing references)
		//IL_051b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0520: Unknown result type (might be due to invalid IL or missing references)
		//IL_0525: Unknown result type (might be due to invalid IL or missing references)
		//IL_0581: Unknown result type (might be due to invalid IL or missing references)
		//IL_058d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0592: Unknown result type (might be due to invalid IL or missing references)
		//IL_0597: Unknown result type (might be due to invalid IL or missing references)
		//IL_059e: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0542: Unknown result type (might be due to invalid IL or missing references)
		//IL_0549: Unknown result type (might be due to invalid IL or missing references)
		int teamNamePlateDistance = Main.teamNamePlateDistance;
		if (teamNamePlateDistance <= 0)
		{
			return;
		}
		SpriteBatch spriteBatch = Main.spriteBatch;
		spriteBatch.End();
		spriteBatch.Begin((SpriteSortMode)1, (BlendState)null, (SamplerState)null, (DepthStencilState)null, (RasterizerState)null, (Effect)null, Main.UIScaleMatrix);
		PlayerInput.SetZoom_World();
		int screenWidth = Main.screenWidth;
		int screenHeight = Main.screenHeight;
		Vector2 screenPosition = Main.screenPosition;
		PlayerInput.SetZoom_UI();
		float uIScale = Main.UIScale;
		int num = teamNamePlateDistance * 8;
		Player[] player = Main.player;
		int myPlayer = Main.myPlayer;
		SpriteViewMatrix gameViewMatrix = Main.GameViewMatrix;
		byte mouseTextColor = Main.mouseTextColor;
		Color[] teamColor = Main.teamColor;
		Camera camera = Main.Camera;
		IPlayerRenderer playerRenderer = Main.PlayerRenderer;
		Vector2 screenPosition2 = Main.screenPosition;
		for (int i = 0; i < 255; i++)
		{
			if (!player[i].active || myPlayer == i || (player[i].dead && !player[i].ghost) || player[myPlayer].team <= 0 || player[myPlayer].team != player[i].team)
			{
				continue;
			}
			string name = player[i].name;
			Vector2 val = FontAssets.MouseText.Value.MeasureString(name);
			float num2 = 0f;
			if (player[i].chatOverhead.timeLeft > 0)
			{
				num2 = (0f - val.Y) * uIScale;
			}
			else if (player[i].emoteTime > 0)
			{
				num2 = (0f - val.Y) * uIScale;
			}
			Vector2 val2 = new Vector2((float)(screenWidth / 2) + screenPosition.X, (float)(screenHeight / 2) + screenPosition.Y);
			Vector2 position = player[i].position;
			position += (position - val2) * (gameViewMatrix.RenderZoom - Vector2.One);
			float num3 = 0f;
			float num4 = (float)(int)mouseTextColor / 255f;
			Color namePlateColor = new Color((int)(byte)((float)(int)teamColor[player[i].team].R * num4), (int)(byte)((float)(int)teamColor[player[i].team].G * num4), (int)(byte)((float)(int)teamColor[player[i].team].B * num4), (int)mouseTextColor);
			float num5 = position.X + (float)(player[i].width / 2) - val2.X;
			float num6 = position.Y - val.Y - 2f + num2 - val2.Y;
			float num7 = (float)Math.Sqrt(num5 * num5 + num6 * num6);
			int num8 = screenHeight;
			if (screenHeight > screenWidth)
			{
				num8 = screenWidth;
			}
			num8 = num8 / 2 - 50;
			if (num8 < 100)
			{
				num8 = 100;
			}
			if (num7 < (float)num8)
			{
				val.X = position.X + (float)(player[i].width / 2) - val.X / 2f - screenPosition.X;
				val.Y = position.Y - val.Y - 2f + num2 - screenPosition.Y;
			}
			else
			{
				num3 = num7;
				num7 = (float)num8 / num7;
				val.X = (float)(screenWidth / 2) + num5 * num7 - val.X / 2f;
				val.Y = (float)(screenHeight / 2) + num6 * num7 + 40f * uIScale;
			}
			Vector2 val3 = FontAssets.MouseText.Value.MeasureString(name);
			val += val3 / 2f;
			val *= 1f / uIScale;
			val -= val3 / 2f;
			if (player[myPlayer].gravDir == -1f)
			{
				val.Y = (float)screenHeight - val.Y;
			}
			if (num3 > 0f)
			{
				float num9 = 20f;
				float num10 = -27f;
				num10 -= (val3.X - 85f) / 2f;
				num5 = player[i].Center.X - player[myPlayer].Center.X;
				num6 = player[i].Center.Y - player[myPlayer].Center.Y;
				float num11 = (float)Math.Sqrt(num5 * num5 + num6 * num6);
				if (!(num11 > (float)num))
				{
					string textValue = Language.GetTextValue("GameUI.PlayerDistance", (int)(num11 / 16f * 2f));
					Vector2 npDistPos = FontAssets.MouseText.Value.MeasureString(textValue);
					npDistPos.X = val.X - num10;
					npDistPos.Y = val.Y + val3.Y / 2f - npDistPos.Y / 2f - num9;
					DrawPlayerName2(spriteBatch, ref namePlateColor, textValue, ref npDistPos);
					Color playerHeadBordersColor = Main.GetPlayerHeadBordersColor(player[i]);
					Vector2 position2 = new Vector2(val.X, val.Y - num9);
					position2.X -= 22f + num10;
					position2.Y += 8f;
					playerRenderer.DrawPlayerHead(camera, player[i], position2, 1f, 0.8f, playerHeadBordersColor);
					Vector2 val4 = npDistPos + screenPosition2 + new Vector2(26f, 20f);
					if (player[i].statLife != player[i].statLifeMax2)
					{
						Main.instance.DrawHealthBar(val4.X, val4.Y, player[i].statLife, player[i].statLifeMax2, 1f, 1.25f, noFlip: true);
					}
					ChatManager.DrawColorCodedStringWithShadow(spriteBatch, FontAssets.MouseText.Value, name, val + new Vector2(0f, -40f), namePlateColor, 0f, Vector2.Zero, Vector2.One);
				}
			}
			else
			{
				DrawPlayerName(spriteBatch, name, ref val, ref namePlateColor);
			}
		}
	}

	private static void DrawPlayerName2(SpriteBatch spriteBatch, ref Color namePlateColor, string npDist, ref Vector2 npDistPos)
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0101: Unknown result type (might be due to invalid IL or missing references)
		//IL_0107: Unknown result type (might be due to invalid IL or missing references)
		//IL_0123: Unknown result type (might be due to invalid IL or missing references)
		//IL_0129: Unknown result type (might be due to invalid IL or missing references)
		//IL_0135: Unknown result type (might be due to invalid IL or missing references)
		//IL_013b: Unknown result type (might be due to invalid IL or missing references)
		float num = 0.85f;
		DynamicSpriteFontExtensionMethods.DrawString(spriteBatch, FontAssets.MouseText.Value, npDist, new Vector2(npDistPos.X - 2f, npDistPos.Y), Color.Black, 0f, default(Vector2), num, (SpriteEffects)0, 0f, (Vector2[])null, (Color[])null);
		DynamicSpriteFontExtensionMethods.DrawString(spriteBatch, FontAssets.MouseText.Value, npDist, new Vector2(npDistPos.X + 2f, npDistPos.Y), Color.Black, 0f, default(Vector2), num, (SpriteEffects)0, 0f, (Vector2[])null, (Color[])null);
		DynamicSpriteFontExtensionMethods.DrawString(spriteBatch, FontAssets.MouseText.Value, npDist, new Vector2(npDistPos.X, npDistPos.Y - 2f), Color.Black, 0f, default(Vector2), num, (SpriteEffects)0, 0f, (Vector2[])null, (Color[])null);
		DynamicSpriteFontExtensionMethods.DrawString(spriteBatch, FontAssets.MouseText.Value, npDist, new Vector2(npDistPos.X, npDistPos.Y + 2f), Color.Black, 0f, default(Vector2), num, (SpriteEffects)0, 0f, (Vector2[])null, (Color[])null);
		DynamicSpriteFontExtensionMethods.DrawString(spriteBatch, FontAssets.MouseText.Value, npDist, npDistPos, namePlateColor, 0f, default(Vector2), num, (SpriteEffects)0, 0f, (Vector2[])null, (Color[])null);
	}

	private static void DrawPlayerName(SpriteBatch spriteBatch, string namePlate, ref Vector2 namePlatePos, ref Color namePlateColor)
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0107: Unknown result type (might be due to invalid IL or missing references)
		//IL_010d: Unknown result type (might be due to invalid IL or missing references)
		//IL_012d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0133: Unknown result type (might be due to invalid IL or missing references)
		//IL_013f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0145: Unknown result type (might be due to invalid IL or missing references)
		DynamicSpriteFontExtensionMethods.DrawString(spriteBatch, FontAssets.MouseText.Value, namePlate, new Vector2(namePlatePos.X - 2f, namePlatePos.Y), Color.Black, 0f, default(Vector2), 1f, (SpriteEffects)0, 0f, (Vector2[])null, (Color[])null);
		DynamicSpriteFontExtensionMethods.DrawString(spriteBatch, FontAssets.MouseText.Value, namePlate, new Vector2(namePlatePos.X + 2f, namePlatePos.Y), Color.Black, 0f, default(Vector2), 1f, (SpriteEffects)0, 0f, (Vector2[])null, (Color[])null);
		DynamicSpriteFontExtensionMethods.DrawString(spriteBatch, FontAssets.MouseText.Value, namePlate, new Vector2(namePlatePos.X, namePlatePos.Y - 2f), Color.Black, 0f, default(Vector2), 1f, (SpriteEffects)0, 0f, (Vector2[])null, (Color[])null);
		DynamicSpriteFontExtensionMethods.DrawString(spriteBatch, FontAssets.MouseText.Value, namePlate, new Vector2(namePlatePos.X, namePlatePos.Y + 2f), Color.Black, 0f, default(Vector2), 1f, (SpriteEffects)0, 0f, (Vector2[])null, (Color[])null);
		DynamicSpriteFontExtensionMethods.DrawString(spriteBatch, FontAssets.MouseText.Value, namePlate, namePlatePos, namePlateColor, 0f, default(Vector2), 1f, (SpriteEffects)0, 0f, (Vector2[])null, (Color[])null);
	}
}
