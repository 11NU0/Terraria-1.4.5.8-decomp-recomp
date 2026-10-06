using Microsoft.Xna.Framework;
using Terraria.GameContent;

namespace Terraria;

public class CombatText
{
	public static readonly Color DamagedFriendly = new Color(255, 80, 90, 255);

	public static readonly Color DamagedFriendlyCrit = new Color(255, 100, 30, 255);

	public static readonly Color DamagedHostile = new Color(255, 160, 80, 255);

	public static readonly Color DamagedHostileCrit = new Color(255, 100, 30, 255);

	public static readonly Color OthersDamagedHostile = DamagedHostile * 0.4f;

	public static readonly Color OthersDamagedHostileCrit = DamagedHostileCrit * 0.4f;

	public static readonly Color HealLife = new Color(100, 255, 100, 255);

	public static readonly Color HealMana = new Color(100, 100, 255, 255);

	public static readonly Color LifeRegen = new Color(255, 60, 70, 255);

	public static readonly Color LifeRegenNegative = new Color(255, 140, 40, 255);

	public Vector2 position;

	public Vector2 velocity;

	public float alpha;

	public int alphaDir = 1;

	public string text = "";

	public float scale = 1f;

	public float rotation;

	public Color color;

	public bool active;

	public int lifeTime;

	public bool crit;

	public bool dot;

	public static float TargetScale => 1f;

	public static int NewText(Rectangle location, Color color, int amount, bool dramatic = false, bool dot = false)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		return NewText(location, color, amount.ToString(), dramatic, dot);
	}

	public static int NewText(Rectangle location, Color color, string text, bool dramatic = false, bool dot = false)
	{
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_011b: Unknown result type (might be due to invalid IL or missing references)
		//IL_012e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0162: Unknown result type (might be due to invalid IL or missing references)
		//IL_0175: Unknown result type (might be due to invalid IL or missing references)
		//IL_0198: Unknown result type (might be due to invalid IL or missing references)
		//IL_0199: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0205: Unknown result type (might be due to invalid IL or missing references)
		//IL_0213: Unknown result type (might be due to invalid IL or missing references)
		if (Main.netMode == 2)
		{
			return 100;
		}
		for (int i = 0; i < 100; i++)
		{
			if (Main.combatText[i].active)
			{
				continue;
			}
			int num = 0;
			if (dramatic)
			{
				num = 1;
			}
			Vector2 val = FontAssets.CombatText[num].Value.MeasureString(text);
			if (Main.NoPooling)
			{
				Main.combatText[i] = new CombatText();
			}
			Main.combatText[i].alpha = 1f;
			Main.combatText[i].alphaDir = -1;
			Main.combatText[i].active = true;
			Main.combatText[i].scale = 0f;
			Main.combatText[i].rotation = 0f;
			Main.combatText[i].position.X = (float)location.X + (float)location.Width * 0.5f - val.X * 0.5f;
			Main.combatText[i].position.Y = (float)location.Y + (float)location.Height * 0.25f - val.Y * 0.5f;
			Main.combatText[i].position.X += Main.rand.Next(-(int)((double)location.Width * 0.5), (int)((double)location.Width * 0.5) + 1);
			Main.combatText[i].position.Y += Main.rand.Next(-(int)((double)location.Height * 0.5), (int)((double)location.Height * 0.5) + 1);
			Main.combatText[i].color = color;
			Main.combatText[i].text = text;
			Main.combatText[i].velocity.Y = -7f;
			if (Main.player[Main.myPlayer].gravDir == -1f)
			{
				Main.combatText[i].velocity.Y *= -1f;
				Main.combatText[i].position.Y = (float)location.Y + (float)location.Height * 0.75f + val.Y * 0.5f;
			}
			Main.combatText[i].lifeTime = 60;
			Main.combatText[i].crit = dramatic;
			Main.combatText[i].dot = dot;
			if (dramatic)
			{
				Main.combatText[i].text = text;
				Main.combatText[i].lifeTime *= 2;
				Main.combatText[i].velocity.Y *= 2f;
				Main.combatText[i].velocity.X = (float)Main.rand.Next(-25, 26) * 0.05f;
				Main.combatText[i].rotation = (float)(Main.combatText[i].lifeTime / 2) * 0.002f;
				if (Main.combatText[i].velocity.X < 0f)
				{
					Main.combatText[i].rotation *= -1f;
				}
			}
			if (dot)
			{
				Main.combatText[i].velocity.Y = -4f;
				Main.combatText[i].lifeTime = 40;
			}
			return i;
		}
		return 100;
	}

	public static void clearAll()
	{
		for (int i = 0; i < 100; i++)
		{
			Main.combatText[i].active = false;
		}
	}

	public void Update()
	{
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		if (!active)
		{
			return;
		}
		float targetScale = TargetScale;
		alpha += (float)alphaDir * 0.05f;
		if ((double)alpha <= 0.6)
		{
			alphaDir = 1;
		}
		if (alpha >= 1f)
		{
			alpha = 1f;
			alphaDir = -1;
		}
		if (dot)
		{
			velocity.Y += 0.15f;
		}
		else
		{
			velocity.Y *= 0.92f;
			if (crit)
			{
				velocity.Y *= 0.92f;
			}
		}
		velocity.X *= 0.93f;
		position += velocity;
		lifeTime--;
		if (lifeTime <= 0)
		{
			scale -= 0.1f * targetScale;
			if ((double)scale < 0.1)
			{
				active = false;
			}
			lifeTime = 0;
			if (crit)
			{
				alphaDir = -1;
				scale += 0.07f * targetScale;
			}
			return;
		}
		if (crit)
		{
			if (velocity.X < 0f)
			{
				rotation += 0.001f;
			}
			else
			{
				rotation -= 0.001f;
			}
		}
		if (dot)
		{
			scale += 0.5f * targetScale;
			if ((double)scale > 0.8 * (double)targetScale)
			{
				scale = 0.8f * targetScale;
			}
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

	public static void UpdateCombatText()
	{
		for (int i = 0; i < 100; i++)
		{
			if (Main.combatText[i].active)
			{
				Main.combatText[i].Update();
			}
		}
	}

	static CombatText()
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
	}
}
