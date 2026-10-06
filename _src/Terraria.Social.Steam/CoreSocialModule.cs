using System;
using System.Threading;
using Microsoft.Xna.Framework;
using ReLogic.OS;
using Steamworks;
using Terraria.Localization;
using Terraria.Utilities;

namespace Terraria.Social.Steam;

public class CoreSocialModule : ISocialModule
{
	private static CoreSocialModule _instance;

	public const int SteamAppId = 105600;

	private bool IsSteamValid;

	private object _steamTickLock = new object();

	private object _steamCallbackLock = new object();

	private Callback<GameOverlayActivated_t> _onOverlayActivated;

	public static event Action OnTick;

	public void Initialize()
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		_instance = this;
		if (!Main.dedServ && SteamAPI.RestartAppIfNecessary(new AppId_t(105600u)))
		{
			Environment.Exit(1);
			return;
		}
		if (!SteamAPI.Init())
		{
			MessageBox.Show(Language.GetTextValue("Error.LaunchFromSteam"), Language.GetTextValue("Error.Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
			Environment.Exit(1);
		}
		SteamInput.Init(false);
		IsSteamValid = true;
		Thread thread = new Thread(SteamCallbackLoop);
		thread.IsBackground = true;
		thread.Start();
		Thread thread2 = new Thread(SteamTickLoop);
		thread2.IsBackground = true;
		thread2.Start();
		Main.OnTickForThirdPartySoftwareOnly += Pulse;
		if (Platform.IsOSX && !Main.dedServ)
		{
			_onOverlayActivated = Callback<GameOverlayActivated_t>.Create((Callback<GameOverlayActivated_t>.DispatchDelegate)OnOverlayActivated);
		}
	}

	private void PulseSteamTick()
	{
		if (Monitor.TryEnter(_steamTickLock))
		{
			Monitor.Pulse(_steamTickLock);
			Monitor.Exit(_steamTickLock);
		}
	}

	private void PulseSteamCallback()
	{
		if (Monitor.TryEnter(_steamCallbackLock))
		{
			Monitor.Pulse(_steamCallbackLock);
			Monitor.Exit(_steamCallbackLock);
		}
	}

	public static void Pulse()
	{
		_instance.PulseSteamTick();
		_instance.PulseSteamCallback();
	}

	private void SteamTickLoop(object context)
	{
		Monitor.Enter(_steamTickLock);
		while (IsSteamValid)
		{
			if (OnTick != null)
			{
				OnTick();
			}
			Monitor.Wait(_steamTickLock);
		}
		Monitor.Exit(_steamTickLock);
	}

	private void SteamCallbackLoop(object context)
	{
		Monitor.Enter(_steamCallbackLock);
		while (IsSteamValid)
		{
			SteamAPI.RunCallbacks();
			Monitor.Wait(_steamCallbackLock);
		}
		Monitor.Exit(_steamCallbackLock);
		SteamAPI.Shutdown();
	}

	public void Shutdown()
	{
		IsSteamValid = false;
	}

	public void OnOverlayActivated(GameOverlayActivated_t result)
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		((Game)Main.instance).IsMouseVisible = result.m_bActive == 1;
	}
}
