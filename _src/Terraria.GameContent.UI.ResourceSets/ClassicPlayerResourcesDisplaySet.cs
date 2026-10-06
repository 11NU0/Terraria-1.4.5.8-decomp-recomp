using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using Terraria.DataStructures;

namespace Terraria.GameContent.UI.ResourceSets;

public class ClassicPlayerResourcesDisplaySet : IPlayerResourcesDisplaySet, IConfigKeyHolder
{
	private int UIDisplay_ManaPerStar = 20;

	private float UIDisplay_LifePerHeart = 20f;

	private int UI_ScreenAnchorX;

	public string NameKey { get; private set; }

	public string ConfigKey { get; private set; }

	public ClassicPlayerResourcesDisplaySet(string nameKey, string configKey)
	{
		NameKey = nameKey;
		ConfigKey = configKey;
	}

	public void Draw()
	{
		UI_ScreenAnchorX = Main.screenWidth - 800;
		DrawLife();
		DrawMana();
	}

	private void DrawLife()
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0114: Unknown result type (might be due to invalid IL or missing references)
		//IL_0119: Unknown result type (might be due to invalid IL or missing references)
		//IL_0149: Unknown result type (might be due to invalid IL or missing references)
		//IL_0164: Unknown result type (might be due to invalid IL or missing references)
		//IL_0169: Unknown result type (might be due to invalid IL or missing references)
		//IL_0171: Unknown result type (might be due to invalid IL or missing references)
		//IL_0177: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0212: Unknown result type (might be due to invalid IL or missing references)
		//IL_0221: Unknown result type (might be due to invalid IL or missing references)
		//IL_047e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0499: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_04cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_03db: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ff: Unknown result type (might be due to invalid IL or missing references)
		Player localPlayer = Main.LocalPlayer;
		SpriteBatch spriteBatch = Main.spriteBatch;
		Color val = new Color((int)Main.mouseTextColor, (int)Main.mouseTextColor, (int)Main.mouseTextColor, (int)Main.mouseTextColor);
		UIDisplay_LifePerHeart = 20f;
		if (localPlayer.ghost)
		{
			return;
		}
		int num = localPlayer.statLifeMax / 20;
		int num2 = (localPlayer.statLifeMax - 400) / 5;
		if (num2 < 0)
		{
			num2 = 0;
		}
		if (num2 > 0)
		{
			num = localPlayer.statLifeMax / (20 + num2 / 4);
			UIDisplay_LifePerHeart = (float)localPlayer.statLifeMax / 20f;
		}
		int num3 = localPlayer.statLifeMax2 - localPlayer.statLifeMax;
		UIDisplay_LifePerHeart += num3 / num;
		int num4 = (int)((float)localPlayer.statLifeMax2 / UIDisplay_LifePerHeart);
		if (num4 >= 10)
		{
			num4 = 10;
		}
		string text = Lang.inter[0].Value + " " + localPlayer.statLifeMax2 + "/" + localPlayer.statLifeMax2;
		Vector2 val2 = FontAssets.MouseText.Value.MeasureString(text);
		if (!localPlayer.ghost)
		{
			DynamicSpriteFontExtensionMethods.DrawString(spriteBatch, FontAssets.MouseText.Value, Lang.inter[0].Value, new Vector2((float)(500 + 13 * num4) - val2.X * 0.5f + (float)UI_ScreenAnchorX, 6f), val, 0f, default(Vector2), 1f, (SpriteEffects)0, 0f, (Vector2[])null, (Color[])null);
			DynamicSpriteFontExtensionMethods.DrawString(spriteBatch, FontAssets.MouseText.Value, localPlayer.statLife + "/" + localPlayer.statLifeMax2, new Vector2((float)(500 + 13 * num4) + val2.X * 0.5f + (float)UI_ScreenAnchorX, 6f), val, 0f, new Vector2(FontAssets.MouseText.Value.MeasureString(localPlayer.statLife + "/" + localPlayer.statLifeMax2).X, 0f), 1f, (SpriteEffects)0, 0f, (Vector2[])null, (Color[])null);
		}
		for (int i = 1; i < (int)((float)localPlayer.statLifeMax2 / UIDisplay_LifePerHeart) + 1; i++)
		{
			int num5 = 255;
			float num6 = 1f;
			bool flag = false;
			if ((float)localPlayer.statLife >= (float)i * UIDisplay_LifePerHeart)
			{
				num5 = 255;
				if ((float)localPlayer.statLife == (float)i * UIDisplay_LifePerHeart)
				{
					flag = true;
				}
			}
			else
			{
				float num7 = ((float)localPlayer.statLife - (float)(i - 1) * UIDisplay_LifePerHeart) / UIDisplay_LifePerHeart;
				num5 = (int)(30f + 225f * num7);
				if (num5 < 30)
				{
					num5 = 30;
				}
				num6 = num7 / 4f + 0.75f;
				if ((double)num6 < 0.75)
				{
					num6 = 0.75f;
				}
				if (num7 > 0f)
				{
					flag = true;
				}
			}
			if (flag)
			{
				num6 += Main.cursorScale - 1f;
			}
			int num8 = 0;
			int num9 = 0;
			if (i > 10)
			{
				num8 -= 260;
				num9 += 26;
			}
			int num10 = (int)((double)num5 * 0.9);
			if (!localPlayer.ghost)
			{
				if (num2 > 0)
				{
					num2--;
					spriteBatch.Draw(TextureAssets.Heart2.Value, new Vector2((float)(500 + 26 * (i - 1) + num8 + UI_ScreenAnchorX + TextureAssets.Heart.Width() / 2), 32f + ((float)TextureAssets.Heart.Height() - (float)TextureAssets.Heart.Height() * num6) / 2f + (float)num9 + (float)(TextureAssets.Heart.Height() / 2)), (Rectangle?)new Rectangle(0, 0, TextureAssets.Heart.Width(), TextureAssets.Heart.Height()), new Color(num5, num5, num5, num10), 0f, new Vector2((float)(TextureAssets.Heart.Width() / 2), (float)(TextureAssets.Heart.Height() / 2)), num6, (SpriteEffects)0, 0f);
				}
				else
				{
					spriteBatch.Draw(TextureAssets.Heart.Value, new Vector2((float)(500 + 26 * (i - 1) + num8 + UI_ScreenAnchorX + TextureAssets.Heart.Width() / 2), 32f + ((float)TextureAssets.Heart.Height() - (float)TextureAssets.Heart.Height() * num6) / 2f + (float)num9 + (float)(TextureAssets.Heart.Height() / 2)), (Rectangle?)new Rectangle(0, 0, TextureAssets.Heart.Width(), TextureAssets.Heart.Height()), new Color(num5, num5, num5, num10), 0f, new Vector2((float)(TextureAssets.Heart.Width() / 2), (float)(TextureAssets.Heart.Height() / 2)), num6, (SpriteEffects)0, 0f);
				}
			}
		}
	}

	private void DrawMana()
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_021a: Unknown result type (might be due to invalid IL or missing references)
		//IL_022c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0250: Unknown result type (might be due to invalid IL or missing references)
		Player localPlayer = Main.LocalPlayer;
		SpriteBatch spriteBatch = Main.spriteBatch;
		Color val = new Color((int)Main.mouseTextColor, (int)Main.mouseTextColor, (int)Main.mouseTextColor, (int)Main.mouseTextColor);
		UIDisplay_ManaPerStar = 20;
		if (localPlayer.ghost || localPlayer.statManaMax2 <= 0)
		{
			return;
		}
		_ = localPlayer.statManaMax2 / 20;
		Vector2 val2 = FontAssets.MouseText.Value.MeasureString(Lang.inter[2].Value);
		int num = 50;
		if (val2.X >= 45f)
		{
			num = (int)val2.X + 5;
		}
		DynamicSpriteFontExtensionMethods.DrawString(spriteBatch, FontAssets.MouseText.Value, Lang.inter[2].Value, new Vector2((float)(800 - num + UI_ScreenAnchorX), 6f), val, 0f, default(Vector2), 1f, (SpriteEffects)0, 0f, (Vector2[])null, (Color[])null);
		for (int i = 1; i < localPlayer.statManaMax2 / UIDisplay_ManaPerStar + 1; i++)
		{
			int num2 = 255;
			bool flag = false;
			float num3 = 1f;
			if (localPlayer.statMana >= i * UIDisplay_ManaPerStar)
			{
				num2 = 255;
				if (localPlayer.statMana == i * UIDisplay_ManaPerStar)
				{
					flag = true;
				}
			}
			else
			{
				float num4 = (float)(localPlayer.statMana - (i - 1) * UIDisplay_ManaPerStar) / (float)UIDisplay_ManaPerStar;
				num2 = (int)(30f + 225f * num4);
				if (num2 < 30)
				{
					num2 = 30;
				}
				num3 = num4 / 4f + 0.75f;
				if ((double)num3 < 0.75)
				{
					num3 = 0.75f;
				}
				if (num4 > 0f)
				{
					flag = true;
				}
			}
			if (flag)
			{
				num3 += Main.cursorScale - 1f;
			}
			int num5 = (int)((double)num2 * 0.9);
			spriteBatch.Draw(TextureAssets.Mana.Value, new Vector2((float)(775 + UI_ScreenAnchorX), (float)(30 + TextureAssets.Mana.Height() / 2) + ((float)TextureAssets.Mana.Height() - (float)TextureAssets.Mana.Height() * num3) / 2f + (float)(28 * (i - 1))), (Rectangle?)new Rectangle(0, 0, TextureAssets.Mana.Width(), TextureAssets.Mana.Height()), new Color(num2, num2, num2, num5), 0f, new Vector2((float)(TextureAssets.Mana.Width() / 2), (float)(TextureAssets.Mana.Height() / 2)), num3, (SpriteEffects)0, 0f);
		}
	}

	public void TryToHover()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		Vector2 mouseScreen = Main.MouseScreen;
		Player localPlayer = Main.LocalPlayer;
		int num = 26 * localPlayer.statLifeMax2 / (int)UIDisplay_LifePerHeart;
		int num2 = 0;
		if (localPlayer.statLifeMax2 > 200)
		{
			num = 260;
			num2 += 26;
		}
		if (mouseScreen.X > (float)(500 + UI_ScreenAnchorX) && mouseScreen.X < (float)(500 + num + UI_ScreenAnchorX) && mouseScreen.Y > 32f && mouseScreen.Y < (float)(32 + TextureAssets.Heart.Height() + num2))
		{
			CommonResourceBarMethods.DrawLifeMouseOver();
		}
		num = 24;
		num2 = 28 * localPlayer.statManaMax2 / UIDisplay_ManaPerStar;
		if (mouseScreen.X > (float)(762 + UI_ScreenAnchorX) && mouseScreen.X < (float)(762 + num + UI_ScreenAnchorX) && mouseScreen.Y > 30f && mouseScreen.Y < (float)(30 + num2))
		{
			CommonResourceBarMethods.DrawManaMouseOver();
		}
	}
}
