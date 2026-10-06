using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using Terraria.Localization;
using Terraria.UI.Chat;

namespace Terraria.GameContent.UI;

public class NewMultiplayerClosePlayersOverlay : IMultiplayerClosePlayersOverlay
{
	private struct PlayerOnScreenCache(string name, Vector2 pos, Color color)
	{
		private string _name = name;

		private Vector2 _pos = pos;

		private Color _color = color;

		public void DrawPlayerName_WhenPlayerIsOnScreen(SpriteBatch spriteBatch)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_0095: Unknown result type (might be due to invalid IL or missing references)
			//IL_009a: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0103: Unknown result type (might be due to invalid IL or missing references)
			//IL_0143: Unknown result type (might be due to invalid IL or missing references)
			//IL_0148: Unknown result type (might be due to invalid IL or missing references)
			//IL_0154: Unknown result type (might be due to invalid IL or missing references)
			//IL_015a: Unknown result type (might be due to invalid IL or missing references)
			//IL_017f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0185: Unknown result type (might be due to invalid IL or missing references)
			//IL_0191: Unknown result type (might be due to invalid IL or missing references)
			//IL_0197: Unknown result type (might be due to invalid IL or missing references)
			_pos = _pos.Floor();
			DynamicSpriteFontExtensionMethods.DrawString(spriteBatch, FontAssets.MouseText.Value, _name, new Vector2(_pos.X - 2f, _pos.Y), Color.Black, 0f, default(Vector2), 1f, (SpriteEffects)0, 0f, (Vector2[])null, (Color[])null);
			DynamicSpriteFontExtensionMethods.DrawString(spriteBatch, FontAssets.MouseText.Value, _name, new Vector2(_pos.X + 2f, _pos.Y), Color.Black, 0f, default(Vector2), 1f, (SpriteEffects)0, 0f, (Vector2[])null, (Color[])null);
			DynamicSpriteFontExtensionMethods.DrawString(spriteBatch, FontAssets.MouseText.Value, _name, new Vector2(_pos.X, _pos.Y - 2f), Color.Black, 0f, default(Vector2), 1f, (SpriteEffects)0, 0f, (Vector2[])null, (Color[])null);
			DynamicSpriteFontExtensionMethods.DrawString(spriteBatch, FontAssets.MouseText.Value, _name, new Vector2(_pos.X, _pos.Y + 2f), Color.Black, 0f, default(Vector2), 1f, (SpriteEffects)0, 0f, (Vector2[])null, (Color[])null);
			DynamicSpriteFontExtensionMethods.DrawString(spriteBatch, FontAssets.MouseText.Value, _name, _pos, _color, 0f, default(Vector2), 1f, (SpriteEffects)0, 0f, (Vector2[])null, (Color[])null);
		}
	}

	private struct PlayerOffScreenCache(string name, Vector2 pos, Color color, Vector2 npDistPos, string npDist, Player thePlayer, Vector2 theMeasurement, bool drawScryingOrb)
	{
		private Player player = thePlayer;

		private string nameToShow = name;

		private Vector2 namePlatePos = pos.Floor();

		private Color namePlateColor = color;

		private Vector2 distanceDrawPosition = npDistPos.Floor();

		private string distanceString = npDist;

		private Vector2 measurement = theMeasurement;

		private bool drawScryingOrb = drawScryingOrb;

		public void DrawPlayerName(SpriteBatch spriteBatch)
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			ChatManager.DrawColorCodedStringWithShadow(spriteBatch, FontAssets.MouseText.Value, nameToShow, namePlatePos + new Vector2(0f, -40f), namePlateColor, 0f, Vector2.Zero, Vector2.One);
		}

		public void DrawPlayerHead()
		{
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			//IL_0074: Unknown result type (might be due to invalid IL or missing references)
			//IL_0079: Unknown result type (might be due to invalid IL or missing references)
			//IL_008a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0095: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00de: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
			float num = 20f;
			float num2 = -27f;
			num2 -= (measurement.X - 85f) / 2f;
			Color playerHeadBordersColor = Main.GetPlayerHeadBordersColor(player);
			Vector2 vec = new Vector2(namePlatePos.X, namePlatePos.Y - num);
			vec.X -= 22f + num2;
			vec.Y += 8f;
			vec = vec.Floor();
			Main.MapPlayerRenderer.DrawPlayerHead(Main.Camera, player, vec, 1f, 0.8f, playerHeadBordersColor);
			if (drawScryingOrb)
			{
				Main.GetItemDrawFrame(5644, out var itemTexture, out var itemFrame);
				Main.spriteBatch.Draw(itemTexture, vec + new Vector2(-26f, 4f), (Rectangle?)itemFrame, Color.White, 0f, itemFrame.Size() / 2f, 1f, (SpriteEffects)0, 0f);
			}
		}

		public void DrawLifeBar()
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			Vector2 val = Main.screenPosition + distanceDrawPosition + new Vector2(26f, 20f);
			if (player.statLife != player.statLifeMax2)
			{
				Main.instance.DrawHealthBar(val.X, val.Y, player.statLife, player.statLifeMax2, 1f, 1.25f, noFlip: true);
			}
		}

		public void DrawPlayerDistance(SpriteBatch spriteBatch)
		{
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0086: Unknown result type (might be due to invalid IL or missing references)
			//IL_008b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0097: Unknown result type (might be due to invalid IL or missing references)
			//IL_009d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00de: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_012c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0131: Unknown result type (might be due to invalid IL or missing references)
			//IL_013d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0143: Unknown result type (might be due to invalid IL or missing references)
			//IL_0164: Unknown result type (might be due to invalid IL or missing references)
			//IL_016a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0176: Unknown result type (might be due to invalid IL or missing references)
			//IL_017c: Unknown result type (might be due to invalid IL or missing references)
			float num = 0.85f;
			DynamicSpriteFontExtensionMethods.DrawString(spriteBatch, FontAssets.MouseText.Value, distanceString, new Vector2(distanceDrawPosition.X - 2f, distanceDrawPosition.Y), Color.Black, 0f, default(Vector2), num, (SpriteEffects)0, 0f, (Vector2[])null, (Color[])null);
			DynamicSpriteFontExtensionMethods.DrawString(spriteBatch, FontAssets.MouseText.Value, distanceString, new Vector2(distanceDrawPosition.X + 2f, distanceDrawPosition.Y), Color.Black, 0f, default(Vector2), num, (SpriteEffects)0, 0f, (Vector2[])null, (Color[])null);
			DynamicSpriteFontExtensionMethods.DrawString(spriteBatch, FontAssets.MouseText.Value, distanceString, new Vector2(distanceDrawPosition.X, distanceDrawPosition.Y - 2f), Color.Black, 0f, default(Vector2), num, (SpriteEffects)0, 0f, (Vector2[])null, (Color[])null);
			DynamicSpriteFontExtensionMethods.DrawString(spriteBatch, FontAssets.MouseText.Value, distanceString, new Vector2(distanceDrawPosition.X, distanceDrawPosition.Y + 2f), Color.Black, 0f, default(Vector2), num, (SpriteEffects)0, 0f, (Vector2[])null, (Color[])null);
			DynamicSpriteFontExtensionMethods.DrawString(spriteBatch, FontAssets.MouseText.Value, distanceString, distanceDrawPosition, namePlateColor, 0f, default(Vector2), num, (SpriteEffects)0, 0f, (Vector2[])null, (Color[])null);
		}
	}

	private List<PlayerOnScreenCache> _playerOnScreenCache = new List<PlayerOnScreenCache>();

	private List<PlayerOffScreenCache> _playerOffScreenCache = new List<PlayerOffScreenCache>();

	public void Draw()
	{
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_025c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0141: Unknown result type (might be due to invalid IL or missing references)
		//IL_022f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0231: Unknown result type (might be due to invalid IL or missing references)
		//IL_0151: Unknown result type (might be due to invalid IL or missing references)
		//IL_0306: Unknown result type (might be due to invalid IL or missing references)
		//IL_0186: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01da: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_020d: Unknown result type (might be due to invalid IL or missing references)
		//IL_020f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0211: Unknown result type (might be due to invalid IL or missing references)
		//IL_0217: Unknown result type (might be due to invalid IL or missing references)
		//IL_034d: Unknown result type (might be due to invalid IL or missing references)
		int teamNamePlateDistance = Main.teamNamePlateDistance;
		if (teamNamePlateDistance <= 0)
		{
			return;
		}
		_playerOnScreenCache.Clear();
		_playerOffScreenCache.Clear();
		SpriteBatch spriteBatch = Main.spriteBatch;
		int num = teamNamePlateDistance * 8;
		Player[] player = Main.player;
		int myPlayer = Main.myPlayer;
		byte mouseTextColor = Main.mouseTextColor;
		Color[] teamColor = Main.teamColor;
		_ = Main.screenPosition;
		Player player2 = player[myPlayer];
		float num2 = (float)(int)mouseTextColor / 255f;
		if (Main.netMode == 0)
		{
			return;
		}
		DynamicSpriteFont value = FontAssets.MouseText.Value;
		for (int i = 0; i < 255; i++)
		{
			if (i == myPlayer)
			{
				continue;
			}
			Player player3 = player[i];
			bool flag = player3.spectating == myPlayer;
			if (!player3.active || (player3.dead && !flag && !player3.ghost) || player3.team != player2.team)
			{
				continue;
			}
			if (player3.team == 0 && !flag)
			{
				return;
			}
			string name = player3.name;
			GetDistance(value, player3, name, out var namePlatePos, out var offScreen, out var measurement);
			Color color = new Color((int)(byte)((float)(int)teamColor[player3.team].R * num2), (int)(byte)((float)(int)teamColor[player3.team].G * num2), (int)(byte)((float)(int)teamColor[player3.team].B * num2), (int)mouseTextColor);
			if (offScreen)
			{
				float num3 = player3.Distance(player2.Center);
				if (!(num3 > (float)num))
				{
					namePlatePos.Y += 40f;
					float num4 = 20f;
					float num5 = -27f;
					num5 -= (measurement.X - 85f) / 2f;
					string textValue = Language.GetTextValue("GameUI.PlayerDistance", (int)(num3 / 16f * 2f));
					Vector2 val = value.MeasureString(textValue);
					val.X = namePlatePos.X - num5;
					val.Y = namePlatePos.Y + measurement.Y / 2f - val.Y / 2f - num4;
					_playerOffScreenCache.Add(new PlayerOffScreenCache(name, namePlatePos, color, val, textValue, player3, measurement, flag));
				}
			}
			else
			{
				_playerOnScreenCache.Add(new PlayerOnScreenCache(name, namePlatePos, color));
			}
		}
		spriteBatch.End();
		spriteBatch.Begin((SpriteSortMode)0, (BlendState)null, (SamplerState)null, (DepthStencilState)null, (RasterizerState)null, (Effect)null, Main.UIScaleMatrix);
		for (int j = 0; j < _playerOnScreenCache.Count; j++)
		{
			_playerOnScreenCache[j].DrawPlayerName_WhenPlayerIsOnScreen(spriteBatch);
		}
		for (int k = 0; k < _playerOffScreenCache.Count; k++)
		{
			_playerOffScreenCache[k].DrawPlayerName(spriteBatch);
		}
		for (int l = 0; l < _playerOffScreenCache.Count; l++)
		{
			_playerOffScreenCache[l].DrawPlayerDistance(spriteBatch);
		}
		spriteBatch.End();
		spriteBatch.Begin((SpriteSortMode)0, (BlendState)null, (SamplerState)null, (DepthStencilState)null, (RasterizerState)null, (Effect)null, Main.UIScaleMatrix);
		for (int m = 0; m < _playerOffScreenCache.Count; m++)
		{
			_playerOffScreenCache[m].DrawLifeBar();
		}
		spriteBatch.End();
		spriteBatch.Begin((SpriteSortMode)1, (BlendState)null, (SamplerState)null, (DepthStencilState)null, (RasterizerState)null, (Effect)null, Main.UIScaleMatrix);
		for (int n = 0; n < _playerOffScreenCache.Count; n++)
		{
			_playerOffScreenCache[n].DrawPlayerHead();
		}
	}

	private static void GetDistance(DynamicSpriteFont font, Player player, string nameToShow, out Vector2 namePlatePos, out bool offScreen, out Vector2 measurement)
	{
		//IL_0004: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0102: Unknown result type (might be due to invalid IL or missing references)
		//IL_0107: Unknown result type (might be due to invalid IL or missing references)
		//IL_010c: Unknown result type (might be due to invalid IL or missing references)
		measurement = font.MeasureString(nameToShow);
		namePlatePos = Main.GetChatDrawPosition(player);
		namePlatePos.Y -= measurement.Y / 2f;
		if (player.chatOverhead.timeLeft > 0 || player.emoteTime > 0)
		{
			namePlatePos.Y -= measurement.Y;
		}
		Vector2 val = Main.ScreenSize.ToVector2() / Main.UIScale;
		Vector2 val2 = val / 2f;
		Vector2 val3 = Vector2.Max(new Vector2(100f), val / 2f - new Vector2(80f, 50f));
		Vector2 val4 = namePlatePos - val2;
		Vector2 val5 = val4 / val3;
		float num = val5.Length();
		if (num > 1f)
		{
			offScreen = true;
			namePlatePos = val2 + val4 / num;
		}
		else
		{
			offScreen = false;
		}
		namePlatePos -= measurement / 2f;
	}
}
