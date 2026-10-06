using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.Achievements;
using Terraria.GameContent;
using Terraria.GameInput;
using Terraria.Social.Base;

namespace Terraria.UI;

public class InGamePopups
{
	public class AchievementUnlockedPopup : IInGameNotification
	{
		private Achievement _theAchievement;

		private Asset<Texture2D> _achievementTexture;

		private Asset<Texture2D> _achievementBorderTexture;

		private const int _iconSize = 64;

		private const int _iconSizeWithSpace = 66;

		private const int _iconsPerRow = 8;

		private int _iconIndex;

		private Rectangle _achievementIconFrame;

		private string _title;

		private int _ingameDisplayTimeLeft;

		public bool ShouldBeRemoved { get; private set; }

		public object CreationObject { get; private set; }

		private float Scale
		{
			get
			{
				if (_ingameDisplayTimeLeft < 30)
				{
					return MathHelper.Lerp(0f, 1f, (float)_ingameDisplayTimeLeft / 30f);
				}
				if (_ingameDisplayTimeLeft > 285)
				{
					return MathHelper.Lerp(1f, 0f, ((float)_ingameDisplayTimeLeft - 285f) / 15f);
				}
				return 1f;
			}
		}

		private float Opacity
		{
			get
			{
				float scale = Scale;
				if (scale <= 0.5f)
				{
					return 0f;
				}
				return (scale - 0.5f) / 0.5f;
			}
		}

		public AchievementUnlockedPopup(Achievement achievement)
		{
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			CreationObject = achievement;
			_ingameDisplayTimeLeft = 300;
			_theAchievement = achievement;
			_title = achievement.FriendlyName.Value;
			int num = (_iconIndex = Main.Achievements.GetIconIndex(achievement.Name));
			_achievementIconFrame = new Rectangle(num % 8 * 66, num / 8 * 66, 64, 64);
			_achievementTexture = Main.Assets.Request<Texture2D>("Images/UI/Achievements", (AssetRequestMode)2);
			_achievementBorderTexture = Main.Assets.Request<Texture2D>("Images/UI/Achievement_Borders", (AssetRequestMode)2);
		}

		public void Update()
		{
			_ingameDisplayTimeLeft--;
			if (_ingameDisplayTimeLeft < 0)
			{
				_ingameDisplayTimeLeft = 0;
			}
		}

		public void PushAnchor(ref Vector2 anchorPosition)
		{
			float num = 50f * Opacity;
			anchorPosition.Y -= num;
		}

		public void DrawInGame(SpriteBatch sb, Vector2 bottomAnchorPosition)
		{
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_006d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0072: Unknown result type (might be due to invalid IL or missing references)
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			//IL_0078: Unknown result type (might be due to invalid IL or missing references)
			//IL_007c: Unknown result type (might be due to invalid IL or missing references)
			//IL_007e: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0094: Unknown result type (might be due to invalid IL or missing references)
			//IL_009e: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0101: Unknown result type (might be due to invalid IL or missing references)
			//IL_010f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0112: Unknown result type (might be due to invalid IL or missing references)
			//IL_011c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0122: Unknown result type (might be due to invalid IL or missing references)
			//IL_013f: Unknown result type (might be due to invalid IL or missing references)
			//IL_015d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0169: Unknown result type (might be due to invalid IL or missing references)
			//IL_016f: Unknown result type (might be due to invalid IL or missing references)
			//IL_018c: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01db: Unknown result type (might be due to invalid IL or missing references)
			float opacity = Opacity;
			if (opacity > 0f)
			{
				float num = Scale * 1.1f;
				Vector2 val = (FontAssets.ItemStack.Value.MeasureString(_title) + new Vector2(58f, 10f)) * num;
				Rectangle r = Utils.CenteredRectangle(bottomAnchorPosition + new Vector2(0f, (0f - val.Y) * 0.5f), val);
				Vector2 mouseScreen = Main.MouseScreen;
				bool flag = r.Contains(mouseScreen.ToPoint());
				Utils.DrawInvBG(c: flag ? (new Color(64, 109, 164) * 0.75f) : (new Color(64, 109, 164) * 0.5f), sb: sb, R: r);
				float num2 = num * 0.3f;
				Vector2 val2 = r.Right() - Vector2.UnitX * num * (12f + num2 * (float)_achievementIconFrame.Width);
				sb.Draw(_achievementTexture.Value, val2, (Rectangle?)_achievementIconFrame, Color.White * opacity, 0f, new Vector2(0f, (float)(_achievementIconFrame.Height / 2)), num2, (SpriteEffects)0, 0f);
				sb.Draw(_achievementBorderTexture.Value, val2, (Rectangle?)null, Color.White * opacity, 0f, new Vector2(4f, (float)(_achievementBorderTexture.Height() / 2)), num2, (SpriteEffects)0, 0f);
				Utils.DrawBorderString(color: new Color((int)Main.mouseTextColor, (int)Main.mouseTextColor, Main.mouseTextColor / 5, (int)Main.mouseTextColor) * opacity, sb: sb, text: _title, pos: val2 - Vector2.UnitX * 10f, scale: num * 0.9f, anchorx: 1f, anchory: 0.4f);
				if (flag)
				{
					OnMouseOver();
				}
			}
		}

		private void OnMouseOver()
		{
			if (PlayerInput.IgnoreMouseInterface)
			{
				return;
			}
			Main.player[Main.myPlayer].mouseInterface = true;
			if (!Main.mouseLeft || !Main.mouseLeftRelease)
			{
				return;
			}
			Main.mouseLeftRelease = false;
			if (Main.gameMenu)
			{
				if (Main.menuMode == 0)
				{
					IngameFancyUI.OpenAchievementsAndGoto(_theAchievement);
				}
			}
			else
			{
				IngameFancyUI.OpenAchievementsAndGoto(_theAchievement);
			}
			_ingameDisplayTimeLeft = 0;
			ShouldBeRemoved = true;
		}

		public void DrawInNotificationsArea(SpriteBatch spriteBatch, Rectangle area, ref int gamepadPointLocalIndexTouse)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			Utils.DrawInvBG(spriteBatch, area, Color.Red);
		}
	}

	public class PlayerWantsToJoinGamePopup : IInGameNotification
	{
		private int _timeLeft;

		private const int _timeLeftMax = 1800;

		private UserJoinToServerRequest _request;

		private float Scale
		{
			get
			{
				if (_timeLeft < 30)
				{
					return MathHelper.Lerp(0f, 1f, (float)_timeLeft / 30f);
				}
				if (_timeLeft > 1785)
				{
					return MathHelper.Lerp(1f, 0f, ((float)_timeLeft - 1785f) / 15f);
				}
				return 1f;
			}
		}

		private float Opacity
		{
			get
			{
				float scale = Scale;
				if (scale <= 0.5f)
				{
					return 0f;
				}
				return (scale - 0.5f) / 0.5f;
			}
		}

		public object CreationObject { get; private set; }

		public bool ShouldBeRemoved => _timeLeft <= 0;

		public PlayerWantsToJoinGamePopup(UserJoinToServerRequest request)
		{
			_request = request;
			CreationObject = request;
			_timeLeft = 1800;
		}

		public void Update()
		{
			_timeLeft--;
		}

		public void DrawInGame(SpriteBatch spriteBatch, Vector2 bottomAnchorPosition)
		{
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0070: Unknown result type (might be due to invalid IL or missing references)
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			//IL_007c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0089: Unknown result type (might be due to invalid IL or missing references)
			//IL_008e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0093: Unknown result type (might be due to invalid IL or missing references)
			//IL_0094: Unknown result type (might be due to invalid IL or missing references)
			//IL_0099: Unknown result type (might be due to invalid IL or missing references)
			//IL_009b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_0100: Unknown result type (might be due to invalid IL or missing references)
			//IL_010b: Unknown result type (might be due to invalid IL or missing references)
			//IL_016e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0173: Unknown result type (might be due to invalid IL or missing references)
			//IL_0184: Unknown result type (might be due to invalid IL or missing references)
			//IL_0189: Unknown result type (might be due to invalid IL or missing references)
			//IL_0190: Unknown result type (might be due to invalid IL or missing references)
			//IL_0195: Unknown result type (might be due to invalid IL or missing references)
			//IL_019a: Unknown result type (might be due to invalid IL or missing references)
			//IL_019e: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01af: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_025e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0263: Unknown result type (might be due to invalid IL or missing references)
			//IL_0274: Unknown result type (might be due to invalid IL or missing references)
			//IL_0279: Unknown result type (might be due to invalid IL or missing references)
			//IL_0280: Unknown result type (might be due to invalid IL or missing references)
			//IL_0285: Unknown result type (might be due to invalid IL or missing references)
			//IL_028a: Unknown result type (might be due to invalid IL or missing references)
			//IL_028e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0290: Unknown result type (might be due to invalid IL or missing references)
			//IL_029f: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_02db: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_0318: Unknown result type (might be due to invalid IL or missing references)
			//IL_0321: Unknown result type (might be due to invalid IL or missing references)
			//IL_0326: Unknown result type (might be due to invalid IL or missing references)
			//IL_0335: Unknown result type (might be due to invalid IL or missing references)
			//IL_033a: Unknown result type (might be due to invalid IL or missing references)
			//IL_033f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0342: Unknown result type (might be due to invalid IL or missing references)
			float opacity = Opacity;
			if (opacity > 0f)
			{
				string text = Utils.FormatWith(_request.GetUserWrapperText(), new
				{
					DisplayName = _request.UserDisplayName,
					FullId = _request.UserFullIdentifier
				});
				float num = Scale * 1.1f;
				Vector2 val = (FontAssets.ItemStack.Value.MeasureString(text) + new Vector2(58f, 10f)) * num;
				Rectangle r = Utils.CenteredRectangle(bottomAnchorPosition + new Vector2(0f, (0f - val.Y) * 0.5f), val);
				Vector2 mouseScreen = Main.MouseScreen;
				Color c = (r.Contains(mouseScreen.ToPoint()) ? (new Color(64, 109, 164) * 0.75f) : (new Color(64, 109, 164) * 0.5f));
				Utils.DrawInvBG(spriteBatch, r, c);
				Vector2 val2 = new Vector2((float)r.Left, (float)r.Center.Y);
				val2.X += 32f;
				Texture2D value = Main.Assets.Request<Texture2D>("Images/UI/ButtonPlay", (AssetRequestMode)1).Value;
				Vector2 val3 = new Vector2((float)(r.Left + 7), MathHelper.Lerp((float)r.Top, (float)r.Bottom, 0.5f) - (float)(value.Height / 2) - 1f);
				Rectangle val4 = Utils.CenteredRectangle(val3 + new Vector2((float)(value.Width / 2), 0f), value.Size());
				bool flag = val4.Contains(mouseScreen.ToPoint());
				spriteBatch.Draw(value, val3, (Rectangle?)null, Color.White * (flag ? 1f : 0.5f), 0f, new Vector2(0f, 0.5f) * value.Size(), 1f, (SpriteEffects)0, 0f);
				if (flag)
				{
					OnMouseOver();
				}
				value = Main.Assets.Request<Texture2D>("Images/UI/ButtonDelete", (AssetRequestMode)1).Value;
				val3 = new Vector2((float)(r.Left + 7), MathHelper.Lerp((float)r.Top, (float)r.Bottom, 0.5f) + (float)(value.Height / 2) + 1f);
				val4 = Utils.CenteredRectangle(val3 + new Vector2((float)(value.Width / 2), 0f), value.Size());
				flag = val4.Contains(mouseScreen.ToPoint());
				spriteBatch.Draw(value, val3, (Rectangle?)null, Color.White * (flag ? 1f : 0.5f), 0f, new Vector2(0f, 0.5f) * value.Size(), 1f, (SpriteEffects)0, 0f);
				if (flag)
				{
					OnMouseOver(reject: true);
				}
				Utils.DrawBorderString(color: new Color((int)Main.mouseTextColor, (int)Main.mouseTextColor, Main.mouseTextColor / 5, (int)Main.mouseTextColor) * opacity, sb: spriteBatch, text: text, pos: r.Center.ToVector2() + new Vector2(10f, 0f), scale: num * 0.9f, anchorx: 0.5f, anchory: 0.4f);
			}
		}

		private void OnMouseOver(bool reject = false)
		{
			if (PlayerInput.IgnoreMouseInterface)
			{
				return;
			}
			Main.player[Main.myPlayer].mouseInterface = true;
			if (Main.mouseLeft && Main.mouseLeftRelease)
			{
				Main.mouseLeftRelease = false;
				_timeLeft = 0;
				if (reject)
				{
					_request.Reject();
				}
				else
				{
					_request.Accept();
				}
			}
		}

		public void PushAnchor(ref Vector2 positionAnchorBottom)
		{
			float num = 70f * Opacity;
			positionAnchorBottom.Y -= num;
		}

		public void DrawInNotificationsArea(SpriteBatch spriteBatch, Rectangle area, ref int gamepadPointLocalIndexTouse)
		{
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_0087: Unknown result type (might be due to invalid IL or missing references)
			//IL_0091: Unknown result type (might be due to invalid IL or missing references)
			//IL_006d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0077: Unknown result type (might be due to invalid IL or missing references)
			//IL_0096: Unknown result type (might be due to invalid IL or missing references)
			//IL_0099: Unknown result type (might be due to invalid IL or missing references)
			//IL_009a: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_011b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0120: Unknown result type (might be due to invalid IL or missing references)
			//IL_0131: Unknown result type (might be due to invalid IL or missing references)
			//IL_0136: Unknown result type (might be due to invalid IL or missing references)
			//IL_013d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0142: Unknown result type (might be due to invalid IL or missing references)
			//IL_0147: Unknown result type (might be due to invalid IL or missing references)
			//IL_014b: Unknown result type (might be due to invalid IL or missing references)
			//IL_014c: Unknown result type (might be due to invalid IL or missing references)
			//IL_015a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0166: Unknown result type (might be due to invalid IL or missing references)
			//IL_017a: Unknown result type (might be due to invalid IL or missing references)
			//IL_018e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0195: Unknown result type (might be due to invalid IL or missing references)
			//IL_019a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0207: Unknown result type (might be due to invalid IL or missing references)
			//IL_020c: Unknown result type (might be due to invalid IL or missing references)
			//IL_021d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0222: Unknown result type (might be due to invalid IL or missing references)
			//IL_0229: Unknown result type (might be due to invalid IL or missing references)
			//IL_022e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0233: Unknown result type (might be due to invalid IL or missing references)
			//IL_0237: Unknown result type (might be due to invalid IL or missing references)
			//IL_0238: Unknown result type (might be due to invalid IL or missing references)
			//IL_0246: Unknown result type (might be due to invalid IL or missing references)
			//IL_0252: Unknown result type (might be due to invalid IL or missing references)
			//IL_0266: Unknown result type (might be due to invalid IL or missing references)
			//IL_027a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0281: Unknown result type (might be due to invalid IL or missing references)
			//IL_0286: Unknown result type (might be due to invalid IL or missing references)
			//IL_02cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d6: Unknown result type (might be due to invalid IL or missing references)
			string userWrapperText = _request.GetUserWrapperText();
			string text = _request.UserDisplayName;
			Utils.TrimTextIfNeeded(ref text, FontAssets.MouseText.Value, 0.9f, area.Width / 4);
			string text2 = Utils.FormatWith(userWrapperText, new
			{
				DisplayName = text,
				FullId = _request.UserFullIdentifier
			});
			Vector2 mouseScreen = Main.MouseScreen;
			Color c = (area.Contains(mouseScreen.ToPoint()) ? (new Color(64, 109, 164) * 0.75f) : (new Color(64, 109, 164) * 0.5f));
			Utils.DrawInvBG(spriteBatch, area, c);
			Vector2 pos = new Vector2((float)area.Left, (float)area.Center.Y);
			pos.X += 32f;
			Texture2D value = Main.Assets.Request<Texture2D>("Images/UI/ButtonPlay", (AssetRequestMode)1).Value;
			Vector2 val = new Vector2((float)(area.Left + 7), MathHelper.Lerp((float)area.Top, (float)area.Bottom, 0.5f) - (float)(value.Height / 2) - 1f);
			Rectangle val2 = Utils.CenteredRectangle(val + new Vector2((float)(value.Width / 2), 0f), value.Size());
			bool flag = val2.Contains(mouseScreen.ToPoint());
			spriteBatch.Draw(value, val, (Rectangle?)null, Color.White * (flag ? 1f : 0.5f), 0f, new Vector2(0f, 0.5f) * value.Size(), 1f, (SpriteEffects)0, 0f);
			if (flag)
			{
				OnMouseOver();
			}
			value = Main.Assets.Request<Texture2D>("Images/UI/ButtonDelete", (AssetRequestMode)1).Value;
			val = new Vector2((float)(area.Left + 7), MathHelper.Lerp((float)area.Top, (float)area.Bottom, 0.5f) + (float)(value.Height / 2) + 1f);
			val2 = Utils.CenteredRectangle(val + new Vector2((float)(value.Width / 2), 0f), value.Size());
			flag = val2.Contains(mouseScreen.ToPoint());
			spriteBatch.Draw(value, val, (Rectangle?)null, Color.White * (flag ? 1f : 0.5f), 0f, new Vector2(0f, 0.5f) * value.Size(), 1f, (SpriteEffects)0, 0f);
			if (flag)
			{
				OnMouseOver(reject: true);
			}
			pos.X += 6f;
			Utils.DrawBorderString(color: new Color((int)Main.mouseTextColor, (int)Main.mouseTextColor, Main.mouseTextColor / 5, (int)Main.mouseTextColor), sb: spriteBatch, text: text2, pos: pos, scale: 0.9f, anchorx: 0f, anchory: 0.4f);
		}
	}
}
