using Terraria.Graphics.Effects;

namespace Terraria.GameContent.Events;

public class LanternNight
{
	public static bool ManualLanterns;

	public static bool GenuineLanterns;

	public static bool NextNightIsLanternNight;

	public static int LanternNightsOnCooldown;

	private static bool _wasLanternNight;

	public static bool LanternsUp
	{
		get
		{
			if (!GenuineLanterns)
			{
				return ManualLanterns;
			}
			return true;
		}
	}

	public static void CheckMorning()
	{
		bool flag = false;
		if (GenuineLanterns)
		{
			flag = true;
			GenuineLanterns = false;
		}
		if (ManualLanterns)
		{
			flag = true;
			ManualLanterns = false;
		}
	}

	public static void CheckNight()
	{
		NaturalAttempt();
	}

	public static bool LanternsCanPersist()
	{
		if (!Main.dayTime)
		{
			return LanternsCanStart();
		}
		return false;
	}

	public static bool LanternsCanStart()
	{
		if (!WorldGen.spawnMeteor && !Main.bloodMoon && !Main.pumpkinMoon && !Main.snowMoon && Main.invasionType == 0 && NPC.MoonLordCountdown == 0)
		{
			return !BossIsActive();
		}
		return false;
	}

	private static bool BossIsActive()
	{
		for (int i = 0; i < Main.maxNPCs; i++)
		{
			NPC nPC = Main.npc[i];
			if (nPC.active && (nPC.boss || (nPC.type >= 13 && nPC.type <= 15)))
			{
				return true;
			}
		}
		return false;
	}

	private static void NaturalAttempt()
	{
		if (Main.netMode != 1 && LanternsCanStart())
		{
			bool flag = false;
			if (LanternNightsOnCooldown > 0)
			{
				LanternNightsOnCooldown--;
			}
			if (LanternNightsOnCooldown == 0 && NPC.downedMoonlord && Main.rand.Next(14) == 0)
			{
				flag = true;
			}
			if (!flag && NextNightIsLanternNight)
			{
				NextNightIsLanternNight = false;
				flag = true;
			}
			if (flag)
			{
				GenuineLanterns = true;
				LanternNightsOnCooldown = Main.rand.Next(5, 11);
			}
		}
	}

	public static void ToggleManualLanterns()
	{
		bool lanternsUp = LanternsUp;
		if (Main.netMode != 1)
		{
			ManualLanterns = !ManualLanterns;
		}
		if (lanternsUp != LanternsUp && Main.netMode == 2)
		{
			NetMessage.SendData(7);
		}
	}

	public static void WorldClear()
	{
		ManualLanterns = false;
		GenuineLanterns = false;
		LanternNightsOnCooldown = 0;
		_wasLanternNight = false;
	}

	public static void UpdateTime()
	{
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		if (GenuineLanterns && !LanternsCanPersist())
		{
			GenuineLanterns = false;
		}
		if (_wasLanternNight != LanternsUp)
		{
			if (Main.netMode != 2)
			{
				if (LanternsUp)
				{
					SkyManager.Instance.Activate("Lantern", default);
				}
				else
				{
					SkyManager.Instance.Deactivate("Lantern");
				}
			}
			else
			{
				NetMessage.SendData(7);
			}
		}
		_wasLanternNight = LanternsUp;
	}
}
