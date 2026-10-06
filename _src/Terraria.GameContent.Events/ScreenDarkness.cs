using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Terraria.GameContent.Events;

public class ScreenDarkness
{
	public static float screenObstruction;

	public static Color frontColor = new Color(0, 0, 120);

	public static void Update(SceneState sceneState, SceneMetrics metrics)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_0128: Unknown result type (might be due to invalid IL or missing references)
		//IL_012d: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		float target = 0f;
		float amount = 1f / 60f;
		Vector2 center = metrics.Center;
		for (int i = 0; i < Main.maxNPCs; i++)
		{
			if (Main.npc[i].active && Main.npc[i].type == 370 && Main.npc[i].Distance(center) < 3000f && (Main.npc[i].ai[0] >= 10f || (Main.npc[i].ai[0] == 9f && Main.npc[i].ai[2] > 120f)))
			{
				target = 0.95f;
				frontColor = new Color(0, 0, 120) * 0.3f;
				amount = 0.03f;
			}
			if (Main.npc[i].active && Main.npc[i].type == 113 && Main.npc[i].Distance(center) < 3000f)
			{
				float num = Utils.Remap(Main.npc[i].Distance(center), 2000f, 3000f, 1f, 0f);
				target = Main.npc[i].localAI[1] * num;
				amount = 1f;
				frontColor = Color.Black;
			}
		}
		sceneState.MoveTowards(ref screenObstruction, target, amount);
	}

	public static void DrawBack(SpriteBatch spriteBatch)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		if (screenObstruction != 0f)
		{
			Color val = Color.Black * screenObstruction;
			spriteBatch.Draw(TextureAssets.MagicPixel.Value, new Rectangle(-2, -2, Main.screenWidth + 4, Main.screenHeight + 4), (Rectangle?)new Rectangle(0, 0, 1, 1), val);
		}
	}

	public static void DrawFront(SpriteBatch spriteBatch)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		if (screenObstruction != 0f)
		{
			Color val = frontColor * screenObstruction;
			spriteBatch.Draw(TextureAssets.MagicPixel.Value, new Rectangle(-2, -2, Main.screenWidth + 4, Main.screenHeight + 4), (Rectangle?)new Rectangle(0, 0, 1, 1), val);
		}
	}

	static ScreenDarkness()
	{
		//IL_0004: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
	}
}
