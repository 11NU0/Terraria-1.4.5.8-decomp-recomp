using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using ReLogic.Graphics;
using Terraria.Audio;
using Terraria.Social;
using Terraria.Social.Base;

namespace Terraria.GameContent.UI;

public class WorkshopPublishingIndicator
{
	private float _displayUpPercent;

	private int _frameCounter;

	private bool _shouldPlayEndingSound;

	private Asset<Texture2D> _indicatorTexture;

	private int _timesSoundWasPlayed;

	public void Hide()
	{
		_displayUpPercent = 0f;
		_frameCounter = 0;
		_timesSoundWasPlayed = 0;
		_shouldPlayEndingSound = false;
	}

	public void Draw(SpriteBatch spriteBatch)
	{
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0100: Unknown result type (might be due to invalid IL or missing references)
		//IL_0105: Unknown result type (might be due to invalid IL or missing references)
		//IL_0110: Unknown result type (might be due to invalid IL or missing references)
		//IL_0115: Unknown result type (might be due to invalid IL or missing references)
		//IL_0117: Unknown result type (might be due to invalid IL or missing references)
		//IL_0119: Unknown result type (might be due to invalid IL or missing references)
		//IL_0123: Unknown result type (might be due to invalid IL or missing references)
		//IL_0128: Unknown result type (might be due to invalid IL or missing references)
		//IL_012d: Unknown result type (might be due to invalid IL or missing references)
		//IL_012f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0136: Unknown result type (might be due to invalid IL or missing references)
		//IL_0140: Unknown result type (might be due to invalid IL or missing references)
		//IL_0186: Unknown result type (might be due to invalid IL or missing references)
		//IL_018e: Unknown result type (might be due to invalid IL or missing references)
		//IL_019d: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cc: Unknown result type (might be due to invalid IL or missing references)
		WorkshopSocialModule workshop = SocialAPI.Workshop;
		if (workshop == null)
		{
			return;
		}
		AWorkshopProgressReporter progressReporter = workshop.ProgressReporter;
		bool hasOngoingTasks = progressReporter.HasOngoingTasks;
		bool flag = _displayUpPercent == 1f;
		_displayUpPercent = MathHelper.Clamp(_displayUpPercent + (float)hasOngoingTasks.ToDirectionInt() / 60f, 0f, 1f);
		bool flag2 = _displayUpPercent == 1f;
		if (flag && !flag2)
		{
			_shouldPlayEndingSound = true;
		}
		if (_displayUpPercent == 0f)
		{
			return;
		}
		if (_indicatorTexture == null)
		{
			_indicatorTexture = Main.Assets.Request<Texture2D>("Images/UI/Workshop/InProgress", (AssetRequestMode)1);
		}
		Texture2D value = _indicatorTexture.Value;
		int num = 6;
		_frameCounter++;
		int num2 = 5;
		int num3 = _frameCounter / num2 % num;
		Vector2 val = Main.ScreenSize.ToVector2() + new Vector2(-40f, 40f);
		Vector2 val2 = val + new Vector2(0f, -80f);
		Vector2 val3 = Vector2.Lerp(val, val2, _displayUpPercent);
		Rectangle val4 = value.Frame(1, 6, 0, num3);
		Vector2 val5 = val4.Size() / 2f;
		spriteBatch.Draw(value, val3, (Rectangle?)val4, Color.White, 0f, val5, 1f, (SpriteEffects)0, 0f);
		if (progressReporter.TryGetProgress(out var progress) && !float.IsNaN(progress))
		{
			string text = progress.ToString("P");
			DynamicSpriteFont value2 = FontAssets.ItemStack.Value;
			int num4 = 1;
			Vector2 origin = value2.MeasureString(text) * (float)num4 * new Vector2(0.5f, 1f);
			Utils.DrawBorderStringFourWay(spriteBatch, value2, text, val3.X, val3.Y - 10f, Color.White, Color.Black, origin, num4);
		}
		if (num3 == 3 && _frameCounter % num2 == 0)
		{
			if (_shouldPlayEndingSound)
			{
				_shouldPlayEndingSound = false;
				_timesSoundWasPlayed = 0;
				SoundEngine.PlaySound(64);
			}
			if (hasOngoingTasks)
			{
				float volumeScale = Utils.Remap(_timesSoundWasPlayed, 0f, 10f, 1f, 0f);
				SoundEngine.PlaySound(21, -1, -1, 1, volumeScale);
				_timesSoundWasPlayed++;
			}
		}
	}
}
