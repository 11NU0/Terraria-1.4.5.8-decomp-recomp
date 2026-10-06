using Terraria.GameContent;
using Terraria.GameContent.NetModules;
using Terraria.Net;

namespace Terraria.Initializers;

public static class NetworkInitializer
{
	public static void Load()
	{
		NetManager.Instance.Register<NetLiquidModule>();
		NetManager.Instance.Register<NetTextModule>();
		NetManager.Instance.Register<NetPingModule>();
		NetManager.Instance.Register<NetAmbienceModule>();
		NetManager.Instance.Register<NetBestiaryModule>();
		NetManager.Instance.Register<NetCreativeUnlocksModule>();
		NetManager.Instance.Register<NetCreativePowersModule>();
		NetManager.Instance.Register<NetCreativeUnlocksPlayerReportModule>();
		NetManager.Instance.Register<NetTeleportPylonModule>();
		NetManager.Instance.Register<NetParticlesModule>();
		NetManager.Instance.Register<NetCreativePowerPermissionsModule>();
		NetManager.Instance.Register<BannerSystem.NetBannersModule>();
		NetManager.Instance.Register<CraftingRequests.NetCraftingRequestsModule>();
		NetManager.Instance.Register<LeashedEntity.NetModule>();
		NetManager.Instance.Register<UnbreakableWallScan.NetModule>();
	}
}
