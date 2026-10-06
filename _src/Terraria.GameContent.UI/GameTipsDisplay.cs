using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using Terraria.Localization;
using Terraria.UI.Chat;

namespace Terraria.GameContent.UI;

public class GameTipsDisplay
{
	private class GameTip
	{
		private const float APPEAR_FROM = 2.5f;

		private const float APPEAR_TO = 0.5f;

		private const float DISAPPEAR_TO = -1.5f;

		private const float APPEAR_TIME = 0.5f;

		private const float DISAPPEAR_TIME = 1f;

		private const float DURATION = 11.5f;

		private LocalizedText _textKey;

		private string _formattedText;

		public float ScreenAnchorX;

		public readonly float Duration;

		public readonly double SpawnTime;

		public string Text
		{
			get
			{
				if (_textKey == null)
				{
					return "What?!";
				}
				return _formattedText;
			}
		}

		public bool IsExpired(double currentTime)
		{
			return currentTime >= SpawnTime + (double)Duration;
		}

		public bool IsExpiring(double currentTime)
		{
			return currentTime >= SpawnTime + (double)Duration - 1.0;
		}

		public GameTip(string textKey, double spawnTime)
		{
			_textKey = Language.GetText(textKey);
			SpawnTime = spawnTime;
			ScreenAnchorX = 2.5f;
			Duration = 11.5f;
			_formattedText = _textKey.Value;
		}

		public void Update(double currentTime)
		{
			double num = currentTime - SpawnTime;
			if (num < 0.5)
			{
				ScreenAnchorX = MathHelper.SmoothStep(2.5f, 0.5f, (float)Utils.GetLerpValue(0.0, 0.5, num, clamped: true));
			}
			else if (num >= (double)(Duration - 1f))
			{
				ScreenAnchorX = MathHelper.SmoothStep(0.5f, -1.5f, (float)Utils.GetLerpValue(Duration - 1f, Duration, num, clamped: true));
			}
			else
			{
				ScreenAnchorX = 0.5f;
			}
		}
	}

	private readonly List<GameTip> _currentTips = new List<GameTip>();

	private ITipProvider _tipProvider;

	public float TipOffsetY = -150f;

	public GameTipsDisplay(ITipProvider tipProvider)
	{
		_tipProvider = tipProvider;
	}

	public void Update()
	{
		double time = Main.gameTimeCache.TotalGameTime.TotalSeconds;
		_currentTips.RemoveAll((GameTip x) => x.IsExpired(time));
		bool flag = true;
		foreach (GameTip currentTip in _currentTips)
		{
			if (!currentTip.IsExpiring(time))
			{
				flag = false;
				break;
			}
		}
		if (flag)
		{
			AddNewTip(time);
		}
		foreach (GameTip currentTip2 in _currentTips)
		{
			currentTip2.Update(time);
		}
	}

	public void ClearTips()
	{
		_currentTips.Clear();
	}

	public void Draw()
	{
		//IL_0182: Unknown result type (might be due to invalid IL or missing references)
		//IL_0187: Unknown result type (might be due to invalid IL or missing references)
		//IL_0197: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_021e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0220: Unknown result type (might be due to invalid IL or missing references)
		//IL_022a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0233: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0203: Unknown result type (might be due to invalid IL or missing references)
		SpriteBatch spriteBatch = Main.spriteBatch;
		float num = Main.screenWidth;
		float num2 = (float)Main.screenHeight + TipOffsetY;
		float num3 = (float)Main.screenWidth * 0.5f;
		foreach (GameTip currentTip in _currentTips)
		{
			if (currentTip.ScreenAnchorX < -0.5f || currentTip.ScreenAnchorX > 1.5f)
			{
				continue;
			}
			DynamicSpriteFont value = FontAssets.MouseText.Value;
			string text = value.CreateWrappedText(currentTip.Text, num3, Language.ActiveCulture.CultureInfo);
			if (text.Split('\n').Length > 2)
			{
				text = value.CreateWrappedText(currentTip.Text, num3 * 1.5f - 50f, Language.ActiveCulture.CultureInfo);
			}
			if (Main.vampireSeed)
			{
				text = Language.GetTextValue("Misc.Vampirism");
			}
			else if (WorldGen.getGoodWorldGen)
			{
				string text2 = "";
				for (int num4 = text.Length - 1; num4 >= 0; num4--)
				{
					text2 += text.Substring(num4, 1);
				}
				text = text2;
			}
			else if (WorldGen.drunkWorldGenText)
			{
				text = string.Concat(Main.rand.Next(999999999));
				for (int i = 0; i < 14; i++)
				{
					if (Main.rand.Next(2) == 0)
					{
						text += Main.rand.Next(999999999);
					}
				}
			}
			Vector2 val = value.MeasureString(text);
			float num5 = 1.1f;
			float num6 = 110f;
			if (val.Y > num6)
			{
				num5 = num6 / val.Y;
			}
			Vector2 val2 = new Vector2(num * currentTip.ScreenAnchorX, num2);
			val2 -= val * num5 * 0.5f;
			if (WorldGen.tenthAnniversaryWorldGen && !Main.zenithWorld)
			{
				ChatManager.DrawColorCodedStringWithShadow(spriteBatch, value, text, val2, Color.HotPink, 0f, Vector2.Zero, new Vector2(num5, num5));
			}
			else
			{
				ChatManager.DrawColorCodedStringWithShadow(spriteBatch, value, text, val2, Color.White, 0f, Vector2.Zero, new Vector2(num5, num5));
			}
		}
	}

	private void AddNewTip(double currentTime)
	{
		string textKey = "UI.Back";
		string key = _tipProvider.RollAvailableTip().Key;
		if (Language.Exists(key))
		{
			textKey = key;
		}
		_currentTips.Add(new GameTip(textKey, currentTime));
	}
}
