using Microsoft.Xna.Framework;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;

namespace Terraria.GameContent;

public class DontStarveDarknessDamageDealer
{
	public const int DARKNESS_HIT_TIMER_MAX_BEFORE_HIT = 60;

	public static int darknessTimer = -1;

	public static int darknessHitTimer = 0;

	public static bool saidMessage = false;

	public static bool lastFrameWasTooBright = true;

	public static void Reset()
	{
		ResetTimer();
		saidMessage = false;
		lastFrameWasTooBright = true;
	}

	private static void ResetTimer()
	{
		darknessTimer = -1;
		darknessHitTimer = 0;
	}

	private static int GetDarknessDamagePerHit()
	{
		return 250;
	}

	private static int GetDarknessTimeBeforeStartingHits()
	{
		return 120;
	}

	private static int GetDarknessTimeForMessage()
	{
		return 60;
	}

	public static void Update(Player player)
	{
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		if (player.dead || !FocusHelper.AllowDontStarveDarknessDamage || player.shimmering || Main.disableDontStarveDarknessDamage)
		{
			ResetTimer();
			return;
		}
		UpdateDarknessState(player);
		int darknessTimeBeforeStartingHits = GetDarknessTimeBeforeStartingHits();
		if (darknessTimer >= darknessTimeBeforeStartingHits)
		{
			darknessTimer = darknessTimeBeforeStartingHits;
			darknessHitTimer++;
			if (darknessHitTimer > 60 && !player.immune)
			{
				int darknessDamagePerHit = GetDarknessDamagePerHit();
				SoundEngine.PlaySound(SoundID.Item1, player.Center);
				player.Hurt(PlayerDeathReason.ByOther(17), darknessDamagePerHit, 0);
				darknessHitTimer = 0;
			}
		}
	}

	private static void UpdateDarknessState(Player player)
	{
		if (IsPlayerSafe(player))
		{
			if (saidMessage)
			{
				if (!Main.getGoodWorld)
				{
					Main.NewText(Language.GetTextValue("Game.DarknessSafe"), 50, 200, 50);
				}
				saidMessage = false;
			}
			ResetTimer();
			return;
		}
		int darknessTimeForMessage = GetDarknessTimeForMessage();
		if (darknessTimer >= darknessTimeForMessage && !saidMessage)
		{
			if (!Main.getGoodWorld)
			{
				Main.NewText(Language.GetTextValue("Game.DarknessDanger"), 200, 50, 50);
			}
			saidMessage = true;
		}
		darknessTimer++;
	}

	private static bool IsPlayerSafe(Player player)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		Color color = Lighting.GetColor((int)player.Center.X / 16, (int)player.Center.Y / 16);
		Vector3 val = color.ToVector3();
		return val.Length() >= 0.1f;
	}
}
