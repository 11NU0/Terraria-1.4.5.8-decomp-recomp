using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Testing;

namespace Terraria.GameContent.Events;

public class ScreenObstruction
{
	public static float lastSpeed = 0.1f;

	public static float screenObstruction;

	public static void Update(SceneState sceneState, SceneMetrics metrics)
	{
		float num = 0f;
		float amount = 0.1f;
		if (metrics.PerspectivePlayer.insideUnbreakableWalls)
		{
			int progressPlayerCanSafelyMatch = DangerousDungeonCurse.GetProgressPlayerCanSafelyMatch();
			int num2 = DangerousDungeonCurse.GetProgressPlayerNeedsToMatch(metrics.PerspectivePlayer) - progressPlayerCanSafelyMatch;
			if (num2 > 0)
			{
				float max = 0.9f;
				if (DebugOptions.devLightTilesCheat)
				{
					max = 0.25f;
				}
				num = Utils.Clamp(0.4f * (float)num2, 0f, max);
				amount = (lastSpeed = 0.01f);
			}
		}
		if (metrics.PerspectivePlayer.headcovered)
		{
			num = 0.95f;
			amount = (lastSpeed = 0.3f);
		}
		if (num == 0f && screenObstruction != 0f)
		{
			amount = lastSpeed;
		}
		else
		{
			lastSpeed = amount;
		}
		sceneState.MoveTowards(ref screenObstruction, num, amount);
	}

	public static void Draw(SpriteBatch spriteBatch)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_010b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0110: Unknown result type (might be due to invalid IL or missing references)
		//IL_0115: Unknown result type (might be due to invalid IL or missing references)
		//IL_0121: Unknown result type (might be due to invalid IL or missing references)
		//IL_0138: Unknown result type (might be due to invalid IL or missing references)
		//IL_013d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0142: Unknown result type (might be due to invalid IL or missing references)
		//IL_014f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0155: Unknown result type (might be due to invalid IL or missing references)
		//IL_015f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0170: Unknown result type (might be due to invalid IL or missing references)
		//IL_0176: Unknown result type (might be due to invalid IL or missing references)
		//IL_0180: Unknown result type (might be due to invalid IL or missing references)
		//IL_0191: Unknown result type (might be due to invalid IL or missing references)
		//IL_0197: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d7: Unknown result type (might be due to invalid IL or missing references)
		if (screenObstruction != 0f)
		{
			Color val = Color.Black * screenObstruction;
			int num = TextureAssets.Extra[49].Width();
			int num2 = 10;
			Rectangle rect = Main.SceneMetrics.PerspectivePlayer.getRect();
			rect.Inflate((num - rect.Width) / 2, (num - rect.Height) / 2 + num2 / 2);
			rect.Offset(-(int)Main.screenPosition.X, -(int)Main.screenPosition.Y + (int)Main.player[Main.myPlayer].gfxOffY - num2);
			Rectangle val2 = Rectangle.Union(new Rectangle(0, 0, 1, 1), new Rectangle(rect.Right - 1, rect.Top - 1, 1, 1));
			Rectangle val3 = Rectangle.Union(new Rectangle(Main.screenWidth - 1, 0, 1, 1), new Rectangle(rect.Right, rect.Bottom - 1, 1, 1));
			Rectangle val4 = Rectangle.Union(new Rectangle(Main.screenWidth - 1, Main.screenHeight - 1, 1, 1), new Rectangle(rect.Left, rect.Bottom, 1, 1));
			Rectangle val5 = Rectangle.Union(new Rectangle(0, Main.screenHeight - 1, 1, 1), new Rectangle(rect.Left - 1, rect.Top, 1, 1));
			spriteBatch.Draw(TextureAssets.MagicPixel.Value, val2, (Rectangle?)new Rectangle(0, 0, 1, 1), val);
			spriteBatch.Draw(TextureAssets.MagicPixel.Value, val3, (Rectangle?)new Rectangle(0, 0, 1, 1), val);
			spriteBatch.Draw(TextureAssets.MagicPixel.Value, val4, (Rectangle?)new Rectangle(0, 0, 1, 1), val);
			spriteBatch.Draw(TextureAssets.MagicPixel.Value, val5, (Rectangle?)new Rectangle(0, 0, 1, 1), val);
			spriteBatch.Draw(TextureAssets.Extra[49].Value, rect, val);
		}
	}
}
