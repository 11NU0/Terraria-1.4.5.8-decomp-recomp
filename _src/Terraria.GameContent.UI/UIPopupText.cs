using Microsoft.Xna.Framework;

namespace Terraria.GameContent.UI;

public class UIPopupText
{
	public Vector2 position;

	public Vector2 velocity;

	public float alpha;

	public int alphaDir = 1;

	public string name;

	public string displayText;

	public float scale = 1f;

	public float rotation;

	public Color color;

	public bool active;

	public int lifeTime;

	public int framesSinceSpawn;

	public static int activeTime = 60;

	public UIPopupTextContext context;

	public float TargetScale => 1f;

	public void PrepareDisplayText()
	{
		displayText = name;
	}

	public void Update(int whoAmI, UIPopupTextManager manager)
	{
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_010b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0126: Unknown result type (might be due to invalid IL or missing references)
		//IL_0135: Unknown result type (might be due to invalid IL or missing references)
		//IL_013d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0145: Unknown result type (might be due to invalid IL or missing references)
		//IL_014c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0272: Unknown result type (might be due to invalid IL or missing references)
		//IL_0278: Unknown result type (might be due to invalid IL or missing references)
		//IL_027d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0282: Unknown result type (might be due to invalid IL or missing references)
		if (!active)
		{
			return;
		}
		framesSinceSpawn++;
		float targetScale = TargetScale;
		alpha += (float)alphaDir * 0.01f;
		if ((double)alpha <= 0.7)
		{
			alpha = 0.7f;
			alphaDir = 1;
		}
		if (alpha >= 1f)
		{
			alpha = 1f;
			alphaDir = -1;
		}
		bool flag = false;
		Vector2 textHitbox = GetTextHitbox();
		Rectangle val = new Rectangle((int)(position.X - textHitbox.X / 2f), (int)(position.Y - textHitbox.Y / 2f), (int)textHitbox.X, (int)textHitbox.Y);
		for (int i = 0; i < 20; i++)
		{
			UIPopupText uIPopupText = manager.popupText[i];
			if (!uIPopupText.active || i == whoAmI)
			{
				continue;
			}
			Vector2 textHitbox2 = uIPopupText.GetTextHitbox();
			Rectangle val2 = new Rectangle((int)(uIPopupText.position.X - textHitbox2.X / 2f), (int)(uIPopupText.position.Y - textHitbox2.Y / 2f), (int)textHitbox2.X, (int)textHitbox2.Y);
			if (val.Intersects(val2) && (position.Y < uIPopupText.position.Y || (position.Y == uIPopupText.position.Y && whoAmI < i)))
			{
				flag = true;
				int num = manager.numActive;
				if (num > 3)
				{
					num = 3;
				}
				uIPopupText.lifeTime = activeTime + 15 * num;
				lifeTime = activeTime + 15 * num;
			}
		}
		if (!flag)
		{
			if (context != UIPopupTextContext.SpecialSeed || (scale != targetScale && lifeTime > 0))
			{
				velocity.Y *= 0.86f;
				if (scale == targetScale)
				{
					velocity.Y *= 0.4f;
				}
			}
		}
		else if (velocity.Y > -6f)
		{
			velocity.Y -= 0.2f;
		}
		else
		{
			velocity.Y *= 0.86f;
		}
		velocity.X *= 0.93f;
		position += velocity;
		lifeTime--;
		if (lifeTime <= 0)
		{
			scale -= 0.03f * targetScale;
			if ((double)scale < 0.1 * (double)targetScale)
			{
				active = false;
			}
			lifeTime = 0;
			return;
		}
		if (scale < targetScale)
		{
			scale += 0.1f * targetScale;
		}
		if (scale > targetScale)
		{
			scale = targetScale;
		}
	}

	public Vector2 GetTextHitbox()
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		string text = displayText;
		Vector2 val = FontAssets.MouseText.Value.MeasureString(text);
		val *= scale;
		val.Y *= 0.8f;
		return val;
	}
}
